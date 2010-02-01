INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,3, 'fixed problem with percent_share error')
GO

DROP view res_compare_by_product
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
	THEN (summary_by_product.unit_share - external_data_share_by_product.unit_share)/external_data_share_by_product.unit_share
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

DROP view res_compare_by_product_channel
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
	THEN (summary_by_product_channel.unit_share - external_data_share_by_product_channel.unit_share)/external_data_share_by_product_channel.unit_share
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



