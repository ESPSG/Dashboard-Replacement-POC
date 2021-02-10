using AMT.Data.Models;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last40DaysClassPeriodAbsenceRateCalculator : AbstractLastXDaysClassPeriodAbsenceRateCalculator
    {
        public Last40DaysClassPeriodAbsenceRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentClassPeriodAbsenceRateLastYDays;
        }

        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastYDays;
        }
    }
}
