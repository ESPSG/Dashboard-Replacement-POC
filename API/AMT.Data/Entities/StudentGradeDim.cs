using AMT.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentGradeDim : IDateRange, ISection
    {
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string SectionIdentifier { get; set; }
        public string SessionName { get; set; }
        public string LocalCourseCode { get; set; }
        public short SchoolYear { get; set; }
        public string GradingPeriodDescription { get; set; }
        public DateTime GradingPeriodBeginDate { get; set; }
        public int GradingPeriodSequence { get; set; }
        public string LetterGradeEarned { get; set; }
        public decimal? NumericGradeEarned { get; set; }

        [NotMapped]
        public DateTime? EndDate { get; set; }
        public string GradeType { get; set; }

        [NotMapped]
        DateTime IDateRange.Start
        {
            get { return GradingPeriodBeginDate; }
        }
        [NotMapped]
        DateTime IDateRange.End
        {
            get { return EndDate ?? DateTime.MaxValue; }
        }

        public override int GetHashCode()
        {
            return SectionComparer.Instance.GetHashCode(this);
        }
        public override bool Equals(object obj)
        {
            return SectionComparer.Instance.Equals(this, obj as ISection);
        }
    }
}