using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class ErrorInfo
    {
        private string summary;
        private string details;
        private DateTime occurrenceTime;

        public string Summary {
            get {
                return this.summary;
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

        public ErrorInfo( string summary, string details ) {
            this.summary = summary;
            this.details = details;
            occurrenceTime = DateTime.Now;
        }
    }
}
