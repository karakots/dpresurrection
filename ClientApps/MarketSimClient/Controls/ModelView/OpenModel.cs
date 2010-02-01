using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ModelView
{
	/// <summary>
	/// Summary description for OpenModel.
	/// </summary>
	public class OpenModel : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView projectView;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OpenModel()
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
			this.projectView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// projectView
			// 
			this.projectView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectView.ImageIndex = -1;
			this.projectView.Location = new System.Drawing.Point(0, 0);
			this.projectView.Name = "projectView";
			this.projectView.SelectedImageIndex = -1;
			this.projectView.Size = new System.Drawing.Size(360, 246);
			this.projectView.TabIndex = 0;
			// 
			// OpenModel
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(360, 246);
			this.Controls.Add(this.projectView);
			this.Name = "OpenModel";
			this.Text = "Select Model";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
