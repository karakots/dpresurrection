using System;
using System.Collections;
using System.Text;

using NitroReader.Library;

namespace NitroReader.Output
{
    /// <summary>
    /// This class handles conversion and writing of NITRO data into the Distribution worksheet of a MarketSim-format file.
    /// </summary>
    class DistributionSheetWriter : IMarketPlanSheetWriter
    {
        private string distributionComponentName = "%ACV DIST";          // name of the source data sheet

        /// <summary>
        /// Creates a new DistributionSheetWriter object.
        /// </summary>
        public DistributionSheetWriter() {
        }

        /// <summary>
        /// Write the Distribution sheet data.  Columns: Product, Channel, % Dist, % Init, Aware Prob, Persuasion, Start Date, End Date
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startDataRow"></param>
        /// <param name="startDataColumn"></param>
        /// <param name="marketPlan">The market plan to process</param>
        /// <param name="writer"></param>
        public void WriteData( string sheetName, int startDataRow, int startDataColumn, MarketPlan marketPlan, Settings settings, ExcelWriter2 writer ) {
            Console.WriteLine( "\r\nDistributionSheetWriter  WriteData()...\r\n" );

            writer.SetSheet( sheetName );

            MarketPlan.Component distComponent = marketPlan.GetComponent( distributionComponentName );

            // sanity checks
            if( distComponent == null ) {
                string w = String.Format( "Warning: No distribution component sheet ({0}) in this NITRO file.", distributionComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }
            if( (distComponent.Channels.Count == 0) || (distComponent.Variants.Count == 0) || (distComponent.Data[ 0 ].Count == 0) ) {
                string w = String.Format( "Warning: No data values found in distribution component sheet ({0}).", distributionComponentName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return;
            }

            int numColumns = 8;
            int numRows = distComponent.Channels.Count * distComponent.Variants.Count * distComponent.Data[ 0 ].Count;
            object[,] tableValues = new object[ numRows, numColumns ];

            int row = 0;

            for( int c = 0; c < distComponent.Channels.Count; c++ ) {
                string channel = (string)distComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList data = distComponent.Data[ v ];
                    for( int i = 0; i < data.Count; i++ ) {

                        MarketPlan.Component.Item item = (MarketPlan.Component.Item)data[ i ];

                        tableValues[ row, 0 ] = settings.GetMarketSimName( vinfo.Name );                                       //product
                        tableValues[ row, 1 ] = channel;                                             //channel
                        tableValues[ row, 2 ] = item.Value1;                                       //% Dist
                        tableValues[ row, 3 ] = item.Value1;                                       //% Init
                        tableValues[ row, 4 ] = distComponent.Awareness;                    //Awareness
                        tableValues[ row, 5 ] = distComponent.Persuasion;                    //Persuasion
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
