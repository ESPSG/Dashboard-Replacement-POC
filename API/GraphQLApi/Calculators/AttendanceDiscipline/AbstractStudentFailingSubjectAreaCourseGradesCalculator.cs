using AMT.Data.Common;
using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Models;
using GraphQLApi.Models.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class AbstractStudentFailingSubjectAreaCourseGradesCalculator : AbstractStudentMetricCalculator
    {
        public AbstractStudentFailingSubjectAreaCourseGradesCalculator(CalculatorAppContext appContext) : base(appContext)
        {

        }
        public override IDictionary<string, List<StudentMetric>> GetAllStudentMetricsForSchool(string schoolKey)
        {
            string cacheKey = "schoolKey:" + schoolKey + " MetricId:" + GetMetricId();

            IDictionary<string, List<StudentMetric>> returnMap;
            if (_memoryCache.TryGetValue(cacheKey, out returnMap))
            {
                return returnMap;
            }
            returnMap = new Dictionary<string, List<StudentMetric>>();

            var studentGradingPeriods = GetStudentGradingPeriods(schoolKey);
            foreach (var studentGradingPeriod in studentGradingPeriods)
            {
                var studentKey = studentGradingPeriod.Key;
                var studentCourseGradingPeriodCalculations = studentGradingPeriod.Value;

                var metricState = string.Empty;
                var value = string.Empty;
                int? trend = null;
                var flag = false;

                if (studentCourseGradingPeriodCalculations.Any())
                {
                    var maxGradingPeriodData = studentCourseGradingPeriodCalculations.Values[studentCourseGradingPeriodCalculations.Count - 1];
                    if (maxGradingPeriodData.StudentGrades.Any())
                    {
                        var previousGradingPeriodData = (studentCourseGradingPeriodCalculations.Count < 2) ? null : studentCourseGradingPeriodCalculations.Values[studentCourseGradingPeriodCalculations.Count - 2];
                        if (previousGradingPeriodData != null && previousGradingPeriodData.StudentGrades.Any())
                        {
                            CourseGradeGranularMetric.GetTrendByStudentGrades(
                                maxGradingPeriodData.NumberOfGradesAtOrBelowThreshold,
                                previousGradingPeriodData.NumberOfGradesAtOrBelowThreshold,
                                maxGradingPeriodData.StudentGrades.Count(),
                                previousGradingPeriodData.StudentGrades.Count(), out trend, out flag);
                        }

                        var metricStateType = CourseGradeGranularMetric.GetMetricStateTypeForStudentCourseGrades(maxGradingPeriodData.NumberOfGradesAtOrBelowThreshold);
                        metricState = MetricUtility.GetMetricState(metricStateType.Value);
                        value = string.Concat(maxGradingPeriodData.NumberOfGradesAtOrBelowThreshold, '/', maxGradingPeriodData.StudentGrades.Count());
                    }
                }

                if (!returnMap.ContainsKey(studentKey))
                {
                    returnMap[studentKey] = new List<StudentMetric>();
                }
                returnMap[studentKey].Add(new StudentMetric
                {
                    Id = _metricInfo.Id,
                    Name = _metricInfo.DisplayName,
                    Value = value,
                    ValueTypeName = "System.String",
                    State = metricState,
                    TrendDirection = trend,
                });
            }
            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }
        protected IDictionary<string, SortedList<GradingPeriodDim, StudentCourseGradingPeriodCalculation>> GetStudentGradingPeriods(string schoolKey)
        {
            IDictionary<string, SortedList<GradingPeriodDim, StudentCourseGradingPeriodCalculation>> studentGradingPeriodsDictionary;

            string cacheKey = "StudentSchoolAssessments" + schoolKey + "MetricId:" + GetMetricId();
            if (_memoryCache.TryGetValue(cacheKey, out studentGradingPeriodsDictionary))
            {
                return studentGradingPeriodsDictionary;
            }
            studentGradingPeriodsDictionary = new Dictionary<string, SortedList<GradingPeriodDim, StudentCourseGradingPeriodCalculation>>();

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey)).FirstOrDefault();
            if (schoolMinMaxDateDim == null || !schoolMinMaxDateDim.MaxDate.HasValue)
            {
                return studentGradingPeriodsDictionary;
            }

            DateTime currentSchoolDay = schoolMinMaxDateDim.MaxDate.Value;
            List<GradingPeriodDim> gradingPeriodDims = StudentDataRepository.GetSchoolGradingPeriods(schoolKey);

            if (!gradingPeriodDims.Any())
                return studentGradingPeriodsDictionary;

            IDictionary<string, List<StudentSectionDim>> studentSectionAssociationsDictionary = StudentDataRepository.GetStudentSectionAssociationsDictionary(schoolKey);
            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);
            IDictionary<string, List<StudentGradeDim>> studentGradeDbRecords = GetStudentGradeDbRecords(schoolKey);
            IDictionary<string, List<StudentGrade>> studentGrades = new Dictionary<string, List<StudentGrade>>();

            foreach (var studentGradeDbRecord in studentGradeDbRecords)
            {
                if (!studentGradeDbRecord.Value.Any())
                    continue;

                SortedList<GradingPeriodDim, StudentCourseGradingPeriodCalculation> studentCourseGradingPeriodCalculations = new SortedList<GradingPeriodDim, StudentCourseGradingPeriodCalculation>(new GradingPeriodComparer());
                foreach (var gradingPeriod in gradingPeriodDims)
                {
                    if (!gradingPeriod.ShouldBeIgnored(currentSchoolDay))
                    {
                        studentCourseGradingPeriodCalculations.Add(gradingPeriod, new StudentCourseGradingPeriodCalculation(CourseGradeGranularMetric, gradingPeriod, studentGradeDbRecord.Key));
                    }
                }

                StudentGrade studentGrade = new StudentGrade { StudentKey = studentGradeDbRecord.Key };
                StudentGradeDim previousStudentGrade = null;
                foreach (var _studentGradeDbRecord in studentGradeDbRecord.Value)
                {
                    studentGrade.GradingPeriod = gradingPeriodDims.Where(sgp => _studentGradeDbRecord.GradingPeriodDescription.Equals(sgp.GradingPeriodDescription)
                                                                                && _studentGradeDbRecord.GradingPeriodBeginDate.Equals(sgp.BeginDate))
                                                                     .FirstOrDefault();
                    if (studentGrade.GradingPeriod == null)
                    {
                        continue;
                    }

                    if (studentSectionAssociationsDictionary.ContainsKey(studentGrade.StudentKey))
                    {
                        var studentSectionAssociations = studentSectionAssociationsDictionary[studentGrade.StudentKey];
                        studentGrade.StudentSection = studentSectionAssociations.Where(ssa => ssa.SectionIdentifier.Equals(_studentGradeDbRecord.SectionIdentifier)
                                                                                            && ssa.LocalCourseCode.Equals(_studentGradeDbRecord.LocalCourseCode)
                                                                                            && ssa.SessionName.Equals(_studentGradeDbRecord.SessionName)
                                                                                            && ssa.SchoolYear.Equals(_studentGradeDbRecord.SchoolYear.ToString())
                                                                                            && ssa.SchoolKey.Equals(_studentGradeDbRecord.SchoolKey))
                                                                                .FirstOrDefault();
                        if (studentGrade.StudentSection == null || !studentGrade.StudentSection.OverlapsWith(studentGrade.GradingPeriod))
                        {
                            continue;
                        }

                    }

                    if (previousStudentGrade != null
                    && SectionComparer.Instance.Equals(previousStudentGrade, _studentGradeDbRecord)
                    && previousStudentGrade.GradingPeriodBeginDate.Equals(_studentGradeDbRecord.GradingPeriodBeginDate)
                    && !previousStudentGrade.GradeType.Equals(_studentGradeDbRecord.GradeType)
                    && previousStudentGrade.SchoolYear == _studentGradeDbRecord.SchoolYear
                    && previousStudentGrade.GradingPeriodSequence == _studentGradeDbRecord.GradingPeriodSequence)
                    {
                        continue;
                    }
                    previousStudentGrade = _studentGradeDbRecord;

                    if (studentGrade.GradingPeriod.BeginDate > currentSchoolDay)
                    {
                        continue;
                    }

                    var studentSchoolAssociation = studentSchoolAssociationsDictionary[studentGrade.StudentKey].FirstOrDefault(ssa => ssa.OverlapsWith(studentGrade.GradingPeriod));
                    if (studentSchoolAssociation == null)
                    {
                        continue;
                    }

                    var gradingScales = GradingScaleRepository.GetGradingScales(studentSchoolAssociation.LocalEducationAgencyKey.ToString());
                    if (!gradingScales.Any())
                    {
                        continue;
                    }

                    var gradingScale = gradingScales.Where(gradingScale => gradingScale.GradingScaleGradeLevelDims.Any()
                                                                        && gradingScale.GradingScaleGradeLevelDims.Exists(gradingScaleGradeLevelDim => gradingScaleGradeLevelDim.GradeLevelDescription.Equals(studentSchoolAssociation.GradeLevel)))
                                                    .FirstOrDefault();
                    if (gradingScale == null)
                    {
                        continue;
                    }

                    var gradeScaleGradeRank = GradingScaleRepository.GetGradingScaleGrade(gradingScale, _studentGradeDbRecord);
                    if (gradeScaleGradeRank == null)
                    {
                        continue;
                    }

                    studentGrade.GradeRank = gradeScaleGradeRank.Rank;
                    studentGrade.GradingScale = gradingScale;
                    studentGrade.StudentSchoolAssociation = studentSchoolAssociation;

                    if (!AcademicSubjects.Contains(studentGrade.StudentSection.Subject))
                    {
                        continue;
                    }

                    if (!currentSchoolDay.IsInRange(studentGrade.StudentSchoolAssociation)
                     && !studentGrade.GradingScale.GradingScaleMetricThresholdDims.ContainsKey(_metricInfo.Id))
                    {
                        continue;
                    }

                    if (!studentGrade.GradingScale.GradingScaleMetricThresholdDims.ContainsKey(CourseGradeContainerMetric.Id))
                    {
                        continue;
                    }
                    studentGrade.GradingScaleMetricThreshold = studentGrade.GradingScale.GradingScaleMetricThresholdDims[CourseGradeContainerMetric.Id];

                    if (studentGrade.GradingPeriod.ShouldBeIgnored(currentSchoolDay))
                    {
                        continue;
                    }

                    var index = studentCourseGradingPeriodCalculations.IndexOfKey(studentGrade.GradingPeriod);
                    if (index < 0)
                    {
                        continue;
                    }

                    var gradingPeriodData = studentCourseGradingPeriodCalculations.Values[index];
                    var previousGradingPeriodData = (index == 0) ? null : studentCourseGradingPeriodCalculations.Values[index - 1];

                    var studentGradeContainer = gradingPeriodData.AddStudentGrade(studentGrade);

                    var failing = IsFailing(studentGrade, studentGrade.GradingScaleMetricThreshold);
                    if (failing)
                    {
                        gradingPeriodData.NumberOfGradesAtOrBelowThreshold++;
                    }
                    var metricStateType = CourseGradeGranularMetric.GetMetricStateTypeForStudentCourseGrades(gradingPeriodData.NumberOfGradesAtOrBelowThreshold);
                    gradingPeriodData.MetricStateType = metricStateType;

                    CalculateMetricComponentTrend(studentGradeContainer, previousGradingPeriodData);

                    if (index == studentCourseGradingPeriodCalculations.Count - 1)
                    {
                        gradingPeriodData.LastCompletedGradingPeriod = true;
                    }
                }

                studentGradingPeriodsDictionary[studentGradeDbRecord.Key] = studentCourseGradingPeriodCalculations;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentGradingPeriodsDictionary, cacheEntryOptions);

            return studentGradingPeriodsDictionary;
        }
        protected IDictionary<string, List<StudentGradeDim>> GetStudentGradeDbRecords(string schoolKey)
        {
            IDictionary<string, List<StudentGradeDim>> studentGradeDictionary;

            string cacheKey = "StudentAssessmentsDbRecords" + schoolKey;
            if (_memoryCache.TryGetValue(cacheKey, out studentGradeDictionary))
            {
                return studentGradeDictionary;
            }
            studentGradeDictionary = new Dictionary<string, List<StudentGradeDim>>();
            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);
            List<string> studentUniqueIds = studentSchoolAssociationsDictionary.Select(d => d.Key).ToList();

            var studentGradeDbRecords = (from studentGrade in _context.StudentGradeDims.AsNoTracking()
                                         where studentGrade.SchoolKey.Equals(schoolKey) && studentUniqueIds.Contains(studentGrade.StudentKey)
                                         select studentGrade)
                                            .ToList()
                                            .OrderBy(a => a.StudentKey);

            var studentGradeDbRecordsDictionary = studentGradeDbRecords
                                                        .GroupBy(x => x.StudentKey)
                                                        .ToDictionary(x => x.Key, x => x.ToList()
                                                        .OrderBy(a => a.GradingPeriodSequence));



            foreach (var studentUniqueId in studentUniqueIds)
            {
                if (studentGradeDbRecordsDictionary.ContainsKey(studentUniqueId))
                {
                    studentGradeDictionary[studentUniqueId] = studentGradeDbRecordsDictionary[studentUniqueId].ToList();
                }
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1000)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentGradeDictionary, cacheEntryOptions);

            return studentGradeDictionary;
        }
        private List<GradingPeriodDim> GetActiveGradingPeriods(string schoolKey, DateTime currentSchoolDay)
        {
            string cacheKey = "ActiveGradingPeriodsBySchool:" + schoolKey + "CurrentSchoolDay:" + currentSchoolDay.ToString();

            List<GradingPeriodDim> gradingPeriodDims;
            if (_memoryCache.TryGetValue(cacheKey, out gradingPeriodDims))
            {
                return gradingPeriodDims;
            }
            IEnumerable<GradingPeriodDim> dbGradingPeriodDims = _context.GradingPeriodDims
                                                                        .Where(gradingPeriodDim => gradingPeriodDim.SchoolKey.Equals(schoolKey))
                                                                        .ToList();

            foreach (var dbGradingPeriodDim in dbGradingPeriodDims)
            {
                if (!dbGradingPeriodDim.ShouldBeIgnored(currentSchoolDay))
                {
                    gradingPeriodDims.Add(dbGradingPeriodDim);
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, gradingPeriodDims, cacheEntryOptions);

            return gradingPeriodDims;
        }
        protected abstract string[] AcademicSubjects { get; }
        protected abstract GraphQLApi.Common.Metric.CourseGradeMetric CourseGradeGranularMetric { get; }
        protected abstract GraphQLApi.Common.Metric.CourseGradeMetric CourseGradeContainerMetric { get; }
        protected bool IsFailing(StudentGrade studentGrade, GradingScaleMetricThresholdDim threshold)
        {
            return studentGrade.GradeRank <= threshold.Value;
        }
        private static void CalculateMetricComponentTrend(StudentGradeContainer studentGradeContainer, StudentCourseGradingPeriodCalculation previousGradingPeriodData)
        {
            if (previousGradingPeriodData == null)
            {
                return;
            }

            var StudentSection = studentGradeContainer.StudentGrade.StudentSection;
            if (StudentSection == null)
            {
                return;
            }

            var matchedPreviousGrade = GetMatchingStudentGradeContainer(studentGradeContainer, previousGradingPeriodData.StudentGrades);
            if (matchedPreviousGrade == null)
            {
                return;
            }

            var currentRank = studentGradeContainer.StudentGrade.GradeRank;
            var previousRank = matchedPreviousGrade.StudentGrade.GradeRank;

            if (currentRank == previousRank)
            {
                studentGradeContainer.Trend = 0;
            }
            else if (currentRank > previousRank)
            {
                // the trend on the metric component for this metric is calculated as the opposite of the trend
                // for the metric calculation, but both end up as the same result:
                // TrendInterpretation is -1, which means that a TrendDirection of -1 means 'doing better'.
                studentGradeContainer.Trend = -1;
            }
            else
            {
                studentGradeContainer.Trend = 1;
            }
        }
        private static StudentGradeContainer GetMatchingStudentGradeContainer(StudentGradeContainer needle, IEnumerable<StudentGradeContainer> haystack)
        {
            StudentGradeContainer primaryChoice = null;
            StudentGradeContainer secondaryChoice = null;
            foreach (var item in haystack)
            {
                if (item.StudentGrade.StudentSection == needle.StudentGrade.StudentSection)
                {
                    primaryChoice = item;
                }
                else if (secondaryChoice == null
                         && item.StudentGrade.StudentSection.CourseTitle == needle.StudentGrade.StudentSection.CourseTitle)
                {
                    secondaryChoice = item;
                }
            }
            return primaryChoice ?? secondaryChoice;
        }

    }
}
