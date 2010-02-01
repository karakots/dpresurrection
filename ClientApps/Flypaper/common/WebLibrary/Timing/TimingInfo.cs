using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaLibrary;

namespace WebLibrary.Timing
{
    /// <summary>
    /// Each TimingInfo object specifies all of the ad times for a particular ad option (prominence) of a MediaItem.
    /// </summary>
    [Serializable]
    public class TimingInfo
    {
        public string AdOption
        {
            get
            {
                return ad_option_name;
            }
        }
        public int AdOptionID
        {
            get
            {
                return ad_option_id;
            }
        }

        public int SpotCount
        {
            get
            {
                int total = 0;
                foreach (TimingInfoItem item in AdDates)
                {
                    total += item.AdCount;
                }
                return total;
            }
        }
        public double SpotPrice
        {
            get
            {
                double spot_count = Math.Max(1, SpotCount);
                return Utils.GetSpotPrice(type.ToString(), vehicle_cpm, ad_option_cost_modifier, vehicle_size / 1000, spot_count);
            }
        }
        public double TotalPrice
        {
            get
            {
                double spot_count = SpotCount;
                return spot_count * Utils.GetSpotPrice(type.ToString(), vehicle_cpm, ad_option_cost_modifier, vehicle_size / 1000, spot_count);
            }
        }

        // NOTE: these properties must be public so they will be serialized with the XML serializer!
        public List<TimingInfoItem> AdDates;

        public int ad_option_id;
        public string ad_option_name;

        public double ad_option_cost_modifier;
        public double vehicle_cpm;
        public double vehicle_size;
        public int type_id;
        public MediaVehicle.MediaType type;

        public TimingInfo() {
        }

        public TimingInfo( int type_id, AdOption option, double vehicle_cpm, double vehicle_size) {
            this.ad_option_name = option.Name;
            this.ad_option_id = option.ID;
            this.vehicle_cpm = vehicle_cpm;
            this.vehicle_size = vehicle_size;
            this.type_id = type_id;
            this.type = Utils.MediaDatabase.GetTypeForID(type_id);
            ad_option_cost_modifier = 1.0;
            SimpleOption simp_option = option as SimpleOption;
            if (simp_option != null)
            {
                ad_option_cost_modifier = simp_option.Cost_Modifier;
            }
            
            this.AdDates = new List<TimingInfoItem>();
        }

        public void ChangeOption( int option_id ) {
            AdOption option = Utils.MediaDatabase.GetAdOption( option_id );
            ChangeOption(option);
        }

        public void ChangeOption(AdOption option)
        {
            this.ad_option_name = option.Name;
            this.ad_option_id = option.ID;
            ad_option_cost_modifier = 1.0;
            SimpleOption simp_option = option as SimpleOption;
            if (simp_option != null)
            {
                ad_option_cost_modifier = simp_option.Cost_Modifier;
            }
        }

        public void AddAirDate(DateTime date, int ad_count)
        {
            AdDates.Add(new TimingInfoItem(date, ad_count));
        }

        public void AddAirDate(TimingInfoItem item)
        {
            AdDates.Add(item);
        }

        public void AddAirDates(List<TimingInfoItem> items)
        {
            AdDates.AddRange(items);
        }

        public void ReplaceAirDates(List<TimingInfoItem> items)
        {
            AdDates.Clear();
            AdDates.AddRange(items);
        }

        public List<TimingDisplayItem> ConvertToDisplayItems( DateTime newItemStartDate, bool isSundayNewspaper )
        {
            return TimingItemConverter.ConvertToUIItems( AdDates, type, newItemStartDate, isSundayNewspaper );
        }

        public List<DateTime> GetSpotDates()
        {
            List<DateTime> rval = new List<DateTime>();
            foreach (TimingInfoItem item in AdDates)
            {
                rval.Add(item.Date);
            }
            rval.Sort();
            return rval;
        }

        /// <summary>
        /// Each TimingInfoItem object specifies a date, and the number of ads on that date.
        /// </summary>
        ///
        [Serializable]
        public class TimingInfoItem
        {
            public DateTime Date;
            public int AdCount;

            public TimingInfoItem() {
            }

            public TimingInfoItem( DateTime date, int adCount ) {
                this.Date = date;
                this.AdCount = adCount;
            }
        }
    }
}
