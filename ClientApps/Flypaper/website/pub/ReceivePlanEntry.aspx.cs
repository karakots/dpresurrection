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

public partial class ReceivePlanEntry : System.Web.UI.Page
{
    private const string unknownIDError = "Sorry, but the referenced media plan does not exist.<br><br>" +
        "Most likely the original media plan has been deleted by its owner.<br><br><br><br>";

    protected void Page_Load( object sender, EventArgs e ) {

        RedirectUnknownUsers();

        LoadTextfields();
    }

    /// <summary>
    /// Populates the textfields appropriately for the situation.
    /// </summary>
    /// <remarks>The plan ID and owner values can either come from session values, which are set by the ReceivePlan.aspx page, or from POST arguments
    /// (hidden fields) from the ReceivePlanCheck.aspx page.</remarks>
    private void LoadTextfields() {
        // this value is set by (only) the ReceivePlan.aspx page
        if( Session[ "ReceivedPlanUser" ] != null ) {
            MediaPlan transferredPlan = GetSpecifiedPlan();
            if( transferredPlan != null ) {
                ReceivedPlanUser.Value = (string)Session[ "ReceivedPlanUser" ];
                ReceivedPlanID.Value = (string)Session[ "ReceivedPlanID" ];

                string uname = ReceivedPlanUser.Value;
                if( ReceivedPlanUser.Value.IndexOf( "@" ) != -1 ) {     // should always be true
                    uname = ReceivedPlanUser.Value.Substring( 0, ReceivedPlanUser.Value.IndexOf( "@" ) );
                }

                CampaignName.Text = String.Format( "{0} - from {1}", transferredPlan.CampaignName, uname );     // append the sending user's name to the campaign
                PlanName.Text = transferredPlan.PlanName;
                if( transferredPlan.Specs.CompetitionName != null && transferredPlan.Specs.CompetitionName.Trim() != "" ) {
                    CompetitionName.Text = transferredPlan.Specs.CompetitionName;
                    CompetitionNameLabelR.Visible = true;
                    CompetitionName.Visible = true;
                }
                else {
                    CompetitionNameLabelR.Visible = false;
                    CompetitionName.Visible = false;
                }

                // override with values form the login page, if any
                if( Session[ "ReceivedPlanCampaign" ] != null ) {
                    CampaignName.Text = (string)Session[ "ReceivedPlanCampaign" ];
                    PlanName.Text = (string)Session[ "ReceivedPlanName" ];
                    CompetitionName.Text = (string)Session[ "ReceivedCompetitionName" ];
                    Session.Remove( "ReceivedPlanCampaign" );
                    Session.Remove( "ReceivedPlanName" );
                    Session.Remove( "ReceivedCompetitionName" );
                }
            }
            else {
                // unable to find the plan from the user aod ID
                CampaignName.Text = "Error - plan not found!";
                PlanName.Text = "Error - plan not found!";

                RcvPlanPanel.Controls.Clear();
                Label label = new Label();
                label.Text = unknownIDError;
                Button button = new Button();
                button.Text = "Return to My Campaigns";
                button.PostBackUrl = "Campaigns.aspx";
                button.Attributes.Add( "style", "font-size:8pt" );
                RcvPlanPanel.Controls.Add( label );
                RcvPlanPanel.Controls.Add( button );
            }
            // ensure the process happens only once
            Session.Remove( "ReceivedPlanUser" );
            Session.Remove( "ReceivedPlanID" );
        }
        else {
            CampaignName.Text = Request[ "CampaignName" ];
            PlanName.Text = Request[ "PlanName" ];
            ReceivedPlanID.Value = Request[ "ReceivedPlanID" ];
            ReceivedPlanUser.Value = Request[ "ReceivedPlanUser" ];

        }
        SenderLabel.Text = ReceivedPlanUser.Value;
    }

    private void RedirectUnknownUsers() {
        if( Utils.UserIsDemo( this ) ) {
            Response.Redirect( "~/Home.aspx" );
        }
    }

    /// <summary>
    /// Handles clicks on the "no thanks" button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RefuseButton_Click( object sender, EventArgs e ) {
        Session.Remove( "ReceivedPlanUser" );
        Session.Remove( "ReceivedPlanID" );

        Response.Redirect( "Campaigns.aspx" );
    }

    /// <summary>
    /// Returns the media plan specified by the session values (user email and plan ID)
    /// </summary>
    /// <returns></returns>
    private MediaPlan GetSpecifiedPlan() {
        string receivedPlanUser = (string)Session[ "ReceivedPlanUser" ];
        string receivedPlanID = (string)Session[ "ReceivedPlanID" ];
        if( receivedPlanUser == null || receivedPlanID == null ) {
            return null;
        }

        Guid guid = new Guid();
        try {
            guid = new Guid( receivedPlanID );
        }
        catch {
            return null;
        }

        PlanStorage storage = new PlanStorage();
        MediaPlan plan = storage.LoadMediaPlan( receivedPlanUser, guid );
        return plan;
    }
}
