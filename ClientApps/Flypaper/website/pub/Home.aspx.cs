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
using System.Reflection;

using WebLibrary;
using BusinessLogic;

public partial class About : System.Web.UI.Page
{

    protected void Page_Load( object sender, EventArgs e ) {
        if( ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] != null )
        {
            SettingsProvider prof = Profile.Providers["SqlProvider"];
            System.Reflection.FieldInfo connectionStringField = typeof( System.Web.Profile.SqlProfileProvider ).GetField( "_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic );
            connectionStringField.SetValue( prof, ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] );
        }

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "Logout" ){
            LogoutUser();
        }

        Demo.Visible = false;
        if( this.User == null || this.User.Identity == null || this.User.Identity.Name == null || this.User.Identity.Name == "" )
        {
            RoleProvider rol = Roles.Providers["SqlProvider"];
            if( rol.IsUserInRole( "guest@adplanit.com", "visitor" ) )
            {
                Demo.Visible = true;
            }
        }
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    protected void LogoutUser() {
        Session.Abandon();
        FormsAuthentication.SignOut();
        Response.Redirect( "Home.aspx" );     // force a reload
    }
    protected void Demo_Click( object sender, EventArgs e )
    {
        FormsAuthentication.SetAuthCookie( "guest@adplanit.com", true );
        Response.Redirect( "Campaigns.aspx" ); 

    }
}
