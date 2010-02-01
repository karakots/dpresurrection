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

public partial class ReceivePlanCheck : System.Web.UI.Page
{
    private MediaPlan transferredPlan;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected void Page_Load( object sender, EventArgs e ) {
        RedirectUnknownUsers();

        InitializeVariables();

        this.transferredPlan = LoadTransferredPlan();

        bool planOK = CheckCampaignAndPlanName();

        if( planOK ) {
            AddPlanAndView();
        }

        InfoLabel.Text = String.Format( "Plan ID: {0}<br/>User: {1}<br/>Campaign: {2}<br/>Plan: {3}",
            Request[ "ReceivedPlanID" ], Request[ "ReceivedPlanUser" ], Request[ "CampaignName" ], Request[ "PlanName" ] );
    }

    /// <summary>
    /// Creates a new plan that is copied from the received plan and uses the new specs, then saves the plan and redirects to the appropriate viewing page.  
    /// </summary>
    /// <remarks>Finally the request is redirected to the HQ.aspx page if the plan has no results, or the Analysis page
    /// if there are results in the plan.</remarks>
    private void AddPlanAndView() {
        string newPlanVersion = "1";
        List<PlanStorage.PlanVersionInfo> curInfo = null;

        if( this.allPlanVersions.ContainsKey( CampaignName.Value ) ) {
            curInfo = this.allPlanVersions[ CampaignName.Value ];
            for( int i = 1; i < 1000000; i++ ) {                    // max limit is for sanity purposes only
                newPlanVersion = i.ToString();
                bool usedAlready = false;
                foreach( PlanStorage.PlanVersionInfo pinfo in curInfo ) {
                    if( pinfo.Version == newPlanVersion ) {
                        usedAlready = true;
                        break;
                    }
                }
                if( usedAlready == false ) {
                    break;
                }
            }
        }
        else {
            curInfo = new List<PlanStorage.PlanVersionInfo>();
            this.allPlanVersions.Add( CampaignName.Value, curInfo );
        }

        MediaPlan plan = this.transferredPlan;
        plan.Specs.CampaignName = CampaignName.Value;
        plan.PlanDescription = PlanName.Value;
        plan.PlanID = Guid.NewGuid();
        plan.PlanVersion = newPlanVersion;

        PlanStorage.PlanVersionInfo newPlanInfo = new PlanStorage.PlanVersionInfo( plan );
        curInfo.Add( newPlanInfo );
        Utils.SetAllPlanVersions( this, this.allPlanVersions );

        PlanStorage storage = new PlanStorage();
        storage.SaveMediaPlan( Utils.GetUser( this ), plan );
        Utils.SetCurrentMediaPlan( this, plan );

        if( this.currentMediaPlans == null ) {
            this.currentMediaPlans = new List<MediaPlan>();
        }
        this.currentMediaPlans.Add( plan );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );

        string targetPage = null;
        if( plan.PlanOverallRatingStars == -1 ){
            targetPage = "HQ.aspx";
        }
        else {
            targetPage = "Analysis.aspx";
        }

