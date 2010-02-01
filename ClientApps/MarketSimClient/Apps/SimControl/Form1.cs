using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Threading;
using System.Diagnostics; // process

using System.IO; // file read write

using SimControlMethods;
using MarketSimSettings;
using MrktSimDb;

namespace SimControl
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;

		public Form1(bool mrktSimStartUp, string fileName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.DoubleBuffered = true;

            this.hideButton.Visible = false;

            this.RunFromMarketSim = mrktSimStartUp;

            this.killAll.Visible = false;
			
			eventLog.Source = "SimController";
			eventLog.Log = "Simulation Control";

            Settings<SimControlSettings>.Read("SimControlSettings.config");

            DbConnectFile = fileName;

            if (DbConnectFile == null)
            {
                DbConnectFile = Settings<SimControlSettings>.Value.ConnectFile;
            }
			
			// try to connect
            SimController.OpenModel(Application.StartupPath, DbConnectFile);

			this.Text = "Simulation Control: " + SimController.ModelName;

			// sim control configure
			// MarketSim process

			simControlArray = new ArrayList();
			simThreadArray = new ArrayList();

            //simTable = new DataTable("Simulations");

            //simTable.Columns.Add("Simulation", typeof(string));
            //simTable.Columns.Add("Result Set", typeof(string));
            //simTable.Columns.Add("Date", typeof(DateTime));
            //simTable.Columns.Add("Status", typeof(string));

			this.runningGrid.Table = SimController.Model.Data.sim_queue;
            this.processedGrid.Table = SimController.Model.Data.sim_queue;

			this.runningGrid.DescriptionWindow = false;
			this.runningGrid.AllowDelete = false;

            this.processedGrid.DescriptionWindow = false;
            this.processedGrid.AllowDelete = false;

			this.queuedGrid2.Table = SimController.Model.Data.simulation;
			this.queuedGrid2.RowFilter = "queued = 1 AND sim_num > 0";

			this.queuedGrid2.DescriptionWindow = false;
			this.queuedGrid2.AllowDelete = false;

			createTableStyle();

			bool engineConfigured = checkStatus();
			
			if (SimController.ModelName != null)
			{
				SimController.Model.Refresh();
			}

            if (SimController.ModelName == null || !engineConfigured)
            {
                // lunach settings form
                DbConnectFile = setUpDlg();
            }

            engineConfigured = checkStatus();

            if (RunFromMarketSim)
            {
                // confiure to run with MarketSim
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                this.ControlBox = false;
                this.exitItem.Visible = false;
                this.killAll.Visible = false;
                this.hideButton.Visible = true;
            }

            this.startSimsRunning();
        }

		private System.Windows.Forms.MenuItem killAll;
        private MarketSimUtilities.MrktSimGrid runningGrid;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private MarketSimUtilities.MrktSimGrid processedGrid;
        private ToolBarButton hideButton;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton2;
		private MarketSimUtilities.MrktSimGrid queuedGrid2;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.setup = new System.Windows.Forms.MenuItem();
            this.exitItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.stopItem = new System.Windows.Forms.MenuItem();
            this.killAll = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.stopButton = new System.Windows.Forms.ToolBarButton();
            this.refreshButton = new System.Windows.Forms.ToolBarButton();
            this.toolImageList = new System.Windows.Forms.ImageList(this.components);
            this.eventLog = new System.Diagnostics.EventLog();
            this.queuedGrid2 = new MarketSimUtilities.MrktSimGrid();
            this.runningGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.processedGrid = new MarketSimUtilities.MrktSimGrid();
            this.hideButton = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.setup,
            this.exitItem});
            this.menuItem1.Text = "File";
            // 
            // setup
            // 
            this.setup.Index = 0;
            this.setup.Text = "Settings...";
            this.setup.Click += new System.EventHandler(this.setup_Click);
            // 
            // exitItem
            // 
            this.exitItem.Index = 1;
            this.exitItem.Text = "Exit";
            this.exitItem.Click += new System.EventHandler(this.exitItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.stopItem,
            this.killAll});
            this.menuItem4.Text = "Simulation";
            // 
            // stopItem
            // 
            this.stopItem.Index = 0;
            this.stopItem.Text = "Stop";
            this.stopItem.Click += new System.EventHandler(this.stopItem_Click);
            // 
            // killAll
            // 
            this.killAll.Index = 1;
            this.killAll.Text = "Kill all running simulations";
            this.killAll.Click += new System.EventHandler(this.killAll_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 347);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(870, 24);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "No Simulation Running";
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.stopButton,
            this.refreshButton,
            this.toolBarButton1,
            this.toolBarButton2,
            this.hideButton});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.toolImageList;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(870, 28);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // stopButton
            // 
            this.stopButton.ImageIndex = 0;
            this.stopButton.Name = "stopButton";
            this.stopButton.ToolTipText = "Stop all simualtions";
            // 
            // refreshButton
            // 
            this.refreshButton.ImageIndex = 1;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.ToolTipText = "Refresh Simulation List";
            // 
            // toolImageList
            // 
            this.toolImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolImageList.ImageStream")));
            this.toolImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.toolImageList.Images.SetKeyName(0, "");
            this.toolImageList.Images.SetKeyName(1, "");
            this.toolImageList.Images.SetKeyName(2, "back.ico");
            // 
            // eventLog
            // 
            this.eventLog.SynchronizingObject = this;
            // 
            // queuedGrid2
            // 
            this.queuedGrid2.DescribeRow = null;
            this.queuedGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queuedGrid2.EnabledGrid = true;
            this.queuedGrid2.Location = new System.Drawing.Point(0, 0);
            this.queuedGrid2.Name = "queuedGrid2";
            this.queuedGrid2.RowFilter = null;
            this.queuedGrid2.RowID = null;
            this.queuedGrid2.RowName = null;
            this.queuedGrid2.Size = new System.Drawing.Size(870, 123);
            this.queuedGrid2.Sort = "";
            this.queuedGrid2.TabIndex = 7;
            this.queuedGrid2.Table = null;
            // 
            // runningGrid
            // 
            this.runningGrid.DescribeRow = null;
            this.runningGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runningGrid.EnabledGrid = true;
            this.runningGrid.Location = new System.Drawing.Point(0, 0);
            this.runningGrid.Name = "runningGrid";
            this.runningGrid.RowFilter = null;
            this.runningGrid.RowID = null;
            this.runningGrid.RowName = null;
            this.runningGrid.Size = new System.Drawing.Size(870, 87);
            this.runningGrid.Sort = "";
            this.runningGrid.TabIndex = 5;
            this.runningGrid.Table = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.queuedGrid2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(870, 319);
            this.splitContainer1.SplitterDistance = 123;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.runningGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.processedGrid);
            this.splitContainer2.Size = new System.Drawing.Size(870, 192);
            this.splitContainer2.SplitterDistance = 87;
            this.splitContainer2.TabIndex = 0;
            // 
            // processedGrid
            // 
            this.processedGrid.DescribeRow = null;
            this.processedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processedGrid.EnabledGrid = true;
            this.processedGrid.Location = new System.Drawing.Point(0, 0);
            this.processedGrid.Name = "processedGrid";
            this.processedGrid.RowFilter = null;
            this.processedGrid.RowID = null;
            this.processedGrid.RowName = null;
            this.processedGrid.Size = new System.Drawing.Size(870, 101);
            this.processedGrid.Sort = "";
            this.processedGrid.TabIndex = 0;
            this.processedGrid.Table = null;
            // 
            // hideButton
            // 
            this.hideButton.ImageIndex = 2;
            this.hideButton.Name = "hideButton";
            this.hideButton.ToolTipText = "Click to hide this form";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(870, 371);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Simulation Controller";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{

            Form1 form = new Form1(false, null);

			Application.Run(form);
		}


		#region fields

        public bool RunFromMarketSim = false;

        private string connectFile = null;
        public string DbConnectFile
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

		// keeps track of the simCOntrol objects
		private ArrayList simControlArray;

		// keeps track of the corresponding threads
		private ArrayList simThreadArray;
    
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem exitItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem stopItem;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.MenuItem setup;
		private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.ToolBarButton stopButton;
        private System.Windows.Forms.ImageList toolImageList;
        private System.Windows.Forms.ToolBarButton refreshButton;
		private System.Diagnostics.EventLog eventLog;
		private SimController.SimState simState = SimController.SimState.NotRunning;
		#endregion

		#region Thread callbacks
		private void simControl_SimUpdated(Object sender, SimController.CancelSim cancelSim)
		{
			if (this.InvokeRequired)
			{
				SimControlMethods.SimController.Refresh refresh = new SimControlMethods.SimController.Refresh(simControl_SimUpdated);
				this.Invoke(refresh, new object[] { sender, cancelSim });
			}
			else
			{
				this.Refresh();

				if (simState == SimController.SimState.Stopping)
				{
					cancelSim.Cancel = true;
				}
			}
		}

		private void simControl_ReportFinished()
		{
			if (this.InvokeRequired)
			{
				SimControlMethods.SimController.SimDone done = new SimControlMethods.SimController.SimDone(simControl_ReportFinished);
				this.Invoke(done);
			}
			else
			{
				bool done = false;
				while(!done)
				{
					done = true;
					foreach(SimController simControl in simControlArray)
					{
						if (simControl.Done)
						{
							simControlArray.Remove(simControl);
							done = false;
							break;
						}
					}
				}
			}
		}

		#endregion

		//private DataTable simTable;

		private void createTableStyle()
		{
			this.runningGrid.Clear();

			this.runningGrid.AddComboBoxColumn("sim_id", SimController.Model.Data.simulation, "name", "id", true);
			this.runningGrid.AddTextColumn("name", "Result Set", true);
			this.runningGrid.AddDateColumn("current_date", true);
			this.runningGrid.AddTextColumn("current_status", "Status", true);
			this.runningGrid.AddNumericColumn("elapsed_time", true);

			this.runningGrid.Reset();

            this.processedGrid.Clear();

            this.processedGrid.AddComboBoxColumn("sim_id", SimController.Model.Data.simulation, "name", "id", true);
            this.processedGrid.AddTextColumn("name", "Result Set", true);
            this.processedGrid.AddDateColumn("current_date", true);
            this.processedGrid.AddTextColumn("current_status", "Status", true);
            this.processedGrid.AddNumericColumn("elapsed_time", true);

            this.processedGrid.Reset();

			this.queuedGrid2.Clear();
			this.queuedGrid2.AddComboBoxColumn("model_id",SimController.Model.Data.Model_info, "model_name", "model_id", true);
			this.queuedGrid2.AddTextColumn("name", "Queued Simulations (waiting)", true);
			this.queuedGrid2.AddTextColumn("descr", "Description", true);
			this.queuedGrid2.Reset();
		}

		private void exitItem_Click(object sender, System.EventArgs e)
		{

			this.Close();
		}

		private void startSimsRunning()
		{
			this.statusBar.Text = "Running";
			
			this.timer.Enabled = true;

			simState = SimController.SimState.Running;
		}

		private void stopItem_Click(object sender, System.EventArgs e)
		{
			SimController.Model.Refresh();

			this.timer.Enabled = false;
			this.statusBar.Text = "Stopping simulations...";

			simState = SimController.SimState.Stopping;

			foreach(Thread thread in simThreadArray)
			{
				thread.Abort();
			}

			foreach(SimController simControl in simControlArray)
			{

                simControl.DequeueSim();

				if (!simControl.Done && !simControl.Sim.HasExited)
				{
					try
					{
						simControl.Sim.Kill();
					}
					catch(Exception){}
				}
			}

			simControlArray.Clear();
		
			simState = SimController.SimState.Stopping;
			
			simState = SimController.SimState.NotRunning;

			SimController.Model.Refresh();

            // after stopping we start right up again
            this.startSimsRunning();
		}

        private void restartSims()
        {
            this.statusBar.Text = "Running";

            simState = SimController.SimState.Running;

            foreach (Thread thread in simThreadArray)
            {
                if (thread.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    try
                    {
                        thread.Resume();
                    }
                    catch (Exception) { }
                }
            }
        }


        private void pauseSims()
        {
            this.statusBar.Text = "Paused";

           
            foreach (Thread thread in simThreadArray)
            {
                if (thread.ThreadState == System.Threading.ThreadState.Running ||
                    thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                {
                    try
                    {
                        thread.Suspend();
                    }
                    catch (Exception) { }
                }
            }

            simState = SimController.SimState.NotRunning;

            SimController.Model.Refresh();
        }

      
		private void timer_Tick(object sender, System.EventArgs e)
		{
			// main event loop

            if (simState != SimController.SimState.Running)
                return;

			queuedGrid2.Suspend = true;
			runningGrid.Suspend = true;
            processedGrid.Suspend = true;


            SimController.Model.Refresh();

			queuedGrid2.Suspend = false;
            this.runningGrid.Suspend = false;
            processedGrid.Suspend = false;

			if (simControlArray.Count >= Settings<SimControlSettings>.Value.NumSims)
			{
                if (SimController.SimulationToRun() != null)
				{
					this.statusBar.Text = "Simulation in queue. Running maximum simulations (" + simControlArray.Count + ")";
				}
				else
				{
					this.statusBar.Text = "No Simulations in queue. Running maximum simulations (" + simControlArray.Count + ")";
				}

				return;
			}

			// check if there are scenarios to run
            MrktSimDBSchema.simulationRow simToRun = SimController.SimulationToRun();
            if (simToRun != null)
			{
				// check if sim control thread is finished
				// start up a new simcontrol as needed
				// start up the simulation

				// clean up process array
		
				// this is the process that is the actual simulation
				this.statusBar.Text = "Found simulation to run";

				Process newProcess = new Process();
				newProcess.StartInfo.FileName = Settings<SimControlSettings>.Value.SimFile;
				newProcess.StartInfo.WorkingDirectory = Settings<SimControlSettings>.Value.SimDirectory;
				newProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			
				// we take the sim controller launches the simulation
                SimController newControl = new SimController(simToRun.id, DbConnectFile, Application.StartupPath, newProcess);
				newControl.SimUpdated +=new SimControlMethods.SimController.Refresh(simControl_SimUpdated);
				newControl.ReportFinished +=new SimControlMethods.SimController.SimDone(simControl_ReportFinished);

				if (newControl.QueuecurrentSimulation())
				{
					simControlArray.Add(newControl);

					Thread newThread = new Thread(new ThreadStart(newControl.Run));

					newThread.Start();

					simThreadArray.Add(newThread);
				}
			}

			if (simControlArray.Count == 0)
			{
				this.statusBar.Text = "No simulations to run";
			}
			else
			{
				this.statusBar.Text = "Running " + simControlArray.Count + " simulations";

				if (simControlArray.Count > 1)
				{
					this.statusBar.Text += "s";
				}
			}
		}


		private void setup_Click(object sender, System.EventArgs e)
        {
            this.pauseSims();

            setUpDlg();

            // check status from settings.
            checkStatus();

            if (SimController.ModelName != null)
                SimController.Model.Refresh();


            Settings<SimControlSettings>.Save();

            this.startSimsRunning();
        }

        private string setUpDlg()
		{
			SimControl.Dialogues.Setup dlg = new SimControl.Dialogues.Setup(this.DbConnectFile, this.RunFromMarketSim);

            dlg.ShowDialog();

            DbConnectFile = dlg.DbConnectFile;

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
			refreshButton.Enabled = true;
			statusBar.Text = null;

			if (Settings<SimControlSettings>.Value.RefreshRate > 0)
			{
				timer.Interval = 1000 * Settings<SimControlSettings>.Value.RefreshRate;
			}
			else
			{
				timer.Interval = 2000;
			}

			bool engineExists = CheckForEngine();
			FileInfo fi = checkForDataSource();

			if (SimController.Model.Connection == null || !engineExists || !fi.Exists)
			{	
				refreshButton.Enabled = false;
				statusBar.Text = "Use setup to configure";
				this.simState = SimController.SimState.NoSimEngine;
			}

            TimeSpan span = new TimeSpan(7 * Settings<SimControlSettings>.Value.NumWeeks, 0, 0, 0);
            TimeSpan twoDays = new TimeSpan(2, 0, 0, 0);

            DateTime then = DateTime.Now - span;
            DateTime yesterday = DateTime.Now - twoDays;

            // display any run that is done
            // or ran yesterday
            // but not weeks ago
            runningGrid.RowFilter = "status < 2 AND run_time >= '" + yesterday.ToShortDateString() + "'";
            processedGrid.RowFilter = "(status = 2 OR run_time < '" + yesterday.ToShortDateString() + "') AND " +
                " (run_time IS NULL OR run_time > '" + then.ToShortDateString() + "')";

			this.Text = "Simulation Control: " + SimController.ModelName;

			return engineExists && fi.Exists;
		}

		static public bool CheckForEngine()
		{
			string msEngineFile = Settings<SimControlSettings>.Value.SimDirectory + @"\" + Settings<SimControlSettings>.Value.SimFile;
			FileInfo fi = new FileInfo(msEngineFile);
			if (Settings<SimControlSettings>.Value.SimFile != null && fi.Exists)
			{
				return true;
			}

			return false;
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.refreshButton)
			{
				SimController.Model.Refresh();
			}

			else if (e.Button == this.stopButton)
			{
				this.stopItem_Click(null, null);
			}
            else if (e.Button == this.hideButton)
            {
                this.Visible = false;
            }
		}

        public bool RunningSims()
        {
            foreach (SimController simControl in simControlArray)
            {
                if (!simControl.Done && !simControl.Sim.HasExited)
                {
                    return true;
                }
            }

            return false;
        }

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if (RunningSims())
            {
				MessageBox.Show("Please stop simulations before closing");

				e.Cancel = true;
				return;
			}
		}

		private void killAll_Click(object sender, System.EventArgs e)
		{
			string execFile = Settings<SimControlSettings>.Value.SimDirectory + @"\" + Settings<SimControlSettings>.Value.SimFile;

			DialogResult rslt = MessageBox.Show("This will kill all processes with the name " + execFile + " do you wish to continue?", "Kill all Simulations",MessageBoxButtons.YesNo);

			if (rslt == DialogResult.No)
			{
				return;
			}

			try
			{
				// no kill any simulations running
				Process[] procArray = System.Diagnostics.Process.GetProcesses();

				foreach(Process proc in procArray)
				{
					if (proc.ProcessName == "System" || proc.ProcessName == "Idle")
						continue;

					if (proc.HasExited)
						continue;

					
					if (proc.MainModule != null && proc.MainModule.FileName == execFile)
					{
						try
						{
							proc.Kill();
							eventLog.WriteEntry("Stopped process: " + proc.MainModule.FileName);
						}
						catch(Exception killError)
						{
							eventLog.WriteEntry("Error Stopping Simulation: " + killError.Message, EventLogEntryType.Error);
						}
					}
				}
			}
			catch(Exception noCanDo)
			{
				MessageBox.Show("Error stopping " + execFile + ": " + noCanDo.Message);
			}
		}

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

          


            // this is a singleton
            public SimControlSettings()
            {
            }
        }

		#endregion
	}
}
