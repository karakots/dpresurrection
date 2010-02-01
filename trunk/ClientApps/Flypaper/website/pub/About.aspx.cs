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

public partial class About : System.Web.UI.Page
{

    protected void Page_Load( object sender, EventArgs e ) {
        if( IsPostBack && Request[ "__EVENTTARGET" ] == "Logout" ){
            LogoutUser();
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
}
