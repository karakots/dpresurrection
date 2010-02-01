INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,3, 'attribute sensitivity conversion and view fix')
GO

IF NOT EXISTS (SELECT * FROM db_schema_info
WHERE major_no = 2 AND minor_no = 2 AND release_no = 8)
BEGIN
INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,8, 'attribute price sensitivity')

ALTER TABLE consumer_preference
ADD [price_sensitivity] [float] NOT NULL DEFAULT(0)
END
GO

DROP VIEW summary_by_product_segment
GO

CREATE VIEW summary_by_product_segment AS
Select   results_std.run_id,   results_std.product_id,   results_std.segment_id,   
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_by_segment.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_by_segment.num_units    
ELSE 0  END   as unit_share, 
 SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_by_segment.num_dollars > 0    
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_segment.num_dollars   
ELSE 0  END  as dollar_share,  
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,  
AVG(persuasion_sku)/simulation.access_time as persuasion  
from results_std, sim_queue, simulation, summary_by_segment  
where sim_queue.run_id = results_std.run_id  
and  simulation.id = sim_queue.sim_id  
and summary_by_segment.run_id = results_std.run_id 
and summary_by_segment.segment_id = results_std.segment_id 
and results_std.calendar_date > simulation.metric_start_date - 1  
and results_std.calendar_date < simulation.metric_end_date + 1  
group by   results_std.run_id,    results_std.product_id,   results_std.segment_id,   
simulation.access_time,  summary_by_segment.num_units,   summary_by_segment.num_dollars
GO
