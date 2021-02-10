using GraphQL.Authorization;
using GraphQL.Types;
using GraphQLApi.Models.Student;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentParentInformationType : ObjectGraphType<StudentParentInformation>
    {
        public StudentParentInformationType()
        {
            this.AuthorizeWith("Authorized");
            Field(x => x.StudentUniqueId, type: typeof(IdGraphType));
            Field(x => x.FullName);
            Field(x => x.Relation, true);
            Field(x => x.HomeAddress, nullable: true);
            Field(x => x.PhysicalAddress, nullable: true);
            Field(x => x.MailingAddress, nullable: true);
            Field(x => x.WorkAddress, nullable: true);
            Field(x => x.TemporaryAddress, nullable: true);
            Field(x => x.TelephoneNumber, nullable: true);
            Field(x => x.WorkTelephoneNumber, nullable: true);
            Field(x => x.EmailAddress, nullable: true);
            Field(x => x.IsPrimaryContact, nullable: true);
        }
    }
}