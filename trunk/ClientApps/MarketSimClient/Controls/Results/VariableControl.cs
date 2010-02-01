using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using MrktSimDb.Metrics;
using Common.Dialogs;

using MarketSimUtilities;
using Utilities.Graphing;

namespace Results
{
	/// <summary>
	/// Summary description for VariableControl.
	/// </summary>
	public class VariableControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button scenarioSummaryButton;
		private System.Windows.Forms.CheckedListBox YAxisBox;
		private Common.Utilities.ProductPicker variableProductPicker;
		private System.Windows.Forms.ComboBox XaxisBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button graphVariableButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox variableBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox scenarioBox;
		private System.Data.DataView scenarioView;
		private System.Data.DataView variableView;
		private System.Windows.Forms.CheckBox createScatter;
		private System.Windows.Forms.CheckBox oneChart;
		private System.Windows.Forms.CheckBox useXmetric;
        private ResultsForm owner;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public VariableControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			variableTable = new DataTable("VariableTable");

			variableTable.Columns.Add("token", typeof(string));
			DataColumn variableTableKeyCol = variableTable.Columns.Add("id", typeof(int));
			variableTable.Columns.Add("sim_id", typeof(int));
			variableTable.Columns.Add("Value", typeof(double));
			variableTable.Columns.Add("Min", typeof(double));
			variableTable.Columns.Add("Max", typeof(double));
			variableTable.PrimaryKey = new DataColumn[] {variableTableKeyCol};

			valueTable = new DataTable("Values");

			YAxisBox.DataSource = MetricMan.MetricValues;
			YAxisBox.DisplayMember = "Descr";

			XaxisBox.DataSource = MetricMan.MetricValues;
			XaxisBox.DisplayMember = "Descr";

			

            variableProductPicker.leafOnly = true;
			variableProductPicker.AllowAll = true;

