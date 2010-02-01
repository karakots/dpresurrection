INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,6, 'results mods and simulation flag and product price and eq untis')
GO

ALTER TABLE results_std
DROP COLUMN num_adds_brand
GO

ALTER TABLE results_std
DROP COLUMN num_drop_brand
GO

ALTER TABLE results_std
DROP COLUMN percent_aware_brand_cum
GO

ALTER TABLE results_std
DROP COLUMN num_brand_triers_cum
GO

ALTER TABLE results_std
DROP COLUMN num_brand_repeaters_cum
GO

ALTER TABLE results_std
DROP COLUMN num_brand_repeater_trips_cum
GO

ALTER TABLE results_std
DROP COLUMN avg_brand_transaction_size_dollars
GO

ALTER TABLE results_std
DROP COLUMN brand_dollars_purchased_tick
GO

ALTER TABLE results_std
DROP COLUMN avg_sku_transaction_dollars
GO

ALTER TABLE results_std
DROP COLUMN percent_distribution_brand
GO

ALTER TABLE results_std 
ADD [num_units_unpromo] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [num_units_promo] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [num_units_display] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [display_price] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [percent_at_display_price] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_std 
ADD [volume] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE simulation
ADD [reset_panel_state] [bit] NOT NULL DEFAULT(0)
GO

ALTER TABLE product
ADD [base_price] [float] NOT NULL DEFAULT(1)
GO


ALTER TABLE product
ADD [eq_units] [float] NOT NULL DEFAULT(1)
GO


ALTER TABLE results_summary_by_product 
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_summary_by_product 
ADD [volume] [float] NOT NULL DEFAULT(0)
GO



ALTER TABLE  results_summary_by_product_channel
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE  results_summary_by_product_channel
ADD [volume] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_summary_by_product_segment
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_summary_by_product_segment
ADD [volume] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_summary_by_channel_segment
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE results_summary_by_channel_segment
ADD [volume] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE  results_summary_by_product_channel_segment
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE  results_summary_by_product_channel_segment
ADD [volume] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE  results_summary_total
ADD [eq_units] [float] NOT NULL DEFAULT(0)
GO

ALTER TABLE  results_summary_total
ADD [volume] [float] NOT NULL DEFAULT(0)
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
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume,
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
ELSE 0  END  as dollar_share,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume
from results_std, summary_by_segment, sim_queue, simulation, product  
where sim_queue.run_id = results_std.run_id  and simulation.id = sim_queue.sim_id  
and summary_by_segment.run_id = results_std.run_id  and summary_by_segment.segment_id = results_std.segment_id  
and results_std.calendar_date > simulation.metric_start_date - 1  and results_std.calendar_date < simulation.metric_end_date + 1
and product.product_id = results_std.product_id and product.brand_id = 1
group by   results_std.run_id,  results_std.channel_id,  results_std.segment_id,   summary_by_segment.num_units,   summary_by_segment.num_dollars  
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
AVG(persuasion_sku)/simulation.access_time as persuasion,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume
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

DROP VIEW [summary_by_channel]
GO

Create view [summary_by_channel] as 
Select   results_std.run_id,   results_std.channel_id,    
SUM(num_sku_bought) as num_units,    
CASE    WHEN summary_total.num_units > 0      
THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units     
ELSE 0.0  END   as unit_share,    
SUM(sku_dollar_purchased_tick) as num_dollars,    
CASE    WHEN summary_total.num_dollars > 0      
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars     
ELSE 0.0  END  as dollar_share,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume
FROM results_std, sim_queue, simulation, summary_total, product    
WHERE sim_queue.run_id = results_std.run_id  
and simulation.id = sim_queue.sim_id  
and summary_total.run_id = results_std.run_id    
and results_std.calendar_date > simulation.metric_start_date - 1  
and results_std.calendar_date < simulation.metric_end_date + 1  
and product.product_id = results_std.product_id 
and product.brand_id = 1  
group by   results_std.run_id,   results_std.channel_id,  
summary_total.num_units,  summary_total.num_dollars
GO

DROP VIEW [summary_by_product]
GO

