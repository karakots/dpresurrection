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
using WebLibrary.Timing;
using MediaLibrary;

public partial class Prominence : System.Web.UI.Page
{
    protected string tableBorder = "solid 1px black";

    // session-level variables
    protected int currentTimingItem;
    protected MediaItem currentMediaItem;
    protected MediaPlan currentMediaPlan;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;

    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected bool addingNew;

    protected string itemBudget = "$ ?";

    protected Dictionary<SimpleOption, double> all_options;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        SetNameChangeLink();

        HandleNameChangeRequest();

        NameDiv.InnerHtml = String.Format( "{0} {1}", this.currentMediaItem.VehicleName, this.currentMediaItem.MediaType.ToString() );

        //HandleVersionChange();

        AddProminenceDisplay();

        SetEditedWarningsForTabs();

        SetEditedWarningsForTabs();
    }

    protected void SetNameChangeLink() {
        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        string changeNameJS = String.Format( "return ChangePlanName( \"{0}\" );", this.currentMediaPlan.PlanDescription );
        ChangePlanLink.Attributes.Add( "onclick", changeNameJS );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}' );", existingPlanNamesList );
        NewNameBox.Attributes[ "onkeydown" ] = "return CheckForEnter(event);";
    }

    protected void SetEditedWarningsForTabs() {
        if( this.currentMediaPlan.Edited == true ) {
            SupportTab.Attributes[ "onclick" ] = "return ConfirmEditLoss();";
            CampaignsTab.Attributes[ "onclick" ] = "return ConfirmEditLoss();";
            HomeTab.Attributes[ "onclick" ] = "return ConfirmEditLoss();";
        }
    }

    /// <summary>
    /// Handle a click on the OK button from the plan-name link
    /// </summary>
    private void HandleNameChangeRequest() {

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanNameOnly" ) {
            if( Request[ "newPlanName" ] != null && Request[ "newPlanName" ].Trim() != "" ) {
                if( this.currentMediaPlan.PlanDescription != Request[ "newPlanName" ].Trim() ) {
                    // the name was changed - update it
                    this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
                    this.currentMediaPlan.Edited = true;
                    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
                    SetNameChangeLink();
                }
            }
        }
    }

    ////private void HandleVersionChange() {
    ////    // see if we have gotten the go-ahead
    ////    if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanName" ) {
    ////        OKToCreateNewVersion();
    ////    }
    ////}

    protected void AddProminenceDisplay() {
        PlanNameLabel.InnerText = Utils.ElideString( this.currentMediaPlan.PlanDescription, Utils.MaxLengthPlanName );

        AvailableMediaListMaker availMedia = new AvailableMediaListMaker( this.currentMediaPlan, this.currentMediaItem.MediaType.ToString(),
             this.userMedia, -1, 1, this.currentMediaItem.Region );

        this.all_options = availMedia.GetProminenceList();
        if( this.all_options == null || this.all_options.Count == 0 ){
            InfoListDiv.Controls.Clear();
            return;
        }

        double[] optionScores = new double[ this.all_options.Count ];
        double[] optionScores2 = new double[ this.all_options.Count ];
        double[] optionCosts = new double[ this.all_options.Count ];
        AdOption[] optionsArray = new AdOption[ this.all_options.Count ];

        // get the actual spot price of the first ad option this media item is using
        double refOptionSpotPrice = -1;
        int refOptionID = -1;
        if( this.currentMediaItem.TimingList != null && this.currentMediaItem.TimingList.Count > 0 ) {
            refOptionSpotPrice = this.currentMediaItem.GetOptionSpotPriceAt( 0 );
            refOptionID = this.currentMediaItem.GetOptionIDAt( 0 );
        }
        else {
            // we need to somehow get the reference price even though the item doesn't have any ad options!

            // temporarily add an option
            this.currentMediaItem.AddTimingInfo( this.all_options.Keys.ElementAt( 0 ) );

            refOptionSpotPrice = this.currentMediaItem.GetOptionSpotPriceAt( 0 );
            refOptionID = this.currentMediaItem.GetOptionIDAt( 0 );

            // remove the temporari item
            this.currentMediaItem.RemoveTimingInfo( 0 );
        }

        int j = 0;
        int refIndx = -1;
        foreach (SimpleOption opt in all_options.Keys)
        {
            optionsArray[ j ] = opt;
            optionScores[ j ] = all_options[ opt ];
            optionScores2[ j ] = all_options[ opt ];
            optionCosts[j] = opt.Cost_Modifier;

            if( opt.ID == refOptionID ) {
                refIndx = j;
            }
            j++;
        }
        // convert each option cost modifier to an actual price
        if( refIndx != -1 &&  optionCosts[ refIndx ] != 0  ) {
            double costFac = refOptionSpotPrice / optionCosts[ refIndx ];
            for( int j2 = 0; j2 < optionCosts.Length; j2++ ) {
                optionCosts[ j2 ] *= costFac;
            }
        }

        Array.Sort( optionScores, optionsArray );
        Array.Sort( optionScores2, optionCosts );
        Array.Reverse( optionScores );
        Array.Reverse( optionsArray );
        Array.Reverse( optionCosts );

        int currentOptionID = -1;
        if( addingNew == false ) {
            currentOptionID = this.currentMediaItem.GetOptionIDAt( this.currentTimingItem );
        }

        Table table = new Table();
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Style.Add( "border", tableBorder );
        table.Style.Add( "width", "100%" );
        table.Style.Add( "color", "#000" );
        table.Style.Add( "font-size", "9pt" );

        AddProminenceHeaderRow( table );

        for( int i = 0; i < optionsArray.Length; i++ ){
            AdOption option = optionsArray[ i ];
            double rating = optionScores[ i ];

            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            TableCell cell3 = new TableCell();
            TableCell cell4 = new TableCell();
            TableCell cell5 = new TableCell();

            RadioButton rb = new RadioButton();
            rb.GroupName = "prominence";
            string selectorID = string.Format( "Prom-{0}", option.ID );
            rb.ID = selectorID;
            if( option.ID == currentOptionID || (addingNew && i == 0 ) ) {
                rb.Checked = true;
            }

            cell1.Controls.Add( rb );
            cell2.Text = option.Name;
            cell3.Text = String.Format( "{0:f2}", rating );
            cell4.Text = Utils.FormatDollarAmount( optionCosts[ i ] );

            cell5.Text = "&nbsp;";

            cell1.Style.Add( "height", "28px" );
            cell2.Style.Add( "height", "28px" );
            cell3.Style.Add( "height", "28px" );
            cell4.Style.Add( "height", "28px" );
            cell5.Style.Add( "height", "28px" );

            row.Cells.Add( cell1 );
            row.Cells.Add( cell2 );
            row.Cells.Add( cell3 );
            row.Cells.Add( cell4 );
            row.Cells.Add( cell5 );
            table.Rows.Add( row );
        }
        InfoListDiv.Controls.Clear();
        InfoListDiv.Controls.Add( table );
    }

    protected void AddProminenceHeaderRow( Table table ) {
        TableRow row = new TableRow();
        TableCell c1 = new TableCell();
        TableCell c2 = new TableCell();
        TableCell c3 = new TableCell();
        TableCell c4 = new TableCell();
        TableCell c5 = new TableCell();

        c1.Text = "&nbsp;";
        c2.Text = "Prominence";
        c3.Text = "Rating";
        c4.Text = "Price per Ad";
        c5.Text = "&nbsp;";

        SetHeaderCellStyle( c1 );
        SetHeaderCellStyle( c2 );
        SetHeaderCellStyle( c3 );
        SetHeaderCellStyle( c4 );
        SetHeaderCellStyle( c5 );

        row.Cells.Add( c1 );
        row.Cells.Add( c2 );
        row.Cells.Add( c3 );
        row.Cells.Add( c4 );
        row.Cells.Add( c5 );

        table.Rows.Add( row );
    }

    private void SetHeaderCellStyle( TableCell cell ){
        cell.Style.Add( "height", "28px" );
        cell.Style.Add( "background-image", "url(Images/BkgFadeBlu1.gif)" );
        cell.Style.Add( "background-repeat", "repeat-x" );
    }

    protected void AddTimingItemRow( Table table, TableRow timingRow ) {
        table.Rows.Add( timingRow );
    }

    protected void OKButton_Click( object sender, EventArgs e ) {
        int newID = GetProminenceIdFromRequest();

        if( this.addingNew == false ) {
            if( newID != this.currentMediaItem.GetOptionIDAt( this.currentTimingItem ) ) {
                // the ID changed - find the AdOption in the list we already have obtained
                this.currentMediaItem.TimingList[ this.currentTimingItem ].ChangeOption( newID );
            }
        }
        else {
            // add a new prominence
            this.currentMediaItem.AddTimingInfo( newID );
        }

        // note we do NOT save any results to disk form this page so "Cancel" can work on the Timing page.
        Response.Redirect( "Timing.aspx" );
    }

    ////private void OKToCreateNewVersion() {
    ////    // change the plan version
    ////    PlanStorage storage = new PlanStorage();
    ////    storage.IncrementPlanVersion( this.currentMediaPlan, this.allPlanVersions, this.userMedia.CurrentPlanVersions );
    ////    this.currentMediaPlan.Results = null;
    ////    this.currentMediaPlan.PlanOverallRatingStars = -1;

    ////    if( Request[ "newPlanName" ] != null ) {
    ////        this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
    ////    }

    ////    // update the user settings
    ////    foreach( UserMedia.CurrentPlanVersion info in this.userMedia.CurrentPlanVersions ) {
    ////        if( info.CampaignName == this.currentMediaPlan.CampaignName ) {
    ////            info.Version = this.currentMediaPlan.PlanVersion;
    ////            Profile.UserMediaItems = this.userMedia;
    ////            Profile.Save();
    ////        }
    ////    }

    ////    int newID = GetProminenceIdFromRequest();

    ////    // now update the plan 
    ////    this.currentMediaItem.TimingList[ this.currentTimingItem ].ChangeOption( newID );
    ////    this.currentMediaPlan.Results = null;
    ////    this.currentMediaPlan.PlanOverallRatingStars = -1;

    ////    SavePlan();
    ////    Response.Redirect( "Timing.aspx" );
    ////}

    private int GetProminenceIdFromRequest() {
        string pval = Request[ "prominence" ];
        if( pval.StartsWith( "Prom-" ) == true ) {      // should always be true
            int id = int.Parse( pval.Substring( 5 ) );
            return id;
        }
        return - 1;
    }

    protected void CancelButton_Click( object sender, EventArgs e ) {
        //if( this.currentMediaPlan.Edited == true ) {
        //    RevertToSavedVersion();
        //}

        Response.Redirect( "Timing.aspx" );
    }

    //private void SavePlan() {
    //    PlanStorage storage = new PlanStorage();
    //    storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
    //    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
    //}

    //private void RevertToSavedVersion() {
    //    PlanStorage storage = new PlanStorage();
    //    this.currentMediaPlan = storage.LoadMediaPlan( Utils.GetUser( this ), this.currentMediaPlan.CampaignName, this.currentMediaPlan.PlanVersion );
    //    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
    //}

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaItem = (MediaItem)Session[ "CurrentMediaItem" ];
        this.currentTimingItem = (int)Session[ "currentTimingItem" ];
        this.allPlanVersions = Utils.AllPlanVersions( this );

        //this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );

        this.addingNew = false;
        if( this.currentTimingItem >= this.currentMediaItem.TimingList.Count ) {
            // we are adding a new prominence
            this.addingNew = true;
        }

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }

        //string helpEmail = String.Format( "<a href=\"mailto:support@adplanit.com?subject=AdPlanit Support Request - {0}\">Question? Send us a message!</a>", "Media Prominence" );
        //HelpEmailLink.Text = helpEmail;
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
