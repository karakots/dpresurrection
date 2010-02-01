using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using MrktSimDb;

using ErrorInterface;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ExcelReader
	{
		private static string excelConnectionOptions()
		{
			string strOpts="";
//			if (this.MixedData ==true)
//				strOpts += "Imex=2;";
//			if (this.Headers==true)
//				strOpts += "HDR=Yes;";
//			else	
//				strOpts += "HDR=No;";
			return strOpts;
		}

		private static string excelConnection(string fileName)
		{
			return
				@"Provider=Microsoft.Jet.OLEDB.4.0;" + 
				@"Data Source=" + fileName  + ";" + 
				@"Extended Properties=" + Convert.ToChar(34).ToString() + 
				@"Excel 8.0;"+ excelConnectionOptions() + Convert.ToChar(34).ToString(); 
		}

		public static ErrorList ReadTable(string workbookPath, string table_name, int numColumns, bool reportMissingSheet, out DataTable table)
		{
			string col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(numColumns,1);
			return ReadTable(workbookPath, table_name, col, reportMissingSheet, out table);
		}

		public static string CheckIfFileOpen(string workbookPath)
		{
			System.IO.FileStream stream = null;

			try
			{
				System.IO.FileInfo info = new System.IO.FileInfo(workbookPath);

				stream = info.Open(System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None);
			}
			catch(Exception oops)
			{
				return oops.Message;
			}

			stream.Close();

			return null;
		}

		public static ErrorList ReadTable(string workbookPath, string table_name, string column, bool reportMissingSheet, out DataTable table)
		{
			table = new DataTable(table_name);
			ErrorList errors = new ErrorList();
			OleDbConnection oleConn = null;
			// open workbook
			try
			{
				using (oleConn = new OleDbConnection(excelConnection(workbookPath)))
				{
					// want to  read from spreadsheet

					OleDbDataAdapter oleAdapter = new OleDbDataAdapter();   
					oleAdapter.SelectCommand = new OleDbCommand(
						@"SELECT * FROM [" 
						+ table_name
						+ "$" + "A3:" + column + "65536"
						+ "]", oleConn);   

					//oleAdapter.FillSchema(table, System.Data.SchemaType.Source); 
			
					oleAdapter.Fill(table);

					oleConn.Close();
				}
			}
			catch(Exception e)
			{
				if (reportMissingSheet)
				{
					errors.addError(oleConn, e.Source, e.Message);
				}

				if (oleConn != null)
				{
					oleConn.Close();
					oleConn.Dispose();
				}

				return errors;
			}

			// check for null values

			foreach(DataRow testRow in table.Select("","",DataViewRowState.CurrentRows))
			{
				if ( (testRow[0] == null) ||
					(testRow[0].GetType() == DBNull.Value.GetType()))
				{
					testRow.Delete();
				}
			}

			if (table.Select("","",DataViewRowState.CurrentRows).Length == 0)
			{
				table = null;
			}
			else
			{

				// clean out the apostrophes - they screw up queries

				foreach(DataColumn col in table.Columns)
				{
					if (col.DataType != typeof(string))
						continue;

					foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
					{
						row[col] = row[col].ToString().Replace("'","");
					}
				}
			}
				
			return errors;
		}

		public static ErrorList ReadCell(string workbookPath, string table_name, string start,  string end, out ArrayList list)
		{
			list = null;

			ErrorList errors = new ErrorList();

			OleDbConnection oleConn = null;

			// open workbook
			try
			{
				using (oleConn = new OleDbConnection(excelConnection(workbookPath)))
				{
			
					OleDbCommand aCommand = new OleDbCommand(@"SELECT * FROM [" + table_name + "$" + start + ":" + end + "]", oleConn);  
					aCommand.CommandTimeout = 600;

					oleConn.Open();
			
					OleDbDataReader dataReader = aCommand.ExecuteReader(CommandBehavior.CloseConnection);
			

					string what = dataReader.GetDataTypeName(0);

					list = new ArrayList();

					while(dataReader.Read())
					{
						list.Add(dataReader.GetValue(0));
					}

					dataReader.Close();
				}
			}
			catch(Exception e)
			{
				errors.addError(oleConn, e.Source, e.Message);

				if (oleConn != null)
				{
					oleConn.Close();
					oleConn.Dispose();
				}
			}

			return errors;
		}
	}

	/*public class ExcelPlanReader : PlanReader
	{
		public ExcelPlanReader(ModelDb db, string inBaseName) : base(db, inBaseName)
		{
		}

		public ErrorList CreatePlan(string workbookPath , PlanType planType)
		{
			return new ErrorList();
		}
	}*/
}
