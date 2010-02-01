INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,5,0, 'Units Model')
GO

alter table Model_info add 
[season_freq_part] [float] NOT NULL DEFAULT ((85))
GO

alter table product add 
[pack_size_id] [int] NOT NULL DEFAULT ((-1))
GO

alter table segment add 
[min_freq] [float] NOT NULL DEFAULT ((0))
GO

alter table segment add 
[max_freq] [float] NOT NULL DEFAULT ((999999))
GO

CREATE TABLE [pack_size](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[model_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	CONSTRAINT [PK_pack_size] PRIMARY KEY CLUSTERED  ( [id] ASC )
	WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF),
	CONSTRAINT [FK_pack_size_Model_info] FOREIGN KEY([model_id])
	REFERENCES [Model_info] ([model_id])
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO

CREATE TABLE [pack_size_dist](
	[pack_size_id] [int] NOT NULL,
	[size] [int] NOT NULL DEFAULT ((0)),
	[dist] [float] NOT NULL DEFAULT ((0)),
	CONSTRAINT [FK_pack_size_dist_pack_size] FOREIGN KEY([pack_size_id])
	REFERENCES [pack_size] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO