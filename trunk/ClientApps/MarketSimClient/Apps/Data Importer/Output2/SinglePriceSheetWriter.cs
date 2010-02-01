using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter.Library;
using DataImporter.ImportSettings;

namespace DataImporter.Output2
{
    /// <summary>
    /// This class handles conversion and writing of imported data into the Price worksheet of a MarketSim-format file.
    /// </summary>
    class SinglePriceSheetWriter : IMarketPlanSheetWriter
    {
        private bool writeCSV = false;

        /// <summary>
        /// Creates a new SinglePriceSheetWriter object.
        /// </summary>
        public SinglePriceSheetWriter() {
        }

        /// <summary>
        /// Write the Price sheet data.  Columns: Campaign, Product, Channel, Price, Price Type, % Distribution, Start Date, End Date
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startDataRow"></param>
        /// <param name="startDataColumn"></param>
        /// <param name="marketPlan">The market plan to process</param>
        /// <param name="writer"></param>
        public void WriteData( WorksheetSettings worksheetSettings, ProjectSettings currentProject, ExcelWriter2 writer, int channelIndex ) {
            Console.WriteLine( "\r\nSinglePriceSheetWriter  WriteData()...\r\n" );

            string csvPath = worksheetSettings.OutputFile.Substring( 0, worksheetSettings.OutputFile.LastIndexOf( "." ) ) + ".csv";
            StreamWriter csvWriter = null;
            if( writeCSV ) {
                csvWriter = new StreamWriter( csvPath );
            }

            XPoint outputStart = new XPoint( 4, 1 );
            writer.SetSheet( "Price" );

            // sanity checks ?

            ProjectSettings.ProjectSection section = worksheetSettings.GetSection();
            if( section == null ) {
                string msg = String.Format( "    Error writing Price sheet -- worksheet has null section value    " );
                MessageBox.Show( msg, "Error writing Price Sheet", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }

            string brand = worksheetSettings.Brand.MarketSimName;
            ////string channel = (string)distComponent.Channels[ c ];
            string wsChannel = worksheetSettings.Channel.MarketSimName;

            int variantCount = section.Variants.Count;
            if( (section.BrandSource == ProjectSettings.InfoSource.DirectoryName) ||
                 (section.BrandSource == ProjectSettings.InfoSource.FileName) ) {

                // set per-sheet variant count
                variantCount = worksheetSettings.VariantCount;
            }

            string[] channels;
            if( section.ChannelSource == ProjectSettings.InfoSource.FileContents ) {
                channels = new string[ section.Channels.Count ];
                for( int i = 0; i < section.Channels.Count; i++ ) {
                    channels[ i ] = (string)section.Channels[ i ];
                }
            }
            else {
                channels = new string[ 1 ];
                channels[ 0 ] = wsChannel;
            }

            int numColumns = 10;
            int numRows = variantCount * section.TimeStepCount;          

            object[ , ] tableValues = new object[ numRows, numColumns ];

            int row = 0;
            int outrow = 0;

            double price = 0;

            string channel = channels[ channelIndex ];
            // convert to MarketSim name
            channel = currentProject.GetChannel( channel, section.ChannelSource ).MarketSimName;

            string[] variantNames = new string[ variantCount ];
            string[] campaigns = new string[ variantCount ];
            string[] itemNamesForGrouping = new string[ variantCount ];

            // first pass -- generate nams of variants and campaigns for the output; process excluded/specific variants
            for( int v = 0; v < variantCount; v++ ) {

                string campaign = "unpromo";

                string variantName = null;
                if( section.BrandSource != ProjectSettings.InfoSource.WorksheetName ) {
                    if( worksheetSettings.Variants != null ) {
                        variantName = brand + " - " + (string)worksheetSettings.Variants[ v ];
                    }
                    else {
                        variantName = brand + " - " + (string)worksheetSettings.GetSection().Variants[ v ];
                    }
                }

                bool excludeThisItem = false;
                if( currentProject.SpecifiedProducts.Count > 0 ) {
                    excludeThisItem = true;

                    foreach( string specificItemPrefix in currentProject.SpecifiedProducts ) {
                        if( variantName.StartsWith( specificItemPrefix ) ) {
                            excludeThisItem = false;
                            break;
                        }
                    }
                }

                foreach( string excludeItemPrefix in currentProject.ExcludeProducts ) {
                    if( variantName.StartsWith( excludeItemPrefix ) ) {
                        excludeThisItem = true;
                        break;
                    }
                }
                if( excludeThisItem ) {
                    continue;
                } 
                variantName = currentProject.GetProduct( variantName ).MarketSimName;

                if( section.BrandSource == ProjectSettings.InfoSource.WorksheetName ) {
                    // the variant name is really the media type name in this case
                    campaign = (string)section.Variants[ v ];
                    variantName = brand;
                }

                variantNames[ v ] = variantName;
                campaigns[ v ] = campaign;
                itemNamesForGrouping[ v ] = variantName;
            }

            // scan the variants and form groups if there are any specified
            int numGroups = -1;
            ArrayList[] variantGroups = Utils.GenerateVariantGroups( itemNamesForGrouping, out numGroups );
            if( numGroups > 0 ) {
                string msgg = String.Format( "\r\n    SinglePriceSheetWriter -- Groups Detected: {0}    \r\n", numGroups );
                //MessageBox.Show( msgg, "Found Variant Groups" );
                Console.WriteLine( msgg );
            }

            // loop to write the data
            for( int v = 0; v < variantCount; v++ ) {
                if( variantNames[ v ] == null || campaigns[ v ] == null || variantGroups[ v ].Count == 0 ) {
                    continue;
                }
                string variantName = variantNames[ v ];
                string campaign = campaigns[ v ];

                ArrayList data = worksheetSettings.GetData( channelIndex, v );

                DataItem item0 = (DataItem)data[ 0 ];
                DataItem onDeckItem = new DataItem( item0 );
                int skipCount = 0;

                for( int i = 0; i < data.Count; i++ ) {

                    DataItem item = (DataItem)data[ i ];
                    if( item.EndDate < currentProject.StartImportDate || item.StartDate > currentProject.EndImportDate ) {
                        continue;
                    }

                    item = Utils.GetGroupedItem( item, channelIndex, i, worksheetSettings, variantGroups[ v ], Utils.ComponentType.Price );

                    bool writeOnDeckItem = false;
                    if( data.Count != 1 && i != 0 ) {
                        if( InBallPark( item, onDeckItem, currentProject.ImportCompressionDeltaTolerance ) == true ) {
                            skipCount += 1;
                            double nAvg = skipCount + 1;
                            double r1 = (nAvg - 1) / nAvg;
                            double r2 = 1 / nAvg;
                            onDeckItem.Value1 = onDeckItem.Value1 * r1 + item.Value1 * r2;
                            onDeckItem.Value2 = onDeckItem.Value2 * r1 + item.Value2 * r2;
                            onDeckItem.EndDate = item.EndDate;
                            if( i != (data.Count - 1) ) {
                                row += 1;
                                continue;
                            }
                        }
                        else {
                            skipCount = 0;
                            writeOnDeckItem = true;
                        }
                    }

                    if( writeOnDeckItem ) {

                        price = onDeckItem.Value1 * currentProject.ImportDataPriceScaling;

                        tableValues[ outrow, 0 ] = campaign;                                          //campaign
                        tableValues[ outrow, 1 ] = variantName;                                      //product
                        tableValues[ outrow, 2 ] = channel;                                             //channel
                        tableValues[ outrow, 3 ] = price;                                                    //Price
                        tableValues[ outrow, 4 ] = "unpromoted";                                         //purch type
                        tableValues[ outrow, 5 ] = 100.0;                                                     //%dist (???NIMO???)
                        tableValues[ outrow, 6 ] = onDeckItem.StartDate.ToShortDateString();         // end date
                        tableValues[ outrow, 7 ] = onDeckItem.EndDate.ToShortDateString();         // end date


                        if( writeCSV ) {
                            string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName,  channel,
                                 price, "unpromoted", 100.0,
                                 onDeckItem.StartDate.ToShortDateString(), onDeckItem.EndDate.ToShortDateString() );
                            csvWriter.WriteLine( csvVal );
                        }
                        outrow += 1;

                        onDeckItem = new DataItem( item );
                    }
                    row += 1;
                }

                // write out the final row

                price = onDeckItem.Value1 * currentProject.ImportDataPriceScaling;

                tableValues[ outrow, 0 ] = campaign;                                          //campaign
                tableValues[ outrow, 1 ] = variantName;                                      //product
                tableValues[ outrow, 2 ] = channel;                                             //channel
                tableValues[ outrow, 3 ] = price;                                                     //Price
                tableValues[ outrow, 4 ] = "unpromoted";                                         //purch type
                tableValues[ outrow, 5 ] = 100.0;                                                     //%dist (???NIMO???)
                tableValues[ outrow, 6 ] = onDeckItem.StartDate.ToShortDateString();         // end date
                tableValues[ outrow, 7 ] = onDeckItem.EndDate.ToShortDateString();         // end date


                if( writeCSV ) {
                    string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName,  channel,
                         price, "unpromoted", 100.0,
                         onDeckItem.StartDate.ToShortDateString(), onDeckItem.EndDate.ToShortDateString() );
                    csvWriter.WriteLine( csvVal );
                }

                outrow += 1;
            }
            if( writeCSV ) {
                csvWriter.Flush();
                csvWriter.Close();
                csvWriter = null;
            }

            Utils.InsertFileInformationLine( currentProject, worksheetSettings, writer );
            writer.SetValues( outputStart.Row, outputStart.Col, outputStart.Row + row - 1, outputStart.Col + numColumns - 1, tableValues );
        }

        private bool InBallPark( DataItem item, DataItem refItem, double tolerance ) {
            double delta1 = Math.Abs( refItem.Value1 - item.Value1 );
            if( refItem.Value1 > 10 ) {
                delta1 /= refItem.Value1;
            }

            double delta2 = Math.Abs( refItem.Value2 - item.Value2 );
            if( refItem.Value2 > 10 ) {
                delta2 /= refItem.Value2;
            }

            if( delta1 > tolerance || delta2 > tolerance ) {
                return false;
            }
            return true;
        }
    }
}

