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

using HouseholdLibrary;
using WebLibrary;
using MediaLibrary;
using GeoLibrary;
using BusinessLogic;

public partial class HelpPage : System.Web.UI.Page
{
    protected AjaxControlToolkit.SliderExtender[] sliders;

    protected string DescriptiveText = "Set the values below to describe the \"campaign\" you are planning for. " +
        "Then you can test and compare media plans for their effectiveness in the campaign.";

    protected string DescriptiveText2 = "<b>Timing</b>.";

    protected string DescriptiveText3 = "<b>Business Type</b>. Choose the category and type that best describes your product or service:";

    protected string DescriptiveText3a = "<b>Goals</b>. Set the goals you wish to accomplish in this campaign, with the most important first:";

    protected string DescriptiveText4 = "<b>Describe</b> your current or proposed media plan.";

    protected string ItemSidebar = "Your Media Plan";

    protected string DescriptiveText999 =
        "Enter a new <b>title</b> if desired." +
        "<br><br>Adjust <b>Purchase Interval</b> and  <b>Purchase Type</b> to describe the sort of product or service you are marketing. AdPlanIt currently only supports businesses that sell to consumers." +
        "<br><br>Use <b>Target Customers</b> to describe your customer and where they live." +
        "<br><br>In the next step, AdPlanIt will suggest a recommended plan to you based on expert opinions.  You can then adjust the recommendation " +
        "and run It past the virtual population of consumers who will experience your media choices and then tell you how effective your campaign was.";

    protected string[] priorityListItemNames = new string[] {
         "-",
         "reach a LOT of people",
         "reach people in a specific LOCATION",
         "PERSUADE the people I reach",
         "reach the right AGE, GENDER, etc.",
         "reach people at the right TIME"
    };

    protected string[] priorityListItemCodes = new string[] {
         "-",
         "A",
         "G",
         "P",
         "D",
         "T"
    };

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected bool editMode = false;
    protected bool creatingNew = false;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        //Utils.AddWaitCursorOnClick( DefineSegButton, "MainDIv" );
        ////Utils.AddWaitCursorOnClick( NextButton, "MainDIv" );
        ////Utils.AddWaitCursorOnClick( NextButton2, "MainDIv" );
        ////Utils.AddWaitCursorOnClick( NextButton3, "MainDIv" );
        //Utils.AddWaitCursorOnClick( DefineSegButton, "DemoPopupInsetPanel" );

        GoalWeightR.Attributes.Add( "onkeyup", "CheckWeightSum()" );
        GoalWeightG.Attributes.Add( "onkeyup", "CheckWeightSum()" );
        GoalWeightP.Attributes.Add( "onkeyup", "CheckWeightSum()" );
        GoalWeightD.Attributes.Add( "onkeyup", "CheckWeightSum()" );
        GoalWeightT.Attributes.Add( "onkeyup", "CheckWeightSum()" );

        CategoryList.Attributes.Add( "onChange", "ShowSubcategoryProgress();" );

        // respond to a request to change/set current plan
        if( Request[ "p" ] != null ) {
            this.currentMediaPlan = Utils.PlanForID( Request[ "p" ], this.currentMediaPlans );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }

