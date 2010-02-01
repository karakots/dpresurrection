using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MarketSimUtilities
{
	/// <summary>
	/// Common utilities used by other components
	/// </summary>
	/// 

	// Implments a ComboBox for column derives from TextBox
	public class DataGridComboBoxColumn : DataGridTextBoxColumn
	{

		public NoKeyUpCombo ColumnComboBox = null;
		protected System.Windows.Forms.CurrencyManager theSource = null;
		protected int theRowNum;
		protected bool isEditing = false;

	
		public DataGridComboBoxColumn() : base()
		{
			ColumnComboBox = new NoKeyUpCombo();
		
			ColumnComboBox.Leave += new EventHandler(LeaveComboBox);
			ColumnComboBox.SelectionChangeCommitted += new System.EventHandler(ComboStartEditing);
		}
		
		private void ComboStartEditing(object sender, EventArgs e)
		{
			isEditing = true;
			base.ColumnStartedEditing((Control) sender);
		}
		

		protected virtual void LeaveComboBox(object sender, EventArgs e)
		{
			if(isEditing)
			{
				SetColumnValueAtRow(theSource, theRowNum, ColumnComboBox.Text);
				isEditing = false;
				Invalidate();
			}

			ColumnComboBox.Hide();
		}

		protected override void Edit(System.Windows.Forms.CurrencyManager aSource, int aRowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			base.Edit(aSource, aRowNum, bounds, readOnly, instantText , cellIsVisible);

			if (ReadOnly)
				return;

			theRowNum = aRowNum;
			theSource = aSource;
		
			ColumnComboBox.Parent = this.TextBox.Parent;
			ColumnComboBox.Location = this.TextBox.Location;
			ColumnComboBox.Size = new Size(this.TextBox.Size.Width, ColumnComboBox.Size.Height);
			ColumnComboBox.Text =  this.TextBox.Text;

			this.TextBox.Visible = false;
			ColumnComboBox.Visible = true;
			ColumnComboBox.BringToFront();
			ColumnComboBox.Focus();	
		}

		protected override bool Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
		{
			if(isEditing)
			{
				isEditing = false;
				SetColumnValueAtRow(dataSource, rowNum, ColumnComboBox.Text);
			}
			return true;
		}
	}

	public class DataGridTableViewColumn : DataGridComboBoxColumn
	{
		public DataGridTableViewColumn() : base() {}

		public object TableObject(object obj)
		{
			DataTable tbl = (DataTable) ColumnComboBox.DataSource;

			// find obj in ColumnComboBox datasource
			DataRow row = tbl.Rows.Find(obj);

			if (row == null)
				return null;

			return row[ColumnComboBox.DisplayMember];
		}

		protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			Object obj =  base.GetColumnValueAtRow (source, rowNum);

			return TableObject(obj);
		}


		protected override void LeaveComboBox(object sender, EventArgs e)
		{
			if(isEditing)
			{
                SetColumnValueAtRow( theSource, theRowNum, ColumnComboBox.SelectedValue );
				isEditing = false;
				Invalidate();
			}

			ColumnComboBox.Hide();
		}

		protected override bool Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
		{
			if(isEditing)
			{
				isEditing = false;
				SetColumnValueAtRow(dataSource, rowNum, ColumnComboBox.SelectedValue);
			}
			return true;
		}
	}

	// Implments a ComboBox for column derives from TextBox
	public class DataGridDateColumn : DataGridTextBoxColumn
	{
		private DateTimePicker dateChooser = null;
		private System.Windows.Forms.CurrencyManager theSource = null;
		private int theRowNum;
		private bool isEditing = false;
		
		public DataGridDateColumn() : base()
		{
			dateChooser  = new DateTimePicker();

			dateChooser.Format = DateTimePickerFormat.Short;

			dateChooser.Leave +=new EventHandler(LeaveDateChooser);
		
			dateChooser.ValueChanged +=new EventHandler(DateStartEditing);
		}
		
		private void DateStartEditing(object sender, EventArgs e)
		{
			isEditing = true;
			base.ColumnStartedEditing((Control) sender);
		}
		
		private void LeaveDateChooser(object sender, EventArgs e)
		{
			if(isEditing)
			{
				SetColumnValueAtRow(theSource, theRowNum, dateChooser.Value);
				isEditing = false;
				Invalidate();
			}

			dateChooser.Hide();
		}

		protected override void Edit(System.Windows.Forms.CurrencyManager aSource, int aRowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			base.Edit(aSource, aRowNum, bounds, readOnly, instantText , cellIsVisible);

			if (ReadOnly)
				return;

			theRowNum = aRowNum;
			theSource = aSource;
		
			dateChooser.Parent = this.TextBox.Parent;
			dateChooser.Location = this.TextBox.Location;
			dateChooser.Size = new Size(this.TextBox.Size.Width, this.TextBox.Size.Height);
			dateChooser.Value =  DateTime.Parse(this.TextBox.Text);

			this.TextBox.Visible = false;
			dateChooser.Visible = true;
			dateChooser.BringToFront();
			dateChooser.Focus();	
		}

		protected override bool Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
		{
			if(isEditing)
			{
				isEditing = false;
				SetColumnValueAtRow(dataSource, rowNum, dateChooser.Value);
			}
			return true;
		}
	}

	//  A usefull class
	public class NoKeyUpCombo : ComboBox
	{
		const int WM_KEYUP = 0x101;
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if(m.Msg == WM_KEYUP)
			{
				//ignore keyup to avoid problem with tabbing & dropdownlist;
				return;
			}
			base.WndProc(ref m);
		}
	}

	public class DataMatrix : DataTable
	{
		public delegate void DataMatrixColumnChangedHandler();
		public event DataMatrixColumnChangedHandler DataMatrixColumnChanged;
			
		public string RowFilter
		{
			set
			{
				rowFilter = value;
			}
		}

		public string ColumnFilter
		{
			set
			{
				colFilter = value;
			}
		}

		public string ValueFilter
		{
			set
			{
				valFilter = value;
			}
		}

		public string ValueSort
		{
			set
			{
				valSort = value;
			}
		}


		private string rowFilter = "";
		private string colFilter = "";
		private string valFilter = "";
		private string valSort = "";

		
		private bool suspend;

		private string headerText; // header text goes above row name in table
		private string colKeyName; // the name of primary key in col table
        private string valColKeyName; // the name of the col key in value table
        private string rowKeyName; // the name of primary key in row table
        private string valRowKeyName; // the name of the row key in value table
		private string rowName; // name field in col table
		private string colName; // name field in row table
		private string valName; // name of field to display in value table

		private DataTable colTable;
		private DataTable rowTable;
		private DataTable valTable;

		public bool Header(string colKey)
		{
			if (colKey == headerText)
				return true;

			return false;
		}

		public string ColumnName(string colKey)
		{
			if (colKey == colKeyName)
				return null;

			if (colKey == rowKeyName)
				return null;

			if (colKey == headerText)
				return colKey;

			DataRow colRow = colTable.Rows.Find(colKey);

			if (colRow != null)
				return colRow[colName].ToString();

			return null;
		}

		private void initialize()
		{
			colTable.RowChanged += new DataRowChangeEventHandler(this.dataColChanged);
			colTable.RowDeleting += new DataRowChangeEventHandler(this.dataColChanged);

			rowTable.RowChanged += new DataRowChangeEventHandler(this.dataRowChanged);
			rowTable.RowDeleting += new DataRowChangeEventHandler(this.dataRowChanged);

            rowTable.TableNewRow += new DataTableNewRowEventHandler(rowTable_TableNewRow);
            colTable.TableNewRow += new DataTableNewRowEventHandler(rowTable_TableNewRow);
			
			// valTable.RowChanged +=new DataRowChangeEventHandler(this.dataValueChanged);
			
			CreateTable();
		}

      

		// edits are written into dabatase
		public void Flush()	
		{
			DataRow[] rows = valTable.Select(valFilter, "", DataViewRowState.CurrentRows);

			foreach (DataRow valRow in rows)
			{
				Object tmp = valRow[valColKeyName];

				if( tmp == null)
					continue;

				string colID = tmp.ToString();

				tmp = valRow[valRowKeyName].ToString();

				if( tmp == null)
					continue;

				string rowID = tmp.ToString();

				DataRow row = this.Rows.Find(rowID);

				if (row != null)
				{

					// only update if row has been modified
					if (row.RowState == DataRowState.Modified)
					{
						Object obj = row[colID];

						if (obj != null && obj.GetType() != typeof(System.DBNull))				
							valRow[valName] = obj;
					}
				}
			}
		}

		private void refreshValues()
		{
			foreach(DataRow row in valTable.Select(valFilter, valSort, DataViewRowState.CurrentRows))
			{
				setValue(row);
			}

			AcceptChanges();
		}

		
		// read values for a given column
		public void Refresh()
		{
			if (stale)
			{
				CreateTable();
			}
			else
			{
				refreshValues();
			}
		}
	
		public bool Suspend
		{
			set
			{
				suspend = value;
			}

			get
			{
				return suspend;
			}
		}

		public DataMatrix (
			string aHeaderText,
			DataTable aRowTable,
			string aRowKeyName,
			string aRowName,
			DataTable aColTable,
			string aColKeyName,
			string aColName,
			DataTable aValTable,
			string aValName)
		{	
			colTable =  aColTable;
			rowTable =  aRowTable;
			valTable =  aValTable;
			headerText = aHeaderText;
			colKeyName = aColKeyName;
            // assumes the same name
            valColKeyName = aColKeyName;

			rowKeyName = aRowKeyName;

            // assumes the same
            valRowKeyName = aRowKeyName;

			rowName = aRowName;
			colName = aColName;
			valName = aValName;

			initialize();
		}

        public DataMatrix
            (
            string aHeaderText,

            DataTable aRowTable,
            string aRowKeyName,
            string aRowName,

            DataTable aColTable,
            string aColKeyName,
            string aColName,

            DataTable aValTable,
            string aValRowKeyName,
            string aValColKeyName,
            string aValName 
            )
        {
            colTable = aColTable;
            rowTable = aRowTable;
            valTable = aValTable;
            headerText = aHeaderText;
            colKeyName = aColKeyName;
            rowKeyName = aRowKeyName;


            rowName = aRowName;
            colName = aColName;
            valName = aValName;

            valRowKeyName = aValRowKeyName;
            valColKeyName = aValColKeyName;


            initialize();
        }


		// create the initial dataset schema
		public void CreateTable()
		{	
			stale = false;

			this.PrimaryKey = null;
			this.Rows.Clear();
			this.Columns.Clear();

			this.TableName = valName;

			// key columns - not seen
			DataColumn keyCol = new DataColumn();
			keyCol.ColumnName = rowKeyName;
			keyCol.DataType =  typeof(string);

			this.Columns.Add(keyCol);
			this.PrimaryKey = new DataColumn[] {keyCol};

			// header column - what the user sees
			DataColumn headCol = new DataColumn();
			headCol.ColumnName = headerText;
			headCol.DataType = typeof(string);

			this.Columns.Add(headCol);
			
			DataRow[] rows = colTable.Select(colFilter, "", DataViewRowState.CurrentRows);
			foreach(DataRow colRow in rows)
				addColumn(colRow);
			
			// now add rows to tables			
			rows = rowTable.Select(rowFilter, "", DataViewRowState.CurrentRows);
			foreach(DataRow rowRow in rows)
			{
				// check if a valid row in value table first
				string query = rowKeyName + " = " + rowRow[rowKeyName].ToString();
				DataRow[] checks = valTable.Select(query, "", DataViewRowState.CurrentRows);

				if (checks.Length > 0)
				{
					addRow(rowRow);
				}
			}
			
			refreshValues();
		}

		// row operations
		private void addRow(DataRow rowRow)
		{
			string rowID = rowRow[rowKeyName].ToString();
			string name = rowRow[rowName].ToString();

			if (rowID == MrktSimControl.AllID.ToString())
			{
				return;
			}

			DataRow row = this.NewRow();

			row[rowKeyName] = rowID;
			row[headerText] = name;

			this.Rows.Add(row);

			refreshValues();
		}

		private void changeRow(DataRow rowRow)
		{
			DataRow row = this.Rows.Find(rowRow[rowKeyName]);

			if( row != null && row[headerText].ToString() != rowRow[rowName].ToString())
			{
				row[headerText] = rowRow[rowName];
			}
		}

		private void deleteRow(DataRow rowRow)
		{
			DataRow row = this.Rows.Find(rowRow[rowKeyName]);

			if(row != null)
			{
				row.Delete();
			}
		}

		// column operations
		private void addColumn(DataRow colRow)
		{
			string colkey = colRow[colKeyName].ToString();

			if (colkey == MrktSimControl.AllID.ToString())
			{
				return;
			}
			
			DataColumn col = valTable.Columns[this.TableName];

			//  column for each column
			DataColumn colCol = new DataColumn();

			colCol.ColumnName = colkey;
			colCol.DataType = col.DataType;

			this.Columns.Add(colCol);

			refreshValues();
		}

		private void deleteColumn(DataRow colRow)
		{
			DataColumn col = this.Columns[colRow[colKeyName].ToString()];
			this.Columns.Remove(col);	
		}

		// set the value
		private void setValue(DataRow valRow)
		{	
			string rowID = valRow[valRowKeyName].ToString();
			string colID = valRow[valColKeyName].ToString();

			
			DataRow row = this.Rows.Find(rowID);

			if (row != null)
				row[colID] = valRow[this.TableName];
		}

        void rowTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            stale = true;
        }
		
		bool stale = false;
		private void dataRowChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if(suspend)
			{

				if (e.Action == DataRowAction.Add ||
                    e.Action == DataRowAction.Delete ||
                    e.Action == DataRowAction.Commit )
				{
					stale = true;
				}

				return;
			}

			switch(e.Action)
			{
				case DataRowAction.Add:
					addRow(e.Row);
					break;

				case DataRowAction.Change:
					changeRow(e.Row);
					break;

				case DataRowAction.Delete:
					deleteRow(e.Row);
					break;

				case DataRowAction.Rollback:
					// nothing doing
					break;
			}
		}
	
		private void dataColChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if(suspend)
			{

				if (e.Action == DataRowAction.Add ||
					e.Action == DataRowAction.Delete ||
                    e.Action == DataRowAction.Commit)
				{
					stale = true;
				}

				return;
			}


			switch(e.Action)
			{
				case DataRowAction.Add:
					addColumn(e.Row);
					break;

				case DataRowAction.Delete:
					deleteColumn(e.Row);
					break;

				case DataRowAction.Rollback:
					// nothing doing
					break;
			}

			if (DataMatrixColumnChanged != null)
				DataMatrixColumnChanged();
		}

		private void dataValueChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if(suspend)
				return;

			refreshValues();
		}
	}
}
