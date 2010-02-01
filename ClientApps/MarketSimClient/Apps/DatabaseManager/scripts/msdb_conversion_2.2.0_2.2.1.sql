INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,1, 'adding save column to scenario')
GO

ALTER TABLE scenario
ADD [saved] bit NOT NULL DEFAULT(1)
GO