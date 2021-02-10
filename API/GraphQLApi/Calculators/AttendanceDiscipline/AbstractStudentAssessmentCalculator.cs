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
    public abstract class AbstractStudentAssessmentCalculator : AbstractStudentMetricCalculator
    {
        public AbstractStudentAssessmentCalculator(CalculatorAppContext appContext) : base(appContext) { }
        protected abstract IDictionary<string, StudentAssessmentDim> GetStudentAssessments(string schoolKey);
        protected IDictionary<string, StudentAssessment> GetStudentAssessmentsDbRecords(string schoolKey)
        {
            IDictionary<string, StudentAssessment> studentAssessmentDictionary;

            string cacheKey = "StudentAssessmentsDbRecords" + schoolKey;
            if (_memoryCache.TryGetValue(cacheKey, out studentAssessmentDictionary))
            {
                return studentAssessmentDictionary;
            }

            studentAssessmentDictionary = new Dictionary<string, StudentAssessment>();

            IDictionary<string, List<StudentSchoolDim>> studentSchoolAssociationsDictionary = StudentDataRepository.GetStudentSchoolAssociationsDictionary(schoolKey);

            List<string> studentUniqueIds = studentSchoolAssociationsDictionary.Select(d => d.Key).ToList();

            var studentAssessmentDbRecords = (from studentAssessment in _context.StudentAssessmentDims.AsNoTracking()
                                              where studentAssessment.SchoolKey.Equals(schoolKey) && studentUniqueIds.Contains(studentAssessment.StudentKey)
                                              select studentAssessment)
                                            .ToList()
                                            .OrderBy(a => a.StudentKey);

            var studentAssessmentDbRecordsDictionary = studentAssessmentDbRecords
                                                        .GroupBy(x => x.StudentKey)
                                                        .ToDictionary(x => x.Key, x => x.ToList()
                                                        .OrderBy(a => a.AdministrationDate));

            var studentAssessmentScoreResultsDbRecords = (from sasrd in _context.StudentAssessmentScoreResultDims.AsNoTracking()
                                                          where sasrd.SchoolKey.Equals(schoolKey) && studentUniqueIds.Contains(sasrd.StudentKey)
                                                          select sasrd)
                                                        .ToList()
                                                        .OrderBy(a => a.StudentKey);

            var studentAssessmentScoreResultsDbRecordsDictionary = studentAssessmentScoreResultsDbRecords
                                                                    .GroupBy(x => x.StudentKey)
                                                                    .ToDictionary(x => x.Key, x => x.ToList()
                                                                    .OrderBy(a => a.AdministrationDate));

            var studentAssessmentItemsDbRecords = (from sasrd in _context.StudentAssessmentItemDims.AsNoTracking()
                                                   where sasrd.SchoolKey.Equals(schoolKey) && studentUniqueIds.Contains(sasrd.StudentKey)
                                                   select sasrd)
                                                        .ToList()
                                                        .OrderBy(a => a.StudentKey);

            var studentAssessmentItemsDbRecordsDictionary = studentAssessmentItemsDbRecords
                                                                    .GroupBy(x => x.StudentKey)
                                                                    .ToDictionary(x => x.Key, x => x.ToList()
                                                                    .OrderBy(a => a.AdministrationDate));


            foreach (var studentUniqueId in studentUniqueIds)
            {
                var studentAssessment = new StudentAssessment { StudentKey = studentUniqueId };
                if (studentAssessmentDbRecordsDictionary.ContainsKey(studentUniqueId))
                {
                    studentAssessment.Assessments = studentAssessmentDbRecordsDictionary[studentUniqueId].ToList();
                }
                if (studentAssessmentScoreResultsDbRecordsDictionary.ContainsKey(studentUniqueId))
                {
                    studentAssessment.ScoreResults = studentAssessmentScoreResultsDbRecordsDictionary[studentUniqueId].ToList();
                }
                if (studentAssessmentItemsDbRecordsDictionary.ContainsKey(studentUniqueId))
                {
                    studentAssessment.AssessmentItems = studentAssessmentItemsDbRecordsDictionary[studentUniqueId].ToList();
                }
                studentAssessmentDictionary[studentUniqueId] = studentAssessment;
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1000)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKey, studentAssessmentDictionary, cacheEntryOptions);

            return studentAssessmentDictionary;
        }
        protected abstract string AssessmentReportingMethodType { get; }
        protected abstract string[] AcademicSubjects { get; }
        protected abstract string[] AssessmentCategories { get; }
    }
}