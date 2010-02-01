using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for SelectScenario.
	/// </summary>
	public class SelectScenario : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.RadioButton NewScenario;
		private System.Windows.Forms.RadioButton ExistingScenario;
		private System.Windows.Forms.RadioButton RunSim;
		private System.Windows.Forms.RadioButton viewResults;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label createDescription;
		private System.Windows.Forms.Label reviewDescription;
		private System.Windows.Forms.Label runDescription;
		private System.Windows.Forms.Label viewDescription;
		private System.Windows.Forms.Label label5;
		private System.ComponentModel.IContainer components;

		public SelectScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			NewScenario.Checked = true;
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
		#region public_members_and_methods
		public Database Db
		{
			get
			{
				return db;
			}

			set
			{
				db = value;
			}
		}

		public bool existingScenario
		{
			get
			{
				return ExistingScenario.Checked;
			}
		}

		public bool skiptosim
		{
			get
			{
				return RunSim.Checked;
			}
		}

		public bool ViewResults
		{
			get
			{
				return viewResults.Checked;
			}
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.NewScenario = new System.Windows.Forms.RadioButton();
			this.ExistingScenario = new System.Windows.Forms.RadioButton();
			this.RunSim = new System.Windows.Forms.RadioButton();
			this.viewResults = new System.Windows.Forms.RadioButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.createDescription = new System.Windows.Forms.Label();
			this.reviewDescription = new System.Windows.Forms.Label();
			this.runDescription = new System.Windows.Forms.Label();
			this.viewDescription = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// NewScenario
			// 
			this.NewScenario.Location = new System.Drawing.Point(40, 88);
			this.NewScenario.Name = "NewScenario";
			this.NewScenario.Size = new System.Drawing.Size(216, 24);
			this.NewScenario.TabIndex = 0;
			this.NewScenario.Text = "Create Scenario From Market Plan";
			this.toolTip1.SetToolTip(this.NewScenario, "Create a new scenario based on an existing baseline scenario");
			this.NewScenario.CheckedChanged += new System.EventHandler(this.NewScenario_CheckedChanged);
			// 
			// ExistingScenario
			// 
			this.ExistingScenario.Location = new System.Drawing.Point(40, 144);
			this.ExistingScenario.Name = "ExistingScenario";
			this.ExistingScenario.Size = new System.Drawing.Size(160, 24);
			this.ExistingScenario.TabIndex = 1;
			this.ExistingScenario.Text = "Review Scenario";
			this.toolTip1.SetToolTip(this.ExistingScenario, "Modify a scenario that you have already created");
			this.ExistingScenario.CheckedChanged += new System.EventHandler(this.NewScenario_CheckedChanged);
			// 
			// RunSim
			// 
			this.RunSim.Location = new System.Drawing.Point(40, 200);
			this.RunSim.Name = "RunSim";
			this.RunSim.Size = new System.Drawing.Size(168, 24);
			this.RunSim.TabIndex = 3;
			this.RunSim.Text = "Run simulations";
			this.toolTip1.SetToolTip(this.RunSim, "Get results for scenarios");
			this.RunSim.CheckedChanged += new System.EventHandler(this.NewScenario_CheckedChanged);
			// 
			// viewResults
			// 
			this.viewResults.Location = new System.Drawing.Point(40, 256);
			this.viewResults.Name = "viewResults";
			this.viewResults.Size = new System.Drawing.Size(144, 24);
			this.viewResults.TabIndex = 4;
			this.viewResults.Text = "View Results";
			this.toolTip1.SetToolTip(this.viewResults, "View and compare results for scenarios that have been run");
			this.viewResults.CheckedChanged += new System.EventHandler(this.NewScenario_CheckedChanged);
			// 
			// createDescription
			// 
			this.createDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.createDescription.Location = new System.Drawing.Point(280, 88);
			this.createDescription.Name = "createDescription";
			this.createDescription.Size = new System.Drawing.Size(264, 32);
			this.createDescription.TabIndex = 5;
			this.createDescription.Text = "Choose this option to import a market plan file and create a new scenario.";
			// 
			// reviewDescription
			// 
			this.reviewDescription.Location = new System.Drawing.Point(280, 144);
			this.reviewDescription.Name = "reviewDescription";
			this.reviewDescription.Size = new System.Drawing.Size(264, 32);
			this.reviewDescription.TabIndex = 6;
			this.reviewDescription.Text = "Choose this option to review the marketing data for a scenario and change its tit" +
				"le and/or description.";
			// 
			// runDescription
			// 
			this.runDescription.Location = new System.Drawing.Point(280, 200);
			this.runDescription.Name = "runDescription";
			this.runDescription.Size = new System.Drawing.Size(264, 32);
			this.runDescription.TabIndex = 7;
			this.runDescription.Text = "Use this option to queue new scenarios and check the status of already queued sce" +
				"narios.";
			// 
			// viewDescription
			// 
			this.viewDescription.Location = new System.Drawing.Point(280, 256);
			this.viewDescription.Name = "viewDescription";
			this.viewDescription.Size = new System.Drawing.Size(264, 32);
			this.viewDescription.TabIndex = 8;
			this.viewDescription.Text = "Choose this option to view and compare results for different scenarios that have " +
				"already been run.";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(296, 40);
			this.label5.TabIndex = 15;
			this.label5.Text = "Start Page";
			// 
			// SelectScenario
			// 
			this.Controls.Add(this.label5);
			this.Controls.Add(this.viewDescription);
			this.Controls.Add(this.runDescription);
			this.Controls.Add(this.reviewDescription);
			this.Controls.Add(this.createDescription);
			this.Controls.Add(this.viewResults);
			this.Controls.Add(this.RunSim);
			this.Controls.Add(this.ExistingScenario);
			this.Controls.Add(this.NewScenario);
			this.Name = "SelectScenario";
			this.Size = new System.Drawing.Size(584, 440);
			this.ResumeLayout(false);

		}
		#endregion

		#region private_members_and_methods
		Database db = null;
		#endregion

		#region Wizard Members

		public bool Next()
		{
			return true;
		}

		public bool Back()
		{
			return true;
		}

		public void Start()
		{
			string query = "user_name = '" + Setup.Settings.User + "'";
			DataRow[] rows = db.Data.scenario.Select(query,"",DataViewRowState.CurrentRows);
			this.NewScenario.Checked = true;
			if(rows.Length > 0)
			{
				// existingScenario = true;

				ExistingScenario.Enabled = true;
			
				// how many are able to be run
				query = "user_name = '" + Setup.Settings.User + "' AND (sim_num <> 0 OR queued = 1)";
				rows = db.Data.scenario.Select(query,"",DataViewRowState.CurrentRows);

				if (rows.Length > 0)
				{
					RunSim.Enabled = true;
				}
				else
				{
					RunSim.Enabled = false;
				}

				// how many have been run
				query = "user_name = '" + Setup.Settings.User + "' AND sim_num = 0 AND queued = 0";
				rows = db.Data.scenario.Select(query,"",DataViewRowState.CurrentRows);

				if (rows.Length > 0)
				{
					if(RunSim.Enabled == false)
					{
						viewResults.Enabled = true;
					}
					viewResults.Checked = true;
				}
				else
				{
					viewResults.Enabled = false;
				}
			}
			else
			{
				ExistingScenario.Enabled = false;
				RunSim.Enabled = false;
				viewResults.Enabled = false;
			}
			

		}

		public void End()
		{
		}

		private void NewScenario_CheckedChanged(object sender, System.EventArgs e)
		{
			this.createDescription.ForeColor = Color.Gray;
			this.reviewDescription.ForeColor = Color.Gray;
			this.runDescription.ForeColor = Color.Gray;
			this.viewDescription.ForeColor = Color.Gray;

			if(this.NewScenario.Checked == true)
			{
				this.createDescription.ForeColor = Color.DarkBlue;
			}
			if(this.ExistingScenario.Checked == true)
			{
				this.reviewDescription.ForeColor = Color.DarkBlue;
			}
			if(this.RunSim.Checked == true)
			{
				this.runDescription.ForeColor = Color.DarkBlue;
			}
			if(this.viewResults.Checked == true)
			{
				this.viewDescription.ForeColor = Color.DarkBlue;
			}
		}


		public event BrandManager.Forms.Finished Done;

		#endregion
	}
}
