namespace MrktSimClient.Controls.Dialogs
{
    partial class CalibrationEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.runBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.setUpPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.attrTab = new System.Windows.Forms.TabPage();
            this.parmPage = new System.Windows.Forms.TabPage();
            this.varPage = new System.Windows.Forms.TabPage();
            this.volPage = new System.Windows.Forms.TabPage();
            this.pricePage = new System.Windows.Forms.TabPage();
            this.plotPage = new System.Windows.Forms.TabPage();
            this.simulationSetUpControl1 = new MrktSimClient.Controls.Dialogs.SimulationSetUpControl();
            this.calibrationSetUp1 = new MrktSimClient.Controls.Dialogs.CalibrationSetUp();
            this.attributeCalibration1 = new MrktSimClient.Controls.Dialogs.Calibration.AttributeCalibration();
            this.parameterControl1 = new MrktSimClient.Controls.Dialogs.ParameterControl();
            this.variableControl1 = new MrktSimClient.Controls.Dialogs.VariableControl();
            this.volumeCalibration1 = new MrktSimClient.Controls.Dialogs.Calibration.VolumeCalibration();
            this.priceCalibration1 = new MrktSimClient.Controls.Dialogs.Calibration.PriceCalibration();
            this.errorPlot = new Utilities.Graphing.PlotControl();
            this.mediaPage = new System.Windows.Forms.TabPage();
            this.mediaCalibration1 = new MrktSimClient.Controls.Dialogs.Calibration.MediaCalibration();
            this.panel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.setUpPage.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.attrTab.SuspendLayout();
            this.parmPage.SuspendLayout();
            this.varPage.SuspendLayout();
            this.volPage.SuspendLayout();
            this.pricePage.SuspendLayout();
            this.plotPage.SuspendLayout();
            this.mediaPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.cancelButton );
            this.panel1.Controls.Add( this.acceptButton );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point( 0, 612 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 982, 52 );
            this.panel1.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point( 767, 17 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point( 872, 17 );
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size( 75, 23 );
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler( this.acceptButton_Click );
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.runBox );
            this.splitContainer2.Panel1.Controls.Add( this.label1 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.tabControl1 );
            this.splitContainer2.Size = new System.Drawing.Size( 982, 612 );
            this.splitContainer2.SplitterDistance = 48;
            this.splitContainer2.TabIndex = 3;
            // 
            // runBox
            // 
            this.runBox.FormattingEnabled = true;
            this.runBox.Location = new System.Drawing.Point( 120, 9 );
            this.runBox.Name = "runBox";
            this.runBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.runBox.Size = new System.Drawing.Size( 186, 21 );
            this.runBox.TabIndex = 11;
            this.runBox.SelectedIndexChanged += new System.EventHandler( this.runBox_SelectedIndexChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 27, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 75, 13 );
            this.label1.TabIndex = 12;
            this.label1.Text = "Select Results";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.setUpPage );
            this.tabControl1.Controls.Add( this.volPage );
            this.tabControl1.Controls.Add( this.pricePage );
            this.tabControl1.Controls.Add( this.attrTab );
            this.tabControl1.Controls.Add( this.mediaPage );
            this.tabControl1.Controls.Add( this.parmPage );
            this.tabControl1.Controls.Add( this.varPage );
            this.tabControl1.Controls.Add( this.plotPage );
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 982, 560 );
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler( this.tabControl1_SelectedIndexChanged );
            // 
            // setUpPage
            // 
            this.setUpPage.Controls.Add( this.splitContainer1 );
            this.setUpPage.Location = new System.Drawing.Point( 4, 22 );
            this.setUpPage.Name = "setUpPage";
            this.setUpPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.setUpPage.Size = new System.Drawing.Size( 974, 534 );
            this.setUpPage.TabIndex = 0;
            this.setUpPage.Text = "Calibration Set Up";
            this.setUpPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point( 3, 3 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.simulationSetUpControl1 );
            this.splitContainer1.Panel1MinSize = 313;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.calibrationSetUp1 );
            this.splitContainer1.Size = new System.Drawing.Size( 968, 528 );
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.TabIndex = 1;
            // 
            // attrTab
            // 
            this.attrTab.Controls.Add( this.attributeCalibration1 );
            this.attrTab.Location = new System.Drawing.Point( 4, 22 );
            this.attrTab.Name = "attrTab";
            this.attrTab.Padding = new System.Windows.Forms.Padding( 3 );
            this.attrTab.Size = new System.Drawing.Size( 974, 534 );
            this.attrTab.TabIndex = 5;
            this.attrTab.Text = "Attributes";
            this.attrTab.UseVisualStyleBackColor = true;
            // 
            // parmPage
            // 
            this.parmPage.Controls.Add( this.parameterControl1 );
            this.parmPage.Location = new System.Drawing.Point( 4, 22 );
            this.parmPage.Name = "parmPage";
            this.parmPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.parmPage.Size = new System.Drawing.Size( 974, 534 );
            this.parmPage.TabIndex = 1;
            this.parmPage.Text = "Parameters";
            this.parmPage.UseVisualStyleBackColor = true;
            // 
            // varPage
            // 
            this.varPage.Controls.Add( this.variableControl1 );
            this.varPage.Location = new System.Drawing.Point( 4, 22 );
            this.varPage.Name = "varPage";
            this.varPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.varPage.Size = new System.Drawing.Size( 974, 534 );
            this.varPage.TabIndex = 2;
            this.varPage.Text = "Variables";
            this.varPage.UseVisualStyleBackColor = true;
            // 
            // volPage
            // 
            this.volPage.Controls.Add( this.volumeCalibration1 );
            this.volPage.Location = new System.Drawing.Point( 4, 22 );
            this.volPage.Name = "volPage";
            this.volPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.volPage.Size = new System.Drawing.Size( 974, 534 );
            this.volPage.TabIndex = 4;
            this.volPage.Text = "Volume";
            this.volPage.UseVisualStyleBackColor = true;
            // 
            // pricePage
            // 
            this.pricePage.Controls.Add( this.priceCalibration1 );
            this.pricePage.Location = new System.Drawing.Point( 4, 22 );
            this.pricePage.Name = "pricePage";
            this.pricePage.Padding = new System.Windows.Forms.Padding( 3 );
            this.pricePage.Size = new System.Drawing.Size( 974, 534 );
            this.pricePage.TabIndex = 6;
            this.pricePage.Text = "Price";
            this.pricePage.UseVisualStyleBackColor = true;
            // 
            // plotPage
            // 
            this.plotPage.Controls.Add( this.errorPlot );
            this.plotPage.Location = new System.Drawing.Point( 4, 22 );
            this.plotPage.Name = "plotPage";
            this.plotPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.plotPage.Size = new System.Drawing.Size( 974, 534 );
            this.plotPage.TabIndex = 3;
            this.plotPage.Text = "Error Chart";
            this.plotPage.UseVisualStyleBackColor = true;
            // 
            // simulationSetUpControl1
            // 
            this.simulationSetUpControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulationSetUpControl1.Location = new System.Drawing.Point( 0, 0 );
            this.simulationSetUpControl1.Name = "simulationSetUpControl1";
            this.simulationSetUpControl1.Size = new System.Drawing.Size( 313, 528 );
            this.simulationSetUpControl1.TabIndex = 0;
            // 
            // calibrationSetUp1
            // 
            this.calibrationSetUp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationSetUp1.Location = new System.Drawing.Point( 0, 0 );
            this.calibrationSetUp1.Name = "calibrationSetUp1";
            this.calibrationSetUp1.Size = new System.Drawing.Size( 651, 528 );
            this.calibrationSetUp1.TabIndex = 0;
            // 
            // attributeCalibration1
            // 
            this.attributeCalibration1.Db = null;
            this.attributeCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeCalibration1.Location = new System.Drawing.Point( 3, 3 );
            this.attributeCalibration1.Name = "attributeCalibration1";
            this.attributeCalibration1.Run = -1;
            this.attributeCalibration1.Size = new System.Drawing.Size( 968, 528 );
            this.attributeCalibration1.TabIndex = 0;
            // 
            // parameterControl1
            // 
            this.parameterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameterControl1.Location = new System.Drawing.Point( 3, 3 );
            this.parameterControl1.Name = "parameterControl1";
            this.parameterControl1.Size = new System.Drawing.Size( 968, 528 );
            this.parameterControl1.TabIndex = 0;
            // 
            // variableControl1
            // 
            this.variableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableControl1.Location = new System.Drawing.Point( 3, 3 );
            this.variableControl1.Name = "variableControl1";
            this.variableControl1.Size = new System.Drawing.Size( 968, 528 );
            this.variableControl1.TabIndex = 0;
            // 
            // volumeCalibration1
            // 
            this.volumeCalibration1.Db = null;
            this.volumeCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumeCalibration1.Location = new System.Drawing.Point( 3, 3 );
            this.volumeCalibration1.Name = "volumeCalibration1";
            this.volumeCalibration1.Run = -1;
            this.volumeCalibration1.Size = new System.Drawing.Size( 968, 528 );
            this.volumeCalibration1.TabIndex = 0;
            // 
            // priceCalibration1
            // 
            this.priceCalibration1.Db = null;
            this.priceCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priceCalibration1.Location = new System.Drawing.Point( 3, 3 );
            this.priceCalibration1.Name = "priceCalibration1";
            this.priceCalibration1.Run = -1;
            this.priceCalibration1.Size = new System.Drawing.Size( 968, 528 );
            this.priceCalibration1.TabIndex = 0;
            // 
            // errorPlot
            // 
            this.errorPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorPlot.End = new System.DateTime( 1899, 12, 31, 4, 48, 0, 0 );
            this.errorPlot.Location = new System.Drawing.Point( 3, 3 );
            this.errorPlot.Max = 1.2;
            this.errorPlot.MaxX = 1.2;
            this.errorPlot.Min = 0;
            this.errorPlot.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.errorPlot.MinX = 0;
            this.errorPlot.Name = "errorPlot";
            this.errorPlot.PercentMax = 100;
            this.errorPlot.PercentMin = 0;
            this.errorPlot.ScatterPlot = false;
            this.errorPlot.Size = new System.Drawing.Size( 968, 528 );
            this.errorPlot.Start = new System.DateTime( 1899, 12, 30, 0, 0, 0, 0 );
            this.errorPlot.TabIndex = 0;
            this.errorPlot.TimeSeries = false;
            this.errorPlot.Title = "";
            this.errorPlot.XAxis = "";
            this.errorPlot.Y2Axis = "Percent";
            this.errorPlot.YAxis = "Error";
            // 
            // mediaPage
            // 
            this.mediaPage.Controls.Add( this.mediaCalibration1 );
            this.mediaPage.Location = new System.Drawing.Point( 4, 22 );
            this.mediaPage.Name = "mediaPage";
            this.mediaPage.Size = new System.Drawing.Size( 974, 534 );
            this.mediaPage.TabIndex = 7;
            this.mediaPage.Text = "Media";
            this.mediaPage.UseVisualStyleBackColor = true;
            // 
            // mediaCalibration1
            // 
            this.mediaCalibration1.Db = null;
            this.mediaCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaCalibration1.Location = new System.Drawing.Point( 0, 0 );
            this.mediaCalibration1.Name = "mediaCalibration1";
            this.mediaCalibration1.Run = -1;
            this.mediaCalibration1.Size = new System.Drawing.Size( 974, 534 );
            this.mediaCalibration1.TabIndex = 0;
            // 
            // CalibrationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 982, 664 );
            this.ControlBox = false;
            this.Controls.Add( this.splitContainer2 );
            this.Controls.Add( this.panel1 );
            this.MinimumSize = new System.Drawing.Size( 766, 614 );
            this.Name = "CalibrationEditor";
            this.Text = "CalibrationEditor";
            this.panel1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.tabControl1.ResumeLayout( false );
            this.setUpPage.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.attrTab.ResumeLayout( false );
            this.parmPage.ResumeLayout( false );
            this.varPage.ResumeLayout( false );
            this.volPage.ResumeLayout( false );
            this.pricePage.ResumeLayout( false );
            this.plotPage.ResumeLayout( false );
            this.mediaPage.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private SimulationSetUpControl simulationSetUpControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage setUpPage;
        private System.Windows.Forms.TabPage parmPage;
        private ParameterControl parameterControl1;
        private System.Windows.Forms.TabPage varPage;
        private VariableControl variableControl1;
        private CalibrationSetUp calibrationSetUp1;
        private System.Windows.Forms.TabPage plotPage;
        private Utilities.Graphing.PlotControl errorPlot;
        private System.Windows.Forms.TabPage volPage;
        private MrktSimClient.Controls.Dialogs.Calibration.VolumeCalibration volumeCalibration1;
        private System.Windows.Forms.TabPage attrTab;
        private MrktSimClient.Controls.Dialogs.Calibration.AttributeCalibration attributeCalibration1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox runBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage pricePage;
        private MrktSimClient.Controls.Dialogs.Calibration.PriceCalibration priceCalibration1;
        private System.Windows.Forms.TabPage mediaPage;
        private MrktSimClient.Controls.Dialogs.Calibration.MediaCalibration mediaCalibration1;

    }
}