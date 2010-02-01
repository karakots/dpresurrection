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

namespace MediaManager
{
    public class MediaBuilder
    {

        #region private members

        public List<string> errors;

        private AdPlanItDataSet database;
        private AdPlanItDataSetTableAdapters.TableAdapterManager table_manager;
        private Dictionary<string, AdPlanItDataSet.ad_optionsRow> offline_ad_options;
        private Dictionary<string, AdPlanItDataSet.internet_ad_optionsRow> online_ad_options;

        private Dictionary<string, List<AdPlanItDataSet.ad_optionsRow>> media_offline_ad_options;
        private Dictionary<string, List<AdPlanItDataSet.internet_ad_optionsRow>> media_online_ad_options;

        private GeoRegion USA;

        private Dictionary<string, GeoRegion> region_map;

        #endregion

        #region Public Members

        //public void AddVehiclesToDb( MediaVehicle.MediaType type, DpMediaDb mediaDb )
        //{
        //    switch( type )
        //    {
        //        case MediaVehicle.MediaType.Magazine:

        //            foreach( MediaVehicle vehicle in Magazines )
        //            {
        //                mediaDb.AddVehicle( vehicle );
        //            }
        //            break;
        //        case MediaVehicle.MediaType.Radio:

        //            foreach( MediaVehicle vehicle in Radio_Stations )
        //            {
        //                mediaDb.AddVehicle( vehicle );
        //            }
        //            break;
        //        case MediaVehicle.MediaType.Internet:

        //            foreach( MediaVehicle vehicle in Internet_Networks )
        //            {
        //                mediaDb.AddVehicle( vehicle );
        //            }
        //            break;
        //        case MediaVehicle.MediaType.Newspaper:

        //            foreach( MediaVehicle vehicle in Newspapers )
        //            {
        //                mediaDb.AddVehicle( vehicle );
        //            }
        //            break;
        //        case MediaVehicle.MediaType.Yellowpages:

        //            foreach( MediaVehicle vehicle in Yellowpages )
        //            {
        //                mediaDb.AddVehicle( vehicle );
        //            }
        //            break;
        //    }
        //}

        public List<MediaVehicle> Magazines;
        public List<MediaVehicle> Radio_Stations;
        public List<MediaVehicle> Internet_Networks;
        public List<MediaVehicle> Newspapers;
        public List<MediaVehicle> Yellowpages;

        #endregion

        public MediaBuilder()
        {

            USA = GeoRegion.TopGeo;

            offline_ad_options = new Dictionary<string, AdPlanItDataSet.ad_optionsRow>();
            online_ad_options = new Dictionary<string, AdPlanItDataSet.internet_ad_optionsRow>();
            media_offline_ad_options = new Dictionary<string, List<AdPlanItDataSet.ad_optionsRow>>();
            media_online_ad_options = new Dictionary<string, List<AdPlanItDataSet.internet_ad_optionsRow>>();

            errors = new List<string>();

            database = new AdPlanItDataSet();

            //Magazines = new List<Magazine>();
            //Radio_Stations = new List<ScalarVehicle>();
            //Internet_Networks = new List<InternetAdNetwork>();
            //Newspapers = new List<Newspaper>();

            region_map = new Dictionary<string, GeoRegion>();
        }

        #region public functions

        public void InitLoad( string connectionString )
        {
            initialize_adapters( connectionString );
            load_from_db();
        }


