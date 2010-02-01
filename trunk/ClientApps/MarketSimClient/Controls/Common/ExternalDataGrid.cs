using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using ErrorInterface;
using ExcelInterface;

using MrktSimDb;
//using Common.Utilities;
using MarketSimUtilities;
using Utilities;

namespace Common
{
	/// <summary>
	/// Summary description for ModelParameter.
	/// </summary>
	public class ExternalDataGrid : MrktSimControl
	{
        private ToolStrip toolStrip1;
        private ToolStripButton folderImportBut;
        private ToolStripButton fileImportBut;

		public override MrktSimDb.ModelDb Db
		{
			set
			{
				base.Db = value;

				mrktSimGrid1.Table = theDb.Data.external_data;
				mrktSimGrid1.DescriptionWindow = false;

				// make sure parameters are up to date
				theDb.UpdateParameters();

				createTableStyle();
			}
		}

		public override void Flush()
		{
			base.Flush ();
			mrktSimGrid1.Flush();
		}

     
        private void ExcelImportFolder() {
            Console.WriteLine( "Excel Import Requested..." );
            System.Windows.Forms.FolderBrowserDialog openFolderDlg = new FolderBrowserDialog();
            openFolderDlg.Description = "Select a folder containing Real Sales data in Excel files.";
            DialogResult frslt = openFolderDlg.ShowDialog();
             if( frslt == DialogResult.OK ) {

                ErrorList errors = new ErrorList();

               this.mrktSimGrid1.Suspend = true;
               string[] filenames = System.IO.Directory.GetFiles( openFolderDlg.SelectedPath, "*.xls", System.IO.SearchOption.TopDirectoryOnly );

                 int c = 1;
                foreach( string fileName in filenames ) {
                    Console.WriteLine( "\n Importing file {0} of {1} -- {2}", c, filenames.Length, fileName );
                    c += 1;

                    DataTable table;
                    errors.addErrors( ExcelReader.ReadTable( fileName, "Real Sales Data", "F", true, out table ) );
                    if( errors.Count > 0 ) {
                        errors.Display();
                        return;
                    }

                    if( table == null ) {
                        return;
                    }

                    errors.addErrors( ReadRealSalesData( table ) );

                    if( errors.Count > 0 ) {
                        errors.Display();
                        this.mrktSimGrid1.Suspend = false;
                        return;
                    }
                }
                this.mrktSimGrid1.Suspend = false;
            }
        }
    
        private void ExcelImport() 
        {
            Console.WriteLine( "Excel Import Requested..." );

            System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.DefaultExt = ".xls";
            openFileDlg.Filter = "Excel File (*.xls)|*.xls";

            DialogResult frslt = openFileDlg.ShowDialog();

            if( frslt == DialogResult.OK ) {
                ErrorList errors = new ErrorList();
                string fileName = openFileDlg.FileName;

                DataTable table;
                errors.addErrors( ExcelReader.ReadTable( fileName, "Real Sales Data", "F", true, out table ) );
                if( errors.Count > 0 ) {
                    errors.Display();
                    return;
                }

                if (table == null)
                {
                    return;
                }

                this.mrktSimGrid1.Suspend = true;
                errors.addErrors( ReadRealSalesData( table ) );
                this.mrktSimGrid1.Suspend = false;

                if( errors.Count > 0 ) {
                    errors.Display();
                    return;
                }
            }
        }

        private ErrorList ReadRealSalesData( DataTable table ) {
            ErrorList errors = new ErrorList();

            Decimal val = new decimal();
            DateTime date = new DateTime();
            int product_id = ModelDb.AllID;
            int segment_id = ModelDb.AllID;
            int channel_id = ModelDb.AllID;


            // create hash tables for products, segments and channels, and types

            Hashtable prodMap = new Hashtable();
            Hashtable segMap = new Hashtable();
            Hashtable chanMap = new Hashtable();

            foreach( DataRow row in table.Rows ) {

                if (row.RowState == DataRowState.Deleted)
                {
                    continue;
                }
                string product = null;
                string segment = null;
                string channel = null;

                string valString = null;
                string dateString = null;

                try {
                    valString = row[ "units sold" ].ToString();
                }
                catch( System.Exception ) {
                    errors.addError( row, "Object not found", "Could not find units sales column in " + table.TableName );
                    continue;
                }

                try {
                    dateString = row[ "date" ].ToString();
                }
                catch( System.Exception ) {
                    errors.addError( row, "Object not found", "Could not find date column in " + table.TableName );
                    continue;
                }

                try {
                    product = row[ "product" ].ToString();
                }
                catch( System.Exception ) {
                    errors.addError( row, "Object not found", "Could not find product column in " + table.TableName );
                    continue;
                }

                if( !prodMap.ContainsKey( product ) ) {
                    // check for product
                    string query = "product_name = '" + product + "'";
                    DataRow[] rows = theDb.Data.product.Select( query, "", DataViewRowState.CurrentRows );

                    if( rows.Length != 1 ) {
                        errors.addError( table, "Object not found", "Could not find product named " + product );
                        continue;
                    }

                    prodMap.Add( product, ((MrktSimDb.MrktSimDBSchema.productRow)rows[ 0 ]).product_id );
                }

                try {
                    segment = row[ "segment" ].ToString();
                }
                catch( System.Exception ) {
                    errors.addError( row, "Object not found", "Could not find segment column in " + table.TableName );
                    continue;
                }

                if( !segMap.ContainsKey( segment ) ) {
                    // check for product
                    string query = "segment_name = '" + segment + "'";
                    DataRow[] rows = theDb.Data.segment.Select( query, "", DataViewRowState.CurrentRows );

                    if( rows.Length != 1 ) {
                        errors.addError( table, "Object not found", "Could not find " + segment );
                        continue;
                    }

                    segMap.Add( segment, ((MrktSimDb.MrktSimDBSchema.segmentRow)rows[ 0 ]).segment_id );
                }

                try {
                    channel = row[ "channel" ].ToString();
                }
                catch( System.Exception ) {
                    errors.addError( row, "Object not found", "Could not find channel column in " + table.TableName );
                    continue;
                }

                if( !chanMap.ContainsKey( channel ) ) {
                    // check for product
                    string query = "channel_name = '" + channel + "'";
                    DataRow[] rows = theDb.Data.channel.Select( query, "", DataViewRowState.CurrentRows );

                    if( rows.Length != 1 ) {
                        errors.addError( table, "Object not found", "Could not find " + channel );
                        continue;
                    }

                    chanMap.Add( channel, ((MrktSimDb.MrktSimDBSchema.channelRow)rows[ 0 ]).channel_id );
                }

                val = Decimal.Parse( valString );
                date = DateTime.Parse( dateString );
                product_id = (int)prodMap[ product ];
                segment_id = (int)segMap[ segment ];
                channel_id = (int)chanMap[ channel ];

                MrktSimDBSchema.external_dataRow extData = theDb.CreateExternalData( date, product_id, segment_id, channel_id, 1 );

                extData.value = (double)val;
            }

            return errors;
        }

