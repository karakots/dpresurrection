using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;

using MarketSimUtilities;
using ExcelInterface;
using ErrorInterface;

namespace Common
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class SegmentGridControl : MrktSimControl
	{
		public override void Refresh()
		{

			base.Refresh();

			iSizeGrid.Refresh();
			iPurchaseGrid.Refresh();
			iRepurchaseGrid.Refresh();
			// iInitialGrid.Refresh();
			iBudgetGrid.Refresh();
			iAwarenessGrid.Refresh();
			iChoiceGrid.Refresh();
            priceUtilityGrid.Refresh();

            computeMeanSize();
		}

        public override bool Suspend
        {
            set
            {
                base.Suspend = value;

                iSizeGrid.Suspend = value;
                iPurchaseGrid.Suspend = value;
                iRepurchaseGrid.Suspend = value;
                // iInitialGrid.Suspend = value;
                iBudgetGrid.Suspend = value;
                iAwarenessGrid.Suspend = value;
                iChoiceGrid.Suspend = value;
                priceUtilityGrid.Suspend = value;

            }
        }
		public override void Flush()
		{

			iSizeGrid.Flush();
			iPurchaseGrid.Flush();
			iRepurchaseGrid.Flush();
			//iInitialGrid.Flush();
			iBudgetGrid.Flush();
			iAwarenessGrid.Flush();
            iChoiceGrid.Flush();
            priceUtilityGrid.Flush();
		}

		// need this as well
		public override void Reset()
		{
			useBudget = theDb.Model.consumer_budget;

			createTableStyles();			
		}

		private bool useBudget
		{
			set
			{
				if (value)
				{
					this.tabControl.SuspendLayout();
					this.tabControl.Controls.Remove(this.budgetPage);
					this.tabControl.Controls.Add(this.budgetPage);
					this.tabControl.ResumeLayout();
				}
				else
				{
					this.tabControl.SuspendLayout();
					this.tabControl.Controls.Remove(this.budgetPage);
					this.tabControl.ResumeLayout();
				}
			}
		}

		private System.Windows.Forms.TabPage sizePage;
		private System.Windows.Forms.TabPage purchasePage;
		private System.Windows.Forms.TabPage repurchasePage;
		private System.Windows.Forms.TabPage budgetPage;
		private System.Windows.Forms.TabPage awarenessPage;
		private System.Windows.Forms.TabPage persuasionPage;
		private MrktSimGrid iPersuasionGrid;
		private MrktSimGrid iSizeGrid;
		//private MrktSimGrid iInitialGrid;
		private MrktSimGrid iPurchaseGrid;
		private MrktSimGrid iBudgetGrid;
		private MrktSimGrid iAwarenessGrid;
		private MrktSimGrid iRepurchaseGrid;
		private MrktSimGrid iChoiceGrid;
        private System.Windows.Forms.Splitter splitter1;

        //NOT CURRENTLY IMPLEMENTED
		private System.Windows.Forms.TabPage choiceMathPage;
		private System.Windows.Forms.TabControl tabControl;
        private TabPage packSizePage;
        private MrktSimGrid packSizeGrid;
        DataTable packSizeTable;

        const string segment_id = "segment_id";
        const string segment_name = "segment_name";
        const string product_size = "Mean Product Size";
        const string units_purchased = "Units Purchased";
        const string volume = "Expected Volume";
        private TabPage priceUtility;
        private MrktSimGrid priceUtilityGrid;
        private ToolStrip toolStrip1;
        private ToolStripButton newSegBut;
        private ToolStripButton importBut;
        private ToolStripButton importPriceUtilBut;
        private ToolStripSeparator toolStripSeparator1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SegmentGridControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

           


			iSizeGrid.Table = theDb.Data.segment;
			iPurchaseGrid.Table = theDb.Data.segment;
			iRepurchaseGrid.Table = theDb.Data.segment;
		//	iInitialGrid.Table = theDb.Data.segment;
			iBudgetGrid.Table = theDb.Data.segment;
			iAwarenessGrid.Table = theDb.Data.segment;
			iPersuasionGrid.Table = theDb.Data.segment;
			iChoiceGrid.Table = theDb.Data.segment;

			iSizeGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iPurchaseGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iRepurchaseGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			//iInitialGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iBudgetGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iAwarenessGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iPersuasionGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;
			iChoiceGrid.RowFilter =  "segment_id <> " + ModelDb.AllID;

			// for model parameters
			iSizeGrid.Db = theDb;
			iPurchaseGrid.Db = theDb;
			iRepurchaseGrid.Db = theDb;
		//	iInitialGrid.Db = theDb;
			iBudgetGrid.Db = theDb;
			iAwarenessGrid.Db = theDb;
			iPersuasionGrid.Db = theDb;
			iChoiceGrid.Db = theDb;

			iSizeGrid.RowID = "segment_id";
			iPurchaseGrid.RowID = "segment_id";
			iRepurchaseGrid.RowID = "segment_id";
		//	iInitialGrid.RowID = "segment_id";
			iBudgetGrid.RowID = "segment_id";
			iAwarenessGrid.RowID = "segment_id";
			iPersuasionGrid.RowID = "segment_id";
			iChoiceGrid.RowID = "segment_id";

			iSizeGrid.RowName = "segment_name";
			iPurchaseGrid.RowName = "segment_name";
			iRepurchaseGrid.RowName = "segment_name";
		//	iInitialGrid.RowName = "segment_name";
			iBudgetGrid.RowName = "segment_name";
			iAwarenessGrid.RowName = "segment_name";
			iPersuasionGrid.RowName = "segment_name";
			iChoiceGrid.RowName = "segment_name";

            priceUtilityGrid.DataMatrix = new DataMatrix(
               "Segment",
               theDb.Data.segment,
               "segment_id",
               "segment_name",
               theDb.Data.price_type,
               "id",
               "name",
               theDb.Data.segment_price_utility,
               "segment_id",
               "price_type_id",
               "util" );


            if( Database.Nimo )
            {
                // pack size table
                packSizeTable = new DataTable( "PackSize" );

                // add columns
                DataColumn idCol = packSizeTable.Columns.Add( segment_id, typeof( int ) );
                packSizeTable.PrimaryKey = new DataColumn[] { idCol };

                packSizeTable.Columns.Add( segment_name, typeof( string ) );
                packSizeTable.Columns.Add( product_size, typeof( double ) );
                packSizeTable.Columns.Add( units_purchased, typeof( double ) );
                packSizeTable.Columns.Add( volume, typeof( double ) );


                packSizeGrid.Table = packSizeTable;

                packSizeGrid.RowFilter = "segment_id <> " + ModelDb.AllID;


                packSizeGrid.DescriptionWindow = false;
                packSizeGrid.AllowDelete = false;
                packSizeGrid.ReadOnly = true;
            }
            else
            {
                // remove pack size tab
                this.tabControl.Controls.Remove( this.packSizePage );
            }

			createTableStyles();

			useBudget = theDb.Model.consumer_budget;

            this.tabControl.Controls.Remove(this.choiceMathPage);
            this.tabControl.Controls.Remove(this.budgetPage);

         
            computeMeanSize();
		
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SegmentGridControl ) );
            this.tabControl = new System.Windows.Forms.TabControl();
            this.sizePage = new System.Windows.Forms.TabPage();
            this.iSizeGrid = new MarketSimUtilities.MrktSimGrid();
            this.purchasePage = new System.Windows.Forms.TabPage();
            this.iPurchaseGrid = new MarketSimUtilities.MrktSimGrid();
            this.repurchasePage = new System.Windows.Forms.TabPage();
            this.iRepurchaseGrid = new MarketSimUtilities.MrktSimGrid();
            this.persuasionPage = new System.Windows.Forms.TabPage();
            this.iPersuasionGrid = new MarketSimUtilities.MrktSimGrid();
            this.awarenessPage = new System.Windows.Forms.TabPage();
            this.iAwarenessGrid = new MarketSimUtilities.MrktSimGrid();
            this.budgetPage = new System.Windows.Forms.TabPage();
            this.iBudgetGrid = new MarketSimUtilities.MrktSimGrid();
            this.choiceMathPage = new System.Windows.Forms.TabPage();
            this.iChoiceGrid = new MarketSimUtilities.MrktSimGrid();
            this.packSizePage = new System.Windows.Forms.TabPage();
            this.packSizeGrid = new MarketSimUtilities.MrktSimGrid();
            this.priceUtility = new System.Windows.Forms.TabPage();
            this.priceUtilityGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newSegBut = new System.Windows.Forms.ToolStripButton();
            this.importBut = new System.Windows.Forms.ToolStripButton();
            this.importPriceUtilBut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl.SuspendLayout();
            this.sizePage.SuspendLayout();
            this.purchasePage.SuspendLayout();
            this.repurchasePage.SuspendLayout();
            this.persuasionPage.SuspendLayout();
            this.awarenessPage.SuspendLayout();
            this.budgetPage.SuspendLayout();
            this.choiceMathPage.SuspendLayout();
            this.packSizePage.SuspendLayout();
            this.priceUtility.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add( this.sizePage );
            this.tabControl.Controls.Add( this.purchasePage );
            this.tabControl.Controls.Add( this.repurchasePage );
            this.tabControl.Controls.Add( this.persuasionPage );
            this.tabControl.Controls.Add( this.awarenessPage );
            this.tabControl.Controls.Add( this.budgetPage );
            this.tabControl.Controls.Add( this.choiceMathPage );
            this.tabControl.Controls.Add( this.packSizePage );
            this.tabControl.Controls.Add( this.priceUtility );
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point( 0, 25 );
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size( 656, 324 );
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler( this.tabControl_SelectedIndexChanged );
            // 
            // sizePage
            // 
            this.sizePage.Controls.Add( this.iSizeGrid );
            this.sizePage.Location = new System.Drawing.Point( 4, 22 );
            this.sizePage.Name = "sizePage";
            this.sizePage.Size = new System.Drawing.Size( 648, 298 );
            this.sizePage.TabIndex = 0;
            this.sizePage.Text = "Size";
            this.sizePage.UseVisualStyleBackColor = true;
            // 
            // iSizeGrid
            // 
            this.iSizeGrid.DescribeRow = null;
            this.iSizeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iSizeGrid.EnabledGrid = true;
            this.iSizeGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iSizeGrid.Name = "iSizeGrid";
            this.iSizeGrid.RowFilter = "";
            this.iSizeGrid.RowID = null;
            this.iSizeGrid.RowName = null;
            this.iSizeGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iSizeGrid.Sort = "";
            this.iSizeGrid.TabIndex = 0;
            this.iSizeGrid.Table = null;
            // 
            // purchasePage
            // 
            this.purchasePage.Controls.Add( this.iPurchaseGrid );
            this.purchasePage.Location = new System.Drawing.Point( 4, 22 );
            this.purchasePage.Name = "purchasePage";
            this.purchasePage.Size = new System.Drawing.Size( 648, 298 );
            this.purchasePage.TabIndex = 2;
            this.purchasePage.Text = "Choice";
            this.purchasePage.UseVisualStyleBackColor = true;
            // 
            // iPurchaseGrid
            // 
            this.iPurchaseGrid.DescribeRow = null;
            this.iPurchaseGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iPurchaseGrid.EnabledGrid = true;
            this.iPurchaseGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iPurchaseGrid.Name = "iPurchaseGrid";
            this.iPurchaseGrid.RowFilter = "";
            this.iPurchaseGrid.RowID = null;
            this.iPurchaseGrid.RowName = null;
            this.iPurchaseGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iPurchaseGrid.Sort = "";
            this.iPurchaseGrid.TabIndex = 0;
            this.iPurchaseGrid.Table = null;
            // 
            // repurchasePage
            // 
            this.repurchasePage.Controls.Add( this.iRepurchaseGrid );
            this.repurchasePage.Location = new System.Drawing.Point( 4, 22 );
            this.repurchasePage.Name = "repurchasePage";
            this.repurchasePage.Size = new System.Drawing.Size( 648, 298 );
            this.repurchasePage.TabIndex = 3;
            this.repurchasePage.Text = "Repurchase";
            this.repurchasePage.UseVisualStyleBackColor = true;
            // 
            // iRepurchaseGrid
            // 
            this.iRepurchaseGrid.DescribeRow = null;
            this.iRepurchaseGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iRepurchaseGrid.EnabledGrid = true;
            this.iRepurchaseGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iRepurchaseGrid.Name = "iRepurchaseGrid";
            this.iRepurchaseGrid.RowFilter = "";
            this.iRepurchaseGrid.RowID = null;
            this.iRepurchaseGrid.RowName = null;
            this.iRepurchaseGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iRepurchaseGrid.Sort = "";
            this.iRepurchaseGrid.TabIndex = 0;
            this.iRepurchaseGrid.Table = null;
            // 
            // persuasionPage
            // 
            this.persuasionPage.Controls.Add( this.iPersuasionGrid );
            this.persuasionPage.Location = new System.Drawing.Point( 4, 22 );
            this.persuasionPage.Name = "persuasionPage";
            this.persuasionPage.Size = new System.Drawing.Size( 648, 298 );
            this.persuasionPage.TabIndex = 10;
            this.persuasionPage.Text = "Persuasion";
            this.persuasionPage.UseVisualStyleBackColor = true;
            // 
            // iPersuasionGrid
            // 
            this.iPersuasionGrid.DescribeRow = null;
            this.iPersuasionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iPersuasionGrid.EnabledGrid = true;
            this.iPersuasionGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iPersuasionGrid.Name = "iPersuasionGrid";
            this.iPersuasionGrid.RowFilter = "";
            this.iPersuasionGrid.RowID = null;
            this.iPersuasionGrid.RowName = null;
            this.iPersuasionGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iPersuasionGrid.Sort = "";
            this.iPersuasionGrid.TabIndex = 0;
            this.iPersuasionGrid.Table = null;
            // 
            // awarenessPage
            // 
            this.awarenessPage.Controls.Add( this.iAwarenessGrid );
            this.awarenessPage.Location = new System.Drawing.Point( 4, 22 );
            this.awarenessPage.Name = "awarenessPage";
            this.awarenessPage.Size = new System.Drawing.Size( 648, 298 );
            this.awarenessPage.TabIndex = 6;
            this.awarenessPage.Text = "Awareness";
            this.awarenessPage.UseVisualStyleBackColor = true;
            // 
            // iAwarenessGrid
            // 
            this.iAwarenessGrid.DescribeRow = null;
            this.iAwarenessGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iAwarenessGrid.EnabledGrid = true;
            this.iAwarenessGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iAwarenessGrid.Name = "iAwarenessGrid";
            this.iAwarenessGrid.RowFilter = "";
            this.iAwarenessGrid.RowID = null;
            this.iAwarenessGrid.RowName = null;
            this.iAwarenessGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iAwarenessGrid.Sort = "";
            this.iAwarenessGrid.TabIndex = 0;
            this.iAwarenessGrid.Table = null;
            // 
            // budgetPage
            // 
            this.budgetPage.Controls.Add( this.iBudgetGrid );
            this.budgetPage.Location = new System.Drawing.Point( 4, 22 );
            this.budgetPage.Name = "budgetPage";
            this.budgetPage.Size = new System.Drawing.Size( 648, 298 );
            this.budgetPage.TabIndex = 5;
            this.budgetPage.Text = "Budget";
            this.budgetPage.UseVisualStyleBackColor = true;
            // 
            // iBudgetGrid
            // 
            this.iBudgetGrid.DescribeRow = null;
            this.iBudgetGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iBudgetGrid.EnabledGrid = true;
            this.iBudgetGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iBudgetGrid.Name = "iBudgetGrid";
            this.iBudgetGrid.RowFilter = "";
            this.iBudgetGrid.RowID = null;
            this.iBudgetGrid.RowName = null;
            this.iBudgetGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iBudgetGrid.Sort = "";
            this.iBudgetGrid.TabIndex = 0;
            this.iBudgetGrid.Table = null;
            // 
            // choiceMathPage
            // 
            this.choiceMathPage.Controls.Add( this.iChoiceGrid );
            this.choiceMathPage.Location = new System.Drawing.Point( 4, 22 );
            this.choiceMathPage.Name = "choiceMathPage";
            this.choiceMathPage.Size = new System.Drawing.Size( 648, 298 );
            this.choiceMathPage.TabIndex = 11;
            this.choiceMathPage.Text = "Choice Math";
            this.choiceMathPage.UseVisualStyleBackColor = true;
            // 
            // iChoiceGrid
            // 
            this.iChoiceGrid.DescribeRow = null;
            this.iChoiceGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iChoiceGrid.EnabledGrid = true;
            this.iChoiceGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iChoiceGrid.Name = "iChoiceGrid";
            this.iChoiceGrid.RowFilter = "";
            this.iChoiceGrid.RowID = null;
            this.iChoiceGrid.RowName = null;
            this.iChoiceGrid.Size = new System.Drawing.Size( 648, 298 );
            this.iChoiceGrid.Sort = "";
            this.iChoiceGrid.TabIndex = 0;
            this.iChoiceGrid.Table = null;
            // 
            // packSizePage
            // 
            this.packSizePage.Controls.Add( this.packSizeGrid );
            this.packSizePage.Location = new System.Drawing.Point( 4, 22 );
            this.packSizePage.Name = "packSizePage";
            this.packSizePage.Padding = new System.Windows.Forms.Padding( 3 );
            this.packSizePage.Size = new System.Drawing.Size( 648, 298 );
            this.packSizePage.TabIndex = 12;
            this.packSizePage.Text = "Pack Size";
            this.packSizePage.UseVisualStyleBackColor = true;
            // 
            // packSizeGrid
            // 
            this.packSizeGrid.DescribeRow = null;
            this.packSizeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packSizeGrid.EnabledGrid = true;
            this.packSizeGrid.Location = new System.Drawing.Point( 3, 3 );
            this.packSizeGrid.Name = "packSizeGrid";
            this.packSizeGrid.RowFilter = null;
            this.packSizeGrid.RowID = null;
            this.packSizeGrid.RowName = null;
            this.packSizeGrid.Size = new System.Drawing.Size( 642, 292 );
            this.packSizeGrid.Sort = "";
            this.packSizeGrid.TabIndex = 0;
            this.packSizeGrid.Table = null;
            // 
            // priceUtility
            // 
            this.priceUtility.Controls.Add( this.priceUtilityGrid );
            this.priceUtility.Location = new System.Drawing.Point( 4, 22 );
            this.priceUtility.Name = "priceUtility";
            this.priceUtility.Padding = new System.Windows.Forms.Padding( 3 );
            this.priceUtility.Size = new System.Drawing.Size( 648, 298 );
            this.priceUtility.TabIndex = 13;
            this.priceUtility.Text = "Price Utilities";
            this.priceUtility.UseVisualStyleBackColor = true;
            // 
            // priceUtilityGrid
            // 
            this.priceUtilityGrid.DescribeRow = null;
            this.priceUtilityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priceUtilityGrid.EnabledGrid = true;
            this.priceUtilityGrid.Location = new System.Drawing.Point( 3, 3 );
            this.priceUtilityGrid.Name = "priceUtilityGrid";
            this.priceUtilityGrid.RowFilter = null;
            this.priceUtilityGrid.RowID = null;
            this.priceUtilityGrid.RowName = null;
            this.priceUtilityGrid.Size = new System.Drawing.Size( 642, 292 );
            this.priceUtilityGrid.Sort = "";
            this.priceUtilityGrid.TabIndex = 0;
            this.priceUtilityGrid.Table = null;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point( 0, 349 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 656, 3 );
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importBut,
            this.importPriceUtilBut,
            this.toolStripSeparator1,
            this.newSegBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 656, 25 );
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newSegBut
            // 
            this.newSegBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newSegBut.Image = ((System.Drawing.Image)(resources.GetObject( "newSegBut.Image" )));
            this.newSegBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newSegBut.Name = "newSegBut";
            this.newSegBut.Size = new System.Drawing.Size( 121, 22 );
            this.newSegBut.Text = "Create New Segment...";
            this.newSegBut.Click += new System.EventHandler( this.createSegementButton_Click );
            // 
            // importBut
            // 
            this.importBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importBut.Image = ((System.Drawing.Image)(resources.GetObject( "importBut.Image" )));
            this.importBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importBut.Name = "importBut";
            this.importBut.Size = new System.Drawing.Size( 99, 22 );
            this.importBut.Text = "Import Segments...";
            this.importBut.Click += new System.EventHandler( this.importBut_Click );
            // 
            // importPriceUtilBut
            // 
            this.importPriceUtilBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importPriceUtilBut.Image = ((System.Drawing.Image)(resources.GetObject( "importPriceUtilBut.Image" )));
            this.importPriceUtilBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importPriceUtilBut.Name = "importPriceUtilBut";
            this.importPriceUtilBut.Size = new System.Drawing.Size( 112, 22 );
            this.importPriceUtilBut.Text = "Import Price Utilities...";
            this.importPriceUtilBut.Click += new System.EventHandler( this.importPriceUtilBut_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // SegmentGridControl
            // 
            this.Controls.Add( this.tabControl );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "SegmentGridControl";
            this.Size = new System.Drawing.Size( 656, 352 );
            this.tabControl.ResumeLayout( false );
            this.sizePage.ResumeLayout( false );
            this.purchasePage.ResumeLayout( false );
            this.repurchasePage.ResumeLayout( false );
            this.persuasionPage.ResumeLayout( false );
            this.awarenessPage.ResumeLayout( false );
            this.budgetPage.ResumeLayout( false );
            this.choiceMathPage.ResumeLayout( false );
            this.packSizePage.ResumeLayout( false );
            this.priceUtility.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

	
		private void createTableStyles()
		{
			this.SuspendLayout();

			string[] daysToYears = {"Days", "Weeks", "Months", "Years"};
			string[] dayToYear = {"Day", "Week", "Month", "Year"};

			// iSizeGrid
			iSizeGrid.Clear();
			iSizeGrid.AddTextColumn("segment_name");

			iSizeGrid.AddTextColumn("segment_size");

            //iSizeGrid.AddCheckBoxColumn("compress_population", "y  ", "n  ");
			iSizeGrid.AddTextColumn("variability");

			//iPurchaseGrid

			iPurchaseGrid.Clear();

			iPurchaseGrid.AddTextColumn("segment_name");


            string[] priceValChoice = null;

            if( Database.Nimo )
            {
                priceValChoice = new string[] { "Absolute", "Base", "Relative" };
            }
            else
            {
                priceValChoice = new string[] { "Absolute", "Relative" };
            }

            iPurchaseGrid.AddComboBoxColumn("price_value", priceValChoice);
           
            iPurchaseGrid.AddTextColumn( "price_disutility" );
			iPurchaseGrid.AddTextColumn("attribute_sensitivity");
			iPurchaseGrid.AddTextColumn("persuasion_scaling");

			iPurchaseGrid.AddTextColumn("max_display_hits_per_trip");

            if( Database.Nimo )
			{
				iPurchaseGrid.AddTextColumn("display_utility");
				iPurchaseGrid.AddTextColumn("display_utility_scaling_factor");
            }
			else
			{
				// do not use inertia for NIMO
				iPurchaseGrid.AddTextColumn("inertia");
			}

			iPurchaseGrid.AddTextColumn("loyalty");


			//iRepurchaseGrid

			iRepurchaseGrid.Clear();

			iRepurchaseGrid.AddTextColumn("segment_name");
			//iRepurchaseGrid.AddCheckBoxColumn("repurchase", "y  ", "n  ");

            if( Database.Nimo )
			{
				// do not need to add this if there is no choice
				//iRepurchaseGrid.AddTextColumn("repurchase_model", true);
				iRepurchaseGrid.AddTextColumn("gamma_location_parameter_a");
				iRepurchaseGrid.AddTextColumn("gamma_shape_parameter_k");
                iRepurchaseGrid.AddNumericColumn( "reference_price", "Flow Through Weight" );

                iRepurchaseGrid.AddNumericColumn( "min_freq", "Min Percentile" );
                iRepurchaseGrid.AddNumericColumn( "max_freq", "Max Percentile" );

			}
			else
			{
				iRepurchaseGrid.AddTextColumn("repurchase_period_frequency");
				iRepurchaseGrid.AddTextColumn("repurchase_frequency_variation");
			}

			iRepurchaseGrid.AddTextColumn("avg_max_units_purch");

			//iInitialGrid

			//iInitialGrid.Clear();

			//iInitialGrid.AddTextColumn("segment_name");

			// iInitialGrid.AddTextColumn("num_initial_buyers");
			// iInitialGrid.AddTextColumn("initial_buying_period");
			// iInitialGrid.AddCheckBoxColumn("seed_with_repurchasers", "y", "n");

			//iBudgetGrid

			iBudgetGrid.Clear();

			iBudgetGrid.AddTextColumn("segment_name");

			iBudgetGrid.AddCheckBoxColumn("use_budget", "y", "n");
			iBudgetGrid.AddTextColumn("budget");
			iBudgetGrid.AddComboBoxColumn("budget_period", dayToYear);
			iBudgetGrid.AddCheckBoxColumn("save_unspent", "y", "n");
			iBudgetGrid.AddTextColumn("initial_savings");

			//iAwarenessGrid

			iAwarenessGrid.Clear();

			iAwarenessGrid.AddTextColumn("segment_name");
		

            //string[] awarenessModelChoice = {"Awareness", "Persuasion & Awareness"};
            //if(!ProjectDb.Nimo)
            //{
            //    iAwarenessGrid.AddComboBoxColumn("awareness_model", awarenessModelChoice);
            //    iAwarenessGrid.AddTextColumn("awareness_threshold");
            //}
			iAwarenessGrid.AddTextColumn("awareness_decay_rate_pre");
			iAwarenessGrid.AddTextColumn("awareness_decay_rate_post");

			// iPersuasionGrid
			iPersuasionGrid.Clear();
			iPersuasionGrid.AddTextColumn("segment_name");

            if( !Database.Nimo )
            {
                string[] valueComputation = { "Absolute", "Base 10 Log", "Share of Voice", "Exponential" };
                iPersuasionGrid.AddComboBoxColumn( "persuasion_value_computation", valueComputation );
            }

            iPersuasionGrid.AddTextColumn( "persuasion_decay_rate_pre" );
            iPersuasionGrid.AddTextColumn( "persuasion_decay_rate_post" );


            if( !Database.Nimo )
			{
                // this is now the saturation value
                iPersuasionGrid.AddTextColumn("units_desired_trigger", "Saturation");
			}

			//iChoiceGrid

			iChoiceGrid.Clear();	


			iSizeGrid.Reset();
			iPurchaseGrid.Reset();
			iRepurchaseGrid.Reset();
			//iInitialGrid.Reset();
			iBudgetGrid.Reset();
			iAwarenessGrid.Reset();
			iPersuasionGrid.Reset();
			iChoiceGrid.Reset();

            if( Database.Nimo )
            {
                packSizeGrid.Clear();
                packSizeGrid.AddTextColumn( segment_name, "segment", true );
                packSizeGrid.AddNumericColumn( product_size, true );

                packSizeGrid.AddNumericColumn( units_purchased, true );
                packSizeGrid.AddNumericColumn( volume, true );

        
                packSizeGrid.Reset();
            }

			this.ResumeLayout(false);

		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.Height +=1;
			this.Height -=1;
		
		}

		private void createSegementButton_Click(object sender, System.EventArgs e)
		{
            InputString dlg = new InputString();

            dlg.Text = "Enter Name for Segment";
            dlg.InputText = "";

            DialogResult rslt = dlg.ShowDialog();

            if( rslt != DialogResult.OK )
            {
                return;
            }

            string token = dlg.InputText;

            theDb.CreateSegment( token );
		}
	
		
		private void ExcelImport()
		{
		
			SegmentReader reader = new SegmentReader(theDb);

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;

			DialogResult frslt = openFileDlg.ShowDialog();

			this.Suspend = true;
			this.SuspendLayout();

			if (frslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;
				ErrorList errors = new ErrorList();

				DataTable table;


                if( Database.Nimo )
                {
                    errors.addErrors( ExcelReader.ReadTable( fileName, "Segments", "K", true, out table ) );
                }
                else
                {
                    errors.addErrors( ExcelReader.ReadTable( fileName, "Segments", "G", true, out table ) );
                }
				
				if(errors.Count > 0)
				{
					errors.addError(null, "Object not found", "Could not find a table named: Segments");
				}
				else
				{	
					errors.addErrors(reader.ReadSegment(table));	
				}

				errors.Display();

				this.Parent.Refresh();

				this.Refresh();
			}	

			this.ResumeLayout();
			this.Suspend = false;
		}

        private void computeMeanSize()
        {
            if(!Database.Nimo )
            {
                return;
            }

            packSizeTable.Clear();

            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Rows )
            {
                double total = 0;
                double mps = 0;
                double mup = 0;

                foreach( MrktSimDBSchema.consumer_preferenceRow pref in seg.Getconsumer_preferenceRows() )
                {
                    // get the attribute for this preference
                    MrktSimDBSchema.product_attributeRow attr = pref.product_attributeRow;

                    // sum over products
                    foreach( MrktSimDBSchema.product_attribute_valueRow attrVal in attr.Getproduct_attribute_valueRows() )
                    {
                        MrktSimDBSchema.productRow prod = attrVal.productRow;

                        double size = 0;
                        double numChan = 0;

                        double unitPurchased = seg.avg_max_units_purch;

                        if (prod.pack_size_id != Database.AllID)
                        {
                            unitPurchased = theDb.MeanPackSize( prod.pack_sizeRow );
                        }

                        foreach( MrktSimDBSchema.product_channel_sizeRow chanSize in prod.Getproduct_channel_sizeRows() )
                        {
                            size += chanSize.prod_size;
                            numChan += 1.0;
                        }

                        if( numChan > 0 )
                        {
                            size /= numChan;
                        }

                        if( attrVal.pre_attribute_value > 0 )
                        {
                            mup += unitPurchased * attrVal.pre_attribute_value * Math.Exp( pref.pre_preference_value + attr.cust_tau );
                            mps += size * attrVal.pre_attribute_value * Math.Exp( pref.pre_preference_value + attr.cust_tau );
                            total += Math.Exp( pref.pre_preference_value + attr.cust_tau );
                        }
                    }
                }

                if( total > 0 )
                {
                    mps /= total;

                    mup /= total;
                }

                DataRow pcksz = packSizeTable.NewRow();

                pcksz["segment_id"] = seg.segment_id;
                pcksz[segment_name] = seg.segment_name;
                pcksz[product_size] = mps;
                pcksz[units_purchased] = mup;
                pcksz[volume] = mps * mup;

                packSizeTable.Rows.Add( pcksz );
            }
        }

        private void importBut_Click( object sender, EventArgs e )
        {
            ExcelImport();
        }

        private void importPriceUtilBut_Click( object sender, EventArgs e )
        {
            SegmentPriceUitlityReader priceUtility = new SegmentPriceUitlityReader( theDb );

            System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.DefaultExt = ".xls";
            openFileDlg.Filter = "Excel File (*.xls)|*.xls";

            // openFileDlg.ReadOnlyChecked = false;

            DialogResult frslt = openFileDlg.ShowDialog();

            this.Suspend = true;
            this.SuspendLayout();

            if( frslt == DialogResult.OK )
            {
                string fileName = openFileDlg.FileName;
                ErrorList errors = new ErrorList();

                DataTable table = null;

                errors.addErrors( ExcelReader.ReadTable( fileName, "Segment Price Utility", "IV", true, out table ) );

                if( errors.Count > 0 || table == null )
                {
                    errors.addError( null, "Object not found", "Could not find a table named: Attributes" );
                }
                else
                {
                    errors.addErrors( priceUtility.ReadInPriceUtilities( table ) );
                }

                errors.Display();

                this.Parent.Refresh();

                this.Refresh();
            }

            this.ResumeLayout();
            this.Suspend = false;
        }
	}
}
