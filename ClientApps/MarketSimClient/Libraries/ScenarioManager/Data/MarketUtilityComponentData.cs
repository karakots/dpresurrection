using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class MarketUtilityComponentData : ComponentData
    {
        /// <summary>
        /// The market utility value during the component interval
        /// </summary>
        public double MarketUtilityValue {
            get {
                return (double)dataRow[ "utility" ];
            }
            set {
                dataRow[ "utility" ] = value;
            }
        }

        /// <summary>
        /// Percent distribution of this market utility during the component interval
        /// </summary>
        public double PercentDistribution {
            get {
                return (double)dataRow[ "percent_dist" ];
            }
            set {
                dataRow[ "percent_dist" ] = value;
            }
        }

        /// <summary>
        /// Creates a new MarketUtilityComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public MarketUtilityComponentData( Model model, DataRow dataRow )
            : base( model, dataRow ) {
        }
                         
        /// <summary>
        /// Creates a new MarketUtilityComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public MarketUtilityComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate, 
                            double marketUtilityValue, double percentDistribution ) : base( model, null ){
            dataRow = model.ModelDb.Data.market_utility.NewRow();
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).utility = marketUtilityValue;
            ((MrktSimDb.MrktSimDBSchema.market_utilityRow)dataRow).percent_dist = percentDistribution;
        }
    }
}
