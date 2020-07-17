using Data.Models;
using GraphQLApi.Models;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IMetricRepository
    {
        IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, int? metricId);

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, List<int> metricIds);
    }
}
