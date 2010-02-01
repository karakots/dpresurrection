using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;



using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreatePlanData.

	/// </summary>

	public class CreateDisplayData : System.Windows.Forms.Form

	{

		private Common.Utilities.PlanData planData1;

		private System.Windows.Forms.GroupBox displayDataBox;

		private System.Windows.Forms.NumericUpDown persuasionBox;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.NumericUpDown awarenessBox;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.NumericUpDown attributeFBox;

		private System.Windows.Forms.Label label1;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreateDisplayData(ModelDb db, MrktSimDBSchema.market_planRow plan)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = db;



			planData1.Db = db;

			planData1.Plan = plan;

			planData1.Type = ModelDb.PlanType.Display;

			planData1.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);



			// initialize values



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



		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{

			this.planData1 = new Common.Utilities.PlanData();

			this.displayDataBox = new System.Windows.Forms.GroupBox();

			this.persuasionBox = new System.Windows.Forms.NumericUpDown();

			this.label3 = new System.Windows.Forms.Label();

			this.awarenessBox = new System.Windows.Forms.NumericUpDown();

			this.label2 = new System.Windows.Forms.Label();

			this.attributeFBox = new System.Windows.Forms.NumericUpDown();

			this.label1 = new System.Windows.Forms.Label();

			this.displayDataBox.SuspendLayout();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).BeginInit();

			this.SuspendLayout();

			// 

			// planData1

			// 

			this.planData1.Dock = System.Windows.Forms.DockStyle.Right;

			this.planData1.Enabled = false;

			this.planData1.Location = new System.Drawing.Point(200, 0);

			this.planData1.Name = "planData1";

			this.planData1.Size = new System.Drawing.Size(320, 222);

			this.planData1.Suspend = false;

			this.planData1.TabIndex = 0;

			// 

			// displayDataBox

			// 

			this.displayDataBox.Controls.Add(this.persuasionBox);

			this.displayDataBox.Controls.Add(this.label3);

			this.displayDataBox.Controls.Add(this.awarenessBox);

			this.displayDataBox.Controls.Add(this.label2);

			this.displayDataBox.Controls.Add(this.attributeFBox);

			this.displayDataBox.Controls.Add(this.label1);

			this.displayDataBox.Dock = System.Windows.Forms.DockStyle.Left;

			this.displayDataBox.Location = new System.Drawing.Point(0, 0);

			this.displayDataBox.Name = "displayDataBox";

			this.displayDataBox.Size = new System.Drawing.Size(200, 222);

			this.displayDataBox.TabIndex = 4;

			this.displayDataBox.TabStop = false;

			this.displayDataBox.Text = "Display Data";

			// 

			// persuasionBox

			// 

			this.persuasionBox.DecimalPlaces = 2;

			this.persuasionBox.Increment = new System.Decimal(new int[] {

																			1,

																			0,

																			0,

																			65536});

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

			// CreateDisplayData

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(520, 222);

			this.Controls.Add(this.displayDataBox);

			this.Controls.Add(this.planData1);

			this.Name = "CreateDisplayData";

			this.Text = "CreatePlanData";

			this.displayDataBox.ResumeLayout(false);

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion



		private ModelDb theDb;



		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)

		{

			MrktSimDBSchema.displayRow display = theDb.CreateDisplay(productID, channelID);

			display.start_date = start;

			display.end_date = end;



			display.attr_value_F = (double) attributeFBox.Value;

			display.message_awareness_probability = (double) awarenessBox.Value;

			display.message_persuation_probability = (double) persuasionBox.Value;

		}

	}

}

