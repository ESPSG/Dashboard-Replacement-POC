using GraphQL.Authorization;
using GraphQL.Types;
using GraphQLApi.Models;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentMetricType : ObjectGraphType<StudentMetric>
    {
        public StudentMetricType()
        {
            this.AuthorizeWith("Authorized");
            Field(x => x.StudentUsi, type: typeof(IdGraphType));
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Value, nullable: true);
            Field(x => x.State, nullable: true);
            Field(x => x.Type);
            Field(x => x.TrendDirection, nullable: true);
            Field(x => x.ParentId, nullable: true);
            Field(x => x.ParentName, nullable: true);
        }
    }
}