        public void BuildMediaFromDB( MediaVehicle.MediaType mediaType )
        {

            //switch( mediaType )
            //{
            //    case MediaVehicle.MediaType.Magazine:




            //        Magazines = new List<Magazine>();
            //        foreach( AdPlanItDataSet.magazine_infoRow magazine_row in database.magazine_info )
            //        {
            //            Magazines.AddRange( build_magazines( magazine_row ) );
            //        }

            //        break;

            //    case MediaVehicle.MediaType.Radio:


            //        Radio_Stations = new List<ScalarVehicle>();

            //        foreach( AdPlanItDataSet.radio_infoRow radio_row in database.radio_info )
            //        {
            //            Radio_Stations.AddRange( build_radio( radio_row ) );
            //        }
            //        break;

            //    case MediaVehicle.MediaType.Internet:

            //        Internet_Networks = new List<InternetAdNetwork>();
            //        foreach( AdPlanItDataSet.internet_infoRow internet_row in database.internet_info )
            //        {
            //            Internet_Networks.AddRange( build_internet( internet_row ) );
            //        }

            //        break;

            //    case MediaVehicle.MediaType.Newspaper:


            //        // compute penetratoin demographics


            //        Newspapers = new List<Newspaper>();
            //        foreach( AdPlanItDataSet.newspaper_infoRow newspaper_row in database.newspaper_info )
            //        {
            //            Newspapers.AddRange( build_newspapers( newspaper_row ) );
            //        }

            //        // compute_penetration_demographics( MediaVehicle.MediaType.Newspaper );

            //        break;

            //    case MediaVehicle.MediaType.Yellowpages:


            //        Yellowpages = new List<Yellowpages>();

            //        foreach( AdPlanItDataSet.yellowpages_infoRow yellowpages_row in database.yellowpages_info )
            //        {
            //            Yellowpages.AddRange( build_yellowpages( yellowpages_row ) );
            //        }
            //        break;
            //}
        }


        public void ClearVehicleData()
        {
            Magazines.Clear();
            Radio_Stations.Clear();
            Internet_Networks.Clear();
            Newspapers.Clear();
            Radio_Stations.Clear();
        }

        public void ClearAll()
        {
            ClearVehicleData();

            offline_ad_options.Clear();
            online_ad_options.Clear();
            media_offline_ad_options.Clear();
            media_online_ad_options.Clear();
            database.Clear();
        }


        #endregion

        #region private functions

        private void initialize_adapters( string connectStr )
        {
            table_manager = new global::MediaManager.AdPlanItDataSetTableAdapters.TableAdapterManager();
            table_manager.magazine_infoTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.magazine_infoTableAdapter();
            table_manager.radio_infoTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.radio_infoTableAdapter();
            table_manager.ad_optionsTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.ad_optionsTableAdapter();
            
            table_manager.internet_ad_optionsTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.internet_ad_optionsTableAdapter();
            table_manager.internet_infoTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.internet_infoTableAdapter();
            table_manager.newspaper_infoTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.newspaper_infoTableAdapter();
            table_manager.yellowpages_infoTableAdapter = new global::MediaManager.AdPlanItDataSetTableAdapters.yellowpages_infoTableAdapter();

            table_manager.Connection.ConnectionString = connectStr;
            table_manager.magazine_infoTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.radio_infoTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.ad_optionsTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.internet_ad_optionsTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.internet_infoTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.newspaper_infoTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.yellowpages_infoTableAdapter.Connection.ConnectionString = connectStr;
        }

        private void load_tables()
        {
            table_manager.magazine_infoTableAdapter.Fill(database.magazine_info);
            table_manager.radio_infoTableAdapter.Fill(database.radio_info);
            table_manager.ad_optionsTableAdapter.Fill(database.ad_options);
            table_manager.internet_ad_optionsTableAdapter.Fill(database.internet_ad_options);
            table_manager.internet_infoTableAdapter.Fill(database.internet_info);
            table_manager.newspaper_infoTableAdapter.Fill(database.newspaper_info);
            table_manager.yellowpages_infoTableAdapter.Fill(database.yellowpages_info);
        }

