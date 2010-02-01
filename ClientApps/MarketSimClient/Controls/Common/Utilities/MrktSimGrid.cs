using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Dialogs;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for MrktSimGrid.
	/// </summary>
	public class MrktSimGrid : System.Windows.Forms.UserControl
	{
//		public enum DataType
//		{
//			TextBox,
//			CheckBox,
//			Numeric
//		}

		public GridColumnStylesCollection Columns
		{
			get
			{
				return iTableStyle.GridColumnStyles;
			}
		}

		public  ArrayList GetSelected()
		{
			ArrayList rows = new ArrayList();

			for(int ii = 0; ii < dataView.Count; ++ii)
			{
				if (dataGrid.IsSelected(ii))
				{
					rows.Add(dataView[ii].Row);
				}
			}

			return rows;
		}

		public int Count
		{
			get
			{
				return dataView.Count;
			}
		}

		//private DataType dataType = DataType.TextBox;

		private string rowIdentifier = null;
		public string RowID
		{
			set
			{
				rowIdentifier = value;
			}

			get
			{
				return rowIdentifier;
			}
		}

		private string rowNameIdentifier = null;
		public string RowName
		{
			set
			{
				rowNameIdentifier = value;
			}

			get
			{
				return rowNameIdentifier;
			}
		}

		public delegate string RowToName(DataRow row);
		public event RowToName GetRowName;

		private Database theDb = null;
		private System.Windows.Forms.MenuItem changePrecision;
		private System.Windows.Forms.MenuItem scaleItem;
		private System.Windows.Forms.MenuItem incItem;
		private System.Windows.Forms.MenuItem exportItem;
		private System.Windows.Forms.MenuItem fillMenuItem;
		private System.Windows.Forms.MenuItem includeFilteritem;
		private System.Windows.Forms.MenuItem excludeFilterItem;
	
		public Database Db
		{
			set
			{
				theDb = value;
			}
		}


		// Getters and Setters
		public string RowFilter
		{
			set
			{
				dataView.RowFilter = value;
				baseFilter = value;
			}

			get
			{
				return baseFilter;
			}
		}

		// Getters and Setters
		bool allowSorting = true;
		public string Sort
		{
			set
			{
				if (value != null && value != "")
				{
					dataView.Sort = value;
					allowSorting = false;
				}
				else
					allowSorting = true;
			}

			get
			{
				return dataView.Sort;
			}
		}

		// Getters
		public DataRow CurrentRow
		{
			get
			{
				int rowNum = dataGrid.CurrentRowIndex;

				if (rowNum < 0)
					return null;

				DataRowView row = dataView[rowNum];

				return row.Row;
			}
		}


		public 	BindingManagerBase Binding
		{
			get
			{
				return dataGrid.BindingContext[dataView];
			}
		}

		public DataView DataSource
		{
			get
			{
				return this.dataView;
			}
		}

		// Setters 
		public DataTable Table
		{
			set
			{
				if(value == null)
				{
					return;
				}
				dataView.Table = value;
				iTableStyle.MappingName = null;
				string tableName = value.TableName;
				iTableStyle.MappingName = tableName;
				dataView.Table.RowChanged +=new DataRowChangeEventHandler(Table_RowChanged);

				if (DescribeRow != null && CurrentRow != null)
				{
					columnDesc.Text = this.CurrentRow[DescribeRow].ToString();
				}
			}

			get
			{
				return dataView.Table;
			}
		}


		// special table type
		public DataMatrix DataMatrix
		{
			set
			{
				dataView.Table = value;
				iTableStyle.MappingName = value.TableName;
				value.DataMatrixColumnChanged +=new Common.Utilities.DataMatrix.DataMatrixColumnChangedHandler(dataMatrix_ColumnChanged);
				
				isDataMatrix = true;

				AllowDelete = false;

				DescriptionWindow = false;

				Reset();
			}
		}

		public bool DescriptionWindow
		{
			set
			{
				columnDesc.Visible = value;

				if (value)
					splitter1.Enabled = true;
				else
					splitter1.Enabled = false;
			}
		}

		private string rowDescr = null;
		public string DescribeRow
		{
			set
			{
				rowDescr = value;
			}

			get
			{
				return rowDescr;
			}
		}

		// instead of getting description from column
		// we display this constant string
		public string CommonDescription
		{
			set
			{
				iCommonDesc = value;
			}
		}

		public bool RowHeaders
		{
			set
			{
				iTableStyle.RowHeadersVisible = value;
			}
		}

		
		public bool AllowDelete
		{
			set
			{
				allowDelete = value;
				deleteRow.Visible = value;
			}
		}

		public  bool Suspend
		{
			set
			{
				// turn of sorts during update
				if (value)
				{
					dataView.Sort = null;
					dataView.RowFilter = null;
				}
				else
					dataView.RowFilter = baseFilter;


				suspend = value;

				if (isDataMatrix)
					((DataMatrix) dataView.Table).Suspend = value;
			}
		}

		
		// public methods
		public void Flush()
		{
			int colNum = dataGrid.CurrentCell.ColumnNumber;
			int rowNum = dataGrid.CurrentCell.RowNumber;

			if (rowNum >= 0 && colNum >= 0)
			{
				DataGridColumnStyle colStyle = iTableStyle.GridColumnStyles[colNum];

				dataGrid.EndEdit(colStyle,rowNum,false);
			}

			CurrencyManager currMan = (CurrencyManager) dataGrid.BindingContext[dataView];
		
			if(currMan.Position < 0 && currMan.Count > 0)
			{
				currMan.Refresh();	
			}


			currMan.EndCurrentEdit();

			if (isDataMatrix)
				((DataMatrix) dataView.Table).Flush();
		}

		public override void Refresh()
		{
			dataGrid.Invalidate();
			dataGrid.Refresh();

			base.Refresh ();

			if (isDataMatrix)
			{
				Suspend = true;
				((DataMatrix) dataView.Table).Refresh();
				Suspend = false;
			
				this.dataGrid.Refresh();
			}
		}

		// exposes index of grid
		private int CurrentRowIndex
		{
			get
			{
				return dataGrid.CurrentRowIndex;
			}
		}

	
		public void Reset()
		{
			Refresh();

			this.SuspendLayout();

			dataGrid.TableStyles.Clear();

			createDataMatrixTableStyle();
			

			dataGrid.TableStyles.Add(iTableStyle);

			MrktSimControl.SetWidth(this.dataGrid, this.dataView);


			this.ResumeLayout(false);

			dataGrid.Invalidate();
			dataGrid.Refresh();
		}


		public void Clear()
		{
			iTableStyle.GridColumnStyles.Clear();
		}

		#region Add Columns
		/// <summary>
		/// Creates a standard text box column
		/// </summary>
		/// <param name="colName"></param>
		/// /// <param name="readOnly"></param>
		
		public DataGridTextBoxColumn AddNumericColumn(string colName)
		{
			string name = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);
			return this.AddNumericColumn(colName, name);
		}

		public DataGridTextBoxColumn AddNumericColumn(string colName, string name)
		{
			return this.AddNumericColumn(colName, name, false);
		}

		public DataGridTextBoxColumn AddNumericColumn(string colName, bool readOnly)
		{
			string name = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);
				return this.AddNumericColumn(colName, name, readOnly);
		}

		public DataGridTextBoxColumn AddNumericColumn(string colName, string name, bool readOnly)
		{
			DataGridTextBoxColumn col = AddTextColumn(colName, name, readOnly);
			col.Format = "N2";

			return col;
		}


		public DataGridTextBoxColumn AddTextColumn(string colName, string name)
		{
			return this.AddTextColumn(colName, name, false);
		}

		public DataGridTextBoxColumn AddTextColumn(string colName)
		{
			return this.AddTextColumn(colName, false);
		}

		public DataGridTextBoxColumn AddTextColumn(string colName,  bool readOnly)
		{
			string name = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);

			return AddTextColumn(colName, name, readOnly);
		}

		public DataGridTextBoxColumn AddTextColumn(string colName, string name, bool readOnly)
		{
			DataGridTextBoxColumn col = new DataGridTextBoxColumn();

			col.TextBox.ContextMenu = dataOpMenu;

			col.Format = "";
			col.FormatInfo = null;
			col.HeaderText = name;
			col.MappingName = colName;
			col.ReadOnly = readOnly;
			col.Width = computeWidth(col.HeaderText);

			iTableStyle.GridColumnStyles.Add(col);
			
			return col;
		}

		/// <summary>
		/// Creates a combo box column
		/// </summary>
		/// <param name="colName"></param>
		public DataGridComboBoxColumn AddComboBoxColumn(string colName, string[] options)
		{
			DataGridComboBoxColumn col = new DataGridComboBoxColumn();

			col.TextBox.ContextMenu = dataOpMenu;

			col.HeaderText = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);
			col.MappingName = colName;

			int width = computeWidth(col.HeaderText);

			if (iTableStyle.PreferredRowHeight < col.ColumnComboBox.Height + 3)
				iTableStyle.PreferredRowHeight = col.ColumnComboBox.Height + 3;

			col.ColumnComboBox.Items.Clear();

			foreach(string opt in options)
			{
				col.ColumnComboBox.Items.Add(opt);

				// plus width of down arrow
				int curWidth = computeWidth(opt) + 20;

				if ( curWidth > width)
					width = curWidth;
			}

			col.Width = width;


			col.ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			iTableStyle.GridColumnStyles.Add(col);

			return col;
		}

		public DataGridComboBoxColumn AddComboBoxColumn(string colName, DataTable tbl, string displayMember, string valueMember)
		{
			return AddComboBoxColumn(colName, tbl, displayMember, valueMember, false);
		}

		public DataGridComboBoxColumn AddComboBoxColumn(string colName, DataTable tbl, string displayMember, string valueMember, bool readOnly)
		{
			DataGridComboBoxColumn col = new DataGridTableViewColumn();
			
			col.TextBox.ContextMenu = dataOpMenu;

			col.HeaderText = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);
			col.MappingName = colName;

			int width = computeWidth(col.HeaderText);

			if (iTableStyle.PreferredRowHeight < col.ColumnComboBox.Height + 3)
				iTableStyle.PreferredRowHeight = col.ColumnComboBox.Height + 3;

			col.ColumnComboBox.Items.Clear();

			col.ColumnComboBox.DataSource = tbl;
			col.ColumnComboBox.ValueMember = valueMember;
			col.ColumnComboBox.DisplayMember = displayMember;

			foreach (DataRow row in tbl.Select("","",DataViewRowState.CurrentRows))
			{
				// plus width of down arrow
				int curWidth = computeWidth(row[displayMember].ToString()) + 20;

				if ( curWidth > width)
					width = curWidth;
			}

			col.Width = width;

			col.ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			col.ReadOnly = readOnly;

			iTableStyle.GridColumnStyles.Add(col);

			return col;
		}

		/// <summary>
		/// Creates a check box column
		/// caller supplies trua and false values
		/// </summary>
		/// <param name="colName"></param>
		/// 
		public DataGridBoolColumn AddCheckBoxColumn(string colName)
		{
			return AddCheckBoxColumn(colName, MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName));
		}

		public DataGridBoolColumn AddCheckBoxColumn(string colName, string name)
		{
			// product attribute relevance
			DataGridBoolColumn col = new DataGridBoolColumn();
			
			// col.ContextMenu = dataOpMenu;

			col.AllowNull = false;
			col.HeaderText = name;
			col.MappingName = colName;
			col.Width = computeWidth(col.HeaderText);
			
			iTableStyle.GridColumnStyles.Add(col);

			return col;
		}

		public DataGridBoolColumn AddCheckBoxColumn(string colName, string trueVal, string falseVal)
		{
			// product attribute relevance
			DataGridBoolColumn col = new DataGridBoolColumn();

			// col.TextBox.ContextMenu = dataOpMenu;

			col.AllowNull = false;
			col.FalseValue = falseVal;
			col.TrueValue = trueVal;
			col.HeaderText = MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName);
			col.MappingName = colName;
			col.Width = computeWidth(col.HeaderText);
			
			iTableStyle.GridColumnStyles.Add(col);

			return col;
		}

		//public DataGridDateColumn AddDateColumn(string colName)
		public DataGridTextBoxColumn AddDateColumn(string colName)
		{
			return AddDateColumn(colName, MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName));
		}

		public DataGridTextBoxColumn AddDateColumn(string colName, bool readOnly)
		{
			return AddDateColumn(colName, MrktSimControl.MrktSimColumnName(iTableStyle.MappingName, colName), readOnly);
		}

		// public DataGridDateColumn AddDateColumn(string colName, string name)
		public DataGridTextBoxColumn AddDateColumn(string colName, string name)
		{
			return AddDateColumn(colName, name, false);
		}

		// public DataGridDateColumn AddDateColumn(string colName, string name)
		public DataGridTextBoxColumn AddDateColumn(string colName, string name, bool readOnly)
		{
			// product attribute relevance
			// DataGridDateColumn col = new DataGridDateColumn();
			//
			// took out the calendar picker now we treat as text
			//
			DataGridTextBoxColumn col = new DataGridTextBoxColumn();
			col.TextBox.ContextMenu = dataOpMenu; // allow pasting

			col.HeaderText = name;
			col.MappingName = colName;			
			col.Width = computeWidth(col.HeaderText);
			
			if (col.Width < 85)
				col.Width = 85;

			col.ReadOnly = readOnly;

			iTableStyle.GridColumnStyles.Add(col);

			return col;
		}

		#endregion



		// private methods
		private bool allowDelete = true;
		private bool suspend = false;

		private DataGridTableStyle iTableStyle;

		private System.Windows.Forms.DataGrid dataGrid;
		private System.Data.DataView dataView;
		private System.Windows.Forms.TextBox columnDesc;
		private System.Windows.Forms.ContextMenu dataOpMenu;
		private System.Windows.Forms.MenuItem deleteRow;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuItem pasteData;

		private string iCommonDesc = null;

		private bool isDataMatrix = false;
		private System.Windows.Forms.MenuItem firstSortItem;
		private System.Windows.Forms.MenuItem secondSortItem;
		private System.Windows.Forms.MenuItem thirdSortItem;
		private System.Windows.Forms.MenuItem clearSortItem;
		private System.Windows.Forms.MenuItem firstAscItem;
		private System.Windows.Forms.MenuItem firstDecItem;

		private DataGridColumnStyle firstSort = null;
		private DataGridColumnStyle secondSort = null;
		private DataGridColumnStyle thirdSort = null;
		private DataGridColumnStyle currentColStyle = null;

		private System.Windows.Forms.MenuItem secondAscItem;
		private System.Windows.Forms.MenuItem secondDecItem;
		private System.Windows.Forms.MenuItem thirdAscItem;
		private System.Windows.Forms.MenuItem thirdDecItem;
		private System.Windows.Forms.MenuItem filterItem;
		private System.Windows.Forms.MenuItem filterOffItem;

		private string baseFilter = null;
		private System.Windows.Forms.MenuItem sortMenu;
		private System.Windows.Forms.MenuItem parametrizeItem;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MrktSimGrid()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			dataView.AllowNew = false;
			dataView.Table = null;

			iTableStyle = MrktSimControl.DefaultTableStyle();
			iTableStyle.AlternatingBackColor = Color.LightBlue;
			iTableStyle.SelectionBackColor = Color.DarkSeaGreen;
			
			iTableStyle.AllowSorting = false;

			this.firstSortItem.Text = "Add Sort";
			this.firstSortItem.Enabled = true;
			this.secondSortItem.Visible = false;
			this.thirdSortItem.Visible = false;
			this.clearSortItem.Visible = false;
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

			// remove event handling

			if (dataView.Table != null)
			{
				dataView.Table.RowChanged -=new DataRowChangeEventHandler(Table_RowChanged);
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
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.dataOpMenu = new System.Windows.Forms.ContextMenu();
			this.deleteRow = new System.Windows.Forms.MenuItem();
			this.pasteData = new System.Windows.Forms.MenuItem();
			this.sortMenu = new System.Windows.Forms.MenuItem();
			this.firstSortItem = new System.Windows.Forms.MenuItem();
			this.firstAscItem = new System.Windows.Forms.MenuItem();
			this.firstDecItem = new System.Windows.Forms.MenuItem();
			this.secondSortItem = new System.Windows.Forms.MenuItem();
			this.secondAscItem = new System.Windows.Forms.MenuItem();
			this.secondDecItem = new System.Windows.Forms.MenuItem();
			this.thirdSortItem = new System.Windows.Forms.MenuItem();
			this.thirdAscItem = new System.Windows.Forms.MenuItem();
			this.thirdDecItem = new System.Windows.Forms.MenuItem();
			this.clearSortItem = new System.Windows.Forms.MenuItem();
			this.filterItem = new System.Windows.Forms.MenuItem();
			this.includeFilteritem = new System.Windows.Forms.MenuItem();
			this.excludeFilterItem = new System.Windows.Forms.MenuItem();
			this.filterOffItem = new System.Windows.Forms.MenuItem();
			this.fillMenuItem = new System.Windows.Forms.MenuItem();
			this.changePrecision = new System.Windows.Forms.MenuItem();
			this.scaleItem = new System.Windows.Forms.MenuItem();
			this.incItem = new System.Windows.Forms.MenuItem();
			this.exportItem = new System.Windows.Forms.MenuItem();
			this.parametrizeItem = new System.Windows.Forms.MenuItem();
			this.dataView = new System.Data.DataView();
			this.columnDesc = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid
			// 
			this.dataGrid.AllowNavigation = false;
			this.dataGrid.AllowSorting = false;
			this.dataGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dataGrid.CaptionVisible = false;
			this.dataGrid.ContextMenu = this.dataOpMenu;
			this.dataGrid.DataMember = "";
			this.dataGrid.DataSource = this.dataView;
			this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(0, 0);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.ParentRowsVisible = false;
			this.dataGrid.PreferredColumnWidth = 20;
			this.dataGrid.RowHeaderWidth = 20;
			this.dataGrid.SelectionForeColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(255)));
			this.dataGrid.Size = new System.Drawing.Size(300, 241);
			this.dataGrid.TabIndex = 0;
			this.dataGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGrid_MouseDown);
			this.dataGrid.SizeChanged += new System.EventHandler(this.dataGrid_SizeChanged);
			this.dataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGrid_MouseUp);
			// 
			// dataOpMenu
			// 
			this.dataOpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.deleteRow,
																					   this.pasteData,
																					   this.sortMenu,
																					   this.filterItem,
																					   this.filterOffItem,
																					   this.fillMenuItem,
																					   this.changePrecision,
																					   this.scaleItem,
																					   this.incItem,
																					   this.exportItem,
																					   this.parametrizeItem});
			// 
			// deleteRow
			// 
			this.deleteRow.Index = 0;
			this.deleteRow.Text = "delete";
			this.deleteRow.Click += new System.EventHandler(this.deleteRow_Click);
			// 
			// pasteData
			// 
			this.pasteData.Index = 1;
			this.pasteData.Text = "paste";
			this.pasteData.Click += new System.EventHandler(this.pasteData_Click);
			// 
			// sortMenu
			// 
			this.sortMenu.Index = 2;
			this.sortMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.firstSortItem,
																					 this.secondSortItem,
																					 this.thirdSortItem,
																					 this.clearSortItem});
			this.sortMenu.Text = "sort";
			// 
			// firstSortItem
			// 
			this.firstSortItem.Index = 0;
			this.firstSortItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.firstAscItem,
																						  this.firstDecItem});
			this.firstSortItem.Text = "sort 1";
			// 
			// firstAscItem
			// 
			this.firstAscItem.Index = 0;
			this.firstAscItem.Text = "Ascending";
			this.firstAscItem.Click += new System.EventHandler(this.firstAscItem_Click);
			// 
			// firstDecItem
			// 
			this.firstDecItem.Index = 1;
			this.firstDecItem.Text = "Descending";
			this.firstDecItem.Click += new System.EventHandler(this.firstDecItem_Click);
			// 
			// secondSortItem
			// 
			this.secondSortItem.Index = 1;
			this.secondSortItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.secondAscItem,
																						   this.secondDecItem});
			this.secondSortItem.Text = "sort 2";
			// 
			// secondAscItem
			// 
			this.secondAscItem.Index = 0;
			this.secondAscItem.Text = "Ascending";
			this.secondAscItem.Click += new System.EventHandler(this.secondAscItem_Click);
			// 
			// secondDecItem
			// 
			this.secondDecItem.Index = 1;
			this.secondDecItem.Text = "Descending";
			this.secondDecItem.Click += new System.EventHandler(this.secondDecItem_Click);
			// 
			// thirdSortItem
			// 
			this.thirdSortItem.Index = 2;
			this.thirdSortItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.thirdAscItem,
																						  this.thirdDecItem});
			this.thirdSortItem.Text = "sort 3";
			// 
			// thirdAscItem
			// 
			this.thirdAscItem.Index = 0;
			this.thirdAscItem.Text = "Ascending";
			this.thirdAscItem.Click += new System.EventHandler(this.thirdAscItem_Click);
			// 
			// thirdDecItem
			// 
			this.thirdDecItem.Index = 1;
			this.thirdDecItem.Text = "Descending";
			this.thirdDecItem.Click += new System.EventHandler(this.thirdDecItem_Click);
			// 
			// clearSortItem
			// 
			this.clearSortItem.Index = 3;
			this.clearSortItem.Text = "clear sorting";
			this.clearSortItem.Click += new System.EventHandler(this.clearSortItem_Click);
			// 
			// filterItem
			// 
			this.filterItem.Index = 3;
			this.filterItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.includeFilteritem,
																					   this.excludeFilterItem});
			this.filterItem.Text = "filter";
			// 
			// includeFilteritem
			// 
			this.includeFilteritem.Index = 0;
			this.includeFilteritem.Text = "include";
			this.includeFilteritem.Click += new System.EventHandler(this.includeFilteritem_Click);
			// 
			// excludeFilterItem
			// 
			this.excludeFilterItem.Index = 1;
			this.excludeFilterItem.Text = "exclude";
			this.excludeFilterItem.Click += new System.EventHandler(this.excludeFilterItem_Click);
			// 
			// filterOffItem
			// 
			this.filterOffItem.Enabled = false;
			this.filterOffItem.Index = 4;
			this.filterOffItem.Text = "turn off filtering";
			this.filterOffItem.Click += new System.EventHandler(this.filterOffItem_Click);
			// 
			// fillMenuItem
			// 
			this.fillMenuItem.Index = 5;
			this.fillMenuItem.Text = "fill";
			this.fillMenuItem.Click += new System.EventHandler(this.fillMenuItem_Click);
			// 
			// changePrecision
			// 
			this.changePrecision.Index = 6;
			this.changePrecision.Text = "precision";
			this.changePrecision.Click += new System.EventHandler(this.changePrecision_Click);
			// 
			// scaleItem
			// 
			this.scaleItem.Index = 7;
			this.scaleItem.Text = "scale";
			this.scaleItem.Click += new System.EventHandler(this.scaleItem_Click);
			// 
			// incItem
			// 
			this.incItem.Index = 8;
			this.incItem.Text = "increment";
			this.incItem.Click += new System.EventHandler(this.incItem_Click);
			// 
			// exportItem
			// 
			this.exportItem.Index = 9;
			this.exportItem.Text = "export...";
			this.exportItem.Click += new System.EventHandler(this.exportItem_Click);
			// 
			// parametrizeItem
			// 
			this.parametrizeItem.Index = 10;
			this.parametrizeItem.Text = "Parametrize";
			this.parametrizeItem.Click += new System.EventHandler(this.parametrizeItem_Click);
			// 
			// columnDesc
			// 
			this.columnDesc.BackColor = System.Drawing.SystemColors.Window;
			this.columnDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.columnDesc.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.columnDesc.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.columnDesc.Location = new System.Drawing.Point(0, 244);
			this.columnDesc.Multiline = true;
			this.columnDesc.Name = "columnDesc";
			this.columnDesc.ReadOnly = true;
			this.columnDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.columnDesc.Size = new System.Drawing.Size(300, 56);
			this.columnDesc.TabIndex = 1;
			this.columnDesc.Text = "";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 241);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(300, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// MrktSimGrid
			// 
			this.Controls.Add(this.dataGrid);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.columnDesc);
			this.Name = "MrktSimGrid";
			this.Size = new System.Drawing.Size(300, 300);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

	
		/// <summary>
		/// Creates a check box column
		/// caller supplies trua and false values
		/// </summary>
		/// <param name="colName"></param>
	
		private void Table_RowChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (suspend)
				return;

			if (e.Action == DataRowAction.Commit)
			{
				Reset();
			}
		}

		private void dataMatrix_ColumnChanged()
		{
			if (suspend)
				return;

			// meed to start afresh

			dataGrid.TableStyles.Clear();

			createDataMatrixTableStyle();
			
			dataGrid.TableStyles.Add(iTableStyle);

			MrktSimControl.SetWidth(this.dataGrid, this.dataView);

			this.ResumeLayout(false);

			dataGrid.Refresh();
		}

		private void dataGrid_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.DataGrid.HitTestInfo info = dataGrid.HitTest(e.X, e.Y);

			pasteData.Visible = false;
			deleteRow.Visible = false;
			sortMenu.Visible = false;
			filterItem.Visible = false;
			parametrizeItem.Visible = false;
			this.scaleItem.Visible = false;
			this.changePrecision.Visible = false;
			this.incItem.Visible = false;
			fillMenuItem.Visible = false;

			currentColStyle = null;

			if (info.Column < 0 )
			{
				if (info.Row >= 0 && allowDelete)
					deleteRow.Visible = true;
				
				return;
			}
			else
			{
				if (iTableStyle.GridColumnStyles.Count > 0)
				{
					sortMenu.Visible = true;
					filterItem.Visible = true;

					currentColStyle = iTableStyle.GridColumnStyles[info.Column];

					if (columnDesc.Visible && this.iCommonDesc == null && this.DescribeRow == null)
						columnDesc.Text = MrktSimControl.MrktSimTableDescription(iTableStyle.MappingName, currentColStyle.MappingName);
				
					DataGridTableViewColumn tableStyle = currentColStyle as DataGridTableViewColumn;

					// when we can paste or parametrize or other operations
					
					DataColumn col = dataView.Table.Columns[currentColStyle.MappingName];

					bool readOnly = currentColStyle.ReadOnly || col.ReadOnly || tableStyle != null;

					Type type = col.DataType;

					if (info.Row >= 0 && !readOnly)
						fillMenuItem.Visible = true;

					if (type == typeof(System.Decimal) ||
						type == typeof(System.Double) ||
						type == typeof(System.Int32))
					{
						// items that change values
						if (!readOnly)
						{
							if (RowID != null)
								parametrizeItem.Visible = true;

							if (info.Row >= 0 )
							{
								pasteData.Visible = true;
								parametrizeItem.Text = "Parametrize Value";
							}
							else
							{
								parametrizeItem.Text = "Parametrize All Values";
							}

							scaleItem.Visible = true;
							incItem.Visible = true;
						}

						changePrecision.Visible = true;
					}
					else if (!readOnly &&
						info.Row >= 0 && 
						type == typeof(System.DateTime))
					{
						pasteData.Visible = true;
					}
				}
			}
	
			if (allowSorting)
				this.enableSort();
			else
				sortMenu.Visible = false;
		}

		
		private int computeWidth( string name)
		{
			Graphics g = Graphics.FromHwnd(dataGrid.Handle);
			StringFormat sf = new StringFormat(StringFormat.GenericTypographic);

			SizeF size = g.MeasureString(name, dataGrid.Font, 500, sf);

			g.Dispose();
			sf.Dispose();

			return (int) size.Width + MrktSimControl.colPadding;
		}

		private void deleteRow_Click(object sender, System.EventArgs e)
		{
			ArrayList rows = GetSelected();

			Suspend = true;

			foreach(DataRow row in rows)
			{
				row.Delete();
			}

			Suspend = false;
		
			// delete the current row from grid
			// int rowNum = dataGrid.CurrentRowIndex;
		}

		private void pasteData_Click(object sender, System.EventArgs e)
		{
			int rowNum = dataGrid.CurrentRowIndex;
			int colNum = dataGrid.CurrentCell.ColumnNumber;

			int numRows = dataView.Count;

			// Create a new instance of the DataObject interface.
			IDataObject data = Clipboard.GetDataObject();
			// If the data is text, then set the text of the 
			// TextBox to the text in the Clipboard.
			if (data.GetDataPresent(DataFormats.Text))
			{
				string text = data.GetData(DataFormats.Text).ToString();

				char[] newLine = {'\r', '\n'};
				char[] tab = {'\t'};
				char[] comma = {',', ' '};

				string[] lines = text.Split(newLine);

				string error = null;
			
				foreach(string line in lines)
				{
					if (line.Length == 0)
						continue;

					if (rowNum >= numRows)
					{
						// not really an error
						// but let user know
						error = "Could not paste in all data";
						break;
					}

					// process line
					DataRow row;
					try
					{
						 row = dataView[rowNum].Row;
					}
					catch(System.Exception tooManyRows)
					{
						error = tooManyRows.Message;
						break;
					}

					int index = 0;

					string[] nums = line.Split(tab);
					foreach(string num in nums)
					{
						// get data type
						currentColStyle = iTableStyle.GridColumnStyles[colNum + index];



						// get name of column
						string colName = currentColStyle.MappingName;

						if (colName == null)
						{
							error = "Incorrect Data Format";
							break;
						}

						DataColumn col = dataView.Table.Columns[currentColStyle.MappingName];	
						Type type = col.DataType;

						if (type == typeof(System.Decimal) ||
							type == typeof(System.Double) ||
							type == typeof(System.Int32))
						{
							Decimal val;

							try
							{
								val = Decimal.Parse(num);
							}
							catch(System.Exception oops)
							{
								error = oops.Message;
								break;
							}

							try 
							{
								row[colName] = val;
							}
							catch(System.Exception oops)
							{
								error = oops.Message;
							}
						}
						else if (type == typeof(System.DateTime))
						{
							DateTime val;

							try
							{
								val = DateTime.Parse(num);
							}
							catch(System.Exception oops)
							{
								error = oops.Message;
								break;
							}

							try 
							{
								row[colName] = val;
							}
							catch(System.Exception oops)
							{
								error = oops.Message;
							}
						}
					
						index++;
					}

					rowNum++;

					if (error != null)
						break;
				}

				if (error != null)
				{
					MessageBox.Show(error);
				}
			}
		}

		private void dataGrid_SizeChanged(object sender, System.EventArgs e)
		{
			MrktSimControl.SetWidth(this.dataGrid, this.dataView);
		}

		private void createDataMatrixTableStyle()
		{
			if (dataView.Table.GetType() != typeof(DataMatrix))
				return;

			Clear();

			DataMatrix mat = (DataMatrix) dataView.Table;

			foreach( DataColumn col in mat.Columns)
			{
				string name = mat.ColumnName(col.ColumnName);

				if (name == null)
					continue;

				if (mat.Header(col.ColumnName))
				{
					AddTextColumn(col.ColumnName, name, true);
				}
				else
				{
					Type dataType = col.DataType;

					if (dataType == typeof(System.Boolean))
						AddCheckBoxColumn(col.ColumnName, name);	
					else
						this.AddTextColumn(col.ColumnName, name);
				}
			}
		}

		
		private void clearSortItem_Click(object sender, System.EventArgs e)
		{
			firstSort = null;
			secondSort = null;
			thirdSort = null;

			dataView.Sort = null;

			this.firstSortItem.Text = "Add Sort";
			this.firstSortItem.Enabled = true;

			this.firstAscItem.Checked = false;
			this.firstAscItem.Enabled = true;

			this.firstDecItem.Checked = false;
			this.firstDecItem.Enabled = true;

			this.secondSortItem.Visible = false;
			this.thirdSortItem.Visible = false;
			this.clearSortItem.Visible = false;

		}

		// enable or disable adding sorting depending on currentColStyle
		private void enableSort()
		{
			// nothing to do
			if (thirdSort != null)
				return;

			if (secondSort != null)
			{
				if (currentColStyle == null ||
					secondSort == currentColStyle ||
					firstSort == currentColStyle)
				{
					// disable
					this.thirdSortItem.Enabled = false;
				}
				else
				{
					//enable the second
					this.thirdSortItem.Enabled = true;
				}

				return;
			}

			if (firstSort != null)
			{
				if (currentColStyle == null ||
					firstSort == currentColStyle)
				{
					// disable
					this.secondSortItem.Enabled = false;
				}
				else
				{
					//enable the second
					this.secondSortItem.Enabled = true;
				}
			}
		}

		private void addSort()
		{
			if (currentColStyle == null)
				return;

			if (firstSort == null && currentColStyle != firstSort)
			{
				this.firstSort = currentColStyle;

				this.firstSortItem.Text = currentColStyle.HeaderText;
				
				


				this.secondSortItem.Visible = true;
				this.secondSortItem.Text = "Add sort";

				this.secondAscItem.Checked = false;
				this.secondAscItem.Enabled = true;

				this.secondDecItem.Checked = false;
				this.secondDecItem.Enabled = true;
				
				this.clearSortItem.Visible = true;
			}
			else if (secondSort == null && currentColStyle != secondSort)
			{
				this.secondSort = currentColStyle;

				this.secondSortItem.Text = currentColStyle.HeaderText;

				this.thirdSortItem.Visible = true;
				this.thirdSortItem.Text = "Add sort";

				this.thirdAscItem.Checked = false;
				this.thirdAscItem.Enabled = true;

				this.thirdDecItem.Checked = false;
				this.thirdDecItem.Enabled = true;
			}
			else if (thirdSort == null && currentColStyle != thirdSort)
			{
				this.thirdSort = currentColStyle;

				this.thirdSortItem.Text = currentColStyle.HeaderText;
			}
		}

		private void createSortString()
		{
			if (firstSort == null)
			{
				dataView.Sort = null;
				return;
			}

			dataView.Sort = firstSort.MappingName;

			if (!this.firstAscItem.Checked)
				dataView.Sort += " DESC";
			

			if (secondSort == null)
				return;

			dataView.Sort += ", " + secondSort.MappingName;

			if (!this.secondAscItem.Checked)
				dataView.Sort += " DESC";

			
			if (thirdSort == null)
				return;

			dataView.Sort += ", " + thirdSort.MappingName;

			if (!this.thirdAscItem.Checked)
				dataView.Sort += " DESC";
		}

		private void firstAscItem_Click(object sender, System.EventArgs e)
		{
			this.firstAscItem.Checked = true;
			this.firstAscItem.Enabled = false;

			this.firstDecItem.Checked = false;
			this.firstDecItem.Enabled = true;

			if (firstSort == null)
			{
				addSort();
			}

			// create sort string
			createSortString();

		}

		private void firstDecItem_Click(object sender, System.EventArgs e)
		{
			this.firstDecItem.Checked = true;
			this.firstDecItem.Enabled = false;

			this.firstAscItem.Checked = false;
			this.firstAscItem.Enabled = true;

			if (firstSort == null)
			{
				addSort();
			}

			// create sort string
			createSortString();
		
		}

		private void secondAscItem_Click(object sender, System.EventArgs e)
		{
			this.secondAscItem.Checked = true;
			this.secondAscItem.Enabled = false;

			this.secondDecItem.Checked = false;
			this.secondDecItem.Enabled = true;

			if (secondSort == null)
			{
				addSort();
			}
			
			// create sort string
			createSortString();
		}

		private void secondDecItem_Click(object sender, System.EventArgs e)
		{
			this.secondDecItem.Checked = true;
			this.secondDecItem.Enabled = false;

			this.secondAscItem.Checked = false;
			this.secondAscItem.Enabled = true;

			if (secondSort == null)
			{
				addSort();
			}

			// create sort string
			createSortString();
		}

		private void thirdAscItem_Click(object sender, System.EventArgs e)
		{
			this.thirdAscItem.Checked = true;
			this.thirdAscItem.Enabled = false;

			this.thirdDecItem.Checked = false;
			this.thirdDecItem.Enabled = true;

			if (thirdSort == null)
			{
				addSort();
			}
			
			// create sort string
			createSortString();
		
		}

		private void thirdDecItem_Click(object sender, System.EventArgs e)
		{
			this.thirdDecItem.Checked = true;
			this.thirdDecItem.Enabled = false;

			this.thirdAscItem.Checked = false;
			this.thirdAscItem.Enabled = true;

			if (thirdSort == null)
			{
				addSort();
			}

			// create sort string
			createSortString();
		}

		

		private void parametrizeRow(DataRow row)
		{
			string colName = currentColStyle.MappingName;

			MrktSimDBSchema.model_parameterRow parm = theDb.CreateModelParameter(row, colName, RowID);

			if (parm == null)
			{
				MessageBox.Show("Error creating parameter");
				return;
			}

			string recordName = null;

			string tableName = dataView.Table.TableName;

			if (RowName != null)
			{
				recordName = row[RowName].ToString();
				parm.name = tableName + "_" + recordName + "_" + colName;
			}
			else if (GetRowName != null)
			{
				recordName = GetRowName(row);
				parm.name = recordName + "_" + colName;
			}
		}

		private void parametrizeItem_Click(object sender, System.EventArgs e)
		{
			if (this.isDataMatrix)
				return;

			if (currentColStyle == null)
				return;

			if (RowID == null)
				return;

			if (theDb == null)
				return;

			int rowNum = dataGrid.CurrentRowIndex;
			
			int numRows = dataView.Count;

			if (rowNum >= numRows)
				return;

			if (rowNum > 0)
			{
				DataRow row;
			
				row = dataView[rowNum].Row;
				parametrizeRow(row);
			}
			else
			{	
				DataRow[] rows = this.dataView.Table.Select(dataView.RowFilter, "", DataViewRowState.CurrentRows);

				foreach(DataRow row in rows)
				{
					parametrizeRow(row);
				}
			}	
		}

		private void scaleItem_Click(object sender, System.EventArgs e)
		{
			if (currentColStyle == null)
				return;

			InputDouble dlg = new InputDouble();

			DialogResult res = dlg.ShowDialog();

			if (res != DialogResult.OK)
				return;

			DataRow[] rows = this.dataView.Table.Select(dataView.RowFilter, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				row[currentColStyle.MappingName] = System.Convert.ToDouble(row[currentColStyle.MappingName]) * dlg.Value;
			}
		}

		private void changePrecision_Click(object sender, System.EventArgs e)
		{
			if (currentColStyle == null)
				return;

			DataGridTextBoxColumn col = (DataGridTextBoxColumn)iTableStyle.GridColumnStyles[currentColStyle.MappingName];

			if (col.Format.Length == 0)
			{
				// set to something reasonable
				col.Format = "N2";
			}

			String s = col.Format.Substring(1,col.Format.Length - 1);
			int current = 2;
			if (s.Length > 0)
			{
				current = int.Parse(s);
			}

			InputDouble dlg = new Common.Dialogs.InputDouble();
			dlg.Max = 14;
			dlg.Min = 0;
			dlg.Value = current;

			DialogResult res = dlg.ShowDialog();

			if (res != DialogResult.OK)
				return;

			col.Format = "N" + ((int)dlg.Value).ToString();

		}

		private void incItem_Click(object sender, System.EventArgs e)
		{
			if (currentColStyle == null)
				return;

			InputDouble dlg = new InputDouble();

			DialogResult res = dlg.ShowDialog();

			if (res != DialogResult.OK)
				return;

			DataRow[] rows = this.dataView.Table.Select(dataView.RowFilter, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				row[currentColStyle.MappingName] = System.Convert.ToDouble(row[currentColStyle.MappingName]) + dlg.Value;
			}
		
		}

		private void exportItem_Click(object sender, System.EventArgs e)
		{
			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();

			saveFileDlg.DefaultExt = ".csv";
			saveFileDlg.Filter = "CSV File (*.csv)|*.csv";

			saveFileDlg.CheckFileExists = false;
			//saveFileDlg.ReadOnlyChecked = false;

			DialogResult rslt = saveFileDlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			string fileName = saveFileDlg.FileName;
				
			System.IO.StreamWriter writer;

			// any open erros are due to file being in use
			try
			{
				writer = new System.IO.StreamWriter(fileName);
			}
			catch(System.IO.IOException oops)
			{
				MessageBox.Show(oops.Message);
				return;
			}

			DataRow[] rows = this.dataView.Table.Select(dataView.RowFilter, dataView.Sort, DataViewRowState.CurrentRows);

			if (rows.Length == 0)
				return;

			// write out headers
			char comma = ',';
			char[] trim = {comma};

			string header = "";

			foreach(DataGridColumnStyle style in iTableStyle.GridColumnStyles)
			{
				header += style.HeaderText + ",";
			}
			
			header.TrimEnd(trim);

			writer.WriteLine(header);

			foreach(DataRow row in rows)
			{
				// write out row
				string values = "";

				foreach(DataGridColumnStyle style in iTableStyle.GridColumnStyles)
				{
					DataGridTableViewColumn tableCol = style as DataGridTableViewColumn;

					string valString = null;

					if (tableCol != null)
						valString = tableCol.TableObject(row[style.MappingName]).ToString();
					else
						valString = row[style.MappingName].ToString();

					values +=  valString + ",";
				}

				values.TrimEnd(trim);
				writer.WriteLine(values);
			}

			writer.Flush();
			writer.Close();
		}

		private void fillMenuItem_Click(object sender, System.EventArgs e)
		{
			if (currentColStyle == null)
				return;

			int rowNum = dataGrid.CurrentRowIndex;
			int colNum = dataGrid.CurrentCell.ColumnNumber;
			int numRows = dataView.Count;

			if (rowNum >= numRows)
				return;

			if (rowNum < 0)
				return;

			// process line
			DataRow thisRow;
			
			thisRow = dataView[rowNum].Row;

			DataRow[] rows = this.dataView.Table.Select(dataView.RowFilter, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				row[currentColStyle.MappingName] = thisRow[currentColStyle.MappingName];
			}
		}

		#region filtering
		private void filterSelection(string equals)
		{
			if (currentColStyle == null)
				return;

			int rowNum = dataGrid.CurrentRowIndex;
			int colNum = dataGrid.CurrentCell.ColumnNumber;
			int numRows = dataView.Count;

			if (rowNum >= numRows)
				return;

			if (rowNum < 0)
				return;

			// process line
			DataRow row;
			
			row = dataView[rowNum].Row;

			string query = currentColStyle.MappingName + " " + equals + " '" + row[currentColStyle.MappingName].ToString() +"'";

			if (dataView.RowFilter != null && !dataView.RowFilter.Equals(""))
			{
				query = dataView.RowFilter + " AND " + query;
			}

			try
			{
				dataView.RowFilter = query;
			}
			catch(Exception)
			{
				MessageBox.Show("Unable to process query");
			}


			filterOffItem.Enabled = true;
		}

		private void filterOffItem_Click(object sender, System.EventArgs e)
		{
			dataView.RowFilter = baseFilter;
			filterOffItem.Enabled = false;
		}
		private void includeFilteritem_Click(object sender, System.EventArgs e)
		{
			filterSelection("=");
		}
		#endregion

		private void excludeFilterItem_Click(object sender, System.EventArgs e)
		{
			filterSelection("<>");
		}

		private void dataGrid_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.DataGrid.HitTestInfo info = dataGrid.HitTest(e.X, e.Y);

			if (info.Column < 0 )
			{
				if (DescribeRow != null && CurrentRow != null)
				{
					columnDesc.Text = this.CurrentRow[DescribeRow].ToString();
				}
			}
		}
	}
}
