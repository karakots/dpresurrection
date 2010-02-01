using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Common.Dialogs;
using MrktSimDb;
using Utilities;


namespace MrktSimClient.Controls
{
    public partial class DbControl : UserControl
    {
        private string newItemString = "New Project...";
        private string deleteItemString = "Delete Project...";

        private string delMsg1 = "Permanently delete the selected project?\r\nAll models and results in the project wil also be deleted.";
        private string delMsg2 = "Project to Delete:";
        private string delTitle = "Confirm Project Delete";
        private string delDoneMsg = "Project Deleted Successfully";
        private string delStatus = "Deleting Project data ({0})...";

        private string createTitle = "Create New Project";
        private string createHelpTag = "CreateNewProject";
        private string createStatus = "Creating New Project...";

        private MsTopProjectNode projectToDelete;
        private NameAndDescr2 newDialog;

        public override void Refresh()
        {
            string nameToKeepSelected = null;      // preserve the selected item - use the name since it will be unique in this view
            if( projectList.SelectedIndex != -1 ) {
                nameToKeepSelected = projectList.SelectedItem.ToString();
            }

            projectList.Items.Clear();
            projectList.SelectedIndex = -1;

            // sort the names alphabetically
            string[] subnodeNames = new string[ node.Nodes.Count ];
            MsProjectNode[] subnodes = new MsProjectNode[ node.Nodes.Count ];
            for( int n = 0; n < node.Nodes.Count; n++ ) {
                subnodes[ n ] = (MsProjectNode)node.Nodes[ n ];
                subnodeNames[ n ] = ((MsProjectNode)node.Nodes[ n ]).ToString();
            }
            Array.Sort( subnodeNames, subnodes );
            for( int n = 0; n < subnodes.Length; n++ ) {
                projectList.Items.Add( subnodes[ n ] );
            }

            dbName.Text = node.ToString();
            dbName.Font = Utilities.UIConfigSettings.Fonts.NavPaneTitleFont;

            //re-select the same item that was selected before
            for( int i = 0; i < projectList.Items.Count; i++ ) {
                if( projectList.Items[ i ].ToString() == nameToKeepSelected ) {
                    projectList.SelectedIndex = i;
                    break;
                }
            }

            // handle this control becoming visible as a result of a "back" operation
            if( projectList.SelectedIndex < 0 && this.Tag != null && this.Tag is ProjectControl ) {
                int projIDToSelect = -1;
                if( ((ProjectControl)this.Tag).Project.RowState != DataRowState.Detached ){
                    projIDToSelect = ((ProjectControl)this.Tag).Project.id;
                }

                if( projIDToSelect != -1 ) {
                    foreach( MsProjectNode nitem in projectList.Items ) {
                        if( nitem.Id == projIDToSelect ) {
                            projectList.SelectedItem = nitem;
                            break;
                        }
                    }
                }
            }

            if( projectList.SelectedItem != null ) {
                ((MsProjectNode)projectList.SelectedItem).Control.Refresh();
            }
            else {
                // hide the project view since there is no selected project
                this.splitContainer1.Panel2.Controls.Clear();
                this.splitContainer1.Panel2.Controls.Add( this.bannerControl1 );
            }

            base.Refresh();
        }


        private MsDbNode node;
        public MsDbNode Node
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

        public Panel PopupMenuPanel {
            set { this.dbLink.PopupMenuPanel = value; }
        }
  
        public DbControl()
        {
            InitializeComponent();

            this.projectList.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.dbName.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.label1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;
            this.panel1.BackColor = Utilities.UIConfigSettings.Colors.MainNavigatorPanelColor;

            this.dbLink.AddItem( newItemString, newProject_Click );
            this.dbLink.AddItem( deleteItemString, deleteProject_Click );
            this.dbLink.DisableLinks( deleteItemString );

            this.dbLink.PopupMenuPanel = this.popupMenuPanel;

            this.dbLink.BeforeActivate += new PopupMenuLinkLabel.OnBeforeActivate( SetDbMenuItemEnabling );
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        private void newProject_Click()
        {
            newDialog = new NameAndDescr2( createTitle, Color.FromArgb( 192, 192, 255 ), createHelpTag );
            DialogResult rslt = newDialog.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                Status status = new Status( this );
                status.UpdateUIAndContinue( newProject_LinkClicked_Part2, createStatus, 50 );
            }
        }