        private void load_from_db()
        {
            load_tables();

            offline_ad_options.Clear();
            foreach (AdPlanItDataSet.ad_optionsRow option_row in database.ad_options)
            {
                try
                {
                    offline_ad_options.Add(option_row.IDENTIFIER, option_row);
                }
                catch (Exception e)
                {
                    errors.Add("Error loading ad_option: " + option_row.AD_OPTION + " " + e.Message);
                }
            }

            online_ad_options.Clear();
            foreach (AdPlanItDataSet.internet_ad_optionsRow option_row in database.internet_ad_options)
            {
                try
                {
                    online_ad_options.Add(option_row.IDENTIFIER.Trim(), option_row);
                }
                catch (Exception e)
                {
                    errors.Add("Error loading ad_option: " + option_row.AD_OPTION + " " + e.Message);
                }
            }

            media_offline_ad_options.Clear();
            media_online_ad_options.Clear();


            foreach( AdPlanItDataSet.magazine_infoRow vehicle in database.magazine_info )
            {
                media_offline_ad_options.Add(vehicle.VEHICLE, new List<AdPlanItDataSet.ad_optionsRow>());
                media_online_ad_options.Add(vehicle.VEHICLE, new List<AdPlanItDataSet.internet_ad_optionsRow>());
                string[] options = vehicle.AD_OPTIONS.Split(';');
                foreach (string option in options)
                {
                    
                    try
                    {
                        if (offline_ad_options.ContainsKey(option))
                        {
                            media_offline_ad_options[vehicle.VEHICLE].Add(offline_ad_options[option]);
                        }
                        else
                        {
                            media_online_ad_options[vehicle.VEHICLE].Add(online_ad_options[option.Trim()]);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add("Error loading ad_option for magazine: " + vehicle + " options: " + option + " " + e.Message);
                    }
                }
            }

            foreach( AdPlanItDataSet.radio_infoRow vehicle in database.radio_info )
            {
                if( !media_offline_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_offline_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.ad_optionsRow>() );
                }

                if( !media_online_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_online_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.internet_ad_optionsRow>() );
                }

                string[] options = vehicle.AD_OPTIONS.Split(';');
                foreach (string option in options)
                {
                    try
                    {
                        if (offline_ad_options.ContainsKey(option))
                        {
                            media_offline_ad_options[vehicle.VEHICLE].Add(offline_ad_options[option]);
                        }
                        else
                        {
                            media_online_ad_options[vehicle.VEHICLE].Add(online_ad_options[option]);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add("Error loading ad_option for radio station: " + vehicle + " options: " + option + " " + e.Message);
                    }
                }
            }

            foreach (AdPlanItDataSet.internet_infoRow vehicle in database.internet_info)
            {
                media_offline_ad_options.Add(vehicle.VEHICLE, new List<AdPlanItDataSet.ad_optionsRow>());
                media_online_ad_options.Add(vehicle.VEHICLE, new List<AdPlanItDataSet.internet_ad_optionsRow>());
                string[] options = vehicle.AD_OPTIONS.Split(';');
                foreach (string option in options)
                {
                    try
                    {
                        if (offline_ad_options.ContainsKey(option))
                        {
                            media_offline_ad_options[vehicle.VEHICLE].Add(offline_ad_options[option]);
                        }
                        else
                        {
                            media_online_ad_options[vehicle.VEHICLE].Add(online_ad_options[option.Trim()]);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add("Error loading ad_option for ad network: " + vehicle + " options: " + option + " " + e.Message);
                    }
                }
            }

            foreach (AdPlanItDataSet.newspaper_infoRow vehicle in database.newspaper_info)
            {

                if( !media_offline_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_offline_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.ad_optionsRow>() );
                }

                if( !media_online_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_online_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.internet_ad_optionsRow>() );
                }

                string[] options = vehicle.AD_OPTIONS.Split(';');
                foreach (string option in options)
                {
                    try
                    {
                        if (offline_ad_options.ContainsKey(option))
                        {
                            media_offline_ad_options[vehicle.VEHICLE].Add(offline_ad_options[option]);
                        }
                        else
                        {
                            media_online_ad_options[vehicle.VEHICLE].Add(online_ad_options[option.Trim()]);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add("Error loading ad_option for ad network: " + vehicle + " options: " + option + " " + e.Message);
                    }
                }
            }

            foreach (AdPlanItDataSet.yellowpages_infoRow vehicle in database.yellowpages_info)
            {

                if( !media_offline_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_offline_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.ad_optionsRow>() );
                }

                if( !media_online_ad_options.ContainsKey( vehicle.VEHICLE ) )
                {
                    media_online_ad_options.Add( vehicle.VEHICLE, new List<AdPlanItDataSet.internet_ad_optionsRow>() );
                }

                string[] options = vehicle.AD_OPTIONS.Split(';');
                foreach (string option in options)
                {
                    try
                    {
                        if (offline_ad_options.ContainsKey(option))
                        {
                            media_offline_ad_options[vehicle.VEHICLE].Add(offline_ad_options[option]);
                        }
                        else
                        {
                            media_online_ad_options[vehicle.VEHICLE].Add(online_ad_options[option.Trim()]);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add("Error loading ad_option for ad network: " + vehicle + " options: " + option + " " + e.Message);
                    }
                }
            }
        }


