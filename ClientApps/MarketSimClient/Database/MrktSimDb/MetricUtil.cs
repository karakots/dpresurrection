using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MrktSimDb;
using MrktSimDb.Metrics;

namespace MrktSimDb
{
    public class MetricUtil
    {
        public class ProdCalibration
        {
            public double MAPE;
            public double simShare;
            public double realShare;
            public double shareDiff;
            public double percentError;

            public ProdCalibration() {
                MAPE = 0.0;
                simShare = 0.0;
                realShare = 0.0;
                shareDiff = 0.0;
                percentError = 0.0;
            }
        }

        public class PricePointValue
        {
            public double Val = 0;
            public double R2 = 0;
        }

        private class pricePointData
        {
            public double velocity;
            public double price;
        }

       

        #region public methods
        public MetricUtil(Database db) {

            genericCommand = Database.newOleDbCommand();
            genericCommand.Connection = new OleDbConnection( db.Connection.ConnectionString );

            theDb = db;

            // initialize
            current_run_id = Database.AllID;

            resetTempStorage();
        }

        public OleDbCommand OleDbCommand {
            get {
                return genericCommand;
            }
        }

        public int Run {
            set {
                if( current_run_id != value ) {

                    current_run_id = value;

                    resetTempStorage();
                }
            }

            get {
                return current_run_id;
            }
        }

        public double TotalMape {
            get {
                return totalMape;
            }
        }

        public SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> RealUnits
        {
            get {

                if( real_units == null ) {
                    computeRealUnits();
                }

                return leaf_units;
            }
        }

        public SortedDictionary<int, SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>> SimUnits {
            get {

                if( sim_units == null ) {
                    computeSimUnits();
                }

                return sim_units;
            }
        }

        public List<List<double>> RealLeafShare {
            get {
                if( real_share == null ) {
                    computeRealLeafShare();
                }

                return real_share;
            }
        }

        public List<List<List<double>>> SimLeafShare {
            get {

                if( sim_share == null ) {
                    computeSimLeafShare();
                }

                return sim_share;
            }
        }

        /// <summary>
        /// This is the mape of the product
        /// differences at the channel level are averaged out
        /// other summary items added to structure for completeness
        /// </summary>
        /// <returns>prodCalSummary
        /// Checks if prodCalSummary is null and if not it recomputes
        /// </returns>
        public SortedDictionary<int, MetricUtil.ProdCalibration> ComputeProductMape() {
            if( prodCalSummary != null ) {
                return prodCalSummary;
            }

            // dependencies
            if( real_units == null ) {
                computeRealUnits();
            }

            if( sim_units == null ) {
                computeSimUnits();
            }

            totalMape = 0.0;
           

            SortedDictionary<int, double> totalRealVolume = new SortedDictionary<int, double>();
            SortedDictionary<int, double> totalSimVolume = new SortedDictionary<int, double>();

            SortedDictionary<DateTime, SortedDictionary<int, double>>  simSalesByDate = new SortedDictionary<DateTime, SortedDictionary<int, double>>();
            SortedDictionary<DateTime, SortedDictionary<int, double>>  realSalesByDate = new SortedDictionary<DateTime, SortedDictionary<int, double>>();
            SortedDictionary<int, double> prodTotal = new SortedDictionary<int, double>();

            //add up real units
            foreach( int channel in real_units.Keys ) {
                foreach( DateTime date in real_units[ channel ].Keys ) {

                    if( !realSalesByDate.ContainsKey( date ) ) {
                        realSalesByDate[ date ] = new SortedDictionary<int, double>();
                    }

                    foreach( int product in real_units[ channel ][ date ].Keys ) {

                        if( !totalRealVolume.ContainsKey( prod_type[ product ] ) ) {
                            totalRealVolume[ prod_type[ product ] ] = 0;
                        }

                        if( !realSalesByDate[ date ].ContainsKey( product ) ) {
                            realSalesByDate[ date ][ product ] = 0.0;
                        }


                        if( !prodTotal.ContainsKey( product ) ) {
                            prodTotal[ product ] = 0.0;
                        }


                        double val = real_units[ channel ][ date ][ product ];
                        realSalesByDate[ date ][ product ] += val;
                        totalRealVolume[prod_type[product]] += val;
                        prodTotal[ product ] += val;
                    }
                }
            }

            // sim units
            foreach( int seg in sim_units.Keys ) {
                foreach( int channel in sim_units[ seg ].Keys ) {

                    foreach( DateTime date in sim_units[ seg ][ channel ].Keys ) {

                        if( !simSalesByDate.ContainsKey( date ) ) {
                            simSalesByDate[ date ] = new SortedDictionary<int, double>();
                        }

                        foreach( int product in sim_units[ seg ][ channel ][ date ].Keys ) {


                            if( !totalSimVolume.ContainsKey( prod_type[ product ] ) ) {
                                totalSimVolume[ prod_type[ product ] ] = 0;
                            }

                            if( !simSalesByDate[ date ].ContainsKey( product ) ) {
                                simSalesByDate[ date ][ product ] = 0.0;
                            }

                            double val = sim_units[ seg ][ channel ][ date ][ product ];
                            simSalesByDate[ date ][ product ] += val;
                            totalSimVolume[ prod_type[ product ] ] += val;
                        }
                    }
                }
            }

            // this is what we return
            prodCalSummary = new SortedDictionary<int, MetricUtil.ProdCalibration>();

            foreach( DateTime date in realSalesByDate.Keys ) {
                foreach( int product in realSalesByDate[ date ].Keys ) {

                    if( !prodCalSummary.ContainsKey( product ) ) {
                        prodCalSummary[ product ] = new ProdCalibration();
                    }

                    double realSales = realSalesByDate[ date ][ product ];
                    double simSales = simSalesByDate[ date ][ product ];
                    double absDiff = 100 * Math.Abs( simSales - realSales );

                    if( totalRealVolume[prod_type[product]] > 0.0 ) {

                        if( prodTotal[ product ] > 0 ) {
                            prodCalSummary[ product ].MAPE += absDiff / prodTotal[ product ];
                        }

                        prodCalSummary[ product ].realShare += 100 * realSales / totalRealVolume[ prod_type[ product ] ];
                        prodCalSummary[ product ].simShare += 100 * simSales / totalSimVolume[ prod_type[ product ] ];

                    }

                    if(isLeaf[product]&&  totalRealVolume[ prod_type[product ]] > 0 ) {
                        totalMape += absDiff / totalRealVolume[prod_type[product]];
                    }
                }
            }

            foreach( MetricUtil.ProdCalibration cal in prodCalSummary.Values ) {
                cal.shareDiff = cal.simShare - cal.realShare;

                if( cal.realShare > 0 ) {
                    cal.percentError = cal.shareDiff / cal.realShare;
                }
            }

            return prodCalSummary;
        }

