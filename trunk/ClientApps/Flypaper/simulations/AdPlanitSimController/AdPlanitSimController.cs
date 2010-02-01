using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;


using DpSimQueue;
using MediaLibrary;
using HouseholdLibrary;
using GeoLibrary;
using Calibration;
using FlySim;

namespace AdPlanitSimController
{
    public partial class AdPlanitSimController : Form
    {
        private delegate void UpdateDelegate( string message );
        private UpdateDelegate UpdateMessage;

        private string defaultName = "AdPlanit Simulation Controller";
        private string SettingsFileName = @"\SimulationSettings.xml";

        public AdPlanitSimController()
        {
            InitializeComponent();

            
            dp = new DpUpdate( Properties.Settings.Default.simConnection );

            simGrid.DataError += new DataGridViewDataErrorEventHandler( simGrid_DataError );

            simGrid.DataSource = queue.sim_queue;

            simdir = Properties.Settings.Default.simdir;

            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            globalSerializer = new XmlSerializer( typeof( GlobalData ) );

            loadData();
            clock.Tick += new EventHandler( clock_Tick );

            clock.Interval = (int)(1000);

            clock.Start();

            UpdateMessage = new UpdateDelegate( updateMessage );


            this.Text = dp.Database + " ::  " + Application.StartupPath;

            DirectoryInfo di = new DirectoryInfo( simdir );

            if( !di.Exists )
            {
                this.mediaStatus.Text = "Directory " + simdir + " does not exist. Please fix configuration file";

                this.dataToolStripMenuItem.Enabled = false;
            }

            loadSimulationSettingsItem_Click( null, null );

            ignoreUserBox.Text = DpUpdate.Calibrator;

        }

        private void updateMessage( string message )
        {
            this.agentStatus.Text = message;

            if( message == doneAgentLoad )
            {

                dataToolStripMenuItem.Enabled = true;

                if( ignoreUserBox.Text != "?" )
                {
                    startNewSimToolStripMenuItem_Click( null, null );
                }
            }
        }

        void simGrid_DataError( object sender, DataGridViewDataErrorEventArgs e )
        {
        }

        #region private fields

        // the data set
        DpSimQueue.SimQueue queue = new SimQueue();

        private DpUpdate dp;

        private System.Windows.Forms.Timer clock = new System.Windows.Forms.Timer();

        #endregion

        #region basic control
        void clock_Tick( object sender, EventArgs e )
        {
            SimControlRefresh();
        }

        #endregion

        #region Simulation Data

        XmlSerializer globalSerializer;
        IFormatter formatter; 
        string simdir = null;

        List<Agent> agents = null;

        GlobalData global = new GlobalData();

        #endregion

        private void loadData()
        {
            dp.Refresh( queue.sim_queue );

            queue.AcceptChanges();
        }

        private void SimControlRefresh()
        {
            try
            {
                if( queue.HasChanges() )
                {
                    dp.Update( queue.sim_queue );
                    queue.AcceptChanges();
                }

                dp.RefreshSinceLast( queue.sim_queue );
            }
            catch( Exception ) { }
        }
  
        private void loadMediaToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if( MediaSim.ad_options != null )
            {
                DialogResult rslt = MessageBox.Show( "Ad Options exists: Do you wish to write over current set?", "Overwrite Ad Options", MessageBoxButtons.YesNo );

                if( rslt == DialogResult.No )
                {
                    return;
                }
            }

            this.Refresh();

            load_media();
        }

        private void load_media()
        {

            // ensure geo regions are read in
            GeoRegion.ReadFromFile(simdir);

            string fileName = simdir + @"\options.dat";


            FileStream file = new FileStream( fileName, FileMode.Open );


            MediaSim.ad_options = (Dictionary<int,AdOption>)formatter.Deserialize( file );
            file.Close();

            if (agents == null)
            {
                this.agentStatus.Text = "Load Agents";
            }
        }

        private void loadAgentsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            DirectoryInfo di = new DirectoryInfo( simdir );

            if(!di.Exists )
            {
                MessageBox.Show( "directory " + simdir + " does not exist" );
                return;
            }


            if( agents != null )
            {
                DialogResult rslt = MessageBox.Show( "Agents exists: Do you wish to write over current agent set?", "Overwrite Agents", MessageBoxButtons.YesNo );

                if( rslt == DialogResult.No )
                {
                    return;
                }
            }


            dataToolStripMenuItem.Enabled = false;

            System.Threading.Thread buildAgentThread = new Thread( read_agents );

