using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Contracts
{
    public interface IStudentMetricCalculator
    {
        public int GetMetricId();

        public List<StudentMetric> GetStudentMetrics(string studentKey, string schoolKey);

        public IDictionary<string,List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey);
    }
}
