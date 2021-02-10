using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMT.Data.Entities.Configurations
{
    public class RlsUserDim : IEntityTypeConfiguration<UserDim>
    {
        public void Configure(EntityTypeBuilder<UserDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.rls_UserDim");
        }
    }
    public class RlsUserAuthorizationDim : IEntityTypeConfiguration<UserAuthorizationDim>
    {
        public void Configure(EntityTypeBuilder<UserAuthorizationDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.rls_UserAuthorization");
        }
    }
    public class RlsUserStudentDataAuthorizationDim : IEntityTypeConfiguration<UserStudentDataAuthorizationDim>
    {
        public void Configure(EntityTypeBuilder<UserStudentDataAuthorizationDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.rls_UserStudentDataAuthorization");
        }
    }
    public class RlsStudentDataAuthorizationDim : IEntityTypeConfiguration<StudentDataAuthorizationDim>
    {
        public void Configure(EntityTypeBuilder<StudentDataAuthorizationDim> builder)
        {
            builder.HasNoKey().ToSqlQuery("SELECT * FROM analytics.rls_StudentDataAuthorization");
        }
    }
}
