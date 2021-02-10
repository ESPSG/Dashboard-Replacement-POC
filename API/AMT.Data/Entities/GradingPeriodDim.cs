using AMT.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class GradingPeriodDim : IDateRange
    {
        public GradingPeriodDim()
        {
            IsYearToDate = false;
        }

        public string GradingPeriodKey { get; set; }
        public string GradingPeriodBeginDateKey { get; set; }
        public string GradingPeriodEndDateKey { get; set; }
        public string GradingPeriodDescription { get; set; }
        public int TotalInstructionalDays { get; set; }
        public int? PeriodSequence { get; set; }
        public string SchoolKey { get; set; }
        public string SchoolYear { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        public bool HasGradingPeriodEnded { get; set; }
        [NotMapped]
        DateTime IDateRange.Start
        {
            get { return BeginDate; }
        }
        [NotMapped]
        DateTime IDateRange.End
        {
            get { return EndDate; }
        }
        [NotMapped]
        public bool IsYearToDate { get; set; }
        [NotMapped]
        public string Context
        {
            get { return (IsYearToDate) ? "YTD" : GradingPeriodDescription; }
        }
    }
}