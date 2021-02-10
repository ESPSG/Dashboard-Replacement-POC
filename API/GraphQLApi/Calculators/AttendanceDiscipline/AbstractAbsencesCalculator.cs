using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractAbsencesCalculator : AbstractStudentAttendanceCalculator
    {
        public AbstractAbsencesCalculator(CalculatorAppContext appContext) : base(appContext){ }

        public override IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            string cacheKey = "schoolKey:" + schoolKey + " MetricId:" + GetMetricId();

            IDictionary<string, List<StudentMetric>> returnMap;
            if(_memoryCache.TryGetValue(cacheKey,out returnMap))
            {
                return returnMap;
            }

            returnMap = new Dictionary<string, List<StudentMetric>>();
            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey.ToString())).FirstOrDefault();
            if (schoolMinMaxDateDim == null)
            {
                //"No entry found in analytics.SchoolMinMaxDateDim
                return returnMap;
            }

            var ytdGradingPeriod = new GradingPeriodDim
            {
                BeginDate = schoolMinMaxDateDim.MinDate.Value,
                EndDate = schoolMinMaxDateDim.MaxDate.Value,
                GradingPeriodDescription = string.Empty,
                IsYearToDate = true
            };

            int temp;
            int schoolKeyInt = int.TryParse(schoolKey, out temp) ? temp : 0;
            var schoolCalendarDays = _context.SchoolCalendarDims.Where(s => s.SchoolKey == schoolKeyInt).ToList();
            HashSet<DateTime> _schoolCalendarDays = new HashSet<DateTime>();
            schoolCalendarDays.ForEach(s => _schoolCalendarDays.Add(s.Date));

            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);
            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary = getStudentSectionAssociationsDictionary(schoolKey);

            IDictionary<string, List<StudentAttendanceEvent>> studentAttendanceEventDictionary = getStudentAttendanceEvents(schoolKey);

            foreach (var studentId in studentAttendanceEventDictionary.Keys)
            {
                DateTime? _lastAttendanceDateForStudent = null;
                HashSet<DateTime> _excusedAbsences = new HashSet<DateTime>();
                HashSet<DateTime> _unexcusedAbsences = new HashSet<DateTime>();
                int ytdTotalDaysAbsences = 0;
                int ytdUnexcusedAbsences = 0;

                foreach (var studentAttendanceEvent in studentAttendanceEventDictionary[studentId])
                {
                    if (_lastAttendanceDateForStudent == studentAttendanceEvent.EventDate)
                        continue;

                    if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                    {
                        _excusedAbsences.Add(studentAttendanceEvent.EventDate);
                        _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                    }
                    else if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                    {
                        _unexcusedAbsences.Add(studentAttendanceEvent.EventDate);
                        _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                    }
                }

                var daysStudentWasEnrolled = _schoolCalendarDays
                            .DaysInRange(studentSchoolAssociationsDictionary.ContainsKey(studentId) ? studentSchoolAssociationsDictionary[studentId] : new List<StudentSchoolDim>())
                            .DaysInRange(studentSectionAssociationsDictionary.ContainsKey(studentId) ? studentSectionAssociationsDictionary[studentId] : new List<StudentSectionDim>())
                            .ToArray();
                if (!daysStudentWasEnrolled.Any())
                {
                    // Do no calculations for students that were not enrolled for any instructional days
                    continue;
                }

                _unexcusedAbsences.RemoveWhere(x => _excusedAbsences.Contains(x));
                var excusedAbsences = _excusedAbsences.CountDaysInRange(ytdGradingPeriod);
                var unExcusedAbsences = _unexcusedAbsences.CountDaysInRange(ytdGradingPeriod);
                ytdTotalDaysAbsences = excusedAbsences + unExcusedAbsences;
                ytdUnexcusedAbsences = unExcusedAbsences;

                if (!returnMap.ContainsKey(studentId))
                {
                    returnMap[studentId] = new List<StudentMetric>();
                }
                int metricValue = GetAbsences(excusedAbsences, unExcusedAbsences);
                var metricStateType = _metricInfo.GetMetricStateType(metricValue, null);
                returnMap[studentId].Add(new Models.StudentMetric
                {
                    StudentUsi = int.TryParse(studentId, out temp) ? temp : 0,
                    Name = _metricInfo.DisplayName,
                    Id = _metricInfo.Id,
                    Value = metricValue.ToString(),
                    State = MetricUtility.GetMetricState(metricStateType.Value)
                }); ;
            }


            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;

        }

        protected abstract int GetAbsences(int excusedAbsences, int unexecusedAbsences);


    }
}
