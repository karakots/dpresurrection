using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CreateConsumerPreference.
	/// </summary>
	public class CreateConsumerPreference : System.Windows.Forms.Form
	{
		public ModelDb Db
		{
			set
			{
				segmentPicker.Db = value;
				segmentDatePicker.Value = value.StartDate;
				segmentDatePicker.MinDate = value.StartDate;
				segmentDatePicker.MaxDate = value.EndDate;

				segmentPicker.AllowAll = true;

				attrComboBox.DataSource = value.Data.product_attribute;
				attrComboBox.DisplayMember = "product_attribute_name";
				attrComboBox.ValueMember = "product_attribute_id";
			}
		}

		public DateTime Date
		{
			get
			{
				return segmentDatePicker.Value;
			}
		}


		public int SegmentID
		{
			get
			{
				return segmentPicker.SegmentID;
			}
		}
				  
		public int AttributeID
		{
			get
			{
				if (createAll.Checked)
				{
					return ModelDb.AllID;
				}

				return (int) attrComboBox.SelectedValue;
			}
		}
		
		private Common.Utilities.SegmentPicker segmentPicker;
		private System.Windows.Forms.Button createPreference;
		private System.Windows.Forms.DateTimePicker segmentDatePicker;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox attrComboBox;
		private System.Windows.Forms.CheckBox createAll;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateConsumerPreference()
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
			this.segmentPicker = new Common.Utilities.SegmentPicker();
			this.createPreference = new System.Windows.Forms.Button();
			this.segmentDatePicker = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.attrComboBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.createAll = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// segmentPicker
			// 
			this.segmentPicker.Location = new System.Drawing.Point(24, 16);
			this.segmentPicker.Name = "segmentPicker";
			this.segmentPicker.SegmentID = -1;
			this.segmentPicker.Size = new System.Drawing.Size(216, 24);
			this.segmentPicker.TabIndex = 12;
			// 
			// createPreference
			// 
			this.createPreference.Location = new System.Drawing.Point(304, 88);
			this.createPreference.Name = "createPreference";
			this.createPreference.Size = new System.Drawing.Size(75, 20);
			this.createPreference.TabIndex = 11;
			this.createPreference.Text = "Create";
			this.createPreference.Click += new System.EventHandler(this.createPreference_Click);
			// 
			// segmentDatePicker
			// 
			this.segmentDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.segmentDatePicker.Location = new System.Drawing.Point(304, 40);
			this.segmentDatePicker.Name = "segmentDatePicker";
			this.segmentDatePicker.Size = new System.Drawing.Size(96, 20);
			this.segmentDatePicker.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(296, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 16);
			this.label1.TabIndex = 13;
			this.label1.Text = "Add new preference value on this data";
			// 
			// attrComboBox
			// 
			this.attrComboBox.Location = new System.Drawing.Point(96, 48);
			this.attrComboBox.Name = "attrComboBox";
			this.attrComboBox.Size = new System.Drawing.Size(144, 21);
			this.attrComboBox.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 15;
			this.label2.Text = "Attribute";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(424, 88);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 20);
			this.cancelButton.TabIndex = 16;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// createAll
			// 
			this.createAll.Location = new System.Drawing.Point(96, 80);
			this.createAll.Name = "createAll";
			this.createAll.Size = new System.Drawing.Size(144, 24);
			this.createAll.TabIndex = 17;
			this.createAll.Text = "Create for all attributes";
			this.createAll.CheckedChanged += new System.EventHandler(this.createAll_CheckedChanged);
			// 
			// CreateConsumerPreference
			// 
			this.AcceptButton = this.createPreference;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(528, 118);
			this.ControlBox = false;
			this.Controls.Add(this.createAll);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.attrComboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.segmentPicker);
			this.Controls.Add(this.createPreference);
			this.Controls.Add(this.segmentDatePicker);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CreateConsumerPreference";
			this.Text = "Create Consumer Preference";
			this.ResumeLayout(false);

		}
		#endregion

		private void createPreference_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void createAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (createAll.Checked)
			{
				this.attrComboBox.Enabled = false;
			}
			else
			{
				this.attrComboBox.Enabled = true;
			}
		}
	}
}
