using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MetricVariant
    {
        public MetricVariant()
        {
            MetricNode = new HashSet<MetricNode>();
        }

        public int MetricVariantId { get; set; }
        public int MetricId { get; set; }
        public int MetricVariantTypeId { get; set; }
        public string MetricName { get; set; }
        public string MetricShortName { get; set; }
        public string MetricDescription { get; set; }
        public string MetricUrl { get; set; }
        public string MetricTooltip { get; set; }
        public string Format { get; set; }
        public string ListFormat { get; set; }
        public string ListDataLabel { get; set; }
        public string NumeratorDenominatorFormat { get; set; }
        public bool? Enabled { get; set; }

        public virtual Metric Metric { get; set; }
        public virtual MetricVariantType MetricVariantType { get; set; }
        public virtual ICollection<MetricNode> MetricNode { get; set; }
    }
}
