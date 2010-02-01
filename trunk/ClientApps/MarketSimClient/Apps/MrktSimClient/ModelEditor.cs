using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MarketSimUtilities.MsTree;
using MarketSimSettings;
using Utilities;

using ModelView.Dialogs;

namespace MrktSimClient
{
    public partial class ModelEditor : Form, StatusShower
    {
        public ModelDb theDb;
        private bool processActive = false;

        public event EventHandler DatabaseChanged;        //??? is this ever used/needed ???

        private static int waitCursorLevel = 0;

        public bool ProcessActive {
            set { processActive = value; }
            get { return processActive; }
        }

        public ModelEditor() {
            InitializeComponent();

            // create models root node
            MsTopModelNode modelNode = new MsTopModelNode();
            ArrayList rootNodes = new ArrayList();
            rootNodes.Add( modelNode );
            this.modelViewControl1.SetContextNodes( rootNodes );

            int wid = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Width;
            int ht = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Height;
            int x = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.X;
            int y = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Y;
            if( x <= 0 && y <= 0 ) {
                x = 96;                     // set reasonable defaults for the first run -- fit into a 1024x768 screen!
                y = 118;
                wid = 927;
                ht = 605;
            }
            this.Location = new Point( x, y );
            if( wid >= this.MinimumSize.Width && ht >= this.MinimumSize.Height &&
                ((x + wid) < SystemInformation.WorkingArea.Width) &&
                ((y + ht) < SystemInformation.WorkingArea.Height) ) {
                this.Size = new Size( wid, ht );
            }
            else {
                // saved bounds won't fit on the screen!
                this.Location = new Point( 30, 30 );
                this.Size = this.MinimumSize;
            }

            this.SizeChanged += new System.EventHandler( this.ModelEditor_SizeChanged );
            this.LocationChanged += new System.EventHandler( this.ModelEditor_LocationChanged );
        }

        public ModelDb Db {
            set {
                theDb = value;

                this.Text = theDb.Model.model_name;

                if( theDb.ReadOnly ) {
                    this.Text += " (Read Only)";
                }

                this.Text += " - DecisionPower MarketSim Model Editor";

                this.marketPlanControl21.Db = theDb;
                this.marketPlanControl21.Init( theDb );
                this.modelViewControl1.Db = theDb;
            }
            get { return theDb; }
        }

        private void UpdateBoundsSettings() {
            //int menuHeight = SystemInformation.MenuHeight;
            //Rectangle newBounds = new Rectangle( this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height - menuHeight );
            Rectangle newBounds = new Rectangle( this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height );

            if( this.WindowState == FormWindowState.Normal ) {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds = newBounds;
            }
            Settings<MrktSim.ClientSettings>.Value.ModelFiltersPanelWidth = this.marketPlanControl21.MPFiltersPanelWidth;
            Settings<MrktSim.ClientSettings>.Value.ModelScenarioPanelHeight = this.marketPlanControl21.MPScenarioPanelHeight;
            Settings<MrktSim.ClientSettings>.Value.ModelPlansPanelHight = this.marketPlanControl21.MPMarketPlansPanelHeight;
            Settings<MrktSim.ClientSettings>.Value.ModelToplLevelPlansPanelWidth = this.marketPlanControl21.MPTopLevelPlansPanelWidth;
        }

        private void ModelEditor_FormClosing( object sender, FormClosingEventArgs e ) {
            // give the user a chance to save if changes were made
            if( !checkDataQueryUser() ) {
                e.Cancel = true;
            }
            else {
                if( theDb != null )
                    theDb.Close();
            }

            // proceed to close the window
            UpdateBoundsSettings();

            Settings<MrktSim.ClientSettings>.Value.ModelEditorVisibleTab = this.topTabControl.SelectedIndex;

            if( this.WindowState == FormWindowState.Maximized ) {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized = true;
            }
            else {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized = false;
            }
        }

