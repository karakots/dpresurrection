using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

using HouseholdLibrary;
using GeoLibrary;
using MediaLibrary;
using DemographicLibrary;
using FlySim;

namespace MediaManager
{
    public class MediaPenetration
    {
        private static Random rand = new Random();

        #region private members
        public List<string> errors;

        private AdPlanItDataSet database;
        private AdPlanItDataSetTableAdapters.TableAdapterManager table_manager;
        private List<Agent> all_agents;
        //<TYPE, <SUBTYPE, <VEHICLE, <DEMOGRAPHIC, PENETRATION>>>>
        private Dictionary<MediaVehicle, Dictionary<Demographic, double>> media_penetration_demographics;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<Demographic, double>>>> media_rate_demographics;

        private MediaSelector media_selector;
        private Dictionary<string, List<Agent>> dma_sorted_agents;
        Dictionary<string, Demographic.PrimVector> region_demo;

        #endregion

        #region Public Members

        private GeoRegion USA;

        #endregion

        public MediaPenetration( List<Agent> agents, Dictionary<string, Demographic.PrimVector> regStats )
        {
            region_demo = regStats;

            USA = GeoRegion.TopGeo;

            media_penetration_demographics = new Dictionary<MediaVehicle, Dictionary<Demographic, double>>();
            media_rate_demographics = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<Demographic, double>>>>();
            
            errors = new List<string>();

            database = new AdPlanItDataSet();

            all_agents = agents;
        }

     

        #region public functions

        public void InitLoad( string connectionString )
        {
            initialize_adapters( connectionString );
            load_from_db();

            if( all_agents != null )
            {
                dma_sorted_agents = SortAgents( GeoRegion.RegionType.DMA );

                dma_sorted_agents.Add( "USA", all_agents );
            }
        }

        public string ApplyMedia( MediaVehicle.MediaType mediaType, List<MediaVehicle> media, string filename)
        {
            string oops = null;

            // Keep Statistics for media db
            Agent.KeepStats = true;
            Agent.ClearStats();

            switch( mediaType )
            {
                case MediaVehicle.MediaType.Radio:
                    {
                       oops =  ReadDemographics(media,  filename );

                        Dictionary<string, List<MediaVehicle>> DMA_radio;
                        SortMedia( GeoRegion.RegionType.DMA, media, out DMA_radio );
                        StandardApplyMedia( dma_sorted_agents, DMA_radio );
                        DMA_radio.Clear();
                        media_penetration_demographics.Clear();
                    }
                    break;

                case MediaVehicle.MediaType.Magazine:
                    {
                        oops = ReadDemographics( media, filename );

                        Dictionary<string, List<MediaVehicle>> DMA_magazine;
                        SortMedia( GeoRegion.RegionType.DMA, media, out DMA_magazine );
                        StandardApplyMedia( dma_sorted_agents, DMA_magazine );
                        DMA_magazine.Clear();

                        media_penetration_demographics.Clear();
                    }
                    break;

                case MediaVehicle.MediaType.Newspaper:
                    {
                        oops = ReadDemographics( media, filename );

                        Dictionary<string, List<MediaVehicle>> DMA_newspaper;
                        SortMedia( GeoRegion.RegionType.DMA, media, out DMA_newspaper );
                        StandardApplyMedia( dma_sorted_agents, DMA_newspaper );
                        DMA_newspaper.Clear();

                        media_penetration_demographics.Clear();
                    }
                    break;

                case MediaVehicle.MediaType.Yellowpages:
                    {
                        Dictionary<string, List<MediaVehicle>> City_Yellowpages;
                        Dictionary<string, List<Agent>> City_agents = SortAgents( GeoRegion.RegionType.City);
                        SortMedia( GeoRegion.RegionType.City, media, out City_Yellowpages );
                        ApplyYellowPages( City_agents, City_Yellowpages );
                        City_agents.Clear();
                        City_Yellowpages.Clear();
                    }
                    break;

                case MediaVehicle.MediaType.Internet:
                    ApplyInternet(media );
                    break;
            }

            return oops;
        }

        public void Clean()
        {
            media_penetration_demographics.Clear();
            media_rate_demographics.Clear();
            database.Clear();

            Agent.ClearStats();
        }


        #endregion

        #region private functions

        private void initialize_adapters( string connectStr )
        {
            table_manager = new global::MediaManager.AdPlanItDataSetTableAdapters.TableAdapterManager();
            table_manager.penetrationTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.penetrationTableAdapter();
            table_manager.rateTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.rateTableAdapter();

            table_manager.Connection.ConnectionString = connectStr;
            table_manager.penetrationTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.rateTableAdapter.Connection.ConnectionString = connectStr;
        }

