//#define IGNORE_REACH_AND_EFFICENCY_EXCEPTIONS
using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

using HouseholdLibrary;
using WebLibrary;
using WebLibrary.Timing;
using MediaLibrary;
using DemographicLibrary;
using GeoLibrary;

namespace BusinessLogic
{
    
        public class SelectDictionary<T> : Dictionary<T, double>
        {
            private double total = 0.0;
            private static Random rand = new Random( 12376 );

            public new void Add(T obj, double val)
            {
                if( !Double.IsNaN( val ) && val >= 0 )
                {
                    base.Add( obj, val );
                    total += val;
                }
            }

            public void Normalize()
            {
                total = 0.0;
                foreach (double key in this.Values)
                {
                    total += key;
                }
            }

            /// <summary>
            /// Selects from distribution
            /// without replacement
            /// </summary>
            /// <returns></returns>
            public T Select()
            {
                double choice = total * rand.NextDouble();

                // make sure something is selected
                double curTotal = 0.0;

                T sel = default( T );

                bool selected = false;
                
                foreach( T key in this.Keys )
                {
                    double val = this[key];

                    if( choice < curTotal + val )
                    {
                        // decrement total remove key from disctionary
                        sel = key;
                        selected = true;
                        break;
                    }

                    curTotal += val;
                }

                if( selected )
                {
                    total -= this[sel];
                    this.Remove( sel );
                }
                else if (this.Keys.Count > 0)
                {
                    // select something at random
                    int index = rand.Next( this.Keys.Count );
                    sel = this.ElementAt( index ).Key;
                }

                return sel;
            }

            /// <summary>
            /// Selects from distribution
            /// WITH replacement
            /// </summary>
            /// <returns></returns>
            public T GetRandom()
            {
                double choice = total * rand.NextDouble();

                // make sure something is selected
                double curTotal = 0.0;

                T sel = default(T);

                bool selected = false;

                foreach (T key in this.Keys)
                {
                    double val = this[key];

                    if (choice < curTotal + val)
                    {
                        // decrement total remove key from disctionary
                        sel = key;
                        selected = true;
                        break;
                    }

                    curTotal += val;
                }

                if (!selected && this.Keys.Count > 0)
                {
                    // select something at random
                    int index = rand.Next(this.Keys.Count);
                    sel = this.ElementAt(index).Key;
                }

                return sel;
            }
        }

    /// <summary>
    /// Summary description for AlocationService
    /// </summary>
    public partial class AllocationService
    {

        public static double SubTypePreferenceWeight = 100;
        public static double ReachFactor = 0.05;
        private static Random rand = new Random();

        
 
        

        private MediaLibrary.DpMediaDb database;

        private Dictionary<int, SelectDictionary<SimpleOption>> rated_options;

        private MediaPlan current_plan;
        private SelectDictionary<Guid> used_vehicles;
        private Dictionary<MediaVehicle, double> vehicle_rating;
        private int plan_span;
        private double target_pop_size;
        private Dictionary<GeoRegion, Demographic.PrimVector> target_pop;

        private static Dictionary<string, List<MediaVehicle>> saved_quereies = new Dictionary<string, List<MediaVehicle>>();

        /// <summary>
        /// Creates a new AllocationService object.  This method reads in the rules from their XML files.
        /// </summary>
        public AllocationService()
        {
            fill_location_dictionaries();

            database = Utils.MediaDatabase;

            rated_options = new Dictionary<int, SelectDictionary<SimpleOption>>();
        }

        
        
        /// <summary>
        /// Creates a new media plan from the user specs.
        /// </summary>
        /// <param name="userCampaignSpecs"></param>
        /// <returns></returns>
        public MediaPlan CreateNewMediaPlan( MediaCampaignSpecs campaign_specs ) {
            return CreateNewMediaPlan( campaign_specs, null, null );
        }