        private void ModelEditor_LocationChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void ModelEditor_SizeChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void ModelEditor_Load( object sender, EventArgs e ) {
            if( Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized == true ) {

                this.LocationChanged -=new EventHandler(ModelEditor_LocationChanged);
                this.SizeChanged -= new EventHandler(ModelEditor_SizeChanged);

                this.WindowState = FormWindowState.Maximized;

                this.LocationChanged += new EventHandler( ModelEditor_LocationChanged );
                this.SizeChanged += new EventHandler( ModelEditor_SizeChanged );
            }

            int tab = Settings<MrktSim.ClientSettings>.Value.ModelEditorVisibleTab;
            if( tab >= 0 && tab < topTabControl.TabCount ) {
                this.topTabControl.SelectedIndex = tab;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripSaveButton_Click( object sender, EventArgs e ) {
            SaveModel();

            this.marketPlanControl21.Refresh();
        }

        private void toolStripRefreshButton_Click( object sender, EventArgs e ) {
            readFromDatabase();
        }

        private void toolStripOptionsButton_Click( object sender, EventArgs e ) {

            // show the model or market plans options dialog, as appropriate
            if( this.topTabControl.SelectedIndex == 0 ) {
                // model view is visible
                ModelOptionsDialog dlg = new ModelOptionsDialog( this.Db );

                DialogResult rslt = dlg.ShowDialog();

                if( rslt == DialogResult.Cancel ) {
                    return;
                }

                update_nodes();
            }
            else {
                //market plan is visible
                MarketPlanOptionsDialog dlg = new MarketPlanOptionsDialog( this.Db );

                DialogResult rslt = dlg.ShowDialog();

                if( rslt == DialogResult.Cancel ) {
                    return;
                }

                update_nodes();
            }
        }

        public void update_nodes() {
            this.SuspendLayout();

            foreach( MsControlNode topModelNode in modelViewControl1.TopModelNodes ) {
                topModelNode.Reset();
            }
            this.marketPlanControl21.Reset();

            this.ResumeLayout();
        }


        private bool checkDataQueryUser() {
            bool rval = true;

            if( HasChanges() ) {
                string msg = "Do you wish to save " + theDb.Model.model_name + "?";
                string caption = "Closing " + theDb.Model.model_name;

                DialogResult res = MessageBox.Show( this, msg, "Saving Model", MessageBoxButtons.YesNoCancel );

                if( res == DialogResult.Cancel ) {
                    rval = false;
                }
                else if( res == DialogResult.Yes ) {
                    // save model - maybe - final checks done here
                    return SaveModel();
                }
            }

            return rval;
        }

        public bool SaveModel() {

            if( theDb.SimulationRunning() ) {
                DialogResult res = MessageBox.Show(this, "There appear to be simulations running - are you sure you want to save the model?", "Save Model?", MessageBoxButtons.YesNo );

                if( res == DialogResult.No ) {
                    return false;
                }
            }

            Cursor temp = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            theDb.AllignPlansWithData();

            if( this.topTabControl.SelectedIndex == 0 ) {
                // model is visible
                this.modelViewControl1.CurrentControl.Flush();

                foreach( MsControlNode topModelNode in modelViewControl1.TopModelNodes ) {
                    topModelNode.Suspend = true;
                }
            }
            else {
                // market plans are visible
                this.marketPlanControl21.Flush();
                this.marketPlanControl21.Suspend = true;
                this.marketPlanControl21.SuspendDataGrids( true );
            }

            theDb.Update();

            if( this.topTabControl.SelectedIndex == 0 ) {
                // model is visible
                foreach( MsControlNode topModelNode in modelViewControl1.TopModelNodes ) {
                    topModelNode.Suspend = false;
                    topModelNode.DbUpdate();
                }
            }
            else {
                // market plans are visible
                this.marketPlanControl21.SuspendDataGrids( false );
                this.marketPlanControl21.Suspend = false;
                this.marketPlanControl21.DbUpate();
            }

            if( DatabaseChanged != null ) {
                DatabaseChanged( this, new System.EventArgs() );
            }

            this.Cursor = temp;

            return true;
        }

        public bool HasChanges() {
            this.modelViewControl1.CurrentControl.Flush();
            this.marketPlanControl21.Flush();

            return theDb.HasChanges();
        }

        /// <summary>
        /// Loads the current model from the database.
        /// </summary>
        private void readFromDatabase() {
            foreach( MsControlNode topModelNode in this.modelViewControl1.TopModelNodes ) {
                topModelNode.Suspend = true;
                this.marketPlanControl21.Suspend = true;
            }

            theDb.Refresh();

            foreach( MsControlNode topModelNode in this.modelViewControl1.TopModelNodes ) {
                topModelNode.Suspend = false;
                this.marketPlanControl21.Suspend = false;
                this.marketPlanControl21.Reset();
                topModelNode.Reset();
            }
        }

        private void topTabControl_SelectedIndexChanged( object sender, EventArgs e ) {
            if( topTabControl.SelectedIndex == 0 ) {
                // model is visible

                // disable market plans
                // market plans are visible
                this.marketPlanControl21.Flush();
                this.marketPlanControl21.Suspend = true;
                this.marketPlanControl21.SuspendDataGrids( true );

                // turn on grids
                foreach( MsControlNode topModelNode in modelViewControl1.TopModelNodes ) {
                    topModelNode.Suspend = false;
                }

                this.modelViewControl1.CurrentControl.Refresh();

            }
            else {
                //market plans are visible

                // diable model
                this.modelViewControl1.CurrentControl.Flush();

                foreach( MsControlNode topModelNode in modelViewControl1.TopModelNodes ) {
                    topModelNode.Suspend = true;
                }


                // turn on plans
               
                this.marketPlanControl21.SuspendDataGrids( false );
                this.marketPlanControl21.Suspend = false;
            }

        }

        #region StatusShower Members

        public int WaitCursorLevel {
            set {
                ModelEditor.waitCursorLevel = value;
            }
            get {
                return ModelEditor.waitCursorLevel;
            }
        }

        public void SetStatus( string txt, int percent ) {
            this.statusLabel.Text = txt;
            this.progressBar.Value = percent;
            if( percent > 0 ) {
                this.processActive = true;          // set the flag indicating that a background process is actfive (most menus s/b disabled if it is true)
                this.progressBar.Visible = true;
            }
            else {
                this.processActive = false;
                this.progressBar.Visible = false;
            }
        }

        public void ClearStatus() {
            this.progressBar.Value = 0;
            this.statusLabel.Text = "";
            this.progressBar.Visible = false;
            this.processActive = false;
        }
        #endregion

        private void toolStripHelpButton_Click( object sender, EventArgs e ) {

            if( topTabControl.SelectedIndex == 0 )
            {
                HelpManager.ShowHelp( this, this.modelViewControl1.CurrentControl.Name );
            }
            else
            {
                HelpManager.ShowHelp( this, "MarketSim-6-1.html" );
            }

           
        }
    }
}