        private void load_tables()
        {
            table_manager.rateTableAdapter.Fill(database.rate);
        }

        private void load_from_db()
        {
            load_tables();

            load_rate();
        }

        private Demographic demographicFromPenetration( AdPlanItDataSet.penetrationRow penetration_row )
        {
            Demographic demographic = new Demographic();
            demographic.Gender = penetration_row.GENDER;
            demographic.Race = penetration_row.RACE;
            demographic.Kids = penetration_row.KIDS;
            demographic.Homeowner = penetration_row.HOMEOWNER;
            if( penetration_row.MIN_AGE != "ANY" )
            {
                demographic.Age.Clear();
                demographic.AddAge( int.Parse( penetration_row.MIN_AGE ) );
            }
            if( penetration_row.MAX_AGE != "ANY" )
            {
                demographic.AddAge( int.Parse( penetration_row.MAX_AGE ) );
            }
            if( penetration_row.MIN_INCOME != "ANY" )
            {
                demographic.Income.Clear();
                demographic.AddIncome( int.Parse( penetration_row.MIN_INCOME ) );
            }
            if( penetration_row.MAX_INCOME != "ANY" )
            {
                demographic.AddIncome( int.Parse( penetration_row.MAX_INCOME ) );
            }

            return demographic;
        }

        private double SizeWiggleFactor = 1.0;
        private void updateMediaDemographic(MediaVehicle vcl, Demographic demographic, double probability)
        {
            if( all_agents == null )
            {
                return;
            }

            if( !media_penetration_demographics.ContainsKey( vcl ) )
            {
                media_penetration_demographics.Add( vcl, new Dictionary<Demographic, double>());
            }

            double numSubsribers = SizeWiggleFactor * probability * vcl.Size * (all_agents.Count) / (double)DpMediaDb.US_HH_SIZE;

            media_penetration_demographics[vcl].Add( demographic, numSubsribers );
        }

        private void load_rate()
        {
            foreach (AdPlanItDataSet.rateRow rate_row in database.rate)
            {
                string TYPE = rate_row.TYPE.ToUpper();
                string SUBTYPE = scrub_string( rate_row.SUBTYPE );

                if (!media_rate_demographics.ContainsKey(TYPE))
                {
                    media_rate_demographics.Add(TYPE, new Dictionary<string, Dictionary<string, Dictionary<Demographic, double>>>());
                }

                if (!media_rate_demographics[TYPE].ContainsKey(SUBTYPE))
                {
                    media_rate_demographics[TYPE].Add(SUBTYPE, new Dictionary<string, Dictionary<Demographic, double>>());
                }

                if (!media_rate_demographics[TYPE][SUBTYPE].ContainsKey(rate_row.VEHICLE))
                {
                    media_rate_demographics[TYPE][SUBTYPE].Add(rate_row.VEHICLE, new Dictionary<Demographic, double>());
                }

                Demographic demographic = new Demographic();
                demographic.Gender = rate_row.GENDER;
                demographic.Race = rate_row.RACE;
                demographic.Kids = rate_row.KIDS;
                demographic.Homeowner = rate_row.HOMEOWNER;

                if( rate_row.MIN_AGE != "ANY" && rate_row.MAX_AGE != "ANY" )
                {
                    demographic.Age.Clear();

                    demographic.AddAge( int.Parse( rate_row.MIN_AGE ), int.Parse( rate_row.MAX_AGE ) );
                }
                else if( rate_row.MIN_AGE != "ANY" )
                {
                    demographic.Age.Clear();

                    demographic.AddAge( int.Parse( rate_row.MIN_AGE ), 100 );
                }
                else if( rate_row.MAX_AGE != "ANY" )
                {
                    demographic.Age.Clear();

                    demographic.AddAge( 0, int.Parse( rate_row.MAX_AGE ));
                }

                if( rate_row.MIN_INCOME != "ANY" && rate_row.MAX_INCOME != "ANY" )
                {
                    demographic.Income.Clear();

                    demographic.AddIncome( int.Parse( rate_row.MIN_INCOME ), int.Parse( rate_row.MAX_INCOME ) );
                }
                else if( rate_row.MIN_INCOME != "ANY" )
                {
                    demographic.Income.Clear();

                    demographic.AddIncome( int.Parse( rate_row.MIN_INCOME ), 1000000 );
                }
                else if (rate_row.MAX_INCOME != "ANY")
                {
                    demographic.Income.Clear();

                    demographic.AddIncome(0, int.Parse(rate_row.MAX_INCOME));
                }

                media_rate_demographics[TYPE][SUBTYPE][rate_row.VEHICLE].Add( demographic, rate_row.RATE );
            }
        }
   