        /// <summary>
        /// Creates a new media plan from the user specs.
        /// </summary>
        /// <param name="userCampaignSpecs"></param>
        /// <returns></returns>
        public MediaPlan CreateNewMediaPlan( MediaCampaignSpecs campaign_specs, Dictionary<string, double> type_budgets, List<Guid> existing_vehicles )
        {
            
            current_plan = new MediaPlan();
            current_plan.Specs = campaign_specs;
            current_plan.TargetBudget = campaign_specs.TargetBudget;
            current_plan.PlanName = campaign_specs.CampaignName + " - AdPlanIt";

            current_plan.Goals = new List<MediaPlan.PlanGoal>();

            for(int ii = 0; ii < campaign_specs.CampaignGoals.Count; ++ii)
            {
                MediaPlan.PlanGoal item = new MediaPlan.PlanGoal( campaign_specs.CampaignGoals[ii], campaign_specs.GoalWeights[ii] );
                current_plan.Goals.Add( item );
            }


            if (campaign_specs.Demographics.Count > 0)
            {
                campaign_specs.SegmentList.Clear();
                for (int ii = 0; ii < campaign_specs.Demographics.Count; ii++)
                {
                    campaign_specs.SegmentList.Add(SimUtils.ConvertToDemographic(current_plan, ii));
                }
            }


            get_target_pop();

            if (target_pop_size <= 0)
            {
                return current_plan;
            }

            buildplan( type_budgets, existing_vehicles );

            return current_plan;
        }


        /// <summary>
        /// generates the target population statistics and size
        /// </summary>
        private void get_target_pop()
        {
            Dictionary<GeoRegion, List<Demographic>> region_pop = new Dictionary<GeoRegion, List<Demographic>>();
            Dictionary<GeoRegion, int> region_demographic_index = new Dictionary<GeoRegion, int>();

            foreach (string regionName in current_plan.Specs.GeoRegionNames)
            {
                GeoRegion region = SimUtils.GeoRegionForName(regionName);

                if (region != null)
                {
                    if (!region_pop.ContainsKey(region))
                    {
                        region_pop.Add(region, new List<Demographic>());
                    }

                    for (int ii = 0; ii < current_plan.Specs.SegmentList.Count; ii++)
                    {
                        region_pop[region].Add(current_plan.Specs.SegmentList[ii]);
                    }
                }
            }


            target_pop = new Dictionary<GeoRegion, Demographic.PrimVector>();
            target_pop_size = 0.0;
            foreach (GeoRegion region in region_pop.Keys)
            {
                target_pop.Add(region, database.TargetPopulation(region_pop[region], region));
                target_pop_size += target_pop[region].Any * DpMediaDb.US_HH_SIZE;
            }

            current_plan.PopulationSize = target_pop_size;
        }

        

        

        public static void Insert<VType>(Double key, VType Value, IDictionary<Double, VType> dict)
        {
            if (key == 0)
            {
                key = 0.0001;
            }
            while (dict.ContainsKey(key))
            {
                key += key * 0.00001;
            }
            dict.Add(key, Value);
        }

        private void normailize<KType>(ref Dictionary<KType, double> dict)
        {
            double max = Double.MinValue;
            double min = Double.MaxValue;

        }

        

        #region old and in the way

        // private static double[,] num_vehicles = new double[6, 6] { { 1, 2, 3, 4, 5, 6 }, { 1, 2, 3, 4, 5, 6 }, { 1, 2, 3, 4, 5, 6 }, { 1, 2, 3, 4, 5, 6 }, { 1, 2, 3, 4, 5, 6 }, { 1, 2, 3, 4, 5, 6 } };

        //private bool remove_expensive( Dictionary<string, double> type_budget, Dictionary<string, double> type_price )
        //{
        //    SortedDictionary<double, string> sorted_by_price = new SortedDictionary<double, string>();
        //    foreach( string type in type_price.Keys )
        //    {
        //        double price_key = type_price[type];
        //        Insert<string>( -price_key, type, sorted_by_price );
        //    }

        //    foreach( string type in sorted_by_price.Values )
        //    {
        //        if( type_budget.ContainsKey( type ) )
        //        {
        //            if( type_budget[type] < type_price[type] )
        //            {
        //                type_budget.Remove( type );
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        /// <summary>
        /// computes the number of types to use based on the campaign goals
        /// </summary>
        /// <param name="campaign_specs"></param>
        /// <returns></returns>
        //private int get_num_types()
        //{
        //    return selection( compute_matrix( num_type_matrix ) ) + 2;
        //}

        /// <summary>
        /// computes the suggested number of subtypes per type
        /// </summary>
        /// <returns></returns>
        //private int get_num_sub_types()
        //{
        //    return selection( compute_matrix( num_subtype_matrix ) ) + 1;
        //}

        //private void read_variable( string name, int index, double[,] matrix )
        //{
        //    double reach;
        //    double persuasion;
        //    double recency;
        //    double demo_target;
        //    double geo_target;
        //    double interest_target;

