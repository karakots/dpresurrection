using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreateProductAttribute.

	/// </summary>

	public class CreateProductAttribute : System.Windows.Forms.Form

	{

		public ModelDb Db

		{

			set

			{

				productPicker.Db = value;

				productDatePicker.Value = value.StartDate;

				productDatePicker.MinDate = value.StartDate;

				productDatePicker.MaxDate = value.EndDate;



				attrComboBox.DataSource = value.Data.product_attribute;

				attrComboBox.DisplayMember = "product_attribute_name";

				attrComboBox.ValueMember = "product_attribute_id";

			}

		}



		public int ProductID

		{

			get

			{

				return productPicker.ProductID;

			}

		}



		public int AttributeID

		{

			get

			{

				return (int) attrComboBox.SelectedValue;

			}

		}



		public DateTime Date

		{

			get

			{

				return productDatePicker.Value;

			}

		}



		private Common.Utilities.ProductPicker productPicker;

		private System.Windows.Forms.DateTimePicker productDatePicker;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button createAttribute;

		private System.Windows.Forms.Button cancelButton;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.ComboBox attrComboBox;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreateProductAttribute()

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();

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

			this.productPicker = new Common.Utilities.ProductPicker();

			this.productDatePicker = new System.Windows.Forms.DateTimePicker();

			this.createAttribute = new System.Windows.Forms.Button();

			this.cancelButton = new System.Windows.Forms.Button();

			this.label1 = new System.Windows.Forms.Label();

			this.attrComboBox = new System.Windows.Forms.ComboBox();

			this.label2 = new System.Windows.Forms.Label();

			this.SuspendLayout();

			// 

			// productPicker

			// 

			this.productPicker.Location = new System.Drawing.Point(16, 16);

			this.productPicker.Name = "productPicker";

			this.productPicker.Size = new System.Drawing.Size(216, 56);

			this.productPicker.TabIndex = 11;

			// 

			// productDatePicker

			// 

			this.productDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;

			this.productDatePicker.Location = new System.Drawing.Point(256, 40);

			this.productDatePicker.Name = "productDatePicker";

			this.productDatePicker.Size = new System.Drawing.Size(96, 20);

			this.productDatePicker.TabIndex = 9;

			// 

			// createAttribute

			// 

			this.createAttribute.Location = new System.Drawing.Point(264, 80);

			this.createAttribute.Name = "createAttribute";

			this.createAttribute.Size = new System.Drawing.Size(75, 20);

			this.createAttribute.TabIndex = 10;

			this.createAttribute.Text = "Create";

			this.createAttribute.Click += new System.EventHandler(this.createAttribute_Click);

			// 

			// cancelButton

			// 

			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			this.cancelButton.Location = new System.Drawing.Point(368, 80);

			this.cancelButton.Name = "cancelButton";

			this.cancelButton.Size = new System.Drawing.Size(75, 20);

			this.cancelButton.TabIndex = 12;

			this.cancelButton.Text = "Cancel";

			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(256, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(152, 23);

			this.label1.TabIndex = 13;

			this.label1.Text = "Date for value to take effect";

			// 

			// attrComboBox

			// 

			this.attrComboBox.Location = new System.Drawing.Point(88, 80);

			this.attrComboBox.Name = "attrComboBox";

			this.attrComboBox.Size = new System.Drawing.Size(144, 21);

			this.attrComboBox.TabIndex = 14;

			// 

			// label2

			// 

			this.label2.Location = new System.Drawing.Point(16, 80);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(56, 16);

			this.label2.TabIndex = 15;

			this.label2.Text = "Attribute";

			// 

			// CreateProductAttribute

			// 

			this.AcceptButton = this.createAttribute;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.CancelButton = this.cancelButton;

			this.ClientSize = new System.Drawing.Size(472, 120);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.attrComboBox);

			this.Controls.Add(this.label1);

			this.Controls.Add(this.cancelButton);

			this.Controls.Add(this.productPicker);

			this.Controls.Add(this.productDatePicker);

			this.Controls.Add(this.createAttribute);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

			this.Name = "CreateProductAttribute";

			this.Text = "Create Product Attribute";

			this.ResumeLayout(false);



		}

		#endregion



		private void createAttribute_Click(object sender, System.EventArgs e)

		{

			this.DialogResult = DialogResult.OK;

		}



		private void cancelButton_Click(object sender, System.EventArgs e)

		{

			this.DialogResult = DialogResult.Cancel;

		}

	}

}

