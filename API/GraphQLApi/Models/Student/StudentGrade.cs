using AMT.Data.Entities;

namespace GraphQLApi.Models.Student
{
    public class StudentGrade
    {
        public string StudentKey { get; set; }
        public GradingPeriodDim GradingPeriod { get; set; }
        public StudentSectionDim StudentSection { get; set; }
        public GradingScale GradingScale { get; set; }
        public int GradeRank { get; set; }
        public StudentSchoolDim StudentSchoolAssociation { get; set; }
        public GradingScaleMetricThresholdDim GradingScaleMetricThreshold { get; set; }
    }
}