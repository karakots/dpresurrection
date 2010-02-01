INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,1, 'Simulation v Scenario')
GO

CREATE TABLE [simulation] (
	[id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[scenario_id] [int] NOT NULL,
	[name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[descr] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[type] [tinyint] NOT NULL DEFAULT ((0)),
	[locked] [bit] NOT NULL DEFAULT ((0)),
	[queued] [bit] NOT NULL DEFAULT ((0)),
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[sim_num] [int] NOT NULL DEFAULT ((-1)),
	[metric_start_date] [datetime] NOT NULL DEFAULT (getdate()),
	[metric_end_date] [datetime] NOT NULL DEFAULT (getdate()),
	[delete_std_results] [bit] NOT NULL DEFAULT ((0)),
	[control_string] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[access_time] [int] NOT NULL DEFAULT ((1)),
	[scale_factor] [float] NOT NULL DEFAULT ((100)),
	CONSTRAINT [ScenarioSimulation] FOREIGN KEY ([scenario_id])
	REFERENCES scenario ([scenario_id])
	ON DELETE CASCADE
) ON [PRIMARY]
GO

INSERT simulation 
SELECT
	[scenario_id],
	[name],
	[descr],
	[type],
	[locked],
	[queued],
	[start_date],
	[end_date],
	[sim_num],
	[metric_start_date],
	[metric_end_date],
	[delete_std_results],
	[control_string],
	[access_time],
	[scale_factor]
from scenario
GO

alter table sim_queue
add sim_id [int] NOT NULL default(-1)
GO

update sim_queue
set sim_id = (select id from simulation 
where simulation.scenario_id = sim_queue.scenario_id)
GO

alter table sim_queue
drop constraint [ScenariorRun]
GO

alter table sim_queue
drop column [scenario_id]
GO

alter table sim_queue
add CONSTRAINT [SimulationSimQueue] FOREIGN KEY (sim_id)
	REFERENCES simulation (id)
	ON DELETE CASCADE
GO

alter table scenario_metric
add sim_id [int] NOT NULL default(-1)
GO

update scenario_metric
set sim_id = (select id from simulation 
where simulation.scenario_id = scenario_metric.scenario_id)
GO

alter table scenario_metric
drop constraint [ScenarioMetric]
GO

alter table scenario_metric
drop column scenario_id
GO

alter table scenario_metric
add CONSTRAINT [SimulationMetric] FOREIGN KEY (sim_id)
	REFERENCES simulation (id)
	ON DELETE CASCADE
GO

alter table [scenario_parameter]
add sim_id [int] NOT NULL default(-1)
GO

update [scenario_parameter]
set sim_id = (select id from simulation 
where simulation.scenario_id = scenario_parameter.scenario_id)
GO

alter table [scenario_parameter]
drop constraint [ModelScenarioParam]
GO

alter table [scenario_parameter]
drop column scenario_id
GO

alter table [scenario_parameter]
add CONSTRAINT [SimulationParameter] FOREIGN KEY (sim_id)
	REFERENCES simulation (id)
	ON DELETE CASCADE
GO

alter table [scenario_simseed]
add sim_id [int] NOT NULL default(-1)
GO

update [scenario_simseed]
set sim_id = (select id from simulation 
where simulation.scenario_id = [scenario_simseed].scenario_id)
GO

alter table [scenario_simseed]
drop constraint [ScenarioSimSeed]
GO

alter table [scenario_simseed]
drop column scenario_id
GO

alter table [scenario_simseed]
add CONSTRAINT [SimulationSeed] FOREIGN KEY (sim_id)
	REFERENCES simulation (id)
	ON DELETE CASCADE
GO

alter table [scenario_variable]
add sim_id [int] NOT NULL default(-1)
GO

update [scenario_variable]
set sim_id = (select id from simulation 
where simulation.scenario_id = [scenario_variable].scenario_id)
GO

alter table [scenario_variable]
drop constraint [ScenarioVariable]
GO

alter table [scenario_variable]
drop column scenario_id
GO

alter table [scenario_variable]
add CONSTRAINT [SimulationVariable] FOREIGN KEY (sim_id)
	REFERENCES simulation (id)
	ON DELETE CASCADE
GO

ALTER TABLE [scenario]  
drop CONSTRAINT [ScenarioType]
GO



