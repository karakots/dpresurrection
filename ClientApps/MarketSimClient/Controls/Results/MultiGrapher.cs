using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Utilities.Graphing;

namespace Results
{
	/// <summary>
	/// Summary description for MultiGrapher.
	/// </summary>
	public class MultiGrapher : System.Windows.Forms.Form
	{
		private bool timeSeries = true;
		private System.Windows.Forms.Panel datePanel;
		private System.Windows.Forms.Panel numericalPanel;
		private System.Windows.Forms.NumericUpDown minX;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown maxX;
		private System.Windows.Forms.ComboBox symbolBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel preview;
		private System.Windows.Forms.MenuItem applyAll;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem imageItem;
		private System.Windows.Forms.MenuItem excelItem;
		private System.Windows.Forms.MenuItem csvItem;
		private System.Windows.Forms.Panel graphControlPanel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker endDate;
		private System.Windows.Forms.DateTimePicker startDate;
		private System.Windows.Forms.TextBox yAxisBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown minY;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown maxY;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.CheckBox fill;
		private System.Windows.Forms.ListBox curveSelect;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuItem noScale;
		private System.Windows.Forms.MenuItem plotMenu;
		private System.Windows.Forms.NumericUpDown minPercent;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown maxPercent;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown curveScale;
        private System.Windows.Forms.Label label13;
        private Button saveButton;
        private Label settingsNameLabel;
        private Label label14;
        private Button saveAsButton;
        private IContainer components;
        private Button moveDownButton;
        private Button moveUpButton;
        private TrackBar transparencyTrackBar;
        private Panel panel2;
        private NumericUpDown widthUpDown;
        private ComboBox styleBox;
        private Label label15;
        private Panel panel3;

        public delegate void HandleUpdatedNamedSettings( NamedSettings newSettingsItem );

        public event HandleUpdatedNamedSettings NewNamedSettingsAdded;
        public event HandleUpdatedNamedSettings NamedSettingsSaved;
        private bool legendsVisible = true;

        private NamedSettings namedSettings;

		public MultiGrapher( NamedSettings namedSettings )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.namedSettings = namedSettings;

            if( namedSettings != null ) {
                settingsNameLabel.Text = namedSettings.SettingsName;
            }
            else {
                settingsNameLabel.Text = "<none>";
            }

			minY.Value = 0;
            minY.Minimum = decimal.MinValue;
            minY.Maximum = decimal.MaxValue;

			maxY.Value = 1;
            maxY.Minimum = decimal.MinValue;
            maxY.Maximum = decimal.MaxValue;

            this.minPercent.Minimum = decimal.MinValue;
            this.minPercent.Maximum = decimal.MaxValue;
			this.minPercent.Value = 0;

            this.maxPercent.Minimum = decimal.MinValue;
            this.maxPercent.Maximum = decimal.MaxValue;
			this.maxPercent.Value = 100;

			this.numericalPanel.Visible = false;

			symbolBox.DataSource = symbols;
            styleBox.DataSource = dashStyles;

			fill.Checked = false;

