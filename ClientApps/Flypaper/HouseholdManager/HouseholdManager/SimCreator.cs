using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


using HouseholdLibrary;
using DemographicLibrary;
using SimInterface;
using GeoLibrary;
using MediaLibrary;

using DpSimQueue;

namespace HouseholdManager
{
    public partial class SimCreator : Form
    {
        public SimInput Input;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>> media_dict;
        private Dictionary<string, MediaVehicle> vehicle_ids;
        private List<MediaComp> media_comps;
        private List<Demographic> demograhics;

        private Demographic target_demo;
        private string target_region;

        static int numSims = 1;


        #region simulation properties

        private DpUpdate database;
        private Guid? sim_id;
        private DirectoryInfo output_dir;

        #endregion

        public SimCreator()
        {
            InitializeComponent();

            media_dict = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>>();
            vehicle_ids = new Dictionary<string, MediaVehicle>();
            media_comps = new List<MediaComp>();
            demograhics = new List<Demographic>();

            // collect Media

            database = new DpUpdate( Properties.Settings.Default.simConnection );

            string out_dir = Properties.Settings.Default.output_directory;

            DpMediaDb mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnection );

            List<MediaVehicle> media = mediaDb.ReadMediaFromDb();

            foreach (MediaVehicle vehicle in media)
            {
                string type = vehicle.Type.ToString();
                string subtype = vehicle.SubType;
                string name = vehicle.Vehicle;
                string region = vehicle.RegionName;
                if (!media_dict.ContainsKey(type))
                {
                    media_dict.Add(type, new Dictionary<string, Dictionary<string,Dictionary<string,Dictionary<string,int>>>>());
                    TypeCombo.Items.Add(type);
                }
                if (!media_dict[type].ContainsKey(subtype))
                {
                    media_dict[type].Add(subtype, new Dictionary<string, Dictionary<string,Dictionary<string,int>>>());
                }
                if (!media_dict[type][subtype].ContainsKey(region))
                {
                    media_dict[type][subtype].Add(region, new Dictionary<string, Dictionary<string, int>>());
                    if(!TargetGeoCombo.Items.Contains(region))
                    {
                        TargetGeoCombo.Items.Add(region);
                    }
                }
                if(!media_dict[type][subtype][region].ContainsKey(name))
                {
                    media_dict[type][subtype][region].Add(name, new Dictionary<string,int>());
                }
                if (!vehicle_ids.ContainsKey(name + region))
                {
                    vehicle_ids.Add(name + vehicle.RegionName, vehicle);
                }
                Dictionary<int, AdOption> all_options = vehicle.GetOptions();
                foreach (int option in all_options.Keys)
                {
                    if( !media_dict[type][subtype][region][name].ContainsKey( all_options[option].Name ) )
                    {
                        media_dict[type][subtype][region][name].Add( all_options[option].Name, option );
                    }
                }
            }

            TypeCombo.SelectedIndex = 0;

            SimNameText.Text = DpSimQueue.DpUpdate.Calibrator;

             numSims++;
        }

        public void Reset()
        {
            media_comps.Clear();
            demograhics.Clear();
            SegmentsList.Items.Clear();
            MediaList.Items.Clear();
        }

        private void AddMediaButton_Click(object sender, EventArgs e)
        {
            MediaComp comp = new MediaComp(MediaVehicle.MediaType.Internet);
            string type = TypeCombo.SelectedItem.ToString();
            string subtype = SubTypeCombo.SelectedItem.ToString();
            string vehicle = VehicleCombo.SelectedItem.ToString();
            string region = RegionCombo.SelectedItem.ToString();
            string option = OptionCombo.SelectedItem.ToString();

            comp.Guid = vehicle_ids[vehicle+region].Guid;
            comp.StartDate = (int)StartNumeric.Value;
            comp.Span = (int)LengthNumeric.Value;
            comp.ad_option = media_dict[type][subtype][region][vehicle][option];

            if (TargetSegmentButton.Enabled && target_demo != null)
            {
                //comp.target_demo = target_demo;
            }

            if (TargetGeoCombo.Enabled && target_region != null)
            {
                //comp.target_region = target_region;
            }

            if (FuzzyFactor.Enabled)
            {
                comp.demo_fuzz_factor = (double)FuzzyFactor.Value / 100.0;
            }

            if (ImpressionsNumeric.Enabled == true)
            {
                comp.Impressions = (double)ImpressionsNumeric.Value;
            }

            string desc = type + ", " + subtype + ", " + vehicle + ", " + region + ", " + option + " Start Date: " + comp.StartDate + " Span: " + comp.Span;
            media_comps.Add(comp);
            MediaList.Items.Add(desc);
        }

