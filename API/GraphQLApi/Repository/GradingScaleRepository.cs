using AMT.Data.Entities;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Models.Student;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLApi.Repository
{
    public class GradingScaleRepository : IGradingScaleRepository
    {
        protected readonly ODSContext _context;
        protected IMemoryCache _memoryCache;

        public GradingScaleRepository(ODSContext oDSContext,
                                        ApiMemoryCache memoryCache)
        {
            _context = oDSContext;
            _memoryCache = memoryCache.Cache;
        }

        public List<GradingScale> GetGradingScales(string districtKey)
        {
            string cacheKey = "GradingScalesByDistrict:" + districtKey;

            if (_memoryCache.TryGetValue(cacheKey, out List<GradingScale> gradingScales))
            {
                return gradingScales;
            }
            gradingScales = new List<GradingScale>();
            var dbGradingScales = _context.GradingScaleDims
                                        .Where(gradingScale => gradingScale.LocalEducationAgencyId.ToString().Equals(districtKey))
                                        .ToList();
            dbGradingScales.ForEach(dbGradingScale =>
            {
                bool isLetterGradingScale;
                bool isNumericGradingScale;
                var gradingScaleGradeDims = GetGradingScaleGrades(dbGradingScale.GradingScaleId, dbGradingScale.GradingScaleName, out isLetterGradingScale, out isNumericGradingScale);
                gradingScales.Add(new GradingScale
                {
                    GradingScaleGradeDims = gradingScaleGradeDims,
                    IsLetterGradingScale = isLetterGradingScale,
                    IsNumericGradingScale = isNumericGradingScale,
                    GradingScaleGradeLevelDims = GetGradingScaleGradeLevels(dbGradingScale.GradingScaleId),
                    GradingScaleMetricThresholdDims = GetGradingScaleMetricThresholds(dbGradingScale.GradingScaleId)
                });
            });

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, gradingScales, cacheEntryOptions);

            return gradingScales;

        }

        private List<GradingScaleGradeLevelDim> GetGradingScaleGradeLevels(int gradingScaleId)
        {
            string cacheKey = "GradingScaleGradeLevelsByGradingScaleId:" + gradingScaleId;

            if (_memoryCache.TryGetValue(cacheKey, out List<GradingScaleGradeLevelDim> gradingScaleGradeLevels))
            {
                return gradingScaleGradeLevels;
            }
            gradingScaleGradeLevels = (from gradingScaleGradeLevelDim in _context.GradingScaleGradeLevelDims
                                       join gradeLevelDim in _context.GradeLevelTypeDims
                                       on gradingScaleGradeLevelDim.GradeLevelTypeId equals gradeLevelDim.GradeLevelTypeId
                                       where gradingScaleGradeLevelDim.GradingScaleId == gradingScaleId
                                       select new GradingScaleGradeLevelDim
                                       {
                                           GradeLevelDescription = gradeLevelDim.CodeValue,
                                           GradeLevelTypeId = gradingScaleGradeLevelDim.GradeLevelTypeId,
                                           GradingScaleGradeLevelId = gradingScaleGradeLevelDim.GradingScaleGradeLevelId,
                                           GradingScaleId = gradingScaleGradeLevelDim.GradingScaleId
                                       }).Distinct().ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, gradingScaleGradeLevels, cacheEntryOptions);

            return gradingScaleGradeLevels;
        }

        private List<GradingScaleGradeDim> GetGradingScaleGrades(int gradingScaleId, string gradingScaleName, out bool isLetterGradingScale, out bool isNumericGradingScale)
        {
            isLetterGradingScale = false;
            isNumericGradingScale = false;

            string cacheKey = "GradingScaleGradesByGradingScaleId:" + gradingScaleId;

            List<GradingScaleGradeDim> gradingScaleGrades;
            if (_memoryCache.TryGetValue(cacheKey, out gradingScaleGrades))
            {
                return gradingScaleGrades;
            }
            var dbGradingScaleGrades = _context.GradingScaleGradeDims
                                                .Where(gradingScaleGradeDim => gradingScaleGradeDim.GradingScaleId == gradingScaleId)
                                                .ToList();
            var localEducationAgencyScaleGrades = dbGradingScaleGrades.OrderBy(x => x.Rank).ToArray();

            if (localEducationAgencyScaleGrades.Any(x => x.Rank == 0))
            {
                var message = string.Format("Grading scale '{0}' has Ranks configured improperly, ranks cannot be zero.", gradingScaleName);
                throw new Exception(message);
            }

            isLetterGradingScale = localEducationAgencyScaleGrades.All(x => x.LetterGrade != null);
            isNumericGradingScale = localEducationAgencyScaleGrades.All(x => x.UpperNumericGrade != null);
            var numericGradingScaleIncomplete = !isNumericGradingScale && localEducationAgencyScaleGrades.Any(x => x.UpperNumericGrade != null);
            var letterGradingScaleIncomplete = !isLetterGradingScale && localEducationAgencyScaleGrades.Any(x => x.LetterGrade != null);

            if (numericGradingScaleIncomplete)
            {
                var message = new StringBuilder();
                foreach (var scaleGrade in localEducationAgencyScaleGrades.Where(x => x.UpperNumericGrade == null))
                {
                    message.AppendFormat("Grading scale '{0}' has UpperNumericGrades configured improperly, for rank {1}.", gradingScaleName, scaleGrade.Rank);
                    message.AppendLine();
                }
                throw new Exception(message.ToString());
            }

            if (letterGradingScaleIncomplete)
            {
                var message = new StringBuilder();
                foreach (var scaleGrade in localEducationAgencyScaleGrades.Where(x => x.LetterGrade == null))
                {
                    message.AppendFormat("Grading scale '{0}' has LetterGrades configured improperly, for rank {1}.", gradingScaleName, scaleGrade.Rank);
                    message.AppendLine();
                }
                throw new Exception(message.ToString());
            }

            if (!isLetterGradingScale && !isNumericGradingScale)
            {
                throw new Exception(string.Format("Cannot determine if grading scale '{0}' is numeric or letter grade.", gradingScaleName));
            }

            var errorMessage = new StringBuilder();
            if (isNumericGradingScale)
            {
                GradingScaleGradeDim previousGradingScaleGrade = null;
                foreach (var gradingScaleGrade in localEducationAgencyScaleGrades)
                {
                    if (previousGradingScaleGrade != null && previousGradingScaleGrade.UpperNumericGrade >= gradingScaleGrade.UpperNumericGrade)
                    {
                        errorMessage.AppendFormat("Grading scale '{0}' has Ranks and UpperNumericGrades configured improperly, for ranks {1} and {2}.",
                            gradingScaleName, previousGradingScaleGrade.Rank, gradingScaleGrade.Rank);
                        errorMessage.AppendLine();
                    }
                    previousGradingScaleGrade = gradingScaleGrade;
                }
            }

            if (isLetterGradingScale)
            {
                var orderedByLetterGrades = localEducationAgencyScaleGrades.OrderBy(x => x.LetterGrade);
                GradingScaleGradeDim previousGradingScaleLetterGrade = null;
                foreach (var gradingScaleLetterGrade in orderedByLetterGrades)
                {
                    if (previousGradingScaleLetterGrade != null && previousGradingScaleLetterGrade.LetterGrade == gradingScaleLetterGrade.LetterGrade)
                    {
                        errorMessage.AppendFormat("Grading scale '{0}' has LetterGrades configured improperly, for ranks {1} and {2}.",
                            gradingScaleName, previousGradingScaleLetterGrade.Rank, gradingScaleLetterGrade.Rank);
                        errorMessage.AppendLine();
                    }
                    previousGradingScaleLetterGrade = gradingScaleLetterGrade;
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }

            gradingScaleGrades = localEducationAgencyScaleGrades.ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, gradingScaleGrades, cacheEntryOptions);

            return gradingScaleGrades;

        }

        private IDictionary<int, GradingScaleMetricThresholdDim> GetGradingScaleMetricThresholds(int gradingScaleId)
        {
            string cacheKey = "GradingScaleMetricThresholdsByGradingScaleId:" + gradingScaleId;

            IDictionary<int, GradingScaleMetricThresholdDim> gradingScaleMetricThresholds;
            if (_memoryCache.TryGetValue(cacheKey, out gradingScaleMetricThresholds))
            {
                return gradingScaleMetricThresholds;
            }
            IEnumerable<GradingScaleMetricThresholdDim> localEducationAgencyGradingScaleMetricThresholds = _context.GradingScaleMetricThresholdDims
                                                                                                            .Where(gradingScaleGradeDim => gradingScaleGradeDim.GradingScaleId == gradingScaleId)
                                                                                                            .ToList();
            gradingScaleMetricThresholds = new SortedDictionary<int, GradingScaleMetricThresholdDim>();
            foreach (var localEducationAgencyGradingScaleMetricThreshold in localEducationAgencyGradingScaleMetricThresholds)
            {
                gradingScaleMetricThresholds.Add(localEducationAgencyGradingScaleMetricThreshold.MetricId, localEducationAgencyGradingScaleMetricThreshold);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(50)//Size amount
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(cacheKey, gradingScaleMetricThresholds, cacheEntryOptions);

            return gradingScaleMetricThresholds;
        }

        public GradingScaleGradeDim GetGradingScaleGrade(GradingScale gradingScale, StudentGradeDim studentGrade)
        {
            if (!gradingScale.IsNumericGradingScale && !gradingScale.IsLetterGradingScale)
            {
                return null;
            }

            if (gradingScale.IsNumericGradingScale && studentGrade.NumericGradeEarned != null)
            {
                var highestGradingScale = gradingScale.GradingScaleGradeDims.Last();
                if (highestGradingScale.UpperNumericGrade <= studentGrade.NumericGradeEarned)
                {
                    return highestGradingScale;
                }

                return gradingScale.GradingScaleGradeDims.FirstOrDefault(x => x.UpperNumericGrade >= studentGrade.NumericGradeEarned);
            }

            return gradingScale.GradingScaleGradeDims.FirstOrDefault(x => string.Equals(x.LetterGrade, studentGrade.LetterGradeEarned, StringComparison.OrdinalIgnoreCase));
        }
    }
}
