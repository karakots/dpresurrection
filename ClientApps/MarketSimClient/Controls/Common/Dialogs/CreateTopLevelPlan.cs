using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using MrktSimDb;
using Common;
using Utilities;

namespace Common.Dialogs
{
	/// <summary>
	/// The diaog for creating or editing a market plan or market plan component.
	/// </summary>
    public class CreateTopLevelPlan : System.Windows.Forms.Form
	{
		MrktSimDBSchema.market_planRow currentPlan = null;

        private string createNewPlanTitle = "Create New Market Plan";
        private string editPlanTitle = "Editing Market Plan";
        private string noNameTitle = "Name Required";
        private string badNameTitle = "Name Invalid";
        private string noNameMessage = "You must enter a name for the Market Plan.";
        private string illegalCharsMessage = "The Market Plan name contains illegal characters.";
        private string illegalCharsMessage2 = "Illegal characters are:";
        private string illegalCharsMessage3 = " . , \' \" ;";
        private string noProductSelecteItem = "<Select a Product>";
        private string noBrandMessage = "You must select a Produict for the Market Plan.";
        private string noBrandTitle = "Product Required";

        private string helpTag = "CreateMarketPlan";
        private string editHelpTag = "EditMarketPlan";

        /// <summary>
        /// Sets or gets the ID of the brand selected in the pulldown list.
        /// </summary>
        public int ProductID {
            get {
                foreach( int brand_id in brands.Keys ) {
                    if( (string)brands[ brand_id ] == (string)brandComboBox.SelectedItem ) {
                        return brand_id;
                    }
                }
                return -2;         // error - should never happen
            }
        }

		/// <summary>
		/// Sets the default name for the plan.
		/// </summary>
		public string PlanName
		{
			set
			{
				nameBox.Text = value;
			}
		}

        /// <summary>
        /// Sets or gets the current market plan value. 
        /// </summary>
        /// <remarks> If a value is set to this property, the dialog performs an edit operation.  Otherwise a new plan is created.</remarks>
		public MrktSimDBSchema.market_planRow CurrentPlan
		{
			set
			{
                if( value == null ) {
                    return;
                }
                helpTag = editHelpTag;

				currentPlan = value;
                this.titleLabel.Text = editPlanTitle;

                string prodName = (string)brands[ currentPlan.product_id ];
                if( prodName == null ) {
                    prodName = currentPlan.product_id.ToString();
                }
                productLabel.Text = prodName;
                brandComboBox.SelectedItem = prodName;

                dateRangeLabel.Text = currentPlan.start_date.ToString( "d MMM yy" ) + " - " + 
                    currentPlan.end_date.ToString( "d MMM yy" );

                productLabel.Visible = true;
                startHdLabel.Visible = false;
                endHdLabel.Visible = false;
                startDateTimePicker.Visible = false;
                endDateTimePicker.Visible = false;

                brandComboBox.Visible = false;
                dateRangeLabel.Visible = true;
                datesLabel.Visible = true;

				nameBox.Text = currentPlan.name;
				descrBox.Text = currentPlan.descr;
			}
			get
			{
				return currentPlan;
			}
		}

        private ModelDb theDb;

        private ModelDb.PlanType planType = ModelDb.PlanType.MarketPlan;

        private ArrayList initialSelectedUserIndexes = new ArrayList();
        private ArrayList initialSelectedUsedIndexes = new ArrayList();

        #region UI Item Declarations
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Data.DataView productView;

        private Label removedLabel;
        private Label addedLabel;
        private Label availCompsLabel;
        private Label compsLabel;

        private DataTable scenarioTable;
        private DataTable componentTable;
        private DataView scenarioView;
        private DataView availScenarioView;
        private DataView componentsView;
        private DataView availComponentsView;
        private Panel panel1;
        private Label titleLabel;
        private SplitContainer splitContainer1;
        private Label label5;
        private Label endHdLabel;
        private ComboBox brandComboBox;
        private DateTimePicker endDateTimePicker;
        private DateTimePicker startDateTimePicker;
        private Label label3;
        private Label label1;
        private TextBox descrBox;
        private TextBox nameBox;
        private Label startHdLabel;
        private TabControl tabControl;
        private TabPage componentTabPage;
        private SplitContainer componentSplitContainer;
        private DataGridView usedDataGridView;
        private DataGridView availDataGridView;
        private TabPage scenarioTabPage;
        private SplitContainer scenarioSplitContainer;
        private DataGridView scenarioDataGridView;
        private DataGridView availScenarioDataGridView;
        private Label removedScenariosLabel;
        private Label addedScenariosLabel;
        private Label availScenariosLabel;
        private Label scenariosLabel;
        private Label label8;
        private Label label7;
        private Label label9;
        private Hashtable brands;
        private Button helpButton;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn ProdCol;
        private DataGridViewTextBoxColumn TypeCol;
        private DataGridViewTextBoxColumn IdCol;
        private DataGridViewTextBoxColumn SelCol;
        private DataGridViewTextBoxColumn OBrCol;
        private DataGridViewTextBoxColumn PrIdCol;
        private DataGridViewTextBoxColumn AvailNameCol;
        private DataGridViewTextBoxColumn AvProdCol;
        private DataGridViewTextBoxColumn AvailTypeCol;
        private DataGridViewTextBoxColumn AvailIdCol;
        private DataGridViewTextBoxColumn AvSelCol;
        private DataGridViewTextBoxColumn AvOBrCol;
        private DataGridViewTextBoxColumn AvPrIdCol;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn ScenarioIdCol;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn AvailScenarioIdCol;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private Label productLabel;
        private Label dateRangeLabel;
        private Label datesLabel;
        private Panel panel2;
        private Panel panel3;
        #endregion

