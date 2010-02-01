using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WebLibrary;

/// <summary>
/// MediaDetailsMaker creates a table displaying a particular type of media in a plan.
/// </summary>
public class MediaDetailsMaker : TableInjector
{
    private string borderStyle = "solid 1px #000";
    private string typeRowBorderStyle = "solid 1px #000";

    private string row1Color = "#DDDDDD";
    private string row1Height = "20px";
    private string row1FontSize = "9pt";

    private MediaPlan plan;
    private UserMedia userMedia;
    private string mediaType;

    private bool isTargeted;

    public MediaDetailsMaker( MediaPlan plan, string mediaType, UserMedia userMedia )
        : base( 6, 0.03, 1, 1, 1, 1, 0.4 ) {
        this.plan = plan;
        this.userMedia = userMedia;
        this.mediaType = mediaType;
        this.isTargeted = false;
        if( mediaType.ToLower() == "internet" ) {
            this.isTargeted = true;
        }
    }

    /// <summary>
    /// Creates and returns the table.
    /// </summary>
    /// <returns></returns>
    public void AddDetailsTableHTML( HtmlTableCell targetDiv ) {
        targetDiv.Controls.Clear();

        Table table = CreateTable();
        table.Style.Add( "border", borderStyle );

        AddHeaderRows( table );

        List<MediaItem> itemsOfType = GetTypeItems();

        this.nRows = itemsOfType.Count;

        if( this.nRows > 0 ) {
            for( int row = 0; row < this.nRows; row++ ) {
                AddMediaVehicleRow( table, row, itemsOfType[ row ] );
            }
        }
        else {
            AddNoVehiclesRow( table );
        }

        targetDiv.Controls.Add( table );
    }

    /// <summary>
    /// Returns the list of all media items of the current type in the current plan.
    /// </summary>
    /// <returns></returns>
    public List<MediaItem> GetTypeItems() {
        return plan.GetTypeItems(mediaType);
    }

    /// <summary>
    /// Adds a row ro indicate there are no vehicles of this type.
    /// </summary>
    /// <param name="table"></param>
    private void AddNoVehiclesRow( Table table ) {
        TableRow vrow = new TableRow();
        TableCell vc1 = new TableCell();
        TableCell vc2 = new TableCell();

        vc1.Text = "&nbsp;";
        vc2.Text = "None";

        vc2.ColumnSpan = 5;

        vrow.Cells.Add( vc1 );
        vrow.Cells.Add( vc2 );
        table.Rows.Add( vrow );
    }

    /// <summary>
    /// Adds a row for a media vehicle.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="row"></param>
    /// <param name="mediaItem"></param>
    private void AddMediaVehicleRow( Table table, int row, MediaItem mediaItem ) {

        string set_timing_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SetTiming", row.ToString() );
        string set_prominence_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SetProminence", row.ToString() );
        string set_targeting_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SetTargeting", row.ToString() );

        TableRow vrow = new TableRow();
        TableCell vc1 = new TableCell();
        TableCell vc2 = new TableCell();
        TableCell vc3 = new TableCell();
        TableCell vc4 = new TableCell();
        TableCell vc5 = new TableCell();
        TableCell vc6 = new TableCell();

        vc1.Text = String.Format( "<input type=\"checkbox\" name=\"Remove-{0}\" >", row );
        vc1.Style.Add( "padding-left", "10px;" );
        vc1.Style.Add( "height", "22px;" );
        vc1.Style.Add( "max-width", "30px;" );

        //vc2.Text = mediaItem.VehicleName;
        vc2.Text = Utils.VehicleInfoLink( mediaItem );

        if( this.isTargeted == false ) {
            vc3.Text = mediaItem.Region;

            //vc4.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_prominence_JS, mediaItem.AdOptionName );
            //vc5.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_timing_JS, mediaItem.GetTimingSummary() );
            vc4.Text = String.Format( "<a href=# onclick='{0}' >{1}&nbsp;&nbsp;-&nbsp;&nbsp;{2}</a>", set_prominence_JS, mediaItem.AdOptionName, mediaItem.GetTimingSummary() );
            vc4.ColumnSpan = 2;
            //vc5.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_timing_JS, mediaItem.GetTimingSummary() );

            vc6.Text = Utils.FormatDollarAmount( mediaItem.TotalPrice );
        }
        else {
            //vc3.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_prominence_JS, mediaItem.AdOptionName );
            //vc4.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_timing_JS, mediaItem.GetTimingSummary() );
            vc3.Text = String.Format( "<a href=# onclick='{0}' >{1}&nbsp;&nbsp;-&nbsp;&nbsp;{2}</a>", set_prominence_JS, mediaItem.AdOptionName, mediaItem.GetTimingSummary() );
            vc3.ColumnSpan = 2;
            //vc4.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_timing_JS, mediaItem.GetTimingSummary() );

            string targInfo = String.Format( "{0} ($ x {1:f1})", mediaItem.Region, mediaItem.TargetingPriceMultiplier );
            vc4.Text = String.Format( "<a href=# onclick='{0}' >{1}</a>", set_targeting_JS, targInfo );

            vc6.Text = Utils.FormatDollarAmount( mediaItem.TotalPrice );
        }

        vc6.Style.Add( "text-align", "right" );
        vc6.Style.Add( "padding-right", "10px;" );

        vrow.Cells.Add( vc1 );
        vrow.Cells.Add( vc2 );
        vrow.Cells.Add( vc3 );
        vrow.Cells.Add( vc4 );
        //vrow.Cells.Add( vc5 );
        vrow.Cells.Add( vc6 );
        table.Rows.Add( vrow );
    }