        Response.Redirect( targetPage );
    }

    /// <summary>
    /// Checks to see if the campaign and/or plan name are already in use in the current account.
    /// </summary>
    /// <returns></returns>
    private bool CheckCampaignAndPlanName() {
        if( this.allPlanVersions == null ) {
            ErrorExit( "Error: Value is null: allPlanVersions", false );
            return false;
        }

        if( this.allPlanVersions.ContainsKey( CampaignName.Value ) ) {
            List<PlanStorage.PlanVersionInfo> planVers = this.allPlanVersions[ CampaignName.Value ];
            foreach( PlanStorage.PlanVersionInfo pinfo in planVers ) {
                if( pinfo.Description == PlanName.Value ) {
                    ErrorExit( String.Format( "Error:  A plan named \"{0}\" already exists in the \"{1}\" campaign.<br><br> Use a different plan or campaign name.",
                        PlanName.Value, CampaignName.Value ), false );
                    return false;
                }
            }

            // determine if the existing campaign has compatible settings with the received plan's campaign - first get a plan from the campaign
            if( this.transferredPlan != null ) {
                MediaPlan campaignCheckPlan = null;
                foreach( MediaPlan chkPlan in this.currentMediaPlans ) {
                    if( chkPlan.CampaignName == CampaignName.Value ) {
                        campaignCheckPlan = chkPlan;
                        break;
                    }
                }

                // just in case the campaign doesn't have a current plan (can this ever happen???), try again to get a representative plan
                if( campaignCheckPlan == null ) {
                    if( planVers.Count > 0 ) {
                        string chkVersion = planVers[ 0 ].Version;
                        PlanStorage storage = new PlanStorage();
                        campaignCheckPlan = storage.LoadMediaPlan( Utils.GetUser( this ), CampaignName.Value, chkVersion );
                    }
                }

                // check the campaign campatibility
                if( campaignCheckPlan != null ) {
                    MediaCampaignSpecs c1 = campaignCheckPlan.Specs;
                    MediaCampaignSpecs c2 = this.transferredPlan.Specs;

                    bool same = true;

                    // compare the scalar values
                    if( c1.StartDate != c2.StartDate ) {
                        same = false;
                    }
                    if( c1.TargetBudget != c2.TargetBudget ) {
                        same = false;
                    }
                    if( c1.TimeInConsideration != c2.TimeInConsideration ) {
                        same = false;
                    }
                    if( c1.PurchaseCycleCode != c2.PurchaseCycleCode ) {
                        same = false;
                    }
                    if( c1.PreviousMediaSpend != c2.PreviousMediaSpend ) {
                        same = false;
                    }
                    if( c1.InitialShare != c2.InitialShare ) {
                        same = false;
                    }
                    if( c1.BusinessCategory != c2.BusinessCategory ) {
                        same = false;
                    }
                    if( c1.BusinessSubcategory != c2.BusinessSubcategory ) {
                        same = false;
                    }
                    if( c1.EndDate != c2.EndDate ) {
                        same = false;
                    }

                    // compare the values that are lists
                    if( c1.GeoRegionNames.Count != c2.GeoRegionNames.Count ) {
                        same = false;
                    }
                    else {
                        foreach( string geoName in c1.GeoRegionNames ) {
                            if( c2.GeoRegionNames.Contains( geoName ) == false ) {
                                same = false;
                                break;
                            }
                        }
                    }

                    if( c1.CampaignGoals.Count != c1.CampaignGoals.Count ) {
                        same = false;
                    }
                    else {
                        for( int i = 0; i < c1.CampaignGoals.Count; i++ ){
                            if( c1.CampaignGoals[ i ] != c2.CampaignGoals[ i ] ){
                                same = false;
                                break;
                            }                           
                            if( c1.GoalWeights[ i ] != c2.GoalWeights[ i ] ){
                                same = false;
                                break;
                            }
                        }
                    }

                    if( c1.Demographics.Count != c2.Demographics.Count ) {
                        same = false;
                        for( int i = 0; i < c1.Demographics.Count; i++ ) {
                            DemographicSettings d1 = c1.Demographics[ i ];
                            DemographicSettings d2 = c2.Demographics[ i ];

                            if( d1.DemographicName != d2.DemographicName ) {
                                same = false;
                                break;
                            }

                            if( d1.ValueCount != d2.ValueCount ) {
                                same = false;
                                break;
                            }

                            if( d1.SummaryDescription() != d2.SummaryDescription() ) {
                                same = false;
                                break;
                            }
                        }
                    }

                    if( same == false ) {
                        ErrorExit( String.Format( "Error:  The campaign settings for the received plan are different from those in the \"{0}\" campaign.<br><br>" +
                        "A received plan can only be added to an existing campaign if all of the campaign settings are the same.<br><br>" +
                        "Choose another name for the campaign to proceed.",
                           PlanName.Value, CampaignName.Value ), false );
                        return false;
                    }
                }
            }

            ErrorExit( String.Format( "OK to add to existing campaign?<br><br>Do you want to add this plan to the \"{0}\" campaign?", CampaignName.Value ), true );
            return false;
        }

        // looks OK
        OKButton.Visible = false;
        CancelButton.Text = "Cancel";
        CancelButton.PostBackUrl = "Campaigns.aspx";
        ErrorLabel.CssClass = "NoErrorLabel";

        return true;
    }

    /// <summary>
    /// Configures the display to show an error or warning message.
    /// </summary>
    /// <param name="errorText"></param>
    /// <param name="warningOnly"></param>
    private void ErrorExit( string errorText, bool warningOnly ) {
        ErrorLabel.Text = errorText;
        if( warningOnly == false ) {
            OKButton.Visible = false;
            CancelButton.Text = "OK";
        }
        else {
            OKButton.Visible = true;
            CancelButton.Text = "Cancel";
            ErrorLabel.CssClass = "NoErrorLabel";
        }
    }

    protected void GoButton_Click( object sender, EventArgs e ) {
        AddPlanAndView();
    }


    /// <summary>
    /// Returns the media plan specified by the commmand-line arguments (user email and plan ID)
    /// </summary>
    /// <returns></returns>
    private MediaPlan LoadTransferredPlan() {
        string receivedPlanUser = ReceivedPlanUser.Value;
        string receivedPlanID = ReceivedPlanID.Value;

        if( receivedPlanUser == null || receivedPlanID == null ) {
            return null;
        }

        Guid guid = new Guid();
        try {
            guid = new Guid( receivedPlanID );
        }
        catch {
            return null;
        }

        PlanStorage storage = new PlanStorage();
        MediaPlan plan = storage.LoadMediaPlan( receivedPlanUser, guid );
        return plan;
    }

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        // load the form-field variables for the plan-receiving process
        ReceivedPlanID.Value = Request[ "ReceivedPlanID" ];
        ReceivedPlanUser.Value = Request[ "ReceivedPlanUser" ];
        CampaignName.Value = Request[ "CampaignName" ];
        PlanName.Value = Request[ "PlanName" ];

        // get the usual variables
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
        List<UserMedia.CurrentPlanVersion> curVers = userMedia.CurrentPlanVersions;

        // we may need to get the allPlanVersions data for the user's current plans and campaigns
        if( this.allPlanVersions == null ) {
            PlanStorage storage = new PlanStorage();
            bool dum = false;
            this.currentMediaPlans = storage.LoadCurrentPlans( Utils.GetUser( this ), out this.allPlanVersions, ref curVers, out dum );
            Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
            Utils.SetAllPlanVersions( this, this.allPlanVersions );
        }
    }

    private void RedirectUnknownUsers() {
        if( Utils.UserIsDemo( this ) ) {
            Response.Redirect( "~/Home.aspx" );
        }
    }
}
