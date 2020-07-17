using Data.Models;
using GraphQLApi.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLApi.Repository
{
    public class StudentInformationRepository : IStudentInformationRepository
    {
        private readonly DDSContext _context;

        public StudentInformationRepository(DDSContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentInformation> GetStudentsBySchool(int schoolId, int? offset, int? limit)
        {
            var students = (from si in _context.StudentInformation.AsNoTracking()
                            join ssi in _context.StudentSchoolInformation.AsNoTracking() on si.StudentUsi equals ssi.StudentUsi
                            join sci in _context.SchoolInformation.AsNoTracking() on ssi.SchoolId equals sci.SchoolId
                            join leagli in _context.LocalEducationAgencyGradeLevelInformation.AsNoTracking()
                            on new { ssi.GradeLevel, sci.LocalEducationAgencyId } equals new { leagli.GradeLevel, leagli.LocalEducationAgencyId }
                            where sci.SchoolId == schoolId
                            orderby leagli.SortOrder ascending
                            select new StudentInformation
                            {
                                StudentUsi = si.StudentUsi,
                                LastSurname = si.LastSurname,
                                FirstName = si.FirstName,
                                MiddleName = si.MiddleName,
                                FullName = si.FullName,
                                Gender = si.Gender,
                                Race = si.Race,
                                HispanicLatinoEthnicity = si.HispanicLatinoEthnicity,
                                SchoolId = ssi.SchoolId,
                                GradeLevel = ssi.GradeLevel,
                                GradeLevelSortOrder = leagli.SortOrder,
                                GradeLevelListDisplayText = leagli.ListDisplayText,
                                LateEnrollment = ssi.LateEnrollment,
                                SchoolName = sci.Name,
                                SchoolCategory = sci.SchoolCategory,
                                AddressLine1 = si.AddressLine1,
                                AddressLine2 = si.AddressLine2,
                                AddressLine3 = si.AddressLine3,
                                City = si.City,
                                CurrentAge = si.CurrentAge,
                                HomeLanguage = si.HomeLanguage


                            }).ToList();

            if (offset.HasValue)
            {
                students = students.Skip(offset.Value).ToList();
            }

            int takeLimit = limit.HasValue ? limit.Value : 25;
            if (limit.HasValue)
            {
                students = students.Take(limit.Value).ToList();
            }
            return students;
        }

        public IEnumerable<StudentInformation> GetDefaultStudents()
        {
            var students = (from si in _context.StudentInformation.AsNoTracking()
                            join ssi in _context.StudentSchoolInformation.AsNoTracking() on si.StudentUsi equals ssi.StudentUsi
                            join sci in _context.SchoolInformation.AsNoTracking() on ssi.SchoolId equals sci.SchoolId
                            join leagli in _context.LocalEducationAgencyGradeLevelInformation.AsNoTracking()
                            on new { ssi.GradeLevel, sci.LocalEducationAgencyId } equals new { leagli.GradeLevel, leagli.LocalEducationAgencyId }
                            orderby leagli.SortOrder ascending
                            select new StudentInformation
                            {
                                StudentUsi = si.StudentUsi,
                                LastSurname = si.LastSurname,
                                FirstName = si.FirstName,
                                MiddleName = si.MiddleName,
                                FullName = si.FullName,
                                Gender = si.Gender,
                                Race = si.Race,
                                HispanicLatinoEthnicity = si.HispanicLatinoEthnicity,
                                SchoolId = ssi.SchoolId,
                                GradeLevel = ssi.GradeLevel,
                                GradeLevelSortOrder = leagli.SortOrder,
                                GradeLevelListDisplayText = leagli.ListDisplayText,
                                LateEnrollment = ssi.LateEnrollment,
                                SchoolName = sci.Name,
                                SchoolCategory = sci.SchoolCategory
                            }).ToList();
            return students.Take(25);
        }

        public IEnumerable<StudentIndicator> GetIndicators(int studentUsi)
        {
            var studentIndicators = _context.StudentIndicator.AsNoTracking()
                                            .Where(student => student.StudentUsi == studentUsi)
                                            .ToList();
            return studentIndicators;
        }

        public IEnumerable<StudentSchoolInformation> GetSchools(int studentUsi)
        {
            var studentSchools = _context.StudentSchoolInformation.AsNoTracking()
                                         .Where(student => student.StudentUsi == studentUsi)
                                         .ToList();
            return studentSchools;
        }

        public StudentInformation GetStudentByUsi(int studentUsi)
        {
            var studentInformation = _context.StudentInformation.AsNoTracking()
                                             .FirstOrDefault(student => student.StudentUsi == studentUsi);
            return studentInformation;
        }

        public IEnumerable<StudentParentInformation> GetParentInformations(int studentUsi)
        {
            var studentParentInformations = _context.StudentParentInformation.AsNoTracking()
                                                    .Where(student => student.StudentUsi == studentUsi)
                                                    .ToList();
            return studentParentInformations;
        }
    }
}