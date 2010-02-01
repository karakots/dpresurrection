using System;
using System.Collections.Generic;
using System.Text;

namespace MrktSimDb.Metrics
{
    /// <summary>
    /// MetricMan is a toplevel managment of metrics.
    /// this allows apps to read, evaluate, or write metrics
    /// and maintain some level of control without worrying about details
    /// </summary>
    public class MetricMan
    {
        #region static fields methods and properties

        // conversion utilities
        static public double Convert2Double( Object obj ) {
            if( obj.GetType() == typeof( double ) )
                return (double)obj;

            else if( obj.GetType() == typeof( int ) ) {
                int ival = (int)obj;

                return (double)ival;
            }

            double val = double.Parse( obj.ToString() );
            return val;

        }

        static public DateTime Convert2DateTime( Object obj ) {
            if( obj.GetType() == typeof( DateTime ) )
                return (DateTime)obj;

            DateTime val;
            try {
                val = DateTime.Parse( obj.ToString() );
            }
            catch( Exception ) {
                val = DateTime.MinValue;
            }


            return val;
        }

        /// <summary>
        /// initialization
        /// </summary>
        static MetricMan() {
            // Metric Groups
            Metric metric = null;
            const int numSimMetrics = 6;
            SimSummaryMetrics = new Metric[ numSimMetrics ];

            // Sim summary metrics
            metric = new SimSummary();
            SimSummaryMetrics[ 0 ] = metric;

            metric = new SimSummaryByProd();
            SimSummaryMetrics[ 1 ] = metric;

            metric = new SimSummaryByProdChan();
            SimSummaryMetrics[ 2 ] = metric;

            metric = new SimSummaryByProdChanSeg();
            SimSummaryMetrics[ 3 ] = metric;

            metric = new SimSummaryByProdSeg();
            SimSummaryMetrics[ 4 ] = metric;

            metric = new SimSummaryByChanSeg();
            SimSummaryMetrics[ 5 ] = metric;


            // calibration metrics
            const int numCalMetrics = 5;
            CalibrationMetrics = new Metric[ numCalMetrics ];

            metric = new CalSummaryByProd();
            CalibrationMetrics[ 0 ] = metric;

            metric = new CalSummaryByProdChan();
            CalibrationMetrics[ 1 ] = metric;

            metric = new CalSummary();
            CalibrationMetrics[ 2 ] = metric;

            metric = new CalSummaryByChan();
            CalibrationMetrics[ 3 ] = metric;

            metric = new CalMAPEByProdChan();
            CalibrationMetrics[ 4 ] = metric;


            // values we can evaluate seperately
            publicValues = new List<Value>();
            hiddenValues = new List<Value>();

            publicValues.Add(new ShareDiff());      
            publicValues.Add(new PercentError());   
            publicValues.Add(new SqrError());        
            publicValues.Add(new UnitsSold());      
            publicValues.Add(new UnitShare());
            publicValues.Add(new Dollars());        
            publicValues.Add(new DollarShare());     
            publicValues.Add(new MAPE());            
            publicValues.Add(new AttrRMS());        
            publicValues.Add(new RunNum());          
            publicValues.Add(new RealUnitsSold());  

            // added in for summary form
            // not needed (yet) in the variables form
            hiddenValues.Add(new NIMOTau() );
            hiddenValues.Add( new SalesRate() );
            hiddenValues.Add( new RealSalesRate() );
            hiddenValues.Add( new SalesRateR2() );
            hiddenValues.Add( new RealSalesRateR2() );
            hiddenValues.Add( new BasePrice() );
            hiddenValues.Add( new AvgPrice() );
            hiddenValues.Add( new AvgDistribution() );
            hiddenValues.Add( new AvgAwareness() );
            hiddenValues.Add( new AvgACVDisplay() );
        }

        // describes the metric groups

        public static Metric[] SimSummaryMetrics = null;
        public static Metric[] CalibrationMetrics = null;

        private static List<Value> publicValues;
        private static List<Value> hiddenValues;

        public static Value[] MetricValues {
            get {
                return publicValues.ToArray();
            }
        }


        /// <summary>
        /// returns a generic metric
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Metric GetMetric( string token ) {
            if( token == null )
                return null;

            foreach( Metric metric in SimSummaryMetrics ) {
                if( metric.Token == token )
                    return metric;
            }

            foreach( Metric metric in CalibrationMetrics ) {
                if( metric.Token == token )
                    return metric;
            }

            return null;
        }


        public static Value GetValue( string name ) {
            for( int ii = 0; ii < publicValues.Count; ++ii ) {
                string metricName = publicValues[ ii ].ToString();
                if( metricName == "MrktSimDb.Metrics." + name ) {
                    return MetricValues[ ii ];
                }
            }


            for( int ii = 0; ii < hiddenValues.Count; ++ii ) {
                string metricName = hiddenValues[ ii ].ToString();
                if( metricName == "MrktSimDb.Metrics." + name ) {
                    return hiddenValues[ ii ];
                }
            }

            return null;
        }
        #endregion

        #region Private Fields

        Database theDb;
        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MaxValue;

        MetricUtil metricUtility;

        #endregion

        #region public Properties

        public delegate bool Progress( string what, double value );
        public event Progress UpdateProgress;

        //// Date we begine computing metric
        //// generally the start of the simulation
        //public DateTime StartDate {
        //    get {
        //        return startDate;
        //    }

        //    set {
        //        startDate = new DateTime( value.Year, value.Month, value.Day );
        //    }
        //}

