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

public partial class Targeting : System.Web.UI.Page
{
    protected string tableBorder = "solid 1px black";

    // session-level variables
    protected MediaItem currentMediaItem;
    protected MediaPlan currentMediaPlan;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;

    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string itemBudget = "$ ?";

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        SetMediaType();

        SetNameChangeLink();

        HandleNameChangeRequest();

        NameDiv.InnerHtml = String.Format( "{0} {1}", this.currentMediaItem.VehicleName, this.currentMediaItem.MediaType.ToString() );

        // see if there are any updated values
        if( IsPostBack ) {
            UpdateValuesFromRequest();
        }


        TargetingLevelTextbox.Attributes.Add( "onchange", "SetTargetingPriceRatio();" );
        AddTargetingDisplay();

        if( IsPostBack == false ) {
            SetTargetingSlider();
        }

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


    //private void HandleVersionChange() {
    //    // see if we have gotten the go-ahead
    //    if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanName" ) {
    //        OKToCreateNewVersion();
    //    }
    //}

    protected List<int> GetRegionIndexesFromRequest() {
        List<int> retval = new List<int>();
        foreach( string key in Request.Params.AllKeys ) {
            if( key.StartsWith( "Region-" ) ){
                int rgnIndex = int.Parse( key.Substring( 7 ) );
                retval.Add( rgnIndex );
            }
        }
        return retval;
    }

    protected List<int> GetSegmentIndexesFromRequest() {
        List<int> retval = new List<int>();
        foreach( string key in Request.Params.AllKeys ) {
            if( key.StartsWith( "Segment-" ) ) {
                int rgnIndex = int.Parse( key.Substring( 8 ) );
                retval.Add( rgnIndex );
            }
        }
        return retval;
    }

