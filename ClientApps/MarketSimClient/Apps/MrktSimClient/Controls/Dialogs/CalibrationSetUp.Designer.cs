namespace MrktSimClient.Controls.Dialogs
{
    partial class CalibrationSetUp
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.calibrationType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.maxiters = new System.Windows.Forms.NumericUpDown();
            this.tolerance = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.metricType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stepSize = new System.Windows.Forms.NumericUpDown();
            this.calibrationInfo = new System.Windows.Forms.TextBox();
            this.createVariables = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.calSubType = new System.Windows.Forms.CheckedListBox();
            this.applyParms = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.maxiters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // calibrationType
            // 
            this.calibrationType.FormattingEnabled = true;
            this.calibrationType.Location = new System.Drawing.Point( 144, 7 );
            this.calibrationType.Name = "calibrationType";
            this.calibrationType.Size = new System.Drawing.Size( 172, 21 );
            this.calibrationType.TabIndex = 25;
            this.calibrationType.SelectedIndexChanged += new System.EventHandler( this.calibrationType_SelectedIndexChanged );
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 82, 41 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 36, 13 );
            this.label5.TabIndex = 24;
            this.label5.Text = "Metric";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point( 29, 167 );
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size( 165, 17 );
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Clear all results after each run";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler( this.updateCalibrationInfo );
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 26, 134 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 111, 20 );
            this.label4.TabIndex = 22;
            this.label4.Text = "Maximum Iterations";
            // 
            // maxiters
            // 
            this.maxiters.Location = new System.Drawing.Point( 144, 134 );
            this.maxiters.Maximum = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            this.maxiters.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.maxiters.Name = "maxiters";
            this.maxiters.Size = new System.Drawing.Size( 175, 20 );
            this.maxiters.TabIndex = 21;
            this.maxiters.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // tolerance
            // 
            this.tolerance.DecimalPlaces = 6;
            this.tolerance.Increment = new decimal( new int[] {
            1,
            0,
            0,
            196608} );
            this.tolerance.Location = new System.Drawing.Point( 144, 102 );
            this.tolerance.Maximum = new decimal( new int[] {
            5,
            0,
            0,
            65536} );
            this.tolerance.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            393216} );
            this.tolerance.Name = "tolerance";
            this.tolerance.Size = new System.Drawing.Size( 175, 20 );
            this.tolerance.TabIndex = 20;
            this.tolerance.Value = new decimal( new int[] {
            1,
            0,
            0,
            393216} );
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 67, 102 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 68, 20 );
            this.label3.TabIndex = 19;
            this.label3.Text = "Tolerance";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 40, 10 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 95, 23 );
            this.label2.TabIndex = 18;
            this.label2.Text = "Calibration Type";
            // 
            // metricType
            // 
            this.metricType.Items.AddRange( new object[] {
            "Distribution",
            "Display",
            "Mass media"} );
            this.metricType.Location = new System.Drawing.Point( 144, 38 );
            this.metricType.Name = "metricType";
            this.metricType.Size = new System.Drawing.Size( 175, 21 );
            this.metricType.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 72, 72 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 63, 20 );
            this.label1.TabIndex = 16;
            this.label1.Text = "Step Size";
            // 
            // stepSize
            // 
            this.stepSize.DecimalPlaces = 4;
            this.stepSize.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
            this.stepSize.Location = new System.Drawing.Point( 144, 70 );
            this.stepSize.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.stepSize.Minimum = new decimal( new int[] {
            10,
            0,
            0,
            -2147483648} );
            this.stepSize.Name = "stepSize";
            this.stepSize.Size = new System.Drawing.Size( 175, 20 );
            this.stepSize.TabIndex = 15;
            this.stepSize.Value = new decimal( new int[] {
            1,
            0,
            0,
            262144} );
            // 
            // calibrationInfo
            // 
            this.calibrationInfo.BackColor = System.Drawing.SystemColors.Info;
            this.calibrationInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.calibrationInfo.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.calibrationInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationInfo.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.calibrationInfo.ForeColor = System.Drawing.SystemColors.InfoText;
            this.calibrationInfo.Location = new System.Drawing.Point( 0, 215 );
            this.calibrationInfo.Multiline = true;
            this.calibrationInfo.Name = "calibrationInfo";
            this.calibrationInfo.ReadOnly = true;
            this.calibrationInfo.Size = new System.Drawing.Size( 511, 256 );
            this.calibrationInfo.TabIndex = 26;
            // 
            // createVariables
            // 
            this.createVariables.Location = new System.Drawing.Point( 213, 184 );
            this.createVariables.Name = "createVariables";
            this.createVariables.Size = new System.Drawing.Size( 211, 23 );
            this.createVariables.TabIndex = 27;
            this.createVariables.Text = "Create Price Sensitivity Parameters";
            this.createVariables.UseVisualStyleBackColor = true;
            this.createVariables.Click += new System.EventHandler( this.createVariables_Click );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.calSubType );
            this.panel1.Controls.Add( this.applyParms );
            this.panel1.Controls.Add( this.maxiters );
            this.panel1.Controls.Add( this.createVariables );
            this.panel1.Controls.Add( this.stepSize );
            this.panel1.Controls.Add( this.label1 );
            this.panel1.Controls.Add( this.calibrationType );
            this.panel1.Controls.Add( this.metricType );
            this.panel1.Controls.Add( this.label5 );
            this.panel1.Controls.Add( this.label2 );
            this.panel1.Controls.Add( this.checkBox1 );
            this.panel1.Controls.Add( this.label3 );
            this.panel1.Controls.Add( this.label4 );
            this.panel1.Controls.Add( this.tolerance );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 511, 215 );
            this.panel1.TabIndex = 28;
            // 
            // calSubType
            // 
            this.calSubType.CheckOnClick = true;
            this.calSubType.FormattingEnabled = true;
            this.calSubType.Location = new System.Drawing.Point( 355, 7 );
            this.calSubType.Name = "calSubType";
            this.calSubType.Size = new System.Drawing.Size( 124, 139 );
            this.calSubType.TabIndex = 29;
            // 
            // applyParms
            // 
            this.applyParms.AutoSize = true;
            this.applyParms.Location = new System.Drawing.Point( 29, 190 );
            this.applyParms.Name = "applyParms";
            this.applyParms.Size = new System.Drawing.Size( 148, 17 );
            this.applyParms.TabIndex = 28;
            this.applyParms.Text = "Apply Parameter Changes";
            this.applyParms.UseVisualStyleBackColor = true;
            // 
            // CalibrationSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.calibrationInfo );
            this.Controls.Add( this.panel1 );
            this.Name = "CalibrationSetUp";
            this.Size = new System.Drawing.Size( 511, 471 );
            ((System.ComponentModel.ISupportInitialize)(this.maxiters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize)).EndInit();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox calibrationType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxiters;
        private System.Windows.Forms.NumericUpDown tolerance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox metricType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown stepSize;
        private System.Windows.Forms.TextBox calibrationInfo;
        private System.Windows.Forms.Button createVariables;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox applyParms;
        private System.Windows.Forms.CheckedListBox calSubType;
    }
}
