using AMT.Data.Entities;
using System.Collections.Generic;

namespace GraphQLApi.Models.Student
{
    public class StudentAssessment
    {
        public string StudentKey;

        public List<StudentAssessmentDim> Assessments;

        public List<StudentAssessmentScoreResultDim> ScoreResults;

        public List<StudentAssessmentItemDim> AssessmentItems;

        public StudentAssessment()
        {
            Assessments = new List<StudentAssessmentDim>();
            ScoreResults = new List<StudentAssessmentScoreResultDim>();
            AssessmentItems = new List<StudentAssessmentItemDim>();
        }
    }
}