using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MrktSimDb;

using Common.Dialogs;
using MarketSimUtilities;
using Results;
using Utilities;

namespace MrktSimClient.Controls
{
    public partial class ProjectControl : UserControl
    {
        private string renameProjectString = "Rename Project";
        private string newItemString = "New Model...";
        private string importItemString = "Import Model...";
        private string exportItemString = "Export Model...";
        private string copyItemString = "Copy Model";
        private string deleteItemString = "Delete Model";

        private string createTitle = "Create New Model";
        private string createStatus = "Creating New Model...";
        private string createHelpTag = "CreateNewModel";

        private string copyMsg1 = "Copy the selected model?";
        private string copyMsg2 = "Model to Copy:";
        private string copyTitle = "Confirm Model Copy";
        private string copyDoneMsg = "Model Copied Successfully";
        private string copyStatus = "Copying Model data ({0})...";

        private string delMsg1 = "Permanently delete the selected model from the database?";
        private string delMsg2 = "Model to Delete:";
        private string delTitle = "Confirm Model Delete";
        private string delDoneMsg = "Model Deleted Successfully";
        private string delStatus = "Deleting Model data ({0})...";

        private string exportDoneMsg = "Model Exported Successfully";
        private string exportStatus = "Exporting Model data ({0})...";

        private string dateRealignMsg = "One or more Market Plan start/end dates have been adjusted to align with their associated data.";
        private string dateRealignTitle = "Dates Realigned";
        private bool realignedPlanOnLoading = false;

        private MsModelNode modelToDelete;
        private MsModelNode modelToCopy;
        private MsModelNode modelToExport;
        private NameAndDescr2 newDialog;
        private string exportFileName;
        private string importFileName;
        private string copiedModelName;

        public override void Refresh()
        {
            if( node.Db.ProjectDeleted( Node.Project.id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();
                return;
            }

            string nameToKeepSelected = null;      // preserve the selected item - use the name since it will be unique in this view
            if( modelList.SelectedIndex != -1 ) {
                nameToKeepSelected = modelList.SelectedItem.ToString();
            }

            modelList.Items.Clear();

            // show only the pertinent models
            for( int n = Node.Nodes.Count - 1; n >= 0; n-- ) {
                string info = ((MsModelNode)Node.Nodes[ n ]).Model.app_code;

                if(info != Database.AppCode)
                {
                    // only show NIMO models in NIMO
                        Node.Nodes[ n ].Remove();       //remove NIMO node from regular display
                }
            }

            // sort the names alphabetically
            string[] subnodeNames = new string[ Node.Nodes.Count ];
            MsModelNode[] subnodes = new MsModelNode[ Node.Nodes.Count ];
            for( int n = 0; n < Node.Nodes.Count; n++ ) {
                subnodes[ n ] = (MsModelNode)Node.Nodes[ n ];
                subnodeNames[ n ] = ((MsModelNode)Node.Nodes[ n ]).ToString();
            }
            Array.Sort( subnodeNames, subnodes );
            for( int n = 0; n < subnodes.Length; n++ ) {
                modelList.Items.Add( subnodes[ n ] );
            }



            projName.Text = Node.ToString();
            projName.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            descBox.Text = Node.Project.descr;

            //re-select the same item that was selected before
            for( int i = 0; i < modelList.Items.Count; i++ ) {
                if( modelList.Items[ i ].ToString() == nameToKeepSelected ) {
                    modelList.SelectedIndex = i;
                    break;
                }
            }

            // handle this control becoming visible as a result of a "back" operation
            if( modelList.SelectedIndex < 0 && this.Tag != null && this.Tag is ModelControl ) {
                int modelIDToSelect = -1;
                if( ((ModelControl)this.Tag).Model.RowState != DataRowState.Detached ){
                    modelIDToSelect = ((ModelControl)this.Tag).Model.model_id;
                }
 
                if( modelIDToSelect != -1 ) {
                    foreach( MsModelNode nitem in modelList.Items ) {
                        if( nitem.Id == modelIDToSelect ) {
                            modelList.SelectedItem = nitem;
                            break;
                        }
                    }
                }
            }

            if (modelList.SelectedItem != null)
            {
                ((MsModelNode) modelList.SelectedItem).Control.Refresh();
            }
            else {
                // hide the model view since there is no selected model
                this.splitContainer1.Panel2.Controls.Clear();
                this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
            }

            if (Node.ActiveNode)
            {
                expandLink.Visible = false;
            }
            else
            {
                expandLink.Visible = true;
            }

            base.Refresh();
        }

        private MsTopProjectNode node;
        public MsTopProjectNode Node
        {
            get
            {
                return node;
            }

            set
            {
                node = value;

                this.Refresh();
            }
        }

        public MrktSimDBSchema.projectRow Project
        {
            get
            {
                return Node.Project;
            }
        }

        public ProjectControl()
        {
            InitializeComponent();

            this.modelList.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.projName.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.label1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.panel1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.splitContainer1.Panel2.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;


            projectLink.AddItem( renameProjectString, renameProject_Click );
            projectLink.AddItem( newItemString, newModel_LinkClicked );
            projectLink.AddItem( importItemString, importModel_LinkClicked );
            projectLink.AddItem( exportItemString, exportModelButton_Click );
            projectLink.AddItem( copyItemString, copyModelButton_Click );
            projectLink.AddItem( deleteItemString, deleteModel_Click );

            projectLink.DisableLinks( deleteItemString, copyItemString, exportItemString );

            this.projectLink.PopupMenuPanel = this.popupMenuPanel;

            this.projectLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetProjectMenuItemEnabling );

        }

