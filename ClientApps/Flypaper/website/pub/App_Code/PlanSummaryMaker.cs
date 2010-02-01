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

/// <summary>
/// PlanSummaryMaker creates a table displaying a summary of the media in a plan.
/// </summary>
public class PlanSummaryMaker : TableInjector
{
    private string borderStyle = "solid 1px #000";
    private string typeRowBorderStyle = "solid 1px #000";

    private string row1Color = "#CCFFCC";
    private string row1Height = "26px";
    private string row1FontSize = "8pt";

    private string row2Color = "#DDDDDD";
    private string row2Height = "20px";
    private string row2FontSize = "9pt";

    private MediaPlan plan;
    private UserMedia userMedia;
    //private bool isIE = false;

    public PlanSummaryMaker( MediaPlan plan, UserMedia userMedia )
        : base( 5, 0.5, 0.09, 0.65, 1.3, 1.0, 0.3 ) {
        this.plan = plan;
        this.userMedia = userMedia;
    }

    /// <summary>
    /// Creates and returns the table.
    /// </summary>
    /// <returns></returns>
    public void AddSummaryTableHTML( HtmlTableCell targetDiv, List<string> expandedTypes, string userAgent, string footer,
        ImageButton button, DropDownList addList ) {
        //return;
        targetDiv.Controls.Clear();
        //this.isIE = false;
        //if( userAgent != null && userAgent.IndexOf( "MSIE" ) != -1 ) {
        //    this.isIE = true;
        //}

        Table table = CreateTable();
        //table.Style.Add( "border", borderStyle );

        AddHeaderRows( table );

        List<int> media_types = plan.GetTypes();
        for( int index = 0; index < media_types.Count; index++ ) {
            int type_id = media_types[ index ];
            AddMediaTypeRow( table, type_id, index, expandedTypes );
            if( index < media_types.Count - 1 ) {
                AddSpacerRow( table );
            }
        }

        AddAddControlRow( table, button, addList );

        AddFooterRow( table, footer );

        //AddBigSpacerRow( table, media_types.Count );

        targetDiv.Controls.Add( table );
    }

