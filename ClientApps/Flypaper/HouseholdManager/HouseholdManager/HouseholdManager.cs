using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

using HouseholdLibrary;
using SimInterface;
using GeoLibrary;
using PopulationGenerator;
using MediaManager;
using DemographicLibrary;
using Calibration;
using MediaLibrary;

namespace HouseholdManager
{
    public partial class HouseholdManagerUI : Form
    {

        #region initialization

        static HouseholdManagerUI()
        {
            GeoInfo.ReadFromFile(Properties.Settings.Default.hhinput_directory);
        }

        public HouseholdManagerUI()
        {
            InitializeComponent();

            // UpdateStatus = new UpdateDelegate( updateStatus );

            agentUpdate1.UpdateStatus = new UpdateDelegate( updateStatus );

            mediaBuilder1.UpdateStatus = new UpdateDelegate( updateStatus );
        }

        #endregion

        #region delegates and messaging

        public delegate void UpdateDelegate( string message);
        // private UpdateDelegate UpdateStatus;

        private void updateStatus( string message)
        {
            if( statusBox.Text.Length > 10000 )
            {
                statusBox.Text = statusBox.Text.Substring( 0, 8000 );
            }

            statusBox.Text = message + "\r\n" + statusBox.Text;
        }

        #endregion


        #region tools

        private void createSimulationToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SimCreator sim_create = new SimCreator();

            sim_create.Show();
        }

     
        #endregion

    }
}