        /// <summary>
        /// Crete a brand-new model.  Exept for the name and description, all model values will be defaults.
        /// </summary>
        private void newModel_LinkClicked()
        {
            newDialog = new NameAndDescr2( createTitle, Color.FromArgb( 192, 192, 255 ), createHelpTag );
            DialogResult rslt = newDialog.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                Status status = new Status( this );
                status.UpdateUIAndContinue( newModel_LinkClicked_Part2, createStatus, 50 );
            }
        }

        private void newModel_LinkClicked_Part2() {

            // need to refresh first
            MrktSimDb.MrktSimDBSchema.Model_infoRow model = Node.Db.CreateModel( Node.Project, newDialog.ObjName, newDialog.ObjDescription );

            // writes data to disk
            Node.Db.Update();

            if (Database.Nimo)
            {
                // create brand and product types
                Node.Db.CreateProductType(model.model_id, "Brand");
                Node.Db.CreateProductType(model.model_id, "Product");
            }

            Node.RefreshTree();

            this.Refresh();

            // select the newly imported item
            for( int i = 0; i < modelList.Items.Count; i++ ) {
                // we can match the name since we know it wil be unique in this view
                if( modelList.Items[ i ].ToString() == model.model_name ) {
                    modelList.SelectedIndex = i;
                }
            }

            Status.ClearStatus( this );
        }

        private string importErrorMessage = null;
        private ModelDb importDb = null;

