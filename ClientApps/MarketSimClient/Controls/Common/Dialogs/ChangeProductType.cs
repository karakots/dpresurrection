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

	public class ChangeProductTypeDialog : System.Windows.Forms.Form

	{

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.ComboBox ProductTypeComboBox;

		private System.Windows.Forms.Button OKButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Button MyCancelButton;

		private System.Windows.Forms.TextBox oldTypeName;



		protected ModelDb theDb;



		public ModelDb Db

		{

			set

			{

				theDb = value;

			}

		}



		public ChangeProductTypeDialog(ModelDb Db, string old_name)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = Db;



			System.Data.DataRow[] types = theDb.Data.product_type.Select("id <> -1","", DataViewRowState.CurrentRows);



			foreach( MrktSimDBSchema.product_typeRow row in types)

			{

				ProductTypeComboBox.Items.Add(row.type_name);

			}



			ProductTypeComboBox.SelectedIndex = 0;



			this.oldTypeName.Text = old_name;

			

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

			this.ProductTypeComboBox = new System.Windows.Forms.ComboBox();

			this.OKButton = new System.Windows.Forms.Button();

			this.MyCancelButton = new System.Windows.Forms.Button();

			this.oldTypeName = new System.Windows.Forms.TextBox();

			this.SuspendLayout();

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(8, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(80, 16);

			this.label1.TabIndex = 1;

			this.label1.Text = "Old Type:";

			// 

			// label2

			// 

			this.label2.Location = new System.Drawing.Point(8, 48);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(80, 16);

			this.label2.TabIndex = 2;

			this.label2.Text = "Node Type:";

			// 

			// ProductTypeComboBox

			// 

			this.ProductTypeComboBox.Location = new System.Drawing.Point(88, 48);

			this.ProductTypeComboBox.Name = "ProductTypeComboBox";

			this.ProductTypeComboBox.Size = new System.Drawing.Size(144, 21);

			this.ProductTypeComboBox.TabIndex = 3;

			this.ProductTypeComboBox.Text = "comboBox1";

			// 

			// OKButton

			// 

			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.OKButton.Location = new System.Drawing.Point(64, 96);

			this.OKButton.Name = "OKButton";

			this.OKButton.TabIndex = 4;

			this.OKButton.Text = "OK";

			// 

			// MyCancelButton

			// 

			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			this.MyCancelButton.Location = new System.Drawing.Point(160, 96);

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

			// ChangeProductTypeDialog

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(248, 134);

			this.Controls.Add(this.oldTypeName);

			this.Controls.Add(this.MyCancelButton);

			this.Controls.Add(this.OKButton);

			this.Controls.Add(this.ProductTypeComboBox);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.label1);

			this.Name = "ChangeProductTypeDialog";

			this.Text = "CreateProductDialog";

			this.ResumeLayout(false);



		}

		#endregion

	}

}