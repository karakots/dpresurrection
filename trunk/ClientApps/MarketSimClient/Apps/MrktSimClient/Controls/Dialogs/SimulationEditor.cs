using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using MrktSimDb;
using MarketSimUtilities;
using MrktSimDb.Metrics;


namespace MrktSimClient.Controls.Dialogs
{
    public partial class SimulationEditor : Form
    {
        private static System.Random rand = new Random(123456);

     
        private TreeNode summaryNode;
        private TreeNode calibrationNode;

        #region fields
        private MrktSimDBSchema.simulationRow currentSimulation = null;
        SimulationDb theDb;
        #endregion

        public SimulationEditor()
        {
            InitializeComponent();

           summaryNode = metricTree.Nodes.Add("Summary", "Summary Metrics");
           calibrationNode = metricTree.Nodes.Add("Calibration", "Calibration Metrics");

            foreach (Metric metric in MetricMan.SimSummaryMetrics)
            {
                TreeNode node = summaryNode.Nodes.Add(metric.Token, metric.ToString());
                node.Tag = metric;
            }

            foreach (Metric metric in MetricMan.CalibrationMetrics)
            {
                TreeNode node = calibrationNode.Nodes.Add(metric.Token, metric.ToString());
                node.Tag = metric;
            }

            metricTree.ExpandAll();
        }

        public SimulationDb Db
        {
            set
            {
                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                if (currentSimulation == null)
                    return;

                this.parameterControl1.Db = theDb;
                this.variableControl1.Db = theDb;
                this.simulationSetUpControl1.Db = theDb;
              
                initializeMetricSelectBox();

                // seed grid
                seedGrid.Table = theDb.Data.scenario_simseed;
                seedGrid.RowFilter = "id = " + currentSimulation.id;
                seedGrid.DescriptionWindow = false;
                seedGrid.RowFilter = "sim_id = " + currentSimulation.id;

                // dialogue
                this.Text = "Editing " + currentSimulation.name;

                if (currentSimulation.sim_num >= 0)
                {
                    this.seedGrid.EnabledGrid = false;

                    this.metricTree.Enabled = false;
                }

                if (currentSimulation.delete_std_results)
                {
                    deleteResults.Checked = true;
                }
                else
                {
                    deleteResults.Checked = false;
                }

                createTableStyle();

                // called when state changes
                setUpScenarioPage();

                if( theDb.ReadOnly ) {
                    this.acceptButton.Enabled = false;
                }
            }

            get {
                return theDb;
            }
        }

        private void setUpScenarioPage()
        {
            SimulationDb.SimulationType type = SimulationDb.SimulationType.Standard; // standard is default

            this.SuspendLayout();

            // default
            this.splitContainer3.Panel1Collapsed = true;
            


            type = (SimulationDb.SimulationType)currentSimulation.type;

            string query = "id = " + currentSimulation.id;

            DataRow[] simseeds = theDb.Data.scenario_simseed.Select(query, "", DataViewRowState.CurrentRows);

            if (simseeds.Length > 0)
            {
                object obj = simseeds[0]["seed"];
                int seed = (int)obj;

                short val = (short) seed;

                if (val < 0)
                {
                    val = (short) -val;
                }
            }
        

            switch (type)
            {
                case SimulationDb.SimulationType.Parallel:
                    break;

                case SimulationDb.SimulationType.Serial:
                    break;

                case SimulationDb.SimulationType.Random:
                    break;

              

                case SimulationDb.SimulationType.Statistical:	// statistical

                    this.splitContainer3.Panel1Collapsed = false;
                    this.tabControl.TabPages.Remove(variablePage);
                   // this.
                   
                    break;

                case SimulationDb.SimulationType.Optimize:
                    break;
                case SimulationDb.SimulationType.Calibration:
                    break;

                default:
                    this.tabControl.TabPages.Remove(variablePage);
                    break;

            }


            this.ResumeLayout(false);
        }

        private void createTableStyle()
        {
            seedGrid.Clear();
            seedGrid.AddNumericColumn("seed");
            seedGrid.Reset();
        }

