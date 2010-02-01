namespace Calibration
{
    partial class CalibrateVehicles
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
            this.MediaTree = new System.Windows.Forms.TreeView();
            this.TreeMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.addAdOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAdOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCPMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleCPMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeRegionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.DetailsBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.queryBox = new System.Windows.Forms.TextBox();
            this.findBut = new System.Windows.Forms.Button();
            this.changeSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeMenuStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MediaTree
            // 
            this.MediaTree.ContextMenuStrip = this.TreeMenuStrip;
            this.MediaTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaTree.Location = new System.Drawing.Point( 0, 0 );
            this.MediaTree.Name = "MediaTree";
            this.MediaTree.Size = new System.Drawing.Size( 247, 257 );
            this.MediaTree.TabIndex = 0;
            this.MediaTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.MediaTree_AfterSelect );
            // 
            // TreeMenuStrip
            // 
            this.TreeMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.addAdOptionToolStripMenuItem,
            this.removeAdOptionToolStripMenuItem,
            this.setCPMToolStripMenuItem,
            this.scaleCPMToolStripMenuItem,
            this.changeRegionToolStripMenuItem,
            this.changeURLToolStripMenuItem,
            this.changeNameToolStripMenuItem,
            this.changeSizeToolStripMenuItem} );
            this.TreeMenuStrip.Name = "TreeMenuStrip";
            this.TreeMenuStrip.Size = new System.Drawing.Size( 165, 202 );
            // 
            // addAdOptionToolStripMenuItem
            // 
            this.addAdOptionToolStripMenuItem.Name = "addAdOptionToolStripMenuItem";
            this.addAdOptionToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.addAdOptionToolStripMenuItem.Text = "Add AdOption";
            this.addAdOptionToolStripMenuItem.Click += new System.EventHandler( this.addAdOptionToolStripMenuItem_Click );
            // 
            // removeAdOptionToolStripMenuItem
            // 
            this.removeAdOptionToolStripMenuItem.Name = "removeAdOptionToolStripMenuItem";
            this.removeAdOptionToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.removeAdOptionToolStripMenuItem.Text = "Remove AdOption";
            this.removeAdOptionToolStripMenuItem.Click += new System.EventHandler( this.removeAdOptionToolStripMenuItem_Click );
            // 
            // setCPMToolStripMenuItem
            // 
            this.setCPMToolStripMenuItem.Name = "setCPMToolStripMenuItem";
            this.setCPMToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.setCPMToolStripMenuItem.Text = "Set CPM";
            this.setCPMToolStripMenuItem.Click += new System.EventHandler( this.setCPMToolStripMenuItem_Click );
            // 
            // scaleCPMToolStripMenuItem
            // 
            this.scaleCPMToolStripMenuItem.Name = "scaleCPMToolStripMenuItem";
            this.scaleCPMToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.scaleCPMToolStripMenuItem.Text = "Scale CPM";
            this.scaleCPMToolStripMenuItem.Click += new System.EventHandler( this.scaleCPMToolStripMenuItem_Click );
            // 
            // changeRegionToolStripMenuItem
            // 
            this.changeRegionToolStripMenuItem.Enabled = false;
            this.changeRegionToolStripMenuItem.Name = "changeRegionToolStripMenuItem";
            this.changeRegionToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.changeRegionToolStripMenuItem.Text = "Change Region";
            this.changeRegionToolStripMenuItem.Click += new System.EventHandler( this.changeRegionToolStripMenuItem_Click );
            // 
            // changeURLToolStripMenuItem
            // 
            this.changeURLToolStripMenuItem.Enabled = false;
            this.changeURLToolStripMenuItem.Name = "changeURLToolStripMenuItem";
            this.changeURLToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.changeURLToolStripMenuItem.Text = "Change URL";
            this.changeURLToolStripMenuItem.Click += new System.EventHandler( this.changeURLToolStripMenuItem_Click );
            // 
            // changeNameToolStripMenuItem
            // 
            this.changeNameToolStripMenuItem.Enabled = false;
            this.changeNameToolStripMenuItem.Name = "changeNameToolStripMenuItem";
            this.changeNameToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.changeNameToolStripMenuItem.Text = "Change Name";
            this.changeNameToolStripMenuItem.Click += new System.EventHandler( this.changeNameToolStripMenuItem_Click );
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Location = new System.Drawing.Point( 442, 9 );
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size( 75, 23 );
            this.ApplyButton.TabIndex = 2;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler( this.ApplyButton_Click );
            // 
            // DetailsBox
            // 
            this.DetailsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailsBox.Location = new System.Drawing.Point( 0, 0 );
            this.DetailsBox.Name = "DetailsBox";
            this.DetailsBox.ReadOnly = true;
            this.DetailsBox.Size = new System.Drawing.Size( 279, 257 );
            this.DetailsBox.TabIndex = 4;
            this.DetailsBox.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
            this.splitContainer1.Panel2.Controls.Add( this.queryBox );
            this.splitContainer1.Panel2.Controls.Add( this.findBut );
            this.splitContainer1.Panel2.Controls.Add( this.ApplyButton );
            this.splitContainer1.Size = new System.Drawing.Size( 530, 307 );
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.MediaTree );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.DetailsBox );
            this.splitContainer2.Size = new System.Drawing.Size( 530, 257 );
            this.splitContainer2.SplitterDistance = 247;
            this.splitContainer2.TabIndex = 0;
            // 
            // queryBox
            // 
            this.queryBox.Location = new System.Drawing.Point( 12, 14 );
            this.queryBox.Name = "queryBox";
            this.queryBox.Size = new System.Drawing.Size( 209, 20 );
            this.queryBox.TabIndex = 4;
            // 
            // findBut
            // 
            this.findBut.Location = new System.Drawing.Point( 245, 11 );
            this.findBut.Name = "findBut";
            this.findBut.Size = new System.Drawing.Size( 89, 23 );
            this.findBut.TabIndex = 3;
            this.findBut.Text = "Find Vehicle";
            this.findBut.UseVisualStyleBackColor = true;
            this.findBut.Click += new System.EventHandler( this.findBut_Click );
            // 
            // changeSizeToolStripMenuItem
            // 
            this.changeSizeToolStripMenuItem.Name = "changeSizeToolStripMenuItem";
            this.changeSizeToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
            this.changeSizeToolStripMenuItem.Text = "Scale Size";
            this.changeSizeToolStripMenuItem.Click += new System.EventHandler( this.changeSizeToolStripMenuItem_Click );
            // 
            // CalibrateVehicles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 530, 307 );
            this.Controls.Add( this.splitContainer1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CalibrateVehicles";
            this.Text = "CalibrateVehicles";
            this.TreeMenuStrip.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TreeView MediaTree;
        private System.Windows.Forms.ContextMenuStrip TreeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addAdOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAdOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCPMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scaleCPMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeRegionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeURLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeNameToolStripMenuItem;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.RichTextBox DetailsBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox queryBox;
        private System.Windows.Forms.Button findBut;
        private System.Windows.Forms.ToolStripMenuItem changeSizeToolStripMenuItem;
    }
}