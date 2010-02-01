using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimFrame;
using SimInterface;
using HouseholdLibrary;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

using GeoLibrary;

namespace FlySim
{
    public class AgentSim : Simulation
    {

        private Movie movie = new Movie();

        private static Random rand = new Random();
        private List<Agent> agents;
        private List<AgentData> agent_data;

        private List<List<MediaComp>> media_input;

        private Dictionary<Demographic, Dictionary<string, Metric>> outputs;
        private int num_outputs;

        public AgentSim(List<Agent> agents)
        {
            this.agents = agents;
            agent_data = new List<AgentData>();
            foreach (Agent agent in agents)
            {
                AgentData data = new AgentData( agent );

                agent_data.Add(data);
            }

            media_input = new List<List<MediaComp>>();
            outputs = new Dictionary<Demographic, Dictionary<string, Metric>>();
        }

        protected override Simulation.SimError Reset()
        {
            try
            {
                foreach( AgentData data in agent_data )
                {
                    data.Reset();

                    double interval = (0.5 * Input.PurchaseInterval) + (0.5 * rand.Next( Input.PurchaseInterval ));

                    if( interval > 1 )
                    {
                        data.shopProb = 1.0 / interval;
                    }


                    // add demographics to agent data
                    int demoIndex = 0;
                    foreach( Demographic demo in Input.Demographics )
                    {
                        if( data.AddDemographic( demo ) )
                        {
                            data.demoIndex = demoIndex;
                        }
                        demoIndex++;
                    }
                }
                foreach( List<MediaComp> media_list in media_input )
                {
                    media_list.Clear();
                }
                media_input.Clear();

                for( int i = Day; i <= Input.EndDate; i++ )
                {
                    media_input.Add( new List<MediaComp>() );
                    foreach( MediaComp media in Input.Media )
                    {
                        if( media.ImpressionsOnDay( i ) > 0 )
                        {
                            media_input[i].Add( media );
                        }
                    }
                }

                foreach( Dictionary<string, Metric> dict in outputs.Values )
                {
                    dict.Clear();
                }
                outputs.Clear();

                foreach( Demographic demographic in Input.Demographics )
                {
                    outputs.Add( demographic, new Dictionary<string, Metric>() );
                    outputs[demographic].Add( "Persuasion", new Metric() );
                    outputs[demographic].Add( "Awareness", new Metric() );
                    outputs[demographic].Add( "Recency", new Metric() );
                    outputs[demographic].Add( "Size", new Metric() );
                }
                num_outputs = 0;
            }
            catch(Exception e)
            {
                return e.Message;
            }

            if( this.Option == "MOVIE" )
            {
                movie.InitMovie();
            }

            return SimError.NoError;
        }

        protected override Simulation.SimError Step()
        {
            DateTime start = DateTime.Now;

            try
            {
                for( int i = 0; i < media_input[Day].Count; i++ )
                {
                    double numInpressions = media_input[Day][i].ImpressionsOnDay( Day );

                    for( int x = 0; x < numInpressions; x++ )
                    {
                        if( (DateTime.Now - start).TotalSeconds > 10 )
                        {
                            return "Too Long in loop: impressions = " + numInpressions.ToString();
                        }

                        int index = rand.Next( agent_data.Count );
                        agent_data[index].ApplyImpression( media_input[Day][i].Guid );

                        //
                        // backed this out it was taking a real long time
                        //double max = 1000 * numInpressions;
                        //while( !agent_data[index].ApplyImpression( media_input[Day][i] ) && max > 0 )
                        //{
                        //    index = rand.Next( agent_data.Count );
                        //    max--;

                        //    // if we ar etaking too long then break

                        //    if( (DateTime.Now - start).TotalSeconds > 10 )
                        //    {
                        //        return "Too Long in loop: impressions = " + numInpressions.ToString();
                        //    }

                        //}
                    }
                }
            }
            catch( Exception oops )
            {
                return oops.Message;
            }

            for( int a = 0; a < agents.Count; a++ )
            {
                try
                {
                    agent_data[a].Decay();
                }
                catch( Exception e )
                {
                    return e.Message;
                }
            }

            try
            {
                write_output();
            }
            catch( Exception oops2 )
            {
                return oops2.Message;
            }


            return SimError.NoError;
        }

        private void write_output()
        {
            foreach( Demographic demographic in outputs.Keys )
            {
                outputs[demographic]["Size"].values.Add( 0 );
                outputs[demographic]["Persuasion"].values.Add( 0 );
                outputs[demographic]["Awareness"].values.Add( 0 );
                outputs[demographic]["Recency"].values.Add( 0 );
            }

            foreach( AgentData data in agent_data )
            {
                foreach( Demographic demographic in data.Demographics )
                {
                    outputs[demographic]["Size"].values[num_outputs] += 1;
                    outputs[demographic]["Persuasion"].values[num_outputs] += data.Persuasion;
                    if( data.Aware )
                    {
                        outputs[demographic]["Awareness"].values[num_outputs] += 1;
                    }
                    outputs[demographic]["Recency"].values[num_outputs] += data.Recency;

                }
            }


            if( this.Option == "MOVIE" )
            {
                movie.CreateMovieFile( agent_data );
            }

            num_outputs++;
        }

        protected override Simulation.SimError Compute()
        {

            if (Option == "MOVIE")
            {
                movie.EndMovie();
            }

            try
            {
                foreach( Demographic demographic in outputs.Keys )
                {
                    Metric persuasion = outputs[demographic]["Persuasion"];
                    Metric awareness = outputs[demographic]["Awareness"];
                    Metric recency = outputs[demographic]["Recency"];
                    for( int i = 0; i < num_outputs; i++ )
                    {
                        if( outputs[demographic]["Size"].values[i] > 0 )
                        {
                            persuasion.values[i] = persuasion.values[i] / outputs[demographic]["Size"].values[i];
                            awareness.values[i] = awareness.values[i] / outputs[demographic]["Size"].values[i];
                            recency.values[i] = recency.values[i] / outputs[demographic]["Size"].values[i];
                        }
                    }

                    persuasion.Span = 1;
                    persuasion.Segment = demographic.Name;
                    persuasion.Type = "Persuasion";

                    awareness.Span = 1;
                    awareness.Segment = demographic.Name;
                    awareness.Type = "Awareness";

                    recency.Span = 1;
                    recency.Segment = demographic.Name;
                    recency.Type = "Recency";

                    Output.metrics.Add( persuasion );
                    Output.metrics.Add( awareness );
                    Output.metrics.Add( recency );
                }
            }
            catch( Exception oops )
            {
                return oops.Message;
            }
            return SimError.NoError;
        }

        

   

    }
}
