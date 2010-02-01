using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;

using AjaxControlToolkit;

namespace WebLibrary
{
    /// <summary>
    /// This class handles all help-related functions
    /// </summary>
    public class Help
    {
        /// <summary>
        /// The primary list of help info.
        /// </summary>
        private static Dictionary<HelpID, HelpData> helpData;

        /// <summary>
        /// Call this once (in Global.asax) to initialize the help system.
        /// </summary>
        public static void Init(){
            helpData = new Dictionary<HelpID, HelpData>();

            LoadHelpData();
        }

        #region AddHelpPopup method
        /// <summary>
        /// Adds a help popup to the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="summaryHelp"></param>
        /// <param name="fullHelpLink"></param>
        public static void AddHelpPopup( HtmlGenericControl owningDiv, string controlID, HelpID helpID ) {
            AddHelpPopup( owningDiv, controlID, helpID, null );
        }

        /// <summary>
        /// Adds a help popup to the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="summaryHelp"></param>
        /// <param name="fullHelpLink"></param>
        public static void AddHelpPopup( HtmlGenericControl owningDiv, string controlID, HelpID helpID, string controlHighlightCssClass ) {
            if( helpData != null ) {
                if( helpData.ContainsKey( helpID ) ) {
                    HelpData data = helpData[ helpID ];
                    if( data != null ) {
                        AddHelpPopup( owningDiv, controlID, data.SummaryTitle, data.SummaryHelp, data.FullHelpLink, data.Offset, controlHighlightCssClass );
                    }
                }
            }
        }

        /// <summary>
        /// Adds a help popup to the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="summaryHelp"></param>
        /// <param name="fullHelpLink"></param>
        private static void AddHelpPopup( HtmlGenericControl owningDiv, string controlID, string summaryHelpTitle, string summaryHelp, string fullHelpLink, 
           Point offset, string controlHighlightCssClass ) {

            string popupPanelID = controlID + "HelpPopup";

            HoverMenuExtender popupExtender = new HoverMenuExtender();
            popupExtender.TargetControlID = controlID;
            popupExtender.PopupControlID = popupPanelID;
            popupExtender.PopupPosition = HoverMenuPopupPosition.Center;
            popupExtender.PopDelay = 500;
            popupExtender.PopupPosition = HoverMenuPopupPosition.Left;

            if( controlHighlightCssClass != null ) {
   //             popupExtender.HoverCssClass = controlHighlightCssClass;
                popupExtender.HoverCssClass = "popupHover";
            }

            Panel popupPanel = new Panel();
            Label label = new Label();
            label.Text = "<h1>" + summaryHelpTitle + "&nbsp;</h1>" + summaryHelp;
            popupPanel.Controls.Add( label );
            popupPanel.ID = popupPanelID;
            popupPanel.Style.Add( HtmlTextWriterStyle.Visibility, "hidden" );     // must do this or the panel will blink on page load!

            popupPanel.CssClass = "popupHelp";     // set the remaining style properties
            //if( offset.X != 0 ) {
            //    popupPanel.Style[ "margin-left" ] = String.Format( "{0}px", offset.X );
            //}

            if( fullHelpLink != null ) {
                LinkButton detailsLink = new LinkButton();
                detailsLink.Text = "Full Help on this Topic";
                detailsLink.PostBackUrl = fullHelpLink;
                detailsLink.CssClass = "popupFullHelpLink";

                popupPanel.Controls.Add( detailsLink );
            }

            owningDiv.Controls.Add( popupPanel );
            owningDiv.Controls.Add( popupExtender );
        }
        #endregion

        /// <summary>
        /// Encapsulates help for a particular situation.
        /// </summary>
        private class HelpData {
            public string SummaryTitle { set; get; }
            public string SummaryHelp { set; get; }
            public string FullHelpLink { set; get; }
            public Point Offset { set; get; }

