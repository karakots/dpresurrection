using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MarketSimUtilities;
using MrktSimDb;

namespace Common.Utilities
{
	

	/// <summary>
	/// Summary description for ProductPicker.
	/// </summary>
	public class ProductPicker : UserControl
	{
		private bool myLeafFlag;
		public bool leafOnly
		{
			set
			{
				myLeafFlag = value;
				if (!value)
				{
					productView.RowFilter = productView.RowFilter.Replace("brand_id = 1","(brand_id = 1 OR brand_id = 0)");
				}
				else
				{
					productView.RowFilter = productView.RowFilter.Replace("(brand_id = 1 OR brand_id = 0)", "brand_id = 1");
				}
			}
			get
			{
				return myLeafFlag;
			}
		}

		private bool allowAll = false;
		public bool AllowAll
		{
			get
			{
				return allowAll;
			}

			set
			{
				allowAll = value;
			}
		}



		public bool TypeOnly
		{
			set
			{
				productBox.Enabled = !value;
			}

			get
			{
				if (productBox.Enabled)
					return false;

				return true;
			}
		}

		public delegate void CurrentProduct(int productID);

		public event CurrentProduct ProductChanged;

		public delegate void CurrentType(int typeID);

		public event CurrentType TypeChanged;

		public int ProductID
		{
			get
			{
				if (productBox.SelectedItem == null)
					return Database.AllID;

				MrktSimDBSchema.productRow prodRow = (MrktSimDBSchema.productRow) 
					((DataRowView) productBox.SelectedItem).Row;

				return prodRow.product_id;
			}

			set
			{
                if (productBox.ValueMember != null &&
                    productBox.ValueMember != "")
				    productBox.SelectedValue = value;
			}
		}

		/*public int BrandID
		{
			get
			{
				if (brandBox.SelectedItem == null)
					return AllID;

				MrktSimDBSchema.brandRow brandRow = (MrktSimDBSchema.brandRow) 
					((DataRowView) brandBox.SelectedItem).Row;

				return brandRow.brand_id;
			}
		}*/

		//private System.Windows.Forms.ComboBox brandBox;
		private System.Windows.Forms.ComboBox productBox;
		private System.Windows.Forms.Label label2;
		private System.Data.DataView productView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox typeBox;
		//private System.Data.DataView brandView;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

        PCSModel theDb = null;
		public PCSModel Db
		{
			set
			{
                theDb = value;

				// bind brand and product to combos
				// combo boxes
				/*brandView.Table = theDb.Data.brand;
				brandBox.DisplayMember = "brand_name";*/

				productView.Table = theDb.Data.product;
				productBox.DisplayMember = "product_name";
				productBox.ValueMember = "product_id";

				typeBox.Items.Clear();
				typeBox.Items.Add("Any");

				DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);

				foreach( MrktSimDBSchema.product_typeRow row in types)
				{
					typeBox.Items.Add(row.type_name);
				}

				typeBox.SelectedIndex = 0;

				string filter = "product_id <> " + ModelDb.AllID + " AND brand_id = 1";

				if (allowAll)
					filter = "product_id = " + ModelDb.AllID + " OR brand_id = 1";

				productView.RowFilter = filter;
			}
		}



        //public ProductPicker(ModelDb db) : base(db)
        //{
        //    // This call is required by the Windows.Forms Form Designer.
        //    InitializeComponent();

        //    // bind brand and product to combos
        //    // combo boxes
        //    //brandView.Table = theDb.Data.brand;
        //    //brandBox.DisplayMember = "brand_name";

        //    string filter = "product_id <> " + ModelDb.AllID + " AND brand_id = 1";

        //    productBox.DataSource = theDb.Data.product;
        //    productBox.DisplayMember = "product_name";
        //    productView.RowFilter = filter;
			
        //    typeBox.Items.Clear();
        //    typeBox.Items.Add("Any");

        //    DataRow[] types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);

        //    foreach( MrktSimDBSchema.product_typeRow row in types)
        //    {
        //        typeBox.Items.Add(row.type_name);
        //    }

        //    typeBox.SelectedIndex = 0;


        //}

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
			this.productBox = new System.Windows.Forms.ComboBox();
			this.productView = new System.Data.DataView();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.typeBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.productView)).BeginInit();
			this.SuspendLayout();
			// 
			// productBox
			// 
			this.productBox.DataSource = this.productView;
			this.productBox.Location = new System.Drawing.Point(72, 32);
			this.productBox.Name = "productBox";
			this.productBox.Size = new System.Drawing.Size(144, 21);
			this.productBox.TabIndex = 1;
			this.productBox.SelectedIndexChanged += new System.EventHandler(this.productBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Product";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Type:";
			// 
			// typeBox
			// 
			this.typeBox.Location = new System.Drawing.Point(72, 8);
			this.typeBox.Name = "typeBox";
			this.typeBox.Size = new System.Drawing.Size(144, 21);
			this.typeBox.TabIndex = 5;
			this.typeBox.SelectedIndexChanged += new System.EventHandler(this.typeBox_SelectedIndexChanged);
			// 
			// ProductPicker
			// 
			this.Controls.Add(this.typeBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.productBox);
			this.Name = "ProductPicker";
			this.Size = new System.Drawing.Size(216, 56);
			((System.ComponentModel.ISupportInitialize)(this.productView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/*private void brandBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Suspend)
				return;

			if (BrandID == ModelDb.AllID)
			{
				if (allowAll)
					productView.RowFilter = null;
				else
					productView.RowFilter = "product_id <> " + ModelDb.AllID;

			}
			else
			{
				productView.RowFilter = "brand_id = " + BrandID;
			}

			if (ProductChanged != null)
				ProductChanged(ProductID);
		}*/

		private void productBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (ProductChanged != null)
				ProductChanged(ProductID);
		}

		private void typeBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string product_type = typeBox.SelectedItem.ToString();

			//product type should always be the last part of the filter.
			//if this changes some sort of more complicated replace function will be needed :(
			int start = productView.RowFilter.IndexOf(" AND product_type = ");
			if(start == -1)
			{
				start = productView.RowFilter.Length;
			}
			productView.RowFilter = productView.RowFilter.Substring(0,start);

			if(!product_type.Equals("Any"))
			{
				DataRow[] type_row = theDb.Data.product_type.Select("type_name = '" + product_type + "'","",DataViewRowState.CurrentRows);
				if(type_row.Length > 0)
				{
					productView.RowFilter += " AND product_type = " + type_row[0]["id"].ToString();
				}
			}

			if (TypeChanged != null)
				TypeChanged(Convert.ToInt32(product_type));
		}

	}
}