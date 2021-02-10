using AMT.Data.Descriptors;
using GraphQLApi.Common;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class StudentFailingSubjectAreaCourseGradesCalculator : AbstractStudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesCalculator(CalculatorAppContext appContext) : base(appContext)
        {

        }

        protected override Metric.CourseGradeMetric CourseGradeGranularMetric => (Common.Metric.CourseGradeMetric)_metricInfo;
        protected override Metric.CourseGradeMetric CourseGradeContainerMetric => Common.Metric.CourseGradeMetric.StudentFailingGrades;
    }

    public class StudentFailingSubjectAreaCourseGradesReadingELACalculator : StudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesReadingELACalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentFailingSubjectAreaCourseGradesReadingEla;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading
                                                                     , AcademicSubjectDescriptor.EnglishLanguageArts };

    }

    public class StudentFailingSubjectAreaCourseGradesWritingCalculator : StudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesWritingCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentFailingSubjectAreaCourseGradesWriting;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Writing };

    }

    public class StudentFailingSubjectAreaCourseGradesMathematicsCalculator : StudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesMathematicsCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentFailingSubjectAreaCourseGradesMathematics;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Mathematics };

    }

    public class StudentFailingSubjectAreaCourseGradesScienceCalculator : StudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesScienceCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentFailingSubjectAreaCourseGradesScience;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Science };

    }

    public class StudentFailingSubjectAreaCourseGradesSocialStudiesCalculator : StudentFailingSubjectAreaCourseGradesCalculator
    {
        public StudentFailingSubjectAreaCourseGradesSocialStudiesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentFailingSubjectAreaCourseGradesSocialStudies;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.SocialStudies };

    }
}
