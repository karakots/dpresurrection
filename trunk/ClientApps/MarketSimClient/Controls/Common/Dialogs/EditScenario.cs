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
	/// The diaog for creating or editing market plans or market plan components.
	/// </summary>
	public class EditScenario : System.Windows.Forms.Form
	{
        private string createNewScenarioTitle = "Create New Scenario";
        private string noNameMessage = "You must enter a name for the Scenario.";
        private string illegalCharsMessage = "The Scenario name contains illegal characters.";
        private string illegalCharsMessage2 = "Illegal characters are:";
        private string illegalCharsMessage3 = " . , \' \" ;";

        private string helpTag = "CreateScenario";

		private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;

        private MrktSimDBSchema.scenarioRow currentScenario;

        private DataTable itemTable;
        private int[] productIDs;

        private ArrayList hiddenAddedItems;
        private ArrayList hiddenRemoveditems;

        private DataView itemsView;
        private DataView availableItemsView;
        private Panel panel1;
        private Label titleLabel;
        private SplitContainer splitContainer1;
        private Label label3;
        private Label label1;
        private TextBox descrBox;
        private TextBox nameBox;
        private GroupBox groupBox1;
        private Label label2;
        private RadioButton externalFactorsRadioButton;
        private RadioButton plansRadioButton;
        private CheckBox showAllCheckBox;
        private Label addedLabel;
        private Label notSelectedLabel;
        private Label removedLabel;
        private Label selectedLabel;
        private SplitContainer itemsSplitContainer;
        private DataGridView usedDataGridView;
        private DataGridView availDataGridView;
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
        private Hashtable brands;
        private Button helpButton;

        /// <summary>
        /// Returns the list of IDs of items selected in the "available" lists (both visible and hidden).
        /// </summary>
        public ArrayList AddedUsedIDs {
            get {
                ArrayList addedIDs = new ArrayList();
                foreach( DataGridViewRow drow in availDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "AvailIdCol" ].Value;
                    addedIDs.Add( id );
                }
                foreach( int hiddenID in hiddenAddedItems ) {
                    addedIDs.Add( hiddenID );
                }
                return addedIDs;
            }
        }

        /// <summary>
        /// Returns the list of IDs of items selected in the "used" lists (both visible and hidden).
        /// </summary>
        public ArrayList RemovedUsedIDs {
            get {
                ArrayList removedIDs = new ArrayList();
                foreach( DataGridViewRow drow in usedDataGridView.SelectedRows ) {
                    int id = (int)drow.Cells[ "IdCol" ].Value;
                    removedIDs.Add( id );
                }
                foreach( int hiddenID in hiddenRemoveditems ) {
                    removedIDs.Add( hiddenID );
                }
                return removedIDs;
            }
        }

        public string ObjName {
            get { return nameBox.Text.Trim(); }
        }

        public string ObjDescription {
            get { return descrBox.Text.Trim(); }
        }

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Sets the current scenario value. 
        /// </summary>
        /// <remarks> If a value is set to this property, the dialog performs an edit operation.  Otherwise a new scenario is created.</remarks>
        public MrktSimDBSchema.scenarioRow CurrentScenario {
            set {
                currentScenario = value;

                if( currentScenario != null ) {
                    helpTag = "EditScenario";
                    nameBox.Text = value.name;
                    descrBox.Text = value.descr;
                }
                else {
                    // we are creating a new scenario
                    nameBox.Text = "";
                    descrBox.Text = "";
                    titleLabel.Text = createNewScenarioTitle;
                }
            }
        }

        /// <summary>
        /// Creates a new dialog for making a new or edited market plan or plan component.
        /// </summary>
        /// <param name="db"></param>
        public EditScenario( int[] productIDs, ModelDb theDb, bool alwaysShowAllProducts ) : this( productIDs, theDb ) {
            if( alwaysShowAllProducts ) {
                this.showAllCheckBox.Checked = true;
                this.showAllCheckBox.Enabled = false;
            }
        }
            
        /// <summary>
        /// Creates a new dialog for making a new or edited market plan or plan component.
        /// </summary>
        /// <param name="db"></param>
        public EditScenario( int[] productIDs, ModelDb theDb )
		{
//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            hiddenAddedItems = new ArrayList();
            hiddenRemoveditems = new ArrayList();

            brands = new Hashtable();
            foreach( MrktSimDBSchema.productRow prow in theDb.Data.product ) {
                brands.Add( prow.product_id, prow.product_name );
            }

            this.productIDs = productIDs;
            if( productIDs != null ) {
                // disable "all products" checkbox if we already are showing all
                foreach( int pid in productIDs ) {
                    if( pid == ModelDb.AllID ) {
                        this.showAllCheckBox.Checked = true;
                        this.showAllCheckBox.Enabled = false;
                    }
                }
            }

            itemTable = new DataTable( "ScenarioItems" );

            DataColumn idCol = itemTable.Columns.Add( "id", typeof( int ) );
            itemTable.PrimaryKey = new DataColumn[] { idCol };

            itemTable.Columns.Add( "name", typeof( string ) );
            itemTable.Columns.Add( "product", typeof( string ) );
            itemTable.Columns.Add( "type", typeof( string ) );
            itemTable.Columns.Add( "selected", typeof( bool ) );
            itemTable.Columns.Add( "product_id", typeof( int ) );

            itemsView.Table = itemTable;
            availableItemsView.Table = itemTable;

            itemsView.Sort = "name";
            availableItemsView.Sort = "name";

            SetViewFilters();

            usedDataGridView.DataSource = itemsView;
            availDataGridView.DataSource = availableItemsView;
        }

        /// <summary>
        /// Sets up the users and uses lists in the dialog according to the given lists of MarketPlanControlRelater.Item objects
        /// </summary>
        /// <param name="users"></param>
        /// <param name="used"></param>
        public void SetUsedList( ArrayList used ) {
            // the used items list
            for( int i = 0; i < used.Count; i++ ) {
                MarketPlanControlRelater.Item item = (MarketPlanControlRelater.Item)used[ i ];
                AddUsedItem( item.Name, item.ID, item.ItemType, item.Selected, item.ProductID );
            }

            SetViewFilters();
        }

        /// <summary>
        /// Adds an item to the "used" list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        private void AddUsedItem( string name, int id, ModelDb.PlanType type, bool selected, int productID ) {
            DataRow drow = itemTable.NewRow();

            drow[ "id" ] = id;
            drow[ "name" ] = name;
            drow[ "product_id" ] = productID;
            drow[ "product" ] = ProductNameForID( productID );
            drow[ "selected" ] = selected;

            drow[ "type" ] = (int)type;
            if( type == ModelDb.PlanType.MarketPlan ) {
                drow[ "type" ] = "Market Plan";
            }
            else if( type == ModelDb.PlanType.ProdEvent ) {
                drow[ "type" ] = "External Factors";
            }

            itemTable.Rows.Add( drow );

            ////string nn = name;      // debug
            ////if( nn.Length > 25 ) {
            ////    nn = nn.Substring( 0, 25 );
            ////}
            ////Console.WriteLine( "s:{0}, p:{1}, {2}, {3}, {4}", selected, productID, type, id, nn );
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
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.itemsView = new System.Data.DataView();
            this.availableItemsView = new System.Data.DataView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.externalFactorsRadioButton = new System.Windows.Forms.RadioButton();
            this.plansRadioButton = new System.Windows.Forms.RadioButton();
            this.showAllCheckBox = new System.Windows.Forms.CheckBox();
            this.addedLabel = new System.Windows.Forms.Label();
            this.notSelectedLabel = new System.Windows.Forms.Label();
            this.removedLabel = new System.Windows.Forms.Label();
            this.selectedLabel = new System.Windows.Forms.Label();
            this.itemsSplitContainer = new System.Windows.Forms.SplitContainer();
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
            ((System.ComponentModel.ISupportInitialize)(this.itemsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableItemsView)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.itemsSplitContainer.Panel1.SuspendLayout();
            this.itemsSplitContainer.Panel2.SuspendLayout();
            this.itemsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usedDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.acceptButton.BackColor = System.Drawing.SystemColors.Control;
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.acceptButton.Location = new System.Drawing.Point( 204, 400 );
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
            this.cancelButton.Location = new System.Drawing.Point( 297, 400 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.panel1.Controls.Add( this.helpButton );
            this.panel1.Controls.Add( this.titleLabel );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 569, 26 );
            this.panel1.TabIndex = 57;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.Control;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 538, 2 );
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
            this.titleLabel.Font = new System.Drawing.Font( "Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point( 5, 5 );
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 93, 16 );
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Edit Scenario";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Location = new System.Drawing.Point( 7, 26 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.label3 );
            this.splitContainer1.Panel1.Controls.Add( this.label1 );
            this.splitContainer1.Panel1.Controls.Add( this.descrBox );
            this.splitContainer1.Panel1.Controls.Add( this.nameBox );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.groupBox1 );
            this.splitContainer1.Size = new System.Drawing.Size( 554, 369 );
            this.splitContainer1.SplitterDistance = 92;
            this.splitContainer1.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 9, 43 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 70, 23 );
            this.label3.TabIndex = 34;
            this.label3.Text = "Description";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 9, 15 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 61, 17 );
            this.label1.TabIndex = 33;
            this.label1.Text = "Name";
            // 
            // descrBox
            // 
            this.descrBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.descrBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.descrBox.Location = new System.Drawing.Point( 85, 41 );
            this.descrBox.MaxLength = 100;
            this.descrBox.Multiline = true;
            this.descrBox.Name = "descrBox";
            this.descrBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descrBox.Size = new System.Drawing.Size( 445, 42 );
            this.descrBox.TabIndex = 32;
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nameBox.Location = new System.Drawing.Point( 85, 12 );
            this.nameBox.MaxLength = 100;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size( 445, 21 );
            this.nameBox.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.externalFactorsRadioButton );
            this.groupBox1.Controls.Add( this.plansRadioButton );
            this.groupBox1.Controls.Add( this.showAllCheckBox );
            this.groupBox1.Controls.Add( this.addedLabel );
            this.groupBox1.Controls.Add( this.notSelectedLabel );
            this.groupBox1.Controls.Add( this.removedLabel );
            this.groupBox1.Controls.Add( this.selectedLabel );
            this.groupBox1.Controls.Add( this.itemsSplitContainer );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 554, 273 );
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Components";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point( 15, 221 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 109, 45 );
            this.label2.TabIndex = 65;
            this.label2.Text = "(use Control-click to multi-select or de-select items)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // externalFactorsRadioButton
            // 
            this.externalFactorsRadioButton.AutoSize = true;
            this.externalFactorsRadioButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.externalFactorsRadioButton.Location = new System.Drawing.Point( 13, 42 );
            this.externalFactorsRadioButton.Name = "externalFactorsRadioButton";
            this.externalFactorsRadioButton.Size = new System.Drawing.Size( 113, 18 );
            this.externalFactorsRadioButton.TabIndex = 64;
            this.externalFactorsRadioButton.Text = "External Factors";
            this.externalFactorsRadioButton.UseVisualStyleBackColor = true;
            // 
            // plansRadioButton
            // 
            this.plansRadioButton.AutoSize = true;
            this.plansRadioButton.Checked = true;
            this.plansRadioButton.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.plansRadioButton.Location = new System.Drawing.Point( 13, 24 );
            this.plansRadioButton.Name = "plansRadioButton";
            this.plansRadioButton.Size = new System.Drawing.Size( 97, 18 );
            this.plansRadioButton.TabIndex = 63;
            this.plansRadioButton.TabStop = true;
            this.plansRadioButton.Text = "Market Plans";
            this.plansRadioButton.UseVisualStyleBackColor = true;
            this.plansRadioButton.CheckedChanged += new System.EventHandler( this.plansRadioButton_CheckedChanged );
            // 
            // showAllCheckBox
            // 
            this.showAllCheckBox.AutoSize = true;
            this.showAllCheckBox.Font = new System.Drawing.Font( "Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.showAllCheckBox.Location = new System.Drawing.Point( 29, 70 );
            this.showAllCheckBox.Name = "showAllCheckBox";
            this.showAllCheckBox.Size = new System.Drawing.Size( 92, 18 );
            this.showAllCheckBox.TabIndex = 62;
            this.showAllCheckBox.Text = "ALL Products";
            this.showAllCheckBox.UseVisualStyleBackColor = true;
            this.showAllCheckBox.CheckedChanged += new System.EventHandler( this.showAllCheckBox_CheckedChanged );
            // 
            // addedLabel
            // 
            this.addedLabel.AutoSize = true;
            this.addedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.addedLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.addedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.addedLabel.Location = new System.Drawing.Point( 15, 170 );
            this.addedLabel.Name = "addedLabel";
            this.addedLabel.Size = new System.Drawing.Size( 85, 14 );
            this.addedLabel.TabIndex = 60;
            this.addedLabel.Text = "188 to be added";
            // 
            // notSelectedLabel
            // 
            this.notSelectedLabel.BackColor = System.Drawing.SystemColors.Control;
            this.notSelectedLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.notSelectedLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.notSelectedLabel.Location = new System.Drawing.Point( 15, 130 );
            this.notSelectedLabel.Name = "notSelectedLabel";
            this.notSelectedLabel.Size = new System.Drawing.Size( 120, 18 );
            this.notSelectedLabel.TabIndex = 59;
            this.notSelectedLabel.Text = "188 available";
            // 
            // removedLabel
            // 
            this.removedLabel.AutoSize = true;
            this.removedLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.removedLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.removedLabel.ForeColor = System.Drawing.Color.Yellow;
            this.removedLabel.Location = new System.Drawing.Point( 15, 154 );
            this.removedLabel.Name = "removedLabel";
            this.removedLabel.Size = new System.Drawing.Size( 97, 14 );
            this.removedLabel.TabIndex = 61;
            this.removedLabel.Text = "188 to be removed";
            // 
            // selectedLabel
            // 
            this.selectedLabel.BackColor = System.Drawing.SystemColors.Control;
            this.selectedLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.selectedLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectedLabel.Location = new System.Drawing.Point( 15, 114 );
            this.selectedLabel.Name = "selectedLabel";
            this.selectedLabel.Size = new System.Drawing.Size( 119, 15 );
            this.selectedLabel.TabIndex = 58;
            this.selectedLabel.Text = "188 Included";
            // 
            // itemsSplitContainer
            // 
            this.itemsSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsSplitContainer.Location = new System.Drawing.Point( 137, 16 );
            this.itemsSplitContainer.Name = "itemsSplitContainer";
            this.itemsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // itemsSplitContainer.Panel1
            // 
            this.itemsSplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(90)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))) );
            this.itemsSplitContainer.Panel1.Controls.Add( this.usedDataGridView );
            this.itemsSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding( 4 );
            // 
            // itemsSplitContainer.Panel2
            // 
            this.itemsSplitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(170)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))) );
            this.itemsSplitContainer.Panel2.Controls.Add( this.availDataGridView );
            this.itemsSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding( 4 );
            this.itemsSplitContainer.Size = new System.Drawing.Size( 398, 247 );
            this.itemsSplitContainer.SplitterDistance = 121;
            this.itemsSplitContainer.TabIndex = 57;
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
            this.usedDataGridView.Size = new System.Drawing.Size( 390, 113 );
            this.usedDataGridView.TabIndex = 55;
            this.usedDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // NameCol
            // 
            this.NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameCol.DataPropertyName = "name";
            this.NameCol.HeaderText = "INCLUDED";
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            this.NameCol.Width = 200;
            // 
            // ProdCol
            // 
            this.ProdCol.DataPropertyName = "product";
            this.ProdCol.HeaderText = "Product";
            this.ProdCol.Name = "ProdCol";
            this.ProdCol.ReadOnly = true;
            this.ProdCol.Width = 75;
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
            this.availDataGridView.Size = new System.Drawing.Size( 390, 114 );
            this.availDataGridView.TabIndex = 56;
            this.availDataGridView.SelectionChanged += new System.EventHandler( this.SelectedIndexChanged );
            // 
            // AvailNameCol
            // 
            this.AvailNameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AvailNameCol.DataPropertyName = "name";
            this.AvailNameCol.HeaderText = "AVAILABLE";
            this.AvailNameCol.Name = "AvailNameCol";
            this.AvailNameCol.ReadOnly = true;
            this.AvailNameCol.Width = 200;
            // 
            // AvProdCol
            // 
            this.AvProdCol.DataPropertyName = "product";
            this.AvProdCol.HeaderText = "Product";
            this.AvProdCol.Name = "AvProdCol";
            this.AvProdCol.ReadOnly = true;
            this.AvProdCol.Width = 75;
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
            // EditScenario
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 569, 429 );
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.acceptButton );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EditScenario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler( this.EditScenario_Load );
            ((System.ComponentModel.ISupportInitialize)(this.itemsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableItemsView)).EndInit();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.itemsSplitContainer.Panel1.ResumeLayout( false );
            this.itemsSplitContainer.Panel2.ResumeLayout( false );
            this.itemsSplitContainer.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.usedDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availDataGridView)).EndInit();
            this.ResumeLayout( false );

		}
		#endregion

        private void SelectedIndexChanged( object sender, EventArgs e ) {
            UpdateLabels();
        }

        /// <summary>
        /// Updates the labels describing the selections
        /// </summary>
        private void UpdateLabels() {
            int selected = usedDataGridView.RowCount;
            int avail = availDataGridView.RowCount;

            int addUsedCount = availDataGridView.SelectedRows.Count;
            int delUsedCount = usedDataGridView.SelectedRows.Count;

            selectedLabel.Text = String.Format( "{0} included", selected );
            notSelectedLabel.Text = String.Format( "{0} available", avail );

            if( addUsedCount > 0 ) {
                addedLabel.Text = String.Format( "{0} to be added", addUsedCount );
                addedLabel.Visible = true;
            }
            else {
                addedLabel.Visible = false;
            }

            if( delUsedCount > 0 ) {
               removedLabel.Text = String.Format( "{0} to be removed", delUsedCount );
               removedLabel.Visible = true;
            }
            else {
                removedLabel.Visible = false;
            }
        }

        private void acceptButton_Click( object sender, EventArgs e ) {
            // be sure that a name is entered
            if( this.nameBox.Text.Trim().Length == 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( noNameMessage, "Name Required" );
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            // validate the name
            char[] illegal = { ',', '\'', '"', '.', ';' };
            int chkIndex = nameBox.Text.IndexOfAny( illegal );
            if( chkIndex >= 0 ) {
                ConfirmDialog cdlg = new ConfirmDialog( illegalCharsMessage, illegalCharsMessage2, illegalCharsMessage3, "Name Invalid" );
                cdlg.SetOkButtonOnlyStyle();
                cdlg.ShowDialog();
                this.DialogResult = DialogResult.None;
                return;
            }

            if( currentScenario != null ) {
                // setting these values is actually making changes to the current row of the scenario DB table - no further return values needed
                currentScenario.name = nameBox.Text.Trim();
                currentScenario.descr = descrBox.Text.Trim();
            }
            this.DialogResult = DialogResult.OK;
        }

        private void showAllCheckBox_CheckedChanged( object sender, EventArgs e ) {
            SetViewFilters();
        }

        /// <summary>
        /// Configures the row filters for the items table to show the appropriate lists
        /// </summary>
        private void SetViewFilters() {
            string usedFilter = "selected = true";
            string availFilter = "selected = false";

            if( plansRadioButton.Checked == true ) {
                usedFilter += " AND type = 'Market Plan'";
                availFilter += " AND type = 'Market Plan'";
            }
            else {
                usedFilter += " AND type = 'External Factors'";
                availFilter += " AND type = 'External Factors'";
            }

            if( showAllCheckBox.Checked == false ) {
                usedFilter += " AND (product_id = -1";
                availFilter += " AND (product_id = -1";
                if( this.productIDs != null ) {
                    foreach( int pid in this.productIDs ) {
                        usedFilter += " OR product_id = " + pid;
                        availFilter += " OR product_id = " + pid;
                    }
                }
                usedFilter += ")";
                availFilter += ")";
            }

            Console.WriteLine( "Used item row filter = " + usedFilter );
            Console.WriteLine( "Avail item row filter = " + availFilter );
            this.availableItemsView.RowFilter = availFilter;
            this.itemsView.RowFilter = usedFilter;
            this.usedDataGridView.ClearSelection();
            this.availDataGridView.ClearSelection();
            UpdateLabels();
        }

        private void EditScenario_Load( object sender, EventArgs e ) {
            this.usedDataGridView.ClearSelection();
            this.availDataGridView.ClearSelection();
        }


        private String ProductNameForID( int productID ) {
            return (string)brands[ productID ];
        }

        private void plansRadioButton_CheckedChanged( object sender, EventArgs e ) {
            ArrayList prevAddedItems = new ArrayList();
            ArrayList prevRemovedItems = new ArrayList();

            // copy the hidden lists to temporary lists
            foreach( int p1 in this.hiddenAddedItems ) {
                prevAddedItems.Add( p1 );
            }
            foreach( int p1 in this.hiddenRemoveditems ) {
                prevRemovedItems.Add( p1 );
            }

            // save the displayed state
            this.hiddenAddedItems = this.AddedUsedIDs;
            this.hiddenRemoveditems = this.RemovedUsedIDs;

            // swap the displayed items
            SetViewFilters();

            //re-select previous selections
            foreach( DataGridViewRow urow in this.usedDataGridView.Rows ) {
                if( prevRemovedItems.Contains( (int)urow.Cells[ "IdCol" ].Value ) ) {
                    urow.Selected = true;
                }
            }
            foreach( DataGridViewRow arow in this.availDataGridView.Rows ) {
                if( prevAddedItems.Contains( (int)arow.Cells[ "AvailIdCol" ].Value ) ) {
                    arow.Selected = true;
                }
            }

        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}
