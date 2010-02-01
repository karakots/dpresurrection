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

public partial class ReceivePlan : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e ) {

        if( IsPostBack == false ) {
            SetHiddenFields();
        }

        if( Utils.UserIsDemo( this ) == false ) {
            Response.Redirect( "~/ReceivePlanEntry.aspx" );
        }
    }

    private void SetHiddenFields() {
        string uname = Request[ "u" ];
        if( uname != null ) {
            Session[ "ReceivedPlanUser" ] = uname.Replace( "--", "@" ).Replace( "-", "." );
            Session[ "ReceivedPlanID" ] = Request[ "p" ];
        }
    }
}
