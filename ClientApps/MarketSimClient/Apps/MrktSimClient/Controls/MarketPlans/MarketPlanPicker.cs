using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Common.Dialogs;
using Common;

using ExcelInterface;
using ErrorInterface;

using MarketSimUtilities;
using MrktSimDb;
using Utilities;

using MrktSimClient.Controls.Dialogs;

namespace MrktSimClient.Controls.MarketPlans
{
    class MarketPlanPicker : MrktSimControl
    {
        private string importTitle = "Create Market Plan and Components from File(s)";
        private string importNotes = "Each imported Excel file should contain one worksheet for each Component Plan.";

        private string importProjectString = "Import from Folder...";
        private string importItemString = "Import from File...";
        private string newItemString = "New...";
        private string copyItemString = "Copy";
        private string editItemString = "Edit...";
        private string shiftItemString = "Time Shift...";
        private string chprodItemString = "Change Product...";
        private string deleteItemString = "Delete";

        private string copyDlgTItle = "Confirm Market Plan Copy";
        private string copyMsg1 = "Are you sure you want to create a copy of the selected Market Plan?";
        private string copyMsg2 = "Plan to Copy:";
        private string copyMsg1M = "Are you sure you want to create copies of the selected Market Plans?";
        private string copyMsg2M = "Plans to Copy:";
        private string copyDoneMsg = "Market Plan Copied Successfully";
        private string copyDoneMsgM = "Market Plans Copied Successfully";

        private string deleteDlgTItle = "Confirm Market Plan Delete";
        private string delMsg1 = "Are you sure you delete the selected Market Plan?";
        private string delMsg2 = "Plan to Delete:";
        private string delMsg1M = "Are you sure you delete the selected Market Plans?";
        private string delMsg2M = "Plans to Delete:";
        private string delDoneMsg = "Market Plan Deleted Successfully";
        private string delDoneMsgM = "Market Plans Deleted Successfully";

        private string copyStatusMsg = "Copying Market Plans...";
        private string deleteStatusMsg = "Deleting Market Plans...";
        private string importStatusMsg = "Updating Market Plans...";
        private string editStatusMsg = "Updating Market Plans...";
        private string createStatusMsg = "Creating Market Plans...";

        private string importPath = null;

        private string OutputFilePattern = "*.xls";

        private CreateTopLevelPlan createEditDialog;
        private ImportNameAndDescr importDialog;
        private ChangePlanDates changeDatesDialog;
        private ChangePlanProduct changeProductDialog;

        private System.Data.DataView planView;

        //ModelDb.PlanType planType = ModelDb.PlanType.MarketPlan;
        private ArrayList selectedPlans;
        private MrktSimDBSchema.market_planRow selectedPlan;
        public MrktSimDBSchema.market_planRow SelectedPlan
        {
            get
            {
                return selectedPlan;
            }
        }

        public delegate void FireSelectedRowChanged();
        public event FireSelectedRowChanged SelectedRowChanged;

        private MarketPlanControlFilter filter;
        private CheckBox selectAllCheckBox;

        private PopupMenuLinkLabel planLinkLabel;
        private DataGridView planDataGridView;
        private CheckBox externalFactorsCheckBox;
        private Label countLabel;

        private DataTable topLevelPlanTable;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn ProductCol;
        private DataGridViewTextBoxColumn CouponsColumn;
        private DataGridViewTextBoxColumn DisplayCol;
        private DataGridViewTextBoxColumn DistCol;
        private DataGridViewTextBoxColumn MediaCol;
        private DataGridViewTextBoxColumn PriceCol;
        private DataGridViewTextBoxColumn IdCol;
        private CheckBox noScanCheckBox;
        private Hashtable brands;

        /// <summary>
        /// Creates a new MarketPlanPicker object.
        /// </summary>
        public MarketPlanPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            topLevelPlanTable = new DataTable("TopLevelPlans");

            DataColumn idCol = topLevelPlanTable.Columns.Add("id", typeof(int));
            topLevelPlanTable.PrimaryKey = new DataColumn[] { idCol };

            topLevelPlanTable.Columns.Add( "type", typeof( int ) );
            topLevelPlanTable.Columns.Add( "name", typeof( string ) );
            topLevelPlanTable.Columns.Add( "product", typeof( string ) );
            topLevelPlanTable.Columns.Add( "start_date", typeof( DateTime ) );
            topLevelPlanTable.Columns.Add( "end_date", typeof( DateTime ) );
            topLevelPlanTable.Columns.Add( "n_coupon", typeof( string ) );
            topLevelPlanTable.Columns.Add( "n_display", typeof( string ) );
            topLevelPlanTable.Columns.Add( "n_distribution", typeof( string ) );
            topLevelPlanTable.Columns.Add( "n_media", typeof( string ) );
            topLevelPlanTable.Columns.Add( "n_price", typeof( string ) );

            PlanView.Table = topLevelPlanTable;
            planDataGridView.DataSource = planView;

            planLinkLabel.AddItem( importProjectString, ProjectImport );
            planLinkLabel.AddItem( importItemString, ExcelImport );
            planLinkLabel.AddItem( newItemString, CreatePlan );
            planLinkLabel.AddItem( copyItemString, CopyPlan );
            planLinkLabel.AddItem( editItemString, EditPlan );
            if( MrktSim.DevlMode == true ) {
                planLinkLabel.AddItem( shiftItemString, ShiftPlan );
                planLinkLabel.AddItem( chprodItemString, ChangePlanProduct );
            }
            planLinkLabel.AddItem( deleteItemString, DeletePlan );

            planLinkLabel.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( EnablePopupMenuItems );
        }

        /// <summary>
        /// Sets the view filter object.
        /// </summary>
        public MarketPlanControlFilter Filter {
            set {
                filter = value;
            }
        }

        /// <summary>
        /// Returns the DataView that is bound to the market plans list.
        /// </summary>
        public System.Data.DataView PlanView {
            get {
                return planView;
            }
        }

        /// <summary>
        /// Returns the count of items in the market plans list.
        /// </summary>
        public int ItemCount {
            get {
                return planDataGridView.RowCount;
            }
        }

        /// <summary>
        /// Selects the specified item in the market plans list.
        /// </summary>
        public int SelectedIndex {
            set {
                Console.WriteLine( "### Set SelectedIndex to " + value );
                planDataGridView.Rows[ value ].Selected = true;
            }
        }

