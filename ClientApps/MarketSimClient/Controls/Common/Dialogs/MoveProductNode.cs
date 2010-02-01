using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Data;

using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreateProductDialog.

	/// </summary>

	public class MoveProductNodeDialog : System.Windows.Forms.Form

	{

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.Button OKButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Button MyCancelButton;

		private System.Windows.Forms.TextBox oldTypeName;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.ComboBox ProductComboBox;

		private System.Windows.Forms.ComboBox TypeComboBox;



		protected ModelDb theDb;



		public ModelDb Db

		{

			set

			{

				theDb = value;

			}

		}



		public MoveProductNodeDialog(ModelDb Db, string old_name)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = Db;



			TypeComboBox.Items.Add("Any");



			DataRow[] types = theDb.Data.product_type.Select("id <> -1","",DataViewRowState.CurrentRows);



			foreach( MrktSimDBSchema.product_typeRow row in types)

			{

				TypeComboBox.Items.Add(row.type_name);

			}



			TypeComboBox.SelectedIndex = 0;



			FillProductComboBox();



			this.oldTypeName.Text = old_name;

			

		}



		private void FillProductComboBox()

		{

			ProductComboBox.Items.Clear();



			string product_type = TypeComboBox.SelectedItem.ToString();



			string filter = "";



			if(product_type.Equals("Any"))

			{

				ProductComboBox.Items.Add("None");

			}

			else

			{

				DataRow[] type_row = theDb.Data.product_type.Select("type_name = '" + product_type + "'","",DataViewRowState.CurrentRows);

				if(type_row.Length > 0)

				{

					filter = "product_type = " + type_row[0]["id"].ToString();

				}

			}



			string query = "product_id <> " + ModelDb.AllID;



			if(filter.Length > 0)

			{

				query += " AND " + filter;

			}



			System.Data.DataRow[] products = theDb.Data.product.Select(query,"", DataViewRowState.CurrentRows);



			foreach( MrktSimDBSchema.productRow row in products)

			{

				ProductComboBox.Items.Add(row.product_name);

			}



			ProductComboBox.SelectedIndex = 0;



		}



		public int Product_Id 

		{

			get

			{

				if(ProductComboBox.SelectedItem.Equals("None"))

				{

					return -1;

				}

				string query = "product_name = '" + ProductComboBox.SelectedItem + "'";

				System.Data.DataRow[] product = theDb.Data.product.Select(query,"", DataViewRowState.CurrentRows);



				return (int)product[0]["product_id"];

			}

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



		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{

			this.label1 = new System.Windows.Forms.Label();

			this.label2 = new System.Windows.Forms.Label();

			this.ProductComboBox = new System.Windows.Forms.ComboBox();

			this.OKButton = new System.Windows.Forms.Button();

			this.MyCancelButton = new System.Windows.Forms.Button();

			this.oldTypeName = new System.Windows.Forms.TextBox();

			this.TypeComboBox = new System.Windows.Forms.ComboBox();

			this.label3 = new System.Windows.Forms.Label();

			this.SuspendLayout();

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(8, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(80, 16);

			this.label1.TabIndex = 1;

			this.label1.Text = "Old Parent:";

			// 

			// label2

			// 

			this.label2.Location = new System.Drawing.Point(8, 80);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(80, 16);

			this.label2.TabIndex = 2;

			this.label2.Text = "New Parent:";

			// 

			// ProductComboBox

			// 

			this.ProductComboBox.Location = new System.Drawing.Point(88, 80);

			this.ProductComboBox.Name = "ProductComboBox";

			this.ProductComboBox.Size = new System.Drawing.Size(144, 21);

			this.ProductComboBox.TabIndex = 3;

			this.ProductComboBox.Text = "comboBox1";

			// 

			// OKButton

			// 

			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.OKButton.Location = new System.Drawing.Point(56, 120);

			this.OKButton.Name = "OKButton";

			this.OKButton.TabIndex = 4;

			this.OKButton.Text = "OK";

			// 

			// MyCancelButton

			// 

			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			this.MyCancelButton.Location = new System.Drawing.Point(152, 120);

			this.MyCancelButton.Name = "MyCancelButton";

			this.MyCancelButton.TabIndex = 5;

			this.MyCancelButton.Text = "Cancel";

			// 

			// oldTypeName

			// 

			this.oldTypeName.BackColor = System.Drawing.SystemColors.Control;

			this.oldTypeName.BorderStyle = System.Windows.Forms.BorderStyle.None;

			this.oldTypeName.Location = new System.Drawing.Point(88, 16);

			this.oldTypeName.Name = "oldTypeName";

			this.oldTypeName.TabIndex = 0;

			this.oldTypeName.Text = "";

			// 

			// TypeComboBox

			// 

			this.TypeComboBox.Location = new System.Drawing.Point(88, 40);

			this.TypeComboBox.Name = "TypeComboBox";

			this.TypeComboBox.Size = new System.Drawing.Size(144, 21);

			this.TypeComboBox.TabIndex = 6;

			this.TypeComboBox.Text = "comboBox1";

			this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);

			// 

			// label3

			// 

			this.label3.Location = new System.Drawing.Point(8, 40);

			this.label3.Name = "label3";

			this.label3.Size = new System.Drawing.Size(80, 16);

			this.label3.TabIndex = 7;

			this.label3.Text = "Parent Type:";

			// 

			// MoveProductNodeDialog

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(248, 158);

			this.Controls.Add(this.label3);

			this.Controls.Add(this.TypeComboBox);

			this.Controls.Add(this.oldTypeName);

			this.Controls.Add(this.MyCancelButton);

			this.Controls.Add(this.OKButton);

			this.Controls.Add(this.ProductComboBox);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.label1);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

			this.Name = "MoveProductNodeDialog";

			this.Text = "Move Product";

			this.ResumeLayout(false);



		}

		#endregion



		private void TypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)

		{

			FillProductComboBox();

		}

	}

}