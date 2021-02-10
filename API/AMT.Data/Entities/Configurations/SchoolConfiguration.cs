using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AMT.Data.Entities.Configurations
{
    public class SchoolMinMaxDateConfig : IEntityTypeConfiguration<SchoolMinMaxDateDim>
    {
        public void Configure(EntityTypeBuilder<SchoolMinMaxDateDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.SchoolMinMaxDateDim");
        }
    }
    public class SchoolCalendarDimConfig : IEntityTypeConfiguration<SchoolCalendarDim>
    {
        public void Configure(EntityTypeBuilder<SchoolCalendarDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.SchoolCalendarDim");
        }
    }
    public class CurrentSchoolYearDimConfig : IEntityTypeConfiguration<CurrentSchoolYearDim>
    {
        public void Configure(EntityTypeBuilder<CurrentSchoolYearDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.CurrentSchoolYearDim");
        }
    }
    public class StudentSchoolAttendanceEventFactConfig : IEntityTypeConfiguration<StudentSchoolAttendanceEventFact>
    {
        public void Configure(EntityTypeBuilder<StudentSchoolAttendanceEventFact> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentSchoolAttendanceEventFact");
        }
    }
    public class GradingPeriodDimConfig : IEntityTypeConfiguration<GradingPeriodDim>
    {
        public void Configure(EntityTypeBuilder<GradingPeriodDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.GradingPeriodDim");
        }
    }
    public class StudentSchoolDimConfig : IEntityTypeConfiguration<StudentSchoolDim>
    {
        public void Configure(EntityTypeBuilder<StudentSchoolDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentSchoolDim");
        }
    }
}
