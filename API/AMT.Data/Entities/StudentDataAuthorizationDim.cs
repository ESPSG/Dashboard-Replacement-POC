using System;

namespace AMT.Data.Entities
{
    public class StudentDataAuthorizationDim
    {
        public string StudentKey { get; set; }
        public string SchoolKey { get; set; }
        public string SectionId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
