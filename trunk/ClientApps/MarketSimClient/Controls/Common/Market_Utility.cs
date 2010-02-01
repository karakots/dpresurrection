using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using Common.Dialogs;

namespace Common
{
	/// <summary>
	/// Summary description for DisplayGrid.
	/// </summary>
	public class MarketUtilityGridControl : MrktSimControl
	{

		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh ();

			iMarketUtilityGrid.Refresh();
		}


		public override void Flush()
		{
			iMarketUtilityGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				iMarketUtilityGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}
		private System.Windows.Forms.GroupBox dataBox;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private Common.Utilities.PlanData planData;
		private Common.Utilities.MrktSimGrid iMarketUtilityGrid;
		private System.Windows.Forms.NumericUpDown persuasionBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown awarenessBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown utilityBox;
		private System.Windows.Forms.NumericUpDown percentBox;
	
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MarketUtilityGridControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = PlanType.Market_Utility;

			mrktPlanComponent.Db = db;

			planData.Db = db;
			planData.Type = PlanType.Market_Utility;
			planData.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);
			planData.CreatePlanRow +=new Common.Utilities.PlanData.ParsePlanData(planData_CreatePlanRow);

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			this.SuspendLayout();

			iMarketUtilityGrid.Table = theDb.Data.market_utility;

			createTableStyle();
			
			this.ResumeLayout(false);
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
			this.dataBox = new System.Windows.Forms.GroupBox();
			this.planData = new Common.Utilities.PlanData();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.persuasionBox = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.awarenessBox = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.percentBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.iMarketUtilityGrid = new Common.Utilities.MrktSimGrid();
			this.label4 = new System.Windows.Forms.Label();
			this.utilityBox = new System.Windows.Forms.NumericUpDown();
			this.dataBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.percentBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.utilityBox)).BeginInit();
			this.SuspendLayout();
			// 
			// dataBox
			// 
			this.dataBox.Controls.Add(this.planData);
			this.dataBox.Controls.Add(this.groupBox1);
			this.dataBox.Controls.Add(this.mrktPlanComponent);
			this.dataBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.dataBox.Location = new System.Drawing.Point(0, 0);
			this.dataBox.Name = "dataBox";
			this.dataBox.Size = new System.Drawing.Size(816, 280);
			this.dataBox.TabIndex = 1;
			this.dataBox.TabStop = false;
			// 
			// planData
			// 
			this.planData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planData.Enabled = false;
			this.planData.Location = new System.Drawing.Point(568, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(245, 261);
			this.planData.Suspend = false;
			this.planData.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.utilityBox);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.persuasionBox);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.awarenessBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.percentBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox1.Location = new System.Drawing.Point(320, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 261);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Display Data";
			// 
			// persuasionBox
			// 
			this.persuasionBox.DecimalPlaces = 2;
			this.persuasionBox.Location = new System.Drawing.Point(104, 96);
			this.persuasionBox.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  -2147483648});
			this.persuasionBox.Name = "persuasionBox";
			this.persuasionBox.Size = new System.Drawing.Size(72, 20);
			this.persuasionBox.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Persuasion";
			// 
			// awarenessBox
			// 
			this.awarenessBox.DecimalPlaces = 2;
			this.awarenessBox.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   65536});
			this.awarenessBox.Location = new System.Drawing.Point(104, 64);
			this.awarenessBox.Maximum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.awarenessBox.Name = "awarenessBox";
			this.awarenessBox.Size = new System.Drawing.Size(72, 20);
			this.awarenessBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Awareness Prob.";
			// 
			// percentBox
			// 
			this.percentBox.DecimalPlaces = 2;
			this.percentBox.Location = new System.Drawing.Point(104, 32);
			this.percentBox.Name = "percentBox";
			this.percentBox.Size = new System.Drawing.Size(72, 20);
			this.percentBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "% Available";
			// 
			// mrktPlanComponent
			// 
			this.mrktPlanComponent.Dock = System.Windows.Forms.DockStyle.Left;
			this.mrktPlanComponent.Location = new System.Drawing.Point(3, 16);
			this.mrktPlanComponent.Name = "mrktPlanComponent";
			this.mrktPlanComponent.SelectedPlan = null;
			this.mrktPlanComponent.Size = new System.Drawing.Size(317, 261);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 1;
			// 
			// iMarketUtilityGrid
			// 
			this.iMarketUtilityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iMarketUtilityGrid.Location = new System.Drawing.Point(0, 280);
			this.iMarketUtilityGrid.Name = "iMarketUtilityGrid";
			this.iMarketUtilityGrid.RowFilter = "";
			this.iMarketUtilityGrid.RowID = null;
			this.iMarketUtilityGrid.RowName = null;
			this.iMarketUtilityGrid.Size = new System.Drawing.Size(816, 144);
			this.iMarketUtilityGrid.Sort = "";
			this.iMarketUtilityGrid.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(56, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Utility";
			// 
			// utilityBox
			// 
			this.utilityBox.DecimalPlaces = 2;
			this.utilityBox.Location = new System.Drawing.Point(104, 128);
			this.utilityBox.Name = "utilityBox";
			this.utilityBox.Size = new System.Drawing.Size(72, 20);
			this.utilityBox.TabIndex = 7;
			// 
			// MarketUtilityGridControl
			// 
			this.Controls.Add(this.iMarketUtilityGrid);
			this.Controls.Add(this.dataBox);
			this.Name = "MarketUtilityGridControl";
			this.Size = new System.Drawing.Size(816, 424);
			this.dataBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.percentBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.utilityBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			iMarketUtilityGrid.Clear();

			iMarketUtilityGrid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			iMarketUtilityGrid.AddTextColumn("product_name", true);
			iMarketUtilityGrid.AddTextColumn("channel_name", true);
			iMarketUtilityGrid.AddTextColumn("segment_name", true);
			
			iMarketUtilityGrid.AddNumericColumn("percent_dist", false);
			iMarketUtilityGrid.AddNumericColumn("awareness", false);
			iMarketUtilityGrid.AddNumericColumn("persuasion", false);
			iMarketUtilityGrid.AddNumericColumn("utility", false);

			iMarketUtilityGrid.AddDateColumn("start_date");
			iMarketUtilityGrid.AddDateColumn("end_date");
			
			iMarketUtilityGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				iMarketUtilityGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				iMarketUtilityGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				iMarketUtilityGrid.RowFilter = "";
			}

			planData.Plan = plan;
		}

		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
		{
			MrktSimDBSchema.market_utilityRow market_utility = theDb.CreateMarketUtility(productID, channelID, segmentID);
			market_utility.start_date = start;
			market_utility.end_date = end;

			market_utility.percent_dist = (double) percentBox.Value;
			market_utility.awareness = (double) awarenessBox.Value;
			market_utility.persuasion = (double) persuasionBox.Value;
			market_utility.utility = (double) utilityBox.Value;

			// will add display types later

		}

		private string planData_CreatePlanRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length != 8)
				return "Expecting eight columns of data";

			
			int index = 0;

			// first find channel
			string query = "channel_name = '" + items[index] + "'";
			DataRow[] rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[index];
			}
			
		
			int channel_id = (int) rows[0]["channel_id"];

			index++;

			// then find segment

			query = "segment_name = '" + items[index] + "'";
			rows = theDb.Data.segment.Select(query);

			if (rows.Length == 0)
			{
				return "could not find segment named " + items[index];
			}
			
			int segment_id = (int) rows[0]["segment_id"];
			

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.market_utilityRow market_utility = theDb.CreateMarketUtility(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			index++;

			try
			{
				market_utility.percent_dist = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			index++;

			try
			{
				market_utility.awareness = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			index++;
			
			try
			{
				market_utility.persuasion = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			index++;

			try
			{
				market_utility.utility = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			index++;

			try
			{
				market_utility.start_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			index++;

			try
			{
				market_utility.end_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}
	}
}
