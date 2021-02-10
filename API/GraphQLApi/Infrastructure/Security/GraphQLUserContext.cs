using GraphQL.Authorization;
using System.Security.Claims;

namespace GraphQLApi.Infrastructure.Security
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
