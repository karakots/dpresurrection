using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DpSimQueue;
using SimInterface;

using HouseholdLibrary;
using DemographicLibrary;

namespace SimControlForm
{
    public partial class SimControl : Form
    {
   
        /// <summary>
        /// Test bed for connecitng to database and running sim
        /// </summary>
        public SimControl() {
            InitializeComponent();

            // uncomment this line and replace with name of server
            // dp.LocalDataSource = "CHAOS";

            clock.Interval = 500;

            clock.Tick += new EventHandler( clock_Tick );

            runSimBut.Enabled = false;
        }

        #region private fields
        private DpUpdate dp = new DpUpdate();

        private Guid? simId = null;

        private Timer clock = new Timer();

        #endregion

        #region basic control
        void clock_Tick( object sender, EventArgs e ) {

            if( !simId.HasValue ) {
                return;
            }

            progressBar.Value = (int) (100 * dp.GetSimProgress( simId.Value ));

            bool done = dp.SimDone( simId.Value );

            if( done ) {
                clock.Stop();

                createSimBut.Enabled = true;

                // get data
                SimOutput output = dp.GetResults( simId.Value );

                foreach( Metric metric in output.metrics ) {
                    resultsBox.Text += metric.Type + "\r\n";
                    resultsBox.Text += metric.Segment + "\r\n";

                    foreach( float val in metric.values ) {

                        resultsBox.Text += "," + val.ToString();
                    }
                    resultsBox.Text += "\r\n";
                }
            }
        }

        private void createSimBut_Click( object sender, EventArgs e ) {

            simId = dp.CreateSimulation("simTestbed");
          
            // create some dummy input for now
            SimInput input = new SimInput();

            input.EndDate = 15;

            MediaComp comp = new MediaComp();

            comp.StartDate = 10;

            comp.Impressions = 1000;

            input.Media.Add( comp );

            comp = new MediaComp();

            comp.StartDate = 17;

            comp.Impressions = 100;

            input.Media.Add( comp );

            Demographic seg = new Demographic();
           

            seg.Age = "18to25";
            seg.Race = "LATINO";
            seg.Gender = "MALE";
            seg.Name = "Young Hispanics";

            input.Demographics.Add( seg );

            dp.UpdateInput( simId.Value, input );

            runSimBut.Enabled = true;
            createSimBut.Enabled = false;
          
        }

        private void runSimBut_Click( object sender, EventArgs e ) {

            if( simId.HasValue ) {
                dp.QueueSim( simId.Value );

                resultsBox.Text = "";
                createSimBut.Enabled = false;
                runSimBut.Enabled = false;

                clock.Start();
            }
        }

        #endregion
    }
}
