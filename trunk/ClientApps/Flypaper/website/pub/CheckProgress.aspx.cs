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

public partial class CheckProgress : System.Web.UI.Page
{
    protected const string adFooter = "";

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
   // protected List<MediaPlan> plansBeingEdited;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string DisplayData = "";
    private List<SimUtils.SimStatus> simStatus;

    protected const string progressImageTag = "<img src=\"images/1animated7.gif\" width=\"133\" height=\"100\" ><br><br>" +
        "<asp:Button ID=\"AbortButton\" runat=\"server\" OnClick=\"AbortButton_Click\" Text=\"Abort\" Visible=\"true\" />";

    protected void Page_Load( object sender, EventArgs e ) {

        InitializeVariables();

        ProgressImage.InnerHtml = progressImageTag;

        if( IsPostBack == false ) {
            // display status for the initial visit to this page
            int numToRun = 0;
            int numRunning = 0;
            //if( this.plansBeingEdited != null ) {
            //    numToRun = this.plansBeingEdited.Count;
            //}
            if( this.runningPlans != null ) {
                numRunning = this.runningPlans.Count;
            }
            DisplayData = String.Format( "RUNNING sims: {0}<br><br>", numToRun );

            SetBodyText( DisplayData );

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
        }
        else {
            // this is a postback from the Javascript interval timer
            if( Request[ "__EVENTTARGET" ] == "UpdateProgressPanel" ) {
                Timer_Tick( this.UpdateProgressPanel, null );
            }
        }

        StoryDisplayer sDisp = new StoryDisplayer( StoryGenerator.GetStoryGenerator( this ), "Running simulation...", this.engineeringMode );
        sDisp.PopulateDisplayDiv( this.StoryDiv, this.StoryBodyDiv, this.StoryFooterDiv, 0 );

        if( this.currentMediaPlan != null ) {
            CampaignSummaryDiv.InnerHtml = this.currentMediaPlan.Specs.GetCampaignSummary( adFooter );
        }
    }

    /// <summary>
    /// Checks the status of all known running sims and refreshes the status display.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Timer_Tick( object sender, EventArgs e ) {
        // UpdatePlanStatus();
    }

    protected void AbortButton_Click( object sender, EventArgs e ) {
        List<Guid> plansStillRunning = new List<Guid>();
        Utils.SetRunningMediaPlanIDs( this, plansStillRunning );
        Response.Redirect( "HQ.aspx" );
    }

    /// <summary>
    /// Checks the status of all known running sims and refreshes the status display. 
    /// </summary>
    //private void UpdatePlanStatus() {
    //    bool readyToProceed = true;

    //    if( this.runningPlans != null && this.runningPlans.Count > 0 ) {

    //        List<MediaPlan> all_plans = new List<MediaPlan>();

    //        all_plans.AddRange(currentMediaPlans);
    //        //all_plans.AddRange(plansBeingEdited);

    //        // get the status of any running sims
    //        List<Guid> plansStillRunning = new List<Guid>();
    //            //SimUtils.UpdateRunningSimStatus( this, this.runningPlans, all_plans, out this.simStatus );

    //        //update the master list
    //        //this.runningPlans = plansStillRunning;
    //        //Utils.SetRunningMediaPlanIDs( this, this.runningPlans );

    //        if( plansStillRunning.Count > 0 ) {

    //            readyToProceed = false;
    //            DisplayData = "Sims running: " + plansStillRunning.Count;
    //            for( int i = 0; i < this.simStatus.Count; i++ ){
    //                SimUtils.SimStatus ss = this.simStatus[ i ];

    //                DisplayData += String.Format( "<br>  Sim {0}: {1:f0} %", i + 1, ss.ProgressPercent );
    //            }
    //        }
    //        else {
    //            // the last running sim is done
    //            readyToProceed = true;
    //            DisplayData = "All sims done";

    //            // change the status of plans with completed sims to SIMULATED and save them to disk
    //            foreach( Guid doneSimPlanID in this.runningPlans ) {
    //                MediaPlan donePlan = Utils.PlanForID( doneSimPlanID, this.currentMediaPlans );
    //                donePlan.PlanStatus = MediaPlan.MediaPlanStatus.SIMULATED;

    //                BusinessLogic.Scoring scoreGen = new BusinessLogic.Scoring( donePlan );
    //                donePlan.PlanOverallRatingStars = 3;

    //                // see if the story should have the improvements or suggestions
    //                if( donePlan.PlanOverallRatingStars < Scoring.MinCongratulationsStars ) {
    //                    // add improvements
    //                    string issuesSummary = "";
    //                }
    //                else {
        
    //                }

    //                PlanStorage storage = new PlanStorage();
    //                storage.SaveMediaPlan( Utils.GetUser( this ), donePlan );
    //            }
    //        }
    //    }
    //    else {
    //        // there are no plans in the runing-plans list!
    //        DisplayData = "<font color='Red'>Error: The Session for this page does not contain any running media plans!</font>";
    //    }

    //    SetBodyText( DisplayData );

    //    // go to the results page if the sims are all done
    //    if( readyToProceed ) {
    //        AllDone();
    //    }
    //}

    /// <summary>
    ///  Proceed to to the results page (in engineering mode, display the "proceed" button)
    /// </summary>
    protected void AllDone() {

        ProgressImage.InnerHtml = "Simulation complete.  Please wait for results dusplay...";

        if( this.engineeringMode == false ) {
           Response.Redirect( "SimulationResults.aspx" );
        }
        else {
            // display the manual-proceed button
            DebugProceedButton.Visible = true;
        }
    }

    /// <summary>
    /// Refreshes the contents of the UpdatePanel
    /// </summary>
    /// <param name="txt"></param>
    private void SetBodyText( string txt ) {
        this.BodyInfo.Text = txt + "<br><br>Checked at: " + DateTime.Now.ToString( "h:mm:ss" );
    }

    protected void NextButton_Click( object sender, EventArgs e ) {
        Response.Redirect( "~/SimulatePlan.aspx" );
    }


    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );
      //  this.plansBeingEdited = Utils.PlansBeingEdited( this );
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

        s += String.Format( "<b>Sim {0}: {1}</b> segments={2} media={3}<br>", planIndx + 1, plan.PlanName, plan.DemographicCount, plan.GetTypes().Count );
        for( int d = 0; d < planInput.Demographics.Count; d++ ) {
            Demographic demo = planInput.Demographics[ d ];
 //           s += String.Format( " Seg {0}: {1} - {2}<br>", d + 1, demo.Name, demo.Region.Name );
            s += String.Format( " Seg {0}: {1} - {2}<br>", d + 1, demo.Name, "???" );
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
