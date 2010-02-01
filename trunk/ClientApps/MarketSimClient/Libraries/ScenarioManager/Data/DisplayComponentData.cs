using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class DisplayComponentData : ComponentData
    {
        /// <summary>
        /// Percent display during the component interval
        /// </summary>
        public double PercentDisplay {
            get {
                return (double)dataRow[ "attr_value_F" ];
            }
            set {
                dataRow[ "attr_value_F" ] = value;
            }
        }
   
        /// <summary>
        /// Creates a new DisplayComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public DisplayComponentData( Model model, DataRow dataRow )
            : base( model, dataRow ) {
        }
 
        /// <summary>
        /// Creates a new DisplayComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public DisplayComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate, double percentDistribution )
            :
                base( model, null ){
            dataRow = model.ModelDb.Data.display.NewRow();
            ((MrktSimDb.MrktSimDBSchema.displayRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.displayRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.displayRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.displayRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.displayRow)dataRow).attr_value_F = percentDistribution;
        }
   }
}
