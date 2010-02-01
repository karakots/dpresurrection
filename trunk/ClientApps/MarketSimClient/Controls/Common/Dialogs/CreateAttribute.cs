using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;
namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CreateAttribute.
	/// </summary>
	public class CreateAttribute : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label attribibuteNameLabel;

        private System.Windows.Forms.TextBox attributeNameBox;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.NumericUpDown numAttributes;
		private System.Windows.Forms.Label numAttributeLabel;
        private ComboBox attrTypeBox;
        private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateAttribute()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			acceptButton.Enabled = false;
			attributeNameBox.Text = "";

            attrTypeBox.DataSource = ModelDb.attribute_type;
            attrTypeBox.DisplayMember = "type";
            attrTypeBox.ValueMember = "id";

            attrTypeBox.SelectedValue = ModelDb.AttributeType.Standard;
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
            this.attribibuteNameLabel = new System.Windows.Forms.Label();
            this.attributeNameBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.numAttributes = new System.Windows.Forms.NumericUpDown();
            this.numAttributeLabel = new System.Windows.Forms.Label();
            this.attrTypeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // attribibuteNameLabel
            // 
            this.attribibuteNameLabel.Location = new System.Drawing.Point( 16, 16 );
            this.attribibuteNameLabel.Name = "attribibuteNameLabel";
            this.attribibuteNameLabel.Size = new System.Drawing.Size( 80, 16 );
            this.attribibuteNameLabel.TabIndex = 16;
            this.attribibuteNameLabel.Text = "Attribute Name";
            // 
            // attributeNameBox
            // 
            this.attributeNameBox.Location = new System.Drawing.Point( 112, 16 );
            this.attributeNameBox.Name = "attributeNameBox";
            this.attributeNameBox.Size = new System.Drawing.Size( 216, 20 );
            this.attributeNameBox.TabIndex = 15;
            this.attributeNameBox.TextChanged += new System.EventHandler( this.attributeNameBox_TextChanged );
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point( 152, 160 );
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size( 75, 23 );
            this.acceptButton.TabIndex = 17;
            this.acceptButton.Text = "Create";
            this.acceptButton.Click += new System.EventHandler( this.acceptButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point( 256, 160 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
            // 
            // numAttributes
            // 
            this.numAttributes.Location = new System.Drawing.Point( 280, 128 );
            this.numAttributes.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numAttributes.Name = "numAttributes";
            this.numAttributes.Size = new System.Drawing.Size( 48, 20 );
            this.numAttributes.TabIndex = 19;
            this.numAttributes.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numAttributeLabel
            // 
            this.numAttributeLabel.Location = new System.Drawing.Point( 112, 128 );
            this.numAttributeLabel.Name = "numAttributeLabel";
            this.numAttributeLabel.Size = new System.Drawing.Size( 160, 23 );
            this.numAttributeLabel.TabIndex = 20;
            this.numAttributeLabel.Text = "Number of Attributes to Create";
            this.numAttributeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // attrTypeBox
            // 
            this.attrTypeBox.FormattingEnabled = true;
            this.attrTypeBox.Location = new System.Drawing.Point( 112, 58 );
            this.attrTypeBox.Name = "attrTypeBox";
            this.attrTypeBox.Size = new System.Drawing.Size( 216, 21 );
            this.attrTypeBox.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 25, 61 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 73, 13 );
            this.label1.TabIndex = 22;
            this.label1.Text = "Attribute Type";
            // 
            // CreateAttribute
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 353, 203 );
            this.ControlBox = false;
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.attrTypeBox );
            this.Controls.Add( this.numAttributeLabel );
            this.Controls.Add( this.numAttributes );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.acceptButton );
            this.Controls.Add( this.attribibuteNameLabel );
            this.Controls.Add( this.attributeNameBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "CreateAttribute";
            this.Text = "Create Attribute";
            ((System.ComponentModel.ISupportInitialize)(this.numAttributes)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		public string AttributeName 
		{
			get
			{
				return attributeNameBox.Text;
			}
		}


        public ModelDb.AttributeType Type {
            get {
                return (ModelDb.AttributeType)attrTypeBox.SelectedValue;
            }
        }

		public int NumAttributes
		{
			get
			{
				return (int) numAttributes.Value;
			}
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void attributeNameBox_TextChanged(object sender, System.EventArgs e)
		{
			if (attributeNameBox.Text != null &&
				attributeNameBox.Text != "")
				acceptButton.Enabled = true;
			else
				acceptButton.Enabled = false;
		}
	}
}
