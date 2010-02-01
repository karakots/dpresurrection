using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using MrktSimDb;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class DistributionComponentData : ComponentData
    {
        /// <summary>
        /// The post-use distribution value during the component interval
        /// </summary>
        public double PostUseDistribution {
            get {
                return (double)dataRow[ "attr_value_G" ];
            }
            set {
                dataRow[ "attr_value_G" ] = value;
            }
        }

        /// <summary>
        /// Percent distribution during the component interval
        /// </summary>
        public double PercentDistribution {
            get {
                return (double)dataRow[ "attr_value_F" ];
            }
            set {
                dataRow[ "attr_value_F" ] = value;
            }
        }
   
        /// <summary>
        /// Creates a new DistributionComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public DistributionComponentData( Model model, DataRow dataRow )
            : base( model, dataRow ) {
        }
         
        /// <summary>
        /// Creates a new DistributionComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public DistributionComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate, 
            double percentDistribution, double postUseDistribution ) : base( model, null ){
            dataRow = model.ModelDb.Data.distribution.NewRow();
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).attr_value_F = percentDistribution;
            ((MrktSimDb.MrktSimDBSchema.distributionRow)dataRow).attr_value_G = postUseDistribution;
        }
    }
}
