using Data.Models;
using GraphQL.Types;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentParentInformationType : ObjectGraphType<StudentParentInformation>
    {
        public StudentParentInformationType()
        {
            Field(x => x.StudentUsi, type: typeof(IdGraphType));
            Field(x => x.FullName);
            Field(x => x.Relation, true);
            Field(x => x.AddressLine1, nullable: true);
            Field(x => x.AddressLine2, nullable: true);
            Field(x => x.AddressLine3, nullable: true);
            Field(x => x.TelephoneNumber, nullable: true);
            Field(x => x.WorkTelephoneNumber, nullable: true);
            Field(x => x.EmailAddress, nullable: true);
            Field(x => x.PrimaryContact, nullable: true);
        }
    }
}