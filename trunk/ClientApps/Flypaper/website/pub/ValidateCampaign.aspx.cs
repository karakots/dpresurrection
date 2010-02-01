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

using BusinessLogic;
using WebLibrary;

/// <summary>
/// This is the pass-thru error-check page for media campaigns.  If the current media plan has a valid campaign, the page specified by the "next"
/// argument is loaded.  Otherwise the error details are shown with an OK button to return to the campaign edit page.
/// </summary>
public partial class ValidateCampaign : System.Web.UI.Page
{
    private const string blankNameErr = "The campaign has a blank name.  A media plan must have a name.";
    private const string dupNameErr = "The campaign name \"{0}\" is already in use.  Campaign names must be unique.";
    private const string zeroBudgetErr = "The campaign \"{0}\" has zero target budget specified.  This campaign will generate no results.";
    private const string noSegmentsErr = "The campaign \"{0}\" has no customer segments specified.  You must specify at least one region.";
    private const string IllegalCharErr = "Illegal character(s) in name. The campaign name cannot include any of these: / \\ : * ? \" < > |";
    
    
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected List<Guid> runningPlans;
    protected List<MediaPlan> plansBeingEdited;
    protected bool engineeringMode = false;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;

    /// <summary>
    /// The errors, if any, found in validation
    /// </summary>
    protected List<string> errorDetailsList;

    protected void Page_Load( object sender, EventArgs e ) {
        //Utils.LoadValuesFromSession( this, out this.currentMediaPlan, out this.allMediaPlans, out this.runningPlans );
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.allPlanVersions = Utils.AllPlanVersions( this );

        engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );

        bool creatingNew = (Request[ "c" ] != null && Request[ "c" ].ToLower() == "true");

