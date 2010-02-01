INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,6, 'Market Magic part two')
GO

drop view scenario_mass_media
GO

create view scenario_mass_media as
select scenario.scenario_id,
market_plan.parm1, market_plan.parm2, market_plan.parm3, market_plan.parm4, market_plan.parm5, market_plan.parm6,
mass_media.*
from scenario, market_plan, mass_media
where 
mass_media.market_plan_id = market_plan.id
and
(
	market_plan.id in 
	(
		select id from my_scenario_market_plans 
		where scenario_id = scenario.scenario_id
		and (
			(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
			or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
			)
	)
	or
	(
		market_plan.id in 
		(
			select id from other_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and (
				(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
				or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
				)
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and (
					(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
					or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
					)
			)
			or
			mass_media.end_date < 
			(
				select MIN(start_date) from mass_media as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (
						(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
						or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
						)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			mass_media.start_date >
			(
				select MAX(end_date) from mass_media as comp
				where comp.market_plan_id in 
				(
					select id 
					from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (
						(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
					or	(type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
						)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and (
				(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
				or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
				)
		)
		and
		(
			mass_media.product_id not in
			(
				select product_id from non_admin_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and	(
					(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
					or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
					)
			)
			or
			mass_media.end_date < 
			(
				select MIN(start_date) from mass_media as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (
						(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
						or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
						)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			mass_media.start_date >
			(
				select MAX(end_date) from mass_media as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (
						(type = 4 AND (mass_media.media_type = 'A' OR mass_media.media_type = 'V')) 
						or (type = 8 AND (mass_media.media_type = 'C' OR mass_media.media_type = 'S'))
						)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
GO

drop view scenario_dist_display
GO

create view scenario_dist_display as
select scenario.scenario_id,
market_plan.parm1, market_plan.parm2, market_plan.parm3, market_plan.parm4, market_plan.parm5, market_plan.parm6,
dist_display.*
from scenario, market_plan, dist_display
where 
dist_display.market_plan_id = market_plan.id
and
(
	market_plan.id in 
	(
		select id from my_scenario_market_plans where scenario_id = scenario.scenario_id
	)
	or
	(
		market_plan.id in 
		(
			select id from other_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and ((type = 2 AND dist_display.media_type = 'D')
				or (type = 3 AND dist_display.media_type = 'Y')
				)
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and  ((type = 2 AND dist_display.media_type = 'D')
					or (type = 3 AND dist_display.media_type = 'Y')
					)
			)
			or
			dist_display.end_date < 
			(
				select MIN(start_date) from dist_display as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and ((type = 2 AND dist_display.media_type = 'D')
						or (type = 3 AND dist_display.media_type = 'Y')
						)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			dist_display.start_date >
			(
				select MAX(end_date) from dist_display as comp
				where comp.market_plan_id in 
				(
					select id 
					from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and ((type = 2 AND dist_display.media_type = 'D')
						or (type = 3 AND dist_display.media_type = 'Y')
						)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and ((type = 2 AND dist_display.media_type = 'D')
				or (type = 3 AND dist_display.media_type = 'Y')
				)
		)
		and
		(
			dist_display.product_id not in
			(
				select product_id from non_admin_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and  ((type = 2 AND dist_display.media_type = 'D')
				or (type = 3 AND dist_display.media_type = 'Y')
				)
			)
			or
			dist_display.end_date < 
			(
				select MIN(start_date) from dist_display as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and ((type = 2 AND dist_display.media_type = 'D')
						or (type = 3 AND dist_display.media_type = 'Y')
						)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			dist_display.start_date >
			(
				select MAX(end_date) from dist_display as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and  ((type = 2 AND dist_display.media_type = 'D')
						or (type = 3 AND dist_display.media_type = 'Y')
						)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
GO

drop view scenario_product_channel
GO

create view scenario_product_channel as
select scenario.scenario_id,
market_plan.parm1, market_plan.parm2, market_plan.parm3, market_plan.parm4, market_plan.parm5, market_plan.parm6,
product_channel.*
from scenario, market_plan, product_channel
where 
product_channel.market_plan_id = market_plan.id
and
(
	market_plan.id in 
	(
		select id from my_scenario_market_plans 
		where scenario_id = scenario.scenario_id
		and (type = 1)
	)
	or
	(
		market_plan.id in 
		(
			select id from other_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and (type = 1)
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and (type = 1)
			)
			or
			product_channel.end_date < 
			(
				select MIN(start_date) from product_channel as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (type = 1)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			product_channel.start_date >
			(
				select MAX(end_date) from product_channel as comp
				where comp.market_plan_id in 
				(
					select id 
					from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (type = 1)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans 
			where scenario_id = scenario.scenario_id
			and (type = 1)
		)
		and
		(
			product_channel.product_id not in
			(
				select product_id from non_admin_scenario_market_plans 
				where scenario_id = scenario.scenario_id
				and (type = 1)
			)
			or
			product_channel.end_date < 
			(
				select MIN(start_date) from product_channel as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (type = 1)
				)
				and comp.product_id = market_plan.product_id
			)
			or
			product_channel.start_date >
			(
				select MAX(end_date) from product_channel as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
					and (type = 1)
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
GO
