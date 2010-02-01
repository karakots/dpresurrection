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
using BusinessLogic;

public partial class Scoreboard : System.Web.UI.Page
{
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected string campaignOwner;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        this.campaignOwner = Utils.GetUser( this );
        if( Request[ "u" ] != null ) {
            List<UserMedia.CurrentPlanVersion> ownerCurVersions = new List<UserMedia.CurrentPlanVersion>();     // we don't know this
            bool dum = true;

            this.campaignOwner = Request[ "u" ].Replace( "--", "@" );
            PlanStorage storage = new PlanStorage();

            // get the campaign owner's info as this page's current info!
            this.currentMediaPlans = storage.LoadCurrentPlans( this.campaignOwner, out this.allPlanVersions, ref ownerCurVersions, out dum );
        }

        // respond to a request to change/set current plan
        if( Request[ "p" ] != null ) {
            Guid planID = new Guid( Request[ "p" ] );
            this.currentMediaPlan = Utils.PlanForID( planID, this.currentMediaPlans, this.allPlanVersions, this.campaignOwner );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }

        List<PlanResults> plansInCampaign = GetAllPlansInCampaign();

        ShowCampaignPlansSummary( plansInCampaign );
    }

    /// <summary>
    /// Displays the summary data
    /// </summary>
    /// <param name="results"></param>
    private void ShowCampaignPlansSummary( List<PlanResults> results ) {
        // sort the results
        double[] sortKey = new double[ results.Count ];
        PlanResults[] sortedRes = new PlanResults[ results.Count ];
        for( int i = 0; i < results.Count; i++ ) {
            sortKey[ i ] = results[ i ].TotalSales;
            sortedRes[ i ] = results[ i ];
        }
        Array.Sort( sortKey, sortedRes );
        Array.Reverse( sortedRes );

        string s = GetTableHeader();
        for( int i = 0; i < sortedRes.Length; i++ ) {
            PlanResults res = sortedRes[ i ];

            
            string planOwner = "&nbsp;";
            if( res.PlanOwner != null ){
                if( res.PlanOwner != Utils.GetUser( this ) ) {
                    planOwner = res.PlanOwner;
                }
                else {
                    planOwner = "you";
                }
            }

            if( planOwner != this.currentMediaPlan.Specs.CompetitionOwner ) {
                s += "<tr>";
            }
            else {
                s += "<tr style=\"background-color:#DDD;\" >";
            }

            s += String.Format( "<td>{0}</td>", planOwner );
            s += String.Format( "<td>{0}</td>", res.TotalSales );
            s += String.Format( "<td>{0}</td>", res.PlanName );
            s += String.Format( "<td>{0}</td>", res.NumStars );
            s += String.Format( "<td>{0} {1}</td>", res.Time.ToShortDateString(), res.Time.ToShortTimeString() );
            if( i < 3 ) {
                s += String.Format( "<td>{0}<img src=\"images/Trophy{0}.gif\" ></td>", i + 1 );
            }
            else {
                s += String.Format( "<td>{0}</td>", i + 1 );
            }
            s += "</tr>";
        }
        s += "</table>";

        ResultsData.InnerHtml = s;
    }

    private string GetTableHeader() {
        string s = "<table cellpadding=3 cellspacing=0 ID=\"ScoreboardTable\" ><tr  style=\"font-style:italic;background-color:#DFF;\" >" +
            "<td>User</td>" +
            "<td>Total Sales</td>" +
            "<td>Plan</td>" +
            "<td>Stars</td>" +
            "<td>Time</td>" +
            "<td>Rank</td></tr>";
        return s;
    }

    /// <summary>
    /// Create the list of results summary objects that correspond to the media plans in the current campaign.
    /// </summary>
    /// <returns></returns>
    private List<PlanResults> GetAllPlansInCampaign() {
        string campaignName = this.currentMediaPlan.CampaignName;

        if( allPlanVersions[ campaignName ] == null ) {
            return null;
        }

        PlanStorage storage = new PlanStorage();
        List<PlanResults> results = new List<PlanResults>();
        List<PlanStorage.PlanVersionInfo> planVersions = allPlanVersions[ campaignName ];
        foreach( PlanStorage.PlanVersionInfo vInfo in planVersions ) {
            MediaPlan plan = storage.LoadMediaPlan( campaignOwner, campaignName, vInfo.Version );
            if( plan.Competitor == null ) {
                continue;
            }
            CompetitionNameLabel.InnerHtml = plan.Specs.CompetitionName + "&nbsp;&nbsp;&nbsp;Owner: " + plan.Specs.CompetitionOwner;
            if( plan.Specs.CompetitionOwner == Utils.GetUser( this ) ) {
                CompetitionNameLabel.InnerHtml += " (you)";
            }
            CompetitionNameLabel.InnerHtml += "&nbsp;&nbsp;&nbsp;Started: " + plan.Specs.CompetitionDate.ToShortDateString();
            PlanResults res = new PlanResults( plan.PlanID );
            res.PlanName = plan.PlanName;
            res.NumStars = (int)Math.Round( plan.PlanOverallRatingStars );
            res.PlanOwner = plan.Competitor;
            res.Time = plan.ModificationDate;

            for( int i = 0; i < plan.Specs.CampaignGoals.Count; i++ ) {
                if( plan.Specs.CampaignGoals[ i ] == MediaCampaignSpecs.CampaignGoal.Persuasion ) {
                    res.Persuasion = 0;
                    if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                        res.Persuasion = (int)Math.Round( plan.PlanOverallGoalScores[ i ] );
                    }
                }
                if( plan.Specs.CampaignGoals[ i ] == MediaCampaignSpecs.CampaignGoal.ReachAndAwareness ) {
                    res.Awareness = 0;
                    if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                        res.Awareness = (int)Math.Round( plan.PlanOverallGoalScores[ i ] );
                    }
                }
                if( plan.Specs.CampaignGoals[ i ] == MediaCampaignSpecs.CampaignGoal.Recency ) {
                    res.Recency = 0;
                    if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                        res.Recency = (int)Math.Round( plan.PlanOverallGoalScores[ i ] );
                    }
                }
                if( plan.Specs.CampaignGoals[ i ] == MediaCampaignSpecs.CampaignGoal.DemographicTargeting ) {
                    res.DemoTargeting = 0;
                    if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                        res.DemoTargeting = (int)Math.Round( plan.PlanOverallGoalScores[ i ] );
                    }
                }
                if( plan.Specs.CampaignGoals[ i ] == MediaCampaignSpecs.CampaignGoal.GeoTargeting ) {
                    res.GeoTargeting = 0;
                    if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                        res.GeoTargeting = (int)Math.Round( plan.PlanOverallGoalScores[ i ] );
                    }
                }

                // compute the results metric(s) used for the overall scoring
                res.TotalSales = 0;
                if( plan.Results == null ) {
                    continue;
                }
                for( int m = 0; m < plan.Results.metrics.Count; m++ ){
                    if( plan.Results.metrics[ m ].Type == "TotalActions" ) {
                        res.TotalSales =+ GetTotal( plan, m );
                    }
                }
            }

            results.Add( res );
        }
        return results;
    }

    private int GetTotal( MediaPlan plan, int metricNum ) {
        double tot = 0;
        foreach( double resVal in plan.Results.metrics[ metricNum ].values ) {
            tot += resVal;
        }
        int itot = (int)Math.Round( tot );
        return itot;
    }

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
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
    }

    /// <summary>
    /// Redirects a user who ins't logged-in back to the login page
    /// </summary>
    private void RedirectUnknownUsers() {
        if( this.User == null || this.User.Identity == null || this.User.Identity.Name == null || this.User.Identity.Name == "" ) {
            Response.Redirect( "Login.aspx" );
        }
    }

    private class PlanResults
    {
        public string PlanOwner;
        public string PlanName;
        public DateTime Time;

        public int TotalSales;           

        public int NumStars;
        public int Awareness;
        public int Persuasion;
        public int Recency;
        public int DemoTargeting;
        public int GeoTargeting;

        public Guid PlanID;

        public PlanResults( Guid planID ) {
            this.PlanID = planID;
        }
    }
}
