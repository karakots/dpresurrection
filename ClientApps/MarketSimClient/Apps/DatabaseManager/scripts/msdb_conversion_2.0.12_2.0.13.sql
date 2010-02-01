INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,0,13, 'add market plan parms')
GO

ALTER TABLE [market_plan]
ADD	[parm1]	[float] NOT NULL Default(1)
GO

ALTER TABLE [market_plan]
ADD	[parm2]	[float] NOT NULL Default(1)
GO

ALTER TABLE [market_plan]
ADD	[parm3]	[float] NOT NULL Default(1)
GO

ALTER TABLE [market_plan]
ADD	[parm4]	[float] NOT NULL Default(1)
GO

ALTER TABLE [market_plan]
ADD	[parm5]	[float] NOT NULL Default(1)
GO

ALTER TABLE [market_plan]
ADD	[parm6]	[float] NOT NULL Default(1)
GO
