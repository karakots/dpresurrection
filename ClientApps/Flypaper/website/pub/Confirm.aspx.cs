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

public partial class Confirm : System.Web.UI.Page
{
    private string signupComfirmationEmailSubject = "AdPlanit Login Information";

    private string signupConfirmationEmailTemplate = "Dear {0},\r\n\r\n" +
        "Thanks for signing up to use AdPlanit - the ultimate planning tool and test market for your ad campaign!\r\n\r\n" +
        "Create an advertising campaign using the best mix of advertising media to target your customers. Use a virtual population" +
        " of your customers to test your plan.  Find ways to improve your plan within your budget and goals.\r\n\r\n" +
        "AdPlanit is in early stage release to obtain feedback from select users like you.  We appreciate your help and are here to help you through " +
        "any questions or issues you might have.  There is a form on the Support page for your feedback, or please feel free to contact us by email or phone (800) 518-9202.\r\n\r\n" +
        "Before implementing your plan, let us review it with you to make sure AdPlanit is doing the best possible job for you.  Here’s the information you need to access the site:\r\n\r\n" +
        "        Your user name: {1}\r\n\r\n" +
        "        Your password: {2}\r\n" +
        //"        AdPlanit beta site: {3}?u={4}" +
        "\r\n\r\nThanks again,\r\nThe AdPlanit Team";

    private string[] ourEmails = new string[] {
        // "ddubbe@decisionpower.com",
        "jjanssen@decisionpower.com",
        // "dchalmers@decisionpower.com",
    };

    private string advisoryEmailTitle = "AdPlanit Website Signup Confirmation";

    private string advisoryEmailTemplate = "Greetings,\r\n" +
    "There was an AdPlanit Website Signup.\r\n\r\n" +
    "{0}\r\n" +
    "{1}\r\n" +
    "{2}\r\n" +
    "{3}\r\n" +
    "{4}\r\n" +
    "{5}\r\n" +
    "{6}\r\n" +
    "{7}\r\n" +
    "{8}\r\n" +
    "{9}\r\n\r\n" +
    " -- AdPlanit Automated Services for www.AdPlanit.com";

    protected void Page_Load( object sender, EventArgs e ) {
        DoSignup.Visible = false;
        EditData.Visible = false;

        // string editDataURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SignupDataEntryPage" ];

        EditData.PostBackUrl = @"signup\index.aspx";

        // preserve the user-entered values
        first_name.Value = Request[ "first_name" ];
        last_name.Value = Request[ "last_name" ];
        company.Value = Request[ "company" ];
        phone.Value = Request[ "phone" ];
        email.Value = Request[ "email" ];
        source.Value = Request[ "source" ];
        source2.Value = Request[ "source2" ];
        
        if( IsPostBack == false && Request.UrlReferrer != null && Request.UrlReferrer.ToString().ToLower().IndexOf( "index.aspx" ) != -1 ) {
            // fire the user-signup process
            DisplayFormData();
            DoSignup.Visible = true;
            EditData.Visible = true;

        }
        else {
            InfoDiv.InnerHtml = "Unknown State!";
        }
    }

    protected void OkButton_Click( object sender, EventArgs e ) {
        

        // the user is ready to sign up
        UserSignup signupObj = new UserSignup( this.Request );

        string uName = null;
        string password = null;
        string signupErr = signupObj.DoSignup( out uName, out password );
        if( signupErr == null ) {
            // signup was successful
            ProfileCommon userProf = Profile.GetProfile( uName );
            userProf.UserMediaItems.FirstName = Request[ "first_name" ];
            userProf.UserMediaItems.LastName = Request[ "last_name" ];
            userProf.UserMediaItems.CompanyName = Request[ "company" ];
            userProf.UserMediaItems.CompanyURL = Request[ "source" ];
            userProf.UserMediaItems.Phone = Request[ "phone" ];
            userProf.UserMediaItems.Referralnfo = Request[ "source2" ];
            userProf.Save();

            // send the email to the user who signed up
            //string entryURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SiteEntryPage" ];
            string emailBody = String.Format( signupConfirmationEmailTemplate, Request[ "first_name" ], uName, password );
            EmailSender emailSender = new EmailSender();
            string sendError = emailSender.SendMessage( uName, signupComfirmationEmailSubject, emailBody );

            if( sendError != null )
            {
                // send an email to us with all of the info
                string usBody = String.Format( advisoryEmailTemplate,
                        DateTime.Now.ToString( "d/M/yyyy" ),
                        DateTime.Now.ToString( "h:mm:sstt" ),
                        Request["first_name"],
                        Request["last_name"],
                        Request["email"],
                        Request["company"],
                        Request["phone"],
                        Request["source"],
                        Request["source2"],
                        password );
                EmailSender emailSender2 = new EmailSender();
                List<string> ourEmailsList = new List<string>();
                string ourEmailsFromConfig = ConfigurationSettings.AppSettings["AdPlanIt.SignupNotificationRecipients"];
                string[] ourEmails = ourEmailsFromConfig.Split( ',' );
                foreach( string uname in ourEmails )
                {
                    ourEmailsList.Add( uname );
                }
                emailSender2.SendMessage( ourEmailsList, advisoryEmailTitle, usBody );

                // log the user's signup
                string successLogMessage = String.Format( "password:{1} first:{2} last:{3} co:{4} ph:{5} web:{6} src:{7}",
                    uName, password, Request["first_name"],
                        Request["last_name"],
                        Request["company"],
                        Request["phone"],
                        Request["source"],
                        Request["source2"] );
                WebLibrary.DataLogger.Log( "SIGNUP", uName, successLogMessage );

                string successMessage = String.Format( "Thank you - your signup was successful!<br><br>  You will receive an email at <b>{0}</b> with your password and AdPlanit site entry information.", uName );
                InfoDiv.InnerHtml = successMessage;
            }
            else
            {
                InfoDiv.InnerHtml = "Error: " + sendError;
            }
        }
        else {
            // there was a problem with the signup process
            InfoDiv.InnerHtml = "Error: " + signupErr;
        }
    }

    //protected void EditButton_Click( object sender, EventArgs e ) {
    //    string editDataURL = ConfigurationSettings.AppSettings[ "AdPlanIt.SignupDataEntryPage" ];
    //    Response.Redirect( editDataURL );
    //}

    protected void DisplayFormData() {
        Table table = new Table();
        table.CellPadding = 2;
        table.CellSpacing = 0;

        AddTableRow( table, "<b>Please verify this information and  click OK to complete the signup process.</b>", null );
        AddTableRow( table, "First Name:", Request[ "first_name" ] );
        AddTableRow( table, "Last Name:", Request[ "last_name" ] );
        AddTableRow( table, "Company:", Request[ "company" ] );
        AddTableRow( table, "Phone:", Request[ "phone" ] );
        AddTableRow( table, "Email Address:", Request[ "email" ] );
        AddTableRow( table, "Your Web Site:", Request[ "source" ] );
        AddTableRow( table, "Where did you learn<br />about AdPlanit?", Request[ "source2" ] );

        InfoDiv.Controls.Clear();
        InfoDiv.Controls.Add( table );
   }

    private void AddTableRow( Table table, string s1, string s2 ) {
        TableRow r1 = new TableRow();

        TableCell c11 = new TableCell();
        TableCell c12 = new TableCell();

        c11.Text = s1;
        c12.Text = s2;

        r1.Cells.Add( c11 );
        if( s2 != null ) {
            r1.Cells.Add( c12 );
        }
        else {
            c11.ColumnSpan = 2;
        }

        table.Rows.Add( r1 );
    }
}
