using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Models;
using GraphQLApi.Common;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractLastXDaysTardyRateCalculator : AbstractLastXDaysAttendanceRateCalculator
    {
        public AbstractLastXDaysTardyRateCalculator(CalculatorAppContext appContext) : base(appContext){}
        protected override List<StudentAttendanceEvent> GetMetricAttendanceEvents(List<StudentAttendanceEvent> studentAttendanceEvents, PeriodData periodData)
        {
            var attendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => periodData.CalendarDates.Any()
                                                            && studentAttendanceEvent.EventDate.IsBetween(periodData.CalendarDates.Min(), periodData.CalendarDates.Max()));

            return attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                    && studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.Tardy))
                                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                            .ToList();
        }

        protected override int GetPeriodNumerator(List<StudentAttendanceEvent> metricAttendanceEvents, int periodDenominator)
        {
            return metricAttendanceEvents.Count;
        }

        protected override bool IsClassPeriodAbsenceRateMetric()
        {
            return false;
        }
    }
}
