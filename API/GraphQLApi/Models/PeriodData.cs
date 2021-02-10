using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Models
{
    public class PeriodData
    {
        public PeriodData()
        {
            CalendarDates = new HashSet<DateTime>();
        }

        public HashSet<DateTime> CalendarDates { get; private set; }
    }
}
