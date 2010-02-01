using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MediaLibrary;
using GeoLibrary;

namespace Calibration
{
    public partial class CalibrateVehicles : Form
    {
        private Dictionary<int, TypeNode> type_nodes;

        private Dictionary<int, Dictionary<int, AdOption>> options;

        private List<string> regions;

        private static DpMediaDb db;

        private Dictionary<Media.vehicleRow, MediaVehicle> vehicle_dict;

        public delegate void LoadMedia();

        public event LoadMedia ReloadMedia;

        public CalibrateVehicles(DpMediaDb database, Dictionary<Media.vehicleRow, MediaVehicle> vehicles)
        {
            InitializeComponent();

            db = database;

            vehicle_dict = vehicles;
            
            options = db.GetAllOptions();

            List<GeoRegion>  temp = new List<GeoRegion>();

            temp.Add(GeoRegion.TopGeo);

            temp.AddRange(GeoRegion.TopGeo.GetRegions(GeoRegion.RegionType.DMA));

            regions = new List<string>();

            foreach (GeoRegion region in temp)
            {
                regions.Add(region.Name);
            }

            type_nodes = new Dictionary<int, TypeNode>();

            foreach (MediaVehicle vehicle in vehicle_dict.Values)
            {
                int type_id = db.GetTypeID(vehicle.Type.ToString());
                int subtype_id = db.GetSubtypeID(type_id, vehicle.SubType);

                //Build Nodes
                if (type_nodes.ContainsKey(type_id))
                {
                    type_nodes[type_id].AddVehicle(subtype_id, vehicle);
                }
                else
                {
                    TypeNode type_node = new TypeNode(type_id, subtype_id, vehicle);
                    MediaTree.Nodes.Add(type_node);
                    type_nodes.Add(type_id, type_node);
                }
            }

            MediaTree.CollapseAll();
        }

        #region node classes

        public interface IMediaTreeNode
        {
            int TypeID { get; }

            string GetDetails();

            void AddOption(int option_id, AdOption option);

            void RemoveOption(int option_id);

            void SetCPM(double new_value);

            void ScaleCPM(double amount);

            void ScaleSize(double amount);
        }

        private class TypeNode : TreeNode, IMediaTreeNode
        {
            int type_id;
            string type_name;
            int num_vehicles;

            private Dictionary<int, SubtypeNode> subtype_nodes;

            public TypeNode(int type_id, int subtype_id, MediaVehicle vehicle)
                : base(vehicle.Type.ToString())
            {
                this.type_id = type_id;
                this.type_name = vehicle.Type.ToString();

                subtype_nodes = new Dictionary<int, SubtypeNode>();

                SubtypeNode subtype_node = new SubtypeNode(type_id, subtype_id, vehicle);
                Nodes.Add(subtype_node);
                subtype_nodes.Add(subtype_id, subtype_node);

                num_vehicles = 1;
            }

            public VehicleNode FindVehicle( MediaVehicle vcl )
            {
                VehicleNode rval = null;

                foreach( SubtypeNode subNode in subtype_nodes.Values )
                {
                    rval = subNode.FindVehicle( vcl );

                    if( rval != null )
                    {
                        return rval;
                    }
                }

                return rval;
            }

            public void AddVehicle(int subtype_id, MediaVehicle vehicle)
            {
                num_vehicles++;

                if(subtype_nodes.ContainsKey(subtype_id))
                {
                    subtype_nodes[subtype_id].AddVehicle(vehicle);
                    return;
                }

                SubtypeNode subtype_node = new SubtypeNode(type_id, subtype_id, vehicle);

                Nodes.Add(subtype_node);
                subtype_nodes.Add(subtype_id, subtype_node);

            }

            #region IMediaTreeNode Members

            public int TypeID
            {
                get
                {
                    return type_id;
                }
            }

            public string GetDetails()
            {
                string details = "Type: " + type_name + "\n";
                details += "ID: " + type_id + "\n";
                details += "Number of Vehicles: " + num_vehicles;

                return details;
            }

            public void AddOption(int option_id, AdOption option)
            {
                foreach (IMediaTreeNode node in subtype_nodes.Values)
                {
                    node.AddOption(option_id, option);
                }
            }

            public void RemoveOption(int option_id)
            {
                foreach (IMediaTreeNode node in subtype_nodes.Values)
                {
                    node.RemoveOption(option_id);
                }
            }

            public void SetCPM(double new_value)
            {
                foreach (IMediaTreeNode node in subtype_nodes.Values)
                {
                    node.SetCPM(new_value);
                }
            }

            public void ScaleCPM(double amount)
            {
                foreach (IMediaTreeNode node in subtype_nodes.Values)
                {
                    node.ScaleCPM(amount);
                }
            }

