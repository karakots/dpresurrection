namespace AdPlanitSimController
{
    partial class AdPlanitSimController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AdPlanitSimController ) );
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loadMediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAgentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SimulationSettingsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTemplateFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.simulationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startNewSimToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startNewCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulationParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrateAdOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreUserBox = new System.Windows.Forms.ToolStripTextBox();
            this.editCalibratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simGrid = new System.Windows.Forms.DataGridView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.mediaStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.agentStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.requeueButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simGrid)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem,
            this.simulationsToolStripMenuItem,
            this.ignoreUserBox,
            this.editCalibratorToolStripMenuItem} );
            this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size( 570, 24 );
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.loadAllItem,
            this.toolStripSeparator1,
            this.loadMediaToolStripMenuItem,
            this.loadAgentsToolStripMenuItem,
            this.SimulationSettingsItem,
            this.toolStripSeparator2} );
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size( 42, 20 );
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // loadAllItem
            // 
            this.loadAllItem.Name = "loadAllItem";
            this.loadAllItem.Size = new System.Drawing.Size( 166, 22 );
            this.loadAllItem.Text = "Load All";
            this.loadAllItem.Click += new System.EventHandler( this.loadAllItem_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 163, 6 );
            // 
            // loadMediaToolStripMenuItem
            // 
            this.loadMediaToolStripMenuItem.Name = "loadMediaToolStripMenuItem";
            this.loadMediaToolStripMenuItem.Size = new System.Drawing.Size( 166, 22 );
            this.loadMediaToolStripMenuItem.Text = "Load Media";
            this.loadMediaToolStripMenuItem.Click += new System.EventHandler( this.loadMediaToolStripMenuItem_Click );
            // 
            // loadAgentsToolStripMenuItem
            // 
            this.loadAgentsToolStripMenuItem.Name = "loadAgentsToolStripMenuItem";
            this.loadAgentsToolStripMenuItem.Size = new System.Drawing.Size( 166, 22 );
            this.loadAgentsToolStripMenuItem.Text = "Load Agents";
            this.loadAgentsToolStripMenuItem.Click += new System.EventHandler( this.loadAgentsToolStripMenuItem_Click );
            // 
            // SimulationSettingsItem
            // 
            this.SimulationSettingsItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.loadSettingsToolStripMenuItem,
            this.createTemplateFileToolStripMenuItem} );
            this.SimulationSettingsItem.Name = "SimulationSettingsItem";
            this.SimulationSettingsItem.Size = new System.Drawing.Size( 166, 22 );
            this.SimulationSettingsItem.Text = "Simulation Settings";
            // 
            // loadSettingsToolStripMenuItem
            // 
            this.loadSettingsToolStripMenuItem.Name = "loadSettingsToolStripMenuItem";
            this.loadSettingsToolStripMenuItem.Size = new System.Drawing.Size( 102, 22 );
            this.loadSettingsToolStripMenuItem.Text = "Load";
            this.loadSettingsToolStripMenuItem.Click += new System.EventHandler( this.loadSimulationSettingsItem_Click );
            // 
            // createTemplateFileToolStripMenuItem
            // 
            this.createTemplateFileToolStripMenuItem.Name = "createTemplateFileToolStripMenuItem";
            this.createTemplateFileToolStripMenuItem.Size = new System.Drawing.Size( 102, 22 );
            this.createTemplateFileToolStripMenuItem.Text = "Save";
            this.createTemplateFileToolStripMenuItem.Click += new System.EventHandler( this.createTemplateFileToolStripMenuItem_Click );
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 163, 6 );
            // 
            // simulationsToolStripMenuItem
            // 
            this.simulationsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.startNewSimToolStripMenuItem,
            this.startNewCalibrationToolStripMenuItem,
            this.arrangeToolStripMenuItem,
            this.simulationParametersToolStripMenuItem,
            this.calibrateAdOptionsToolStripMenuItem} );
            this.simulationsToolStripMenuItem.Name = "simulationsToolStripMenuItem";
            this.simulationsToolStripMenuItem.Size = new System.Drawing.Size( 72, 20 );
            this.simulationsToolStripMenuItem.Text = "Simulations";
            // 
            // startNewSimToolStripMenuItem
            // 
            this.startNewSimToolStripMenuItem.Name = "startNewSimToolStripMenuItem";
            this.startNewSimToolStripMenuItem.Size = new System.Drawing.Size( 181, 22 );
            this.startNewSimToolStripMenuItem.Text = "Start New Sim";
            this.startNewSimToolStripMenuItem.Click += new System.EventHandler( this.startNewSimToolStripMenuItem_Click );
            // 
            // startNewCalibrationToolStripMenuItem
            // 
            this.startNewCalibrationToolStripMenuItem.Name = "startNewCalibrationToolStripMenuItem";
            this.startNewCalibrationToolStripMenuItem.Size = new System.Drawing.Size( 181, 22 );
            this.startNewCalibrationToolStripMenuItem.Text = "Local Test Simulation";
            this.startNewCalibrationToolStripMenuItem.Click += new System.EventHandler( this.startNewCalibrationToolStripMenuItem_Click );
            // 
            // arrangeToolStripMenuItem
            // 
            this.arrangeToolStripMenuItem.Name = "arrangeToolStripMenuItem";
            this.arrangeToolStripMenuItem.Size = new System.Drawing.Size( 181, 22 );
            this.arrangeToolStripMenuItem.Text = "Arrange";
            this.arrangeToolStripMenuItem.Click += new System.EventHandler( this.arrangeToolStripMenuItem_Click );
            // 
            // simulationParametersToolStripMenuItem
            // 
            this.simulationParametersToolStripMenuItem.Name = "simulationParametersToolStripMenuItem";
            this.simulationParametersToolStripMenuItem.Size = new System.Drawing.Size( 181, 22 );
            this.simulationParametersToolStripMenuItem.Text = "Simulation Parameters";
            this.simulationParametersToolStripMenuItem.Click += new System.EventHandler( this.simGlobalsMenu_Click );
            // 
            // calibrateAdOptionsToolStripMenuItem
            // 
            this.calibrateAdOptionsToolStripMenuItem.Name = "calibrateAdOptionsToolStripMenuItem";
            this.calibrateAdOptionsToolStripMenuItem.Size = new System.Drawing.Size( 181, 22 );
            this.calibrateAdOptionsToolStripMenuItem.Text = "Calibrate Ad Options";
            this.calibrateAdOptionsToolStripMenuItem.Click += new System.EventHandler( this.calibrateAdOptionsToolStripMenuItem_Click );
            // 
            // ignoreUserBox
            // 
            this.ignoreUserBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ignoreUserBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.ignoreUserBox.BackColor = System.Drawing.Color.LightGreen;
            this.ignoreUserBox.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.ignoreUserBox.Name = "ignoreUserBox";
            this.ignoreUserBox.ReadOnly = true;
            this.ignoreUserBox.Size = new System.Drawing.Size( 200, 20 );
            this.ignoreUserBox.Text = "?";
            // 
            // editCalibratorToolStripMenuItem
            // 
            this.editCalibratorToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.editCalibratorToolStripMenuItem.AutoToolTip = true;
            this.editCalibratorToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.editCalibratorToolStripMenuItem.Name = "editCalibratorToolStripMenuItem";
            this.editCalibratorToolStripMenuItem.Size = new System.Drawing.Size( 46, 20 );
            this.editCalibratorToolStripMenuItem.Text = "Edit >";
            this.editCalibratorToolStripMenuItem.Click += new System.EventHandler( this.editCalibratorToolStripMenuItem_Click );
            // 
            // simGrid
            // 
            this.simGrid.AllowUserToAddRows = false;
            this.simGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.simGrid.Location = new System.Drawing.Point( 0, 184 );
            this.simGrid.Name = "simGrid";
            this.simGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simGrid.Size = new System.Drawing.Size( 570, 181 );
            this.simGrid.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mediaStatus,
            this.agentStatus,
            this.toolStripStatusLabel1,
            this.requeueButton} );
            this.statusStrip.Location = new System.Drawing.Point( 0, 159 );
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size( 570, 22 );
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // mediaStatus
            // 
            this.mediaStatus.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.mediaStatus.Name = "mediaStatus";
            this.mediaStatus.Size = new System.Drawing.Size( 95, 17 );
            this.mediaStatus.Text = "99999 media";
            // 
            // agentStatus
            // 
            this.agentStatus.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.agentStatus.Name = "agentStatus";
            this.agentStatus.Size = new System.Drawing.Size( 115, 17 );
            this.agentStatus.Text = "9999999 agents";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size( 13, 17 );
            this.toolStripStatusLabel1.Text = "::";
            // 
            // requeueButton
            // 
            this.requeueButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.requeueButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.requeueButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.requeueButton.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.requeueButton.ForeColor = System.Drawing.Color.Red;
            this.requeueButton.Image = ((System.Drawing.Image)(resources.GetObject( "requeueButton.Image" )));
            this.requeueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.requeueButton.Name = "requeueButton";
            this.requeueButton.ShowDropDownArrow = false;
            this.requeueButton.Size = new System.Drawing.Size( 62, 20 );
            this.requeueButton.Text = "Requeue";
            this.requeueButton.ToolTipText = "Set status to zero";
            this.requeueButton.Click += new System.EventHandler( this.requeueButton_Click );
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point( 0, 181 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 570, 3 );
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // AdPlanitSimController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size( 570, 365 );
            this.Controls.Add( this.statusStrip );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.menuStrip1 );
            this.Controls.Add( this.simGrid );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.IsMdiContainer = true;
            this.Name = "AdPlanitSimController";
            this.Text = "datbase :: app name";
            this.menuStrip1.ResumeLayout( false );
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simGrid)).EndInit();
            this.statusStrip.ResumeLayout( false );
            this.statusStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView simGrid;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMediaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAgentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startNewSimToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAllItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem SimulationSettingsItem;
        private System.Windows.Forms.ToolStripMenuItem loadSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTemplateFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel mediaStatus;
        private System.Windows.Forms.ToolStripStatusLabel agentStatus;
        private System.Windows.Forms.ToolStripTextBox ignoreUserBox;
        private System.Windows.Forms.ToolStripMenuItem editCalibratorToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripMenuItem startNewCalibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulationParametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton requeueButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem calibrateAdOptionsToolStripMenuItem;
    }
}

