using GraphQLApi.Models.Student;
using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IStudentInformationRepository
    {
        IEnumerable<StudentInformation> GetStudentsBySchool(long schoolId, int? offset, int? limit);
        IEnumerable<StudentInformation> GetDefaultStudents();

        StudentInformation GetStudentByUsi(long studentUniqueId);
        IEnumerable<StudentIndicator> GetIndicators(long studentUniqueId);
        IEnumerable<StudentSchoolInformation> GetSchools(long studentUniqueId);
        IEnumerable<StudentParentInformation> GetParentInformations(long studentUniqueId);
    }
}