            YAxisBox.Enabled = true;
            useXmetric.Enabled = true;
		}


        public void SetOwner( ResultsForm owner ) {
            this.owner = owner;
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
            this.scenarioSummaryButton = new System.Windows.Forms.Button();
            this.YAxisBox = new System.Windows.Forms.CheckedListBox();
            this.variableProductPicker = new Common.Utilities.ProductPicker();
            this.createScatter = new System.Windows.Forms.CheckBox();
            this.XaxisBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.graphVariableButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.variableBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.scenarioBox = new System.Windows.Forms.ComboBox();
            this.scenarioView = new System.Data.DataView();
            this.variableView = new System.Data.DataView();
            this.oneChart = new System.Windows.Forms.CheckBox();
            this.useXmetric = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.variableView)).BeginInit();
            this.SuspendLayout();
            // 
            // scenarioSummaryButton
            // 
            this.scenarioSummaryButton.Location = new System.Drawing.Point(344, 24);
            this.scenarioSummaryButton.Name = "scenarioSummaryButton";
            this.scenarioSummaryButton.Size = new System.Drawing.Size(104, 23);
            this.scenarioSummaryButton.TabIndex = 28;
            this.scenarioSummaryButton.Text = "Summary...";
            this.scenarioSummaryButton.Click += new System.EventHandler(this.scenarioSummaryButton_Click);
            // 
            // YAxisBox
            // 
            this.YAxisBox.Location = new System.Drawing.Point(8, 72);
            this.YAxisBox.Name = "YAxisBox";
            this.YAxisBox.Size = new System.Drawing.Size(240, 244);
            this.YAxisBox.TabIndex = 27;
            // 
            // variableProductPicker
            // 
            this.variableProductPicker.AllowAll = false;
            this.variableProductPicker.leafOnly = false;
            this.variableProductPicker.Location = new System.Drawing.Point(264, 80);
            this.variableProductPicker.Name = "variableProductPicker";
            this.variableProductPicker.ProductID = -1;
            this.variableProductPicker.Size = new System.Drawing.Size(232, 56);
            this.variableProductPicker.TabIndex = 26;
            this.variableProductPicker.TypeOnly = false;
            // 
            // createScatter
            // 
            this.createScatter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createScatter.Location = new System.Drawing.Point(320, 396);
            this.createScatter.Name = "createScatter";
            this.createScatter.Size = new System.Drawing.Size(144, 24);
            this.createScatter.TabIndex = 25;
            this.createScatter.Text = "Create Scatter Chart";
            // 
            // XaxisBox
            // 
            this.XaxisBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.XaxisBox.Enabled = false;
            this.XaxisBox.Location = new System.Drawing.Point(16, 428);
            this.XaxisBox.Name = "XaxisBox";
            this.XaxisBox.Size = new System.Drawing.Size(232, 21);
            this.XaxisBox.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "Select Data";
            // 
            // graphVariableButton
            // 
            this.graphVariableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.graphVariableButton.Location = new System.Drawing.Point(320, 428);
            this.graphVariableButton.Name = "graphVariableButton";
            this.graphVariableButton.Size = new System.Drawing.Size(88, 23);
            this.graphVariableButton.TabIndex = 21;
            this.graphVariableButton.Text = "Create Graph";
            this.graphVariableButton.Click += new System.EventHandler(this.graphButton_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(16, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Select Variable";
            // 
            // variableBox
            // 
            this.variableBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableBox.Location = new System.Drawing.Point(16, 372);
            this.variableBox.Name = "variableBox";
            this.variableBox.Size = new System.Drawing.Size(176, 21);
            this.variableBox.TabIndex = 19;
            this.variableBox.SelectedIndexChanged += new System.EventHandler(this.variableBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Select Scenario";
            // 
            // scenarioBox
            // 
            this.scenarioBox.Location = new System.Drawing.Point(8, 24);
            this.scenarioBox.Name = "scenarioBox";
            this.scenarioBox.Size = new System.Drawing.Size(248, 21);
            this.scenarioBox.TabIndex = 17;
            this.scenarioBox.SelectedIndexChanged += new System.EventHandler(this.scenarioBox_SelectedIndexChanged);
            // 
            // oneChart
            // 
            this.oneChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.oneChart.Location = new System.Drawing.Point(320, 372);
            this.oneChart.Name = "oneChart";
            this.oneChart.Size = new System.Drawing.Size(160, 24);
            this.oneChart.TabIndex = 30;
            this.oneChart.Text = "All products on one charts";
            // 
            // useXmetric
            // 
            this.useXmetric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.useXmetric.Location = new System.Drawing.Point(16, 404);
            this.useXmetric.Name = "useXmetric";
            this.useXmetric.Size = new System.Drawing.Size(136, 24);
            this.useXmetric.TabIndex = 31;
            this.useXmetric.Text = "Use Metric for X-Axis";
            this.useXmetric.CheckedChanged += new System.EventHandler(this.useXmetric_CheckedChanged);
            // 
            // VariableControl
            // 
            this.Controls.Add(this.useXmetric);
            this.Controls.Add(this.oneChart);
            this.Controls.Add(this.scenarioSummaryButton);
            this.Controls.Add(this.YAxisBox);
            this.Controls.Add(this.variableProductPicker);
            this.Controls.Add(this.createScatter);
            this.Controls.Add(this.XaxisBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.graphVariableButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.variableBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.scenarioBox);
            this.Name = "VariableControl";
            this.Size = new System.Drawing.Size(513, 468);
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.variableView)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region private fields
		private ResultsDb theDb;
		private DataTable variableTable = null;

		private DataTable valueTable;

		private string currentProductName = null;
		MrktSimDb.MrktSimDBSchema.scenario_variableRow currVariable = null;
		MrktSimDBSchema.simulationRow currScenario = null;


		#endregion

		#region public properties

        private MetricMan metricMan = null;

		
		
		public ResultsDb Db
		{
			set
			{
				theDb = value;

				scenarioView.Table = theDb.Data.simulation;
				scenarioView.RowFilter = "type <> 0";
				scenarioBox.DataSource = scenarioView;
				scenarioBox.DisplayMember = "name";

				variableView.Table = theDb.Data.scenario_variable;
				variableView.RowFilter = "id = -1";
				variableBox.DataSource = variableView;
				variableBox.DisplayMember = "token";

				// initializeVariableTable();

				initializeValueTable();

				this.variableProductPicker.Db = theDb;

				scenarioBox_SelectedIndexChanged(this, new System.EventArgs());
				variableBox_SelectedIndexChanged(this, new System.EventArgs());

                metricMan = new MetricMan( theDb );
			}
		}
		#endregion


		#region UI events
		
		private void scenarioBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{

            YAxisBox.Enabled = true;
            useXmetric.Enabled = true;
            XaxisBox.Enabled = true;
            graphVariableButton.Enabled = true;

			if (scenarioBox.SelectedItem == null)
			{
				currScenario = null;
				return;
			}

			currScenario = (MrktSimDb.MrktSimDBSchema.simulationRow) ((DataRowView) scenarioBox.SelectedItem).Row;

			if (currScenario.type == (byte) SimulationDb.SimulationType.Statistical)
			{
				variableBox.Enabled = false;
				graphVariableButton.Enabled = true;
				YAxisBox.Enabled = true;
				createScatter.Enabled = false;
				createScatter.Checked = true;
				XaxisBox.Enabled = true;

				useXmetric.Checked = true;
				useXmetric.Enabled = false;

				currVariable = null;
			}
			else
			{
				variableView.RowFilter = "sim_id = " + currScenario.id;
		

				if (variableView.Count == 0)
				{
					variableBox.Enabled = false;
                    currVariable = null;
					
				}
				else
				{
					variableBox.Enabled = true;
					graphVariableButton.Enabled = true;
					createScatter.Enabled = true;
					variableBox.SelectedIndex = 0;
				}

				if (variableBox.SelectedItem == null)
					return;

				currVariable = (MrktSimDb.MrktSimDBSchema.scenario_variableRow) ((DataRowView) variableBox.SelectedItem).Row;
			}
		}

		private void variableBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (variableBox.SelectedItem == null)
			{
				currVariable = null;
				return;
			}

			currVariable = (MrktSimDb.MrktSimDBSchema.scenario_variableRow) ((DataRowView) variableBox.SelectedItem).Row;
		}


		private void useXmetric_CheckedChanged(object sender, System.EventArgs e)
		{
				XaxisBox.Enabled = useXmetric.Checked;
		}

		private void graphButton_Click(object sender, System.EventArgs e)
		{
			// create plot(s)
			if (currScenario == null)
				return;

			// we can create a scatter plot without a variable
			// double check logic
			if(currVariable == null && !useXmetric.Checked)
				return;

            //if (currVariable == null && !createScatter.Checked)
            //    return;

            MultiGrapher mgraph = new MultiGrapher( owner.GetCurrentResultsSettings() );
            mgraph.NewNamedSettingsAdded += new MultiGrapher.HandleUpdatedNamedSettings( owner.mgraph_NewNamedSettingsAdded );
            mgraph.NamedSettingsSaved += new MultiGrapher.HandleUpdatedNamedSettings( owner.mgraph_NamedSettingsSaved );

			mgraph.Text = currScenario.name;
			mgraph.TimeSeries = false;

			mgraph.SuspendLayout();

			Value xValue = null;

			currentProductName = null;

            // check if we need only one chart
            bool oneChartForAll = true;
            foreach( Value yValue in YAxisBox.CheckedItems ) {
                if( !yValue.ConstantOverProducts ) {
                    oneChartForAll = false;
                    break;
                }
            }

			if (createScatter.Checked)
				mgraph.ScatterPlot = true;

			if (useXmetric.Checked)
			{
				xValue = (Value) XaxisBox.SelectedItem;
				
				xValue.Product = this.variableProductPicker.ProductID;
			}

            if( oneChartForAll ) {
                Plot plot = mgraph.NewPlot();

                plot.AutoScaleAxis();

                ResultsForm.InitializeColor();

                if( xValue != null )
                    plot.XAxis = xValue.Descr;
                else
                    plot.XAxis = currVariable.token;

                currentProductName = "All";

                createPlot( plot, Database.AllID, xValue );
            }

			else if (this.variableProductPicker.ProductID == ModelDb.AllID)
			{
                string query = "brand_id = 1 OR product_id = " + Database.AllID;

				if (oneChart.Checked)
				{
					Plot plot = mgraph.NewPlot();
                    plot.AutoScaleAxis();

					ResultsForm.InitializeColor();

					if (xValue != null)
						plot.XAxis = xValue.Descr;
					else
						plot.XAxis = currVariable.token;

                    DataRow[] rows = theDb.Data.product.Select(query, "product_name", DataViewRowState.CurrentRows);
					foreach(DataRow row in rows)
					{
						int product_id = (int) row["product_id"];
							
						MrktSimDBSchema.productRow product = theDb.Data.product.FindByproduct_id(product_id);

						currentProductName = product.product_name;

						createPlot(plot, product_id, xValue);
					}
				}
				else
				{


                    DataRow[] rows = theDb.Data.product.Select(query, "product_name", DataViewRowState.CurrentRows);
					foreach(DataRow row in rows)
					{
						int product_id = (int) row["product_id"];

						Plot plot = mgraph.NewPlot();
                        plot.AutoScaleAxis();

						ResultsForm.InitializeColor();

						if (xValue != null)
							plot.XAxis = xValue.Descr;
						else
							plot.XAxis = currVariable.token;

						MrktSimDBSchema.productRow product = theDb.Data.product.FindByproduct_id(product_id);
						
						plot.Text = product.product_name;
						plot.Title = plot.Text;

						createPlot(plot, product_id, xValue);
					}
				}
			}
			else
			{
				ResultsForm.InitializeColor();

				Plot plot = mgraph.NewPlot();

				if (xValue != null)
					plot.XAxis = xValue.Descr;
				else
					plot.XAxis = currVariable.token;

				MrktSimDBSchema.productRow product =  theDb.Data.product.FindByproduct_id(variableProductPicker.ProductID);
						
				plot.Text = product.product_name;
				plot.Title = plot.Text;

				createPlot(plot, this.variableProductPicker.ProductID, xValue);
			}
			
			mgraph.PlotsChanged();
			mgraph.ResumeLayout(false);
			// mgraph.Initialize();
			mgraph.Show();
		}


		private void scenarioSummaryButton_Click(object sender, System.EventArgs e)
		{
			if (scenarioBox.SelectedItem == null)
				return;

			GridView dlg = new GridView();

			dlg.Text = currScenario.name;

			// create table for dlg
			DataTable varSumTable = new DataTable("VariableSummary");

			varSumTable.Columns.Add("sim", typeof(string));
			varSumTable.Columns.Add("product", typeof(string));

			if (currScenario.type == (byte) SimulationDb.SimulationType.Serial)
			{
				varSumTable.Columns.Add("parameter", typeof(string));
			}

			// add a column for each variable in the current scenario
			foreach (MrktSimDBSchema.scenario_variableRow variable in currScenario.Getscenario_variableRows())
			{
				varSumTable.Columns.Add(variable.token, typeof(double));
			}

			// for each metric we also add a column
			foreach (Value val in YAxisBox.CheckedItems)
			{
				varSumTable.Columns.Add(val.Descr, typeof(double));
			}

			// now for each run and product we create a row
			foreach (MrktSimDBSchema.sim_queueRow sim in currScenario.Getsim_queueRows())
			{
				string parmName = null;
				
				if (currScenario.type == (byte) SimulationDb.SimulationType.Serial)
				{
					MrktSimDBSchema.model_parameterRow parm = theDb.Data.model_parameter.FindByid(sim.param_id);

					if (parm != null )
					{
						parmName = parm.name;
					}
					else
					{
						parmName = "NA";
					}
				}

				foreach(MrktSimDBSchema.productRow product in theDb.Data.product.Select("brand_id = 1", "product_name", DataViewRowState.CurrentRows))
				{
					DataRow simRow = varSumTable.NewRow();

					simRow["sim"] = sim.name;
					simRow["product"] = product.product_name;

					if (parmName != null)
					{
						simRow["parameter"]  = parmName;
					}

					string simQuery = "run_id = " + sim.run_id;

					// assign variable values
					DataRow[] values =  theDb.Data.sim_variable_value.Select(simQuery, "", DataViewRowState.CurrentRows);

					foreach (MrktSimDBSchema.sim_variable_valueRow simVal in values)
					{
						// need to find name of this value
						MrktSimDBSchema.scenario_variableRow variable = theDb.Data.scenario_variable.FindByid(simVal.var_id);
					
						simRow[variable.token] = simVal.val;
					}

				

					// assign metric values
					foreach (Value yValue in YAxisBox.CheckedItems)
					{
						yValue.Run = sim.run_id;
						yValue.Product = product.product_id;
						simRow[yValue.Descr] = metricMan.Evaluate(yValue);
					}

					varSumTable.Rows.Add(simRow);
				}
			}

			dlg.Table = varSumTable;

			dlg.Show();
		}

		#endregion

		#region old and in the way
//		private void graphVariableButton_Click(object sender, System.EventArgs e)
//		{
//			// create plot(s)
//			if (variableBox.SelectedItem == null)
//				return;
//
//			MrktSimDBSchema.scenario_variableRow currVariable = (MrktSimDb.MrktSimDBSchema.scenario_variableRow) ((DataRowView) variableBox.SelectedItem).Row;
//
//			Grapher graph = new Grapher();
//
//			MrktSimDb.MrktSimDBSchema.scenarioRow scenario = (MrktSimDb.MrktSimDBSchema.scenarioRow) ((DataRowView) scenarioBox.SelectedItem).Row;
//
//			graph.Title = scenario.name;
//
//			graph.YAxis = YAxisBox.SelectedItem.ToString();
//
//			if (createScatter.Checked)
//			{
//				graph.XAxis = XaxisBox.SelectedItem.ToString();
//			}
//			else
//			{
//				graph.XAxis =  currVariable.token;
//			}
//
//			graph.Text = theDb.Model.model_name + " - " + scenario.name + " - " +  YAxisBox.SelectedItem.ToString();
//
//			plotAllCurves(graph);
//
//			graph.DataChanged();
//
//			graph.Show();
//		}

		#endregion

		#region plot code

		private void createPlot(Plot plot, int product_id, Value xValue)
		{
			
			if (xValue != null)
				xValue.Product = product_id;

			foreach (Value yValue in YAxisBox.CheckedItems)
			{
				
				yValue.Product = product_id;

				plot.YAxis = yValue.Descr;

				plotAllCurves(plot, yValue, xValue);
			}
		}

		private void plotAllCurves(Plot plot, Value yValue, Value xValue)
		{

			if (plot.ScatterPlot)
			{
				scatterPlot(plot, yValue, xValue);
				ResultsForm.NextColor();
			}
			else if (currScenario.type == (byte) SimulationDb.SimulationType.Serial)
			{
				setVariableTableValuesAtMin();

				bool done = false;

				while(!done)
				{
					foreach(MrktSimDb.MrktSimDBSchema.scenario_parameterRow param in currScenario.Getscenario_parameterRows())
					{
						plotCurve(plot, param, yValue, xValue);
						ResultsForm.NextColor();
					}
					incVariableTableValues(out done);
					
				}
			}
			else if (xValue != null)
			{
                plotCurveAgainstMetric(plot, yValue, xValue);

			}
            else
            {

                setVariableTableValuesAtMin();

                bool done = false;

                while (!done)
                {
                    plotCurve(plot, null, yValue, xValue);
                    incVariableTableValues(out done);
                    ResultsForm.NextColor();
                }
            }

		}

		private void scatterPlot(Plot plot, Value yValue, Value xValue)
		{
			string currentVariableID = null;

			if (currVariable != null)
				currentVariableID = "var" + currVariable.id.ToString();

			string valueQuery = "sim_id = " + currScenario.id;

			// now we can query the valueTable
			DataRow[] values = valueTable.Select(valueQuery);

			DataCurve crv = new DataCurve(values.Length);
			
			foreach (DataRow valRow in values)
			{
				double val;

				yValue.Run = (int) valRow["run_id"];

				if (xValue != null)
				{
					xValue.Run = (int) valRow["run_id"];
					val = metricMan.Evaluate(xValue);
				}
				else
				{
					val = (double) valRow[currentVariableID];
				}

				
				// add to curve
				 crv.Add(val, metricMan.Evaluate(yValue));
			}

			crv.Label = currentProductName;

			crv.Label += "-" + yValue.Descr;

			crv.Color = ResultsForm.CurrentColor;

			plot.Data = crv;

			plot.DataChanged();
		}

        private void plotCurveAgainstMetric(Plot plot, Value yValue, Value xValue)
        {

            DataCurve crv = new DataCurve(currScenario.Getsim_queueRows().Length);

            // run thru all runs
            foreach (MrktSimDBSchema.sim_queueRow run in currScenario.Getsim_queueRows())
            {
                xValue.Run = run.run_id;
                yValue.Run = run.run_id;

                  // add to curve
                crv.Add( metricMan.Evaluate(xValue), metricMan.Evaluate(yValue));
            }

            crv.Label = currentProductName;

            crv.Label += "-" + yValue.ToString();

            // crv.Label += varvals;
            crv.Color = ResultsForm.CurrentColor;

            plot.Data = crv;

            plot.DataChanged();
        }

		private void plotCurve(Plot plot, MrktSimDb.MrktSimDBSchema.scenario_parameterRow param, Value yValue, Value xValue)
		{
			const double eps = 0.0000001;

			// snap values
			string varvals = snapVariableTableValues();

			string currentVariableID = "var" + currVariable.id.ToString();
			string valueQuery = "sim_id = " + currScenario.id;

			if (param != null)
			{
				valueQuery += " AND param_id = " + param.param_id;
			}

			string valueSort = currentVariableID;

			// now we loop through variable constructing

			string variableQuery = "sim_id = " + currVariable.sim_id;
			variableQuery += " AND id <> " + currVariable.id;

			DataRow[] variables = variableTable.Select(variableQuery);

			foreach (DataRow variable in variables)
			{
				double min = (double) variable["Value"];

				min -= eps;
				double max = min + 2*eps;

				valueQuery += " AND " + "var" + variable["id"] + " <= " + max.ToString();
				valueQuery += " AND " + "var" + variable["id"] + " >= " + min.ToString();
			}

			// now we can query the valueTable
			DataRow[] values = valueTable.Select(valueQuery, valueSort);

			DataCurve crv = new DataCurve(values.Length);

			foreach (DataRow valRow in values)
			{
				// should be sorted and unique
				double val = 0.0;
					
				if (valRow[currentVariableID] != null)
					val = (double) valRow[currentVariableID];
				else
					break;

				
				yValue.Run = (int) valRow["run_id"];

				if (xValue != null)
				{
					xValue.Run = (int) valRow["run_id"];
					val = metricMan.Evaluate(xValue);
				}

				// add to curve
				crv.Add(val, metricMan.Evaluate(yValue));
			}

			crv.Label = currentProductName;

			if (param != null)
			{
				crv.Label += theDb.Data.model_parameter.FindByid(param.param_id).name;
			}

			crv.Label += "-" + yValue.ToString();

			crv.Label += varvals;
			crv.Color = ResultsForm.CurrentColor;

			plot.Data = crv;

			plot.DataChanged();
		}
		
		private void setVariableTableValuesAtMin()
		{
			// Need to select for each variable value
			if (variableBox.SelectedItem == null)
				return;

			MrktSimDBSchema.scenario_variableRow currVariable = (MrktSimDb.MrktSimDBSchema.scenario_variableRow) ((DataRowView) variableBox.SelectedItem).Row;

			// look for other variables for current scenario
			string query = "sim_id = " + currVariable.sim_id;
			query += " AND id <> " + currVariable.id;

			DataRow[] variables = variableTable.Select(query);

			foreach (DataRow variable in variables)
			{
				// need to find allowable values for this variable
				string simQuery = "var_id = " + variable["id"];
				string sort = "val";

				double currentVal = (double) variable["Value"];

				DataRow[] values =  theDb.Data.sim_variable_value.Select(simQuery, sort);

				if (values.Length == 0)
				{
					// this variable had no values for this scenario?
					// lets abort
					continue;
				}
				
				variable["Value"] = values[0]["val"];				
			}
		}

		
		// otherwise will increment the first value NOT at max
		// signal stop if ALL variables are at max
		private void incVariableTableValues(out bool stop)
		{
			stop = true;

			// Need to select for each variable value
			if (currVariable == null)
				return;

			// look for other variables for current scenario
			string query = "sim_id = " + currVariable.sim_id;
			query += " AND id <> " + currVariable.id;

			DataRow[] variables = variableTable.Select(query);

			foreach (DataRow variable in variables)
			{
				// need to find allowable values for this variable
				string simQuery = "var_id = " + variable["id"];
				string sort = "val";

				double currentVal = (double) variable["Value"];

				DataRow[] values =  theDb.Data.sim_variable_value.Select(simQuery, sort);

				if (values.Length == 0)
				{
					// this variable had no values for this scenario?
					// lets abort
					continue;
				}

				if (currentVal < (double) values[values.Length - 1]["val"])
				{
					// this value is less then the maximum
					// we will increment it
					// and stop looping
					stop = false;

					int index = 0;
					while(index < values.Length && ((double) values[index]["val"]) <= currentVal)
						index++;

					variable["Value"] = values[index]["val"];

					break;
				}
	
				// if get here then we need to set the variable to min
				// if we get here for ALL variables then done will be true
				variable["Value"] = values[0]["val"];
			}
		}

		// snap values (not current variable though)
		private string snapVariableTableValues()
		{
			string variableValues = null;

			// Need to select for each variable value
			if (currVariable == null)
				return variableValues;

			// look for other variables for current scenario
			string query = "sim_id = " + currVariable.sim_id;
			query += " AND id <> " + currVariable.id;

			DataRow[] variables = variableTable.Select(query);

			foreach (DataRow variable in variables)
			{
				// need to find allowable values for this variable
				string simQuery = "var_id = " + variable["id"];
				string sort = "val";

				double currentVal = (double) variable["Value"];

				DataRow[] values =  theDb.Data.sim_variable_value.Select(simQuery, sort);

				if (values.Length == 0)
				{
					// this variable had no values for this scenario?
					// lets abort
					continue;
				}

				int index = 0;
				while(index < values.Length && ((double) values[index]["val"]) <= currentVal)
					index++;

				// snap value
				if (index == 0)
					variable["Value"] = values[0]["val"];
				else
					variable["Value"] = values[index -1]["val"];

				// get name of variable
				variableValues = ", " + variable["token"].ToString() + " = " + variable["Value"].ToString();
			}

			return variableValues;
		}


		private void initializeVariableTable()
		{
			foreach(MrktSimDb.MrktSimDBSchema.scenario_variableRow variable in theDb.Data.scenario_variable.Rows)
			{
				DataRow variableRow = variableTable.NewRow();

				variableRow["token"] = variable.token;
				variableRow["id"] = variable.id;
				variableRow["sim_id"] = variable.sim_id;
				variableRow["Value"] = variable.min;
				variableRow["Min"] = variable.min;
				variableRow["Max"] = variable.max;

				variableTable.Rows.Add(variableRow);
			}

			variableTable.AcceptChanges();
		}

		private void initializeValueTable()
		{
			valueTable.Columns.Clear();

			valueTable.Columns.Add("sim_id", typeof(int));
			valueTable.Columns.Add("run_id", typeof(int));
			valueTable.Columns.Add("name", typeof(string));
			valueTable.Columns.Add("param_id", typeof(int));

			// add a column for each variable
			foreach (MrktSimDBSchema.scenario_variableRow variable in theDb.Data.scenario_variable.Rows)
			{
				valueTable.Columns.Add("var" + variable.id.ToString(), typeof(double));
			}

			// now for each run we create a row
			foreach (MrktSimDBSchema.sim_queueRow sim in theDb.Data.sim_queue.Rows)
			{
				DataRow simRow = valueTable.NewRow();

				simRow["sim_id"] = sim.sim_id;
				simRow["run_id"] = sim.run_id;
				simRow["name"] = sim.name;

				simRow["param_id"] = sim.param_id;

				string simQuery = "run_id = " + sim.run_id;
				DataRow[] values =  theDb.Data.sim_variable_value.Select(simQuery, "", DataViewRowState.CurrentRows);

				foreach (MrktSimDBSchema.sim_variable_valueRow simVal in values)
				{
					string id = "var" + simVal.var_id.ToString();
					simRow[id] = simVal.val;
				}

                if (values.Length > 0)
                {
                    valueTable.Rows.Add(simRow);
                }
			}
		}
		
		#endregion

	
	}
}
