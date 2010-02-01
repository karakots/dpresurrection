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

public partial class SetPassword : System.Web.UI.Page
{
    protected string lostPasswordEmailTitle = "Your AdPlanit User Information";
    protected string lostPasswordEmailBody = "Hello, {0},\r\n\r\nHere is your AdPlanit user account information:\r\n\r\n" +
        "User: {1}\r\n\r\n" +
        "Password: {2}\r\n\r\n" +
        "Thank you for using AdPlanit.\r\n\r\n --- AdPlanit Automated Services";

    protected void Page_Load( object sender, EventArgs e ) {
    }

    protected void GoButton_Click( object sender, EventArgs e ) {
        string userID = EmailTextbox.Text.Trim();

        MembershipUser mu = Membership.GetUser( userID );
        string userPassword = mu.GetPassword();
        string uName = mu.UserName;

        string firstName = "";
        try {      // just in case (this part isn't essential to getting the pasword)
            ProfileCommon pc = Profile.GetProfile( userID );
            if( pc != null && pc.UserMediaItems != null ) {
                firstName = pc.UserMediaItems.FirstName;
            }
        }
        catch( Exception ) {
        }

        string emailBody = String.Format( lostPasswordEmailBody, firstName, uName, userPassword );
        EmailSender emailSender = new EmailSender();
        emailSender.SendMessage( uName, lostPasswordEmailTitle, emailBody );
        SentLabel.Visible = true;
    }
}
