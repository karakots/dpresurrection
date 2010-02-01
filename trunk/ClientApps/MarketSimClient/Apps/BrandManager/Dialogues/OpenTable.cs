using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace BrandManager.Dialogues
{
	/// <summary>
	/// Summary description for OpenProjectandModel.
	/// </summary>
	public class OpenTable : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox tableBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OpenTable(DataTable table, string displayMember)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			tableBox.DataSource = table;
			tableBox.DisplayMember = displayMember;
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
			this.tableBox = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableBox
			// 
			this.tableBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableBox.Location = new System.Drawing.Point(0, 0);
			this.tableBox.Name = "tableBox";
			this.tableBox.Size = new System.Drawing.Size(352, 225);
			this.tableBox.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Controls.Add(this.acceptButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 174);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(352, 56);
			this.panel1.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(152, 16);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.Location = new System.Drawing.Point(248, 16);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// OpenTable
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(352, 230);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableBox);
			this.Name = "OpenTable";
			this.Text = "Select Project";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public DataRow SelectedRow()
		{
			if (tableBox.SelectedItem != null)
			{
				return ((DataRowView) tableBox.SelectedItem).Row;
			}

			return null;
		}

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
