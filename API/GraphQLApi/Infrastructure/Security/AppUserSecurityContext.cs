using AMT.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using GraphQLApi.Calculators;
using GraphQLApi.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GraphQLApi.Common;

namespace GraphQLApi.Infrastructure.Security
{
    public class AppUserSecurityContext
    {
        public bool IsDistrictAdministrator
        {
            get
            {
                bool isDistrictAdministrator = false;
                if (UserAuthorizationDims.Any() && UserAuthorizationDims.Count == 1)
                {
                    var districtUser = UserAuthorizationDims.First();
                    isDistrictAdministrator = districtUser.UserScope.Equals("AuthorizationScope.District");
                    DistrictId = Convert.ToInt32(districtUser.SchoolPermission);
                }
                return isDistrictAdministrator;
            }
        }
        public bool IsSchoolAdministrator
        {
            get
            {
                bool isSchoolAdministrator = false;
                if (UserAuthorizationDims.Any() && UserAuthorizationDims.Count == 1)
                {
                    var schoolUser = UserAuthorizationDims.First();
                    isSchoolAdministrator = schoolUser.UserScope.Equals("AuthorizationScope.School");
                    SchoolId = Convert.ToInt32(schoolUser.SchoolPermission);
                }
                return isSchoolAdministrator;
            }
        }

        public bool IsTeacher
        {
            get
            {
                return UserAuthorizationDims.Any()
                        && UserAuthorizationDims.Exists(uad => uad.UserScope.Equals("AuthorizationScope.Section"));
            }

        }

        public int? DistrictId;

        public int? SchoolId;
        public List<UserAuthorizationDim> UserAuthorizationDims { get; set; }

        protected readonly ODSContext ODSContext;
        protected IMemoryCache MemoryCache { get; }

        public AppUserSecurityContext(CalculatorAppContext calculatorAppContext, ApiMemoryCache memoryCache)
        {
            ODSContext = calculatorAppContext.ODSContext;
            MemoryCache = memoryCache.Cache;
            UserAuthorizationDims = new List<UserAuthorizationDim>();

            string userEmailAddress = string.Empty;
            var userContext = calculatorAppContext.UserContext;
            if (userContext != null && userContext.HttpContext != null)
            {
                var identities = userContext.HttpContext.User.Identities.ToList();//TODO : handle null exceptions
                if (identities.Any())
                {
                    userEmailAddress = identities.FirstOrDefault().Name;
                }
            }

            if (!string.IsNullOrEmpty(userEmailAddress))
            {
                string cacheKey = "UserAuthorizationDims:" + userEmailAddress;
                List<UserAuthorizationDim> _userAuthorizationDims;
                if (MemoryCache.TryGetValue(cacheKey, out _userAuthorizationDims))
                {
                    UserAuthorizationDims = _userAuthorizationDims;
                }
                else
                {
                    UserAuthorizationDims = (from uad in ODSContext.UserAuthorizationDims.AsNoTracking()
                                             join ud in ODSContext.UserDims.AsNoTracking()
                                                on uad.UserKey equals ud.UserKey
                                             where ud.UserEmail.Equals(userEmailAddress)
                                             select new UserAuthorizationDim
                                             {
                                                 UserKey = uad.UserKey,
                                                 UserScope = uad.UserScope,
                                                 SectionPermission = uad.SectionPermission,
                                                 StudentPermission = uad.StudentPermission,
                                                 SchoolPermission = uad.SchoolPermission,
                                                 DistrictId = uad.DistrictId
                                             })
                                        .ToList();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(20)//Size amount
                        .SetPriority(CacheItemPriority.High)
                        .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
                    MemoryCache.Set(cacheKey, UserAuthorizationDims, cacheEntryOptions);

                }

            }
        }
    }
}
