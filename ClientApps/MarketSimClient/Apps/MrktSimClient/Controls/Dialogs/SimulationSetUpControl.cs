using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class SimulationSetUpControl : UserControl
    {
        public SimulationSetUpControl()
        {
            InitializeComponent();

            this.initialSeed.Value = 123;
            this.initialSeed.Visible = true;
            this.seedLabel.Visible = true;
        }

        private MrktSimDBSchema.simulationRow currentSimulation = null;
        SimulationDb theDb;
        private int modelPopSize;
        private double simScaling = 0;


        public SimulationDb Db
        {
            set
            {
                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                if (currentSimulation == null)
                    return;

                modelPopSize = currentSimulation.scenarioRow.Model_infoRow.pop_size;

                simScaling = currentSimulation.scale_factor;

                scalingNumericUpDown.Value = (int) ((simScaling / 100.0) * modelPopSize);

                numConsumers.Text = modelPopSize.ToString("N");


                // dates
                startEndDate.Start = currentSimulation.start_date;
                startEndDate.End = currentSimulation.end_date;

                // reset panel data bit
                resetPanelCheckBox.Checked = currentSimulation.reset_panel_state;

                // write rate
                if (currentSimulation.access_time % 7 == 0)
                {
                    access_timeCombo.SelectedIndex = 1;
                    this.numSpans.Value = (int)currentSimulation.access_time / 7;
                }
                else if (currentSimulation.access_time % 30 == 0)
                {
                    access_timeCombo.SelectedIndex = 2;
                    this.numSpans.Value = (int)currentSimulation.access_time / 30;
                }
                else
                {
                    access_timeCombo.SelectedIndex = 0;
                    this.numSpans.Value = (int)currentSimulation.access_time;
                }


                nameBox.Text = currentSimulation.name;
                descrBox.Text = currentSimulation.descr;

                if (currentSimulation.type == (byte)SimulationDb.SimulationType.Statistical)
                {
                    seedLabel.Visible = false;
                    initialSeed.Visible = false;
                }
                else
                {
                    MrktSimDBSchema.scenario_simseedRow[] seeds = currentSimulation.Getscenario_simseedRows();

                     if (seeds.Length > 0)
                     {
                         initialSeed.Value = seeds[0].seed;
                     }
                }

                numSimsLabel.Text = SimulationDb.NumSims(currentSimulation).ToString();

                DataRow sim_type = SimulationDb.simulation_type.Rows.Find(currentSimulation.type);

                if (sim_type != null)
                    simType.Text = sim_type["type"].ToString();
                else
                    simType.Text = "Unkown simulation type.";
            }
        }


        public override void Refresh()
        {
            base.Refresh();

            numSimsLabel.Text = SimulationDb.NumSims(currentSimulation).ToString();
        }

        public void WriteData()
        {
            if (currentSimulation.name != nameBox.Text)
            {
                string filter = "id <> " + currentSimulation.id;
                currentSimulation.name = ModelDb.CreateUniqueName(theDb.Data.simulation, "name", nameBox.Text, filter);
            }

            // update the rest of the data
            currentSimulation.start_date = startEndDate.Start;
            currentSimulation.end_date = startEndDate.End;
            currentSimulation.reset_panel_state = resetPanelCheckBox.Checked;
            currentSimulation.scale_factor = 100.0 * ((double) scalingNumericUpDown.Value) / modelPopSize;
            currentSimulation.metric_start_date = currentSimulation.start_date;
            currentSimulation.metric_end_date = currentSimulation.end_date;
            switch (access_timeCombo.SelectedIndex)
            {
                case 0:
                    currentSimulation.access_time = 1;
                    break;
                case 1:
                    currentSimulation.access_time = 7;
                    break;
                case 2:
                    currentSimulation.access_time = 30;
                    break;
                default:
                    currentSimulation.access_time = 1;
                    break;
            }

            currentSimulation.access_time *= (int)numSpans.Value;

            currentSimulation.descr = descrBox.Text;

            // update the sim seed if not a statistical run
            if (currentSimulation != null &&
                ((SimulationDb.SimulationType)currentSimulation.type) != SimulationDb.SimulationType.Statistical)
            {
                short seed = (short)Math.Floor((double)this.initialSeed.Value);

                string query = "sim_id = " + currentSimulation.id;

                DataRow[] simseeds = theDb.Data.scenario_simseed.Select(query, "", DataViewRowState.CurrentRows);

                if (simseeds.Length > 0)
                    simseeds[0]["seed"] = seed;
                else
                    // create seed with value
                    theDb.CreateScenarioSimSeed(currentSimulation, seed);
            }
        }

    }
}
