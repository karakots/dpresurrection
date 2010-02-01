namespace MrktSimClient.Controls.Dialogs
{
    partial class SimulationSetUpControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.simType = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.initialSeed = new System.Windows.Forms.NumericUpDown();
            this.seedLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numConsumers = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numSimsLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numSpans = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.access_timeCombo = new System.Windows.Forms.ComboBox();
            this.scalingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.startEndDate = new Common.Utilities.StartEndDate();
            this.resetPanelCheckBox = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scalingNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.descrBox );
            this.splitContainer1.Panel1.Controls.Add( this.label1 );
            this.splitContainer1.Panel1.Controls.Add( this.nameBox );
            this.splitContainer1.Panel1.Controls.Add( this.label2 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.resetPanelCheckBox );
            this.splitContainer1.Panel2.Controls.Add( this.simType );
            this.splitContainer1.Panel2.Controls.Add( this.label8 );
            this.splitContainer1.Panel2.Controls.Add( this.initialSeed );
            this.splitContainer1.Panel2.Controls.Add( this.seedLabel );
            this.splitContainer1.Panel2.Controls.Add( this.label7 );
            this.splitContainer1.Panel2.Controls.Add( this.numConsumers );
            this.splitContainer1.Panel2.Controls.Add( this.label6 );
            this.splitContainer1.Panel2.Controls.Add( this.numSimsLabel );
            this.splitContainer1.Panel2.Controls.Add( this.label5 );
            this.splitContainer1.Panel2.Controls.Add( this.label4 );
            this.splitContainer1.Panel2.Controls.Add( this.numSpans );
            this.splitContainer1.Panel2.Controls.Add( this.label3 );
            this.splitContainer1.Panel2.Controls.Add( this.access_timeCombo );
            this.splitContainer1.Panel2.Controls.Add( this.scalingNumericUpDown );
            this.splitContainer1.Panel2.Controls.Add( this.startEndDate );
            this.splitContainer1.Size = new System.Drawing.Size( 542, 504 );
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 0;
            // 
            // descrBox
            // 
            this.descrBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descrBox.Location = new System.Drawing.Point( 0, 46 );
            this.descrBox.Multiline = true;
            this.descrBox.Name = "descrBox";
            this.descrBox.Size = new System.Drawing.Size( 542, 86 );
            this.descrBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point( 0, 33 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 60, 13 );
            this.label1.TabIndex = 8;
            this.label1.Text = "Description";
            // 
            // nameBox
            // 
            this.nameBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameBox.Location = new System.Drawing.Point( 0, 13 );
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size( 542, 20 );
            this.nameBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point( 0, 0 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 35, 13 );
            this.label2.TabIndex = 6;
            this.label2.Text = "Name";
            // 
            // simType
            // 
            this.simType.AutoSize = true;
            this.simType.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.simType.Location = new System.Drawing.Point( 162, 260 );
            this.simType.Name = "simType";
            this.simType.Size = new System.Drawing.Size( 93, 13 );
            this.simType.TabIndex = 34;
            this.simType.Text = "Parallel Search";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 48, 260 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 85, 13 );
            this.label8.TabIndex = 33;
            this.label8.Text = "Simulation Type:";
            // 
            // initialSeed
            // 
            this.initialSeed.Location = new System.Drawing.Point( 179, 187 );
            this.initialSeed.Maximum = new decimal( new int[] {
            999999,
            0,
            0,
            0} );
            this.initialSeed.Minimum = new decimal( new int[] {
            999999,
            0,
            0,
            -2147483648} );
            this.initialSeed.Name = "initialSeed";
            this.initialSeed.Size = new System.Drawing.Size( 66, 20 );
            this.initialSeed.TabIndex = 31;
            this.initialSeed.Value = new decimal( new int[] {
            12345,
            0,
            0,
            0} );
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point( 96, 189 );
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size( 75, 13 );
            this.seedLabel.TabIndex = 32;
            this.seedLabel.Text = "Random Seed";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 38, 128 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 133, 13 );
            this.label7.TabIndex = 29;
            this.label7.Text = "Number of real  consumers";
            // 
            // numConsumers
            // 
            this.numConsumers.AutoSize = true;
            this.numConsumers.Location = new System.Drawing.Point( 178, 128 );
            this.numConsumers.Name = "numConsumers";
            this.numConsumers.Size = new System.Drawing.Size( 67, 13 );
            this.numConsumers.TabIndex = 28;
            this.numConsumers.Text = "110,000,000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 79, 232 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 84, 13 );
            this.label6.TabIndex = 27;
            this.label6.Text = "Number of Runs";
            // 
            // numSimsLabel
            // 
            this.numSimsLabel.AutoSize = true;
            this.numSimsLabel.Location = new System.Drawing.Point( 178, 232 );
            this.numSimsLabel.Name = "numSimsLabel";
            this.numSimsLabel.Size = new System.Drawing.Size( 19, 13 );
            this.numSimsLabel.TabIndex = 26;
            this.numSimsLabel.Text = "11";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label5.Location = new System.Drawing.Point( 247, 101 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 15, 13 );
            this.label5.TabIndex = 25;
            this.label5.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 71, 100 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 58, 13 );
            this.label4.TabIndex = 24;
            this.label4.Text = "Write Data";
            // 
            // numSpans
            // 
            this.numSpans.Location = new System.Drawing.Point( 268, 98 );
            this.numSpans.Name = "numSpans";
            this.numSpans.Size = new System.Drawing.Size( 36, 20 );
            this.numSpans.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 155 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 159, 13 );
            this.label3.TabIndex = 23;
            this.label3.Text = "Number of simulation consumers";
            // 
            // access_timeCombo
            // 
            this.access_timeCombo.FormattingEnabled = true;
            this.access_timeCombo.Items.AddRange( new object[] {
            "Daily",
            "Weekly",
            "Monthly"} );
            this.access_timeCombo.Location = new System.Drawing.Point( 142, 97 );
            this.access_timeCombo.Name = "access_timeCombo";
            this.access_timeCombo.Size = new System.Drawing.Size( 97, 21 );
            this.access_timeCombo.TabIndex = 17;
            // 
            // scalingNumericUpDown
            // 
            this.scalingNumericUpDown.Increment = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.scalingNumericUpDown.Location = new System.Drawing.Point( 181, 153 );
            this.scalingNumericUpDown.Maximum = new decimal( new int[] {
            999999,
            0,
            0,
            0} );
            this.scalingNumericUpDown.Name = "scalingNumericUpDown";
            this.scalingNumericUpDown.Size = new System.Drawing.Size( 64, 20 );
            this.scalingNumericUpDown.TabIndex = 18;
            this.scalingNumericUpDown.ThousandsSeparator = true;
            this.scalingNumericUpDown.Value = new decimal( new int[] {
            999999,
            0,
            0,
            0} );
            // 
            // startEndDate
            // 
            this.startEndDate.End = new System.DateTime( 2007, 2, 5, 14, 5, 35, 953 );
            this.startEndDate.Location = new System.Drawing.Point( 39, 13 );
            this.startEndDate.Name = "startEndDate";
            this.startEndDate.Size = new System.Drawing.Size( 200, 48 );
            this.startEndDate.Start = new System.DateTime( 2007, 2, 5, 14, 5, 35, 953 );
            this.startEndDate.TabIndex = 16;
            // 
            // resetPanelCheckBox
            // 
            this.resetPanelCheckBox.AutoSize = true;
            this.resetPanelCheckBox.Location = new System.Drawing.Point( 40, 61 );
            this.resetPanelCheckBox.Name = "resetPanelCheckBox";
            this.resetPanelCheckBox.Size = new System.Drawing.Size( 198, 17 );
            this.resetPanelCheckBox.TabIndex = 35;
            this.resetPanelCheckBox.Text = "Reset Panel Data at Simulation Start";
            this.resetPanelCheckBox.UseVisualStyleBackColor = true;
            // 
            // SimulationSetUpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "SimulationSetUpControl";
            this.Size = new System.Drawing.Size( 542, 504 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.initialSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scalingNumericUpDown)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox descrBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSpans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox access_timeCombo;
        private System.Windows.Forms.NumericUpDown scalingNumericUpDown;
        private Common.Utilities.StartEndDate startEndDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label numConsumers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label numSimsLabel;
        private System.Windows.Forms.NumericUpDown initialSeed;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Label simType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox resetPanelCheckBox;
    }
}
