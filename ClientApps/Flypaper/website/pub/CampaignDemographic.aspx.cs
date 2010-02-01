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

public partial class CampaignDemographics : System.Web.UI.Page
{
    int demoIndex = -1;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        if( IsPostBack == false )
        {
            if( demoIndex <0 )
            {
                SetDefaultSegmentSettings();
            }
            else
            {
                // we ar editing a segment
                try
                {
                   DemographicSettings demo = currentMediaPlan.Specs.Demographics[demoIndex];
                   SetCurrentSegmentSettings( demo );
                }
                catch( Exception )
                {
                    // guess we are not editing a segment
                    demoIndex = -1;
                    SetDefaultSegmentSettings();
                  
                }
            }
        }
    }

    /// <summary>
    /// Gets the next available default segment name.
    /// </summary>
    /// <returns></returns>
    private string GetNextSegmentName() {
        int sn = 1;
        string nextSegName = "";
        do {
            nextSegName = String.Format( "Segment {0}", sn );
            sn += 1;
        }
        while( this.currentMediaPlan.ContainsSegment( nextSegName ) == true );

        return nextSegName;
    }


            // - - - - -  METHODS NEEDED FOR POPUP DEMO DIALOG - - - - 


    /// <summary>
    /// Called when the user has clicked OK to add a demographic segment
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AddSegment_Click( object sender, EventArgs e ) {

        DemographicSettings curDemoSet = GetCurrentSegmentSettings();

        if( demoIndex < 0 )
        {
            this.currentMediaPlan.Specs.Demographics.Add( curDemoSet );
        }
        else
        {
            this.currentMediaPlan.Specs.Demographics[demoIndex] = curDemoSet;
        }

        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );

        Response.Redirect( "Campaign.aspx" );
    }

    protected void SetDefaultSegmentSettings()
    {
        SegmentName.Value = GetNextSegmentName();
    }

    // sets the current setting based on a given demographic
    protected void SetCurrentSegmentSettings(DemographicSettings demo)
    {
        SegmentName.Value = demo.DemographicName.Trim();

        foreach( DemographicGroupValues grpVal in demo.Values )
        {

            switch( grpVal.GroupName )
            {
                case "Gender":
                    if( AllValuesTrue( grpVal.Values, 2 ) ) {
                        break;
                    } 
                    GenderAny.Checked = false;
                    GenderMale.Disabled = false;
                    GenderFemale.Disabled = false;

                    GenderMale.Checked = grpVal.Values[0];
                    GenderFemale.Checked = grpVal.Values[1];
                    break;
                case "Age":
                    if( AllValuesTrue( grpVal.Values, 7 ) ) {
                        break;
                    } 
                    AgeAny.Checked = false;
                    Age1.Disabled = false;
                    Age2.Disabled = false;
                    Age3.Disabled = false;
                    Age4.Disabled = false;
                    Age5.Disabled = false;
                    Age6.Disabled = false;
                    Age7.Disabled = false;

                    Age1.Checked = grpVal.Values[0];
                    Age2.Checked = grpVal.Values[1];
                    Age3.Checked = grpVal.Values[2];
                    Age4.Checked = grpVal.Values[3];
                    Age5.Checked = grpVal.Values[4];
                    Age6.Checked = grpVal.Values[5];
                    Age7.Checked = grpVal.Values[6];

                    break;
                case "Income":
                    if( AllValuesTrue( grpVal.Values, 5 ) ){
                        break;
                    }
                    IncomeAny.Checked = false;
                    Income1.Disabled = false;
                    Income2.Disabled = false;
                    Income3.Disabled = false;
                    Income4.Disabled = false;
                    Income5.Disabled = false;

                    Income1.Checked = grpVal.Values[0];
                    Income2.Checked = grpVal.Values[1];
                    Income3.Checked = grpVal.Values[2];
                    Income4.Checked = grpVal.Values[3];
                    Income5.Checked = grpVal.Values[4];
                    break;
                case "Kids":
                     if( AllValuesTrue( grpVal.Values, 2 ) ){
                        break;
                    }                   KidsAny.Checked = false;
                    Kids0.Disabled = false;
                    Kids1.Disabled = false;

                    Kids0.Checked = grpVal.Values[0];
                    Kids1.Checked = grpVal.Values[1];
                    break;
                case "Race":
                    if( AllValuesTrue( grpVal.Values, 4 ) ){
                        break;
                    }
                    RaceAny.Checked = false;
                    Race1.Disabled = false;
                    Race2.Disabled = false;
                    Race3.Disabled = false;
                    Race4.Disabled = false;

                    Race1.Checked = grpVal.Values[0];
                    Race2.Checked = grpVal.Values[1];
                    Race3.Checked = grpVal.Values[2];
                    Race4.Checked = grpVal.Values[3];
                    break;
            }
        }
    }

    private bool AllValuesTrue( bool[] values, int nValues ) {
        bool allTrue = true;
        for( int i = 0; i < nValues; i++ ) {
            if( values[ i ] == false ) {
                allTrue = false;
                break;
            }
        }
        return allTrue;
    }

    /// <summary>
    /// Gets the current demographics segment settings from the UI and converts them into a DemographicSettings object
    /// </summary>
    protected DemographicSettings GetCurrentSegmentSettings() {
        // create demographic group data for the current settings
        DemographicGroupValues gender = new DemographicGroupValues( "Gender" );
        DemographicGroupValues age = new DemographicGroupValues( "Age" );
        DemographicGroupValues income = new DemographicGroupValues( "Income" );
        DemographicGroupValues kids = new DemographicGroupValues( "Kids" );
        DemographicGroupValues race = new DemographicGroupValues( "Race" );

        gender.Values = new bool[ 2 ] { GenderMale.Checked, GenderFemale.Checked };
        gender.ValueNames = new string[ 2 ] { "Male", "Female" };

        age.Values = new bool[ 7 ] { Age1.Checked, Age2.Checked, Age3.Checked, Age4.Checked, Age5.Checked, Age6.Checked, Age7.Checked };
        age.ValueNames = new string[ 7 ] { "Under 18", "18-25", "26-35", "36-45", "46-55", "56-65", "Over 65" };

        income.Values = new bool[ 5 ] { Income1.Checked, Income2.Checked, Income3.Checked, Income4.Checked, Income5.Checked };
        income.ValueNames = new string[ 5 ] { "Under 50K", "50K-75K", "75K-100K", "100K-150K", "Over 150K" };

        kids.Values = new bool[ 2 ] { Kids0.Checked, Kids1.Checked };
        kids.ValueNames = new string[ 2 ] { "None", "One or more" };

        race.Values = new bool[ 4 ] { Race1.Checked, Race2.Checked, Race3.Checked, Race4.Checked };
        race.ValueNames = new string[ 4 ] { "Asian", "African American", "Hispanic", "Other" };

        // if no checkboxes are checked in a group, this realiy means to target all subsegments
        MakeAllFalseBeAllTrue( gender.Values );
        MakeAllFalseBeAllTrue( age.Values );
        MakeAllFalseBeAllTrue( income.Values );
        MakeAllFalseBeAllTrue( kids.Values );
        MakeAllFalseBeAllTrue( race.Values );

        // add the demographic data to the media plan
        DemographicSettings demographicsSet = new DemographicSettings();
        demographicsSet.DemographicName = SegmentName.Value;

        demographicsSet.Values.Add( gender );
        demographicsSet.Values.Add( age );
        demographicsSet.Values.Add( income );
        demographicsSet.Values.Add( kids );
        demographicsSet.Values.Add( race );

        return demographicsSet;
    }

    /// <summary>
    /// Leaves the array unchanged unless all values are false -- in that case it sets all values to true.
    /// </summary>
    /// <param name="values"></param>
    private void MakeAllFalseBeAllTrue( bool[] values ) {
        bool isATrue = false;
        foreach( bool v in values ) {
            if( v == true ) {
                isATrue = true;
            }
        }
        if( isATrue == false ) {
            // no true values were found -- set all to true
            for( int i = 0; i < values.Length; i++ ) {
                values[ i ] = true;
            }
        }
    }

    /// <summary>
    /// Clears all checkboxes in the demographic popup
    /// </summary>
    private void ResetDemographicCheckboxes() {
        HtmlInputCheckBox[] allDemoCheckboxes = new HtmlInputCheckBox[]{
            GenderMale,
            GenderFemale,
            Age1,
            Age2,
            Age3,
            Age4,
            Age5,
            Age6,
            Age7,
            Income1,
            Income2,
            Income3,
            Income4,
            Income5,
            Kids0,
            Kids1,
            Race1,
            Race2,
            Race3,
            Race4
        };

        foreach( HtmlInputCheckBox checkbox in allDemoCheckboxes ) {
            checkbox.Checked = false;
            checkbox.Disabled = false;
        }
    }


    /// <summary>
    /// Loads the values from the session and the user profile.
    /// </summary>
    private void InitializeVariables() {

        if( Request["DemographicIndex"] != null )
        {
            // we ar editing a segment
            try
            {
               this.demoIndex = Int32.Parse( Request["DemographicIndex"] );
            }
            catch( Exception )
            { }
        }

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