    private void AddHeaderRows( Table table ) {
        TableRow row1 = new TableRow();
        TableCell hc1 = new TableCell();
        TableCell hc2 = new TableCell();
        TableCell hc3 = new TableCell();
        TableCell hc4 = new TableCell();
        TableCell hc5 = new TableCell();
        TableCell hc6 = new TableCell();

        string remove_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "RemoveItem", "" );
        string removeButtonTag = "<img src=\"images/Remove.gif\" height=20 width-120 border=0 style=\"position:relative;top:3px;\">";
        
        hc1.Text = "&nbsp;";
        hc2.Text = String.Format( "<span style=\"position:relative;top:-3px;font-size:11pt;\" >Media</span>&nbsp;&nbsp;<a href=# onclick='{0}' >{1}</a>", remove_item_JS, removeButtonTag );

        if( this.isTargeted == false ) {
            hc2.ColumnSpan = 2;
            hc3.Text = "Prominence and Timing";
            hc3.ColumnSpan = 2;
            //hc4.Text = "Timing";
            hc5.Text = "Cost";
        }
        else {
            // targeted media (like internet)
            hc3.Text = "Prominence and Timing";
            //hc4.Text = "Timing";
            hc3.ColumnSpan = 2;
            hc5.Text = "Targeting";
            hc6.Text = "Cost";
        }

        SetRow1CellStyle( hc1 );
        SetRow1CellStyle( hc2 );
        SetRow1CellStyle( hc3 );
        SetRow1CellStyle( hc4 );
        SetRow1CellStyle( hc5 );
        SetRow1CellStyle( hc6 );

        hc1.Style.Add( "border-left", borderStyle );
        hc1.Style.Add( "padding-left", "10px" );

        if( this.isTargeted == false ) {
            hc5.Style.Add( "border-right", borderStyle );
            hc5.Style.Add( "padding-right", "10px" );
            hc5.Style.Add( "text-align", "right" );
        }
        else {
            hc6.Style.Add( "border-right", borderStyle );
            hc6.Style.Add( "padding-right", "10px" );
            hc6.Style.Add( "text-align", "right" );
        }

        row1.Cells.Add( hc1 );
        row1.Cells.Add( hc2 );
        row1.Cells.Add( hc3 );
        //row1.Cells.Add( hc4 );
        row1.Cells.Add( hc5 );
        if( this.isTargeted == true ) {
            row1.Cells.Add( hc6 );
        }

        table.Rows.Add( row1 );
    }

    private void SetRow1CellStyle( TableCell cell ) {
        cell.Style.Add( "border-top", borderStyle );
        cell.Style.Add( "border-bottom", borderStyle );
        cell.Style.Add( "height", row1Height );
        cell.Style.Add( "background-color", row1Color );
        cell.Style.Add( "font-size", row1FontSize );
    }
}
