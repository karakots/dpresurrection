#define WRITE_PROMO
//#define WRITE_UNPROMO
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
    class TwoPriceSheetWriter : IMarketPlanSheetWriter
    {
        private bool writeCSV = false;

        /// <summary>
        /// Creates a new TwoPriceSheetWriter object.
        /// </summary>
        public TwoPriceSheetWriter() {
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

            // sanity checks 
            WorksheetSettings promoWorksheet = null;
            WorksheetSettings unpromoWorksheet = null;
            if( worksheetSettings.RelatedWorksheetIndexes != null ) {
                ArrayList relatedIndexes = (ArrayList)worksheetSettings.RelatedWorksheetIndexes;
                // there should be two
                if( relatedIndexes.Count != 2 ) {
                    MessageBox.Show( "    Error: Wrong count of related worksheets in TwoPriceSheetWriter.WriteData()   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                int i1 = (int)relatedIndexes[ 0 ];
                int i2 = (int)relatedIndexes[ 1 ];
                ArrayList allWorksheets = worksheetSettings.GetSection().WorksheetSettingsList;
                WorksheetSettings testWs = (WorksheetSettings)allWorksheets[ i1 ];
                if( testWs.DataType == ProjectSettings.DataType.PriceRegular ) {
                    unpromoWorksheet = testWs;
                    promoWorksheet = (WorksheetSettings)allWorksheets[ i2 ];
                }
                else {
                    promoWorksheet = testWs;
                    unpromoWorksheet = (WorksheetSettings)allWorksheets[ i2 ];
                }
            }
            if( unpromoWorksheet == null || promoWorksheet == null ) {
                MessageBox.Show( "    Error: Missing related worksheet(s) in TwoPriceSheetWriter.WriteData()   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            ProjectSettings.ProjectSection section = worksheetSettings.GetSection();
            if( section == null ) {
                string msg = String.Format( "    Error writing Price sheet -- worksheet has null section value    " );
                MessageBox.Show( msg, "Error writing Price Sheet", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            string brand = worksheetSettings.Brand.MarketSimName;
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
            int numRows = variantCount * section.TimeStepCount * 2;          

            object[ , ] tableValues = new object[ numRows, numColumns ];

            //int row = 0;
            int outrow = 0;

            double unpromoPrice = 0;
            double promoPrice = 0;

            string channel = channels[ channelIndex ];
            // convert to MarketSim name
            channel = currentProject.GetChannel( channel, section.ChannelSource ).MarketSimName;

            string[] variantNames = new string[ variantCount ];
            string[] campaigns = new string[ variantCount ];
            string[] itemNamesForGrouping = new string[ variantCount ];

            // first pass -- generate nams of variants and campaigns for the output; process excluded/specific variants
            for( int v = 0; v < variantCount; v++ ) {

                string campaign = "PU";

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

                string msgg = String.Format( "\r\n    TwoPriceSheetWriter -- Groups Detected: {0}    \r\n      WARNING: Groups ignored by this data writer.    \r\n", numGroups );
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

                ArrayList dataDist = worksheetSettings.GetData( channelIndex, v );
                ArrayList dataPromoPrice = promoWorksheet.GetData( channelIndex, v );
                ArrayList dataUnpromoPrice = unpromoWorksheet.GetData( channelIndex, v );

                DataItem item0Dist = (DataItem)dataDist[ 0 ];
                DataItem item0Promo = (DataItem)dataPromoPrice[ 0 ];
                DataItem item0Unpromo = (DataItem)dataUnpromoPrice[ 0 ];
                item0Promo.Value2 = item0Dist.Value1;
                item0Unpromo.Value2 = item0Dist.Value1;

                //DataItem onDeckItem = new DataItem( item0 );
                DataItem onDeckItemPromo = new DataItem( item0Promo );
                DataItem onDeckItemUnpromo = new DataItem( item0Unpromo );

                int skipCountPromo = 0;
                int skipCountUnpromo = 0;

                for( int i = 0; i < dataDist.Count; i++ ) {    // (there is the same number of promo-price points as unpromo)

                    DataItem itemDist = (DataItem)dataDist[ i ];
                    DataItem itemPromo = (DataItem)dataPromoPrice[ i ];
                    DataItem itemUnpromo = (DataItem)dataUnpromoPrice[ i ];

                    // the dates are the same for all items, so checking one is sufficient
                    if( itemDist.EndDate < currentProject.StartImportDate || itemDist.StartDate > currentProject.EndImportDate ) {
                        continue;
                    }

                    // !!! grouping is messing up price plans (prices are wrong somehow) !!!!
                    //itemDist = Utils.GetGroupedItem( itemDist, channelIndex, i, worksheetSettings, variantGroups[ v ], Utils.ComponentType.PriceDistribution );
                    //itemPromo = Utils.GetGroupedItem( itemPromo, channelIndex, i, worksheetSettings, variantGroups[ v ], Utils.ComponentType.Price );
                    //itemUnpromo = Utils.GetGroupedItem( itemUnpromo, channelIndex, i, worksheetSettings, variantGroups[ v ], Utils.ComponentType.Price );

                    // transfer the distribution values into the promo/unpromo items
                    itemPromo.Value2 = itemDist.Value1;
                    itemUnpromo.Value2 = itemDist.Value1;

                    bool writeOnDeckPromoItem = false;
                    bool writeOnDeckUnpromoItem = false;

                    if( dataUnpromoPrice.Count != 1 && i != 0 ) {
                        // check promo data
                        if( InBallPark( itemPromo, onDeckItemPromo, currentProject.ImportCompressionDeltaTolerance ) == true ) {
                            skipCountPromo += 1;
                            double nAvg = skipCountPromo + 1;
                            double r1 = (nAvg - 1) / nAvg;
                            double r2 = 1 / nAvg;
                            onDeckItemPromo.Value1 = onDeckItemPromo.Value1 * r1 + itemPromo.Value1 * r2;
                            onDeckItemPromo.Value2 = onDeckItemPromo.Value2 * r1 + itemPromo.Value2 * r2;
                            onDeckItemPromo.EndDate = itemPromo.EndDate;
                        }
                        else {
                            skipCountPromo = 0;
                            writeOnDeckPromoItem = true;
                        }

                        // check unpromo data
                        if( InBallPark( itemUnpromo, onDeckItemUnpromo, currentProject.ImportCompressionDeltaTolerance ) == true ) {
                            skipCountUnpromo += 1;
                            double nAvg = skipCountUnpromo + 1;
                            double r1 = (nAvg - 1) / nAvg;
                            double r2 = 1 / nAvg;
                            onDeckItemUnpromo.Value1 = onDeckItemUnpromo.Value1 * r1 + itemUnpromo.Value1 * r2;
                            onDeckItemUnpromo.Value2 = onDeckItemUnpromo.Value2 * r1 + itemUnpromo.Value2 * r2;
                            onDeckItemUnpromo.EndDate = itemUnpromo.EndDate;
                        }
                        else {
                            skipCountUnpromo = 0;
                            writeOnDeckUnpromoItem = true;
                        }
                    }

#if WRITE_UNPROMO
                    if( writeOnDeckUnpromoItem ) {
                        // write unpromo price

                        unpromoPrice = onDeckItemUnpromo.Value1 * currentProject.ImportDataPriceScaling;

                        tableValues[ outrow, 0 ] = campaign;                                          //campaign
                        tableValues[ outrow, 1 ] = variantName;                                      //product
                        tableValues[ outrow, 2 ] = channel;                                             //channel
                        tableValues[ outrow, 3 ] = unpromoPrice;                                       //Price
                        tableValues[ outrow, 4 ] = "unpromoted";                                         //purch type
                        tableValues[ outrow, 5 ] = 100.0 - onDeckItemUnpromo.Value2;                                //%dist (???NIMO???)
                        tableValues[ outrow, 6 ] = onDeckItemUnpromo.StartDate.ToShortDateString();         // end date
                        tableValues[ outrow, 7 ] = onDeckItemUnpromo.EndDate.ToShortDateString();         // end date


                        if( writeCSV ) {
                            string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName, channel,
                                 unpromoPrice, "unpromoted", 100.0 - onDeckItemUnpromo.Value2,
                                 onDeckItemUnpromo.StartDate.ToShortDateString(), onDeckItemUnpromo.EndDate.ToShortDateString() );
                            csvWriter.WriteLine( csvVal );
                        }
                        outrow += 1;
                        onDeckItemUnpromo = new DataItem( itemUnpromo );
                    }
#endif

#if WRITE_PROMO
                    if( writeOnDeckPromoItem ) {
                        // write promo price

                        promoPrice = onDeckItemPromo.Value1 * currentProject.ImportDataPriceScaling;

                        tableValues[ outrow, 0 ] = campaign;                                          //campaign
                        tableValues[ outrow, 1 ] = variantName;                                      //product
                        tableValues[ outrow, 2 ] = channel;                                             //channel
                        tableValues[ outrow, 3 ] = promoPrice;                                       //Price
                        tableValues[ outrow, 4 ] = "promoted";                                         //purch type
                        tableValues[ outrow, 5 ] = onDeckItemPromo.Value2;                         //%dist (???NIMO???)
                        tableValues[ outrow, 6 ] = onDeckItemPromo.StartDate.ToShortDateString();         // end date
                        tableValues[ outrow, 7 ] = onDeckItemPromo.EndDate.ToShortDateString();         // end date


                        if( writeCSV ) {
                            string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName, channel,
                                 promoPrice, "promoted", onDeckItemPromo.Value2,
                                 onDeckItemPromo.StartDate.ToShortDateString(), onDeckItemPromo.EndDate.ToShortDateString() );
                            csvWriter.WriteLine( csvVal );
                        }
                        outrow += 1;

                        //onDeckItem = new DataItem( item );
                        onDeckItemPromo = new DataItem( itemPromo );
                    }
#endif
                    //row += 1;
                }

 #if WRITE_UNPROMO
                // write out the final unpromo row
                unpromoPrice = onDeckItemUnpromo.Value1 * currentProject.ImportDataPriceScaling;

                tableValues[ outrow, 0 ] = campaign;                                          //campaign
                tableValues[ outrow, 1 ] = variantName;                                      //product
                tableValues[ outrow, 2 ] = channel;                                             //channel
                tableValues[ outrow, 3 ] = unpromoPrice;                                       //Price
                tableValues[ outrow, 4 ] = "unpromoted";                                         //purch type
                tableValues[ outrow, 5 ] = 100.0 - onDeckItemUnpromo.Value2;                                                     //%dist (???NIMO???)
                tableValues[ outrow, 6 ] = onDeckItemUnpromo.StartDate.ToShortDateString();         // end date
                tableValues[ outrow, 7 ] = onDeckItemUnpromo.EndDate.ToShortDateString();         // end date


                if( writeCSV ) {
                    string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName, channel,
                         unpromoPrice, "unpromoted", 100.0 - onDeckItemUnpromo.Value2,
                         onDeckItemUnpromo.StartDate.ToShortDateString(), onDeckItemUnpromo.EndDate.ToShortDateString() );
                    csvWriter.WriteLine( csvVal );
                }

                outrow += 1;
#endif

 #if WRITE_PROMO
                // write out the final promo row

                promoPrice = onDeckItemPromo.Value1 * currentProject.ImportDataPriceScaling;

                tableValues[ outrow, 0 ] = campaign;                                              //campaign
                tableValues[ outrow, 1 ] = variantName;                                         //product
                tableValues[ outrow, 2 ] = channel;                                                 //channel
                tableValues[ outrow, 3 ] = promoPrice;                                            //Price
                tableValues[ outrow, 4 ] = "promoted";                                                //purch type
                tableValues[ outrow, 5 ] = onDeckItemPromo.Value2;                                         //%dist (???NIMO???)
                tableValues[ outrow, 6 ] = onDeckItemPromo.StartDate.ToShortDateString();         // end date
                tableValues[ outrow, 7 ] = onDeckItemPromo.EndDate.ToShortDateString();         // end date


                if( writeCSV ) {
                    string csvVal = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", campaign, variantName, channel,
                         promoPrice, "promoted", onDeckItemPromo.Value2,
                         onDeckItemPromo.StartDate.ToShortDateString(), onDeckItemPromo.EndDate.ToShortDateString() );
                    csvWriter.WriteLine( csvVal );
                }

                outrow += 1;
#endif
            }


            if( writeCSV ) {
                csvWriter.Flush();
                csvWriter.Close();
                csvWriter = null;
            }

            Utils.InsertFileInformationLine( currentProject, worksheetSettings, writer );
            writer.SetValues( outputStart.Row, outputStart.Col, outputStart.Row + outrow - 1, outputStart.Col + numColumns - 1, tableValues );
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

