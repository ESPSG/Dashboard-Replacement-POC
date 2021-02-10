using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentAssessmentDim
    {
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string AssessmentIdentifier { get; set; }
        public string StudentAssessmentIdentifier { get; set; }
        public string AssessmentCategory { get; set; }
        public string AssessmentTitle { get; set; }
        public string AcademicSubject { get; set; }
        public string AssessedGradeLevel { get; set; }
        public int? Version { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string ReasonNotTested { get; set; }
        public decimal? MaxScoreResult { get; set; }
        public int? PerformanceLevel { get; set; }
        public bool? PerformanceLevelMet { get; set; }
        public string Namespace { get; set; }

        [NotMapped]
        public List<StudentAssessmentScoreResultDim> ScoreResults { get; set; }
        [NotMapped]
        public decimal? BenchmarkAssessmentRatio { get; set; }
        [NotMapped]
        public int AssessmentCorrectItems { get; set; }
        [NotMapped]
        public int AssessmentTotalItems { get; set; }
    }
}