        /// <summary>
        /// Returns the list of IDs of all scenarios that use this plan (the non-selected items in the used-scenario list plus items selected in the available list)
        /// </summary>
        public ArrayList AllUserIDs {
            get {
                ArrayList userIDs = new ArrayList();
                foreach( DataGridViewRow drow in this.scenarioDataGridView.Rows ) {
                    if( drow.Selected == false ) {      // don't include rows selected for removal
                        int id = (int)drow.Cells[ "ScenarioIdCol" ].Value;
                        userIDs.Add( id );
                    }
                }
                // include available rows selected for addition
                foreach( DataGridViewRow drow in this.availScenarioDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "AvailScenarioIdCol" ].Value;
                    userIDs.Add( id );
                }
                return userIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of all scenarios that use this plan (the non-selected items in the used-scenario list plus items selected in the available list)
        /// </summary>
        public ArrayList AllUsedIDs {
            get {
                ArrayList usedIDs = new ArrayList();
                foreach( DataGridViewRow drow in this.usedDataGridView.Rows ) {
                    if( drow.Selected == false ) {      // don't include rows selected for removal
                        int id = (int)drow.Cells[ "IdCol" ].Value;
                        usedIDs.Add( id );
                    }
                }
                // include available rows selected for addition
                foreach( DataGridViewRow drow in this.availDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "AvailIdCol" ].Value;
                    usedIDs.Add( id );
                }
                return usedIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of items selected in the available scenarios list.
        /// </summary>
        public ArrayList AddedUserIDs {
            get {
                ArrayList addedIDs = new ArrayList();
                foreach( DataGridViewRow drow in this.availScenarioDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "AvailScenarioIdCol" ].Value;
                    addedIDs.Add( id );
                }
                return addedIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of items selected in the currently-using scenarios list.
        /// </summary>
        public ArrayList RemovedUserIDs {
            get {
                ArrayList removedIDs = new ArrayList();
                foreach( DataGridViewRow drow in this.scenarioDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "ScenarioIdCol" ].Value;
                    removedIDs.Add( id );
                }
                return removedIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of items selected in the available components list.
        /// </summary>
        public ArrayList AddedUsedIDs {
            get {
                ArrayList addedIDs = new ArrayList();
                foreach( DataGridViewRow drow in availDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "AvailIdCol" ].Value;
                    addedIDs.Add( id );
                }
                return addedIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of items selected in the "used" components list.
        /// </summary>
        public ArrayList RemovedUsedIDs {
            get {
                ArrayList removedIDs = new ArrayList();
                foreach( DataGridViewRow drow in this.usedDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "IdCol" ].Value;
                    removedIDs.Add( id );
                }
                return removedIDs;
            }
        }

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Creates a new dialog for making a new or edited market plan or plan component.
        /// </summary>
        /// <param name="db"></param>
        public CreateTopLevelPlan( ModelDb db )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			theDb = db;
            this.titleLabel.Text = createNewPlanTitle;

            startDateTimePicker.Value = db.StartDate;
            endDateTimePicker.Value = db.EndDate;

            // setup product picker
            string filter = "product_id <> " + ModelDb.AllID;
            MrktSimDBSchema.productRow[] mprows = (MrktSimDBSchema.productRow[])db.Data.product.Select( filter );
            string[] prods = new string[ mprows.Length ];
            for( int i = 0; i < mprows.Length; i++ ){
                prods[ i ] = mprows[ i ].product_name;
            }
            Array.Sort( prods, mprows );

            brands = new Hashtable();
            for( int i = 0; i < mprows.Length; i++ ){
                brands.Add( mprows[ i ].product_id, mprows[ i ].product_name );
            }
            brandComboBox.Items.Add( noProductSelecteItem );
            brandComboBox.Items.AddRange( prods );
            brandComboBox.SelectedIndex = 0;

            startHdLabel.Visible = true;
            endHdLabel.Visible = true;
            startDateTimePicker.Visible = true;
            endDateTimePicker.Visible = true;
            brandComboBox.Visible = true;

            productLabel.Visible = false;
            dateRangeLabel.Visible = false;
            datesLabel.Visible = false;

            // create the data tables for the scenarios and components lists
            scenarioTable = new DataTable( "ScenarioTable" );

            DataColumn idCol = scenarioTable.Columns.Add( "id", typeof( int ) );
            scenarioTable.PrimaryKey = new DataColumn[] { idCol };

            scenarioTable.Columns.Add( "name", typeof( string ) );
            scenarioTable.Columns.Add( "product", typeof( string ) );
            scenarioTable.Columns.Add( "type", typeof( string ) );
            scenarioTable.Columns.Add( "selected", typeof( bool ) );
            scenarioTable.Columns.Add( "product_id", typeof( int ) );

            componentTable = new DataTable( "ComponentTable" );

            DataColumn idCol2 = componentTable.Columns.Add( "id", typeof( int ) );
            componentTable.PrimaryKey = new DataColumn[] { idCol2 };

            componentTable.Columns.Add( "name", typeof( string ) );
            componentTable.Columns.Add( "product", typeof( string ) );
            componentTable.Columns.Add( "type", typeof( string ) );
            componentTable.Columns.Add( "selected", typeof( bool ) );
            componentTable.Columns.Add( "product_id", typeof( int ) );

            scenarioView.Table = scenarioTable;
            availScenarioView.Table = scenarioTable;
            componentsView.Table = componentTable;
            availComponentsView.Table = componentTable;

            scenarioView.Sort = "name";
            availScenarioView.Sort = "name";
            componentsView.Sort = "name";
            availComponentsView.Sort = "name";

            SetViewFilters();

            usedDataGridView.DataSource = componentsView;
            availDataGridView.DataSource = availComponentsView;
            scenarioDataGridView.DataSource = scenarioView;
            availScenarioDataGridView.DataSource = availScenarioView;
        }

        /// <summary>
        /// Sets up the users and uses lists in the dialog according to the given lists of MarketPlanControlRelater.Item objects
        /// </summary>
        /// <param name="users"></param>
        /// <param name="used"></param>
        public void SetUsersAndUsedLists( ArrayList users, ArrayList used ) {
            // the users (scenarios) list
            for( int i = 0; i < users.Count; i++ ) {
                MarketPlanControlRelater.Item item = (MarketPlanControlRelater.Item)users[ i ];
                AddUserItem( item.Name, item.ID, item.ItemType, item.Selected, item.ProductID );
            }
            // the used items (components) list
            for( int i = 0; i < used.Count; i++ ) {
                MarketPlanControlRelater.Item item = (MarketPlanControlRelater.Item)used[ i ];
                AddUsedItem( item.Name, item.ID, item.ItemType, item.Selected, item.ProductID );
            }

            SetViewFilters();
        }

        /// <summary>
        /// Adds an item to the "users" list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        private void AddUserItem( string name, int id, ModelDb.PlanType type, bool selected, int productID ) {
            DataRow drow = scenarioTable.NewRow();
            drow[ "id" ] = id;
            drow[ "name" ] = name;
            drow[ "product_id" ] = productID;
            drow[ "product" ] = ProductNameForID( productID );
            drow[ "type" ] = type.ToString();
            drow[ "selected" ] = selected;
            scenarioTable.Rows.Add( drow );

            string nn = name;        // debug
            if( nn.Length > 25 ) {
                nn = nn.Substring( 0, 25 );
            }
        }

        /// <summary>
        /// Adds an item to the "used" list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        private void AddUsedItem( string name, int id, ModelDb.PlanType type, bool selected, int productID ) {
            DataRow drow = componentTable.NewRow();
            drow[ "id" ] = id;
            drow[ "name" ] = name;
            drow[ "product_id" ] = productID;
            drow[ "product" ] = ProductNameForID( productID );
            drow[ "type" ] = type.ToString();
            drow[ "selected" ] = selected;
            componentTable.Rows.Add( drow );

            string nn = name;        // debug
            if( nn.Length > 25 ) {
                nn = nn.Substring( 0, 25 );
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.productView = new System.Data.DataView();
            this.removedLabel = new System.Windows.Forms.Label();
            this.addedLabel = new System.Windows.Forms.Label();
            this.availCompsLabel = new System.Windows.Forms.Label();
            this.compsLabel = new System.Windows.Forms.Label();
            this.scenarioView = new System.Data.DataView();
            this.availScenarioView = new System.Data.DataView();
            this.componentsView = new System.Data.DataView();
            this.availComponentsView = new System.Data.DataView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.scenariosLabel = new System.Windows.Forms.Label();
            this.availScenariosLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.addedScenariosLabel = new System.Windows.Forms.Label();
            this.removedScenariosLabel = new System.Windows.Forms.Label();
            this.dateRangeLabel = new System.Windows.Forms.Label();
            this.datesLabel = new System.Windows.Forms.Label();
            this.productLabel = new System.Windows.Forms.Label();
            this.brandComboBox = new System.Windows.Forms.ComboBox();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.endHdLabel = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.startHdLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.componentTabPage = new System.Windows.Forms.TabPage();
            this.componentSplitContainer = new System.Windows.Forms.SplitContainer();
            this.usedDataGridView = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OBrCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availDataGridView = new System.Windows.Forms.DataGridView();
            this.AvailNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvProdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvSelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvOBrCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvPrIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scenarioTabPage = new System.Windows.Forms.TabPage();
            this.scenarioSplitContainer = new System.Windows.Forms.SplitContainer();
            this.scenarioDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availScenarioDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailScenarioIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.productView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.componentsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availComponentsView)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.componentTabPage.SuspendLayout();
            this.componentSplitContainer.Panel1.SuspendLayout();
            this.componentSplitContainer.Panel2.SuspendLayout();
            this.componentSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usedDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availDataGridView)).BeginInit();
            this.scenarioTabPage.SuspendLayout();
            this.scenarioSplitContainer.Panel1.SuspendLayout();
            this.scenarioSplitContainer.Panel2.SuspendLayout();
            this.scenarioSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.acceptButton.BackColor = System.Drawing.SystemColors.Control;
            this.acceptButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.acceptButton.Location = new System.Drawing.Point( 289, 371 );
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size( 75, 23 );
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = false;
            this.acceptButton.Click += new System.EventHandler( this.acceptButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.cancelButton.Location = new System.Drawing.Point( 382, 371 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // removedLabel
            // 
            this.removedLabel.AutoSize = true;
            this.removedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.removedLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.removedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.removedLabel.Location = new System.Drawing.Point( 13, 77 );
            this.removedLabel.Name = "removedLabel";
            this.removedLabel.Size = new System.Drawing.Size( 79, 15 );
            this.removedLabel.TabIndex = 55;
            this.removedLabel.Text = "188 removed";
            // 
            // addedLabel
            // 
            this.addedLabel.AutoSize = true;
            this.addedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.addedLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.addedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.addedLabel.Location = new System.Drawing.Point( 13, 58 );
            this.addedLabel.Name = "addedLabel";
            this.addedLabel.Size = new System.Drawing.Size( 66, 15 );
            this.addedLabel.TabIndex = 54;
            this.addedLabel.Text = "188 added";
            // 
            // availCompsLabel
            // 
            this.availCompsLabel.AutoSize = true;
            this.availCompsLabel.BackColor = System.Drawing.SystemColors.Control;
            this.availCompsLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.availCompsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.availCompsLabel.Location = new System.Drawing.Point( 13, 39 );
            this.availCompsLabel.Name = "availCompsLabel";
            this.availCompsLabel.Size = new System.Drawing.Size( 48, 15 );
            this.availCompsLabel.TabIndex = 53;
            this.availCompsLabel.Text = "188 not";
            // 
            // compsLabel
            // 
            this.compsLabel.AutoSize = true;
            this.compsLabel.BackColor = System.Drawing.SystemColors.Control;
            this.compsLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.compsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.compsLabel.Location = new System.Drawing.Point( 13, 21 );
            this.compsLabel.Name = "compsLabel";
            this.compsLabel.Size = new System.Drawing.Size( 78, 15 );
            this.compsLabel.TabIndex = 52;
            this.compsLabel.Text = "188 selected";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.panel1.Controls.Add( this.helpButton );
            this.panel1.Controls.Add( this.titleLabel );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 755, 26 );
            this.panel1.TabIndex = 58;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.Control;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 724, 2 );
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size( 24, 21 );
            this.helpButton.TabIndex = 61;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler( this.helpButton_Click );
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.titleLabel.Font = new System.Drawing.Font( "Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point( 5, 5 );
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 162, 16 );
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Create New Market Plan";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Location = new System.Drawing.Point( 7, 26 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(211)))), ((int)(((byte)(243)))), ((int)(((byte)(237)))) );
            this.splitContainer1.Panel1.Controls.Add( this.panel2 );
            this.splitContainer1.Panel1.Controls.Add( this.dateRangeLabel );
            this.splitContainer1.Panel1.Controls.Add( this.datesLabel );
            this.splitContainer1.Panel1.Controls.Add( this.productLabel );
            this.splitContainer1.Panel1.Controls.Add( this.brandComboBox );
            this.splitContainer1.Panel1.Controls.Add( this.endDateTimePicker );
            this.splitContainer1.Panel1.Controls.Add( this.label5 );
            this.splitContainer1.Panel1.Controls.Add( this.endHdLabel );
            this.splitContainer1.Panel1.Controls.Add( this.startDateTimePicker );
            this.splitContainer1.Panel1.Controls.Add( this.label3 );
            this.splitContainer1.Panel1.Controls.Add( this.label1 );
            this.splitContainer1.Panel1.Controls.Add( this.descrBox );
            this.splitContainer1.Panel1.Controls.Add( this.nameBox );
            this.splitContainer1.Panel1.Controls.Add( this.startHdLabel );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add( this.tabControl );
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding( 0, 8, 9, 8 );
            this.splitContainer1.Size = new System.Drawing.Size( 740, 340 );
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 59;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add( this.panel3 );
            this.panel2.Location = new System.Drawing.Point( 2, 224 );
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding( 10, 9, 0, 9 );
            this.panel2.Size = new System.Drawing.Size( 367, 116 );
            this.panel2.TabIndex = 67;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add( this.label8 );
            this.panel3.Controls.Add( this.compsLabel );
            this.panel3.Controls.Add( this.availCompsLabel );
            this.panel3.Controls.Add( this.addedLabel );
            this.panel3.Controls.Add( this.removedLabel );
            this.panel3.Controls.Add( this.scenariosLabel );
            this.panel3.Controls.Add( this.availScenariosLabel );
            this.panel3.Controls.Add( this.label7 );
            this.panel3.Controls.Add( this.addedScenariosLabel );
            this.panel3.Controls.Add( this.removedScenariosLabel );
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point( 10, 9 );
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size( 357, 98 );
            this.panel3.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point( 170, 3 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 70, 15 );
            this.label8.TabIndex = 61;
            this.label8.Text = "Scenarios:";
            // 
            // scenariosLabel
            // 
            this.scenariosLabel.AutoSize = true;
            this.scenariosLabel.BackColor = System.Drawing.SystemColors.Control;
            this.scenariosLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenariosLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.scenariosLabel.Location = new System.Drawing.Point( 176, 21 );
            this.scenariosLabel.Name = "scenariosLabel";
            this.scenariosLabel.Size = new System.Drawing.Size( 78, 15 );
            this.scenariosLabel.TabIndex = 56;
            this.scenariosLabel.Text = "188 selected";
            // 
            // availScenariosLabel
            // 
            this.availScenariosLabel.AutoSize = true;
            this.availScenariosLabel.BackColor = System.Drawing.SystemColors.Control;
            this.availScenariosLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.availScenariosLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.availScenariosLabel.Location = new System.Drawing.Point( 176, 39 );
            this.availScenariosLabel.Name = "availScenariosLabel";
            this.availScenariosLabel.Size = new System.Drawing.Size( 48, 15 );
            this.availScenariosLabel.TabIndex = 57;
            this.availScenariosLabel.Text = "188 not";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point( 7, 3 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 122, 17 );
            this.label7.TabIndex = 60;
            this.label7.Text = "Component Plans:";
            // 
            // addedScenariosLabel
            // 
            this.addedScenariosLabel.AutoSize = true;
            this.addedScenariosLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.addedScenariosLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.addedScenariosLabel.ForeColor = System.Drawing.Color.Yellow;
            this.addedScenariosLabel.Location = new System.Drawing.Point( 176, 58 );
            this.addedScenariosLabel.Name = "addedScenariosLabel";
            this.addedScenariosLabel.Size = new System.Drawing.Size( 66, 15 );
            this.addedScenariosLabel.TabIndex = 58;
            this.addedScenariosLabel.Text = "188 added";
            // 
            // removedScenariosLabel
            // 
            this.removedScenariosLabel.AutoSize = true;
            this.removedScenariosLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.removedScenariosLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.removedScenariosLabel.ForeColor = System.Drawing.Color.Yellow;
            this.removedScenariosLabel.Location = new System.Drawing.Point( 176, 77 );
            this.removedScenariosLabel.Name = "removedScenariosLabel";
            this.removedScenariosLabel.Size = new System.Drawing.Size( 79, 15 );
            this.removedScenariosLabel.TabIndex = 59;
            this.removedScenariosLabel.Text = "188 removed";
            // 
            // dateRangeLabel
            // 
            this.dateRangeLabel.AutoSize = true;
            this.dateRangeLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateRangeLabel.Location = new System.Drawing.Point( 85, 172 );
            this.dateRangeLabel.Name = "dateRangeLabel";
            this.dateRangeLabel.Size = new System.Drawing.Size( 116, 15 );
            this.dateRangeLabel.TabIndex = 63;
            this.dateRangeLabel.Text = "12/12/06 - 12/12//07";
            // 
            // datesLabel
            // 
            this.datesLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.datesLabel.Location = new System.Drawing.Point( 12, 172 );
            this.datesLabel.Name = "datesLabel";
            this.datesLabel.Size = new System.Drawing.Size( 61, 16 );
            this.datesLabel.TabIndex = 65;
            this.datesLabel.Text = "Dates:";
            // 
            // productLabel
            // 
            this.productLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.productLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(211)))), ((int)(((byte)(243)))), ((int)(((byte)(237)))) );
            this.productLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.productLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.productLabel.Location = new System.Drawing.Point( 86, 151 );
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size( 70, 15 );
            this.productLabel.TabIndex = 62;
            this.productLabel.Text = "Product Name";
            // 
            // brandComboBox
            // 
            this.brandComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.brandComboBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.brandComboBox.FormattingEnabled = true;
            this.brandComboBox.Location = new System.Drawing.Point( 89, 147 );
            this.brandComboBox.Name = "brandComboBox";
            this.brandComboBox.Size = new System.Drawing.Size( 254, 23 );
            this.brandComboBox.TabIndex = 46;
            this.brandComboBox.SelectedIndexChanged += new System.EventHandler( this.brandComboBox_SelectedIndexChanged );
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endDateTimePicker.CalendarFont = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDateTimePicker.Location = new System.Drawing.Point( 227, 181 );
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size( 99, 20 );
            this.endDateTimePicker.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label5.Location = new System.Drawing.Point( 12, 151 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 90, 23 );
            this.label5.TabIndex = 49;
            this.label5.Text = "Product";
            // 
            // endHdLabel
            // 
            this.endHdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endHdLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.endHdLabel.Location = new System.Drawing.Point( 191, 185 );
            this.endHdLabel.Name = "endHdLabel";
            this.endHdLabel.Size = new System.Drawing.Size( 47, 16 );
            this.endHdLabel.TabIndex = 48;
            this.endHdLabel.Text = "End";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.CalendarFont = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDateTimePicker.Location = new System.Drawing.Point( 51, 181 );
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size( 99, 20 );
            this.startDateTimePicker.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 12, 50 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 71, 23 );
            this.label3.TabIndex = 43;
            this.label3.Text = "Description";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 12, 20 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 76, 18 );
            this.label1.TabIndex = 42;
            this.label1.Text = "Plan Name";
            // 
            // descrBox
            // 
            this.descrBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.descrBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.descrBox.Location = new System.Drawing.Point( 89, 49 );
            this.descrBox.MaxLength = 200;
            this.descrBox.Multiline = true;
            this.descrBox.Name = "descrBox";
            this.descrBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descrBox.Size = new System.Drawing.Size( 254, 92 );
            this.descrBox.TabIndex = 41;
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nameBox.Location = new System.Drawing.Point( 89, 17 );
            this.nameBox.MaxLength = 100;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size( 254, 21 );
            this.nameBox.TabIndex = 40;
            // 
            // startHdLabel
            // 
            this.startHdLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.startHdLabel.Location = new System.Drawing.Point( 12, 185 );
            this.startHdLabel.Name = "startHdLabel";
            this.startHdLabel.Size = new System.Drawing.Size( 64, 16 );
            this.startHdLabel.TabIndex = 47;
            this.startHdLabel.Text = "Start";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add( this.componentTabPage );
            this.tabControl.Controls.Add( this.scenarioTabPage );
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.tabControl.Location = new System.Drawing.Point( 0, 8 );
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size( 361, 324 );
            this.tabControl.TabIndex = 57;
            this.tabControl.SelectedIndexChanged += new System.EventHandler( this.tabControl_SelectedIndexChanged );
            // 
            // componentTabPage
            // 
            this.componentTabPage.Controls.Add( this.componentSplitContainer );
            this.componentTabPage.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.componentTabPage.Location = new System.Drawing.Point( 4, 24 );
            this.componentTabPage.Name = "componentTabPage";
            this.componentTabPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.componentTabPage.Size = new System.Drawing.Size( 353, 296 );
            this.componentTabPage.TabIndex = 0;
            this.componentTabPage.Text = "  Component Plans  ";
            this.componentTabPage.UseVisualStyleBackColor = true;
            // 
            // componentSplitContainer
            // 
            this.componentSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentSplitContainer.Location = new System.Drawing.Point( 3, 3 );
            this.componentSplitContainer.Name = "componentSplitContainer";
            this.componentSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // componentSplitContainer.Panel1
            // 
            this.componentSplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.componentSplitContainer.Panel1.Controls.Add( this.usedDataGridView );
            this.componentSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding( 4 );
            // 
            // componentSplitContainer.Panel2
            // 
            this.componentSplitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.componentSplitContainer.Panel2.Controls.Add( this.availDataGridView );
            this.componentSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding( 4 );
            this.componentSplitContainer.Size = new System.Drawing.Size( 347, 290 );
            this.componentSplitContainer.SplitterDistance = 141;
            this.componentSplitContainer.TabIndex = 0;
            // 
            // usedDataGridView
            // 
            this.usedDataGridView.AllowUserToAddRows = false;
            this.usedDataGridView.AllowUserToDeleteRows = false;
            this.usedDataGridView.AllowUserToResizeRows = false;
            this.usedDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.usedDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.usedDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.usedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usedDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.NameCol,
            this.ProdCol,
            this.TypeCol,
            this.IdCol,
            this.SelCol,
            this.OBrCol,
            this.PrIdCol} );
            this.usedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usedDataGridView.Location = new System.Drawing.Point( 4, 4 );
            this.usedDataGridView.Name = "usedDataGridView";
            this.usedDataGridView.ReadOnly = true;
            this.usedDataGridView.RowHeadersVisible = false;
            this.usedDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.usedDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Yellow;
            this.usedDataGridView.RowTemplate.Height = 14;
            this.usedDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.usedDataGridView.Size = new System.Drawing.Size( 339, 133 );
            this.usedDataGridView.TabIndex = 56;
            this.usedDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // NameCol
            // 
            this.NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameCol.DataPropertyName = "name";
            this.NameCol.HeaderText = "INCLUDED";
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            this.NameCol.Width = 150;
            // 
            // ProdCol
            // 
            this.ProdCol.DataPropertyName = "product";
            this.ProdCol.HeaderText = "Product";
            this.ProdCol.Name = "ProdCol";
            this.ProdCol.ReadOnly = true;
            // 
            // TypeCol
            // 
            this.TypeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeCol.DataPropertyName = "type";
            this.TypeCol.HeaderText = "Type";
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.ReadOnly = true;
            // 
            // IdCol
            // 
            this.IdCol.DataPropertyName = "id";
            this.IdCol.HeaderText = "ID";
            this.IdCol.Name = "IdCol";
            this.IdCol.ReadOnly = true;
            this.IdCol.Visible = false;
            this.IdCol.Width = 70;
            // 
            // SelCol
            // 
            this.SelCol.DataPropertyName = "selected";
            this.SelCol.HeaderText = "Sel";
            this.SelCol.Name = "SelCol";
            this.SelCol.ReadOnly = true;
            this.SelCol.Visible = false;
            // 
            // OBrCol
            // 
            this.OBrCol.DataPropertyName = "other_brand";
            this.OBrCol.HeaderText = "Oth Br";
            this.OBrCol.Name = "OBrCol";
            this.OBrCol.ReadOnly = true;
            this.OBrCol.Visible = false;
            // 
            // PrIdCol
            // 
            this.PrIdCol.DataPropertyName = "product_id";
            this.PrIdCol.HeaderText = "PID";
            this.PrIdCol.Name = "PrIdCol";
            this.PrIdCol.ReadOnly = true;
            this.PrIdCol.Visible = false;
            // 
            // availDataGridView
            // 
            this.availDataGridView.AllowUserToAddRows = false;
            this.availDataGridView.AllowUserToDeleteRows = false;
            this.availDataGridView.AllowUserToResizeRows = false;
            this.availDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.availDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.availDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.availDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.AvailNameCol,
            this.AvProdCol,
            this.AvailTypeCol,
            this.AvailIdCol,
            this.AvSelCol,
            this.AvOBrCol,
            this.AvPrIdCol} );
            this.availDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.availDataGridView.Location = new System.Drawing.Point( 4, 4 );
            this.availDataGridView.Name = "availDataGridView";
            this.availDataGridView.ReadOnly = true;
            this.availDataGridView.RowHeadersVisible = false;
            this.availDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.availDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Yellow;
            this.availDataGridView.RowTemplate.Height = 14;
            this.availDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availDataGridView.Size = new System.Drawing.Size( 339, 137 );
            this.availDataGridView.TabIndex = 57;
            this.availDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // AvailNameCol
            // 
            this.AvailNameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AvailNameCol.DataPropertyName = "name";
            this.AvailNameCol.HeaderText = "AVAILABLE";
            this.AvailNameCol.Name = "AvailNameCol";
            this.AvailNameCol.ReadOnly = true;
            this.AvailNameCol.Width = 150;
            // 
            // AvProdCol
            // 
            this.AvProdCol.DataPropertyName = "product";
            this.AvProdCol.HeaderText = "Product";
            this.AvProdCol.Name = "AvProdCol";
            this.AvProdCol.ReadOnly = true;
            // 
            // AvailTypeCol
            // 
            this.AvailTypeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AvailTypeCol.DataPropertyName = "type";
            this.AvailTypeCol.HeaderText = "Type";
            this.AvailTypeCol.Name = "AvailTypeCol";
            this.AvailTypeCol.ReadOnly = true;
            // 
            // AvailIdCol
            // 
            this.AvailIdCol.DataPropertyName = "id";
            this.AvailIdCol.HeaderText = "ID";
            this.AvailIdCol.Name = "AvailIdCol";
            this.AvailIdCol.ReadOnly = true;
            this.AvailIdCol.Visible = false;
            this.AvailIdCol.Width = 70;
            // 
            // AvSelCol
            // 
            this.AvSelCol.DataPropertyName = "selected";
            this.AvSelCol.HeaderText = "Sel";
            this.AvSelCol.Name = "AvSelCol";
            this.AvSelCol.ReadOnly = true;
            this.AvSelCol.Visible = false;
            // 
            // AvOBrCol
            // 
            this.AvOBrCol.DataPropertyName = "other_brand";
            this.AvOBrCol.HeaderText = "OBr";
            this.AvOBrCol.Name = "AvOBrCol";
            this.AvOBrCol.ReadOnly = true;
            this.AvOBrCol.Visible = false;
            // 
            // AvPrIdCol
            // 
            this.AvPrIdCol.DataPropertyName = "product_id";
            this.AvPrIdCol.HeaderText = "PID";
            this.AvPrIdCol.Name = "AvPrIdCol";
            this.AvPrIdCol.ReadOnly = true;
            this.AvPrIdCol.Visible = false;
            // 
            // scenarioTabPage
            // 
            this.scenarioTabPage.Controls.Add( this.scenarioSplitContainer );
            this.scenarioTabPage.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenarioTabPage.Location = new System.Drawing.Point( 4, 24 );
            this.scenarioTabPage.Name = "scenarioTabPage";
            this.scenarioTabPage.Padding = new System.Windows.Forms.Padding( 3 );
            this.scenarioTabPage.Size = new System.Drawing.Size( 353, 296 );
            this.scenarioTabPage.TabIndex = 1;
            this.scenarioTabPage.Text = "   Scenarios  ";
            this.scenarioTabPage.UseVisualStyleBackColor = true;
            // 
            // scenarioSplitContainer
            // 
            this.scenarioSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioSplitContainer.Location = new System.Drawing.Point( 3, 3 );
            this.scenarioSplitContainer.Name = "scenarioSplitContainer";
            this.scenarioSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scenarioSplitContainer.Panel1
            // 
            this.scenarioSplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.scenarioSplitContainer.Panel1.Controls.Add( this.scenarioDataGridView );
            this.scenarioSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding( 4 );
            // 
            // scenarioSplitContainer.Panel2
            // 
            this.scenarioSplitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.scenarioSplitContainer.Panel2.Controls.Add( this.availScenarioDataGridView );
            this.scenarioSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding( 4 );
            this.scenarioSplitContainer.Size = new System.Drawing.Size( 347, 290 );
            this.scenarioSplitContainer.SplitterDistance = 141;
            this.scenarioSplitContainer.TabIndex = 1;
            // 
            // scenarioDataGridView
            // 
            this.scenarioDataGridView.AllowUserToAddRows = false;
            this.scenarioDataGridView.AllowUserToDeleteRows = false;
            this.scenarioDataGridView.AllowUserToResizeRows = false;
            this.scenarioDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.scenarioDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scenarioDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scenarioDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.scenarioDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scenarioDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.ScenarioIdCol,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14} );
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scenarioDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.scenarioDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioDataGridView.Location = new System.Drawing.Point( 4, 4 );
            this.scenarioDataGridView.Name = "scenarioDataGridView";
            this.scenarioDataGridView.ReadOnly = true;
            this.scenarioDataGridView.RowHeadersVisible = false;
            this.scenarioDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.scenarioDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Yellow;
            this.scenarioDataGridView.RowTemplate.Height = 14;
            this.scenarioDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scenarioDataGridView.Size = new System.Drawing.Size( 339, 133 );
            this.scenarioDataGridView.TabIndex = 56;
            this.scenarioDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn8.HeaderText = "USING this Plan";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "product";
            this.dataGridViewTextBoxColumn9.HeaderText = "Product";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn10.DataPropertyName = "type";
            this.dataGridViewTextBoxColumn10.HeaderText = "Type";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 115;
            // 
            // ScenarioIdCol
            // 
            this.ScenarioIdCol.DataPropertyName = "id";
            this.ScenarioIdCol.HeaderText = "ID";
            this.ScenarioIdCol.Name = "ScenarioIdCol";
            this.ScenarioIdCol.ReadOnly = true;
            this.ScenarioIdCol.Visible = false;
            this.ScenarioIdCol.Width = 70;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "selected";
            this.dataGridViewTextBoxColumn12.HeaderText = "Sel";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "other_brand";
            this.dataGridViewTextBoxColumn13.HeaderText = "Oth Br";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "product_id";
            this.dataGridViewTextBoxColumn14.HeaderText = "PID";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // availScenarioDataGridView
            // 
            this.availScenarioDataGridView.AllowUserToAddRows = false;
            this.availScenarioDataGridView.AllowUserToDeleteRows = false;
            this.availScenarioDataGridView.AllowUserToResizeRows = false;
            this.availScenarioDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.availScenarioDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.availScenarioDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.availScenarioDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availScenarioDataGridView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.AvailScenarioIdCol,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7} );
            this.availScenarioDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.availScenarioDataGridView.Location = new System.Drawing.Point( 4, 4 );
            this.availScenarioDataGridView.Name = "availScenarioDataGridView";
            this.availScenarioDataGridView.ReadOnly = true;
            this.availScenarioDataGridView.RowHeadersVisible = false;
            this.availScenarioDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.availScenarioDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Yellow;
            this.availScenarioDataGridView.RowTemplate.Height = 14;
            this.availScenarioDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availScenarioDataGridView.Size = new System.Drawing.Size( 339, 137 );
            this.availScenarioDataGridView.TabIndex = 57;
            this.availScenarioDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn1.HeaderText = "NOT USING this Plan";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "product";
            this.dataGridViewTextBoxColumn2.HeaderText = "Product";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "type";
            this.dataGridViewTextBoxColumn3.HeaderText = "Type";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 115;
            // 
            // AvailScenarioIdCol
            // 
            this.AvailScenarioIdCol.DataPropertyName = "id";
            this.AvailScenarioIdCol.HeaderText = "ID";
            this.AvailScenarioIdCol.Name = "AvailScenarioIdCol";
            this.AvailScenarioIdCol.ReadOnly = true;
            this.AvailScenarioIdCol.Visible = false;
            this.AvailScenarioIdCol.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "selected";
            this.dataGridViewTextBoxColumn5.HeaderText = "Sel";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "other_brand";
            this.dataGridViewTextBoxColumn6.HeaderText = "OBr";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "product_id";
            this.dataGridViewTextBoxColumn7.HeaderText = "PID";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.label9.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label9.Location = new System.Drawing.Point( 479, 366 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 265, 14 );
            this.label9.TabIndex = 60;
            this.label9.Text = "(use Control-click to multi-select or de-select items)";
            // 
            // CreateTopLevelPlan
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 755, 400 );
            this.Controls.Add( this.label9 );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.acceptButton );
            this.Controls.Add( this.splitContainer1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CreateTopLevelPlan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler( this.CreateComponentPlan2_Load );
            ((System.ComponentModel.ISupportInitialize)(this.productView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.componentsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availComponentsView)).EndInit();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.panel2.ResumeLayout( false );
            this.panel3.ResumeLayout( false );
            this.panel3.PerformLayout();
            this.tabControl.ResumeLayout( false );
            this.componentTabPage.ResumeLayout( false );
            this.componentSplitContainer.Panel1.ResumeLayout( false );
            this.componentSplitContainer.Panel2.ResumeLayout( false );
            this.componentSplitContainer.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.usedDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availDataGridView)).EndInit();
            this.scenarioTabPage.ResumeLayout( false );
            this.scenarioSplitContainer.Panel1.ResumeLayout( false );
            this.scenarioSplitContainer.Panel2.ResumeLayout( false );
            this.scenarioSplitContainer.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.scenarioDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioDataGridView)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

        #region OK-Button Handler
        /// <summary>
        /// This method does the real work of creating a new or copied market plan or plan component.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void acceptButton_Click(object sender, System.EventArgs e)
		{
            // be sure that a name is entered
            if( this.nameBox.Text.Trim().Length == 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( noNameMessage, noNameTitle );
                cdlg.Width -= 75;
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            // be sure that a product is selected
            if( this.brandComboBox.SelectedIndex == 0 ){
                ConfirmDialog cdlg = new ConfirmDialog( noBrandMessage, noBrandTitle );
                cdlg.Width -= 75;
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            // validate the name
            char[] illegal = { ',', '\'', '"', '.', ';' };
            int chkIndex = nameBox.Text.IndexOfAny( illegal );
            if( chkIndex >= 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( illegalCharsMessage, illegalCharsMessage2, illegalCharsMessage3, badNameTitle );
                cdlg.SetOkButtonOnlyStyle();
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

			if (currentPlan == null)
			{
				currentPlan = theDb.CreateMarketPlan(nameBox.Text, planType);
			}
			else 
			{
                if( currentPlan.name != nameBox.Text ) {
                    currentPlan.name = Database.CreateUniqueName( theDb.Data.market_plan, "name", nameBox.Text, null );
                }
			}

            currentPlan.product_id = this.ProductID;
            currentPlan.start_date = startDateTimePicker.Value;
            currentPlan.end_date = endDateTimePicker.Value;

			currentPlan.descr = descrBox.Text;
			currentPlan.type = (byte) planType;

			this.DialogResult = DialogResult.OK;
        }

        ////private void modifyButton_Click( object sender, System.EventArgs e ) {
        ////    // be sure that a name is entered
        ////    if( this.nameBox.Text.Trim().Length == 0 ) {
        ////        ConfirmDialog cdlg = new ConfirmDialog( noNameMessage, noNameTitle );
        ////        cdlg.ShowDialog();
        ////        this.DialogResult = DialogResult.None;
        ////        return;
        ////    }

        ////    // validate the name
        ////    char[] illegal = { ',', '\'', '"', '.', ';' };
        ////    int chkIndex = nameBox.Text.IndexOfAny( illegal );
        ////    if( chkIndex >= 0 ) {
        ////        ConfirmDialog cdlg = new ConfirmDialog( illegalCharsMessage, illegalCharsMessage2, illegalCharsMessage3, badNameTitle );
        ////        cdlg.SetOkButtonOnlyStyle();
        ////        cdlg.ShowDialog();
        ////        this.DialogResult = DialogResult.None;
        ////        return;
        ////    }

        ////    ChangePlanDates modifyDialog = new ChangePlanDates( theDb, this.currentPlan );
        ////    DialogResult resp = modifyDialog.ShowDialog();
        ////    if( resp == DialogResult.Cancel ) {
        ////        this.DialogResult = DialogResult.None;
        ////        return;
        ////    }

        ////    int newPlanProductID = modifyDialog.ProductID;
        ////    int newPlanShiftDays = modifyDialog.ShiftAmountDays;

        ////    if( currentPlan.name != nameBox.Text ) {
        ////        currentPlan.name = Database.CreateUniqueName( theDb.Data.market_plan, "name", nameBox.Text, null );
        ////    }

        ////    currentPlan.product_id = newPlanProductID;
        ////    currentPlan.start_date = currentPlan.start_date.AddDays( newPlanShiftDays );
        ////    currentPlan.end_date = currentPlan.end_date.AddDays( newPlanShiftDays );

        ////    currentPlan.descr = descrBox.Text;
        ////    currentPlan.type = (byte)this.planType;

        ////    theDb.UpdateMarketPlan( currentPlan, newPlanShiftDays );         // this will create a ccpy of the plan, all components, and all component data

        ////    this.DialogResult = DialogResult.OK;
        ////}
        #endregion

        /// <summary>
        /// Updates the labels to match the view state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedIndexChanged( object sender, EventArgs e ) {
            UpdateLabels();
        }

        /// <summary>
        /// Updates the labels to match the view state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateComponentPlan2_Load( object sender, EventArgs e ) {
            SetViewFilters();
        }

        /// <summary>
        /// Configures the row filters for the items table to show the appropriate lists
        /// </summary>
        private void SetViewFilters() {
            SetViewFilters( -1 );
        }

        /// <summary>
        /// Configures the row filters for the items table to show the appropriate lists
        /// </summary>
        private void SetViewFilters( int prodictID ) {
            string compFilter = "selected = true AND type <> 'ProdEvent'";
            string availCompFilter = "selected = false AND type <> 'ProdEvent'";
            string scenFilter = "selected = true";
            string availScenFilter = "selected = false";
            if( brands.ContainsKey( prodictID ) ){
                availCompFilter += " AND product = '" + (string)brands[ prodictID ] + "'";
            }

            this.componentsView.RowFilter = compFilter;
            this.availComponentsView.RowFilter = availCompFilter;
            this.scenarioView.RowFilter = scenFilter;
            this.availScenarioView.RowFilter = availScenFilter;

            this.usedDataGridView.ClearSelection();
            this.availDataGridView.ClearSelection();
            this.scenarioDataGridView.ClearSelection();
            this.availScenarioDataGridView.ClearSelection();
            UpdateLabels();
        }

        /// <summary>
        /// Updates the labels to match the view state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLabels() {
            compsLabel.Text = String.Format( "{0} in this Market Plan", this.usedDataGridView.RowCount );
            availCompsLabel.Text = String.Format( "{0} availablle", this.availDataGridView.RowCount );
            int c1 = this.availDataGridView.SelectedRows.Count;
            if( c1 > 0 ) {
                addedLabel.Text = String.Format( "{0} to be added", c1 );
                addedLabel.Visible = true;
            }
            else {
                addedLabel.Visible = false;
            }
            int c2 = this.usedDataGridView.SelectedRows.Count;
            if( c2 > 0 ) {
                removedLabel.Text = String.Format( "{0} to be removed", c2 );
                removedLabel.Visible = true;
            }
            else {
                removedLabel.Visible = false;
            }

            if( this.scenarioDataGridView.RowCount == 1 ) {
                scenariosLabel.Text = String.Format( "1 is using this Market Plan" );
            }
            else {
                scenariosLabel.Text = String.Format( "{0} are using this Market Plan", this.scenarioDataGridView.RowCount );
            }
            if( this.availScenarioDataGridView.RowCount == 1 ) {
                availScenariosLabel.Text = String.Format( "1 is not" );
            }
            else {
                availScenariosLabel.Text = String.Format( "{0} are not", this.availScenarioDataGridView.RowCount );
            }
            int c3 = this.availScenarioDataGridView.SelectedRows.Count;
            if( c3 > 0 ) {
                addedScenariosLabel.Text = String.Format( "{0} to be added to", c3 );
                addedScenariosLabel.Visible = true;
            }
            else {
                 addedScenariosLabel.Visible = false;
           }
            int c4 = this.scenarioDataGridView.SelectedRows.Count;
            if( c4 > 0 ) {
                removedScenariosLabel.Text = String.Format( "{0} to be removed from", c4 );
                removedScenariosLabel.Visible = true;
            }
            else {
                removedScenariosLabel.Visible = false;
            }
        }

        private bool b = false;

        private void tabControl_SelectedIndexChanged( object sender, EventArgs e ) {
            if( tabControl.SelectedIndex == 1 ) {
                if( b == false ) {
                    this.scenarioDataGridView.ClearSelection();
                    this.availScenarioDataGridView.ClearSelection();

                    b = true;
                }
            }
        }

        private String ProductNameForID( int productID ) {
            return (string)brands[ productID ];
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        private void brandComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            int prod_id = this.ProductID;
            SetViewFilters( prod_id );
        }
    }
}
