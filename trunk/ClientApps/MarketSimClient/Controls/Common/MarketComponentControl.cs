using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;
using Common.Dialogs;

using ExcelInterface;
using ErrorInterface;

using MarketSimUtilities;

namespace Common
{
	/// <summary>
	/// Summary description for MarketComponentControl.
	/// </summary>
	public class MarketComponentControl : MrktSimControl
	{
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Splitter splitter1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MarketComponentControl(ModelDb db, ModelDb.PlanType planType) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = planType;
			mrktPlanComponent.Db = db;

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			this.SuspendLayout();

			switch(planType)
			{
				case ModelDb.PlanType.Price:
						mrktSimGrid.Table = theDb.Data.product_channel;
					break;

				case ModelDb.PlanType.Media:
					mrktSimGrid.Table = theDb.Data.mass_media;
					break;
				
				case ModelDb.PlanType.Coupons:
					mrktSimGrid.Table = theDb.Data.mass_media;
					break;

				case ModelDb.PlanType.Display:
					mrktSimGrid.Table = theDb.Data.display;
					break;

				case ModelDb.PlanType.Distribution:
					mrktSimGrid.Table = theDb.Data.distribution;
					break;

				case ModelDb.PlanType.ProdEvent:
					mrktSimGrid.Table = theDb.Data.product_event;
					break;

				case ModelDb.PlanType.Market_Utility:
					mrktSimGrid.Table = theDb.Data.market_utility;
					break;

			
			}

			mrktSimGrid.RowFilter = defaultPlanRowFilter();

			mrktSimGrid.Db = theDb;
			mrktSimGrid.RowID = "record_id";
			mrktSimGrid.GetRowName +=new MrktSimGrid.RowToName(mrktSimGrid_GetRowName);


			theDb.Data.product.RowChanged +=new DataRowChangeEventHandler(dataChanged);
			theDb.Data.channel.RowChanged +=new DataRowChangeEventHandler(dataChanged);

			createTableStyle();			
			
			this.ResumeLayout(false);

