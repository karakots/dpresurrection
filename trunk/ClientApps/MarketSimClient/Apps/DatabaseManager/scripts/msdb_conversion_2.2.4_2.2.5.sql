INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,5, 'adding check pointing')
GO

ALTER TABLE Model_info
ADD [checkpoint_file] [varchar] (100) NOT NULL DEFAULT 'NA'
GO

ALTER TABLE Model_info
ADD [checkpoint_date] [datetime] NOT NULL DEFAULT getdate()
GO

ALTER TABLE Model_info
ADD [checkpoint_valid] [bit] NOT NULL DEFAULT 0
GO

INSERT scenario_type VALUES (150, 'Checkpoint')
GO

ALTER TABLE scenario
ALTER COLUMN
[name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO