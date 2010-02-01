namespace MrktSimClient.Controls
{
    partial class ProjectControl
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
            this.modelList = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.descBox = new Utilities.FadeTextPanel();
            this.projName = new Utilities.FadeTextPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.projectLink = new Utilities.PopupMenuLinkLabel();
            this.expandLink = new System.Windows.Forms.LinkLabel();
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
            // modelList
            // 
            this.modelList.BackColor = System.Drawing.SystemColors.Control;
            this.modelList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelList.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.modelList.ForeColor = System.Drawing.Color.Blue;
            this.modelList.FormattingEnabled = true;
            this.modelList.ItemHeight = 15;
            this.modelList.Items.AddRange( new object[] {
            "aaaaaaa",
            "bbbbbbbb",
            "cccccccc",
            "xxxxxxxx",
            "zzzz",
            "aaaaaa"} );
            this.modelList.Location = new System.Drawing.Point( 5, 23 );
            this.modelList.Name = "modelList";
            this.modelList.Size = new System.Drawing.Size( 188, 257 );
            this.modelList.TabIndex = 6;
            this.modelList.SelectedIndexChanged += new System.EventHandler( this.modelList_SelectedIndexChanged );
            this.modelList.DoubleClick += new System.EventHandler( this.modelList_DoubleClick );
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
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
            this.splitContainer1.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 7;
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
            this.splitContainer2.Panel1.Controls.Add( this.descBox );
            this.splitContainer2.Panel1.Controls.Add( this.projName );
            this.splitContainer2.Panel1.Controls.Add( this.panel1 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.modelList );
            this.splitContainer2.Panel2.Controls.Add( this.label1 );
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding( 5, 0, 5, 1 );
            this.splitContainer2.Size = new System.Drawing.Size( 198, 418 );
            this.splitContainer2.SplitterDistance = 133;
            this.splitContainer2.TabIndex = 10;
            // 
            // descBox
            // 
            this.descBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descBox.Location = new System.Drawing.Point( 0, 67 );
            this.descBox.Name = "descBox";
            this.descBox.Size = new System.Drawing.Size( 198, 66 );
            this.descBox.TabIndex = 9;
            // 
            // projName
            // 
            this.projName.BackColor = System.Drawing.SystemColors.Control;
            this.projName.Dock = System.Windows.Forms.DockStyle.Top;
            this.projName.Location = new System.Drawing.Point( 0, 32 );
            this.projName.Name = "projName";
            this.projName.Size = new System.Drawing.Size( 198, 35 );
            this.projName.StyleNumber = 2;
            this.projName.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.projectLink );
            this.panel1.Controls.Add( this.expandLink );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 198, 32 );
            this.panel1.TabIndex = 10;
            // 
            // projectLink
            // 
            this.projectLink.BackColor = System.Drawing.Color.Transparent;
            this.projectLink.BottomMargin = 4;
            this.projectLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.projectLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.projectLink.LeftMargin = 13;
            this.projectLink.LinkText = "Project:";
            this.projectLink.Location = new System.Drawing.Point( 0, 0 );
            this.projectLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.projectLink.MenuItemSpacing = 5;
            this.projectLink.Name = "projectLink";
            this.projectLink.PopupBackColor = System.Drawing.Color.White;
            this.projectLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.projectLink.PopupParentLevelsAbove = 2;
            this.projectLink.PopupXAdjust = 1;
            this.projectLink.RightMargin = 5;
            this.projectLink.Size = new System.Drawing.Size( 129, 30 );
            this.projectLink.TabIndex = 11;
            this.projectLink.TabMargin = 15;
            this.projectLink.TopMargin = 9;
            // 
            // expandLink
            // 
            this.expandLink.AutoSize = true;
            this.expandLink.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandLink.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.expandLink.Location = new System.Drawing.Point( 156, 0 );
            this.expandLink.Name = "expandLink";
            this.expandLink.Size = new System.Drawing.Size( 42, 13 );
            this.expandLink.TabIndex = 10;
            this.expandLink.TabStop = true;
            this.expandLink.Text = "expand";
            this.expandLink.Click += new System.EventHandler( this.projName_DoubleClick );
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 5, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 188, 23 );
            this.label1.TabIndex = 8;
            this.label1.Text = "Models";
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
            this.popupMenuPanel.TabIndex = 8;
            this.popupMenuPanel.Visible = false;
            this.popupMenuPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.popupMenuPanel_Paint );
            // 
            // ProjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "ProjectControl";
            this.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ListBox modelList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Utilities.FadeTextPanel projName;
        private Utilities.FadeTextPanel descBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel expandLink;
        private System.Windows.Forms.Label label1;
        private BannerControl bannerControl1;
        private Utilities.PopupMenuLinkLabel projectLink;
        private System.Windows.Forms.Panel popupMenuPanel;
    }
}
