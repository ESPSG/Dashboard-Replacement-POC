using AMT.Data.Entities;
using GraphQLApi.Calculators;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Repository
{
    public class StudentDataRepository : IStudentDataRepository
    {
        protected readonly ODSContext _context;
        protected IMemoryCache _memoryCache;

        public StudentDataRepository(ODSContext oDSContext,
                                    ApiMemoryCache memoryCache)
        {
            _context = oDSContext;
            _memoryCache = memoryCache.Cache;
        }

        #region School
        public IDictionary<string, List<StudentSchoolDim>> GetStudentSchoolAssociationsDictionary(string schoolKey)
        {
            string cacheKey = "StudentSchoolAssociations:" + schoolKey;

            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary;
            if (_memoryCache.TryGetValue(cacheKey, out studentSchoolAssociationsDictionary))
            {
                return studentSchoolAssociationsDictionary;
            }
            var studentSchoolAssociations = _context.StudentSchoolDims
                            .Where(s => s.SchoolKey.Equals(schoolKey))
                            .ToList();

            studentSchoolAssociationsDictionary
                = studentSchoolAssociations
                .GroupBy(x => x.StudentKey)
                .ToDictionary(x => x.Key, x => x.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(500)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, studentSchoolAssociationsDictionary, cacheEntryOptions);

            return studentSchoolAssociationsDictionary;

        }

        public List<SchoolCalendarDim> GetSchoolCalendarDays(string schoolKey)
        {
            string cacheKey = "SchoolCalendarDays:" + schoolKey;
            List<SchoolCalendarDim> schoolCalendarDays;
            if (_memoryCache.TryGetValue(cacheKey, out schoolCalendarDays))
            {
                return schoolCalendarDays;
            }


            int temp;
            int schoolKeyInt = int.TryParse(schoolKey, out temp) ? temp : 0;
            schoolCalendarDays = _context.SchoolCalendarDims.Where(s => s.SchoolKey == schoolKeyInt).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, schoolCalendarDays, cacheEntryOptions);

            return schoolCalendarDays;
        }

        public IDictionary<string, List<StudentSectionDim>> GetStudentSectionAssociationsDictionary(string schoolKey)
        {
            string cacheKey = "StudentSectionAssociations:" + schoolKey;

            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary;

            if (_memoryCache.TryGetValue(cacheKey, out studentSectionAssociationsDictionary))
            {
                return studentSectionAssociationsDictionary;
            }

            var studentSectionAssociations = _context.StudentSectionDims
                            .Where(s => s.SchoolKey.Equals(schoolKey))
                            .ToList();

            studentSectionAssociationsDictionary = studentSectionAssociations
                .GroupBy(x => x.StudentKey)
                .ToDictionary(x => x.Key, x => x.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(500)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, studentSectionAssociationsDictionary, cacheEntryOptions);

            return studentSectionAssociationsDictionary;
        }

        public List<GradingPeriodDim> GetSchoolGradingPeriods(string schoolKey)
        {
            string cacheKey = "SchoolGradingPeriods:" + schoolKey;
            List<GradingPeriodDim> schoolGradingPeriods;
            if (_memoryCache.TryGetValue(cacheKey, out schoolGradingPeriods))
            {
                return schoolGradingPeriods;
            }

            schoolGradingPeriods = _context.GradingPeriodDims.Where(s => s.SchoolKey.Equals(schoolKey)).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, schoolGradingPeriods, cacheEntryOptions);

            return schoolGradingPeriods;
        }

        #endregion
    }
}
