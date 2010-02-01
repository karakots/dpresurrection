INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,2, 'new metric tables and views')
GO

DROP TABLE sim_metric_value
GO

DROP Table [scenario_metric]
GO

CREATE TABLE [scenario_metric] (
	[scenario_id] [int] NOT NULL,
	[token] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT ScenarioMetric FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


Create view summary_total as
Select results_std.run_id,
SUM(num_sku_bought) as num_units,
SUM(sku_dollar_purchased_tick) as num_dollars
from results_std, sim_queue, scenario
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by results_std.run_id
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
AVG(percent_aware_sku_cum) /scenario.access_time as awareness,
AVG(persuasion_sku)/scenario.access_time as persuasion
from results_std, sim_queue, scenario, summary_total
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
scenario.access_time,
summary_total.num_units, 
summary_total.num_dollars 
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
from results_std, sim_queue, scenario, summary_total
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id, 
results_std.channel_id,
summary_total.num_units,
summary_total.num_dollars
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
AVG(percent_aware_sku_cum) /scenario.access_time as awareness,
AVG(persuasion_sku)/scenario.access_time as persuasion
from results_std, sim_queue, scenario, summary_by_channel
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_by_channel.run_id = results_std.run_id
and summary_by_channel.channel_id = results_std.channel_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
results_std.channel_id, 
scenario.access_time,
summary_by_channel.num_units, 
summary_by_channel.num_dollars
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
AVG(percent_aware_sku_cum) /scenario.access_time as awareness,
AVG(persuasion_sku)/scenario.access_time as persuasion
from results_std, sim_queue, scenario, summary_total
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_total.run_id = results_std.run_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id, 
results_std.segment_id,
summary_total.num_units,
summary_total.num_dollars,
scenario.access_time
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
AVG(percent_aware_sku_cum) /scenario.access_time as awareness,
AVG(persuasion_sku)/scenario.access_time as persuasion
from results_std, sim_queue, scenario, summary_by_segment
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_by_segment.run_id = results_std.run_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id,  
results_std.product_id, 
results_std.segment_id, 
scenario.access_time,
summary_by_segment.num_units, 
summary_by_segment.num_dollars
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
from results_std, summary_by_segment, sim_queue, scenario
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_by_segment.run_id = results_std.run_id
and summary_by_segment.segment_id = results_std.segment_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id,
results_std.channel_id,
results_std.segment_id, 
summary_by_segment.num_units, 
summary_by_segment.num_dollars
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
AVG(percent_aware_sku_cum) /scenario.access_time as awareness,
AVG(persuasion_sku)/scenario.access_time as persuasion
from results_std, summary_by_channel_segment, sim_queue, scenario
where sim_queue.run_id = results_std.run_id
and scenario.scenario_id = sim_queue.scenario_id
and summary_by_channel_segment.run_id = results_std.run_id
and summary_by_channel_segment.channel_id = results_std.channel_id
and summary_by_channel_segment.segment_id = results_std.segment_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id,
results_std.product_id,
results_std.channel_id,
results_std.segment_id,
summary_by_channel_segment.num_units, 
summary_by_channel_segment.num_dollars,
scenario.access_time
GO

create table results_summary_total
(
	[run_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	CONSTRAINT [ResSum] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_summary_by_product
(
	[run_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[unit_share] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	[dollar_share] [float] NOT NULL,
	[awareness] [float] NOT NULL,
	[persuasion] [float] NOT NULL,
	CONSTRAINT [ResSumProd] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_summary_by_product_channel
(
	[run_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[unit_share] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	[dollar_share] [float] NOT NULL,
	[awareness] [float] NOT NULL,
	[persuasion] [float] NOT NULL,
	CONSTRAINT [ResSumProdChan] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_summary_by_product_channel_segment
(
	[run_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[segment_id] [int] NOT NULL,
	[num_units] [float] NOT NULL,
	[unit_share] [float] NOT NULL,
	[num_dollars] [float] NOT NULL,
	[dollar_share] [float] NOT NULL,
	[awareness] [float] NOT NULL,
	[persuasion] [float] NOT NULL,
	CONSTRAINT [ResSumProdChanSeg] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create view external_data_summary as
select sim_queue.run_id, SUM(value) as num_units
from external_data, scenario, sim_queue
where
external_data.model_id = sim_queue.model_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.type = 1
and external_data.calendar_date > scenario.metric_start_date - 1
and external_data.calendar_date < scenario.metric_end_date + 1
group by sim_queue.run_id
GO


create view external_data_summary_by_product as
select 
sim_queue.run_id, 
external_data.product_id,
SUM(value) as num_units
from external_data, scenario, sim_queue
where
external_data.model_id = sim_queue.model_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.type = 1
and external_data.calendar_date > scenario.metric_start_date - 1
and external_data.calendar_date < scenario.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.product_id
GO


create view external_data_summary_by_product_channel as
select 
sim_queue.run_id, 
external_data.product_id,
external_data.channel_id,
SUM(value) as num_units
from external_data, scenario, sim_queue
where
external_data.model_id = sim_queue.model_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.type = 1
and external_data.calendar_date > scenario.metric_start_date - 1
and external_data.calendar_date < scenario.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.product_id,
external_data.channel_id
GO

create view external_data_summary_by_channel as
select 
sim_queue.run_id, 
external_data.channel_id,
SUM(value) as num_units
from external_data, scenario, sim_queue
where
external_data.model_id = sim_queue.model_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.type = 1
and external_data.calendar_date > scenario.metric_start_date - 1
and external_data.calendar_date < scenario.metric_end_date + 1
group by 
sim_queue.run_id,
external_data.channel_id
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

create view external_data_share_by_product_channel as
select external_data_summary_by_product_channel.run_id,
external_data_summary_by_product_channel.product_id,
external_data_summary_by_product_channel.channel_id,
CASE
	WHEN external_data_summary_by_channel.num_units > 0
	THEN 100.0 * external_data_summary_by_product_channel.num_units/external_data_summary_by_channel.num_units
	ELSE 0
END
as unit_share
FROM external_data_summary_by_product_channel, external_data_summary_by_channel
WHERE external_data_summary_by_product_channel.run_id = external_data_summary_by_channel.run_id
AND external_data_summary_by_product_channel.channel_id = external_data_summary_by_channel.channel_id
GO

create view sim_calendar_dates as
select DISTINCT run_id, calendar_date
from results_std
GO

create view external_data_alligned_sales_by_product as
select 
sim_calendar_dates.run_id as run_id,
sim_calendar_dates.calendar_date,
external_data.product_id,
SUM(external_data.value) as num_units
from sim_calendar_dates, external_data, scenario, sim_queue
where
sim_queue.run_id = sim_calendar_dates.run_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.model_id = sim_queue.model_id 
and sim_calendar_dates.calendar_date > scenario.metric_start_date - 1
and sim_calendar_dates.calendar_date < scenario.metric_end_date + 1
and external_data.calendar_date < sim_calendar_dates.calendar_date  + 1
and external_data.calendar_date > sim_calendar_dates.calendar_date -  scenario.access_time
and external_data.type = 1
group by 
sim_calendar_dates.run_id, 
sim_calendar_dates.calendar_date,
external_data.product_id
GO

create view external_data_alligned_sales_by_product_channel as
select 
sim_calendar_dates.run_id as run_id,
sim_calendar_dates.calendar_date,
external_data.product_id,
external_data.channel_id,
SUM(external_data.value) as num_units
from sim_calendar_dates, external_data, scenario, sim_queue
where
sim_queue.run_id = sim_calendar_dates.run_id
and scenario.scenario_id = sim_queue.scenario_id
and external_data.model_id = sim_queue.model_id 
and sim_calendar_dates.calendar_date > scenario.metric_start_date - 1
and sim_calendar_dates.calendar_date < scenario.metric_end_date + 1
and external_data.calendar_date < sim_calendar_dates.calendar_date  + 1
and external_data.calendar_date > sim_calendar_dates.calendar_date -  scenario.access_time
and external_data.type = 1
group by 
sim_calendar_dates.run_id, 
sim_calendar_dates.calendar_date,
external_data.product_id,
external_data.channel_id
GO

create view results_by_product as
select 
results_std.run_id as run_id,
results_std.calendar_date,
results_std.product_id,
SUM(results_std.num_sku_bought) as num_units
from results_std, scenario, sim_queue
where
sim_queue.run_id = results_std.run_id
and sim_queue.scenario_id = scenario.scenario_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id, 
results_std.calendar_date,
results_std.product_id
GO

create view results_by_product_channel as
select 
results_std.run_id as run_id,
results_std.calendar_date,
results_std.product_id,
results_std.channel_id,
SUM(results_std.num_sku_bought) as num_units
from results_std, scenario, sim_queue
where
sim_queue.run_id = results_std.run_id
and sim_queue.scenario_id = scenario.scenario_id
and results_std.calendar_date > scenario.metric_start_date - 1
and results_std.calendar_date < scenario.metric_end_date + 1
group by 
results_std.run_id, 
results_std.calendar_date,
results_std.product_id,
results_std.channel_id
GO

create view diff_by_product as
select 
results_by_product.run_id,
results_by_product.calendar_date,
results_by_product.product_id,
results_by_product.num_units - external_data_alligned_sales_by_product.num_units as diff
from results_by_product, external_data_alligned_sales_by_product
where
external_data_alligned_sales_by_product.run_id = results_by_product.run_id
and external_data_alligned_sales_by_product.calendar_date = results_by_product.calendar_date
and external_data_alligned_sales_by_product.product_id = results_by_product.product_id
GO


create view diff_by_product_channel as
select 
results_by_product_channel.run_id,
results_by_product_channel.calendar_date,
results_by_product_channel.product_id,
results_by_product_channel.channel_id,
results_by_product_channel.num_units - external_data_alligned_sales_by_product_channel.num_units as diff
from results_by_product_channel, external_data_alligned_sales_by_product_channel
where
external_data_alligned_sales_by_product_channel.run_id = results_by_product_channel.run_id 
and external_data_alligned_sales_by_product_channel.calendar_date = results_by_product_channel.calendar_date
and external_data_alligned_sales_by_product_channel.product_id = results_by_product_channel.product_id 
and external_data_alligned_sales_by_product_channel.channel_id = results_by_product_channel.channel_id
GO


create view res_compare_by_product as
select summary_by_product.run_id, summary_by_product.product_id,
CASE
	WHEN external_data_summary_by_product.num_units > 0
	THEN 100.0 * SUM(ABS(diff_by_product.diff))/external_data_summary_by_product.num_units
	ELSE 0
END
as mape,
summary_by_product.unit_share as sim_share,
external_data_share_by_product.unit_share as real_share,
summary_by_product.unit_share - external_data_share_by_product.unit_share as share_diff,
CASE
	WHEN external_data_share_by_product.unit_share > 0
	THEN ABS(summary_by_product.unit_share - external_data_share_by_product.unit_share)/external_data_share_by_product.unit_share
	ELSE 0
END
as percent_share_error
from 
diff_by_product, 
external_data_summary_by_product,
summary_by_product, 
external_data_share_by_product
where
external_data_share_by_product.run_id = summary_by_product.run_id
and external_data_summary_by_product.run_id = summary_by_product.run_id
and diff_by_product.run_id = summary_by_product.run_id
and diff_by_product.product_id = summary_by_product.product_id
and external_data_summary_by_product.product_id = summary_by_product.product_id
and external_data_share_by_product.product_id = summary_by_product.product_id
group by 
summary_by_product.run_id, 
summary_by_product.product_id, 
summary_by_product.unit_share, 
external_data_summary_by_product.num_units,
external_data_share_by_product.unit_share
GO

create view res_compare_by_product_channel as
select 
summary_by_product_channel.run_id, 
summary_by_product_channel.product_id, 
summary_by_product_channel.channel_id,
CASE
	WHEN external_data_summary_by_product_channel.num_units > 0
	THEN 100.0 * SUM(ABS(diff_by_product_channel.diff))/external_data_summary_by_product_channel.num_units
	ELSE 0
END
as mape,
summary_by_product_channel.unit_share as sim_share,
external_data_share_by_product_channel.unit_share as real_share,
summary_by_product_channel.unit_share - external_data_share_by_product_channel.unit_share as share_diff,
CASE
	WHEN external_data_share_by_product_channel.unit_share > 0
	THEN ABS(summary_by_product_channel.unit_share - external_data_share_by_product_channel.unit_share)/external_data_share_by_product_channel.unit_share
	ELSE 0
END
as percent_share_error
from 
diff_by_product_channel, 
external_data_summary_by_product_channel,
summary_by_product_channel, 
external_data_share_by_product_channel
where
diff_by_product_channel.run_id = summary_by_product_channel.run_id
and external_data_summary_by_product_channel.run_id = summary_by_product_channel.run_id
and external_data_share_by_product_channel.run_id = summary_by_product_channel.run_id
and diff_by_product_channel.product_id = summary_by_product_channel.product_id
and external_data_summary_by_product_channel.product_id = summary_by_product_channel.product_id
and external_data_share_by_product_channel.product_id = summary_by_product_channel.product_id
and diff_by_product_channel.channel_id = summary_by_product_channel.channel_id
and external_data_summary_by_product_channel.channel_id = summary_by_product_channel.channel_id
and external_data_share_by_product_channel.channel_id = summary_by_product_channel.channel_id
group by 
summary_by_product_channel.run_id, 
summary_by_product_channel.product_id, 
summary_by_product_channel.channel_id,
summary_by_product_channel.unit_share, 
external_data_summary_by_product_channel.num_units,
external_data_share_by_product_channel.unit_share
GO

create view res_compare_by_channel as
select 
diff_by_product_channel.run_id, 
diff_by_product_channel.channel_id,
CASE
	WHEN external_data_summary_by_channel.num_units > 0
	THEN 100.0 * SUM(ABS(diff_by_product_channel.diff))/external_data_summary_by_channel.num_units
	ELSE 0
END
as mape
from 
diff_by_product_channel,  
external_data_summary_by_channel
where
external_data_summary_by_channel.run_id = diff_by_product_channel.run_id
and external_data_summary_by_channel.channel_id = diff_by_product_channel.channel_id
group by 
diff_by_product_channel.run_id, 
diff_by_product_channel.channel_id,
external_data_summary_by_channel.num_units
GO

create view res_compare_total as
select diff_by_product_channel.run_id,
CASE
	WHEN external_data_summary.num_units > 0
	THEN 100.0 * SUM(ABS(diff_by_product_channel.diff))/external_data_summary.num_units
	ELSE 0
END
as mape
from 
diff_by_product_channel,
external_data_summary
where
external_data_summary.run_id = diff_by_product_channel.run_id
group by 
diff_by_product_channel.run_id,
external_data_summary.num_units
GO

create table results_calibration_total
(
	[run_id] [int] NOT NULL,
	[mape] [float] NOT NULL,
	CONSTRAINT [ResCalTotal] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_calibration_by_product
(
	[run_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL,
	[mape] [float] NOT NULL,
	[sim_share] [float] NOT NULL,
	[real_share] [float] NOT NULL,
	[share_diff] [float] NOT NULL,
	[percent_share_error] [float] NOT NULL,
	CONSTRAINT [ResCalProd] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_calibration_by_channel
(
	[run_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL,
	[mape] [float] NOT NULL,
	CONSTRAINT [ResCalChan] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO

create table results_calibration_by_product_channel
(
	[run_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[mape] [float] NOT NULL,
	[sim_share] [float] NOT NULL,
	[real_share] [float] NOT NULL,
	[share_diff] [float] NOT NULL,
	[percent_share_error] [float] NOT NULL,
	CONSTRAINT [ResCalProdChan] FOREIGN KEY ([run_id])
	REFERENCES [sim_queue] ([run_id])
	ON DELETE CASCADE
) on [primary]
GO


