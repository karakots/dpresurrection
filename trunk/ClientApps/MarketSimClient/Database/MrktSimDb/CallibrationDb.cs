using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;

using MathUtility;
using MrktSimDb.Metrics;

namespace MrktSimDb
{
    /// <summary>
    /// adds attributes to SimulationDb
    /// </summary>
    public class CallibrationDb : SimulationDb
    {
        private double scale = 0;
        private double saturation = 0;
        private MetricUtil metricUtil = null;

        public Solver.Saturation InitSaturationFcn() {

            scale = 0;
            saturation = 0;

            int numSegs = 0;
            bool useSaturation = false;

            foreach( MrktSimDBSchema.segmentRow seg in Data.segment.Select() ) {

                if( seg.segment_id == Database.AllID ) {
                    continue;
                }

                numSegs++;

                if( seg.persuasion_value_computation != "Absolute" ) {
                    saturation += seg.units_desired_trigger;
                    useSaturation = true;
                }

                scale += seg.persuasion_scaling;  
            }

            if( numSegs > 0 ) {
                scale /= numSegs;
                saturation /= numSegs;
            }

            if( useSaturation ) {
                return new Solver.Saturation( ExpSaturation );
            }

            return new Solver.Saturation( NoSaturation );
        }

        /// <summary>
        /// Saturation function
        /// </summary>
        /// <param name="persuasion"></param>
        /// <param name="numDerivs"></param>
        /// <returns></returns>
        public double ExpSaturation( double persuasion, int numDerivs ) {

            if( saturation > 0 ) {
                switch( numDerivs ) {
                    case 0:
                        persuasion = scale * saturation * (1 - Math.Exp( -persuasion / saturation ));
                        break;
                    case 1:
                        persuasion = scale * Math.Exp( -persuasion / saturation );
                        break;
                    case 2:
                        persuasion = -scale * Math.Exp( -persuasion / saturation ) / saturation;
                        break;
                    default:
                        persuasion = scale * Math.Exp( -persuasion / saturation ) / Math.Pow( -saturation, numDerivs - 1 );
                        break;
                }
            }

            return persuasion;
        }

        /// <summary>
        /// Share of voice saturation
        /// </summary>
        /// <param name="persuasion"></param>
        /// <param name="numDerivs"></param>
        /// <returns></returns>
        public double ShareOfVoiceSaturation( double persuasion, int numDerivs )
        {
            bool pos = persuasion > 0;

            if( !pos )
            {
                persuasion = -persuasion;
            }

            double sum = saturation + persuasion;

            if( saturation > 0 )
            {
                switch( numDerivs )
                {
                    case 0:
                        persuasion = scale * saturation * persuasion / sum;
                        break;
                    case 1:
                        persuasion = scale * saturation * saturation / (sum*sum);
                        pos = true;
                        break;
                    case 2:
                        persuasion = -2 * scale * saturation * saturation / (sum * sum * sum);
                        break;
                    default:
                        persuasion = scale * scale * saturation * saturation * Math.Pow( -sum, numDerivs + 1 ) * factorial(numDerivs);
                        break;
                }
            }

            if( !pos )
            {
                persuasion = -persuasion;
            }

            return persuasion;
        }

        // this will do for now
        private int factorial( int n )
        {
            if( n == 0 || n == 1 )
                return 1;

            if( n == 2 )
                return 2;

            return n * factorial( n - 1 );
        }

        /// <summary>
        /// Saturation function
        /// </summary>
        /// <param name="persuasion"></param>
        /// <param name="numDerivs"></param>
        /// <returns></returns>
        public double NoSaturation( double persuasion, int numDerivs ) {


            switch( numDerivs ) {
                case 0:
                    persuasion = scale * persuasion;
                    break;
                case 1:
                    persuasion = scale;
                    break;

                default:
                    persuasion = 0;
                    break;
            }
           

            return persuasion;
        }


        /// <summary>
        /// Sim Units per segment per channel, per date, per product
        /// </summary>
    
        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.product_attribute ||
                table == Data.consumer_preference ||
                table == Data.segment ||
                table == Data.channel ||
                table == Data.product_type ||
                 table == Data.product_tree ||
                table == Data.product ||
                table == Data.pack_size)
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();


            if (Id < 0)
                return;

            OleDbDataAdapter adapter = null;

