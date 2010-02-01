using System;
using System.Collections;
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

using SimInterface;
using WebLibrary;
using BusinessLogic;

public partial class SimulationResults : System.Web.UI.Page
{
    protected const string adFooter = "";

    protected const string footerHtml = "<br /><br />" + Utils.FooterHtml;

    private string resultsSummary = "";

    protected List<Guid> displayPlanIDs = new List<Guid>(); 
    private List<SimOutput> simOutput;

    private List<int> numMetrics = new List<int>();
    private List<int> numMetricValues = new List<int>();

    private List<List<double>> metricMin;
    private List<List<double>> metricMax;
    private List<List<double>> compareMetricMin;
    private List<List<double>> compareMetricMax;
    private List<List<double>> metricDeltaMin;
    private List<List<double>> metricDeltaMax;

    private List<List<double>> metricOverallValue;          // the business-level score results

    protected List<List<List<double>>> metricDelta;        // difference between corresponding metric values in results and comparison plan results

    protected MediaPlan comparisonMediaPlan;
    protected List<Guid> plansStillRunning = new List<Guid>(); 
    private List<SimUtils.SimStatus> simStatus;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected List<Guid> runningPlans;
    // protected List<MediaPlan> plansBeingEdited;
    protected Guid? referencePlanID;
    protected Guid? comparisonPlanID;
    protected bool engineeringMode;
    protected int storyTabIndex = 0;
    protected UserMedia userMedia;

    protected ResultsListMaker results_maker;

    protected int DisplayPlanCount {
        get { return displayPlanIDs.Count; }
    }

