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
    public class YTDClassPeriodAbsenceRateCaclulator : AbstractYTDAttendanceRateCalculator
    {
        public YTDClassPeriodAbsenceRateCaclulator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentClassPeriodAbsenceRateYearToDate;
        }

        protected override int GetMetricCount(List<StudentAttendanceEvent> studentAttendanceEvents)
        {
            return studentAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsStudentSectionAttendanceEvent
                                                                        && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                            || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                    .ToList()
                                                                    .Count();
        }

        protected override decimal GetYtdAttendanceRate(int metricCount, int daysStudentWasEnrolled)
        {
            var ytdNumerator = metricCount;
            var ytdClassPeriodAttendanceRate = ((decimal)ytdNumerator / daysStudentWasEnrolled).RoundTo(3);
            return ytdClassPeriodAttendanceRate;

        }
    }
}
