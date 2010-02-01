using System;
using System.Collections;
using System.Text;

using System.Data;

using MrktSimDb;

namespace MrktSimClient.Controls.MarketPlans
{
    /// <summary>
    /// Validator has methods for validating market plan component data.
    /// </summary>
    public class Validator
    {
        private static string curveNameFormat = "{0}: {1} [{2}]";
        private static string curveNameWithInfoFormat = "{0}: {1} - {3} [{2}]";

        /// <summary>
        /// Returns an object describing the validation status of the market plan component data of the given type.  
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="summaryOnly"></param>
        /// <param name="planType"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        /// <remarks>The data is specified by means of a filter string so it can also be constrained by other parameters such as date range, etc.</remarks>
        public static ValidationData ValidateComponentData( ModelDb theDb, bool summaryOnly, ModelDb.PlanType planType, string filterString, string planName ) {
            return ValidateComponentData( theDb, summaryOnly, planType, filterString, null, planName );
        }

        /// <summary>
        /// Returns an object describing the validation status of the market plan component data of the given type.  
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="summaryOnly"></param>
        /// <param name="planType"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        /// <remarks>The data is specified by means of a filter string so it can also be constrained by other parameters such as date range, etc.</remarks>
        public static ValidationData ValidateComponentData( ModelDb theDb, bool summaryOnly, ModelDb.PlanType planType, string filterString,
            MrktSimDBSchema.market_planRow plan ) {
            return ValidateComponentData( theDb, summaryOnly, planType, filterString, plan, null );
        }

