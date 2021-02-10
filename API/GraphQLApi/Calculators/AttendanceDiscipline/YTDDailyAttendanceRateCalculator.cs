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
    public class YTDDailyAttendanceRateCalculator : AbstractYTDAttendanceRateCalculator
    {
        public YTDDailyAttendanceRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDailyAttendanceRateYearToDate;
        }
       
        protected override int GetMetricCount(List<StudentAttendanceEvent> studentAttendanceEvents)
        {
            return studentAttendanceEvents.Where(studentAttendanceEvent => (studentAttendanceEvent.IsForDailyAttendance()
                                                                     && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                         || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence))))
                                                                 .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                                 .ToList()
                                                                 .Count();
        }

        protected override decimal GetYtdAttendanceRate(int metricCount, int daysStudentWasEnrolled)
        {
            var ytdNumerator = daysStudentWasEnrolled - metricCount;
            var ytdDailyAttendanceRate = ((decimal)ytdNumerator / daysStudentWasEnrolled).RoundTo(3);
            return ytdDailyAttendanceRate;
        }
    }
}
