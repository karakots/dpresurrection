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
	public class DisplayGridControl : MrktSimControl
	{

		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh ();

			iDisplayGrid.Refresh();
		}


		public override void Flush()
		{
			iDisplayGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				iDisplayGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}
		private System.Windows.Forms.GroupBox dataBox;
		
		private Common.Utilities.MrktSimGrid iDisplayGrid;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private Common.Utilities.PlanData planData;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown attributeFBox;
		private System.Windows.Forms.NumericUpDown awarenessBox;
		private System.Windows.Forms.NumericUpDown persuasionBox;
	
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DisplayGridControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = PlanType.Display;

			mrktPlanComponent.Db = db;

			planData.Db = db;
			planData.Type = PlanType.Display;
			planData.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);
			planData.CreatePlanRow +=new Common.Utilities.PlanData.ParsePlanData(planData_CreatePlanRow);

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			this.SuspendLayout();

			iDisplayGrid.Table = theDb.Data.display;

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
			this.attributeFBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.iDisplayGrid = new Common.Utilities.MrktSimGrid();
			this.dataBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).BeginInit();
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
			this.planData.Location = new System.Drawing.Point(568, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(245, 261);
			this.planData.Suspend = false;
			this.planData.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.persuasionBox);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.awarenessBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.attributeFBox);
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
			// attributeFBox
			// 
			this.attributeFBox.DecimalPlaces = 2;
			this.attributeFBox.Location = new System.Drawing.Point(104, 32);
			this.attributeFBox.Name = "attributeFBox";
			this.attributeFBox.Size = new System.Drawing.Size(72, 20);
			this.attributeFBox.TabIndex = 1;
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
			this.mrktPlanComponent.Size = new System.Drawing.Size(317, 261);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 1;
			// 
			// iDisplayGrid
			// 
			this.iDisplayGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iDisplayGrid.Location = new System.Drawing.Point(0, 280);
			this.iDisplayGrid.Name = "iDisplayGrid";
			this.iDisplayGrid.RowFilter = "";
			this.iDisplayGrid.RowID = null;
			this.iDisplayGrid.RowName = null;
			this.iDisplayGrid.Size = new System.Drawing.Size(816, 144);
			this.iDisplayGrid.Sort = "";
			this.iDisplayGrid.TabIndex = 0;
			// 
			// DisplayGridControl
			// 
			this.Controls.Add(this.iDisplayGrid);
			this.Controls.Add(this.dataBox);
			this.Name = "DisplayGridControl";
			this.Size = new System.Drawing.Size(816, 424);
			this.dataBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			iDisplayGrid.Clear();

			iDisplayGrid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			iDisplayGrid.AddTextColumn("product_name", true);
			iDisplayGrid.AddTextColumn("channel_name", true);

			if (theDb.Model.app_code == "NIMO")
				iDisplayGrid.AddTextColumn("media_type", false);

			iDisplayGrid.AddNumericColumn("attr_value_F", false);
			
			iDisplayGrid.AddNumericColumn("message_awareness_probability", false);
			iDisplayGrid.AddNumericColumn("message_persuation_probability", false);
			iDisplayGrid.AddDateColumn("start_date");
			iDisplayGrid.AddDateColumn("end_date");
			
			iDisplayGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				iDisplayGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				iDisplayGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				iDisplayGrid.RowFilter = "";
			}

			planData.Plan = plan;
		}

		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
		{
			MrktSimDBSchema.displayRow display = theDb.CreateDisplay(productID, channelID);
			display.start_date = start;
			display.end_date = end;

			display.attr_value_F = (double) attributeFBox.Value;
			display.message_awareness_probability = (double) awarenessBox.Value;
			display.message_persuation_probability = (double) persuasionBox.Value;

			// will add display types later

		}

		private string planData_CreatePlanRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length != 6)
				return "Expecting six columns of data";

			// first find channel
			string query = "channel_name = '" + items[0] + "'";
			DataRow[] rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[0];
			}
			
		
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.displayRow display = theDb.CreateDisplay(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				display.attr_value_F = (double) Decimal.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.message_awareness_probability = (double) Decimal.Parse(items[2]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				display.message_persuation_probability = (double) Decimal.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.start_date = DateTime.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.end_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}
	}
}
