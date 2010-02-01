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
using BusinessLogic;

public partial class AddUser : System.Web.UI.Page
{
    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> allMediaPlans;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;

    protected void Page_Load( object sender, EventArgs e )
    {
        Utils.LoadValuesFromSession( this, out this.currentMediaPlan, out this.allMediaPlans, out this.runningPlans );
        //engineeringMode = Utils.InEngineeringMode( this, this.PhoneLabel );
        if( IsPostBack == false ) {
            StoryGenerator.GetStoryGenerator( this ).AddElement( new StoryGenerator.PageEventElement( "Create New User Login" ) );
        }
                
        // check for postback from tabs
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "ListTabLink" ) {
            Response.Redirect( "PlansList.aspx" );
        }
        else if( IsPostBack && Request[ "__EVENTTARGET" ] == "NewCampaignTab" ) {
            Response.Redirect( "NewPlan.aspx" );
        } 
        
        if( IsPostBack ) {
            // copy the user-name value (which is really their email address) into the email-setting value so the users DB doesn't complain about missing/duplicate email (setting EmailRequired=false doesn't seem to do the trick)
            CreateUserWizard1.Email = Request[ "CreateUserWizard1$CreateUserStepContainer$UserName" ];
        }
    }

    protected void HomeButton_Click( object sender, EventArgs e ) {
        Response.Redirect( "~/Home.aspx" );
    }
}
