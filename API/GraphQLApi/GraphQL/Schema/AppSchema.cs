using GraphQL;
using GraphQLApi.GraphQL.Queries;
using GraphQLTypesSchema = GraphQL.Types.Schema;

namespace GraphQLApi.GraphQL.Schema
{
    public class AppSchema : GraphQLTypesSchema
    {
        public AppSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<AppQuery>();
        }
    }
}
