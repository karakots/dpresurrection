using System;
using System.Collections;
using System.Text;

using NitroReader.Library;

namespace NitroReader.Output
{
    /// <summary>
    /// This class handles conversion and writing of NITRO data into the Price worksheet of a MarketSim-format file.
    /// </summary>
    class PriceSheetWriter : IMarketPlanSheetWriter
    {
        // this sheet combines a variety of input components
        //private string avgPriceComponentName = "AVG UNIT PRICE";

        private string regularPriceComponentName = "NON-PROMO UNIT PRICE";
        private string promoPriceComponentName = "ANY PROMO UNIT PRICE";
        private string absolutePriceComponentName = "DISPLAY PRICE ABSOLUTE";
        
        private string promoPercentComponentName = "%ACV ANY PROMO";
        private string absolutePercentComponentName = "%ACV DISPLAY PRICE ABSOLUTE";
        private string distributionComponentName = "%ACV DIST";         

        /// <summary>
        /// Creates a new PriceSheetWriter object.
        /// </summary>
        public PriceSheetWriter() {
        }

        /// <summary>
        /// Write the Price sheet data.  Columns: Campaign, Product, Channel, Price, Purchase Type, % Distribution, Start Date, End Date
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startDataRow"></param>
        /// <param name="startDataColumn"></param>
        /// <param name="marketPlan"></param>
        /// <param name="writer"></param>
        public void WriteData( string sheetName, int startDataRow, int startDataColumn, MarketPlan marketPlan, Settings settings, ExcelWriter2 writer ) {
            Console.WriteLine( "\r\nPriceSheetWriter  WriteData()...\r\n" );

            writer.SetSheet( sheetName );

            // a variety of measures are needed to produce this sheet
            MarketPlan.Component regPriceComponent = marketPlan.GetComponent( regularPriceComponentName );
            MarketPlan.Component promoPriceComponent = marketPlan.GetComponent( promoPriceComponentName );
            MarketPlan.Component absPriceComponent = marketPlan.GetComponent( absolutePriceComponentName );
            MarketPlan.Component promoAcvComponent = marketPlan.GetComponent( promoPercentComponentName );
            MarketPlan.Component absAcvComponent = marketPlan.GetComponent( absolutePercentComponentName );
            MarketPlan.Component distComponent = marketPlan.GetComponent( distributionComponentName );

            //do sanity checks on the data
            if( !SanityCheck( regPriceComponent, regularPriceComponentName ) ||
                !SanityCheck( promoPriceComponent, promoPriceComponentName ) ||
                !SanityCheck( absPriceComponent, absolutePriceComponentName ) ||
                !SanityCheck( promoAcvComponent, promoPercentComponentName ) ||
                !SanityCheck( absAcvComponent, promoPercentComponentName ) ||
                !SanityCheck( distComponent, absolutePercentComponentName ) ) {
                return;
            }

            int numTables = 3;       //!!!???do we always have 3 price tables (regular, promo, absolute)?

            int numColumns = 8;
            int numRows = regPriceComponent.Channels.Count * regPriceComponent.Variants.Count * regPriceComponent.Data[ 0 ].Count;
            numRows *= numTables;

            object[ , ] tableValues = new object[ numRows, numColumns ];

            int row = 0;

            // write out the unpromototed price rows
            string campaign = "unpromo";
            string purchType = "unpromoted";
            for( int c = 0; c < regPriceComponent.Channels.Count; c++ ) {
                string channel = (string)regPriceComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList regPriceData = regPriceComponent.Data[ v ];
                    ArrayList promoAcvData = promoAcvComponent.Data[ v ];
                    ArrayList distData = distComponent.Data[ v ];

                    for( int i = 0; i < regPriceData.Count; i++ ) {

                        MarketPlan.Component.Item regPriceItem = (MarketPlan.Component.Item)regPriceData[ i ];
                        MarketPlan.Component.Item promoAcvItem = (MarketPlan.Component.Item)promoAcvData[ i ];
                        MarketPlan.Component.Item distItem = (MarketPlan.Component.Item)distData[ i ];

                        double normalizedPromoAcvItemVal = 0;
                        if( distItem.Value1 != 0 ) {
                            normalizedPromoAcvItemVal = promoAcvItem.Value1 / distItem.Value1;   //normalize %ACV ANY PROMO by dividing by %ACV DIST value
                        }

                        tableValues[ row, 0 ] = campaign;                                                          //campaign
                        tableValues[ row, 1 ] = settings.GetMarketSimName( vinfo.Name );             //product
                        tableValues[ row, 2 ] = channel;                                                             //channel
                        tableValues[ row, 3 ] = regPriceItem.Value1;                                            //price
                        tableValues[ row, 4 ] = purchType;                                                          //purchase type
                        if( settings.NormalizeForNIMO ) {
                            tableValues[ row, 5 ] = 100 * (1 - normalizedPromoAcvItemVal);              //%dist, normalized for NIMO
                        }
                        else {
                            double pctDist = Math.Max( distItem.Value1 - promoAcvItem.Value1, 0 );
                            tableValues[ row, 5 ] = pctDist;                                                          //%dist, not normalized
                        }
                        tableValues[ row, 6 ] = regPriceItem.StartDate.AddDays( 1 ).ToShortDateString();            //start date
                        tableValues[ row, 7 ] = regPriceItem.EndDate.ToShortDateString();              // end date
                        row += 1;
                    }
                }
            }

