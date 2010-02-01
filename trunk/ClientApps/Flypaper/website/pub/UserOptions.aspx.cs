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

public partial class UserOptions : System.Web.UI.Page
{
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> allMediaPlans;
    protected List<Guid> runningPlans;

    protected void Page_Load( object sender, EventArgs e ) {
        // get the session-level variables
        Utils.LoadValuesFromSession( this, out this.currentMediaPlan, out this.allMediaPlans, out this.runningPlans );

        // check for postback from tabs
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ListTabLink" ) {
            Response.Redirect( "PlansList.aspx" );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "NewCampaignTab" ) {
            Response.Redirect( "NewPlan.aspx" );
        }

        if( Utils.UserIsDemo( this ) == false ) {
            UserMedia userMedia = Profile.UserMediaItems;
            StartInEngineeringModeBox.Visible = true;

            this.BodyInfo.Text = "Plans List Sorting: " + userMedia.plansListSortColumn;

            //
            // I am commenting user media items out...  for now
            // SSN 9/6/2008
            //
            //if( userMedia.MediaVehicleSpecs != null ) {
            //    this.BodyInfo.Text += "<br><br>User Media Items: " + userMedia.MediaVehicleSpecs.Count;
            //}
            //else {
            //    this.BodyInfo.Text += "<br><br>User Media Items: null";
            //}

            //if( userMedia.MediaSpotSpecs != null ) {
            //    this.BodyInfo.Text += "<br><br>User Media Prominences: " + userMedia.MediaSpotSpecs.Count;
            //}
            //else {
            //    this.BodyInfo.Text += "<br><br>User Media Prominences: null";
            //}

            //if( userMedia.MediaPackageSpecs != null ) {
            //    this.BodyInfo.Text += "<br><br>User Media Packages: " + userMedia.MediaPackageSpecs.Count;
            //}
            //else {
            //    this.BodyInfo.Text += "<br><br>User Media Packages: null";
            //}

            if( IsPostBack == false ) {
                StartInEngineeringModeBox.Checked = userMedia.StartInEngineeringMode;
            }
        }
        else {
            // not logged in
            this.BodyInfo.Text = "Not Logged in:  User options not available to anonymous users";
            StartInEngineeringModeBox.Visible = false;
        }


        if( IsPostBack == false ) {
            this.ReturnPage.Value = "";
            if( Request.UrlReferrer != null ) {
                this.ReturnPage.Value = Request.UrlReferrer.ToString();
            }
        }
        else {
            this.ReturnPage.Value = Request[ "ReturnPage" ];
        }
    }

    protected void AutoEng_Changed( object sender, EventArgs e ) {
        UserMedia userMedia = Profile.UserMediaItems;
        userMedia.StartInEngineeringMode = StartInEngineeringModeBox.Checked;
        Profile.UserMediaItems = userMedia;
    }

    protected void OkButton_Click( object sender, EventArgs e ) {
        ReturnToReferringPage();
    }

    protected void CancelButton_Click( object sender, EventArgs e ) {
        ReturnToReferringPage();
    }

    private void ReturnToReferringPage() {
        if( Request[ "ReturnPage" ] != null ) {
            Response.Redirect( Request[ "ReturnPage" ] );
        }
    }

}
