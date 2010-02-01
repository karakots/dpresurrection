using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.ProductHierarchy;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for ProductTree.
	/// </summary>
	public class ProductTree : System.Windows.Forms.TreeView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Database theDb;

		public ProductTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.CheckBoxes = true;

			this.AfterCheck +=new TreeViewEventHandler(ProductTree_AfterCheck);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ProductTree
			// 
			this.Size = new System.Drawing.Size(248, 208);

		}
		#endregion


		public delegate void SelectedItems(ArrayList selList);
		public event SelectedItems SelectedItemsChanged;

		public Database Db
		{
			set
			{
				theDb = value;
				this.Nodes.Clear();
				productList.Clear();
				TopProductNode topNode = new TopProductNode(value);
				this.Nodes.Add(topNode);
				topNode.Initialize();
				topNode.Checked = true;
				topNode.Expand();
			}
		}

		public void Rebuild()
		{
			this.Nodes.Clear();
			productList.Clear();
			TopProductNode topNode = new TopProductNode(theDb);
			this.Nodes.Add(topNode);
			topNode.Initialize();
			topNode.Checked = true;
			topNode.Expand();
		}

		private ArrayList productList = new ArrayList();
		public ArrayList CheckedProducts
		{
			get
			{
				return productList;
			}
		}

		public ProductForest getProductForest()
		{
			if(productList.Count == 0)
			{
				return null;
			}
			if(productList.Count == 1)
			{
				if(this.Nodes[0].Checked)
				{
					return null;
				}
			}
			if(this.Nodes[0].Checked)
			{
				return new ProductForest(theDb, productList, true);
			}
			else
			{
				return new ProductForest(theDb, productList, false);
			}
		}

		public void SelectByType(string type_name)
		{
			string query = "type_name = '" + type_name + "'";
			DataRow[] rows = theDb.Data.product_type.Select(query,"",DataViewRowState.CurrentRows);
			int type = Convert.ToInt32(rows[0]["id"].ToString());
			TreeNode top = this.Nodes[0];
			top.Checked = false;
			top.ExpandAll();
			foreach(TreeNode node in top.Nodes)
			{
				SelectByType(type, node);
			}
		}

		public void UnSelectAll()
		{
			foreach(TreeNode node in this.Nodes)
			{
				node.Checked = false;
			}
		}

		public void SelectByProductID(int product_id)
		{
			foreach(TreeNode node in this.Nodes)
			{
				if ( selectByProductId(product_id, node))
					return;
			}
		}

		private bool selectByProductId(int product_id, TreeNode aNode)
		{
			MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow) aNode.Tag;

			if (row != null && (int) row["product_id"] == product_id)
			{
				aNode.Checked = true;
				return true;
			}

			foreach(TreeNode node in aNode.Nodes)
			{
				if (selectByProductId(product_id, node))
					return true;
			}

			return false;
		}

		private bool SelectByType(int type, TreeNode node)
		{
			MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow) node.Tag;
			if(row.product_type == type)
			{
				node.Checked = true;
				node.Collapse();
				return true;
			}
			else
			{
				bool rval = false;

				foreach(TreeNode myNode in node.Nodes)
				{
					if(SelectByType(type, myNode))
					{
						rval = true;
					}
				}

				if(!rval)
				{
					node.Collapse();
				}

				return rval;
			}
		}

		// cascade down
		private void ProductTree_AfterCheck(object sender, TreeViewEventArgs args)
		{
			if (args.Node.Nodes.Count == 0)
			{
				if(args.Node.Checked)
				{
					// add
					if( productList.IndexOf(args.Node.Tag) < 0)
					{
						if(args.Node.Tag != null)
						{
							productList.Add(args.Node.Tag);
						}
					}
				}
				else
				{
					// remove
					productList.Remove(args.Node.Tag);
				}
			}
			else
			{
				// make sure all sub nodes are checked as well
				foreach( TreeNode node in args.Node.Nodes)
				{
					node.Checked = args.Node.Checked;

					if (node.Nodes.Count == 0)
					{
						if(node.Checked)
						{
							// add
							if( productList.IndexOf(node.Tag) < 0)
							{
								if(node.Tag != null)
								{
									productList.Add(node.Tag);
								}
							}
						}
						else
						{
							// remove
							productList.Remove(node.Tag);
						}
					}
				}

				//Now do myself
				if(args.Node.Checked && args.Node.Tag != null)
				{
					// add
					if( productList.IndexOf(args.Node.Tag) < 0)
					{
						productList.Add(args.Node.Tag);
					}
				}
				else if(args.Node.Tag != null)
				{
					// remove
					productList.Remove(args.Node.Tag);
				}
			}

			if (SelectedItemsChanged != null)
				SelectedItemsChanged(productList);
		}
	}

	public class ProductForest
	{

		private System.Collections.ArrayList myForest;
		private Database theDb;

		public ProductForest(Database db, ArrayList productList, bool createTotalTree)
		{
			theDb = db;
			

			// I hate special cases....grrrrrrrr Isaac 4-3-2006
			if(!createTotalTree)
			{
				createForest(productList);
			}
			else
			{
				createTotalsTree(productList);
			}
		}

		private void createForest(ArrayList productList)
		{
			myForest = new ArrayList();
			MrktSimDBSchema.productRow parent;
			string env_var = System.Environment.GetEnvironmentVariable("MARKETSIM_ALL_PRODUCT_RESULTS");
			bool allProds = false;
			if(env_var == "TRUE")
			{
				allProds = true;
			}
			foreach(MrktSimDBSchema.productRow row in productList)
			{
				if(isLeaf(row) || allProds)
				{
					parent = getTopParent(row, productList);
					bool found = false;
					foreach(ArrayList prodTree in myForest)
					{
						if(listContains(parent, prodTree))
						{
							prodTree.Add(row);
							found = true;
						}
					}
					if(!found)
					{
						ArrayList prodTree = new ArrayList();
						prodTree.Add(parent);
						prodTree.Add(parent);
						prodTree.Add(row);
						myForest.Add(prodTree);
					}
				}
			}
		}

		private void createTotalsTree(ArrayList productList)
		{
			myForest = new ArrayList();
			MrktSimDBSchema.productRow row = theDb.Data.product.NewproductRow();
			row.product_name = "Total";
			row.product_id = Database.AllID;
			ArrayList totalList = new ArrayList(productList.Count + 1);
			totalList.Add(row);
			foreach(MrktSimDBSchema.productRow child in productList)
			{
				totalList.Add(child);
			}
			myForest.Add(totalList);
		}

		private bool listContains(MrktSimDBSchema.productRow row, ArrayList prodList)
		{
			if(row == null)
			{
				return false;
			}
			foreach(MrktSimDBSchema.productRow aRow in prodList)
			{
				if(aRow.product_id == row.product_id)
				{
					return true;
				}
			}

			return false;
		}

		private bool listContains(int product_id, ArrayList prodList)
		{
			foreach(MrktSimDBSchema.productRow aRow in prodList)
			{
				if(aRow.product_id == product_id)
				{
					return true;
				}
			}

			return false;
		}

		private bool isLeaf(MrktSimDBSchema.productRow row)
		{
			string query = "parent_id = " + row.product_id;
			DataRow[] children = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
			if(children.Length > 0)
			{
				return false;
			}
			return true;
		}

		private MrktSimDBSchema.productRow getTopParent(MrktSimDBSchema.productRow row, ArrayList productList)
		{
			MrktSimDBSchema.productRow parent;
			MrktSimDBSchema.productRow temp;

			parent = row;
			temp = getParent(row);
			while(temp != null && listContains(temp, productList))
			{
				parent = temp;
				temp = getParent(parent);
			}

			return parent;
		}

		private MrktSimDBSchema.productRow getParent(MrktSimDBSchema.productRow row)
		{
			string query = "child_id = " + row.product_id;
			DataRow[] parent = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
			if(parent.Length < 1)
			{
				return null;
			}
			query = "product_id = " + parent[0]["parent_id"].ToString();
			return (MrktSimDBSchema.productRow)theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows)[0];
		}

		public ArrayList getParents()
		{
			ArrayList parents = new ArrayList(myForest.Count);
			foreach(ArrayList list in myForest)
			{
				parents.Add(list[0]);
			}
			return parents;
		}

		public MrktSimDBSchema.productRow ParentOf(MrktSimDBSchema.productRow row)
		{
			foreach(ArrayList sumList in myForest)
			{
				if(listContains(row, sumList))
				{
					return (MrktSimDBSchema.productRow)sumList[0];
				}
			}

			return null;
		}
	
		public ArrayList ChildrenOf(MrktSimDBSchema.productRow row)
		{
			foreach(ArrayList sumList in myForest)
			{
				if(listContains(row, sumList))
				{
					return sumList.GetRange(1,sumList.Count - 1);
				}
			}

			return null;
		}

		public ArrayList ChildrenOf(int product_id)
		{
			foreach(ArrayList sumList in myForest)
			{
				if(listContains(product_id, sumList))
				{
					return sumList.GetRange(1,sumList.Count - 1);
				}
			}
			return null;
		}
	}

	
}
