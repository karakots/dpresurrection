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

	public class DeleteProductTypeDialog : System.Windows.Forms.Form

	{

		private System.Windows.Forms.Button OKButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Button MyCancelButton;

		private System.Data.DataView TypeDataView;

		private System.Windows.Forms.ComboBox TypeComboBox;



		protected ModelDb theDb;



		public ModelDb Db

		{

			set

			{

				theDb = value;



				TypeDataView.Table = theDb.Data.product_type;
                TypeDataView.RowFilter = "id <> -1";

				TypeComboBox.DisplayMember = "type_name";

				TypeComboBox.ValueMember = "id";

			}

		}



		public DeleteProductTypeDialog()

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();

			

		}



		public string TypeName 

		{

			get

			{

				return TypeComboBox.SelectedText;

			}

		}



		public int TypeID

		{

			get

			{

				return (int)TypeComboBox.SelectedValue;

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

			this.OKButton = new System.Windows.Forms.Button();

			this.MyCancelButton = new System.Windows.Forms.Button();

			this.TypeComboBox = new System.Windows.Forms.ComboBox();

			this.TypeDataView = new System.Data.DataView();

			((System.ComponentModel.ISupportInitialize)(this.TypeDataView)).BeginInit();

			this.SuspendLayout();

			// 

			// OKButton

			// 

			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.OKButton.Location = new System.Drawing.Point(64, 48);

			this.OKButton.Name = "OKButton";

			this.OKButton.TabIndex = 4;

			this.OKButton.Text = "OK";

			// 

			// MyCancelButton

			// 

			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			this.MyCancelButton.Location = new System.Drawing.Point(160, 48);

			this.MyCancelButton.Name = "MyCancelButton";

			this.MyCancelButton.TabIndex = 5;

			this.MyCancelButton.Text = "Cancel";

			// 

			// TypeComboBox

			// 

			this.TypeComboBox.DataSource = this.TypeDataView;

			this.TypeComboBox.Location = new System.Drawing.Point(16, 16);

			this.TypeComboBox.Name = "TypeComboBox";

			this.TypeComboBox.Size = new System.Drawing.Size(216, 21);

			this.TypeComboBox.TabIndex = 6;

			// 

			// DeleteProductTypeDialog

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(248, 86);

			this.Controls.Add(this.TypeComboBox);

			this.Controls.Add(this.MyCancelButton);

			this.Controls.Add(this.OKButton);

			this.Name = "DeleteProductTypeDialog";

			this.Text = "Delete Type";

			((System.ComponentModel.ISupportInitialize)(this.TypeDataView)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion

	}

}

