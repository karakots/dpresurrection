using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Common.Utilities;
using MrktSimDb;
using ErrorInterface;
using ExcelInterface;

using MarketSimUtilities;

namespace ModelView
{
	/// <summary>
	/// Summary description for ExternalData.
	/// </summary>
	public class ExternalData : System.Windows.Forms.Form
	{
		ModelDb theDb;

		public ModelDb Db
		{
			set
			{
				theDb = value;

				mrktSimGrid.Table = theDb.Data.external_data;

				dataType.DataSource = 
                    
                ModelDb.external_data_type;
				dataType.DisplayMember = "descr";
				dataType.ValueMember = "id";

				productPicker1.Db = theDb;
				segmentPicker1.Db = theDb;
				channelPicker1.Db = theDb;

				createTableStyle();
			}
		}

		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox dataType;
		private Common.Utilities.ProductPicker productPicker1;
		private Common.Utilities.SegmentPicker segmentPicker1;
		private Common.Utilities.ChannelPicker channelPicker1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button createData;
		private System.Windows.Forms.Button newDataType;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem saveitem;
		private System.Windows.Forms.MenuItem exitItem;
		private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem excelImpotMenu;
        private IContainer components;

		public ExternalData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ExternalData ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.newDataType = new System.Windows.Forms.Button();
            this.createData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.channelPicker1 = new Common.Utilities.ChannelPicker();
            this.segmentPicker1 = new Common.Utilities.SegmentPicker();
            this.productPicker1 = new Common.Utilities.ProductPicker();
            this.dataType = new System.Windows.Forms.ComboBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu( this.components );
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.saveitem = new System.Windows.Forms.MenuItem();
            this.exitItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.excelImpotMenu = new System.Windows.Forms.MenuItem();
            this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.newDataType );
            this.groupBox1.Controls.Add( this.createData );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.channelPicker1 );
            this.groupBox1.Controls.Add( this.segmentPicker1 );
            this.groupBox1.Controls.Add( this.productPicker1 );
            this.groupBox1.Controls.Add( this.dataType );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 692, 152 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // newDataType
            // 
            this.newDataType.Enabled = false;
            this.newDataType.Location = new System.Drawing.Point( 336, 56 );
            this.newDataType.Name = "newDataType";
            this.newDataType.Size = new System.Drawing.Size( 136, 23 );
            this.newDataType.TabIndex = 8;
            this.newDataType.Text = "Create New Data Type...";
            this.newDataType.Click += new System.EventHandler( this.newDataType_Click );
            // 
            // createData
            // 
            this.createData.Location = new System.Drawing.Point( 336, 112 );
            this.createData.Name = "createData";
            this.createData.Size = new System.Drawing.Size( 136, 23 );
            this.createData.TabIndex = 7;
            this.createData.Text = "Paste in Real Sales";
            this.createData.Click += new System.EventHandler( this.createData_Click );
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 272, 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 56, 23 );
            this.label1.TabIndex = 6;
            this.label1.Text = "Data Type";
            // 
            // channelPicker1
            // 
            this.channelPicker1.ChannelID = -1;
            this.channelPicker1.Location = new System.Drawing.Point( 16, 120 );
            this.channelPicker1.Name = "channelPicker1";
            this.channelPicker1.Size = new System.Drawing.Size( 216, 24 );
            this.channelPicker1.TabIndex = 5;
            // 
            // segmentPicker1
            // 
            this.segmentPicker1.Location = new System.Drawing.Point( 16, 88 );
            this.segmentPicker1.Name = "segmentPicker1";
            this.segmentPicker1.SegmentID = -1;
            this.segmentPicker1.Size = new System.Drawing.Size( 216, 24 );
            this.segmentPicker1.TabIndex = 4;
            // 
            // productPicker1
            // 
            this.productPicker1.AllowAll = false;
            this.productPicker1.leafOnly = false;
            this.productPicker1.Location = new System.Drawing.Point( 16, 24 );
            this.productPicker1.Name = "productPicker1";
            this.productPicker1.ProductID = -1;
            this.productPicker1.Size = new System.Drawing.Size( 216, 56 );
            this.productPicker1.TabIndex = 3;
            this.productPicker1.TypeOnly = false;
            // 
            // dataType
            // 
            this.dataType.Location = new System.Drawing.Point( 352, 24 );
            this.dataType.Name = "dataType";
            this.dataType.Size = new System.Drawing.Size( 121, 21 );
            this.dataType.TabIndex = 9;
            this.dataType.SelectedIndexChanged += new System.EventHandler( this.dataType_SelectedIndexChanged );
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2} );
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.saveitem,
            this.exitItem} );
            this.menuItem1.Text = "File";
            // 
            // saveitem
            // 
            this.saveitem.Index = 0;
            this.saveitem.Text = "Save";
            this.saveitem.Click += new System.EventHandler( this.saveitem_Click );
            // 
            // exitItem
            // 
            this.exitItem.Index = 1;
            this.exitItem.Text = "Exit";
            this.exitItem.Click += new System.EventHandler( this.exitItem_Click );
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.excelImpotMenu} );
            this.menuItem2.Text = "Tools";
            // 
            // excelImpotMenu
            // 
            this.excelImpotMenu.Index = 0;
            this.excelImpotMenu.Text = "Import From Excel...";
            this.excelImpotMenu.Click += new System.EventHandler( this.excelImpotMenu_Click );
            // 
            // mrktSimGrid
            // 
            this.mrktSimGrid.DescribeRow = null;
            this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid.EnabledGrid = true;
            this.mrktSimGrid.Location = new System.Drawing.Point( 0, 152 );
            this.mrktSimGrid.Name = "mrktSimGrid";
            this.mrktSimGrid.RowFilter = null;
            this.mrktSimGrid.RowID = null;
            this.mrktSimGrid.RowName = null;
            this.mrktSimGrid.Size = new System.Drawing.Size( 692, 0 );
            this.mrktSimGrid.Sort = "";
            this.mrktSimGrid.TabIndex = 1;
            this.mrktSimGrid.Table = null;
            // 
            // ExternalData
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 692, 0 );
            this.Controls.Add( this.mrktSimGrid );
            this.Controls.Add( this.groupBox1 );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.Menu = this.mainMenu1;
            this.Name = "ExternalData";
            this.Text = "ExternalData";
            this.Closing += new System.ComponentModel.CancelEventHandler( this.ExternalData_Closing );
            this.groupBox1.ResumeLayout( false );
            this.ResumeLayout( false );

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid.Clear();

			mrktSimGrid.AddComboBoxColumn("type", ModelDb.external_data_type, "descr", "id", true);
			mrktSimGrid.AddComboBoxColumn("product_id", theDb.Data.product, "product_name", "product_id", true);
			mrktSimGrid.AddComboBoxColumn("segment_id", theDb.Data.segment, "segment_name", "segment_id", true);
			mrktSimGrid.AddComboBoxColumn("channel_id", theDb.Data.channel, "channel_name", "channel_id", true);
			mrktSimGrid.AddNumericColumn("value");
			mrktSimGrid.AddDateColumn("calendar_date", true);
			
			mrktSimGrid.Reset();
		}

		private void createData_Click(object sender, System.EventArgs e)
		{
			int prodID = productPicker1.ProductID;
			int segID = segmentPicker1.SegmentID;
			int chanID = channelPicker1.ChannelID;
			int type = (int) dataType.SelectedValue;

			Decimal val = new decimal();
			DateTime date = new DateTime();



			// Create a new instance of the DataObject interface.
			IDataObject data = Clipboard.GetDataObject();
			// If the data is text, then set the text of the 
			// TextBox to the text in the Clipboard.
			if (data.GetDataPresent(DataFormats.Text))
			{
				string text = data.GetData(DataFormats.Text).ToString();

				char[] newLine = {'\r', '\n'};
				char[] tab = {'\t'};
				char[] comma = {',', ' '};

				string[] lines = text.Split(newLine);

				string error = null;
			
				foreach(string line in lines)
				{
					if (line.Length == 0)
						continue;

					bool readDate = false;
					bool readValue = false;
				
					string[] valStrings = line.Split(tab);
					foreach(string valString in valStrings)
					{
						if (!readDate)
						{
							try
							{
								date = DateTime.Parse(valString);
								readDate = true;
								continue;
							}
							catch(System.Exception )
							{
							}
						}

						if (!readValue)
						{
							try
							{
								val = Decimal.Parse(valString);
								readValue = true;
								continue;
							}
							catch(System.Exception)
							{		
							}
						}
					}

					if (!readDate && !readValue)
					{
						error = "Error reading dates and values";
						break;
					}

					// create row in external data table

					MrktSimDBSchema.external_dataRow extData = theDb.CreateExternalData(date, prodID,segID,chanID, type);

					extData.value = (double) val;
				}

				if (error != null)
				{
					MessageBox.Show(error);
				}
			}
		}

		private void newDataType_Click(object sender, System.EventArgs e)
		{
		
		}

		private void saveitem_Click(object sender, System.EventArgs e)
		{
			mrktSimGrid.Flush();
			mrktSimGrid.Suspend = true;
			theDb.Update();
			mrktSimGrid.Suspend = false;
		}

		private void exitItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ExternalData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mrktSimGrid.Flush();
			if (theDb.HasChanges())
			{
				string msg = "Do you wish to save your changes?";

				DialogResult res = MessageBox.Show(this, msg, "Save Changes", MessageBoxButtons.YesNoCancel);

				if( res == DialogResult.Yes)
				{
					// save model
					saveitem_Click(sender, e);
				}
			}
		}

		private void dataType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string filter = "desrc = " + dataType.SelectedText;

		}

		private void excelImpotMenu_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				ErrorList errors = new ErrorList();
				string fileName = openFileDlg.FileName;

				DataTable table;
				errors.addErrors(ExcelReader.ReadTable(fileName, "Real Sales Data", "F", true, out table));
				if(errors.Count > 0)
				{
					errors.Display();
					return;
				}

				this.mrktSimGrid.Suspend = true;
				errors.addErrors(readRealSalesData(table));
				this.mrktSimGrid.Suspend = false;

				if(errors.Count > 0)
				{
					errors.Display();
					return;
				}
			}
		}

		private ErrorList readRealSalesData(DataTable table)
		{
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

			foreach(DataRow row in table.Rows)
			{
				string product = null;
				string segment = null;
				string channel = null;

				string valString = null;
				string dateString = null;

				try
				{
					valString = row["units sold"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find units sales column in " + table.TableName);
					continue;
				}

				try
				{
					dateString = row["date"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find date column in " + table.TableName);
					continue;
				}
	
				try
				{
					product = row["product"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find product column in " + table.TableName);
					continue;
				}

				if (!prodMap.ContainsKey(product))
				{
					// check for product
					string query = "product_name = '" + product + "'";
					DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);

					if (rows.Length != 1)
					{
						errors.addError(table,"Object not found","Could not find product named " + product);
						continue;
					}

					prodMap.Add(product,  ((MrktSimDb.MrktSimDBSchema.productRow) rows[0]).product_id);
				}

				try
				{
					segment = row["segment"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find segment column in " + table.TableName);
					continue;
				}

				if (!segMap.ContainsKey(segment))
				{
					// check for product
					string query = "segment_name = '" + segment + "'";
					DataRow[] rows = theDb.Data.segment.Select(query,"",DataViewRowState.CurrentRows);

					if (rows.Length != 1)
					{
						errors.addError(table,"Object not found","Could not find " + segment);
						continue;
					}

					segMap.Add(segment, ((MrktSimDb.MrktSimDBSchema.segmentRow) rows[0]).segment_id);
				}

				try
				{
					channel = row["channel"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find channel column in " + table.TableName);
					continue;
				}

				if (!chanMap.ContainsKey(channel))
				{
					// check for product
					string query = "channel_name = '" + channel + "'";
					DataRow[] rows = theDb.Data.channel.Select(query,"",DataViewRowState.CurrentRows);

					if (rows.Length != 1)
					{
						errors.addError(table,"Object not found","Could not find " + channel);
						continue;
					}

					chanMap.Add(channel, ((MrktSimDb.MrktSimDBSchema.channelRow) rows[0]).channel_id);
				}

				val = Decimal.Parse(valString);
				date = DateTime.Parse(dateString);
				product_id = (int) prodMap[product];
				segment_id = (int) segMap[segment];
				channel_id = (int) chanMap[channel];

				MrktSimDBSchema.external_dataRow extData = theDb.CreateExternalData(date, product_id,segment_id,channel_id, 1);

				extData.value = (double) val;
			}

			return errors;
		}
	}
}
