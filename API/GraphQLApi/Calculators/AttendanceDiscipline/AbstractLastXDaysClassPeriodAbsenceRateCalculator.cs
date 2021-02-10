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
    public abstract class AbstractLastXDaysClassPeriodAbsenceRateCalculator : AbstractLastXDaysAttendanceRateCalculator
    {
        public AbstractLastXDaysClassPeriodAbsenceRateCalculator(CalculatorAppContext appContext) : base(appContext){}

        protected override List<StudentAttendanceEvent> GetMetricAttendanceEvents(List<StudentAttendanceEvent> studentAttendanceEvents, PeriodData periodData)
        {
            var attendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => periodData.CalendarDates.Any()
                                                            && studentAttendanceEvent.EventDate.IsBetween(periodData.CalendarDates.Min(), periodData.CalendarDates.Max()));

            return attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsStudentSectionAttendanceEvent
                         && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                        || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                        .ToList();

        }

        protected override bool IsClassPeriodAbsenceRateMetric()
        {
            return true;
        }

        protected override int GetPeriodNumerator(List<StudentAttendanceEvent> metricAttendanceEvents, int periodDenominator)
        {
            return metricAttendanceEvents.Count;
        }
    }
}
