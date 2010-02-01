using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HouseholdManager
{
    public partial class SimulationGlobals : Form
    {
        public SimulationGlobals()
        {
            InitializeComponent();
        }

        public double PersuasionDecay
        {
            set
            {
                PersuasionDecayUpDown.Value = (Decimal)value;
            }
            get
            {
                return (double)PersuasionDecayUpDown.Value;
            }
        }

        public double AwarenessDecay
        {
            set
            {
                AwarenessDecayUpDown.Value = (Decimal)value;
            }
            get
            {
                return (double)AwarenessDecayUpDown.Value;
            }
        }

        public double RecencyDecay
        {
            set
            {
                RecencyDecayUpDown.Value = (Decimal)value;
            }
            get
            {
                return (double)RecencyDecayUpDown.Value;
            }
        }

        public double PersuasionSaturation
        {
            set
            {
                PersuasionSaturationUpDown.Value = (Decimal)value;
            }
            get
            {
                return (double)PersuasionSaturationUpDown.Value;
            }
        }

        public double CategoryConstant
        {
            set
            {
                CategoryConstantUpDown.Value = (Decimal)value;
            }
            get
            {
                return (double)CategoryConstantUpDown.Value;
            }
        }
    }
}
