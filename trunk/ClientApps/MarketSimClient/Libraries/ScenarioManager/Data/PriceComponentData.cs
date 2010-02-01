using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class PriceComponentData : ComponentData
    {
        /// <summary>
        /// The price value during the component interval
        /// </summary>
        public double Price {
            get {
                return (double)dataRow[ "price" ];
            }
            set {
                dataRow[ "price" ] = value;
            }
        }

        /// <summary>
        /// The price type (promoted, unpromoted, etc.)
        /// </summary>
        public int PriceType {
            get {
                int price_type_id = (int)dataRow[ "price_type" ];

                return price_type_id;
            }
            set {
                    dataRow["price_type"] = value;
            }
        }

        /// <summary>
        /// Percent distribution of this price type during the component interval
        /// </summary>
        public double PercentDistribution {
            get {
                return (double)dataRow[ "percent_SKU_in_dist" ];
            }
            set {
                dataRow[ "percent_SKU_in_dist" ] = value;
            }
        }

        /// <summary>
        /// Creates a new PriceComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public PriceComponentData( Model model, DataRow dataRow ) : base( model, dataRow ) {
        }

                 
        /// <summary>
        /// Creates a new PriceComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public PriceComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate,
                                            double price, int price_type_id, double percentDistribution )
            : base( model, null ) {
            dataRow = model.ModelDb.Data.product_channel.NewRow();
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).percent_SKU_in_dist = percentDistribution;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).price = price;
            ((MrktSimDb.MrktSimDBSchema.product_channelRow)dataRow).price_type = price_type_id;
        }
    }
}
