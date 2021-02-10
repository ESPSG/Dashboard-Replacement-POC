using AMT.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public class StudentSectionDim : IDateRange
    {
        public string StudentSectionKey { get; set; }
        public string StudentKey { get; set; }
        public string SectionKey { get; set; }
        public string LocalCourseCode { get; set; }
        public string Subject { get; set; }
        public string CourseTitle { get; set; }
        public string TeacherName { get; set; }
        public string StudentSectionStartDateKey { get; set; }
        public string StudentSectionEndDateKey { get; set; }
        public string SchoolKey { get; set; }
        public string SchoolYear { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? HomeroomIndicator { get; set; }
        public string SectionIdentifier { get; set; }
        public string SessionName { get; set; }
        public string SectionId { get; set; }

        [NotMapped]
        DateTime IDateRange.Start
        {
            get { return BeginDate; }
        }
        [NotMapped]
        DateTime IDateRange.End
        {
            get { return EndDate ?? DateTime.MaxValue; }
        }
    }
}