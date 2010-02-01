using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Utilities;

namespace Common
{
	/// <summary>
	/// Summary description for OptControl.
	/// </summary>
	public class OptControl : MrktSimControl
	{
		public OptInfoDB theOptInfoDB;

		// plan variables
		private int iNumSteps;
		private int iModeExec;
		private int iExploreMode;
		private int iOptFor;
		private int iOptScenarioID;

		private System.Windows.Forms.TextBox OptPlanNameText;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox ExploreModeCombo;
		private System.Windows.Forms.ComboBox ModeExecCombo;
		private System.Windows.Forms.TextBox NumStepsText;
		private System.Windows.Forms.ComboBox OptForCombo;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button acceptbutton;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox SegmentCombo;
		private System.Windows.Forms.ComboBox ParamCombo;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox LeaderCheck;
		private System.Windows.Forms.CheckBox SlaveCheck;
		private System.Windows.Forms.TextBox LowerText;
		private System.Windows.Forms.TextBox UpperText;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox ExistingCombo;
		private System.Windows.Forms.DataGrid ParamsDataGrid;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OptControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		public override void Reset()
		{
			InitOptForm();
		}

		public void InitOptForm()
		{
			ExploreModeCombo.ValueMember = "explore_mode_id";
			ExploreModeCombo.DisplayMember = "explore_mode";
			ExploreModeCombo.DataSource = theOptInfoDB.OptData.Tables["optimization_explore_mode"];

			OptForCombo.ValueMember = "optimize_for_id";
			OptForCombo.DisplayMember = "optimize_for";
			OptForCombo.DataSource = theOptInfoDB.OptData.Tables["optimization_optimize_for"];

			ModeExecCombo.ValueMember = "mode_exec_id";
			ModeExecCombo.DisplayMember = "mode_exec";
			ModeExecCombo.DataSource = theOptInfoDB.OptData.Tables["optimization_mode_exec"];

			// segments
			SegmentCombo.DataSource = theDb.Data.Tables["Segment"];
			SegmentCombo.DisplayMember = "segment_name";

			// existing plans
			ExistingCombo.ValueMember = "scenario_id";
			ExistingCombo.DisplayMember = "scenario_name";
			ExistingCombo.DataSource = theOptInfoDB.OptData.Tables["optimization_plan"];

			// params
			ParamsDataGrid.DataSource = theOptInfoDB.OptData.Tables["optimization_params"];

			//foreach(OptInfo.optimization_explore_modeRow row in theOptInfoDB.OptData.optimization_explore_mode.Rows)
			//{
			//	row.explore_mode;
			//}
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
			this.OptPlanNameText = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.ExploreModeCombo = new System.Windows.Forms.ComboBox();
			this.ModeExecCombo = new System.Windows.Forms.ComboBox();
			this.NumStepsText = new System.Windows.Forms.TextBox();
			this.OptForCombo = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.acceptbutton = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.SegmentCombo = new System.Windows.Forms.ComboBox();
			this.ParamCombo = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.LeaderCheck = new System.Windows.Forms.CheckBox();
			this.SlaveCheck = new System.Windows.Forms.CheckBox();
			this.LowerText = new System.Windows.Forms.TextBox();
			this.UpperText = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.ExistingCombo = new System.Windows.Forms.ComboBox();
			this.ParamsDataGrid = new System.Windows.Forms.DataGrid();
			((System.ComponentModel.ISupportInitialize)(this.ParamsDataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// OptPlanNameText
			// 
			this.OptPlanNameText.Location = new System.Drawing.Point(64, 32);
			this.OptPlanNameText.Name = "OptPlanNameText";
			this.OptPlanNameText.Size = new System.Drawing.Size(144, 20);
			this.OptPlanNameText.TabIndex = 25;
			this.OptPlanNameText.Text = "Enter A Name Here";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 32);
			this.label7.Name = "label7";
			this.label7.TabIndex = 24;
			this.label7.Text = "Name:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 144);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(168, 23);
			this.label6.TabIndex = 23;
			this.label6.Text = "Parameters for Optimization";
			// 
			// ExploreModeCombo
			// 
			this.ExploreModeCombo.Location = new System.Drawing.Point(344, 96);
			this.ExploreModeCombo.Name = "ExploreModeCombo";
			this.ExploreModeCombo.Size = new System.Drawing.Size(121, 21);
			this.ExploreModeCombo.TabIndex = 22;
			this.ExploreModeCombo.Text = "comboBox3";
			this.ExploreModeCombo.SelectedIndexChanged += new System.EventHandler(this.ExploreModeCombo_SelectedIndexChanged);
			// 
			// ModeExecCombo
			// 
			this.ModeExecCombo.Location = new System.Drawing.Point(344, 64);
			this.ModeExecCombo.Name = "ModeExecCombo";
			this.ModeExecCombo.Size = new System.Drawing.Size(121, 21);
			this.ModeExecCombo.TabIndex = 21;
			this.ModeExecCombo.Text = "comboBox2";
			this.ModeExecCombo.SelectedIndexChanged += new System.EventHandler(this.ModeExecCombo_SelectedIndexChanged);
			// 
			// NumStepsText
			// 
			this.NumStepsText.Location = new System.Drawing.Point(104, 96);
			this.NumStepsText.Name = "NumStepsText";
			this.NumStepsText.TabIndex = 20;
			this.NumStepsText.Text = "number";
			// 
			// OptForCombo
			// 
			this.OptForCombo.Location = new System.Drawing.Point(104, 64);
			this.OptForCombo.Name = "OptForCombo";
			this.OptForCombo.Size = new System.Drawing.Size(121, 21);
			this.OptForCombo.TabIndex = 19;
			this.OptForCombo.Text = "comboBox1";
			this.OptForCombo.SelectedIndexChanged += new System.EventHandler(this.OptForCombo_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 96);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 18;
			this.label5.Text = "Number of Steps:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(240, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 17;
			this.label4.Text = "Mode of Execution:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(240, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Explore Mode:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 15;
			this.label2.Text = "Optimize For:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 14;
			this.label1.Text = "Optimization Plan";
			// 
			// acceptbutton
			// 
			this.acceptbutton.Location = new System.Drawing.Point(416, 368);
			this.acceptbutton.Name = "acceptbutton";
			this.acceptbutton.TabIndex = 26;
			this.acceptbutton.Text = "Accept";
			this.acceptbutton.Click += new System.EventHandler(this.acceptbutton_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 168);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 23);
			this.label8.TabIndex = 28;
			this.label8.Text = "Segment:";
			// 
			// SegmentCombo
			// 
			this.SegmentCombo.Location = new System.Drawing.Point(80, 160);
			this.SegmentCombo.Name = "SegmentCombo";
			this.SegmentCombo.Size = new System.Drawing.Size(121, 21);
			this.SegmentCombo.TabIndex = 29;
			this.SegmentCombo.Text = "comboBox1";
			this.SegmentCombo.SelectedIndexChanged += new System.EventHandler(this.SegmentCombo_SelectedIndexChanged);
			// 
			// ParamCombo
			// 
			this.ParamCombo.Location = new System.Drawing.Point(80, 192);
			this.ParamCombo.Name = "ParamCombo";
			this.ParamCombo.Size = new System.Drawing.Size(121, 21);
			this.ParamCombo.TabIndex = 31;
			this.ParamCombo.Text = "comboBox1";
			this.ParamCombo.SelectedIndexChanged += new System.EventHandler(this.ParamCombo_SelectedIndexChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 192);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(60, 23);
			this.label9.TabIndex = 30;
			this.label9.Text = "Parameter:";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 232);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(60, 23);
			this.label10.TabIndex = 32;
			this.label10.Text = "Lower:";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(16, 264);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(60, 23);
			this.label11.TabIndex = 33;
			this.label11.Text = "Upper:";
			// 
			// LeaderCheck
			// 
			this.LeaderCheck.Location = new System.Drawing.Point(16, 288);
			this.LeaderCheck.Name = "LeaderCheck";
			this.LeaderCheck.Size = new System.Drawing.Size(64, 24);
			this.LeaderCheck.TabIndex = 34;
			this.LeaderCheck.Text = "Leader";
			this.LeaderCheck.CheckedChanged += new System.EventHandler(this.LeaderCheck_CheckedChanged);
			// 
			// SlaveCheck
			// 
			this.SlaveCheck.Location = new System.Drawing.Point(16, 312);
			this.SlaveCheck.Name = "SlaveCheck";
			this.SlaveCheck.Size = new System.Drawing.Size(56, 24);
			this.SlaveCheck.TabIndex = 35;
			this.SlaveCheck.Text = "Slave";
			this.SlaveCheck.CheckedChanged += new System.EventHandler(this.SlaveCheck_CheckedChanged);
			// 
			// LowerText
			// 
			this.LowerText.Location = new System.Drawing.Point(80, 232);
			this.LowerText.Name = "LowerText";
			this.LowerText.TabIndex = 36;
			this.LowerText.Text = "number";
			// 
			// UpperText
			// 
			this.UpperText.Location = new System.Drawing.Point(80, 264);
			this.UpperText.Name = "UpperText";
			this.UpperText.TabIndex = 37;
			this.UpperText.Text = "number";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(208, 32);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(136, 23);
			this.label12.TabIndex = 38;
			this.label12.Text = "or select an existing plan:";
			// 
			// ExistingCombo
			// 
			this.ExistingCombo.Location = new System.Drawing.Point(336, 32);
			this.ExistingCombo.Name = "ExistingCombo";
			this.ExistingCombo.Size = new System.Drawing.Size(144, 21);
			this.ExistingCombo.TabIndex = 39;
			this.ExistingCombo.Text = "comboBox1";
			this.ExistingCombo.SelectedIndexChanged += new System.EventHandler(this.ExistingCombo_SelectedIndexChanged);
			// 
			// ParamsDataGrid
			// 
			this.ParamsDataGrid.DataMember = "";
			this.ParamsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.ParamsDataGrid.Location = new System.Drawing.Point(216, 160);
			this.ParamsDataGrid.Name = "ParamsDataGrid";
			this.ParamsDataGrid.Size = new System.Drawing.Size(376, 192);
			this.ParamsDataGrid.TabIndex = 40;
			// 
			// OptControl
			// 
			this.Controls.Add(this.ParamsDataGrid);
			this.Controls.Add(this.ExistingCombo);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.UpperText);
			this.Controls.Add(this.LowerText);
			this.Controls.Add(this.SlaveCheck);
			this.Controls.Add(this.LeaderCheck);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.ParamCombo);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.SegmentCombo);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.OptPlanNameText);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.ExploreModeCombo);
			this.Controls.Add(this.ModeExecCombo);
			this.Controls.Add(this.NumStepsText);
			this.Controls.Add(this.OptForCombo);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.acceptbutton);
			this.Name = "OptControl";
			this.Size = new System.Drawing.Size(616, 408);
			((System.ComponentModel.ISupportInitialize)(this.ParamsDataGrid)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void ExploreModeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = ExploreModeCombo.SelectedValue.ToString();
			iExploreMode = Int32.Parse(str);
		}

		private void ModeExecCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = ModeExecCombo.SelectedValue.ToString();
			iModeExec = Int32.Parse(str);
		}

		private void OptForCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = OptForCombo.SelectedValue.ToString();
			iOptFor = Int32.Parse(str);
		}

		private void SegmentCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		;
		}

		private void acceptbutton_Click(object sender, System.EventArgs e)
		{
			iNumSteps = Int32.Parse(NumStepsText.Text);
		}

		private void ParamCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		;
		}

		private void LeaderCheck_CheckedChanged(object sender, System.EventArgs e)
		{
		;
		}

		private void SlaveCheck_CheckedChanged(object sender, System.EventArgs e)
		{
		;
		}

		private void ExistingCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = ExistingCombo.SelectedValue.ToString();
			iOptScenarioID = Int32.Parse(str);

			theOptInfoDB.SetScenarioID(iOptScenarioID);
			theOptInfoDB.SelectParams();
		}
	}
}
