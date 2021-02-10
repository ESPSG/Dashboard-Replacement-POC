using GraphQL.Authorization;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GraphQLApi.Infrastructure.Security
{
    public static class GraphQLExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings> configure)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.AddTransient<IUserContextBuilder>(s => new UserContextBuilder<GraphQLUserContext>(context =>
            {
                var userContext = new GraphQLUserContext
                {
                    User = context.User
                };

                return Task.FromResult(userContext);
            }));

            services.AddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();
                configure(authSettings);
                return authSettings;
            });
            
        }
    }
}
