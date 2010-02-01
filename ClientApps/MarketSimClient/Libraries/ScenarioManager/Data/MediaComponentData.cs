using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Data
{
    public class MediaComponentData : ComponentData
    {
        /// <summary>
        /// Total GRPs from these coupons during the component interval
        /// </summary>
        public double TotalGRPs {
            get {
                return (double)dataRow[ "attr_value_G" ];
            }
            set {
                dataRow[ "attr_value_G" ] = value;
            }
        }

        /// <summary>
        /// Computes daily GRPs for the current total GRPs, start date, and end date of the component.
        /// </summary>
        /// <returns>GRPs per day</returns>
        public double GetDailyGRPs() {
            TimeSpan interval = this.EndDate - this.StartDate;
            if( interval.TotalDays != 0 ) {
                return this.TotalGRPs / interval.TotalDays;
            }
            else {
                return this.TotalGRPs;     // daily GRPs is defined as equal to total GRPs if start=end
            }
        }

        /// <summary>
        /// Creates a new MediaComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public MediaComponentData( Model model, DataRow dataRow )
            : base( model, dataRow ) {
        }

        /// <summary>
        /// Creates a new MediaComponentData object with the specified values.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataRow"></param>
        public MediaComponentData( Model model, Product product, Channel channel, DateTime startDate, DateTime endDate, double TotalGRPs ) :
                base( model, null ){
            dataRow = model.ModelDb.Data.mass_media.NewRow();
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).start_date = startDate;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).end_date = endDate;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).product_id = product.ID;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).channel_id = channel.ID;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).attr_value_G = TotalGRPs;
            ((MrktSimDb.MrktSimDBSchema.mass_mediaRow)dataRow).media_type = "V";
        }
    }
}
