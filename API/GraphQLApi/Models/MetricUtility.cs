using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Models
{
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
