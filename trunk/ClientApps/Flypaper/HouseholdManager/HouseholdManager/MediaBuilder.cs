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

using GeoLibrary;
using MediaManager;
using MediaLibrary;
using Calibration;

namespace HouseholdManager
{
    public partial class MediaBuilder : UserControl
    {
        public HouseholdManagerUI.UpdateDelegate UpdateStatus;
        public MediaBuilder()
        {
            InitializeComponent();

            foreach( string name in Enum.GetNames( typeof( MediaLibrary.MediaVehicle.MediaType ) ) )
            {
                mediaTypeBox.Items.Add( name );
            }

            initTree();
        }

        MediaManager.MediaBuilder media_builder;

        DpMediaDb mediaDb = null;

        private void updateMedia_Click( object sender, EventArgs e )
        {
            updateItems.Clear();

            foreach(ListViewItem  item in mediaTypeBox.CheckedItems )
            {
                string name = item.Text;
                updateItems.Add( (MediaVehicle.MediaType) Enum.Parse( typeof( MediaVehicle.MediaType ), name ) );
            }

            Thread thread = new Thread(rebuild_all);

            thread.Start();
        }

        List<MediaVehicle.MediaType> updateItems = new List<MediaVehicle.MediaType>();
        private void initMediaDb()
        {
            DateTime then = DateTime.Now;
            this.Invoke( UpdateStatus, "Initializing Media Manager" );

            this.media_builder = new MediaManager.MediaBuilder();

            media_builder.InitLoad( Properties.Settings.Default.rawMediaDbConnection );

            mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            mediaDb.RefreshWebData();

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, "Media manager initialized: " + (now - then).ToString() );
        }


        private void rebuild_all()
        {
            this.Invoke( UpdateStatus, "Updating..." );
            DateTime then = DateTime.Now;


            foreach( MediaVehicle.MediaType mediaType in updateItems )
            {
                buildMediaType( mediaType );
            }

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, " DONE elapsed time: " + (now - then).ToString() );
        }

        private void buildMediaType( MediaVehicle.MediaType type )
        {
            DateTime then = DateTime.Now;

            if( this.media_builder == null )
            {
                initMediaDb();
            }

           // mediaDb.DeleteFromVehicleDatabase( type );

            this.Invoke( UpdateStatus, "Building " + type.ToString() );

            media_builder.BuildMediaFromDB( type );

            // media_builder.AddVehiclesToDb( type, mediaDb );

            this.Invoke( UpdateStatus, "saving media db" );

            mediaDb.Update();

            this.Invoke( UpdateStatus, "clearing memory" );
            mediaDb.ClearVehicle();
            media_builder.ClearVehicleData();

            DateTime now = DateTime.Now;

            this.Invoke( UpdateStatus, " DONE updating " + type.ToString() + " elapsed tims = " + (now - then).ToString() );

        }

        private void readFile_Click( object sender, EventArgs e )
        {
            if( this.media_builder == null )
            {
                initMediaDb();
            }

            media_builder.UpdateRadio(Properties.Settings.Default.rawdata_directory + @"\info\radio_info.txt" );


        }

        private void RefreshAdOptions_Click(object sender, EventArgs e)
        {
            if (mediaDb == null)
            {
                mediaDb = new DpMediaDb(Properties.Settings.Default.mediaConnection);

                mediaDb.RefreshWebData();
            }
            //mediaDb.BuildAdOptionTable();
            MessageBox.Show("Finished");
        }

        private void build_options_Click(object sender, EventArgs e)
        {
            if (mediaDb == null)
            {
                mediaDb = new DpMediaDb(Properties.Settings.Default.mediaConnection);

                mediaDb.RefreshWebData();
            }
            Dictionary<int, Dictionary<int, AdOption>> options = mediaDb.GetAllOptions();
            foreach (int type_id in options.Keys)
            {
                MediaVehicle.MediaType type = mediaDb.GetTypeForID(type_id);
                foreach (int option_id in options[type_id].Keys)
                {
                    AdOption option = options[type_id][option_id];
                    if (type == MediaVehicle.MediaType.Internet)
                    {
                        SimpleOption simp_option = option as SimpleOption;
                        option = new OnlineOption(simp_option.Name, simp_option.ID, simp_option.Awareness, simp_option.Persuasion, simp_option.Recency, 0.5, 0.1, 1.0, 0.5, 0.01, 0.01, simp_option.Cost_Modifier, simp_option.ConsiderationProbScalar, simp_option.ConsiderationPersuasionScalar, simp_option.ConsiderationAwarenessScalar);
                    }
                    //options[type_id][option_id].set_id(option_id);
                   // mediaDb.UpdateOption(option_id, option);
                }
            }

            mediaDb.Update();
        }

        private void calVehicleButton_Click( object sender, EventArgs e )
        {
            GeoRegion DMA = null;

            if( regionView.SelectedNode != null )
            {
                DMA = (GeoRegion)regionView.SelectedNode.Tag;
            }
         
            string region = "all regions";
            string message = "All Media vehicles will be read in, continue?";

            if (DMA != null)
            {
                region = DMA.Name;
            }

            if( DMA == GeoRegion.TopGeo )
            {
                DialogResult rslt = MessageBox.Show( "View in all vehicles?\r\n (If no only national vehicles will be viewed)", "Continue?", MessageBoxButtons.YesNoCancel );

                if( rslt == DialogResult.Cancel )
                {
                    return;
                }

                if( rslt == DialogResult.Yes )
                {
                    DMA = null;
                }
            }
            else
            {
                DialogResult rslt = MessageBox.Show( "Media will be read in for " + region + ", continue?", "Continue?", MessageBoxButtons.OKCancel );
                if( rslt != DialogResult.OK )
                {
                    return;
                }
            }

            DpMediaDb db = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            db.RefreshAllMediaData( DMA );

            Dictionary<Media.vehicleRow, MediaVehicle> media = null;
            string error = null; // db.ReadVehiclesFromDb( out media );

            if( error != null )
            {
                MessageBox.Show( error );
                return;
            }

            CalibrateVehicles cal = new CalibrateVehicles( db, media );

            cal.Text = "Edting Media in " + region;

            cal.Show();

        }

        private void calOptionsButton_Click( object sender, EventArgs e )
        {
            // read in options file

            // ensure geo regions are read in
            string fileName = Properties.Settings.Default.hhinput_directory + @"\options.dat";

            FileStream file = new FileStream( fileName, FileMode.Open );

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Dictionary<int, AdOption> ad_options = (Dictionary<int, AdOption>)formatter.Deserialize( file );
            file.Close();

            // update database with options
            if( mediaDb == null )
            {
                mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnection );

                mediaDb.RefreshWebData();
            }

            foreach(AdOption option in ad_options.Values)
            {
                mediaDb.UpdateAdOption( option );
            }

            mediaDb.Update();
        }

        private void initTree()
        {
            if( GeoRegion.TopGeo != null )
            {
                this.regionView.Nodes.Clear();

                this.regionView.Nodes.Add( createTree( GeoRegion.TopGeo ) );
            }
        }

        private TreeNode createTree( GeoRegion region )
        {
            TreeNode node = new TreeNode( region.Name );
            node.Tag = region;

            foreach( GeoRegion sub in region.SubRegions )
            {
                if( sub.SubRegions != null )
                {
                    node.Nodes.Add( createTree( sub ) );
                }
            }

            return node;
        }

        private void mediaToFIleButton_Click( object sender, EventArgs e )
        {
            DpMediaDb db = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            db.RefreshAllMediaData();

            Dictionary<Media.vehicleRow, MediaVehicle> media = null;
            string error = null; // db.ReadVehiclesFromDb( out media );

            Dictionary<Guid, MediaVehicle> vehicles = new Dictionary<Guid, MediaVehicle>();
            foreach( MediaVehicle vehicle in media.Values )
            {
                vehicles.Add( vehicle.Guid, vehicle );
            }

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            string fileName = Properties.Settings.Default.hhinput_directory + @"\vehicles.dat";


            FileStream file = new FileStream( fileName, FileMode.Create );

            serializer.Serialize( file, vehicles );
            file.Close();
        }

        private void updateDPMButton_Click( object sender, EventArgs e )
        {
              DpMediaDb db = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            db.RefreshAllMediaData();

            Dictionary<Media.vehicleRow, MediaVehicle> media = null;
            string error = null; // db.ReadVehiclesFromDb( out media );

            // db.WriteCPM( media );

            db.Update();

        }

        private void OptionWrite_Click( object sender, EventArgs e )
        {

          
            DpMediaDb db = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            // db.ConvertVehicles();

            ////db.RefreshAdOptions();

            ////foreach( int id in ad_options.Keys )
            ////{
            ////    AdOption option = ad_options[id];

            ////    db.UpdateAdOption( option );
            ////}

            //// db.Update();

            //fileName = Properties.Settings.Default.hhinput_directory + @"\options.dat";


            //file = new FileStream( fileName, FileMode.Create );

            //serializer.Serialize( file, ad_options );
            //file.Close();
        }

    }
}
