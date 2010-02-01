using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebLibrary;
using BusinessLogic;
using HouseholdLibrary;
using SimInterface;
using MediaLibrary;
using DemographicLibrary;

public partial class PreSimRun : System.Web.UI.Page
{
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    
    // TBD not using
    //protected List<MediaPlan> plansBeingEdited;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string DisplayData = "";

    protected const string adFooter = "";

    // ---------  values for engineering debug  ---------
    private const double fakePersuasionIndexValue = 0.075; // SR (index/val) thresholds 0.9, 0.8, 0.6
    //private const double fakePersuasionValue = 0.1;
    private const double fakePersuasionValue = 0.0000123;
    private const double fakeReferencePersuasionValue = 0.02;

    // awareness thresholds are 30, 60, 80
    //private const double fakeAwarenessValue = 0.65;
    private const double fakeAwarenessValue = 0.0000065;

    private const double fakeRecencyValue = 0.05;

    private const double fakeReach1Value = 0.85;  // thresholds 90, 75, 50
    private const double fakeReach3Value = 0.85;  // thresholds 80, 60, 30
    private const double fakeR3r = 0.85;
    private const double fakeReach4Value = 0.10;
    // ---------

    //protected const string progressImageTag = "<img src=\"images/1animated7.gif\" width=\"133\" height=\"100\" >";
    protected const string progressImageTag = "<b>Please wait while the simulation is initialized...</b>";

