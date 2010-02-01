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

	public class CreateProductDialog : System.Windows.Forms.Form

	{

		private System.Windows.Forms.TextBox ProductNameTextBox;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.ComboBox ProductTypeComboBox;

		private System.Windows.Forms.Button OKButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Button MyCancelButton;

		private System.Windows.Forms.RadioButton ParentRadioButton;

		private System.Windows.Forms.RadioButton ChildRadioButton;



		protected ModelDb theDb;

		private System.Windows.Forms.GroupBox PlacementGroupBox;

		private System.Windows.Forms.Button NewTypeButton;



		public ModelDb Db

		{

			set

			{

				theDb = value;

			}

		}



		private string type_filter;





		public CreateProductDialog(ModelDb Db, DataRow Row)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = Db;



			type_filter = "id <> -1";



			if(Row == null)

			{

				//We are creating a root, so disable parent and child radio

				ParentRadioButton.Text = "Parent of none";

				ChildRadioButton.Text = "Child of none";

				ParentRadioButton.Enabled = false;

				ChildRadioButton.Enabled = false;



				if(ProjectDb.Nimo)

				{

					type_filter = "type_name = 'Brand'";

				}



			}

			else

			{

				//Check if parent already exists

				int product_id = (int)Row["product_id"];



				string query = "child_id = " + product_id;



				DataRow[] rows = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);



				if(rows.Length > 0)

				{

					//Parent does exist

					ParentRadioButton.Text = Row["product_name"].ToString() + " already has a parent";

					ChildRadioButton.Text = "Child of " + Row["product_name"].ToString();

					ParentRadioButton.Enabled = false;

					ChildRadioButton.Enabled = true;

					ChildRadioButton.Select();

				

				}

				else

				{

					//Parent Doesn't Exist

					ParentRadioButton.Text = "Parent of " + Row["product_name"].ToString();

					ChildRadioButton.Text = "Child of " + Row["product_name"].ToString();

					ParentRadioButton.Enabled = true;

					ChildRadioButton.Enabled = true;

					ChildRadioButton.Select();



					

					if(ProjectDb.Nimo)

					{

						type_filter = "type_name = 'Product'";

						ParentRadioButton.Text = "NIMO cannot have parents of brands";

						ParentRadioButton.Enabled = false;

					}

				}

			}



			ProductTypeComboBox.Items.Clear();



			System.Data.DataRow[] types = theDb.Data.product_type.Select(type_filter,"", DataViewRowState.CurrentRows);

			if(types.Length == 0)

			{

				OKButton.Enabled = false;

				ProductTypeComboBox.Enabled = false;



			}

			else

			{

				foreach( MrktSimDBSchema.product_typeRow row in types)

				{

					ProductTypeComboBox.Items.Add(row.type_name);

				}



				ProductTypeComboBox.SelectedIndex = 0;



				OKButton.Enabled = true;

				ProductTypeComboBox.Enabled = true;

			}



			if(ProjectDb.Nimo)

			{

				NewTypeButton.Visible = false;

			}

			

		}



		public string ProdName 

		{

			get

			{

				return ProductNameTextBox.Text;

			}

		}



		public int ProductType 

		{

			get

			{

				string query = "type_name = '" + ProductTypeComboBox.SelectedItem + "'";

				System.Data.DataRow[] type = theDb.Data.product_type.Select(query,"", DataViewRowState.CurrentRows);



				return (int)type[0]["id"];

			}

		}



		public string ProductPlacement

		{

			get

			{

				if(ChildRadioButton.Checked)

				{

					return "Child";

				}

				else if(ParentRadioButton.Checked)

				{

					return "Parent";

				}

				else

				{

					return "Root";

				}

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



		public override void Refresh()

		{

			base.Refresh();



			update_product_types();

		}



		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{

			this.ProductNameTextBox = new System.Windows.Forms.TextBox();

			this.label1 = new System.Windows.Forms.Label();

			this.label2 = new System.Windows.Forms.Label();

			this.ProductTypeComboBox = new System.Windows.Forms.ComboBox();

			this.OKButton = new System.Windows.Forms.Button();

			this.MyCancelButton = new System.Windows.Forms.Button();

			this.ParentRadioButton = new System.Windows.Forms.RadioButton();

			this.ChildRadioButton = new System.Windows.Forms.RadioButton();

			this.PlacementGroupBox = new System.Windows.Forms.GroupBox();

			this.NewTypeButton = new System.Windows.Forms.Button();

			this.PlacementGroupBox.SuspendLayout();

			this.SuspendLayout();

			// 

			// ProductNameTextBox

			// 

			this.ProductNameTextBox.Location = new System.Drawing.Point(88, 96);

			this.ProductNameTextBox.Name = "ProductNameTextBox";

			this.ProductNameTextBox.Size = new System.Drawing.Size(144, 20);

			this.ProductNameTextBox.TabIndex = 0;

			this.ProductNameTextBox.Text = "Product_Name";

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(8, 96);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(80, 16);

			this.label1.TabIndex = 1;

			this.label1.Text = "Product Name:";

			// 

			// label2

			// 

			this.label2.Location = new System.Drawing.Point(8, 128);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(80, 16);

			this.label2.TabIndex = 2;

			this.label2.Text = "Type:";

			// 

			// ProductTypeComboBox

			// 

			this.ProductTypeComboBox.Location = new System.Drawing.Point(88, 128);

			this.ProductTypeComboBox.Name = "ProductTypeComboBox";

			this.ProductTypeComboBox.Size = new System.Drawing.Size(144, 21);

			this.ProductTypeComboBox.TabIndex = 3;

			this.ProductTypeComboBox.Text = "Product Type";

			// 

			// OKButton

			// 

			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.OKButton.Location = new System.Drawing.Point(40, 200);

			this.OKButton.Name = "OKButton";

			this.OKButton.Size = new System.Drawing.Size(80, 23);

			this.OKButton.TabIndex = 4;

			this.OKButton.Text = "OK";

			// 

			// MyCancelButton

			// 

			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			this.MyCancelButton.Location = new System.Drawing.Point(152, 200);

			this.MyCancelButton.Name = "MyCancelButton";

			this.MyCancelButton.Size = new System.Drawing.Size(80, 23);

			this.MyCancelButton.TabIndex = 5;

			this.MyCancelButton.Text = "Cancel";

			// 

			// ParentRadioButton

			// 

			this.ParentRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;

			this.ParentRadioButton.Location = new System.Drawing.Point(8, 16);

			this.ParentRadioButton.Name = "ParentRadioButton";

			this.ParentRadioButton.Size = new System.Drawing.Size(208, 32);

			this.ParentRadioButton.TabIndex = 6;

			this.ParentRadioButton.TabStop = true;

			this.ParentRadioButton.Text = "Parent of";

			this.ParentRadioButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;

			// 

			// ChildRadioButton

			// 

			this.ChildRadioButton.Location = new System.Drawing.Point(8, 48);

			this.ChildRadioButton.Name = "ChildRadioButton";

			this.ChildRadioButton.Size = new System.Drawing.Size(208, 24);

			this.ChildRadioButton.TabIndex = 7;

			this.ChildRadioButton.Text = "Child of";

			// 

			// PlacementGroupBox

			// 

			this.PlacementGroupBox.Controls.Add(this.ParentRadioButton);

			this.PlacementGroupBox.Controls.Add(this.ChildRadioButton);

			this.PlacementGroupBox.Location = new System.Drawing.Point(8, 8);

			this.PlacementGroupBox.Name = "PlacementGroupBox";

			this.PlacementGroupBox.Size = new System.Drawing.Size(224, 80);

			this.PlacementGroupBox.TabIndex = 8;

			this.PlacementGroupBox.TabStop = false;

			this.PlacementGroupBox.Text = "Placement";

			// 

			// NewTypeButton

			// 

			this.NewTypeButton.Location = new System.Drawing.Point(152, 160);

			this.NewTypeButton.Name = "NewTypeButton";

			this.NewTypeButton.Size = new System.Drawing.Size(80, 23);

			this.NewTypeButton.TabIndex = 11;

			this.NewTypeButton.Text = "New Type...";

			this.NewTypeButton.Click += new System.EventHandler(this.NewTypeButton_Click);

			// 

			// CreateProductDialog

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(242, 232);

			this.Controls.Add(this.NewTypeButton);

			this.Controls.Add(this.PlacementGroupBox);

			this.Controls.Add(this.MyCancelButton);

			this.Controls.Add(this.OKButton);

			this.Controls.Add(this.ProductTypeComboBox);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.label1);

			this.Controls.Add(this.ProductNameTextBox);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

			this.Name = "CreateProductDialog";

			this.Text = "Create New Product";

			this.PlacementGroupBox.ResumeLayout(false);

			this.ResumeLayout(false);



		}

		#endregion



		private void update_product_types()

		{

			ProductTypeComboBox.Items.Clear();



			System.Data.DataRow[] types = theDb.Data.product_type.Select(type_filter,"", DataViewRowState.CurrentRows);



			if(types.Length == 0)

			{

				OKButton.Enabled = false;

				ProductTypeComboBox.Enabled = false;

			}

			else

			{

				foreach( MrktSimDBSchema.product_typeRow row in types)

				{

					ProductTypeComboBox.Items.Add(row.type_name);

				}



				ProductTypeComboBox.SelectedIndex = 0;



				OKButton.Enabled = true;

				ProductTypeComboBox.Enabled = true;

			}

		}



		private void NewTypeButton_Click(object sender, System.EventArgs e)

		{

			CreateProductTypeDialog dlg = new CreateProductTypeDialog();



			DialogResult rslt = dlg.ShowDialog();



			if (rslt == DialogResult.OK)

			{

				theDb.CreateProductType(dlg.TypeName);



				this.Refresh();

			}



		}

	}

}

