IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentDisciplineDim')
BEGIN
	DROP VIEW analytics.StudentDisciplineDim
END
GO

CREATE VIEW [analytics].StudentDisciplineDim AS

	SELECT DISTINCT stu.StudentUniqueId AS StudentKey
				,di.IncidentIdentifier
				,CAST(sdia.SchoolId AS VARCHAR) as SchoolKey
				,di.IncidentDate
				,BehaviorDescriptor.CodeValue AS BehaviorDescription
				,DisciplineDescriptor.CodeValue AS DisciplineDescription
				,dib.BehaviorDetailedDescription
	FROM edfi.Student stu
	INNER JOIN analytics.StudentDim  ON StudentDim.StudentKey = stu.StudentUniqueId
    INNER JOIN edfi.StudentDisciplineIncidentAssociation sdia ON sdia.StudentUSI = stu.StudentUSI
    INNER JOIN edfi.StudentParticipationCodeDescriptor spct
        ON spct.StudentParticipationCodeDescriptorId = sdia.StudentParticipationCodeDescriptorId
	INNER JOIN edfi.Descriptor StudentParticipationDescriptor
        ON StudentParticipationDescriptor.DescriptorId = spct.StudentParticipationCodeDescriptorId
    INNER JOIN edfi.DisciplineIncident DI
        ON sdia.SchoolId = di.SchoolId
        AND sdia.IncidentIdentifier = di.IncidentIdentifier
	
    CROSS APPLY
    (
        SELECT
            d.BehaviorDescriptorId,
            d.BehaviorDetailedDescription
        FROM edfi.DisciplineIncidentBehavior d
        WHERE d.IncidentIdentifier = di.IncidentIdentifier
            AND d.SchoolId = di.SchoolId
            AND NOT EXISTS(
                SELECT d.IncidentIdentifier
                FROM edfi.StudentDisciplineIncidentAssociationBehavior d2
                WHERE d2.IncidentIdentifier = di.IncidentIdentifier
                AND d2.SchoolId = di.SchoolId
                AND d2.StudentUSI = sdia.StudentUSI
            )

            UNION

        SELECT
            d.BehaviorDescriptorId,
            d.BehaviorDetailedDescription
        FROM edfi.StudentDisciplineIncidentAssociationBehavior d
        WHERE d.IncidentIdentifier = di.IncidentIdentifier
            AND d.SchoolId = di.SchoolId
            AND d.StudentUSI = sdia.StudentUSI
        ) DIB
	LEFT JOIN edfi.Descriptor BehaviorDescriptor ON BehaviorDescriptor.DescriptorId= dib.BehaviorDescriptorId
    LEFT JOIN
    (
        SELECT
            -- There may be more than one discipline action for the same incident with the same discipline descriptor
            DISTINCT
            dadi.StudentUSI
            , dadi.IncidentIdentifier
            , dadi.SchoolId
            , dad.DisciplineDescriptorId
        FROM edfi.DisciplineActionStudentDisciplineIncidentAssociation dadi
            INNER JOIN edfi.DisciplineAction da
                ON da.StudentUSI = dadi.StudentUSI
                AND da.DisciplineActionIdentifier = dadi.DisciplineActionIdentifier
                AND da.DisciplineDate = dadi.DisciplineDate
            INNER JOIN edfi.DisciplineActionDiscipline dad
                ON dad.StudentUSI = da.StudentUSI
                AND dad.DisciplineActionIdentifier = da.DisciplineActionIdentifier
                AND dad.DisciplineDate = da.DisciplineDate
    ) UniqueDisciplineActionDiscipline
        ON UniqueDisciplineActionDiscipline.StudentUSI = sdia.StudentUSI
        AND UniqueDisciplineActionDiscipline.IncidentIdentifier = sdia.IncidentIdentifier
        AND UniqueDisciplineActionDiscipline.SchoolId = sdia.SchoolId
   LEFT JOIN edfi.Descriptor DisciplineDescriptor ON DisciplineDescriptor.PriorDescriptorId= UniqueDisciplineActionDiscipline.DisciplineDescriptorId
   WHERE StudentParticipationDescriptor.CodeValue='Perpetrator' AND StudentDim.IsEnrolledToSchool=1

GO