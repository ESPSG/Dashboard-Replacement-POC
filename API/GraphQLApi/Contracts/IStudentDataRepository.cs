using AMT.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.Contracts
{
    public interface IStudentDataRepository
    {
        #region School
        public IDictionary<string, List<StudentSchoolDim>> GetStudentSchoolAssociationsDictionary(string schoolKey);
        public List<SchoolCalendarDim> GetSchoolCalendarDays(string schoolKey);
        public IDictionary<string, List<StudentSectionDim>> GetStudentSectionAssociationsDictionary(string schoolKey);
        public List<GradingPeriodDim> GetSchoolGradingPeriods(string schoolKey);
        #endregion
    }
}
