using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using MediaLibrary;
using WebLibrary.Timing;
using SimInterface;

namespace WebLibrary
{
    /// <summary>
    /// This is the primary building block for a media plan.
    /// </summary>
    [Serializable]
    public class MediaItem
    {
        #region Public Fields
        public Guid PlanID;

        #region list code

        public bool is_generic = false;
        public bool is_vehicle = true;

        public List<MediaItem> sub_items;

        #endregion


        public MediaVehicle.MediaType MediaType;
        public string MediaSubtype;
        public string VehicleName = "Generic";

        public int type_id;
        public int subtype_id;
        public Guid vehicle_id;

        //VehicleInfo
        public int Span = 1;
        public string Region = "USA";
        public string URL = "http://www.adplanit.com";

        //AdOption Options
        public List<DemographicLibrary.Demographic> Demographics = new List<DemographicLibrary.Demographic>();
        public List<string> Regions = new List<string>();
        public int TargetingLevel = -1;                    // TargetingLevel ranges from 0 to 40 and corresponds to slider posiiton in the UI
        public int NumImpressions = -1;
        #endregion

        public double vehicle_size;
        public double vehicle_cpm;
        public List<TimingInfo> TimingList;

        // rating
        public double VehicleRating;

        // for serialization
        public MediaItem() {
            this.TimingList = new List<TimingInfo>();
        }

        public MediaItem(MediaVehicle vehicle, AdOption option, MediaPlan media_plan)
        {
            MediaType = vehicle.Type;
            MediaSubtype = vehicle.SubType;
            VehicleName = vehicle.Vehicle;
            VehicleRating = vehicle.Rating;
            //AdOptionName = option.Name;

            type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
            subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);
            vehicle_id = vehicle.Guid;
            //ad_option_id = option.ID;

            Span = Utils.DaysInAdCycle(vehicle.Cycle);
            Region = vehicle.RegionName;
            URL = vehicle.URL;

            vehicle_cpm = vehicle.CPM;
            vehicle_size = vehicle.Size;

            this.TimingList = new List<TimingInfo>();
            AddTimingInfo( option );

            if (media_plan.DemographicCount > 0)
            {
                for (int i = 0; i < media_plan.DemographicCount; i++)
                {
                    Demographics.Add(SimUtils.ConvertToDemographic(media_plan, i));
                }
            }
            else
            {
                for (int i = 0; i < media_plan.SegmentCount; i++)
                {
                    Demographics.Add(media_plan.Specs.SegmentList[i]);
                }
            }

            for( int i = 0; i < media_plan.Specs.GeoRegionNames.Count; i++ ) 
            {
                Regions.Add( media_plan.Specs.GeoRegionNames[ i ] );
            }
        }

        public MediaItem( int type_id )
        {
            MediaType = Utils.MediaDatabase.GetTypeForID(type_id);
            this.type_id = type_id;
            this.sub_items = new List<MediaItem>();
        }

        public void AddTimingInfo(AdOption option)
        {
            TimingList.Add(new TimingInfo(type_id, option, vehicle_cpm, vehicle_size));
        }

        public void AddTimingInfo(int option_id)
        {
            AdOption option = Utils.MediaDatabase.GetAdOption(option_id);
            AddTimingInfo(option);
        }

        public void ClearTimingInfo() {
            TimingList = new List<TimingInfo>();
        }

        public void AddTimingInfo( AdOption option, List<Timing.TimingInfo.TimingInfoItem> timing_info ) {
            TimingInfo info = new TimingInfo( type_id, option, vehicle_cpm, vehicle_size );
            info.AddAirDates( timing_info );
            TimingList.Add( info );
        }

        public List<List<TimingDisplayItem>> GetTimingDisplay( DateTime newItemStartDate )
        {
            bool isSundayPaper = false;
            if( this.VehicleName.ToUpper().IndexOf( "SUNDAY" ) != -1 ) {
                isSundayPaper = true;
            }

            List<List<TimingDisplayItem>> rval = new List<List<TimingDisplayItem>>();
            for(int i = 0; i < TimingList.Count; i++)
            {
                rval.Add(new List<TimingDisplayItem>());
                List<TimingDisplayItem> dItems = TimingList[ i ].ConvertToDisplayItems( newItemStartDate, isSundayPaper );
                dItems = SortItemsByDate( dItems );
                rval[ i ].AddRange( dItems );
            }
            return rval;
        }

        private List<TimingDisplayItem> SortItemsByDate( List<TimingDisplayItem> dispItems ) {
            TimingDisplayItem[] itemList = new TimingDisplayItem[ dispItems.Count ];
            DateTime[] dateList = new DateTime[ dispItems.Count ];
            for( int i = 0; i < dispItems.Count; i++ ) {
                dateList[ i ] = new DateTime( dispItems[ i ].Year, dispItems[ i ].Month, dispItems[ i ].Day );
                itemList[ i ] = dispItems[ i ];
            }

            // sort by date
            Array.Sort( dateList, itemList );

            List<TimingDisplayItem> sortedList = new List<TimingDisplayItem>();
            for( int i = 0; i < dispItems.Count; i++ ) {
                sortedList.Add( itemList[ i ] );
            }
            return sortedList;
        }