        public SortedDictionary<int, PricePointValue> PricePoint {
            get {
                if( price_point == null ) {
                    computePricePoint();
                }

                return price_point;
            }
        }

        public SortedDictionary<int, PricePointValue> RealPricePoint {
            get {
                if( real_price_point == null ) {
                    computeRealPricePoint();
                }

                return real_price_point;
            }
        }

        #endregion

        // the database
        Database theDb;

        // just a generic command for use
        private OleDbCommand genericCommand;

        #region Temporay storage changes when run_id changes

        private int current_run_id;

        private void resetTempStorage() {
            // null out all temporary storage
            real_units = null;
            sim_units = null;
            real_share = null;
            sim_share = null;

            prodCalSummary = null;

            totalMape = 0.0;

            price_point = null;

            real_price_point = null;
        }

        // for price elasticity
        private SortedDictionary<int, PricePointValue> price_point;
        private SortedDictionary<int, PricePointValue> real_price_point;


      
        // this for calibration purposes
        double totalMape;

        // real_units is structuted as (channel, date, product) -> #num
        private SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> real_units;

        // like real units - but only the leafs
        private SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> leaf_units;
        private SortedDictionary<int, SortedDictionary<DateTime, double>> real_totals;

        public SortedDictionary<int, SortedDictionary<DateTime, double>> RealTotals
        {
            get
            {
                return real_totals;
            }
        }
        
        // these depend on real_units
        // sim_units is structured as (segment, channel, date, product) -> #num
        // sim units are dependent on the real units
        private SortedDictionary<int, SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>> sim_units;
        private SortedDictionary<int, SortedDictionary<DateTime, double>> sim_totals;
        public SortedDictionary<int, SortedDictionary<DateTime, double>> SimTotals
        {
            get
            {
                return sim_totals;
            }
        }

        // map from prod_id to type of product
        private SortedDictionary<int, int> prod_type;
        private SortedDictionary<int, bool> isLeaf;

        /// <summary>
        /// The real leafs
        /// </summary>
        public List<int> RealLeafs
        {
            get
            {
                List<int> leafs = new List<int>();

                foreach( int prod in isLeaf.Keys )
                {
                    if( isLeaf[prod] )
                    {
                        leafs.Add( prod );
                    }
                }

                return leafs;
            }
        }

        /// <summary>
        /// assumes all locals hav ebeen initialized
        /// </summary>
        /// <param name="prod_id"></param>
        /// <returns></returns>
        public bool IsRealLeaf( int prod_id )
        {
            if (isLeaf.ContainsKey(prod_id))
            {
                return isLeaf[prod_id];
            }

            return false;
        }

        //// sim sales and real sales by date and product
        //SortedDictionary<DateTime, SortedDictionary<int, double>> simSalesByDate;
        //SortedDictionary<DateTime, SortedDictionary<int, double>> realSalesByDate;
        //SortedDictionary<int, double> prodTotal;

        SortedDictionary<int, MetricUtil.ProdCalibration> prodCalSummary;

        // (channel-date, product)
        private List<List<double>> real_share = null;

        // (segment, channel-date, product)
        private List<List<List<double>>> sim_share = null;
       

        #endregion

        private Dictionary<int, double> Units_By_Day( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict, DateTime day ) {
            Dictionary<int, double> output = new Dictionary<int, double>();

            DateTime previous_date = DateTime.MinValue;
            DateTime next_date = DateTime.MinValue;
            TimeSpan span = next_date - previous_date;

            // set next_date to first date in date_dict at or after day
            // previous_date should be the date in date_dict just preceeding
            foreach( DateTime date in date_dict.Keys ) {
                next_date = date;

                if( date >= day ) {
                    break;
                }
                previous_date = date;
            }

            // we are at the beginning so we need to comput the span for the first two
            if( previous_date == DateTime.MinValue ) {
                SortedDictionary<DateTime, SortedDictionary<int, double>>.Enumerator iter = date_dict.GetEnumerator();

                iter.MoveNext();
                DateTime first = iter.Current.Key;
                iter.MoveNext();
                DateTime second = iter.Current.Key;
                span = second - first;
            }
            else {
                span = next_date - previous_date;
            }


            foreach( KeyValuePair<int, double> pair in date_dict[ next_date ] ) {
                if( span.Days > 0 ) {
                    output.Add( pair.Key, pair.Value / (double)span.Days );
                }
                else {
                    output.Add( pair.Key, 0 );
                }
            }

            return output;
        }

