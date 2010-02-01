using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using MrktSimDb;

using Common.Dialogs;
using MarketSimUtilities;
using ModelView;
using Results;
using Utilities;

namespace MrktSimClient.Controls 
{
    public partial class ModelControl : UserControl
    {
        private string editItemString = "Edit Model";
        private string resultsItemString = "Results...";
        private string renameItemString = "Rename Model...";
        private string exportItemString = "Export Model...";
        private string copyItemString = "Copy Model";

        private string openStatusMsg1 = "Loading Model data ({0})...";
        private string openStatusMsg2 = "Displaying Model ({0})....";
        private string openStatusMsg3 = "Displaying Model ({0})...";
        private string dateRealignMsg = "One or more Market Plan start/end dates have been adjusted to align with their associated data.";
        private string dateRealignTitle = "Dates Realigned";

        private string copyMsg1 = "Copy the selected model?";
        private string copyMsg2 = "Model to Copy:";
        private string copyTitle = "Confirm Model Copy";
        private string copyDoneMsg = "Model Copied Successfully";
        private string copyStatus = "Copying Model data ({0})...";

        private string renameTitle = "Rename Model";
        private string renameHelpTag = "RenameModel";

        private string clearCheckpointMessage = "Do you want to remove the Checkpoint ({1}) from the {0} Model?\n\n(Once the Checkpoint has been removed, you must run a Checkpoint Simulation to re-establish it.)\n\nRemove Checkpoint now?";
        private string clearCheckpointTitle = "Confirm Removing Checkpoint";
        private string clearCheckpointHelpTag = "ClearCheckpoint";
        private string clearCheckpointStatus = "Clearing Checkpoint ({0}:{1})...";

        private string establishCheckpointMessage = "To set a Checkpoint, you must run a Checkpoint Simulation.\n\nThe Checkpoint date will be the end date of the Checkpoint Simulation.";
        private string establishCheckpointTitle = "How to Establish a Checkpoint";
        private string establishCheckpointHelpTag = "EstablishCheckpoint";

        private string confirmEditMessage = "Open Model Editor?";
        private string confirmEditTitle = "";

        // where we display the model
//        private ModelEditor form;
        private ModelDb openModelDb = null;
        private bool realignedPlanOnLoading = false;
        private MsModelNode node;
        private ModelEditor modelEditorForm;
        private string exportFileName;
        private string exportStatus = "Exporting Model data ({0})...";
        private string exportDoneMsg = "Model Exported Successfully";
        private MsModelNode modelToCopy;
        private string copiedModelName;

        public bool ScenarioIsSelected {
            get {
                return (scenarioList.SelectedIndex != -1);
            }
        }

        public override void Refresh() {
            Refresh( false );
        }

