using AMT.Data.Entities;
using GraphQLApi.Models.Student;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IGradingScaleRepository
    {
        public List<GradingScale> GetGradingScales(string districtKey);
        public GradingScaleGradeDim GetGradingScaleGrade(GradingScale gradingScale, StudentGradeDim studentGrade);
    }
}
