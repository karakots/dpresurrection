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
	/// Summary description for MassMedia.
	/// </summary>
	public class MassMediaControl : MrktSimControl
	{
		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh ();

			iMediaGrid.Refresh();
		}



		public override void Flush()
		{
			iMediaGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				iMediaGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}
		private System.Windows.Forms.GroupBox dataBox;
		
		private Common.Utilities.MrktSimGrid iMediaGrid;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private Common.Utilities.PlanData planData;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown persuasionBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown awarenessBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown attributeHBox;
		private System.Windows.Forms.NumericUpDown attributeGBox;
		private System.Windows.Forms.ComboBox typeBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown attributeIBox;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MassMediaControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = PlanType.Media;
			mrktPlanComponent.Db = db;

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

		
			planData.Db = db;
			planData.Type = PlanType.Media;
			planData.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);
			planData.CreatePlanRow +=new Common.Utilities.PlanData.ParsePlanData(planData_CreatePlanRow);

			this.SuspendLayout();

			iMediaGrid.Table = theDb.Data.mass_media;

			createTableStyle();

			typeBox.SelectedIndex = 0;
			
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
			this.label6 = new System.Windows.Forms.Label();
			this.typeBox = new System.Windows.Forms.ComboBox();
			this.attributeIBox = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.attributeHBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.attributeGBox = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.persuasionBox = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.awarenessBox = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.iMediaGrid = new Common.Utilities.MrktSimGrid();
			this.dataBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.attributeIBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeHBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).BeginInit();
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
			this.dataBox.Size = new System.Drawing.Size(840, 280);
			this.dataBox.TabIndex = 3;
			this.dataBox.TabStop = false;
			// 
			// planData
			// 
			this.planData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planData.Location = new System.Drawing.Point(568, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(269, 261);
			this.planData.Suspend = false;
			this.planData.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.typeBox);
			this.groupBox1.Controls.Add(this.attributeIBox);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.attributeHBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.attributeGBox);
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
			this.groupBox1.Text = "Mass Media Data";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(88, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 23);
			this.label6.TabIndex = 25;
			this.label6.Text = "Type";
			// 
			// typeBox
			// 
			this.typeBox.Items.AddRange(new object[] {
														 "Absolute",
														 "Variable",
														 "Coupons",
														 "Samples"});
			this.typeBox.Location = new System.Drawing.Point(136, 24);
			this.typeBox.Name = "typeBox";
			this.typeBox.Size = new System.Drawing.Size(72, 21);
			this.typeBox.TabIndex = 24;
			this.typeBox.Text = "<Select>";
			// 
			// attributeIBox
			// 
			this.attributeIBox.DecimalPlaces = 2;
			this.attributeIBox.Location = new System.Drawing.Point(136, 120);
			this.attributeIBox.Name = "attributeIBox";
			this.attributeIBox.Size = new System.Drawing.Size(72, 20);
			this.attributeIBox.TabIndex = 23;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(32, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 23);
			this.label5.TabIndex = 22;
			this.label5.Text = "Redemption Rate";
			// 
			// attributeHBox
			// 
			this.attributeHBox.DecimalPlaces = 2;
			this.attributeHBox.Location = new System.Drawing.Point(136, 88);
			this.attributeHBox.Name = "attributeHBox";
			this.attributeHBox.Size = new System.Drawing.Size(72, 20);
			this.attributeHBox.TabIndex = 21;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 20;
			this.label1.Text = "Cost per Impression";
			// 
			// attributeGBox
			// 
			this.attributeGBox.DecimalPlaces = 2;
			this.attributeGBox.Location = new System.Drawing.Point(136, 56);
			this.attributeGBox.Name = "attributeGBox";
			this.attributeGBox.Size = new System.Drawing.Size(72, 20);
			this.attributeGBox.TabIndex = 19;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(88, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 23);
			this.label4.TabIndex = 18;
			this.label4.Text = "GRP";
			// 
			// persuasionBox
			// 
			this.persuasionBox.DecimalPlaces = 2;
			this.persuasionBox.Location = new System.Drawing.Point(136, 184);
			this.persuasionBox.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  -2147483648});
			this.persuasionBox.Name = "persuasionBox";
			this.persuasionBox.Size = new System.Drawing.Size(72, 20);
			this.persuasionBox.TabIndex = 17;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(40, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 23);
			this.label3.TabIndex = 16;
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
			this.awarenessBox.Location = new System.Drawing.Point(136, 152);
			this.awarenessBox.Maximum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.awarenessBox.Name = "awarenessBox";
			this.awarenessBox.Size = new System.Drawing.Size(72, 20);
			this.awarenessBox.TabIndex = 15;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 14;
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
			// iMediaGrid
			// 
			this.iMediaGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iMediaGrid.Location = new System.Drawing.Point(0, 280);
			this.iMediaGrid.Name = "iMediaGrid";
			this.iMediaGrid.RowFilter = "";
			this.iMediaGrid.RowID = null;
			this.iMediaGrid.RowName = null;
			this.iMediaGrid.Size = new System.Drawing.Size(840, 168);
			this.iMediaGrid.Sort = "";
			this.iMediaGrid.TabIndex = 0;
			// 
			// MassMediaControl
			// 
			this.Controls.Add(this.iMediaGrid);
			this.Controls.Add(this.dataBox);
			this.Name = "MassMediaControl";
			this.Size = new System.Drawing.Size(840, 448);
			this.dataBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.attributeIBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeHBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void createTableStyle()
		{
			iMediaGrid.Clear();

			iMediaGrid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);


			iMediaGrid.AddTextColumn("product_name", true);
			iMediaGrid.AddTextColumn("segment_name", true);
			iMediaGrid.AddTextColumn("channel_name", true);
			
			string[] types = {"A","V","C","S"};
			iMediaGrid.AddComboBoxColumn("media_type", types);
			iMediaGrid.AddNumericColumn("attr_value_G");
			// iMediaGrid.AddNumericColumn("attr_value_H");
			iMediaGrid.AddNumericColumn("attr_value_I");
			iMediaGrid.AddNumericColumn("message_awareness_probability");
			iMediaGrid.AddNumericColumn("message_persuation_probability");
			iMediaGrid.AddDateColumn("start_date");
			iMediaGrid.AddDateColumn("end_date");

			iMediaGrid.Reset();

		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				iMediaGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				iMediaGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				iMediaGrid.RowFilter = "";
			}

			planData.Plan = plan;
		}

		
		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
		{
			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(productID, channelID, segmentID);
			media.start_date = start;
			media.end_date = end;

			
			media.media_type = typeBox.SelectedItem.ToString()[0].ToString();
			media.attr_value_G = (double) attributeGBox.Value;
			media.attr_value_H = (double) attributeHBox.Value;
			media.attr_value_I = (double) attributeIBox.Value;
			media.message_awareness_probability = (double) awarenessBox.Value;
			media.message_persuation_probability = (double) persuasionBox.Value;
		}

		private string planData_CreatePlanRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			// we do not use cost per impression right now
			// const bool useCost = false;
			const bool doNotSeperateCoupons = false;

			if (items.Length < 8)
				return "Missing data from row";

			// first find segment
			int index = 0;

			string query = "segment_name = '" + items[index] + "'";
			DataRow[] rows = theDb.Data.segment.Select(query);

			if (rows.Length == 0)
			{
				return "could not find segment named " + items[index];
			}
			index++;
			
			int segment_id = (int) rows[0]["segment_id"];

			// first find channel
			query = "channel_name = '" + items[index] + "'";
			rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[index];
			}
			index++;
			
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			// type
			media.media_type = items[index];
			index++;

			// check for coupons
			bool Coupon = media.media_type == "C";

			// GRP or coupon reach
			try
			{
				media.attr_value_G = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			// cost per advertisement
			// if (useCost)
//			{
//				try
//				{
//					media.attr_value_H = (double) Decimal.Parse(items[index]);
//				}
//				catch(System.Exception e)
//				{
//					return e.Message;
//				}
//				index++;
//			}

			// coupon redemption rate
			if (doNotSeperateCoupons || Coupon)
			{
				try
				{
					media.attr_value_I = (double) Decimal.Parse(items[index]);
				}
				catch(System.Exception e)
				{
					return e.Message;
				}
				index++;
			}


			if (index == items.Length)
				return "Missing data from row";
			// awareness
			try
			{
				media.message_awareness_probability = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;
			
			if (index == items.Length)
				return "Missing data from row";
			// persuasion
			try
			{
				media.message_persuation_probability = (double) Decimal.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			if (index == items.Length)
				return "Missing data from row";
			// start date
			try
			{
				media.start_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			if (index == items.Length)
				return "Missing data from row";
			// end data
			try
			{
				media.end_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			return null;
		}
	}
}
