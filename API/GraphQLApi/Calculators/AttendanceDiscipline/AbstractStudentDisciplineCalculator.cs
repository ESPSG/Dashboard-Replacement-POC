using AMT.Data.Common;
using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Enumerations;
using GraphQLApi.Infrastructure;
using GraphQLApi.Models;
using GraphQLApi.Models.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractStudentDisciplineCalculator : AbstractStudentMetricCalculator
    {
        public AbstractStudentDisciplineCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected string BehaviorDescription => "State Offense";

        public override IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            string cacheKey = "schoolKey:" + schoolKey + " MetricId:" + GetMetricId();

            IDictionary<string, List<StudentMetric>> returnMap;
            if (_memoryCache.TryGetValue(cacheKey, out returnMap))
            {
                return returnMap;
            }
            returnMap = new Dictionary<string, List<StudentMetric>>();
            var studentDisciplines = GetStudentDisciplines(schoolKey);
            studentDisciplines.ToList().ForEach(studentDiscipline =>
            {
                var studentKey = studentDiscipline.Key;
                var disciplineIncidentsForStudentByPeriod = studentDiscipline.Value.DisciplineIncidentsByPeriod;
                var currentPeriodIncidents = disciplineIncidentsForStudentByPeriod.GetValueOrDefault(Period.First);
                var previousPeriodIncidents = disciplineIncidentsForStudentByPeriod.GetValueOrDefault(Period.Second);

                var metricStateType = _metricInfo.GetMetricState(currentPeriodIncidents, null);
                var metricState = MetricUtility.GetMetricState(metricStateType.Value);
                int? trend;
                bool flag;
                _metricInfo.GetTrendDirection(currentPeriodIncidents, previousPeriodIncidents, out trend, out flag);

                if (!returnMap.ContainsKey(studentKey))
                {
                    returnMap[studentKey] = new List<StudentMetric>();
                }
                returnMap[studentKey].Add(new StudentMetric
                {
                    Id = _metricInfo.Id,
                    Name = string.Format(_metricInfo.DisplayName, ReportingPeriodLength),
                    Value = currentPeriodIncidents.Display(),
                    ValueTypeName = "System.Int32",
                    State = metricState,
                    TrendDirection = trend
                });
            });

            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected List<StudentDisciplineDim> GetStudentDisciplineDbRecords(string SchoolKey)
        {
            List<StudentDisciplineDim> returnList;
            string cacheKey = "StudentDisciplineDbRecords:" + SchoolKey;
            if (_memoryCache.TryGetValue(cacheKey, out returnList))
            {
                return returnList;
            }

            returnList = (from ssaef in _context.StudentDisciplineDims.AsNoTracking()
                          where ssaef.SchoolKey.Equals(SchoolKey)
                          select ssaef)
                        .ToList()
                        .OrderBy(a => a.StudentKey)
                        .ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, returnList, cacheEntryOptions);

            return returnList;
        }
        protected IDictionary<string, StudentDiscipline> GetStudentDisciplines(string schoolKey)
        {
            IDictionary<string, StudentDiscipline> studentDisciplineDictionary;

            string cacheKey = "StudentDisciplineEvents" + schoolKey + ":IsStateOffense:" + IsStateOffense.ToString() + ":PeriodLength" + (ReportingPeriodLength.HasValue ? ReportingPeriodLength.Value : 99999);
            if (_memoryCache.TryGetValue(cacheKey, out studentDisciplineDictionary))
            {
                return studentDisciplineDictionary;
            }
            var currentSchoolYearDim = _context.CurrentSchoolYearDims.FirstOrDefault();

            studentDisciplineDictionary = new Dictionary<string, StudentDiscipline>();
            if (currentSchoolYearDim == null)
                return studentDisciplineDictionary;

            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);

            List<string> studentUniqueIds = studentSchoolAssociationsDictionary.Select(d => d.Key).ToList();

            var studentSchoolDisciplineDbRecords = GetStudentDisciplineDbRecords(schoolKey);

            studentSchoolDisciplineDbRecords = FilterDisciplineRecordsByBehaviorDescription(studentSchoolDisciplineDbRecords);

            var studentSchoolDisciplineDbRecordsDictionary = studentSchoolDisciplineDbRecords
                                                                        .GroupBy(x => x.StudentKey)
                                                                        .ToDictionary(x => x.Key, x => x.ToList().OrderBy(a => a.IncidentDate));


            var schoolDaysPeriod = GetSchoolDaysPeriod(schoolKey, ReportingPeriodLength);

            foreach (var studentUniqueId in studentUniqueIds)
            {
                string _previousIncidentIdentifier = string.Empty;
                var studentSchoolAssociations = studentSchoolAssociationsDictionary.ContainsKey(studentUniqueId) ? studentSchoolAssociationsDictionary[studentUniqueId] : new List<StudentSchoolDim>();
                var studentDiscipline = new StudentDiscipline { PeriodLength = ReportingPeriodLength };
                if (studentSchoolDisciplineDbRecordsDictionary != null && studentSchoolDisciplineDbRecordsDictionary.ContainsKey(studentUniqueId))
                {
                    foreach (var studentSchoolDisciplineDbRecord in studentSchoolDisciplineDbRecordsDictionary[studentUniqueId])
                    {
                        if (studentSchoolAssociations.ContainsDate(studentSchoolDisciplineDbRecord.IncidentDate)
                             && !string.Equals(studentSchoolDisciplineDbRecord.IncidentIdentifier, _previousIncidentIdentifier, StringComparison.Ordinal))
                        {
                            Period period;
                            if (schoolDaysPeriod.TryGetValue(studentSchoolDisciplineDbRecord.IncidentDate, out period))
                            {
                                studentDiscipline.DisciplineIncidentsByPeriod.Increment(period);
                                _previousIncidentIdentifier = studentSchoolDisciplineDbRecord.IncidentIdentifier;
                            }
                        }
                    }
                }
                else
                {
                    studentDiscipline.DisciplineIncidentsByPeriod.Add(Period.First, 0);
                    studentDiscipline.DisciplineIncidentsByPeriod.Add(Period.Second, 0);
                }

                studentDisciplineDictionary[studentUniqueId] = studentDiscipline;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentDisciplineDictionary, cacheEntryOptions);

            return studentDisciplineDictionary;

        }

        protected abstract List<StudentDisciplineDim> FilterDisciplineRecordsByBehaviorDescription(List<StudentDisciplineDim> studentSchoolDisciplineDbRecords);

        protected abstract bool IsStateOffense { get; }

        protected abstract int? ReportingPeriodLength { get; }

        public IDictionary<DateTime, Period> GetSchoolDaysPeriod(string schoolkey, int? periodLength)
        {
            string cacheKey = "SchoolDaysPeriod:" + schoolkey + ":PeriodLength" + (ReportingPeriodLength.HasValue ? ReportingPeriodLength.Value : 99999);
            IDictionary<DateTime, Period> schoolDaysPeriod;
            if (_memoryCache.TryGetValue(cacheKey, out schoolDaysPeriod))
            {
                return schoolDaysPeriod;
            }

            schoolDaysPeriod = new Dictionary<DateTime, Period>();
            var schoolCalendarDays = StudentDataRepository.GetSchoolCalendarDays(schoolkey);
            IReadOnlyCollection<Period> Periods = Enum.GetValues(typeof(Period)).Cast<Period>().ToArray();
            schoolCalendarDays.ForEach(schoolCalendarDay =>
            {
                var periodIndex = (periodLength == null) ? (int)Period.First : schoolCalendarDay.GetPeriodIndex(periodLength.Value);
                if (periodIndex > Periods.Count)
                {
                    return;
                }

                var period = (Period)periodIndex;
                schoolDaysPeriod[schoolCalendarDay.Date] = period;
            });

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, schoolDaysPeriod, cacheEntryOptions);

            return schoolDaysPeriod;
        }
    }
}
