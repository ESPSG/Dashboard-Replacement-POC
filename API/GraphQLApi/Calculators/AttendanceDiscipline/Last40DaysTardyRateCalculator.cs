

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last40DaysTardyRateCalculator : AbstractLastXDaysTardyRateCalculator
    {
        public Last40DaysTardyRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentTardyRateLastYDays;
        }
        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastYDays;
        }
    }
}