              public void ScaleSize(double amount)
            {
                foreach (IMediaTreeNode node in subtype_nodes.Values)
                {
                    node.ScaleSize(amount);
                }
            }

            #endregion
        }

        private class SubtypeNode : TreeNode, IMediaTreeNode
        {
            int subtype_id;
            string subtype_name;
            int num_vehicles;
            int type_id;

            private Dictionary<string, RegionNode> region_nodes;

            public SubtypeNode(int type_id, int subtype_id, MediaVehicle vehicle) : base(vehicle.SubType)
            {
                this.subtype_id = subtype_id;
                this.subtype_name = vehicle.SubType;
                this.type_id = type_id;

                region_nodes = new Dictionary<string, RegionNode>();

                RegionNode region_node = new RegionNode(type_id, vehicle);
                Nodes.Add(region_node);
                region_nodes.Add(vehicle.RegionName, region_node);

                num_vehicles = 1;
            }

            public VehicleNode FindVehicle( MediaVehicle vcl )
            {
                VehicleNode rval = null;

                foreach( RegionNode regNode in region_nodes.Values )
                {
                    rval = regNode.FindVehicle( vcl );

                    if( rval != null )
                    {
                        return rval;
                    }
                }

                return rval;
            }

            public void AddVehicle(MediaVehicle vehicle)
            {
                num_vehicles++;

                if (region_nodes.ContainsKey(vehicle.RegionName))
                {
                    region_nodes[vehicle.RegionName].AddVehicle(vehicle);
                    return;
                }

                RegionNode region_node = new RegionNode(type_id, vehicle);
                Nodes.Add(region_node);
                region_nodes.Add(vehicle.RegionName, region_node);
            }

            

            #region IMediaTreeNode Members

            public int TypeID
            {
                get
                {
                    return type_id;
                }
            }

            public string GetDetails()
            {
                string details = "Subtype: " + subtype_name + "\n";
                details += "ID: " + subtype_id + "\n";
                details += "Number of Vehicles: " + num_vehicles;

                return details;
            }

            public void AddOption(int option_id, AdOption option)
            {
                foreach (IMediaTreeNode node in region_nodes.Values)
                {
                    node.AddOption(option_id, option);
                }
            }

            public void RemoveOption(int option_id)
            {
                foreach (IMediaTreeNode node in region_nodes.Values)
                {
                    node.RemoveOption(option_id);
                }
            }

            public void SetCPM(double new_value)
            {
                foreach (IMediaTreeNode node in region_nodes.Values)
                {
                    node.SetCPM(new_value);
                }
            }

            public void ScaleCPM(double amount)
            {
                foreach (IMediaTreeNode node in region_nodes.Values)
                {
                    node.ScaleCPM(amount);
                }
            }

               public void ScaleSize(double amount)
            {
                foreach (IMediaTreeNode node in region_nodes.Values)
                {
                    node.ScaleSize(amount);
                }
            }


            #endregion
        }

        private class RegionNode : TreeNode, IMediaTreeNode
        {
            string region_name;
            int num_vehicles;
            int type_id;

            private Dictionary<Guid, VehicleNode> vehicle_nodes;

            public RegionNode(int type_id, MediaVehicle vehicle)
                : base(vehicle.RegionName)
            {
                this.region_name = vehicle.RegionName;
                this.type_id = type_id;

                vehicle_nodes = new Dictionary<Guid, VehicleNode>();

                VehicleNode vehicle_node = new VehicleNode(type_id, vehicle);
                Nodes.Add(vehicle_node);
                vehicle_nodes.Add(vehicle.Guid, vehicle_node);
                
                num_vehicles = 1;
            }

            public VehicleNode FindVehicle( MediaVehicle vcl )
            {
                VehicleNode rval = null;

                if( vehicle_nodes.ContainsKey( vcl.Guid ) )
                {
                    return vehicle_nodes[vcl.Guid];
                }

                return null;
            }
            
            public void AddVehicle(MediaVehicle vehicle)
            {
                num_vehicles++;
                VehicleNode vehicle_node = new VehicleNode(type_id, vehicle);
                Nodes.Add(vehicle_node);
                vehicle_nodes.Add(vehicle.Guid, vehicle_node);
            }

            public void RemoveVehicle(MediaVehicle vehicle)
            {
                VehicleNode vehicle_node = vehicle_nodes[vehicle.Guid];
                vehicle_nodes.Remove(vehicle.Guid);
                Nodes.Remove(vehicle_node);
                num_vehicles--;
            }

            #region IMediaTreeNode Members

            public int TypeID
            {
                get
                {
                    return type_id;
                }
            }

            public string GetDetails()
            {
                string details = "Region: " + region_name + "\n";
                details += "Number of Vehicles: " + num_vehicles;

                return details;
            }

            public void AddOption(int option_id, AdOption option)
            {
                foreach (IMediaTreeNode node in vehicle_nodes.Values)
                {
                    node.AddOption(option_id, option);
                }
            }

