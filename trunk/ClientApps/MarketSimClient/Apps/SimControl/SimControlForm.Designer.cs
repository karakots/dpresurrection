namespace SimControl
{
    partial class SimControlForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SimControlForm ) );
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.simBox = new System.Windows.Forms.GroupBox();
            this.simDataGridView = new System.Windows.Forms.DataGridView();
            this.SimulationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDatCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResetPanelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sim_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.runGrpBox = new System.Windows.Forms.GroupBox();
            this.runningGrid = new System.Windows.Forms.DataGridView();
            this.ResultsSimulationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsNameColCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsStatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentStatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.processedGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusImage = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.numQueuedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.numRunninglabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.settingsButton = new System.Windows.Forms.ToolStripButton();
            this.stopSimsButton = new System.Windows.Forms.ToolStripButton();
            this.runStateImages = new System.Windows.Forms.ImageList( this.components );
            this.unlockButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.simBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simDataGridView)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.runGrpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runningGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processedGrid)).BeginInit();
            this.statusBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 25 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.simBox );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Size = new System.Drawing.Size( 702, 408 );
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 1;
            // 
            // simBox
            // 
            this.simBox.Controls.Add( this.simDataGridView );
            this.simBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simBox.Location = new System.Drawing.Point( 0, 0 );
            this.simBox.Name = "simBox";
            this.simBox.Size = new System.Drawing.Size( 702, 112 );
            this.simBox.TabIndex = 6;
            this.simBox.TabStop = false;
            this.simBox.Text = "Queued or Running Simulations";
            // 
            // simDataGridView
            // 
            this.simDataGridView.AllowUserToAddRows = false;
            this.simDataGridView.AllowUserToDeleteRows = false;
            this.simDataGridView.AllowUserToResizeRows = false;
            this.simDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.simDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.simDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.simDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.SimulationIdCol,
            this.ScenarioIdCol,
            this.NameCol,
            this.StatusCol,
            this.TypeCol,
            this.StartDatCol,
            this.EndDateCol,
            this.ResetPanelCol,
            this.sim_num} );
            this.simDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simDataGridView.Location = new System.Drawing.Point( 3, 16 );
            this.simDataGridView.Name = "simDataGridView";
            this.simDataGridView.ReadOnly = true;
            this.simDataGridView.RowHeadersVisible = false;
            this.simDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.simDataGridView.RowTemplate.Height = 16;
            this.simDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simDataGridView.Size = new System.Drawing.Size( 696, 93 );
            this.simDataGridView.TabIndex = 5;
            // 
            // SimulationIdCol
            // 
            this.SimulationIdCol.DataPropertyName = "simulation_id";
            this.SimulationIdCol.HeaderText = "simulation_id";
            this.SimulationIdCol.Name = "SimulationIdCol";
            this.SimulationIdCol.ReadOnly = true;
            this.SimulationIdCol.Visible = false;
            this.SimulationIdCol.Width = 10;
            // 
            // ScenarioIdCol
            // 
            this.ScenarioIdCol.DataPropertyName = "scenario_id";
            this.ScenarioIdCol.HeaderText = "scenario_id";
            this.ScenarioIdCol.Name = "ScenarioIdCol";
            this.ScenarioIdCol.ReadOnly = true;
            this.ScenarioIdCol.Visible = false;
            this.ScenarioIdCol.Width = 10;
            // 
            // NameCol
            // 
            this.NameCol.DataPropertyName = "name";
            this.NameCol.HeaderText = "Name";
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            this.NameCol.Width = 170;
            // 
            // StatusCol
            // 
            this.StatusCol.DataPropertyName = "status";
            this.StatusCol.HeaderText = "Status";
            this.StatusCol.Name = "StatusCol";
            this.StatusCol.ReadOnly = true;
            this.StatusCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StatusCol.Width = 60;
            // 
            // TypeCol
            // 
            this.TypeCol.DataPropertyName = "type";
            this.TypeCol.HeaderText = "Type";
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.ReadOnly = true;
            // 
            // StartDatCol
            // 
            this.StartDatCol.DataPropertyName = "start_date";
            this.StartDatCol.HeaderText = "Start";
            this.StartDatCol.Name = "StartDatCol";
            this.StartDatCol.ReadOnly = true;
            this.StartDatCol.Width = 80;
            // 
            // EndDateCol
            // 
            this.EndDateCol.DataPropertyName = "end_date";
            this.EndDateCol.HeaderText = "End";
            this.EndDateCol.Name = "EndDateCol";
            this.EndDateCol.ReadOnly = true;
            this.EndDateCol.Width = 80;
            // 
            // ResetPanelCol
            // 
            this.ResetPanelCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ResetPanelCol.DataPropertyName = "reset_panel_state";
            this.ResetPanelCol.HeaderText = "Reset Panel";
            this.ResetPanelCol.Name = "ResetPanelCol";
            this.ResetPanelCol.ReadOnly = true;
            // 
            // sim_num
            // 
            this.sim_num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sim_num.DataPropertyName = "sim_num";
            this.sim_num.HeaderText = "STATE";
            this.sim_num.Name = "sim_num";
            this.sim_num.ReadOnly = true;
            this.sim_num.Visible = false;
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
            this.splitContainer2.Panel1.Controls.Add( this.runGrpBox );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.groupBox1 );
            this.splitContainer2.Size = new System.Drawing.Size( 702, 292 );
            this.splitContainer2.SplitterDistance = 141;
            this.splitContainer2.TabIndex = 0;
            // 
            // runGrpBox
            // 
            this.runGrpBox.Controls.Add( this.runningGrid );
            this.runGrpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runGrpBox.Location = new System.Drawing.Point( 0, 0 );
            this.runGrpBox.Name = "runGrpBox";
            this.runGrpBox.Size = new System.Drawing.Size( 702, 141 );
            this.runGrpBox.TabIndex = 1;
            this.runGrpBox.TabStop = false;
            this.runGrpBox.Text = "Running Simulations";
            // 
            // runningGrid
            // 
            this.runningGrid.AllowUserToAddRows = false;
            this.runningGrid.AllowUserToDeleteRows = false;
            this.runningGrid.AllowUserToResizeRows = false;
            this.runningGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.runningGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.runningGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.runningGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.runningGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.runningGrid.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.ResultsSimulationIdCol,
            this.ResultsNameColCol,
            this.RunCol,
            this.ResultsStatusCol,
            this.CurrentStatusCol,
            this.CurrentDateCol,
            this.RunIdCol,
            this.ResultsScenarioIdCol,
            this.StartedCol,
            this.RunTimeCol} );
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.runningGrid.DefaultCellStyle = dataGridViewCellStyle14;
            this.runningGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runningGrid.Location = new System.Drawing.Point( 3, 16 );
            this.runningGrid.Name = "runningGrid";
            this.runningGrid.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.runningGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.runningGrid.RowHeadersVisible = false;
            this.runningGrid.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.runningGrid.RowTemplate.Height = 16;
            this.runningGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.runningGrid.Size = new System.Drawing.Size( 696, 122 );
            this.runningGrid.TabIndex = 6;
            // 
            // ResultsSimulationIdCol
            // 
            this.ResultsSimulationIdCol.DataPropertyName = "simulation_id";
            this.ResultsSimulationIdCol.HeaderText = "simulation_id";
            this.ResultsSimulationIdCol.Name = "ResultsSimulationIdCol";
            this.ResultsSimulationIdCol.ReadOnly = true;
            this.ResultsSimulationIdCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ResultsSimulationIdCol.Visible = false;
            this.ResultsSimulationIdCol.Width = 80;
            // 
            // ResultsNameColCol
            // 
            this.ResultsNameColCol.DataPropertyName = "name";
            this.ResultsNameColCol.HeaderText = "Name";
            this.ResultsNameColCol.Name = "ResultsNameColCol";
            this.ResultsNameColCol.ReadOnly = true;
            this.ResultsNameColCol.Width = 170;
            // 
            // RunCol
            // 
            this.RunCol.DataPropertyName = "run_name";
            this.RunCol.HeaderText = "Run";
            this.RunCol.Name = "RunCol";
            this.RunCol.ReadOnly = true;
            this.RunCol.Width = 60;
            // 
            // ResultsStatusCol
            // 
            this.ResultsStatusCol.DataPropertyName = "status";
            this.ResultsStatusCol.HeaderText = "Status";
            this.ResultsStatusCol.Name = "ResultsStatusCol";
            this.ResultsStatusCol.ReadOnly = true;
            this.ResultsStatusCol.Width = 85;
            // 
            // CurrentStatusCol
            // 
            this.CurrentStatusCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurrentStatusCol.DataPropertyName = "current_status";
            this.CurrentStatusCol.HeaderText = "Progress";
            this.CurrentStatusCol.Name = "CurrentStatusCol";
            this.CurrentStatusCol.ReadOnly = true;
            // 
            // CurrentDateCol
            // 
            this.CurrentDateCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurrentDateCol.DataPropertyName = "current_date";
            this.CurrentDateCol.HeaderText = "Date";
            this.CurrentDateCol.Name = "CurrentDateCol";
            this.CurrentDateCol.ReadOnly = true;
            this.CurrentDateCol.Visible = false;
            // 
            // RunIdCol
            // 
            this.RunIdCol.DataPropertyName = "run_id";
            this.RunIdCol.HeaderText = "run_id";
            this.RunIdCol.Name = "RunIdCol";
            this.RunIdCol.ReadOnly = true;
            this.RunIdCol.Visible = false;
            // 
            // ResultsScenarioIdCol
            // 
            this.ResultsScenarioIdCol.DataPropertyName = "scenario_id";
            this.ResultsScenarioIdCol.HeaderText = "scenario_id";
            this.ResultsScenarioIdCol.Name = "ResultsScenarioIdCol";
            this.ResultsScenarioIdCol.ReadOnly = true;
            this.ResultsScenarioIdCol.Visible = false;
            // 
            // StartedCol
            // 
            this.StartedCol.DataPropertyName = "run_time";
            this.StartedCol.HeaderText = "Start";
            this.StartedCol.Name = "StartedCol";
            this.StartedCol.ReadOnly = true;
            this.StartedCol.Width = 130;
            // 
            // RunTimeCol
            // 
            this.RunTimeCol.DataPropertyName = "elapsed_time";
            this.RunTimeCol.HeaderText = "Elapsed Time";
            this.RunTimeCol.Name = "RunTimeCol";
            this.RunTimeCol.ReadOnly = true;
            this.RunTimeCol.Width = 101;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.processedGrid );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 702, 147 );
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processed Simulations";
            // 
            // processedGrid
            // 
            this.processedGrid.AllowUserToAddRows = false;
            this.processedGrid.AllowUserToDeleteRows = false;
            this.processedGrid.AllowUserToResizeRows = false;
            this.processedGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.processedGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.processedGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.processedGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.processedGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.processedGrid.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10} );
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.processedGrid.DefaultCellStyle = dataGridViewCellStyle17;
            this.processedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processedGrid.Location = new System.Drawing.Point( 3, 16 );
            this.processedGrid.Name = "processedGrid";
            this.processedGrid.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.processedGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.processedGrid.RowHeadersVisible = false;
            this.processedGrid.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.processedGrid.RowTemplate.Height = 16;
            this.processedGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.processedGrid.Size = new System.Drawing.Size( 696, 128 );
            this.processedGrid.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "simulation_id";
            this.dataGridViewTextBoxColumn1.HeaderText = "simulation_id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 170;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "run_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Run";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "status";
            this.dataGridViewTextBoxColumn4.HeaderText = "Status";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 85;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "current_status";
            this.dataGridViewTextBoxColumn5.HeaderText = "Progress";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "current_date";
            this.dataGridViewTextBoxColumn6.HeaderText = "Date";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "run_id";
            this.dataGridViewTextBoxColumn7.HeaderText = "run_id";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "scenario_id";
            this.dataGridViewTextBoxColumn8.HeaderText = "scenario_id";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "run_time";
            this.dataGridViewTextBoxColumn9.HeaderText = "Start";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 130;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "elapsed_time";
            this.dataGridViewTextBoxColumn10.HeaderText = "Elapsed Time";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 101;
            // 
            // timer
            // 
            this.timer.Interval = 10000;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusImage,
            this.StatusLabel,
            this.numQueuedLabel,
            this.numRunninglabel} );
            this.statusBar.Location = new System.Drawing.Point( 0, 433 );
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size( 702, 25 );
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusImage
            // 
            this.statusImage.Image = global::SimControl.Properties.Resources.run2;
            this.statusImage.Name = "statusImage";
            this.statusImage.Size = new System.Drawing.Size( 16, 20 );
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = false;
            this.StatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size( 256, 20 );
            this.StatusLabel.Text = "Click on settings button to configure";
            // 
            // numQueuedLabel
            // 
            this.numQueuedLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.numQueuedLabel.Name = "numQueuedLabel";
            this.numQueuedLabel.Size = new System.Drawing.Size( 207, 20 );
            this.numQueuedLabel.Spring = true;
            this.numQueuedLabel.Text = "No simulations in queue";
            // 
            // numRunninglabel
            // 
            this.numRunninglabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.numRunninglabel.Name = "numRunninglabel";
            this.numRunninglabel.Size = new System.Drawing.Size( 207, 20 );
            this.numRunninglabel.Spring = true;
            this.numRunninglabel.Text = "No simulations runnning";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.settingsButton,
            this.stopSimsButton,
            this.unlockButton} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 702, 25 );
            this.toolStrip1.TabIndex = 3;
            // 
            // settingsButton
            // 
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject( "settingsButton.Image" )));
            this.settingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size( 58, 22 );
            this.settingsButton.Text = "Settings...";
            this.settingsButton.Click += new System.EventHandler( this.settingsButton_Click );
            // 
            // stopSimsButton
            // 
            this.stopSimsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopSimsButton.Image = ((System.Drawing.Image)(resources.GetObject( "stopSimsButton.Image" )));
            this.stopSimsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopSimsButton.Name = "stopSimsButton";
            this.stopSimsButton.Size = new System.Drawing.Size( 89, 22 );
            this.stopSimsButton.Text = "Stop Simulations";
            this.stopSimsButton.Click += new System.EventHandler( this.stopSimsButton_Click );
            // 
            // runStateImages
            // 
            this.runStateImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject( "runStateImages.ImageStream" )));
            this.runStateImages.TransparentColor = System.Drawing.Color.Transparent;
            this.runStateImages.Images.SetKeyName( 0, "run1.ico" );
            this.runStateImages.Images.SetKeyName( 1, "run2.ico" );
            // 
            // unlockButton
            // 
            this.unlockButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.unlockButton.Image = ((System.Drawing.Image)(resources.GetObject( "unlockButton.Image" )));
            this.unlockButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unlockButton.Name = "unlockButton";
            this.unlockButton.Size = new System.Drawing.Size( 94, 22 );
            this.unlockButton.Text = "Unlock Database";
            this.unlockButton.ToolTipText = "Unlock Database for Running Simulations";
            this.unlockButton.Click += new System.EventHandler( this.unlockButton_Click );
            // 
            // SimControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 702, 458 );
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.toolStrip1 );
            this.Controls.Add( this.statusBar );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 600, 300 );
            this.Name = "SimControlForm";
            this.Text = "Simulation Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.SimControlForm_FormClosing );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.simBox.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.simDataGridView)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.runGrpBox.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.runningGrid)).EndInit();
            this.groupBox1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.processedGrid)).EndInit();
            this.statusBar.ResumeLayout( false );
            this.statusBar.PerformLayout();
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.GroupBox runGrpBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView runningGrid;
        private System.Windows.Forms.DataGridView simDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimulationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScenarioIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDatCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResetPanelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn sim_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsSimulationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsNameColCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsStatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentStatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentDateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsScenarioIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunTimeCol;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton settingsButton;
        private System.Windows.Forms.ToolStripButton stopSimsButton;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel numQueuedLabel;
        private System.Windows.Forms.ToolStripStatusLabel numRunninglabel;
        private System.Windows.Forms.ToolStripStatusLabel statusImage;
        private System.Windows.Forms.ImageList runStateImages;
        private System.Windows.Forms.GroupBox simBox;
        private System.Windows.Forms.DataGridView processedGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.ToolStripButton unlockButton;
    }
}