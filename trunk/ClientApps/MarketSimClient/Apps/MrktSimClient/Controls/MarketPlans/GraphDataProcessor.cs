using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Data;

using Utilities.Graphing;
using MarketSimUtilities;
using MrktSimDb;

namespace MrktSimClient.Controls.MarketPlans
{
    public class GraphDataProcessor
    {
        private Color[] graphColorList = new Color[] { Color.DarkBlue, Color.Orange, Color.Green, Color.Purple, Color.Blue, Color.DarkOrange, Color.DarkRed, Color.DarkTurquoise };
        private int curGraphColor = -1;
        
        private MarketPlanControlFilter filter;
        private int[] planComponentIDs;
        private ModelDb theDb;

        // we use these only to lookup the actual DataTable type to be used for a given component type (is there a better way???)
        private MrktSimGrid[] mrktSimGrids;

        /// <summary>
        /// Create a new graph data processor object
        /// </summary>
        /// <param name="planComponentIDs"></param>
        /// <param name="theDb"></param>
        /// <param name="filter"></param>
        /// <param name="mrktSimGrids"></param>
        public GraphDataProcessor( int[] planComponentIDs, ModelDb theDb, MarketPlanControlFilter filter, MrktSimGrid[] mrktSimGrids ) {
            this.filter = filter;
            this.planComponentIDs = planComponentIDs;
            this.theDb = theDb;
            this.mrktSimGrids = mrktSimGrids;
        }

        #region Primary Processing Method
        /// <summary>
        /// Generate the actual DataCurve objects to be displayed
        /// </summary>
        /// <param name="combineSimilarTypes"></param>
        /// <returns></returns>
        public GraphData ProcessCurves( bool combineSimilarTypes ) {
            GraphData data = new GraphData();

            int nPlans = planComponentIDs.Length;
            MrktSimDBSchema.market_planRow[] plans = new MrktSimDBSchema.market_planRow[ nPlans ];
            int[] typeCount = new int[ Enum.GetNames( typeof( ModelDb.PlanType ) ).Length ];
            int[] pass2TypeCount = new int[ Enum.GetNames( typeof( ModelDb.PlanType ) ).Length ];
            for( int i = 0; i < typeCount.Length; i++ ) {
                typeCount[ i ] = 0;
                pass2TypeCount[ i ] = 0;
            }

            // pass 1 - determine what plan types we have 
            for( int i = 0; i < nPlans; i++ ) {
                int graphPlanID = planComponentIDs[ i ];
                plans[ i ] = theDb.Data.market_plan.FindByid( graphPlanID );
                int typeNumber = plans[ i ].type;
                ModelDb.PlanType planType = (ModelDb.PlanType)typeNumber;

                typeCount[ typeNumber ] = typeCount[ typeNumber ] + 1;  //increment the count of this plan type

                // set up the graph title
                if( data.Title == null ) {
                    data.Title = planType.ToString();
                }
                else if( !data.Title.Contains( planType.ToString() ) ) {
                    data.Title += ", " + planType.ToString();
                }
            }

            // decide how many curves we will be displaying
            int nCurves = nPlans;
            if( nPlans == 1 ) {
                if( typeCount[ (int)ModelDb.PlanType.Price ] == 1 ) {
                    // we have just a single price component -- create promoted and umpromoted price curves
                    nCurves = 2;
                }
            }

            // pass 2 
            for( int i = 0; i < plans.Length; i++ ) {
                int graphPlanID = planComponentIDs[ i ];
                MrktSimDBSchema.market_planRow plan = plans[ i ];
                int typeNumber = plans[ i ].type;
                ModelDb.PlanType planType = (ModelDb.PlanType)typeNumber;
                string planFilter = filter.MarketSingleComponentDataQuery( graphPlanID, planType );
                Color graphColor = graphColorList[ i % graphColorList.Length ];

                pass2TypeCount[ typeNumber ] = pass2TypeCount[ typeNumber ] + 1;  //increment the count of this plan type for this pass

                DataTable table = mrktSimGrids[ (int)planType ].Table;
                if( table == null ) {
                    continue;
                }

                // get the name of the channel
                DataRow[] cRow = theDb.Data.channel.Select( "channel_id = " + plan.channel_id );               // should always return 1 row
                string chanName = "?";
                if( cRow.Length == 1 ) {
                    chanName = (string)cRow[ 0 ][ "channel_name" ];
                }

                bool accumulatePartial = pass2TypeCount[ plan.type ] < typeCount[ plan.type ];

                switch( planType ) {
                    case ModelDb.PlanType.Display:
                        CreateDisplayCurves( table, planFilter, plan.name, data, accumulatePartial );
                        break;
                    case ModelDb.PlanType.Distribution:
                        CreateDistributionCurves( table, planFilter, plan.name, data, accumulatePartial );
                        break;
                    case ModelDb.PlanType.Price:
                        CreatePriceCurves( table, planFilter, plan.name, data, accumulatePartial );
                        break;
                    case ModelDb.PlanType.Media:
                        CreateMediaCurves( table, planFilter, plan.name, data, accumulatePartial, chanName );
                        break;
                    case ModelDb.PlanType.Coupons:
                        CreateCouponsCurves( table, planFilter, plan.name, data, accumulatePartial, chanName );
                        break;
                    case ModelDb.PlanType.ProdEvent:
                        CreateExternalFactorCurves( table, planFilter, plan.name, data, accumulatePartial, chanName );
                        break;
                    case ModelDb.PlanType.Market_Utility:
                        CreateMarketUtilityCurves( table, planFilter, plan.name, data, accumulatePartial );
                        break;
                }
            }
            return data;
        }
        #endregion

