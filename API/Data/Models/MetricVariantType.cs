using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MetricVariantType
    {
        public MetricVariantType()
        {
            MetricVariant = new HashSet<MetricVariant>();
        }

        public int MetricVariantTypeId { get; set; }
        public string MetricVariantTypeName { get; set; }

        public virtual ICollection<MetricVariant> MetricVariant { get; set; }
    }
}
