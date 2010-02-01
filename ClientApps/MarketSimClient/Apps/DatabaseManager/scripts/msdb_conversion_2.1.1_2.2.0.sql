INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,0, 'adding user names')
GO ALTER TABLE scenario
ADD [user_name] varchar(100) NOT NULL DEFAULT('all')
GO

ALTER TABLE market_plan
ADD [user_name] varchar(100) NOT NULL DEFAULT('all')
GO