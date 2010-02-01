using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MarketSimUtilities;
using MrktSimDb;
using Utilities;
using Utilities.Graphing;

namespace MrktSimClient.Controls.MarketPlans
{
    class MarketPlanDataControl : MrktSimControl
    {

        private Button refreshButton;

        /// <summary>
        /// template grid for designer
        /// </summary>
        private MrktSimGrid mrktSimGrid;

        /// <summary>
        /// where we store data
        /// </summary>
        private MrktSimGrid[] mrktSimGrids;

        /// <summary>
        /// the grid currently displayed
        /// </summary>
        private MrktSimGrid currentGrid = null;

        private bool autoUpdate = true;

        private MarketPlanGraphControl graphControl;
        private CheckBox graphCheckBox;

        private Label infoLabel;
        private MarketPlanControlFilter filter;
        private string refreshNowString = "Refresh Now";
        private string refreshOffString = "Turn Off Auto Update";
        private Color refreshNeededBkg = Color.LightYellow;
        private Color refreshNeededFg = Color.Red;
        private Color refreshNotNeededBkg = Color.White;
        private Color refreshNotNeededFg = Color.DarkGreen;

        private string autoRefreshOntemString = "Turn On Auto Update";
        private string autoRefreshOfftemString = "Turn Off Auto Update";

        private PopupMenuLinkLabel dataLinkLabel;

        #region Propertiies
        /// <summary>
        /// Turns auto-update of the grid off or on.  If autoUpdate is true, calling DataSettingsChanged will immediately update the data.
        /// </summary>
        public bool AutoUpdate {
            get {
                return autoUpdate;
            }
        }

        /// <summary>
        /// Sets the view filter object.
        /// </summary>
        public MarketPlanControlFilter Filter {
            set {
                filter = value;
            }
        }
        #endregion

        #region Constructor and Initialization
        public override MrktSimDb.ModelDb Db {
            set {
                theDb = value;

                mrktSimGrid.Suspend = true;
                mrktSimGrid.DescriptionWindow = false;

                mrktSimGrids = new MrktSimGrid[ Enum.GetNames( typeof( ModelDb.PlanType ) ).Length ];
                for( int i = 0; i < mrktSimGrids.Length; i++ ) {
                    mrktSimGrids[ i ] = new MrktSimGrid();
                    mrktSimGrids[ i ].Bounds = mrktSimGrid.Bounds;
                    mrktSimGrids[ i ].DescriptionWindow = false;
                    mrktSimGrids[ i ].Suspend = true;
                    mrktSimGrids[i].Enabled = false;
                    mrktSimGrids[ i ].Visible = false;
                    mrktSimGrids[ i ].Anchor = mrktSimGrid.Anchor;
                    mrktSimGrids[ i ].RowID = "record_id";

                    ModelDb.PlanType type = (ModelDb.PlanType)i;         // plan types are 0, 1, 2...
                    switch( type ) {
                        case ModelDb.PlanType.MarketPlan:
                            mrktSimGrids[ i ].Table = null;
                            break;
                        case ModelDb.PlanType.Coupons:
                            mrktSimGrids[ i ].Table = theDb.Data.mass_media;
                            break;
                        case ModelDb.PlanType.Display:
                            mrktSimGrids[ i ].Table = theDb.Data.display;
                            break;
                        case ModelDb.PlanType.Distribution:
                            mrktSimGrids[ i ].Table = theDb.Data.distribution;
                            break;
                        case ModelDb.PlanType.Market_Utility:
                            mrktSimGrids[i].Table = theDb.Data.market_utility;
                            break;
                        case ModelDb.PlanType.Media:
                            mrktSimGrids[ i ].Table = theDb.Data.mass_media;
                            break;
                        case ModelDb.PlanType.Price:
                            mrktSimGrids[ i ].Table = theDb.Data.product_channel;
                            break;
                        case ModelDb.PlanType.ProdEvent:
                            mrktSimGrids[ i ].Table = theDb.Data.product_event;
                            break;
                        case ModelDb.PlanType.TaskEvent:
                            mrktSimGrids[ i ].Table = theDb.Data.task_event;
                            break;
                    }
                }

                for( int i = 0; i < mrktSimGrids.Length; i++ ) {
                    CreateTableStyle( theDb, mrktSimGrids[ i ], (ModelDb.PlanType)i );
                    this.Controls.Add( this.mrktSimGrids[ i ] );
                }
            }
        }

