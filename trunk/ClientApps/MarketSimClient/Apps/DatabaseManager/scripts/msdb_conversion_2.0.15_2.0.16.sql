INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,16, 'added market_utility table and fixed market plan dates')
GO

CREATE TABLE [market_utility] (
	[record_id] [int] IDENTITY (1, 1) NOT NULL ,
	[model_id] [int] NOT NULL ,
	[market_plan_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[percent_dist] [float] NULL ,
	[awareness] [float] NULL ,
	[persuasion] [float] NULL ,
	[utility] [float] NULL ,
	[start_date] [datetime] NULL ,
	[end_date] [datetime] NULL ,
	 PRIMARY KEY  CLUSTERED 
	(
		[record_id]
	)  ON [PRIMARY] ,
	CONSTRAINT [ModelMarketUtility] FOREIGN KEY 
	(
		[model_id]
	) REFERENCES [Model_info] (
		[model_id]
	) ON DELETE CASCADE ,
	CONSTRAINT [PlanMarketUtility] FOREIGN KEY 
	(
		[market_plan_id]
	) REFERENCES [market_plan] (
		[id]
	)
) ON [PRIMARY]
GO

INSERT market_plan_type VALUES (7, 'Market Utility')
GO

ALTER TABLE Model_info
ADD [market_utility] [bit] NOT NULL DEFAULT 0
GO


update market_plan 
set start_date = (Select MIN(start_date) from product_channel where market_plan_id = market_plan.id)
where type = 1
GO

update market_plan 
set end_date = (Select MAX(end_date) from product_channel where market_plan_id = market_plan.id)
where type = 1
GO


update market_plan 
set start_date = (Select MIN(start_date) from dist_display where market_plan_id = market_plan.id)
where type = 2 or type = 3
GO

update market_plan 
set end_date = (Select MAX(end_date) from dist_display where market_plan_id = market_plan.id)
where type = 2 or type = 3
GO


update market_plan 
set start_date = (Select MIN(start_date) from mass_media where market_plan_id = market_plan.id)
where type = 4
GO

update market_plan 
set end_date = (Select MAX(end_date) from mass_media where market_plan_id = market_plan.id)
where type = 4
GO


update market_plan 
set start_date = (Select MIN(start_date) from product_event where market_plan_id = market_plan.id)
where type = 5
GO

update market_plan 
set end_date = (Select MAX(end_date) from product_event where market_plan_id = market_plan.id)
where type = 5
GO


update market_plan 
set start_date = (Select MIN(start_date) from task_event where market_plan_id = market_plan.id)
where type = 5
GO

update market_plan 
set end_date = (Select MAX(end_date) from task_event where market_plan_id = market_plan.id)
where type = 5
GO


update market_plan 
set start_date = (select start_date from model_info where model_id = market_plan.model_id)
where start_date is null
GO

update market_plan 
set end_date = (select end_date from model_info where model_id = market_plan.model_id)
where end_date is null
GO


update market_plan
set start_date =
(select MIN(start_date) from market_plan as X, market_plan_tree 
where X.id = market_plan_tree.child_id
and market_plan_tree.parent_id = market_plan.id)
where
market_plan.id in (Select parent_id from market_plan_tree)
GO

update market_plan
set end_date =
(select MAX(end_date) from market_plan as X, market_plan_tree 
where X.id = market_plan_tree.child_id
and market_plan_tree.parent_id = market_plan.id)
where
market_plan.id in (Select parent_id from market_plan_tree)
GO


update market_plan 
set start_date = (select start_date from model_info where model_id = market_plan.model_id)
where start_date is null
GO

update market_plan 
set end_date = (select end_date from model_info where model_id = market_plan.model_id)
where end_date is null
GO


INSERT market_plan_type VALUES (8, 'Coupons')
GO


update market_plan set type = 8
where 
exists(select * from mass_media 
where market_plan_id = market_plan.id 
AND media_type = 'C')