        /// <summary>
        /// Imports a new model from an XML (.msdb) file
        /// </summary>
        private void importModel_LinkClicked() {
            System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.Filter = "Market Sim Model (*.msdb)|*.msdb";

            openFileDlg.CheckFileExists = false;

            openFileDlg.ReadOnlyChecked = false;

            DialogResult rslt = openFileDlg.ShowDialog();

            if( rslt != DialogResult.OK ) {
                return;
            }

            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately
            
            this.importFileName = openFileDlg.FileName;
            string shortFileName = this.importFileName;
            if( shortFileName.IndexOf( "\\" ) != -1 ) {
                shortFileName = shortFileName.Substring( shortFileName.LastIndexOf( "\\" ) + 1 );
                if( shortFileName.IndexOf( "." ) != -1 ) {
                    shortFileName = shortFileName.Substring( 0, shortFileName.LastIndexOf( "." ) );
                }
            }

            // refresh the database in a background thread
            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( ModelFromXml );

            // update the status while the background thread runs
            string statusMsg = String.Format( "Reading Model data ({0})...", shortFileName );

            status.UpdateUIAndContinue( importModel_LinkClicked_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void importModel_LinkClicked_Part2() {

            Status status = new Status( this );

            if( importDb == null ) {
                MessageBox.Show( this.ParentForm, importErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );

                Status.ClearStatus( this );
                return;
            }

            realignedPlanOnLoading = importDb.AllignPlansWithData();

            System.Threading.Thread backgroundThread = status.StartBackgroundThread(WriteModelToDb);

            string statusMsg = String.Format(" Writing to database: ({0})...", importDb.Model.model_name);

            Status.ClearWaitCursor( this );

            status.UpdateUIAndContinue(importModel_LinkClicked_Part2A, backgroundThread, 200, statusMsg, 1, 60, "Updating database...");


            Status.ClearStatus( this );
        }

        private void importModel_LinkClicked_Part2A()
        {
            Node.Db.Refresh();
            Node.RefreshTree();

            this.Refresh();

            Status.ClearStatus( this );

            if( realignedPlanOnLoading == true ) {
                MessageBox.Show( dateRealignMsg, dateRealignTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
            }
        }

        private void WriteModelToDb()
        {
            ModelDb newdb = new ModelDb();

            this.importDb.Update();
        }

        private void convertDB(ModelDb import)
        {
            // we do this always
            // This makes sure the primary keys in database are negative
            importDb.ResetPrimaryKeys();


            DataRow[] versionRows = import.Data.db_schema_info.Select();

            if( versionRows.Length != 1 )
            {
                return;
            }

            MrktSimDBSchema.db_schema_infoRow version = (MrktSimDBSchema.db_schema_infoRow)versionRows[0];

            // No conversion prior to version 2.5
            if( version.major_no <= 2 && version.minor_no < 5 )
            {
                // set pac_size to default for products
                foreach( MrktSimDBSchema.productRow prod in import.Data.product.Select() )
                {
                    prod.pack_size_id = Database.AllID;
                }

                // create default pack size
                import.CreateDefaultPackSize( (MrktSimDBSchema.Model_infoRow) import.Data.Model_info.Rows[0] );

                version.minor_no = 2;
                version.minor_no = 5;
            }

            // For future releases
            //if (version.major_no <= conversion_major_no && version.minor_no < conversion_minor no)
            //{
            //    // perform converion
            //}
            //
            // and so on and so forth
            // conversion listed chronologically

        }

        private void ModelFromXml( ) {

            importErrorMessage = null;
            this.importDb = new ModelDb();
            importDb.Data.EnforceConstraints = false;

            XmlReadMode mode = XmlReadMode.Auto;

            // importDb.Data.Namespace = "http://www.decisionpower.com/MrktSimDBSchema.xsd";

            try {
                // db.Data.db_schema_info.ReadXml(importFileName);
                // here is where we can figure out if we need to convert
                // db.Data.db_schema_info.Clear();


                mode = importDb.Data.ReadXml( importFileName);
               
            }
            catch (Exception e) {
                importDb = null;
                importErrorMessage = "Error reading file: " + e.Message + 
                    "\r\n Read mode: " + mode.ToString() +
                    " \r\n Displaying top of file... \r\n";

                System.IO.StreamReader reader = new System.IO.StreamReader( importFileName );

                for( int ii = 0; ii < 6; ii++ ) {
                    importErrorMessage += "\r\n " + reader.ReadLine();
                }

                reader.Close();

                return;
            }


            if( importDb.Data.Model_info.Rows.Count == 0 ) {
                importErrorMessage = "File is out of date or damaged Error: No data read";
                importDb = null;
                return;
            }

            convertDB( importDb );

            importDb.ModelID = (int)importDb.Data.Model_info.Rows[ 0 ][ "model_id" ];

            importDb.Connection = Node.Db.Connection;

            // create new name for model
            string modelName = importDb.Model.model_name;

            // get a unique name
            string filter = "project_id = " + Node.Project.id;
            string newModelName = ModelDb.CreateUniqueName( node.Db.Data.Model_info, "model_name", modelName, filter );

            importDb.Model.model_name = newModelName;

            // we need to switch project on the model
           

            MrktSimDBSchema.projectRow currentProj = importDb.Model.projectRow;

            importDb.Model.project_id = Node.Project.id;
            currentProj.id = importDb.Model.project_id;


            try
            {
                importDb.Data.EnforceConstraints = true;
            }
            catch( Exception constraintError )
            {
                importErrorMessage = "Cound not convert model: " + constraintError.Message;
                importDb = null;
                return;
            }

            // we do not want to write the project to db.
            importDb.Data.project.AcceptChanges();

            importDb.Model.locked = false;
            importDb.Model.read_only = false;
            importDb.ReadOnly = false;
        }

        private void goToModel()
        {
            if( !checkModelStatus() ) {
                return;
            }

            MsModelNode model = (MsModelNode)modelList.SelectedItem;

            if (model != null)
            {
                ((MrktSim)this.ParentForm).ActivateNode(model);
            }

        }

        private void modelList_DoubleClick(object sender, EventArgs e)
        {
            goToModel();
        }

        /// <summary>
        /// Creates a copy of the selected model
        /// </summary>
        private void copyModelButton_Click()
        {
            if( !checkModelStatus() ) {
                return;
            }

            MsModelNode model = (MsModelNode)modelList.SelectedItem;

            if( model == null ) {
                return;
            }

            this.modelToCopy = model;

            ConfirmDialog cdlg = new ConfirmDialog( copyMsg1, copyMsg2, modelToCopy.ToString(), copyTitle );
            DialogResult rslt = cdlg.ShowDialog();

            if( rslt != DialogResult.Yes ) {
                return;
            }
            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( CopyModel );

            // update the status while the background thread runs
            string statusMsg = String.Format( copyStatus, modelToCopy.ToString() );
            status.UpdateUIAndContinue( copyModelButton_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void copyModelButton_Click_Part2()
        {

            Utilities.Status.SetWaitCursor( this );

            Node.RefreshTree();

            this.Refresh();

            // select the newly created (copied) item
            for( int i = 0; i < modelList.Items.Count; i++ ) {
                // we can match the name since we know it wil be unique in this view
                if( modelList.Items[ i ].ToString() == this.copiedModelName ) {
                    modelList.SelectedIndex = i;
                }
            }

            Status.ClearStatus( this );

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
            string filter = "project_id = " + Node.Project.id;
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

            MsModelNode model = (MsModelNode)modelList.SelectedItem;

            if( model == null ) {
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

            this.modelToExport = model;
            this.exportFileName = saveFileDlg.FileName;

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( ExportModel );

            // update the status while the background thread runs
            string statusMsg = String.Format( exportStatus, modelToExport.ToString() );
            status.UpdateUIAndContinue( exportModelButton_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void exportModelButton_Click_Part2()
        {
            Status.ClearStatus( this );

            CompletionDialog dlg = new CompletionDialog( exportDoneMsg );
            dlg.ShowDialog();
        }

        private void ExportModel() {
            string tempFile = Application.StartupPath + "\\temp.msdb";

            // new empty database
            ModelDb db = new ModelDb();
            db.Connection = Node.Db.Connection;
            db.ModelID = this.modelToExport.Id;

            db.Refresh();

            db.Model.checkpoint_valid = false;
            db.Model.checkpoint_file = "NA";

            db.ResetPrimaryKeys();

            // db.Data.Namespace = "http://www.decisionpower.com/MrktSimDBSchema.xsd";

            db.Data.WriteXml( tempFile );           // the workhorse method

            FileCleaner cleaner = new FileCleaner();

            cleaner.clean( tempFile, this.exportFileName );
        }

        /// <summary>
        ///  Enables or disables the appropriate menu items.  
        /// </summary>
        /// <remarks>Invoked by the OnBeforeActivate event of the popup menu so the menu state is always up to date.</remarks>
        private void SetProjectMenuItemEnabling() {
            bool disableNew = false;
            bool disableImport = false;
            bool disableExport = false;
            bool disableCopy = false;
            bool disableDelete = false;

            if( ((MrktSim)this.ParentForm).ProcessActive == true ) {
                disableNew = true;
                disableImport = true;
                disableExport = true;
                disableCopy = true;
                disableDelete = true;
            }

            if( modelList.SelectedItem == null ) {
                disableExport = true;
                disableCopy = true;
                disableDelete = true;
            }

            this.projectLink.EnableAllLinks();
            if( disableNew ) {
                this.projectLink.DisableLink( this.newItemString );
            }
            if( disableImport == true ) {
                this.projectLink.DisableLink( this.importItemString );
            }
            if( disableExport == true ) {
                this.projectLink.DisableLink( this.exportItemString );
            }
            if( disableCopy == true ) {
                this.projectLink.DisableLink( this.copyItemString );
            }
            if( disableDelete == true ) {
                this.projectLink.DisableLink( this.deleteItemString );
            }
        }

        private void modelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();

            MsModelNode mod = (MsModelNode) modelList.SelectedItem;

            if (this.ParentForm != null && mod != null)
            {
                ((MrktSim)this.ParentForm).ActiveModelNode = mod;
            }

            if( mod != null )
            {
               
                this.splitContainer1.Panel2.Controls.Add(mod.Control);

                mod.Control.Dock = DockStyle.Fill;
                mod.Control.Refresh();
            }
        }

        private void projName_DoubleClick(object sender, EventArgs e)
        {
            ((MrktSim)this.ParentForm).ActivateNode(Node);
        }

        /// <summary>
        /// Deletes the currently selected nodel.
        /// </summary>
        private void deleteModel_Click() {

            if(!checkModelStatus()) {
                return;
            }

            MsModelNode model = (MsModelNode)modelList.SelectedItem;

            if( model == null ) {
                return;
            }

            if( node.Db.ModelDeleted( model.Model.model_id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();
                return;
            }

            string who = null;
            if( node.Db.ModelLocked( model.Model.model_id, out who ) ) {
                MessageBox.Show(MrktSim.Message( "Model.Locked by user: " + who ));
                return;
            }

            if( node.Db.ModelHasSimulationRunning( model.Model.model_id ) ) {
                MessageBox.Show(MrktSim.Message( "Simulation.Queued" ));
                return;
            }

            this.modelToDelete = model;

            ConfirmDialog cdlg = new ConfirmDialog( delMsg1, delMsg2, modelToDelete.ToString(), delTitle );
            DialogResult rslt = cdlg.ShowDialog();

            if( rslt != DialogResult.Yes ) {
                return;
            }
            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteModel );

            // update the status while the background thread runs
            string statusMsg = String.Format( delStatus, modelToDelete.ToString() );
            status.UpdateUIAndContinue( deleteModel_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void deleteModel_Click_Part2()
        {
            Status.ClearStatus( this );

            CompletionDialog dlg = new CompletionDialog( delDoneMsg );
            dlg.ShowDialog();
            modelList.SelectedIndex = -1;
            Node.Refresh();
            this.Refresh();
        }

        private void DeleteModel() {
            modelToDelete.DeleteItem();
            Node.Db.Update();
        }

        private void renameProject_Click()
        {
            MrktSimDBSchema.projectRow proj = Node.Project;

            if (proj != null)
            {
                if( node.Db.ProjectDeleted( Node.Project.id ) ) {
                    ((MrktSim)this.ParentForm).ResetUI();
                    return;
                }

                NameAndDescr dlg = new NameAndDescr();
                dlg.Type = "Project";
                dlg.ObjName = proj.name;
                dlg.ObjDescription = proj.descr;
                dlg.Editing = true;

                DialogResult rslt = dlg.ShowDialog();

                if (rslt == DialogResult.OK)
                {

                    proj.name = dlg.ObjName;

                    proj.descr = dlg.ObjDescription;

                    Node.Db.Update();

                    if (Node.ActiveNode)
                        this.Refresh();
                    else
                        Node.ParentNode.Control.Refresh();
                }
            }

        }

        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
        }

        private bool checkModelStatus() {

            MsModelNode model = (MsModelNode)modelList.SelectedItem;

            if( model != null && node.Db.ModelDeleted( model.Model.model_id ) ) {


                ((MrktSim)this.ParentForm).ResetUI();

                return false;
            }


            return true;
        }
    }
}
