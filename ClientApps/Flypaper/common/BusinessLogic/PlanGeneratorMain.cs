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
    /// <summary>
    /// Summary description for AlocationService
    /// </summary>
    public partial class AllocationService
    {
        /// <summary>
        /// Fills the current media plan with media items, will perserve and work around existing media items
        /// </summary>
        /// <param name="set_type_budgets">Set type budget to use otherwise type budget will be set using campaign goals</param>
        /// <param name="existing_vehicles">A set of existing vehicles to use</param>
        private void buildplan(Dictionary<string, double> set_type_budgets, List<Guid> vehicles_to_use)
        {
            SelectDictionary<string> type_budgets;
            //First check if the types have been budgeted
            if (set_type_budgets != null)
            {
                type_budgets = new SelectDictionary<string>();
                foreach (string type in set_type_budgets.Keys)
                {
                    type_budgets.Add(type, set_type_budgets[type]);
                }
            }
            else
            {
                //Types haven't been budgeted so allocate over the types using the campaign goals
                type_budgets = allocate_types();
            }

            //Adjust budget for existing vehicles
            type_budgets = adjust_budget_for_existing_vehicles(type_budgets, out used_vehicles);

            //Grab the list of regions in this plan
            List<GeoRegion> regions = new List<GeoRegion>(target_pop.Keys);

            //Get the budgeted vehicles for each type
            Dictionary<string, SelectDictionary<MediaVehicle>> type_vehicles = new Dictionary<string, SelectDictionary<MediaVehicle>>();
            vehicle_rating = new Dictionary<MediaVehicle, double>();
            foreach (string type in type_budgets.Keys)
            {
                SelectDictionary<MediaVehicle> rated_vehicles = get_rated_vehicles_with_subtypes(type, regions);
                type_vehicles.Add(type, get_type_vehicles(type, rated_vehicles, type_budgets[type], vehicles_to_use));
            }

            //Budget over types in a set order with the more flexible items last
            List<string> type_order = new List<string>();
            type_order.Add(MediaVehicle.MediaType.Magazine.ToString());
            type_order.Add(MediaVehicle.MediaType.Newspaper.ToString());
            type_order.Add(MediaVehicle.MediaType.Radio.ToString());
            type_order.Add(MediaVehicle.MediaType.Yellowpages.ToString());
            type_order.Add(MediaVehicle.MediaType.Internet.ToString());

            double extra_budget = 0.0;
            foreach (string type_name in type_order)
            {
                //Check if we are using this type
                if (!type_vehicles.ContainsKey(type_name))
                {
                    continue;
                }

                //Loop over vehicles
                List<MediaVehicle> order = get_vehicle_order(type_vehicles[type_name]);
                foreach(MediaVehicle vehicle in order)
                {
                    //Add the vehicle to the plan and store the extra budget (could be negative)
                    extra_budget = add_vehicle_to_plan(vehicle, type_vehicles[type_name][vehicle] + extra_budget);
                }
            }
        }

        /// <summary>
        /// Computes the type budget for the current media plan using the campaign goals
        /// </summary>
        /// <returns>Returns a Dictionary where the key is the type name and the value is the budget</returns>
        private SelectDictionary<string> allocate_types()
        {
            SelectDictionary<string> rval;

            //Compute the bpm, which is the dollars per person per day for the current plan
            double current_budget = current_plan.Specs.TargetBudget;
            plan_span = (current_plan.EndDate - current_plan.StartDate).Days;
            double bpm = 1000 * current_budget / (plan_span * target_pop_size);

            //Initialize the type score matrix
            SelectDictionary<string> type_scores = new SelectDictionary<string>();
            foreach (string type in media_location.Keys)
            {
                type_scores.Add(type, 0);
            }

            //Compute the type scores based on the type_weighting matrix
            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                foreach (string type in media_location.Keys)
                {
                    type_scores[type] += type_weighting_matrix[goal_location[current_plan.Specs.CampaignGoals[i]], media_location[type]] * current_plan.Specs.GoalWeights[i];
                }
            }

            //Remove media types that are not supported do not support for now
            type_scores.Remove("Direct Marketing");
            type_scores.Remove("TV");

            //Internet is Search so use the Search score if it is higher
            //TBD - need to make sure to select search vehicles in this case
            if (type_scores["Search"] > type_scores["Internet"])
            {
                type_scores["Internet"] = type_scores["Search"];
            }
            type_scores.Remove("Search");


            //bmp threshold is 1 penny per 1000 HH per day
            //use this limit the number of types used in the plan
            double treshold = 0.01;
            int numToKeep = type_scores.Count;
            if (bpm < treshold)
            {
                numToKeep = 1;
            }
            else if (bpm < 5 * treshold)
            {
                numToKeep = 2;
            }
            else if (bpm < 20 * treshold)
            {
                numToKeep = 3;
            }
            else if (bpm < 50 * treshold)
            {
                numToKeep = 4;
            }

            //Remove types until we have the desired number
            while (type_scores.Count > numToKeep)
            {
                //Remove the most expensive types first, exluding internet which will always be used
                KeyValuePair<string, double> remove = type_scores.Where(row => row.Key != "Internet").OrderBy(row => row.Value).First();
                type_scores.Remove(remove.Key);
            }

            rval = normalize_to_budget<string>(type_scores, current_budget);

            return rval;
        }

        /// <summary>
        /// Function for adjusting the type budgets to account for existing vehicles
        /// </summary>
        /// <param name="type_budgets">Current type budgets</param>
        /// <param name="existing_vehicles">Parameter will contain the existing vehicles found in the plan</param>
        /// <returns>New type budgets adjusted for the existing vehicles</returns>
        private SelectDictionary<string> adjust_budget_for_existing_vehicles(SelectDictionary<string> type_budgets, out SelectDictionary<Guid> existing_vehicles)
        {
            List<MediaItem> items = current_plan.GetAllItems();

            //First gather the current vehicles along with there total price
            //Organize the current vehicles by type
            Dictionary<string, SelectDictionary<Guid>> current_vehicles = new Dictionary<string, SelectDictionary<Guid>>();
            foreach (MediaItem item in items)
            {
                string type = item.GetType().ToString();
                Guid guid = item.vehicle_id;
                double cost = item.TotalPrice;

                if (!current_vehicles.ContainsKey(type))
                {
                    current_vehicles.Add(type, new SelectDictionary<Guid>());
                }

                if (!current_vehicles[type].ContainsKey(guid))
                {
                    current_vehicles[type].Add(guid, cost);
                }
            }

            //Now find the new type budgets
            //Start by finding the current total budget
            double total = 0.0;
            foreach (string type in type_budgets.Keys)
            {
                total += type_budgets[type];
            }

            //Now subtract off types that exist in the plan but are not in the current budget
            foreach (string type in current_vehicles.Keys)
            {
                if (!type_budgets.ContainsKey(type))
                {
                    foreach (Guid key in current_vehicles[type].Keys)
                    {
                        total -= current_vehicles[type][key];
                    }
                }
            }

            //Normalize to the new budget
            SelectDictionary<string> rval = normalize_to_budget(type_budgets, total);

            //Subtract off existing vehicles for the types that are budgeted for
            //TBD - budgets could be negative at this point
            existing_vehicles = new SelectDictionary<Guid>();
            foreach (string type in current_vehicles.Keys)
            {
                if (type_budgets.ContainsKey(type))
                {
                    foreach (Guid key in current_vehicles[type].Keys)
                    {
                        rval[type] -= current_vehicles[type][key];
                        existing_vehicles.Add(key, current_vehicles[type][key]);
                    }
                }
            }

            return rval;
        }



        /// <summary>
        /// Normalize a set of budgets to have a new total equal to new_budget, does not modify budgets
        /// </summary>
        /// <param name="type_budgets">The budgets to normalize, where the current weighting is the value</param>
        /// <param name="new_budget">The new budget to normalize to</param>
        /// <returns>A new dictionary with the normalized budgets at the values</returns>
        private SelectDictionary<Ktype> normalize_to_budget<Ktype>(SelectDictionary<Ktype> budgets, double new_budget)
        {
            SelectDictionary<Ktype> rval = new SelectDictionary<Ktype>();

            //Normalize the scores and multiply by the budget
            //Start by computing the total after types have been removed
            double total = 0.0;
            foreach (double value in budgets.Values)
            {
                total += value;
            }

            //TBD - if this fails then we have problems...throw error?
            if (total > 0)
            {
                //Normalize the type scores and apply the budget
                foreach (Ktype item in budgets.Keys)
                {
                    rval.Add(item, new_budget * budgets[item] / total);
                }
            }

            rval.Normalize();

            return rval;
        }


        /// <summary>
        /// Gets the vehicles and their budgets for the given type and type budget
        /// </summary>
        /// <param name="type">The media type to select vehicles for</param>
        /// <param name="vehicles">The weighted dictionary of vehicles for this type</param>
        /// <param name="budget">The target budget for the type</param>
        /// <returns></returns>
        private SelectDictionary<MediaVehicle> get_type_vehicles(string type, SelectDictionary<MediaVehicle> vehicles, double budget, List<Guid> use_vehicles)
        {
            //Gather initial values
            //Get the min percent coverage a vehicle can have
            double min_percentage = get_min_percentage(type);
            //Get the actual percent coverage a vehicle will use
            double use_percentage = get_use_percentage(type);
            //Minimum budget a vehicle can use for this type
            double min_cost = min_media_costs[type];

            double current_budget = budget;


            //Get the expected value of the cost modifier for the vehicle ad options
            double option_modifier = expected_option_modifier(type);

            //This loop is what builds the vehicles for the type
            SelectDictionary<MediaVehicle> rval = new SelectDictionary<MediaVehicle>();

            //First build dictionary of vehicles to use
            SelectDictionary<MediaVehicle> vehicles_to_use = new SelectDictionary<MediaVehicle>();
            if (used_vehicles.Count > 0)
            {
                foreach (MediaVehicle vehicle in vehicles.Keys)
                {
                    if (use_vehicles.Contains(vehicle.Guid))
                    {
                        vehicles_to_use.Add(vehicle, vehicles[vehicle]);
                    }
                }
            }

            //Continue to loop while the current budget is greater than the min cost and there are vehicles left to select from
            while (current_budget > min_cost && vehicles.Count > 0)
            {
                MediaVehicle vehicle;

                //Select a vehicle at random based on weight
                if (vehicles_to_use.Count > 0)
                {
                    vehicle = vehicles_to_use.Select();
                }
                else
                {
                    vehicle = vehicles.GetRandom();
                }

                if (used_vehicles.ContainsKey(vehicle.Guid))
                {
                    vehicles.Remove(vehicle);
                    continue;
                }
                
                //Compute the expected cost of the vehicle using full coverage
                double cost = estimated_vehicle_cost(vehicle, option_modifier);

                //See if the vehicle fits the budget using the min coverage and giving the budget 10% flexibility
                if ((cost * min_percentage) < (current_budget * 1.1))
                {
                    //Subtract the full use percentage and add the vehicle
                    current_budget -= cost * use_percentage;
                    rval.Add(vehicle, vehicles[vehicle]);
                    vehicle_rating.Add(vehicle, vehicles[vehicle]);
                }

                //Remove the vehicle from the dictionary so we don't select it again
                vehicles.Remove(vehicle);
            }

            //Normalize the vehicle list to the budget based on the weight
            rval = normalize_to_budget<MediaVehicle>(rval, budget);

            //Remove vehicles that are below the minimum cost
            while (below_min_cost(type, rval))
            {
                //Re-normalize the budget
                rval = normalize_to_budget<MediaVehicle>(remove_lowest(rval), budget);
            }

            //Return the list of vehicles
            return rval;
        }

        /// <summary>
        /// Computes the expected cost modifier for the ad options
        /// </summary>
        /// <param name="type">The media type to compute the expected value over</param>
        /// <returns>The expected cost modifier</returns>
        private double expected_option_modifier(string type)
        {
            //Get the rated options
            SelectDictionary<SimpleOption> options = get_rated_options(database.GetTypeID(type));
            double cost_modifier = 0.0;

            //Compute the expected cost modifier based on the option weights
            double total = 0.0;
            foreach (SimpleOption option in options.Keys)
            {
                cost_modifier += option.Cost_Modifier * options[option];
                total += options[option];
            }

            cost_modifier /= total;

            return cost_modifier;

        }

        /// <summary>
        /// Estimates the vehicle cost based on the expected option cost modifier
        /// </summary>
        /// <param name="vehicle">MediaVehicle to estimate the cost for</param>
        /// <param name="option_cost_modifier">Ad Option cost modifier</param>
        /// <returns></returns>
        private double estimated_vehicle_cost(MediaVehicle vehicle, double option_cost_modifier)
        {
            double rval = 0.0;
            if (vehicle.Type == MediaVehicle.MediaType.Internet)
            {
                rval = get_num_ads(vehicle) * vehicle.CPM * option_cost_modifier;
            }
            else
            {
                rval = get_num_ads(vehicle) * vehicle.Size / 1000 * vehicle.CPM * option_cost_modifier;
            }

            return rval;
        }

        /// <summary>
        /// Checks if any vehicles are below the minimum vehicle budget for this type
        /// </summary>
        /// <param name="type">The media type</param>
        /// <param name="budgets">The vehicle budgets</param>
        /// <returns>True if any vehicle is below the minimum cost, false otherwise</returns>
        private bool below_min_cost(string type, Dictionary<MediaVehicle, double> budgets)
        {
            double min_cost = min_media_costs[type];
            foreach (MediaVehicle vehicle in budgets.Keys)
            {

                if (budgets[vehicle] < min_cost)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the lowest budgeted vehicle from the list of vehicles
        /// </summary>
        /// <param name="budgets">The vehicle budgets from which the remove the lowest</param>
        /// <returns>A new dictionary with the lowest budgeted vehicle removed</returns>
        private SelectDictionary<MediaVehicle> remove_lowest(SelectDictionary<MediaVehicle> budgets)
        {
            double min_budget = Double.MaxValue;
            MediaVehicle lowest = null;
            SelectDictionary<MediaVehicle> rval = new SelectDictionary<MediaVehicle>();
            foreach (MediaVehicle vehicle in budgets.Keys)
            {
                if (budgets[vehicle] < min_budget)
                {
                    min_budget = budgets[vehicle];
                    lowest = vehicle;
                }

                rval.Add(vehicle, budgets[vehicle]);
            }

            rval.Remove(lowest);
            return rval;
        }

        /// <summary>
        /// Returns a list of vehicles sorted by size
        /// </summary>
        /// <param name="vehicles">Dictionary of vehicles</param>
        /// <returns>List of vehicles sorted by size</returns>
        private List<MediaVehicle> get_vehicle_order(SelectDictionary<MediaVehicle> vehicles)
        {
            List<MediaVehicle> rval = new List<MediaVehicle>();

            SortedDictionary<double, MediaVehicle> sorted_vehicles = new SortedDictionary<double, MediaVehicle>();
            foreach (MediaVehicle vehicle in vehicles.Keys)
            {
                AllocationService.Insert<MediaVehicle>(vehicle.Size, vehicle, sorted_vehicles);
            }

            foreach (MediaVehicle vehicle in sorted_vehicles.Values)
            {
                rval.Add(vehicle);   
            }

            return rval;
        }

    }
}