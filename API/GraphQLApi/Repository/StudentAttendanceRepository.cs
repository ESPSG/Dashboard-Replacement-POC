using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Descriptors.Enumerations;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Metric = GraphQLApi.Common.Metric;

namespace GraphQLApi.Repository
{
    public class StudentAttendanceRepository : IStudentMetricCalculator
    {
        private readonly ODSContext _context;

        public StudentAttendanceRepository(ODSContext context)
        {
            _context = context;
        }

        public List<StudentMetric> GetStudentMetrics(string studentKey, string schoolKey)
        {
            List<StudentMetric> studentMetrics = new List<StudentMetric>();
            string format = "{0:P1}";
            var currentSchoolYearDim = _context.CurrentSchoolYearDims.FirstOrDefault();

            if (currentSchoolYearDim == null)
                return null;

            string studentKeyStr = studentKey + "";
            var studentSchoolAttendanceEventFactDbRecords = _context.StudentSchoolAttendanceEventFacts
                                                              .Where(student => student.StudentKey == studentKeyStr && student.SchoolYear == currentSchoolYearDim.SchoolYear)
                                                              .ToList();

            var studentSchoolAssociations = _context.StudentSchoolDims
                                                    .Where(s => s.StudentKey.Equals(studentKey.ToString()) && s.SchoolKey.Equals(schoolKey.ToString()))
                                                    .ToList();

            StudentSchoolAttendanceEventFact previousAttendanceEvent = null;
            var studentAttendanceEvents = new List<StudentAttendanceEvent>();
            foreach (var attendanceEvent in studentSchoolAttendanceEventFactDbRecords)
            {
                if (previousAttendanceEvent != null && previousAttendanceEvent.EventDate == attendanceEvent.EventDate)
                {
                    //Found multiple daily attendance events on the same day: {@attendanceEvent} AND {@duplicateAttendanceEvent}", previousAttendanceEvent, attendanceEvent);
                    continue;
                }

                studentAttendanceEvents.Add(new StudentAttendanceEvent
                {
                    DailyAttendanceCalculcationSource = DailyAttendanceCalculcationSource.BothAttendanceEventSources,
                    EventDate = attendanceEvent.EventDate,
                    AttendanceEventReason = attendanceEvent.AttendanceEventReason,
                    AttendanceEventCategoryDescriptor = attendanceEvent.AttendanceEventCategoryDescriptor
                });

                previousAttendanceEvent = attendanceEvent;
            }

            var studentSectionAssociations = _context.StudentSectionDims
                                                    .Where(s => s.StudentKey.Equals(studentKey.ToString()) && s.SchoolKey.Equals(schoolKey.ToString()))
                                                    .ToList();


            var studentSectionAttendanceEventFactDbRecords = _context.StudentSectionAttendanceEventFacts
                                                              .Where(student => student.StudentKey == studentKeyStr && student.SchoolYear == currentSchoolYearDim.SchoolYear)
                                                              .ToList();

            if (studentAttendanceEvents.Any() && studentSectionAttendanceEventFactDbRecords.Any())
            {

                studentSectionAttendanceEventFactDbRecords = studentSectionAttendanceEventFactDbRecords.Where(p => !studentAttendanceEvents.Any(x => x.EventDate == p.EventDate))
                                                                                                       .OrderBy(p => p.EventDate)
                                                                                                       .ToList();
            }

            StudentSectionAttendanceEventFact previousSectionAttendanceEvent = null;
            foreach (var studentAttendanceEvent in studentSectionAttendanceEventFactDbRecords)
            {
                var studentSectionAssociation = studentSectionAssociations
                    .Where(x => studentAttendanceEvent.EventDate.IsInRange(x))
                    .Where(x => string.Equals(x.SectionIdentifier, studentAttendanceEvent.SectionIdentifier, StringComparison.OrdinalIgnoreCase))
                    .Where(x => string.Equals(x.SessionName, studentAttendanceEvent.SessionName, StringComparison.OrdinalIgnoreCase))
                    .Where(x => string.Equals(x.LocalCourseCode, studentAttendanceEvent.LocalCourseCode, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (studentSectionAssociation == null)
                {
                    //"Found an attendance event for a student not currently enrolled in the section: {@attendanceEvent}", studentAttendanceEvent);
                    continue;
                }

                if (previousSectionAttendanceEvent != null
                    && previousSectionAttendanceEvent.EventDate == studentAttendanceEvent.EventDate
                    && string.Equals(previousSectionAttendanceEvent.SectionIdentifier, studentAttendanceEvent.SectionIdentifier, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(previousSectionAttendanceEvent.SessionName, studentAttendanceEvent.SessionName, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(previousSectionAttendanceEvent.LocalCourseCode, studentAttendanceEvent.LocalCourseCode, StringComparison.OrdinalIgnoreCase))
                {
                    //Found an attendance event on the same day for the same section: {@attendanceEvent}", studentAttendanceEvent);
                    continue;
                }

                studentAttendanceEvents.Add(new StudentAttendanceEvent
                {
                    DailyAttendanceCalculcationSource = DailyAttendanceCalculcationSource.BothAttendanceEventSources,
                    EventDate = studentAttendanceEvent.EventDate,
                    AttendanceEventReason = studentAttendanceEvent.AttendanceEventReason,
                    StudentSectionDim = studentSectionAssociation,
                    AttendanceEventCategoryDescriptor = studentAttendanceEvent.AttendanceEventCategoryDescriptor
                });

                previousSectionAttendanceEvent = studentAttendanceEvent;
            }

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey.ToString())).FirstOrDefault();
            if (schoolMinMaxDateDim == null)
            {
                //"No entry found in analytics.SchoolMinMaxDateDim
                return null;
            }

            var ytdGradingPeriod = new GradingPeriodDim
            {
                BeginDate = schoolMinMaxDateDim.MinDate.Value,
                EndDate = schoolMinMaxDateDim.MaxDate.Value,
                GradingPeriodDescription = string.Empty,
                IsYearToDate = true
            };

            int temp;
            int schoolKeyInt = int.TryParse(schoolKey, out temp) ? temp : 0;
            var schoolCalendarDays = _context.SchoolCalendarDims.Where(s => s.SchoolKey == schoolKeyInt).ToList();

            DateTime? _lastAttendanceDateForStudent = null;
            HashSet<DateTime> _excusedAbsences = new HashSet<DateTime>();
            HashSet<DateTime> _unexcusedAbsences = new HashSet<DateTime>();

            foreach (var studentAttendanceEvent in studentAttendanceEvents)
            {
                if (_lastAttendanceDateForStudent == studentAttendanceEvent.EventDate)
                    continue;

                if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                {
                    _excusedAbsences.Add(studentAttendanceEvent.EventDate);
                    _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                }
                else if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                {
                    _unexcusedAbsences.Add(studentAttendanceEvent.EventDate);
                    _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                }
            }

            HashSet<DateTime> _schoolCalendarDays = new HashSet<DateTime>();
            schoolCalendarDays.ForEach(s => _schoolCalendarDays.Add(s.Date));

            var daysStudentWasEnrolled = _schoolCalendarDays
                                        .DaysInRange(studentSchoolAssociations)
                                        .DaysInRange(studentSectionAssociations)
                                        .ToArray();
            if (!daysStudentWasEnrolled.Any())
            {
                // Do no calculations for students that were not enrolled for any instructional days
                return null;
            }

            _unexcusedAbsences.RemoveWhere(x => _excusedAbsences.Contains(x));
            var excusedAbsences = _excusedAbsences.CountDaysInRange(ytdGradingPeriod);
            var unExcusedAbsences = _unexcusedAbsences.CountDaysInRange(ytdGradingPeriod);

            var ytdTotalDaysAbsences = excusedAbsences + unExcusedAbsences;
            var ytdUnexcusedAbsences = unExcusedAbsences;

            Metric metric = Metric.StudentTotalDaysAbsent;
            var metricStateType = metric.GetMetricStateType(ytdTotalDaysAbsences, null);
            var metricState = MetricUtility.GetMetricState(metricStateType.Value);

            studentMetrics.Add(new StudentMetric
            {
                Name = "Days Absent",
                Value = ytdTotalDaysAbsences.ToString(),
                State = metricState,
                TrendDirection = null
            });

            metric = Metric.StudentTotalUnexcusedDaysAbsent;
            metricStateType = metric.GetMetricStateType(ytdUnexcusedAbsences, null);
            metricState = MetricUtility.GetMetricState(metricStateType.Value);

            studentMetrics.Add(new StudentMetric
            {
                Name = "Unexcused Days",
                Value = ytdUnexcusedAbsences.ToString(),
                State = metricState,
                TrendDirection = null
            });


            IReadOnlyCollection<Period> Periods = Enum.GetValues(typeof(Period))
                                                       .Cast<Period>()
                                                       .ToArray();

            IDictionary<Period, PeriodData> _periodData = new SortedList<Period, PeriodData>();
            foreach (var period in Periods)
            {
                _periodData[period] = new PeriodData();
            }

            List<int> ReportingPeriodLengths = ((ReportingPeriodLength[])Enum.GetValues(typeof(ReportingPeriodLength))).Select(c => (int)c).ToList();
            ReportingPeriodLengths.ForEach(_periodLength =>
            {
                schoolCalendarDays.ForEach(schoolCalendarDay =>
                {
                    var periodIndex = schoolCalendarDay.GetPeriodIndex(_periodLength);
                    if (periodIndex > 2)
                    {
                        return;
                    }

                    var period = (Period)periodIndex;
                    var periodData = _periodData[period];
                    periodData.CalendarDates.Add(schoolCalendarDay.Date);

                });
                foreach (var periodData in _periodData.Values)
                {
                    periodData.DailyAttendanceRateEvents = 0;
                    periodData.ClassPeriodAbsenceRateAttendanceEvents = 0;
                    periodData.TardyAttendanceEvents = 0;
                }

                var firstPeriod = _periodData[Period.First];

                var attendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => firstPeriod.CalendarDates.Any()
                                                                            && studentAttendanceEvent.EventDate.IsBetween(firstPeriod.CalendarDates.Min(), firstPeriod.CalendarDates.Max()));

                var dailyAttendanceRateAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                                && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                    || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                                            .ToList();
                firstPeriod.DailyAttendanceRateEvents = dailyAttendanceRateAttendanceEvents.Count;

                var classPeriodAbsenceRateAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsStudentSectionAttendanceEvent
                                                                                    && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                        || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                             .ToList();
                firstPeriod.ClassPeriodAbsenceRateAttendanceEvents = classPeriodAbsenceRateAttendanceEvents.Count;


                var tardyAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                    && studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.Tardy))
                                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                            .ToList();
                firstPeriod.TardyAttendanceEvents = tardyAttendanceEvents.Count;


                if (firstPeriod.CalendarDates.Count < _periodLength)
                {
                    // Business Rule: School with less instructional days than reporting period days.
                    return;
                }
                var secondPeriod = _periodData[Period.Second];
                attendanceEvents = studentAttendanceEvents.Where(studentAttendanceEvent => secondPeriod.CalendarDates.Any()
                                                                            && studentAttendanceEvent.EventDate.IsBetween(secondPeriod.CalendarDates.Min(), secondPeriod.CalendarDates.Max()));

                dailyAttendanceRateAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                                && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                    || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                                            .ToList();
                secondPeriod.DailyAttendanceRateEvents = dailyAttendanceRateAttendanceEvents.Count;

                classPeriodAbsenceRateAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsStudentSectionAttendanceEvent
                                                                                    && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                        || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                             .ToList();
                secondPeriod.ClassPeriodAbsenceRateAttendanceEvents = classPeriodAbsenceRateAttendanceEvents.Count;


                tardyAttendanceEvents = attendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                    && studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.Tardy))
                                                            .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                            .ToList();
                secondPeriod.TardyAttendanceEvents = tardyAttendanceEvents.Count;


