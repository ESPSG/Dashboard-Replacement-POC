using AMT.Data.Descriptors.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentSectionAttendanceEventFact
    {
        public string StudentKey { get; set; }
        public DateTime EventDate { get; set; }
        public string SectionIdentifier { get; set; }
        public string SessionName { get; set; }
        public string LocalCourseCode { get; set; }
        public int? TermDescriptor { get; set; }
        public short? SchoolYear { get; set; }
        public string AttendanceEventCategoryDescriptor { get; set; }
        public string AttendanceEventReason { get; set; }
    }
}