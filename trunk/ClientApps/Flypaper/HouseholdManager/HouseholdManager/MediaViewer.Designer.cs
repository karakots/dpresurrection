namespace HouseholdManager
{
    partial class MediaViewer
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
            this.MediaInfoBox = new System.Windows.Forms.ListBox();
            this.SegmentInfoBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.addSegmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.clearSegmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
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
            this.MediaTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaTree.Location = new System.Drawing.Point( 0, 0 );
            this.MediaTree.Name = "MediaTree";
            this.MediaTree.Size = new System.Drawing.Size( 233, 355 );
            this.MediaTree.TabIndex = 0;
            // 
            // MediaInfoBox
            // 
            this.MediaInfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaInfoBox.FormattingEnabled = true;
            this.MediaInfoBox.Location = new System.Drawing.Point( 0, 0 );
            this.MediaInfoBox.Name = "MediaInfoBox";
            this.MediaInfoBox.Size = new System.Drawing.Size( 566, 355 );
            this.MediaInfoBox.TabIndex = 7;
            // 
            // SegmentInfoBox
            // 
            this.SegmentInfoBox.ContextMenuStrip = this.contextMenuStrip1;
            this.SegmentInfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SegmentInfoBox.FormattingEnabled = true;
            this.SegmentInfoBox.Location = new System.Drawing.Point( 0, 0 );
            this.SegmentInfoBox.Name = "SegmentInfoBox";
            this.SegmentInfoBox.Size = new System.Drawing.Size( 372, 355 );
            this.SegmentInfoBox.TabIndex = 8;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.addSegmentToolStripMenuItem,
            this.clearSegmentsToolStripMenuItem} );
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size( 153, 70 );
            // 
            // addSegmentToolStripMenuItem
            // 
            this.addSegmentToolStripMenuItem.Name = "addSegmentToolStripMenuItem";
            this.addSegmentToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.addSegmentToolStripMenuItem.Text = "Add Segment";
            this.addSegmentToolStripMenuItem.Click += new System.EventHandler( this.SegButton_Click );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.MediaTree );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Size = new System.Drawing.Size( 1179, 355 );
            this.splitContainer1.SplitterDistance = 233;
            this.splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.MediaInfoBox );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.SegmentInfoBox );
            this.splitContainer2.Size = new System.Drawing.Size( 942, 355 );
            this.splitContainer2.SplitterDistance = 566;
            this.splitContainer2.TabIndex = 0;
            // 
            // clearSegmentsToolStripMenuItem
            // 
            this.clearSegmentsToolStripMenuItem.Name = "clearSegmentsToolStripMenuItem";
            this.clearSegmentsToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.clearSegmentsToolStripMenuItem.Text = "Clear Segments";
            this.clearSegmentsToolStripMenuItem.Click += new System.EventHandler( this.clearSegmentsToolStripMenuItem_Click );
            // 
            // MediaViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 1179, 355 );
            this.Controls.Add( this.splitContainer1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MediaViewer";
            this.Text = "MediaViewer";
            this.contextMenuStrip1.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TreeView MediaTree;
        private System.Windows.Forms.ListBox MediaInfoBox;
        private System.Windows.Forms.ListBox SegmentInfoBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addSegmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSegmentsToolStripMenuItem;
    }
}