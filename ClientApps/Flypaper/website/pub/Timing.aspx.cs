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

public partial class Timing : System.Web.UI.Page
{
    protected string tableBorder = "solid 1px black";

    // session-level variables
    protected MediaItem currentMediaItem;
    protected MediaPlan currentMediaPlan;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;

    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string expandedItemIndexString = "";
    protected List<int> expandedItemIndexes;

    protected string itemBudget = "$ ?";

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        SetMediaType();

        SetTimeLimitFields();

        SetNameChangeLink();

        HandleNameChangeRequest();

        PlanNameSpan.InnerHtml = String.Format( "{0} {1}", this.currentMediaItem.VehicleName, this.currentMediaItem.MediaType.ToString() );
        GrandTotal.InnerHtml = String.Format( "Total:&nbsp;<b>{0}</b>", Utils.FormatDollarAmount( this.currentMediaItem.TotalPrice ).Replace( " ", "&nbsp;" ) );

        // see if there are any updated values
        if( IsPostBack ) {
            UpdateValuesFromRequest();
        }

        HandleProminenceChange();      // go to the Prominence page if there is a request to change
        HandleTargetingChange();         // go to the Targeting page if there is a request to change
        HandleProminenceAdd();

        HandleItemAdd();
        HandleItemRemove();

        SetExpandedItems();

        string grandTotalExpr = AddTimingDisplay();
        this.totalExpression.Value = grandTotalExpr;

