using System;
using System.Collections.Generic;
using AMT.Data.Entities;

namespace AMT.Data.Common
{
    public class GradingPeriodComparer : IComparer<GradingPeriodDim>
    {
        public int Compare(GradingPeriodDim x, GradingPeriodDim y)
        {
            if (x.PeriodSequence == null || y.PeriodSequence == null)
            {
                throw new ArgumentException("GradingPeriod cannot have null PeriodSequence for this metric");
            }

            return x.PeriodSequence.Value.CompareTo(y.PeriodSequence.Value);
        }
    }
}