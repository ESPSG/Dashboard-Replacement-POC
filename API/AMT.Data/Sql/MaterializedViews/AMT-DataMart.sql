-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.


/*
This script will create a data mart with the views materialized into tables.
Run this with sqlcmd or in SSMS with "sqlcmd mode" turned on. Adjust the two 
variables immediately below as needed. Assumes that the destination database 
already exists and is on the same server as the ODS. Run the script 
periodically to refresh the data.
*/


:setvar DataMartDB GDale_3X_AMT_Datamart
:setvar OdsDb GDale_EdFi


USE [$(DataMartDB)]
GO

IF NOT EXISTS (SELECT 1 FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE SCHEMA_NAME = 'analytics')
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [analytics]';
END

IF NOT EXISTS (SELECT 1 FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE SCHEMA_NAME = 'analytics_config')
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [analytics_config]';
END
----------------------------
-- Populate Staging Tables
----------------------------
PRINT 'Creating staging tables...'

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_GradingPeriodDim]
FROM [$(OdsDb)].[analytics].[GradingPeriodDim]

SELECT * 
INTO [$(DataMartDB)].[analytics].[stg_SchoolMinMaxDateDim]
FROM [$(OdsDb)].[analytics].[SchoolMinMaxDateDim]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_CurrentSchoolYearDim]
FROM [$(OdsDb)].[analytics].[CurrentSchoolYearDim]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_SchoolCalendarDim]
FROM [$(OdsDb)].[analytics].[SchoolCalendarDim]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentSectionAttendanceEventFact]
FROM [$(OdsDb)].[analytics].[StudentSectionAttendanceEventFact]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentSchoolAttendanceEventFact]
FROM [$(OdsDb)].[analytics].[StudentSchoolAttendanceEventFact]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentSectionDim]
FROM [$(OdsDb)].[analytics].[StudentSectionDim]

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentSchoolDim]
FROM [$(OdsDb)].[analytics].[StudentSchoolDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentDim]
FROM [$(OdsDb)].[analytics].[StudentDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentLanguageDim]
FROM [$(OdsDb)].[analytics].[StudentLanguageDim]
GO


SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentDisciplineDim]
FROM [$(OdsDb)].[analytics].[StudentDisciplineDim]
GO

--RLS related tables
SELECT *
INTO [$(DataMartDB)].[analytics].[stg_rls_StudentDataAuthorization]
FROM [$(OdsDb)].[analytics].[rls_StudentDataAuthorization]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_rls_UserDim]
FROM [$(OdsDb)].[analytics].[rls_UserDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_rls_UserAuthorization]
FROM [$(OdsDb)].[analytics].[rls_UserAuthorization]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_rls_UserStudentDataAuthorization]
FROM [$(OdsDb)].[analytics].[rls_UserStudentDataAuthorization]
GO

SELECT *
INTO [$(DataMartDB)].[analytics_config].[stg_rls_StaffClassificationDescriptorScopeList]
FROM [$(OdsDb)].[analytics_config].[rls_StaffClassificationDescriptorScopeList]
GO
--Remove when DDS is not in place
SELECT *
INTO [$(DataMartDB)].[analytics].[stg_WarehouseStudentUSIMappingDim]
FROM [$(OdsDb)].[analytics].[WarehouseStudentUSIMappingDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_ProgramDim]
FROM [$(OdsDb)].[analytics].[ProgramDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentProgramAssociationDim]
FROM [$(OdsDb)].[analytics].[StudentProgramAssociationDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_ContactPersonDim]
FROM [$(OdsDb)].[analytics].[ContactPersonDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentAssessmentDim]
FROM [$(OdsDb)].[analytics].[StudentAssessmentDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentAssessmentScoreResultDim]
FROM [$(OdsDb)].[analytics].[StudentAssessmentScoreResultDim]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentGradeDim]
FROM [$(OdsDb)].[analytics].[StudentGradeDim]
GO

SELECT *
INTO [$(DataMartDB)].analytics_config.[stg_GradingScale]
FROM [$(OdsDb)].[analytics_config].[GradingScale]
GO

SELECT *
INTO [$(DataMartDB)].[analytics_config].[stg_GradingScaleGrade]
FROM [$(OdsDb)].[analytics_config].[GradingScaleGrade]
GO

SELECT *
INTO [$(DataMartDB)].[analytics_config].[stg_GradingScaleGradeLevel]
FROM [$(OdsDb)].[analytics_config].[GradingScaleGradeLevel]
GO

SELECT *
INTO [$(DataMartDB)].[analytics_config].[stg_GradingScaleMetricThreshold]
FROM [$(OdsDb)].[analytics_config].[GradingScaleMetricThreshold]
GO

SELECT *
INTO [$(DataMartDB)].[analytics_config].[stg_GradeLevelType]
FROM [$(OdsDb)].[analytics_config].[GradeLevelType]
GO

SELECT *
INTO [$(DataMartDB)].[analytics].[stg_StudentAssessmentItemDim]
FROM [$(OdsDb)].[analytics].[StudentAssessmentItemDim]
GO

----------------------------
-- Add Indexes to Staging Tables
----------------------------

PRINT 'Adding indexes to staging tables...'

CREATE UNIQUE CLUSTERED INDEX [UCX_GradingPeriodDim]
ON [analytics].[stg_GradingPeriodDim] (
	[GradingPeriodKey]
) ON [Primary]

CREATE NONCLUSTERED INDEX [IX_GradingPeriodDim_SchoolKey]
ON [analytics].[stg_GradingPeriodDim] (
	[SchoolKey]
) ON [Primary]


CREATE UNIQUE CLUSTERED INDEX [UCX_StudentSectionDim]
ON [analytics].[stg_StudentSectionDim] (
	[StudentSectionKey]
) ON [Primary]


