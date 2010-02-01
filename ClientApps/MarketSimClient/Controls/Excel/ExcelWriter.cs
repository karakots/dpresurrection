using System;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for ExcelWriter.
	/// </summary>
	public class ExcelWriter
	{
		public ExcelWriter()
		{
		}

		private void NAR(object o)
		{
			try 
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
			}
			catch {}
			finally 
			{
				o = null;
			}
		}

		public void Start(string Filename, string sheet_name)
		{
			xlApp = new Microsoft.Office.Interop.Excel.Application();
			wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
			ws = (Worksheet)wb.Worksheets[1];
			ws.Name = sheet_name;
			name = Filename;
		}

		public void Quit()
		{
			wb.SaveAs(name, XlFileFormat.xlWorkbookNormal,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
			
			// really really get rid of this
			Kill();
		}

		public void Kill()
		{
			NAR(ws);
			wb.Close(false,Missing.Value,Missing.Value);
			NAR(wb);
			xlApp.Quit();
			NAR(xlApp);
			GC.Collect();
		}

		public void FillCell(int row, int col, string val)
		{
			((Range)ws.Cells[row, col]).Value2 = val;
		}

		public void MergeCells(int row1, int col1, int row2, int col2)
		{
			ws.get_Range(ws.Cells[row1,col1],ws.Cells[row2,col2]).Merge(Missing.Value);
		}

		public void ColumnAutofit(int col)
		{
			((Range)ws.Cells[1, col]).EntireColumn.AutoFit();
		}

		public void ColumnWidth(int col, double width)
		{
			((Range)ws.Cells[1, col]).EntireColumn.ColumnWidth = width;
		}

		public void RowAutofit(int row)
		{
			((Range)ws.Cells[row, 1]).EntireRow.AutoFit();
		}

		public void RowHeight(int row, double height)
		{
			((Range)ws.Cells[row, 1]).EntireRow.RowHeight = height;
		}

		public void RowBold(int row, bool bold)
		{
			((Range)ws.Cells[row, 1]).EntireRow.Font.Bold = bold;
		}

		public void ColumnBold(int col, bool bold)
		{
			((Range)ws.Cells[1, col]).EntireColumn.Font.Bold = bold;
		}

		public void CellBold(int row, int col, bool bold)
		{
			((Range)ws.Cells[row, col]).Font.Bold = bold;
		}

		public void RowFontSize(int row, int size)
		{
			((Range)ws.Cells[row, 1]).EntireRow.Font.Size = size;
		}

		public void ColumnFontSize(int col, int size)
		{
			((Range)ws.Cells[1, col]).EntireColumn.Font.Size = size;
		}

		public void CellFontSize(int row, int col, int size)
		{
			((Range)ws.Cells[row, col]).Font.Size = size;
		}

		public void RowBackColor(int row, int colorIndex)
		{
			((Range)ws.Cells[row, 1]).EntireRow.Interior.ColorIndex = colorIndex;
		}

		public void ColumnBackColor(int col, int colorIndex)
		{
			((Range)ws.Cells[1, col]).EntireColumn.Interior.ColorIndex = colorIndex;
		}

		public void CellBackColor(int row, int col, int colorIndex)
		{
			((Range)ws.Cells[row, col]).Interior.ColorIndex = colorIndex;
		}

		public void RowCenterAlign(int row)
		{
			((Range)ws.Cells[row, 1]).EntireRow.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
		}

		public void ColumnCenterAlign(int col)
		{
			((Range)ws.Cells[1, col]).EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
		}

		public void CellCenterAlign(int row, int col)
		{
			((Range)ws.Cells[row, col]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
		}

		public void RowRightAlign(int row)
		{
			((Range)ws.Cells[row, 1]).EntireRow.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
		}

		public void ColumnRightAlign(int col)
		{
			((Range)ws.Cells[1, col]).EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
		}

		public void CellRightAlign(int row, int col)
		{
			((Range)ws.Cells[row, col]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
		}

		public void RowLeftAlign(int row)
		{
			((Range)ws.Cells[row, 1]).EntireRow.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
		}

		public void ColumnLeftAlign(int col)
		{
			((Range)ws.Cells[1, col]).EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
		}

		public void CellLeftAlign(int row, int col)
		{
			((Range)ws.Cells[row, col]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
		}

		public void RowNumberFormat(int row, string format)
		{
			((Range)ws.Cells[row, 1]).EntireRow.NumberFormat = format;
		}

		public void ColumnNumberFormat(int col, string format)
		{
			((Range)ws.Cells[1, col]).EntireColumn.NumberFormat = format;
		}

		public void CellNumberFormat(int row, int col, string format)
		{
			((Range)ws.Cells[row, col]).NumberFormat = format;
		}

		public void NewSheet(string sheet_name)
		{
			ws = (Worksheet)wb.Worksheets.Add(Missing.Value,Missing.Value,Missing.Value,Missing.Value);
			ws.Name = sheet_name;
		}

		private Microsoft.Office.Interop.Excel.Application xlApp;
		private Workbook wb;
		private Worksheet ws;
		private string name;
		
	}
}
