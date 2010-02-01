using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using MarketSimUtilities;
namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for SelectProducts.
	/// </summary>
	public class SelectProducts : System.Windows.Forms.Form
	{
		private ProductTree productTree1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectProducts(ModelDb db)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			this.productTree1.Db = db;
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
			this.productTree1 = new ProductTree();
			this.SuspendLayout();
			// 
			// productTree1
			// 
			this.productTree1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.productTree1.ImageIndex = -1;
			this.productTree1.Location = new System.Drawing.Point(0, 0);
			this.productTree1.Name = "productTree1";
			this.productTree1.SelectedImageIndex = -1;
			this.productTree1.Size = new System.Drawing.Size(292, 266);
			this.productTree1.TabIndex = 0;
			// 
			// SelectProducts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.productTree1);
			this.Name = "SelectProducts";
			this.Text = "Select Products";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