        private void Refresh( bool modelEditorIsClosing )
        {
            if( node.Db.ModelDeleted( Node.Model.model_id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();
                return;
            }

            if( this.ParentForm != null ) {
                ((MrktSim)this.ParentForm).ActiveModelNode = this.Node;         // update to make the Edit Model button enabling reflect the enabled/disabled state of this control
            }
            else if( MrktSim.ActiveForm != null ) {
                MrktSim.ActiveMrktSimForm.SetToolstripButtonEnabling( modelEditorIsClosing );       
            }

            string nameToKeepSelected = null;      // preserve the selected item - use the name since it will be unique in this view
            if( scenarioList.SelectedIndex != -1 ) {
                nameToKeepSelected = scenarioList.SelectedItem.ToString();
            }

            scenarioList.Items.Clear();

            // sort the names alphabetically
            string[] subnodeNames = new string[ Node.Nodes.Count ];
            MsScenarioNode[] subnodes = new MsScenarioNode[ Node.Nodes.Count ];
            for( int n = 0; n < Node.Nodes.Count; n++ ) {
                subnodes[ n ] = (MsScenarioNode)Node.Nodes[ n ];
                subnodeNames[ n ] = ((MsScenarioNode)Node.Nodes[ n ]).ToString();
            }
            Array.Sort( subnodeNames, subnodes );
            for( int n = 0; n < subnodes.Length; n++ ) {
                scenarioList.Items.Add( subnodes[ n ] );
            }

            modName.Text = Node.ToString();
            modName.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            descBox1.Text = Node.Model.descr;

            RefreshCheckpoint( false );

            //re-select the same item that was selected before
            for( int i = 0; i < scenarioList.Items.Count; i++ ) {
                if( scenarioList.Items[ i ].ToString() == nameToKeepSelected ) {
                    scenarioList.SelectedIndex = i;
                    break;
                }
            }

            // handle this control becoming visible as a result of a "back" operation
            if( scenarioList.SelectedIndex < 0 && this.Tag != null && this.Tag is ScenarioControl ) {
                int scenIDToSelect = -1;
                if( ((ScenarioControl)this.Tag).Scenario.RowState != DataRowState.Detached ) {
                    scenIDToSelect = ((ScenarioControl)this.Tag).Scenario.scenario_id;
                }

                if( scenIDToSelect != -1 ) {
                    foreach( MsScenarioNode sitem in scenarioList.Items ) {
                        if( sitem.Id == scenIDToSelect ) {
                            scenarioList.SelectedItem = sitem;
                            break;
                        }
                    }
                }
            }

            if (Node.ActiveNode)
            {
                expandLink.Visible = false;
                this.splitContainer1.Panel2Collapsed = false;

                if (scenarioList.SelectedItem != null)
                {
                    ((MsScenarioNode)scenarioList.SelectedItem).Control.Refresh();
                }
                else {
                    // hide the scenario view since there is no selected scenario
                    this.splitContainer1.Panel2.Controls.Clear();
                    this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
                }
            }
            else
            {
                expandLink.Visible = true;
                this.splitContainer1.Panel2Collapsed = true;
            }

            base.Refresh();
        }

        public void RefreshCheckpoint( bool reloadData ) {
            if( reloadData ) {
                Node.Db.RefreshTable( Node.Db.Data.Model_info );
            }

            if( Node.Model.RowState == DataRowState.Deleted || Node.Model.RowState == DataRowState.Detached ) {
                this.checkpointCheckBox.Text = "x";
                return;
            }

            if( Node.Model.checkpoint_valid ) {
                this.checkpointCheckBox.Text = "Checkpoint: " + Node.Model.checkpoint_date.ToShortDateString();
                this.checkpointCheckBox.ForeColor = Color.Black;
            }
            else {
                this.checkpointCheckBox.Text = "Checkpoint: off";
            }
            this.checkpointCheckBox.CheckedChanged -= new EventHandler( checkpointCheckBox_CheckedChanged );
            this.checkpointCheckBox.Checked = Node.Model.checkpoint_valid;
            this.checkpointCheckBox.CheckedChanged += new EventHandler( checkpointCheckBox_CheckedChanged );
        }

        /// <summary>
        /// Sets or gets the ModelEditor form that this control can launch
        /// </summary>
        public ModelEditor ModelEditorForm
        {
            set
            {
                modelEditorForm = value;

                modelEditorForm.Closed += new EventHandler( modelEditorForm_Closed );
            }

            get
            {
                return modelEditorForm;
            }
        }

        /// <summary>
        /// Refreshes the main-form data tables (since the Model Editor may have changed the master tables)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelEditorForm_Closed( object sender, EventArgs e )
        {
            if( !openModelDb.ReadOnly ) {

                // update scenarios if needed.
                Node.Db.RefreshTable( Node.Db.Data.simulation, "id" );
                Node.Db.RefreshTable( Node.Db.Data.scenario, "scenario_id" );
                Node.Db.RefreshTable( Node.Db.Data.Model_info );

                this.Node.Refresh();

                this.Refresh( true );
            }

            openModelDb = null;
            modelEditorForm = null;
            this.statusControl = this;   //revert to regular status (this control is no longer showing its model editor)
        }

        public MsModelNode Node
        {
            get
            {
                return node;
            }

            set
            {
                node = value;

                Refresh();
            }
        }

        public MrktSimDBSchema.Model_infoRow Model
        {
            get
            {
                return Node.Model;
            }
        }

        public ModelControl()
        {
            InitializeComponent();

            this.scenarioList.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.modName.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.checkpointCheckBox.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.label1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.panel1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.splitContainer1.Panel2.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.splitContainer2.Panel1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;

            this.modelLink.AddItem( editItemString, openModelButton );
            this.modelLink.AddItem( resultsItemString, viewResults );
            this.modelLink.AddItem( renameItemString, renameModelItem_Click );
            this.modelLink.AddItem( exportItemString, exportModelButton_Click );
            this.modelLink.AddItem( copyItemString, copyModelButton_Click );

            this.modelLink.PopupMenuPanel = this.popupMenuPanel;

            this.modelLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetModelMenuItemEnabling );

            this.modName.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            this.statusControl = this;
        }