    protected string DisplayPlanIDListString() {
        string s = "";
        if( this.runningPlans != null ) {
            foreach( Guid pid in this.runningPlans ) {
                s += pid.ToString() + "<br> ";
            }
        }
        return s;
    }

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "StoryContainer" ) {
            string tabIndxStr = Request[ "__EVENTARGUMENT" ];
            if( tabIndxStr == "EmailStory" ) {
                StoryGenerator.GetStoryGenerator( this ).SendEmail( Utils.GetUser( this ) );
            }
            else {
                storyTabIndex = int.Parse( tabIndxStr );
            }
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "PlanReportLink" ) {
            string guidStr = Request[ "__EVENTARGUMENT" ];
            Guid guidToReport = new Guid( guidStr );
            this.currentMediaPlan = Utils.PlanForID( guidToReport, this.currentMediaPlans );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            Response.Redirect( "Report.aspx" );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "PlanCloneLink" ) {
            string guidStr = Request[ "__EVENTARGUMENT" ];
            Guid guidToClone = new Guid( guidStr );
            this.currentMediaPlan = Utils.PlanForID( guidToClone, this.currentMediaPlans );
            ClonePlan();
            Response.Redirect( "EditPlan.aspx" );
        }

        // respond to a request to set plan to view by adding it to the the running plans list
        if( Request[ "p" ] != null ) {

            this.runningPlans = new List<Guid>();

            Guid setGuid =  new Guid( Request[ "p" ] );

            if( this.runningPlans.Contains( setGuid ) == false ) {
                this.runningPlans.Add( setGuid );
                Utils.SetRunningMediaPlanIDs( this, this.runningPlans );

                this.currentMediaPlan = Utils.PlanForID( setGuid, this.currentMediaPlans );
                Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            }
        }

        ////// check for postback from tabs
        ////if( IsPostBack && Request[ "ResultsNav.x" ] != null ) {
        ////    int x = 0;
        ////    try {
        ////        x = int.Parse( Request[ "ResultsNav.x" ] );
        ////    }
        ////    catch( Exception ) {
        ////    }
        ////    if( x >= 201 ) {
        ////        string toPage = String.Format( "ShoppingList.aspx?p={0}", this.currentMediaPlan.PlanID.ToString() );
        ////        Response.Redirect( toPage );
        ////    }
        ////    else if( x >= 76 && x <= 199 ) {
        ////        string toPage = String.Format( "Report.aspx?p={0}", this.currentMediaPlan.PlanID.ToString() );
        ////        Response.Redirect( toPage );
        ////    }
        ////}
            
        if( IsPostBack == false ) {
            // populate the add0item list and select the approipriate item
            PopulateComparisonDropdown();
        }

        // Display the status of all media plans in the runningPlans list - ALSO this sets up the simOutput list

        if( this.runningPlans != null ) {
            //!!! remove all but the last from the running-IDs list of there are more than one!!
            if( this.runningPlans.Count > 1 ) {
                Guid lastGuid = this.runningPlans[ this.runningPlans.Count - 1 ];
                this.runningPlans = new List<Guid>();
                this.runningPlans.Add( lastGuid );
                Utils.SetRunningMediaPlanIDs( this, this.runningPlans );
            }
        }
        else {
            // if there is no running-plans list at all, make the curent plan the one "running" plan
            if( this.currentMediaPlan != null ) {
                this.runningPlans = new List<Guid>();
                this.runningPlans.Add( this.currentMediaPlan.PlanID );
                Utils.SetRunningMediaPlanIDs( this, this.runningPlans );
            }
        }
        DisplaySimStatusList( this.runningPlans );


        results_maker = new ResultsListMaker(currentMediaPlan);

        string allPlanSuggestions =
            results_maker.AddResultsListHTML(this.ResultTableDiv, footerHtml, this.engineeringMode);

        CampaignSummaryDiv.InnerHtml = this.currentMediaPlan.Specs.GetCampaignSummary( adFooter );

        StoryDisplayer sDisp = new StoryDisplayer( StoryGenerator.GetStoryGenerator( this ), "", this.engineeringMode );
        sDisp.PopulateDisplayDiv( this.StoryDiv, this.StoryBodyDiv, this.StoryFooterDiv, this.storyTabIndex );

        this.StoryBodyDiv.Visible = false;

        if( Utils.InEngineeringMode( this ) == true ) {
            DebugListDiv.Visible = true;
            //SetRefButtonDiv.Visible = true;
            DebugData.Visible = true;
            //CompareLabel.Visible = true;
            //CompareList.Visible = true;
        }
    }

    protected void DisplaySimStatusList( List<Guid> displayPlanIDsList ) {
        this.displayPlanIDs = displayPlanIDsList;


        simOutput = new List<SimOutput>();
        for( int i = 0; i < this.displayPlanIDs.Count; i++ ) {
            Guid planID = this.displayPlanIDs[ i ];
            MediaPlan chkPlan = Utils.PlanForID( planID, this.currentMediaPlans );
            simOutput.Add( chkPlan.Results );
        }

        ComputeResultsRanges();

        //
    }

    //protected string TextForSuggestionWindow( int indx, List<double> allPlanStars, List<string> allPlanReasons, List<string> allPlanNextStarSteps, List<string> allPlanIssuesSummary, 
    //    List<Dictionary<MediaCampaignSpecs.CampaignGoal, List<Scoring.SuggestedImprovement>>> allPlanSuggestions, List<List<string>> allPlanCongrats ) {

    //    string s = "";
    //    MediaPlan plan = Utils.PlanForID( this.displayPlanIDs[ indx ], this.currentMediaPlans );
    //    return s;
    //}

    protected string SegmentName( int planIndex, int segmentIndex ) {
        MediaPlan plan = Utils.PlanForID( this.displayPlanIDs[ planIndex ], this.currentMediaPlans );
        return plan.Specs.Demographics[ segmentIndex ].DemographicName;
    }

    /// <summary>
    /// Initializes the CompareList item in the UI.
    /// </summary>
    protected void PopulateComparisonDropdown() {
        ////CompareList.Items.Clear();
        ////CompareList.Items.Add( "None" );
        ////CompareList.SelectedIndex = 0;

        //AddList.Items.Clear();
        //AddList.Items.Add( "--" );
        //AddList.SelectedIndex = 0;

        //////int curIndx = 2;
        //foreach( MediaPlan otherPlan in this.allMediaPlans ) {
        //    if( (this.currentMediaPlan != null) && (otherPlan.PlanID != this.currentMediaPlan.PlanID) ) {
        //        if( otherPlan.PlanStatus == MediaPlan.MediaPlanStatus.SIMULATED ) {
        //            ////CompareList.Items.Add( new ListItem( otherPlan.PlanName, otherPlan.PlanID.ToString() ) );
        //            AddList.Items.Add( new ListItem( otherPlan.PlanName, otherPlan.PlanID.ToString() ) );

        //            ////if( otherPlan.PlanID == this.comparisonPlanID ) {
        //            ////    CompareList.SelectedIndex = curIndx;
        //            ////}
        //        }
        //    }
        //}
    }

    protected void ComputeResultsRanges(){

        this.metricOverallValue = new List<List<double>>();

        this.metricMax = new List<List<double>>();
        this.metricMin = new List<List<double>>();
        this.metricMax = new List<List<double>>();
        this.compareMetricMax = new List<List<double>>();
        this.compareMetricMin = new List<List<double>>();
        this.metricDeltaMax = new List<List<double>>();
        this.metricDeltaMin = new List<List<double>>();
        this.metricDelta = new List<List<List<double>>>();

        for( int planIndex = 0; planIndex < this.DisplayPlanCount; planIndex++ ) {

            List<double> metricOverallValueList = new List<double>( NumMetrics( planIndex ) );
            List<double> metricMaxPlanList = new List<double>( NumMetrics( planIndex ) );
            List<double> metricMinPlanList = new List<double>( NumMetrics( planIndex ) );
            List<double> compareMetricMinPlanList = new List<double>( NumMetrics( planIndex ) );
            List<double> compareMetricMaxPlanList = new List<double>( NumMetrics( planIndex ) );
            List<double> metricDeltaMinPlanList = new List<double>( NumMetrics( planIndex ) );
            List<double> metricDeltaMaxPlanList = new List<double>( NumMetrics( planIndex ) );
            List<List<double>> metricDeltaPlanList = new List<List<double>>();

            for( int i = 0; i < NumMetrics( planIndex ); i++ ) {

                metricOverallValueList.Add( 0.0 );

                metricMaxPlanList.Add( Double.MinValue );
                metricMinPlanList.Add( Double.MaxValue );
                compareMetricMinPlanList.Add( Double.MaxValue );
                compareMetricMaxPlanList.Add( Double.MinValue );
                metricDeltaMinPlanList.Add( Double.MaxValue );
                metricDeltaMaxPlanList.Add( Double.MinValue );
                metricDeltaPlanList.Add( new List<double>( NumMetrics( planIndex ) ) );

                for( int v = 0; v < NumMetricValues( planIndex, i ); v++ ) {

                    double val = MetricValue( planIndex, i, v );

                    metricMaxPlanList[ i ] = Math.Max( metricMaxPlanList[ i ], val );
                    metricMinPlanList[ i ] = Math.Min( metricMinPlanList[ i ], val );

                    // accumulate the average overall value
                    metricOverallValueList[ i ] += val / (double)NumMetricValues( planIndex, i );

                    if( this.comparisonMediaPlan != null ) {
                        double comp_val = MetricValue( planIndex, i, v, true );
                        double err = val - comp_val;
                        metricDeltaPlanList[ i ].Add( err );

                        compareMetricMaxPlanList[ i ] = Math.Max( compareMetricMaxPlanList[ i ], comp_val );
                        compareMetricMinPlanList[ i ] = Math.Min( compareMetricMinPlanList[ i ], comp_val );

                        metricDeltaMaxPlanList[ i ] = Math.Max( metricDeltaMaxPlanList[ i ], err );
                        metricDeltaMinPlanList[ i ] = Math.Min( metricDeltaMinPlanList[ i ], err );
                    }
                }
            }

            this.metricOverallValue.Add( metricOverallValueList );

            this.metricMax.Add( metricMaxPlanList );
            this.metricMin.Add( metricMinPlanList );
            this.compareMetricMax.Add( compareMetricMaxPlanList );
            this.compareMetricMin.Add( compareMetricMinPlanList );
            this.metricDeltaMax.Add( metricMaxPlanList );
            this.metricDeltaMin.Add( metricDeltaMinPlanList );
            this.metricDelta.Add( metricDeltaPlanList );
        }
    }

    protected bool PlanIsDone( int planIndex ) {
        //if( planIndex < this.displayPlanIDs.Count ) {
        //    MediaPlan plan = Utils.PlanForID( this.displayPlanIDs[ planIndex ], this.allMediaPlans );
        //    if( plan != null && plan.Results != null ) {
        //        // a plan in the displayPlanIDs list can have results only if it is done
        //        return true;
        //    }
        //}
        //return false;
        return true;        // should not be at results page unless plan is done
    }

    protected double ProgressPercent( int planIndex ) {
        SimUtils.SimStatus theStatus = simStatus[ planIndex ];
        if( theStatus != null ) {
            return theStatus.ProgressPercent;
        }
        else {
            return -1;
        }
    }

    protected string ProgressBarString( int planIndex ) {
        double progress = ProgressPercent( planIndex );
        string progressStr = "&nbsp;&nbsp;";
        for( int i = 0; i < progress; i++ ) {
            progressStr += "&nbsp;&nbsp;";
        }
        return progressStr;
    }

    protected int NumMetrics( int planIndex ) {
        int numMetrics = 0;
        if( planIndex < this.displayPlanIDs.Count ) {
            MediaPlan plan = Utils.PlanForID( this.displayPlanIDs[ planIndex ], this.currentMediaPlans );
            if( plan != null && plan.Results != null ) {
                numMetrics = plan.Results.metrics.Count;
            }
        }
        return numMetrics;
    }

    protected int NumMetricValues( int planIndex, int metricIndex ) {
        Metric metric = simOutput[ planIndex ].metrics[ metricIndex ];
        return metric.values.Count;
    }

    protected string PlanName( int planIndex ) {
        MediaPlan plan = Utils.PlanForID( this.displayPlanIDs[ planIndex ], this.currentMediaPlans );
        return plan.PlanName;
    }

    protected Guid PlanID( int planIndex ) {
        return this.displayPlanIDs[ planIndex ];
    }

    protected string MetricName( int planIndex, int metricIndex ) {
        Metric metric = simOutput[ planIndex ].metrics[ metricIndex ];
        return metric.Type;
    }

    protected double MetricValue( int planIndex, int metricIndex, int valueIndex, bool useComparisonPlan ) {
        if( useComparisonPlan == true ) {
            if( this.comparisonMediaPlan.Results != null && metricIndex < this.comparisonMediaPlan.Results.metrics.Count ) {
                Metric metric = this.comparisonMediaPlan.Results.metrics[ metricIndex ];
                if( valueIndex < metric.values.Count ) {
                    return (double)metric.values[ valueIndex ];
                }
                else {
                    // this metric doesn't have this value index-- return 0
                    return 0;
                }
            }
            else {
                // this plan doesn't have this result metric -- return 0
                return 0;
            }
        }
        else {
            Metric metric = simOutput[ planIndex ].metrics[ metricIndex ];
            return (double)metric.values[ valueIndex ];
        }
    }

    protected string PlanOverallValue( int planIndex ) {
        double val = 0;
        double nMetrics = NumMetrics( planIndex );
        for( int m = 0; m < nMetrics; m++ ) {
            val += metricOverallValue[ planIndex ][ m ] / nMetrics;
        }
        val = Math.Min( val / 2, 5 );
        string sval = String.Format( "{0:f0} Stars", val );
        return sval;
    }

    protected string MetricOverallValue( int planIndex, int metricIndex ) {
        double val = metricOverallValue[ planIndex ][ metricIndex ];
        string sval = String.Format( "{0:f1}", val );
        return sval;
    }

    protected double MetricValue( int planIndex, int metricIndex, int valueIndex ) {
        return MetricValue( planIndex, metricIndex, valueIndex, false );
    }

    protected string MetricValueString( int planIndex, int metricIndex, int valueIndex, int decimalPlaces, bool useComparisonPlan ) {
        string format = String.Format( "0:f{0}", decimalPlaces );
        string valStr = String.Format( "{" + format + "}", MetricValue( planIndex, metricIndex, valueIndex, useComparisonPlan ) );
        return valStr;
    }

    protected string MetricValueString( int planIndex, int metricIndex, int valueIndex, int decimalPlaces ) {
        return MetricValueString( planIndex, metricIndex, valueIndex, decimalPlaces, false );
    }

    protected string MetricValueDiffColor( int planIndex, int metricIndex, int valueIndex ) {
        return MetricValueColor( planIndex, metricIndex, valueIndex, false, true );
    }

    protected string MetricValueColor( int planIndex, int metricIndex, int valueIndex, bool useComparisonPlan ) {
        return MetricValueColor( planIndex, metricIndex, valueIndex, useComparisonPlan, false );
    }

    protected string MetricValueColor( int planIndex, int metricIndex, int valueIndex ) {
        return MetricValueColor( planIndex, metricIndex, valueIndex, false, false );
    }

    protected string MetricValueColor( int planIndex, int metricIndex, int valueIndex, bool useComparisonPlan, bool useDiff ) {
        string color = "#FF0000";

        if( metricIndex == -1 ){
            // the headings (detes) row
           return   "#AABBCC";
        }

        double val = MetricValue( planIndex, metricIndex, valueIndex, useComparisonPlan );

        double valRange = metricMax[ planIndex ][ metricIndex ] - metricMin[ planIndex ][ metricIndex ];
        double normalizedVal = 0;
        if( valRange != 0 ) {
            normalizedVal = (val - metricMin[ planIndex ][ metricIndex ]) / valRange;
        }

        bool diffIsNegative = false;
        if( useComparisonPlan == true ) {
            valRange = compareMetricMax[ planIndex ][ metricIndex ] - compareMetricMin[ planIndex ][ metricIndex ];
            normalizedVal = 0;
            if( valRange != 0 ) {
                normalizedVal = (val - compareMetricMin[ planIndex ][ metricIndex ]) / valRange;
            }
        }
        if( useDiff == true ) {
            val = metricDelta[ planIndex ][ metricIndex ][ valueIndex ];
            double posValRange = Math.Max( metricDeltaMax[ planIndex ][ metricIndex ], 0 );
            double negValRange = Math.Max( -metricDeltaMin[ planIndex ][ metricIndex ], 0 );
            if( val >= 0 ) {
                normalizedVal = 0;
                if( posValRange != 0 ) {
                    normalizedVal = val / posValRange;
                }
            }
            else {
                normalizedVal = 0;
                if( negValRange != 0 ) {
                    normalizedVal = val / negValRange;
                }
                diffIsNegative = true;
            }
        }

        int r1 = 0xCC;         // odd row color
        int g1 = 0x66;
        int b1 = 0xEE;

        int r2 = 0x55;         // even row color
        int g2 = 0xFF;
        int b2 = 0xAA;

        int r_dp = 0xFF;         // positive difference color
        int g_dp = 0x00;
        int b_dp = 0x00;

        int r_dn = 0x00;         // negative difference color
        int g_dn = 0xFF;
        int b_dn = 0x00;

        int r = -1;
        int g =-1;
        int b = -1;

        if( useDiff == false ) {
            if( metricIndex % 2 == 0 ) {
                r = ValueColor( r1, normalizedVal );
                g = ValueColor( g1, normalizedVal );
                b = ValueColor( b1, normalizedVal );
            }
            else {
                r = ValueColor( r2, normalizedVal );
                g = ValueColor( g2, normalizedVal );
                b = ValueColor( b2, normalizedVal );
            }
        }
        else {
            // doing a diff display
            if( diffIsNegative == false ) {
                r = ValueColor( r_dp, normalizedVal );
                g = ValueColor( g_dp, normalizedVal );
                b = ValueColor( b_dp, normalizedVal );
            }
            else {
                r = ValueColor( r_dn, normalizedVal );
                g = ValueColor( g_dn, normalizedVal );
                b = ValueColor( b_dn, normalizedVal );
            }
        }

        color = String.Format( "#{0:x2}{1:x2}{2:x2}", r, g, b );
        return color;
    }

    protected int ValueColor( int baseColor, double ratio ) {
        int c = 0xFF - baseColor;
        int c2 = (int)Math.Round( c * ratio * ratio );
        int c3 =  0xFF - c2;
        return c3;
    }

    protected string SimResultsMetricsText( SimOutput output ) {

        string s = "";
        foreach( Metric metric in output.metrics ) {
            s += metric.Type + "<br>";
            s += metric.Segment + "<br>";

            foreach( float val in metric.values ) {

                s += "," + val.ToString();
            }
            s += "<br>";
        }
        return s;
    }

    protected string SimResultsSummaryText() {
        string simResults = "Simulation Results: <br>";

        simResults += resultsSummary;

        return simResults;
    }

    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );
        //this.plansBeingEdited = Utils.PlansBeingEdited( this );
        this.runningPlans = Utils.RunningMediaPlanIDs( this );

        bool loadUserMedia = false;
        this.userMedia = Utils.CurrentUserProfile( this, out loadUserMedia );
        if( loadUserMedia == true ) {
            this.userMedia = Profile.UserMediaItems;
            Utils.SetCurrentUserProfile( this, this.userMedia );
        }

        string helpEmail = String.Format( "<a href=\"mailto:support@adplanit.com?subject=AdPlanit Support Request - {0}\">Question? Send us a message!</a>", "Simulation Resullts" );
        HelpEmailLink.Text = helpEmail;
    }


    /// <summary>
    /// Redirects a user who ins't logged-in back to the login page
    /// </summary>
    private void RedirectUnknownUsers() {
        if( this.User == null || this.User.Identity == null || this.User.Identity.Name == null || this.User.Identity.Name == "" ) {
            Response.Redirect( "Login.aspx" );
        }
    }



    ////protected void SetRefButton_Click( object sender, EventArgs e ) {
    ////    SetCurrentPlanAsReference();
    ////}

    protected void ListButton_Click( object sender, EventArgs e ) {
        ClearAllButStillRunningSimsFromActiveList();
        Server.Transfer( "~/PlansList.aspx" );
    }

    private void ClearAllButStillRunningSimsFromActiveList() {
        this.runningPlans = this.plansStillRunning;
        Utils.SetRunningMediaPlanIDs( this, this.runningPlans );
    }

    protected void OverButton_Click( object sender, EventArgs e ) {
        ClearAllButStillRunningSimsFromActiveList();
        this.currentMediaPlan = new MediaPlan();
        Utils.SetCurrentMediaPlan( this, currentMediaPlan );
        Response.Redirect( "~/StartSearch.aspx" );
    }

    ////private void SetCurrentPlanAsReference() {
    ////    this.referencePlanID = this.currentMediaPlan.PlanID;
    ////    Utils.SetReferenceMediaPlan( this, this.referencePlanID );       //set session variable
    ////    AddRefererenceItemToMenu( this.referencePlanID, true );
    ////    SetComparisonDataDisplay( this.referencePlanID );
    ////}

    /////// <summary>
    /////// Adds the "Reference" item to the CompareList plan comparison menu.
    /////// </summary>
    /////// <param name="referencePlanID"></param>
    /////// <param name="selectNow"></param>
    ////private void AddRefererenceItemToMenu( Guid? referencePlanID, bool selectNow ){
    ////    if( referencePlanID == null ) {
    ////        return;
    ////    }

    ////    if( CompareList.Items.Count > 1 && CompareList.Items[ 1 ].Text == "Reference" ) {
    ////        // there is already a Reference item in the menu -- reset the ID value
    ////        CompareList.Items[ 1 ].Value = referencePlanID.ToString();
    ////    }
    ////    else {
    ////        // there is no Reference item in te menu -- create and add one
    ////        ListItem refListItem = new ListItem( "Reference", referencePlanID.ToString() );
    ////        CompareList.Items.Insert( 1, refListItem );
    ////    }

    ////    if( selectNow ) {
    ////        CompareList.SelectedIndex = 1;
    ////    }
    ////}

    ////protected void AddList_SelectedIndexChanged( object sender, EventArgs e ) {
    ////    if( AddList.SelectedIndex > 0 ) {
    ////        string compGuidStr = (string)AddList.SelectedValue;
    ////        Guid compGuid = new Guid( compGuidStr );
    ////        if( this.runningPlans.Contains( compGuid ) == false ){
    ////            MediaPlan addMediaPlan = Utils.PlanForID( compGuid, this.allMediaPlans );
    ////            this.runningPlans.Add( compGuid );
    ////            Utils.SetRunningMediaPlanIDs( this, this.runningPlans );
    ////            simOutput.Add( addMediaPlan.Results );
    ////            ComputeResultsRanges();
    ////        }
    ////    }
    ////}

    ////protected void CompareList_SelectedIndexChanged( object sender, EventArgs e ) {
    ////    if( CompareList.SelectedIndex <= 0 ) {
    ////        // turn the comparison display off
    ////        SetComparisonDataDisplay( null );
    ////    }
    ////    else if( CompareList.SelectedIndex == 1 ) {
    ////        // compare to the reference plan
    ////        SetComparisonDataDisplay( this.referencePlanID );
    ////    }
    ////    else {
    ////        string compGuidStr = (string)CompareList.SelectedValue;
    ////        Guid compGuid = new Guid( compGuidStr );
    ////        MediaPlan comparePlan = Utils.PlanForID( compGuid, this.allMediaPlans );
    ////        SetComparisonDataDisplay( (Guid?)compGuid );
    ////    }
    ////}

    /// <summary>
    /// Sets the plan to compare the current plan to.  Set to null to hide the comparison display.
    /// </summary>
    /// <param name="comparisonPlan"></param>
    private void SetComparisonDataDisplay( Guid? comparisonPlanID ) {
        this.comparisonMediaPlan = null;
        if( comparisonPlanID != null ) {
            this.comparisonMediaPlan = Utils.PlanForID( (Guid)comparisonPlanID, this.currentMediaPlans );
        }
        ComputeResultsRanges();
        Utils.SetComparisonMediaPlan( this, comparisonPlanID );
    }

    protected string DebugDataText() {
        return "";
        //return Utils.GetDebugData( this );
    }

    protected void HomeButton_Click( object sender, EventArgs e ) {
        Response.Redirect( "~/Home.aspx" );
    }

    /// <summary>
    /// Creates a clone of the current media plan.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ClonePlan() {

        //!!! THIS IS DUPLICATED IN PLANSLIST.ASPX !!!
        int cloneNum = 0;
        string cloneName;
        bool nameExistsAlready = false;
        do {
            nameExistsAlready = false;
            cloneNum += 1;
            cloneName = String.Format( "{0} var {1}", this.currentMediaPlan.PlanName, cloneNum );
            //foreach( MediaPlan ePlan in this.plansBeingEdited ) {
            //    if( ePlan.PlanName == cloneName ) {
            //        nameExistsAlready = true;
            //        break;
            //    }
            //}
            foreach( MediaPlan ePlan in this.currentMediaPlans ) {
                if( ePlan.PlanName == cloneName ) {
                    nameExistsAlready = true;
                    break;
                }
            }
        }
        while( nameExistsAlready );

        MediaPlan clonedPlan = new MediaPlan( this.currentMediaPlan, cloneName );
        //this.plansBeingEdited = new List<MediaPlan>();
        //this.plansBeingEdited.Add( clonedPlan );
        //Utils.SetPlansBeingEdited( this, this.plansBeingEdited );
        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
    }

}
