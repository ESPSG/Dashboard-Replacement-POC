IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'ProgramDim')
BEGIN
	DROP VIEW analytics.ProgramDim
END
GO

CREATE VIEW [analytics].ProgramDim AS

	SELECT	p.EducationOrganizationId AS EducationOrganizationKey
			, d.CodeValue as ProgramType
			, p.ProgramName
	FROM edfi.Program p
		INNER JOIN edfi.ProgramTypeDescriptor ptd ON ptd.ProgramTypeDescriptorId = p.ProgramTypeDescriptorId
		INNER JOIN edfi.Descriptor d ON d.DescriptorId = ptd.ProgramTypeDescriptorId 
		INNER JOIN edfi.LocalEducationAgency lea ON lea.LocalEducationAgencyId = p.EducationOrganizationId
	WHERE
		d.CodeValue IN (
			'Section 504 Placement',
			'Bilingual',
			'Bilingual Summer',
			'Career and Technical Education',
			'English as a Second Language (ESL)',
			'Gifted and Talented',
			'Special Education',
			'Title I Part A'
		)
	OR (d.CodeValue = 'Other' AND p.ProgramName LIKE 'Food Service%')
GO