namespace GraphQLApi.Models.Student
{
    public class StudentIndicator
    {
        public string StudentUniqueId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