        private string scrub_string(string input)
        {
            string retval = input.ToUpper().Replace(" ", "");
            retval.Replace("&", "");
            retval.Replace("(", "");
            retval.Replace(")", "");
            retval.Replace("/", "");
            retval.Trim();
            return retval;
        }

        private List<MediaVehicle> build_radio( AdPlanItDataSet.radio_infoRow radio_row )
        {
            List<MediaVehicle> radio_stations = new List<MediaVehicle>();

            //string geography = format_geo_string(radio_row.GEOGRAPHY);

            //GeoRegion region = USA.GetSubRegion(radio_row.GEOGRAPHY);

            //if (region == null)
            //{
            //    if (!region_map.ContainsKey(geography))
            //    {
            //        string match = bestMatch( geography );

            //        region = USA.GetSubRegion( match );
            //        region_map.Add( geography, region );
            //    }
            //    else
            //    {
            //        region = region_map[geography];
            //    }
            //    radio_row.GEOGRAPHY = region.Name;
            //    table_manager.radio_infoTableAdapter.Update(radio_row);
            //}


            //GeoRegion DMA = region;

            //if( region.Type > GeoRegion.RegionType.DMA )
            //{
            //    DMA = region.GetDMA();
            //}

            //string subtype = scrub_string( radio_row.SUBTYPE );

            //ScalarVehicle radio_station = new ScalarVehicle( radio_row.GUID,
            //                                                radio_row.CIRCULATION,
            //                                                radio_row.PERSUASION_SCALAR,
            //                                                radio_row.PROBABILITY_SCALAR,
            //                                                MediaVehicle.MediaType.Radio,
            //                                                subtype,
            //                                                radio_row.VEHICLE,
            //                                                DMA.Name,
            //                                                MediaVehicle.AdCycle.Daily,
            //                                                radio_row.CPM,
            //                                                radio_row.URL);

            //foreach (AdPlanItDataSet.ad_optionsRow ad_option in media_offline_ad_options[radio_row.VEHICLE])
            //{
            //    //SimpleOption option = new SimpleOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.PROB_PER_HOUR, ad_option.AWARENESS, ad_option.PERSUASION, ad_option.RECENCY, ad_option.COST_MODIFIER);
            //    //radio_station.AddOption(option);
            //}

            //foreach (AdPlanItDataSet.internet_ad_optionsRow ad_option in media_online_ad_options[radio_row.VEHICLE])
            //{
            //    //OnlineOption option = new OnlineOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.IMP_AWARENESS, ad_option.IMP_PERSUASION, ad_option.PROB_IMPRESSION, ad_option.CLK_AWARENESS, ad_option.CLK_PERSUASION, ad_option.ACT_AWARENESS, ad_option.ACT_PERSUASION, ad_option.PROB_CLICK, ad_option.PROB_ACTION, ad_option.COST_MODIFIER);
            //    //radio_station.AddOption(option);
            //}


//            radio_stations.Add(radio_station);
            return radio_stations;
        }

