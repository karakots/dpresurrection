using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WebLibrary;
using BusinessLogic;
using MediaLibrary;

public partial class Order : System.Web.UI.Page
{
    private string col1Width = "170px";
    private string col1Indent = "30px";
    private string lastColWidth = "170px";

    private bool printMode = false;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        // respond to a request to change/set current plan
        if( Request[ "p" ] != null ) {
            this.currentMediaPlan = Utils.PlanForID( Request[ "p" ], this.currentMediaPlans );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }

        SetNameChangeLink();

        HandleModifyRequest();

        DetectPrintMode();

        DisplayPlanInfo();

        DisplayOrderInfo();

        DisplaySponsorInfo();

        if( this.IsPostBack == false ) {
            DataLogger.LogPageVisit( "ORDER", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription );
        }
    }

    protected void CreateRFP_Click( object sender, EventArgs e ) {
    }

    private void DetectPrintMode() {
        if( Request[ "print" ] == "1" ) {
            this.printMode = true;

            PrintListButton.Visible = false;
            ////CreateRFPButton.Visible = false;
            PageBody.Attributes.Add( "onload", "window.print();" );
            TabHelp.Visible = false;
            TabCampaigns.Visible = false;
            TabHome.Visible = false;
            TabShop.Visible = false;
            TabOrder.Visible = false;
            TabAnalysis.Visible = false;
        }
    }

    private void DisplaySponsorInfo() {
        //??? !!!!Where do we get this information??? does it ever change???

        OrderRHInfoDiv.InnerHtml = "<b>Agency Sponsors</b><br><br><br>" +
        "Campbell Ad Agency<br>" +
        "&quot;we do the creative<br>" +
        "and media plans for you&quot;<br>" +
        "<a href=\"www.agency1.com\">www.agency1.com</a><br><br>" +

        "SD Creative<br>" +
        "&quot;we can make your plan <br>" +
        "happen&quot;<br>" +
        "<a href=\"www.sdcreative.com\">www.sdcreative.com</a><br><br><br><br>" +


        "<b>Media Sponsors</b><br><br><br>" +

        "Google Adwords<br>" +
        "&quot;we can do it all&quot;<br>" +
        "<a href=\"www.google.com\">www.google.com/adwords</a><br><br>" +

        "SJ Mercury News<br>" +
        "&quot;your most effective media&quot;<br>" +
        "<a href=\"www.sjmnews.com\">www.sjmnews.com/adv</a><br>";
    }

    private void DisplayOrderInfo() {
        Table table = new Table();
        table.CellSpacing  = 0;
        table.CellPadding = 0;
        table.Style.Add( "font-size", "8pt" );
        table.Style.Add( "width", "600px" );

        AddCampaignInfoRows( table );

        foreach( MediaItem typeItem in this.currentMediaPlan.MediaItems ){
            if( typeItem.sub_items.Count > 0 ) {
                AddMediaTypeHeader( table, typeItem.MediaType );
                foreach( MediaItem planVehicle in typeItem.sub_items ) {
                    AddVehicleInfoRows( table, planVehicle );
                }
            }
        }


        AddFooterRow( table );

        XPlanSummaryCell.Controls.Clear();
        XPlanSummaryCell.Controls.Add( table );
        //XPlanSummaryCell.Style.Add( "width", "100%" );
    }

    private void AddCampaignInfoRows( Table table ) {
        TableRow row = new TableRow();

        TableCell titleCell = new TableCell();
        titleCell.ColumnSpan = 5;
        titleCell.Text = this.currentMediaPlan.CampaignName;
        row.Cells.Add( titleCell );
        titleCell.Style.Add( "font-size", "9pt" );
        titleCell.Style.Add( "font-weight", "bold" );
        titleCell.Style.Add( "padding-top", "15px;" );
        titleCell.Style.Add( "padding-bottom", "20px;" );

        string budgetStr = null;
        if( this.currentMediaPlan.Specs.TargetBudget < 1000 ) {
            budgetStr = String.Format( "$ {0:0}", this.currentMediaPlan.TargetBudget );
        }
        else {
            budgetStr = String.Format( "$ {0:0,0}", this.currentMediaPlan.TargetBudget );
        }

        string costStr = null;
        if( this.currentMediaPlan.SumOfItemBudgets < 1000 ) {
            costStr = String.Format( "<b>$ {0:0}</b>", this.currentMediaPlan.SumOfItemBudgets );
        }
        else {
            costStr = String.Format( "<b>$ {0:0,0}</b>", this.currentMediaPlan.SumOfItemBudgets );
        }

        table.Rows.Add( row );

        AddInfoRow( table, "Budget:", budgetStr );
        AddInfoRow( table, "Actual Cost (Est.):", costStr );
        AddInfoRow( table, "Scheduled&nbsp;Start&nbsp;Date:", this.currentMediaPlan.Specs.StartDate.ToString( "MM/dd/yy" ), "End Date:", this.currentMediaPlan.Specs.EndDate.ToString( "MM/dd/yy" ) );
        for( int s = 0; s < this.currentMediaPlan.DemographicCount; s++ ) {
            string rowTitle = String.Format( "Target&nbsp;segment&nbsp;{0}:", s + 1 );
            AddInfoRow( table, rowTitle, this.currentMediaPlan.Specs.Demographics[ s ].SummaryDescription() );
        }
        for( int r = 0; r < this.currentMediaPlan.Specs.GeoRegionNames.Count; r++ ) {
            string rowTitle = String.Format( "Target&nbsp;geography&nbsp;{0}:", r + 1 );
            AddInfoRow( table, rowTitle, this.currentMediaPlan.Specs.GeoRegionNames[ r ] );
            if( r == this.currentMediaPlan.Specs.GeoRegionNames.Count - 1 ){
                AddInfoRow( table, "", "" );
            }
        }
    }

    private void AddMediaTypeHeader( Table table, MediaVehicle.MediaType mType ) {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.ColumnSpan = 5;

        string mtypestr = mType.ToString();
        if( mtypestr == "Yellowpages" ) {
            mtypestr = "Yellow Pages";
        }

        string costStr = null;
        double typeCost = this.currentMediaPlan.GetTypeSpending( mType );
        if( typeCost < 1000 ) {
            costStr = String.Format( "<b>$ {0:0}</b>", typeCost );
        }
        else {
            costStr = String.Format( "<b>$ {0:0,0}</b>", typeCost );
        }

        cell.Text = String.Format( "{0} -- {1} Spending&nbsp;&nbsp;{2}", this.currentMediaPlan.PlanDescription, mtypestr, costStr );

        cell.Style.Add( "font-size", "9pt" );
        cell.Style.Add( "font-weight", "bold" );
        cell.Style.Add( "margin-top", "25px;" );
        cell.Style.Add( "padding-top", "15px;" );
        cell.Style.Add( "padding-bottom", "20px;" );
        cell.Style.Add( "border-top", "solid 2px #8fC2F4" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    private void AddVehicleInfoRows( Table table, MediaItem item ) {
        string itemBudgetStr = null;
        if( item.TotalPrice < 1000 ) {
            itemBudgetStr = String.Format( "&nbsp;&nbsp;$&nbsp;{0:0}", item.TotalPrice );
        }
        else {
            itemBudgetStr = String.Format( "&nbsp;&nbsp;$&nbsp;{0:0,0}", item.TotalPrice );
        }

        AddInfoRow( table, item.VehicleName, itemBudgetStr );

        AddItemTimingRows( table, item );

        AddInfoRow( table, "", "" );
    }

    /// <summary>
    /// Adds the row(s) to the given table that describe the timing of the given media item (in a format suitable for ordering that item).
    /// </summary>
    /// <param name="item"></param>
    private void AddItemTimingRows( Table table, MediaItem item ) {

        List<List<WebLibrary.Timing.TimingDisplayItem>> display = item.GetTimingDisplay( this.currentMediaPlan.StartDate );
        for( int i = 0; i < display.Count; i++ ) {
            List<WebLibrary.Timing.TimingDisplayItem> dispItem = display[ i ];
            string adOptName = item.GetOptionNameAt( i );

            for( int d = 0; d < dispItem.Count; d++ ) {

                string rowText = dispItem[ d ].ToOrderString( adOptName );
                AddInfoRow( table, "", rowText );
            }

            //List<DateTime> adDates = item.GetAdDates();
            //switch( item.MediaType ) {
            //    case MediaVehicle.MediaType.Internet:
            //        WebLibrary.Timing.InternetTimingDisplayItem tdi = dispItem as WebLibrary.Timing.InternetTimingDisplayItem;
            //        int nImps = tdi.NumberOfImpresions;
            //        AddItemDateRows( table, adDates, adOptName, "impression", nImps, "D" );
            //        break;
            //    case MediaVehicle.MediaType.Magazine:
            //        AddItemDateRows( table, adDates, adOptName, "ad",  "issue", "M" );
            //        break;
            //    case MediaVehicle.MediaType.Newspaper:
            //        AddItemDateRows( table, adDates, adOptName, "ad", "issue", "D" );
            //        break;
            //    case MediaVehicle.MediaType.Radio:
            //        AddItemDateRows( table, adDates, adOptName, "ad", "day", "D" );
            //        break;
            //    case MediaVehicle.MediaType.Yellowpages:
            //        AddItemDateRows( table, adDates, adOptName, "ad", "issue", "Y" );
            //        break;
            //}
        }
    }

    //private void AddItemDateRows( Table table, List<DateTime> adDates, string adOptName, string spotName, string mediaName, int nImps, string intervalCode ) {
    //    if( adDates.Count == 1 ) {
    //        if( intervalCode == "I" ) {
    //            string s = String.Format( "1 {0} {1} in 1 {2} : {3}-{4}", adOptName, spotName, mediaName, d0.Year, d0.Year + 1 );
    //            AddInfoRow( table, "", s );
    //        } 
    //        else if( intervalCode == "D" ) {
    //            string s = String.Format( "1 {0} {1} in 1 {2} : {3}", adOptName, spotName, mediaName, d0.ToString( "MMM d, yyyy" ) );
    //            AddInfoRow( table, "", s );
    //        }
    //        else if( intervalCode == "M" ) {
    //            string s = String.Format( "1 {0} {1} in 1 {2} : {3}", adOptName, spotName, mediaName, d0.ToString( "MMM yyyy" ) );
    //            AddInfoRow( table, "", s );
    //        }
    //        else if( intervalCode == "Y" ) {
    //            string s = String.Format( "1 {0} {1} in 1 {2} : {3}-{4}", adOptName, spotName, mediaName, d0.Year, d0.Year + 1 );
    //            AddInfoRow( table, "", s );
    //        }
    //        AddInfoRow( table, "", "1 " + spotName + ": " + adDates[ 0 ].ToString( "MMM d, yyyy" ) );
    //    }
    //    else {
    //        int interval = 1;
    //        if( intervalCode == "M" ) {
    //            interval = 31;
    //        }
    //        else if( intervalCode == "Y" ) {
    //            interval = 380;        
    //        }

    //        DateTime d0 = adDates[ 0 ];      // start of the current range
    //        DateTime d1 = adDates[ 0 ];      // end of the current range

    //        for( int d = 1; d < adDates.Count; d++ ) {

    //            if( (adDates[ d ] - d1).TotalDays <= interval ) {
    //                // dates are consecutive - just extend the interval
    //                d1 = adDates[ d ];
    //            }
    //            else {
    //                AddDateItem( table, adOptName, spotName, intervalCode, d0, d1 );
    //                d0 = adDates[ d ];
    //                d1 = adDates[ d ];
    //            }
    //        }

    //        // handle the last date
    //        AddDateItem( table, adOptName,  spotName, intervalCode, d0, d1 );
    //    }
    //}

    //private void AddDateItem( Table table, string adOptName, string spotName, string intervalCode, DateTime d0, DateTime d1 ) {
    //    if( d0 == d1 ) {
    //        if( intervalCode == "I" ) {
    //            AddInfoRow( table, "", "1 " + spotName + ": " + d0.ToString( "MMM d, yyyy" ) );
    //        }
    //        else if( intervalCode == "D" ) {
    //            AddInfoRow( table, "", "1 " + spotName + ": " + d0.ToString( "MMM d, yyyy" ) );
    //        }
    //        else if( intervalCode == "M" ) {
    //            AddInfoRow( table, "", "1 " + spotName + ": " + d0.ToString( "MMM yyyy" ) );
    //        }
    //        else if( intervalCode == "Y" ) {
    //            string s = String.Format( "1 {0}: {1}-{2}", spotName, d0.Year, d0.Year + 1  );
    //            AddInfoRow( table, "", s );
    //        }
    //    }
    //    else {
    //        int nIntervals = (int)(d1 - d0).TotalDays;
    //        if( intervalCode == "M" ) {
    //            nIntervals = (int)Math.Round( nIntervals / 30.0 );
    //        }
    //        else if( intervalCode == "Y" ) {
    //            nIntervals = (int)Math.Round( nIntervals / 365.0 );
    //        }
    //        nIntervals += 1;

    //        if( intervalCode == "I" ) {
    //            AddInfoRow( table, "", nIntervals.ToString() + " " + spotName + "s: " + d0.ToString( "MMM d, yyyy" ) + " - " + d1.ToString( "MMM d, yyyy" ) );
    //        }
    //        else if( intervalCode == "D" ) {
    //            AddInfoRow( table, "", nIntervals.ToString() + " " + spotName + "s: " + d0.ToString( "MMM d, yyyy" ) + " - " + d1.ToString( "MMM d, yyyy" ) );
    //        }
    //        else if( intervalCode == "M" ) {
    //            AddInfoRow( table, "", nIntervals.ToString() + " " + spotName + "s: " + d0.ToString( "MMM yyyy" ) + " - " + d1.ToString( "MMM yyyy" ) );
    //        }
    //        else if( intervalCode == "Y" ) {
    //            string s = String.Format( "1 {0}: {1}-{2}", spotName, d0.Year, d1.Year + 1 );
    //            AddInfoRow( table, "", s );
    //        }
    //    }
    //}

    private void AddInfoRow( Table table, string col1Txt, string col2Txt ) {
        AddInfoRow( table, col1Txt, col2Txt, null, null );
    }

    private void AddInfoRow( Table table, string col1Txt, string col2Txt, string col3Txt, string col4Txt ) {
        bool is2Col = false;
        if( col3Txt == null ){
            is2Col = true;
        }

        TableRow row = new TableRow();
        TableCell cell1 = new TableCell();
        TableCell cell2 = new TableCell();
        TableCell cell3 = new TableCell();
        TableCell cell4 = new TableCell();
        TableCell cell5 = new TableCell();

        cell1.Text = col1Txt;
        cell2.Text = col2Txt;
        row.Cells.Add( cell1 );
        row.Cells.Add( cell2 );

        row.Style.Add( "height", "16px" );
        cell1.Style.Add( "width", col1Width );
        cell1.Style.Add( "padding-left", col1Indent ); 

        if( is2Col == false ) {
            cell3.Text = col3Txt;
            cell4.Text = col4Txt;
            row.Cells.Add( cell3 );
            row.Cells.Add( cell4 );
            cell5.Text = "&nbsp;";
            row.Cells.Add( cell5 );
            cell5.Style.Add( "width", lastColWidth );
        }
        else {
            cell2.ColumnSpan = 4;
        }


        table.Rows.Add( row );
    }

    private void AddFooterRow( Table table ){
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.ColumnSpan = 5;

        cell.Text = "<br><br><br><br>";
        cell.Text += Utils.Footer;
        cell.Style.Add( "font-size", "8pt" );
        cell.Style.Add( "padding-left", "130px" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    protected void SetNameChangeLink() {
        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        string changeNameJS = String.Format( "return ChangePlanName( \"{0}\" );", this.currentMediaPlan.PlanDescription );
        ChangePlanLink.Attributes.Add( "onclick", changeNameJS );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}' );", existingPlanNamesList );
        NewNameBox.Attributes[ "onkeydown" ] = "return CheckForEnter(event);";
    }

    /// <summary>
    /// Handle a click on the Plan-rename link button
    /// </summary>
    private void HandleModifyRequest() {

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanName" ) {
            if( Request[ "newPlanName" ] != null && Request[ "newPlanName" ].Trim() != "" ) {
                if( this.currentMediaPlan.PlanDescription != Request[ "newPlanName" ].Trim() ) {
                    // the name was changed - update it
                    this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
                    PlanStorage storage = new PlanStorage();
                    storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
                    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
                    SetNameChangeLink();
                }
            }
        }
    }

    private void DisplayPlanInfo() {

        CampaignName.Text = Utils.ElideString( this.currentMediaPlan.CampaignName, Utils.MaxLengthPlanName ) +
            " - " + this.currentMediaPlan.Specs.StartDate.ToString( "MM/dd/yy" );
        CampaignName.ToolTip = this.currentMediaPlan.CampaignName;

        string planDesc = this.currentMediaPlan.PlanDescription;
        if( planDesc == null || planDesc == "" ) {
            planDesc = String.Format( "---   [version {0}]", this.currentMediaPlan.PlanVersion );
        }
        PlanName.Text = Utils.ElideString( planDesc, Utils.MaxLengthPlanName ) + " - " + this.currentMediaPlan.StartDate.ToString( "MM/dd/yy" );
        PlanName.ToolTip = planDesc;

        SmallSummary smallSummaryMaker = new SmallSummary();
        smallSummaryMaker.AddSummaryHTML( this.PieChartDiv, this.currentMediaPlan, this.engineeringMode, true );


        // display the stars rating
        StarsDiv.InnerHtml = "";
        if( this.currentMediaPlan.PlanOverallRatingStars >= 0 ) {
            int nStars = (int)Math.Max( 0, Math.Min( this.currentMediaPlan.PlanOverallRatingStars, 5 ) );

            string starTag = "<img src=\"images/Star19.gif\" style=\"margin-right:1px\" />";
            string blankSstarTag = "<img src=\"images/Star19Blank.gif\" style=\"margin-right:1px\" />";
            for( int s = 0; s < nStars; s++ ) {
                StarsDiv.InnerHtml += starTag;
            }
            for( int s2 = nStars; s2 < 5; s2++ ) {
                StarsDiv.InnerHtml += blankSstarTag;
            }
        }
    }

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.allPlanVersions = Utils.AllPlanVersions( this );
        this.engineeringMode = Utils.InEngineeringMode( this, null );

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }
    }

    /// <summary>
    /// Redirects a user who ins't logged-in back to the login page
    /// </summary>
    private void RedirectUnknownUsers() {
        if( this.User == null || this.User.Identity == null || this.User.Identity.Name == null || this.User.Identity.Name == "" ) {
            Response.Redirect( "Login.aspx" );
        }
    }
}