        private void newProject_LinkClicked_Part2() {
            Status.SetWaitCursor( this );

            projectList.BeginUpdate();
            MrktSimDBSchema.projectRow proj = node.Db.CreateProject( newDialog.ObjName, newDialog.ObjDescription );

            node.Db.Update();

            node.RefreshTree();

            this.Refresh();

            // select the newly imported item
            for( int i = 0; i < projectList.Items.Count; i++ ) {
                // we can match the name since we know it wil be unique in this view
                if( projectList.Items[ i ].ToString() == proj.name ) {
                    projectList.SelectedIndex = i;
                }
            }

            Status.ClearStatus( this );
           
            projectList.EndUpdate();
        }

        /// <summary>
        /// Deletes the selected project
        /// </summary>
        private void deleteProject_Click() {
            MsTopProjectNode proj = (MsTopProjectNode)projectList.SelectedItem;
            if( proj == null ) {
                return;
            }

            if( node.Db.ProjectDeleted( proj.Id ) ) {
                ((MrktSim)this.ParentForm).ResetUI();
                return;
            }

            if( node.Db.ProjectLocked( proj.Id ) ) {
                MessageBox.Show( MrktSim.Message( "Project.Locked" ) );
                return;
            }

             if( node.Db.ProjectHasSimulationRunning( proj.Id ) ) {
                MessageBox.Show( MrktSim.Message( "Simulation.Queued" ) );
                return;
            }

            this.projectToDelete = proj;
            ConfirmDialog cdlg = new ConfirmDialog( delMsg1, delMsg2, projectToDelete.ToString(), delTitle );
            cdlg.Height += 30;
            DialogResult rslt = cdlg.ShowDialog();
            if( rslt != DialogResult.Yes ) {
                return;
            }
            ((MrktSim)this.ParentForm).ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );

            System.Threading.Thread backgroundThread = status.StartBackgroundThread( DeleteProject );

            // update the status while the background thread runs
            string statusMsg = String.Format( delStatus, projectToDelete.ToString() );
            status.UpdateUIAndContinue( deleteProject_Click_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void deleteProject_Click_Part2() {

            Status.SetWaitCursor( this );

            this.Refresh();
            Status.ClearStatus( this );

            CompletionDialog dlg = new CompletionDialog( delDoneMsg );
            dlg.ShowDialog();
            projectList.SelectedIndex = -1;
            this.Refresh();
        }

        private void DeleteProject() {
            projectToDelete.DeleteItem();
            node.Db.Update();
        }

        private void projectList_DoubleClick(object sender, EventArgs e)
        {
            goToProject();
        }

        private void goToProject()
        {
            MsTopProjectNode proj = (MsTopProjectNode)projectList.SelectedItem;

            if (proj != null)
            {
                if( node.Db.ProjectDeleted( proj.Id ) ) {
                    ((MrktSim)this.ParentForm).ResetUI();
                    return;
                }

                ((MrktSim)this.ParentForm).ActivateNode(proj);
            }
        }

        /// <summary>
        ///  Enables or disables the appropriate menu items.  
        /// </summary>
        /// <remarks>Invoked by the OnBeforeActivate event of the popup menu so the menu state is always up to date.</remarks>
        private void SetDbMenuItemEnabling() {
            bool disableNew = false;
            bool disableDelete = false;

            if( ((MrktSim)this.ParentForm).ProcessActive == true ) {
                disableNew = true;
                disableDelete = true;
            }

            if( projectList.SelectedIndex == -1 ) {
                disableDelete = true;
            }

            this.dbLink.EnableAllLinks();
            if( disableNew ) {
                this.dbLink.DisableLink( this.newItemString );
            }
            if( disableDelete == true ) {
                this.dbLink.DisableLink( this.deleteItemString );
            }
        }

        private void projectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();

            MsTopProjectNode proj = (MsTopProjectNode)projectList.SelectedItem;

            if( this.ParentForm != null ) {
                ((MrktSim)this.ParentForm).ActiveProjectNode = proj;
            }

            if (proj != null)
            {

                this.splitContainer1.Panel2.Controls.Add(proj.Control);

                proj.Control.Dock = DockStyle.Fill;
                proj.Control.Refresh();
            }
        }

        private void popupMenuPanel_Paint( object sender, PaintEventArgs e ) {
            Utilities.PopupMenuLinkLabel.PaintMenuPanelBackground( this.popupMenuPanel, e.Graphics, 50 );
        }
    }
}