        #region Plan Component Curve Creation
        /// <summary>
        /// Returns a list of curves that represent the given price plan component data.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="planFilter"></param>
        /// <param name="planName"></param>
        /// <param name="data"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private ArrayList CreatePriceCurves( DataTable table, string planFilter, string planName, GraphData graphData, bool partial ) {

            string valueColumn = "price";
            string valueUnits = "$";

            ValidationData vdata = Validator.ValidateComponentData( theDb, false, ModelDb.PlanType.Price, planFilter, planName );
            for( int k = 0; k < vdata.CurveCount; k++ ) {
                if( vdata.CurvePointsCount[ k ] != 0 ){
                    DataRow[] curveTableData = vdata.CurveTableData[ k ];
                    int curvePoints = (4 * curveTableData.Length) + 2;
                    DataCurve dataCurve = new DataCurve( curvePoints );
                    dataCurve.Color = NextCurveColor();
                    dataCurve.Label = vdata.CurveDescription[ k ];
                    dataCurve.Units = valueUnits;
                    AddDataToCurve( dataCurve, curvePoints, curveTableData, valueColumn, graphData, false, true );
                    graphData.Curves.Add( dataCurve );
                    graphData.Legends.Add( dataCurve.Label );
                    graphData.LegendColors.Add( dataCurve.Color );
                }
            }
            return graphData.Curves;
        }

        /// <summary>
        /// Returns a list of curves that represent the given display plan component data.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="planFilter"></param>
        /// <param name="planName"></param>
        /// <param name="data"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private ArrayList CreateDisplayCurves( DataTable table, string planFilter, string planName, GraphData graphData, bool partial ) {

            string valueColumn = "attr_value_F";
            string valueUnits = "%";

