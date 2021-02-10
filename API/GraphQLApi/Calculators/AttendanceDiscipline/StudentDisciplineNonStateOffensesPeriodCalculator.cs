using AMT.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class StudentDisciplineNonStateOffensesPeriodCalculator : AbstractStudentDisciplineCalculator
    {
        public StudentDisciplineNonStateOffensesPeriodCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected override bool IsStateOffense => false;

        protected override List<StudentDisciplineDim> FilterDisciplineRecordsByBehaviorDescription(List<StudentDisciplineDim> studentSchoolDisciplineDbRecords)
        {
            return studentSchoolDisciplineDbRecords.Where(ssd => string.IsNullOrEmpty(ssd.BehaviorDescription)
                                                            || (!string.IsNullOrEmpty(ssd.BehaviorDescription) && !BehaviorDescription.Equals(ssd.BehaviorDescription)))
                                                    .ToList();
        }
    }

    public class Last20DaysStudentDisciplineNonStateOffensesCalculator : StudentDisciplineNonStateOffensesPeriodCalculator
    {
        public Last20DaysStudentDisciplineNonStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineNonStateOffensesLastX;
        }

        protected override int? ReportingPeriodLength => _appConfiguration.Value.ReportingPeriodLastXDays;
    }

    public class Last40DaysStudentDisciplineNonStateOffensesCalculator : StudentDisciplineNonStateOffensesPeriodCalculator
    {
        public Last40DaysStudentDisciplineNonStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineNonStateOffensesLastY;
        }
        protected override int? ReportingPeriodLength => _appConfiguration.Value.ReportingPeriodLastYDays;
    }
    public class YTDStudentDisciplineNonStateOffensesCalculator : StudentDisciplineNonStateOffensesPeriodCalculator
    {
        public YTDStudentDisciplineNonStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineNonStateOffensesYTD;
        }
        protected override int? ReportingPeriodLength => null;
    }
}
