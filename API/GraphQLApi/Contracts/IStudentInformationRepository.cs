using Data.Models;
using GraphQLApi.Models;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IStudentInformationRepository
    {
        IEnumerable<StudentInformation> GetStudentsBySchool(int schoolId, int? offset, int? limit);
        IEnumerable<StudentInformation> GetDefaultStudents();

        StudentInformation GetStudentByUsi(int studentUsi);
        IEnumerable<StudentIndicator> GetIndicators(int studentUsi);
        IEnumerable<StudentSchoolInformation> GetSchools(int studentUsi);
        IEnumerable<StudentParentInformation> GetParentInformations(int studentUsi);
    }
}