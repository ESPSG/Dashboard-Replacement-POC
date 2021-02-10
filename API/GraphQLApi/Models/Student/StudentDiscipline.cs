using GraphQLApi.Enumerations;
using System.Collections.Generic;

namespace GraphQLApi.Models.Student
{
    public class StudentDiscipline
    {
        public int? PeriodLength;

        public IDictionary<Period, int> DisciplineIncidentsByPeriod;

        public StudentDiscipline()
        {
            DisciplineIncidentsByPeriod = new SortedDictionary<Period, int>();
        }
    }
}
