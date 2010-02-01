using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimFrame;
using SimInterface;

using HouseholdLibrary;
using DemographicLibrary;

namespace TestSim
{
    class TestSim : Simulation
    {
        static void Main( string[] args ) {

            TestSim sim = new TestSim();

            SimRun frame = new SimRun(sim, null);

            // uncomment line to add own server
            // frame.SetDataSource( "CHAOS" );

            Console.WriteLine( "TestSim simulation server started" );

            frame.Run();
        }

        static int stepCount = 0;

        protected override SimError Step() {
            System.Threading.Thread.Sleep( 400 );

            Console.WriteLine( "Sim step {0}...", ++stepCount );
            if( (stepCount % 10) == 0 ) {
                Console.WriteLine( "" );
            }

            foreach( Metric metric in Output.metrics ) {

                if( metric.Segment == "All" ) {
                    metric.values.Add( (float)(Day / 100.0) );
                }
                else {
                    metric.values.Add( (float)Math.Cos( Day / 100.0 ) );
                }
            }

            return SimError.NoError;
        }

        protected override SimError Reset() {

            Console.WriteLine( "Starting new simulation");

            stepCount = 0;

            Metric metric = new Metric();

            metric.Segment = "All";

            metric.Type = "Awareness";

            Output.metrics.Add( metric );

            foreach( Demographic seg in Input.Demographics ) {
                Metric segMetric = new Metric();
                segMetric.Segment = seg.Name;
                segMetric.Type = "persuasion";
                Output.metrics.Add( segMetric );
            }

            return SimError.NoError;
        }

        protected override SimError Compute() {
            Console.WriteLine( "Simulation Done" );
            return SimError.NoError;
        }
    }
}
