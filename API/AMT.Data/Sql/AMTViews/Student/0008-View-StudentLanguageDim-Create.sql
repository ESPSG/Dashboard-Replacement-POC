IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentLanguageDim')
BEGIN
	DROP VIEW analytics.StudentLanguageDim
END
GO

CREATE VIEW analytics.StudentLanguageDim AS

	SELECT	Student.StudentUniqueId AS StudentKey
			, ldd.CodeValue as [Language]
			, ldud.CodeValue as LanguageUse
	FROM edfi.Student Student
		INNER JOIN edfi.StudentEducationOrganizationAssociationLanguage sl 
			ON Student.StudentUSI = sl.StudentUSI
		LEFT JOIN edfi.LanguageDescriptor ld
			ON sl.LanguageDescriptorId = ld.LanguageDescriptorId
		LEFT JOIN edfi.Descriptor ldd
			ON ldd.DescriptorId = ld.LanguageDescriptorId
		LEFT JOIN edfi.StudentEducationOrganizationAssociationLanguageUse slu
			ON sl.StudentUSI = slu.StudentUSI
			AND sl.LanguageDescriptorId = slu.LanguageDescriptorId
			AND sl.EducationOrganizationId = slu.EducationOrganizationId
		LEFT JOIN edfi.LanguageDescriptor ldu
			ON slu.LanguageDescriptorId = ldu.LanguageDescriptorId
		LEFT JOIN edfi.Descriptor ldud
			ON ldud.DescriptorId = ldu.LanguageDescriptorId
		LEFT JOIN [analytics].[StudentSchoolDim] StudentSchoolDim
			ON StudentSchoolDim.StudentKey = Student.StudentUniqueId
		LEFT JOIN edfi.School School 
			ON School.SchoolId = StudentSchoolDim.SchoolKey
GO