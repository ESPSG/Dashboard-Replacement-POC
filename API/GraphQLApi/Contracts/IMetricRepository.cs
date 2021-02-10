using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Contracts
{
    public interface IMetricRepository
    {
        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, int? metricId);

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, List<int> metricIds);
    }
}
