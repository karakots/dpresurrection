INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,7, 'Breaking up summary views')
GO

ALTER TABLE simulation
Add [output_mode] [int] NOT NULL default(0)
GO

CREATE TABLE [results_mape_by_product_channel](
	[run_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[mape] [float] NOT NULL,
	CONSTRAINT [ResMAPECalProdChan] FOREIGN KEY([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) ON [PRIMARY]
GO

ALTER TABLE results_calibration_by_product_channel
DROP COLUMN mape
GO

DROP VIEW res_compare_by_product_channel
GO

Create view res_compare_by_product_channel
as
SELECT		
	summary_by_product_channel.run_id, 
	summary_by_product_channel.product_id, 
	summary_by_product_channel.channel_id, 
	summary_by_product_channel.unit_share AS sim_share, 
	external_data_share_by_product_channel.unit_share AS real_share, 
	summary_by_product_channel.unit_share - external_data_share_by_product_channel.unit_share AS share_diff, 
	CASE WHEN external_data_share_by_product_channel.unit_share > 0 
		 THEN (summary_by_product_channel.unit_share - external_data_share_by_product_channel.unit_share)
               / external_data_share_by_product_channel.unit_share 
		  ELSE 0 
	END AS percent_share_error
FROM         
	summary_by_product_channel INNER JOIN
	external_data_summary_by_product_channel ON summary_by_product_channel.run_id = external_data_summary_by_product_channel.run_id AND 
	summary_by_product_channel.product_id = external_data_summary_by_product_channel.product_id AND 
	summary_by_product_channel.channel_id = external_data_summary_by_product_channel.channel_id INNER JOIN
	external_data_share_by_product_channel ON summary_by_product_channel.run_id = external_data_share_by_product_channel.run_id AND 
	summary_by_product_channel.product_id = external_data_share_by_product_channel.product_id AND 
	summary_by_product_channel.channel_id = external_data_share_by_product_channel.channel_id
GROUP BY summary_by_product_channel.run_id, summary_by_product_channel.product_id, summary_by_product_channel.channel_id, 
                      summary_by_product_channel.unit_share, external_data_summary_by_product_channel.num_units, 
                      external_data_share_by_product_channel.unit_share
GO


Create View mape_by_product_channel
as
SELECT     diff_by_product_channel.run_id, 
diff_by_product_channel.product_id, 
diff_by_product_channel.channel_id, 
           CASE		WHEN external_data_summary_by_product_channel.num_units > 0 
					THEN 100.0 * SUM(ABS(diff_by_product_channel.diff)) 
                      / external_data_summary_by_product_channel.num_units ELSE 0 END AS mape
FROM         
diff_by_product_channel INNER JOIN
                      external_data_summary_by_product_channel ON diff_by_product_channel.run_id = external_data_summary_by_product_channel.run_id AND 
                      diff_by_product_channel.product_id = external_data_summary_by_product_channel.product_id AND 
                      diff_by_product_channel.channel_id = external_data_summary_by_product_channel.channel_id 
where diff_by_product_channel.run_id = 14
GROUP BY diff_by_product_channel.run_id, diff_by_product_channel.product_id, diff_by_product_channel.channel_id,
external_data_summary_by_product_channel.num_units
GO