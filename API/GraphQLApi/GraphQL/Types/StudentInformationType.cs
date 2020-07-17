using Data.Models;
using GraphQL.Types;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.GraphQL.Types
{
    public class StudentInformationType : ObjectGraphType<StudentInformation>
    {
        public StudentInformationType(IStudentInformationRepository studentInformationrepository,
                                      IMetricRepository metricRepository)
        {
            Field(x => x.StudentUsi, type: typeof(IdGraphType));
            Field(x => x.FullName);
            Field(x => x.FirstName);
            Field(x => x.MiddleName, nullable:true);
            Field(x => x.LastSurname);
            Field(x => x.AddressLine1);
            Field(x => x.AddressLine2, nullable: true);
            Field(x => x.AddressLine3, nullable: true);
            Field(x => x.City);
            Field(x => x.State);
            Field(x => x.ZipCode);
            Field(x => x.TelephoneNumber, nullable: true);
            Field(x => x.EmailAddress, nullable: true);
            Field(x => x.DateOfBirth);
            Field(x => x.PlaceOfBirth, nullable: true);
            Field(x => x.CurrentAge, nullable: true);
            Field(x => x.Gender);
            Field(x => x.HispanicLatinoEthnicity, nullable: true);
            Field(x => x.Race, nullable: true);
            Field(x => x.HomeLanguage, nullable: true);
            Field(x => x.Language, nullable: true);
            Field(x => x.ParentMilitary, nullable: true);
            Field(x => x.SchoolId, nullable: true);
            Field(x => x.GradeLevel, nullable: true);
            Field(x => x.GradeLevelListDisplayText, nullable: true);
            Field(x => x.GradeLevelSortOrder, nullable: true);
            Field(x => x.LateEnrollment, nullable: true);
            Field(x => x.SchoolName, nullable: true);
            Field(x => x.SchoolCategory, nullable: true);

            Field<ListGraphType<StudentIndicatorType>>(
            "StudentIndicators",
                resolve: context => studentInformationrepository.GetIndicators(studentUsi: context.Source.StudentUsi).ToList()
             );
            Field<ListGraphType<StudentSchoolInformationType>>(
                "StudentSchoolInformation",
                resolve: context => studentInformationrepository.GetSchools(studentUsi: context.Source.StudentUsi).ToList()
            );
            Field<ListGraphType<StudentParentInformationType>>(
                "StudentParentInformation",
                 resolve: context => studentInformationrepository.GetParentInformations(studentUsi: context.Source.StudentUsi).ToList()
            );
            Field<ListGraphType<StudentMetricType>>(
                "Metrics",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "schoolId" },
                                              new QueryArgument<IdGraphType> { Name = "metricId" },
                                              new QueryArgument<ListGraphType<IntGraphType>> { Name = "metricIds" }),
                resolve: context =>
                {
                    var schoolId = context.GetArgument<int?>("schoolId");
                    if (schoolId.HasValue)
                    {
                        var metricId = context.GetArgument<int?>("metricId");
                        if (metricId.HasValue)
                        {
                            return metricRepository.GetStudentMetricsById(studentUsi: context.Source.StudentUsi, schoolId: schoolId.Value, metricId: metricId).ToList();
                        }
                        var metricIds = context.GetArgument<List<int>>("metricIds");
                        return metricRepository.GetStudentMetricsById(studentUsi: context.Source.StudentUsi, schoolId: schoolId.Value, metricIds: metricIds).ToList();
                    }
                    return null;
                });
        }
    }
}