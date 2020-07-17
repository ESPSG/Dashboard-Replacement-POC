using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MetricInstance
    {
        public Guid MetricInstanceSetKey { get; set; }
        public int MetricId { get; set; }
        public int? MetricStateTypeId { get; set; }
        public string Context { get; set; }
        public string Value { get; set; }
        public string ValueTypeName { get; set; }
        public bool Flag { get; set; }
        public int? TrendDirection { get; set; }

        public virtual Metric Metric { get; set; }
    }
}
