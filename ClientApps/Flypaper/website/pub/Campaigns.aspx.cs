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
using System.IO;

using WebLibrary;
using BusinessLogic;

public partial class Campaigns : System.Web.UI.Page
{
    // session-level variables
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string sortKey = "SortByName";
    protected string expandedItemIDsString = "";
    protected List<Guid> expandedItemIDs;

    protected bool showVersions = true;        //TBD - make this a user profile item

    protected List<MediaPlan> sortedCampaignPlans;

  
    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        bool viewOnly = false;
        if( this.User.IsInRole( "visitor" ) )
        {
            viewOnly = true;
           
        }

        AdminLink.Visible = false;
        if( this.User.IsInRole( "developer" ) )
        {
            AdminLink.Visible = true;
        }


        bool isNewUser = (currentMediaPlans == null);

        if( IsPostBack == false ){
            ReloadAllUserData();
        }

        VisitorInfo.Visible = false;
        if( viewOnly )
        {
            VisitorInfo.Visible = true;
            NewCampaignButton.Enabled = false;
            NewCampaignButton.BackColor = System.Drawing.Color.LightGray;
            NewCampaignButton.ToolTip = "Sign in to AdPlanit to enable features";
        }
        else if( isNewUser ) {
            ShowNewUserWelcome();
        }

        HandlePlanViewRequest();

        SetColumnSorting();

        SetExpandedItems();

        CampaignsList listMaker = new CampaignsList( Utils.GetUser( this ), viewOnly );
        sortedCampaignPlans = listMaker.AddCampaignListHTML( this.CampaignsListDiv, this.currentMediaPlans, ref this.allPlanVersions, null, this.sortKey, this.expandedItemIDs);
        //sortedCampaignPlans = listMaker.AddCampaignListHTML( this.CampaignsListDiv, this.currentMediaPlans, ref this.allPlanVersions, null, this.sortKey, this.expandedItemIDs, true, this.engineeringMode );
        SetupJavascriptVariables( sortedCampaignPlans );

