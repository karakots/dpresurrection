using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreateMediaData.

	/// </summary>

	public class CreateMediaData : System.Windows.Forms.Form

	{

		private Common.Utilities.PlanData planData1;

		private System.Windows.Forms.Label label6;

		private System.Windows.Forms.ComboBox typeBox;

		private System.Windows.Forms.NumericUpDown attributeGBox;

		private System.Windows.Forms.Label label4;

		private System.Windows.Forms.NumericUpDown persuasionBox;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.NumericUpDown awarenessBox;

		private System.Windows.Forms.Label label2;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreateMediaData(ModelDb db, MrktSimDBSchema.market_planRow plan)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = db;



			planData1.Db = db;

			planData1.Plan = plan;

			planData1.Type = ModelDb.PlanType.Media;

			planData1.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);



			// initialize values

			typeBox.SelectedIndex = 0;





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

			this.label6 = new System.Windows.Forms.Label();

			this.typeBox = new System.Windows.Forms.ComboBox();

			this.attributeGBox = new System.Windows.Forms.NumericUpDown();

			this.label4 = new System.Windows.Forms.Label();

			this.persuasionBox = new System.Windows.Forms.NumericUpDown();

			this.label3 = new System.Windows.Forms.Label();

			this.awarenessBox = new System.Windows.Forms.NumericUpDown();

			this.label2 = new System.Windows.Forms.Label();

			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();

			this.SuspendLayout();

			// 

			// planData1

			// 

			this.planData1.Dock = System.Windows.Forms.DockStyle.Right;

			this.planData1.Enabled = false;

			this.planData1.Location = new System.Drawing.Point(224, 0);

			this.planData1.Name = "planData1";

			this.planData1.Size = new System.Drawing.Size(320, 230);

			this.planData1.Suspend = false;

			this.planData1.TabIndex = 0;

			// 

			// label6

			// 

			this.label6.Location = new System.Drawing.Point(88, 16);

			this.label6.Name = "label6";

			this.label6.Size = new System.Drawing.Size(40, 23);

			this.label6.TabIndex = 25;

			this.label6.Text = "Type";

			// 

			// typeBox

			// 

			this.typeBox.Items.AddRange(new object[] {

														 "Variable",

														 "Absolute"});

			this.typeBox.Location = new System.Drawing.Point(136, 16);

			this.typeBox.Name = "typeBox";

			this.typeBox.Size = new System.Drawing.Size(72, 21);

			this.typeBox.TabIndex = 24;

			this.typeBox.Text = "<Select>";

			// 

			// attributeGBox

			// 

			this.attributeGBox.DecimalPlaces = 2;

			this.attributeGBox.Location = new System.Drawing.Point(136, 48);

			this.attributeGBox.Name = "attributeGBox";

			this.attributeGBox.Size = new System.Drawing.Size(72, 20);

			this.attributeGBox.TabIndex = 19;

			// 

			// label4

			// 

			this.label4.Location = new System.Drawing.Point(88, 48);

			this.label4.Name = "label4";

			this.label4.Size = new System.Drawing.Size(32, 23);

			this.label4.TabIndex = 18;

			this.label4.Text = "GRP";

			// 

			// persuasionBox

			// 

			this.persuasionBox.DecimalPlaces = 2;

			this.persuasionBox.Increment = new System.Decimal(new int[] {

																			1,

																			0,

																			0,

																			65536});

			this.persuasionBox.Location = new System.Drawing.Point(136, 112);

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

			this.label3.Location = new System.Drawing.Point(40, 112);

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

			this.awarenessBox.Location = new System.Drawing.Point(136, 80);

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

			this.label2.Location = new System.Drawing.Point(40, 80);

			this.label2.Name = "label2";

			this.label2.Size = new System.Drawing.Size(96, 23);

			this.label2.TabIndex = 14;

			this.label2.Text = "Awareness Prob.";

			// 

			// CreateMediaData

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(544, 230);

			this.Controls.Add(this.planData1);

			this.Controls.Add(this.typeBox);

			this.Controls.Add(this.label6);

			this.Controls.Add(this.label2);

			this.Controls.Add(this.awarenessBox);

			this.Controls.Add(this.label3);

			this.Controls.Add(this.persuasionBox);

			this.Controls.Add(this.label4);

			this.Controls.Add(this.attributeGBox);

			this.Name = "CreateMediaData";

			this.Text = "CreateMediaData";

			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion



		private ModelDb theDb;



		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)

		{

			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(productID, channelID, segmentID);

			media.start_date = start;

			media.end_date = end;



			

			media.media_type = typeBox.SelectedItem.ToString()[0].ToString();

			media.attr_value_G = (double) attributeGBox.Value;

			media.message_awareness_probability = (double) awarenessBox.Value;

			media.message_persuation_probability = (double) persuasionBox.Value;

		}

	}

}

