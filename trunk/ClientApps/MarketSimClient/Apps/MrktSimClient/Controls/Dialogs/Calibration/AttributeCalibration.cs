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
    public partial class AttributeCalibration : UserControl
    {
        private DataTable attributePrefs;
        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private int currentRun = -1;
        public int Run {
            set {
                currentRun = value;
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

                //initParamBox();
            }
        }

        public AttributeCalibration()
        {
            InitializeComponent();

            attributePrefs = new DataTable("AttrPrefs");

            attributePrefs.Columns.Add("product_attribute_id", typeof(int));
            attributePrefs.Columns.Add("segment_id", typeof(int));
            attributePrefs.Columns.Add("segment", typeof(string));
            attributePrefs.Columns.Add("attribute", typeof(string));
            attributePrefs.Columns.Add("date", typeof(DateTime));
             
            attributePrefs.Columns.Add("current_pref_value", typeof(double));
            attributePrefs.Columns.Add("step_size", typeof(double));

            this.mrktSimGrid.Table = attributePrefs;
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
            mrktSimGrid.AddTextColumn("attribute", true);
            mrktSimGrid.AddDateColumn("date", true);

            DataGridTextBoxColumn col = mrktSimGrid.AddNumericColumn( "current_pref_value", true );
            col.Format = "N8";
            col = mrktSimGrid.AddNumericColumn("step_size", true);
            col.Format = "N8";

            mrktSimGrid.Reset();
        }


        private void initParamBox() {

            attributePrefs.Clear();

            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {

             
                foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                        theDb.Data.consumer_preference.Select( "segment_id = " + seg.segment_id, "product_attribute_id", DataViewRowState.CurrentRows ) ) {

                    MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id );

                    if( simParm != null ) {
                        DataRow pref = attributePrefs.NewRow();

                        pref[ "product_attribute_id" ] = dbPref.product_attributeRow.product_attribute_id;
                        pref[ "segment_id" ] = seg.segment_id;
                        pref[ "segment" ] = seg.segment_name;
                        pref[ "date" ] = dbPref.start_date;
                        pref[ "attribute" ] = dbPref.product_attributeRow.product_attribute_name;
                        pref[ "current_pref_value" ] = simParm.aValue;
                        pref[ "step_size" ] = 0;

                        attributePrefs.Rows.Add( pref );
                    }
                }
            }

            attributePrefs.AcceptChanges();
        }


        private void solveButton_Click( object sender, EventArgs e ) {

            if( Run == -1 )
                return;

            int run_id = Run;
            List<List<double>> totalStep = new List<List<double>>();
            List<List<List<double>>> sim_share = null;
            List<List<double>> real_share = null;
            List<List<double>> weights = null;
            List<List<List<double>>> attributes = new List<List<List<double>>>();
           

            theDb.GetLeafShareData(run_id, out sim_share, out real_share, out weights);


            // check which attributes are active for update
            
            List<int> attrsAllowed = new List<int>();

            foreach( MrktSimDBSchema.product_attributeRow attr in
                theDb.Data.product_attribute.Select(
                "",
                "product_attribute_id",
                DataViewRowState.CurrentRows ) ) {

                bool allow = false;

                foreach( MrktSimDBSchema.segmentRow seg in
                    theDb.Data.segment.Select(
                    "segment_id <> " + Database.AllID,
                    "segment_id",
                    DataViewRowState.CurrentRows ) ) {

                    foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                            theDb.Data.consumer_preference.Select(
                            "segment_id = " + seg.segment_id + " AND product_attribute_id = " + attr.product_attribute_id,
                            "",
                            DataViewRowState.CurrentRows ) ) {

                        MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( theDb.Id, 
                            "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id );

                        if( simParm != null ) {
                            allow = true;
                            break;
                        }
                    }

                    if( allow ) {
                        break;
                    }
                }

                if( allow ) {
                    attrsAllowed.Add( attr.product_attribute_id );
                }

            }

            theDb.ProdAttrVals(run_id, attrsAllowed, out attributes);

            // we iterate over each segment seperately
            int segDex = 0;
        
            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {
                totalStep.Add(Solver.Solve(sim_share[segDex], real_share, weights[segDex], attributes));
                segDex++;
            }

            attributePrefs.Clear();

            // loop over segments

            segDex = 0;

          
            int simID = theDb.Id;

            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {

                int attrDex = 0;

                foreach( MrktSimDBSchema.product_attributeRow attr in theDb.Data.product_attribute.Select( "", "product_attribute_id", DataViewRowState.CurrentRows ) ) {

                    foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                            theDb.Data.consumer_preference.Select( "segment_id = " + seg.segment_id + " AND product_attribute_id = " + attr.product_attribute_id,
                            "", DataViewRowState.CurrentRows ) ) {
                        MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( simID, "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id );

                        if( simParm != null ) {
                            DataRow pref = attributePrefs.NewRow();

                            int index = attrsAllowed.IndexOf( dbPref.product_attributeRow.product_attribute_id );


                            if( index < 0 ) {
                                continue;
                            }

                            pref[ "product_attribute_id" ] = dbPref.product_attributeRow.product_attribute_id;
                            pref[ "segment_id" ] = seg.segment_id;
                            pref[ "segment" ] = seg.segment_name;
                            pref[ "date" ] = dbPref.start_date;
                            pref[ "attribute" ] = dbPref.product_attributeRow.product_attribute_name;
                            pref[ "current_pref_value" ] = simParm.aValue;
                            pref[ "step_size" ] = totalStep[ segDex ][ index ];

                            attributePrefs.Rows.Add( pref );
                        }
                    }

                    attrDex++;

                }
                segDex++;
            }

            attributePrefs.AcceptChanges();

            if(!appliedOnce ) {
                applyButton.Enabled = true;
            }
        }

        private bool appliedOnce = false;
        private void applyButton_Click(object sender, EventArgs e)
        {
           // string error = null;

            foreach (DataRow pref in attributePrefs.Rows)
            {
                int attr_id = (int) pref["product_attribute_id"];
                int seg_id = (int)pref["segment_id"];
                double aVal = (double)pref[ "step_size" ];

                string query = "segment_id = " + seg_id + " AND product_attribute_id = " + attr_id;
                foreach (MrktSimDBSchema.consumer_preferenceRow dbPref in theDb.Data.consumer_preference.Select(query))
                {

                    theDb.UpdateParm( "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id, aVal );

                    // also update post use preference
                    theDb.UpdateParm("consumer_preference", "post_preference_value", "record_id", dbPref.record_id, aVal );
                }
            }
            
            attributePrefs.Clear();

            appliedOnce = true;

            applyButton.Enabled = false;
        }


   
        double[] prodError = null;
        double[] prodUtil = null;

        private void updatePlot() {
            DataCurve crv = null;

            double maxUtil = double.MinValue;
            double minUtil = double.MaxValue;

            double maxError = double.MinValue;
            double minError = double.MaxValue;

            this.plotControl1.ClearAllGraphs();

            plotControl1.TimeSeries = false;
            this.plotControl1.AutoScaleAxis();

            plotControl1.YAxis = "";

            plotControl1.Title = "Excess Share";
            plotControl1.Y2Axis = "Error";
            plotControl1.XAxis = "Product Utility";
            plotControl1.ScatterPlot = true;

          

            int prod_index = 0;
            foreach( MrktSimDBSchema.productRow prod in theDb.Data.product.Select( "product_id <> " + Database.AllID + " AND brand_id = 1", "product_id" ) ) {

                if( maxUtil < prodUtil[ prod_index ] ) {
                    maxUtil = prodUtil[ prod_index ];
                }

                if( minUtil > prodUtil[ prod_index ] ) {
                    minUtil = prodUtil[ prod_index ];
                }

                if( maxError < prodError[ prod_index ] ) {
                    maxError = prodError[ prod_index ];
                }

                if( minError > prodError[ prod_index ] ) {
                    minError = prodError[ prod_index ];

                }

                crv = new DataCurve( 1);

                crv.Add( prodUtil[prod_index], prodError[prod_index] );
               
                crv.Label = prod.product_name;
                crv.Units = "%";
                crv.Color = Color.Red;

                plotControl1.Data = crv;

                plotControl1.DataChanged();


                prod_index++;
            }


            plotControl1.MinX = minUtil;
            plotControl1.MaxX = maxUtil;

            plotControl1.Max = maxError + 0.1;
            plotControl1.Min = minError - 0.1;

            plotControl1.MinX = minUtil - 0.1;
            plotControl1.MaxX = maxUtil + 0.1;

            plotControl1.Scale();

            plotControl1.Refresh();
          
        }

        private void refreshBut_Click( object sender, EventArgs e ) {

            if( Run == -1 )
                return;

            int simID = (int)theDb.Data.sim_queue.FindByrun_id( Run )[ "sim_id" ];

            double[][] sim_share = null;
            double[][] real_share = null;
            double[][] attrs = null;

            theDb.AttrVals( out attrs );
            int numAttrs = attrs.Length;

            if( numAttrs == 0 )
                return;

            int numProducts = attrs[ 0 ].Length;

            if( prodError == null ) {
                prodError = new double[ numProducts ];
                prodUtil = new double[ numProducts ];
            }

            int numSegments = theDb.Data.segment.Select( "segment_id <> " + Database.AllID ).Length;

            int run_id = Run;

            theDb.ShareInfo( run_id, out sim_share, out real_share );

            // we iterate over each segment seperately
            int segDex = 0;
            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {

                for( int kk = 0; kk < numProducts; ++kk ) {
                    prodError[ kk ] = (sim_share[ segDex ][ kk ] - real_share[ segDex ][ kk ]) * seg.segment_size;
                    prodUtil[ kk ] = 0.0;


                    int attr_index = 0;
                    foreach( MrktSimDBSchema.product_attributeRow attr in theDb.Data.product_attribute.Select( "", "product_attribute_id", DataViewRowState.CurrentRows ) ) {
                        double avePref = 0;
                        double numVals = 0;
                        foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                            theDb.Data.consumer_preference.Select( "segment_id = " + seg.segment_id + " AND product_attribute_id = " + attr.product_attribute_id,
                            "product_attribute_id", DataViewRowState.CurrentRows ) ) {

                             MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm(simID, "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id);

                             if( simParm != null ) {
                                 avePref += attrs[ attr_index ][ kk ] * simParm.aValue * seg.segment_size / 100.0;
                                 numVals += 1.0;
                             }

                        }

                        if( numVals > 0 ) {
                            avePref /= numVals;
                        }

                        prodUtil[ kk ] += attrs[ attr_index ][ kk ] * avePref * seg.segment_size / 100.0;
                        attr_index++;
                    }
                }


                segDex++;
            }


            updatePlot();
        }

    }
}
