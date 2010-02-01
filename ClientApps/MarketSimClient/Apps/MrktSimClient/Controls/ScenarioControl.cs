using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;


using MrktSimDb;

using Utilities;
using MrktSimClient.Controls.Dialogs;
using MrktSimClient.Controls.Dialogs.Calibration;
using Common.Dialogs;
using MarketSimSettings;
using Results;


namespace MrktSimClient.Controls
{
    public partial class ScenarioControl0 : UserControl
    {
        private string newSimItemString = "Create New Simulation...";
        private string viewResItemString = "View results...";
        private string renameScenariotemString = "Rename Scenario...";

        private string viewLogItemString = "View Warnings";
        private string dbResItemString = "Delete Results";
        private string viewStatusItemString = "View Status";

         private string runSimItemString = "Run Simulation...";
         private string editSimItemString = "Edit Simulation";
         private string copySimItemString = "Copy Simulation";
         private string deleteSimItemString = "Delete Simulation";

        private string scaleMismatchErrorMsg = "Warning: Checkpoint will be ignored since the population size used to create the \n" +
            "checkpoint ( {0} ) is different from from the population setting for this Simulation ( {1} ).\n\n" +
            "Proceed with Run?";

        private string scaleMismatchErrorTtile = "Population Size Does Not Match Checkpoint";

        private const double SIM_SCALE_MATCH_TOLERANCE = 0.005;

        private const string WarnString = "Warnings logged";

        public override void Refresh()
        {
            simGrid1.RowFilter = "scenario_id = " + Node.Id;
            resultsGrid.RowFilter = "scenario_id = " + Node.Id;

            name.Text = Node.ToString();
            name.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            descr.Text = Node.Scenario.descr;

            if (Node.ActiveNode)
            {
                expandLink1.Visible = false;
            }
            else
            {
                expandLink1.Visible = true;
            }

            base.Refresh();
        }


        new public void SuspendLayout()
        {
            base.SuspendLayout();

            this.resultsGrid.Suspend = true;
            this.simGrid1.Suspend = true;
        }

        new public void ResumeLayout()
        {
            base.ResumeLayout();

            this.resultsGrid.Suspend = false;
            this.simGrid1.Suspend = false;
        }

        private MsScenarioNode node;
        public MsScenarioNode Node
        {
            get
            {
                return node;
            }

            set
            {
                node = value;

                simGrid1.Table = Node.Db.Data.simulation;
                simGrid1.RowFilter = "scenario_id = " + Node.Id;
                simGrid1.ReadOnly = true;
                simGrid1.DescribeRow = "descr";
                simGrid1.AllowDelete = false;

                resultsGrid.Table = Node.Db.Data.sim_queue;
                resultsGrid.RowFilter = "scenario_id = " + Node.Id;
                resultsGrid.ReadOnly = true;
                resultsGrid.AllowDelete = false;

              

                createTableStyle();

              
                Refresh();
            }
        }

        public MrktSimDBSchema.scenarioRow Scenario
        {
            get
            {
                return Node.Scenario;
            }
        }