        private List<MediaVehicle> build_magazines( AdPlanItDataSet.magazine_infoRow magazine_row )
        {
            List<MediaVehicle> magazines = new List<MediaVehicle>();

            //string geography = format_geo_string(magazine_row.GEOGRAPHY);

            //if( geography == "US" )
            //{
            //    geography = "USA";
            //}

            //GeoRegion region = USA.GetSubRegion( geography );
            //if (region == null)
            //{
            //    if (!region_map.ContainsKey(geography))
            //    {
            //        string match = bestMatch( geography );

            //        if( match == null )
            //        {
            //            region = USA;
            //        }
            //        else
            //        {
            //            region = USA.GetSubRegion( match );
            //            region_map.Add( geography, region );
            //        }
            //    }
            //    else
            //    {
            //        region = region_map[geography];
            //    }
            //    magazine_row.GEOGRAPHY = region.Name;
            //    table_manager.magazine_infoTableAdapter.Update(magazine_row);
            //}

            //GeoRegion DMA = region;

            //if( region.Type > GeoRegion.RegionType.DMA )
            //{
            //    DMA = region.GetDMA();
            //}

            //string subtype = scrub_string(magazine_row.SUBTYPE);

            //Magazine magazine = new Magazine( magazine_row.GUID,
            //                                            magazine_row.CIRCULATION * 1000,
            //                                            magazine_row.PERSUASION_SCALAR,
            //                                            magazine_row.PROBABIILTY_SCALAR,
            //                                            MediaVehicle.MediaType.Magazine,
            //                                            subtype,
            //                                            magazine_row.VEHICLE,
            //                                            DMA.Name,
            //                                            MediaVehicle.GetCycle(magazine_row.CYCLE),
            //                                            magazine_row.CPM,
            //                                            magazine_row.URL);

            //foreach (AdPlanItDataSet.ad_optionsRow ad_option in media_offline_ad_options[magazine_row.VEHICLE])
            //{
            //    //SimpleOption option = new SimpleOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.PROB_PER_HOUR, ad_option.AWARENESS, ad_option.PERSUASION, ad_option.RECENCY, ad_option.COST_MODIFIER);
            //    //magazine.AddOption(option);
            //}

            //foreach (AdPlanItDataSet.internet_ad_optionsRow ad_option in media_online_ad_options[magazine_row.VEHICLE])
            //{
            //    //OnlineOption option = new OnlineOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.IMP_AWARENESS, ad_option.IMP_PERSUASION, ad_option.PROB_IMPRESSION, ad_option.CLK_AWARENESS, ad_option.CLK_PERSUASION, ad_option.ACT_AWARENESS, ad_option.ACT_PERSUASION, ad_option.PROB_CLICK, ad_option.PROB_ACTION, ad_option.COST_MODIFIER);
            //    //magazine.AddOption(option);
            //}

            //if(magazine_row.BUY.Contains('N'))
            //{
            //    magazine.AddPurchaseOption(Magazine.PurchaseOption.National);
            //}

            //if ((DMA == USA && magazine_row.BUY.Contains('D')) || region != USA)
            //{
            //    magazine.AddPurchaseOption(Magazine.PurchaseOption.DMA);
            //}

            //magazines.Add(magazine);
           

            return magazines;
        }

        private string bestMatch( string inStr )
        {
            string rval = null;

            int max = 0;
            int cur = 0;
            int length = inStr.Length;
            foreach( string name in GeoRegion.GeoLibrary.Keys )
            {
                for( cur = 0; cur < length; ++cur )
                {
                    if( !name.ToUpper().StartsWith( inStr.ToUpper().Substring( 0, cur ) ) )
                    {
                        break;
                    }
                }

                if( cur > max )
                {
                    max = cur;

                    rval = name;
                }
            }

            return rval;
        }

