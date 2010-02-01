using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for CreatePriceData.

	/// </summary>

	public class CreatePriceData : System.Windows.Forms.Form

	{

		private Common.Utilities.PlanData planData1;

		private System.Windows.Forms.Label label6;

		private System.Windows.Forms.NumericUpDown percentDistBox;

		private System.Windows.Forms.Label promoLabel;

		private System.Windows.Forms.ComboBox promoBox;

		private System.Windows.Forms.Label howOftenLabel;

		private System.Windows.Forms.ComboBox howOftenBox;

		private System.Windows.Forms.Label periodicPriceLabel;

		private System.Windows.Forms.NumericUpDown periodicPriceBox;

		private System.Windows.Forms.Label markupLabel;

		private System.Windows.Forms.NumericUpDown markupBox;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.NumericUpDown priceBox;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CreatePriceData(ModelDb db, MrktSimDBSchema.market_planRow plan)

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			theDb = db;



			planData1.Db = db;

			planData1.Plan = plan;

			planData1.Type = ModelDb.PlanType.Price;

			planData1.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);



			percentDistBox.Value = 100;



			promoBox.SelectedIndex = 0;

			if (!theDb.Model.promoted_price)

			{

				this.promoLabel.Visible = false;

				promoBox.Visible = false;

			}

			

			if (!theDb.Model.profit_loss)

			{

				markupBox.Visible = false;

				this.markupLabel.Visible = false;

			}



			howOftenBox.SelectedIndex = 2;

			if (!theDb.Model.periodic_price)

			{

				periodicPriceBox.Visible = false;

				howOftenBox.Visible = false;



				this.periodicPriceLabel.Visible = false;

				this.howOftenLabel.Visible = false;

			}

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

			this.percentDistBox = new System.Windows.Forms.NumericUpDown();

			this.promoLabel = new System.Windows.Forms.Label();

			this.promoBox = new System.Windows.Forms.ComboBox();

			this.howOftenLabel = new System.Windows.Forms.Label();

			this.howOftenBox = new System.Windows.Forms.ComboBox();

			this.periodicPriceLabel = new System.Windows.Forms.Label();

			this.periodicPriceBox = new System.Windows.Forms.NumericUpDown();

			this.markupLabel = new System.Windows.Forms.Label();

			this.markupBox = new System.Windows.Forms.NumericUpDown();

			this.label1 = new System.Windows.Forms.Label();

			this.priceBox = new System.Windows.Forms.NumericUpDown();

			((System.ComponentModel.ISupportInitialize)(this.percentDistBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.periodicPriceBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.markupBox)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.priceBox)).BeginInit();

			this.SuspendLayout();

			// 

			// planData1

			// 

			this.planData1.Dock = System.Windows.Forms.DockStyle.Right;

			this.planData1.Enabled = false;

			this.planData1.Location = new System.Drawing.Point(258, 0);

			this.planData1.Name = "planData1";

			this.planData1.Size = new System.Drawing.Size(312, 224);

			this.planData1.Suspend = false;

			this.planData1.TabIndex = 0;

			// 

			// label6

			// 

			this.label6.Location = new System.Drawing.Point(40, 48);

			this.label6.Name = "label6";

			this.label6.Size = new System.Drawing.Size(48, 16);

			this.label6.TabIndex = 11;

			this.label6.Text = "% Dist.";

			// 

			// percentDistBox

			// 

			this.percentDistBox.DecimalPlaces = 2;

			this.percentDistBox.Location = new System.Drawing.Point(112, 48);

			this.percentDistBox.Maximum = new System.Decimal(new int[] {

																		   1000,

																		   0,

																		   0,

																		   0});

			this.percentDistBox.Name = "percentDistBox";

			this.percentDistBox.TabIndex = 10;

			this.percentDistBox.ThousandsSeparator = true;

			// 

			// promoLabel

			// 

			this.promoLabel.Location = new System.Drawing.Point(32, 80);

			this.promoLabel.Name = "promoLabel";

			this.promoLabel.Size = new System.Drawing.Size(56, 16);

			this.promoLabel.TabIndex = 9;

			this.promoLabel.Text = "Promotion";

			// 

			// promoBox

			// 

			this.promoBox.Items.AddRange(new object[] {

														  "Unpromoted",

														  "Promoted",

														  "BOGO"});

			this.promoBox.Location = new System.Drawing.Point(112, 80);

			this.promoBox.Name = "promoBox";

			this.promoBox.Size = new System.Drawing.Size(120, 21);

			this.promoBox.TabIndex = 8;

			this.promoBox.Text = "<Select>";

			// 

			// howOftenLabel

			// 

			this.howOftenLabel.Location = new System.Drawing.Point(32, 176);

			this.howOftenLabel.Name = "howOftenLabel";

			this.howOftenLabel.Size = new System.Drawing.Size(56, 16);

			this.howOftenLabel.TabIndex = 7;

			this.howOftenLabel.Text = "How often";

			// 

			// howOftenBox

			// 

			this.howOftenBox.Items.AddRange(new object[] {

															 "Day",

															 "Week",

															 "Month",

															 "Year"});

			this.howOftenBox.Location = new System.Drawing.Point(112, 176);

			this.howOftenBox.Name = "howOftenBox";

			this.howOftenBox.Size = new System.Drawing.Size(121, 21);

			this.howOftenBox.TabIndex = 6;

			this.howOftenBox.Text = "<Select>";

			// 

			// periodicPriceLabel

			// 

			this.periodicPriceLabel.Location = new System.Drawing.Point(8, 144);

			this.periodicPriceLabel.Name = "periodicPriceLabel";

			this.periodicPriceLabel.Size = new System.Drawing.Size(80, 16);

			this.periodicPriceLabel.TabIndex = 5;

			this.periodicPriceLabel.Text = "Periodic Price";

			// 

			// periodicPriceBox

			// 

			this.periodicPriceBox.DecimalPlaces = 2;

			this.periodicPriceBox.Location = new System.Drawing.Point(112, 144);

			this.periodicPriceBox.Maximum = new System.Decimal(new int[] {

																			 1000,

																			 0,

																			 0,

																			 0});

			this.periodicPriceBox.Name = "periodicPriceBox";

			this.periodicPriceBox.TabIndex = 4;

			this.periodicPriceBox.ThousandsSeparator = true;

			// 

			// markupLabel

			// 

			this.markupLabel.Location = new System.Drawing.Point(40, 112);

			this.markupLabel.Name = "markupLabel";

			this.markupLabel.Size = new System.Drawing.Size(48, 16);

			this.markupLabel.TabIndex = 3;

			this.markupLabel.Text = "Markup";

			// 

			// markupBox

			// 

			this.markupBox.DecimalPlaces = 2;

			this.markupBox.Location = new System.Drawing.Point(112, 112);

			this.markupBox.Maximum = new System.Decimal(new int[] {

																	  1000,

																	  0,

																	  0,

																	  0});

			this.markupBox.Name = "markupBox";

			this.markupBox.TabIndex = 2;

			this.markupBox.ThousandsSeparator = true;

			// 

			// label1

			// 

			this.label1.Location = new System.Drawing.Point(48, 16);

			this.label1.Name = "label1";

			this.label1.Size = new System.Drawing.Size(40, 16);

			this.label1.TabIndex = 1;

			this.label1.Text = "Price";

			// 

			// priceBox

			// 

			this.priceBox.DecimalPlaces = 2;

			this.priceBox.Location = new System.Drawing.Point(112, 16);

			this.priceBox.Maximum = new System.Decimal(new int[] {

																	 1000,

																	 0,

																	 0,

																	 0});

			this.priceBox.Name = "priceBox";

			this.priceBox.TabIndex = 0;

			this.priceBox.ThousandsSeparator = true;

			// 

			// CreatePriceData

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(570, 224);

			this.Controls.Add(this.planData1);

			this.Controls.Add(this.label6);

			this.Controls.Add(this.percentDistBox);

			this.Controls.Add(this.promoLabel);

			this.Controls.Add(this.promoBox);

			this.Controls.Add(this.howOftenLabel);

			this.Controls.Add(this.howOftenBox);

			this.Controls.Add(this.periodicPriceLabel);

			this.Controls.Add(this.periodicPriceBox);

			this.Controls.Add(this.markupLabel);

			this.Controls.Add(this.markupBox);

			this.Controls.Add(this.label1);

			this.Controls.Add(this.priceBox);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

			this.Name = "CreatePriceData";

			this.Text = "CreatePriceData";

			((System.ComponentModel.ISupportInitialize)(this.percentDistBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.periodicPriceBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.markupBox)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.priceBox)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion

	

		private ModelDb theDb;



		private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)

		{

			MrktSimDBSchema.product_channelRow price = theDb.CreateProductChannel(productID, channelID);

			price.start_date = start;

			price.end_date = end;



			price.price = (double) priceBox.Value;

			price.percent_SKU_in_dist = (double) percentDistBox.Value;



			if (theDb.Model.promoted_price)

			{

				// price.ptype = this.promoBox.SelectedItem.ToString();

			}



			if (theDb.Model.profit_loss)

				price.markup = (double) this.markupBox.Value;



			if (theDb.Model.periodic_price)

			{

				price.periodic_price = (double) this.periodicPriceBox.Value;



				price.how_often = howOftenBox.SelectedItem.ToString();

			}

		}

	}

}

