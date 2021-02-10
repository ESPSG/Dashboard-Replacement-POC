using System.Collections.Generic;

namespace GraphQLApi.Contracts
{
    public interface IStudentAttendanceRepository
    {
        List<Models.StudentMetric> GetGranulars(int studentKey, int schoolKey);

        List<Models.StudentMetric>  GetDaysAbsent(int schoolKey);
    }
}