        private ContainerControl statusControl;
        
        /// <summary>
        /// Starts the process of opening the Model form for the selected model
        /// </summary>
        public void openModelButton() {
            OpenModelFrom( this );
        }

        public void OpenModelFrom( ContainerControl opener ) {

            if( openModelDb != null)
            {
                if( this.ParentForm != null )
                {
                    ((MrktSim)this.ParentForm).ProcessActive = false;         // re-enable menus
                }
                
                this.Refresh( true );

                if( ModelEditorForm != null )
                {
                    if( ModelEditorForm.WindowState == FormWindowState.Minimized )
                    {
                        ModelEditorForm.WindowState = FormWindowState.Normal;
                    }

                    ModelEditorForm.Activate();
                }
                return;
            }

            if(!checkModelStatus()) {
                return;
            }

          

            ConfirmDialog cdlg = new ConfirmDialog( confirmEditMessage, confirmEditTitle );
            cdlg.SetOkCancelButtonStyle();
            cdlg.SelectQuestionIcon();
            cdlg.Width -= 270;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ){
                if( this.ParentForm != null ) {
                    ((MrktSim)this.ParentForm).ProcessActive = false;         // re-enable menus
                }
                this.Refresh( true );
                return;
            }
           
            this.statusControl = opener;                  //this control is pretty much disabled as long as it is showing a model editor

            Status.SetWaitCursor( this.statusControl );

            if( this.ParentForm != null ) {
                ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately
            }
            this.checkpointCheckBox.Enabled = false;

            openModelDb = new ModelDb();
            openModelDb.Connection = Node.Db.Connection;
            openModelDb.ModelID = Model.model_id;

            string who = null;
            openModelDb.Open(out who);

            Status.ClearWaitCursor( this.statusControl );

            DialogResult rslt;
            if( openModelDb.ReadOnly ) {
                rslt = MessageBox.Show( MrktSim.Message( "Model.Locked" ) + who, "Continue?", MessageBoxButtons.OKCancel );

                if( rslt != DialogResult.OK ) {
                 
                    if( this.ParentForm != null ) {
                        ((MrktSim)this.ParentForm).ProcessActive = false;         // re-enable menus
                    }
                    this.Refresh( true );

                    openModelDb = null;
                    this.statusControl = this;

                    return;
                }
            }
            Status status = new Status( this.statusControl );

            // refresh the database in a background thread
            System.Threading.Thread backgroundThread = status.StartBackgroundThread( openModelDb.Refresh );

