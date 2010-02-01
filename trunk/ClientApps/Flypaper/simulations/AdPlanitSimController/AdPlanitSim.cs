using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using FlySim;
using SimFrame;

using DpSimQueue;
using MediaLibrary;

namespace AdPlanitSimController
{
    public partial class AdPlanitSim : Form
    {
        private MediaSim simulation = null;
        private SimRun sim_frame;
        private Thread thread;

        private SimRun.UpdateSimStatus UpdateSimStatus;

        private bool stopSim = false;

        private SimRun.Status runState = SimRun.Status.Waiting;

        public AdPlanitSim( List<Agent> agents, string connectionString, GlobalData glob, bool calibrate)
        {
            InitializeComponent();

            UpdateSimStatus = new SimRun.UpdateSimStatus( sim_frame_UpdateUI );

            simulation = new MediaSim( agents );

            AgentData.PersuasionDecay = glob.persuasion_decay;
            simulation.SaturationCoefficient = glob.saturation_coeff;
            simulation.CategoryConstant = glob.category_constant;
            AgentData.AwareDecay = glob.awareness_decay;
            AgentData.RecencyDecay = glob.recency_decay;

            sim_frame = new SimRun( simulation, connectionString );
            sim_frame.Calibration = calibrate;

            sim_frame.UpdateUI += new SimRun.UpdateSimStatus( InvokeSimStatusUpdate );

            thread = new Thread( new ThreadStart( sim_frame.Run ) );

            thread.Start();
        }

        public bool Alive
        {
            get
            {
                return thread != null && thread.IsAlive;
            }
        }

        private void Stop()
        {
            stopSim = true;

            if(thread.IsAlive )
            {
                // thread is runnning - tell it to stop
               

                // change display
            }
        
        
        }

        private void InvokeSimStatusUpdate( SimRun.SimStatus status )
        {
            try
            {
                object[] parms = new object[] { status };

                this.Invoke( UpdateSimStatus, parms );
            }
            catch( Exception ) { }
        }

        void sim_frame_UpdateUI( SimRun.SimStatus status )
        {
            // display status

            runState = status.state;

            if( runState == SimRun.Status.Waiting )
            {
                if( simState.BackColor != Color.LightGreen )
                {
                    simState.BackColor = Color.LightGreen;
                }
                else
                {
                    if( sim_frame.Calibration )
                    {
                        simState.BackColor = Color.Tomato;
                    }
                    else
                    {
                        simState.BackColor = Color.Cyan;
                    }
                }

                // wwiting - look busy
                if( sim_frame.Calibration )
                {
                    this.Text = "Waiting for " + DpUpdate.Calibrator;
                }
                else
                {
                    this.Text = "Filtering:  " + DpUpdate.Calibrator;
                }
            }
            else if( runState == SimRun.Status.Done )
            {
                simState.BackColor = Color.Red;

                this.Text = "GOOD BYE";

                this.Close();
            }
            else
            {
                simState.BackColor = SystemColors.Control;

                if( this.WindowState == FormWindowState.Minimized )
                {
                    this.WindowState = FormWindowState.Normal;
                }

                // show states
                if( simState.Text.Length > 1000 )
                {
                    simState.Text = simState.Text.Substring( 0, 800 );
                }

                simState.Text = status.message + "\r\n" + simState.Text;

            }

            status.cancel = stopSim;
        }

        private void AdPlanitSim_FormClosing( object sender, FormClosingEventArgs e )
        {
              if(thread.IsAlive )
            {
                if( runState != SimRun.Status.Done )
                {
                    e.Cancel = true;
                    Stop();
                }
            }
        }
    }
}
