using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using Common.Dialogs;
using MarketSimUtilities.MsTree;
using ExcelInterface;
using ErrorInterface;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Display InitialConditions
	/// </summary>
	public class InitialConditionControl : MrktSimControl
	{
		public override void Refresh()
		{
			base.Refresh ();

			initialShareGrid.Refresh();
			penetrateGrid.Refresh();
			persuasionGrid.Refresh();
			brandAwareGrid.Refresh();
		}

		// edits are written into dabatase
		public override void Flush()
		{
			initialShareGrid.Flush();
			penetrateGrid.Flush();
			persuasionGrid.Flush();
			brandAwareGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;

				initialShareGrid.Suspend = value;
				penetrateGrid.Suspend = value;
				persuasionGrid.Suspend = value;
				brandAwareGrid.Suspend = value;
			}
		}

		public override void DbUpate()
		{
			base.DbUpate ();

			initialShareTable.CreateTable();
			penetrateTable.CreateTable();
			persuasionTable.CreateTable();
			brandAwareTable.CreateTable();
		}

		public override void Reset()
		{
			initialShareTable.CreateTable();
			penetrateTable.CreateTable();
			persuasionTable.CreateTable();
			brandAwareTable.CreateTable();

			initialShareGrid.Reset();
			penetrateGrid.Reset();
			persuasionGrid.Reset();
			brandAwareGrid.Reset();
		}

		private DataMatrix initialShareTable;
		private DataMatrix penetrateTable;
		private DataMatrix persuasionTable;
        private DataMatrix brandAwareTable;
		private System.Windows.Forms.TabControl initCondTab;
		private System.Windows.Forms.TabPage initSharePage;
		private System.Windows.Forms.TabPage persuasionPage;
		private MrktSimGrid initialShareGrid;
		private System.Windows.Forms.TabPage penetrationPage;
		private System.Windows.Forms.TabPage brandAwarePage;
		private MrktSimGrid penetrateGrid;
		private MrktSimGrid persuasionGrid;
		private MrktSimGrid brandAwareGrid;
        private ToolStrip toolStrip1;
        private ToolStripButton importBut;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InitialConditionControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			initialShareTable = new DataMatrix("Product",
				theDb.Data.product,
				"product_id",
				"product_name",
				theDb.Data.segment,
				"segment_id",
				"segment_name",
				theDb.Data.share_pen_brand_aware,
				"initial_share");

//			initialShareTable.RowFilter = "brand_id = 1";
//			initialShareTable.CreateTable();

			initialShareGrid.DataMatrix = initialShareTable;

			penetrateTable = new DataMatrix("Product",
				theDb.Data.product,
				"product_id",
				"product_name",
				theDb.Data.segment,
				"segment_id",
				"segment_name",
				theDb.Data.share_pen_brand_aware,
				"penetration");
//			penetrateTable.RowFilter = "brand_id = 1";
//			penetrateTable.CreateTable();

			penetrateGrid.DataMatrix = penetrateTable;

			persuasionTable = new DataMatrix("Product",
				theDb.Data.product,
				"product_id",
				"product_name",
				theDb.Data.segment,
				"segment_id",
				"segment_name",
				theDb.Data.share_pen_brand_aware,
				"persuasion");
//			persuasionTable.RowFilter = "brand_id = 1";
//			persuasionTable.CreateTable();

			persuasionGrid.DataMatrix = persuasionTable;

			brandAwareTable = new DataMatrix("Product",
				theDb.Data.product,
				"product_id",
				"product_name",
				theDb.Data.segment,
				"segment_id",
				"segment_name",
				theDb.Data.share_pen_brand_aware,
				"brand_awareness");
