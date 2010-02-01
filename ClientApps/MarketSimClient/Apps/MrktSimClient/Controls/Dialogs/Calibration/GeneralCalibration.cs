using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections; //Arraylist

using MrktSimDb;
using MrktSimDb.Metrics;

namespace MrktSimClient.Controls.Dialogs.Calibration
{
    public partial class GeneralCalibration : UserControl
    {
        public GeneralCalibration() {
            InitializeComponent();
        }

        private MrktSimDBSchema.simulationRow currentSimulation = null;
        private CallibrationDb theDb;

        public MrktSimDBSchema.simulationRow Simulation
        {
            set
            {
                currentSimulation = value;
            }
        }

        public CallibrationDb Db
        {
            set
            {
                theDb = value;
            }
        }

    }
}
