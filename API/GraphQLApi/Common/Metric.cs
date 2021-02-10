using System;

namespace GraphQLApi.Common
{
    public enum RateDirection
    {
        ZeroToOne,
        OneToZero
    }

    public abstract class Metric : Enumeration<Metric>
    {
        public static readonly Metric StudentTardyRateLastXDays = new AttendanceMetric(1077, "Student Tardy Rate Last {0} Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentTardyRateLastYDays = new AttendanceMetric(1078, "Student Tardy Rate Last {0} Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentTardyRateYearToDate = new AttendanceMetric(1079, "Student Tardy Rate Year To Date", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentTardyRate = new AttendanceMetric(1000, "Student Tardy Rate", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentDaysAbsent = new AttendanceMetric(1670, "Student Days Absent", 7, RateDirection.ZeroToOne);
        public static readonly Metric StudentTotalDaysAbsent = new AttendanceMetric(1483, "Student Total Days Absent", 7, RateDirection.ZeroToOne);
        public static readonly Metric StudentTotalUnexcusedDaysAbsent = new AttendanceMetric(1671, "Student Total Unexcused Days Absent", 7, RateDirection.ZeroToOne);
        public static readonly Metric StudentDailyAttendanceRate = new AttendanceMetric(2, "Student Daily Attendance Rate Historical", .95m, RateDirection.OneToZero);
        public static readonly Metric StudentDailyAttendanceRateLastXDays = new AttendanceMetric(3, "Student Daily Attendance Rate Last {0} Days", .9m, RateDirection.OneToZero);
        public static readonly Metric StudentDailyAttendanceRateLastYDays = new AttendanceMetric(4, "Student Daily Attendance Rate Last {0} Days", .9m, RateDirection.OneToZero);
        public static readonly Metric StudentDailyAttendanceRateYearToDate = new AttendanceMetric(5, "Student Daily Attendance Rate Year To Date", .9m, RateDirection.OneToZero);
        public static readonly Metric StudentClassPeriodAbsenceRate = new AttendanceMetric(6, "Student Class Period Absence Rate", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentClassPeriodAbsenceRateLastXDays = new AttendanceMetric(7, "Student Class Period Absence Rate Last {0} Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentClassPeriodAbsenceRateLastYDays = new AttendanceMetric(8, "Student Class Period Absence Rate Last {0} Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric StudentClassPeriodAbsenceRateYearToDate = new AttendanceMetric(9, "Student Class Period Absence Rate Year To Date", .1m, RateDirection.ZeroToOne);

        public static readonly Metric StudentDisciplineStateOffensesContainer = new StudentDisciplineMetric(105, "Student - State Offenses");
        public static readonly StudentDisciplineMetric StudentDisciplineStateOffensesLastX = new StudentDisciplineMetric(106, "Student - State Offenses (Last {0})");
        public static readonly StudentDisciplineMetric StudentDisciplineStateOffensesLastY = new StudentDisciplineMetric(107, "Student - State Offenses (Last {0})");
        public static readonly StudentDisciplineMetric StudentDisciplineStateOffensesYTD = new StudentDisciplineMetric(108, "Student - State Offenses (YTD)");
        public static readonly Metric StudentDisciplineNonStateOffensesContainer = new StudentDisciplineMetric(111, "Student - Non-State Offenses");
        public static readonly StudentDisciplineMetric StudentDisciplineNonStateOffensesLastX = new StudentDisciplineMetric(112, "Student - Non-State Offenses (Last {0})");
        public static readonly StudentDisciplineMetric StudentDisciplineNonStateOffensesLastY = new StudentDisciplineMetric(113, "Student - Non-State Offenses (Last {0})");
        public static readonly StudentDisciplineMetric StudentDisciplineNonStateOffensesYTD = new StudentDisciplineMetric(114, "Student - Non-State Offenses (YTD)");

        public static readonly StudentAssessmentMetric StudentStateAssessmentElaReading = new StudentAssessmentMetric(16, "Student - State Assessment - ELA / Reading", 0);
        public static readonly StudentAssessmentMetric StudentStateAssessmentMathematics = new StudentAssessmentMetric(17, "Student - State Assessment - Mathematics", 0);
        public static readonly StudentAssessmentMetric StudentStateAssessmentScience = new StudentAssessmentMetric(19, "Student - State Assessment - Science", 0);
        public static readonly StudentAssessmentMetric StudentStateAssessmentSocialStudies = new StudentAssessmentMetric(20, "Student - State Assessment - Social Studies", 0);

        public static readonly StudentAssessmentMetric StudentLanguageAssessmentProficiency = new StudentAssessmentMetric(1008, "Student - Language Assessment Proficiency", 0);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentReadingAssessment = new StudentAssessmentMetric(1083, "Student - Reading Assessment", 0);

        public static readonly StudentAssessmentMetric StudentLocalAssessmentBenchmarkAssessmentMasteryWriting = new StudentAssessmentMetric(1042, "Student - Local Assessment(Benchmark Assessment Mastery) - Writing", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentBenchmarkAssessmentMasteryElaReading = new StudentAssessmentMetric(74, "Student - Local Assessment(Benchmark Assessment Mastery) - ELA / Reading", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentBenchmarkAssessmentMasteryMathematics = new StudentAssessmentMetric(75, "Student - Local Assessment(Benchmark Assessment Mastery) - Mathematics", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentBenchmarkAssessmentMasteryScience = new StudentAssessmentMetric(76, "Student - Local Assessment(Benchmark Assessment Mastery) - Science", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentBenchmarkAssessmentMasterySocialStudies = new StudentAssessmentMetric(80, "Student - Local Assessment(Benchmark Assessment Mastery) - Social Studies", .7m);

        public static readonly StudentAssessmentMetric StudentLocalAssessmentLearningStandardMasteryElaReading = new StudentAssessmentMetric(1232, "Student - Local Assessment(Learning Standard Mastery) - ELA / Reading", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentLearningStandardMasteryMathematics = new StudentAssessmentMetric(1238, "Student - Local Assessment(Learning Standard Mastery) - Mathematics", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentLearningStandardMasteryScience = new StudentAssessmentMetric(1240, "Student - Local Assessment(Learning Standard Mastery) - Science", .7m);
        public static readonly StudentAssessmentMetric StudentLocalAssessmentLearningStandardMasterySocialStudies = new StudentAssessmentMetric(1244, "Student - Local Assessment(Learning Standard Mastery) - Social Studies", .7m);

        public static readonly CourseGradeMetric StudentGradesBelowCLevel = new CourseGradeMetric(26, "Student Grades Below C Level", 0);
        public static readonly CourseGradeMetric StudentAlgebraIPassingOrHasPassed = new CourseGradeMetric(29, "Student Algebra I Passing or Has Passed", 0);
        public static readonly CourseGradeMetric StudentAlgebraITakenOrEnrolled = new CourseGradeMetric(28, "Student Algebra I Taken or Enrolled", 0);
        public static readonly CourseGradeMetric StudentAlgebraI = new CourseGradeMetric(27, "Student Algebra I", 0);
        public static readonly CourseGradeMetric StudentCourseGradesDropping = new CourseGradeMetric(25, "Student Course Grades Dropping", 0);
        public static readonly CourseGradeMetric StudentFailingGrades = new CourseGradeMetric(24, "Student Failing Grades", 0);
        public static readonly CourseGradeMetric StudentClassGrades = new CourseGradeMetric(23, "Student Class Grades", 1);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGrades = new CourseGradeMetric(1491, "Student Failing Subject Area Course Grades", 0);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGradesReadingEla = new CourseGradeMetric(1492, "Student Failing Subject Area Course Grades - Reading/ELA", 0);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGradesWriting = new CourseGradeMetric(1493, "Student Failing Subject Area Course Grades - Writing", 0);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGradesMathematics = new CourseGradeMetric(1494, "Student Failing Subject Area Course Grades - Mathematics", 0);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGradesScience = new CourseGradeMetric(1495, "Student Failing Subject Area Course Grades - Science", 0);
        public static readonly CourseGradeMetric StudentFailingSubjectAreaCourseGradesSocialStudies = new CourseGradeMetric(1496, "Student Failing Subject Area Course Grades - Social Studies", 0);

        public static readonly ContainerMetric StudentCreditAccumulationContainer = new ContainerMetric(2379, "Student Credit Accumulation");
        public static readonly CourseGradeMetric StudentCreditAccumulationTotal = new CourseGradeMetric(2366, "Total Credit Accumulation", .25m);
        public static readonly CourseGradeMetric StudentCreditAccumulationNinthGrade = new CourseGradeMetric(2367, "Student Credit Accumulation Ninth Grade", StudentCreditAccumulationTotal.DefaultGoal);
        public static readonly CourseGradeMetric StudentCreditAccumulationTenthGrade = new CourseGradeMetric(2368, "Student Credit Accumulation Tenth Grade", StudentCreditAccumulationTotal.DefaultGoal);
        public static readonly CourseGradeMetric StudentCreditAccumulationEleventhGrade = new CourseGradeMetric(2369, "Student Credit Accumulation Eleventh Grade", StudentCreditAccumulationTotal.DefaultGoal);
        public static readonly CourseGradeMetric StudentCreditAccumulationTwelfthGrade = new CourseGradeMetric(2370, "Student Credit Accumulation Twelfth Grade", StudentCreditAccumulationTotal.DefaultGoal);
        public static readonly CreditAccumulation LocalEducationAgencyCreditAccumulation = new CreditAccumulation(1315, "Local Education Agency Credit Accumulation", .90m, RateDirection.OneToZero);

        public static readonly ContainerMetric StudentOnTrackToGraduateContainer = new ContainerMetric(2381, "Student On Track To Graduate Container");
        public static readonly CourseCreditMetric StudentOnTrackToGraduate = new CourseCreditMetric(2377, "Student On Track To Graduate", .25m);
        public static readonly ContainerMetric SchoolOnTrackToGraduateContainer = new ContainerMetric(288, "School On Track To Graduate Container");
        public static readonly OnTrackToGraduateMetric SchoolOnTrackToGraduate10thGrade = new OnTrackToGraduateMetric(417, "School On Track To Graduate 10th Grade", .800m, RateDirection.OneToZero);
        public static readonly OnTrackToGraduateMetric SchoolOnTrackToGraduate11thGrade = new OnTrackToGraduateMetric(418, "School On Track To Graduate 11th Grade", .800m, RateDirection.OneToZero);
        public static readonly OnTrackToGraduateMetric SchoolOnTrackToGraduate12thGrade = new OnTrackToGraduateMetric(419, "School On Track To Graduate 12th Grade", .800m, RateDirection.OneToZero);
        public static readonly OnTrackToGraduateMetric LocalEducationAgencyOnTrackToGraduate = new OnTrackToGraduateMetric(1316, "LocalEducationAgency On Track To Graduate", .900m, RateDirection.OneToZero);

        public static readonly CourseCreditMetric StudentAdvancedCoursePotential = new CourseCreditMetric(1285, "Student Advanced Course Potential", 80m);
        public static readonly CourseCreditMetric StudentAdvancedCoursePotentialEla = new CourseCreditMetric(1472, "Student Advanced Course Potential English Language Arts", 80m);
        public static readonly CourseCreditMetric StudentAdvancedCoursePotentialMathematics = new CourseCreditMetric(1474, "Student Advanced Course Potential Mathematics", 80m);
        public static readonly CourseCreditMetric StudentAdvancedCoursePotentialScience = new CourseCreditMetric(1476, "Student Advanced Course Potential Science", 80m);
        public static readonly CourseCreditMetric StudentAdvancedCoursePotentialSocialStudies = new CourseCreditMetric(1478, "Student Advanced Course Potential Social Studies", 80m);
        public static readonly ContainerMetric SchoolAdvancedCoursePotentialHighSchoolContainer = new ContainerMetric(2383, "School Advanced Course Potential High School Container");
        public static readonly ContainerMetric SchoolAdvancedCoursePotentialMiddleSchoolContainer = new ContainerMetric(1106, "School Advanced Course Potential Middle School Container");
        public static readonly AdvancedCoursePotentialMetric SchoolAdvancedCoursePotentialEla = new AdvancedCoursePotentialMetric(2384, "School Advanced Course Potential ELA", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric SchoolAdvancedCoursePotentialMathematics = new AdvancedCoursePotentialMetric(2385, "School Advanced Course Potential Mathematics", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric SchoolAdvancedCoursePotentialScience = new AdvancedCoursePotentialMetric(2386, "School Advanced Course Potential Science", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric SchoolAdvancedCoursePotentialSocialStudies = new AdvancedCoursePotentialMetric(2387, "School Advanced Course Potential Social Studies", .600m, RateDirection.OneToZero);

        public static readonly ContainerMetric LocalEducationAgencyAdvancedCourseContainer = new ContainerMetric(2388, "Local Education Agency Advanced Course Container Metric");
        public static readonly AdvancedCoursePotentialMetric LocalEducationAgencyAdvancedCoursePotentialELA = new AdvancedCoursePotentialMetric(2389, "Local Education Agency Advanced Course Potential ELA", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric LocalEducationAgencyAdvancedCoursePotentialMathematics = new AdvancedCoursePotentialMetric(2390, "Local Education Agency Advanced Course Potential Mathematics", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric LocalEducationAgencyAdvancedCoursePotentialScience = new AdvancedCoursePotentialMetric(2391, "Local Education Agency Advanced Course Potential Science", .600m, RateDirection.OneToZero);
        public static readonly AdvancedCoursePotentialMetric LocalEducationAgencyAdvancedCoursePotentialSocialStudies = new AdvancedCoursePotentialMetric(2392, "Local Education Agency Advanced Course Potential Social Studies", .600m, RateDirection.OneToZero);

        public static readonly StudentAdvancedCourseMasteryAssessmentMetric StudentAdvancedCourseMasteryContainer = new StudentAdvancedCourseMasteryAssessmentMetric(1286, "Advanced Course Enrollment, Completion and Mastery - Prior Years");
        public static readonly StudentAdvancedCourseMasteryAssessmentMetric StudentAdvancedCourseMasteryAssessmentELAMetric = new StudentAdvancedCourseMasteryAssessmentMetric(1473, "Student - Advanced Course Enrollment, Completion and Mastery - Prior Years (ELA/Reading)");
        public static readonly StudentAdvancedCourseMasteryAssessmentMetric StudentAdvancedCourseMasteryAssessmentMathematicsMetric = new StudentAdvancedCourseMasteryAssessmentMetric(1475, "Student - Advanced Course Enrollment, Completion and Mastery - Prior Years (Mathematics)");
        public static readonly StudentAdvancedCourseMasteryAssessmentMetric StudentAdvancedCourseMasteryAssessmentScienceMetric = new StudentAdvancedCourseMasteryAssessmentMetric(1477, "Student - Advanced Course Enrollment, Completion and Mastery - Prior Years (Science)");
        public static readonly StudentAdvancedCourseMasteryAssessmentMetric StudentAdvancedCourseMasteryAssessmentSocialStudiesMetric = new StudentAdvancedCourseMasteryAssessmentMetric(1479, "Student - Advanced Course Enrollment, Completion and Mastery - Prior Years (Social Studies)");

        public static readonly CollegeEntranceExamsPSAT StudentPSAT = new CollegeEntranceExamsPSAT(46, "Student PSAT", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT StudentPSATTaken = new CollegeEntranceExamsPSAT(101, "Student PSAT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT StudentPSATAtOrAbove = new CollegeEntranceExamsPSAT(102, "Student PSAT At Or Above", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolPSATTaken = new CollegeEntranceExamsPSAT(325, "School PSAT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolPSATNinthGradeTaken = new CollegeEntranceExamsPSAT(1269, "School PSAT 9th Grade Taken", .10m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolPSATTenthGradeTaken = new CollegeEntranceExamsPSAT(1270, "School PSAT 10th Grade Taken", .50m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolPSATEleventhGradeTaken = new CollegeEntranceExamsPSAT(1271, "School PSAT 11th Grade Taken", .60m, RateDirection.OneToZero);

        public static readonly CollegeEntranceExamsSAT StudentSAT = new CollegeEntranceExamsSAT(48, "Student SAT", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsACT StudentACT = new CollegeEntranceExamsACT(52, "Student ACT", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsSAT StudentSATTaken = new CollegeEntranceExamsSAT(49, "Student SAT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsACT StudentACTTaken = new CollegeEntranceExamsACT(53, "Student ACT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsSAT StudentSATAtOrAbove = new CollegeEntranceExamsSAT(50, "Student SAT At Or Above", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsACT StudentACTAtOrAbove = new CollegeEntranceExamsACT(54, "Student ACT At Or Above", 0m, RateDirection.OneToZero);

        public static readonly CollegeEntranceExamsPSAT SchoolACTorSATTaken = new CollegeEntranceExamsPSAT(499, "School ACT or SAT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolACTorSATNinthGradeTaken = new CollegeEntranceExamsPSAT(500, "School ACT or SAT 9th Grade Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolACTorSATTenthGradeTaken = new CollegeEntranceExamsPSAT(501, "School ACT or SAT 10th Grade Taken", .05m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolACTorSATEleventhGradeTaken = new CollegeEntranceExamsPSAT(502, "School ACT or SAT 11th Grade Taken", .40m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolACTorSATTwelfthGradeTaken = new CollegeEntranceExamsPSAT(503, "School ACT or SAT 12th Grade Taken", .60m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(1226, "School At or Above Benchmark", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolACTAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(505, "School At or Above Benchmark ACT", .90m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT SchoolSATAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(504, "School At or Above Benchmark SAT", .90m, RateDirection.OneToZero);

        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTorSATTaken = new CollegeEntranceExamsPSAT(1383, "Local Education Agency ACT or SAT Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTorSATNinthGradeTaken = new CollegeEntranceExamsPSAT(1427, "Local Education Agency ACT or SAT 9th Grade Taken", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTorSATTenthGradeTaken = new CollegeEntranceExamsPSAT(1434, "Local Education Agency ACT or SAT 10th Grade Taken", .05m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTorSATEleventhGradeTaken = new CollegeEntranceExamsPSAT(1435, "Local Education Agency ACT or SAT 11th Grade Taken", .40m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTorSATTwelfthGradeTaken = new CollegeEntranceExamsPSAT(1436, "Local Education Agency ACT or SAT 12th Grade Taken", .60m, RateDirection.OneToZero);

        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(1384, "Local Education Agency At or Above Benchmark", 0m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencyACTAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(1432, "Local Education Agency At or Above Benchmark ACT", .90m, RateDirection.OneToZero);
        public static readonly CollegeEntranceExamsPSAT LocalEducationAgencySATAtOrAboveBenchmark = new CollegeEntranceExamsPSAT(1433, "Local Education Agency At or Above Benchmark SAT", .90m, RateDirection.OneToZero);

        public static readonly Metric SchoolTardyRate = new AttendanceMetric(1130, "School Tardy Rate", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolTardyRateLastXDays = new AttendanceMetric(1132, "School Tardy Rate Last X Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolTardyRateLastYDays = new AttendanceMetric(1133, "School Tardy Rate Last Y Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolTardyRateYearToDate = new AttendanceMetric(1134, "School Tardy Rate Year To Date", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolTotalDaysAbsent = new AttendanceMetric(1484, "School Total Days Absent", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolDaysAbsent = new ContainerMetric(1672, "School Days Absent");
        public static readonly Metric SchoolTotalUnexcusedDaysAbsent = new AttendanceMetric(1673, "School Total Unexcused Days Absent", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolDailyAttendanceRate = new AttendanceMetric(211, "School Daily Attendance Rate", .95m, RateDirection.OneToZero);
        public static readonly Metric SchoolDailyAttendanceRateLastXDays = new AttendanceMetric(212, "School Daily Attendance Rate Last X Days", .95m, RateDirection.OneToZero);
        public static readonly Metric SchoolDailyAttendanceRateLastYDays = new AttendanceMetric(213, "School Daily Attendance Rate Last Y Days", .95m, RateDirection.OneToZero);
        public static readonly Metric SchoolDailyAttendanceRateYearToDate = new AttendanceMetric(214, "School Daily Attendance Rate Year To Date", .95m, RateDirection.OneToZero);
        public static readonly Metric SchoolClassPeriodAbsenceRate = new AttendanceMetric(216, "School Class Period Absence Rate", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolClassPeriodAbsenceRateLastXDays = new AttendanceMetric(217, "School Class Period Absence Rate Last X Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolClassPeriodAbsenceRateLastYDays = new AttendanceMetric(218, "School Class Period Absence Rate Last Y Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolClassPeriodAbsenceRateYearToDate = new AttendanceMetric(219, "School Class Period Absence Rate Year To Date", .1m, RateDirection.ZeroToOne);
        public static readonly Metric SchoolAverageDailyAttendance = new AttendanceMetric(1444, "School Average Daily Attendance", .9m, RateDirection.OneToZero);
        public static readonly Metric SchoolAverageDailyAttendanceRateLastXDays = new AttendanceMetric(1445, "School Average Daily Attendance Rate Last X Days", .90m, RateDirection.OneToZero);
        public static readonly Metric SchoolAverageDailyAttendanceRateLastYDays = new AttendanceMetric(1448, "School Average Daily Attendance Rate Last Y Days", .90m, RateDirection.OneToZero);
        public static readonly Metric SchoolAverageDailyAttendanceRateYearToDate = new AttendanceMetric(1446, "School Average Daily Attendance Rate Year To Date", .90m, RateDirection.OneToZero);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGrades = new SchoolCourseGradeMetric(1062, "School Failing Subject Areas", .1m);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGradesMathematics = new SchoolCourseGradeMetric(1065, "School Failing Subject Area Mathematics", .1m);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGradesSocialStudies = new SchoolCourseGradeMetric(1067, "School Failing Subject Area Social Studies", .1m);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGradesELAReading = new SchoolCourseGradeMetric(1063, "School Failing Subject Area ELA/Reading", .1m);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGradesScience = new SchoolCourseGradeMetric(1066, "School Failing Subject Area Science", .1m);
        public static readonly SchoolCourseGradeMetric SchoolFailingSubjectAreaCourseGradesWriting = new SchoolCourseGradeMetric(1064, "School Failing Subject Area Writing", .1m);
        public static readonly SchoolCourseGradeMetric SchoolCourseGradesDroppingBySubjectArea = new SchoolCourseGradeMetric(266, "School Course Grades Dropping By Subject Areas", .15m);
        public static readonly SchoolCourseGradeMetric SchoolCourseGradesDroppingMathematics = new SchoolCourseGradeMetric(268, "School Course Grades Dropping Mathematics", .15m);
        public static readonly SchoolCourseGradeMetric SchoolCourseGradesDroppingSocialStudies = new SchoolCourseGradeMetric(414, "School Course Grades Dropping Social Studies", .15m);
        public static readonly SchoolCourseGradeMetric SchoolCourseGradesDroppingElaReading = new SchoolCourseGradeMetric(267, "School Course Grades Dropping ELA/Reading", .15m);
        public static readonly SchoolCourseGradeMetric SchoolCourseGradesDroppingScience = new SchoolCourseGradeMetric(269, "School Course Grades Dropping Science", .15m);
        public static readonly SchoolCourseGradeMetric SchoolGradesBelowProficiencyCourseGrades = new SchoolCourseGradeMetric(262, "School Grades Below Proficiency Level", .4m);
        public static readonly SchoolCourseGradeMetric SchoolGradesBelowProficiencyOneOrMoreCourseGrades = new SchoolCourseGradeMetric(263, "School Grades With One or More Below Proficiency Level", .4m);
        public static readonly SchoolCourseGradeMetric SchoolGradesBelowProficiencyTwoOrMoreCourseGrades = new SchoolCourseGradeMetric(264, "School Grades With Two or More Below Proficiency Level", .3m);
        public static readonly SchoolCourseGradeMetric SchoolGradesBelowProficiencyThreeOrMoreCourseGrades = new SchoolCourseGradeMetric(265, "School Grades With Three or More Below Proficiency Level", .1m);
        public static readonly SchoolCourseGradeMetric AlgebraIHighSchool = new SchoolCourseGradeMetric(270, "Algebra I High School", .75m);
        public static readonly SchoolCourseGradeMetric AlgebraIHighSchoolTakenOrHaveTaken = new SchoolCourseGradeMetric(2331, "Algebra I High School Taking or Have Taken", .75m);
        public static readonly SchoolCourseGradeMetric AlgebraIHighSchoolPassingOrHavePassed = new SchoolCourseGradeMetric(2332, "Algebra I High School Passing or Have Passed", .75m);
        public static readonly SchoolCourseGradeMetric AlgebraIMiddleSchool = new SchoolCourseGradeMetric(1283, "Algebra I Middle School", .75m);
        public static readonly SchoolCourseGradeMetric AlgebraIMiddleSchoolTakenOrHaveTaken = new SchoolCourseGradeMetric(1442, "Algebra I Middle School Taking or Have Taken", .75m);
        public static readonly SchoolCourseGradeMetric AlgebraIMiddleSchoolPassingOrHavePassed = new SchoolCourseGradeMetric(1443, "Algebra I Middle School Passing or Have Passed", .75m);

        public static readonly SchoolDisciplineMetric SchoolDisciplineNonStateOffenses = new SchoolDisciplineMetric(2354, "School Discipline Non-State Offenses", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineNonStateOffensesLastX = new SchoolDisciplineMetric(2355, "School Discipline Non-State Offenses (Last X)", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineNonStateOffensesLastY = new SchoolDisciplineMetric(2356, "School Discipline Non-State Offenses (Last Y)", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineNonStateOffensesYTD = new SchoolDisciplineMetric(2357, "Year to Date School Discipline Non-State Offenses", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineStateOffenses = new SchoolDisciplineMetric(2350, "School Discipline State Offenses", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineStateOffensesLastX = new SchoolDisciplineMetric(2351, "School Discipline State Offenses (Last X)", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineStateOffensesLastY = new SchoolDisciplineMetric(2352, "School Discipline State Offenses (Last Y)", .05m);
        public static readonly SchoolDisciplineMetric SchoolDisciplineStateOffensesYTD = new SchoolDisciplineMetric(2353, "Year to Date School Discipline State Offenses", .05m);

        public static readonly Metric SchoolTeacherEducation = new TeacherEducationMetric(1069, "School Teacher Education", .90m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherEducationBachelorsOrHigher = new TeacherEducationMetric(1070, "School Teacher Education Bachelors or Higher", .90m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherEducationMastersOrDoctorate = new TeacherEducationMetric(1071, "School Teacher Education Masters or Doctorate", .30m, RateDirection.OneToZero);

        public static readonly Metric SchoolTeacherAttendanceRate = new AttendanceMetric(361, "School Teacher Attendance Rate", 1m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherAttendanceRateLastXDays = new AttendanceMetric(1032, "School Teacher Attendance Rate Last X Days", 1m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherAttendanceRateLastYDays = new AttendanceMetric(1033, "School Teacher Attendance Rate Last Y Days", 1m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherAttendanceRateYearToDate = new AttendanceMetric(1034, "School Teacher Attendance Rate Year To Date", 1m, RateDirection.OneToZero);
        public static readonly Metric TeacherAttendanceThreshold = new AttendanceMetric(2380, "Teacher Attendance Threshold", .90m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyTeacherAttendanceRateContainer = new AttendanceMetric(1297, "Local Education Agency Teacher Attendance Rate Container", 1m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherAttendanceRateLastXDays = new AttendanceMetric(1298, "Local Education Agency Teacher Attendance Rate Last X Days", 1m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherAttendanceRateLastYDays = new AttendanceMetric(2378, "Local Education Agency Teacher Attendance Rate Last Y Days", 1m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherAttendanceRateYearToDate = new AttendanceMetric(1299, "Local Education Agency Teacher Attendance Rate Year To Date", 1m, RateDirection.OneToZero);

        public static readonly Metric SchoolTeacherRetention = new TeacherRetentionMetric(375, "School Teacher Retention", .80m, RateDirection.OneToZero);

        public static readonly Metric SchoolTeacherExperience = new TeacherExperienceMetric(369, "School Teacher Experience", .60m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherExperienceAllSubjects = new TeacherExperienceMetric(370, "School Teacher Experience All Subjects", .60m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherExperienceElaReading = new TeacherExperienceMetric(371, "School Teacher Experience ELA/Reading", .60m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherExperienceMathematics = new TeacherExperienceMetric(372, "School Teacher Experience Mathematics", .60m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherExperienceScience = new TeacherExperienceMetric(424, "School Teacher Experience Science", .60m, RateDirection.OneToZero);
        public static readonly Metric SchoolTeacherExperienceSocialStudies = new TeacherExperienceMetric(425, "School Teacher Experience Social Studies", .60m, RateDirection.OneToZero);
        public static readonly Metric TeacherExperience = new TeacherExperienceMetric(2382, "Teacher Experience", 6, RateDirection.OneToZero);

        public static readonly AdvancedCourseEnrollment SchoolAdvancedCourseEnrollment = new AdvancedCourseEnrollment(301, "School Advanced Course Enrollment", .60m, RateDirection.OneToZero);
        public static readonly AdvancedCourseCompletion SchoolAdvancedCourseCompletion = new AdvancedCourseCompletion(303, "School Advanced Course Completion", .75m, RateDirection.OneToZero);

        public static readonly Metric LocalEducaitonAgencyTardyRate = new ContainerMetric(1354, "LEA Tardy Rate");
        public static readonly Metric LocalEducationAgencyTardyRateLastXDays = new AttendanceMetric(1355, "LEA Tardy Rate Last X Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyTardyRateLastYDays = new AttendanceMetric(1343, "LEA Tardy Rate Last Y Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyTardyRateYearToDate = new AttendanceMetric(1356, "LEA Tardy Rate Year To Date", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyDaysAbsent = new ContainerMetric(1674, "LEA Days Absent");
        public static readonly Metric LocalEducationAgencyTotalDaysAbsent = new AttendanceMetric(1561, "LEA Total Days Absent", .1m, RateDirection.ZeroToOne);

        public static readonly CreditAccumulation SchoolCreditAccumulation = new CreditAccumulation(283, "School Credit Accumulation", .80m, RateDirection.OneToZero);
        public static readonly CreditAccumulation SchoolCreditAccumulationTenthGrade = new CreditAccumulation(285, "School Credit Accumulation Tenth Grade", .80m, RateDirection.OneToZero);
        public static readonly CreditAccumulation SchoolCreditAccumulationEleventhGrade = new CreditAccumulation(286, "School Credit Accumulation Eleventh Grade", .80m, RateDirection.OneToZero);
        public static readonly CreditAccumulation SchoolCreditAccumulationTwelfthGrade = new CreditAccumulation(287, "School Credit Accumulation Twelfth Grade", .80m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTotalUnexcusedDaysAbsent = new AttendanceMetric(1675, "LEA Total Unexcused Days Absent", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyDailyAttendanceRate = new ContainerMetric(1348, "LEA Daily Attendance Rate");
        public static readonly Metric LocalEducationAgencyDailyAttendanceRateLastXDays = new AttendanceMetric(1349, "LEA Daily Attendance Rate Last X Days", .95m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyDailyAttendanceRateLastYDays = new AttendanceMetric(1341, "LEA Daily Attendance Rate Last Y Days", .95m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyDailyAttendanceRateYearToDate = new AttendanceMetric(1350, "LEA Daily Attendance Rate Year To Date", .95m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyClassPeriodAbsenceRate = new ContainerMetric(1351, "LEA Class Period Absence Rate");
        public static readonly Metric LocalEducationAgencyClassPeriodAbsenceRateLastXDays = new AttendanceMetric(1352, "LEA Class Period Absence Rate Last X Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyClassPeriodAbsenceRateLastYDays = new AttendanceMetric(1342, "LEA Class Period Absence Rate Last Y Days", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyClassPeriodAbsenceRateYearToDate = new AttendanceMetric(1353, "LEA Class Period Absence Rate Year To Date", .1m, RateDirection.ZeroToOne);
        public static readonly Metric LocalEducationAgencyAverageDailyAttendance = new ContainerMetric(1345, "LEA Average Daily Attendance");
        public static readonly Metric LocalEducationAgencyAverageDailyAttendanceRateLastXDays = new AttendanceMetric(1346, "LEA Average Daily Attendance Rate Last X Days", .90m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyAverageDailyAttendanceRateLastYDays = new AttendanceMetric(1311, "LEA Average Daily Attendance Rate Last Y Days", .90m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyAverageDailyAttendanceRateYearToDate = new AttendanceMetric(1347, "LEA Average Daily Attendance Rate Year To Date", .90m, RateDirection.OneToZero);

        public static readonly SchoolCourseGradeMetric LocalEducationAgencyGradesBelowProficiencyTwoOrMoreCourseGrades = new SchoolCourseGradeMetric(1303, "Local Education Agency Grades With Two or More Below Proficiency Level", .3m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyCourseGradesDroppingBySubjectArea = new SchoolCourseGradeMetric(1304, "Local Education Agency Course Grades Dropping By Subject Areas", .15m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyCourseGradesDroppingMathematics = new SchoolCourseGradeMetric(1412, "Local Education Agency Course Grades Dropping Mathematics", .15m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyCourseGradesDroppingSocialStudies = new SchoolCourseGradeMetric(1414, "Local Education Agency Course Grades Dropping Social Studies", .15m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyCourseGradesDroppingElaReading = new SchoolCourseGradeMetric(1411, "Local Education Agency Course Grades Dropping ELA/Reading", .15m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyCourseGradesDroppingScience = new SchoolCourseGradeMetric(1413, "Local Education Agency Course Grades Dropping Science", .15m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingBySubjectArea = new SchoolCourseGradeMetric(1305, "Local Education Agency Failing By Subject Areas", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingSubjectAreaMathematics = new SchoolCourseGradeMetric(1417, "Local Education Agency Failing Subject Area Mathematics", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingSubjectAreaSocialStudies = new SchoolCourseGradeMetric(1419, "Local Education Agency Failing Subject Area Social Studies", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingSubjectAreaElaReading = new SchoolCourseGradeMetric(1415, "Local Education Agency Failing Subject Area ELA/Reading", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingSubjectAreaScience = new SchoolCourseGradeMetric(1418, "Local Education Agency Failing Subject Area Science", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyFailingSubjectAreaWriting = new SchoolCourseGradeMetric(1416, "Local Education Agency Failing Subject Area Writing", .1m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyMiddleSchoolAlgebraI = new SchoolCourseGradeMetric(1676, "Local Education Agency Middle School Algebra I", .75m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyMiddleSchoolAlgebraITakingOrHaveTaken = new SchoolCourseGradeMetric(1309, "Local Education Agency Middle School Taking Or Have Taken", .75m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyMiddleSchoolAlgebraIPassingOrHavePassed = new SchoolCourseGradeMetric(1310, "Local Education Agency Middle School Passing Or Have Passed", .75m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyHighSchoolAlgebraI = new SchoolCourseGradeMetric(1307, "Local Education Agency High School Algebra I", .75m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyHighSchoolAlgebraITakingOrHaveTaken = new SchoolCourseGradeMetric(1312, "Local Education Agency High School Taking Or Have Taken", .75m);
        public static readonly SchoolCourseGradeMetric LocalEducationAgencyHighSchoolAlgebraIPassingOrHavePassed = new SchoolCourseGradeMetric(1313, "Local Education Agency High School Passing Or Have Passed", .75m);

        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineNonStateOffenses = new LocalEducationAgencyDisciplineMetric(2362, "Local Education Agency Discipline Non-State Offenses", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineNonStateOffensesLastX = new LocalEducationAgencyDisciplineMetric(2363, "Local Education Agency Discipline Non-State Offenses (Last X)", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineNonStateOffensesLastY = new LocalEducationAgencyDisciplineMetric(2364, "Local Education Agency Discipline Non-State Offenses (Last Y)", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineNonStateOffensesYTD = new LocalEducationAgencyDisciplineMetric(2365, "Year to Date Local Education Agency Discipline Non-State Offenses", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineStateOffenses = new LocalEducationAgencyDisciplineMetric(2358, "Local Education Agency Discipline State Offenses", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineStateOffensesLastX = new LocalEducationAgencyDisciplineMetric(2359, "Local Education Agency Discipline State Offenses (Last X)", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineStateOffensesLastY = new LocalEducationAgencyDisciplineMetric(2360, "Local Education Agency Discipline State Offenses (Last Y)", .05m);
        public static readonly LocalEducationAgencyDisciplineMetric LocalEducationAgencyDisciplineStateOffensesYTD = new LocalEducationAgencyDisciplineMetric(2361, "Year to Date Local Education Agency Discipline State Offenses", .05m);

        public static readonly Metric LocalEducationAgencyTeacherEducation = new TeacherEducationMetric(1391, "LocalEducationAgency Teacher Education", .90m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherEducationBachelorsOrHigher = new TeacherEducationMetric(1392, "LocalEducationAgency Teacher Education Bachelors or Higher", .90m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherEducationMastersOrDoctorate = new TeacherEducationMetric(1393, "LocalEducationAgency Teacher Education Masters or Doctorate", .30m, RateDirection.OneToZero);

        public static readonly Metric LocalEducationAgencyTeacherExperience = new LocalEducationAgencyTeacherExperienceMetric(1389, "LocalEducationAgency Teacher Experience", .60m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherExperienceAllSubjectAreas = new LocalEducationAgencyTeacherExperienceMetric(1462, "LocalEducationAgency Teacher Experience All Subject Areas", .60m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherExperienceElaReading = new LocalEducationAgencyTeacherExperienceMetric(1463, "LocalEducationAgency Teacher Experience ELA/Reading", .60m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherExperienceMathematics = new LocalEducationAgencyTeacherExperienceMetric(1464, "LocalEducationAgency Teacher Experience Mathematics", .60m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherExperienceScience = new LocalEducationAgencyTeacherExperienceMetric(1465, "LocalEducationAgency Teacher Experience Science", .60m, RateDirection.OneToZero);
        public static readonly Metric LocalEducationAgencyTeacherExperienceSocialStudies = new LocalEducationAgencyTeacherExperienceMetric(1466, "LocalEducationAgency Teacher Experience Social Studies", .60m, RateDirection.OneToZero);

        public static readonly Metric LocalEducationAgencyTeacherRetention = new TeacherRetentionMetric(1401, "LocalEducationAgency Teacher Retention", .80m, RateDirection.OneToZero);

        public static readonly AdvancedCourseEnrollment LocalEducationAgencyAdvancedCourseEnrollment = new AdvancedCourseEnrollment(1373, "District Advanced Course Enrollment", .60m, RateDirection.OneToZero);
        public static readonly AdvancedCourseEnrollment LocalEducationAgencyAdvancedCourseEnrollmentMiddleSchool = new AdvancedCourseEnrollment(1454, "District Advanced Course Enrollment Middle School", .60m, RateDirection.OneToZero);
        public static readonly AdvancedCourseEnrollment LocalEducationAgencyAdvancedCourseEnrollmentHighSchool = new AdvancedCourseEnrollment(1455, "District Advanced Course Enrollment High School", .60m, RateDirection.OneToZero);

        public static readonly AdvancedCourseCompletion LocalEducationAgencyAdvancedCourseCompletion = new AdvancedCourseCompletion(1374, "District Advanced Course Completion", .75m, RateDirection.OneToZero);
        public static readonly AdvancedCourseCompletion LocalEducationAgencyAdvancedCourseCompletionMiddleSchool = new AdvancedCourseCompletion(1456, "District Advanced Course Completion Middle School", .75m, RateDirection.OneToZero);
        public static readonly AdvancedCourseCompletion LocalEducationAgencyAdvancedCourseCompletionHighSchool = new AdvancedCourseCompletion(1457, "District Advanced Course Completion High School", .75m, RateDirection.OneToZero);

        public static readonly Metric LocalEducationAgencyPrincipalRetention = new TeacherRetentionMetric(1468, "Local Education Agency Principal Retention", .80m, RateDirection.OneToZero);
        public static readonly Metric PrincipalRetention = new TeacherRetentionMetric(1469, "Principal Retention", 1.0m, RateDirection.OneToZero);

        public abstract decimal DefaultGoal { get; set; }

        public abstract RateDirection RateDirection { get; set; }

        private Metric(int value, string displayName) : base(value, displayName)
        {
            OriginalMetricId = value;
            OverriddenByMetric = null;
        }

        private Metric(int value, string displayName, Metric overriddenByMetric) : base(value, displayName)
        {
            OriginalMetricId = value;

            if (overriddenByMetric == null)
            {
                throw new Exception(string.Format("Metric Override Error: Metric ID {0} ({1}): Attempted to override metric ID with another metric that does not yet exist. Ensure that the metric override is initialized first.", value, displayName));
            }

            if (OriginalMetricId == overriddenByMetric.OriginalMetricId || OriginalMetricId == overriddenByMetric.Value)
            {
                throw new Exception(string.Format("Metric Override Error: Metric ID {0} ({1}): Circular reference", value, displayName));
            }

            OverriddenByMetric = overriddenByMetric;
            SetValue(overriddenByMetric.Id);
        }

        public class ContainerMetric : Metric
        {
            public ContainerMetric(int value, string name)
                : base(value, name)
            { }

            public override decimal DefaultGoal
            {
                get
                {
                    throw new Exception("Container metric does not have a Default Goal to get");
                }
                set
                {
                    throw new Exception("Container metric does not have a Default Goal to set");
                }
            }

            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("Container metric does not have a Rate Direction to get");
                }
                set
                {
                    throw new Exception("Container metric does not have a Rate Direction to set");
                }
            }
        }
        public class StudentDisciplineMetric : Metric
        {
            public StudentDisciplineMetric(int value, string displayName)
                : base(value, displayName)
            {
                DefaultGoal = 0;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class StudentAdvancedCourseMasteryAssessmentMetric : Metric
        {
            public StudentAdvancedCourseMasteryAssessmentMetric(int value, string displayName)
                : base(value, displayName) { }

            public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection { get; set; }
        }

        public class StudentAssessmentMetric : Metric
        {
            public StudentAssessmentMetric(int value, string displayName, decimal defaultGoal)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection { get; set; }
        }

        public class AdvancedCoursePotentialMetric : Metric
        {
            public AdvancedCoursePotentialMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection { get; set; }
        }

        public class AdvancedCourseEnrollment : Metric
        {
            public AdvancedCourseEnrollment(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }
        public class OnTrackToGraduateMetric : Metric
        {
            public OnTrackToGraduateMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }
            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }
        public class AdvancedCourseCompletion : Metric
        {
            public AdvancedCourseCompletion(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class CollegeEntranceExamsSAT : Metric
        {
            public CollegeEntranceExamsSAT(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class CollegeEntranceExamsACT : Metric
        {
            public CollegeEntranceExamsACT(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class CollegeEntranceExamsPSAT : Metric
        {
            public CollegeEntranceExamsPSAT(int value, string displayName, decimal defaultGoal, RateDirection rateDirection) : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        /*TODO: Change Attendance Metric to public, pull in all methods that apply to calculating attendance trend and metric state into this class.
         * Change all attendance translators to use the AttendanceMetric subclass instead of the Metric base class.  
        */
        public class AttendanceMetric : Metric
        {
            public AttendanceMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class TeacherEducationMetric : Metric
        {
            public TeacherEducationMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class TeacherRetentionMetric : Metric
        {
            public TeacherRetentionMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class LocalEducationAgencyTeacherExperienceMetric : Metric
        {
            public LocalEducationAgencyTeacherExperienceMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class TeacherExperienceMetric : Metric
        {
            public TeacherExperienceMetric(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class CreditAccumulation : Metric
        {
            public CreditAccumulation(int value, string displayName, decimal defaultGoal, RateDirection rateDirection)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
                RateDirection = rateDirection;
            }

            sealed public override decimal DefaultGoal { get; set; }
            sealed public override RateDirection RateDirection { get; set; }
        }

        public class SchoolDisciplineMetric : Metric
        {
            public SchoolDisciplineMetric(int value, string displayName, decimal defaultGoal)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            sealed public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
                set
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
            }
        }

        public class LocalEducationAgencyDisciplineMetric : Metric
        {
            public LocalEducationAgencyDisciplineMetric(int value, string displayName, decimal defaultGoal)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            sealed public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
                set
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
            }
        }

        public class SchoolCourseGradeMetric : Metric
        {
            public SchoolCourseGradeMetric(int value, string displayName, decimal defaultGoal) : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            sealed public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
                set
                {
                    throw new Exception("School Course Grade metric does not utilize Rate Direction.");
                }
            }

            public MetricStateType GetMetricStateTypeForSchoolCourseGrades(decimal? metricValue, IMetricLimit metricLimit)
            {
                if (!metricValue.HasValue)
                {
                    return null;
                }

                var compareValue = GetGoal(metricLimit);
                return metricValue <= compareValue ? MetricStateType.Good : MetricStateType.Bad;
            }

            public MetricStateType GetMetricStateTypeForAlgebra(decimal? metricValue, IMetricLimit metricLimit)
            {
                if (!metricValue.HasValue)
                {
                    return null;
                }

                var compareValue = GetGoal(metricLimit);
                return metricValue >= compareValue ? MetricStateType.Good : MetricStateType.Bad;
            }

            public void GetSchoolCourseGradesTrend(decimal? maxGradingPeriodValue, decimal? previousGradingPeriodValue, out int? trend, out bool flag)
            {
                trend = null;
                flag = false;
                if (!maxGradingPeriodValue.HasValue || !previousGradingPeriodValue.HasValue)
                {
                    return;
                }

                if (maxGradingPeriodValue - previousGradingPeriodValue > 0.05m)
                {
                    trend = 1;
                }
                else if (previousGradingPeriodValue - maxGradingPeriodValue > 0.05m)
                {
                    trend = -1;
                }
                else
                {
                    trend = 0;
                }
            }
        }

        public class CourseGradeMetric : Metric
        {
            public CourseGradeMetric(int value, string displayName, decimal defaultGoal)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            sealed public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("Course Grade metric does not have a Rate Direction to get");
                }
                set
                {
                    throw new Exception("Course Grade metric does not have a Rate Direction to set");
                }
            }

            public MetricStateType GetMetricStateTypeForStudentCourseGrades(decimal? metricValue)
            {
                return metricValue == DefaultGoal ? MetricStateType.Good : MetricStateType.Bad;
            }

            public void GetTrendByStudentGrades(int maxGradingPeriodAtOrBelowThreshold, int previousAtOrBelowThreshold, int maxGradingPeriodTotalClasses, int previousTotalNumberOfClassesPerGradingPeriod, out int? trend, out bool flag)
            {
                flag = false;
                trend = null;

                if (maxGradingPeriodTotalClasses == 0 || previousTotalNumberOfClassesPerGradingPeriod == 0)
                {
                    return;
                }

                if (maxGradingPeriodAtOrBelowThreshold > previousAtOrBelowThreshold)
                {
                    trend = 1;
                }
                else if (maxGradingPeriodAtOrBelowThreshold < previousAtOrBelowThreshold)
                {
                    trend = -1;
                }
                else
                {
                    trend = 0;
                }
            }
        }

        public class CourseCreditMetric : Metric
        {
            public CourseCreditMetric(int value, string displayName, decimal defaultGoal)
                : base(value, displayName)
            {
                DefaultGoal = defaultGoal;
            }

            sealed public override decimal DefaultGoal { get; set; }
            public override RateDirection RateDirection
            {
                get
                {
                    throw new Exception("Credit metric does not have a Rate Direction to get");
                }
                set
                {
                    throw new Exception("Credit metric does not have a Rate Direction to set");
                }
            }
        }

        public int Id
        {
            get
            {
                return OverriddenByMetric != null ? OverriddenByMetric.Id : Value;
            }
        }

        protected int OriginalMetricId { get; set; }
        protected Metric OverriddenByMetric { get; set; }

        public MetricStateType GetMetricStateType(int? metricValue, IMetricLimit metricLimit)
        {
            return metricValue == null ? null : GetMetricStateType((decimal)metricValue.Value, metricLimit);
        }

        public MetricStateType GetMetricStateType(decimal? metricValue, IMetricLimit metricLimit)
        {
            if (!metricValue.HasValue)
            {
                return null;
            }

            var compareValue = (metricLimit != null) ? metricLimit.LimitValue : DefaultGoal;
            if (RateDirection == RateDirection.ZeroToOne)
            {
                return metricValue <= compareValue ? MetricStateType.Good : MetricStateType.Bad;
            }
            return metricValue >= compareValue ? MetricStateType.Good : MetricStateType.Bad;
        }

        public decimal GetGoal(IMetricLimit metricLimit)
        {
            return (metricLimit != null) ? metricLimit.LimitValue : DefaultGoal;
        }

        /// <summary>
        /// For calculating trend given a number of student absences for the current and prior reporting period.  The prior and current denominator must be equal for a 
        /// correct trend calculation. 
        /// </summary>
        public void GetTrendByAttendance(int firstPeriodTotal, int secondPeriodTotal, int firstPeriodAttendance, int secondPeriodAttendance, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal != secondPeriodTotal || firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            GetTrendByStudent(firstPeriodTotal, secondPeriodTotal, firstPeriodAttendance, secondPeriodAttendance, out trend, out flag);
        }

        /// <summary>
        /// For calculating trend given a (number of students who meet a goal or a sum of all students attendance rate) / (total number of students enrolled).  The prior and current denominator can
        /// be different since a student can not be enrolled in the prior period but enrolled in the current period.
        /// </summary>
        public void GetTrendByStudent(int firstPeriodTotal, int secondPeriodTotal, decimal firstPeriodAttendance, decimal secondPeriodAttendance, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            var firstPeriodRate = firstPeriodAttendance / firstPeriodTotal;
            var secondPeriodRate = secondPeriodAttendance / secondPeriodTotal;

            GetTrend(out trend, out flag, firstPeriodRate, secondPeriodRate);
        }

        public void GetTrend(out int? trend, out bool flag, decimal? firstPeriodRate, decimal? secondPeriodRate)
        {
            trend = null;
            flag = false;
            if (!firstPeriodRate.HasValue || !secondPeriodRate.HasValue)
            {
                return;
            }
            if (firstPeriodRate - secondPeriodRate > 0.05m)
            {
                trend = 1;
            }
            else if (secondPeriodRate - firstPeriodRate > 0.05m)
            {
                trend = -1;
                if (RateDirection == RateDirection.OneToZero && secondPeriodRate - firstPeriodRate > 0.1m)
                {
                    flag = true;
                }
            }
            else
            {
                trend = 0;
            }
        }

        public virtual void GetTrendDirection(decimal? currentValue, decimal? previousValue, out int? trend, out bool flag)
        {
            trend = null;
            flag = false;
            if (!currentValue.HasValue || !previousValue.HasValue)
            {
                return;
            }

            if (currentValue - previousValue > 0.05m)
            {
                trend = 1;
            }
            else if (previousValue - currentValue > 0.05m)
            {
                trend = -1;
            }
            else
            {
                trend = 0;
            }
        }

        public virtual void GetTrendDirectionWithoutThreshold(int? currentValue, int? previousValue, out int? trend)
        {
            if (!currentValue.HasValue || !previousValue.HasValue)
            {
                trend = null;
            }
            else if (previousValue > currentValue)
            {
                trend = -1;
            }
            else if (previousValue < currentValue)
            {
                trend = 1;
            }
            else
            {
                trend = 0;
            }
        }

        public MetricStateType GetMetricState(decimal? metricValue, IMetricLimit metricLimit)
        {
            if (!metricValue.HasValue)
            {
                return null;
            }

            var compareValue = GetGoal(metricLimit);
            return metricValue <= compareValue ? MetricStateType.Good : MetricStateType.Bad;
        }
    }
}
