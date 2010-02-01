INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,15, 'added clustered index, param_id to sim_queue')
GO

ALTER TABLE sim_queue
ADD [param_id] [int] NOT NULL DEFAULT -1
GO

ALTER TABLE scenario_variable
ADD [product_id] [int] NOT NULL DEFAULT -1
GO

drop index results_std.results_sort_index
GO

CREATE  CLUSTERED  INDEX [results_sort_index] ON [results_std]([run_id], [calendar_date], [product_id], [channel_id], [segment_id]) ON [PRIMARY]
GO