                var firstPeriodDenominator = GetDaysStudentWasEnrolled(false, firstPeriod.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;
                if (firstPeriodDenominator >= _periodLength)
                {
                    var secondPeriodDenominator = GetDaysStudentWasEnrolled(false, secondPeriod.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;

                    var firstPeriodNumerator = firstPeriodDenominator - firstPeriod.DailyAttendanceRateEvents;
                    var firstPeriodRatio = ((decimal)firstPeriodNumerator / firstPeriodDenominator).RoundTo(3);
                    var secondPeriodNumerator = secondPeriodDenominator - secondPeriod.DailyAttendanceRateEvents;

                    int? trend;
                    bool flag;
                    GetTrendByAttendance(firstPeriodDenominator, secondPeriodDenominator, firstPeriodNumerator, secondPeriodNumerator, RateDirection.OneToZero, out trend, out flag);

                    Metric metric = _periodLength == (int)ReportingPeriodLength.LastXDays ? Metric.StudentDailyAttendanceRateLastXDays : Metric.StudentDailyAttendanceRateLastYDays;
                    var metricStateType = metric.GetMetricStateType(firstPeriodRatio, null);
                    var metricState = MetricUtility.GetMetricState(metricStateType.Value);

                    studentMetrics.Add(new StudentMetric
                    {
                        Name = string.Format("Daily Attendance Rate - Last {0} Days", _periodLength),
                        Value = firstPeriodRatio.Display().DisplayValue(format),
                        TrendDirection = trend ?? null,
                        State = metricState
                    });

                    firstPeriodNumerator = firstPeriod.TardyAttendanceEvents;
                    firstPeriodRatio = ((decimal)firstPeriodNumerator / firstPeriodDenominator).RoundTo(3);
                    secondPeriodNumerator = secondPeriod.TardyAttendanceEvents;

                    GetTrendByAttendance(firstPeriodDenominator, secondPeriodDenominator, firstPeriodNumerator, secondPeriodNumerator, RateDirection.ZeroToOne, out trend, out flag);

                    metric = _periodLength == (int)ReportingPeriodLength.LastXDays ? Metric.StudentTardyRateLastXDays : Metric.StudentTardyRateLastYDays;
                    metricStateType = metric.GetMetricStateType(firstPeriodRatio, null);
                    metricState = MetricUtility.GetMetricState(metricStateType.Value);

                    studentMetrics.Add(new StudentMetric
                    {
                        Name = string.Format("Tardy Rate - Last {0} Days", _periodLength),
                        Value = firstPeriodRatio.Display().DisplayValue(format),
                        TrendDirection = trend ?? null,
                        State = metricState
                    });
                }

                firstPeriodDenominator = GetDaysStudentWasEnrolled(true, firstPeriod.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;
                if (firstPeriodDenominator >= _periodLength)
                {
                    var secondPeriodDenominator = GetDaysStudentWasEnrolled(true, secondPeriod.CalendarDates, studentSchoolAssociations, studentSectionAssociations).Count;

                    var firstPeriodNumerator = firstPeriod.ClassPeriodAbsenceRateAttendanceEvents;
                    var firstPeriodRatio = ((decimal)firstPeriodNumerator / firstPeriodDenominator).RoundTo(3);
                    var secondPeriodNumerator = secondPeriod.ClassPeriodAbsenceRateAttendanceEvents;

                    int? trend;
                    bool flag;
                    GetTrendByAttendance(firstPeriodDenominator, secondPeriodDenominator, firstPeriodNumerator, secondPeriodNumerator, RateDirection.ZeroToOne, out trend, out flag);

                    Metric metric = _periodLength == (int)ReportingPeriodLength.LastXDays ? Metric.StudentClassPeriodAbsenceRateLastXDays : Metric.StudentClassPeriodAbsenceRateLastYDays;
                    var metricStateType = metric.GetMetricStateType(firstPeriodRatio, null);
                    var metricState = MetricUtility.GetMetricState(metricStateType.Value);

                    studentMetrics.Add(new StudentMetric
                    {
                        Name = string.Format("Class Period Absence Rate - Last {0} Days", _periodLength),
                        Value = firstPeriodRatio.Display().DisplayValue(format),
                        TrendDirection = trend ?? null,
                        State = metricState
                    });
                }
            });


            var ytdNumerator = 0;
            var _daysStudentWasEnrolled = 0;
            var ytdDailyAttendanceRateAbsences = studentAttendanceEvents.Where(studentAttendanceEvent => (studentAttendanceEvent.IsForDailyAttendance()
                                                                                 && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                     || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence))))
                                                                             .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                                             .ToList()
                                                                             .Count();
            _daysStudentWasEnrolled = GetDaysStudentWasEnrolled(false, _schoolCalendarDays, studentSchoolAssociations, studentSectionAssociations).Count;
            ytdNumerator = _daysStudentWasEnrolled - ytdDailyAttendanceRateAbsences;
            var ytdDailyAttendanceRate = ((decimal)ytdNumerator / _daysStudentWasEnrolled).RoundTo(3);

