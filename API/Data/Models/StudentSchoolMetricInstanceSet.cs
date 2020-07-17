using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class StudentSchoolMetricInstanceSet
    {
        public Guid MetricInstanceSetKey { get; set; }
        public int StudentUsi { get; set; }
        public int SchoolId { get; set; }
    }
}
