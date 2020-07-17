using Data.Models;
using GraphQL.Types;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentIndicatorType : ObjectGraphType<StudentIndicator>
    {
        public StudentIndicatorType()
        {
            Field(x => x.StudentUsi, type: typeof(IdGraphType));
            Field(x => x.Type);
            Field(x => x.Name);
            Field(x => x.Status);
            Field(x => x.DisplayOrder, nullable: true);
        }
    }
}