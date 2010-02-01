using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebLibrary;

namespace BusinessLogic
{
    public class Validation
    {
        /// <summary>
        /// Checks if the proposed new budget makes sense (i.e. is adequate) for the current media plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="proposedNewBudget"></param>
        /// <returns></returns>
        public static bool CheckBudgetSanity( MediaPlan plan, double proposedNewBudget ) {

            //!!! TBD !!!

            if( proposedNewBudget > 0 ) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
