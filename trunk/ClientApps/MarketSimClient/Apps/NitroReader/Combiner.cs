using System;
using System.Collections;
using System.Text;

namespace NitroReader
{
    class Combiner
    {
        /// <summary>
        /// Combines items in the market plan component data as specifed by the group items in the settings.
        /// </summary>
        /// <param name="marketPlan"></param>
        /// <param name="settings"></param>
        /// <remarks>Do not call more than once! This method adds rows for groups to the component data!  It also adds varisnts to the market plan!</remarks>
        public static void CombineItems( MarketPlan marketPlan, Settings settings ) {

            // extend the Data array of each component to have space for the grouped data 
            int nGroups = settings.Groups.Count;
            int groupDataStartIndex = -1;
            foreach( MarketPlan.Component component in marketPlan.Components ) {
                ArrayList[] currentData = component.Data;
                ArrayList[] newData = new ArrayList[ currentData.Length + nGroups ];
                groupDataStartIndex = currentData.Length;
                int i = 0;
                for( ; i < currentData.Length; i++ ) {
                    newData[ i ] = currentData[ i ];
                }
                for( ; i < newData.Length; i++ ) {
                    newData[ i ] = new ArrayList();
                    // fill the group data array with the correct number of zeros so it is valid data now
                    for( int j = 0; j < currentData[ 0 ].Count; j++ ) {
                        MarketPlan.Component.Item copyItem = (MarketPlan.Component.Item)currentData[ 0 ][ j ];
                        MarketPlan.Component.Item newItem = new MarketPlan.Component.Item( copyItem.StartDate, copyItem.EndDate, 0 );
                        newData[ i ].Add( newItem );
                    }
                }
                component.Data = newData;
            }

            // add the groups name and index to the Variants list
            for( int g = 0; g < nGroups; g++ ) {

                Settings.GroupInfo groupInfo = (Settings.GroupInfo)settings.Groups[ g ];
                if( groupInfo.ItemIndexes.Count == 0 ) {
                    continue;    // nothing more to do if there are 0 items in the group
                }
                int groupDataIndex = groupDataStartIndex + g;
                MarketPlan.VariantInfo newVinfo = new MarketPlan.VariantInfo( groupInfo.Name, groupDataIndex );
                newVinfo.IsGroup = true;
                marketPlan.Variants.Add( newVinfo );
                //foreach( MarketPlan.Component component in marketPlan.Components ) {
                //    Console.WriteLine( "Adding group name \"{0}\" to component.Variants ({1})", groupInfo.Name, component.Name );
                //    component.Variants.Add( newVinfo );
                //}
            }

            // the algorithm for grouping prices requires the %ACV DIST data 
            MarketPlan.Component acvDist = marketPlan.GetComponent( "%ACV DIST" );

            // now actually do the combining of the group data
            for( int g = 0; g < nGroups; g++ ) {
                Settings.GroupInfo groupInfo = (Settings.GroupInfo)settings.Groups[ g ];
                if( groupInfo.ItemIndexes.Count == 0 ) {
                    continue;    // nothing more to do if there are 0 items in the group
                }
                int groupDataIndex = groupDataStartIndex + g;
                MarketPlan.VariantInfo newVinfo = new MarketPlan.VariantInfo( groupInfo.Name, groupDataIndex );
                //marketPlan.Variants.Add( newVinfo );

                //we know the items to combine and the destination - do each component
                foreach( MarketPlan.Component component in marketPlan.Components ) {

                    // determine which type of grouping algorithm to apply
                    bool groupByTotal = false;
                    bool groupByMean = false;
                    bool groupByProbability = false;

                    if( (component.Name == "AVG UNIT PRICE") ||
                        (component.Name == "ANY PROMO UNIT PRICE" ) ||
                        (component.Name == "NON-PROMO UNIT PRICE" ) ||
                        (component.Name == "DISPLAY PRICE ABSOLUTE" ) ){

                        groupByMean = true;
                    }
                    else if( component.Name == "UNITS" ) {
                        groupByTotal = true;
                    }
                    else if( component.Name.StartsWith( "%ACV" ) ) {

                        groupByProbability = true;
                    }
                    else {
                        string w = String.Format( "Warning: No grouping algorithm known for measure named {0}", component.Name );
                        Console.WriteLine( w );
                        MarketPlan.warnings.Add( w );
                    }
  //                  component.Variants.Add( newVinfo );

                    // actually apply the combining algorithm
                    if( groupByTotal ) {
                        CombineDataByIntegerTotal( component.Data, groupInfo, groupDataIndex );
                    }
                    else if( groupByMean ) {
                        if( acvDist != null ) {
                            CombineDataByWeightedAverage( component.Data, acvDist.Data, groupInfo, groupDataIndex );
                        }
                        else {
                            CombineDataByMean( component.Data, groupInfo, groupDataIndex );
                        }
                    }
                    else if( groupByProbability ) {
                        CombineDataByProbability( component.Data, groupInfo, groupDataIndex );
                    }
                }
           }
        }

