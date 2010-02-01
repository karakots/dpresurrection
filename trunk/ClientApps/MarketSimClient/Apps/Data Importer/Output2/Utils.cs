using System;
using System.Collections;
using System.Text;

using DataImporter.ImportSettings;
using DataImporter.Library;

namespace DataImporter.Output2
{
    class Utils
    {
        /// <summary>
        /// Inserts a descriptive comment in cell A1 of the worksheet currently open by the given ExcelWriter2
        /// </summary>
        /// <param name="project"></param>
        /// <param name="worksheet"></param>
        /// <param name="writer"></param>
        public static void InsertFileInformationLine( ProjectSettings project, WorksheetSettings worksheet, ExcelWriter2 writer ) {

            string creationNotes = String.Format( "File automatically generated as part of \"{0}\" project at {1} on {2}.  Source sheet=\"{3}\", file=\"{4}\"",
                                                                    project.ProjectName, 
                                                                    DateTime.Now.ToString( "h:mm:ss t" ),
                                                                    DateTime.Now.ToString( "MMM-dd yyyy" ),
                                                                    worksheet.SheetName, 
                                                                    worksheet.InputFile );

            object[ , ] cellVals = new object[ 1, 1 ];
            cellVals[ 0, 0 ] = creationNotes;
            writer.SetValues( 1, 1, 1, 1, cellVals );
        }

        public static ArrayList[] GenerateVariantGroups( string[] variantNames, out int numGroups ) {
            ArrayList[] variantLists = new ArrayList[ variantNames.Length ];
            numGroups = 0;

            for( int v1 = 0; v1 < variantNames.Length; v1++ ) {
                if( variantLists[ v1 ] != null ) {
                    // this item is already a subitem
                    continue;
                }
                variantLists[ v1 ] = new ArrayList();
                variantLists[ v1 ].Add( v1 );  //include ourself

                if( variantNames[ v1 ] == null || variantNames[ v1 ].Trim() == "" ) {
                    // ignore blank variants for clarity
                    continue;
                }

                // see if other items use the same name
                for( int v2 = v1 + 1; v2 < variantNames.Length; v2++ ) {        // note this loop does nothing for the final point
                    if( variantNames[ v1 ] == variantNames[ v2 ] ) {
                        // found a match
                        Console.WriteLine( " GROUPED ITEM: " + variantNames[ v2 ] );
                        if( variantLists[ v1 ].Count == 1 ) {
                            numGroups += 1;
                        }
                        variantLists[ v1 ].Add( v2 );               // add the secind item's index to the group list

                        variantLists[ v2 ] = new ArrayList();        // flag the second item as a subitem
                    }
                }
            }

            return variantLists;
        }

        public enum ComponentType {
            Display,
            Distribution,
            Media,
            Coupons,
            Price,
            PriceDistribution,
            RealSales,
            MarketUtility
        }

        /// <summary>
        /// Returns the item, combining subitems of this item's group as specified
        /// </summary>
        /// <param name="item"></param>
        /// <param name="worksheetSettings"></param>
        /// <param name="groupInfoList"></param>
        /// <returns></returns>
        public static DataItem GetGroupedItem( DataItem item, int channelIndex, int dataItemIndex, WorksheetSettings worksheetSettings, ArrayList groupInfoList,
           ComponentType planType ) {

            DataItem groupedItem = new DataItem( item );

            double groupedValue1 = 0;
            double groupedValue2 = 0;

            int itemsInGroup = groupInfoList.Count;

            // add values in the group
            foreach( int itemIndex in groupInfoList ) {
                DataItem gItem = (DataItem)worksheetSettings.GetData( channelIndex, itemIndex )[ dataItemIndex ];

                groupedValue1 += gItem.Value1;
                groupedValue2 += gItem.Value2;
            }

            // convert sums to averages where applicable (all but GRPs)
            switch( planType ) {
                case ComponentType.Display:
                case ComponentType.PriceDistribution:
                case ComponentType.Distribution:
                case ComponentType.MarketUtility:
                case ComponentType.Media:
                case ComponentType.RealSales:
                    groupedValue1 /= (double)itemsInGroup;
                    groupedValue2 /= (double)itemsInGroup;
                    break;
            }

            groupedItem.Value1 = groupedValue1;
            groupedItem.Value2 = groupedValue2;
            return groupedItem;
        }
    }
}
