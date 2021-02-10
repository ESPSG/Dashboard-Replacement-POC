	
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'SchoolCalendarDim')
BEGIN
	DROP VIEW analytics.SchoolCalendarDim
END
GO

CREATE VIEW analytics.SchoolCalendarDim AS
	WITH Calendar AS
	(
		SELECT CalendarCode,
		SchoolId,
		SchoolYear
		FROM
		(
			SELECT CalendarCode,
			SchoolId,
			sy.SchoolYear,
			ROW_NUMBER() OVER (PARTITION BY c.SchoolId, c.SchoolYear ORDER BY c.CreateDate DESC) as [Priority]
			FROM edfi.Calendar c 
			INNER JOIN [analytics].[CurrentSchoolYearDim] sy
				ON c.SchoolYear = sy.SchoolYear
			WHERE c.CalendarTypeDescriptorId = (SELECT TOP 1 CalendarTypeDescriptorId
												FROM edfi.CalendarTypeDescriptor ctd 
												INNER JOIN edfi.Descriptor d 
													ON ctd.CalendarTypeDescriptorId = d.DescriptorId 
												WHERE [Namespace] = 'uri://ed-fi.org/CalendarTypeDescriptor' 
													AND CodeValue IN ('School') 
												ORDER BY CalendarTypeDescriptorId ASC)
		) c
		WHERE c.[Priority] = 1
	)
	SELECT	caldate.SchoolId AS SchoolKey,
			caldate.[Date],
			ced.CalendarEventDescriptorId as CalendarEventDescriptor,
			(ROW_NUMBER() OVER (PARTITION BY caldate.SchoolId ORDER BY caldate.[Date] DESC) - 1) as DaysBack
	FROM Calendar c
	INNER JOIN edfi.CalendarDate caldate 
		ON caldate.CalendarCode = c.CalendarCode
		AND caldate.SchoolId = c.SchoolId
		AND caldate.SchoolYear = c.SchoolYear
	INNER JOIN [analytics].[SchoolMinMaxDateDim] mmd 
		ON mmd.SchoolKey = caldate.SchoolId
	LEFT JOIN edfi.School s
		ON caldate.SchoolId = s.SchoolId
	LEFT JOIN edfi.CalendarDateCalendarEvent  cdce
		ON caldate.SchoolId = cdce.SchoolId
		AND caldate.Date = cdce.Date
	LEFT JOIN edfi.CalendarEventDescriptor ced
		ON cdce.CalendarEventDescriptorId = ced.CalendarEventDescriptorId
	WHERE
		ced.CalendarEventDescriptorId IN (	SELECT CalendarEventDescriptorId 
											FROM edfi.CalendarEventDescriptor ced 
											INNER JOIN edfi.Descriptor d 
												ON ced.CalendarEventDescriptorId = d.DescriptorId 
											WHERE CodeValue IN('Instructional day','Make-up day'))
		AND caldate.[Date] <= mmd.MaxDate
		-- The MaxDate is determined by the latest date in the attendance event table; that date may be a date in the future
		AND caldate.[Date] <= GETDATE()
GO