using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using MrktSimDb.Metrics;

using Results;

using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
	public class ResultsToFile : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Panel panel1;
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.TextBox fileName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numWeeks;
		private System.Windows.Forms.Button writeToFile;
		private System.Windows.Forms.ComboBox scenFilterBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.RadioButton writeSeries;
		private System.Windows.Forms.RadioButton writeMetric;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label5;
		private System.ComponentModel.IContainer components;

		public ResultsToFile()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktSimGrid.DescribeRow = "descr";

			scenarioList = new ArrayList();

			scenFilterBox.SelectedIndex = 0;
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
			this.scenFilterBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.writeToFile = new System.Windows.Forms.Button();
			this.writeMetric = new System.Windows.Forms.RadioButton();
			this.numWeeks = new System.Windows.Forms.NumericUpDown();
			this.fileName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.writeSeries = new System.Windows.Forms.RadioButton();
			this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel3 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWeeks)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.scenFilterBox);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.writeToFile);
			this.panel1.Controls.Add(this.writeMetric);
			this.panel1.Controls.Add(this.numWeeks);
			this.panel1.Controls.Add(this.fileName);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.writeSeries);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.ImeMode = System.Windows.Forms.ImeMode.On;
			this.panel1.Location = new System.Drawing.Point(0, 176);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 152);
			this.panel1.TabIndex = 1;
			// 
			// scenFilterBox
			// 
			this.scenFilterBox.Items.AddRange(new object[] {
															   "My Scenarios",
															   "Baseline",
															   "All"});
			this.scenFilterBox.Location = new System.Drawing.Point(80, 8);
			this.scenFilterBox.Name = "scenFilterBox";
			this.scenFilterBox.Size = new System.Drawing.Size(128, 21);
			this.scenFilterBox.TabIndex = 11;
			this.scenFilterBox.Text = "comboBox1";
			this.toolTip1.SetToolTip(this.scenFilterBox, "Filters scenario list");
			this.scenFilterBox.SelectedIndexChanged += new System.EventHandler(this.scenFilterBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 8);
			this.label2.Name = "label2";
			this.label2.TabIndex = 12;
			this.label2.Text = "View:";
			// 
			// writeToFile
			// 
			this.writeToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.writeToFile.Location = new System.Drawing.Point(24, 80);
			this.writeToFile.Name = "writeToFile";
			this.writeToFile.Size = new System.Drawing.Size(128, 23);
			this.writeToFile.TabIndex = 10;
			this.writeToFile.Text = "Write to File";
			this.toolTip1.SetToolTip(this.writeToFile, "Writes the data to the file");
			this.writeToFile.Click += new System.EventHandler(this.writeToFile_Click);
			// 
			// writeMetric
			// 
			this.writeMetric.Location = new System.Drawing.Point(280, 32);
			this.writeMetric.Name = "writeMetric";
			this.writeMetric.Size = new System.Drawing.Size(152, 24);
			this.writeMetric.TabIndex = 8;
			this.writeMetric.Text = "Write summary data";
			this.toolTip1.SetToolTip(this.writeMetric, "Write high level summary data to file");
			this.writeMetric.Visible = false;
			// 
			// numWeeks
			// 
			this.numWeeks.Location = new System.Drawing.Point(384, 8);
			this.numWeeks.Maximum = new System.Decimal(new int[] {
																	 52,
																	 0,
																	 0,
																	 0});
			this.numWeeks.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.numWeeks.Name = "numWeeks";
			this.numWeeks.Size = new System.Drawing.Size(40, 20);
			this.numWeeks.TabIndex = 6;
			this.numWeeks.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.numWeeks.Visible = false;
			// 
			// fileName
			// 
			this.fileName.Location = new System.Drawing.Point(216, 80);
			this.fileName.Name = "fileName";
			this.fileName.Size = new System.Drawing.Size(256, 20);
			this.fileName.TabIndex = 2;
			this.fileName.Text = "textBox1";
			this.toolTip1.SetToolTip(this.fileName, "File to write results to");
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(160, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "File Name:";
			// 
			// writeSeries
			// 
			this.writeSeries.Checked = true;
			this.writeSeries.Location = new System.Drawing.Point(240, 8);
			this.writeSeries.Name = "writeSeries";
			this.writeSeries.Size = new System.Drawing.Size(248, 24);
			this.writeSeries.TabIndex = 7;
			this.writeSeries.TabStop = true;
			this.writeSeries.Text = "Write time series every                    week(s)";
			this.toolTip1.SetToolTip(this.writeSeries, "Writes dollar share time series data to file");
			this.writeSeries.Visible = false;
			this.writeSeries.CheckedChanged += new System.EventHandler(this.writeSeries_CheckedChanged);
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.DescribeRow = null;
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.EnabledGrid = true;
			this.mrktSimGrid.Location = new System.Drawing.Point(0, 40);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(496, 136);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 2;
			this.mrktSimGrid.Table = null;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label5);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(496, 40);
			this.panel3.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(384, 23);
			this.label5.TabIndex = 17;
			this.label5.Text = "Export Scenario Summary";
			// 
			// ResultsToFile
			// 
			this.Controls.Add(this.mrktSimGrid);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "ResultsToFile";
			this.Size = new System.Drawing.Size(496, 328);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numWeeks)).EndInit();
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

				this.mrktSimGrid.Table = value.Data.scenario;

				this.mrktSimGrid.RowFilter = "user_name = '" + Setup.Settings.User + "' AND sim_num = 0";

				// make the table
				createTableStyle();
			}

			get
			{
				return db;
			}
		}

		public EditScenario EditScenarioControl
		{
			set
			{
				inter = value;
			}
		}

		#endregion

		#region Wizard Members

		public bool Next()
		{
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

			string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Year.ToString();

			this.fileName.Text = Setup.Settings.User + "s Results " + time;
		}

		public void End()
		{
			
			
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region private data and methods
		Database db = null;
		EditScenario inter = null;
		ArrayList scenarioList = null;

		private void createTableStyle()
		{
			this.mrktSimGrid.Clear();

			this.mrktSimGrid.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid.AddTextColumn("user_name", "Owner", true);

			this.mrktSimGrid.Reset();
		}
		
		private void writeTimeSeriesResultsToCVS()
		{
			ModelInfoDb modeldb = new ModelInfoDb();
			modeldb.Connection = db.Connection;
			modeldb.ProjectID = Setup.Settings.Project;

			ResultsForm resultsForm = new ResultsForm();
			resultsForm.Db = db;
			resultsForm.Project = modeldb;
			resultsForm.addEvalByToken("sku_share");
			resultsForm.addEvalByToken("dollar_share");
			resultsForm.addEvalByToken("num_sku_bought");
			resultsForm.addEvalByToken("sku_dollar_purchased_tick");
			scenarioList = mrktSimGrid.GetSelected();

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			//Select Products
			resultsForm.ClearSelectedProducts();

			bool noProductSelected = true;

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
				{
					// only add products for my plans

					MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;

					if (plan.user_name == Setup.Settings.User)
					{
						resultsForm.SelectProduct(plan.product_id);
						noProductSelected = false;
					}
				}
			}

			if(noProductSelected)
			{
				DialogResult rslt = MessageBox.Show("None of the market plans for the selected scenarios are associated with your user.  Would you like to view results for all products? /nWarning: For large models this can take a while to compute.","No Products",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
				if(rslt == DialogResult.OK)
				{
					foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
					{
						foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
						{
							MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;
							resultsForm.SelectProduct(plan.product_id);
						}
					}
				}
				else
				{
					return;
				}
			}

			this.Cursor = Cursors.WaitCursor;

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

			int numDays = (int)(this.numWeeks.Value * 7);

			MultiGrapher graph = resultsForm.GraphNoShow(scenarioList, start, end, numDays);

			System.IO.StreamWriter writer;

			// any open erros are due to file being in use
			try
			{
				writer = new System.IO.StreamWriter(fileName.Text);
			}
			catch(System.IO.IOException oops)
			{
				MessageBox.Show(oops.Message);
				return;
			}

			if(scenarioList.Count == 1)
			{
				MrktSimDb.MrktSimDBSchema.scenarioRow scenario = (MrktSimDb.MrktSimDBSchema.scenarioRow)scenarioList[0];
				writer.Write(scenario.name);
				writer.Write(writer.NewLine);
				writer.Write(scenario.descr);
				writer.Write(writer.NewLine);
			}

			graph.write(writer);

			if(scenarioList.Count == 1)
			{
				MrktSimDb.MrktSimDBSchema.scenarioRow scenario = (MrktSimDb.MrktSimDBSchema.scenarioRow)scenarioList[0];
				inter.CurrentScenario = scenario;
				writer.Write(writer.NewLine);
				writer.Write("Data Modifications:");
				writer.Write(writer.NewLine);
				writer.Write(inter.ParameterList);
			}

			writer.Close();

			this.Cursor = Cursors.Arrow;
			
		}

		/*private void writeMetricsToFile()
		{
			ModelInfoDb modeldb = new ModelInfoDb();
			modeldb.Connection = db.Connection;
			modeldb.ProjectID = Setup.Settings.Project;

			ResultsForm resultsForm = new ResultsForm();
			resultsForm.Db = db;
			resultsForm.Project = modeldb;
			
			scenarioList = mrktSimGrid.GetSelected();

			if (scenarioList.Count == 0)
			{
				MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			//Select Products
			resultsForm.ClearSelectedProducts();

			bool noProductSelected = true;

			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
			{
				foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
				{
					// only add products for my plans

					MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;

					if (plan.user_name == Setup.Settings.User)
					{
						resultsForm.SelectProduct(plan.product_id);
						noProductSelected = false;
					}
				}
			}

			if(noProductSelected)
			{
				DialogResult rslt = MessageBox.Show("None of the market plans for the selected scenarios are associated with your user.  Would you like to view results for all products? /nWarning: For large models this can take a while to compute.","No Products",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
				if(rslt == DialogResult.OK)
				{
					foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
					{
						foreach(MrktSimDb.MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
						{
							MrktSimDb.MrktSimDBSchema.market_planRow plan = planRef.market_planRow;
							resultsForm.SelectProduct(plan.product_id);
						}
					}
				}
				else
				{
					return;
				}
			}

			System.IO.StreamWriter writer;

			// any open erros are due to file being in use
			try
			{
				writer = new System.IO.StreamWriter(fileName.Text);
			}
			catch(System.IO.IOException oops)
			{
				MessageBox.Show(oops.Message);
				return;
			}

			if(scenarioList.Count == 1)
			{
				MrktSimDb.MrktSimDBSchema.scenarioRow scenario = (MrktSimDb.MrktSimDBSchema.scenarioRow)scenarioList[0];
				writer.Write(scenario.name);
				writer.Write(writer.NewLine);
				writer.Write(scenario.descr);
				writer.Write(writer.NewLine);
			}

			resultsForm.SelectMetric((Metric)this.metricBox.SelectedItem);

			resultsForm.WriteSummaryToCSV(scenarioList, writer);

			if(scenarioList.Count == 1)
			{
				MrktSimDb.MrktSimDBSchema.scenarioRow scenario = (MrktSimDb.MrktSimDBSchema.scenarioRow)scenarioList[0];
				inter.CurrentScenario = scenario;
				writer.Write(inter.ParameterList);
			}


			writer.Close();
		}*/

		private void writeSeries_CheckedChanged(object sender, System.EventArgs e)
		{
			numWeeks.Enabled = writeSeries.Checked;
		}

		/*private void Browse_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveFileDlg = new SaveFileDialog();

			saveFileDlg.DefaultExt = ".csv";
			string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Year.ToString();
			saveFileDlg.FileName = Setup.Settings.User + "s Results " + time;
			saveFileDlg.InitialDirectory = Setup.Settings.LocalDirectory;
			saveFileDlg.Filter = "CSV File (*.csv)|*.csv";

			saveFileDlg.CheckFileExists = false;
			//saveFileDlg.ReadOnlyChecked = false;

			DialogResult rslt = saveFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string filename = saveFileDlg.FileName;

				this.fileName.Text = filename;

			}
		}*/

		private void writeToFile_Click(object sender, System.EventArgs e)
		{
			if(this.writeSeries.Checked == true && mrktSimGrid.GetSelected().Count == 0)
			{
				MessageBox.Show("Please select scenario to save","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			if(this.writeSeries.Checked == true && fileName.Text == "")
			{
				MessageBox.Show("Please select a file to save your results too.","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			writeSeries.Checked = false;

			if(this.writeSeries.Checked)
			{
				this.writeTimeSeriesResultsToCVS();
			}
			else
			{
			//	this.WriteToExcel();
			}
			return;
		}


		#endregion

		private void scenFilterBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			mrktSimGrid.Suspend = true;
			if(scenFilterBox.SelectedIndex == 0)
			{
				mrktSimGrid.RowFilter = "type = 0 AND user_name = '" + Setup.Settings.User + "' AND sim_num = 0";
			}
			else if(scenFilterBox.SelectedIndex == 1)
			{
				mrktSimGrid.RowFilter = "type = 0 AND user_name = '" + Setup.Settings.User + "' OR user_name = 'all' AND sim_num = 0";
			}
			else
			{
				mrktSimGrid.RowFilter = "type = 0 AND sim_num = 0";
			}
			mrktSimGrid.Refresh();
			mrktSimGrid.Suspend = false;
			this.ResumeLayout();

		}

        //private void WriteToExcel()
        //{
        //    ModelInfoDb modeldb = new ModelInfoDb();
        //    modeldb.Connection = db.Connection;
        //    modeldb.ProjectID = Setup.Settings.Project;

        //    ResultsForm resultsForm = new ResultsForm();
        //    resultsForm.Db = db;
        //    resultsForm.Project = modeldb;

        //    scenarioList = mrktSimGrid.GetSelected();

        //    if (scenarioList.Count == 0)
        //    {
        //        MessageBox.Show("Error: There are no scenarios selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //        return;
        //    }


        //    // fiure out the time range
        //    DateTime start = new DateTime(9999,1,1);
        //    DateTime end = new DateTime(1, 1, 1);

        //    foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in scenarioList)
        //    {
        //        if (scenario.user_name == Setup.Settings.User)
        //        {
        //            if (scenario.start_date < start)
        //                start = scenario.start_date;

				
        //            if (scenario.end_date > end)
        //                end = scenario.end_date;
        //        }
        //    }

        //    // tell the results form
        //    resultsForm.SetStartEnd(start, end);

        //    this.Cursor = Cursors.WaitCursor;

        //    resultsForm.SelectMetric((Metric)MetricMan.SimSummaryMetrics[2]);

        //    ExcelInterface.ExcelWriter writer = new ExcelInterface.ExcelWriter();

        //    build_product_list();

        //    ArrayList table_list = resultsForm.GetMetricTables(scenarioList);

        //    ArrayList sim_order = new ArrayList();
        //    ArrayList channel_order = new ArrayList();

        //    foreach(DataTable table in table_list)
        //    {
        //        foreach(DataRow row in table.Rows)
        //        {
        //            productTree.AddRow((string)row["product_name"],row);
        //            string lookup = (string)row["scenario"];
        //            if(!sim_order.Contains(lookup))
        //            {
        //                sim_order.Add(lookup);
        //            }
        //            if(!channel_order.Contains(row["channel_name"]))
        //            {
        //                channel_order.Add(row["channel_name"]);
        //            }
        //        }
        //    }
        //    channel_order.Add("All");

        //    resultsForm.SelectMetric((Metric)MetricMan.SimSummaryMetrics[1]);
        //    table_list = resultsForm.GetMetricTables(scenarioList);
        //    foreach(DataTable table in table_list)
        //    {
        //        table.Columns.Add("channel_name");
        //        foreach(DataRow row in table.Rows)
        //        {
        //            row["channel_name"] = "All";
        //            productTree.AddRow((string)row["product_name"],row);
        //        }
        //    }



        //    sim_order.Sort();
        //    channel_order.Sort();

        //    ArrayList parents = productTree.GetParents();

        //    int index = 5;

        //    string file = Setup.Settings.LocalDirectory + "\\" + fileName.Text;
        //    file = file.Replace("csv","xls");
        //    try
        //    {
        //        writer.Start(file, "Total Dollars");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("There was an error starting Excel, please make you the correct Excel libraries are installed.","Excel Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //        this.Cursor = Cursors.Arrow;
        //        return;
        //    }

        //    try
        //    {
        //        setupSheet(writer, "Total Dollars", sim_order, channel_order, start, end);
        //        foreach(productTree parent in parents)
        //        {
        //            writer.RowBold(index+1,true);
        //            printToExcel(writer, parent, ref index, "", "num_dollars", sim_order, channel_order);
        //        }
        //        finishSheet(writer, index, sim_order, channel_order);

        //        index = 5;
        //        writer.NewSheet("Dollar Share");
        //        setupSheet(writer, "Dollar Share", sim_order, channel_order, start, end);
        //        foreach(productTree parent in parents)
        //        {
        //            writer.RowBold(index+1,true);
        //            printToExcel(writer, parent, ref index, "", "dollar_share", sim_order, channel_order);
        //        }
        //        finishSheet(writer, index, sim_order, channel_order);

        //        index = 5;
        //        writer.NewSheet("Unit Sales");
        //        setupSheet(writer, "Unit Sales", sim_order, channel_order, start, end);
        //        foreach(productTree parent in parents)
        //        {
        //            writer.RowBold(index+1,true);
        //            printToExcel(writer, parent, ref index, "", "num_units", sim_order, channel_order);
        //        }
        //        finishSheet(writer, index, sim_order, channel_order);

        //        index = 5;
        //        writer.NewSheet("Unit Share");
        //        setupSheet(writer, "Unit Share", sim_order, channel_order, start, end);
        //        foreach(productTree parent in parents)
        //        {
        //            writer.RowBold(index+1,true);
        //            printToExcel(writer, parent, ref index, "", "unit_share", sim_order, channel_order);
        //        }
        //        finishSheet(writer, index, sim_order, channel_order);
        //    }
        //    catch
        //    {
        //        MessageBox.Show("There was an error while writing to the Excel file, please make sure you have the correct Excel libraries installed.","Excel Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //        writer.Kill();
        //        this.Cursor = Cursors.Arrow;
        //        return;
        //    }
			
        //    //writer.FillCell(1,1,"Hello World");
        //    try
        //    {
        //        writer.Quit();
        //    }
        //    catch(Exception e)
        //    {
        //        MessageBox.Show("There was an Error saving the Excel file.  The file may be read only or may be in use by another program.", "File In Use",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //        writer.Kill();
        //        this.Cursor = Cursors.Arrow;
        //        return;
        //    }
        //    this.Cursor = Cursors.Arrow;
        //}

        //private void setupSheet(ExcelInterface.ExcelWriter writer, string metric, 
        //    ArrayList sims, ArrayList channels, DateTime start, DateTime end)
        //{
        //    int num_sims = sims.Count;
        //    int num_channels = channels.Count;

        //    for(int i = 1; i <= 1+num_sims*num_channels; i++)
        //    {
        //        writer.ColumnCenterAlign(i);
        //    }

        //    //First Three Rows
        //    writer.MergeCells(1,1,1,num_sims*num_channels+1);
        //    writer.RowFontSize(1,10);
        //    writer.RowBold(1,true);
        //    writer.FillCell(1,1,"DECISION POWER EXCEL REPORT");

        //    writer.MergeCells(2,1,2,num_sims*num_channels+1);
        //    writer.RowFontSize(2,10);
        //    writer.RowBold(2,true);

        //    string modelData = "Model: " + this.db.Model.model_name;
        //    modelData += "  From Start Date: " + start.ToShortDateString();
        //    modelData += "  To End Date: " + end.ToShortDateString();

        //    writer.FillCell(2,1, modelData);

        //    writer.MergeCells(3,1,3,num_sims*num_channels+1);
        //    writer.RowFontSize(3,10);
        //    writer.RowBold(3,true);
        //    writer.FillCell(3,1,metric.ToUpper()+" BASIS");

        //    writer.RowBold(4,true);
        //    writer.RowFontSize(4,9);
        //    writer.MergeCells(4,2,4,1+num_sims);
        //    writer.FillCell(4,2,"TOTAL " + (num_channels-1).ToString() + " CHANNELS");
        //    for(int i = 1; i < num_channels; i++)
        //    {
        //        writer.MergeCells(4,2+num_sims*i,4,1+num_sims*(1+i));
        //        writer.FillCell(4,2+num_sims*i,(string)channels[i]);
        //    }

        //    writer.RowFontSize(5,7);
        //    writer.RowBackColor(5,37);
        //    writer.RowBold(5,true);
        //    writer.CellCenterAlign(5,1);
        //    writer.FillCell(5,1,"Category Items");
        //    for(int i = 0; i < num_channels; i++)
        //    {
        //        for(int ii = 0; ii < num_sims; ii++)
        //        {
        //            writer.FillCell(5,2+i*num_sims+ii,"  " + (string)sims[ii] + "  ");
        //        }
        //    }
        //}

        //private void finishSheet(ExcelInterface.ExcelWriter writer, int index, ArrayList sims, ArrayList channels)
        //{
        //    int num_sims = sims.Count;
        //    int num_channels = channels.Count;

        //    for(int i = 1; i <= 1+num_sims*num_channels; i++)
        //    {
        //        writer.ColumnAutofit(i);
        //    }
		
        //    for(int i = 6; i <= index; i++)
        //    {
        //        writer.RowFontSize(i,7);
        //        writer.RowHeight(i,11.25);
        //    }
        //}

        //private ArrayList printToExcel(ExcelInterface.ExcelWriter writer, productTree tree,ref int index, string seperator, string metric, ArrayList sim_order, ArrayList channel_order)
        //{
        //    int myIndex = ++index;
        //    writer.CellLeftAlign(myIndex,1);
        //    writer.FillCell(myIndex, 1, seperator + tree.Name);
        //    ArrayList compile = new ArrayList();
        //    if(tree.Rows.Count > 0)
        //    {
        //        compile.Add(compileRows(tree.Rows, metric, sim_order, channel_order));
        //    }
        //    foreach(productTree child in tree.Trees)
        //    {
        //        compile.Add(printToExcel(writer, child,ref index, seperator + "  ", metric, sim_order, channel_order));
        //    }
        //    ArrayList total = new ArrayList();
        //    total.AddRange((ArrayList)compile[0]);
        //    if(compile.Count > 1)
        //    {
        //        for(int i = 1; i < compile.Count; i++)
        //        {
        //            for(int ii = 0; ii < ((ArrayList)compile[i]).Count; ii++)
        //            {
        //                total[ii] = (double)total[ii] + (double)((ArrayList)compile[i])[ii];
        //            }
        //        }
        //    }
        //    int col = 2;
        //    foreach(object val in total)
        //    {
        //        writer.CellRightAlign(myIndex,col);
        //        writer.CellNumberFormat(myIndex,col,"0.00");
        //        writer.FillCell(myIndex, col, val.ToString());
        //        col++;
        //    }

        //    return total;
        //}

		private ArrayList compileRows(ArrayList rows, string metric, ArrayList sim_order, ArrayList channel_order)
		{
			System.Collections.Hashtable sim_table = new Hashtable();
			ArrayList output = new ArrayList();
			foreach(DataRow row in rows)
			{
				string lookup = (string)row["scenario"];
				if(!sim_table.Contains(lookup))
				{
					sim_table.Add(lookup,new System.Collections.Hashtable());
					((System.Collections.Hashtable)sim_table[lookup]).Add(row["channel_name"],row[metric]);
					//((System.Collections.Hashtable)sim_table[lookup]).Add("All",row[metric]);
				}
				else
				{
					//((System.Collections.Hashtable)sim_table[lookup])["All"] = ((double)((System.Collections.Hashtable)sim_table[lookup])["All"]) + ((double)row[metric]);
					((System.Collections.Hashtable)sim_table[lookup]).Add(row["channel_name"],row[metric]);
				}
			}

			foreach(string channel in channel_order)
			{
				foreach(string sim in sim_order)
				{
					System.Collections.Hashtable table = (System.Collections.Hashtable)sim_table[sim];
					double val = (double)table[channel];
					output.Add(val);
				}
			}

			return output;
		}

		private void build_product_list()
		{
			string query = "";
			ArrayList child_names = new ArrayList();
			string parent_name;
			string name;
			productTree.Reset();
			DataRow[] rows = db.Data.product.Select(query,"",DataViewRowState.CurrentRows);
			foreach(DataRow row in rows)
			{
				child_names.Clear();
				name = (string)row["product_name"];
				if(name == "All")
				{
					continue;
				}
				query = "parent_id = " + row["product_id"];
				DataRow[] child_ids = db.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
				foreach(DataRow child_id in child_ids)
				{
					query = "product_id = " + child_id["child_id"];
					DataRow[] child_name = db.Data.product.Select(query,"",DataViewRowState.CurrentRows);
					child_names.Add(child_name[0]["product_name"]);
				}
				query = "child_id = " + row["product_id"];
				DataRow[] parent_id = db.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
				if(parent_id.Length > 0)
				{
					query = "product_id = " + parent_id[0]["parent_id"];
					DataRow[] parent_name_row = db.Data.product.Select(query,"",DataViewRowState.CurrentRows);
					parent_name = (string)parent_name_row[0]["product_name"];
				}
				else
				{
					parent_name = null;
				}

				productTree.AddProduct(name, child_names, parent_name);
				
			}

			
		}

		private class productTree
		{
			private static System.Collections.Hashtable AllTrees = new Hashtable();
			private static System.Collections.Hashtable Parents = new Hashtable();
			private ArrayList myRows;
			private ArrayList myTrees;
			private string myName;

			private productTree(string product_name)
			{
				myName = product_name;
				myRows = new ArrayList();
				myTrees = new ArrayList();
			}

			public static void AddProduct(string product_name, ArrayList child_names, string parent_name)
			{	
				if(AllTrees.Contains(product_name))
				{
					return;
				}
				productTree tree = new productTree(product_name);
				if(child_names.Count > 0)
				{
					foreach(string child_name in child_names)
					{
						if(Parents.Contains(child_name))
						{
							tree.Trees.Add(Parents[child_name]);
							Parents.Remove(child_name);
						}
					}
				}
				if(parent_name != null)
				{
					if(AllTrees.Contains(parent_name))
					{
						((productTree)AllTrees[parent_name]).Trees.Add(tree);
					}
					else
					{
						Parents.Add(product_name, tree);
					}
				}
				else
				{
					Parents.Add(product_name, tree);
				}

				AllTrees.Add(product_name, tree);

			}

			public string Name
			{
				get
				{
					return myName;
				}
			}

			public ArrayList Trees
			{
				get
				{
					return myTrees;
				}
			}

			public ArrayList Rows
			{
				get
				{
					return myRows;
				}
			}

			public static void AddRow(string product_name, DataRow row)
			{
				if(AllTrees.Contains(product_name))
				{
					((productTree)AllTrees[product_name]).Rows.Add(row);
				}
			}

			public static ArrayList GetParents()
			{
				ArrayList parent_list = new ArrayList();
				parent_list.AddRange(Parents.Values);
				return parent_list;
			}

			public static void Reset()
			{
				AllTrees.Clear();
				Parents.Clear();
			}
		}

	}

	
}