        /// <summary>
        /// Returns an object describing the validation status of the market plan component data of the given type.  
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="summaryOnly"></param>
        /// <param name="planType"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        /// <remarks>The data is specified by means of a filter string so it can also be constrained by other parameters such as date range, etc.</remarks>
        private static ValidationData ValidateComponentData( ModelDb theDb, bool summaryOnly, ModelDb.PlanType planType, string filterString,
            MrktSimDBSchema.market_planRow plan, string planName ) {

            ////long t0 = System.Environment.TickCount;
         
            ValidationData results = new ValidationData();

            string curveLabelPrefix = "";

            DataTable table = null;
            switch( planType ) {
                case ModelDb.PlanType.Coupons:
                    table = theDb.Data.mass_media;
                    curveLabelPrefix = "Coupon GRPs";
                    break;
                case ModelDb.PlanType.Display:
                    table = theDb.Data.display;
                    curveLabelPrefix = "% Display";
                    break;
                case ModelDb.PlanType.Distribution:
                    table = theDb.Data.distribution;
                    curveLabelPrefix = "% Dist";
                    break;
                case ModelDb.PlanType.Media:
                    table = theDb.Data.mass_media;
                    curveLabelPrefix = "Media GRPs";
                    break;
                case ModelDb.PlanType.Price:
                    table = theDb.Data.product_channel;
                    curveLabelPrefix = "Price";
                    break;
                case ModelDb.PlanType.ProdEvent:
                    // ???can we validate External Factors???
                    Console.WriteLine( "\nWARNING: Attempt to validate External Factors as a Component -- ignored" );
                    return results;
                case ModelDb.PlanType.Market_Utility:
                    table = theDb.Data.market_utility;
                    curveLabelPrefix = "Market Utility";
                    break;
                case ModelDb.PlanType.MarketPlan:
                    // ???why are trying to validate a plan as a component???
                    Console.WriteLine( "\nWARNING: Attempt to validate Market Plan as a Component -- ignored" );
                    return results;
            }

            DataRow[] tableData = table.Select( filterString, "start_date" );

            ////long t1 = System.Environment.TickCount;

            if( tableData.Length == 0 ) {
                return results;
            }

            results = new ValidationData( 1 );
            results.PointsCount = tableData.Length;
            DateTime prevStartDate = (DateTime)tableData[ 0 ][ "start_date" ];
            DateTime prevEndDate = (DateTime)tableData[ 0 ][ "end_date" ];
            prevEndDate = prevEndDate.AddDays( 1 );
            results.StartDate = prevStartDate;
            results.CurvePointsCount[ 0 ] = tableData.Length;
            if( summaryOnly == false ) {
                results.CurveTableData[ 0 ] = tableData;
            }
            results.CurveQuery[ 0 ] = filterString;


            // get the name of the plan
            if( planName == null ) {
                if( plan != null ) {
                    planName = plan.name;
                }
                else {
                    planName ="Error! null plan!";
                }
            }

            // get the name of the channel
            int channelID = (int)tableData[ 0 ][ "channel_id" ];
            DataRow[] cRow = theDb.Data.channel.Select( "channel_id = " + channelID );               // should always return 1 row
            string chanName = "?";
            if( cRow.Length == 1 ) {
                chanName = (string)cRow[ 0 ][ "channel_name" ];
            }
            results.CurveDescription[ 0 ] = GraphNameFor( curveLabelPrefix, planName, chanName, null );

            ////long t_pre_loop = System.Environment.TickCount;

            // loop through the data rows
            for( int i = 1; i < tableData.Length; i++ ) {
                DateTime startDate = (DateTime)tableData[ i ][ "start_date" ];
                DateTime endDate = (DateTime)tableData[ i ][ "end_date" ];
                endDate = endDate.AddDays( 1 );       //MarketSim end dates are the day before the start of the next contiguous period
                if( endDate > results.EndDate ) {
                    results.EndDate = endDate;
                }

                // see if this interval doesn't continue the previous one
                if( startDate > prevEndDate ) {
                    results.HasGaps = true;
                    results.GapStartDates.Add( prevEndDate );
                    results.GapEndDates.Add( startDate );
                }
                // see if the previous interval overlaps this one
                else if( startDate < prevEndDate ) {

                    // overlap may be caused my data with multiple curves - run multicurve validation if appropriate
                    bool scanForMultiCurve = OverlapCauseIsChannelOrPriceType( tableData, i, planType );
                    if( scanForMultiCurve == true ) {
                        ArrayList baseFiilters = BaseFiltersFor( filterString, planType );

                        // return multicurve validation results here
                        return ValidateComponentMultiCurveData( theDb, summaryOnly, table, planType, AllChannelsIn( tableData ), baseFiilters, curveLabelPrefix, planName );
                    }

                    results.HasOverlap = true;
                    results.OverlapStartDates.Add( startDate );
                    DateTime overlapEnd = prevEndDate;
                    if( endDate < prevEndDate ) {
                        overlapEnd = endDate;
                    }
                    results.OverlapEndDates.Add( prevEndDate );
                }

                prevStartDate = startDate;
                prevEndDate = endDate;
            }

            ////long t_post_loop = System.Environment.TickCount;

            results.SetSummaryFor( planType, plan );
            ////long t5 = System.Environment.TickCount;

            ////string info = String.Format( "validation: {0}", filterString );
            ////string info2 = String.Format( "validation: {0}, {1}, {2}, {3}\n", t1 - t0, t_pre_loop - t1, t_post_loop - t_pre_loop, t5 - t_post_loop );
            ////Utilities.DataLogger.Log( info );
            ////Utilities.DataLogger.Log( info2 );

            return results;
        }