        public int GetOptionIDAt( int index )
        {
            return TimingList[index].AdOptionID;
        }

        public string GetOptionNameAt( int index ) {
            return TimingList[ index ].AdOption;
        }

        public double GetOptionSpotPriceAt( int index ) {
            return TimingList[ index ].SpotPrice * this.TargetingPriceMultiplier;
        }

        public void RemoveTimingInfo( int index )
        {
            TimingList.RemoveAt(index);
        }

        public List<DateTime> GetAdDates()
        {
            List<DateTime> rval = new List<DateTime>();
            foreach (TimingInfo info in TimingList)
            {
                rval.AddRange(info.GetSpotDates());
            }
            rval.Sort();
            return rval;
        }

        public List<MediaComp> GetMediaComponents(DateTime start_date)
        {
          
            List<MediaComp> rval = new List<MediaComp>();
            foreach (TimingInfo info in TimingList)
            {
                foreach (TimingInfo.TimingInfoItem time in info.AdDates)
                {
                    if( time.AdCount > 0 )
                    {
                        if( this.MediaType == MediaVehicle.MediaType.Internet )
                        {
                            MediaComp comp = new MediaComp( this.MediaType );

                            comp.Guid = vehicle_id;
                            comp.ad_option = info.ad_option_id;

                            comp.Impressions = time.AdCount;
                            comp.StartDate = (time.Date - start_date).Days;

                            if( comp.StartDate < 0 )
                            {
                                comp.StartDate = 0;
                            }

                            comp.Span = Span;


                            comp.demo_fuzz_factor = TargetingLevel;
                            comp.region_fuzz_factor = TargetingLevel;
                            comp.target_demogrpahic = Demographics;
                            comp.target_regions = Regions;

                            rval.Add( comp );
                        }
                        else
                        {
                            for( int adDex = 0; adDex < time.AdCount; ++adDex )
                            {

                                MediaComp comp = new MediaComp( this.MediaType );

                                comp.Guid = vehicle_id;
                                comp.ad_option = info.ad_option_id;

                                comp.Impressions = 0;

                                DateTime adDate = time.Date;

                                if( this.MediaType == MediaVehicle.MediaType.Newspaper && this.VehicleName.ToUpper().IndexOf( "SUNDAY" ) != -1 )
                                {
                                    // if this is a Sunday newspaper, be sure the date is a Sunday.  If not, advance to the next Sunday.
                                    while( adDate.DayOfWeek != DayOfWeek.Sunday )
                                    {
                                        adDate = adDate.AddDays( 1 );
                                    }
                                }

                                comp.StartDate = (adDate - start_date).Days;

                                if( comp.StartDate < 0 )
                                {
                                    comp.StartDate = 0;
                                }

                                comp.Span = Span;

                                comp.demo_fuzz_factor = 0.0;
                                comp.region_fuzz_factor = 0.0;

                                rval.Add( comp );
                            }
                        }
                    }
                }
            }

            return rval;
        }

        public double TargetingPriceMultiplier {
            get {
                double t = Math.Max( 0, this.TargetingLevel );     // TargetingLevel ranges from 0 to 40
                double ratio = 1.0 + (t / 10.0);                          // corresponding price ration ranges from 1 to 4
                return ratio;
            }
        }

        public double TotalPrice
        {
            get
            {
                double total = 0.0;
                foreach (TimingInfo info in TimingList)
                {
                    total += info.TotalPrice;
                }
                total *= this.TargetingPriceMultiplier;
                return total;
            }
        }

        public int SpotCount
        {
            get
            {
                int total = 0;
                foreach (TimingInfo info in TimingList)
                {
                    total += info.SpotCount;
                }
                return total;
            }
        }

        public string AdOptionName
        {
            get
            {
                if (TimingList.Count == 0)
                {
                    return "None";
                }
                else if (TimingList.Count == 1)
                {
                    return TimingList[0].AdOption;
                }
                else
                {
                    return "Multiple";
                }
            }
        }

        public double Size
        {
            get
            {
                return vehicle_size;
            }
        }

        public double CPM
        {
            get
            {
                return vehicle_cpm;
            }
        }

        /// <summary>
        /// Returns a description of the overall timing of this item (for all prominences)
        /// </summary>
        /// <returns></returns>
        public string GetTimingSummary() {
            if( TimingList.Count > 0 ) {
                // we have at least one prominence
                int adCnt = TimingList[ 0 ].SpotCount;
                if( adCnt == 0 ) {
                    return "none scheduled (click to set)";
                }
                else if( adCnt == 1 ) {
                    // (we know thhere is at least one item in GetSpotDates since otherwise the SpotCount couldn't be nonzero)
                    return String.Format( "1 ad on {0}", TimingList[ 0 ].GetSpotDates()[ 0 ].ToShortDateString() );
                }                
                else {
                    // (we know thhere is at least one item in GetSpotDates since otherwise the SpotCount couldn't be nonzero)
                    return String.Format( "{0} ads starting {1}", adCnt, TimingList[ 0 ].GetSpotDates()[ 0 ].ToShortDateString() ); 
                }
            }
            else {
                // no prominence selected!
                return "-";
            }
        }
    }
}
