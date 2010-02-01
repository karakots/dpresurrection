using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
// using Common.Dialogs;
using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for CheckScenario.
	/// </summary>
	public class CheckScenario : System.Windows.Forms.UserControl, Wizard
	{
		private System.ComponentModel.IContainer components;

		public CheckScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			planTable = new DataTable("PlanTable");

			planTable.Columns.Add("plan", typeof(MrktSimDBSchema.market_planRow));
			planTable.Columns.Add("Name", typeof(string));
			planTable.Columns.Add("Type", typeof(string));
			planTable.Columns.Add("StartDate", typeof(DateTime));
			planTable.Columns.Add("EndDate", typeof(DateTime));


			this.componentGrid.Table = planTable;

			componentGrid.DescriptionWindow = false;

			this.componentGrid.AllowDelete = false;

			MarketPlanTypeCombo.SelectedIndex = 0;


			createCompTableStyle();
		}

		public bool GotoNext()
		{
			return true;
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
			this.helpBox = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.descrBox = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.MarketPlanTypeCombo = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.scenarioName = new System.Windows.Forms.TextBox();
			this.endLabel = new System.Windows.Forms.Label();
			this.startLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.dataButton = new System.Windows.Forms.Button();
			this.topPlanBox = new System.Windows.Forms.ComboBox();
			this.componentGrid = new MarketSimUtilities.MrktSimGrid();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(32, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = " Scenario:";
			// 
			// helpBox
			// 
			this.helpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.helpBox.Location = new System.Drawing.Point(24, 8);
			this.helpBox.Name = "helpBox";
			this.helpBox.Size = new System.Drawing.Size(552, 32);
			this.helpBox.TabIndex = 7;
			this.helpBox.Text = "Review the Marketing Data";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(32, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Description";
			// 
			// descrBox
			// 
			this.descrBox.AcceptsReturn = true;
			this.descrBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.descrBox.Location = new System.Drawing.Point(120, 112);
			this.descrBox.Multiline = true;
			this.descrBox.Name = "descrBox";
			this.descrBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.descrBox.Size = new System.Drawing.Size(384, 56);
			this.descrBox.TabIndex = 9;
			this.descrBox.Text = "";
			this.toolTip1.SetToolTip(this.descrBox, "Click here to change the scenario description");
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.MarketPlanTypeCombo);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.scenarioName);
			this.panel1.Controls.Add(this.endLabel);
			this.panel1.Controls.Add(this.startLabel);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.dataButton);
			this.panel1.Controls.Add(this.topPlanBox);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.helpBox);
			this.panel1.Controls.Add(this.descrBox);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(640, 296);
			this.panel1.TabIndex = 11;
			// 
			// MarketPlanTypeCombo
			// 
			this.MarketPlanTypeCombo.Items.AddRange(new object[] {
																	 "My Plans",
																	 "Baseline Plans",
																	 "Other Users Plans"});
			this.MarketPlanTypeCombo.Location = new System.Drawing.Point(128, 216);
			this.MarketPlanTypeCombo.Name = "MarketPlanTypeCombo";
			this.MarketPlanTypeCombo.Size = new System.Drawing.Size(176, 21);
			this.MarketPlanTypeCombo.TabIndex = 21;
			this.MarketPlanTypeCombo.SelectedIndexChanged += new System.EventHandler(this.MarketPlanTypeCombo_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(0, 216);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 23);
			this.label8.TabIndex = 20;
			this.label8.Text = "Market Plan Type:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(0, 248);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(128, 23);
			this.label7.TabIndex = 19;
			this.label7.Text = "Top Level Market Plans:";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(0, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(192, 16);
			this.label6.TabIndex = 18;
			this.label6.Text = "Market Plan Components";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(32, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(392, 16);
			this.label5.TabIndex = 17;
			this.label5.Text = "You can also change the scenario name and edit the description";
			// 
			// scenarioName
			// 
			this.scenarioName.Location = new System.Drawing.Point(120, 80);
			this.scenarioName.Name = "scenarioName";
			this.scenarioName.Size = new System.Drawing.Size(384, 20);
			this.scenarioName.TabIndex = 16;
			this.scenarioName.Text = "";
			this.toolTip1.SetToolTip(this.scenarioName, "Click here to change the scenario name");
			// 
			// endLabel
			// 
			this.endLabel.Location = new System.Drawing.Point(304, 184);
			this.endLabel.Name = "endLabel";
			this.endLabel.TabIndex = 15;
			// 
			// startLabel
			// 
			this.startLabel.Location = new System.Drawing.Point(112, 184);
			this.startLabel.Name = "startLabel";
			this.startLabel.TabIndex = 14;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(256, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 23);
			this.label4.TabIndex = 13;
			this.label4.Text = "End";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(56, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 23);
			this.label3.TabIndex = 12;
			this.label3.Text = "Start";
			// 
			// dataButton
			// 
			this.dataButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dataButton.Location = new System.Drawing.Point(448, 248);
			this.dataButton.Name = "dataButton";
			this.dataButton.Size = new System.Drawing.Size(160, 23);
			this.dataButton.TabIndex = 11;
			this.dataButton.Text = "View Component Details...";
			this.toolTip1.SetToolTip(this.dataButton, "View the data for the selected rows");
			this.dataButton.Click += new System.EventHandler(this.dataButton_Click);
			// 
			// topPlanBox
			// 
			this.topPlanBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.topPlanBox.Location = new System.Drawing.Point(128, 248);
			this.topPlanBox.Name = "topPlanBox";
			this.topPlanBox.Size = new System.Drawing.Size(304, 21);
			this.topPlanBox.TabIndex = 10;
			this.toolTip1.SetToolTip(this.topPlanBox, "Select market plan component");
			this.topPlanBox.SelectedIndexChanged += new System.EventHandler(this.topPlanBox_SelectedIndexChanged);
			// 
			// componentGrid
			// 
			this.componentGrid.DescribeRow = null;
			this.componentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentGrid.EnabledGrid = true;
			this.componentGrid.Location = new System.Drawing.Point(0, 296);
			this.componentGrid.Name = "componentGrid";
			this.componentGrid.RowFilter = null;
			this.componentGrid.RowID = null;
			this.componentGrid.RowName = null;
			this.componentGrid.Size = new System.Drawing.Size(640, 192);
			this.componentGrid.Sort = "";
			this.componentGrid.TabIndex = 13;
			this.componentGrid.Table = null;
			// 
			// CheckScenario
			// 
			this.Controls.Add(this.componentGrid);
			this.Controls.Add(this.panel1);
			this.Name = "CheckScenario";
			this.Size = new System.Drawing.Size(640, 488);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

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
			this.startLabel.Text = scenario.start_date.ToShortDateString();
			this.endLabel.Text = scenario.end_date.ToShortDateString();
		}

		public void End()
		{
			bool update = false;
			if(scenario.name != scenarioName.Text)
			{
				scenario.name = scenarioName.Text;
				update = true;
			}
			if(scenario.descr != descrBox.Text)
			{
				scenario.descr = descrBox.Text;
				update = true;
			}
			if(update)
			{
				db.Update();
			}
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region Public properties and methods
		public MrktSimDb.MrktSimDBSchema.scenarioRow CurrentScenario
		{
			set
			{
				scenario = value;

				this.scenarioName.Text = scenario.name;
				this.descrBox.Text = scenario.descr;

				this.startLabel.Text = scenario.start_date.ToShortDateString();
				this.endLabel.Text = scenario.end_date.ToShortDateString();

				setupPlanInfo();
			}

			get
			{
				return scenario;
			}
		}

		public Database Db
		{
			set
			{
				db = value;
			}

			get
			{
				return db;
			}
		}

		#endregion

		#region private data and methods
		Database db = null;
		MrktSimDb.MrktSimDBSchema.scenarioRow scenario = null;
		DataTable planTable;
		PlanType currentPlanType;


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label helpBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox descrBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox topPlanBox;
		private System.Windows.Forms.Button dataButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label startLabel;
		private System.Windows.Forms.Label endLabel;
		private System.Windows.Forms.TextBox scenarioName;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox MarketPlanTypeCombo;
		private MrktSimGrid componentGrid;

	

		private void setupPlanInfo()
		{
			if(scenario == null)
			{
				return;
			}
			// find media plans
			MrktSimDBSchema.scenario_market_planRow[] topLevelPlanRefs = scenario.Getscenario_market_planRows();

			if (topLevelPlanRefs.Length == 0)
				return;

			// find all top level plans that only belong to this scenario



			ArrayList list = new ArrayList();

			foreach (MrktSimDBSchema.scenario_market_planRow topPlanRef in topLevelPlanRefs)
			{
				MrktSimDBSchema.market_planRow topPlan = topPlanRef.market_planRow;
				if(MarketPlanTypeCombo.SelectedIndex == 0)
				{
					if(topPlan.user_name != Setup.Settings.User)
					{
						continue;
					}
				}
				else if(MarketPlanTypeCombo.SelectedIndex == 1)
				{
					if(topPlan.user_name != "admin")
					{
						continue;
					}
				}
				else if(MarketPlanTypeCombo.SelectedIndex == 2)
				{
					if(topPlan.user_name == "admin" || topPlan.user_name == Setup.Settings.User)
					{
						continue;
					}
				}
				list.Add(topPlan);
			}

			topPlanBox.DataSource = list;
			topPlanBox.DisplayMember = "name";

			// set the "New Plan" as the currently selected

			foreach(MrktSimDBSchema.market_planRow topPlan in list)
			{
				if (topPlan.Getscenario_market_planRows().Length == 1)
				{
					topPlanBox.SelectedItem = topPlan;
					break;
				}
			}

			topPlanBox_SelectedIndexChanged(null, null);
		}
		#endregion

		private void topPlanBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			// fill the planBox with plan components
			MrktSimDBSchema.market_planRow topPlan = (MrktSimDBSchema.market_planRow) topPlanBox.SelectedItem;

			planTable.Clear();

			if (topPlan == null)
			{
				this.Cursor = Cursors.Arrow;
				return;
			}

			// find all plan components
			MrktSimDBSchema.market_plan_treeRow[] tree = topPlan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_parent();

			foreach (MrktSimDBSchema.market_plan_treeRow row in tree)
			{
				MrktSimDBSchema.market_planRow plan = row.market_planRowBymarket_planmarket_plan_tree_child;

				DataRow planRow = planTable.NewRow();

				planRow["plan"] = plan;
				planRow["Name"] = plan.name;
				planRow["Type"] = plan.market_plan_typeRow.type;
				planRow["StartDate"] = plan.start_date;
				planRow["EndDate"] = plan.end_date;

				planTable.Rows.Add(planRow);
			}

			if (componentGrid.CurrentRow == null)
			{
				this.dataButton.Enabled = false;
			}
			else
			{
				this.dataButton.Enabled = true;
			}
			this.Cursor = Cursors.Arrow;

		}

		private void createCompTableStyle()
		{
			this.componentGrid.Clear();

			this.componentGrid.AddTextColumn("Name", true);
			this.componentGrid.AddTextColumn("Type", true);
			this.componentGrid.AddDateColumn("StartDate", "Start", true);
			this.componentGrid.AddDateColumn("EndDate", "End", true);


			componentGrid.Reset();
		}

		private void dataButton_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			DataRow row = this.componentGrid.CurrentRow;

			if (row == null)
			{
				this.Cursor = Cursors.Arrow;
				return;
			}

			MrktSimDBSchema.market_planRow plan = (MrktSimDBSchema.market_planRow) row["plan"];

			currentPlanType = (PlanType) plan.type;

			using (GridView dlg = new GridView())
			{

				dlg.CreateTableStyle +=new GridView.TableStyle(createTableStyle);

				switch(currentPlanType)
				{
					case PlanType.Coupons:
						dlg.Table = Db.Data.mass_media;
						break;
					case PlanType.Display:
						dlg.Table = Db.Data.display;
						break;
					case PlanType.Distribution:
						dlg.Table = Db.Data.distribution;
						break;
					case PlanType.Market_Utility:
						dlg.Table = Db.Data.market_utility;
						break;
					case PlanType.Media:
						dlg.Table = Db.Data.mass_media;
						break;
					case PlanType.Price:
						dlg.Table = Db.Data.product_channel;
						break;
				}

				Db.ReadPlanData(plan);
			
				dlg.RowFilter = "market_plan_id = " + plan.id;

				dlg.Text = currentPlanType.ToString() + " Plan Data for " + plan.productRow.product_name + " " + plan.name;

				dlg.ReadOnly = true;
			
				dlg.ShowDialog();
			}
			this.Cursor = Cursors.Arrow;
		}

		private void createTableStyle(MrktSimGrid grid)
		{
			Common.MarketComponentControl.CreateTableStyle(this.Db, grid, currentPlanType);
		}

		private void MarketPlanTypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			setupPlanInfo();
			this.Cursor = Cursors.Arrow;
		}

	}
}
