IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'analytics_config' AND TABLE_NAME = 'GradingScaleGrade')
BEGIN

	CREATE TABLE [analytics_config].[GradingScaleGrade](
			[GradingScaleGradeId] [int] IDENTITY(1,1) NOT NULL,
			[GradingScaleId] [int] NOT NULL,
			[Rank] [int] NOT NULL,
			[LetterGrade] [nvarchar](20) NULL,
			[UpperNumericGrade] [decimal](6, 2) NULL,
			CONSTRAINT [PK_GradingScaleGrade] PRIMARY KEY CLUSTERED ([GradingScaleGradeId] ASC) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [analytics_config].[GradingScaleGrade] ADD  CONSTRAINT [UX_GradingScaleGrade_Rank] UNIQUE NONCLUSTERED 
	(
		[GradingScaleId] ASC,
		[Rank] ASC
	) ON [PRIMARY]

	ALTER TABLE [analytics_config].[GradingScaleGrade]  WITH CHECK ADD  CONSTRAINT [FK_GradingScaleGrade_GradingScale] FOREIGN KEY([GradingScaleId])
	REFERENCES [analytics_config].[GradingScale] ([GradingScaleId])
	
	ALTER TABLE [analytics_config].[GradingScaleGrade] CHECK CONSTRAINT [FK_GradingScaleGrade_GradingScale]
	
	ALTER TABLE [analytics_config].[GradingScaleGrade]  WITH CHECK ADD  CONSTRAINT [CK_GradingScaleGrade_LetterOrNumber] CHECK  (([LetterGrade] IS NOT NULL OR [UpperNumericGrade] IS NOT NULL))
	
	ALTER TABLE [analytics_config].[GradingScaleGrade] CHECK CONSTRAINT [CK_GradingScaleGrade_LetterOrNumber]


	SET IDENTITY_INSERT [analytics_config].[GradingScaleGrade] ON 
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (1, 1, 5, NULL, CAST(100.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (2, 1, 4, NULL, CAST(89.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (3, 1, 3, NULL, CAST(79.00 AS Decimal(6, 2)))

	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (4, 1, 2, NULL, CAST(69.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (5, 1, 1, NULL, CAST(59.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (6, 2, 5, N'A', NULL)
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (7, 2, 4, N'B', NULL)

	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (8, 2, 3, N'C', NULL)
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (9, 2, 2, N'D', NULL)
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (10, 2, 1, N'F', NULL)
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (11, 3, 5, NULL, CAST(100.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (12, 3, 4, NULL, CAST(89.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (13, 3, 3, NULL, CAST(79.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (14, 3, 2, NULL, CAST(69.00 AS Decimal(6, 2)))
	
	INSERT [analytics_config].[GradingScaleGrade] ([GradingScaleGradeId], [GradingScaleId], [Rank], [LetterGrade], [UpperNumericGrade]) 
	VALUES (15, 3, 1, NULL, CAST(59.00 AS Decimal(6, 2)))
	
	SET IDENTITY_INSERT [analytics_config].[GradingScaleGrade] OFF
END;