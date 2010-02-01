INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,2, 'Metric Scaling and simulation fix')
GO

DROP VIEW   summary_total
GO

CREATE VIEW summary_total AS
SELECT      dbo.results_std.run_id, SUM(dbo.results_std.num_sku_bought) AS num_units, SUM(dbo.results_std.sku_dollar_purchased_tick) AS num_dollars
FROM        dbo.results_std INNER JOIN
                      dbo.sim_queue ON dbo.results_std.run_id = dbo.sim_queue.run_id INNER JOIN
                      dbo.simulation ON dbo.sim_queue.sim_id = dbo.simulation.id AND dbo.results_std.calendar_date > dbo.simulation.metric_start_date - 1 AND 
                      dbo.results_std.calendar_date < dbo.simulation.metric_end_date + 1 INNER JOIN
                      dbo.product ON dbo.results_std.product_id = dbo.product.product_id
WHERE       (dbo.product.brand_id = 1)
GROUP BY    dbo.results_std.run_id
GO

DROP VIEW summary_by_product
GO

Create view summary_by_product as
Select 
results_std.run_id, 
results_std.product_id, 
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_total.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units
	ELSE 0.0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_total.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars
	ELSE 0.0
END
as dollar_share,
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,
AVG(persuasion_sku)/simulation.access_time as persuasion
from results_std, sim_queue, simulation, summary_total
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
simulation.access_time,
summary_total.num_units, 
summary_total.num_dollars 
GO

drop view summary_by_channel
GO

Create view summary_by_channel as
Select 
results_std.run_id, 
results_std.channel_id,
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_total.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units
	ELSE 0.0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_total.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars
	ELSE 0.0
END
as dollar_share
from results_std, sim_queue, simulation, summary_total
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id, 
results_std.channel_id,
summary_total.num_units,
summary_total.num_dollars
GO

DROP VIEW summary_by_product_channel
GO

Create view summary_by_product_channel as
Select 
results_std.run_id, 
results_std.product_id, 
results_std.channel_id, 
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_by_channel.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_by_channel.num_units 
	ELSE 0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_by_channel.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_channel.num_dollars
	ELSE 0
END
as dollar_share,
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,
AVG(persuasion_sku)/simulation.access_time as persuasion
from results_std, sim_queue, simulation, summary_by_channel
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_by_channel.run_id = results_std.run_id
and summary_by_channel.channel_id = results_std.channel_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
results_std.channel_id, 
simulation.access_time,
summary_by_channel.num_units, 
summary_by_channel.num_dollars
GO

DROP view summary_by_segment
GO

Create view summary_by_segment as
Select 
results_std.run_id, 
results_std.segment_id,
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_total.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_total.num_units
	ELSE 0.0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_total.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_total.num_dollars
	ELSE 0.0
END
as dollar_share,
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,
AVG(persuasion_sku)/simulation.access_time as persuasion
from results_std, sim_queue, simulation, summary_total
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id, 
results_std.segment_id,
summary_total.num_units,
summary_total.num_dollars,
simulation.access_time
GO

DROP view summary_by_product_segment
GO

Create view summary_by_product_segment as
Select 
results_std.run_id, 
results_std.product_id, 
results_std.segment_id, 
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_by_segment.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_by_segment.num_units 
	ELSE 0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_by_segment.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_segment.num_dollars
	ELSE 0
END
as dollar_share,
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,
AVG(persuasion_sku)/simulation.access_time as persuasion
from results_std, sim_queue, simulation, summary_by_segment
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_by_segment.run_id = results_std.run_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
results_std.segment_id, 
simulation.access_time,
summary_by_segment.num_units, 
summary_by_segment.num_dollars
GO

DROP view summary_by_channel_segment
GO

Create view summary_by_channel_segment as
Select 
results_std.run_id, 
results_std.channel_id, 
results_std.segment_id, 
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_by_segment.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_by_segment.num_units 
	ELSE 0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_by_segment.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_segment.num_dollars
	ELSE 0
END
as dollar_share
from results_std, summary_by_segment, sim_queue, simulation
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_by_segment.run_id = results_std.run_id
and summary_by_segment.segment_id = results_std.segment_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id,
results_std.channel_id,
results_std.segment_id, 
summary_by_segment.num_units, 
summary_by_segment.num_dollars
GO

DROP view summary_by_product_channel_segment
GO

Create view summary_by_product_channel_segment as
Select 
results_std.run_id, 
results_std.product_id, 
results_std.channel_id, 
results_std.segment_id, 
SUM(num_sku_bought) as num_units,
CASE 
	WHEN summary_by_channel_segment.num_units > 0 
	THEN 100.0 * SUM(num_sku_bought)/summary_by_channel_segment.num_units 
	ELSE 0
END 
as unit_share,
SUM(sku_dollar_purchased_tick) as num_dollars,
CASE 
	WHEN summary_by_channel_segment.num_dollars > 0 
	THEN 100.0 * SUM(sku_dollar_purchased_tick)/ summary_by_channel_segment.num_dollars
	ELSE 0
END
as dollar_share,
AVG(percent_aware_sku_cum) /simulation.access_time as awareness,
AVG(persuasion_sku)/simulation.access_time as persuasion
from results_std, summary_by_channel_segment, sim_queue, simulation
where sim_queue.run_id = results_std.run_id
and simulation.id = sim_queue.sim_id
and summary_by_channel_segment.run_id = results_std.run_id
and summary_by_channel_segment.channel_id = results_std.channel_id
and summary_by_channel_segment.segment_id = results_std.segment_id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id,
results_std.product_id,
results_std.channel_id,
results_std.segment_id,
summary_by_channel_segment.num_units, 
summary_by_channel_segment.num_dollars,
simulation.access_time
GO

