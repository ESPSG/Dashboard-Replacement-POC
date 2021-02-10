
namespace AMT.Data.Descriptors.Enumerations
{
    public class DailyAttendanceCalculcationSource : Enumeration<DailyAttendanceCalculcationSource, string>
    {
        public static readonly DailyAttendanceCalculcationSource SchoolAttendanceEventSource = new DailyAttendanceCalculcationSource("School", "School Attendance Event Source");
        public static readonly DailyAttendanceCalculcationSource SectionAttendanceEventSource = new DailyAttendanceCalculcationSource("Section", "Section Attendance Event Source");
        public static readonly DailyAttendanceCalculcationSource BothAttendanceEventSources = new DailyAttendanceCalculcationSource("Both", "Both Attendance Event Sources");
        public DailyAttendanceCalculcationSource(string value, string displayName) : base(value, displayName) { }
    }
}
