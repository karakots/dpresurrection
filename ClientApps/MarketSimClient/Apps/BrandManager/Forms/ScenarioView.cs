using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using BrandManager.Dialogues;
using MrktSimDb;

using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for CreateScenarios.
	/// </summary>
	public class ScenarioView : System.Windows.Forms.UserControl, Wizard
	{
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label5;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ScenarioView()
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
			this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.DescribeRow = null;
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.EnabledGrid = true;
			this.mrktSimGrid.Location = new System.Drawing.Point(0, 56);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(512, 248);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 10;
			this.mrktSimGrid.Table = null;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label5);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(512, 56);
			this.panel1.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(312, 23);
			this.label5.TabIndex = 16;
			this.label5.Text = "Select Scenario to Review";
			// 
			// ScenarioView
			// 
			this.Controls.Add(this.mrktSimGrid);
			this.Controls.Add(this.panel1);
			this.Name = "ScenarioView";
			this.Size = new System.Drawing.Size(512, 304);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		#region Wizard Members

		public bool Next()
		{
			userScenario = SelectedScenario;

			if (userScenario == null)
			{
				MessageBox.Show("Please select a scenario to modify","Delete Results",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				
				return false;
			}

			/*DialogResult rslt = MessageBox.Show("This will delete any existing results for this scenario, are you sure you want to continue?","Delete Results",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
			if(rslt == DialogResult.OK)
			{
				return true;
			}
			return false;*/
			return true;
		}

		public bool Back()
		{
			// TODO:  Add AddCompetitor.Back implementation
			return true;
		}

		public void Start()
		{
			this.mrktSimGrid.Suspend = false;
			userScenario = null;
		}

		public void End()
		{
			//This will delete the results for the scenario
			// db.ReadModelForBrandManager();

			/*if (userScenario == null)
			{
				MessageBox.Show("WTF");
				return;
			}

			
			this.mrktSimGrid.Suspend = true;

			ModelInfoDb modelDb = new ModelInfoDb();
			modelDb.Connection = db.Connection;
			modelDb.ProjectID = Setup.Settings.Project;
			modelDb.RefreshSimQueue();
			

			string query = "scenario_id = " + userScenario.scenario_id;
			DataRow[] sims = modelDb.ModelData.sim_queue.Select(query,"",DataViewRowState.CurrentRows);
			foreach(ModelInfo.sim_queueRow sim in sims)
			{
				modelDb.DeleteRun(sim);
			}

			userScenario.queued = false;

			userScenario.sim_num = -1;

			db.Update();
			modelDb.UpdateModelData();*/
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region Public Methods

		public Database Db
		{
			set
			{
				db = value;
				this.mrktSimGrid.Table = value.Data.scenario;

				this.mrktSimGrid.RowFilter = "type = 0 AND queued = 0 AND sim_num <= 0 AND user_name = '" + Setup.Settings.User + "'";

				// make the table
				createTableStyle();
			}
		}


		public MrktSimDb.MrktSimDBSchema.scenarioRow SelectedScenario
		{
			get
			{
				if(userScenario != null)
				{
					return userScenario;
				}
				// get current scenario from grid
				DataRow row = this.mrktSimGrid.CurrentRow;

				if (row == null)
					return null;

				// construct name from doc name
				return (MrktSimDb.MrktSimDBSchema.scenarioRow) row;
			}
		}

		/// <summary>
		/// Flush data from grid
		/// </summary>
		public void Flush()
		{
			this.mrktSimGrid.Flush();
		}

		#endregion

		#region Private members and methods
		private Database db;
		MrktSimDb.MrktSimDBSchema.scenarioRow userScenario;
		private void createTableStyle()
		{
			mrktSimGrid.Clear();

			mrktSimGrid.AddTextColumn("name", "Scenario", true);
			//mrktSimGrid.AddCheckBoxColumn("saved","Saved");
		
			mrktSimGrid.Reset();
		}
		#endregion
	}
}
