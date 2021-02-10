using System;
using System.Collections.Generic;
using System.Text;
using AMT.Data.Descriptors.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using AMT.Data.Entities;

namespace AMT.Data.Models
{
    public class StudentAttendanceEvent
    {
        public DailyAttendanceCalculcationSource DailyAttendanceCalculcationSource { get; set; }
        public StudentSectionDim StudentSectionDim { get; set; }
        public DateTime EventDate { get; set; }
        public string AttendanceEventReason { get; set; }
        public string AttendanceEventCategoryDescriptor { get; set; }

        public bool IsStudentSchoolAttendanceEvent
        {
            get { return StudentSectionDim == null; }
        }

        public bool IsStudentSectionAttendanceEvent
        {
            get { return StudentSectionDim != null; }
        }

        public bool IsForDailyAttendance()
        {
            if (DailyAttendanceCalculcationSource == DailyAttendanceCalculcationSource.BothAttendanceEventSources)
            {
                return IsStudentSchoolAttendanceEvent || (StudentSectionDim.HomeroomIndicator.HasValue && StudentSectionDim.HomeroomIndicator.Value);
            }

            if (DailyAttendanceCalculcationSource == DailyAttendanceCalculcationSource.SchoolAttendanceEventSource)
            {
                return IsStudentSchoolAttendanceEvent;
            }

            return IsStudentSectionAttendanceEvent && (StudentSectionDim.HomeroomIndicator.HasValue && StudentSectionDim.HomeroomIndicator.Value);
        }
    }
}
