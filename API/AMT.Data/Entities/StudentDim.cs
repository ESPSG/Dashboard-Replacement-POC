using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AMT.Data.Entities
{
    public class StudentDim
    {
        public string StudentKey { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastSurname { get; set; }
        public string Gender { get; set; }
        public string Races { get; set; }
        public bool? HispanicLatinoEthnicity { get; set; }
        public string SchoolKey { get; set; }
        public string GradeLevel { get; set; }
        public int SortOrder { get; set; }
        public string GradeLevelDisplayText { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCategory { get; set; }
        public string AddressStreetNumberName { get; set; }
        public string AddressApartmentRoomSuiteNumber { get; set; }
        public string AddressBuildingSiteNumber { get; set; }
        public string AddressCity { get; set; }
        public int? AddressState { get; set; }
        public string AddressPostalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Characteristics { get; set; }
        public string Indicators { get; set; }
        public string LimitedEnglishProficiency { get; set; }
        public bool? EconomicDisadvantaged { get; set; }
        public bool? HasFreeOrReducedPriceFoodServiceEligibility { get; set; }
        public int LocalEducationAgencyKey { get; set; }
        public long StudentSchoolAssociationOrderKey { get; set; }
        public long StudentLeaSchoolAssociationOrderKey { get; set; }
        public bool? IsEnrolledToSchool { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                var fullName = string.Concat(FirstName, " ", (string.IsNullOrWhiteSpace(MiddleName) ? string.Empty : string.Concat(MiddleName.Substring(0, 1), ". ")), LastSurname);
                return fullName;
            }
        }

        [NotMapped]
        public string AddressLine1
        {
            get
            {
                return string.Concat(
                    AddressStreetNumberName,
                    (string.IsNullOrEmpty(AddressApartmentRoomSuiteNumber) ? "" : string.Concat(" ", AddressApartmentRoomSuiteNumber)),
                    (string.IsNullOrEmpty(AddressBuildingSiteNumber) ? "" : string.Concat(" ", AddressBuildingSiteNumber))
                    );
            }
        }

        [NotMapped]
        public string AddressLine2 { get; set; }

        [NotMapped]
        public string AddressLine3 { get; set; }
    }
}