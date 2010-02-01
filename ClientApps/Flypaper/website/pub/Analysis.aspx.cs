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
using MediaLibrary;

public partial class Analysis : System.Web.UI.Page
{
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected bool summaryExpanded = true;
    protected bool detailsExpanded = false;
    protected bool suggestionsExpanded = false;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        //
        // To turn on only for staging site
        //
        //if( System.Configuration.ConfigurationManager.AppSettings["Adplanit.DevelopmentMode"] == "TRUE" )
        //{
        //    AgentLink.Visible = true;
        //}
         AgentLink.Visible = true;
        
        

        // respond to a request to change/set current plan
        if( Request[ "p" ] != null ) {
            this.currentMediaPlan = Utils.PlanForID( Request[ "p" ], this.currentMediaPlans );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }

        if (this.currentMediaPlan.Results == null)
        {
            Response.Redirect(@"Simulation\Simulate.aspx");
        }

        HandleSuggestionAcceptance();

        SetExpandedSections();

        SetNameChangeLink();

        SetupEnterCompetitionLink();

        HandleModifyRequest();

        DisplayPlanInfo();

        DisplayResultsSummary();

        if( this.IsPostBack == false ) {
            DataLogger.LogPageVisit( "ANALYSIS", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription );
        }

        if( this.User.IsInRole( "visitor" ) )
        {
            GetSuggestionsButton.Enabled = false;
            GetSuggestionsButton.ToolTip = "Sign in to AdPlanit to enable features";
            GetSuggestionsButton.BackColor = System.Drawing.Color.Gray;
        }
    }

    protected void GetSuggedtionsNow( object sender, EventArgs e )
    {

        double suggBudget = this.currentMediaPlan.TargetBudget;
        if( this.SetBudget.Checked == true )
        {
            try
            {
                suggBudget = double.Parse( NewBudgetTextBox.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }
        SuggestionMaker suggestionMaker = new SuggestionMaker( this.currentMediaPlan, this.CreateNew.Checked, suggBudget, ImprovMetric.SelectedValue, this.allPlanVersions );
        SuggestionCell.InnerHtml = suggestionMaker.GetSuggestionHTML();
        MediaPlan suggestedPlan = suggestionMaker.GetSuggestedPlan();
        Session["SuggestedPlan"] = suggestedPlan;
        if( CreateNew.Checked == false )
        {
            Session["SuggestedAddItems"] = suggestionMaker.GetSuggestedAddItems();
            Session["SuggestedRemoveItems"] = suggestionMaker.GetSuggestedRemoveItems();
        }

    }

    private void SetExpandedSections() {
        if( Request[ "expandSummary" ] == "True" ) {
            this.summaryExpanded = true;
        }
        else if( Request[ "expandSummary" ] == "False" ) {
            this.summaryExpanded = false;
        }

        if( Request[ "expandDetails" ] == "True" ) {
            this.detailsExpanded = true;
        }
        else if( Request[ "expandDetails" ] == "False" ) {
            this.detailsExpanded = false;
        }

        if( Request[ "expandSuggestions" ] == "True" ) {
            this.suggestionsExpanded = true;
        }
        else if( Request[ "expandSuggestions" ] == "False" ) {
            this.suggestionsExpanded = false;
        }
    }

    protected string SummaryExpandImage() {
        string condSum_JS = String.Format( "document.getElementById( \"expandSummary\" ).value = \"{0}\"; __doPostBack( \"exp\", 0 ); return false;", !this.summaryExpanded );
        string expLink = null; 
        if( this.summaryExpanded == true ) {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Condense.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );   
        }
        else {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Expand.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );   
        }
        return expLink;
    }

    protected string DetailsExpandImage() {
        string condSum_JS = String.Format( "document.getElementById( \"expandDetails\" ).value = \"{0}\"; __doPostBack( \"exp\", 0 ); return false;", !this.detailsExpanded );
        string expLink = null;
        if( this.detailsExpanded == true ) {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Condense.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );
        }
        else {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Expand.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );
        }
        return expLink;
    }

    protected string SuggestionsExpandImage() {
        string condSum_JS = String.Format( "document.getElementById( \"expandSuggestions\" ).value = \"{0}\"; __doPostBack( \"exp\", 0 ); return false;", !this.suggestionsExpanded );
        string expLink = null;
        if( this.suggestionsExpanded == true ) {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Condense.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );
        }
        else {
            expLink = String.Format( " <a href=\"#\" onclick='{0}'><img src=\"images/Expand.gif\" width=\"20\" height=\"20\" border=\"0\" /></a>", condSum_JS );
        }
        return expLink;
    }

    protected void DisplayResultsSummary() {
        if( this.currentMediaPlan.Results == null ) {

            Label notSimulatedLabel = new Label();
            notSimulatedLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;This plan has not been simulated yet.&nbsp;&nbsp;";
            notSimulatedLabel.Style.Add( "position", "relative" );
            notSimulatedLabel.Style.Add( "top", "-4px" );

            ImageButton simButton = new ImageButton();
            simButton.ImageUrl = "images/Button-TestPlan.gif";
            simButton.ID = "TestPlanButton";
            simButton.PostBackUrl = "ValidatePlans.aspx?this=HQ&next=Simulation/Simulate";

            ResultsSummaryCell.Controls.Clear();

            ResultsSummaryCell.Controls.Add( notSimulatedLabel );
            ResultsSummaryCell.Controls.Add( simButton );
            SelectionCell.Style.Add( "display", "none" );
            ResultsChartCell.Style.Add( "display", "none" );
            //SelectionCell2.Style.Add( "display", "none" );
            //SelectionCell3.Style.Add( "display", "none" );
            ResultsGraphCell.Style.Add( "display", "none" );
            DetailsHeader.Style.Add( "display", "none" );
            SummaryHeader.Style.Add( "display", "none" );
            ComparisonPlans.Style.Add( "display", "none" );
            //CompareLabel.Style.Add( "display", "none" );
            GraphSelectionRow.Style.Add( "display", "none" );
            GraphSelectionRow2.Style.Add( "display", "none" );
        }
        else {
            ResultsSummaryCell.Style.Add( "display", "none" );
            DetailsHeader.Style.Add( "display", "block" );
            SummaryHeader.Style.Add( "display", "block" );

            if( summaryExpanded == false ) {
                SelectionCell.Style.Add( "display", "none" );
                ResultsChartCell.Style.Add( "display", "none" );
            }
            else {
                SelectionCell.Style.Add( "display", "inline" );
                ResultsChartCell.Style.Add( "display", "inline" );
            }

            if( suggestionsExpanded == false ) {
                SuggestionSelectionRow.Style.Add( "display", "none" );
                SuggestionSelectionRow2.Style.Add( "display", "none" );
                SuggestionSelectionRow3.Style.Add( "display", "none" );
                SuggestionSelectionRow4.Style.Add( "display", "none" );
                SuggestionSelectionRow5.Style.Add( "display", "none" );
            }
            else {
                SuggestionSelectionRow.Style.Add( "display", "inline" );
                SuggestionSelectionRow2.Style.Add( "display", "inline" );
                SuggestionSelectionRow3.Style.Add( "display", "inline" );
                SuggestionSelectionRow4.Style.Add( "display", "inline" );
                SuggestionSelectionRow5.Style.Add( "display", "inline" );
            }

            if( detailsExpanded == false ) {
                //SelectionCell2.Style.Add( "display", "none" );
                //SelectionCell3.Style.Add( "display", "none" );
                ResultsGraphCell.Style.Add( "display", "none" );
                ComparisonPlans.Style.Add( "display", "none" );
                //CompareLabel.Style.Add( "display", "none" );
                GraphSelectionRow.Style.Add( "display", "none" );
                GraphSelectionRow2.Style.Add( "display", "none" );
            }
            else {
                //SelectionCell2.Style.Add( "display", "block" );
                //SelectionCell3.Style.Add( "display", "block" );
                ResultsGraphCell.Style.Add( "display", "inline" );
                if( this.allPlanVersions.ContainsKey( this.currentMediaPlan.CampaignName ) && this.allPlanVersions[ this.currentMediaPlan.CampaignName ].Count > 1 ) {
                    ComparisonPlans.Style.Add( "display", "inline" );
                    CompareLabel.Style.Add( "display", "inline" );
                    CompareLegend.Style.Add( "display", "inline" );
                }
                else {
                    ComparisonPlans.Style.Add( "display", "none" );
                    CompareLabel.Style.Add( "display", "none" );
                    CompareLegend.Style.Add( "display", "none" );
                }
                GraphSelectionRow.Style.Add( "display", "inherit" );
                GraphSelectionRow2.Style.Add( "display", "inherit" );
            }

            if( IsPostBack == false ) {
                ChartSelection.Items.Clear();
                GraphSelection.Items.Clear();
                if( this.currentMediaPlan.SegmentCount > 1 ) {
                    ChartSelection.Items.Add( new ListItem( "All Segments", "-1" ) );
                }
                for( int i = 0; i < this.currentMediaPlan.SegmentCount; i++ ) {
                    string segName = this.currentMediaPlan.Specs.SegmentList[ i ].Name;
                    string[] split = segName.Split( '@' );
                    string demo_name = split[ 0 ];

                    string final_name = demo_name;
                    if( split.Length > 1 ) {
                        string region_name = split[ 1 ];
                        final_name = String.Format( "Seg {0} -- {1}", demo_name, region_name );
                    }
                    ChartSelection.Items.Add( new ListItem( final_name, i.ToString() ) );
                }
                for( int i = 0; i < this.currentMediaPlan.Specs.Demographics.Count; i++ ) {
                    string demoName = this.currentMediaPlan.Specs.Demographics[ i ].DemographicName;
                    GraphSelection.Items.Add( new ListItem( demoName, i.ToString() ) );
                }
                GraphRegion.Items.Clear();
                for( int r = 0; r < this.currentMediaPlan.Specs.GeoRegionNames.Count; r++ ) {
                    GraphRegion.Items.Add( new ListItem( this.currentMediaPlan.Specs.GeoRegionNames[ r ], r.ToString() ) );
                }
                //ChartSelection.Items.Add( new ListItem( "Test Data", "999" ) );

                fill_metric_list();

                ComparisonPlans.Items.Clear();
                ComparisonPlans.Items.Add( new ListItem( "none", "" ) );
                if( this.allPlanVersions.ContainsKey( this.currentMediaPlan.CampaignName ) ) {
                    List<PlanStorage.PlanVersionInfo> vinfo = this.allPlanVersions[ this.currentMediaPlan.CampaignName ];
                    for( int v = 0; v < vinfo.Count; v++ ) {
                        if( vinfo[ v ].Version != this.currentMediaPlan.PlanVersion && vinfo[ v ].StarsRating != -1 ) {
                            ComparisonPlans.Items.Add( new ListItem( vinfo[ v ].Description, vinfo[ v ].Version ) );
                        }
                    }
                }
                //if( ComparisonPlans.Items.Count > 1 ) {
                //    ComparisonPlans.Style.Remove( "display" );
                //    CompareLabel.Style.Remove( "display" );
                //    ComparisonPlans.Style.Add( "display", "inline" );
                //    CompareLabel.Style.Add( "display", "inline" );
                //}
                //else {
                //    ComparisonPlans.Style.Remove( "display" );
                //    CompareLabel.Style.Remove( "display" );
                //    ComparisonPlans.Style.Add( "display", "none" );
                //    CompareLabel.Style.Add( "display", "none" );
                //}
            }
        }
    }

    private void fill_metric_list()
    {
        Dictionary<string, string> name_conversion = new Dictionary<string, string>();
        name_conversion.Add("Persuasion", "");
        name_conversion.Add("Awareness", "Percent aware");
        name_conversion.Add("Recency", "Impressions while shopping");
        name_conversion.Add("Reach0", "");
        name_conversion.Add( "Reach1", "% receiving 1 or more impressions per purchase cycle" );
        name_conversion.Add( "Reach3", "% receiving 3 or more impressions per purchase cycle" );
        name_conversion.Add("Reach4", "");
        name_conversion.Add("Reach3All", "");
        name_conversion.Add("Consideration1", "");
        name_conversion.Add("Consideration3", "");
        name_conversion.Add("Consideration4", "");
        name_conversion.Add("PersuasionIndex", "Ad persuasiveness");
        name_conversion.Add("MarketIndex1", "");
        name_conversion.Add("MarketIndex2", "Marketing effectiveness");
        name_conversion.Add("Reach", "");
        name_conversion.Add("Efficency", "");
        name_conversion.Add("Choosing", "");
        name_conversion.Add("ActualShare", "Percent Choosing");
        name_conversion.Add("EstimatedShare", "Estimated Share");
        name_conversion.Add( "GRP-Total", "GRP All Media" );

        foreach( MediaVehicle.MediaType type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
        {
            name_conversion.Add( "GRP-" + type.ToString(), "GRP " + type.ToString());
        }

        string action_name = currentMediaPlan.Specs.ShareUnits;
        name_conversion.Add("TotalActions", "Total " + action_name);
        GraphMetric.Items.Clear();
        for (int m = 0; m < this.currentMediaPlan.Results.metrics.Count; m++)
        {
            string metricName = this.currentMediaPlan.Results.metrics[m].Type;
            if( name_conversion.ContainsKey(metricName) && name_conversion[metricName] != "" )
            {
                GraphMetric.Items.Add(new ListItem(name_conversion[metricName], m.ToString()));
                name_conversion[metricName] = "";
            }
        }
    }

    protected string ChartImageTag() {
        string seg = "X";
        if( Request[ "ChartSelection" ] != null ) {
            seg = Request[ "ChartSelection" ];
        }
        int showAll = 0;
        if( ShowAllMetrics.Checked ) {
            showAll = 1;
        }
        return String.Format( "<img src=\"ResultsChart.aspx?p={0}&s={1}&a={2}\" />", this.currentMediaPlan.PlanID.ToString(), seg, showAll );
    }

    protected string GraphImageTag() {
        string seg = "-1";
        if( Request[ "GraphSelection" ] != null ) {
            seg = Request[ "GraphSelection" ];
        }

        string metricIndx = "0";
        if( Request[ "GraphMetric" ] != null ) {
            metricIndx = Request[ "GraphMetric" ];
        }

        string regionIndx = "0";
        if( Request[ "GraphRegion" ] != null ) {
            regionIndx = Request[ "GraphRegion" ];
        }

        string compareVersion = "0";
        if( Request[ "ComparisonPlans" ] != null ) {
            compareVersion = Request[ "ComparisonPlans" ];
        }

        return String.Format( "<img src=\"ResultsGraphTwo.aspx?p={0}&s={1}&m={2}&r={3}&c={4}\" />", 
            this.currentMediaPlan.PlanID.ToString(), seg, metricIndx, regionIndx, compareVersion );
    }

    protected void SetupEnterCompetitionLink() {
        EnterCompetitionDiv.InnerHtml = "";
        EnterCompetitionDiv.Visible = false;
        if( this.currentMediaPlan.Specs.CompetitionName != null && this.currentMediaPlan.Specs.CompetitionName.Trim() != "" ) {
            if( this.currentMediaPlan.Specs.CompetitionOwner != Utils.GetUser( this ) ) {
                string compInfo = null;
                if( this.currentMediaPlan.Specs.CompetitionOwner != this.currentMediaPlan.Competitor ) {
                    compInfo = String.Format( "<b>Competition Alert!</b><br><br>This plan is part of the \"{0}\" planning competition, opened on {1} by {2}.<br><br>",
                        this.currentMediaPlan.Specs.CompetitionName, this.currentMediaPlan.Specs.CompetitionDate.ToShortDateString(), this.currentMediaPlan.Specs.CompetitionOwner );

                    if( this.currentMediaPlan.Competitor == null ) {
                        compInfo += "<a href='Compete.aspx' >Enter Competition with this Plan</a>";
                    }
                    else {
                    }
                }
                else {
                    compInfo = String.Format( "<b>Competition Alert!</b><br><br>This plan is the <b>base plan</b> for the \"{0}\" planning competition, opened on {1} by {2}.<br><br>" +
                    "To enter, create your own media plans in this campaign, and look for the \"Enter Competition\" link in this space.  To win, get the highest TOTAL SALES!",
                       this.currentMediaPlan.Specs.CompetitionName, this.currentMediaPlan.Specs.CompetitionDate.ToShortDateString(), this.currentMediaPlan.Specs.CompetitionOwner );
                }

                EnterCompetitionDiv.InnerHtml = compInfo;
                EnterCompetitionDiv.Visible = true;
            }
        }
    }

    protected void SetNameChangeLink() {
        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        string changeNameJS = String.Format( "return ChangePlanName( \"{0}\" );", this.currentMediaPlan.PlanDescription );
        ChangePlanLink.Attributes.Add( "onclick", changeNameJS );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}' );", existingPlanNamesList );
        NewNameBox.Attributes[ "onkeydown" ] = "return CheckForEnter(event);";
    }

    /// <summary>
    /// Handle a click on the Plan-rename link button
    /// </summary>
    private void HandleModifyRequest() {

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanName" ) {
            if( Request[ "newPlanName" ] != null && Request[ "newPlanName" ].Trim() != "" ) {
                if( this.currentMediaPlan.PlanDescription != Request[ "newPlanName" ].Trim() ) {
                    // the name was changed - update it
                    this.currentMediaPlan.PlanDescription = Request[ "newPlanName" ].Trim();
                    PlanStorage storage = new PlanStorage();
                    storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
                    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
                    SetNameChangeLink();
                }
            }
        }
    }

    private void HandleSuggestionAcceptance() {
        if( Request[ "CreateSuggestedPlan.x" ] == null ) {
            return;
        }

        MediaPlan suggestedPlan = (MediaPlan)Session[ "SuggestedPlan" ];

        // check to be sure the name is available
        string suggestedPlanName = Request[ "SuggestedPlanName" ];
        List<PlanStorage.PlanVersionInfo> curVers = this.allPlanVersions[ suggestedPlan.CampaignName ];
        if( curVers != null ) {
            foreach( PlanStorage.PlanVersionInfo vinfo in curVers ) {
                if( vinfo.Description == suggestedPlanName ) {
                    // the name is already in use
                    return;
                }
            }
        }

        Session[ "SuggestedPlan" ] = null;

        if( CreateNew.Checked == false ) {
            // we are going to modify the plan according to the currently selected add/remove items 
            List<MediaItem> suggestedAddItems = (List<MediaItem>)Session[ "SuggestedAddItems" ];
            List<MediaItem> suggestedRemoveItems = (List<MediaItem>)Session[ "SuggestedRemoveItems" ];

            if( suggestedRemoveItems != null ) {
                for( int i = 0; i < suggestedRemoveItems.Count; i++ ) {
                    string rmvKey = String.Format( "Remove{0}", i );
                    if( Request[ rmvKey ] != null ) {
                        suggestedPlan.RemoveMediaItem( suggestedRemoveItems[ i ] );
                    }
                }
            }
            if( suggestedAddItems != null ) {
                for( int i = 0; i < suggestedAddItems.Count; i++ ) {
                    string addKey = String.Format( "Add{0}", i );
                    if( Request[ addKey ] != null ) {
                        suggestedPlan.AddMediaItem( suggestedAddItems[ i ] );
                    }
                }
            }

            Session[ "SuggestedAddItems" ] = null;
            Session[ "SuggestedRemoveItems" ] = null;
        }

        PlanStorage storage = new PlanStorage();
        suggestedPlan.PlanDescription = suggestedPlanName;
        storage.IncrementPlanVersion( suggestedPlan, this.allPlanVersions, userMedia.CurrentPlanVersions );
        Utils.SetAllPlanVersions( this, this.allPlanVersions );


        // change this current plan to the new one
        foreach( MediaPlan cplan in this.currentMediaPlans ) {
            if( cplan.CampaignName == suggestedPlan.CampaignName ) {
                currentMediaPlans.Remove( cplan );
                break;
            }
        }
        currentMediaPlans.Add( suggestedPlan );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );

        this.currentMediaPlan = suggestedPlan;
        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );

        if( CreateNew.Checked == true ) {
            DataLogger.Log( "GEN-ISCN", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription );
        }
        else {
            DataLogger.Log( "GEN-ISME", Utils.GetUser( this ), this.currentMediaPlan.PlanDescription );
        }

        storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
        Response.Redirect( @"Simulation\Simulate.aspx" );
    }

    private void DisplayPlanInfo() {

        CampaignName.Text = Utils.ElideString( this.currentMediaPlan.CampaignName, Utils.MaxLengthPlanName ) +
            " - " + this.currentMediaPlan.Specs.StartDate.ToString( "MM/dd/yy" );
        CampaignName.ToolTip = this.currentMediaPlan.CampaignName;

        string planDesc = this.currentMediaPlan.PlanDescription;
        if( planDesc == null || planDesc == "" ) {
            planDesc = String.Format( "---   [version {0}]", this.currentMediaPlan.PlanVersion );
        }
        PlanName.Text = Utils.ElideString( planDesc, Utils.MaxLengthPlanName ) + " - " + this.currentMediaPlan.StartDate.ToString( "MM/dd/yy" );
        PlanName.ToolTip = planDesc;

        SmallSummary smallSummaryMaker = new SmallSummary();
        smallSummaryMaker.AddSummaryHTML( this.RHInfoDiv, this.currentMediaPlan, this.engineeringMode );

        // display the overall stars rating
        StarsDiv.InnerHtml = "";
        if( this.currentMediaPlan.PlanOverallRatingStars >= 0 ) {
            int nStars = (int)Math.Max( 0, Math.Min( this.currentMediaPlan.PlanOverallRatingStars, 5 ) );

            string starTag = "<img src=\"images/Star19.gif\" style=\"margin-right:1px\" />";
            string blankSstarTag = "<img src=\"images/Star19Blank.gif\" style=\"margin-right:1px\" />";
            for( int s = 0; s < nStars; s++ ) {
                StarsDiv.InnerHtml += starTag;
            }
            for( int s2 = nStars; s2 < 5; s2++ ) {
                StarsDiv.InnerHtml += blankSstarTag;
            }
        }
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
}
