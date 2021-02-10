IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'analytics_config' AND TABLE_NAME = 'GradingScale')
BEGIN

	CREATE TABLE [analytics_config].[GradingScale](
		[GradingScaleId] [int] IDENTITY(1,1) NOT NULL,
		[LocalEducationAgencyId] [int] NOT NULL,
		[GradingScaleName] [nvarchar](50) NOT NULL,
		CONSTRAINT [PK_GradingScale] PRIMARY KEY CLUSTERED ([GradingScaleId] ASC) ON [PRIMARY],
		CONSTRAINT [AK_GradingScaleName] UNIQUE NONCLUSTERED ([LocalEducationAgencyId] ASC,	[GradingScaleName] ASC) ON [PRIMARY]
	) ON [PRIMARY];

	SET IDENTITY_INSERT [analytics_config].[GradingScale] ON 
	
	INSERT [analytics_config].[GradingScale] ([GradingScaleId], [LocalEducationAgencyId], [GradingScaleName]) 
	VALUES (3, 867530, N'8th Grade Numeric Grading Scale')
	
	INSERT [analytics_config].[GradingScale] ([GradingScaleId], [LocalEducationAgencyId], [GradingScaleName]) 
	VALUES (2, 867530, N'Letter Grading Scale')
	
	INSERT [analytics_config].[GradingScale] ([GradingScaleId], [LocalEducationAgencyId], [GradingScaleName]) 
	VALUES (1, 867530, N'Numeric Grading Scale')
	
	SET IDENTITY_INSERT [analytics_config].[GradingScale] OFF

END;