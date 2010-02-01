using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;

using Results;

using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
	public class Results : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button resultsFormBUtton;
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Button myResultsButton;
		private System.Windows.Forms.Button deleteButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Results()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktSimGrid.DescribeRow = "descr";
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.myResultsButton = new System.Windows.Forms.Button();
			this.resultsFormBUtton = new System.Windows.Forms.Button();
			this.mrktSimGrid = new MrktSimGrid();
			this.deleteButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.deleteButton);
			this.panel1.Controls.Add(this.myResultsButton);
			this.panel1.Controls.Add(this.resultsFormBUtton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 272);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 56);
			this.panel1.TabIndex = 1;
			// 
			// myResultsButton
			// 
			this.myResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.myResultsButton.Location = new System.Drawing.Point(32, 16);
			this.myResultsButton.Name = "myResultsButton";
			this.myResultsButton.Size = new System.Drawing.Size(128, 23);
			this.myResultsButton.TabIndex = 1;
			this.myResultsButton.Text = "Selected  Results...";
			this.myResultsButton.Click += new System.EventHandler(this.myResultsButton_Click);
			// 
			// resultsFormBUtton
			// 
			this.resultsFormBUtton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.resultsFormBUtton.Location = new System.Drawing.Point(176, 16);
			this.resultsFormBUtton.Name = "resultsFormBUtton";
			this.resultsFormBUtton.Size = new System.Drawing.Size(136, 23);
			this.resultsFormBUtton.TabIndex = 0;
			this.resultsFormBUtton.Text = "All MarketSim Results...";
			this.resultsFormBUtton.Click += new System.EventHandler(this.resultsFormBUtton_Click);
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.DescribeRow = null;
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.Location = new System.Drawing.Point(0, 0);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(496, 272);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 2;
			this.mrktSimGrid.Table = null;
			// 
			// deleteButton
			// 
			this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.deleteButton.Location = new System.Drawing.Point(344, 16);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(104, 23);
			this.deleteButton.TabIndex = 3;
			this.deleteButton.Text = "Delete Scenario";
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// Results
			// 
			this.Controls.Add(this.mrktSimGrid);
			this.Controls.Add(this.panel1);
			this.Name = "Results";
			this.Size = new System.Drawing.Size(496, 328);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods
		public Database Db
		{
			set
			{
				db = value;

				this.mrktSimGrid.Table = value.Data.scenario;

				this.mrktSimGrid.RowFilter = "sim_num >= 0";

				// make the table
				createTableStyle();
			}

			get
			{
				return db;
			}
		}
		#endregion

		#region private data and methods
		Database db = null;

		private void createTableStyle()
		{
			this.mrktSimGrid.Clear();

			this.mrktSimGrid.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid.AddTextColumn("user_name", "Owner", true);

			this.mrktSimGrid.Reset();
		}
		

		#endregion

		#region Wizard Members

		public event BrandManager.Forms.StateChange StateChanged;

		public event BrandManager.Forms.TaskDone Done;

		public bool GotoNext()
		{
			// TODO:  Add Results.GotoNext implementation
			return false;
		}

		#endregion

		private void resultsFormBUtton_Click(object sender, System.EventArgs e)
		{
			Database aDb = new Database();
			aDb.Connection = Db.Connection;
			aDb.CurrentModel = Db.CurrentModel;
			aDb.ReadModelForResults();

			ResultsForm dlg = new ResultsForm();

			dlg.Db = aDb;

			dlg.ShowDialog();
		
		}

		private void myResultsButton_Click(object sender, System.EventArgs e)
		{
			ArrayList scenarioList = mrktSimGrid.GetSelected();

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			Database aDb = new Database();
			aDb.Connection = Db.Connection;
			aDb.CurrentModel = Db.CurrentModel;
			aDb.ReadModelForResults();

			ResultsForm dlg = new ResultsForm();

			dlg.Db = aDb;


			// for each scenario selected -- make sure only my products are selected
			dlg.ClearSelectedProducts();

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
				{
					// only add products for my plans

					MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;

					if (plan.user_name == Setup.Settings.User)
					{
						dlg.SelectProduct(plan.product_id);
					}
				}
			}

			// fiure out the time range

			DateTime start = new DateTime(9999,1,1);
			DateTime end = new DateTime(1, 1, 1);

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				if (scenario.user_name == Setup.Settings.User)
				{
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			if (start > end)
			{
				// must be looking at strategic scenarios
				foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
				{
					
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			dlg.ShowGraph(scenarioList, start, end);
		}

		private void deleteButton_Click(object sender, System.EventArgs e)
		{
			ArrayList scenarioList = mrktSimGrid.GetSelected();

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				deleteScenario(scenario);
			}
		}

		// truly belongs in MrktSimDb
		private void deleteScenario(MrktSimDb.MrktSimDBSchema.scenarioRow scenario)
		{
			// this not only deletes the scenario but deletes the market plan and
			// also deletes market plans components owned by this user

			// start at the components
			foreach(MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
			{
				MrktSimDBSchema.market_planRow topPlan = planRef.market_planRow;

				// if user owns plan then delete it

				if (topPlan.user_name == Setup.Settings.User)
				{
					topPlan.Delete();
				}
			}

			scenario.Delete();

			// cleanup
			// delete all market plan components that are no longer referenced
			string query = "user_name = '" + Setup.Settings.User + "'";
			DataRow[] plans = Db.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

			foreach(MrktSimDBSchema.market_planRow plan in plans)
			{
				// if this is not in a top level plan it needs to be deleted

				if (plan.type != 0)
				{
					// not a top level plan then if it does not have a reference in the market plan tree
					// delete it

					if (plan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_child().Length == 0)
					{
						plan.Delete();
					}
				}
			}
		}

	}
}
