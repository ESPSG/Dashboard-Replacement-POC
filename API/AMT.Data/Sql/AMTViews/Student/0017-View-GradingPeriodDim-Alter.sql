ALTER VIEW [analytics].[GradingPeriodDim] AS

	SELECT
		CAST(GradingPeriod.GradingPeriodDescriptorId as NVARCHAR)
			+ '-' + CAST(GradingPeriod.SchoolId as NVARCHAR)
			+ '-' + CONVERT(NVARCHAR, GradingPeriod.BeginDate, 112) as GradingPeriodKey,
		CONVERT(NVARCHAR, GradingPeriod.BeginDate, 112) as GradingPeriodBeginDateKey,
		CONVERT(NVARCHAR, GradingPeriod.EndDate, 112) as GradingPeriodEndDateKey,
		GradingPeriodDescriptor.CodeValue as GradingPeriodDescription,
		GradingPeriod.TotalInstructionalDays,
		GradingPeriod.PeriodSequence,
		CAST(GradingPeriod.SchoolId AS VARCHAR) as SchoolKey,
		CAST(GradingPeriod.SchoolYear AS VARCHAR) as SchoolYear,
		GradingPeriod.LastModifiedDate,
		GradingPeriod.BeginDate,
		GradingPeriod.EndDate
	FROM
		edfi.GradingPeriod
	INNER JOIN
		edfi.Descriptor as GradingPeriodDescriptor ON
			GradingPeriod.GradingPeriodDescriptorId = GradingPeriodDescriptor.DescriptorId
	WHERE GradingPeriod.BeginDate < GETDATE()
GO