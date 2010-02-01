using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MrktSimDb.Metrics;
using Utilities.Graphing;

namespace MrktSimClient.Controls.Dialogs.Calibration
{
    public partial class VolumeCalibration : UserControl
    {
        public VolumeCalibration()
        {
            InitializeComponent();

            salesTable = new DataTable("CatagorySales");

            DataColumn idCol = salesTable.Columns.Add("channel_id", typeof(int));
            salesTable.PrimaryKey = new DataColumn[] { idCol };
            salesTable.Columns.Add("channel_name", typeof(string));
            salesTable.Columns.Add("real_sales", typeof(double));
            salesTable.Columns.Add("sim_sales", typeof(double));
            salesTable.Columns.Add("percent_error", typeof(double));
            salesTable.Columns.Add("volume_mape", typeof(double));

            catagorySalesGrid.Table = salesTable;
            catagorySalesGrid.DescriptionWindow = false;
            catagorySalesGrid.ReadOnly = true;


            segmentTable = new DataTable("SegmentInfo");

            idCol = segmentTable.Columns.Add("segment_id", typeof(int));
            segmentTable.PrimaryKey = new DataColumn[] { idCol };
            segmentTable.Columns.Add("segment_name", typeof(string));
            segmentTable.Columns.Add("percent_size", typeof(double));
            segmentTable.Columns.Add("num_consumers", typeof(double));
            segmentTable.Columns.Add("purchase_frequency", typeof(double));
            segmentTable.Columns.Add("ave_units", typeof(double));

            // segmentTable.Columns.Add("delta_percent_size", typeof(double));
            segmentTable.Columns.Add("delta_num_consumers", typeof(double));
            segmentTable.Columns.Add("delta_purchase_frequency", typeof(double));
            segmentTable.Columns.Add("delta_ave_units", typeof(double));

            segmentInfoGrid.Table = segmentTable;
            segmentInfoGrid.DescriptionWindow = false;
            segmentInfoGrid.ReadOnly = true;


            plotControl1.Title = "Volume Error & Demand Modification";
            plotControl1.Y2Axis = "";

            this.methodBox.Enabled = false;


            createTableStyle();

        }

        #region local fields for sales stats
        /// <summary>
        /// How each segment needs to scale the units bought by date (segment, date)
        /// used when writing data out and plotting
        /// </summary>
        private double[][] segmentDemandMod = null;

       
       /// <summary>
       /// total population scaling
       /// // used in writing data out
       /// </summary>
        private double total_pop_scale = 1;

        /// <summary>
        /// segment size scaling
        /// used in write data out
        /// </summary>
        private double[] segmentScale = null;

        /// <summary>
        /// catagory sales by date
        /// used in plotting
        /// </summary>
        private List<double> real_sales_by_date = null;

        /// <summary>
        /// catagory sales error by date
        /// used in plotting
        /// used in plotting
        /// </summary>
        private List<double> total_error_by_date = null;

        /// <summary>
        /// simulated catagory sales by channel and date (channel, date)
        /// used in plotting 
        /// </summary>
        private List<List<double>> sim_sales = null;

        /// <summary>
        /// simulated catagory sales by channel and date (channel, date)
        /// used in plotting 
        /// </summary>
        private List<List<double>> real_sales = null;

        /// <summary>
        /// catagory sales error by channel and date (channel, date)
        /// </summary>
        private List<List<double>> error = null;

        /// <summary>
        /// dates
        /// </summary>
        private List<DateTime> calendar_dates = null;
        #endregion

        /// <summary>
        /// for user the catagory and simulated sales
        /// </summary>
        private DataTable salesTable;

        /// <summary>
        /// for user: segment changes 
        /// </summary>
        private DataTable segmentTable;

        private int currentRun = -1;
        public int Run {
            set {
                currentRun = value;
                sim_sales = null;
            }

            get {
                return currentRun;
            }
        }