    private void AddMediaTypeRow( Table table, int type_id, int row, List<string> expandedTypes ) {
        MediaVehicle.MediaType mediaType = Utils.MediaDatabase.GetTypeForID(type_id);
        bool expanded = expandedTypes.Contains( mediaType.ToString() );
        string expand_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "ExpandItem", mediaType.ToString() );
        string collapse_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "CollapseItem", mediaType.ToString() );
        string modify_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "Modifytem", mediaType.ToString() );

        List<string> colVals = new List<string>();
        string typName = mediaType.ToString();
        if( typName == "Yellowpages" ) {
            typName = "Yellow Pages";
        }
        colVals.Add( typName );

        bool hasPlans = true;
        if( this.plan.GetTypeItems( mediaType ) == null || this.plan.GetTypeItems( mediaType ).Count == 0 ) {
            hasPlans = false;
        }

        if( hasPlans == true ) {
            if( expanded == false ) {
                string expandButtonTag = "<img src=\"images/Expand.gif\" width=20 height=20 border=0 >";
                colVals.Add( String.Format( "<a href=\"#\" onclick='{0}'  style=\"font-size:8pt;position:relative;top:-2px;\" >{1}</a>", expand_item_JS, expandButtonTag ) );
            }
            else {
                string condenseButtonTag = "<img src=\"images/Condense.gif\" width=20 height=20 border=0 >";
                colVals.Add( String.Format( "<a href=\"#\" onclick='{0}'  style=\"font-size:8pt;position:relative;top:-2px;\" >{1}</a>", collapse_item_JS, condenseButtonTag ) );
            }
        }
        else {
            colVals.Add( "&nbsp;&nbsp;" );
        }

        string modButton = "images/Modify.gif";
        switch( mediaType ) {
            case MediaVehicle.MediaType.Internet:
                modButton = "images/Button-EditInternet.gif";
                break;
            case MediaVehicle.MediaType.Radio:
                modButton = "images/Button-EditRadio.gif";
                break;
            case MediaVehicle.MediaType.Yellowpages:
                modButton = "images/Button-EditYP.gif";
                break;
            case MediaVehicle.MediaType.Newspaper:
                modButton = "images/Button-EditNewspaper.gif";
                break;
            case MediaVehicle.MediaType.Magazine:
                modButton = "images/Button-EditMagazines.gif";
                break;
        }
        colVals.Add( String.Format( "<a href=\"#\" onclick='{0}' ><img src=\"{1}\" width=135 height=20 border=0  style=\"position:relative;top:-2px;\" ></a>", modify_item_JS, modButton ) );
        
        colVals.Add( "&nbsp;" + this.plan.GetTypeSummaryDescription( mediaType ) );

        colVals.Add( "" );

        double spending = this.plan.GetTypeSpending( mediaType );
        if( spending < 1000 ) {
            colVals.Add( String.Format( "$ {0:0}", this.plan.GetTypeSpending( mediaType ) ) );
        }
        else {
            colVals.Add( String.Format( "$ {0:0,0}", this.plan.GetTypeSpending( mediaType ) ) );
        }

        TableRow typeRow = AddTableRow( table, row, colVals );
        typeRow.Cells[ 3 ].ColumnSpan = 2;
        typeRow.Cells.RemoveAt( 4 );
        AddTypeRowBorders( typeRow, expanded );

        if( expanded ) {
            AddVehicleRows( table, mediaType );
        }
    }

    private void AddAddControlRow( Table table, ImageButton addButton, DropDownList typeList )
    {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.ColumnSpan = 4;

       // DropDownList typeList = new DropDownList();
       // typeList.ID = "AddTypeList";

        typeList.Items.Clear();

        foreach( string mType in Utils.AllMediaTypes ()) {
            bool showType = true;

            //??? how do we convert from media type codes to names???
             foreach( int typeCode in this.plan.GetTypes() ){
                 MediaVehicle.MediaType planMediaType = Utils.MediaDatabase.GetTypeForID( typeCode );
                 if( planMediaType.ToString() == mType ){
                     showType = false;
                 }
            }
             if( showType ) {
                 string dispName = mType;
                 if( dispName == "Yellowpages" ) {
                     dispName = "Yellow Pages";
                 }
                 ListItem item = new ListItem( dispName, mType.Replace( " ", "" ).ToLower() );
                 typeList.Items.Add( item );
             }
        }

        
        cell.Controls.Clear();
        if( typeList.Items.Count > 0 )
        {

            //ImageButton addButton = new ImageButton();
            //addButton.ID = "AddButton";
            addButton.ImageUrl = "images/Button-AddType.gif";
            addButton.Style.Add( "position", "relative" );
            addButton.Style.Add( "top", "4px" );
            addButton.Style.Add( "display", "inline" );
            addButton.OnClientClick = "__doPostBack( 'AddButton', 0 )";

            cell.Controls.Add( addButton );
            cell.Controls.Add( typeList );
        }
        else
        {
            addButton.Visible = false;
            typeList.Visible = false;
        }

        //cell.Style.Add( "width", "300px" );
        cell.Style.Add( "vertical-align", "top" );
        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    private void AddVehicleRows( Table table, MediaVehicle.MediaType mediaType ) {
        // get all of the items of the given type
        List<MediaItem> vehicles = plan.GetTypeItems(mediaType);

        for( int i = 0; i < vehicles.Count; i++ ) {
            TableRow vrow = new TableRow();
            TableCell vc1 = new TableCell();
            TableCell vc2 = new TableCell();
            TableCell vc3 = new TableCell();
            TableCell vc4 = new TableCell();
            TableCell vc5 = new TableCell();

            vc1.Text = Utils.VehicleInfoLink( vehicles[ i ] );
            string vsub = Utils.FixSubtypeName( vehicles[ i ].MediaSubtype.ToLower(), mediaType, vehicles[ i ].VehicleName );
            vc1.Text += String.Format( "<span style=\"font-size:8pt;font-style:italic;\"><br>&nbsp;&nbsp;&nbsp;&nbsp;-{0}</span>", vsub );

            vc1.ColumnSpan = 4;
            vc1.Style.Add( "padding-left", "25px" );
            vc1.Style.Add( "height", "22px" );

            int nAds = vehicles[ i ].SpotCount;

            // determine the start date for the ads
            List<List<WebLibrary.Timing.TimingDisplayItem>> tList = vehicles[ i ].GetTimingDisplay( this.plan.StartDate );
            DateTime adStart = this.plan.StartDate;
            bool setDate = false;
            foreach( List<WebLibrary.Timing.TimingDisplayItem> perOptionList in tList ) {
                WebLibrary.Timing.TimingDisplayItem firstItem = perOptionList[ 0 ];
                if( setDate == false ) {
                    adStart = new DateTime( firstItem.Year, firstItem.Month, firstItem.Day );
                    setDate = true;
                }
                else {
                    DateTime otherAdStart = new DateTime( firstItem.Year, firstItem.Month, firstItem.Day );
                    if( otherAdStart < adStart ) {
                        adStart = otherAdStart;
                    }
                }
            }

            string adDescription = "";
            if( mediaType == MediaVehicle.MediaType.Magazine ) {
                if( nAds == 1 ) {
                    adDescription = String.Format( "1 ad in the {0} issue", adStart.ToString( "MMM yyyy" ) );
                }
                else {
                    adDescription = String.Format( "Ads in {0} issues starting {1}", nAds, adStart.ToString( "MMM yyyy" ) );
                }
            }
            else if( mediaType == MediaVehicle.MediaType.Newspaper ) {
                if( nAds == 1 ) {
                    adDescription = String.Format( "1 ad in the {0} issue", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
                else {
                    adDescription = String.Format( "Ads in {0} issues starting {1}", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
            }
            else if( mediaType == MediaVehicle.MediaType.Yellowpages ) {
                if( nAds == 1 ) {
                    adDescription = String.Format( "1 ad in the {0}-{1} issue", adStart.Year, adStart.Year + 1 );
                }
                else {
                    adDescription = String.Format( "Ads in {0} issues starting {1}-{2}", nAds, adStart.Year, adStart.Year + 1 );
                }
            }
            else if( mediaType == MediaVehicle.MediaType.Radio ) {
                if( nAds == 1 ) {
                    adDescription = String.Format( "1 ad on {0}", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
                else {
                    adDescription = String.Format( "{0} ads starting {1}", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
            }
            else if( mediaType == MediaVehicle.MediaType.Internet ) {
                if( nAds == 1 ) {
                    adDescription = String.Format( "1 impression on {0}", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
                else {
                    adDescription = String.Format( "{0} impressions starting {1}", nAds, adStart.ToString( "MMM d, yyyy" ) );
                }
            }
            vc3.Text = adDescription;
            vc3.ColumnSpan = 2;
            vc3.Style.Add( "vertical-align", "top" );

            double vprice = vehicles[ i ].TotalPrice;
            if( vprice < 1000 ) {
                vc5.Text = String.Format( "$ {0:0}", vehicles[ i ].TotalPrice );
            }
            else {
                vc5.Text = String.Format( "$ {0:0,0}", vehicles[ i ].TotalPrice );
            }
            vc5.Style.Add( "text-align", "right" );
            vc5.Style.Add( "padding-right", "10px;" );
            vc5.Style.Add( "vertical-align", "top" );

            vc1.Style.Add( "border-left", borderStyle );
            vc5.Style.Add( "border-right", borderStyle );

            vrow.Cells.Add( vc1 );
            //vrow.Cells.Add( vc2 );
            vrow.Cells.Add( vc3 );
            //vrow.Cells.Add( vc4 );
            vrow.Cells.Add( vc5 );
            table.Rows.Add( vrow );

            vc1.Style.Add( "padding-top", "5px" );
            vc3.Style.Add( "padding-top", "5px" );
            vc5.Style.Add( "padding-top", "5px" );

            if( i == vehicles.Count - 1 ) {
                vc1.Style.Add( "border-bottom", typeRowBorderStyle );
                //vc2.Style.Add( "border-bottom", typeRowBorderStyle );
                vc3.Style.Add( "border-bottom", typeRowBorderStyle );
                vc4.Style.Add( "border-bottom", typeRowBorderStyle );
                vc5.Style.Add( "border-bottom", typeRowBorderStyle );

                vc1.Style.Add( "padding-bottom", "5px" );
                vc3.Style.Add( "padding-bottom", "5px" );
                vc4.Style.Add( "padding-bottom", "5px" );
                vc5.Style.Add( "padding-bottom", "5px" );
            }
        }
    }

    private void AddHeaderRows( Table table ) {

        TableRow row1 = new TableRow();
        TableCell infoCell = new TableCell();

        infoCell.Text = "Add or edit your media selections to create your plan. AdPlanit can also suggest an initial plan for your campaign.   " +
            "Once complete, test your plan to see your results";
        infoCell.ColumnSpan = 6;
        infoCell.Style.Add( "padding-bottom", "5px" );
        infoCell.Style.Add( "padding-top", "5px" );

        row1.Cells.Add( infoCell );
        table.Rows.Add( row1 );

        SetRow1CellStyle( infoCell );

        TableRow row2 = new TableRow();
        TableCell hc1 = new TableCell();
        TableCell hc1a = new TableCell();
        TableCell hc2 = new TableCell();
        //TableCell hc3 = new TableCell();
        TableCell hc4 = new TableCell();

        hc1.Text = "Type";
        hc1.ColumnSpan = 3;
        hc1a.Text = "&nbsp;";

        hc2.Text = "Advertising&nbsp;Description";
        hc2.ColumnSpan = 2;
        //hc3.Text = "Start Date";
        double psum = this.plan.SumOfItemBudgets;
        if( psum < 1000 ) {
            hc4.Text = String.Format( "Total&nbsp;&nbsp;${0:0}", this.plan.SumOfItemBudgets );
        }
        else {
            hc4.Text = String.Format( "Total&nbsp;&nbsp;${0:0,0}", this.plan.SumOfItemBudgets );
        }

        SetRow2CellStyle( hc1 );
        SetRow2CellStyle( hc1a );
        SetRow2CellStyle( hc2 );
        //SetRow2CellStyle( hc3 );
        SetRow2CellStyle( hc4 );

        hc1.Style.Add( "border-left", borderStyle );
        hc1.Style.Add( "padding-left", "10px" );

        hc1a.Style.Add( "width", "140px" );

        hc4.Style.Add( "border-right", borderStyle );
        hc4.Style.Add( "padding-right", "10px" );
        hc4.Style.Add( "text-align", "right" );

        row2.Cells.Add( hc1 );
        row2.Cells.Add( hc1a );
        row2.Cells.Add( hc2 );
        //row2.Cells.Add( hc3 );
        row2.Cells.Add( hc4 );
        row2.Style.Add( "border-top", borderStyle );

        table.Rows.Add( row2 );
    }

    private void SetRow1CellStyle( TableCell cell ) {
        cell.Style.Add( "border-bottom", borderStyle );
        cell.Style.Add( "height", row1Height );
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

    private void AddTypeRowBorders( TableRow row, bool expanded ) {
        for( int i = 0; i < row.Cells.Count; i++ ) {
            TableCell c = row.Cells[ i ];

            if( i == 2 ) {
                c.ColumnSpan = 2;
            }
            c.Style.Add( "border-top", typeRowBorderStyle );
            if( expanded == false ) {
                c.Style.Add( "border-bottom", typeRowBorderStyle );
            }
            if( i == 0 ) {
                c.Style.Add( "border-left", typeRowBorderStyle );
                c.Style.Add( "padding-left", "10px" );
                c.Style.Add( "font-size", "11pt" );
                c.Style.Add( "font-weight", "bold" );
            }
            else if( i == row.Cells.Count - 1 ) {
                c.Style.Add( "border-right", typeRowBorderStyle );
                c.Style.Add( "text-align", "right" );
                c.Style.Add( "padding-right", "10px" );
            }
        }
    }

    private void AddFooterRow( Table table, string footer ) {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        cell.ColumnSpan = 5;
        cell.Text = "<br><br><br><br><br><br><br><br><br><br><br><br><br>";
        cell.Text += footer;
        cell.Text += "<br>";            // !!!FORCE THE DISPLAY UP TO THE TOP OF THE PAGE

        cell.Style.Add( "font-size", "8pt" );
        cell.Style.Add( "padding-top", "18px" );
        cell.Style.Add( "text-align", "center" );
        cell.Style.Add( "padding-left", "130px;" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    private void AddSpacerRow( Table table ) {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.Style.Add( "height", "6px" );
        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    //private void AddBigSpacerRow( Table table, int nActiveRows ) {
    //    if( nActiveRows >= 5 ) {
    //        return;
    //    }

    //    TableRow row = new TableRow();
    //    TableCell cell = new TableCell();

    //    switch( nActiveRows ){
    //        case 0:
    //            if( this.isIE == true ) {
    //            cell.Style.Add( "height", "162px" );
    //            }
    //            else {
    //                // Firefox
    //                cell.Style.Add( "height", "158px" );
    //            }
    //            break;
    //        case 1:
    //            cell.Style.Add( "height", "128px" );
    //            break;
    //        case 2:
    //            cell.Style.Add( "height", "92px" );
    //            break;
    //        case 3:
    //            if( this.isIE == true ) {
    //                cell.Style.Add( "height", "50px" );
    //            }
    //            else {
    //                // Firefox
    //                cell.Style.Add( "height", "54px" );
    //            }
    //            break;
    //        case 4:
    //            if( this.isIE == true ) {
    //                // IE
    //                cell.Style.Add( "height", "10px" );
    //            }
    //            else {
    //                // Firefox
    //                cell.Style.Add( "height", "10px" );
    //            }
                
    //            break;
    //    }

    //    row.Cells.Add( cell );
    //    table.Rows.Add( row );
    //}
}
