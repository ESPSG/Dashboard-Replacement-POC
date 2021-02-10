using AMT.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentProgramAssociationDim : IDateRange
    {
        public string StudentKey { get; set; }
        public string ProgramType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public bool IsActive { get; set; }
        public int EducationOrganizationKey { get; set; }

        [NotMapped]
        DateTime IDateRange.Start
        {
            get { return BeginDate; }
        }
        [NotMapped]
        DateTime IDateRange.End
        {
            get { return EndDate ?? DateTime.MaxValue; }
        }
    }
}