            // write out the promototed price rows
            campaign = "promo";
            purchType = "promoted";
            for( int c = 0; c < regPriceComponent.Channels.Count; c++ ) {    // just use the regular-price component since the other ones are similar except data
                string channel = (string)regPriceComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList promoPriceData = promoPriceComponent.Data[ v ];
                    ArrayList promoAcvData = promoAcvComponent.Data[ v ];
                    ArrayList distData = distComponent.Data[ v ];
                    ArrayList absAcvData = absAcvComponent.Data[ v ];

                    for( int i = 0; i < promoPriceData.Count; i++ ) {

                        MarketPlan.Component.Item promoPriceItem = (MarketPlan.Component.Item)promoPriceData[ i ];
                        MarketPlan.Component.Item promoAcvItem = (MarketPlan.Component.Item)promoAcvData[ i ];
                        MarketPlan.Component.Item absAcvItem = (MarketPlan.Component.Item)absAcvData[ i ];
                        MarketPlan.Component.Item distItem = (MarketPlan.Component.Item)distData[ i ];

                        double normalizedPromoAcvItemVal = 0;
                        double normalizedAbsAcvItemVal = 0;
                        if( distItem.Value1 != 0 ) {
                            normalizedPromoAcvItemVal = promoAcvItem.Value1 / distItem.Value1;   //normalize %ACV ANY PROMO by dividing by %ACV DIST value
                            normalizedAbsAcvItemVal = absAcvItem.Value1 / distItem.Value1;           //normalize %ACV DISPLAY PRICE ABSOLUTE by dividing by %ACV DIST value
                        }

                        tableValues[ row, 0 ] = campaign;                                                           //campaign
                        tableValues[ row, 1 ] = settings.GetMarketSimName( vinfo.Name );             //product
                        tableValues[ row, 2 ] = channel;                                                              //channel
                        tableValues[ row, 3 ] = promoPriceItem.Value1;                                        //price
                        tableValues[ row, 4 ] = purchType;                                                          //purchase type
                        if( settings.NormalizeForNIMO ) {
                            tableValues[ row, 5 ] = 100 * (normalizedPromoAcvItemVal - normalizedAbsAcvItemVal);    //%dist, normalized for NIMO 
                        }
                        else {
                            tableValues[ row, 5 ] = promoAcvItem.Value1 - absAcvItem.Value1;         //%dist, not normalized
                        }
                        tableValues[ row, 6 ] = promoPriceItem.StartDate.AddDays( 1 ).ToShortDateString();        //start date
                        tableValues[ row, 7 ] = promoPriceItem.EndDate.ToShortDateString();          // end date
                        row += 1;
                    }
                }
            }

            // write out the absolute price rows
            campaign = "display";
            purchType = "Z";
            for( int c = 0; c < regPriceComponent.Channels.Count; c++ ) {
                string channel = (string)regPriceComponent.Channels[ c ];

                for( int v = 0; v < marketPlan.Variants.Count; v++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                        // the item is in a group - skip it
                        continue;
                    }

                    ArrayList absPriceData = absPriceComponent.Data[ v ];
                    ArrayList absAcvData = absAcvComponent.Data[ v ];
                    ArrayList distData = distComponent.Data[ v ];

                    for( int i = 0; i < absPriceData.Count; i++ ) {

                        MarketPlan.Component.Item absPriceItem = (MarketPlan.Component.Item)absPriceData[ i ];
                        MarketPlan.Component.Item absAcvItem = (MarketPlan.Component.Item)absAcvData[ i ];
                        MarketPlan.Component.Item distItem = (MarketPlan.Component.Item)distData[ i ];

                        double normalizedAbsAcvItemVal = 0;
                        if( distItem.Value1 != 0 ) {
                            normalizedAbsAcvItemVal = absAcvItem.Value1 / distItem.Value1;           //normalize %ACV DISPLAY PRICE ABSOLUTE by dividing by %ACV DIST value
                        }

                        tableValues[ row, 0 ] = campaign;                                                         //campaign
                        tableValues[ row, 1 ] = settings.GetMarketSimName( vinfo.Name );            //product
                        tableValues[ row, 2 ] = channel;                                                             //channel
                        tableValues[ row, 3 ] = absPriceItem.Value1;                                            //price
                        tableValues[ row, 4 ] = purchType;                                                         //purchase type
                        if( settings.NormalizeForNIMO ) {
                            tableValues[ row, 5 ] = 100 * normalizedAbsAcvItemVal;                         //%dist, normalized for NIMO
                        }
                        else {
                            tableValues[ row, 5 ] = absAcvItem.Value1;                                          //%dist, not normalized
                        }
                        tableValues[ row, 6 ] = absPriceItem.StartDate.AddDays( 1 ).ToShortDateString();            //start date
                        tableValues[ row, 7 ] = absPriceItem.EndDate.ToShortDateString();              // end date
                        row += 1;
                    }
                }
            }

            // asctually write the data to the Excel worksheet
            writer.SetValues( startDataRow, startDataColumn, startDataRow + numRows - 1, startDataColumn + numColumns - 1, tableValues );
        }

        /// <summary>
        /// Ensures that we have all data values set that we need to successfully create an output table
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="compName"></param>
        /// <returns></returns>
        private bool SanityCheck( MarketPlan.Component comp, string compName ) {
            if( comp == null ) {
                string w = String.Format( "Warning: No price component sheet ({0}) in this NITRO file.", compName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return false;
            }
            if( (comp.Channels.Count == 0) || (comp.Variants.Count == 0) || (comp.Data[ 0 ].Count == 0) ) {
                string w = String.Format( "Warning: No data values found in price component sheet ({0}).", compName );
                MarketPlan.warnings.Add( w );
                Console.WriteLine( w );
                return false;
            }
            return true;
        }
    }
}
