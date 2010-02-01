using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Common.Utilities;

using MarketSimUtilities;

namespace Common
{
	/// <summary>
	/// Summary description for ProductAttributeOptionsDlg.
	/// </summary>
	public class ProductAttributeOptionsDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	
		public bool ProdAttrPreAndPost
		{
			get
			{
				return checkBox1.Checked;
			}

			set
			{
				checkBox1.Checked = value;
			}
		}

		public bool CustPrefPreAndPost
		{
			get
			{
				return checkBox2.Checked;
			}

			set
			{
				checkBox2.Checked = value;
			}
		}

		public bool WarnUser
		{
			set
			{
				warnOnce = value;
			}
		}

		bool warnOnce;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductAttributeOptionsDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			// default is to turn off warnings
			warnOnce = false;
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
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(24, 8);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(248, 24);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Edit Pre and Post Product Attribute Values";
			this.checkBox1.CheckedChanged += new System.EventHandler(this.loseInfo_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(24, 40);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(232, 24);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Edit Pre and Post Customer Preferences";
			this.checkBox2.CheckedChanged += new System.EventHandler(this.loseInfo_CheckedChanged);
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(72, 104);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(48, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(168, 104);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			// 
			// ProductAttributeOptionsDlg
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(312, 142);
			this.ControlBox = false;
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Name = "ProductAttributeOptionsDlg";
			this.Text = "Product Attribute Options";
			this.ResumeLayout(false);

		}
		#endregion

		private void loseInfo_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!warnOnce)
				return;

			warnOnce = false;

			// warn use about losing values if goinf from checked to unchecked
			if (!((CheckBox) sender).Checked)
			{
				MessageBox.Show(this, MrktSimControl.MrktSimMessage("ProdAttr.LoseInfo"), "Warning");
			}		
		}
	}
}
