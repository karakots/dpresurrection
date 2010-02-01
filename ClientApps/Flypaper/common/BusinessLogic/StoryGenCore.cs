using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    #region Abstract Core Class
    /// <summary>
    /// A StoryGenCore converts a list of story elements into a human-readable story.
    /// </summary>
    public abstract class StoryGenCore
    {
        protected List<StoryGenerator.StoryElement> storyElements;
        protected DateTime genInitializationTime;
        protected Random rand;

        public string LineBreak = "<br>";

        public StoryGenCore( List<StoryGenerator.StoryElement> storyElements, DateTime initializationTime ) {
            this.storyElements = storyElements;
            this.genInitializationTime = initializationTime;
        }

        public abstract string StoryText();
    }
    #endregion

    public class GeneralStoryGenCore : StoryGenCore
    {
        private bool first = true;
        private DateTime prevTime = new DateTime();
        private string prevPlanName = "";
        private bool waitingForLogin = false;

        private List<RandomChoice> startChoices;
        private List<RandomChoice> connectorChoices;
        private List<RandomChoice> pageSentenceChoices;
        private List<RandomChoice> loginChoices;
        private List<RandomChoice> loginWelcomeChoices;
        private List<RandomChoice> startSimChoices;
        private List<RandomChoice> ranSimChoices;
        private List<RandomChoice> newCampaignAndPlanChoices;
        private List<RandomChoice> loginToPageChoices;
        private List<RandomChoice> addMediaChoices;
        private List<RandomChoice> addMedia2Choices;
        private List<RandomChoice> addMedia3Choices;
        private List<RandomChoice> addMediaAndSetBudgetChoices;
        private List<RandomChoice> addMediaAndSetParamChoices;
        private List<RandomChoice> addMediaAndSetParam2Choices;
        private List<RandomChoice> acceptedSuggestionChoices;

        private void InitChoices() {
            // user login
            loginChoices = new List<RandomChoice>();    // args: time
            AddChoice( "You logged in to the AdPlanit system at {0} PDT.  ", 100, loginChoices );

            loginWelcomeChoices = new List<RandomChoice>();    // args: time
            AddChoice( "<br><br><br>(As you proceed, this panel will keep track of your actions and results.)", 100, loginWelcomeChoices );

            // user login then page view
            loginToPageChoices = new List<RandomChoice>();    // args: time, email, dest page
            AddChoice( "You logged in to the AdPlanit system at {0} PDT.  ", 100, loginToPageChoices );
            AddChoice( "You logged in to the AdPlanit system at {0} PDT.  ", 100, loginToPageChoices );
            AddChoice( "Your login took place at {0} PDT.  ", 100, loginToPageChoices );

            // created campaign then plan
            newCampaignAndPlanChoices = new List<RandomChoice>();    // args: time, campaign name, plan name
            AddChoice( "created the campaign \"{1}\" and the media plan named \"{2}\".  ", 100, newCampaignAndPlanChoices );
            AddChoice( "created the campaign named \"{1}\" and the media plan \"{2}\".  ", 100, newCampaignAndPlanChoices );
            AddChoice( "created the campaign named \"{1}\" and added the media plan \"{2}\".  ", 100, newCampaignAndPlanChoices );

            // ran a simulation
            ranSimChoices = new List<RandomChoice>();    // args: time, plan name, run time, stars
            AddChoice( "At {0} you simulated the \"{1}\" plan, which completed in {2} seconds and got a {3}-star rating.  ", 100, ranSimChoices );
            AddChoice( "You simulated the \"{1}\" plan at {0}, and it received a {3}-star rating.  ", 100, ranSimChoices );
            AddChoice( "You ran a simulation of the \"{1}\" plan at {0}, and it received a {3}-star rating.  ", 100, ranSimChoices );
            AddChoice( "At {0} you ran a simulation of the \"{1}\" plan, which completed in {2} seconds and received a {3}-star rating.  ", 100, ranSimChoices );

            // header for first action after a while of inaction
            startChoices = new List<RandomChoice>();   // args: time
            AddChoice( "At {0}, you ", 100, startChoices );

            // header (opening for sentence)
            connectorChoices = new List<RandomChoice>();      // args: none
            AddChoice( "Then, you ", 150, connectorChoices );
            AddChoice( "After that, you ", 150, connectorChoices );
            AddChoice( "You then ", 100, connectorChoices );
            AddChoice( "Next, you ", 50, connectorChoices );

            // general page view
            pageSentenceChoices = new List<RandomChoice>();   // args: time, page name
            AddChoice( "{0}went to the {1} page.  ", 400, pageSentenceChoices );
            AddChoice( "{0}navigated to the {1} page.  ", 100, pageSentenceChoices );
            AddChoice( "{0}viewed the {1} page.  ", 150, pageSentenceChoices );

            // single media add
            addMediaChoices = new List<RandomChoice>();   // args: time, vehicle, type
            AddChoice( "added \"{1}\" ({2}) to the plan.", 100, addMediaChoices );
            AddChoice( "added media \"{1}\" ({2}) to the plan.  ", 100, addMediaChoices );
            AddChoice( "added \"{1}\" ({2}) to the media plan.  ", 100, addMediaChoices );

            // dual media add
            addMedia2Choices = new List<RandomChoice>();   // args: time, vehicle1, type1,  vehicle2, type2
            AddChoice( "added \"{1}\" ({2}) and  \"{3}\" ({4}) to the plan.  ", 100, addMedia2Choices );
            AddChoice( "added media \"{1}\" ({2}) and  \"{3}\" ({4}) to the plan.  ", 100, addMedia2Choices );
            AddChoice( "added media to the plan: \"{1}\" ({2}) and  \"{3}\" ({4}).  ", 100, addMedia2Choices );

            // triple media add
            addMedia3Choices = new List<RandomChoice>();   // args: time, vehicle1, type1,  vehicle2, type2,  vehicle3, type3
            AddChoice( "added \"{1}\" ({2}),  \"{3}\" ({4}), and  \"{5}\" ({6}) to the plan.  ", 100, addMedia3Choices );
            AddChoice( "added media \"{1}\" ({2}),  \"{3}\" ({4}), and  \"{5}\" ({6}) to the plan.  ", 100, addMedia3Choices );
            AddChoice( "added media to the plan: \"{1}\" ({2}),  \"{3}\" ({4}), and  \"{5}\" ({6}).  ", 100, addMedia3Choices );

            // add media amd set param
            addMediaAndSetParamChoices = new List<RandomChoice>();   // args: time, vehicle, type, paramName, newVaue
            AddChoice( "added \"{1}\" ({2}) to the plan and set its {3} to \"{4}\".  ", 100, addMediaAndSetParamChoices );
            AddChoice( "added \"{1}\" ({2}) to the plan and then set its {3} to \"{4}\".  ", 100, addMediaAndSetParamChoices );
            AddChoice( "added \"{1}\" ({2}) to the plan and changed its {3} to \"{4}\".  ", 100, addMediaAndSetParamChoices );

            // add media amd set budget
            addMediaAndSetBudgetChoices = new List<RandomChoice>();   // args: time, vehicle, type, paramName, newVaue
            AddChoice( "added \"{1}\" ({2}) to the plan and set its {3} to ${4:f2}.  ", 100, addMediaAndSetBudgetChoices );
            AddChoice( "added \"{1}\" ({2}) to the plan and then set its {3} to ${4:f2}.  ", 100, addMediaAndSetBudgetChoices );
            AddChoice( "added \"{1}\" ({2}) to the plan and changed its {3} to ${4:f2}.  ", 100, addMediaAndSetBudgetChoices );

            //// add media amd set two params
            //addMediaAndSetParam2Choices = new List<RandomChoice>();   // args: time, vehicle, type, paramName1, newVaue1, paramName2, newVaue2
            //AddChoice( "added \"{1}\" ({2}) to the plan, and set its {3} to \"{4}\" and {5} to \"{6}\".  ", 100, addMediaAndSetParam2Choices );
            //AddChoice( "added \"{1}\" ({2}) to the plan, and then set its {3} to \"{4}\" and {5} to \"{6}\".  ", 100, addMediaAndSetParam2Choices );
            //AddChoice( "added \"{1}\" ({2}) to the plan, and changed its {3} to \"{4}\" and {5} to \"{6}\".  ", 100, addMediaAndSetParam2Choices );

            // accepted suggesion for improvement
            acceptedSuggestionChoices = new List<RandomChoice>();   // args: time, suggestion, plan name, new plan name
            AddChoice( "You chose the \"{1}\" suggestion for improving your plan, which automatically created the new plan named \"{3}\".  ", 100, acceptedSuggestionChoices );
            AddChoice( "You chose the \"{1}\" suggestion for improving \"{2}\", which automatically created the new plan named \"{3}\".  ", 100, acceptedSuggestionChoices );
            AddChoice( "You accepted the \"{1}\" suggestion for improving your plan, which automatically created the new plan named \"{3}\".  ", 100, acceptedSuggestionChoices );
            AddChoice( "You accepted the \"{1}\" suggestion for improving \"{2}\", which automatically created the new plan named \"{3}\".  ", 100, acceptedSuggestionChoices );
        }

        private void AddChoice( string text, int weight, List<RandomChoice> list ) {
            list.Add( new RandomChoice( text, weight ) );
        }


    #region Constructor
        public GeneralStoryGenCore( List<StoryGenerator.StoryElement> storyElements, DateTime initializationTime )
            : base( storyElements, initializationTime ) {

            InitChoices();
        }
    #endregion

    #region Primary Loop
        public override string StoryText() {
            string s = "";
            bool useConnector = true;
            rand = new Random( (int)this.genInitializationTime.Ticks );

            for( int i = 0; i < storyElements.Count; i++ ) {
                StoryGenerator.StoryElement se = storyElements[ i ];

                if( se is StoryGenerator.UserLoginElement ) {
                     waitingForLogin = false;
                }
                else if( waitingForLogin == true ) {
                    continue;
                }

                // look for anticipated 3-event sequences
                if( i < storyElements.Count - 2 ) {
                    StoryGenerator.StoryElement nextSe = storyElements[ i + 1 ];
                    StoryGenerator.StoryElement next2Se = storyElements[ i + 2 ];

                    // add 3 media items in succession
                    if( se is StoryGenerator.AddMediaElement && nextSe is StoryGenerator.AddMediaElement && next2Se is StoryGenerator.AddMediaElement ) {
                        StoryGenerator.AddMediaElement ame1 = se as StoryGenerator.AddMediaElement;
                        StoryGenerator.AddMediaElement ame2 = nextSe as StoryGenerator.AddMediaElement;
                        StoryGenerator.AddMediaElement ame3 = next2Se as StoryGenerator.AddMediaElement;
                        s += AddMediaText( ame1.VehicleName, ame1.MediaType, ame2.VehicleName, ame2.MediaType, ame3.VehicleName, ame3.MediaType, ame1.PlanName, useConnector, ame1.TimeStamp );
                        i += 2;
                        continue;
                    }

                    ////// add media items and them modify 2 params  -- !!!this need some way to handle budget/other combinations!!!
                    ////if( se is StoryGenerator.AddMediaElement && nextSe is StoryGenerator.ModifyMediaElement && next2Se is StoryGenerator.ModifyMediaElement ) {
                    ////    StoryGenerator.AddMediaElement ame = se as StoryGenerator.AddMediaElement;
                    ////    StoryGenerator.ModifyMediaElement mme = nextSe as StoryGenerator.ModifyMediaElement;
                    ////    StoryGenerator.ModifyMediaElement mme2 = next2Se as StoryGenerator.ModifyMediaElement;
                    ////    s += AddMediaAndSetParamText( ame.VehicleName, ame.MediaType, mme.ParamName, mme.NewValueStr, mme2.ParamName, mme2.NewValueStr, useConnector, mme2.TimeStamp );
                    ////    i += 2;
                    ////    continue;
                    ////}
                }

                // look for anticipated 2-event sequences
                if( i < storyElements.Count - 1 ) {
                    StoryGenerator.StoryElement nextSe = storyElements[ i + 1 ];

                    ////// login followed by page view
                    ////if( se is StoryGenerator.UserLoginElement && nextSe is StoryGenerator.PageEventElement ) {
                    ////    StoryGenerator.UserLoginElement ule = se as StoryGenerator.UserLoginElement;
                    ////    StoryGenerator.PageEventElement pe = nextSe as StoryGenerator.PageEventElement;
                    ////    s += LoggedInToPage( ule.UserEmail, pe.PageName, ule.TimeStamp );
                    ////    i += 1;
                    ////    continue;
                    ////}

                    // create campaign followed by create plan
                    if( se is StoryGenerator.CreateCampaignElement && nextSe is StoryGenerator.CreatePlanElement ) {
                        StoryGenerator.CreateCampaignElement cce = se as StoryGenerator.CreateCampaignElement;
                        StoryGenerator.CreatePlanElement cpe = nextSe as StoryGenerator.CreatePlanElement;
                        s += CreatedCampaignAndPlanText( cce.CampaignName, cpe.PlanName, cpe.TimeStamp );
                        i += 1;
                        continue;
                    }

                    // start sim followed by sim done
                    if( se is StoryGenerator.SimulationStartedElement && nextSe is StoryGenerator.SimulationDoneElement ) {
                        StoryGenerator.SimulationStartedElement sse = se as StoryGenerator.SimulationStartedElement;
                        StoryGenerator.SimulationDoneElement sde = nextSe as StoryGenerator.SimulationDoneElement;
                        double runTime = (sde.TimeStamp - sse.TimeStamp).TotalSeconds;
                        s += RanSimulationText( sse.PlanName, runTime, sde.OverallStars, sse.TimeStamp );
                        i += 1;
                        continue;
                    }

                    // add 2 media items in succession
                    if( se is StoryGenerator.AddMediaElement && nextSe is StoryGenerator.AddMediaElement ) {
                        StoryGenerator.AddMediaElement ame1 = se as StoryGenerator.AddMediaElement;
                        StoryGenerator.AddMediaElement ame2 = nextSe as StoryGenerator.AddMediaElement;
                        s += AddMediaText( ame1.VehicleName, ame1.MediaType, ame2.VehicleName, ame2.MediaType, ame2.PlanName, useConnector, ame1.TimeStamp );
                        i += 1;
                        continue;
                    }

                    // add media items and them modify param
                    if( se is StoryGenerator.AddMediaElement && nextSe is StoryGenerator.ModifyMediaElement ) {
                        StoryGenerator.AddMediaElement ame = se as StoryGenerator.AddMediaElement;
                        StoryGenerator.ModifyMediaElement mme = nextSe as StoryGenerator.ModifyMediaElement;
                        s += AddMediaAndSetParamText( ame.VehicleName, ame.MediaType, mme.ParamName, mme.NewValueStr, useConnector, mme.TimeStamp );
                        i += 1;
                        continue;
                    }
                }

                if( se is StoryGenerator.UserLoginElement ) {
                    StoryGenerator.UserLoginElement ule = se as StoryGenerator.UserLoginElement;
                    s += UserLoginText( ule.UserEmail, se.TimeStamp );
                    if( i == storyElements.Count - 1 ) {
                        // if there are no more items in the story, add the special login "welcome"
                        if( this.LineBreak == "<br>" ) {
                            // only show this in the web-page version, not the email
                            s += LoginWelcomeText();
                        }
                    }
                    continue;
                }

               ////if( se is StoryGenerator.PageEventElement ) {
               ////     StoryGenerator.PageEventElement pe = se as StoryGenerator.PageEventElement;
               ////     s += PageElementText( pe.PageName, se.TimeStamp );
               //// }

                else if( se is StoryGenerator.GeneralInfoElement ) {
                    StoryGenerator.GeneralInfoElement gie = se as StoryGenerator.GeneralInfoElement;
                    s += GeneralInfoElementText( gie.Info, se.TimeStamp );
                }

                else if( se is StoryGenerator.CreateCampaignElement ) {
                    StoryGenerator.CreateCampaignElement cce = se as StoryGenerator.CreateCampaignElement;
                    s += CreateCampaignText( cce.CampaignName, se.TimeStamp );
                }

                else if( se is StoryGenerator.CreatePlanElement ) {
                    StoryGenerator.CreatePlanElement cpe = se as StoryGenerator.CreatePlanElement;
                    s += CreatePlanText( cpe.PlanName, se.TimeStamp );
                }

                else if( se is StoryGenerator.AutoCreatePlanElement ) {
                    StoryGenerator.AutoCreatePlanElement acpe = se as StoryGenerator.AutoCreatePlanElement;
                    s += AutoCreatePlanText( acpe.PlanName, se.TimeStamp );
                }

                else if( se is StoryGenerator.AddMediaElement ) {
                    StoryGenerator.AddMediaElement ame = se as StoryGenerator.AddMediaElement;
                    s += AddMediaText( ame.VehicleName, ame.MediaType, ame.PlanName, useConnector, se.TimeStamp );
                }

                ////else if( se is StoryGenerator.SimulationStartedElement ) {
                ////    StoryGenerator.SimulationStartedElement sse = se as StoryGenerator.SimulationStartedElement;
                ////    s += SimulationStartedText( sse.PlanName, se.TimeStamp );
                ////}

                else if( se is StoryGenerator.SimulationDoneElement ) {
                    StoryGenerator.SimulationDoneElement sde = se as StoryGenerator.SimulationDoneElement;
                    s += SimulationDoneText( sde.PlanName, sde.OverallStars, se.TimeStamp );
                }

                else if( se is StoryGenerator.SuggestionAcceptedElement ) {
                    StoryGenerator.SuggestionAcceptedElement sae = se as StoryGenerator.SuggestionAcceptedElement;
                    s += SuggestionAcceptedText( sae.Suggestion, sae.PlanName, sae.NewPlanName, se.TimeStamp );
                    useConnector = false;
                }

                else if( se is StoryGenerator.ModifyPlanElement ) {
                    StoryGenerator.ModifyPlanElement mpe = se as StoryGenerator.ModifyPlanElement;
                    s += ModifyPlanText( mpe.ParamName, mpe.OldValueStr, mpe.NewValueStr, se.TimeStamp );
                }

                else if( se is StoryGenerator.ModifyMediaElement ) {
                    StoryGenerator.ModifyMediaElement mme = se as StoryGenerator.ModifyMediaElement;
                    s += ModifyMediaText( mme.ParamName, mme.VehicleName, mme.OldValueStr, mme.NewValueStr, useConnector, se.TimeStamp );
                }
            }
            return s;
        }
    #endregion

    #region Low-level Utilities
        /// <summary>
        /// Gets the string version of the current time.  If only a short interval has elaspsed since the previous call, null is returned.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deltaSeconds"></param>
        /// <returns></returns>
        private string TimeStr( DateTime time, out int deltaSeconds ) {
            return TimeStr( time, false, out deltaSeconds );
        }
        
        /// <summary>
        /// Gets the string version of the current time.  If only a short interval has elaspsed since the previous call, null is returned.
        /// If detailed==true, a value is always retiurned, which includes seconds and AM/PM.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="detailed"></param>
        /// <param name="deltaSeconds"></param>
        /// <returns></returns>
        private string TimeStr( DateTime time, bool detailed, out int deltaSeconds ) {
            TimeSpan ts = time - prevTime;
            deltaSeconds = (int)Math.Round( ts.TotalSeconds );
            prevTime = time;

            bool showTime = false;
            // check the conditions for showing the time
            if( detailed == true || deltaSeconds > 60 ) {
                showTime = true;
            }

            if( showTime ) {
                if( detailed == false ) {
                    return time.ToString( "h:mm" );
                }
                else {
                    return time.ToString( "h:mm:sstt" );
                }
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// An item to choose from a set of choices
        /// </summary>
        private class RandomChoice {
            public string Text;
            public int Weight;

            public RandomChoice( string text, int weight ) {
                this.Text = text;
                this.Weight = weight;
            }
        }

        /// <summary>
        /// Total weight of all the choices
        /// </summary>
        /// <param name="choices"></param>
        /// <returns></returns>
        private int TotalWeight( List<RandomChoice> choices ) {
            int tot = 0;
            foreach( RandomChoice choice in choices ) {
                tot += choice.Weight;
            }
            return tot;
        }

        /// <summary>
        /// Returns a selection from the weighted list of choices.
        /// </summary>
        /// <param name="choices"></param>
        /// <returns></returns>
        private string GetRandomChoice( List<RandomChoice> choices ) {
            int weightSel = rand.Next( TotalWeight( choices ) );
            int cum = 0;
            for( int i = 0; i < choices.Count; i++ ) {
                cum += choices[ i ].Weight;
                if( cum > weightSel ) {
                    return choices[ i ].Text;
                }
            }
            // shouldn't get here - just in case
            return choices[ choices.Count - 1 ].Text;
        }

        private string FormatRandomChoice( List<RandomChoice> choices, params object[] args ){
            string fmt = GetRandomChoice( choices );
            string txt = String.Format( fmt, args );
            return txt;
        }
    #endregion

        private string ConnectorText( int secondsSincePrev ) {
            return GetRandomChoice( connectorChoices );
        }

    #region Element Text Methods
        // user login
        private string UserLoginText( string userName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            return FormatRandomChoice( loginChoices, t );
        }

        private string LoginWelcomeText() {
            return FormatRandomChoice( loginWelcomeChoices, "" );
        }

        private string LoggedInToPage( string userName, string pageName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            return FormatRandomChoice( loginToPageChoices, t, userName, pageName );
        }

        // created campaign then created plan
        private string CreatedCampaignAndPlanText( string campaignName, string planName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            return LineBreak + LineBreak + ConnectorText( deltaT ) + FormatRandomChoice( newCampaignAndPlanChoices, t, campaignName, planName );
        }

        // simulation started and then finished
        private string RanSimulationText( string planName, double runSeconds, double overallStars, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            return LineBreak + LineBreak + FormatRandomChoice( ranSimChoices, t, planName, runSeconds, overallStars );
        }

        // added a single media item
        private string AddMediaText( string vehicleName, string vehicleType, string planName, bool useConnector,  DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string hdr = "You ";
            if( useConnector ) {
                hdr = ConnectorText( deltaT );
            }
            return hdr + FormatRandomChoice( addMediaChoices, t, vehicleName, vehicleType );
        }

        // added two media items in succesion
        private string AddMediaText( string vehicle1Name, string vehicle1Type, string vehicle2Name, string vehicle2Type, string planName, bool useConnector,  DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string hdr = "You ";
            if( useConnector ) {
                hdr = ConnectorText( deltaT );
            }
            return hdr + FormatRandomChoice( addMedia2Choices, t, vehicle1Name, vehicle1Type, vehicle2Name, vehicle2Type );
        }

        // added three media items in succesion
        private string AddMediaText( string vehicle1Name, string vehicle1Type, string vehicle2Name, string vehicle2Type, string vehicle3Name, string vehicle3Type, string planName, bool useConnector, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string hdr = "You ";
            if( useConnector ) {
                hdr = ConnectorText( deltaT );
            }
            return hdr + FormatRandomChoice( addMedia3Choices, t, vehicle1Name, vehicle1Type, vehicle2Name, vehicle2Type, vehicle3Name, vehicle3Type );
        }

        // added media item and set a param
        private string AddMediaAndSetParamText( string vehicleName, string vehicleType, string paramName, string paramValue, bool useConnector, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string hdr = "You ";
            if( useConnector ) {
                hdr = ConnectorText( deltaT );
            }
            if( paramName != "budget" ) {
                return hdr + FormatRandomChoice( addMediaAndSetParamChoices, t, vehicleName, vehicleType, paramName, paramValue );
            }
            else {
                return hdr + FormatRandomChoice( addMediaAndSetBudgetChoices, t, vehicleName, vehicleType, paramName, paramValue );
            }
        }

        // added media item and set two  params
        private string AddMediaAndSetParamText( string vehicleName, string vehicleType, string param1Name, string param1Value, string param2Name, string param2Value, bool useConnector, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string hdr = "You ";
            if( useConnector ) {
                hdr = ConnectorText( deltaT );
            }
            return hdr + FormatRandomChoice( addMediaAndSetParam2Choices, t, vehicleName, vehicleType, param1Name, param1Value, param2Name, param2Value );
        }

        private string SuggestionAcceptedText( string suggestion, string planName, string newPlanName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            return LineBreak + LineBreak + FormatRandomChoice( acceptedSuggestionChoices, t, suggestion, planName, newPlanName );
        }


        private string PageElementText( string pageName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, out deltaT );
            string head = "";
            if( t != null ) {
                head = FormatRandomChoice( startChoices, t );
            }
            else {
                head = ConnectorText( deltaT );
            }
            return FormatRandomChoice( pageSentenceChoices, head, pageName );
        }

        private string GeneralInfoElementText( string info, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, out deltaT );

            string txt = null;
            if( t != null ) {
                string fmt = "<br>At {1}, {0}. ";
                txt = String.Format( fmt, info, t );
            }
            else {
                string fmt = "Then {0}. ";
                txt = String.Format( fmt, info );
            }

            return txt;
        }

        private string CreateCampaignText( string campaignName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, out deltaT );
            string txt = null;
            if( t != null ) {
                string fmt = "<br>At {1}, you created the campaign named \"{0}\". ";
                txt = String.Format( fmt, campaignName, t );
            }
            else {
                string fmt = "Then you created the campaign named \"{0}\".";
                txt = String.Format( fmt, campaignName );
            }

            return txt;
        }

        private string CreatePlanText( string planName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, out deltaT );
            string txt = null;
            if( t != null ) {
                string fmt = "<br>At {1}, you created the plan named \"{0}\". ";
                txt = String.Format( fmt, planName, t );
            }
            else {
                string fmt = "Then you created the plan named \"{0}\". ";
                txt = String.Format( fmt, planName );
            }

            this.prevPlanName = planName;
            return txt;
        }

        private string AutoCreatePlanText( string planName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, out deltaT );
            string txt = null;
            if( t != null ) {
                string fmt = "<br>At {1}, you had AdPlanit generate a plan named \"{0}\".";
                txt = String.Format( fmt, planName, t );
            }
            else {
                string fmt = "Then you had AdPlanit generate a plan named \"{0}\".";
                txt = String.Format( fmt, planName );
            }

            this.prevPlanName = planName;
            return txt;
        }


        private string SimulationStartedText( string planName, DateTime time ) {
            int deltaT = -1;
            string t = TimeStr( time, true, out deltaT );
            string fmt = "<br><br>Started simulation for \"{0}\" at {1}. ";
            this.prevPlanName = planName;
            string txt = String.Format( fmt, planName, t );
            return txt;
        }

        private string SimulationDoneText( string planName, double overallStars, DateTime time ) {
            int deltaT = -1;
            string txt = null;
            string t = TimeStr( time, true, out deltaT );
            if( planName == this.prevPlanName ) {
                string fmt = "Simulation done at {0}, with {1:f0}-star results. ";
                txt = String.Format( fmt, t, overallStars );
            }
            else {
                string fmt = "Completed simulation for \"{0}\" at {1}, with {2:f0}-star results. ";
                txt = String.Format( fmt, planName, t, overallStars );
            }
            this.prevPlanName = planName;
            return txt;
        }

        private string ModifyPlanText( string paramName, string oldValue, string newValue, DateTime time ) {
            string txt = null;
            if( oldValue != null && oldValue != "" ) {
                string fmt = "You changed {0} for the plan from {1} to {2}. ";
                txt = String.Format( fmt, paramName, oldValue, newValue );
            }
            else {
                string fmt = "You changed {0} for the plan to {1}. ";
                txt = String.Format( fmt, paramName, newValue );
            }
            return txt;
        }

        private string ModifyMediaText( string paramName, string vehicleName, string oldValue, string newValue, DateTime time ) {
            return ModifyMediaText( paramName, vehicleName, oldValue, newValue, true, time );
        }

        private string ModifyMediaText( string paramName, string vehicleName, string oldValue, string newValue, bool useConmnector, DateTime time ) {
            string txt = null;
            if( paramName != "budget" ) {
                if( oldValue != null && oldValue != "" ) {
                    string fmt = "You changed {0} for {3} from \"{1}\" to \"{2}\". ";
                    txt = String.Format( fmt, paramName, oldValue, newValue, vehicleName );
                }
                else {
                    string fmt = "You changed {0} for {2} to \"{1}\". ";
                    txt = String.Format( fmt, paramName, newValue, vehicleName );
                }
            }
            else {
                // change to budget
                if( oldValue != null && oldValue != "" ) {
                    string fmt = "You changed {0} for {3} from ${1:f0} to ${2:f2}. ";
                    txt = String.Format( fmt, paramName, oldValue, newValue, vehicleName );
                }
                else {
                    string fmt = "You changed {0} for {2} to ${1:f2}. ";
                    txt = String.Format( fmt, paramName, newValue, vehicleName );
                }
            }
            return txt;
        }
    #endregion
    }
}
