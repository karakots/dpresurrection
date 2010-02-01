using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MsDbInterface
{
	/// <summary>
	/// Summary description for TableGrid.
	/// </summary>
	public class TableGrid : System.Windows.Forms.Form
	{
		public System.Data.DataTable Table
		{
			set
			{
				tableView.Table = value;
				this.Text = value.TableName;
			}
		}

		private System.Windows.Forms.DataGrid grid;
		private System.Data.DataView tableView;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TableGrid()
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
			this.grid = new System.Windows.Forms.DataGrid();
			this.tableView = new System.Data.DataView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tableView)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.DataMember = "";
			this.grid.DataSource = this.tableView;
			this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grid.Location = new System.Drawing.Point(0, 48);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(488, 286);
			this.grid.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(488, 48);
			this.panel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(176, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Filter On...";
			// 
			// textBox1
			// 
			this.textBox1.AcceptsReturn = true;
			this.textBox1.Location = new System.Drawing.Point(232, 16);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(224, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
			this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
			// 
			// TableGrid
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(488, 334);
			this.ControlBox = false;
			this.Controls.Add(this.grid);
			this.Controls.Add(this.panel1);
			this.Name = "TableGrid";
			this.ShowInTaskbar = false;
			this.Text = "TableGrid";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tableView)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
		tableView.RowFilter  = textBox1.Text;
		}

		private void textBox1_Leave(object sender, System.EventArgs e)
		{
		tableView.RowFilter  = textBox1.Text;
		}

	}
}
