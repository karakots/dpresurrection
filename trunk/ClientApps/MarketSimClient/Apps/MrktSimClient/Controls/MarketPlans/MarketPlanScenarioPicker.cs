using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Dialogs;
using Common;

using ModelView;
using ModelView.Dialogs;

using MarketSimUtilities;
using MrktSimDb;
using Utilities;

namespace MrktSimClient.Controls.MarketPlans
{
    /// <summary>
    /// A control for selecting a scenario in the current model.  For use in the market plans UI.
    /// </summary>
    class MarketPlanScenarioPicker : MrktSimControl
    {
        private ComboBox scenarioComboBox;
        private Label scenarioDetailsLabel;
        private System.Data.DataView scenarioView;
        private MrktSimDBSchema.scenarioRow currentScenario;
        private PopupMenuLinkLabel popupMenuLinkLabel;

        public delegate void FireSelectedScenarioRowChanged( MrktSimDBSchema.scenarioRow scenarioRow );
        public event FireSelectedScenarioRowChanged SelectedScenarioRowChanged;

        private string newItemString = "New...";
        private string copyItemString = "Copy";
        private string editItemString = "Edit";
        private string deleteItemString = "Delete";
        private string extendPlanstemString = "Extend Market Plans...";
        private string transferDatatemString = "Transfer Market Plan Data...";
        private string replicateDatatemString = "Intra-Plan Data Replication...";
        private string fillPlanstemString = "Fill Market Plans...";

        private string copyMsg = "Do you want to make a copy of the selected scenario?";
        private string copyMsg2 = "Scenario to Copy:";
        private string copyStatusMsg = "Copying Scenario ({0})...";
        private string copyStatusMsg2 = "Selecting Copied Scenario...";
        private string copyTitle = "Confirm Scenario Copy";
        private string copyDoneMsg = "Scenario Copied Successfully";
        private string copyHelpTag = "CopyScenario";

        private string delMsg = "Are you sure you want to delete the selected scenario?";
        private string delMsg2 = "Scenario to Delete:";
        private string delStatusMsg = "Deleting Scenario ({0})...";
        private string delTitle = "Confirm Scenario Delete";
        private string delDoneMsg = "Scenario Deleted Successfully";
        private string delHelpTag = "DeleteScenario";

        private string newStatusMsg = "Creating New Scenario ({0})...";
        private string editStatusMsg = "Updating Edited Scenario ({0})...";
        private string changeScenarioStatus = "Changing Scenario...";

        private string extStatusMsg = "Extending Plans in Scenario ({0})...";
        private string repStatusMsg = "Replicating Data for Plans. (Scenario = {0})...";
        private string xferStatusMsg = "Transferring Data to Plans. (Scenario = {0})...";

        private string noScenarioChoiceString = "<No Scenarios Defined>";
        private string noScenarioDescrString = "Create a scenario by clicking the Scenario link and selecting New from the popup menu.";
        private string unassignedItemsChoiceString = "Unassigned Items";
        private string unassignedItemsDescrString = "Market Plans used by NO scenarios and Components used by NO Market Plans.";

        private MrktSimDBSchema.scenarioRow copiedScenario;
        private EditScenario editDlg;
        private EditScenario newDlg;

        private MarketPlanControlFilter filter;

        /// <summary>
        /// Sets the view filter object.
        /// </summary>
        public MarketPlanControlFilter Filter {
            set {
                filter = value;
            }
        }
        
        public System.Data.DataView ScenarioView {
            get {
                return scenarioView;
            }
        }

        /// <summary>
        /// Sets the panel that is used to display the popup menu items
        /// </summary>
        /// <remarks>The popup menu panel is typically owned by a higher-level Control so that it isn't clipped at the edge of this control</remarks>
        public Panel PopupMenuPanel {
            set {
                this.popupMenuLinkLabel.PopupMenuPanel = value;
            }
        }

        public override MrktSimDb.ModelDb Db {
            set {
                theDb = value;

                UpdateMenuItems();
            }
        }

        /// <summary>
        /// Creates a new MarketPlanScenarioPicker object.
        /// </summary>
        public MarketPlanScenarioPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            popupMenuLinkLabel.AddItem( newItemString, NewScenario );
            popupMenuLinkLabel.AddItem( copyItemString, CopyScenario );
            popupMenuLinkLabel.AddItem( editItemString, EditScenario );
            popupMenuLinkLabel.AddItem( deleteItemString, DeleteScenario );

