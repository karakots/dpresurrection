namespace MrktSimClient
{
    partial class ModelEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ModelEditor ) );
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSaveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripRefreshButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripOptionsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripHelpButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.topTabControl = new System.Windows.Forms.TabControl();
            this.modelTab = new System.Windows.Forms.TabPage();
            this.marketPlansTab = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.modelViewControl1 = new ModelView.ModelViewControl();
            this.marketPlanControl21 = new MrktSimClient.Controls.MarketPlanControl3();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.topTabControl.SuspendLayout();
            this.modelTab.SuspendLayout();
            this.marketPlansTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size( 32, 32 );
            this.toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSaveButton,
            this.toolStripRefreshButton,
            this.toolStripOptionsButton,
            this.toolStripHelpButton} );
            this.toolStrip.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size( 792, 31 );
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripSaveButton
            // 
            this.toolStripSaveButton.BackColor = System.Drawing.Color.White;
            this.toolStripSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSaveButton.Image = global::MrktSimClient.Properties.Resources.App;
            this.toolStripSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveButton.Margin = new System.Windows.Forms.Padding( 7 );
            this.toolStripSaveButton.Name = "toolStripSaveButton";
            this.toolStripSaveButton.Size = new System.Drawing.Size( 36, 17 );
            this.toolStripSaveButton.Text = "Save";
            this.toolStripSaveButton.Click += new System.EventHandler( this.toolStripSaveButton_Click );
            // 
            // toolStripRefreshButton
            // 
            this.toolStripRefreshButton.BackColor = System.Drawing.Color.White;
            this.toolStripRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripRefreshButton.Image = global::MrktSimClient.Properties.Resources.App;
            this.toolStripRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRefreshButton.Margin = new System.Windows.Forms.Padding( 7 );
            this.toolStripRefreshButton.Name = "toolStripRefreshButton";
            this.toolStripRefreshButton.Size = new System.Drawing.Size( 48, 17 );
            this.toolStripRefreshButton.Text = "Refresh";
            this.toolStripRefreshButton.Visible = false;
            this.toolStripRefreshButton.Click += new System.EventHandler( this.toolStripRefreshButton_Click );
            // 
            // toolStripOptionsButton
            // 
            this.toolStripOptionsButton.BackColor = System.Drawing.Color.White;
            this.toolStripOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripOptionsButton.Image = global::MrktSimClient.Properties.Resources.App;
            this.toolStripOptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOptionsButton.Margin = new System.Windows.Forms.Padding( 7 );
            this.toolStripOptionsButton.Name = "toolStripOptionsButton";
            this.toolStripOptionsButton.Size = new System.Drawing.Size( 47, 17 );
            this.toolStripOptionsButton.Text = "Options";
            this.toolStripOptionsButton.Click += new System.EventHandler( this.toolStripOptionsButton_Click );
            // 
            // toolStripHelpButton
            // 
            this.toolStripHelpButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripHelpButton.BackColor = System.Drawing.Color.White;
            this.toolStripHelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripHelpButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripHelpButton.Image" )));
            this.toolStripHelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHelpButton.Margin = new System.Windows.Forms.Padding( 7 );
            this.toolStripHelpButton.Name = "toolStripHelpButton";
            this.toolStripHelpButton.Size = new System.Drawing.Size( 39, 17 );
            this.toolStripHelpButton.Text = " Help ";
            this.toolStripHelpButton.Click += new System.EventHandler( this.toolStripHelpButton_Click );
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar} );
            this.statusStrip.Location = new System.Drawing.Point( 0, 444 );
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size( 792, 22 );
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size( 0, 17 );
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size( 100, 16 );
            this.progressBar.Visible = false;
            // 
            // topTabControl
            // 
            this.topTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.topTabControl.Controls.Add( this.modelTab );
            this.topTabControl.Controls.Add( this.marketPlansTab );
            this.topTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topTabControl.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.topTabControl.Location = new System.Drawing.Point( 0, 31 );
            this.topTabControl.Multiline = true;
            this.topTabControl.Name = "topTabControl";
            this.topTabControl.SelectedIndex = 0;
            this.topTabControl.Size = new System.Drawing.Size( 792, 413 );
            this.topTabControl.TabIndex = 2;
            this.topTabControl.SelectedIndexChanged += new System.EventHandler( this.topTabControl_SelectedIndexChanged );
            // 
            // modelTab
            // 
            this.modelTab.Controls.Add( this.modelViewControl1 );
            this.modelTab.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.modelTab.Location = new System.Drawing.Point( 23, 4 );
            this.modelTab.Name = "modelTab";
            this.modelTab.Padding = new System.Windows.Forms.Padding( 3 );
            this.modelTab.Size = new System.Drawing.Size( 765, 405 );
            this.modelTab.TabIndex = 0;
            this.modelTab.Text = "    Model     ";
            this.modelTab.UseVisualStyleBackColor = true;
            // 
            // marketPlansTab
            // 
            this.marketPlansTab.Controls.Add( this.panel1 );
            this.marketPlansTab.Location = new System.Drawing.Point( 23, 4 );
            this.marketPlansTab.Name = "marketPlansTab";
            this.marketPlansTab.Padding = new System.Windows.Forms.Padding( 3 );
            this.marketPlansTab.Size = new System.Drawing.Size( 765, 405 );
            this.marketPlansTab.TabIndex = 1;
            this.marketPlansTab.Text = "    Market Plans    ";
            this.marketPlansTab.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.marketPlanControl21 );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point( 3, 3 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 759, 399 );
            this.panel1.TabIndex = 0;
            // 
            // modelViewControl1
            // 
            this.modelViewControl1.Db = null;
            this.modelViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelViewControl1.Location = new System.Drawing.Point( 3, 3 );
            this.modelViewControl1.Name = "modelViewControl1";
            this.modelViewControl1.Size = new System.Drawing.Size( 759, 399 );
            this.modelViewControl1.TabIndex = 0;
            // 
            // marketPlanControl21
            // 
            this.marketPlanControl21.BackColor = System.Drawing.SystemColors.Control;
            this.marketPlanControl21.Cursor = System.Windows.Forms.Cursors.Default;
            this.marketPlanControl21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketPlanControl21.Location = new System.Drawing.Point( 0, 0 );
            this.marketPlanControl21.Name = "marketPlanControl21";
            this.marketPlanControl21.Size = new System.Drawing.Size( 759, 399 );
            this.marketPlanControl21.Suspend = false;
            this.marketPlanControl21.TabIndex = 0;
            // 
            // ModelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 14F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 792, 466 );
            this.Controls.Add( this.topTabControl );
            this.Controls.Add( this.statusStrip );
            this.Controls.Add( this.toolStrip );
            this.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 600, 400 );
            this.Name = "ModelEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MarketSim Model Editor";
            this.Load += new System.EventHandler( this.ModelEditor_Load );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ModelEditor_FormClosing );
            this.toolStrip.ResumeLayout( false );
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout( false );
            this.statusStrip.PerformLayout();
            this.topTabControl.ResumeLayout( false );
            this.modelTab.ResumeLayout( false );
            this.marketPlansTab.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl topTabControl;
        private System.Windows.Forms.TabPage modelTab;
        private System.Windows.Forms.TabPage marketPlansTab;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Panel panel1;
        private MrktSimClient.Controls.MarketPlanControl3 marketPlanControl21;
        private ModelView.ModelViewControl modelViewControl1;
        private System.Windows.Forms.ToolStripButton toolStripSaveButton;
        private System.Windows.Forms.ToolStripButton toolStripRefreshButton;
        private System.Windows.Forms.ToolStripButton toolStripOptionsButton;
        private System.Windows.Forms.ToolStripButton toolStripHelpButton;


    }
}