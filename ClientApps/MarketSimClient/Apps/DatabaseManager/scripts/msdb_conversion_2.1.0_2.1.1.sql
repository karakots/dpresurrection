INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,1,1, 'Added database access time to scenario')
GO

ALTER TABLE Scenario
ADD [access_time] [int] NOT NULL DEFAULT 1
GO