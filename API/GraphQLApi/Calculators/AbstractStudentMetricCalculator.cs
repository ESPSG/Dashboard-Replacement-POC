using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Infrastructure;
using GraphQLApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace GraphQLApi.Calculators
{
    public abstract class AbstractStudentMetricCalculator : IStudentMetricCalculator
    {
        protected readonly ODSContext _context;
        protected Common.Metric _metricInfo;
        protected IMemoryCache _memoryCache;
        protected IOptions<AppConfiguration> _appConfiguration;
        protected MemoryCacheEntryOptions _cacheEntryOptions;

        protected IStudentDataRepository StudentDataRepository { get; }
        protected IGradingScaleRepository GradingScaleRepository { get; }
        public AbstractStudentMetricCalculator(CalculatorAppContext appContext)
        {
            if (appContext != null)
            {
                _context = appContext.ODSContext;
                _memoryCache = appContext.MemoryCache;
                _appConfiguration = appContext.AppConfiguration;
                StudentDataRepository = appContext.StudentDataRepository;
                GradingScaleRepository = appContext.GradingScaleRepository;
            }

            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(100)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
        }
        public abstract IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey);
        public int GetMetricId()
        {
            if (_metricInfo == null)
            {
                throw new AMT.Data.Common.FatalException(" Metric info needs to be initialized for each calculator ");
            }

            return _metricInfo.Id;
        }
        public List<StudentMetric> GetStudentMetrics(string studentKey, string schoolKey)
        {
            var schoolLevelMetrics = GetAllStudentMetricsForSchool(schoolKey);
            if (schoolLevelMetrics.ContainsKey(studentKey))
            {
                return schoolLevelMetrics[studentKey];
            }
            else
            {
                return new List<StudentMetric>();
            }
        }

        protected void GetTrendByAttendance(int firstPeriodTotal, int secondPeriodTotal, int firstPeriodAttendance, int secondPeriodAttendance, RateDirection rateDirection, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal != secondPeriodTotal || firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            GetTrendByStudent(firstPeriodTotal, secondPeriodTotal, firstPeriodAttendance, secondPeriodAttendance, rateDirection, out trend, out flag);
        }

        private void GetTrendByStudent(int firstPeriodTotal, int secondPeriodTotal, decimal firstPeriodAttendance, decimal secondPeriodAttendance, RateDirection rateDirection, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            var firstPeriodRate = firstPeriodAttendance / firstPeriodTotal;
            var secondPeriodRate = secondPeriodAttendance / secondPeriodTotal;

            GetTrend(out trend, out flag, firstPeriodRate, secondPeriodRate, rateDirection);
        }

        protected void GetTrend(out int? trend, out bool flag, decimal? firstPeriodRate, decimal? secondPeriodRate, RateDirection rateDirection)
        {
            trend = null;
            flag = false;
            if (!firstPeriodRate.HasValue || !secondPeriodRate.HasValue)
            {
                return;
            }
            if (firstPeriodRate - secondPeriodRate > 0.05m)
            {
                trend = 1;
            }
            else if (secondPeriodRate - firstPeriodRate > 0.05m)
            {
                trend = -1;
                if (rateDirection == RateDirection.OneToZero && secondPeriodRate - firstPeriodRate > 0.1m)
                {
                    flag = true;
                }
            }
            else
            {
                trend = 0;
            }
        }
    }
}
