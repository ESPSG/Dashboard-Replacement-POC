
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentSchoolAttendanceEventFact')
BEGIN
	DROP VIEW analytics.StudentSchoolAttendanceEventFact
END
GO

CREATE VIEW analytics.StudentSchoolAttendanceEventFact AS
		SELECT	stu.StudentUniqueId as StudentKey,
				ssae.EventDate,
				ttd.TermDescriptorId as TermDescriptor,
				ssae.SchoolYear,
				D.CodeValue as AttendanceEventCategoryDescriptor,
				ssae.AttendanceEventReason
		FROM edfi.Student stu
		INNER JOIN edfi.StudentSchoolAttendanceEvent ssae 
			ON ssae.StudentUSI = stu.StudentUSI
		INNER JOIN edfi.[Session] sess
			ON sess.SchoolId = ssae.SchoolId
			AND sess.SchoolYear = ssae.SchoolYear
			AND sess.SessionName = ssae.SessionName
		INNER JOIN edfi.TermDescriptor ttd 
			ON ttd.TermDescriptorId = sess.TermDescriptorId
		INNER JOIN edfi.AttendanceEventCategoryDescriptor aecd 
			ON aecd.AttendanceEventCategoryDescriptorId = ssae.AttendanceEventCategoryDescriptorId
		INNER JOIN edfi.Descriptor D
			ON D.DescriptorId = SSAE.AttendanceEventCategoryDescriptorId
		INNER JOIN [analytics].[StudentSchoolDim] ssa
				ON ssa.SchoolKey = ssae.SchoolId
				AND ssa.StudentKey = stu.StudentUniqueId
GO