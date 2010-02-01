using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MathUtility;
using Utilities.Graphing;

namespace MrktSimClient.Controls.Dialogs.Calibration
{
    public partial class PriceCalibration : UserControl
    {
        private DataTable priceUtilities;
        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private int currentRun = -1;
        public int Run {
            set {
                currentRun = value;
                sim_share = null;
                dates = null;
            }

            get {
                return currentRun;
            }
        }

        private CallibrationDb theDb;
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

                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                //initParmBox();
            }
        }

        public PriceCalibration()
        {
            InitializeComponent();

            priceUtilities = new DataTable("AttrPrefs");

            priceUtilities.Columns.Add("segment_id", typeof(int));
            priceUtilities.Columns.Add("segment", typeof(string));

            priceUtilities.Columns.Add("current_price_disutility", typeof(double));
            priceUtilities.Columns.Add("step_size", typeof(double));
            priceUtilities.Columns.Add("suggested_price_disutility", typeof(double));

            this.mrktSimGrid.Table = priceUtilities;
           // mrktSimGrid.ReadOnly = true;
            mrktSimGrid.DescriptionWindow = false;

            plotControl1.Title = "Share  Error  vs Utility";
            plotControl1.Y2Axis = "";

            createTableStyle();

            applyButton.Enabled = false;
        }

        private void createTableStyle()
        {
            mrktSimGrid.Clear();

            mrktSimGrid.AddTextColumn("segment",true);
            DataGridTextBoxColumn col = mrktSimGrid.AddNumericColumn( "current_price_disutility", true );
            col.Format = "N8";

            col = mrktSimGrid.AddNumericColumn( "step_size", true );
            col.Format = "N8";

            mrktSimGrid.Reset();
        }


        // for charting purposes
        List<List<List<double>>> sim_share = null; //new List<List<List<double>>>();
        List<List<double>> real_share = null;
        List<List<double>> weights = null;
        List<List<List<double>>> prices = null;

        private void initParmBox() {
            priceUtilities.Clear();

         
            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {

                MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "segment", "price_disutility", "segment_id", seg.segment_id );

                if( simParm != null ) {
                    DataRow pref = priceUtilities.NewRow();

                    pref[ "segment_id" ] = seg.segment_id;
                    pref[ "segment" ] = seg.segment_name;
                    pref[ "current_price_disutility" ] = simParm.aValue;
                    pref[ "step_size" ] = 0;

                    priceUtilities.Rows.Add( pref );
                }
            }

            priceUtilities.AcceptChanges();
        }

        private void solveButton_Click( object sender, EventArgs e ) {
            if( Run == -1 )
                return;

            int run_id = Run;
            List<List<double>> totalStep = new List<List<double>>();

            if( sim_share == null ) {

                theDb.GetLeafShareData( run_id, out sim_share, out real_share, out weights );
                theDb.RelPriceVals( run_id, out prices );
            }

            // we iterate over each segment seperately
            int segDex = 0;
            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {

                totalStep.Add(Solver.Solve(sim_share[segDex], real_share, weights[segDex], prices));
                segDex++;
            }


            priceUtilities.Clear();

            // loop over segments

            segDex = 0;

            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {
                MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "segment", "price_disutility", "segment_id", seg.segment_id );

                   if( simParm != null ) {
                       DataRow pref = priceUtilities.NewRow();


                       pref[ "segment_id" ] = seg.segment_id;
                       pref[ "segment" ] = seg.segment_name;
                       pref[ "current_price_disutility" ] = simParm.aValue;
                       pref[ "step_size" ] = totalStep[ segDex ][ 0 ];

                       priceUtilities.Rows.Add( pref );
                   }
                segDex++;
            }

            priceUtilities.AcceptChanges();


            if( !appliedOnce ) {
                applyButton.Enabled = true;
            }

        }

        private bool appliedOnce = false;
        private void applyButton_Click(object sender, EventArgs e)
        {
            string error = null;

            foreach (DataRow pref in priceUtilities.Rows)
            {
                int seg_id = (int)pref["segment_id"];
                double aVal = (double)pref["step_size"];

                string query = "segment_id = " + seg_id;
                foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select(query))
                {

                    if( !theDb.UpdateParm("segment", "price_disutility", "segment_id", seg.segment_id, aVal ) ) {
                        if( error == null ) {
                            error = "Price sensitivity parameter not available for \n\r";
                        }

                        error += seg.segment_name + "\n\r";
                    }
                }
            }

            if( error != null ) {
                MessageBox.Show( error );
            }


            priceUtilities.Clear();
            appliedOnce = true;

            applyButton.Enabled = false;
        }


        Dictionary<int, List<double>> priceError = null;
        List<DateTime> dates = null;

        private void updatePlot() {

            if( priceError == null ) {
                return;
            }

            if( dates == null || dates.Count == 0 ) {
                return;
            }

            double maxPersuasion = double.MinValue;
            double minPersuasion = double.MaxValue;

            this.plotControl1.ClearAllGraphs();

            plotControl1.TimeSeries = true;
            this.plotControl1.AutoScaleAxis();

            plotControl1.YAxis = "RMS Error";

            plotControl1.Title = "Price Error v Time";
            plotControl1.Y2Axis = "";
            plotControl1.XAxis = "Date";
            plotControl1.ScatterPlot = false;

            foreach( int segId in priceError.Keys ) {
                List<double> series = priceError[ segId ];

                DataCurve crv = new DataCurve( dates.Count );


             


                for( int tt = 0; tt < dates.Count; ++tt ) {

                    if( maxPersuasion < series[ tt ] ) {
                        maxPersuasion = series[ tt ];
                    }

                    if( minPersuasion > series[ tt ] ) {
                        minPersuasion = series[ tt ];

                    }

                    crv.Add( dates[ tt ], series[ tt ] );
                }

                MrktSimDBSchema.segmentRow seg = theDb.Data.segment.FindBysegment_id( segId );
                crv.Label = seg.segment_name;
                crv.Units = "#";
                crv.Color = Color.Red;

                plotControl1.Data = crv;

                plotControl1.DataChanged();
            }

            plotControl1.MakeUnique();

            plotControl1.Start = dates[ 0 ];
            plotControl1.End = dates[ dates.Count - 1 ];

            plotControl1.Refresh();

        }

        private void refreshBut_Click( object sender, EventArgs e ) {

            if( Run == -1 )
                return;

            if( sim_share == null ) {

                theDb.GetLeafShareData( Run, out sim_share, out real_share, out weights );
                theDb.RelPriceVals( Run, out prices );
            }

            if( dates == null ) {
                theDb.Dates( Run, out dates );
            }

            priceError = new Dictionary<int, List<double>>();
           
            int numChannels = theDb.Data.channel.Select( "channel_id <> " + Database.AllID ).Length;
            int numSegments = theDb.Data.segment.Select( "segment_id <> " + Database.AllID ).Length;

          
            // need to compute channel weights first

            List<List<double>> chanWght = new List<List<double>>();
             int segDex = 0;
             foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id" ) ) {
                 chanWght.Add( new List<double>() );

                 int channelDateIndex = 0;
                 for( int chanDex = 0; chanDex < numChannels; ++chanDex ) {
                     chanWght[ segDex ].Add( 0 );

                     // compute price error for each time
                     for( int tt = 0; tt < dates.Count; ++tt ) {
                         chanWght[segDex][chanDex] += weights[ segDex ][ channelDateIndex ];
                         channelDateIndex++;
                     }
                 }
                 segDex++;
             }

            segDex = 0;
            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id") ) {

                priceError[ seg.segment_id ] = new List<double>();

                for( int ii = 0; ii < dates.Count; ++ii ) {
                    priceError[seg.segment_id].Add( 0 );
                }

                int channelDateIndex = 0;
                for( int chanDex = 0; chanDex < numChannels; ++chanDex ) {

                    double totalSimShare = 0.0;
                    double totalRealShare = 0.0;
                    // compute price error for each time
                    for( int tt = 0; tt < dates.Count; ++tt ) {
                        for( int prodDex = 0; prodDex < prices[ channelDateIndex ].Count; ++prodDex ) {

                            double val = prices[ channelDateIndex ][ prodDex ][ 0 ] * chanWght[ segDex ][ chanDex ] *
                                (sim_share[ segDex ][ channelDateIndex ][ prodDex ] - real_share[ channelDateIndex ][ prodDex ]);

                              priceError[ seg.segment_id ][ tt ] += val;
                            totalRealShare += real_share[ channelDateIndex ][ prodDex ];
                            totalSimShare += sim_share[ segDex ][ channelDateIndex ][ prodDex ] - real_share[ channelDateIndex ][ prodDex ];

                           
                        }

                        channelDateIndex++;
                    }
                }

                segDex++;
            }

            updatePlot();
        }

    }
}