        //    int reach_index = goal_location[MediaCampaignSpecs.CampaignGoal.ReachAndAwareness];
        //    int demo_index = goal_location[MediaCampaignSpecs.CampaignGoal.DemographicTargeting];
        //    int geo_index = goal_location[MediaCampaignSpecs.CampaignGoal.GeoTargeting];
        //    int persuasion_index = goal_location[MediaCampaignSpecs.CampaignGoal.Persuasion];
        //    int recency_index = goal_location[MediaCampaignSpecs.CampaignGoal.Recency];
        //    int interest_index = goal_location[MediaCampaignSpecs.CampaignGoal.InterestTargeting];

        //    database.GetVariableWegihts( name, out reach, out persuasion, out recency, out demo_target, out geo_target, out interest_target );
        //    matrix[reach_index, index] = reach;
        //    matrix[persuasion_index, index] = persuasion;
        //    matrix[recency_index, index] = recency;
        //    matrix[demo_index, index] = demo_target;
        //    matrix[geo_index, index] = geo_target;
        //    matrix[interest_index, index] = interest_target;
        //}


        //public void write_matrices_to_db()
        //{
        //    int reach_index = goal_location[MediaCampaignSpecs.CampaignGoal.ReachAndAwareness];
        //    int demo_index = goal_location[MediaCampaignSpecs.CampaignGoal.DemographicTargeting];
        //    int geo_index = goal_location[MediaCampaignSpecs.CampaignGoal.GeoTargeting];
        //    int persuasion_index = goal_location[MediaCampaignSpecs.CampaignGoal.Persuasion];
        //    int recency_index = goal_location[MediaCampaignSpecs.CampaignGoal.Recency];
        //    int interest_index = goal_location[MediaCampaignSpecs.CampaignGoal.InterestTargeting];
        //    foreach( string media_type in media_location.Keys )
        //    {
        //        int index = media_location[media_type];
        //        database.AddVariable( "media_allocation_" + media_type, media_allocation_matrix[reach_index, index],
        //            media_allocation_matrix[persuasion_index, index],
        //            media_allocation_matrix[recency_index, index],
        //            media_allocation_matrix[demo_index, index],
        //            media_allocation_matrix[geo_index, index],
        //            media_allocation_matrix[interest_index, index] );
        //    }

        //    database.AddVariable( "prominence_persuasion", prominence_selection_matrix[reach_index, 0],
        //        prominence_selection_matrix[persuasion_index, 0],
        //        prominence_selection_matrix[recency_index, 0],
        //        prominence_selection_matrix[demo_index, 0],
        //        prominence_selection_matrix[geo_index, 0],
        //        prominence_selection_matrix[interest_index, 0] );

        //    database.AddVariable( "prominence_awareness", prominence_selection_matrix[reach_index, 1],
        //        prominence_selection_matrix[persuasion_index, 1],
        //        prominence_selection_matrix[recency_index, 1],
        //        prominence_selection_matrix[demo_index, 1],
        //        prominence_selection_matrix[geo_index, 1],
        //        prominence_selection_matrix[interest_index, 1] );

        //    database.AddVariable( "prominence_recency", prominence_selection_matrix[reach_index, 2],
        //        prominence_selection_matrix[persuasion_index, 2],
        //        prominence_selection_matrix[recency_index, 2],
        //        prominence_selection_matrix[demo_index, 2],
        //        prominence_selection_matrix[geo_index, 2],
        //        prominence_selection_matrix[interest_index, 2] );

        //    database.AddVariable( "prominence_cost", prominence_selection_matrix[reach_index, 3],
        //        prominence_selection_matrix[persuasion_index, 3],
        //        prominence_selection_matrix[recency_index, 3],
        //        prominence_selection_matrix[demo_index, 3],
        //        prominence_selection_matrix[geo_index, 3],
        //        prominence_selection_matrix[interest_index, 3] );

        //    database.AddVariable( "vehicle_sort_size", vehicle_sorting_matrix[reach_index, 0],
        //        vehicle_sorting_matrix[persuasion_index, 0],
        //        vehicle_sorting_matrix[recency_index, 0],
        //        vehicle_sorting_matrix[demo_index, 0],
        //        vehicle_sorting_matrix[geo_index, 0],
        //        vehicle_sorting_matrix[interest_index, 0] );

        //    database.AddVariable( "vehicle_sort_cost", vehicle_sorting_matrix[reach_index, 1],
        //        vehicle_sorting_matrix[persuasion_index, 1],
        //        vehicle_sorting_matrix[recency_index, 1],
        //        vehicle_sorting_matrix[demo_index, 1],
        //        vehicle_sorting_matrix[geo_index, 1],
        //        vehicle_sorting_matrix[interest_index, 1] );

