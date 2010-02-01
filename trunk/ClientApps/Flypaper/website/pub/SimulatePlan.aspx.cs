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
using MediaLibrary;
using HouseholdLibrary;
using SimInterface;
using DemographicLibrary;
using BusinessLogic;

public partial class SimulatePlan2 : System.Web.UI.Page
{
    protected const string adFooter = "";
    //protected const string adFooter = "<img src=\"images/180x150ad.gif\" style=\"padding-left:10px;\">";

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    //protected List<MediaPlan> plansBeingEdited;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;
    protected UserMedia userMedia;


    protected string DisplayData = "";
    //private List<SimUtils.SimStatus> simStatus;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        // display status for the initial visit to this page
        int numToRun = 0;
        int numRunning = 0;
        //if( this.plansBeingEdited != null ) {
        //    numToRun = this.plansBeingEdited.Count;
        //}
        if( this.runningPlans != null ) {
            numRunning = this.runningPlans.Count;
        }
        DisplayData = String.Format( "STARTING sims: {0}<br><br>", numToRun );
        SetBodyText( DisplayData );

        RunAllEditedPlans();     // RUN THE SIMS!

        // engineering-mode-only stuff (dump sim details)
        if( this.engineeringMode == true ) {
            string s = "Engineering-mode data:<br>";
            //if( this.plansBeingEdited != null ) {
            //    for( int d = 0; d < this.plansBeingEdited.Count; d++ ) {
            //        MediaPlan planToRun = this.plansBeingEdited[ d ];
            //        SimInterface.SimInput testInput = SimUtils.ConvertMediaPlanToSimInput( planToRun, this.userMedia );
            //        s += DebugDumpStringFor( d, planToRun, testInput );
            //    }
            //}
            DebugExtraText.Text = s;
        }

        StoryDisplayer sDisp = new StoryDisplayer( StoryGenerator.GetStoryGenerator( this ), "Running simulation...", this.engineeringMode );
        sDisp.PopulateDisplayDiv( StoryDiv, StoryBodyDiv, StoryFooterDiv, 0 );

        if( this.currentMediaPlan != null ) {
            CampaignSummaryDiv.InnerHtml = this.currentMediaPlan.Specs.GetCampaignSummary( adFooter );
        }

        // this is just a pass-thru page -- change to the progress-monitor page as soon as the sims have loaded and started running
        Response.Redirect( "CheckProgress.aspx" );
    }

    ///// <summary>
    ///// Checks the status of all known running sims and refreshes the status display.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void Timer_Tick( object sender, EventArgs e ) {
    //    UpdatePlanStatus();
    //}

    /// <summary>
    /// Runs any sims in the plansBeingEdited list, trransfers their IDs to the runningPlans list, and then clears the plansBeingEdited list.
    /// </summary>
    private void RunAllEditedPlans(){

        //if( this.plansBeingEdited.Count > 0 ) {
        //    foreach( MediaPlan planToStart in this.plansBeingEdited ) {

        //        // change the status of plans with queued/running sims to RUNNING
        //        planToStart.PlanStatus = MediaPlan.MediaPlanStatus.RUNNING;

        //        // save plan to disk before running sim
        //        PlanStorage storage = new PlanStorage();
        //        storage.SaveMediaPlan( Utils.GetUser( this ), planToStart );

        //        // queue up the sim -- this updates the RunningMediaPlanIDs list of the session
        //        SimUtils.RunSimulation( this, planToStart, this.userMedia );

        //        StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.SimulationStartedElement( planToStart.CampaignName, planToStart.PlanName ) );
        //    }
            // update the local running-sims list
            this.runningPlans = Utils.RunningMediaPlanIDs( this );

            // clear the edited-plans list
            //this.plansBeingEdited = new List<MediaPlan>();

            // TBD
            // no longer needed
            // ssn
            //Utils.SetPlansBeingEdited( this, this.plansBeingEdited );
        //}
    }

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
     //   this.plansBeingEdited = Utils.PlansBeingEdited( this );
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

    //---------------- debug stuff ---------------
    private string DebugDumpStringFor( int planIndx, MediaPlan plan, SimInterface.SimInput planInput ) {
        string s = "";

        s += String.Format( "<b>Sim {0}: {1}</b> segments={2} media={3}<br>", planIndx + 1, plan.PlanDescription, plan.DemographicCount, plan.GetTypes().Count );
        for( int d = 0; d < planInput.Demographics.Count; d++ ) {
            Demographic demo = planInput.Demographics[ d ];
//            s += String.Format( " Seg {0}: {1} - {2}<br>", d + 1, demo.Name, demo.Region.Name );
            s += String.Format( " Seg {0}: {1} - {2}<br>", d + 1, demo.Name, "??" );
        }
        for( int m = 0; m < planInput.Media.Count; m++ ) {
            MediaComp mc = planInput.Media[ m ];
            s += String.Format( " MediaComp {0}: {1} days={2}-{3} ad_option={4}<br>", m + 1, mc.Guid, mc.StartDate, mc.StartDate + mc.Span, mc.ad_option );
        }
        s += "<br>";
        return s;
    }


    /// <summary>
    /// Checks the status of all known running sims and refreshes the status display.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProceedButton_Click( object sender, EventArgs e ) {
        Response.Redirect( "SimulationResults.aspx" );
    }
}
