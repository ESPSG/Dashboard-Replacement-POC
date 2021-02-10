export const defaultStudentUniqueId = 100110233;

export const studentProfileQuery = (studentUniqueId: number | string) => {
  return `query{
    student(studentUniqueId:${studentUniqueId})
      { fullName,
        addressLine1,
        addressLine2,
        addressLine3,
        city,
        state,
        zipCode,
        telephoneNumber,
        emailAddress,
        dateOfBirth,
        placeOfBirth,
        currentAge,
        gender,
        hispanicLatinoEthnicity,
        race,
        homeLanguage,
        language,
        parentMilitary
        studentUniqueId
        studentParentInformation {
          fullName,
          relation,
          homeAddress,
          physicalAddress,
          mailingAddress,
          telephoneNumber,
          workTelephoneNumber,
          emailAddress,
          isPrimaryContact
        }
        studentSchoolInformation {
          gradeLevel,
          lateEnrollment,
          homeroom,
          dateOfEntry,
          dateOfWithdrawal,
          graduationPlan,
          expectedGraduationYear
        },
        studentIndicators {
          type,
          name,
          status,
          displayOrder,
        }
    }
  }`
}

export const allStudentMetricsQuery = () => {
  return `query {
    students(schoolId:867530011, limit: 1)
      {
        metrics(schoolId:867530011, metricId: 65)
        {
          id,
          name,
          value,
          state,
          trendDirection,
          parentId,
          parentName,
        }
      }
  }`
}

export const studentMetricsQuery = (metricIds: number[], limit?: number) => {
  return `query {
    students(schoolId:867530011${limit ? `, limit:${limit}` : ""})
      { fullName,
        studentUniqueId,
        gradeLevel,
        gradeLevelSortOrder,
        schoolName,
        schoolCategory
        metrics(schoolId:867530011, metricIds:[${metricIds.join(", ")}])
        {
          id,
          name,
          value,
          state,
          trendDirection,
          parentId,
          parentName,
        }
      }
  }`
}

export const metricMetadataQuery = () => {
  return `query{
    metricMetadata{
      description displayName domainEntityType displayOrder metricId name url
      children{
        description displayName domainEntityType displayOrder metricId name url
        children{
          description displayName domainEntityType displayOrder metricId name url
          children{
            description displayName domainEntityType displayOrder metricId name url
            children{
              description displayName domainEntityType displayOrder metricId name url
              children{
                description displayName domainEntityType displayOrder metricId name url
                children{
                  description displayName domainEntityType displayOrder metricId name url
                }
              }
            }
          }
        }
      }
    }
  }`
}