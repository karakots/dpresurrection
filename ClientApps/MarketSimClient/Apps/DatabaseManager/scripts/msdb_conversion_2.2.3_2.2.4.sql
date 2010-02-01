INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,4, 'adding market magic')
GO

update scenario set user_name = 'admin' 
where user_name = 'all' or user_name = ''
GO

update market_plan set user_name = 'admin' 
where user_name = 'all' or user_name = ''
GO

create view scenario_market_plans as
select scenario.scenario_id, scenario.user_name as scenario_user_name, market_plan.*
from scenario, market_plan
where 
market_plan.id in
(
	select child_id from market_plan_tree where parent_id in
	(
		select market_plan_id from scenario_market_plan where scenario_id = scenario.scenario_id
	)
)
GO

create view my_scenario_market_plans as
select scenario_market_plans.*
from scenario_market_plans
where user_name = scenario_user_name
GO

create view admin_scenario_market_plans as
select scenario_market_plans.*
from scenario_market_plans
where user_name = 'admin'
GO

create view other_scenario_market_plans as
select scenario_market_plans.*
from scenario_market_plans
where user_name <> 'admin'
and user_name <> scenario_user_name
GO

create view non_admin_scenario_market_plans as
select scenario_market_plans.*
from scenario_market_plans
where user_name <> 'admin'
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
		select id from my_scenario_market_plans where scenario_id = scenario.scenario_id
	)
	or
	(
		market_plan.id in 
		(
			select id from other_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			mass_media.end_date < 
			(
				select MIN(start_date) from mass_media as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			mass_media.product_id not in
			(
				select product_id from non_admin_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			mass_media.end_date < 
			(
				select MIN(start_date) from mass_media as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
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
			select id from other_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			dist_display.end_date < 
			(
				select MIN(start_date) from dist_display as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			dist_display.product_id not in
			(
				select product_id from non_admin_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			dist_display.end_date < 
			(
				select MIN(start_date) from dist_display as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
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
		select id from my_scenario_market_plans where scenario_id = scenario.scenario_id
	)
	or
	(
		market_plan.id in 
		(
			select id from other_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			market_plan.product_id not in
			(
				select product_id from my_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			product_channel.end_date < 
			(
				select MIN(start_date) from product_channel as comp 
				where comp.market_plan_id in 
				(
					select id from my_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
	or
	(
		market_plan.id in 
		(
			select id from admin_scenario_market_plans where scenario_id = scenario.scenario_id
		)
		and
		(
			product_channel.product_id not in
			(
				select product_id from non_admin_scenario_market_plans where scenario_id = scenario.scenario_id
			)
			or
			product_channel.end_date < 
			(
				select MIN(start_date) from product_channel as comp
				where comp.market_plan_id in 
				(
					select id from non_admin_scenario_market_plans 
					where scenario_id = scenario.scenario_id
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
				)
				and comp.product_id = market_plan.product_id
			)
		)
	)
)
GO
