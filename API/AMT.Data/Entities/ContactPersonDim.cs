using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class ContactPersonDim
    {
        public string UniqueKey { get; set; }
        public string ContactPersonKey { get; set; }
        public string StudentKey { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContactLastName { get; set; }
        public string RelationshipToStudent { get; set; }
        public string ContactHomeAddress { get; set; }
        public string ContactPhysicalAddress { get; set; }
        public string ContactMailingAddress { get; set; }
        public string ContactWorkAddress { get; set; }
        public string ContactTemporaryAddress { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string WorkEmailAddress { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? StudentLivesWith { get; set; }
        public bool? IsEmergencyContact { get; set; }
        public int? ContactPriority { get; set; }
        public string ContactRestrictions { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                var fullName = string.Concat(ContactFirstName, " ", (string.IsNullOrWhiteSpace(ContactMiddleName) ? string.Empty : string.Concat(ContactMiddleName.Substring(0, 1), ". ")), ContactLastName);
                return fullName;
            }
        }
    }
}
