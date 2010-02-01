using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class MrktSimException : Exception
    {
        private string details;
        private DateTime occurrenceTime;

        public string Summary {
            get {
                return this.Message;
            }
        }

        public string Details {
            get {
                return this.details;
            }
        }

        public DateTime Timestamp {
            get {
                return this.occurrenceTime;
            }
        }

        public MrktSimException( string summary, string details )
            : base( summary ) {

            this.details = details;
            occurrenceTime = DateTime.Now;
        }
    }
}
