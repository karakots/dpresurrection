using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MrktSimDb;
using MrktSimDb.Metrics;
using Common.Utilities;
using ModelView;
using Common.Dialogs;
using EquationParser;

using MarketSimUtilities;

namespace  ModelView.Dialogs
{
	/// <summary>
	/// Summary description for CreateScenario.
	/// </summary>
	public class CreateScenario : System.Windows.Forms.Form
	{
		private static System.Random rand = new Random(123456);

		public void SaveModel()
		{
			if ( currentSimulation.name != nameBox.Text )
			{
				string filter = "id <> " + currentSimulation.id;
				currentSimulation.name = ModelDb.CreateUniqueName(theDb.Data.simulation, "name", nameBox.Text, filter);
			}

			// update the rest of the data
			currentSimulation.start_date = iStartEndDate.Start;
			currentSimulation.end_date = iStartEndDate.End;
            currentSimulation.scale_factor = (double)scalingNumericUpDown.Value;
			currentSimulation.metric_start_date = currentSimulation.start_date;
			currentSimulation.metric_end_date = currentSimulation.end_date;
			switch(access_timeCombo.SelectedIndex)
			{
				case 0:
					currentSimulation.access_time = 1;
					break;
				case 1:
					currentSimulation.access_time = 7;
					break;
				case 2:
					currentSimulation.access_time = 30;
					break;
				default:
					currentSimulation.access_time = 1;
					break;
			}

			currentSimulation.access_time *= (int) numSpans.Value;

			if( deleteResults.Checked )
			{
				currentSimulation.delete_std_results = true;
			}
			else
			{
				currentSimulation.delete_std_results = false;
			}
			currentSimulation.descr = descrBox.Text;


//			// save data
//			scenarioMarketPlanGrid.Suspend = true;
//			scenarioFactorsGrid.Suspend = true;

            theDb.Update();

			// theDb.CloseScenario(currentSimulation.id);

			if (ScenarioDbChanged != null)
				ScenarioDbChanged();
		}

		public delegate void ScenarioUpdated();

		public event ScenarioUpdated ScenarioDbChanged;

		SimulationDb theDb;
		DataTable	availablePlans;
		DataTable	availableFactors;

		DataTable	selectedPlans;
		DataTable	selectedFactors;

		DataTable	availableParms;

		public SimulationDb Db
		{
			set
			{
				theDb = value;

                //iStartEndDate.Db = theDb;


                //if (theDb..task_based)
                //    factorType = ModelDb.PlanType.TaskEvent;
                //else
                //    factorType = ModelDb.PlanType.ProdEvent;

				if (currentSimulation != null)
				{
					initializeTables();
				}

				scenarioParmGrid.Table = theDb.Data.scenario_parameter;

				scenarioParmGrid.DescriptionWindow = false;
				scenarioParmGrid.AllowDelete = false;
				

				if (currentSimulation != null)
				{
					scenarioParmGrid.RowFilter = "sim_id = " + currentSimulation.id;
				}

				variableGrid.Table = theDb.Data.scenario_variable;
				if (currentSimulation != null)
				{
					variableGrid.RowFilter = "id = " + currentSimulation.id;
				}

				variableGrid.DescriptionWindow = false;

				seedGrid1.Table = theDb.Data.scenario_simseed;
				
				if (currentSimulation != null)
				{
					seedGrid1.RowFilter = "id = " + currentSimulation.id;
				}

				seedGrid1.DescriptionWindow = false;

                //if (theDb.ReadOnly && !theDb.Queued)
                //{
                //    acceptButton.Enabled = false;
                //}


				this.scenarioTypeView.Table = SimulationDb.simulation_type;

				if (MrktSimControl.MrktSimMessage("devl_version") != "true" && currentSimulation != null)
				{
					scenarioTypeView.RowFilter = "id <> 96 "; //optimization
					scenarioTypeView.RowFilter += " AND id <> 100 "; // calibration

					if (Database.Nimo)
					{
						scenarioTypeView.RowFilter += " AND id <> 150 "; // check pointing
					}
				}

				scenarioTypeBox.DisplayMember = "type";
				scenarioTypeBox.ValueMember = "id";

				if (currentSimulation != null)
				{
					scenarioTypeBox.SelectedValue = currentSimulation.type;

					initializeMetricSelectBox();
				}

				

				createTableStyle();

				// check for exisiting simulations
				if (currentSimulation != null && currentSimulation.sim_num >= 0)
				{
					disableSimDependentControls();
				}

			}
		}

		private MrktSimDBSchema.simulationRow currentSimulation = null;