            public void RemoveOption(int option_id)
            {
                foreach (IMediaTreeNode node in vehicle_nodes.Values)
                {
                    node.RemoveOption(option_id);
                }
            }

            public void SetCPM(double new_value)
            {
                foreach (IMediaTreeNode node in vehicle_nodes.Values)
                {
                    node.SetCPM(new_value);
                }
            }

            public void ScaleCPM(double amount)
            {
                foreach (IMediaTreeNode node in vehicle_nodes.Values)
                {
                    node.ScaleCPM(amount);
                }
            }

                public void ScaleSize(double amount)
            {
                foreach (IMediaTreeNode node in vehicle_nodes.Values)
                {
                    node.ScaleSize(amount);
                }
            }

            #endregion
        }


        private class VehicleNode : TreeNode, IMediaTreeNode
        {
            Guid vehicle_id;
            string vehicle_name;
            int type_id;
            MediaVehicle vehicle;
            Dictionary<int, AdOption> options;

            public VehicleNode(int type_id, MediaVehicle vehicle)
                : base(vehicle.Vehicle)
            {
                this.vehicle_id = vehicle.Guid;
                this.vehicle_name = vehicle.Vehicle;
                this.vehicle = vehicle;
                this.options = vehicle.GetOptions();
                this.type_id = type_id;
            }

            public string RegionName
            {
                get
                {
                    return vehicle.RegionName;
                }
            }

            public string URL
            {
                get
                {
                    return vehicle.URL;
                }
            }

            public string VehicleName
            {
                get
                {
                    return vehicle.Vehicle;
                }
            }

            public void ChangeURL(string new_value)
            {
                throw new Exception( "FIX THIS" );
                // vehicle.URL = new_value;
            }

            public void ChangeRegion(string new_region)
            {
                RegionNode region_node = Parent as RegionNode;
                GeoRegion region = GeoRegion.TopGeo.GetSubRegion(new_region);
                vehicle.RegionName = region.Name;

                // update db as well
               
                if (region_node != null)
                {
                    SubtypeNode subtype_node = region_node.Parent as SubtypeNode;
                    if (subtype_node != null)
                    {
                        region_node.RemoveVehicle(vehicle);
                        subtype_node.AddVehicle(vehicle);
                    }
                }
            }

            public void ChangeName(string new_value)
            {
                throw new Exception( "FIX THIS" );
                // vehicle.Vehicle = new_value;
            }

            

            #region IMediaTreeNode Members

            public int TypeID
            {
                get
                {
                    return type_id;
                }
            }

            public string GetDetails()
            {
                string details = "Vehicle: " + vehicle_name + "\n";
                details += "GUID: " + vehicle_id + "\n";
                details += "Type: " + vehicle.Type.ToString() + "\n";
                details += "Subtype: " + vehicle.SubType + "\n";
                details += "Region: " + vehicle.RegionName + "\n";
                details += "CPM: " + vehicle.CPM + "\n";
                details += "URL: " + vehicle.URL + "\n";
                foreach (int option_id in options.Keys)
                {
                    details += "Option: " + options[option_id].Name + "\n";
                    details += "Option ID: " + option_id + "\n";
                }

                return details;
            }

            public void AddOption(int option_id, AdOption option)
            {
                if (!options.ContainsKey(option_id))
                {
                    options.Add(option_id, option);
                }
            }

            public void RemoveOption(int option_id)
            {
                if (options.ContainsKey(option_id))
                {
                    options.Remove(option_id);
                }
            }

            public void SetCPM(double new_value)
            {
                vehicle.CPM = new_value;
            }

            public void ScaleCPM(double amount)
            {
                vehicle.CPM *= amount;
            }

               
            public void ScaleSize(double amount)
            {
                vehicle.Size *= amount;
            }

            #endregion
        }

        #endregion

        #region UI
        private void MediaTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IMediaTreeNode media_node = e.Node as IMediaTreeNode;
            DetailsBox.Text = media_node.GetDetails();

            VehicleNode vehicle_node = e.Node as VehicleNode;
            if (vehicle_node != null)
            {
                changeRegionToolStripMenuItem.Enabled = true;
                changeURLToolStripMenuItem.Enabled = true;
                changeNameToolStripMenuItem.Enabled = true;
            }
            else
            {
                changeRegionToolStripMenuItem.Enabled = false;
                changeURLToolStripMenuItem.Enabled = false;
                changeNameToolStripMenuItem.Enabled = false;
            }
        }

        private void addAdOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMediaTreeNode media_node = MediaTree.SelectedNode as IMediaTreeNode;
            if (media_node == null)
            {
                return;
            }

            dialogs.SelectAdOption dialog = new Calibration.dialogs.SelectAdOption(options[media_node.TypeID]);