        private string scrub_string(string input)
        {
            string retval = input.ToUpper().Replace(" ", "");
            retval = retval.Replace("&", "");
            retval = retval.Replace("(", "");
            retval = retval.Replace(")", "");
            retval = retval.Replace("/", "");
            retval = retval.Replace( "\"", "" );
            retval = retval.Replace( "\"", "" );
            retval = retval.Replace( "?", "" );
            retval = retval.Replace( "*", "" );
            
            retval = retval.Trim();
            return retval;
        }

       
        private string StandardApplyMedia(Dictionary<string, List<Agent>> sorted_agents, Dictionary<string, List<MediaVehicle>> sorted_vehicles) 
        {
            string error = null;
            int totalSubscibers = 0;
            Dictionary<Demographic, int> numScribers = new Dictionary<Demographic, int>();

            foreach (string region_name in sorted_agents.Keys)
            {
                if (!sorted_vehicles.ContainsKey(region_name))
                {
                    continue;
                }
                foreach( MediaVehicle vehicle in sorted_vehicles[region_name] )
                {
                    double scaledSize = SizeWiggleFactor * vehicle.Size * all_agents.Count / DpMediaDb.US_HH_SIZE;

                    Dictionary<Demographic, double> pen_demographics = media_penetration_demographics[vehicle];

                    totalSubscibers = 0;
                    numScribers.Clear();

                    foreach( Demographic demo in pen_demographics.Keys )
                    {
                        numScribers.Add( demo, 0 );
                    }

                    if( pen_demographics == null )
                    {
                        throw new Exception( "No penetration numbers exist for vehicle" );
                    }

                    Bucket bucket = new Bucket( sorted_agents[region_name].Count );

                    while( totalSubscibers < scaledSize && !bucket.Empty )
                    {
                        int index = bucket.Draw;
                        Agent agent = sorted_agents[region_name][index];

                        foreach( KeyValuePair<Demographic, double> demographic_pen_pair in pen_demographics )
                        {
                            if( numScribers[demographic_pen_pair.Key] < demographic_pen_pair.Value &&
                            agent.House.Match( demographic_pen_pair.Key ) )
                            {
                                double rate = get_rate( vehicle, agent.House );

                                agent.AddMedia( vehicle, rate );

                                numScribers[demographic_pen_pair.Key] += 1;

                                totalSubscibers += 1;

                                break;
                            }
                        }
                    }

                    if( totalSubscibers < scaledSize )
                    {
                        error += "\r\nCannot find enough subsribers for " + vehicle.Vehicle;
                    }
                }
            }

            return error;
        }