            ValidationData vdata = Validator.ValidateComponentData( theDb, false, ModelDb.PlanType.Display, planFilter, planName );
            for( int k = 0; k < vdata.CurveCount; k++ ) {
                if( vdata.CurvePointsCount[ k ] != 0 ) {
                    DataRow[] curveTableData = vdata.CurveTableData[ k ];
                    int curvePoints = (4 * curveTableData.Length) + 2;
                    DataCurve dataCurve = new DataCurve( curvePoints );
                    dataCurve.Color = NextCurveColor();
                    dataCurve.Label = vdata.CurveDescription[ k ];
                    dataCurve.Units = valueUnits;
                    AddDataToCurve( dataCurve, curvePoints, curveTableData, valueColumn, graphData );
                    graphData.Curves.Add( dataCurve );
                    graphData.Legends.Add( dataCurve.Label );
                    graphData.LegendColors.Add( dataCurve.Color );
                }
            }
            return graphData.Curves;
        }

        /// <summary>
        /// Returns a list of curves that represent the given distribution plan component data.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="planFilter"></param>
        /// <param name="planName"></param>
        /// <param name="data"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private ArrayList CreateDistributionCurves( DataTable table, string planFilter, string planName, GraphData graphData, bool partial ) {

            string valueColumn = "attr_value_F";
            string valueUnits = "%";

            ValidationData vdata = Validator.ValidateComponentData( theDb, false, ModelDb.PlanType.Distribution, planFilter, planName );
            for( int k = 0; k < vdata.CurveCount; k++ ) {
                if( vdata.CurvePointsCount[ k ] != 0 ) {
                    DataRow[] curveTableData = vdata.CurveTableData[ k ];
                    int curvePoints = (4 * curveTableData.Length) + 2;
                    DataCurve dataCurve = new DataCurve( curvePoints );
                    dataCurve.Color = NextCurveColor();
                    dataCurve.Label = vdata.CurveDescription[ k ];
                    dataCurve.Units = valueUnits;
                    AddDataToCurve( dataCurve, curvePoints, curveTableData, valueColumn, graphData, false, true );
                    graphData.Curves.Add( dataCurve );
                    graphData.Legends.Add( dataCurve.Label );
                    graphData.LegendColors.Add( dataCurve.Color );
                }
            }
            return graphData.Curves;
        }

        private ArrayList CreateMarketUtilityCurves( DataTable table, string planFilter, string planName, GraphData graphData, bool partial ) {

            string valueColumn = "percent_dist";
            string valueUnits = "Value";

            ValidationData vdata = Validator.ValidateComponentData( theDb, false, ModelDb.PlanType.Market_Utility, planFilter, planName );
            for( int k = 0; k < vdata.CurveCount; k++ ) {
                if( vdata.CurvePointsCount[ k ] != 0 ) {
                    DataRow[] curveTableData = vdata.CurveTableData[ k ];
                    int curvePoints = (4 * curveTableData.Length) + 2;
                    DataCurve dataCurve = new DataCurve( curvePoints );
                    dataCurve.Color = NextCurveColor();
                    dataCurve.Label = vdata.CurveDescription[ k ];
                    dataCurve.Units = valueUnits;
                    AddDataToCurve( dataCurve, curvePoints, curveTableData, valueColumn, graphData, false, true );
                    graphData.Curves.Add( dataCurve );
                    graphData.Legends.Add( dataCurve.Label );
                    graphData.LegendColors.Add( dataCurve.Color );
                }
            }
            return graphData.Curves;
        }



        private ArrayList CreateExternalFactorCurves( DataTable table, string planFilter, string planName, GraphData graphData, bool partial, string channelName ) {
            DataRow[] tableData = table.Select( planFilter, "start_date" );
            if( tableData.Length == 0 ) {
                return null;
            }

            int curvePoints = (4 * tableData.Length) + 2;
            DataCurve dataCurve = new DataCurve( curvePoints );
            dataCurve.Color = NextCurveColor();

            // configure the curve
            dataCurve.Label = Validator.GraphNameFor( "Ext. Factors", planName, channelName, null );
            dataCurve.Units = "Value";
            string valueColumn = "demand_modification";

            AddDataToCurve( dataCurve, curvePoints, tableData, valueColumn, graphData, true, true );

            ArrayList curves = new ArrayList();
            curves.Add( dataCurve );
            graphData.Legends.Add( dataCurve.Label );
            graphData.LegendColors.Add( dataCurve.Color );
            graphData.Curves.AddRange( curves );
            return graphData.Curves;
        }

            /// <summary>
            /// Returns a list of curves that represent the given media plan component data.
            /// </summary>
            /// <param name="table"></param>
            /// <param name="planFilter"></param>
            /// <param name="planName"></param>
            /// <param name="data"></param>
            /// <param name="partial"></param>
            /// <returns></returns>
        private ArrayList CreateMediaCurves( DataTable table, string planFilter, string planName, GraphData data, bool partial, string channelName ) {
            DataRow[] tableData = table.Select( planFilter, "start_date" );
            if( tableData.Length == 0 ) {
                return null;
            }

            int curvePoints = (4 * tableData.Length) + 2;
            DataCurve dataCurve = new DataCurve( curvePoints );
            dataCurve.Color = NextCurveColor();

            // configure the curve
            dataCurve.Label = Validator.GraphNameFor( "GRPs", planName, channelName, null );
            dataCurve.Units = "GRP";
            string valueColumn = "attr_value_G";

            AddDataToCurve( dataCurve, curvePoints, tableData, valueColumn, data, true, true );

            ArrayList curves = new ArrayList();
            curves.Add( dataCurve );
            data.Legends.Add( dataCurve.Label );
            data.LegendColors.Add( dataCurve.Color );
            data.Curves.AddRange( curves );
            return curves;
        }

        /// <summary>
        /// Returns a list of curves that represent the given coupons plan component data.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="planFilter"></param>
        /// <param name="planName"></param>
        /// <param name="data"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private ArrayList CreateCouponsCurves( DataTable table, string planFilter, string planName, GraphData data, bool partial, string channelName ) {
            DataRow[] tableData = table.Select( planFilter, "start_date" );
            if( tableData.Length == 0 ) {
                return null;
            }

            int curvePoints = (4 * tableData.Length) + 2;
            DataCurve dataCurve = new DataCurve( curvePoints );
            dataCurve.Color = NextCurveColor();

            // configure the curve
            dataCurve.Label = Validator.GraphNameFor( "Coupons", planName, channelName, null );
            dataCurve.Units = "%";
            string valueColumn = "attr_value_G";

            AddDataToCurve( dataCurve, curvePoints, tableData, valueColumn, data, false, true );

            ArrayList curves = new ArrayList();
            curves.Add( dataCurve );
            data.Legends.Add( dataCurve.Label );
            data.LegendColors.Add( dataCurve.Color );
            data.Curves.AddRange( curves );
            return curves;
        }
        #endregion

         /// <summary>
        /// Adds the data in the given column of the given data rows to the given curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="numCurvePoints"></param>
        /// <param name="tableData"></param>
        /// <param name="valueColumn"></param>
        /// <param name="overallData"></param>
        private void AddDataToCurve( DataCurve curve, int numCurvePoints, DataRow[] tableData, string valueColumn, GraphData overallData ) {
            AddDataToCurve( curve, numCurvePoints, tableData, valueColumn, overallData, false, false );
        }

        /// <summary>
        /// Adds the data in the given column of the given data rows to the given curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="numCurvePoints"></param>
        /// <param name="tableData"></param>
        /// <param name="valueColumn"></param>
        /// <param name="overallData"></param>
        private void AddDataToCurve( DataCurve curve, int numCurvePoints, DataRow[] tableData, string valueColumn, GraphData overallData, bool normalizeByDuration, bool stepwiseStyle ) {
            DateTime prevEndDate = DateTime.MinValue;
            double prevValue = -1;

            int nPointsAdded = 0;

            // loop over the set of points
            for( int j = 0; j < tableData.Length; j++ ) {

                DataRow datarow = tableData[ j ];

                DateTime startDate = (DateTime)datarow[ "start_date" ];
                DateTime endDate = (DateTime)datarow[ "end_date" ];
                endDate = endDate.AddDays( 1 );      //include the last day in the segment

                double valF = (double)datarow[ valueColumn ];
                if( normalizeByDuration ) {
                    TimeSpan range = endDate - startDate;
                    valF = valF / range.TotalDays;
                }

                bool addLeadingZero = false;
                if( (j != 0) && (prevEndDate != startDate) ) {
                    addLeadingZero = true;
                }

                // add a leading zero if the data isn't continuous
                if( addLeadingZero ) {
                    curve.Add( prevEndDate, 0 );
                    curve.Add( startDate, 0 );
                    nPointsAdded += 2;
                }
                else if( j == 0 ) {
                    curve.Add( startDate, 0 );
                    nPointsAdded += 1;
                }

                // add the actual data point 
                curve.Add( startDate, valF );

                if( stepwiseStyle == true ) {
                    // add a horizontal segment from the start to the end date
                    curve.Add( endDate, valF );
                }

                prevValue = valF;
                prevEndDate = endDate;

                // track overall limits
                if( valF > overallData.Max ) {
                    overallData.Max = valF;
                }
                if( startDate < overallData.Start ) {
                    overallData.Start = startDate;
                }
                if( endDate > overallData.End ) {
                    overallData.End = endDate;
                }
            }
            // pad out the end of the curve data with duplicates of the last point
            if( nPointsAdded < numCurvePoints - 1 ) {
                for( int ix = 0; ix < (numCurvePoints - nPointsAdded); ix++ ) {
                    if( stepwiseStyle == true ) {
                        curve.Add( prevEndDate, 0 );
                    }
                    else {
                        curve.Add( prevEndDate, prevValue );
                    }
                }
            }

            // add a trailing zero to close the area (so the curve can be filled)
            if( stepwiseStyle == true ) {
                curve.Add( prevEndDate, 0 );
            }
            else {
                curve.Add( prevEndDate, prevValue );
            }
        }

        private Color NextCurveColor() {
            curGraphColor = (curGraphColor + 1)  % graphColorList.Length;
            return graphColorList[ curGraphColor ];
        }
    }
 
    /// <summary>
    /// GraphData encapsulates the results of processing by GraphDataProcessor
    /// </summary>
    public class GraphData
    {
        public string Title;
        public DateTime Start;
        public DateTime End;
        public double Max;
        public ArrayList Curves;
        public ArrayList Legends;
        public ArrayList LegendColors;

        public GraphData() {
            this.Curves = new ArrayList();
            this.Legends = new ArrayList();
            this.LegendColors = new ArrayList();
            this.Start = DateTime.MaxValue;
            this.End = DateTime.MinValue;
            this.Max = -1;
            this.Title = null;
        }
    }
}
