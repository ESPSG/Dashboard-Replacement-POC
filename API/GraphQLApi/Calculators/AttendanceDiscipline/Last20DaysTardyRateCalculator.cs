

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last20DaysTardyRateCalculator : AbstractLastXDaysTardyRateCalculator
    {
        public Last20DaysTardyRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentTardyRateLastXDays;
        }
        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastXDays;
        }
    }
}
