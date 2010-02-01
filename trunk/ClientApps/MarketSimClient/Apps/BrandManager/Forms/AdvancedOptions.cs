using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using MarketSimUtilities;
using Utilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for AddCompetitor.
	/// </summary>
	public class AdvancedOptions : System.Windows.Forms.UserControl, Wizard
	{
		private System.ComponentModel.IContainer components;

		public AdvancedOptions()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.access_timeCombo.SelectedIndex = 1;

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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numSpans = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.access_timeCombo = new System.Windows.Forms.ComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.scenarioStartEnd = new Common.Utilities.StartEndDate();
			((System.ComponentModel.ISupportInitialize)(this.numSpans)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(448, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "Scenario Timing";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(224, 128);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 61;
			this.label6.Text = "x";
			// 
			// numSpans
			// 
			this.numSpans.Location = new System.Drawing.Point(256, 128);
			this.numSpans.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.numSpans.Name = "numSpans";
			this.numSpans.Size = new System.Drawing.Size(48, 20);
			this.numSpans.TabIndex = 60;
			this.toolTip1.SetToolTip(this.numSpans, "Frequency for writing data");
			this.numSpans.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 59;
			this.label5.Text = "Write Data:";
			// 
			// access_timeCombo
			// 
			this.access_timeCombo.Items.AddRange(new object[] {
																  "Daily",
																  "Weekly",
																  "Monthly"});
			this.access_timeCombo.Location = new System.Drawing.Point(112, 128);
			this.access_timeCombo.Name = "access_timeCombo";
			this.access_timeCombo.Size = new System.Drawing.Size(96, 21);
			this.access_timeCombo.TabIndex = 58;
			this.access_timeCombo.Text = "comboBox1";
			this.toolTip1.SetToolTip(this.access_timeCombo, "Units for writing data");
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(344, 24);
			this.label3.TabIndex = 64;
			this.label3.Text = "Specifies how often results are aggragated";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 192);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(432, 32);
			this.label4.TabIndex = 65;
			this.label4.Text = "Change the scenario start and end date.  This sets the time period during with re" +
				"sults are recorded";
			// 
			// scenarioStartEnd
			// 
			this.scenarioStartEnd.End = new System.DateTime(2006, 8, 4, 9, 17, 19, 406);
			this.scenarioStartEnd.Location = new System.Drawing.Point(24, 224);
			this.scenarioStartEnd.Name = "scenarioStartEnd";
			this.scenarioStartEnd.Size = new System.Drawing.Size(160, 48);
			this.scenarioStartEnd.Start = new System.DateTime(2006, 8, 4, 9, 17, 19, 406);
			this.scenarioStartEnd.Suspend = false;
			this.scenarioStartEnd.TabIndex = 66;
			// 
			// AdvancedOptions
			// 
			this.Controls.Add(this.scenarioStartEnd);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.numSpans);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.access_timeCombo);
			this.Controls.Add(this.label1);
			this.Name = "AdvancedOptions";
			this.Size = new System.Drawing.Size(480, 384);
			((System.ComponentModel.ISupportInitialize)(this.numSpans)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods
		public Database Db
		{
			set
			{
				db = value;
			}

			get
			{
				return db;
			}
		}

		
		public MrktSimDb.MrktSimDBSchema.scenarioRow CurrentScenario
		{
			set
			{
				scenario = value;
				if (scenario.access_time % 7 == 0)
				{
					access_timeCombo.SelectedIndex = 1;
					this.numSpans.Value = (int) scenario.access_time / 7;
				}
				else if (scenario.access_time % 30 == 0)
				{
					access_timeCombo.SelectedIndex = 2;
					this.numSpans.Value = (int) scenario.access_time / 30;
				}
				else
				{
					access_timeCombo.SelectedIndex = 0;
					this.numSpans.Value = (int) scenario.access_time;
				}

				this.scenarioStartEnd.Start = scenario.start_date;
				this.scenarioStartEnd.End = scenario.end_date;
			}

			get
			{
				return scenario;
			}
		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add AddCompetitor.Next implementation
			return true;
		}

		public bool Back()
		{
			MessageBox.Show("Please continue with this scenario before creating a new one","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}

		public void Start()
		{
			
		}

		public void End()
		{
			switch(access_timeCombo.SelectedIndex)
			{
				case 0:
					scenario.access_time = 1;
					break;
				case 1:
					scenario.access_time = 7;
					break;
				case 2:
					scenario.access_time = 30;
					break;
				default:
					scenario.access_time = 1;
					break;
			}

			scenario.access_time *= (int) numSpans.Value;

			scenario.start_date = this.scenarioStartEnd.Start;
			scenario.end_date = this.scenarioStartEnd.End;

			//Db.Update();
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region private data and members
		private Database db;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numSpans;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox access_timeCombo;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private Common.Utilities.StartEndDate scenarioStartEnd;
		private MrktSimDb.MrktSimDBSchema.scenarioRow scenario = null;
		#endregion
	}
}
