using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

using MrktSimDb;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for SimStatusControl.
	/// </summary>
	public class SimStatusControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label timeRemainingLbl;
		private System.Windows.Forms.Label timelabel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label elapsedTime;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label currDate;
		private System.Windows.Forms.Label endDate;
		private System.Windows.Forms.Label startDate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label simName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label currStatus;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Timer simTime;
		private System.ComponentModel.IContainer components;

		public SimStatusControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.timeRemainingLbl = new System.Windows.Forms.Label();
			this.timelabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.elapsedTime = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.currDate = new System.Windows.Forms.Label();
			this.endDate = new System.Windows.Forms.Label();
			this.startDate = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.simName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.currStatus = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.simTime = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// timeRemainingLbl
			// 
			this.timeRemainingLbl.Location = new System.Drawing.Point(112, 160);
			this.timeRemainingLbl.Name = "timeRemainingLbl";
			this.timeRemainingLbl.Size = new System.Drawing.Size(112, 16);
			this.timeRemainingLbl.TabIndex = 34;
			// 
			// timelabel
			// 
			this.timelabel.Location = new System.Drawing.Point(16, 160);
			this.timelabel.Name = "timelabel";
			this.timelabel.Size = new System.Drawing.Size(88, 16);
			this.timelabel.TabIndex = 33;
			this.timelabel.Text = "Time Remaining";
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(16, 184);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(208, 23);
			this.progressBar.TabIndex = 32;
			// 
			// elapsedTime
			// 
			this.elapsedTime.Location = new System.Drawing.Point(88, 128);
			this.elapsedTime.Name = "elapsedTime";
			this.elapsedTime.Size = new System.Drawing.Size(248, 16);
			this.elapsedTime.TabIndex = 31;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 128);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 30;
			this.label7.Text = "Elapsed Time";
			// 
			// currDate
			// 
			this.currDate.Location = new System.Drawing.Point(88, 104);
			this.currDate.Name = "currDate";
			this.currDate.Size = new System.Drawing.Size(248, 16);
			this.currDate.TabIndex = 29;
			// 
			// endDate
			// 
			this.endDate.Location = new System.Drawing.Point(88, 56);
			this.endDate.Name = "endDate";
			this.endDate.Size = new System.Drawing.Size(248, 16);
			this.endDate.TabIndex = 27;
			// 
			// startDate
			// 
			this.startDate.Location = new System.Drawing.Point(88, 32);
			this.startDate.Name = "startDate";
			this.startDate.Size = new System.Drawing.Size(248, 16);
			this.startDate.TabIndex = 25;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 23;
			this.label6.Text = "End Date";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 22;
			this.label5.Text = "Start Date";
			// 
			// simName
			// 
			this.simName.Location = new System.Drawing.Point(88, 8);
			this.simName.Name = "simName";
			this.simName.Size = new System.Drawing.Size(248, 16);
			this.simName.TabIndex = 20;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 19;
			this.label3.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 18;
			this.label2.Text = "Current Date";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "Status";
			// 
			// currStatus
			// 
			this.currStatus.Location = new System.Drawing.Point(88, 80);
			this.currStatus.Name = "currStatus";
			this.currStatus.Size = new System.Drawing.Size(248, 16);
			this.currStatus.TabIndex = 15;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(112, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(248, 16);
			this.label8.TabIndex = 21;
			this.label8.Text = "12345678901234567890";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(112, 32);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(248, 16);
			this.label9.TabIndex = 24;
			this.label9.Text = "12345678901234567890";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(112, 56);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(248, 16);
			this.label10.TabIndex = 26;
			this.label10.Text = "12345678901234567890";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(112, 104);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(248, 16);
			this.label11.TabIndex = 28;
			this.label11.Text = "12345678901234567890";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(112, 80);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(248, 16);
			this.label12.TabIndex = 16;
			this.label12.Text = "12345678901234567890";
			// 
			// simTime
			// 
			this.simTime.Enabled = true;
			this.simTime.Tick += new System.EventHandler(this.simTime_Tick);
			// 
			// SimStatusControl
			// 
			this.Controls.Add(this.timeRemainingLbl);
			this.Controls.Add(this.timelabel);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.elapsedTime);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.currDate);
			this.Controls.Add(this.endDate);
			this.Controls.Add(this.startDate);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.simName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.currStatus);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label12);
			this.Name = "SimStatusControl";
			this.Size = new System.Drawing.Size(256, 224);
			this.ResumeLayout(false);

		}
		#endregion

		public OleDbConnection Connection 
		{
			set
			{	
				if (command == null)
				{
					command = new OleDbCommand();

					command.CommandTimeout = 0;
				}
				command.Connection = value;

				Reset(false);
			}
		}

		OleDbCommand command = null;


		private int totalNumDays;
		private DateTime start;
		private DateTime end;
		private double estMinRemaining;

		private int scenario_id = -1;
		public int Scenario
		{
			set
			{
				scenario_id = value;
			}

			get
			{
				return scenario_id;
			}
		}

		public void Reset(bool wait)
		{
			DataRow sim = simStatus();

			DateTime start = DateTime.Now;
			while(wait && sim == null)
			{		
				DateTime curr = DateTime.Now;
				sim = simStatus();

				TimeSpan elapse = curr - start;

				if (elapse.Seconds > 5)
				{
					break;
				}
			}

			if (sim == null)
			{
				simTime.Stop();
				this.currStatus.Text = "no sim running";
				return;
			}

			simTime.Start();			

			// get the scenario
			simName.Text = sim["name"].ToString();

			start = (DateTime) sim["start_date"];
			end = (DateTime) sim["end_date"];

			startDate.Text = start.ToShortDateString();
			endDate.Text = end.ToShortDateString();

			totalNumDays = (end - start).Days;

			if (totalNumDays < 1)
				totalNumDays = 1;

			currDate.Text = "";
			elapsedTime.Text = "";
			currStatus.Text = "";
			timeRemainingLbl.Text = "";

			estMinRemaining = 0;
			this.timelabel.Visible = false;
			this.timeRemainingLbl.Visible = false;

			return;
		}

		private void simTime_Tick(object sender, System.EventArgs e)
		{
			// update current status, elapsed time, current date

			DataRow sim = simStatus();

			
			if (sim == null)
			{
				simTime.Stop();
				this.currStatus.Text = "no sim running";
				return;
			}

			simName.Text = sim["name"].ToString();

			start = (DateTime) sim["start_date"];
			end = (DateTime) sim["end_date"];

			startDate.Text = start.ToShortDateString();
			endDate.Text = end.ToShortDateString();

			totalNumDays = (end - start).Days;

			if (totalNumDays < 1)
				totalNumDays = 1;

			// simulation time is in milliseconds
			double timeInSeconds = ((int) (sim["elapsed_time"])) * 0.001;

			DateTime curr = (DateTime) (sim["current_date"]);

			this.currDate.Text = curr.ToShortDateString();
			this.elapsedTime.Text = timeInSeconds.ToString();
			this.currStatus.Text = sim["current_status"].ToString();
			TimeSpan currSpan = curr - start;
			TimeSpan remainSpan = end - curr;
			TimeSpan totalSpan = end - start;
			int elapseTime = currSpan.Days;
			double  remainTime = (double) remainSpan.Days;
			double totalTime = (double) totalSpan.Days;

			// after 1 week we start estimating how much time is left
			if (elapseTime > 7 )
			{
				double remainingTime = ((timeInSeconds)/((double) elapseTime)*(remainTime))/60;

				// pessimistic
				remainingTime = Math.Ceiling(remainingTime);

				// estimate remaing time pessimistically
				if (elapseTime <= 21 && remainingTime > estMinRemaining)
					estMinRemaining = remainingTime;
				else
					estMinRemaining = (remainingTime + 2*estMinRemaining)/3;
			}

			// after 3 weeks we start display how much time is left in the sim
			if(elapseTime > 21 && elapseTime < totalTime )
			{
				this.timeRemainingLbl.Visible = true;
				this.timelabel.Visible = true;

				int numMinutesRemaining = (int) estMinRemaining;

				if( numMinutesRemaining > 1)
				{
					this.timeRemainingLbl.Text = numMinutesRemaining.ToString() + " minutes";
				}
				else if( numMinutesRemaining == 1)
				{
					this.timeRemainingLbl.Text = numMinutesRemaining.ToString() + " minute";
				}
				else
				{
					this.timeRemainingLbl.Text = " < 1 minute";
				}
			}
			else
			{
				this.timeRemainingLbl.Visible = false;
				this.timelabel.Visible = false;
			}


			int currDays = currSpan.Days;

			if (currDays >= 0)
			{
				progressBar.Value = 100 * currDays/totalNumDays;
			}

		}

		private void SimStatus_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// turn off timer
			simTime.Stop();
		}

		private DataRow simStatus()
		{
			if (command == null)
				return null;

			// ModelInfo simDB = new ModelInfo();
			
			
			command.CommandText = "SELECT scenario.name, elapsed_time, current_status, [current_date], " +
				" model_info.start_date, scenario.end_date " +
				" FROM model_info, sim_queue, scenario WHERE model_info.model_id = sim_queue.model_id AND sim_queue.scenario_id = scenario.scenario_id";
			
			if (Scenario >= 0)
			{
				command.CommandText += " AND scenario.scenario_id = " + Scenario;
			}
			else
			{
				command.CommandText += "AND status = 1";
			}


			OleDbDataAdapter da = new OleDbDataAdapter(command);

			DataSet ds = new DataSet();

			try
			{
				da.Fill(ds, "sim_queue");
			}
			catch(Exception)
			{
				return null;
			}

			if (ds.Tables["sim_queue"].Rows.Count == 0)
				return null;

			return ds.Tables["sim_queue"].Rows[0];
		}
	}
}
