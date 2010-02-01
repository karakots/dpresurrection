using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdPlanitSimController
{
    public partial class SimulationGlobals : Form
    {
        GlobalData GlobleSimulationSettings;
        public SimulationGlobals(GlobalData glob)
        {
            InitializeComponent();

            GlobleSimulationSettings = glob;

            PersuasionDecayUpDown.Value = (decimal) GlobleSimulationSettings.persuasion_decay;
            AwarenessDecayUpDown.Value = (decimal) GlobleSimulationSettings.awareness_decay;
            RecencyDecayUpDown.Value = (decimal) GlobleSimulationSettings.recency_decay;
            PersuasionSaturationUpDown.Value = (decimal) GlobleSimulationSettings.saturation_coeff;
            CategoryConstantUpDown.Value = (decimal) GlobleSimulationSettings.category_constant;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            GlobleSimulationSettings.persuasion_decay = (double) PersuasionDecayUpDown.Value;
            GlobleSimulationSettings.awareness_decay = (double) AwarenessDecayUpDown.Value;
            GlobleSimulationSettings.recency_decay = (double) RecencyDecayUpDown.Value;
            GlobleSimulationSettings.saturation_coeff = (double) PersuasionSaturationUpDown.Value;
            GlobleSimulationSettings.category_constant = (double) CategoryConstantUpDown.Value;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.Close();
        }
    }


    [Serializable]
    public class GlobalData
    {
        public double persuasion_decay = 0.02;
        public double saturation_coeff = 0.02;
        public double category_constant = 100;
        public double awareness_decay = 0.02;
        public double recency_decay = 0.3;
    }

}
