using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Enums
    {
        public enum CouponType
        {
            Regular,
            BOGO
        }
          
        public enum DataIntervalCheckType
        {
            /// <summary>
            /// To match, a data component must end within the test interval (interval end date is within the interval)
            /// </summary>
            DataEndsInInterval,   
        
            /// <summary> 
            /// To match, a data component interval must start within the test interval (interval start date is within the interval)
            /// </summary>
            DataStartsInInterval,         

            /// <summary>
            /// To match, a data component start and end dates must both be within the test interval (interval start and end dates are within the interval)
            /// </summary>
            DataEntirelyWithinInterval
        }
    }
}
