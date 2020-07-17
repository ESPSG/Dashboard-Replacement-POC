using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class StudentIndicator
    {
        public int StudentUsi { get; set; }
        public int EducationOrganizationId { get; set; }
        public string Type { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual StudentInformation StudentUsiNavigation { get; set; }
    }
}