        public ScenarioControl0() {
            InitializeComponent();

            Control[] coloredPanels = new Control[]{ this.tableLayoutPanel1, this.tableLayoutPanel2,
                this.tableLayoutPanel3, this.name, this.panel2 };

            foreach( Control cp in coloredPanels ) {
                cp.BackColor = UIConfigSettings.Colors.MainNavigatorPanelColor;
            }
            this.descr.BackColor = Utilities.UIConfigSettings.Colors.GreenFadeStart;

            scenarioLink.AddItem( newSimItemString, newSim );
            scenarioLink.AddItem( viewResItemString, viewScenarioResults );
            scenarioLink.AddItem( renameScenariotemString, renameScenario );

            resultsLink.AddItem( viewResItemString, viewSelectedResults );
            resultsLink.AddItem( viewLogItemString, viewSimulationLog );
            resultsLink.AddItem( dbResItemString, deleteResults );
            resultsLink.AddItem( viewStatusItemString, viewSimProcess );

            simLink.AddItem( viewResItemString, viewSelectedSimResults );
            simLink.AddItem( runSimItemString, runSim );
            simLink.AddItem( editSimItemString, editSim );
            simLink.AddItem( copySimItemString, copySim );
            simLink.AddItem( deleteSimItemString, deleteSim );

            this.simLink.PopupMenuPanel = this.popupMenuPanel;
            this.resultsLink.PopupMenuPanel = this.popupMenuPanel;
            this.scenarioLink.PopupMenuPanel = this.popupMenuPanel;

            this.scenarioLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetScenarioMenuItemEnabling );
            this.simLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetSimMenuItemEnabling );
            this.resultsLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetResultsMenuItemEnabling );
        }

       // DataTable resTable;

        private void createTableStyle()
        {
            simGrid1.Clear();
            simGrid1.AddTextColumn("name");

            simGrid1.AddComboBoxColumn("type", SimulationDb.simulation_type, "type", "id", true);

            //simGrid1.AddComboBoxColumn("sim_num", ProjectDb.simulation_state_type, "name", "id", true);
            simGrid1.AddDateColumn("start_date");
            simGrid1.AddDateColumn("end_date");
            simGrid1.AddCheckBoxColumn( "reset_panel_state", "reset panel" );

            simGrid1.Reset();
          

            resultsGrid.Clear();

            //iProductGrid.AddTextColumn("brandName", true);
            resultsGrid.AddComboBoxColumn("sim_id", Node.Db.Data.simulation, "name", "id", true);

            resultsGrid.AddTextColumn("name");
            resultsGrid.AddDateColumn("current_date");
            resultsGrid.AddTextColumn("current_status");
            resultsGrid.AddDateColumn("run_time");

            resultsGrid.Reset();

        }

        private void expandLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ((MrktSim)this.ParentForm).ActivateNode(Node);
        }

        #region simulation methods


        private void copySim()
        {
            MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)simGrid1.CurrentRow;

            if (simulation != null)
            {
                if ((SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Standard)
                {
                    SimulationDb.SimulationType[] excludedTypes = null;
                    // allow user to change type                

                    excludedTypes = new SimulationDb.SimulationType[] {
                            SimulationDb.SimulationType.CheckPoint,
                            SimulationDb.SimulationType.Calibration,
                            SimulationDb.SimulationType.Optimize };
                 
                    CreateSimulation dlg = new CreateSimulation();

                    dlg.Text = "Change Type of Copied Simulation.";

                    dlg.DisAllowedType(excludedTypes);

                    dlg.Type = (SimulationDb.SimulationType) simulation.type;

                    DialogResult rslt = dlg.ShowDialog(this.ParentForm);

                    if (rslt == DialogResult.Cancel)
                        return;

                    MrktSimDBSchema.simulationRow copySim = Node.Db.CopySimulation(simulation);

                    copySim.name = dlg.Name;
                    copySim.type = (byte)dlg.Type;

                    Node.Db.Update();
                }
                else if( (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Parallel ||
                    (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Random ||
                    (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Serial)
                {
                    // can change to some other variable search
                    
                    // allow user to change type                
                    SimulationDb.SimulationType[] excludedTypes = new SimulationDb.SimulationType[] { 
                        SimulationDb.SimulationType.CheckPoint, 
                        SimulationDb.SimulationType.Calibration, 
                        SimulationDb.SimulationType.Optimize, 
                        SimulationDb.SimulationType.Standard, 
                        SimulationDb.SimulationType.Statistical };

                    CreateSimulation dlg = new CreateSimulation();

                    dlg.Text = "Change Type of Variable Search";

                    dlg.DisAllowedType(excludedTypes);

                    dlg.Type = (SimulationDb.SimulationType)simulation.type;

                    DialogResult rslt = dlg.ShowDialog(this.ParentForm);

                    if (rslt == DialogResult.Cancel)
                        return;

                    MrktSimDBSchema.simulationRow copySim = Node.Db.CopySimulation(simulation);

                    copySim.name = dlg.Name;
                    copySim.type = (byte)dlg.Type;

                    Node.Db.Update();
                }
                else
                {
                    // just copy
                    Node.Db.CopySimulation(simulation);
                }

                this.Refresh();
            }
        }

        private void deleteSim()
        {
            DialogResult rslt = MessageBox.Show("This will delete the simulaitons and results from the database, this action cannot be undone",
                "Delete Simulations", MessageBoxButtons.OKCancel);

            if (rslt == DialogResult.OK)
            {
                ArrayList simsToRun = simGrid1.GetSelected();

                foreach (MrktSimDBSchema.simulationRow sim in simsToRun)
                {
                    Node.Db.DeleteSimulation(sim.id);
                }

                Node.Db.RefreshTable(Node.Db.Data.sim_queue, "run_id");
                Node.Db.RefreshTable(Node.Db.Data.simulation, "id");
                
                this.Refresh();
            }
        }

        private void runSim()
        {
            ArrayList simsToRun = simGrid1.GetSelected();

            // if a checkpoint is active, be sure the sim to be run has a scale factor that agrees with the checkpiunt
            foreach( MrktSimDBSchema.simulationRow sim in simsToRun ) {
                if( sim.scenarioRow.Model_infoRow.checkpoint_valid ) {
                    double ckpScale = sim.scenarioRow.Model_infoRow.checkpoint_scale_factor;
                    double simScale = sim.scale_factor;
                    double ratio = 0;
                    // form the ratio so it is <= 1.0
                    if( (simScale <= ckpScale) && (ckpScale != 0) ) {
                        ratio = simScale / ckpScale;
                    }
                    else if( simScale != 0 ) {
                        ratio = ckpScale / simScale;
                    }
                    if( ratio < (1.0 - SIM_SCALE_MATCH_TOLERANCE) ) {
                        // the scale factors don't match
                        int pop = sim.scenarioRow.Model_infoRow.pop_size;

                        string msg = String.Format( scaleMismatchErrorMsg, pop * ckpScale / 100, pop * simScale / 100 );
                        ConfirmDialog cdlg = new ConfirmDialog( msg, scaleMismatchErrorTtile );
                        cdlg.SelectWarningIcon();
                        cdlg.SetOkCancelButtonStyle();
                        cdlg.Width += 85;
                        cdlg.Height += 85;
                        DialogResult resp = cdlg.ShowDialog();
                        if( resp != DialogResult.OK ) {
                            return;
                        }
                    }
                }
            }

            foreach (MrktSimDBSchema.simulationRow sim in simsToRun)
            {
                sim.sim_num = 1;

                //log the run
                DataLogger.Log( SimStartLogMessage( sim ) );
            }

            if (simsToRun.Count > 0)
            {
                Node.Db.Update();

                // make sure sim controller is running
                ((MrktSim)this.ParentForm).RunSim();
            }
            else
            {
                // TODO: Fix this
                MessageBox.Show("No simulations selected");
            }

        }

        private string simStartLogMessage = "Simulation Started: {6}/{5}/{4}/{0} {1}-{2} {3}";

        private string SimStartLogMessage( MrktSimDBSchema.simulationRow sim ) {
            MrktSimDBSchema.scenarioRow scenario = sim.scenarioRow;
            MrktSimDBSchema.Model_infoRow model = scenario.Model_infoRow;
            MrktSimDBSchema.projectRow project = (MrktSimDBSchema.projectRow) model.GetParentRow( "project_Model_info" );

            return String.Format( simStartLogMessage, sim.name, sim.start_date.ToString( "d/M/yy" ), sim.end_date.ToString( "d/M/yy" ), sim.type, 
                scenario.name, model.model_name, project.name );
        }

        private void editSim(MrktSimDBSchema.simulationRow sim, bool restrict)
        {
            Form dlg = null;
      
            if (sim.type == (byte)SimulationDb.SimulationType.Calibration)
            {
                CallibrationDb calDb = new CallibrationDb();
                calDb.Connection = Node.Db.Connection;
                calDb.Id = sim.id;
                calDb.Refresh();

                CalibrationEditor calEdit = new CalibrationEditor();
                calEdit.Db = calDb;

                dlg = calEdit;
            }
            else
            {
                SimulationDb simDb = new SimulationDb();
                simDb.Connection = Node.Db.Connection;
                simDb.Id = sim.id;
                simDb.Refresh();

                SimulationEditor simEdit = new SimulationEditor();
                simEdit.Db = simDb;

                dlg = simEdit;
            }

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                Node.Db.RefreshTable(Node.Db.Data.simulation);

                this.Refresh();
            }
        }

        private void editSim()
        {
            MrktSimDBSchema.simulationRow sim = (MrktSimDBSchema.simulationRow) simGrid1.CurrentRow;

            if (sim == null)
                return;


            if (sim.Getsim_queueRows().Length > 0)
            {
                editSim(sim, true);
            }
            else
            {
                editSim(sim, false);
            }
        }
        #endregion

        #region scenario methods

        private void newSim()
        {
            CreateSimulation dlg = new CreateSimulation();

            SimulationDb.SimulationType[] exludeTypes = null;


            if (((MrktSim)this.ParentForm).CalibrationControlsVisible == false)
            {
                exludeTypes = new SimulationDb.SimulationType[] { 
                    SimulationDb.SimulationType.Calibration, 
                    SimulationDb.SimulationType.Optimize};
            }
            else
            {
                exludeTypes = new SimulationDb.SimulationType[] { 
                    SimulationDb.SimulationType.Optimize};
            }

            dlg.DisAllowedType(exludeTypes);

            DialogResult rslt = dlg.ShowDialog(this);

            if (rslt == DialogResult.Cancel)
                return;

            MrktSimDBSchema.simulationRow sim = Node.Db.CreateStandardSimulation(Node.Scenario, dlg.Name);

            sim.type = (byte) dlg.Type;

            Node.Db.Update();

            int id = sim.id;

            Node.Refresh();

            Refresh();

            editSim(sim, false);
        }

        private void scenarioName_DoubleClick(object sender, EventArgs e)
        {
            ((MrktSim)this.ParentForm).ActivateNode(Node);

        }

        private void viewScenarioResults()
        {
            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.ScenarioID = Node.Id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm();

            dlg.Db = db;

            dlg.Show();
        }

   

        private void renameScenario()
        {
            NameAndDescr dlg = new NameAndDescr();

            dlg.Type = "Scenario";

            dlg.ObjDescription = node.Scenario.descr;

            dlg.ObjName = node.Scenario.name;

            dlg.Editing = true;

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                // rename
                Node.Scenario.name = dlg.ObjName;
                Node.Scenario.descr = dlg.ObjDescription;

                // write to disk
                Node.Db.Update();

                if (Node.ActiveNode)
                    this.Refresh();
                else
                    Node.ParentNode.Control.Refresh();
            }
        }

        ////private void sceneLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        ////{
        ////    PopUpMenu menu = new PopUpMenu((Control)sender);

        ////    menu.AddItem("Create New Simulation...", new PopUpMenu.LinkClicked(newSim));
        ////    menu.AddItem("View results...", new PopUpMenu.LinkClicked(viewScenarioResults));
        ////    menu.AddItem("Rename Scenario...", new PopUpMenu.LinkClicked(renameScenario));

        ////    DialogResult rslt = menu.ShowDialog();

        ////    if (menu.ClickedItem != null)
        ////    {
        ////        menu.ClickedItem();
        ////    }

        ////}

        #endregion

        #region results grid

        private void viewSimProcess()
        {
            MrktSimDBSchema.sim_queueRow row = (MrktSimDBSchema.sim_queueRow) resultsGrid.CurrentRow;

            if (row == null)
                return;

            MrktSimDBSchema.simulationRow sim = row.simulationRow;

            SimStatus dlg = new SimStatus(Application.StartupPath, MarketSimSettings.Settings<MrktSim.ClientSettings>.Value.ConnectFile, sim);

            dlg.Show();
        }

        private MrktSimDBSchema.simulationRow currentResultSim
        {
            get
            {
                MrktSimDBSchema.sim_queueRow row = (MrktSimDBSchema.sim_queueRow) resultsGrid.CurrentRow;

                if (row == null)
                {
                    return null;
                }

                return row.simulationRow;
            }
        }

        private void deleteResults()
        {
            ArrayList array = resultsGrid.GetSelected();

            foreach (MrktSimDBSchema.sim_queueRow res in array)
            {
                Node.Db.DeleteSimQueue(res.run_id);
            }

            Node.Db.RefreshTable(Node.Db.Data.sim_queue, "run_id");

            this.Refresh();
        }

        private void viewSimulationLog()
        {
            MrktSimDBSchema.sim_queueRow row = (MrktSimDBSchema.sim_queueRow) resultsGrid.CurrentRow;

            if (row == null)
                return;

            SimLogDb logDb = new SimLogDb();

            logDb.Connection = Node.Db.Connection;

            logDb.Run = row;

            logDb.Refresh();

            SimLog dlg = new SimLog();

            dlg.Db = logDb;


            dlg.ShowDialog();
        }

        private void viewSelectedSimResults() {
            MrktSimDBSchema.simulationRow sim = (MrktSimDBSchema.simulationRow)simGrid1.CurrentRow;

            if( sim == null )
                return;

            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.SimID = sim.id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm();

            dlg.Db = db;

            dlg.Show();
        }

        private void viewSelectedResults()
        {
            MrktSimDBSchema.sim_queueRow row = (MrktSimDBSchema.sim_queueRow) resultsGrid.CurrentRow;

            if (row == null)
                return;

            MrktSimDBSchema.simulationRow sim = row.simulationRow;

            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.SimID = sim.id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm();

            dlg.Db = db;

            dlg.Show();
        }

        #endregion

        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
        }

        private void SetScenarioMenuItemEnabling() {
         
            scenarioLink.EnableAllLinks();
          
        }

        private void SetSimMenuItemEnabling() {

            ArrayList sims = simGrid1.GetSelected();

            bool running = false;
            bool calSim = false;

            foreach (MrktSimDBSchema.simulationRow sim in sims)
            {
                if (sim != null && sim.sim_num >= 0)
                {
                    running = true;
                }

                if (sim.type == (byte)SimulationDb.SimulationType.Calibration)
                {
                    calSim = true;
                }
            }

            if (running ||
                (calSim &&  ((MrktSim)this.ParentForm).CalibrationControlsVisible == false))
            {
                 simLink.DisableLinks(runSimItemString, editSimItemString, copySimItemString, deleteSimItemString);
            }
            else if (sims.Count > 1)
            {
                simLink.DisableLinks(editSimItemString, copySimItemString);
            }
            else
            {
                simLink.EnableAllLinks();  
            }
        }

        private void SetResultsMenuItemEnabling() {
            DataRow resSim = resultsGrid.CurrentRow;

            //!!!???doesn't seem this is ever null if there any rows at all in resultsGrid (the first row is implicitly selected, though not visually highlighted)!!!
            if( resSim == null ) {
                resultsLink.DisableLinks( viewResItemString, viewLogItemString, dbResItemString, viewStatusItemString );
            }
            else {
                resultsLink.EnableAllLinks();
                // need to fix properly - right now setting this so warnings can be viewed
                //if( resSim[ "Status" ].ToString() != WarnString ) {
                //    resultsLink.DisableLinks( viewLogItemString );
                //}

                //if( (int)resSim[ "canDelete" ] == 0 ) {
                //    resultsLink.DisableLinks( dbResItemString );
               // }
            }
        }      
    }
}
