using Data.Models;
using GraphQL.Types;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentSchoolInformationType : ObjectGraphType<StudentSchoolInformation>
    {
        public StudentSchoolInformationType()
        {
            Field(x => x.StudentUsi, type: typeof(IdGraphType));
            Field(x => x.SchoolId);
            Field(x => x.GradeLevel);
            Field(x => x.Homeroom, true);
            Field(x => x.LateEnrollment, nullable: true);
            Field(x => x.DateOfEntry, nullable: true);
            Field(x => x.DateOfWithdrawal, nullable: true);
            Field(x => x.GraduationPlan, nullable: true);
            Field(x => x.ExpectedGraduationYear, nullable: true);
        }
    }
}