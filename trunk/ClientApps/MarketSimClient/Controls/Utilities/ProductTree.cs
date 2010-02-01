using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;

using MarketSimUtilities.ProductHierarchy;

namespace MarketSimUtilities
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

        private PCSModel theDb;

        public ProductTree() {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            this.CheckBoxes = true;

            this.AfterCheck += new TreeViewEventHandler( ProductTree_AfterCheck );
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) {
            if( disposing ) {
                if( components != null ) {
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
        private void InitializeComponent() {
            // 
            // ProductTree
            // 
            this.Size = new System.Drawing.Size( 248, 208 );

        }
        #endregion


        public delegate void SelectedItems( ArrayList selList );
        public event SelectedItems SelectedItemsChanged;

        public PCSModel Db {
            set {
                theDb = value;
                this.Nodes.Clear();
                productList.Clear();
                TopProductNode topNode = new TopProductNode( value );
                this.Nodes.Add( topNode );
                topNode.Initialize();
                topNode.Checked = true;
                topNode.Expand();
            }
        }

        public void Rebuild() {
            this.Nodes.Clear();
            productList.Clear();
            TopProductNode topNode = new TopProductNode( theDb );
            this.Nodes.Add( topNode );
            topNode.Initialize();
            topNode.Checked = true;
            topNode.Expand();
        }

        private ArrayList productList = new ArrayList();
        public ArrayList CheckedProducts {
            get {
                return productList;
            }
        }

        public ProductForest getProductForest() {
            // nothing selected
            if( productList.Count == 0 ) {
                return null;
            }

            // we compute totals is the toplevel node is checked
            return new ProductForest( theDb, productList );
        }

        public void SelectByType( string type_name ) {
            string query = "type_name = '" + type_name + "'";
            DataRow[] rows = theDb.Data.product_type.Select( query, "", DataViewRowState.CurrentRows );
            int type = Convert.ToInt32( rows[ 0 ][ "id" ].ToString() );
            TreeNode top = this.Nodes[ 0 ];
            top.Checked = false;
            top.ExpandAll();
            foreach( TreeNode node in top.Nodes ) {
                SelectByType( type, node );
            }
        }

        public void UnSelectAll() {
            foreach( TopProductNode node in this.Nodes ) {
                node.Checked = false;

                node.UnSelectAll();
            }
        }

        public void SelectByProductID( int product_id ) {
            foreach( TreeNode node in this.Nodes ) {
                if( selectByProductId( product_id, node ) )
                    return;
            }
        }

        private bool selectByProductId( int product_id, TreeNode aNode ) {
            MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow)aNode.Tag;

            if( row != null && (int)row[ "product_id" ] == product_id ) {
                aNode.Checked = true;
                return true;
            }

            foreach( TreeNode node in aNode.Nodes ) {
                if( selectByProductId( product_id, node ) )
                    return true;
            }

            return false;
        }

        private bool SelectByType( int type, TreeNode node ) {
            MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow)node.Tag;
            if( row.product_type == type ) {
                node.Checked = true;
                node.Collapse();
                return true;
            }
            else {
                bool rval = false;

                foreach( TreeNode myNode in node.Nodes ) {
                    if( SelectByType( type, myNode ) ) {
                        rval = true;
                    }
                }

                if( !rval ) {
                    node.Collapse();
                }

                return rval;
            }
        }

        /// <summary>
        /// Handles a change in the checked state of an item by checking all subnodes.   Calls SelectedItemsChanged( productList ) after all nodes have been adjusted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ProductTree_AfterCheck( object sender, TreeViewEventArgs args ) {
            //Console.WriteLine( "ProductTree_AfterCheck()..." );

            this.AfterCheck -= new TreeViewEventHandler( ProductTree_AfterCheck );
            CheckWithSubnodes( args.Node, args.Node.Checked );
            this.AfterCheck += new TreeViewEventHandler( ProductTree_AfterCheck );

            if( SelectedItemsChanged != null ) {
                SelectedItemsChanged( productList );
            }
        }

        /// <summary>
        /// Sets or clears the checked state of a tree node and all of its subnodes.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doCheck"></param>
        private void CheckWithSubnodes( TreeNode node, bool doCheck ) {
            node.Checked = doCheck;
            ProcessCheckChanged( node );

            // recursively handle subnodes
            if( node.Nodes.Count > 0 ) {
                foreach( TreeNode subnode in node.Nodes ) {
                    CheckWithSubnodes( subnode, doCheck );
                }
            }
        }

        /// <summary>
        /// Adds or removes the item from the productList that corresponds to the given node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doCheck"></param>
        private void ProcessCheckChanged( TreeNode node ) {
            if( node.Tag != null ) {
                if( node.Checked ) {
                    if( productList.Contains( node.Tag ) == false ) {
                        productList.Add( node.Tag );
                    }
                }
                else {    // node is not checked
                    productList.Remove( node.Tag );
                }
            }
        }
    }

	public class ProductForest
	{

		private System.Collections.ArrayList myForest;
		private PCSModel theDb;

		public ProductForest(PCSModel db, ArrayList productList)
		{
			theDb = db;

			myForest = new ArrayList();
		
			createForest(productList);
		}


		/// <summary>
		/// Returns all my ancestors above me
		/// includes the ALL product
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		private ArrayList getAncestors(MrktSimDBSchema.productRow prod)
		{
			ArrayList rval = new ArrayList();

			MrktSimDBSchema.productRow parent = this.getParent(prod);
			
			while(parent.product_id != Database.AllID)
			{
				rval.Add(parent);
				parent = getParent(parent);
			}

			// add ALL product
			rval.Add(parent);

			return rval;
		}

		/// <summary>
		/// Compares two products
		/// if left is ancestor of right returns true
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		private bool isAncestor(MrktSimDBSchema.productRow parent, MrktSimDBSchema.productRow child)
		{
			// I am not my own parent
			if (parent == child)
				return false;

			// all product is parent of all
			if (parent.product_id == Database.AllID)
				return true;

			MrktSimDBSchema.productRow myParent = this.getParent(child);

            while (myParent.product_id != Database.AllID)
			{
				if (myParent == parent)
					return true;

				myParent = getParent(myParent);
			}

			return false;
		}

		private bool isTopParent(MrktSimDBSchema.productRow prod, ArrayList productList)
		{
            if (prod.product_id == Database.AllID)
			{
				return true;
			}

			// I am a top parent if no parent of mine is in list
			ArrayList myAncestors = getAncestors(prod);

			// if any ancestor is in the list then I am not a top parent
			foreach(MrktSimDBSchema.productRow ancestor in myAncestors)
			{
				if (productList.IndexOf(ancestor) >= 0)
				{
					return false;
				}
			}
			
			return true;
		}

		private void getTopParents(ArrayList productList, out ArrayList topParents, out ArrayList children)
		{
			topParents = new ArrayList();
			children = new ArrayList();

			// check if productList contains the all product
			foreach(MrktSimDBSchema.productRow row in productList)
			{
				if (isTopParent(row, productList))
				{
					topParents.Add(row);		
				}
				else
				{
					children.Add(row);
				}
			}
		}

		private void createForest(ArrayList productList)
		{
			// clear cut forest
			myForest.Clear();

			// first create the forest

			ArrayList topParents = null;
			ArrayList children = null;
			
			getTopParents(productList, out topParents, out children);

			foreach(MrktSimDBSchema.productRow parent in topParents)
			{
				ArrayList prodTree = new ArrayList();
				prodTree.Add(parent);
				myForest.Add(prodTree);
			}

			// add trees to forest
			foreach (MrktSimDBSchema.productRow child in children)
			{
				foreach(ArrayList prodTree in myForest)
				{
					MrktSimDBSchema.productRow topParent = (MrktSimDBSchema.productRow) prodTree[0];

					if (isAncestor(topParent, child))
					{
						prodTree.Add(child);
					}
				}
			}
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

		private MrktSimDBSchema.productRow getParent(MrktSimDBSchema.productRow row)
		{
			string query = "child_id = " + row.product_id;

			DataRow[] parent = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
			if(parent.Length < 1)
			{
                return theDb.Data.product.FindByproduct_id(Database.AllID);
			}

			int parentID = (int) parent[0]["parent_id"];

			// query = "product_id = " + parent[0]["parent_id"].ToString();
			return theDb.Data.product.FindByproduct_id(parentID);
		}


		#region public methods

		public ArrayList Trees
		{
			get
			{
				return this.myForest;
			}
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

		/// <summary>
		/// combines tree from one forest with tree in other forest
		/// does not check anything
		/// </summary>
		/// <param name="other"></param>
		public void CombineForest(ProductForest other)
		{
			foreach(ArrayList prodTree in other.Trees)
			{
				myForest.Add(prodTree);
			}
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

		#endregion
	}

	
}
