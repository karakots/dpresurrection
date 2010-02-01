using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebLibrary;

namespace BusinessLogic
{
    /// <summary>
    /// PlanImprover generates modified MediaPlan items based on an existing plan and a suggested change.
    /// </summary>
    public class PlanImprover
    {
        private MediaPlan plan;

        /// <summary>
        /// Create an improved-plan generator object.
        /// </summary>
        /// <param name="existingPlan"></param>
        /// <param name="newPlanName"></param>
        /// <param name="allMediaPlans"></param>
        public PlanImprover( MediaPlan existingPlan, string newPlanName, string specialUserInstructions, List<MediaPlan> allMediaPlans ) {
            string uniqueNewPlanName = Utils.NewPlanName( newPlanName, allMediaPlans );
            this.plan = new MediaPlan( existingPlan, uniqueNewPlanName );
            this.plan.SpecialInstructions = specialUserInstructions;
        }

        /// <summary>
        /// Get the improved plan for this PlanImprover.
        /// </summary>
        /// <returns></returns>
        public MediaPlan GetImprovedPlan() {
            return this.plan;
        }       

    }
}
