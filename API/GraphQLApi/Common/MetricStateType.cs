namespace GraphQLApi.Common
{
    public class MetricStateType : Enumeration<MetricStateType>
    {
        public static readonly MetricStateType Good = new MetricStateType(1, "Good");
        public static readonly MetricStateType Bad = new MetricStateType(3, "Bad");
        public static readonly MetricStateType NotApplicable = new MetricStateType(4, "N/A");
        public static readonly MetricStateType None = new MetricStateType(5, "None");

        private MetricStateType(int value, string displayName) : base(value, displayName) {}
    }
}