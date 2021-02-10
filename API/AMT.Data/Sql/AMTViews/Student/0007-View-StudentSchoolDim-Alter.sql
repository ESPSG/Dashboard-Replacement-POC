
ALTER VIEW [analytics].[StudentSchoolDim]
AS
    SELECT
        CONCAT(Student.StudentUniqueId, '-', StudentSchoolAssociation.SchoolId) AS StudentSchoolKey,
        Student.StudentUniqueId AS StudentKey,
        CAST(StudentSchoolAssociation.SchoolId AS VARCHAR) AS SchoolKey,
        COALESCE(CAST(StudentSchoolAssociation.SchoolYear AS VARCHAR), 'Unknown') AS SchoolYear,
        Student.FirstName AS StudentFirstName,
        COALESCE(Student.MiddleName, '') AS StudentMiddleName,
        COALESCE(Student.LastSurname, '') AS StudentLastName,
        CAST(StudentSchoolAssociation.EntryDate AS NVARCHAR) AS EnrollmentDateKey,
        Descriptor.CodeValue AS GradeLevel,
        COALESCE(CASE
                    WHEN schoolEdOrg.StudentUSI IS NOT NULL
                    THEN LimitedEnglishDescriptorSchool.CodeValue
                    ELSE LimitedEnglishDescriptorDist.CodeValue
                END, 'Not applicable') AS LimitedEnglishProficiency,
        COALESCE(CASE
                    WHEN schoolEdOrg.StudentUSI IS NOT NULL
                    THEN schoolEdOrg.HispanicLatinoEthnicity
                    ELSE districtEdOrg.HispanicLatinoEthnicity
                END, CAST(0 as BIT)) AS IsHispanic,
        COALESCE(CASE
                    WHEN schoolEdOrg.StudentUSI IS NOT NULL
                    THEN SexTypeSchool.CodeValue
                    ELSE SexTypeDist.CodeValue
                END, '') AS Sex,
        (
            SELECT
                MAX(MaxLastModifiedDate)
            FROM (VALUES
                (Student.LastModifiedDate)
                ,(schoolEdOrg.LastModifiedDate)
                ,(districtEdOrg.LastModifiedDate)
            ) AS VALUE(MaxLastModifiedDate)
        ) AS LastModifiedDate,
		--added
		 StudentSchoolAssociation.[EntryDate],
		 StudentSchoolAssociation.[ExitWithdrawDate] as ExitWithdrawDate,
		 Student.StudentUSI AS StudentUSI,
		 GraduationPlanTypeDescriptor.CodeValue AS GraduationPlan,
		 ExitWithdrawTypeDescriptor.CodeValue AS WithdrawalDescription,
         School.LocalEducationAgencyId AS LocalEducationAgencyKey
		 , ROW_NUMBER() OVER (PARTITION BY StudentSchoolAssociation.[StudentUSI] 
            ORDER BY 
            StudentSchoolAssociation.[StudentUSI], 
            CASE WHEN StudentSchoolAssociation.ExitWithdrawDate IS NULL THEN 1 ELSE 2 END,
            StudentSchoolAssociation.ExitWithdrawDate DESC,
            StudentSchoolAssociation.[EntryDate] ASC,
            school.[LocalEducationAgencyId],
            StudentSchoolAssociation.[SchoolId]) AS StudentSchoolAssociationOrderKey
		, ROW_NUMBER() OVER (PARTITION BY StudentSchoolAssociation.[StudentUSI], School.[LocalEducationAgencyId] 
			ORDER BY
			StudentSchoolAssociation.[StudentUSI],
			CASE WHEN StudentSchoolAssociation.ExitWithdrawDate IS NULL THEN 1 ELSE 2 END,
			StudentSchoolAssociation.ExitWithdrawDate DESC,
			StudentSchoolAssociation.[EntryDate] ASC,
			School.[LocalEducationAgencyId],
			StudentSchoolAssociation.[SchoolId]) AS StudentLeaSchoolAssociationOrderKey
		,CAST
			(
				(
				CASE 
					WHEN
						StudentSchoolAssociation.EntryDate <= mmd.MaxDate
						AND (StudentSchoolAssociation.ExitWithdrawDate IS NULL OR StudentSchoolAssociation.ExitWithdrawDate >= mmd.MaxDate)
					THEN 1
					ELSE 0
				END
				)
			AS BIT
			) AS IsEnrolledToSchool
    FROM
        edfi.Student
    INNER JOIN
        edfi.StudentSchoolAssociation ON
            Student.StudentUSI = StudentSchoolAssociation.StudentUSI
    INNER JOIN
        edfi.Descriptor ON
            StudentSchoolAssociation.EntryGradeLevelDescriptorId = Descriptor.DescriptorId
    INNER JOIN
        edfi.School ON
            StudentSchoolAssociation.SchoolId = School.SchoolId
    LEFT OUTER JOIN
        edfi.StudentEducationOrganizationAssociation AS schoolEdOrg ON
            Student.StudentUSI = schoolEdOrg.StudentUSI
            AND StudentSchoolAssociation.SchoolId = schoolEdOrg.EducationOrganizationId
    LEFT OUTER JOIN
        edfi.Descriptor AS LimitedEnglishDescriptorSchool ON
            schoolEdOrg.LimitedEnglishProficiencyDescriptorId = LimitedEnglishDescriptorSchool.DescriptorId
    LEFT OUTER JOIN
        edfi.Descriptor AS SexTypeSchool ON
            schoolEdOrg.SexDescriptorId = SexTypeSchool.DescriptorId
    LEFT OUTER JOIN
        edfi.StudentEducationOrganizationAssociation AS districtEdOrg ON
            Student.StudentUSI = districtEdOrg.StudentUSI
            AND School.LocalEducationAgencyId = districtEdOrg.EducationOrganizationId
    LEFT OUTER JOIN
        edfi.Descriptor AS LimitedEnglishDescriptorDist ON
            districtEdOrg.LimitedEnglishProficiencyDescriptorId = LimitedEnglishDescriptorDist.DescriptorId
    LEFT OUTER JOIN
        edfi.Descriptor AS SexTypeDist ON
            districtEdOrg.SexDescriptorId = SexTypeDist.DescriptorId
	LEFT OUTER JOIN
        edfi.Descriptor AS GraduationPlanTypeDescriptor ON
            StudentSchoolAssociation.GraduationPlanTypeDescriptorId = GraduationPlanTypeDescriptor.DescriptorId
	LEFT OUTER JOIN
        edfi.Descriptor AS ExitWithdrawTypeDescriptor ON
            StudentSchoolAssociation.ExitWithdrawTypeDescriptorId = ExitWithdrawTypeDescriptor.DescriptorId
	LEFT JOIN analytics.SchoolMinMaxDateDim mmd 
	 ON mmd.SchoolKey = StudentSchoolAssociation.SchoolId
    WHERE(
        StudentSchoolAssociation.ExitWithdrawDate IS NULL
        OR StudentSchoolAssociation.ExitWithdrawDate >= GETDATE());
GO


