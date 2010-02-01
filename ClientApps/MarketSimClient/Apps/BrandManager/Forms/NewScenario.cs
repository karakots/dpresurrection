using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

// using Common.Utilities;
using MrktSimDb;
using BrandManager.Utilities;
using ExcelInterface;

using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for NewScenario.
	/// </summary>
	public class NewScenario : System.Windows.Forms.UserControl, Wizard
	{
		private System.ComponentModel.IContainer components;

		public NewScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
			this.label1 = new System.Windows.Forms.Label();
			this.baselineLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ImportButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.fileNameLabel = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Strategic Scenario:";
			// 
			// baselineLabel
			// 
			this.baselineLabel.BackColor = System.Drawing.SystemColors.Window;
			this.baselineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.baselineLabel.Location = new System.Drawing.Point(152, 152);
			this.baselineLabel.Name = "baselineLabel";
			this.baselineLabel.Size = new System.Drawing.Size(328, 16);
			this.baselineLabel.TabIndex = 2;
			this.toolTip1.SetToolTip(this.baselineLabel, "Baseline scenario to be modified or copied");
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 184);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Market Plan File:";
			// 
			// ImportButton
			// 
			this.ImportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ImportButton.Location = new System.Drawing.Point(24, 224);
			this.ImportButton.Name = "ImportButton";
			this.ImportButton.Size = new System.Drawing.Size(168, 23);
			this.ImportButton.TabIndex = 5;
			this.ImportButton.Text = "Select Maket Plan File";
			this.toolTip1.SetToolTip(this.ImportButton, "Browse for a market plan file");
			this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.clearButton.Location = new System.Drawing.Point(232, 224);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(168, 23);
			this.clearButton.TabIndex = 6;
			this.clearButton.Text = "Clear Market Plan File";
			this.toolTip1.SetToolTip(this.clearButton, "Clears the current market plan file so a copy of the baseline can be craeted");
			this.clearButton.Visible = false;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(24, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(304, 32);
			this.label3.TabIndex = 7;
			this.label3.Text = "Select a Market Plan File";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(24, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(416, 64);
			this.label4.TabIndex = 8;
			this.label4.Text = "Please select a market plan file to use with this baseline scenario. If no market" +
				" plan is selected then a copy of the current baseline scenario will be made";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 264);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(408, 40);
			this.label5.TabIndex = 9;
			this.label5.Text = "Note: Clicking Next will import the data which may take a few minutes depending o" +
				"n the size of the model";
			// 
			// fileNameLabel
			// 
			this.fileNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.fileNameLabel.Location = new System.Drawing.Point(152, 184);
			this.fileNameLabel.Name = "fileNameLabel";
			this.fileNameLabel.Size = new System.Drawing.Size(328, 20);
			this.fileNameLabel.TabIndex = 11;
			this.fileNameLabel.Text = "textBox1";
			// 
			// NewScenario
			// 
			this.Controls.Add(this.fileNameLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.ImportButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.baselineLabel);
			this.Controls.Add(this.label1);
			this.Name = "NewScenario";
			this.Size = new System.Drawing.Size(504, 360);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods

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

		public MrktSimDb.MrktSimDBSchema.scenarioRow BaselineScenario
		{
			set
			{
				scenario = value;
				
				newScenario = null;
				this.fileNameLabel.Text = "";

				if (scenario != null)
				{
					this.baselineLabel.Text = scenario.name;
				}
			}

			get
			{
				return scenario;
			}
		}

		public MrktSimDb.MrktSimDBSchema.scenarioRow CurrentScenario
		{
			set
			{
				newScenario = value;
			}

			get
			{
				return newScenario;
			}
		}
		
		#endregion

		#region Wizard Members

		public bool Next()
		{
			return this.import_scenario();
		}

		public bool Back()
		{
			if(this.newScenario != null)
			{
				MessageBox.Show("Please continue with this scenario before creating a new one","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			return true;
		}

		public void Start()
		{

		}

		public void End()
		{
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region Private Data and methods
		
		private MrktSimDb.MrktSimDBSchema.scenarioRow scenario = null;
		private MrktSimDb.MrktSimDBSchema.scenarioRow newScenario = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ImportButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label baselineLabel;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox fileNameLabel;

		private Database db = null;

		private bool import_scenario()
		{
			if(this.fileNameLabel.Text != "")
			{
				string fileName = this.fileNameLabel.Text;

				System.IO.FileInfo info = new System.IO.FileInfo(fileName);

				System.IO.FileStream stream = null;

				try
				{
					stream = info.Open(System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None);
				}
				catch(Exception oops)
				{
					MessageBox.Show(oops.Message,"File busy",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}

				stream.Close();

				newScenario = ScenarioUtilities.CopyScenario(Db, BaselineScenario, fileName);

				if(newScenario == null)
				{
					MessageBox.Show("Error: There was an error creating the new scenario","Creation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
				
				ArrayList list = new ArrayList();

				// scenario name
				ExcelReader.ReadCell(fileName, "Dashboard", "B3", "B4", out list);

				if (list != null && list.Count > 0)
				{
					newScenario.descr += " from file " + newScenario.name + " ";
					newScenario.name = this.db.CreateUniqueName(db.Data.scenario, "name", list[0].ToString());
				}

				// description
				ExcelReader.ReadCell(fileName, "Dashboard", "B7", "B8", out list);
				if (list != null && list.Count > 0)
				{
					newScenario.descr += list[0].ToString();
				}

				// start and end date
				ExcelReader.ReadCell(fileName, "Dashboard", "B4", "B7", out list);
				if (list != null && list.Count > 1)
				{
					newScenario.start_date = (DateTime) list[0];
					newScenario.end_date = (DateTime) list[2];
				}
			}
			else
			{
				newScenario = ScenarioUtilities.CopyScenario(Db, BaselineScenario);
				if(newScenario == null)
				{
					MessageBox.Show("Error: There was an error creating the new scenario","Creation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
			}
			newScenario.user_name = Setup.Settings.User;
			foreach(MrktSimDBSchema.market_planRow plan in Db.Data.market_plan.Select("user_name = 'admin'", "", DataViewRowState.Added))
			{
				plan.user_name = Setup.Settings.User;
			}
			allignPlansWithData();
			db.Update();
			return true;
		}

		private void allignPlansWithData()
		{
			string query = "type <> 0 AND user_name = '" + Setup.Settings.User + "'";

			foreach(MrktSimDBSchema.market_planRow plan in Db.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows))
			{
				allignPlanWithData(plan);
			}

			/*query = "type = 0 AND user_name = '" + Setup.Settings.User + "'";
			foreach(MrktSimDBSchema.market_planRow plan in Db.Data.market_plan.Select(query, "", DataViewRowState.Added))
			{
				allignPlanWithData(plan);
			}*/

		}

		private void allignPlanWithData(MrktSimDBSchema.market_planRow plan)
		{
			if (plan.type == 0)
			{
				// this is a top level plan

				return;
			}

			string query = "market_plan_id = " + plan.id;

			DataRow[] rows = null;

			switch((PlanType) plan.type)
			{
				case PlanType.Media:

					rows = Db.Data.mass_media.Select(query, "", DataViewRowState.CurrentRows);

					break;
				case PlanType.Display:

					rows = Db.Data.display.Select(query, "", DataViewRowState.CurrentRows);

					break;
				case PlanType.Distribution:

					rows = Db.Data.distribution.Select(query, "", DataViewRowState.CurrentRows);

					break;
				case PlanType.Market_Utility:

					rows = Db.Data.market_utility.Select(query, "", DataViewRowState.CurrentRows);

					break;
				case PlanType.Price:

					rows = Db.Data.product_channel.Select(query, "", DataViewRowState.CurrentRows);

					break;
				case PlanType.Coupons:

					rows = Db.Data.mass_media.Select(query, "", DataViewRowState.CurrentRows);

					break;
			}

			// find max min of dates

			if (rows.Length == 0)
			{
				return;
			}

			DateTime start = (DateTime) rows[0]["start_date"];
			DateTime end = (DateTime) rows[0]["end_date"];

			foreach(DataRow row in rows)
			{
				DateTime curStart = (DateTime)  row["start_date"];
				DateTime curEnd = (DateTime) row["end_date"];

				if (start > curStart)
					start = curStart;

				if (end < curEnd)
					end = curEnd;
			}

			plan.start_date = start;
			plan.end_date = end;
		}

		
		#endregion

		#region UI
		private void clearButton_Click(object sender, System.EventArgs e)
		{
			this.fileNameLabel.Text = "";
		}

		private void ImportButton_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.InitialDirectory = Setup.Settings.LocalDirectory;

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			openFileDlg.Title = "Select a Market Plan File";

			// openFileDlg.ReadOnlyChecked = false;
			DialogResult frslt = openFileDlg.ShowDialog();
			
			if (frslt == DialogResult.OK)
			{
				this.fileNameLabel.Text = openFileDlg.FileName;
			}
		}

		#endregion
	}
}
