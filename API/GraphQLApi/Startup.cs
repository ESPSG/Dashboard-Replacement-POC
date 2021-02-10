using AMT.Data.Entities;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.GraphQL.Schema;
using GraphQLApi.Infrastructure;
using GraphQLApi.Infrastructure.Security;
using GraphQLApi.Models;
using GraphQLApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoogleOptions = GraphQLApi.Infrastructure.Security.Models.GoogleOptions;

namespace GraphQLApi
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                           .WithMethods("GET", "POST")
                           .WithOrigins(Configuration["DashboardUrl"]);
                });
            });

            services.AddSingleton<ApiMemoryCache>();

            services.AddDbContext<ODSContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ODSConnection")));

            services.AddScoped<Calculators.CalculatorAppContext>();
            services.AddScoped<AppUserSecurityContext>();
            services.AddSingleton<IMetricCalculatorRepository, MetricCalculatorRepository>();

            services.AddScoped<IMetricRepository, MetricRepository>();

            services.AddScoped<IStudentInformationRepository, StudentInformationRepository>();
            services.AddScoped<IStudentMetricCalculator, StudentAttendanceRepository>();
            services.AddScoped<IStudentDataRepository, StudentDataRepository>();
            services.AddScoped<IGradingScaleRepository, GradingScaleRepository>();

            var azureAuthSettings = Configuration.GetSection("AzureAd").Get<AzureAdOptions>();
            var googleAuthSettings = Configuration.GetSection("Google").Get<GoogleOptions>();
            var authMethod = Configuration.GetSection("AuthenticationMethod").Get<string>();
            string audience = "";
            switch (authMethod)
            {
                case "Google":
                case "google":
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.SecurityTokenValidators.Clear();
                            options.SecurityTokenValidators.Add(new GoogleTokenValidator(googleAuthSettings));
                        });
                    audience = googleAuthSettings.ClientId;
                    break;
                case "Azure":
                case "azure":
                default:
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.Audience = azureAuthSettings.ClientId;
                            options.Authority = azureAuthSettings.Authority;
                        });
                    audience = azureAuthSettings.ClientId;
                    break;
            }
            

            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<AppSchema>();
            services.AddGraphQLAuth(_ =>
            {
                _.AddPolicy("Authorized", p => p.RequireClaim("aud", audience));

            });
            services.AddGraphQL(o => { o.ExposeExceptions = false; })
                .AddGraphTypes(ServiceLifetime.Scoped);


            services.AddControllers()
                .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<AppConfiguration>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("DefaultPolicy");
            app.UseAuthentication();
            app.UseRouting();

            app.UseGraphQL<AppSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
