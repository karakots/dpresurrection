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

public partial class Index2 : System.Web.UI.Page
{
    protected string destURL = "http://localhost:1889/pub/confirm.aspx";

    protected void Page_Load( object sender, EventArgs e ) {
        if( Request[ "first_name" ] != null ) {
            first_name.Value = Request[ "first_name" ];
        }
        if( Request[ "last_name" ] != null ) {
            last_name.Value = Request[ "last_name" ];
        }
        if( Request[ "email" ] != null ) {
            email.Value = Request[ "email" ];
        }
        if( Request[ "company" ] != null ) {
            company.Value = Request[ "company" ];
        }
        if( Request[ "phone" ] != null ) {
            phone.Value = Request[ "phone" ];
        }
        if( Request[ "source" ] != null ) {
            source.Value = Request[ "source" ];
        }
        if( Request[ "source2" ] != null ) {
            source2.Value = Request[ "source2" ];
        }

        if( Request[ "err" ] == "true" ){
            ErrorLabel.InnerHtml = "You must provide values for all fields marked with an asterisk (*)";
        }

       // string loginPageURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SiteEntryPage" ];

        LoginPageLink.HRef = @"../Login.aspx";
    }

    protected string GetProcessUrl() {
        // string processSignupURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SignupProcessPage" ];
        return @"../Confirm.aspx";
    }

    protected void OkButton_Clicked( object sender, EventArgs e ) {
        bool allOK = true;
        if( first_name.Value.Trim() == "" || last_name.Value.Trim() == "" || email.Value.Trim() == "" ) {
            allOK = false;
        }

        ErrorLabel.InnerHtml = "You must provide values for all fields marked with an asterisk (*)";

        if( allOK ) {
            // check the format of the email address
            string em = email.Value.Trim();

            int atIndx = em.IndexOf( "@" );
            int dotIndx = em.LastIndexOf( "." );
            if( atIndx == -1 || dotIndx == -1 || dotIndx < atIndx ) {
                ErrorLabel.InnerHtml = "Email address is not valid.";
                allOK = false;
            }
        }

        if( allOK == true ){

            //SendEmail();

            //Response.Redirect( destURL );
        }
        else {
            ErrorLabel.Style[ "visibility" ] = "visible";
        }
    }
}
