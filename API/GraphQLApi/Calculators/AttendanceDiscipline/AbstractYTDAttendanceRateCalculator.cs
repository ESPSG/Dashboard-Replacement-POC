using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractYTDAttendanceRateCalculator : AbstractStudentAttendanceCalculator
    {
        public AbstractYTDAttendanceRateCalculator(CalculatorAppContext appContext) : base(appContext) { }

        public override IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            string cacheKey = "schoolKey:" + schoolKey + " MetricId:" + GetMetricId();

            IDictionary<string, List<StudentMetric>> returnMap;
            if (_memoryCache.TryGetValue(cacheKey, out returnMap))
            {
                return returnMap;
            }

            returnMap = new Dictionary<string, List<StudentMetric>>();
            IDictionary<string, List<StudentAttendanceEvent>> studentAttendanceEventDictionary = getStudentAttendanceEvents(schoolKey);
            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);
            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary = getStudentSectionAssociationsDictionary(schoolKey);

            foreach (string studentKey in studentAttendanceEventDictionary.Keys)
            {
                List<StudentAttendanceEvent> studentAttendanceEvents = studentAttendanceEventDictionary[studentKey];
                List<StudentSchoolDim> studentSchoolAssociations = studentSchoolAssociationsDictionary.ContainsKey(studentKey) ? studentSchoolAssociationsDictionary[studentKey] : new List<StudentSchoolDim>();
                List<StudentSectionDim> studentSectionAssociations = studentSectionAssociationsDictionary.ContainsKey(studentKey) ? studentSectionAssociationsDictionary[studentKey] : new List<StudentSectionDim>();

                int metricCount = GetMetricCount(studentAttendanceEvents);
                HashSet<DateTime> schoolCalendarDaysAsSet = new HashSet<DateTime>();
                StudentDataRepository.GetSchoolCalendarDays(schoolKey).ForEach(s => schoolCalendarDaysAsSet.Add(s.Date));

                var daysStudentWasEnrolled = GetDaysStudentWasEnrolled(true, schoolCalendarDaysAsSet, studentSchoolAssociations, studentSectionAssociations).Count;
                decimal ytdClassPeriodAttendanceRate = GetYtdAttendanceRate(metricCount, daysStudentWasEnrolled);

                var metricStateType = _metricInfo.GetMetricStateType(ytdClassPeriodAttendanceRate, null);
                var metricState = MetricUtility.GetMetricState(metricStateType.Value);

                if (!returnMap.ContainsKey(studentKey))
                {
                    returnMap[studentKey] = new List<StudentMetric>();
                }

                returnMap[studentKey].Add(new StudentMetric
                {
                    Id = _metricInfo.Id,
                    Name = _metricInfo.DisplayName,
                    Value = ytdClassPeriodAttendanceRate.Display().DisplayValue(DISPLAY_FORMAT_ATTENDANCE_RATE),
                    State = metricState,
                    TrendDirection = null,
                });


            }

            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected abstract decimal GetYtdAttendanceRate(int metricCount, int daysStudentWasEnrolled);
        protected abstract int GetMetricCount(List<StudentAttendanceEvent> studentAttendanceEvents);
    }
}
