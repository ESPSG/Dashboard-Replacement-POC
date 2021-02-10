using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMT.Data.Entities.Configurations
{
    public class StudentGradeDimConfig : IEntityTypeConfiguration<StudentGradeDim>
    {
        public void Configure(EntityTypeBuilder<StudentGradeDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentGradeDim");
        }
    }
    public class GradingScaleDimConfig : IEntityTypeConfiguration<GradingScaleDim>
    {
        public void Configure(EntityTypeBuilder<GradingScaleDim> builder)
        {
            builder.HasKey(e => e.GradingScaleId);

            builder.ToTable("GradingScale", "analytics_config");

            builder.HasIndex(e => new { e.LocalEducationAgencyId, e.GradingScaleName }, "AK_GradingScaleName")
                .IsUnique();

            builder.Property(e => e.GradingScaleName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
    public class GradingScaleGradeDimConfig : IEntityTypeConfiguration<GradingScaleGradeDim>
    {
        public void Configure(EntityTypeBuilder<GradingScaleGradeDim> builder)
        {
            builder.HasKey(e => e.GradingScaleGradeId);

            builder.ToTable("GradingScaleGrade", "analytics_config");

            builder.HasIndex(e => new { e.GradingScaleId, e.Rank }, "UX_GradingScaleGrade_Rank")
                .IsUnique();

            builder.Property(e => e.LetterGrade).HasMaxLength(20);

            builder.Property(e => e.UpperNumericGrade).HasColumnType("decimal(6, 2)");
        }
    }
    public class GradingScaleGradeLevelDimConfig : IEntityTypeConfiguration<GradingScaleGradeLevelDim>
    {
        public void Configure(EntityTypeBuilder<GradingScaleGradeLevelDim> builder)
        {
            builder.HasKey(e => e.GradingScaleGradeLevelId);

            builder.ToTable("GradingScaleGradeLevel", "analytics_config");
        }
    }
    public class GradingScaleMetricThresholdDimConfig : IEntityTypeConfiguration<GradingScaleMetricThresholdDim>
    {
        public void Configure(EntityTypeBuilder<GradingScaleMetricThresholdDim> builder)
        {
            builder.HasKey(e => e.GradingScaleMetricThresholdId);

            builder.ToTable("GradingScaleMetricThreshold", "analytics_config");
        }
    }
    public class GradeLevelTypeDimConfig : IEntityTypeConfiguration<GradeLevelTypeDim>
    {
        public void Configure(EntityTypeBuilder<GradeLevelTypeDim> builder)
        {
            builder.HasKey(e => e.GradeLevelTypeId);

            builder.ToTable("GradeLevelType", "analytics_config");
        }
    }
}
