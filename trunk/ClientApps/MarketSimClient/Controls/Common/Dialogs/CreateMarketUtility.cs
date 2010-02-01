using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;



using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreateMarketUtility.

	/// </summary>

	public class CreateMarketUtility : System.Windows.Forms.Form

	{

		private System.Windows.Forms.NumericUpDown utilityBox;

		private System.Windows.Forms.Label label4;

		private System.Windows.Forms.NumericUpDown persuasionBox;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.NumericUpDown awarenessBox;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.NumericUpDown percentBox;

		private System.Windows.Forms.Label label1;

		private Common.Utilities.PlanData planData1;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreateMarketUtility(ModelDb db, MrktSimDBSchema.market_planRow plan)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = db;



			planData1.Db = db;

			planData1.Plan = plan;

			planData1.Type = ModelDb.PlanType.Market_Utility;

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

			this.utilityBox = new System.Windows.Forms.NumericUpDown();

			this.label4 = new System.Windows.Forms.Label();

			this.persuasionBox = new System.Windows.Forms.NumericUpDown();

			this.label3 = new System.Windows.Forms.Label();

			this.awarenessBox = new System.Windows.Forms.NumericUpDown();

			this.label2 = new System.Windows.Forms.Label();

			this.percentBox = new System.Windows.Forms.NumericUpDown();

			this.label1 = new System.Windows.Forms.Label();

			this.planData1 = new Common.Utilities.PlanData();

			((System.ComponentModel.ISupportInitialize)(this.utilityBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.percentBox)).BeginInit();

			this.SuspendLayout();

			// 

			// utilityBox

			// 

			this.utilityBox.DecimalPlaces = 2;

			this.utilityBox.Location = new System.Drawing.Point(120, 112);

			this.utilityBox.Name = "utilityBox";

			this.utilityBox.Size = new System.Drawing.Size(72, 20);

			this.utilityBox.TabIndex = 15;

			// 

			// label4

			// 

			this.label4.Location = new System.Drawing.Point(72, 112);

			this.label4.Name = "label4";

			this.label4.Size = new System.Drawing.Size(40, 16);

			this.label4.TabIndex = 14;

			this.label4.Text = "Utility";

			// 

			// persuasionBox

			// 

			this.persuasionBox.DecimalPlaces = 2;

			this.persuasionBox.Increment = new System.Decimal(new int[] {

																			1,

																			0,

																			0,

																			65536});

			this.persuasionBox.Location = new System.Drawing.Point(120, 80);

			this.persuasionBox.Minimum = new System.Decimal(new int[] {

																		  100,

																		  0,

																		  0,

																		  -2147483648});

			this.persuasionBox.Name = "persuasionBox";

			this.persuasionBox.Size = new System.Drawing.Size(72, 20);

			this.persuasionBox.TabIndex = 13;

			// 

			// label3

			// 

			this.label3.Location = new System.Drawing.Point(48, 80);

			this.label3.Name = "label3";

			this.label3.Size = new System.Drawing.Size(64, 23);

			this.label3.TabIndex = 12;

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

			this.awarenessBox.Location = new System.Drawing.Point(120, 48);

			this.awarenessBox.Maximum = new System.Decimal(new int[] {

																		 1,

																		 0,

																		 0,

																		 0});

			this.awarenessBox.Name = "awarenessBox";

			this.awarenessBox.Size = new System.Drawing.Size(72, 20);

			this.awarenessBox.TabIndex = 11;

			// 

			// label2

			// 

			this.label2.Location = new System.Drawing.Point(24, 48);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(96, 23);

			this.label2.TabIndex = 10;

			this.label2.Text = "Awareness Prob.";

			// 

			// percentBox

			// 

			this.percentBox.DecimalPlaces = 2;

			this.percentBox.Location = new System.Drawing.Point(120, 16);

			this.percentBox.Name = "percentBox";

			this.percentBox.Size = new System.Drawing.Size(72, 20);

			this.percentBox.TabIndex = 9;

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(48, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(72, 23);

			this.label1.TabIndex = 8;

			this.label1.Text = "% Available";

			// 

			// planData1

			// 

			this.planData1.Dock = System.Windows.Forms.DockStyle.Right;

			this.planData1.Enabled = false;

			this.planData1.Location = new System.Drawing.Point(240, 0);

			this.planData1.Name = "planData1";

			this.planData1.Size = new System.Drawing.Size(328, 246);

			this.planData1.Suspend = false;

			this.planData1.TabIndex = 16;

			// 

			// CreateMarketUtility

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(568, 246);

			this.Controls.Add(this.planData1);

			this.Controls.Add(this.utilityBox);

			this.Controls.Add(this.label4);

			this.Controls.Add(this.persuasionBox);

			this.Controls.Add(this.label3);

			this.Controls.Add(this.awarenessBox);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.percentBox);

			this.Controls.Add(this.label1);

			this.Name = "CreateMarketUtility";

			this.Text = "CreateMarketUtility";

			((System.ComponentModel.ISupportInitialize)(this.utilityBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.percentBox)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion



		private ModelDb theDb;



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

	}

}

