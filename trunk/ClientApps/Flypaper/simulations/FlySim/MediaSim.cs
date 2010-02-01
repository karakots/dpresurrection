using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimFrame;
using SimInterface;
using HouseholdLibrary;
using GeoLibrary;
using MediaLibrary;
using DemographicLibrary;

using System.Runtime.Serialization.Formatters.Binary;
using Utilities;

using System.IO;

namespace FlySim
{
    public class MediaSim : Simulation
    {
        static public Dictionary<int, AdOption> ad_options = null;
        private Dictionary<int, AdOption> options = ad_options;

        private Movie movie = new Movie();

        private static Random rand = new Random();

        /// <summary>
        /// The raw household data
        /// </summary>
        private List<Agent> agents;

        /// <summary>
        /// All the agents active in the simulation
        /// </summary>
        private List<int> active_agents;

        /// <summary>
        /// The simulation information for the agent at the cooresponding index
        /// </summary>
        private List<AgentData> agent_data;

        
        /// <summary>
        /// Stores the media for each day of the simulation
        /// Index: Day of simulation
        ///     Key: Media guid
        ///     Value: List of media components
        /// </summary>
        private List<Dictionary<Guid,List<MediaComp>>> media_input;

        /// <summary>
        /// Used for tracking the daily metrics
        /// Key: Demographic region pair
        /// Value:
        ///     Key: Metric name
        ///     Value: Metric data
        /// </summary>
        private Dictionary<Demographic, Dictionary<string, Metric>> outputs;
        private int num_outputs;

        /// <summary>
        /// Used for reporting the status of the simulation
        /// </summary>
        private int num_active_agents;
        private double avg_media;

        /// <summary>
        /// Used to scale metrics
        /// </summary>
        private Dictionary<Demographic, int> demo_sizes;

        /// <summary>
        /// Used to calculate reach and efficency
        /// </summary>
        private int num_media_agents;
        private int num_demo_agents;

        /// <summary>
        /// Data structure to store the dialy log for the agent at the cooresponding index
        /// Key: Index of agent
        /// Value: Daily log
        /// </summary>
        private Dictionary<int, AgentLog> agent_logs;

        public MediaSim(List<Agent> agents)
        {
            //Initialize data
            this.agents = agents;
            agent_data = new List<AgentData>();
            AgentData.Max_Impressions = 4;
            media_input = new List<Dictionary<Guid, List<MediaComp>>>();
            outputs = new Dictionary<Demographic, Dictionary<string, Metric>>();
            demo_sizes = new Dictionary<Demographic, int>();
            num_media_agents = 0;

            //Set up agent data for each agent
            foreach (Agent agent in agents)
            {
                AgentData data = new AgentData(agent);
                agent_data.Add(data);
            }
            active_agents = new List<int>();

            
            
            //Set global variables
            SaturationCoefficient = 1;
            CategoryConstant = 100;
        }

        /// <summary>
        /// The amount of persuasion the simulation saturates at
        /// </summary>
        public double SaturationCoefficient { get; set; }

        /// <summary>
        /// The competitive constant for the simulation
        /// </summary>
        public double CategoryConstant { get; set; }


        /// <summary>
        /// Delegate for getting status information from the simulation
        /// </summary>
        /// <param name="info"></param>
        public delegate void WriteSimInfo(string info);

        /// <summary>
        /// Event triggered when the simulation reports information
        /// </summary>
        public event WriteSimInfo WriteInfo;

