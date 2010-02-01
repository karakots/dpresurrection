using System;
using System.Collections;
using System.Text;

using NitroReader.Library;

namespace NitroReader.Output
{
    /// <summary>
    /// This class handles conversion and writing of NITRO data into the Real Sales Data worksheet of a MarketSim-format file.
    /// </summary>
    class RealSalesDataSheetWriter : IMarketPlanSheetWriter
    {
        private string unitsComponentName = "UNITS";          // name of the source data sheet

        /// <summary>
        /// Creates a new RealSalesDataSheetWriter object.
        /// </summary>
        public RealSalesDataSheetWriter() {
        }

        /// <summary>
        /// Write the Real Sales Data sheet data.  Columns: Product, Channel, Segment, Units Sold,  Date
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startDataRow"></param>
        /// <param name="startDataColumn"></param>
        /// <param name="marketPlan"></param>
        /// <param name="writer"></param>
        public void WriteData( string sheetName, int startDataRow, int startDataColumn, MarketPlan marketPlan, Settings settings, ExcelWriter2 writer ) {
            Console.WriteLine( "\r\nRealSalesDataSheetWriter  WriteData()...\r\n" );

            writer.SetSheet( sheetName );

            MarketPlan.Component unitsComponent = marketPlan.GetComponent( unitsComponentName );

            // sanity checks
            if( unitsComponent == null ) {
                string w = String.Format( "Warning: No units component sheet ({0}) in this NITRO file.", unitsComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }
            if( (unitsComponent.Channels.Count == 0) || (unitsComponent.Variants.Count == 0) || (unitsComponent.Data[ 0 ].Count == 0) ) {
                string w = String.Format( "Warning: No data values found in units component sheet ({0}).", unitsComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }

            int numColumns = 5;
            int numRows = unitsComponent.Channels.Count * unitsComponent.Variants.Count * unitsComponent.Data[ 0 ].Count;
            object[ , ] tableValues = new object[ numRows, numColumns ];

            int row = 0;

            for( int c = 0; c < unitsComponent.Channels.Count; c++ ) {
                string channel = (string)unitsComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList data = unitsComponent.Data[ v ];
                    for( int i = 0; i < data.Count; i++ ) {

                        MarketPlan.Component.Item item = (MarketPlan.Component.Item)data[ i ];

                        tableValues[ row, 0 ] = settings.GetMarketSimName( vinfo.Name );                                      //product
                        tableValues[ row, 1 ] = channel;                                             //channel
                        tableValues[ row, 2 ] = "All";                                                  //segment
                        tableValues[ row, 3 ] = item.Value1;                                       //units sold
                        tableValues[ row, 4 ] = item.EndDate.ToShortDateString();         //(end) date
                        row += 1;
                    }
                }
            }
            writer.SetValues( startDataRow, startDataColumn, startDataRow + numRows - 1, startDataColumn + numColumns - 1, tableValues );
        }
    }
}
