namespace Common
{
    partial class PriceType
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( PriceType ) );
            this.priceTypeGrid = new MarketSimUtilities.MrktSimGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newPriceTypeBut = new System.Windows.Forms.ToolStripButton();
            this.priceTypeMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.newPriceTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.priceTypeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // priceTypeGrid
            // 
            this.priceTypeGrid.ContextMenuStrip = this.priceTypeMenu;
            this.priceTypeGrid.DescribeRow = null;
            this.priceTypeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priceTypeGrid.EnabledGrid = true;
            this.priceTypeGrid.Location = new System.Drawing.Point( 0, 25 );
            this.priceTypeGrid.Name = "priceTypeGrid";
            this.priceTypeGrid.RowFilter = null;
            this.priceTypeGrid.RowID = null;
            this.priceTypeGrid.RowName = null;
            this.priceTypeGrid.Size = new System.Drawing.Size( 607, 392 );
            this.priceTypeGrid.Sort = "";
            this.priceTypeGrid.TabIndex = 0;
            this.priceTypeGrid.Table = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newPriceTypeBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 607, 25 );
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newPriceTypeBut
            // 
            this.newPriceTypeBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newPriceTypeBut.Image = ((System.Drawing.Image)(resources.GetObject( "newPriceTypeBut.Image" )));
            this.newPriceTypeBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newPriceTypeBut.Name = "newPriceTypeBut";
            this.newPriceTypeBut.Size = new System.Drawing.Size( 87, 22 );
            this.newPriceTypeBut.Text = "New Price Type";
            this.newPriceTypeBut.Click += new System.EventHandler( this.addNewPriceTypeToolStripMenuItem_Click );
            // 
            // priceTypeMenu
            // 
            this.priceTypeMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newPriceTypeToolStripMenuItem} );
            this.priceTypeMenu.Name = "priceTypeMenu";
            this.priceTypeMenu.Size = new System.Drawing.Size( 154, 26 );
            // 
            // newPriceTypeToolStripMenuItem
            // 
            this.newPriceTypeToolStripMenuItem.Name = "newPriceTypeToolStripMenuItem";
            this.newPriceTypeToolStripMenuItem.Size = new System.Drawing.Size( 153, 22 );
            this.newPriceTypeToolStripMenuItem.Text = "New Price Type";
            // 
            // PriceType
            // 
            this.Controls.Add( this.priceTypeGrid );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "PriceType";
            this.Size = new System.Drawing.Size( 607, 417 );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.priceTypeMenu.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private MarketSimUtilities.MrktSimGrid priceTypeGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newPriceTypeBut;
        private System.Windows.Forms.ContextMenuStrip priceTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem newPriceTypeToolStripMenuItem;
    }
}
