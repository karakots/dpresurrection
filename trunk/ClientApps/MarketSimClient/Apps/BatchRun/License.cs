using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BatchRun
{
	/// <summary>
	/// Summary description for License.
	/// </summary>
	public class License : System.Windows.Forms.Form
	{

		public string UserCode
		{
			get
			{
				return userCodeBox.Text;
			}

			set
			{
				userCodeBox.Text = value;
			}	
		}

		public string Key
		{
			get
			{
				return licenseKeyBox.Text;
			}
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox userCodeBox;
		private System.Windows.Forms.TextBox licenseKeyBox;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button done;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public License()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.userCodeBox = new System.Windows.Forms.TextBox();
			this.licenseKeyBox = new System.Windows.Forms.TextBox();
			this.ok = new System.Windows.Forms.Button();
			this.done = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "User Code:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "License Key:";
			// 
			// userCodeBox
			// 
			this.userCodeBox.Location = new System.Drawing.Point(88, 8);
			this.userCodeBox.Name = "userCodeBox";
			this.userCodeBox.Size = new System.Drawing.Size(120, 20);
			this.userCodeBox.TabIndex = 2;
			this.userCodeBox.Text = "";
			// 
			// licenseKeyBox
			// 
			this.licenseKeyBox.Location = new System.Drawing.Point(88, 32);
			this.licenseKeyBox.Name = "licenseKeyBox";
			this.licenseKeyBox.Size = new System.Drawing.Size(120, 20);
			this.licenseKeyBox.TabIndex = 3;
			this.licenseKeyBox.Text = "";
			// 
			// ok
			// 
			this.ok.Location = new System.Drawing.Point(112, 64);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(32, 23);
			this.ok.TabIndex = 4;
			this.ok.Text = "OK";
			this.ok.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// done
			// 
			this.done.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.done.Location = new System.Drawing.Point(160, 64);
			this.done.Name = "done";
			this.done.Size = new System.Drawing.Size(48, 23);
			this.done.TabIndex = 5;
			this.done.Text = "Cancel";
			this.done.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// License
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.done;
			this.ClientSize = new System.Drawing.Size(218, 96);
			this.ControlBox = false;
			this.Controls.Add(this.done);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.licenseKeyBox);
			this.Controls.Add(this.userCodeBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "License";
			this.Text = "License";
			this.ResumeLayout(false);

		}
		#endregion

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