        /// <summary>
        /// Checks to see if an apparent overlap in data intervals is in fact caused by multiple curves in the data 
        /// (multiple channels and/or price types).
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="overlapIndex"></param>
        /// <returns></returns>
        private static bool OverlapCauseIsChannelOrPriceType( DataRow[] tableData, int overlapIndex, ModelDb.PlanType planType ) {
            bool res = false;

            int chanID = (int)tableData[ overlapIndex ][ "channel_id" ];
            int prevChanID = (int)tableData[ overlapIndex - 1 ][ "channel_id" ];
            if( chanID != prevChanID ) {
                res = true;
            }
            else if( planType == ModelDb.PlanType.Price ) {
                int priceType = (int)tableData[ overlapIndex ][ "price_type" ];
                int  prevPriceType = (int)tableData[overlapIndex - 1]["price_type"];
                if( prevPriceType != priceType ) {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Returns the set of filters needed for a given component data type, assuming there is no multi-channel data
        /// </summary>
        /// <param name="filterString"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        private static ArrayList BaseFiltersFor( string filterString, ModelDb.PlanType planType ) {
            ArrayList filters = new ArrayList();
            if( planType != ModelDb.PlanType.Price ) {
                filters.Add( filterString );
            }
            else {
                string unpromotedPlanFilter = filterString + " AND (price_type  = -1)";
                string promotedPlanFilter = filterString + " AND (price_type  <> -1)";
                filters.Add( unpromotedPlanFilter );
                filters.Add( promotedPlanFilter );
            }
            return filters;
        }

        /// <summary>
        /// Returns a list of all channels IDs in the given data
        /// </summary>
        /// <param name="tableData"></param>
        /// <returns></returns>
        private static ArrayList AllChannelsIn( DataRow[] tableData ) {
            ArrayList channels = new ArrayList();
            foreach( DataRow drow in tableData ) {
                int tstChan = (int)drow[ "channel_id" ];
                if( channels.Contains( tstChan ) == false ) {
                    channels.Add( tstChan );
                }
            }
            return channels;
        }

        /// <summary>
        /// Returns an object describing the validation status of the market plan component data of the given type.  
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="summaryOnly"></param>
        /// <param name="planType"></param>
        /// <param name="channels"></param>
        /// <param name="filterStrings"></param>
        /// <returns></returns>
        /// <remarks>The data is specified by means of filter strings so it can also be constrained by other parameters such as date range, price type, etc.</remarks>
        private static ValidationData ValidateComponentMultiCurveData( ModelDb theDb, bool summaryOnly, DataTable theTable,
            ModelDb.PlanType planType, ArrayList channels, ArrayList filterStrings, string legendPrefix, string planName ) {

            int nCurves = channels.Count * filterStrings.Count;
            string[] filters = new string[ nCurves ];
            string[] curveNames = new string[ nCurves ];
            int indx = 0;

            // build up the complete filters list and corresponding curve names
            foreach( string filterString in filterStrings ) {

                // price type info for the curve name
                string otherInfo = "" ;
                if( filterString.IndexOf( "'unpromoted'" ) != -1 ) {
                    otherInfo = "regular price";
                }
                else if( filterString.IndexOf( "'promoted'" ) != -1 ) {
                    otherInfo = "promo price";
                }

                // loop over all channels
                foreach( int channelID in channels ) {
                    // create the filter for a single curve
                    filters[ indx ] = String.Format( "{0} AND (channel_id = {1})", filterString, channelID );

                    // get the name of the channel
                    DataRow[] cRow = theDb.Data.channel.Select( "channel_id = " + channelID );               // should always return 1 row
                    string chanName = "?";
                    if( cRow.Length == 1 ) {
                        chanName = (string)cRow[ 0 ][ "channel_name" ];
                    }

                    // set the curve name
                    curveNames[ indx ] = GraphNameFor( legendPrefix, planName, chanName, otherInfo );
                    indx = indx + 1;
                }
            }

            ValidationData results = new ValidationData( nCurves );
            results.CurveDescription = curveNames;
            results.CurveQuery = filters;

            // validate each of the individual curves
            for( int i = 0; i < nCurves; i++ ) {
                ValidationData subResults = new ValidationData();
               DataRow[] tableData = theTable.Select( filters[ i ], "start_date" );
               if( summaryOnly == false ) {
                   results.CurveTableData[ i ] = tableData;
               }
               if( tableData.Length > 0 ) {
                   subResults = ValidateIndividualCurveData( tableData );
               }
               else {
                   subResults = new ValidationData();
               }
                // accumulate the sub-results into the overall results
               results.CurvePointsCount[ i ] = subResults.PointsCount;
               results.PointsCount += subResults.PointsCount;
               results.Ok &= subResults.Ok;
               results.HasGaps |= subResults.HasGaps;
               results.HasOverlap |= subResults.HasOverlap;
               results.GapStartDates.AddRange( subResults.GapStartDates );
               results.GapEndDates.AddRange( subResults.GapEndDates );
               results.OverlapStartDates.AddRange( subResults.OverlapStartDates );
               results.OverlapEndDates.AddRange( subResults.OverlapEndDates );
               if( subResults.StartDate < results.StartDate ) {
                   results.StartDate = subResults.StartDate;
               }
               if( subResults.EndDate < results.EndDate ) {
                   results.EndDate = subResults.EndDate;
               }
           }
           results.SetSummaryFor( planType, null );
           //results.Summary += String.Format( " {0} curves", nCurves );    //!!! DEBUG
           return results;
        }

        /// <summary>
        /// Validate an individual curve
        /// </summary>
        /// <param name="tableData"></param>
        /// <returns></returns>
        private static ValidationData ValidateIndividualCurveData( DataRow[] tableData ) {
            ValidationData results = new ValidationData();

            DateTime prevStartDate = (DateTime)tableData[ 0 ][ "start_date" ];
            DateTime prevEndDate = (DateTime)tableData[ 0 ][ "end_date" ];
            prevEndDate = prevEndDate.AddDays( 1 );

            results.PointsCount = tableData.Length;
            results.StartDate = prevStartDate;

            // loop through the data rows
            for( int i = 1; i < tableData.Length; i++ ) {
                DateTime startDate = (DateTime)tableData[ i ][ "start_date" ];
                DateTime endDate = (DateTime)tableData[ i ][ "end_date" ];
                endDate = endDate.AddDays( 1 );       //MarketSim end dates are the day before the start of the next contiguous period
                if( endDate > results.EndDate ) {
                    results.EndDate = endDate;
                }

                // see if this interval doesn't continue the previous one
                if( startDate > prevEndDate ) {
                    results.HasGaps = true;
                    results.GapStartDates.Add( prevEndDate );
                    results.GapEndDates.Add( startDate );
                }
                // see if the previous interval overlaps this one
                else if( startDate < prevEndDate ) {
                    results.HasOverlap = true;
                    results.OverlapStartDates.Add( startDate );
                    DateTime overlapEnd = prevEndDate;
                    if( endDate < prevEndDate ) {
                        overlapEnd = endDate;
                    }
                    results.OverlapEndDates.Add( prevEndDate );
                }
                prevStartDate = startDate;
                prevEndDate = endDate;
            }

            return results;
        }

        /// <summary>
        /// Returns a string formatted for a curve legend label.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="planName"></param>
        /// <param name="channel"></param>
        /// <param name="otherIno"></param>
        /// <returns></returns>
        public static string GraphNameFor( string prefix, string planName, string channel, string otherIno ) {
            if( otherIno == null ) {
                return String.Format( curveNameFormat, prefix, planName, channel );
            }
            else {
                return String.Format( curveNameWithInfoFormat, prefix, planName, channel, otherIno );
            }
        }
    }

    /// <summary>
    /// ValidationData not only describes the component's validity but also contains values useful for graphing the data.
    /// </summary>
    public class ValidationData
    {
        public bool Ok;
        public string Summary;

        public int PointsCount;
        public int CurveCount;
        public int[] CurvePointsCount;
        public string[] CurveDescription;
        public string[] CurveQuery;
        public DataRow[][] CurveTableData;

        public DateTime StartDate;
        public DateTime EndDate;

        public bool HasOverlap;
        public bool HasGaps;

        public ArrayList GapStartDates;
        public ArrayList GapEndDates;

        public ArrayList OverlapStartDates;
        public ArrayList OverlapEndDates;

        public ValidationData( int nCurves ) : this() {
            this.CurveCount = nCurves;
            this.CurvePointsCount = new int[ nCurves ];
            this.CurveDescription = new string[ nCurves ];
            this.CurveQuery = new string[ nCurves ];
            this.CurveTableData = new DataRow[ nCurves ][];
        }

        public ValidationData() {
            this.Ok = true;
            this.Summary = "no values";
            this.HasOverlap = false;
            this.HasGaps = false;
            this.StartDate = DateTime.MaxValue;
            this.EndDate = DateTime.MinValue;
            this.PointsCount = 0;
            this.CurveCount = 0;
            this.GapStartDates = new ArrayList();
            this.GapEndDates = new ArrayList();
            this.OverlapStartDates = new ArrayList();
            this.OverlapEndDates = new ArrayList();
        }

        public void SetSummaryFor( ModelDb.PlanType planType, MrktSimDBSchema.market_planRow plan ) {

            this.Summary = SummarizePlanParms( plan );

            // create the appropriate status message
            switch( planType ) {
                case ModelDb.PlanType.Media:
                case ModelDb.PlanType.Coupons:
                    // these can have gaps and overlaps
                    if( this.Summary.Length == 0 ) {
                        this.Summary = "-";
                    }
                    break;
                case ModelDb.PlanType.Price:
                case ModelDb.PlanType.Distribution:
                case ModelDb.PlanType.Display:
                    if( this.HasOverlap == true ) {
                        if( this.Summary.Length > 0 ) {
                            this.Summary += "; ";
                        }
                        this.Summary += "OVERLAP! ";
                        for( int k = 0; k < this.OverlapStartDates.Count; k++ ) {
                            this.Summary += String.Format( "{0}-{1}",
                                ((DateTime)this.OverlapStartDates[ k ]).ToString( "M/d/yy" ),
                                (((DateTime)this.OverlapEndDates[ k ]).AddDays( -1 ).ToString( "M/d/yy" )) );
                        }
                    }
                    else if( this.HasGaps == true ) {
                        if( this.Summary.Length > 0 ) {
                            this.Summary += "; ";
                        }
                        this.Summary += "Gaps in data: ";
                        for( int k = 0; k < this.GapStartDates.Count; k++ ) {
                            this.Summary += String.Format( "{0}-{1}",
                                ((DateTime)this.GapStartDates[ k ]).ToString( "M/d/yy" ),
                                (((DateTime)this.GapEndDates[ k ]).AddDays( -1 ).ToString( "M/d/yy" )) );
                        }
                    }
                    else {
                        if( this.Summary.Length == 0 ) {
                            this.Summary = "ok";
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns a string describing the settings of parm1-6 in the given market plan component.
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private string SummarizePlanParms( MrktSimDBSchema.market_planRow plan ) {

            // return an empty string if no parms have non-default value
            if( plan == null || (plan.parm1 == 1 && plan.parm2 == 1 && plan.parm3 == 1 && plan.parm4 == 1 && plan.parm5 == 1 && plan.parm6 == 1) ) {
                return "";
            }

            string[] parmNames = null;
            string summary = "";

            // the available parameters depend on type of market plan
            switch( (ModelDb.PlanType)plan.type ) {
                case ModelDb.PlanType.Display:
                    parmNames = new string[] { "Awar", "Pers", "Dist", "Parm4", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.Distribution:
                    parmNames = new string[] { "Awar", "Pers", "Vals", "Parm4", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.Market_Utility:
                    parmNames = new string[] { "Awar", "Pers", "Util", "Dist", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.Media:
                    parmNames = new string[] { "Awar", "Pers", "GRP", "Parm4", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.Coupons:
                    parmNames = new string[] { "Awar", "Pers", "PctPop", "Redem", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.Price:
                    parmNames = new string[] { "Vals", "Mrkp", "PerPrc", "Dist", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.ProdEvent:
                    parmNames = new string[] { "Mod", "Parm2", "Parm3", "Parm4", "Parm5", "Parm6" };
                    break;

                case ModelDb.PlanType.TaskEvent:
                    parmNames = new string[] { "Mod", "Parm2", "Parm3", "Parm4", "Parm5", "Parm6" };
                    break;
            }

            bool firstParm = true;

            if( plan.parm1 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 0 ], plan.parm1, firstParm );
                firstParm = false;
            }
            if( plan.parm2 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 1 ], plan.parm2, firstParm );
                firstParm = false;
            }
            if( plan.parm3 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 2 ], plan.parm3, firstParm );
                firstParm = false;
            }
            if( plan.parm4 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 3 ], plan.parm4, firstParm );
                firstParm = false;
            }
            if( plan.parm5 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 4 ], plan.parm5, firstParm );
                firstParm = false;
            }
            if( plan.parm6 != 1 ) {
                summary += IndividualParmSummary( parmNames[ 5 ], plan.parm6, firstParm );
                firstParm = false;
            }

            return summary;
        }

        /// <summary>
        /// Returns a string describing a simgle parameterization.
        /// </summary>
        /// <param name="parmName"></param>
        /// <param name="parmValue"></param>
        /// <param name="isFirstParm"></param>
        /// <returns></returns>
        private string IndividualParmSummary( string parmName, double parmValue, bool isFirstParm ) {
            string s = "";
            if( isFirstParm == false ) {
                s = "; ";
            }
            string num = String.Format( "{0:f2}", parmValue );
            if( num.EndsWith( "0" ) ){
                num = num.Substring( 0, num.Length - 1 );
            }
            s += String.Format( "{1}*{0}", num, parmName );
            
            return s;
        }
    }
}
