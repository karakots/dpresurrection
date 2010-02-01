INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,10, 'Create Index on Results')
GO

CREATE INDEX results_sort_index ON results_std (run_id, calendar_date)
GO



