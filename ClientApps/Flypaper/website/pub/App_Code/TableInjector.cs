using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for TableInjector
/// </summary>
public class TableInjector
{
    protected int minimumTableWidth = 800;

    protected int nCols = 7;
    protected int nRows = 5;

    protected List<int> minimumColWidth;
    protected List<int> columnWidthPercent;

    public TableInjector() {
    }

    public TableInjector( int nCols, params double[] columnWidths ) : this() {
        this.nCols = nCols;
        SetColumnWidths( columnWidths );
    }

    /// <summary>
    /// Set the relative widths of the columns.  
    /// </summary>
    /// <param name="widths"></param>
    public void SetColumnWidths( params double[] widths ) {

        this.nCols = widths.Length;

        double totWid = 0;
        for( int c = 0; c < this.nCols; c++ ) {
            totWid += widths[ c ];
        }

        minimumColWidth = new List<int>();
        columnWidthPercent = new List<int>();
        for( int c = 0; c < this.nCols; c++ ) {
            columnWidthPercent.Add( (int)Math.Round( 100.0 * widths[ c ] / totWid ) );
            minimumColWidth.Add( (int)Math.Round( (double)minimumTableWidth * widths[ c ] / totWid ) );
        }
    }

    /// <summary>
    /// Creates and returns the table.
    /// </summary>
    /// <returns></returns>
    public void AddTableHTML( HtmlTableCell targetDiv ) {
        targetDiv.Controls.Clear();

        Table table = CreateTable();

        for( int row = 0; row < this.nRows; row++ ) {
            AddTableRow( table, row, new List<string>() );
        }

        targetDiv.Controls.Add( table );
    }

    /// <summary>
    /// Creates the table.
    /// </summary>
    /// <returns></returns>
    protected Table CreateTable() {
        Table table = new Table();
        table.CellPadding = 1;
        table.CellSpacing = 0;

        table.Style.Add( HtmlTextWriterStyle.FontFamily, "Arial, Verdana" );
        table.Style.Add( HtmlTextWriterStyle.FontSize, "9pt" );
        table.Style.Add( HtmlTextWriterStyle.Width, "99%" );
        table.Style.Add( HtmlTextWriterStyle.Color, "Black" );
        table.Style.Add( HtmlTextWriterStyle.BackgroundColor, "White" );

        return table;
    }

    /// <summary>
    /// Adds a row to the table.
    /// </summary>
    /// <returns></returns>
    protected TableRow AddTableRow( Table table, int rowNum, List<string> rowItemText ) {
        TableRow row = new TableRow();
        row.Style.Add( "height", "30px" );

        for( int colNum = 0; colNum < this.nCols; colNum++ ) {
            string txt = null;
            if( colNum < rowItemText.Count ){
                txt = rowItemText[ colNum ];
            }
            AddTableColumn( row, rowNum, colNum, txt );
        }
        table.Rows.Add( row );
        return row;
    }

    /// <summary>
    /// Adds a cell to the row.
    /// </summary>
    /// <returns></returns>
    protected TableCell AddTableColumn( TableRow row, int rowNum, int colNum, string cellText ) {
        TableCell cell = new TableCell();
        row.Cells.Add( cell );

        cell.Style.Add( "max-width", String.Format( "{0}%", columnWidthPercent[ colNum ] ) );
        cell.Style.Add( "width", String.Format( "{0}%", columnWidthPercent[ colNum ] ) );

        cell.Style.Add( "min-width", String.Format( "{0}px", minimumColWidth[ colNum ] ) );

        // testing!
        ////if( colNum == 0 && cellText == null ) {
        ////    cellText = "<input type=checkbox name=\"foo\"  style=\"position:relative;top:-2px;\" >";
        ////}

        ////if( cellText == null ) {
        ////    cellText = String.Format( "Sample{0}{1} Sample{0}{1} Sample{0}{1} Sample{0}{1}", rowNum, colNum );
        ////}

        ////if( colNum == 5 ) {
        ////    cellText = cellText.Replace( " ", "" );
        ////}

        cell.Text = String.Format( "<div style=\"max-height:18px; height:18px; overflow:hidden;position:relative;top:3px;\" >{0}</div>",
             cellText );

        return cell;
    }
}
