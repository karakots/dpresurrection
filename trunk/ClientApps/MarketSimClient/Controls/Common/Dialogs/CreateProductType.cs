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

	public class CreateProductTypeDialog : System.Windows.Forms.Form

	{

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button OKButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Button MyCancelButton;

		private System.Windows.Forms.TextBox typeNameTextBox;



		protected ModelDb theDb;



		public ModelDb Db

		{

			set

			{

				theDb = value;

			}

		}



		public CreateProductTypeDialog()

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

				return typeNameTextBox.Text;

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

			this.typeNameTextBox = new System.Windows.Forms.TextBox();

			this.label1 = new System.Windows.Forms.Label();

			this.OKButton = new System.Windows.Forms.Button();

			this.MyCancelButton = new System.Windows.Forms.Button();

			this.SuspendLayout();

			// 

			// typeNameTextBox

			// 

			this.typeNameTextBox.Location = new System.Drawing.Point(88, 16);

			this.typeNameTextBox.Name = "typeNameTextBox";

			this.typeNameTextBox.Size = new System.Drawing.Size(144, 20);

			this.typeNameTextBox.TabIndex = 0;

			this.typeNameTextBox.Text = "Type_Name";

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(8, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(80, 16);

			this.label1.TabIndex = 1;

			this.label1.Text = "Type Name:";

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

			// CreateProductTypeDialog

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(248, 86);

			this.Controls.Add(this.MyCancelButton);

			this.Controls.Add(this.OKButton);

			this.Controls.Add(this.label1);

			this.Controls.Add(this.typeNameTextBox);

			this.Name = "CreateProductDialog";

			this.Text = "CreateProductDialog";

			this.ResumeLayout(false);



		}

		#endregion

	}

}

