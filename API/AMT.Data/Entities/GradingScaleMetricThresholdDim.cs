namespace AMT.Data.Entities
{
    public partial class GradingScaleMetricThresholdDim
    {
        public int GradingScaleId { get; set; }
        public int MetricId { get; set; }
        public int Value { get; set; }
        public int GradingScaleMetricThresholdId { get; set; }
    }
}
