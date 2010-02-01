using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;

using MediaLibrary;
using MediaManager;
using DemographicLibrary;
using PopulationGenerator;
using GeoLibrary;
using HouseholdLibrary;

namespace HouseholdManager
{
    public partial class AgentUpdate : UserControl
    {
        public HouseholdManagerUI.UpdateDelegate UpdateStatus;


        public AgentUpdate()
        {
            InitializeComponent();

            rand = new Random();

            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


            HH_dir = Properties.Settings.Default.hhinput_directory + @"\households";
            Penetration_dir = Properties.Settings.Default.rawdata_directory + @"\media\penetration";


            foreach( string name in Enum.GetNames( typeof( MediaLibrary.MediaVehicle.MediaType ) ) )
            {
                mediaTypeBox.Items.Add( name );
            }

            mediaDb = null;
        }

        #region private members
        
        List<MediaVehicle> mediaVehicles = null;

        private Random rand;

        private MediaManager.MediaPenetration media_penetration = null;

        private DpMediaDb mediaDb = null;

        private List<Agent> agents = null;

        private IFormatter formatter;

        private string HH_dir;
        private string Penetration_dir;

        #endregion

        List<MediaVehicle.MediaType> updateItems = new List<MediaVehicle.MediaType>();

        private void updateAgents_Click( object sender, EventArgs e )
        {
            updateItems.Clear();

            foreach( ListViewItem item in mediaTypeBox.CheckedItems )
            {
                string name = item.Text;
                updateItems.Add( (MediaVehicle.MediaType)Enum.Parse( typeof( MediaVehicle.MediaType ), name ) );
            }

            Thread thread = new Thread( rebuildAndApply_all );

            thread.Start();
        }

        private void initPenetrationDb()
        {
            DateTime then = DateTime.Now;
            this.Invoke( UpdateStatus, "Initializing Media Manager" );

            mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            Dictionary<string, Demographic.PrimVector> regStats = mediaDb.RefreshRegionData();

            mediaVehicles = mediaDb.ReadAllFromDb();

            this.media_penetration = new MediaManager.MediaPenetration( agents, regStats );

            media_penetration.InitLoad( Properties.Settings.Default.rawMediaDbConnection );

            Agent.KeepStats = true;
            Agent.ClearStats();

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, "Media manager initialized: " + (now - then).ToString() );
        }

        private void readAgents()
        {

            agents = read_List<Agent>( HH_dir + @"\agent", 0 );

            this.Invoke( UpdateStatus, new object[] { "Done loading agents" } );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootName"></param>
        /// <param name="list"></param>
        /// <param name="numToRead"> 0 means to read all</param>
        private List<T> read_List<T>( string rootName, int numToRead )
        {
            List<T> list = new List<T>();

            List<T> tmp = null;

            int fileNum = 0;
            bool done = false;
            while( !done && (numToRead == 0 || list.Count < numToRead) )
            {
                FileInfo fi = new FileInfo( rootName + fileNum.ToString() + ".dat" );

                if( fi.Exists )
                {
                    this.Invoke( UpdateStatus, new object[] { fi.FullName } );

                    FileStream file = new FileStream( fi.FullName, FileMode.Open );
                    tmp = (List<T>)formatter.Deserialize( file );
                    file.Close();

                    list.AddRange( tmp );

                    ++fileNum;
                }
                else
                {
                    done = true;
                }
            }

            return list;
        }

        private void saveAgents()
        {
            save_List<Agent>( ref agents, HH_dir + @"\agent" );
        }

        const int chunkSize = 10000;
        private void save_List<T>( ref List<T> list, string rootName )
        {
            List<T> tmp = new List<T>();
            List<T> saved = new List<T>();

            int fileNum = 0;
            int index = 0;
            while( list.Count > 0 )
            {
                tmp.Clear();

                for( int ii = 0; ii < chunkSize && 0 < list.Count; ++ii )
                {
                    // select a random agent

                    index = rand.Next( list.Count );

                    T item = list[index];
                    tmp.Add( item );
                    list.Remove( item );
                    saved.Add( item );
                }

                FileStream file = new FileStream( rootName + fileNum.ToString() + ".dat", FileMode.Create );

                this.Invoke( UpdateStatus, new object[] { "updating " + file.Name } );

                formatter.Serialize( file, tmp );
                file.Close();

                ++fileNum;
            }

            list = saved;

            this.Invoke( UpdateStatus, new object[] { "done" } );
        }


        private void rebuildAndApply_all()
        {
            DateTime then = DateTime.Now;

            // read agents as needed
            if( agents == null )
            {
                readAgents();
            }

            if( media_penetration == null )
            {
                initPenetrationDb();
            }


            foreach( Agent agent in agents )
            {
                agent.ClearMedia();
            }

            foreach( MediaVehicle.MediaType mediaType in updateItems )
            {
                updateMediaType( mediaType );
            }

            this.Invoke( UpdateStatus, "Saving Agents" );

            saveAgents();

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, " DONE elapsed time: " + (now - then).ToString() );
        }


