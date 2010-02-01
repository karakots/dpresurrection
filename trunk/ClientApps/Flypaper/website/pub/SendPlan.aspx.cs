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

public partial class SendPlan : System.Web.UI.Page
{
    private const string sendMessageSubject = "You have received an invitation from AdPlanit to view a media plan";
    private const string sendMessageHeader = "{0} has sent you an invitation to view a media plan from AdPlanit.com. ";
    private const string sendMessageBody = "To proceed, click on the link below (or copy and paste it into your web browser's location field):";
    private const string sendMessageFooter = "The process will create your own copy of the media plan in your AdPlanit account, " +
        "where you can examine it in detail.  For further information, see our website www.AdPlanit.com.";
    private const string sendMessageSignature = "AdPlanit Support";

    private const string competeMessageSubject = "You have received an invitation from AdPlanit to join a media plan competition";
    private const string competeMessageHeader = "{0} has sent you an invitation to join a media plan competition, hosted by AdPlanit.com!  ";
    private const string competeMessageBody = "To proceed, click on the link below (or copy and paste it into your web browser's location field):";
    private const string competeMessageFooter = "The process will create a base media plan in your AdPlanit account.  " +
        "Then, create your own media plans using the campaign you used to receive the base plan.  The goal is to get the highest TOTAL SALES number for "+
        "a plan while staying within the campaign's set budget and other constraints.  To enter " +
        "the competition, click the \"Enter This Plan in Competition\" Link, which you will see on the Analysis page for any plan in this campaign. " +
        "For further information, see our website www.AdPlanit.com.  Good luck!";

    protected MediaPlan currentMediaPlan;

    protected void Page_Load( object sender, EventArgs e ) {
        RedirectUnknownUsers();

        currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        if( currentMediaPlan != null ) {
            PlanName.Text = currentMediaPlan.PlanName;
            CampaignName.Text = currentMediaPlan.CampaignName;
        }

        if( Request[ "NameTextbox2" ] != null ) {
            NameTextbox.Text = Request[ "NameTextbox2" ];
        }

        if( Request[ "MessageTextBox2" ] != null ) {
            MessageTextBox.Text = Request[ "MessageTextBox2" ];
        }

        if( Request[ "ReceiverEmail2" ] != null ) {
            ReceiverEmail.Text = Request[ "ReceiverEmail2" ];
        }

        if( Request[ "CompetitionName2" ] != null ) {
            CompetitionName.Text = Request[ "CompetitionName2" ];
        }

        if( Request[ "CompetitionMode" ] != null ) {
            CompetitionMode.Value = Request[ "CompetitionMode" ];
            if( CompetitionMode.Value == "true" ) {
                CompNameLabel.CssClass = "CompVis";
                CompetitionName.CssClass = "CompVis";
                CompetitionSelection.Checked = true;
            }
            else {
                CompNameLabel.CssClass = "CompVisHidden";
                CompetitionName.CssClass = "CompVisHidden";
                CompetitionSelection.Checked = false;
            }
        }

        if( Request[ "b1" ] != null ) {
            GoButton_Click( null, null );
        }

        if( this.currentMediaPlan.Specs.IsCompetition == true ) {
            if( this.currentMediaPlan.Specs.CompetitionOwner != Utils.GetUser( this ) ) {
                CompetitonLabel.Text = String.Format( "This media plan is part of the competiton named: <b>{0}</b><br>  Competition initiated by {1}",
                    this.currentMediaPlan.Specs.CompetitionName, this.currentMediaPlan.Specs.CompetitionOwner );
                CompetitionSelection.Visible = false;
            }
            else {
                // you own the competition
                if( this.currentMediaPlan.Specs.CompetitionBasePlanID == this.currentMediaPlan.PlanID ) {
                    // this is the base plan
                    CompetitonLabel.Text = String.Format( "This media plan is the base plan for your active competiton: <b>{0}</b><br>" +
                        "Sending it to new recipients now will invite them to join the competiton.",
                        this.currentMediaPlan.Specs.CompetitionName );
                    CompetitionSelection.Visible = false;
                    CompetitionSelection.Checked = true;
                    CompNameLabel.CssClass = "CompVis";
                    CompetitionName.CssClass = "CompVis";
                    CompetitionName.Text = this.currentMediaPlan.Specs.CompetitionName;
                }
                else {
                    // this isn't the base plan
                    CompetitonLabel.Text = String.Format( "This media plan is part of your active competiton: <b>{0}</b>" +
                    "<br>To invite other user(s) to join the competition, send them the base plan for this competition.",
                       this.currentMediaPlan.Specs.CompetitionName );
                    CompetitionSelection.Visible = false;
                }
            }
        }
    }

