namespace MrktSimClient.Controls.Dialogs.Calibration
{
    partial class AttributeCalibration
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
            this.solveButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.refreshBut = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.plotControl1 = new Utilities.Graphing.PlotControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point( 14, 18 );
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size( 168, 23 );
            this.solveButton.TabIndex = 1;
            this.solveButton.Text = "Compute Preferences";
            this.solveButton.UseVisualStyleBackColor = true;
            this.solveButton.Click += new System.EventHandler( this.solveButton_Click );
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
            this.splitContainer1.Panel1.Controls.Add( this.mrktSimGrid );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Size = new System.Drawing.Size( 605, 407 );
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 3;
            // 
            // mrktSimGrid
            // 
            this.mrktSimGrid.DescribeRow = null;
            this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid.EnabledGrid = true;
            this.mrktSimGrid.Location = new System.Drawing.Point( 0, 0 );
            this.mrktSimGrid.Name = "mrktSimGrid";
            this.mrktSimGrid.RowFilter = null;
            this.mrktSimGrid.RowID = null;
            this.mrktSimGrid.RowName = null;
            this.mrktSimGrid.Size = new System.Drawing.Size( 605, 141 );
            this.mrktSimGrid.Sort = "";
            this.mrktSimGrid.TabIndex = 2;
            this.mrktSimGrid.Table = null;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.refreshBut );
            this.splitContainer2.Panel1.Controls.Add( this.solveButton );
            this.splitContainer2.Panel1.Controls.Add( this.applyButton );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.plotControl1 );
            this.splitContainer2.Size = new System.Drawing.Size( 605, 262 );
            this.splitContainer2.SplitterDistance = 201;
            this.splitContainer2.TabIndex = 6;
            // 
            // refreshBut
            // 
            this.refreshBut.Location = new System.Drawing.Point( 14, 160 );
            this.refreshBut.Name = "refreshBut";
            this.refreshBut.Size = new System.Drawing.Size( 160, 23 );
            this.refreshBut.TabIndex = 4;
            this.refreshBut.Text = "Refresh Error Plot";
            this.refreshBut.UseVisualStyleBackColor = true;
            this.refreshBut.Click += new System.EventHandler( this.refreshBut_Click );
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point( 14, 56 );
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size( 168, 23 );
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply to Simulation Parameters";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler( this.applyButton_Click );
            // 
            // plotControl1
            // 
            this.plotControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotControl1.End = new System.DateTime( 1899, 12, 31, 4, 48, 0, 0 );
            this.plotControl1.Location = new System.Drawing.Point( 0, 0 );
            this.plotControl1.Max = 1.2;
            this.plotControl1.MaxX = 1.2;
            this.plotControl1.Min = 0;
            this.plotControl1.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.plotControl1.MinX = 0;
            this.plotControl1.Name = "plotControl1";
            this.plotControl1.PercentMax = 100;
            this.plotControl1.PercentMin = 0;
            this.plotControl1.ScatterPlot = false;
            this.plotControl1.Size = new System.Drawing.Size( 400, 262 );
            this.plotControl1.Start = new System.DateTime( 1899, 12, 30, 0, 0, 0, 0 );
            this.plotControl1.TabIndex = 0;
            this.plotControl1.TimeSeries = true;
            this.plotControl1.Title = "Title";
            this.plotControl1.XAxis = "Date";
            this.plotControl1.Y2Axis = "Percent";
            this.plotControl1.YAxis = "Y-Axis";
            // 
            // AttributeCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "AttributeCalibration";
            this.Size = new System.Drawing.Size( 605, 407 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button solveButton;
        private MarketSimUtilities.MrktSimGrid mrktSimGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Utilities.Graphing.PlotControl plotControl1;
        private System.Windows.Forms.Button refreshBut;
    }
}
