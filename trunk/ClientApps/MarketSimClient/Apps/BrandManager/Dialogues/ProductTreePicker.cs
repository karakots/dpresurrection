using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;

namespace BrandManager.Dialogues
{
	/// <summary>
	/// Summary description for ProductPicker.
	/// </summary>
	public class ProductTreePicker : System.Windows.Forms.Form
	{
		private MarketSimUtilities.ProductTree productTree;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CanButton;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductTreePicker()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			productTree.CheckBoxes = false;

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
			this.productTree = new MarketSimUtilities.ProductTree();
			this.OKButton = new System.Windows.Forms.Button();
			this.CanButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// productTree
			// 
			this.productTree.CheckBoxes = true;
			this.productTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.productTree.ImageIndex = -1;
			this.productTree.Location = new System.Drawing.Point(0, 0);
			this.productTree.Name = "productTree";
			this.productTree.SelectedImageIndex = -1;
			this.productTree.Size = new System.Drawing.Size(304, 318);
			this.productTree.TabIndex = 23;
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKButton.Location = new System.Drawing.Point(224, 12);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 24;
			this.OKButton.Text = "OK";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CanButton
			// 
			this.CanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CanButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CanButton.Location = new System.Drawing.Point(128, 12);
			this.CanButton.Name = "CanButton";
			this.CanButton.TabIndex = 25;
			this.CanButton.Text = "Cancel";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.CanButton);
			this.panel1.Controls.Add(this.OKButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 318);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(304, 48);
			this.panel1.TabIndex = 26;
			// 
			// ProductTreePicker
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 366);
			this.ControlBox = false;
			this.Controls.Add(this.productTree);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(304, 328);
			this.Name = "ProductTreePicker";
			this.Text = "Product Selector";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			if(this.productTree.SelectedNode.Tag == null ||((MrktSimDb.MrktSimDBSchema.productRow) this.productTree.SelectedNode.Tag).product_id == Database.AllID)
			{
				MessageBox.Show("Please Select a valid product.", "Select Error", MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}

			this.DialogResult = DialogResult.OK;
		}

		public Database Db
		{
			set
			{
				productTree.Db = value;
			}
		}

		public MrktSimDBSchema.productRow Product
		{
			get
			{
				return (MrktSimDBSchema.productRow)this.productTree.SelectedNode.Tag;
			}
		}
	}
}
