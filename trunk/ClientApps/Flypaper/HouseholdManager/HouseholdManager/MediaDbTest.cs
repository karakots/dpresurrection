using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MediaLibrary;
using GeoLibrary;
using DemographicLibrary;

namespace HouseholdManager
{
    public partial class MediaDbTest : UserControl
    {
        public MediaDbTest()
        {
            InitializeComponent();
            vclTable = new DataTable( "Vehicles" );

            vclTable.Columns.Add( "name", typeof( string ) );
            vclTable.Columns.Add( "region", typeof( string ) );
            vclTable.Columns.Add( "size", typeof( double ) );
            vclTable.Columns.Add( "rel_size", typeof( string ) );

            vclGrid.DataSource = vclTable;

            // GeoInfo.ReadFromFile(Properties.Settings.Default.hhinput_directory);

            initTree();
        }

        private void initTree()
        {
            if( GeoRegion.TopGeo != null )
            {
                this.regionView.Nodes.Clear();

                this.regionView.Nodes.Add( createTree( GeoRegion.TopGeo ) );
            }
        }

        private DpMediaDb mediaDb;
        private DataTable vclTable;
        private void button1_Click( object sender, EventArgs e )
        {
            mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnection );
            mediaDb.RefreshWebData();

            mediaBox.DataSource = mediaDb.GetMediaTypes();
        }

        private void mediaBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.subtypeBox.DataSource = null;

            this.subtypeBox.DataSource = mediaDb.GetSubTypes( mediaBox.SelectedItem.ToString() );
        }

        // select vihicles
        private void button2_Click( object sender, EventArgs e )
        {
            if( regionView.SelectedNode == null )
            {
                return;
            }

            List<GeoRegion> regs = new List<GeoRegion>();

            GeoRegion DMA = (GeoRegion)regionView.SelectedNode.Tag;

            if( DMA != null )
            {
                regs.Add( DMA );
            }
            else
            {
                regs.Add( GeoRegion.TopGeo );
            }

            List<MediaVehicle> vclSize = mediaDb.GetMediaVehicleWithSize( mediaBox.SelectedItem.ToString(), subtypeBox.SelectedItem.ToString(), regs );

            
            Demographic seg = new Demographic();
            List<Demographic> segs = new List<Demographic>();
            segs.Add( seg );

            Demographic.PrimVector vec = mediaDb.TargetPopulation( segs, regs );

            double reg_size = vec.Any;

            regSize.Text = (reg_size * DpMediaDb.US_HH_SIZE).ToString();

            vclTable.Clear(); 
            foreach( MediaVehicle vcl in vclSize )
            {
                DataRow row = vclTable.NewRow();

                row["name"] = vcl.Vehicle;
                row["region"] = vcl.RegionName;
                row["size"] = vcl.Size;
                row["rel_size"] = vcl.Size / (reg_size * DpMediaDb.US_HH_SIZE);

                vclTable.Rows.Add( row );
            }

            vclTable.AcceptChanges();
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
    }
}
