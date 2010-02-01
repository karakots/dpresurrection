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
	public class CreateComponentPlan2 : System.Windows.Forms.Form
	{
		MrktSimDBSchema.market_planRow currentPlan = null;

        private string noNameMessage = "You must enter a name for the Plan Component.";
        private string noNameTitle = "Name Required";
        private string illegalCharsTitle = "Name Invalid";
        private string illegalCharsMessage = "The Plan Component name contains illegal characters.";
        private string illegalCharsMessage2 = "Illegal characters are:";
        private string illegalCharsMessage3 = " . , \' \" ;";

        private string stats1MsgS = "{0} is using this Plan";
        private string stats1Msg = "{0} are using this Plan";
        private string stats2MsgS = "{0} is not";
        private string stats2Msg = "{0} are not";
        private string statsAddMsg = "{0} adding this Plan";
        private string statsRemMsg = "{0} removing this Plan";

        private string helpTag = "CreateComponentPlan";

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
				currentPlan = value;

                switch( planType ) {
                    case ModelDb.PlanType.Coupons:
                        titleLabel.Text = "Editing Coupons Plan";
                        break;
                    case ModelDb.PlanType.Price:
                        titleLabel.Text = "Editing Price Plan";
                        break;
                    case ModelDb.PlanType.Distribution:
                        titleLabel.Text = "Editing Distribution Plan";
                        break;
                    case ModelDb.PlanType.Display:
                        titleLabel.Text = "Editing Display Plan";
                        break;
                    case ModelDb.PlanType.Media:
                        titleLabel.Text = "Editing Media Plan";
                        break;
                }
                helpTag = "EditComponentPlan";

                string prodName = (string)brands[ currentPlan.product_id ];
                if( prodName == null ) {
                    prodName = currentPlan.product_id.ToString();
                }
                brandLabel.Text = prodName;

                startLabel.Text = currentPlan.start_date.ToString( "d MMM yy" );
                endLabel.Text = currentPlan.end_date.ToString( "d MMM yy" );

				nameBox.Text = currentPlan.name;
				descrBox.Text = currentPlan.descr;
			}
			get
			{
				return currentPlan;
			}
		}

        /// <summary>
        /// Setting this property makes the display be configured appropriately for the given plan type.
        /// </summary>
        /// <remarks>Set this only once per instance -- not set up for general switching between plan types.</remarks>
		public ModelDb.PlanType Type
		{
			set
			{
				planType = value;
				switch(planType)
				{
                    case ModelDb.PlanType.Coupons:
                        titleLabel.Text = "Create Coupons Plan";
                        break;
                    case ModelDb.PlanType.Price:
                        titleLabel.Text = "Create Price Plan";
                        break;
                    case ModelDb.PlanType.Distribution:
                        titleLabel.Text = "Create Distribution Plan";
                        break;
					case ModelDb.PlanType.Display:
                        titleLabel.Text = "Create Display Plan";
                        break;
					case ModelDb.PlanType.Media:
                        titleLabel.Text = "Create Media Plan";
                        break;
				}
			}
		}

        private ModelDb theDb;


        private ModelDb.PlanType planType;

        private ArrayList initialSelectedUserIndexes = new ArrayList();
        private ArrayList initialSelectedUsedIndexes = new ArrayList();

        #region UI Item Declarations
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Data.DataView productView;
        private DataTable plansTable;
        private DataView plansView;
        private DataView availPlansView;
        private Panel panel1;
        private Label titleLabel;
        private SplitContainer splitContainer1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label1;
        private TextBox descrBox;
        private TextBox nameBox;
        private Label label2;
        private Label label9;
        private Hashtable brands;
        private SplitContainer planSplitContainer;
        private DataGridView scenarioDataGridView;
        private DataGridView availScenarioDataGridView;
        private Label label8;
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
        private Button helpButton;
        private Label endLabel;
        private Label startLabel;
        private Label brandLabel;
        private Panel panel2;
        private Label label7;
        private Label removedLabel;
        private Label addedLabel;
        private Label availCompsLabel;
        private Label compsLabel;
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
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Creates a new dialog for making a new or edited market plan or plan component.
        /// </summary>
        /// <param name="db"></param>
        public CreateComponentPlan2( ModelDb db )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			theDb = db;

            startLabel.Text = "--";
            endLabel.Text = "--";

            brands = new Hashtable();
            foreach( MrktSimDBSchema.productRow prow in theDb.Data.product ){
                brands.Add( prow.product_id, prow.product_name );
            }

            // create the data tables for the scenarios and components lists
            plansTable = new DataTable( "ScenarioTable" );

            DataColumn idCol = plansTable.Columns.Add( "id", typeof( int ) );
            plansTable.PrimaryKey = new DataColumn[] { idCol };

            plansTable.Columns.Add( "name", typeof( string ) );
            plansTable.Columns.Add( "product", typeof( string ) );
            plansTable.Columns.Add( "type", typeof( string ) );
            plansTable.Columns.Add( "selected", typeof( bool ) );
            plansTable.Columns.Add( "product_id", typeof( int ) );

            plansView.Table = plansTable;
            availPlansView.Table = plansTable;

            plansView.Sort = "name";
            availPlansView.Sort = "name";
            
            SetViewFilters();

            scenarioDataGridView.DataSource = plansView;
            availScenarioDataGridView.DataSource = availPlansView;
        }

        /// <summary>
        /// Sets up the users and uses lists in the dialog according to the given lists of MarketPlanControlRelater.Item objects
        /// </summary>
        /// <param name="users"></param>
        /// <param name="used"></param>
        public void SetUsersAndUsedLists( ArrayList users, ArrayList used ) {
            // the users (top-level plans) list
            for( int i = 0; i < users.Count; i++ ) {
                MarketPlanControlRelater.Item item = (MarketPlanControlRelater.Item)users[ i ];
                AddUserItem( item.Name, item.ID, item.ItemType, item.Selected, item.ProductID );
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
            DataRow drow = plansTable.NewRow();
            drow[ "id" ] = id;
            drow[ "name" ] = name;
            drow[ "product_id" ] = productID;
            drow[ "product" ] = GetProductName( productID );
            drow[ "type" ] = type.ToString();
            drow[ "selected" ] = selected;
            plansTable.Rows.Add( drow );

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
            this.plansView = new System.Data.DataView();
            this.availPlansView = new System.Data.DataView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.removedLabel = new System.Windows.Forms.Label();
            this.addedLabel = new System.Windows.Forms.Label();
            this.availCompsLabel = new System.Windows.Forms.Label();
            this.compsLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.brandLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.planSplitContainer = new System.Windows.Forms.SplitContainer();
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
            ((System.ComponentModel.ISupportInitialize)(this.plansView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availPlansView)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.planSplitContainer.Panel1.SuspendLayout();
            this.planSplitContainer.Panel2.SuspendLayout();
            this.planSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.acceptButton.BackColor = System.Drawing.SystemColors.Control;
            this.acceptButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.acceptButton.Location = new System.Drawing.Point( 289, 329 );
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
            this.cancelButton.Location = new System.Drawing.Point( 382, 329 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
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
            this.helpButton.TabIndex = 13;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler( this.helpButton_Click );
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.titleLabel.Font = new System.Drawing.Font( "Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point( 5, 5 );
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 160, 16 );
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Create Plan Component";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.splitContainer1.Location = new System.Drawing.Point( 7, 26 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add( this.panel2 );
            this.splitContainer1.Panel1.Controls.Add( this.endLabel );
            this.splitContainer1.Panel1.Controls.Add( this.startLabel );
            this.splitContainer1.Panel1.Controls.Add( this.brandLabel );
            this.splitContainer1.Panel1.Controls.Add( this.label5 );
            this.splitContainer1.Panel1.Controls.Add( this.label4 );
            this.splitContainer1.Panel1.Controls.Add( this.label3 );
            this.splitContainer1.Panel1.Controls.Add( this.label1 );
            this.splitContainer1.Panel1.Controls.Add( this.descrBox );
            this.splitContainer1.Panel1.Controls.Add( this.nameBox );
            this.splitContainer1.Panel1.Controls.Add( this.label2 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.splitContainer1.Panel2.Controls.Add( this.label8 );
            this.splitContainer1.Panel2.Controls.Add( this.planSplitContainer );
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding( 8, 8, 4, 8 );
            this.splitContainer1.Size = new System.Drawing.Size( 740, 298 );
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 59;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.panel2.Controls.Add( this.label7 );
            this.panel2.Controls.Add( this.removedLabel );
            this.panel2.Controls.Add( this.addedLabel );
            this.panel2.Controls.Add( this.availCompsLabel );
            this.panel2.Controls.Add( this.compsLabel );
            this.panel2.Location = new System.Drawing.Point( 216, 160 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 149, 138 );
            this.panel2.TabIndex = 64;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.label7.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point( 5, 12 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 118, 15 );
            this.label7.TabIndex = 65;
            this.label7.Text = "Top-Level Plans";
            // 
            // removedLabel
            // 
            this.removedLabel.AutoSize = true;
            this.removedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.removedLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.removedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.removedLabel.Location = new System.Drawing.Point( 11, 95 );
            this.removedLabel.Name = "removedLabel";
            this.removedLabel.Size = new System.Drawing.Size( 79, 15 );
            this.removedLabel.TabIndex = 64;
            this.removedLabel.Text = "188 removed";
            // 
            // addedLabel
            // 
            this.addedLabel.AutoSize = true;
            this.addedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.addedLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.addedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.addedLabel.Location = new System.Drawing.Point( 11, 73 );
            this.addedLabel.Name = "addedLabel";
            this.addedLabel.Size = new System.Drawing.Size( 66, 15 );
            this.addedLabel.TabIndex = 63;
            this.addedLabel.Text = "188 added";
            // 
            // availCompsLabel
            // 
            this.availCompsLabel.AutoSize = true;
            this.availCompsLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.availCompsLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.availCompsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.availCompsLabel.Location = new System.Drawing.Point( 11, 50 );
            this.availCompsLabel.Name = "availCompsLabel";
            this.availCompsLabel.Size = new System.Drawing.Size( 48, 15 );
            this.availCompsLabel.TabIndex = 62;
            this.availCompsLabel.Text = "188 not";
            // 
            // compsLabel
            // 
            this.compsLabel.AutoSize = true;
            this.compsLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.compsLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.compsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.compsLabel.Location = new System.Drawing.Point( 11, 33 );
            this.compsLabel.Name = "compsLabel";
            this.compsLabel.Size = new System.Drawing.Size( 78, 15 );
            this.compsLabel.TabIndex = 61;
            this.compsLabel.Text = "188 selected";
            // 
            // endLabel
            // 
            this.endLabel.AutoSize = true;
            this.endLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.endLabel.Location = new System.Drawing.Point( 72, 218 );
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size( 59, 15 );
            this.endLabel.TabIndex = 63;
            this.endLabel.Text = "endLabel";
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.startLabel.Location = new System.Drawing.Point( 72, 197 );
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size( 62, 15 );
            this.startLabel.TabIndex = 62;
            this.startLabel.Text = "startLabel";
            // 
            // brandLabel
            // 
            this.brandLabel.AutoSize = true;
            this.brandLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.brandLabel.Location = new System.Drawing.Point( 72, 167 );
            this.brandLabel.Name = "brandLabel";
            this.brandLabel.Size = new System.Drawing.Size( 71, 15 );
            this.brandLabel.TabIndex = 61;
            this.brandLabel.Text = "brandLabel";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label5.Location = new System.Drawing.Point( 12, 167 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 90, 16 );
            this.label5.TabIndex = 49;
            this.label5.Text = "Product";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label4.Location = new System.Drawing.Point( 12, 218 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 47, 17 );
            this.label4.TabIndex = 48;
            this.label4.Text = "End";
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
            this.label1.Text = "Name";
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
            // label2
            // 
            this.label2.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 12, 197 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 64, 23 );
            this.label2.TabIndex = 47;
            this.label2.Text = "Start";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(237)))) );
            this.label8.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point( 7, 6 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 157, 14 );
            this.label8.TabIndex = 61;
            this.label8.Text = "Top-Level Plan Relations";
            // 
            // planSplitContainer
            // 
            this.planSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.planSplitContainer.Location = new System.Drawing.Point( 6, 25 );
            this.planSplitContainer.Name = "planSplitContainer";
            this.planSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // planSplitContainer.Panel1
            // 
            this.planSplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.planSplitContainer.Panel1.Controls.Add( this.scenarioDataGridView );
            this.planSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding( 4 );
            // 
            // planSplitContainer.Panel2
            // 
            this.planSplitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.planSplitContainer.Panel2.Controls.Add( this.availScenarioDataGridView );
            this.planSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding( 4 );
            this.planSplitContainer.Size = new System.Drawing.Size( 357, 265 );
            this.planSplitContainer.SplitterDistance = 128;
            this.planSplitContainer.TabIndex = 2;
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
            this.scenarioDataGridView.Size = new System.Drawing.Size( 349, 120 );
            this.scenarioDataGridView.TabIndex = 56;
            this.scenarioDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn8.HeaderText = "USING this Component";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "product";
            this.dataGridViewTextBoxColumn9.HeaderText = "Product";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
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
            this.availScenarioDataGridView.Size = new System.Drawing.Size( 349, 125 );
            this.availScenarioDataGridView.TabIndex = 57;
            this.availScenarioDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn1.HeaderText = "NOT USING this Component";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "product";
            this.dataGridViewTextBoxColumn2.HeaderText = "Product";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
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
            this.label9.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.label9.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label9.Location = new System.Drawing.Point( 485, 324 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 265, 14 );
            this.label9.TabIndex = 60;
            this.label9.Text = "(use Control-click to multi-select or de-select items)";
            // 
            // CreateComponentPlan2
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 755, 358 );
            this.Controls.Add( this.label9 );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.acceptButton );
            this.Controls.Add( this.splitContainer1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CreateComponentPlan2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler( this.CreateComponentPlan2_Load );
            ((System.ComponentModel.ISupportInitialize)(this.productView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plansView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availPlansView)).EndInit();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            this.planSplitContainer.Panel1.ResumeLayout( false );
            this.planSplitContainer.Panel2.ResumeLayout( false );
            this.planSplitContainer.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.scenarioDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availScenarioDataGridView)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

        #region OK-Button Handler
        /// <summary>
        /// This method does the real work of creating a new plan component.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void acceptButton_Click(object sender, System.EventArgs e)
		{
            // be sure that a name is entered
            if( this.nameBox.Text.Trim().Length == 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( noNameMessage, noNameTitle );
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            // validate the name
            char[] illegal = { ',', '\'', '"', '.', ';' };
            int chkIndex = nameBox.Text.IndexOfAny( illegal );
            if( chkIndex >= 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( illegalCharsMessage, illegalCharsMessage2, illegalCharsMessage3, illegalCharsTitle );
                cdlg.SetOkButtonOnlyStyle();
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            int productID = ModelDb.AllID;
            DateTime startDate = currentPlan.start_date;
            DateTime endDate = currentPlan.end_date;

            if( currentPlan == null ) {
                //currentPlan = theDb.CreateMarketPlan( nameBox.Text, planType );
                throw new Exception( "Error: Attempt to use CreateComponentPlan2 to create a new Component (this form is for editing only)" );
            }
            else {
                productID = currentPlan.product_id;
                if( currentPlan.name != nameBox.Text ) {
                    currentPlan.name = Database.CreateUniqueName( theDb.Data.market_plan, "name", nameBox.Text, null );
                }
            }
			currentPlan.descr = descrBox.Text;

            // be sure these values didn't change
            currentPlan.product_id = productID;
            currentPlan.start_date = startDate;
            currentPlan.end_date = endDate;
			currentPlan.type = (byte) planType;

            theDb.UpdateMarketPlan( currentPlan, 0 );

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
        ////        ConfirmDialog cdlg = new ConfirmDialog( illegalCharsMessage, illegalCharsMessage2, illegalCharsMessage3, illegalCharsTitle );
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

        ////    theDb.UpdateMarketPlan( currentPlan, newPlanShiftDays );     // this creates a copy of the component and its data

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
           // string compFilter = "selected = true";
           // string availCompFilter = "selected = false";
            string scenFilter = "selected = true";
            string availScenFilter = "selected = false";

            this.plansView.RowFilter = scenFilter;
            this.availPlansView.RowFilter = availScenFilter;

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
            // current users (top-level plans)
            if( this.scenarioDataGridView.RowCount == 1 ) {
                compsLabel.Text = String.Format( stats1MsgS, this.scenarioDataGridView.RowCount );
            }
            else {
                compsLabel.Text = String.Format( stats1Msg, this.scenarioDataGridView.RowCount );
            }

            // current non-users (top-level plans)
            if( this.availScenarioDataGridView.RowCount == 1 ) {
                availCompsLabel.Text = String.Format( stats2MsgS, this.availScenarioDataGridView.RowCount );
            }
            else {
                availCompsLabel.Text = String.Format( stats2Msg, this.availScenarioDataGridView.RowCount );
            }

            // adding users (top-level plans)
            int c1 = this.availScenarioDataGridView.SelectedRows.Count;
            if( c1 > 0 ) {
                addedLabel.Text = String.Format( statsAddMsg, c1 );
                addedLabel.Visible = true;
            }
            else {
                addedLabel.Visible = false;
            }

            // removing users (top-level plans)
            int c2 = this.scenarioDataGridView.SelectedRows.Count;
            if( c2 > 0 ) {
                removedLabel.Text = String.Format( statsRemMsg, c2 );
                removedLabel.Visible = true;
            }
            else {
                removedLabel.Visible = false;
            }
        }

        private String GetProductName( int productID ) {
            return (string)brands[ productID ];
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}