            if( MrktSim.DevlMode == true ) {
                popupMenuLinkLabel.AddItem( extendPlanstemString, ExtendScenario );
                popupMenuLinkLabel.AddItem( transferDatatemString, TransferScenarioData );
                popupMenuLinkLabel.AddItem( replicateDatatemString, ReplicateScenarioData );
                popupMenuLinkLabel.AddItem( fillPlanstemString, FillScenarioPlans );
           }
            
            popupMenuLinkLabel.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( EnablePopupMenuItems );
        }

        /// <summary>
        /// Selects the specified item in the scenario pulldown list.
        /// </summary>
        /// <param name="selIndx"></param>
        public void SelectItem( int selIndx ) {
            if( selIndx < scenarioComboBox.Items.Count ) {
                scenarioComboBox.SelectedIndex = selIndx;
            }
        }

        /// <summary>
        /// Updates the items in the menu to match the data in the database scenario table.   Clears the selection.
        /// </summary>
        private void UpdateMenuItems() {
            MrktSimDBSchema.scenarioRow[] rows = null;
            MrktSimDBSchema.scenarioRow[] selrows = (MrktSimDBSchema.scenarioRow[])theDb.Data.scenario.Select();

            int addLen = 1;
            if( selrows.Length > 1 ){
                addLen = 2;
            }

            //make a new array, with room for "All" and "Unassigned" special items
            rows = new MrktSimDBSchema.scenarioRow[ selrows.Length + addLen ];
            MrktSimDBSchema.scenarioRow copyRow = null;
            if( selrows.Length >= 1 ) {
                copyRow = (MrktSimDBSchema.scenarioRow)selrows[ 0 ];
            }

            if( selrows.Length > 1 ) {
                string[] names = new string[ selrows.Length ];
                for( int i = 0; i < selrows.Length; i++ ) {
                    names[ i ] = selrows[ i ].name;
                }
                Array.Sort( names, selrows );

                Array.Copy( selrows, 0, rows, 1, selrows.Length );

                // ... add the "All" item as the 1st item
                MrktSimDBSchema.scenarioRow allRow = (MrktSimDBSchema.scenarioRow)theDb.Data.scenario.NewRow();

                allRow.model_id = copyRow.model_id;
                allRow.user_name = copyRow.user_name;

                allRow.name = "All";
                allRow.descr = "All of the scenarios in this model";
                allRow.scenario_id = ModelDb.AllID;

                rows[ 0 ] = allRow;

                scenarioComboBox.Enabled = true;
            }
            else if( selrows.Length == 1 ) {
                rows[ 0 ] = selrows[ 0 ];
                scenarioComboBox.Enabled = true;
            }
            else {
                // there are 0 scenarios
                rows = null;
                scenarioComboBox.Enabled = false;
                scenarioComboBox.Text = noScenarioChoiceString;
                scenarioDetailsLabel.Text = noScenarioDescrString;
            }

            if( selrows.Length >= 1 ) {
                // ...and add the "Unassigned Items" item as the last item
                MrktSimDBSchema.scenarioRow unassRow = (MrktSimDBSchema.scenarioRow)theDb.Data.scenario.NewRow();

                unassRow.model_id = copyRow.model_id;
                unassRow.user_name = copyRow.user_name;

                unassRow.name = unassignedItemsChoiceString;
                unassRow.descr = unassignedItemsDescrString;
                unassRow.scenario_id = -2;

                rows[ rows.Length - 1 ] = unassRow;
            }

            scenarioComboBox.DataSource = rows;
            scenarioComboBox.DisplayMember = "name";
            scenarioComboBox.ValueMember = "scenario_id";

            scenarioComboBox.Update();
        }

