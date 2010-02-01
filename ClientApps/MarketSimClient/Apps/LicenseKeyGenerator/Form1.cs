using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DPLicense;

namespace LicenseKeyGenerator
{



	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		// fields
		private LicComputer lic;


		private System.Windows.Forms.TextBox userCode;
		private System.Windows.Forms.Button test;
		private System.Windows.Forms.TextBox key;
		private System.Windows.Forms.DateTimePicker expireDate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			lic = new LicComputer();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.userCode = new System.Windows.Forms.TextBox();
			this.test = new System.Windows.Forms.Button();
			this.key = new System.Windows.Forms.TextBox();
			this.expireDate = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// userCode
			// 
			this.userCode.Location = new System.Drawing.Point(24, 48);
			this.userCode.Name = "userCode";
			this.userCode.Size = new System.Drawing.Size(200, 20);
			this.userCode.TabIndex = 8;
			this.userCode.Text = "";
			this.userCode.TextChanged += new System.EventHandler(this.fre);
			// 
			// test
			// 
			this.test.Location = new System.Drawing.Point(240, 16);
			this.test.Name = "test";
			this.test.Size = new System.Drawing.Size(48, 24);
			this.test.TabIndex = 7;
			this.test.Text = "test...";
			this.test.Click += new System.EventHandler(this.test_Click);
			// 
			// key
			// 
			this.key.Location = new System.Drawing.Point(24, 16);
			this.key.Name = "key";
			this.key.ReadOnly = true;
			this.key.Size = new System.Drawing.Size(200, 20);
			this.key.TabIndex = 6;
			this.key.Text = "";
			// 
			// expireDate
			// 
			this.expireDate.Location = new System.Drawing.Point(24, 80);
			this.expireDate.MinDate = new System.DateTime(1922, 2, 22, 0, 0, 0, 0);
			this.expireDate.Name = "expireDate";
			this.expireDate.TabIndex = 5;
			this.expireDate.ValueChanged += new System.EventHandler(this.expireDate_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 142);
			this.Controls.Add(this.userCode);
			this.Controls.Add(this.test);
			this.Controls.Add(this.key);
			this.Controls.Add(this.expireDate);
			this.Name = "Form1";
			this.Text = "License Key Generator";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void userCode_TextChanged(object sender, System.EventArgs e)
		{
			lic.UserName = userCode.Text;
			lic.ExpirationDate = this.expireDate.Value;

			this.key.Text = lic.LicenseKey;
		}

		private void expireDate_ValueChanged(object sender, System.EventArgs e)
		{
			lic.UserName = userCode.Text;
			lic.ExpirationDate = this.expireDate.Value;

			this.key.Text = lic.LicenseKey;
		}

		private void fre(object sender, System.EventArgs e)
		{
		
		}

		private void test_Click(object sender, System.EventArgs e)
		{

			if (!lic.ValidKey)
				MessageBox.Show("Key is not valid");

			DateTime expiresOn = lic.ExpirationDate;

			MessageBox.Show("Expires On: " + expiresOn.ToShortDateString());
		}

	}
}
