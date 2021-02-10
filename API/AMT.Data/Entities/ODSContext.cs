using Microsoft.EntityFrameworkCore;

namespace AMT.Data.Entities
{
    public partial class ODSContext : DbContext
    {
        public ODSContext()
        {
        }

        public ODSContext(DbContextOptions<ODSContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(300);
        }
        public virtual DbSet<SchoolMinMaxDateDim> SchoolMinMaxDateDims { get; set; }
        public virtual DbSet<CurrentSchoolYearDim> CurrentSchoolYearDims { get; set; }
        public virtual DbSet<SchoolCalendarDim> SchoolCalendarDims { get; set; }
        public virtual DbSet<StudentSectionAttendanceEventFact> StudentSectionAttendanceEventFacts { get; set; }
        public virtual DbSet<StudentSchoolAttendanceEventFact> StudentSchoolAttendanceEventFacts { get; set; }
        public virtual DbSet<GradingPeriodDim> GradingPeriodDims { get; set; }
        public virtual DbSet<StudentSectionDim> StudentSectionDims { get; set; }
        public virtual DbSet<StudentSchoolDim> StudentSchoolDims { get; set; }
        public virtual DbSet<StudentDim> StudentDims { get; set; }
        public virtual DbSet<StudentLanguageDim> StudentLanguageDims { get; set; }
        public virtual DbSet<WarehouseStudentUSIMappingDim> WarehouseStudentUSIMappingDims { get; set; }
        public virtual DbSet<ProgramDim> ProgramDims { get; set; }
        public virtual DbSet<StudentProgramAssociationDim> StudentProgramAssociationDims { get; set; }
        public virtual DbSet<ContactPersonDim> ContactPersonDims { get; set; }
        public virtual DbSet<UserDim> UserDims { get; set; }
        public virtual DbSet<UserAuthorizationDim> UserAuthorizationDims { get; set; }
        public virtual DbSet<UserStudentDataAuthorizationDim> UserStudentDataAuthorizationDims { get; set; }
        public virtual DbSet<StudentDataAuthorizationDim> StudentDataAuthorizationDims { get; set; }
        public virtual DbSet<StudentDisciplineDim> StudentDisciplineDims { get; set; }
        public virtual DbSet<StudentAssessmentDim> StudentAssessmentDims { get; set; }
        public virtual DbSet<StudentAssessmentScoreResultDim> StudentAssessmentScoreResultDims { get; set; }
        public virtual DbSet<StudentGradeDim> StudentGradeDims { get; set; }
        public virtual DbSet<GradingScaleDim> GradingScaleDims { get; set; }
        public virtual DbSet<GradingScaleGradeDim> GradingScaleGradeDims { get; set; }
        public virtual DbSet<GradingScaleGradeLevelDim> GradingScaleGradeLevelDims { get; set; }
        public virtual DbSet<GradingScaleMetricThresholdDim> GradingScaleMetricThresholdDims { get; set; }
        public virtual DbSet<GradeLevelTypeDim> GradeLevelTypeDims { get; set; }
        public virtual DbSet<StudentAssessmentItemDim> StudentAssessmentItemDims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

