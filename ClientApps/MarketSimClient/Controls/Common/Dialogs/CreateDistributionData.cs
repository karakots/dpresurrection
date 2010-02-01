using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;



using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreateDistributionData.

	/// </summary>

	public class CreateDistributionData : System.Windows.Forms.Form

	{

		private Common.Utilities.PlanData planData1;

		private System.Windows.Forms.NumericUpDown attributeGBox;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.NumericUpDown attributeFBox;

		private System.Windows.Forms.Label label4;

		private System.Windows.Forms.NumericUpDown persuasionBox;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.NumericUpDown awarenessBox;

		private System.Windows.Forms.Label label2;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreateDistributionData(ModelDb db, MrktSimDBSchema.market_planRow plan)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = db;



			planData1.Db = db;

			planData1.Plan = plan;

			planData1.Type = ModelDb.PlanType.Distribution;

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

			this.attributeGBox = new System.Windows.Forms.NumericUpDown();

			this.label1 = new System.Windows.Forms.Label();

			this.attributeFBox = new System.Windows.Forms.NumericUpDown();

			this.label4 = new System.Windows.Forms.Label();

			this.persuasionBox = new System.Windows.Forms.NumericUpDown();

			this.label3 = new System.Windows.Forms.Label();

			this.awarenessBox = new System.Windows.Forms.NumericUpDown();

			this.label2 = new System.Windows.Forms.Label();

			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();

			this.SuspendLayout();

			// 

			// planData1

			// 

			this.planData1.Dock = System.Windows.Forms.DockStyle.Right;

			this.planData1.Enabled = false;

			this.planData1.Location = new System.Drawing.Point(208, 0);

			this.planData1.Name = "planData1";

			this.planData1.Size = new System.Drawing.Size(328, 230);

			this.planData1.Suspend = false;

			this.planData1.TabIndex = 0;

			// 

			// attributeGBox

			// 

			this.attributeGBox.DecimalPlaces = 2;

			this.attributeGBox.Location = new System.Drawing.Point(120, 48);

			this.attributeGBox.Name = "attributeGBox";

			this.attributeGBox.Size = new System.Drawing.Size(72, 20);

			this.attributeGBox.TabIndex = 13;

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(32, 48);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(80, 23);

			this.label1.TabIndex = 12;

			this.label1.Text = "% Considered";

			// 

			// attributeFBox

			// 

			this.attributeFBox.DecimalPlaces = 2;

			this.attributeFBox.Location = new System.Drawing.Point(120, 16);

			this.attributeFBox.Name = "attributeFBox";

			this.attributeFBox.Size = new System.Drawing.Size(72, 20);

			this.attributeFBox.TabIndex = 11;

			// 

			// label4

			// 

			this.label4.Location = new System.Drawing.Point(40, 16);

			this.label4.Name = "label4";

			this.label4.Size = new System.Drawing.Size(72, 23);

			this.label4.TabIndex = 10;

			this.label4.Text = "% Available";

			// 

			// persuasionBox

			// 

			this.persuasionBox.DecimalPlaces = 2;

			this.persuasionBox.Increment = new System.Decimal(new int[] {

																			1,

																			0,

																			0,

																			65536});

			this.persuasionBox.Location = new System.Drawing.Point(120, 112);

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

			this.label3.Location = new System.Drawing.Point(48, 112);

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

			this.awarenessBox.Location = new System.Drawing.Point(120, 80);

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

			this.label2.Location = new System.Drawing.Point(24, 80);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(96, 23);

			this.label2.TabIndex = 6;

			this.label2.Text = "Awareness Prob.";

			// 

			// CreateDistributionData

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(536, 230);

			this.Controls.Add(this.planData1);

			this.Controls.Add(this.attributeFBox);

			this.Controls.Add(this.label4);

			this.Controls.Add(this.attributeGBox);

			this.Controls.Add(this.awarenessBox);

			this.Controls.Add(this.persuasionBox);

			this.Controls.Add(this.label1);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.label3);

			this.Name = "CreateDistributionData";

			this.Text = "CreateDistributionData";

			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion





		private ModelDb theDb;



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

	}

}

