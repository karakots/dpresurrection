using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DpSimQueue;
using SimulationControlPanel.Dialogues;

namespace SimulationControlPanel
{
    public partial class SimulationControlPanel : Form
    {
        public SimulationControlPanel() {
            InitializeComponent();

            clock.Tick +=new EventHandler(clock_Tick);

            simGrid.DataError += new DataGridViewDataErrorEventHandler( simGrid_DataError );
       
          simGrid.DataSource = queue.sim_queue;
          ////  simGrid.DataMember = "sim_queue";

            loadData();

            clock.Interval = (int) (1000 * updateTime.Value);

            clock.Start();
        }

        void simGrid_DataError( object sender, DataGridViewDataErrorEventArgs e ) {
        }

        #region private fields

        // the data set
        DpSimQueue.SimQueue queue = new SimQueue();

        private DpUpdate dp = new DpUpdate();

        private Timer clock = new Timer();

        #endregion

        #region basic control
        void clock_Tick( object sender, EventArgs e ) {

            SimControlRefresh();
        }

        #endregion

        private void loadData() {

            dp.Refresh( queue.sim_queue );

            queue.AcceptChanges();
        }


        private void SimControlRefresh() {

            try {

                if( queue.HasChanges() ) {
                    dp.Update( queue.sim_queue );
                    queue.AcceptChanges();
                }

                dp.RefreshSinceLast( queue.sim_queue );
            }
            catch( Exception ) { }
        }

        private void createSim_Click( object sender, EventArgs e ) {
            CreateTestSim dlg = new CreateTestSim();

            dlg.Show();

        }

        private void updateTime_ValueChanged( object sender, EventArgs e ) {

            if( updateTime.Value == 0 ) {
                clock.Stop();
            }
            else {
                clock.Interval = (int) (1000 * updateTime.Value);

                clock.Start();
            }
        }
    }
}
