using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MrktSimDb.Metrics;
using Utilities.Graphing;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class CalibrationEditor : Form
    {

        #region fields
        private MrktSimDBSchema.simulationRow currentSimulation = null;
        CallibrationDb theDb;
        MetricMan metricMan;
        int numSKUs;
        #endregion


        public CallibrationDb Db
        {
            get
            {
                return theDb;
            }

            set
            {
                theDb = value;

                 currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                if (currentSimulation == null)
                    return;

                metricMan = new MetricMan( theDb );
                this.parameterControl1.Db = theDb;
                this.variableControl1.Db = theDb;
                this.simulationSetUpControl1.Db = theDb;
                this.calibrationSetUp1.Db = theDb;
                this.volumeCalibration1.Db = theDb;
                this.attributeCalibration1.Db = theDb;
                this.priceCalibration1.Db = theDb;
                this.mediaCalibration1.Db = theDb;

                // dialogue
                this.Text = "Editing Calibration: " + currentSimulation.name;

                if( theDb.ReadOnly ) {
                    this.Text += " (Read Only)";
                }

                numSKUs = theDb.Data.product.Select("brand_id = 1").Length;
                errorPlot.Y2Axis = "RMS Error";

                this.runBox.DataSource = currentSimulation.Getsim_queueRows();
                this.runBox.DisplayMember = "name";
                this.runBox.ValueMember = "run_id";


                MrktSimDBSchema.sim_queueRow recentSim = null;
                foreach( MrktSimDBSchema.sim_queueRow sim in currentSimulation.Getsim_queueRows() ) {
                    // all this because we allow NULL for run_time
                    // need to add script to change that to NOT NULL with 'now' as default
                    DateTime simDate;

                    try {
                        simDate = sim.run_time;
                    }
                    catch( Exception ) {
                        continue;
                    }

                    if( recentSim == null ) {
                        recentSim = sim;
                    }
                    else if( recentSim.run_time < simDate ) {
                        recentSim = sim;
                    }
                }

                // set most recent as the current
                if( recentSim != null ) {
                    this.runBox.SelectedItem = recentSim;
                }

                if( theDb.ReadOnly ) {
                    this.acceptButton.Enabled = false;
                }
            }
        }

        public CalibrationEditor()
        {
            InitializeComponent();

            errorPlot.Send.Visible = false;
            errorPlot.AutoScaleAxis();

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if( theDb.SimulationRunning()) {
                DialogResult res = MessageBox.Show( this, "This calibration appears to be running - are you sure you want to save?", "Save Calibration?", MessageBoxButtons.YesNo );

                if( res == DialogResult.No ) {
                    return;
                }
            }

            if (calibrationSetUp1.CheckExpressions)
            {
                string parserError = this.variableControl1.ParserTest();

                if (parserError != null)
                {
                    MessageBox.Show("Error parsing expressions: " + parserError);

                    return;
                }
            }

            foreach (Metric metric in MetricMan.SimSummaryMetrics)
            {
                theDb.CreateScenarioMetric(currentSimulation, metric.Token);
            }

            foreach (Metric metric in MetricMan.CalibrationMetrics)
            {
                 theDb.CreateScenarioMetric(currentSimulation, metric.Token);
            }

            
            simulationSetUpControl1.WriteData();
            calibrationSetUp1.WriteData();

            if (!this.volumeCalibration1.WriteData())
            {
                return;
            }

            this.parameterControl1.SuspendLayout();

            theDb.Update();

            this.parameterControl1.ResumeLayout();

            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == plotPage)
            {
                updatePlot();
            }
            else if (tabControl1.SelectedTab == this.setUpPage)
            {
                calibrationSetUp1.Refresh();
            }
        }

        private void updatePlot()
        {
            errorPlot.ClearAllGraphs();

            // ok ok I know we needed a better interface for the values
            Value runNum = MetricMan.GetValue( "RunNum" );
            Value rmsError = MetricMan.GetValue( "SqrError" );
            Value mape = MetricMan.GetValue( "MAPE" );

            DataCurve rmsCrv = new DataCurve(this.currentSimulation.Getsim_queueRows().Length);
            DataCurve mapeCrv = new DataCurve(this.currentSimulation.Getsim_queueRows().Length);

            // run thru all runs
            foreach (MrktSimDBSchema.sim_queueRow run in currentSimulation.Getsim_queueRows())
            {
                runNum.Run = run.run_id;
                rmsError.Run = run.run_id;
                mape.Run = run.run_id;

                runNum.Product = Database.AllID;
                rmsError.Product = Database.AllID;
                mape.Product = Database.AllID;

                // add to curve

                rmsCrv.Add(metricMan.Evaluate(runNum), metricMan.Evaluate(rmsError));
                mapeCrv.Add(metricMan.Evaluate(runNum), metricMan.Evaluate(mape));
            }


            rmsCrv.Label = rmsError.Descr; ;
            rmsCrv.Units = "%";

            mapeCrv.Label = mape.Descr;
            mapeCrv.Units = "MAPE";

            
            rmsCrv.Color = Color.Blue;
            mapeCrv.Color = Color.Green;

            errorPlot.Data = mapeCrv;
            errorPlot.Data = rmsCrv;

            errorPlot.MinX = 0;
            errorPlot.MaxX = mapeCrv.X.Length + 1;

           // errorPlot.Scale();

            errorPlot.DataChanged();

            errorPlot.Refresh();
        }

        private void runBox_SelectedIndexChanged( object sender, EventArgs e ) {

            if( runBox.SelectedItem == null )
                return;

            int run_id = ((MrktSimDBSchema.sim_queueRow)runBox.SelectedItem).run_id;
            // tell controls that the current run has changed
            this.volumeCalibration1.Run = run_id;

            this.attributeCalibration1.Run = run_id;

            this.priceCalibration1.Run = run_id;
            this.mediaCalibration1.Run = run_id;

        }
    }
}