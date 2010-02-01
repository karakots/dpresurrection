
using System;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
namespace MarketSimUtilities.ProductHierarchy
{
	public abstract class ProductHierarchy : TreeNode
	{
		public ProductHierarchy(PCSModel db)
		{
			theDb = db;
		}

		protected PCSModel theDb;

		public abstract void Initialize();

		// public abstract int[] productsIDs();
	}

	/// <summary>
	/// Under the new model, everything is a product, must construct forest from product_tree table
	/// </summary>
	public class TopProductNode : ProductHierarchy
	{
		public TopProductNode(PCSModel db) : base(db)
		{
			this.Text = "Category";
			this.Tag = db.Data.product.FindByproduct_id(Database.AllID);
		}

		public override void Initialize()
		{	
			Nodes.Clear();

			// this is the top level brand node
			foreach(MrktSimDBSchema.productRow row in theDb.Data.product.Select("","",DataViewRowState.CurrentRows))
			{
				// skip the ALL brand
				if (row.product_id == Database.AllID)
					continue;

				if( isRoot(row) )
				{
					ProductNode aRootNode = new ProductNode(theDb, row);
				
					this.Nodes.Add(aRootNode);

					aRootNode.Initialize();
				}

			}
		}

		private bool isRoot(MrktSimDBSchema.productRow row)
		{
			string query = "child_id = " + row.product_id;
			return !(theDb.Data.product_tree.Select(query,"", DataViewRowState.CurrentRows).Length > 0);
		}

		public void UnSelectAll()
		{
			foreach(ProductNode node in Nodes)
			{
				node.Checked = false;

				node.UnSelectAll();
			}
		}
	}

	/*public class BrandNode : ProductHierarchy
	{
		public BrandNode(ModelDb db, MrktSimDBSchema.brandRow row) : base(db)
		{
			Text = row.brand_name;
			Tag = row;
		}

		public override void Initialize()
		{
			MrktSimDBSchema.brandRow brand = (MrktSimDBSchema.brandRow) Tag;

			// add the products to this node
			foreach(MrktSimDBSchema.productRow row in theDb.Data.product.Select("brand_id = " + brand.brand_id))
			{
				if (row.product_id == ModelDb.AllID)
					continue;

				ProductNode aProductNode = new ProductNode(theDb, row);

				this.Nodes.Add(aProductNode);

				aProductNode.Initialize();
			}
		}
	}*/

	public class ProductNode : ProductHierarchy
	{
		public ProductNode(PCSModel db, MrktSimDBSchema.productRow row) : base(db)
		{
			Text = row.product_name;
			Tag = row;
		}

		public ProductNode(PCSModel db, System.Data.DataRow data) : base(db)
		{
			MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow) data;
			Text = row.product_name;
			Tag = row;
		}

		public override void Initialize()
		{
			Nodes.Clear();

			addChildren();
		}

		public void UnSelectAll()
		{
			foreach(ProductNode node in Nodes)
			{
				node.Checked = false;

				node.UnSelectAll();
			}
		}

		private void addChildren()
		{
			MrktSimDBSchema.productRow myData = (MrktSimDBSchema.productRow) this.Tag;
			string query = "parent_id = " + myData.product_id;
			System.Data.DataRow[] children = theDb.Data.product_tree.Select(query,"", DataViewRowState.CurrentRows);
			foreach( MrktSimDBSchema.product_treeRow row in children)
			{
				query = "product_id = " + row.child_id;
				System.Data.DataRow[] product = theDb.Data.product.Select(query,"", DataViewRowState.CurrentRows);

				ProductNode aProductNode = new ProductNode(theDb, product[0]);
				
				this.Nodes.Add(aProductNode);

				aProductNode.Initialize();

			}
		}
	}
}
