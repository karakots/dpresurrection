INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,2,8, 'attribute price sensitivity')

ALTER TABLE consumer_preference
ADD [price_sensitivity] [float] NOT NULL DEFAULT(0)