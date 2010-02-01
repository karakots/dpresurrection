INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,14, 'scenario: metric dates, calibration, control string')
GO

ALTER TABLE scenario
ADD [metric_start_date] [datetime] NOT NULL DEFAULT getdate()
GO

ALTER TABLE scenario
ADD [metric_end_date] [datetime] NOT NULL DEFAULT getdate()
GO

ALTER TABLE scenario
ADD [delete_std_results] [bit] NOT NULL DEFAULT(0)
GO

Update scenario set metric_start_date = start_date
GO

Update scenario set metric_end_date = end_date
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (100, 'Calibration')
GO

ALTER TABLE scenario
ADD [control_string] varchar(100) NOT NULL DEFAULT('')
GO