using GraphQLApi.Calculators;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Repository
{
    public class MetricRepository : IMetricRepository
    {
        private IMetricCalculatorRepository _metricCalculatorRepository;
        private CalculatorAppContext _calculatorAppContext;
        public MetricRepository(CalculatorAppContext calculatorAppContext, IMetricCalculatorRepository metricCalculatorRepository)
        {
            _metricCalculatorRepository = metricCalculatorRepository;
            _calculatorAppContext = calculatorAppContext;
        }

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, int? metricId)
        {
            if (metricId.HasValue && _metricCalculatorRepository.GetProcessableMetrics().Contains((int)metricId))
            {
                IStudentMetricCalculator metricCalculator = _metricCalculatorRepository.GetStudentMetricCalculator((int)metricId, _calculatorAppContext);
                return metricCalculator.GetStudentMetrics(studentUsi + "", schoolId + "");
            }
            else
            {
                //If no metric calculator is present, just return an empty list.
                return new List<StudentMetric>();
            }
        }

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, List<int> metricIds)
        {
            //Get the list of metric ID's processed by _metricCalculatorRepository
            var processableMetrics = _metricCalculatorRepository.GetProcessableMetrics().Intersect(metricIds);
            var unProcessableMetrics = metricIds.Except(processableMetrics).ToList();

            List<StudentMetric> returnList = new List<StudentMetric>();
            foreach (var processableMetricId in processableMetrics)
            {
                IStudentMetricCalculator metricCalculator = _metricCalculatorRepository.GetStudentMetricCalculator(processableMetricId, _calculatorAppContext);
                returnList.AddRange(metricCalculator.GetStudentMetrics(studentUsi + "", schoolId + ""));
            }

            return returnList;
        }
    }
}
