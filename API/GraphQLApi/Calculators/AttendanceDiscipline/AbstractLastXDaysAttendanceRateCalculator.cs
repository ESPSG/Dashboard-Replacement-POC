using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Calculators.AttendanceDiscipline;
using GraphQLApi.Common;
using GraphQLApi.Enumerations;
using GraphQLApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractLastXDaysAttendanceRateCalculator : AbstractStudentAttendanceCalculator
    {
        public AbstractLastXDaysAttendanceRateCalculator(CalculatorAppContext calculatorAppContext) : base(calculatorAppContext){}
        public override IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            string cacheKey = "schoolKey:" + schoolKey + " MetricId:" + GetMetricId();

            IDictionary<string, List<StudentMetric>> returnMap;
            if (_memoryCache.TryGetValue(cacheKey, out returnMap))
            {
                return returnMap;
            }

            int periodLength = GetPeriodLength();
            returnMap = new Dictionary<string, List<StudentMetric>>();
            IDictionary<Period, PeriodData> periodDataDictionary = GetPeriodData(schoolKey, periodLength);
            PeriodData firstPeriodData = periodDataDictionary[Period.First];
            PeriodData secondPeriodData = periodDataDictionary[Period.Second];
            IDictionary<string, List<StudentAttendanceEvent>> studentAttendanceEventDictionary = getStudentAttendanceEvents(schoolKey);
            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);
            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary = getStudentSectionAssociationsDictionary(schoolKey);


            foreach (string studentKey in studentAttendanceEventDictionary.Keys)
            {
                List<StudentAttendanceEvent> studentAttendanceEvents = studentAttendanceEventDictionary[studentKey];
                List<StudentSchoolDim> studentSchoolAssociations = studentSchoolAssociationsDictionary.ContainsKey(studentKey) ? studentSchoolAssociationsDictionary[studentKey] : new List<StudentSchoolDim>();
                List<StudentSectionDim> studentSectionAssociations = studentSectionAssociationsDictionary.ContainsKey(studentKey) ? studentSectionAssociationsDictionary[studentKey] : new List<StudentSectionDim>();

                //var firstPeriodAttendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => firstPeriodData.CalendarDates.Any()
                //                                                                && studentAttendanceEvent.EventDate.IsBetween(firstPeriodData.CalendarDates.Min(), firstPeriodData.CalendarDates.Max()));


                List<StudentAttendanceEvent> firstPeriodMetricAttendanceEvents = GetMetricAttendanceEvents(studentAttendanceEvents, firstPeriodData);
                //var firstPeriodDailyAttendanceRateAttendanceEvents = firstPeriodAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                //                                                && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                //                                                    || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                //                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                //                                            .ToList();



                List<StudentAttendanceEvent> secondPeriodMetricAttendanceEvents = GetMetricAttendanceEvents(studentAttendanceEvents, secondPeriodData);

                var firstPeriodDenominator = GetDaysStudentWasEnrolled(IsClassPeriodAbsenceRateMetric(), firstPeriodData.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;

                if (firstPeriodDenominator >= periodLength)
                {
                    var secondPeriodDenominator = GetDaysStudentWasEnrolled(IsClassPeriodAbsenceRateMetric(), secondPeriodData.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;
                    var firstPeriodNumerator = GetPeriodNumerator(firstPeriodMetricAttendanceEvents,firstPeriodDenominator);
                    var firstPeriodRatio = ((decimal)firstPeriodNumerator / firstPeriodDenominator).RoundTo(3);
                    var secondPeriodNumerator = GetPeriodNumerator(secondPeriodMetricAttendanceEvents, secondPeriodDenominator);
                    int? trend;
                    bool flag;
                    GetTrendByAttendance(firstPeriodDenominator, secondPeriodDenominator, firstPeriodNumerator, secondPeriodNumerator, _metricInfo.RateDirection, out trend, out flag);
                    var metricStateType = _metricInfo.GetMetricStateType(firstPeriodRatio, null);
                    var metricState = MetricUtility.GetMetricState(metricStateType.Value);

                    if (!returnMap.ContainsKey(studentKey))
                    {
                        returnMap[studentKey] = new List<StudentMetric>();
                    }
                    returnMap[studentKey].Add(new StudentMetric
                    {
                        Id = _metricInfo.Id,
                        Name = string.Format(_metricInfo.DisplayName, periodLength),
                        Value = firstPeriodRatio.Display().DisplayValue(DISPLAY_FORMAT_ATTENDANCE_RATE),
                        TrendDirection = trend ?? null,
                        State = metricState
                    });
                }
            }

            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected abstract List<StudentAttendanceEvent> GetMetricAttendanceEvents(List<StudentAttendanceEvent> studentAttendanceEvents, PeriodData periodData);
        protected abstract int GetPeriodLength();

        protected abstract bool IsClassPeriodAbsenceRateMetric();

        protected abstract int GetPeriodNumerator(List<StudentAttendanceEvent> metricAttendanceEvents, int periodDenominator);

    }
}