            public HelpData( string summaryHelpTitle, string fullHelpLink, string summaryHelp ) {
                this.SummaryTitle = summaryHelpTitle;
                this.SummaryHelp = summaryHelp;
                this.FullHelpLink = fullHelpLink;
                this.Offset = new Point( 0, 0 );
            }

            public HelpData( string summaryHelpTitle, string fullHelpLink, Point offset, string summaryHelp ) {
                this.SummaryTitle = summaryHelpTitle;
                this.SummaryHelp = summaryHelp;
                this.FullHelpLink = fullHelpLink;
                this.Offset = offset;
            }
        }

        // -------------------- HELP DATA --------------------------

        /// <summary>
        /// Selections of help items
        /// </summary>
        public enum HelpID
        {
            CampaignName,
            CampaignBudget,
            CampaignDates,
            CampaignSettings,
            CampaignDemographics,
            CampaignEnterPlan,
            CampaignGetPlan,
            EnterPlan,
            EditPlan,
            HomeGo,
        }

        /// <summary>
        /// initializes specific help items.
        /// </summary>
        public static void LoadHelpData()
        {
            helpData.Add( HelpID.CampaignName,
                new HelpData( "Campaign Name:", "help/HelpIndex.htm", new Point( -460, 0 ),
                    "Enter a name to identify your campaign." ) );

            helpData.Add( HelpID.CampaignDates,
                new HelpData( "Campaign Start/End Dates:", "help/HelpIndex.htm", new Point( -460, 0 ),
                    "Target audience response will be evaluated over this range of dates." ) );

            helpData.Add( HelpID.CampaignBudget,
                new HelpData( "Campaign Budget:", "help/HelpIndex.htm", new Point( -460, 0 ),
                    "Enter the planned total budget for all media in the campaign in dollars (USD)." ) );

            helpData.Add( HelpID.CampaignSettings,
                new HelpData( "Product Type and Business Situation:", "help/HelpIndex.htm", new Point( -460, 0 ),
                    "Set the sliders to best describe your product and business situation. " ) );

            helpData.Add( HelpID.CampaignDemographics,
                new HelpData( "Customer Definition:", "help/HelpIndex.htm", new Point( -460, 0 ),
                    "Click 'Define a Specific Target Segment' to bring up the popup dialog where you can enter the descriptive settings." ) );

            helpData.Add( HelpID.CampaignEnterPlan,
                new HelpData( "Enter Plan:", null,
                    "Click this button to enter the details of the media plan you are currently using, or are interested in possiby using in the future. " ) );

            helpData.Add( HelpID.CampaignGetPlan,
                new HelpData( "Get AdPlanIt Base Plan:", null,
                    "Click this button tand AdPlanIt will automatically creeate a media plan that is a good starting point for your investigations." ) );

            //helpData.Add( HelpID.EditMediaRemovePlan,
            //    new HelpData( "Remove Plan:", "help/HelpIndex.htm",
            //        "Click this link to remove the plan from this list.  Removed plans are not deleted - they are are still available through the Saved Plans list." ) );

            //helpData.Add( HelpID.EditMediaClonePlan,
            //    new HelpData( "Copy Plan:", "help/HelpIndex.htm",
            //        "Click this link to add a copy of this plan to the list.  The new plan wil automatically be given a new name." ) );

            helpData.Add( HelpID.EnterPlan,
                new HelpData( "Enter Media Plan:", "help/HelpIndex.htm",
                    "To create a media plan, first add media item using the Add Media and Add Subtype links.  Then choose a Prominence, set the Timing," +
                    "and finally select the number of ad spots by settiing the budget for each media item." ) );

            helpData.Add( HelpID.EditPlan,
                new HelpData( "AdPlanIt Base Media Plan:", "help/HelpIndex.htm",
                    "The recommended base media plan is generated based on best-practice rules for your media campaign.  It is intended as a starting point  " +
            " for your personal investigations - click the Create a Copy link to create a new media plan, and then edit that plan to suit your wishes.  " +
            "Then run the AdPlanIt media simulation to compare the effectiveness of each of the plans." ) );
        }
    }
}
