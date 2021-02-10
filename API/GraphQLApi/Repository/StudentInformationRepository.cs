using AMT.Data.Common;
using AMT.Data.Entities;
using GraphQLApi.Contracts;
using GraphQLApi.Enumerations;
using GraphQLApi.Infrastructure.Security;
using GraphQLApi.Models.Student;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GraphQLApi.Repository
{
    public class StudentInformationRepository : IStudentInformationRepository
    {
        private readonly ODSContext _context;
        private readonly AppUserSecurityContext _appUserContext;

        public StudentInformationRepository(ODSContext context, AppUserSecurityContext appUserContext)
        {
            _context = context;
            _appUserContext = appUserContext;
        }

        public IEnumerable<StudentInformation> GetStudentsBySchool(long schoolId, int? offset, int? limit)
        {
            if(!_appUserContext.IsDistrictAdministrator && !_appUserContext.IsSchoolAdministrator && !_appUserContext.IsTeacher)
            {
                return new List<StudentInformation>();
            }
            var students = (from si in _context.StudentDims.AsNoTracking()
                            join ssi in _context.StudentSchoolDims.AsNoTracking()
                                on si.StudentKey equals ssi.StudentKey
                            where ssi.SchoolKey.Equals(schoolId.ToString())
                            select new StudentInformation
                            {
                                StudentUniqueId = Convert.ToInt32(si.StudentKey),
                                FirstName = si.FirstName,
                                MiddleName = si.MiddleName,
                                LastSurname = si.LastSurname,
                                FullName = si.FullName,
                                Gender = si.Gender,
                                Race = si.Races,
                                HispanicLatinoEthnicity = si.HispanicLatinoEthnicity.HasValue ? si.HispanicLatinoEthnicity.Value.ToString() : null,
                                SchoolId = si.SchoolKey,
                                GradeLevel = si.GradeLevel,
                                GradeLevelSortOrder = si.SortOrder,
                                GradeLevelListDisplayText = si.GradeLevelDisplayText,
                                SchoolName = si.SchoolName,
                                SchoolCategory = si.SchoolCategory,
                                AddressLine1 = si.AddressLine1,
                                AddressLine2 = si.AddressLine2,
                                AddressLine3 = si.AddressLine3,
                                City = si.AddressCity,
                                DateOfBirth = si.BirthDate.HasValue ? si.BirthDate.Value : DateTime.MinValue
                            })
                            .ToList();
            
            if (_appUserContext.IsSchoolAdministrator)
            {
                students = (from student in students
                            join uad in _appUserContext.UserAuthorizationDims
                                on student.SchoolId equals uad.SchoolPermission
                            select student
                            )
                            .ToList();
            }
            if (_appUserContext.IsTeacher)
            {

                //All Student Sections for a given SchoolId
                var schoolIdAsStr = schoolId.ToString();
                var studentSectionEnrollments = _context.StudentSectionDims.Where(s => s.SchoolKey.Equals(schoolIdAsStr)).ToList();

                //Filter the student section enrollments by the valid sections for the Teacher.
                var validSectionEnrollments = (from sse in studentSectionEnrollments
                                               join uad in _appUserContext.UserAuthorizationDims
                                               on sse.SectionId equals uad.SectionPermission
                                               select sse)
                                     .Distinct()
                                     .ToList();

                //Filter the School Student List by the valid section enrollments.
                students = students.Where(s => validSectionEnrollments.Any(sse => sse.StudentKey.Equals(s.StudentUniqueId.ToString()))).ToList();
            }


            return students;
        }

        public IEnumerable<StudentInformation> GetDefaultStudents()
        {
            var students = (from si in _context.StudentDims.AsNoTracking()
                            join ssi in _context.StudentSchoolDims.AsNoTracking()
                                on si.StudentKey equals ssi.StudentKey
                            orderby si.SortOrder ascending
                            select new StudentInformation
                            {
                                StudentUniqueId = Convert.ToInt32(si.StudentKey),
                                FirstName = si.FirstName,
                                MiddleName = si.MiddleName,
                                LastSurname = si.LastSurname,
                                FullName = si.FullName,
                                Gender = si.Gender,
                                Race = si.Races,
                                HispanicLatinoEthnicity = si.HispanicLatinoEthnicity.HasValue ? si.HispanicLatinoEthnicity.Value.ToString() : null,
                                SchoolId = si.SchoolKey,
                                GradeLevel = si.GradeLevel,
                                GradeLevelSortOrder = si.SortOrder,
                                GradeLevelListDisplayText = si.GradeLevelDisplayText,
                                SchoolName = si.SchoolName,
                                SchoolCategory = si.SchoolCategory,
                                AddressLine1 = si.AddressLine1,
                                AddressLine2 = si.AddressLine2,
                                AddressLine3 = si.AddressLine3,
                                City = si.AddressCity,
                                DateOfBirth = si.BirthDate.HasValue ? si.BirthDate.Value : DateTime.MinValue
                            }).ToList();
            return students.Take(25);
        }

        public IEnumerable<StudentIndicator> GetIndicators(long studentUniqueId)
        {
            var student = _context.StudentDims.AsNoTracking()
                                            .Where(studentDim => studentDim.StudentKey.Equals(studentUniqueId.ToString())
                                                               && (studentDim.IsEnrolledToSchool.HasValue && studentDim.IsEnrolledToSchool.Value)
                                                               && studentDim.StudentSchoolAssociationOrderKey == 1)
                                            .FirstOrDefault();
            if (student == null)
                return null;

            List<StudentIndicator> studentIndicators = new List<StudentIndicator>();
            Dictionary<IndicatorSource, string[]> otherIndicators = new Dictionary<IndicatorSource, string[]>
            {
                { IndicatorSource.Indicators,       new string[]{ "At Risk"} },
                { IndicatorSource.Characteristics,  new string[]{ "Homeless" ,"Immigrant" , "Migrant" } }
            };
            foreach (var otherIndicator in otherIndicators)
            {
                var indicatorSource = otherIndicator.Key;
                foreach (var indicator in otherIndicator.Value)
                {
                    bool status = false;

                    if (indicatorSource == IndicatorSource.Indicators)
                        status = !string.IsNullOrEmpty(student.Indicators) && student.Indicators.Contains(indicator, StringComparison.OrdinalIgnoreCase);

                    if (indicatorSource == IndicatorSource.Characteristics)
                        status = !string.IsNullOrEmpty(student.Characteristics) && student.Characteristics.Contains(indicator, StringComparison.OrdinalIgnoreCase);

                    studentIndicators.Add(new StudentIndicator
                    {
                        StudentUniqueId = student.StudentKey,
                        Name = indicator,
                        Status = status,
                        Type = "Other",
                        DisplayOrder = null
                    });
                }
            }

            studentIndicators.Add(new StudentIndicator
            {
                StudentUniqueId = student.StudentKey,
                Name = "Economically Disadvantaged",
                Status = student.EconomicDisadvantaged.HasValue ? student.EconomicDisadvantaged.Value : false,
                Type = "Other",
                DisplayOrder = null
            });
            studentIndicators.Add(new StudentIndicator
            {
                StudentUniqueId = student.StudentKey,
                Name = "Limited English Proficiency",
                Status = !string.IsNullOrEmpty(student.LimitedEnglishProficiency) ? student.LimitedEnglishProficiency.Equals("Limited English Proficiency") : false,
                Type = "Other",
                DisplayOrder = null
            });
            studentIndicators.Add(new StudentIndicator
            {
                StudentUniqueId = student.StudentKey,
                Name = "Limited English Proficiency Monitored 1",
                Status = !string.IsNullOrEmpty(student.LimitedEnglishProficiency) ? student.LimitedEnglishProficiency.Equals("Limited English Proficiency Monitored 1") : false,
                Type = "Other",
                DisplayOrder = null
            });
            studentIndicators.Add(new StudentIndicator
            {
                StudentUniqueId = student.StudentKey,
                Name = "Limited English Proficiency Monitored 2",
                Status = !string.IsNullOrEmpty(student.LimitedEnglishProficiency) ? student.LimitedEnglishProficiency.Equals("Limited English Proficiency Monitored 2") : false,
                Type = "Other",
                DisplayOrder = null
            });
            studentIndicators.Add(new StudentIndicator
            {
                StudentUniqueId = student.StudentKey,
                Name = "Free or Reduced Priced Lunch Eligible",
                Status = student.HasFreeOrReducedPriceFoodServiceEligibility.HasValue ? student.HasFreeOrReducedPriceFoodServiceEligibility.Value : false,
                Type = "Other",
                DisplayOrder = null
            });

            var currentSchoolYearDim = _context.CurrentSchoolYearDims.FirstOrDefault();
            if (currentSchoolYearDim != null && student.BirthDate.HasValue)
            {
                Func<DateTime, int, string, bool> IsOverAge = (birthDate, schoolYear, gradeLevel) =>
                {
                    var yearDiff = schoolYear - birthDate.Year;
                    var age = (birthDate.Month < 9 || (birthDate.Month == 9 && birthDate.Day == 1)) ? yearDiff : yearDiff - 1;
                    if (gradeLevel.Equals("Ninth grade"))
                    {
                        return age >= 15;
                    }
                    if (gradeLevel.Equals("Tenth grade"))
                    {
                        return age >= 16;
                    }
                    if (gradeLevel.Equals("Eleventh grade"))
                    {
                        return age >= 17;
                    }
                    if (gradeLevel.Equals("Twelfth grade"))
                    {
                        return age >= 18;
                    }
                    return false;
                };
                studentIndicators.Add(new StudentIndicator
                {
                    StudentUniqueId = student.StudentKey,
                    Name = "Over Age",
                    Status = IsOverAge(student.BirthDate.Value, currentSchoolYearDim.SchoolYear.Value, student.GradeLevel),
                    Type = "Other",
                    DisplayOrder = null
                });
            }

            var schoolMinMaxDateDim = _context.SchoolMinMaxDateDims.Where(s => s.SchoolKey.Equals(student.SchoolKey)).FirstOrDefault();
            if (schoolMinMaxDateDim != null && schoolMinMaxDateDim.MaxDate.HasValue)
            {
                DateTime systemDate = schoolMinMaxDateDim.MaxDate.Value;
                var programs = _context.ProgramDims.AsNoTracking()
                                     .Where(programDim => programDim.EducationOrganizationKey == student.LocalEducationAgencyKey
                                                        && !programDim.ProgramType.Equals("Other")
                                                        && !programDim.ProgramName.StartsWith("Food Service"))
                                     .OrderBy(programDim => programDim.ProgramName)
                                     .ToList();

                var studentPrograms = _context.StudentProgramAssociationDims.AsNoTracking()
                                            .Where(studentProgramAssociationDim => studentProgramAssociationDim.StudentKey.Equals(student.StudentKey)
                                                                                && studentProgramAssociationDim.EducationOrganizationKey == student.LocalEducationAgencyKey)
                                            .ToList();
                studentPrograms = studentPrograms.Where(studentProgram => systemDate.IsInRange(studentProgram)).ToList();

                int displayOrder = 0;
                programs.ForEach(program =>
                {
                    studentIndicators.Add(new StudentIndicator
                    {
                        StudentUniqueId = student.StudentKey,
                        Name = program.ProgramName,
                        Status = studentPrograms.Any() && studentPrograms.Exists(studentProgram => studentProgram.ProgramType.Equals(program.ProgramType)),
                        Type = "Program",
                        DisplayOrder = displayOrder++
                    });
                });
            }
            return studentIndicators;
        }

        public IEnumerable<StudentSchoolInformation> GetSchools(long studentUniqueId)
        {
            List<StudentSchoolInformation> studentSchools = new List<StudentSchoolInformation>();

            var studentSchoolDims = _context.StudentSchoolDims.AsNoTracking()
                                           .Where(ssd => ssd.StudentKey.Equals(studentUniqueId.ToString()))
                                           .ToList();
            if (!studentSchoolDims.Any())
                return null;


            studentSchoolDims.ForEach(studentSchoolDim =>
            {
                DateTime _schoolYearStartDate = DateTime.MinValue;
                var gradingPeriod = _context.GradingPeriodDims.AsNoTracking()
                                    .Where(gpd => gpd.SchoolKey.Equals(studentSchoolDim.SchoolKey)
                                                && (gpd.PeriodSequence.HasValue && gpd.PeriodSequence.Value == 1))
                                    .FirstOrDefault();

                if (gradingPeriod != null)
                {
                    DateTime.TryParseExact(gradingPeriod.GradingPeriodBeginDateKey, "yyyyddMM", new CultureInfo("en-US"), DateTimeStyles.None, out _schoolYearStartDate);
                }
                studentSchools.Add(new StudentSchoolInformation
                {
                    StudentUniqueId = studentSchoolDim.StudentKey,
                    SchoolId = studentSchoolDim.SchoolKey,
                    GradeLevel = studentSchoolDim.GradeLevel,
                    DateOfEntry = studentSchoolDim.EntryDate,
                    DateOfWithdrawal = studentSchoolDim.ExitWithdrawDate,
                    LateEnrollment = _schoolYearStartDate != DateTime.MinValue && studentSchoolDim.EntryDate > _schoolYearStartDate.AddDays(/*_configurations.DaysNeededForLateEnrollment*/14) ? "Yes" : "No",
                    GraduationPlan = studentSchoolDim.GraduationPlan,
                    WithdrawalDescription = studentSchoolDim.WithdrawalDescription,
                    //IncompleteTranscript
                    //ExpectedGraduationYear
                });
            });
            return studentSchools.Any() ? studentSchools : null;
        }

        public StudentInformation GetStudentByUsi(long studentUniqueId)
        {
            var student = (from si in _context.StudentDims.AsNoTracking()
                           join ssi in _context.StudentSchoolDims.AsNoTracking()
                               on si.StudentKey equals ssi.StudentKey
                           where ssi.StudentKey.Equals(studentUniqueId.ToString())
                           select new StudentInformation
                           {
                               StudentUniqueId = Convert.ToInt32(si.StudentKey),
                               FirstName = si.FirstName,
                               MiddleName = si.MiddleName,
                               LastSurname = si.LastSurname,
                               FullName = si.FullName,
                               Gender = si.Gender,
                               Race = si.Races,
                               HispanicLatinoEthnicity = si.HispanicLatinoEthnicity.HasValue ? si.HispanicLatinoEthnicity.Value.ToString() : null,
                               SchoolId = si.SchoolKey,
                               GradeLevel = si.GradeLevel,
                               GradeLevelSortOrder = si.SortOrder,
                               GradeLevelListDisplayText = si.GradeLevelDisplayText,
                               SchoolName = si.SchoolName,
                               SchoolCategory = si.SchoolCategory,
                               AddressLine1 = si.AddressLine1,
                               AddressLine2 = si.AddressLine2,
                               AddressLine3 = si.AddressLine3,
                               City = si.AddressCity,
                               DateOfBirth = si.BirthDate.HasValue ? si.BirthDate.Value : DateTime.MinValue
                           })
                                      .FirstOrDefault();
            return student;
        }

        public IEnumerable<StudentParentInformation> GetParentInformations(long studentUniqueId)
        {
            var studentParentInformations = _context.ContactPersonDims.AsNoTracking()
                                                    .Where(student => student.StudentKey.Equals(studentUniqueId.ToString()))
                                                    .Select(spi => new StudentParentInformation
                                                    {
                                                        StudentUniqueId = spi.StudentKey,
                                                        FullName = spi.FullName,
                                                        Relation = spi.RelationshipToStudent,
                                                        HomeAddress = spi.ContactHomeAddress,
                                                        PhysicalAddress = spi.ContactPhysicalAddress,
                                                        MailingAddress = spi.ContactMailingAddress,
                                                        WorkAddress = spi.ContactWorkAddress,
                                                        TemporaryAddress = spi.ContactTemporaryAddress,
                                                        TelephoneNumber = spi.MobilePhoneNumber,
                                                        WorkTelephoneNumber = spi.WorkPhoneNumber,
                                                        EmailAddress = spi.PersonalEmailAddress,
                                                        IsPrimaryContact = spi.IsPrimaryContact.HasValue ? spi.IsPrimaryContact.Value : false,
                                                    })
                                                    .ToList();
            return studentParentInformations;
        }
    }
}