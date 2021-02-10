using AMT.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class StudentDisciplineStateOffensesPeriodCalculator : AbstractStudentDisciplineCalculator
    {
        public StudentDisciplineStateOffensesPeriodCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected override bool IsStateOffense => true;

        protected override List<StudentDisciplineDim> FilterDisciplineRecordsByBehaviorDescription(List<StudentDisciplineDim> studentSchoolDisciplineDbRecords)
        {
            return studentSchoolDisciplineDbRecords.Where(ssd => string.IsNullOrEmpty(ssd.BehaviorDescription)
                                                            || (!string.IsNullOrEmpty(ssd.BehaviorDescription) && BehaviorDescription.Equals(ssd.BehaviorDescription)))
                                                    .ToList();
        }
    }

    public class Last20DaysStudentDisciplineStateOffensesCalculator : StudentDisciplineStateOffensesPeriodCalculator
    {
        public Last20DaysStudentDisciplineStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineStateOffensesLastX;
        }
        protected override int? ReportingPeriodLength => _appConfiguration.Value.ReportingPeriodLastXDays;


    }

    public class Last40DaysStudentDisciplineStateOffensesCalculator : StudentDisciplineStateOffensesPeriodCalculator
    {
        public Last40DaysStudentDisciplineStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineStateOffensesLastY;
        }

        protected override int? ReportingPeriodLength => _appConfiguration.Value.ReportingPeriodLastXDays;
    }
    public class YTDStudentDisciplineStateOffensesCalculator : StudentDisciplineStateOffensesPeriodCalculator
    {
        public YTDStudentDisciplineStateOffensesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentDisciplineStateOffensesYTD;
        }

        protected override int? ReportingPeriodLength => null;
    }
}
