using AMT.Data.Entities;
using GraphQLApi.Calculators;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IMetricCalculatorRepository
    {
        public void RegisterCalculator(IStudentMetricCalculator metricCalculator);

        public IStudentMetricCalculator GetStudentMetricCalculator(int metricId, CalculatorAppContext calculatorAppContext);

        public ICollection<int> GetProcessableMetrics();
    }
}
