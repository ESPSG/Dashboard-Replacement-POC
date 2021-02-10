using System;

namespace GraphQLApi.Models.Student
{
    public class StudentParentInformation
    {
        public string StudentUniqueId { get; set; }
        public string FullName { get; set; }
        public string Relation { get; set; }
        public string HomeAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string MailingAddress { get; set; }
        public string WorkAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string WorkTelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool? IsPrimaryContact { get; set; }
    }
}

