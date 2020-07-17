using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Models
{
    public class MetricMetadataMetricJoin
    {
        public int MetricNodeId { get; set; }
        public int MetricVariantId { get; set; }
        public int MetricId { get; set; }
        public int? ParentNodeId { get; set; }
        public int RootNodeId { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public int MetricTypeId { get; set; }
        public int MetricVariantTypeId { get; set; }
        public string DomainEntityType { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Tooltip { get; set; }
        public string Format { get; set; }
        public string ListFormat { get; set; }
        public string ListDataLabel { get; set; }
        public string NumeratorDenominatorFormat { get; set; }
        public int? TrendInterpretation { get; set; }
        public bool Enabled { get; set; }
        public int ChildMetricId { get; set; }
    }
}
