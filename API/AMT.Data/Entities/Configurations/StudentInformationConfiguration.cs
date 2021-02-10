using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMT.Data.Entities.Configurations
{
    public class WarehouseStudentUSIMappingDimConfig : IEntityTypeConfiguration<WarehouseStudentUSIMappingDim>
    {
        public void Configure(EntityTypeBuilder<WarehouseStudentUSIMappingDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.WarehouseStudentUSIMappingDim");
        }
    }
    public class ProgramDimConfig : IEntityTypeConfiguration<ProgramDim>
    {
        public void Configure(EntityTypeBuilder<ProgramDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.ProgramDim");
        }
    }
    public class StudentProgramAssociationDimConfig : IEntityTypeConfiguration<StudentProgramAssociationDim>
    {
        public void Configure(EntityTypeBuilder<StudentProgramAssociationDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.StudentProgramAssociationDim");
        }
    }
    public class ContactPersonDimConfig : IEntityTypeConfiguration<ContactPersonDim>
    {
        public void Configure(EntityTypeBuilder<ContactPersonDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.ContactPersonDim");
        }
    }
}