			//			minY.DataBindings.Add("Maximum", maxY, "Value");
			//			maxY.DataBindings.Add("Minimum", minY, "Value");
			//
			//			startDate.DataBindings.Add("MaxDate", endDate, "Value");
			//			endDate.DataBindings.Add("MinDate", startDate, "Value");

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MultiGrapher ) );
            this.mainMenu = new System.Windows.Forms.MainMenu( this.components );
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.imageItem = new System.Windows.Forms.MenuItem();
            this.excelItem = new System.Windows.Forms.MenuItem();
            this.csvItem = new System.Windows.Forms.MenuItem();
            this.applyAll = new System.Windows.Forms.MenuItem();
            this.noScale = new System.Windows.Forms.MenuItem();
            this.plotMenu = new System.Windows.Forms.MenuItem();
            this.graphControlPanel = new System.Windows.Forms.Panel();
            this.curveSelect = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.curveScale = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.styleBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.symbolBox = new System.Windows.Forms.ComboBox();
            this.transparencyTrackBar = new System.Windows.Forms.TrackBar();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.minPercent = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.maxPercent = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericalPanel = new System.Windows.Forms.Panel();
            this.maxX = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.minX = new System.Windows.Forms.NumericUpDown();
            this.minY = new System.Windows.Forms.NumericUpDown();
            this.yAxisBox = new System.Windows.Forms.TextBox();
            this.fill = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.colorButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.maxY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.datePanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.settingsNameLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveAsButton = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.preview = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.graphControlPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curveScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transparencyTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPercent)).BeginInit();
            this.numericalPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxY)).BeginInit();
            this.datePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.plotMenu} );
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.imageItem,
            this.excelItem,
            this.applyAll,
            this.noScale} );
            this.menuItem1.Text = "Graph";
            // 
            // imageItem
            // 
            this.imageItem.Index = 0;
            this.imageItem.Text = "Save Image";
            this.imageItem.Click += new System.EventHandler( this.imageItem_Click );
            // 
            // excelItem
            // 
            this.excelItem.Index = 1;
            this.excelItem.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.csvItem} );
            this.excelItem.Text = "Export";
            // 
            // csvItem
            // 
            this.csvItem.Index = 0;
            this.csvItem.Text = "comma delimited format (csv)";
            this.csvItem.Click += new System.EventHandler( this.csvItem_Click );
            // 
            // applyAll
            // 
            this.applyAll.Index = 2;
            this.applyAll.Text = "Allign Charts";
            this.applyAll.Click += new System.EventHandler( this.applyAll_CheckedChanged );
            // 
            // noScale
            // 
            this.noScale.Checked = true;
            this.noScale.Index = 3;
            this.noScale.Text = "auto scaling";
            this.noScale.Click += new System.EventHandler( this.noScale_Click );
            // 
            // plotMenu
            // 
            this.plotMenu.Index = 1;
            this.plotMenu.Text = "Plot Select";
            // 
            // graphControlPanel
            // 
            this.graphControlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.graphControlPanel.Controls.Add( this.curveSelect );
            this.graphControlPanel.Controls.Add( this.panel1 );
            this.graphControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.graphControlPanel.Location = new System.Drawing.Point( 0, 281 );
            this.graphControlPanel.Name = "graphControlPanel";
            this.graphControlPanel.Size = new System.Drawing.Size( 848, 136 );
            this.graphControlPanel.TabIndex = 1;
            // 
            // curveSelect
            // 
            this.curveSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curveSelect.Location = new System.Drawing.Point( 552, 0 );
            this.curveSelect.Name = "curveSelect";
            this.curveSelect.Size = new System.Drawing.Size( 296, 134 );
            this.curveSelect.TabIndex = 29;
            this.curveSelect.SelectedIndexChanged += new System.EventHandler( this.curveSelect_SelectedIndexChanged );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.panel3 );
            this.panel1.Controls.Add( this.widthUpDown );
            this.panel1.Controls.Add( this.styleBox );
            this.panel1.Controls.Add( this.label15 );
            this.panel1.Controls.Add( this.symbolBox );
            this.panel1.Controls.Add( this.transparencyTrackBar );
            this.panel1.Controls.Add( this.moveDownButton );
            this.panel1.Controls.Add( this.moveUpButton );
            this.panel1.Controls.Add( this.minPercent );
            this.panel1.Controls.Add( this.label10 );
            this.panel1.Controls.Add( this.maxPercent );
            this.panel1.Controls.Add( this.label11 );
            this.panel1.Controls.Add( this.numericalPanel );
            this.panel1.Controls.Add( this.minY );
            this.panel1.Controls.Add( this.yAxisBox );
            this.panel1.Controls.Add( this.fill );
            this.panel1.Controls.Add( this.label9 );
            this.panel1.Controls.Add( this.colorButton );
            this.panel1.Controls.Add( this.label5 );
            this.panel1.Controls.Add( this.maxY );
            this.panel1.Controls.Add( this.label2 );
            this.panel1.Controls.Add( this.label8 );
            this.panel1.Controls.Add( this.datePanel );
            this.panel1.Controls.Add( this.panel2 );
            this.panel1.Controls.Add( this.label1 );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 552, 136 );
            this.panel1.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(212)))), ((int)(((byte)(222)))), ((int)(((byte)(255)))) );
            this.panel3.Controls.Add( this.curveScale );
            this.panel3.Controls.Add( this.label12 );
            this.panel3.Controls.Add( this.label13 );
            this.panel3.Location = new System.Drawing.Point( 432, -7 );
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size( 140, 34 );
            this.panel3.TabIndex = 50;
            // 
            // curveScale
            // 
            this.curveScale.Location = new System.Drawing.Point( 43, 10 );
            this.curveScale.Maximum = new decimal( new int[] {
            9,
            0,
            0,
            0} );
            this.curveScale.Minimum = new decimal( new int[] {
            9,
            0,
            0,
            -2147483648} );
            this.curveScale.Name = "curveScale";
            this.curveScale.Size = new System.Drawing.Size( 40, 20 );
            this.curveScale.TabIndex = 37;
            this.curveScale.ThousandsSeparator = true;
            this.curveScale.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.curveScale.ValueChanged += new System.EventHandler( this.curveScale_ValueChanged );
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point( 5, 12 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size( 48, 16 );
            this.label12.TabIndex = 36;
            this.label12.Text = "Scale";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label13.Location = new System.Drawing.Point( 85, 12 );
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size( 32, 16 );
            this.label13.TabIndex = 38;
            this.label13.Text = "x 10";
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point( 413, 36 );
            this.widthUpDown.Maximum = new decimal( new int[] {
            9,
            0,
            0,
            0} );
            this.widthUpDown.Minimum = new decimal( new int[] {
            9,
            0,
            0,
            -2147483648} );
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size( 40, 20 );
            this.widthUpDown.TabIndex = 49;
            this.widthUpDown.ThousandsSeparator = true;
            this.widthUpDown.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.widthUpDown.ValueChanged += new System.EventHandler( this.widthUpDown_ValueChanged );
            // 
            // styleBox
            // 
            this.styleBox.Location = new System.Drawing.Point( 421, 60 );
            this.styleBox.Name = "styleBox";
            this.styleBox.Size = new System.Drawing.Size( 88, 21 );
            this.styleBox.TabIndex = 47;
            this.styleBox.SelectedIndexChanged += new System.EventHandler( this.styleBox_SelectedIndexChanged );
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point( 374, 65 );
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size( 48, 16 );
            this.label15.TabIndex = 48;
            this.label15.Text = "Style";
            // 
            // symbolBox
            // 
            this.symbolBox.Location = new System.Drawing.Point( 421, 85 );
            this.symbolBox.Name = "symbolBox";
            this.symbolBox.Size = new System.Drawing.Size( 88, 21 );
            this.symbolBox.TabIndex = 25;
            this.symbolBox.SelectedIndexChanged += new System.EventHandler( this.symbolBox_SelectedIndexChanged );
            // 
            // transparencyTrackBar
            // 
            this.transparencyTrackBar.Location = new System.Drawing.Point( 428, 108 );
            this.transparencyTrackBar.Maximum = 255;
            this.transparencyTrackBar.Name = "transparencyTrackBar";
            this.transparencyTrackBar.Size = new System.Drawing.Size( 86, 34 );
            this.transparencyTrackBar.TabIndex = 45;
            this.transparencyTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.transparencyTrackBar.Scroll += new System.EventHandler( this.transparencyTrackBar_Scroll );
            // 
            // moveDownButton
            // 
            this.moveDownButton.Font = new System.Drawing.Font( "Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)) );
            this.moveDownButton.Location = new System.Drawing.Point( 525, 68 );
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size( 26, 24 );
            this.moveDownButton.TabIndex = 44;
            this.moveDownButton.Text = "Ñ";
            this.moveDownButton.Click += new System.EventHandler( this.moveDownButton_Click );
            // 
            // moveUpButton
            // 
            this.moveUpButton.Font = new System.Drawing.Font( "Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)) );
            this.moveUpButton.Location = new System.Drawing.Point( 525, 44 );
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size( 26, 24 );
            this.moveUpButton.TabIndex = 43;
            this.moveUpButton.Text = "D";
            this.moveUpButton.Click += new System.EventHandler( this.moveUpButton_Click );
            // 
            // minPercent
            // 
            this.minPercent.DecimalPlaces = 2;
            this.minPercent.Location = new System.Drawing.Point( 71, 59 );
            this.minPercent.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.minPercent.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
            this.minPercent.Name = "minPercent";
            this.minPercent.Size = new System.Drawing.Size( 96, 20 );
            this.minPercent.TabIndex = 32;
            this.minPercent.ThousandsSeparator = true;
            this.minPercent.ValueChanged += new System.EventHandler( this.minPercent_ValueChanged );
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point( 206, 62 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size( 40, 16 );
            this.label10.TabIndex = 35;
            this.label10.Text = "Max %";
            // 
            // maxPercent
            // 
            this.maxPercent.DecimalPlaces = 2;
            this.maxPercent.Location = new System.Drawing.Point( 246, 60 );
            this.maxPercent.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.maxPercent.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
            this.maxPercent.Name = "maxPercent";
            this.maxPercent.Size = new System.Drawing.Size( 96, 20 );
            this.maxPercent.TabIndex = 34;
            this.maxPercent.ThousandsSeparator = true;
            this.maxPercent.ValueChanged += new System.EventHandler( this.maxPercent_ValueChanged );
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point( 23, 62 );
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size( 40, 16 );
            this.label11.TabIndex = 33;
            this.label11.Text = "Min %";
            // 
            // numericalPanel
            // 
            this.numericalPanel.Controls.Add( this.maxX );
            this.numericalPanel.Controls.Add( this.label7 );
            this.numericalPanel.Controls.Add( this.label6 );
            this.numericalPanel.Controls.Add( this.minX );
            this.numericalPanel.Location = new System.Drawing.Point( 7, 2 );
            this.numericalPanel.Name = "numericalPanel";
            this.numericalPanel.Size = new System.Drawing.Size( 344, 32 );
            this.numericalPanel.TabIndex = 24;
            // 
            // maxX
            // 
            this.maxX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxX.DecimalPlaces = 2;
            this.maxX.Location = new System.Drawing.Point( 239, 6 );
            this.maxX.Maximum = new decimal( new int[] {
            1410065407,
            2,
            0,
            0} );
            this.maxX.Minimum = new decimal( new int[] {
            1410065407,
            2,
            0,
            -2147483648} );
            this.maxX.Name = "maxX";
            this.maxX.Size = new System.Drawing.Size( 96, 20 );
            this.maxX.TabIndex = 3;
            this.maxX.ValueChanged += new System.EventHandler( this.maxX_ValueChanged );
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point( 200, 8 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 27, 23 );
            this.label7.TabIndex = 2;
            this.label7.Text = "End";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point( 24, 8 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 32, 23 );
            this.label6.TabIndex = 1;
            this.label6.Text = "Start";
            // 
            // minX
            // 
            this.minX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minX.DecimalPlaces = 2;
            this.minX.Location = new System.Drawing.Point( 64, 6 );
            this.minX.Maximum = new decimal( new int[] {
            1410065407,
            2,
            0,
            0} );
            this.minX.Minimum = new decimal( new int[] {
            1410065407,
            2,
            0,
            -2147483648} );
            this.minX.Name = "minX";
            this.minX.Size = new System.Drawing.Size( 96, 20 );
            this.minX.TabIndex = 0;
            this.minX.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.minX.ValueChanged += new System.EventHandler( this.minX_ValueChanged );
            // 
            // minY
            // 
            this.minY.DecimalPlaces = 2;
            this.minY.Location = new System.Drawing.Point( 71, 36 );
            this.minY.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.minY.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
            this.minY.Name = "minY";
            this.minY.Size = new System.Drawing.Size( 96, 20 );
            this.minY.TabIndex = 17;
            this.minY.ThousandsSeparator = true;
            this.minY.ValueChanged += new System.EventHandler( this.minY_ValueChanged );
            // 
            // yAxisBox
            // 
            this.yAxisBox.Location = new System.Drawing.Point( 71, 84 );
            this.yAxisBox.Name = "yAxisBox";
            this.yAxisBox.Size = new System.Drawing.Size( 272, 20 );
            this.yAxisBox.TabIndex = 11;
            this.yAxisBox.TextChanged += new System.EventHandler( this.yAxisBox_TextChanged );
            // 
            // fill
            // 
            this.fill.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fill.Location = new System.Drawing.Point( 375, 109 );
            this.fill.Name = "fill";
            this.fill.Size = new System.Drawing.Size( 48, 24 );
            this.fill.TabIndex = 28;
            this.fill.Text = "Fill";
            this.fill.CheckedChanged += new System.EventHandler( this.fill_CheckedChanged );
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point( 374, 40 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 40, 16 );
            this.label9.TabIndex = 30;
            this.label9.Text = "Width";
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point( 467, 34 );
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size( 42, 23 );
            this.colorButton.TabIndex = 22;
            this.colorButton.Text = "Color";
            this.colorButton.Click += new System.EventHandler( this.colorButton_Click );
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 206, 38 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 40, 16 );
            this.label5.TabIndex = 20;
            this.label5.Text = "Max Y";
            // 
            // maxY
            // 
            this.maxY.DecimalPlaces = 2;
            this.maxY.Location = new System.Drawing.Point( 246, 35 );
            this.maxY.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.maxY.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
            this.maxY.Name = "maxY";
            this.maxY.Size = new System.Drawing.Size( 96, 20 );
            this.maxY.TabIndex = 19;
            this.maxY.ThousandsSeparator = true;
            this.maxY.ValueChanged += new System.EventHandler( this.maxY_ValueChanged );
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 23, 38 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 40, 16 );
            this.label2.TabIndex = 18;
            this.label2.Text = "Min Y";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point( 374, 90 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 48, 16 );
            this.label8.TabIndex = 26;
            this.label8.Text = "Symbol";
            // 
            // datePanel
            // 
            this.datePanel.Controls.Add( this.label3 );
            this.datePanel.Controls.Add( this.startDate );
            this.datePanel.Controls.Add( this.label4 );
            this.datePanel.Controls.Add( this.endDate );
            this.datePanel.Location = new System.Drawing.Point( 7, 1 );
            this.datePanel.Name = "datePanel";
            this.datePanel.Size = new System.Drawing.Size( 362, 32 );
            this.datePanel.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 8, 8 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 56, 16 );
            this.label3.TabIndex = 14;
            this.label3.Text = "Start Date";
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point( 64, 5 );
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size( 96, 20 );
            this.startDate.TabIndex = 12;
            this.startDate.ValueChanged += new System.EventHandler( this.startDate_ValueChanged );
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 184, 8 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 56, 16 );
            this.label4.TabIndex = 15;
            this.label4.Text = "End Date";
            // 
            // endDate
            // 
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDate.Location = new System.Drawing.Point( 240, 5 );
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size( 96, 20 );
            this.endDate.TabIndex = 13;
            this.endDate.ValueChanged += new System.EventHandler( this.endDate_ValueChanged );
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(212)))), ((int)(((byte)(222)))), ((int)(((byte)(255)))) );
            this.panel2.Controls.Add( this.settingsNameLabel );
            this.panel2.Controls.Add( this.saveButton );
            this.panel2.Controls.Add( this.saveAsButton );
            this.panel2.Controls.Add( this.label14 );
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point( -4, 107 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 373, 43 );
            this.panel2.TabIndex = 46;
            // 
            // settingsNameLabel
            // 
            this.settingsNameLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.settingsNameLabel.Location = new System.Drawing.Point( 66, 7 );
            this.settingsNameLabel.Name = "settingsNameLabel";
            this.settingsNameLabel.Size = new System.Drawing.Size( 162, 23 );
            this.settingsNameLabel.TabIndex = 40;
            this.settingsNameLabel.Text = "Settings Name";
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))) );
            this.saveButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.saveButton.ForeColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))) );
            this.saveButton.Location = new System.Drawing.Point( 237, 3 );
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size( 40, 22 );
            this.saveButton.TabIndex = 41;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
            // 
            // saveAsButton
            // 
            this.saveAsButton.BackColor = System.Drawing.SystemColors.Control;
            this.saveAsButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.saveAsButton.Location = new System.Drawing.Point( 283, 3 );
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size( 76, 22 );
            this.saveAsButton.TabIndex = 42;
            this.saveAsButton.Text = "Save As...";
            this.saveAsButton.UseVisualStyleBackColor = false;
            this.saveAsButton.Click += new System.EventHandler( this.saveAsButton_Click );
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label14.Location = new System.Drawing.Point( 11, 7 );
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size( 72, 23 );
            this.label14.TabIndex = 39;
            this.label14.Text = "Settings:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 4, 86 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 72, 23 );
            this.label1.TabIndex = 16;
            this.label1.Text = "Y-Axis Label";
            // 
            // preview
            // 
            this.preview.AutoScroll = true;
            this.preview.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.preview.Dock = System.Windows.Forms.DockStyle.Left;
            this.preview.Location = new System.Drawing.Point( 0, 0 );
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size( 200, 281 );
            this.preview.TabIndex = 3;
            this.preview.Resize += new System.EventHandler( this.preview_Resize );
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point( 200, 0 );
            this.splitter1.MinExtra = 200;
            this.splitter1.MinSize = 200;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 3, 281 );
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // MultiGrapher
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size( 848, 417 );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.preview );
            this.Controls.Add( this.graphControlPanel );
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size( 600, 400 );
            this.Name = "MultiGrapher";
            this.Text = "Plot";
            this.Load += new System.EventHandler( this.MultiGrapher_Load );
            this.MdiChildActivate += new System.EventHandler( this.MultiGrapher_MdiChildActivate );
            this.Resize += new System.EventHandler( this.MultiGrapher_Resize );
            this.graphControlPanel.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.curveScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transparencyTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPercent)).EndInit();
            this.numericalPanel.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.maxX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxY)).EndInit();
            this.datePanel.ResumeLayout( false );
            this.panel2.ResumeLayout( false );
            this.ResumeLayout( false );

		}
		#endregion

		static private bool doNotUpdate = false;
        public static ZedGraph.SymbolType[] symbols = new ZedGraph.SymbolType[] 
		{
			ZedGraph.SymbolType.Default,
			ZedGraph.SymbolType.Star,
			ZedGraph.SymbolType.Circle,
			ZedGraph.SymbolType.Plus,
			ZedGraph.SymbolType.None
		};

        public static System.Drawing.Drawing2D.DashStyle[] dashStyles = new System.Drawing.Drawing2D.DashStyle[] 
		{
			System.Drawing.Drawing2D.DashStyle.Solid,
			System.Drawing.Drawing2D.DashStyle.Dash,
			System.Drawing.Drawing2D.DashStyle.Dot,
			System.Drawing.Drawing2D.DashStyle.DashDot,
			System.Drawing.Drawing2D.DashStyle.DashDotDot
		};

        public ArrayList Plots
		{
			get
			{
				return this.plotList;
			}
		}

        public void AutoScaleAxis() {
            foreach( Plot plot in this.plotList ) {
                plot.AutoScaleAxis();
            }
           
        }

		public void PlotsChanged()
		{
			
			ScalePlots();

			// setup menus

			int index = 0;
			foreach(MenuItem plotItem in plotMenu.MenuItems)
			{
				plotItem.Text = ((Plot) plotList[index]).Title;

				index++;
			}
		}

		public bool TimeSeries
		{
			set
			{
				timeSeries = value;

				if (value)
				{
					this.datePanel.Visible = true;
					this.numericalPanel.Visible = false;
				}
				else
				{
					this.datePanel.Visible = false;
					this.numericalPanel.Visible = true;
				}
			}

			get
			{
				return timeSeries;
			}
		}

		
		public DateTime Start
		{
			set
			{
				if (startDate.MaxDate < value)
					startDate.MaxDate = value;

				
				if (startDate.MinDate > value)
					startDate.MinDate = value;

				startDate.Value = value;
			}
			get
			{
				return startDate.Value;
			}
		}

		public DateTime End
		{
			set
			{
				if (startDate.MaxDate < value)
					startDate.MaxDate = value;

				
				if (startDate.MinDate > value)
					startDate.MinDate = value;

				endDate.Value = value;
			}
			get
			{
				return endDate.Value;
			}

		}

		public bool ScatterPlot
		{
			set
			{
				scatterPlot = value;
			}
			get
			{
				return scatterPlot;
			}
		}

		private bool singleGraphMod = false;
		public bool IndividualGraphs
		{
			set
			{
				singleGraphMod = value;
				this.applyAll.Enabled = !value;
			}

			get
			{
				return singleGraphMod;
			}
		}

		public Plot NewPlot()
		{
            Plot aPlot = new Plot();
			bool firstPlot = this.plotList.Count == 0;

			MenuItem plotItem = new MenuItem();

			plotItem.Text = aPlot.Title;

			plotItem.Click +=new EventHandler(plotItem_Click);
	
			plotMenu.MenuItems.Add(plotItem);
			
			plotList.Add(aPlot);
			aPlot.TimeSeries = TimeSeries;
			aPlot.ScatterPlot = ScatterPlot;

			aPlot.MdiParent = this;

			aPlot.Show();

			if (TimeSeries)
			{
				aPlot.Start = startDate.Value;
				aPlot.End = endDate.Value;
			}

			aPlot.PlotSelected +=new Utilities.Graphing.Plot.SelectMe(aPlot_PlotSelected);
            aPlot.CurveSelected += new Utilities.Graphing.Plot.SelectedCurve( aPlot_CurveSelected );
            aPlot.LegendVisibilitySet += new Plot.SetLegendVisibility( aPlot_LegendVisibilitySet );

			// first one is child
			// the rest go on preview
			if (firstPlot)
			{
				preview.Visible = false;
				this.splitter1.Visible = false;

				aPlot.Dock = System.Windows.Forms.DockStyle.Fill;

				aPlot.ActivePlot = true;
			}
			else
			{
				int width;
				int height;
				previewWidthHeight(out width, out height);

				aPlot.ActivePlot = false;

				preview.Visible = true;
				this.splitter1.Visible = true;

				aPlot.Size = new Size(width, height);

				aPlot.Dock = System.Windows.Forms.DockStyle.Top;

				preview.Controls.Add(aPlot);

				aPlot.SendToBack();
			}
			return aPlot;
		}
		public void ScalePlots()
		{
			foreach(Plot plot in this.plotList)
			{
				plot.Scale();
			}
		}


        public void write( System.IO.StreamWriter writer ) {
            write( writer, false );
        }

		public void write( System.IO.StreamWriter writer, bool useConsumerPanelFormat )
		{
            if( useConsumerPanelFormat == false ) {
                foreach( Plot plot in this.plotList ) {
                    writer.WriteLine( plot.Title );

                    plot.Write( writer );
                }
            }
            else {
                // write the CSV data in a format that is suitable for a consumer panel.
                string titleForPlotRow = "Consumer Summary Panel";
                foreach( Plot plot in this.plotList ) {
                    plot.Write( writer, true, titleForPlotRow );
                    titleForPlotRow = null;
                }
            }
		}

        public void write( ArrayList listOfDataRows ) {
            foreach( Plot plot in this.plotList ) {
                plot.Write( listOfDataRows );
            }
        }

		private ArrayList plotList = new ArrayList();
		
		private bool scatterPlot = false;
		
		private void plotItem_Click(object sender, System.EventArgs e)
		{

			MenuItem plotItem = (MenuItem) sender;

			int index = plotItem.Index;
			aPlot_PlotSelected((Plot) this.plotList[index]);
		}

		
		private void previewWidthHeight(out int width, out int height)
		{
			width = preview.Size.Width;
			height = 10 * width/12;

			if (height > this.Height - 150)
				height = this.Height - 150;
		}
		private void allignCharts()
		{
			if (this.preview.Controls.Count == 0)
			{
				return;
			}
			double min =  100000;
			double max = -100000;
			double percentMin =  0;
			double percentMax = 100;
			// compute min and max
			foreach(Plot plot in plotList)
			{
				if (min >  plot.Min)
				{
					min = plot.Min;
				}
				if (max < plot.Max)
				{
					max = plot.Max;
				}
				if (percentMin >  plot.PercentMin)
				{
					percentMin = plot.PercentMin;
				}
				if (percentMax < plot.PercentMax)
				{
					percentMax = plot.PercentMax;
				}
				
			}
			foreach(Plot plot in this.plotList)
			{
				plot.Min = min;
				plot.Max = max;
				plot.PercentMax = percentMax;
				plot.PercentMin = percentMin;

				plot.DataChanged();
			}
			if (minY.Minimum > (decimal) min)
			{
				minY.Minimum = (decimal) (min - Math.Abs(min));
			}
			if (maxY.Maximum < (decimal) max)
			{
				maxY.Maximum = (decimal) (max + Math.Abs(max));
			}
			minY.Value = (decimal) min;
			maxY.Value = (decimal) max;
			minY.DataBindings.Clear();
			maxY.DataBindings.Clear();

			if (minPercent.Minimum > (decimal) percentMin)
			{
				minPercent.Minimum = (decimal) (percentMin - Math.Abs(percentMin));
			}
			if (maxPercent.Maximum < (decimal) percentMax)
			{
				maxPercent.Maximum = (decimal) (percentMax + Math.Abs(percentMax));
			}
			minPercent.Value = (decimal) percentMin;
			maxPercent.Value = (decimal) percentMax;
			minPercent.DataBindings.Clear();
			maxPercent.DataBindings.Clear();
		}

		private void yAxisBox_TextChanged(object sender, System.EventArgs e)
		{
			string newName = "    ";

			if (yAxisBox.Text.Length > 0)
				newName = yAxisBox.Text;

			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.YAxis = newName;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.YAxis = newName;
				plot.DataChanged();
			}
		}

		private void startDate_ValueChanged(object sender, System.EventArgs e)
		{
			DateTime val;

			if (startDate.Value == endDate.Value)
				val = startDate.Value.AddDays(-1);
			else
				val = startDate.Value;

			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.Start = val;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.Start = val;
				plot.DataChanged();
			}
		}

		private void endDate_ValueChanged(object sender, System.EventArgs e)
		{
			DateTime val;

			if (startDate.Value == endDate.Value)
				val = endDate.Value.AddDays(1);
			else
				val = endDate.Value;

			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.End = val;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.End = val;
				plot.DataChanged();
			}
		}

		private void minY_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.Min = (double) minY.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.Min = (double) minY.Value;
				plot.DataChanged();
			}
		
		}

		private void maxY_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.Max = (double) maxY.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;

				if (plot == null)
					return;

				plot.Max = (double) maxY.Value;
				plot.DataChanged();
			}
		}

		private void imageItem_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild == null)
				return;

			Plot plot = (Plot) this.ActiveMdiChild;
		
			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();

			saveFileDlg.DefaultExt = ".bmp";
			saveFileDlg.Filter = "bitmap (*.bmp)|*.bmp|jpeg (*.jpg)|*.jpg";

			saveFileDlg.CheckFileExists = false;

			DialogResult rslt = saveFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string fileName = saveFileDlg.FileName;

				System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Bmp;

				if (fileName.EndsWith(".jpg"))
					format = System.Drawing.Imaging.ImageFormat.Jpeg;

				Bitmap image = plot.Image;

				image.Save(fileName, format);
			}
		}

		private void csvItem_Click(object sender, System.EventArgs e)
		{
			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();

			saveFileDlg.DefaultExt = ".csv";
			saveFileDlg.Filter = "CSV File (*.csv)|*.csv";

			saveFileDlg.CheckFileExists = false;
			//saveFileDlg.ReadOnlyChecked = false;

			DialogResult rslt = saveFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string fileName = saveFileDlg.FileName;
				
				System.IO.StreamWriter writer;

				// any open erros are due to file being in use
				try
				{
					writer = new System.IO.StreamWriter(fileName);
				}
				catch(System.IO.IOException oops)
				{
					MessageBox.Show(oops.Message);
					return;
				}

				foreach(Plot plot in this.plotList)
				{
					writer.WriteLine(plot.Title);

					plot.Write(writer);
				}

				writer.Flush();
				writer.Close();
			}
		}

		private void MultiGrapher_MdiChildActivate(object sender, System.EventArgs e)
		{
			if (!applyAll.Checked)
			{
                // turn off callbacks
                
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				yAxisBox.Text = plot.YAxis;

                minY.ValueChanged -= minY_ValueChanged;
				minY.Value = (decimal) plot.Min;
                minY.ValueChanged += minY_ValueChanged;

                maxY.ValueChanged -= maxY_ValueChanged;
				maxY.Value = (decimal) plot.Max;
                maxY.ValueChanged += maxY_ValueChanged;

				this.minPercent.Value = (decimal) plot.PercentMin;
				this.maxPercent.Value = (decimal) plot.PercentMax;

				if (TimeSeries)
				{
                    endDate.ValueChanged -= endDate_ValueChanged;
					endDate.Value = plot.End;
                    endDate.ValueChanged += endDate_ValueChanged;

                    startDate.ValueChanged -= startDate_ValueChanged;
					startDate.Value = plot.Start;
                    startDate.ValueChanged += startDate_ValueChanged;
				}
				else
				{
                    minX.ValueChanged -= minX_ValueChanged;
					minX.Value = (decimal) plot.MinX;
                    minX.ValueChanged += minX_ValueChanged;

                    maxX.ValueChanged -= maxX_ValueChanged;
					maxX.Value = (decimal) plot.MaxX;
                    maxX.ValueChanged += maxX_ValueChanged;
				}

				curveSelect.DataSource = plot.Curves;
				curveSelect.DisplayMember = "Label";
			}
		}

		private void applyAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
				applyAll.Checked = false;
			else
				applyAll.Checked = true;

			if (applyAll.Checked)
			{
				this.allignCharts();
			}
			else
			{

				foreach(Plot aPlot in plotList)
				{
					aPlot.AutoScaleAxis();
					aPlot.DataChanged();
				}

				if (this.ActiveMdiChild == null)
					return;
				Plot plot = (Plot) this.ActiveMdiChild;
				yAxisBox.Text = plot.YAxis;
				minY.Value = (decimal) plot.Min;
				maxY.Value = (decimal) plot.Max;
				if (TimeSeries)
				{
					endDate.Value = plot.End;
					startDate.Value = plot.Start;
				}
				else
				{
					minX.Value = (decimal) plot.MinX;
					maxX.Value = (decimal) plot.MaxX;
				}
			}
		}

		private void colorButton_Click(object sender, System.EventArgs e)
		{
			if (curveSelect.SelectedItem == null)
				return;

			ZedGraph.LineItem crv = (ZedGraph.LineItem) curveSelect.SelectedItem;

			ColorDialog dlg = new ColorDialog();

			dlg.AllowFullOpen = false;
			dlg.SolidColorOnly = true;

			dlg.Color = crv.Color;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				colorButton.BackColor = dlg.Color;

				int crvIndex = curveSelect.SelectedIndex;

				if (this.IndividualGraphs)
				{
					if (this.ActiveMdiChild == null)
						return;

					Plot curPlot = (Plot) this.ActiveMdiChild;
				
					ZedGraph.LineItem plotCrv = (ZedGraph.LineItem) curPlot.Curves[crvIndex]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
					plotCrv.Color = dlg.Color;
                    if( plotCrv.Line.Fill.Type != ZedGraph.FillType.None ) {
                        SetCrvFillColor( plotCrv );
                    }
					curPlot.DataChanged();

					return;
				}

				foreach(Plot plot in this.plotList)
				{
					if (crvIndex < plot.Curves.Count)
					{
						ZedGraph.LineItem plotCrv = (ZedGraph.LineItem) plot.Curves[crvIndex]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
				 
						plotCrv.Color = dlg.Color;
                        if( plotCrv.Line.Fill.Type != ZedGraph.FillType.None ) {
                            SetCrvFillColor( plotCrv );
                        }

						plot.DataChanged();
					}
				}

				
			}
		}

	
		private void curveSelect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (curveSelect.SelectedItem == null)
				return;

		
			Plot curPlot = ((Plot) this.ActiveMdiChild);

			ZedGraph.LineItem crv = (ZedGraph.LineItem) curveSelect.SelectedItem;

			doNotUpdate = true;

			colorButton.BackColor = crv.Color;

			symbolBox.SelectedItem = crv.Symbol.Type;

            if( curPlot != null ) {
                for( int cc = 0; cc < curveSelect.Items.Count; cc++ ) {
                    Console.WriteLine( "index chg --> scale {0} = {1}", cc, curPlot.CrvScale( cc ) );
                }

                this.curveScale.Value = curPlot.CrvScale( curveSelect.SelectedIndex );
            }

			if (crv.Line.Fill.Type != ZedGraph.FillType.None)
			{
				fill.Checked = true;
                Color fillColor = crv.Line.Fill.Color;
                this.transparencyTrackBar.Scroll -= new EventHandler(transparencyTrackBar_Scroll);
                this.transparencyTrackBar.Value = 255 - fillColor.A;
                this.transparencyTrackBar.Scroll += new EventHandler( transparencyTrackBar_Scroll );
                this.transparencyTrackBar.Visible = true;
            }
			else
			{	
				fill.Checked = false;
                this.transparencyTrackBar.Visible = false;
			}

            double curWidth = crv.Line.Width;
            this.widthUpDown.Value = Convert.ToDecimal( curWidth );

            System.Drawing.Drawing2D.DashStyle curLineStyle = crv.Line.Style;
            this.styleBox.SelectedItem = curLineStyle;

			doNotUpdate = false;
		}

		private void minX_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.MinX = (double) minX.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.MinX = (double) minX.Value;
				plot.DataChanged();
			}
		
		}

		private void maxX_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.MaxX = (double) maxX.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;
				
				if (plot == null)
					return;

				plot.MaxX = (double) maxX.Value;
				plot.DataChanged();
			}
		}

		private void symbolBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (doNotUpdate)
				return;

			if (curveSelect.SelectedItem == null)
				return;

			int crvIndex = curveSelect.SelectedIndex;

			if (this.IndividualGraphs)
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot curPlot = (Plot) this.ActiveMdiChild;
				
				ZedGraph.LineItem crv = (ZedGraph.LineItem) curPlot.Curves[crvIndex]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
				crv.Symbol.Type = (ZedGraph.SymbolType) symbolBox.SelectedItem;
				curPlot.DataChanged();

				return;
			}

			foreach(Plot plot in this.plotList)
			{
				if (crvIndex < plot.Curves.Count)
				{
					ZedGraph.LineItem crv = (ZedGraph.LineItem) plot.Curves[crvIndex]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
				 
					crv.Symbol.Type = (ZedGraph.SymbolType) symbolBox.SelectedItem;

					plot.DataChanged();
				}
			}
		}

		private void aPlot_PlotSelected(Plot activePlot)
		{
			int width;
			int height;
			previewWidthHeight(out width, out height);

			preview.SuspendLayout();
			preview.Controls.Clear();

			// activate this plot
			activePlot.MdiParent = this;
			activePlot.Show();
			activePlot.ActivePlot = true;
			activePlot.Dock = System.Windows.Forms.DockStyle.Fill;
            activePlot.LegendVisible = legendsVisible;

			foreach(Plot plot in plotList)
			{
				if (plot == activePlot)
					continue;

				if (plot == null)
				{
					plotList.Remove(plot);
					continue;
				}

				plot.Size = new Size(width, height);
				preview.Controls.Add(plot);
				plot.ActivePlot = false;
                plot.LegendVisible = false;
				plot.Dock = System.Windows.Forms.DockStyle.Top;
				plot.SendToBack();
			}

			preview.ResumeLayout(true);
		}

        private void aPlot_LegendVisibilitySet( bool visible ){
            legendsVisible = visible;
        }

		private void aPlot_CurveSelected(int index)
		{
			if (index < curveSelect.Items.Count)
			{
				curveSelect.SelectedIndex = index;
			}
		}

		private void preview_Resize(object sender, System.EventArgs e)
		{
			int width;
			int height;
			previewWidthHeight(out width, out height);

			foreach(Plot plot in plotList)
			{
				if (plot == this.ActiveMdiChild)
					continue;

				plot.Size = new Size(width, height);
			}
		
		}

		private void MultiGrapher_Resize(object sender, System.EventArgs e)
		{
			if (preview.Width > this.Width - 200)
			{
				preview.Width = this.Width - 200;
			}
		}

		private void fill_CheckedChanged(object sender, System.EventArgs e)
		{
			if (doNotUpdate)
				return;
			if (curveSelect.SelectedItem == null)
				return;
			if (this.ActiveMdiChild == null)
				return;

            this.transparencyTrackBar.Visible = fill.Checked;
			int crvIndex =  curveSelect.SelectedIndex;

			if (this.IndividualGraphs)
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot curPlot = (Plot) this.ActiveMdiChild;
				
				ZedGraph.LineItem crv = (ZedGraph.LineItem) curPlot.Curves[crvIndex]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
				
				if (fill.Checked)
				{
                    SetCrvFillColor( crv );
                    int curveFillAlpha = crv.Line.Fill.Color.A;
                    this.transparencyTrackBar.Scroll -= new EventHandler( transparencyTrackBar_Scroll );
                    this.transparencyTrackBar.Value = curveFillAlpha;
                    this.transparencyTrackBar.Scroll +=new EventHandler(transparencyTrackBar_Scroll);
                }
				else
				{
					crv.Line.Fill.Type = ZedGraph.FillType.None;
                }

				curPlot.DataChanged();

				return;
			}

			
			if (fill.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					if (crvIndex < plot.Curves.Count)
					{
						ZedGraph.LineItem crv = (ZedGraph.LineItem) plot.Curves[crvIndex]; 
                        SetCrvFillColor( crv );
                        if( plot.ActivePlot ) {
                            int curveFillAlpha = crv.Line.Fill.Color.A;
                            this.transparencyTrackBar.Scroll -= new EventHandler( transparencyTrackBar_Scroll );
                            this.transparencyTrackBar.Value = curveFillAlpha;
                            this.transparencyTrackBar.Scroll += new EventHandler( transparencyTrackBar_Scroll );
                        }
                        plot.DataChanged();
					}
				}
			}
			else
			{
				foreach(Plot plot in this.plotList)
				{
					if (crvIndex < plot.Curves.Count)
					{
						ZedGraph.LineItem crv = (ZedGraph.LineItem) plot.Curves[crvIndex];
						crv.Line.Fill.Type = ZedGraph.FillType.None;
						plot.DataChanged();
					}
				}
			}		
		}

        private void SetCrvFillColor( ZedGraph.LineItem crv ) {
            Color c1 = Color.FromArgb( 100, Color.White );
            Color c2 = Color.FromArgb( 100, crv.Color );

            crv.Line.Fill = new ZedGraph.Fill( c1, c2, 45 );
        }

		private void moveUp_Click(object sender, System.EventArgs e)
		{
			int crvIndex =  curveSelect.SelectedIndex;
			foreach(Plot plot in this.plotList)
			{
				if (crvIndex < plot.Curves.Count)
				{
					ZedGraph.LineItem crv = (ZedGraph.LineItem) plot.Curves[crvIndex];
					plot.PushToBack(crv);
					plot.DataChanged();
				}
			}
			if (this.ActiveMdiChild == null)
				return;
			Plot curPlot = (Plot) this.ActiveMdiChild;
		
			curveSelect.DataSource = null;
			curveSelect.DataSource = curPlot.Curves;
			curveSelect.DisplayMember = "Label";

            this.curveSelect.SelectedIndexChanged -= new EventHandler( curveSelect_SelectedIndexChanged );
            this.curveSelect.SelectedIndex = this.curveSelect.Items.Count - 1;       // reselect last item
            this.curveSelect.SelectedIndexChanged += new EventHandler( curveSelect_SelectedIndexChanged );
		}

		// remove scaling from plots
		private void noScale_Click(object sender, System.EventArgs e)
		{
			foreach(Plot plot in this.plotList)
			{
				if (noScale.Checked)
				{	
					plot.RemoveScaling();
					curveScale.Enabled = false;
				}
				else
				{
					plot.Scale();
					curveScale.Enabled = true;
				}

				plot.DataChanged();
			}

			noScale.Checked = !noScale.Checked;
		}

		private void curveScale_ValueChanged(object sender, System.EventArgs e)
		{
			if (doNotUpdate)
				return;
			if (curveSelect.SelectedItem == null)
				return;
			if (this.ActiveMdiChild == null)
				return;

			int crvIndex =  curveSelect.SelectedIndex;

            Plot curActivePlot = (Plot) this.ActiveMdiChild;
			curActivePlot.ScaleCrv(crvIndex, (int) curveScale.Value);

            // show the new scaling in the curve list
			curveSelect.DataSource = null;
            curveSelect.DataSource = curActivePlot.Curves;
			curveSelect.DisplayMember = "Label";
        }

		private void maxPercent_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.PercentMax = (double) maxPercent.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;

				if (plot == null)
					return;

				plot.PercentMax = (double) maxPercent.Value;
				plot.DataChanged();
			}
			
		}

		private void minPercent_ValueChanged(object sender, System.EventArgs e)
		{
			if (applyAll.Checked)
			{
				foreach(Plot plot in this.plotList)
				{
					plot.PercentMin = (double) minPercent.Value;
					plot.DataChanged();
				}
			}
			else
			{
				if (this.ActiveMdiChild == null)
					return;

				Plot plot = (Plot) this.ActiveMdiChild;

				if (plot == null)
					return;

				plot.PercentMin = (double) minPercent.Value;
				plot.DataChanged();
			}
        }

        private void saveAsButton_Click( object sender, EventArgs e ) {

            Common.Dialogs.NameAndDescr2 ndlg = null;
            if( this.namedSettings != null ) {
                ndlg = new Common.Dialogs.NameAndDescr2( "Save Settings As", Color.Beige, "SaveSettingsAs" );

                ndlg.ObjName = this.namedSettings.SettingsName;
                ndlg.ObjDescription = this.namedSettings.SettingsDescription;
            }
            else {
                // create a new NamedSettings object
                NamedSettings newSettings = new NamedSettings();
                newSettings.SettingsName = "";
                newSettings.SettingsDescription = "";
                newSettings.ControllerClass = "ResultsForm";
                newSettings.RendererClass = "MultiGrapher";
                this.namedSettings = newSettings;

                ndlg = new Common.Dialogs.NameAndDescr2( "Save Settings", Color.Beige, "SaveSettings" );
            }
            DialogResult resp = ndlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( Results.Settings.NamedResultsSettings == null ) {
                Results.Settings.NamedResultsSettings = new NamedSettings[ 0 ];
            }

            // make sure the settings name is a new one
            foreach( NamedSettings existingNamedSettings in Results.Settings.NamedResultsSettings ) {
                if( existingNamedSettings != null ) {
                    if( ndlg.ObjName == existingNamedSettings.SettingsName ) {

                        string msg2 = String.Format( "\r\n      Error: Results Settings \"{0}\" already exists.      \r\n", existingNamedSettings.SettingsName );
                        Common.Dialogs.ConfirmDialog cdlg = new Common.Dialogs.ConfirmDialog( msg2, "Error", null );
                        cdlg.SetOkButtonOnlyStyle();
                        cdlg.SelectWarningIcon();
                        cdlg.Height += 20;
                        cdlg.ShowDialog();
                        return;
                    }
                }
            }

            // add the new settings to the master list
            NamedSettings newSettingsItem = new NamedSettings( this.namedSettings, ndlg.ObjName, ndlg.ObjDescription );
            newSettingsItem.ControllerClass = "ResultsForm";
            newSettingsItem.RendererClass = "MultiGrapher";
            this.namedSettings = newSettingsItem;

            // update the new settings from the UI settings
            UpdateSettingsFromUI();

            NewNamedSettingsAdded( this.namedSettings );      // call the ResultsForm method

            settingsNameLabel.Text = newSettingsItem.SettingsName;

            ConfirmSave();
        }

        private void saveButton_Click( object sender, EventArgs e ) {

            if( this.namedSettings == null ) {
                saveAsButton_Click( sender, e );
                return;
            }

            // update the current named settings from the UI settings
            UpdateSettingsFromUI();

            NamedSettingsSaved( this.namedSettings );      // call the ResultsForm method

            ConfirmSave();
        }

        private void ConfirmSave() {
            string msg = String.Format( "    Settings Saved    \r\n", this.namedSettings.SettingsName );
            Common.Dialogs.CompletionDialog cdlg = new Common.Dialogs.CompletionDialog( msg );
            cdlg.ShowDialog();
        }

        // updates the UI (and graph) state from the named settings
        private void UpdateUIFromSettings() {
            //try {
                int plotCount = this.namedSettings.GetInt( "PlotCount" );

                // sanity check
                if( this.plotList.Count != plotCount ) {
                    // !!! how do we handle this???
                    Console.WriteLine( "Error: Mismatch between plot count in settings ({0}) and in current display ({1})", plotCount, this.plotList.Count );
                    return;
                }

                for( int p = 0; p < plotCount; p++ ) {
                    string curveCountKey = String.Format( "CurveCount{0}", p );
                    int curveCount = this.namedSettings.GetInt( curveCountKey );

                    Console.WriteLine( "\nRESTORE plot {0}", p );

                    Plot plot = (Plot)this.plotList[ p ];
                    if( plot.Curves.Count != curveCount ) {
                        // !!! how do we handle this???
                        Console.WriteLine( "Error: Mismatch between curve count for plot {2} in settings ({0}) and in current display ({1})", plotCount, this.plotList.Count, p );
                        return;
                    }

                    // restore the overall display settings
                    if( this.namedSettings.GetSetting( "PlotMinX" + p.ToString() ) != null ) {
                        plot.MinX = this.namedSettings.GetDouble( "PlotMinX" + p.ToString() );
                        plot.MaxX = this.namedSettings.GetDouble( "PlotMaxX" + p.ToString() );
                        plot.Start = this.namedSettings.GetDateTime( "PlotStart" + p.ToString() );
                        plot.End = this.namedSettings.GetDateTime( "PlotEnd" + p.ToString() );
                        plot.Min = this.namedSettings.GetDouble( "PlotMinY" + p.ToString() );
                        plot.Max = this.namedSettings.GetDouble( "PlotMaxY" + p.ToString() );
                        plot.PercentMin = this.namedSettings.GetDouble( "PlotMinPercentY" + p.ToString() );
                        plot.PercentMax = this.namedSettings.GetDouble( "PlotMaxPercentY" + p.ToString() );
                        string yLabel = this.namedSettings.GetString( "PlotYLabel" + p.ToString() );
                        if( yLabel == null ) {
                            yLabel = "";
                        }
                        plot.YAxis = yLabel;
                    }

                    // first pass - put the curves into the sequence in the settings
                    Hashtable theCurves = new Hashtable();
                    Hashtable theScalings = new Hashtable();
                    for( int c = 0; c < curveCount; c++ ) {
                        ZedGraph.LineItem theCrv = (ZedGraph.LineItem)plot.Curves[ c ];
                        int curveScl = 0;
                        string curveBaseName = GetBaseCurveName( theCrv.Label, out curveScl );
                        theCurves.Add( curveBaseName, theCrv );
                        theScalings.Add( curveBaseName, curveScl );
                    }
                    plot.Curves.Clear();

                    int[] newCrvScale = new int[ curveCount ];
                    int[] resequencedExistingiCrvScale = new int[ curveCount ];
                    for( int c = 0; c < curveCount; c++ ) {
                        string nameKey = String.Format( "GraphCurveName{0}-{1}", p, c );
                        string desiredCurveBaseName = this.namedSettings.GetString( nameKey );

                        string scalingKey = String.Format( "GraphCurveScale{0}-{1}", p, c );
                        int scaling = this.namedSettings.GetInt( scalingKey );
                        newCrvScale[ c ] = scaling;

                        ZedGraph.LineItem theCrv = (ZedGraph.LineItem)theCurves[ desiredCurveBaseName ];
                        int theCrvScaling = 0;
                        if( theScalings.ContainsKey( desiredCurveBaseName ) ) {
                            theCrvScaling = (int)theScalings[ desiredCurveBaseName ];
                        }
                        resequencedExistingiCrvScale[ c ] = theCrvScaling;

                        if( theCrv != null ) {
                            Console.WriteLine( "RESTORE PLOT {0} curve {1}:  scaling {2} name is {3}", p, c, scaling, desiredCurveBaseName );
                            plot.Curves.Add( theCrv );
                        }
                        else {
                            Console.WriteLine( "Error!  Curve \"{0}\" not found!  Plot = {1}", desiredCurveBaseName, p );
                        }
                    }

                    // this method also adjusts the data for any curves that have their scale value changed 
                    for( int ii = 0; ii < newCrvScale.Length; ii++ ) {
                        Console.WriteLine( "curve {0} new scale {1} old scale {2}", ii, newCrvScale[ ii ], resequencedExistingiCrvScale[ ii ] );
                    }
                    plot.SetCrvScale( newCrvScale, resequencedExistingiCrvScale );

                    // second pass -- transfer the curve settings to the curves, now that they are in the right sequence
                    for( int c = 0; c < curveCount; c++ ) {
                        string nameKey = String.Format( "GraphCurveName{0}-{1}", p, c );
                        string scalingKey = String.Format( "GraphCurveScale{0}-{1}", p, c );
                        string colorKey = String.Format( "GraphCurveColor{0}-{1}", p, c );
                        string fillKey = String.Format( "GraphCurveFill{0}-{1}", p, c );
                        string symbolKey = String.Format( "GraphCurveSymbol{0}-{1}", p, c );
                        string transparencyKey = String.Format( "GraphCurveAlpha{0}-{1}", p, c );
                        string styleKey = String.Format( "GraphCurveStyle{0}-{1}", p, c );
                        string widthKey = String.Format( "GraphCurveWidth{0}-{1}", p, c );

                        string cName = this.namedSettings.GetString( nameKey );
                        Color color = (Color)this.namedSettings.GetSetting( colorKey );

                        int scaling = this.namedSettings.GetInt( scalingKey );
                        double lineWidth = this.namedSettings.GetDouble( widthKey );
                        string lineStyleStr = this.namedSettings.GetString( styleKey );
                        System.Drawing.Drawing2D.DashStyle lineStyle = (System.Drawing.Drawing2D.DashStyle)Enum.Parse( typeof( System.Drawing.Drawing2D.DashStyle ), lineStyleStr );

                        int alpha = 255;
                        if( this.namedSettings.GetSetting( transparencyKey ) != null ) {
                            alpha = this.namedSettings.GetInt( transparencyKey );
                        }

                        bool doFill = this.namedSettings.GetBool( fillKey );

                        string symName = this.namedSettings.GetString( symbolKey );
                        ZedGraph.SymbolType symbolType = new ZedGraph.SymbolType();
                        if( symName != null ) {
                            symbolType = (ZedGraph.SymbolType)Enum.Parse( typeof( ZedGraph.SymbolType ), symName );
                        }

                        if( c < plot.Curves.Count ) {     //sanity check -- some curves may be missing (?)
                            ZedGraph.LineItem theCrv = (ZedGraph.LineItem)plot.Curves[ c ];

                            theCrv.Color = color;
                            theCrv.Label = GetDisplayCurveName( cName, scaling );

                            Console.WriteLine( "** Setting UI line color to {0} from key {1}", theCrv.Color.ToString(), colorKey );

                            if( doFill ) {
                                Color c1 = Color.FromArgb( alpha, Color.White );
                                Color c2 = Color.FromArgb( alpha, theCrv.Color );
                                theCrv.Line.Fill = new ZedGraph.Fill( c1, c2, 45 );
                            }

                            if( symName != null ) {
                                theCrv.Symbol.Type = symbolType;
                            }

                            theCrv.Line.Width = (float)lineWidth;
                            theCrv.Line.Style = lineStyle;
                        }
                        else {
                            Console.WriteLine( "WARNING: Ignoring settings for curve {0} of plot {1} !", c, p );
                        }
                    }
                }


                int activePlotIndex = this.namedSettings.GetInt( "ActivePlotIndex" );
                Plot plotToActivate = null;
                if( activePlotIndex >= 0 && activePlotIndex < this.plotList.Count ) {
                    plotToActivate = (Plot)this.plotList[ activePlotIndex ];
                    if( plotToActivate != null ) {
                        // activate the desired plot
                        Console.WriteLine( "Activate Plot: " + plotToActivate.Title );
                        aPlot_PlotSelected( plotToActivate );
                    }
                }

                if( plotToActivate != null ) {
                    // refresh the curve-select list
                    this.curveSelect.DataSource = null;
                    this.curveSelect.DataSource = plotToActivate.Curves;
                    this.curveSelect.DisplayMember = "Label";

                    // show or hide the legend
                    if( this.namedSettings.GetSetting( "ActivePlotLegend" ) != null ) {
                        plotToActivate.LegendVisible = this.namedSettings.GetBool( "ActivePlotLegend" );
                    }
                }
            //}
            //catch( Exception ex ) {
            //    System.IO.FileStream fs = new System.IO.FileStream( "ErrorInfo.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write );
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter( fs );
            //    sw.WriteLine( String.Format( "ERROR: Exception from UpdateSettingsFromUI() at {0}", DateTime.Now.ToString() ) );
            //    sw.WriteLine();
            //    sw.WriteLine( ex.Message );
            //    if( ex.InnerException != null ) {
            //        sw.WriteLine( "Inner Exception: " + ex.InnerException.Message );
            //    }
            //    sw.WriteLine();
            //    sw.Flush();
            //    sw.Close();
            //    fs.Close();

            //    MessageBox.Show( "\r\n\r\n    Exception Caught!  Details have been appended to ErrorInfo.txt    \r\n\r\n", "Error" );
            //}
        }

        // updates the current named settings from the UI and graph states
        private void UpdateSettingsFromUI() {
            int plotCount = this.plotList.Count;
            this.namedSettings.SetInt( "PlotCount", plotCount );
            if( plotCount == 0 ) {
                return;
            }

            // save the individual plot settings
            for( int p = 0; p < plotCount; p++ ) {

                Plot plot = (Plot)this.plotList[ p ];

                // save the overall display settings
                this.namedSettings.SetDouble( "PlotMinX" + p.ToString(), plot.MinX );
                this.namedSettings.SetDouble( "PlotMaxX" + p.ToString(), plot.MaxX );
                this.namedSettings.SetDateTime( "PlotStart" + p.ToString(), plot.Start );
                this.namedSettings.SetDateTime( "PlotEnd" + p.ToString(), plot.End );
                this.namedSettings.SetDouble( "PlotMinY" + p.ToString(), plot.Min );
                this.namedSettings.SetDouble( "PlotMaxY" + p.ToString(), plot.Max );
                this.namedSettings.SetDouble( "PlotMinPercentY" + p.ToString(), plot.PercentMin );
                this.namedSettings.SetDouble( "PlotMaxPercentY" + p.ToString(), plot.PercentMax );
                this.namedSettings.SetString( "PlotYLabel" + p.ToString(), plot.YAxis );

                int curveCount = plot.Curves.Count;

                if( plot.ActivePlot == true ) {
                    this.namedSettings.SetInt( "ActivePlotIndex", p );
                    this.namedSettings.SetBool( "ActivePlotLegend", plot.LegendVisible );
                }

                string curveCountKey = String.Format( "CurveCount{0}", p );
                this.namedSettings.SetInt( curveCountKey, curveCount );

                Console.WriteLine( "\nSAVE plot {0} -- has (1) curves", plot.Title, curveCount );

                for( int c = 0; c < curveCount; c++ ) {

                    ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ c ];
                    int curveScaling = 0;
                    string curveName = GetBaseCurveName( crv.Label, out curveScaling );
                    Color color = crv.Color;
                    ZedGraph.Fill fill = crv.Line.Fill;
                    ZedGraph.Symbol symbol = crv.Symbol;
                    System.Drawing.Drawing2D.DashStyle style = crv.Line.Style;
                    double linewid = crv.Line.Width;

                    bool doFill = false;
                    int alpha = 255;
                    if( fill.Type != ZedGraph.FillType.None ) {
                        doFill = true;
                        Color fillColor1 = fill.Color;
                        alpha = fillColor1.A;
                    }
                    string symbolCode = symbol.Type.ToString();
                    string styleCode = style.ToString();

                    string nameKey = String.Format( "GraphCurveName{0}-{1}", p, c );
                    string scalingKey = String.Format( "GraphCurveScale{0}-{1}", p, c );
                    string colorKey = String.Format( "GraphCurveColor{0}-{1}", p, c );
                    string fillKey = String.Format( "GraphCurveFill{0}-{1}", p, c );
                    string symbolKey = String.Format( "GraphCurveSymbol{0}-{1}", p, c );
                    string transparencyKey = String.Format( "GraphCurveAlpha{0}-{1}", p, c );
                    string styleKey = String.Format( "GraphCurveStyle{0}-{1}", p, c );
                    string widthKey = String.Format( "GraphCurveWidth{0}-{1}", p, c );

                    this.namedSettings.SetString( nameKey, curveName );
                    this.namedSettings.SetInt( scalingKey, curveScaling );
                    this.namedSettings.SetSetting( colorKey, color );
                    Console.WriteLine( "** Writing color {0} to key {1}", color.ToString(), colorKey );
                    this.namedSettings.SetBool( fillKey, doFill );
                    this.namedSettings.SetString( symbolKey, symbolCode );
                    this.namedSettings.SetInt( transparencyKey, alpha );
                    this.namedSettings.SetString( styleKey, styleCode );
                    this.namedSettings.SetDouble( widthKey, linewid );

                    Console.WriteLine( "** Curve {0} is named \"{1}\" and has scaling {2}, color {3}", c, curveName, curveScaling, color );
                }
            }
        }

        private string GetDisplayCurveName( string baseName, int scaling ){
            if( scaling == 0 ){
                return baseName;
            }
            else {
                return String.Format( "{0}  (10^{1})", baseName, scaling );
            }
        }

        private string GetBaseCurveName( string overallName, out int curveScaling ) {
            string baseName = overallName.Trim();
            curveScaling = 0;

            if( baseName.EndsWith( ">" ) ) {
                baseName = baseName.Substring( 0, baseName.Length - 1 ).Trim();
            }

            int lastSpaceIndex = baseName.LastIndexOf( " " );
            if( lastSpaceIndex == -1 ) {
                // no scaling suffix
                return baseName;
            }

            string scalingSuffix = baseName.Substring( lastSpaceIndex + 1 );
            string scalingStart = "(10^";
            if( scalingSuffix.StartsWith( scalingStart ) == false || scalingSuffix.EndsWith( ")" ) == false ) {
                // no valid scaling suffix
                return baseName;
            }
            scalingSuffix = scalingSuffix.Substring( 0, scalingSuffix.Length - 1 ); 
            scalingSuffix = scalingSuffix.Substring( scalingStart.Length );
            try {
                curveScaling = int.Parse( scalingSuffix );
            }
            catch {
                // scaling value not an int!
                return baseName;
            }

            // we found a valid scaling value
            baseName = baseName.Substring( 0, lastSpaceIndex ).Trim();       // strip off the scaling part of the name
            return baseName;
        }

        private void MultiGrapher_Load( object sender, EventArgs e ) {
            if( this.namedSettings != null ) {
                UpdateUIFromSettings();
            }
        }

        private void moveUpButton_Click( object sender, EventArgs e ) {
            int crvIndex = curveSelect.SelectedIndex;
            if( crvIndex == 0 ){
                return;
            }

            bool plotMoved = false;
            foreach( Plot plot in this.plotList ) {
                ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ crvIndex ];
                if( plot.MoveForward( crv ) ) {
                    plotMoved = true;
                    plot.DataChanged();
                }
            }

            if( plotMoved ) {
                this.curveSelect.SelectedIndexChanged -=new EventHandler(curveSelect_SelectedIndexChanged);
                this.curveSelect.SelectedIndex -= 1;
                this.curveSelect.SelectedIndexChanged += new EventHandler( curveSelect_SelectedIndexChanged );
            }
            if( this.ActiveMdiChild == null )
                return;
            Plot curPlot = (Plot)this.ActiveMdiChild;

            curveSelect.DataSource = null;
            curveSelect.DataSource = curPlot.Curves;
            curveSelect.DisplayMember = "Label";
        }

        private void moveDownButton_Click( object sender, EventArgs e ) {
            int crvIndex = curveSelect.SelectedIndex;
            if( crvIndex == curveSelect.Items.Count - 1 ) {
                return;
            }

            bool plotMoved = false;
            foreach( Plot plot in this.plotList ) {
                ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ crvIndex ];
                if( plot.MoveBackward( crv ) ) {
                    plotMoved = true;
                    plot.DataChanged();
                }
            }

            if( plotMoved ) {
                this.curveSelect.SelectedIndexChanged -= new EventHandler( curveSelect_SelectedIndexChanged );
                this.curveSelect.SelectedIndex += 1;
                this.curveSelect.SelectedIndexChanged += new EventHandler( curveSelect_SelectedIndexChanged );
            }

            if( this.ActiveMdiChild == null )
                return;
            Plot curPlot = (Plot)this.ActiveMdiChild;

            curveSelect.DataSource = null;
            curveSelect.DataSource = curPlot.Curves;
            curveSelect.DisplayMember = "Label";
        }

        private void transparencyTrackBar_Scroll( object sender, EventArgs e ) {
            if( doNotUpdate )
                return;
            if( curveSelect.SelectedItem == null )
                return;
            if( this.ActiveMdiChild == null )
                return;

            int newAlpha = 255 - transparencyTrackBar.Value;
            int crvIndex = curveSelect.SelectedIndex;

            if( this.IndividualGraphs ) {
                if( this.ActiveMdiChild == null )
                    return;

                Plot curPlot = (Plot)this.ActiveMdiChild;

                ZedGraph.LineItem crv = (ZedGraph.LineItem)curPlot.Curves[ crvIndex ]; //(ZedGraph.LineItem) curveSelect.SelectedItem;

                if( fill.Checked ) {
                    Color c1 = Color.FromArgb( newAlpha, Color.White );
                    Color c2 = Color.FromArgb( newAlpha, crv.Color );

                    crv.Line.Fill = new ZedGraph.Fill( c1, c2, 45 );
                    //crv.Line.Fill = new ZedGraph.Fill( Color.White, crv.Color, 45 );
                }
                else {
                    crv.Line.Fill.Type = ZedGraph.FillType.None;
                }

                curPlot.DataChanged();

                return;
            }


            if( fill.Checked ) {
                foreach( Plot plot in this.plotList ) {
                    if( crvIndex < plot.Curves.Count ) {
                        ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ crvIndex ]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
                        Color c1 = Color.FromArgb( newAlpha, Color.White );
                        Color c2 = Color.FromArgb( newAlpha, crv.Color );
                        crv.Line.Fill = new ZedGraph.Fill( c1, c2, 45 );
                        plot.DataChanged();
                    }
                }
            }
            else {
                foreach( Plot plot in this.plotList ) {
                    if( crvIndex < plot.Curves.Count ) {
                        ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ crvIndex ]; //(ZedGraph.LineItem) curveSelect.SelectedItem;
                        crv.Line.Fill.Type = ZedGraph.FillType.None;
                        plot.DataChanged();
                    }
                }
            }
        }

        private void widthUpDown_ValueChanged( object sender, EventArgs e ) {
            if( curveSelect.SelectedItem == null )
                return;

            ZedGraph.LineItem crv = (ZedGraph.LineItem)curveSelect.SelectedItem;

            double newWid = (double)widthUpDown.Value;

            int crvIndex = curveSelect.SelectedIndex;

            if( this.IndividualGraphs ) {
                if( this.ActiveMdiChild == null )
                    return;

                Plot curPlot = (Plot)this.ActiveMdiChild;

                ZedGraph.LineItem plotCrv = (ZedGraph.LineItem)curPlot.Curves[ crvIndex ];

                plotCrv.Line.Width = (float)newWid;

                curPlot.DataChanged();

                return;
            }

            foreach( Plot plot in this.plotList ) {
                if( crvIndex < plot.Curves.Count ) {
                    ZedGraph.LineItem plotCrv = (ZedGraph.LineItem)plot.Curves[ crvIndex ];

                    plotCrv.Line.Width = (float)newWid;

                    plot.DataChanged();
                }
            }
        }

        private void styleBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( doNotUpdate )
                return;

            if( curveSelect.SelectedItem == null )
                return;

            int crvIndex = curveSelect.SelectedIndex;

            if( this.IndividualGraphs ) {
                if( this.ActiveMdiChild == null )
                    return;

                Plot curPlot = (Plot)this.ActiveMdiChild;

                ZedGraph.LineItem crv = (ZedGraph.LineItem)curPlot.Curves[ crvIndex ];

                crv.Line.Style = (System.Drawing.Drawing2D.DashStyle)styleBox.SelectedItem;

                curPlot.DataChanged();

                return;
            }

            foreach( Plot plot in this.plotList ) {
                if( crvIndex < plot.Curves.Count ) {
                    ZedGraph.LineItem crv = (ZedGraph.LineItem)plot.Curves[ crvIndex ]; 

                    crv.Line.Style = (System.Drawing.Drawing2D.DashStyle)styleBox.SelectedItem;

                    plot.DataChanged();
                }
            }
        }       
	}
}
