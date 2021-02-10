using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class YTDTardyRateCalculator : AbstractYTDAttendanceRateCalculator
    {
        public YTDTardyRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentTardyRateYearToDate;
        }
        protected override int GetMetricCount(List<StudentAttendanceEvent> studentAttendanceEvents)
        {
            return studentAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                         && studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.Tardy))
                                                    .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                    .ToList()
                                                    .Count();
        }

        protected override decimal GetYtdAttendanceRate(int metricCount, int daysStudentWasEnrolled)
        {
            var ytdNumerator = metricCount;
            var ytdTardyRate = ((decimal)ytdNumerator / daysStudentWasEnrolled).RoundTo(3);
            return ytdTardyRate;
        }
    }
}
