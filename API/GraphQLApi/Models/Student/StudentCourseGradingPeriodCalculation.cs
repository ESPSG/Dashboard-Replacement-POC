using AMT.Data.Entities;
using GraphQLApi.Common;
using System.Collections.Generic;
using System.Globalization;


namespace GraphQLApi.Models.Student
{
    public class StudentCourseGradingPeriodCalculation
    {
        private readonly GraphQLApi.Common.Metric.CourseGradeMetric _metric;
        private readonly List<StudentGradeContainer> _studentGrades;

        public StudentCourseGradingPeriodCalculation(GraphQLApi.Common.Metric.CourseGradeMetric metric, GradingPeriodDim gradingPeriod, string studentUniqueId)
        {
            _metric = metric;
            GradingPeriod = gradingPeriod;
            StudentUniqueId = studentUniqueId;
            _studentGrades = new List<StudentGradeContainer>();
        }

        public int NumberOfGradesAtOrBelowThreshold { get; set; }

        public int NumberOfGrades
        {
            get { return _studentGrades.Count; }
        }

        public IEnumerable<StudentGradeContainer> StudentGrades
        {
            get { return _studentGrades; }
        }

        public GraphQLApi.Common.Metric Metric
        {
            get { return _metric; }
        }

        public string StudentUniqueId { get; private set; }

        public bool LastCompletedGradingPeriod { get; set; }

        public GradingPeriodDim GradingPeriod { get; private set; }

        public string Value
        {
            get { return NumberOfGradesAtOrBelowThreshold.ToString(CultureInfo.InvariantCulture); }
        }

        public MetricStateType MetricStateType { get; set; }
        public int? Trend { get; set; }
        public bool Flag { get; set; }

        int Numerator
        {
            get { return NumberOfGradesAtOrBelowThreshold; }
        }

        int Denominator
        {
            get { return NumberOfGrades; }
        }

        public string Context
        {
            get { return GradingPeriod.GradingPeriodDescription; }
        }

        public StudentGradeContainer AddStudentGrade(StudentGrade studentGrade)
        {
            var studentGradeContainer = new StudentGradeContainer(studentGrade);
            _studentGrades.Add(studentGradeContainer);
            return studentGradeContainer;
        }
    }

    public class StudentGradeContainer
    {
        private readonly StudentGrade _studentGrade;

        public StudentGradeContainer(StudentGrade studentGrade)
        {
            _studentGrade = studentGrade;
        }

        public StudentGrade StudentGrade
        {
            get { return _studentGrade; }
        }

        public bool IsDropping { get; set; }
        public int? Trend { get; set; }
    }
}