            buildAgentThread.Start();
        }

        private string doneAgentLoad = "Done loading Agents";

        private void read_agents()
        {
            agents = new List<Agent>();

            agents.Clear();

            List<Agent> tmp = null;

            int fileNum = 0;
            bool done = false;
            while( !done  )
            {
                FileInfo fi = new FileInfo( simdir + @"\households\agent" + fileNum.ToString() + ".dat" );

                if( fi.Exists )
                {
                    this.Invoke( UpdateMessage, new object[] { "loading " +  fi.FullName } );

                    FileStream file = new FileStream( fi.FullName, FileMode.Open );
                    tmp = (List<Agent>)formatter.Deserialize( file );
                    file.Close();

                    agents.AddRange( tmp );

                    ++fileNum;
                }
                else
                {
                    done = true;
                }
            }

            this.Invoke( UpdateMessage, new object[] { doneAgentLoad} );
        }

        private void startNewSimToolStripMenuItem_Click( object sender, EventArgs e )
        {
            AdPlanitSim sim = new AdPlanitSim( agents, Properties.Settings.Default.simConnection, this.global, false );

            sim.MdiParent = this;

            sim.Show();
        }

        private void arrangeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.LayoutMdi( MdiLayout.TileVertical );
        }

        private void loadSimulationSettingsItem_Click( object sender, EventArgs e )
        {
            FileInfo fi = new FileInfo( simdir + SettingsFileName );

            if( !fi.Exists )
            {
                this.mediaStatus.Text = "Simulation Settings file does not exist.   Using defaults.";
                return;

            }

            mediaStatus.Text = "load media";
            agentStatus.Text = "load agents";

            SimulationSettingsItem.Checked = true;

            TextReader r = null;
            try
            {

                r = new StreamReader( simdir + SettingsFileName );
                global = (GlobalData)globalSerializer.Deserialize( r );
            }
            catch( Exception err )
            {
                MessageBox.Show( err.Message );

                this.mediaStatus.Text = "Error loading settings";
                SimulationSettingsItem.Checked = false;
            }

            if( r != null )
            {
                r.Close();
            }
        }

      
        private void createTemplateFileToolStripMenuItem_Click( object sender, EventArgs e )
        {
            TextWriter w = new StreamWriter( simdir + SettingsFileName );
            globalSerializer.Serialize( w, this.global );
            w.Close();
        }

        private void loadAllItem_Click( object sender, EventArgs e )
        {
            this.loadSimulationSettingsItem_Click( sender, e );
            this.loadMediaToolStripMenuItem_Click( sender, e );
            this.loadAgentsToolStripMenuItem_Click( sender, e );
        }

        private void simGlobalsMenu_Click( object sender, EventArgs e )
        {
            foreach( Form form in this.MdiChildren )
            {
                if( form.Name == "SimulationGlobals" )
                {
                    this.ActivateMdiChild( form );
                    return;
                }
            }

            SimulationGlobals dlg = new SimulationGlobals( global );

            dlg.MdiParent = this;

            dlg.Show();
        }

        private void editCalibratorToolStripMenuItem_Click( object sender, EventArgs e )
        {

            if( ignoreUserBox.ReadOnly )
            {
                ignoreUserBox.ReadOnly = false;
                editCalibratorToolStripMenuItem.Text = "Apply >";
                ignoreUserBox.BackColor = Color.White;
            }
            else
            {
                ignoreUserBox.ReadOnly = true;
                editCalibratorToolStripMenuItem.Text = "Edit >";
                DpUpdate.Calibrator = ignoreUserBox.Text;

                ignoreUserBox.BackColor = Color.LightGreen;
            }
                
        }

        private void startNewCalibrationToolStripMenuItem_Click( object sender, EventArgs e )
        {
            AdPlanitSim sim = new AdPlanitSim( agents, Properties.Settings.Default.simConnection, this.global, true );

            sim.MdiParent = this;

            sim.Show();
        }

        private void requeueButton_Click( object sender, EventArgs e )
        {
           DataGridViewSelectedRowCollection selRows = simGrid.SelectedRows;

           if( selRows.Count > 0 )
            {
                DataGridViewRow row = selRows[0];

                SimQueue.sim_queueRow sim = (SimQueue.sim_queueRow)((DataRowView)row.DataBoundItem).Row;

                sim.state = 0;

                SimControlRefresh();
            }

        }

        private void calibrateAdOptionsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string fileName = simdir + @"\vehicles.dat";

            //CalibrateOptions options = new CalibrateOptions( this.media, fileName );

            //options.MdiParent = this;

            //options.Show();
        }
    }
}