        //// Date we begine computing metric
        //// generally the day after the end of the simulation
        //public DateTime EndDate {
        //    get {
        //        return endDate;
        //    }

        //    set {
        //        endDate = new DateTime( value.Year, value.Month, value.Day );
        //    }
        //}

        #endregion

        #region public methods

        public MetricMan(Database db) {
            theDb = db;

            metricUtility = new MetricUtil( db );
        }

        /// <summary>
        /// computes and writes metrics to database
        /// </summary>
        /// <param name="modelDb"></param>
        //		public bool Compute(int runID, 
        //			bool simSummary,  bool byProd, bool byProdChan, bool byProdChanSeg,
        //			bool calSummary, bool calByProd, bool calByChan, bool calByProdChan,
        //			DateTime start, DateTime end)
        public bool Compute(int runID, Metric[] metrics, DateTime start, DateTime end ) {

            // reset calculations if needed
            metricUtility.Run = runID;

            // check if run exists
            if( !runExists( runID ) ) {
                if( UpdateProgress != null )
                    UpdateProgress( "Simulation has been deleted", 0 );
                return false;
            }

            if( metrics.Length == 0 ) {
                if( UpdateProgress != null )
                    UpdateProgress( "Nothing to update", 0 );
                return false;
            }


            int numSteps = metrics.Length;
            double curStep = 0.0;


            // first clear out exising metrics
            if( UpdateProgress != null ) {
                if( !UpdateProgress( "deleting old metrics", curStep / numSteps ) ) {
                    return false;
                }
            }

            clearMetrics();

            // set the scenario  start date
            theDb.genericCommand.CommandText = "UPDATE simulation SET metric_start_date = '" + start.ToShortDateString() + "'";
            theDb.genericCommand.CommandText += " WHERE exists (SELECT * FROM sim_queue WHERE run_id  = " + runID;
            theDb.genericCommand.CommandText += " AND simulation.id = sim_queue.sim_id)";

            theDb.genericCommand.Connection.Open();

            try {
                theDb.genericCommand.ExecuteNonQuery();
            }
            catch( System.Data.OleDb.OleDbException ) {
                theDb.genericCommand.Connection.Close();

                if( UpdateProgress != null ) {
                    UpdateProgress( "DB Error", 0 );
                }

                return false;
            }

            theDb.genericCommand.Connection.Close();

            // set simulation metric end date
            if( UpdateProgress != null ) {
                if( !UpdateProgress( "setting start & end dates", curStep / numSteps ) )
                    return false;
            }

            theDb.genericCommand.CommandText = "UPDATE simulation SET metric_end_date = '" + end.ToShortDateString() + "'";
            theDb.genericCommand.CommandText += " WHERE exists (SELECT * FROM sim_queue WHERE run_id  = " + runID;
            theDb.genericCommand.CommandText += " AND simulation.id = sim_queue.sim_id)";

            theDb.genericCommand.Connection.Open();

            try {
                theDb.genericCommand.ExecuteNonQuery();
            }
            catch( System.Data.OleDb.OleDbException ) {
                theDb.genericCommand.Connection.Close();

                if( UpdateProgress != null )
                    UpdateProgress( "DB Error", 0 );


                return false;
            }

            theDb.genericCommand.Connection.Close();

            foreach( Metric metric in metrics ) {
                curStep += 1.0;

                if( UpdateProgress != null ) {
                    if( !UpdateProgress( metric.ToString(), curStep / (numSteps + 1.0) ) )
                        return false;
                }

                if( !metric.CreateMetric(metricUtility ) ) {
                    if( UpdateProgress != null )
                        UpdateProgress( "DB Error", 0 );
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// computes and writes metrics to database
        /// </summary>
        /// <param name="modelDb"></param>
        public bool Compute(int runID ) {
            // check if run exists
            if( !runExists( runID ) ) {
                UpdateProgress( "Simulation has been deleted", 0 );
                return false;
            }

            MrktSimDBSchema.sim_queueRow sim = theDb.Data.sim_queue.FindByrun_id( runID );
            MrktSimDBSchema.simulationRow simulation = sim.simulationRow;

            MrktSimDBSchema.scenario_metricRow[] metricRows = simulation.Getscenario_metricRows();

            Metric[] metrics = new Metric[ metricRows.Length ];

            int metricIndex = 0;
            foreach( MrktSimDBSchema.scenario_metricRow metricRow in metricRows ) {
                metrics[ metricIndex ] = GetMetric( metricRow.token );
                ++metricIndex;
            }

            return Compute(runID, metrics, simulation.metric_start_date, simulation.metric_end_date );
        }


        public double Evaluate( Value val ) {
            return val.Evaluate( metricUtility );
        }

        #endregion

        #region private methods

        private bool runExists( int runID ) {
            theDb.genericCommand.CommandText = "SELECT run_id FROM sim_queue WHERE run_id = " + runID;
            theDb.genericCommand.Connection.Open();

            object obj = theDb.genericCommand.ExecuteScalar();

            theDb.genericCommand.Connection.Close();

            if( obj != null && obj.GetType() != typeof( System.DBNull ) )
                return true;

            return false;
        }

        private void clearMetrics() {
            // delete the results product & channel & segment

            foreach( Metric metric in SimSummaryMetrics ) {
                metric.ClearMetric(metricUtility);
            }

            foreach( Metric metric in CalibrationMetrics ) {
                metric.ClearMetric(metricUtility);
            }
        }

        #endregion
    }
}
