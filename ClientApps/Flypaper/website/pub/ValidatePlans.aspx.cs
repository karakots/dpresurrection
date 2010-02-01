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

using BusinessLogic;
using WebLibrary;

/// <summary>
/// This is the pass-thru error-check page for media plans.  If all of the plans being edited are valid, the page specified by the "next"
/// argument is loaded.  Otherwise the error details are shown with an OK button to return to the plan edit page.
/// </summary>
public partial class ValidatePlans : System.Web.UI.Page
{
    private string callingPage;

    protected void Page_Load( object sender, EventArgs e ) {

        this.UsingPage.Value = Request[ "this" ];
        this.callingPage = Request[ "UsingPage" ] + ".aspx";

        GoToNext();
    }

    /// <summary>
    /// Returns the user to the page that requested the validation (validation failed)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OkButton_Click( object sender, EventArgs e ) {
        Response.Redirect( this.callingPage );
    }

    /// <summary>
    /// Transfers control to the next page (validation suceeded)
    /// </summary>
    private void GoToNext() {
        string targetPage = Request[ "next" ];
        if( targetPage != null && targetPage != "" ) {
            targetPage += ".aspx";
            Response.Redirect( targetPage );
        }
    }
}
