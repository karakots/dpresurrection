using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;
using Common.Dialogs;

using ExcelInterface;
using ErrorInterface;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Displays Product Data.
	/// </summary>
	public class ProductGridControl : MrktSimControl
	{
		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh();

			this.productTree.Rebuild();

			iProductGrid.Refresh();
		}
		
		public override void Flush()
		{
			iProductGrid.Flush();
			//brandGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;

				iProductGrid.Suspend = value;
				//brandGrid.Suspend = value;
			}
		}

		//public event EventHandler BrandTreeChanged;
        // public event EventHandler BrandNameChanged;
		//private System.Windows.Forms.GroupBox brandGroupBox;
		//private System.Windows.Forms.TextBox brandNameBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		//private System.Windows.Forms.Button createBrandButton;
		private System.Windows.Forms.NumericUpDown numProductUpDown;
        private MrktSimGrid iProductGrid;
		//private System.Windows.Forms.ComboBox selBrandBox;
		private System.Data.DataView brandView;
        private ProductTree productTree;
        private System.Windows.Forms.ContextMenu ProductTreeContextMenu;
        private SplitContainer splitContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton importBut;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem moveToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem deleteTypeToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripButton cleanTreeBut;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductGridControl(ModelDb db) : base(db)
		{

			//This code is not currently being used as the context menu is non-operational
			/*DataRow[] Types = db.Data.product_type.Select("","",DataViewRowState.CurrentRows);

			Product_Types = new System.Windows.Forms.MenuItem[Types.Length];
			
			int i = 0;
			foreach(MrktSimDBSchema.product_typeRow row in Types)
			{
				System.Windows.Forms.MenuItem menuItem = new MenuItem(row.type_name);
				Product_Types[i] = menuItem;
				i++;
			}*/


			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// wait until user enters text
			//createBrandButton.Enabled = false;
			//createProdButton.Enabled = false;

			iProductGrid.Table = theDb.Data.product;
			iProductGrid.RowFilter = "product_id <> " + ModelDb.AllID;

			// turns on parameters
			iProductGrid.RowID = "product_id";
			iProductGrid.RowName = "product_name";
			iProductGrid.Db = db;
			iProductGrid.AllowDelete = false;

			iProductGrid.Table.RowChanged += new DataRowChangeEventHandler(productGrid_rowChanged);

			this.productTree.Db = theDb;
			productTree.SelectedItemsChanged +=new ProductTree.SelectedItems(productTree_SelectedItemsChanged);

			/*brandGrid.Table = theDb.Data.brand;
			brandGrid.RowFilter = "brand_id <> " + ModelDb.AllID;
			brandGrid.DescriptionWindow = false;
			// brandGrid.RowHeaders = false;

	
			brandView.Table = theDb.Data.brand;
			brandView.RowFilter = "brand_id <> " + ModelDb.AllID;*/
	
			/*selBrandBox.DisplayMember = "brand_name";
			selBrandBox.ValueMember = "brand_id";

			if (selBrandBox.SelectedValue == null)
				prodNameBox.Enabled = false;*/

			// turn off CLEAN button of not devl model
			if (MrktSimControl.MrktSimMessage("devl_version") != "true")
			{
                this.cleanTreeBut.Visible = false;
			}

			if(ProjectDb.Nimo)
			{
                deleteTypeToolStripMenuItem.Visible = false;
			}

			DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);
			if(types.Length == 0)
			{
                this.deleteTypeToolStripMenuItem.Enabled = false;
			}
			else
			{
                deleteTypeToolStripMenuItem.Enabled = true;
			}

			createTableStyle();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ProductGridControl ) );
            this.productTree = new MarketSimUtilities.ProductTree();
            this.ProductTreeContextMenu = new System.Windows.Forms.ContextMenu();
            this.brandView = new System.Data.DataView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numProductUpDown = new System.Windows.Forms.NumericUpDown();
            this.iProductGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.importBut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanTreeBut = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.brandView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProductUpDown)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // productTree
            // 
            this.productTree.CheckBoxes = true;
            this.productTree.ContextMenu = this.ProductTreeContextMenu;
            this.productTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productTree.Location = new System.Drawing.Point( 0, 0 );
            this.productTree.Name = "productTree";
            this.productTree.Size = new System.Drawing.Size( 207, 386 );
            this.productTree.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 16, 24 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 100, 16 );
            this.label2.TabIndex = 3;
            this.label2.Text = "Brand Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 192, 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 104, 16 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of products";
            // 
            // numProductUpDown
            // 
            this.numProductUpDown.Location = new System.Drawing.Point( 200, 48 );
            this.numProductUpDown.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numProductUpDown.Name = "numProductUpDown";
            this.numProductUpDown.Size = new System.Drawing.Size( 48, 20 );
            this.numProductUpDown.TabIndex = 1;
            this.numProductUpDown.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // iProductGrid
            // 
            this.iProductGrid.DescribeRow = null;
            this.iProductGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iProductGrid.EnabledGrid = true;
            this.iProductGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iProductGrid.Name = "iProductGrid";
            this.iProductGrid.RowFilter = "";
            this.iProductGrid.RowID = null;
            this.iProductGrid.RowName = null;
            this.iProductGrid.Size = new System.Drawing.Size( 412, 386 );
            this.iProductGrid.Sort = "";
            this.iProductGrid.TabIndex = 0;
            this.iProductGrid.Table = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 25 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.productTree );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.iProductGrid );
            this.splitContainer1.Size = new System.Drawing.Size( 623, 386 );
            this.splitContainer1.SplitterDistance = 207;
            this.splitContainer1.TabIndex = 30;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importBut,
            this.toolStripSeparator1,
            this.toolStripDropDownButton1,
            this.cleanTreeBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 623, 25 );
            this.toolStrip1.TabIndex = 31;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // importBut
            // 
            this.importBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importBut.Image = ((System.Drawing.Image)(resources.GetObject( "importBut.Image" )));
            this.importBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importBut.Name = "importBut";
            this.importBut.Size = new System.Drawing.Size( 49, 22 );
            this.importBut.Text = "Import...";
            this.importBut.Click += new System.EventHandler( this.toolStripButton1_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.moveToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.deleteTypeToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.removeToolStripMenuItem} );
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripDropDownButton1.Image" )));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size( 63, 22 );
            this.toolStripDropDownButton1.Text = "Tree Edit";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler( this.NewNode_Click );
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.moveToolStripMenuItem.Text = "Move...";
            this.moveToolStripMenuItem.Click += new System.EventHandler( this.MoveNodeButton_Click );
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler( this.DeleteNodeButton_Click );
            // 
            // deleteTypeToolStripMenuItem
            // 
            this.deleteTypeToolStripMenuItem.Name = "deleteTypeToolStripMenuItem";
            this.deleteTypeToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.deleteTypeToolStripMenuItem.Text = "Delete Type...";
            this.deleteTypeToolStripMenuItem.Click += new System.EventHandler( this.DeleteType_Click );
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler( this.exportButton_Click );
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.removeToolStripMenuItem.Text = "Remove...";
            this.removeToolStripMenuItem.Click += new System.EventHandler( this.removeButton_Click );
            // 
            // cleanTreeBut
            // 
            this.cleanTreeBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cleanTreeBut.Image = ((System.Drawing.Image)(resources.GetObject( "cleanTreeBut.Image" )));
            this.cleanTreeBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cleanTreeBut.Name = "cleanTreeBut";
            this.cleanTreeBut.Size = new System.Drawing.Size( 41, 22 );
            this.cleanTreeBut.Text = "Clean!";
            this.cleanTreeBut.Click += new System.EventHandler( this.CleanButton_Click );
            // 
            // ProductGridControl
            // 
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "ProductGridControl";
            this.Size = new System.Drawing.Size( 623, 411 );
            ((System.ComponentModel.ISupportInitialize)(this.brandView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProductUpDown)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void createTableStyle()
		{
			iProductGrid.Clear();

			//iProductGrid.AddTextColumn("brandName", true);
			iProductGrid.AddTextColumn("product_name", "Name", false);
			//iProductGrid.AddTextColumn("type_name", true);
			if(ProjectDb.Nimo)
			{
				iProductGrid.AddComboBoxColumn("product_type",theDb.Data.product_type,"type_name","id",true);
			}
			else
			{
				iProductGrid.AddComboBoxColumn("product_type",theDb.Data.product_type,"type_name","id",false);
			}
			

			iProductGrid.Reset();
		}


		private void productGrid_rowChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (this.Suspend)
				return;

			this.Suspend = true;
			iProductGrid.Suspend = true;

			this.productTree.Rebuild();

			iProductGrid.Suspend = false;
			this.Refresh();

			this.Suspend = false;
		}

		private void ExcelImport()
		{
			ProductReader productReader = new ProductReader(theDb);

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;

			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				this.Suspend = true;

				string fileName = openFileDlg.FileName;

				ErrorList errors = new ErrorList();

				DataTable table;
				errors.addErrors(ExcelReader.ReadTable(fileName,"Products","Z", true, out table));

				if (table == null)
				{
					errors.addError(null, "No values found", "Product cannot have empty names");
				}
				else if(errors.Count > 0)
				{
					errors.addError(null, "Object not found", "Could not find a table named: Products");
				}
				else
				{
					errors.addErrors(productReader.CreateProducts(table));
				}

				DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);
				if(types.Length == 0)
				{

                    deleteTypeToolStripMenuItem.Enabled = false;
				}
				else
				{
                    deleteTypeToolStripMenuItem.Enabled = true;
				}


				this.Suspend = false;

				this.Refresh();

				
				errors.Display();
			}	

		
		}

        public void RemoveExcelProductsList() {
            ProductReader productReader = new ProductReader( theDb );

            System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.DefaultExt = ".xls";
            openFileDlg.Filter = "Excel File (*.xls)|*.xls";

            // openFileDlg.ReadOnlyChecked = false;

            DialogResult frslt = openFileDlg.ShowDialog();

            if( frslt == DialogResult.OK ) {
                this.Suspend = true;

                string fileName = openFileDlg.FileName;

                ErrorList errors = new ErrorList();

                DataTable table;
                errors.addErrors( ExcelReader.ReadTable( fileName, "Removed Products", "Z", true, out table ) );

                if( table == null ) {
                    errors.addError( null, "No values found", "Product-remove list cannot be empty" );
                }
                else if( errors.Count > 0 ) {
                    errors.addError( null, "Object not found", "Could not find a table named: Products" );
                }
                else {
                    errors.addErrors( productReader.RemoveProducts( table ) );
                }

                 this.Suspend = false;

                this.Refresh();

                errors.Display();
            }
        }


		private void productTree_SelectedItemsChanged(ArrayList prodList)
		{
			if (Suspend)
				return;

			if(prodList.Count == 0)
			{
				iProductGrid.RowFilter = "product_id = 0 AND product_id = 1";
				return;
			}

			string query = "product_id <> " + ModelDb.AllID;

			bool firstTime = true;
			foreach(DataRow row in prodList)
			{
				if (firstTime)
				{
					query += " AND (product_id = " + row["product_id"].ToString();
					firstTime = false;
				}
				else
				{

					query +=  " OR product_id = " + row["product_id"].ToString();
				}
			}

			if (prodList.Count > 0)
			{
				query += " )";
			}

			iProductGrid.RowFilter = query;
		}

		private void NewNode_Click(object sender, System.EventArgs e)
		{
			DataRow current = null;

			if (productTree.SelectedNode != null)
			{
				current = (DataRow) productTree.SelectedNode.Tag;
			}

			if (current != null)
			{
				// check that we are not processing the top level node
				if ((int) current["product_id"] == ModelDb.AllID)
				{
					current = null;
				}
			}

			if(ProjectDb.Nimo && current != null)
			{
				DataRow[] prods = theDb.Data.product_type.Select("type_name = 'Product'","",DataViewRowState.CurrentRows);

				if (prods.Length > 0 )
				{
					DataRow product = prods[0];
					if((int)current["product_type"] == (int)product["id"])
					{
						MessageBox.Show("Error: NIMO cannot have children of products","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
						return;
					}
				}
			}

			CreateProductDialog dlg = new CreateProductDialog(theDb, current);

			DialogResult rslt = dlg.ShowDialog();

			this.Suspend = true;

			if (rslt == DialogResult.OK)
			{
				if(dlg.ProductPlacement.Equals("Child"))
				{
					theDb.CreateChildProduct((int)current["product_id"], dlg.ProdName, dlg.ProductType);
				}
				else if(dlg.ProductPlacement.Equals("Parent"))
				{
					theDb.CreateParentProduct((int)current["product_id"], dlg.ProdName, dlg.ProductType);
				}
				else
				{
					theDb.CreateRootProduct(dlg.ProdName, dlg.ProductType);
				}

				DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);
				if(types.Length == 0)
				{
                    deleteTypeToolStripMenuItem.Enabled = false;
				}
				else
				{
                    deleteTypeToolStripMenuItem.Enabled = true;
				}

				this.productTree.Rebuild();

				this.Parent.Refresh();

				this.Refresh();
			}

			this.Suspend = false;


		}

		private void DeleteNodeButton_Click(object sender, System.EventArgs e)
		{
			DataRow myself = null;

			if (productTree.SelectedNode != null)
			{
				myself = (DataRow) productTree.SelectedNode.Tag;
			}

			if (myself != null)
			{
				if ( (int) myself["product_id"] == ModelDb.AllID)
				{
					myself = null;
				}
			}

			if(myself == null)
			{
				MessageBox.Show("Error: Invalid Node Selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			DialogResult rslt =  MessageBox.Show("Warning: All Data Associated with this product will be deleted","Warning",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);

			if (rslt != DialogResult.OK)
			{
				return;
			}

			this.Suspend = true;

			int product_id = (int)myself["product_id"];

			string query = "parent_id = " + product_id;
			DataRow[] children = theDb.Data.product_tree.Select(query,"", DataViewRowState.CurrentRows);
			query = "child_id = " + product_id;
			DataRow[] parent = theDb.Data.product_tree.Select(query,"", DataViewRowState.CurrentRows);

			if(parent.Length > 0)
			{
			
				if(children.Length > 0)
				{
					int parent_id = (int)parent[0]["parent_id"];
			
					foreach( MrktSimDBSchema.product_treeRow child in children)
					{
						theDb.CreateProductTree(parent_id, child.child_id);
					}
				}
				else
				{
					int parent_id = (int)parent[0]["parent_id"];
					query = "parent_id = " + parent_id;
					children = theDb.Data.product_tree.Select(query,"", DataViewRowState.CurrentRows);
					if(children.Length < 2)
					{
						query = "product_id = " + parent_id;
						DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
						MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow) rows[0];
						row.brand_id = 1;
						theDb.CreateAuxForProduct(row);
					}
				}

			}
			else
			{
				if(ProjectDb.Nimo && children.Length > 0)
				{
					MessageBox.Show("Error: NIMO conditions violated","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Suspend = false;
					return;
				}
			}

			myself.Delete();

			this.productTree.Rebuild();

			this.Refresh();

			this.Suspend = false;
		}

		private void MoveNodeButton_Click(object sender, System.EventArgs e)
		{
			DataRow myself = null;

			if (productTree.SelectedNode != null)
			{
				myself = (DataRow) productTree.SelectedNode.Tag;
			}

			if (myself != null)
			{
				if ( (int) myself["product_id"] == ModelDb.AllID)
				{
					myself = null;
				}
			}

			if(myself == null)
			{
				MessageBox.Show("Error: Invalid Node Selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			string query = "child_id = " + myself["product_id"].ToString();

			DataRow[] parent_trow = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);


			string old_parent;
			if(parent_trow.Length > 0)
			{
				old_parent = theDb.Data.product.FindByproduct_id((int)parent_trow[0]["parent_id"]).product_name;
			}
			else
			{
				old_parent = "None";
			}
			
			MoveProductNodeDialog dlg = new MoveProductNodeDialog(theDb, old_parent);

			DialogResult rslt = dlg.ShowDialog();

			this.Suspend = true;

			if (rslt == DialogResult.OK)
			{
				//special NIMO conditions
				if(ProjectDb.Nimo)
				{
					bool error = false;
					MrktSimDBSchema.productRow myRow = theDb.Data.product.FindByproduct_id((int)myself["product_id"]);
					DataRow brand = theDb.Data.product_type.Select("type_name = 'Brand'","",DataViewRowState.CurrentRows)[0];
					DataRow product = theDb.Data.product_type.Select("type_name = 'Product'","",DataViewRowState.CurrentRows)[0];
					if(dlg.Product_Id != -1)
					{
						MrktSimDBSchema.productRow parentRow = theDb.Data.product.FindByproduct_id(dlg.Product_Id);
					
						if(myRow.product_type == (int)brand["id"])
						{
							error = true;
						}
						if(parentRow.product_type == (int)product["id"])
						{
							error = true;
						}
					}
					else
					{
						if(myRow.product_type == (int)product["id"])
						{
							error = true;
						}
					}
					if(error)
					{
						MessageBox.Show("Error: NIMO conditions violated","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Suspend = false;
						return;
					}
					
				}

				theDb.MoveProduct((int)myself["product_id"], dlg.Product_Id);

				this.productTree.Rebuild();

				this.Refresh();
			}

			this.Suspend = false;
		}

		private void CleanButton_Click(object sender, System.EventArgs e)
		{
			this.Suspend = true;

			theDb.CleanProductAux();

			this.productTree.Rebuild();

			this.Refresh();

			this.Suspend = false;
		}

		private void DeleteType_Click(object sender, System.EventArgs e)
		{
			DeleteProductTypeDialog dlg = new DeleteProductTypeDialog();
 
			dlg.Db = theDb;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string query = "product_type = " + dlg.TypeID.ToString();
				DataRow[] rows = theDb.Data.product.Select(query, "", DataViewRowState.CurrentRows);
				if(rows.Length > 0)
				{
					MessageBox.Show("Error: Cannot delete a type that still has products associated with it.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return;
				}
				this.Suspend = true;

				query = "id = " + dlg.TypeID.ToString();
				rows = theDb.Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
				rows[0].Delete();

				DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);
				if(types.Length == 0)
				{
                    deleteTypeToolStripMenuItem.Enabled = false;
				}
				else
				{
                    deleteTypeToolStripMenuItem.Enabled = true;
				}

				this.Suspend = false;

				this.Parent.Refresh();

				this.Refresh();
			}

		}

        private void exportButton_Click( object sender, EventArgs e ) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.Title = "Save Products";
            sfd.FileName = "Products.csv";
            DialogResult resp = sfd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            StreamWriter writer = new StreamWriter( sfd.OpenFile() );
            string header1 = String.Format( "\"MarketSim Products List for Model: {0}\"", theDb.Model.model_name );
            string header2 = "Products";
            writer.WriteLine( header1 );
            writer.WriteLine( header2 );

            MrktSimDBSchema.product_typeRow[] types = (MrktSimDBSchema.product_typeRow[])theDb.Data.product_type.Select( "", "", DataViewRowState.CurrentRows );
            string prodStr = "";
            for( int i = 1; i < types.Length; i++ ) {
                if( types[ i ].type_name.IndexOf( " " ) != -1 || types[ i ].type_name.IndexOf( "," ) != -1 ) {
                    prodStr += String.Format( "\"{0}\"", types[ i ].type_name );
                }
                else {
                    prodStr += types[ i ].type_name;
                }
                if( i != types.Length - 1 ) {
                    prodStr += ",";
                }
            }
            writer.WriteLine( prodStr );

            if( this.productTree.Nodes.Count != 0 ) {
                ExportProductSubdones( this.productTree.Nodes[ 0 ], writer, "" );    //we know there is only one root node
            }
            else {
                writer.WriteLine( "\"[This model has 0 Products]\"" );
            }
            writer.Flush();
            writer.Close();

            Common.Dialogs.CompletionDialog cdlg = new CompletionDialog( "Products exported successfully" );
            cdlg.ShowDialog();
        }

        private void ExportProductSubdones( TreeNode node, StreamWriter writer, string csvLine ) {

            for( int n = 0; n < node.Nodes.Count; n++ ) {
                TreeNode subnode = node.Nodes[ n ];
                string subCsvLine = csvLine;

                // build the csv line as we go
                if( subCsvLine.Length > 0 ) {
                    subCsvLine += ",";
                }
                // quote names if necesary
                if( subnode.Text.IndexOf( " " ) != -1 || subnode.Text.IndexOf( "," ) != -1 ) {
                    subCsvLine += String.Format( "\"{0}\"", subnode.Text );
                }
                else {
                    subCsvLine += subnode.Text;
                }

                if( subnode.Nodes.Count == 0 ) {
                    // this is a leaf node -- write the csv line
                    writer.WriteLine( subCsvLine );
                }
                else {
                    ExportProductSubdones( subnode, writer, subCsvLine );
                }
            }
        }

        private void removeButton_Click( object sender, EventArgs e ) {
            RemoveExcelProductsList();
        }

        private void toolStripButton1_Click( object sender, EventArgs e )
        {
            ExcelImport();
        }
    }
}
