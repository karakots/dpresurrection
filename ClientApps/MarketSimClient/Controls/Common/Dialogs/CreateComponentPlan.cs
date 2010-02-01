using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CreatePricePlan.
	/// </summary>
	public class CreateComponentPlan : System.Windows.Forms.Form
	{
		MrktSimDBSchema.market_planRow currentPlan = null;

		public int ProductID
		{
			set
			{
				iProductBox.ProductID = value;
			}

			get
			{
				return iProductBox.ProductID;
			}

		}

		public int TaskID
		{
			set
			{
				iTaskBox.TaskID = value;
			}

			get
			{
				return iTaskBox.TaskID;
			}

		}

		public bool leafOnly
		{
			set
			{
				iProductBox.leafOnly = value;
			}
		}
		/// <summary>
		/// sets the default name for the plan
		/// </summary>
		public string PlanName
		{
			set
			{
				nameBox.Text = value;
			}
		}

		public MrktSimDBSchema.market_planRow CurrentPlan
		{
			set
			{
				currentPlan = value;
				this.Text = "Editing " + currentPlan.name;

				if (currentPlan.type != (byte) ModelDb.PlanType.TaskEvent)
					iProductBox.ProductID = currentPlan.product_id;
				else
					iTaskBox.TaskID = currentPlan.task_id;


				// create a new plan
				iStartEndDate.Start = currentPlan.start_date;
				iStartEndDate.End = currentPlan.end_date;

				nameBox.Text = currentPlan.name;
				descrBox.Text = currentPlan.descr;
			}

			get
			{
				return currentPlan;
			}
		}

		ModelDb.PlanType planType;
		public ModelDb.PlanType Type
		{
			set
			{
				planType = value;

				switch(planType)
				{
					case ModelDb.PlanType.MarketPlan:
						this.Text = "Create Marketing Plan";
						
						break;
					case ModelDb.PlanType.Price:
						this.Text = "Create Product Pricing Plan";
						
						break;

					case ModelDb.PlanType.Distribution:
						this.Text = "Create Distribution Plan";
					
						break;

					case ModelDb.PlanType.Display:
						this.Text = "Create Display Plan";
						
						break;

					case ModelDb.PlanType.Media:
						this.Text = "Create Media Plan";
						
						break;

					case ModelDb.PlanType.ProdEvent:
						
						//iProductBox.AllowAll = true;
						this.Text = "Create External Factors";
						// hide tasks - unhide products
						iProductBox.Visible = true;
						

						iTaskBox.Visible = false;
						
						break;

					case ModelDb.PlanType.TaskEvent:

						this.Text = "Create Task Based Adjustments";
						
						// hide products - unhide tasks
						iProductBox.Visible = false;
						
						iTaskBox.Visible = true;
						break;
				}
			}
		}

		ModelDb theDb;


		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private Common.Utilities.ProductPicker iProductBox;
		private Common.Utilities.StartEndDate iStartEndDate;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox descrBox;
		private Common.Utilities.TaskPicker iTaskBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateComponentPlan(ModelDb db)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			theDb = db;

			iProductBox.Db = db;
			
			iProductBox.leafOnly = true;
			
			iStartEndDate.Db = db;

			//default
			iTaskBox.Visible = false;
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
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.descrBox = new System.Windows.Forms.TextBox();
			this.iProductBox = new Common.Utilities.ProductPicker();
			this.iStartEndDate = new Common.Utilities.StartEndDate();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.iTaskBox = new Common.Utilities.TaskPicker();
			this.SuspendLayout();
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.Location = new System.Drawing.Point(360, 152);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(480, 152);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			// 
			// descrBox
			// 
			this.descrBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.descrBox.Location = new System.Drawing.Point(336, 48);
			this.descrBox.Multiline = true;
			this.descrBox.Name = "descrBox";
			this.descrBox.Size = new System.Drawing.Size(248, 88);
			this.descrBox.TabIndex = 28;
			this.descrBox.Text = "";
			// 
			// iProductBox
			// 
			this.iProductBox.Location = new System.Drawing.Point(16, 16);
			this.iProductBox.Name = "iProductBox";
			this.iProductBox.ProductID = -1;
			this.iProductBox.Size = new System.Drawing.Size(216, 56);
			this.iProductBox.TabIndex = 26;
			// 
			// iStartEndDate
			// 
			this.iStartEndDate.End = new System.DateTime(2005, 2, 15, 13, 1, 42, 343);
			this.iStartEndDate.Location = new System.Drawing.Point(64, 88);
			this.iStartEndDate.Name = "iStartEndDate";
			this.iStartEndDate.Size = new System.Drawing.Size(168, 56);
			this.iStartEndDate.Start = new System.DateTime(2005, 2, 15, 13, 1, 42, 343);
			this.iStartEndDate.TabIndex = 25;
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nameBox.Location = new System.Drawing.Point(336, 16);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(248, 20);
			this.nameBox.TabIndex = 24;
			this.nameBox.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(256, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 30;
			this.label3.Text = "Description";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(256, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 29;
			this.label1.Text = "Name";
			// 
			// iTaskBox
			// 
			this.iTaskBox.Location = new System.Drawing.Point(16, 48);
			this.iTaskBox.Name = "iTaskBox";
			this.iTaskBox.Size = new System.Drawing.Size(216, 24);
			this.iTaskBox.Suspend = false;
			this.iTaskBox.TabIndex = 32;
			this.iTaskBox.TaskID = -1;
			// 
			// CreateComponentPlan
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(602, 184);
			this.ControlBox = false;
			this.Controls.Add(this.iTaskBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.descrBox);
			this.Controls.Add(this.iProductBox);
			this.Controls.Add(this.iStartEndDate);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CreateComponentPlan";
			this.Text = "Create Plan";
			this.ResumeLayout(false);

		}
		#endregion

		private void acceptButton_Click(object sender, System.EventArgs e)
		{

			bool update = false;
			int days = 0;

			if (currentPlan == null)
			{
				currentPlan = theDb.CreateMarketPlan(nameBox.Text, planType);
			}
			else 
			{
				if ( currentPlan.name != nameBox.Text )
					currentPlan.name = Database.CreateUniqueName(theDb.Data.market_plan, "name", nameBox.Text, "");

				if (currentPlan.product_id != iProductBox.ProductID)
					update = true;

				if (currentPlan.start_date != iStartEndDate.Start)
				{
					// compute difference
					TimeSpan span = iStartEndDate.Start - currentPlan.start_date;

					// make a nice string
					string timeSpan = null;

					if (span.Days > 0)
						timeSpan = " forward by ";
					else
						timeSpan = " backward by ";

					int numYears = iStartEndDate.Start.Year - currentPlan.start_date.Year;
					int numDays = span.Days;
					
					if (numYears != 0)
					{
						DateTime temp = currentPlan.start_date.AddYears(numYears);

						numDays = (iStartEndDate.Start -temp).Days;

						timeSpan += Math.Abs(numYears);

						if (Math.Abs(numYears) != 1)
							timeSpan += " Years ";
						else
							timeSpan += " Year ";
					}

					if (numDays != 0)
					{
						timeSpan += Math.Abs(numDays);

						if (Math.Abs(numDays) != 1)
							timeSpan += " Days ";
						else
							timeSpan += " Day ";
					}

					DialogResult rslt = MessageBox.Show("Move Entries for this plan " + timeSpan + "?", "Change Dates?", MessageBoxButtons.YesNo);

					if (rslt == DialogResult.Yes)
					{
						days = span.Days;
						update = true;

						// adjust end date
						iStartEndDate.End = iStartEndDate.End.AddDays(days);
					}
					else
					{
						// set start and end back to what they were
						iStartEndDate.Start = currentPlan.start_date;
						iStartEndDate.End = currentPlan.end_date;
					}

				}
			}
		
			currentPlan.product_id = iProductBox.ProductID;
			currentPlan.task_id = iTaskBox.TaskID;

			currentPlan.start_date = iStartEndDate.Start;
			currentPlan.end_date = iStartEndDate.End;

			currentPlan.descr = descrBox.Text;
			currentPlan.type = (byte) planType;

			if (update)
			{
				if ((ModelDb.PlanType) currentPlan.type == ModelDb.PlanType.MarketPlan)
				{
					DialogResult rslt = MessageBox.Show("This will make a copy of each component of this Market Plan, and the entries in the copied components will be updated", "Copy market Plan Components?", MessageBoxButtons.OKCancel);

					if (rslt != DialogResult.OK)
						update = false;
				}

				if (update)
				{
					theDb.UpdateMarketPlan(currentPlan, days);
				}
			}

			this.DialogResult = DialogResult.OK;
		}
	}
}
