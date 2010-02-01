INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,3,4, 'channel summary fix')
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
ELSE 0.0  END  as dollar_share  
FROM results_std, sim_queue, simulation, summary_total, product  
WHERE sim_queue.run_id = results_std.run_id  and simulation.id = sim_queue.sim_id  and summary_total.run_id = results_std.run_id  
and results_std.calendar_date > simulation.metric_start_date - 1  and results_std.calendar_date < simulation.metric_end_date + 1
and product.product_id = results_std.product_id and product.brand_id = 1
group by   results_std.run_id,   results_std.channel_id,  summary_total.num_units,  summary_total.num_dollars  
GO