        public string SimName
        {
            get
            {
                return this.SimNameText.Text;
            }
        }
        private void RunButton_Click(object sender, EventArgs e)
        {
            Input = new SimInput();
            Input.Demographics.AddRange(demograhics);
            if (Input.Demographics.Count == 0)
            {
                Input.Demographics.Add(new Demographic());
            }
            Input.Media.AddRange(media_comps);
            Input.EndDate = (int)SimLengthNumeric.Value;

            if( optionBox.Text != null && optionBox.Text != "" )
            {
                Input.option = optionBox.Text;
            }

            sim_id = database.CreateSimulation(SimName );

            if( !sim_id.HasValue )
            {
                MessageBox.Show( "Error creating simulation" );
                return;
            }

            database.UpdateInput( sim_id.Value, Input );

            database.QueueSim( sim_id.Value );

            run_sim.Start();
        }

        private void TypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = TypeCombo.SelectedItem.ToString();
            SubTypeCombo.Items.Clear();
            SubTypeCombo.Items.AddRange(media_dict[type].Keys.ToArray());
            SubTypeCombo.SelectedIndex = 0;

            if (type.ToUpper() == "INTERNET")
            {
                TargetGeoCombo.Enabled = true;
                TargetGeoCombo.SelectedIndex = 0;
                TargetSegmentButton.Enabled = true;
                ImpressionsNumeric.Enabled = true;
                FuzzyFactor.Enabled = true;
            }
            else
            {
                TargetGeoCombo.Enabled = false;
                TargetSegmentButton.Enabled = false;
                ImpressionsNumeric.Enabled = false;
                FuzzyFactor.Enabled = false;
            }
        }

        private void SubTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = TypeCombo.SelectedItem.ToString();
            string subtype = SubTypeCombo.SelectedItem.ToString();
            RegionCombo.Items.Clear();
            RegionCombo.Items.AddRange(media_dict[type][subtype].Keys.ToArray());
            RegionCombo.SelectedIndex = 0;
        }

        private void RegionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = TypeCombo.SelectedItem.ToString();
            string subtype = SubTypeCombo.SelectedItem.ToString();
            string region = RegionCombo.SelectedItem.ToString();
            VehicleCombo.Items.Clear();
            VehicleCombo.Items.AddRange(media_dict[type][subtype][region].Keys.ToArray());
            VehicleCombo.SelectedIndex = 0;
        }

        private void VehicleCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = TypeCombo.SelectedItem.ToString();
            string subtype = SubTypeCombo.SelectedItem.ToString();
            string region = RegionCombo.SelectedItem.ToString();
            string vehicle = VehicleCombo.SelectedItem.ToString();
            OptionCombo.Items.Clear();
            OptionCombo.Items.AddRange(media_dict[type][subtype][region][vehicle].Keys.ToArray());
            if (OptionCombo.Items.Count > 0)
            {
                OptionCombo.SelectedIndex = 0;
            }

            if (type.ToUpper() == "MAGAZINE")
            {
                //Magazine magazine = vehicle_ids[vehicle + region] as Magazine;

                //if (magazine != null)
                //{
                //    if (magazine.DMAAds)
                //    {
                //        TargetGeoCombo.Enabled = true;
                //    }
                //    else
                //    {
                //        TargetGeoCombo.Enabled = false;
                //    }
                //}
            }
        }

        

        private void AddSegmentButton_Click(object sender, EventArgs e)
        {
            SegmentDesigner designer = new SegmentDesigner();

            if (designer.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            demograhics.Add(designer.Demographic);
            SegmentsList.Items.Add(designer.Demographic.Name + " " + designer.Demographic.ToString());
        }

        private void cancelButton_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
        }

        private void TargetSegmentButton_Click(object sender, EventArgs e)
        {
            SegmentDesigner designer = new SegmentDesigner();

            if (designer.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            target_demo = designer.Demographic;
        }

        private void TargetGeoCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            target_region = TargetGeoCombo.SelectedItem.ToString();
        }



        #region simulation control

        private void run_sim_Tick( object sender, EventArgs e )
        {
            if( !sim_id.HasValue )
            {
                return;
            }


            bool done = database.SimDone( sim_id.Value );

            if( done )
            {
                run_sim.Stop();

                SimOutput output = database.GetResults( sim_id.Value );

                write_output( output );
            }
        }

        private void write_output( SimOutput output )
        {
            StreamWriter writer = new StreamWriter( output_dir.FullName + "\\output.csv" );

            Dictionary<string, int> metric_lengths = new Dictionary<string, int>();

            foreach( Metric metric in output.metrics )
            {
                string name = "Segment: " + metric.Segment + " Type: " + metric.Type;
                metric_lengths.Add( metric.Type + metric.Segment, metric.values.Count );
                writer.Write( name + "," );
            }
            writer.Write( writer.NewLine );

            int max = metric_lengths.Values.Max();

            for( int i = 0; i < max; i++ )
            {
                foreach( Metric metric in output.metrics )
                {
                    if( i < metric_lengths[metric.Type + metric.Segment] )
                    {
                        writer.Write( metric.values[i] + "," );
                    }
                }
                writer.Write( writer.NewLine );
            }

            writer.Close();

        }

        #endregion
        

        


    }
}
