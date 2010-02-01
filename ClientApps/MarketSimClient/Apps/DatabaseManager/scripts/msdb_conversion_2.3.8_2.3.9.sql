INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,9, 'Added simulation_status and results_status views')
GO


UPDATE [scenario_type]
SET type = 'Standard'
WHERE scenario_type_id = 0 
GO

CREATE TABLE [simulation_state_type] (
    [id] [int] PRIMARY KEY, 
    [name] varchar(50) NOT NULL
)
GO

INSERT INTO [simulation_state_type]
VALUES (-1, 'Ready')
GO

INSERT INTO [simulation_state_type]
VALUES (0, 'Running')
GO

INSERT INTO [simulation_state_type]
VALUES (1, 'Queued')
GO

CREATE VIEW [simulation_status] AS
SELECT simulation.[name], scenario_type.type, 
simulation_state_type.[name] as status,
simulation.start_date, simulation.end_date, simulation.reset_panel_state,
simulation.scenario_id, simulation.id as simulation_id
FROM simulation, scenario_type, simulation_state_type 
WHERE simulation_state_type.id = simulation.sim_num AND 
scenario_type.scenario_type_id = simulation.[type]
GO

CREATE VIEW [results_status] AS
SELECT simulation.[name], sim_queue.[name] as run_name, 
sim_status.status, sim_queue.run_time, 
sim_queue.current_status, sim_queue.[current_date],
simulation.scenario_id, simulation.id as simulation_id, 
sim_queue.run_id
FROM sim_queue, simulation, sim_status
WHERE sim_status.status_id = sim_queue.status AND simulation.id = sim_queue.sim_id
GO
