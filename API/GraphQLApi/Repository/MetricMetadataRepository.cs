using Data.Models;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLApi.Repository
{
    public class MetricMetadataRepository : IMetricMetadataRepository
    {
        private MetricMetadataTree _metadataTree;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MetricMetadataTree GetMetadataTree()
        {
            return _metadataTree;
        }

        public MetricMetadataRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            init();
        }

        private void init()
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                DDSContext dbContext = scope.ServiceProvider.GetRequiredService<DDSContext>();
                var metricNodeToMetricJoinData = GetMetricNodeToMetricJoinData(dbContext);
                _metadataTree = new MetricMetadataTree(metricNodeToMetricJoinData);
            }
        }

        public IEnumerable<Models.Metric> GetMetricsMetadata(int? metricId)
        {
            return _metadataTree.GetMetricsMetadata(metricId);
        }

        private IList<MetricMetadataMetricJoin> GetMetricNodeToMetricJoinData(DDSContext dbContext)
        {
            var domainEntityTypeData = dbContext.DomainEntityType.ToList();
            var tempMetricNodeToMetricJoin = (from mi in dbContext.MetricNode.AsNoTracking()
                                              join mui in dbContext.MetricVariant.AsNoTracking() on mi.MetricVariantId equals mui.MetricVariantId
                                              join m in dbContext.Metric.AsNoTracking() on mui.MetricId equals m.MetricId
                                              orderby mi.DisplayOrder
                                              select new
                                              {
                                                  mi.MetricNodeId,
                                                  mi.MetricVariantId,
                                                  mi.MetricId,
                                                  mi.ParentNodeId,
                                                  mi.RootNodeId,
                                                  mi.DisplayName,
                                                  mi.DisplayOrder,
                                                  m.MetricTypeId,
                                                  mui.MetricVariantTypeId,
                                                  mui.MetricName,
                                                  mui.MetricShortName,
                                                  mui.MetricDescription,
                                                  mui.MetricUrl,
                                                  mui.MetricTooltip,
                                                  mui.Format,
                                                  mui.ListFormat,
                                                  mui.ListDataLabel,
                                                  mui.NumeratorDenominatorFormat,
                                                  m.TrendInterpretation,
                                                  m.Enabled,
                                                  m.DomainEntityTypeId,
                                                  m.ChildDomainEntityMetricId
                                              }).ToList();

            return (from tm in tempMetricNodeToMetricJoin
                    select new MetricMetadataMetricJoin
                    {
                        MetricNodeId = tm.MetricNodeId,
                        MetricVariantId = tm.MetricVariantId,
                        MetricId = tm.MetricId,
                        ParentNodeId = tm.ParentNodeId.GetValueOrDefault(),
                        RootNodeId = tm.RootNodeId.GetValueOrDefault(),
                        DisplayName = String.IsNullOrEmpty(tm.DisplayName) ? tm.MetricName : tm.DisplayName,
                        DisplayOrder = tm.DisplayOrder,
                        MetricTypeId = tm.MetricTypeId,
                        MetricVariantTypeId = tm.MetricVariantTypeId,
                        Name = tm.MetricName,
                        ShortName = tm.MetricShortName,
                        Description = tm.MetricDescription,
                        Url = tm.MetricUrl,
                        Tooltip = tm.MetricTooltip,
                        Format = tm.Format,
                        ListFormat = tm.ListFormat,
                        ListDataLabel = tm.ListDataLabel,
                        NumeratorDenominatorFormat = tm.NumeratorDenominatorFormat,
                        TrendInterpretation = tm.TrendInterpretation,
                        Enabled = tm.Enabled == null || tm.Enabled.GetValueOrDefault(),
                        DomainEntityType = MetricUtility.GetDomainEntityName(tm.DomainEntityTypeId, domainEntityTypeData),
                        ChildMetricId = tm.ChildDomainEntityMetricId.GetValueOrDefault(),
                    }).ToList();
        }
    }
}