        //    database.AddVariable( "imps_per_vehicle_1", imps_per_vehicle_matrix[reach_index, 0],
        //        imps_per_vehicle_matrix[persuasion_index, 0],
        //        imps_per_vehicle_matrix[recency_index, 0],
        //        imps_per_vehicle_matrix[demo_index, 0],
        //        imps_per_vehicle_matrix[geo_index, 0],
        //        imps_per_vehicle_matrix[interest_index, 0] );

        //    database.AddVariable( "imps_per_vehicle_2", imps_per_vehicle_matrix[reach_index, 1],
        //        imps_per_vehicle_matrix[persuasion_index, 1],
        //        imps_per_vehicle_matrix[recency_index, 1],
        //        imps_per_vehicle_matrix[demo_index, 1],
        //        imps_per_vehicle_matrix[geo_index, 1],
        //        imps_per_vehicle_matrix[interest_index, 1] );

        //    database.AddVariable( "imps_per_vehicle_3", imps_per_vehicle_matrix[reach_index, 2],
        //        imps_per_vehicle_matrix[persuasion_index, 2],
        //        imps_per_vehicle_matrix[recency_index, 2],
        //        imps_per_vehicle_matrix[demo_index, 2],
        //        imps_per_vehicle_matrix[geo_index, 2],
        //        imps_per_vehicle_matrix[interest_index, 2] );


        //    database.AddVariable( "num_subtypes_1", num_subtype_matrix[reach_index, 0],
        //        num_subtype_matrix[persuasion_index, 0],
        //        num_subtype_matrix[recency_index, 0],
        //        num_subtype_matrix[demo_index, 0],
        //        num_subtype_matrix[geo_index, 0],
        //        num_subtype_matrix[interest_index, 0] );

        //    database.AddVariable( "num_subtypes_2", num_subtype_matrix[reach_index, 1],
        //        num_subtype_matrix[persuasion_index, 1],
        //        num_subtype_matrix[recency_index, 1],
        //        num_subtype_matrix[demo_index, 1],
        //        num_subtype_matrix[geo_index, 1],
        //        num_subtype_matrix[interest_index, 1] );

        //    database.AddVariable( "num_subtypes_3", num_subtype_matrix[reach_index, 2],
        //        num_subtype_matrix[persuasion_index, 2],
        //        num_subtype_matrix[recency_index, 2],
        //        num_subtype_matrix[demo_index, 2],
        //        num_subtype_matrix[geo_index, 2],
        //        num_subtype_matrix[interest_index, 2] );

        //    for( int i = 0; i < 7; i++ )
        //    {
        //        database.AddVariable( "num_types_" + (i + 1), num_type_matrix[reach_index, i],
        //            num_type_matrix[persuasion_index, i],
        //            num_type_matrix[recency_index, i],
        //            num_type_matrix[demo_index, i],
        //            num_type_matrix[geo_index, i],
        //            num_type_matrix[interest_index, i] );
        //    }
        //}

        //public void read_matrices_from_db()
        //{
        //    foreach( string media_type in media_location.Keys )
        //    {
        //        int index = media_location[media_type];
        //        read_variable( "media_allocation_" + media_type, index, media_allocation_matrix );
        //    }

        //    read_variable( "prominence_persuasion", 0, prominence_selection_matrix );
        //    read_variable( "prominence_awareness", 1, prominence_selection_matrix );
        //    read_variable( "prominence_recency", 2, prominence_selection_matrix );
        //    read_variable( "prominence_cost", 3, prominence_selection_matrix );

        //    read_variable( "vehicle_sort_size", 0, vehicle_sorting_matrix );
        //    read_variable( "vehicle_sort_cost", 1, vehicle_sorting_matrix );

        //    read_variable( "imps_per_vehicle_1", 0, imps_per_vehicle_matrix );
        //    read_variable( "imps_per_vehicle_2", 1, imps_per_vehicle_matrix );
        //    read_variable( "imps_per_vehicle_3", 2, imps_per_vehicle_matrix );

        //    read_variable( "num_subtypes_1", 0, num_subtype_matrix );
        //    read_variable( "num_subtypes_2", 1, num_subtype_matrix );
        //    read_variable( "num_subtypes_3", 2, num_subtype_matrix );

