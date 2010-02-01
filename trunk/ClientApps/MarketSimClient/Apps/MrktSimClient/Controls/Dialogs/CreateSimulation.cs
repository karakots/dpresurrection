using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class CreateSimulation : Form
    {
        public new string Name
        {
            get
            {
                return nameBox.Text;
            }
        }

        public SimulationDb.SimulationType Type
        {
            get
            {
                return (SimulationDb.SimulationType) scenarioTypeBox.SelectedValue;
            }

            set
            {
                scenarioTypeBox.SelectedValue = value;
            }
        }

        public void DisAllowedType(SimulationDb.SimulationType[] types)
        {
            scenarioTypeView.RowFilter = "";

            foreach (SimulationDb.SimulationType type in types)
            {
                if (scenarioTypeView.RowFilter == "")
                {
                  scenarioTypeView.RowFilter = "id  <> " + ((int)type).ToString();
                }
                else
                {
                    scenarioTypeView.RowFilter += " AND id  <> " + ((int)type).ToString();
                }
            }
        }

        public CreateSimulation()
        {
            InitializeComponent();

            scenarioTypeView = new DataView(SimulationDb.simulation_type);
            scenarioTypeBox.DataSource = scenarioTypeView;
            scenarioTypeBox.DisplayMember = "type";
            scenarioTypeBox.ValueMember = "id";
        }

        DataView scenarioTypeView = null;

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {

            if (this.nameBox.Text == null || this.nameBox.Text == "")
            {
                MessageBox.Show(this, "Please enter a name for the simulation");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void scenarioTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView sel = (DataRowView) scenarioTypeBox.SelectedItem;

            DataRow row = sel.Row;

            this.descr.Text = MarketSimUtilities.MrktSimControl.MrktSimMessage("Simulation.type." + row["type"].ToString());
        }
    }
}