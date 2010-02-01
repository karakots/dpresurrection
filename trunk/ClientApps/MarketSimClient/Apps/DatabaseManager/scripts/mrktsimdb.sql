CREATE TABLE db_schema_info
(
	[date]   	datetime NOT NULL DEFAULT getdate(),
	[major_no] 	int NOT NULL,
	[minor_no] 	int NOT NULL,
	[release_no] 	int NOT NULL,
	[comments]   	varchar (100) NOT NULL
) ON [PRIMARY]
GO

INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,13, 'release version')
GO

CREATE TABLE [project] (
	[id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[descr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[read_only] [bit] NOT NULL DEFAULT(0),
	[locked] [bit] NOT NULL DEFAULT(0),
	[created] [datetime] NOT NULL DEFAULT getdate(),
	[modified] [datetime] NOT NULL DEFAULT getdate()
) ON [PRIMARY]
GO

INSERT [project] (name, descr) VALUES ('ssio', 'temp project')
GO

CREATE TABLE [Model_info] (
	[model_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[project_id] [int] NOT NULL ,	
	[model_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[model_type] [int] NOT NULL DEFAULT(0),
	[app_code] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL default(''),
	[descr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[read_only] [bit] NOT NULL DEFAULT(0),
	[locked] [bit] NOT NULL DEFAULT(0),
	[created] [datetime] DEFAULT getdate(),
	[modified] [datetime] DEFAULT getdate(),
	[start_date] [datetime]  ,
	[end_date] [datetime],
	[task_based] [bit] NOT NULL DEFAULT(0),
	[profit_loss] [bit] NOT NULL DEFAULT(0),
	[product_extensions] [bit] NOT NULL DEFAULT(0),
	[product_dependency] [bit] NOT NULL DEFAULT(0),
	[segment_growth] [bit] NOT NULL DEFAULT(0),
	[consumer_budget] [bit] NOT NULL DEFAULT(0),
	[periodic_price] [bit] NOT NULL DEFAULT(0),
	[promoted_price] [bit] NOT NULL DEFAULT(0),
	[distribution] [bit] NOT NULL DEFAULT(0),
	[display] [bit] NOT NULL DEFAULT(0),
	[social_network] [bit] NOT NULL DEFAULT(0),
	[attribute_pre_and_post] [bit] NOT NULL DEFAULT(0),
	CONSTRAINT ProjectModel FOREIGN KEY (project_id)
	REFERENCES project (id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [brand] (
	[model_id] [int] NOT NULL ,	
	[brand_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED ,
	[brand_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	CONSTRAINT ModelBrand FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [product] (
	[model_id] [int] NOT NULL ,
	[product_id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[product_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[brand_id] [int] NOT NULL ,
	[product_base_name] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[type] [varchar] (25) NOT NULL DEFAULT ('Initial'),
	[product_group] [varchar] (25) NOT NULL DEFAULT ('1'),
	[related_group] [varchar] (25) NOT NULL DEFAULT ('none'),
	[percent_relation] [varchar] (25) NOT NULL DEFAULT('irrellevant'),
	[cost] [float] NULL ,
	[initial_dislike_probability] [float] NULL ,
	[repeat_like_probability] [float] NULL ,
	[color] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	CONSTRAINT ModelProduct FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [channel] (
	[model_id] [int] NOT NULL ,
	[channel_id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[channel_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL  ,
	CONSTRAINT ModelChannel FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [segment] (
	[model_id] [int] NOT NULL ,
	[segment_id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[segment_model] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[segment_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[descr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[color] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[show_current_share_pie_chart] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[show_cmulative_share_chart] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[segment_size] [int] NULL ,
	[growth_rate] [float] NULL ,
	[growth_rate_people_percent] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[growth_rate_month_year] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[compress_population] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[variability] [int] NULL ,
	[price_disutility] [float] NULL ,
	[attribute_sensitivity] [float] NULL ,
	[persuasion_scaling] [float] NULL ,
	[display_utility] [float] NULL ,
	[display_utility_scaling_factor] [float] NULL ,
	[max_display_hits_per_trip] [float] NULL ,
	[inertia] [float] NULL ,
	[repurchase] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[repurchase_model] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[gamma_location_parameter_a] [float] NULL ,
	[gamma_shape_parameter_k] [float] NULL ,
	[repurchase_period_frequency] [float] NULL ,
	[repurchase_frequency_variation] [float] NULL ,
	[repurchase_timescale] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[avg_max_units_purch] [float] NULL ,
	[shopping_trip_interval] [float] NULL ,
	[category_penetration] [int] NULL ,
	[category_rejection] [int] NULL ,
	[num_initial_buyers] [bigint] NULL ,
	[initial_buying_period] [float] NULL ,
	[seed_with_repurchasers] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[use_budget] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[budget] [float] NULL ,
	[budget_period] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[save_unspent] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[initial_savings] [float] NULL ,
	[social_network_model] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[num_close_contacts] [float] NULL ,
	[prob_talking_close_contact_pre] [float] NULL ,
	[prob_talking_close_contact_post] [float] NULL ,
	[use_local] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[num_distant_contacts] [float] NULL ,
	[prob_talk_distant_contact_pre] [float] NULL ,
	[prob_talk_distant_contact_post] [float] NULL ,
	[awareness_weight_personal_message] [float] NULL ,
	[pre_persuasion_prob] [float] NULL ,
	[post_persuasion_prob] [float] NULL ,
	[units_desired_trigger] [float] NULL ,
	[awareness_model] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[awareness_threshold] [float] NULL ,
	[awareness_decay_rate_pre] [float] NULL ,
	[awareness_decay_rate_post] [float] NULL ,
	[persuasion_decay_rate_pre] [float] NULL ,
	[persuasion_decay_rate_post] [float] NULL ,
	[persuasion_decay_method] [char] (1)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[product_choice_model] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[persuasion_score] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[persuasion_value_computation] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[persuasion_contribution_overall_score] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[utility_score] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[combination_part_utilities] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[price_contribution_overall_score] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[price_score] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[price_value] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[reference_price] [float] NULL ,
	[choice_prob] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[inertia_model] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[error_term] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[error_term_user_value] [int] NULL  ,
	[loyalty] [float] not null DEFAULT (0),
	CONSTRAINT ModelSegment FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE network_type
(
	[id] [int] NOT NULL PRIMARY KEY CLUSTERED,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

INSERT network_type VALUES (0, 'Talk Anytime')
GO

INSERT network_type VALUES (1, 'Purchased Triggered')
GO

CREATE TABLE [network_parameter]
(
	[model_id] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[type] [int] NOT NULL,
	[persuasion_pre_use] [float] NOT NULL,
	[persuasion_post_use] [float] NOT NULL,
	[awareness_weight] [float] NOT NULL,
	[num_contacts] [float] NOT NULL,
	[prob_of_talking_pre_use] [float] NOT NULL,
	[prob_of_talking_post_use] [float] NOT NULL,
	[use_local] [bit] NOT NULL,
	[percent_talking] [float] NOT NULL,
	[neg_persuasion_reject] [float] NOT NULL,
	[neg_persuasion_pre_use] [float] NOT NULL,
	[neg_persuasion_post_use] [float] NOT NULL,
	CONSTRAINT ModelNetworkParam FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT NetworkTypeNetworkParam FOREIGN KEY ([type])
	REFERENCES network_type ([id])
	ON DELETE CASCADE
)
GO

CREATE TABLE [segment_network]
(
	[model_id] [int] NOT NULL,
	[from_id] [int] NOT NULL,
	[to_id] [int] NOT NULL,
	[network_param] [int] NOT NULL,
	CONSTRAINT ModelSegmentNetwork FOREIGN KEY ([model_id])
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
)
GO


CREATE TABLE [product_attribute] (
	[model_id] [int] NOT NULL ,
	[product_attribute_id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[product_attribute_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[utility_max] [float] NULL ,
	[utility_min] [float] NULL   ,
	[cust_pref_max] [float] NULL ,
	[cust_pref_min] [float] NULL   ,
	[cust_tau] [float] NULL ,
	CONSTRAINT ModelProdAtrr FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [task] (
	[model_id] [int] NOT NULL ,
	[task_id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[task_name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL   ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL ,
	[suitability_min] [tinyint] NULL ,
	[suitability_max] [tinyint] NULL ,
	CONSTRAINT ModelTask FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO



CREATE TABLE [product_matrix] (
	[model_id] [int] NOT NULL ,
	[have_product_id] [int] NOT NULL ,
	[want_product_id] [int] NOT NULL ,
	[value] [varchar] (25) NOT NULL   ,
	CONSTRAINT ModelProdMat FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [consumer_preference] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[product_attribute_id] [int] NOT NULL ,
	[start_date] [datetime] NULL ,
	[pre_preference_value] [float] NOT NULL  DEFAULT (0),
	[post_preference_value] [float] NOT NULL DEFAULT (0),
	CONSTRAINT ModelConsPref FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [product_attribute_value] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[product_attribute_id] [int] NOT NULL ,
	[start_date] [datetime] NULL ,
	[pre_attribute_value] [float] NOT NULL  DEFAULT (0),
	[post_attribute_value] [float] NOT NULL DEFAULT (0),
	[has_attribute] [bit] NOT NULL DEFAULT(0),
	CONSTRAINT ModelProdAttrValues FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [segment_channel] (
	[model_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[probability_of_choice] [char] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL   ,
	CONSTRAINT ModelSegChannel FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [share_pen_brand_aware] (
	[model_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[initial_share] [float] NULL ,
	[penetration] [float] NULL ,
	[brand_awareness] [float] NULL ,
	[persuasion] [float] NULL   ,
	CONSTRAINT ModelSharePenBrandAware FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [task_product_fact] (
	[model_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[task_id] [int] NOT NULL ,
	[pre_use_upsku] [int] NULL ,
	[post_use_upsku] [int] NULL ,
	[suitability] [tinyint] NULL ,
	CONSTRAINT ModelTaskProdFact FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO




CREATE TABLE [task_rate_fact] (
	[model_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[task_id] [int] NOT NULL ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL ,
	[time_period] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[task_rate] [float] NULL ,
	CONSTRAINT ModelTaskRateFact FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [time_interval]
(
	[interval_id] [tinyint] PRIMARY KEY CLUSTERED,
	[interval] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)ON [PRIMARY]
GO

INSERT [time_interval] VALUES (0, 'None')
GO

INSERT [time_interval] VALUES (1, 'Daily')
GO

INSERT [time_interval] VALUES (2, 'Weekly')
GO

INSERT [time_interval] VALUES (3, 'Monthly')
GO

INSERT [time_interval] VALUES (4, 'Yearly')
GO

CREATE TABLE [market_plan_type]
(
	[id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [char] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)ON [PRIMARY]
GO

INSERT market_plan_type VALUES (0, 'Market Plan')
GO
INSERT market_plan_type VALUES (1, 'Price')
GO
INSERT market_plan_type VALUES (2, 'Distribution')
GO
INSERT market_plan_type VALUES (3, 'Display')
GO
INSERT market_plan_type VALUES (4, 'Media')
GO
INSERT market_plan_type VALUES (5, 'External Factors')
GO
INSERT market_plan_type VALUES (6, 'Task Factors')
GO


CREATE TABLE [market_plan]
(
	[model_id] [int] NOT NULL ,
	[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[type] [tinyint] NOT NULL DEFAULT(0),
	[name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[descr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[start_date] [datetime],
	[end_date]	[datetime],
	[interval] [tinyint] NOT NULL DEFAULT(0),
	[product_id] [int] NOT NULL DEFAULT(-1),
	[segment_id] [int] NOT NULL DEFAULT(-1),
	[channel_id] [int] NOT NULL DEFAULT(-1),
	[task_id] [int] NOT NULL DEFAULT(-1),
	[parm1]	[float] NOT NULL Default(1),
	[parm2]	[float] NOT NULL Default(1),
	[parm3]	[float] NOT NULL Default(1),
	[parm4]	[float] NOT NULL Default(1),
	[parm5]	[float] NOT NULL Default(1),
	[parm6]	[float] NOT NULL Default(1),
	CONSTRAINT ModelMarketPlan FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [market_plan_tree]
(
	[model_id] [int] NOT NULL ,
	[parent_id] [int] NOT NULL,
	[child_id] [int] NOT NULL,
	CONSTRAINT ModelMarketPlanTree FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [product_channel] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[market_plan_id] [int] NOT NULL,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[markup] [float] NULL ,
	[price] [float] NULL ,
	[periodic_price] [float] NULL ,
	[how_often] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[percent_SKU_in_dist] [float] NULL ,
	[ptype] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL,
	[duration] [int] NULL   ,
	CONSTRAINT ModelProdChannel FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT PlanProdChannel FOREIGN KEY (market_plan_id)
	REFERENCES market_plan (id)
	ON DELETE NO ACTION
) ON [PRIMARY]
GO

CREATE TABLE [dist_display] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[market_plan_id] [int] NOT NULL,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL,
	[media_type] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[attr_value_F] [float] NULL ,
	[attr_value_G] [float] NULL ,
	[message_awareness_probability] [float] NULL ,
	[message_persuation_probability] [float] NULL ,
	[start_date] [datetime] NULL ,
	[end_date] [datetime] NULL ,
	[duration] [int] NULL 	  ,
	CONSTRAINT ModelDistDisplay FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT PlanDistDisplay FOREIGN KEY (market_plan_id)
	REFERENCES market_plan (id)
	ON DELETE NO ACTION
	
) ON [PRIMARY]
GO

CREATE TABLE [mass_media] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[market_plan_id] [int] NOT NULL,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[media_type] [nchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[attr_value_G] [float] NULL ,
	[attr_value_H] [float] NULL ,
	[attr_value_I] [float] NULL ,
	[message_awareness_probability] [float] NULL ,
	[message_persuation_probability] [float] NULL ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL ,
	[duration] [int] NULL   ,
	CONSTRAINT ModelMassMedia FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT PlanMassMedia FOREIGN KEY (market_plan_id)
	REFERENCES market_plan (id)
	ON DELETE NO ACTION
) ON [PRIMARY]
GO

CREATE TABLE [product_event_type] (
	[type_id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO

INSERT product_event_type (type_id, type)
VALUES (0, 'Units Purchased')
GO

INSERT product_event_type (type_id, type)
VALUES (1, 'Shopping trips')
GO

INSERT product_event_type (type_id, type)
VALUES (2, 'Price Sensitivity')
GO

CREATE TABLE [product_event] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL,
	[market_plan_id] [int] NOT NULL,
	[segment_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[demand_modification] [float] NULL ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL ,
	[type] [tinyint] NOT NULL default(0),
	CONSTRAINT ModelProdEvent FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT PlanProdEvent FOREIGN KEY (market_plan_id)
	REFERENCES market_plan (id)
	ON DELETE NO ACTION,
	CONSTRAINT ProductEventType FOREIGN KEY (type)
	REFERENCES [product_event_type] (type_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [task_event] (
	[record_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL ,
	[market_plan_id] [int] NOT NULL,
	[segment_id] [int] NOT NULL ,
	[task_id] [int] NOT NULL ,
	[demand_modification] [float] NULL ,
	[start_date] [datetime] NOT NULL ,
	[end_date] [datetime] NOT NULL ,
	CONSTRAINT ModelTaskEvent FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE,
	CONSTRAINT PlanTaskEvent FOREIGN KEY (market_plan_id)
	REFERENCES market_plan (id)
	ON DELETE NO ACTION
) ON [PRIMARY]
GO


CREATE TABLE [scenario_type]
(
	[scenario_type_id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)ON [PRIMARY]
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (0, 'standard')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (32, 'Parallel Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (64, 'Serial Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (80, 'Random Search')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (96, 'Optimization')
GO

INSERT scenario_type (scenario_type_id, type)
VALUES (128, 'Statistical')
GO

CREATE TABLE [scenario]
(
	[scenario_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[model_id] [int] NOT NULL,
	[name] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[descr] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[type] [tinyint] NOT NULL DEFAULT(0),
	locked [bit] NOT NULL DEFAULT(0),
	[queued] [bit] NOT NULL DEFAULT(0),
	[start_date] [datetime],
	[end_date] [datetime],
	[sim_num] [int] NOT NULL Default(-1),
	CONSTRAINT ScenarioType FOREIGN KEY (type)
	REFERENCES [scenario_type] (scenario_type_id)
	ON DELETE CASCADE,
	CONSTRAINT ModelScenario FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [scenario_market_plan]
(
	[scenario_id] [int] NOT NULL,
	[market_plan_id] [int] NOT NULL,
	[model_id] [int] NOT NULL,
	CONSTRAINT ModelScenarioMarketPlan FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [sim_status]
(
	[status_id] [tinyint] PRIMARY KEY CLUSTERED,
	[status] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)ON [PRIMARY]
GO

INSERT [sim_status]VALUES (0, 'pending')
GO
INSERT [sim_status]VALUES (1, 'running')
GO
INSERT [sim_status]VALUES (2, 'done')
GO

	
CREATE TABLE [sim_queue]
(
	[run_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[scenario_id] [int],
	[model_id] [int],
	[status] [tinyint] NOT NULL DEFAULT(0),
	[num]	[int]	NOT NULL,
	[name]  [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[current_date] [datetime],
	[elapsed_time] [int] NOT NULL Default(0),
	[current_status] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL default('not started'),
	[run_time] [datetime],
	[seed] [int] NOT NULL Default(1),
	CONSTRAINT StatusQueue FOREIGN KEY (status)
	REFERENCES sim_status (status_id)
	ON DELETE CASCADE,
	CONSTRAINT ScenariorRun FOREIGN KEY (scenario_id)
	REFERENCES scenario (scenario_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [results_std] (
	[run_id] [int] NOT NULL ,
	[calendar_date] [datetime] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[num_adds_sku] [int] NULL ,
	[num_drop_sku] [int] NULL ,
	[num_adds_brand] [int] NULL ,
	[num_drop_brand] [int] NULL ,
	[percent_aware_sku_cum] [float] NULL ,
	[percent_aware_brand_cum] [float] NULL ,
	[persuasion_sku] [float] NULL ,
	[num_brand_triers_cum] [int] NULL ,
	[num_brand_repeaters_cum] [int] NULL ,
	[num_brand_repeater_trips_cum] [int] NULL ,
	[avg_brand_transaction_size_dollars] [float] NULL ,
	[brand_dollars_purchased_tick] [float] NULL ,
	[GRPs_SKU_tick] [float] NULL ,
	[percent_distribution_brand] [float] NULL ,
	[promoprice] [float] NULL ,
	[unpromoprice] [float] NULL ,
	[num_coupon_redemptions] [int] NULL ,
	[num_units_bought_on_coupon] [int] NULL ,
	[num_sku_bought] [int] NULL ,
	[num_sku_triers] [int] NULL ,
	[num_sku_repeaters] [int] NULL ,
	[num_trips] [int] NULL,
	[num_sku_repeater_trips_cum] [int] NULL ,
	[avg_sku_transaction_dollars] [float] NULL ,
	[sku_dollar_purchased_tick] [float] NULL ,
	[percent_preuse_distribution_sku] [float] NULL ,
	[percent_on_display_sku] [float] NULL ,
	[percent_sku_at_promo_price] [float] NULL   ,
	CONSTRAINT ModelResults FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO


CREATE TABLE [external_data_type]
(
	[id] [int] PRIMARY KEY CLUSTERED,
	[type] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[descr] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[sim_type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[average] [bit] NOT NULL default(0)
)ON [PRIMARY]
GO

INSERT external_data_type VALUES (1, 'real_sales', 'Real Sales', 'num_sku_bought', 0)
GO

INSERT external_data_type VALUES (2, 'real_awareness', 'Real Awareness', 'percent_aware_sku_cum', 1)
GO

CREATE TABLE [external_data]
(
	[model_id] [int] NOT NULL ,
	[calendar_date] [datetime] NOT NULL ,
	[segment_id] [int] NOT NULL ,
	[product_id] [int] NOT NULL ,
	[channel_id] [int] NOT NULL ,
	[type] [int] NOT NULL,
	[value] [float] NOT NULL,
	CONSTRAINT DataTypeExternalData FOREIGN KEY (type)
	REFERENCES external_data_type ([id])
	ON DELETE CASCADE,
	CONSTRAINT ModelExternalData FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE INDEX results_sort_index ON results_std (run_id, calendar_date)
GO


CREATE VIEW top_market_plan AS
SELECT model_id, id, name, descr, product_id
FROM market_plan WHERE type = 0
GO


CREATE TABLE model_parameter (
[model_id] [int] NOT NULL ,
[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[table_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[col_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[filter] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[identity_row] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[row_id] int NULL,
CONSTRAINT ModelParam FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE scenario_parameter (
[model_id] [int] NOT NULL ,
[scenario_id] [int] NOT NULL,
[param_id] [int] NOT NULL,
[aValue] [float] NOT NULL,
[origValue] [float] NOT NULL Default(0),
[expression] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
CONSTRAINT ModelScenarioParam FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [run_log] (
	[run_id] [int] NOT NULL,
	[calendar_date] [datetime] NOT NULL ,
	[product_id]	[int] NULL,
	[segment_id]	[int] NULL,
	[channel_id]	[int] NULL,
	[comp_id]	[int] NOT NULL,
	[message_id]	[int]  NOT NULL,
	[message]	varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT SimQueueLog FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO


CREATE TABLE [variable_type] (
	[type_id] [tinyint] PRIMARY KEY CLUSTERED,
	[type] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO

INSERT variable_type (type_id, type)
VALUES (0, 'Stepped')
GO

INSERT variable_type (type_id, type)
VALUES (1, 'Random (Uniform)')
GO

INSERT variable_type (type_id, type)
VALUES (2, 'Random (Centered)')
GO

CREATE TABLE [scenario_variable] (
	[scenario_id] [int] NOT NULL,
	[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[token] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[descr] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[min] [float] NOT NULL,
	[max] [float] NOT NULL,
	[num_steps] [int] NOT NULL Default(0),
	[type] [tinyint] NOT NULL default(0),
	CONSTRAINT ScenarioVariable FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE,
	CONSTRAINT VariableType FOREIGN KEY (type)
	REFERENCES [variable_type] (type_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO

CREATE TABLE [scenario_simseed] (
	[scenario_id] [int] NOT NULL,
	[id]  [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[seed] [int] NOT NULL Default(1),
	CONSTRAINT ScenarioSimSeed FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE 
) ON [PRIMARY]
GO

CREATE TABLE [sim_variable_value] (
	[run_id] [int] NOT NULL,
	[var_id] [int] NOT NULL,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimVariableValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [sim_metric_value] (
	[run_id] [int] NOT NULL,
	[product_id] [int],
	[segment_id] [int],
	[channel_id] [int],
	[token] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[val]	 [float] NOT NULL,
	CONSTRAINT SimMetricValue FOREIGN KEY (run_id)
	REFERENCES sim_queue(run_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


CREATE TABLE [scenario_metric] (
	[scenario_id] [int] NOT NULL,
	[token] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT ScenarioMetric FOREIGN KEY (scenario_id)
	REFERENCES scenario(scenario_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO


