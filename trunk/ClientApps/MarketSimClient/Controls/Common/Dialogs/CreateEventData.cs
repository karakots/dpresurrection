using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CreateEventData.
	/// </summary>
	public class CreateEventData : System.Windows.Forms.Form
	{
		//private Common.Utilities.PlanData planData1;
		private System.Windows.Forms.NumericUpDown modBox;
		private System.Windows.Forms.Label label5;
		private Common.Utilities.StartEndDate startEndDate1;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button OKButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateEventData(ModelDb db, MrktSimDBSchema.market_planRow plan)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			theDb = db;

			/*planData1.Db = db;
			planData1.Plan = plan;
			planData1.Type = PlanType.ProdEvent;
			planData1.NewPlanRow +=new Common.Utilities.PlanData.CreatePlanData(planData_NewPlanRow);*/

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
			this.modBox = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.startEndDate1 = new Common.Utilities.StartEndDate();
			this.cancelButton = new System.Windows.Forms.Button();
			this.OKButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.modBox)).BeginInit();
			this.SuspendLayout();
			// 
			// modBox
			// 
			this.modBox.DecimalPlaces = 2;
			this.modBox.Location = new System.Drawing.Point(136, 24);
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
			this.label5.Location = new System.Drawing.Point(16, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 23);
			this.label5.TabIndex = 0;
			this.label5.Text = "Demand Modifcation";
			// 
			// startEndDate1
			// 
			this.startEndDate1.End = new System.DateTime(2006, 5, 11, 23, 26, 49, 593);
			this.startEndDate1.Location = new System.Drawing.Point(16, 56);
			this.startEndDate1.Name = "startEndDate1";
			this.startEndDate1.Size = new System.Drawing.Size(192, 48);
			this.startEndDate1.Start = new System.DateTime(2006, 5, 11, 23, 26, 49, 593);
			this.startEndDate1.TabIndex = 2;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(136, 120);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(72, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(40, 120);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "OK";
			// 
			// CreateEventData
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(224, 158);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.startEndDate1);
			this.Controls.Add(this.modBox);
			this.Controls.Add(this.label5);
			this.Name = "CreateEventData";
			this.Text = "CreateEventData";
			((System.ComponentModel.ISupportInitialize)(this.modBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private ModelDb theDb;

        //private void planData_NewPlanRow(int productID, int channelID, int segmentID, DateTime start, DateTime end)
        //{
        //    MrktSimDBSchema.product_eventRow prodEvent = theDb.CreateProductEvent(productID, channelID, segmentID);
        //    prodEvent.start_date = start;
        //    prodEvent.end_date = end;
        //    prodEvent.demand_modification = (double) modBox.Value;
        //}
	}
}
