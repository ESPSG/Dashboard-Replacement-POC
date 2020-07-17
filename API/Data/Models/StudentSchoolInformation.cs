﻿using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class StudentSchoolInformation
    {
        public int StudentUsi { get; set; }
        public int SchoolId { get; set; }
        public string GradeLevel { get; set; }
        public string Homeroom { get; set; }
        public string LateEnrollment { get; set; }
        public string IncompleteTranscript { get; set; }
        public DateTime? DateOfEntry { get; set; }
        public DateTime? DateOfWithdrawal { get; set; }
        public string WithdrawalCode { get; set; }
        public string WithdrawalDescription { get; set; }
        public string GraduationPlan { get; set; }
        public string ExpectedGraduationYear { get; set; }
        public string FeederSchools { get; set; }

        public virtual StudentInformation StudentUsiNavigation { get; set; }
    }
}
