using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class LocalEducationAgencyGradeLevelInformation
    {
        public int LocalEducationAgencyId { get; set; }
        public int GradeLevelTypeId { get; set; }
        public string GradeLevel { get; set; }
        public int SortOrder { get; set; }
        public string ListDisplayText { get; set; }
    }
}