        /// <summary>
        /// Combines the lists of data into the given result list using the given group info.  Uses an integer summing algorithm.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupInfo"></param>
        /// <param name="groupedDataIndex"></param>
        public static void CombineDataByIntegerTotal( ArrayList[] data, Settings.GroupInfo groupInfo, int groupedDataIndex ) {

            int dataCount = data[ groupedDataIndex ].Count;

            // process each point
            for( int i = 0; i < dataCount; i++ ) {
                int firstGroupedItemIndex = (int)groupInfo.ItemIndexes[ 0 ];
                MarketPlan.Component.Item firstDataitem = (MarketPlan.Component.Item)data[ firstGroupedItemIndex ][ i ];
                DateTime start = firstDataitem.StartDate;
                DateTime end = firstDataitem.EndDate;

                int total = 0;
                foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];
                    double val = dataitem.Value1;
                    total += (int)Math.Round( val );
                }

                MarketPlan.Component.Item combinedItem = new MarketPlan.Component.Item( start, end, total );
                data[ groupedDataIndex ][ i ] = combinedItem;        //the result is the total
            }
        }

        /// <summary>
        /// Combines the lists of data into the given result list using the given group info.  Uses an averaging algorithm.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupInfo"></param>
        /// <param name="groupedDataIndex"></param>
        public static void CombineDataByMean( ArrayList[] data, Settings.GroupInfo groupInfo, int groupedDataIndex ) {

            int dataCount = data[ groupedDataIndex ].Count;

            // process each point
            for( int i = 0; i < dataCount; i++ ) {
                int firstGroupedItemIndex = (int)groupInfo.ItemIndexes[ 0 ];
                MarketPlan.Component.Item firstDataitem = (MarketPlan.Component.Item)data[ firstGroupedItemIndex ][ i ];
                DateTime start = firstDataitem.StartDate;
                DateTime end = firstDataitem.EndDate;

                double total = 0;
                double num = 0;
                foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];

                    // only avearage positive values

                    if (dataitem.Value1 > 0)
                    {
                        total += dataitem.Value1;
                        num += 1;
                    }
                }

                MarketPlan.Component.Item combinedItem = new MarketPlan.Component.Item( start, end, total / num ); //the result is the average
                data[ groupedDataIndex ][ i ] = combinedItem;                
           }
        }


        /// <summary>
        /// Combines the lists of data into the given result list using the given group info.  Uses an averaging algorithm.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupInfo"></param>
        /// <param name="groupedDataIndex"></param>
        public static void CombineDataByWeightedAverage( ArrayList[] data, ArrayList[] weightData, Settings.GroupInfo groupInfo, int groupedDataIndex ) {

            int dataCount = data[ groupedDataIndex ].Count;

            // process each point
            for( int i = 0; i < dataCount; i++ ) {
                int firstGroupedItemIndex = (int)groupInfo.ItemIndexes[ 0 ];
                MarketPlan.Component.Item firstDataitem = (MarketPlan.Component.Item)data[ firstGroupedItemIndex ][ i ];
                DateTime start = firstDataitem.StartDate;
                DateTime end = firstDataitem.EndDate;

                double total = 0;
                double totalWeight = 0;
                foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];
                    MarketPlan.Component.Item weightitem = (MarketPlan.Component.Item)weightData[ groupedItemIndex ][ i ];

                    // only avearage positive values

                    if( dataitem.Value1 > 0 ) {
                        total += dataitem.Value1 * weightitem.Value1;
                        totalWeight += weightitem.Value1;
                    }
                }

                MarketPlan.Component.Item combinedItem = new MarketPlan.Component.Item( start, end, total / totalWeight ); //the result is the average
                data[ groupedDataIndex ][ i ] = combinedItem;
            }
        }

        /// <summary>
        /// Combines the lists of data into the given result list using the given group info.  Uses a probability-combining algorithm.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupInfo"></param>
        /// <param name="groupedDataIndex"></param>
        public static void CombineDataByProbability( ArrayList[] data, Settings.GroupInfo groupInfo, int groupedDataIndex ) {

            int dataCount = data[ groupedDataIndex ].Count;

            double correlation = groupInfo.Correlation;

            // process each point
            for( int i = 0; i < dataCount; i++ ) {
                int firstGroupedItemIndex = (int)groupInfo.ItemIndexes[ 0 ];
                MarketPlan.Component.Item firstDataitem = (MarketPlan.Component.Item)data[ firstGroupedItemIndex ][ i ];
                DateTime start = firstDataitem.StartDate;
                DateTime end = firstDataitem.EndDate;

                // compute the correlated probability
                double max = 0;
                foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];
                    double val = dataitem.Value1;
                    max = Math.Max( val, max );
                }

                // compute the uncorrelated probability
                double pctFactor = 100;
                //foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                //    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];
                //    if( dataitem.Value1 > 1 ) {                     // treat values as percentages if any are over 1.0
                //        pctFactor = 100;
                //    }
                //}
                double accum = 0;
                foreach( int groupedItemIndex in groupInfo.ItemIndexes ) {
                    MarketPlan.Component.Item dataitem = (MarketPlan.Component.Item)data[ groupedItemIndex ][ i ];
                    double remaining = 1.0 - accum;
                    double amt = remaining * (dataitem.Value1 / pctFactor);
                    accum += amt;
                }
                accum *= pctFactor;

                // interpolate based on correlation setting
                double cval = ((accum - max) * (1 - correlation)) + max;

                MarketPlan.Component.Item combinedItem = new MarketPlan.Component.Item( start, end, cval );
                data[ groupedDataIndex ][ i ] = combinedItem;
            }
        }
    }
}
