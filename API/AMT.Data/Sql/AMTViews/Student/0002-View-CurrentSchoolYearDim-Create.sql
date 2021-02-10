IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'CurrentSchoolYearDim')
BEGIN
	DROP VIEW analytics.CurrentSchoolYearDim
END
GO

CREATE VIEW [analytics].CurrentSchoolYearDim AS

	SELECT MIN(SchoolYear) AS SchoolYear
	FROM edfi.Session
GO