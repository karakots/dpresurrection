using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
// using Common.Utilities;
using MarketSimUtilities.MsTree;
using Common.Dialogs;
using MarketSimUtilities;
using ExcelInterface;
using ErrorInterface;

namespace Common
{
	/// <summary>
	/// Summary description for AttributeControl.
	/// </summary>
	public class AttributeControl : MrktSimControl
	{
		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void DbUpate()
		{
			base.DbUpate ();
		}


		public override void Refresh()
		{
			base.Refresh ();

			if (theDb.Data.product.Select("","",DataViewRowState.CurrentRows).Length == 1)
				this.newValBut.Enabled = false;
			else
                newValBut.Enabled = true;

			
			if (theDb.Data.segment.Select("","",DataViewRowState.CurrentRows).Length == 1)
				this.newPrefbut.Enabled = false;
			else
                newPrefbut.Enabled = true;

			attributeGrid.Refresh();
			iProdAttrGrid.Refresh();
			iSegAttrGrid.Refresh();
		}

		public override void Flush()
		{
			attributeGrid.Flush();
			iProdAttrGrid.Flush();
			iSegAttrGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				attributeGrid.Suspend = value;
				iProdAttrGrid.Suspend = value;
				iSegAttrGrid.Suspend = value;
			}
        }
		private MrktSimGrid iSegAttrGrid;
		private System.Windows.Forms.TabControl attributeTabControl;
        private System.Windows.Forms.TabPage segmentPage;
        private MrktSimGrid attributeGrid;
		private System.Windows.Forms.TabPage productPage;
        private MrktSimGrid iProdAttrGrid;
		private System.Windows.Forms.Splitter splitter1;
        private ToolStrip toolStrip1;
        private ToolStripButton newBut;
        private ToolStripButton newValBut;
        private ToolStripButton newPrefbut;
        private SplitContainer splitContainer1;
        private ToolStripButton importBut;
        private ToolStripSeparator toolStripSeparator1;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AttributeControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// iProdAttrGrid.RowFilter = "product_attribute_id = -1";
			
			this.SuspendLayout();

			attributeGrid.Table = theDb.Data.product_attribute;
			attributeGrid.DescriptionWindow = false;

			attributeGrid.Db = theDb;
			attributeGrid.RowID = "product_attribute_id";
			attributeGrid.RowName = "product_attribute_name";

			iSegAttrGrid.Table = theDb.Data.consumer_preference;
			iProdAttrGrid.Table = theDb.Data.product_attribute_value;

			iSegAttrGrid.Db = theDb;
			iSegAttrGrid.RowID = "record_id";
			iSegAttrGrid.GetRowName += new MarketSimUtilities.MrktSimGrid.RowToName(iSegAttrGrid_GetRowName);

			iProdAttrGrid.Db = theDb;
			iProdAttrGrid.RowID = "record_id";
			iProdAttrGrid.GetRowName +=new MrktSimGrid.RowToName(iProdAttrGrid_GetRowName);

			if (db.Data.product.Select("","",DataViewRowState.CurrentRows).Length == 1)
				this.newValBut.Enabled = false;
			else
                newValBut.Enabled = true;

			
			if (db.Data.segment.Select("","",DataViewRowState.CurrentRows).Length == 1)
				this.newPrefbut.Enabled = false;
			else
                newPrefbut.Enabled = true;


			
			// used to havve these but no more
			// only keeping here as reference
//			prodAttrTable = new DataMatrix("Product", theDb.Data.product, "product_id", "product_name", 
//				theDb.Data.product_attribute, "product_attribute_id", "product_attribute_name", 
//				theDb.Data.product_attribute_value, "pre_attribute_value");
//
//			prodPostAttrTable = new DataMatrix("Product", theDb.Data.product, "product_id", "product_name", 
//				theDb.Data.product_attribute, "product_attribute_id", "product_attribute_name", 
//				theDb.Data.product_attribute_value, "post_attribute_value");
//
//			hasAttributeTable = new DataMatrix("Product", theDb.Data.product, "product_id", "product_name", 
//				theDb.Data.product_attribute, "product_attribute_id", "product_attribute_name", 
//				theDb.Data.product_attribute_value, "has_attribute");
		
			createTableStyle();