        private List<MediaVehicle> build_newspapers( AdPlanItDataSet.newspaper_infoRow newspaper_row )
        {
            List<MediaVehicle> newspapers = new List<MediaVehicle>();

            //string geography = format_geo_string(newspaper_row.GEOGRAPHY);

            //GeoRegion region = USA.GetSubRegion( geography );
            //if (region == null)
            //{
            //    if (!region_map.ContainsKey(geography))
            //    {
            //        string match = bestMatch( geography );

            //        region = USA.GetSubRegion( match );
            //        region_map.Add(geography, region);
            //    }
            //    else
            //    {
            //        region = region_map[geography];
            //    }

            //    newspaper_row.GEOGRAPHY = region.Name;
            //    table_manager.newspaper_infoTableAdapter.Update(newspaper_row);
            //}

            //GeoRegion DMA = region;

            //if (region.Type > GeoRegion.RegionType.DMA)
            //{
            //    DMA = region.GetDMA();
            //}

            //string subtype = scrub_string(newspaper_row.SUBTYPE);

            //Newspaper newspaper = new Newspaper( newspaper_row.GUID,
            //                                            newspaper_row.CIRCULATION,
            //                                            Newspaper.GetDistribution( newspaper_row.CYCLE ),
            //                                            newspaper_row.PERSUASION_SCALAR,
            //                                            newspaper_row.PROBABILITY_SCALAR,
            //                                            MediaVehicle.MediaType.Newspaper,
            //                                            subtype,
            //                                            newspaper_row.VEHICLE,
            //                                            DMA.Name,
            //                                            MediaVehicle.AdCycle.Daily,
            //                                            newspaper_row.CPM,
            //                                            newspaper_row.URL);

            //foreach (AdPlanItDataSet.ad_optionsRow ad_option in media_offline_ad_options[newspaper_row.VEHICLE])
            //{
            //    //SimpleOption option = new SimpleOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.PROB_PER_HOUR, ad_option.AWARENESS, ad_option.PERSUASION, ad_option.RECENCY, ad_option.COST_MODIFIER);
            //    //newspaper.AddOption(option);
            //}

            //foreach (AdPlanItDataSet.internet_ad_optionsRow ad_option in media_online_ad_options[newspaper_row.VEHICLE])
            //{
            //    //OnlineOption option = new OnlineOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.IMP_AWARENESS, ad_option.IMP_PERSUASION, ad_option.PROB_IMPRESSION, ad_option.CLK_AWARENESS, ad_option.CLK_PERSUASION, ad_option.ACT_AWARENESS, ad_option.ACT_PERSUASION, ad_option.PROB_CLICK, ad_option.PROB_ACTION, ad_option.COST_MODIFIER);
            //    //newspaper.AddOption(option);
            //}

            //newspapers.Add(newspaper);


            return newspapers;
        }

        private List<MediaVehicle> build_yellowpages( AdPlanItDataSet.yellowpages_infoRow yellowpages_row )
        {
            List<MediaVehicle> yellowpages_list = new List<MediaVehicle>();

            //string city = format_geo_string(yellowpages_row.CITY);
            //string state = format_geo_string(yellowpages_row.STATE);
            //string geo_id = yellowpages_row.GEOGRAPHY;
            ////For right now ignore missing geo_ids, in future put code for finding correct geo_id here...
            //if(!GeoRegion.GeoLibrary.ContainsKey(geo_id))
            //{
            //    return yellowpages_list;
            //}


            

            //GeoRegion DMA = GeoRegion.GeoLibrary[geo_id].GetDMA();

            //string subtype = scrub_string(yellowpages_row.SUBTYPE);

            //Yellowpages yellowpages = new Yellowpages( yellowpages_row.GUID,
            //                                            geo_id,
            //                                            DpMediaDb.US_HH_SIZE,
            //                                            yellowpages_row.PERSUASION_SCALAR,
            //                                            yellowpages_row.PROBABILITY_SCALAR,
            //                                            MediaVehicle.MediaType.Yellowpages,
            //                                            subtype,
            //                                            yellowpages_row.VEHICLE,
            //                                            DMA.Name,
            //                                            MediaVehicle.AdCycle.Yearly,
            //                                            yellowpages_row.CPM,
            //                                            yellowpages_row.URL);

            //foreach (AdPlanItDataSet.ad_optionsRow ad_option in media_offline_ad_options[yellowpages_row.VEHICLE])
            //{
            //    //SimpleOption option = new SimpleOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.PROB_PER_HOUR, ad_option.AWARENESS, ad_option.PERSUASION, ad_option.RECENCY, ad_option.COST_MODIFIER);
            //    //yellowpages.AddOption(option);
            //}

            //foreach (AdPlanItDataSet.internet_ad_optionsRow ad_option in media_online_ad_options[yellowpages_row.VEHICLE])
            //{
            //    //OnlineOption option = new OnlineOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.IMP_AWARENESS, ad_option.IMP_PERSUASION, ad_option.PROB_IMPRESSION, ad_option.CLK_AWARENESS, ad_option.CLK_PERSUASION, ad_option.ACT_AWARENESS, ad_option.ACT_PERSUASION, ad_option.PROB_CLICK, ad_option.PROB_ACTION, ad_option.COST_MODIFIER);
            //    //yellowpages.AddOption(option);
            //}

            //yellowpages_list.Add(yellowpages);


            return yellowpages_list;
        }