            // update the status while the background thread runs
            string statusMsg = String.Format( openStatusMsg1, Node.ToString() );
            status.UpdateUIAndContinue( OpenModelButtonPart2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void OpenModelButtonPart2() {

            if( !openModelDb.ReadOnly ) {
                realignedPlanOnLoading = openModelDb.AllignPlansWithData();
            }
            string statusMsg = String.Format( openStatusMsg2, Node.ToString() );
            Status status = new Status( this.statusControl );
            status.UpdateUIAndContinue( OpenModelButtonPart2A, statusMsg, 90 );
        }

         private void OpenModelButtonPart2A(){
             this.ModelEditorForm = new ModelEditor();    // this sets up a Closed event handler for the model editor form

             this.ModelEditorForm.Db = openModelDb;

            string statusMsg = String.Format( openStatusMsg3, Node.ToString() );
            Status status = new Status( this.statusControl );
            status.UpdateUIAndContinue( OpenModelButtonPart3, statusMsg, 90 );
        }

        private void OpenModelButtonPart3() {
            this.ModelEditorForm.Show();

            ClearStatusDisplay();

            if( realignedPlanOnLoading == true ) {
                MessageBox.Show( dateRealignMsg, dateRealignTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
            }
        }

        /// <summary>
        ///  Enables or disables the appropriate menu items.  
        /// </summary>
        /// <remarks>Invoked by the OnBeforeActivate event of the popup menu so the menu state is always up to date.</remarks>
        private void SetModelMenuItemEnabling() {
            bool disableEdit = false;
            bool disableResults = false;
            bool disableRename = false;
            bool disableExport = false;
            bool disableCopy = false;

            if( ((MrktSim)this.ParentForm).ProcessActive == true ) {
                disableEdit = true;
                disableResults = true;
                disableRename = true;
                disableExport = true;
                disableCopy = true;
            }

            this.modelLink.EnableAllLinks();
            if( disableEdit ) {
                this.modelLink.DisableLink( this.editItemString );
            }
            if( disableResults == true ) {
                this.modelLink.DisableLink( this.resultsItemString );
            }
            if( disableRename == true ) {
                this.modelLink.DisableLink( this.renameItemString );
            }
            if( disableExport == true ) {
                this.modelLink.DisableLink( this.exportItemString );
            }
            if( disableCopy == true ) {
                this.modelLink.DisableLink( this.copyItemString );
            }
        }

        private void scenarioList_DoubleClick( object sender, EventArgs e )
        {
            gotoScenario();

        }

        private void scenarioList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();

            MsScenarioNode scenario = (MsScenarioNode) scenarioList.SelectedItem;

            if( this.ParentForm != null ) {
                ((MrktSim)this.ParentForm).ActiveScenarioNode = scenario;
            }

            if (scenario != null)
            {
                if( node.Db.ScenarioDeleted( scenario.Scenario.scenario_id ) ) {


                    ((MrktSim)this.ParentForm).ResetUI();
                }
                else {

                    this.splitContainer1.Panel2.Controls.Add( scenario.Control );

                    scenario.Control.Dock = DockStyle.Fill;
                    scenario.Control.Refresh();
                }
            }

        }

        private void gotoScenario()
        {
            MsScenarioNode scene = (MsScenarioNode)scenarioList.SelectedItem;

            if (scene != null)
            {
                ((MrktSim)this.ParentForm).ActivateNode(scene);
            }
        }

        private void scenarioLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void expandLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ((MrktSim)this.ParentForm).ActivateNode(Node);
        }

        private void renameModelItem_Click()
        {
            if( !checkModelStatus() ) {
                return;
            }

            string who = null;

            if( node.Db.ModelLocked(node.Model.model_id, out who)) {
                MessageBox.Show(MrktSim.Message("Model.Locked by user: " + who));
                return;
            }

            NameAndDescr2 dlg = new NameAndDescr2( renameTitle, Utilities.UIConfigSettings.Colors.EditDialoglColor, renameHelpTag );

            dlg.ObjDescription = node.Model.descr;

            dlg.ObjName = node.Model.model_name;

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                // rename
                Node.Model.model_name = dlg.ObjName;
                Node.Model.descr = dlg.ObjDescription;

                // write to disk
                Node.Db.Update();

                if( Node.ActiveNode ) {
                    this.Refresh();
                }
                else {
                    Node.ParentNode.Control.Refresh();
                }
            }
        }

