Drop Table [sim_queue]
GO

/****** Object:  Table [dbo].[sim_queue]    Script Date: 09/15/2008 11:43:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sim_queue](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_sim_queue_user_id]  DEFAULT (N'admin'),
	[state] [int] NOT NULL CONSTRAINT [DF_sim_queue_state]  DEFAULT ((-1)),
	[created] [datetime] NOT NULL CONSTRAINT [DF_sim_queue_created]  DEFAULT (getdate()),
	[run_time] [datetime] NOT NULL CONSTRAINT [DF_sim_queue_run_time]  DEFAULT ('1-1-2000'),
	[progress] [float] NOT NULL CONSTRAINT [DF_sim_queue_progress]  DEFAULT ((0)),
	[status] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_sim_queue_status]  DEFAULT (''),
	[input] [varbinary](max) NULL,
	[output] [varbinary](max) NULL,
	[movie] [varbinary] (max) NULL,
	[total_time] [int] NOT NULL CONSTRAINT [DF_sim_queue_total_time]  DEFAULT ((0)),
	[app_code] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_sim_queue_app_code]  DEFAULT (''),
	[sim_name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_sim_queue_sim_name]  DEFAULT (''),
 CONSTRAINT [PK_sim_queue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF