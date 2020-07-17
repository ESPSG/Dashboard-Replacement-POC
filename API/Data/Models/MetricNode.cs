using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MetricNode
    {
        public MetricNode()
        {
            InverseParentNode = new HashSet<MetricNode>();
        }

        public int MetricNodeId { get; set; }
        public int MetricVariantId { get; set; }
        public int MetricId { get; set; }
        public int? ParentNodeId { get; set; }
        public int? RootNodeId { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }

        public virtual Metric Metric { get; set; }
        public virtual MetricVariant MetricVariant { get; set; }
        public virtual MetricNode ParentNode { get; set; }
        public virtual ICollection<MetricNode> InverseParentNode { get; set; }
    }
}
