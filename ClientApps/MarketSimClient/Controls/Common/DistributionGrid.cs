using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;

namespace Common
{
	/// <summary>
	/// Summary description for DistributionGridControl.
	/// </summary>
	public class DistributionGridControl : MrktSimControl
	{

		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}


		public override void Refresh()
		{
			base.Refresh ();

			iDistributionGrid.Refresh();
		}

		public override void Flush()
		{
			iDistributionGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				iDistributionGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}
		private System.Windows.Forms.GroupBox dataBox;
		
		private Common.Utilities.MrktSimGrid iDistributionGrid;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private Common.Utilities.PlanData planData;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown persuasionBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown awarenessBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown attributeGBox;
		private System.Windows.Forms.NumericUpDown attributeFBox;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DistributionGridControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = PlanType.Distribution;
			mrktPlanComponent.Db = db;

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			planData.Db = db;
			planData.Type = PlanType.Distribution;
			planData.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);
			planData.CreatePlanRow +=new Common.Utilities.PlanData.ParsePlanData(planData_CreatePlanRow);
			
			this.SuspendLayout();

			iDistributionGrid.Table = theDb.Data.distribution;

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
			this.attributeGBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.attributeFBox = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.persuasionBox = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.awarenessBox = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.iDistributionGrid = new Common.Utilities.MrktSimGrid();
			this.dataBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();
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
			this.dataBox.Size = new System.Drawing.Size(832, 280);
			this.dataBox.TabIndex = 1;
			this.dataBox.TabStop = false;
			// 
			// planData
			// 
			this.planData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planData.Location = new System.Drawing.Point(568, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(261, 261);
			this.planData.Suspend = false;
			this.planData.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.attributeGBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.attributeFBox);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.persuasionBox);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.awarenessBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox1.Location = new System.Drawing.Point(320, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 261);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Distribution Data";
			// 
			// attributeGBox
			// 
			this.attributeGBox.DecimalPlaces = 2;
			this.attributeGBox.Location = new System.Drawing.Point(144, 64);
			this.attributeGBox.Name = "attributeGBox";
			this.attributeGBox.Size = new System.Drawing.Size(72, 20);
			this.attributeGBox.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 12;
			this.label1.Text = "% Considered";
			// 
			// attributeFBox
			// 
			this.attributeFBox.DecimalPlaces = 2;
			this.attributeFBox.Location = new System.Drawing.Point(144, 32);
			this.attributeFBox.Name = "attributeFBox";
			this.attributeFBox.Size = new System.Drawing.Size(72, 20);
			this.attributeFBox.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(64, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 10;
			this.label4.Text = "% Available";
			// 
			// persuasionBox
			// 
			this.persuasionBox.DecimalPlaces = 2;
			this.persuasionBox.Location = new System.Drawing.Point(144, 128);
			this.persuasionBox.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  -2147483648});
			this.persuasionBox.Name = "persuasionBox";
			this.persuasionBox.Size = new System.Drawing.Size(72, 20);
			this.persuasionBox.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(72, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 8;
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
			this.awarenessBox.Location = new System.Drawing.Point(144, 96);
			this.awarenessBox.Maximum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.awarenessBox.Name = "awarenessBox";
			this.awarenessBox.Size = new System.Drawing.Size(72, 20);
			this.awarenessBox.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 6;
			this.label2.Text = "Awareness Prob.";
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
			// iDistributionGrid
			// 
			this.iDistributionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iDistributionGrid.Location = new System.Drawing.Point(0, 280);
			this.iDistributionGrid.Name = "iDistributionGrid";
			this.iDistributionGrid.RowFilter = "";
			this.iDistributionGrid.RowID = null;
			this.iDistributionGrid.RowName = null;
			this.iDistributionGrid.Size = new System.Drawing.Size(832, 168);
			this.iDistributionGrid.Sort = "";
			this.iDistributionGrid.TabIndex = 0;
			// 
			// DistributionGridControl
			// 
			this.Controls.Add(this.iDistributionGrid);
			this.Controls.Add(this.dataBox);
			this.Name = "DistributionGridControl";
			this.Size = new System.Drawing.Size(832, 448);
			this.dataBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			iDistributionGrid.Clear();

			iDistributionGrid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			iDistributionGrid.AddTextColumn("product_name", true);
			iDistributionGrid.AddTextColumn("channel_name", true);

			iDistributionGrid.AddNumericColumn("attr_value_F", false);
			iDistributionGrid.AddNumericColumn("attr_value_G", false);
			
			iDistributionGrid.AddNumericColumn("message_awareness_probability", false);
			iDistributionGrid.AddNumericColumn("message_persuation_probability", false);
			iDistributionGrid.AddDateColumn("start_date");
			iDistributionGrid.AddDateColumn("end_date");
			
			iDistributionGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				iDistributionGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				iDistributionGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				iDistributionGrid.RowFilter = "";
			}

			planData.Plan = plan;
		}

		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
		{
			MrktSimDBSchema.distributionRow distribution = theDb.CreateDistribution(productID, channelID);
			distribution.start_date = start;
			distribution.end_date = end;

			distribution.attr_value_F = (double) attributeFBox.Value;
			distribution.attr_value_G = (double) attributeGBox.Value;
			distribution.message_awareness_probability = (double) this.awarenessBox.Value;
			distribution.message_persuation_probability = (double) this.persuasionBox.Value;
		}

		private string planData_CreatePlanRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length != 7)
				return "Expecting seven columns of data";

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

			MrktSimDBSchema.distributionRow distribution = theDb.CreateDistribution(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				distribution.attr_value_F = (double) Decimal.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.attr_value_G = (double) Decimal.Parse(items[2]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.message_awareness_probability = (double) Decimal.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				distribution.message_persuation_probability = (double) Decimal.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.start_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.end_date = DateTime.Parse(items[6]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}
	}
}
