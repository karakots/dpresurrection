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
using MediaLibrary;
using BusinessLogic;

/// <summary>
/// AvailableMediaListMaker creates a table displaying a particular type of media in a plan.
/// </summary>
public class AvailableMediaListMaker : TableInjector
{
    private string borderStyle = "solid 1px #000";
    private string typeRowBorderStyle = "solid 1px #000";

    private string row1Color = "#CCFFCC";
    private string row1Height = "26px";
    private string row1FontSize = "11pt";

    private string row2Color = "#DDDDDD";
    private string row2Height = "18px";
    private string row2FontSize = "8pt";

    private MediaPlan plan;
    private UserMedia userMedia;
    private string mediaType;
    private int adOptionID;
    private double option_cost_mod;
    private string region;
    private bool isTargeted;

    private double minRating;
    private double maxRating;

    private Dictionary<MediaVehicle, double> vehiclesList;

    private AllocationService media_rater;

    public AvailableMediaListMaker(MediaPlan plan, string mediaType, UserMedia userMedia, int adOptionID, double adOptionCostModifer, string region)
        : base(6, 0.03, 1, 1, 1, 1, 0.4)
    {
        this.plan = plan;
        this.userMedia = userMedia;

        if( mediaType == "Yellow Pages" )
        {
            mediaType = "Yellowpages";
        }

        this.mediaType = mediaType;
        this.adOptionID = adOptionID;
        this.option_cost_mod = adOptionCostModifer; 
        this.region = region;
        this.vehiclesList = null;
        this.minimumTableWidth = 600;
        if (mediaType.ToLower() == "internet")
        {
            this.isTargeted = true;
        }

        media_rater = new AllocationService();
    }

    /// <summary>
    /// Creates the table and adds it to the given table cell.
    /// </summary>
    /// <returns></returns>
    public void AddAvailableMediaListHTML( HtmlTableCell targetDiv, string sortCol, out List<MediaVehicle> sortedVehicles, out List<double> scores, 
        ref int startPage, int itemsPerPage, out int totalPages, out int totalVehicles )
    {
        targetDiv.Controls.Clear();

        Table table = CreateTable();
        table.Style.Add("border", borderStyle);

        AddHeaderRows( table, sortCol );

        Dictionary<MediaVehicle, double> itemsOfType = GetVehiclesList();
        totalVehicles = itemsOfType.Count;
        totalPages = (int)Math.Ceiling( (double)itemsOfType.Count / (double)itemsPerPage );

        sortedVehicles = SortVehiclesList( itemsOfType, out scores, sortCol, startPage, itemsPerPage );

        if (itemsOfType.Count > 0)
        {
            for( int row = 0; row < sortedVehicles.Count; row++ ) {
                AddMediaVehicleRow( table, row, sortedVehicles[ row ], scores[ row ] );
            }
        }
        else
        {
            AddNoVehiclesRow(table);
        }

        targetDiv.Controls.Add(table);
    }

    /// <summary>
    /// Returns the list of all media items of the current type in the current plan and their corresponding ratings.
    /// </summary>
    /// <returns></returns>
    public Dictionary<MediaVehicle, double> GetVehiclesList()
    {
        if (this.vehiclesList != null)
        {
            // don't repeat the expensive code below if we did it already
            return this.vehiclesList;
        }

        vehiclesList = media_rater.GetRatedVehicles(this.mediaType, this.region, this.adOptionID, plan, false, out minRating, out maxRating);

         //remove any items already in the plan
        List<MediaVehicle> vehiclesToRemove = new List<MediaVehicle>();
        foreach( MediaVehicle mv in vehiclesList.Keys ) {
            if( plan.GetMediaItem( mv.Guid ) != null ) {
                vehiclesToRemove.Add( mv );
            }
        }
        foreach( MediaVehicle remMv in vehiclesToRemove ) {
            vehiclesList.Remove( remMv );
        }

        return vehiclesList;
    }

    public Dictionary<SimpleOption, double> GetProminenceList()
    {
        Dictionary<SimpleOption, double> rval = media_rater.GetRatedProminence(MediaVehicle.GetType(mediaType), plan);
        return rval;
    }

