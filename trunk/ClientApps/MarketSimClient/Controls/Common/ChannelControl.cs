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
	/// Summary description for ChannelControl.
	/// </summary>
	public class ChannelControl : MrktSimControl
	{
		public override void Refresh()
		{
			base.Refresh ();

			prodSizeGrid.Refresh();
			channelSegmentGrid.Refresh();
			
			channelGrid.Refresh();
		}


		// edits are written into dabatase
		public override void Flush()
		{
			channelSegmentGrid.Flush();
			prodSizeGrid.Flush();
			channelGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				channelSegmentGrid.Suspend = value;
				prodSizeGrid.Suspend = value;
				channelGrid.Suspend = value;
			}
		}

		public override void Reset()
		{
			base.Reset ();

			channelSegmentGrid.Reset();
			prodSizeGrid.Reset();

			this.createTableStyle();
		}

		private MrktSimGrid channelGrid;
        private MrktSimGrid channelSegmentGrid;
        private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TabControl channelTabs;
		private System.Windows.Forms.TabPage chanProbPage;
		private System.Windows.Forms.TabPage prodSizePage;
		private MarketSimUtilities.MrktSimGrid prodSizeGrid;
        private SplitContainer splitContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton newChannelBut;
        private ToolStripButton importBut;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ChannelControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();			
 
			channelGrid.Table = theDb.Data.channel;
			channelGrid.RowFilter = "channel_id <> " + AllID;
			channelGrid.DescriptionWindow = false;

			// channelGrid.RowHeaders = false;

			channelSegmentGrid.DataMatrix = new DataMatrix(
				"Segment",
				theDb.Data.segment,
				"segment_id",
				"segment_name",
				theDb.Data.channel, 
				"channel_id",
				"channel_name",
				theDb.Data.segment_channel,
				"probability_of_choice");

			prodSizeGrid.DataMatrix = new DataMatrix(
				"Product",
				theDb.Data.product,
				"product_id",
				"product_name",
				theDb.Data.channel, 
				"channel_id",
				"channel_name",
				theDb.Data.product_channel_size,
				"prod_size");

			createTableStyle();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ChannelControl ) );
            this.channelGrid = new MarketSimUtilities.MrktSimGrid();
            this.channelSegmentGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.channelTabs = new System.Windows.Forms.TabControl();
            this.chanProbPage = new System.Windows.Forms.TabPage();
            this.prodSizePage = new System.Windows.Forms.TabPage();
            this.prodSizeGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newChannelBut = new System.Windows.Forms.ToolStripButton();
            this.importBut = new System.Windows.Forms.ToolStripButton();
            this.channelTabs.SuspendLayout();
            this.chanProbPage.SuspendLayout();
            this.prodSizePage.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelGrid
            // 
            this.channelGrid.DescribeRow = null;
            this.channelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelGrid.EnabledGrid = true;
            this.channelGrid.Location = new System.Drawing.Point( 0, 0 );
            this.channelGrid.Name = "channelGrid";
            this.channelGrid.RowFilter = "";
            this.channelGrid.RowID = null;
            this.channelGrid.RowName = null;
            this.channelGrid.Size = new System.Drawing.Size( 624, 192 );
            this.channelGrid.Sort = "";
            this.channelGrid.TabIndex = 0;
            this.channelGrid.Table = null;
            // 
            // channelSegmentGrid
            // 
            this.channelSegmentGrid.DescribeRow = null;
            this.channelSegmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelSegmentGrid.EnabledGrid = true;
            this.channelSegmentGrid.Location = new System.Drawing.Point( 0, 0 );
            this.channelSegmentGrid.Name = "channelSegmentGrid";
            this.channelSegmentGrid.RowFilter = "";
            this.channelSegmentGrid.RowID = null;
            this.channelSegmentGrid.RowName = null;
            this.channelSegmentGrid.Size = new System.Drawing.Size( 616, 163 );
            this.channelSegmentGrid.Sort = "";
            this.channelSegmentGrid.TabIndex = 1;
            this.channelSegmentGrid.Table = null;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point( 0, 25 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 624, 3 );
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // channelTabs
            // 
            this.channelTabs.Controls.Add( this.chanProbPage );
            this.channelTabs.Controls.Add( this.prodSizePage );
            this.channelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelTabs.Location = new System.Drawing.Point( 0, 0 );
            this.channelTabs.Name = "channelTabs";
            this.channelTabs.SelectedIndex = 0;
            this.channelTabs.Size = new System.Drawing.Size( 624, 189 );
            this.channelTabs.TabIndex = 5;
            // 
            // chanProbPage
            // 
            this.chanProbPage.Controls.Add( this.channelSegmentGrid );
            this.chanProbPage.Location = new System.Drawing.Point( 4, 22 );
            this.chanProbPage.Name = "chanProbPage";
            this.chanProbPage.Size = new System.Drawing.Size( 616, 163 );
            this.chanProbPage.TabIndex = 0;
            this.chanProbPage.Text = "Probability of Choosing a channel";
            // 
            // prodSizePage
            // 
            this.prodSizePage.Controls.Add( this.prodSizeGrid );
            this.prodSizePage.Location = new System.Drawing.Point( 4, 22 );
            this.prodSizePage.Name = "prodSizePage";
            this.prodSizePage.Size = new System.Drawing.Size( 616, 163 );
            this.prodSizePage.TabIndex = 1;
            this.prodSizePage.Text = "Product Size in Channel";
            // 
            // prodSizeGrid
            // 
            this.prodSizeGrid.DescribeRow = null;
            this.prodSizeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prodSizeGrid.EnabledGrid = true;
            this.prodSizeGrid.Location = new System.Drawing.Point( 0, 0 );
            this.prodSizeGrid.Name = "prodSizeGrid";
            this.prodSizeGrid.RowFilter = null;
            this.prodSizeGrid.RowID = null;
            this.prodSizeGrid.RowName = null;
            this.prodSizeGrid.Size = new System.Drawing.Size( 616, 163 );
            this.prodSizeGrid.Sort = "";
            this.prodSizeGrid.TabIndex = 0;
            this.prodSizeGrid.Table = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 28 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.channelGrid );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.channelTabs );
            this.splitContainer1.Size = new System.Drawing.Size( 624, 385 );
            this.splitContainer1.SplitterDistance = 192;
            this.splitContainer1.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importBut,
            this.newChannelBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 624, 25 );
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newChannelBut
            // 
            this.newChannelBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newChannelBut.Image = ((System.Drawing.Image)(resources.GetObject( "newChannelBut.Image" )));
            this.newChannelBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newChannelBut.Name = "newChannelBut";
            this.newChannelBut.Size = new System.Drawing.Size( 84, 22 );
            this.newChannelBut.Text = "New Channel...";
            this.newChannelBut.Click += new System.EventHandler( this.createChannelButton_Click );
            // 
            // importBut
            // 
            this.importBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importBut.Image = ((System.Drawing.Image)(resources.GetObject( "importBut.Image" )));
            this.importBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importBut.Name = "importBut";
            this.importBut.Size = new System.Drawing.Size( 112, 22 );
            this.importBut.Text = "Import Product Size...";
            this.importBut.Click += new System.EventHandler( this.importBut_Click );
            // 
            // ChannelControl
            // 
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "ChannelControl";
            this.Size = new System.Drawing.Size( 624, 413 );
            this.channelTabs.ResumeLayout( false );
            this.chanProbPage.ResumeLayout( false );
            this.prodSizePage.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void createTableStyle()
		{
			this.SuspendLayout();

			channelGrid.Clear();

			channelGrid.AddTextColumn("channel_name");

			channelGrid.Reset();

			this.ResumeLayout(false);
		}

		private void createChannelButton_Click(object sender, System.EventArgs e)
		{
            InputString dlg = new InputString();

            dlg.Text = "Enter Name for Channel";
            dlg.InputText = "";

            DialogResult rslt = dlg.ShowDialog();

            if( rslt != DialogResult.OK )
            {
                return;
            }

            string token = dlg.InputText;

            theDb.CreateChannel( token );

			channelSegmentGrid.Reset();
			prodSizeGrid.Reset();
		}

        private void JJTest1() {
            int totw = 0;
            int col0w = 0;
            for( int c = 0; c < channelSegmentGrid.Columns.Count; c++ ) {
                DataGridColumnStyle col = channelSegmentGrid.Columns[ c ];
                if( c == 0 ){
                    col0w = col.Width;
                }
                totw += col.Width;
                Console.WriteLine( "Col {0} width = {1}", c + 1, col.Width );
            }
            Console.WriteLine( "Total width = {0}", totw );
            if( col0w <= 0 ) {
                channelSegmentGrid.Columns[ 0 ].Width = 50;
            }
        }


		private void ExcelImport()
		{
			ProductSizeReader sizeReader = new ProductSizeReader(theDb);

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
				errors.addErrors(ExcelReader.ReadTable(fileName,"Product Size","Z", true, out table));

				if(errors.Count > 0)
				{
					errors.addError(null, "Object not found", "Could not find a table named: Attributes");
				}
				else
				{
					errors.addErrors(sizeReader.Read(table));
				}

				errors.Display();

				this.Parent.Refresh();

				this.Refresh();
			}	

			this.ResumeLayout();
			this.Suspend = false;
		}

        private void importBut_Click( object sender, EventArgs e )
        {
            ExcelImport();
        }
	}
}
