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
using MediaLibrary;
using BusinessLogic;

public partial class MediaDetails : System.Web.UI.Page
{
    protected string mediaTypeName;
    protected string mediaTypeBudget;

    protected int avaiItemsPerPage = 25;
    protected int viewPageNum = 1;
    protected int viewPageTotal;
    protected int viewVehicleTotal;

    // filter values
    protected string availMediaRegion = null;
    protected int availMediaAdOptionID = -1;
    protected double availMediaOptionCostMod = 1.0;

    protected string sortKey = "score";

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected MediaPlan currentMediaItem;
    protected bool engineeringMode;
    protected UserMedia userMedia;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected AvailableMediaListMaker availMedia;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();
        
        SetMediaType();

        string mType = this.MediaType.Value;
        if( mType == "Yellow Pages" ) {
            mType = "Yellowpages";
        }
        MediaDetailsMaker yourMedia = new MediaDetailsMaker( this.currentMediaPlan, mType, this.userMedia );

        List<MediaItem> itemsOfType = yourMedia.GetTypeItems();

        CheckForDetailsEdit( itemsOfType );

        HandleRemoveRequest( itemsOfType );

        FillFilterMenus( this.MediaType.Value, itemsOfType );

        SetColumnSorting();

        SetPageNumber();

        SetNameChangeLink();

        HandleNameChangeRequest();

        // get the available media items
        availMedia = new AvailableMediaListMaker( this.currentMediaPlan, this.MediaType.Value, this.userMedia, this.availMediaAdOptionID, this.availMediaOptionCostMod, this.availMediaRegion );
        Dictionary<MediaVehicle, double> availItems = availMedia.GetVehiclesList();

        List<MediaVehicle> sortedVehicles;
        List<double> sortedRatings;
        availMedia.AddAvailableMediaListHTML( this.AvailMedia, sortKey, out sortedVehicles, out sortedRatings, ref this.viewPageNum, this.avaiItemsPerPage,
            out this.viewPageTotal , out this.viewVehicleTotal );

        SetPageLinkState();

        HandleAddRequest( sortedVehicles, sortedRatings );

        if( IsPostBack == true && Request[ "__EVENTTARGET" ] ==  "ChangePlanName" ) {
            // this is needed to handle automatic new-version generation on saving -- but it causes plan renaming via the link to be immediate 
            SaveButton_Click( null, null );
        }

        SetSaveButtonState();

        // display the media items
        yourMedia.AddDetailsTableHTML( this.YourMedia );

        // display the budget
        this.mediaTypeBudget = Utils.FormatDollarAmount( this.currentMediaPlan.GetTypeSpending( MediaVehicle.GetType( this.MediaType.Value ) ) );

        SetEditedWarningsForTabs();

        if(this.User.IsInRole( "visitor"))
        {
            SaveButton.Enabled = false;
            SaveButton.ToolTip = "Sign in to AdPlanit to enable features";

            ChangePlanLink.Visible = false;

            SaveNeededDiv.InnerHtml = "";

            SaveButton.ImageUrl = "images/SaveButton2Disabled.gif";
            ReturnButton.ImageUrl = "images/ReturnToHQ.gif";
        }

        VisitorInfo.Visible = false;
        if( this.User.IsInRole( "visitor" ) )
        {
            VisitorInfo.Visible = true;
        }
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

    protected void SaveButton_Click( object sender, EventArgs e ) {
        if( this.currentMediaPlan.Edited == true ){
            if( this.currentMediaPlan.Results != null ) {
                // change the plan version
                PlanStorage storage = new PlanStorage();
                storage.IncrementPlanVersion( this.currentMediaPlan, this.allPlanVersions, this.userMedia.CurrentPlanVersions );
                this.currentMediaPlan.Results = null;
                this.currentMediaPlan.PlanOverallRatingStars = -1;

                if( Request[ "newPlanName" ] != null ) {
                    this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
                }

                // update the user settings
                foreach( UserMedia.CurrentPlanVersion info in this.userMedia.CurrentPlanVersions ) {
                    if( info.CampaignName == this.currentMediaPlan.CampaignName ) {
                        info.Version = this.currentMediaPlan.PlanVersion;
                        Profile.UserMediaItems = this.userMedia;
                        Profile.Save();
                    }
                }
            }

            SavePlan();
        }
    }

    protected void ReturnButton_Click( object sender, EventArgs e ) {
        if( this.currentMediaPlan.Edited == true ) {
            RevertToSavedVersion();
        }

        Response.Redirect( "HQ.aspx" );
    }

    private void SavePlan() {
        PlanStorage storage = new PlanStorage();
        storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );

