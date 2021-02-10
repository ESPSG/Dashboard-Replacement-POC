using GraphQLApi.Calculators;
using GraphQLApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Repository
{
    public class MetricCalculatorRepository : IMetricCalculatorRepository
    {
        private IDictionary<int, Type> _metricCalculatorTypeDictionary;

        public MetricCalculatorRepository()
        {
            _metricCalculatorTypeDictionary = new Dictionary<int, Type>();
            var calculatorType = typeof(IStudentMetricCalculator);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => calculatorType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            foreach(var calcImplType in types)
            {
                try
                {
                    Object[] args = { null };
                    IStudentMetricCalculator calculator = Activator.CreateInstance(calcImplType, args) as IStudentMetricCalculator;
                    int metricId = calculator.GetMetricId();
                    _metricCalculatorTypeDictionary[metricId] = calcImplType;
                }
                catch(Exception e)
                {//Do nothing
                }
            }

        }

        public void RegisterCalculator(IStudentMetricCalculator metricCalculator)
        {
            _metricCalculatorTypeDictionary.Add(metricCalculator.GetMetricId(), metricCalculator.GetType());
        }

        public IStudentMetricCalculator GetStudentMetricCalculator(int metricId, CalculatorAppContext calculatorAppContext)
        {
            if (_metricCalculatorTypeDictionary.ContainsKey(metricId))
            {
                var type = _metricCalculatorTypeDictionary[metricId];
                Object[] args = { calculatorAppContext };
                IStudentMetricCalculator calculator = Activator.CreateInstance(type, args) as IStudentMetricCalculator;
                return calculator;
            }
            return null;
        }

        public ICollection<int> GetProcessableMetrics()
        {
            return _metricCalculatorTypeDictionary.Keys;

        }
    }
}
