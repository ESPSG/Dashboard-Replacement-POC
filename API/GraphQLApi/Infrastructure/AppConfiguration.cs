namespace GraphQLApi.Infrastructure
{
    public class AppConfiguration
    {
        public string DailyAttendanceCalculationSource { get; set; }
        public int DaysNeededForLateEnrollment { get; set; }
        public int ReportingPeriodLastXDays { get; set; }
        public int ReportingPeriodLastYDays { get; set; }
    }
}