        // aggregates and collates the real sales data to the simulation sales data
        // No assumptions are made concerning the spacing of the real sales data
        // simulation sales are assumed to be aggregated over the simulation access_time
        // TBD: If the user changes the simulation access time then simulations based on that will not work as advertised.
        private void computeRealUnits() {

            if( current_run_id == Database.AllID )
            {
                return;
            }

            // the data reader we will be using throughout
            System.Data.OleDb.OleDbDataReader dataReader;

            //First build dictionaries by selecting all distinct values
            // first up is the channel -> date -> product dictionary

            // unaligned data is raw data from the database
            // real_units are saved for use by other utilities
            // and is the collated/aggreated version of the raw data
            SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> unaligned_data = new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>();
            real_units = new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>();

            // the model we are reading from - we grab the first model
            int model_id = (int)theDb.Data.Model_info.Select()[ 0 ][ "model_id" ];

            //Get the channels - note how the real sales forms the template
            // Create the channel part of the dictionaries
            genericCommand.CommandText = "select distinct channel_id from external_data where model_id = " + model_id;

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] IdData = new object[ 1 ];

            while( dataReader.Read() ) {
                dataReader.GetValues( IdData );
                unaligned_data.Add( (int)IdData[ 0 ], new SortedDictionary<DateTime, SortedDictionary<int, double>>() );
                real_units.Add( (int)IdData[ 0 ], new SortedDictionary<DateTime, SortedDictionary<int, double>>() );
            }

            dataReader.Close();

            //Next get the dates

            //This is the real_dates - this is where the unalligned and real sales differ
            genericCommand.CommandText = "select distinct calendar_date from external_data where model_id = " + model_id;

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            while( dataReader.Read() ) {
                dataReader.GetValues( IdData );
                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in unaligned_data.Values ) {
                    date_dict.Add( MetricMan.Convert2DateTime( IdData[ 0 ] ), new SortedDictionary<int, double>() );
                }
            }

            dataReader.Close();

            //This is the sim_dates - we fill in two structures here
            // the real_units and a list of the simulaiton dates
            // note that we only want dates after metric start and at or before the metric end
            DateTime startDate = DateTime.MaxValue;
            DateTime endDate = DateTime.MinValue;

            List<DateTime> sim_calendar_dates = new List<DateTime>();

