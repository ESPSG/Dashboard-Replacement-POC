using AMT.Data.Entities;
using System.Collections.Generic;

namespace GraphQLApi.Models.Student
{
    public class GradingScale
    {
        public bool IsLetterGradingScale;
        public bool IsNumericGradingScale;
        public List<GradingScaleGradeLevelDim> GradingScaleGradeLevelDims;
        public List<GradingScaleGradeDim> GradingScaleGradeDims;
        public IDictionary<int, GradingScaleMetricThresholdDim> GradingScaleMetricThresholdDims;
    }
}