        /// <summary>
        /// Sets the panel that is used to display the popup menu items
        /// </summary>
        /// <remarks>The popup menu panel is typically owned by a higher-level Control so that it won't be clipped at the edge of this control</remarks>
        public Panel PopupMenuPanel {
            set {
                this.dataLinkLabel.PopupMenuPanel = value;
            }
        }

        /// <summary>
        /// Creates a new control for viewing market plan data.
        /// </summary> 
        public MarketPlanDataControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            // hide the prototype grid used by the form designer
            mrktSimGrid.Visible = false;
            mrktSimGrid.Suspend = true;
            mrktSimGrid.Enabled = false;

            this.graphControl.Send.Visible = false;

            if( this.autoUpdate == true ) {
                dataLinkLabel.AddItem( autoRefreshOfftemString, ToggleAutoRefresh );
            }
            else {
                dataLinkLabel.AddItem( autoRefreshOntemString, ToggleAutoRefresh );
            }
        }
        #endregion

        public void SuspendDataGrids( bool suspend ) {
            foreach( MrktSimGrid dg in mrktSimGrids ) {
                dg.Suspend = suspend;
            }
        }

        /// <summary>
        /// Responds to a change in the state of the "graph" checkbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graphCheckBox_CheckedChanged( object sender, EventArgs e ) {
            UpdateDataNow();
        }
        
        /// <summary>
        /// Call this method to tell the data control it can update its display.  If autoUpdate is on, the update is immediate.
        /// </summary>
        public void DataSettingsChanged() {
            if( autoUpdate ) {
                UpdateDataNow();
                refreshButton.ForeColor = refreshNotNeededFg;
                refreshButton.BackColor = refreshNotNeededBkg;
            }
            else {
                if( (filter.SelectedMarketPlanComponentIDs != null) && (filter.SelectedMarketPlanComponentIDs.Length > 0) ) {
                    infoLabel.Text = "Data Needs to be Refreshed";
                    infoLabel.Visible = true;
                    refreshButton.ForeColor = refreshNeededFg;
                    refreshButton.BackColor = refreshNeededBkg;
                }
                else {
                    infoLabel.Text = "(No Selected Components)";
                    infoLabel.Visible = true;
                    refreshButton.ForeColor = refreshNotNeededFg;
                    refreshButton.BackColor = refreshNotNeededBkg;
                }

                if( currentGrid != null ) {
                    currentGrid.Visible = false;
                }
                graphControl.Visible = false;
            }
        }

        /// <summary>
        /// Updates the data display to match the most recently requested settings for PlanType and row filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click( object sender, EventArgs e ) {
            if( autoUpdate == false ) {
                UpdateDataNow();
            }
            else {
                // turn off the auto-update
                ToggleAutoRefresh();
            }
        }

        /// <summary>
        /// Actually update the data display according to the most recently set PlanType and RowFilter values
        /// </summary>
        private void UpdateDataNow() {
            Utilities.Status status = new Utilities.Status( this );
            string updateStatusMsg = null;
            if( graphCheckBox.Checked == false ) {
                updateStatusMsg = "Updating Component Data...";
            }
            else {
                updateStatusMsg = "Updating Component Graph...";
            }
            //Console.WriteLine( "--- UpdateDataNow()" );
            status.UpdateUIAndContinue( UpdateDataNow_Part2, updateStatusMsg, 85 );
            //Console.WriteLine( "---...return" );
            updateDataNeeded = true;
        }

        private bool updateDataNeeded = false;

        private void UpdateDataNow_Part2() {

            if( updateDataNeeded == false ) {
                //Console.WriteLine( "$$$ UpdateDataNow() -- Part 2 Skipped" );
                return;  
            }

            updateDataNeeded = false;
            //Console.WriteLine( "--- UpdateDataNow() -- Part 2" );
            Utilities.Status.SetWaitCursor( this );
            if( graphCheckBox.Checked == false )
            {
                if (currentGrid != null)
                {
                    currentGrid.Suspend = true;
                    currentGrid.Visible = false;
                    currentGrid.Enabled = false;
                    currentGrid = null;
                }

                if (filter.PlanType != ModelDb.PlanType.MarketPlan)
                {
                    currentGrid = mrktSimGrids[(int)filter.PlanType];
                    currentGrid.RowFilter = filter.MarketPlanDataQuery();
                    currentGrid.Suspend = false;
                    currentGrid.Enabled = true;
                    currentGrid.Visible = true;

                    // force the grid to get a scrollbar refresh by tweaking the size by 1 pixel
                    int h2 = currentGrid.Size.Height;
                    if( h2 % 2 == 0 ) {
                        h2 = h2 + 1;        // even values bump up 1
                    }
                    else {
                        h2 = h2 - 1;        // odd values bump down 1
                    }
                    Size s2 = new Size( currentGrid.Size.Width, h2 );
                    currentGrid.Size = s2;
                }

                graphControl.Visible = false;

                // the info label text will be visible only if there is no data grid
                infoLabel.Visible = false;
            }
            else 
            {
                if( currentGrid != null ) {
                    currentGrid.Visible = false;
                }

                // graph check box is set
                if (UpdateGraphData())
                {
                    graphControl.Visible = true;
                }
                else
                {
                    graphControl.Visible = false;
                }

                // the info label text will be visible only if there is no graph
                infoLabel.Text = "(Selected Component(s) Have No Data to Graph)";
                infoLabel.Visible = true;
            }

            refreshButton.ForeColor = refreshNotNeededFg;
            refreshButton.BackColor = refreshNotNeededBkg;

            Status.ClearStatus( this );
        }

        /// <summary>
        /// Updates the graph data.
        /// </summary>
        /// <returns></returns>
        private bool UpdateGraphData() {
            Console.WriteLine( "*** UpdateGraphData()" );
            graphControl.ClearCurves();
            graphControl.GraphTitle = null;

            if( filter.SelectedMarketPlanComponentIDs == null ) {
                return false;
            }

            GraphDataProcessor gdProc = new GraphDataProcessor( filter.SelectedMarketPlanComponentIDs, theDb, filter, mrktSimGrids );
            GraphData gdata = gdProc.ProcessCurves( true );



            for( int i = 0; i < gdata.Curves.Count; i++ ){
                DataCurve curve = (DataCurve)gdata.Curves[ i ];
                Color c = (Color)gdata.LegendColors[ i ];
                string s = (string)gdata.Legends[ i ];
                graphControl.AddCurve( curve );
            }

            graphControl.CondenseLegends();

            if( graphControl.Plot.Curves.Count > 0 ) {
                graphControl.GraphTitle = gdata.Title;
                graphControl.Start = gdata.Start;
                graphControl.End = gdata.End;
                graphControl.Max = 1.1 * gdata.Max;

                //if( graphControl.Plot.Curves.Count > 1 ) {
                // automatically fill the last (or only) curve
                    ZedGraph.LineItem lineItem = (ZedGraph.LineItem)graphControl.Plot.Curves[ graphControl.Plot.Curves.Count - 1 ];
                    lineItem.Line.Fill = new ZedGraph.Fill( Color.White, lineItem.Color, 45 );
                //}

                graphControl.Visible = true;
                graphControl.Plot.DataChanged();
                return true;
            }
            else {
                return false;
            }
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.refreshButton = new System.Windows.Forms.Button();
            this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
            this.infoLabel = new System.Windows.Forms.Label();
            this.graphCheckBox = new System.Windows.Forms.CheckBox();
            this.dataLinkLabel = new Utilities.PopupMenuLinkLabel();
            this.graphControl = new MrktSimClient.Controls.MarketPlans.MarketPlanGraphControl();
            this.SuspendLayout();
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.BackColor = System.Drawing.Color.White;
            this.refreshButton.Location = new System.Drawing.Point( 518, 4 );
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size( 131, 20 );
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Turn Off Auto Update";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler( this.refreshButton_Click );
            // 
            // mrktSimGrid
            // 
            this.mrktSimGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mrktSimGrid.DescribeRow = null;
            this.mrktSimGrid.EnabledGrid = true;
            this.mrktSimGrid.Location = new System.Drawing.Point( 6, 29 );
            this.mrktSimGrid.Name = "mrktSimGrid";
            this.mrktSimGrid.RowFilter = null;
            this.mrktSimGrid.RowID = null;
            this.mrktSimGrid.RowName = null;
            this.mrktSimGrid.Size = new System.Drawing.Size( 656, 215 );
            this.mrktSimGrid.Sort = "";
            this.mrktSimGrid.TabIndex = 3;
            this.mrktSimGrid.Table = null;
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.infoLabel.Location = new System.Drawing.Point( 106, 126 );
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size( 444, 21 );
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "info1";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // graphCheckBox
            // 
            this.graphCheckBox.AutoSize = true;
            this.graphCheckBox.Checked = true;
            this.graphCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.graphCheckBox.Location = new System.Drawing.Point( 81, 6 );
            this.graphCheckBox.Name = "graphCheckBox";
            this.graphCheckBox.Size = new System.Drawing.Size( 55, 17 );
            this.graphCheckBox.TabIndex = 6;
            this.graphCheckBox.Text = "Graph";
            this.graphCheckBox.UseVisualStyleBackColor = true;
            this.graphCheckBox.CheckedChanged += new System.EventHandler( this.graphCheckBox_CheckedChanged );
            // 
            // dataLinkLabel
            // 
            this.dataLinkLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))) );
            this.dataLinkLabel.BottomMargin = 4;
            this.dataLinkLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dataLinkLabel.HighlightColor = System.Drawing.Color.LightSalmon;
            this.dataLinkLabel.LinkText = "Data";
            this.dataLinkLabel.Location = new System.Drawing.Point( 8, 0 );
            this.dataLinkLabel.MenuItemSpacing = 5;
            this.dataLinkLabel.Name = "dataLinkLabel";
            this.dataLinkLabel.PopupBackColor = System.Drawing.Color.White;
            this.dataLinkLabel.PopupFont = new System.Drawing.Font( "Arial", 8F );
            this.dataLinkLabel.PopupParentLevelsAbove = 2;
            this.dataLinkLabel.RightMargin = 5;
            this.dataLinkLabel.Size = new System.Drawing.Size( 43, 23 );
            this.dataLinkLabel.TabIndex = 7;
            this.dataLinkLabel.TabMargin = 15;
            this.dataLinkLabel.TopMargin = 9;
            // 
            // graphControl
            // 
            this.graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.graphControl.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.graphControl.GraphTitle = "Title";
            this.graphControl.Location = new System.Drawing.Point( 6, 29 );
            this.graphControl.Margin = new System.Windows.Forms.Padding( 0 );
            this.graphControl.Name = "graphControl";
            this.graphControl.Size = new System.Drawing.Size( 656, 215 );
            this.graphControl.Suspend = false;
            this.graphControl.TabIndex = 5;
            this.graphControl.Visible = false;
            // 
            // MarketPlanDataControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))) );
            this.Controls.Add( this.dataLinkLabel );
            this.Controls.Add( this.graphCheckBox );
            this.Controls.Add( this.graphControl );
            this.Controls.Add( this.infoLabel );
            this.Controls.Add( this.mrktSimGrid );
            this.Controls.Add( this.refreshButton );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.MinimumSize = new System.Drawing.Size( 644, 209 );
            this.Name = "MarketPlanDataControl";
            this.Size = new System.Drawing.Size( 665, 247 );
            this.ResumeLayout( false );
            this.PerformLayout();

        }
                #endregion

        private void ToggleAutoRefresh() {
            autoUpdate = !autoUpdate;
            if( autoUpdate ) {
                refreshButton.Text = refreshOffString;
                refreshButton.ForeColor = Color.Black;
                refreshButton.BackColor = refreshNotNeededBkg;
                dataLinkLabel.ClearAllItems();
                dataLinkLabel.AddItem( autoRefreshOfftemString, ToggleAutoRefresh );
            }
            else {
                refreshButton.Text = refreshNowString;
                refreshButton.ForeColor = refreshNotNeededFg;
                refreshButton.BackColor = refreshNotNeededBkg;
                dataLinkLabel.ClearAllItems();
                dataLinkLabel.AddItem( autoRefreshOntemString, ToggleAutoRefresh );
            }
        }

        #region Grid-Column Creation Methods
        //      - - - - - - - - -    code below mostly copied from old classes     - - - - - - - - - - 

        /// <summary>
        /// Create the appropriate columns in the component data grid.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        /// <param name="type">Component type specifier</param>
        static public void CreateTableStyle( ModelDb theDb, MrktSimGrid grid, ModelDb.PlanType type ) {
            switch( type ) {
                case ModelDb.PlanType.Price:
                    createPriceColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.Media:
                    createMediaColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.Coupons:
                    createCouponColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.Display:
                    createDisplayColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.Distribution:
                    createDistributionColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.ProdEvent:
                    createEventColumns( theDb, grid );
                    break;

                case ModelDb.PlanType.Market_Utility:
                    createUtilityColumns( theDb, grid );     //???what table does this one need???
                    break;
            }
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a price plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createPriceColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );

            grid.AddTextColumn( "productName", true );
            grid.AddTextColumn( "channelName", true );

            if( theDb.Model.profit_loss )
                grid.AddNumericColumn( "markup", false );

            grid.AddNumericColumn( "price", false );

            grid.AddNumericColumn( "percent_SKU_in_dist", "Distribution", false );

            grid.AddComboBoxColumn( "price_type", theDb.Data.price_type, "name", "id", false );
           
            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a media plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createMediaColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );


            grid.AddTextColumn( "product_name", true );
            grid.AddTextColumn( "segment_name", true );
            grid.AddTextColumn( "channel_name", true );

            string[] types = { "V", "A" };
            grid.AddComboBoxColumn( "media_type", types );
            grid.AddNumericColumn( "attr_value_G" );
            grid.AddNumericColumn( "message_awareness_probability" );
            grid.AddNumericColumn( "message_persuation_probability" );
            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a coupon plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createCouponColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );

            grid.AddTextColumn( "product_name", true );
            grid.AddTextColumn( "segment_name", true );
            grid.AddTextColumn( "channel_name", true );

            string[] types = { "C", "S" };
            grid.AddComboBoxColumn( "media_type", types );

            grid.AddNumericColumn( "attr_value_G", "Percent Receiving" );
            grid.AddNumericColumn( "attr_value_I" );
            grid.AddNumericColumn( "message_awareness_probability" );
            grid.AddNumericColumn( "message_persuation_probability" );
            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a display plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createDisplayColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );

            grid.AddTextColumn( "product_name", true );
            grid.AddTextColumn( "channel_name", true );

            grid.AddNumericColumn( "attr_value_F", false );

            grid.AddNumericColumn( "message_awareness_probability", false );
            grid.AddNumericColumn( "message_persuation_probability", false );
            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a distribution plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createDistributionColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );

            grid.AddTextColumn( "product_name", true );
            grid.AddTextColumn( "channel_name", true );

            grid.AddNumericColumn( "attr_value_F", false );
            grid.AddNumericColumn( "attr_value_G", false );

            grid.AddNumericColumn( "message_awareness_probability", false );
            grid.AddNumericColumn( "message_persuation_probability", false );
            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a price plan component.
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createEventColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddTextColumn( "product_name", true );
            grid.AddTextColumn( "segment_name", true );
            grid.AddTextColumn( "channel_name", true );

            if( Database.Nimo )
            {
                grid.AddComboBoxColumn( "type", ModelDb.product_event_type, "type", "id" );
            }

           
            grid.AddNumericColumn( "demand_modification" );

            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }

        /// <summary>
        /// Create columns in the data grid suitable for displaying a utility plan component (external factors).
        /// </summary>
        /// <param name="theDb"></param>
        /// <param name="grid"></param>
        static private void createUtilityColumns( ModelDb theDb, MrktSimGrid grid ) {
            grid.Clear();

            grid.AddComboBoxColumn( "market_plan_id", theDb.Data.market_plan, "name", "id", true );

            grid.AddComboBoxColumn( "product_id", theDb.Data.product, "product_name", "product_id", true );
            grid.AddComboBoxColumn( "channel_id", theDb.Data.channel, "channel_name", "channel_id", true );
            grid.AddComboBoxColumn( "segment_id", theDb.Data.segment, "segment_name", "segment_id", true );

            grid.AddNumericColumn( "percent_dist", false );
            grid.AddNumericColumn( "awareness", false );
            grid.AddNumericColumn( "persuasion", false );
            grid.AddNumericColumn( "utility", false );

            grid.AddDateColumn( "start_date" );
            grid.AddDateColumn( "end_date" );

            grid.Reset();
        }
        #endregion

    }
}
