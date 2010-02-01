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

public partial class SendPlanDone : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e ) {
        if( Request[ "CompetitionMode" ] == "true" ) {
            SentLabel.Text = "Your email has been sent successfully.<br><br>This campaign has also been set to \"competiton mode\";<br>look for extra options in the Action menu for this campaign.";
        }
    }
}
