namespace GeoFormatter
{
    partial class GeoFormatter
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
            this.ProcessButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.updateDb = new System.Windows.Forms.Button();
            this.openDb = new System.Windows.Forms.Button();
            this.dbStatus = new System.Windows.Forms.Label();
            this.webFolderLabel = new System.Windows.Forms.Label();
            this.geoFolderlabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panDown = new System.Windows.Forms.Button();
            this.panUp = new System.Windows.Forms.Button();
            this.panRight = new System.Windows.Forms.Button();
            this.panLeft = new System.Windows.Forms.Button();
            this.saveBut = new System.Windows.Forms.Button();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.missingCounties = new System.Windows.Forms.ListBox();
            this.regionTree = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
            this.panel1.SuspendLayout();
            this.mapPanel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProcessButton
            // 
            this.ProcessButton.Location = new System.Drawing.Point( 25, 15 );
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size( 88, 23 );
            this.ProcessButton.TabIndex = 14;
            this.ProcessButton.Text = "Process Data";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler( this.ProcessButton_Click );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.button3 );
            this.panel1.Controls.Add( this.updateDb );
            this.panel1.Controls.Add( this.openDb );
            this.panel1.Controls.Add( this.dbStatus );
            this.panel1.Controls.Add( this.webFolderLabel );
            this.panel1.Controls.Add( this.geoFolderlabel );
            this.panel1.Controls.Add( this.button2 );
            this.panel1.Controls.Add( this.button1 );
            this.panel1.Controls.Add( this.panDown );
            this.panel1.Controls.Add( this.panUp );
            this.panel1.Controls.Add( this.panRight );
            this.panel1.Controls.Add( this.panLeft );
            this.panel1.Controls.Add( this.saveBut );
            this.panel1.Controls.Add( this.ProcessButton );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point( 0, 480 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 940, 98 );
            this.panel1.TabIndex = 15;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point( 251, 17 );
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size( 83, 23 );
            this.button3.TabIndex = 27;
            this.button3.Text = "Db Compare";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler( this.DbCompare_Click );
            // 
            // updateDb
            // 
            this.updateDb.Location = new System.Drawing.Point( 251, 58 );
            this.updateDb.Name = "updateDb";
            this.updateDb.Size = new System.Drawing.Size( 83, 23 );
            this.updateDb.TabIndex = 26;
            this.updateDb.Text = "Update Db";
            this.updateDb.UseVisualStyleBackColor = true;
            this.updateDb.Click += new System.EventHandler( this.updateDb_Click );
            // 
            // openDb
            // 
            this.openDb.Location = new System.Drawing.Point( 481, 58 );
            this.openDb.Name = "openDb";
            this.openDb.Size = new System.Drawing.Size( 83, 23 );
            this.openDb.TabIndex = 25;
            this.openDb.Text = "Open Db";
            this.openDb.UseVisualStyleBackColor = true;
            this.openDb.Click += new System.EventHandler( this.openDb_Click );
            // 
            // dbStatus
            // 
            this.dbStatus.AutoSize = true;
            this.dbStatus.Location = new System.Drawing.Point( 356, 22 );
            this.dbStatus.Name = "dbStatus";
            this.dbStatus.Size = new System.Drawing.Size( 95, 13 );
            this.dbStatus.TabIndex = 24;
            this.dbStatus.Text = "Found in database";
            // 
            // webFolderLabel
            // 
            this.webFolderLabel.AutoSize = true;
            this.webFolderLabel.Location = new System.Drawing.Point( 132, 68 );
            this.webFolderLabel.Name = "webFolderLabel";
            this.webFolderLabel.Size = new System.Drawing.Size( 92, 13 );
            this.webFolderLabel.TabIndex = 23;
            this.webFolderLabel.Text = "C:\\the web Folder";
            this.toolTip1.SetToolTip( this.webFolderLabel, "Double click to change" );
            this.webFolderLabel.Click += new System.EventHandler( this.webFolderLabel_Click );
            // 
            // geoFolderlabel
            // 
            this.geoFolderlabel.AutoSize = true;
            this.geoFolderlabel.Location = new System.Drawing.Point( 132, 20 );
            this.geoFolderlabel.Name = "geoFolderlabel";
            this.geoFolderlabel.Size = new System.Drawing.Size( 99, 13 );
            this.geoFolderlabel.TabIndex = 22;
            this.geoFolderlabel.Text = "C:\\Geoemtry Folder";
            this.toolTip1.SetToolTip( this.geoFolderlabel, "Double click to change" );
            this.geoFolderlabel.Click += new System.EventHandler( this.geoFolderlabel_Click );
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.button2.Location = new System.Drawing.Point( 818, 67 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 31, 23 );
            this.button2.TabIndex = 21;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler( this.button2_Click );
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.button1.Location = new System.Drawing.Point( 818, 12 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 31, 23 );
            this.button1.TabIndex = 20;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler( this.button1_Click );
            // 
            // panDown
            // 
            this.panDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panDown.Location = new System.Drawing.Point( 684, 67 );
            this.panDown.Name = "panDown";
            this.panDown.Size = new System.Drawing.Size( 30, 23 );
            this.panDown.TabIndex = 19;
            this.panDown.Text = "\\/";
            this.panDown.UseVisualStyleBackColor = true;
            this.panDown.Click += new System.EventHandler( this.panDown_Click );
            // 
            // panUp
            // 
            this.panUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panUp.Location = new System.Drawing.Point( 684, 12 );
            this.panUp.Name = "panUp";
            this.panUp.Size = new System.Drawing.Size( 30, 23 );
            this.panUp.TabIndex = 18;
            this.panUp.Text = "/\\";
            this.panUp.UseVisualStyleBackColor = true;
            this.panUp.Click += new System.EventHandler( this.panUp_Click );
            // 
            // panRight
            // 
            this.panRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panRight.Location = new System.Drawing.Point( 717, 39 );
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size( 30, 23 );
            this.panRight.TabIndex = 17;
            this.panRight.Text = ">";
            this.panRight.UseVisualStyleBackColor = true;
            this.panRight.Click += new System.EventHandler( this.panRight_Click );
            // 
            // panLeft
            // 
            this.panLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panLeft.Location = new System.Drawing.Point( 651, 39 );
            this.panLeft.Name = "panLeft";
            this.panLeft.Size = new System.Drawing.Size( 30, 23 );
            this.panLeft.TabIndex = 16;
            this.panLeft.Text = "<";
            this.panLeft.UseVisualStyleBackColor = true;
            this.panLeft.Click += new System.EventHandler( this.panLeft_Click );
            // 
            // saveBut
            // 
            this.saveBut.Location = new System.Drawing.Point( 25, 63 );
            this.saveBut.Name = "saveBut";
            this.saveBut.Size = new System.Drawing.Size( 88, 23 );
            this.saveBut.TabIndex = 15;
            this.saveBut.Text = "Save";
            this.saveBut.UseVisualStyleBackColor = true;
            this.saveBut.Click += new System.EventHandler( this.saveBut_Click );
            // 
            // mapPanel
            // 
            this.mapPanel.Controls.Add( this.splitContainer1 );
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point( 0, 0 );
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size( 940, 480 );
            this.mapPanel.TabIndex = 16;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.splitContainer2 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler( this.splitContainer1_Panel2_Paint );
            this.splitContainer1.Size = new System.Drawing.Size( 940, 480 );
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add( this.missingCounties );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.regionTree );
            this.splitContainer2.Size = new System.Drawing.Size( 313, 480 );
            this.splitContainer2.SplitterDistance = 104;
            this.splitContainer2.TabIndex = 0;
            // 
            // missingCounties
            // 
            this.missingCounties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.missingCounties.FormattingEnabled = true;
            this.missingCounties.Location = new System.Drawing.Point( 0, 0 );
            this.missingCounties.Name = "missingCounties";
            this.missingCounties.Size = new System.Drawing.Size( 313, 95 );
            this.missingCounties.TabIndex = 0;
            // 
            // regionTree
            // 
            this.regionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionTree.Location = new System.Drawing.Point( 0, 0 );
            this.regionTree.Name = "regionTree";
            this.regionTree.Size = new System.Drawing.Size( 313, 372 );
            this.regionTree.TabIndex = 0;
            this.regionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.regionTree_AfterSelect );
            // 
            // GeoFormatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 940, 578 );
            this.Controls.Add( this.mapPanel );
            this.Controls.Add( this.panel1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeoFormatter";
            this.Text = "Geo Formatter";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.mapPanel.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button saveBut;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox missingCounties;
        private System.Windows.Forms.TreeView regionTree;
        private System.Windows.Forms.Button panRight;
        private System.Windows.Forms.Button panLeft;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button panDown;
        private System.Windows.Forms.Button panUp;
        private System.Windows.Forms.Label webFolderLabel;
        private System.Windows.Forms.Label geoFolderlabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button openDb;
        private System.Windows.Forms.Label dbStatus;
        private System.Windows.Forms.Button updateDb;
        private System.Windows.Forms.Button button3;
    }
}

