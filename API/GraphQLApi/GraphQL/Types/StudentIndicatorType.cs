using GraphQL.Authorization;
using GraphQL.Types;
using GraphQLApi.Models.Student;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentIndicatorType : ObjectGraphType<StudentIndicator>
    {
        public StudentIndicatorType()
        {
            this.AuthorizeWith("Authorized");
            Field(x => x.StudentUniqueId, type: typeof(IdGraphType));
            Field(x => x.Type);
            Field(x => x.Name);
            Field(x => x.Status);
            Field(x => x.DisplayOrder, nullable: true);
        }
    }
}