        private void ApplyYellowPages( Dictionary<string, List<Agent>> sorted_agents, Dictionary<string, List<MediaVehicle>> sorted_print )
        {
            foreach( string region_name in sorted_print.Keys )
            {
                if( !sorted_agents.ContainsKey( region_name ) || !sorted_print.ContainsKey( region_name ) )
                {
                    continue;
                }

                double region_size = ((double)DpMediaDb.US_HH_SIZE) * ((double)sorted_agents[region_name].Count) / (double)all_agents.Count;

                foreach( Agent agent in sorted_agents[region_name] )
                {
                    foreach( MediaVehicle print in sorted_print[region_name] )
                    {
                        double probability = print.Size / region_size;

                        if( rand.NextDouble() < probability )
                        {
                            double rate = get_rate( print, agent.House );
                          
                            agent.AddMedia( print, rate );
                            break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// We still need a better internet model
        /// </summary>
        private void ApplyInternet( List<MediaVehicle> ad_networks )
        {
            foreach( MediaVehicle ad_network in ad_networks )
            {
                double scaledSize = ad_network.Size * all_agents.Count / DpMediaDb.US_HH_SIZE;

                Bucket bucket = new Bucket( all_agents.Count );

                int totalSubscibers = 0;

                while( totalSubscibers < scaledSize && !bucket.Empty )
                {
                    int index = bucket.Draw;

                    Agent agent = all_agents[index];

                    double rate = get_rate( ad_network, agent.House );

                    agent.AddMedia( ad_network, rate );

                    totalSubscibers++;
                }
            }
        }

        private Dictionary<Demographic, double> get_matches(MediaVehicle vehicle, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<Demographic, double>>>> type_dictionary)
        {
            Dictionary<string, Dictionary<string, Dictionary<Demographic, double>>> subtype_dict;
            string type = vehicle.Type.ToString().ToUpper();
            string subtype = scrub_string( vehicle.SubType );

            if (type_dictionary.ContainsKey(type))
            {
                subtype_dict = type_dictionary[type];
            }
            else if (type_dictionary.ContainsKey("ALL"))
            {
                subtype_dict = type_dictionary["ALL"];
            }
            else
            {
                return null;
            }

            Dictionary<string, Dictionary<Demographic, double>> vehicle_dict;
            if (subtype_dict.ContainsKey(subtype))
            {
                vehicle_dict = subtype_dict[subtype];
            }
            else if (subtype_dict.ContainsKey("ALL"))
            {
                vehicle_dict = subtype_dict["ALL"];
            }
            else
            {
                return null;
            }

            if (vehicle_dict.ContainsKey(vehicle.Vehicle))
            {
                return vehicle_dict[vehicle.Vehicle];
            }
            else if (vehicle_dict.ContainsKey("ALL"))
            {
                return vehicle_dict["ALL"];
            }
            else
            {
                return null;
            }
        }

        private double get_rate(MediaVehicle media, Household house)
        {
            double rate = 0.0;
            Dictionary<Demographic, double> rate_demographics = get_matches(media, media_rate_demographics);

            foreach( Person person in house.People )
            {
                double personRate = 0.0;
                int numRates = 0;

                foreach( KeyValuePair<Demographic, double> demographic_rate_pair in rate_demographics )
                {
                    if( person.Match( demographic_rate_pair.Key ) )
                    {
                        personRate += demographic_rate_pair.Value;
                        numRates++;
                    }
                }

                if( numRates > 0 )
                {
                    rate +=  personRate / numRates;
                }
            }

            if( rate > 0 )
            {
                // give it a 10% jiggle
                double rnd = rand.NextDouble();
                rate *= 1 + (rnd - 0.5) / 5.0;
            }

            return rate;
        }

        public void Normalize_rates(MediaVehicle.MediaType type)
        {
            foreach (Agent agent in all_agents)
            {
                if( !agent.House.HasMedia( (int) type ))
                {
                    continue;
                }

                // num media
                double numMedia = agent.House.Media( (int)type ).Count;

                if( numMedia > 0 )
                {
                    // the current total is too large
                    // We need to scale the amount of time down
                    foreach( HouseholdMedia media in agent.House.Media( (int)type ) )
                    {
                        media.Rate /= numMedia;
                    }
                }
            }
        }

        private  Dictionary<string, List<Agent>> SortAgents(GeoRegion.RegionType type)
        {
            Dictionary<string, List<Agent>> rval = new Dictionary<string,List<Agent>>();

            foreach (Agent agent in all_agents)
            {
                GeoRegion geo_region = USA.GetSubRegion(agent.House.GeoID.ToString());
                if (geo_region == null)
                {
                    continue;
                }
                GeoRegion type_region = geo_region.GetType(type);
                if (type_region == null)
                {
                    continue;
                }

                if( !rval.ContainsKey( type_region.Name ) )
                {
                    rval.Add( type_region.Name, new List<Agent>() );
                }

                rval[type_region.Name].Add( agent );
            }

            return rval;
        }

        private void SortMedia(GeoRegion.RegionType type, List<MediaVehicle> all_media, 
            out Dictionary<string, List<MediaVehicle>> sorted_media)
        {
            sorted_media = new Dictionary<string, List<MediaVehicle>>();

            foreach (MediaVehicle media in all_media)
            {
                GeoRegion geo_region = GeoRegion.TopGeo.GetSubRegion( media.RegionName );

                //Ugly....HACK
                if (media.Type == MediaVehicle.MediaType.Yellowpages)
                {
                   //  geo_region = GeoRegion.GeoLibrary[(media as Yellowpages).City];
                }

                GeoRegion type_region = geo_region.GetType(type);

                if (type_region == null)
                {
                    type_region = USA;
                }

                if (!sorted_media.ContainsKey(type_region.Name))
                {
                    sorted_media.Add( type_region.Name, new List<MediaVehicle>() );
                }

                sorted_media[type_region.Name].Add(media);
            }
        }


        private static int valueIndex = 3;
        public string ReadDemographics( List<MediaVehicle> media, string fileName )
        {
            string oops = null;

            StreamReader reader = new StreamReader( fileName );
            string line = reader.ReadLine();
            string[] vals = line.Split( '\t' );

            if( vals[0].ToUpper() == "REACH" && vals.Length > 1 )
            {
                SizeWiggleFactor = Double.Parse( vals[1] );
            }
            else
            {
                return "BAD FORMAT FOR PENETRATION FILE";
            }

            // need to parse demographics
            List<Demographic> demos = null;

            while( !reader.EndOfStream )
            {
                line = reader.ReadLine();

                vals = line.Split( '\t' );

                if( vals[0].ToUpper() == "TYPE" )
                {
                    // done reading demographics
                    break;
                }

                if( demos == null )
                {
                    demos = new List<Demographic>();
                    for( int jj = valueIndex; jj < vals.Length; ++jj )
                    {
                        demos.Add( new Demographic() );
                    }
                }

                string demoType = vals[2];
                for( int ii = valueIndex; ii < vals.Length; ++ii )
                {
                    switch( demoType )
                    {
                        case "GENDER":
                            demos[ii - valueIndex].Gender = vals[ii];
                            break;
                        case "RACE":
                            demos[ii - valueIndex].Race = vals[ii];
                            break;
                        case "HOMEOWNER":
                            demos[ii - valueIndex].Homeowner = vals[ii];
                            break;
                        case "KIDS":
                            demos[ii - valueIndex].Kids = vals[ii];
                            break;
                        case "MIN AGE":

                            if( vals[ii] != "ANY" )
                            {
                                demos[ii - valueIndex].Age.Clear();
                                demos[ii - valueIndex].AddAge( int.Parse( vals[ii] ) );
                            }
                            break;

                        case "MAX AGE":
                            if( vals[ii] != "ANY" )
                            {
                                // demos[ii - valueIndex].Age.Clear();
                                demos[ii - valueIndex].AddAge( int.Parse( vals[ii] ) );
                            }
                            break;

                        case "MIN INCOME":

                            if( vals[ii] != "ANY" )
                            {
                                demos[ii - valueIndex].Income.Clear();

                                int income = int.Parse( vals[ii] );

                                income *= 1000;

                                demos[ii - valueIndex].AddIncome( income + 1 );
                            }
                            break;

                        case "MAX INCOME":
                            if( vals[ii] != "ANY" )
                            {
                                // demos[ii - valueIndex].Income.Clear();

                                int income = int.Parse( vals[ii] );

                                income *= 1000;

                                demos[ii - valueIndex].AddIncome( income - 1 );
                            }
                            break;
                    }
                }
            }

            if( media == null )
            {
                foreach( Demographic demo in demos )
                {
                    oops += demo.ToString() + "\r\n";
                }

                return oops;
            }

            // now read penetration for vehicles
            while( !reader.EndOfStream )
            {
                line = reader.ReadLine();

                vals = line.Split( '\t' );

                string mediaType = vals[0];
                string subtype = scrub_string( vals[1] );
                string vclName = scrub_string(vals[2]);

                if( vclName == "ALL" )
                {
                    for( int ii = valueIndex; ii < vals.Length; ++ii )
                    {
                        double pntr = Double.Parse( vals[ii] );

                        if( pntr > 0 )
                        {
                            // update for all vehicles
                            foreach( MediaVehicle vcl in media.Where( row => scrub_string(row.SubType) == subtype ) )
                            {
                                updateMediaDemographic( vcl, demos[ii - valueIndex], pntr );
                            }
                        }
                    }
                }
                else
                {

                    try
                    {
                        MediaVehicle vcl = media.First( row =>  scrub_string(row.SubType) == subtype && scrub_string( row.Vehicle ) == vclName );

                        for( int ii = valueIndex; ii < vals.Length; ++ii )
                        {
                            double pntr = Double.Parse( vals[ii] );

                            if( pntr > 0 )
                            {
                                updateMediaDemographic( vcl, demos[ii - valueIndex], pntr );
                            }
                        }
                    }
                    catch( Exception e )
                    {
                        oops += "\r\n " + e.Message + " : " + subtype + " : " + vclName;
                    }
                }
            }

            // check if all vehicles have a penetration

            foreach( MediaVehicle vcl in media )
            {
                if( !media_penetration_demographics.ContainsKey( vcl ) )
                {
                    oops += "Missing data for " + vcl.SubType + " : " + vcl.Vehicle;
                }
            }

            return oops;
              
        }

        #endregion
    }
}
