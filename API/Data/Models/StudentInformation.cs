using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public partial class StudentInformation
    {
        public StudentInformation()
        {
            StudentIndicator = new HashSet<StudentIndicator>();
            StudentSchoolInformation = new HashSet<StudentSchoolInformation>();
            StudentParentInformation = new HashSet<StudentParentInformation>();
        }

        public int StudentUsi { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastSurname { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int? CurrentAge { get; set; }
        public int? CohortYear { get; set; }
        public string Gender { get; set; }
        public string OldEthnicity { get; set; }
        public string HispanicLatinoEthnicity { get; set; }
        public string Race { get; set; }
        public string HomeLanguage { get; set; }
        public string Language { get; set; }
        public string ParentMilitary { get; set; }
        public string SingleParentPregnantTeen { get; set; }
        public string ProfileThumbnail { get; set; }

        [NotMapped]
        public int SchoolId { get; set; }
        [NotMapped]
        public string GradeLevel { get; set; }
        [NotMapped]
        public int GradeLevelSortOrder { get; set; }
        [NotMapped]
        public string GradeLevelListDisplayText { get; set; }
        [NotMapped]
        public string LateEnrollment { get; set; }
        [NotMapped]
        public string SchoolName { get; set; }
        [NotMapped]
        public string SchoolCategory { get; set; }
        public virtual ICollection<StudentIndicator> StudentIndicator { get; set; }
        public virtual ICollection<StudentSchoolInformation> StudentSchoolInformation { get; set; }
        public virtual ICollection<StudentParentInformation> StudentParentInformation { get; set; }
    }
}
