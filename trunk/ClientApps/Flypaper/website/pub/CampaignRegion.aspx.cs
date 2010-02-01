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
using GeoLibrary;

public partial class CampaignRegion : System.Web.UI.Page
{

    protected string segmentBeingAdded;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;
    protected bool engineeringMode;
    protected UserMedia userMedia;

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        InitializeVariables();

        RegionsList.Attributes.Add( "onChange", "desired.center = new GLatLng(36, -122); AnimateMap()" );

       
     

        if( IsPostBack == false )
        {
            // initiaize regions menu
            List<string> states = Utils.AllUSAStates();
            foreach( string state in states )
            {
                ListItem rgnItem = new ListItem( state );
                RegionsList.Items.Add( rgnItem );
            }

            // initiaize subregions menu
            RegionsList_SelectedIndexChanged( null, null );
        }
    }

    protected void AddRegion_Click( object sender, EventArgs e ) {

        bool dmaSelect = true;
        List<string> countyNames = new List<string>();

        if( UseCounties.Checked )
        {
            // gather up counties
            foreach(ListItem item in CountyList.Items)
            {
                if( item.Selected )
                {
                    countyNames.Add( item.Value );
                }
                else
                {
                    dmaSelect = false;
                }
            }

        }

        if( dmaSelect )
        {
            string subRgnName = this.SubregionsList.SelectedItem.Text;
            if( this.currentMediaPlan.Specs.GeoRegionNames.Contains( subRgnName ) == false )
            {
                this.currentMediaPlan.Specs.GeoRegionNames.Add( subRgnName );
                Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
            }
        }
        else
        {
            foreach( string county in countyNames )
            {
                if( this.currentMediaPlan.Specs.GeoRegionNames.Contains( county ) == false )
                {
                    this.currentMediaPlan.Specs.GeoRegionNames.Add( county );
                }
            }
        }

        Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );

        Response.Redirect( "Campaign.aspx" );
    }

    /// <summary>
    /// Handles a change to the region selection by refilling the subregions menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RegionsList_SelectedIndexChanged( object sender, EventArgs e ) {

        string state = RegionsList.SelectedValue;

        if( state != null )
        {
            List<string> subRegions = Utils.StateToDma()[state];

            SubregionsList.Items.Clear();
            for( int i = 0; i < subRegions.Count; i++ )
            {
                SubregionsList.Items.Add( new ListItem( subRegions[i] ) );
            }
        }

        SubregionsList_SelectedIndexChanged( null, null );

       // SetStateProgressImage.Style[ "visibility" ] = "hidden";
    }

    /// <summary>
    /// Handles a change to the county selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SubregionsList_SelectedIndexChanged( object sender, EventArgs e ) {
        string dma = SubregionsList.SelectedValue;

        if( dma != null )
        {
            GeoLibrary.GeoRegion reg = GeoLibrary.GeoRegion.TopGeo.GetSubRegion( dma );

            List<GeoRegion> subRegions = reg.SubRegions;

            CountyList.Items.Clear();

            for( int i = 0; i < subRegions.Count; i++ )
            {
                string county = subRegions[i].Name.Split(',')[0];

                ListItem item = new ListItem( county, subRegions[i].Name );

                item.Selected = true;

                CountyList.Items.Add( item );
            }
        }

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
    protected void UseCounties_CheckedChanged( object sender, EventArgs e )
    {
        if( UseCounties.Checked )
        {
            CountyList.Enabled = true;
        }
        else
        {
            CountyList.Enabled = false;
        }
    }
}

