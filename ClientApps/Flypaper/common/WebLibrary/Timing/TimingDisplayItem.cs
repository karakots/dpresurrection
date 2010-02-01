using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

//JimJ - Edited to force CVS to update

namespace WebLibrary.Timing
{
    /// <summary>
    /// TimingDisplayItem corresponds to a row (or expandable/contractable multi-row set) in the main Timing display
    /// </summary>
    public abstract class TimingDisplayItem
    {
        public bool Continuous;

        public int Day;
        public int Month;
        public int Year;

        public double SpotPrice;

        public TimingDisplayItem() {
        }

        public TimingDisplayItem( DateTime defaultStart ) {
            this.Day = defaultStart.Day;
            this.Month = defaultStart.Month;
            this.Year = defaultStart.Year;
        }

        /// <summary>
        /// Parses the values for this timing display item from the list of TimingInfo, and returns the list of remaining TimingInfo objects.
        /// </summary>
        /// <param name="timingInfoList"></param>
        /// <returns>The unused input items.</returns>
        public abstract List<TimingInfo.TimingInfoItem> Parse( List<TimingInfo.TimingInfoItem> timingInfoList );

        /// <summary>
        /// Counts up all of the ads which occur in a continuous series in the given list and increments the totalAds value accordingly.  
        /// What is considered "continuous" is dependent on the given ad cycle.  The return value is the number of items in the timing info list that were traversed 
        /// while accumulating ads (often the next action will be removing these items friom the list).
        /// </summary>
        /// <param name="timingInfoList"></param>
        /// <param name="fromDate"></param>
        /// <param name="totalAds"></param>
        /// <param name="adCycle"></param>
        /// <returns></returns>
        public int GetAllAdsInPulse( ref int totalAds, ref int durationDays, List<TimingInfo.TimingInfoItem> timingInfoList, DateTime fromDate, MediaLibrary.MediaVehicle.AdCycle adCycle ) {

            // set the definition of "continuous" as a maximum days difference between ad times
            int limitDelta = 1;
            switch( adCycle ) {
                case MediaLibrary.MediaVehicle.AdCycle.Weekly:
                    limitDelta = 8;
                    break;
                case MediaLibrary.MediaVehicle.AdCycle.Bimonthly:
                    limitDelta = 20;
                    break;
                case MediaLibrary.MediaVehicle.AdCycle.Monthly:
                    limitDelta = 40;
                    break;
                case MediaLibrary.MediaVehicle.AdCycle.Yearly:
                    limitDelta = 400;
                    break;
            }

            int nRemoved = 0;
            if( timingInfoList.Count > 0 ) {

                for( int i = 0; i < timingInfoList.Count; i++ ) {
                    if( timingInfoList[ i ].Date < fromDate ) {
                        continue;   // ignore items occurring before the given start date
                    }
                    TimeSpan delta = timingInfoList[ i ].Date - fromDate;

                    // as long as the ads keep coming on successive intervals, keep accumulating
                    // also break if we find ads on the exact same day - 21Oct08
                    if( delta.TotalDays > limitDelta || delta.TotalDays == 0 ) {
                        break;
                    }

                    // the dates are in successive intervals - keep going
                    fromDate = timingInfoList[ i ].Date;
                    totalAds += timingInfoList[ i ].AdCount;
                    durationDays += (int)Math.Max( 1, Math.Round( delta.TotalDays ) );
                    nRemoved += 1;
                }
            }
            return nRemoved;
        }

        /// <summary>
        /// Parses the values for this timing display item from the given row of the requesting page's display.
        /// </summary>
        /// <remarks>The name of each input in the page will include the row number, and the ad option ID (example: Month-3-1-FPC is the Month entry for row 3, which is 
        /// is part of the ad option 1 or "FPC") </remarks>
        /// <param name="request"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public abstract void Parse( HttpRequest request, int rowNumber, out int adOptionID );

        /// <summary>
        /// Converts this item to one or more TimingInfoItem objects.
        /// </summary>
        /// <returns></returns>
        public abstract List<TimingInfo.TimingInfoItem> ToTimingInfo();

        /// <summary>
        /// Retuens the HTML code that represents this item as a row (or rows) in a table, with minimized and expanded forms.
        /// </summary>
        /// <param name="overallRowNumber"></param>
        /// <returns></returns>
        public abstract TableRow GetRow( int overallRowNumber, int adOptionID, double spotPrice, bool expand, out string totalExpr );

        public abstract string ToOrderString( string adOptionName );

        protected string[] mos = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        /// <summary>
        /// Returns the HTML code for a month drop-down menu
        /// </summary>
        /// <param name="selectedMonth"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string MonthInput( int selectedMonth, string name ) {

            string monthInput = String.Format( "<select name=\"{0}\" id=\"{0}\" style=\"width:65px;\" >", name );
            for( int m = 0; m < 12; m++ ) {
                if( (m + 1) != selectedMonth ) {
                    monthInput += String.Format( "<option value=\"{0}\" >{1}</option>", m + 1, mos[ m ] );
                }
                else {
                    monthInput += String.Format( "<option value=\"{0}\" selected >{1}</option>", m + 1, mos[ m ] );
                }
            }
            monthInput += "</select>";
            return monthInput;
        }
    }
}