    protected List<MediaVehicle> SortVehiclesList( Dictionary<MediaVehicle, double> vehiclesList, out List<double> ratings, string sortKey, int startPage, int itemsPerPage ) {
        // should probably use a SortedDictionary for this purpose
        List<MediaVehicle> vehicles = new List<MediaVehicle>();
        List<double> scores = new List<double>();
        foreach( MediaVehicle mv in vehiclesList.Keys ) {
            vehicles.Add( mv );
            scores.Add( vehiclesList[ mv ] );
        }

        // sort the data
        MediaVehicle[] sortedVs = new MediaVehicle[ vehicles.Count ];
        double[] sortedScores = new double[ vehicles.Count ];
        string[] sortedNames = new string[ vehicles.Count ];
        double[] sortedPrices = new double[ vehicles.Count ];
        string[] sortedNames2 = new string[ vehicles.Count ];
        double[] sortedPrices2 = new double[ vehicles.Count ];
        vehicles.CopyTo( sortedVs );
        scores.CopyTo( sortedScores );
        for( int i = 0; i < vehicles.Count; i++ ) {
            sortedNames[ i ] = sortedVs[ i ].Vehicle;
            sortedPrices[ i ] = Utils.GetSpotPrice( sortedVs[ i ].Type.ToString(), sortedVs[ i ].CPM, this.option_cost_mod, sortedVs[ i ].Size / 1000, 1 );
            sortedNames2[ i ] = sortedNames[ i ];
            sortedPrices2[ i ] = sortedPrices[ i ];
        }

        if( sortKey == "name" ) {
            // sort by name
            Array.Sort( sortedNames, sortedVs );
            Array.Sort( sortedNames2, sortedScores );
        }
        else if( sortKey == "price" ) {
            Array.Sort( sortedPrices, sortedVs );
            Array.Sort( sortedPrices2, sortedScores );
            Array.Reverse( sortedVs );
            Array.Reverse( sortedScores );
        }
        else {
            // sort by score
            Array.Sort( sortedScores, sortedVs );
            Array.Reverse( sortedScores );
            Array.Reverse( sortedVs );
        }

        // set the return values
        List<MediaVehicle> sortedVehicles = new List<MediaVehicle>();
        ratings = new List<double>();

        int nStart = (startPage - 1) * itemsPerPage;
        if( nStart >= vehicles.Count ) {
            nStart = 0;
            startPage = 1;     // this is a ref value that is used to set the UI
        }
        int nShow = (int)Math.Min( vehicles.Count - nStart, itemsPerPage );

        for( int i = nStart; i < nStart + nShow; i++ ) {
            sortedVs[ i ].Rating = sortedScores[ i ];
            sortedVehicles.Add( sortedVs[ i ] );
            ratings.Add( sortedScores[ i ] );
        }

        return sortedVehicles;
    }

