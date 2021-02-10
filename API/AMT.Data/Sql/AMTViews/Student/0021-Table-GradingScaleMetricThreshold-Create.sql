IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'analytics_config' AND TABLE_NAME = 'GradingScaleMetricThreshold')
BEGIN

	CREATE TABLE [analytics_config].[GradingScaleMetricThreshold](
			[GradingScaleId] [int] NOT NULL,
			[MetricId] [int] NOT NULL,
			[Value] [int] NOT NULL,
			[GradingScaleMetricThresholdId] [int] IDENTITY(1,1) NOT NULL,
			CONSTRAINT [PK_GradingScaleMetricThreshold] PRIMARY KEY CLUSTERED ([GradingScaleMetricThresholdId] ASC) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [analytics_config].[GradingScaleMetricThreshold]  WITH CHECK ADD  CONSTRAINT [FK_GradingScaleMetricThreshold_GradingScale] FOREIGN KEY([GradingScaleId])
	REFERENCES [analytics_config].[GradingScale] ([GradingScaleId])

	ALTER TABLE [analytics_config].[GradingScaleMetricThreshold] CHECK CONSTRAINT [FK_GradingScaleMetricThreshold_GradingScale]

	SET IDENTITY_INSERT [analytics_config].[GradingScaleMetricThreshold] ON 
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (1, 24, 2, 1)
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (1, 25, 1, 2)
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (1, 26, 3, 3)
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (2, 24, 2, 4)
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (2, 25, 1, 5)
	
	INSERT [analytics_config].[GradingScaleMetricThreshold] ([GradingScaleId], [MetricId], [Value], [GradingScaleMetricThresholdId]) 
	VALUES (2, 26, 3, 6)
	
	SET IDENTITY_INSERT [analytics_config].[GradingScaleMetricThreshold] OFF
END;