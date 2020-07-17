using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Models
{
    public class MetricMetadataTree
    {
        //private MetricMetadataTree _metadataTree;
        private readonly IList<MetricMetadataMetricJoin> _metricNodeToMetricJoinData;

        public MetricMetadataNode Parent
        {
            get { return null; }
            set { throw new NotSupportedException("Cannot set the Parent of a Tree."); }
        }

        private List<MetricMetadataNode> _children;

        public IEnumerable<MetricMetadataNode> Children
        {
            get { return _children; }
            set { _children = value.ToList(); }
        }

        public MetricMetadataTree()
        {
        }

        public MetricMetadataTree(IList<MetricMetadataMetricJoin> metricNodeToMetricJoinData)
        {
            _metricNodeToMetricJoinData = metricNodeToMetricJoinData;
            InitializeMetricTree();
        }

        /// <summary>
        /// Gets complete metric collection with parent-child hierarchy
        /// </summary>
        /// <returns>Complete metric collection</returns>
        public void InitializeMetricTree()
        {
            var treeRootChildren = new List<MetricMetadataNode>();
            MetricMetadataNode previousMetadataNode = null;

            var rootNodes = _metricNodeToMetricJoinData.Where(x => x.MetricNodeId == x.RootNodeId);
            foreach (var rootNode in rootNodes)
            {
                var node = TransformJoinToNode(null, rootNode);

                if (previousMetadataNode != null)
                    previousMetadataNode.NextSibling = node;

                previousMetadataNode = node;

                //set _children to this node
                node.Children = GetChildren(node);

                //add this child to the root node
                treeRootChildren.Add(node);
            }

            Children = treeRootChildren;

            return;
        }

        /// <summary>
        /// Converts the MetricMetadataMetricJoin to MetricMetadataNode
        /// </summary>
        /// <param name="parent">MetricMetadataNode</param>
        /// <param name="childNode">MetricMetadataMetricJoin</param>
        /// <returns>MetricMetadataNode</returns>
        private MetricMetadataNode TransformJoinToNode(MetricMetadataNode parent, MetricMetadataMetricJoin childNode)
        {
            var metadataNode = new MetricMetadataNode(this)
            {
                Parent = parent,
                DisplayName = childNode.DisplayName,
                DisplayOrder = childNode.DisplayOrder,
                ChildDomainEntityMetricId = childNode.ChildMetricId,
                Enabled = childNode.Enabled,
                Format = childNode.Format,
                ListFormat = childNode.ListFormat,
                ListDataLabel = childNode.ListDataLabel,
                Description = childNode.Description,
                MetricId = childNode.MetricId,
                Name = childNode.Name,
                MetricNodeId = childNode.MetricNodeId,
                MetricVariantId = childNode.MetricVariantId,
                ShortName = childNode.ShortName,
                Tooltip = childNode.Tooltip,
                MetricTypeId = childNode.MetricTypeId,
                MetricVariantTypeId = childNode.MetricVariantTypeId,
                Url = childNode.Url,
                NumeratorDenominatorFormat = childNode.NumeratorDenominatorFormat,
                RootNodeId = childNode.RootNodeId,
                TrendInterpretation = childNode.TrendInterpretation,
                DomainEntityType = childNode.DomainEntityType,
            };
            return metadataNode;
        }

        /// <summary>
        /// Returns the metric _children
        /// </summary>
        /// <param name="parent">Represents parent metric</param>
        /// <returns>Return child metric collection</returns>
        private IEnumerable<MetricMetadataNode> GetChildren(MetricMetadataNode parent)
        {
            var returnList = new List<MetricMetadataNode>();
            MetricMetadataNode previousMetadataNode = null;
            var childNodes = _metricNodeToMetricJoinData.Where(x => x.ParentNodeId == parent.MetricNodeId).OrderBy(x => x.DisplayOrder).ToList();
            foreach (var childNode in childNodes)
            {
                var metadataNode = TransformJoinToNode(parent, childNode);

                if (previousMetadataNode != null)
                    previousMetadataNode.NextSibling = metadataNode;

                previousMetadataNode = metadataNode;

                metadataNode.Children = GetChildren(metadataNode);
                returnList.Add(metadataNode);
            }

            return returnList;
        }

        public List<Metric> GetMetricsMetadata(int? metricId)
        {
            MetricMetadataNode metricMetadataNode = null;
            List<Metric> metricMetadata = new List<Metric>();
            if (metricId.HasValue)
            {
                if (Children.Any())
                {
                    foreach (var child in Children)
                    {
                        if (metricMetadataNode == null)
                        {
                            metricMetadataNode = FindMetricNodeById(child, metricId.Value);
                        }
                    }
                }
                if (metricMetadataNode != null && metricMetadataNode.Children.Any())
                {
                    ReadMetricsMetadataChildren(metricMetadataNode, metricMetadata);
                }
            }
            else
            {
                if (Children.Any())
                {
                    foreach (var children in Children)
                    {
                        ReadMetricsMetadataChildren(children, metricMetadata);
                    }
                }
            }

            return metricMetadata;
        }

        private void ReadMetricsMetadataChildren(MetricMetadataNode metricMetadataNode, List<Metric> metricMetadata)
        {
            if (metricMetadataNode.Children.Any())
            {
                foreach (var child in metricMetadataNode.Children)
                {
                    Metric metric = new Metric
                    {
                        Id = child.MetricId,
                        Name = child.Name,
                        Type = MetricUtility.GetMetricTypeName(child.MetricTypeId)
                    };
                    if (child.Parent != null)
                    {
                        metric.ParentId = child.Parent.MetricId;
                        metric.ParentName = child.Parent.Name;
                    }
                    metricMetadata.Add(metric);
                    ReadMetricsMetadataChildren(child, metricMetadata);
                }
            }
        }

        private MetricMetadataNode FindMetricNodeById(MetricMetadataNode metricMetadataNode, int metricId)
        {
            if (metricMetadataNode.Children.Any())
            {
                foreach (var child in metricMetadataNode.Children)
                {
                    if (child.MetricId == metricId)
                    {
                        return child;
                    }
                    metricMetadataNode = FindMetricNodeById(child, metricId);
                    if (metricMetadataNode != null)
                        return metricMetadataNode;
                }
            }
            return null;
        }


    }
}
