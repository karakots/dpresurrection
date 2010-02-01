using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Utilities;
using Common.Dialogs;
using ExcelInterface;
using ErrorInterface;
using MrktSimDb;

using MarketSimUtilities;

namespace Common
{
	/// <summary>
	/// Summary description for MarketSummary.
	/// </summary>
	public class MarketSummary : MrktSimControl
	{
		public override void Refresh()
		{
			base.Refresh ();

			// do this in case plans were edited
			MrktSimDBSchema.market_planRow plan = mrktPlanComponent.SelectedPlan;
			mrktPlanComponent_MarketPlanChanged(null, plan);
			mrktPlanComponent.ShowAllCheckBox = false;
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
				mrktPlanComponent.Suspend = value;
			}
		}

		public override void Reset()
		{
			base.Reset ();
		}

		DataTable selectedTable;
		DataTable availableTable;

		private System.Windows.Forms.GroupBox dataBox;
		private Common.Utilities.MrktPlanComponent mrktPlanComponent;
		private System.Data.DataView aPriceView;
		private System.Data.DataView aDistView;
		private System.Data.DataView aDisplayView;
		private System.Data.DataView aMediaView;
		private System.Data.DataView componentView;
		private System.Windows.Forms.Button removePlanButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private MrktSimGrid selectedGrid;
		private System.Windows.Forms.Panel panel1;
		private System.Data.DataView aUtilityView;
		private MrktSimGrid availableGrid;
		private System.Windows.Forms.Button addPlanButton;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MarketSummary(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// selected Table and Grid
			selectedTable = new DataTable();
			selectedTable.TableName = "selectedTable";
			DataColumn key = selectedTable.Columns.Add("id", typeof(int));
			selectedTable.Columns.Add("name", typeof(string));
			selectedTable.Columns.Add("product_id", typeof(int));
			selectedTable.Columns.Add("channel_id", typeof(int));
			selectedTable.Columns.Add("segment_id", typeof(int));
			selectedTable.Columns.Add("type", typeof(string));
			selectedTable.PrimaryKey = new DataColumn[] {key};
			
			selectedGrid.Table = selectedTable;
			selectedGrid.AllowDelete = false;
			selectedGrid.DescriptionWindow = false;

			selectedGrid.Clear();

			selectedGrid.AddComboBoxColumn("product_id", theDb.Data.product, "product_name", "product_id", true);
			selectedGrid.AddComboBoxColumn("type", ModelDb.market_plan_type, "type", "id", true);
			selectedGrid.AddTextColumn("name", true);
			
			selectedGrid.Reset();

			// available Table and Grid
			availableTable = new DataTable();
			availableTable.TableName = "availableTable";
			DataColumn availkey = availableTable.Columns.Add("id", typeof(int));
			availableTable.Columns.Add("name", typeof(string));
			availableTable.Columns.Add("product_id", typeof(int));
			availableTable.Columns.Add("channel_id", typeof(int));
			availableTable.Columns.Add("segment_id", typeof(int));
			availableTable.Columns.Add("type", typeof(string));
			availableTable.PrimaryKey = new DataColumn[] {availkey};
			
			availableGrid.Table = availableTable;
			availableGrid.AllowDelete = false;
			availableGrid.DescriptionWindow = false;

			availableGrid.Clear();

			availableGrid.AddComboBoxColumn("product_id", theDb.Data.product, "product_name", "product_id", true);
			availableGrid.AddComboBoxColumn("type", ModelDb.market_plan_type, "type", "id", true);
			availableGrid.AddTextColumn("name", true);
			
			availableGrid.Reset();

			mrktPlanComponent.Type = ModelDb.PlanType.MarketPlan;
			mrktPlanComponent.Db = db;

			mrktPlanComponent.MarketPlanChanged +=new Common.Utilities.MrktPlanComponent.CurrentMarketPlan(mrktPlanComponent_MarketPlanChanged);

			// not sure why I am needing to force this
			MrktSimDBSchema.market_planRow plan = mrktPlanComponent.SelectedPlan;
			mrktPlanComponent_MarketPlanChanged(null, plan);
			mrktPlanComponent.ShowAllCheckBox = false;
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
			this.dataBox = new System.Windows.Forms.GroupBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.selectedGrid = new MrktSimGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.removePlanButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.availableGrid = new MrktSimGrid();
			this.panel2 = new System.Windows.Forms.Panel();
			this.addPlanButton = new System.Windows.Forms.Button();
			this.aPriceView = new System.Data.DataView();
			this.aDisplayView = new System.Data.DataView();
			this.aMediaView = new System.Data.DataView();
			this.aDistView = new System.Data.DataView();
			this.aUtilityView = new System.Data.DataView();
			this.mrktPlanComponent = new Common.Utilities.MrktPlanComponent();
			this.componentView = new System.Data.DataView();
			this.dataBox.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.aPriceView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.aDisplayView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.aMediaView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.aDistView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.aUtilityView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.componentView)).BeginInit();
			this.SuspendLayout();
			// 
			// dataBox
			// 
			this.dataBox.Controls.Add(this.splitter1);
			this.dataBox.Controls.Add(this.groupBox2);
			this.dataBox.Controls.Add(this.groupBox3);
			this.dataBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataBox.Location = new System.Drawing.Point(0, 280);
			this.dataBox.Name = "dataBox";
			this.dataBox.Size = new System.Drawing.Size(808, 160);
			this.dataBox.TabIndex = 1;
			this.dataBox.TabStop = false;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(376, 16);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 141);
			this.splitter1.TabIndex = 25;
			this.splitter1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.selectedGrid);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(376, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(429, 141);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Active Plan Components";
			// 
			// selectedGrid
			// 
			this.selectedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedGrid.Location = new System.Drawing.Point(3, 16);
			this.selectedGrid.Name = "selectedGrid";
			this.selectedGrid.RowFilter = null;
			this.selectedGrid.RowID = null;
			this.selectedGrid.RowName = null;
			this.selectedGrid.Size = new System.Drawing.Size(423, 82);
			this.selectedGrid.Sort = "";
			this.selectedGrid.TabIndex = 20;
			this.selectedGrid.Table = null;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.removePlanButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 98);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(423, 40);
			this.panel1.TabIndex = 21;
			// 
			// removePlanButton
			// 
			this.removePlanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removePlanButton.Location = new System.Drawing.Point(72, 8);
			this.removePlanButton.Name = "removePlanButton";
			this.removePlanButton.Size = new System.Drawing.Size(144, 23);
			this.removePlanButton.TabIndex = 19;
			this.removePlanButton.Text = "Remove Plan Component";
			this.removePlanButton.Click += new System.EventHandler(this.removePlanButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.availableGrid);
			this.groupBox3.Controls.Add(this.panel2);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox3.Location = new System.Drawing.Point(3, 16);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(373, 141);
			this.groupBox3.TabIndex = 24;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Available Plans";
			// 
			// availableGrid
			// 
			this.availableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableGrid.Location = new System.Drawing.Point(3, 16);
			this.availableGrid.Name = "availableGrid";
			this.availableGrid.RowFilter = null;
			this.availableGrid.RowID = null;
			this.availableGrid.RowName = null;
			this.availableGrid.Size = new System.Drawing.Size(367, 82);
			this.availableGrid.Sort = "";
			this.availableGrid.TabIndex = 22;
			this.availableGrid.Table = null;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.addPlanButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(3, 98);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(367, 40);
			this.panel2.TabIndex = 24;
			// 
			// addPlanButton
			// 
			this.addPlanButton.Location = new System.Drawing.Point(80, 8);
			this.addPlanButton.Name = "addPlanButton";
			this.addPlanButton.Size = new System.Drawing.Size(136, 24);
			this.addPlanButton.TabIndex = 23;
			this.addPlanButton.Text = "Add Plan Component";
			this.addPlanButton.Click += new System.EventHandler(this.addPlanButton_Click);
			// 
			// mrktPlanComponent
			// 
			this.mrktPlanComponent.Dock = System.Windows.Forms.DockStyle.Top;
			this.mrktPlanComponent.Location = new System.Drawing.Point(0, 0);
			this.mrktPlanComponent.Name = "mrktPlanComponent";
			this.mrktPlanComponent.SelectedPlan = null;
			this.mrktPlanComponent.Size = new System.Drawing.Size(808, 280);
			this.mrktPlanComponent.Suspend = false;
			this.mrktPlanComponent.TabIndex = 0;
			this.mrktPlanComponent.Type = ModelDb.PlanType.MarketPlan;
			// 
			// MarketSummary
			// 
			this.Controls.Add(this.dataBox);
			this.Controls.Add(this.mrktPlanComponent);
			this.Name = "MarketSummary";
			this.Size = new System.Drawing.Size(808, 440);
			this.dataBox.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.aPriceView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.aDisplayView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.aMediaView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.aDistView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.aUtilityView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.componentView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void initializeTables(MrktSimDBSchema.market_planRow plan)
		{
			selectedTable.Clear();

			initializeAvailableTable(plan);
		}

		private bool planHasComponent(MrktSimDBSchema.market_planRow plan, int compID)
		{
			string query = "parent_id = " + plan.id + " AND child_id = " + compID;
			return theDb.Data.market_plan_tree.Select(query,"", DataViewRowState.CurrentRows).Length > 0;
		}

		private void initializeAvailableTable(MrktSimDBSchema.market_planRow plan)
		{
			// remove all plans of this type from the selected table
			
			availableTable.Clear();

			if (plan == null)
				return;

			string query = "product_id = " + plan.product_id;

			query += " AND type <> " + ((int) ModelDb.PlanType.MarketPlan).ToString();
			query += " AND  type <> " + ((int) ModelDb.PlanType.ProdEvent).ToString();
			query += " AND  type <> " + ((int) ModelDb.PlanType.TaskEvent).ToString();

			DataRow[] rows = theDb.Data.market_plan.Select(query,"", DataViewRowState.CurrentRows);
			foreach(DataRow row in rows)
			{
//				availableTable.Columns.Add("name", typeof(string));
//				availableTable.Columns.Add("product_id", typeof(int));
//				availableTable.Columns.Add("channel_id", typeof(int));
//				availableTable.Columns.Add("segment_id", typeof(int));
//				availableTable.Columns.Add("type", typeof(string));

				// Object[] tableVals = { row["id"], row["name"], row["product_id"]};
				Object[] vals = { row["id"], row["name"], row["product_id"], row["channel_id"], row["segment_id"], row["type"]};

				if (planHasComponent(plan, (int) row["id"]))
				{
					// if plan not in selected table then add
					if (selectedTable.Rows.Find(row["id"]) == null)
						selectedTable.Rows.Add(vals);
				}
				else
					availableTable.Rows.Add(vals);
			}
		}

		private void initializeTable(MrktSimDBSchema.market_planRow plan, DataTable table, int type)
		{
			// remove all plans of this type from the selected table
			
			table.Clear();

			if (plan == null)
				return;

			string query = "type = " + type + " AND product_id = " + plan.product_id;

			DataRow[] rows = theDb.Data.market_plan.Select(query,"", DataViewRowState.CurrentRows);
			foreach(DataRow row in rows)
			{
				Object[] tableVals = { row["id"], row["name"], row["product_id"]};
				Object[] vals = { row["id"], row["name"], row["product_id"], row["channel_id"], row["segment_id"], row["type"]};

				if (planHasComponent(plan, (int) row["id"]))
				{
					// if plan not in selected table then add
					if (selectedTable.Rows.Find(row["id"]) == null)
						selectedTable.Rows.Add(vals);
				}
				else
					table.Rows.Add(tableVals);
			}
		}