    /// <summary>
    /// Adds a row for a media vehicle.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="row"></param>
    /// <param name="mediaItem"></param>
    private void AddMediaVehicleRow( Table table, int row, MediaVehicle mediaItem, double rating ) {

        TableRow vrow = new TableRow();
        TableCell vc1 = new TableCell();
        TableCell vc2 = new TableCell();
        TableCell vc3 = new TableCell();
        TableCell vc4 = new TableCell();
        TableCell vc6 = new TableCell();

        vc1.Text = String.Format( "<input type=\"checkbox\" name=\"Add-{0}\" >", mediaItem.Guid.ToString() );
        vc1.Style.Add( "padding-left", "10px;" );
        vc1.Style.Add( "height", "22px;" );
        vc1.Style.Add( "max-width", "30px;" );

        vc2.Text = String.Format( "{0:f0}", AllocationService.DisplayRating( rating, this.minRating, this.maxRating ) );
        vc3.Text = Utils.FixSubtypeName( mediaItem.SubType, mediaItem.Type, mediaItem.Vehicle ).ToUpper();
        vc3.Style.Add( "font-size", "8pt;" );
        vc4.Text = Utils.VehicleInfoLink( mediaItem );

        // get the price of an ad
        if( mediaItem.GetOptions().ContainsKey( this.adOptionID ) ) {
            double size = mediaItem.Size;
            double spotPrice = Utils.GetSpotPrice( mediaItem.Type.ToString(), mediaItem.CPM, this.option_cost_mod, mediaItem.Size / 1000, 1 );

            if( spotPrice > 1 ) {
                if( spotPrice > 1000 ) {
                    vc6.Text = String.Format( "$ {0:0,0}", spotPrice );
                }
                else {
                    vc6.Text = String.Format( "$ {0:0}", spotPrice );
                }
            }
            else {
                if( spotPrice * 1000 > 1000 ) {
                    vc6.Text = String.Format( "$ {0:0,0} (1000)", spotPrice * 1000 );
                }
                else {
                    vc6.Text = String.Format( "$ {0:0} (1000)", spotPrice * 1000 );
                }
            }
        }
        else {
            vc6.Text = "?-bad-ad-opt";
        }

        vc6.ColumnSpan = 2;
        vc6.Style.Add( "text-align", "right" );
        vc6.Style.Add( "padding-right", "10px;" );

        vrow.Cells.Add( vc1 );
        vrow.Cells.Add( vc2 );
        vrow.Cells.Add( vc3 );
        vrow.Cells.Add( vc4 );
        vrow.Cells.Add( vc6 );
        table.Rows.Add( vrow );
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

    private void AddHeaderRows( Table table, string sortKey ) {
        TableRow row1 = new TableRow();
        TableCell titleCell = new TableCell();

        string add_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "AddItem", "" );
        string addButtonTag = "<img src=\"images/Add.gif\" height=20 width-120 border=0 style=\"position:relative;top:3px;\">";

        titleCell.Text = String.Format( "<span style=\"position:relative;top:-3px;font-size:11pt;\" >Available Media</span>&nbsp;&nbsp;<a href=# onclick='{0}' >{1}</a>", add_item_JS, addButtonTag );

        titleCell.ColumnSpan = 3;

        TableCell regionCell = new TableCell();
        Label lb1 = new Label();
        lb1.Text = "Region:";
        DropDownList ddl1 = new DropDownList();
        ddl1.Items.Add( "(select a region)" );
        regionCell.Controls.Add( lb1 );
        regionCell.Controls.Add( ddl1 );
        regionCell.ColumnSpan = 2;

        TableCell prominenceCell = new TableCell();
        Label lb2 = new Label();
        lb2.Text = "Prominence:";
        DropDownList ddl2 = new DropDownList();
        ddl2.Items.Add( "(select a prominence)" );
        prominenceCell.Controls.Add( lb2 );
        prominenceCell.Controls.Add( ddl2 );
        prominenceCell.ColumnSpan = 2;

        row1.Cells.Add( titleCell );
        //row1.Cells.Add( regionCell );
        //row1.Cells.Add( prominenceCell );
        //table.Rows.Add( row1 );

        SetRow1CellStyle( titleCell );
        SetRow1CellStyle( regionCell );
        SetRow1CellStyle( prominenceCell );
        titleCell.Style.Add( "border-left", borderStyle );
        regionCell.Style[ "font-size" ] = "9pt";

        prominenceCell.Style.Add( "border-right", borderStyle );
        prominenceCell.Style.Add( "text-align", "right" );
        prominenceCell.Style[ "font-size" ] = "9pt";

        TableRow row2 = new TableRow();
        TableCell hc1 = new TableCell();
        TableCell hc2 = new TableCell();
        TableCell hc3 = new TableCell();
        TableCell hc4 = new TableCell();
        TableCell hc6 = new TableCell();

        string sort_by_name_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByNameLink", "" );
        string sort_by_score_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByScoreLink", "" );
        string sort_by_price_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByPriceLink", "" );

        string sortUpImg = "<img src=\"images/Sort-Up.gif\" style=\"position:relative;top:2px;left:2px;\" >";
        string sortDownImg = "<img src=\"images/Sort-Down.gif\" style=\"position:relative;top:2px;left:2px;\" >";

        hc1.Text = "&nbsp;";

        if( sortKey == "score" ) {
            hc2.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_score_JS, "Rating", sortDownImg );
        }
        else {
            hc2.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_score_JS, "Rating" );
        }

        hc3.Text = "Type";

        if( sortKey == "name" ) {
            hc4.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_name_JS, "Media", sortUpImg );
        }
        else {
            hc4.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_name_JS, "Media" );
        }

        if( sortKey == "price" ) {
            hc6.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_price_JS, "Price&nbsp;per&nbsp;Ad", sortDownImg );
        }
        else {
            hc6.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_price_JS, "Price&nbsp;per&nbsp;Ad" );
        }
        hc6.ColumnSpan = 2;

        SetRow2CellStyle( hc1 );
        SetRow2CellStyle( hc2 );
        SetRow2CellStyle( hc3 );
        SetRow2CellStyle( hc4 );
        SetRow2CellStyle( hc6 );

        hc1.Style.Add( "border-left", borderStyle );
        hc1.Style.Add( "padding-left", "10px" );

        hc6.Style.Add( "border-right", borderStyle );
        hc6.Style.Add( "padding-right", "10px" );
        hc6.Style.Add( "text-align", "right" );

        row2.Cells.Add( hc1 );
        row2.Cells.Add( hc2 );
        row2.Cells.Add( hc3 );
        row2.Cells.Add( hc4 );
        row2.Cells.Add( hc6 );

        table.Rows.Add( row2 );
    }

    private void SetRow1CellStyle( TableCell cell ) {
        cell.Style.Add( "border-top", borderStyle );
        cell.Style.Add( "border-bottom", borderStyle );
        cell.Style.Add( "height", row1Height );
        cell.Style.Add( "background-color", row1Color );
        cell.Style.Add( "font-size", row1FontSize );
        cell.Style.Add( "padding-left", "10px" );
        cell.Style.Add( "padding-right", "10px" );
    }

    private void SetRow2CellStyle( TableCell cell ) {
        cell.Style.Add( "border-top", borderStyle );
        cell.Style.Add( "border-bottom", borderStyle );
        cell.Style.Add( "height", row2Height );
        cell.Style.Add( "background-color", row2Color );
        cell.Style.Add( "font-size", row2FontSize );
    }
}
