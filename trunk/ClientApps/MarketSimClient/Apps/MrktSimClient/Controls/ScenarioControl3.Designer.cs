namespace MrktSimClient.Controls
{
    partial class ScenarioControl
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
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.simLink = new Utilities.PopupMenuLinkLabel();
            this.simDataGridView = new System.Windows.Forms.DataGridView();
            this.SimulationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDatCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResetPanelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultsLink = new Utilities.PopupMenuLinkLabel();
            this.resultsDataGridView = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scenarioLink = new Utilities.PopupMenuLinkLabel();
            this.expandLink1 = new System.Windows.Forms.LinkLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.name = new Utilities.FadeTextPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.descr = new Utilities.FadeTextPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.popupMenuPanel = new System.Windows.Forms.Panel();
            this.RunTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsSimulationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsNameColCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsStatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentStatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultsScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add( this.simLink );
            this.splitContainer3.Panel1.Controls.Add( this.simDataGridView );
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add( this.resultsLink );
            this.splitContainer3.Panel2.Controls.Add( this.resultsDataGridView );
            this.splitContainer3.Size = new System.Drawing.Size( 784, 306 );
            this.splitContainer3.SplitterDistance = 136;
            this.splitContainer3.TabIndex = 0;
            // 
            // simLink
            // 
            this.simLink.BackColor = System.Drawing.Color.Transparent;
            this.simLink.BottomMargin = 4;
            this.simLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.simLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.simLink.LeftMargin = 13;
            this.simLink.LinkText = "Simulations:";
            this.simLink.Location = new System.Drawing.Point( 4, 4 );
            this.simLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.simLink.MenuItemSpacing = 5;
            this.simLink.Name = "simLink";
            this.simLink.PopupBackColor = System.Drawing.Color.White;
            this.simLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.simLink.PopupParentLevelsAbove = 5;
            this.simLink.RightMargin = 5;
            this.simLink.Size = new System.Drawing.Size( 113, 28 );
            this.simLink.TabIndex = 2;
            this.simLink.TabMargin = 15;
            this.simLink.TopMargin = 9;
            // 
            // simDataGridView
            // 
            this.simDataGridView.AllowUserToAddRows = false;
            this.simDataGridView.AllowUserToDeleteRows = false;
            this.simDataGridView.AllowUserToResizeRows = false;
            this.simDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ResetPanelCol} );
            this.simDataGridView.Location = new System.Drawing.Point( 122, 8 );
            this.simDataGridView.Name = "simDataGridView";
            this.simDataGridView.ReadOnly = true;
            this.simDataGridView.RowHeadersVisible = false;
            this.simDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.simDataGridView.RowTemplate.Height = 16;
            this.simDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simDataGridView.Size = new System.Drawing.Size( 654, 120 );
            this.simDataGridView.TabIndex = 4;
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
            // resultsLink
            // 
            this.resultsLink.BackColor = System.Drawing.Color.Transparent;
            this.resultsLink.BottomMargin = 4;
            this.resultsLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.resultsLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.resultsLink.LeftMargin = 13;
            this.resultsLink.LinkText = "Results:";
            this.resultsLink.Location = new System.Drawing.Point( 4, 11 );
            this.resultsLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.resultsLink.MenuItemSpacing = 5;
            this.resultsLink.Name = "resultsLink";
            this.resultsLink.PopupBackColor = System.Drawing.Color.White;
            this.resultsLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.resultsLink.PopupParentLevelsAbove = 5;
            this.resultsLink.RightMargin = 5;
            this.resultsLink.Size = new System.Drawing.Size( 78, 29 );
            this.resultsLink.TabIndex = 3;
            this.resultsLink.TabMargin = 15;
            this.resultsLink.TopMargin = 9;
            // 
            // resultsDataGridView
            // 
            this.resultsDataGridView.AllowUserToAddRows = false;
            this.resultsDataGridView.AllowUserToDeleteRows = false;
            this.resultsDataGridView.AllowUserToResizeRows = false;
            this.resultsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.resultsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.resultsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.resultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.RunTimeCol,
            this.ResultsSimulationIdCol,
            this.ResultsNameColCol,
            this.RunCol,
            this.ResultsStatusCol,
            this.CurrentStatusCol,
            this.CurrentDateCol,
            this.RunIdCol,
            this.ResultsScenarioIdCol,
            this.StartedCol} );
            this.resultsDataGridView.Location = new System.Drawing.Point( 123, 11 );
            this.resultsDataGridView.Name = "resultsDataGridView";
            this.resultsDataGridView.ReadOnly = true;
            this.resultsDataGridView.RowHeadersVisible = false;
            this.resultsDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.resultsDataGridView.RowTemplate.Height = 16;
            this.resultsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsDataGridView.Size = new System.Drawing.Size( 653, 146 );
            this.resultsDataGridView.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add( this.scenarioLink );
            this.panel2.Controls.Add( this.expandLink1 );
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point( 0, 0 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 784, 29 );
            this.panel2.TabIndex = 0;
            // 
            // scenarioLink
            // 
            this.scenarioLink.BackColor = System.Drawing.Color.Transparent;
            this.scenarioLink.BottomMargin = 4;
            this.scenarioLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenarioLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.scenarioLink.LeftMargin = 13;
            this.scenarioLink.LinkText = "Scenario:";
            this.scenarioLink.Location = new System.Drawing.Point( 4, 0 );
            this.scenarioLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.scenarioLink.MenuItemSpacing = 5;
            this.scenarioLink.Name = "scenarioLink";
            this.scenarioLink.PopupBackColor = System.Drawing.Color.White;
            this.scenarioLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.scenarioLink.PopupParentLevelsAbove = 2;
            this.scenarioLink.RightMargin = 5;
            this.scenarioLink.Size = new System.Drawing.Size( 100, 29 );
            this.scenarioLink.TabIndex = 2;
            this.scenarioLink.TabMargin = 15;
            this.scenarioLink.TopMargin = 9;
            // 
            // expandLink1
            // 
            this.expandLink1.AutoSize = true;
            this.expandLink1.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandLink1.Location = new System.Drawing.Point( 742, 0 );
            this.expandLink1.Name = "expandLink1";
            this.expandLink1.Size = new System.Drawing.Size( 42, 13 );
            this.expandLink1.TabIndex = 1;
            this.expandLink1.TabStop = true;
            this.expandLink1.Text = "expand";
            this.expandLink1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.expandLink_LinkClicked );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.splitContainer2 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer3 );
            this.splitContainer1.Size = new System.Drawing.Size( 784, 448 );
            this.splitContainer1.SplitterDistance = 138;
            this.splitContainer1.TabIndex = 13;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.name );
            this.splitContainer2.Panel1.Controls.Add( this.panel2 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.tableLayoutPanel3 );
            this.splitContainer2.Size = new System.Drawing.Size( 784, 138 );
            this.splitContainer2.SplitterDistance = 61;
            this.splitContainer2.TabIndex = 0;
            // 
            // name
            // 
            this.name.Cursor = System.Windows.Forms.Cursors.Default;
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point( 0, 29 );
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size( 784, 35 );
            this.name.StyleNumber = 2;
            this.name.TabIndex = 10;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 120F ) );
            this.tableLayoutPanel3.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel3.Controls.Add( this.descr, 1, 0 );
            this.tableLayoutPanel3.Controls.Add( this.label1, 0, 0 );
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point( 0, 0 );
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel3.Size = new System.Drawing.Size( 784, 73 );
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // descr
            // 
            this.descr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descr.Location = new System.Drawing.Point( 123, 3 );
            this.descr.Name = "descr";
            this.descr.Size = new System.Drawing.Size( 658, 67 );
            this.descr.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 4, 4 );
            this.label1.Margin = new System.Windows.Forms.Padding( 4 );
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding( 4 );
            this.label1.Size = new System.Drawing.Size( 90, 25 );
            this.label1.TabIndex = 13;
            this.label1.Text = "Description";
            // 
            // popupMenuPanel
            // 
            this.popupMenuPanel.Location = new System.Drawing.Point( 50, 200 );
            this.popupMenuPanel.Name = "popupMenuPanel";
            this.popupMenuPanel.Size = new System.Drawing.Size( 70, 70 );
            this.popupMenuPanel.TabIndex = 14;
            this.popupMenuPanel.Visible = false;
            this.popupMenuPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.popupMenuPanel_Paint );
            // 
            // RunTimeCol
            // 
            this.RunTimeCol.DataPropertyName = "run_time";
            this.RunTimeCol.HeaderText = "Started";
            this.RunTimeCol.Name = "RunTimeCol";
            this.RunTimeCol.ReadOnly = true;
            this.RunTimeCol.Visible = false;
            this.RunTimeCol.Width = 101;
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
            this.ResultsStatusCol.Width = 70;
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
            this.StartedCol.DataPropertyName = "start_time";
            this.StartedCol.HeaderText = "Started";
            this.StartedCol.Name = "StartedCol";
            this.StartedCol.ReadOnly = true;
            this.StartedCol.Width = 115;
            // 
            // ScenarioControl
            // 
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "ScenarioControl";
            this.Size = new System.Drawing.Size( 784, 448 );
            this.splitContainer3.Panel1.ResumeLayout( false );
            this.splitContainer3.Panel2.ResumeLayout( false );
            this.splitContainer3.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.simDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).EndInit();
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.tableLayoutPanel3.ResumeLayout( false );
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel expandLink1;
        private Utilities.FadeTextPanel name;
        private Utilities.FadeTextPanel descr;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private Utilities.PopupMenuLinkLabel simLink;
        private Utilities.PopupMenuLinkLabel resultsLink;
        private Utilities.PopupMenuLinkLabel scenarioLink;
        private System.Windows.Forms.Panel popupMenuPanel;
        private System.Windows.Forms.DataGridView simDataGridView;
        private System.Windows.Forms.DataGridView resultsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimulationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScenarioIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDatCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResetPanelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunTimeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsSimulationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsNameColCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsStatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentStatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentDateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultsScenarioIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartedCol;
    }
}
