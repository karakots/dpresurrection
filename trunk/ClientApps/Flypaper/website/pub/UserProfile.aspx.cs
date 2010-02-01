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

public partial class UserProfile : System.Web.UI.Page
{

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        UserMedia userInfo = Profile.UserMediaItems;

        if( IsPostBack && Request[ "__EVENTTARGET" ] == "SaveProfile" ) {
            userInfo.FirstName = this.first_name.Value.Trim();
            userInfo.LastName = this.last_name.Value.Trim();
            userInfo.CompanyName = this.company.Value.Trim();
            userInfo.Phone = this.phone.Value.Trim();
            userInfo.CompanyURL = this.URL.Value.Trim();

            int dum = 9;
        }

        this.first_name.Value = userInfo.FirstName;
        this.last_name.Value = userInfo.LastName;
        this.company.Value = userInfo.CompanyName;
        this.phone.Value = userInfo.Phone;
        this.URL.Value = userInfo.CompanyURL;

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
