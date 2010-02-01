INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,12, 'added product, segement, channel to sim_metric value')
GO

DROP TABLE sim_metric_value
GO

CREATE TABLE [sim_metric_value] (
	[run_id] [int] NOT NULL,
	[product_id] [int],
	[segment_id] [int],
	[channel_id] [int],
	[token] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimMetricValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