        SetEditedWarningsForTabs();
    }

    protected void SetNameChangeLink() {
        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        string changeNameJS = String.Format( "return ChangePlanName( \"{0}\" );", this.currentMediaPlan.PlanDescription );
        ChangePlanLink.Attributes.Add( "onclick", changeNameJS );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}', true );", existingPlanNamesList );
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

    /// <summary>
    /// Adds the display.  Returns the javascript expression that evaluates to the grand total of the page.
    /// </summary>
    protected string AddTimingDisplay() {
        PlanNameLabel.InnerText = Utils.ElideString( this.currentMediaPlan.PlanDescription, Utils.MaxLengthPlanName );

        this.InfoListDiv.Controls.Clear();

        // loop over the ad options
        int overallRowNumber = 0;
        int headerIndex = 0;

        List<List<TimingDisplayItem>> display = this.currentMediaItem.GetTimingDisplay( this.currentMediaPlan.StartDate );

        string grandTotalExpr = "0";

        for( int i = 0; i < display.Count; i++ ) {

            Table table = new Table();
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Style.Add( "border", tableBorder );
            table.Style.Add( "width", "100%" );
            table.Style.Add( "color", "#000" );
            table.Style.Add( "font-size", "9pt" );

            List<TimingDisplayItem> dispItems = display[ i ];
            // creete the UI -- add a header for the ad option
            string optionName = this.currentMediaItem.GetOptionNameAt( i );
            int optionID = this.currentMediaItem.GetOptionIDAt( i );
            double optionSpotPrice = this.currentMediaItem.GetOptionSpotPriceAt( i );
            AddOptionHeaderRow( table, optionName, optionSpotPrice, headerIndex );
            headerIndex += 1;
            // then add the timing items themselves
            for( int j = 0; j < dispItems.Count; j++ ) {
                bool expand = this.expandedItemIndexes.Contains( j );
                string itemTotalExpr = "0";
                AddTimingItemRow( table, dispItems[ j ].GetRow( overallRowNumber, optionID, optionSpotPrice, expand, out itemTotalExpr ) );
                overallRowNumber += 1;
                grandTotalExpr += "+" + itemTotalExpr;
            }
            AddAddItemRow( table, i );

            this.InfoListDiv.Controls.Add( table );
        }

        this.numRows.Value = overallRowNumber.ToString();

        Table xtable = new Table();
        xtable.CellPadding = 8;
        xtable.CellSpacing = 0;

        AddAddOptionRow( xtable, display.Count );

        this.InfoListDiv.Controls.Add( xtable );
        this.TimingItemCount.Value = overallRowNumber.ToString();      // store the total row count in a hidden field
        return grandTotalExpr;
    }

    protected void HandleProminenceChange() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "Change" ) {
            int currentTimingItem = int.Parse( Request[ "__EVENTARGUMENT" ] );
            Session[ "currentTimingItem" ] = currentTimingItem;
            Response.Redirect( "Prominence.aspx" );
        }
    }

    protected void HandleItemAdd() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "AddItem" ) {
            int currentTimingItem = int.Parse( Request[ "__EVENTARGUMENT" ] );

            List<DateTime> adDates = this.currentMediaItem.GetAdDates();
            if( adDates.Count > 0 ){
                DateTime lastDate = adDates[ adDates.Count - 1 ];
                switch( this.currentMediaItem.MediaType ) {
                    case MediaVehicle.MediaType.Internet:
                    case MediaVehicle.MediaType.Radio:
                    case MediaVehicle.MediaType.Newspaper:
                        lastDate = lastDate.AddDays( 2 );
                        break;
                    case MediaVehicle.MediaType.Magazine:
                        lastDate = lastDate.AddMonths( 2 );
                        break;
                    case MediaVehicle.MediaType.Yellowpages:
                        lastDate = lastDate.AddYears( 2 );
                        break;
                }
                this.currentMediaItem.TimingList[ currentTimingItem ].AddAirDate( lastDate, 1 );
                this.currentMediaPlan.Edited = true;
            }
        }
    }

    protected void HandleItemRemove() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "Remove" ) {
            int removeTimingItem = int.Parse( Request[ "__EVENTARGUMENT" ] );
            this.currentMediaItem.RemoveTimingInfo( removeTimingItem );
            Session[ "CurrentMediaItem" ] = this.currentMediaItem;
        }
    }

    protected void HandleProminenceAdd() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "AddOption" ) {
            int numTimingItems = int.Parse( Request[ "__EVENTARGUMENT" ] );
            Session[ "currentTimingItem" ] = numTimingItems;
            Response.Redirect( "Prominence.aspx" );
        }
    }

    protected void HandleTargetingChange() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "SetTargeting" ) {
            Response.Redirect( "Targeting.aspx" );
        }
    }

    protected void UpdateValuesFromRequest() {
        TimingItemConverter.ParseUserTimingInputs( Request, this.currentMediaItem );
        this.currentMediaPlan.Edited = true;
    }


    /// <summary>
    /// Deals with the selection of displayed items that are in an expanded state.
    /// </summary>
    private void SetExpandedItems() {
        // preserve the existing expanded items on postback
        if( Request[ "ExpandedItemIndexes" ] != null ) {
            this.expandedItemIndexString = Request[ "ExpandedItemIndexes" ];
            this.ExpandedItemIndexes.Value = this.expandedItemIndexString;
        }

        this.expandedItemIndexes = new List<int>();
        if( this.expandedItemIndexString.Length > 0 ) {
            string[] items = this.expandedItemIndexString.Split( ' ' );
            foreach( string idstr in items ) {
                expandedItemIndexes.Add( int.Parse( idstr ) );
            }
        }

        // see if the user wants to expand an item
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ExpandItem" ) {
            int expIndex = int.Parse( Request[ "__EVENTARGUMENT" ] );
            if( expandedItemIndexes.Contains( expIndex ) == false ) {
                expandedItemIndexes.Add( expIndex );
            }
        }

        // see if the user wants to collapse an item
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "CollapseItem" ) {
            int collapseIndex = int.Parse( Request[ "__EVENTARGUMENT" ] );
            if( expandedItemIndexes.Contains( collapseIndex ) == true ) {
                expandedItemIndexes.Remove( collapseIndex );
            }
        }

        // regenerate the single string representation (for the hidden form field)
        this.expandedItemIndexString = "";
        for( int i = 0; i < expandedItemIndexes.Count; i++ ) {
            expandedItemIndexString += expandedItemIndexes[ i ].ToString();
            if( i < expandedItemIndexes.Count - 1 ) {
                expandedItemIndexString += " ";        // separate the items
            }
        }
        this.ExpandedItemIndexes.Value = this.expandedItemIndexString;
    }


    protected void AddAddItemRow( Table table, int optionNum ) {
        string add_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "AddItem", optionNum );

        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        cell.Text = String.Format( "<br><a href=# onclick='{0}' style=\"font-size:9pt;font-weight:normal;\" >add more</a>", add_JS );
        cell.Style.Add( "padding-left", "20px" );
        cell.Style.Add( "padding-bottom", "12px" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    protected void AddAddOptionRow( Table table, int nOptions ) {
        string add_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "AddOption", nOptions );

        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        cell.Text = String.Format( "<a href=# onclick='{0}' style=\"font-size:9pt;font-weight:normal;\" >add another prominence...</a>", add_JS );
        cell.Style.Add( "padding-bottom", "22px" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    protected void AddOptionHeaderRow( Table table, string adOption, double adOptionCostPer, int headerIndex ) {
        TableRow row = new TableRow();
        TableCell sectionTitleCell = new TableCell();

        string change_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "Change", headerIndex.ToString() );
        string remove_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "Remove", headerIndex.ToString() );
        string update_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "Update", "" );

        string changeButtonLabel = String.Format( "<span style=\"position:relative;top:4px;\"><a href=# onclick='{0}' ><img src=\"images/Change.gif\" height=20 width=65 border=0></a></span>", change_JS );

        sectionTitleCell.Text = String.Format( "{0} {1}  <a href=# onclick='{2}' style=\"font-size:9pt;font-weight:normal;\" >remove</a>", adOption, changeButtonLabel, remove_JS );

        TableCell choiceCell = new TableCell();

        choiceCell.Text = "&nbsp;";

        TableCell costCell = new TableCell();
        costCell.Text = String.Format( "Cost&nbsp;per&nbsp;ad:{0}", Utils.FormatDollarAmount( adOptionCostPer ) );

        SetHeaderCellStyle( sectionTitleCell, headerIndex );
        SetHeaderCellStyle( choiceCell, headerIndex );
        SetHeaderCellStyle( costCell, headerIndex );
        sectionTitleCell.Style.Add( "padding-left", "20px" );
        sectionTitleCell.Style.Add( "padding-top", "5px" );
        sectionTitleCell.Style.Add( "font-size", "10pt" );
        sectionTitleCell.Style.Add( "font-weight", "bold" );

        row.Cells.Add( sectionTitleCell );
        row.Cells.Add( choiceCell );
        row.Cells.Add( costCell );

        table.Rows.Add( row );
    }

    private void SetHeaderCellStyle( TableCell cell, int headerIndex ){
        cell.Style.Add( "height", "38px" );
        cell.Style.Add( "background-image", "url(Images/BkgFadeBlu1.gif)" );
        cell.Style.Add( "background-repeat", "repeat-x" );
    }

    protected void AddTimingItemRow( Table table, TableRow timingRow ) {
        table.Rows.Add( timingRow );
    }

    protected void OKButton_Click( object sender, EventArgs e ) {
        //if( this.currentMediaPlan.Edited == true ) {
        //    if( this.currentMediaPlan.Results != null  ) {
        //        string suggestedNewName = Utils.GetSuggestedPlanName( this.currentMediaPlan.PlanDescription, this.currentMediaPlan.CampaignName, this.allPlanVersions, false );
        //        string nextVerJS = String.Format( "return GetNameForNextVersion( \"{0}\", false );", suggestedNewName );
        //        PageBody.Attributes.Add( "onLoad", nextVerJS );
        //        return;
        //    }

            ////SavePlan();
        //}
        Response.Redirect( "MediaDetails.aspx" );
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
        else if (this.currentMediaItem != null ){
            this.TypeSelection.Value = this.currentMediaItem.MediaType.ToString();
        }
    }

    private void SetTimeLimitFields() {
        this.PlanStartDay.Value = this.currentMediaPlan.Specs.StartDate.Day.ToString();
        this.PlanStartMonth.Value = this.currentMediaPlan.Specs.StartDate.Month.ToString();
        this.PlanStartYear.Value = this.currentMediaPlan.Specs.StartDate.Year.ToString();

        DateTime leadDate = this.currentMediaPlan.Specs.StartDate;
        if( this.TypeSelection.Value == "Radio" ) {
            leadDate = leadDate.AddDays( 14 );
        }
        else if( this.TypeSelection.Value == "Newspaper" ) {
            leadDate = leadDate.AddDays( 7 );
        }
        else if( this.TypeSelection.Value == "Magazine" ) {
            leadDate = leadDate.AddDays( 6 * 7 );
        }
        else if( this.TypeSelection.Value == "Yellowpages" ) {
            leadDate = leadDate.AddDays( 300 );
        }
        this.LeadDateDay.Value = leadDate.Day.ToString();
        this.LeadDateMonth.Value = leadDate.Month.ToString();
        this.LeadDateYear.Value = leadDate.Year.ToString();

        this.PlanEndDay.Value = this.currentMediaPlan.Specs.EndDate.Day.ToString();
        this.PlanEndMonth.Value = this.currentMediaPlan.Specs.EndDate.Month.ToString();
        this.PlanEndYear.Value = this.currentMediaPlan.Specs.EndDate.Year.ToString();
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
        //this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );
        this.allPlanVersions = Utils.AllPlanVersions( this );

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
