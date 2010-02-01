using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Specialized;

using MrktSimDb;
using Common.MsTree;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for MrktSimControl.
	/// </summary>
	public class MrktSimControl : System.Windows.Forms.UserControl
	{
		public const int AllID = Database.AllID;
		
		public static NameValueCollection AppSettings = null;

		public static string MrktSimMessage(string token)
		{
			if (AppSettings == null)
				return token;

			string message = AppSettings[token];

			if (message == null)
				return token;

			return message;
		}

		// TODO map column names
		public static string MrktSimColumnName(string tableName, string token)
		{
			if (AppSettings == null)
				return token;

			string message = AppSettings[tableName + "." + token];

			if (message == null)
				return token;

			return message;
		}

		public static string MrktSimTableDescription(string tableName, string token)
		{
			if (AppSettings == null)
				return tableName + "." + token;

			string message = AppSettings[tableName + "." + token + ".desc"];

			if (message == null)
				return token;

			return message;

		}

		// sets column width for grid
		// returns total width of datagrid
		public const int colPadding = 15;
		public const int colFudgeFact = 1;
		public const int gridFudgeFact = 4;

		// make columns wide enough to fill grid
		static public void SetWidth(DataGrid datagrid, DataView dataview)
		{
			Graphics g = Graphics.FromHwnd(datagrid.Handle);
			StringFormat sf = new StringFormat(StringFormat.GenericTypographic);

			foreach(DataGridTableStyle table in datagrid.TableStyles)
			{
				int totalWidth = 0;

				foreach(DataGridColumnStyle colStyle in table.GridColumnStyles)
				{		
					// grow width as needed
					int width = 0;

					Type type = colStyle.GetType();

				
					// compute width of this column		
					SizeF size = g.MeasureString(colStyle.HeaderText, datagrid.Font, 500, sf);

					int curWidth = (int) size.Width + colPadding;

					if (curWidth > width)
						width = curWidth;

					if (dataview != null && dataview.Table != null &&
						type != typeof(DataGridDateColumn) &&
						type != typeof(DataGridComboBoxColumn) &&
						type != typeof(DataGridBoolColumn) &&
						type != typeof(DataGridTableViewColumn))
					{
						foreach( DataRow row in dataview.Table.Select("","",DataViewRowState.CurrentRows))
						{
							Object obj = row[colStyle.MappingName];
							string text = "NULL";

							if (obj != null)
							{
								Type objType = obj.GetType();

								if (objType == typeof(System.Decimal))
								{
									decimal num = (decimal) obj;

									text = num.ToString("N");
								}
								else if (objType == typeof(System.Double))
								{
									double num = (double) obj;

									text = num.ToString("N");
								}
								else if (objType == typeof(System.Int32))
								{
									int num = (int) obj;
								}
								else
								{
									text = obj.ToString();
								}
							}
								


							size = g.MeasureString(text, datagrid.Font, 500, sf);
							curWidth = (int) size.Width + colPadding;

							if( curWidth > width)
								width = curWidth;
						}
					}
					else if (type == typeof(DataGridDateColumn))
					{
						if (width < 85)
							width = 85;
					}
					else if (type == typeof(DataGridComboBoxColumn))
					{
						DataGridComboBoxColumn comboCol = (DataGridComboBoxColumn) colStyle;

						foreach( Object obj in comboCol.ColumnComboBox.Items)
						{
							size = g.MeasureString(obj.ToString(), datagrid.Font, 500, sf);
							curWidth = (int) size.Width + 20;

							if ( curWidth > width)
								width = curWidth;
						}
					}
					else if (type == typeof(DataGridTableViewColumn))
					{
						DataGridTableViewColumn comboCol = (DataGridTableViewColumn) colStyle;

						foreach( DataRow row in  ((DataTable) comboCol.ColumnComboBox.DataSource).Select("","",DataViewRowState.CurrentRows))
						{
							object obj = row[comboCol.ColumnComboBox.DisplayMember];

							if (obj != null)
							{
								size = g.MeasureString(obj.ToString(), datagrid.Font, 500, sf);
								curWidth = (int) size.Width + 20;

								if ( curWidth > width)
									width = curWidth;
							}
						}

					}


					colStyle.Width = width;

					totalWidth += width;
				}
	
				int diff = datagrid.Width - gridFudgeFact - totalWidth;

				diff -= SystemInformation.VerticalScrollBarWidth;

				if (table.RowHeadersVisible)
				{
					diff -=  table.RowHeaderWidth;
				}

				int numCols = table.GridColumnStyles.Count;

				if (diff > 0 && numCols > 0)
				{
					// add to each col
					if (numCols != 0)
					{
						int amount = diff/numCols;
						int inchToGrow = diff - numCols*amount;
					
						foreach(DataGridColumnStyle colStyle in table.GridColumnStyles)
						{
							colStyle.Width += amount;
						}

						table.GridColumnStyles[numCols -1].Width += inchToGrow;
					}
				}
			}

			g.Dispose();
			sf.Dispose();
		}


		static public DataGridTableStyle DefaultTableStyle()
		{
			DataGridTableStyle tableStyle = new DataGridTableStyle();
			tableStyle.AlternatingBackColor = Color.LightGray;
			tableStyle.PreferredRowHeight = 10;
			tableStyle.RowHeadersVisible = true;
			tableStyle.RowHeaderWidth = 20;

			return tableStyle;
		}

		protected Database theDb;

		public virtual Database Db
		{
			set
			{
				theDb = value;
			}
		}

		/// <summary>
		/// Called after the database is updated
		/// only needed by components that may be 
		/// sensitive to IDs changing
		/// </summary>
		public virtual void DbUpate()
		{}

		/// <summary>
		/// common variabes to marketSim components
		/// </summary>
		/// 
		private bool suspend = false;

		/// <summary>
		/// Method called when the database changes
		/// should re-initialize control
		/// </summary>
		// protected virtual void dbChanged() {}

		/// <summary>
		/// Method called when we wish to be sure changes in the
		/// control are put into the database
		/// Flush data to dataset
		/// Suspend update actions from dataset (true or false)
		/// Reset form from dataset
		/// </summary>
		
		public virtual void Flush() {}
		public virtual bool Suspend
		{
			set 
			{
				suspend = value;

				if (value == true)
				{
					this.SuspendLayout();
				}
				else
				{
					this.ResumeLayout(false);
				}
			}

			get
			{
				return suspend;
			}
		}

		public virtual void Reset() {}

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MrktSimControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			theDb = null;
		}

		public MrktSimControl(Database db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			theDb = db;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion


		// additional interface for creating - importing and pasting in data

		public virtual bool AllowPaste()
		{
			return false;
		}

		public virtual void Paste()
		{
		}

		public virtual bool AllowExcelImport()
		{
			return false;
		}

		public virtual void ExcelImport()
		{
		}

		public virtual bool AllowCreate()
		{
			return false;
		}

		public virtual void Create()
		{
		}
	}
}
