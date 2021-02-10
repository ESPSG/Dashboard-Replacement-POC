using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last40DaysDailyAttendanceRateCalculator : AbstractLastXDaysDailyAttendanceRateCalculator
    {
        public Last40DaysDailyAttendanceRateCalculator(CalculatorAppContext appContext):base(appContext)
        {
            _metricInfo = Common.Metric.StudentDailyAttendanceRateLastYDays;
        }
        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastYDays;
        }
    }
}
