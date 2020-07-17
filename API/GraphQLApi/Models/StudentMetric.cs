namespace GraphQLApi.Models
{
    public class StudentMetric
    {
        public int StudentUsi { get; set; }
        public int SchoolId { get; set; }
        public string State { get; set; }
        public string Value { get; set; }
        public int? TrendDirection { get; set; }
        public string ValueTypeName { get; set; }
        public int Id { get; set; }
        public int? TrendInterpretation { get; set; }
        public string Format { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string Type { get; set; }
    }
}