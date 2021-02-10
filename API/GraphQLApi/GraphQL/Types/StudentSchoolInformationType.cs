using GraphQL.Authorization;
using GraphQL.Types;
using GraphQLApi.Models.Student;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentSchoolInformationType : ObjectGraphType<StudentSchoolInformation>
    {
        public StudentSchoolInformationType()
        {
            this.AuthorizeWith("Authorized");
            Field(x => x.StudentUniqueId, type: typeof(IdGraphType));
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