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
    public abstract class StudentStateAssessmentPerformanceCalculator : AbstractStudentAssessmentCalculator
    {
        public StudentStateAssessmentPerformanceCalculator(CalculatorAppContext appContext) : base(appContext) { }

        protected override IDictionary<string, StudentAssessmentDim> GetStudentAssessments(string schoolKey)
        {
            IDictionary<string, StudentAssessmentDim> studentAssessmentsDictionary;

            string cacheKey = "StudentSchoolAssessments" + schoolKey + " MetricId:" + GetMetricId();
            if (_memoryCache.TryGetValue(cacheKey, out studentAssessmentsDictionary))
            {
                return studentAssessmentsDictionary;
            }
            studentAssessmentsDictionary = new Dictionary<string, StudentAssessmentDim>();

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
                var value = string.Empty;

                if (studentAssessment != null)
                {
                    var metricStateType = studentAssessment.PerformanceLevelMet.HasValue && studentAssessment.PerformanceLevelMet.Value ? MetricStateType.Good.Value : MetricStateType.Bad.Value;
                    metricState = MetricUtility.GetMetricState(metricStateType);
                    var scoreResult = studentAssessment.ScoreResults.Single(x => x.ReportingMethod.Equals(AssessmentReportingMethodType));
                    value = scoreResult.Result;
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
                    ValueTypeName = "System.Int32",
                    State = metricState,
                    TrendDirection = null,
                });
            }
            _memoryCache.Set(cacheKey, returnMap, _cacheEntryOptions);
            return returnMap;
        }

        protected override string AssessmentReportingMethodType => AssessmentReportingMethodDescriptor.RawScore;
    }

    #region [ State Assessnent Performance Caluclators ]

    public class StudentStateAssessmentPerformanceReadingCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentStateAssessmentPerformanceReadingCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentStateAssessmentElaReading;
        }

        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading
                                                                     , AcademicSubjectDescriptor.EnglishLanguageArts };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.StateSummativeAssessment3To8General
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolSubjectAssessment
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolCourseAssessment };
    }

    public class StudentStateAssessmentPerformanceMathematicsCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentStateAssessmentPerformanceMathematicsCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentStateAssessmentMathematics;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Mathematics };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.StateSummativeAssessment3To8General
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolSubjectAssessment
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolCourseAssessment };
    }

    public class StudentStateAssessmentPerformanceScienceCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentStateAssessmentPerformanceScienceCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentStateAssessmentScience;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Science };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.StateSummativeAssessment3To8General
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolSubjectAssessment
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolCourseAssessment };
    }

    public class StudentStateAssessmentPerformanceSocialStudiesCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentStateAssessmentPerformanceSocialStudiesCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentStateAssessmentSocialStudies;
        }
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.SocialStudies };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.StateSummativeAssessment3To8General
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolSubjectAssessment
                                                                           , AssessmentCategoryDescriptor.StateHighSchoolCourseAssessment };
    }

    #endregion

    #region [ Language Assessment Proficiency Calculator ]

    public class StudentLanguageAssessmentProficiencyReadingAssessmentCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentLanguageAssessmentProficiencyReadingAssessmentCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLanguageAssessmentProficiency;
        }

        protected override string AssessmentReportingMethodType => AssessmentReportingMethodDescriptor.LexileMeasure;
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.LanguageProficiencyTest};

    }

    #endregion

    #region [ Reading Assessment Calculator ]
    public class StudentLocalAssessmentReadingAssessmentCalculator : StudentStateAssessmentPerformanceCalculator
    {
        public StudentLocalAssessmentReadingAssessmentCalculator(CalculatorAppContext appContext) : base(appContext)
        {
            _metricInfo = Common.Metric.StudentLocalAssessmentReadingAssessment;
        }

        protected override string AssessmentReportingMethodType => AssessmentReportingMethodDescriptor.LexileMeasure;
        protected override string[] AcademicSubjects => new string[] { AcademicSubjectDescriptor.Reading };
        protected override string[] AssessmentCategories => new string[] {   AssessmentCategoryDescriptor.ReadingReadinessTest
                                                                            ,AssessmentCategoryDescriptor.PrekindergartenReadiness};

    }

    #endregion

}
