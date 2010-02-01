namespace MrktSimClient.Controls.Dialogs
{
    partial class SimulationEditor
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homePage = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.simulationSetUpControl1 = new MrktSimClient.Controls.Dialogs.SimulationSetUpControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.seedGrid = new MarketSimUtilities.MrktSimGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.addSeedButton = new System.Windows.Forms.Button();
            this.metricTree = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.deleteResults = new System.Windows.Forms.CheckBox();
            this.parameterpage = new System.Windows.Forms.TabPage();
            this.parameterControl1 = new MrktSimClient.Controls.Dialogs.ParameterControl();
            this.variablePage = new System.Windows.Forms.TabPage();
            this.variableControl1 = new MrktSimClient.Controls.Dialogs.VariableControl();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.homePage.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.parameterpage.SuspendLayout();
            this.variablePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cancelButton);
            this.splitContainer1.Panel2.Controls.Add(this.acceptButton);
            this.splitContainer1.Size = new System.Drawing.Size(899, 520);
            this.splitContainer1.SplitterDistance = 468;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.homePage);
            this.tabControl.Controls.Add(this.parameterpage);
            this.tabControl.Controls.Add(this.variablePage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(899, 468);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // homePage
            // 
            this.homePage.Controls.Add(this.splitContainer2);
            this.homePage.Location = new System.Drawing.Point(4, 22);
            this.homePage.Name = "homePage";
            this.homePage.Padding = new System.Windows.Forms.Padding(3);
            this.homePage.Size = new System.Drawing.Size(891, 442);
            this.homePage.TabIndex = 0;
            this.homePage.Text = "Simulation";
            this.homePage.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.simulationSetUpControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(885, 436);
            this.splitContainer2.SplitterDistance = 331;
            this.splitContainer2.TabIndex = 1;
            // 
            // simulationSetUpControl1
            // 
            this.simulationSetUpControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulationSetUpControl1.Location = new System.Drawing.Point(0, 0);
            this.simulationSetUpControl1.Name = "simulationSetUpControl1";
            this.simulationSetUpControl1.Size = new System.Drawing.Size(331, 436);
            this.simulationSetUpControl1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.seedGrid);
            this.splitContainer3.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.metricTree);
            this.splitContainer3.Panel2.Controls.Add(this.panel2);
            this.splitContainer3.Size = new System.Drawing.Size(550, 436);
            this.splitContainer3.SplitterDistance = 210;
            this.splitContainer3.TabIndex = 0;
            // 
            // seedGrid
            // 
            this.seedGrid.DescribeRow = null;
            this.seedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seedGrid.EnabledGrid = true;
            this.seedGrid.Location = new System.Drawing.Point(0, 41);
            this.seedGrid.Name = "seedGrid";
            this.seedGrid.RowFilter = null;
            this.seedGrid.RowID = null;
            this.seedGrid.RowName = null;
            this.seedGrid.Size = new System.Drawing.Size(210, 395);
            this.seedGrid.Sort = "";
            this.seedGrid.TabIndex = 3;
            this.seedGrid.Table = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.addSeedButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 41);
            this.panel1.TabIndex = 2;
            // 
            // addSeedButton
            // 
            this.addSeedButton.Location = new System.Drawing.Point(45, 10);
            this.addSeedButton.Name = "addSeedButton";
            this.addSeedButton.Size = new System.Drawing.Size(75, 23);
            this.addSeedButton.TabIndex = 0;
            this.addSeedButton.Text = "Add Seeds";
            this.addSeedButton.UseVisualStyleBackColor = true;
            this.addSeedButton.Click += new System.EventHandler(this.addSeedButton_Click);
            // 
            // metricTree
            // 
            this.metricTree.CheckBoxes = true;
            this.metricTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricTree.Location = new System.Drawing.Point(0, 41);
            this.metricTree.Name = "metricTree";
            this.metricTree.Size = new System.Drawing.Size(336, 395);
            this.metricTree.TabIndex = 1;
            this.metricTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.metricTree_AfterCheck);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.deleteResults);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(336, 41);
            this.panel2.TabIndex = 0;
            // 
            // deleteResults
            // 
            this.deleteResults.AutoSize = true;
            this.deleteResults.Location = new System.Drawing.Point(36, 12);
            this.deleteResults.Name = "deleteResults";
            this.deleteResults.Size = new System.Drawing.Size(141, 17);
            this.deleteResults.TabIndex = 31;
            this.deleteResults.Text = "Delete Time Series Data";
            this.deleteResults.UseVisualStyleBackColor = true;
            // 
            // parameterpage
            // 
            this.parameterpage.Controls.Add(this.parameterControl1);
            this.parameterpage.Location = new System.Drawing.Point(4, 22);
            this.parameterpage.Name = "parameterpage";
            this.parameterpage.Padding = new System.Windows.Forms.Padding(3);
            this.parameterpage.Size = new System.Drawing.Size(891, 442);
            this.parameterpage.TabIndex = 1;
            this.parameterpage.Text = "Parameters";
            this.parameterpage.UseVisualStyleBackColor = true;
            // 
            // parameterControl1
            // 
            this.parameterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameterControl1.Location = new System.Drawing.Point(3, 3);
            this.parameterControl1.Name = "parameterControl1";
            this.parameterControl1.Size = new System.Drawing.Size(885, 436);
            this.parameterControl1.TabIndex = 0;
            // 
            // variablePage
            // 
            this.variablePage.Controls.Add(this.variableControl1);
            this.variablePage.Location = new System.Drawing.Point(4, 22);
            this.variablePage.Name = "variablePage";
            this.variablePage.Padding = new System.Windows.Forms.Padding(3);
            this.variablePage.Size = new System.Drawing.Size(891, 442);
            this.variablePage.TabIndex = 2;
            this.variablePage.Text = "Variables";
            this.variablePage.UseVisualStyleBackColor = true;
            // 
            // variableControl1
            // 
            this.variableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableControl1.Location = new System.Drawing.Point(3, 3);
            this.variableControl1.Name = "variableControl1";
            this.variableControl1.Size = new System.Drawing.Size(885, 436);
            this.variableControl1.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(682, 13);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(787, 13);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // SimulationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 520);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(721, 475);
            this.Name = "SimulationEditor";
            this.Text = "SimulationEditor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.homePage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.parameterpage.ResumeLayout(false);
            this.variablePage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage homePage;
        private System.Windows.Forms.TabPage parameterpage;
        private System.Windows.Forms.TabPage variablePage;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private ParameterControl parameterControl1;
        private VariableControl variableControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button addSeedButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView metricTree;
        private SimulationSetUpControl simulationSetUpControl1;
        private System.Windows.Forms.CheckBox deleteResults;
        private MarketSimUtilities.MrktSimGrid seedGrid;
    }
}