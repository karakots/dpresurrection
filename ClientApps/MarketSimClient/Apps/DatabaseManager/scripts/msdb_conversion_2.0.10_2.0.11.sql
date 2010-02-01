INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,11, 'variable types added')
GO


ALTER TABLE scenario_type
alter Column type varchar(25)
GO

delete from scenario_type where scenario_type_id = 1
GO

delete from scenario_type where scenario_type_id = 2
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (32, 'Parallel Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (64, 'Serial Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (80, 'Random Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (96, 'Optimization')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (128, 'Statistical')
GO

update scenario_type set type = 'standard' where scenario_type_id = 0
GO


DROP TABLE sim_metric_value
GO

CREATE TABLE [sim_metric_value] (
	[run_id] [int] NOT NULL,
	[token] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimMetricValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [scenario_metric] (
	[scenario_id] [int] NOT NULL,
	[token] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT ScenarioMetric FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [variable_type] (
	[type_id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO

INSERT variable_type (type_id, type)
VALUES (0, 'Stepped')
GO

INSERT variable_type (type_id, type)
VALUES (1, 'Random (Uniform)')
GO

INSERT variable_type (type_id, type)
VALUES (2, 'Random (Centered)')
GO

ALTER TABLE [scenario_variable]
ADD [type] [tinyint] NOT NULL default(0)
GO

ALTER TABLE [scenario_variable]
ADD CONSTRAINT VariableType FOREIGN KEY (type)
REFERENCES [variable_type] (type_id)
ON DELETE CASCADE
GO

CREATE TABLE [product_event_type] (
	[type_id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO

INSERT product_event_type (type_id, type)
VALUES (0, 'Units Purchased')
GO

INSERT product_event_type (type_id, type)
VALUES (1, 'Shopping trips')
GO

INSERT product_event_type (type_id, type)
VALUES (2, 'Price Sensitivity')
GO


ALTER Table product_event
ADD [type] [tinyint] NOT NULL default(0)
GO

ALTER TABLE [product_event]
ADD CONSTRAINT ProductEventType FOREIGN KEY (type)
REFERENCES [product_event_type] (type_id)
ON DELETE CASCADE
GO
