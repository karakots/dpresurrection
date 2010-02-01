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
    public partial class ScenarioControl : UserControl
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
        private string deQueueSimItemString = "Dequeue Simulation";

        private string confirmResultDeleteMsg = "Delete selected Result? ";
        private string confirmResultsDeleteMsg = "Delete all ( {0} ) selected Results? ";
        private string confirmResultDeleteTitle = "Confirm Delete";
        private string deleteResultsStatus = "Deleting Results...";
        private string deleteResultsDoneMsg = "Results deleted successfully.";

        private string confirmSimDeleteMsg = "Delete selected Simulation and all associated Results? ";
        private string confirmSimsDeleteMsg = "Delete all ( {0} ) selected Simulations and associated Results? ";
        private string confirmSimDeleteTitle = "Confirm Delete";
        private string deleteSimStatus = "Deleting Simulation(s)..";
        private string deleteSimDoneMsg = "Simulation(s) deleted successful.";

        private const string WarnString = "Warnings logged";

        private Font inactiveFont = new Font( "Arial", 8 );
        private Font activeFont = new Font( "Arial", 8, FontStyle.Bold );

        private Color runningBackColor = Color.FromArgb( 210, 248, 192 );
        private Color runningForeColor = Color.FromArgb( 6, 96, 2 );
        private Color runningSelectedBackColor = Color.FromArgb( 96, 19, 166 );
        private Color runningSelectedForeColor = Color.FromArgb( 92, 245, 41 );

        private Color queuedBackColor = Color.FromArgb( 252, 254, 179 );
        private Color queuedForeColor = Color.FromArgb( 153, 79, 23 );
        private Color queuedSelectedBackColor = Color.FromArgb( 2, 91, 9 );
        private Color queuedSelectedForeColor = Color.FromArgb( 255, 198, 0 );

        private string simStartLogMessage = "Simulation Started: {6}/{5}/{4}/{0} {1}-{2} {3}";

        private int previousLastActiveRow = -1;
        private Timer autoRefreshResultsTimer;

        private System.Data.OleDb.OleDbDataAdapter simStatusAdapter;
        private System.Data.OleDb.OleDbDataAdapter resultsStatusAdapter;

        private MrktSimDBSchema.simulation_statusDataTable simulation_statusTable;
        private MrktSimDBSchema.results_statusDataTable results_statusTable;

        private static Hashtable runPrevTimeStepInfo;

        /// <summary>
        /// Refreshes the scenario control.
        /// </summary>
        public override void Refresh() {
            if( this.Visible ) {
                this.Focus();
            }

            name.Text = Node.ToString();
            name.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            descr.Text = Node.Scenario.descr;

            UpdateDisplays();

            if( Node.ActiveNode ) {
                expandLink1.Visible = false;
            }
            else {
                expandLink1.Visible = true;
            }
            base.Refresh();
        }

        private MsScenarioNode node;
        public MsScenarioNode Node {
            get {
                return node;
            }

            set {
                node = value;

                System.Data.OleDb.OleDbCommand simStatCommand = new System.Data.OleDb.OleDbCommand();
                simStatCommand.CommandTimeout = 0;
                simStatCommand.Connection = node.Db.Connection;
                MrktSimDb.SqlQuery.InitializeSimulationStatusSelect( simStatCommand );       // initialize the SELECT query for the simulation_status view
                simStatCommand.CommandText += " AND scenario_id = " + node.Id;

                // set up the SQL adapter for the simulation status 
                simStatusAdapter = new System.Data.OleDb.OleDbDataAdapter();
                simStatusAdapter.SelectCommand = simStatCommand;

                simulation_statusTable = new MrktSimDBSchema.simulation_statusDataTable();
                this.simDataGridView.DataSource = simulation_statusTable;                        // hook up to the UI

                System.Data.OleDb.OleDbCommand resStatCommand = new System.Data.OleDb.OleDbCommand();
                resStatCommand.CommandTimeout = 0;
                resStatCommand.Connection = node.Db.Connection;
                MrktSimDb.SqlQuery.InitializeResultsStatusSelect( resStatCommand );           // initialize the SELECT query for the results_status view
                resStatCommand.CommandText += " AND scenario_id = " + node.Id;

                // set up the SQL adapter for the results status 
                resultsStatusAdapter = new System.Data.OleDb.OleDbDataAdapter();
                resultsStatusAdapter.SelectCommand = resStatCommand;

                results_statusTable = new MrktSimDBSchema.results_statusDataTable();
                this.resultsDataGridView.DataSource = results_statusTable;

                if( MrktSim.DevlMode == true ) {        //!!!!TEST/DEVEL
                    this.ioPlotsButton.Visible = true;
                }

                Refresh();
            }
        }

        /// <summary>
        /// Get the scenario for the active view.
        /// </summary>
        public MrktSimDBSchema.scenarioRow Scenario {
            get {
                return Node.Scenario;
            }
        }

        /// <summary>
        /// Create a new scenario control.
        /// </summary>
        public ScenarioControl() {
            InitializeComponent();

            Control[] coloredPanels = new Control[] { this.tableLayoutPanel3, this.name, this.panel2, this.splitContainer3.Panel1, this.splitContainer3.Panel2 };

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
            simLink.AddItem( deQueueSimItemString, dequeueSim );

            this.simLink.PopupMenuPanel = this.popupMenuPanel;
            this.resultsLink.PopupMenuPanel = this.popupMenuPanel;
            this.scenarioLink.PopupMenuPanel = this.popupMenuPanel;

            this.scenarioLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetScenarioMenuItemEnabling );
            this.simLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetSimMenuItemEnabling );
            this.resultsLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetResultsMenuItemEnabling );

            this.autoRefreshResultsTimer = new Timer();
            this.autoRefreshResultsTimer.Interval = 3000;
            this.autoRefreshResultsTimer.Tick += new EventHandler( autoRefreshResultsTimer_Tick );

            runPrevTimeStepInfo = new Hashtable();
        }

        /// <summary>
        /// Updates the simulation status display to reflect the current status.
        /// </summary>
        private bool RefreshSimulationStatusDisplay() {

            //JimJ 12-27-07 preserve the scroll point for the window
            int currentScrollPoint = simDataGridView.FirstDisplayedScrollingRowIndex;

            bool rval = false;
            //save selected ids
            ArrayList selectedSimIDs = new ArrayList();
            foreach( DataGridViewRow row in this.simDataGridView.SelectedRows ) {
                int simID = (int)row.Cells[ "SimulationIdCol" ].Value;
                selectedSimIDs.Add( simID );
            }

            try {
                // update the data
                simDataGridView.SuspendLayout();
                simulation_statusTable.Clear();
                simStatusAdapter.Fill( simulation_statusTable );
                simDataGridView.ResumeLayout();
            }
            catch( Exception oops ) {
                Console.WriteLine( "Exception Filling simulation_statusTable!\n" + oops.ToString() );
                return rval;
            }

         

            // set the color coding
            for( int i = 0; i < simDataGridView.RowCount; i++ ) {
                DataGridViewRow row = simDataGridView.Rows[ i ];
                string stat = (string)row.Cells[ "StatusCol" ].Value;
                if( stat == "Running" ) {
                    row.DefaultCellStyle.Font = activeFont;
                    row.DefaultCellStyle.ForeColor = runningForeColor;
                    row.DefaultCellStyle.BackColor = runningBackColor;
                    row.DefaultCellStyle.SelectionForeColor = runningSelectedForeColor;
                    row.DefaultCellStyle.SelectionBackColor = runningSelectedBackColor;
                  
                  

                    rval = true;
                }
                else if( stat == "Queued" ) {
                    row.DefaultCellStyle.Font = inactiveFont;
                    row.DefaultCellStyle.ForeColor = queuedForeColor;
                    row.DefaultCellStyle.BackColor = queuedBackColor;
                    row.DefaultCellStyle.SelectionForeColor = queuedSelectedForeColor;
                    row.DefaultCellStyle.SelectionBackColor = queuedSelectedBackColor;
                   

                    rval = true;
                }

                // reselect same items
                int simID = (int)row.Cells[ "SimulationIdCol" ].Value;
                row.Selected = selectedSimIDs.Contains( simID );

            }

            //JimJ 12-27-07 preserve the scroll point for the window
            if( currentScrollPoint >= simDataGridView.RowCount ) {
                currentScrollPoint = simDataGridView.RowCount - 1;
            }
            if( currentScrollPoint > 0 ) {
                simDataGridView.FirstDisplayedScrollingRowIndex = currentScrollPoint;
            }

            return rval;
        }

        private void autoRefreshResultsTimer_Tick( Object sender, System.EventArgs e ) {
            //Console.WriteLine( "\n -- -- -- >>> Auto Refresh Tick :: " + DateTime.Now.Ticks );

            UpdateDisplays();
        }

        public void UpdateDisplays() {
            // shut down the timer - so no collisions
            if( autoRefreshResultsTimer != null ) {
                autoRefreshResultsTimer.Stop();
            }

            // this will fire a new timer if there are still running simulations
            bool runningSims = RefreshResultStatusDisplay();

            bool queuedSims = RefreshSimulationStatusDisplay();

            if( queuedSims || runningSims ) {
                this.autoRefreshResultsTimer.Start();
            }
        }

        /// <summary>
        /// Updates the results status display to reflect the current status.
        /// </summary>
        /// <param name="calledByTimer"></param>
        /// <returns>true if timer started - false elsewise</returns>
        private bool RefreshResultStatusDisplay() {
            //save selected ids
            ArrayList selectedRunIDs = new ArrayList();
            foreach( DataGridViewRow row in this.resultsDataGridView.SelectedRows ) {
                int runID = (int)row.Cells[ "RunIdCol" ].Value;
                selectedRunIDs.Add( runID );
            }

            try {
                // update the data
                resultsDataGridView.SuspendLayout();
                results_statusTable.Clear();
                resultsStatusAdapter.Fill( results_statusTable );
                resultsDataGridView.ResumeLayout();
            }
            catch( Exception oops ) {
                Console.WriteLine( "Exception Filling results_statusTable!\n" + oops.ToString() );
                return false;
            }

            // style the display rows depending on their status
            int lastRunningRow = -1;
            int lastPendingRow = -1;

            for( int i = 0; i < resultsDataGridView.RowCount; i++ ) {
                DataGridViewRow row = resultsDataGridView.Rows[ i ];
                int runID = (int)row.Cells[ "RunIdCol" ].Value;

                string stat = (string)row.Cells[ "ResultsStatusCol" ].Value;
                string curProgress = (string)row.Cells[ "CurrentStatusCol" ].Value;
                DateTime progressStart = new DateTime();
                DateTime progressSimStart = new DateTime();
                DateTime progressEnd = new DateTime();
                DateTime progressDate = new DateTime();

               //  bool isRunning = false;

                row.Cells[ "TimeLeft" ].Value = "";

                if( stat == "running" ) {
                    // isRunning = true;

                    lastRunningRow = i;
                    row.DefaultCellStyle.Font = activeFont;
                    row.DefaultCellStyle.ForeColor = runningForeColor;
                    row.DefaultCellStyle.BackColor = runningBackColor;
                    row.DefaultCellStyle.SelectionForeColor = runningSelectedForeColor;
                    row.DefaultCellStyle.SelectionBackColor = runningSelectedBackColor;

                    // put a progress bar in place of the status details while a sim is running
                    int progressColWid = resultsDataGridView.Columns[ "CurrentStatusCol" ].Width;
                    int simID = (int)row.Cells[ "ResultsSimulationIdCol" ].Value;
                    int scenarioID = (int)row.Cells[ "ResultsScenarioIdCol" ].Value;
                    MrktSimDBSchema.scenarioRow scen = Node.Db.Data.scenario.FindByscenario_id( scenarioID );
                    MrktSimDBSchema.simulationRow sim = Node.Db.Data.simulation.FindByid( simID );
                    MrktSimDBSchema.Model_infoRow mod = Node.Db.Data.Model_info.FindBymodel_id( scen.model_id );

                    progressStart = mod.start_date;
                    if( mod.checkpoint_valid ) {
                        double ckpRatio = mod.checkpoint_scale_factor / sim.scale_factor;
                        if( ckpRatio >= 0.995 && ckpRatio <= 1.005 ) {
                            progressStart = mod.checkpoint_date;
                        }
                    }
                    progressSimStart = sim.start_date;
                    progressEnd = sim.end_date;
                    progressDate = (DateTime)row.Cells[ "CurrentDateCol" ].Value;
                    string runStat = String.Format( "run: {0}", progressDate.ToString( "MM/dd/yy" ) );
                    row.Cells[ "ResultsStatusCol" ].Value = runStat;

                    string textProgressBar = CreateProgressBar( progressColWid, progressDate, progressStart, progressSimStart, progressEnd, row.DefaultCellStyle.Font );
                    if( textProgressBar != null ) {
                        row.Cells[ "CurrentStatusCol" ].Value = textProgressBar;
                    }

                    string remainingTime = ComputeTimeRemaining( runID, progressDate, progressStart, progressSimStart, progressEnd );
                    row.Cells[ "TimeLeft" ].Value = remainingTime;
                }
                else if( stat == "pending" ) {
                    lastPendingRow = i;
                    row.DefaultCellStyle.Font = inactiveFont;
                    row.DefaultCellStyle.ForeColor = queuedForeColor;
                    row.DefaultCellStyle.BackColor = queuedBackColor;
                    row.DefaultCellStyle.SelectionForeColor = queuedSelectedForeColor;
                    row.DefaultCellStyle.SelectionBackColor = queuedSelectedBackColor;

                    row.Cells[ "TimeLeft" ].Value = "?";
                }
                    // TBD need to add the actual numeric stat
                    // so we do not depend on the exact wording of state here
                    // SSN 10-20-2007
                else if (stat != "done ") {
                    if( curProgress != "stopped" && curProgress != "Sim Processed" && curProgress != "done" ) {
                        // the sim is post=processing
                        row.Cells[ "ResultsStatusCol" ].Value = "metrics";
                        row.DefaultCellStyle.Font = activeFont;
                        row.DefaultCellStyle.ForeColor = runningForeColor;
                        row.DefaultCellStyle.BackColor = runningBackColor;
                        row.DefaultCellStyle.SelectionForeColor = runningSelectedForeColor;
                        row.DefaultCellStyle.SelectionBackColor = runningSelectedBackColor;
                    }
                }
                
                // reselect same items
                int thisRowRunID = (int)row.Cells[ "RunIdCol" ].Value;
                row.Selected = selectedRunIDs.Contains( thisRowRunID );
            }

            int lastActiveRow = (int)Math.Max( lastRunningRow, lastPendingRow );

            if( previousLastActiveRow != lastActiveRow ) {
                // there has been a change in the row index of the last queued or running result since the previous update - select the newly active item
                resultsDataGridView.ClearSelection();
                if( lastActiveRow != -1 ) {
                    resultsDataGridView.Rows[ lastActiveRow ].Selected = true;
                }
            }

            //make the last row always viisble
            if( resultsDataGridView.RowCount > 0 ) {
                int firstDisplayedRow = (int)Math.Min( Math.Max( 0, resultsDataGridView.RowCount - resultsDataGridView.DisplayedRowCount( false ) + 2 ),
                            resultsDataGridView.RowCount - 1 );
                resultsDataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedRow;
            }

            //automatically refresh if there are running simulations
            //Console.WriteLine( "last active row = {0}", lastActiveRow );
            bool rval = false;
            if( lastActiveRow != -1 ) {
                rval = true;
                resultsDataGridView.Columns[ "TimeLeft" ].Visible = true;
                this.TimeLeft.DisplayIndex = 10;
            }
            else {
                resultsDataGridView.Columns[ "TimeLeft" ].Visible = false;
            }

            this.previousLastActiveRow = lastActiveRow;

            return rval;
        }

        private string CreateProgressBar( int colWid, DateTime currentSimDate, DateTime silentStart, DateTime runStart, DateTime endDate, Font font ) {
            if( currentSimDate == endDate ) {
                // we are done!
                return null;
            }
            TimeSpan span = endDate - silentStart;
            TimeSpan mid = runStart - silentStart;
            TimeSpan cur = currentSimDate - silentStart;
            double totDays = span.TotalDays;
            double m = mid.TotalDays / totDays;
            double c = cur.TotalDays / totDays;
            double bar1Wid = Math.Min( m, c ) * colWid;
            double bar2Wid = 0;
            if( c > m ) {
                bar2Wid = c * colWid;
            }
            // now we know the width of bar1 (silent running) and bar2 (the sim date range itself)
            Graphics g = Graphics.FromHwnd( this.Handle );
            string bar = "";
            do {
                bar += "x";
            } while( g.MeasureString( bar, font ).Width < bar1Wid );
            if( bar.Length > 0 ) {
                bar = bar.Substring( 0, bar.Length - 1 );
            }
            if( bar2Wid > 0 ) {
                do {
                    bar += "#";
                } while( g.MeasureString( bar, font ).Width < bar2Wid );
                if( bar.Length > 0 ) {
                    bar = bar.Substring( 0, bar.Length - 1 );
                }
            }
            return bar;
        }

        private string timeRemainingString = "";

        private string ComputeTimeRemaining( int runID, DateTime currentSimDate, DateTime modelOrCheclpointStart, DateTime simStartDate, DateTime simEndDate ) {
            if( currentSimDate == simEndDate ) {
                // we are done!
                return "0:30 (final processing)";
            }
            double nAveragePoints = 3;

            TimeSpan elapsed = currentSimDate - simStartDate;
            TimeSpan remain = simEndDate - currentSimDate;
            TimeSpan simDuration = simEndDate - simStartDate;

            double ratio = remain.TotalDays / simDuration.TotalDays;
            double ratio2 = remain.TotalDays / elapsed.TotalDays;

            //Console.WriteLine( "\nComputeTimeRemaining( cur={0}, start={1}, end={2})", currentSimDate.ToShortDateString(), simStartDate.ToShortDateString(),
            //   simEndDate.ToShortDateString() );
            //Console.WriteLine( "\nRemaining time ratio  (runID = {1}) ==> {0:f2}      ratio2 = {2:f2}", ratio, runID, ratio2 );

            if( runPrevTimeStepInfo.Contains( runID ) == false ) {
                object[] stepData = new object[] { DateTime.Now, simStartDate, simStartDate, -1.0 };      // remTicks is -1 since we don't include the very first point (it is initialization time)
                runPrevTimeStepInfo.Add( runID, stepData );
                timeRemainingString = "left: ? (initializing)";
                return timeRemainingString;
            }
            else {
                object[] prevStepData = (object[])runPrevTimeStepInfo[ runID ];
                DateTime startTime = (DateTime)prevStepData[ 0 ];
                DateTime actualStartSimDate = (DateTime)prevStepData[ 1 ];
                DateTime prevSimDate = (DateTime)prevStepData[ 2 ];
                double prevRemTicks = (double)prevStepData[ 3 ];

                if( currentSimDate != prevSimDate ) {

                    if( prevRemTicks == -1.0 ) {
                        // this is the first real data point - save it.
                        object[] stepData = new object[] { DateTime.Now, currentSimDate, currentSimDate, -2.0 };
                        runPrevTimeStepInfo[ runID ] = stepData;
                        //timeRemainingString = "(at 1st point)";
                        return timeRemainingString;
                    }
                    else {
                        // now we can compute an estimate for remaining time
                        TimeSpan timeSinceSimStart = DateTime.Now - startTime;

                        double estRemainingTicks = timeSinceSimStart.Ticks * ratio2;
                        bool isFirstEstimate = false;
                        if( prevRemTicks != -2.0 ) {
                            // if we are going down, filter
                            if( estRemainingTicks < prevRemTicks ) {
                                estRemainingTicks = (estRemainingTicks + ((nAveragePoints - 1) * prevRemTicks)) / nAveragePoints;
                            }
                        }
                        else {
                            isFirstEstimate = true;      // we won't bother to show the first estimate since it is rarely very accurate
                        }

                        object[] simRunData = new object[] { startTime, actualStartSimDate, currentSimDate, estRemainingTicks };
                        runPrevTimeStepInfo[ runID ] = simRunData;        // save the current estimate


                        TimeSpan estRemaing = new TimeSpan( (long)Math.Round( estRemainingTicks ) );
                        estRemaing = estRemaing.Add( new TimeSpan( 0, 0, 30 ) );       // allow 30sec for post-processing

                        Console.WriteLine( "Time Remaining::: " + estRemaing.ToString() );

                        if( isFirstEstimate == false ) {
                            if( ratio < 0.5 ) {
                                if( estRemaing.Minutes == 0 ) {
                                    timeRemainingString = String.Format( "left: {0} seconds", estRemaing.Seconds - (estRemaing.Seconds % 10) );
                                }
                                else if( estRemaing.Minutes == 1 ) {
                                    timeRemainingString = String.Format( "left: 1 min {0} seconds", estRemaing.Seconds - (estRemaing.Seconds % 10) );
                                }
                                else if( estRemaing.Minutes < 60 ) {
                                    timeRemainingString = String.Format( "left: {0} mins {1} seconds", estRemaing.Minutes, estRemaing.Seconds - (estRemaing.Seconds % 10) );
                                }
                                else if( estRemaing.Minutes < 120 ) {
                                    timeRemainingString = String.Format( "left: 1 hr {0} minutes", estRemaing.Minutes );
                                }
                                else {
                                    timeRemainingString = String.Format( "left: {1} hrs {0} minutes", estRemaing.Minutes, estRemaing.Hours );
                                }
                            }
                            else {
                                // make approximate estimates only during the first half of the run
                                if( estRemaing.Minutes == 0 ) {
                                    timeRemainingString = String.Format( "left: ~1-2 minutes" );
                                }
                                else if( estRemaing.Minutes == 1 ) {
                                    timeRemainingString = String.Format( "left: ~2-3 minutes" );
                                }
                                else if( estRemaing.Minutes < 60 ) {
                                    timeRemainingString = String.Format( "left: ~{0}-{1} minutes", estRemaing.Minutes + 1, estRemaing.Minutes + 2 );
                                }
                                else if( estRemaing.Minutes < 120 ) {
                                    timeRemainingString = String.Format( "left: ~1 hr {0} minutes", estRemaing.Minutes + 10 - (estRemaing.Minutes % 10) );
                                }
                                else {
                                    timeRemainingString = String.Format( "left: ~{0} hrs {1} minutes", estRemaing.Hours, estRemaing.Minutes + 10 - (estRemaing.Minutes % 10) );
                                }
                            }
                        }
                        return timeRemainingString;
                    }
                }
                else {
                    return timeRemainingString;
                }
            }
        }

        /// <summary>
        /// Returns an array of all selected simulations
        /// </summary>
        /// <returns></returns>
        private MrktSimDBSchema.simulationRow[] GetSelectedSims() {
            MrktSimDBSchema.simulationRow[] selSims = new MrktSimDBSchema.simulationRow[ this.simDataGridView.SelectedRows.Count ];
            for( int i = 0; i < this.simDataGridView.SelectedRows.Count; i++ ) {
                DataGridViewRow dgrow = this.simDataGridView.SelectedRows[ i ];
                int simID = (int)dgrow.Cells[ "SimulationIdCol" ].Value;
                selSims[ i ] = Node.Db.Data.simulation.FindByid( simID );
            }
            return selSims;
        }

        /// <summary>
        /// Returns the selected simulation, if exactly one simulation is selected.  Othersise returns null.
        /// </summary>
        /// <param name="selItem"></param>
        /// <returns></returns>
        private MrktSimDBSchema.simulationRow GetSelectedSim() {
            MrktSimDBSchema.simulationRow[] selSims = GetSelectedSims();
            if( selSims.Length == 1 ) {
                return selSims[ 0 ];
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Returns an array of all selected results
        /// </summary>
        /// <returns></returns>
        private MrktSimDBSchema.results_statusRow[] GetSelectedResults()
        {
            MrktSimDBSchema.results_statusRow[] selResults = new MrktSimDBSchema.results_statusRow[this.resultsDataGridView.SelectedRows.Count];
            for( int i = 0; i < this.resultsDataGridView.SelectedRows.Count; i++ ) {
                DataGridViewRow dgrow = this.resultsDataGridView.SelectedRows[ i ];
               // int runID = (int)dgrow.Cells[ "RunIdCol" ].Value;
                selResults[i] = (MrktSimDBSchema.results_statusRow) ((DataRowView) dgrow.DataBoundItem).Row;
            }
            return selResults;
        }

        /// <summary>
        /// Returns the selected result item, if exactly one result item is selected.  Othersise returns null.
        /// </summary>
        /// <param name="selItem"></param>
        /// <returns></returns>
        private MrktSimDBSchema.results_statusRow GetSelectedResult()
        {
            MrktSimDBSchema.results_statusRow[] selRes = GetSelectedResults();
            if( selRes.Length == 1 ) {
                return selRes[ 0 ];
            }
            else {
                return null;
            }
        }


        private void expandLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
            ((MrktSim)this.ParentForm).ActivateNode( Node );
        }

        #region simulation methods

        /// <summary>
        /// Allows the user to copy the selected simulation.  The copied simulation can be the same type or an (appropriate) different type.
        /// This could use some clean up - SSN 9-21-2007
        /// </summary>
        private void copySim() {

            // sanity check
            MrktSimDBSchema.simulationRow simulation = GetSelectedSim();
            if( simulation == null ) {
                return;
            }

            if( node.Db.SimDeleted( simulation.id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();

                return;
            }

            if( (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Standard ) {
                SimulationDb.SimulationType[] excludedTypes = null;
                // allow user to change type                

                if( ((MrktSim)this.ParentForm).CalibrationControlsVisible == false ) {
                    excludedTypes = new SimulationDb.SimulationType[] {
                            SimulationDb.SimulationType.CheckPoint,
                            SimulationDb.SimulationType.Calibration,
                            SimulationDb.SimulationType.Optimize };
                }
                else {
                    excludedTypes = new SimulationDb.SimulationType[] {
                            SimulationDb.SimulationType.CheckPoint,
                            SimulationDb.SimulationType.Optimize };
                }

                CreateSimulation dlg = new CreateSimulation();

                dlg.Text = "Change Type of Copied Simulation.";

                dlg.DisAllowedType( excludedTypes );

                dlg.Type = (SimulationDb.SimulationType)simulation.type;

                DialogResult rslt = dlg.ShowDialog( this.ParentForm );

                if( rslt == DialogResult.Cancel )
                    return;

                MrktSimDBSchema.simulationRow copySim = Node.Db.CopySimulation( simulation );

                copySim.name = dlg.Name;
                copySim.type = (byte)dlg.Type;

                Node.Db.Update();
            }
            else if( (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Parallel ||
                    (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Random ||
                    (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Serial ) {
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

                dlg.DisAllowedType( excludedTypes );

                dlg.Type = (SimulationDb.SimulationType)simulation.type;

                DialogResult rslt = dlg.ShowDialog( this.ParentForm );

                if( rslt == DialogResult.Cancel )
                    return;

                MrktSimDBSchema.simulationRow copySim = Node.Db.CopySimulation( simulation );

                copySim.name = dlg.Name;
                copySim.type = (byte)dlg.Type;

                Node.Db.Update();
            }
            else if( (SimulationDb.SimulationType)simulation.type == SimulationDb.SimulationType.Calibration ) {
                SimulationDb.SimulationType[] excludedTypes = new SimulationDb.SimulationType[] { 
                        SimulationDb.SimulationType.CheckPoint, 
                        SimulationDb.SimulationType.Optimize, 
                        SimulationDb.SimulationType.Standard, 
                        SimulationDb.SimulationType.Statistical };

                CreateSimulation dlg = new CreateSimulation();

                dlg.Text = "Change Type of Simulation";

                dlg.DisAllowedType( excludedTypes );

                dlg.Type = (SimulationDb.SimulationType)simulation.type;

                DialogResult rslt = dlg.ShowDialog( this.ParentForm );

                if( rslt == DialogResult.Cancel )
                    return;

                MrktSimDBSchema.simulationRow copySim = Node.Db.CopySimulation( simulation );

                copySim.name = dlg.Name;
                copySim.type = (byte)dlg.Type;

                Node.Db.Update();
            }
            else {
                // just copy
                Node.Db.CopySimulation( simulation );
            }

            this.Refresh();
        }

        /// <summary>
        /// Allows the user to delete the selected simulation(s).
        /// </summary>
        private void deleteSim() {
            MrktSimDBSchema.simulationRow[] selectedSims = GetSelectedSims();

            foreach( MrktSimDBSchema.simulationRow sim in selectedSims ) {
                if( sim != null ) {

                    if( node.Db.SimDeleted( sim.id ) ) {
                        ((MrktSim)this.ParentForm).ResetUI();

                        return;
                    }
                    else {
                        if( node.Db.SimulationRunning( sim.id ) ) {
                            MessageBox.Show( MrktSim.Message( "Simulation.Queued" ) );
                            return;
                        }

                        string who = null;
                        if( node.Db.SimulationLocked( sim.id, out who ) ) {
                            MessageBox.Show( MrktSim.Message( "Simulation.Locked by:" + who) );
                            return;
                        }
                    }
                }
            }

            int nSims = selectedSims.Length;

            string msg = confirmSimDeleteMsg;
            if( nSims > 1 ) {
                msg = String.Format( confirmSimsDeleteMsg, nSims );
            }
            ConfirmDialog cdlg = new ConfirmDialog( msg, confirmSimDeleteTitle );
            cdlg.SelectWarningIcon();
            cdlg.SetOkCancelButtonStyle();
            cdlg.Width -= 80;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteSimulationsNow );

            // update the status while the background thread runs
            string statusMsg = String.Format( deleteSimStatus, node.ToString() );
            status.UpdateUIAndContinue( deleteSim_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void deleteSim_Part2() {
            this.Refresh();

            ClearStatusDisplay();

            CompletionDialog dlg = new CompletionDialog( deleteSimDoneMsg );
            dlg.ShowDialog();
        }

        /// <summary>
        /// Deletes selected simulations (can be run in a background thread).
        /// </summary>
        private void DeleteSimulationsNow() {
            MrktSimDBSchema.simulationRow[] selectedSims = GetSelectedSims();

            foreach( MrktSimDBSchema.simulationRow sim in selectedSims ) {
                Node.Db.DeleteSimulation( sim.id );
            }

            Node.Db.RefreshTable( Node.Db.Data.simulation, "id" );
        }

        /// <summary>
        /// Runs the selected simulation(s).
        /// </summary>
        /// <remarks>This method doesn't physically "run" a simulation; it simply sets a flag in the appropriate record of the simulation data
        /// table, and checks to be sure the simulation engine is actually running.  The simulation engine automatically detects the change 
        /// in the simulation data table, and in response it commences the actual processing of the sim.  Status of running simulations from the 
        /// engine is provided in both the sim_queue table (for which the rows are created and updated by the engine) and the "sim_num" column 
        /// of the simulations table.</remarks>
        private void runSim() {

            string runningSims = null;
            foreach( MrktSimDBSchema.simulationRow sim in GetSelectedSims() ) {

                if( node.Db.SimDeleted( sim.id ) ) {

                    ((MrktSim)this.ParentForm).ResetUI();
                    return;

                }
                else if( node.Db.SimulationRunning( sim.id ) ) {
                    if( runningSims == null ) {
                        runningSims = "\n\r";
                    }

                    runningSims += sim.name + "\n\r";
                }
            }

            if( runningSims != null ) {
                MessageBox.Show( "The following simulations are already running" + runningSims );
                
                return;
            }

            int numSimsToRun = simDataGridView.SelectedRows.Count;

            foreach( MrktSimDBSchema.simulationRow sim in GetSelectedSims() ) {

                // start the sim (set the state so the engine knows we want this sim to be run)
                sim.sim_num = 1;

                //log the run
                DataLogger.Log( SimStartLogMessage( sim ) );
            }

            if( numSimsToRun > 0 ) {
                Node.Db.Update();

                // make sure sim controller is running
                ((MrktSim)this.ParentForm).RunSim();
            }

            UpdateDisplays();
        }

        private void dequeueSim() {

            string errorSims = null;
            foreach( MrktSimDBSchema.simulationRow sim in GetSelectedSims() ) {

                if( node.Db.SimDeleted( sim.id ) ) {

                    ((MrktSim)this.ParentForm).ResetUI();
                    return;

                }
                
                if( sim.sim_num == 1 && !node.Db.DequeueSim( sim.id ) ) {
                    if( errorSims == null ) {
                        errorSims = "\n\r";
                    }

                    errorSims += sim.name + "\n\r";
                }
            }

            if( errorSims != null ) {
                MessageBox.Show( "The following simulations could not be dequeued" + errorSims );

                return;
            }

            UpdateDisplays();
        }

        /// <summary>
        /// Writes a message to the data log recording a request to start a simulation.
        /// </summary>
        /// <param name="sim"></param>
        /// <returns></returns>
        private string SimStartLogMessage( MrktSimDBSchema.simulationRow sim ) {
            MrktSimDBSchema.scenarioRow scenario = sim.scenarioRow;
            MrktSimDBSchema.Model_infoRow model = scenario.Model_infoRow;
            MrktSimDBSchema.projectRow project = (MrktSimDBSchema.projectRow)model.GetParentRow( "project_Model_info" );

            return String.Format( simStartLogMessage, sim.name, sim.start_date.ToString( "d/M/yy" ), sim.end_date.ToString( "d/M/yy" ), sim.type,
                scenario.name, model.model_name, project.name );
        }

        /// <summary>
        /// Track how many sims we are editing
        /// </summary>
        public int NumOpenSims = 0;

        /// <summary>
        /// Opens the edit dialog for the given simulation.
        /// </summary>
        /// <param name="sim"></param>
        private void editSim( MrktSimDBSchema.simulationRow sim ) {

            string who = null;

            if( node.Db.SimDeleted( sim.id ) ) {

                ((MrktSim)this.ParentForm).ResetUI();

                return;
            }
            else if( node.Db.SimulationLocked( sim.id, out who ) ) {
                MessageBox.Show( MrktSim.Message( "Simulation.Locked by: " + who ) );
            }
           
            Form dlg = null;

            SimulationDb simDb = null;

            if( sim.type == (byte)SimulationDb.SimulationType.Calibration ) {
                CallibrationDb calDb = new CallibrationDb();
                calDb.Connection = Node.Db.Connection;
                calDb.Id = sim.id;

                calDb.Open();
                calDb.Refresh();

                CalibrationEditor calEdit = new CalibrationEditor();
                calEdit.Db = calDb;

                dlg = calEdit;
                simDb = calDb;
            }
            else {
                simDb = new SimulationDb();
                simDb.Connection = Node.Db.Connection;
                simDb.Id = sim.id;

                simDb.Open();
                simDb.Refresh();

                SimulationEditor simEdit = new SimulationEditor();
                simEdit.Db = simDb;

                dlg = simEdit;
            }

            dlg.FormClosed += new FormClosedEventHandler( simulation_FormClosed );

            dlg.Show();
            ++NumOpenSims;
        }

        void simulation_FormClosed( object sender, FormClosedEventArgs e ) {

            --NumOpenSims;

            SimulationDb simDb = null;
            SimulationEditor simEdit = sender as SimulationEditor;
            if( simEdit != null ) {
                simDb = simEdit.Db;
            }

            if( simDb == null ) {
                CalibrationEditor calEdit = sender as CalibrationEditor;
                simDb = calEdit.Db;
            }

            if( simDb != null ) {
                simDb.Close();
            }

            Node.Db.RefreshTable( Node.Db.Data.simulation );

            this.Refresh();
        }

        /// <summary>
        /// Opens the edit dialog for the scurrently-elected simulation.
        /// </summary>
        /// <param name="sim"></param>
        private void editSim() {
            MrktSimDBSchema.simulationRow sim = GetSelectedSim();

            if( sim == null ) {
                return;
            }

            editSim( sim );

        }
        #endregion

        #region scenario methods

        private void newSim() {
            CreateSimulation dlg = new CreateSimulation();

            SimulationDb.SimulationType[] exludeTypes = null;


            if( ((MrktSim)this.ParentForm).CalibrationControlsVisible == false ) {
                exludeTypes = new SimulationDb.SimulationType[] { 
                    SimulationDb.SimulationType.Calibration, 
                    SimulationDb.SimulationType.Optimize};
            }
            else {
                exludeTypes = new SimulationDb.SimulationType[] { 
                    SimulationDb.SimulationType.Optimize};
            }

            dlg.DisAllowedType( exludeTypes );

            DialogResult rslt = dlg.ShowDialog( this );

            if( rslt == DialogResult.Cancel )
                return;

            MrktSimDBSchema.simulationRow sim = Node.Db.CreateStandardSimulation( Node.Scenario, dlg.Name );

            sim.type = (byte)dlg.Type;

            Node.Db.Update();

            int id = sim.id;

            Node.RefreshTree();

            Refresh();

            editSim( sim );
        }

        private void scenarioName_DoubleClick( object sender, EventArgs e ) {
            ((MrktSim)this.ParentForm).ActivateNode( Node );

        }

        private void viewScenarioResults() {
            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.ScenarioID = Node.Id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm( MrktSim.DevlMode );

            dlg.Db = db;

            dlg.Show();
        }

        private void renameScenario() {

            if( node.Db.ScenarioDeleted( node.Scenario.scenario_id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();

                return;
            }

            NameAndDescr dlg = new NameAndDescr();

            dlg.Type = "Scenario";

            dlg.ObjDescription = node.Scenario.descr;

            dlg.ObjName = node.Scenario.name;

            dlg.Editing = true;

            DialogResult rslt = dlg.ShowDialog();

            if( rslt == DialogResult.OK ) {
                // rename
                Node.Scenario.name = dlg.ObjName;
                Node.Scenario.descr = dlg.ObjDescription;

                // write to disk
                Node.Db.Update();

                if( Node.ActiveNode )
                    this.Refresh();
                else
                    Node.ParentNode.Control.Refresh();
            }
        }
        #endregion

        #region results grid

        /// <summary>
        /// Pops up the sumulation status dialog for the selected result (sim_queue) item.
        /// </summary>
        private void viewSimProcess() {
            MrktSimDBSchema.results_statusRow row = GetSelectedResult();

            if (row == null)
            {
                return;
            }

            MrktSimDBSchema.simulationRow sim = Node.Db.Data.simulation.FindByid(row.simulation_id);

            SimStatus dlg = new SimStatus(Application.StartupPath, MarketSimSettings.Settings<MrktSim.ClientSettings>.Value.ConnectFile, sim);

            dlg.Show();
        }


        /// <summary>
        /// Allows the user to delete the selected results item(s).
        /// </summary>
        private void deleteResults() {
            MrktSimDBSchema.results_statusRow[] selectedResults = GetSelectedResults();
            int nRes = selectedResults.Length;

            string msg = confirmResultDeleteMsg;
            if( nRes > 1 ) {
                msg = String.Format( confirmResultsDeleteMsg, nRes );
            }
            ConfirmDialog cdlg = new ConfirmDialog( msg, confirmResultDeleteTitle );
            cdlg.SelectWarningIcon();
            cdlg.SetOkCancelButtonStyle();
            cdlg.Width -= 240;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteResultsNow );

            // update the status while the background thread runs
            string statusMsg = String.Format( deleteResultsStatus, node.ToString() );
            status.UpdateUIAndContinue( deleteResults_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void deleteResults_Part2() {
            this.Refresh();

            ClearStatusDisplay();

            CompletionDialog dlg = new CompletionDialog( deleteResultsDoneMsg );
            dlg.ShowDialog();
        }

        private void DeleteResultsNow() {
            MrktSimDBSchema.results_statusRow[] selectedResults = GetSelectedResults();

            foreach (MrktSimDBSchema.results_statusRow row in selectedResults)
            {
                Node.Db.DeleteSimQueue( row.run_id );
            }
        }

        private void ClearStatusDisplay()
        {
            Status.ClearStatus( this );
        }

        /// <summary>
        /// Opens up the window to display the warnings for the selected sim run
        /// </summary>
        private void viewSimulationLog() {
            MrktSimDBSchema.results_statusRow row = GetSelectedResult();

            if (row == null)
            {
                return;
            }

            SimLogDb logDb = new SimLogDb();

            logDb.Connection = Node.Db.Connection;


            logDb.RunID = row.run_id;
            logDb.Id = row.simulation_id;
            
            logDb.Refresh();

            SimLog dlg = new SimLog();

            dlg.Db = logDb;

            dlg.ShowDialog();
        }

        /// <summary>
        /// Opens the Results form for the selected simulation.
        /// </summary>
        private void viewSelectedSimResults() {
            MrktSimDBSchema.simulationRow sim = GetSelectedSim();
            if( sim == null ) {
                return;
            }

            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.SimID = sim.id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm( MrktSim.DevlMode );

            dlg.Db = db;

            dlg.Show();
        }

        /// <summary>
        /// Opens the results form for the selected simulation run.
        /// </summary>
        private void viewSelectedResults() {
            MrktSimDBSchema.results_statusRow row = GetSelectedResult();

            if (row == null)
                return;

            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = Node.Scenario.model_id;
            db.SimID = row.simulation_id;

            db.Refresh();
            ResultsForm dlg = new ResultsForm( MrktSim.DevlMode );

            dlg.Db = db;

            dlg.Show();
        }

        #endregion

        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
        }

        /// <summary>
        /// This method is called just before the Scenario popup menu is displayed
        /// </summary>
        private void SetScenarioMenuItemEnabling() {
            scenarioLink.EnableAllLinks();
        }

        /// <summary>
        /// This method is called just before the Simulations popup menu is displayed
        /// </summary>
        private void SetSimMenuItemEnabling() {
            bool calSim = false;

            bool enableResults = true;
            bool enableEdit = true;
            bool enableRun = true;
            bool enableCopy = true;
            bool enableDelete = true;
            bool enableDequeue = true;

            foreach( MrktSimDBSchema.simulationRow sim in GetSelectedSims() ) {
                if( sim != null && sim.type == (byte)SimulationDb.SimulationType.Calibration ) {
                    calSim = true;
                }

                MrktSimDBSchema.simulation_statusRow simState = simulation_statusTable.FindBysimulation_id( sim.id );

                if( simState == null || simState.sim_num != 1 ) {
                    enableDequeue = false;
                }
            }

            int nsel = GetSelectedSims().Length;

            if( nsel == 0 ) {
                // no selection
                enableResults = false;
                enableEdit = false;
                enableRun = false;
                enableCopy = false;
                enableDelete = false;
                enableDequeue = false;
            }
            else if( nsel > 1 ) {
                // multiple sims selected
                enableEdit = false;
                enableCopy = false;
            }

            if(calSim && ((MrktSim)this.ParentForm).CalibrationControlsVisible == false ) {

                // when cal sim selected when cal controls aren't visible
                enableEdit = false;
                enableRun = false;
                enableCopy = false;
                enableDelete = false;
            }

            simLink.EnableAllLinks();
            if( enableResults == false ) {
                simLink.DisableLink( viewResItemString );
            }
            if( enableEdit == false ) {
                simLink.DisableLink( editSimItemString );
            }
            if( enableRun == false ) {
                simLink.DisableLink( runSimItemString );
            }
            if( enableCopy == false ) {
                simLink.DisableLink( copySimItemString );
            }
            if( enableDelete == false ) {
                simLink.DisableLink( deleteSimItemString );
            }
            if( enableDequeue == false ) {
                simLink.DisableLink( deQueueSimItemString );
            }
        }

        /// <summary>
        /// This method is called just before the Results popup menu is displayed
        /// </summary>
        private void SetResultsMenuItemEnabling() {
            if( resultsDataGridView.SelectedRows.Count == 0 ) {
                resultsLink.DisableLinks( viewResItemString, viewLogItemString, dbResItemString, viewStatusItemString );
            }
            else {
                resultsLink.EnableAllLinks();
            }
        }

        private void ioPlotsButton_Click( object sender, EventArgs e ) {
            MrktSimDBSchema.results_statusRow res = GetSelectedResult();
            if( res == null ) {
                return;   // need to have 1 selected result item
            }
            MrktSimDBSchema.simulationRow sim = Node.Db.Data.simulation.FindByid( res.simulation_id );

            string titleFormat = String.Format( " - {0}, {1}", sim.name, res.run_name );
            titleFormat = "{0}" + titleFormat;
            string subtitle = res.run_time.ToString();
            Results.Standardized.MarketingIOPlotLauncher spl =
                 new Results.Standardized.MarketingIOPlotLauncher( sim.start_date, sim.end_date, Node.Scenario.model_id, res.run_id, titleFormat );
            spl.SubTitle = subtitle;
            spl.Connection = Node.Db.Connection;
            spl.ShowDialog();
        }
    }
}