			mrktPlanComponent.ShowAll = false;
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
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.mrktSimGrid = new MrktSimGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// mrktPlanComponent
			// 
			this.mrktPlanComponent.Dock = System.Windows.Forms.DockStyle.Top;
			this.mrktPlanComponent.Location = new System.Drawing.Point(0, 0);
			this.mrktPlanComponent.Name = "mrktPlanComponent";
			this.mrktPlanComponent.SelectedPlan = null;
			this.mrktPlanComponent.Size = new System.Drawing.Size(472, 256);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 0;
			this.mrktPlanComponent.Type = MrktSimDb.ModelDb.PlanType.MarketPlan;
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.Location = new System.Drawing.Point(0, 256);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(472, 216);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 1;
			this.mrktSimGrid.Table = null;
			// 
			// splitter1
			// 
			this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 256);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(472, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// MarketComponentControl
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.mrktSimGrid);
			this.Controls.Add(this.mrktPlanComponent);
			this.Name = "MarketComponentControl";
			this.Size = new System.Drawing.Size(472, 472);
			this.ResumeLayout(false);

		}
		#endregion

		#region MarktSim Control Interface
		public override void Refresh()
		{
			base.Refresh();

			mrktSimGrid.Refresh();
		}

		// edits are written into dabatase
		public override void Flush()
		{
			mrktSimGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				mrktSimGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}

		public override void Reset()
		{
			this.createTableStyle();
		}

		
		public override bool AllowCreate()
		{
			return curPlan != null;
		}


		public override void Create()
		{
			switch(mrktPlanComponent.Type)
			{
				case ModelDb.PlanType.Price:
					CreatePriceData priceDlg = new CreatePriceData(theDb, curPlan);
					priceDlg.ShowDialog();
					break;

				case ModelDb.PlanType.Media:
					CreateMediaData mediaDlg = new CreateMediaData(theDb, curPlan);
					mediaDlg.ShowDialog();
					break;

				case ModelDb.PlanType.Coupons:
					CreateCouponData couponDlg = new CreateCouponData(theDb, curPlan);
					couponDlg.ShowDialog();
					break;

				case ModelDb.PlanType.Display:
					CreateDisplayData displayDlg = new CreateDisplayData(theDb, curPlan);
					displayDlg.ShowDialog();
					break;

				case ModelDb.PlanType.Distribution:
					CreateDistributionData distDlg = new CreateDistributionData(theDb, curPlan);
					distDlg.ShowDialog();
					break;

				case ModelDb.PlanType.ProdEvent:
					CreateEventData eventDlg = new CreateEventData(theDb, curPlan);
					eventDlg.ShowDialog();
					break;

				case ModelDb.PlanType.Market_Utility:
					CreateMarketUtility utilDlg = new CreateMarketUtility(theDb, curPlan);
					utilDlg.ShowDialog();
					break;
			}		
		}

		public override bool AllowPaste()
		{
			return false;
		}

		/*public override void Paste()
		{
			// ask user for a name for plans
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "MarketPlan";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			string planName = dlg.ObjName;
			string planDesc = dlg.ObjDescription;

			PastePlanData paste = new PastePlanData(theDb, planName);

			IDataObject data = Clipboard.GetDataObject();

			// If the data is text, then set the text of the 
			// TextBox to the text in the Clipboard.
			if (data.GetDataPresent(DataFormats.Text))
			{
				string text = data.GetData(DataFormats.Text).ToString();
				string error = paste.Paste(text, mrktPlanComponent.Type);
				
				if (error != null)
					MessageBox.Show(error);
			}
		}*/

		private void ExcelImport()
		{
			// ask user for a name for plans
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "MarketPlan";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			string planName = dlg.ObjName;
			string planDesc = dlg.ObjDescription;

			PlanReader planReader = new PlanReader(theDb, planName);

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;

			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;

                ErrorList errors = planReader.CreatePlan(fileName, mrktPlanComponent.Type, true);

				errors.Display();
			}

			//string error = , mrktPlanComponent.Type);

		}

		#endregion

		MrktSimDBSchema.market_planRow curPlan = null;

		private void dataChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (!Suspend)
				mrktSimGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(ArrayList prodList, MrktSimDBSchema.market_planRow plan)
		{
			string defaultFilter = defaultPlanRowFilter();
			string filter = "";

			if (plan != null)
			{
				filter = " market_plan_id = " + plan.id;

			}
			else if (prodList != null)
			{
				bool firstTime = true;
				foreach(DataRow row in prodList)
				{
					if (firstTime)
					{
						filter = "product_id = " + row["product_id"];
						firstTime = false;
					}
					else
					{
						filter += " OR product_id = " + row["product_id"];
					}
				}
			}

			if (defaultFilter.Length > 0 && filter.Length > 0)
			{
				mrktSimGrid.RowFilter = defaultFilter + " AND (" + filter + ")";
			}
			else
			{
				mrktSimGrid.RowFilter = defaultFilter + filter;
			}

			curPlan = plan;
		}

		
		// turn a row into a nice name for a parameters
		private string mrktSimGrid_GetRowName(DataRow row)
		{
			int market_plan_id = (int)row["market_plan_id"];
			DataRow market_plan_row = theDb.Data.market_plan.Select("id = " + market_plan_id, "", DataViewRowState.CurrentRows)[0];
			return market_plan_row["name"] + " " + ((DateTime) row["start_date"]).ToShortDateString();
		}

		
		#region Depends on Plan Type

		private string defaultPlanRowFilter()
		{
			string filter = null;

			if (mrktPlanComponent.Type == ModelDb.PlanType.Media)
			{
				filter = "(media_type = 'V' OR media_type = 'A')";
			}
			else if (mrktPlanComponent.Type == ModelDb.PlanType.Coupons)
			{
				filter = "(media_type = 'C' OR media_type = 'S' OR media_type = 'B')";
			}
			else
			{
				filter = "";
			}

			return filter;
		}

		private void createTableStyle()
		{
			CreateTableStyle(theDb, mrktSimGrid, mrktPlanComponent.Type);
		}

		static public void CreateTableStyle(ModelDb theDb, MrktSimGrid grid, ModelDb.PlanType type)
		{
			switch(type)
			{
				case ModelDb.PlanType.Price:
					createPriceColumns(theDb, grid);
					break;

				case ModelDb.PlanType.Media:
					createMediaColumns(theDb, grid);
					break;

				case ModelDb.PlanType.Coupons:
					createCouponColumns(theDb, grid);
					break;

				case ModelDb.PlanType.Display:
					createDisplayColumns(theDb, grid);
					break;

				case ModelDb.PlanType.Distribution:
					createDistributionColumns(theDb, grid);
					break;

				case ModelDb.PlanType.ProdEvent:
					createEventColumns(theDb, grid);
					break;

				case ModelDb.PlanType.Market_Utility:
					createUtilityColumns(theDb, grid);
					break;
			}		
		}

		static private void createPriceColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			grid.AddTextColumn("productName", true);
			grid.AddTextColumn("channelName", true);

			if (theDb.Model.profit_loss)
				grid.AddNumericColumn("markup", false);

			DataGridTextBoxColumn price = grid.AddNumericColumn("price", false);

			if (theDb.Model.periodic_price)
			{
				DataGridTextBoxColumn periodic = grid.AddNumericColumn("periodic_price", false);


				string[] periods = {"Day", "Week", "Month", "Year"};
				grid.AddComboBoxColumn("how_often", periods);
			}

			if (theDb.Model.promoted_price)
			{
                // no longer used
                //string[] purchTypes = {"Unpromoted", "Promoted", "BOGO", "Z"};
                //grid.AddComboBoxColumn("ptype", purchTypes);

				grid.AddTextColumn("percent_SKU_in_dist", false);
			}
			
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");

			grid.Reset();
		}


		static private void createMediaColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);


			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("segment_name", true);
			grid.AddTextColumn("channel_name", true);
			
			string[] types = {"V","A"};
			grid.AddComboBoxColumn("media_type", types);
			grid.AddNumericColumn("attr_value_G");
			grid.AddNumericColumn("message_awareness_probability");
			grid.AddNumericColumn("message_persuation_probability");
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");

			grid.Reset();

		}


		static private void createCouponColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("segment_name", true);
			grid.AddTextColumn("channel_name", true);
			
			string[] types = {"C","S", "B"};
			grid.AddComboBoxColumn("media_type", types);

			grid.AddNumericColumn("attr_value_G", "Percent Receiving");
			grid.AddNumericColumn("attr_value_I");
			grid.AddNumericColumn("message_awareness_probability");
			grid.AddNumericColumn("message_persuation_probability");
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");

			grid.Reset();

		}

		static private void createDisplayColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("channel_name", true);

			grid.AddNumericColumn("attr_value_F", false);
			
			grid.AddNumericColumn("message_awareness_probability", false);
			grid.AddNumericColumn("message_persuation_probability", false);
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");
			
			grid.Reset();
		}

		static private void createDistributionColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("channel_name", true);

			grid.AddNumericColumn("attr_value_F", false);
			grid.AddNumericColumn("attr_value_G", false);
			
			grid.AddNumericColumn("message_awareness_probability", false);
			grid.AddNumericColumn("message_persuation_probability", false);
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");
			
			grid.Reset();
		}

		static private void createEventColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();
		
			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("segment_name", true);
			grid.AddTextColumn("channel_name", true);
			grid.AddComboBoxColumn("type", ModelDb.product_event_type, "type", "type_id");
			grid.AddNumericColumn("demand_modification");
			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");

			grid.Reset();
		}

		static private void createUtilityColumns(ModelDb theDb, MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			grid.AddTextColumn("product_name", true);
			grid.AddTextColumn("channel_name", true);
			grid.AddTextColumn("segment_name", true);
			
			grid.AddNumericColumn("percent_dist", false);
			grid.AddNumericColumn("awareness", false);
			grid.AddNumericColumn("persuasion", false);
			grid.AddNumericColumn("utility", false);

			grid.AddDateColumn("start_date");
			grid.AddDateColumn("end_date");
			
			grid.Reset();
		}
		#endregion
	}
}
