using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMT.Data.Entities.Configurations
{
    public class StudentDimConfig : IEntityTypeConfiguration<StudentDim>
    {
        public void Configure(EntityTypeBuilder<StudentDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentDim");
        }
    }
    public class StudentSectionAttendanceEventFactConfig : IEntityTypeConfiguration<StudentSectionAttendanceEventFact>
    {
        public void Configure(EntityTypeBuilder<StudentSectionAttendanceEventFact> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentSectionAttendanceEventFact");
        }
    }
    public class StudentSectionDimConfig : IEntityTypeConfiguration<StudentSectionDim>
    {
        public void Configure(EntityTypeBuilder<StudentSectionDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentSectionDim");
        }
    }
    public class StudentLanguageDimConfig : IEntityTypeConfiguration<StudentLanguageDim>
    {
        public void Configure(EntityTypeBuilder<StudentLanguageDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentLanguageDim");
        }
    }
    public class StudentDisciplineDimConfig : IEntityTypeConfiguration<StudentDisciplineDim>
    {
        public void Configure(EntityTypeBuilder<StudentDisciplineDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentDisciplineDim");
        }
    }
}
