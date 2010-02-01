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
    public partial class MediaCalibration : UserControl
    {
        private DataTable mediaScale;
        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private int currentRun = -1;
        public int Run {
            set {
                currentRun = value;

                // zero out the share value
                sim_share = null;

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

        public MediaCalibration()
        {
            InitializeComponent();

            mediaScale = new DataTable("AttrPrefs");

            mediaScale.Columns.Add("market_plan_id", typeof(int));
            mediaScale.Columns.Add("product_id", typeof(int));
            mediaScale.Columns.Add("plan", typeof(string));
            mediaScale.Columns.Add("product", typeof(string));
             
            mediaScale.Columns.Add("current_scale", typeof(double));
            mediaScale.Columns.Add("step_size", typeof(double));

            this.mrktSimGrid.Table = mediaScale;

           
           // mrktSimGrid.ReadOnly = true;
            mrktSimGrid.DescriptionWindow = false;

            plotControl1.Title = "Perusasion";
            plotControl1.Y2Axis = "";

            createTableStyle();

            applyButton.Enabled = false;
        }

        private void createTableStyle()
        {
            mrktSimGrid.Clear();

            mrktSimGrid.AddTextColumn( "plan", true );
            mrktSimGrid.AddTextColumn("product",true);
            DataGridTextBoxColumn col = mrktSimGrid.AddNumericColumn( "current_scale", true );
            col.Format = "N8";

            col = mrktSimGrid.AddNumericColumn("step_size", true);
            col.Format = "N8";

            mrktSimGrid.Reset();
        }

        private void initParmBox() {

            if( planNames == null ) {
                planNames = theDb.SimulationMediaPlans( currentSimulation.id );
            }

            foreach( int prodID in planNames.Keys ) {

                Dictionary<int, string> plans = planNames[ prodID ];
                foreach( int mediaID in plans.Keys ) {

                    MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "market_plan", "parm2", "id", mediaID );

                    if( simParm != null ) {

                        MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id( prodID );

                        DataRow mediaPref = mediaScale.NewRow();

                        mediaPref[ "current_scale" ] = simParm.aValue;

                        mediaPref[ "market_plan_id" ] = mediaID; // 
                        mediaPref[ "product_id" ] = prodID;
                        mediaPref[ "product" ] = prod.product_name;
                        mediaPref[ "plan" ] = plans[ mediaID ];

                        mediaPref[ "step_size" ] = 0;

                        mediaScale.Rows.Add( mediaPref );
                    }
                }

                mediaScale.AcceptChanges();
            }
        }
          
          
    

        // for charting purposes
        Dictionary<int, Dictionary<int, string>> planNames = null;

        List<List<double>> sim_share = null;
        List<List<double>> real_share = null;
        List<double> weights = null;
       

        private void solveButton_Click( object sender, EventArgs e ) {

            const double eps = 0.00001;

            if( Run == -1 )
                return;

            int run_id = Run;

            if( planNames == null ) {
                planNames = theDb.SimulationMediaPlans(currentSimulation.id );
            }

            SortedDictionary<int, SortedDictionary<int, List<double>>> mediaPersuasion = null;
            List<List<List<double>>> attributes = null;
            SortedDictionary<int, List<double>> prodPersuasion = null;
            List<List<double>> totalVals = null;

            // find plans that we have parameters for
            List<int> plans = new List<int>();

            foreach( MrktSimDBSchema.scenario_parameterRow simParm in theDb.Data.scenario_parameter.Select() ) {

                // check if this is a media parameter
                MrktSimDBSchema.model_parameterRow modelParm = simParm.model_parameterRow;

                if( modelParm.table_name == "market_plan" && modelParm.col_name == "parm2" ) {


                  
                    Database.PlanType planType;

                    theDb.GetMarketPlanInfo( theDb.Simulation.scenario_id, modelParm.row_id, out  planType );

                    if( planType == Database.PlanType.Media ) {
                        plans.Add( modelParm.row_id );
                    }
                }
            }

            theDb.MediaEGRP( Run, plans, out mediaPersuasion, out prodPersuasion, out dates );
            List<int> leafs = null;
            theDb.GetMediaData(run_id, mediaPersuasion, prodPersuasion, out sim_share, out real_share, out weights, out attributes, out totalVals, out leafs);

            Solver.Saturation fcn = theDb.InitSaturationFcn();

            List<double> totalStep = null;
            if( fcn != null ) {
                totalStep = Solver.SolveWithSaturation( fcn, sim_share, real_share, weights, totalVals, attributes );
            }
            else {
                totalStep = Solver.Solve( sim_share, real_share, weights, attributes );
            }


            // ensure that we do not overstep
            int mediaIndex = 0;
            foreach( int prodId in mediaPersuasion.Keys )
            {
                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id( prodId );
                SortedDictionary<int, List<double>> prodPlans = mediaPersuasion[prodId];

                foreach( int mediaID in prodPlans.Keys )
                {
                    double aVal = totalStep[mediaIndex];

                    MrktSimDBSchema.scenario_parameterRow simParm = this.theDb.GetParm( theDb.Id, "market_plan", "parm2", "id", mediaID );

                    if( simParm != null )
                    {
                        if( simParm.aValue > 0 )
                        {
                            if( aVal < 0 && aVal < eps - simParm.aValue )
                            {
                                // apply scaling to values

                                totalStep[mediaIndex] = (eps - simParm.aValue);
                            }
                        }

                    }

                    mediaIndex++;
                }
            }

            mediaScale.Clear();

            mediaIndex = 0;
            foreach( int prodId in mediaPersuasion.Keys ) {

                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id( prodId );

                SortedDictionary<int, List<double>> prodPlans = mediaPersuasion[ prodId ];

                foreach( int mediaID in prodPlans.Keys ) {

                    MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "market_plan", "parm2", "id", mediaID );

                    if( simParm != null ) {

                        // make sure step does not cause persuasion to go negative

                        DataRow mediaPref = mediaScale.NewRow();

                        mediaPref[ "current_scale" ] = simParm.aValue;
                        mediaPref[ "market_plan_id" ] = mediaID; // 
                        mediaPref[ "product_id" ] = prodId;
                        mediaPref[ "product" ] = prod.product_name;
                        mediaPref[ "plan" ] = planNames[ prodId ][ mediaID ];

                        mediaPref[ "step_size" ] = totalStep[ mediaIndex ];

                        mediaScale.Rows.Add( mediaPref );
                    }

                    mediaIndex++;
                }
            }
          
            mediaScale.AcceptChanges();


            if( !appliedOnce ) {
                applyButton.Enabled = true;
            }
        }

        bool appliedOnce = false;
        private void applyButton_Click(object sender, EventArgs e)
        {
           // string error = null;

            foreach( DataRow pref in mediaScale.Rows ) {
                int product_id = (int)pref[ "product_id" ];
                int plan_id = (int)pref[ "market_plan_id" ];

                double aVal = (double)pref[ "step_size" ];

                theDb.UpdateParm("market_plan", "parm2", "id", plan_id, aVal );                
            }

            mediaScale.Clear();
            appliedOnce = true;

            applyButton.Enabled = false;

        }


        Dictionary<int, List<double>> productPersuasion = null;
        Dictionary<int, List<double>> productError = null;
        List<DateTime> dates = null;

        private void updateMediaPlot() {

            if (dates == null || dates.Count == 0)
                return;

            Dictionary<int, List<double>> plotData = null;

            if( persuasionOn.Checked ) {
                plotData = productPersuasion;
                plotControl1.Title = "Estimated Persuasion v Time";
                plotControl1.YAxis = "Persuasion";
            }
            else {
                plotData = productError;
                plotControl1.Title = "Persuasion Error v Time";
                plotControl1.YAxis = "Error";
            }

            if( plotData == null )
                return;

            DataCurve crv = null;


            double maxPersuasion = double.MinValue;
            double minPersuasion = double.MaxValue;

            this.plotControl1.ClearAllGraphs();

            plotControl1.TimeSeries = true;
            this.plotControl1.AutoScaleAxis();

         

         
            plotControl1.Y2Axis = "";
            plotControl1.XAxis = "Date";
            plotControl1.ScatterPlot = false;

            foreach( int prodId in plotData.Keys ) {

                List<double> series = plotData[ prodId ];
                crv = new DataCurve( dates.Count );

                for( int tt = 0; tt < dates.Count; ++tt ) {

                    if( maxPersuasion < series[ tt ] ) {
                        maxPersuasion = series[ tt ];
                    }

                    if( minPersuasion > series[ tt ] ) {
                        minPersuasion = series[ tt ];

                    }

                    crv.Add( dates[tt], series[ tt ] );
                }

                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id(prodId);

                crv.Label = prod.product_name;
                crv.Units = "#";
                crv.Color = Color.Red;

                plotControl1.Max = maxPersuasion + 0.01;
                plotControl1.Min = minPersuasion - 0.01;

                plotControl1.Data = crv;

                plotControl1.DataChanged();
            }


            plotControl1.Start = dates[0];
            plotControl1.End = dates[dates.Count - 1];


            plotControl1.MakeUnique();

            plotControl1.Refresh();          
        }

        private void refreshBut_Click( object sender, EventArgs e ) {

            if( Run == -1 )
                return;

            SortedDictionary<int, SortedDictionary<int, List<double>>> mediaPersuasion = null;
            List<List<List<double>>> attributes = null;

            SortedDictionary<int, List<double>> prodPersuasion = null;
            List<List<double>> totalVals = null;

            theDb.MediaEGRP(Run, null,  out mediaPersuasion, out prodPersuasion, out dates);

            List<int> leafs = null;
            theDb.GetMediaData( Run, mediaPersuasion, prodPersuasion,
                out sim_share, out real_share, out weights, out attributes, out totalVals, out leafs);

            productError = new Dictionary<int, List<double>>();

            productPersuasion = new Dictionary<int, List<double>>();

            int numChannels = theDb.Data.channel.Select( "channel_id <> " + Database.AllID ).Length;

            // for each product
            int prodIndex = 0;
            foreach( int prodId in leafs ) {
           
                List<double> errorSeries = new List<double>();

                productError[ prodId ] = errorSeries;

                // fill out date-channel arrays
                for( int tt = 0; tt < dates.Count; ++tt ) {
                    errorSeries.Add( 0 );
                    if( mediaPersuasion.ContainsKey( prodId ) )
                    {
                        SortedDictionary<int, List<double>> prodPlans = mediaPersuasion[prodId];

                        foreach( List<double> grps in prodPlans.Values )
                        {
                            double val = grps[tt] * (sim_share[tt][prodIndex] - real_share[tt][prodIndex]);
                            errorSeries[tt] += val;
                        }
                    }
                }
                prodIndex++;
            }

          
            foreach( int prodId in mediaPersuasion.Keys ) {
                SortedDictionary<int, List<double>> planEGRPS = mediaPersuasion[ prodId ];

                List<double> totalSeries = new List<double>();


                foreach( int plan_id in planEGRPS.Keys ) {

                    List<double> series = planEGRPS[ plan_id ];


                    for( int tt = 0; tt < series.Count; ++tt ) {

                        if( tt >= totalSeries.Count ) {
                            totalSeries.Add( 0 );
                        }

                        totalSeries[ tt ] += series[ tt ];
                    }
                }

                productPersuasion[ prodId ] = totalSeries;
            }
         

            updateMediaPlot();
        }

        private void persuasionOn_CheckedChanged( object sender, EventArgs e ) {
            updateMediaPlot();
        }

    }
}