CREATE INDEX [IX_StudentSectionDim_StudentSectionKey] ON
	[analytics].[stg_StudentSectionDim]
(
	[StudentKey],
	[StudentSectionKey],
	[Subject],
	[SchoolKey]
) ON [Primary]

CREATE NONCLUSTERED INDEX [IX_StudentSectionDim_SchoolKey]
ON [analytics].[stg_StudentSectionDim] (
	[SchoolKey]
) ON [Primary]

CREATE NONCLUSTERED INDEX [IX_StudentSectionDim_SectionKey]
ON [analytics].[stg_StudentSectionDim] (
	[SectionKey]
) ON [Primary]

CREATE NONCLUSTERED INDEX [IX_StudentSectionAttendanceEventFact_StudentKey_SchoolYear]
ON [analytics].[stg_StudentSectionAttendanceEventFact] ([StudentKey], [SchoolYear])
INCLUDE ([EventDate],[SectionIdentifier],[SessionName],[LocalCourseCode],[TermDescriptor],[AttendanceEventCategoryDescriptor],[AttendanceEventReason])
 ON [Primary]

----------------------------
-- Drop real tables so they can be replaced by the staging ones
-- (Dropping and then renaming staging tables is much faster
-- than merging records into the real tables)
----------------------------
PRINT 'Dropping live tables...'

DECLARE [AnalyticsTables] CURSOR READ_ONLY FORWARD_ONLY FOR
	SELECT 
		[TABLE_SCHEMA],[TABLE_NAME] 
	FROM 
		[INFORMATION_SCHEMA].[TABLES] 
	WHERE 
		[TABLE_SCHEMA] in ('Analytics','analytics_config')
	AND [TABLE_TYPE] = 'Base Table' 
	AND [TABLE_NAME] NOT LIKE 'stg_%'

DECLARE @TableToDelete as NVARCHAR(128)
DECLARE @SchemaName as NVARCHAR(128)

OPEN [AnalyticsTables]

FETCH NEXT FROM [AnalyticsTables] INTO @SchemaName, @TableToDelete

WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE @sql NVARCHAR(200) = N'DROP TABLE ['+@SchemaName+N'].[' + @TableToDelete + N']';
	EXEC sp_executesql @sql;

	FETCH NEXT FROM [AnalyticsTables] INTO @SchemaName, @TableToDelete
END

CLOSE [AnalyticsTables]
DEALLOCATE [AnalyticsTables]
GO


----------------------------
-- Rename staging tables to be the real tables
----------------------------
PRINT 'Renaming staging to real tables...'

DECLARE [StagingTables] CURSOR READ_ONLY FORWARD_ONLY FOR
	SELECT 
		[TABLE_SCHEMA],[TABLE_NAME] 
	FROM 
		[INFORMATION_SCHEMA].[TABLES] 
	WHERE 
		[TABLE_SCHEMA] in ('Analytics','analytics_config') 
	AND [TABLE_TYPE] = 'Base Table' 
	AND [TABLE_NAME] LIKE 'stg_%'

DECLARE @StagingTable as NVARCHAR(128)
DECLARE @SchemaName as NVARCHAR(128)

OPEN [StagingTables]

FETCH NEXT FROM [StagingTables] INTO  @SchemaName,@StagingTable

WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE @source NVARCHAR(300) = '['+@SchemaName+'].' + @StagingTable
	DECLARE @dest NVARCHAR(128) = REPLACE(@StagingTable, 'stg_', '')
	
	EXEC sp_rename @source, @dest

	FETCH NEXT FROM [StagingTables] INTO @SchemaName,@StagingTable
END

CLOSE [StagingTables]
DEALLOCATE [StagingTables]

----------------------------
-- Fake out installation of the base Analytics Middle Tier views - which have
-- been replaced by matieralized tables - so that the EWS views can be 
-- installed using the middle tier installer
----------------------------
PRINT 'Creating fake schema version log...'
IF NOT EXISTS (SELECT 1 FROM [$(DataMartDB)].[INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_NAME] = 'AnalyticsMiddleTierSchemaVersion')
BEGIN

	SELECT * 
	INTO [$(DataMartDB)].[dbo].[AnalyticsMiddleTierSchemaVersion]
	FROM [$(OdsDb)].[dbo].[AnalyticsMiddleTierSchemaVersion]
	WHERE [ScriptName] LIKE 'EdFi.AnalyticsMiddleTier.Lib.Base.%'

	ALTER TABLE [$(DataMartDB)].[dbo].[AnalyticsMiddleTierSchemaVersion] ADD CONSTRAINT [PK_AnalyticsMiddleTierSchemaVersion_Id] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)

END
ELSE
BEGIN
	
	SET IDENTITY_INSERT [$(DataMartDB)].[dbo].[AnalyticsMiddleTierSchemaVersion] ON 

	MERGE INTO [$(DataMartDB)].[dbo].[AnalyticsMiddleTierSchemaVersion] as [Target]
	USING (
		SELECT Id, ScriptName, Applied 
		FROM [$(OdsDb)].[dbo].[AnalyticsMiddleTierSchemaVersion]
		WHERE [ScriptName] LIKE 'EdFi.AnalyticsMiddleTier.Lib.Base.%'
	) AS [Source] (Id, ScriptName, Applied)
	ON [Target].[Id] = [Source].[Id]
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (Id, ScriptName, Applied) 
	VALUES (Id, ScriptName, Applied);

	SET IDENTITY_INSERT [$(DataMartDB)].[dbo].[AnalyticsMiddleTierSchemaVersion] OFF

END



PRINT 'All operations complete. Run the Analytics Middle Tier installer to install EWS views.'
GO