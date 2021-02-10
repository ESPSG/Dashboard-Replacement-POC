using System;
using System.Collections.Generic;
using System.Linq;

namespace AMT.Data.Common
{
    public static class DateRangeExtensions
    {
        public static int CountDaysInRange(this IEnumerable<DateTime> dates, IEnumerable<IDateRange> dateRanges)
        {
            return dates.Count(dateRanges.ContainsDate);
        }

        public static int CountDaysInRange(this IEnumerable<DateTime> dates, IDateRange dateRanges)
        {
            return dates.Count(dateRanges.ContainsDate);
        }

        public static bool ContainsDate(this IEnumerable<IDateRange> dateRanges, DateTime date)
        {
            return dateRanges.Any(dr => date.IsBetween(dr.Start, dr.End));
        }

        public static bool ContainsDate(this IDateRange dateRange, DateTime date)
        {
            return date.IsBetween(dateRange.Start, dateRange.End);
        }

        public static bool IsInRange(this DateTime dateTime, IDateRange dateRange)
        {
            return dateTime.IsBetween(dateRange.Start, dateRange.End);
        }

        public static IEnumerable<DateTime> DaysInRange(this IEnumerable<DateTime> dates, IEnumerable<IDateRange> dateRanges)
        {
            return dates.Where(dateRanges.ContainsDate);
        }

        public static IEnumerable<DateTime> DaysInRange(this IEnumerable<DateTime> dates, IDateRange dateRange)
        {
            return dates.Where(dateRange.ContainsDate);
        }

        public static bool OverlapsWith(this IDateRange dateRange, IDateRange possiblyOverlapping)
        {
            return dateRange.End >= possiblyOverlapping.Start && dateRange.Start <= possiblyOverlapping.End;
        }
    }
}