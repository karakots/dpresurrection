INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,4,0, 'extrinsic attributes and locking')
GO

alter table product_attribute add [type] int NOT NULL DEFAULT(0)
GO

alter table product_attribute 
add initial_awareness float NOT NULL DEFAULT(0)
GO

alter table Model_infoadd locked_by varchar(100) NOT NULL DEFAULT('')
GO

alter table Model_info
add locked_on [datetime] NOT NULL DEFAULT(getdate())
GO

alter table simulationadd locked_by varchar(100) NOT NULL DEFAULT('')
GO

alter table simulation
add locked_on [datetime] NOT NULL DEFAULT(getdate())
GO

alter table project
add locked_by varchar(100) NOT NULL DEFAULT('')
GO

alter table project
add locked_on [datetime] NOT NULL DEFAULT(getdate())
GO