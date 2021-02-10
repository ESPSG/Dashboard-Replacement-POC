using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators
{
    public class CalculatorAppContext
    {
        public IStudentDataRepository StudentDataRepository { get; }

        public IGradingScaleRepository GradingScaleRepository { get; }

        public IMemoryCache MemoryCache { get; }

        public ODSContext ODSContext { get; }

        public IOptions<AppConfiguration> AppConfiguration { get; }

        public IHttpContextAccessor UserContext { get; }

        public CalculatorAppContext(ODSContext oDSContext,
                                    ApiMemoryCache memoryCache,
                                    IHttpContextAccessor contextAccessor,
                                    IOptions<AppConfiguration> appConfiguration,
                                    IStudentDataRepository dataRepository,
                                    IGradingScaleRepository gradingScaleRepository)
        {
            MemoryCache = memoryCache.Cache;
            ODSContext = oDSContext;
            UserContext = contextAccessor;
            AppConfiguration = appConfiguration;
            StudentDataRepository = dataRepository;
            GradingScaleRepository = gradingScaleRepository;
        }
    }
}
