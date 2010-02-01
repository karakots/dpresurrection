using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Utilities;
using MrktSimDb;

namespace Common
{
	/// <summary>
	/// Summary description for Events.
	/// </summary>
	public class EventsControl : MrktSimControl
	{
		public override void Reset()
		{
			base.Reset ();

			if (useTasks)
			{
				mrktPlanComponent.Type = PlanType.TaskEvent;
			}
			else
			{
				mrktPlanComponent.Type = PlanType.ProdEvent;
			}

			if (useTasks)
			{
				eventsGrid.Table = theDb.Data.task_event;
			}
			else
			{
				eventsGrid.Table = theDb.Data.product_event;
			}

			this.createTableStyle();
		}
		public override void Refresh()
		{
			base.Refresh ();

			eventsGrid.Refresh();
		}


		public override void Flush()
		{
			eventsGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;

				eventsGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}

		private bool useTasks
		{
			get
			{
				return theDb.Model.task_based;
			}
		}

		private System.Windows.Forms.GroupBox planBox;
		private Common.Utilities.MrktSimGrid eventsGrid;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private Common.Utilities.PlanData planData;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown modBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EventsControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		

			mrktPlanComponent.Db = db;
			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			planData.Db = db;

			if (useTasks)
			{
				mrktPlanComponent.Type = PlanType.TaskEvent;
				planData.Type = PlanType.TaskEvent;
			}
			else
			{
				mrktPlanComponent.Type = PlanType.ProdEvent;
				planData.Type = PlanType.ProdEvent;
			}

			planData.NewPlanRow += new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);
			

			this.SuspendLayout();

			if (useTasks)
			{
				eventsGrid.Table = theDb.Data.task_event;
			}
			else
			{
				eventsGrid.Table = theDb.Data.product_event;
			}

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
			this.eventsGrid = new Common.Utilities.MrktSimGrid();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.planBox = new System.Windows.Forms.GroupBox();
			this.planData = new Common.Utilities.PlanData();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.modBox = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.planBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.modBox)).BeginInit();
			this.SuspendLayout();
			// 
			// eventsGrid
			// 
			this.eventsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventsGrid.Location = new System.Drawing.Point(0, 280);
			this.eventsGrid.Name = "eventsGrid";
			this.eventsGrid.RowFilter = "";
			this.eventsGrid.RowID = null;
			this.eventsGrid.RowName = null;
			this.eventsGrid.Size = new System.Drawing.Size(816, 200);
			this.eventsGrid.Sort = "";
			this.eventsGrid.TabIndex = 1;
			// 
			// mrktPlanComponent
			// 
			this.mrktPlanComponent.Dock = System.Windows.Forms.DockStyle.Left;
			this.mrktPlanComponent.Location = new System.Drawing.Point(3, 16);
			this.mrktPlanComponent.Name = "mrktPlanComponent";
			this.mrktPlanComponent.Size = new System.Drawing.Size(317, 261);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 2;
			// 
			// planBox
			// 
			this.planBox.Controls.Add(this.planData);
			this.planBox.Controls.Add(this.groupBox1);
			this.planBox.Controls.Add(this.mrktPlanComponent);
			this.planBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.planBox.Location = new System.Drawing.Point(0, 0);
			this.planBox.Name = "planBox";
			this.planBox.Size = new System.Drawing.Size(816, 280);
			this.planBox.TabIndex = 3;
			this.planBox.TabStop = false;
			// 
			// planData
			// 
			this.planData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planData.Location = new System.Drawing.Point(565, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(248, 261);
			this.planData.Suspend = false;
			this.planData.TabIndex = 3;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.modBox);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox1.Location = new System.Drawing.Point(320, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(245, 261);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Exterl Factor Data";
			// 
			// modBox
			// 
			this.modBox.DecimalPlaces = 2;
			this.modBox.Location = new System.Drawing.Point(144, 32);
			this.modBox.Minimum = new System.Decimal(new int[] {
																   100,
																   0,
																   0,
																   -2147483648});
			this.modBox.Name = "modBox";
			this.modBox.Size = new System.Drawing.Size(72, 20);
			this.modBox.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 0;
			this.label5.Text = "Demand Modifcation";
			// 
			// EventsControl
			// 
			this.Controls.Add(this.eventsGrid);
			this.Controls.Add(this.planBox);
			this.Name = "EventsControl";
			this.Size = new System.Drawing.Size(816, 480);
			this.planBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.modBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			eventsGrid.Clear();

			if (useTasks)
			{
				eventsGrid.AddTextColumn("task_name", true);
				eventsGrid.AddTextColumn("segment_name", true);
			}
			else
			{
				eventsGrid.AddTextColumn("product_name", true);
				eventsGrid.AddTextColumn("segment_name", true);
				eventsGrid.AddTextColumn("channel_name", true);
			}

			eventsGrid.AddComboBoxColumn("type", theDb.Data.product_event_type, "type", "type_id");
			eventsGrid.AddNumericColumn("demand_modification");
			eventsGrid.AddDateColumn("start_date");
			eventsGrid.AddDateColumn("end_date");

			
			eventsGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				eventsGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				eventsGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				eventsGrid.RowFilter = "";
			}

			planData.Plan = plan;
		}

		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
		{
			if (useTasks)
			{
				MrktSimDBSchema.task_eventRow taskEvent = theDb.CreateTaskEvent(productID, segmentID);
				taskEvent.start_date = start;
				taskEvent.end_date = end;
				taskEvent.demand_modification = (double) modBox.Value;
			}
			else
			{
				MrktSimDBSchema.product_eventRow prodEvent = theDb.CreateProductEvent(productID, channelID, segmentID);
				prodEvent.start_date = start;
				prodEvent.end_date = end;
				prodEvent.demand_modification = (double) modBox.Value;
			}
		}
	}
}
