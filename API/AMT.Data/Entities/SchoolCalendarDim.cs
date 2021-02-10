using System;

namespace AMT.Data.Entities
{
    public class SchoolCalendarDim
    {
        public int SchoolKey { get; set; }
        public DateTime Date { get; set; }
        public int? CalendarEventDescriptor { get; set; }
        public long DaysBack { get; set; }

        public int GetPeriodIndex(int periodLength)
        {
            return (int)((DaysBack / periodLength) + 1);
        }
    }
}