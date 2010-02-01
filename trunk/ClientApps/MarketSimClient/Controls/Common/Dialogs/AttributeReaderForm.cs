using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ExcelInterface;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for AttributeReaderForm.
	/// </summary>
	public class AttributeReaderForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DateTimePicker startDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox dataType;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AttributeReaderForm()
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
			this.startDate = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.dataType = new System.Windows.Forms.ComboBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// startDate
			// 
			this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.startDate.Location = new System.Drawing.Point(160, 8);
			this.startDate.Name = "startDate";
			this.startDate.Size = new System.Drawing.Size(88, 20);
			this.startDate.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Start Date of Attribute";
			// 
			// dataType
			// 
			this.dataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dataType.Location = new System.Drawing.Point(16, 72);
			this.dataType.Name = "dataType";
			this.dataType.Size = new System.Drawing.Size(232, 21);
			this.dataType.TabIndex = 2;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(224, 120);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(40, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(120, 120);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(72, 24);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(232, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Assign To...";
			// 
			// AttributeReaderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(274, 152);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.dataType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.startDate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "AttributeReaderForm";
			this.Text = "Attribute Data";
			this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		public void UseProductValues()
		{
			dataType.Items.Clear();

			dataType.Items.Add("Pre Use Attribute Value");
			dataType.Items.Add("Post Use Attribute Value");
			dataType.Items.Add("Both Pre and Post Use Attribute Value");
			dataType.SelectedIndex = 0;
			
			this.Text = "Select Product Attribute Data Type";
		}

		public void UseSegmentValues()
		{
			dataType.Items.Clear();

			dataType.Items.Add("Pre Use Preference value");
			dataType.Items.Add("Post Use Preference Value");
			dataType.Items.Add("Both Pre and Post Use Preference Value");
			dataType.Items.Add("Attribute Price Sensitivity");

			dataType.SelectedIndex = 0;

			this.Text = "Select Consumer Preference Data Type";
		}

		public AttributeDataReader.AttributeValueType DataType
		{
			get
			{
				switch(dataType.SelectedIndex)
				{
					case 0: 
						return AttributeDataReader.AttributeValueType.PreVal;
					case 1:
						return AttributeDataReader.AttributeValueType.PostVal;
					case 3:
						return AttributeDataReader.AttributeValueType.PriceUtil;
				}

				return AttributeDataReader.AttributeValueType.PreAndPost;
			}
		}
		
			
		public DateTime StartDate
		{
			get
			{
				return this.startDate.Value;
			}

			set
			{
				this.startDate.Value = value;
			}
		}


	}
}
