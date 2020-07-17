using GraphQLApi.Models;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IMetricMetadataRepository
    {
        IEnumerable<Metric> GetMetricsMetadata(int? metricId);
        MetricMetadataTree GetMetadataTree();
    }
}