    protected void GoButton_Click( object sender, EventArgs e ) {
        string rcvr = ReceiverEmail.Text.Trim();

        if( (rcvr.IndexOf( "@" ) != 0) && (rcvr.IndexOf( "." ) != 0) && (rcvr.IndexOf( "@" ) < rcvr.LastIndexOf( "." )) ) {

            string messageBody = GetMessageBody( false );
            if( messageBody != null ) {
                EmailSender emailSender = new EmailSender();
                string emailSubj = sendMessageSubject;
                if( CompetitionMode.Value == "true" ) {
                    emailSubj = competeMessageSubject;

                    // adjust  the plan so that it is set up for a competition
                    if( Request[ "CompetitionName" ] != null ) {
                        this.currentMediaPlan.Specs.CompetitionName = Request[ "CompetitionName" ];
                    }
                    else {
                        this.currentMediaPlan.Specs.CompetitionName = Request[ "CompetitionName2" ];  // from preview
                    }
                    this.currentMediaPlan.Specs.CompetitionDate = DateTime.Now;
                    this.currentMediaPlan.Specs.CompetitionOwner = Utils.GetUser( this );
                    this.currentMediaPlan.Specs.CompetitionCampaign = this.currentMediaPlan.CampaignName;    // open competition campaigns cannot be renamed!!!
                    this.currentMediaPlan.Specs.CompetitionBasePlanID = this.currentMediaPlan.PlanID;
                    this.currentMediaPlan.Competitor = Utils.GetUser( this );
                    PlanStorage storage = new PlanStorage();
                    storage.SaveMediaPlan( Utils.GetUser( this ), this.currentMediaPlan );
                    Utils.SetCurrentMediaPlan( this, this.currentMediaPlan );
                }

                // send the message
                List<string> receivers = new List<string>();
                if( rcvr.IndexOf( ";" ) == -1 ) {
                    receivers.Add( rcvr );
                }
                else {
                    string[] rcvrs = rcvr.Split( ';' );
                    foreach( string rc in rcvrs ) {
                        receivers.Add( rc );
                    }
                }

                bool invalidEmailEncountered = false;
                try {
                    emailSender.SendMessage( receivers, emailSubj, messageBody );
                }
                catch( FormatException ) {
                    // one or more of the receivers were not valid email addresses
                    invalidEmailEncountered = true;
                }

                if( invalidEmailEncountered == false ) {
                    if( CompetitionMode.Value != "true" ) {
                        Response.Redirect( "SendPlanDone.aspx" );
                    }
                    else {
                        Response.Redirect( "SendPlanDone.aspx?CompetitionMode=true" );
                    }
                }
                else {
                    InvalidEmail.Visible = true;
                }
            }
            else {
                InvalidEmail.Visible = true;
                InvalidEmail.Text = "Error: Unable to get server from request URL!";
            }
        }
        else {
            InvalidEmail.Visible = true;
        }
    }

