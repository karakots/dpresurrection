
INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,5, 'Added new metric - ensure summaries sum over leafs')
GO

CREATE TABLE [dbo].[results_summary_by_product_segment](
	[run_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[segment_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[unit_share] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	[dollar_share] [float] NOT NULL,
	[awareness] [float] NOT NULL,
	[persuasion] [float] NOT NULL,
	CONSTRAINT [ResSumProdSeg] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[results_summary_by_channel_segment](
	[run_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[segment_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[unit_share] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	[dollar_share] [float] NOT NULL,
	CONSTRAINT [ResSumChanSeg] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) ON [PRIMARY]
GO

DROP VIEW [summary_by_segment]
GO

Create view [summary_by_segment] AS
Select   results_std.run_id,   results_std.segment_id,  
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_total.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units   
ELSE 0.0  END   as unit_share,  
SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_total.num_dollars > 0    
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars   
ELSE 0.0  END  as dollar_share,  
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,  
AVG(persuasion_sku)/simulation.access_time as persuasion  
from results_std, sim_queue, simulation, summary_total, product  
where sim_queue.run_id = results_std.run_id  and simulation.id = sim_queue.sim_id  and summary_total.run_id = results_std.run_id  
and results_std.calendar_date > simulation.metric_start_date - 1  and results_std.calendar_date < simulation.metric_end_date + 1
and product.product_id = results_std.product_id and product.brand_id = 1
group by   results_std.run_id,   results_std.segment_id,  summary_total.num_units,  summary_total.num_dollars,  simulation.access_time  
GO

DROP VIEW summary_by_channel_segment
GO

CREATE VIEW summary_by_channel_segment AS
Select   results_std.run_id,   results_std.channel_id,   results_std.segment_id,   
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_by_segment.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_by_segment.num_units    
ELSE 0  END   as unit_share,  
SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_by_segment.num_dollars > 0
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_segment.num_dollars   
ELSE 0  END  as dollar_share  
from results_std, summary_by_segment, sim_queue, simulation, product  
where sim_queue.run_id = results_std.run_id  and simulation.id = sim_queue.sim_id  
and summary_by_segment.run_id = results_std.run_id  and summary_by_segment.segment_id = results_std.segment_id  
and results_std.calendar_date > simulation.metric_start_date - 1  and results_std.calendar_date < simulation.metric_end_date + 1
and product.product_id = results_std.product_id and product.brand_id = 1
group by   results_std.run_id,  results_std.channel_id,  results_std.segment_id,   summary_by_segment.num_units,   summary_by_segment.num_dollars  
GO