Create view [summary_by_product] as  
Select   results_std.run_id,   results_std.product_id,   
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_total.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units   
ELSE 0.0  END   as unit_share,  
SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_total.num_dollars > 0    
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars   
ELSE 0.0  END  as dollar_share,  
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,  
AVG(persuasion_sku)/simulation.access_time as persuasion,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume
from results_std, sim_queue, simulation, summary_total  
where sim_queue.run_id = results_std.run_id  
and simulation.id = sim_queue.sim_id  
and summary_total.run_id = results_std.run_id  
and results_std.calendar_date > simulation.metric_start_date - 1  
and results_std.calendar_date < simulation.metric_end_date + 1  
group by   results_std.run_id,    results_std.product_id,   
simulation.access_time,  summary_total.num_units,   summary_total.num_dollars   
GO

DROP VIEW [summary_by_product_channel]
GO

Create view [summary_by_product_channel] as  
Select   results_std.run_id,   results_std.product_id,   results_std.channel_id,   
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_by_channel.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_by_channel.num_units    
ELSE 0  END   as unit_share,  
SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_by_channel.num_dollars > 0    
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_channel.num_dollars   
ELSE 0  END  as dollar_share,  
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,  
AVG(persuasion_sku)/simulation.access_time as persuasion,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume  
from results_std, sim_queue, simulation, summary_by_channel  
where sim_queue.run_id = results_std.run_id  and simulation.id = sim_queue.sim_id  
and summary_by_channel.run_id = results_std.run_id  
and summary_by_channel.channel_id = results_std.channel_id  
and results_std.calendar_date > simulation.metric_start_date - 1  
and results_std.calendar_date < simulation.metric_end_date + 1  
group by   results_std.run_id,    results_std.product_id,   
results_std.channel_id,   simulation.access_time,  summary_by_channel.num_units,   
summary_by_channel.num_dollars
GO

DROP VIEW [summary_by_product_channel_segment]
GO

Create view [summary_by_product_channel_segment] as  
Select   results_std.run_id,   results_std.product_id,   results_std.channel_id,   results_std.segment_id,   
SUM(num_sku_bought) as num_units,  
CASE    WHEN summary_by_channel_segment.num_units > 0    
THEN 100.0 * SUM(num_sku_bought)/summary_by_channel_segment.num_units    
ELSE 0  END   as unit_share,  
SUM(sku_dollar_purchased_tick) as num_dollars,  
CASE    WHEN summary_by_channel_segment.num_dollars > 0    
THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_channel_segment.num_dollars   
ELSE 0  END  as dollar_share,  
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,  
AVG(persuasion_sku)/simulation.access_time as persuasion,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume    
from results_std, summary_by_channel_segment, sim_queue, simulation  
where sim_queue.run_id = results_std.run_id  
and simulation.id = sim_queue.sim_id  
and summary_by_channel_segment.run_id = results_std.run_id  
and summary_by_channel_segment.channel_id = results_std.channel_id  
and summary_by_channel_segment.segment_id = results_std.segment_id  
and results_std.calendar_date > simulation.metric_start_date - 1  
and results_std.calendar_date < simulation.metric_end_date + 1  
group by   results_std.run_id,  results_std.product_id,  results_std.channel_id,  
results_std.segment_id,  summary_by_channel_segment.num_units,   
summary_by_channel_segment.num_dollars,  simulation.access_time
GO

DROP VIEW [summary_total]
GO

CREATE VIEW [summary_total] AS  
SELECT  results_std.run_id, 
SUM(results_std.num_sku_bought) AS num_units, 
SUM(results_std.sku_dollar_purchased_tick) AS num_dollars,
SUM(results_std.eq_units) as eq_units,
SUM(results_std.volume) as volume      
FROM results_std 
INNER JOIN  sim_queue ON results_std.run_id = sim_queue.run_id 
INNER JOIN  simulation ON sim_queue.sim_id = simulation.id 
AND results_std.calendar_date > simulation.metric_start_date - 1 
AND  results_std.calendar_date < simulation.metric_end_date + 1 
INNER JOIN product ON results_std.product_id = product.product_id  
WHERE (product.brand_id = 1)  
GROUP BY    results_std.run_id
GO

ALTER TABLE results_std
ALTER COLUMN 
[num_sku_bought] float NOT NULL
GO

ALTER TABLE results_std
ALTER COLUMN 
[sku_dollar_purchased_tick] float NOT NULL
GO
