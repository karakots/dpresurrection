using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using System.Data;
using System.Data.OleDb;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for SimStatus.
	/// </summary>
	public class SimStatus : System.Windows.Forms.Form
	{

		private System.Windows.Forms.Label currStatus;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label simName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label startDate;
		private System.Windows.Forms.Label endDate;
		private System.Windows.Forms.Label currDate;
		private System.Windows.Forms.Label elapsedTime;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Timer simTime;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label timeRemainingLbl;
		private System.Windows.Forms.Label timelabel;
		private System.ComponentModel.IContainer components;

        public SimStatus(string path, string file, MrktSimDBSchema.simulationRow sim)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            if (sim == null)
                return;

            MSConnect msConnect = new MSConnect(path);

            if (msConnect == null)
            {
                return;
            }

            msConnect.ConnectFile = file;

            if (!msConnect.TestConnection())
            {
                return;
            }

            OleDbCommand genericCommand = Database.newOleDbCommand();

            genericCommand.Connection = msConnect.Connection;

            genericCommand.CommandText = "SELECT TOP(1) elapsed_time, current_status, [current_date] " +
              " FROM sim_queue WITH (NOLOCK) WHERE sim_id = " + sim.id + " ORDER BY run_time DESC";

            da = new OleDbDataAdapter(genericCommand);
            da.ContinueUpdateOnError = true;

            simQTable = new DataTable();

            init(sim);
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
			this.components = new System.ComponentModel.Container();
			this.currStatus = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.simName = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.startDate = new System.Windows.Forms.Label();
			this.endDate = new System.Windows.Forms.Label();
			this.currDate = new System.Windows.Forms.Label();
			this.elapsedTime = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.simTime = new System.Windows.Forms.Timer(this.components);
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.timelabel = new System.Windows.Forms.Label();
			this.timeRemainingLbl = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// currStatus
			// 
			this.currStatus.Location = new System.Drawing.Point(96, 80);
			this.currStatus.Name = "currStatus";
			this.currStatus.Size = new System.Drawing.Size(248, 16);
			this.currStatus.TabIndex = 0;
			this.currStatus.Text = "12345678901234567890";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Status";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Current Date";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Name";
			// 
			// simName
			// 
			this.simName.Location = new System.Drawing.Point(96, 8);
			this.simName.Name = "simName";
			this.simName.Size = new System.Drawing.Size(248, 16);
			this.simName.TabIndex = 4;
			this.simName.Text = "12345678901234567890";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 5;
			this.label5.Text = "Start Date";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "End Date";
			// 
			// startDate
			// 
			this.startDate.Location = new System.Drawing.Point(96, 32);
			this.startDate.Name = "startDate";
			this.startDate.Size = new System.Drawing.Size(248, 16);
			this.startDate.TabIndex = 7;
			this.startDate.Text = "12345678901234567890";
			// 
			// endDate
			// 
			this.endDate.Location = new System.Drawing.Point(96, 56);
			this.endDate.Name = "endDate";
			this.endDate.Size = new System.Drawing.Size(248, 16);
			this.endDate.TabIndex = 8;
			this.endDate.Text = "12345678901234567890";
			// 
			// currDate
			// 
			this.currDate.Location = new System.Drawing.Point(96, 104);
			this.currDate.Name = "currDate";
			this.currDate.Size = new System.Drawing.Size(248, 16);
			this.currDate.TabIndex = 9;
			this.currDate.Text = "12345678901234567890";
			// 
			// elapsedTime
			// 
			this.elapsedTime.Location = new System.Drawing.Point(96, 128);
			this.elapsedTime.Name = "elapsedTime";
			this.elapsedTime.Size = new System.Drawing.Size(248, 16);
			this.elapsedTime.TabIndex = 11;
			this.elapsedTime.Text = "12345678901234567890";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 128);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Elapsed Time";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(120, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(248, 16);
			this.label8.TabIndex = 4;
			this.label8.Text = "12345678901234567890";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(120, 32);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(248, 16);
			this.label9.TabIndex = 7;
			this.label9.Text = "12345678901234567890";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(120, 56);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(248, 16);
			this.label10.TabIndex = 8;
			this.label10.Text = "12345678901234567890";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(120, 104);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(248, 16);
			this.label11.TabIndex = 9;
			this.label11.Text = "12345678901234567890";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(120, 80);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(248, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "12345678901234567890";
			// 
			// simTime
			// 
			this.simTime.Enabled = true;
			this.simTime.Interval = 2000;
			this.simTime.Tick += new System.EventHandler(this.simTime_Tick);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar.Location = new System.Drawing.Point(16, 184);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(208, 23);
			this.progressBar.TabIndex = 12;
			// 
			// timelabel
			// 
			this.timelabel.Location = new System.Drawing.Point(16, 160);
			this.timelabel.Name = "timelabel";
			this.timelabel.Size = new System.Drawing.Size(88, 16);
			this.timelabel.TabIndex = 13;
			this.timelabel.Text = "Time Remaining";
			// 
			// timeRemainingLbl
			// 
			this.timeRemainingLbl.Location = new System.Drawing.Point(120, 160);
			this.timeRemainingLbl.Name = "timeRemainingLbl";
			this.timeRemainingLbl.Size = new System.Drawing.Size(112, 16);
			this.timeRemainingLbl.TabIndex = 14;
			this.timeRemainingLbl.Text = "1234567890";
			// 
			// SimStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(242, 216);
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
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SimStatus";
			this.Text = "Simulation Status";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SimStatus_Closing);
			this.ResumeLayout(false);

		}
		#endregion


        DataTable simQTable = null;
        OleDbDataAdapter da = null;

		private int totalNumDays;
		private DateTime start;
		private DateTime end;
		private double estMinRemaining;

      
		private void init(MrktSimDBSchema.simulationRow sim)
		{
            simName.Text = sim.name;

			start = sim.start_date;
            end = sim.end_date;

			startDate.Text = start.ToShortDateString();
			endDate.Text = end.ToShortDateString();

			totalNumDays = (end - start).Days;

			if (totalNumDays < 1)
				totalNumDays = 1;

			currDate.Text = "";
			elapsedTime.Text = "";
			currStatus.Text = "";
			timeRemainingLbl.Text = "";

			estMinRemaining = -1;
			this.timelabel.Visible = false;
			this.timeRemainingLbl.Visible = false;
		}

        private DataRow update()
        {
            if (da == null)
                return null;

            simQTable.Clear();

            try
            {
                da.Fill(simQTable);
            }
            catch (Exception)
            {
                return null;
            }

            if (simQTable.Rows.Count == 0)
                return null;

            return simQTable.Rows[0];
        }


		private void simTime_Tick(object sender, System.EventArgs e)
		{
			// update current status, elapsed time, current date

            DataRow run = update();

            if (run == null)
			{
				simTime.Stop();
				this.currStatus.Text = "no connection";
				return;
			}

			// simulation time is in milliseconds
            double timeInSeconds = ((int)(run["elapsed_time"])) * 0.001;

            DateTime curr = (DateTime)(run["current_date"]);

			this.currDate.Text = curr.ToShortDateString();

			int seconds = (int) Math.Floor(timeInSeconds);
			int minutes = seconds/60;
			int hours = minutes/60;

			seconds = seconds % 60;
			minutes = minutes % 60;

			this.elapsedTime.Text = hours.ToString() + "h  " + minutes.ToString() + "m  " + seconds.ToString() + "s";

            this.currStatus.Text = run["current_status"].ToString();
			TimeSpan currSpan = curr - start;
			TimeSpan remainSpan = end - curr;
			TimeSpan totalSpan = end - start;
			int elapseTime = currSpan.Days;
			double  remainTime = (double) remainSpan.Days;
			double totalTime = (double) totalSpan.Days;

			// after 1 week we start estimating how much time is left
            if( elapseTime > 7 ) {
                double remainingTime = ((timeInSeconds) / ((double)elapseTime) * (remainTime)) / 60;

                // pessimistic
                remainingTime = Math.Ceiling( remainingTime );

                // estimate remaing time 
                if( estMinRemaining < 0 ) {
                    estMinRemaining = remainingTime;
                }
                else {
                    estMinRemaining = (remainingTime + 2 * estMinRemaining) / 3;
                }
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

			if (currDays >= 0 && currDays <= totalNumDays)
			{
				progressBar.Value = 100 * currDays/totalNumDays;
			}

		}

		private void SimStatus_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// turn off timer
			simTime.Stop();
		
		}
	}
}
