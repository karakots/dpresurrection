using System;
using System.Collections;
using System.Text;

using NitroReader.Library;

namespace NitroReader.Output
{
    /// <summary>
    /// This class handles conversion and writing of NITRO data into the Display worksheet of a MarketSim-format file.
    /// </summary>
    class DisplaySheetWriter : IMarketPlanSheetWriter
    {
        private string displayComponentName = "%ACV ANY DISPLAY";          // name of the source data sheet

        /// <summary>
        /// Creates a new DisplaySheetWriter object.
        /// </summary>
        public DisplaySheetWriter() {
        }

        /// <summary>
        /// Write the Display sheet data.  Columns: Campaign, Product, Channel, % Display, Aware Prob, Persuasion, Start Date, End Date
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startDataRow"></param>
        /// <param name="startDataColumn"></param>
        /// <param name="marketPlan"></param>
        /// <param name="writer"></param>
        public void WriteData( string sheetName, int startDataRow, int startDataColumn, MarketPlan marketPlan, Settings settings, ExcelWriter2 writer ) {
            Console.WriteLine( "\r\nDisplaySheetWriter  WriteData()...\r\n" );

            writer.SetSheet( sheetName );

            MarketPlan.Component dispComponent = marketPlan.GetComponent( displayComponentName );

            // sanity checks
            if( dispComponent == null ) {
                string w = String.Format( "Warning: No display component sheet ({0}) in this NITRO file.", displayComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }
            if( (dispComponent.Channels.Count == 0) || (dispComponent.Variants.Count == 0) || (dispComponent.Data[ 0 ].Count == 0) ) {
                string w = String.Format( "Warning: No data values found in distribution component sheet ({0}).", displayComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }

            int numColumns = 8;
            int numRows = dispComponent.Channels.Count * dispComponent.Variants.Count * dispComponent.Data[ 0 ].Count;
            object[ , ] tableValues = new object[ numRows, numColumns ];

            int row = 0;

            for( int c = 0; c < dispComponent.Channels.Count; c++ ) {
                string channel = (string)dispComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList data = dispComponent.Data[ v ];
                    for( int i = 0; i < data.Count; i++ ) {

                        MarketPlan.Component.Item item = (MarketPlan.Component.Item)data[ i ];

                        tableValues[ row, 0 ] = channel;                                             //campaign
                        tableValues[ row, 1 ] = settings.GetMarketSimName( vinfo.Name );                                        //product
                        tableValues[ row, 2 ] = channel;                                             //channel
                        tableValues[ row, 3 ] = item.Value1;                                       //% Display
                        tableValues[ row, 4 ] = dispComponent.Awareness;                    //Awareness
                        tableValues[ row, 5 ] = dispComponent.Persuasion;                    //Persuasion
                        tableValues[ row, 6 ] = item.StartDate.AddDays( 1 ).ToShortDateString();       //start date
                        tableValues[ row, 7 ] = item.EndDate.ToShortDateString();         // end date
                        row += 1;
                    }
                }
            }
            writer.SetValues( startDataRow, startDataColumn, startDataRow + numRows - 1, startDataColumn + numColumns - 1, tableValues );
        }
    }
}
