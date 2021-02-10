using System;

namespace AMT.Data.Entities
{
    public class StudentAssessmentScoreResultDim
    {
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string AssessmentIdentifier { get; set; }
        public string StudentAssessmentIdentifier { get; set; }
        public string AssessmentTitle { get; set; }
        public string AcademicSubject { get; set; }
        public string AssessedGradeLevel { get; set; }
        public int? Version { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string Result { get; set; }
        public string ReportingMethod { get; set; }
        public string ResultDataType { get; set; }
        public string Namespace { get; set; }
    }
}