        private List<MediaVehicle> build_internet( AdPlanItDataSet.internet_infoRow internet_row )
        {
            List<MediaVehicle> ad_networks = new List<MediaVehicle>();

            string subtype = scrub_string(internet_row.SUBTYPE);

            //InternetAdNetwork ad_network = new InternetAdNetwork( internet_row.GUID,
            //                                                        internet_row.CPC,
            //                                                        internet_row.CPA,
            //                                                        internet_row.UNIQUE_USERS,
            //                                                        internet_row.MONTHLY_IMPRESSIONS,
            //                                                        internet_row.CLK_SCALAR,
            //                                                        internet_row.ACT_SCALAR,
            //                                                        1.00,
            //                                                        1.00,
            //                                                        MediaVehicle.MediaType.Internet,
            //                                                        subtype,
            //                                                        internet_row.VEHICLE.Trim(),
            //                                                        USA.Name,
            //                                                        MediaVehicle.AdCycle.Instant,
            //                                                        internet_row.CPM,
            //                                                        internet_row.URL);

            foreach (AdPlanItDataSet.internet_ad_optionsRow ad_option in media_online_ad_options[internet_row.VEHICLE])
            {
                //OnlineOption option = new OnlineOption(ad_option.AD_OPTION, ad_option.IDENTIFIER, ad_option.IMP_AWARENESS, ad_option.IMP_PERSUASION, ad_option.PROB_IMPRESSION, ad_option.CLK_AWARENESS, ad_option.CLK_PERSUASION, ad_option.ACT_AWARENESS, ad_option.ACT_PERSUASION, ad_option.PROB_CLICK, ad_option.PROB_ACTION, ad_option.COST_MODIFIER);
                //ad_network.AddOption(option);
            }

           // ad_networks.Add(ad_network);


            return ad_networks;
        }

        private string format_geo_string(string geography)
        {
            if (geography.Contains(','))
            {
                geography = geography.Split(',')[0];
            }

            return geography;
        }

        #endregion

        #region file io

        public string misMatch = null;

        public void UpdateRadio(string fileName)
        {
            foreach( AdPlanItDataSet.radio_infoRow radio in database.radio_info )
            {
                radio.CIRCULATION = 0;
            }

            StreamReader reader = new StreamReader( fileName );

            // remove first line
            reader.ReadLine();

            while( !reader.EndOfStream )
            {
                // format state, county, dma
                string line = reader.ReadLine();

                string[] vals = line.Split( '\t' );

                string type = vals[0];
                string subtype = vals[1];
                string vehicle = vals[2];
                string dma = vals[3];
                string adOption = vals[4];
                string cpm = vals[5];
                string url = vals[6];
                string circ = vals[7];

                int numMatches = database.radio_info.Count( 
                    row => scrub_string( row.SUBTYPE ) == scrub_string( subtype ) && row.VEHICLE == vehicle);

                if( numMatches == 1 )
                {
                    AdPlanItDataSet.radio_infoRow radio = database.radio_info.First(
                         row => scrub_string( row.SUBTYPE ) == scrub_string( subtype ) && row.VEHICLE == vehicle );

                    double val =  Double.Parse( circ );
                    radio.CIRCULATION += val;

                    val = 0;

                }
                else
                {
                    AdPlanItDataSet.radio_infoRow radio = database.radio_info.Newradio_infoRow();

                    radio.TYPE = "RADIO";
                    radio.SUBTYPE = scrub_string( subtype );
                    radio.VEHICLE = vehicle;
                    radio.GEOGRAPHY = dma;
                    radio.PROBABILITY_SCALAR = 1.0;
                    radio.PERSUASION_SCALAR = 1.0;
                    radio.AD_OPTIONS = adOption;
                    radio.CPM = Double.Parse( cpm );
                    radio.URL = url;
                    radio.CIRCULATION = Double.Parse( circ );
                    radio.GUID = Guid.NewGuid();

                    database.radio_info.Addradio_infoRow( radio );
                }
            }

            reader.Close();

            foreach( AdPlanItDataSet.radio_infoRow radio in database.radio_info.Where( row => row.CIRCULATION == 0 ))
            {
                radio.Delete();
            }


             table_manager.radio_infoTableAdapter.Update(database.radio_info);
        }

        #endregion
    }
}
