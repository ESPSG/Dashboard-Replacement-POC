using System;

namespace AMT.Data.Entities
{
    public class SchoolMinMaxDateDim
    {
        public int LocalEducationAgencyKey { get; set; }
        public string SchoolKey { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public DateTime? SessionEndDate { get; set; }
    }
}