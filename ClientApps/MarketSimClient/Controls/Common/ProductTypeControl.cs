using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Displays Product Data.
	/// </summary>
	public class ProductTypeGridControl : MrktSimControl
	{
		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh();

			iProductGrid.Refresh();
			//brandGrid.Refresh();
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

		private System.Windows.Forms.GroupBox productGroupBox;
		private System.Windows.Forms.GroupBox gridBox;
		//private System.Windows.Forms.GroupBox brandGroupBox;
		//private System.Windows.Forms.TextBox brandNameBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		//private System.Windows.Forms.Button createBrandButton;
		private System.Windows.Forms.NumericUpDown numProductUpDown;
		private MrktSimGrid iProductGrid;
		private System.Windows.Forms.Splitter splitter1;
		//private System.Windows.Forms.ComboBox selBrandBox;
		private System.Data.DataView brandView;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductTypeGridControl(ModelDb db, MrktSimDBSchema.product_typeRow row) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// wait until user enters text
			//createBrandButton.Enabled = false;
			//createProdButton.Enabled = false;

			iProductGrid.Table = theDb.Data.product;
			iProductGrid.RowFilter = "product_id <> " + ModelDb.AllID + " AND product_type = " + row.id;

            // check if any products of this type are leaf nodes
            MrktSimDBSchema.productRow[] prods = row.GetproductRows();

            foreach( MrktSimDBSchema.productRow prod in prods )
            {
                if( prod.brand_id == 1 )
                {
                    this.productDisplay = true;
                    break;
                }
            }

			// turns on parameters
			iProductGrid.RowID = "product_id";
			iProductGrid.RowName = "product_name";
			iProductGrid.Db = db;
			iProductGrid.AllowDelete = false;

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
			this.productGroupBox = new System.Windows.Forms.GroupBox();
			this.brandView = new System.Data.DataView();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.numProductUpDown = new System.Windows.Forms.NumericUpDown();
			this.gridBox = new System.Windows.Forms.GroupBox();
			this.iProductGrid = new MrktSimGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this.brandView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numProductUpDown)).BeginInit();
			this.gridBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// productGroupBox
			// 
			this.productGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.productGroupBox.Location = new System.Drawing.Point(0, 0);
			this.productGroupBox.Name = "productGroupBox";
			this.productGroupBox.Size = new System.Drawing.Size(608, 368);
			this.productGroupBox.TabIndex = 0;
			this.productGroupBox.TabStop = false;
			this.productGroupBox.Text = "Products";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Brand Name";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(192, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Number of products";
			// 
			// numProductUpDown
			// 
			this.numProductUpDown.Location = new System.Drawing.Point(200, 48);
			this.numProductUpDown.Minimum = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.numProductUpDown.Name = "numProductUpDown";
			this.numProductUpDown.Size = new System.Drawing.Size(48, 20);
			this.numProductUpDown.TabIndex = 1;
			this.numProductUpDown.Value = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			// 
			// gridBox
			// 
			this.gridBox.Controls.Add(this.iProductGrid);
			this.gridBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.gridBox.Location = new System.Drawing.Point(0, 0);
			this.gridBox.Name = "gridBox";
			this.gridBox.Size = new System.Drawing.Size(608, 368);
			this.gridBox.TabIndex = 1;
			this.gridBox.TabStop = false;
			// 
			// iProductGrid
			// 
			this.iProductGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iProductGrid.Location = new System.Drawing.Point(3, 16);
			this.iProductGrid.Name = "iProductGrid";
			this.iProductGrid.RowFilter = "";
			this.iProductGrid.RowID = null;
			this.iProductGrid.RowName = null;
			this.iProductGrid.Size = new System.Drawing.Size(602, 349);
			this.iProductGrid.Sort = "";
			this.iProductGrid.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(608, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// ProductTypeGridControl
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.gridBox);
			this.Controls.Add(this.productGroupBox);
			this.Name = "ProductTypeGridControl";
			this.Size = new System.Drawing.Size(608, 368);
			((System.ComponentModel.ISupportInitialize)(this.brandView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numProductUpDown)).EndInit();
			this.gridBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        private bool productDisplay = false;
		private void createTableStyle()
		{
			iProductGrid.Clear();

            iProductGrid.AddTextColumn( "product_name", "Name", false );
			

            if( productDisplay )
            {
                iProductGrid.AddTextColumn( "initial_dislike_probability", false );
                iProductGrid.AddTextColumn( "repeat_like_probability", false );

                if( Database.Nimo )
                {
                    iProductGrid.AddTextColumn( "base_price", false );
                    iProductGrid.AddTextColumn( "eq_units", false );

                    iProductGrid.AddComboBoxColumn( "pack_size_id", theDb.Data.pack_size, "name", "id" );
                }
            }

			iProductGrid.Reset();
		}
	}
}