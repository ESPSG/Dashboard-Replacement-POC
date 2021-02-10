IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentProgramAssociationDim')
BEGIN
	DROP VIEW analytics.StudentProgramAssociationDim
END
GO

CREATE VIEW [analytics].StudentProgramAssociationDim AS

	SELECT	stu.StudentUniqueId as StudentKey
			, ProgramTypeDescriptor.CodeValue as ProgramType
			, [BeginDate]
			, [EndDate]
			, P.EducationOrganizationId AS EducationOrganizationKey
	FROM edfi.Student stu
		INNER JOIN edfi.GeneralStudentProgramAssociation spa ON spa.StudentUSI = stu.StudentUSI
		INNER JOIN analytics.StudentSchoolDim ssa ON ssa.StudentKey = stu.StudentUniqueId
		INNER JOIN edfi.School s ON ssa.SchoolKey = s.SchoolId
		INNER JOIN edfi.Descriptor ProgramTypeDescriptor ON ProgramTypeDescriptor.DescriptorId=spa.ProgramTypeDescriptorId
		INNER JOIN edfi.Program p ON p.EducationOrganizationId=s.LocalEducationAgencyId 
			AND p.ProgramTypeDescriptorId = spa.ProgramTypeDescriptorId
			AND p.ProgramName = spa.ProgramName
GO