using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MetricType
    {
        public MetricType()
        {
            Metric = new HashSet<Metric>();
        }

        public int MetricTypeId { get; set; }
        public string MetricTypeName { get; set; }

        public virtual ICollection<Metric> Metric { get; set; }
    }
}