		public override bool Suspend
		{
			get
			{
				return base.Suspend;
			}
			set
			{
				base.Suspend = value;
				mrktSimGrid1.Suspend = value;
			}
		}

		public override void Refresh()
		{
			base.Refresh ();
			
			// make sure parameters are up to date
			theDb.UpdateParameters();

			mrktSimGrid1.Refresh();
		}

		public override void Reset()
		{
			base.Refresh ();
			
			// make sure parameters are up to date
			theDb.UpdateParameters();

			mrktSimGrid1.Reset();
        }
        private MrktSimGrid mrktSimGrid1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public ExternalDataGrid()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ExternalDataGrid ) );
            this.mrktSimGrid1 = new MarketSimUtilities.MrktSimGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileImportBut = new System.Windows.Forms.ToolStripButton();
            this.folderImportBut = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mrktSimGrid1
            // 
            this.mrktSimGrid1.DescribeRow = null;
            this.mrktSimGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid1.EnabledGrid = true;
            this.mrktSimGrid1.Location = new System.Drawing.Point( 0, 25 );
            this.mrktSimGrid1.Name = "mrktSimGrid1";
            this.mrktSimGrid1.RowFilter = null;
            this.mrktSimGrid1.RowID = null;
            this.mrktSimGrid1.RowName = null;
            this.mrktSimGrid1.Size = new System.Drawing.Size( 560, 335 );
            this.mrktSimGrid1.Sort = "";
            this.mrktSimGrid1.TabIndex = 0;
            this.mrktSimGrid1.Table = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileImportBut,
            this.folderImportBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 560, 25 );
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileImportBut
            // 
            this.fileImportBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileImportBut.Image = ((System.Drawing.Image)(resources.GetObject( "fileImportBut.Image" )));
            this.fileImportBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileImportBut.Name = "fileImportBut";
            this.fileImportBut.Size = new System.Drawing.Size( 69, 22 );
            this.fileImportBut.Text = "Excel Import";
            this.fileImportBut.Click += new System.EventHandler( this.fileImportBut_Click );
            // 
            // folderImportBut
            // 
            this.folderImportBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.folderImportBut.Image = ((System.Drawing.Image)(resources.GetObject( "folderImportBut.Image" )));
            this.folderImportBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.folderImportBut.Name = "folderImportBut";
            this.folderImportBut.Size = new System.Drawing.Size( 98, 22 );
            this.folderImportBut.Text = "Import From Folder";
            this.folderImportBut.Click += new System.EventHandler( this.folderImportBut_Click );
            // 
            // ExternalDataGrid
            // 
            this.Controls.Add( this.mrktSimGrid1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "ExternalDataGrid";
            this.Size = new System.Drawing.Size( 560, 360 );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid1.Clear();

            mrktSimGrid1.AddTextColumn("type", true);

            mrktSimGrid1.AddComboBoxColumn("product_id", theDb.Data.product, "product_name", "product_id", true);

            mrktSimGrid1.AddComboBoxColumn("channel_id", theDb.Data.channel, "channel_name", "channel_id", true);

            mrktSimGrid1.AddComboBoxColumn("segment_id", theDb.Data.segment, "segment_name", "segment_id", true);

            mrktSimGrid1.AddNumericColumn("value");
            mrktSimGrid1.AddDateColumn("calendar_date", true);
            mrktSimGrid1.Reset();
		}

        private void fileImportBut_Click( object sender, EventArgs e )
        {
            ExcelImport();
        }

        private void folderImportBut_Click( object sender, EventArgs e )
        {
            ExcelImportFolder();
        }
	}
}
