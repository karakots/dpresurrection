using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using Common.MsTree;
using Common.Dialogs;

using ExcelInterface;

namespace Common
{
	/// <summary>
	/// Summary description for ChannelControl.
	/// </summary>
	public class PriceControl : MrktSimControl
	{
		private Common.Utilities.MrktSimGrid iPriceGrid;


		public override void Refresh()
		{
			base.Refresh();

			iPriceGrid.Refresh();
		}

		// edits are written into dabatase
		public override void Flush()
		{
			iPriceGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;
				iPriceGrid.Suspend = value;
				mrktPlanComponent.Suspend = value;
			}
		}

		public override void Reset()
		{
			this.createTableStyle();
		}
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PriceControl(Database db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktPlanComponent.Type = PlanType.Price;
			mrktPlanComponent.Db = db;

		
			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			this.SuspendLayout();

			iPriceGrid.Table = theDb.Data.product_channel;
			iPriceGrid.Db = theDb;
			iPriceGrid.RowID = "record_id";
			iPriceGrid.GetRowName +=new Common.Utilities.MrktSimGrid.RowToName(iPriceGrid_GetRowName);


			theDb.Data.product.RowChanged +=new DataRowChangeEventHandler(dataChanged);
			theDb.Data.channel.RowChanged +=new DataRowChangeEventHandler(dataChanged);

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
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.iPriceGrid = new Common.Utilities.MrktSimGrid();
			this.SuspendLayout();
			// 
			// mrktPlanComponent
			// 
			this.mrktPlanComponent.Dock = System.Windows.Forms.DockStyle.Top;
			this.mrktPlanComponent.Location = new System.Drawing.Point(0, 0);
			this.mrktPlanComponent.Name = "mrktPlanComponent";
			this.mrktPlanComponent.SelectedPlan = null;
			this.mrktPlanComponent.Size = new System.Drawing.Size(600, 264);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 1;
			// 
			// iPriceGrid
			// 
			this.iPriceGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iPriceGrid.Location = new System.Drawing.Point(0, 264);
			this.iPriceGrid.Name = "iPriceGrid";
			this.iPriceGrid.RowFilter = "";
			this.iPriceGrid.RowID = null;
			this.iPriceGrid.RowName = null;
			this.iPriceGrid.Size = new System.Drawing.Size(600, 168);
			this.iPriceGrid.Sort = "";
			this.iPriceGrid.TabIndex = 0;
			// 
			// PriceControl
			// 
			this.Controls.Add(this.iPriceGrid);
			this.Controls.Add(this.mrktPlanComponent);
			this.Name = "PriceControl";
			this.Size = new System.Drawing.Size(600, 432);
			this.ResumeLayout(false);

		}
		#endregion

		MrktSimDBSchema.market_planRow curPlan = null;

		// turn a row into a nice name for a parameters
		private string iPriceGrid_GetRowName(DataRow row)
		{
			MrktSimDBSchema.product_channelRow priceItem = (MrktSimDBSchema.product_channelRow) row;

			return priceItem.market_planRow.name + " " + priceItem.start_date.ToShortDateString();
		}

		private void createTableStyle()
		{
			iPriceGrid.Clear();

			iPriceGrid.AddComboBoxColumn(  "market_plan_id", theDb.Data.market_plan, "name", "id", true);

			iPriceGrid.AddTextColumn("productName", true);
			iPriceGrid.AddTextColumn("channelName", true);

			if (theDb.Model.profit_loss)
				iPriceGrid.AddNumericColumn("markup", false);

			DataGridTextBoxColumn price = iPriceGrid.AddNumericColumn("price", false);

			if (theDb.Model.periodic_price)
			{
				DataGridTextBoxColumn periodic = iPriceGrid.AddNumericColumn("periodic_price", false);


				string[] periods = {"Day", "Week", "Month", "Year"};
				iPriceGrid.AddComboBoxColumn("how_often", periods);
			}

			if (theDb.Model.promoted_price)
			{
				string[] purchTypes = {"Unpromoted", "Promoted", "BOGO"};
				iPriceGrid.AddComboBoxColumn("ptype", purchTypes);

				iPriceGrid.AddTextColumn("percent_SKU_in_dist", false);
			}
			
			iPriceGrid.AddDateColumn("start_date");
			iPriceGrid.AddDateColumn("end_date");

			iPriceGrid.Reset();
		}

		private void dataChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (!Suspend)
				iPriceGrid.Reset();
		}

		private void mrktPlanComponent_MarketPlanChanged(int product_id, MrktSimDBSchema.market_planRow plan)
		{
			if (plan != null)
			{
				iPriceGrid.RowFilter = "market_plan_id = " + plan.id;
			}
			else if (product_id != Database.AllID)
			{
				iPriceGrid.RowFilter = "product_id = " + product_id;
			}
			else
			{
				iPriceGrid.RowFilter = "";
			}

			curPlan = plan;
		}

		public override bool AllowCreate()
		{
			return curPlan != null;
		}


		public override void Create()
		{
			CreatePriceData dlg = new CreatePriceData(theDb, curPlan);

			dlg.ShowDialog();
		}

		public override bool AllowPaste()
		{
			return true;
		}

		public override void Paste()
		{
			// ask user for a name for plans
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "MarketPlan";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			string planName = dlg.ObjName;
			string planDesc = dlg.ObjDescription;

			PasteData paste = new PasteData(theDb, planName);

			string error = paste.Paste(PlanType.Price);
				
			if (error != null)
				MessageBox.Show(error);
			
		}

		public override bool AllowExcelImport()
		{
			return true;
		}

		public override void ExcelImport()
		{
			SelectProducts selProdDlg = new SelectProducts(this.theDb);

			selProdDlg.ShowDialog();

			// ask user for a name for plans
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "MarketPlan";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			string planName = dlg.ObjName;
			string planDesc = dlg.ObjDescription;

			ExcelReader excelReader = new ExcelReader(theDb, planName);

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;

			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;

				string error = excelReader.Read(fileName, PlanType.Price);

				if (error != null)
					MessageBox.Show(error);
			}
		}
	}
}
