using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class StudentParentInformation
    {
        public int ParentUsi { get; set; }
        public int StudentUsi { get; set; }
        public string FullName { get; set; }
        public string Relation { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string WorkTelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool? PrimaryContact { get; set; }
        public bool? LivesWith { get; set; }

        public virtual StudentInformation StudentUsiNavigation { get; set; }
    }
}