        //    for( int i = 0; i < 7; i++ )
        //    {
        //        read_variable( "num_types_" + (i + 1), i, num_type_matrix );
        //    }
        //}


        //private bool split_region()
        //{
        //    double total_cost = 0.0;
        //    foreach (string type in type_costs.Keys)
        //    {
        //        total_cost += type_costs[type];
        //    }

        //    double needed_money = total_cost / type_costs.Count * 3; // Math.Min( num_types, 3 );

        //    double fraction = current_plan.Specs.TargetBudget / needed_money;

        //    if (fraction > 0.75)
        //    {
        //        return false;
        //    }

        //    List<Demographic> demographics = new List<Demographic>();
        //    for (int i = 0; i < current_plan.Specs.Demographics.Count; i++)
        //    {
        //        demographics.Add(SimUtils.ConvertToDemographic(current_plan, i));
        //    }

        //    List<GeoRegion> DMAs = GeoRegion.TopGeo.GetRegions(GeoRegion.RegionType.DMA);
        //    Dictionary<GeoRegion, Demographic.PrimVector> geo_sizes = new Dictionary<GeoRegion, Demographic.PrimVector>();
        //    foreach (GeoRegion DMA in DMAs)
        //    {
        //        geo_sizes.Add(DMA, database.TargetPopulation(demographics, DMAs));
        //    }

        //    SortedDictionary<Double, GeoRegion> size_regions = new SortedDictionary<double, GeoRegion>();

        //    foreach (GeoRegion region in geo_sizes.Keys)
        //    {
        //        Insert<GeoRegion>(geo_sizes[region].Any, region, size_regions);
        //    }

        //    double smallest = size_regions.First().Key;

        //    if (smallest > fraction)
        //    {
        //        return false;
        //    }

        //    double current_fraction = 0.0;

        //    List<GeoRegion> split_regions = new List<GeoRegion>();

        //    while (current_fraction < fraction && (fraction - current_fraction) > smallest)
        //    {
        //        Double key = size_regions.Last(last => last.Key < (fraction - current_fraction)).Key;
        //        split_regions.Add(size_regions[key]);
        //        size_regions.Remove(key);
        //        current_fraction += key;
        //        if (key <= smallest)
        //        {
        //            smallest = size_regions.First().Key;
        //        }
        //    }

        //    target_pop = new Dictionary<GeoRegion, Demographic.PrimVector>();
        //    target_pop_size = 0.0;

        //    foreach (GeoRegion region in split_regions)
        //    {
        //        target_pop.Add(region, database.TargetPopulation(demographics, region));
        //        target_pop_size += target_pop[region].Any * DpMediaDb.US_HH_SIZE;
        //    }

        //    return true;
        //}



        //
        // FROM allocate_types()
        //
        //
        //            type_budgets = allocate(type_scores);

        //while (remove_expensive(type_budgets, type_costs))
        //{
        //    type_budgets = allocate(type_budgets);
        //}

        //hack for small budgets
        //if (type_budgets.Count == 0)
        //{
        //    type_budgets.Add("Internet", current_plan.Specs.TargetBudget);
        //    return false;
        //}

        //if (type_budgets.Count > num_types)
        //{
        //    int num_to_elimintate = type_budgets.Count - num_types;
        //    SortedDictionary<double, string> types = new SortedDictionary<double, string>();
        //    foreach (string type in type_budgets.Keys)
        //    {
        //        double price_key = type_budgets[type];
        //        Insert<string>(price_key, type, types);
        //    }
        //    int num_eliminated = 0;
        //    foreach (string type in types.Values)
        //    {
        //        type_budgets.Remove(type);
        //        num_eliminated++;
        //        if (num_eliminated >= num_to_elimintate)
        //        {
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// estimated cost for each media type
        /// </summary>
        /// <param name="campaign_specs"></param>
        /// <returns></returns>
        //private void get_type_costs()
        //{
        //    num_impressions = get_imp_per_vehicle();
        //    double avg_impressions = 0.0;
        //    for (int i = 0; i < num_impressions.Count; i++)
        //    {
        //        avg_impressions = (i + 1) * num_impressions[i];
        //    }
        //    plan_span = (current_plan.EndDate - current_plan.StartDate).Days;
        //    purchase_cycles = plan_span / purchase_cycle;
        //    double num_cpm = avg_impressions * target_pop_size * 0.001 * ReachFactor * purchase_cycles;

