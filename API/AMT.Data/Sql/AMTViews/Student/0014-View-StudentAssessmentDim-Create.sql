IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentAssessmentDim')
BEGIN
	DROP VIEW analytics.StudentAssessmentDim
END
GO

CREATE VIEW [analytics].StudentAssessmentDim AS

	WITH StudentAssessmentPerformanceLevelPrioritized AS
		(
		SELECT
			p.StudentUSI
			, p.AssessmentIdentifier
			, p.StudentAssessmentIdentifier
			, p.[Namespace]
			, p.AssessmentReportingMethodDescriptorId
			, p.PerformanceLevelDescriptorId
			, p.PerformanceLevelMet        
			, ROW_NUMBER() OVER (
				PARTITION BY
					p.StudentUSI
					, p.AssessmentIdentifier
					, p.StudentAssessmentIdentifier
					, p.[Namespace]
					, p.AssessmentReportingMethodDescriptorId
				ORDER BY
					p.PerformanceLevelMet DESC
					, p.PerformanceLevelDescriptorId DESC
				) AS [Priority]
		FROM edfi.StudentAssessmentPerformanceLevel p
	)
	SELECT
		  stu.StudentUniqueId as StudentKey
		, CAST(s.SchoolId AS VARCHAR) as SchoolKey
		, a.AssessmentIdentifier
		, sa.StudentAssessmentIdentifier
		, AssessmentCategoryDescriptor.CodeValue AS AssessmentCategory
		, LTRIM(RTRIM(a.AssessmentTitle)) AS AssessmentTitle
		, AssessmentAcademicSubjectDescriptor.CodeValue AS AcademicSubject
		, AssessmentAssessedGradeLevelDescriptor.CodeValue AS AssessedGradeLevel
		, a.AssessmentVersion AS [Version]
		, sa.AdministrationDate
		, ReasonNotTestedDescriptor.CodeValue AS ReasonNotTested
		, a.MaxRawScore AS MaxScoreResult
		, sapl.PerformanceLevelDescriptorId AS PerformanceLevel
		, sapl.PerformanceLevelMet
		, a.Namespace
	FROM edfi.Assessment a
		INNER JOIN edfi.AssessmentAcademicSubject aas
			ON aas.AssessmentIdentifier = a.AssessmentIdentifier
			AND aas.[Namespace] = a.[Namespace]    
		INNER JOIN edfi.StudentAssessment sa
			ON sa.AssessmentIdentifier = a.AssessmentIdentifier
			AND sa.[Namespace] = a.[Namespace]
		INNER JOIN edfi.AssessmentAssessedGradeLevel aagl
			ON aagl.AssessmentIdentifier = a.AssessmentIdentifier
			AND aagl.[Namespace] = a.[Namespace]
		INNER JOIN	edfi.Descriptor AS AssessmentCategoryDescriptor
            ON AssessmentCategoryDescriptor.DescriptorId = a.AssessmentCategoryDescriptorId
		INNER JOIN	edfi.Descriptor AS AssessmentAcademicSubjectDescriptor
            ON AssessmentAcademicSubjectDescriptor.DescriptorId = aas.AcademicSubjectDescriptorId
		INNER JOIN	edfi.Descriptor AS AssessmentAssessedGradeLevelDescriptor
            ON AssessmentAssessedGradeLevelDescriptor.DescriptorId = aagl.GradeLevelDescriptorId
		LEFT JOIN	edfi.Descriptor AS ReasonNotTestedDescriptor
            ON ReasonNotTestedDescriptor.DescriptorId = sa.ReasonNotTestedDescriptorId
		LEFT JOIN StudentAssessmentPerformanceLevelPrioritized sapl
			ON sapl.StudentUSI = sa.StudentUSI
			AND sapl.AssessmentIdentifier = a.AssessmentIdentifier
			AND sapl.StudentAssessmentIdentifier = sa.StudentAssessmentIdentifier
			AND sapl.[Namespace] = a.[Namespace]
			AND sapl.[Priority] = 1
		INNER JOIN analytics.StudentSchoolDim ON StudentSchoolDim.StudentUSI = sa.StudentUSI
		INNER JOIN edfi.School s ON s.SchoolId = StudentSchoolDim.SchoolKey
		INNER JOIN edfi.Student stu ON stu.StudentUSI = sa.StudentUSI
GO