        // make sure the auto-start mode is set appropriately
        if( IsPostBack == false ) {
            if( Request[ "GeneratePlanButton.x" ] != null || Request[ "autogen" ] != null ) {
                this.AutoStartMode.Value = "true";
            }
            else {
                this.AutoStartMode.Value = "false";
            }
        }
        else {
            // preserve value on postback
            this.AutoStartMode.Value = Request[ "AutoStartMode" ];
        }

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "AddRegion" ) {
            UpdateCurrentPlan();
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            Response.Redirect( "CampaignRegion.aspx" );
        }

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "AddSegment" ) {
            UpdateCurrentPlan();
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            Response.Redirect( "CampaignDemographic.aspx" );
        }

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "EditSegment" ) {
            UpdateCurrentPlan();
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );

            int segToEdit = int.Parse(  Request[ "__EVENTARGUMENT" ] );
            Response.Redirect( String.Format( "CampaignDemographic.aspx?DemographicIndex={0}", segToEdit ) );
        }

        // see if we want to go into engineering mode
        engineeringMode = Utils.InEngineeringMode( this, null );

        PlanStorage storage = new PlanStorage();

        bool noResultsYet = true;
        if( this.currentMediaPlan != null ) {
            noResultsYet = storage.AllPlansInCampaignHaveNoResults( Utils.GetUser( this ), this.currentMediaPlan.CampaignName, this.allPlanVersions, this.currentMediaPlans );
        }

        if( this.currentMediaPlan == null || noResultsYet ) {
            // We can keep "creating" (i.e. freely editing) a campaign as long as there are no results...
            editMode = false;
        }
        else {
            //...otherwise we need to restrict what can be edited to the items that will not invalidate existing results.
            editMode = true;
        }


        if( IsPostBack == false && Request.UrlReferrer != null || Request.UrlReferrer.ToString().IndexOf( "Campaigns.aspx" ) != -1 ) {
            // set the flag that says we're not creating a new campaign - we will change this to true in a few lines if necessary
            Session[ "CreatingNewCampaign" ] = false;      
        }
        else {
            // otherwise just keep the existing state
            creatingNew = (bool)Session[ "CreatingNewCampaign" ];
        }

        // create a new campaign if the session has a null currentMediaPlan
        if( this.currentMediaPlan == null ) {
            this.currentMediaPlan = new MediaPlan( Utils.NewCampaignName( this.currentMediaPlans ) );     // create a new plan/campaign !!
            this.currentMediaPlan.PlanValid = false;       // this "plan" is only a campaign for now
            this.currentMediaPlan.Specs.SetDefaultGoals();                 // set the default values for goals

            // we know for sure we're creating a new campaign now
            creatingNew = true;
            Session[ "CreatingNewCampaign" ] = true;

            // if we have access to the user prefs, set the first segment's region name to the one used previously
            if( this.userMedia != null ) {
                List<string> userPrefGeos = this.userMedia.InitialGeoRegionChoices;
                if( userPrefGeos != null && userPrefGeos.Count > 0 ) {
                    foreach( string geoRgn in userPrefGeos ) {
                        this.currentMediaPlan.Specs.GeoRegionNames.Add( geoRgn );
                    }
                }
            }

            DemographicSettings demoSettings = new DemographicSettings();
            demoSettings.DemographicName = "Everybody";
            this.currentMediaPlan.Specs.Demographics.Add( demoSettings );



            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }


        string existingPlanNamesList = Utils.ConvertToJavascriptArg( Utils.AllPlanNamesFor( this.currentMediaPlan.CampaignName, this.allPlanVersions ) );
        string suggestedNewAutogenName = Utils.GetSuggestedPlanName( "Autogenerated Plan", this.currentMediaPlan.CampaignName, this.allPlanVersions, true );
        string suggestedNewName = Utils.GetSuggestedPlanName( "Media Plan", this.currentMediaPlan.CampaignName, this.allPlanVersions, false );
        hideModalPopupViaClientButton.Attributes[ "onclick" ] = String.Format( "return ReceiveNewPlanName( '{0}' );", existingPlanNamesList );
        NewNameBox.Attributes[ "onkeydown" ] = "return CheckForEnter(event);";
        if( editMode == false ) {
            if( this.currentMediaPlan.Specs.GeoRegionNames.Count > 0 ) {
                CreatePlanButton.OnClientClick = "planIsAutogenerated=true; if( CampaignNameIsLegal() ){ return GetPlanName( '" + suggestedNewAutogenName + "' ); }else{return false;} ";
                EnterPlanButton.OnClientClick = "planIsAutogenerated=false; if( CampaignNameIsLegal() ){ return GetPlanName( '" + suggestedNewName + "' ); }else{return false;} ";
            }
            else {
                CreatePlanButton.OnClientClick = "ShowNoRegionWarning(); return false";
                EnterPlanButton.OnClientClick = "ShowNoRegionWarning(); return false";
            }
        }
        else {
            CreatePlanButton.OnClientClick = "planIsAutogenerated=true; if( CampaignNameIsLegal() ){ return GetPlanName( '" + suggestedNewAutogenName + "' ); }else{return false;} ";
            EnterPlanButton.OnClientClick = "planIsAutogenerated=false; if( CampaignNameIsLegal() ){ return GetPlanName( '" + suggestedNewName + "' ); }else{return false;} ";
        }

        GoalSelR.Attributes.Add( "onClick", "CheckGoalCheckboxes();" );
        GoalSelG.Attributes.Add( "onClick", "CheckGoalCheckboxes();" );
        GoalSelP.Attributes.Add( "onClick", "CheckGoalCheckboxes();" );
        GoalSelD.Attributes.Add( "onClick", "CheckGoalCheckboxes();" );
        GoalSelT.Attributes.Add( "onClick", "CheckGoalCheckboxes();" );

        SaveCampaignButton.OnClientClick = "return CampaignNameIsLegal();";

        if( Request[ "deleteSegmentId" ] != null && Request[ "deleteSegmentId" ] != "" ) {
            this.currentMediaPlan.RemoveSegment( new Guid( Request[ "deleteSegmentId" ] ) );
        }

        if( Request[ "deleteRegionNum" ] != null && Request[ "deleteRegionNum" ] != "" ) {
            int rgnNum = int.Parse( Request[ "deleteRegionNum" ] );
            this.currentMediaPlan.Specs.GeoRegionNames.RemoveAt( rgnNum );
        }

        sliders = new AjaxControlToolkit.SliderExtender[]{ 
            SliderExtender1
        };

        foreach( AjaxControlToolkit.SliderExtender slider in sliders ) {
            slider.EnableHandleAnimation = true;
            slider.Length = 365;
        }


        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ChangePlanName" ) {
            if( Request[ "__EVENTARGUMENT" ] == "true" ) {
                NextButton2_Click( null, null );
            }
            else {
                NextButton_Click( null, null );
            }
        }


        // verify that the geo info is initialized
        if( GeoRegion.TopGeo == null ) {
            throw new Exception( "Error: Problem loading geographic information: TopGeo object is null." );
        }

        if( IsPostBack && CategoryList.SelectedIndex > 0 ) {
            SubcategoryList.CssClass = "SubcategoryTextbox_Shown";
            SubcategoryTextbox.CssClass = "SubcategoryTextbox_Hidden";
        }
        else {
            SubcategoryList.CssClass = "SubcategoryList_Hidden";
            SubcategoryTextbox.CssClass = "SubcategoryTextbox_Hidden";
        }

        if(this.editMode )
        {
            DurationTextBox.ReadOnly = true;
            DurationTextBox.BorderStyle = BorderStyle.None;

            StartDateTextBox.ReadOnly = true;
            StartDateTextBox.BorderStyle = BorderStyle.None;
            StartDateCalendar.Enabled = false;
        }

        if( IsPostBack == false ) {
            // set the UI items to agree with the current media plan
            CampaignNameTextBox.Text = this.currentMediaPlan.CampaignName;
            //Comments.Text = this.currentMediaPlan.Comments;
            StartDateTextBox.Text = this.currentMediaPlan.StartDate.ToShortDateString();

            int nWeeks = (int)Math.Ceiling( (this.currentMediaPlan.EndDate - this.currentMediaPlan.StartDate).TotalDays / 7 );
            DurationTextBox.Text = nWeeks.ToString();
            PlanCost.Text = String.Format( "{0:f0}", Math.Round( this.currentMediaPlan.TargetBudget / 1000 ) );
            purchaseCycleTextBox.Text = this.currentMediaPlan.PurchaseCycleCode.ToString();
            ConsiderationBox.Text = this.currentMediaPlan.Specs.TimeInConsideration.ToString();
            
            PrevSpendingBox.Text = String.Format( "{0:f0}", Math.Round( this.currentMediaPlan.Specs.PreviousMediaSpend / 1000 ) );
            CurrConversionBox.Text = String.Format( "{0:f0}", Math.Round( this.currentMediaPlan.Specs.InitialShare ) );
            if( this.currentMediaPlan.Specs.ShareUnits != null ) {
                ConversionUnits.SelectedValue = this.currentMediaPlan.Specs.ShareUnits;
            }

            // set up the categories lists
            BusinessLogic.BusinessCategories bc = new BusinessLogic.BusinessCategories();
            CategoryList.Items.Clear();
            for( int bci = 0; bci < bc.Categories.Length; bci++ ) {
                CategoryList.Items.Add( bc.Categories[ bci ] );
            }

            string cat = this.currentMediaPlan.Specs.BusinessCategory;

            if( cat != "Other" && cat != "")
            {
                CategoryList.SelectedValue = cat;

                string[] subcats = bc.Subcategories( cat );
                if( subcats != null )
                {
                    SubcategoryList.CssClass = "SubcategoryList_Shown";
                    SubcategoryList.Items.Clear();
                    for( int i = 0; i < subcats.Length; i++ )
                    {
                        SubcategoryList.Items.Add( subcats[i] );
                    }


                    try
                    {
                        SubcategoryList.SelectedValue = currentMediaPlan.Specs.BusinessSubcategory;
                    }
                    catch( Exception ) { }
                }
            }
            else
            {
                CategoryList.SelectedValue = cat;

                SubcategoryTextbox.CssClass = "SubcategoryTextbox_Shown";
                SubcategoryList.CssClass = "SubcategoryList_Hidden";
            }

            GoalWeightR.Text = "";
            GoalSelR.Checked = false;
            GoalWeightR.CssClass = "HideGoalWeight";

            GoalWeightG.Text = "";
            GoalSelG.Checked = false;
            GoalWeightG.CssClass = "HideGoalWeight";

            GoalWeightP.Text = "";
            GoalSelP.Checked = false;
            GoalWeightP.CssClass = "HideGoalWeight";

            GoalWeightD.Text = "";
            GoalSelD.Checked = false;
            GoalWeightD.CssClass = "HideGoalWeight";

            GoalWeightT.Text = "";
            GoalSelT.Checked = false;
            GoalWeightT.CssClass = "HideGoalWeight";

            for( int g = 0; g < this.currentMediaPlan.Specs.CampaignGoals.Count; g++ ){

                string goalText = "20";
                bool goalChecked = true;
                // allow for old plans with no weighting set yet
                if( this.currentMediaPlan.Specs.GoalWeights != null && this.currentMediaPlan.Specs.GoalWeights.Count > 0 ) {
                    goalText = this.currentMediaPlan.Specs.GoalWeights[ g ].ToString();
                    goalChecked = (this.currentMediaPlan.Specs.GoalWeights[ g ] > 0);
                }

                switch(  this.currentMediaPlan.Specs.CampaignGoals[ g ] ){
                    case MediaCampaignSpecs.CampaignGoal.ReachAndAwareness:
                        GoalWeightR.Text = goalText;
                        GoalSelR.Checked = goalChecked;
                        GoalWeightR.CssClass = "ShowGoalWeight";

                        break;
                    case MediaCampaignSpecs.CampaignGoal.GeoTargeting:
                        GoalWeightG.Text = goalText;
                        GoalSelG.Checked = goalChecked;
                        GoalWeightG.CssClass = "ShowGoalWeight";
                        break;
                    case MediaCampaignSpecs.CampaignGoal.Persuasion:
                        GoalWeightP.Text = goalText;
                        GoalSelP.Checked = goalChecked;
                        GoalWeightP.CssClass = "ShowGoalWeight";
                        break;
                    case MediaCampaignSpecs.CampaignGoal.DemographicTargeting:
                        GoalWeightD.Text = goalText;
                        GoalSelD.Checked = goalChecked;
                        GoalWeightD.CssClass = "ShowGoalWeight";
                        break;
                    case MediaCampaignSpecs.CampaignGoal.Recency:
                        GoalWeightT.Text = goalText;
                        GoalSelT.Checked = goalChecked;
                        GoalWeightT.CssClass = "ShowGoalWeight";
                        break;
                }
            }
        }
        PlanCost.Attributes.Add( "onKeyPress", "return AllowOnlyNumbers(event);" );
        CurrConversionBox.Attributes.Add( "onKeyPress", "return AllowOnlyNumbers(event);" );
        PrevSpendingBox.Attributes.Add( "onKeyPress", "return AllowOnlyNumbers(event);" );
        ConsiderationBox.Attributes.Add( "onKeyPress", "return AllowOnlyNumbers(event);" );
    }

    protected void Category_Changed( object sender, EventArgs e ) {
        string cat = CategoryList.SelectedItem.Text;

        if( cat != "Other" ) {
            BusinessLogic.BusinessCategories bc = new BusinessLogic.BusinessCategories();
            string[] subcats = bc.Subcategories( cat );
            if( subcats != null ) {
                SubcategoryList.CssClass = "SubcategoryList_Shown";
                SubcategoryList.Items.Clear();
                for( int i = 0; i < subcats.Length; i++ ) {
                    SubcategoryList.Items.Add( subcats[ i ] );
                }
            }
        }
        else {
            SubcategoryTextbox.CssClass = "SubcategoryTextbox_Shown";
            SubcategoryList.CssClass = "SubcategoryList_Hidden";
        }
        SetCategoryProgressImage.Style[ "visibility" ] = "hidden";
    }

    private int PriorityItemIndex( string p ) {
        int n = -1;
        for( n = 0; n < this.priorityListItemNames.Length; n++ ) {
            if( this.priorityListItemNames[ n ] == p ) {
                break;
            }
        }
        return n;
    }

    protected string RemoveSegmentLink( Guid segmentIdToRemove ) {
        string js = String.Format( "DoRemovePostback( \"{0}\" )", segmentIdToRemove.ToString() );
        return js;
    }

    protected string RemoveRegionLink( int regionToRemove ) {
        string js = String.Format( "DoRemoveRegionPostback( \"{0}\" )", regionToRemove );
        return js;
    }

    protected string EditSegmentLink( int segmentIndexToEdit ) {
        string js = String.Format( "__doPostBack( \"EditSegment\", \"{0}\" );", segmentIndexToEdit );
        return js;
    }

    protected void SaveButton_Click( object sender, EventArgs e ) {
        UpdateCurrentPlan();          // automatically saves all plans in campaign if campaign name has changed
        PlanStorage storage = new PlanStorage();
        if( this.creatingNew == false ) {
            storage.UpdateMediaCampaign( Utils.GetUser( this ), this.currentMediaPlan, this.allPlanVersions, this.currentMediaPlans );
        }
        else {
            DataLogger.Log( "CREATECAM", Utils.GetUser( this ), this.currentMediaPlan.CampaignName );
        }
        storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
        string nextUrl = String.Format( "ValidateCampaign.aspx?next={1}&c={0}", this.creatingNew, "SaveOnly" );
        Response.Redirect( nextUrl );
        //Response.Redirect( "Campaigns.aspx" );
    }

    // go to user plan entry
    protected void NextButton_Click( object sender, EventArgs e ) {
        if( this.creatingNew == true ) {
            DataLogger.Log( "CREATECAM", Utils.GetUser( this ), this.currentMediaPlan.CampaignName );
        }
        GoToPlanEditingPage( false );
    }

    // create an autogenerated plan
    protected void NextButton2_Click( object sender, EventArgs e ) {
        if( this.creatingNew == true ) {
            DataLogger.Log( "CREATECAM", Utils.GetUser( this ), this.currentMediaPlan.CampaignName );
        } 
        GoToPlanEditingPage( true );
    }

    private void GoToPlanEditingPage( bool isAutogenerated ) {
        UpdateCurrentPlan();                   // automatically saves all plans in campaign if campaign name has changed!
        string nextCode = "EditPlan";
        if( isAutogenerated == false ) {
            nextCode = "EnterPlan";
        }
        string nextUrl = String.Format( "ValidateCampaign.aspx?next={1}&c={0}", this.creatingNew, nextCode );

        PlanStorage storage = new PlanStorage();
        if( this.creatingNew == false ) {
            storage.UpdateMediaCampaign( Utils.GetUser( this ), this.currentMediaPlan, this.allPlanVersions, this.currentMediaPlans );
        }
        CreateNewPlanIfNecessary( storage );
        Server.Transfer( nextUrl );
    }

    private void CreateNewPlanIfNecessary( PlanStorage storage ) {
        if( this.creatingNew == false && this.currentMediaPlan.PlanValid == true ) {
            // we need to create a new media plan
            MediaPlan newMediaPlan = new MediaPlan( this.currentMediaPlan.CampaignName );     // create a new plan/campaign !!
            newMediaPlan.CopyCampaignDataFrom( this.currentMediaPlan.Specs, true );
            this.currentMediaPlan = newMediaPlan;
            storage.IncrementPlanVersion( this.currentMediaPlan, this.allPlanVersions, userMedia.CurrentPlanVersions );
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );
            Utils.SetAllPlanVersions( this, this.allPlanVersions );
        }
        else {
            // since we aren't creating a new plan, make sure the current plan is really empty
            this.currentMediaPlan.ClearAllMediaItems();
            Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        }
    }

    /// <summary>
    /// Updates the current plan to reflect the UI settings.
    /// </summary>
    /// <param name="page"></param>
    private void UpdateCurrentPlan() {

        this.currentMediaPlan.IsUserPlan = true;

        this.currentMediaPlan.Specs.TargetBudget = Convert.ToDouble( PlanCost.Text ) * 1000;
        this.currentMediaPlan.TargetBudget = this.currentMediaPlan.Specs.TargetBudget;

        this.currentMediaPlan.Specs.StartDate = Convert.ToDateTime( StartDateTextBox.Text );
        int planWeeks = Convert.ToInt32( DurationTextBox.Text );
        this.currentMediaPlan.Specs.EndDate = this.currentMediaPlan.Specs.StartDate.AddDays( planWeeks * 7 );

        TimeSpan planDuration = this.currentMediaPlan.Specs.EndDate - this.currentMediaPlan.Specs.StartDate;
        double planMonths = planDuration.TotalDays / 30.0;
        this.currentMediaPlan.Specs.MonthlySpendRate = this.currentMediaPlan.Specs.TargetBudget / planMonths;

        double totTatgetSize = -1;
        MediaCampaignSpecs.SegmentSize[] targetSizes = this.currentMediaPlan.PopulationPercentTargeted( out totTatgetSize );

        if( this.CampaignNameTextBox.Text != null && this.CampaignNameTextBox.Text.Trim() != "" ) {

            if( this.currentMediaPlan.Specs.CampaignName != this.CampaignNameTextBox.Text.Trim() ) {
                // the name is changing -- this forces the data to be saved to disk immediately
                string oldName =  this.currentMediaPlan.Specs.CampaignName;
                string newName = this.CampaignNameTextBox.Text.Trim();

                bool isOnlyVersion = (this.allPlanVersions.ContainsKey( oldName ) == false || this.allPlanVersions[ oldName ].Count == 1 );
                this.currentMediaPlan.Specs.CampaignName = newName;
                if( isOnlyVersion == false ) {
                    PlanStorage storage = new PlanStorage();
                    storage.RenameMediaCampaign( Utils.GetUser( this ), oldName, newName, this.allPlanVersions, this.currentMediaPlans );
                    storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
                }
            }
        }

        this.currentMediaPlan.Specs.PurchaseCycleCode = Convert.ToInt32( purchaseCycleTextBox.Text );
        this.currentMediaPlan.Specs.TimeInConsideration = (int)Math.Max( 1, Math.Min( 100,  Convert.ToInt32( ConsiderationBox.Text ) ) );
        this.currentMediaPlan.Specs.BusinessCategory = CategoryList.SelectedItem.Text;
        this.currentMediaPlan.Specs.BusinessSubcategory = SubcategoryList.SelectedItem.Text;
        if( this.currentMediaPlan.Specs.BusinessCategory == "Other" ) {
            this.currentMediaPlan.Specs.BusinessSubcategory = SubcategoryTextbox.Text.Trim();
        }

        this.currentMediaPlan.Specs.PreviousMediaSpend = Convert.ToDouble( PrevSpendingBox.Text ) * 1000;
        this.currentMediaPlan.Specs.InitialShare = Convert.ToDouble( CurrConversionBox.Text );
        this.currentMediaPlan.Specs.ShareUnits = ConversionUnits.SelectedValue;

        double[] goalWeights = new double[ 5 ];
        MediaCampaignSpecs.CampaignGoal[] goalItems = new MediaCampaignSpecs.CampaignGoal[ 5 ];

        goalItems[ 0 ] = MediaCampaignSpecs.CampaignGoal.ReachAndAwareness;
        goalWeights[ 0 ] = 0.0;
        if( GoalSelR.Checked )
        {
            try
            {
                goalWeights[0] = double.Parse( GoalWeightR.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }

        goalItems[ 1 ] = MediaCampaignSpecs.CampaignGoal.GeoTargeting;
        goalWeights[1] = 0.0;
        if( GoalSelG.Checked )
        {
            try
            {
                goalWeights[1] = double.Parse( GoalWeightG.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }

      
        goalItems[ 2 ] = MediaCampaignSpecs.CampaignGoal.Persuasion;
        goalWeights[2] = 0.0;
        if( GoalSelP.Checked )
        {
            try
            {
                goalWeights[2] = double.Parse( GoalWeightP.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }

        goalItems[ 3 ] = MediaCampaignSpecs.CampaignGoal.DemographicTargeting;
        goalWeights[3] = 0.0;
        if( GoalSelD.Checked )
        {
            try
            {
                goalWeights[3] = double.Parse( GoalWeightD.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }

       
        goalItems[ 4 ] = MediaCampaignSpecs.CampaignGoal.Recency;
        goalWeights[4] = 0.0;
        if( GoalSelT.Checked )
        {
            try
            {
                goalWeights[4] = double.Parse( GoalWeightT.Text.Trim() );
            }
            catch( Exception )
            {
            }
        }

        Array.Sort( goalWeights, goalItems );
        Array.Reverse( goalWeights );
        Array.Reverse( goalItems );

        this.currentMediaPlan.Specs.CampaignGoals = new List<MediaCampaignSpecs.CampaignGoal>();
        this.currentMediaPlan.Specs.GoalWeights = new List<double>();
        this.currentMediaPlan.Goals = new List<MediaPlan.PlanGoal>();

        bool useAllGoals = true;
        for( int i = 0; i < 5; i++ ) {
            if( goalWeights[ i ] > 0 || useAllGoals )
            {
                this.currentMediaPlan.Specs.CampaignGoals.Add( goalItems[i] );
                this.currentMediaPlan.Specs.GoalWeights.Add( goalWeights[i] );
                this.currentMediaPlan.Goals.Add( new MediaPlan.PlanGoal( goalItems[ i ], goalWeights[ i ] ) );
            }
        }

        List<string> curGeos = new List<string>();
        foreach( string rgnName in this.currentMediaPlan.Specs.GeoRegionNames ){
            curGeos.Add( rgnName );
        }

        if( Utils.UserIsDemo( this ) == false ) {
            this.userMedia.InitialGeoRegionChoices = curGeos;
            // save the profile data to the session...
            Utils.SetCurrentUserProfile( this, this.userMedia );
            //...and also to the Profiile itself
            Profile.UserMediaItems = this.userMedia;
        }

        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
        Utils.SetCurrentMediaPlans( this, this.currentMediaPlans );                 // needed if the campaign was renamed

        //ISN 11/14/2008 need to get population size here...
        this.currentMediaPlan.PopulationSize = SimUtils.GetPopulationSize(this.currentMediaPlan);

    }

    private double GetGoalWeight( TextBox textBox ) {
        double w = 0;
        try {
            w = double.Parse( textBox.Text );
        }
        catch( Exception ) {
        }
        return w;
    }

    private MediaCampaignSpecs.CampaignGoal GoalForValue( string goalValueCode ) {
        if( goalValueCode == "G" ) {
            return MediaCampaignSpecs.CampaignGoal.GeoTargeting;
        }
        else if( goalValueCode == "RA" ) {
            return MediaCampaignSpecs.CampaignGoal.ReachAndAwareness;
        }
        else if( goalValueCode == "P" ) {
            return MediaCampaignSpecs.CampaignGoal.Persuasion;
        }
        else if( goalValueCode == "D" ) {
            return MediaCampaignSpecs.CampaignGoal.DemographicTargeting;
        }
        else if( goalValueCode == "T" ) {
            return MediaCampaignSpecs.CampaignGoal.Recency;
        }
        else {
            return MediaCampaignSpecs.CampaignGoal.ReachAndAwareness;   //default - should never be used but we cannot return null
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

        //string helpEmail = String.Format( "<a href=\"mailto:support@adplanit.com?subject=AdPlanit Support Request - {0}\">Question? Send us a message!</a>", "Campaign Settings" );
        //HelpEmailLink.Text = helpEmail;
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