			this.ResumeLayout(false);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AttributeControl ) );
            this.attributeGrid = new MarketSimUtilities.MrktSimGrid();
            this.iSegAttrGrid = new MarketSimUtilities.MrktSimGrid();
            this.attributeTabControl = new System.Windows.Forms.TabControl();
            this.productPage = new System.Windows.Forms.TabPage();
            this.iProdAttrGrid = new MarketSimUtilities.MrktSimGrid();
            this.segmentPage = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newBut = new System.Windows.Forms.ToolStripButton();
            this.newValBut = new System.Windows.Forms.ToolStripButton();
            this.newPrefbut = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.importBut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.attributeTabControl.SuspendLayout();
            this.productPage.SuspendLayout();
            this.segmentPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // attributeGrid
            // 
            this.attributeGrid.DescribeRow = null;
            this.attributeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeGrid.EnabledGrid = true;
            this.attributeGrid.Location = new System.Drawing.Point( 0, 0 );
            this.attributeGrid.Name = "attributeGrid";
            this.attributeGrid.RowFilter = "";
            this.attributeGrid.RowID = null;
            this.attributeGrid.RowName = null;
            this.attributeGrid.Size = new System.Drawing.Size( 808, 186 );
            this.attributeGrid.Sort = "";
            this.attributeGrid.TabIndex = 0;
            this.attributeGrid.Table = null;
            // 
            // iSegAttrGrid
            // 
            this.iSegAttrGrid.DescribeRow = null;
            this.iSegAttrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iSegAttrGrid.EnabledGrid = true;
            this.iSegAttrGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iSegAttrGrid.Name = "iSegAttrGrid";
            this.iSegAttrGrid.RowFilter = "";
            this.iSegAttrGrid.RowID = null;
            this.iSegAttrGrid.RowName = null;
            this.iSegAttrGrid.Size = new System.Drawing.Size( 800, 195 );
            this.iSegAttrGrid.Sort = "";
            this.iSegAttrGrid.TabIndex = 1;
            this.iSegAttrGrid.Table = null;
            // 
            // attributeTabControl
            // 
            this.attributeTabControl.Controls.Add( this.productPage );
            this.attributeTabControl.Controls.Add( this.segmentPage );
            this.attributeTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeTabControl.Location = new System.Drawing.Point( 0, 0 );
            this.attributeTabControl.Name = "attributeTabControl";
            this.attributeTabControl.SelectedIndex = 0;
            this.attributeTabControl.Size = new System.Drawing.Size( 808, 182 );
            this.attributeTabControl.TabIndex = 2;
            this.attributeTabControl.SelectedIndexChanged += new System.EventHandler( this.attributeTabControl_SelectedIndexChanged );
            // 
            // productPage
            // 
            this.productPage.Controls.Add( this.iProdAttrGrid );
            this.productPage.Location = new System.Drawing.Point( 4, 22 );
            this.productPage.Name = "productPage";
            this.productPage.Size = new System.Drawing.Size( 800, 156 );
            this.productPage.TabIndex = 1;
            this.productPage.Text = "Product Attribute Values";
            // 
            // iProdAttrGrid
            // 
            this.iProdAttrGrid.DescribeRow = null;
            this.iProdAttrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iProdAttrGrid.EnabledGrid = true;
            this.iProdAttrGrid.Location = new System.Drawing.Point( 0, 0 );
            this.iProdAttrGrid.Name = "iProdAttrGrid";
            this.iProdAttrGrid.RowFilter = null;
            this.iProdAttrGrid.RowID = null;
            this.iProdAttrGrid.RowName = null;
            this.iProdAttrGrid.Size = new System.Drawing.Size( 800, 156 );
            this.iProdAttrGrid.Sort = "";
            this.iProdAttrGrid.TabIndex = 0;
            this.iProdAttrGrid.Table = null;
            // 
            // segmentPage
            // 
            this.segmentPage.Controls.Add( this.iSegAttrGrid );
            this.segmentPage.Location = new System.Drawing.Point( 4, 22 );
            this.segmentPage.Name = "segmentPage";
            this.segmentPage.Size = new System.Drawing.Size( 800, 195 );
            this.segmentPage.TabIndex = 2;
            this.segmentPage.Text = "Consumer Preference";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point( 0, 25 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 808, 3 );
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importBut,
            this.toolStripSeparator1,
            this.newBut,
            this.newValBut,
            this.newPrefbut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 808, 25 );
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newBut
            // 
            this.newBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newBut.Image = ((System.Drawing.Image)(resources.GetObject( "newBut.Image" )));
            this.newBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newBut.Name = "newBut";
            this.newBut.Size = new System.Drawing.Size( 84, 22 );
            this.newBut.Text = "New Attribute...";
            this.newBut.Click += new System.EventHandler( this.createAttrButton_Click );
            // 
            // newValBut
            // 
            this.newValBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newValBut.Image = ((System.Drawing.Image)(resources.GetObject( "newValBut.Image" )));
            this.newValBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newValBut.Name = "newValBut";
            this.newValBut.Size = new System.Drawing.Size( 72, 22 );
            this.newValBut.Text = "New Value...";
            this.newValBut.Click += new System.EventHandler( this.createProdAttrButton_Click );
            // 
            // newPrefbut
            // 
            this.newPrefbut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newPrefbut.Image = ((System.Drawing.Image)(resources.GetObject( "newPrefbut.Image" )));
            this.newPrefbut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newPrefbut.Name = "newPrefbut";
            this.newPrefbut.Size = new System.Drawing.Size( 97, 22 );
            this.newPrefbut.Text = "New Preference...";
            this.newPrefbut.Click += new System.EventHandler( this.createPrefButton_Click );
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
            this.splitContainer1.Panel1.Controls.Add( this.attributeGrid );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.attributeTabControl );
            this.splitContainer1.Size = new System.Drawing.Size( 808, 372 );
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 17;
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // AttributeControl
            // 
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "AttributeControl";
            this.Size = new System.Drawing.Size( 808, 400 );
            this.attributeTabControl.ResumeLayout( false );
            this.productPage.ResumeLayout( false );
            this.segmentPage.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private string iProdAttrGrid_GetRowName(DataRow row)
		{
			MrktSimDBSchema.product_attribute_valueRow paRow = (MrktSimDBSchema.product_attribute_valueRow) row;

			return paRow.productRow.product_name + "_" + paRow.product_attributeRow.product_attribute_name;
		}

		private string iSegAttrGrid_GetRowName(DataRow row)
		{
			MrktSimDBSchema.consumer_preferenceRow paRow = (MrktSimDBSchema.consumer_preferenceRow) row;

			return paRow.segmentRow.segment_name + "_" + paRow.product_attributeRow.product_attribute_name;
		}

		private void createTableStyle()
		{
			// attribute data

			attributeGrid.Clear();

			attributeGrid.AddTextColumn("product_attribute_name", "attribute");

            attributeGrid.AddComboBoxColumn( "type", ModelDb.attribute_type, "type", "id" );

			if (ProjectDb.Nimo)
				attributeGrid.AddNumericColumn("cust_tau");
			
			attributeGrid.Reset();

			//
			// product data
			//

			iProdAttrGrid.Clear();
			iProdAttrGrid.AddTextColumn("product_name", true);
			iProdAttrGrid.AddTextColumn("attribute_name", true);
			iProdAttrGrid.AddCheckBoxColumn("has_attribute", "Assign");

			if (theDb.Model.attribute_pre_and_post)
			{
				iProdAttrGrid.AddNumericColumn("pre_attribute_value");
				iProdAttrGrid.AddNumericColumn("post_attribute_value");
			}
			else
				iProdAttrGrid.AddNumericColumn("pre_attribute_value", "Value");


			iProdAttrGrid.AddDateColumn("start_date");

			iProdAttrGrid.Reset();

			// 
			// segment Data
			// 

			iSegAttrGrid.Clear();
			iSegAttrGrid.AddTextColumn("attribute_name", true);
			iSegAttrGrid.AddTextColumn("segment_name", true);

			if (theDb.Model.attribute_pre_and_post &&
				ProjectDb.Nimo)
			{
				iSegAttrGrid.AddNumericColumn("pre_preference_value");
				iSegAttrGrid.AddNumericColumn("post_preference_value");
			}
			else
				iSegAttrGrid.AddNumericColumn("pre_preference_value", "Preference");

			iSegAttrGrid.AddNumericColumn("price_sensitivity");

			iSegAttrGrid.AddDateColumn("start_date");

			iSegAttrGrid.Reset();
		}

		private void createProdAttrButton_Click(object sender, System.EventArgs e)
		{	
			CreateProductAttribute dlg = new CreateProductAttribute();

			dlg.Db = theDb;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.Cancel)
				return;

			int product_id = dlg.ProductID;
			int attribute_id = dlg.AttributeID;

			DateTime date = dlg.Date;

			if ( product_id != ModelDb.AllID)
				theDb.CreateProductAttributeValue(product_id, attribute_id, date);
			else
			{
				// create for each product
				foreach(DataRow prow in theDb.Data.product.Select("product_id <> " + ModelDb.AllID,"",DataViewRowState.CurrentRows))
				{
					MrktSimDBSchema.productRow prod = (MrktSimDBSchema.productRow) prow;

					theDb.CreateProductAttributeValue(prod.product_id, attribute_id, date);
				}
			}

			this.iProdAttrGrid.Refresh();
		}

		private void createPrefButton_Click(object sender, System.EventArgs e)
		{
			CreateConsumerPreference dlg = new CreateConsumerPreference();

			dlg.Db = theDb;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.Cancel)
				return;

			int segment_id = dlg.SegmentID;
			int attribute_id = dlg.AttributeID;

			DateTime date = dlg.Date;

			if ( segment_id != ModelDb.AllID)
			{
				if (attribute_id != ModelDb.AllID)
				{
					theDb.CreateConsumerPreference(segment_id, attribute_id, date);
				}
				else
				{
					foreach(DataRow arow in theDb.Data.product_attribute.Select("","",DataViewRowState.CurrentRows))
					{
						MrktSimDBSchema.product_attributeRow attr = (MrktSimDBSchema.product_attributeRow) arow;

						theDb.CreateConsumerPreference(segment_id, attr.product_attribute_id, date);
					}
				}
			}
			else
			{
				// create for each product
				foreach(DataRow srow in theDb.Data.segment.Select("segment_id <> " + ModelDb.AllID,"",DataViewRowState.CurrentRows))
				{
					MrktSimDBSchema.segmentRow seg = (MrktSimDBSchema.segmentRow) srow;


					if (attribute_id != ModelDb.AllID)
					{
						theDb.CreateConsumerPreference(seg.segment_id, attribute_id, date);
					}
					else
					{
						foreach(DataRow arow in theDb.Data.product_attribute.Select("","",DataViewRowState.CurrentRows))
						{
							MrktSimDBSchema.product_attributeRow attr = (MrktSimDBSchema.product_attributeRow) arow;

							theDb.CreateConsumerPreference(seg.segment_id, attr.product_attribute_id, date);
						}
					}
				}
			}
		}

	
		private void attributeTabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void createAttrButton_Click(object sender, System.EventArgs e)
		{
			CreateAttribute dlg = new CreateAttribute();

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				for(int i = 0; i < dlg.NumAttributes; ++i)
				{
					MrktSimDBSchema.product_attributeRow attribute = theDb.CreateProductAttribute(dlg.AttributeName, (ModelDb.AttributeType)dlg.Type);

					iProdAttrGrid.Refresh();
					iSegAttrGrid.Refresh();
					attributeGrid.Refresh();
				}
			}
		}


		private void ExcelImport()
		{
		

			// get information on what kind of data to read in
			AttributeReaderForm dlg = new AttributeReaderForm();

			dlg.StartDate = theDb.StartDate;

			bool useProducts = true;
			if (attributeTabControl.SelectedIndex != 0)
			{
				useProducts = false;
			}

			if (useProducts)
			{
				dlg.UseProductValues();
			}
			else
			{
				dlg.UseSegmentValues();
			}

			DialogResult rslt = dlg.ShowDialog();

			if (rslt != DialogResult.OK)
			{
				return;
			}

			DateTime startDate = dlg.StartDate;
			AttributeDataReader.AttributeValueType dataType = dlg.DataType;

			AttributeDataReader attributeReader = new AttributeDataReader(theDb);

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

				if (useProducts)
				{
					errors.addErrors(ExcelReader.ReadTable(fileName,"Attribute Values","IV", true, out table));
				}
				else
				{
					if (dataType == AttributeDataReader.AttributeValueType.PriceUtil)
					{
							errors.addErrors(ExcelReader.ReadTable(fileName,"Attribute Price Sensitivity","IV", true, out table));
					}
					else
					{
						errors.addErrors(ExcelReader.ReadTable(fileName,"Attribute Preference","IV", true, out table));
					}
				}

				if(errors.Count > 0 || table == null)
				{
					errors.addError(null, "Object not found", "Could not find a table named: Attributes");
				}
				else
				{
					if (useProducts)
					{
						errors.addErrors(attributeReader.ReadProductAttributeValues(table, startDate, dataType));
					}
					else
					{
						errors.addErrors(attributeReader.ReadSegmentAttributeValues(table, startDate, dataType));
					}
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