        private void viewResults()
        {
            // new empty database
            ResultsDb db = new ResultsDb();

            db.Connection = Node.Db.Connection;
            db.ModelID = node.Id;
            db.Refresh();
            ResultsForm dlg = new ResultsForm( MrktSim.DevlMode );

            dlg.Db = db;

            dlg.Show();
        }

        /// <summary>
        /// Creates a copy of the selected model
        /// </summary>
        private void copyModelButton_Click() {
         
            if(!checkModelStatus()) {
                return;
            }

            this.modelToCopy = this.node;

            ConfirmDialog cdlg = new ConfirmDialog( copyMsg1, copyMsg2, modelToCopy.ToString(), copyTitle );
            DialogResult rslt = cdlg.ShowDialog();

            if( rslt != DialogResult.Yes ) {
                return;
            }
            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this.statusControl );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( CopyModel );

            // update the status while the background thread runs
            string statusMsg = String.Format( copyStatus, modelToCopy.ToString() );
            status.UpdateUIAndContinue( copyModelButton_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void copyModelButton_Click_Part2() {
            Node.RefreshTree();

            if( Node.ActiveNode ) {
                this.Refresh();
            }
            else {
                Node.ParentNode.Control.Refresh();
            }

            this.Refresh();

            //???how do we  select the newly created (copied) item ???
            ////for( int i = 0; i < modelList.Items.Count; i++ ) {
            ////    // we can match the name since we know it wil be unique in this view
            ////    if( modelList.Items[ i ].ToString() == this.copiedModelName ) {
            ////        modelList.SelectedIndex = i;
            ////    }
            ////}

            ClearStatusDisplay();

            CompletionDialog dlg = new CompletionDialog( copyDoneMsg );
            dlg.ShowDialog();
        }

        private void CopyModel() {
            // new empty database
            ModelDb db = new ModelDb();
            db.Connection = Node.Db.Connection;
            db.ModelID = this.modelToCopy.Id;

            db.Refresh();

            // we make a copy
            // this makes all the records "new"
            // cannot find how to force this in an existing dataset
            ModelDb dbCopy = db.Copy();

            // make sure it is unique in this project
            string filter = "project_id = " + Model.project_id;
            this.copiedModelName = ModelDb.CreateUniqueName( Node.Db.Data.Model_info, "model_name", dbCopy.Model.model_name, filter );
            dbCopy.Model.model_name = this.copiedModelName;

            dbCopy.Update();

            Node.Db.Refresh();
        }
        
        /// <summary>
        /// Exports the current model to an XML (.msdb) file
        /// </summary>
        private void exportModelButton_Click() {
         
            if( !checkModelStatus() ) {
                return;
            }

            System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();

            saveFileDlg.DefaultExt = ".msdb";

            saveFileDlg.Filter = "Market Sim Model (*.msdb)|*.msdb";

            saveFileDlg.CheckFileExists = false;

            DialogResult rslt = saveFileDlg.ShowDialog();

            if( rslt != DialogResult.OK ) {
                return;
            }
            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            this.exportFileName = saveFileDlg.FileName;

            Status status = new Status( this.statusControl );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( ExportModel );

            // update the status while the background thread runs
            string statusMsg = String.Format( exportStatus, node.ToString() );
            status.UpdateUIAndContinue( exportModelButton_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void exportModelButton_Click_Part2() {
            ClearStatusDisplay();

            CompletionDialog dlg = new CompletionDialog( exportDoneMsg );
            dlg.ShowDialog();
        }

        private void ExportModel() {

            string tempFile = Application.StartupPath + "\\temp.msdb";

            // new empty database
            ModelDb db = new ModelDb();
            db.Connection = Node.Db.Connection;
            db.ModelID = node.Id;

            db.Refresh();

            db.Model.checkpoint_valid = false;
            db.Model.checkpoint_file = "NA";

            db.ResetPrimaryKeys();

           // db.Data.Namespace = "http://www.decisionpower.com/MrktSimDBSchema.xsd";

            db.Data.WriteXml( tempFile );           // the workhorse method

            FileCleaner cleaner = new FileCleaner();

            cleaner.clean( tempFile, this.exportFileName );
        }


        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
        }

        //private void button1_Click( object sender, EventArgs e ) {
        //    ClearCheckpoint();
        //}

        private ModelDb checlpointClearingDb;

        private void ClearCheckpoint(){
            if( this.ParentForm != null ) {
                ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately
            }
            
            // new empty database
            checlpointClearingDb = new ModelDb();
            checlpointClearingDb.Connection = Node.Db.Connection;
            checlpointClearingDb.ModelID = node.Id;

            Status status = new Status( this.statusControl );
            System.Threading.Thread backgroundThread = status.StartBackgroundThread( ClearCheckpointNow );

            // update the status while the background thread runs
            string statusMsg = String.Format( clearCheckpointStatus, Node.Model.model_name, Node.Model.checkpoint_date.ToString( "d/M/yy" ) );
            status.UpdateUIAndContinue( ClearCheckpoint_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void ClearCheckpointNow(){
            checlpointClearingDb.Refresh();
            checlpointClearingDb.Model.checkpoint_valid = false;
            checlpointClearingDb.Update();
        }

        private void ClearCheckpoint_Part2(){
            ClearStatusDisplay();      
            RefreshCheckpoint( true );
        }

        private void ClearStatusDisplay() {
            Status.ClearStatus( this );

            this.checkpointCheckBox.Enabled = true;
        }

        private void checkpointCheckBox_CheckedChanged( object sender, EventArgs e ) {
            if( checkpointCheckBox.Checked == false ) {
                checkpointCheckBox.CheckedChanged -=new EventHandler(checkpointCheckBox_CheckedChanged);
                // defer the un-checking until it is confirmed
                checkpointCheckBox.Checked = true;

                string msg = String.Format( clearCheckpointMessage, Node.Model.model_name, Node.Model.checkpoint_date.ToString( "d-MMM-yyyy" ) );
                ConfirmDialog cdlg = new ConfirmDialog( msg, clearCheckpointTitle, clearCheckpointHelpTag );
                cdlg.SetOkCancelButtonStyle();
                cdlg.Width += 90;
                cdlg.Height += 50;
                DialogResult resp = cdlg.ShowDialog();

                if( resp != DialogResult.OK ) {
                    // disallow the change
                    checkpointCheckBox.CheckedChanged +=new EventHandler(checkpointCheckBox_CheckedChanged);
                    return;
                }
                // now we can really change
                checkpointCheckBox.Checked = false;
                checkpointCheckBox.CheckedChanged += new EventHandler( checkpointCheckBox_CheckedChanged );

                ClearCheckpoint();
            }
            else {
                // un-do the checking of the box - just show the message of how a checkpoint should be re-established
                checkpointCheckBox.CheckedChanged -= new EventHandler( checkpointCheckBox_CheckedChanged );
                checkpointCheckBox.Checked = false;

                ConfirmDialog cdlg = new ConfirmDialog( establishCheckpointMessage, establishCheckpointTitle, establishCheckpointHelpTag );
                cdlg.Width += 80;
                cdlg.Height += 50;
                DialogResult resp = cdlg.ShowDialog();

                checkpointCheckBox.CheckedChanged += new EventHandler( checkpointCheckBox_CheckedChanged );
            }
        }

        /// <summary>
        /// Checks if model exists
        /// </summary>
        /// <returns> true if all is well</returns>
        private bool checkModelStatus() {

            if( node == null || node.Model == null || node.Db.ModelDeleted( node.Model.model_id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();
             
                return false;
            }

            return true;
        }


    }
}