        //    type_costs = database.GetTypeCPM();
        //    List<string> types = database.GetMediaTypes();
        //    foreach (string type in types)
        //    {
        //        //Use 0.5 because most ad options are cheaper then full price
        //        if (type == "Newspaper")
        //        {
        //            type_costs[type] *= 20 * num_cpm * 0.5;
        //        }
        //        else
        //        {
        //            type_costs[type] *= num_cpm * 0.5;
        //        }
        //    }
        //}



        ////
        ////
        //// OLD CODE FROm select_vehicle
        ////
        ////

        //foreach (MediaVehicle vehicle in vehicles.Keys)
        //{
        //    double size = vehicles[vehicle] * DpMediaDb.US_HH_SIZE;
        //    double price_per_ad;
        //    double num_ads_per_cycle;
        //    double actual_price;
        //    double cycles_in_plan;
        //    double final_spot_price;

        //    if (used_vehicles.ContainsKey(vehicle.SubType + vehicle.Guid.ToString()))
        //    {
        //        continue;
        //    }

        //    if (!vehicle.GetOptions().ContainsKey(option.ID))
        //    {
        //        continue;
        //    }


        //    // Number of days one ad will run
        //    double days = Utils.DaysInAdCycle( vehicle.Cycle );
        //    if( type == "Newspaper" )
        //    {
        //        days = 1.1666;
        //        Newspaper paper = vehicle as Newspaper;
        //        if( paper.Distribute == Newspaper.Distribution.Sunday )
        //        {
        //            days = 7;
        //        }
        //    }
        //    else
        //    {
        //        days = Utils.DaysInAdCycle( vehicle.Cycle );
        //    }

        //    // how many ads we need to purchase for a continuous ads during purchase cycle
        //    double cycles_per_cycle = Math.Ceiling( purchase_cycle / days );

        //    // how many ads we need to purchase (continuous model
        //    cycles_in_plan = Math.Ceiling( plan_span / days );

        //    price_per_ad = Utils.GetSpotPrice(type, vehicle.CPM, option.Cost_Modifier, vehicle.Size / 1000, 0);

        //    num_ads_per_cycle = Math.Min( impressions, Math.Floor( budget / (price_per_ad * cycles_in_plan) ) );

        //    // calculate cpm of vehicle
        //    if (type == "Internet")
        //    {
        //        num_ads_per_cycle = size * impressions * purchase_cycles / cycles_per_cycle;
        //        num_ads_per_cycle = Math.Floor(Math.Min(budget / (price_per_ad * cycles_in_plan), num_ads_per_cycle));
        //    }

        //    final_spot_price = Utils.GetSpotPrice(type, vehicle.CPM, option.Cost_Modifier, vehicle.Size / 1000, 0);

        //    actual_price = Math.Max(final_spot_price * num_ads_per_cycle * cycles_in_plan, price_per_ad * purchase_cycles);

        //    if (num_ads_per_cycle == 0)
        //    {
        //        continue;
        //    }

        //    if( actual_price <= budget )
        //    {
        //        if (actual_price < min_price)
        //        {
        //            min_price = actual_price;
        //        }

        //        vehicle_price.Add(vehicle, new List<double>());
        //        vehicle_price[vehicle].Add(actual_price);
        //        vehicle_price[vehicle].Add(price_per_ad);
        //        vehicle_price[vehicle].Add(num_ads_per_cycle);

        //        if (vehicle_sorting == 0)
        //        {
        //            select_vehicles.Add(vehicle, size);
        //        }
        //        else
        //        {
        //            select_vehicles.Add(vehicle, size / final_spot_price);
        //        }
        //    }
        //}

        //max_option_price = 0.0;

        //if (vehicle_price.Count == 0)
        //{
        //    max_option_price = option.Cost_Modifier / (min_price / (budget * impressions));
        //    vehicle_price_num = null;
        //    return false;
        //}

        //vehicle_price_num = new Dictionary<MediaVehicle, List<double>>();

        //double total_value = 0.0;

        //while (total_value + min_price < budget && select_vehicles.Count > 0)
        //{
        //    // select from subtype list
        //    MediaVehicle vcl = select_vehicles.Select();

        //    if (vcl != null)
        //    {

        //        double price = vehicle_price[vcl][0];
        //        if (total_value + price <= budget && vcl != null)
        //        {
        //            total_value += price;
        //            vehicle_price_num.Add(vcl, vehicle_price[vcl]);
        //        }
        //    }
        //    else
        //    {
        //        bool stop = true;
        //    }
        //}

        #endregion
    }
}