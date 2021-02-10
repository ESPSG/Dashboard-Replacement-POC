using AMT.Data.Descriptors;
using AMT.Data.Entities;
using System;
using System.Linq;

namespace AMT.Data.Common
{
    public static class StudentCourseGradeHelper
    {
        private static string[] RollupGradingPeriodDescriptors => new string[] {    GradingPeriodDescriptor.FirstSemester,
                                                                                    GradingPeriodDescriptor.SecondSemester,
                                                                                    GradingPeriodDescriptor.EndOfYear
                                                                               };

        public static bool ShouldBeIgnored(this GradingPeriodDim gradingPeriod, DateTime currentSchoolDay)
        {
            return gradingPeriod == null
                   || gradingPeriod.EndDate > currentSchoolDay
                   || gradingPeriod.PeriodSequence == null
                   || RollupGradingPeriodDescriptors.Contains(gradingPeriod.GradingPeriodDescription);
        }
    }
}