        public MrktSimDBSchema.simulationRow CurrentSimulation
		{
			set
			{
				currentSimulation = value;
				this.Text = "Editing " + currentSimulation.name;

				iStartEndDate.Start = currentSimulation.start_date;
				iStartEndDate.End = currentSimulation.end_date;
                scalingNumericUpDown.Value = (int)Math.Round(currentSimulation.scale_factor);

				if (currentSimulation.access_time % 7 == 0)
				{
					access_timeCombo.SelectedIndex = 1;
					this.numSpans.Value = (int) currentSimulation.access_time / 7;
				}
				else if (currentSimulation.access_time % 30 == 0)
				{
					access_timeCombo.SelectedIndex = 2;
					this.numSpans.Value = (int) currentSimulation.access_time / 30;
				}
				else
				{
					access_timeCombo.SelectedIndex = 0;
					this.numSpans.Value = (int) currentSimulation.access_time;
				}

				if( currentSimulation.delete_std_results )
				{
					deleteResults.Checked = true;
				}
				else
				{
					deleteResults.Checked = false;
				}

				nameBox.Text = currentSimulation.name;
				descrBox.Text = currentSimulation.descr;

				if (theDb != null)
				{

					initializeTables();

					scenarioParmGrid.RowFilter = "sim_id = " + currentSimulation.id;
                    variableGrid.RowFilter = "sim_id = " + currentSimulation.id;
                    seedGrid1.RowFilter = "sim_id = " + currentSimulation.id;

					if (currentSimulation.type == 96 ||
						currentSimulation.type == 100)
					{
						this.scenarioTypeView.RowFilter = null;

						if (Database.Nimo)
						{
							scenarioTypeView.RowFilter += "scenario_type_id <> 150 "; // check pointing
						}
					}
					else if (MrktSimControl.MrktSimMessage("devl_version") != "true")
					{
						scenarioTypeView.RowFilter = "id <> 96 "; //optimization
						scenarioTypeView.RowFilter += " AND id <> 100 "; // calibration

						if (Database.Nimo)
						{
							scenarioTypeView.RowFilter += " AND id <> 150 "; // check pointing
						}
					}
					else
					{
						if (Database.Nimo)
						{
							scenarioTypeView.RowFilter = "id <> 150 "; // check pointing
						}
					}

					scenarioTypeBox.SelectedValue = currentSimulation.type;
					setUpScenarioPage();

					createTableStyle();

					initializeMetricSelectBox();
				}

				if (currentSimulation.sim_num >= 0)
				{
					disableSimDependentControls();
				}
			}

			get
			{
				return currentSimulation;
			}
		}

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox descrBox;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage homePage;
        private System.Windows.Forms.Panel panel1;
		private MrktSimGrid modelParmGrid;
		private MrktSimGrid scenarioParmGrid;
		private System.Windows.Forms.Button addparmButton;
        private System.Windows.Forms.Button removeParmButton;
		private Common.Utilities.StartEndDate iStartEndDate;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.TabPage parameterPage;
		private System.Windows.Forms.TabPage variablePage;
		private MrktSimGrid variableGrid;
		private System.Windows.Forms.TextBox tokenBox;
		private System.Windows.Forms.Label tokenLabel;
		private System.Windows.Forms.Button createvariableButton;
		private System.Windows.Forms.Button resetParamValuesButton;
		private System.Windows.Forms.Panel variablePanel;
		private System.Windows.Forms.ComboBox scenarioTypeBox;
		private System.Windows.Forms.Label label2;
		private MrktSimGrid seedGrid1;
		private System.Windows.Forms.Button addSeedButton;
		private System.Data.DataView scenarioTypeView;
		private System.Windows.Forms.TextBox scenarioTypeDesc;
		private System.Windows.Forms.GroupBox scenarioGroupDefine;
		private System.Windows.Forms.GroupBox scenarioGroupStatistics;
		private System.Windows.Forms.GroupBox scenarioGroupSelectMetrics;
		private System.Windows.Forms.CheckedListBox metricBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label numSimsLabel;
		private System.Windows.Forms.NumericUpDown initialSeed;
		private System.Windows.Forms.Label seedLabel;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.CheckBox deleteResults;
		private System.Windows.Forms.Button checkAllBtn;
		private System.Windows.Forms.Button scenarioOptButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox access_timeCombo;
		private System.Windows.Forms.NumericUpDown numSpans;
		private System.Windows.Forms.Label label6;
        private Label label7;
        private NumericUpDown scalingNumericUpDown;
        private Label label8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateScenario()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			access_timeCombo.SelectedIndex = 0;
			
			this.Text = "Create New Scenario";

			nameBox.Text = "";
			descrBox.Text = "";

			// initialize available tables
			availablePlans = new DataTable();
			availablePlans.TableName = "AvailablePlans";
			availablePlans.Columns.Add("product_name", typeof(string));
			availablePlans.Columns.Add("name", typeof(string));
			availablePlans.Columns.Add("id", typeof(int));
			//availablePlans.Columns.Add("product_id", typeof(int));
			
			availableFactors = new DataTable();
			availableFactors.TableName = "AvailableFactors";
			availableFactors.Columns.Add("product_name", typeof(string));
			availableFactors.Columns.Add("name", typeof(string));
			availableFactors.Columns.Add("id", typeof(int));
			
			// initialize selected tables
			selectedPlans = new DataTable();
			selectedPlans.TableName = "SelectedPlans";
			selectedPlans.Columns.Add("product_name", typeof(string));
			selectedPlans.Columns.Add("name", typeof(string));
			selectedPlans.Columns.Add("id", typeof(int));
			//selectedPlans.Columns.Add("product_id", typeof(int));
			
			selectedFactors = new DataTable();
			selectedFactors.TableName = "SelectedFactors";
			selectedFactors.Columns.Add("product_name", typeof(string));
			selectedFactors.Columns.Add("name", typeof(string));
			selectedFactors.Columns.Add("id", typeof(int));

			availableParms = new DataTable();
			availableParms.TableName = "AvailableParms";
			availableParms.Columns.Add("name", typeof(string));
			availableParms.Columns.Add("type", typeof(string));
			availableParms.Columns.Add("value", typeof(string));
			availableParms.Columns.Add("id", typeof(int));

			
			// initialize grids
			modelParmGrid.Table = availableParms;
			modelParmGrid.DescriptionWindow = false;
			modelParmGrid.AllowDelete = false;

			this.tabControl.SelectedIndex = 0;

			foreach(Metric metric in MetricMan.SimSummaryMetrics)
				metricBox.Items.Add(metric);

			foreach(Metric metric in MetricMan.CalibrationMetrics)
				metricBox.Items.Add(metric);

