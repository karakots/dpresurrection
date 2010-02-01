INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,8, 'Adding checkpoint_scale_factor to Model_Info')
GO

ALTER TABLE Model_Info 
ADD [checkpoint_scale_factor] [float] NOT NULL DEFAULT(0)
GO
