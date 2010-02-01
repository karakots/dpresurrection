using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics; // process
using System.Threading;
using System.Configuration;
using System.Collections.Specialized; // app settings

using System.IO;

using MrktSimDb;
using Common.Dialogs;
using Common.Utilities;
using DPLicense;
using SimControlMethods;

using MarketSimUtilities;

namespace BatchRun
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class BatchRunForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox batchedModelsBox;
		private System.Windows.Forms.GroupBox AvailableModels;
		private System.Windows.Forms.GroupBox runningSims;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.TextBox simtextBox;
		private System.Windows.Forms.Button simInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button runSimulation;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Button RemoveButton;
		private System.Windows.Forms.Button moveUp;
		private System.Windows.Forms.Button moveDown;
		private System.Windows.Forms.Button removeSim;
		
		private System.Windows.Forms.Button logButton;
		private MrktSimGrid availableGrid;
		private MrktSimGrid pendingGrid;
		private MrktSimGrid runningGrid;
		private System.Windows.Forms.GroupBox availableButtons;
		private System.Windows.Forms.GroupBox pendingButtons;
		private System.Windows.Forms.GroupBox runningButtons;
		private System.Windows.Forms.Splitter leftSplitter;
		private System.Windows.Forms.Splitter rightSplitter;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem setSimEngine;
		private System.Windows.Forms.MenuItem resetSimConnection;
		private System.Windows.Forms.MenuItem optionMenu;
		private System.ComponentModel.IContainer components = null;

		public BatchRunForm(int projectID)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// read in application settings
			MrktSimControl.AppSettings = System.Configuration.ConfigurationSettings.AppSettings;

			// perform any licence checks now

			// read in any saved settings
			Settings.Read();

			checkLicense();

			// called during installation
			if (projectID == -2)
				return;

			//check connection
			msConnect = new MSConnect(Application.StartupPath);

			string error = null;
			if (!msConnect.TestConnection(out error))
			{
				MessageBox.Show("Connection Error: " + error + " (Please use MarketSim Client to configure)");
				Close();
			}

			// open model
			modelDb = new ModelInfoDb();

			modelDb.Connection = msConnect.Connection;

			if (projectID == -1)
				projectID = openProject();

			if (projectID == -1)
				Close();

			modelDb.ProjectID = projectID;
			modelDb.Refresh();

			availableGrid.Table = modelDb.ModelData.scenario;
			availableGrid.RowFilter = "sim_num < 0";
			availableGrid.DescriptionWindow = false;
			availableGrid.AllowDelete = false;
			

		
			pendingGrid.Table = modelDb.ModelData.scenario;
			pendingGrid.RowFilter = "queued = 1 AND sim_num > 0";
			pendingGrid.Sort = "sim_num";
			pendingGrid.DescriptionWindow = false;
			pendingGrid.AllowDelete = false;

			runningGrid.Table = modelDb.ModelData.sim_queue;
			runningGrid.DescriptionWindow = false;
			runningGrid.AllowDelete = false;

			createTableStyle();

			if (availableGrid.Count == 0)
				AddButton.Enabled = false;

			if (pendingGrid.Count == 0)
			{
				this.RemoveButton.Enabled = false;
				this.moveDown.Enabled = false;
				this.moveUp.Enabled = false;
				runSimulation.Enabled = false;
			}


			if (runningGrid.Count == 0)
			{
				removeSim.Enabled = false;
				simInfo.Enabled = false;
				logButton.Enabled = false;
			}

			this.ResumeLayout(true);

			availableGrid.Size = new System.Drawing.Size(10,10);
			pendingGrid.Size = new System.Drawing.Size(10,10);
			runningGrid.Size = new System.Drawing.Size(10,10);

			availableGrid.Refresh();
			pendingGrid.Refresh();
			runningGrid.Refresh(); 
			
			//System.Drawing.Size curSize = this.Size;
			// this.Size = new System.Drawing.Size(curSize.Width - 100, curSize.Height - 100);

			
			// sim control configure
			// MarketSim process
			simProcess = new Process();

			simProcess.StartInfo.FileName = Settings.SimEngine;

			if (Settings.DefaultServerDirectory == null)
			{
				simProcess.StartInfo.WorkingDirectory = Application.StartupPath;
			}
			else
			{
				simProcess.StartInfo.WorkingDirectory = Settings.DefaultServerDirectory;
			}

			simProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

			simControl = new SimController(projectID, Application.StartupPath, simProcess);

			simControl.SimUpdated +=new SimControlMethods.SimController.Refresh(simControl_SimUpdated);
			simControl.ReportFinished +=new SimControlMethods.SimController.SimDone(simControl_ReportFinished);

			simState = SimController.SimState.NotRunning;

			FileInfo fi = new FileInfo(simProcess.StartInfo.WorkingDirectory + @"\" + simProcess.StartInfo.FileName);
			if (!fi.Exists || simProcess.StartInfo.FileName == null)
			{
				// set state to No Sim Engine connected
				simState =  SimController.SimState.NoSimEngine;
				runSimulation.Text = "Configure Simulation Engine";
				runSimulation.Enabled = true;
			}
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BatchRunForm));
			this.runningSims = new System.Windows.Forms.GroupBox();
			this.runningGrid = new MrktSimGrid();
			this.runningButtons = new System.Windows.Forms.GroupBox();
			this.removeSim = new System.Windows.Forms.Button();
			this.simInfo = new System.Windows.Forms.Button();
			this.logButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.simtextBox = new System.Windows.Forms.TextBox();
			this.batchedModelsBox = new System.Windows.Forms.GroupBox();
			this.pendingGrid = new MrktSimGrid();
			this.pendingButtons = new System.Windows.Forms.GroupBox();
			this.RemoveButton = new System.Windows.Forms.Button();
			this.moveUp = new System.Windows.Forms.Button();
			this.moveDown = new System.Windows.Forms.Button();
			this.runSimulation = new System.Windows.Forms.Button();
			this.AvailableModels = new System.Windows.Forms.GroupBox();
			this.availableGrid = new MrktSimGrid();
			this.availableButtons = new System.Windows.Forms.GroupBox();
			this.AddButton = new System.Windows.Forms.Button();
			this.refreshButton = new System.Windows.Forms.Button();
			this.leftSplitter = new System.Windows.Forms.Splitter();
			this.rightSplitter = new System.Windows.Forms.Splitter();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.optionMenu = new System.Windows.Forms.MenuItem();
			this.setSimEngine = new System.Windows.Forms.MenuItem();
			this.resetSimConnection = new System.Windows.Forms.MenuItem();
			this.runningSims.SuspendLayout();
			this.runningButtons.SuspendLayout();
			this.batchedModelsBox.SuspendLayout();
			this.pendingButtons.SuspendLayout();
			this.AvailableModels.SuspendLayout();
			this.availableButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// runningSims
			// 
			this.runningSims.Controls.Add(this.runningGrid);
			this.runningSims.Controls.Add(this.runningButtons);
			this.runningSims.Dock = System.Windows.Forms.DockStyle.Fill;
			this.runningSims.Location = new System.Drawing.Point(506, 0);
			this.runningSims.Name = "runningSims";
			this.runningSims.Size = new System.Drawing.Size(278, 350);
			this.runningSims.TabIndex = 13;
			this.runningSims.TabStop = false;
			this.runningSims.Text = "Completed or Running  Simulations";
			// 
			// runningGrid
			// 
			this.runningGrid.DescribeRow = null;
			this.runningGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.runningGrid.Location = new System.Drawing.Point(3, 16);
			this.runningGrid.Name = "runningGrid";
			this.runningGrid.RowFilter = null;
			this.runningGrid.RowID = null;
			this.runningGrid.RowName = null;
			this.runningGrid.Size = new System.Drawing.Size(272, 231);
			this.runningGrid.Sort = "";
			this.runningGrid.TabIndex = 0;
			this.runningGrid.Table = null;
			// 
			// runningButtons
			// 
			this.runningButtons.Controls.Add(this.removeSim);
			this.runningButtons.Controls.Add(this.simInfo);
			this.runningButtons.Controls.Add(this.logButton);
			this.runningButtons.Controls.Add(this.label1);
			this.runningButtons.Controls.Add(this.simtextBox);
			this.runningButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.runningButtons.Location = new System.Drawing.Point(3, 247);
			this.runningButtons.Name = "runningButtons";
			this.runningButtons.Size = new System.Drawing.Size(272, 100);
			this.runningButtons.TabIndex = 1;
			this.runningButtons.TabStop = false;
			// 
			// removeSim
			// 
			this.removeSim.Location = new System.Drawing.Point(24, 16);
			this.removeSim.Name = "removeSim";
			this.removeSim.Size = new System.Drawing.Size(88, 23);
			this.removeSim.TabIndex = 0;
			this.removeSim.Text = "Delete results";
			this.removeSim.Click += new System.EventHandler(this.removeSim_Click);
			// 
			// simInfo
			// 
			this.simInfo.Location = new System.Drawing.Point(136, 16);
			this.simInfo.Name = "simInfo";
			this.simInfo.Size = new System.Drawing.Size(40, 24);
			this.simInfo.TabIndex = 13;
			this.simInfo.Text = "info...";
			this.simInfo.Click += new System.EventHandler(this.simInfo_Click);
			// 
			// logButton
			// 
			this.logButton.Location = new System.Drawing.Point(192, 16);
			this.logButton.Name = "logButton";
			this.logButton.TabIndex = 16;
			this.logButton.Text = "View Log";
			this.logButton.Click += new System.EventHandler(this.logButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 11;
			this.label1.Text = "Currently Running";
			// 
			// simtextBox
			// 
			this.simtextBox.Location = new System.Drawing.Point(24, 64);
			this.simtextBox.Name = "simtextBox";
			this.simtextBox.ReadOnly = true;
			this.simtextBox.Size = new System.Drawing.Size(200, 20);
			this.simtextBox.TabIndex = 14;
			this.simtextBox.Text = "";
			this.simtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// batchedModelsBox
			// 
			this.batchedModelsBox.Controls.Add(this.pendingGrid);
			this.batchedModelsBox.Controls.Add(this.pendingButtons);
			this.batchedModelsBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.batchedModelsBox.Location = new System.Drawing.Point(253, 0);
			this.batchedModelsBox.Name = "batchedModelsBox";
			this.batchedModelsBox.Size = new System.Drawing.Size(250, 350);
			this.batchedModelsBox.TabIndex = 9;
			this.batchedModelsBox.TabStop = false;
			this.batchedModelsBox.Text = "Simulation Queue";
			// 
			// pendingGrid
			// 
			this.pendingGrid.DescribeRow = null;
			this.pendingGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pendingGrid.Location = new System.Drawing.Point(3, 16);
			this.pendingGrid.Name = "pendingGrid";
			this.pendingGrid.RowFilter = null;
			this.pendingGrid.RowID = null;
			this.pendingGrid.RowName = null;
			this.pendingGrid.Size = new System.Drawing.Size(244, 231);
			this.pendingGrid.Sort = "";
			this.pendingGrid.TabIndex = 0;
			this.pendingGrid.Table = null;
			// 
			// pendingButtons
			// 
			this.pendingButtons.Controls.Add(this.RemoveButton);
			this.pendingButtons.Controls.Add(this.moveUp);
			this.pendingButtons.Controls.Add(this.moveDown);
			this.pendingButtons.Controls.Add(this.runSimulation);
			this.pendingButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pendingButtons.Location = new System.Drawing.Point(3, 247);
			this.pendingButtons.Name = "pendingButtons";
			this.pendingButtons.Size = new System.Drawing.Size(244, 100);
			this.pendingButtons.TabIndex = 1;
			this.pendingButtons.TabStop = false;
			// 
			// RemoveButton
			// 
			this.RemoveButton.Location = new System.Drawing.Point(48, 16);
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(72, 24);
			this.RemoveButton.TabIndex = 5;
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
			// 
			// moveUp
			// 
			this.moveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.moveUp.Location = new System.Drawing.Point(136, 16);
			this.moveUp.Name = "moveUp";
			this.moveUp.Size = new System.Drawing.Size(23, 24);
			this.moveUp.TabIndex = 8;
			this.moveUp.Text = "/\\";
			this.moveUp.Click += new System.EventHandler(this.moveUp_Click);
			// 
			// moveDown
			// 
			this.moveDown.Location = new System.Drawing.Point(160, 16);
			this.moveDown.Name = "moveDown";
			this.moveDown.Size = new System.Drawing.Size(23, 24);
			this.moveDown.TabIndex = 9;
			this.moveDown.Text = "V";
			this.moveDown.Click += new System.EventHandler(this.moveDown_Click);
			// 
			// runSimulation
			// 
			this.runSimulation.Location = new System.Drawing.Point(24, 56);
			this.runSimulation.Name = "runSimulation";
			this.runSimulation.Size = new System.Drawing.Size(192, 23);
			this.runSimulation.TabIndex = 10;
			this.runSimulation.Text = "Run Market Simulation";
			this.runSimulation.Click += new System.EventHandler(this.runSimulation_Click);
			// 
			// AvailableModels
			// 
			this.AvailableModels.Controls.Add(this.availableGrid);
			this.AvailableModels.Controls.Add(this.availableButtons);
			this.AvailableModels.Dock = System.Windows.Forms.DockStyle.Left;
			this.AvailableModels.Location = new System.Drawing.Point(0, 0);
			this.AvailableModels.Name = "AvailableModels";
			this.AvailableModels.Size = new System.Drawing.Size(250, 350);
			this.AvailableModels.TabIndex = 10;
			this.AvailableModels.TabStop = false;
			this.AvailableModels.Text = "Available Scenarios";
			// 
			// availableGrid
			// 
			this.availableGrid.DescribeRow = null;
			this.availableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableGrid.Location = new System.Drawing.Point(3, 16);
			this.availableGrid.Name = "availableGrid";
			this.availableGrid.RowFilter = null;
			this.availableGrid.RowID = null;
			this.availableGrid.RowName = null;
			this.availableGrid.Size = new System.Drawing.Size(244, 231);
			this.availableGrid.Sort = "";
			this.availableGrid.TabIndex = 0;
			this.availableGrid.Table = null;
			// 
			// availableButtons
			// 
			this.availableButtons.Controls.Add(this.AddButton);
			this.availableButtons.Controls.Add(this.refreshButton);
			this.availableButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.availableButtons.Location = new System.Drawing.Point(3, 247);
			this.availableButtons.Name = "availableButtons";
			this.availableButtons.Size = new System.Drawing.Size(244, 100);
			this.availableButtons.TabIndex = 1;
			this.availableButtons.TabStop = false;
			// 
			// AddButton
			// 
			this.AddButton.Location = new System.Drawing.Point(40, 16);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(144, 24);
			this.AddButton.TabIndex = 4;
			this.AddButton.Text = "Add to Simulation queue";
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// refreshButton
			// 
			this.refreshButton.Location = new System.Drawing.Point(72, 56);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.TabIndex = 15;
			this.refreshButton.Text = "Refresh";
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// leftSplitter
			// 
			this.leftSplitter.Location = new System.Drawing.Point(250, 0);
			this.leftSplitter.MinSize = 220;
			this.leftSplitter.Name = "leftSplitter";
			this.leftSplitter.Size = new System.Drawing.Size(3, 350);
			this.leftSplitter.TabIndex = 14;
			this.leftSplitter.TabStop = false;
			// 
			// rightSplitter
			// 
			this.rightSplitter.Location = new System.Drawing.Point(503, 0);
			this.rightSplitter.MinSize = 220;
			this.rightSplitter.Name = "rightSplitter";
			this.rightSplitter.Size = new System.Drawing.Size(3, 350);
			this.rightSplitter.TabIndex = 15;
			this.rightSplitter.TabStop = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.optionMenu});
			// 
			// optionMenu
			// 
			this.optionMenu.Index = 0;
			this.optionMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.setSimEngine,
																					   this.resetSimConnection});
			this.optionMenu.Text = "Options";
			this.optionMenu.Select += new System.EventHandler(this.optionMenu_Click);
			// 
			// setSimEngine
			// 
			this.setSimEngine.Index = 0;
			this.setSimEngine.Text = "Set Simulation Engine";
			this.setSimEngine.Click += new System.EventHandler(this.setSimEngine_Click);
			// 
			// resetSimConnection
			// 
			this.resetSimConnection.Index = 1;
			this.resetSimConnection.Text = "Reset Simulation Database";
			this.resetSimConnection.Click += new System.EventHandler(this.resetSimConnection_Click);
			// 
			// BatchRunForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(784, 350);
			this.Controls.Add(this.runningSims);
			this.Controls.Add(this.rightSplitter);
			this.Controls.Add(this.batchedModelsBox);
			this.Controls.Add(this.leftSplitter);
			this.Controls.Add(this.AvailableModels);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(792, 384);
			this.Name = "BatchRunForm";
			this.Text = "Batch Processing";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BatchRunForm_Closing);
			this.runningSims.ResumeLayout(false);
			this.runningButtons.ResumeLayout(false);
			this.batchedModelsBox.ResumeLayout(false);
			this.pendingButtons.ResumeLayout(false);
			this.AvailableModels.ResumeLayout(false);
			this.availableButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			int projectID = -1;

			if (args.Length > 0)
				projectID = Int32.Parse(args[0]);

			BatchRunForm form = new BatchRunForm(projectID);

			if (projectID != -2)
				Application.Run(form);
		}

		private SimController simControl = null;
		private ModelInfoDb modelDb;
		private MSConnect msConnect;
		private SimController.SimState simState;
		private Process simProcess;

		private int openProject()
		{
			modelDb.ReadProjects();

			OpenProject dlg = new OpenProject();

			dlg.Db = modelDb;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
				return dlg.ProjectID;

			return -1;
		}

		// batch up selected model
		private void AddButton_Click(object sender, System.EventArgs e)
		{
			ArrayList scenarios = availableGrid.GetSelected();

			if (scenarios.Count == 0)
				return;

			

			this.SuspendLayout();

			// make sure scenarios are up to date
			this.refreshButton_Click(sender, e);

			this.availableGrid.Suspend = true;
			this.pendingGrid.Suspend = true;
			this.runningGrid.Suspend = true;

			int queueNum = 1;
			foreach(ModelInfo.scenarioRow scenario in scenarios)
			{
				string query = "queued = 1 AND sim_num > 0";
				DataRow[] pending = modelDb.ModelData.scenario.Select(query, "", DataViewRowState.CurrentRows);

				foreach(ModelInfo.scenarioRow aRow in pending)
				{
					if( queueNum <= aRow.sim_num)
						queueNum = aRow.sim_num + 1;
				}

				scenario.queued = true;
				scenario.sim_num = queueNum;
				queueNum++;
			}

			// update database
			update();

			if (pendingGrid.Count > 0)
			{
				this.RemoveButton.Enabled = true;
				this.moveDown.Enabled = true;
				this.moveUp.Enabled = true;
				runSimulation.Enabled = true;
			}

			if (availableGrid.Count == 0)
				AddButton.Enabled = false;

			
			this.ResumeLayout(false);

			this.availableGrid.Suspend = false;
			this.pendingGrid.Suspend = false;
			this.runningGrid.Suspend = false;
		}

		// remove model from batch processing
		private void RemoveButton_Click(object sender, System.EventArgs e)
		{
			ArrayList scenarios = pendingGrid.GetSelected();

			if (scenarios.Count == 0)
				return;

			this.SuspendLayout();
			
			this.availableGrid.Suspend = true;
			this.pendingGrid.Suspend = true;
			this.runningGrid.Suspend = true;

			foreach(ModelInfo.scenarioRow scenario in scenarios)
			{
				scenario.queued = false;
				scenario.sim_num = -1;
			}

			// update database
			update();

			if (pendingGrid.Count == 0)
			{
				this.RemoveButton.Enabled = false;
				this.moveDown.Enabled = false;
				this.moveUp.Enabled = false;
				runSimulation.Enabled = false;
			}

			
			if (availableGrid.Count > 0)
				AddButton.Enabled = true;

			this.ResumeLayout(false);

		
		}

		private void moveUp_Click(object sender, System.EventArgs e)
		{
			DataRow row = pendingGrid.CurrentRow;
			if ( row == null)
				return;

			ModelInfo.scenarioRow scenario = (ModelInfo.scenarioRow) row;

			string query = "queued  = 1 AND sim_num > 0 AND sim_num < " + scenario.sim_num;

			DataRow[] rows = modelDb.ModelData.scenario.Select(query, "sim_num DESC", DataViewRowState.CurrentRows);

			if (rows.Length == 0)
				return;

			ModelInfo.scenarioRow aRow = (ModelInfo.scenarioRow) rows[0];

			// swap
			int prevNum = aRow.sim_num;
			aRow.sim_num = scenario.sim_num;
			scenario.sim_num = prevNum;

			// update database
			update();
		}

		private void moveDown_Click(object sender, System.EventArgs e)
		{
			DataRow row = pendingGrid.CurrentRow;

			if ( row == null)
				return;

			ModelInfo.scenarioRow scenario = (ModelInfo.scenarioRow) row;

			string query = "queued = 1 AND sim_num > " + scenario.sim_num;

			DataRow[] rows = modelDb.ModelData.scenario.Select(query, "sim_num", DataViewRowState.CurrentRows);

			if (rows.Length == 0)
				return;

			ModelInfo.scenarioRow aRow = (ModelInfo.scenarioRow) rows[0];

			// swap
			int nextNum = aRow.sim_num;
			aRow.sim_num = scenario.sim_num;
	
			scenario.sim_num = nextNum;

			// update database
			update();
		}

		private void removeSim_Click(object sender, System.EventArgs e)
		{
			ArrayList sims = runningGrid.GetSelected();

			if (sims.Count == 0)
				return;

			this.SuspendLayout();

			this.availableGrid.Suspend = true;
			this.pendingGrid.Suspend = true;
			this.runningGrid.Suspend = true;


			int[] scenarioArr = new int[sims.Count];

			int index = 0;

			foreach(ModelInfo.sim_queueRow sim in sims)
			{
				scenarioArr[index] = sim.scenario_id;
				index++;

				try
				{
					modelDb.DeleteRun(sim);
				}
				catch(Exception) {}
			}
			
			for (index = 0; index < scenarioArr.Length; ++index)
			{
				int scenarioID = scenarioArr[index];

				// check if sccenario has any sims
				string query = "scenario_id = " + scenarioID;

				DataRow[] rows = modelDb.ModelData.sim_queue.Select(query, "",DataViewRowState.CurrentRows);

				if (rows.Length > 0)
					continue;

				// has sims so update scenario
				ModelInfo.scenarioRow scenario = modelDb.ModelData.scenario.FindByscenario_id(scenarioID);

				if (scenario.queued == true)
					scenario.queued = false;

				if (scenario.sim_num >= 0)
					scenario.sim_num = -1;
			}
		
			update();

			if (runningGrid.Count == 0)
			{
				this.removeSim.Enabled = false;
				simInfo.Enabled = false;
				logButton.Enabled = false;
			}

			if (availableGrid.Count > 0)
				AddButton.Enabled = true;

			this.ResumeLayout(false);
		}

		public int NumToRun
		{
			get
			{
				// all the initial queue items
				string query = "queued = 1 AND sim_num > 0";
				DataRow[] rows = modelDb.ModelData.scenario.Select(query, "", DataViewRowState.CurrentRows);

				return rows.Length;
			}
		}

		private void run()
		{
			simControl.Run();
		}

		private void runSimulation_Click(object sender, System.EventArgs e)
		{
			refreshButton_Click(sender, e);

			switch (simState)
			{
				case SimController.SimState.NoSimEngine:

					setSimEngine_Click(sender, e);
					break;
				case SimController.SimState.NotRunning:

					// start up the simulation
					Thread simThread = new Thread(new ThreadStart(run));

					runSimulation.Text = "Stop Market Simulation";
					simState = SimController.SimState.Running;

					simThread.Start();

					// disable controls while running
					disableControls(true);

					break;

				case SimController.SimState.Running:
					// stop the simulation
					runSimulation.Text = "Stopping...";
					runSimulation.Enabled = false;
					simState = SimController.SimState.Stopping;

					if (!simProcess.HasExited)
						simProcess.Kill();
					break;
			}
		
			//			DataRow row = pendingGrid.CurrentRow;
			//			if ( row == null)
			//				return;
			//
			//			ModelInfo.sim_queueRow simRow = (ModelInfo.sim_queueRow) row;
			//
			//			SimStatus dlg = new SimStatus();
			//
			//			dlg.Db = this.modelDb;
			//			dlg.Run = simRow.run_id;
			//
			//			dlg.ShowDialog();

			//			refreshButton_Click(sender, e);
		}

		private void simInfo_Click(object sender, System.EventArgs e)
		{
			DataRow row = runningGrid.CurrentRow;
			if ( row == null)
				return;

			ModelInfo.sim_queueRow simRow = (ModelInfo.sim_queueRow) row;

			SimStatus dlg = new SimStatus();

			dlg.Db = this.modelDb;
			dlg.Run = simRow.run_id;

			dlg.Show();
		}

	
		private void createTableStyle()
		{
			availableGrid.Clear();
			availableGrid.AddComboBoxColumn("model_id", modelDb.ModelData.Model_info, "model_name", "model_id", true);
			availableGrid.AddTextColumn("name", true);

			availableGrid.Reset();
			
			pendingGrid.Clear();
			pendingGrid.AddComboBoxColumn("model_id", modelDb.ModelData.Model_info, "model_name", "model_id", true);
			pendingGrid.AddTextColumn("name", true);
			pendingGrid.Reset();

			runningGrid.Clear();
			runningGrid.AddComboBoxColumn("model_id", modelDb.ModelData.Model_info, "model_name", "model_id", true);
			runningGrid.AddComboBoxColumn("scenario_id", modelDb.ModelData.scenario, "name", "scenario_id", true);
			runningGrid.AddTextColumn("name", true);
			runningGrid.AddTextColumn("current_status", true);
			runningGrid.Reset();
		}

		private void refreshButton_Click(object sender, System.EventArgs e)
		{
			availableGrid.Suspend = true;
			pendingGrid.Suspend = true;
			runningGrid.Suspend = true;

			this.SuspendLayout();
			modelDb.RefreshSimQueue();
			
			// what sim is actually running now
			this.simInfo.Enabled = false;
			logButton.Enabled = false;
			this.simtextBox.Text = "";

			DataRow[] runningSims = modelDb.ModelData.sim_queue.Select("status = 1","",DataViewRowState.CurrentRows);

			foreach(DataRow sim in runningSims)
			{
				// really at most one here
				this.simInfo.Enabled = true;	
				logButton.Enabled = true;
				this.simtextBox.Text += sim["name"] + " ";
			}

			//			if (pendingGrid.Count == 0)
			//			{
			//				this.RemoveButton.Enabled = false;
			//				this.moveDown.Enabled = false;
			//				this.moveUp.Enabled = false;
			//				runSimulation.Enabled = false;
			//			}

			if (runningGrid.Count > 0) 
			{
				simInfo.Enabled = true;
				logButton.Enabled = true;

				if (simState == SimController.SimState.NotRunning)
				{
					this.removeSim.Enabled = true;
				
				}
			}
		
			this.ResumeLayout(false);

			availableGrid.Suspend = false;
			pendingGrid.Suspend = false;
			runningGrid.Suspend = false;

			
			// query database for info concerning running sim
			//			int currentSimID = modelDb.RunningSimulation();
			//
			//			if (currentSimID == Database.AllID)
			//				this.simtextBox.Text = "";
			//			else
			//			{
			//				// get name of sim
			//				ModelInfo.sim_queueRow sim = modelDb.ModelData.sim_queue.FindByrun_id(currentSimID);
			//
			//				if (sim != null)
			//					this.simtextBox.Text = sim.name;
			//			}
			//				
			//			if (simListBox.SelectedItem == null)
			//				this.simInfo.Enabled = false;
			//			else
			//				this.simInfo.Enabled = true;	
		}

		private void logButton_Click(object sender, System.EventArgs e)
		{
			DataRow row = runningGrid.CurrentRow;
			if ( row == null)
				return;

			ModelInfo.sim_queueRow simRow = (ModelInfo.sim_queueRow) row;

			SimLog dlg = new SimLog();

			dlg.Db = this.modelDb;
			dlg.Run = simRow.run_id;

			dlg.ShowDialog();
		}

		private void update()
		{
			// suspend grids
			this.availableGrid.Suspend = true;
			this.pendingGrid.Suspend = true;
			this.runningGrid.Suspend = true;

			// update database
			try
			{
				modelDb.UpdateModelData();
			}
			catch(Exception err)
			{
				MessageBox.Show("There was an error with the server, please close the application: " + err.Message);
				this.DialogResult = DialogResult.OK;
			}

			this.availableGrid.Suspend = false;
			this.pendingGrid.Suspend = false;
			this.runningGrid.Suspend = false;

			this.pendingGrid.Sort = "sim_num";
			this.availableGrid.Reset();
			this.pendingGrid.Reset();
			this.runningGrid.Reset();
		}

		private void simControl_SimUpdated(Object sender, SimController.CancelSim cancelSim)
		{
			if (this.InvokeRequired)
			{
				SimControlMethods.SimController.Refresh refresh = new SimControlMethods.SimController.Refresh(simControl_SimUpdated);
				this.Invoke(refresh, new object[] { sender, cancelSim });
			}
			else
			{
				refreshButton_Click(this, cancelSim);

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
				simState =  SimController.SimState.NotRunning;

				runSimulation.Enabled = true;
				simState = SimController.SimState.NotRunning;
				runSimulation.Text = "Run Market Simulation";
				resetSimConnection.Enabled = true;
				setSimEngine.Enabled = true;

				disableControls(false);

				EventArgs e = new EventArgs();
				refreshButton_Click(this, e);
			}
		}

		private void disableControls(bool simRunning)
		{
			RemoveButton.Enabled = !simRunning;
			AddButton.Enabled = !simRunning;
			moveUp.Enabled = !simRunning;
			moveDown.Enabled = !simRunning;
			removeSim.Enabled = !simRunning;
		}

		private void BatchRunForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
			if (this.simState == SimController.SimState.Running)
			{
				// check with user if we want to quit
				MessageBox.Show("Simulations are running, Please stop the simulation engine before quitting");

				e.Cancel = true;

				return;
			}

			Settings.DefaultServerDirectory = simProcess.StartInfo.WorkingDirectory;
			Settings.SimEngine = simProcess.StartInfo.FileName;

			Settings.Save();
		}

		private void setSimEngine_Click(object sender, System.EventArgs e)
		{
			// try to find a simulation engine
			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.CheckFileExists = true;
			openFileDlg.CheckPathExists = true;

			openFileDlg.FileName = simProcess.StartInfo.FileName;
			openFileDlg.InitialDirectory = simProcess.StartInfo.WorkingDirectory;

			openFileDlg.DefaultExt = ".exe";
			openFileDlg.Filter = "MarketSim Simulation File (*.exe)|*.exe";

			DialogResult rslt = openFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;

				int trimDex = fileName.LastIndexOf(@"\");

				simProcess.StartInfo.WorkingDirectory = fileName.Substring(0, trimDex);
				
				if (trimDex < fileName.Length)
				{
					simProcess.StartInfo.FileName = fileName.Substring(trimDex + 1);
				}

				simState = SimController.SimState.NotRunning;
				runSimulation.Text = "Run Market Simulation";

			}
		}

		private void resetSimConnection_Click(object sender, System.EventArgs e)
		{
			// check for engine connect file
			string msEngineConnectFile = simProcess.StartInfo.WorkingDirectory + @"\dbconnect";
			FileInfo fi = new FileInfo(msEngineConnectFile);
			if (fi.Exists)
			{
				try
				{
					fi.Delete();
				}
				catch(Exception)
				{}
			}
		}

		private void optionMenu_Click(object sender, System.EventArgs e)
		{
			string msEngineConnectFile = simProcess.StartInfo.WorkingDirectory + @"\dbconnect";
			FileInfo fi = new FileInfo(msEngineConnectFile);
			if (fi.Exists)
				resetSimConnection.Enabled = true;
			else
				resetSimConnection.Enabled = false;
		}

		private void checkLicense()
		{
			// check date against settings

			LicComputer lic = new LicComputer();

			lic.UserName = Settings.UserCode;
			lic.LicenseKey = Settings.LicenseKey;

			DateTime expirationDate = lic.ExpirationDate;
			DateTime today = DateTime.Today;

			while( expirationDate < today)
			{
				License dlg = new License();
				dlg.UserCode = lic.UserName;

				DialogResult rslt = dlg.ShowDialog();

				if (rslt == DialogResult.OK)
				{
					lic.LicenseKey = dlg.Key;
					lic.UserName = dlg.UserCode;
					expirationDate = lic.ExpirationDate;
				}
				else
					break;
			}

			if (expirationDate < today)
			{
				this.availableButtons.Enabled = false;
				this.pendingButtons.Enabled = false;
			}
			else
			{
				Settings.LicenseKey = lic.LicenseKey;
				Settings.UserCode = lic.UserName;

				this.availableButtons.Enabled = true;
				this.pendingButtons.Enabled = true;
			}
		}
	}
}
