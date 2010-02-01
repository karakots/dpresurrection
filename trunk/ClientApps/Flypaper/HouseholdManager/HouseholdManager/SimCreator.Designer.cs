namespace HouseholdManager
{
    partial class SimCreator
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SegmentsList = new System.Windows.Forms.ListBox();
            this.label20 = new System.Windows.Forms.Label();
            this.MediaList = new System.Windows.Forms.ListBox();
            this.AddMediaButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.SimNameText = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.SubTypeCombo = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.LengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.StartNumeric = new System.Windows.Forms.NumericUpDown();
            this.Type = new System.Windows.Forms.Label();
            this.TypeCombo = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.SimLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.RunButton = new System.Windows.Forms.Button();
            this.AddSegmentButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.optionBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ImpressionsNumeric = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.FuzzyFactor = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TargetGeoCombo = new System.Windows.Forms.ComboBox();
            this.TargetSegmentButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.RegionCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OptionCombo = new System.Windows.Forms.ComboBox();
            this.VehicleCombo = new System.Windows.Forms.ComboBox();
            this.run_sim = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SimLengthNumeric)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImpressionsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FuzzyFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // SegmentsList
            // 
            this.SegmentsList.Dock = System.Windows.Forms.DockStyle.Top;
            this.SegmentsList.FormattingEnabled = true;
            this.SegmentsList.Location = new System.Drawing.Point(0, 0);
            this.SegmentsList.Name = "SegmentsList";
            this.SegmentsList.Size = new System.Drawing.Size(810, 134);
            this.SegmentsList.TabIndex = 4;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(433, 229);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 13);
            this.label20.TabIndex = 9;
            this.label20.Text = "Media";
            // 
            // MediaList
            // 
            this.MediaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaList.FormattingEnabled = true;
            this.MediaList.Location = new System.Drawing.Point(0, 134);
            this.MediaList.Name = "MediaList";
            this.MediaList.Size = new System.Drawing.Size(810, 420);
            this.MediaList.TabIndex = 8;
            // 
            // AddMediaButton
            // 
            this.AddMediaButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddMediaButton.Location = new System.Drawing.Point(3, 315);
            this.AddMediaButton.Name = "AddMediaButton";
            this.AddMediaButton.Size = new System.Drawing.Size(206, 23);
            this.AddMediaButton.TabIndex = 19;
            this.AddMediaButton.Text = "Add Media";
            this.AddMediaButton.UseVisualStyleBackColor = true;
            this.AddMediaButton.Click += new System.EventHandler(this.AddMediaButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Simulation Name";
            // 
            // SimNameText
            // 
            this.SimNameText.Location = new System.Drawing.Point(8, 25);
            this.SimNameText.Name = "SimNameText";
            this.SimNameText.Size = new System.Drawing.Size(208, 20);
            this.SimNameText.TabIndex = 16;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 48);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 13);
            this.label19.TabIndex = 15;
            this.label19.Text = "SubType";
            // 
            // SubTypeCombo
            // 
            this.SubTypeCombo.FormattingEnabled = true;
            this.SubTypeCombo.Location = new System.Drawing.Point(83, 45);
            this.SubTypeCombo.Name = "SubTypeCombo";
            this.SubTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.SubTypeCombo.TabIndex = 14;
            this.SubTypeCombo.SelectedIndexChanged += new System.EventHandler(this.SubTypeCombo_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 183);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Length";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 157);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 13);
            this.label14.TabIndex = 10;
            this.label14.Text = "Start Day";
            // 
            // LengthNumeric
            // 
            this.LengthNumeric.Location = new System.Drawing.Point(84, 181);
            this.LengthNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LengthNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LengthNumeric.Name = "LengthNumeric";
            this.LengthNumeric.Size = new System.Drawing.Size(120, 20);
            this.LengthNumeric.TabIndex = 7;
            this.LengthNumeric.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // StartNumeric
            // 
            this.StartNumeric.Location = new System.Drawing.Point(84, 155);
            this.StartNumeric.Name = "StartNumeric";
            this.StartNumeric.Size = new System.Drawing.Size(120, 20);
            this.StartNumeric.TabIndex = 6;
            // 
            // Type
            // 
            this.Type.AutoSize = true;
            this.Type.Location = new System.Drawing.Point(10, 22);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(31, 13);
            this.Type.TabIndex = 3;
            this.Type.Text = "Type";
            // 
            // TypeCombo
            // 
            this.TypeCombo.FormattingEnabled = true;
            this.TypeCombo.Location = new System.Drawing.Point(84, 19);
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.Size = new System.Drawing.Size(121, 21);
            this.TypeCombo.TabIndex = 2;
            this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(428, 15);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Define Media";
            // 
            // SimLengthNumeric
            // 
            this.SimLengthNumeric.Location = new System.Drawing.Point(134, 470);
            this.SimLengthNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SimLengthNumeric.Name = "SimLengthNumeric";
            this.SimLengthNumeric.Size = new System.Drawing.Size(78, 20);
            this.SimLengthNumeric.TabIndex = 10;
            this.SimLengthNumeric.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // RunButton
            // 
            this.RunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RunButton.Location = new System.Drawing.Point(67, 526);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 12;
            this.RunButton.Text = "Run!";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // AddSegmentButton
            // 
            this.AddSegmentButton.Location = new System.Drawing.Point(8, 77);
            this.AddSegmentButton.Name = "AddSegmentButton";
            this.AddSegmentButton.Size = new System.Drawing.Size(138, 23);
            this.AddSegmentButton.TabIndex = 13;
            this.AddSegmentButton.Text = "Add Segment...";
            this.AddSegmentButton.UseVisualStyleBackColor = true;
            this.AddSegmentButton.Click += new System.EventHandler(this.AddSegmentButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.optionBox);
            this.splitContainer1.Panel1.Controls.Add(this.SimLengthNumeric);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.RunButton);
            this.splitContainer1.Panel1.Controls.Add(this.AddSegmentButton);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.SimNameText);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MediaList);
            this.splitContainer1.Panel2.Controls.Add(this.SegmentsList);
            this.splitContainer1.Size = new System.Drawing.Size(1036, 561);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Option";
            // 
            // optionBox
            // 
            this.optionBox.Location = new System.Drawing.Point(49, 51);
            this.optionBox.Name = "optionBox";
            this.optionBox.Size = new System.Drawing.Size(100, 20);
            this.optionBox.TabIndex = 21;
            this.optionBox.Text = "Test";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 474);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Length of Simulation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ImpressionsNumeric);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.FuzzyFactor);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TargetGeoCombo);
            this.groupBox1.Controls.Add(this.TargetSegmentButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.RegionCombo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.OptionCombo);
            this.groupBox1.Controls.Add(this.VehicleCombo);
            this.groupBox1.Controls.Add(this.AddMediaButton);
            this.groupBox1.Controls.Add(this.TypeCombo);
            this.groupBox1.Controls.Add(this.LengthNumeric);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.SubTypeCombo);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.StartNumeric);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.Type);
            this.groupBox1.Location = new System.Drawing.Point(8, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 341);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Media";
            // 
            // ImpressionsNumeric
            // 
            this.ImpressionsNumeric.Location = new System.Drawing.Point(83, 207);
            this.ImpressionsNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ImpressionsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ImpressionsNumeric.Name = "ImpressionsNumeric";
            this.ImpressionsNumeric.Size = new System.Drawing.Size(120, 20);
            this.ImpressionsNumeric.TabIndex = 31;
            this.ImpressionsNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Impressions";
            // 
            // FuzzyFactor
            // 
            this.FuzzyFactor.Enabled = false;
            this.FuzzyFactor.Location = new System.Drawing.Point(83, 262);
            this.FuzzyFactor.Name = "FuzzyFactor";
            this.FuzzyFactor.Size = new System.Drawing.Size(120, 20);
            this.FuzzyFactor.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 264);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Fuzz Factor";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Target Geo";
            // 
            // TargetGeoCombo
            // 
            this.TargetGeoCombo.Enabled = false;
            this.TargetGeoCombo.FormattingEnabled = true;
            this.TargetGeoCombo.Location = new System.Drawing.Point(81, 288);
            this.TargetGeoCombo.Name = "TargetGeoCombo";
            this.TargetGeoCombo.Size = new System.Drawing.Size(121, 21);
            this.TargetGeoCombo.TabIndex = 27;
            this.TargetGeoCombo.SelectedIndexChanged += new System.EventHandler(this.TargetGeoCombo_SelectedIndexChanged);
            // 
            // TargetSegmentButton
            // 
            this.TargetSegmentButton.Enabled = false;
            this.TargetSegmentButton.Location = new System.Drawing.Point(10, 233);
            this.TargetSegmentButton.Name = "TargetSegmentButton";
            this.TargetSegmentButton.Size = new System.Drawing.Size(138, 23);
            this.TargetSegmentButton.TabIndex = 26;
            this.TargetSegmentButton.Text = "Add Segment...";
            this.TargetSegmentButton.UseVisualStyleBackColor = true;
            this.TargetSegmentButton.Click += new System.EventHandler(this.TargetSegmentButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Region";
            // 
            // RegionCombo
            // 
            this.RegionCombo.FormattingEnabled = true;
            this.RegionCombo.Location = new System.Drawing.Point(83, 72);
            this.RegionCombo.Name = "RegionCombo";
            this.RegionCombo.Size = new System.Drawing.Size(121, 21);
            this.RegionCombo.TabIndex = 24;
            this.RegionCombo.SelectedIndexChanged += new System.EventHandler(this.RegionCombo_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Ad Option";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Vehicle";
            // 
            // OptionCombo
            // 
            this.OptionCombo.FormattingEnabled = true;
            this.OptionCombo.Location = new System.Drawing.Point(83, 127);
            this.OptionCombo.Name = "OptionCombo";
            this.OptionCombo.Size = new System.Drawing.Size(121, 21);
            this.OptionCombo.TabIndex = 21;
            // 
            // VehicleCombo
            // 
            this.VehicleCombo.FormattingEnabled = true;
            this.VehicleCombo.Location = new System.Drawing.Point(83, 99);
            this.VehicleCombo.Name = "VehicleCombo";
            this.VehicleCombo.Size = new System.Drawing.Size(121, 21);
            this.VehicleCombo.TabIndex = 20;
            this.VehicleCombo.SelectedIndexChanged += new System.EventHandler(this.VehicleCombo_SelectedIndexChanged);
            // 
            // run_sim
            // 
            this.run_sim.Tick += new System.EventHandler(this.run_sim_Tick);
            // 
            // SimCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label18);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimCreator";
            this.Text = "Create your own simulation - it\'s fun!";
            ((System.ComponentModel.ISupportInitialize)(this.LengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SimLengthNumeric)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImpressionsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FuzzyFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SegmentsList;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListBox MediaList;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox SimNameText;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox SubTypeCombo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown LengthNumeric;
        private System.Windows.Forms.NumericUpDown StartNumeric;
        private System.Windows.Forms.Label Type;
        private System.Windows.Forms.ComboBox TypeCombo;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown SimLengthNumeric;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button AddMediaButton;
        private System.Windows.Forms.Button AddSegmentButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox optionBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox OptionCombo;
        private System.Windows.Forms.ComboBox VehicleCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox RegionCombo;
        private System.Windows.Forms.Button TargetSegmentButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox TargetGeoCombo;
        private System.Windows.Forms.NumericUpDown FuzzyFactor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ImpressionsNumeric;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer run_sim;
    }
}