        private MrktSimDBSchema.simulationRow currentSimulation = null;
        private MrktSimDb.CallibrationDb theDb;
        public CallibrationDb Db
        {
            
            get
            {
                return theDb;
            }

            set
            {
                if (value == null)
                    return;

                this.theDb = value;

                 currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                if (currentSimulation == null)
                    return;

                foreach (MrktSimDBSchema.channelRow channel in theDb.Data.channel.Select("", "channel_id"))
                {
                    DataRow row = salesTable.NewRow();
                    row["channel_id"] = channel.channel_id;
                    row["channel_name"] = channel.channel_name;
                    row["real_sales"] = 0;
                    row["sim_sales"] = 0;
                    row["percent_error"] = 0;
                    row["volume_mape"] = 0;

                    salesTable.Rows.Add(row);
                }

                salesTable.AcceptChanges();

                foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("", "segment_id"))
                {
                    DataRow row = segmentTable.NewRow();
                    row["segment_id"] = segment.segment_id;
                  
                    if (segment.segment_id == Database.AllID)
                    {
                        row["segment_name"] = "Catagory Pop";
                        row["percent_size"] = 100.0;
                        row["num_consumers"] = currentSimulation.scenarioRow.Model_infoRow.pop_size;
                        row["purchase_frequency"] = 0;
                        row["ave_units"] = 0;
                    }
                    else
                    {
                        row["segment_name"] = segment.segment_name;
                        row["percent_size"] = segment.segment_size;
                        row["num_consumers"] = segment.segment_size * currentSimulation.scenarioRow.Model_infoRow.pop_size / 100.0;
                        row["purchase_frequency"] = segment.repurchase_period_frequency;
                        row["ave_units"] = segment.avg_max_units_purch;
                    }

                 //   row["delta_percent_size"] = 0;
                    row["delta_num_consumers"] = 0;
                    row["delta_purchase_frequency"] = 0;
                    row["delta_ave_units"] = 0;

                    segmentTable.Rows.Add(row);

                }

                segmentTable.AcceptChanges();

             
            }
        }

