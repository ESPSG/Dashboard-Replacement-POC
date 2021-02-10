using AMT.Data.Common;
using System;

namespace AMT.Data.Entities
{
    public class StudentSchoolDim : IDateRange
    {
        public string StudentSchoolKey { get; set; }
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string SchoolYear { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string StudentLastName { get; set; }
        public string EnrollmentDateKey { get; set; }
        public string GradeLevel { get; set; }
        public string LimitedEnglishProficiency { get; set; }
        public bool IsHispanic { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Sex { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitWithdrawDate { get; set; }
        public string GraduationPlan { get; set; }
        public string WithdrawalDescription { get; set; }
        public int LocalEducationAgencyKey { get; set; }
        public long StudentSchoolAssociationOrderKey { get; set; }
        public long StudentLeaSchoolAssociationOrderKey { get; set; }
        public bool? IsEnrolledToSchool { get; set; }
        DateTime IDateRange.Start
        {
            get { return EntryDate; }
        }

        DateTime IDateRange.End
        {
            get { return ExitWithdrawDate ?? DateTime.MaxValue; }
        }
    }
}