            genericCommand.CommandText = "SELECT DISTINCT calendar_date FROM results_std, sim_queue, simulation " +
                " WHERE sim_queue.run_id = results_std.run_id" +
                " AND sim_queue.sim_id = simulation.id" +
                " AND results_std.run_id = " + current_run_id + 
                " AND calendar_date > simulation.metric_start_date" +
                " AND calendar_date < simulation.metric_end_date + 1 " +
                " ORDER BY calendar_date";

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            while( dataReader.Read() ) {
                dataReader.GetValues( IdData );

                endDate = MetricMan.Convert2DateTime( IdData[ 0 ] );

                if( endDate < startDate ) {
                    startDate = endDate;
                }

                sim_calendar_dates.Add( endDate );

                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in real_units.Values ) {
                    date_dict.Add( endDate, new SortedDictionary<int, double>() );
                }
            }
            dataReader.Close();

            //Finally get the products
            // this is the same for both real and raw units
            // and is derived from the real sales
            // we do not want to reference the simulaiton sales as there may be products introduced
            // that are not a part of the simulation sales

            // this is an auxilary structure so we can tell what KIND of product the product is
            // it is used in subsequent methods to aid in aggregation
            prod_type = new SortedDictionary<int, int>();
            isLeaf = new SortedDictionary<int, bool>();

            // note the use of of the Common Table structure unique to SQL Server
            // this allows 
            genericCommand.CommandText = ";WITH Dec(ans_id, dec_id) AS " +
              " ( " +
              " 	SELECT Me.product_id, Myself.product_id  FROM product Me, product Myself WHERE Me.product_id =  Myself.product_id " +
              " 	UNION ALL  	SELECT product_tree.parent_id, product_tree.child_id  FROM product_tree, product child " +
              "     WHERE  child.product_id = product_tree.child_id AND child.brand_id = 0 " +
              " 	UNION ALL  	Select D.ans_id, leaf.product_id" +
              " 	FROM product leaf, product_tree T, Dec D " +
              " 	WHERE leaf.product_id =  T.child_id " +
              "     AND leaf.brand_id = 1	" +
              " 	AND T.parent_id = D.dec_id " +
              " ) ";

            genericCommand.CommandText += " select distinct ans_id, product_type, brand_id FROM external_data, Dec, product WITH (NOLOCK) " +
                " WHERE ans_id = product.product_id AND external_data.product_id = Dec.dec_id AND external_data.model_id = " + model_id;

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] prodData = new object[ 4 ];
            while( dataReader.Read() ) {
                dataReader.GetValues( prodData );

                bool aLeaf = false;

                if( (int)prodData[ 2 ] == 1 ) 
                {
                    aLeaf = true;
                }

                prod_type.Add( (int)prodData[ 0 ], (int)prodData[ 1 ] );
                isLeaf.Add( (int)prodData[0], aLeaf );


                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in unaligned_data.Values ) {
                    foreach( SortedDictionary<int, double> prod_dict in date_dict.Values ) {
                        prod_dict.Add( (int)prodData[ 0 ], 0 );
                    }
                }

                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in real_units.Values ) {
                    foreach( SortedDictionary<int, double> prod_dict in date_dict.Values ) {
                        prod_dict.Add( (int)prodData[ 0 ], 0 );
                    }
                }
            }

            dataReader.Close();

            //Now that the dictionary is created fill it with values
            genericCommand.CommandText = ";WITH Dec(ans_id, dec_id) AS " +
              " ( " +
              " 	SELECT Me.product_id, Myself.product_id  from product Me, product Myself WHERE Me.product_id =  Myself.product_id " +
              " 	UNION ALL  	SELECT product_tree.parent_id, product_tree.child_id  FROM product_tree, product child " +
              "     WHERE  child.product_id = product_tree.child_id AND child.brand_id = 0 " +
              " 	UNION ALL  	Select D.ans_id, leaf.product_id" +
              " 	FROM product leaf, product_tree T, Dec D " +
              " 	WHERE leaf.product_id =  T.child_id " +
              "     AND leaf.brand_id = 1	" +
              " 	AND T.parent_id = D.dec_id " +
              " ) ";

            // NOTE assumption that external data is just leaf data
            genericCommand.CommandText += " select channel_id, calendar_date,  Dec.ans_id as product_id, SUM(value) from external_data, DEC WITH (NOLOCK) " +
        " where external_data.product_id = Dec.dec_id AND external_data.type = 1 AND model_id = " + model_id +
        " GROUP BY channel_id, calendar_date, ans_id" +
        " ORDER BY channel_id, calendar_date, ans_id";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] shareData = new object[ 4 ];

            while( dataReader.Read() ) {
                dataReader.GetValues( shareData );

                //If this crashes then there are problems...
                unaligned_data[ (int)shareData[ 0 ] ][ MetricMan.Convert2DateTime( shareData[ 1 ] ) ][ (int)shareData[ 2 ] ] = MetricMan.Convert2Double( shareData[ 3 ] );
            }

            dataReader.Close();

            ////We will need to align the data to the sim data
            //object[] dateData = new object[ 1 ];
            //genericCommand.CommandText = "SELECT DISTINCT calendar_date FROM results_std, sim_queue, simulation " +
            //    " WHERE sim_queue.run_id = results_std.run_id" +
            //    " AND sim_queue.sim_id = simulation.id" +
            //    " AND results_std.run_id = " + current_run_id +
            //    " AND calendar_date > simulation.metric_start_date - 1 " +
            //    " AND calendar_date < simulation.metric_end_date + 1 " +
            //    " ORDER BY calendar_date";

            //genericCommand.Connection.Open();
            //dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            //List<DateTime> sim_calendar_dates = new List<DateTime>();

            //while( dataReader.Read() ) {
            //    dataReader.GetValues( dateData );
            //    sim_calendar_dates.Add( MetricMan.Convert2DateTime( dateData[ 0 ] ) );
            //}

            //dataReader.Close();

            //Now need to compute the aligned units
            //Get the sim_start_date
            int sim_id = (int)theDb.Data.sim_queue.Select( "run_id = " + current_run_id, "", DataViewRowState.CurrentRows )[ 0 ][ "sim_id" ];
            int accessTime = (int) theDb.Data.simulation.Select( "id = " + sim_id, "", DataViewRowState.CurrentRows )[ 0 ][ "access_time" ];

            TimeSpan span = new TimeSpan( accessTime - 1, 0, 0, 0 );

            // collation/aggregation !!!!
            DateTime currentDate = new DateTime();

            Dictionary<int, double> sales;

            foreach( KeyValuePair<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_date_pair in real_units ) {
                currentDate = startDate - span;
                foreach( KeyValuePair<DateTime, SortedDictionary<int, double>> date_prod_pair in chan_date_pair.Value ) {

                    // do not go past end data
                    // SSN 12/26/2007
                    if (date_prod_pair.Key > endDate) {
                        break;
                    }

                    while( currentDate <= date_prod_pair.Key ) {
                        sales = Units_By_Day( unaligned_data[ chan_date_pair.Key ], currentDate );
                        int[] keys = new int[ date_prod_pair.Value.Keys.Count ];
                        date_prod_pair.Value.Keys.CopyTo( keys, 0 );
                        foreach( int prod in keys ) {
                            date_prod_pair.Value[ prod ] = date_prod_pair.Value[ prod ] + sales[ prod ];
                        }
                        currentDate = currentDate.AddDays( 1 );
                    }
                }
            }


            // construct leaf units
            leaf_units = new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>();

            foreach( int channel in real_units.Keys ) {

                leaf_units.Add( channel, new SortedDictionary<DateTime, SortedDictionary<int, double>>() );

                foreach( DateTime date in real_units[ channel ].Keys ) {
                    leaf_units[ channel ].Add( date, new SortedDictionary<int, double>() );

                    foreach( int product in real_units[ channel ][ date ].Keys ) {

                        if( isLeaf[ product ] ) {
                            leaf_units[ channel ][ date ][ product ] = real_units[ channel ][ date ][ product ];
                        }
                    }
                }
            }


            // compute totals
            // now do same for real data
            // how many dates
            this.real_totals = new SortedDictionary<int, SortedDictionary<DateTime, double>>();

            foreach(int channelID in leaf_units.Keys )
            {
                 SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales = leaf_units[channelID];
                SortedDictionary<DateTime, double> timeTotals = new SortedDictionary<DateTime, double>();

                real_totals.Add( channelID, timeTotals );
           
                foreach( DateTime date in dateProductSales.Keys )
                {
                    SortedDictionary<int, double> prodSales = dateProductSales[date];

                    double total = 0;
                    foreach( double curSale in prodSales.Values )
                    {
                        total += curSale;
                    }

                    if(!timeTotals.ContainsKey(date) )
                    {
                        timeTotals.Add(date, total);
                    }
                    else
                    {
                        timeTotals[date] += total;
                    }
                }
            }
        }

        private void computeSimUnits()
        {

            // note dependency
            if( real_units == null )
            {
                computeRealUnits();
            }

            int seg_id = 0;
            int chan_id = 0;

            int prod_id = 0;
            double units = 0;

            //First build sim_units dictionary from real units
            //This is to ensure that products not in the external are not included
            sim_units = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>>();

            //First need to select segments
            genericCommand.CommandText = "select distinct segment_id from results_std where run_id = " + current_run_id;
            object[] segs = new object[1];
            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            while( dataReader.Read() )
            {
                dataReader.GetValues( segs );
                sim_units.Add( (int)segs[0], new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>() );
            }

            dataReader.Close();

            //This will construct the sim_units dictionary
            foreach( SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_dict in sim_units.Values )
            {
                foreach( KeyValuePair<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_date_pair in real_units )
                {
                    chan_dict.Add( chan_date_pair.Key, new SortedDictionary<DateTime, SortedDictionary<int, double>>() );
                    foreach( KeyValuePair<DateTime, SortedDictionary<int, double>> date_prod_pair in chan_date_pair.Value )
                    {
                        chan_dict[chan_date_pair.Key].Add( date_prod_pair.Key, new SortedDictionary<int, double>() );
                        foreach( KeyValuePair<int, double> prod_units_pair in date_prod_pair.Value )
                        {
                            chan_dict[chan_date_pair.Key][date_prod_pair.Key].Add( prod_units_pair.Key, 0 );
                        }
                    }
                }
            }

            //Grab the units data from the database to fill the sim_units dictionary
            genericCommand.CommandText = "select results_std.segment_id, results_std.channel_id, results_std.calendar_date, results_std.product_id, " +
                " results_std.num_sku_bought as units " +
                " FROM results_std " +
                " WHERE results_std.run_id = " + current_run_id;



            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] shareData = new object[5];
            seg_id = 0;
            chan_id = 0;
            // date = new DateTime();
            prod_id = 0;
            units = 0;

            //Grab data from database and store in a multimap
            while( dataReader.Read() )
            {
                dataReader.GetValues( shareData );

                seg_id = (int)shareData[0];
                chan_id = (int)shareData[1];
                DateTime date = MetricMan.Convert2DateTime( shareData[2] );
                prod_id = (int)shareData[3];
                units = MetricMan.Convert2Double( shareData[4] );

                //If the correct data is not found then it is not in the external data and should not be included
                if( !sim_units.ContainsKey( seg_id ) )
                {
                    continue;
                }
                if( !sim_units[seg_id].ContainsKey( chan_id ) )
                {
                    continue;
                }
                if( !sim_units[seg_id][chan_id].ContainsKey( MetricMan.Convert2DateTime( date ) ) )
                {
                    continue;
                }
                if( !sim_units[seg_id][chan_id][date].ContainsKey( prod_id ) )
                {
                    continue;
                }
                sim_units[seg_id][chan_id][date][prod_id] = units;
            }

            dataReader.Close();

            sim_totals = new SortedDictionary<int, SortedDictionary<DateTime, double>>();
            // computes totals
            foreach( SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> channelDateProductSales in sim_units.Values )
            {

                foreach( int channelID in channelDateProductSales.Keys )
                {
                    SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales = channelDateProductSales[channelID];

                    SortedDictionary<DateTime, double> timeTotals = null;
                    if( this.sim_totals.ContainsKey( channelID ) )
                    {
                        timeTotals = sim_totals[channelID];
                    }
                    else
                    {
                        timeTotals = new SortedDictionary<DateTime, double>();
                        sim_totals.Add( channelID, timeTotals );
                    }

                    foreach( DateTime thisDate in dateProductSales.Keys )
                    {
                        SortedDictionary<int, double> prodSales = dateProductSales[thisDate];

                        double total = 0;
                        foreach( int prodId in prodSales.Keys )
                        {
                            double sales = prodSales[prodId];

                            if( RealUnits[channelID][thisDate].ContainsKey( prodId ) )
                            {
                                total += sales;
                            }
                        }

                        if( !timeTotals.ContainsKey( thisDate ) )
                        {
                            timeTotals.Add( thisDate, total );
                        }
                        else
                        {
                            timeTotals[thisDate] += total;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// returns the vector of real shares per product for each channel-date
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="shares">
        /// outer list is over channel-dates
        /// inner list is over products</param>
        private void computeRealLeafShare() {

            if( real_units == null )
            {
                computeRealUnits();
            }

            real_share = new List<List<double>>();
            int chan_time_index = 0;
            int prod_index = 0;
            double total = 0;
            foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in leaf_units.Values )
            {
                foreach( SortedDictionary<int, double> prod_dict in date_dict.Values ) {
                    total = 0.0;
                    real_share.Add( new List<double>() );
                    foreach( double units in prod_dict.Values ) {
                        total += units;
                        real_share[ chan_time_index ].Add( 0 );
                    }

                    if( total > 0 ) {
                        prod_index = 0;
                        foreach( double units in prod_dict.Values ) {
                            real_share[ chan_time_index ][ prod_index ] = units / total;
                            prod_index++;
                        }
                    }
                    chan_time_index++;
                }
            }
        }


        /// <summary>
        /// Returns list for each segment
        /// the vector of shares for each channel-date
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="shareArr">
        /// outer list is over segments
        /// middle list is over channel-dates
        /// inner list is over products</param>
        private void computeSimLeafShare() {
            if( real_units == null ) {
                computeRealUnits();
            }

            if(sim_units == null ) {
                computeSimUnits();
            }

            //Units sales has been stored in sim_units.
            //Simulation share is computed based on sim_units


            sim_share = new List<List<List<double>>>();

            //Unwind multimap and store in sim_share array
            int seg_id = 0;
            int chan_time = 0;
            double total = 0.0;

            //Loop over segments
            foreach( SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> seg_dict in sim_units.Values ) {
                sim_share.Add( new List<List<double>>() );
                //Loop over channels
                chan_time = 0;
                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> chan_dict in seg_dict.Values ) {
                    foreach( SortedDictionary<int, double> time_dict in chan_dict.Values ) {
                        total = 0.0;
                        sim_share[ seg_id ].Add( new List<double>() );
                        //Compute total units
                        
                        foreach( int prod_id in time_dict.Keys ) {

                            double sales = time_dict[prod_id];

                            // check if product is a leaf
                            if( isLeaf[prod_id] )
                            {
                                total += sales;
                                sim_share[seg_id][chan_time].Add( 0 );
                            }
                        }
                        //Divide by total units to get share
                        if( total > 0 ) {
                            int prod_index = 0;
                            foreach( int prod_id in time_dict.Keys )
                            {
                                  // check if product is a leaf
                                if( isLeaf[prod_id] )
                                {
                                    double sales = time_dict[prod_id];
                                    sim_share[seg_id][chan_time][prod_index] = sales / total;
                                    prod_index++;
                                }
                            }
                        }
                        chan_time++;
                    }
                }
                seg_id++;
            }
        }



        /// <summary>
        /// returns the vector of real shares per product for each date
        /// </summary>
        public List<List<double>> computeChannelTotalRealLeafShare()
        {
            if( real_units == null )
            {
                computeRealUnits();
            }

            //int chan_time_index = 0;
            //int prod_index = 0;
            //double total = 0;

            List<double> totals = new List<double>();

            // first compute totals over channels
            // result is total at time tt

            foreach( int chan_id in real_totals.Keys )
            {
                int tt = 0;
                foreach( double val in real_totals[chan_id].Values )
                {
                    if( tt == totals.Count )
                    {
                        totals.Add( 0 );
                    }

                    totals[tt] += val;
                    ++tt;
                }
            }

            List<List<double>> date_prod_share = new List<List<double>>();

            foreach( int chan_id in leaf_units.Keys )
            {
                SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict = leaf_units[chan_id];

                int tt = 0;
                foreach( SortedDictionary<int, double> prod_dict in date_dict.Values )
                {
                    if( tt == date_prod_share.Count )
                    {
                        date_prod_share.Add( new List<double>() );
                    }

                    List<double> prod_share = date_prod_share[tt];

                    int prodDex = 0;
                    foreach( double units in prod_dict.Values )
                    {
                        if( prodDex == prod_share.Count )
                        {
                            prod_share.Add( 0 );
                        }

                        if( totals[tt] > 0 )
                        {
                            prod_share[prodDex] += units / totals[tt];
                        }

                        ++prodDex;
                    }

                    ++tt;
                }
            }

            return date_prod_share;
        }

        /// <summary>
        /// Returns sim share for each product per time 
        /// </summary>
        public List<List<double>> computeChannelTotalSimLeafShare()
        {
            if( real_units == null )
            {
                computeRealUnits();
            }

            if( sim_units == null )
            {
                computeSimUnits();
            }

            List<double> totals = new List<double>();

            // first compute totals over channels
            // result is total at time tt

            foreach( int chan_id in sim_totals.Keys )
            {
                int tt = 0;
                foreach( double val in sim_totals[chan_id].Values )
                {
                    if( tt == totals.Count )
                    {
                        totals.Add( 0 );
                    }

                    totals[tt] += val;
                    ++tt;
                }
            }

            List<List<double>> date_prod_share = new List<List<double>>();

            foreach( int seg_id in sim_units.Keys )
            {

                foreach( int chan_id in sim_units[seg_id].Keys )
                {
                    SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict = sim_units[seg_id][chan_id];

                    int tt = 0;
                    foreach( SortedDictionary<int, double> prod_dict in date_dict.Values )
                    {
                        if( tt == date_prod_share.Count )
                        {
                            date_prod_share.Add( new List<double>() );
                        }

                        List<double> prod_share = date_prod_share[tt];

                        int prodDex = 0;
                        foreach(int prodID in prod_dict.Keys )
                        {
                            if( IsRealLeaf(prodID) )
                            {
                                double units = prod_dict[prodID];

                                if( prodDex == prod_share.Count )
                                {
                                    prod_share.Add( 0 );
                                }

                                if( totals[tt] > 0 )
                                {
                                    prod_share[prodDex] += units / totals[tt];
                                }

                                ++prodDex;
                            }
                        }

                        ++tt;
                    }
                }
            }


            return date_prod_share;
        }

        /// <summary>
        /// Returns sim share for each product per time 
        /// </summary>
        public List<double> computeChannelTotalWeights()
        {
            if( real_units == null )
            {
                computeRealUnits();
            }

            if( sim_units == null )
            {
                computeSimUnits();
            }

            double total = 0;

            List<double> totals = new List<double>();

            // first compute totals over channels
            // result is total at time tt

            foreach( int chan_id in real_totals.Keys )
            {
                int tt = 0;
                foreach( double val in real_totals[chan_id].Values )
                {
                    if( tt == totals.Count )
                    {
                        totals.Add( 0 );
                    }

                    totals[tt] += val;
                    total += val;
                    ++tt;
                }
            }

            if( total > 0 )
            {
                for( int ii = 0; ii < totals.Count; ++ii )
                {
                    totals[ii] /= total;
                }
            }

            return totals;
        }

        private void computePricePoint() {

            price_point = new SortedDictionary<int, PricePointValue>();
            SortedDictionary<int, SortedDictionary<DateTime, pricePointData>> price_point_by_date = new SortedDictionary<int, SortedDictionary<DateTime, pricePointData>>();

            // set up products and dates
            genericCommand.CommandText = "SELECT DISTINCT results_std.product_id " +
             " FROM results_std, product " +
             " WHERE results_std.product_id = product.product_id   " +
             " AND product.brand_id = 1 " +
             " and results_std.run_id = " + current_run_id;

       
            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] prodData = new object[ 1 ];
            int prod_id = 0;

            //Grab data from database and store in dictionary
            while( dataReader.Read() ) {
                dataReader.GetValues( prodData );
                prod_id = (int)prodData[ 0 ];

                price_point_by_date[ prod_id ] = new SortedDictionary<DateTime, pricePointData>();
                price_point[ prod_id ] = new PricePointValue();
            }

            dataReader.Close();

            if( price_point_by_date.Count == 0 ) {
                return;
            }

            // units
            genericCommand.CommandText = "select results_std.product_id,  results_std.calendar_date, " +
                " SUM(results_std.num_sku_bought) as units, " +
                " AVG(results_std.percent_preuse_distribution_sku)/(100* AVG(simulation.access_time)), " +
                " AVG(results_std.percent_aware_sku_cum)/(100* AVG(simulation.access_time)) " +
                " FROM results_std, product, simulation , sim_queue " +
                " WHERE results_std.product_id = product.product_id   " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND product.brand_id = 1 " +
                " and results_std.run_id = " + current_run_id +
                " Group By results_std.product_id,  results_std.calendar_date";

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] velData = new object[ 5 ];
      
            double units = 0;
            double dist = 0.0;
            double aware = 0.0;
            DateTime date = new DateTime();
            

            //Grab data from database and store in a multimap
            while( dataReader.Read() ) {
                dataReader.GetValues( velData );

                prod_id = (int)velData[ 0 ];
                date = MetricMan.Convert2DateTime( velData[ 1 ] );
                units = MetricMan.Convert2Double( velData[ 2 ] );
                dist = MetricMan.Convert2Double( velData[ 3 ] );
                aware = MetricMan.Convert2Double( velData[ 4 ] );
                pricePointData ppData = new pricePointData();

                 
                if( aware * dist > 0 ) {
                    ppData.velocity = units / (aware * dist);
                }
                else {
                    ppData.velocity = 0.0;
                }

                price_point_by_date[ prod_id ].Add(date, ppData);
            }

            dataReader.Close();


            // price
            genericCommand.CommandText = "SELECT " +
               " results_std.product_id, " +
               " results_std.calendar_date, " +
               " (AVG(results_std.unpromoprice)* " +
               " (100 -  " +
               " (AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) -  " +
               " (AVG(percent_at_display_price)/AVG(simulation.access_time))) + " +
               " (AVG(results_std.promoprice)*AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) +  " +
               " (AVG(results_std.display_price) * AVG(percent_at_display_price)/AVG(simulation.access_time)))/(100* AVG(simulation.access_time)) " +
               " from results_std, product, simulation, sim_queue " +
               " where	sim_queue.run_id = " + current_run_id +
               " AND sim_queue.run_id = results_std.run_id " +
               " AND simulation.id = sim_queue.sim_id " +
               " AND results_std.product_id = product.product_id " +
               " AND product.brand_id = 1 " +
               " group by  results_std.product_id, results_std.calendar_date";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            double price = 0.0;
            object[] priceData = new object[ 3 ];
            
            while( dataReader.Read() ) {
                dataReader.GetValues( priceData );

                prod_id = (int)priceData[ 0 ];
                date = MetricMan.Convert2DateTime( priceData[ 1 ] );
                price = MetricMan.Convert2Double( priceData[ 2 ] );

                price_point_by_date[ prod_id ][ date ].price = price;
            }

            dataReader.Close();

             compute_price_point_stats(price_point_by_date, price_point);
        }

        private void computeRealPricePoint() {

            int model_id = (int)theDb.Data.Model_info.Select()[ 0 ][ "model_id" ];

            real_price_point = new SortedDictionary<int, PricePointValue>();
            SortedDictionary<int, SortedDictionary<DateTime, pricePointData>> price_point_by_date = new SortedDictionary<int, SortedDictionary<DateTime, pricePointData>>();

            // set up products and dates
            genericCommand.CommandText = "SELECT DISTINCT product_id " +
             " FROM external_data " +
             " WHERE model_id = " + model_id;

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] prodData = new object[ 1 ];
            int prod_id = 0;

            //Grab data from database and store in dictionary
            while( dataReader.Read() ) {
                dataReader.GetValues( prodData );
                prod_id = (int)prodData[ 0 ];

                price_point_by_date[ prod_id ] = new SortedDictionary<DateTime, pricePointData>();
                real_price_point[ prod_id ] = new PricePointValue();
            }

            dataReader.Close();

            if( price_point_by_date.Count == 0 ) {
                return;
            }

            // use the leaf units - they are alligned with the sim dates
            SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> real_units = RealUnits;

            foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> date_prod_units in real_units.Values ) {
                foreach( DateTime date in date_prod_units.Keys ) {
                    foreach(int prodID in date_prod_units[date].Keys) {

                        // check that price_point data contains keys

                        if( price_point_by_date.ContainsKey( prodID ) ) {

                            if( price_point_by_date[ prodID ].ContainsKey( date ) ) {
                                price_point_by_date[ prodID ][ date ].velocity += date_prod_units[ date ][ prodID ];
                            }
                            else {
                                pricePointData ppData = new pricePointData();

                                ppData.velocity = date_prod_units[ date ][ prodID ];
                                price_point_by_date[ prodID ].Add( date, ppData );
                            }
                        }
                        else {
                            throw new Exception( "Inconsistant ids in Real data" );
                        }
                    }
                }
            }

            // distribution and awareness
            genericCommand.CommandText = "select results_std.product_id,  results_std.calendar_date, " +
                " AVG(results_std.percent_preuse_distribution_sku)/(100* AVG(simulation.access_time)), " +
                " AVG(results_std.percent_aware_sku_cum)/(100* AVG(simulation.access_time)) " +
                " FROM results_std, product, simulation , sim_queue " +
                " WHERE results_std.product_id = product.product_id   " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND product.brand_id = 1 " +
                " and results_std.run_id = " + current_run_id +
                " Group By results_std.product_id,  results_std.calendar_date";

            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] velData = new object[ 4 ];

            double dist = 0.0;
            double aware = 0.0;
            DateTime dateTime = new DateTime();

            //Grab data from database and store in a multimap
            while( dataReader.Read() ) {
                dataReader.GetValues( velData );

                prod_id = (int)velData[ 0 ];
                dateTime = MetricMan.Convert2DateTime( velData[ 1 ] );
                dist = MetricMan.Convert2Double( velData[ 2 ] );
                aware = MetricMan.Convert2Double( velData[ 3 ] );

                // actually do not use awareness for real data...
                if( dist > 0 ) {
                    if( price_point_by_date.ContainsKey( prod_id ) && price_point_by_date[ prod_id ].ContainsKey( dateTime ) ) {
                        price_point_by_date[ prod_id ][ dateTime ].velocity /= dist;
                    }
                }
            }

            dataReader.Close();


            // price
            genericCommand.CommandText = "SELECT " +
               " results_std.product_id, " +
               " results_std.calendar_date, " +
               " (AVG(results_std.unpromoprice)* " +
               " (100 -  " +
               " (AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) -  " +
               " (AVG(percent_at_display_price)/AVG(simulation.access_time))) + " +
               " (AVG(results_std.promoprice)*AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) +  " +
               " (AVG(results_std.display_price) * AVG(percent_at_display_price)/AVG(simulation.access_time)))/(100* AVG(simulation.access_time)) " +
               " from results_std, product, simulation, sim_queue " +
               " where	sim_queue.run_id = " + current_run_id +
               " AND sim_queue.run_id = results_std.run_id " +
               " AND simulation.id = sim_queue.sim_id " +
               " AND results_std.product_id = product.product_id " +
               " AND product.brand_id = 1 " +
               " group by  results_std.product_id, results_std.calendar_date";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            double price = 0.0;
            object[] priceData = new object[ 3 ];

            while( dataReader.Read() ) {
                dataReader.GetValues( priceData );

                prod_id = (int)priceData[ 0 ];
                dateTime = MetricMan.Convert2DateTime( priceData[ 1 ] );
                price = MetricMan.Convert2Double( priceData[ 2 ] );

                if( price_point_by_date.ContainsKey( prod_id ) && price_point_by_date[prod_id].ContainsKey(dateTime) ) {

                    price_point_by_date[ prod_id ][ dateTime ].price = price;
                }
            }

            dataReader.Close();

            compute_price_point_stats(price_point_by_date, real_price_point);
        }

        private void compute_price_point_stats( SortedDictionary<int, SortedDictionary<DateTime, pricePointData>> price_point_by_date,
            SortedDictionary<int, PricePointValue> pp_out) {



            // trim last date    
            foreach( SortedDictionary<DateTime, pricePointData> dateValues in price_point_by_date.Values ) {
                DateTime lastDate = DateTime.MinValue;

                foreach( DateTime key in dateValues.Keys ) {
                    lastDate = key;
                }


                dateValues.Remove( lastDate );

            }


            
            List<double> priceList = new List<double>();
            List<double> velList = new List<double>();

            foreach( int prod in price_point_by_date.Keys ) {

                priceList.Clear();
                velList.Clear();

                double avePrice = 0.0;
                double aveVel = 0.0;
                double medPrice = 0.0;
                double medVelocity = 0.0;

                foreach( pricePointData ppData in price_point_by_date[ prod ].Values ) {

                    if( ppData.price > 0 && ppData.velocity > 0 ) {
                        priceList.Add( ppData.price );
                        velList.Add( ppData.velocity );

                        avePrice += ppData.price;
                        aveVel += ppData.velocity;
                    }
                }

                int count = priceList.Count;

                if( count > 0 ) {

                    priceList.Sort();
                    velList.Sort();

                    medPrice = priceList[ count / 2 ];
                    medVelocity = velList[ count / 2 ];

                    avePrice /= count;
                    aveVel /= count;

                    double pe = 0.0;
                    double sigma = 0.0;
                    double sigma_vel = 0;
                    double r2 = 0;

                    foreach( pricePointData ppData in price_point_by_date[ prod ].Values ) {
                        if( ppData.price > 0 && ppData.velocity > 0 ) {
                            pe += (ppData.velocity - aveVel) * (ppData.price - avePrice);
                            sigma += (ppData.price - avePrice) * (ppData.price - avePrice);
                            sigma_vel += (ppData.velocity - aveVel) * (ppData.velocity - aveVel);
                        }
                    }

                    // if sigma zero then the price did not change and we cannot calculate
                    // (pe should therefore be zero as well
                    if( medVelocity * sigma > 0 ) {
                        pe *= medPrice / (medVelocity * sigma);
                    }

                    if( sigma * sigma_vel > 0 ) {
                        r2 = pe * pe / (sigma * sigma_vel);
                    }

                    pp_out[ prod ].Val = pe;
                    pp_out[ prod ].R2 = r2;
                }
            }
        }
    }
}