DROP view external_data_summary
GO

create view external_data_summary as
select sim_queue.run_id, SUM(value) as num_units
from external_data, simulation, sim_queue
where
external_data.model_id = sim_queue.model_id
and simulation.id = sim_queue.sim_id
and external_data.type = 1
and external_data.calendar_date > simulation.metric_start_date - 1
and external_data.calendar_date < simulation.metric_end_date + 1
group by sim_queue.run_id
GO

DROP view external_data_summary_by_product
GO

create view external_data_summary_by_product as
select 
sim_queue.run_id, 
external_data.product_id,
SUM(value) as num_units
from external_data, simulation, sim_queue
where
external_data.model_id = sim_queue.model_id
and simulation.id = sim_queue.sim_id
and external_data.type = 1
and external_data.calendar_date > simulation.metric_start_date - 1
and external_data.calendar_date < simulation.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.product_id
GO

DROP view external_data_summary_by_product_channel
GO

create view external_data_summary_by_product_channel as
select 
sim_queue.run_id, 
external_data.product_id,
external_data.channel_id,
SUM(value) as num_units
from external_data, simulation, sim_queue
where
external_data.model_id = sim_queue.model_id
and simulation.id = sim_queue.sim_id
and external_data.type = 1
and external_data.calendar_date > simulation.metric_start_date - 1
and external_data.calendar_date < simulation.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.product_id,
external_data.channel_id
GO

DROP view external_data_summary_by_channel
GO

create view external_data_summary_by_channel as
select 
sim_queue.run_id, 
external_data.channel_id,
SUM(value) as num_units
from external_data, simulation, sim_queue
where
external_data.model_id = sim_queue.model_id
and simulation.id = sim_queue.sim_id
and external_data.type = 1
and external_data.calendar_date > simulation.metric_start_date - 1
and external_data.calendar_date < simulation.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.channel_id
GO

DROP view external_data_share_by_product
GO

create view external_data_share_by_product as
select external_data_summary_by_product.run_id,
external_data_summary_by_product.product_id,
CASE
	WHEN external_data_summary.num_units > 0
	THEN 100.0 * external_data_summary_by_product.num_units/external_data_summary.num_units
	ELSE 0
END
as unit_share
FROM external_data_summary_by_product, external_data_summary
WHERE external_data_summary_by_product.run_id = external_data_summary.run_id
GO

DROP view external_data_alligned_sales_by_product
GO

create view external_data_alligned_sales_by_product as
select 
sim_calendar_dates.run_id as run_id,
sim_calendar_dates.calendar_date,
external_data.product_id,
SUM(external_data.value) as num_units
from sim_calendar_dates, external_data, simulation, sim_queue
where
sim_queue.run_id = sim_calendar_dates.run_id
and simulation.id = sim_queue.sim_id
and external_data.model_id = sim_queue.model_id 
and sim_calendar_dates.calendar_date > simulation.metric_start_date - 1
and sim_calendar_dates.calendar_date < simulation.metric_end_date + 1
and external_data.calendar_date < sim_calendar_dates.calendar_date  + 1
and external_data.calendar_date > sim_calendar_dates.calendar_date -  simulation.access_time
and external_data.type = 1
group by 
sim_calendar_dates.run_id, 
sim_calendar_dates.calendar_date,
external_data.product_id
GO

DROP view external_data_alligned_sales_by_product_channel
GO

create view external_data_alligned_sales_by_product_channel as
select 
sim_calendar_dates.run_id as run_id,
sim_calendar_dates.calendar_date,
external_data.product_id,
external_data.channel_id,
SUM(external_data.value) as num_units
from sim_calendar_dates, external_data, simulation, sim_queue
where
sim_queue.run_id = sim_calendar_dates.run_id
and simulation.id = sim_queue.sim_id
and external_data.model_id = sim_queue.model_id 
and sim_calendar_dates.calendar_date > simulation.metric_start_date - 1
and sim_calendar_dates.calendar_date < simulation.metric_end_date + 1
and external_data.calendar_date < sim_calendar_dates.calendar_date  + 1
and external_data.calendar_date > sim_calendar_dates.calendar_date -  simulation.access_time
and external_data.type = 1
group by 
sim_calendar_dates.run_id, 
sim_calendar_dates.calendar_date,
external_data.product_id,
external_data.channel_id
GO

DROP view results_by_product
GO

create view results_by_product as
select 
results_std.run_id as run_id,
results_std.calendar_date,
results_std.product_id,
SUM(results_std.num_sku_bought) as num_units
from results_std, simulation, sim_queue
where
sim_queue.run_id = results_std.run_id
and sim_queue.sim_id = simulation.id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id, 
results_std.calendar_date,
results_std.product_id
GO

DROP view results_by_product_channel
GO

create view results_by_product_channel as
select 
results_std.run_id as run_id,
results_std.calendar_date,
results_std.product_id,
results_std.channel_id,
SUM(results_std.num_sku_bought) as num_units
from results_std, simulation, sim_queue
where
sim_queue.run_id = results_std.run_id
and sim_queue.sim_id = simulation.id
and results_std.calendar_date > simulation.metric_start_date - 1
and results_std.calendar_date < simulation.metric_end_date + 1
group by 
results_std.run_id, 
results_std.calendar_date,
results_std.product_id,
results_std.channel_id
GO