using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using MrktSimDb.Metrics;

namespace ModelView.Dialogs
{
	/// <summary>
	/// Summary description for CreateCalibration.
	/// </summary>
	public class CreateCalibration : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCalibration(MrktSimDBSchema.simulationRow simulation)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            currentSimulation = simulation;

			componentType.DataSource = CalibrationControl.Types;
			componentType.DisplayMember = "Descr";

			// parse control string

            CalibrationControl cal = new CalibrationControl(currentSimulation.control_string);

			this.componentType.SelectedItem = cal.Type;

			decimal step = (decimal) cal.StepSize;

			if (step < stepSize.Minimum)
				stepSize.Minimum = step;
			else if (step >  stepSize.Maximum)
				stepSize.Maximum = step;

			stepSize.Value = step;

			decimal calTolerance = (decimal) cal.Tolerance;

			if (calTolerance < tolerance.Minimum)
				tolerance.Minimum = calTolerance;
			else if (calTolerance >  stepSize.Maximum)
				tolerance.Maximum = calTolerance;

			tolerance.Value = calTolerance;

			int iters =  cal.MaxIters;

			if (iters < maxiters.Minimum)
				maxiters.Minimum = iters;
			else if (iters >  stepSize.Maximum)
				maxiters.Maximum = iters;

			maxiters.Value = iters;
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
			this.stepSize = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.componentType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tolerance = new System.Windows.Forms.NumericUpDown();
			this.maxiters = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.stepSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tolerance)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.maxiters)).BeginInit();
			this.SuspendLayout();
			// 
			// stepSize
			// 
			this.stepSize.DecimalPlaces = 4;
			this.stepSize.Increment = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   131072});
			this.stepSize.Location = new System.Drawing.Point(160, 48);
			this.stepSize.Maximum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.stepSize.Minimum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 -2147483648});
			this.stepSize.Name = "stepSize";
			this.stepSize.Size = new System.Drawing.Size(168, 20);
			this.stepSize.TabIndex = 0;
			this.stepSize.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   262144});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(88, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Step Size";
			// 
			// componentType
			// 
			this.componentType.Items.AddRange(new object[] {
															   "Distribution",
															   "Display",
															   "Mass media"});
			this.componentType.Location = new System.Drawing.Point(160, 16);
			this.componentType.Name = "componentType";
			this.componentType.Size = new System.Drawing.Size(168, 21);
			this.componentType.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(56, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Calibration Type";
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.Location = new System.Drawing.Point(146, 152);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 6;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(250, 152);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(88, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Tolerance";
			// 
			// tolerance
			// 
			this.tolerance.DecimalPlaces = 6;
			this.tolerance.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		196608});
			this.tolerance.Location = new System.Drawing.Point(160, 80);
			this.tolerance.Maximum = new System.Decimal(new int[] {
																	  5,
																	  0,
																	  0,
																	  65536});
			this.tolerance.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  393216});
			this.tolerance.Name = "tolerance";
			this.tolerance.Size = new System.Drawing.Size(168, 20);
			this.tolerance.TabIndex = 9;
			this.tolerance.Value = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	393216});
			// 
			// maxiters
			// 
			this.maxiters.Location = new System.Drawing.Point(160, 112);
			this.maxiters.Maximum = new System.Decimal(new int[] {
																	 50,
																	 0,
																	 0,
																	 0});
			this.maxiters.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.maxiters.Name = "maxiters";
			this.maxiters.Size = new System.Drawing.Size(168, 20);
			this.maxiters.TabIndex = 10;
			this.maxiters.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(40, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 23);
			this.label4.TabIndex = 11;
			this.label4.Text = "Maximum Iterations";
			// 
			// CreateCalibration
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(338, 184);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.maxiters);
			this.Controls.Add(this.tolerance);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.componentType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.stepSize);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CreateCalibration";
			this.Text = "Create Calibration";
			((System.ComponentModel.ISupportInitialize)(this.stepSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tolerance)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.maxiters)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.NumericUpDown stepSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox componentType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown tolerance;
		private System.Windows.Forms.NumericUpDown maxiters;
		private System.Windows.Forms.Label label4;

		private MrktSimDBSchema.simulationRow currentSimulation;

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			CalibrationControl cal = new CalibrationControl();

			cal.Type = (Value) this.componentType.SelectedItem;

			cal.StepSize = (double) stepSize.Value;
			cal.Tolerance = (double) tolerance.Value;
			cal.MaxIters = (int) maxiters.Value;

            currentSimulation.control_string = cal.Control;

			this.DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
