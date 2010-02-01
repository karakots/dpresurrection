using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics; // process

using System.Data.OleDb;

using MrktSimDb;

using Common.Utilities; // SimStatusControl
using Common.Dialogs;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for RunSims.
	/// </summary>
	public class RunSims : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label mrtkSimGirdTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button queueButton;
		private System.Windows.Forms.Button statusButton;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.CheckedListBox scenarioCheckList;
		private MarketSimUtilities.MrktSimGrid simMrktGrid;
		private System.Windows.Forms.Timer refreshTimer;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		public RunSims()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			modeldb = new ModelInfoDb();

			runOrNot = new DataTable("RunOrNot");

			DataColumn numCol = runOrNot.Columns.Add("sim_num", typeof(int));
			runOrNot.Columns.Add("status", typeof(string));
			runOrNot.PrimaryKey = new DataColumn[] {numCol};

			DataRow queued = runOrNot.NewRow();
			queued["sim_num"] = 1;
			queued["status"] = "Queued";
			runOrNot.Rows.Add(queued);


			DataRow running = runOrNot.NewRow();
			running["sim_num"] = 0;
			running["status"] = "Running";
			runOrNot.Rows.Add(running);

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.refreshButton = new System.Windows.Forms.Button();
			this.statusButton = new System.Windows.Forms.Button();
			this.queueButton = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.scenarioCheckList = new System.Windows.Forms.CheckedListBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.simMrktGrid = new MarketSimUtilities.MrktSimGrid();
			this.panel5 = new System.Windows.Forms.Panel();
			this.mrtkSimGirdTitle = new System.Windows.Forms.Label();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.refreshButton);
			this.panel1.Controls.Add(this.statusButton);
			this.panel1.Controls.Add(this.queueButton);
			this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 352);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 72);
			this.panel1.TabIndex = 1;
			// 
			// refreshButton
			// 
			this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.refreshButton.Location = new System.Drawing.Point(352, 24);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(136, 23);
			this.refreshButton.TabIndex = 8;
			this.refreshButton.Text = "Refresh";
			this.toolTip1.SetToolTip(this.refreshButton, "Refresh the queue list");
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// statusButton
			// 
			this.statusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.statusButton.Location = new System.Drawing.Point(184, 24);
			this.statusButton.Name = "statusButton";
			this.statusButton.Size = new System.Drawing.Size(136, 23);
			this.statusButton.TabIndex = 7;
			this.statusButton.Text = "View Sim Status";
			this.toolTip1.SetToolTip(this.statusButton, "View the status of a running scenario");
			this.statusButton.Click += new System.EventHandler(this.statusButton_Click);
			// 
			// queueButton
			// 
			this.queueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.queueButton.Location = new System.Drawing.Point(16, 24);
			this.queueButton.Name = "queueButton";
			this.queueButton.Size = new System.Drawing.Size(136, 23);
			this.queueButton.TabIndex = 6;
			this.queueButton.Text = "Run Scenarios";
			this.toolTip1.SetToolTip(this.queueButton, "Put the selected scenarios in the queue");
			this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.scenarioCheckList);
			this.panel2.Controls.Add(this.panel4);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(504, 128);
			this.panel2.TabIndex = 2;
			// 
			// scenarioCheckList
			// 
			this.scenarioCheckList.CheckOnClick = true;
			this.scenarioCheckList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scenarioCheckList.Location = new System.Drawing.Point(0, 24);
			this.scenarioCheckList.Name = "scenarioCheckList";
			this.scenarioCheckList.Size = new System.Drawing.Size(504, 94);
			this.scenarioCheckList.TabIndex = 1;
			this.scenarioCheckList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.scenarioCheckList_ItemCheck);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.label2);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(504, 24);
			this.panel4.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(504, 24);
			this.label2.TabIndex = 0;
			this.label2.Text = "Scenarios Available to Run";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.simMrktGrid);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 128);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(504, 224);
			this.panel3.TabIndex = 3;
			// 
			// simMrktGrid
			// 
			this.simMrktGrid.DescribeRow = null;
			this.simMrktGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simMrktGrid.EnabledGrid = true;
			this.simMrktGrid.Location = new System.Drawing.Point(0, 24);
			this.simMrktGrid.Name = "simMrktGrid";
			this.simMrktGrid.RowFilter = null;
			this.simMrktGrid.RowID = null;
			this.simMrktGrid.RowName = null;
			this.simMrktGrid.Size = new System.Drawing.Size(504, 200);
			this.simMrktGrid.Sort = "";
			this.simMrktGrid.TabIndex = 1;
			this.simMrktGrid.Table = null;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.mrtkSimGirdTitle);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(504, 24);
			this.panel5.TabIndex = 0;
			// 
			// mrtkSimGirdTitle
			// 
			this.mrtkSimGirdTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrtkSimGirdTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mrtkSimGirdTitle.Location = new System.Drawing.Point(0, 0);
			this.mrtkSimGirdTitle.Name = "mrtkSimGirdTitle";
			this.mrtkSimGirdTitle.Size = new System.Drawing.Size(504, 24);
			this.mrtkSimGirdTitle.TabIndex = 0;
			this.mrtkSimGirdTitle.Text = "Queued or Running Scenarios";
			this.mrtkSimGirdTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// refreshTimer
			// 
			this.refreshTimer.Interval = 30000;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// RunSims
			// 
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "RunSims";
			this.Size = new System.Drawing.Size(504, 424);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			this.UpdateScenarioList();
			/*if(this.scenarioCheckList.Items.Count != 0)
			{
				DialogResult rslt = MessageBox.Show("Not all of your sims have been run are you sure you want to view results?","Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if(rslt == DialogResult.Yes)
				{
					return true;
				}
				return false;
			}*/
			return true;
		}

		public bool Back()
		{
			return true;
		}

		public void Start()
		{
			if (brandManagerDb != null)
			{
				brandManagerDb.Update();
			}

			this.UpdateScenarioList();
			if(this.scenarioCheckList.Items.Count == 0)
			{
				MessageBox.Show("All your sims have results or are running","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			this.refreshTimer.Start();
		}

		public void End()
		{
			this.refreshTimer.Stop();

			if (brandManagerDb != null)
			{
				brandManagerDb.ReadModelForBrandManager();
			}
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region UI

		#endregion
		#region Public Properties and Methods

		Database brandManagerDb = null;
		public Database Db
		{
			set
			{
				// we remember this so we can update it when we are finished
				brandManagerDb = value;

				modeldb.Connection = value.Connection;
				modeldb.ProjectID = Setup.Settings.Project;

				this.simMrktGrid.Table = modeldb.ModelData.scenario;

				this.simMrktGrid.RowFilter = "model_id = " + Setup.Settings.Model + " AND user_name = '" + Setup.Settings.User + "' AND queued = 1";

				createTableStyle();
			}
		}

		public void UpdateScenarioList()
		{
			this.Cursor = Cursors.WaitCursor;
			this.SuspendLayout();
			this.simMrktGrid.Suspend = true;
			modeldb.RefreshSimQueue();
		
			this.scenarioCheckList.Items.Clear();

			string query = "model_id = " + Setup.Settings.Model + " AND type = 0 AND user_name = '" + Setup.Settings.User + "' AND sim_num < 0";

			DataRow[] rows = modeldb.ModelData.scenario.Select(query,"",DataViewRowState.CurrentRows);

			foreach(ModelInfo.scenarioRow scenario in rows)
			{
				this.scenarioCheckList.Items.Add(new Scenario(scenario));		
			}
			this.simMrktGrid.Suspend = false;
			this.ResumeLayout();
			this.Cursor = Cursors.Arrow;
		}

		#endregion


		#region Private Data and Methods
		
		private ModelInfoDb modeldb;

		private System.Data.DataTable runOrNot;

		private void createTableStyle()
		{
			this.simMrktGrid.Clear();

			this.simMrktGrid.AddTextColumn("name","Scenario",true);
			this.simMrktGrid.AddComboBoxColumn("sim_num",runOrNot, "status", "sim_num", true);

			//this.simMrktGrid.AddCheckBoxColumn("sim_num", "Running", "0", "1", true);
			//this.simMrktGrid.AddTextColumn("sim_num", true);

			this.simMrktGrid.Reset();
		}

		#endregion

		private void queueButton_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			this.SuspendLayout();
			this.simMrktGrid.Suspend = true;
			if(this.scenarioCheckList.CheckedItems.Count == 0)
			{
				MessageBox.Show("Please select a scenario to run","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			foreach(Scenario wrapper in scenarioCheckList.CheckedItems)
			{
				wrapper.myScenario.queued = true;
				wrapper.myScenario.sim_num = 1;
			}

			modeldb.UpdateModelData();

			this.UpdateScenarioList();

			this.simMrktGrid.Suspend = false;
			this.ResumeLayout();
			this.Cursor = Cursors.Arrow;
		}

		private void statusButton_Click(object sender, System.EventArgs e)
		{
			ModelInfo.scenarioRow scenario = (ModelInfo.scenarioRow) this.simMrktGrid.CurrentRow;
			if ( scenario == null)
			{
				MessageBox.Show("Please select a scenario first","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}

			ModelInfo.sim_queueRow[] simRows = scenario.Getsim_queueRows();

			foreach(ModelInfo.sim_queueRow sim in simRows)
			{
				if (sim.status != 2)
				{
					SimStatus dlg = new SimStatus();

					dlg.Db = modeldb;
					dlg.Run = sim.run_id;

					dlg.Show();
				}
			}

			if ( simRows.Length == 0)
			{
				MessageBox.Show("This scenario is waiting to be run","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}
		}

		private void refreshButton_Click(object sender, System.EventArgs e)
		{
			UpdateScenarioList();
		}

		private void scenarioCheckList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if (e.CurrentValue == CheckState.Unchecked || this.scenarioCheckList.CheckedItems.Count > 1)
			{
				queueButton.Enabled = true;
			}
			else if (e.CurrentValue == CheckState.Checked && this.scenarioCheckList.CheckedItems.Count == 1)
			{
				queueButton.Enabled = false;
			}
		}

		private void refreshTimer_Tick(object sender, System.EventArgs e)
		{
			modeldb.RefreshSimQueue();
		}

		
	}

	public class Scenario
	{
		public override string ToString()
		{
			return myScenario.name;
		}

		public ModelInfo.scenarioRow myScenario;

		public Scenario(ModelInfo.scenarioRow scenario)
		{
			myScenario = scenario;
		}
	}
}
