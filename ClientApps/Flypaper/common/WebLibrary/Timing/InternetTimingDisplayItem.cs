using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace WebLibrary.Timing
{
    public class InternetTimingDisplayItem : TimingDisplayItem
    {
        public int DurationDays;
        public int NumberOfImpresions;

        public int DaysBetweenPulses;
        public int TotalPulses;

        public InternetTimingDisplayItem( DateTime defaultStart )
            : base( defaultStart ) {
        }

        public override List<TimingInfo.TimingInfoItem> Parse( List<TimingInfo.TimingInfoItem> timingInfoList ) {
            if( timingInfoList.Count == 0 ) {
                this.NumberOfImpresions = 0;
                this.DurationDays = 1;
                this.DaysBetweenPulses = 0;
                this.TotalPulses = 1;
                this.Continuous = true;
            }
            else {
                this.Continuous = false;

                TimingInfo.TimingInfoItem firstTimingItem = timingInfoList[ 0 ];

                this.Day = firstTimingItem.Date.Day;
                this.Month = firstTimingItem.Date.Month;
                this.Year = firstTimingItem.Date.Year;
                this.DurationDays = 1;
                this.DaysBetweenPulses = 0;
                this.TotalPulses = 1;
                this.NumberOfImpresions = firstTimingItem.AdCount;
                timingInfoList.RemoveAt( 0 );

                // also get any immediately following items
                int nRemoved = GetAllAdsInPulse( ref this.NumberOfImpresions, ref this.DurationDays, timingInfoList, firstTimingItem.Date, MediaLibrary.MediaVehicle.AdCycle.Instant );
                // remove the items we have used
                timingInfoList.RemoveRange( 0, nRemoved );

                if( timingInfoList.Count > 0 ) {
                    nRemoved = 0;
                    bool foundPulse = false;
                    int basePulseImpressions = this.NumberOfImpresions;

                    DateTime pulseStart = timingInfoList[ 0 ].Date;
                    DateTime prevPulseStart = new DateTime( this.Year, this.Month, this.Day );

                    int sanityLimit = 10000;
                    // see if we have any succeeding pulses
                    do {
                        if( nRemoved < timingInfoList.Count ) {
                            pulseStart = timingInfoList[ nRemoved ].Date;
                        }
                        else {
                            break;
                        }
                        foundPulse = false;

                        int pulseImps = 0;
                        int pulseDays = 0;
                        int pulseDelay = 0;

                        int pulseRemoved = GetAllAdsInPulse( ref pulseImps, ref pulseDays, timingInfoList, pulseStart, MediaLibrary.MediaVehicle.AdCycle.Instant );

                        if( pulseDays == this.DurationDays && pulseImps == this.NumberOfImpresions ) {
                            // we have found a duplicate pulse
                            if( this.DaysBetweenPulses == 0 ) {     // if true, the DaysBetweenPulses value hasn't been set yet
                                foundPulse = true;
                                this.DaysBetweenPulses = (int)((pulseStart - prevPulseStart).TotalDays - this.DurationDays);
                                this.TotalPulses += 1;
                                prevPulseStart = pulseStart;
                                nRemoved += pulseRemoved;
                            }
                            else {
                                int nextDaysBetweenPulses = (int)((pulseStart - prevPulseStart).TotalDays - this.DurationDays);
                                if( nextDaysBetweenPulses == this.DaysBetweenPulses ) {
                                    foundPulse = true;
                                    this.TotalPulses += 1;
                                    prevPulseStart = pulseStart;
                                    nRemoved += pulseRemoved;
                                }
                            }
                        }
                    }
                    while( foundPulse == true && sanityLimit-- > 0 );

                    if( nRemoved > 0 ) {
                        // remove the items we have used for the additional pulses
                        timingInfoList.RemoveRange( 0, nRemoved );
                    }
                }
            }
            return timingInfoList;
        }

        public override void Parse( HttpRequest request, int rowNumber, out int adOptionID ) {
            adOptionID = -1;
            bool foundMonth = false;

            string monthInputHead = String.Format( "Month-{0}-", rowNumber );

            foreach( string key in request.Params.AllKeys ) {
                if( key.StartsWith( monthInputHead ) == true ) {

                    this.Month = int.Parse( request[ key ] );

                    string keyEnd = key.Substring( monthInputHead.Length );
                    adOptionID = int.Parse( keyEnd );
                    foundMonth = true;

                    break;
                }
            }

            if( foundMonth ) {
                // we found the month - get the other values
                string yearInputName = String.Format( "Year-{0}-{1}", rowNumber, adOptionID );
                this.Year = int.Parse( request[ yearInputName ] );

                string dayInputName = String.Format( "Day-{0}-{1}", rowNumber, adOptionID );
                this.Day = int.Parse( request[ dayInputName ] );

                string impressionsInputName = String.Format( "Imps-{0}-{1}", rowNumber, adOptionID );
                this.NumberOfImpresions = int.Parse( request[ impressionsInputName ] );

                string durationInputName = String.Format( "Span-{0}-{1}", rowNumber, adOptionID );
                this.DurationDays = int.Parse( request[ durationInputName ] );
                this.DurationDays = (int)Math.Max( 1, this.DurationDays );

                string interPulseDelayInputName = String.Format( "PDly-{0}-{1}", rowNumber, adOptionID );
                if( request[ interPulseDelayInputName ] != null ) {
                    this.DaysBetweenPulses = int.Parse( request[ interPulseDelayInputName ] );
                    this.DaysBetweenPulses = (int)Math.Max( 1, this.DaysBetweenPulses );
                }

                string pulseCountInputName = String.Format( "PCnt-{0}-{1}", rowNumber, adOptionID );
                if( request[ pulseCountInputName ] != null ) {
                    this.TotalPulses = int.Parse( request[ pulseCountInputName ] ) + 1;
                    this.TotalPulses = (int)Math.Max( 1, this.TotalPulses );
                }
                else {
                    this.TotalPulses = 1;
                }
            }
        }

        public override List<TimingInfo.TimingInfoItem> ToTimingInfo() {
            List<TimingInfo.TimingInfoItem> newInfo = new List<TimingInfo.TimingInfoItem>();

            DateTime date = new DateTime( this.Year, this.Month, this.Day );

            int impressionsPerDay = (int)Math.Floor( this.NumberOfImpresions / (double)this.DurationDays );
            int nExtraImpressions = this.NumberOfImpresions - (this.DurationDays * impressionsPerDay);

            for( int p = 0; p < this.TotalPulses; p++ ) {
                for( int i = 0; i < this.DurationDays; i++ ) {
                    int nImps = impressionsPerDay;
                    if( i < nExtraImpressions ) {
                        nImps += 1;
                    }
                    TimingInfo.TimingInfoItem item = new TimingInfo.TimingInfoItem( date, nImps );
                    newInfo.Add( item );
                    date = date.AddDays( 1 );
                }
                date = date.AddDays( this.DaysBetweenPulses );
            }

            return newInfo;
        }

        public override TableRow GetRow( int overallRowNumber, int adOptionID, double spotPrice, bool expand, out string totalExpr ) {
            totalExpr = "0";
            this.SpotPrice = spotPrice;
            TableRow row = new TableRow();
            TableCell inputCell = new TableCell();
            if( adOptionID < 0 ) {
                adOptionID = 0;       // we use dashes as formatting items so we can't have negative IDs
            }

            bool hasExpandedData = false; 

            string dayInputName = String.Format( "Day-{0}-{1}", overallRowNumber, adOptionID );
            string monthInputName = String.Format( "Month-{0}-{1}", overallRowNumber, adOptionID );
            string yearInputName = String.Format( "Year-{0}-{1}", overallRowNumber, adOptionID );
            string impressionsInputName = String.Format( "Imps-{0}-{1}", overallRowNumber, adOptionID );
            string durationInputName = String.Format( "Span-{0}-{1}", overallRowNumber, adOptionID );
            string interPulseDelayInputName = String.Format( "PDly-{0}-{1}", overallRowNumber, adOptionID );
            string pulseCountInputName = String.Format( "PCnt-{0}-{1}", overallRowNumber, adOptionID );

            string dayInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\" onKeyPress='return AllowOnlyNumbers(event);'  >", dayInputName, this.Day );
            string monthInput = MonthInput( this.Month, monthInputName );
            string yearInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\"  onKeyPress='return AllowOnlyNumbers(event);' >", yearInputName, this.Year );

            string priceUpdJS = String.Format( "UpdatePrice( {0}, \"{1}\" );", overallRowNumber, impressionsInputName );
            string impressionsInput = String.Format( "<input type=text style=\"width:50px;\" name=\"{0}\" value=\"{1}\" onKeyPress='return AllowOnlyNumbers(event);' onKeyup='{2}' >", impressionsInputName, this.NumberOfImpresions, priceUpdJS );

            string durationInput = String.Format( "<input type=text style=\"width:30px;\" name=\"{0}\" value=\"{1}\"  onKeyPress='return AllowOnlyNumbers(event);'  >", durationInputName, this.DurationDays );

            if( this.TotalPulses > 1 ) {
                hasExpandedData = true;
            }
            string pricePulseUpdJS = String.Format( "UpdatePPrice( {0}, \"{1}\", \"{2}\" );", overallRowNumber, impressionsInputName, pulseCountInputName );

            string pulseDelayInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\"  onKeyPress='return AllowOnlyNumbers(event);' >", interPulseDelayInputName, this.DaysBetweenPulses );
            string pulseCountInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\" onKeyPress='return AllowOnlyNumbers(event);' onKeyup='{2}' >", pulseCountInputName, this.TotalPulses - 1, pricePulseUpdJS );

            double rowPrice = this.NumberOfImpresions * spotPrice;
            string rowPriceField = String.Format( "RowPrice{0}", overallRowNumber );
            string rowTotalField = String.Format( "RowTotal{0}", overallRowNumber );
            string priceInput = String.Format( "<input type=hidden name=\"{0}\" value=\"{1}\" ><input type=hidden name=\"{2}\" value=\"{3:f2}\" >",
                rowPriceField, spotPrice, rowTotalField, rowPrice );

            string rowOptionIDField = String.Format( "RowOpt{0}", overallRowNumber );
            string optionIDInput = String.Format( "<input type=hidden name=\"{0}\" id=\"{0}\" value=\"{1}\" >", rowOptionIDField, adOptionID );

            //build up the row
            inputCell.Text = String.Format( "{0} impressions over {1} days starting {2} {3}, {4} {5} {6}", impressionsInput, durationInput, dayInput, monthInput, yearInput, priceInput, optionIDInput );

            string expand_item_JS = String.Format("ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "ExpandItem", overallRowNumber );
            string collapse_item_JS = String.Format("ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "CollapseItem", overallRowNumber );

            if( hasExpandedData ) {
                expand = true;                // items with non-defaut expanded data cannot be collapsed
            }

            if( expand == false ) {
                inputCell.Text += String.Format( " <a href=# onclick='{0}' >{1}</a>", expand_item_JS, "more options" );

                totalExpr = String.Format( "({0}*document.getElementById('{1}').value)", spotPrice, impressionsInputName );
            }
            else {
                inputCell.Text += String.Format( " <a href=# onclick='{0}' >{1}</a>", collapse_item_JS, "less options" );

                inputCell.Text += String.Format( "<br>Pulsing: Perform {0} additional pulses by repeating these ads with {1} days off between each active interval.", pulseCountInput, pulseDelayInput );

                totalExpr = String.Format( "({0}*document.getElementById('{1}').value*((document.getElementById('{2}').value*1)+1))", spotPrice, impressionsInputName, pulseCountInputName );
                rowPrice *= this.TotalPulses;
            }


            inputCell.Style.Add( "padding-left", "20px;" );
            inputCell.Style.Add( "padding-top", "10px;" );
            row.Cells.Add( inputCell );

            TableCell costCell = new TableCell();
            costCell.Style.Add( "text-align", "right" );
            costCell.Style.Add( "padding-top", "10px;" );
            costCell.Style.Add( "padding-right", "20px;" );

            string rowTotalDispField = String.Format( "RowTotalDisp{0}", overallRowNumber );
            costCell.Text = Utils.FormatDollarAmount( rowTotalDispField, rowPrice );

            row.Cells.Add( costCell );

            return row;
        }

        public override string ToOrderString( string adOptionName ) {
            string s = "";
            DateTime d0 = new DateTime( this.Year, this.Month, this.Day );

            for( int p = 0; p < this.TotalPulses; p++ ) {
                if( this.NumberOfImpresions == 1 ) {
                    s += String.Format( "1 {0} impression on {1}", adOptionName, d0.ToString( "MMM d, yyyy" ) );
                }
                else if( this.DurationDays == 1 ) {
                    s += String.Format( "{0} {1} impressions on {2}", this.NumberOfImpresions, adOptionName, d0.ToString( "MMM d, yyyy" ) );
                }
                else if( this.NumberOfImpresions > 1 ) {
                    DateTime d1 = d0.AddDays( this.DurationDays - 1 );

                    if( this.NumberOfImpresions < 1000 ) {
                        s += String.Format( "{0} {1} impressions over {2} days: {3} thru {4}", this.NumberOfImpresions, adOptionName, this.DurationDays,
                            d0.ToString( "MMM d, yyyy" ), d1.ToString( "MMM d, yyyy" ) );
                    }
                    else {
                        s += String.Format( "{0:0,0} {1} impressions over {2} days: {3} thru {4}", this.NumberOfImpresions, adOptionName, this.DurationDays,
                           d0.ToString( "MMM d, yyyy" ), d1.ToString( "MMM d, yyyy" ) );
                    }
                }

                d0 = d0.AddDays( this.DaysBetweenPulses + this.DurationDays );
                if( p < this.TotalPulses - 1 ) {
                    s += "<br>&nbsp;&nbsp;";
                }
            }
            return "&nbsp;&nbsp;" + s.Replace( " ", "&nbsp;" );
        }
    }
}
