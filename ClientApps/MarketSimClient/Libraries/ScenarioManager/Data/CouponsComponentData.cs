using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class CouponsComponentData : ComponentData
    {
        /// <summary>
        /// Percent of consumers receiving these coupons
        /// </summary>
        public double PercentReceiving {
            get {
                return (double)dataRow[ "attr_value_G" ];
            }
            set {
                dataRow[ "attr_value_G" ] = value;
            }
        }

        /// <summary>
        /// Probability a coupon being redeemed
        /// </summary>
        public double RedemptionProbability {
            get {
                return (double)dataRow[ "attr_value_I" ];
            }
            set {
                dataRow[ "attr_value_I" ] = value;
            }
        }

        /// <summary>
        /// Creates a new CouponsComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public CouponsComponentData( Model model, DataRow dataRow )
            : base( model, dataRow ) {
        }

        /// <summary>
        /// Creates a new CouponsComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public CouponsComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate, 
            double PercentReceiving, double redemptionRate )
            :
                base( model, null ){
            dataRow = model.ModelDb.Data.mass_media.NewRow();
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).attr_value_G = PercentReceiving;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).attr_value_I = redemptionRate;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).media_type = "C";
        }
    }
}
