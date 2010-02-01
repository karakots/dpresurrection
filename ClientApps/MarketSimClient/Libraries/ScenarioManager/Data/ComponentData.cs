using System;
using System.Collections;
using System.Text;
using System.Data;

using DecisionPower.MarketSim.ScenarioManagerLibrary;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public abstract class ComponentData
    {
        protected Model model;

        protected DataRow dataRow;

        public Channel Channel {
            get {
                int channelID = (int)dataRow[ "channel_id" ];
                return model.ChannelForID( channelID );
            }
            set {
                dataRow[ "channel_id" ] = value.ID;
            }
        }

        public double Awareness {
            get {
                if( dataRow.Table.Columns.Contains( "message_awareness_probability" ) ) {
                    return (double)dataRow[ "message_awareness_probability" ];
                }
                else {
                    return 0;
                }
            }
            set {
                if( dataRow.Table.Columns.Contains( "message_awareness_probability" ) ) {
                    dataRow[ "message_awareness_probability" ] = value;
                }
            }
        }

        public double Persuasion {
            get {
                if( dataRow.Table.Columns.Contains( "message_persuation_probability" ) ) {
                    return (double)dataRow[ "message_persuation_probability" ];
                }
                else {
                    return 0;
                }
            }
            set {
                if( dataRow.Table.Columns.Contains( "message_persuation_probability" ) ) {
                    dataRow[ "message_persuation_probability" ] = value;
                }
            }
        }

        public DateTime StartDate {
            get {
                return (DateTime)dataRow[ "start_date" ];
            }
            set {
                dataRow[ "start_date" ] = value;
            }
        }

        public DateTime EndDate {
            get {
                return (DateTime)dataRow[ "end_date" ];
            }
            set {
                dataRow[ "end_date" ] = value;
            }
        }

        public DataRow DataRow {
            get {
                return dataRow;
            }
            set {
                dataRow = value;
            }
        }

        public ComponentData( Model model , DataRow dataRow ) {
            this.model = model;
            this.dataRow = dataRow;
        }
    }
}
