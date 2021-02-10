IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentAssessmentItemDim')
BEGIN
	DROP VIEW analytics.StudentAssessmentItemDim
END
GO

CREATE VIEW [analytics].StudentAssessmentItemDim AS
	SELECT	stu.StudentUniqueId as StudentKey
			, CAST(s.SchoolId AS VARCHAR) as SchoolKey
			, a.AssessmentIdentifier
			, a.Namespace
			, sa.StudentAssessmentIdentifier
			, sa.AdministrationDate
			, RTRIM(LTRIM(a.AssessmentTitle)) AS AssessmentTitle
			, AssessmentAcademicSubjectDescriptor.CodeValue AS AcademicSubject
			, AssessmentAssessedGradeLevelDescriptor.CodeValue AS AssessedGradeLevel
			, a.AssessmentVersion AS [Version]
			, AssessmentItemResultDescriptor.CodeValue AS AssessmentItemResult
			, sai.RawScoreResult
			, sai.AssessmentResponse
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
        AND sa.ReasonNotTestedDescriptorId IS NULL  -- Reason Not Tested holds precedence over assessment items    
    INNER JOIN edfi.StudentAssessmentItem sai
        ON sai.StudentUSI = sa.StudentUSI
        AND sai.AssessmentIdentifier = sa.AssessmentIdentifier
        AND sai.[Namespace] = sa.[Namespace]
        AND sai.StudentAssessmentIdentifier = sa.[StudentAssessmentIdentifier]
	INNER JOIN	edfi.Descriptor AS AssessmentAcademicSubjectDescriptor
            ON AssessmentAcademicSubjectDescriptor.DescriptorId = aas.AcademicSubjectDescriptorId
	INNER JOIN	edfi.Descriptor AS AssessmentAssessedGradeLevelDescriptor
            ON AssessmentAssessedGradeLevelDescriptor.DescriptorId = aagl.GradeLevelDescriptorId
	INNER JOIN	edfi.Descriptor AS AssessmentItemResultDescriptor
            ON AssessmentItemResultDescriptor.DescriptorId = sai.AssessmentItemResultDescriptorId
	INNER JOIN analytics.StudentSchoolDim ON StudentSchoolDim.StudentUSI = sa.StudentUSI
	INNER JOIN edfi.School s ON s.SchoolId = StudentSchoolDim.SchoolKey
    INNER JOIN edfi.Student stu ON stu.StudentUSI = sai.StudentUSI
GO