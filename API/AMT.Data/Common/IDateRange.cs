using System;

namespace AMT.Data.Common
{
    public interface IDateRange
    {
        DateTime Start { get; }
        DateTime End { get; }
    }
}