//#define EDIT_DATA

using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
    class MarketPlanComponentPicker : MrktSimControl
    {
        private string importItemString = "Import from Excel...";
        private string newItemString = "New...";
        private string copyItemString = "Copy";
        private string editItemString = "Edit";
        private string editDataItemString = "Edit Data";
        private string shiftItemString = "Time Shift...";
        // private string chprodItemString = "Change Product...";
        private string paramsItemString = "Params";
        private string unparameterizeItemString = "Unparameterize...";
        private string awarPersuasItemString = "Set Awareness & Persuasion...";
        private string deleteItemString = "Delete";

        private string unassTitle = "Unassigned Components";
        private string unassMsg = "WARNING: Brand Mismatch\r\n\r\n{0} of the {1} imported {2} Plan Components are for Brands that do not match\r\nthe Brand of any selected Market Plan(s).\r\n\r\n" +
            "Ignore these component(s)? \r\n\r\n(If you answer No, the {2} Plan Components with unmatched Brands will be imported,\r\n but not assigned to any Market Plan).";
        private string unassAllTitle = "Unassigned Components";
        private string unassAllMsg = "ERROR: Brand Mismatch\r\n\r\nNone of the {0} imported {1} Plan Components are for a Brand that matches\r\nthe Brand of any selected Market Plan(s).\r\n\r\n" +
            "OK to abort Import? \r\n\r\n(If you answer No, the {1} Plan Components will be imported,\r\n but not assigned to any Market Plan).";

        private string copyDlgTItle = "Confirm Plan Component Copy";
        private string copyDlgMsg = "Do you want to create a copy of the selected Plan Component?";
        private string copyDlgMsgM = "Do you want to create a copy of the selected Plan Components?";
        private string copyDlgMsg2 = "Plan Component to Copy:";
        private string copyDlgMsg2M = "Plan Components to Copy:";
        private string copyStatusMsg = "Copying Plan Component...";
        private string copyStatusMsgM = "Copying Plan Components...";

        private string deleteDlgTItle = "Confirm Plan Component Delete";
        private string deleteDlgMsg = "Do you want to delete the selected Plan Component?";
        private string deleteDlgMsgM = "Do you want to delete the selected Plan Components?";
        private string deleteDlgMsg2 = "Plan Component to Delete:";
        private string deleteDlgMsg2M = "Plan Components to Delete:";
        private string deleteStatusMsg = "Deleting Plan Component...";
        private string deleteStatusMsgM = "Deleting Plan Components...";
        private string deleteDoneMsg = "Plan Component Deleted Successfully";
        private string deleteDoneMsgM = "Plan Components Deleted Successfully";

        private string importStatusMsg = "Importing Plan Component)...";
        private string importDelUnassStatusMsg = "Deleting unassigned Plan Components...";
        private string editStatusMsg = "Updating Plan Component...";
        private string createStatusMsg = "Creating Plan Component...";
        private string chgTypeStatus = "Selecting Plan Components...";
        //private string editDataStatusMsg = "Updating Plan Component Data...";

        private ChangePlanDates changeDatesDialog;
        private ChangePlanProduct changeProductDialog;

        private DataTable componentPlanTable;
        private System.Data.DataView componentView;

        private CheckBox selectAllCheckBox;
        private Label allComponentsLabel;

        private CreateComponentPlan2 createEditDialog;
        private ImportNameAndDescr importDialog;

#if EDIT_DATA        
        private EditComponentData editDataDialog;
#endif

        private MrktSimDBSchema.market_planRow selectedComponent;
        private ArrayList selectedComponents = new ArrayList();
     
        public delegate void FireSelectedRowChanged( object specialCode );
        public event FireSelectedRowChanged SelectedRowChanged;

        private MarketPlanControlFilter filter;
        private ArrayList unassignedComponents;

        private ModelDb.PlanType defaultPlanType = ModelDb.PlanType.MarketPlan;
        private DataGridView componentDataGridView;

        private PopupMenuLinkLabel componentLinkLabel;
        private ComboBox typeComboBox;

        private ArrayList createdPlanRows;
        private string importPath = null;
        private Label countLabel;
        private bool multipleTypesSelected = false;


        private Hashtable brands;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn ProductCol;
        private DataGridViewTextBoxColumn TypeCol;
        private DataGridViewTextBoxColumn Values;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn IdCol;
        private CheckBox noScanCheckBox;

        public ModelDb.PlanType DefaultPlanType
        {
            set
            {
                defaultPlanType = value;
            }
        }

        private ModelDb.PlanType currentType = ModelDb.PlanType.MarketPlan;
        public ModelDb.PlanType PlanType
        {
            get
            {
                return currentType;
            }
        }
        
        /// <summary>
        /// Create a new MarketPlanComponentPicker object.
        /// </summary>
        public MarketPlanComponentPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            componentPlanTable = new DataTable("ComponentPlans");

            DataColumn idCol = componentPlanTable.Columns.Add("id", typeof(int));
            componentPlanTable.PrimaryKey = new DataColumn[] { idCol };

            componentPlanTable.Columns.Add( "name", typeof( string ) );
            componentPlanTable.Columns.Add( "product", typeof( string ) );
            componentPlanTable.Columns.Add( "type_name", typeof( string ) );
            componentPlanTable.Columns.Add( "start_date", typeof( DateTime ) );
            componentPlanTable.Columns.Add("end_date", typeof(DateTime));
            componentPlanTable.Columns.Add("type", typeof(ModelDb.PlanType));
            componentPlanTable.Columns.Add( "status", typeof( string ) );
            componentPlanTable.Columns.Add( "num_values", typeof( int ) );

            componentView.Table = componentPlanTable;

            componentDataGridView.DataSource = componentView;

            componentLinkLabel.AddItem( importItemString, ExcelImport );
            //componentLinkLabel.AddItem( newItemString, CreatePlan );   -- disable until we determine some way of creating a component with some data
            componentLinkLabel.AddItem( copyItemString, CopyPlans );
            componentLinkLabel.AddItem( editItemString, EditPlan );
#if EDIT_DATA
            componentLinkLabel.AddItem( editDataItemString, EditPlanData );
#endif
            componentLinkLabel.AddItem( shiftItemString, ShiftPlan );
            //componentLinkLabel.AddItem( chprodItemString, ChangePlanProduct );
            componentLinkLabel.AddItem( paramsItemString, ShowParams );
            if( MrktSim.DevlMode == true ) {
                componentLinkLabel.AddItem( awarPersuasItemString, SetPlansAwarnessPerswuasion );
            }
            componentLinkLabel.AddItem( unparameterizeItemString, UnparameterizePlans );
            componentLinkLabel.AddItem( deleteItemString, DeletePlans );

            componentLinkLabel.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( EnablePopupMenuItems );
        }

        /// <summary>
        /// Sets the view filter to adjust when plans are edited/copied/etc.
        /// </summary>
        public MarketPlanControlFilter Filter {
            set {
                filter = value;
            }
        }

        /// <summary>
        /// Sets the component picker mode for the specified plan type (enables apppropriate items in the popup menu)
        /// </summary>
        private void EnablePopupMenuItems()
        {
            // configure the popup menu for the current selection
            bool enableImport = true;
            bool enableNew = true;
            bool enableCopy = true;
            bool enableEdit = true;
            bool enableEditData = true;
            bool enableShift = true;
           // bool enableChProd = true;
            bool enableDelete = true;
            bool enableParams = true;
            bool enableUnparameterize = true;

            if( filter.MarketPlanIDs == null || filter.MarketPlanIDs.Length == 0  ) {
                // no market plans selected
                if( this.defaultPlanType != Database.PlanType.ProdEvent ) {
                    enableImport = false;
                }
                enableNew = false;

            }

            if( filter.SelectedScenarioID == -1 ) {
                enableImport = false;
            }

            if( this.defaultPlanType == ModelDb.PlanType.MarketPlan ) {
                enableImport = false;
                enableNew = false;
            }

            if( this.selectedComponents.Count == 0 ) {
                // no component selected
                enableCopy = false;
                enableEdit = false;
                enableDelete = false;
                enableEditData = false;
                enableParams = false;
                enableShift = false;
               // enableChProd = false;
                enableUnparameterize = false;
            }
            else if( this.selectedComponents.Count > 1 ) {
                // multiple component selections
                enableEdit = false;
                enableParams = false;
            }

            if( multipleTypesSelected ) {
                enableEditData = false;
            }

            if( ((ModelEditor)this.ParentForm).ProcessActive == true ) {
                enableImport = false;
                enableNew = false;
                enableCopy = false;
                enableEdit = false;
                enableEditData = false;
                enableShift = false;
                // enableChProd = false;
                enableDelete = false;
                enableParams = false;
                enableUnparameterize = false;
            }

            componentLinkLabel.EnableAllLinks();
            if( !enableImport ) {
                componentLinkLabel.DisableLink( importItemString );
            }
            if( !enableNew ) {
                componentLinkLabel.DisableLink( newItemString );
            }
            if( !enableCopy ) {
                componentLinkLabel.DisableLink( copyItemString );
            }
            if( !enableEdit ) {
                componentLinkLabel.DisableLink( editItemString );
            }
            if( !enableEditData ) {
                componentLinkLabel.DisableLink( editDataItemString );
            }
            if( !enableShift ) {
                componentLinkLabel.DisableLink( shiftItemString );
            }
            //if( !enableChProd ) {
            //    componentLinkLabel.DisableLink( chprodItemString );
            //}
            if( !enableParams ) {
                componentLinkLabel.DisableLink( paramsItemString );
            }
            if( !enableUnparameterize ) {
                componentLinkLabel.DisableLink( unparameterizeItemString );
            }
            if( !enableDelete ) {
                componentLinkLabel.DisableLink( deleteItemString );
            }
        }

        /// <summary>
        /// Sets the panel that is used to display the popup menu items
        /// </summary>
        /// <remarks>The popup menu panel is typically owned by a higher-level Control so that it won't be clipped at the edge of this control</remarks>
        public Panel PopupMenuPanel {
            set {
                this.componentLinkLabel.PopupMenuPanel = value;
            }
        }

        /// <summary>
        /// Sets the database and initializes the market plan component list.
        /// </summary>
        public override MrktSimDb.ModelDb Db {
            set {
                base.Db = value;
                componentView.Sort = "name";      // this initializes the sort arrow display of the componentDataGridView
            }
        }

        		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.componentView = new System.Data.DataView();
            this.selectAllCheckBox = new System.Windows.Forms.CheckBox();
            this.allComponentsLabel = new System.Windows.Forms.Label();
            this.componentDataGridView = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Values = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.componentLinkLabel = new Utilities.PopupMenuLinkLabel();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.noScanCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.componentView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.componentDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // selectAllCheckBox
            // 
            this.selectAllCheckBox.AutoSize = true;
            this.selectAllCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.selectAllCheckBox.Location = new System.Drawing.Point( 260, 4 );
            this.selectAllCheckBox.Name = "selectAllCheckBox";
            this.selectAllCheckBox.Size = new System.Drawing.Size( 71, 18 );
            this.selectAllCheckBox.TabIndex = 2;
            this.selectAllCheckBox.Text = "Select All";
            this.selectAllCheckBox.UseVisualStyleBackColor = true;
            this.selectAllCheckBox.CheckedChanged += new System.EventHandler( this.selectAllCheckBox_CheckedChanged );
            // 
            // allComponentsLabel
            // 
            this.allComponentsLabel.AutoSize = true;
            this.allComponentsLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.allComponentsLabel.Location = new System.Drawing.Point( 3, 3 );
            this.allComponentsLabel.Name = "allComponentsLabel";
            this.allComponentsLabel.Size = new System.Drawing.Size( 96, 15 );
            this.allComponentsLabel.TabIndex = 3;
            this.allComponentsLabel.Text = "All Components";
            // 
            // componentDataGridView
            // 
            this.componentDataGridView.AllowUserToAddRows = false;
            this.componentDataGridView.AllowUserToDeleteRows = false;
            this.componentDataGridView.AllowUserToResizeRows = false;
            this.componentDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.componentDataGridView.AutoGenerateColumns = false;
            this.componentDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.componentDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.componentDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.componentDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.componentDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.componentDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.componentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.componentDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.NameCol,
            this.ProductCol,
            this.TypeCol,
            this.Values,
            this.Status,
            this.IdCol} );
            this.componentDataGridView.DataSource = this.componentView;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.componentDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.componentDataGridView.GridColor = System.Drawing.Color.FromArgb( ((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))) );
            this.componentDataGridView.Location = new System.Drawing.Point( 9, 26 );
            this.componentDataGridView.Name = "componentDataGridView";
            this.componentDataGridView.ReadOnly = true;
            this.componentDataGridView.RowHeadersVisible = false;
            this.componentDataGridView.RowHeadersWidth = 4;
            this.componentDataGridView.RowTemplate.Height = 14;
            this.componentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.componentDataGridView.Size = new System.Drawing.Size( 492, 124 );
            this.componentDataGridView.TabIndex = 4;
            this.componentDataGridView.DoubleClick += new System.EventHandler( this.componentDataGridView_DoubleClick );
            this.componentDataGridView.SelectionChanged += new System.EventHandler( this.componentList_SelectedIndexChanged );
            // 
            // NameCol
            // 
            this.NameCol.DataPropertyName = "name";
            this.NameCol.HeaderText = "Name";
            this.NameCol.MinimumWidth = 25;
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            this.NameCol.Width = 150;
            // 
            // ProductCol
            // 
            this.ProductCol.DataPropertyName = "product";
            this.ProductCol.HeaderText = "Product";
            this.ProductCol.Name = "ProductCol";
            this.ProductCol.ReadOnly = true;
            this.ProductCol.Width = 85;
            // 
            // TypeCol
            // 
            this.TypeCol.DataPropertyName = "type";
            this.TypeCol.HeaderText = "Type";
            this.TypeCol.MinimumWidth = 25;
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.ReadOnly = true;
            this.TypeCol.Width = 75;
            // 
            // Values
            // 
            this.Values.DataPropertyName = "num_values";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Values.DefaultCellStyle = dataGridViewCellStyle2;
            this.Values.HeaderText = "Values";
            this.Values.MinimumWidth = 25;
            this.Values.Name = "Values";
            this.Values.ReadOnly = true;
            this.Values.Width = 45;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.DataPropertyName = "status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // IdCol
            // 
            this.IdCol.DataPropertyName = "id";
            this.IdCol.HeaderText = "ID";
            this.IdCol.Name = "IdCol";
            this.IdCol.ReadOnly = true;
            this.IdCol.Visible = false;
            // 
            // componentLinkLabel
            // 
            this.componentLinkLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.componentLinkLabel.BottomMargin = 4;
            this.componentLinkLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.componentLinkLabel.HighlightColor = System.Drawing.Color.LightSalmon;
            this.componentLinkLabel.LeftMargin = 13;
            this.componentLinkLabel.LinkText = "Components:";
            this.componentLinkLabel.Location = new System.Drawing.Point( 3, -1 );
            this.componentLinkLabel.MenuItemSpacing = 5;
            this.componentLinkLabel.Name = "componentLinkLabel";
            this.componentLinkLabel.PopupBackColor = System.Drawing.Color.White;
            this.componentLinkLabel.PopupFont = new System.Drawing.Font( "Arial", 8F );
            this.componentLinkLabel.PopupParentLevelsAbove = 3;
            this.componentLinkLabel.RightMargin = 5;
            this.componentLinkLabel.Size = new System.Drawing.Size( 96, 24 );
            this.componentLinkLabel.TabIndex = 5;
            this.componentLinkLabel.TabMargin = 15;
            this.componentLinkLabel.TopMargin = 9;
            // 
            // typeComboBox
            // 
            this.typeComboBox.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.typeComboBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange( new object[] {
            "All",
            "Coupons",
            "Display",
            "Distribution",
            "Media",
            "Price",
            "Market Utility",
            "----------------",
            "External Factors"} );
            this.typeComboBox.Location = new System.Drawing.Point( 93, 2 );
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size( 117, 23 );
            this.typeComboBox.TabIndex = 7;
            this.typeComboBox.Text = "All";
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler( this.typeComboBox_SelectedIndexChanged );
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.countLabel.Location = new System.Drawing.Point( 214, 5 );
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size( 33, 13 );
            this.countLabel.TabIndex = 8;
            this.countLabel.Text = "[100]";
            // 
            // noScanCheckBox
            // 
            this.noScanCheckBox.AutoSize = true;
            this.noScanCheckBox.Checked = true;
            this.noScanCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noScanCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.noScanCheckBox.Location = new System.Drawing.Point( 351, 3 );
            this.noScanCheckBox.Name = "noScanCheckBox";
            this.noScanCheckBox.Size = new System.Drawing.Size( 67, 18 );
            this.noScanCheckBox.TabIndex = 9;
            this.noScanCheckBox.Text = "No Scan";
            this.noScanCheckBox.UseVisualStyleBackColor = true;
            // 
            // MarketPlanComponentPicker
            // 
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.Controls.Add( this.noScanCheckBox );
            this.Controls.Add( this.countLabel );
            this.Controls.Add( this.typeComboBox );
            this.Controls.Add( this.componentLinkLabel );
            this.Controls.Add( this.componentDataGridView );
            this.Controls.Add( this.allComponentsLabel );
            this.Controls.Add( this.selectAllCheckBox );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.Name = "MarketPlanComponentPicker";
            this.Size = new System.Drawing.Size( 511, 153 );
            ((System.ComponentModel.ISupportInitialize)(this.componentView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.componentDataGridView)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
                #endregion

        /// <summary>
        /// Imports one or more plan components from an Excel file.
        /// </summary>
       private void ExcelImport() {
            createdPlanRows = null;
            if( defaultPlanType == ModelDb.PlanType.MarketPlan ) {
                //this isn't the way to create top-level market plans (s/b disallowed by UI)
                return;
            }
            // ask user for a name for plans
            string dlgTitle = null;
            string dlgInfo = "Specify a name and description and then click \'Choose File...\' to choose the Excel file.";
            switch( defaultPlanType ) {
                case ModelDb.PlanType.Coupons:
                    dlgTitle = "Import Coupon Plan Component";
                    break;
                case ModelDb.PlanType.Display:
                    dlgTitle = "Import Display Plan Component";
                    break;
                case ModelDb.PlanType.Distribution:
                    dlgTitle = "Import Distribution Plan Component";
                    break;
                case ModelDb.PlanType.Media:
                    dlgTitle = "Import Media Plan Component";
                    break;
                case ModelDb.PlanType.Price:
                    dlgTitle = "Import Price Plan Component";
                    break;
                case ModelDb.PlanType.Market_Utility:
                    dlgTitle = "Import Market Utility Component";
                    break;
                case ModelDb.PlanType.ProdEvent:
                    dlgTitle = "Import External Factors";
                    dlgInfo = "External Factors components will appear in the Market Plans list.";
                    break;
            }
            importDialog = new ImportNameAndDescr( dlgTitle, dlgInfo, importPath, false, this.BackColor, "ImportMarketPlanComponent" );

            DialogResult rslt = importDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                System.Threading.Thread thread = status.StartBackgroundThread( CreatePlanNow );
                status.UpdateUIAndContinue( ExcelImport_Part2, thread, 200, importStatusMsg, 1, 60, "Processing..." );
            }
        }

        private ErrorList createPlanErrors;

        private void CreatePlanNow() {
            PlanReader planReader = new PlanReader( theDb, importDialog.ObjName );
            string fileName = importDialog.ObjDataFile;
            createPlanErrors = planReader.CreatePlan( fileName, defaultPlanType, true, out createdPlanRows );
        }

        private void ExcelImport_Part2(){

            Utilities.Status status = new Status( this );

            Utilities.Status.SetWaitCursor( this );

            DateTime start = DateTime.Now;
            DateTime t1 = DateTime.Now;
            DateTime t1x = DateTime.Now;
            DateTime t2 = DateTime.Now;
            DateTime t3 = DateTime.Now;
            DateTime t4 = DateTime.Now;

            string planName = importDialog.ObjName;
            string planDesc = importDialog.ObjDescription;

            ErrorList errors = createPlanErrors;

            unassignedComponents = new ArrayList();
            ArrayList assignedComponents = new ArrayList();

            if( errors.Count == 0 ) {
                ArrayList externalFactorsPlans = null;

                if( defaultPlanType != ModelDb.PlanType.ProdEvent ) {
                    // add standard components (other than External Factors)
                    // add the imported component plan(s) to each of the selected top-level plans
                    foreach( int topLevelPlanID in filter.MarketPlanIDs ) {
                        MrktSimDBSchema.market_planRow topLevelPlan = theDb.Data.market_plan.FindByid( topLevelPlanID );
                        foreach( MrktSimDBSchema.market_planRow importedComponent in createdPlanRows ) {
                            // add only components for the same product as the selected plan (plus components for all products)
                            if( importedComponent.product_id == topLevelPlan.product_id || importedComponent.product_id == ModelDb.AllID ) {
                                theDb.CreatePlanRelation( topLevelPlanID, importedComponent.id );
                                assignedComponents.Add( importedComponent );
                            }
                        }
                    }
                    // see if we assigned all of the components
                    foreach( MrktSimDBSchema.market_planRow importedComponent in createdPlanRows ) {
                        if( assignedComponents.Contains( importedComponent ) == false ) {
                            unassignedComponents.Add( importedComponent );
                        }
                    }
                    t1 = DateTime.Now;
                }
                else {
                    // the component is an External Factors item!  Since it is effectively a top-level plan add it to the current scenario(s)
                    int scenarioID = filter.SelectedScenarioID;
                    externalFactorsPlans = new ArrayList();
                    Console.WriteLine( "Imported External Factors...Adding to Scenario ({0})", scenarioID );
                    if( scenarioID != -1 ) {
                        foreach( MrktSimDBSchema.market_planRow importedComponent in createdPlanRows ) {
                            theDb.AddMarketPlanToScenario( scenarioID, importedComponent );
                            externalFactorsPlans.Add( importedComponent );
                        }
                    }
                    else {
                        Console.WriteLine( "Import External Factors to ALL scenarios!!!" );
                        string sFilter = String.Format( "model_id = {0}", theDb.ModelID );
                        DataRow[] allScenarios = theDb.Data.scenario.Select( sFilter );
                        foreach( MrktSimDBSchema.scenarioRow srow in allScenarios ) {
                            foreach( MrktSimDBSchema.market_planRow importedComponent in createdPlanRows ) {
                                theDb.AddMarketPlanToScenario( srow.scenario_id, importedComponent );
                                externalFactorsPlans.Add( importedComponent );
                            }
                        }
                    }
                    t1x = DateTime.Now;
                }

                // since the list of market plans has changed, update the rest of the display
                componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );

                foreach( MrktSimDBSchema.market_planRow goodComponent in assignedComponents ) {
                    this.addPlanToPlanTable( goodComponent );
                }

                bool firstRow = true;
                foreach( DataGridViewRow drow in this.componentDataGridView.Rows ) {
                    int rowID = (int)drow.Cells[ "IdCol" ].Value;
                    drow.Selected = false;
                    foreach( MrktSimDBSchema.market_planRow createdComponent in assignedComponents ) {
                        if( rowID == createdComponent.id ) {
                            drow.Selected = true;
                            if( firstRow ) {
                                componentDataGridView.FirstDisplayedScrollingRowIndex = drow.Index;    // scroll the view to make the first new item visible
                                firstRow = false;
                            }
                            break;
                        }
                    }
                }
                updateSelectedIDs();
                componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );
                t2 = DateTime.Now;

                if( SelectedRowChanged != null ) {
                    if( externalFactorsPlans != null ) {
                        SelectedRowChanged( externalFactorsPlans );
                    }
                    else {
                        SelectedRowChanged( 2 );
                    }
                }
                t3 = DateTime.Now;

                Utilities.Status.ClearStatus( this );
                
                // inform the user if any plans didn't get assigned
                if( unassignedComponents.Count > 0 ) {

                    // perhaps none of them were assigned?
                    if( unassignedComponents.Count == createdPlanRows.Count ) {
                        string msg = String.Format( unassAllMsg, unassignedComponents.Count, this.defaultPlanType.ToString() );
                        ConfirmDialog cdlg = new ConfirmDialog( msg, "", "", unassAllTitle );
                        cdlg.Height += 65;
                        cdlg.Width += 100;
                        t4 = DateTime.Now;
                        DialogResult resp = cdlg.ShowDialog();
                        if( resp == DialogResult.Yes ) {
                            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteUnassignedComponents );
                            status.UpdateUIAndContinue( ExcelImport_Part3, backgroundThread, 200, importDelUnassStatusMsg, 1, 60, "Processing..." );
                            DebugReportTimings( start, t1, t2, t3, t4 );


                            Utilities.Status.ClearWaitCursor( this );
                            return;
                        }
                    }
                    else {
                        // not all of the components matched the product_id of any selected market plan(s)
                        string msg = String.Format( unassMsg, unassignedComponents.Count, createdPlanRows.Count, this.defaultPlanType.ToString() );
                        ConfirmDialog cdlg = new ConfirmDialog( msg, "", "", unassTitle );
                        cdlg.Height += 65;
                        cdlg.Width += 100;
                        t4 = DateTime.Now;
                        DialogResult resp = cdlg.ShowDialog();
                        if( resp == DialogResult.Yes ) {
                            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteUnassignedComponents );
                            status.UpdateUIAndContinue( ExcelImport_Part3, backgroundThread, 200, importDelUnassStatusMsg, 1, 60, "Processing..." );
                            DebugReportTimings( start, t1, t2, t3, t4 );
                            theDb.AllignPlansWithData();  //since there are some matched plans


                            Utilities.Status.ClearWaitCursor( this );
                            return;
                        }
                    }
                }
                t4 = DateTime.Now;
                DebugReportTimings( start, t1, t2, t3, t4 );

                theDb.AllignPlansWithData();
            }
            else {
                // errors.Count is nonzero

                Utilities.Status.ClearStatus( this );
            
                errors.Display();
            }


            Utilities.Status.ClearStatus( this );
        }

        private void DebugReportTimings( DateTime start, DateTime t1, DateTime t2, DateTime t3, DateTime t4 ) {
            TimeSpan s1 = t1 - start;
            TimeSpan s2 = t2 - t1;
            TimeSpan s3 = t3 - t2;
            TimeSpan s4 = t4 - t3;

            Console.WriteLine( "-->\n-->Timings {0}, {1}, {2}, {3}\n-->", (int)s1.TotalMilliseconds, (int)s2.TotalMilliseconds, (int)s3.TotalMilliseconds, (int)s4.TotalMilliseconds );
        }

        private void ExcelImport_Part3() {

            Utilities.Status.ClearStatus( this );
        }

        private void DeleteUnassignedComponents() {
            foreach( MrktSimDBSchema.market_planRow unassignedComponent in unassignedComponents ) {
                unassignedComponent.Delete();
            }
        }

        /// <summary>
        /// Brings up the dialog for creating a new market plan component.
        /// </summary>
        private void CreatePlan() {
            createEditDialog = new CreateComponentPlan2( theDb );
            createEditDialog.Type = defaultPlanType;

            // a new component will initially be configued to live in the current plan only
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList users = relater.GetPlansForComponent( -1, -1 );
            ArrayList used = new ArrayList();

            foreach( MarketPlanControlRelater.Item item in users ) {
                foreach( int selectedID in filter.MarketPlanIDs ){
                    if( selectedID == item.ID ) {                    // initial owners are the market plans that are currently selected in the UI
                        item.Selected = true;
                    }
                }
            }
            createEditDialog.SetUsersAndUsedLists( users, used );

            // dlg.ProductID = this.productPicker.ProductID;

            DialogResult rslt = createEditDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( CreatePlan_Part2, createStatusMsg, 0 );
            }
        }

        private void CreatePlan_Part2() {

            Utilities.Status.SetWaitCursor( this );

            // the dialog's ok-button processing has already created the plan (dlg.CurrentPlan) at this point

            MrktSimDBSchema.market_planRow addedComponent = createEditDialog.CurrentPlan;

            UpdateMarketPlansTree( addedComponent.id, createEditDialog.AllUserIDs, new ArrayList() );

            // refresh the component list without triggering an event
            componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );

            this.addPlanToPlanTable( addedComponent );

            selectAllCheckBox.Checked = false;
            selectedComponent = addedComponent;

            componentDataGridView.ClearSelection();
            foreach( DataGridViewRow drow in this.componentDataGridView.Rows ) {
                if( addedComponent.id == (int)drow.Cells[ "IdCol" ].Value ) {
                    drow.Selected = true;
                    componentDataGridView.FirstDisplayedScrollingRowIndex = drow.Index;    // be sure the new selection is visible
                }
            }

            updateSelectedIDs();
            componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );

            Utilities.Status.ClearStatus( this );
            
            if( SelectedRowChanged != null ) {
                SelectedRowChanged( 2 );
            }
        }

        /// <summary>
        /// Brings up the parameters dialog for the selected component.  Does nothing unless exactly one component is selected.
        /// </summary>
        private void ShowParams() {
            MrktSimDBSchema.market_planRow plan = this.selectedComponent;

            if( plan == null )
                return;

            MarketPlanParameter dlg = new MarketPlanParameter( plan );
            dlg.StartPosition = FormStartPosition.CenterParent;

            dlg.Db = theDb;

            DialogResult resp = dlg.ShowDialog();
            if( resp == DialogResult.OK ) {
                if( SelectedRowChanged != null ) {
                    SelectedRowChanged( 1 );
                }

                ModelEditor modelEditor = (ModelEditor)this.ParentForm;
            }
        }

        /// <summary>
        /// Brings up the dialog where the user can edit the current market plan.
        /// </summary>
        private void EditPlan() {
            if( selectedComponent.type == (byte)ModelDb.PlanType.ProdEvent ) {
                return;
            }
            createEditDialog = new CreateComponentPlan2( theDb );
            createEditDialog.Type = currentType;
            createEditDialog.CurrentPlan = selectedComponent;

            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList users = relater.GetPlansForComponent( selectedComponent.id, selectedComponent.product_id );
            ArrayList used = new ArrayList();
            createEditDialog.SetUsersAndUsedLists( users, used );

            // dlg.ProductID = this.productPicker.ProductID;

            DialogResult rslt = createEditDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( EditPlan_Part2, editStatusMsg, 0 );
            }
        }

        private void EditPlan_Part2() {
            Utilities.Status.SetWaitCursor( this );

            MrktSimDBSchema.market_planRow editedComponent = createEditDialog.CurrentPlan;

            UpdateMarketPlansTree( editedComponent.id, createEditDialog.AddedUserIDs, createEditDialog.RemovedUserIDs );

            // find out if the plan is still going to be visible
            bool isVisible = false;
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            if( filter.MarketPlanIDs != null ) {
                foreach( int selectedPlanID in filter.MarketPlanIDs ) {
                    if( relater.PlanContainsComponent( editedComponent.id, selectedPlanID ) ) {
                        isVisible = true;
                        break;
                    }
                }
            }
            if( isVisible == false ) {
                // the edits have made this component not visible in this plan -- hide it
                componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );

                this.addPlanToPlanTable( editedComponent );

                if( componentDataGridView.RowCount > 0 ) {

                    componentDataGridView.ClearSelection();
                    componentDataGridView.Rows[ 0 ].Selected = true;
                    DataRow editedRow = componentView[ componentDataGridView.SelectedRows[ 0 ].Index ].Row;
                    editedComponent = theDb.Data.market_plan.FindByid( (int)editedRow[ "id" ] );
                }

                selectedComponent = editedComponent;

                componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );

                if( SelectedRowChanged != null ) {
                    SelectedRowChanged( 1 );
                }
            }
            else {
                // change the displayed name
                DataGridViewRow selrow = componentDataGridView.SelectedRows[ 0 ];
                selrow.Cells[ "NameCol" ].Value = editedComponent.name;
            }

            Utilities.Status.ClearStatus( this );
        }

        /// <summary>
        /// Brings up the dialog where the user can edit the current market plan.
        /// </summary>
        /// <remarks>Assumes there is at least one selected item and that all selected items are of the same type.</remarks>
        private void EditPlanData() {
#if EDIT_DATA
            editDataDialog = new EditComponentData( theDb, this.selectedComponents, currentType );

            DialogResult rslt = editDataDialog.ShowDialog();

            if( rslt == DialogResult.OK ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( EditPlanData_Part2, editDataStatusMsg, 0 );
            }
#endif
        }

        private void EditPlanData_Part2() {
            Utilities.Status.ClearStatus( this );
        }

        /// <summary>
        /// Makes a copy of the selected market plan component(s).
        /// </summary>
        private void CopyPlans() 
        {
            if( componentDataGridView.SelectedRows.Count == 0 ) {
                return;
            }

            string statusMsg = copyStatusMsg;
            ConfirmDialog cDlg = null;
            if( componentDataGridView.SelectedRows.Count == 1 ) {
                DataRow row = componentView[ componentDataGridView.SelectedRows[ 0 ].Index ].Row;
                MrktSimDBSchema.market_planRow theComp = theDb.Data.market_plan.FindByid((int)row["id"]);

                cDlg = new ConfirmDialog( copyDlgMsg, copyDlgMsg2, theComp.name, copyDlgTItle );
            }
            else {
                // multople components selected
                statusMsg = copyStatusMsgM;
                cDlg = new ConfirmDialog( copyDlgMsgM, copyDlgMsg2M, componentDataGridView.SelectedRows.Count.ToString(), copyDlgTItle );
            }

            cDlg.SelectQuestionIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp == DialogResult.Yes ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( CopyPlans_Part2, statusMsg, 0 );
            }
        }

        private void CopyPlans_Part2() {
            Utilities.Status.SetWaitCursor( this );

            ArrayList copiedComponents = new ArrayList();
            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );

            // loop over each selected component
            foreach( DataGridViewRow dgvRow in componentDataGridView.SelectedRows ) {
                //DataRow row = ((DataRowView)componentListBox.Items[index]).Row;
                DataRow row = componentView[ dgvRow.Index ].Row;
                MrktSimDBSchema.market_planRow componentToCopy = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );

                // find out which plans use this component
                ArrayList selectedItemPlans = relater.GetPlansForComponent( componentToCopy.id, componentToCopy.product_id );
                ArrayList selectedItemPlanIDs = new ArrayList();
                foreach( MarketPlanControlRelater.Item item in selectedItemPlans ) {
                    if( item.Selected ) {
                        selectedItemPlanIDs.Add( item.ID );
                    }
                }

                MrktSimDBSchema.market_planRow copiedComponent = theDb.CopyPlan( componentToCopy );  // actually copy a component plan
                copiedComponents.Add( copiedComponent );
                UpdateMarketPlansTree( copiedComponent.id, selectedItemPlanIDs, new ArrayList() );
            }
            // end loop over selected components

            Utilities.Status.ClearStatus( this );

            CompletionDialog mDlg = null;
            if( componentDataGridView.SelectedRows.Count == 1 ) {
                mDlg = new CompletionDialog( "Plan Component Copied Successfully" );
            }
            else {
                mDlg = new CompletionDialog( "Plan Components Copied Successfully" );
            }
            mDlg.ShowDialog();

            // refresh the components list without triggering an event
            componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );

            // make the new plan(s) visible
            foreach( MrktSimDBSchema.market_planRow copiedComponent in copiedComponents ) {
                addPlanToPlanTable( copiedComponent );

                // add the copied plan to each of the selected top-level plans
                foreach( int topLevelPlanID in filter.MarketPlanIDs ) {
                    theDb.CreatePlanRelation( topLevelPlanID, copiedComponent.id );
                }
            }

            selectedComponent = (MrktSimDBSchema.market_planRow)copiedComponents[ 0 ];

            foreach( MrktSimDBSchema.market_planRow copiedComponent in copiedComponents ) {
                Console.WriteLine( "Selecting Comp: " + copiedComponent.id );
            }

            // since the list of market plans has changed, update the rest of the display
            bool firstChangedRow = true;
            foreach( DataGridViewRow drow in this.componentDataGridView.Rows ) {
                int rowID = (int)drow.Cells[ "IdCol" ].Value;
                drow.Selected = false;
                foreach( MrktSimDBSchema.market_planRow copiedComponent in copiedComponents ) {
                    if( rowID == copiedComponent.id ) {
                        drow.Selected = true;
                        if( firstChangedRow ) {
                            componentDataGridView.FirstDisplayedScrollingRowIndex = drow.Index;    // be sure we can see the first new item
                            firstChangedRow = false;
                        }
                        break;
                    }
                }
            }
            updateSelectedIDs();
            componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );

            if( SelectedRowChanged != null ) {
                SelectedRowChanged( 1 );
            }
        }

        private void SetPlansAwarnessPerswuasion() {
            if( componentDataGridView.SelectedRows.Count == 0 ) {
                return;
            }

            double awareness = -1;
            double persuasion = -1;
            double awTot = 0;
            double perTot = 0;
            int nPoints = 0;
            bool awVaries = false;
            bool perVaries = false;
            string label = "< no components selected >";
           
            foreach( DataGridViewRow dgrow in componentDataGridView.SelectedRows ) {
                DataRow row = componentView[ dgrow.Index ].Row;
                MrktSimDBSchema.market_planRow theComp = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );
                label = theComp.name;
                DataTable compTable = null;
                switch( theComp.type ) {
                    case (byte)ModelDb.PlanType.Display:
                        compTable = theDb.Data.display;
                        break;
                    case (byte)ModelDb.PlanType.Distribution:
                        compTable = theDb.Data.distribution;
                        break;
                    case (byte)ModelDb.PlanType.Media:
                        compTable = theDb.Data.mass_media;
                        break;
                }
                if( compTable == null ) {
                    string msg = String.Format( "Unsupported component type selected: {0}", (ModelDb.PlanType)theComp.type );
                    MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                if( compTable.Columns.Contains( "message_awareness_probability" ) == false || compTable.Columns.Contains( "message_persuation_probability" ) == false ) {
                    string msg = String.Format( "Component type : {0} does not have awareness and persuasion columns in the data!", theComp.type );
                    MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                string query = String.Format( "market_plan_id = {0}", theComp.id );
                DataRow[] compDataRows = compTable.Select( query );
                foreach( DataRow crow in compDataRows ) {
                    double a = (double)crow[ "message_awareness_probability" ];
                    double p = (double)crow[ "message_persuation_probability" ];

                    if( nPoints == 0 ) {
                        awareness = a;
                        persuasion = p;
                    }
                    else {
                        if( a != awareness ) {
                            awVaries = true;
                        }
                        if( p != persuasion ) {
                            perVaries = true;
                        }
                    }

                    awTot += a;
                    perTot += p;
                    nPoints += 1;
                }
            }

            awareness = awTot / (double) nPoints;
            persuasion = perTot / (double) nPoints;

            if( componentDataGridView.SelectedRows.Count > 1 ) {
                label = String.Format( "{0} Components Selected", componentDataGridView.SelectedRows.Count );
            }

            PlanAwarenessPersuasionForm apForm = new PlanAwarenessPersuasionForm( label, awareness, awVaries, persuasion, perVaries );
            DialogResult resp = apForm.ShowDialog();
            if( resp == DialogResult.OK ) {

                // loop over all rows again - no tests this time since we did that already
                foreach( DataGridViewRow dgrow in componentDataGridView.SelectedRows ) {
                    DataRow row = componentView[ dgrow.Index ].Row;
                    MrktSimDBSchema.market_planRow theComp = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );
                    label = theComp.name;
                    DataTable compTable = null;
                    switch( theComp.type ) {
                        case (byte)ModelDb.PlanType.Display:
                            compTable = theDb.Data.display;
                            break;
                        case (byte)ModelDb.PlanType.Distribution:
                            compTable = theDb.Data.distribution;
                            break;
                        case (byte)ModelDb.PlanType.Media:
                            compTable = theDb.Data.mass_media;
                            break;
                    }

                    string query = String.Format( "market_plan_id = {0}", theComp.id );
                    DataRow[] compDataRows = compTable.Select( query );

                    foreach( DataRow setRow in compDataRows ) {
                        double olda = (double)setRow[ "message_awareness_probability" ];
                        double oldp = (double)setRow[ "message_persuation_probability" ];

                        // set up the new values
                        double newa = apForm.Awareness;
                        double newp = apForm.Persuasion;
                        if( apForm.ForceSpecificValues == false ) {
                            if( awVaries ) {
                                newa = olda * apForm.Awareness;     // scale the old awareness
                            }
                            if( perVaries ) {
                                newp = oldp * apForm.Persuasion;     // scale the old persuasion
                            }
                        }

                        // set the values
                        setRow[ "message_awareness_probability" ] = newa;
                        setRow[ "message_persuation_probability" ] = newp;
                    }
                }

                string msg = String.Format( "Done!  {0} Component(s) updated.", componentDataGridView.SelectedRows.Count );
                CompletionDialog cdlg = new CompletionDialog( msg );
                cdlg.ShowDialog();
            }
        }
        
        // data structure used by the unparameterize operation
        private struct ParamStruct
        {
            public string parmCol;
            public string valueCol;

            public ParamStruct( string parmCol, string valueCol ) {
                this.parmCol = parmCol;
                this.valueCol = valueCol;
            }
        }

        /// <summary>
        /// Returns the list of ParamStruct objects appropriate for unparameterizing the given component type.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        private ParamStruct[] ParamStructsFor( byte componentType ) {
            switch( componentType ) {
                case (byte)ModelDb.PlanType.Display:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "message_awareness_probability" ),
                        new ParamStruct( "parm2", "message_persuation_probability" ),
                        new ParamStruct( "parm3", "attr_value_F" ),
                    };
                case (byte)ModelDb.PlanType.Distribution:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "message_awareness_probability" ),
                        new ParamStruct( "parm2", "message_persuation_probability" ),
                        new ParamStruct( "parm3", "attr_value_F" ),
                    };
                case (byte)ModelDb.PlanType.Media:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "message_awareness_probability" ),
                        new ParamStruct( "parm2", "message_persuation_probability" ),
                        new ParamStruct( "parm3", "attr_value_G" ),
                    };
                case (byte)ModelDb.PlanType.Coupons:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "message_awareness_probability" ),
                        new ParamStruct( "parm2", "message_persuation_probability" ),
                        new ParamStruct( "parm3", "attr_value_G" ),     
                        new ParamStruct( "parm4", "attr_value_I" ),
                    };
                case (byte)ModelDb.PlanType.Market_Utility:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "awareness" ),
                        new ParamStruct( "parm2", "persuasion" ),
                        new ParamStruct( "parm3", "utility" ),
                        new ParamStruct( "parm4", "percent_dist" ),
                    };
                case (byte)ModelDb.PlanType.Price:
                    return new ParamStruct[] {
                        new ParamStruct( "parm1", "price" ),
                        new ParamStruct( "parm2", "markup" ),
                        new ParamStruct( "parm3", "periodic_price" ),
                        new ParamStruct( "parm4", "percent_SKU_in_dist" ),
                    };
                default:
                    return null;
            }
        }

        /// <summary>
        /// Incorporates current param values from the selected component(s) into the data (multiplies data by the appropriate param) 
        /// for each of the selected component(s).  Params are then set to 1.0.  This makes the param values a "permanent" part of the data.
        /// </summary>
        private void UnparameterizePlans() {
            if( componentDataGridView.SelectedRows.Count == 0 ) {
                // this should never happen, but prevent a problem just in case it does
                return;
            }

            // warn the user!
            string msg0 = "OK to un-parameterize the selected component(s)?\r\n\r\nThis process will:" +
                "\r\n  1) multiply the component data values by the appropriate parameter values" +
                "\r\n  2) Reset all parameter values to 1.0\r\n\r\n";
            msg0 += String.Format( "Selected Components: {0}", componentDataGridView.SelectedRows.Count );
            ConfirmDialog cdlg1 = new ConfirmDialog( msg0, "Confirm Un-Parameterizing Components" );
            cdlg1.SelectWarningIcon();
            cdlg1.SetOkCancelButtonStyle();
            cdlg1.Width += 40;
            cdlg1.Height += 80;
            DialogResult resp = cdlg1.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            int changedComponents = 0;

            foreach( DataGridViewRow dgrow in componentDataGridView.SelectedRows ) {
                DataRow row = componentView[ dgrow.Index ].Row;
                MrktSimDBSchema.market_planRow theComp = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );

                bool hasParams = false;
                ParamStruct[] allParamInfo = ParamStructsFor( theComp.type );
                foreach( ParamStruct paramInfo in allParamInfo ) {
                    double param = (double)theComp[ paramInfo.parmCol ];
                    // if any named parameter is not equal to 1.0 we need to process the data
                    if( param != 1.0 ) {
                        hasParams = true;
                        break;
                    }
                }
                if( hasParams == false ) {
                    // nothing to do for this component
                    continue;
                }

                changedComponents += 1;

                DataTable compTable = null;
                switch( theComp.type ) {
                    case (byte)ModelDb.PlanType.Display:
                        compTable = theDb.Data.display;
                        break;
                    case (byte)ModelDb.PlanType.Distribution:
                        compTable = theDb.Data.distribution;
                        break;
                    case (byte)ModelDb.PlanType.Media:
                        compTable = theDb.Data.mass_media;
                        break;
                    case (byte)ModelDb.PlanType.Coupons:
                        compTable = theDb.Data.mass_media;
                        break;
                    case (byte)ModelDb.PlanType.Market_Utility:
                        compTable = theDb.Data.market_utility;
                        break;
                    case (byte)ModelDb.PlanType.Price:
                        compTable = theDb.Data.product_channel;
                        break;
                }
                if( compTable == null ) {
                    // this shouldn't happen
                    string msg = String.Format( "Error: Unsupported component type selected: {0}", (ModelDb.PlanType)theComp.type );
                    MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                string query = String.Format( "market_plan_id = {0}", theComp.id );
                DataRow[] compDataRows = compTable.Select( query );

                // apply the params to the appropriate data columns
                foreach( ParamStruct paramInfo in allParamInfo ) {
                    double parm = (double)theComp[ paramInfo.parmCol ];
                    if( parm != 1.0 ) {

                        // loop over each row of data and apply the paramater
                        foreach( DataRow crow in compDataRows ) {
                            double origValue = (double)crow[ paramInfo.valueCol ];
                            double parameterizedValue = origValue * parm;
                            crow[ paramInfo.valueCol ] = parameterizedValue;
                        }
                    }

                }

                // set the params in the component to 1.0
                foreach( ParamStruct paramInfo in allParamInfo ) {
                    theComp[ paramInfo.parmCol ] = 1.0;
                }
            }

            if( SelectedRowChanged != null ) {
                SelectedRowChanged( 1 );
            }

            string msg2 = String.Format( "Unparameterization done!  \r\n{0} component(s) were updated.", changedComponents );
            CompletionDialog cdlg = new CompletionDialog( msg2 );
            cdlg.ShowDialog();
        }

        /// <summary>
        /// Deletes the selected market plan component(s).
        /// </summary>
        private void DeletePlans() 
        {
            if( componentDataGridView.SelectedRows.Count == 0 ) {
                return;
            }

            string statusMsg = deleteStatusMsg;
            ConfirmDialog cDlg = null;
            if (componentDataGridView.SelectedRows.Count == 1){
                DataRow row = componentView[ componentDataGridView.SelectedRows[ 0 ].Index ].Row;
                MrktSimDBSchema.market_planRow theComp = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );

                cDlg = new ConfirmDialog( deleteDlgMsg, deleteDlgMsg2, theComp.name, deleteDlgTItle );
            }
            else {
                // multiple selections
                statusMsg = deleteStatusMsgM;
                cDlg = new ConfirmDialog( deleteDlgMsgM, deleteDlgMsg2M, componentDataGridView.SelectedRows.Count.ToString(), deleteDlgTItle );
            }
            cDlg.SelectWarningIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp == DialogResult.Yes ) {
                Utilities.Status status = new Status( this );
                status.UpdateUIAndContinue( DeletePlans_Part2, statusMsg, 0 );
            }
        }

        private void DeletePlans_Part2() {

            Utilities.Status.SetWaitCursor( this );

            // loop over each selected component
            foreach( DataGridViewRow dgvRow in componentDataGridView.SelectedRows ) {
                DataRow row = componentView[ dgvRow.Index ].Row;
                MrktSimDBSchema.market_planRow componentToDelete = theDb.Data.market_plan.FindByid( (int)row[ "id" ] );

                componentToDelete.Delete();
            }

            Utilities.Status.ClearStatus( this );

            CompletionDialog mDlg = null;

            if( componentDataGridView.SelectedRows.Count == 1 ) {
                mDlg = new CompletionDialog( deleteDoneMsg );
            }
            else {
                mDlg = new CompletionDialog( deleteDoneMsgM );
            }
            mDlg.ShowDialog();

            foreach( DataGridViewRow dgvRow in componentDataGridView.SelectedRows ) {
                componentDataGridView.Rows.Remove( dgvRow );
            }
        }


        private void ShiftPlan() {
            if( this.selectedComponents.Count == 0 ) {
                return;
            }
            changeDatesDialog = new ChangePlanDates( theDb, this.selectedComponents );
            changeDatesDialog.SetPlanType( ModelDb.PlanType.Price );     // as long as it isn't MarketPlan
            DialogResult resp = changeDatesDialog.ShowDialog();
            if( resp == DialogResult.OK ) {
                // is any display update needed???
            }
        }

        private void ChangePlanProduct() {
            if( this.selectedComponents.Count == 0 ) {
                return;
            }
            changeProductDialog = new ChangePlanProduct( theDb, this.selectedComponents );
            changeProductDialog.SetPlanType( this.defaultPlanType );
            DialogResult resp = changeProductDialog.ShowDialog();
            if( resp == DialogResult.OK ) {
                // is any display update needed???
            }
        }


        /// <summary>
        /// Responds to a change in the selected item in the list of components.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Fires a SelectedRowChanged event unless the selected row index is -1.</remarks>
        private void componentList_SelectedIndexChanged( object sender, EventArgs e ) 
        {
            selectAllCheckBox.CheckedChanged -= new EventHandler(selectAllCheckBox_CheckedChanged);
            selectAllCheckBox.Checked = false;
            selectAllCheckBox.CheckedChanged += new EventHandler(selectAllCheckBox_CheckedChanged);

            updateSelectedIDs();

            if( SelectedRowChanged != null ) {
                SelectedRowChanged( 2 );
            }
        }

        private void updateSelectedIDs()
        {
            filter.SelectedMarketPlanComponentIDs = new int[ componentDataGridView.SelectedRows.Count ];

            int ii = 0;
            multipleTypesSelected = false;
            currentType = ModelDb.PlanType.MarketPlan;

            foreach( DataGridViewRow dgvRow in componentDataGridView.SelectedRows ) {
                DataRow row = componentView[ dgvRow.Index ].Row;

                filter.SelectedMarketPlanComponentIDs[ ii ] = (int)row[ "id" ];
 
                if (ii == 0) // first time
                {
                    currentType = (ModelDb.PlanType)row[ "type" ];
                }
                else
                {
                    if( currentType != (ModelDb.PlanType)row[ "type" ] ) {
                        multipleTypesSelected = true;
                        currentType = ModelDb.PlanType.MarketPlan;
                    }
                }

                ii++;
            }

            filter.PlanType = currentType;


            if( componentDataGridView.SelectedRows.Count == 1 )
            {
                DataGridViewRow drow = componentDataGridView.SelectedRows[ 0 ];
                selectedComponent = theDb.Data.market_plan.FindByid( (int)drow.Cells[ "IdCol" ].Value );
            }
            else
            {
                selectedComponent = null;
            }
            selectedComponents = new ArrayList();
            foreach( DataGridViewRow drow in componentDataGridView.SelectedRows ) {
                selectedComponents.Add( (int)drow.Cells[ "IdCol" ].Value );
            }
        }

        #region Market Plan Tree Updating
        /// <summary>
        /// Makes the needed updates to the market_plan_tree table for a new or edited market plan.
        /// </summary>
        /// <param name="addedPlanID"></param>
        /// <param name="addedScenarioIDs"></param>
        /// <param name="removedScenarioIDs"></param>
        private void UpdateMarketPlansTree( int componentID, ArrayList addedMarketPlanIDs, ArrayList removedMarketPlanIIDs ) {
            if( addedMarketPlanIDs.Count == 0 && removedMarketPlanIIDs.Count == 0 ) 
            {
                return;
            }

            // remove rows if any are specified
            foreach( int removeID in removedMarketPlanIIDs ) {
                string removeFilter = String.Format( "parent_id={0} AND child_id={1}", removeID, componentID );
                DataRow[] rows = theDb.Data.market_plan_tree.Select( removeFilter );
                foreach( DataRow row in rows ) {
                    row.Delete();
                }
            }

            // add rows if any are specified
            foreach( int addID in addedMarketPlanIDs ) 
            {
                theDb.CreatePlanRelation( addID, componentID );
            }
        }

        #endregion

        /// <summary>
        /// Clears and updates the top level plan table
        /// </summary>
        public void UpdatePlanTable() {
            bool selectAll = this.selectAllCheckBox.Checked;
            ////DateTime startUpdate = DateTime.Now;

            componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );

            // turn off the display while we update the data
            componentDataGridView.DataSource = null;
            this.componentPlanTable.BeginLoadData();

            this.componentPlanTable.Clear();

            if( filter.SelectedScenarioID != -2 ) {
                componentDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;

                if( filter.MarketPlanIDs != null ) {

                    foreach( int planID in filter.MarketPlanIDs ) {
                        MrktSimDBSchema.market_planRow topLevelPlan = theDb.Data.market_plan.FindByid( planID );


                        // check if plan is in selected list of products
                        if( topLevelPlan != null ) {
                            MrktSimDBSchema.market_plan_treeRow[] trows = topLevelPlan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_parent();

                            foreach( MrktSimDBSchema.market_plan_treeRow trow in trows ) {
                                addPlanToPlanTable( trow.market_planRowBymarket_planmarket_plan_tree_child );
                            }

                            // add External Factors item, if one exists for this scenario
                            if( this.defaultPlanType == ModelDb.PlanType.MarketPlan || this.defaultPlanType == ModelDb.PlanType.ProdEvent ) {
                                string mpf = filter.ExternalFactorsQuery( planID );
                                DataRow[] externalFactorRows = theDb.Data.product_event.Select( mpf );
                                if( externalFactorRows.Length > 0 ) {
                                    AddExternalFactorToPlanTable( topLevelPlan, externalFactorRows.Length );
                                }
                            }
                        }
                    }
                }
            }
            else {
                // filter.SelectedScenarioID is -2 -- show all unassigned components
                ArrayList unassignedComponents = theDb.AllComponentsNotInAnyPlan();
                componentDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.Red;

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

                foreach( MrktSimDBSchema.market_planRow component in unassignedComponents ) {
                    if( allProds || prodList.Contains( component.product_id ) ) {
                        addPlanToPlanTable( component );
                        ////Console.WriteLine( "calling addPlanToPlanTable({1}) ... row count = {0}", this.componentPlanTable.Rows.Count, "y" );
                    }
                }
            }

            // turn on the display updates again
            this.componentPlanTable.EndLoadData();
            componentDataGridView.DataSource = componentView;

            ////DateTime preSelectTime = DateTime.Now;

            // establish the initial selection(s)
            this.componentDataGridView.ClearSelection();

            if( selectAll ) {
                checkAll();
            }
            else {
                // default situation - select the first row only
                if( this.componentDataGridView.Rows.Count > 0 ) {
                    this.componentDataGridView.Rows[ 0 ].Selected = true;
                    this.componentDataGridView.FirstDisplayedScrollingRowIndex = 0;
                }
            }

            componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );

            this.countLabel.Text = String.Format( "[{0}]", componentDataGridView.Rows.Count );

            ////DateTime preUpdateIDsTime = DateTime.Now;

            updateSelectedIDs();

            //!!!debug/devel stuff for performance testing
            ////DateTime doneTime = DateTime.Now;
            ////TimeSpan t1 = preSelectTime - startUpdate;
            ////TimeSpan t2 = preUpdateIDsTime - preSelectTime;
            ////TimeSpan t3 = doneTime - preUpdateIDsTime;
            ////string reportString = String.Format( "Comp. Picker Update times: {0:f2}, {1:f2}, {2:f2}", t1.TotalSeconds, t2.TotalSeconds, t3.TotalSeconds );
            ////Utilities.DataLogger.Log( reportString );
        }

        private bool AddExternalFactorToPlanTable( MrktSimDBSchema.market_planRow externalFactorRow, int nValues ) {
            Console.WriteLine( "AddExternalFactorToPlanTable({0})", externalFactorRow.id );

            DataRow row = componentPlanTable.NewRow();

            row[ "id" ] = externalFactorRow.id;
            row[ "name" ] = externalFactorRow.name;
            //row[ "start_date" ] = externalFactor.start_date;
            //row[ "end_date" ] = externalFactor.end_date;
            row[ "type" ] = ModelDb.PlanType.ProdEvent;

            //row[ "status" ] = externalFactor.demand_modification.ToString();    //!!!test
            //row[ "status" ] = "id = " + planID.ToString();
            row[ "status" ] = "-";
            row[ "num_values" ] = nValues;

            componentPlanTable.Rows.Add( row );

            return true;
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

            if (defaultPlanType != ModelDb.PlanType.MarketPlan &&
                (ModelDb.PlanType)plan.type != defaultPlanType)
            {
                return false;
            }

            // check if already in table
            // if so this is an error
            DataRow orig = componentPlanTable.Rows.Find(plan.id);

            if (orig != null)
            {
                // maybe the name changed
                orig["name"] = plan.name;

                return false;
            }

            DataRow row = componentPlanTable.NewRow();

            row["id"] = plan.id;
            row["name"] = plan.name;
            row["start_date"] = plan.start_date;
            row["end_date"] = plan.end_date;
            row[ "type" ] = plan.type;
            row[ "type_name" ] = plan.type.ToString();
            row[ "product" ] = GetProductName( plan.product_id );

            if( noScanCheckBox.Checked == false ) {
                string basicFilter = filter.MarketSingleComponentDataQuery( plan.id, (ModelDb.PlanType)plan.type );
                ValidationData vdata = Validator.ValidateComponentData( theDb, true, (ModelDb.PlanType)plan.type, basicFilter, plan );
                row[ "status" ] = vdata.Summary;
                row[ "num_values" ] = vdata.PointsCount;
            }

            componentPlanTable.Rows.Add(row);
 
            return true;
        }

        /// <summary>
        /// Handles a change in the select-all checlbox status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllCheckBox_CheckedChanged( object sender, EventArgs e ) 
        {
            componentDataGridView.SelectionChanged -= new EventHandler( componentList_SelectedIndexChanged );
            bool sel = selectAllCheckBox.Checked;
            if( sel == false ) {
                componentDataGridView.ClearSelection();      
            }
            else 
            {
                checkAll();   
            }

            componentDataGridView.SelectionChanged += new EventHandler( componentList_SelectedIndexChanged );

            updateSelectedIDs();

            if (SelectedRowChanged != null)
            {
                SelectedRowChanged( 0 );
            }
        }

        private void checkAll()
        {
            componentDataGridView.SelectAll(); 
        }

        /// <summary>
        /// Responds to a double-click by launching the edit dialog for the double-clicked component.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void componentDataGridView_DoubleClick( object sender, EventArgs e ) {
            EditPlan();
        }

        private void typeComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            string t = (string)typeComboBox.SelectedItem;

            // make the separator non-selectable
            if( t.StartsWith( "-" ) ){
                typeComboBox.SelectedIndex += 1;
                return;
            }

            switch( t ) {
                case "All":
                    this.defaultPlanType = ModelDb.PlanType.MarketPlan;
                    break;
                case "Coupons":
                    this.defaultPlanType = ModelDb.PlanType.Coupons;
                    break;
                case "Display":
                    this.defaultPlanType = ModelDb.PlanType.Display;
                    break;
                case "Distribution":
                    this.defaultPlanType = ModelDb.PlanType.Distribution;
                    break;
                case "Media":
                    this.defaultPlanType = ModelDb.PlanType.Media;
                    break;
                case "Price":
                    this.defaultPlanType = ModelDb.PlanType.Price;
                    break;
                case "External Factors":
                    this.defaultPlanType = ModelDb.PlanType.ProdEvent;
                    break;
                case "Market Utility":
                    this.defaultPlanType = ModelDb.PlanType.Market_Utility;
                    break;
            }

            Status status = new Status( this );
            status.UpdateUIAndContinue( typeComboBox_SelectedIndexChanged_Part2, chgTypeStatus, 50 );
        }

        private void typeComboBox_SelectedIndexChanged_Part2(){

            Utilities.Status.SetWaitCursor( this );

            this.UpdatePlanTable();

            updateSelectedIDs();

            if( SelectedRowChanged != null ) {
                SelectedRowChanged( 0 );
            }

            Utilities.Status.ClearStatus( this );
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
