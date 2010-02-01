using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using MediaLibrary;

namespace WebLibrary.Timing
{
    /// <summary>
    /// TimingItemConverter converts between a list of (generic) TimingInfoItem objects and a list of (media-type-specific) TimingDisplayItem objects.
    /// </summary>
    public class TimingItemConverter
    {
        /// <summary>
        /// Converts a list of TimingInfoItem objects into an equivalent set of TimingDisplayItem objects suitable for the given media type.
        /// </summary>
        /// <param name="timingInfoItems"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static List<TimingDisplayItem> ConvertToUIItems( List<TimingInfo.TimingInfoItem> timingInfoItems, MediaVehicle.MediaType mediaType, 
            DateTime newItemStartDate, bool isSundayPaper ) {
            List<TimingDisplayItem> dispItems = new List<TimingDisplayItem>();

            // make a copy of the list of timing info items
            List<TimingInfo.TimingInfoItem> tempList = new List<TimingInfo.TimingInfoItem>();
            for( int i = 0; i < timingInfoItems.Count; i++ ) {
                tempList.Add( new TimingInfo.TimingInfoItem( timingInfoItems[ i ].Date, timingInfoItems[ i ].AdCount ) );
            }

            do {
                TimingDisplayItem tItem = CreateTimingDisplayItem( mediaType, newItemStartDate, isSundayPaper );

                // convert one or more timing info items into this tming display item
                List<TimingInfo.TimingInfoItem> remainingItems = tItem.Parse( tempList );

                dispItems.Add( tItem );

                tempList = remainingItems;

            } while( tempList.Count > 0 );

            return dispItems;
        }

        /// <summary>
        /// Converts the UI inputs for the given media type into the equivalent list of timing info objects.
        /// </summary>
        /// <param name="timingInfoItems"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static void ParseUserTimingInputs( HttpRequest request, MediaItem media_item ) {
            MediaVehicle.MediaType mediaType = media_item.MediaType;
            int nMax = int.Parse( request[ "TimingItemCount" ] );

            Dictionary<int, List<TimingInfo.TimingInfoItem>> dict = new Dictionary<int, List<TimingInfo.TimingInfoItem>>();

            for( int i = 0; i < nMax; i++ ) {
                TimingDisplayItem tItem = CreateTimingDisplayItem( mediaType );
                int adOptionID;
                try {
                    tItem.Parse( request, i, out adOptionID );

                    int adOptionIdentifier = adOptionID;
                    foreach( TimingInfo tInfo in media_item.TimingList ) {
                        if( tInfo.AdOptionID == adOptionIdentifier ) {
                            tItem.SpotPrice = tInfo.SpotPrice;
                            break;
                        }
                    }

                    if( dict.ContainsKey( adOptionIdentifier ) == false ) {
                        List<TimingInfo.TimingInfoItem> newTimingItems = new List<TimingInfo.TimingInfoItem>();
                        dict.Add( adOptionIdentifier, newTimingItems );
                    }

                    List<TimingInfo.TimingInfoItem> timingItems = dict[ adOptionIdentifier ];
                    timingItems.AddRange( tItem.ToTimingInfo() );
                }
                catch( Exception ) {
                    //??? not sure what  to do if the inputs cannot be parsed!!!
                }
            }

            List<TimingInfo> infoItems = new List<TimingInfo>();
            // convert from the dictionary
            media_item.ClearTimingInfo();
            foreach( int key in dict.Keys ) {
                AdOption opt = Utils.MediaDatabase.GetAdOption(key);
                media_item.AddTimingInfo(opt, dict[key]);
            }
        }

        /// <summary>
        /// Use this version if the default date setting if the item is not important
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private static TimingDisplayItem CreateTimingDisplayItem( MediaVehicle.MediaType mediaType ) {
            return CreateTimingDisplayItem( mediaType, DateTime.Now, false );
        }

        /// <summary>
        /// Creates and returns an object of the appropriate subclass of TimingDisplayItem for the given media type.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private static TimingDisplayItem CreateTimingDisplayItem( MediaVehicle.MediaType mediaType, DateTime newItemStartDate, bool isSundayNewspaper ) {
            TimingDisplayItem tItem = null;
            switch( mediaType ) {
                case MediaVehicle.MediaType.Internet:
                    tItem = new InternetTimingDisplayItem( newItemStartDate );
                    break;
                case MediaVehicle.MediaType.Magazine:
                    tItem = new MagazineTimingDisplayItem( newItemStartDate );
                    break;
                case MediaVehicle.MediaType.Newspaper:
                    if( isSundayNewspaper == false ) {
                        tItem = new NewspaperTimingDisplayItem( newItemStartDate );
                    }
                    else {
                        tItem = new SundayNewspaperTimingDisplayItem( newItemStartDate );
                    }
                    break;
                case MediaVehicle.MediaType.Radio:
                    tItem = new RadioTimingDisplayItem( newItemStartDate );
                    break;
                case MediaVehicle.MediaType.Yellowpages:
                    tItem = new YellowpagesTimingDisplayItem( newItemStartDate );
                    break;
            }
            return tItem;
        }
    }
}
