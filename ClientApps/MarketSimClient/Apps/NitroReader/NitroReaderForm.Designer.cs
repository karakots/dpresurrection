namespace NitroReader
{
    partial class NitroReaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            if( disposing ) {
                // get rid of any running EXCEL.EXE processes before we are all done.
                if( NitroReaderForm.reader != null ) {
                    NitroReaderForm.reader.Kill();
                    NitroReaderForm.reader = null;
                }
                if( NitroReaderForm.writer != null ) {
                    NitroReaderForm.writer.Kill();
                    NitroReaderForm.writer = null;
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "ABC",
            "Product ABC",
            "100"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.White, null );
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "DEF",
            "Variant DEF",
            "50"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.White, null );
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "            --- group ---",
            "Group 1",
            "20"}, 0, System.Drawing.Color.Empty, System.Drawing.Color.LightCyan, null );
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "XYZ",
            "Variant XYZ",
            "15"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.White, null );
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "            --- group ---",
            "Group 2",
            "15"}, 1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "EFGH",
            "Variant EFGH",
            "10"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "IJKL",
            "Variant IKJL",
            "3"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "MNOP",
            "Variant MNOP",
            "2"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "MNOR",
            "Variant MNOR",
            "1"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem( new string[] {
            "",
            "MNOS",
            "Product MNOS",
            "0"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.AntiqueWhite, null );
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( NitroReaderForm ) );
            this.selectButton = new System.Windows.Forms.Button();
            this.nitroFileLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.warningsButton = new System.Windows.Forms.Button();
            this.infoLabel2 = new System.Windows.Forms.Label();
            this.infoLabel1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.normalizeCheckBox = new System.Windows.Forms.CheckBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.renameButton = new System.Windows.Forms.Button();
            this.ungroupButton = new System.Windows.Forms.Button();
            this.groupButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader( 2 );
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList( this.components );
            this.copySettingsButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.goButton = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.customerLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // selectButton
            // 
            this.selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.selectButton.Location = new System.Drawing.Point( 525, 14 );
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size( 75, 23 );
            this.selectButton.TabIndex = 2;
            this.selectButton.Text = "Select...";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler( this.selectButton_Click );
            // 
            // nitroFileLabel
            // 
            this.nitroFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nitroFileLabel.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nitroFileLabel.Location = new System.Drawing.Point( 15, 18 );
            this.nitroFileLabel.Name = "nitroFileLabel";
            this.nitroFileLabel.Size = new System.Drawing.Size( 504, 17 );
            this.nitroFileLabel.TabIndex = 3;
            this.nitroFileLabel.Text = "C:\\Documents and Settings\\Joe User\\NITRO Files\\Product1 data 1-Jan2002.xls";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add( this.warningsButton );
            this.groupBox1.Controls.Add( this.infoLabel2 );
            this.groupBox1.Controls.Add( this.infoLabel1 );
            this.groupBox1.Controls.Add( this.nitroFileLabel );
            this.groupBox1.Controls.Add( this.selectButton );
            this.groupBox1.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox1.Location = new System.Drawing.Point( 12, 12 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 609, 109 );
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NITRO File to Process";
            // 
            // warningsButton
            // 
            this.warningsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.warningsButton.BackColor = System.Drawing.Color.Yellow;
            this.warningsButton.Font = new System.Drawing.Font( "Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.warningsButton.Location = new System.Drawing.Point( 496, 49 );
            this.warningsButton.Name = "warningsButton";
            this.warningsButton.Size = new System.Drawing.Size( 104, 19 );
            this.warningsButton.TabIndex = 6;
            this.warningsButton.Text = "Show Warnings";
            this.warningsButton.UseVisualStyleBackColor = false;
            this.warningsButton.Visible = false;
            this.warningsButton.Click += new System.EventHandler( this.warningsButton_Click );
            // 
            // infoLabel2
            // 
            this.infoLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel2.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.infoLabel2.Location = new System.Drawing.Point( 21, 35 );
            this.infoLabel2.Name = "infoLabel2";
            this.infoLabel2.Size = new System.Drawing.Size( 471, 17 );
            this.infoLabel2.TabIndex = 5;
            this.infoLabel2.Text = "Dates 1/1/2002 thru 1/1/2005";
            // 
            // infoLabel1
            // 
            this.infoLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel1.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.infoLabel1.Location = new System.Drawing.Point( 21, 52 );
            this.infoLabel1.Name = "infoLabel1";
            this.infoLabel1.Size = new System.Drawing.Size( 471, 48 );
            this.infoLabel1.TabIndex = 4;
            this.infoLabel1.Text = "5 Components: % Acv Dist, ...";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add( this.normalizeCheckBox );
            this.groupBox2.Controls.Add( this.nameTextBox );
            this.groupBox2.Controls.Add( this.label1 );
            this.groupBox2.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox2.Location = new System.Drawing.Point( 12, 131 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 666, 80 );
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Market Plan to Create";
            // 
            // normalizeCheckBox
            // 
            this.normalizeCheckBox.AutoSize = true;
            this.normalizeCheckBox.Location = new System.Drawing.Point( 58, 49 );
            this.normalizeCheckBox.Name = "normalizeCheckBox";
            this.normalizeCheckBox.Size = new System.Drawing.Size( 156, 18 );
            this.normalizeCheckBox.TabIndex = 4;
            this.normalizeCheckBox.Text = "Normalize Price Distribution";
            this.normalizeCheckBox.UseVisualStyleBackColor = true;
            this.normalizeCheckBox.CheckedChanged += new System.EventHandler( this.normalizeCheckBox_CheckedChanged );
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nameTextBox.Location = new System.Drawing.Point( 56, 23 );
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size( 595, 20 );
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.Text = "Your Market Plan Name";
            this.nameTextBox.TextChanged += new System.EventHandler( this.nameTextBox_TextChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 16, 25 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 34, 14 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add( this.renameButton );
            this.groupBox3.Controls.Add( this.ungroupButton );
            this.groupBox3.Controls.Add( this.groupButton );
            this.groupBox3.Controls.Add( this.listView1 );
            this.groupBox3.Controls.Add( this.copySettingsButton );
            this.groupBox3.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox3.Location = new System.Drawing.Point( 12, 220 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size( 666, 267 );
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Variants";
            this.groupBox3.Paint += new System.Windows.Forms.PaintEventHandler( this.groupBox3_Paint );
            // 
            // renameButton
            // 
            this.renameButton.Enabled = false;
            this.renameButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.renameButton.Location = new System.Drawing.Point( 246, 11 );
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size( 83, 23 );
            this.renameButton.TabIndex = 4;
            this.renameButton.Text = "Rename...";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Visible = false;
            this.renameButton.Click += new System.EventHandler( this.renameButton_Click );
            // 
            // ungroupButton
            // 
            this.ungroupButton.Enabled = false;
            this.ungroupButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.ungroupButton.Location = new System.Drawing.Point( 157, 11 );
            this.ungroupButton.Name = "ungroupButton";
            this.ungroupButton.Size = new System.Drawing.Size( 83, 23 );
            this.ungroupButton.TabIndex = 3;
            this.ungroupButton.Text = "Ungroup";
            this.ungroupButton.UseVisualStyleBackColor = true;
            this.ungroupButton.Click += new System.EventHandler( this.ungroupButton_Click );
            // 
            // groupButton
            // 
            this.groupButton.Enabled = false;
            this.groupButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupButton.Location = new System.Drawing.Point( 68, 11 );
            this.groupButton.Name = "groupButton";
            this.groupButton.Size = new System.Drawing.Size( 83, 23 );
            this.groupButton.TabIndex = 2;
            this.groupButton.Text = "Group...";
            this.groupButton.UseVisualStyleBackColor = true;
            this.groupButton.Click += new System.EventHandler( this.groupButton_Click );
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4} );
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange( new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10} );
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point( 20, 40 );
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size( 632, 210 );
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler( this.listView1_DoubleClick );
            this.listView1.SelectedIndexChanged += new System.EventHandler( this.listView1_SelectedIndexChanged );
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler( this.listView1_KeyDown );
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler( this.listView1_ColumnClick );
            this.listView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler( this.listView1_AfterLabelEdit );
            this.listView1.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler( this.listView1_BeforeLabelEdit );
            this.listView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler( this.listView1_ColumnWidthChanging );
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 22;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "MarketSim Name";
            this.columnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "NITRO ID";
            this.columnHeader3.Width = 280;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Volume";
            this.columnHeader4.Width = 80;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject( "imageList1.ImageStream" )));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName( 0, "folder-minus.GIF" );
            this.imageList1.Images.SetKeyName( 1, "folder-plus.GIF" );
            this.imageList1.Images.SetKeyName( 2, "up.gif" );
            this.imageList1.Images.SetKeyName( 3, "down.gif" );
            // 
            // copySettingsButton
            // 
            this.copySettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copySettingsButton.Enabled = false;
            this.copySettingsButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.copySettingsButton.Location = new System.Drawing.Point( 496, 11 );
            this.copySettingsButton.Name = "copySettingsButton";
            this.copySettingsButton.Size = new System.Drawing.Size( 155, 23 );
            this.copySettingsButton.TabIndex = 0;
            this.copySettingsButton.Text = "Import Settings  From...";
            this.copySettingsButton.UseVisualStyleBackColor = true;
            this.copySettingsButton.Click += new System.EventHandler( this.copySettingsButton_Click );
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.closeButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.closeButton.Location = new System.Drawing.Point( 32, 497 );
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size( 75, 23 );
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler( this.closeButton_Click );
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.saveButton.Location = new System.Drawing.Point( 122, 497 );
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size( 136, 23 );
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Visible = false;
            this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Enabled = false;
            this.goButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.goButton.Location = new System.Drawing.Point( 484, 497 );
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size( 179, 23 );
            this.goButton.TabIndex = 8;
            this.goButton.Text = "Process NITRO File";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler( this.goButton_Click );
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logoPictureBox.Image = global::NitroReader.Properties.Resources.MarketSimLogoIcon48x48;
            this.logoPictureBox.Location = new System.Drawing.Point( 627, 14 );
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size( 52, 52 );
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logoPictureBox.TabIndex = 10;
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.DoubleClick += new System.EventHandler( this.logoPictureBox_DoubleClick );
            // 
            // customerLogoPictureBox
            // 
            this.customerLogoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customerLogoPictureBox.Location = new System.Drawing.Point( 626, 69 );
            this.customerLogoPictureBox.Name = "customerLogoPictureBox";
            this.customerLogoPictureBox.Size = new System.Drawing.Size( 52, 52 );
            this.customerLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.customerLogoPictureBox.TabIndex = 11;
            this.customerLogoPictureBox.TabStop = false;
            this.customerLogoPictureBox.DoubleClick += new System.EventHandler( this.customerLogoPictureBox_DoubleClick );
            // 
            // NitroReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 690, 535 );
            this.Controls.Add( this.customerLogoPictureBox );
            this.Controls.Add( this.logoPictureBox );
            this.Controls.Add( this.goButton );
            this.Controls.Add( this.saveButton );
            this.Controls.Add( this.closeButton );
            this.Controls.Add( this.groupBox3 );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.groupBox1 );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 698, 569 );
            this.Name = "NitroReaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NITRO Reader";
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.NitroReaderForm_Paint );
            this.SizeChanged += new System.EventHandler( this.NitroReaderForm_SizeChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.NitroReaderForm_FormClosing );
            this.Load += new System.EventHandler( this.NitroReader_Load );
            this.groupBox1.ResumeLayout( false );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerLogoPictureBox)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Label nitroFileLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button copySettingsButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button ungroupButton;
        private System.Windows.Forms.Button groupButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Button warningsButton;
        private System.Windows.Forms.Label infoLabel2;
        private System.Windows.Forms.Label infoLabel1;
        private System.Windows.Forms.CheckBox normalizeCheckBox;
        private System.Windows.Forms.PictureBox customerLogoPictureBox;
        private System.Windows.Forms.Button renameButton;
    }
}

