using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
//using Common.Utilities;
using MarketSimUtilities.MsTree;
using Common.Dialogs;

using ExcelInterface;
using ErrorInterface;

using MarketSimUtilities;
using MrktSimClient.Controls.MarketPlans;

namespace MrktSimClient.Controls
{
    /// <summary>
    /// The containing control for the Market Plan editor UI.
    /// </summary>
    public class MarketPlanControl2 : MrktSimControl
    {
        private Splitter splitter1;
        private Panel scenarioPanel;
        private Splitter splitter2;
        private Panel planPanel;
        private Splitter splitter3;
        private Panel componentPanel;
        private Splitter splitter4;
        private Panel dataPanel;
        private MarketPlanPicker marketPlanPicker;
        private MarketPlanScenarioPicker scenarioPicker;
        private MarketPlanComponentPicker marketPlanComponentPicker;
        private Panel filtersPanel;
        private Splitter splitter5;
        private Panel dateFilterPanel;
        private Panel channelFilterPanel;
        private MarketPlanDatePicker datePicker;
        private Panel brandFilterPanel;
        private Splitter splitter6;
        private MarketPlanChannelPicker channelPicker;
        private Panel productTreeContainerPanel;
        private Label label1;
        private ProductTree productTree;
        private MarketPlanDataControl dataControl;

        private MarketPlanControlFilter filter;
        private Panel popupMenuPanel;

        private ModelDb.PlanType selectedPlanType = ModelDb.PlanType.MarketPlan;

        public MarketPlanControl2() {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new Market Plan editor control.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="planType"></param>
        public MarketPlanControl2( MrktSimDb.ModelDb db, ModelDb.PlanType planType )
            : base( db )
		{
			// This call is required by the Windows.Forms Form Designer.
 			InitializeComponent();
            Init( db );
        }

        public void Init( MrktSimDb.ModelDb db ){
            filter = new MarketPlanControlFilter();

            scenarioPicker.Filter = filter;
            scenarioPicker.Db = db;
            scenarioPicker.PopupMenuPanel = this.popupMenuPanel;

            marketPlanPicker.Filter = filter;
            marketPlanPicker.Db = db;
            marketPlanPicker.PopupMenuPanel = this.popupMenuPanel;

            marketPlanComponentPicker.Filter = filter;
            marketPlanComponentPicker.Db = db;
            marketPlanComponentPicker.PopupMenuPanel = this.popupMenuPanel;

            dataControl.Filter = filter;
            dataControl.Db = db;
            dataControl.PopupMenuPanel = this.popupMenuPanel;

            channelPicker.Db = db;
            datePicker.Db = db;
            productTree.Db = db;

            // preselect nothing
            productTree.UnSelectAll();

            scenarioPicker.SelectedScenarioRowChanged += new MarketPlanScenarioPicker.FireSelectedScenarioRowChanged( scenarioPicker_SelectedScenarioRowChanged );
            marketPlanPicker.SelectedRowChanged += new MarketPlanPicker.FireSelectedRowChanged( marketPlanPicker_SelectedRowChanged );
            marketPlanComponentPicker.SelectedRowChanged += new MarketPlanComponentPicker.FireSelectedRowChanged( marketPlanComponentPicker_SelectedRowsChanged );
            productTree.SelectedItemsChanged += new ProductTree.SelectedItems( productTree_SelectedItemsChanged );
            channelPicker.SelectedRowChanged += new MarketPlanChannelPicker.FireSelectedRowChanged( channelPicker_SelectedRowChanged );
            datePicker.DateViewRangeChanged += new MarketPlanDatePicker.FireDateRangeChanged( datePicker_DateViewRangeChanged );

            popupMenuPanel.Paint += new PaintEventHandler( popupMenuPanel_Paint );

            //set the view to be appropriate for the initial scenario
            scenarioPicker.SelectItem( -1 );     // doesn't trigger a callback
            scenarioPicker.SelectItem( 0 );      // triggers a callback
        }

        #region Filter Settings Handlers
        /// <summary>
        /// Responds to a change in the setting of the channel picker.
        /// </summary>
        /// <param name="channelRow"></param>
        private void channelPicker_SelectedRowChanged( MrktSimDBSchema.channelRow channelRow ) {
            Console.WriteLine( "channelPicker_SelectedRowChanged()    Channel:" + channelRow.channel_name );

            filter.ChannelID = channelRow.channel_id;

            UpdateControls();
        }

        /// <summary>
        /// Responds to a change in the date-range control settings.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void datePicker_DateViewRangeChanged( DateTime startDate, DateTime endDate ) {
            Console.WriteLine( "datePicker_DateViewRangeChanged()    Start:" + startDate.ToShortDateString() + "  End: " +
                endDate.ToShortDateString() );

            filter.StartDate = startDate;
            filter.EndDate = endDate;

            updatePlanControl();
        }

