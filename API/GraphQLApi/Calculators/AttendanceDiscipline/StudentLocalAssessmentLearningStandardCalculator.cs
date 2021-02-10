using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public abstract class StudentLocalAssessmentCalculator : AbstractStudentAssessmentCalculator
    {
        public static string DISPLAY_FORMAT_BENCHMARK_ASSESSMENT = "{0:P1}";
        public StudentLocalAssessmentCalculator(CalculatorAppContext appContext) : base(appContext) { }
    }

    #region [ Benchmark Assessment Mastery Calculators ]

    public abstract class StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator : StudentLocalAssessmentCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected override IDictionary<string, StudentAssessmentDim> GetStudentAssessments(string schoolKey)
        {
            IDictionary<string, StudentAssessmentDim> studentAssessmentsDictionary;

            string cacheKey = "StudentSchoolAssessments" + schoolKey + "MetricId:" + GetMetricId();
            if (_memoryCache.TryGetValue(cacheKey, out studentAssessmentsDictionary))
            {
                return studentAssessmentsDictionary;
            }
            studentAssessmentsDictionary = new Dictionary<string, StudentAssessmentDim>();

            var schoolDays = StudentDataRepository.GetSchoolCalendarDays(schoolKey).Select(calendarDay => calendarDay.Date).ToList();

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey.ToString())).FirstOrDefault();
            if (schoolMinMaxDateDim == null)
            {
                return studentAssessmentsDictionary;
            }

            var studentAssessmentsDbRecords = GetStudentAssessmentsDbRecords(schoolKey);
            foreach (var studentAssessmentDbRecord in studentAssessmentsDbRecords)
            {
                if (studentAssessmentDbRecord.Value.Assessments.Any())
                {
                    List<StudentAssessmentDim> studentAssessmentDims = new List<StudentAssessmentDim>();
                    foreach (var assessmentDim in studentAssessmentDbRecord.Value.Assessments)
                    {
                        if (assessmentDim.AdministrationDate > schoolMinMaxDateDim.MaxDate)
                            continue;

                        if (!(AcademicSubjects.Contains(assessmentDim.AcademicSubject) && AssessmentCategories.Contains(assessmentDim.AssessmentCategory)))
                            continue;

                        if (!studentAssessmentDbRecord.Value.ScoreResults.Any())
                        {
                            continue;
                        }
                        var scoreResults = studentAssessmentDbRecord.Value.ScoreResults.Where(sr => sr.StudentAssessmentIdentifier.Equals(assessmentDim.StudentAssessmentIdentifier, StringComparison.Ordinal)
                                                                                                    && sr.AssessmentIdentifier.Equals(assessmentDim.AssessmentIdentifier, StringComparison.Ordinal)
                                                                                                    && sr.Namespace.Equals(assessmentDim.Namespace, StringComparison.Ordinal)
                                                                                                    && sr.ReportingMethod.Equals(AssessmentReportingMethodType, StringComparison.Ordinal))
                                                                                            .ToList();
                        if (!scoreResults.Any())
                            continue;

                        assessmentDim.ScoreResults = scoreResults;
                        studentAssessmentDims.Add(assessmentDim);

                    }
                    if (studentAssessmentDims.Any())
                    {
                        var mostRecentAssessment = studentAssessmentDims.Where(assessment => string.IsNullOrEmpty(assessment.ReasonNotTested))
                                                                        .OrderByDescending(assessment => assessment.AdministrationDate).FirstOrDefault();
                        if (mostRecentAssessment != null)
                        {
                            var numerator = mostRecentAssessment.ScoreResults.Single(x => x.ReportingMethod.Equals(AssessmentReportingMethodType)).Result.ToNullableDecimal();
                            var assessmentRatio = numerator.DivideBySafeAndRound(mostRecentAssessment.MaxScoreResult);
                            if (assessmentRatio == null)
                            {
                                continue;
                            }
                            mostRecentAssessment.BenchmarkAssessmentRatio = assessmentRatio;
                            studentAssessmentsDictionary[mostRecentAssessment.StudentKey] = mostRecentAssessment;
                        }
                    }
                    else
                    {
                        studentAssessmentsDictionary[studentAssessmentDbRecord.Key] = null;
                    }
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentAssessmentsDictionary, cacheEntryOptions);

            return studentAssessmentsDictionary;
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

            var studentMostRecentAssessments = GetStudentAssessments(schoolKey);
            foreach (var studentMostRecentAssessment in studentMostRecentAssessments)
            {
                var studentKey = studentMostRecentAssessment.Key;
                var studentAssessment = studentMostRecentAssessment.Value;

                var metricState = string.Empty;
                decimal? ratio = null;

                if (studentAssessment != null && studentAssessment.BenchmarkAssessmentRatio != decimal.MaxValue)
                {
                    ratio = studentAssessment.BenchmarkAssessmentRatio;
                    if (ratio != null)
                    {
                        var metricStateType = (ratio.Value >= _metricInfo.DefaultGoal) ? MetricStateType.Good.Value : MetricStateType.Bad.Value;
                        metricState = MetricUtility.GetMetricState(metricStateType);
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
                    Value = !ratio.HasValue ? "" : ratio.Display().DisplayValue(DISPLAY_FORMAT_BENCHMARK_ASSESSMENT),
                    ValueTypeName = "System.Double",
                    State = metricState,
                    TrendDirection = null,
                });
            }
            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected override string AssessmentReportingMethodType => AssessmentReportingMethodDescriptor.RawScore;
    }

    public class StudentLocalAssessmentBenchmarkAssessmentMasteryWritingCalculator : StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasteryWritingCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentBenchmarkAssessmentMasteryWriting;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Writing };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentBenchmarkAssessmentMasteryReadingElaCalculator : StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasteryReadingElaCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentBenchmarkAssessmentMasteryElaReading;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading
                                                                     , AcademicSubjectDescriptor.EnglishLanguageArts };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentBenchmarkAssessmentMasteryMathematicsCalculator : StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasteryMathematicsCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentBenchmarkAssessmentMasteryMathematics;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Mathematics };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentBenchmarkAssessmentMasteryScienceCalculator : StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasteryScienceCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentBenchmarkAssessmentMasteryScience;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Science };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentBenchmarkAssessmentMasterySocialStudiesCalculator : StudentLocalAssessmentBenchmarkAssessmentMasteryCalculator
    {
        public StudentLocalAssessmentBenchmarkAssessmentMasterySocialStudiesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentBenchmarkAssessmentMasterySocialStudies;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.SocialStudies };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    #endregion

    #region [ Learning Standard Mastery By Core Subject Area Calculators ]

    public abstract class StudentLocalAssessmentLearningStandardMasteryCalculator : StudentLocalAssessmentCalculator
    {
        public StudentLocalAssessmentLearningStandardMasteryCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected override IDictionary<string, StudentAssessmentDim> GetStudentAssessments(string schoolKey)
        {
            IDictionary<string, StudentAssessmentDim> studentAssessmentsDictionary;

            string cacheKey = "StudentSchoolAssessments" + schoolKey + "MetricId:" + GetMetricId();
            if (_memoryCache.TryGetValue(cacheKey, out studentAssessmentsDictionary))
            {
                return studentAssessmentsDictionary;
            }
            studentAssessmentsDictionary = new Dictionary<string, StudentAssessmentDim>();

            var schoolDays = StudentDataRepository.GetSchoolCalendarDays(schoolKey).Select(calendarDay => calendarDay.Date).ToList();

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(schoolKey.ToString())).FirstOrDefault();
            if (schoolMinMaxDateDim == null)
            {
                return studentAssessmentsDictionary;
            }

            var studentAssessmentsDbRecords = GetStudentAssessmentsDbRecords(schoolKey);
            foreach (var studentAssessmentDbRecord in studentAssessmentsDbRecords)
            {
                if (studentAssessmentDbRecord.Value.Assessments.Any())
                {
                    StudentAssessmentDim studentAssessment = new StudentAssessmentDim { AssessmentCorrectItems = 0, AssessmentTotalItems = 0 };
                    foreach (var assessmentDim in studentAssessmentDbRecord.Value.Assessments)
                    {
                        if (assessmentDim.AdministrationDate > schoolMinMaxDateDim.MaxDate)
                            continue;

                        if (!schoolDays.Contains(assessmentDim.AdministrationDate))
                            continue;

                        if (!(AcademicSubjects.Contains(assessmentDim.AcademicSubject) && AssessmentCategories.Contains(assessmentDim.AssessmentCategory)))
                            continue;

                        if (!studentAssessmentDbRecord.Value.ScoreResults.Any())
                        {
                            continue;
                        }
                        if (!studentAssessmentDbRecord.Value.AssessmentItems.Any())
                        {
                            continue;
                        }
                        var assessmentItems = studentAssessmentDbRecord.Value.AssessmentItems.Where(sr => sr.StudentAssessmentIdentifier.Equals(assessmentDim.StudentAssessmentIdentifier, StringComparison.Ordinal)
                                                                                                   && sr.AssessmentIdentifier.Equals(assessmentDim.AssessmentIdentifier, StringComparison.Ordinal)
                                                                                                   && sr.Namespace.Equals(assessmentDim.Namespace, StringComparison.Ordinal))
                                                                                           .ToList();
                        if (!assessmentItems.Any())
                            continue;

                        studentAssessment.AssessmentTotalItems += assessmentItems.Count();
                        studentAssessment.AssessmentCorrectItems += assessmentItems.Where(ai => AssessmentItemResults.Contains(ai.AssessmentItemResult)).Count();
                    }

                    studentAssessmentsDictionary[studentAssessmentDbRecord.Key] = studentAssessment;
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(200)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentAssessmentsDictionary, cacheEntryOptions);

            return studentAssessmentsDictionary;
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

            var studentAssessments = GetStudentAssessments(schoolKey);
            foreach (var studentAssessment in studentAssessments)
            {
                var studentKey = studentAssessment.Key;
                var studentAssessmentDim = studentAssessment.Value;

                var metricState = string.Empty;
                decimal? ratio = null;

                if (studentAssessmentDim != null)
                {
                    ratio = studentAssessmentDim.AssessmentCorrectItems.DivideBySafeAndRound(studentAssessmentDim.AssessmentTotalItems);
                    if (ratio != null)
                    {
                        var metricStateType = (ratio.Value >= _metricInfo.DefaultGoal) ? MetricStateType.Good.Value : MetricStateType.Bad.Value;
                        metricState = MetricUtility.GetMetricState(metricStateType);
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
                    Value = !ratio.HasValue ? "" : ratio.Display().DisplayValue(DISPLAY_FORMAT_BENCHMARK_ASSESSMENT),
                    ValueTypeName = "System.Double",
                    State = metricState,
                    TrendDirection = null,
                });
            }
            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected override string AssessmentReportingMethodType => AssessmentReportingMethodDescriptor.InternationalBaccalaureateScore;
        private string[] AssessmentItemResults => new string[] {   AssessmentItemResultDescriptor.AboveStandard
                                                                           , AssessmentItemResultDescriptor.MetStandard
                                                                           , AssessmentItemResultDescriptor.Correct };

    }

    public class StudentLocalAssessmentLearningStandardMasteryReadingElaCalculator : StudentLocalAssessmentLearningStandardMasteryCalculator
    {
        public StudentLocalAssessmentLearningStandardMasteryReadingElaCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentLearningStandardMasteryElaReading;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading
                                                                     , AcademicSubjectDescriptor.EnglishLanguageArts };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentLearningStandardMasteryMathematicsCalculator : StudentLocalAssessmentLearningStandardMasteryCalculator
    {
        public StudentLocalAssessmentLearningStandardMasteryMathematicsCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentLearningStandardMasteryMathematics;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Mathematics };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentLearningStandardMasteryScienceCalculator : StudentLocalAssessmentLearningStandardMasteryCalculator
    {
        public StudentLocalAssessmentLearningStandardMasteryScienceCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentLearningStandardMasteryScience;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Science };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    public class StudentLocalAssessmentLearningStandardMasterySocialStudiesCalculator : StudentLocalAssessmentLearningStandardMasteryCalculator
    {
        public StudentLocalAssessmentLearningStandardMasterySocialStudiesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentLearningStandardMasterySocialStudies;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.SocialStudies };
        protected override string[] AssessmentCategories => new string[] { AssessmentCategoryDescriptor.BenchmarkTest };
    }

    #endregion
}
