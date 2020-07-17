using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class DomainEntityType
    {
        public DomainEntityType()
        {
            Metric = new HashSet<Metric>();
        }

        public int DomainEntityTypeId { get; set; }
        public string DomainEntityTypeName { get; set; }

        public virtual ICollection<Metric> Metric { get; set; }
    }
}