        protected override Simulation.SimError Reset()
        {

            // set up ad options dugin calibration
            options = ad_options;

            if( Input.calOptions != null )
            {
                options = Input.calOptions;
            }

            //Temp option for getting agent information
            // Input.option = "MICROSCOPE";


            //Clear the data form the last sim
            active_agents.Clear();
            num_active_agents = 0;
            avg_media = 0.0;
            AgentData.ImpressionLength = Input.PurchaseInterval;
            foreach (Dictionary<string, Metric> dict in outputs.Values)
            {
                dict.Clear();
            }
            outputs.Clear();
            demo_sizes.Clear();

            
            //Build the demographic information
            foreach (Demographic demographic in Input.Demographics)
            {
                demo_sizes.Add(demographic, 0);
                outputs.Add(demographic, new Dictionary<string, Metric>());
                outputs[demographic].Add("Persuasion", new Metric());
                outputs[demographic].Add("Awareness", new Metric());
                outputs[demographic].Add("Recency", new Metric());
                outputs[demographic].Add("Reach0", new Metric());
                outputs[demographic].Add("Reach1", new Metric());
                outputs[demographic].Add("Reach3", new Metric());
                outputs[demographic].Add("Reach4", new Metric());
                outputs[demographic].Add("PersuasionIndex", new Metric());
                outputs[demographic].Add("MarketIndex1", new Metric());
                outputs[demographic].Add("MarketIndex2", new Metric());
                outputs[demographic].Add("Consideration1", new Metric());
                outputs[demographic].Add("Consideration3", new Metric());
                outputs[demographic].Add("Consideration4", new Metric());
                outputs[demographic].Add("Reach", new Metric());
                outputs[demographic].Add("Efficency", new Metric()); 
                outputs[demographic].Add("Choosing", new Metric()); 
                outputs[demographic].Add("ActualShare", new Metric());
                outputs[demographic].Add("EstimatedShare", new Metric());
                outputs[demographic].Add("TotalActions", new Metric());

                // Adding metric: step 1 - add to list
                outputs[demographic].Add( "GRP-Total", new Metric() );
                foreach( MediaVehicle.MediaType type in MediaVehicle.MediaTypes )
                {
                    outputs[demographic].Add( MediaVehicle.MediaGrpString[type], new Metric() ); 
                }
            }

            //Calculate individual demographic sizes and total target size
            num_demo_agents = 0;
            for (int i = 0; i < agents.Count; i++)
            {
                bool matched = false;
                foreach (Demographic demographic in Input.Demographics)
                {

                    GeoRegion geoRegion = GeoRegion.TopGeo.GetSubRegion(demographic.Region);

                    if (geoRegion == null || geoRegion.ContainsGeoId(agents[i].House.GeoID))
                    {
                        //If match then add to the individual demographic size
                        if (agents[i].House.Match(demographic))
                        {
                            demo_sizes[demographic]++;
                            matched = true;
                        }
                    }
                }
                //If matched to any demographic then add to the total target size
                if (matched)
                {
                    num_demo_agents++;
                }
            }
            //Prevent a divide by zero when calculating reach
            num_demo_agents = Math.Max(num_demo_agents, 1);

            //Build a dictionary of all unique media components for each media component
            Dictionary<Guid, List<MediaComp>> media_guids = new Dictionary<Guid, List<MediaComp>>();
            foreach (MediaComp media in Input.Media)
            {
                //Scale the number of impressions by the US household size
                media.Impressions = Math.Ceiling((double)media.Impressions * ((double)agents.Count) / ((double)DpMediaDb.US_HH_SIZE));
                if (!media_guids.ContainsKey(media.Guid))
                {
                    media_guids.Add(media.Guid, new List<MediaComp>());
                }

                //Check if media component already exists
                bool found = false;
                foreach (MediaComp media_comp in media_guids[media.Guid])
                {
                    if (equal(media_comp, media))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    media_guids[media.Guid].Add(media);
                }
            }

            //Add media to all the agents, not limited by region...
            num_media_agents = 0;
            for(int i = 0; i < agents.Count; i++)
            {
                agent_data[i].active_media.Clear();
                //Loop through all the media to see if any match the agent
                foreach (Guid media_guid in media_guids.Keys)
                {
                    if (agents[i].HasMedia(media_guid) && !agent_data[i].active_media.Contains(media_guid))
                    {
                        //Check each media compenent for this media
                        foreach (MediaComp media in media_guids[media_guid])
                        {
                            //Check if there is geographic match
                            bool geo_match = true;
                            //If the target is empty assume agent matches, as we already know they access this media
                            if (media.target_regions.Count > 0)
                            {
                                geo_match = false;
                                //Check all regions
                                foreach (string region in media.target_regions)
                                {
                                    GeoRegion geo_region = GeoRegion.TopGeo.GetSubRegion(region);
                                    //Check if the agent is in the region or if the fuzz factors gives a false positive
                                    if (rand.NextDouble() < media.region_fuzz_factor || geo_region.ContainsGeoId(agents[i].House.GeoID))
                                    {
                                        geo_match = true;
                                        break;
                                    }
                                }
                            }

                            //Check for demographic match
                            bool demo_match = true;
                            //If the target is empty assume agent matches, as we already know they access this media
                            if (media.target_demogrpahic.Count > 0)
                            {
                                demo_match = false;
                                //Check all demographics
                                foreach (Demographic demographic in media.target_demogrpahic)
                                {
                                    //Check if the agent is in the demographic or if the fuzz factors gives a false positive
                                    if (rand.NextDouble() < media.demo_fuzz_factor || agents[i].House.Match(demographic))
                                    {
                                        demo_match = true;
                                        break;
                                    }
                                }
                            }

                            if (demo_match && geo_match)
                            {
                                //The agent matches so add the media to the agent data
                                agent_data[i].active_media.Add(media.Guid);
                                avg_media++;
                                //Break from component loop
                                break;
                            }
                        }
                    }
                }

                //The agent access some media used in this simulation so add them to list and reset the data
                if (agent_data[i].active_media.Count > 0)
                {
                    num_media_agents++;
                }
            }

            //Prevent a divide by zero when calculating efficency
            num_media_agents = Math.Max(num_media_agents, 1);
            
            //Filter out agents that don't meet the demographic or region criteria
            for (int i = 0; i < agents.Count; i++)
            {
                agent_data[i].Reset();

                bool found = false;
                foreach (Demographic demographic in Input.Demographics)
                {
                    GeoRegion geoRegion = GeoRegion.TopGeo.GetSubRegion(demographic.Region);

                    if (geoRegion == null || geoRegion.ContainsGeoId(agents[i].House.GeoID))
                    {
                        if (agents[i].House.Match(demographic))
                        {
                            agent_data[i].AddDemographic(demographic);

                            found = true;

                        }
                    }
                }

                if( found )
                {
                    active_agents.Add( i );
                }
            }

            //Temp until UI catches up
            if (Input.initial_persuasion.Count < 1)
            {
                Input.initial_persuasion.Add(new InitialPersuasion());
                Input.initial_persuasion[0].persuasion = 0.0;
                Input.initial_persuasion[0].percent_aware = 1.0;
            }


            //Now that we have the active agents assign the initial awareness and persuasion
            //First build vector for selecting persuasion
            List<double> selection = new List<double>();
            selection.Add(Input.initial_persuasion[0].percent_aware);
            for (int i = 1; i < Input.initial_persuasion.Count; i++)
            {
                selection.Add(selection[i - 1] + Input.initial_persuasion[i].percent_aware);
            }
            for (int i = 0; i < selection.Count; i++)
            {
                selection[i] /= selection.Last();
            }
            //Next loop through all the active agents
            foreach (int index in active_agents)
            {
                //Check is agent is aware
                if (rand.NextDouble() < Input.initial_awareness)
                {
                    agent_data[index].Aware = true;
                    //Now set persuasion
                    double a_random_number = rand.NextDouble();
                    int choice = 0;
                    for (choice = 0; choice < selection.Count; choice++)
                    {
                        if (a_random_number < selection[choice])
                        {
                            break;
                        }
                    }
                    if (choice == selection.Count)
                    {
                        choice = selection.Count - 1;
                    }
                    //Initial persuasion is stored as saturated persuasion so it needs to be converted back
                    double sp = Input.initial_persuasion[choice].persuasion;
                    agent_data[index].Persuasion = SaturationCoefficient * sp / (SaturationCoefficient - sp);
                }
            }

            //Build the agent logs from random active agents
            agent_logs = new Dictionary<int, AgentLog>();
            Bucket bucket = new Bucket(active_agents.Count);
            for (int i = 0; i < Input.microscope_size && !bucket.Empty; i++)
            {
                int index = active_agents[bucket.Draw];
                agent_logs.Add(index, new AgentLog(agent_data[index].agent));
            }

           
            
            //Reset the media inputs
            foreach (Dictionary<Guid, List<MediaComp>> dict in media_input)
            {
                dict.Clear();
            }
            media_input.Clear();

            //Build the media input
            //Add one input for each day of the simulation
            for (int i = 0; i <= Input.EndDate; i++)
            {
                media_input.Add(new Dictionary<Guid, List<MediaComp>>());
            }
            //For each media component find the active days and add to the media input
            foreach (MediaComp media in Input.Media)
            {
                for (int i = media.StartDate; i > 0 && i < media.StartDate + media.Span && i <= Input.EndDate; i++)
                {
                    if (!media_input[i].ContainsKey(media.Guid))
                    {
                        media_input[i].Add(media.Guid, new List<MediaComp>());
                    }
                    media_input[i][media.Guid].Add(media);
                }
            }

            //Reset the number of outputs to the first day
            num_outputs = 0;

            //Configure movie if option is set
            if (this.Option == "MOVIE")
            {
                movie.InitMovie();
            }

            //Write out simulation information
            avg_media /= (double)num_active_agents;

            if (WriteInfo != null)
            {
                WriteInfo("Active Agents: " + num_active_agents + " Avg Media: " + avg_media);
            }

            return Simulation.SimError.NoError;
        }

