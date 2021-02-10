using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Common
{
    public class ApiMemoryCache
    {
        public static int SIZE_LIMIT_DEFAULT = 30000;
        public IMemoryCache Cache { get; set; }

        public ApiMemoryCache(IConfiguration configuration)
        {
            int temp;
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = int.TryParse(configuration["ApiMemoryCacheSizeLimit"], out temp) ? temp : SIZE_LIMIT_DEFAULT
            }); ;
        }
    }
}