        /// <summary>
        /// Refreshes the display in response to a change in the brand/product filter tree.
        /// </summary>
        /// <param name="prodList"></param>
        private void productTree_SelectedItemsChanged( ArrayList prodList ) {
            Console.WriteLine( "productTree_SelectedItemsChanged()" );

            filter.SetProductIDs(prodList);

            updatePlanControl();
        }

        /// <summary>
        /// Refreshes the market plan components and data panels to display the specified plan type.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="planType"></param>
        /// <remarks>This method is called when the user clicks a market plan component type in the main tree view.</remarks>
        public void SetViewFor( ModelDb.PlanType planType ) 
        {
            marketPlanComponentPicker.DefaultPlanType = planType;

            this.updateComponentControl();
        }
        #endregion

        /// <summary>
        /// Handle a change to the scenario selection by triggering the market plans list to reload appropriately.
        /// </summary>
        /// <param name="scenarioRow"></param>
        private void scenarioPicker_SelectedScenarioRowChanged( MrktSimDBSchema.scenarioRow scenarioRow ) 
        {
            if (scenarioRow != null)
            {
                filter.SelectedScenarioID = scenarioRow.scenario_id;
            }
            else
            {
                filter.SelectedScenarioID = Database.AllID;
            }

            updatePlanControl();
        }

        /// <summary>
        /// Handles a change in the selected item in the market plan picker.
        /// </summary>
        /// <param name="planRow"></param>
        private void marketPlanPicker_SelectedRowChanged() 
        {
            this.updateComponentControl();
        }

        /// <summary>
        /// Handles a change in the selection state of the market plans component list.
        /// </summary>
        /// <param name="componentRows"></param>
        private void marketPlanComponentPicker_SelectedRowsChanged()
        {
            UpdateControls();
        }

        /// <summary>
        /// update plans in picker if date or products change
        /// </summary>
        private void updatePlanControl()
        {
            Console.WriteLine( ">>> 1a" );
            marketPlanPicker.UpdatePlanTable();

            Console.WriteLine( ">>> 1b" );
            updateComponentControl();
            Console.WriteLine( ">>> 1x" );
        }

        // update components if date or plans change
        private void updateComponentControl()
        {
            Console.WriteLine( ">>> 2a" );
            marketPlanComponentPicker.UpdatePlanTable();
            Console.WriteLine( ">>> 2b" );

           UpdateControls();
           Console.WriteLine( ">>> 2x" );
       }