        private void initializeMetricSelectBox()
        {
            metricTree.AfterCheck -= new TreeViewEventHandler(metricTree_AfterCheck);

            summaryNode.Checked = false;
            foreach (TreeNode node in summaryNode.Nodes)
            {
                node.Checked = false;
            }

            calibrationNode.Checked = false;
            foreach (TreeNode node in calibrationNode.Nodes)
            {
                node.Checked = false;
            }


            MrktSimDBSchema.scenario_metricRow[] metrics = currentSimulation.Getscenario_metricRows();

            foreach (MrktSimDBSchema.scenario_metricRow metric in metrics)
            {
                // look for token in summary
                TreeNode node = summaryNode.Nodes[metric.token];

                if (node != null)
                {
                    summaryNode.Checked = true;
                    node.Checked = true;
                }

                // look for token in summary
                node = calibrationNode.Nodes[metric.token];

                if (node != null)
                {
                    calibrationNode.Checked = true;
                    node.Checked = true;
                }
            }

            metricTree.AfterCheck += new TreeViewEventHandler(metricTree_AfterCheck);
        }

        private void updateSimulationMetrics()
        {
            foreach (TreeNode node in summaryNode.Nodes)
            {
                if (node.Checked)
                {
                    theDb.CreateScenarioMetric(currentSimulation, ((Metric)node.Tag).Token);
                }
                else
                {
                    theDb.DeleteScenarioMetric(currentSimulation, ((Metric)node.Tag).Token);
                }
            }

            foreach (TreeNode node in calibrationNode.Nodes)
            {
                if (node.Checked)
                {
                    theDb.CreateScenarioMetric(currentSimulation, ((Metric)node.Tag).Token);
                }
                else
                {
                    theDb.DeleteScenarioMetric(currentSimulation, ((Metric)node.Tag).Token);
                }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.simulationSetUpControl1.Refresh();
          
        }

        private void addSeedButton_Click(object sender, System.EventArgs e)
        {
            InputDouble dlg = new InputDouble();

            dlg.Max = 10;
            dlg.Value = 3;
            dlg.Min = 1;

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                int numSeeds = (int)Math.Floor(dlg.Value);

                // create this number of seeds
                // make sure they are different

                int curMin = 100;
                for (int ii = 0; ii < numSeeds; ++ii)
                {
                    int seed = rand.Next(curMin, curMin + 300);

                    // create seed with value
                    theDb.CreateScenarioSimSeed(currentSimulation, seed);

                    curMin = seed + 1;
                }

                this.simulationSetUpControl1.Refresh();
            }
        }

        private void acceptButton_Click(object sender, System.EventArgs e)
        {
            if (SimulationDb.NumSims(currentSimulation) == 0)
            {
                MessageBox.Show("This simulation will generate no results");

                return;
            }


            if( theDb.SimulationRunning() ) {
                DialogResult res = MessageBox.Show( this, "This simulation appers to be running - are you sure you want to save?", "Save Simulation?", MessageBoxButtons.YesNo );

                if( res == DialogResult.No ) {
                    return;
                }
            }

            string parserError = this.variableControl1.ParserTest();

            if (parserError != null)
            {
                MessageBox.Show("Error parsing expressions: " + parserError);

                return;
            }

            bool noneChecked = true;

            foreach (TreeNode node in summaryNode.Nodes)
            {
                if (node.Checked)
                {
                    noneChecked = false;
                }
            }

            foreach (TreeNode node in calibrationNode.Nodes)
            {
                if (node.Checked)
                {
                    noneChecked = false;
                }
            }

            if (noneChecked && deleteResults.Checked)
            {
                MessageBox.Show("Error: No metrics are computed and results are deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (noneChecked)
            {
                DialogResult rslt = MessageBox.Show("Warning: No metrics are being computed", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rslt == DialogResult.Cancel)
                {
                    return;
                }
            }

            SaveModel();

            this.Close();
        }

        public void SaveModel()
        {
            simulationSetUpControl1.WriteData();

            // create checked items for scenario
            updateSimulationMetrics();

            if (deleteResults.Checked)
            {
                currentSimulation.delete_std_results = true;
            }
            else
            {
                currentSimulation.delete_std_results = false;
            }

            this.parameterControl1.SuspendLayout();

            theDb.Update();

            this.parameterControl1.ResumeLayout();
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
        private void metricTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            metricTree.AfterCheck -= new TreeViewEventHandler(metricTree_AfterCheck);
            // make everyone like me - sorta
         
            if (e.Node.Parent != null)
            {
                // are any siblings still checked
                e.Node.Parent.Checked = false;

                foreach (TreeNode child in e.Node.Parent.Nodes)
                {
                    if (child.Checked)
                    {
                        e.Node.Parent.Checked = true;
                        break;
                    }
                }
            }

            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
            
            metricTree.AfterCheck += new TreeViewEventHandler(metricTree_AfterCheck);

        }
    }
}