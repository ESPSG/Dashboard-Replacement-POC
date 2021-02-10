IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentDim')
BEGIN
	DROP VIEW analytics.StudentDim
END
GO

CREATE VIEW analytics.StudentDim AS

		WITH AddressPriority ([Order], AddressCodeDescription) AS (
		SELECT 1, 'Home'
		UNION ALL SELECT 2, 'Mailing'
		UNION ALL SELECT 3, 'Work'
		UNION ALL SELECT 4, 'Billing'
		UNION ALL SELECT 5, 'Physical'
		UNION ALL SELECT 6, 'Guardian Address'
		UNION ALL SELECT 7, 'Mother Address'
		UNION ALL SELECT 8, 'Father Address'
		UNION ALL SELECT 9, 'Temporary'
		UNION ALL SELECT 10, 'Other'
		UNION ALL SELECT 11, 'Shipping'
		UNION ALL SELECT 12, 'Shelters, Transitional housing, Awaiting Foster Care'
		UNION ALL SELECT 13, 'Doubled - up (i.e., living with another family)'
		UNION ALL SELECT 14, 'Unsheltered (e.g. cars, parks, campgrounds, temporary trailers including FEMA trailers, or abandoned buildings)'
		UNION ALL SELECT 15, 'Hotels/Motels'
	),  
	studentAddress (StudentUSI, StreetNumberName, ApartmentRoomSuiteNumber, BuildingSiteNumber, City, StateAbbreviationDescriptor, PostalCode, [Priority]) AS (
		SELECT
			t.StudentUSI
			, t.StreetNumberName
			, t.ApartmentRoomSuiteNumber
			, t.BuildingSiteNumber
			, t.City
			, t.StateAbbreviationDescriptorId
			, t.PostalCode
			, ROW_NUMBER() OVER (PARTITION BY t.StudentUSI ORDER BY t.IsStudentAddress DESC) as [Priority]
		FROM
		(
			SELECT
				1 AS IsStudentAddress
				, sa.StudentUSI
				, sa.StreetNumberName
				, sa.ApartmentRoomSuiteNumber
				, sa.BuildingSiteNumber
				, sa.City
				, sa.StateAbbreviationDescriptorId
				, sa.PostalCode
				, ROW_NUMBER() OVER (PARTITION BY sa.StudentUSI ORDER BY AddressPriority.[Order]) as RowNumber
			FROM edfi.StudentEducationOrganizationAssociationAddress sa
				INNER JOIN edfi.Descriptor d ON d.DescriptorId = sa.AddressTypeDescriptorId
				INNER JOIN AddressPriority ON AddressCodeDescription = d.[Description]
			WHERE sa.DoNotPublishIndicator = 0

			UNION ALL

			SELECT
				0 AS IsStudentAddress
				, spa.StudentUSI
				, pa.StreetNumberName
				, pa.ApartmentRoomSuiteNumber
				, pa.BuildingSiteNumber
				, pa.City
				, pa.StateAbbreviationDescriptorId
				, pa.PostalCode
				, ROW_NUMBER() OVER (PARTITION BY spa.StudentUSI ORDER BY spa.LivesWith DESC, spa.PrimaryContactStatus DESC, spa.ParentUSI, AddressPriority.[Order]) as RowNumber
			FROM edfi.StudentParentAssociation spa
				INNER JOIN edfi.ParentAddress pa ON pa.ParentUSI = spa.ParentUSI
				INNER JOIN edfi.Descriptor d ON d.DescriptorId = pa.AddressTypeDescriptorId
				INNER JOIN AddressPriority ON AddressCodeDescription = d.[Description]
			WHERE DoNotPublishIndicator = 0
		) t
		WHERE RowNumber = 1
	)

	select   Student.StudentUniqueId AS StudentKey
			, Student.LastSurname
			, Student.FirstName
			, Student.MiddleName
			, Descriptor.CodeValue as Gender
			, StudentRace.Races
			, StudentCharacteristics.Characteristics
			, StudentIndicators.Indicators
			, studentEdOrg.HispanicLatinoEthnicity
			, StudentSchoolDim.SchoolKey
			, StudentSchoolDim.GradeLevel
			, CASE  StudentSchoolDim.GradeLevel
					WHEN 'Early Education' THEN -3
					WHEN 'Infant/toddler' THEN -2
					WHEN 'Preschool/Prekindergarten' THEN -1
					WHEN 'Kindergarten' THEN 0
					WHEN 'First grade' THEN 1
					WHEN 'Second grade' THEN 2
					WHEN 'Third grade' THEN 3
					WHEN 'Fourth grade' THEN 4
					WHEN 'Fifth grade' THEN 5
					WHEN 'Sixth grade' THEN 6
					WHEN 'Seventh grade' THEN 7
					WHEN 'Eighth grade' THEN 8
					WHEN 'Ninth grade' THEN 9
					WHEN 'Tenth grade' THEN 10
					WHEN 'Eleventh grade' THEN 11
					WHEN 'Twelfth grade' THEN 12
					WHEN 'Postsecondary' THEN 13
					WHEN 'Ungraded' THEN 14
					WHEN 'Other' THEN 15
					WHEN 'Grade 13' THEN 16
					WHEN 'Adult Education' THEN 17
					ELSE 18
				END AS [SortOrder]
			   ,CASE StudentSchoolDim.GradeLevel
					WHEN 'Early Education' THEN 'E-E'
					WHEN 'Infant/toddler' THEN 'Inf'
					WHEN 'Preschool/Prekindergarten' THEN 'Pre'
					WHEN 'Kindergarten' THEN 'K'
					WHEN 'First grade' THEN '1st'
					WHEN 'Second grade' THEN '2nd'
					WHEN 'Third grade' THEN '3rd'
					WHEN 'Fourth grade' THEN '4th'
					WHEN 'Fifth grade' THEN '5th'
					WHEN 'Sixth grade' THEN '6th'
					WHEN 'Seventh grade' THEN '7th'
					WHEN 'Eighth grade' THEN '8th'
					WHEN 'Ninth grade' THEN '9th'
					WHEN 'Tenth grade' THEN '10th'
					WHEN 'Eleventh grade' THEN '11th'
					WHEN 'Twelfth grade' THEN '12th'
					WHEN 'Postsecondary' THEN 'Post'
					WHEN 'Ungraded' THEN 'U'
					WHEN 'Other' THEN 'O'
					WHEN 'Grade 13' THEN '13'
					WHEN 'Adult Education' THEN 'A-E'
					ELSE 'Undef'
				END AS [GradeLevelDisplayText]
			, EducationOrganization.NameOfInstitution AS SchoolName
			, SchoolDescriptor.CodeValue AS SchoolCategory
			, OldEthnicityDescriptor.CodeValue as OldEthnicity
			, sa.StreetNumberName as AddressStreetNumberName
			, sa.ApartmentRoomSuiteNumber as AddressApartmentRoomSuiteNumber
			, sa.BuildingSiteNumber as AddressBuildingSiteNumber
			, sa.City as AddressCity
			, sa.StateAbbreviationDescriptor as AddressState
			, sa.PostalCode as AddressPostalCode
			, Student.BirthDate
			, LimitedEnglishProficiencyDescriptor.CodeValue AS LimitedEnglishProficiency
			, CAST((CASE WHEN EXISTS
					(SELECT 1
					FROM edfi.StudentEducationOrganizationAssociationStudentCharacteristic sc
					INNER JOIN edfi.StudentCharacteristicDescriptor StudentCharacteristicDescriptor
						ON StudentCharacteristicDescriptor.StudentCharacteristicDescriptorId=sc.StudentCharacteristicDescriptorId
					WHERE sc.StudentUSI = student.StudentUSI)
					THEN 1
					ELSE 0
					END)
				AS BIT) AS EconomicDisadvantaged
			, CAST((CASE WHEN EXISTS
				(SELECT 1
				FROM edfi.StudentSchoolFoodServiceProgramAssociationSchoolFoodServiceProgramService sc
				WHERE sc.StudentUSI = Student.StudentUSI
					AND sc.SchoolFoodServiceProgramServiceDescriptorId IN (
						SELECT d.DescriptorId from edfi.SchoolFoodServiceProgramServiceDescriptor f
						INNER JOIN edfi.Descriptor d on f.SchoolFoodServiceProgramServiceDescriptorId = d.DescriptorId
						WHERE d.CodeValue LIKE 'Free%'
						OR d.CodeValue LIKE 'Reduced Price%'
					))
				THEN 1
				ELSE 0
				END)
			AS BIT) AS HasFreeOrReducedPriceFoodServiceEligibility
			,  StudentSchoolDim.LocalEducationAgencyKey
			, StudentSchoolDim.IsEnrolledToSchool
			, StudentSchoolDim.StudentSchoolAssociationOrderKey
			, StudentSchoolDim.StudentLeaSchoolAssociationOrderKey
	FROM edfi.Student Student
	INNER JOIN analytics.StudentSchoolDim StudentSchoolDim
		ON Student.StudentUSI=StudentSchoolDim.StudentUSI
	INNER JOIN edfi.StudentEducationOrganizationAssociation studentEdOrg
		ON Student.StudentUSI = studentEdOrg.StudentUSI
	INNER JOIN edfi.EducationOrganization EducationOrganization
		ON EducationOrganization.EducationOrganizationId = StudentSchoolDim.SchoolKey
	INNER JOIN edfi.Descriptor Descriptor
		ON Descriptor.DescriptorId = studentEdOrg.SexDescriptorId
	INNER JOIN edfi.SchoolCategory SchoolCategory
		ON SchoolCategory.SchoolId=StudentSchoolDim.SchoolKey
	INNER JOIN edfi.SchoolCategoryDescriptor SchoolCategoryDescriptor
		ON SchoolCategoryDescriptor.SchoolCategoryDescriptorId=SchoolCategory.SchoolCategoryDescriptorId
	INNER JOIN  edfi.Descriptor SchoolDescriptor
		ON SchoolDescriptor.DescriptorId=SchoolCategoryDescriptor.SchoolCategoryDescriptorId
	LEFT JOIN studentAddress sa ON sa.StudentUSI = student.StudentUSI AND sa.[Priority] = 1
	LEFT JOIN (
				SELECT student.StudentUSI,
						SUBSTRING(
							(SELECT ',' + descriptor.description
							FROM edfi.StudentEducationOrganizationAssociationRace sr
							INNER JOIN edfi.Descriptor descriptor
								ON descriptor.DescriptorId=sr.RaceDescriptorId
							WHERE sr.StudentUSI = Student.StudentUSI
							ORDER BY sr.RaceDescriptorId FOR XML PATH('')), 2, 2000000) AS Races
					FROM edfi.Student student
			 ) AS StudentRace 
		ON student.StudentUSI = StudentRace.StudentUSI
	LEFT JOIN (
				 SELECT	Student.StudentUSI,
						SUBSTRING(
							(SELECT ',' + descriptor.description
							FROM edfi.StudentEducationOrganizationAssociationStudentCharacteristic sc
							INNER JOIN edfi.Descriptor descriptor
								ON descriptor.DescriptorId=sc.StudentCharacteristicDescriptorId
							WHERE sc.StudentUSI = Student.StudentUSI
							ORDER BY sc.StudentCharacteristicDescriptorId FOR XML PATH('')), 2, 2000000) AS Characteristics
				FROM edfi.Student student
    ) AS StudentCharacteristics ON student.StudentUSI = StudentCharacteristics.StudentUSI
	LEFT JOIN (
				 SELECT	Student.StudentUSI,
						SUBSTRING(
							(SELECT ',' + si.[IndicatorName]
							FROM edfi.[StudentEducationOrganizationAssociationStudentIndicator] si
							WHERE si.StudentUSI = Student.StudentUSI 
							AND si.Indicator = 'True'
							ORDER BY si.[IndicatorName] FOR XML PATH('')), 2, 2000000) AS Indicators
				FROM edfi.Student student
    ) AS StudentIndicators ON student.StudentUSI = StudentIndicators.StudentUSI
	LEFT JOIN edfi.Descriptor OldEthnicityDescriptor
		ON OldEthnicityDescriptor.DescriptorId = studentEdOrg.OldEthnicityDescriptorId
	LEFT JOIN edfi.LimitedEnglishProficiencyDescriptor lepd
        ON studentEdOrg.LimitedEnglishProficiencyDescriptorId = lepd.LimitedEnglishProficiencyDescriptorId
	LEFT JOIN edfi.Descriptor LimitedEnglishProficiencyDescriptor
        ON LimitedEnglishProficiencyDescriptor.DescriptorId = lepd.LimitedEnglishProficiencyDescriptorId
GO