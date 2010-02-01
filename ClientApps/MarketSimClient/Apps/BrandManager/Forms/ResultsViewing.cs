using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;

using Results;

using MarketSimUtilities;

using BrandManager.Dialogues;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
	public class ResultsViewing : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Panel panel1;
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Button myResultsButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox myGroupBox;
		private MarketSimUtilities.MrktSimGrid mrktSimGrid1;
		private System.Windows.Forms.GroupBox otherGroupBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.Button SelectMyProduct;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label productLabel;
		private System.Windows.Forms.Button selectOtherProduct;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label product2Label;
		private System.Windows.Forms.Label label3;

		private int my_product_id;

		public ResultsViewing()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktSimGrid.DescribeRow = "descr";
			mrktSimGrid1.DescribeRow = "descr";

			scenarioList = new ArrayList();

			if(Setup.Settings.Products == null)
			{
				Setup.Settings.Products = "0.0";
			}

			this.checkBox4.Checked = false;
			this.mrktSimGrid.EnabledGrid = false;

			//scenFilterBox.SelectedIndex = 0;
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
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.productLabel = new System.Windows.Forms.Label();
			this.SelectMyProduct = new System.Windows.Forms.Button();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.product2Label = new System.Windows.Forms.Label();
			this.selectOtherProduct = new System.Windows.Forms.Button();
			this.myResultsButton = new System.Windows.Forms.Button();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.myGroupBox = new System.Windows.Forms.GroupBox();
			this.mrktSimGrid1 = new MarketSimUtilities.MrktSimGrid();
			this.otherGroupBox = new System.Windows.Forms.GroupBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.myGroupBox.SuspendLayout();
			this.otherGroupBox.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.myResultsButton);
			this.panel1.Controls.Add(this.checkBox4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 224);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 264);
			this.panel1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
			this.textBox1.Location = new System.Drawing.Point(408, 48);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(152, 112);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "Select two products to display.\r\n\r\nYou may also compare with another scenario.\r\n\r" +
				"\nMarketing Data can also be displayed for each scenario.";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.productLabel);
			this.groupBox2.Controls.Add(this.SelectMyProduct);
			this.groupBox2.Controls.Add(this.checkBox5);
			this.groupBox2.Controls.Add(this.checkBox6);
			this.groupBox2.Controls.Add(this.checkBox3);
			this.groupBox2.Controls.Add(this.checkBox2);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Location = new System.Drawing.Point(8, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(376, 128);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Plot Dollar Share and Marketing Mix";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(184, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Display Marketing Data for Product";
			// 
			// productLabel
			// 
			this.productLabel.Location = new System.Drawing.Point(8, 48);
			this.productLabel.Name = "productLabel";
			this.productLabel.Size = new System.Drawing.Size(296, 16);
			this.productLabel.TabIndex = 6;
			this.productLabel.Text = "A long name of selected product - a really long name";
			// 
			// SelectMyProduct
			// 
			this.SelectMyProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.SelectMyProduct.Location = new System.Drawing.Point(8, 16);
			this.SelectMyProduct.Name = "SelectMyProduct";
			this.SelectMyProduct.Size = new System.Drawing.Size(200, 23);
			this.SelectMyProduct.TabIndex = 5;
			this.SelectMyProduct.Text = "Select Product, Brand, or Company...";
			this.toolTip1.SetToolTip(this.SelectMyProduct, "View the dollar share for products in the selected scenario");
			this.SelectMyProduct.Click += new System.EventHandler(this.SelectMyProduct_Click);
			// 
			// checkBox5
			// 
			this.checkBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox5.Location = new System.Drawing.Point(136, 104);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(64, 16);
			this.checkBox5.TabIndex = 4;
			this.checkBox5.Text = "Coupon";
			// 
			// checkBox6
			// 
			this.checkBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox6.Location = new System.Drawing.Point(72, 104);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(56, 16);
			this.checkBox6.TabIndex = 3;
			this.checkBox6.Text = "GRPs";
			// 
			// checkBox3
			// 
			this.checkBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox3.Location = new System.Drawing.Point(280, 104);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(88, 16);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "Distribution";
			// 
			// checkBox2
			// 
			this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox2.Location = new System.Drawing.Point(216, 104);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(64, 16);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Display";
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.Location = new System.Drawing.Point(8, 104);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(64, 16);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Pricing";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.product2Label);
			this.groupBox1.Controls.Add(this.selectOtherProduct);
			this.groupBox1.Location = new System.Drawing.Point(8, 168);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(376, 80);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Plot Dollar Share";
			// 
			// product2Label
			// 
			this.product2Label.Location = new System.Drawing.Point(8, 48);
			this.product2Label.Name = "product2Label";
			this.product2Label.Size = new System.Drawing.Size(304, 16);
			this.product2Label.TabIndex = 9;
			this.product2Label.Text = "A long name of selected product - a really long name";
			// 
			// selectOtherProduct
			// 
			this.selectOtherProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selectOtherProduct.Location = new System.Drawing.Point(8, 16);
			this.selectOtherProduct.Name = "selectOtherProduct";
			this.selectOtherProduct.Size = new System.Drawing.Size(200, 23);
			this.selectOtherProduct.TabIndex = 8;
			this.selectOtherProduct.Text = "Select Product, Brand, or Company...";
			this.toolTip1.SetToolTip(this.selectOtherProduct, "View the dollar share for products in the selected scenario");
			this.selectOtherProduct.Click += new System.EventHandler(this.selectOtherProduct_Click);
			// 
			// myResultsButton
			// 
			this.myResultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.myResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.myResultsButton.Location = new System.Drawing.Point(408, 216);
			this.myResultsButton.Name = "myResultsButton";
			this.myResultsButton.Size = new System.Drawing.Size(152, 23);
			this.myResultsButton.TabIndex = 1;
			this.myResultsButton.Text = "View Results...";
			this.toolTip1.SetToolTip(this.myResultsButton, "View the dollar share for products in the selected scenario");
			this.myResultsButton.Click += new System.EventHandler(this.myResultsButton_Click);
			// 
			// checkBox4
			// 
			this.checkBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox4.Location = new System.Drawing.Point(408, 24);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(144, 16);
			this.checkBox4.TabIndex = 0;
			this.checkBox4.Text = "Compare two scenarios";
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.DescribeRow = null;
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.EnabledGrid = true;
			this.mrktSimGrid.Location = new System.Drawing.Point(3, 16);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(295, 165);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 2;
			this.mrktSimGrid.Table = null;
			// 
			// myGroupBox
			// 
			this.myGroupBox.Controls.Add(this.mrktSimGrid1);
			this.myGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.myGroupBox.Location = new System.Drawing.Point(0, 0);
			this.myGroupBox.Name = "myGroupBox";
			this.myGroupBox.Size = new System.Drawing.Size(288, 184);
			this.myGroupBox.TabIndex = 4;
			this.myGroupBox.TabStop = false;
			this.myGroupBox.Text = "My Scenarios";
			// 
			// mrktSimGrid1
			// 
			this.mrktSimGrid1.DescribeRow = null;
			this.mrktSimGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid1.EnabledGrid = true;
			this.mrktSimGrid1.Location = new System.Drawing.Point(3, 16);
			this.mrktSimGrid1.Name = "mrktSimGrid1";
			this.mrktSimGrid1.RowFilter = null;
			this.mrktSimGrid1.RowID = null;
			this.mrktSimGrid1.RowName = null;
			this.mrktSimGrid1.Size = new System.Drawing.Size(282, 165);
			this.mrktSimGrid1.Sort = "";
			this.mrktSimGrid1.TabIndex = 0;
			this.mrktSimGrid1.Table = null;
			// 
			// otherGroupBox
			// 
			this.otherGroupBox.Controls.Add(this.mrktSimGrid);
			this.otherGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.otherGroupBox.Location = new System.Drawing.Point(291, 0);
			this.otherGroupBox.Name = "otherGroupBox";
			this.otherGroupBox.Size = new System.Drawing.Size(301, 184);
			this.otherGroupBox.TabIndex = 5;
			this.otherGroupBox.TabStop = false;
			this.otherGroupBox.Text = "Select Scenario to compare";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.otherGroupBox);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.myGroupBox);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 40);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(592, 184);
			this.panel2.TabIndex = 7;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(288, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 184);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label5);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(592, 40);
			this.panel3.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(384, 23);
			this.label5.TabIndex = 17;
			this.label5.Text = "Plot Dollar Share and Marketing Mix";
			// 
			// ResultsViewing
			// 
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "ResultsViewing";
			this.Size = new System.Drawing.Size(592, 488);
			this.panel1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.myGroupBox.ResumeLayout(false);
			this.otherGroupBox.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods
		public Database Db
		{
			set
			{
				db = value;

				this.modelDb = new ModelInfoDb();
				this.modelDb.Connection = db.Connection;
				this.modelDb.ProjectID = Setup.Settings.Project;
				this.modelDb.ReadProjects();

				this.mrktSimGrid.Table = value.Data.scenario;
				this.mrktSimGrid1.Table = value.Data.scenario;


				this.mrktSimGrid1.RowFilter = "type = 0 AND user_name = '" + Setup.Settings.User + "' AND sim_num = 0";
				this.mrktSimGrid.RowFilter = "type = 0 AND sim_num = 0";

				// make the table
				createTableStyle();
				update_product_label();
			}

			get
			{
				return db;
			}
		}

		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add AddCompetitor.Next implementation
			return true;
		}

		public bool Back()
		{
			// TODO:  Add AddCompetitor.Back implementation
			return true;
		}

		public void Start()
		{
			db.ReadModelForBrandManager();

		
		}

		public void End()
		{
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region private data and methods
		private Database db = null;
		private ModelInfoDb modelDb = null;
		private ArrayList scenarioList = null;

		private void createTableStyle()
		{
			this.mrktSimGrid.Clear();

			this.mrktSimGrid.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid.AddTextColumn("user_name", "Owner", true);

			this.mrktSimGrid.Reset();

			this.mrktSimGrid1.Clear();

			this.mrktSimGrid1.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid1.AddTextColumn("user_name", "Owner", true);

			this.mrktSimGrid1.Reset();
		}

		#endregion

		private void resultsFormBUtton_Click(object sender, System.EventArgs e)
		{
			ResultsForm resultsForm = new ResultsForm();
			resultsForm.Db = db;
			resultsForm.Project = modelDb;

			scenarioList = mrktSimGrid.GetSelected();

			// for each scenario selected -- make sure only my products are selected
			resultsForm.ClearSelectedProducts();

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
				{
					// only add products for my plans

					MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;

					if (plan.user_name == Setup.Settings.User)
					{
						resultsForm.SelectProduct(plan.product_id);
					}
				}
			}
			
			resultsForm.ShowDialog();
		}

		private void myResultsButton_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			ResultsForm resultsForm = new ResultsForm();
			resultsForm.Db = db;
			resultsForm.Project = modelDb;

			//resultsForm.addEvalByToken("sku_share");
			resultsForm.clearEvals();
			resultsForm.addEvalByToken("dollar_share");
			//resultsForm.addEvalByToken("num_sku_bought");
			//resultsForm.addEvalByToken("sku_dollar_purchased_tick");

			scenarioList.Clear();
			scenarioList.Add(mrktSimGrid1.CurrentRow);
			if(mrktSimGrid.Enabled)
			{
				scenarioList.Add(mrktSimGrid.CurrentRow);
			}

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				this.Cursor = Cursors.Arrow;
				return;
			}


			// for each scenario selected -- make sure only my products are selected
			resultsForm.ClearSelectedProducts();

			string[] product_ids = Setup.Settings.Products.Split('.');
			string query = "product_id = " + product_ids[0];
			DataRow[] rows = Db.Data.product.Select(query, "", DataViewRowState.CurrentRows);

			ProductForest selectedForest = null;
			if(rows.Length > 0)
			{
				selectedForest = resultsForm.SelectProduct(int.Parse(product_ids[0]));
			}
			else
			{
				MessageBox.Show("Error: Please select two valid products","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				this.Cursor = Cursors.Arrow;
				return;
			}

			resultsForm.ClearSelectedProducts();

			query = "product_id = " + product_ids[1];
			rows = Db.Data.product.Select(query, "", DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				selectedForest.CombineForest(resultsForm.SelectProduct(int.Parse(product_ids[1])));
			}
			else
			{
				MessageBox.Show("Error: Please select two valid products","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				this.Cursor = Cursors.Arrow;
				return;
			}

			// fiure out the time range

			DateTime start = new DateTime(9999,1,1);
			DateTime end = new DateTime(1, 1, 1);

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				if (scenario.user_name == Setup.Settings.User)
				{
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			if (start > end)
			{
				// must be looking at strategic scenarios
				foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
				{
					
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			resultsForm.AllProductsOnOneGraph(true);

			MultiGrapher mgraph = resultsForm.GraphNoShow(scenarioList, start, end, 7, selectedForest);

			foreach(Plot plot in mgraph.Plots)
			{
				plot.Title = "Dollar Share";
			}

			mgraph.IndividualGraphs = true;

			resultsForm.ClearSelectedProducts();

			
			query = "product_id = " + product_ids[0];
			rows = Db.Data.product.Select(query, "", DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				resultsForm.SelectProduct(int.Parse(product_ids[0]));
			}
			else
			{
				MessageBox.Show("Error: Please select two valid products","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				
				this.Cursor = Cursors.Arrow;
				return;
			}

			scenarioList.Add(mrktSimGrid1.CurrentRow);
			if(mrktSimGrid.Enabled)
			{
				scenarioList.Add(mrktSimGrid.CurrentRow);
			}

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				this.Cursor = Cursors.Arrow;
				return;
			}

			resultsForm.AllProductsOnOneGraph(true);

			start = new DateTime(9999,1,1);
			end = new DateTime(1, 1, 1);

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				if (scenario.user_name == Setup.Settings.User)
				{
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			if (start > end)
			{
				// must be looking at strategic scenarios
				foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
				{
					
					if (scenario.start_date < start)
						start = scenario.start_date;

				
					if (scenario.end_date > end)
						end = scenario.end_date;
				}
			}

			int ii;
			int startDex;

			if(this.checkBox5.Checked)
			{
				resultsForm.clearEvals();
				resultsForm.addEvalByToken("num_coupon_redemptions");
				resultsForm.addEvalByToken("num_units_bought_on_coupon");
				startDex = mgraph.Plots.Count;
				resultsForm.AddPlotToGraph(mgraph, scenarioList, start, end, 7);

				for(ii = startDex; ii < mgraph.Plots.Count; ii++)
				{
					((Plot) mgraph.Plots[ii]).Title = "Coupons";
				}
			}
			
			resultsForm.switchToMarketing();

			if(this.checkBox1.Checked)
			{
				resultsForm.clearEvals();
				resultsForm.addEvalByToken("unpromoprice");
				resultsForm.addEvalByToken("promoprice");

				startDex = mgraph.Plots.Count;
				resultsForm.AddPlotToGraph(mgraph, scenarioList, start, end, 7);

				for(ii = startDex; ii < mgraph.Plots.Count; ii++)
				{
					((Plot) mgraph.Plots[ii]).Title = "Price";
				}
			}

			if(this.checkBox2.Checked)
			{
				resultsForm.clearEvals();
				resultsForm.addEvalByToken("percent_on_display_sku");
				startDex = mgraph.Plots.Count;
				resultsForm.AddPlotToGraph(mgraph, scenarioList, start, end, 7);

				for(ii = startDex; ii < mgraph.Plots.Count; ii++)
				{
					((Plot) mgraph.Plots[ii]).Title = "Display";
				}
			}

			if(this.checkBox3.Checked)
			{
				resultsForm.clearEvals();
				resultsForm.addEvalByToken("percent_preuse_distribution_sku");
				startDex = mgraph.Plots.Count;
				resultsForm.AddPlotToGraph(mgraph, scenarioList, start, end, 7);

				for(ii = startDex; ii < mgraph.Plots.Count; ii++)
				{
					((Plot) mgraph.Plots[ii]).Title = "Distribution";
				}
			}

			if(this.checkBox6.Checked)
			{
				resultsForm.clearEvals();
				resultsForm.addEvalByToken("GRPs_SKU_tick");
				startDex = mgraph.Plots.Count;
				resultsForm.AddPlotToGraph(mgraph, scenarioList, start, end, 7);

				for(ii = startDex; ii < mgraph.Plots.Count; ii++)
				{
					((Plot) mgraph.Plots[ii]).Title = "GRP";
				}
			}

			

			this.Cursor = Cursors.Arrow;

			mgraph.Show();
		}

		private void scenFilterBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
//			if(scenFilterBox.SelectedIndex == 0)
//			{
//				mrktSimGrid.RowFilter = "user_name = '" + Setup.Settings.User + "' AND sim_num = 0";
//			}
//			else if(scenFilterBox.SelectedIndex == 1)
//			{
//				mrktSimGrid.RowFilter = "user_name = '" + Setup.Settings.User + "' OR user_name = 'admin' AND sim_num = 0";
//			}
//			else
//			{
//				mrktSimGrid.RowFilter = "sim_num = 0";
//			}
			mrktSimGrid.Refresh();
		}

		private void SelectMyProduct_Click(object sender, System.EventArgs e)
		{
			ProductTreePicker dlg = new ProductTreePicker();
			dlg.Db = this.Db;
			dlg.ShowDialog();
			if(dlg.DialogResult == DialogResult.OK)
			{
				my_product_id = (int)dlg.Product["product_id"];
				int index = Setup.Settings.Products.IndexOf(".");
				Setup.Settings.Products = my_product_id.ToString() + Setup.Settings.Products.Substring(index,Setup.Settings.Products.Length - index);
				update_product_label();
			}
		}

		private void selectOtherProduct_Click(object sender, System.EventArgs e)
		{
			ProductTreePicker dlg = new ProductTreePicker();
			dlg.Db = this.Db;
			dlg.ShowDialog();
			if(dlg.DialogResult == DialogResult.OK)
			{
				my_product_id = (int)dlg.Product["product_id"];
				int index = Setup.Settings.Products.IndexOf(".");
				Setup.Settings.Products = Setup.Settings.Products.Substring(0,index+1) + my_product_id.ToString();
				update_product_label();
			}
		}

		private void update_product_label()
		{
			string my_product_name = "";
			string my_product_type = "";
			string other_product_name = "";
			string other_product_type = "";
			int type = -1;
			string[] product_ids = Setup.Settings.Products.Split('.');
			string query = "product_id = " + product_ids[0];
			DataRow[] rows = Db.Data.product.Select(query, "", DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				my_product_name = (string)rows[0]["product_name"];
				type = (int)rows[0]["product_type"];
				query = "id = " + type;
				rows = Db.Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
				my_product_type = (string)rows[0]["type_name"];
			}
			else
			{
				my_product_name = "Nothing";
				my_product_type = "Nothing";
			}
			query = "product_id = " + product_ids[1];
			rows = Db.Data.product.Select(query, "", DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				other_product_name = (string)rows[0]["product_name"];
				type = (int)rows[0]["product_type"];
				query = "id = " + type;
				rows = Db.Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
				other_product_type = (string)rows[0]["type_name"];
			}
			else
			{
				other_product_name = "Nothing";
				other_product_type = "Nothing";
			}
			if(my_product_name != "Nothing")
			{
				this.productLabel.Text = "Selected " + my_product_type + ": " + my_product_name;
			}
			else
			{
				this.productLabel.Text = "Nothing Selected";
			}
			if(other_product_name != "Nothing")
			{
				this.product2Label.Text =  "Selected " + other_product_type + ": " + other_product_name;
			}
			else
			{
				this.product2Label.Text =  "Nothing Selected";
			}
		}

		private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.checkBox4.Checked == true)
			{
				this.mrktSimGrid.EnabledGrid = true;
			}
			else
			{
				this.mrktSimGrid.EnabledGrid = false;
			}
		}


		// truly belongs in MrktSimDb
		// yes it does Isaac 7/7/2006
		/*public void deleteScenario(MrktSimDb.MrktSimDBSchema.scenarioRow scenario)
		{
			// this not only deletes the scenario but deletes the market plan and
			// also deletes market plans components owned by this user

			// start at the components
			foreach(MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
			{
				MrktSimDBSchema.market_planRow topPlan = planRef.market_planRow;

				// if user owns plan then delete it

				if (topPlan.user_name == Setup.Settings.User)
				{
					topPlan.Delete();
				}
			}

			scenario.Delete();

			// cleanup
			// delete all market plan components that are no longer referenced
			string query = "user_name = '" + Setup.Settings.User + "'";
			DataRow[] plans = Db.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

			foreach(MrktSimDBSchema.market_planRow plan in plans)
			{
				// if this is not in a top level plan it needs to be deleted

				if (plan.type != 0)
				{
					// not a top level plan then if it does not have a reference in the market plan tree
					// delete it

					if (plan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_child().Length == 0)
					{
						plan.Delete();
					}
				}
			}
		}*/

	}
}
