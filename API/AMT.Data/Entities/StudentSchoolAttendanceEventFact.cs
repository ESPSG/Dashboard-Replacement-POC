using AMT.Data.Descriptors.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentSchoolAttendanceEventFact
    {
        public string StudentKey { get; set; }
        public DateTime EventDate { get; set; }
        public int? TermDescriptor { get; set; }
        public short SchoolYear { get; set; }
        public string AttendanceEventCategoryDescriptor { get; set; }
        public string AttendanceEventReason { get; set; }
        
        [NotMapped]
        public DailyAttendanceCalculcationSource DailyAttendanceCalculcationSource { get; set; }
        [NotMapped]
        public StudentSchoolDim StudentSchoolDim { get; set; }
    }
}