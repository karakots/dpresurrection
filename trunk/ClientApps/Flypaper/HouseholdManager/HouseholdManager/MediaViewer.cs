using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HouseholdLibrary;

namespace HouseholdManager
{
    public partial class MediaViewer : Form
    {
        private Dictionary<string, Dictionary<string, MediaVehicle>> media_tree;

        private List<MediaVehicle> selected_media;

   
        private List<Demographic> filters = new List<Demographic>();

        public MediaViewer(List<MediaVehicle> media)
        {
            InitializeComponent();

            MediaTree.Nodes.Add("All", "All");

            TreeNode top = MediaTree.Nodes["All"];
            media_tree = new Dictionary<string, Dictionary<string, MediaVehicle>>();
            selected_media = new List<MediaVehicle>();

            foreach (MediaVehicle data in media)
            {
                if (!media_tree.ContainsKey(data.Type.ToString()))
                {
                    media_tree.Add(data.Type.ToString(), new Dictionary<string, MediaVehicle>());
                }
                if (!top.Nodes.ContainsKey(data.Type.ToString()))
                {
                    top.Nodes.Add(data.Type.ToString(), data.Type.ToString());
                }
                media_tree[data.Type.ToString()].Add(data.SubType, data);
                top.Nodes[data.Type.ToString()].Nodes.Add(data.SubType, data.SubType);
            }

            this.MediaTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MediaTree_AfterSelect);
        }

        private void MediaTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            update_info();
        }

        private void update_info()
        {
            selected_media.Clear();
            TreeNode top = MediaTree.SelectedNode;

            gather_media(top);

            MediaInfoBox.Items.Clear();
            SegmentInfoBox.Items.Clear();

            foreach (MediaVehicle data in selected_media)
            {
               
                foreach (Demographic demographic in data.Demographics)
                {
                    MediaInfoBox.Items.Add(data.Type + " " + data.SubType + " " + demographic.ToString() + "     Reach: " + demographic.Info.Reach + " Rate: " + demographic.Info.Rate);
                }

                // compute reach info
                if( filters.Count > 0 )
                {
                    foreach( Demographic filter in filters )
                    {

                        ReachInfo demReach = data.Reach( filter );
                        SegmentInfoBox.Items.Add( data.Type + " " + data.SubType + " " + filter.ToString() + "     Reach: " + demReach.Reach + " Eff: " + demReach.Effeciency );
                    }

                    ReachInfo reach = data.Reach( filters );
                    SegmentInfoBox.Items.Add( data.Type + " " + data.SubType + "Total     Reach: " + reach.Reach + " Eff: " + reach.Effeciency );
                }
            }
        }

        private void gather_media(TreeNode top)
        {
            if (top.Nodes.Count == 0)
            {
                selected_media.Add(media_tree[top.Parent.Text][top.Text]);
            }
            else
            {
                foreach (TreeNode node in top.Nodes)
                {
                    gather_media(node);
                }
            }
        }

        private void SegButton_Click(object sender, EventArgs e)
        {
            SegmentDesigner dlg = new SegmentDesigner();

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            filters.Add( dlg.Demographic );

            update_info();
        }

        private void clearSegmentsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            filters.Clear();

            update_info();
        }


    }
}