//		private void initializeSelectedTable(MrktSimDBSchema.market_planRow plan)
//		{
//			// price plans
//			
//			selectedTable.Clear();
//
//			if (plan == null)
//				return;
//
//			string query = "parent_id = " + plan.id;
//
//			DataRow[] rows = theDb.Data.market_plan_tree.Select(query,"", DataViewRowState.CurrentRows);
//			foreach(DataRow row in rows)
//			{
//				int compID = (int) row["child_id"];
//
//				MrktSimDBSchema.market_planRow comp = theDb.Data.market_plan.FindByid(compID);
//
//				Object[] vals = {comp.name, comp.product_id, comp.id};
//
//				selectedTable.Rows.Add(vals);
//			}
//		}

		private void mrktPlanComponent_MarketPlanChanged(ArrayList prodList, MrktSimDBSchema.market_planRow plan)
		{
			initializeTables(plan);
//
//			// nudge componentBox
//			CurrencyManager man = componentBox.BindingContext[componentView] as CurrencyManager;
//
//			if (man != null)
//				man.Refresh();
		}

		private void removePlanButton_Click(object sender, System.EventArgs e)
		{
			ArrayList items = selectedGrid.GetSelected();

			MrktSimDBSchema.market_planRow plan = mrktPlanComponent.SelectedPlan;

			foreach(DataRow item in items)
			{
				// this should be a method in db
				int compID = (int) item["id"];

				string query = "parent_id = " + plan.id;
				query += " AND child_id = " + compID;

				DataRow[] rows = theDb.Data.market_plan_tree.Select(query,"", DataViewRowState.CurrentRows);

				foreach(DataRow row in rows)
				{
					MrktSimDBSchema.market_plan_treeRow assoc = (MrktSimDBSchema.market_plan_treeRow) row;
			
					assoc.Delete();
				}
			}

			initializeTables(mrktPlanComponent.SelectedPlan);
		}

		private void addPlan(MrktSimDBSchema.market_planRow compPlan)
		{
			if (compPlan == null)
				return;

			MrktSimDBSchema.market_planRow plan = mrktPlanComponent.SelectedPlan;

			if (plan == null)
				return;

			theDb.CreatePlanRelation(plan, compPlan);
//
//			// nudge componentBox
//			CurrencyManager man = componentBox.BindingContext[componentView] as CurrencyManager;
//
//			if (man != null)
//				man.Refresh();
		}

		private void addPlanButton_Click(object sender, System.EventArgs e)
		{
			ArrayList items = availableGrid.GetSelected();

			MrktSimDBSchema.market_planRow plan = mrktPlanComponent.SelectedPlan;

			foreach(DataRow item in items)
			{
				// this should be a method in db
				int compID = (int) item["id"];

				MrktSimDBSchema.market_planRow comp = theDb.Data.market_plan.FindByid(compID);

				addPlan(comp);
			}

			initializeAvailableTable(mrktPlanComponent.SelectedPlan);
		}

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

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			
			openFileDlg.Multiselect = true;

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				PlanReader planReader = new PlanReader(theDb, planName);

				ErrorList errors = new ErrorList();

				using(ProcessStatus process = new ProcessStatus())
				{
					process.Text = "Importing Market Plans";
					process.ProcessType = "Importing:";
				
					// progress.Location = this.Location;
					process.Show();
					
					ModelDb.PlanType[] types = new ModelDb.PlanType[] { ModelDb.PlanType.Price,
														  ModelDb.PlanType.Display,
														  ModelDb.PlanType.Distribution,
														  ModelDb.PlanType.Coupons,
														  ModelDb.PlanType.Market_Utility,
														  ModelDb.PlanType.Media };

					double numPlans = openFileDlg.FileNames.Length * types.Length;
					double curIndex = 0;

					foreach(string fileName in openFileDlg.FileNames)
					{

						int trimDex = fileName.LastIndexOf(@"\");
						int fLength = fileName.Length - trimDex - 1;

						string fName = null;
						
						if (fLength > 0)
						{
							fName = fileName.Substring(trimDex + 1,fLength);
						}
						
						string errorMsg = ExcelReader.CheckIfFileOpen(fileName);
						if (errorMsg != null)
						{
							MessageBox.Show(errorMsg,"File busy",MessageBoxButtons.OK,MessageBoxIcon.Error);
							continue;
						}

						foreach(ModelDb.PlanType type in types)
						{
							process.Progress( fName + "      " + type, curIndex/numPlans);
							curIndex++;

							errors.addErrors(planReader.CreatePlan(fileName, type, false));
						}

						planReader.createTopLevelMarketPlan(null);

						if(errors.Count > 0)
						{
							errors.addError(null, "Errors found in plan: " + fName, "Processing stopped");
							break;
						}
					}
				}

				errors.Display();
			}

		}
	}
}
