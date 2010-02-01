using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebLibrary;

namespace BusinessLogic
{
    public class StoryGenerator
    {
        private string emailTitle = "AdPlanit Session Transcript";

        /// <summary>
        /// The story elements are the raw events that the user's actions accumulate
        /// </summary>
        public List<StoryElement> storyElements;
        protected DateTime initializationTime;

        public string LineBreak = "<br>";

        /// <summary>
        /// Creates a new story generator.
        /// </summary>
        public StoryGenerator() {
            storyElements = new List<StoryElement>();
            initializationTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the text of the story.
        /// </summary>
        /// <returns></returns>
        public string StoryText( bool engineeringMode ) {
            string s = "";

            if( engineeringMode == false ) {
                StoryGenCore core = new GeneralStoryGenCore( storyElements, this.initializationTime );
                core.LineBreak = this.LineBreak;
                return core.StoryText();
            }
            else {
                // engineering mode - dump raw event data for the story
                for( int i = 0; i < storyElements.Count; i++ ) {
                    StoryElement se = storyElements[ i ];

                    if( se is PageEventElement ) {
                        PageEventElement pe = se as PageEventElement;
                        s += "Page View: " + pe.PageName + "<br><br>";
                    }

                    else if( se is UserLoginElement ) {
                        UserLoginElement ule = se as UserLoginElement;
                        s += "User Login: " + ule.UserEmail + "<br><br>";
                    }

                    else if( se is GeneralInfoElement ) {
                        GeneralInfoElement gie = se as GeneralInfoElement;
                        s += "Info: " + gie.Info + "<br><br>";
                    }

                    else if( se is CreateCampaignElement ) {
                        CreateCampaignElement cce = se as CreateCampaignElement;
                        s += "Create Campaign: " + cce.CampaignName + "<br><br>";
                    }

                    else if( se is CreatePlanElement ) {
                        CreatePlanElement cpe = se as CreatePlanElement;
                        s += "Create Plan: " + cpe.PlanName + "<br><br>";
                    }

                    else if( se is AutoCreatePlanElement ) {
                        AutoCreatePlanElement acpe = se as AutoCreatePlanElement;
                        s += "Auto-Create Plan: " + acpe.PlanName + "<br><br>";
                    }

                    else if( se is AddMediaElement ) {
                        AddMediaElement ame = se as AddMediaElement;
                        s += "Add Media: " + ame.VehicleName + " (" + ame.MediaType + ")<br><br>";
                    }

                    else if( se is SimulationStartedElement ) {
                        SimulationStartedElement sse = se as SimulationStartedElement;
                        s += "Simulate Plan: " + sse.PlanName + "<br><br>";
                    }

                    else if( se is SimulationDoneElement ) {
                        SimulationDoneElement sde = se as SimulationDoneElement;
                        s += "Simulaton Done: " + sde.PlanName + " stars=" + sde.OverallStars.ToString() + "<br><br>";
                    }

                    else if( se is SuggestionAcceptedElement ) {
                        SuggestionAcceptedElement sae = se as SuggestionAcceptedElement;
                        s += "Accepted suggestion: " + sae.Suggestion + "<br><br>";
                    }

                    else if( se is ModifyPlanElement ) {
                        ModifyPlanElement mpe = se as ModifyPlanElement;
                        s += "Modify Plan: Changed " + mpe.ParamName + " from " + mpe.OldValueStr + " to " + mpe.NewValueStr + "<br><br>";
                    }

                    else if( se is ModifyMediaElement ) {
                        ModifyMediaElement mme = se as ModifyMediaElement;
                        s += "Modify Media: Changed " + mme.ParamName + " for " + mme.VehicleName + " from " + mme.OldValueStr + " to " + mme.NewValueStr + "<br><br>";
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Adds an element to the story generator's event list.
        /// </summary>
        /// <param name="element"></param>
        public void AddElement( StoryElement element ) {
            storyElements.Add( element );
        }

        public void SendEmail( string toEmailAddress ) {
            string savedLineBreak = this.LineBreak;
            this.LineBreak = "\r\n";

            string custBody = this.StoryText( false );

            EmailSender emailSender = new EmailSender();
            emailSender.SendMessage( toEmailAddress, emailTitle, custBody );

            this.LineBreak = savedLineBreak;
        }

        /// <summary>
        /// Gets the session's story generator object.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static StoryGenerator GetStoryGenerator( System.Web.UI.Page callingPage ) {
            StoryGenerator storyGen = null;
            if( callingPage.Session[ "StoryGenerator" ] != null ) {
                storyGen = (StoryGenerator)callingPage.Session[ "StoryGenerator" ];
            }
            else {
                storyGen = new StoryGenerator();
                callingPage.Session.Add( "StoryGenerator", storyGen );
            }
            return storyGen;
        }


        /// <summary>
        /// A StoryElement represents a specific user action or system event
        /// </summary>
        public abstract class StoryElement {
            public DateTime TimeStamp;

            public StoryElement() {
                TimeStamp = DateTime.Now;
            }
        }

        /// <summary>
        /// Page transition story element
        /// </summary>
        public class PageEventElement : StoryElement
        {
            public string PageName;

            public PageEventElement( string pageName )
                : base() {
                this.PageName = pageName;
            }
        }

        /// <summary>
        /// General-information story element
        /// </summary>
        public class GeneralInfoElement : StoryElement
        {
            public string Info;

            public GeneralInfoElement( string info )
                : base() {
                this.Info = info;
            }
        }

        /// <summary>
        /// Suggestion-accepted story element
        /// </summary>
        public class SuggestionAcceptedElement : StoryElement
        {
            public string Suggestion;
            public string PlanName;
            public string NewPlanName;

            public SuggestionAcceptedElement( string suggestion, string planName, string newPlanName )
                : base() {
                this.Suggestion = suggestion;
                this.PlanName = planName;
                this.NewPlanName = newPlanName;
            }
        }

        /// <summary>
        /// Login story element
        /// </summary>
        public class UserLoginElement : StoryElement
        {
            public string UserEmail;
            public string UserFirstName;

            public UserLoginElement( string userEmail, string firstName )
                : base() {
                this.UserEmail = userEmail;
                this.UserFirstName = firstName;
            }
        }

        /// <summary>
        /// New-campaign creation story element
        /// </summary>
        public class CreateCampaignElement : StoryElement
        {
            public string CampaignName;

            public CreateCampaignElement( string campaignName )
                : base() {
                this.CampaignName = campaignName;
            }
        }

        /// <summary>
        /// New-plen creation (created by the system) story element
        /// </summary>
        public class AutoCreatePlanElement : StoryElement
        {
            public string PlanName;

            public AutoCreatePlanElement( string planName )
                : base() {
                this.PlanName = planName;
            }
        }

        /// <summary>
        /// New-plen creation story element
        /// </summary>
        public class CreatePlanElement : StoryElement
        {
            public string PlanName;

            public CreatePlanElement( string planName )
                : base() {
                this.PlanName = planName;
            }
        }

        /// <summary>
        /// Adding demographic story element
        /// </summary>
        public class AddDemographicElement : StoryElement
        {
            public string CampaignName;
            public string SegmentName;
            public string DemoSummary;
            public string GeoSummary;

            public AddDemographicElement( string campaigntName,  string segmentName, string demographicSummary, string geographcSummary )
                : base() {
                this.CampaignName = campaigntName;
                this.SegmentName = segmentName;
                this.GeoSummary = geographcSummary;
                this.DemoSummary = demographicSummary;
            }
        }

        /// <summary>
        /// Adding media vehicle story element
        /// </summary>
        public class AddMediaElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;
            public string VehicleName;
            public string MediaType;
            public string MediaSubtype;

            public AddMediaElement( string campaignName, string planName, string vehicleName, string mediaType, string mediaSubtype )
                : base() {
                this.CampaignName = campaignName;
                this.PlanName = planName;
                this.VehicleName = vehicleName;
                this.MediaType = mediaType;
                this.MediaSubtype = mediaSubtype;
            }
        }

        /// <summary>
        /// Removing media vehicle story element
        /// </summary>
        public class RemoveMediaElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;
            public string VehicleName;

            public RemoveMediaElement( string campaigntName, string planName, string vehicleName )
                : base() {
                this.CampaignName = campaigntName;
                this.PlanName = planName;
                this.VehicleName = vehicleName;
            }
        }

        /// <summary>
        /// Media-element modification story element
        /// </summary>
        public class ModifyMediaElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;
            public string VehicleName;
            public string ParamName;
            public string OldValueStr;
            public string NewValueStr;

            public ModifyMediaElement( string campaigntName, string planName, string vehicleName, string paramName, string oldValueStr, string newValueStr )
                : base() {
                this.CampaignName = campaigntName;
                this.PlanName = planName;
                this.VehicleName = vehicleName;
                this.ParamName = paramName;
                this.OldValueStr = oldValueStr;
                this.NewValueStr = newValueStr;
            }
        }

        /// <summary>
        /// Media-plan modification story element
        /// </summary>
        public class ModifyPlanElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;
            public string ParamName;
            public string OldValueStr;
            public string NewValueStr;

            public ModifyPlanElement( string campaigntName, string planName, string paramName, string oldValueStr, string newValueStr )
                : base() {
                this.CampaignName = campaigntName;
                this.PlanName = planName;
                this.ParamName = paramName;
                this.OldValueStr = oldValueStr;
                this.NewValueStr = newValueStr;
            }
        }

        /// <summary>
        /// Simulation-started story element
        /// </summary>
        public class SimulationStartedElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;

            public SimulationStartedElement( string campaigntName, string planName )
                : base() {
                this.CampaignName = campaigntName;
                this.PlanName = planName;
            }
        }

        /// <summary>
        /// Simulation-done story element
        /// </summary>
        public class SimulationDoneElement : StoryElement
        {
            public string CampaignName;
            public string PlanName;
            public double OverallStars;
            public string IssueSummary;
            public List<string> Comgratulations;

            public SimulationDoneElement( string campaigntName, string planName, double overallStars, string issueSummary, List<string> comgratulations )
                : base() {
                this.CampaignName = campaigntName;
                this.PlanName = planName;
                this.OverallStars = overallStars;
                this.IssueSummary = issueSummary;
                this.Comgratulations = comgratulations;
            }
        }
    }
}
