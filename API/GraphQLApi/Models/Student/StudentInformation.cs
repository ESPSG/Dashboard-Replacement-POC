using System;
using System.Collections.Generic;

namespace GraphQLApi.Models.Student
{
    public class StudentInformation
    {
        public StudentInformation()
        {
            StudentIndicator = new HashSet<StudentIndicator>();
            StudentSchoolInformation = new HashSet<StudentSchoolInformation>();
            StudentParentInformation = new HashSet<StudentParentInformation>();
        }

        public int StudentUniqueId { get; set; }
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
        public string SchoolId { get; set; }
        public string GradeLevel { get; set; }
        public int GradeLevelSortOrder { get; set; }
        public string GradeLevelListDisplayText { get; set; }
        public string LateEnrollment { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCategory { get; set; }

        public ICollection<StudentIndicator> StudentIndicator { get; set; }
        public ICollection<StudentSchoolInformation> StudentSchoolInformation { get; set; }
        public ICollection<StudentParentInformation> StudentParentInformation { get; set; }
    }
}
