IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'analytics_config' AND TABLE_NAME = 'GradeLevelType')
BEGIN

	CREATE TABLE [analytics_config].[GradeLevelType](
			[GradeLevelTypeId] [int] NOT NULL,
			[CodeValue] [nvarchar](50) NULL,
			[Description] [nvarchar](1024) NULL,
		CONSTRAINT [PK_GradeLevelType] PRIMARY KEY CLUSTERED ([GradeLevelTypeId] ASC)	ON [PRIMARY]
	) ON [PRIMARY]
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (19, N'Fifth grade', N'Fifth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (20, N'Adult Education', N'Adult Education')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (21, N'Eighth grade', N'Eighth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (22, N'Early Education', N'Early Education')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (23, N'Grade 13', N'Grade 13')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (24, N'Eleventh grade', N'Eleventh grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (25, N'Fourth grade', N'Fourth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (26, N'First grade', N'First grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (27, N'Infant/toddler', N'Infant/toddler')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (28, N'Postsecondary', N'Postsecondary')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description])
	VALUES (29, N'Other', N'Other')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (30, N'Kindergarten', N'Kindergarten')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (31, N'Ninth grade', N'Ninth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (32, N'Second grade', N'Second grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (33, N'Preschool/Prekindergarten', N'Preschool/Prekindergarten')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (34, N'Seventh grade', N'Seventh grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (35, N'Tenth grade', N'Tenth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (36, N'Third grade', N'Third grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (37, N'Sixth grade', N'Sixth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (38, N'Twelfth grade', N'Twelfth grade')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (39, N'Ungraded', N'Ungraded')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (2216, N'No grade level', N'No grade level')
	
	INSERT [analytics_config].[GradeLevelType] ([GradeLevelTypeId], [CodeValue], [Description]) 
	VALUES (2234, N'No grade level', N'No grade level')
END;