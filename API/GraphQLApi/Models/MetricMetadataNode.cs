using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Models
{
    public class MetricMetadataNode
    {
        public MetricMetadataNode(MetricMetadataTree tree)
        {
            Tree = tree;
            Children = new List<MetricMetadataNode>();
        }
        public MetricMetadataTree Tree { get; set; }
        public int MetricNodeId { get; set; }
        public int MetricVariantId { get; set; }
        public int MetricId { get; set; }
        public MetricMetadataNode Parent { get; set; }
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
        public int? ChildDomainEntityMetricId { get; set; }

        public MetricMetadataNode NextSibling { get; set; }

        private List<MetricMetadataNode> children;

        /// <summary>
        /// Gets or sets the children for the current node in the metadata hierarchy.
        /// </summary>
        public IEnumerable<MetricMetadataNode> Children
        {
            get { return children; }
            set { children = value.ToList(); }
        }
    }

    public class Metric
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
    }

    public static class MetricUtility
    {
        public static string GetMetricTypeName(int metricTypeId)
        {
            switch (metricTypeId)
            {
                case 1: return "ContainerMetric";
                case 2: return "AggregateMetric";
                case 3: return "GranularMetric";
                default: return string.Empty;
            }
        }

        public static string GetDomainEntityName(int? domainEntityTypeId, IEnumerable<DomainEntityType> domainEntityTypeData)
        {
            if (domainEntityTypeId == null)
                return string.Empty;

            string entityTypeName = domainEntityTypeData
                .Where(x => x.DomainEntityTypeId == domainEntityTypeId)
                .Select(x => x.DomainEntityTypeName).SingleOrDefault();

            return entityTypeName;
        }

        public static string GetMetricState(int metricStateTypeId)
        {
            switch (metricStateTypeId)
            {
                case 1: return "Good";
                case 2: return "Caution";
                case 3: return "Bad";
                case 4: return "N/A";
                case 5: return "None";
                case 6: return "VeryGood";
                default: return string.Empty;
            }
        }
    }
}