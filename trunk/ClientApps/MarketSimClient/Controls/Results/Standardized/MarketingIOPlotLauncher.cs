using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Results.Standardized
{
    public partial class MarketingIOPlotLauncher : Form
    {
        private System.Data.OleDb.OleDbCommand dbCommand;
        private System.Data.OleDb.OleDbDataAdapter dbAdapter;

        private int modelID;
        private int runID;
        private string titleFormat;
        private string subTitle;

        private bool useShareWeightedAveragesToCombine = false;

        private string[] productNames;
        private int[] productIDs;

        public string SubTitle {
            set { subTitle = value; }
        }

        public MarketingIOPlotLauncher() {
            InitializeComponent();
        }

        public MarketingIOPlotLauncher( DateTime startDateValue, DateTime endDateValue, int modelID, int runID, string titleFormat ): this() {
            Console.WriteLine( "PLOTTING modelID = {0}, rrun_id = {1}", modelID, runID );
            this.startDate.Value = startDateValue;
            this.endDate.Value = endDateValue;
            this.modelID = modelID;
            this.runID = runID;
            this.titleFormat = titleFormat;
        }

        public System.Data.OleDb.OleDbConnection Connection {
            set { 
                this.dbCommand = MrktSimDb.Database.newOleDbCommand();
                this.dbCommand.Connection = value;

                this.dbAdapter = new System.Data.OleDb.OleDbDataAdapter( this.dbCommand );

                SetupProductsList();
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            this.Cursor = Cursors.WaitCursor;

            GetPlotData();
            ShowPlot();

            this.Cursor = Cursors.Default;
        }

        private DateTime startTime;

        private DataTable totalsTable;
        private DataTable productTotalsTable;
        private DataTable realTotalsTable;
        private DataTable realProductTotalsTable;

        private DateTime[] dates;
        private double[ , ] data;

        /// <summary>
        /// Get the data to plot from the results_std table.
        /// </summary>
        private void GetPlotData() {
            startTime = DateTime.Now;

            int productID = productIDs[ this.productComboBox.SelectedIndex ];

            string stdWhere = String.Format( " WHERE run_id = {0} " +
                                                    "AND calendar_date >= '{1}' AND calendar_date <= '{2}' ",
                runID,
                startDate.Value.ToString( "M/d/yyyy" ),
                endDate.Value.ToString( "M/d/yyyy" )
                );

            string stdRealWhere = String.Format( " WHERE  calendar_date >= '{0}' AND calendar_date <= '{1}'  AND [type] = 1 ",
                startDate.Value.ToString( "M/d/yyyy" ),
                endDate.Value.ToString( "M/d/yyyy" )
                );

            // total sales per day -- sum only products with brand_id = 1 which are leaf items; the remaining items are category items, which have brand_id = 0
            string tot_cmd = String.Format( "SELECT [calendar_date], SUM(num_sku_bought) as total_sku_bought FROM results_std WITH (NOLOCK) " +
                stdWhere + " AND product_id IN (SELECT product_id FROM product WHERE brand_id = 1) GROUP BY calendar_date ORDER BY calendar_date" );           
            totalsTable = new DataTable( "daily_totals" );
            FillTable( tot_cmd, totalsTable );

            // total real sales per day
            string totReal_cmd = String.Format( "SELECT [calendar_date], SUM(value) as total_sku_bought FROM external_data  " + stdRealWhere + 
                " AND model_id = " + this.modelID + " GROUP BY calendar_date ORDER BY calendar_date" );
            realTotalsTable = new DataTable( "daily_real_totals" );
            FillTable( totReal_cmd, realTotalsTable );

             string summable_cmd = null;
             if( useShareWeightedAveragesToCombine == false ) {
                 // combine numbers over channels and segments by straight (non-share-weighted) averaging
                 summable_cmd = String.Format( "SELECT " +
                     " [calendar_date], " +
                     " SUM(num_sku_bought) as [sku_bought], " +
                     " AVG(percent_aware_sku_cum * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_awareness, " +
                     " AVG(persuasion_sku * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_persuasion, " +
                     " AVG(unpromoprice * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_unpromo_price, " +
                     " AVG(promoprice * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_promo_price, " +
                     " AVG(percent_sku_at_promo_price * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_promo, " +
                     " AVG(percent_preuse_distribution_sku * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_dist, " +
                     " AVG(percent_on_display_sku * segment.segment_size/100.0) * (select count(*) from segment where model_id = {1}) as avg_display, " +
                     " SUM(num_units_bought_on_coupon) as sku_coupons, " +
                     " SUM(GRPs_SKU_tick) as sku_grps " +
                     " FROM results_std, segment WITH (NOLOCK) " +
                     stdWhere + " AND results_std.segment_id = segment.segment_id " +
                     " AND product_id = {0} GROUP BY calendar_date ", productID, this.modelID );
             }
             else {
                 // combine numbers over channels and segments using share-weighted averaging
                 summable_cmd = String.Format( "SELECT " +
                     " [calendar_date], " +
                     " SUM(num_sku_bought) as [sku_bought], " +
                     " SUM(percent_aware_sku_cum * num_sku_bought) / SUM(num_sku_bought) as sku_awareness, " +
                     " SUM(persuasion_sku * num_sku_bought) / SUM(num_sku_bought) as sku_persuasion, " +
                     " SUM(unpromoprice * num_sku_bought) / SUM(num_sku_bought) as sku_unpromo_price, " +
                     " SUM(promoprice * num_sku_bought) / SUM(num_sku_bought) as sku_promo_price, " +
                     " SUM(percent_sku_at_promo_price * num_sku_bought) / SUM(num_sku_bought) as sku_promo, " +
                     " SUM(percent_preuse_distribution_sku * num_sku_bought) / SUM(num_sku_bought) as sku_dist, " +
                     " SUM(percent_on_display_sku * num_sku_bought) / SUM(num_sku_bought) as sku_display, " +
                     " SUM(num_units_bought_on_coupon) as sku_coupons, " +
                     " SUM(GRPs_SKU_tick) as sku_grps " +
                     " FROM results_std, segment WITH (NOLOCK) " +
                     stdWhere + " AND results_std.segment_id = segment.segment_id " +
                     " AND product_id = {0} GROUP BY calendar_date ", productID );
             }

            productTotalsTable = new DataTable( "product_totals" );
            FillTable( summable_cmd, productTotalsTable );

            // total real sales per day, per product
            string summableReal_cmd = String.Format( "SELECT [calendar_date], SUM(value) as sku_bought FROM external_data  " + stdRealWhere +
                " AND product_id = {0} GROUP BY calendar_date ORDER BY calendar_date", productID );
            realProductTotalsTable = new DataTable( "product_real_titles" );
            FillTable( summableReal_cmd, realProductTotalsTable );

            // get the media data - total GRP per day, per product - start by getting all parents (brands, etc.) for the current product
            ArrayList productAndBrandIDs = new ArrayList();
            productAndBrandIDs.Add( productID );
            DataTable productTreItemTable = new DataTable( "tree_item" );
            int iDToCheck = productID;
            string productWhere = "(product_id = " + productID;
            do {
                string par_cmd = "SELECT [parent_id] from product_tree where [child_id] = " + iDToCheck;
                FillTable( par_cmd, productTreItemTable );

                if( productTreItemTable.Rows.Count == 1 ) {
                    int prodParentId = (int)productTreItemTable.Rows[ 0 ][ 0 ];
                    productAndBrandIDs.Add( prodParentId );
                    iDToCheck = prodParentId;
                    productWhere += " OR product_id = " + prodParentId;
                }
            } 
            while( productTreItemTable.Rows.Count == 1 );
            productWhere += ")";
            // the product-and-parent "where" clause is now ready

            // get the media data itself
            string media_cmd = "select [start_date], [end_date], [attr_value_G] as grps, [media_type] FROM mass_media " +
                " WHERE (end_date >= '" + startDate.Value.ToShortDateString() + "' OR start_date <= '" + endDate.Value.ToShortDateString() + "') AND " +
                productWhere + " AND model_id = " + modelID + " ORDER BY [start_date]";

            DataTable productMediaTable = new DataTable( "media_table" );
            FillTable( media_cmd, productMediaTable );
            // normalize the media data
            foreach( DataRow mRow in productMediaTable.Rows ) {
                DateTime d1 = (DateTime)mRow[ "start_date" ];
                DateTime d2 = (DateTime)mRow[ "end_date" ];
                double unNorm = (double)mRow[ "grps" ];
                TimeSpan t1 = d2 - d1;
                double normVal = unNorm / t1.TotalDays;
                mRow[ "grps" ] = normVal;
            }

            // get the market utility data
            string mkt_utility_cmd = "select [start_date], [end_date], [percent_dist] FROM market_utility " +
                " WHERE (end_date >= '" + startDate.Value.ToShortDateString() + "' OR start_date <= '" + endDate.Value.ToShortDateString() + "') AND " +
                "product_id =" + productID + " AND model_id = " + modelID + " ORDER BY [start_date]";

            DataTable marketUtilityTable = new DataTable( "marhet_utility" );
            FillTable( mkt_utility_cmd, marketUtilityTable );

            // put the data array together for the Graph
            dates = new DateTime[ totalsTable.Rows.Count ];
            data = new double[ totalsTable.Rows.Count, 13 ];
            int shareDataCol = 0;
            int persuasionDataCol = 1;
            int awarenessDataCol = 2;
            int realShareDataCol = 3;
            int errDataCol = 4;
            int unpromoPriceDataCol = 5;
            int promoDataCol = 6;
            int distDataCol = 7;
            int promoPriceDataCol = 8;
            int grpDataCol = 9;
            int dispDataCol = 10;
            int couponsDataCol = 11;
            int marketUtilityDataCol = 12;

            DateTime prevDataDate = DateTime.MinValue;
            int realDataIndex = 0;
            int realProductDataIndex = 0;
            int mktUtilityDataIndex = 0;
            for( int r = 0; r < dates.Length; r++ ) {
                // set the date for this row
                DateTime dataDate = (DateTime)totalsTable.Rows[ r ][ 0 ];

                dates[ r ] = dataDate;

                //determine the measurement interval in days, to normalize awareness values
                TimeSpan interval = new TimeSpan( 7, 0, 0, 0 );
                if( r != 0 ) {
                    interval = dataDate - prevDataDate;
                }
                else {
                    // the first row needs special handling since there is no previous point
                    if( dates.Length > 1 ) {
                        DateTime nextDataDate = (DateTime)totalsTable.Rows[ 1 ][ 0 ];
                        interval = nextDataDate - dataDate;
                        // see if this is monthly data - using the previous month isn't necessarily ok
                        if( interval.TotalDays > 25 && interval.TotalDays < 35 ) {
                            DateTime nextDay = dataDate.AddDays( 1 );
                            if( nextDay.Day == 1 ) {
                                // the dataDate point is the last day of the month, so use the length of the first month as the first interval
                                interval = dataDate - new DateTime( dataDate.Year, dataDate.Month, 1 ).AddDays( -1 );
                            }
                        }
                    }
                }

                // compute sim share
                if( (double)totalsTable.Rows[ r ][ 1 ] != 0 ) {
                    data[ r, shareDataCol ] = 100 * (double)productTotalsTable.Rows[ r ][ "sku_bought" ] / (double)totalsTable.Rows[ r ][ 1 ];
                }
                else {
                    data[ r, shareDataCol ] = 0;
                }

                if( useShareWeightedAveragesToCombine == false ) {
                    data[ r, persuasionDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_persuasion" ] / interval.TotalDays;
                    data[ r, awarenessDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_awareness" ] / interval.TotalDays;      
                    data[ r, unpromoPriceDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_unpromo_price" ] / interval.TotalDays;
                    data[ r, promoPriceDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_promo_price" ] / interval.TotalDays;
                    data[ r, promoDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_promo" ] / interval.TotalDays;
                    data[ r, distDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_dist" ] / interval.TotalDays;                     
                    data[ r, dispDataCol ] = (double)productTotalsTable.Rows[ r ][ "avg_display" ] / interval.TotalDays;               
                }
                else {
                    data[ r, persuasionDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_persuasion" ] / interval.TotalDays;
                    data[ r, awarenessDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_awareness" ] / interval.TotalDays;     
                    data[ r, unpromoPriceDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_unpromo_price" ] / interval.TotalDays;
                    data[ r, promoPriceDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_promo_price" ] / interval.TotalDays;
                    data[ r, promoDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_promo" ] / interval.TotalDays;
                    data[ r, distDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_dist" ] / interval.TotalDays;                    
                    data[ r, dispDataCol ] = (double)productTotalsTable.Rows[ r ][ "sku_display" ] / interval.TotalDays;                
                }

                // get the real data that matches the time interval
                if( realDataIndex < realTotalsTable.Rows.Count && realProductDataIndex < realProductTotalsTable.Rows.Count ) {
                    DateTime resultsValueDate = (DateTime)realTotalsTable.Rows[ realDataIndex ][ 0 ];
                    DateTime resultsProductValueDate = (DateTime)realProductTotalsTable.Rows[ realProductDataIndex ][ 0 ];
                    if( (resultsValueDate > prevDataDate) && (resultsValueDate <= dataDate) ) {
                        // we are good -- accumulate all real data up to the next sim time step
                        double realProductTotal = 0;
                        double realTotal = 0;
                        do {
                            // accumulate product data, provided there is a row for this datel
                            if( resultsProductValueDate == resultsValueDate ) {
                                realProductTotal += (double)realProductTotalsTable.Rows[ realProductDataIndex ][ 1 ];
                                realProductDataIndex += 1;
                                if( realProductDataIndex < realProductTotalsTable.Rows.Count ) {
                                    resultsProductValueDate = (DateTime)realProductTotalsTable.Rows[ realProductDataIndex ][ 0 ];
                                }
                            }
                            realTotal += (double)realTotalsTable.Rows[ realDataIndex ][ 1 ];
                            realDataIndex += 1;
                            if( realDataIndex < realTotalsTable.Rows.Count ) {
                                resultsValueDate = (DateTime)realTotalsTable.Rows[ realDataIndex ][ 0 ];
                            }
                        } while( realDataIndex < realTotalsTable.Rows.Count && resultsValueDate <= dataDate );

                        data[ r, realShareDataCol ] = 100 * realProductTotal / realTotal;
                    }
                    else if( resultsValueDate > dataDate ) {
                        // no real data for this time interval
                        data[ r, realShareDataCol ] = 0;
                    }
                }
                else {
                    // we've run past the end of the real data
                    data[ r, realShareDataCol ] = 0;
                }

                // get the market utility data that matches the time interval
                if( mktUtilityDataIndex < marketUtilityTable.Rows.Count ) {
                    DateTime utilityValueDate = (DateTime)marketUtilityTable.Rows[ mktUtilityDataIndex ][ 1 ];       // the end date of the interval
                    if( (utilityValueDate > prevDataDate) && (utilityValueDate <= dataDate) ) {
                        // we are good -- accumulate all real data up to the next sim time step
                        double utilityTotal = 0;
                        double utilityItemCount = 0;
                        do {
                            utilityTotal += (double)marketUtilityTable.Rows[ mktUtilityDataIndex ][ 2 ];
                            utilityItemCount += 1;

                            mktUtilityDataIndex += 1;
                            if( mktUtilityDataIndex < marketUtilityTable.Rows.Count ) {
                                utilityValueDate = (DateTime)marketUtilityTable.Rows[ mktUtilityDataIndex ][ 1 ];             // the end date of the interval
                            }
                        } while( mktUtilityDataIndex < marketUtilityTable.Rows.Count && utilityValueDate <= dataDate );

                        if( utilityTotal > 0 ) {
                            data[ r, marketUtilityDataCol ] = utilityTotal / utilityItemCount;       // use a simple average for market utility if multiple items match an interval
                        }
                        else {
                            data[ r, marketUtilityDataCol ] = 0;
                        }
                    }
                    else if( utilityValueDate > dataDate ) {
                        // no real data for this time interval
                        data[ r, marketUtilityDataCol ] = 0;
                    }
                }
                else {
                    // we've run past the end of the real data
                    data[ r, marketUtilityDataCol ] = 0;
                }

                // accumulate the media GRPs from the input data so we can aggregate over brands as well as products
                double grps = 0;
                double coupons = 0;
                foreach( DataRow mRow in productMediaTable.Rows ) {
                    DateTime mediaD1 = (DateTime)mRow[ "start_date" ];
                    DateTime mediaD2 = (DateTime)mRow[ "end_date" ];
                    double itemDailyGrpVal = (double)mRow[ "grps" ];
                    string  mediaType = (string)mRow[ "media_type" ];
                    DateTime intervalStart = dataDate.AddDays( -interval.TotalDays );
                    if( mediaD2 < intervalStart || mediaD1 > dataDate ) {
                        // no overlap of the interval and this media item
                        continue;
                    }
                    else {
                        DateTime overlapStart = intervalStart;
                        DateTime overlapEnd = dataDate;
                        if( mediaD1 > intervalStart ) {
                            overlapStart = mediaD1;
                        }
                        else if( mediaD2 < dataDate ) {
                            overlapEnd = mediaD2;
                        }
                        TimeSpan overlap = overlapEnd - overlapStart;
                        double overlapRatio = overlap.TotalDays / interval.TotalDays;
                        if( mediaType == "V" || mediaType == "v" ) {
                            grps += itemDailyGrpVal * overlapRatio;
                        }
                        else if( mediaType == "C" || mediaType == "c" ) {
                            coupons += itemDailyGrpVal * overlapRatio;
                       }
                    }
                }
                data[ r, grpDataCol ] = grps;
                data[ r, couponsDataCol ] = coupons;

                prevDataDate = dataDate;

                double err = 0;
                double maxErr = 200;

                if( data[ r, realShareDataCol ] == 0.0 ) {
                    if( data[ r, shareDataCol ] > 0.0 ) {
                        err = maxErr;
                    }
                    else if( data[ r, shareDataCol ] < 0.0 ) {
                        err = -maxErr;
                    }
                }
                else {
                    err = ((data[ r, shareDataCol ] / data[ r, realShareDataCol ]) - 1.0) * 100;
                    err = Math.Max( Math.Min( err, maxErr ), -maxErr );
                }

                data[ r, errDataCol ] = err;

                Console.WriteLine( "Date: {0}:  Unpromo Price = {1:f2}   Promo Price = {2:f2}, GRPs = {3:f2}",
                   dataDate.ToShortDateString(), data[ r, unpromoPriceDataCol ], data[ r, promoPriceDataCol ], data[ r, grpDataCol ] );

            }   // end of loop for r
        }

        private bool FillTable( string selectCommand, DataTable table ) {
            dbCommand.CommandText = selectCommand;

            try {
                dbAdapter.Fill( table );
            }
            catch( Exception ex ) {
                MessageBox.Show( "Exception reading results data table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }
            return true;
        }


        private void ShowPlot() {
            TimeSpan ts = DateTime.Now - startTime;
            ////string s = String.Format( "PLOT!  Command Time {0:f3} sec\n\n# of Results rows = {1}\n# of Totals rows = {2}", 
            ////    ts.TotalSeconds , productTotalsTable.Rows.Count, totalsTable.Rows.Count );

            ////MessageBox.Show( s );

            Console.WriteLine( "\nPlotting product ID = {0}", productIDs[ this.productComboBox.SelectedIndex ] );

            string title = String.Format( titleFormat, (string)productComboBox.SelectedItem );
            string product = (string)this.productComboBox.SelectedItem;

            MarketingIOPlotForm gform = new MarketingIOPlotForm( dates, data, product, title, subTitle );
            gform.Show();
        }

        #region Fill Products List
        /// <summary>
        /// Populates the product combo box wil the names of all (leaf) products; loads productIDs with the corresponding ID values.
        /// </summary>
        private void SetupProductsList()
        {
            DataTable productTable = new DataTable( "products" );
            dbCommand.CommandText = "SELECT * FROM product WHERE model_id = " + modelID.ToString();

            try {
                dbAdapter.Fill( productTable );
            }
            catch( Exception ex ) {
                MessageBox.Show( "Exception reading products table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            DataTable productTreeTable = new DataTable( "product_tree" );
            dbCommand.CommandText = "SELECT * FROM product_tree WHERE model_id = " + modelID.ToString();

            try {
                dbAdapter.Fill( productTreeTable );
            }
            catch( Exception ex ) {
                MessageBox.Show( "Exception reading product_tree table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            // get all products and ther ids
            int id_col = 1;
            int name_col = 2;

            ArrayList names = new ArrayList();
            ArrayList ids = new ArrayList();

            for( int i = 0; i < productTable.Rows.Count; i++ ) {
                int id = (int)productTable.Rows[ i ][ id_col ];
                string name = (string)productTable.Rows[ i ][ name_col ];
                names.Add( name );
                ids.Add( id );
            }

            // get all parent ids from the product tre
            ArrayList parentIDs = new ArrayList();
            int parent_id_col = 1;

            for( int i = 0; i < productTreeTable.Rows.Count; i++ ) {
                int id = (int)productTreeTable.Rows[ i ][ parent_id_col ];
                parentIDs.Add( id );
            }

            // remove all items that are parents
            int num = names.Count;
            for( int i = num - 1; i >= 0; i-- ) {
                if( parentIDs.Contains( ids[ i ] ) ){
                    // remove non-leaf item
                    names.RemoveAt( i );
                    ids.RemoveAt( i );
                }
            }

            // sort the results
            productNames = new string[ names.Count ];
            productIDs = new int[ names.Count ];
            names.CopyTo( productNames );
            ids.CopyTo( productIDs );
            Array.Sort( productNames, productIDs );

            // set the control
            this.productComboBox.Items.AddRange( productNames );
            this.productComboBox.SelectedIndex = 0;
        }
        #endregion
    }
}