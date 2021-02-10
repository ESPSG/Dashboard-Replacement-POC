using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Entities;
using AMT.Data.Models;
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
    public abstract class AbstractLastXDaysDailyAttendanceRateCalculator : AbstractLastXDaysAttendanceRateCalculator
    {
        public AbstractLastXDaysDailyAttendanceRateCalculator(CalculatorAppContext calculatorAppContext) : base(calculatorAppContext){}

        protected override List<StudentAttendanceEvent> GetMetricAttendanceEvents(List<StudentAttendanceEvent> studentAttendanceEvents, PeriodData periodData)
        {
            var periodAttendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => periodData.CalendarDates.Any()
                                                                && studentAttendanceEvent.EventDate.IsBetween(periodData.CalendarDates.Min(), periodData.CalendarDates.Max()));

            return periodAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                    || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                            .ToList();
        }

        protected override int GetPeriodNumerator(List<StudentAttendanceEvent> metricAttendanceEvents, int periodDenominator)
        {
            return periodDenominator - metricAttendanceEvents.Count;
        }

        protected override bool IsClassPeriodAbsenceRateMetric()
        {
            return false;
        }
    }
}