    /// <summary>
    /// Replaces the display with the preview of the message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PreviewButton_Click( object sender, EventArgs e ) {
        string rcvr = ReceiverEmail.Text.Trim();
        if( (rcvr.IndexOf( "@" ) != 0) && (rcvr.IndexOf( "." ) != 0) && (rcvr.IndexOf( "@" ) < rcvr.LastIndexOf( "." )) ) {

            SendPlanPanel.Controls.Clear();
            Label preview = new Label();
            preview.Text = "<br><b>Preview of Your Message:</b><br><br><br><br><br>" + GetMessageBody( true ) + "<br><br><br><br><br><br><br>";
            string h1 = String.Format( "<input type='hidden' name='MessageTextBox2' value='{0}'>", Request[ "MessageTextBox" ] );
            string h2 = String.Format( "<input type='hidden' name='ReceiverEmail2' value='{0}'>", Request[ "ReceiverEmail" ] );
            string h3 = String.Format( "<input type='hidden' name='NameTextbox2' value='{0}'>", Request[ "NameTextBox" ] );
            string h4 = String.Format( "<input type='hidden' name='CompetitionName2' value='{0}'>", Request[ "CompetitionName" ] );
            preview.Text += h1 + h2 + h3 + h4;
            SendPlanPanel.Controls.Add( preview );
            Button b1 = new Button();
            b1.Text = "Send Now";
            b1.Attributes.Add( "style", "font-size:8pt; width:150px;" );
            b1.ID = "b1";
            Button b2 = new Button();
            b2.Text = "Edit Message";
            b2.Attributes.Add( "style", "font-size:8pt; margin-left:10px;" );
            SendPlanPanel.Controls.Add( b1 );
            SendPlanPanel.Controls.Add( b2 );
        }
        else {
            InvalidEmail.Visible = true;
        }
    }

    private string GetMessageBody( bool forDisplay ) {

        string msgHeader = sendMessageHeader;
        string msgBody = sendMessageBody;
        string msgFooter = sendMessageFooter;

        if( Request[ "CompetitionMode" ] == "true" || this.currentMediaPlan.Specs.IsCompetition ) {
            msgHeader = competeMessageHeader;
            msgBody = competeMessageBody;
            msgFooter = competeMessageFooter;
        }

        string lineBreak = "\r\n\r\n";
        if( forDisplay ) {
            lineBreak = "<br><br>";
        }
        string requestUrl = Request.Url.ToString();
        if( requestUrl.IndexOf( "/" ) == -1 ) {
            return null;
        }

        string salutation = "Hello,";
        string rname = Request[ "NameTextbox" ];
        if( Request[ "NameTextbox" ] == null ) {
            rname = Request[ "NameTextbox2" ];
        }

        if( rname != null  && rname.Trim() != "" ) {
            salutation = String.Format( "Hello {0},", Request[ "NameTextbox" ] );
        }
        salutation += lineBreak; 

        string fname = Profile.UserMediaItems.FirstName;
        string lname = Profile.UserMediaItems.LastName;
        string uname = "";
        if( fname != null && fname != "" && lname != null && lname != "" ) {
            uname = String.Format( "{0} {1} ({2})", fname, lname, Utils.GetUser( this ) );
        }
        else {
            uname = Utils.GetUser( this );
        }

        string head = String.Format( msgHeader, uname );

        string userMsg = MessageTextBox.Text.Trim();
        if( Request[ "MessageTextBox2" ] != null ) {
            userMsg = Request[ "MessageTextBox2" ];
        }

        if( userMsg != "" ) {
            userMsg = String.Format( "{0}Their message: \"{1}\"{0}", lineBreak, userMsg );
            head += userMsg;
        }
        head += msgBody + lineBreak;

        requestUrl = requestUrl.Substring( 0, requestUrl.LastIndexOf( "/" ) );
        string copyLinkLoc = String.Format( "{0}/ReceivePlan.aspx?p={1}&u={2}", requestUrl, this.currentMediaPlan.PlanID,
            Utils.GetUser( this ).Replace( ".", "-" ).Replace( "@", "--" ) );

        string foot = lineBreak + String.Format( msgFooter ) + lineBreak + sendMessageSignature;

        return salutation + head + copyLinkLoc + foot;
    }

    private void RedirectUnknownUsers() {
        if( Utils.UserIsDemo( this ) ) {
            Response.Redirect( "~/Home.aspx" );
        }
    }
}
