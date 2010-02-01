INSERT db_schema_info (major_no, minor_no, release_no, comments)
VALUES (2,1,0, 'Add product tree table, create trees from existing brands')
GO

CREATE TABLE [product_tree]
(
	[model_id] [int] NOT NULL ,
	[parent_id] [int] NOT NULL,
	[child_id] [int] NOT NULL,
	CONSTRAINT ModelProductTree FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [product_type]
(
	[model_id] [int] NOT NULL ,
	[id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[type_name] [varchar] (25) NOT NULL,
	CONSTRAINT ModelProductType FOREIGN KEY (model_id)
	REFERENCES Model_info (model_id)
	ON DELETE CASCADE
) ON [PRIMARY]
GO

INSERT INTO product
SELECT model_id, brand_name as product_name, -brand_id as brand_id, 'none' as product_base_name,
'none' as type, 'none' as product_group, 'none' as related_group, 'none' as percent_relation,
0.0 as cost, 0.0 as initial_dislike_probability, 0.0 as repeat_like_probability, 'none' as color
FROM brand
WHERE (SELECT COUNT(child.brand_id) FROM product as child WHERE brand.brand_id = child.brand_id) > 1
GO

ALTER TABLE Product
ADD [product_type] [int] NOT NULL DEFAULT -1
GO

INSERT INTO product_tree
SELECT parent.model_id as model_id, parent.product_id as parent_id, child.product_id as child_id 
FROM product as parent, product as child 
WHERE parent.brand_id = -child.brand_id 
AND parent.model_id = child.model_id 
AND parent.brand_id < 0
GO

INSERT INTO product_type
SELECT model_id, 'Brand' as type_name FROM Model_info
GO

INSERT INTO product_type
SELECT model_id, 'Product' as type_name FROM Model_info
GO

UPDATE product
SET product_type = (SELECT product_type.id FROM product_type 
WHERE product_type.model_id = product.model_id AND product_type.type_name = 'Product')
WHERE brand_id > 0
GO

UPDATE product
SET product_type = (SELECT product_type.id FROM product_type 
WHERE product_type.model_id = product.model_id AND product_type.type_name = 'Brand')
WHERE brand_id < 0
GO

UPDATE product SET brand_id = 1

UPDATE product SET brand_id = 0 WHERE product_id IN (SELECT product_id FROM product, product_tree WHERE product_id = parent_id)