        /// <summary>
        /// Updates the data control to reflect the current state of the filter and selection controls.
        /// </summary>
        /// <remarks>The components cannot be preserved unless the plans are also preserved.</remarks>
        private void UpdateControls() 
        {
            Console.WriteLine( ">>> 3a" );
            dataControl.DataSettingsChanged();
            Console.WriteLine( ">>> 3x" );
        }

 
       #region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            this.filtersPanel = new System.Windows.Forms.Panel();
            this.brandFilterPanel = new System.Windows.Forms.Panel();
            this.productTreeContainerPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.productTree = new MarketSimUtilities.ProductTree();
            this.splitter6 = new System.Windows.Forms.Splitter();
            this.channelFilterPanel = new System.Windows.Forms.Panel();
            this.channelPicker = new MrktSimClient.Controls.MarketPlans.MarketPlanChannelPicker();
            this.splitter5 = new System.Windows.Forms.Splitter();
            this.dateFilterPanel = new System.Windows.Forms.Panel();
            this.datePicker = new MrktSimClient.Controls.MarketPlans.MarketPlanDatePicker();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.scenarioPanel = new System.Windows.Forms.Panel();
            this.scenarioPicker = new MrktSimClient.Controls.MarketPlans.MarketPlanScenarioPicker();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.planPanel = new System.Windows.Forms.Panel();
            this.marketPlanPicker = new MrktSimClient.Controls.MarketPlans.MarketPlanPicker();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.componentPanel = new System.Windows.Forms.Panel();
            this.marketPlanComponentPicker = new MrktSimClient.Controls.MarketPlans.MarketPlanComponentPicker();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.dataControl = new MrktSimClient.Controls.MarketPlans.MarketPlanDataControl();
            this.popupMenuPanel = new System.Windows.Forms.Panel();
            this.filtersPanel.SuspendLayout();
            this.brandFilterPanel.SuspendLayout();
            this.productTreeContainerPanel.SuspendLayout();
            this.channelFilterPanel.SuspendLayout();
            this.dateFilterPanel.SuspendLayout();
            this.scenarioPanel.SuspendLayout();
            this.planPanel.SuspendLayout();
            this.componentPanel.SuspendLayout();
            this.dataPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // filtersPanel
            // 
            this.filtersPanel.BackColor = System.Drawing.SystemColors.Control;
            this.filtersPanel.Controls.Add( this.brandFilterPanel );
            this.filtersPanel.Controls.Add( this.splitter6 );
            this.filtersPanel.Controls.Add( this.channelFilterPanel );
            this.filtersPanel.Controls.Add( this.splitter5 );
            this.filtersPanel.Controls.Add( this.dateFilterPanel );
            this.filtersPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.filtersPanel.Location = new System.Drawing.Point( 625, 0 );
            this.filtersPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Size = new System.Drawing.Size( 162, 535 );
            this.filtersPanel.TabIndex = 0;
            // 
            // brandFilterPanel
            // 
            this.brandFilterPanel.BackColor = System.Drawing.Color.ForestGreen;
            this.brandFilterPanel.Controls.Add( this.productTreeContainerPanel );
            this.brandFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandFilterPanel.Location = new System.Drawing.Point( 0, 71 );
            this.brandFilterPanel.Name = "brandFilterPanel";
            this.brandFilterPanel.Size = new System.Drawing.Size( 162, 464 );
            this.brandFilterPanel.TabIndex = 4;
            // 
            // productTreeContainerPanel
            // 
            this.productTreeContainerPanel.BackColor = System.Drawing.Color.White;
            this.productTreeContainerPanel.Controls.Add( this.label1 );
            this.productTreeContainerPanel.Controls.Add( this.productTree );
            this.productTreeContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productTreeContainerPanel.Location = new System.Drawing.Point( 0, 0 );
            this.productTreeContainerPanel.Name = "productTreeContainerPanel";
            this.productTreeContainerPanel.Size = new System.Drawing.Size( 162, 464 );
            this.productTreeContainerPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 2, 1 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 89, 15 );
            this.label1.TabIndex = 4;
            this.label1.Text = "Brand/Product";
            // 
            // productTree
            // 
            this.productTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.productTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productTree.CheckBoxes = true;
            this.productTree.Location = new System.Drawing.Point( 1, 21 );
            this.productTree.Name = "productTree";
            this.productTree.Size = new System.Drawing.Size( 158, 443 );
            this.productTree.TabIndex = 3;
            // 
            // splitter6
            // 
            this.splitter6.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter6.Location = new System.Drawing.Point( 0, 68 );
            this.splitter6.Name = "splitter6";
            this.splitter6.Size = new System.Drawing.Size( 162, 3 );
            this.splitter6.TabIndex = 3;
            this.splitter6.TabStop = false;
            // 
            // channelFilterPanel
            // 
            this.channelFilterPanel.BackColor = System.Drawing.Color.GreenYellow;
            this.channelFilterPanel.Controls.Add( this.channelPicker );
            this.channelFilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.channelFilterPanel.Location = new System.Drawing.Point( 0, 40 );
            this.channelFilterPanel.Name = "channelFilterPanel";
            this.channelFilterPanel.Size = new System.Drawing.Size( 162, 28 );
            this.channelFilterPanel.TabIndex = 2;
            // 
            // channelPicker
            // 
            this.channelPicker.BackColor = System.Drawing.Color.White;
            this.channelPicker.ChannelID = -1;
            this.channelPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelPicker.Location = new System.Drawing.Point( 0, 0 );
            this.channelPicker.Name = "channelPicker";
            this.channelPicker.Size = new System.Drawing.Size( 162, 28 );
            this.channelPicker.TabIndex = 0;
            // 
            // splitter5
            // 
            this.splitter5.BackColor = System.Drawing.SystemColors.Control;
            this.splitter5.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter5.Location = new System.Drawing.Point( 0, 37 );
            this.splitter5.Name = "splitter5";
            this.splitter5.Size = new System.Drawing.Size( 162, 3 );
            this.splitter5.TabIndex = 1;
            this.splitter5.TabStop = false;
            // 
            // dateFilterPanel
            // 
            this.dateFilterPanel.BackColor = System.Drawing.Color.ForestGreen;
            this.dateFilterPanel.Controls.Add( this.datePicker );
            this.dateFilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateFilterPanel.Location = new System.Drawing.Point( 0, 0 );
            this.dateFilterPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.dateFilterPanel.Name = "dateFilterPanel";
            this.dateFilterPanel.Size = new System.Drawing.Size( 162, 37 );
            this.dateFilterPanel.TabIndex = 0;
            // 
            // datePicker
            // 
            this.datePicker.BackColor = System.Drawing.Color.White;
            this.datePicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePicker.Location = new System.Drawing.Point( 0, 0 );
            this.datePicker.Margin = new System.Windows.Forms.Padding( 0 );
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size( 162, 37 );
            this.datePicker.Suspend = false;
            this.datePicker.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point( 622, 0 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 3, 535 );
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // scenarioPanel
            // 
            this.scenarioPanel.BackColor = System.Drawing.Color.PeachPuff;
            this.scenarioPanel.Controls.Add( this.scenarioPicker );
            this.scenarioPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.scenarioPanel.Location = new System.Drawing.Point( 0, 0 );
            this.scenarioPanel.Name = "scenarioPanel";
            this.scenarioPanel.Size = new System.Drawing.Size( 622, 34 );
            this.scenarioPanel.TabIndex = 2;
            // 
            // scenarioPicker
            // 
            this.scenarioPicker.BackColor = System.Drawing.Color.WhiteSmoke;
            this.scenarioPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioPicker.Location = new System.Drawing.Point( 0, 0 );
            this.scenarioPicker.Margin = new System.Windows.Forms.Padding( 0 );
            this.scenarioPicker.Name = "scenarioPicker";
            this.scenarioPicker.Size = new System.Drawing.Size( 622, 34 );
            this.scenarioPicker.Suspend = false;
            this.scenarioPicker.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point( 0, 34 );
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size( 622, 3 );
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // planPanel
            // 
            this.planPanel.BackColor = System.Drawing.Color.LightCyan;
            this.planPanel.Controls.Add( this.marketPlanPicker );
            this.planPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.planPanel.Location = new System.Drawing.Point( 0, 37 );
            this.planPanel.Name = "planPanel";
            this.planPanel.Size = new System.Drawing.Size( 622, 120 );
            this.planPanel.TabIndex = 4;
            // 
            // marketPlanPicker
            // 
            this.marketPlanPicker.BackColor = System.Drawing.Color.MintCream;
            this.marketPlanPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketPlanPicker.Location = new System.Drawing.Point( 0, 0 );
            this.marketPlanPicker.Margin = new System.Windows.Forms.Padding( 0 );
            this.marketPlanPicker.Name = "marketPlanPicker";
            this.marketPlanPicker.Size = new System.Drawing.Size( 622, 120 );
            this.marketPlanPicker.Suspend = false;
            this.marketPlanPicker.TabIndex = 0;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point( 0, 157 );
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size( 622, 3 );
            this.splitter3.TabIndex = 5;
            this.splitter3.TabStop = false;
            // 
            // componentPanel
            // 
            this.componentPanel.BackColor = System.Drawing.Color.Thistle;
            this.componentPanel.Controls.Add( this.marketPlanComponentPicker );
            this.componentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.componentPanel.Location = new System.Drawing.Point( 0, 160 );
            this.componentPanel.Name = "componentPanel";
            this.componentPanel.Size = new System.Drawing.Size( 622, 150 );
            this.componentPanel.TabIndex = 6;
            // 
            // marketPlanComponentPicker
            // 
            this.marketPlanComponentPicker.BackColor = System.Drawing.Color.LightCyan;
            this.marketPlanComponentPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketPlanComponentPicker.Location = new System.Drawing.Point( 0, 0 );
            this.marketPlanComponentPicker.Margin = new System.Windows.Forms.Padding( 0 );
            this.marketPlanComponentPicker.Name = "marketPlanComponentPicker";
            this.marketPlanComponentPicker.Size = new System.Drawing.Size( 622, 150 );
            this.marketPlanComponentPicker.Suspend = false;
            this.marketPlanComponentPicker.TabIndex = 0;
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter4.Location = new System.Drawing.Point( 0, 310 );
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size( 622, 3 );
            this.splitter4.TabIndex = 7;
            this.splitter4.TabStop = false;
            // 
            // dataPanel
            // 
            this.dataPanel.BackColor = System.Drawing.Color.MintCream;
            this.dataPanel.Controls.Add( this.dataControl );
            this.dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPanel.Location = new System.Drawing.Point( 0, 313 );
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size( 622, 222 );
            this.dataPanel.TabIndex = 8;
            // 
            // dataControl
            // 
            this.dataControl.BackColor = System.Drawing.Color.MintCream;
            this.dataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataControl.Location = new System.Drawing.Point( 0, 0 );
            this.dataControl.Margin = new System.Windows.Forms.Padding( 0 );
            this.dataControl.Name = "dataControl";
            this.dataControl.Size = new System.Drawing.Size( 622, 222 );
            this.dataControl.Suspend = false;
            this.dataControl.TabIndex = 0;
            // 
            // popupMenuPanel
            // 
            this.popupMenuPanel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))) );
            this.popupMenuPanel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.popupMenuPanel.Location = new System.Drawing.Point( 100, 100 );
            this.popupMenuPanel.Name = "popupMenuPanel";
            this.popupMenuPanel.Size = new System.Drawing.Size( 100, 100 );
            this.popupMenuPanel.TabIndex = 9;
            this.popupMenuPanel.Visible = false;
            // 
            // MarketPlanControl2
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add( this.popupMenuPanel );
            this.Controls.Add( this.dataPanel );
            this.Controls.Add( this.splitter4 );
            this.Controls.Add( this.componentPanel );
            this.Controls.Add( this.splitter3 );
            this.Controls.Add( this.planPanel );
            this.Controls.Add( this.splitter2 );
            this.Controls.Add( this.scenarioPanel );
            this.Controls.Add( this.splitter1 );
            this.Controls.Add( this.filtersPanel );
            this.Name = "MarketPlanControl2";
            this.Size = new System.Drawing.Size( 787, 535 );
            this.Load += new System.EventHandler( this.MarketPlanControl2_Load );
            this.filtersPanel.ResumeLayout( false );
            this.brandFilterPanel.ResumeLayout( false );
            this.productTreeContainerPanel.ResumeLayout( false );
            this.productTreeContainerPanel.PerformLayout();
            this.channelFilterPanel.ResumeLayout( false );
            this.dateFilterPanel.ResumeLayout( false );
            this.scenarioPanel.ResumeLayout( false );
            this.planPanel.ResumeLayout( false );
            this.componentPanel.ResumeLayout( false );
            this.dataPanel.ResumeLayout( false );
            this.ResumeLayout( false );

        }
       #endregion

        /// <summary>
        /// One-time initialization of new form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketPlanControl2_Load( object sender, EventArgs e ) {
            if( marketPlanPicker.ItemCount > 0 ) {
                marketPlanPicker.SelectedIndex = 0;
            }
        }

        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Control c = sender as Control;
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
//            PaintMenuPanel( this.popupMenuPanel, e.Graphics );
        }

        //private void PaintMenuPanel( Panel menuPanel, Graphics g ) {
        //    int w = menuPanel.Width;
        //    int h = menuPanel.Height;
        //    Point[] points = new Point[ 4 ];
        //    points[ 0 ] = new Point( 0, 0 );
        //    points[ 1 ] = new Point( 0, h - 1 );
        //    points[ 2 ] = new Point( w - 1, h - 1 );
        //    points[ 3 ] = new Point( w - 1, 0 );
        //    points[ 4 ] = new Point( w - 1, 0 );
        //    g.DrawLines( SystemPens.ControlDarkDark, points );
        //}
    }
}