//			brandAwareTable.RowFilter = "brand_id = 1";
//			brandAwareTable.CreateTable();
	
			brandAwareGrid.DataMatrix = brandAwareTable;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( InitialConditionControl ) );
            this.initialShareGrid = new MarketSimUtilities.MrktSimGrid();
            this.initCondTab = new System.Windows.Forms.TabControl();
            this.initSharePage = new System.Windows.Forms.TabPage();
            this.penetrationPage = new System.Windows.Forms.TabPage();
            this.penetrateGrid = new MarketSimUtilities.MrktSimGrid();
            this.persuasionPage = new System.Windows.Forms.TabPage();
            this.persuasionGrid = new MarketSimUtilities.MrktSimGrid();
            this.brandAwarePage = new System.Windows.Forms.TabPage();
            this.brandAwareGrid = new MarketSimUtilities.MrktSimGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.importBut = new System.Windows.Forms.ToolStripButton();
            this.initCondTab.SuspendLayout();
            this.initSharePage.SuspendLayout();
            this.penetrationPage.SuspendLayout();
            this.persuasionPage.SuspendLayout();
            this.brandAwarePage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // initialShareGrid
            // 
            this.initialShareGrid.DescribeRow = null;
            this.initialShareGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.initialShareGrid.EnabledGrid = true;
            this.initialShareGrid.Location = new System.Drawing.Point( 0, 0 );
            this.initialShareGrid.Name = "initialShareGrid";
            this.initialShareGrid.RowFilter = "";
            this.initialShareGrid.RowID = null;
            this.initialShareGrid.RowName = null;
            this.initialShareGrid.Size = new System.Drawing.Size( 520, 249 );
            this.initialShareGrid.Sort = "";
            this.initialShareGrid.TabIndex = 0;
            this.initialShareGrid.Table = null;
            // 
            // initCondTab
            // 
            this.initCondTab.Controls.Add( this.initSharePage );
            this.initCondTab.Controls.Add( this.penetrationPage );
            this.initCondTab.Controls.Add( this.persuasionPage );
            this.initCondTab.Controls.Add( this.brandAwarePage );
            this.initCondTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.initCondTab.Location = new System.Drawing.Point( 0, 25 );
            this.initCondTab.Name = "initCondTab";
            this.initCondTab.SelectedIndex = 0;
            this.initCondTab.Size = new System.Drawing.Size( 528, 275 );
            this.initCondTab.TabIndex = 0;
            this.initCondTab.SelectedIndexChanged += new System.EventHandler( this.initCondTab_SelectedIndexChanged );
            // 
            // initSharePage
            // 
            this.initSharePage.Controls.Add( this.initialShareGrid );
            this.initSharePage.Location = new System.Drawing.Point( 4, 22 );
            this.initSharePage.Name = "initSharePage";
            this.initSharePage.Size = new System.Drawing.Size( 520, 249 );
            this.initSharePage.TabIndex = 0;
            this.initSharePage.Text = "Initial Share";
            // 
            // penetrationPage
            // 
            this.penetrationPage.Controls.Add( this.penetrateGrid );
            this.penetrationPage.Location = new System.Drawing.Point( 4, 22 );
            this.penetrationPage.Name = "penetrationPage";
            this.penetrationPage.Size = new System.Drawing.Size( 520, 250 );
            this.penetrationPage.TabIndex = 2;
            this.penetrationPage.Text = "Initial Penetration";
            // 
            // penetrateGrid
            // 
            this.penetrateGrid.DescribeRow = null;
            this.penetrateGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.penetrateGrid.EnabledGrid = true;
            this.penetrateGrid.Location = new System.Drawing.Point( 0, 0 );
            this.penetrateGrid.Name = "penetrateGrid";
            this.penetrateGrid.RowFilter = "";
            this.penetrateGrid.RowID = null;
            this.penetrateGrid.RowName = null;
            this.penetrateGrid.Size = new System.Drawing.Size( 520, 250 );
            this.penetrateGrid.Sort = "";
            this.penetrateGrid.TabIndex = 0;
            this.penetrateGrid.Table = null;
            // 
            // persuasionPage
            // 
            this.persuasionPage.Controls.Add( this.persuasionGrid );
            this.persuasionPage.Location = new System.Drawing.Point( 4, 22 );
            this.persuasionPage.Name = "persuasionPage";
            this.persuasionPage.Size = new System.Drawing.Size( 520, 250 );
            this.persuasionPage.TabIndex = 1;
            this.persuasionPage.Text = "Initial Persuasion";
            // 
            // persuasionGrid
            // 
            this.persuasionGrid.DescribeRow = null;
            this.persuasionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.persuasionGrid.EnabledGrid = true;
            this.persuasionGrid.Location = new System.Drawing.Point( 0, 0 );
            this.persuasionGrid.Name = "persuasionGrid";
            this.persuasionGrid.RowFilter = "";
            this.persuasionGrid.RowID = null;
            this.persuasionGrid.RowName = null;
            this.persuasionGrid.Size = new System.Drawing.Size( 520, 250 );
            this.persuasionGrid.Sort = "";
            this.persuasionGrid.TabIndex = 0;
            this.persuasionGrid.Table = null;
            // 
            // brandAwarePage
            // 
            this.brandAwarePage.Controls.Add( this.brandAwareGrid );
            this.brandAwarePage.Location = new System.Drawing.Point( 4, 22 );
            this.brandAwarePage.Name = "brandAwarePage";
            this.brandAwarePage.Size = new System.Drawing.Size( 520, 250 );
            this.brandAwarePage.TabIndex = 3;
            this.brandAwarePage.Text = "Initial Brand Awareness";
            // 
            // brandAwareGrid
            // 
            this.brandAwareGrid.DescribeRow = null;
            this.brandAwareGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandAwareGrid.EnabledGrid = true;
            this.brandAwareGrid.Location = new System.Drawing.Point( 0, 0 );
            this.brandAwareGrid.Name = "brandAwareGrid";
            this.brandAwareGrid.RowFilter = "";
            this.brandAwareGrid.RowID = null;
            this.brandAwareGrid.RowName = null;
            this.brandAwareGrid.Size = new System.Drawing.Size( 520, 250 );
            this.brandAwareGrid.Sort = "";
            this.brandAwareGrid.TabIndex = 0;
            this.brandAwareGrid.Table = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 528, 25 );
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // importBut
            // 
            this.importBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importBut.Image = ((System.Drawing.Image)(resources.GetObject( "importBut.Image" )));
            this.importBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importBut.Name = "importBut";
            this.importBut.Size = new System.Drawing.Size( 49, 22 );
            this.importBut.Text = "Import...";
            this.importBut.Click += new System.EventHandler( this.importBut_Click );
            // 
            // InitialConditionControl
            // 
            this.Controls.Add( this.initCondTab );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "InitialConditionControl";
            this.Size = new System.Drawing.Size( 528, 300 );
            this.initCondTab.ResumeLayout( false );
            this.initSharePage.ResumeLayout( false );
            this.penetrationPage.ResumeLayout( false );
            this.persuasionPage.ResumeLayout( false );
            this.brandAwarePage.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void initCondTab_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			initialShareGrid.Flush();
			initialShareGrid.Refresh();

			penetrateGrid.Flush();
			penetrateGrid.Refresh();

			persuasionGrid.Flush();
			persuasionGrid.Refresh();

			brandAwareGrid.Flush();
			brandAwareGrid.Refresh();
		
		}

		private void ExcelImport()
		{
		

			// get information on what kind of data to read in
			InitialConditionsReaderForm dlg = new InitialConditionsReaderForm();

			DialogResult rslt = dlg.ShowDialog();

			if(rslt != DialogResult.OK)
			{
				return;
			}

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;

			DialogResult frslt = openFileDlg.ShowDialog();

			this.Suspend = true;
			this.SuspendLayout();

			if(frslt != DialogResult.OK)
			{
				this.ResumeLayout();
				this.Suspend = false;
				return;
			}

			this.Cursor = Cursors.WaitCursor;

			InitialConditionsReader ICReader = new InitialConditionsReader(theDb);

			string filename = openFileDlg.FileName;

			ErrorList errors = new ErrorList();
			DataTable table;

			switch(dlg.ValueType)
			{
				case 1:
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Share","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Share));
					break;
				case 2:
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Penetration","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Penetration));
					break;
				case 3:
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Persuasion","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Persuasion));
					break;
				case 4:
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Awareness","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Awareness));
					break;
				case 0:
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Share","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Share));
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Penetration","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Penetration));
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Persuasion","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Persuasion));
					errors.addErrors(ExcelReader.ReadTable(filename,"Initial Awareness","IV",true, out table));
					errors.addErrors(ICReader.ReadInitialConditions(table,InitialConditionsReader.InitialConditionValueType.Awareness));
					break;
				default:
					errors.addError(new Error(null, "Unknown Type", "Unknown type used"));
					break;
			}

			errors.Display();

			this.Parent.Refresh();
			this.Refresh();

			this.ResumeLayout();
			this.Suspend = false;
			this.Cursor = Cursors.Arrow;

		}

        private void importBut_Click( object sender, EventArgs e )
        {
            ExcelImport();
        }


	}
}
