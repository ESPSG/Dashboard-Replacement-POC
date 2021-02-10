using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentAssessmentItemDim
    {
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string AssessmentIdentifier { get; set; }
        public string Namespace { get; set; }
        public string StudentAssessmentIdentifier { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string AssessmentTitle { get; set; }
        public string AcademicSubject { get; set; }
        public string AssessedGradeLevel { get; set; }
        public int? Version { get; set; }
        public string AssessmentItemResult { get; set; }
        public decimal? RawScoreResult { get; set; }
        public string AssessmentResponse { get; set; }
       
        [NotMapped]
        public List<StudentAssessmentScoreResultDim> ScoreResults { get; set; }
    }
}