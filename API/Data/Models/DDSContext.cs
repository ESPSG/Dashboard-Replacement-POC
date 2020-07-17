using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public partial class DDSContext : DbContext
    {
        public DDSContext()
        {
        }

        public DDSContext(DbContextOptions<DDSContext> options)
            : base(options)
        {
        }
        public virtual DbSet<DomainEntityType> DomainEntityType { get; set; }
        public virtual DbSet<Metric> Metric { get; set; }
        public virtual DbSet<MetricInstance> MetricInstance { get; set; }
        public virtual DbSet<MetricNode> MetricNode { get; set; }
        public virtual DbSet<MetricType> MetricType { get; set; }
        public virtual DbSet<MetricVariant> MetricVariant { get; set; }
        public virtual DbSet<MetricVariantType> MetricVariantType { get; set; }
        public virtual DbSet<StudentSchoolMetricInstanceSet> StudentSchoolMetricInstanceSet { get; set; }
        public virtual DbSet<StudentIndicator> StudentIndicator { get; set; }
        public virtual DbSet<StudentInformation> StudentInformation { get; set; }
        public virtual DbSet<StudentSchoolInformation> StudentSchoolInformation { get; set; }
        public virtual DbSet<StudentParentInformation> StudentParentInformation { get; set; }
        public virtual DbSet<LocalEducationAgencyGradeLevelInformation> LocalEducationAgencyGradeLevelInformation { get; set; }
        public virtual DbSet<SchoolInformation> SchoolInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalEducationAgencyGradeLevelInformation>(entity =>
            {
                entity.HasKey(e => new { e.LocalEducationAgencyId, e.GradeLevelTypeId });

                entity.ToTable("LocalEducationAgencyGradeLevelInformation", "domain");

                entity.Property(e => e.GradeLevel)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ListDisplayText)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SchoolInformation>(entity =>
            {
                entity.HasKey(e => e.SchoolId);

                entity.ToTable("SchoolInformation", "domain");

                entity.HasComment("Stores basic information about schools within a given LEA including School Name, School Category (Ungraded, High School, Middle School, Elementary School, etc.), Mailing Address, Telephone & Fax Number, and Website.");

                entity.Property(e => e.SchoolId)
                    .HasComment("Identifier for the school")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(80)
                    .HasComment("Street name and number components of the school's address");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(20)
                    .HasComment("Possible building number or other second address component");

                entity.Property(e => e.AddressLine3)
                    .HasMaxLength(20)
                    .HasComment("Possible room number or other third address component");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .HasComment("City component of the school's address");

                entity.Property(e => e.FaxNumber)
                    .HasMaxLength(25)
                    .HasComment("School's fax number");

                entity.Property(e => e.LocalEducationAgencyId).HasComment("Identifier of the school's LEA");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Name of the school");

                entity.Property(e => e.ProfileThumbnail)
                    .HasMaxLength(150)
                    .HasComment("Image [path/file?] of the school");

                entity.Property(e => e.SchoolCategory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Type of school: \"Elementary School\" \"Middle School\" \"High School\" or \"Ungraded\"");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .HasComment("State component of the school's address");

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(25)
                    .HasComment("School's telephone number");

                entity.Property(e => e.WebSite)
                    .HasMaxLength(150)
                    .HasComment("School's website");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(17)
                    .HasComment("Zip Code component of the school's address");
            });

            modelBuilder.Entity<DomainEntityType>(entity =>
            {
                entity.ToTable("DomainEntityType", "metric");

                entity.HasComment("List of domain entity types tracked in the dashboard implementation.  Currently, this includes: Student-School, Staff, School, and Local Education Agency (LEA).");

                entity.Property(e => e.DomainEntityTypeId)
                    .HasComment("Identifier for the corresponding Domain Entity Type")
                    .ValueGeneratedNever();

                entity.Property(e => e.DomainEntityTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of the Domain Entity Type: \"StudentSchool, \"Staff\", \"School\", \"Local Education Agency\", \"Educational Service Center\", and \"State\"");
            });

            modelBuilder.Entity<Metric>(entity =>
            {
                entity.ToTable("Metric", "metric");

                entity.HasComment("Central metric table that defines the individual metrics and metric containers that can be visualized in the dashboard.  Includes metric name, domain entity type (School, LEA, Student-School, Staff Member, etc.), metric type (GranularMetric, ContainerMetric, etc.), name, trend interpretation (i.e. is an increase in this metric good or bad), enabled flag, and child roll-up metric identifier.");

                entity.Property(e => e.MetricId)
                    .HasComment("Identifier for this metric")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildDomainEntityMetricId).HasComment("Identifier for child metric");

                entity.Property(e => e.DomainEntityTypeId).HasComment("Identifier for the domain entity type this metric applies to");

                entity.Property(e => e.Enabled).HasComment("Flag for if this metric is enabled for use");

                entity.Property(e => e.MetricName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Readable name of this metric");

                entity.Property(e => e.MetricTypeId).HasComment("Identifier for this metric's MetricType");

                entity.Property(e => e.TrendInterpretation).HasComment("Upwards/Downwards trending value: 1, -1, or NULL");

                entity.HasOne(d => d.DomainEntityType)
                    .WithMany(p => p.Metric)
                    .HasForeignKey(d => d.DomainEntityTypeId)
                    .HasConstraintName("FK_Metric_DomainEntityType");

                entity.HasOne(d => d.MetricType)
                    .WithMany(p => p.Metric)
                    .HasForeignKey(d => d.MetricTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Metric_MetricType");
            });

            modelBuilder.Entity<MetricInstance>(entity =>
            {
                entity.HasKey(e => new { e.MetricInstanceSetKey, e.MetricId });

                entity.ToTable("MetricInstance", "metric");

                entity.HasComment("Includes actual metric values for LEA's, schools, student-schools, staff members, etc.  Also includes supporting information such as metric state, context, and trend direction.");

                entity.HasIndex(e => new { e.MetricInstanceSetKey, e.MetricStateTypeId, e.Value, e.ValueTypeName, e.TrendDirection, e.MetricId })
                    .HasName("IX_MetricInstance_MetricId");

                entity.Property(e => e.MetricInstanceSetKey).HasComment("Identifier for this metric instance");

                entity.Property(e => e.MetricId).HasComment("Identifier for the corresponding metric");

                entity.Property(e => e.Context)
                    .HasMaxLength(100)
                    .HasComment("Readable context detailing this metric instance");

                entity.Property(e => e.Flag).HasComment("Flag for this metric instance");

                entity.Property(e => e.MetricStateTypeId).HasComment("Identifier for the metric state type");

                entity.Property(e => e.TrendDirection).HasComment("Upwards/Downwards trending value: 1, -1, or NULL");

                entity.Property(e => e.Value)
                    .HasMaxLength(20)
                    .HasComment("Value for this metric instance");

                entity.Property(e => e.ValueTypeName)
                    .HasMaxLength(50)
                    .HasComment("Data type of the associated Metric's value (for implementation)");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.MetricInstance)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MetricInstance_Metric");
            });

            modelBuilder.Entity<MetricNode>(entity =>
            {
                entity.ToTable("MetricNode", "metric");

                entity.HasComment("Organizes metrics into hierarchical tree structures that support the layout of the tab and section-based user interface");

                entity.Property(e => e.MetricNodeId)
                    .HasComment("Identifier for this Node")
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(150)
                    .HasComment("Display name for this Node");

                entity.Property(e => e.DisplayOrder).HasComment("Display order for this Node within the tree");

                entity.Property(e => e.MetricId).HasComment("Identifier for the corresponding metric");

                entity.Property(e => e.MetricVariantId).HasComment("Identifier for the corresponding metric variant");

                entity.Property(e => e.ParentNodeId).HasComment("Identifer for the parent of this node");

                entity.Property(e => e.RootNodeId).HasComment("Identifer for the root node of this tree");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.MetricNode)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MetricNode_Metric");

                entity.HasOne(d => d.MetricVariant)
                    .WithMany(p => p.MetricNode)
                    .HasForeignKey(d => d.MetricVariantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MetricNode_MetricVariant");

                entity.HasOne(d => d.ParentNode)
                    .WithMany(p => p.InverseParentNode)
                    .HasForeignKey(d => d.ParentNodeId)
                    .HasConstraintName("FK_MetricNode_MetricNode");
            });

            modelBuilder.Entity<MetricType>(entity =>
            {
                entity.ToTable("MetricType", "metric");

                entity.HasComment("Lists all of the different types of metrics displayed in the dashboards including: ContainerMetric, AggregateMetric, GranularMetric, CampusAggregateMetric, CampusGranularMetric.  A ContainerMetric is one that contains other metrics or other container metrics.  An AggregateMetric is a special type of container metric that renders as a gray table header containing other container and granular metrics.  A GranularMetric is a metric at the student level that does not contain other metrics but instead has a specific value, trend direction, etc.  A CampusAggregateMetric is rendered at the page header level and contains other aggregate metrics.  A CampusGranularMetric is a campus metric that does not contain other sub-metrics but that is a roll-up of GranularMetrics which are at the student level.");

                entity.Property(e => e.MetricTypeId)
                    .HasComment("Identifier for this type of Metric")
                    .ValueGeneratedNever();

                entity.Property(e => e.MetricTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of the type: \"ContainerMetric\", \"AggregateMetric\", \"GranularMetric\", \"CampusAggregateMetric\", \"CampusGranularMetric\"");
            });

            modelBuilder.Entity<MetricVariant>(entity =>
            {
                entity.ToTable("MetricVariant", "metric");

                entity.HasComment("Used primarily to display prior year metrics alongside their closely related current-year metric counterparts.  For both Current Year and Prior Year metrics, the table stores some extended information used in the display of metric values in the user interface.");

                entity.Property(e => e.MetricVariantId)
                    .HasComment("Identifier for this Metric Variant")
                    .ValueGeneratedNever();

                entity.Property(e => e.Enabled).HasComment("Flag for if this metric is enabled for use");

                entity.Property(e => e.Format)
                    .HasMaxLength(50)
                    .HasComment("Display formatting for this  variant's info");

                entity.Property(e => e.ListDataLabel)
                    .HasMaxLength(150)
                    .HasComment("Display label for this variant");

                entity.Property(e => e.ListFormat)
                    .HasMaxLength(50)
                    .HasComment("Display formatting for this  variant's List Data Label");

                entity.Property(e => e.MetricDescription)
                    .HasMaxLength(250)
                    .HasComment("Description of what this metric represents");

                entity.Property(e => e.MetricId).HasComment("Identifer for the corresponding Metric");

                entity.Property(e => e.MetricName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Name of this Variant");

                entity.Property(e => e.MetricShortName)
                    .HasMaxLength(10)
                    .HasComment("Shortened name for this metric/variant, if needed");

                entity.Property(e => e.MetricTooltip)
                    .HasMaxLength(250)
                    .HasComment("Display tooltip for this variant");

                entity.Property(e => e.MetricUrl)
                    .HasMaxLength(250)
                    .HasComment("URL this variant is related to in the implementation / on the site");

                entity.Property(e => e.MetricVariantTypeId).HasComment("Identifier for this variant's type (current or prior year)");

                entity.Property(e => e.NumeratorDenominatorFormat)
                    .HasMaxLength(50)
                    .HasComment("Display formatting for this variant's metric values");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.MetricVariant)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MetricVariant_Metric");

                entity.HasOne(d => d.MetricVariantType)
                    .WithMany(p => p.MetricVariant)
                    .HasForeignKey(d => d.MetricVariantTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MetricVariant_MetricVariantType");
            });

            modelBuilder.Entity<MetricVariantType>(entity =>
            {
                entity.ToTable("MetricVariantType", "metric");

                entity.HasComment("Stores a definition of the two types of metric variants: Current Year and Prior Year.");

                entity.Property(e => e.MetricVariantTypeId)
                    .HasComment("Identifier for this variant type")
                    .ValueGeneratedNever();

                entity.Property(e => e.MetricVariantTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of this variant type: \"Current Year\" or \"Prior Year\"");
            });

            modelBuilder.Entity<StudentSchoolMetricInstanceSet>(entity =>
            {
                entity.HasKey(e => e.MetricInstanceSetKey);

                entity.ToTable("StudentSchoolMetricInstanceSet", "domain");

                entity.HasComment("Specifies the link between a student-school pairing and the metric instance set ID by which all of the standard metric instance data for that student is stored.");

                entity.HasIndex(e => new { e.StudentUsi, e.SchoolId })
                    .HasName("IX_StudentSchoolMetricInstanceSet")
                    .IsUnique();

                entity.Property(e => e.MetricInstanceSetKey)
                    .HasComment("Identifier for  this MetricInstanceSet")
                    .ValueGeneratedNever();

                entity.Property(e => e.SchoolId).HasComment("Identifier for the school this metric is realted to");

                entity.Property(e => e.StudentUsi)
                    .HasColumnName("StudentUSI")
                    .HasComment("Identifier fro the student this  metric is related to");
            });

            modelBuilder.Entity<StudentParentInformation>(entity =>
            {
                entity.HasKey(e => new { e.ParentUsi, e.StudentUsi });

                entity.ToTable("StudentParentInformation", "domain");

                entity.HasComment("Stores parent/guardian data for students.  The data tracked for parents/guardians includes Parent ID, Related Student ID, Name, Relationship (i.e. Father, Mother, Grandparent), Mailing Address, Home & Work Phone, E-mail Address, Primary Contact Flag, and Lives With Flag.");

                entity.Property(e => e.ParentUsi)
                    .HasColumnName("ParentUSI")
                    .HasComment("Identifier for this student/parent pair");

                entity.Property(e => e.StudentUsi)
                    .HasColumnName("StudentUSI")
                    .HasComment("Identifier for student  for this parent");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(80)
                    .HasComment("Number / Street  component of the parent's address");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(20)
                    .HasComment("Possible building number or other second address component");

                entity.Property(e => e.AddressLine3)
                    .HasMaxLength(20)
                    .HasComment("Possible room number or other third address component");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .HasComment("City component of the parent's address");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .HasComment("Email address of this parent");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Combination of the first and last names of the parent");

                entity.Property(e => e.LivesWith).HasComment("Whether or not the student lives with this parent");

                entity.Property(e => e.PrimaryContact).HasComment("Whether or not this is the primary contact for the parents");

                entity.Property(e => e.Relation)
                    .HasMaxLength(50)
                    .HasComment("Relation to the Student - in addtition to Mother/Father/Gaurdians, includes grandparents, case workers, doctors, and \"Other\"");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .HasComment("State component of the parent's address");

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(20)
                    .HasComment("Home telephone number of this parent");

                entity.Property(e => e.WorkTelephoneNumber)
                    .HasMaxLength(20)
                    .HasComment("Work telephone number of this parent");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .HasComment("Zip code component of the parent's address");

                entity.HasOne(d => d.StudentUsiNavigation)
                    .WithMany(p => p.StudentParentInformation)
                    .HasForeignKey(d => d.StudentUsi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentParentInformation_StudentInformation");
            });

            modelBuilder.Entity<StudentIndicator>(entity =>
            {
                entity.HasKey(e => new { e.StudentUsi, e.EducationOrganizationId, e.Type, e.Name });

                entity.ToTable("StudentIndicator", "domain");

                entity.HasComment("Includes a listing of indicators by student and educational organization.  For example, one row of the table might indicate that Tommy Watson at Johnson Middle School was Not Homeless.");

                entity.HasIndex(e => new { e.StudentUsi, e.EducationOrganizationId, e.Name })
                    .HasName("IX_StudentIndicator");

                entity.HasIndex(e => new { e.StudentUsi, e.Name, e.Status })
                    .HasName("IX_StudentIndicator_NameAndStatus");

                entity.HasIndex(e => new { e.StudentUsi, e.EducationOrganizationId, e.Status, e.Name })
                    .HasName("IX_StudentIndicator_EdOrgId_Status_Name");

                entity.Property(e => e.StudentUsi)
                    .HasColumnName("StudentUSI")
                    .HasComment("Identifier for the corresponding student");

                entity.Property(e => e.EducationOrganizationId).HasComment("Identifier of the LEA or school this student is associated with");

                entity.Property(e => e.Type)
                    .HasMaxLength(15)
                    .HasComment("Type of this indicator: \"Program\" (Special Education, Gifted, Bilingual, Career Education, Title I, English as a second language), \"Special\" (Special Education Services, Other Services, Primary Instructional Setting), or \"Other\" (At Risk, Economically Disadvantaged, Homeless, Immigrant, Limited English Proficiency, Migrant, Over Age, Repeater)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Name of the corresponding Indicator (includes indicators and programs): \"At Risk\",\"Bilingual Program\",\"Career and Technical Education\",\"Economically Disadvantaged\",\"English as Second Language\",\"Gifted/Talented\",\"Homeless\",\"Immigrant\",\"Limited English Proficiency\",\"Migrant\",\",\"Other Services\",\"Over Age\",\"Primary Instructional Setting\",\"Repeater\",\"Special Education\",\"Special Education Services\",\"Title I Participation\"");

                entity.Property(e => e.DisplayOrder).HasComment("No data found");

                entity.Property(e => e.ParentName)
                    .HasMaxLength(50)
                    .HasComment("Parent indicator - no data found");

                entity.Property(e => e.Status).HasComment("If this student is associated with the indicator");

                entity.HasOne(d => d.StudentUsiNavigation)
                    .WithMany(p => p.StudentIndicator)
                    .HasForeignKey(d => d.StudentUsi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentIndicator_StudentInformation");
            });

            modelBuilder.Entity<StudentInformation>(entity =>
            {
                entity.HasKey(e => e.StudentUsi);

                entity.ToTable("StudentInformation", "domain");

                entity.HasComment("Contains general data about a student independent of the school he or she is attending such as Name, Home Address, Telephone Number, E-mail Address, Birth Date and Location, Age, Cohort Year, Gender, Race, Ethnicity, Home Language, Parent in Military, Single Parent/Pregnant Teen, etc.");

                entity.HasIndex(e => new { e.StudentUsi, e.Gender })
                    .HasName("IX_StudentInformation_Gender");

                entity.HasIndex(e => new { e.StudentUsi, e.HispanicLatinoEthnicity, e.Race })
                    .HasName("IX_StudentInformation_HispanicLatinoEthnicityAndRace");

                entity.HasIndex(e => new { e.StudentUsi, e.FirstName, e.MiddleName, e.LastSurname, e.Gender })
                    .HasName("IX_StudentInformation_Gender_INC_StudentUSI_First_Middle_Last");

                entity.Property(e => e.StudentUsi)
                    .HasColumnName("StudentUSI")
                    .HasComment("Identifier for the student")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(80)
                    .HasComment("Street name and number component of the student's address");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(20)
                    .HasComment("Student address - no data found");

                entity.Property(e => e.AddressLine3)
                    .HasMaxLength(20)
                    .HasComment("Student address - no data found");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .HasComment("City component of the student's address");

                entity.Property(e => e.CohortYear).HasComment("Year this student was placed in a cohort");

                entity.Property(e => e.CurrentAge).HasComment("Student's age");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasComment("Student's date of birth");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .HasComment("Student e-mail address - no data found");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Student's first name");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Student's first and last name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasComment("Student's gender (\"Male\" or \"Female\")");

                entity.Property(e => e.HispanicLatinoEthnicity)
                    .HasMaxLength(5)
                    .HasComment("If the student's ethnicity is Hispanic/Latino (\"Yes\" or \"No\")");

                entity.Property(e => e.HomeLanguage)
                    .HasMaxLength(150)
                    .HasComment("Student's natural language");

                entity.Property(e => e.Language)
                    .HasMaxLength(150)
                    .HasComment("Student's primary language");

                entity.Property(e => e.LastSurname)
                    .IsRequired()
                    .HasMaxLength(35)
                    .HasComment("Student's last name");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(20)
                    .HasComment("Student's middle initial or NULL");

                entity.Property(e => e.OldEthnicity)
                    .HasMaxLength(35)
                    .HasComment("Student's ethnicity using old nomenclature: \"White, Not Of Hispanic Origin\", \"Hispanic\", \"Black, Not Of Hispanic Origin\", \"Asian Or Pacific Islander\", \"American Indian Or Alaskan Native\", or NULL");

                entity.Property(e => e.ParentMilitary)
                    .HasMaxLength(5)
                    .HasComment("If at least one of the student's parents is in the military (\"Yes\" or \"No\")");

                entity.Property(e => e.PlaceOfBirth)
                    .HasMaxLength(56)
                    .HasComment("Student's place of birth as \"[City], [State], [Nation]\"");

                entity.Property(e => e.ProfileThumbnail)
                    .HasMaxLength(150)
                    .HasComment("Image [path/file]? of the student");

                entity.Property(e => e.Race)
                    .HasMaxLength(175)
                    .HasComment("Any one or any comma-separated combination of the following, or NULL:  \"White\",  \"Black - African American\", \"Asian\", \"Native Hawaiian - Pacific Islander\", \"American Indian - Alaskan Native\"");

                entity.Property(e => e.SingleParentPregnantTeen)
                    .HasMaxLength(5)
                    .HasComment("If the student is pregnant or a single parent teen (\"Yes\" or \"No\")");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .HasComment("State component of the student's address");

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(20)
                    .HasComment("Student telephone number - no data found");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .HasComment("Zip code component of the student's address");
            });

            modelBuilder.Entity<StudentSchoolInformation>(entity =>
            {
                entity.HasKey(e => new { e.StudentUsi, e.SchoolId });

                entity.ToTable("StudentSchoolInformation", "domain");

                entity.HasComment("Includes details about a student's enrollment at a particular school.  The details tracked include student, school, grade level, homeroom, late enrollment flag, incomplete transcript flag, date of entry into school, date of withdrawal from school, withdrawal details, graduation plan type (i.e. Recommended, Minimum, etc.), expected graduation year, and feeder schools (although this value is never specified in the Lubbock ISD LPR data).");

                entity.HasIndex(e => new { e.StudentUsi, e.GradeLevel, e.SchoolId })
                    .HasName("IX_StudentSchoolInformation");

                entity.HasIndex(e => new { e.StudentUsi, e.SchoolId, e.LateEnrollment })
                    .HasName("IX_StudentSchoolInformation_SchoolIdAndLateEnrollment");

                entity.Property(e => e.StudentUsi)
                    .HasColumnName("StudentUSI")
                    .HasComment("Identifier for the student attached to this school information");

                entity.Property(e => e.SchoolId).HasComment("Identifier for the school this student attends");

                entity.Property(e => e.DateOfEntry)
                    .HasColumnType("date")
                    .HasComment("Date the student entered the school system");

                entity.Property(e => e.DateOfWithdrawal)
                    .HasColumnType("date")
                    .HasComment("Date when the student withdrew from school");

                entity.Property(e => e.ExpectedGraduationYear)
                    .HasMaxLength(4)
                    .HasComment("Caldendar year the student is expected to graduate in");

                entity.Property(e => e.FeederSchools)
                    .HasMaxLength(120)
                    .HasComment("Information on feeder schools for the student - no data found");

                entity.Property(e => e.GradeLevel)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Current grade level of the student: \"Infant/toddler\", \"Kindergarten\", \"Preschool/Prekindergarten\", or \"[1st-12th] Grade\"");

                entity.Property(e => e.GraduationPlan)
                    .HasMaxLength(20)
                    .HasComment("Academic graduation plan the student has selected, if any : \"Distinguished\", \"Minimum\", \"Recommended\", or NULL");

                entity.Property(e => e.Homeroom)
                    .HasMaxLength(50)
                    .HasComment("First Initial and Last Name of the student's homeroom teacher");

                entity.Property(e => e.IncompleteTranscript)
                    .HasMaxLength(5)
                    .HasComment("Is this student's transcript incomlete?: \"YES\" or NULL");

                entity.Property(e => e.LateEnrollment)
                    .HasMaxLength(5)
                    .HasComment("Did this student enroll late? \"YES\" or NULL");

                entity.Property(e => e.WithdrawalCode)
                    .HasMaxLength(20)
                    .HasComment("Reason code the student withdrew from school - no data found");

                entity.Property(e => e.WithdrawalDescription)
                    .HasMaxLength(200)
                    .HasComment("Reason description for why the student withdrew from school");

                entity.HasOne(d => d.StudentUsiNavigation)
                    .WithMany(p => p.StudentSchoolInformation)
                    .HasForeignKey(d => d.StudentUsi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentSchoolInformation_StudentInformation");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