			setUpScenarioPage();
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homePage = new System.Windows.Forms.TabPage();
            this.scenarioGroupSelectMetrics = new System.Windows.Forms.GroupBox();
            this.metricBox = new System.Windows.Forms.CheckedListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkAllBtn = new System.Windows.Forms.Button();
            this.deleteResults = new System.Windows.Forms.CheckBox();
            this.scenarioGroupStatistics = new System.Windows.Forms.GroupBox();
            this.seedGrid1 = new MarketSimUtilities.MrktSimGrid();
            this.addSeedButton = new System.Windows.Forms.Button();
            this.scenarioGroupDefine = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.scalingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numSpans = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.access_timeCombo = new System.Windows.Forms.ComboBox();
            this.scenarioOptButton = new System.Windows.Forms.Button();
            this.seedLabel = new System.Windows.Forms.Label();
            this.initialSeed = new System.Windows.Forms.NumericUpDown();
            this.numSimsLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.iStartEndDate = new Common.Utilities.StartEndDate();
            this.label2 = new System.Windows.Forms.Label();
            this.scenarioTypeBox = new System.Windows.Forms.ComboBox();
            this.scenarioTypeView = new System.Data.DataView();
            this.scenarioTypeDesc = new System.Windows.Forms.TextBox();
            this.parameterPage = new System.Windows.Forms.TabPage();
            this.scenarioParmGrid = new MarketSimUtilities.MrktSimGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.resetParamValuesButton = new System.Windows.Forms.Button();
            this.addparmButton = new System.Windows.Forms.Button();
            this.removeParmButton = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.modelParmGrid = new MarketSimUtilities.MrktSimGrid();
            this.variablePage = new System.Windows.Forms.TabPage();
            this.variableGrid = new MarketSimUtilities.MrktSimGrid();
            this.variablePanel = new System.Windows.Forms.Panel();
            this.tokenLabel = new System.Windows.Forms.Label();
            this.tokenBox = new System.Windows.Forms.TextBox();
            this.createvariableButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.homePage.SuspendLayout();
            this.scenarioGroupSelectMetrics.SuspendLayout();
            this.panel5.SuspendLayout();
            this.scenarioGroupStatistics.SuspendLayout();
            this.scenarioGroupDefine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scalingNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioTypeView)).BeginInit();
            this.parameterPage.SuspendLayout();
            this.panel4.SuspendLayout();
            this.variablePage.SuspendLayout();
            this.variablePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(638, 12);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 42;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(518, 12);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 41;
            this.acceptButton.Text = "Accept";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 40;
            this.label3.Text = "Description";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 39;
            this.label1.Text = "Name";
            // 
            // descrBox
            // 
            this.descrBox.Location = new System.Drawing.Point(80, 56);
            this.descrBox.Multiline = true;
            this.descrBox.Name = "descrBox";
            this.descrBox.Size = new System.Drawing.Size(248, 40);
            this.descrBox.TabIndex = 38;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(80, 24);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(248, 20);
            this.nameBox.TabIndex = 36;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.homePage);
            this.tabControl.Controls.Add(this.parameterPage);
            this.tabControl.Controls.Add(this.variablePage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(728, 408);
            this.tabControl.TabIndex = 45;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // homePage
            // 
            this.homePage.Controls.Add(this.scenarioGroupSelectMetrics);
            this.homePage.Controls.Add(this.scenarioGroupStatistics);
            this.homePage.Controls.Add(this.scenarioGroupDefine);
            this.homePage.Location = new System.Drawing.Point(4, 22);
            this.homePage.Name = "homePage";
            this.homePage.Size = new System.Drawing.Size(720, 382);
            this.homePage.TabIndex = 0;
            this.homePage.Text = "Scenario";
            this.homePage.UseVisualStyleBackColor = true;
            // 
            // scenarioGroupSelectMetrics
            // 
            this.scenarioGroupSelectMetrics.Controls.Add(this.metricBox);
            this.scenarioGroupSelectMetrics.Controls.Add(this.panel5);
            this.scenarioGroupSelectMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioGroupSelectMetrics.Location = new System.Drawing.Point(464, 0);
            this.scenarioGroupSelectMetrics.Name = "scenarioGroupSelectMetrics";
            this.scenarioGroupSelectMetrics.Size = new System.Drawing.Size(256, 382);
            this.scenarioGroupSelectMetrics.TabIndex = 51;
            this.scenarioGroupSelectMetrics.TabStop = false;
            this.scenarioGroupSelectMetrics.Text = "Select Metrics";
            // 
            // metricBox
            // 
            this.metricBox.CheckOnClick = true;
            this.metricBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.metricBox.Location = new System.Drawing.Point(3, 56);
            this.metricBox.Name = "metricBox";
            this.metricBox.Size = new System.Drawing.Size(250, 319);
            this.metricBox.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkAllBtn);
            this.panel5.Controls.Add(this.deleteResults);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(250, 40);
            this.panel5.TabIndex = 1;
            // 
            // checkAllBtn
            // 
            this.checkAllBtn.Location = new System.Drawing.Point(8, 8);
            this.checkAllBtn.Name = "checkAllBtn";
            this.checkAllBtn.Size = new System.Drawing.Size(88, 24);
            this.checkAllBtn.TabIndex = 44;
            this.checkAllBtn.Text = "Check All";
            this.checkAllBtn.Click += new System.EventHandler(this.checkAllBtn_Click);
            // 
            // deleteResults
            // 
            this.deleteResults.Location = new System.Drawing.Point(112, 8);
            this.deleteResults.Name = "deleteResults";
            this.deleteResults.Size = new System.Drawing.Size(128, 16);
            this.deleteResults.TabIndex = 43;
            this.deleteResults.Text = "Delete Daily Results";
            // 
            // scenarioGroupStatistics
            // 
            this.scenarioGroupStatistics.Controls.Add(this.seedGrid1);
            this.scenarioGroupStatistics.Controls.Add(this.addSeedButton);
            this.scenarioGroupStatistics.Dock = System.Windows.Forms.DockStyle.Left;
            this.scenarioGroupStatistics.Location = new System.Drawing.Point(344, 0);
            this.scenarioGroupStatistics.Name = "scenarioGroupStatistics";
            this.scenarioGroupStatistics.Size = new System.Drawing.Size(120, 382);
            this.scenarioGroupStatistics.TabIndex = 50;
            this.scenarioGroupStatistics.TabStop = false;
            this.scenarioGroupStatistics.Text = "Mulitple Seedings";
            this.scenarioGroupStatistics.Visible = false;
            // 
            // seedGrid1
            // 
            this.seedGrid1.DescribeRow = null;
            this.seedGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seedGrid1.EnabledGrid = true;
            this.seedGrid1.Location = new System.Drawing.Point(3, 39);
            this.seedGrid1.Name = "seedGrid1";
            this.seedGrid1.RowFilter = null;
            this.seedGrid1.RowID = null;
            this.seedGrid1.RowName = null;
            this.seedGrid1.Size = new System.Drawing.Size(114, 340);
            this.seedGrid1.Sort = "";
            this.seedGrid1.TabIndex = 46;
            this.seedGrid1.Table = null;
            // 
            // addSeedButton
            // 
            this.addSeedButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.addSeedButton.Location = new System.Drawing.Point(3, 16);
            this.addSeedButton.Name = "addSeedButton";
            this.addSeedButton.Size = new System.Drawing.Size(114, 23);
            this.addSeedButton.TabIndex = 47;
            this.addSeedButton.Text = "Add Seeds...";
            this.addSeedButton.Click += new System.EventHandler(this.addSeedButton_Click);
            // 
            // scenarioGroupDefine
            // 
            this.scenarioGroupDefine.Controls.Add(this.label8);
            this.scenarioGroupDefine.Controls.Add(this.label7);
            this.scenarioGroupDefine.Controls.Add(this.scalingNumericUpDown);
            this.scenarioGroupDefine.Controls.Add(this.label6);
            this.scenarioGroupDefine.Controls.Add(this.numSpans);
            this.scenarioGroupDefine.Controls.Add(this.label5);
            this.scenarioGroupDefine.Controls.Add(this.access_timeCombo);
            this.scenarioGroupDefine.Controls.Add(this.scenarioOptButton);
            this.scenarioGroupDefine.Controls.Add(this.seedLabel);
            this.scenarioGroupDefine.Controls.Add(this.initialSeed);
            this.scenarioGroupDefine.Controls.Add(this.numSimsLabel);
            this.scenarioGroupDefine.Controls.Add(this.label4);
            this.scenarioGroupDefine.Controls.Add(this.label3);
            this.scenarioGroupDefine.Controls.Add(this.label1);
            this.scenarioGroupDefine.Controls.Add(this.descrBox);
            this.scenarioGroupDefine.Controls.Add(this.nameBox);
            this.scenarioGroupDefine.Controls.Add(this.iStartEndDate);
            this.scenarioGroupDefine.Controls.Add(this.label2);
            this.scenarioGroupDefine.Controls.Add(this.scenarioTypeBox);
            this.scenarioGroupDefine.Controls.Add(this.scenarioTypeDesc);
            this.scenarioGroupDefine.Dock = System.Windows.Forms.DockStyle.Left;
            this.scenarioGroupDefine.Location = new System.Drawing.Point(0, 0);
            this.scenarioGroupDefine.Name = "scenarioGroupDefine";
            this.scenarioGroupDefine.Size = new System.Drawing.Size(344, 382);
            this.scenarioGroupDefine.TabIndex = 49;
            this.scenarioGroupDefine.TabStop = false;
            this.scenarioGroupDefine.Text = "Define Scenario";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(310, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 60;
            this.label8.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(192, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 59;
            this.label7.Text = "Scaling";
            // 
            // scalingNumericUpDown
            // 
            this.scalingNumericUpDown.DecimalPlaces = 1;
            this.scalingNumericUpDown.Location = new System.Drawing.Point(240, 112);
            this.scalingNumericUpDown.Name = "scalingNumericUpDown";
            this.scalingNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.scalingNumericUpDown.TabIndex = 58;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(192, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 57;
            this.label6.Text = "x";
            // 
            // numSpans
            // 
            this.numSpans.Location = new System.Drawing.Point(224, 152);
            this.numSpans.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSpans.Name = "numSpans";
            this.numSpans.Size = new System.Drawing.Size(48, 20);
            this.numSpans.TabIndex = 56;
            this.numSpans.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 23);
            this.label5.TabIndex = 55;
            this.label5.Text = "Write Data:";
            // 
            // access_timeCombo
            // 
            this.access_timeCombo.Items.AddRange(new object[] {
            "Daily",
            "Weekly",
            "Monthly"});
            this.access_timeCombo.Location = new System.Drawing.Point(80, 152);
            this.access_timeCombo.Name = "access_timeCombo";
            this.access_timeCombo.Size = new System.Drawing.Size(96, 21);
            this.access_timeCombo.TabIndex = 54;
            this.access_timeCombo.Text = "comboBox1";
            // 
            // scenarioOptButton
            // 
            this.scenarioOptButton.Location = new System.Drawing.Point(232, 304);
            this.scenarioOptButton.Name = "scenarioOptButton";
            this.scenarioOptButton.Size = new System.Drawing.Size(72, 23);
            this.scenarioOptButton.TabIndex = 53;
            this.scenarioOptButton.Text = "Options...";
            this.scenarioOptButton.Click += new System.EventHandler(this.scenarioOptButton_Click);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(88, 336);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(56, 16);
            this.seedLabel.TabIndex = 52;
            this.seedLabel.Text = "Seed";
            // 
            // initialSeed
            // 
            this.initialSeed.Location = new System.Drawing.Point(184, 336);
            this.initialSeed.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.initialSeed.Name = "initialSeed";
            this.initialSeed.Size = new System.Drawing.Size(144, 20);
            this.initialSeed.TabIndex = 51;
            // 
            // numSimsLabel
            // 
            this.numSimsLabel.Location = new System.Drawing.Point(184, 360);
            this.numSimsLabel.Name = "numSimsLabel";
            this.numSimsLabel.Size = new System.Drawing.Size(100, 16);
            this.numSimsLabel.TabIndex = 50;
            this.numSimsLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(80, 360);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 49;
            this.label4.Text = "Number of Sims:";
            // 
            // iStartEndDate
            // 
            this.iStartEndDate.End = new System.DateTime(2005, 5, 19, 17, 41, 8, 281);
            this.iStartEndDate.Location = new System.Drawing.Point(16, 104);
            this.iStartEndDate.Name = "iStartEndDate";
            this.iStartEndDate.Size = new System.Drawing.Size(160, 48);
            this.iStartEndDate.Start = new System.DateTime(2005, 5, 19, 17, 41, 8, 296);
            this.iStartEndDate.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 43;
            this.label2.Text = "Type";
            // 
            // scenarioTypeBox
            // 
            this.scenarioTypeBox.DataSource = this.scenarioTypeView;
            this.scenarioTypeBox.Location = new System.Drawing.Point(80, 184);
            this.scenarioTypeBox.Name = "scenarioTypeBox";
            this.scenarioTypeBox.Size = new System.Drawing.Size(248, 21);
            this.scenarioTypeBox.TabIndex = 42;
            this.scenarioTypeBox.SelectedIndexChanged += new System.EventHandler(this.scenarioTypeBox_SelectedIndexChanged);
            // 
            // scenarioTypeDesc
            // 
            this.scenarioTypeDesc.Location = new System.Drawing.Point(80, 216);
            this.scenarioTypeDesc.Multiline = true;
            this.scenarioTypeDesc.Name = "scenarioTypeDesc";
            this.scenarioTypeDesc.ReadOnly = true;
            this.scenarioTypeDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.scenarioTypeDesc.Size = new System.Drawing.Size(248, 80);
            this.scenarioTypeDesc.TabIndex = 48;
            // 
            // parameterPage
            // 
            this.parameterPage.Controls.Add(this.scenarioParmGrid);
            this.parameterPage.Controls.Add(this.panel4);
            this.parameterPage.Controls.Add(this.splitter3);
            this.parameterPage.Controls.Add(this.modelParmGrid);
            this.parameterPage.Location = new System.Drawing.Point(4, 22);
            this.parameterPage.Name = "parameterPage";
            this.parameterPage.Size = new System.Drawing.Size(720, 382);
            this.parameterPage.TabIndex = 3;
            this.parameterPage.Text = "Parameters";
            this.parameterPage.UseVisualStyleBackColor = true;
            // 
            // scenarioParmGrid
            // 
            this.scenarioParmGrid.DescribeRow = null;
            this.scenarioParmGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioParmGrid.EnabledGrid = true;
            this.scenarioParmGrid.Location = new System.Drawing.Point(0, 187);
            this.scenarioParmGrid.Name = "scenarioParmGrid";
            this.scenarioParmGrid.RowFilter = null;
            this.scenarioParmGrid.RowID = null;
            this.scenarioParmGrid.RowName = null;
            this.scenarioParmGrid.Size = new System.Drawing.Size(720, 195);
            this.scenarioParmGrid.Sort = "";
            this.scenarioParmGrid.TabIndex = 1;
            this.scenarioParmGrid.Table = null;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.resetParamValuesButton);
            this.panel4.Controls.Add(this.addparmButton);
            this.panel4.Controls.Add(this.removeParmButton);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 147);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(720, 40);
            this.panel4.TabIndex = 4;
            // 
            // resetParamValuesButton
            // 
            this.resetParamValuesButton.Location = new System.Drawing.Point(488, 8);
            this.resetParamValuesButton.Name = "resetParamValuesButton";
            this.resetParamValuesButton.Size = new System.Drawing.Size(144, 23);
            this.resetParamValuesButton.TabIndex = 4;
            this.resetParamValuesButton.Text = "Reset Parameter Values";
            this.resetParamValuesButton.Click += new System.EventHandler(this.resetParamValuesButton_Click);
            // 
            // addparmButton
            // 
            this.addparmButton.Location = new System.Drawing.Point(64, 8);
            this.addparmButton.Name = "addparmButton";
            this.addparmButton.Size = new System.Drawing.Size(152, 23);
            this.addparmButton.TabIndex = 2;
            this.addparmButton.Text = "Add Parameter to Scenario";
            this.addparmButton.Click += new System.EventHandler(this.addparmButton_Click);
            // 
            // removeParmButton
            // 
            this.removeParmButton.Location = new System.Drawing.Point(248, 8);
            this.removeParmButton.Name = "removeParmButton";
            this.removeParmButton.Size = new System.Drawing.Size(200, 23);
            this.removeParmButton.TabIndex = 3;
            this.removeParmButton.Text = "Remove Parameter from Scenario";
            this.removeParmButton.Click += new System.EventHandler(this.removeParmButton_Click);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 144);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(720, 3);
            this.splitter3.TabIndex = 5;
            this.splitter3.TabStop = false;
            // 
            // modelParmGrid
            // 
            this.modelParmGrid.DescribeRow = null;
            this.modelParmGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelParmGrid.EnabledGrid = true;
            this.modelParmGrid.Location = new System.Drawing.Point(0, 0);
            this.modelParmGrid.Name = "modelParmGrid";
            this.modelParmGrid.RowFilter = null;
            this.modelParmGrid.RowID = null;
            this.modelParmGrid.RowName = null;
            this.modelParmGrid.Size = new System.Drawing.Size(720, 144);
            this.modelParmGrid.Sort = "";
            this.modelParmGrid.TabIndex = 0;
            this.modelParmGrid.Table = null;
            // 
            // variablePage
            // 
            this.variablePage.Controls.Add(this.variableGrid);
            this.variablePage.Controls.Add(this.variablePanel);
            this.variablePage.Location = new System.Drawing.Point(4, 22);
            this.variablePage.Name = "variablePage";
            this.variablePage.Size = new System.Drawing.Size(720, 382);
            this.variablePage.TabIndex = 4;
            this.variablePage.Text = "Variables";
            this.variablePage.UseVisualStyleBackColor = true;
            // 
            // variableGrid
            // 
            this.variableGrid.DescribeRow = null;
            this.variableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableGrid.EnabledGrid = true;
            this.variableGrid.Location = new System.Drawing.Point(0, 72);
            this.variableGrid.Name = "variableGrid";
            this.variableGrid.RowFilter = null;
            this.variableGrid.RowID = null;
            this.variableGrid.RowName = null;
            this.variableGrid.Size = new System.Drawing.Size(720, 310);
            this.variableGrid.Sort = "";
            this.variableGrid.TabIndex = 0;
            this.variableGrid.Table = null;
            // 
            // variablePanel
            // 
            this.variablePanel.Controls.Add(this.tokenLabel);
            this.variablePanel.Controls.Add(this.tokenBox);
            this.variablePanel.Controls.Add(this.createvariableButton);
            this.variablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.variablePanel.Location = new System.Drawing.Point(0, 0);
            this.variablePanel.Name = "variablePanel";
            this.variablePanel.Size = new System.Drawing.Size(720, 72);
            this.variablePanel.TabIndex = 4;
            // 
            // tokenLabel
            // 
            this.tokenLabel.Location = new System.Drawing.Point(8, 24);
            this.tokenLabel.Name = "tokenLabel";
            this.tokenLabel.Size = new System.Drawing.Size(64, 23);
            this.tokenLabel.TabIndex = 3;
            this.tokenLabel.Text = "mnemonic";
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(80, 32);
            this.tokenBox.MaxLength = 10;
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(72, 20);
            this.tokenBox.TabIndex = 2;
            this.tokenBox.TextChanged += new System.EventHandler(this.tokenBox_TextChanged);
            // 
            // createvariableButton
            // 
            this.createvariableButton.Enabled = false;
            this.createvariableButton.Location = new System.Drawing.Point(432, 24);
            this.createvariableButton.Name = "createvariableButton";
            this.createvariableButton.Size = new System.Drawing.Size(136, 23);
            this.createvariableButton.TabIndex = 1;
            this.createvariableButton.Text = "Create New Variable";
            this.createvariableButton.Click += new System.EventHandler(this.createvariableButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.acceptButton);
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 408);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(728, 48);
            this.panel1.TabIndex = 46;
            // 
            // CreateScenario
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 456);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CreateScenario";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CreateScenario_Closing);
            this.tabControl.ResumeLayout(false);
            this.homePage.ResumeLayout(false);
            this.scenarioGroupSelectMetrics.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.scenarioGroupStatistics.ResumeLayout(false);
            this.scenarioGroupDefine.ResumeLayout(false);
            this.scenarioGroupDefine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scalingNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioTypeView)).EndInit();
            this.parameterPage.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.variablePage.ResumeLayout(false);
            this.variablePanel.ResumeLayout(false);
            this.variablePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			modelParmGrid.Clear();
			modelParmGrid.AddTextColumn("name", true);
			modelParmGrid.AddTextColumn("type", true);
			modelParmGrid.AddTextColumn("value", true);
			modelParmGrid.Reset();

			scenarioParmGrid.Clear();
			scenarioParmGrid.AddComboBoxColumn("param_id", theDb.Data.model_parameter, "name", "id", true);
			scenarioParmGrid.AddNumericColumn("aValue");

			if (currentSimulation != null && currentSimulation.type != 0)
				scenarioParmGrid.AddTextColumn("expression");

			scenarioParmGrid.Reset();

			variableGrid.Clear();
			variableGrid.AddTextColumn("token", true);
			variableGrid.AddNumericColumn("min");
			variableGrid.AddNumericColumn("max");
			variableGrid.AddNumericColumn("num_steps");
            variableGrid.AddComboBoxColumn("type", SimulationDb.variable_type, "type", "type_id", false);
			variableGrid.AddTextColumn("descr");
			variableGrid.Reset();

			seedGrid1.Clear();
			seedGrid1.AddNumericColumn("seed");
			seedGrid1.Reset();
		}

        //private DataRow scenarioHasPlan(int id)
        //{
        //    string query = "scenario_id = " + currentScenario.scenario_id;
        //    query += "AND market_plan_id = " + id;

        //    DataRow[] scenarioPlans = theDb.Data.scenario_market_plan.Select(query, "", DataViewRowState.CurrentRows);

        //    // already in scenario
        //    if (scenarioPlans.Length > 0)
        //        return scenarioPlans[0];
				
        //    return null;
        //}

		private void initializeTables()
		{
			// updateFactors();

			updateParms();
		}

		private void updateParms()
		{
			modelParmGrid.Suspend = true;
			scenarioParmGrid.Suspend = true;

			availableParms.Clear();

			foreach(MrktSimDBSchema.model_parameterRow parm in theDb.Data.model_parameter.Select())
			{
				string name = parm.name;
				string type = parm.table_name;
				string val = parm.col_name;
				int id = parm.id;

				// first check if scenario has parm if so then do not include it

				string scenarioQuery = "sim_id = " + this.currentSimulation.id + " AND " + "param_id = " + parm.id;

				if (theDb.Data.scenario_parameter.Select(scenarioQuery,"", DataViewRowState.CurrentRows).Length > 0)
					continue;

				Object[] vals = {name, type, val, id};

				availableParms.Rows.Add(vals);
			}

			availableParms.AcceptChanges();

			modelParmGrid.Suspend = false;
			scenarioParmGrid.Suspend = false;
		}

        //private void updateFactors()
        //{
        //    this.availableFactorsGrid.Suspend = true;
        //    this.scenarioFactorsGrid.Suspend = true;

        //    // clear deck
        //    availableFactors.Clear();
        //    selectedFactors.Clear();

        //    // now add plans
        //    string query = "type = " + ((int) factorType).ToString();
        //    DataRow[] rows = theDb.Data.market_plan.Select(query,"",DataViewRowState.CurrentRows);
			
        //    foreach (DataRow row in rows)
        //    {
        //        MrktSimDBSchema.market_planRow plan = (MrktSimDBSchema.market_planRow) row;

        //        string prodName = plan.GetParentRow("productmarket_plan")["product_name"].ToString();

        //        Object[] vals = {prodName, row["name"], row["id"]};

        //        //  in or out of scenario
        //        if (scenarioHasPlan((int) row["id"]) != null)
        //            selectedFactors.Rows.Add(vals);
        //        else
        //            availableFactors.Rows.Add(vals);
        //    }

        //    availableFactors.AcceptChanges();
        //    selectedFactors.AcceptChanges();

        //    this.availableFactorsGrid.Suspend = false;
        //    this.scenarioFactorsGrid.Suspend = false;
        //}

        //private void updatePlans()
        //{
        //    availableGrid.Suspend = true;
        //    scenarioMarketPlanGrid.Suspend = true;
			
        //    // remove plans
        //    availablePlans.Clear();
        //    selectedPlans.Clear();
			

        //    // now add plans
        //    string query = "type = " + ((int) ModelDb.PlanType.MarketPlan).ToString();
        //    DataRow[] rows = theDb.Data.market_plan.Select(query,"",DataViewRowState.CurrentRows);

        //    foreach (DataRow row in rows)
        //    {
        //        MrktSimDBSchema.market_planRow plan = (MrktSimDBSchema.market_planRow) row;

        //        string prodName = plan.GetParentRow("productmarket_plan")["product_name"].ToString();

        //        Object[] vals = {prodName, row["name"], row["id"]};

        //        if (scenarioHasPlan((int) row["id"]) != null)
        //        {
        //            selectedPlans.Rows.Add(vals);
        //        }
        //            // this tests if the product already has a plan
        //        else if (theDb.ScenarioHasProductPlan(currentScenario.scenario_id, (int) row["product_id"]) == null ||
        //            !showAllBox.Checked)
        //        {
        //            availablePlans.Rows.Add(vals);
        //        }
        //    }

        //    availablePlans.AcceptChanges();
        //    selectedPlans.AcceptChanges();

        //    availableGrid.Suspend = false;
        //    scenarioMarketPlanGrid.Suspend = false;
        //}

	
		private void acceptButton_Click(object sender, System.EventArgs e)
		{
            if (SimulationDb.NumSims(currentSimulation) == 0)
			{
				MessageBox.Show("This scenario will generate no sims");

				return;
			}

			string parserError = parserTest();

			if (parserError != null)
			{
				MessageBox.Show("Error parsing expressions: " + parserError);

				return;
			}

			// update the sim seed if not a statistical run
            if (currentSimulation != null && ((SimulationDb.SimulationType)currentSimulation.type) != SimulationDb.SimulationType.Statistical)
			{
				int seed = (int) Math.Floor((double) this.initialSeed.Value);
				
				string query = "id = " + currentSimulation.id;

				DataRow[] simseeds = theDb.Data.scenario_simseed.Select(query, "", DataViewRowState.CurrentRows);

				if (simseeds.Length > 0)
					simseeds[0]["seed"] = seed;
				else
					// create seed with value
					theDb.CreateScenarioSimSeed(currentSimulation, seed);
			}

			
			
			bool noneChecked = true;

			for(int ii = 0; ii < metricBox.Items.Count; ++ii)
			{
				if( metricBox.GetItemChecked(ii) )
				{
					noneChecked = false;
					break;
				}
			}

			if(noneChecked && deleteResults.Checked)
			{
				MessageBox.Show("Error: No metrics are computed and results are deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if(noneChecked)
			{
				DialogResult rslt = MessageBox.Show("Warning: No metrics are being computed","Warning", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
				if(rslt == DialogResult.Cancel)
				{
					return;
				}
			}

			// create checked items for scenario
			updateScenarioMetrics();

			string calibrateError = createCalibrationVariables();

			if (calibrateError != null)
			{
				MessageBox.Show(calibrateError);
				return;
			}

			SaveModel();
		}

        //private void addPlanButton_Click(object sender, System.EventArgs e)
        //{
			

        //    ArrayList rows = availableGrid.GetSelected();

        //    foreach(DataRow row in rows)
        //    {
        //        int market_plan_id = (int) row["id"];

        //        MrktSimDBSchema.scenario_market_planRow scenarioPlan = theDb.AddMarketPlanToScenario(this.CurrentScenario.scenario_id, market_plan_id);
        //    }
	
        //    updateParms();

			
        //}

        //private void removePlanButton_Click(object sender, System.EventArgs e)
        //{
        //    ArrayList rows = scenarioMarketPlanGrid.GetSelected();

        //    foreach(DataRow row in rows)
        //    {
        //        // need to delete from database table
        //        DataRow plan = scenarioHasPlan((int) row["id"]);

        //        if (plan != null)
        //            plan.Delete();

        //        row.Delete();
        //    }

        //    updatePlans();
        //    updateParms();
        //}

        //private void addFactorButton_Click(object sender, System.EventArgs e)
        //{
        //    ArrayList rows = availableFactorsGrid.GetSelected();

        //    foreach(DataRow row in rows)
        //    {
        //        int market_plan_id = (int) row["id"];

        //        MrktSimDBSchema.scenario_market_planRow scenarioPlan = theDb.AddMarketPlanToScenario(this.CurrentScenario.scenario_id, market_plan_id);
        //    }

        //    updateFactors();
        //    updateParms();
        //}

        //private void removeFactorButton_Click(object sender, System.EventArgs e)
        //{
        //    ArrayList rows = scenarioFactorsGrid.GetSelected();

        //    foreach(DataRow row in rows)
        //    {
        //        // need to delete from database table
        //        DataRow plan = scenarioHasPlan((int) row["id"]);

        //        if (plan != null)
        //            plan.Delete();

        //        row.Delete();
        //    }

        //    updateFactors();
        //    updateParms();
        //}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
            //if (this.CurrentSimulation != null)
            //    theDb.CloseScenario(this.CurrentSimulation.id);
            //else
            //    theDb.CloseScenario(ModelDb.AllID);

			this.Close();
		}

		private void showAllBox_CheckedChanged(object sender, System.EventArgs e)
		{
		}

		private void addparmButton_Click(object sender, System.EventArgs e)
		{
			ArrayList rows = this.modelParmGrid.GetSelected();

			foreach(DataRow row in rows)
			{
				int parm_id = (int) row["id"];
				theDb.CreateScenarioParameter(CurrentSimulation.scenario_id, parm_id);
			}

			updateParms();
		}

		private void removeParmButton_Click(object sender, System.EventArgs e)
		{

			ArrayList rows = this.scenarioParmGrid.GetSelected();

			foreach(DataRow row in rows)
			{
				row.Delete();
			}

			updateParms();
		}

		private void CreateScenario_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (theDb != null)
			{
                //if (CurrentSimulation != null)
                //    theDb.CloseScenario(this.CurrentSimulation.id);
                //else
                //    theDb.CloseScenario(ModelDb.AllID);
			}
		}

		private void createvariableButton_Click(object sender, System.EventArgs e)
		{
			string token = tokenBox.Text;

			token = token.Replace(" ", "_");

            theDb.CreateScenarioVariable(this.CurrentSimulation, token);
		}

		private void tokenBox_TextChanged(object sender, System.EventArgs e)
		{
			if ( tokenBox.Text != null && tokenBox.Text.Length > 0)
			{
				createvariableButton.Enabled = true;
			}
			else
			{
				createvariableButton.Enabled = true;
			}
		}

		private void resetParamValuesButton_Click(object sender, System.EventArgs e)
		{
			// now check if parameter already exists
			string query = "id = " + this.currentSimulation.id;

			DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query, "", DataViewRowState.CurrentRows);

			foreach(MrktSimDBSchema.scenario_parameterRow parmRow in parmRows)
			{
                MrktSimDBSchema.model_parameterRow modelParm = parmRow.model_parameterRow;

                double val = 0; // theDb.EvaluateModelParameter(modelParm);

				parmRow.aValue = val;
			}
		}

		private void addSeedButton_Click(object sender, System.EventArgs e)
		{
			InputDouble dlg = new InputDouble();

			dlg.Max = 10;
			dlg.Value = 3;
			dlg.Min = 1;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				int numSeeds = (int) Math.Floor(dlg.Value);

				// create this number of seeds
				// make sure they are different

				int curMin = 100;
				for(int ii = 0; ii < numSeeds; ++ii)
				{
					int seed = rand.Next(curMin, curMin + 300);
				
					// create seed with value
                    theDb.CreateScenarioSimSeed(CurrentSimulation, seed);

					curMin = seed + 1;
				}
			}
		}

		private void scenarioTypeBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (scenarioTypeBox.ValueMember == "")
				return;

			if (scenarioTypeBox.SelectedValue == null)
				return;

			currentSimulation.type = (byte) scenarioTypeBox.SelectedValue;

			this.tabControl.SuspendLayout();

			setUpScenarioPage();

            this.numSimsLabel.Text = SimulationDb.NumSims(currentSimulation).ToString();

			this.createTableStyle();

			this.tabControl.ResumeLayout();
		}

		private void setUpScenarioPage()
		{
            SimulationDb.SimulationType type = SimulationDb.SimulationType.Standard; // standard is default

			this.SuspendLayout();

			// default
			this.scenarioGroupSelectMetrics.Visible = true;
			this.scenarioGroupStatistics.Visible = false;
			this.initialSeed.Value = 123456;
			this.initialSeed.Visible = true;
			this.seedLabel.Visible = true;
			this.scenarioOptButton.Visible = false;

			if (currentSimulation != null)
			{

                type = (SimulationDb.SimulationType)currentSimulation.type;

				this.scenarioTypeDesc.Text = MrktSimControl.MrktSimMessage("Scenario.type." ); // + ModelDb.simulation_typ.simulcurrentSimulation.type);

				string query = "id = " + currentSimulation.id;

				DataRow[] simseeds = theDb.Data.scenario_simseed.Select(query, "", DataViewRowState.CurrentRows);

				if (simseeds.Length > 0)
				{
					object obj = simseeds[0]["seed"];
					int seed = (int) obj;
					this.initialSeed.Value = seed;
				}
			}
			
			switch (type)
			{
                case SimulationDb.SimulationType.Parallel:
					break;

                case SimulationDb.SimulationType.Serial:
					break;

                case SimulationDb.SimulationType.Random:
					break;

                case SimulationDb.SimulationType.Optimize:
					this.scenarioOptButton.Visible = true;
					break;

                case SimulationDb.SimulationType.Statistical:	// statistical
					this.scenarioGroupStatistics.Visible = true;
					this.initialSeed.Visible = false;
					this.seedLabel.Visible = false;
					this.scenarioOptButton.Visible = true;
					break;
                case SimulationDb.SimulationType.Calibration:
					this.scenarioOptButton.Visible = true;
					break;

			}

			this.ResumeLayout(false);
		}

		private void initializeMetricSelectBox()
		{
			for(int ii = 0; ii < metricBox.Items.Count; ++ii)
			{
				metricBox.SetItemCheckState(ii, CheckState.Unchecked);			
			}

			MrktSimDBSchema.scenario_metricRow[] metrics = CurrentSimulation.Getscenario_metricRows();

			foreach(MrktSimDBSchema.scenario_metricRow metric in metrics)
			{
				// look for token in list

				for(int ii = 0; ii < metricBox.Items.Count; ++ii)
				{
					if ( ((Metric) metricBox.Items[ii]).Token == metric.token)
					{
						metricBox.SetItemCheckState(ii, CheckState.Checked);
						break;
					}
				}
			}
		}

		private void updateScenarioMetrics()
		{
			for(int ii = 0; ii < metricBox.Items.Count; ++ii)
			{
				if (metricBox.GetItemChecked(ii))
				{
					theDb.CreateScenarioMetric(CurrentSimulation, ((Metric) metricBox.Items[ii]).Token);
				}
				else
				{
					theDb.DeleteScenarioMetric(CurrentSimulation, ((Metric) metricBox.Items[ii]).Token);
				}
			}
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.tabControl.SelectedIndex == 0)
			{
                numSimsLabel.Text = SimulationDb.NumSims(currentSimulation).ToString();
			}
		}


		private string parserTest()
		{
			// check if this actually uses the paser
			if (currentSimulation == null)
				return null;

			// one lowly sim
            if (currentSimulation.type == (byte)SimulationDb.SimulationType.Standard)
				return null;

            if (currentSimulation.type == (byte)SimulationDb.SimulationType.CheckPoint)
				return null;

            if (currentSimulation.type == (byte)SimulationDb.SimulationType.Statistical)
				return null;

            if (currentSimulation.type == (byte)SimulationDb.SimulationType.Calibration)
				return null;

			if (currentSimulation.Getscenario_variableRows().Length == 0)
				return "No variables defined";
	
			MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
			MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();


			// values and stepsize
			double[] val = new double[variables.Length];
			int[] curEval = new int[variables.Length];
			string[] tokens = new string[variables.Length + 1];

			for( int ii = 0; ii < variables.Length; ii++)
			{
				// set each variable to the beginning value
				val[ii] = variables[ii].min;

				tokens[ii] = variables[ii].token;
			}

			tokens[variables.Length] = "CurVal";

			SimpleParser parser = new SimpleParser(tokens);

			// update scenario parameter values
			parser.updateValues(val);

			bool updated = false;

			foreach(MrktSimDBSchema.scenario_parameterRow param in parameters)
			{
				parser.updateValue("CurVal", param.aValue);

				if (param.expression != null && param.expression.Length > 0)
				{
					updated = true;
					try 
					{
						double aval = parser.ParseEquation(param.expression);
					}
					catch(Exception e)
					{
						return e.Message;
					}
				}
			}

			if (!updated)
				return "Parameters do not depend on Variables";

			return null;
		}

		private void checkAllBtn_Click(object sender, System.EventArgs e)
		{
			for(int ii = 0; ii < metricBox.Items.Count; ++ii)
			{
				metricBox.SetItemCheckState(ii, CheckState.Checked);			
			}

			updateScenarioMetrics();

		}

		private void scenarioOptButton_Click(object sender, System.EventArgs e)
		{
			if (currentSimulation == null)
				return;

			int type = 0; // standard is default

				
			type = currentSimulation.type;

            switch ((SimulationDb.SimulationType)type)
			{
                case SimulationDb.SimulationType.Parallel:
					break;

                case SimulationDb.SimulationType.Serial:
					break;

                case SimulationDb.SimulationType.Random:
					break;

                case SimulationDb.SimulationType.Optimize:
					// launch optimization dialgoue
					break;

                case SimulationDb.SimulationType.Statistical:	
					// launch statistical dialogue
					break;
                case SimulationDb.SimulationType.Calibration:
					CreateCalibration dlg = new CreateCalibration(currentSimulation);

					dlg.ShowDialog();
					break;
			}
		
		}

		private string createCalibrationVariables()
		{
			
			// check if this actually uses the paser
			if (currentSimulation == null)
				return null;

			// this only does calibration
            if (currentSimulation.type != (byte)SimulationDb.SimulationType.Calibration)
				return null;

			MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

			// compute list of products for this scenario
			ArrayList products = new ArrayList();

			int[] parameterProduct = new int[parameters.Length];

			int parmDex = 0;
			foreach(MrktSimDBSchema.scenario_parameterRow param in parameters)
			{
				MrktSimDBSchema.model_parameterRow modelParm = param.model_parameterRow;

				// get product id if possible
				DataRow[] rows = theDb.Data.Tables[modelParm.table_name].Select(modelParm.filter, "", DataViewRowState.CurrentRows);

				if (rows.Length == 0)
				{
					return "Reference error with parameter " + modelParm.name;
				}

				int product_id = -1;
				
				try
				{	
					product_id = (int) rows[0]["product_id"];
				}
				catch(Exception)
				{
					return "No product associated with parameter " + modelParm.name;
				}

				if (product_id == ModelDb.AllID)
				{
					return "Parameter " + modelParm.name + " does not reference a specific product";
				}

				// check if in list already

				if (products.IndexOf(product_id) < 0)
				{
					products.Add(product_id);
				}

				parameterProduct[parmDex] = product_id;
				++parmDex;
			}

			MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();

			bool createParms = false;

			if (variables.Length != products.Count)
				createParms = true;
			else
			{
				for(int ii = 0; !createParms && ii < variables.Length; ++ii)
				{
					if (variables[ii].product_id == ModelDb.AllID)
					{
						createParms = true;
					}
				}
			}


			if (createParms)
			{
				if (variables.Length > 0)
				{
					DialogResult rslt = MessageBox.Show("Current variables will be deleted for calibration run", "Delete Variables?",MessageBoxButtons.OKCancel);

					if (rslt != DialogResult.OK)
						return "Calibration Run cannot be saved";
				}
			}
			else
			{
				return null;
			}

			// remove any pre-existing variables
			foreach(MrktSimDBSchema.scenario_variableRow variable in variables)
			{
				variable.Delete();
			}

			// create one variable for each product referenced
			foreach(int product_id in products)
			{
				string prodName = theDb.Data.product.FindByproduct_id(product_id).product_name;

				if (prodName.Length > 8)
				{
					prodName = prodName.Substring(0,8);
				}

				MrktSimDBSchema.scenario_variableRow variable = theDb.CreateScenarioVariable(currentSimulation, prodName);
				variable.product_id =  product_id;

				// all parsm referencing this product should evaluate off this variable
				for(int ii = 0; ii < parameters.Length; ++ii)
				{	
					if (parameterProduct[ii] == product_id)
					{
						MrktSimDBSchema.scenario_parameterRow param = parameters[ii];
						param.expression = variable.token;
					}
				}
			}

			return null;
		}

		private void disableSimDependentControls()
		{
			// scenario tab
//			this.access_timeCombo.Enabled = false;
//			this.numSpans.Enabled = false;
//			this.iStartEndDate.Enabled = false;
			this.scenarioTypeBox.Enabled = false;
			this.checkAllBtn.Enabled = false;
			this.deleteResults.Enabled = false;
			this.addSeedButton.Enabled = false;
			this.seedGrid1.EnabledGrid = false;
			
			this.metricBox.Enabled = false;

			// variables tab
			this.variableGrid.EnabledGrid= false;
			this.createvariableButton.Enabled = false;
		}
	}
}
