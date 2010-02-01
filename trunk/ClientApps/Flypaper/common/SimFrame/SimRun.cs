using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimInterface;

namespace SimFrame
{

  

    /// <summary>
    /// a frame work for runing a simulation
    /// </summary>
    public class SimRun
    {
        public enum Status
        {
            Waiting,
            Running,
            Done
        }

        public class SimStatus : EventArgs
        {
            public Status state = Status.Waiting;
            public string message = "";
            public bool cancel = false;
        }

        
        public delegate void UpdateSimStatus(SimStatus status);

        public event UpdateSimStatus UpdateUI;


        /// <summary>
        /// The simulation to run
        /// </summary>
        private Simulation sim;
        private DpSimQueue.DpUpdate dp;
        public bool Calibration { get; set; }

        public SimRun(Simulation aSim, string connectionString) {
            dp = new DpSimQueue.DpUpdate( connectionString );
            sim = aSim;
            Calibration = false;
        }

        SimStatus status = new SimStatus();

        private void updateStatus(ref DateTime refTime )
        {
            double secs = (DateTime.Now - refTime).TotalSeconds;

            if( refTime == null || secs > 2 )
            {
                if( UpdateUI != null )
                {
                    UpdateUI(status );
                }

                if( refTime != null )
                {
                    refTime = DateTime.Now;
                }
            }
        }

        public void Run() {

            bool done = false;
            DateTime refTime = DateTime.Now;
            status.state = Status.Waiting;

            while( !done ) {
                // check for sims to run in the database
                Guid? simId = dp.GetSimToRun(Calibration);

                if( simId.HasValue )
                {
                    status.state = Status.Running;
                    status.message = "Initializing sim ";
                    UpdateUI( status );
                    
                    done = status.cancel;

                    DateTime start = DateTime.Now;

                    if( !dp.SetSimToRun( simId.Value ) )
                    {
                        // was not able to change the state
                        continue;
                    }

                    if( UpdateUI != null )
                    {
                        status.message = "Reading Data";
                        UpdateUI( status );
                    }

                    // get data
                    try
                    {
                        sim.SimIn = dp.GetInput( simId.Value );
                    }
                    catch(Exception oops)
                    {
                        if( UpdateUI != null )
                        {
                            status.message = "err reading input: " + oops.Message;
                            UpdateUI( status );
                        }
                       
                        dp.UpdateProgress( simId.Value, 1 );
                        continue;
                    }

                   
                    // write agent data
                    try
                    {
                        dp.UpdateData( simId.Value, sim.Data );
                    }
                    catch( Exception oops2 )
                    {
                        if( UpdateUI != null )
                        {
                            status.message = "err writing agent data: " + oops2.Message;
                            UpdateUI( status );
                        }

                        dp.UpdateProgress( simId.Value, 1 );
                        continue;
                    }
                   

                    if( sim.Option != null )
                    {
                        if( UpdateUI != null )
                        {
                            status.message = "option = " + sim.Option;
                            UpdateUI( status );
                        }
                    }

                    if( UpdateUI != null )
                    {
                        status.message = "Starting sim loop";
                        UpdateUI( status );
                    }

                    while( !sim.Done && !done)
                    {

                        double progress = sim.Progress;

                        dp.UpdateProgress( simId.Value, progress );

                        Simulation.SimError error = sim.SimStep();
                        if( error )
                        {
                            if( UpdateUI != null )
                            {
                                status.message = error.Message;
                                UpdateUI( status );
                            }
                        }


                        status.message = "Sim Step day = " + sim.Day.ToString();
                        updateStatus( ref refTime );
                        done = status.cancel;
                    }

                    if( UpdateUI != null )
                    {
                        DateTime end = DateTime.Now;
                        TimeSpan span = end - start;

                        status.message = "Sim done. Elapsed time (secs) = " + span.TotalSeconds;
                        UpdateUI(status);
                    }

                    try
                    {
                        dp.UpdateResults( simId.Value, sim.SimOut );

                        dp.UpdateData( simId.Value, sim.Data );
                    }
                    catch( Exception errorOut )
                    {
                        if( UpdateUI != null )
                        {
                            status.message = "err reading input: " + errorOut.Message;
                            UpdateUI( status );
                        }

                        dp.UpdateProgress( simId.Value, 1 );
                        continue;
                    }

                    // done running
                    dp.SetSimState( simId.Value, 2 );
                }
                else
                {
                    status.state = Status.Waiting;
                    status.message = "Waiting";
                    updateStatus( ref refTime );
                    done = status.cancel;
                }
            }

            if( UpdateUI != null )
            {
                status.state = Status.Done;
                status.message = "Simulation done";

                UpdateUI( status );
            }
        }
    }
}
