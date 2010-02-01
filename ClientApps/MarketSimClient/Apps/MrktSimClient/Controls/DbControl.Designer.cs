namespace MrktSimClient.Controls
{
    partial class DbControl
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
            this.projectList = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.dbName = new Utilities.FadeTextPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dbLink = new Utilities.PopupMenuLinkLabel();
            this.popupMenuPanel = new System.Windows.Forms.Panel();
            this.bannerControl1 = new MrktSimClient.Controls.BannerControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectList
            // 
            this.projectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectList.BackColor = System.Drawing.SystemColors.Control;
            this.projectList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.projectList.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.projectList.ForeColor = System.Drawing.Color.Blue;
            this.projectList.FormattingEnabled = true;
            this.projectList.ItemHeight = 15;
            this.projectList.Location = new System.Drawing.Point( 3, 99 );
            this.projectList.Name = "projectList";
            this.projectList.Size = new System.Drawing.Size( 191, 317 );
            this.projectList.TabIndex = 1;
            this.projectList.DoubleClick += new System.EventHandler( this.projectList_DoubleClick );
            this.projectList.SelectedIndexChanged += new System.EventHandler( this.projectList_SelectedIndexChanged );
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
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add( this.projectList );
            this.splitContainer1.Panel1.Controls.Add( this.label1 );
            this.splitContainer1.Panel1.Controls.Add( this.dbName );
            this.splitContainer1.Panel1.Controls.Add( this.panel1 );
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
            this.splitContainer1.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font( "Copperplate Gothic Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 0, 67 );
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding( 3, 0, 0, 0 );
            this.label1.Size = new System.Drawing.Size( 198, 31 );
            this.label1.TabIndex = 7;
            this.label1.Text = "Projects";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dbName
            // 
            this.dbName.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbName.Location = new System.Drawing.Point( 0, 32 );
            this.dbName.Name = "dbName";
            this.dbName.Size = new System.Drawing.Size( 198, 35 );
            this.dbName.StyleNumber = 2;
            this.dbName.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.dbLink );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 198, 32 );
            this.panel1.TabIndex = 6;
            // 
            // dbLink
            // 
            this.dbLink.BackColor = System.Drawing.Color.Transparent;
            this.dbLink.BottomMargin = 4;
            this.dbLink.Font = new System.Drawing.Font( "Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dbLink.HighlightColor = System.Drawing.Color.LightSalmon;
            this.dbLink.LeftMargin = 13;
            this.dbLink.LinkText = "Database:";
            this.dbLink.Location = new System.Drawing.Point( 0, 0 );
            this.dbLink.Margin = new System.Windows.Forms.Padding( 4, 4, 4, 4 );
            this.dbLink.MenuItemSpacing = 5;
            this.dbLink.Name = "dbLink";
            this.dbLink.PopupBackColor = System.Drawing.Color.White;
            this.dbLink.PopupFont = new System.Drawing.Font( "Microsoft Sans Serif", 8F );
            this.dbLink.PopupParentLevelsAbove = 3;
            this.dbLink.PopupXAdjust = 1;
            this.dbLink.RightMargin = 5;
            this.dbLink.Size = new System.Drawing.Size( 99, 29 );
            this.dbLink.TabIndex = 0;
            this.dbLink.TabMargin = 15;
            this.dbLink.TopMargin = 9;
            // 
            // popupMenuPanel
            // 
            this.popupMenuPanel.Location = new System.Drawing.Point( 147, 295 );
            this.popupMenuPanel.Name = "popupMenuPanel";
            this.popupMenuPanel.Size = new System.Drawing.Size( 125, 102 );
            this.popupMenuPanel.TabIndex = 3;
            this.popupMenuPanel.Visible = false;
            this.popupMenuPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.popupMenuPanel_Paint );
            // 
            // bannerControl1
            // 
            this.bannerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bannerControl1.Location = new System.Drawing.Point( 0, 0 );
            this.bannerControl1.Name = "bannerControl1";
            this.bannerControl1.Size = new System.Drawing.Size( 494, 418 );
            this.bannerControl1.TabIndex = 0;
            // 
            // DbControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "DbControl";
            this.Size = new System.Drawing.Size( 700, 420 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ListBox projectList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Utilities.FadeTextPanel dbName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private BannerControl bannerControl1;
        private System.Windows.Forms.Panel popupMenuPanel;
        private Utilities.PopupMenuLinkLabel dbLink;
    }
}
