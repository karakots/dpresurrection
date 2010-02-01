namespace MrktSimClient.Controls
{
    partial class ModelControl
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
            this.modName = new Utilities.FadeTextPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkpointCheckBox = new System.Windows.Forms.CheckBox();
            this.descBox1 = new Utilities.FadeTextPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.modelLink = new Utilities.PopupMenuLinkLabel();
            this.expandLink = new System.Windows.Forms.LinkLabel();
            this.scenarioList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bannerControl1 = new MrktSimClient.Controls.BannerControl();
            this.popupMenuPanel = new System.Windows.Forms.Panel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // modName
            // 
            this.modName.BackColor = System.Drawing.SystemColors.Control;
            this.modName.Cursor = System.Windows.Forms.Cursors.Default;
            this.modName.Dock = System.Windows.Forms.DockStyle.Top;
            this.modName.Location = new System.Drawing.Point( 0, 31 );
            this.modName.Name = "modName";
            this.modName.Size = new System.Drawing.Size( 198, 35 );
            this.modName.StyleNumber = 2;
            this.modName.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 10;
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
            this.splitContainer2.Panel1.Controls.Add( this.checkpointCheckBox );
            this.splitContainer2.Panel1.Controls.Add( this.descBox1 );
            this.splitContainer2.Panel1.Controls.Add( this.modName );
            this.splitContainer2.Panel1.Controls.Add( this.panel1 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.scenarioList );
            this.splitContainer2.Panel2.Controls.Add( this.label1 );
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 1 );
            this.splitContainer2.Size = new System.Drawing.Size( 198, 418 );
            this.splitContainer2.SplitterDistance = 200;
            this.splitContainer2.TabIndex = 12;
            // 
            // checkpointCheckBox
            // 
            this.checkpointCheckBox.AutoSize = true;
            this.checkpointCheckBox.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.checkpointCheckBox.Location = new System.Drawing.Point( 6, 71 );
            this.checkpointCheckBox.Name = "checkpointCheckBox";
            this.checkpointCheckBox.Size = new System.Drawing.Size( 148, 17 );
            this.checkpointCheckBox.TabIndex = 19;
            this.checkpointCheckBox.Text = "Check Point: 11/11/2007";
            this.checkpointCheckBox.UseVisualStyleBackColor = true;
            this.checkpointCheckBox.CheckedChanged += new System.EventHandler( this.checkpointCheckBox_CheckedChanged );
            // 
            // descBox1
            // 
            this.descBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.descBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descBox1.Location = new System.Drawing.Point( 1, 103 );
            this.descBox1.Name = "descBox1";
            this.descBox1.Size = new System.Drawing.Size( 197, 96 );
            this.descBox1.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.modelLink );
            this.panel1.Controls.Add( this.expandLink );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 198, 31 );
            this.panel1.TabIndex = 13;
            // 
            // modelLink
            // 
            this.modelLink.BackColor = System.Drawing.Color.Transparent;
            this.modelLink.BottomMargin = 4;
            this.modelLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.modelLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.modelLink.LeftMargin = 13;
            this.modelLink.LinkText = "Model:";
            this.modelLink.Location = new System.Drawing.Point( 0, 0 );
            this.modelLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.modelLink.MenuItemSpacing = 5;
            this.modelLink.Name = "modelLink";
            this.modelLink.PopupBackColor = System.Drawing.Color.White;
            this.modelLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.modelLink.PopupParentLevelsAbove = 2;
            this.modelLink.PopupXAdjust = 1;
            this.modelLink.RightMargin = 5;
            this.modelLink.Size = new System.Drawing.Size( 62, 31 );
            this.modelLink.TabIndex = 12;
            this.modelLink.TabMargin = 15;
            this.modelLink.TopMargin = 9;
            // 
            // expandLink
            // 
            this.expandLink.AutoSize = true;
            this.expandLink.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandLink.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.expandLink.Location = new System.Drawing.Point( 156, 0 );
            this.expandLink.Name = "expandLink";
            this.expandLink.Size = new System.Drawing.Size( 42, 13 );
            this.expandLink.TabIndex = 11;
            this.expandLink.TabStop = true;
            this.expandLink.Text = "expand";
            this.expandLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.expandLink_LinkClicked );
            // 
            // scenarioList
            // 
            this.scenarioList.BackColor = System.Drawing.SystemColors.Control;
            this.scenarioList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scenarioList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioList.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenarioList.ForeColor = System.Drawing.Color.Blue;
            this.scenarioList.FormattingEnabled = true;
            this.scenarioList.ItemHeight = 15;
            this.scenarioList.Location = new System.Drawing.Point( 5, 30 );
            this.scenarioList.Name = "scenarioList";
            this.scenarioList.Size = new System.Drawing.Size( 188, 182 );
            this.scenarioList.TabIndex = 0;
            this.scenarioList.SelectedIndexChanged += new System.EventHandler( this.scenarioList_SelectedIndexChanged );
            this.scenarioList.DoubleClick += new System.EventHandler( this.scenarioList_DoubleClick );
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 5, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 188, 30 );
            this.label1.TabIndex = 9;
            this.label1.Text = "Scenarios";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // bannerControl1
            // 
            this.bannerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bannerControl1.Location = new System.Drawing.Point( 0, 0 );
            this.bannerControl1.Name = "bannerControl1";
            this.bannerControl1.Size = new System.Drawing.Size( 494, 418 );
            this.bannerControl1.TabIndex = 0;
            // 
            // popupMenuPanel
            // 
            this.popupMenuPanel.Location = new System.Drawing.Point( 150, 300 );
            this.popupMenuPanel.Name = "popupMenuPanel";
            this.popupMenuPanel.Size = new System.Drawing.Size( 70, 70 );
            this.popupMenuPanel.TabIndex = 11;
            this.popupMenuPanel.Visible = false;
            this.popupMenuPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.popupMenuPanel_Paint );
            // 
            // ModelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "ModelControl";
            this.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private Utilities.FadeTextPanel modName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox scenarioList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel expandLink;
        private System.Windows.Forms.Label label1;
        private BannerControl bannerControl1;
        private Utilities.PopupMenuLinkLabel modelLink;
        private System.Windows.Forms.Panel popupMenuPanel;
        private Utilities.FadeTextPanel descBox1;
        private System.Windows.Forms.CheckBox checkpointCheckBox;

    }
}
