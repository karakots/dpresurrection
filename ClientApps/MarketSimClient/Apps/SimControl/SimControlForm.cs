using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Threading;
using System.Diagnostics; // process

using System.IO; // file read write


using SimControlMethods;
using MarketSimSettings;
using MrktSimDb;
using DPLicense;

namespace SimControl
{
    public partial class SimControlForm : Form
    {
        private string runString = "Running";
        private string configString = "Click on settings button to configure";
        private string invalidLicString = "Invalid License";
        private string stopString = "Stopped";
        private string lockedString = "Database locked by SimController";
        private string warnUnlockString = "This operation will unlock the database in order to run simulaitons.\n\r Please stop any other simulation controllers that may be running";

        public SimControlForm(bool mrktSimStartUp, string fileName)
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.RunFromMarketSim = mrktSimStartUp;

           // this.killAll.Visible = false;

            //eventLog.Source = "SimController";
            //eventLog.Log = "Simulation Control";

            Settings<SimControlSettings>.Read("SimControlSettings.config");

            DbConnectFile = fileName;

            if (DbConnectFile == null)
            {
                DbConnectFile = Settings<SimControlSettings>.Value.ConnectFile;
            }

            // sim control configure
            // MarketSim process

            simControlArray = new ArrayList();
            simThreadArray = new ArrayList();    

            int w = Settings<SimControlSettings>.Value.FormBounds.Width;
            if( w >= this.MinimumSize.Width ) {
                this.Bounds = Settings<SimControlSettings>.Value.FormBounds;
            }

            // try to connect
            SimController.OpenModel( Application.StartupPath, DbConnectFile );

            if( SimController.ModelName != null ) {
                SimController.Model.Refresh();

                if( !RunFromMarketSim ) {
                    initDisplayAdaptors();
                }
            }

            // checkStatus
            checkStatus();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            SimControlForm form = new SimControlForm(false, null);

            Application.Run(form);
        }

        #region Private Fields

        private DataView processedGridView;
        private DataView runningGridView;

        private bool RunFromMarketSim = false;

        private SimController.SimState simState = SimController.SimState.NotRunning;

        // keeps track of the simControl objects
        private ArrayList simControlArray;

        // keeps track of the corresponding threads
        private ArrayList simThreadArray;

        private string DbConnectFile = null;

       // private bool lockedDb = false;
        private bool simLocked = false;

        #endregion

        #region Public properties, methods and queries


        public bool SimLocked {
            get {
                return simLocked;
            }

            set {
                simLocked = value;
            }
        }

        /// <summary>
        /// True if sims are running
        /// </summary>
        /// <returns></returns>
        public bool RunningSims() {
            return (RunningSimsCount() > 0);
        }
     
