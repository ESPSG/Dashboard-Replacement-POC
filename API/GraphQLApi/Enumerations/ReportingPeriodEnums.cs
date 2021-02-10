using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Enumerations
{
    public enum Period
    {
        First = 1,
        Second = 2
    }

    public enum ReportingPeriodLength
    {
        LastXDays = 20,
        LastYDays = 40
    }
}
