using GraphQL.Types;
using GraphQLApi.Contracts;
using GraphQLApi.GraphQL.Types;
using GraphQLApi.Models;
using System.Collections.Generic;
using GraphQL.Authorization;

namespace GraphQLApi.GraphQL.Queries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(IStudentInformationRepository repository)
        {
            this.AuthorizeWith("Authorized");
            Field<ListGraphType<StudentInformationType>>(
                "students",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "schoolId" }
                                                     , new QueryArgument<IdGraphType> { Name = "offset" }
                                                     , new QueryArgument<IdGraphType> { Name = "limit" }),
                resolve: context =>
                {
                    var schoolId = context.GetArgument<int?>("schoolId");
                    var offset = context.GetArgument<int?>("offset");
                    var limit = context.GetArgument<int?>("limit");
                    if (schoolId.HasValue)
                    {
                        return repository.GetStudentsBySchool(schoolId.Value, offset, limit);
                    }
                    else
                    {
                        return repository.GetDefaultStudents();
                    }
                });


            Field<StudentInformationType>(
                "student",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "studentUniqueId" }),
                resolve: context =>
                {
                    var studentUniqueId = context.GetArgument<int?>("studentUniqueId");
                    if (studentUniqueId.HasValue)
                    {
                        return repository.GetStudentByUsi(studentUniqueId.Value);
                    }
                    return null;
                });


        }
    }
}