using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ErrorInterface
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ErrorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox ErrorListBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorDialog(ErrorList errors)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			foreach(Error error in errors)
			{
				ErrorListBox.Items.Add(error.ToString());
			}
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
			this.ErrorListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// ErrorListBox
			// 
			this.ErrorListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorListBox.HorizontalScrollbar = true;
			this.ErrorListBox.Location = new System.Drawing.Point(0, 0);
			this.ErrorListBox.Name = "ErrorListBox";
			this.ErrorListBox.Size = new System.Drawing.Size(292, 264);
			this.ErrorListBox.Sorted = true;
			this.ErrorListBox.TabIndex = 0;
			// 
			// ErrorDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.ErrorListBox);
			this.Name = "ErrorDialog";
			this.Text = "Errors";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
