using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Common.Utilities;
using System.Data;

using MarketSimUtilities;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CalibrationView.
	/// </summary>
	public class GridView : System.Windows.Forms.Form
	{
		public delegate void TableStyle(MrktSimGrid grid);

		public event TableStyle CreateTableStyle;

		DataTable table;

		public DataTable Table
		{
			set
			{
				table = value;
				mrktSimGrid1.Table = table;

				if (CreateTableStyle != null)
					CreateTableStyle(mrktSimGrid1);
				else
					createTableStyle();
			}
		}


		public string RowFilter
		{
			set
			{
				mrktSimGrid1.RowFilter = value;
			}
		}

		private MrktSimGrid mrktSimGrid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			mrktSimGrid1.AllowDelete = false;
			mrktSimGrid1.DescriptionWindow = false;
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

				mrktSimGrid1.Dispose();
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
			this.mrktSimGrid1 = new MrktSimGrid();
			this.SuspendLayout();
			// 
			// mrktSimGrid1
			// 
			this.mrktSimGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid1.Location = new System.Drawing.Point(0, 0);
			this.mrktSimGrid1.Name = "mrktSimGrid1";
			this.mrktSimGrid1.RowFilter = null;
			this.mrktSimGrid1.RowID = null;
			this.mrktSimGrid1.RowName = null;
			this.mrktSimGrid1.Size = new System.Drawing.Size(432, 302);
			this.mrktSimGrid1.Sort = "";
			this.mrktSimGrid1.TabIndex = 0;
			// 
			// GridView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 302);
			this.Controls.Add(this.mrktSimGrid1);
			this.Name = "GridView";
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid1.Clear();

			foreach(DataColumn col in table.Columns)
			{
				if (col.DataType == typeof(double))
				{		
					mrktSimGrid1.AddNumericColumn(col.ColumnName, true);
				}
				else if (col.DataType == typeof(string))
				{
						mrktSimGrid1.AddTextColumn(col.ColumnName, true);
				}
			}

			mrktSimGrid1.Reset();
		}

		
	}
}