        //"<asp:Button ID=\"AbortButton\" runat=\"server\" OnClick=\"AbortButton_Click\" Text=\"Abort\" Visible=\"true\" />";

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        if( engineeringMode == false ) {
            Body.Attributes.Add( "onLoad", "startTimer();" );
        }

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }

        ProgressImage.InnerHtml = progressImageTag;

        if( this.engineeringMode == true ) {
            DebugProceedButton.Visible = true;
            DebugSkipSimButton.Visible = true;
        }

        StoryDisplayer sDisp = new StoryDisplayer( StoryGenerator.GetStoryGenerator( this ), "Initializing simulation...", this.engineeringMode );
        sDisp.PopulateDisplayDiv( this.StoryDiv, this.StoryBodyDiv, this.StoryFooterDiv, 0 );

        if( IsPostBack == false ) {
            // display status for the initial visit to this page
            int numToRun = 0;
            int numRunning = 0;
            // TBD not using
            //if( this.plansBeingEdited != null ) {
            //    numToRun = this.plansBeingEdited.Count;
            //}
            if( this.runningPlans != null ) {
                numRunning = this.runningPlans.Count;
            }
            DisplayData = String.Format( "Preparing to run sims: {0} <br><br>Sims running now: {1}", numToRun, numRunning );
            SetBodyText( DisplayData );

            // engineering-mode-only stuff (dump sim details)
            if( this.engineeringMode == true ) {
                string s = "Engineering-mode data:<br>";

                // TBD not using
                //if( this.plansBeingEdited != null ) {
                //    for( int d = 0; d < this.plansBeingEdited.Count; d++ ) {
                //        MediaPlan planToRun = this.plansBeingEdited[ d ];
                //        SimInterface.SimInput testInput = SimUtils.ConvertMediaPlanToSimInput( planToRun, this.userMedia );
                //        s += DebugDumpStringFor( d, planToRun, testInput );
                //    }
                //}
                DebugExtraText.Text = s;
            }
        }
        else {
            // this is a timer postback
            if( this.engineeringMode == false ){
                //Server.Transfer( "SimulatePlan.aspx" );    // causes exceptiion creating child page!!
                Response.Redirect( "Simulation/Simulate.aspx" );
            }
        }

        if( this.currentMediaPlan != null ) {
            CampaignSummaryDiv.InnerHtml = this.currentMediaPlan.Specs.GetCampaignSummary( adFooter );
        }
    }

    #region Engineering-Mode Items
    // for engineering-mode "proceed" button
    protected void GoToNext_Click( object sender, EventArgs e ) {
        Response.Redirect( "Simulation/Simulate.aspx" );
    }


    protected void AbortButton_Click( object sender, EventArgs e ) {
        List<Guid> plansStillRunning = new List<Guid>();
        Utils.SetRunningMediaPlanIDs( this, plansStillRunning );
        Response.Redirect( "HQ.aspx" );
    }


    // for engineering-mode "skip sim" button
    protected void GoToResults_Click( object sender, EventArgs e ) {
        this.runningPlans = new List<Guid>();

        // TBD not using
        //foreach( MediaPlan edPlan in this.plansBeingEdited ) {
        //    int nDays = (int)((edPlan.EndDate - edPlan.StartDate).TotalDays);

        //    StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.SimulationStartedElement( edPlan.CampaignName, edPlan.PlanName ) );

        //    Random rand = new Random();

        //    // generate fake results!

        //    edPlan.Results = new SimOutput();
        //    edPlan.Results.metrics = new List<Metric>();

        //    for( int s = 0; s < edPlan.SegmentCount; s++ ) {

        //        // add faked metrics
        //        edPlan.Results.metrics.Add( FakedMetric( 0, nDays, "Persuasion", fakePersuasionValue ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 1, nDays, "PersuasionIndex", fakePersuasionIndexValue ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 2, nDays, "ReferencePersuasion", fakeReferencePersuasionValue ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 3, nDays, "Awareness", fakeAwarenessValue ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 4, nDays, "Recency", fakeRecencyValue ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 5, nDays, "Reach1", fakeReach1Value ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 6, nDays, "Reach3", fakeReach3Value ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 7, nDays, "R3r", fakeR3r ) );
        //        edPlan.Results.metrics.Add( FakedMetric( 8, nDays, "Reach4", fakeReach4Value ) );
        //    }
        //    edPlan.PlanStatus = MediaPlan.MediaPlanStatus.SIMULATED;

        //    BusinessLogic.Scoring scoreGen = new BusinessLogic.Scoring( edPlan);
        //    string issuesSummary = null;
        //    string nextStarSteps = null;
        //    edPlan.PlanOverallRatingStars = 3;

        //    PlanStorage storage = new PlanStorage();
        //    storage.SaveMediaPlan( Utils.GetUser( this ), edPlan );

        //    StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.SimulationDoneElement( edPlan.CampaignName, edPlan.PlanName,
        //        edPlan.PlanOverallRatingStars, issuesSummary, null ) );

        //    runningPlans.Add( edPlan.PlanID );
        //}

        // TBD not using SSN
        //this.plansBeingEdited = new List<MediaPlan>();
        //Utils.SetPlansBeingEdited( this, this.plansBeingEdited );

        Utils.SetRunningMediaPlanIDs( this, this.runningPlans );

        Response.Redirect( "SimulationResults.aspx" );
    }

    /// <summary>
    /// Returns a faked results metric (a time-series result item).
    /// </summary>
    /// <param name="metricID"></param>
    /// <param name="nDays"></param>
    /// <param name="metricName"></param>
    /// <param name="metricValue"></param>
    /// <returns></returns>
    private Metric FakedMetric( int metricID, int nDays, string metricName, double metricValue ) {
        Metric fakeMetric = new Metric();
        fakeMetric.Identifier = metricID;
        fakeMetric.Type = metricName;
        fakeMetric.values = new List<double>();
        for( int d = 0; d < nDays; d++ ) {
            fakeMetric.values.Add( metricValue );
        }
        return fakeMetric;
    }
    #endregion

    /// <summary>
    /// Refreshes the contents of the UpdatePanel
    /// </summary>
    /// <param name="txt"></param>
    private void SetBodyText( string txt ) {
        this.BodyInfo.Text = txt + "<br><br>Checked at: " + DateTime.Now.ToString( "h:mm:ss" );
    }

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );

        // TBD not using
        // this.plansBeingEdited = Utils.PlansBeingEdited( this );
        this.runningPlans = Utils.RunningMediaPlanIDs( this );

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

    #region More Engineering-Mode Items
    //---------------- debug stuff ---------------
    private string DebugDumpStringFor( int planIndx, MediaPlan plan, SimInterface.SimInput planInput ) {
        string s = "";

        s += String.Format( "<b>Sim {0}: {1}</b> segments={2} media={3}<br>", planIndx + 1, plan.PlanName, plan.DemographicCount, plan.GetTypes().Count );
        for( int d = 0; d < planInput.Demographics.Count; d++ ) {
            Demographic demo = planInput.Demographics[ d ];
 //           s += String.Format( " Seg {0}: {1} - {2}<br>{3}<br><br>", d + 1, demo.Name, demo.Region.Name, demo.ToString() );
            s += String.Format( " Seg {0}: {1} - {2}<br>{3}<br><br>", d + 1, demo.Name, "???", demo.ToString() );
        }
        for( int m = 0; m < planInput.Media.Count; m++ ) {
            MediaComp mc = planInput.Media[ m ];
            MediaItem item = plan.GetMediaItem(mc.Guid);
            s += String.Format( " MediaComp {0}: {1} ", m + 1, mc.Guid );
            s += String.Format( "<br> days={0}-{1} ad_option={2} target_region={3} fuzz={4}<br>",
                mc.StartDate, mc.StartDate + mc.Span, mc.ad_option, mc.target_regions[0], mc.region_fuzz_factor );
            s += String.Format( " target_demo={0} fuzz={1}<br>", mc.target_demogrpahic[0], mc.demo_fuzz_factor );
            s += String.Format( " impressions={0}<br>", mc.Impressions );
            s += String.Format( " CPM = {0} ", item.Size );
            if( item.MediaType == MediaVehicle.MediaType.Magazine ) {
                s += String.Format(" Circulation = {0}", item.Size);
            }
            s += "<br><br>";
        }
        s += "<br>";
        return s;
    }
    #endregion
}
