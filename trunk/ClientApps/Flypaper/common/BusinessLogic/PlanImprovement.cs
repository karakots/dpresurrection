using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebLibrary;

namespace BusinessLogic
{
    public class PlanImprovement
    {
        /// <summary>
        /// Returns the list of improvements that the system suggests for the given plan (which is assumed to have results).
        /// </summary>
        /// <returns></returns>
        public static List<Suggestion> GetImprovementSuggestions( MediaPlan plan ) {

            //!!! DEMO !!!
            List<Suggestion> suggestions = new List<Suggestion>();

            //Suggestion s0 = new Suggestion( "Debug Suggestion 1" );
            //MediaPlan debugModifiedPlan = new MediaPlan( plan, plan.PlanName + " 2x mod" );
            //foreach( MediaItem genericType in debugModifiedPlan.MediaItems ) {
            //    foreach( MediaItem subType in genericType.SubItems ) {
            //        subType.ItemBudget = subType.ItemBudget * 2;
            //    }
            //}
            //s0.Items.Add( new SuggestionItem( "Try 2x total budget", debugModifiedPlan ) );
            //suggestions.Add( s0 );

            Suggestion s2 = new Suggestion( "Your plan is generating good awareness among the consumers reached, but marginal persuasion (influence to choose your product)." +
                " If you wish to increase this influence, consider:" );
            MediaPlan debugModifiedPlan = new MediaPlan( plan, plan.PlanName + " plus radio" );
            s2.Items.Add( new SuggestionItem( "Adding more investment in awareness-generating media like radio, shifting away from  persuasion-generating media like magazines if necessary to keeu under budget.", debugModifiedPlan ) );
            suggestions.Add( s2 );            
            
            Suggestion s1 = new Suggestion( "Your plan is reaching the right target, but only 35% of the target (150,000 people).  If you would like to reach more, try this:" );
            s1.Items.Add( new SuggestionItem( "Try advertising in more magazines.", null ) );
            s1.Items.Add( new SuggestionItem( "If you can afford to, maintain the current spending per magazine as you increase the number of magazines.", null ) );
            suggestions.Add( s1 );

            // !!! END DEMO !!!

            return suggestions;
        }

        /// <summary>
        /// Encapsulates all plan improvement suggestions of a general type.
        /// </summary>
        public class Suggestion
        {
            public string DescriptionText { set; get; }
            public List<SuggestionItem> Items { set; get; }

            public Suggestion( string description ) {
                this.DescriptionText = description;
                this.Items = new List<SuggestionItem>();
            }
        }

        /// <summary>
        /// Encapsulates a single plan improvement suggestion.  
        /// </summary>
        public class SuggestionItem
        {
            public string DescriptionText { set;  get; }
            public MediaPlan ModifiedPlan { set; get; }

            public SuggestionItem( string descriptionText, MediaPlan modifiedPlan ) {
                this.DescriptionText = descriptionText;
                this.ModifiedPlan = modifiedPlan;
            }
        }
    }
}