        HandleActionRequest();
        HandleCampaignActionRequest();
    }

    /// <summary>
    /// Sets up the javaascript variables which contain the list of campaign names, the list of next-version plan names, and all of the existing plans in each campaign
    /// </summary>
    private void SetupJavascriptVariables( List<MediaPlan> sortedMediaPlans ) {
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = "ReceiveNewName();";
        
        string setCs = "SetCampaignNames( \"";
        string setNVs = "SetNextPlanNames( \"";
        for( int c = 0; c < sortedMediaPlans.Count; c++ ) {

            string nextPlanName = Utils.GetSuggestedPlanName( "Media Plan", sortedMediaPlans[ c ].CampaignName, this.allPlanVersions, false );

            setCs += sortedMediaPlans[ c ].CampaignName;
            setNVs += nextPlanName;

            if( c != sortedMediaPlans.Count - 1 ) {
                setCs += "^";
                setNVs += "^";
            }
        }
        setCs += "\");";
        setNVs += "\");";

        string setCPs = "";
        for( int c = 0; c < sortedMediaPlans.Count; c++ ) {
            string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( sortedMediaPlans[ c ].CampaignName, this.allPlanVersions ) );
            setCPs += "SetCampaignPlanNames(\"" + existingPlanNamesList + "\", " + c.ToString() + " );";
        }

        this.PageBody.Attributes.Add( "onload", setCs + setNVs + setCPs );
    }

    /// <summary>
    /// Deals with a request to perform an action on a campaign
    /// </summary>
    private void HandleCampaignActionRequest() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "DoCampaignAction" ) {
            string actionInfo = Request[ "__EVENTARGUMENT" ];

            int actIndx = actionInfo.IndexOf( "-" );
            string action = actionInfo.Substring( 0, actIndx );

            string campaignName = actionInfo.Substring( actIndx + 1 );

            switch( action ) {
                case "Edit":
                    EditCampaign( campaignName );
                    break;

                case "Delete":
                    DeleteCampaign( campaignName );
                    break;

                case "Scoreboard":
                    ScoreCompetitionCampaign( campaignName );
                    break;
            }
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "CopyCampaign" ) {
            int cIndx = int.Parse( Request[ "__EVENTARGUMENT" ] );
            string campToCopy = sortedCampaignPlans[ cIndx - 1 ].CampaignName;
            CopyCampaign( campToCopy );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "CreateNewPlan" ) {
            int cIndx = int.Parse( Request[ "__EVENTARGUMENT" ] );
            MediaPlan campToAddTo = sortedCampaignPlans[ cIndx - 1 ];

            AddPlanToCampaign( campToAddTo );
        }
    }

    private void EditCampaign( string campaignName ) {
        foreach( MediaPlan cPlan in this.currentMediaPlans ){
            if( cPlan.CampaignName == campaignName ){
                string targ = String.Format( "Campaign.aspx?p={0}", cPlan.PlanID.ToString() );
                Response.Redirect( targ );
           }
        }
        // should never get here
    }

    private void CopyCampaign( string campaignName ) {
        string nameForCopy = "Copy of " + campaignName;
        if( Request[ "newCampaignName" ] != null && Request[ "newCampaignName" ].Trim() != "" ) {
            nameForCopy = Request[ "newCampaignName" ].Trim();

            PlanStorage storage = new PlanStorage();
            storage.CopyMediaCampaign( Utils.GetUser( this ), campaignName, nameForCopy, this.allPlanVersions, this.currentMediaPlans );
            Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
            Utils.SetAllPlanVersions( this, this.allPlanVersions );
            Response.Redirect( "Campaigns.aspx" );       // make sure everything is refrshed
        }
    }

    private void ScoreCompetitionCampaign( string campaignName ) {
        foreach( MediaPlan cPlan in this.currentMediaPlans ) {

            string compOwnerCode = null;
            if( cPlan.Specs.CompetitionOwner != null ) {
                compOwnerCode = cPlan.Specs.CompetitionOwner.Replace( "@", "--" ).Replace( ".", "-" );
            }

            if( cPlan.CampaignName == campaignName ) {
                if( cPlan.Specs.CompetitionOwner == Utils.GetUser( this ) ) {
                    // viewing a competition that you initiated
                    string targ = String.Format( "Scoreboard.aspx?p={0}&u={1}", cPlan.PlanID, compOwnerCode );
                    Response.Redirect( targ );
                }
                else {
                    // viewing a competition that somebody else initiated
                    string targ = String.Format( "Scoreboard.aspx?p={0}&u={1}", cPlan.Specs.CompetitionBasePlanID, compOwnerCode );
                    Response.Redirect( targ );
                }
            }
        }
    }

    private void AddPlanToCampaign( MediaPlan protoPlan ) {
        MediaPlan newPlan = new MediaPlan( protoPlan.CampaignName );

        bool hasCurrentPlans = true;
        if( protoPlan.PlanValid == false ) {
            hasCurrentPlans = false;
        }

        int dt = -1;
        if( CreatePlanUseAuto.Checked == true ) {
            // autogenerate a plan
            DateTime t0 = DateTime.Now;
            AllocationService allocationService = new AllocationService();
            newPlan = allocationService.CreateNewMediaPlan( protoPlan.PlanSpecs );
            DateTime t1 = DateTime.Now;
            dt = (int)Math.Round( (t1 - t0).TotalMilliseconds );
        }

        newPlan.CopyCampaignDataFrom( protoPlan.Specs );
        PlanStorage storage = new PlanStorage();
        if( hasCurrentPlans == true ) {
            storage.IncrementPlanVersion( newPlan, this.allPlanVersions, userMedia.CurrentPlanVersions );
        }
        string nameForCopiedPlan = Request[ "newPlanName" ].Trim();
        newPlan.PlanDescription = nameForCopiedPlan;

        // change this current plan to the new one
        foreach( MediaPlan cplan in this.currentMediaPlans ) {
            if( cplan.CampaignName == protoPlan.CampaignName ) {
                currentMediaPlans.Remove( cplan );
                break;
            }
        }
        currentMediaPlans.Add( newPlan );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
        storage.SaveMediaPlan( Utils.GetUser( this ), newPlan );

        if( CreatePlanUseAuto.Checked == true ) {
            string timingSpecs = String.Format( "t={0:f3}", dt / 1000.0 );
            DataLogger.Log( "GEN-CL", Utils.GetUser( this ), newPlan.PlanDescription, timingSpecs );
        }
        else {
            DataLogger.Log( "CREATE-CL", Utils.GetUser( this ), newPlan.PlanDescription );
        }

        string targUrl = String.Format( "HQ.aspx?p={0}", newPlan.PlanID );
        Response.Redirect( targUrl );
    }

    private void DeleteCampaign( string campaignName ) {
        PlanStorage storage = new PlanStorage();
        storage.DeleteMediaCampaign( Utils.GetUser( this ), campaignName, this.allPlanVersions, this.currentMediaPlans );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
        Utils.SetAllPlanVersions( this, this.allPlanVersions );
        Response.Redirect( "Campaigns.aspx" );      // make sure everything is refrshed
    }

        /// <summary>
    /// Deals with a request to perform an action on a plan
    /// </summary>
    private void HandleActionRequest() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "DoAction" ) {
            string actionInfo = Request[ "__EVENTARGUMENT" ];

            int actIndx = actionInfo.IndexOf( "-" );
            string action = actionInfo.Substring( 0, actIndx );

            actionInfo = actionInfo.Substring( actIndx + 1 );

            int Indx1 = actionInfo.IndexOf( "::" );
            int Indx2 = actionInfo.LastIndexOf( "::" );

            string campaignName = actionInfo.Substring( 0, Indx1 );
            string planName = actionInfo.Substring( Indx1 + 2, Indx2 - Indx1 );
            string planVersion = actionInfo.Substring( Indx2 + 2 );

            switch( action ) {
                case "View":
                    EditCampaignPlanVersion( campaignName, planVersion, "Shop" );
                    break;

                case "Edit":
                    EditCampaignPlanVersion( campaignName, planVersion, "Shop" );
                    break;

                case "Order":
                    EditCampaignPlanVersion( campaignName, planVersion, "Order" );
                    break;

                case "Analyze":
                    EditCampaignPlanVersion( campaignName, planVersion, "Analyze" );
                    break;

                //case "Duplicate":
                //    DuplicatePlan( campaignName, planVersion );
                //    break;

                case "Delete":
                    DeletePlan( campaignName, planVersion );
                    break;

                case "Send":
                    SendPlan( campaignName, planVersion );
                    break;
            }
        }
        else {
            if( IsPostBack && Request[ "__EVENTTARGET" ] == "CopyPlan" ) {
                string actionInfo = Request[ "__EVENTARGUMENT" ];
                int splitIndx = actionInfo.IndexOf( "::" );
                int actionCampaignIndex = int.Parse( actionInfo.Substring( 0, splitIndx ) );
                string existingPlanName = actionInfo.Substring( splitIndx + 2 );
                string actionCampaign = this.sortedCampaignPlans[ actionCampaignIndex - 1 ].CampaignName;
                List<PlanStorage.PlanVersionInfo> actionVers = this.allPlanVersions[ actionCampaign ];
                string actionVersion = null;
                foreach( PlanStorage.PlanVersionInfo pvinfo in actionVers ) {
                    if( pvinfo.Description == existingPlanName ) {
                        // we found the plan!
                        actionVersion = pvinfo.Version;
                    }
                }
                if( actionVersion != null ) {
                    DuplicatePlan( actionCampaign, actionVersion );
                }
            }
        }
    }

    
    /// <summary>
    /// Starts the process of sending a media plan to another user.
    /// </summary>
    /// <param name="campaignName"></param>
    /// <param name="planVersion"></param>
    private void SendPlan( string campaignName, string planVersion ) {
        MediaPlan planToSend = SetCampaignPlanVersion( campaignName, planVersion );
        Utils.SetCurrentMediaPlan( this, planToSend );
        Response.Redirect( "SendPlan.aspx" );
    }

    /// <summary>
    /// Makes a duplicate of a media plan.
    /// </summary>
    /// <param name="campaignName"></param>
    /// <param name="planVersion"></param>
    private void DuplicatePlan( string campaignName, string planVersion ) {

        MediaPlan curVersionPlan = null;
        bool isExpanded = false;
        foreach( MediaPlan mp in this.currentMediaPlans ) {
            if( mp.CampaignName == campaignName ) {
                isExpanded = this.expandedItemIDs.Contains( mp.PlanID );
                this.expandedItemIDs.Remove( mp.PlanID );
            }
        }

        MediaPlan curPlan = SetCampaignPlanVersion( campaignName, planVersion );
        string newPlanDescription = "duplicate of " + curPlan.PlanDescription;
        if( Request[ "newPlanName" ] != null && Request[ "newPlanName" ].Trim() != "" ) {
            newPlanDescription = Request[ "newPlanName" ].Trim();
        }

        MediaPlan dupPlan = new MediaPlan( curPlan, newPlanDescription );

        if( isExpanded ) {
            this.expandedItemIDs.Add( dupPlan.PlanID );
        }

        this.ExpandedItemIDs.Value = GenerateFormValueFromList( this.expandedItemIDs );
        Session[ "expandedItemIDs" ] = this.ExpandedItemIDs.Value;

        PlanStorage storage = new PlanStorage();
        storage.IncrementPlanVersion( dupPlan, this.allPlanVersions, this.userMedia.CurrentPlanVersions );
        Profile.UserMediaItems = this.userMedia;
        Profile.Save();

        storage.SaveMediaPlan( Utils.GetUser( this ), dupPlan );
    //    ReloadAllUserData();
        DataLogger.Log( "DUP", Utils.GetUser( this ), dupPlan.PlanDescription );
        Response.Redirect( "Campaigns.aspx" );       // make sure everything is refreshed
    }

    /// <summary>
    /// Deletes a media plan
    /// </summary>
    /// <param name="campaignName"></param>
    /// <param name="planVersion"></param>
    private void DeletePlan( string campaignName, string planVersion ) {
        List<PlanStorage.PlanVersionInfo> versions = this.allPlanVersions[ campaignName ];
        PlanStorage storage = new PlanStorage();
        bool doDelete = true;

        // get the plan description so we can log its deletion
        string planDesc = null;
        foreach( PlanStorage.PlanVersionInfo pv in versions ){
            if( pv.Version == planVersion ){
                planDesc = pv.Description;
                break;
            }
        }
        if( planDesc != null ) {
            DataLogger.Log( "DELETE", Utils.GetUser( this ), planDesc );
        }

        if( versions.Count == 1 ) {
            // just make the last plan invalid; do not actually delete it
            MediaPlan planToInvalidate = SetCampaignPlanVersion( campaignName, planVersion );
            if( planToInvalidate.PlanValid == true ) {

                planToInvalidate.PlanValid = false;
                doDelete = false;
                storage.SaveMediaPlan( Utils.GetUser( this ), planToInvalidate );
            }
        }

        if( doDelete == true ) {
            storage.DeleteMediaPlan( Utils.GetUser( this ), campaignName, planVersion );
        }
        Response.Redirect( "Campaigns.aspx" );      // make sure everything is refreshed
    }

    /// <summary>
    /// Deals with a click on a plan name
    /// </summary>
    private void HandlePlanViewRequest() {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "SelectAndEditVersion" ) {
            string campAndVer = Request[ "__EVENTARGUMENT" ];
            int sp = campAndVer.LastIndexOf( "::" );

            string campaign = campAndVer.Substring( 0, sp );
            string version = campAndVer.Substring( sp + 2 );

            EditCampaignPlanVersion( campaign, version, "Shop" );
        }
    }

    /// <summary>
    /// Redirects to the given tab of the Edit Plan page after activating the specified version of the specified plan.
    /// </summary>
    /// <param name="campaign"></param>
    /// <param name="version"></param>
    /// <param name="targetTab"></param>
    private void EditCampaignPlanVersion( string campaign, string version, string targetTab ) {
        MediaPlan plan = SetCampaignPlanVersion( campaign, version );
        if( targetTab == "Shop" ) {
            Response.Redirect( String.Format( "HQ.aspx?p={0}", plan.PlanID ) );
        }
        else if( targetTab == "Order" ) {
            Response.Redirect( String.Format( "Order.aspx?p={0}", plan.PlanID ) );
        }
        else if( targetTab == "Analyze" ) {
            Response.Redirect( String.Format( "Analysis.aspx?p={0}", plan.PlanID ) );
        }
    }

    /// <summary>
    /// Sets the given version number of the given campaign to be actve
    /// </summary>
    /// <param name="campaign"></param>
    /// <param name="version"></param>
    /// <param name="targetTab"></param>
    /// <returns></returns>
    private MediaPlan SetCampaignPlanVersion( string campaign, string version ) {
        foreach( UserMedia.CurrentPlanVersion info in this.userMedia.CurrentPlanVersions ) {
            if( info.CampaignName == campaign ) {
                info.Version = version;
                Profile.UserMediaItems = this.userMedia;
                Profile.Save();
                break;
            }
        }
        // now change the plan version in the list
        for( int i = 0; i < this.currentMediaPlans.Count; i++ ) {
            MediaPlan mp = this.currentMediaPlans[ i ];
            if( mp.CampaignName == campaign ) {
                // found the plan - replace with the new version
                PlanStorage storage = new PlanStorage();
                this.currentMediaPlans[ i ] = storage.LoadMediaPlan( Utils.GetUser( this ), campaign, version );
                Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
                return this.currentMediaPlans[ i ];
            }
        }
        return null;
    }

    /// <summary>
    /// Deals with the selection of displayed items that are in an expanded state.
    /// </summary>
    private void SetExpandedItems() {

        if( Session[ "expandedItemIDs" ] != null ) {
            this.expandedItemIDsString = (string) Session[ "expandedItemIDs" ];
            this.ExpandedItemIDs.Value = this.expandedItemIDsString;
        }
        else {
            // preserve the existing expanded items on postback
            if( Request[ "ExpandedItemIDs" ] != null ) {
                this.expandedItemIDsString = Request[ "ExpandedItemIDs" ];
                this.ExpandedItemIDs.Value = this.expandedItemIDsString;
            }
        }

        this.expandedItemIDs = new List<Guid>();
        if( this.expandedItemIDsString.Length > 0 ) {
            string[] items = this.expandedItemIDsString.Split( ' ' );
            foreach( string idstr in items ) {
                expandedItemIDs.Add( new Guid( idstr ) );
            }
        }

        // see if the user wants to expand an item
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ExpandItem" ) {
            Guid expGuid = new Guid( Request[ "__EVENTARGUMENT" ] );
            if( expandedItemIDs.Contains( expGuid ) == false ) {
                expandedItemIDs.Add( expGuid );
            }
        }

        // see if the user wants to collapse an item
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "CollapseItem" ) {
            Guid collapseGuid = new Guid( Request[ "__EVENTARGUMENT" ] );
            if( expandedItemIDs.Contains( collapseGuid ) == true ) {
                expandedItemIDs.Remove( collapseGuid );
            }
        }

        this.ExpandedItemIDs.Value = GenerateFormValueFromList( expandedItemIDs );
        Session[ "expandedItemIDs" ] = this.ExpandedItemIDs.Value;
    }

    private string GenerateFormValueFromList( List<Guid> expandedItemIDs ) {
        // regenerate the single string representation (for the hidden form field)
        string formString = "";
        for( int i = 0; i < expandedItemIDs.Count; i++ ) {
            formString += expandedItemIDs[ i ].ToString();
            if( i < expandedItemIDs.Count - 1 ) {
                formString += " ";        // separate the items
            }
        }
        return formString;
    }

    protected void CreateCampaign_Click( object sender, EventArgs e ) {
        Utils.SetCurrentMediaPlan( this, null );
        Response.Redirect( "Campaign.aspx" );
    }

    /// <summary>
    /// Deals with the sort-column setting and changes to it.
    /// </summary>
    private void SetColumnSorting() {
        // preserve the existing sorting on postback
        if( Request[ "SortColumnName" ] != null ) {
            sortKey = Request[ "SortColumnName" ];
        }
        if( sortKey == "" ) {
            // default!
            this.SortColumnName.Value = "SortByName";
            this.sortKey = "SortByName";
        }

        if( IsPostBack && Request["__EVENTTARGET"] != null &&  Request["__EVENTTARGET"].StartsWith("SortBy" ))
        {
            //sort items within campaign by name
            SetSortColumn( Request["__EVENTTARGET"]);
        }
    }

    /// <summary>
    /// Sets the column to sort the plans in each campaign
    /// </summary>
    /// <param name="sortColName"></param>
    protected void SetSortColumn( string sortColName ) {
        this.sortKey = sortColName;
        this.SortColumnName.Value = this.sortKey;
        if( Utils.UserIsDemo( this ) == false ) {
            this.userMedia.plansListSortColumn = sortColName;
            // save the profile data to the session...
            Utils.SetCurrentUserProfile( this, this.userMedia );
            //...and also to the Profiile itself
            Profile.UserMediaItems = this.userMedia;
            Profile.Save();
        }
    }

    /// <summary>
    /// Loads plans, campaignns, and profile data for the currently logged-in user.
    /// </summary>
    private void ReloadAllUserData() {
        // load data for a new user (who has just logged in) -- first, reload data from the User Profile database
        this.userMedia = Profile.UserMediaItems;
        Utils.SetCurrentUserProfile( this, this.userMedia );

        this.currentMediaPlans = new List<MediaPlan>();
        bool curVersionsChanged = false;
        List<UserMedia.CurrentPlanVersion> curVersions = this.userMedia.CurrentPlanVersions;
        PlanStorage storage = new PlanStorage();

        // load the media plans
        this.currentMediaPlans.AddRange( storage.LoadCurrentPlans( Utils.GetUser( this ), out this.allPlanVersions, ref curVersions, out curVersionsChanged ) );

        if( curVersionsChanged ) {
            this.userMedia.CurrentPlanVersions = curVersions;
            Utils.SetCurrentUserProfile( this, this.userMedia );
            Profile.UserMediaItems = this.userMedia;
            Profile.Save();
        }

        // auto-set engineering mode if the user has that configuration selected
        if( this.userMedia.StartInEngineeringMode == true && this.engineeringMode == false ) {
            this.engineeringMode = true;
            Utils.SetEngineeringMode( this, this.engineeringMode );
            //Utils.SetEngineeringModeDisplay( this.PhoneLabel );
        }

        Utils.SetRunningMediaPlanIDs( this, null );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
        Utils.SetAllPlanVersions( this, this.allPlanVersions );
    }

    private void ShowNewUserWelcome() {
        string userName = Utils.GetUser( this );
        userName = userName.Replace( "@", "@<br>" );

        if( this.userMedia != null ) {
            if( this.userMedia.FirstName != null && this.userMedia.FirstName != "" ) {
                userName = this.userMedia.FirstName;
            }
        }
        RHInfoArea.InnerHtml = String.Format( "<br><br>Welcome to AdPlanit, <br>{0}", userName );
        DataLogger.Log( "LOGIN", Utils.GetUser( this ) );
        if( this.currentMediaPlans.Count == 0 ) {
            RHInfoArea.InnerHtml += "<br><br>Click the \"<b>Create New Campaign</b>\" button to start creating your first Media Campaign with AdPlanit.";
        }
    }


    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.allPlanVersions = Utils.AllPlanVersions( this );
        this.engineeringMode = Utils.InEngineeringMode( this, null );

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }

        this.sortKey = this.userMedia.plansListSortColumn;
        SortColumnName.Value = this.sortKey;

        string dir = HttpContext.Current.Server.MapPath(null);
        string mediaInfoDirectory = dir + @"\Data";

        Utils.InitNames( mediaInfoDirectory );

        if (!Scoring.LoadedStats)
        {
            string stats_file = mediaInfoDirectory + @"\stats.dat";
            if (File.Exists(stats_file) == false)
            {
                string msg = String.Format("Error: Scoring statistics file not found: {0}", stats_file);
                throw new Exception(msg);
            }
            FileStream fs = new FileStream(stats_file, FileMode.Open, FileAccess.Read);
            Scoring.Load_Stats(fs);
            fs.Close();
        }

        if (!Scoring.LoadedWeights)
        {
            string weights_file = mediaInfoDirectory + @"\scoring_values.txt";
            if (File.Exists(weights_file) == false)
            {
                string msg = String.Format("Error: Scoring weights file not found: {0}", weights_file);
                throw new Exception(msg);
            }
            FileStream fs = new FileStream(weights_file, FileMode.Open, FileAccess.Read);
            StreamReader str = new StreamReader(fs);
            Scoring.Load_Weights(str);
            str.Close();
            fs.Close();
        }

        if (!AllocationService.LoadedMatrices)
        {
            string matrix_file = mediaInfoDirectory + @"\plan_gen_values.txt";
            if (File.Exists(matrix_file) == false)
            {
                string msg = String.Format("Error: Scoring weights file not found: {0}", matrix_file);
                throw new Exception(msg);
            }
            FileStream fs = new FileStream(matrix_file, FileMode.Open, FileAccess.Read);
            StreamReader str = new StreamReader(fs);
            AllocationService.Load_Matrices(str);
            str.Close();
            fs.Close();
        }

        if (!Calibrator.LoadedStats)
        {
            string awareness_file = mediaInfoDirectory + @"\awareness.dat";
            string persuasion_file = mediaInfoDirectory + @"\persuasion.dat";
            if (!File.Exists(awareness_file))
            {
                string msg = String.Format("Error: Input calibration stats file not found: {0}", awareness_file);
                throw new Exception(msg);
            }
            if (!File.Exists(persuasion_file))
            {
                string msg = String.Format("Error: Input calibration stats file not found: {0}", persuasion_file);
                throw new Exception(msg);
            }
            FileStream a_fs = new FileStream(awareness_file, FileMode.Open, FileAccess.Read);
            FileStream p_fs = new FileStream(persuasion_file, FileMode.Open, FileAccess.Read);
            Calibrator.Load_Stats(p_fs, a_fs);
            a_fs.Close();
            p_fs.Close();
        }

        Utils.RefreshMediaDatabase();
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
