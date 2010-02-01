namespace Common
{
    partial class PackSize
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( PackSize ) );
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.packSizeGroup = new System.Windows.Forms.GroupBox();
            this.packSizeList = new System.Windows.Forms.ListBox();
            this.distGroup = new System.Windows.Forms.GroupBox();
            this.packValuesGrid = new MarketSimUtilities.MrktSimGrid();
            this.packSizeToolStrip = new System.Windows.Forms.ToolStrip();
            this.newPackSizeBut = new System.Windows.Forms.ToolStripButton();
            this.editBut = new System.Windows.Forms.ToolStripButton();
            this.deleteBut = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.packSizeGroup.SuspendLayout();
            this.distGroup.SuspendLayout();
            this.packSizeToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 25 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.packSizeGroup );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.distGroup );
            this.splitContainer2.Size = new System.Drawing.Size( 721, 444 );
            this.splitContainer2.SplitterDistance = 231;
            this.splitContainer2.TabIndex = 3;
            // 
            // packSizeGroup
            // 
            this.packSizeGroup.Controls.Add( this.packSizeList );
            this.packSizeGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packSizeGroup.Location = new System.Drawing.Point( 0, 0 );
            this.packSizeGroup.Name = "packSizeGroup";
            this.packSizeGroup.Size = new System.Drawing.Size( 231, 444 );
            this.packSizeGroup.TabIndex = 1;
            this.packSizeGroup.TabStop = false;
            this.packSizeGroup.Text = "Pack Size";
            // 
            // packSizeList
            // 
            this.packSizeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packSizeList.FormattingEnabled = true;
            this.packSizeList.Location = new System.Drawing.Point( 3, 16 );
            this.packSizeList.Name = "packSizeList";
            this.packSizeList.Size = new System.Drawing.Size( 225, 420 );
            this.packSizeList.TabIndex = 0;
            this.packSizeList.SelectedIndexChanged += new System.EventHandler( this.packSizeList_SelectedIndexChanged );
            // 
            // distGroup
            // 
            this.distGroup.Controls.Add( this.packValuesGrid );
            this.distGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.distGroup.Location = new System.Drawing.Point( 0, 0 );
            this.distGroup.Name = "distGroup";
            this.distGroup.Size = new System.Drawing.Size( 486, 444 );
            this.distGroup.TabIndex = 1;
            this.distGroup.TabStop = false;
            this.distGroup.Text = "Pack Size Distribution";
            // 
            // packValuesGrid
            // 
            this.packValuesGrid.DescribeRow = null;
            this.packValuesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packValuesGrid.EnabledGrid = true;
            this.packValuesGrid.Location = new System.Drawing.Point( 3, 16 );
            this.packValuesGrid.Name = "packValuesGrid";
            this.packValuesGrid.RowFilter = null;
            this.packValuesGrid.RowID = null;
            this.packValuesGrid.RowName = null;
            this.packValuesGrid.Size = new System.Drawing.Size( 480, 425 );
            this.packValuesGrid.Sort = "";
            this.packValuesGrid.TabIndex = 0;
            this.packValuesGrid.Table = null;
            // 
            // packSizeToolStrip
            // 
            this.packSizeToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.packSizeToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newPackSizeBut,
            this.editBut,
            this.deleteBut} );
            this.packSizeToolStrip.Location = new System.Drawing.Point( 0, 0 );
            this.packSizeToolStrip.Name = "packSizeToolStrip";
            this.packSizeToolStrip.Size = new System.Drawing.Size( 721, 25 );
            this.packSizeToolStrip.TabIndex = 4;
            this.packSizeToolStrip.Text = "toolStrip1";
            // 
            // newPackSizeBut
            // 
            this.newPackSizeBut.BackColor = System.Drawing.SystemColors.Control;
            this.newPackSizeBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newPackSizeBut.Image = ((System.Drawing.Image)(resources.GetObject( "newPackSizeBut.Image" )));
            this.newPackSizeBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newPackSizeBut.Name = "newPackSizeBut";
            this.newPackSizeBut.Size = new System.Drawing.Size( 51, 22 );
            this.newPackSizeBut.Text = "Create...";
            this.newPackSizeBut.Click += new System.EventHandler( this.createPackSize_Click );
            // 
            // editBut
            // 
            this.editBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editBut.Image = ((System.Drawing.Image)(resources.GetObject( "editBut.Image" )));
            this.editBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editBut.Name = "editBut";
            this.editBut.Size = new System.Drawing.Size( 38, 22 );
            this.editBut.Text = "Edit...";
            this.editBut.Click += new System.EventHandler( this.editToolStripMenuItem_Click );
            // 
            // deleteBut
            // 
            this.deleteBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteBut.Image = ((System.Drawing.Image)(resources.GetObject( "deleteBut.Image" )));
            this.deleteBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteBut.Name = "deleteBut";
            this.deleteBut.Size = new System.Drawing.Size( 51, 22 );
            this.deleteBut.Text = "Delete...";
            this.deleteBut.Click += new System.EventHandler( this.deleteToolStripMenuItem_Click );
            // 
            // PackSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer2 );
            this.Controls.Add( this.packSizeToolStrip );
            this.Name = "PackSize";
            this.Size = new System.Drawing.Size( 721, 469 );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.packSizeGroup.ResumeLayout( false );
            this.distGroup.ResumeLayout( false );
            this.packSizeToolStrip.ResumeLayout( false );
            this.packSizeToolStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private MarketSimUtilities.MrktSimGrid packValuesGrid;
        private System.Windows.Forms.ListBox packSizeList;
        private System.Windows.Forms.GroupBox packSizeGroup;
        private System.Windows.Forms.GroupBox distGroup;
        private System.Windows.Forms.ToolStrip packSizeToolStrip;
        private System.Windows.Forms.ToolStripButton newPackSizeBut;
        private System.Windows.Forms.ToolStripButton editBut;
        private System.Windows.Forms.ToolStripButton deleteBut;
    }
}
