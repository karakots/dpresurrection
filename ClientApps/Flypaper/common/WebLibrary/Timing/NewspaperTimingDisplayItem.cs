using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace WebLibrary.Timing
{
    public class NewspaperTimingDisplayItem : TimingDisplayItem
    {
        public int NumberOfIssues;

        public NewspaperTimingDisplayItem( DateTime defaultStart )
            : base( defaultStart ) {
        }

         public override List<TimingInfo.TimingInfoItem> Parse( List<TimingInfo.TimingInfoItem> timingInfoList ) {

            if( timingInfoList.Count == 0 ) {
                this.NumberOfIssues = 0;
            }
            else {
                TimingInfo.TimingInfoItem firstTimingItem = timingInfoList[ 0 ];

                this.Day = firstTimingItem.Date.Day;
                this.Month = firstTimingItem.Date.Month;
                this.Year = firstTimingItem.Date.Year;
                this.NumberOfIssues = 1;
                int durationDays = 1;
                timingInfoList.RemoveAt( 0 );

                // also get any immediately following items
                int nRemoved = GetAllAdsInPulse( ref this.NumberOfIssues, ref durationDays, timingInfoList, firstTimingItem.Date, MediaLibrary.MediaVehicle.AdCycle.Daily );
                // remove the items we have used
                timingInfoList.RemoveRange( 0, nRemoved );
            }
            return timingInfoList;
         }

         public override void Parse( HttpRequest request, int rowNumber, out int adOptionID ) {
             adOptionID = 0;
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

                 string issuesInputName = String.Format( "Issues-{0}-{1}", rowNumber, adOptionID );
                 this.NumberOfIssues = int.Parse( request[ issuesInputName ] );
             }
         }

         public override List<TimingInfo.TimingInfoItem> ToTimingInfo() {
             List<TimingInfo.TimingInfoItem> newInfo = new List<TimingInfo.TimingInfoItem>();

             DateTime date = new DateTime( this.Year, this.Month, this.Day );

             for( int i = 0; i < this.NumberOfIssues; i++ ) {
                 TimingInfo.TimingInfoItem item = new TimingInfo.TimingInfoItem( date, 1 );
                 newInfo.Add( item );

                 DateTime nextDay;

                 nextDay = date.AddDays( 1 );

                 date = nextDay;
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

             string dayInputName = String.Format( "Day-{0}-{1}", overallRowNumber, adOptionID );
             string monthInputName = String.Format( "Month-{0}-{1}", overallRowNumber, adOptionID );
             string yearInputName = String.Format( "Year-{0}-{1}", overallRowNumber, adOptionID );
             string issuesInputName = String.Format( "Issues-{0}-{1}", overallRowNumber, adOptionID );

             string dayInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\"  onKeyPress='return AllowOnlyNumbers(event);'  >", dayInputName, this.Day );
             string monthInput = MonthInput( this.Month, monthInputName );
             string yearInput = String.Format( "<input type=text style=\"width:40px;\" name=\"{0}\" value=\"{1}\" onKeyPress='return AllowOnlyNumbers(event);'  >", yearInputName, this.Year );

             string priceUpdJS = String.Format( "UpdatePrice( {0}, \"{1}\" );", overallRowNumber, issuesInputName );
             string issuesInput = String.Format( "<input type=text style=\"width:30px;\" name=\"{0}\" value=\"{1}\" onKeyPress='return AllowOnlyNumbers(event);'  onKeyup='{2}' >", issuesInputName, this.NumberOfIssues, priceUpdJS );

             double rowPrice = this.NumberOfIssues * spotPrice;
             string rowPriceField = String.Format( "RowPrice{0}", overallRowNumber );
             string rowTotalField = String.Format( "RowTotal{0}", overallRowNumber );
             string priceInput = String.Format( "<input type=hidden name=\"{0}\" value=\"{1}\" ><input type=hidden name=\"{2}\" value=\"{3:f2}\" >",
                 rowPriceField, spotPrice, rowTotalField, rowPrice );

             string rowOptionIDField = String.Format( "RowOpt{0}", overallRowNumber );
             string optionIDInput = String.Format( "<input type=hidden name=\"{0}\" id=\"{0}\" value=\"{1}\" >", rowOptionIDField, adOptionID );

             totalExpr = String.Format( "({0}*document.getElementById('{1}').value)", spotPrice, issuesInputName );

             //build up the row
             inputCell.Text = String.Format( "{0} daily issues starting {1} {2}, {3} {4} {5}", issuesInput, dayInput, monthInput, yearInput, priceInput, optionIDInput );

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
             if( this.NumberOfIssues == 1 ) {
                 s = String.Format( "{0} ad in the {1} {2}, {3} edition", adOptionName, this.mos[ this.Month - 1 ], this.Day, this.Year );
             }
             else if( this.NumberOfIssues > 1 ) {
                 DateTime d0 = new DateTime( this.Year, this.Month, this.Day );
                 DateTime d1 = d0.AddDays( this.NumberOfIssues - 1 );

                 s = String.Format( "{0} ad in {1} editions: {2} thru {3}", adOptionName, this.NumberOfIssues,
                     d0.ToString( "MMM d, yyyy" ), d1.ToString( "MMM d, yyyy" ) );
             }
             return "&nbsp;&nbsp;" + s.Replace( " ", "&nbsp;" );
         }
    }
}
