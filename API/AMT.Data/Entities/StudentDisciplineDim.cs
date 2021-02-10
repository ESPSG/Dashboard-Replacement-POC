using System;

namespace AMT.Data.Entities
{
    public class StudentDisciplineDim
    {
        public string StudentKey { get; set; }
        public string IncidentIdentifier { get; set; }
        public string SchoolKey { get; set; }
        public DateTime IncidentDate { get; set; }
        public string BehaviorDescription { get; set; }
        public string DisciplineDescription { get; set; }
        public string BehaviorDetailedDescription { get; set; }
    }
}
