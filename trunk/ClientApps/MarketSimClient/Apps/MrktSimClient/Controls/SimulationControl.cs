using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MrktSimDb;

namespace MrktSimClient.Controls
{
    public partial class SimulationControl : UserControl
    {
        private MsSimulationNode node;
        public MsSimulationNode Node
        {
            get
            {
                return node;
            }

            set
            {
                node = value;
            }
        }

        public MrktSimDBSchema.simulationRow Simulation
        {
            get
            {
                return Node.Simulation;
            }
        }

        public SimulationControl()
        {
            InitializeComponent();
        }

        private void runSim_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void runSimButton_Click(object sender, EventArgs e)
        {
            Node.Simulation.sim_num = 1;
            //Node.Db.CreateRun(Node.Simulation);
            Node.Db.Update();

        }
    }
}