        private void createTableStyle()
        {
            catagorySalesGrid.Clear();

            catagorySalesGrid.AddTextColumn("channel_name", "Channel");
            catagorySalesGrid.AddNumericColumn("real_sales", "Real Sales");
            catagorySalesGrid.AddNumericColumn("sim_sales", "Sim Sales");
            catagorySalesGrid.AddNumericColumn("percent_error", "Percent Error");
            catagorySalesGrid.AddNumericColumn("volume_mape", "Volume MAPE");

            catagorySalesGrid.Reset();

            segmentInfoGrid.Clear();

            segmentInfoGrid.AddTextColumn("segment_name", "Segment");
            segmentInfoGrid.AddNumericColumn("percent_size", "Percent Size");
            segmentInfoGrid.AddNumericColumn("num_consumers", "Num Consumers ");
            segmentInfoGrid.AddNumericColumn("purchase_frequency", "Purchase Freq");
            segmentInfoGrid.AddNumericColumn("ave_units", "Ave Units");

           // segmentInfoGrid.AddNumericColumn("delta_percent_size", "Increase Percent by");
            segmentInfoGrid.AddNumericColumn("delta_num_consumers", "Increase Consumers by");
            segmentInfoGrid.AddNumericColumn("delta_purchase_frequency", "Increase Freq by");
            segmentInfoGrid.AddNumericColumn("delta_ave_units", "Increase Units by");

            segmentInfoGrid.Reset();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {

            if (sim_sales != null) {
                return;
            }

            int numChannels = theDb.Data.channel.Select("channel_id <> " + Database.AllID).Length;
            // must have channels
            if (numChannels == 0)
                return;

            int numSegments = theDb.Data.segment.Select("segment_id <> " + Database.AllID).Length;
            // must have channels
            if (numSegments == 0)
                return;

            // some usefull indices
            int segDex = 0;         // segments
            int chanDex = 0;        // channel
            int tt = 0;             // time

          
            this.Cursor = Cursors.WaitCursor;

            // need to find the percent of sales from each segment to each channel

            List<List<double>> segChannelShare = null;

            theDb.SegmentChannelShare(Run, out segChannelShare);

            if( segChannelShare.Count == 0 ) {
                return;
            }


            // for each date - compute sim_sales and error by channel
            theDb.Volume(Run, out sim_sales, out real_sales);

            theDb.Dates( Run, out calendar_dates );

            #region Compute Error for channel and date and Totals by date
            // a little bookkeeping << used for plotting
            // real sales over all channels by date
            // error (sim - real) over all channels by date

         
            error = new List<List<double>>();
            real_sales_by_date = new List<double>();
            total_error_by_date = new List<double>();

            for( chanDex = 0; chanDex < real_sales.Count; ++chanDex ) {
                error.Add( new List<double>() );

                for( tt = 0; tt < real_sales[ chanDex ].Count; ++tt ) {

                    double errVal = sim_sales[ chanDex ][ tt ] - real_sales[ chanDex ][ tt ];

                    error[ chanDex ].Add( errVal );

                    if( tt == real_sales_by_date.Count ) {

                        real_sales_by_date.Add( real_sales[ chanDex ][ tt ] );
                        total_error_by_date.Add( errVal );
                    }
                    else {
                        real_sales_by_date[ tt ] += real_sales[ chanDex ][ tt ];
                        total_error_by_date[ tt ] = errVal;
                    }
                }
            }
            #endregion

            #region totals by channel and catagory

            double total_real_sales = 0;
            double total_sales_error = 0;
            double total_abs_error = 0;
            double[] real_channel_sales = new double[numChannels];
            double[] error_by_channel = new double[numChannels];
            double[] abs_error_by_channel = new double[numChannels];

            // by channel stats
            for( chanDex = 0; chanDex < real_sales.Count; ++chanDex ) {
                real_channel_sales[chanDex] = 0;
                error_by_channel[chanDex] = 0;
                abs_error_by_channel[chanDex] = 0;

                for( tt = 0; tt < real_sales[ chanDex ].Count; ++tt )
                {
                    real_channel_sales[ chanDex ] += real_sales[ chanDex ][ tt ];
                    error_by_channel[chanDex] += error[chanDex][tt];
                    abs_error_by_channel[chanDex] += Math.Abs(error[chanDex][tt]);
                }

                total_abs_error += abs_error_by_channel[chanDex];
            }

            // catagory stats

            for (chanDex = 0; chanDex < numChannels; ++chanDex)
            {
                total_real_sales += real_channel_sales[chanDex];
                total_sales_error += error_by_channel[chanDex];
            }

            #endregion

            #region update sales and segment tables with user info
            // update sales table

            // global first
            DataRow chanRow = salesTable.Rows.Find(Database.AllID);
            chanRow["real_sales"] = total_real_sales;
            chanRow["sim_sales"] = total_real_sales + total_sales_error;

            if (total_real_sales > 0)
            {
                chanRow["percent_error"] = 100 * total_sales_error / total_real_sales;
                chanRow["volume_mape"] = 100 * total_abs_error / total_real_sales;
            }
            else
            {
                chanRow["percent_error"] = 0;
                chanRow["volume_mape"] = 0;
            }

            chanDex = 0;
            foreach (MrktSimDBSchema.channelRow channel in theDb.Data.channel.Select("channel_id <> " + Database.AllID, "channel_id"))
            {
                chanRow = salesTable.Rows.Find(channel.channel_id);
                chanRow["real_sales"] = real_channel_sales[chanDex];
                chanRow["sim_sales"] = real_channel_sales[chanDex] + error_by_channel[chanDex];

                if (real_channel_sales[chanDex] > 0)
                {
                    chanRow["percent_error"] = 100 * error_by_channel[chanDex] / real_channel_sales[chanDex]; ;
                    chanRow["volume_mape"] = 100 * abs_error_by_channel[chanDex] / real_channel_sales[chanDex]; ;
                }
                else
                {
                    chanRow["percent_error"] = 0;
                    chanRow["volume_mape"] = 0;
                }

                ++chanDex;

            }
            #endregion

            #region conmpute population scaling


            // Here is the calculation

            // we want to increase the model size of each segment BUT we want to presereve the overall percentage to the model pop.
            // if we sum over segements we may get more or less then the total population size.
            // 

            // compute scaling of each segment
            segmentScale = new double[numSegments];
            double currentScale = 0;
            double newScale = 0;

            foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
            {
                segmentScale[segDex] = 0;

                double real_sales_by_segment = 0;
                double sim_sales_by_segment = 0;
                for( chanDex = 0; chanDex < real_sales.Count; ++chanDex )
                {
                    real_sales_by_segment += segChannelShare[segDex][chanDex] * real_channel_sales[chanDex];
                    sim_sales_by_segment += segChannelShare[segDex][chanDex] * (real_channel_sales[chanDex] + error_by_channel[chanDex]);
                }

                segmentScale[segDex] = 1;

                if (sim_sales_by_segment > 0)
                {
                    segmentScale[segDex] = real_sales_by_segment / sim_sales_by_segment;
                }

                currentScale += segment.segment_size;
                newScale += segmentScale[segDex] * segment.segment_size;
                ++segDex;
            }


            total_pop_scale = 1;
            if (currentScale > 0)
            {
                total_pop_scale = newScale / currentScale;
            }


            // We do not want total percent of all segments to change
            // the actual scaling of the segment sizes will be segmentScale/total_pop_scale
            // so that....  
            //
            // NOTE sum[] == sum over segments
            //
            // sum[] new_segment_size
            // = sum[] segment_scale * segment_size /total_pop_scale
            // = (sum[] (segment_scale * segment_size )/((sum[] segment_scale * segment_size)/{sum[] segment_size))
            // = sum[] segment_size

            DataRow catSeg = segmentTable.Rows.Find(Database.AllID);

            catSeg["delta_num_consumers"] = ((double)catSeg["num_consumers"]) * (total_pop_scale - 1);

            segDex = 0;
            foreach (DataRow seg in segmentTable.Rows)
            {
                if ((int)seg["segment_id"] == Database.AllID)
                {
                    continue;
                }

                seg["delta_num_consumers"] = ((double)seg["num_consumers"]) * ((segmentScale[segDex] / total_pop_scale) - 1);
                seg["delta_purchase_frequency"] = ((double)seg["purchase_frequency"]) * (segmentScale[segDex] - 1);
                seg["delta_ave_units"] = ((double)seg["ave_units"]) * (segmentScale[segDex] - 1);
                segDex++;
            }

            #endregion

            #region Update segment Demand Modification

            if (segmentDemandMod == null)
            {
                segmentDemandMod = new double[numSegments][];

                for (segDex = 0; segDex < numSegments; ++segDex)
                {
                    segmentDemandMod[segDex] = new double[calendar_dates.Count];
                }
            }

            // loop over dates
            DateTime prevDate = currentSimulation.start_date;

            int num = calendar_dates.Count;

            for (int ii = 0; ii < calendar_dates.Count; ++ii)
            {
                // loop over segments
                for (segDex = 0; segDex < numSegments; ++segDex)
                {
                    double segUnitsDesired = 0;
                    double segUnitsActual = 0;
                    for (int jj = 0; jj < segChannelShare[segDex].Count; ++jj)
                    {
                        segUnitsDesired += segChannelShare[segDex][jj] * (sim_sales[jj][ii] - error[jj][ii]);
                        segUnitsActual += segChannelShare[segDex][jj] * sim_sales[jj][ii];
                    }

                    // now create event for segment with this as a percentage
                    segmentDemandMod[segDex][ii] = 0;

                    // hard to modify demand when there is no purchasing
                    if (segUnitsActual > 0)
                        segmentDemandMod[segDex][ii] = 100 * ((segUnitsDesired / segUnitsActual) - 1);
                }
            }
            #endregion

            this.Cursor = Cursors.Default;
            this.methodBox.Enabled = true;

            updatePlot();
        }

        private void updatePlot()
        {
            DataCurve crv = null;

            if (calendar_dates == null || calendar_dates.Count == 0)
                return;

            this.plotControl1.ClearAllGraphs();

            plotControl1.TimeSeries = true;
            this.plotControl1.AutoScaleAxis();

            plotControl1.YAxis = "";

            if (channelViewButton.Checked)
            {
                plotControl1.Title = "Volume Error";
                plotControl1.Y2Axis = "Percent Error";

                // add total catagory volume error
                crv = new DataCurve(calendar_dates.Count);

                plotControl1.Start = calendar_dates[0];
                plotControl1.End = calendar_dates[calendar_dates.Count - 1];

                for (int dateIndex = 0; dateIndex < calendar_dates.Count; ++dateIndex)
                {
                    if (real_sales_by_date[dateIndex] > 0)
                    {
                        crv.Add(calendar_dates[dateIndex], 100 * total_error_by_date[dateIndex] / real_sales_by_date[dateIndex]);
                    }
                    else
                    {
                        crv.Add(calendar_dates[dateIndex], 0);
                    }
                }

                crv.Label = "Catagory volume error";
                crv.Units = "%";

                crv.Color = Color.Red;

                plotControl1.Data = crv;


                if (theDb.Data.channel.Count > 2)
                {
                    int channelIndex = 0;
                    DataRow[] rows = theDb.Data.channel.Select( "channel_id <> " + Database.AllID, "channel_id" );
                    foreach (MrktSimDBSchema.channelRow channel in theDb.Data.channel.Select("channel_id <> " + Database.AllID, "channel_id"))
                    {
                        crv = new DataCurve(calendar_dates.Count);

                        for (int dateIndex = 0; dateIndex < calendar_dates.Count; ++dateIndex)
                        {
                            double realSales = sim_sales[channelIndex][dateIndex] - error[channelIndex][dateIndex];
                            if (realSales > 0)
                            {
                                crv.Add(calendar_dates[dateIndex], 100 * error[channelIndex][dateIndex] / realSales);
                            }
                            else
                            {
                                crv.Add(calendar_dates[dateIndex], 0);
                            }

                        }

                        crv.Label = channel.channel_name + " volume error";
                        crv.Units = "%";
                        crv.Color = Color.Blue;
                        plotControl1.Data = crv;


                        ++channelIndex;
                        if( channelIndex >= sim_sales.Count ) {
                            break;
                        }
                    }
                }
            }

            if (segmentViewButton.Checked)
            {
                plotControl1.Title = "Demand Modification by Segment";
                plotControl1.Y2Axis = "Percent Increase";

                // now add segment demand modifications
                int segDex = 0;
                foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
                {
                    crv = new DataCurve(calendar_dates.Count);

                    for (int tt = 0; tt < calendar_dates.Count; ++tt)
                    {
                        crv.Add(calendar_dates[tt], segmentDemandMod[segDex][tt]);
                    }

                    crv.Label = segment.segment_name + " demand mod";
                    crv.Color = Color.Red;
                    crv.Units = "%";

                    plotControl1.Data = crv;

                    ++segDex;
                }

                plotControl1.Title = "Demand Modification";
            }

            plotControl1.Scale();

            plotControl1.DataChanged();

            plotControl1.Refresh();
        }

        public bool WriteData()
        {
            if (methodBox.SelectedIndex > 0)
            {
                DialogResult rslt = MessageBox.Show(this.ParentForm, "You have chosen to " + methodBox.SelectedItem.ToString() + ": do you wish to continue?",
                    "Update Model?", MessageBoxButtons.YesNo);

                if (rslt == DialogResult.No)
                {
                    return false;
                }
            }


            int segDex = 0;
            switch (methodBox.SelectedIndex)
            {
                case 1:
                    // adjust population size
                    // adjust for "all" channels
                    DataRow total = salesTable.Rows.Find(Database.AllID);

                    double simSales = (double)total["sim_sales"];

                     
                    DataRow seg = segmentTable.Rows.Find(Database.AllID);
                    currentSimulation.scenarioRow.Model_infoRow.pop_size = (int)(currentSimulation.scenarioRow.Model_infoRow.pop_size * total_pop_scale);

                    segDex = 0;
                    foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
                    {
                        seg = segmentTable.Rows.Find(segment.segment_id);

                        segment.segment_size = segmentScale[segDex] * segment.segment_size / total_pop_scale;
                        ++segDex;
                    }
                 
                    break;

                case 2:
                    // adjust segment frequency
                    segDex = 0;
                    foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
                    {
                        seg = segmentTable.Rows.Find(segment.segment_id);

                        segment.repurchase_period_frequency  *= segmentScale[segDex];
                        ++segDex;
                    }
                    break;

                case 3:
                    
                    // adjust segment units
                    // adjust segment frequency
                    segDex = 0;
                    foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
                    {
                        seg = segmentTable.Rows.Find(segment.segment_id);

                        segment.avg_max_units_purch *= segmentScale[segDex];
                        ++segDex;
                    }
                    break;

                case 4:
                    // add external events

                  
                    // need to augment schema
                    // SSN
                    // Not sure if this is the best way to do this
                    // but it will do for now
                    theDb.AddTable(theDb.Data.scenario_market_plan);
                    theDb.AddTable(theDb.Data.market_plan);
                    theDb.AddTable(theDb.Data.product_event);
                    
                    // new external factor component
                    MrktSimDBSchema.market_planRow extComp = theDb.CreateMarketPlan("Auto " + currentSimulation.name, Database.PlanType.ProdEvent);

                    // add plan to scenario
                    theDb.AddMarketPlanToScenario(currentSimulation.scenarioRow, extComp);
                    
                    // loop over dates
                    DateTime prevDate = currentSimulation.start_date;

                    for (int ii = 0; ii < calendar_dates.Count; ++ii)
                    {
                        DateTime curDate = calendar_dates[ii];

                        // loop over segments
                        segDex = 0;
                        foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id"))
                        {
                            // do not bother with nodifications less then .1 %
                            if (Math.Abs(segmentDemandMod[segDex][ii]) > 0.1)
                            {

                                MrktSimDBSchema.product_eventRow segEvent = theDb.CreateProductEvent(extComp.id, Database.AllID, Database.AllID, segment.segment_id);

                                segEvent.start_date = prevDate;
                                segEvent.end_date = curDate;

                                segEvent.demand_modification = segmentDemandMod[segDex][ii];
                            }

                            ++segDex;
                        }

                        prevDate = curDate.AddDays(1);
                    }



                    // add data to component
                    break;
            }

            return true;
        }

        private void channelViewButton_CheckedChanged(object sender, EventArgs e)
        {
            updatePlot();
        }
    }
}