    protected void AddTargetingDisplay() {
        PlanNameLabel.InnerText = Utils.ElideString( this.currentMediaPlan.PlanDescription, Utils.MaxLengthPlanName );

        // get all regions and all demographic segments in the plan
        List<string> allRegions = this.currentMediaPlan.Specs.GeoRegionNames;
        List<DemographicSettings> allSegments = this.currentMediaPlan.Specs.Demographics;

        // get the regions and segments currently targeted by this item
        List<DemographicLibrary.Demographic> currentSegments = this.currentMediaItem.Demographics;
        List<string> currentRegions = this.currentMediaItem.Regions;

        int nRows = Math.Max( 1,  Math.Max( allRegions.Count, allSegments.Count ) );
        
        Table table = new Table();
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Style.Add( "border", tableBorder );
        table.Style.Add( "width", "100%" );
        table.Style.Add( "color", "#000" );
        table.Style.Add( "font-size", "9pt" );

        AddTargetingHeaderRow( table );

        for( int i = 0; i < nRows; i++ ){

            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            TableCell cell3 = new TableCell();
            TableCell cell4 = new TableCell();
            TableCell cell5 = new TableCell();

            if( i < allRegions.Count ) {
                CheckBox cb = new CheckBox();
                string selectorID = string.Format( "Region-{0}", i );
                cb.ID = selectorID;
                cell1.Controls.Add( cb );

                string rgn = allRegions[ i ];

                foreach( string curRgn in currentRegions ) {
                    if( curRgn == rgn ) {
                        cb.Checked = true;
                    }
                }

                cell2.Text = rgn;
            }
            else {
                cell1.Text = "&nbsp;";
                cell2.Text = "&nbsp;";
            }

            if( i < allSegments.Count ){
                CheckBox cb = new CheckBox();
                string selectorID = string.Format( "Segment-{0}", i );
                cb.ID = selectorID;
                cell3.Controls.Add( cb );


                DemographicSettings settings = allSegments[ i ];
                foreach( DemographicLibrary.Demographic curSeg in currentSegments ) {
                    if( curSeg.Name == settings.DemographicName ) {
                        cb.Checked = true;
                    }
                }
                cell4.Text = settings.DemographicName;
            }
            else {
                cell3.Text = "&nbsp;";
                cell4.Text = "&nbsp;";
            }

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

    protected void AddTargetingHeaderRow( Table table ) {
        TableRow row = new TableRow();
        TableCell c1 = new TableCell();
        TableCell c2 = new TableCell();
        TableCell c3 = new TableCell();
        TableCell c4 = new TableCell();
        TableCell c5 = new TableCell();

        c1.Text = "&nbsp;";
        c2.Text = "Region";
        c3.Text = "&nbsp;";
        c4.Text = "Demographic Segment";
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

    protected void UpdateValuesFromRequest() {

        this.currentMediaPlan.Edited = true;
    }

    private void SetHeaderCellStyle( TableCell cell, int headerIndex ){
        cell.Style.Add( "height", "28px" );
        cell.Style.Add( "background-image", "url(Images/BkgFadeBlu1.gif)" );
        cell.Style.Add( "background-repeat", "repeat-x" );
    }

    protected void AddTimingItemRow( Table table, TableRow timingRow ) {
        table.Rows.Add( timingRow );
    }

    protected void OKButton_Click( object sender, EventArgs e ) {

        List<int> selSegIndexes = GetSegmentIndexesFromRequest();
        List<int> selRegIndexes = GetRegionIndexesFromRequest();

        if( selSegIndexes.Count == 0 || selRegIndexes.Count == 0 ) {
            MessageDiv.InnerHtml = "<br>Error: You must check at least one box in each column.<br><br>";
        }
        else {
            MessageDiv.InnerHtml = "<br>&nbsp;<br><br>";

            // perhaps the targeting level changed?
            int newTargLevel = int.Parse( TargetingLevelTextbox.Text );
            if( this.currentMediaItem.TargetingLevel != newTargLevel ) {
                this.currentMediaPlan.Edited = true;
            }

            if( this.currentMediaPlan.Edited == true ) {
                //if( this.currentMediaPlan.Results != null ) {
                //    // change the plan version
                //    string suggestedNewName = Utils.GetSuggestedPlanName( this.currentMediaPlan.PlanDescription, this.currentMediaPlan.CampaignName, this.allPlanVersions, false );
                //    string nextVerJS = String.Format( "return GetNameForNextVersion( \"{0}\", true );", suggestedNewName );
                //    PageBody.Attributes.Add( "onLoad", nextVerJS );
                //    return;
                //}

                // now update the plan

                // get all regions and all demographic segments in the plan
                List<string> allRegions = this.currentMediaPlan.Specs.GeoRegionNames;
                List<string> newItemRegions = new List<string>();
                for( int i = 0; i < allRegions.Count; i++ ) {
                    if( selRegIndexes.Contains( i ) ){
                        newItemRegions.Add( allRegions[ i ] );       
                    }
                }

                List<DemographicSettings> allSegments = this.currentMediaPlan.Specs.Demographics;
                List<DemographicLibrary.Demographic> newItemSegments = new List<DemographicLibrary.Demographic>();
                for( int i = 0; i < allSegments.Count; i++ ) {
                    if( selSegIndexes.Contains( i ) ) {

                        DemographicLibrary.Demographic demo = SimUtils.ConvertToDemographic( this.currentMediaPlan, i );
                        newItemSegments.Add( demo );
                    }
                }

                // now actually apply the new settings to the item
                this.currentMediaItem.Regions = newItemRegions;
                this.currentMediaItem.Demographics = newItemSegments;
                this.currentMediaItem.TargetingLevel = int.Parse( TargetingLevelTextbox.Text );
                Session[ "CurrentMediaItem" ] = this.currentMediaItem;

                //SavePlan();
            }
            Response.Redirect( "MediaDetails.aspx" );
        }
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


    ////    List<int> selSegIndexes = GetSegmentIndexesFromRequest();
    ////    List<int> selRegIndexes = GetRegionIndexesFromRequest();

    ////    // now update the plan

    ////    // get all regions and all demographic segments in the plan
    ////    List<string> allRegions = this.currentMediaPlan.Specs.GeoRegionNames;
    ////    List<string> newItemRegions = new List<string>();
    ////    for( int i = 0; i < allRegions.Count; i++ ) {
    ////        if( selRegIndexes.Contains( i ) ) {
    ////            newItemRegions.Add( allRegions[ i ] );
    ////        }
    ////    }

    ////    List<DemographicSettings> allSegments = this.currentMediaPlan.Specs.Demographics;
    ////    List<DemographicLibrary.Demographic> newItemSegments = new List<DemographicLibrary.Demographic>();
    ////    for( int i = 0; i < allSegments.Count; i++ ) {
    ////        if( selSegIndexes.Contains( i ) ) {

    ////            DemographicLibrary.Demographic demo = SimUtils.ConvertToDemographic( this.currentMediaPlan, i );
    ////            newItemSegments.Add( demo );
    ////        }
    ////    }

    ////    // now actually apply the new settings to the item
    ////    this.currentMediaItem.Regions = newItemRegions;
    ////    this.currentMediaItem.Demographics = newItemSegments;
    ////    this.currentMediaItem.TargetingLevel = int.Parse( TargetingLevelTextbox.Text );

    ////    SavePlan();
    ////    Response.Redirect( "MediaDetails.aspx" );
    ////}

    /// <summary>
    /// Sets the media type for the display.
    /// </summary>
    private void SetMediaType() {
        if( Request[ "TypeSelection" ] != null ) {
            this.TypeSelection.Value = Request[ "TypeSelection" ];
        }
    }

    private void SetTargetingSlider() {
        this.TargetingLevelTextbox.Text = this.currentMediaItem.TargetingLevel.ToString();
        this.adPriceMultiplier.InnerHtml = String.Format( "{0:f1}", this.currentMediaItem.TargetingPriceMultiplier );
    }

    protected void CancelButton_Click( object sender, EventArgs e ) {
        //if( this.currentMediaPlan.Edited == true ) {
        //    RevertToSavedVersion();
        //}

        Response.Redirect( "MediaDetails.aspx" );
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
        this.allPlanVersions = Utils.AllPlanVersions( this );

        //this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }

        //string helpEmail = String.Format( "<a href=\"mailto:support@adplanit.com?subject=AdPlanit Support Request - {0}\">Question? Send us a message!</a>", "Media Timing" );
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