       #region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            this.scenarioComboBox = new System.Windows.Forms.ComboBox();
            this.scenarioDetailsLabel = new System.Windows.Forms.Label();
            this.scenarioView = new System.Data.DataView();
            this.popupMenuLinkLabel = new Utilities.PopupMenuLinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).BeginInit();
            this.SuspendLayout();
            // 
            // scenarioComboBox
            // 
            this.scenarioComboBox.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenarioComboBox.FormattingEnabled = true;
            this.scenarioComboBox.Location = new System.Drawing.Point( 71, 5 );
            this.scenarioComboBox.Name = "scenarioComboBox";
            this.scenarioComboBox.Size = new System.Drawing.Size( 200, 23 );
            this.scenarioComboBox.TabIndex = 1;
            this.scenarioComboBox.SelectedIndexChanged += new System.EventHandler( this.scenarioComboBox_SelectedIndexChanged );
            // 
            // scenarioDetailsLabel
            // 
            this.scenarioDetailsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioDetailsLabel.AutoSize = true;
            this.scenarioDetailsLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.scenarioDetailsLabel.Location = new System.Drawing.Point( 277, 9 );
            this.scenarioDetailsLabel.Name = "scenarioDetailsLabel";
            this.scenarioDetailsLabel.Size = new System.Drawing.Size( 100, 14 );
            this.scenarioDetailsLabel.TabIndex = 2;
            this.scenarioDetailsLabel.Text = "(select a scenario)";
            // 
            // popupMenuLinkLabel
            // 
            this.popupMenuLinkLabel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.popupMenuLinkLabel.BottomMargin = 4;
            this.popupMenuLinkLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.popupMenuLinkLabel.HighlightColor = System.Drawing.Color.Red;
            this.popupMenuLinkLabel.LinkText = "Scenario";
            this.popupMenuLinkLabel.Location = new System.Drawing.Point( 3, 2 );
            this.popupMenuLinkLabel.MenuItemSpacing = 5;
            this.popupMenuLinkLabel.Name = "popupMenuLinkLabel";
            this.popupMenuLinkLabel.PopupBackColor = System.Drawing.Color.White;
            this.popupMenuLinkLabel.PopupFont = new System.Drawing.Font( "Arial", 8F );
            this.popupMenuLinkLabel.PopupParentLevelsAbove = 2;
            this.popupMenuLinkLabel.RightMargin = 5;
            this.popupMenuLinkLabel.Size = new System.Drawing.Size( 66, 25 );
            this.popupMenuLinkLabel.TabIndex = 3;
            this.popupMenuLinkLabel.TabMargin = 15;
            this.popupMenuLinkLabel.TopMargin = 9;
            // 
            // MarketPlanScenarioPicker
            // 
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.Controls.Add( this.popupMenuLinkLabel );
            this.Controls.Add( this.scenarioDetailsLabel );
            this.Controls.Add( this.scenarioComboBox );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.MaximumSize = new System.Drawing.Size( 0, 119 );
            this.MinimumSize = new System.Drawing.Size( 279, 33 );
            this.Name = "MarketPlanScenarioPicker";
            this.Size = new System.Drawing.Size( 426, 33 );
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
                #endregion

        #region Scenario Modification Methods
        /// <summary>
        /// Allows the user to create a new scenario.
        /// </summary>
        private void NewScenario() {
            newDlg = new EditScenario( filter.ProductIDs, theDb );
            newDlg.CurrentScenario = null;

            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList used = relater.GetPlansForScenario( -1 );
            newDlg.SetUsedList( used );

            DialogResult rslt = newDlg.ShowDialog();

            if( rslt != DialogResult.OK ) {
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( newStatusMsg, newDlg.ObjName );
            status.UpdateUIAndContinue( NewScenario_Part2, statusMsg, 50 );
        }

        private void NewScenario_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            MrktSimDBSchema.scenarioRow newScenario = theDb.CreateStandardScenario( newDlg.ObjName );
            newScenario.descr = newDlg.ObjDescription;
            this.currentScenario = newScenario;

            UpdateMenuItems();
            scenarioComboBox.SelectedValue = newScenario.scenario_id;

            // handle possible changes to the scenario_market_plans data
            ArrayList removedIDs = newDlg.RemovedUsedIDs;
            ArrayList addedIDs = newDlg.AddedUsedIDs;

            if( removedIDs.Count > 0 || addedIDs.Count > 0 ) {
                // we have items to add or remove from scenario_market_plans
                AddAndRemovePlans( addedIDs, removedIDs );
            }

            Status.ClearStatus( this );
        }

        /// <summary>
        /// Allows the user to edit the currently selected scenario.
        /// </summary>
        private void EditScenario() {
            if( scenarioComboBox.SelectedItem == null ) {
                return;
            }

            editDlg = new EditScenario( filter.ProductIDs, theDb );
            editDlg.CurrentScenario = currentScenario;

            MarketPlanControlRelater relater = new MarketPlanControlRelater( theDb );
            ArrayList used = relater.GetPlansForScenario( currentScenario.scenario_id );
            editDlg.SetUsedList( used );

            DialogResult resp = editDlg.ShowDialog();

            if( resp != DialogResult.OK ) {
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( editStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( EditScenario_Part2, statusMsg, 50 );
        }

        private void EditScenario_Part2() {

            Utilities.Status.SetWaitCursor( this );

            int curId = currentScenario.scenario_id;
            UpdateMenuItems();
            scenarioComboBox.SelectedValue = curId;    // reselect the item we just edited

            // handle possible changes to the scenario_market_plans data
            ArrayList removedIDs = editDlg.RemovedUsedIDs;
            ArrayList addedIDs = editDlg.AddedUsedIDs;

            if( removedIDs.Count > 0 || addedIDs.Count > 0 ) {
                // we have items to add or remove from scenario_market_plans
                AddAndRemovePlans( addedIDs, removedIDs );
            }

            Status.ClearStatus( this );
        }

        public void AddAndRemovePlans( ArrayList addedIDs, ArrayList removedIDs ) {
            AddAndRemovePlans( addedIDs, removedIDs, currentScenario, theDb );

            // since the list of market plans has changed, update the rest of the display
            if( SelectedScenarioRowChanged != null ) {
                SelectedScenarioRowChanged( currentScenario );
            }
        }

        public static void AddAndRemovePlans( ArrayList addedIDs, ArrayList removedIDs, MrktSimDBSchema.scenarioRow scenario, ModelDb addRmvDb ) {
            DataTable table = addRmvDb.Data.scenario_market_plan;
            MrktSimDBSchema.scenario_market_planRow[] deleteRows = null;
            MrktSimDBSchema.scenario_market_planRow[] addRows = null;

            if( removedIDs.Count > 0 ) {
                string query = String.Format( "scenario_id = {0} AND (", scenario.scenario_id );
                for( int i = 0; i < removedIDs.Count; i++ ) {
                    query += String.Format( "market_plan_id = {0}", (int)removedIDs[ i ] );
                    if( i < (removedIDs.Count - 1) ) {
                        query += " OR ";
                    }
                    else {
                        query += ")";
                    }
                }
                deleteRows = (MrktSimDBSchema.scenario_market_planRow[])table.Select( query );
            }
            if( addedIDs.Count > 0 ) {
                addRows = new MrktSimDBSchema.scenario_market_planRow[ addedIDs.Count ];

                for( int i = 0; i < addedIDs.Count; i++ ) {
                    addRows[ i ] = (MrktSimDBSchema.scenario_market_planRow)table.NewRow();
                    addRows[ i ].model_id = scenario.model_id;
                    addRows[ i ].scenario_id = scenario.scenario_id;
                    addRows[ i ].market_plan_id = (int)addedIDs[ i ];
                }
            }

            // actually make the changes to the scenario_market_plan data
            if( removedIDs.Count > 0 ) {
                foreach( MrktSimDBSchema.scenario_market_planRow delRow in deleteRows ) {
                    // not calling theDb.RemovePlanFromScenario() here since all it adds is a check that the plan is indeed in the scenario (already checked by the UI)
                    delRow.Delete();
                }
            }
            if( addedIDs.Count > 0 ) {
                foreach( MrktSimDBSchema.scenario_market_planRow addRow in addRows ) {
                    // not calling theDb.AddPlanToScenario() here since all it adds is a check to prevent duplicate plans in the scenario (already checked by the UI)
                    table.Rows.Add( addRow );
                }
            }
        }
   
        /// <summary>
        /// Deletes the currently selected scenario.
        /// </summary>
        private void DeleteScenario() {
            if( scenarioComboBox.SelectedItem == null ) {
                return;
            }

            ConfirmDialog cDlg = new ConfirmDialog( delMsg, delMsg2, currentScenario.name, delTitle, delHelpTag );
            cDlg.SelectWarningIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp != DialogResult.Yes ) {
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( delStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( DeleteScenario_Part2, statusMsg, 50 );
        }

        private void DeleteScenario_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            int selIndx = scenarioComboBox.SelectedIndex;

            currentScenario.Delete();

            UpdateMenuItems();
            if( selIndx > 0 ) {
                selIndx -= 1;        // select the item above the deleted one
            }
            if( scenarioComboBox.Items.Count == 1 ) {      // only one item left (All may have diasppeared also)
                selIndx = 0;
            }
            else if( scenarioComboBox.Items.Count == 0 ) {
                selIndx = -1;
            }
            scenarioComboBox.SelectedIndex = selIndx;

            Status.ClearStatus( this );

            CompletionDialog mDlg = new CompletionDialog( delDoneMsg );
            mDlg.ShowDialog();
        }

        /// <summary>
        /// Copies the currently selected scenario.
        /// </summary>
        private void CopyScenario() {
            if( scenarioComboBox.SelectedItem == null ) {
                return;
            }

            ConfirmDialog cDlg = new ConfirmDialog( copyMsg, copyMsg2, currentScenario.name, copyTitle, copyHelpTag );
            cDlg.SelectQuestionIcon();
            DialogResult resp = cDlg.ShowDialog();

            if( resp != DialogResult.Yes ) {
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );
            System.Threading.Thread backgroundThread = status.StartBackgroundThread( CopyScenarioNow );

            string statusMsg = String.Format( copyStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( CopyScenario_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void CopyScenario_Part2() {
            UpdateMenuItems();
            scenarioComboBox.SelectedValue = copiedScenario.scenario_id;
             Status status = new Status( this );
             status.UpdateUIAndContinue( CopyScenario_Part3, copyStatusMsg2, 80 );
        }

        private void CopyScenario_Part3() {
            Status.ClearStatus( this );

            CompletionDialog mDlg = new CompletionDialog( copyDoneMsg );
            mDlg.ShowDialog();
        }

        private void CopyScenarioNow() {
            copiedScenario = theDb.CopyScenario( currentScenario, currentScenario.name );
        }

        private MrktSimClient.Controls.Dialogs.ExtendScenarioForm xForm;

        private void ExtendScenario() {
            xForm = new MrktSimClient.Controls.Dialogs.ExtendScenarioForm( theDb );
            DialogResult resp = xForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            string msg = "\r\n\r\n    Caution: The data replication process may alter multiple market plans.    \r\n\r\n    Proceed?\r\n";
            DialogResult resp2 = MessageBox.Show( msg, "Confirm Data Extension", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning );
            if( resp2 != DialogResult.OK ) {
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( extStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( ExtendScenario_Part2, statusMsg, 50 );
        }

        private void ExtendScenario_Part2() {
            
            Utilities.Status.SetWaitCursor( this );

            int nRowsCopied = theDb.ExtendComponentData( currentScenario.scenario_id, xForm.PlanTypeToExtend,
                xForm.ExtensionProductType, xForm.ExtensionStartDate, xForm.ExtensionEndDate, xForm.ExtensionPatternMonths );

            Status.ClearStatus( this );

            string msg2 = String.Format( "    Plan Extension complete.  {0} rows were added.    ", nRowsCopied );
            CompletionDialog mDlg = new CompletionDialog( msg2 );
            mDlg.ShowDialog();
        }

        private MrktSimClient.Controls.Dialogs.TransferPlanValuesForm rForm;

        private void ReplicateScenarioData() {
            rForm = new MrktSimClient.Controls.Dialogs.TransferPlanValuesForm( theDb );
            DialogResult resp = rForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            string msg = "\r\n\r\n    Caution: The data replication process may alter multiple market plans.    \r\n\r\n    Proceed?\r\n";
            DialogResult resp2 = MessageBox.Show( msg, "Confirm Data Replication", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning );
            if( resp2 != DialogResult.OK ) {
                return;
            }

            
            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( repStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( ReplicateScenario_Part2, statusMsg, 50 );
        }

        private void ReplicateScenario_Part2() {
            Utilities.Status.SetWaitCursor( this );
            int nRowsReplicated = theDb.ReplicateComponentData( currentScenario.scenario_id, rForm.PlanType,
                rForm.TransferProductType, rForm.TransferStartDate, rForm.TransferEndDate, rForm.TransferPatternMonths );

            Status.ClearStatus( this );

            string msg2 = String.Format( "    Awareness and Persusaion Replication complete.  {0} rows were changed.   ", nRowsReplicated );
            CompletionDialog mDlg = new CompletionDialog( msg2 );
            mDlg.ShowDialog();
        }

        private MrktSimClient.Controls.Dialogs.TransferScenarioPlanValuesForm transferForm;

        private void TransferScenarioData() {
            transferForm = new MrktSimClient.Controls.Dialogs.TransferScenarioPlanValuesForm( theDb );
            DialogResult resp = transferForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            string msg = "\r\n\r\n    Caution: The transfer process may alter multiple market plans.    \r\n\r\n    Proceed?\r\n";
            DialogResult resp2 = MessageBox.Show( msg, "Confirm Data Transfer", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning );
            if( resp2 != DialogResult.OK ) {
                return;
            }


            
            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            string statusMsg = String.Format( xferStatusMsg, currentScenario.name );
            status.UpdateUIAndContinue( TransferScenario_Part2, statusMsg, 50 );
        }

        private void TransferScenario_Part2() {
            Utilities.Status.SetWaitCursor( this );
            int nRowsTransferred = theDb.TransferComponentData( transferForm.SourceScenarioID, currentScenario.scenario_id, transferForm.TransferPlanType,
                transferForm.TransferProductType, transferForm.TransferColumns, transferForm.StartDate, transferForm.EndDate );

            Status.ClearStatus( this );

            string msg2 = String.Format( "    Transfer complete.  {0} rows were changed.    ", nRowsTransferred );
            CompletionDialog mDlg = new CompletionDialog( msg2 );
            mDlg.ShowDialog();
        }

        #endregion

        /// <summary>
        /// Responds to a change in the selected item by firng a SelectedScenarioRowChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scenarioComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( scenarioComboBox.SelectedItem == null ) {
                currentScenario = null;
                return;
            }

            ((ModelEditor)this.ParentForm).ProcessActive = true;         // disable all menus immediately
            
            Status status = new Status( this );
            status.UpdateUIAndContinue( scenarioComboBox_SelectedIndexChanged_Part2, changeScenarioStatus, 50 );
        }

        private void scenarioComboBox_SelectedIndexChanged_Part2(){

            currentScenario = (MrktSimDBSchema.scenarioRow)((DataRow)scenarioComboBox.SelectedItem);

            if( currentScenario.scenario_id != -2 ){
                scenarioDetailsLabel.ForeColor = Color.Black;
            }
            else {
                scenarioDetailsLabel.ForeColor = Color.Red;
            }
            scenarioDetailsLabel.Text = currentScenario.descr;

            if( SelectedScenarioRowChanged != null ) {
                SelectedScenarioRowChanged( currentScenario );
            }
            
            Status.ClearStatus( this );
        }

        private void FillScenarioPlans() {
            ConfirmDialog cDlg = new ConfirmDialog( "Fill every market plan in this scenario with all available Components of the appropriate product?",
                "Confirm Filling Market Plans" );
            cDlg.SetOkCancelButtonStyle();
            cDlg.SelectQuestionIcon();

            DialogResult resp = cDlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            MrktSimDBSchema.scenario_market_planRow[] smprows = currentScenario.Getscenario_market_planRows();
            for( int i = 0; i < smprows.Length; i++ ) {
                MrktSimDBSchema.market_planRow mpRow = smprows[ i ].market_planRow;
                string prodName = mpRow.name;
                int planProd = mpRow.product_id;
                string cquery = "type <> 0 AND type <> 5 AND product_id = " + planProd.ToString();
                MrktSimDBSchema.market_planRow[] comps = (MrktSimDBSchema.market_planRow[])theDb.Data.market_plan.Select( cquery );
                for( int j = 0; j < comps.Length; j++ ) {
                    MrktSimDBSchema.market_planRow comp = comps[ j ];
                    string compName = comp.name;
                    theDb.CreatePlanRelation( mpRow.id, comp.id );
                }
            }
            CompletionDialog mDlg = new CompletionDialog( "Market Plans Filled Successfuly" );
        }

        private void EnablePopupMenuItems() {
            bool enableNew = true;
            bool enableCopy = true;
            bool enableEdit = true;
            bool enableDelete = true;

            if( (currentScenario == null) || (currentScenario.scenario_id < 0) ) {
                enableCopy = false;
                enableEdit = false;
                enableDelete = false;
            }

            if( ((ModelEditor)this.ParentForm).ProcessActive == true  ) {
                enableCopy = false;
                enableCopy = false;
                enableEdit = false;
                enableDelete = false;
            }

            popupMenuLinkLabel.EnableAllLinks();
            if( !enableNew ) {
                popupMenuLinkLabel.DisableLink( newItemString );
            }
            if( !enableCopy ) {
                popupMenuLinkLabel.DisableLink( copyItemString );
            }
            if( !enableEdit ) {
                popupMenuLinkLabel.DisableLink( editItemString );
            }
            if( !enableDelete ) {
                popupMenuLinkLabel.DisableLink( deleteItemString );
            }
        }
    }
}
