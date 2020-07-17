using GraphQL.Types;
using GraphQLApi.Contracts;
using GraphQLApi.GraphQL.Types;
using GraphQLApi.Models;
using System.Collections.Generic;

namespace GraphQLApi.GraphQL.Queries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(IStudentInformationRepository repository, IMetricMetadataRepository metadataRepository)
        {
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
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "studentUsi" }),
                resolve: context =>
                {
                    var studentUsi = context.GetArgument<int?>("studentUsi");
                    if (studentUsi.HasValue)
                    {
                        return repository.GetStudentByUsi(studentUsi.Value);
                    }
                    return null;
                });

            Field<ListGraphType<MetricMetadataNodeType>>(
                "metricMetadata",
                resolve: context =>
                {
                    MetricMetadataTree metadataTree = metadataRepository.GetMetadataTree();
                    return metadataTree.Children;
                });
        }
    }
}