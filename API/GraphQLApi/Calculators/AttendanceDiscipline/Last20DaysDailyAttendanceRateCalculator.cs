using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last20DaysDailyAttendanceRateCalculator : AbstractLastXDaysDailyAttendanceRateCalculator
    {
        public Last20DaysDailyAttendanceRateCalculator(CalculatorAppContext appContext):base(appContext)
        {
            _metricInfo = Common.Metric.StudentDailyAttendanceRateLastXDays;
        }
        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastXDays;
        }
    }
}
