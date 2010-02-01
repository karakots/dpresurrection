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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e ) {
        // redirect everybody to the Home page
        Response.Redirect( "Home.aspx" );

        if( Request[ "u" ] != null ) {
            Login1.UserName = Request[ "u" ];
        }
        string editDataURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SignupDataEntryPage" ];
        NewUserLink.HRef = editDataURL;
    }

    protected void NewUserButton_Click( object sender, EventArgs e ) {
        Server.Transfer( "AddUser.aspx" );
    }
}