            dialog.Title = "Add Ad Option";
            dialog.Info = "Select Ad Option to Add";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will add the selected option to all vehicles under this node.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    media_node.AddOption(dialog.Option.ID, dialog.Option);
                }
            }
        }

        private void removeAdOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMediaTreeNode media_node = MediaTree.SelectedNode as IMediaTreeNode;
            if (media_node == null)
            {
                return;
            }

            dialogs.SelectAdOption dialog = new Calibration.dialogs.SelectAdOption(options[media_node.TypeID]);

            dialog.Title = "Remove Ad Option";
            dialog.Info = "Select Ad Option to Remove";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will remove the selected option from all vehicles under this node.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    media_node.AddOption(dialog.Option.ID, dialog.Option);
                }
            }
        }

        private void setCPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMediaTreeNode media_node = MediaTree.SelectedNode as IMediaTreeNode;
            if (media_node == null)
            {
                return;
            }

            dialogs.GetDouble dialog = new Calibration.dialogs.GetDouble();

            dialog.Title = "Set CPM";
            dialog.Info = "Enter new CPM value";
            dialog.Type = "Value:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will set the CPM for all vehicles under this node.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    media_node.SetCPM(dialog.Value);
                }
            }
            
        }

        private void scaleCPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMediaTreeNode media_node = MediaTree.SelectedNode as IMediaTreeNode;
            if (media_node == null)
            {
                return;
            }

            dialogs.GetDouble dialog = new Calibration.dialogs.GetDouble();

            dialog.Title = "Scale CPM";
            dialog.Info = "Enter CPM scalar";
            dialog.Type = "Scalar:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will scale the CPM for all vehicles under this node.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    media_node.ScaleCPM(dialog.Value);
                }
            }
        }

        private void changeRegionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleNode vehicle_node = MediaTree.SelectedNode as VehicleNode;
            if (vehicle_node == null)
            {
                return;
            }

            dialogs.SelectRegion dialog = new Calibration.dialogs.SelectRegion(regions);

            dialog.Info = "Current Region: " + vehicle_node.RegionName;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will change the region for the selected vehicle.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    vehicle_node.ChangeRegion(dialog.RegionName);
                }
            }
        }

        private void changeURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleNode vehicle_node = MediaTree.SelectedNode as VehicleNode;
            if (vehicle_node == null)
            {
                return;
            }

            dialogs.GetString dialog = new Calibration.dialogs.GetString();

            dialog.Title = "Set URL";
            dialog.Info = "Current URL: " + vehicle_node.URL;
            dialog.Type = "URL:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation change the URL for the selected vehicle.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    vehicle_node.ChangeURL(dialog.Value);
                }
            }
        }

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleNode vehicle_node = MediaTree.SelectedNode as VehicleNode;
            if (vehicle_node == null)
            {
                return;
            }

            dialogs.GetString dialog = new Calibration.dialogs.GetString();

            dialog.Title = "Set Name";
            dialog.Info = "Current Name: " + vehicle_node.VehicleName;
            dialog.Type = "Name:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation change the name for the selected vehicle.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    vehicle_node.ChangeName(dialog.Value);
                }
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This operation will write all changes made to the database.  It may take several minutes to complete the operation.\nDo you want to proceed?", "Confirm Database Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                throw new Exception( "FIX THIS" );
                //db.ReplaceVehicleOptions(vehicle_dict.Values);
                //db.WriteVehiclesToDb(vehicle_dict);
                db.Update();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        private void findBut_Click( object sender, EventArgs e )
        {
            string queryText = queryBox.Text.ToUpper();

            // find among vehicles
            if( vehicle_dict.Any( row => row.Key.name.IndexOf( queryText ) >= 0 ) )
            {
                KeyValuePair<Media.vehicleRow, MediaVehicle> vclPair = vehicle_dict.First( row => row.Key.name.ToUpper().IndexOf( queryText) >= 0 );

                Media.vehicleRow vclRow = vclPair.Key;
                MediaVehicle vcl = vclPair.Value;

                TypeNode mediaNode = type_nodes[vclRow.media_subtypeRow.media];

                VehicleNode vclNode = mediaNode.FindVehicle( vcl );

                if( vcl != null )
                {
                    MediaTree.SelectedNode = vclNode;
                }
            }
        }

        private void changeSizeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            IMediaTreeNode media_node = MediaTree.SelectedNode as IMediaTreeNode;
            if (media_node == null)
            {
                return;
            }

            dialogs.GetDouble dialog = new Calibration.dialogs.GetDouble();

            dialog.Title = "Scale Size";
            dialog.Info = "Enter Size scalar";
            dialog.Type = "Scalar:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("This operation will scale the size for all vehicles under this node.\nAre you sure you wish to proceed?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    media_node.ScaleCPM(dialog.Value);
                }
            }

        }
    }
}