        private void updateMediaType( MediaVehicle.MediaType type )
        {
            DateTime then = DateTime.Now;

            // collect vehicles of this type
            List<MediaVehicle> vcls = new List<MediaVehicle>();

            foreach( MediaVehicle vcl in mediaVehicles.Where( row => row.Type == type ) )
            {
                vcls.Add(vcl );
            }

            // clear agent data
              this.Invoke( UpdateStatus, "Cleaning Agents " + type.ToString() );

            foreach( Agent agent in agents )
            {
                agent.House.ClearMedia( (int) type );

            }

            mediaDb.DeleteFromDemographicDatabase( type );

            this.Invoke( UpdateStatus, "Applying " + type.ToString() );

            string fileName = this.Penetration_dir + @"\" + type.ToString() + "_penetration.txt";

            FileInfo fi = new FileInfo( fileName );


            if (!fi.Exists)
            {
                fileName = null;
            }

            media_penetration.ApplyMedia( type, vcls, fileName );

             
            this.Invoke( UpdateStatus, "Normalizing media db values" + type.ToString() );

            mediaDb.NormalizeVehicleDemographics( agents.Count );

            this.Invoke( UpdateStatus, "saving media db" );

            mediaDb.Update();

              this.Invoke( UpdateStatus, "Normalizing agent rates" + type.ToString() );
            media_penetration.Normalize_rates(type);

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, " DONE updating " + type.ToString() + " elapsed tims = " + (now - then).ToString() );
        }

        private void newPopulationToolStripMenuItem_Click( object sender, EventArgs e )
        {
            PopulationGenerator.PopulationGenerator2 generator = new PopulationGenerator2( Properties.Settings.Default.censusConnection );

            generator.Directory = new DirectoryInfo( HH_dir );

            if( generator.ShowDialog() != DialogResult.OK )
            {
                return;
            }
        }

        private void saveAgentsBut_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(saveAgents);

            thread.Start();
        }

        private void testPenetration_Click( object sender, EventArgs e )
        {
            updateItems.Clear();

            foreach( ListViewItem item in mediaTypeBox.CheckedItems )
            {
                string name = item.Text;
                updateItems.Add( (MediaVehicle.MediaType)Enum.Parse( typeof( MediaVehicle.MediaType ), name ) );
            }

            if( media_penetration == null )
            {
                this.media_penetration = new MediaManager.MediaPenetration( agents, null );
            }

            string oops = null;

            foreach( MediaVehicle.MediaType mediaType in updateItems )
            {
                string fileName = this.Penetration_dir + @"\" + mediaType.ToString() + "_penetration.txt";

                FileInfo fi = new FileInfo( fileName );

                if( fi.Exists )
                {

                    oops += media_penetration.ReadDemographics( null, fileName ) + "\r\n";
                }
                else
                {
                    oops += "Cannot read: " + fileName + "\r\n";
                }
            }

            if( oops != null && oops != "")
            {
                MessageBox.Show( oops );
            }

            this.media_penetration = null;
        }

        private void loadAgentsButton_Click( object sender, EventArgs e )
        {

            loadAgent();
        }

        class Stats
        {
            public double num;
            public double min;
            public double max;
            public double ave;

            public Stats()
            {
                min = double.MaxValue;
                max = double.MinValue;
                ave = 0;
                num = 0;
            }
        }


        private void loadAgent()
        {
            readAgents();

           // this.Invoke(UpdateStatus, "Reading Media" );

            // System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            //string fileName = Properties.Settings.Default.hhinput_directory + @"\vehicles.dat";


            //FileStream file = new FileStream( fileName, FileMode.Open );

            //Dictionary<Guid, MediaVehicle> vehicles = ( Dictionary<Guid, MediaVehicle>) serializer.Deserialize( file);
            //file.Close();

            // display information about agents

            this.Invoke(UpdateStatus, "Computing stats" );
            Dictionary<MediaVehicle.MediaType, Stats> info = new Dictionary<MediaVehicle.MediaType, Stats>();

            Array arr = Enum.GetValues( typeof( MediaVehicle.MediaType ) );

            foreach( MediaVehicle.MediaType type in arr )
            {
                double ave = 0;
                double max = double.MinValue;
                double min = double.MaxValue;
                double num = 0;
                foreach( Agent agent in this.agents )
                {
                    List<HouseholdMedia> hhmedia = agent.House.Media( (int) type );

                    foreach( HouseholdMedia media in hhmedia )
                    {
                        ave += media.Rate;
                        num += 1;
                        if( max < media.Rate )
                        {
                            max = media.Rate;
                        }

                        if( min > media.Rate )
                        {
                            min = media.Rate;
                        }
                    }
                }

                agentInfo.Text += "\r\n";
                agentInfo.Text += type.ToString() + "> ";
                agentInfo.Text += " num: " + num;
                agentInfo.Text += " max: " + max;
                agentInfo.Text += " min: " + min;

                if( num > 0 )
                {
                    ave = ave / num;

                    agentInfo.Text += " ave: " + ave;
                }
                else
                {
                    agentInfo.Text += " ave: NA";
                }
            }
        }

        private void regUpdate_Click( object sender, EventArgs e )
        {
            // read agents as needed
            if( agents == null )
            {
                readAgents();
            }


            DpMediaDb db = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            db.RefreshRegionInfo();

            db.AddAgentsToRegions( agents );

            db.Update();
        }


        //private void cleanAgents( GeoRegion region )
        //{

        //    List<Agent> tmp = new List<Agent>();
        //    foreach( Agent agent in agents.Where( row => region.ContainsGeoId( row.House.GeoID ) ) )
        //    {
        //        tmp.Add( agent );
        //    }

        //    agents = tmp;
        //}
    }
}
