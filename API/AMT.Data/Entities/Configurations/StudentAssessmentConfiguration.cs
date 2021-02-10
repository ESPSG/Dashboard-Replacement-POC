using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMT.Data.Entities.Configurations
{
    public class StudentAssessmentDimConfig : IEntityTypeConfiguration<StudentAssessmentDim>
    {
        public void Configure(EntityTypeBuilder<StudentAssessmentDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentAssessmentDim");
        }
    }
    public class StudentAssessmentScoreResultDimConfig : IEntityTypeConfiguration<StudentAssessmentScoreResultDim>
    {
        public void Configure(EntityTypeBuilder<StudentAssessmentScoreResultDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentAssessmentScoreResultDim");
        }
    }
    public class StudentAssessmentItemDimConfig : IEntityTypeConfiguration<StudentAssessmentItemDim>
    {
        public void Configure(EntityTypeBuilder<StudentAssessmentItemDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentAssessmentItemDim");
        }
    }
}
