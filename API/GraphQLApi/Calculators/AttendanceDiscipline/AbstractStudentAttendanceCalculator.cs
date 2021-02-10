using AMT.Data.Descriptors.Enumerations;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMT.Data.Common;
using GraphQLApi.Enumerations;
using GraphQLApi.Common;
using Microsoft.Extensions.Options;
using GraphQLApi.Infrastructure;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractStudentAttendanceCalculator : AbstractStudentMetricCalculator
    {
        public static string DISPLAY_FORMAT_ATTENDANCE_RATE = "{0:P1}";

        public AbstractStudentAttendanceCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected IDictionary<string, List<StudentSectionDim>> getStudentSectionAssociationsDictionary(string schoolKey)
        {
            string cacheKey = "StudentSectionAssociations:" + schoolKey;

            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary;

            if (_memoryCache.TryGetValue(cacheKey, out studentSectionAssociationsDictionary))
            {
                return studentSectionAssociationsDictionary;
            }

            var studentSectionAssociations = _context.StudentSectionDims
                            .Where(s => s.SchoolKey.Equals(schoolKey))
                            .ToList();

            studentSectionAssociationsDictionary = studentSectionAssociations
                .GroupBy(x => x.StudentKey)
                .ToDictionary(x => x.Key, x => x.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(500)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, studentSectionAssociationsDictionary, cacheEntryOptions);

            return studentSectionAssociationsDictionary;
        }
        protected IDictionary<string, List<StudentAttendanceEvent>> getStudentAttendanceEvents(string schoolKey)
        {
            IDictionary<string, List<StudentAttendanceEvent>> studentAttendanceEventDictionary;

            string cacheKey = "StudentAttendanceEvents" + schoolKey;
            if (_memoryCache.TryGetValue(cacheKey, out studentAttendanceEventDictionary))
            {
                return studentAttendanceEventDictionary;
            }
            var currentSchoolYearDim = _context.CurrentSchoolYearDims.FirstOrDefault();

            studentAttendanceEventDictionary = new Dictionary<string, List<StudentAttendanceEvent>>();
            if (currentSchoolYearDim == null)
                return studentAttendanceEventDictionary;

            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary = getStudentSectionAssociationsDictionary(schoolKey);

            var studentIdList = studentSectionAssociationsDictionary.Keys.ToList();

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

                    if (!studentAttendanceEventDictionary.ContainsKey(dictionaryKey))
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
                    var studentSectionAssociations = studentSectionAssociationsDictionary.ContainsKey(dictionaryKey) ? studentSectionAssociationsDictionary[dictionaryKey] : new List<StudentSectionDim>();
                    var studentSectionAssociation = studentSectionAssociations
                        .Where(x => string.Equals(x.StudentKey, dictionaryKey)) //This is probably redundant as we are looking up in the dictionary.
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

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1000)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentAttendanceEventDictionary, cacheEntryOptions);

            return studentAttendanceEventDictionary;

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
        
        public IDictionary<Period, PeriodData> GetPeriodData(string schoolkey, int periodLength)
        {
            string cacheKey = "PeriodLength:"+periodLength+":PeriodData:" + schoolkey;
            IDictionary<Period, PeriodData> periodData;
            if(_memoryCache.TryGetValue(cacheKey,out periodData))
            {
                return periodData;
            }

            var schoolCalendarDays = StudentDataRepository.GetSchoolCalendarDays(schoolkey);

            IReadOnlyCollection<Period> Periods = Enum.GetValues(typeof(Period))
                                           .Cast<Period>()
                                           .ToArray();

            periodData = new SortedList<Period, PeriodData>();
            foreach (var period in Periods)
            {
                periodData[period] = new PeriodData();
            }

            schoolCalendarDays.ForEach((Action<SchoolCalendarDim>)(schoolCalendarDay =>
            {
                var periodIndex = schoolCalendarDay.GetPeriodIndex(periodLength);
                if (periodIndex > 2)
                {
                    return;
                }

                var period = (Period)periodIndex;
                var periodDatum = periodData[period];
                periodDatum.CalendarDates.Add(schoolCalendarDay.Date);

            }));


            int i = 0;
            foreach (var periodDatum in periodData.Values)
            {
                if(periodDatum.CalendarDates.Count < periodLength)
                {
                    periodDatum.CalendarDates.Clear();
                }
                i++;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, periodData, cacheEntryOptions);

            return periodData;
        }





    }
}
