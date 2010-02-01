using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using MrktSimDb.Metrics;
using System.Data;
using Common.Utilities; // ProductForest
using Common.Dialogs;
using System.IO;

using MarketSimUtilities;
using Utilities.Graphing;

namespace Results
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
	/// 
	public class ResultsForm : System.Windows.Forms.Form
	{
        private enum ProductDesc
        {
            SKU,
            Brand
        };

        public enum SummaryReportType
        {
            CalibrationMaster,
            FWReport,
            TrialReport,
            ConsumerPanel,
            None
        };

        static Color[] colorOpts = 
				{
					Color.Red,
					Color.Green,
					Color.Blue,
					Color.Violet,
					Color.Black,
					Color.Orange,
					Color.DarkGreen,
					Color.DarkBlue,
					Color.Brown,
					Color.DarkRed,
					Color.DarkOrange,
					Color.Gold,
					Color.Magenta,
					Color.Yellow
				};
		private System.Windows.Forms.RadioButton oneChart;
		private System.Windows.Forms.Button regressionButton;
		private System.Windows.Forms.Button recalcButton;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Button refreshButton;
		private ProductForest myForest;
		private System.Windows.Forms.ContextMenu ProductTreeContextMenu;
		private System.Windows.Forms.MenuItem[] Product_Types;
		private System.Windows.Forms.MenuItem SelectByType;
		private System.Windows.Forms.Button csvDump;
		private System.Windows.Forms.GroupBox calScopeBox;
		private System.Windows.Forms.GroupBox simScopeBox;
		private System.Windows.Forms.CheckedListBox simMetricBox;
		private System.Windows.Forms.CheckedListBox calMetricBox;
        private ProductTree productTree;
        private ProductTree hiddenProductTree;
        private Common.Utilities.StartEndDate startEndDate1;
		private Common.Utilities.ChannelPicker channelPicker1;
		private Common.Utilities.SegmentPicker segmentPicker1;
		private Results.VariableControl variableControl1;
        private Button UnGroupButton;
        private Button GroupButton;
        private CheckBox AdvStatCheck;
        private Button writeCsvButton;
        private CheckBox selectAllCheckBox;
        private Button newSettingsButton;
        private ComboBox settingsComboBox;
        private Label label2;
        private Button delButton;
        private Button saveButton;
        private Button button1;
        private CheckBox reloadCheckBox;
        private GroupBox summaryReportGroupBox;
        private Button trialReportButton;
        private Button fwReportButton;
        private Button calMasterButton;
        private RadioButton bothLevelsRadioButton;
        private RadioButton brandLevelRadioButton;
        private RadioButton productLevelRadioButton;
		// private Results.VariableControl variableControl1;

		static int colorIndex;

		static public void InitializeColor()
		{
			colorIndex = 0;
		}

		static public Color CurrentColor
		{
			get
			{
				return colorOpts[colorIndex];
			}
		}

		static public void NextColor()
		{
			colorIndex++;

			if (colorIndex >= colorOpts.Length)
				colorIndex = 0;
		}

        private ToolTip toolTip;

		DataView  dataColView;

		static DataTable dataCol = null;

		private DataTable simTable = null;

        private ResultsDb theDb;

        public ResultsDb Db
		{
			set
			{
				theDb = value;

				this.Text = "Results for " + theDb.Model.model_name;

				//productPicker1.Db = theDb;
				this.productTree.Db = theDb;
                hiddenProductTree.Db = theDb;
				segmentPicker1.Db = theDb;
				channelPicker1.Db = theDb;
				startEndDate1.Db = theDb;
				
				//set up simulation table
				initializeSimTable();

				simBox.DataSource = simTable;
				simBox.DisplayMember = "sim";

				this.variableControl1.Db = theDb;

				this.SelectByType.MenuItems.Clear();

				DataRow[] Types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);

				Product_Types = new System.Windows.Forms.MenuItem[Types.Length];
			
				int i = 0;
				foreach(MrktSimDBSchema.product_typeRow row in Types)
				{
					System.Windows.Forms.MenuItem menuItem = new MenuItem(row.type_name);
					menuItem.Click += new System.EventHandler(this.SelectByType_Click);
					Product_Types[i] = menuItem;
					i++;
				}

				this.SelectByType.MenuItems.AddRange(Product_Types);

				configureSummaryLists();
			}

			get
			{
				return theDb;
			}
		}


		private void refresh()
		{		
			theDb.Refresh();
			initializeSimTable();
			this.productTree.Rebuild();
		}

		private Color currentColor = colorOpts[0];

		// designer variables

		private System.Windows.Forms.Button graphButton;
		private System.Windows.Forms.ListBox dataListBox;
		private System.Windows.Forms.GroupBox graphBox;
		private System.Windows.Forms.Button addToGraphButton;
		private System.Windows.Forms.Button removeItemButton;
		private System.Windows.Forms.ListBox columnNameBox;
		private System.Windows.Forms.TextBox descriptionBox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox timeSpan;
		private System.Windows.Forms.NumericUpDown dailyAverageBox;
		private System.Windows.Forms.ComboBox dataTableBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button summaryButton;
		private System.Windows.Forms.TabPage chartPage;
		private System.Windows.Forms.TabPage summaryPage;
		private System.Windows.Forms.RadioButton segmentPerChart;
		private System.Windows.Forms.RadioButton productPerChart;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox sumSegments;
        private System.Windows.Forms.CheckBox sumProducts;
		private System.Windows.Forms.Button saveSummarySettingsButton;
		private System.Windows.Forms.CheckedListBox simBox;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage paramPage;
		private System.Windows.Forms.GroupBox timingBox;
		private System.ComponentModel.IContainer components;

		static ResultsForm()
		{
			dataCol = new DataTable("AvailableData");
			dataCol.Columns.Add("userName", typeof(string));
			dataCol.Columns.Add("evalData", typeof(EvalData));
			dataCol.Columns.Add("dataType", typeof(DataType));
			dataCol.Columns.Add("productDesc", typeof(ProductDesc));
			dataCol.Columns.Add("descr", typeof(string));

		
			

			foreach(EvalData evalData in SqlEval.Variables)
			{
                if (evalData.hidden)
                {
                    continue;
                }

				DataRow row = dataCol.NewRow();
				row["evalData"] = evalData;
				row["userName"] = MrktSimControl.MrktSimColumnName("results_std", evalData.Token);
				row["dataType"] = evalData.type;
	
                row["productDesc"] = ProductDesc.SKU;

				row["descr"] = MrktSimControl.MrktSimColumnName("results_std", evalData.Token + ".desc");

				dataCol.Rows.Add(row);
			}
			
			dataCol.AcceptChanges();
		}

        public ResultsForm() : this( false ){
        }
            
		public ResultsForm( bool devlMode )
		{
//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            variableControl1.SetOwner( this );
            this.toolTip = new ToolTip();
            this.toolTip.SetToolTip( this.saveButton, "Saves ALL current control settings on this form.\r\nUse Ctrl-Click to change the description for the current settings." );
            this.hiddenProductTree = new MarketSimUtilities.ProductTree();

            Settings.Read();

			//productTree.SelectedItemsChanged +=new ProductTree.SelectedItems(productTree_SelectedItemsChanged);

			dataColView = new DataView();
			dataColView.Table = dataCol;

			columnNameBox.DataSource = dataColView;
			columnNameBox.DisplayMember = "userName";

            dataTableBox.DataSource = Enum.GetNames(typeof(DataType));

            dataTableBox.SelectedItem = Enum.GetName(typeof(DataType), DataType.SimResults);
            
			dataColView.RowFilter = "dataType = " + (int) DataType.SimResults;

			timeSpan.SelectedIndex = 1;

			simTable = new DataTable("SimTable");

			simTable.Columns.Add("sim", typeof(string));
			simTable.Columns.Add("run_ids", typeof(ArrayList));
            simTable.Columns.Add("sim_ids", typeof(ArrayList));
            simTable.Columns.Add("sim_names", typeof(ArrayList));

            //sumSegments.Enabled = false;
            //sumProducts.Enabled = false;

			foreach(Metric metric in MetricMan.SimSummaryMetrics)
				this.simMetricBox.Items.Add(metric);

			foreach(Metric metric in MetricMan.CalibrationMetrics)
				this.calMetricBox.Items.Add(metric);

            ////if (Settings.DefaultGraphOption != null)
            ////{
            ////    productPerChart.Checked = Settings.DefaultGraphOption.productPerGraph;
            ////    sumSegments.Checked = Settings.DefaultGraphOption.sumSegments;

            ////    segmentPerChart.Checked = Settings.DefaultGraphOption.segmentPerGraph;
            ////    sumProducts.Checked = Settings.DefaultGraphOption.sumProducts;

            ////    if (Settings.DefaultGraphOption.tokens != null)
            ////    {
            ////        // find by token
            ////        foreach(string token in Settings.DefaultGraphOption.tokens)
            ////        {
            ////            foreach(DataRow row in dataCol.Rows)
            ////            {
            ////                EvalData eval = (EvalData) row["evalData"];

            ////                if (eval.Token == token)
            ////                {
            ////                    // add to list
            ////                    dataListBox.Items.Add(eval);
            ////                    break;
            ////                }
            ////            }
            ////        }
            ////    }
            ////}
            ////else
            ////{
            ////    productPerChart.Checked = true;
            ////    sumSegments.Checked = true;
            ////    segmentPerChart.Checked = false;
            ////    sumProducts.Checked = true;
            ////}

		

			// regression button


            if( devlMode == false ) {
                regressionButton.Visible = false;
                //summaryReportGroupBox.Visible = false;
            }
            else {
                //summaryReportGroupBox.Visible = true;
                regressionButton.Visible = true;
            }

            if(!Database.Nimo )
            {
                this.summaryReportGroupBox.Visible = false;
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ResultsForm ) );
            this.graphButton = new System.Windows.Forms.Button();
            this.dataListBox = new System.Windows.Forms.ListBox();
            this.addToGraphButton = new System.Windows.Forms.Button();
            this.graphBox = new System.Windows.Forms.GroupBox();
            this.columnNameBox = new System.Windows.Forms.ListBox();
            this.removeItemButton = new System.Windows.Forms.Button();
            this.dataTableBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.UnGroupButton = new System.Windows.Forms.Button();
            this.GroupButton = new System.Windows.Forms.Button();
            this.reloadCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.delButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.newSettingsButton = new System.Windows.Forms.Button();
            this.settingsComboBox = new System.Windows.Forms.ComboBox();
            this.simBox = new System.Windows.Forms.CheckedListBox();
            this.selectAllCheckBox = new System.Windows.Forms.CheckBox();
            this.AdvStatCheck = new System.Windows.Forms.CheckBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.timingBox = new System.Windows.Forms.GroupBox();
            this.startEndDate1 = new Common.Utilities.StartEndDate();
            this.timeSpan = new System.Windows.Forms.ComboBox();
            this.dailyAverageBox = new System.Windows.Forms.NumericUpDown();
            this.summaryButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.segmentPicker1 = new Common.Utilities.SegmentPicker();
            this.channelPicker1 = new Common.Utilities.ChannelPicker();
            this.productTree = new MarketSimUtilities.ProductTree();
            this.ProductTreeContextMenu = new System.Windows.Forms.ContextMenu();
            this.SelectByType = new System.Windows.Forms.MenuItem();
            this.regressionButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.oneChart = new System.Windows.Forms.RadioButton();
            this.sumProducts = new System.Windows.Forms.CheckBox();
            this.sumSegments = new System.Windows.Forms.CheckBox();
            this.segmentPerChart = new System.Windows.Forms.RadioButton();
            this.productPerChart = new System.Windows.Forms.RadioButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.chartPage = new System.Windows.Forms.TabPage();
            this.summaryPage = new System.Windows.Forms.TabPage();
            this.summaryReportGroupBox = new System.Windows.Forms.GroupBox();
            this.bothLevelsRadioButton = new System.Windows.Forms.RadioButton();
            this.brandLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.productLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.trialReportButton = new System.Windows.Forms.Button();
            this.fwReportButton = new System.Windows.Forms.Button();
            this.calMasterButton = new System.Windows.Forms.Button();
            this.writeCsvButton = new System.Windows.Forms.Button();
            this.calScopeBox = new System.Windows.Forms.GroupBox();
            this.calMetricBox = new System.Windows.Forms.CheckedListBox();
            this.simScopeBox = new System.Windows.Forms.GroupBox();
            this.simMetricBox = new System.Windows.Forms.CheckedListBox();
            this.csvDump = new System.Windows.Forms.Button();
            this.recalcButton = new System.Windows.Forms.Button();
            this.saveSummarySettingsButton = new System.Windows.Forms.Button();
            this.paramPage = new System.Windows.Forms.TabPage();
            this.variableControl1 = new Results.VariableControl();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon( this.components );
            this.graphBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.timingBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dailyAverageBox)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.chartPage.SuspendLayout();
            this.summaryPage.SuspendLayout();
            this.summaryReportGroupBox.SuspendLayout();
            this.calScopeBox.SuspendLayout();
            this.simScopeBox.SuspendLayout();
            this.paramPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphButton
            // 
            this.graphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.graphButton.Location = new System.Drawing.Point( 433, 240 );
            this.graphButton.Name = "graphButton";
            this.graphButton.Size = new System.Drawing.Size( 96, 23 );
            this.graphButton.TabIndex = 0;
            this.graphButton.Text = "Create Charts...";
            this.graphButton.Click += new System.EventHandler( this.graphButton_Click );
            // 
            // dataListBox
            // 
            this.dataListBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataListBox.Location = new System.Drawing.Point( 371, 16 );
            this.dataListBox.Name = "dataListBox";
            this.dataListBox.Size = new System.Drawing.Size( 193, 238 );
            this.dataListBox.TabIndex = 9;
            this.dataListBox.SelectedIndexChanged += new System.EventHandler( this.dataListBox_SelectedIndexChanged );
            // 
            // addToGraphButton
            // 
            this.addToGraphButton.Location = new System.Drawing.Point( 207, 96 );
            this.addToGraphButton.Name = "addToGraphButton";
            this.addToGraphButton.Size = new System.Drawing.Size( 128, 23 );
            this.addToGraphButton.TabIndex = 11;
            this.addToGraphButton.Text = "add to graph >";
            this.addToGraphButton.Click += new System.EventHandler( this.addToGraphButton_Click );
            // 
            // graphBox
            // 
            this.graphBox.Controls.Add( this.dataListBox );
            this.graphBox.Controls.Add( this.columnNameBox );
            this.graphBox.Controls.Add( this.addToGraphButton );
            this.graphBox.Controls.Add( this.removeItemButton );
            this.graphBox.Controls.Add( this.dataTableBox );
            this.graphBox.Controls.Add( this.label1 );
            this.graphBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.graphBox.Location = new System.Drawing.Point( 0, 280 );
            this.graphBox.Name = "graphBox";
            this.graphBox.Size = new System.Drawing.Size( 567, 257 );
            this.graphBox.TabIndex = 12;
            this.graphBox.TabStop = false;
            this.graphBox.Text = "Available Data";
            // 
            // columnNameBox
            // 
            this.columnNameBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.columnNameBox.Location = new System.Drawing.Point( 3, 16 );
            this.columnNameBox.Name = "columnNameBox";
            this.columnNameBox.Size = new System.Drawing.Size( 193, 238 );
            this.columnNameBox.TabIndex = 14;
            this.columnNameBox.SelectedIndexChanged += new System.EventHandler( this.columnNameBox_SelectedIndexChanged );
            // 
            // removeItemButton
            // 
            this.removeItemButton.Location = new System.Drawing.Point( 207, 120 );
            this.removeItemButton.Name = "removeItemButton";
            this.removeItemButton.Size = new System.Drawing.Size( 128, 23 );
            this.removeItemButton.TabIndex = 14;
            this.removeItemButton.Text = "< remove from graph";
            this.removeItemButton.Click += new System.EventHandler( this.removeItemButton_Click );
            // 
            // dataTableBox
            // 
            this.dataTableBox.Location = new System.Drawing.Point( 207, 64 );
            this.dataTableBox.Name = "dataTableBox";
            this.dataTableBox.Size = new System.Drawing.Size( 128, 21 );
            this.dataTableBox.TabIndex = 19;
            this.dataTableBox.SelectedIndexChanged += new System.EventHandler( this.dataTableBox_SelectedIndexChanged );
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 208, 48 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 64, 16 );
            this.label1.TabIndex = 27;
            this.label1.Text = "Data Type";
            // 
            // descriptionBox
            // 
            this.descriptionBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descriptionBox.Location = new System.Drawing.Point( 0, 537 );
            this.descriptionBox.Multiline = true;
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            this.descriptionBox.Size = new System.Drawing.Size( 567, 32 );
            this.descriptionBox.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.UnGroupButton );
            this.groupBox2.Controls.Add( this.GroupButton );
            this.groupBox2.Controls.Add( this.reloadCheckBox );
            this.groupBox2.Controls.Add( this.button1 );
            this.groupBox2.Controls.Add( this.saveButton );
            this.groupBox2.Controls.Add( this.delButton );
            this.groupBox2.Controls.Add( this.label2 );
            this.groupBox2.Controls.Add( this.newSettingsButton );
            this.groupBox2.Controls.Add( this.settingsComboBox );
            this.groupBox2.Controls.Add( this.simBox );
            this.groupBox2.Controls.Add( this.selectAllCheckBox );
            this.groupBox2.Controls.Add( this.AdvStatCheck );
            this.groupBox2.Controls.Add( this.refreshButton );
            this.groupBox2.Controls.Add( this.label5 );
            this.groupBox2.Controls.Add( this.timingBox );
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 224, 595 );
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // UnGroupButton
            // 
            this.UnGroupButton.Location = new System.Drawing.Point( 84, 271 );
            this.UnGroupButton.Name = "UnGroupButton";
            this.UnGroupButton.Size = new System.Drawing.Size( 75, 23 );
            this.UnGroupButton.TabIndex = 34;
            this.UnGroupButton.Text = "UnGroup";
            this.UnGroupButton.UseVisualStyleBackColor = true;
            this.UnGroupButton.Click += new System.EventHandler( this.UnGroupButton_Click );
            // 
            // GroupButton
            // 
            this.GroupButton.Location = new System.Drawing.Point( 3, 271 );
            this.GroupButton.Name = "GroupButton";
            this.GroupButton.Size = new System.Drawing.Size( 75, 23 );
            this.GroupButton.TabIndex = 33;
            this.GroupButton.Text = "Group";
            this.GroupButton.UseVisualStyleBackColor = true;
            this.GroupButton.Click += new System.EventHandler( this.GroupButton_Click );
            // 
            // reloadCheckBox
            // 
            this.reloadCheckBox.AutoSize = true;
            this.reloadCheckBox.Location = new System.Drawing.Point( 15, 61 );
            this.reloadCheckBox.Name = "reloadCheckBox";
            this.reloadCheckBox.Size = new System.Drawing.Size( 169, 17 );
            this.reloadCheckBox.TabIndex = 43;
            this.reloadCheckBox.Text = "Reload settings when opening";
            this.reloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.button1.Location = new System.Drawing.Point( 193, 59 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 25, 23 );
            this.button1.TabIndex = 42;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler( this.button1_Click );
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))) );
            this.saveButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.saveButton.ForeColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))) );
            this.saveButton.Location = new System.Drawing.Point( 58, 9 );
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size( 55, 23 );
            this.saveButton.TabIndex = 41;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
            // 
            // delButton
            // 
            this.delButton.BackColor = System.Drawing.SystemColors.Control;
            this.delButton.Enabled = false;
            this.delButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.delButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.delButton.Location = new System.Drawing.Point( 190, 9 );
            this.delButton.Name = "delButton";
            this.delButton.Size = new System.Drawing.Size( 26, 23 );
            this.delButton.TabIndex = 40;
            this.delButton.Text = "X";
            this.delButton.UseVisualStyleBackColor = false;
            this.delButton.Click += new System.EventHandler( this.delButton_Click );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 3, 14 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 57, 13 );
            this.label2.TabIndex = 39;
            this.label2.Text = "Settings:";
            // 
            // newSettingsButton
            // 
            this.newSettingsButton.BackColor = System.Drawing.SystemColors.Control;
            this.newSettingsButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.newSettingsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.newSettingsButton.Location = new System.Drawing.Point( 116, 9 );
            this.newSettingsButton.Name = "newSettingsButton";
            this.newSettingsButton.Size = new System.Drawing.Size( 71, 23 );
            this.newSettingsButton.TabIndex = 38;
            this.newSettingsButton.Text = "Save As...";
            this.newSettingsButton.UseVisualStyleBackColor = false;
            this.newSettingsButton.Click += new System.EventHandler( this.saveAsButton_Click );
            // 
            // settingsComboBox
            // 
            this.settingsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsComboBox.FormattingEnabled = true;
            this.settingsComboBox.Location = new System.Drawing.Point( 5, 36 );
            this.settingsComboBox.Name = "settingsComboBox";
            this.settingsComboBox.Size = new System.Drawing.Size( 212, 21 );
            this.settingsComboBox.TabIndex = 37;
            this.settingsComboBox.SelectedIndexChanged += new System.EventHandler( this.settingsComboBox_SelectedIndexChanged );
            // 
            // simBox
            // 
            this.simBox.CheckOnClick = true;
            this.simBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.simBox.Location = new System.Drawing.Point( 3, 318 );
            this.simBox.Name = "simBox";
            this.simBox.Size = new System.Drawing.Size( 218, 274 );
            this.simBox.TabIndex = 29;
            // 
            // selectAllCheckBox
            // 
            this.selectAllCheckBox.Location = new System.Drawing.Point( 6, 291 );
            this.selectAllCheckBox.Name = "selectAllCheckBox";
            this.selectAllCheckBox.Size = new System.Drawing.Size( 106, 29 );
            this.selectAllCheckBox.TabIndex = 36;
            this.selectAllCheckBox.Text = "Select All";
            this.selectAllCheckBox.UseVisualStyleBackColor = true;
            this.selectAllCheckBox.CheckedChanged += new System.EventHandler( this.selectAllCheckBox_CheckedChanged );
            // 
            // AdvStatCheck
            // 
            this.AdvStatCheck.AutoSize = true;
            this.AdvStatCheck.Location = new System.Drawing.Point( 6, 245 );
            this.AdvStatCheck.Name = "AdvStatCheck";
            this.AdvStatCheck.Size = new System.Drawing.Size( 120, 17 );
            this.AdvStatCheck.TabIndex = 35;
            this.AdvStatCheck.Text = "Advanced Statistics";
            this.AdvStatCheck.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point( 84, 214 );
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size( 75, 24 );
            this.refreshButton.TabIndex = 32;
            this.refreshButton.Text = "Refresh List";
            this.refreshButton.Click += new System.EventHandler( this.refreshButton_Click );
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label5.Location = new System.Drawing.Point( 4, 219 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 72, 21 );
            this.label5.TabIndex = 14;
            this.label5.Text = "Simulations";
            // 
            // timingBox
            // 
            this.timingBox.Controls.Add( this.startEndDate1 );
            this.timingBox.Controls.Add( this.timeSpan );
            this.timingBox.Controls.Add( this.dailyAverageBox );
            this.timingBox.Location = new System.Drawing.Point( 16, 88 );
            this.timingBox.Name = "timingBox";
            this.timingBox.Size = new System.Drawing.Size( 176, 117 );
            this.timingBox.TabIndex = 31;
            this.timingBox.TabStop = false;
            this.timingBox.Text = "Time Span";
            // 
            // startEndDate1
            // 
            this.startEndDate1.End = new System.DateTime( 2006, 7, 11, 23, 18, 11, 62 );
            this.startEndDate1.Location = new System.Drawing.Point( 16, 64 );
            this.startEndDate1.Name = "startEndDate1";
            this.startEndDate1.Size = new System.Drawing.Size( 160, 48 );
            this.startEndDate1.Start = new System.DateTime( 2006, 7, 11, 23, 18, 11, 62 );
            this.startEndDate1.TabIndex = 26;
            // 
            // timeSpan
            // 
            this.timeSpan.Items.AddRange( new object[] {
            "Day",
            "Week",
            "Month",
            "Year"} );
            this.timeSpan.Location = new System.Drawing.Point( 64, 24 );
            this.timeSpan.Name = "timeSpan";
            this.timeSpan.Size = new System.Drawing.Size( 96, 21 );
            this.timeSpan.TabIndex = 25;
            this.timeSpan.Text = "Month";
            // 
            // dailyAverageBox
            // 
            this.dailyAverageBox.Location = new System.Drawing.Point( 16, 24 );
            this.dailyAverageBox.Maximum = new decimal( new int[] {
            52,
            0,
            0,
            0} );
            this.dailyAverageBox.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.dailyAverageBox.Name = "dailyAverageBox";
            this.dailyAverageBox.Size = new System.Drawing.Size( 40, 20 );
            this.dailyAverageBox.TabIndex = 23;
            this.dailyAverageBox.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // summaryButton
            // 
            this.summaryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.summaryButton.Location = new System.Drawing.Point( 386, 99 );
            this.summaryButton.Name = "summaryButton";
            this.summaryButton.Size = new System.Drawing.Size( 145, 23 );
            this.summaryButton.TabIndex = 28;
            this.summaryButton.Text = "Summary...";
            this.summaryButton.Click += new System.EventHandler( this.summaryButton_Click );
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add( this.segmentPicker1 );
            this.groupBox3.Controls.Add( this.channelPicker1 );
            this.groupBox3.Controls.Add( this.productTree );
            this.groupBox3.Controls.Add( this.regressionButton );
            this.groupBox3.Controls.Add( this.groupBox1 );
            this.groupBox3.Controls.Add( this.graphButton );
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size( 567, 537 );
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scope";
            // 
            // segmentPicker1
            // 
            this.segmentPicker1.Location = new System.Drawing.Point( 288, 16 );
            this.segmentPicker1.Name = "segmentPicker1";
            this.segmentPicker1.SegmentID = -1;
            this.segmentPicker1.Size = new System.Drawing.Size( 241, 24 );
            this.segmentPicker1.TabIndex = 24;
            // 
            // channelPicker1
            // 
            this.channelPicker1.ChannelID = -1;
            this.channelPicker1.Location = new System.Drawing.Point( 288, 58 );
            this.channelPicker1.Name = "channelPicker1";
            this.channelPicker1.Size = new System.Drawing.Size( 241, 24 );
            this.channelPicker1.TabIndex = 23;
            // 
            // productTree
            // 
            this.productTree.CheckBoxes = true;
            this.productTree.ContextMenu = this.ProductTreeContextMenu;
            this.productTree.Location = new System.Drawing.Point( 16, 24 );
            this.productTree.Name = "productTree";
            this.productTree.Size = new System.Drawing.Size( 232, 200 );
            this.productTree.TabIndex = 22;
            // 
            // ProductTreeContextMenu
            // 
            this.ProductTreeContextMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.SelectByType} );
            // 
            // SelectByType
            // 
            this.SelectByType.Index = 0;
            this.SelectByType.Text = "Select By Type";
            // 
            // regressionButton
            // 
            this.regressionButton.Location = new System.Drawing.Point( 152, 240 );
            this.regressionButton.Name = "regressionButton";
            this.regressionButton.Size = new System.Drawing.Size( 112, 24 );
            this.regressionButton.TabIndex = 21;
            this.regressionButton.Text = "Create Regression";
            this.regressionButton.Click += new System.EventHandler( this.regressionButton_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.oneChart );
            this.groupBox1.Controls.Add( this.sumProducts );
            this.groupBox1.Controls.Add( this.sumSegments );
            this.groupBox1.Controls.Add( this.segmentPerChart );
            this.groupBox1.Controls.Add( this.productPerChart );
            this.groupBox1.Location = new System.Drawing.Point( 288, 96 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 208, 128 );
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph Options";
            // 
            // oneChart
            // 
            this.oneChart.Location = new System.Drawing.Point( 16, 96 );
            this.oneChart.Name = "oneChart";
            this.oneChart.Size = new System.Drawing.Size( 152, 24 );
            this.oneChart.TabIndex = 21;
            this.oneChart.Text = "All Products on one chart";
            this.oneChart.CheckedChanged += new System.EventHandler( this.oneChart_CheckedChanged );
            // 
            // sumProducts
            // 
            this.sumProducts.Location = new System.Drawing.Point( 16, 72 );
            this.sumProducts.Name = "sumProducts";
            this.sumProducts.Size = new System.Drawing.Size( 128, 16 );
            this.sumProducts.TabIndex = 20;
            this.sumProducts.Text = "sum over products";
            this.sumProducts.Visible = false;
            // 
            // sumSegments
            // 
            this.sumSegments.Checked = true;
            this.sumSegments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sumSegments.Location = new System.Drawing.Point( 16, 32 );
            this.sumSegments.Name = "sumSegments";
            this.sumSegments.Size = new System.Drawing.Size( 128, 16 );
            this.sumSegments.TabIndex = 19;
            this.sumSegments.Text = "sum over segments";
            // 
            // segmentPerChart
            // 
            this.segmentPerChart.Location = new System.Drawing.Point( 16, 56 );
            this.segmentPerChart.Name = "segmentPerChart";
            this.segmentPerChart.Size = new System.Drawing.Size( 144, 16 );
            this.segmentPerChart.TabIndex = 18;
            this.segmentPerChart.Text = "One segment per chart";
            this.segmentPerChart.CheckedChanged += new System.EventHandler( this.segmentPerChart_CheckedChanged );
            // 
            // productPerChart
            // 
            this.productPerChart.Checked = true;
            this.productPerChart.Location = new System.Drawing.Point( 16, 16 );
            this.productPerChart.Name = "productPerChart";
            this.productPerChart.Size = new System.Drawing.Size( 144, 16 );
            this.productPerChart.TabIndex = 17;
            this.productPerChart.TabStop = true;
            this.productPerChart.Text = "One product per chart";
            this.productPerChart.CheckedChanged += new System.EventHandler( this.productPerChart_CheckedChanged );
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add( this.chartPage );
            this.tabControl.Controls.Add( this.summaryPage );
            this.tabControl.Controls.Add( this.paramPage );
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point( 224, 0 );
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size( 575, 595 );
            this.tabControl.TabIndex = 31;
            this.tabControl.SelectedIndexChanged += new System.EventHandler( this.tabControl_SelectedIndexChanged );
            // 
            // chartPage
            // 
            this.chartPage.Controls.Add( this.graphBox );
            this.chartPage.Controls.Add( this.groupBox3 );
            this.chartPage.Controls.Add( this.descriptionBox );
            this.chartPage.Location = new System.Drawing.Point( 4, 22 );
            this.chartPage.Name = "chartPage";
            this.chartPage.Size = new System.Drawing.Size( 567, 569 );
            this.chartPage.TabIndex = 0;
            this.chartPage.Text = "Time Series Charts";
            // 
            // summaryPage
            // 
            this.summaryPage.Controls.Add( this.summaryReportGroupBox );
            this.summaryPage.Controls.Add( this.writeCsvButton );
            this.summaryPage.Controls.Add( this.calScopeBox );
            this.summaryPage.Controls.Add( this.simScopeBox );
            this.summaryPage.Controls.Add( this.csvDump );
            this.summaryPage.Controls.Add( this.recalcButton );
            this.summaryPage.Controls.Add( this.saveSummarySettingsButton );
            this.summaryPage.Controls.Add( this.summaryButton );
            this.summaryPage.Location = new System.Drawing.Point( 4, 22 );
            this.summaryPage.Name = "summaryPage";
            this.summaryPage.Size = new System.Drawing.Size( 567, 569 );
            this.summaryPage.TabIndex = 1;
            this.summaryPage.Text = "Summary Tables";
            // 
            // summaryReportGroupBox
            // 
            this.summaryReportGroupBox.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))) );
            this.summaryReportGroupBox.Controls.Add( this.bothLevelsRadioButton );
            this.summaryReportGroupBox.Controls.Add( this.brandLevelRadioButton );
            this.summaryReportGroupBox.Controls.Add( this.productLevelRadioButton );
            this.summaryReportGroupBox.Controls.Add( this.trialReportButton );
            this.summaryReportGroupBox.Controls.Add( this.fwReportButton );
            this.summaryReportGroupBox.Controls.Add( this.calMasterButton );
            this.summaryReportGroupBox.Location = new System.Drawing.Point( 67, 378 );
            this.summaryReportGroupBox.Name = "summaryReportGroupBox";
            this.summaryReportGroupBox.Size = new System.Drawing.Size( 407, 115 );
            this.summaryReportGroupBox.TabIndex = 43;
            this.summaryReportGroupBox.TabStop = false;
            this.summaryReportGroupBox.Text = "Summary Reports";
            // 
            // bothLevelsRadioButton
            // 
            this.bothLevelsRadioButton.AutoSize = true;
            this.bothLevelsRadioButton.Location = new System.Drawing.Point( 220, 75 );
            this.bothLevelsRadioButton.Name = "bothLevelsRadioButton";
            this.bothLevelsRadioButton.Size = new System.Drawing.Size( 148, 17 );
            this.bothLevelsRadioButton.TabIndex = 7;
            this.bothLevelsRadioButton.Text = "Brand and Product Levels";
            this.bothLevelsRadioButton.UseVisualStyleBackColor = true;
            // 
            // brandLevelRadioButton
            // 
            this.brandLevelRadioButton.AutoSize = true;
            this.brandLevelRadioButton.Location = new System.Drawing.Point( 220, 53 );
            this.brandLevelRadioButton.Name = "brandLevelRadioButton";
            this.brandLevelRadioButton.Size = new System.Drawing.Size( 82, 17 );
            this.brandLevelRadioButton.TabIndex = 6;
            this.brandLevelRadioButton.Text = "Brand Level";
            this.brandLevelRadioButton.UseVisualStyleBackColor = true;
            // 
            // productLevelRadioButton
            // 
            this.productLevelRadioButton.AutoSize = true;
            this.productLevelRadioButton.Checked = true;
            this.productLevelRadioButton.Location = new System.Drawing.Point( 220, 30 );
            this.productLevelRadioButton.Name = "productLevelRadioButton";
            this.productLevelRadioButton.Size = new System.Drawing.Size( 91, 17 );
            this.productLevelRadioButton.TabIndex = 5;
            this.productLevelRadioButton.TabStop = true;
            this.productLevelRadioButton.Text = "Product Level";
            this.productLevelRadioButton.UseVisualStyleBackColor = true;
            // 
            // trialReportButton
            // 
            this.trialReportButton.Location = new System.Drawing.Point( 37, 79 );
            this.trialReportButton.Name = "trialReportButton";
            this.trialReportButton.Size = new System.Drawing.Size( 127, 23 );
            this.trialReportButton.TabIndex = 4;
            this.trialReportButton.Text = "Trial Report";
            this.trialReportButton.UseVisualStyleBackColor = true;
            this.trialReportButton.Click += new System.EventHandler( this.trialReportButton_Click );
            // 
            // fwReportButton
            // 
            this.fwReportButton.Location = new System.Drawing.Point( 37, 50 );
            this.fwReportButton.Name = "fwReportButton";
            this.fwReportButton.Size = new System.Drawing.Size( 127, 23 );
            this.fwReportButton.TabIndex = 3;
            this.fwReportButton.Text = "F-W Report";
            this.fwReportButton.UseVisualStyleBackColor = true;
            this.fwReportButton.Click += new System.EventHandler( this.fwReportButton_Click );
            // 
            // calMasterButton
            // 
            this.calMasterButton.Location = new System.Drawing.Point( 37, 21 );
            this.calMasterButton.Name = "calMasterButton";
            this.calMasterButton.Size = new System.Drawing.Size( 127, 23 );
            this.calMasterButton.TabIndex = 2;
            this.calMasterButton.Text = "Calibration Master";
            this.calMasterButton.UseVisualStyleBackColor = true;
            this.calMasterButton.Click += new System.EventHandler( this.calMasterButton_Click );
            // 
            // writeCsvButton
            // 
            this.writeCsvButton.Location = new System.Drawing.Point( 386, 208 );
            this.writeCsvButton.Name = "writeCsvButton";
            this.writeCsvButton.Size = new System.Drawing.Size( 145, 24 );
            this.writeCsvButton.TabIndex = 42;
            this.writeCsvButton.Text = "Write Consumer Panel";
            this.writeCsvButton.Click += new System.EventHandler( this.writeCsvButton_Click );
            // 
            // calScopeBox
            // 
            this.calScopeBox.Controls.Add( this.calMetricBox );
            this.calScopeBox.Location = new System.Drawing.Point( 40, 208 );
            this.calScopeBox.Name = "calScopeBox";
            this.calScopeBox.Size = new System.Drawing.Size( 296, 136 );
            this.calScopeBox.TabIndex = 41;
            this.calScopeBox.TabStop = false;
            this.calScopeBox.Text = "Calibration Summary Scope";
            // 
            // calMetricBox
            // 
            this.calMetricBox.CheckOnClick = true;
            this.calMetricBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calMetricBox.Location = new System.Drawing.Point( 3, 16 );
            this.calMetricBox.Name = "calMetricBox";
            this.calMetricBox.Size = new System.Drawing.Size( 290, 109 );
            this.calMetricBox.TabIndex = 1;
            // 
            // simScopeBox
            // 
            this.simScopeBox.Controls.Add( this.simMetricBox );
            this.simScopeBox.Location = new System.Drawing.Point( 40, 32 );
            this.simScopeBox.Name = "simScopeBox";
            this.simScopeBox.Size = new System.Drawing.Size( 296, 152 );
            this.simScopeBox.TabIndex = 37;
            this.simScopeBox.TabStop = false;
            this.simScopeBox.Text = "Simulation Summary Scope";
            // 
            // simMetricBox
            // 
            this.simMetricBox.CheckOnClick = true;
            this.simMetricBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simMetricBox.Location = new System.Drawing.Point( 3, 16 );
            this.simMetricBox.Name = "simMetricBox";
            this.simMetricBox.Size = new System.Drawing.Size( 290, 124 );
            this.simMetricBox.TabIndex = 0;
            // 
            // csvDump
            // 
            this.csvDump.Location = new System.Drawing.Point( 386, 149 );
            this.csvDump.Name = "csvDump";
            this.csvDump.Size = new System.Drawing.Size( 145, 23 );
            this.csvDump.TabIndex = 35;
            this.csvDump.Text = "Write Summary Data";
            this.csvDump.Click += new System.EventHandler( this.csvDump_Click );
            // 
            // recalcButton
            // 
            this.recalcButton.Location = new System.Drawing.Point( 386, 310 );
            this.recalcButton.Name = "recalcButton";
            this.recalcButton.Size = new System.Drawing.Size( 145, 23 );
            this.recalcButton.TabIndex = 34;
            this.recalcButton.Text = "Recompute...";
            this.recalcButton.Click += new System.EventHandler( this.recalcButton_Click );
            // 
            // saveSummarySettingsButton
            // 
            this.saveSummarySettingsButton.Location = new System.Drawing.Point( 386, 48 );
            this.saveSummarySettingsButton.Name = "saveSummarySettingsButton";
            this.saveSummarySettingsButton.Size = new System.Drawing.Size( 145, 24 );
            this.saveSummarySettingsButton.TabIndex = 31;
            this.saveSummarySettingsButton.Text = "Save  Settngs";
            this.saveSummarySettingsButton.Click += new System.EventHandler( this.saveSummarySettingsButton_Click );
            // 
            // paramPage
            // 
            this.paramPage.Controls.Add( this.variableControl1 );
            this.paramPage.Location = new System.Drawing.Point( 4, 22 );
            this.paramPage.Name = "paramPage";
            this.paramPage.Size = new System.Drawing.Size( 567, 569 );
            this.paramPage.TabIndex = 2;
            this.paramPage.Text = "Variables";
            // 
            // variableControl1
            // 
            this.variableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableControl1.Location = new System.Drawing.Point( 0, 0 );
            this.variableControl1.Name = "variableControl1";
            this.variableControl1.Size = new System.Drawing.Size( 567, 569 );
            this.variableControl1.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // ResultsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 799, 595 );
            this.Controls.Add( this.tabControl );
            this.Controls.Add( this.groupBox2 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 752, 592 );
            this.Name = "ResultsForm";
            this.Text = " +96";
            this.Closing += new System.ComponentModel.CancelEventHandler( this.ResultsForm_Closing );
            this.Load += new System.EventHandler( this.ResultsForm_Load );
            this.graphBox.ResumeLayout( false );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.timingBox.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.dailyAverageBox)).EndInit();
            this.groupBox3.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.tabControl.ResumeLayout( false );
            this.chartPage.ResumeLayout( false );
            this.chartPage.PerformLayout();
            this.summaryPage.ResumeLayout( false );
            this.summaryReportGroupBox.ResumeLayout( false );
            this.summaryReportGroupBox.PerformLayout();
            this.calScopeBox.ResumeLayout( false );
            this.simScopeBox.ResumeLayout( false );
            this.paramPage.ResumeLayout( false );
            this.ResumeLayout( false );

		}
		#endregion

		
        //private void setUpExternalData()
        //{
        //    // look for user types
        //    DataRow[] extTypeRows = ModelDb.external_data_type.Select("", "",DataViewRowState.CurrentRows);

        //    foreach (DataRow extType in extTypeRows)
        //    {
        //        string query = "token = '" + extType["type"].ToString() + "'";

        //        DataRow[] extRows = dataCol.Select(query, "", DataViewRowState.CurrentRows);

        //        if (extRows.Length == 0)
        //        {
        //            // add this to list
        //            DataRow extRow = dataCol.NewRow();

        //            //need to create eval data as well
        //            ExternalEvalData evalData = new ExternalEvalData();

        //            evalData.token = extType["type"].ToString();
        //            evalData.Id = (int) extType["id"];

        //            extRow["evalData"] = evalData;

        //            extRow["userName"] = extType["descr"];
        //            extRow["token"] = extType["type"];

        //            dataCol.Rows.Add(extRow);
        //        }
        //    }

        //    dataCol.AcceptChanges();
        //}

		private void graphButton_Click(object sender, System.EventArgs e)
		{
			myForest = productTree.getProductForest();
			if(myForest == null)
			{
				MessageBox.Show("Error: There are no valid products selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			//Check to make sure data columns and sims are selected
			bool noSimsChecked = true;
			bool noColumnsSelected = true;
			for(int i = 0; i < simBox.Items.Count; ++i)
			{
				if(simBox.GetItemChecked(i))
				{
					noSimsChecked = false;
					break;
				}
			}
			if(dataListBox.Items.Count > 0)
			{
				noColumnsSelected = false;
			}
			if(noSimsChecked && noColumnsSelected)
			{
				MessageBox.Show("Error: There are no sims or data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			if(noSimsChecked)
			{
				MessageBox.Show("Error: There are no sims selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			if(noColumnsSelected)
			{
				MessageBox.Show("Error: There are no data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			
			MultiGrapher mgraph = new MultiGrapher( GetCurrentResultsSettings() );
            mgraph.NewNamedSettingsAdded += new MultiGrapher.HandleUpdatedNamedSettings( mgraph_NewNamedSettingsAdded );
            mgraph.NamedSettingsSaved += new MultiGrapher.HandleUpdatedNamedSettings( mgraph_NamedSettingsSaved );

			int days = 1;

			switch( this.timeSpan.SelectedIndex)
			{
				case 0:
					days = (int) dailyAverageBox.Value;
					break;
				case 1:
					days = 7 * ((int) dailyAverageBox.Value);
					break;
				case 2:
					days = 30 * ((int) dailyAverageBox.Value);
					break;
				case 3:
					days = 365 * ((int) dailyAverageBox.Value);
					break;
			}

			DateTime actualStart = this.startEndDate1.Start;

			mgraph.Start = actualStart;
			mgraph.End = this.startEndDate1.End;
			mgraph.Text = "Plot " + theDb.Model.model_name;

			mgraph.SuspendLayout();

			
			createPlots(mgraph, segmentPicker1.SegmentID, channelPicker1.ChannelID, days, SummaryReportType.None);

			mgraph.PlotsChanged();

			mgraph.ResumeLayout(false);

			// mgraph.Initialize();

			mgraph.Show();
			
		}

        /// <summary>
        /// Create a non-displayed MultiGrapher object that will be used to save CSV 
        /// </summary>
        /// <param name="scenarioList"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="days"></param>
        /// <returns></returns>
		public MultiGrapher GraphNoShow( DateTime start, DateTime end, SummaryReportType summaryReportType, bool getByBrand )
		{

            // save the button state
            bool ppcChecked = productPerChart.Checked;
            bool spcChecked = segmentPerChart.Checked;
            bool ocChecked = oneChart.Checked;
            bool ssChecked = sumSegments.Checked;
            bool spChecked = sumProducts.Checked;

            // we always want this setup
            productPerChart.Checked = true;
            sumSegments.Checked = true;

            // use the hidden product tree
            if( getByBrand == false ) {
                this.hiddenProductTree.SelectByType( GetProductTypeString() );                 
            }
            else {
                this.hiddenProductTree.SelectByType( GetBrandTypeString() );
            }

            ProductForest perProductOrBrandForest = this.hiddenProductTree.getProductForest();

            // generate the panel data values
            MultiGrapher multiGrapher = GraphNoShow( start, end, perProductOrBrandForest, summaryReportType );

            // restore button state
            productPerChart.Checked = ppcChecked;
            segmentPerChart.Checked = spcChecked;
            oneChart.Checked = ocChecked;
            sumSegments.Checked = ssChecked;
            sumProducts.Checked = spChecked;

            return multiGrapher;
		}

        public MultiGrapher GraphNoShow( DateTime start, DateTime end, ProductForest forestToUse, SummaryReportType summaryReportType )
		{
            TimeSpan span = (startEndDate1.End - startEndDate1.Start);
            int days = (int)span.TotalDays;

			myForest = forestToUse;
			if(myForest == null)
			{
				MessageBox.Show("Error: There are no valid products selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}

			this.startEndDate1.Start = start;
			this.startEndDate1.End = end;

			//Check to make sure data columns and sims are selected
			bool noSimsChecked = true;
            for( int i = 0; i < simBox.Items.Count; ++i ) {
                if( simBox.GetItemChecked( i ) ) {
                    noSimsChecked = false;
                    break;
                }
            }
			if(noSimsChecked)
			{
				MessageBox.Show("Error: There are no sims selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}

            MultiGrapher mgraph = new MultiGrapher( GetCurrentResultsSettings() );
            mgraph.NewNamedSettingsAdded += new MultiGrapher.HandleUpdatedNamedSettings( mgraph_NewNamedSettingsAdded );
            mgraph.NamedSettingsSaved += new MultiGrapher.HandleUpdatedNamedSettings( mgraph_NamedSettingsSaved );

			DateTime actualStart = this.startEndDate1.Start;

			mgraph.Start = actualStart;
			mgraph.End = this.startEndDate1.End;
			mgraph.Text = "Plot " + theDb.Model.model_name;

			mgraph.SuspendLayout();

            createPlots( mgraph, segmentPicker1.SegmentID, channelPicker1.ChannelID, days, summaryReportType );
		
			mgraph.PlotsChanged();

			mgraph.ResumeLayout(false);

			// mgraph.Initialize();

			return mgraph;
		}

		public MultiGrapher AddPlotToGraph(MultiGrapher mgraph, ArrayList scenarioList, DateTime start, DateTime end, int days)
		{
			myForest = productTree.getProductForest();
			if(myForest == null)
			{
				MessageBox.Show("Error: There are no valid products selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}
			
			if(scenarioList != null)
			{
				foreach(MrktSimDb.MrktSimDBSchema.simulationRow simulation in scenarioList)
				{
					// get simulation id and match to simTable to get object
					for(int index = 0; index < simTable.Rows.Count; ++index)
					{
						if ((int) simTable.Rows[index]["sim_id"] == simulation.id)
						{
							simBox.SetItemChecked(index, true);
						}
					}
				}
			}

			this.startEndDate1.Start = start;

			this.startEndDate1.End = end;

			//Check to make sure data columns and sims are selected
			bool noSimsChecked = true;
			bool noColumnsSelected = true;
			for(int i = 0; i < simBox.Items.Count; ++i)
			{
				if(simBox.GetItemChecked(i))
				{
					noSimsChecked = false;
					break;
				}
			}
			if(dataListBox.Items.Count > 0)
			{
				noColumnsSelected = false;
			}
			if(noSimsChecked && noColumnsSelected)
			{
				MessageBox.Show("Error: There are no sims or data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}
			if(noSimsChecked)
			{
				MessageBox.Show("Error: There are no sims selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}
			if(noColumnsSelected)
			{
				MessageBox.Show("Error: There are no data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}

			DateTime actualStart = this.startEndDate1.Start;

			mgraph.Start = actualStart;
			mgraph.End = this.startEndDate1.End;

			mgraph.SuspendLayout();

			
			createPlots(mgraph, segmentPicker1.SegmentID, channelPicker1.ChannelID, days, SummaryReportType.None );

		
			mgraph.PlotsChanged();

			mgraph.ResumeLayout(false);

			// mgraph.Initialize();

			return mgraph;
		}

        private bool doingConsumerPanel = false;
        private bool doingCalibrationMaster = false;
        private bool doingFWReport= false;
        private bool doingTrialReport = false;

        private void createPlots( MultiGrapher mgraph, int segment_id, int channel_id, int days, SummaryReportType summaryReportType )
		{
			double numToProcess = 1;
			double currProcessed = 0;

            this.doingConsumerPanel = false;
            this.doingCalibrationMaster = false;
            this.doingFWReport = false;
            this.doingTrialReport = false;

            if( summaryReportType == SummaryReportType.ConsumerPanel ) {
                this.doingConsumerPanel = true;
            }
            else if( summaryReportType == SummaryReportType.CalibrationMaster ) {
                this.doingCalibrationMaster = true;
            }
            else if( summaryReportType == SummaryReportType.FWReport ) {
                this.doingFWReport = true;
            }
            else if( summaryReportType == SummaryReportType.TrialReport ) {
                this.doingTrialReport = true;
            }

			using(ProcessStatus process = new ProcessStatus())
			{
				process.ProcessType = "Computing Results";
				

				// progress.Location = this.Location;
				process.Show();

				process.Progress("Plotting", 0);

				if (this.productPerChart.Checked )
				{
					
					numToProcess = myForest.Trees.Count;

					foreach(ArrayList prods in myForest.Trees)
					{
						MrktSimDBSchema.productRow prod = (MrktSimDBSchema.productRow) prods[0];

						process.Progress("Processing: " + prod.product_name, currProcessed++/numToProcess);
						setupPlot(mgraph, prods, segment_id, channel_id, days);
					}

					return;
				}
				else if (this.segmentPerChart.Checked)
				{
					// create a chart for each segment
					if (segment_id == ModelDb.AllID)
					{
						string filter = "segment_id <> " + ModelDb.AllID;

						DataRow[] rows = theDb.Data.segment.Select(filter,"", DataViewRowState.CurrentRows);
						
						numToProcess = rows.Length;
						foreach(DataRow row in rows)
						{
							process.Progress("Processing: " + row["segment_name"].ToString(), currProcessed++/numToProcess);

							foreach(ArrayList prods in myForest.Trees)
							{
								MrktSimDBSchema.productRow prod = (MrktSimDBSchema.productRow) prods[0];

								setupPlot(mgraph, prods, (int) row["segment_id"], channel_id, days);
							}
						}
						return;
					}
					else
					{
						setupPlotAllProdsOnOnePlot(mgraph, myForest.Trees, segment_id, channel_id, days, process);
					}
				}
				else
				{
					setupPlotAllProdsOnOnePlot(mgraph, myForest.Trees, segment_id, channel_id, days, process);
				}
			}
		}

		private void setupPlotAllProdsOnOnePlot(MultiGrapher mgraph, ArrayList forest, 
			int segment_id, int channel_id, int days, ProcessStatus process)
		{
			// we have a specific product or brand or segment
			Plot plot = mgraph.NewPlot();
			colorIndex = 0;

			// figure out name of chart
			plot.Text = "";

			 
			if (simBox.CheckedItems.Count > 0 )
			{
				plot.Text = ((DataRowView) simBox.CheckedItems[0])["sim"].ToString();
			}

			if (simBox.CheckedItems.Count > 1 )
			{
				plot.Text += " v " +  ((DataRowView) simBox.CheckedItems[1])["sim"].ToString();
			}


			if (this.segmentPerChart.Checked)
			{
				MrktSimDBSchema.segmentRow segment =  theDb.Data.segment.FindBysegment_id(segment_id);
				plot.Text += segment.segment_name + " ";
			}

			plot.Title = plot.Text;

			int numToProcess = forest.Count;
			int currProcessed = 0;

			foreach(ArrayList prods in forest)
			{
				MrktSimDBSchema.productRow prod = (MrktSimDBSchema.productRow) prods[0];

				process.Progress("Processing: " + prod.product_name, currProcessed++/numToProcess);

				createCurves(plot, prods, segment_id, channel_id, days);
			}
		}
		
		private void setupPlot(MultiGrapher mgraph, ArrayList prods, int segment_id, int channel_id, int days)
		{
			MrktSimDBSchema.productRow product =  (MrktSimDBSchema.productRow) prods[0];
			// we have a specific product or brand or segment
			Plot plot = mgraph.NewPlot();
			colorIndex = 0;

			// figure out name of chart
			plot.Text = "";

			if (simBox.CheckedItems.Count == 1)
			{
				plot.Text = "(" +  ((DataRowView) simBox.CheckedItems[0])["sim"] + ") ";
			}

			if (this.productPerChart.Checked)
			{
				if(product.product_id == ModelDb.AllID)
				{
					plot.Text += "Total";
				}
				else
				{
					plot.Text += product.product_name;
				}
			}
			else if (this.segmentPerChart.Checked)
			{
				MrktSimDBSchema.segmentRow segment =  theDb.Data.segment.FindBysegment_id(segment_id);
				plot.Text += segment.segment_name + " ";
			}

			plot.Title = plot.Text;

			createCurves(plot, prods, segment_id, channel_id, days);
		}

		private void createCurves(Plot plot, ArrayList prods, int segment_id, int channel_id, int days)
		{
			MrktSimDBSchema.productRow product =  (MrktSimDBSchema.productRow) prods[0];

			// create a curve for each segment
			if (!this.sumSegments.Checked && segment_id == ModelDb.AllID)
			{
				string filter = "segment_id <> " + ModelDb.AllID;

				DataRow[] rows = theDb.Data.segment.Select(filter,"", DataViewRowState.CurrentRows);
				foreach(DataRow row in rows)
				{
					setupCurves(plot, prods, (int) row["segment_id"], channel_id, days);
				}
					
				return;
			}

//			if( product.product_id == ModelDb.AllID )
//			{
//				ArrayList parents = myForest.getParents();
//				if (((MrktSimDBSchema.productRow)parents[0]).product_id == ModelDb.AllID)
//				{
//					setupCurves(plot, product_id, segment_id, channel_id, days);
//				}
//				else
//				{
//					foreach(MrktSimDBSchema.productRow row in parents)
//					{
//						setupCurves(plot, row.product_id, segment_id, channel_id, days);
//					}
//				}
//				return;
//			}

			setupCurves(plot, prods, segment_id, channel_id, days);

		}

        private void setupCurves(Plot plot, ArrayList prods, int segment_id, int channel_id, int days)
        {
            ArrayList evalDataItems = new ArrayList();
            if( this.doingConsumerPanel == false && this.doingCalibrationMaster == false && this.doingFWReport == false && this.doingTrialReport == false ) {
                foreach( object dlbObj in dataListBox.Items ) {
                    evalDataItems.Add( dlbObj );
                }
            }
            else {
                // we are generating a summary report -- set the metrics accordingly (we are using values from the final (only) data point of each curve)
                if( this.doingConsumerPanel == true ) {
                    foreach( EvalData eval in SqlEval.GetEvalData( DataType.PanelData ) ) {
                        evalDataItems.Add( eval );
                    }
                }
                else if( this.doingCalibrationMaster == true ) {
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is tau -- dummy val; will be replaced with real value later
                    AddMetricToList( "sku_share", evalDataItems );                                 // sim share
                    AddMetricToList( "real_share", evalDataItems );                                // real share
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is MAPE-- dummy val; will be replaced with real value later
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is sales rate -- dummy val; will be replaced with real value later
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is real-sales rate -- dummy val; will be replaced with real value later
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is sales rate R2 -- dummy val; will be replaced with real value later
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is real-sales rate R2 -- dummy val; will be replaced with real value later
                }
                else if( this.doingFWReport == true ) {
                    AddMetricToList( "trial_rate", evalDataItems );                                 // trial rate
                    AddMetricToList( "repeaters_per_trier", evalDataItems );                   // repeats rate
                    AddMetricToList( "repeats_per_repeater", evalDataItems );                 // repeats per repater
                    AddMetricToList( "purchase_frequency", evalDataItems );                   // purchase frequency
                    AddMetricToList( "transaction_size_units", evalDataItems );                // transaction size
                    AddMetricToList( "num_sku_bought", evalDataItems );                       // sim unit sales
                }
                else if( this.doingTrialReport == true ) {
                    AddMetricToList( "trial_rate", evalDataItems );                                 // trial rate
                    AddMetricToList( "percent_preuse_distribution_sku", evalDataItems );  // distribution
                    AddMetricToList( "percent_aware_sku_cum", evalDataItems );            // awareness
                    AddMetricToList( "GRPs_SKU_tick", evalDataItems );                         // GRPs
                    AddMetricToList( "percent_on_display_sku", evalDataItems );              // % display
                    AddMetricToList( "num_units_bought_on_coupon", evalDataItems );     // coupon purchases
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is base price (abs)-- dummy val; will be replaced with real value later
                    AddMetricToList( "trial_rate", evalDataItems );                                 // really is avg price (abs)-- dummy val; will be replaced with real value later
                }
            }

            foreach( object anObj in evalDataItems )
            {
                EvalData evalData = (EvalData)anObj;

                SqlEval eval = SqlEval.CreateEvaluator(evalData);

                if (eval != null)
                {
                    eval.Connection = theDb.Connection;

                    if (evalData.type == DataType.externalData)
                    {
                        eval.Run = theDb.ModelID;
                        string label = "(real data)";

                        
                        plot.Data = plotData(eval, label, prods, segment_id, channel_id, days);
                        plot.DataChanged();

                        colorIndex++;

                        if (colorIndex >= colorOpts.Length)
                            colorIndex = 0;
                    }
                    else
                    {
                        //  mulitple scenarios can be added
                        foreach (Object obj in simBox.CheckedItems)
                        {
                            string label;
                            ArrayList curves = new ArrayList();
                            ArrayList run_ids = (ArrayList)((DataRowView)obj).Row["run_ids"];

                            foreach (int run_id in run_ids)
                            {
                                if (simBox.CheckedItems.Count == 1)
                                    label = "";
                                else
                                    label = "(" + ((DataRowView)obj).Row["sim"] + ") ";

                                eval.Run = run_id;

                                curves.Add(plotData(eval, label, prods, segment_id, channel_id, days));
                            }

                            if (curves.Count > 1)
                            {
                                curves = calcStats(curves);
                            }

                            if (curves[0] != null)
                            {
                                if (AdvStatCheck.Checked)
                                {
                                    foreach (DataCurve curve in curves)
                                    {
                                        if (curve != null)
                                        {
                                            plot.Data = curve;
                                        }
                                    }
                                }
                                else
                                {
                                    plot.Data = (DataCurve)curves[0];
                                }
                                plot.DataChanged();

                                colorIndex++;
                                if (colorIndex >= colorOpts.Length)
                                {
                                    colorIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddMetricToList( string metricToken, ArrayList list ) {
            foreach( EvalData eval in SqlEval.Variables ) {
                if( eval.Token == metricToken ) {
                    list.Add( eval );
                    break;
                }
            }
        }

        private ArrayList calcStats(ArrayList curves)
        {
            int sample_num = -1;
            int length = 0;
            for(int i = 0; i < curves.Count; i++)
            {
                DataCurve crv = (DataCurve)curves[i];
                if(crv != null && crv.Y.Length > length)
                {
                    sample_num = i;
                    length = crv.Y.Length;
                }
            }

            if (sample_num == -1)
            {
                //MessageBox.Show("None of the selected sims have results", "Results Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return curves;
            }

            double[] avg = new double[((DataCurve)curves[sample_num]).Y.Length];
            double num_curves = 0;

            for (int i = 0; i < avg.Length; i++)
            {
                num_curves = 0;
                avg[i] = 0;
                foreach (DataCurve crv in curves)
                {
                    if (crv != null && i < crv.Y.Length)
                    {
                        if (crv.Y[i] != 0)
                        {
                            avg[i] += crv.Y[i];
                            num_curves += 1;
                        }
                    }
                }
                avg[i] = avg[i] / num_curves;
            }

            double[] high_bar = new double[avg.Length];
            double[] low_bar = new double[avg.Length];
            double high_num = 0;
            double low_num = 0;
            
            for (int i = 0; i < avg.Length; i++)
            {
                high_num = 0;
                low_num = 0;
                low_bar[i] = 0;
                high_bar[i] = 0;
                foreach (DataCurve crv in curves)
                {
                    if (crv != null && i < crv.Y.Length)
                    {
                        if (crv.Y[i] != 0 && crv.Y[i] < avg[i])
                        {
                            low_bar[i] += Math.Pow(crv.Y[i] - avg[i], 2.0);
                            low_num += 1;
                        }
                        else if(crv.Y[i] != 0)
                        {
                            high_bar[i] += Math.Pow(crv.Y[i] - avg[i], 2.0);
                            high_num += 1;
                        }
                    }
                }
                high_bar[i] = avg[i] + Math.Sqrt(high_bar[i] / high_num);
                low_bar[i] = avg[i] - Math.Sqrt(low_bar[i] / low_num);
            }

            DataCurve average = new DataCurve(avg.Length);
            DataCurve high = new DataCurve(high_bar.Length);
            DataCurve low = new DataCurve(low_bar.Length);

            DataCurve sample = (DataCurve)curves[sample_num];


            for (int i = 0; i < sample.Y.Length && i < sample.X.Length; i++)
            {
                average.Add(sample.X[i], avg[i]);
                high.Add(sample.X[i], high_bar[i]);
                low.Add(sample.X[i], low_bar[i]);
            }

            average.Color = sample.Color;
            high.Color = sample.Color;
            low.Color = sample.Color;

            average.Label = sample.Label + " Average";
            high.Label = sample.Label + " High Std Dev";
            low.Label = sample.Label + " Low Std Dev";

            average.Units = sample.Units;
            high.Units = sample.Units;
            low.Units = sample.Units;

            ArrayList ret_val = new ArrayList();
            ret_val.Add(average);
            ret_val.Add(high);
            ret_val.Add(low);

            return ret_val;

        }

        private DataCurve plotData(SqlEval eval, string label, ArrayList prods, int segment_id, int channel_id, int days)
        {
            if (prods.Count == 0)
            {
                return null;
            }

            ArrayList curves = new ArrayList();

            MrktSimDBSchema.productRow product = (MrktSimDBSchema.productRow)prods[0];

            string productName = null;
            string segmentName = null;
            string channelName = null;

            if (product.product_id == ModelDb.AllID)
            {
                productName = "Total ";
            }
            else
            {
                productName = product.product_name + " ";
            }

            if (segment_id != ModelDb.AllID)
            {
                MrktSimDBSchema.segmentRow segment = theDb.Data.segment.FindBysegment_id(segment_id);
                segmentName = segment.segment_name + " ";
            }

            if (channel_id != ModelDb.AllID)
            {
                MrktSimDBSchema.channelRow channel = theDb.Data.channel.FindBychannel_id(channel_id);
                channelName = channel.channel_name + " ";
            }

            DataCurve crv = null;

            eval.Product = product.product_id;
            eval.Segment = segment_id;
            eval.Channel = channel_id;

            eval.Start = startEndDate1.Start;
            eval.End = startEndDate1.End;

            crv = eval.CreateDataCurve(days);

            if (crv == null)
                return null;

            // contruct label for curve
            crv.Units = eval.EvalData.units;

            if( this.doingConsumerPanel == false ) {
                crv.Label = eval.EvalData.ToString();

                crv.Label += "  " + label;
                crv.Label += productName + segmentName + channelName;
            }
            else {
                crv.Label = productName + "::" + eval.EvalData.ToString();     // consumer panel data uses product name for row header and metric name for column header
            }

            crv.Color = CurrentColor;

            return crv;
        }


		private void addToGraphButton_Click(object sender, System.EventArgs e)
		{
			if (columnNameBox.SelectedItem == null)
				return;

			EvalData eval = (EvalData) ((DataRowView) columnNameBox.SelectedItem).Row["evalData"];

			if (dataListBox.Items.IndexOf(eval) < 0)
			{
				dataListBox.Items.Add(eval);
			}
		}

	
		private void removeItemButton_Click(object sender, System.EventArgs e)
		{
			if (dataListBox.SelectedItem == null)
			{
				return;
			}

			dataListBox.Items.Remove(dataListBox.SelectedItem);
		}

		private void dataListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (dataListBox.SelectedItem == null)
			{
				removeItemButton.Enabled = false;
				return;
			}

			removeItemButton.Enabled = true;
		}

		
		private void columnNameBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.columnNameBox.SelectedItem == null)
			{
				//productPicker1.Enabled = false;
				segmentPicker1.Enabled = false;
				channelPicker1.Enabled = false;
				simBox.Enabled = false;
				addToGraphButton.Enabled = false;
				return;
			}

			simBox.Enabled = true;
			addToGraphButton.Enabled = true;

			DataRow row = ((DataRowView) columnNameBox.SelectedItem).Row;

			EvalData evalData = (EvalData) row["evalData"];

			

			descriptionBox.Text = row["descr"].ToString();
		}

		private void setDataTypeFilter()
		{
			int index = dataTableBox.SelectedIndex;
            if( index != -1 ) {
                DataType selectedType = (DataType)Enum.Parse( typeof( DataType ), dataTableBox.SelectedItem.ToString() );

                this.dataColView.RowFilter = "dataType = " + (int)selectedType;
            }
            else {
                this.dataColView.RowFilter = "dataType = 0";    // default is sim results
            }

			// 0 is the Simulation results
			// 1 is Market inputs
			// 2 is the Real results
			// both 1 and 2 use the same evaluators

            //switch ((DataType) index)
            //{
            //    case DataType:
            //        this.dataColView.RowFilter = "dataType = " + (int) DataType.SimResults;
            //        break;

            //    case 1:
            //        this.dataColView.RowFilter = "dataType = " + (int) DataType.InputData;
            //        break;

            //    case 2:
            //        this.dataColView.RowFilter = "dataType = " + (int) DataType.externalData;
            //        break;
            //}

		
		}

		private void dataTableBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			setDataTypeFilter();
		}

		private void summaryButton_Click(object sender, System.EventArgs e)
		{
			//Check to make sure data columns and sims are selected
			bool noSimsChecked = true;
//			bool noSummaryChecked = true;
//			bool noTotalChecked = true;
			for(int i = 0; i < simBox.Items.Count; ++i)
			{
				if(simBox.GetItemChecked(i))
				{
					noSimsChecked = false;
					break;
				}
			}

			Metric[] metrics = selectedMetrics();

		
			if(noSimsChecked && metrics.Length == 0)
			{
				MessageBox.Show("Error: There are no sims or data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			if(noSimsChecked)
			{
				MessageBox.Show("Error: There are no sims selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			if(metrics.Length == 0)
			{
				MessageBox.Show("Error: There are no data columns selected","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			Summary dlg = new Summary(metrics);
		
			dlg.Connection = theDb.Connection;

			foreach(Object obj in simBox.CheckedItems)
			{
				ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

                foreach (int run_id in run_ids)
                {
                    dlg.AddMetricTable(run_id);
                }
			}

			dlg.Show();
		}

		private void initializeSimTable()
		{
			DateTime start = new DateTime(9999,1,1);
			DateTime end = new DateTime(1, 1, 1);

			simTable.Clear();

			foreach(MrktSimDb.MrktSimDBSchema.sim_queueRow sim in theDb.Data.sim_queue.Rows)
			{
				DataRow simRow = simTable.NewRow();
                ArrayList sim_ids = new ArrayList(1);
                ArrayList run_ids = new ArrayList(1);
                ArrayList sim_names = new ArrayList(1);

                run_ids.Add(sim.run_id);
                sim_ids.Add(sim.sim_id);
                

				simRow["sim"] = sim.simulationRow.name + " - " + sim.name;
                sim_names.Add(simRow["sim"]);
				simRow["run_ids"] = run_ids;
				simRow["sim_ids"] = sim_ids;
                simRow["sim_names"] = sim_names;

				simTable.Rows.Add(simRow);

				if (sim.simulationRow.start_date < start)
					start = sim.simulationRow.start_date;

				
				if (sim.simulationRow.end_date > end)
					end = sim.simulationRow.end_date;

			}

			simTable.AcceptChanges();

			if (start < end)
			{
				this.startEndDate1.Start = start;
				this.startEndDate1.End = end;
			}
		}

		private void ResultsForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if( reloadCheckBox.Checked == true && settingsComboBox.SelectedItem != null ) {
                Settings.CurrentNamedSetting = settingsComboBox.SelectedItem.ToString();
            }
            else {
                Settings.CurrentNamedSetting = "";
            }
            Settings.Save();
		}

        // JimJ -- this is now handled by UpdateNamedResultsSettingsFromUI()
        //private void saveGraphSettings_Click(object sender, System.EventArgs e)
        //{
        //    // Grab current info and create a new Graph Object
        //    if (Settings.DefaultGraphOption == null)
        //        Settings.DefaultGraphOption = new GraphOption();

        //    Settings.DefaultGraphOption.productPerGraph = this.productPerChart.Checked;
        //    Settings.DefaultGraphOption.segmentPerGraph = this.segmentPerChart.Checked;
        //    Settings.DefaultGraphOption.sumProducts = this.sumProducts.Checked;
        //    Settings.DefaultGraphOption.sumSegments = this.sumSegments.Checked;

        //    Settings.DefaultGraphOption.tokens = new string[dataListBox.Items.Count];

        //    for(int index = 0; index < dataListBox.Items.Count; index++)
        //    {
        //        EvalData evalData = (EvalData) dataListBox.Items[index];

        //        Settings.DefaultGraphOption.tokens[index] = evalData.Token;
        //    }
        //}

		private void configureSummaryLists()
		{

//			// create summary dlg but do not show it
//			// we only need the columns
//			Summary dlg = new Summary();
//
//			dlg.Db = theDb;
//
//			foreach(DataGridColumnStyle sumCol in dlg.SummaryColumns)
//			{
//				summaryColumnsBox.Items.Add(sumCol.HeaderText, true);
//			}
//
//			foreach(DataGridColumnStyle totCol in dlg.TotalsColumns)
//			{
//				totalsColumnsBox.Items.Add(totCol.HeaderText, true);
//			}
//
//			if (Settings.DefaultSummaryOption != null)
//			{
//				if (Settings.DefaultSummaryOption.checkedSummaryItems != null)
//				{
//					int index = 0;
//
//					for(index = 0; index < summaryColumnsBox.Items.Count; ++index)
//						summaryColumnsBox.SetItemCheckState(index, CheckState.Unchecked);
//					// summaryColumnsBox.SetItemChecked(index, false)
//
//					foreach( string text in Settings.DefaultSummaryOption.checkedSummaryItems)
//					{
//						if (text == null)
//							continue;
//
//						index = summaryColumnsBox.Items.IndexOf(text);
//
//						if (index >= 0)
//							summaryColumnsBox.SetItemCheckState(index, CheckState.Checked);
//					}
//				}
//
//
//				if (Settings.DefaultSummaryOption.checkedTotalsItems != null)
//				{
//					int index = 0;
//
//					for(index = 0; index < totalsColumnsBox.Items.Count; ++index)
//						totalsColumnsBox.SetItemCheckState(index, CheckState.Unchecked);
//
//					foreach( string text in Settings.DefaultSummaryOption.checkedTotalsItems)
//					{
//						if (text == null)
//							continue;
//
//						index = totalsColumnsBox.Items.IndexOf(text);
//
//						if (index >= 0)
//							totalsColumnsBox.SetItemCheckState(index, CheckState.Checked);
//					}
//
//				}
//			}
		}

		private void saveSummarySettingsButton_Click(object sender, System.EventArgs e)
		{
//			if (Settings.DefaultSummaryOption == null)
//				Settings.DefaultSummaryOption = new SummaryOption();
//
//			int num = summaryColumnsBox.CheckedItems.Count;
//
//			if (num > 0)
//			{
//
//				Settings.DefaultSummaryOption.checkedSummaryItems = new string[num];
//
//				for(int sumDex = 0; sumDex < num; ++sumDex)
//				{
//					Settings.DefaultSummaryOption.checkedSummaryItems[sumDex] = summaryColumnsBox.CheckedItems[sumDex].ToString();
//				}
//			}
//			else
//				Settings.DefaultSummaryOption.checkedSummaryItems = null;
//
//
//			num = totalsColumnsBox.CheckedItems.Count;
//
//			if (num > 0)
//			{
//				Settings.DefaultSummaryOption.checkedTotalsItems = new string[num];
//
//				for(int totDex = 0; totDex < num; ++totDex)
//				{
//					Settings.DefaultSummaryOption.checkedTotalsItems[totDex] = totalsColumnsBox.CheckedItems[totDex].ToString();
//				}
//			}
//			else
//				Settings.DefaultSummaryOption.checkedTotalsItems = null;
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl.SelectedTab == this.paramPage)
			{
				simBox.Enabled = false;
				this.timingBox.Enabled = false;
			}
			else
			{
				simBox.Enabled = true;
				this.timingBox.Enabled = true;
			}
		}


		#region Radio Buttons
		private void productPerChart_CheckedChanged(object sender, System.EventArgs e)
		{
			if (productPerChart.Checked)
				sumSegments.Enabled = true;
			else
				sumSegments.Enabled = false;
		}

		private void segmentPerChart_CheckedChanged(object sender, System.EventArgs e)
		{
			if (segmentPerChart.Checked)
				sumProducts.Enabled = true;
			else
				sumProducts.Enabled = false;
		}

		private void oneChart_CheckedChanged(object sender, System.EventArgs e)
		{
			if (oneChart.Checked)
			{
				sumSegments.Checked = true;
				sumProducts.Checked = false;

				sumSegments.Enabled = false;
				sumProducts.Enabled = false;
			}
		}
		#endregion

		private void regressionButton_Click(object sender, System.EventArgs e)
		{
			//  mulitple scenarios can be added
			if (simBox.CheckedItems.Count != 1)
			{
				MessageBox.Show("Please check exactly one simulation to use as a standard");
				return;
			}

			object obj = simBox.CheckedItems[0];

			ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

            if (run_ids.Count > 1)
            {
                MessageBox.Show("Cannot create regression on multiple sim, please ungroup.");
                return;
            }

            int run_id = (int)run_ids[0];


            DialogResult rslt = MessageBox.Show( "This will delete all real sales data", "Delete Real Sales",
				MessageBoxButtons.OKCancel);

			if (rslt != DialogResult.OK)
				return;

			// get time span
			int days = 0;

			switch( this.timeSpan.SelectedIndex)
			{
				case 0:
					days = (int) dailyAverageBox.Value;
					break;
				case 1:
					days = 7 * ((int) dailyAverageBox.Value);
					break;
				case 2:
					days = 30 * ((int) dailyAverageBox.Value);
					break;
				case 3:
					days = 365 * ((int) dailyAverageBox.Value);
					break;
			}

			if (days == 0)
				return;

			System.Data.OleDb.OleDbCommand command = ProjectDb.newOleDbCommand();
			command.Connection = theDb.Connection;

            command.CommandText = "DELETE FROM external_data WHERE model_id = " + theDb.Model.model_id;

			command.Connection.Open();
			int numRows = command.ExecuteNonQuery();
			command.Connection.Close();

			DateTime curDate = this.startEndDate1.Start;
			DateTime endDate = this.startEndDate1.End;
			
			// add one to account for fourly differences
			endDate = endDate.AddDays(1);

			while(curDate < endDate)
			{
				DateTime curEnd = curDate.AddDays(days);
				DateTime endEvalDate = curEnd.AddDays(-1);

				// we will actaally do the calcualtion as if from create charts
				// set this run as real data
				command.CommandText = "INSERT INTO external_data (model_id, calendar_date, product_id, segment_id, channel_id, value, type) ";
			
				command.CommandText += "SELECT ";
				command.CommandText += theDb.Model.model_id + " as model_id, ";
				command.CommandText += "'" + endEvalDate.ToShortDateString() + "' as calendar_date, results_std.product_id, " + 
                    " -1 as segment_id, results_std.channel_id, SUM(num_sku_bought) as value, 1 as type ";
				command.CommandText += " FROM results_std, product WHERE run_id = " + run_id;
                command.CommandText += " AND product.product_id = results_std.product_id ";
                command.CommandText += " AND product.brand_id = 1 ";
				command.CommandText += " AND calendar_date >= " + "'" + curDate.ToShortDateString() + "'";
				command.CommandText += " AND calendar_date < " + "'" + curEnd.ToShortDateString() + "'";
				command.CommandText +=  " GROUP BY results_std.product_id, results_std.channel_id";

				command.Connection.Open();
				numRows = command.ExecuteNonQuery();
				command.Connection.Close();
				curDate = curEnd;
			}
		}

		private void recalcButton_Click(object sender, System.EventArgs e)
		{
			DialogResult rslt = MessageBox.Show("Selected metrics for the selected simulations will be recalculated at the new start and end dates;", "Recompute Metrics", MessageBoxButtons.OKCancel);

			if (rslt != DialogResult.OK)
			{
				return;
			}

			RecomputeMetrics();

			
		}

        public void RecomputeMetrics() {
            RecomputeMetrics( null );
        }
            
		public void RecomputeMetrics( Metric[] specificMetrics )
		{
            //if( specificMetrics == null ) {
            //    this.refresh();
            //}

			foreach(Object obj in simBox.CheckedItems)
			{
				ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

                foreach (int run_id in run_ids)
                {
                    MrktSimDBSchema.sim_queueRow sim = theDb.Data.sim_queue.FindByrun_id(run_id);
                    MrktSimDBSchema.simulationRow simulation = sim.simulationRow;

                    if (simulation.delete_std_results)
                    {
                        MessageBox.Show("Cannot recompute metrics for a simulation that has its results deleted");
                        return;
                    }
                }
			}

 			Metric[] metrics = null;
            if( specificMetrics != null ) {
                metrics = specificMetrics;
            }
            else {
                metrics = selectedMetrics();
            }

			// now compute
			MetricMan metricMan = new MetricMan(theDb);

			using (ProcessStatus progress = new ProcessStatus())
			{
				metricMan.UpdateProgress +=new MrktSimDb.Metrics.MetricMan.Progress(progress.Progress);

				progress.ProcessType = "Computing";
				progress.CurrentProcess = "";

				// progress.Location = this.Location;
				progress.Show();

				// first delete all the metrics
				foreach(Object obj in simBox.CheckedItems)
				{
					ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

                    foreach (int run_id in run_ids)
                    {
                        if (!metricMan.Compute(run_id, metrics, startEndDate1.Start, startEndDate1.End))
                        {
                            string sim_name = (string)((DataRowView)obj).Row["sim"];
                            MessageBox.Show("The simulation may have been deleted, the simluation list will be refreshed");
                            this.refresh();
                            return;
                        }
                    }
				}
			}
		}

		private Metric[] selectedMetrics()
		{
			int count = this.simMetricBox.CheckedItems.Count + this.calMetricBox.CheckedItems.Count;

			Metric[] metrics = new Metric[count];
			int index = 0;

			foreach(Object obj in simMetricBox.CheckedItems)
			{
				metrics[index] = (Metric) obj;
				index++;
			}

			foreach(Object obj in calMetricBox.CheckedItems)
			{
				metrics[index] = (Metric) obj;
				index++;
			}

			return metrics;
		}

		private void refreshButton_Click(object sender, System.EventArgs e)
		{
			this.refresh();
		}

		private void SelectByType_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem) sender;
			productTree.SelectByType(item.Text);
		}

		#region AutoPilot

		public void addEvalByToken(string token)
		{
			foreach(DataRowView row in columnNameBox.Items)
			{
				EvalData eval = (EvalData) row["evalData"];
				if(eval.Token.ToLower().CompareTo(token.ToLower()) == 0)
				{
					if (dataListBox.Items.IndexOf(eval) < 0)
					{
						dataListBox.Items.Add(eval);
					}
				}
			}
		}

		public void clearEvals()
		{
			dataListBox.Items.Clear();
		}

		public void switchToMarketing()
		{
			this.dataTableBox.SelectedIndex = 1;
		}

		public void switchToResults()
		{
			this.dataTableBox.SelectedIndex = 0;
		}

		/// <summary>
		/// selects exactly one metric
		/// </summary>
		/// <param name="metric"></param>
		public void SelectMetric(Metric metric)
		{
			for(int index = 0; index < simMetricBox.Items.Count; ++index)
			{
				if (metric == simMetricBox.Items[index])
				{
					simMetricBox.SetItemChecked(index, true);
				}
				else
				{
					simMetricBox.SetItemChecked(index, false);
				}
			}
		}

		public void ClearSelectedProducts()
		{
			productTree.UnSelectAll();
		}

		public ProductForest SelectProduct(int product_id)
		{
			this.productTree.SelectByProductID(product_id);
			
			return productTree.getProductForest();
		}

		public void SelectMetricsFromList(ArrayList list)
		{
		}

		public void AllProductsOnOneGraph(bool yes)
		{
			if(yes)
			{
				this.oneChart.Checked = true;
				this.productPerChart.Checked = false;
			}
			else
			{
				this.oneChart.Checked = true;
				this.productPerChart.Checked = false;
			}
		}


		public void ShowGraph(ArrayList scenarioList, DateTime start, DateTime end)
		{
			simBox.ClearSelected();
			foreach(MrktSimDb.MrktSimDBSchema.simulationRow simulation in scenarioList)
			{
				// get simulation id and match to simTable to get object
				for(int index = 0; index < simTable.Rows.Count; ++index)
				{
                    if ((int)simTable.Rows[index]["sim_id"] == simulation.id)
					{
						simBox.SetItemChecked(index, true);
					}
				}
			}

			this.startEndDate1.Start = start;
			this.startEndDate1.End = end;

			graphButton_Click(this, null);
		}

		public string WriteSummaryToCSV(ArrayList scenarioList, System.IO.StreamWriter writer)
		{
			// first uncheck all
			for(int index = 0; index < simTable.Rows.Count; ++index)
			{
				simBox.SetItemChecked(index, false);
			}

			foreach(MrktSimDb.MrktSimDBSchema.simulationRow simultion in scenarioList)
			{
				// get simulation id and match to simTable to get object
				for(int index = 0; index < simTable.Rows.Count; ++index)
				{
                    if ((int)simTable.Rows[index]["sim_id"] == simultion.id)
					{
						simBox.SetItemChecked(index, true);
					}
				}
			}

			RecomputeMetrics();

			return writeSimsToCSV(writer);
		}

		public ArrayList GetMetricTables(ArrayList scenarioList)
		{
			ArrayList tableList = new ArrayList();
			// first uncheck all
			for(int index = 0; index < simTable.Rows.Count; ++index)
			{
				simBox.SetItemChecked(index, false);
			}

			foreach(MrktSimDb.MrktSimDBSchema.simulationRow simulation in scenarioList)
			{
				// get simulation id and match to simTable to get object
				for(int index = 0; index < simTable.Rows.Count; ++index)
				{
					if ((int) simTable.Rows[index]["sim_id"] == simulation.id)
					{
						simBox.SetItemChecked(index, true);
					}
				}
			}

			RecomputeMetrics();

			Metric[] metrics = selectedMetrics();

			Summary dlg = new Summary(metrics);
		
			dlg.Connection = theDb.Connection;

			foreach(Object obj in simBox.CheckedItems)
			{
				ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

                foreach (int run_id in run_ids)
                {
                    dlg.AddMetricTable(run_id);
                }
			}

			dlg.GetTables(tableList);

			return tableList;

		}


		public void SetStartEnd(DateTime start, DateTime end)
		{
			this.startEndDate1.Start = start;
			this.startEndDate1.End = end;
		}
		
		#endregion

		private string writeSimsToCSV(System.IO.StreamWriter writer)
		{
			//Check to make sure data columns and sims are selected
			bool noSimsChecked = true;
			//			bool noSummaryChecked = true;
			//			bool noTotalChecked = true;
			for(int i = 0; i < simBox.Items.Count; ++i)
			{
				if(simBox.GetItemChecked(i))
				{
					noSimsChecked = false;
					break;
				}
			}

			Metric[] metrics = selectedMetrics();

		
			if(noSimsChecked && metrics.Length == 0)
			{
				return "Error: There are no sims or data columns selected";
			}
			
			if(noSimsChecked)
			{
				return "Error: There are no sims selected";
			}
			if(metrics.Length == 0)
			{
				return "Error: There are no data columns selected";
			}

			Summary dlg = new Summary(metrics);
		
			dlg.Connection = theDb.Connection;

			foreach(Object obj in simBox.CheckedItems)
			{
				ArrayList run_ids = (ArrayList) ((DataRowView) obj).Row["run_ids"];

                foreach (int run_id in run_ids)
                {
                    dlg.AddMetricTable(run_id);
                }
			}

			dlg.WriteToCVS(writer);

			return null;
		}

		private void csvDump_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();
			saveFileDlg.DefaultExt = ".csv";
			saveFileDlg.Filter = "CSV File (*.csv)|*.csv";
			saveFileDlg.CheckFileExists = false;
			DialogResult rslt = saveFileDlg.ShowDialog();
			if (rslt == DialogResult.OK)
			{
				string fileName = saveFileDlg.FileName;
				System.IO.StreamWriter writer;

				try
				{
					writer = new System.IO.StreamWriter(fileName);
				}
				catch(System.IO.IOException oops)
				{
					MessageBox.Show(oops.Message);
					return;
				}

				writeSimsToCSV(writer);

				writer.Flush();
				writer.Close();
			}
		}

        private void UnGroupButton_Click(object sender, EventArgs e)
        {
            simBox.SuspendLayout();

            ArrayList rows_to_delete = new ArrayList();
            ArrayList row_to_add = new ArrayList();

            foreach (DataRowView obj in simBox.CheckedItems)
            {
                rows_to_delete.Add(obj.Row);
                ArrayList run_ids = (ArrayList)obj.Row["run_ids"];
                ArrayList sim_ids = (ArrayList)obj.Row["sim_ids"];
                ArrayList sim_names = (ArrayList)obj.Row["sim_names"];
                for (int i = 0; i < run_ids.Count; i++)
                {
                    ArrayList run_id = new ArrayList(1);
                    ArrayList sim_id = new ArrayList(1);
                    ArrayList sim_name = new ArrayList(1);

                    run_id.Add(run_ids[i]);
                    sim_id.Add(sim_ids[i]);
                    sim_name.Add(sim_names[i]);

                    DataRow row = simTable.NewRow();
                    row["sim"] = sim_names[i];
                    row["run_ids"] = run_id;
                    row["sim_ids"] = sim_id;
                    row["sim_names"] = sim_name;

                    row_to_add.Add(row);
                }
            }

            foreach (DataRow row in rows_to_delete)
            {
                simTable.Rows.Remove(row);
            }

            foreach (DataRow row in row_to_add)
            {
                simTable.Rows.Add(row);
            }

            simTable.AcceptChanges();

            simBox.ResumeLayout();
            simBox.Refresh();
        }

        private void GroupButton_Click(object sender, EventArgs e)
        {
            if(simBox.CheckedItems.Count < 2)
            {
                return;
            }

            String name = (string)((DataRowView)simBox.CheckedItems[0])["sim"];

            GroupingDialog dlg = new GroupingDialog(name);

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.Cancel)
            {
                return;
            }

            name = dlg.GroupName;

            simBox.SuspendLayout();

            ArrayList new_run_ids = new ArrayList();
            ArrayList new_sim_ids = new ArrayList();
            ArrayList new_sim_names = new ArrayList();

            foreach (DataRowView obj in simBox.CheckedItems)
            {
                new_run_ids.AddRange((ArrayList)obj.Row["run_ids"]);

                new_sim_ids.AddRange((ArrayList)obj.Row["sim_ids"]);

                new_sim_names.AddRange((ArrayList)obj.Row["sim_names"]);
            }

            DataRow new_row = simTable.NewRow();
            new_row["sim"] = name;
            new_row["run_ids"] = new_run_ids;
            new_row["sim_ids"] = new_sim_ids;
            new_row["sim_names"] = new_sim_names;

            ArrayList rows_to_delete = new ArrayList();
            foreach (DataRowView obj in simBox.CheckedItems)
            {
                rows_to_delete.Add(obj.Row);
            }

            foreach (DataRow row in rows_to_delete)
            {
                simTable.Rows.Remove(row);
            }

            simTable.Rows.Add(new_row);

            simTable.AcceptChanges();

            simBox.ResumeLayout();
            simBox.Refresh();
        }

        /// <summary>
        /// Writes the data for the "consumer panel" summary to a CSV file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void writeCsvButton_Click( object sender, EventArgs e ) {

            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.Filter = "CSV Files (*.csv)|*.csv";
            saveDlg.Title = "Save Graph Data to CSV File";

            DialogResult resp = saveDlg.ShowDialog();

            if( resp == DialogResult.OK ) {

                ////ArrayList scenarioIDList = new ArrayList();
                ////foreach( DataRowView obj in simBox.CheckedItems ) {
                ////    scenarioIDList.AddRange( (ArrayList)obj.Row[ "sim_ids" ] );
                ////}

                MultiGrapher hiddenGraph = GraphNoShow( startEndDate1.Start, startEndDate1.End, SummaryReportType.ConsumerPanel, false );

                if( hiddenGraph == null ) {
                    return;
                }

                Stream fileStream = saveDlg.OpenFile();
                StreamWriter sw = new StreamWriter( fileStream );
           
                hiddenGraph.write( sw, true );      // write the CSV data to the file

                sw.Flush();
                sw.Close();
                fileStream.Close();

                CompletionDialog conf = new CompletionDialog( "Consumer Panel Data Written Successfully." );
                conf.ShowDialog();
            }
        }

        /// <summary>
        /// Selects or un-selects all of the sims in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllCheckBox_CheckedChanged( object sender, EventArgs e ) {
            bool doCheck = selectAllCheckBox.Checked;

            for( int i = 0; i < simBox.Items.Count; i++ ) {
                simBox.SetItemChecked( i, doCheck );
            }
        }

        

        #region Named Settings Methods

        /// <summary>
        /// Lets the user save all of the current results-form settings with a new name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsButton_Click( object sender, EventArgs e ) {
            CreateNewNamedResults();
        }

        /// <summary>
        /// Creates a new NamedResults object with the current UI settings.
        /// </summary>
        /// <returns></returns>
        private bool CreateNewNamedResults(){
            string msg = "Save Results Settings";
            NameAndDescr2 nameDlg = new NameAndDescr2( msg, Color.Beige, "SaveResultsSetting" );
            DialogResult resp = nameDlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return false;
            }

            if( Settings.NamedResultsSettings == null ) {
                Settings.NamedResultsSettings = new NamedSettings[ 0 ];
            }

            // make sure the settings name is a new one
            foreach( NamedSettings existingNamedSettings in Settings.NamedResultsSettings ) {
                if( existingNamedSettings != null ) {
                    if( nameDlg.ObjName == existingNamedSettings.SettingsName ) {

                        string msg2 = String.Format( "\r\n      Error: Results Settings \"{0}\" already exists.      \r\n", existingNamedSettings.SettingsName );
                        ConfirmDialog cdlg = new ConfirmDialog( msg2, "Error", null );
                        cdlg.SetOkButtonOnlyStyle();
                        cdlg.SelectWarningIcon();
                        cdlg.Height += 20;
                        cdlg.ShowDialog();
                        return false;
                     }
                }
            }

            // create a new NamedSettings object
            NamedSettings newSettings = new NamedSettings();
            newSettings.SettingsName = nameDlg.ObjName;
            newSettings.SettingsDescription = nameDlg.ObjDescription;
            newSettings.ControllerClass = "ResultsForm";
            newSettings.RendererClass = "MultiGrapher";

            return AddSettingAndSave( newSettings );
        }

        private bool AddSettingAndSave( NamedSettings newSettings ) {
            // create the updated names list
            ArrayList newSettingsList = new ArrayList();
            newSettingsList.Add( newSettings );

            foreach( NamedSettings existingNamedSettings in Settings.NamedResultsSettings ) {
                if( existingNamedSettings != null ) {        // strip out any null settings 
                    newSettingsList.Add( existingNamedSettings );
                }
            }

            NamedSettings[] newSettingsArray = new NamedSettings[ newSettingsList.Count ];
            for( int i = 0; i < newSettingsList.Count; i++ ){
                newSettingsArray[ i ] = (NamedSettings)newSettingsList[ i ];
            }

            // emplace the updated list as the master
            Settings.NamedResultsSettings = newSettingsArray;

            UpdateNamedResultsSettingsFromUI( newSettings );
            Settings.Save();

            RefreshSettingsList( newSettings.SettingsName );

            if( newSettings.SettingsDescription == "" ) {
                descriptionBox.Text = "Settings Saved: " + newSettings.SettingsName;
            }
            else {
                descriptionBox.Text = "Settings Saved: " + newSettings.SettingsName + " - " + newSettings.SettingsDescription;
            }
            toolTip.SetToolTip( settingsComboBox, newSettings.SettingsDescription );
            return true;
        }

        private void RefreshSettingsList( string nameToSelect ) {
            RefreshSettingsList( nameToSelect, false );
        }

        /// <summary>
        /// Reloads the list of saved results settings.  Fires a selectedIndexChanged event after selecting the the specified name, if fireEvent is true.
        /// </summary>
        /// <param name="nameToSelect"></param>
        /// <param name="fireEvent"></param>
        private void RefreshSettingsList( string nameToSelect, bool fireEvent ) {
            settingsComboBox.Items.Clear();
            if( Settings.NamedResultsSettings != null ) {
                ArrayList applicableItems = new ArrayList();
                for( int i = 0; i < Settings.NamedResultsSettings.Length; i++ ) {

                    // show only the results settiings for this type of controller
                    if( Settings.NamedResultsSettings[ i ].ControllerClass == "ResultsForm" ){
                        applicableItems.Add( Settings.NamedResultsSettings[ i ].SettingsName );
                    }
                }

                string[] names = new string[ applicableItems.Count ];
                applicableItems.CopyTo( names );
                Array.Sort( names );
                settingsComboBox.Items.AddRange( names );

                if( fireEvent == false ) {
                    settingsComboBox.SelectedIndexChanged -= new EventHandler( settingsComboBox_SelectedIndexChanged );
                }

                settingsComboBox.SelectedItem = nameToSelect;

                if( fireEvent == false ) {
                   settingsComboBox.SelectedIndexChanged += new EventHandler( settingsComboBox_SelectedIndexChanged );
               }

            }
        }

        /// <summary>
        /// Handles a click on the delete-settings button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delButton_Click( object sender, EventArgs e ) {
            string msg = String.Format( "\r\n        Delete \"{0}\" Results Settings?        \r\n", settingsComboBox.SelectedItem );
            ConfirmDialog cdlg = new ConfirmDialog( msg, "Confirm Settings Delete", null );
            cdlg.SetOkCancelButtonStyle();
            cdlg.SelectQuestionIcon();
            cdlg.Height += 20;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            ArrayList newSettings = new ArrayList();
            for( int i = 0; i < Settings.NamedResultsSettings.Length; i++ ) {
                if( Settings.NamedResultsSettings[ i ].SettingsName != (string)settingsComboBox.SelectedItem ) {
                    newSettings.Add( Settings.NamedResultsSettings[ i ] );
                }
            }

            NamedSettings[] newSettingsArray = new NamedSettings[ newSettings.Count ];
            for( int i = 0; i < newSettings.Count; i++ ) {
                newSettingsArray[ i ] = (NamedSettings)newSettings[ i ];
            }

            Settings.NamedResultsSettings = newSettingsArray;

            RefreshSettingsList( null );
            delButton.Enabled = false;
        }

        /// <summary>
        /// Saves the current results-form settings.  Asks for a name if the current settings are not named.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click( object sender, EventArgs e ) {
            bool confirm = true;
            if( settingsComboBox.Items.Count == 0 || (string)settingsComboBox.SelectedItem == null || (string)settingsComboBox.SelectedItem == "" ) {
                bool didit = CreateNewNamedResults();
                if( didit == false ) {
                    return;
                }
                confirm = false;
            }

            NamedSettings curSettings = GetCurrentResultsSettings();

            // change the description only, of Ctrl-Click was used
            if( confirm == true &&( Control.ModifierKeys & Keys.Control ) == Keys.Control ) {
                string msg = "Change Results Settings Description";
                NameAndDescr2 nameDlg = new NameAndDescr2( msg, Color.Beige, "SaveResultsSetting" );
                nameDlg.ObjName = curSettings.SettingsName;
                nameDlg.ObjDescription = curSettings.SettingsDescription;
                nameDlg.DisableNameField();
                DialogResult resp = nameDlg.ShowDialog();
                if( resp == DialogResult.OK ) {
                    curSettings.SettingsDescription = nameDlg.ObjDescription;
                }
                else {
                    return;  // user cancelled
                }
            }

            UpdateNamedResultsSettingsFromUI( curSettings );
            Settings.Save();

            string msg2 = String.Format( "    Settings Saved    \r\n", curSettings.SettingsName );
            CompletionDialog cdlg = new CompletionDialog( msg2 );
            cdlg.ShowDialog();

            if( curSettings.SettingsDescription == "" ) {
                descriptionBox.Text = "Settings Saved: " + curSettings.SettingsName;
            }
            else {
                descriptionBox.Text = "Settings Saved: " + curSettings.SettingsName + " - " + curSettings.SettingsDescription;
            }
            toolTip.SetToolTip( settingsComboBox, curSettings.SettingsDescription );
        }

        /// <summary>
        /// Returns the NamedSettings object corresponding to the currently-selected results settings name.
        /// </summary>
        /// <returns></returns>
        public NamedSettings GetCurrentResultsSettings() {

            if( Settings.NamedResultsSettings == null ) {
                return null;
            }

            string selName = (string)settingsComboBox.SelectedItem;
            foreach( NamedSettings nSettings in Settings.NamedResultsSettings ) {
                if( nSettings.ControllerClass == "ResultsForm" ) {
                    if( nSettings.SettingsName == selName ) {
                        return nSettings;
                    }
                }
            }
            return null;     // shouldn't happen
        }

        /// <summary>
        /// Loads a set of named results settings in response to a change in the selected results set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( settingsComboBox.SelectedIndex != -1 && settingsComboBox.SelectedItem != null && (string)settingsComboBox.SelectedItem != "" ) {
                delButton.Enabled = true;
            }
            else {
                delButton.Enabled = false;
            }

            NamedSettings curSettings = GetCurrentResultsSettings();
            UpdateUIFromNamedResultsSettings( curSettings );

            if( curSettings.SettingsDescription == "" ) {
                descriptionBox.Text = "Settings Selected: " + curSettings.SettingsName;
            }
            else {
                descriptionBox.Text = "Settings Saved: " + curSettings.SettingsName + " - " + curSettings.SettingsDescription;
            }
            toolTip.SetToolTip( settingsComboBox, curSettings.SettingsDescription );
        }

        /// <summary>
        /// Possibly loads a set of results settings when the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsForm_Load( object sender, EventArgs e ) {

            if( Settings.CurrentNamedSetting != null && Settings.CurrentNamedSetting != "" ) {
                reloadCheckBox.Checked = true;
                RefreshSettingsList( Settings.CurrentNamedSetting, true );
            }
            else {
                 RefreshSettingsList( null );
           }
        }

        /// <summary>
        /// Updates the given NamedSettings object, which is expected to be non-null, to reflect current UI state
        /// </summary>
        /// <param name="namedSettings"></param>
        private void UpdateNamedResultsSettingsFromUI( NamedSettings namedSettings ) {
            foreach( Control control in this.Controls ) {
                UpdateResultsSettingsItemFromUI( control, namedSettings );
            }
        }

        /// <summary>
        /// Updates the UI to correspond to the given NamedSettings object, which is expected to be non-null
        /// </summary>
        /// <param name="namedSettings"></param>
        private void UpdateUIFromNamedResultsSettings( NamedSettings namedSettings ) {

            // clear all radio butons
            this.productPerChart.Checked = false;
            this.segmentPerChart.Checked = false;
            this.oneChart.Checked = false;

            foreach( Control control in this.Controls ) {
                UpdateUIFromResultsSettingsItem( control, namedSettings );
            }
        }

        /// <summary>
        /// Recursively processes the given control and all subcontrols
        /// </summary>
        /// <param name="control"></param>
        /// <param name="namedSettings"></param>
        private void UpdateResultsSettingsItemFromUI( Control control, NamedSettings namedSettings ) {

            // update this control
            UpdateResultsSettingsItemFromUIControl( control, namedSettings );

            // recurse
            foreach( Control subcontrol in control.Controls ) {
                UpdateResultsSettingsItemFromUI( subcontrol, namedSettings );
            }
        }

        /// <summary>
        /// Recursively processes the given control and all subcontrols
        /// </summary>
        /// <param name="control"></param>
        /// <param name="namedSettings"></param>
        private void UpdateUIFromResultsSettingsItem( Control control, NamedSettings namedSettings ) {

            // update this control
            UpdateUIControlFromResultsSettingsItem( control, namedSettings );

            // recurse over all subcontrols
            foreach( Control subcontrol in control.Controls ) {
                UpdateUIFromResultsSettingsItem( subcontrol, namedSettings );
            }
        }

        /// <summary>
        /// Update an individual control from settings
        /// </summary>
        /// <param name="control"></param>
        /// <param name="namedSettings"></param>
        private void UpdateUIControlFromResultsSettingsItem( Control control, NamedSettings namedSettings ) {

            // Radio Buttons
            if( control is RadioButton ) {
                bool thisOneChecked = namedSettings.GetBool( control.Name );
                Console.WriteLine( "RB set: {0} -- {1}", control.Name, thisOneChecked );
                if( thisOneChecked == true ) {
                    (control as RadioButton).Checked = true;
                }
            }

            // Checkboxes
            else if( control is CheckBox && control != this.reloadCheckBox ) {        // the "reload" checkbox is handled by the main settings
                (control as CheckBox).Checked = namedSettings.GetBool( control.Name );
            }

            // NumericUpDown controls
            else if( control is NumericUpDown ) {
                NumericUpDown nud = control as NumericUpDown;
                double nValue = namedSettings.GetDouble( control.Name );
                if( nValue < (double)nud.Minimum ) {
                    nValue = (double)nud.Minimum;
                }
                if( nValue > (double)nud.Maximum ) {
                    nValue = (double)nud.Maximum;
                }
                (control as NumericUpDown).Value = Convert.ToDecimal( nValue );
            }

            // DateTimePicker controls
            else if( control is DateTimePicker ) {
                DateTime dateValue = namedSettings.GetDateTime( control.Name );
                if( dateValue < (control as DateTimePicker).MinDate ) {
                    dateValue = (control as DateTimePicker).MinDate;
                }
                if( dateValue > (control as DateTimePicker).MaxDate ) {
                    dateValue = (control as DateTimePicker).MaxDate;
                }
                (control as DateTimePicker).Value = dateValue;
            }

            //ComboBox controls
            else if( control is ComboBox && control != this.settingsComboBox ) {   // the state of the settings=select combobox is handled by the main settings
                ComboBox cb = control as ComboBox;
                string valToSet = namedSettings.GetString( control.Name );
                if( valToSet != null ) {
                    if( cb.ValueMember != null && cb.ValueMember != "" ) {
                        cb.SelectedValue = valToSet;
                    }
                    else {
                        cb.SelectedItem = valToSet;
                    }
                }
            }

            //CheckedListBox controls
            else if( control is CheckedListBox && control != simBox ) {
                CheckedListBox clb = control as CheckedListBox;
                for( int i = 0; i < clb.Items.Count; i++ ) {
                    string s = clb.GetItemText( clb.Items[ i ] );
                    string itemName = clb.Name + "." + s;
                    bool chk = namedSettings.GetBool( itemName );       // unmatched names will not be checked (GetBool() default is false)
                    clb.SetItemChecked( i, chk );
                }
            }

            //ListBox controls
            else if( control is ListBox && control == dataListBox ) {      // ONLY the dataListBox is processed
                ListBox lb = control as ListBox;

                lb.Items.Clear();
                int n = namedSettings.GetInt( lb.Name + ".ItemCount" );

                for( int i = 0; i < n; i++ ) {
                    string iname = lb.Name + ".Item" + i.ToString();

                    // we dop this for backwards compatibility
                    EvalData eval = namedSettings.GetSetting( iname ) as EvalData;

                    if( eval == null ) {
                        string token = namedSettings.GetString( iname );

                        eval = SqlEval.GetEvalDataFromToken( token );
                    }

                    if( eval != null ) {
                        lb.Items.Add( eval );
                    }
                }
            }
    // ProductTree controls
            else if( control is ProductTree ) {
                ProductTree pt = control as ProductTree;
                foreach( TreeNode node in pt.Nodes ) {
                    LoadProductTreeNodeSettings( pt, node, namedSettings );
                }
            }
        }

        /// <summary>
        /// Update an individual setting to reflect the current control state
        /// </summary>
        /// <param name="control"></param>
        /// <param name="namedSettings"></param>
        private void UpdateResultsSettingsItemFromUIControl( Control control, NamedSettings namedSettings ) {

            // Radio Buttons
            if( control is RadioButton ) {
                Console.WriteLine( "RB save: {0} -- {1}", control.Name, (control as RadioButton).Checked );
                namedSettings.SetBool( control.Name, (control as RadioButton).Checked );
            }

            // Checkboxes
            else if( control is CheckBox && control != this.reloadCheckBox ) {        // the "reload" checkbox is handled by the main settings
                namedSettings.SetBool( control.Name, (control as CheckBox).Checked );
            }

            // NumericUpDown controls
            else if( control is NumericUpDown ) {
                namedSettings.SetDouble( control.Name, (double)((control as NumericUpDown).Value) );
            }

            // DateTimePicker controls
            else if( control is DateTimePicker ) {
                namedSettings.SetDateTime( control.Name, (control as DateTimePicker).Value );
            }

            //ComboBox controls
            else if( control is ComboBox && control != this.settingsComboBox ) {   // the state of the settings=select combobox is handled by the main settings
                ComboBox cb = control as ComboBox;
                if( cb.ValueMember != null && cb.ValueMember != "" ) {
                    if( (control as ComboBox).SelectedValue != null ) {
                        string sval = (control as ComboBox).SelectedValue.ToString();
                        namedSettings.SetString( control.Name, sval );
                    }
                }
                else {
                    if( (control as ComboBox).SelectedItem != null ) {
                        string sval = (control as ComboBox).SelectedItem.ToString();
                        namedSettings.SetString( control.Name, sval );
                    }
                }
            }

            //CheckedListBox controls
            else if( control is CheckedListBox ) {
                CheckedListBox clb = control as CheckedListBox;
                for( int i = 0; i < clb.Items.Count; i++ ) {
                    string itemName = clb.Name + "." + clb.GetItemText( clb.Items[ i ] );
                    namedSettings.SetBool( itemName, clb.GetItemChecked( i ) );
                }
            }

            //ListBox controls
            else if( control is ListBox && control == dataListBox ) {     // ONLY the data list box
                ListBox lb = control as ListBox;

                namedSettings.SetInt( lb.Name + ".ItemCount", lb.Items.Count );
                for( int i = 0; i < lb.Items.Count; i++ ) {
                    string itemName = lb.Name + ".Item" + i.ToString();
                    EvalData eval = lb.Items[ i] as EvalData;

                    if (eval != null) {
                        namedSettings.SetString( itemName, eval.token);
                    }
                }
            }

            //ProductTree controls
            else if( control is ProductTree ) {
                ProductTree pt = control as ProductTree;
                foreach( TreeNode node in pt.Nodes ) {
                    SaveProductTreeNodeSettings( pt, node, namedSettings );
                }
            }
        }

        /// <summary>
        /// Saves the settings of the given ProductTree
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="node"></param>
        /// <param name="namedSettings"></param>
        private void SaveProductTreeNodeSettings( ProductTree pt, TreeNode node, NamedSettings namedSettings ) {
            string itemNameCk = pt.Name + ".NodeChkd" + node.Text;
            string itemNameEx = pt.Name + ".NodeExpd" + node.Text;
            namedSettings.SetBool( itemNameCk, node.Checked );
            namedSettings.SetBool( itemNameEx, node.IsExpanded );

            foreach( TreeNode subnode in node.Nodes ) {
                SaveProductTreeNodeSettings( pt, subnode, namedSettings );
            }
        }

        /// <summary>
        /// Loads the checked/expanded states of each tree node from the given NamedSettings
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="node"></param>
        /// <param name="namedSettings"></param>
        private void LoadProductTreeNodeSettings( ProductTree pt, TreeNode node, NamedSettings namedSettings ) {
            string itemNameCk = pt.Name + ".NodeChkd" + node.Text;
            string itemNameEx = pt.Name + ".NodeExpd" + node.Text;
            bool isChkd = namedSettings.GetBool( itemNameCk );
            bool isExpd = namedSettings.GetBool( itemNameEx );
            node.Checked = isChkd;
            if( isExpd == true ) {
                node.Expand();
            }
            else {
                node.Collapse();
            }

            foreach( TreeNode subnode in node.Nodes ) {
                LoadProductTreeNodeSettings( pt, subnode, namedSettings );
            }
        }

        public void mgraph_NewNamedSettingsAdded( NamedSettings newNamedSettingsItem ) {
            AddSettingAndSave( newNamedSettingsItem );
        }

        public void mgraph_NamedSettingsSaved( NamedSettings updatedSettingsItem ) {
            // the settings may have changed in ths form, so don't just save current
            bool foundItem = false;
            for( int i = 0; i < Settings.NamedResultsSettings.Length; i++ ) {
                NamedSettings nSettings = Settings.NamedResultsSettings[ i ];
                if( nSettings.ControllerClass == "ResultsForm" && nSettings.SettingsName == updatedSettingsItem.SettingsName ) {
                    // found the item
                    Settings.NamedResultsSettings[ i ] = updatedSettingsItem;
                    foundItem = true;
                    break;
                }
            }
            if( foundItem == false ) {
                Console.WriteLine( "Error: mgraph_NamedSettingsSaved() cannot find match for given settings object!!" );
            }

            Settings.Save();
        }

        // delete ALL named settings immediately (devel only)
        private void button1_Click( object sender, EventArgs e ) {
            Settings.NamedResultsSettings = new NamedSettings[ 0 ];
        }
#endregion

        #region Summary Reports

        private void SaveSummaryReport( SummaryReportType reportType ) {
            if( simBox.CheckedItems.Count == 0 ) {
                MessageBox.Show( "\r\n         Error: You must select at least one simulation.     \r\n" );
                return;
            }

            hiddenProductTree.Rebuild();

            string simName = null;
            string[] runNames = new string[ simBox.CheckedItems.Count ];
            int[] runIDs = new int[ simBox.CheckedItems.Count ];
            for( int j = 0; j < simBox.CheckedItems.Count; j++ ) {
                DataRowView rowView = (DataRowView)simBox.CheckedItems[ j ];
                string srnam = (string)rowView.Row[ 0 ];
                ArrayList rids = (ArrayList)rowView.Row[ 1 ];
                runIDs[ j ] = (int)rids[ 0 ];

                if( srnam.IndexOf( "-" ) != -1 ) {    // should always be true
                    runNames[ j ] = srnam.Substring( srnam.LastIndexOf( "-" ) + 1 ).Trim();
                    simName = srnam.Substring( 0, srnam.LastIndexOf( "-" ) ).Trim();

                    if( runNames[ j ].StartsWith( "run " ) ) {
                        runNames[ j ] = runNames[ j ].Substring( 4 );      // trim all but run number
                    }
                }
                else {
                    // just in case!
                    runNames[ j ] = String.Format( "Selection {0}", j + 1 );
                    simName = srnam;
                }
            }

            string reportName = "?";
            bool[] colsToNormalize = null;
            bool[] prodOnlyCols = null;
            if( reportType == SummaryReportType.CalibrationMaster ) {
                reportName = "Calibration Master";
                colsToNormalize = new bool[] { false, false, false, true, true, false, false, false, false, false, false, false, false, false };
                prodOnlyCols = new bool[] { false, false, true, false, false, false, false, false, false, false, false, false, false, false };
            }
            else if( reportType == SummaryReportType.FWReport ) {
                reportName = "F-W Report";
                colsToNormalize = new bool[] { false, false, true, false, false, false, false, false, false, false, false };
                prodOnlyCols = new bool[] { false, false, false, false, false, false, false, false, false, false, false };
            }
            else if( reportType == SummaryReportType.TrialReport ) {
                reportName = "Trial Report";
                colsToNormalize = new bool[] { false, false, true, true, true, false, true, false, false, false, false };
                prodOnlyCols = new bool[] { false, false, false, false, false, false, false, false, false, false, false };
            }

            bool templateOK = VerifyTemplateExists( reportType );
            if( templateOK == false ) {
                MessageBox.Show( "\r\n\r\n    Error: Template file not found: " + reportName + "    \r\n\r\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.Filter = "Excel Files (*.xls)|*.xls";
            saveDlg.Title = String.Format( "Save {0} Report File", reportName );

            DialogResult resp = saveDlg.ShowDialog();

            if( resp == DialogResult.OK ) {

                string productTypeString = GetProductTypeString();
                string brandTypeString = GetBrandTypeString();

                ArrayList perProductData = new ArrayList();
                ArrayList perBrandData = new ArrayList();

                // compute the values we can get using the time-series-based approach used for the Consumer Panel
                if( productLevelRadioButton.Checked == true || bothLevelsRadioButton.Checked == true ) {
                    // get the per-product data
                    MultiGrapher hiddenGraph = GraphNoShow( startEndDate1.Start, startEndDate1.End, reportType, false );

                    if( hiddenGraph != null ) {
                        hiddenGraph.write( perProductData );      // write the summary data values into the given array list
                    }
                }

                if( brandLevelRadioButton.Checked == true || bothLevelsRadioButton.Checked == true ) {
                    // get the per-brand data
                    MultiGrapher hiddenGraph = GraphNoShow( startEndDate1.Start, startEndDate1.End, reportType, true );

                    if( hiddenGraph != null ) {
                        hiddenGraph.write( perBrandData );      // write the summary data values into the given array list
                    }
                }

                // get the items for a calibration master that aren't part of the regular metrics
                if( reportType == SummaryReportType.CalibrationMaster ) {
                    RecomputeMAPE();

                    // get the list of all items to possibly generate, with products under their brands
                    hiddenProductTree.SelectByType( brandTypeString );
                    ArrayList brandsAndProds = hiddenProductTree.CheckedProducts;
                    MetricMan metricMan = new MetricMan( theDb );

                    // replicate the real share data, which shows up only once
                    if( runIDs.Length > 1 ) {
                        int repIndx = (2 * runIDs.Length) + 1;
                        for( int rrr = 1; rrr < runIDs.Length; rrr++ ) {
                            if( perProductData.Count > 0 ) {
                                for( int pp = 0; pp < perProductData.Count; pp++ ) {
                                    ((ArrayList)perProductData[ pp ]).Insert( repIndx + 1, ((ArrayList)perProductData[ pp ])[ repIndx ] );
                                }
                            }
                            if( perBrandData.Count > 0 ) {
                                for( int pb = 0; pb < perBrandData.Count; pb++ ) {
                                    ((ArrayList)perBrandData[ pb ]).Insert( repIndx + 1, ((ArrayList)perBrandData[ pb ])[ repIndx ] );
                                }
                            }
                        }
                    }

                    for( int rr = 0; rr < runIDs.Length; rr++ ) {
                        int brandIndx = 0;
                        int prodIndx = 0;
                        for( int i = 0; i < brandsAndProds.Count; i++ ) {
                            MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                            MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                            if( typerow.type_name == productTypeString || typerow.type_name == brandTypeString ) {
                                double mape = GetMAPE( metricMan, runIDs[ rr ], row.product_id );
                                double tau = 0;
                                if( typerow.type_name == productTypeString ) {
                                    tau = GetNimoTau( metricMan, runIDs[ rr ], row.product_id );
                                }

                                double salesRate = GetNimoSalesRate( metricMan, runIDs[ rr ], row.product_id );
                                double realSalesRate = GetNimoRealSalesRate( metricMan, runIDs[ rr ], row.product_id );
                                double salesRateR2 = GetNimoSalesRateR2( metricMan, runIDs[ rr ], row.product_id );
                                double realSalesRateR2 = GetNimoRealSalesRateR2( metricMan, runIDs[ rr ], row.product_id );

                                Console.WriteLine( " MAPE = {0:f2} tau = {1:f2} for run_id = {2}, product = {3}", mape, tau, runIDs[ rr ], row.product_id );
                                Console.WriteLine( " Sales Rate = {0:f2} Real Sales Rate = {1:f2} Sales Rate R2 = {2:f2}, Real Sales Rate R2 = {3:f2}",
                                    salesRate, realSalesRate, salesRateR2, realSalesRateR2 );

                                ArrayList vals1 = null;
                                if( typerow.type_name == productTypeString && perProductData.Count > 0 ) {
                                    vals1 = (ArrayList)perProductData[ prodIndx ];
                                    prodIndx += 1;
                                }
                                else if( typerow.type_name == brandTypeString && perBrandData.Count > 0 ) {
                                    vals1 = (ArrayList)perBrandData[ brandIndx ];
                                    brandIndx += 1;
                                }

                                if( vals1 != null ) {
                                    int tau_col = rr + 1;  // col 1
                                    int mape_col = (3 * runIDs.Length) + rr + 1;         // col 4   ((col-1)*numRuns) * runNum + 1 is formula for col
                                    int rate_col = (4 * runIDs.Length) + rr + 1;           // col 5
                                    int real_rate_col = (5 * runIDs.Length) + rr + 1;           // col 6
                                    int rate_r2_col = (6 * runIDs.Length) + rr + 1;           // col 7
                                    int real_rate_r2_col = (7 * runIDs.Length) + rr + 1;           // col 8
                                    vals1[ tau_col ] = tau;
                                    vals1[ mape_col ] = mape;
                                    vals1[ rate_col ] = salesRate;
                                    vals1[ real_rate_col ] = realSalesRate;
                                    vals1[ rate_r2_col ] = salesRateR2;
                                    vals1[ real_rate_r2_col ] = realSalesRateR2;
                                }
                            }
                        }
                    }
                }   // end of calibration master report process

                // get the items for a trial report that aren't part of the regular metrics
                if( reportType == SummaryReportType.TrialReport ) {

                    // get the list of all items to possibly generate, with products under their brands
                    hiddenProductTree.SelectByType( "Brand" );
                    ArrayList brandsAndProds = hiddenProductTree.CheckedProducts;
                    MetricMan metricMan = new MetricMan( theDb );

                    for( int rr = 0; rr < runIDs.Length; rr++ ) {
                        int brandIndx = 0;
                        int prodIndx = 0;
                        for( int i = 0; i < brandsAndProds.Count; i++ ) {
                            MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                            MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                            if( typerow.type_name == productTypeString || typerow.type_name == brandTypeString ) {
                                double avgDistribution = GetAvgDistribution( metricMan, runIDs[ rr ], row.product_id );
                                double avgAwareness = GetAvgAwareness( metricMan, runIDs[ rr ], row.product_id );
                                double avgACVDisplay = GetAvgACVDisplay( metricMan, runIDs[ rr ], row.product_id );
                                double basePrice = GetNimoBasePriceAbs( metricMan, runIDs[ rr ], row.product_id );
                                double avgPrice = GetNimoAvgPriceAbs( metricMan, runIDs[ rr ], row.product_id );

                                ArrayList vals1 = null;
                                if( typerow.type_name == productTypeString && perProductData.Count > 0 ) {
                                    vals1 = (ArrayList)perProductData[ prodIndx ];
                                    prodIndx += 1;
                                }
                                else if( typerow.type_name == brandTypeString && perBrandData.Count > 0 ) {
                                    vals1 = (ArrayList)perBrandData[ brandIndx ];
                                    brandIndx += 1;
                                }

                                if( vals1 != null ) {
                                    int base_pr_col = (7 * runIDs.Length) + rr + 1;         // col 8   ((col-1)*numRuns) * runNum + 1 is formula for col
                                    int avg_pr_col = (6 * runIDs.Length) + rr + 1;           // col 7
                                    int avg_dist_col = (1 * runIDs.Length) + rr + 1;           // col 2
                                    int avg_aware_col = (2 * runIDs.Length) + rr + 1;           // col 3
                                    int avg_display_col = (4 * runIDs.Length) + rr + 1;           // col 5
                                    vals1[ base_pr_col ] = basePrice;
                                    vals1[ avg_pr_col ] = avgPrice;
                                    vals1[ avg_dist_col ] = avgDistribution;
                                    vals1[ avg_aware_col ] = avgAwareness;
                                    vals1[ avg_display_col ] = avgACVDisplay;
                                }
                            }
                        }
                    }
                } // end of trial report processing

                string dateRangeStr = String.Format( "{0} - {1}", startEndDate1.Start.ToShortDateString(), startEndDate1.End.ToShortDateString() );
                int numAgents = theDb.Model.pop_size;

                // now we have all of the data -- create the output file
                string errorDetails = "";
                bool ok = WriteSummaryFile( saveDlg.FileName, reportType, perProductData, perBrandData, hiddenProductTree,
                    simName, runNames, dateRangeStr, numAgents, colsToNormalize, prodOnlyCols, GetBrandTypeString(), out errorDetails );

                if( ok ) {
                    string okMsg = String.Format( "{0} Written Successfully.", reportName );
                    CompletionDialog conf = new CompletionDialog( okMsg, true );
                    conf.ShowDialog();
                }
                else {
                    string okMsg = String.Format( "Error:  {0} Not Created.\r\n{1}", reportName, errorDetails );
                    ConfirmDialog conf = new ConfirmDialog( okMsg, "Error" );
                    conf.SelectErrorIcon();
                    conf.SetOkButtonOnlyStyle();
                    conf.Width += 75;
                    conf.Height += 25;
                    conf.ShowDialog();
                }
            }
        }

        private string GetProductTypeString() {
            MrktSimDBSchema.product_typeRow[] rows = theDb.Model.Getproduct_typeRows();
            if( rows != null && rows.Length > 1 ) {
                return rows[ rows.Length - 1 ].type_name;
            }
            else {
                // default
                return "Product";
            }
        }

        private string GetBrandTypeString() {
            MrktSimDBSchema.product_typeRow[] rows = theDb.Model.Getproduct_typeRows();
            if( rows != null && rows.Length > 1 ) {
                return rows[ rows.Length - 2 ].type_name;
            }
            else {
                // default
                return "Brand";
            }
        }

        private void RecomputeMAPE() {

            Metric[] mapeMetrics = new Metric[] { MetricMan.GetMetric( "SimSummaryByProd" ) };
            Metric[] mapeMetrics2 = new Metric[] { MetricMan.GetMetric( "CalSummaryByProd" ) };

            RecomputeMetrics( mapeMetrics );
            RecomputeMetrics( mapeMetrics2 );
        }

        private double GetMAPE( MetricMan metricMan, int runID, int productID ) {

            Value mape = MetricMan.GetValue( "MAPE" );

            mape.Run = runID;
            mape.Product = productID;

            return metricMan.Evaluate( mape );
        }

        private double GetNimoTau( MetricMan metricMan, int runID, int productID ) {

            Value nimoTau = MetricMan.GetValue( "NIMOTau" );

            nimoTau.Run = runID;
            nimoTau.Product = productID;

            return metricMan.Evaluate( nimoTau );            
        }

        private double GetNimoSalesRate( MetricMan metricMan, int runID, int productID ) {

            Value nimoSalesRate = MetricMan.GetValue( "SalesRate" ); 

            nimoSalesRate.Run = runID;
            nimoSalesRate.Product = productID;

            return metricMan.Evaluate( nimoSalesRate );
        }

        private double GetNimoSalesRateR2( MetricMan metricMan, int runID, int productID ) {

            Value nimoSalesRateR2 = MetricMan.GetValue( "SalesRateR2" );

            nimoSalesRateR2.Run = runID;
            nimoSalesRateR2.Product = productID;

            return metricMan.Evaluate( nimoSalesRateR2 );
        }

        private double GetNimoRealSalesRate( MetricMan metricMan, int runID, int productID ) {

            Value nimoRealSalesRate = MetricMan.GetValue( "RealSalesRate" );

            nimoRealSalesRate.Run = runID;
            nimoRealSalesRate.Product = productID;

            return metricMan.Evaluate( nimoRealSalesRate );
        }

        private double GetNimoRealSalesRateR2( MetricMan metricMan, int runID, int productID ) {

            Value nimoRealSalesRateR2 = MetricMan.GetValue( "RealSalesRateR2" );

            nimoRealSalesRateR2.Run = runID;
            nimoRealSalesRateR2.Product = productID;

            return metricMan.Evaluate( nimoRealSalesRateR2 );
        }

        private double GetNimoBasePriceAbs( MetricMan metricMan, int runID, int productID ) {

            Value nimoBasePriceAbs = MetricMan.GetValue( "BasePrice" );

            nimoBasePriceAbs.Run = runID;
            nimoBasePriceAbs.Product = productID;

            return metricMan.Evaluate( nimoBasePriceAbs );
        }

        private double GetNimoAvgPriceAbs( MetricMan metricMan, int runID, int productID ) {

            Value nimoAvgPriceAbs = MetricMan.GetValue( "AvgPrice" );

            nimoAvgPriceAbs.Run = runID;
            nimoAvgPriceAbs.Product = productID;

            return metricMan.Evaluate( nimoAvgPriceAbs );
        }

        private double GetAvgDistribution( MetricMan metricMan, int runID, int productID ) {

            Value avgDistribution = MetricMan.GetValue( "AvgDistribution" );

            avgDistribution.Run = runID;
            avgDistribution.Product = productID;

            return metricMan.Evaluate( avgDistribution );
        }

        private double GetAvgAwareness( MetricMan metricMan, int runID, int productID ) {

            Value avgAwareness = MetricMan.GetValue( "AvgAwareness" );

            avgAwareness.Run = runID;
            avgAwareness.Product = productID;

            return metricMan.Evaluate( avgAwareness );
        }

        private double GetAvgACVDisplay( MetricMan metricMan, int runID, int productID ) {

            Value avgACVDisplay = MetricMan.GetValue( "AvgACVDisplay" );

            avgACVDisplay.Run = runID;
            avgACVDisplay.Product = productID;

            return metricMan.Evaluate( avgACVDisplay );
        }

        private void fwReportButton_Click( object sender, EventArgs e ) {
            SaveSummaryReport( SummaryReportType.FWReport );
        }

        private void trialReportButton_Click( object sender, EventArgs e ) {
            SaveSummaryReport( SummaryReportType.TrialReport );
        }

        private void calMasterButton_Click( object sender, EventArgs e ) {
            SaveSummaryReport( SummaryReportType.CalibrationMaster );
        }

        private bool VerifyTemplateExists( SummaryReportType reportType ) {
            return SummaryReportGenerator.VerifyTemplateExists( reportType );
        }

        private bool WriteSummaryFile( string fileName, SummaryReportType reportType, ArrayList perProductData, ArrayList perBrandData, ProductTree productTree,
            string simName, string[] runStrs, string dateRange, int popSize, bool[] normalizeCol, bool[] prodOnlyCol, string brandTypeString, out string errorDetails ) {
            return SummaryReportGenerator.WriteSummaryReport( fileName, reportType, perProductData, perBrandData, productTree, 
                                                                                            simName, runStrs, dateRange, popSize, normalizeCol, prodOnlyCol, brandTypeString, out errorDetails );
        }

        #endregion
    }
}