        /// <summary>
        /// returns number of running simulations
        /// </summary>
        /// <returns></returns>
        public int RunningSimsCount()
        {
            // make sure alll simulations are done or died

            bool done = false;
            while (!done)
            {
                done = true;
                foreach (Thread proc in simThreadArray)
                {
                    if (!proc.IsAlive)
                    {
                        simThreadArray.Remove(proc);
                        done = false;
                        break;
                    }
                }
            }

            return simThreadArray.Count;
        }

  
        /// <summary>
        /// Check sim queue and run sim if found
        /// </summary>
        /// <returns></returns>
        public bool CheckQueue()
        {
            // main event loop

            if (simState != SimController.SimState.Running)
            {
                return false;
            }

            SimController.Model.Refresh();

            if( simControlArray.Count == 0 ) {
                this.numRunninglabel.Text = "No simulations running";
            }
            else if( simControlArray.Count == 1 ) {
                this.numRunninglabel.Text = "1 simulation running";
            }
            else {
                this.numRunninglabel.Text = simControlArray.Count + " simulations running";
            }

            MrktSimDBSchema.simulationRow simToRun = SimController.SimulationToRun();

            if( simToRun != null ) {
                this.numQueuedLabel.Text = "Simulations in queue";

            }
            else {
                this.numQueuedLabel.Text = "No simulations in queue";
            }

            if (simControlArray.Count >= Settings<SimControlSettings>.Value.NumSims)
            {
                this.numRunninglabel.Text += " (maximum allowed)";
                return false;
            }

            string user = Settings<SimControlSettings>.Value.UserName;
            string key = Settings<SimControlSettings>.Value.LicenseKey;
            if( !CheckLicense( user, key ) ) {
                StatusLabel.Text = invalidLicString;
                if( RunFromMarketSim ) {
                    MessageBox.Show( "Your license is either invalid or expired, no sims will be run.", "Invalid License", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                }
                else {
                    StatusLabel.Text = invalidLicString;
                    simState = SimController.SimState.NotConfigured;
                    this.pauseSims();
                }

                return false;
            }

            // check if there are scenarios to run
            bool rval = false;
        
            if (simToRun != null)
            {
                // check if sim control thread is finished
                // start up a new simcontrol as needed
                // start up the simulation

                // clean up process array

                RunSim(simToRun);
                rval = true;
            }
        
            return rval;
        }

       /// <summary>
       /// invoke setup dialogue
       /// and check status
       /// </summary>
        public void SimConfig()
        {
            this.pauseSims();

            // unlock db as needed
            lockDatabase( false );

            string tmpConnectFile = setUpDlg();

            // if successfully opened then refresh
            if( SimController.ModelName != null ) {
                SimController.Model.Refresh();
            }

            if( !RunFromMarketSim ) {
                initDisplayAdaptors();

                DbConnectFile = tmpConnectFile;
                Settings<SimControlSettings>.Value.ConnectFile = DbConnectFile;
                Settings<SimControlSettings>.Save();
            }

            // if all ok check the queue immediately
            // this is because if MarketSim checked status there may be items in queue
            if( checkStatus() ) {
                CheckQueue();
            }
        }

        private void stopProcesses() {

            if( SimController.ModelName != null ) {
                SimController.Model.Refresh();
            }

           
            // save current state info
            SimController.SimState origState = simState;
            string origStatus = this.StatusLabel.Text;

            statusImage.Enabled = false;
            this.timer.Enabled = false;
            this.StatusLabel.Text = stopString;
            simState = SimController.SimState.Stopping;

            foreach( Thread thread in simThreadArray ) {
                //    if (thread.ThreadState == System.Threading.ThreadState.Running ||
                //        thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                //    {
                try {
                    thread.Abort();
                }
                catch( Exception ) { }
                //}
            }

            simThreadArray.Clear();

            foreach( SimController simControl in simControlArray ) {
                if( !simControl.Done && !simControl.Sim.HasExited ) {
                    try {
                        simControl.Sim.Kill();
                    }
                    catch( Exception ) { }
                }
            }


            simControlArray.Clear();

            simState = SimController.SimState.NotRunning;

            // screeeeeech!!
            SimController.Model.DequeueAllSims();
            SimController.Model.AllSimsStopped();


            if( SimController.ModelName != null ) {
                SimController.Model.Refresh();
            }


            // after stopping we start right up again

            simState = origState;
            StatusLabel.Text = origStatus;

            if( simState == SimController.SimState.Running ) {
                this.startSimsRunning();
            }
        }

        /// <summary>
        ///  stop all running simulations on this machine
        /// </summary>
        public void KillAll()
        {

            stopProcesses();

            string execFile = Settings<SimControlSettings>.Value.SimDirectory + @"\" + Settings<SimControlSettings>.Value.SimFile;

            try
            {
                // kill any simulations running
                Process[] procArray = System.Diagnostics.Process.GetProcesses();

                foreach (Process proc in procArray)
                {
                    try
                    {
                        if( proc.ProcessName == "System" || proc.ProcessName == "Idle" )
                            continue;

                        if( proc.HasExited )
                            continue;


                        if( proc.MainModule != null && proc.MainModule.FileName == execFile )
                        {
                            try
                            {
                                proc.Kill();
                                //  eventLog.WriteEntry("Stopped process: " + proc.MainModule.FileName);
                            }
                            catch( Exception )
                            {
                                //   eventLog.WriteEntry("Error Stopping Simulation: " + killError.Message, EventLogEntryType.Error);
                            }
                        }
                    }
                    catch( Exception )
                    {
                      //  MessageBox.Show( "Error stopping " + execFile + ": " + oops.Message );
                    }
                }
            }
            catch (Exception )
            {
                // MessageBox.Show("Error stopping " + execFile + ": " + noCanDo.Message);
            }
        }

        /// <summary>
        /// Check for license status
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="license_key"></param>
        /// <returns></returns>
        public static bool CheckLicense(string user_name, string license_key)
        {
            LicComputer lic = new LicComputer();
            lic.UserName = user_name;
            lic.LicenseKey = license_key;
            DateTime exp = lic.ExpirationDate;
            DateTime today = DateTime.Today;
            if (exp < today)
            {
                return false;
            }
            return true;
        }
    
        #endregion

 
        #region Thread callbacks

        public delegate void SimQueueStateChanged(int run_id);
        public event SimQueueStateChanged UpdateSimQueueState;

        public delegate void SimStateChanged(int id);
        public event SimStateChanged UpdateSimState;

        private void simControl_SimUpdated(Object sender, SimController.CancelSim cancelSim, int run_id)
        {
            if (this.InvokeRequired)
            {
                SimControlMethods.SimController.Refresh refresh = new SimControlMethods.SimController.Refresh(simControl_SimUpdated);
                this.Invoke(refresh, new object[] { sender, cancelSim, run_id });
            }
            else
            {
                if (!RunFromMarketSim)
                {
                    this.Refresh();

                    if (simState == SimController.SimState.Stopping)
                    {
                        cancelSim.Cancel = true;
                    }
                }

                if (UpdateSimQueueState != null)
                {
                    UpdateSimQueueState(run_id);
                }
            }
        }

        private void simControl_ReportFinished(int id)
        {
            if (this.InvokeRequired)
            {
                SimControlMethods.SimController.SimDone done = new SimControlMethods.SimController.SimDone(simControl_ReportFinished);
                this.Invoke(done, new object[] {id});
            }
            else
            {
                bool done = false;
                while (!done)
                {
                    done = true;
                    foreach (SimController simControl in simControlArray)
                    {
                        if (simControl.Done)
                        {
                            simControlArray.Remove(simControl);
                            done = false;
                            break;
                        }
                    }

                    foreach (Thread proc in simThreadArray)
                    {
                        if (!proc.IsAlive)
                        {
                            simThreadArray.Remove(proc);
                            done = false;
                            break;
                        }
                    }
                }

                if (UpdateSimState != null)
                    UpdateSimState(id);

                // before we exit check if we need to queue up another
                // this keeps the proper  number of threads active
                this.CheckQueue();
            }
        }

        #endregion

        #region UI events

        private void settingsButton_Click( object sender, EventArgs e ) {
            SimConfig();
        }

        private void stopSimsButton_Click( object sender, EventArgs e ) {

            KillAll();
        }

        private void SimControlForm_FormClosing( object sender, FormClosingEventArgs e ) {
            lockDatabase( false );
            if( !this.RunFromMarketSim && RunningSims() ) {
                MessageBox.Show( "Please stop simulations before closing" );

                e.Cancel = true;
                return;
            }

            Settings<SimControlSettings>.Save();
        }

        private void timer_Tick( object sender, System.EventArgs e ) {

            statusImage.Enabled = !statusImage.Enabled;

            CheckQueue();

            RefreshSimulationStatusDisplay();
            RefreshResultStatusDisplay();

        }

        #endregion

        #region Simulation Control

        private bool RunSim( MrktSimDBSchema.simulationRow simToRun ) {
            if( simControlArray.Count >= Settings<SimControlSettings>.Value.NumSims ) {
                return false;
            }

            Process newProcess = new Process();
            newProcess.StartInfo.FileName = Settings<SimControlSettings>.Value.SimFile;
            newProcess.StartInfo.WorkingDirectory = Settings<SimControlSettings>.Value.SimDirectory;
    
            // new engine does not have to be told 
           // newProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
         

            // the sim controller launches the simulation
            SimController newControl = new SimController( simToRun, DbConnectFile, Application.StartupPath, newProcess );

            newControl.SimUpdated += new SimControlMethods.SimController.Refresh( simControl_SimUpdated );

            newControl.ReportFinished += new SimControlMethods.SimController.SimDone( simControl_ReportFinished );

            if( newControl.QueuecurrentSimulation() ) {
                simControlArray.Add( newControl );

                //Thread newThread = new Thread(new ThreadStart(newControl.Run));

                Thread newThread = new Thread( newControl.Run );

                newThread.Start();

                simThreadArray.Add( newThread );


                if( UpdateSimState != null )
                    UpdateSimState( simToRun.id );
            }

            return true;
        }

        private void startSimsRunning() {
            if( SimLocked ) {
                return;
            }

            // ensure running state
            simState = SimController.SimState.Running;
            this.StatusLabel.Text = runString;

            if( !RunFromMarketSim ) {
                this.timer.Enabled = true;
            } 
        }

        private void pauseSims() {

            if( RunFromMarketSim || SimLocked ) {
                return;
            }

            timer.Enabled = false;

            statusImage.Enabled = false;

            if( simState == SimController.SimState.Running ) {
                this.StatusLabel.Text = stopString;


                simState = SimController.SimState.NotRunning;
            }


            if( SimController.ModelName != null ) {
                SimController.Model.Refresh();
            }
        }

        #endregion


        #region Setup and Status
        private string setUpDlg()
        {
            SimControl.Dialogues.Setup dlg = new SimControl.Dialogues.Setup(this.DbConnectFile, this.RunFromMarketSim);

            dlg.ShowDialog();

            

            return dlg.DbConnectFile;
        }

        private FileInfo checkForDataSource()
        {
            // check for engine connect file
            string msEngineConnectFile = Settings<SimControlSettings>.Value.SimDirectory + @"\connect\" + SimController.EngineConnectFile;
            return new FileInfo(msEngineConnectFile);
        }

        private bool checkStatus()
        {
            statusImage.Enabled = false;
            stopSimsButton.Visible = true;
            unlockButton.Visible = false;
            SimLocked = false;

            StatusLabel.Text = null;

            if( !RunFromMarketSim ) {
                // these items are not needed when run from the client
                // configure refresh rate
                if( Settings<SimControlSettings>.Value.RefreshRate > 0 ) {
                    timer.Interval = 1000 * Settings<SimControlSettings>.Value.RefreshRate;
                }
                else {
                    timer.Interval = 2000;
                }

                // filter grids based on settings
                TimeSpan span = new TimeSpan( 7 * Settings<SimControlSettings>.Value.NumWeeks, 0, 0, 0 );
                DateTime then = DateTime.Now - span;

                //display any run that is done
                //or ran yesterday
                //but not weeks ago


                if( processedGridView != null ) {
                    processedGridView.RowFilter = "status = 'done ' AND (run_time IS NULL OR run_time > '" + then.ToShortDateString() + "')";
                }

                // what am i
                this.Text = "Simulation Control: " + SimController.ModelName;

                // if no connection then
                // disable certain operations
                if( SimController.ModelName == null ) {
                    // cannot perform these operations
                    this.stopSimsButton.Enabled = false;
                }
                else {
                    stopSimsButton.Enabled = true;
                }
            }

            // set sim state
            // check for engine status
            bool engineExists = CheckForEngine();
            FileInfo fi = checkForDataSource();

            // if no db or engine not set or no data source
            if (SimController.ModelName == null || !engineExists || !fi.Exists)
            {
                StatusLabel.Text = this.configString;

                this.simState = SimController.SimState.NotConfigured;

                return false;
            }

            // check if database is already locked
            // check if database is locked
            foreach( MrktSimDBSchema.projectRow proj in SimController.Model.Data.project ) {
                if( proj.locked ) {
                    SimLocked = true;
                }
            }

            if( SimLocked ) {
                StatusLabel.Text = lockedString;
                stopSimsButton.Visible = false;
                unlockButton.Visible = true;
                return false;
            }
            
           // all is ok
            StatusLabel.Text = stopString;

            // lock db as needed
            lockDatabase( true );

            this.startSimsRunning();

            return true;
        }

        static private bool CheckForEngine()
        {
            string msEngineFile = Settings<SimControlSettings>.Value.SimDirectory + @"\" + Settings<SimControlSettings>.Value.SimFile;
            FileInfo fi = new FileInfo(msEngineFile);
            if (Settings<SimControlSettings>.Value.SimFile != null && fi.Exists)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region display update

        private void initDisplayAdaptors() {

            // sims
            
            System.Data.OleDb.OleDbCommand simStatCommand = new System.Data.OleDb.OleDbCommand();
            simStatCommand.CommandTimeout = 0;
            simStatCommand.Connection = SimController.Model.Connection;
            MrktSimDb.SqlQuery.InitializeSimulationStatusSelect( simStatCommand );       // initialize the SELECT query for the simulation_status view
           
            // set up the SQL adapter for the simulation status 
            simStatusAdapter = new System.Data.OleDb.OleDbDataAdapter();
            simStatusAdapter.SelectCommand = simStatCommand;

            simulation_statusTable = new MrktSimDBSchema.simulation_statusDataTable();
            DataView simView = new DataView( simulation_statusTable );
            simView.RowFilter = "sim_num <> -1";
            this.simDataGridView.DataSource = simView;

            // running sims
            System.Data.OleDb.OleDbCommand resStatCommand = new System.Data.OleDb.OleDbCommand();
            resStatCommand.CommandTimeout = 0;
            resStatCommand.Connection = SimController.Model.Connection;
            MrktSimDb.SqlQuery.InitializeResultsStatusSelect( resStatCommand );           // initialize the SELECT query for the results_status view

            // set up the SQL adapter for the results status 
            resultsStatusAdapter = new System.Data.OleDb.OleDbDataAdapter();
            resultsStatusAdapter.SelectCommand = resStatCommand;

            results_statusTable = new MrktSimDBSchema.results_statusDataTable();
            runningGridView = new DataView( results_statusTable );
            runningGridView.RowFilter = "status <> 'done '";
            this.runningGrid.DataSource = runningGridView;

            processedGridView = new DataView( results_statusTable );
            processedGridView.RowFilter = "status = 'done '";
            processedGrid.DataSource = processedGridView;
        }

        private System.Data.OleDb.OleDbDataAdapter simStatusAdapter;
        private MrktSimDBSchema.simulation_statusDataTable simulation_statusTable;

        private System.Data.OleDb.OleDbDataAdapter resultsStatusAdapter;
        private MrktSimDBSchema.results_statusDataTable results_statusTable;

        private bool RefreshSimulationStatusDisplay() {

            bool rval = false;
    
            try {
                // update the data
                simDataGridView.SuspendLayout();

                MrktSimDBSchema.simulation_statusDataTable tmpTbl = new MrktSimDBSchema.simulation_statusDataTable();

                simStatusAdapter.Fill( tmpTbl );

                // remove items not in current table
                foreach( MrktSimDBSchema.simulation_statusRow simStat in simulation_statusTable ) {

                    if( tmpTbl.FindBysimulation_id(simStat.simulation_id) == null ) {
                        simStat.Delete();
                    }
                }
                
                simulation_statusTable.Merge( tmpTbl );

                simulation_statusTable.AcceptChanges();

                // simulation_statusTable.Clear();
                // ((DataView)simDataGridView.DataSource).RowFilter = "sim_num <> -1";
                simDataGridView.ResumeLayout();
                
            }
            catch( Exception oops ) {
                this.statusBar.Text = "Exception Filling simulation_statusTable!\n" + oops.ToString();
                return rval;
            }

            return rval;
        }
        /// <summary>
        /// Updates the results status display to reflect the current status.
        /// </summary>
        /// <param name="calledByTimer"></param>
        /// <returns>true if timer started - false elsewise</returns>
        private bool RefreshResultStatusDisplay() {
            try {
                // update the data
                runningGrid.SuspendLayout();
                MrktSimDBSchema.results_statusDataTable tmpTbl = new MrktSimDBSchema.results_statusDataTable();
                resultsStatusAdapter.Fill( tmpTbl );

                foreach( MrktSimDBSchema.results_statusRow resRow in results_statusTable ) {
                    if( tmpTbl.FindByrun_id( resRow.run_id ) == null ) {
                        resRow.Delete();
                    }
                }

                results_statusTable.Merge( tmpTbl );
                results_statusTable.AcceptChanges();

                runningGrid.ResumeLayout();
            }
            catch( Exception oops ) {
               this.statusBar.Text = "Exception Filling results_statusTable!\n" + oops.ToString();
                return false;
            }

            for( int i = 0; i < runningGrid.RowCount; i++ ) {
                DataGridViewRow row = runningGrid.Rows[ i ];
                int runID = (int)row.Cells[ "RunIdCol" ].Value;

                string stat = (string)row.Cells[ "ResultsStatusCol" ].Value;
                string curProgress = (string)row.Cells[ "CurrentStatusCol" ].Value;
                //DateTime progressStart = new DateTime();
                //DateTime progressSimStart = new DateTime();
                //DateTime progressEnd = new DateTime();
                DateTime progressDate = new DateTime();

                //bool isRunning = false;

                if( stat == "running" ) {
                    //isRunning = true;


                    // put a progress bar in place of the status details while a sim is running
                    //int progressColWid = runningGrid.Columns[ "CurrentStatusCol" ].Width;
                    //int simID = (int)row.Cells[ "ResultsSimulationIdCol" ].Value;
                    //int scenarioID = (int)row.Cells[ "ResultsScenarioIdCol" ].Value;
                    //MrktSimDBSchema.scenarioRow scen = SimController.Model.Data.scenario.FindByscenario_id( scenarioID );
                    //MrktSimDBSchema.simulationRow sim = SimController.Model.Data.simulation.FindByid( simID );
                    //MrktSimDBSchema.Model_infoRow mod = SimController.Model.Data.Model_info.FindBymodel_id( scen.model_id );

                    //progressStart = mod.start_date;
                    //if( mod.checkpoint_valid ) {
                    //    double ckpRatio = mod.checkpoint_scale_factor / sim.scale_factor;
                    //    if( ckpRatio >= 0.995 && ckpRatio <= 1.005 ) {
                    //        progressStart = mod.checkpoint_date;
                    //    }
                    //}
                    //progressSimStart = sim.start_date;
                    //progressEnd = sim.end_date;
                    progressDate = (DateTime)row.Cells[ "CurrentDateCol" ].Value;
                    string runStat = String.Format( "run: {0}", progressDate.ToString( "MM/dd/yy" ) );
                    row.Cells[ "ResultsStatusCol" ].Value = runStat;
                }
            }

            return true;
        }

        #endregion

        #region misc utilities

        private void lockDatabase(bool val) {

            if( SimLocked || RunFromMarketSim || SimController.ModelName == null ) {
                return;
            }

            foreach( MrktSimDBSchema.projectRow proj in SimController.Model.Data.project ) {
                proj.locked = val;
            }

            SimController.Model.Update();
        }

        
        #endregion

        #region Settings
        /// <summary>
        /// Everyone should have Settings.
        /// </summary>
        [SerializableAttribute]
        public class SimControlSettings
        {
            private int numWeeks = 12;
            public int NumWeeks
            {
                get
                {
                    return numWeeks;
                }

                set
                {
                    numWeeks = value;
                }
            }


            private int numSims = 3;
            public int NumSims
            {
                get
                {
                    return numSims;
                }

                set
                {
                    numSims = value;
                }
            }

            private string simDirectory = null;
            public string SimDirectory
            {
                get
                {
                    return simDirectory;
                }

                set
                {
                    simDirectory = value;
                }
            }

            private string simFile = null;
            public string SimFile
            {
                get
                {
                    return simFile;
                }

                set
                {
                    simFile = value;
                }
            }

            private string connectFile = null;
            public string ConnectFile
            {
                get
                {
                    return connectFile;
                }

                set
                {
                    connectFile = value;
                }
            }


            private int refreshRate = 2;
            public int RefreshRate
            {
                get
                {
                    return refreshRate;
                }

                set
                {
                    refreshRate = value;
                }
            }

            private Rectangle formBounds = new Rectangle( 0, 0, 0, 0 );
            public Rectangle FormBounds 
            {
                get {
                    return formBounds;
                }

                set {
                    formBounds = value;
                }
            }

            private string user_name = "DPUSER";
            public string UserName
            {
                get
                {
                    return user_name;
                }

                set
                {
                    user_name = value;
                }
            }

            private string license_key = "INVALIDKEY";
            public string LicenseKey
            {
                get
                {
                    return license_key;
                }

                set
                {
                    license_key = value;
                }
            }



            // this is a singleton
            public SimControlSettings()
            {
            }
        }

        #endregion

        private void unlockButton_Click( object sender, EventArgs e ) {

            DialogResult rslt = MessageBox.Show( warnUnlockString, "Unlock Database?", MessageBoxButtons.OKCancel );

            if( rslt != DialogResult.OK ) {
                return;
            }

            SimLocked = false;

            this.lockDatabase( false );

            checkStatus();
        }

        

     
  

      
    }
}