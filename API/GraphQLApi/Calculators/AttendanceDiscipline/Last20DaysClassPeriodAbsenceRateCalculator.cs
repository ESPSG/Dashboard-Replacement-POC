using AMT.Data.Models;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class Last20DaysClassPeriodAbsenceRateCalculator : AbstractLastXDaysClassPeriodAbsenceRateCalculator
    {
        public Last20DaysClassPeriodAbsenceRateCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentClassPeriodAbsenceRateLastXDays;
        }

        protected override int GetPeriodLength()
        {
            return _appConfiguration.Value.ReportingPeriodLastXDays;
        }
    }
}
