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
	public class SaveScenario : System.Windows.Forms.UserControl, Wizard
	{
		private MrktSimGrid mrktSimGrid;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SaveScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktSimGrid.DescribeRow = "descr";

			this.saveScenarioList = new ArrayList();
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
			this.SuspendLayout();
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
			this.mrktSimGrid.Size = new System.Drawing.Size(496, 328);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 2;
			this.mrktSimGrid.Table = null;
			// 
			// SaveScenario
			// 
			this.Controls.Add(this.mrktSimGrid);
			this.Name = "SaveScenario";
			this.Size = new System.Drawing.Size(496, 328);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods
		public Database Db
		{
			set
			{
				db = value;

				if (db != null)
				{

					this.mrktSimGrid.Table = value.Data.scenario;

					this.mrktSimGrid.RowFilter = "user_name = '" + Setup.Settings.User  + "' AND queued <> 1";

					// make the table
					createTableStyle();
				}
			}

			get
			{
				return db;
			}
		}

		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add AddCompetitor.Next implementation
			return true;
		}

		public bool Back()
		{
			// TODO:  Add AddCompetitor.Back implementation
			return true;
		}

		public void Start()
		{
			db.ReadModelForBrandManager();
		}

		public void End()
		{
			mrktSimGrid.Flush();
			db.Update();
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region private data and methods
		Database db = null;
		ArrayList saveScenarioList = null;
	
		private void createTableStyle()
		{
			this.mrktSimGrid.Clear();

			this.mrktSimGrid.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid.AddTextColumn("user_name", "Owner", true);
			mrktSimGrid.AddCheckBoxColumn("saved","Save");

			this.mrktSimGrid.Reset();
		}

		#endregion


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
				scenario.saved = false;
			}
		}

		private void saveButton_Click(object sender, System.EventArgs e)
		{
			ArrayList scenarioList = mrktSimGrid.GetSelected();

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				scenario.saved = true;
			}
		}

	}
}