        if( CheckCampaignValidity( this.currentMediaPlan.Specs, creatingNew ) == true ) {

            string target = Request[ "next" ];
            if( target != "SaveOnly" ) {
                // get the name for the plan
                if( Request[ "newPlanName" ] != null && Request[ "newPlanName" ].Trim() != "" ) {
                    this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
                }
            }
            this.currentMediaPlan.PlanValid = true;

            GoToNext( target );
        }
        else {
            if( this.errorDetailsList.Count > 0 ) {
                ErrorDetails.Text = "";
                for( int i = 0; i < errorDetailsList.Count; i++ ) {
                    ErrorDetails.Text += this.errorDetailsList[ i ] + "<br><br>";
                }
            }
        }
    }

    /// <summary>
    /// Checks the validity of the given campaign spec.  Fills errorDetaisList with details of any errors found.
    /// </summary>
    /// <param name="campaignSpecs"></param>
    /// <returns></returns>
    private bool CheckCampaignValidity( MediaCampaignSpecs campaignSpecs, bool creatingNew ) {
        this.errorDetailsList = new List<string>();
        bool valid = true;

        // be sure a campaign has a name
        if( campaignSpecs.CampaignName == null || campaignSpecs.CampaignName.Trim() == "" ) {
            string msg = String.Format( blankNameErr );
            errorDetailsList.Add( msg );
            valid = false;
        }
        else {
            char[] badChars = new char[ 9 ];
            badChars[ 0 ] = '/';
            badChars[ 1 ] = '\\';
            badChars[ 2 ] = ':';
            badChars[ 3 ] = '*';
            badChars[ 4 ] = '?';
            badChars[ 5 ] = '\"';
            badChars[ 6 ] = '<';
            badChars[ 7 ] = '>';
            badChars[ 8 ] = '|';

            if( campaignSpecs.CampaignName.IndexOfAny( badChars ) != -1 ) {
                // the name contains illegal characters
                errorDetailsList.Add( IllegalCharErr );
                valid = false;
            }
            else if( creatingNew ) {
                // be sure the name is unique if a new plan is being created
                //bool unique = true;

                foreach( MediaPlan allPlan in this.currentMediaPlans ) {
                    if( campaignSpecs != allPlan.Specs && campaignSpecs.CampaignName == allPlan.Specs.CampaignName ) {
                        string msg = String.Format( dupNameErr, campaignSpecs.CampaignName );
                        errorDetailsList.Add( msg );
                        valid = false;
                    }
                }
            }
        }

        // be sure a campaign has at least one segment
        if( campaignSpecs.SegmentCount ==  0 ) {
            if( this.currentMediaPlan.Specs.Demographics.Count == 0 ) {
                DemographicSettings allSettings = new DemographicSettings();
                allSettings.DemographicName = "Everybody";
                this.currentMediaPlan.Specs.Demographics.Add( allSettings );
            } 
        }

        // be sure a campaign has a nonzero budget
        if( campaignSpecs.TargetBudget <= 0 ) {
            errorDetailsList.Add( zeroBudgetErr );
            valid = false;
        }

        return valid;
    }

    /// <summary>
    /// Autigenerates a plan
    /// </summary>
    private MediaPlan CreateAutogeneratedPlan( out string timingSpecs ) {
        DateTime t0 = DateTime.Now;

        AllocationService allocationService = new AllocationService();
        MediaPlan baseMediaPlan = allocationService.CreateNewMediaPlan( this.currentMediaPlan.PlanSpecs );

        DateTime t1= DateTime.Now;

        int dt = (int)Math.Round( (t1 - t0).TotalMilliseconds );

        timingSpecs = String.Format( "t={0:f3}", dt / 1000.0 );

        return baseMediaPlan;
    }

    /// <summary>
    /// Returns the user to the page that requested the valication (validation failed)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OkButton_Click( object sender, EventArgs e ) {
        Response.Redirect( "Campaign.aspx" );
    }

    /// <summary>
    /// Transfers control to the next page (validation suceeded)
    /// </summary>
    private void GoToNext( string target ) {

        if( target == "SaveOnly" ) {
            Response.Redirect( "Campaigns.aspx" );
        }

        // take the active plan out of the current plans list, if it is in there
        if( this.currentMediaPlans.Contains( this.currentMediaPlan ) ) {
            this.currentMediaPlans.Remove( this.currentMediaPlan );
        }

        // see if we need to proceed to add (only) the base plan to the edited-plans list
        if( target == "EditPlan" ) {

            // replace the current plan
            string timingSpecs = "";
            MediaPlan autoGenPlan = CreateAutogeneratedPlan( out timingSpecs );

            // transfer the non-campaign data since we're going to replace the current plan with the autogen plan
            autoGenPlan.PlanDescription = this.currentMediaPlan.PlanDescription;
            autoGenPlan.PlanVersion = this.currentMediaPlan.PlanVersion;
            autoGenPlan.PlanID = this.currentMediaPlan.PlanID;
            autoGenPlan.TargetBudget = this.currentMediaPlan.Specs.TargetBudget;

            this.currentMediaPlan = autoGenPlan;

            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            DataLogger.Log( "GEN-C", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription, timingSpecs );
        }
        else {
            DataLogger.Log( "CREATE-C", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription );
        }

        // add active plan to the current list, so we are sure it is there
        this.currentMediaPlans.Add( this.currentMediaPlan );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );

        // be sure the versions list has the plan
        if( this.allPlanVersions.ContainsKey( this.currentMediaPlan.CampaignName ) == false ) {
            this.allPlanVersions.Add( this.currentMediaPlan.CampaignName, new List<PlanStorage.PlanVersionInfo>() );
        }
        List<PlanStorage.PlanVersionInfo> planVers = this.allPlanVersions[ this.currentMediaPlan.CampaignName ];
        //PlanStorage.PlanVersionInfo vinfo = null;
        //foreach( PlanStorage.PlanVersionInfo info in planVers ) {
        //    if( info.Version == this.currentMediaPlan.PlanVersion ) {
        //        vinfo = info;
        //    }
        //}
        //if( vinfo != null ) {
        //    planVers.Remove( vinfo );
        //}
        planVers.Add( new PlanStorage.PlanVersionInfo( this.currentMediaPlan ) );

        // save the plan to disk
        PlanStorage storage = new PlanStorage();
        storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );

        StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.CreateCampaignElement( this.currentMediaPlan.CampaignName ) );
        StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.CreatePlanElement( this.currentMediaPlan.PlanDescription ) );
        Response.Redirect( "HQ.aspx" );
    }   
}
