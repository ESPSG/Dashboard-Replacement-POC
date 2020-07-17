using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Metric
    {
        public Metric()
        {
            MetricInstance = new HashSet<MetricInstance>();
            MetricNode = new HashSet<MetricNode>();
            MetricVariant = new HashSet<MetricVariant>();
        }

        public int MetricId { get; set; }
        public int MetricTypeId { get; set; }
        public int? DomainEntityTypeId { get; set; }
        public string MetricName { get; set; }
        public int? TrendInterpretation { get; set; }
        public bool? Enabled { get; set; }
        public int? ChildDomainEntityMetricId { get; set; }
        public int? OverriddenByMetricId { get; set; }

        public virtual DomainEntityType DomainEntityType { get; set; }
        public virtual MetricType MetricType { get; set; }
        public virtual ICollection<MetricInstance> MetricInstance { get; set; }
        public virtual ICollection<MetricNode> MetricNode { get; set; }
        public virtual ICollection<MetricVariant> MetricVariant { get; set; }
    }
}
