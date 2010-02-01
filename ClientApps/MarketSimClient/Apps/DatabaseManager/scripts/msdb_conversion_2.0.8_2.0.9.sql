INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,9, 'Adding Variables')
GO

alter table sim_queue 
add [run_time] [datetime]
GO

alter table sim_queue 
add [seed] [int] NOT NULL Default(1)
GO

alter table scenario
add [sim_num] [int] NOT NULL Default(-1)
GO

alter table external_data_type
add [sim_type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

alter table external_data_type
add [average] [bit] NOT NULL default(0)
GO

update scenario set queued = 1 where scenario_id in (Select scenario_id from sim_queue)
GO

update scenario set sim_num = 0 where queued = 1
GO

update external_data_type set sim_type = 'num_sku_bought' where id = 1
GO

update external_data_type set sim_type = 'percent_aware_sku_cum' where id = 2
GO

update external_data_type set average = 1 where id = 2
GO

alter table scenario_parameter
add [expression] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

CREATE TABLE [scenario_variable] (
	[scenario_id] [int] NOT NULL,
	[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[token] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[descr] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[min] [float] NOT NULL,
	[max] [float] NOT NULL,
	[num_steps] [int] NOT NULL Default(0),
	CONSTRAINT ScenarioVariable FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO

CREATE TABLE [scenario_simseed] (
	[scenario_id] [int] NOT NULL,
	[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[seed] [int] NOT NULL Default(1),
	CONSTRAINT ScenarioSimSeed FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO

CREATE TABLE [sim_variable_value] (
	[run_id] [int] NOT NULL,
	[var_id] [int] NOT NULL,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimVariableValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [sim_metric_value] (
	[run_id] [int] NOT NULL,
	[token] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS PRIMARY KEY CLUSTERED,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimMetricValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