        /// <summary>
        /// Refreshes the market plans list.
        /// </summary>
        public override void Refresh() {
            planDataGridView.Refresh();
        }

        /// <summary>
        /// Sets the panel that is used to display the popup menu items
        /// </summary>
        /// <remarks>The popup menu panel is typically owned by a higher-level Control so that it won't be clipped at the edge of this control</remarks>
        public Panel PopupMenuPanel {
            set {
                this.planLinkLabel.PopupMenuPanel = value;
            }
        }

        /// <summary>
        /// Sets the database for the control and initializes the market plans list boc.
        /// </summary>
        public override MrktSimDb.ModelDb Db {
            set {
                base.Db = value;

                planView.Sort = "name";             // this initializes the sort arrow display of the planDataGridView

                this.UpdatePlanTable();
            }
        }

        		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.planView = new System.Data.DataView();
            this.selectAllCheckBox = new System.Windows.Forms.CheckBox();
            this.planLinkLabel = new Utilities.PopupMenuLinkLabel();
            this.planDataGridView = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CouponsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MediaCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalFactorsCheckBox = new System.Windows.Forms.CheckBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.noScanCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.planView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.planDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // selectAllCheckBox
            // 
            this.selectAllCheckBox.AutoSize = true;
            this.selectAllCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.selectAllCheckBox.Location = new System.Drawing.Point( 122, 4 );
            this.selectAllCheckBox.Name = "selectAllCheckBox";
            this.selectAllCheckBox.Size = new System.Drawing.Size( 71, 18 );
            this.selectAllCheckBox.TabIndex = 3;
            this.selectAllCheckBox.Text = "Select All";
            this.selectAllCheckBox.UseVisualStyleBackColor = true;
            this.selectAllCheckBox.CheckedChanged += new System.EventHandler( this.selectAllCheckBox_CheckedChanged );
            // 
            // planLinkLabel
            // 
            this.planLinkLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.planLinkLabel.BottomMargin = 4;
            this.planLinkLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.planLinkLabel.HighlightColor = System.Drawing.Color.LightSalmon;
            this.planLinkLabel.LeftMargin = 13;
            this.planLinkLabel.LinkText = "Market Plans";
            this.planLinkLabel.Location = new System.Drawing.Point( 3, -1 );
            this.planLinkLabel.MenuItemSpacing = 5;
            this.planLinkLabel.Name = "planLinkLabel";
            this.planLinkLabel.PopupBackColor = System.Drawing.Color.White;
            this.planLinkLabel.PopupFont = new System.Drawing.Font( "Arial", 8F );
            this.planLinkLabel.PopupParentLevelsAbove = 3;
            this.planLinkLabel.RightMargin = 5;
            this.planLinkLabel.Size = new System.Drawing.Size( 99, 21 );
            this.planLinkLabel.TabIndex = 4;
            this.planLinkLabel.TabMargin = 15;
            this.planLinkLabel.TopMargin = 9;
            // 
            // planDataGridView
            // 
            this.planDataGridView.AllowUserToAddRows = false;
            this.planDataGridView.AllowUserToDeleteRows = false;
            this.planDataGridView.AllowUserToResizeRows = false;
            this.planDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.planDataGridView.AutoGenerateColumns = false;
            this.planDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.planDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.planDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.planDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.planDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.planDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.planDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.planDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.NameCol,
            this.ProductCol,
            this.CouponsColumn,
            this.DisplayCol,
            this.DistCol,
            this.MediaCol,
            this.PriceCol,
            this.IdCol} );
            this.planDataGridView.DataSource = this.planView;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.planDataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.planDataGridView.GridColor = System.Drawing.Color.FromArgb( ((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))) );
            this.planDataGridView.Location = new System.Drawing.Point( 9, 27 );
            this.planDataGridView.Name = "planDataGridView";
            this.planDataGridView.ReadOnly = true;
            this.planDataGridView.RowHeadersVisible = false;
            this.planDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.planDataGridView.Size = new System.Drawing.Size( 494, 131 );
            this.planDataGridView.TabIndex = 5;
            this.planDataGridView.DoubleClick += new System.EventHandler( this.planListBox_DoubleClick );
            this.planDataGridView.SelectionChanged += new System.EventHandler( this.planDataGridView_SelectionChanged );
            // 
            // NameCol
            // 
            this.NameCol.DataPropertyName = "name";
            this.NameCol.HeaderText = "Name";
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            this.NameCol.ToolTipText = "Name of the Market Plan or External Factors";
            this.NameCol.Width = 140;
            // 
            // ProductCol
            // 
            this.ProductCol.DataPropertyName = "product";
            this.ProductCol.HeaderText = "Product";
            this.ProductCol.Name = "ProductCol";
            this.ProductCol.ReadOnly = true;
            this.ProductCol.Width = 85;
            // 
            // CouponsColumn
            // 
            this.CouponsColumn.DataPropertyName = "n_coupon";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CouponsColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.CouponsColumn.HeaderText = "Co";
            this.CouponsColumn.MinimumWidth = 2;
            this.CouponsColumn.Name = "CouponsColumn";
            this.CouponsColumn.ReadOnly = true;
            this.CouponsColumn.ToolTipText = "Co = number of Coupon components in the plan";
            this.CouponsColumn.Width = 26;
            // 
            // DisplayCol
            // 
            this.DisplayCol.DataPropertyName = "n_display";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DisplayCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.DisplayCol.HeaderText = "Dy";
            this.DisplayCol.MinimumWidth = 2;
            this.DisplayCol.Name = "DisplayCol";
            this.DisplayCol.ReadOnly = true;
            this.DisplayCol.ToolTipText = "Dy = number of Display components in the plan";
            this.DisplayCol.Width = 26;
            // 
            // DistCol
            // 
            this.DistCol.DataPropertyName = "n_distribution";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DistCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.DistCol.HeaderText = "Dn";
            this.DistCol.MinimumWidth = 2;
            this.DistCol.Name = "DistCol";
            this.DistCol.ReadOnly = true;
            this.DistCol.ToolTipText = "Dn = number of Distribution components in the plan";
            this.DistCol.Width = 26;
            // 
            // MediaCol
            // 
            this.MediaCol.DataPropertyName = "n_media";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MediaCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.MediaCol.HeaderText = "Me";
            this.MediaCol.MinimumWidth = 2;
            this.MediaCol.Name = "MediaCol";
            this.MediaCol.ReadOnly = true;
            this.MediaCol.ToolTipText = "Me = number of Media components in the plan";
            this.MediaCol.Width = 26;
            // 
            // PriceCol
            // 
            this.PriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PriceCol.DataPropertyName = "n_price";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PriceCol.DefaultCellStyle = dataGridViewCellStyle6;
            this.PriceCol.HeaderText = "Pr";
            this.PriceCol.MinimumWidth = 2;
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            this.PriceCol.ToolTipText = "Pr = number of Price components in the plan";
            // 
            // IdCol
            // 
            this.IdCol.DataPropertyName = "id";
            this.IdCol.HeaderText = "ID";
            this.IdCol.Name = "IdCol";
            this.IdCol.ReadOnly = true;
            this.IdCol.Visible = false;
            // 
            // externalFactorsCheckBox
            // 
            this.externalFactorsCheckBox.AutoSize = true;
            this.externalFactorsCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.externalFactorsCheckBox.Location = new System.Drawing.Point( 207, 4 );
            this.externalFactorsCheckBox.Name = "externalFactorsCheckBox";
            this.externalFactorsCheckBox.Size = new System.Drawing.Size( 105, 18 );
            this.externalFactorsCheckBox.TabIndex = 6;
            this.externalFactorsCheckBox.Text = "External Factors";
            this.externalFactorsCheckBox.UseVisualStyleBackColor = true;
            this.externalFactorsCheckBox.CheckedChanged += new System.EventHandler( this.externalFactorsCheckBox_CheckedChanged );
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.countLabel.Location = new System.Drawing.Point( 86, 5 );
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size( 33, 13 );
            this.countLabel.TabIndex = 7;
            this.countLabel.Text = "[100]";
            // 
            // noScanCheckBox
            // 
            this.noScanCheckBox.AutoSize = true;
            this.noScanCheckBox.Checked = true;
            this.noScanCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noScanCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.noScanCheckBox.Location = new System.Drawing.Point( 327, 4 );
            this.noScanCheckBox.Name = "noScanCheckBox";
            this.noScanCheckBox.Size = new System.Drawing.Size( 67, 18 );
            this.noScanCheckBox.TabIndex = 8;
            this.noScanCheckBox.Text = "No Scan";
            this.noScanCheckBox.UseVisualStyleBackColor = true;
            // 
            // MarketPlanPicker
            // 
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.Controls.Add( this.noScanCheckBox );
            this.Controls.Add( this.countLabel );
            this.Controls.Add( this.externalFactorsCheckBox );
            this.Controls.Add( this.planDataGridView );
            this.Controls.Add( this.planLinkLabel );
            this.Controls.Add( this.selectAllCheckBox );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.Name = "MarketPlanPicker";
            this.Size = new System.Drawing.Size( 511, 161 );
            this.Load += new System.EventHandler( this.MarketPlanPicker_Load );
            ((System.ComponentModel.ISupportInitialize)(this.planView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.planDataGridView)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
                #endregion

        public void ProjectImport() {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select MarketSim Data Directory"; 
            DialogResult resp = fbd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            string importFolder = fbd.SelectedPath;

            // get all the output files
            ArrayList importFiles = new ArrayList();
            string[] importDirs = new string[] { "", "Display", "Distribution", "Price", "Media" };
            foreach( string impDir in importDirs ) {
                string path = importFolder;
                if( impDir.Length > 0 ) {
                    path += "\\" + impDir;
                }
                if( Directory.Exists( path ) ) {
                    string[] pathFiles = Directory.GetFiles( path, OutputFilePattern );
                    Array.Sort( pathFiles );
                    for( int i = 0; i < pathFiles.Length; i++ ) {
                        pathFiles[ i ] = pathFiles[ i ].Substring( importFolder.Length + 1 );
                        importFiles.Add( pathFiles[ i ] );
                    }
                }
            }

            ImportProjectFileSelectionForm selForm = new ImportProjectFileSelectionForm( importFiles );
            DialogResult resp2 = selForm.ShowDialog();
            if( resp2 != DialogResult.OK ) {
                return;
            }

            // the import dialog does not allow file selection in this case
            importDialog = new ImportNameAndDescr( importTitle, importNotes, importPath, true, this.BackColor, "ImportMarketPlan" );
            importDialog.SetPreselectedFiles( selForm.GetSelectedFiles( importFolder ) );

            DialogResult resp3 = importDialog.ShowDialog();

            if( resp3 == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( ExcelImport_Part2, importStatusMsg, 0 );
            }
        }

        #region Excel Import
        /// <summary>
        /// Allows the user to create a market plan by importing data from Excel.
        /// </summary>
        private void ExcelImport() {
            // ask user for a name for plans
            importDialog = new ImportNameAndDescr( importTitle, importNotes, importPath, true, this.BackColor, "ImportMarketPlan" );

            DialogResult rslt = importDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( ExcelImport_Part2, importStatusMsg, 0 );
            }
        }

        private void ExcelImport_Part2(){
            string planName = importDialog.ObjName;
            string planDesc = importDialog.ObjDescription;

            PlanReader planReader = new PlanReader( theDb, planName );

            ErrorList errors = new ErrorList();
            ArrayList createdPlansList = new ArrayList();

            Utilities.Status.SetWaitCursor( this );

            using( ProcessStatus process = new ProcessStatus() ) {
                process.Text = "Importing Market Plans";
                process.ProcessType = "Importing:";

                process.Show();

                ModelDb.PlanType[] types = new ModelDb.PlanType[] { ModelDb.PlanType.Price,
														  ModelDb.PlanType.Display,
														  ModelDb.PlanType.Distribution,
														  ModelDb.PlanType.Coupons,
														  ModelDb.PlanType.Market_Utility,
														  ModelDb.PlanType.Media };

                double numPlans = importDialog.ObjDataFiles.Length * types.Length;
                double curIndex = 0;

                bool firstFile = true;
                foreach( string fileName in importDialog.ObjDataFiles ) {

                    int trimDex = fileName.LastIndexOf( @"\" );
                    int fLength = fileName.Length - trimDex - 1;

                    string fName = null;

                    if( fLength > 0 ) {
                        fName = fileName.Substring( trimDex + 1, fLength );
                    }

                    string errorMsg = ExcelReader.CheckIfFileOpen( fileName );
                    if( errorMsg != null ) {
                        MessageBox.Show( errorMsg, "File busy", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        continue;
                    }

                    foreach( ModelDb.PlanType type in types ) {
                        process.Progress( fName + "      " + type, curIndex / numPlans );
                        curIndex++;

                        ArrayList thisCreatedPlans;
                        errors.addErrors( planReader.CreatePlan( fileName, type, false, out thisCreatedPlans ) );
                        foreach( MrktSimDBSchema.market_planRow createdPlan in thisCreatedPlans ) {
                            createdPlansList.Add( createdPlan );
                        }
                    }

                    // create a dummy scenario row to pass the scenario ID into createTopLevelMarketPlan()
                    MrktSimDBSchema.scenarioRow tempScenarioRow = (MrktSimDBSchema.scenarioRow)theDb.Data.scenario.NewRow();
                    tempScenarioRow.scenario_id = filter.SelectedScenarioID;
                    planReader.createTopLevelMarketPlan( tempScenarioRow, firstFile );

                    if( errors.Count > 0 ) {
                        errors.addError( null, "Errors found in plan: " + fName, "Processing stopped" );
                        break;
                    }
                    firstFile = false;
                }
            }

            theDb.AllignPlansWithData();

            Status.ClearStatus( this );

            errors.Display();

            // since the list of market plans has changed, update the rest of the display
            foreach( DataGridViewRow drow in this.planDataGridView.Rows ) {
                int rowID = (int)drow.Cells[ "IdCol" ].Value;
                drow.Selected = false;
                foreach( MrktSimDBSchema.market_planRow createdPlan in createdPlansList ) {
                    if( rowID == createdPlan.id ) {
                        drow.Selected = true;
                        break;
                    }
                }
            }
            updateSelectedIDs();

            this.UpdatePlanTable();

            // since the list of market plans has changed, update the rest of the display
            if( SelectedRowChanged != null ) {
                SelectedRowChanged();
            }
        }
        #endregion

        /// <summary>
        /// Allows the user to create a new market plan.
        /// </summary>
        private void CreatePlan() {
            createEditDialog = new CreateTopLevelPlan( theDb );

            // a new plan will initially be configued to live in the current scenario only, with no components
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList used = relater.GetComponentsForPlan( -1, -1 );

            ArrayList users = relater.GetScenariosForPlan( -1 );
            foreach( MarketPlanControlRelater.Item item in users ) {
                if( filter.SelectedScenarioID == item.ID ) {                    // select just the current scenario
                    item.Selected = true;
                    break;
                }
            }
            createEditDialog.SetUsersAndUsedLists( users, used );

            DialogResult rslt = createEditDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( CreatePlan_Part2, createStatusMsg, 0 );
            }
        }

        private void CreatePlan_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            MrktSimDBSchema.market_planRow addedPlan = createEditDialog.CurrentPlan;

            UpdateScenarioPlansTree( addedPlan.id, createEditDialog.AllUserIDs, createEditDialog.RemovedUserIDs );
            UpdateMarketPlansTree( addedPlan.id, createEditDialog.AllUsedIDs, createEditDialog.RemovedUsedIDs );

            this.addPlanToPlanTable( addedPlan );

            // since the list of market plans has changed, update the rest of the display
            foreach( DataGridViewRow drow in this.planDataGridView.Rows ) {
                int rowID = (int)drow.Cells[ "IdCol" ].Value;
                drow.Selected = (rowID == addedPlan.id);
            }
            updateSelectedIDs();

            Status.ClearStatus( this );
        }
        
        /// <summary>
        /// Lets the user edit the selected market plan.
        /// </summary>
        private void EditPlan() {
            createEditDialog = new CreateTopLevelPlan( theDb );
            createEditDialog.CurrentPlan = selectedPlan;

            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList users = relater.GetScenariosForPlan( selectedPlan.id );
            ArrayList used = relater.GetComponentsForPlan( selectedPlan.id, selectedPlan.product_id );
            createEditDialog.SetUsersAndUsedLists( users, used );

            DialogResult rslt = createEditDialog.ShowDialog();

            if( rslt == DialogResult.OK ) 
            {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( EditPlan_Part2, editStatusMsg, 0 );
            }
        }

        private void EditPlan_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            MrktSimDBSchema.market_planRow editedPlan = createEditDialog.CurrentPlan;

            UpdateScenarioPlansTree( editedPlan.id, createEditDialog.AddedUserIDs, createEditDialog.RemovedUserIDs );
            UpdateMarketPlansTree( editedPlan.id, createEditDialog.AddedUsedIDs, createEditDialog.RemovedUsedIDs );

            // find out if the plan is still going to be visible
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            if( relater.ScenarioContainsPlan( editedPlan.id, filter.SelectedScenarioID ) == false ) {
                UpdatePlanTable();
                updateSelectedIDs();
            }
            else {
                updateSelectedIDs();
                // keep the edited item selected - update display directly for speed
                if( planDataGridView.SelectedRows.Count == 1 ) {
                    planDataGridView.SelectedRows[ 0 ].Cells[ "NameCol" ].Value = editedPlan.name;
                }
            }

            Status.ClearStatus( this );
        }

        /// <summary>
        /// Makes a copy of the selected market plan.
        /// </summary>
        private void CopyPlan() {
            if( this.selectedPlans.Count == 0 ) {
                return;
            }
            MrktSimDBSchema.market_planRow plan0 = (MrktSimDBSchema.market_planRow)this.selectedPlans[ 0 ];

            string msg1 = copyMsg1;
            string msg2 = copyMsg2;
            string msg3 = plan0.name;

            if( this.selectedPlans.Count > 1 ) {
                msg1 = copyMsg1M;
                msg2 = copyMsg2M;
                msg3 = String.Format( "{0} selected", this.selectedPlans.Count );
            }

            ConfirmDialog cDlg = new ConfirmDialog( msg1, msg2, msg3, copyDlgTItle );
            //ConfirmDialog cDlg = new ConfirmDialog( "Do you want to make a copy of the selected market plan?",
            //                                                           "Plan to Copy:", plan.name, "Confirm Market Plan Copy" );
            cDlg.SelectQuestionIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp == DialogResult.Yes ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( CopyPlan_Part2, copyStatusMsg, 0 );
            }
        }

        private void CopyPlan_Part2() {

            Utilities.Status.SetWaitCursor( this );

            // find out which scenarios use this plan
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList copiedPlans = new ArrayList();
            foreach( MrktSimDBSchema.market_planRow plan in this.selectedPlans ) {
                ArrayList selectedItemScenarios = relater.GetScenariosForPlan( plan.id );
                ArrayList selectedItemScenarioIDs = new ArrayList();
                foreach( MarketPlanControlRelater.Item item in selectedItemScenarios ) {
                    if( item.Selected ) {
                        selectedItemScenarioIDs.Add( item.ID );
                    }
                }

                MrktSimDBSchema.market_planRow copiedPlan = theDb.CopyPlan( plan );
                UpdateScenarioPlansTree( copiedPlan.id, selectedItemScenarioIDs, new ArrayList() );
                copiedPlans.Add( copiedPlan );
            }

            Status.ClearStatus( this );
            
            string msg4 = copyDoneMsg;
            if( this.selectedPlans.Count > 1 ) {
                msg4 = copyDoneMsgM;
            }
            CompletionDialog mDlg = new CompletionDialog( msg4 );
            mDlg.ShowDialog();
            UpdatePlanTable();

            // since the list of market plans has changed, update the rest of the display
            foreach( DataGridViewRow drow in this.planDataGridView.Rows ) {
                int rowID = (int)drow.Cells[ "IdCol" ].Value;
                drow.Selected = false;
                foreach( MrktSimDBSchema.market_planRow copiedPlan in copiedPlans ) {
                    if( rowID == copiedPlan.id ) {
                        drow.Selected = true;
                        break;
                    }
                }
            }
            updateSelectedIDs();
        }

        /// <summary>
        /// Deletes the selected market plan.
        /// </summary>
        private void DeletePlan() {
            if( this.selectedPlans.Count == 0 ) {
                return;
            }
            MrktSimDBSchema.market_planRow plan0 = ( MrktSimDBSchema.market_planRow)this.selectedPlans[ 0 ];

            string msg1 = delMsg1;
            string msg2 = delMsg2;
            string msg3 = plan0.name;
            string msg4 = delDoneMsg;

            if( this.selectedPlans.Count > 1 ) {
                msg1 = delMsg1M;
                msg2 = delMsg2M;
                msg3 = String.Format( "{0} selected", this.selectedPlans.Count );
                msg4 = delDoneMsgM;
            }

            ConfirmDialog cDlg = new ConfirmDialog( msg1, msg2, msg3, deleteDlgTItle);
            cDlg.SelectWarningIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp == DialogResult.Yes ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( DeletePlan_Part2, deleteStatusMsg, 0 );
            }
        }

        private void DeletePlan_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            foreach( MrktSimDBSchema.market_planRow plan in selectedPlans ) {
                plan.Delete();
            }

            Status.ClearStatus( this );

            string msg4 = delDoneMsg;
            if( this.selectedPlans.Count > 1 ) {
                msg4 = delDoneMsgM;
            }
            CompletionDialog mDlg = new CompletionDialog( msg4 );
            mDlg.ShowDialog();

            UpdatePlanTable();

            // since the list of market plans has changed, update the rest of the display
            updateSelectedIDs();
        }

        private void ShiftPlan() {
            if( this.selectedPlans.Count == 0 ) {
                return;
            }
            changeDatesDialog = new ChangePlanDates( theDb, this.selectedPlans );
            changeDatesDialog.SetPlanType( ModelDb.PlanType.MarketPlan );
            DialogResult resp = changeDatesDialog.ShowDialog();
            if( resp == DialogResult.OK ) {
                updateSelectedIDs();
            }
        }

        private void ChangePlanProduct() {
            if( this.selectedPlans.Count == 0 ) {
                return;
            }
            changeProductDialog = new ChangePlanProduct( theDb, this.selectedPlans );
            changeProductDialog.SetPlanType( ModelDb.PlanType.MarketPlan );
            DialogResult resp = changeProductDialog.ShowDialog();
            if( resp == DialogResult.OK ) {
                this.UpdatePlanTable();
                updateSelectedIDs();
            }
        }

        #region Scenario and Market-Plan Tree Updating
        /// <summary>
        /// Makes the needed updates to the scenario_market_plan table for a new or edited market plan.
        /// </summary>
        /// <param name="addedPlanID"></param>
        /// <param name="addedScenarioIDs"></param>
        /// <param name="removedScenarioIDs"></param>
        private void UpdateScenarioPlansTree( int marketPlanID, ArrayList addedScenarioIDs, ArrayList removedScenarioIDs ) {
            DataTable table = theDb.Data.scenario_market_plan;
            if( addedScenarioIDs.Count == 0 &&  removedScenarioIDs.Count == 0 ) 
            {
                return;
            }

            // remove rows if any are specified
            foreach( int removeID in removedScenarioIDs ) {
                string removeFilter = String.Format( "market_plan_id={0} AND scenario_id={1}", marketPlanID, removeID );
                DataRow[] rows = table.Select( removeFilter );
                foreach( DataRow row in rows ) {
                    Console.WriteLine( "...del row: {0}", removeFilter );
                    row.Delete();
                    //table.Rows.Remove( row );
                }
            }
            // add rows if any are specified
            foreach( int addID in addedScenarioIDs ) {
                MrktSimDBSchema.scenario_market_planRow newRow = (MrktSimDBSchema.scenario_market_planRow)table.NewRow();
                newRow.scenario_id = addID;
                newRow.market_plan_id = marketPlanID;
                newRow.model_id = theDb.ModelID;
                Console.WriteLine( "...add row: scenario_id={0} market_plan_id={1} modei_id={2}", addID, marketPlanID, theDb.ModelID );
                table.Rows.Add( newRow );
            }
        }

        /// <summary>
        /// Makes the needed updates to the market_plan_tree table for a new or edited market plan.
        /// </summary>
        /// <param name="addedPlanID"></param>
        /// <param name="addedScenarioIDs"></param>
        /// <param name="removedScenarioIDs"></param>
        private void UpdateMarketPlansTree( int marketPlanID, ArrayList addedComponentIDs, ArrayList removedComponentIDs ) {
            DataTable table = theDb.Data.market_plan_tree;
            if( (addedComponentIDs.Count + removedComponentIDs.Count) == 0 ) {
                return;
            }
            Console.WriteLine( "UpdateMarketPlansTree() adding {0} and removing {1} rows from market_plan_tree table (market plan id = {2})",
                                                                                            addedComponentIDs.Count, removedComponentIDs.Count, marketPlanID );

            // remove rows if any are specified
            foreach( int removeID in removedComponentIDs ) {
                string removeFilter = String.Format( "parent_id={0} AND child_id={1}", marketPlanID, removeID );
                DataRow[] rows = table.Select( removeFilter );
                foreach( DataRow row in rows ) {
                    Console.WriteLine( "...del row: {0}", removeFilter );
                    row.Delete();
                }
            }
            // add rows if any are specified
            foreach( int addID in addedComponentIDs ) {
                MrktSimDBSchema.market_plan_treeRow newRow = (MrktSimDBSchema.market_plan_treeRow)table.NewRow();
                newRow.child_id = addID;
                newRow.parent_id = marketPlanID;
                newRow.model_id = theDb.ModelID;
                Console.WriteLine( "...add row: parent_id={0} child_id={1} modei_id={2}", marketPlanID, addID, theDb.ModelID );
                table.Rows.Add( newRow );
            }
        }
        #endregion

        /// <summary>
        /// Responds to a change in the selected item in the list of market plans.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Fires a SelectedRowChanged event unless the selected row index is -1.</remarks>
        private void planDataGridView_SelectionChanged( object sender, EventArgs e ) {
            selectAllCheckBox.CheckedChanged -= new EventHandler( selectAllCheckBox_CheckedChanged );
            selectAllCheckBox.Checked = false;
            selectAllCheckBox.CheckedChanged += new EventHandler( selectAllCheckBox_CheckedChanged );

            updateSelectedIDs();
        }

        /// <summary>
        /// Updates the filter to reflect the selected rows.  Also enables appropriate popup menu items, and sets selectedPlan and selectedPlans values.
        /// Fires a SelectedRowChanged() callback.
        /// </summary>
        private void updateSelectedIDs() {
            updateSelectedIDs( true );
        }

        /// <summary>
        /// Updates the filter to reflect the selected rows.  Also enables appropriate popup menu items, and sets selectedPlan and selectedPlans values.
        /// Fires a SelectedRowChanged() callback if callSelectedRowChanged is true.
        /// </summary>
        /// <param name="callSelectedRowChanged"></param>
        private void updateSelectedIDs( bool callSelectedRowChanged )
        {
            //Console.WriteLine( "updateSelectedIDs()...processing {0} selected rows", planDataGridView.SelectedRows.Count );
            if( filter == null ) {
                return;
            }
            filter.MarketPlanIDs = new int[ planDataGridView.SelectedRows.Count ];

            int ii = 0;
            foreach( DataGridViewRow row in planDataGridView.SelectedRows ){
                filter.MarketPlanIDs[ ii ] = (int)row.Cells[ "IdCol" ].Value;
                ii++;
            }

            selectedPlans = new ArrayList();
            if( planDataGridView.SelectedRows.Count == 1 ) {
                DataGridViewRow row = planDataGridView.SelectedRows[ 0 ];
                selectedPlan = theDb.Data.market_plan.FindByid( (int)row.Cells[ "IdCol" ].Value );
            }
            else {
                selectedPlan = null;
            }
            foreach( DataGridViewRow row in planDataGridView.SelectedRows ) {
                selectedPlans.Add( (MrktSimDBSchema.market_planRow) theDb.Data.market_plan.FindByid( (int)row.Cells[ "IdCol" ].Value ) );
            }

            // call the callback if appropriate
            if( SelectedRowChanged != null && callSelectedRowChanged == true )
            {
                SelectedRowChanged();
            }
        }

        /// <summary>
        /// configure the popup menu for the current selection
        /// </summary>
        private void EnablePopupMenuItems(){
            bool enableImport = true;
            bool enableNew = true;
            bool enableCopy = true;
            bool enableEdit = true;
            bool enableShift = true;
            bool enableChProd = true;
            bool enableDelete = true;

            if( filter.SelectedScenarioID == -1 ) {
                enableImport = false;
                enableNew = false;
            }
            if( this.selectedPlans.Count == 0 ) {
                enableCopy = false;
                enableEdit = false;
                enableDelete = false;
                enableShift = false;
                enableChProd = false;
            }
            else if( this.selectedPlans.Count > 1 ) {
                // multiple selections
                enableEdit = false;
            }

            if( this.selectedPlan != null && this.selectedPlan.type == (int)ModelDb.PlanType.ProdEvent ) {
                enableCopy = false;
                enableEdit = false;
            }

            if( ((ModelEditor)this.ParentForm).ProcessActive == true ) {
                enableImport = false;
                enableNew = false;
                enableCopy = false;
                enableEdit = false;
                enableDelete = false;
                enableShift = false;
                enableChProd = false;
            }

            planLinkLabel.EnableAllLinks();
            if( !enableImport ) {
                planLinkLabel.DisableLink( importItemString );
                planLinkLabel.DisableLink( importProjectString );
            }
            if( !enableNew ) {
                planLinkLabel.DisableLink( newItemString );
            }
            if( !enableCopy ) {
                planLinkLabel.DisableLink( copyItemString );
            }
            if( !enableEdit ) {
                planLinkLabel.DisableLink( editItemString );
            }
            if( !enableShift ) {
                planLinkLabel.DisableLink( shiftItemString );
            }
            if( !enableChProd ) {
                planLinkLabel.DisableLink( chprodItemString );
            }
            if( !enableDelete ) {
                planLinkLabel.DisableLink( deleteItemString );
            }
        }

        /// <summary>
        /// Clears the selection in the market plans list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketPlanPicker_Load( object sender, EventArgs e ) {
            Console.WriteLine( "MarketPlanPicker_Load() - clearing planDataGridView" );
            planDataGridView.ClearSelection();
        }

        /// <summary>
        /// Responds to a double-click by launching the edit dialog for the double-clicked plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void planListBox_DoubleClick( object sender, EventArgs e ) {
            // disallow editing of External Factors items
            if( this.selectedPlan != null && this.selectedPlan.type != (int)ModelDb.PlanType.ProdEvent ) {
                EditPlan();
            }
        }

        /// <summary>
        /// Recomputes and redisplays the component count for all displayed plans -- does nothing else
        /// </summary>
        public void UpdateSelectedPlanStats() {
            if( this.noScanCheckBox.Checked == false ) {
                Console.WriteLine( "In UpdateSelectedPlanStats()(picker)..." );
                foreach( DataGridViewRow drow in this.planDataGridView.Rows ) {
                    int selID = (int)drow.Cells[ "IdCol" ].Value;
                    foreach( DataRow tblrow in this.topLevelPlanTable.Rows ) {
                        if( (int)tblrow[ "id" ] == selID ) {
                            UpdateComponentCountsInPlanTable( tblrow );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears and updates the top level plan table
        /// </summary>
        public void UpdatePlanTable() {
            UpdatePlanTable( null );
        }
        
        /// <summary>
        /// Clears and updates the top level plan table
        /// </summary>
        public void UpdatePlanTable( ArrayList plansToSelect ){
            bool selectAll = this.selectAllCheckBox.Checked;

            this.planDataGridView.SelectionChanged -= new EventHandler( planDataGridView_SelectionChanged );

            // turn off the display while we update the data
            planDataGridView.DataSource = null;
            this.topLevelPlanTable.BeginLoadData();

            this.topLevelPlanTable.Clear();

            if( filter.SelectedScenarioID != -2 ) {
                // check if plan is in selected list of products
                planDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
                ArrayList allUnusedPlans = theDb.AllMarketPlansNotInAnyScenario();
                if( filter.ProductIDs != null ) {
                    bool addedAllProducts = false;
                    foreach( int prodID in filter.ProductIDs ) {
                        if( prodID == -1 ) {
                            addedAllProducts = true;
                        }
                        string query = null;
                        if( externalFactorsCheckBox.Checked == true ) {
                            // show market plans and external factors
                            query = "(type = 0 OR type = 5) AND product_id = " + prodID;
                        }
                        else {
                            // show market plans only
                            query = "type = 0 AND product_id = " + prodID;
                        }

                        foreach( MrktSimDBSchema.market_planRow plan in theDb.Data.market_plan.Select( query, "name", DataViewRowState.CurrentRows ) ) {
                            if( allUnusedPlans.Contains( plan ) == false ) {       // be sure the All scenario doesn't show unused items.
                                addPlanToPlanTable( plan );
                                ////Console.WriteLine( "calling addPlanToPlanTable({2}) ...data rows = {0}, selected rows count = {1}",
                                    ////this.topLevelPlanTable.Rows.Count, planDataGridView.SelectedRows.Count, "x" );
                            }
                        }
                    }

                    Console.WriteLine( "UpdatePlanTable() done adding regular plans ...selected rows count = {0}", planDataGridView.SelectedRows.Count );

                    if( addedAllProducts == false ) {
                        // include External Factors plans for ALL products
                        if( externalFactorsCheckBox.Checked == true ) {
                            string queryAll = "type = 5 AND product_id = -1";

                            DataRow[] testRows = theDb.Data.market_plan.Select( queryAll, "name", DataViewRowState.CurrentRows );
                            foreach( MrktSimDBSchema.market_planRow plan in theDb.Data.market_plan.Select( queryAll, "name", DataViewRowState.CurrentRows ) ) {
                                addPlanToPlanTable( plan );
                            }
                        }
                    }
                }
            }
            else {
                // filter.SelectedScenarioID is -2 -- show all unassigned plans
                ArrayList unassignedPlans = theDb.AllMarketPlansNotInAnyScenario();
                planDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.Red;

                bool allProds = false;
                ArrayList prodList = new ArrayList();
                if( filter.ProductIDs != null ) {
                    foreach( int prodID in filter.ProductIDs ) {
                        if( prodID == -1 ) {
                            allProds = true;
                        }
                        else {
                            prodList.Add( prodID );
                        }
                    }
                }

                foreach( MrktSimDBSchema.market_planRow plan in unassignedPlans ) {
                    if( allProds || prodList.Contains( plan.product_id ) ) {
                        addPlanToPlanTable( plan );
                        ////Console.WriteLine( "calling addPlanToPlanTable({1}) ... row count = {0}", this.topLevelPlanTable.Rows.Count, "y" );
                    }
                }
            }

            //Console.WriteLine( "UpdatePlanTable() pre special selection...row count = {0} data table rows = {1}  selected count = {2}", 
            //    planDataGridView.RowCount, this.topLevelPlanTable.Rows.Count, planDataGridView.SelectedRows.Count );

            // turn on the display updates again
            this.topLevelPlanTable.EndLoadData();
            planDataGridView.DataSource = planView;
            
            // establish the initial selection(s)
            this.planDataGridView.ClearSelection();

            if( selectAll )
            {
                checkAll();
            }
            else if( plansToSelect != null ) {
                foreach( MrktSimDBSchema.market_planRow xfacComp in plansToSelect ) {
                    string planName = xfacComp.name;
                    for( int i = 0; i < planDataGridView.Rows.Count; i++ ) {
                        DataGridViewRow drv = planDataGridView.Rows[ i ];

                        if( (string)drv.Cells[ "NameCol" ].Value == planName ) {
                            planDataGridView.Rows[ i ].Selected = true;
                        }
                    }
                }
            }
            else {
                // default situation - select the first row only
                if( this.planDataGridView.Rows.Count > 0 ) {
                    this.planDataGridView.Rows[ 0 ].Selected = true;
                    this.planDataGridView.FirstDisplayedScrollingRowIndex = 0;
                }
            }

            this.planDataGridView.SelectionChanged += new EventHandler( planDataGridView_SelectionChanged );
            //Console.WriteLine( "UpdatePlanTable() almost done...selected rows count = {0}", planDataGridView.SelectedRows.Count );

            this.countLabel.Text = String.Format( "[{0}]", planDataGridView.Rows.Count );

            updateSelectedIDs( false ); 
        }

        /// <summary>
        /// adds a plan to table after checking filter contraints
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private bool addPlanToPlanTable(MrktSimDBSchema.market_planRow plan)
        {
            if (plan.end_date < filter.StartDate ||
                plan.start_date > filter.EndDate)
            {
                return false;
            }

            if( filter.SelectedScenarioID != Database.AllID && filter.SelectedScenarioID != -2 ) {
                string query = "scenario_id = " + filter.SelectedScenarioID + " AND market_plan_id = " + plan.id;

                if( theDb.Data.scenario_market_plan.Select( query, "", DataViewRowState.CurrentRows ).Length == 0 ) {
                    return false;
                }
            }

            // determine how many compoents are in the plan
            bool isExternalFactors = true;
            int nCoupon = 0;
            int nDisplay = 0;
            int nDistribution = 0;
            int nPrice = 0;
            int nMedia = 0;

            if( plan.type == (int)ModelDb.PlanType.MarketPlan ) {
                isExternalFactors = false;
                if( this.noScanCheckBox.Checked == false ) {
                    CountAllPlanComponents( plan.id, out nCoupon, out nDisplay, out nDistribution, out nMedia, out nPrice );
                }
            }

            DataRow row = topLevelPlanTable.NewRow();

            row[ "id" ] = plan.id;
            row[ "type" ] = plan.type;
            row[ "name" ] = plan.name;
            row[ "product" ] = GetProductName( plan.product_id );
            row[ "start_date" ] = plan.start_date;
            row["end_date"] = plan.end_date;
            row[ "n_coupon" ] = ComponentDisplayVal( nCoupon, isExternalFactors );
            row[ "n_price" ] = ComponentDisplayVal( nPrice, isExternalFactors );
            row[ "n_display" ] = ComponentDisplayVal( nDisplay, isExternalFactors );
            row[ "n_distribution" ] = ComponentDisplayVal( nDistribution, isExternalFactors );
            row[ "n_media" ] = ComponentDisplayVal( nMedia, isExternalFactors );
            
            topLevelPlanTable.Rows.Add(row);

            return true;
        }

        /// <summary>
        /// Updates just the component-count columns of the given DataRow.
        /// </summary>
        /// <param name="topLevelPlanRow"></param>
        private void UpdateComponentCountsInPlanTable( DataRow topLevelPlanRow ) {
            bool isExternalFactors = true;
            int nCoupon = 0;
            int nDisplay = 0;
            int nDistribution = 0;
            int nPrice = 0;
            int nMedia = 0;

            int planID = (int)topLevelPlanRow[ "id" ];
            int planType = (int)topLevelPlanRow[ "type" ];
            if( planType == (int)ModelDb.PlanType.MarketPlan ) {
                isExternalFactors = false;
                CountAllPlanComponents( planID, out nCoupon, out nDisplay, out nDistribution, out nMedia, out nPrice );
            }

            topLevelPlanRow[ "n_coupon" ] = ComponentDisplayVal( nCoupon, isExternalFactors );
            topLevelPlanRow[ "n_price" ] = ComponentDisplayVal( nPrice, isExternalFactors );
            topLevelPlanRow[ "n_display" ] = ComponentDisplayVal( nDisplay, isExternalFactors );
            topLevelPlanRow[ "n_distribution" ] = ComponentDisplayVal( nDistribution, isExternalFactors );
            topLevelPlanRow[ "n_media" ] = ComponentDisplayVal( nMedia, isExternalFactors );
        }


        private string ComponentDisplayVal( int nComponents, bool isExternalFactors ) {
            if( isExternalFactors == false ) {
                if( nComponents > 0 ) {
                    return nComponents.ToString();
                }
                else {
                    return "";
                }
            }
            else {
                // an external factors item
                return "-";
            }
        }

        /// <summary>
        /// Detemines the number of Component Plans in the given top-level Market Plan
        /// </summary>
        /// <param name="planID"></param>
        /// <param name="nCoupon"></param>
        /// <param name="nDisplay"></param>
        /// <param name="nDistribution"></param>
        /// <param name="nMedia"></param>
        /// <param name="nPrice"></param>
        private void CountAllPlanComponents( int planID, out int nCoupon, out int nDisplay, out int nDistribution, out int nMedia, out int nPrice ) {
            string countQuery1 = String.Format( "parent_id = {0}", planID );
            ArrayList countQuerys2 = new ArrayList();
            nCoupon = 0;
            nDisplay = 0;
            nDistribution = 0;
            nMedia = 0;
            nPrice = 0;

            int sectionCount = 0;
            string countQuery2 = "";

            MrktSimDBSchema.market_plan_treeRow[] trows = (MrktSimDBSchema.market_plan_treeRow[]) theDb.Data.market_plan_tree.Select( countQuery1 );

            int sectionSize = 100;
            // split the market plan query IDs into bits that aren't unreasonably big
            for( int r = 0; r < trows.Length; r++ ) {
                MrktSimDBSchema.market_plan_treeRow trow = trows[ r ];
                if( countQuery2.Length == 0 ) {
                    countQuery2 = String.Format( "id = {0}", trow.child_id );
                }
                else {
                    countQuery2 += String.Format( " OR id = {0}", trow.child_id );
                }
                sectionCount += 1;
                if( sectionCount == sectionSize ) {
                    countQuerys2.Add( countQuery2 );
                    countQuery2 = "";
                    sectionCount = 0;
                }
            }
            if( sectionCount > 0 ) {
                countQuerys2.Add( countQuery2 );
            }

            // loop over each of the query sections
            foreach( string checkQuery2 in countQuerys2 ){
                foreach( MrktSimDBSchema.market_planRow mprow in theDb.Data.market_plan.Select( checkQuery2 ) ) {
                    switch( (ModelDb.PlanType)mprow.type ) {
                        case ModelDb.PlanType.Coupons:
                            nCoupon += 1;
                            break;
                        case ModelDb.PlanType.Display:
                            nDisplay += 1;
                            break;
                        case ModelDb.PlanType.Distribution:
                            nDistribution += 1;
                            break;
                        case ModelDb.PlanType.Media:
                            nMedia += 1;
                            break;
                        case ModelDb.PlanType.Price:
                            nPrice += 1;
                            break;
                    }
                }
            }
        }

          /// <summary>
        /// Handles a change in the select-all checlbox status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllCheckBox_CheckedChanged( object sender, EventArgs e ) 
        {
            this.planDataGridView.SelectionChanged -= new EventHandler( planDataGridView_SelectionChanged );
            bool sel = selectAllCheckBox.Checked;
            if( sel == false ) {
                planDataGridView.ClearSelection();
            }
            else 
            {
                checkAll();   
            }

            this.planDataGridView.SelectionChanged += new EventHandler( planDataGridView_SelectionChanged );

            updateSelectedIDs();
        }

        private void checkAll()
        {
            planDataGridView.SelectAll();
        }

        private void externalFactorsCheckBox_CheckedChanged( object sender, EventArgs e ) {

            Utilities.Status.SetWaitCursor( this );
            this.UpdatePlanTable();
            Status.ClearWaitCursor( this );
        }

        private String GetProductName( int productID ) {
            if( brands == null ) {
                brands = new Hashtable();
                foreach( MrktSimDBSchema.productRow prow in theDb.Data.product ) {
                    brands.Add( prow.product_id, prow.product_name );
                }
            }
            return (string)brands[ productID ];
        }
    }
}
