namespace MrktSimClient.Controls.Dialogs.Calibration
{
    partial class VolumeCalibration
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.segmentViewButton = new System.Windows.Forms.RadioButton();
            this.channelViewButton = new System.Windows.Forms.RadioButton();
            this.methodBox = new System.Windows.Forms.ComboBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.catagorySalesGrid = new MarketSimUtilities.MrktSimGrid();
            this.segmentInfoGrid = new MarketSimUtilities.MrktSimGrid();
            this.plotControl1 = new Utilities.Graphing.PlotControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add( this.catagorySalesGrid );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Panel2MinSize = 150;
            this.splitContainer1.Size = new System.Drawing.Size( 820, 506 );
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 10;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.segmentInfoGrid );
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.splitContainer3 );
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Panel2MinSize = 120;
            this.splitContainer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size( 820, 332 );
            this.splitContainer2.SplitterDistance = 100;
            this.splitContainer2.TabIndex = 16;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add( this.segmentViewButton );
            this.splitContainer3.Panel1.Controls.Add( this.channelViewButton );
            this.splitContainer3.Panel1.Controls.Add( this.methodBox );
            this.splitContainer3.Panel1.Controls.Add( this.refreshButton );
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add( this.plotControl1 );
            this.splitContainer3.Size = new System.Drawing.Size( 820, 228 );
            this.splitContainer3.SplitterDistance = 222;
            this.splitContainer3.TabIndex = 17;
            // 
            // segmentViewButton
            // 
            this.segmentViewButton.AutoSize = true;
            this.segmentViewButton.Location = new System.Drawing.Point( 13, 158 );
            this.segmentViewButton.Name = "segmentViewButton";
            this.segmentViewButton.Size = new System.Drawing.Size( 107, 17 );
            this.segmentViewButton.TabIndex = 17;
            this.segmentViewButton.Text = "View by Segment";
            this.segmentViewButton.UseVisualStyleBackColor = true;
            // 
            // channelViewButton
            // 
            this.channelViewButton.AutoSize = true;
            this.channelViewButton.Checked = true;
            this.channelViewButton.Location = new System.Drawing.Point( 13, 135 );
            this.channelViewButton.Name = "channelViewButton";
            this.channelViewButton.Size = new System.Drawing.Size( 104, 17 );
            this.channelViewButton.TabIndex = 16;
            this.channelViewButton.TabStop = true;
            this.channelViewButton.Text = "View by Channel";
            this.channelViewButton.UseVisualStyleBackColor = true;
            this.channelViewButton.CheckedChanged += new System.EventHandler( this.channelViewButton_CheckedChanged );
            // 
            // methodBox
            // 
            this.methodBox.FormattingEnabled = true;
            this.methodBox.Items.AddRange( new object[] {
            "Do Not Change Model",
            "Change Population Size",
            "Change  Repurchase Frequency",
            "Change  Repurchase Units",
            "Create External Events"} );
            this.methodBox.Location = new System.Drawing.Point( 13, 59 );
            this.methodBox.Name = "methodBox";
            this.methodBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.methodBox.Size = new System.Drawing.Size( 186, 21 );
            this.methodBox.TabIndex = 13;
            this.methodBox.Text = "Select Method to Apply...";
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point( 13, 19 );
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size( 186, 23 );
            this.refreshButton.TabIndex = 15;
            this.refreshButton.Text = "Compute  Volume Changes";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler( this.refreshButton_Click );
            // 
            // catagorySalesGrid
            // 
            this.catagorySalesGrid.DescribeRow = null;
            this.catagorySalesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.catagorySalesGrid.EnabledGrid = true;
            this.catagorySalesGrid.Location = new System.Drawing.Point( 0, 0 );
            this.catagorySalesGrid.Name = "catagorySalesGrid";
            this.catagorySalesGrid.RowFilter = null;
            this.catagorySalesGrid.RowID = null;
            this.catagorySalesGrid.RowName = null;
            this.catagorySalesGrid.Size = new System.Drawing.Size( 820, 170 );
            this.catagorySalesGrid.Sort = "";
            this.catagorySalesGrid.TabIndex = 9;
            this.catagorySalesGrid.Table = null;
            // 
            // segmentInfoGrid
            // 
            this.segmentInfoGrid.DescribeRow = null;
            this.segmentInfoGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentInfoGrid.EnabledGrid = true;
            this.segmentInfoGrid.Location = new System.Drawing.Point( 0, 0 );
            this.segmentInfoGrid.Name = "segmentInfoGrid";
            this.segmentInfoGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.segmentInfoGrid.RowFilter = null;
            this.segmentInfoGrid.RowID = null;
            this.segmentInfoGrid.RowName = null;
            this.segmentInfoGrid.Size = new System.Drawing.Size( 820, 100 );
            this.segmentInfoGrid.Sort = "";
            this.segmentInfoGrid.TabIndex = 0;
            this.segmentInfoGrid.Table = null;
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
            this.plotControl1.Size = new System.Drawing.Size( 594, 228 );
            this.plotControl1.Start = new System.DateTime( 1899, 12, 30, 0, 0, 0, 0 );
            this.plotControl1.TabIndex = 16;
            this.plotControl1.TimeSeries = true;
            this.plotControl1.Title = "Title";
            this.plotControl1.XAxis = "Date";
            this.plotControl1.Y2Axis = "Percent";
            this.plotControl1.YAxis = "Y-Axis";
            // 
            // VolumeCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "VolumeCalibration";
            this.Size = new System.Drawing.Size( 820, 506 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.splitContainer3.Panel1.ResumeLayout( false );
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout( false );
            this.splitContainer3.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private MarketSimUtilities.MrktSimGrid catagorySalesGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ComboBox methodBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MarketSimUtilities.MrktSimGrid segmentInfoGrid;
        private Utilities.Graphing.PlotControl plotControl1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RadioButton segmentViewButton;
        private System.Windows.Forms.RadioButton channelViewButton;
    }
}
