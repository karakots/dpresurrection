using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Dialogs;
using MarketSimUtilities;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for PlanData.
	/// </summary>
	public class PlanData : MrktSimControl
	{

		public delegate void CreatePlanData(int productID, int channelID, int segmentID, DateTime start, DateTime end);

		public event CreatePlanData NewPlanRow;

		public delegate string ParsePlanData(MrktSimDBSchema.market_planRow thePlan, string[] items);

		MrktSimDBSchema.market_planRow plan = null;

		public override ModelDb Db
		{
			set
			{
				base.Db = value;
				channelPicker.Db = theDb;
				segmentPicker.Db = theDb;

				startEndDate.Db = theDb;
			}
		}


		public ModelDb.PlanType Type
		{
			set
			{
				switch(value)
				{
					case ModelDb.PlanType.MarketPlan:
						channelPicker.Visible = false;
						segmentPicker.Visible = false;
						segmentPicker.SegmentID = ModelDb.AllID;
						break;
					case ModelDb.PlanType.Price:
						channelPicker.Visible = true;
						channelPicker.AlLowAll = false;
						segmentPicker.Visible = false;
						segmentPicker.SegmentID = ModelDb.AllID;
						break;

					case ModelDb.PlanType.Distribution:
						channelPicker.Visible = true;
						segmentPicker.Visible = false;
						segmentPicker.SegmentID = ModelDb.AllID;
						break;

					case ModelDb.PlanType.Display:
						channelPicker.Visible = true;
						segmentPicker.Visible = false;
						segmentPicker.SegmentID = ModelDb.AllID;
						break;

					case ModelDb.PlanType.Media:
						channelPicker.Visible = true;
						segmentPicker.Visible = true;
						break;

					case ModelDb.PlanType.Coupons:
						channelPicker.Visible = true;
						segmentPicker.Visible = true;
						break;


					case ModelDb.PlanType.ProdEvent:
						channelPicker.Visible = true;
						segmentPicker.Visible = true;
						
						break;

					case ModelDb.PlanType.TaskEvent:
						channelPicker.Visible = false;
						channelPicker.ChannelID = ModelDb.AllID;
						segmentPicker.Visible = true;
						break;
				}
			}
		}

		public MrktSimDBSchema.market_planRow Plan
		{
			set
			{
				plan = value;

				if (plan == null)
				{
					this.Enabled = false;
//					dataTiming.Enabled = false;
//					createDataButton.Enabled = false;
				}
				else
				{
					this.Enabled = true;
//					dataTiming.Enabled = true;
//					createDataButton.Enabled = true;

					this.startEndDate.Start = plan.start_date;
					this.startEndDate.End = plan.end_date;
				}
			}
		}

		bool foreachChannel = false;

		private System.Windows.Forms.GroupBox createDataBox;
		private Common.Utilities.Timing dataTiming;
		private System.Windows.Forms.Button createDataButton;
		private Common.Utilities.ChannelPicker channelPicker;
		private Common.Utilities.SegmentPicker segmentPicker;
		private Common.Utilities.StartEndDate startEndDate;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PlanData()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// start off disabled
			this.Enabled = false;
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
			this.createDataBox = new System.Windows.Forms.GroupBox();
			this.startEndDate = new Common.Utilities.StartEndDate();
			this.segmentPicker = new Common.Utilities.SegmentPicker();
			this.channelPicker = new Common.Utilities.ChannelPicker();
			this.dataTiming = new Common.Utilities.Timing();
			this.createDataButton = new System.Windows.Forms.Button();
			this.createDataBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// createDataBox
			// 
			this.createDataBox.Controls.Add(this.startEndDate);
			this.createDataBox.Controls.Add(this.segmentPicker);
			this.createDataBox.Controls.Add(this.channelPicker);
			this.createDataBox.Controls.Add(this.dataTiming);
			this.createDataBox.Controls.Add(this.createDataButton);
			this.createDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createDataBox.Location = new System.Drawing.Point(0, 0);
			this.createDataBox.Name = "createDataBox";
			this.createDataBox.Size = new System.Drawing.Size(320, 224);
			this.createDataBox.TabIndex = 22;
			this.createDataBox.TabStop = false;
			// 
			// startEndDate
			// 
			this.startEndDate.End = new System.DateTime(2005, 6, 8, 15, 9, 56, 500);
			this.startEndDate.Location = new System.Drawing.Point(32, 160);
			this.startEndDate.Name = "startEndDate";
			this.startEndDate.Size = new System.Drawing.Size(160, 48);
			this.startEndDate.Start = new System.DateTime(2005, 6, 8, 15, 9, 56, 500);
			this.startEndDate.TabIndex = 18;
			// 
			// segmentPicker
			// 
			this.segmentPicker.Location = new System.Drawing.Point(24, 128);
			this.segmentPicker.Name = "segmentPicker";
			this.segmentPicker.SegmentID = -1;
			this.segmentPicker.Size = new System.Drawing.Size(216, 24);
			this.segmentPicker.TabIndex = 16;
			// 
			// channelPicker
			// 
			this.channelPicker.ChannelID = -1;
			this.channelPicker.Location = new System.Drawing.Point(24, 96);
			this.channelPicker.Name = "channelPicker";
			this.channelPicker.Size = new System.Drawing.Size(232, 24);
			this.channelPicker.TabIndex = 15;
			// 
			// dataTiming
			// 
			this.dataTiming.Location = new System.Drawing.Point(16, 24);
			this.dataTiming.Name = "dataTiming";
			this.dataTiming.Size = new System.Drawing.Size(176, 64);
			this.dataTiming.TabIndex = 14;
			// 
			// createDataButton
			// 
			this.createDataButton.Location = new System.Drawing.Point(216, 184);
			this.createDataButton.Name = "createDataButton";
			this.createDataButton.Size = new System.Drawing.Size(80, 23);
			this.createDataButton.TabIndex = 13;
			this.createDataButton.Text = "Create Data";
			this.createDataButton.Click += new System.EventHandler(this.createDataButton_Click);
			// 
			// PlanData
			// 
			this.Controls.Add(this.createDataBox);
			this.Name = "PlanData";
			this.Size = new System.Drawing.Size(320, 224);
			this.createDataBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void createPlanData(int productID, int channelID, int segmentID, int taskID, DateTime start, DateTime end)
		{			
			if (foreachChannel && channelID == AllID)
			{
				// create for all channels
				string filterAll = "channel_id <> " + MrktSimControl.AllID;
		
				foreach(DataRow dRow in theDb.Data.channel.Select(filterAll,"", DataViewRowState.CurrentRows))
				{
					createPlanData(productID, ((MrktSimDBSchema.channelRow) dRow).channel_id, segmentID, taskID, start, end);
				}

				return;
			}

			if (NewPlanRow != null)
				NewPlanRow(productID, channelID, segmentID, start, end);
		}

		// create data
		private void createPlanData(MrktSimDBSchema.market_planRow plan, DateTime start, DateTime end)
		{
			int product_id = plan.product_id;
			int channel_id = channelPicker.ChannelID;
			int segment_id = segmentPicker.SegmentID;
			int task_id = plan.task_id;

			createPlanData(product_id, channel_id, segment_id, task_id, start, end);
		}

		// create data
		private void createPlanData(MrktSimDBSchema.market_planRow plan, Timing.TimingType interval)
		{
			TimeSpan oneDay = new TimeSpan(1,0,0,0,0);

			DateTime endDate = this.startEndDate.End;

			DateTime start = this.startEndDate.Start;
			DateTime end = this.startEndDate.Start;

			if (interval == Timing.TimingType.None)
				end = endDate;
			else if (interval == Timing.TimingType.Weekly)
				end = start.AddDays(6);
			else if (interval == Timing.TimingType.Monthly)
			{
				end = start.AddMonths(1);
				end = end.AddDays(-1);
			}
			else if (interval == Timing.TimingType.Yearly)
			{
				end = start.AddYears(1);
				end = end.AddDays(-1);
			}
	
			while (end < endDate)
			{
				createPlanData(plan, start, end);

				start = end + oneDay;

				if (interval == Timing.TimingType.Weekly)
				{
					end = start.AddDays(6);
				}
				else if (interval == Timing.TimingType.Daily)
				{
					end = start;
				}
				else if (interval == Timing.TimingType.Monthly)
				{
					end = start.AddMonths(1);
					end = end.AddDays(-1);
				}
				else if (interval == Timing.TimingType.Yearly)
				{
					end = start.AddYears(1);
					end = end.AddDays(-1);
				}
			}

			createPlanData(plan, start, end);				
		}

		// create data
		private void createPlanData(MrktSimDBSchema.market_planRow plan, int num)
		{
			if (num == 0)
				return;

			TimeSpan oneDay = new TimeSpan(1,0,0,0,0);

			DateTime start = this.startEndDate.Start;
			DateTime EndDate = this.startEndDate.End;

			TimeSpan total = EndDate - start;

			int numDays = total.Days;

			int interval = numDays/num;
			TimeSpan span = new TimeSpan(interval,0,0,0,0);
			DateTime end = start + span;

			for(int ii = 0; ii < num; ++ii)
			{
				createPlanData(plan, start, end);

				start = end + oneDay;
				end = start + span;
			}
		}

		private void createDataButton_Click(object sender, System.EventArgs e)
		{
			if (plan == null)
				return;

			// set current plan in database
			theDb.MarketPlanID = plan.id;

			if (dataTiming.Interval)
			{
				createPlanData(plan, dataTiming.Type);
			}
			else
			{
				int numRows = dataTiming.NumRows;

				createPlanData(plan, numRows);
			}
		}

	}
}
