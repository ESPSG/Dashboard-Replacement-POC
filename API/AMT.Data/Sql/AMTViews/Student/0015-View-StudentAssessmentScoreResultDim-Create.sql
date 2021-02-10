IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentAssessmentScoreResultDim')
BEGIN
	DROP VIEW analytics.StudentAssessmentScoreResultDim
END
GO

CREATE VIEW [analytics].StudentAssessmentScoreResultDim AS
	SELECT
		stu.StudentUniqueId as StudentKey
		, CAST(s.SchoolId AS VARCHAR) as SchoolKey
		, a.AssessmentIdentifier
		, sa.StudentAssessmentIdentifier
		, LTRIM(RTRIM(a.AssessmentTitle)) AS AssessmentTitle
		, AssessmentAcademicSubjectDescriptor.CodeValue AS AcademicSubject
		, AssessmentAssessedGradeLevelDescriptor.CodeValue AS AssessedGradeLevel
		, a.AssessmentVersion AS [Version]
		, sa.AdministrationDate
		, sasr.Result
		, StudentAssessmentScoreResultDescriptor.CodeValue AS ReportingMethod
		, ResultDatatypeTypeDescriptor.CodeValue AS ResultDataType
		, a.[Namespace]
	FROM edfi.Assessment a
		INNER JOIN edfi.AssessmentAcademicSubject aas
			ON aas.AssessmentIdentifier = a.AssessmentIdentifier
			AND aas.[Namespace] = a.[Namespace]
		INNER JOIN edfi.AssessmentAssessedGradeLevel aagl
			ON aagl.AssessmentIdentifier = a.AssessmentIdentifier
			AND aagl.[Namespace] = a.[Namespace]
		INNER JOIN edfi.StudentAssessment sa
			ON sa.AssessmentIdentifier = a.AssessmentIdentifier
			AND sa.[Namespace] = a.[Namespace]
		INNER JOIN edfi.StudentAssessmentScoreResult sasr
			ON sasr.StudentUSI = sa.StudentUSI
			AND sasr.StudentAssessmentIdentifier = sa.StudentAssessmentIdentifier
			AND sasr.AssessmentIdentifier = sa.AssessmentIdentifier
			AND sasr.[Namespace] = sa.[Namespace]
		INNER JOIN	edfi.Descriptor AS StudentAssessmentScoreResultDescriptor
            ON StudentAssessmentScoreResultDescriptor.DescriptorId = sasr.AssessmentReportingMethodDescriptorId
		INNER JOIN	edfi.Descriptor AS AssessmentAcademicSubjectDescriptor
            ON AssessmentAcademicSubjectDescriptor.DescriptorId = aas.AcademicSubjectDescriptorId
		INNER JOIN	edfi.Descriptor AS AssessmentAssessedGradeLevelDescriptor
            ON AssessmentAssessedGradeLevelDescriptor.DescriptorId = aagl.GradeLevelDescriptorId
		INNER JOIN	edfi.Descriptor AS ResultDatatypeTypeDescriptor
            ON ResultDatatypeTypeDescriptor.DescriptorId = sasr.ResultDatatypeTypeDescriptorId
		INNER JOIN analytics.StudentSchoolDim ON StudentSchoolDim.StudentUSI = sa.StudentUSI
		INNER JOIN edfi.School s ON s.SchoolId = StudentSchoolDim.SchoolKey
		INNER JOIN edfi.Student stu ON stu.StudentUSI = sa.StudentUSI
GO