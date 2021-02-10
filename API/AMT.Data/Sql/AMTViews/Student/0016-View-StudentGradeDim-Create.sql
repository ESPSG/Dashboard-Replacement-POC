IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'analytics' AND TABLE_NAME = 'StudentGradeDim')
BEGIN
	DROP VIEW analytics.StudentGradeDim
END
GO

CREATE VIEW [analytics].StudentGradeDim AS
	SELECT	stu.StudentUniqueId as StudentKey
			, CAST(g.SchoolId AS VARCHAR) as SchoolKey
			, sec.SectionIdentifier
			, sec.SessionName
			, g.LocalCourseCode
			, TermDescriptor.CodeValue AS TermType
			, g.SchoolYear
			, GradingPeriodDescriptor.CodeValue AS GradingPeriodDescription
			, gp.BeginDate AS GradingPeriodBeginDate
			, g.GradingPeriodSequence
			, g.LetterGradeEarned
			, g.NumericGradeEarned
			, GradeTypeDescriptor.CodeValue AS GradeType
	FROM edfi.Student stu
    INNER JOIN edfi.Grade g ON g.StudentUSI = stu.StudentUSI
    INNER JOIN edfi.GradingPeriod gp
        ON gp.GradingPeriodDescriptorId = g.GradingPeriodDescriptorId
        AND gp.SchoolId = g.SchoolId
        AND gp.PeriodSequence = g.GradingPeriodSequence
        AND gp.SchoolYear = g.SchoolYear
    INNER JOIN edfi.[Session] s
        ON s.SchoolId = g.SchoolId
        AND s.SchoolYear = g.SchoolYear
        AND s.SessionName = g.SessionName
	INNER JOIN analytics.StudentSchoolDim ON StudentSchoolDim.StudentUSI = g.StudentUSI AND StudentSchoolDim.SchoolKey = g.SchoolId
    INNER JOIN edfi.School sch ON sch.SchoolId = StudentSchoolDim.SchoolKey
    INNER JOIN edfi.GradingPeriodDescriptor gpd ON g.GradingPeriodDescriptorId = gpd.GradingPeriodDescriptorId
	INNER JOIN edfi.Descriptor GradingPeriodDescriptor ON GradingPeriodDescriptor.DescriptorId = gpd.GradingPeriodDescriptorId
    INNER JOIN edfi.Section sec
        ON sec.SchoolId = g.SchoolId
        AND sec.LocalCourseCode = g.LocalCourseCode
        AND sec.SchoolYear = g.SchoolYear
        AND sec.SessionName = g.SessionName
        AND sec.SectionIdentifier = g.SectionIdentifier
    INNER JOIN edfi.Descriptor TermDescriptor
        ON TermDescriptor.DescriptorId = s.TermDescriptorId
	INNER JOIN edfi.Descriptor GradeTypeDescriptor
    ON GradeTypeDescriptor.DescriptorId = g.GradeTypeDescriptorId
GO