            metric = Metric.StudentDailyAttendanceRateYearToDate;
            metricStateType = metric.GetMetricStateType(ytdDailyAttendanceRate, null);
            metricState = MetricUtility.GetMetricState(metricStateType.Value);

            studentMetrics.Add(new StudentMetric
            {
                Name = "Daily Attendance Rate - Year to Date",
                Value = ytdDailyAttendanceRate.Display().DisplayValue(format),
                State = metricState,
                TrendDirection = null,
            });


            var ytdClassPeriodAttendanceRateAbsences = studentAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsStudentSectionAttendanceEvent
                                                                                    && (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence)
                                                                                        || studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence)))
                                                                                .ToList()
                                                                                .Count();
            _daysStudentWasEnrolled = GetDaysStudentWasEnrolled(true, _schoolCalendarDays, studentSchoolAssociations, studentSectionAssociations).Count;
            ytdNumerator = ytdClassPeriodAttendanceRateAbsences;
            var ytdClassPeriodAttendanceRate = ((decimal)ytdNumerator / _daysStudentWasEnrolled).RoundTo(3);

            metric = Metric.StudentClassPeriodAbsenceRateYearToDate;
            metricStateType = metric.GetMetricStateType(ytdClassPeriodAttendanceRate, null);
            metricState = MetricUtility.GetMetricState(metricStateType.Value);

            studentMetrics.Add(new StudentMetric
            {
                Name = "Class Period Absence Rate - Year to Date",
                Value = ytdClassPeriodAttendanceRate.Display().DisplayValue(format),
                State = metricState,
                TrendDirection = null,
            });


            var ytdTardyRateAbsences = studentAttendanceEvents.Where(studentAttendanceEvent => studentAttendanceEvent.IsForDailyAttendance()
                                                                     && studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.Tardy))
                                                                .DistinctBy(studentAttendanceEvent => studentAttendanceEvent.EventDate)
                                                                .ToList()
                                                                .Count();
            _daysStudentWasEnrolled = GetDaysStudentWasEnrolled(false, _schoolCalendarDays, studentSchoolAssociations, studentSectionAssociations).Count;
            ytdNumerator = ytdTardyRateAbsences;
            var ytdTardyRate = ((decimal)ytdNumerator / _daysStudentWasEnrolled).RoundTo(3);

            metric = Metric.StudentTardyRateYearToDate;
            metricStateType = metric.GetMetricStateType(ytdTardyRate, null);
            metricState = MetricUtility.GetMetricState(metricStateType.Value);

            studentMetrics.Add(new StudentMetric
            {
                Name = "Tardy Rate - Year to Date",
                Value = ytdTardyRate.Display().DisplayValue(format),
                State = metricState,
                TrendDirection = null,
            });

            return studentMetrics;

        }

        public IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            IDictionary<string, List<StudentMetric>> returnMap = new Dictionary<string, List<StudentMetric>>();
            var currentSchoolYearDim = _context.CurrentSchoolYearDims.FirstOrDefault();

            if (currentSchoolYearDim == null)
                return returnMap;

            var studentSchoolAssociations = _context.StudentSchoolDims
                                        .Where(s => s.SchoolKey.Equals(schoolKey))
                                        .ToList();

            int temp;

            var studentSchoolAssociationsDictionary = studentSchoolAssociations
                                                        .GroupBy(x => x.StudentKey)
                                                        .ToDictionary(x => x.Key, x => x.ToList());

            var studentSectionAssociations = _context.StudentSectionDims
                                        .Where(s => s.SchoolKey.Equals(schoolKey))
                                        .ToList();

            var studentIdList = studentSectionAssociations.Select(ssa => ssa.StudentKey).Distinct().ToList();

            var studentSectionAssociationsDictionary = studentSectionAssociations
                                                        .GroupBy(x => x.StudentKey)
                                                        .ToDictionary(x => x.Key, x => x.ToList());

            var studentSchoolAttendanceEventFactDbRecords = (from ssaef in _context.StudentSchoolAttendanceEventFacts.AsNoTracking()
                                                             where ssaef.SchoolYear == currentSchoolYearDim.SchoolYear && studentIdList.Contains(ssaef.StudentKey)
                                                             select ssaef).ToList().OrderBy(a => a.StudentKey);

            /**var studentSchoolAttendanceEventFactDbRecordsIntermediate = (from ssaef in _context.StudentSchoolAttendanceEventFact.AsNoTracking()
                                                             join id in studentIdList on ssaef.StudentKey equals id
                                                             where ssaef.SchoolYear == currentSchoolYearDim.SchoolYear
                                                             select new { ssaef, id}).ToList();

            var studentSchoolAttendanceEventFactDbRecords = studentSchoolAttendanceEventFactDbRecordsIntermediate.Select(x => x.ssaef);
            **/
            var studentSchoolAttendanceEventFactDbRecordsDictionary = studentSchoolAttendanceEventFactDbRecords
                                                                        .GroupBy(x => x.StudentKey)
                                                                        .ToDictionary(x => x.Key, x => x.ToList().OrderBy(a => a.EventDate));

            var studentAttendanceEventDictionary = new Dictionary<string, List<StudentAttendanceEvent>>();

            foreach (var dictionaryKey in studentSchoolAttendanceEventFactDbRecordsDictionary.Keys)
            {
                StudentSchoolAttendanceEventFact previousAttendanceEvent = null;
                foreach (var attendanceEvent in studentSchoolAttendanceEventFactDbRecordsDictionary[dictionaryKey])
                {
                    if (previousAttendanceEvent != null && previousAttendanceEvent.EventDate == attendanceEvent.EventDate)
                    {
                        //Found multiple daily attendance events on the same day: {@attendanceEvent} AND {@duplicateAttendanceEvent}", previousAttendanceEvent, attendanceEvent);
                        continue;
                    }

                    if (studentAttendanceEventDictionary[dictionaryKey] == null)
                    {
                        studentAttendanceEventDictionary[dictionaryKey] = new List<StudentAttendanceEvent>();
                    }

                    studentAttendanceEventDictionary[dictionaryKey].Add(new StudentAttendanceEvent
                    {
                        DailyAttendanceCalculcationSource = DailyAttendanceCalculcationSource.BothAttendanceEventSources,
                        EventDate = attendanceEvent.EventDate,
                        AttendanceEventReason = attendanceEvent.AttendanceEventReason,
                        AttendanceEventCategoryDescriptor = attendanceEvent.AttendanceEventCategoryDescriptor
                    });

                    previousAttendanceEvent = attendanceEvent;
                }
            }


            /**var studentSectionAttendanceEventFactDbRecords = _context.StudentSectionAttendanceEventFact
                                                              .Where(student => student.StudentKey == studentKey && student.SchoolYear == currentSchoolYearDim.SchoolYear)
                                                              .ToList();
            **/
            var studentSectionAttendanceEventFactDbRecords = (from ssaef in _context.StudentSectionAttendanceEventFacts
                                                              join ssa in _context.StudentSectionDims on ssaef.StudentKey equals ssa.StudentKey
                                                              where ssaef.SchoolYear == currentSchoolYearDim.SchoolYear
                                                                && ssa.SchoolKey == schoolKey
                                                              select ssaef).Distinct().ToList().OrderBy(x => x.StudentKey);

            //where ssaef.SchoolYear == currentSchoolYearDim.SchoolYear && studentIdList.Contains(ssaef.StudentKey)
            // select ssaef).Distinct().ToList().OrderBy(x => x.StudentKey);

            var studentSectionAttendanceEventFactDbRecordsDictionary = studentSectionAttendanceEventFactDbRecords
                                                                        .GroupBy(x => x.StudentKey)
                                                                        .ToDictionary(x => x.Key, x => x.ToList().OrderBy(a => a.EventDate));

            HashSet<string> masterStudentKeySet = studentSchoolAttendanceEventFactDbRecordsDictionary.Keys.ToHashSet();
            masterStudentKeySet.AddRange(studentSectionAttendanceEventFactDbRecordsDictionary.Keys);
            foreach (var dictionaryKey in masterStudentKeySet)
            {
                var schoolAttendanceEvents = studentSchoolAttendanceEventFactDbRecordsDictionary.ContainsKey(dictionaryKey) ? studentSchoolAttendanceEventFactDbRecordsDictionary[dictionaryKey] : null;
                var sectionAttendanceEvents = studentSectionAttendanceEventFactDbRecordsDictionary.ContainsKey(dictionaryKey) ? studentSectionAttendanceEventFactDbRecordsDictionary[dictionaryKey] : null;

                IEnumerable<StudentSectionAttendanceEventFact> trimmedSectionAttendanceEvents = null;
                if (schoolAttendanceEvents != null && schoolAttendanceEvents.Any() && sectionAttendanceEvents != null && sectionAttendanceEvents.Any())
                {
                    trimmedSectionAttendanceEvents = sectionAttendanceEvents.Where(p => !schoolAttendanceEvents.Any(x => x.EventDate == p.EventDate))
                                                        .OrderBy(p => p.EventDate)
                                                        .ToList();
                }

                trimmedSectionAttendanceEvents = trimmedSectionAttendanceEvents != null ? trimmedSectionAttendanceEvents : sectionAttendanceEvents;
                StudentSectionAttendanceEventFact previousSectionAttendanceEvent = null;
                foreach (var studentAttendanceEvent in trimmedSectionAttendanceEvents)
                {
                    var studentSectionAssociation = studentSectionAssociations
                        .Where(x => string.Equals(x.StudentKey, dictionaryKey))
                        .Where(x => studentAttendanceEvent.EventDate.IsInRange(x))
                        .Where(x => string.Equals(x.SectionIdentifier, studentAttendanceEvent.SectionIdentifier, StringComparison.OrdinalIgnoreCase))
                        .Where(x => string.Equals(x.SessionName, studentAttendanceEvent.SessionName, StringComparison.OrdinalIgnoreCase))
                        .Where(x => string.Equals(x.LocalCourseCode, studentAttendanceEvent.LocalCourseCode, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (studentSectionAssociation == null)
                    {
                        //"Found an attendance event for a student not currently enrolled in the section: {@attendanceEvent}", studentAttendanceEvent);
                        continue;
                    }

                    if (previousSectionAttendanceEvent != null
                        && previousSectionAttendanceEvent.EventDate == studentAttendanceEvent.EventDate
                        && string.Equals(previousSectionAttendanceEvent.SectionIdentifier, studentAttendanceEvent.SectionIdentifier, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(previousSectionAttendanceEvent.SessionName, studentAttendanceEvent.SessionName, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(previousSectionAttendanceEvent.LocalCourseCode, studentAttendanceEvent.LocalCourseCode, StringComparison.OrdinalIgnoreCase))
                    {
                        //Found an attendance event on the same day for the same section: {@attendanceEvent}", studentAttendanceEvent);
                        continue;
                    }

                    if (!studentAttendanceEventDictionary.ContainsKey(dictionaryKey))
                    {
                        studentAttendanceEventDictionary[dictionaryKey] = new List<StudentAttendanceEvent>();
                    }
                    studentAttendanceEventDictionary[dictionaryKey].Add(new StudentAttendanceEvent
                    {
                        DailyAttendanceCalculcationSource = DailyAttendanceCalculcationSource.BothAttendanceEventSources,
                        EventDate = studentAttendanceEvent.EventDate,
                        AttendanceEventReason = studentAttendanceEvent.AttendanceEventReason,
                        StudentSectionDim = studentSectionAssociation,
                        AttendanceEventCategoryDescriptor = studentAttendanceEvent.AttendanceEventCategoryDescriptor
                    });

                    previousSectionAttendanceEvent = studentAttendanceEvent;
                }

            }

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey.ToString())).FirstOrDefault();
            if (schoolMinMaxDateDim == null)
            {
                //"No entry found in analytics.SchoolMinMaxDateDim
                return returnMap;
            }

            var ytdGradingPeriod = new GradingPeriodDim
            {
                BeginDate = schoolMinMaxDateDim.MinDate.Value,
                EndDate = schoolMinMaxDateDim.MaxDate.Value,
                GradingPeriodDescription = string.Empty,
                IsYearToDate = true
            };

            int schoolKeyInt = int.TryParse(schoolKey, out temp) ? temp : 0;
            var schoolCalendarDays = _context.SchoolCalendarDims.Where(s => s.SchoolKey == schoolKeyInt).ToList();
            HashSet<DateTime> _schoolCalendarDays = new HashSet<DateTime>();
            schoolCalendarDays.ForEach(s => _schoolCalendarDays.Add(s.Date));

            foreach (var studentId in studentAttendanceEventDictionary.Keys)
            {
                DateTime? _lastAttendanceDateForStudent = null;
                HashSet<DateTime> _excusedAbsences = new HashSet<DateTime>();
                HashSet<DateTime> _unexcusedAbsences = new HashSet<DateTime>();
                int ytdTotalDaysAbsences = 0;
                int ytdUnexcusedAbsences = 0;

                foreach (var studentAttendanceEvent in studentAttendanceEventDictionary[studentId])
                {
                    if (_lastAttendanceDateForStudent == studentAttendanceEvent.EventDate)
                        continue;

                    if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.ExcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                    {
                        _excusedAbsences.Add(studentAttendanceEvent.EventDate);
                        _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                    }
                    else if (studentAttendanceEvent.AttendanceEventCategoryDescriptor.Equals(AttendanceEventCategoryDescriptor.UnexcusedAbsence) && studentAttendanceEvent.IsForDailyAttendance())
                    {
                        _unexcusedAbsences.Add(studentAttendanceEvent.EventDate);
                        _lastAttendanceDateForStudent = studentAttendanceEvent.EventDate;
                    }
                }

                var daysStudentWasEnrolled = _schoolCalendarDays
                            .DaysInRange(studentSchoolAssociationsDictionary.ContainsKey(studentId) ? studentSchoolAssociationsDictionary[studentId] : new List<StudentSchoolDim>())
                            .DaysInRange(studentSectionAssociationsDictionary.ContainsKey(studentId) ? studentSectionAssociationsDictionary[studentId] : new List<StudentSectionDim>())
                            .ToArray();
                if (!daysStudentWasEnrolled.Any())
                {
                    // Do no calculations for students that were not enrolled for any instructional days
                    continue;
                }

                _unexcusedAbsences.RemoveWhere(x => _excusedAbsences.Contains(x));
                var excusedAbsences = _excusedAbsences.CountDaysInRange(ytdGradingPeriod);
                var unExcusedAbsences = _unexcusedAbsences.CountDaysInRange(ytdGradingPeriod);
                ytdTotalDaysAbsences = excusedAbsences + unExcusedAbsences;
                ytdUnexcusedAbsences = unExcusedAbsences;

                if (!returnMap.ContainsKey(studentId))
                {
                    returnMap[studentId] = new List<StudentMetric>();
                }
                returnMap[studentId].Add(new Models.StudentMetric { StudentUsi = int.TryParse(studentId, out temp) ? temp : 0, Name = "Days Absent", Value = ytdTotalDaysAbsences.ToString() });
                returnMap[studentId].Add(new Models.StudentMetric { StudentUsi = int.TryParse(studentId, out temp) ? temp : 0, Name = "Unexcused Days", Value = ytdUnexcusedAbsences.ToString() });
            }



            return returnMap;
        }
        private enum Period
        {
            First = 1,
            Second = 2
        }

        private enum ReportingPeriodLength
        {
            LastXDays = 20,
            LastYDays = 40
        }

        private class PeriodData
        {
            public PeriodData()
            {
                CalendarDates = new HashSet<DateTime>();
            }

            public HashSet<DateTime> CalendarDates { get; private set; }
            public int DailyAttendanceRateEvents { get; set; }
            public int ClassPeriodAbsenceRateAttendanceEvents { get; set; }
            public int TardyAttendanceEvents { get; set; }
        }

        public List<DateTime> GetDaysStudentWasEnrolled(bool isClassPeriodAbsenceRate, HashSet<DateTime> calendarDates, List<StudentSchoolDim> studentSchoolAssociations, List<StudentSectionDim> studentSectionAssociations)
        {
            if (isClassPeriodAbsenceRate)
            {
                var dateTotal = new List<DateTime>();
                foreach (var dateRange in studentSectionAssociations)
                {
                    var dateTimes = calendarDates
                        .DaysInRange(studentSchoolAssociations)
                        .Where(dateRange.ContainsDate);
                    dateTotal.AddRange(dateTimes);
                }
                dateTotal.Sort();
                return dateTotal;
            }
            else
            {
                var dateTotal = new SortedSet<DateTime>();
                foreach (var dateRange in studentSectionAssociations)
                {
                    var dateTimes = calendarDates
                        .DaysInRange(studentSchoolAssociations)
                        .Where(dateRange.ContainsDate);
                    foreach (var dateTime in dateTimes)
                    {
                        dateTotal.Add(dateTime);
                    }
                }
                return dateTotal.ToList();
            }
        }

        private void GetTrendByAttendance(int firstPeriodTotal, int secondPeriodTotal, int firstPeriodAttendance, int secondPeriodAttendance, RateDirection rateDirection, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal != secondPeriodTotal || firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            GetTrendByStudent(firstPeriodTotal, secondPeriodTotal, firstPeriodAttendance, secondPeriodAttendance, rateDirection, out trend, out flag);
        }

        private void GetTrendByStudent(int firstPeriodTotal, int secondPeriodTotal, decimal firstPeriodAttendance, decimal secondPeriodAttendance, RateDirection rateDirection, out int? trend, out bool flag)
        {
            flag = false;
            trend = null;

            if (firstPeriodTotal == 0 || secondPeriodTotal == 0)
            {
                return;
            }

            var firstPeriodRate = firstPeriodAttendance / firstPeriodTotal;
            var secondPeriodRate = secondPeriodAttendance / secondPeriodTotal;

            GetTrend(out trend, out flag, firstPeriodRate, secondPeriodRate, rateDirection);
        }

        public void GetTrend(out int? trend, out bool flag, decimal? firstPeriodRate, decimal? secondPeriodRate, RateDirection rateDirection)
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
                if (rateDirection == RateDirection.OneToZero && secondPeriodRate - firstPeriodRate > 0.1m)
                {
                    flag = true;
                }
            }
            else
            {
                trend = 0;
            }
        }

        public int GetMetricId()
        {
            throw new NotImplementedException();
        }
    }
}