        /// <summary>
        /// Checks if two media components contain the same data
        /// </summary>
        /// <param name="comp1">First component to check</param>
        /// <param name="comp2">Second component to check</param>
        /// <returns></returns>
        private bool equal(MediaComp comp1, MediaComp comp2)
        {
            //First check the vehicle Guid
            if (comp1.Guid != comp2.Guid)
            {
                return false;
            }

            //Next check the ad option
            if (comp1.ad_option != comp2.ad_option)
            {
                return false;
            }

            //Quick check if the target regions and target demographics are the same length
            if (comp1.target_regions.Count != comp2.target_regions.Count || comp1.target_demogrpahic.Count != comp2.target_demogrpahic.Count)
            {
                return false;
            }

            //Check is the fuzz factors are equal
            if (comp1.demo_fuzz_factor != comp2.demo_fuzz_factor || comp1.region_fuzz_factor != comp2.region_fuzz_factor)
            {
                return false;
            }

            //Check if region and demographic target lists are the same
            if (comp1.target_demogrpahic == comp2.target_demogrpahic && comp1.target_regions == comp2.target_regions)
            {
                return true;
            }

            //If the above check fails, do a more through check of the list contents
            foreach (string region in comp1.target_regions)
            {
                if (!comp2.target_regions.Contains(region))
                {
                    return false;
                }
            }

            foreach (Demographic demographic in comp1.target_demogrpahic)
            {
                if (!comp2.target_demogrpahic.Contains(demographic))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The main simulation loop, steps through one day
        /// </summary>
        /// <returns></returns>
        protected override Simulation.SimError Step()
        {
            Simulation.SimError rval = Simulation.SimError.NoError;

            //Initialize a bucket of all active agents
            Bucket bucket = new Bucket(active_agents.Count);
            while(!bucket.Empty)
            {
                //Draw an active agent
                int index = active_agents[bucket.Draw];

                //Decay the agent
                //Performed first to keep recency from immediatly decaying before it is recorded
                agent_data[index].Decay();
                
                //Loop over all active media for this agent
                foreach (Guid mediaId in agent_data[index].active_media)
                {
                    //Check if media is active on this day
                    if( media_input[Day].ContainsKey( mediaId ) )
                    {
                        //For each active media component check for interaction with agent
                        foreach( MediaComp media_comp in media_input[Day][mediaId] )
                        {
                            AdOption option = options[media_comp.ad_option];
                            //Compute number of hours household interacts with media
                            double hours = agents[index].GetRate( mediaId);
                            //Check if the household interacts with the specific ad option while interacting with media
                            if( rand.NextDouble() < option.prob_of_ad( agent_data[index], hours, media_comp ) )
                            {
                                //If agent is using a log keep track of media interaction
                                if( agent_logs.ContainsKey( index ) )
                                {
                                    MediaRecord record;
                                    option.modify_agent( agent_data[index], hours, media_comp, out record );
                                    agent_logs[index].AddRecord( Day, record );
                                }
                                //Have the ad option modify the agent data
                                option.modify_agent( agent_data[index], hours, media_comp );
                            }
                           
                        //End media component loop         
                        }
                    }
                //End active media loop
                }

                //See if agent enters in consideration based on the purchase interval
                //Based on geometric distribution with mean lambda = (1-C)*T and probability of success P=1/(1+lambda)
               double chance = 1 / ((1.0 - (double)Input.ConsiderationInterval) * (double)Input.PurchaseInterval);
                //Safety Check
                if (Double.IsNaN(chance) || Double.IsInfinity(chance))
                {
                    chance = 1.0;
                }
                if (rand.NextDouble() < chance && agent_data[index].InConsideration == false)
                {
                    //Put the agent into consideration
                    agent_data[index].InConsideration = true;
                }

                //Check if the agent is in consideration
                bool inConsideration = agent_data[index].InConsideration;
                if( inConsideration )
                {
                    //Compute the chance of leaving consideration based ont he purchase interval times the ConsiderationInterval scalar
                    //Same as above except now the mean is C*T
                    chance = 1 / ((double)Input.PurchaseInterval * (double)Input.ConsiderationInterval);
                    //Safety Check
                    if (Double.IsNaN(chance) || Double.IsInfinity(chance))
                    {
                        chance = 1.0;
                    }
                    if (rand.NextDouble() < chance)
                    {
                        //Invoke choice logic to check for action
                        //First check awareness
                        agent_data[index].MadeChoice = true;
                        if (agent_data[index].Aware)
                        {
                            //Calculate saturated persuasion based on saturation
                            //use recency TBD
                            double sp = SaturationCoefficient * agent_data[index].Persuasion / (SaturationCoefficient + agent_data[index].Persuasion);
                            //Compute the chance of action from the saturated_persuasion and the category constant
                            double chance_action = Math.Exp(sp) / (Input.mu + Math.Exp(sp));
                            agent_data[index].ActionTaken = chance_action;
                        }

                        //Remove the agent from consideration
                        inConsideration = false;
                    }
                }

                //Record the end state for the agent if using log
                if (agent_logs.ContainsKey(index))
                {
                    agent_logs[index].UpdateState(Day, agent_data[index]);
                }

                // now set consideration state
                // we delay till now so state is correctly recorded for log
                agent_data[index].InConsideration = inConsideration;

             
            //End agent loop
            }

            //Write out the daily metrics
            write_output();

            return rval;
        }

        private void write_output()
        {
            //Add outputs for this day
            foreach (Demographic demographic in outputs.Keys)
            {
                //Initialize metrics to 0
                outputs[demographic]["Persuasion"].values.Add(0);
                outputs[demographic]["Awareness"].values.Add(0);
                outputs[demographic]["Recency"].values.Add(0);
                outputs[demographic]["Reach0"].values.Add(0);
                outputs[demographic]["Reach1"].values.Add(0);
                outputs[demographic]["Reach3"].values.Add(0);
                outputs[demographic]["Reach4"].values.Add(0);
                outputs[demographic]["PersuasionIndex"].values.Add(0);
                outputs[demographic]["MarketIndex1"].values.Add(0);
                outputs[demographic]["MarketIndex2"].values.Add(0);
                outputs[demographic]["Consideration1"].values.Add( 0 );
                outputs[demographic]["Consideration3"].values.Add( 0 );
                outputs[demographic]["Consideration4"].values.Add( 0 );
                outputs[demographic]["Choosing"].values.Add(0);
                outputs[demographic]["ActualShare"].values.Add(0);
                outputs[demographic]["EstimatedShare"].values.Add(0);
                outputs[demographic]["TotalActions"].values.Add(0);

                // Adding metric: step 2 - initialize to zero for the day
                outputs[demographic]["GRP-Total"].values.Add( 0 );
                foreach( MediaVehicle.MediaType type in MediaVehicle.MediaTypes )
                {
                    outputs[demographic][MediaVehicle.MediaGrpString[type]].values.Add( 0 );
                }

                //Initialize metrics to constants that represent the reach and efficency
                double reach = (double)active_agents.Count / (double)num_demo_agents;
                double efficency = (double)active_agents.Count / (double)num_media_agents;
                outputs[demographic]["Reach"].values.Add(reach);
                outputs[demographic]["Efficency"].values.Add(efficency);
            }

            //Variables for computing PersuasionIndex, MarketIndex1, and MarketIndex2
            // double U = 0;
            double V = 0;
            double C = CategoryConstant;

            //Loop over all active agents
            foreach(int i in active_agents)
            {
                //Get the agent data
                AgentData data = agent_data[i];

                //Get the number of impressions
                int num_impressions = data.NumImpressions();

                //Loop over all the cooresponding demographics
                foreach (Demographic demographic in data.Demographics)
                {
                  
                    // Adding metric: step 3 - compute for demographic

                    // GRP calc
                    int total = 0;
                    foreach( MediaVehicle.MediaType type in MediaVehicle.MediaTypes )
                    {
                        outputs[demographic][MediaVehicle.MediaGrpString[type]].values[num_outputs] += data.ImpressionsToday[type];
                        total += data.ImpressionsToday[type];
                    }

                    outputs[demographic]["GRP-Total"].values[num_outputs] += total;

                    //Check for awareness
                    if (data.Aware)
                    {
                        //Record awareness and raw persuasion
                        outputs[demographic]["Awareness"].values[num_outputs] += 1;
                        outputs[demographic]["Persuasion"].values[num_outputs] += data.Persuasion;

                        //Record saturated persuasion
                        //Use saturation coffiecient TBD
                        double sp = SaturationCoefficient * data.Persuasion / (SaturationCoefficient + data.Persuasion);
                        outputs[demographic]["PersuasionIndex"].values[num_outputs] += sp / SaturationCoefficient;

                        //Record MarketIndex1 and MarketIndex2
                        // SSN: U = sp
                        // U = (data.Persuasion * S) / (data.Persuasion + S);
                        // V = Math.Exp(U); 
                        V = Math.Exp( sp );
                        outputs[demographic]["MarketIndex1"].values[num_outputs] += V / (V + C);
                        outputs[demographic]["MarketIndex2"].values[num_outputs] += (V - 1)/(V + C);

                        //Record actual and estimated actions
                        if (data.MadeChoice)
                        {
                            outputs[demographic]["ActualShare"].values[num_outputs] += data.ActionTaken;
                            outputs[demographic]["TotalActions"].values[num_outputs] += data.ActionTaken * (DpMediaDb.US_HH_SIZE / agents.Count);
                        }
                        
                        outputs[demographic]["EstimatedShare"].values[num_outputs] += Math.Exp(sp) / (Input.mu + Math.Exp(sp));
                    }

                    //Record ad timiing and reach metrics
                    outputs[demographic]["Recency"].values[num_outputs] += data.Recency;
                    outputs[demographic]["Reach0"].values[num_outputs] += 1;
                    
                    if (num_impressions > 0)
                    {
                        outputs[demographic]["Reach1"].values[num_outputs] += 1;
                    }
                    if (num_impressions > 2)
                    {
                        outputs[demographic]["Reach3"].values[num_outputs] += 1;
                    }
                    if (num_impressions > 3)
                    {
                        outputs[demographic]["Reach4"].values[num_outputs] += 1;
                    }

                    if (data.MadeChoice)
                    {
                        outputs[demographic]["Choosing"].values[num_outputs] += 1;
                    }

                    if (data.ConsiderationImpressions > 0)
                    {
                        outputs[demographic]["Consideration1"].values[num_outputs] += 1;
                    }
                    if (data.ConsiderationImpressions > 2)
                    {
                        outputs[demographic]["Consideration3"].values[num_outputs] += 1;
                    }
                    if (data.ConsiderationImpressions > 3)
                    {
                        outputs[demographic]["Consideration4"].values[num_outputs] += 1;
                    }

                    
                }


            }

            //If the movie is option is set, create a picture
            if (this.Option == "MOVIE")
            {
                movie.CreateMovieFile(agent_data);
            }

            //Increment the number of outputs
            num_outputs++;
        }

        protected override Simulation.SimError Compute()
        {
            //If the movie option is set then end the movie
            if (Option == "MOVIE")
            {
                movie.EndMovie();
            }

            //If the microscope option is set then write the microscope files
            if (Option == "MICROSCOPE")
            {
                int i = 0;
                foreach (AgentLog log in agent_logs.Values)
                {
                    //Output.agent_microscope.Add(log);
                    string file_name = "Agent_Log" + i.ToString() + ".csv";
                    StreamWriter writer = new StreamWriter(file_name);
                    log.Print(writer);
                    writer.Close();
                    i++;
                }

            }

            //Get the total size
            int totalSize = active_agents.Count;
            if( totalSize == 0 )
            {
                totalSize = 1;
            }

            //Compute final persuasion
            double num_buckets = 100;
            double max_sp = Double.MinValue;
            //First find maximum for better partitioning
            foreach (int index in active_agents)
            {
                //Record saturated persuasion for better calibration of mu
                double sp = SaturationCoefficient * agent_data[index].Persuasion / (SaturationCoefficient + agent_data[index].Persuasion);
                if (sp > max_sp)
                {
                    max_sp = sp;
                }
            }
            //Initialized the final persuasion vector
            for (int i = 0; i < num_buckets; i++)
            {
                Output.final_persuasion.Add(new FinalPersuasion());
                Output.final_persuasion[i].persuasion = ((double)i) * (max_sp / num_buckets);
            }
            //Fill the final persuasion vector with aware agents
            foreach (int index in active_agents)
            {
                if (agent_data[index].Aware)
                {
                    double sp = SaturationCoefficient * agent_data[index].Persuasion / (SaturationCoefficient + agent_data[index].Persuasion);
                    int bucket = (int)(sp * (num_buckets - 1) / max_sp);
                    if( bucket >= 0 && bucket < Output.final_persuasion.Count )
                    {
                        Output.final_persuasion[bucket].num_agents++;
                    }
                }
            }

            //Loop through all demographics to calculate final persuasion, awareness, and recency
            foreach( Demographic demographic in outputs.Keys )
            {
                double size = demo_sizes[demographic];
                Metric persuasion = outputs[demographic]["Persuasion"];
                Metric persausion_index = outputs[demographic]["PersuasionIndex"];
                Metric recency = outputs[demographic]["Recency"];
                Metric actual_share = outputs[demographic]["ActualShare"];
                Metric estimated_share = outputs[demographic]["EstimatedShare"];
                Metric choosing = outputs[demographic]["Choosing"];
                Metric total_actions = outputs[demographic]["TotalActions"];
                Metric awareness = outputs[demographic]["Awareness"];
                for( int i = 0; i < num_outputs; i++ )
                {
                    if (size > 0)
                    {
                        //Scale persuasion and recency by the number of aware agents
                        if( awareness.values[i] > 0 )
                        {
                            persuasion.values[i] = persuasion.values[i] / awareness.values[i];
                            recency.values[i] = recency.values[i] / awareness.values[i];
                            persausion_index.values[i] = persausion_index.values[i] / awareness.values[i];
                        }
                        estimated_share.values[i] = estimated_share.values[i] / size;

                        //Scale actual actions by number of people who made a choice
                        if (choosing.values[i] > 0)
                        {
                            actual_share.values[i] = actual_share.values[i] / choosing.values[i];
                        }

                        awareness.values[i] = awareness.values[i] / size;

                        
                    }
                }

                //Set up the metrics for output
                persuasion.Span = 1;
                persuasion.Segment = demographic.Name;
                persuasion.Type = "Persuasion";

                persausion_index.Span = 1;
                persausion_index.Segment = demographic.Name;
                persausion_index.Type = "PersuasionIndex";

                awareness.Span = 1;
                awareness.Segment = demographic.Name;
                awareness.Type = "Awareness";

                recency.Span = 1;
                recency.Segment = demographic.Name;
                recency.Type = "Recency";

                actual_share.Span = 1;
                actual_share.Segment = demographic.Name;
                actual_share.Type = "ActualShare";

                estimated_share.Span = 1;
                estimated_share.Segment = demographic.Name;
                estimated_share.Type = "EstimatedShare";

                total_actions.Span = 1;
                total_actions.Segment = demographic.Name;
                total_actions.Type = "TotalActions";

                //Finally add the metrics to the output
                Output.metrics.Add(actual_share);
                Output.metrics.Add(estimated_share);
                Output.metrics.Add(total_actions);
                Output.metrics.Add( persuasion );
                Output.metrics.Add(persausion_index);
                Output.metrics.Add( awareness );
                Output.metrics.Add( recency );
            }

            //Loop through the demographics and calculate final impression statistics
            foreach (Demographic demographic in outputs.Keys)
            {
                double size = demo_sizes[demographic];
                Metric reach0 = outputs[demographic]["Reach0"];
                Metric reach1 = outputs[demographic]["Reach1"];
                Metric reach3 = outputs[demographic]["Reach3"];
                Metric reach4 = outputs[demographic]["Reach4"];
                Metric consideration1 = outputs[demographic]["Consideration1"];
                Metric consideration3 = outputs[demographic]["Consideration3"];
                Metric consideration4 = outputs[demographic]["Consideration4"];

                // Adding metric: step 4 - add to results
                Metric grpTotal = outputs[demographic]["GRP-Total"];
             
                for( int i = 0; i < num_outputs; i++ )
                {
                    if( size > 0 )
                    {
                        reach0.values[i] = reach0.values[i] / size;
                        reach1.values[i] = reach1.values[i] / size;
                        reach3.values[i] = reach3.values[i] / size;
                        reach4.values[i] = reach4.values[i] / size;
                        consideration1.values[i] = consideration1.values[i] / size;
                        consideration3.values[i] = consideration3.values[i] / size;
                        consideration4.values[i] = consideration4.values[i] / size;
                        grpTotal.values[i] = 100 * grpTotal.values[i] / size;
                    }
                }
                
                reach0.Span = 1;
                reach0.Segment = demographic.Name;
                reach0.Type = "Reach0";

                reach1.Span = 1;
                reach1.Segment = demographic.Name;
                reach1.Type = "Reach1";

                reach3.Span = 1;
                reach3.Segment = demographic.Name;
                reach3.Type = "Reach3";

                reach4.Span = 1;
                reach4.Segment = demographic.Name;
                reach4.Type = "Reach4";

                consideration1.Span = 1;
                consideration1.Segment = demographic.Name;
                consideration1.Type = "Consideration1";

                consideration3.Span = 1;
                consideration3.Segment = demographic.Name;
                consideration3.Type = "Consideration3";

                consideration4.Span = 1;
                consideration4.Segment = demographic.Name;
                consideration4.Type = "Consideration4";

                grpTotal.Span = 1;                       // Adding metric: step 5 - set type etc
                grpTotal.Segment = demographic.Name;
                grpTotal.Type = "GRP-Total";

                Output.metrics.Add(reach0);
                Output.metrics.Add(reach1);
                Output.metrics.Add(reach3);
                Output.metrics.Add(reach4);
                Output.metrics.Add(consideration1);
                Output.metrics.Add(consideration3);
                Output.metrics.Add(consideration4);
                Output.metrics.Add( grpTotal ); // Adding metric: step 6 - add to results - done

                // now do same for media impressions
                foreach( MediaVehicle.MediaType type in MediaVehicle.MediaTypes )
                {

                    // Adding metric: step 4 - add to results
                    Metric grpMedia = outputs[demographic][MediaVehicle.MediaGrpString[type]];

                    grpMedia.Span = 1;                       // Adding metric: step 5 - set type etc
                    grpMedia.Segment = demographic.Name;
                    grpMedia.Type = MediaVehicle.MediaGrpString[type];

                    for( int i = 0; i < num_outputs; i++ )
                    {
                        if( size > 0 )
                        {
                            grpMedia.values[i] = 100 * grpMedia.values[i] / size;
                        }
                    }

                    Output.metrics.Add( grpMedia ); // Adding metric: step 6 - add to results - done
                }
            }

            double C = CategoryConstant;
            double A = 0;

            foreach (Demographic demographic in outputs.Keys)
            {
                double size = demo_sizes[demographic];
                double scale1 = 1 + C / Math.Exp( SaturationCoefficient );
                Metric awareness = outputs[demographic]["Awareness"];
                
                Metric market_index1 = outputs[demographic]["MarketIndex1"];
                Metric market_index2 = outputs[demographic]["MarketIndex2"];
                for (int i = 0; i < num_outputs; i++)
                {
                    A = Output.metrics[1].values[i];
                    double scale2 = (Math.Exp( SaturationCoefficient ) + C) / (Math.Exp( SaturationCoefficient ) - A);
                    if (size > 0)
                    {
                        

                        //Scale MarketIndex1 and MarketIndex2 by market scaling factors
                        market_index1.values[i] = market_index1.values[i] * (scale1 / size);
                        market_index2.values[i] = market_index2.values[i] * (scale2 / size);
                    }
                }

                

                market_index1.Span = 1;
                market_index1.Segment = demographic.Name;
                market_index1.Type = "MarketIndex1";

                market_index2.Span = 1;
                market_index2.Segment = demographic.Name;
                market_index2.Type = "MarketIndex2";

                
                Output.metrics.Add(market_index1);
                Output.metrics.Add(market_index2);
            }

            //Loop through all demographics to calculate reach and efficency
            foreach (Demographic demographic in outputs.Keys)
            {
                double size = demo_sizes[demographic];
                Metric reach = outputs[demographic]["Reach"];
                Metric efficency = outputs[demographic]["Efficency"];

                //Set up the metrics for output
                reach.Span = 1;
                reach.Segment = demographic.Name;
                reach.Type = "Reach";

                efficency.Span = 1;
                efficency.Segment = demographic.Name;
                efficency.Type = "Efficency";

                //Finally add the metrics to the output
                Output.metrics.Add(reach);
                Output.metrics.Add(efficency);
            }


            return SimError.NoError;
        }

        public override byte[] Data
        {
            get
            {
                MemoryStream str = new MemoryStream();

                BinaryFormatter serializer = new BinaryFormatter();

                serializer.Serialize( str, agent_logs );

                byte[] buffer = StreamUtilities.Compress( str.GetBuffer() );

                return buffer;
            }
        }
    }
}