            adapter = getAdapter(Data.product_attribute);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.consumer_preference);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.segment);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.product_type);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.product_tree);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.product);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter( Data.pack_size );
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

            adapter = getAdapter(Data.channel);
            adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

        }

    

        public int ParameterProduct(MrktSimDBSchema.model_parameterRow parm)
        {
            int prodID = -1;

            genericCommand.CommandText = " SELECT product_id FROM " + parm.table_name + " WHERE " + parm.filter;

            Connection.Open();
            try
            {
                prodID = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException) { }

            Connection.Close();

            return prodID;
        }

        #region volume calibration
        /// <summary>

       /// Computes volume error for each channel at associated dates
       /// </summary>
       /// <param name="run_id"></param>
       /// <param name="error"></param>
       /// <param name="calendarDates"></param>


        private void init_share_data(int run_id)
        {
            if( metricUtil == null ) {
                metricUtil = new MetricUtil(this);
            }

            metricUtil.Run = run_id;
        }

        public void Volume( int run_id, out List<List<double>> sim_sales, out List<List<double>> real_sales)
        {
            init_share_data( run_id );

            sim_sales = new List<List<double>>();

            List<double> chan_cat_units = null;
            int chanDex = 0;
        
            foreach(SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> channelDateProductSales in metricUtil.SimUnits.Values ) {

                chanDex = 0;
                foreach(int channelID in channelDateProductSales.Keys ) {
                    SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales = channelDateProductSales[channelID];

                    if( chanDex == sim_sales.Count ) {
                        chan_cat_units = new List<double>();
                        sim_sales.Add( chan_cat_units );
                    }

                    chan_cat_units = sim_sales[ chanDex ];

                    int tt = 0;
                    foreach(DateTime date  in dateProductSales.Keys ) {

                        SortedDictionary<int, double> prodSales = dateProductSales[ date ];

                        double total = 0;
                        foreach( int prodId in prodSales.Keys ) {
                            double sales = prodSales[prodId];

                            if( metricUtil.RealUnits[ channelID ][ date ].ContainsKey( prodId ) ) {
                                total += sales;
                            }
                        }

                        if( tt == chan_cat_units.Count ) {
                            chan_cat_units.Add( total );
                        }
                        else {
                            chan_cat_units[tt] += total;
                        }

                        tt++;
                    }
                    chanDex++;
                }

                
            }

            // now do same for real data
            // how many dates
            real_sales = new List<List<double>>();


            chanDex = 0;
            foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales in metricUtil.RealUnits.Values ) {
                if( chanDex == real_sales.Count ) {
                    chan_cat_units = new List<double>();
                    real_sales.Add( chan_cat_units );
                }

                int tt = 0;
                foreach( DateTime date in dateProductSales.Keys ) {

                    SortedDictionary<int, double> prodSales = dateProductSales[ date ];


                    double total = 0;
                    foreach( double sales in prodSales.Values ) {
                        total += sales;
                    }

                    if( tt == chan_cat_units.Count ) {
                        chan_cat_units.Add( total );
                    }
                    else {
                        chan_cat_units[ tt ] += total;
                    }

                    tt++;

                }
                chanDex++;
            }

           
        }
       

        /// <summary>
        /// computes the percent share each segment has in each channel
        /// this is the num units segment puchased in channel/total units in channel
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="probs"></param>
        /// <returns></returns>
        public void SegmentChannelShare( int run_id, out List<List<double>> seg_chan_prob )
        {
            init_share_data( run_id );

            seg_chan_prob = new List<List<double>>();
            List<double> chan_units = null;

            List<double> chan_total = new List<double>();

            // first compute channel totals
            int segDex = 0;
            foreach( SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> channelDateProductSales in metricUtil.SimUnits.Values ) {

                int chanDex = 0;

                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales in channelDateProductSales.Values ) {

                    double total = 0;

                    foreach( SortedDictionary<int, double> prodSales in dateProductSales.Values ) {
                        foreach( double sales in prodSales.Values ) {
                            total += sales;
                        }
                    }

                    if( chanDex == chan_total.Count ) {
                        chan_total.Add( total );
                    }
                    else {
                        chan_total[ chanDex ] += total;
                    }

                    chanDex++;
                }
                segDex++;
            }

            // now compute probs
            segDex = 0;
            foreach( SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> channelDateProductSales in metricUtil.SimUnits.Values ) {

                if( segDex == seg_chan_prob.Count ) {
                    chan_units = new List<double>();

                    seg_chan_prob.Add( chan_units );
                }

                int chanDex = 0;
            
                foreach( SortedDictionary<DateTime, SortedDictionary<int, double>> dateProductSales in channelDateProductSales.Values ) {

                    if( chan_total[ chanDex ] > 0 ) {
                        double total = 0;

                        foreach( SortedDictionary<int, double> prodSales in dateProductSales.Values ) {
                            foreach( double sales in prodSales.Values ) {
                                total += sales;
                            }
                        }

                        total /= chan_total[ chanDex ];


                        if( chanDex == chan_units.Count ) {
                            chan_units.Add( total );
                        }
                        else {
                            chan_units[ chanDex ] += total ;
                        }
                    }

                    chanDex++;
                }
                segDex++;
            }

        }

        #endregion

        #region new calibration tools

        /// <summary>
        /// Gets simulation parameter
        /// </summary>
        /// <param name="sim_id"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="idCol"></param>
        /// <param name="id"></param>
        /// <returns> null if simulation parameter does not exist</returns>
        public MrktSimDBSchema.scenario_parameterRow GetParm( int sim_id, string table, string column, string idCol, int id) {

            string query = "table_name = '" + table + "' AND col_name = '" + column + "' AND identity_row = '" + idCol + "' AND row_id = " + id.ToString();

            DataRow[] rows = Data.model_parameter.Select( query );

            if( rows.Length > 0 ) {
                // create simulation parameter and 

                int modelParmId = (int)rows[ 0 ][ "id" ];

                MrktSimDBSchema.scenario_parameterRow simParm = GetSimulationParameter( sim_id, modelParmId );

                return simParm;
            }

            return null;
        }

        // adds val to current value of the simulation parameter assoicated with this parameter
        // if it exists
        public bool UpdateParm( string table, string column, string idCol, int id, double val ) {

            MrktSimDBSchema.scenario_parameterRow simParm = GetParm( Id, table, column, idCol, id );

            if( simParm != null ) {
                simParm.aValue += val;

                return true;
            }


            return false;
        }

        private int countChildren( int prodID ) {

            int numCildren = 0;

            string query = "parent_id = " + prodID;

            foreach( MrktSimDBSchema.product_treeRow treeRow in Data.product_tree.Select( query ) ) {
                numCildren += countChildren( treeRow.child_id );
            }


            // I am a child
            if( numCildren == 0 ) {
                numCildren = 1;
            }

            return numCildren;
        }

        /// <summary>
        /// translates the media data into lists for use by solver
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="mediaPersuasion"> [prod][plan][date] = egrp</param>
        /// <param name="prodPersuasion">[prod][date_index] = persuasion</param>
        /// <param name="dates">[date_index] = date</param>
        /// <param name="weights"> [date_index] = weight</param>
        /// <param name="media_data">[date][prod][plan] = egrp</param>
        /// <param name="totalVals">[prod][date_index] = persuasion</param>
        public void GetMediaData(
            int run_id,
            SortedDictionary<int, SortedDictionary<int, List<double>>> mediaPersuasion,
            SortedDictionary<int, List<double>> prodPersuasion,
            out List<List<double>> sim_share,
            out List<List<double>> real_share,
            out List<double> weights,
            out List<List<List<double>>> media_data,
            out List<List<double>> totalVals,
            out List<int> leafs)
        {
            init_share_data( run_id );

            real_share = metricUtil.computeChannelTotalRealLeafShare();
            sim_share = metricUtil.computeChannelTotalSimLeafShare();
            weights = metricUtil.computeChannelTotalWeights();

            totalVals = new List<List<double>>();
            media_data = new List<List<List<double>>>();

            // we simply average the totalVals from the proPerusasion
            int prodIndex = 0;

            leafs = metricUtil.RealLeafs;
            totalVals = new List<List<double>>();

            // create persuasion
            foreach( int prodID in leafs )
            {
                // we check that this product is a leaf in the real data

                if( prodPersuasion.ContainsKey( prodID ) )
                {
                    int dateDex = 0;
                    foreach( double pers in prodPersuasion[prodID] )
                    {
                        if( dateDex == totalVals.Count )
                        {
                            totalVals.Add( new List<double>() );
                        }

                        List<double> dateProdPersuasion = totalVals[dateDex];

                        if( prodIndex == dateProdPersuasion.Count )
                        {
                            dateProdPersuasion.Add( pers );
                        }
                        else
                        {
                            throw new Exception( "error with products" );
                        }

                        ++dateDex;
                    }


                    ++prodIndex;
                }
                else
                {
                    throw new Exception( "prod not found" );
                }

            }


            media_data = new List<List<List<double>>>();
            prodIndex = 0;
            foreach( int prodID in leafs )
            {
                int mediaIndex = 0;
                foreach( int parent_id in mediaPersuasion.Keys )
                {
                    bool setValue = isAncestor( parent_id, prodID );

                    SortedDictionary<int, List<double>> planPersuasion = mediaPersuasion[parent_id];
                    foreach( int plan_id in planPersuasion.Keys )
                    {
                        List<double> persuasion_per_date = planPersuasion[plan_id];

                        int dateDex = 0;
                        foreach( double pers in persuasion_per_date )
                        {
                            if( dateDex == media_data.Count )
                            {
                                media_data.Add( new List<List<double>>() );
                            }

                            List<List<double>> prodPlanVal = media_data[dateDex];

                            if( prodIndex == prodPlanVal.Count )
                            {
                                prodPlanVal.Add( new List<double>() );
                            }

                            List<double> mediaPers = prodPlanVal[prodIndex];

                            if( mediaIndex == mediaPers.Count )
                            { // add values
                                if( setValue )
                                {
                                    mediaPers.Add( pers );
                                }
                                else
                                {
                                    mediaPers.Add( 0 );
                                }
                            }
                            else
                            {
                                throw new Exception( "oops" );
                            }
                            ++dateDex;
                        }
                        ++mediaIndex;
                    }
                }
                ++prodIndex;
            }
        }

        private bool isAncestor(int parent_id, int child_id)
        {
            while (child_id != parent_id)
            {
                DataRow[] rows = Data.product_tree.Select("child_id = " + child_id);
                if (rows.Length > 0)
                {
                    child_id = (int)rows[0]["parent_id"];
                }
                else
                {
                    break;
                }
            }

            if (child_id == parent_id)
            {
                return true;
            }

            return false;
        }

        public void GetLeafShareData(int run_id, out List<List<List<double>>> sim_share, out List<List<double>> real_share, out List<List<double>> weights)
        {
            init_share_data( run_id );

            real_share = metricUtil.RealLeafShare;
            sim_share = metricUtil.SimLeafShare;

            Weights(run_id, out weights);
        }

      
        /// <summary>
        /// returns a list for each segment
        /// the weight for each channel-date
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="weightArr"> 
        /// outer list is over segments
        /// inner list is over channel-dates</param>
        private void Weights(int run_id, out List<List<double>> weights)
        {
            weights = new List<List<double>>();

            if (metricUtil == null)
            {
                throw new NullReferenceException("Could not find units");
            }

            int seg_index = 0;
            int chan_index = 0;
            double seg_total = 0.0;
            List<List<double>> seg_weights = new List<List<double>>();
            foreach (SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_dict in metricUtil.SimUnits.Values)
            {
                seg_weights.Add(new List<double>());
                seg_total = 0.0;
                chan_index = 0;
                foreach (SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in chan_dict.Values)
                {
                    seg_weights[seg_index].Add(0);
                    foreach (SortedDictionary<int, double> prod_dict in date_dict.Values)
                    {
                        foreach (double value in prod_dict.Values)
                        {
                            seg_total += value;
                            seg_weights[seg_index][chan_index] += value;
                        }
                    }
                    chan_index++;
                }
                if (seg_total > 0)
                {
                    chan_index = 0;
                    foreach (SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in chan_dict.Values)
                    {
                        seg_weights[seg_index][chan_index] = seg_weights[seg_index][chan_index] / seg_total;
                        chan_index++;
                    }
                }
                seg_index++;
            }

            double time_total = 0.0;
            double prod_total = 0.0;
            int chan_time = 0;
            for (int seg = 0; seg < seg_weights.Count; ++seg)
            {
                weights.Add(new List<double>());
                chan_time = 0;
                chan_index = 0;
                foreach (SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in metricUtil.RealUnits.Values)
                {
                    time_total = 0.0;
                    foreach (SortedDictionary<int, double> prod_dict in date_dict.Values)
                    {
                        weights[seg].Add(0);
                        foreach (double value in prod_dict.Values)
                        {
                            time_total += value;
                        }
                    }

                    foreach (SortedDictionary<int, double> prod_dict in date_dict.Values)
                    {
                        prod_total = 0.0;
                        foreach (double value in prod_dict.Values)
                        {
                            prod_total += value;
                        }
                        if (time_total > 0)
                        {
                            weights[seg][chan_time] = seg_weights[seg][chan_index] * prod_total / time_total;
                        }
                        chan_time++;
                    }
                    chan_index++;
                }
            }


        }

        public void Dates(int run_id, out List<DateTime> dates)
        {
            dates = new List<DateTime>();
            genericCommand.CommandText = "SELECT DISTINCT calendar_date FROM results_std, sim_queue, simulation " +
                " WHERE sim_queue.run_id = results_std.run_id " + 
                " AND results_std.run_id = " + run_id + 
                " AND sim_queue.sim_id = simulation.id " +
                " AND calendar_date > simulation.metric_start_date " +
                " AND calendar_date < simulation.metric_end_date + 1 " +
                " ORDER BY calendar_date";

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                dates.Add(dataReader.GetDateTime(0));
            }

            dataReader.Close();
        }

        /// <summary>
        /// Calculates the relative price for each product, channel, and date
        /// </summary>
        /// <param name="attrProdArr"> 
        /// Outer list represents channel-dates
        /// middle list is over product
        /// and inner list is over attributes</param>
        public void RelPriceVals(int run_id, out List<List<List<double>>> prod_price)
        {

            if (metricUtil == null)
            {
                throw new NullReferenceException("Could not find units");
            }

            SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> prod_price_dict = new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, double>>>();

            //First construct the dictionary from the real units
            foreach (KeyValuePair<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_date_pair in metricUtil.RealUnits)
            {
                prod_price_dict.Add(chan_date_pair.Key, new SortedDictionary<DateTime, SortedDictionary<int, double>>());
                foreach (KeyValuePair<DateTime, SortedDictionary<int, double>> date_prod_pair in chan_date_pair.Value)
                {
                    prod_price_dict[chan_date_pair.Key].Add(date_prod_pair.Key, new SortedDictionary<int, double>());
                    foreach (KeyValuePair<int, double> prod_units_pair in date_prod_pair.Value)
                    {
                        prod_price_dict[chan_date_pair.Key][date_prod_pair.Key].Add(prod_units_pair.Key, 0);
                    }
                }
            }

            //Now fill the dictionary with values from the results_std table
            genericCommand.CommandText = "SELECT  results_std.channel_id, results_std.calendar_date, results_std.product_id, " +
                " CASE   WHEN AVG(average_price.price) > 0 Then " +
                " (AVG(results_std.unpromoprice)*(100 -   (AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) - (AVG(percent_at_display_price)/AVG(simulation.access_time))) +  " +
                " AVG(results_std.promoprice)*(AVG(results_std.percent_sku_at_promo_price)/AVG(simulation.access_time)) + " +
                " AVG(results_std.display_price)*(AVG(percent_at_display_price)/AVG(simulation.access_time)))/(100*AVG(average_price.price))  " +
                " Else 0   " +
                " END as relative_price  " +
                " from results_std, product, simulation, sim_queue,  " +
                " (Select results_std.run_id,  results_std.calendar_date,  results_std.channel_id,  " +
                " AVG(results_std.unpromoprice) as price  " +
                " FROM results_std, product  " +
                " where results_std.product_id = product.product_id    " +
                " AND product.brand_id = 1  " +
                " AND unpromoprice > 0   " +
                " group by results_std.run_id,  results_std.calendar_date,  results_std.channel_id) as average_price  " +
                " where	sim_queue.run_id = results_std.run_id    " +
                " AND simulation.id = sim_queue.sim_id    " +
                " and average_price.run_id = results_std.run_id  " +
                " and average_price.channel_id = results_std.channel_id  " +
                " and average_price.calendar_date = results_std.calendar_date  " +
                " AND results_std.product_id = product.product_id    " +
                " AND product.brand_id = 1  " +
                " and results_std.run_id = " + run_id +
                " GROUP BY results_std.channel_id, results_std.calendar_date, results_std.product_id" +
                " ORDER BY results_std.channel_id, results_std.calendar_date, results_std.product_id";



            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] priceData = new object[4];
            int chan_id = 0;
            DateTime date = new DateTime();
            int prod_id = 0;
            double price = 0;

            //Grab data from database and store in a multimap
            while (dataReader.Read())
            {
                dataReader.GetValues(priceData);

                chan_id = (int)priceData[0];
                date = MetricMan.Convert2DateTime( priceData[ 1 ] );
                prod_id = (int)priceData[2];
                price = MetricMan.Convert2Double(priceData[3]);

                //If the correct data is not found then it is not in the external data and should not be included
                if (!prod_price_dict.ContainsKey(chan_id))
                {
                    continue;
                }
                if (!prod_price_dict[chan_id].ContainsKey(MetricMan.Convert2DateTime(date)))
                {
                    continue;
                }
                if (!prod_price_dict[chan_id][date].ContainsKey(prod_id))
                {
                    continue;
                }
                prod_price_dict[chan_id][date][prod_id] = -price;
            }

            dataReader.Close();

            //Now unwind the prod_price_dict to form the attribute matrices
            prod_price = new List<List<List<double>>>();
            int chan_time = 0;
            prod_id = 0;
            foreach (SortedDictionary<DateTime, SortedDictionary<int, double>> date_dict in prod_price_dict.Values)
            {
                foreach (SortedDictionary<int, double> prod_dict in date_dict.Values)
                {
                    prod_price.Add(new List<List<double>>());
                    prod_id = 0;
                    foreach (double value in prod_dict.Values)
                    {
                        prod_price[chan_time].Add(new List<double>());
                        prod_price[chan_time][prod_id].Add(value);
                        prod_id++;
                    }
                    chan_time++;
                }
            }
        }

        public void ProdAttrVals(int run_id, List<int> attr_ids, out List<List<List<double>>> prod_attr)
        {
            int attr_id;

            if (metricUtil == null)
            {
                throw new NullReferenceException("Could not find units");
            }

            SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>> prod_attr_dict = new SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>>();

            //Grab all the attribute ids
            int model_id = ((MrktSimDBSchema.Model_infoRow)this.Data.Model_info.Select("model_id <> " + Database.AllID)[0]).model_id;
            //genericCommand.CommandText = "select distinct product_attribute_id from product_attribute where model_id = " + model_id;
            //genericCommand.CommandText += "ORDER BY product_attribute_id";

            //genericCommand.Connection.Open();
            //System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //object[] attrData = new object[1];
            //List<int> attr_ids = new List<int>();

            //while (dataReader.Read())
            //{
            //    dataReader.GetValues(attrData);

            //    attr_id = (int)attrData[ 0 ];

            //    attr_ids.Add(id);

            //}

            //dataReader.Close();

            //First construct from the dictionary from the real units
            foreach (KeyValuePair<int, SortedDictionary<DateTime, SortedDictionary<int, double>>> chan_date_pair in metricUtil.RealUnits)
            {
                prod_attr_dict.Add(chan_date_pair.Key, new SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>());
                foreach (KeyValuePair<DateTime, SortedDictionary<int, double>> date_prod_pair in chan_date_pair.Value)
                {
                    prod_attr_dict[chan_date_pair.Key].Add(date_prod_pair.Key, new SortedDictionary<int, SortedDictionary<int, double>>());
                    foreach (KeyValuePair<int, double> prod_units_pair in date_prod_pair.Value)
                    {
                        prod_attr_dict[chan_date_pair.Key][date_prod_pair.Key].Add(prod_units_pair.Key, new SortedDictionary<int, double>());
                        foreach (int attr in attr_ids)
                        {
                            prod_attr_dict[chan_date_pair.Key][date_prod_pair.Key][prod_units_pair.Key].Add(attr, 0.0);
                        }
                    }
                }
            }

            genericCommand.CommandText = "select start_date, product_attribute_value.product_id, product_attribute_id, pre_attribute_value " +
            " from product_attribute_value, product " +
            " where product_attribute_value.model_id = " + model_id +
            " and product.product_id = product_attribute_value.product_id " +
            " and product.brand_id = 1 " +
            " order by start_date, product_id, product_attribute_id";

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] sigmaData = new object[4];

            DateTime start_date = new DateTime();
            DateTime current_date = new DateTime();
            SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>.Enumerator iter;
            int prod_id = 0;
            attr_id = 0;
            double value = 0.0;

            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                start_date = MetricMan.Convert2DateTime(sigmaData[0]);
                prod_id = (int)sigmaData[1];
                attr_id = (int)sigmaData[2];
                value = MetricMan.Convert2Double(sigmaData[3]);

                if( !attr_ids.Contains( attr_id ) ) {
                    continue;
                }

                foreach (SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>> chan_dict in prod_attr_dict.Values)
                {

                    iter = chan_dict.GetEnumerator();
                    while (iter.Current.Key < start_date && iter.MoveNext())
                    {
                    }

                    current_date = iter.Current.Key;
                    if (!chan_dict.ContainsKey(current_date))
                    {
                        continue;
                    }
                    if (!chan_dict[current_date].ContainsKey(prod_id))
                    {
                        continue;
                    }
                    if (!chan_dict[current_date][prod_id].ContainsKey(attr_id))
                    {
                        continue;
                    }
                    chan_dict[current_date][prod_id][attr_id] = value;

                    while (iter.MoveNext())
                    {
                        current_date = iter.Current.Key;
                        if (!chan_dict[current_date].ContainsKey(prod_id))
                        {
                            continue;
                        }
                        if (!chan_dict[current_date][prod_id].ContainsKey(attr_id))
                        {
                            continue;
                        }
                        chan_dict[current_date][prod_id][attr_id] = value;
                    }
                }

            }

            dataReader.Close();

            prod_attr = new List<List<List<double>>>();
            int chan_time = 0;
            prod_id = 0;
            foreach (SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>> date_dict in prod_attr_dict.Values)
            {
                foreach (SortedDictionary<int, SortedDictionary<int, double>> prod_dict in date_dict.Values)
                {
                    prod_attr.Add(new List<List<double>>());
                    prod_id = 0;
                    foreach (SortedDictionary<int, double> attr_dict in prod_dict.Values)
                    {
                        prod_attr[chan_time].Add(new List<double>());
                        foreach (double attr_value in attr_dict.Values)
                        {
                            prod_attr[chan_time][prod_id].Add(attr_value);
                        }
                        prod_id++;
                    }
                    chan_time++;
                }
            }


        }

        #endregion


        /// <summary>
        /// Returns the attribute values for the model
        /// </summary>
        /// <param name="attrs"></param>
        public void AttrValsTranspose(out double[][] attrs)
        {

            int model_id = ((MrktSimDBSchema.Model_infoRow)this.Data.Model_info.Select("model_id <> " + Database.AllID)[0]).model_id;

            int numProducts = this.Data.product.Select("brand_id = 1").Length;
            int numAttrs = this.Data.product_attribute.Select("product_attribute_id <> " + Database.AllID).Length;

            attrs = new double[numAttrs][];

            for (int ii = 0; ii < numAttrs; ii++)
            {
                attrs[ii] = new double[numProducts];
            }


        }

        public void AttrVals(out double[][] attrs)
        {

            int model_id = ((MrktSimDBSchema.Model_infoRow)this.Data.Model_info.Select("model_id <> " + Database.AllID)[0]).model_id;

            int numProducts = this.Data.product.Select("brand_id = 1").Length;
            int numAttrs = this.Data.product_attribute.Select("product_attribute_id <> " + Database.AllID).Length;

            attrs = new double[numAttrs][];

            for (int ii = 0; ii < numAttrs; ii++)
            {
                attrs[ii] = new double[numProducts];
            }

            genericCommand.CommandText = "select product_attribute_id, product_attribute_value.product_id, AVG(pre_attribute_value) " +
            " from product_attribute_value, product " +
            " where product_attribute_value.model_id = " + model_id +
            " and product.product_id = product_attribute_value.product_id " +
            " and product.brand_id = 1 " +
            " group by product_attribute_id, product_attribute_value.product_id" +
            " order by product_attribute_id, product_attribute_value.product_id";



            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] sigmaData = new object[3];

            int attr_index = 0;
            int prod_index = 0;
            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                int seg = (int)sigmaData[0];
                int prod = (int)sigmaData[1];

                double val = MetricMan.Convert2Double(sigmaData[2]);

                // find index of attributes

                attrs[attr_index][prod_index] = val;
                prod_index++;

                if (prod_index == numProducts)
                {
                    prod_index = 0;
                    attr_index++;
                }

            }

            dataReader.Close();
        }



        /// <summary>
        /// Inotformation about share for this run
        /// this is per segment
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="attr"></param>b
        /// <param name="simShare"></param>
        /// <param name="realShare"></param>
        /// <returns></returns>
        public bool ShareInfo(int run_id, out double[][] simShare, out double[][] realShare)
        {
            // generate a map from ids to ints
            // sort attributes by id

            int numProducts = this.Data.product.Select("brand_id = 1").Length;
            int numSegments = this.Data.segment.Select("segment_id <> " + Database.AllID).Length;

            simShare = new double[numSegments][];
            realShare = new double[numSegments][];

            for (int ii = 0; ii < numSegments; ii++)
            {
                simShare[ii] = new double[numProducts];
                realShare[ii] = new double[numProducts];
            }

            genericCommand.CommandText = "SELECT results_summary_by_product_channel_segment.segment_id,  results_summary_by_product_channel_segment.product_id, " +
                " SUM((results_summary_by_channel_segment.unit_share/100.0) * (results_summary_by_product_channel_segment.unit_share/100)) as simShare " +
                " FROM results_summary_by_channel_segment, results_summary_by_product_channel_segment, product  " +
                " WHERE results_summary_by_channel_segment.run_id = " + run_id +
                " AND results_summary_by_product_channel_segment.run_id = " + run_id +
                " AND results_summary_by_channel_segment.segment_id  = results_summary_by_product_channel_segment.segment_id " +
                " AND results_summary_by_channel_segment.channel_id  = results_summary_by_product_channel_segment.channel_id " +
                " AND product.product_id = results_summary_by_product_channel_segment.product_id " +
                " AND product.brand_id = 1 " +
                " GROUP BY results_summary_by_product_channel_segment.segment_id,  results_summary_by_product_channel_segment.product_id " +
                " ORDER BY results_summary_by_product_channel_segment.segment_id,  results_summary_by_product_channel_segment.product_id";



            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] sigmaData = new object[3];

            int seg_index = 0;
            int prod_index = 0;
            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                int seg = (int)sigmaData[0];
                int prod = (int)sigmaData[1];

                double val = MetricMan.Convert2Double(sigmaData[2]);

                // find index of attributes

                simShare[seg_index][prod_index] = val;
                prod_index++;

                if (prod_index == numProducts)
                {
                    prod_index = 0;
                    seg_index++;
                }

            }

            dataReader.Close();


            genericCommand.CommandText = "SELECT results_summary_by_channel_segment.segment_id,  external_data_summary_by_product_channel.product_id, " +
                " SUM((results_summary_by_channel_segment.unit_share/100.0) * " +
                " ((external_data_summary_by_product_channel.num_units/external_data_summary_by_channel.num_units))) as realShare " +
                " FROM results_summary_by_channel_segment,  external_data_summary_by_product_channel,  " +
                "  external_data_summary_by_channel  " +
                " WHERE results_summary_by_channel_segment.run_id = " + run_id +
                " AND external_data_summary_by_product_channel.run_id = " + run_id +
                " AND external_data_summary_by_channel.run_id =  " + run_id +
                " AND results_summary_by_channel_segment.channel_id  = external_data_summary_by_product_channel.channel_id  " +
                " AND external_data_summary_by_product_channel.channel_id = results_summary_by_channel_segment.channel_id  " +
                " AND external_data_summary_by_channel.channel_id = results_summary_by_channel_segment.channel_id  " +
                " GROUP BY results_summary_by_channel_segment.segment_id,  external_data_summary_by_product_channel.product_id " +
                " ORDER BY results_summary_by_channel_segment.segment_id,  external_data_summary_by_product_channel.product_id";



            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            seg_index = 0;
            prod_index = 0;
            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                int seg = (int)sigmaData[0];
                int prod = (int)sigmaData[1];

                double val = MetricMan.Convert2Double(sigmaData[2]);

                // find index of attributes

                realShare[seg_index][prod_index] = val;
                prod_index++;

                if (prod_index == numProducts)
                {
                    prod_index = 0;
                    seg_index++;
                }

            }

            dataReader.Close();

            return true;
        }

        public bool AttributeStats(int run_id, out double[][] diff, out double[][][] sigma)
        {
            // generate a map from ids to ints
            // sort attributes by id
            ArrayList attrIds = new ArrayList();
            ArrayList segIds = new ArrayList();

            foreach (MrktSimDBSchema.product_attributeRow attr in
                Data.product_attribute.Select("", "product_attribute_id", DataViewRowState.CurrentRows))
            {
                attrIds.Add(attr.product_attribute_id);
            }

            foreach (MrktSimDBSchema.segmentRow seg in
               Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {
                segIds.Add(seg.segment_id);
            }

            int numAttributes = attrIds.Count;
            int numSegments = segIds.Count;

            sigma = new double[numSegments][][];

            for (int ii = 0; ii < numSegments; ii++)
            {
                sigma[ii] = new double[numAttributes][];

                for (int jj = 0; jj < numAttributes; jj++)
                {
                    sigma[ii][jj] = new double[numAttributes];
                }
            }

            diff = new double[numSegments][];

            for (int ii = 0; ii < numSegments; ii++)
            {
                diff[ii] = new double[numAttributes];
            }

            genericCommand.CommandText = " SELECT  results_summary_by_product_segment.segment_id, A.product_attribute_id, B.product_attribute_id,   " +
                " SUM(A.pre_attribute_value * B.pre_attribute_value * results_summary_by_product_segment.unit_share/100.0) -   " +
                " ( SUM(A.pre_attribute_value * results_summary_by_product_segment.unit_share/100.0) *   " +
                " SUM(B.pre_attribute_value * results_summary_by_product_segment.unit_share/100.0)  ) as sigma   " +
                " FROM product_attribute_value as A, product_attribute_value as B, results_summary_by_product_segment   " +
                " WHERE results_summary_by_product_segment.run_id =  " + run_id +
                " AND results_summary_by_product_segment.product_id = A.product_id   " +
                " AND results_summary_by_product_segment.product_id = B.product_id   " +
                " GROUP BY results_summary_by_product_segment.segment_id, A.product_attribute_id, B.product_attribute_id   " +
                " ORDER BY  results_summary_by_product_segment.segment_id, A.product_attribute_id, B.product_attribute_id  ";



            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] sigmaData = new object[4];

            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                int seg = (int)sigmaData[0];
                int attr1 = (int)sigmaData[1];
                int attr2 = (int)sigmaData[2];

                double val = MetricMan.Convert2Double(sigmaData[3]);

                // find index of attributes
                int seg_index = segIds.IndexOf(seg);
                int index1 = attrIds.IndexOf(attr1);
                int index2 = attrIds.IndexOf(attr2);


                sigma[seg_index][index1][index2] = val;
            }

            dataReader.Close();
            // Two cases
            // we have channel data
            genericCommand.CommandText = " SELECT results_summary_by_channel_segment.segment_id, attr.product_attribute_id, " +
                " SUM(attr.pre_attribute_value * " +
                " (results_summary_by_channel_segment.unit_share/100.0) * results_summary_by_product_channel_segment.unit_share/100) - " +
                " SUM(attr.pre_attribute_value * " +
                " (results_summary_by_channel_segment.unit_share/100.0) *  " +
                " ((external_data_summary_by_product_channel.num_units/external_data_summary_by_channel.num_units))) as diff  " +
                " FROM product_attribute_value as attr, " +
                " results_summary_by_channel_segment, " +
                " external_data_summary_by_product_channel, " +
                " external_data_summary_by_channel, " +
                " results_summary_by_product_channel_segment " +
                " WHERE results_summary_by_channel_segment.run_id = " + run_id +
                " AND external_data_summary_by_product_channel.run_id = " + run_id +
                " AND results_summary_by_product_channel_segment.run_id = " + run_id +
                " AND external_data_summary_by_channel.run_id = " + run_id +
                " AND results_summary_by_product_channel_segment.segment_id = results_summary_by_channel_segment.segment_id " +
                " AND external_data_summary_by_product_channel.channel_id = results_summary_by_channel_segment.channel_id " +
                " AND external_data_summary_by_channel.channel_id = results_summary_by_channel_segment.channel_id " +
                " AND results_summary_by_product_channel_segment.channel_id = results_summary_by_channel_segment.channel_id " +
                " AND external_data_summary_by_product_channel.product_id = attr.product_id " +
                " AND results_summary_by_product_channel_segment.product_id = attr.product_id " +
                " GROUP BY results_summary_by_channel_segment.segment_id , attr.product_attribute_id  " +
                " ORDER BY results_summary_by_channel_segment.segment_id, attr.product_attribute_id";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] diffData = new object[3];

            while (dataReader.Read())
            {
                dataReader.GetValues(diffData);

                int seg = (int)diffData[0];
                int attr = (int)diffData[1];

                double diffVal = MetricMan.Convert2Double(diffData[2]);

                // find index of attributes
                int seg_index = segIds.IndexOf(seg);
                int attr_index = attrIds.IndexOf(attr);

                diff[seg_index][attr_index] = diffVal;
            }

            dataReader.Close();


            return true;
        }


        public void PriceSensitivity(int run_id, out double[] diff, out double[] sigma)
        {
            // TODO: as coded now this is a relative price 
            // SSN 7-3-2007

            int numSegments = Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows).Length;

            diff = new double[numSegments];
            sigma = new double[numSegments];

            genericCommand.CommandText = "select " +
                " results_summary_by_channel_segment.segment_id, " +
                " SUM((results_summary_by_channel_segment.unit_share/100.0) * results_std.num_sku_bought*average_price)/ " +
                " (AVG(results_std.unpromoprice/access_time) * SUM(results_std.num_sku_bought)) -  " +
                " SUM((results_summary_by_channel_segment.unit_share/100.0) * external_data_alligned_sales_by_product_channel.num_units*average_price)/ " +
                " (AVG(results_std.unpromoprice/access_time) * SUM(external_data_alligned_sales_by_product_channel.num_units)) AS diff " +
                " FROM " +
                " external_data_alligned_sales_by_product_channel, " +
                " results_std, " +
                " results_summary_by_channel_segment, " +
                " ( " +
                " SELECT " +
                " results_std.run_id, " +
                " results_std.calendar_date, " +
                " results_std.channel_id, " +
                " results_std.product_id, " +
                " simulation.access_time, " +
                " (AVG(results_std.unpromoprice)* " +
                " (100 -  " +
                " (AVG(results_std.percent_sku_at_promo_price)/simulation.access_time) -  " +
                " (AVG(percent_at_display_price)/simulation.access_time)) + " +
                " (AVG(results_std.promoprice)*AVG(results_std.percent_sku_at_promo_price)/simulation.access_time) +  " +
                " (AVG(results_std.display_price)* AVG(percent_at_display_price)/simulation.access_time))/(100* simulation.access_time) as average_price " +
                " from results_std, product, simulation, sim_queue " +
                " where	sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND results_std.product_id = product.product_id " +
                " AND product.brand_id = 1 " +
                " group by results_std.run_id, results_std.calendar_date, results_std.channel_id, results_std.product_id, simulation.access_time " +
                " ) as real_price " +
            " where external_data_alligned_sales_by_product_channel.run_id =  " + run_id +
            " AND real_price.run_id = external_data_alligned_sales_by_product_channel.run_id " +
            " AND real_price.channel_id = external_data_alligned_sales_by_product_channel.channel_id " +
            " AND real_price.product_id = external_data_alligned_sales_by_product_channel.product_id " +
            " AND real_price.calendar_date = external_data_alligned_sales_by_product_channel.calendar_date " +
            " AND results_std.run_id = external_data_alligned_sales_by_product_channel.run_id " +
            " AND results_std.channel_id = external_data_alligned_sales_by_product_channel.channel_id " +
            " AND results_std.product_id = external_data_alligned_sales_by_product_channel.product_id " +
            " AND results_std.calendar_date = external_data_alligned_sales_by_product_channel.calendar_date " +
            " AND results_summary_by_channel_segment.channel_id = external_data_alligned_sales_by_product_channel.channel_id " +
            " AND results_summary_by_channel_segment.run_id = external_data_alligned_sales_by_product_channel.run_id " +
            " group by	results_summary_by_channel_segment.segment_id " +
            " order by	results_summary_by_channel_segment.segment_id";


            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] diffData = new object[2];

            int segDex = 0;
            while (dataReader.Read())
            {
                dataReader.GetValues(diffData);

                int seg = (int)diffData[0];
                double val = MetricMan.Convert2Double(diffData[1]);


                diff[segDex++] = val;
            }

            dataReader.Close();


            genericCommand.CommandText = " select " +
                " results_std.segment_id, " +
                " SUM(results_std.num_sku_bought*average_price*average_price)/(AVG(results_std.unpromoprice/access_time) * AVG(results_std.unpromoprice/access_time) * SUM(results_std.num_sku_bought)) -  " +
                " SUM(results_std.num_sku_bought*average_price*average_price)* SUM(results_std.num_sku_bought*average_price*average_price)/ " +
                " (AVG(results_std.unpromoprice/access_time) * AVG(results_std.unpromoprice/access_time) * SUM(results_std.num_sku_bought) * SUM(results_std.num_sku_bought)) as sigma " +
                " FROM " +
                " results_std, " +
                " ( " +
                " SELECT " +
                " results_std.run_id, " +
                " results_std.calendar_date, " +
                " results_std.channel_id, " +
                " results_std.product_id, " +
                " simulation.access_time, " +
                " (AVG(results_std.unpromoprice)* " +
                " (100 -  " +
                " (AVG(results_std.percent_sku_at_promo_price)/simulation.access_time) -  " +
                " (AVG(percent_at_display_price)/simulation.access_time)) + " +
                " (AVG(results_std.promoprice)*AVG(results_std.percent_sku_at_promo_price)/simulation.access_time) +  " +
                " (AVG(results_std.display_price)* AVG(percent_at_display_price)/simulation.access_time))/(100* simulation.access_time) as average_price " +
                " from results_std, product, simulation, sim_queue " +
                " where	sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND results_std.product_id = product.product_id " +
                " AND product.brand_id = 1 " +
                " group by results_std.run_id, results_std.calendar_date, results_std.channel_id, results_std.product_id, simulation.access_time " +
                " ) as real_price " +
           " where results_std.run_id = " + run_id +
           " AND real_price.run_id = results_std.run_id " +
           " AND real_price.channel_id = results_std.channel_id " +
           " AND real_price.product_id = results_std.product_id " +
           " AND real_price.calendar_date = results_std.calendar_date " +
           " group by	results_std.segment_id " +
           " order by	results_std.segment_id ";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader(CommandBehavior.CloseConnection);

            object[] sigmaData = new object[2];

            segDex = 0;
            while (dataReader.Read())
            {
                dataReader.GetValues(sigmaData);

                int seg = (int)sigmaData[0];
                double val = MetricMan.Convert2Double(sigmaData[1]);


                sigma[segDex++] = val;
            }

            dataReader.Close();

        }
    }
}
