IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'analytics_config' AND TABLE_NAME = 'GradingScaleGradeLevel')
BEGIN

	CREATE TABLE [analytics_config].[GradingScaleGradeLevel](
		[GradingScaleId] [int] NOT NULL,
		[GradeLevelTypeId] [int] NOT NULL,
		[GradingScaleGradeLevelId] [int] IDENTITY(1,1) NOT NULL,
		CONSTRAINT [PK_GradingScaleGradeLevel] PRIMARY KEY CLUSTERED ([GradingScaleGradeLevelId] ASC) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [analytics_config].[GradingScaleGradeLevel]  WITH CHECK ADD  CONSTRAINT [FK_GradingScaleGradeLevel_GradingScale] FOREIGN KEY([GradingScaleId])
	REFERENCES [analytics_config].[GradingScale] ([GradingScaleId])

	ALTER TABLE [analytics_config].[GradingScaleGradeLevel] CHECK CONSTRAINT [FK_GradingScaleGradeLevel_GradingScale]

	SET IDENTITY_INSERT [analytics_config].[GradingScaleGradeLevel] ON 

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (1, 19, 1)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (1, 24, 4)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (1, 34, 2)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (1, 35, 3)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (1, 38, 5)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 25, 9)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 26, 6)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 31, 11)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 32, 7)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 36, 8)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (2, 37, 10)

	INSERT [analytics_config].[GradingScaleGradeLevel] ([GradingScaleId], [GradeLevelTypeId], [GradingScaleGradeLevelId]) 
	VALUES (3, 21, 12)

	SET IDENTITY_INSERT [analytics_config].[GradingScaleGradeLevel] OFF
END;