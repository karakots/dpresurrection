namespace MrktSimClient.Controls
{
    partial class ScenarioControl0
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.simGrid1 = new MarketSimUtilities.MrktSimGrid();
            this.simLink = new Utilities.PopupMenuLinkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.resultsGrid = new MarketSimUtilities.MrktSimGrid();
            this.resultsLink = new Utilities.PopupMenuLinkLabel();
            this.descr = new Utilities.FadeTextPanel();
            this.name = new Utilities.FadeTextPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scenarioLink = new Utilities.PopupMenuLinkLabel();
            this.expandLink1 = new System.Windows.Forms.LinkLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.popupMenuPanel = new System.Windows.Forms.Panel();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.splitContainer3.Panel1.Controls.Add( this.tableLayoutPanel1 );
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add( this.tableLayoutPanel2 );
            this.splitContainer3.Size = new System.Drawing.Size( 700, 278 );
            this.splitContainer3.SplitterDistance = 124;
            this.splitContainer3.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 120F ) );
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.Controls.Add( this.simGrid1, 1, 0 );
            this.tableLayoutPanel1.Controls.Add( this.simLink, 0, 0 );
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.Size = new System.Drawing.Size( 700, 124 );
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // simGrid1
            // 
            this.simGrid1.DescribeRow = null;
            this.simGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simGrid1.EnabledGrid = true;
            this.simGrid1.Location = new System.Drawing.Point( 123, 3 );
            this.simGrid1.Name = "simGrid1";
            this.simGrid1.RowFilter = null;
            this.simGrid1.RowID = null;
            this.simGrid1.RowName = null;
            this.simGrid1.Size = new System.Drawing.Size( 574, 118 );
            this.simGrid1.Sort = "";
            this.simGrid1.TabIndex = 1;
            this.simGrid1.Table = null;
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
            this.simLink.PopupParentLevelsAbove = 6;
            this.simLink.RightMargin = 5;
            this.simLink.Size = new System.Drawing.Size( 112, 28 );
            this.simLink.TabIndex = 2;
            this.simLink.TabMargin = 15;
            this.simLink.TopMargin = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 120F ) );
            this.tableLayoutPanel2.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel2.Controls.Add( this.resultsGrid, 1, 0 );
            this.tableLayoutPanel2.Controls.Add( this.resultsLink, 0, 0 );
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point( 0, 0 );
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel2.Size = new System.Drawing.Size( 700, 150 );
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // resultsGrid
            // 
            this.resultsGrid.DescribeRow = null;
            this.resultsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsGrid.EnabledGrid = true;
            this.resultsGrid.Location = new System.Drawing.Point( 123, 3 );
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.RowFilter = null;
            this.resultsGrid.RowID = null;
            this.resultsGrid.RowName = null;
            this.resultsGrid.Size = new System.Drawing.Size( 574, 144 );
            this.resultsGrid.Sort = "";
            this.resultsGrid.TabIndex = 2;
            this.resultsGrid.Table = null;
            // 
            // resultsLink
            // 
            this.resultsLink.BackColor = System.Drawing.Color.Transparent;
            this.resultsLink.BottomMargin = 4;
            this.resultsLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.resultsLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.resultsLink.LeftMargin = 13;
            this.resultsLink.LinkText = "Results:";
            this.resultsLink.Location = new System.Drawing.Point( 4, 4 );
            this.resultsLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.resultsLink.MenuItemSpacing = 5;
            this.resultsLink.Name = "resultsLink";
            this.resultsLink.PopupBackColor = System.Drawing.Color.White;
            this.resultsLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.resultsLink.PopupParentLevelsAbove = 6;
            this.resultsLink.RightMargin = 5;
            this.resultsLink.Size = new System.Drawing.Size( 78, 29 );
            this.resultsLink.TabIndex = 3;
            this.resultsLink.TabMargin = 15;
            this.resultsLink.TopMargin = 9;
            // 
            // descr
            // 
            this.descr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descr.Location = new System.Drawing.Point( 123, 3 );
            this.descr.Name = "descr";
            this.descr.Size = new System.Drawing.Size( 574, 67 );
            this.descr.TabIndex = 12;
            // 
            // name
            // 
            this.name.Cursor = System.Windows.Forms.Cursors.Default;
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point( 0, 29 );
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size( 700, 35 );
            this.name.StyleNumber = 2;
            this.name.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add( this.scenarioLink );
            this.panel2.Controls.Add( this.expandLink1 );
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point( 0, 0 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 700, 29 );
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
            this.scenarioLink.Location = new System.Drawing.Point( 0, 0 );
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
            this.expandLink1.Location = new System.Drawing.Point( 658, 0 );
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
            this.splitContainer1.Size = new System.Drawing.Size( 700, 420 );
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
            this.splitContainer2.Size = new System.Drawing.Size( 700, 138 );
            this.splitContainer2.SplitterDistance = 61;
            this.splitContainer2.TabIndex = 0;
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size( 700, 73 );
            this.tableLayoutPanel3.TabIndex = 13;
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
            this.popupMenuPanel.Location = new System.Drawing.Point( 150, 300 );
            this.popupMenuPanel.Name = "popupMenuPanel";
            this.popupMenuPanel.Size = new System.Drawing.Size( 70, 70 );
            this.popupMenuPanel.TabIndex = 14;
            this.popupMenuPanel.Visible = false;
            this.popupMenuPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.popupMenuPanel_Paint );
            // 
            // ScenarioControl
            // 
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "ScenarioControl";
            this.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer3.Panel1.ResumeLayout( false );
            this.splitContainer3.Panel2.ResumeLayout( false );
            this.splitContainer3.ResumeLayout( false );
            this.tableLayoutPanel1.ResumeLayout( false );
            this.tableLayoutPanel2.ResumeLayout( false );
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
        private MarketSimUtilities.MrktSimGrid simGrid1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MarketSimUtilities.MrktSimGrid resultsGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private Utilities.PopupMenuLinkLabel simLink;
        private Utilities.PopupMenuLinkLabel resultsLink;
        private Utilities.PopupMenuLinkLabel scenarioLink;
        private System.Windows.Forms.Panel popupMenuPanel;
    }
}