        // make sure the saved-version info has the correct total cost
        List<PlanStorage.PlanVersionInfo> campPlanVers = this.allPlanVersions[ this.currentMediaPlan.CampaignName ];
        foreach( PlanStorage.PlanVersionInfo pInfo in campPlanVers ) {
            if( pInfo.PlanID == this.currentMediaPlan.PlanID ) {
                pInfo.TotalCost = this.currentMediaPlan.SumOfItemBudgets;
            }
        }

        SetSaveButtonState();
    }

    private void RevertToSavedVersion() {
        PlanStorage storage = new PlanStorage();
        this.currentMediaPlan = storage.LoadMediaPlan( Utils.GetUser( this ), this.currentMediaPlan.CampaignName, this.currentMediaPlan.PlanVersion );
        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        SetSaveButtonState();
    }

    /// <summary>
    /// See if the user wants to go to a details-editing page
    /// </summary>
    /// <param name="itemsList"></param>
    private void CheckForDetailsEdit( List<MediaItem> itemsList ) {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "SetTiming" ) {
            Session[ "CurrentMediaType" ] = this.TypeSelection.SelectedItem.Text;
            OpenDetailsPage( itemsList, "Timing.aspx" );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "SetProminence" ) {
            Session[ "CurrentMediaType" ] = this.TypeSelection.SelectedItem.Text;
            OpenDetailsPage( itemsList, "Timing.aspx" );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "SetTargeting" ) {
            Session[ "CurrentMediaType" ] = this.TypeSelection.SelectedItem.Text;
            OpenDetailsPage( itemsList, "Targeting.aspx" );
        }
    }

    private void OpenDetailsPage( List<MediaItem> itemsList, string pageUrl ) {
        int which = int.Parse( Request[ "__EVENTARGUMENT" ] );
        MediaItem edItem = itemsList[ which ];
        Session[ "CurrentMediaItem" ] = edItem;
        Response.Redirect( pageUrl );
    }

    /// <summary>
    /// Responds to a click on the Add button
    /// </summary>
    private List<MediaVehicle> HandleAddRequest( List<MediaVehicle> sortedVehicles, List<double> sortedRankings )
    {
        List<MediaVehicle> addedItems = new List<MediaVehicle>();

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "AddItem" ) {

            foreach( string key in Request.Params.AllKeys ) {
                if( key.StartsWith( "Add-" ) ) {
                    string idStr = key.Substring( 4 );
                    Guid addVehicleID = new Guid( idStr );
                    for( int i = 0; i < sortedVehicles.Count; i++ ) {
                        if( sortedVehicles[ i ].Guid == addVehicleID ) {
                            // found one!
                            addedItems.Add( sortedVehicles[ i ] );
                            this.currentMediaPlan.AddMediaItem( sortedVehicles[ i ], this.availMediaAdOptionID );
                        }
                    }
                }
            }

            //// find out which ones to add
            //for( int i = 0; i < sortedVehicles.Count; i++ ) {
            //    string addKey = String.Format( "Add-{0}", i );
            //    if( Request[ addKey ] != null ) {
            //        // found one!
            //        addedItems.Add( sortedVehicles[ i ] );
            //        this.currentMediaPlan.AddMediaItem( sortedVehicles[ i ], this.availMediaAdOptionID );
            //    }
            //}
        }

        if( addedItems.Count > 0 ) {
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }
        return addedItems;
    }

    /// <summary>
    /// Responds to a click on the Remove button
    /// </summary>
    private List<MediaItem> HandleRemoveRequest( List<MediaItem> itemsInList ) {
        List<MediaItem> removedItems = new List<MediaItem>();

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "RemoveItem" ) {

            // find out which ones to remove
            for( int i = 0; i < itemsInList.Count; i++ ) {
                string removeKey = String.Format( "Remove-{0}", i );
                if( Request[ removeKey ] != null ) {
                    // found one!
                    removedItems.Add( itemsInList[ i ] );
                    this.currentMediaPlan.RemoveMediaItem( itemsInList[ i ] );
                }
            }
        }

        if( removedItems.Count > 0 ) {
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }
        return removedItems;
    }

    private void SetPageLinkState() {
        int itemStart = ((this.viewPageNum - 1) * this.avaiItemsPerPage) + 1;
        int itemEnd = (int)Math.Min( itemStart + this.avaiItemsPerPage - 1, this.viewVehicleTotal );
        string s = String.Format( "<table cellpadding=0 cellspacing=0 width='98%' ><tr><td style=\"text-align:left;\" >Items {0}-{1} of {2}</td><td style=\"text-align:right;\">", itemStart, itemEnd, this.viewVehicleTotal );
        int nPages = (int)Math.Ceiling( (double)this.viewVehicleTotal / (double)this.avaiItemsPerPage );

        string prevPageJS = String.Format( "__doPostBack( \"ShowAvailPage\", {0} );", this.viewPageNum - 1 );
        string nextPageJS = String.Format( "__doPostBack( \"ShowAvailPage\", {0} );", this.viewPageNum + 1 );

        if( nPages > 1 ) {
            s += "Page: ";
            if( this.viewPageNum > 1 ) {
                s += String.Format( "<a href='#' onclick='{0}' >Prev</a>&nbsp;&nbsp;", prevPageJS );
            }
            if( itemEnd != this.viewVehicleTotal ) {
                s += String.Format( "<a href='#' onclick='{0}' >Next</a>&nbsp;&nbsp;", nextPageJS );
            }
            s += "<span ID='PageNums' >";
            for( int i = 0; i < nPages; i++ ) {
                if( this.viewPageNum != i + 1 ) {
                    string selectPageJS = String.Format( "__doPostBack( \"ShowAvailPage\", {0} );", i + 1 );
                    s += String.Format( "<a href='#' onclick='{0}' class='PageLink' >{1}</a> ", selectPageJS, i + 1 );
                }
                else {
                    s += String.Format( "<b class='PageNum' >{0}</b> ", i + 1 );
                }
            }
            s += "</span></td></table>";
        }

        MoreLink.InnerHtml = s;
        MoreLink2.InnerHtml = s;
    }

    /// <summary>
    /// Set the state of the buttons to be appropropriate for the deited/not-edited state of the plan.
    /// </summary>
    private void SetSaveButtonState() {
        PlanNameLabel.InnerText = Utils.ElideString( this.currentMediaPlan.PlanDescription, Utils.MaxLengthPlanName );

        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        Utils.SetupNextVersionNamePopup( this.currentMediaPlan, this.SaveButton, existingPlanNamesList, this.allPlanVersions );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}' );", existingPlanNamesList );
        NewNameBox.Attributes[ "onkeydown" ] = "return CheckForEnter(event);";

        if(this.currentMediaPlan.Edited == true ) {
            SaveNeededDiv.InnerHtml = "(needed)";
            SaveButton.ImageUrl = "images/SaveButton2.gif";
            ReturnButton.ImageUrl = "images/ReturnToHQNoSave.gif";
        }
        else
        {
            SaveNeededDiv.InnerHtml = "(not needed)";
            SaveButton.ImageUrl = "images/SaveButton2Disabled.gif";
            ReturnButton.ImageUrl = "images/ReturnToHQ.gif";
        }
    }

    /// <summary>
    /// Fill the menus offering the choices of region and prominence for the available media list
    /// </summary>
    /// <param name="planItems"></param>
    /// <param name="availItems"></param>
    private void FillFilterMenus( string mediaType, List<MediaItem> planItems ) {
        if( IsPostBack == false || (Request[ "__EVENTTARGET" ] == "TypeSelection") ) {

            availMedia = new AvailableMediaListMaker( this.currentMediaPlan, this.MediaType.Value, this.userMedia, this.availMediaAdOptionID, this.availMediaOptionCostMod, this.availMediaRegion );
            Dictionary<SimpleOption, double> all_options = availMedia.GetProminenceList();

            ProminenceSelection.Items.Clear();
            foreach( SimpleOption option in all_options.Keys ) {
                ProminenceSelection.Items.Add( new ListItem( option.Name, option.ID.ToString() ) );
            }
            ProminenceSelection.SelectedIndex = 0;

            RegionSelection.Items.Clear();

            //foreach( DemographicSettings demo in this.currentMediaPlan.Specs.Demographics ) {
            //    RegionSelection.Items.Add( demo.LocationDescription() );
            //}
            foreach( string rgn in this.currentMediaPlan.Specs.GeoRegionNames ) {
                RegionSelection.Items.Add( rgn );
            }
            this.availMediaRegion = RegionSelection.SelectedItem.Text;

            if (ProminenceSelection.SelectedValue == "-")
            {
                this.availMediaAdOptionID = -1;
            }
            else
            {
                this.availMediaAdOptionID = Int32.Parse(ProminenceSelection.SelectedValue);
                AdOption option = Utils.MediaDatabase.GetAdOption(availMediaAdOptionID);
                SimpleOption simp_option = option as SimpleOption;
                if (simp_option != null)
                {
                    this.availMediaOptionCostMod = simp_option.Cost_Modifier;
                }
                else
                {
                    this.availMediaOptionCostMod = 1;
                }
            }
        }
        else {
            if (ProminenceSelection.SelectedValue == "-")
            {
                this.availMediaAdOptionID = -1;
            }
            else
            {
                this.availMediaAdOptionID = Int32.Parse(ProminenceSelection.SelectedValue);
                AdOption option = Utils.MediaDatabase.GetAdOption(availMediaAdOptionID);
                SimpleOption simp_option = option as SimpleOption;
                if (simp_option != null)
                {
                    this.availMediaOptionCostMod = simp_option.Cost_Modifier;
                }
                else
                {
                    this.availMediaOptionCostMod = 1;
                }
            }
            this.availMediaRegion = RegionSelection.SelectedItem.Text;
        }
    }

    /// <summary>
    /// Sets the media type for the display.
    /// </summary>
    private void SetMediaType() {
        // set the hidden form field
        if( Request[ "t" ] != null ) {
            this.MediaType.Value = Request[ "t" ];
        }
        else if( Request[ "MediaType" ] != null ) {
            this.MediaType.Value = Request[ "MediaType" ];
        }

        if( Request[ "TypeSelection" ] != null ) {
            this.MediaType.Value = Request[ "TypeSelection" ];
        }

        // perhaps we've been away editing timing, etc.
        if( this.MediaType.Value == "" ) {
            this.MediaType.Value = (string)Session[ "CurrentMediaType" ];
        }

        // get the display name of the type
        this.mediaTypeName = this.MediaType.Value.Substring( 0, 1 ).ToUpper() + this.MediaType.Value.Substring( 1 ).ToLower();
        if( this.mediaTypeName == "Yellow pages" ) {
            this.mediaTypeName = "Yellowpages";
        }

        // fill the dropdown list of types 
        List<int> allTypeIDs = this.currentMediaPlan.GetTypes();
        List<string> allTypes = new List<string>();
        foreach( int typeID in allTypeIDs ) {
            allTypes.Add( Utils.MediaDatabase.GetTypeForID( typeID ).ToString() );
        }
        allTypes.Sort();
        TypeSelection.Items.Clear();
        for( int i = 0; i < allTypes.Count; i++ ) {
           string typeName = allTypes[ i ].Substring( 0, 1 ).ToUpper() + allTypes[ i ].Substring( 1 ).ToLower();
           string dispTypName = typeName;
           if( dispTypName == "Yellowpages" ) {
               dispTypName = "Yellow Pages";
           }
           TypeSelection.Items.Add( new ListItem( dispTypName, typeName ) );
            if( typeName == this.mediaTypeName ){
                TypeSelection.SelectedIndex = i;
            }
        }

        //availMedia = new AvailableMediaListMaker(this.currentMediaPlan, this.MediaType.Value, this.userMedia, this.availMediaAdOptionID, this.availMediaRegion);

    }

    private void SetPageNumber() {
        this.viewPageNum = 1;
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ShowAvailPage" ) {
            this.viewPageNum = int.Parse( Request[ "__EVENTARGUMENT" ] );
        }
        else if( Request[ "pageNumber" ] != null ) {
            this.viewPageNum = int.Parse( Request[ "pageNumber" ] );
        }
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "TypeSelection" ) {
            this.viewPageNum = 1;
        }

        this.pageNumber.Value = this.viewPageNum.ToString();
   }


    /// <summary>
    /// Deals with the sort-column setting and changes to it.
    /// </summary>
    private void SetColumnSorting() {
        // preserve the existing sorting on postback
        bool sortKeySet = false;
        if( Request[ "MediaSortColumnName" ] != null ) {
            this.sortKey = Request[ "MediaSortColumnName" ];
            sortKeySet = true;
        }

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "SortByNameLink" ) {
            //sort items within campaign by name
            SetSortColumn( "name" );
            sortKeySet = true;
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "SortByScoreLink" ) {
            //sort items within campaign by name
            SetSortColumn( "score" );
            sortKeySet = true;
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "SortByDateLink" ) {
            //sort items within campaign by name
            SetSortColumn( "date" );
            sortKeySet = true;
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "SortByPriceLink" ) {
            //sort items within campaign by name
            SetSortColumn( "price" );
            sortKeySet = true;
        }

        if( sortKeySet == false ) {
            if( this.userMedia.availableMediaSortColumn != null ) {
                // get the value from the users profile
                this.sortKey = this.userMedia.availableMediaSortColumn;
            }
            else {
                // default!
                this.MediaSortColumnName.Value = "score";
                this.sortKey = "score";
            }
        }
    }

    /// <summary>
    /// Sets the column to sort the plans in each campaign
    /// </summary>
    /// <param name="sortColName"></param>
    protected void SetSortColumn( string sortColName ) {
        this.sortKey = sortColName;
        this.MediaSortColumnName.Value = this.sortKey;
        if( Utils.UserIsDemo( this ) == false ) {
            this.userMedia.availableMediaSortColumn = sortColName;
            // save the profile data to the session...
            Utils.SetCurrentUserProfile( this, this.userMedia );
            //...and also to the Profiile itself
            Profile.UserMediaItems = this.userMedia;
            Profile.Save();
        }
    }


    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        //this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );
        this.allPlanVersions = Utils.AllPlanVersions( this );

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
