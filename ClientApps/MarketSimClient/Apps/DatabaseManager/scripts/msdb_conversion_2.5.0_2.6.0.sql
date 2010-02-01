INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,6,0, 'Price utility')
GO

CREATE TABLE [price_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[model_id] [int] NOT NULL CONSTRAINT [DF_price_type_model_id]  DEFAULT ((-1)),
	[name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_price_type_name]  DEFAULT (''),
	[relative] [bit] NOT NULL CONSTRAINT [DF_price_type_relative]  DEFAULT ((0)),
	[awareness] [float] NOT NULL CONSTRAINT [DF_price_type_awareness]  DEFAULT ((0.0)),
	[persuasion] [float] NOT NULL CONSTRAINT [DF_price_type_persuasion]  DEFAULT ((0.0)),
	[BOGN] [int] NOT NULL DEFAULT ((0)),
	CONSTRAINT [PK_price_type] PRIMARY KEY CLUSTERED ( [id] ASC )
	WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF),
	CONSTRAINT [FK_price_type_Model_info] FOREIGN KEY([model_id])
	REFERENCES [dbo].[Model_info] ([model_id])
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[segment_price_utility](
	[segment_id] [int] NOT NULL CONSTRAINT [DF_segment_price_utility_segment_id]  DEFAULT ((-1)),
	[price_type_id] [int] NOT NULL CONSTRAINT [DF_segment_price_utility_price_type_id]  DEFAULT ((-1)),
	[util] [float] NOT NULL CONSTRAINT [DF_segment_price_utility_util]  DEFAULT ((0.0)),
	CONSTRAINT [FK_segment_price_utility_price_type] FOREIGN KEY([price_type_id])
	REFERENCES [dbo].[price_type] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO

alter table product_channel add 
price_type [int] NOT NULL DEFAULT ((-1))
GO

INSERT price_type (model_id, [name], awareness)
SELECT model_id, 'Z' as [name], 0.25 as awareness
FROM model_info
GO

INSERT price_type (model_id, [name], BOGN)
SELECT model_id, 'BOGN' as [name], 1 as BOGN
FROM model_info
GO

INSERT price_type (model_id, [name])
SELECT model_id, 'Promoted' as [name]
FROM model_info
GO

update product_channel
set price_type = 
(Select TOP(1) price_type.id from price_type 
where price_type.name = 'Promoted'
AND price_type.model_id = product_channel.model_id)
where ptype = 'promoted'
GO

update product_channel
set price_type = 
(Select TOP(1) price_type.id from price_type 
where price_type.name = 'BOGN'
AND price_type.model_id = product_channel.model_id)
where ptype LIKE 'BOG'
GO

update product_channel
set price_type = 
(Select TOP(1) price_type.id from price_type 
where price_type.name = 'Z'
AND price_type.model_id = product_channel.model_id)
where ptype = 'Z'
GO

INSERT segment_price_utility (segment_id, price_type_id, util)
SELECT segment_id, price_type.id as price_type_id, segment.display_utility as util
FROM segment, price_type
WHERE segment.model_id = price_type.model_id
AND price_type.name = 'Z'
GO

INSERT segment_price_utility (segment_id, price_type_id, util)
SELECT segment_id, price_type.id as price_type_id, 0 as util
FROM segment, price_type
WHERE segment.model_id = price_type.model_id
AND price_type.name = 'Promoted'
GO

INSERT segment_price_utility (segment_id, price_type_id, util)
SELECT segment_id, price_type.id as price_type_id, 0 as util
FROM segment, price_type
WHERE segment.model_id = price_type.model_id
AND price_type.name = 'BOGN'
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