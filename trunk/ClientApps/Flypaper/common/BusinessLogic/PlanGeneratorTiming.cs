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
        /// Main entry point for vehicle timing.  Requires a vehicle and a budget.
        /// </summary>
        /// <param name="vehicle">The vehicle to get timing for</param>
        /// <param name="budget">The budget for this vehicle, will contain left over money after timing is set (this could be negative if over spending occured)</param>
        /// <returns>A Dictionary of SimpleOptions and their respective ad dates</returns>
        private double add_vehicle_to_plan(MediaVehicle vehicle, double budget)
        {
            Dictionary<SimpleOption, List<DateTime>> rval = new Dictionary<SimpleOption, List<DateTime>>(); 

            //Get the rated options for this vehicle
            SelectDictionary<SimpleOption> options = get_rated_options(vehicle);
            //Get the minimum coverage for this vehicle
            double min_coverage = absolute_min_coverage(vehicle);
            //Get the minimum price point
            double min_cost = min_media_costs[vehicle.Type.ToString()];

            double current_budget = budget;
            bool added = false;

            //Main loop for setting timing
            while (options.Count > 0 && current_budget > min_cost)
            {
                //Get an option and the cost
                SimpleOption option = options.GetRandom();
                double cost = full_coverage_cost(vehicle, option);

                //See if we can afford the option using inflated budget
                if (cost * absolute_min_coverage(vehicle) > current_budget * 1.1)
                {
                    options.Remove(option);
                    continue;
                }

                //We can afford this option so use the vehicle
                added = true;
                add_vehicle(vehicle, option);

                //Compute best coverage option using an inflated budget
                double coverage = get_usable_coverage(vehicle, current_budget * 1.1 / cost);

                //Update the budget
                current_budget -= cost * coverage;
                
                List<DateTime> ad_dates;
                double num_ads;

                if(coverage >= 1.0)
                {
                    //Get the full coverage ad dates and num ads
                    ad_dates = get_all_cycle_days(vehicle);
                    double num_full_coverage = ((int)coverage);
                    num_ads = num_full_coverage * get_ads_per_cycle(vehicle);

                    //Update the timing
                    update_timing(vehicle, option, ad_dates, num_ads);

                    //Get the partial covereage
                    coverage = coverage - num_full_coverage;
                }

                if (coverage > 0.0)
                {
                    //Use the partial coverage values to fill out the rest of budget
                    ad_dates = get_ad_dates(vehicle, coverage);
                    num_ads = get_ads_per_cycle(vehicle);
                    if (vehicle.Type == MediaVehicle.MediaType.Internet)
                    {
                        //Internet modulates coverage by changing the number of ads...
                        num_ads *= coverage;
                    }
                    update_timing(vehicle, option, ad_dates, num_ads);
                }

                //Yeah added has to be true here, but added it in case that changes...
                if (vehicle.Type == MediaVehicle.MediaType.Yellowpages || vehicle.Type == MediaVehicle.MediaType.Newspaper || vehicle.Type == MediaVehicle.MediaType.Magazine)
                {
                    //Yellowpages should only run one ad per issue
                    break;
                }
            }

            //update the budget
            if (added)
            {
                MediaItem item = current_plan.GetMediaItem(vehicle);
                item.VehicleRating = vehicle_rating[vehicle];
                return (budget - item.TotalPrice);
            }
            else
            {
                return budget;
            }
        }

        /// <summary>
        /// Get the absolute minimum coverage for a vehicle based on type
        /// </summary>
        /// <param name="vehicle">The vehicle in question</param>
        /// <returns>Minimum coverage allowed</returns>
        private double absolute_min_coverage(MediaVehicle vehicle)
        {
            switch (vehicle.Type)
            {
                case MediaVehicle.MediaType.Internet:
                    //One tenth of one percent of the population
                    return 0.05;
                case MediaVehicle.MediaType.Radio:
                    //Only run 1 day a week
                    return 1.0 / 7.0;
                case MediaVehicle.MediaType.Newspaper:
                    if (vehicle.Cycle == MediaVehicle.AdCycle.Weekly)
                    {
                        //Newspaper is sunday so you have to use all of them
                        return 1.0;
                    }
                    else
                    {
                        //Newspaper is daily so only use 1 ad per week
                        return 1.0 / 7.0;
                    }
                case MediaVehicle.MediaType.Magazine:
                    //Alternate cycle magazines
                    return 0.5;
                case MediaVehicle.MediaType.Yellowpages:
                    //Have to run the whole time
                    return 1.0;
                default:
                    return 1.0;
            }
        }

        /// <summary>
        /// Computes the cost of full coverage for this vehicle using this ad_option
        /// </summary>
        /// <param name="vehicle">The vehicle to compute coverage for</param>
        /// <param name="option">The option to compute coverage for</param>
        /// <returns>The cost of full coverage</returns>
        private double full_coverage_cost(MediaVehicle vehicle, SimpleOption option)
        {
            return estimated_vehicle_cost(vehicle, option.Cost_Modifier);
        }

        /// <summary>
        /// Returns the number of ads required for full coverage using the specified vehicle
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <returns>The number of ads</returns>
        private double get_num_ads(MediaVehicle vehicle)
        {
            return get_all_cycle_days(vehicle).Count * get_ads_per_cycle(vehicle);
        }

        /// <summary>
        /// Returns the start days for all possible cycles for the given campaign specs for the specified vehicle
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <returns>List of cycle start dates</returns>
        private List<DateTime> get_all_cycle_days(MediaVehicle vehicle)
        {
            List<DateTime> rval = new List<DateTime>();
            DateTime cur_day = current_plan.StartDate;
            double cycle_days = Utils.DaysInAdCycle(vehicle.Cycle);
            while (cur_day <= current_plan.EndDate)
            {
                switch (vehicle.Type)
                {
                    case MediaVehicle.MediaType.Newspaper:
                        //Deal with Sunday papers
                        if (vehicle.Cycle == MediaVehicle.AdCycle.Weekly && cur_day.DayOfWeek == DayOfWeek.Sunday)
                        {
                            //This is a sunday paper and it is sunday so add to the cycles and move to next sunday
                            rval.Add(new DateTime(cur_day.Year, cur_day.Month, cur_day.Day));
                            cur_day = cur_day.AddDays(cycle_days);
                        }
                        else if (vehicle.Cycle == MediaVehicle.AdCycle.Weekly)
                        {
                            //This is a sunday paper but is it isn't sunday so just move forward one day until a sunday is reached
                            cur_day = cur_day.AddDays(1.0);
                        }
                        else if (cur_day.DayOfWeek != DayOfWeek.Sunday)
                        {
                            //This isn't a sunday paper it isn't sunday so add to the cycles and move forward a day
                            rval.Add(new DateTime(cur_day.Year, cur_day.Month, cur_day.Day));
                            cur_day = cur_day.AddDays(cycle_days);
                        }
                        else
                        {
                            //This isn't a sunday paper but is it sunday so just add a day
                            cur_day = cur_day.AddDays(cycle_days);
                        }
                        break;
                    case MediaVehicle.MediaType.Radio:
                    case MediaVehicle.MediaType.Internet:
                    case MediaVehicle.MediaType.Magazine:
                    case MediaVehicle.MediaType.Yellowpages:
                        //Everybody else is simple :)
                        rval.Add(new DateTime(cur_day.Year, cur_day.Month, cur_day.Day));
                        cur_day =  cur_day.AddDays(cycle_days);
                        break;
                }
            }
            return rval;
        }

        /// <summary>
        /// Returns the number of cycles the specified vehicle uses per day
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <returns>The number of ads per day</returns>
        private double get_ads_per_cycle(MediaVehicle vehicle)
        {
            switch (vehicle.Type)
            {
                case MediaVehicle.MediaType.Internet:
                    //Internet should hit approximately 1% of the target per day
                    return Math.Min(current_plan.PopulationSize*0.01,10000);
                case MediaVehicle.MediaType.Radio:
                    //Radio should put out 3 ads per day
                    //This is somewhat based on knowledge of how media plans are actually constructed
                    return 3.0;
                case MediaVehicle.MediaType.Newspaper:
                case MediaVehicle.MediaType.Magazine:
                case MediaVehicle.MediaType.Yellowpages:
                    //For print media 1 ad per cycle is effective
                    return 1.0;
                default:
                    return 1.0;
            }
        }

        /// <summary>
        /// Converts an arbitrary coverage number into an amount of coverage the vehicle is capable of supplying
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="max_coverage">The coverage amound to convert</param>
        /// <returns>The usuable coverage less than or equal to max_coverage</returns>
        private double get_usable_coverage(MediaVehicle vehicle, double max_coverage)
        {
            //Get minimum coverage
            double min_coverage = absolute_min_coverage(vehicle);
            //Compute usuable coverage at multiples of min coverage
            double usable = Math.Floor(max_coverage / min_coverage) * min_coverage;

            if (vehicle.Type == MediaVehicle.MediaType.Radio || vehicle.Type == MediaVehicle.MediaType.Yellowpages)
            {
                //Radio should use other options to fill out coverage
                //Yellowpages should only run a single ad
                return Math.Min(1.0, usable);
            }

            if (vehicle.Type == MediaVehicle.MediaType.Newspaper || vehicle.Type == MediaVehicle.MediaType.Magazine)
            {
                //Print media should use at most 100% coverage
                return Math.Min(1.0, usable);
            }

            return usable;
        }

        /// <summary>
        /// Gets the ad_dates for a vehicle based on the coverage, uses the vehicle type to set up various pulsing schedules
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="coverage">The coverage</param>
        /// <returns>A list of ad dates that confrom to the given covereage</returns>
        private List<DateTime> get_ad_dates(MediaVehicle vehicle, double coverage)
        {
            List<DateTime> rval = new List<DateTime>();

            //Initialize pulsing schedules for radio and newspapers
            List<DayOfWeek> valid_days = get_days(Math.Min(coverage, 1.0) * 7.0);
            if (vehicle.Type == MediaVehicle.MediaType.Newspaper && vehicle.Cycle == MediaVehicle.AdCycle.Daily)
            {
                //Daily papers don't run on sunday so remove it
                valid_days.Remove(DayOfWeek.Sunday);
            }

            //First compute all valid days
            List<DateTime> all_dates = get_all_cycle_days(vehicle);
            
            //Main loop, this will remove ad dates to get the correct coverage
            bool use_date = true;
            foreach (DateTime date in all_dates)
            {
                switch (vehicle.Type)
                {
                    case MediaVehicle.MediaType.Newspaper:
                        //Newspapers are tricky as Sunday papers always have 100% coverage
                        if (vehicle.Cycle != MediaVehicle.AdCycle.Weekly && valid_days.Contains(date.DayOfWeek))
                        {
                            //This is a daily paper so it the day is in the schedule use it
                            use_date = true;
                        }
                        else if (vehicle.Cycle == MediaVehicle.AdCycle.Weekly)
                        {
                            //This is a sunday paper, for now, always use it
                            use_date = true;
                        }
                        else
                        {
                            //The is not valid so don't use it
                            use_date = false;
                        }
                        break;
                    case MediaVehicle.MediaType.Radio:
                        //For radio simply stick to the pulsing schedule
                        if (valid_days.Contains(date.DayOfWeek))
                        {
                            use_date = true;
                        }
                        else
                        {
                            use_date = false;
                        }
                        break;
                    case MediaVehicle.MediaType.Magazine:
                        //If not full coverage then alternate cycles
                        if (coverage < 1.0)
                        {
                            use_date = !use_date;
                        }
                        break;
                    case MediaVehicle.MediaType.Internet:
                    case MediaVehicle.MediaType.Yellowpages:
                        //Yellowpages and Internet use all dates
                        //Internet coverage is modulated by number of ads
                        use_date = true;
                        break;
                    default:
                        use_date = true;
                        break;
                }
                if (use_date)
                {
                    //Date is valid so use it
                    rval.Add(date);
                }
            }
            return rval;
        }

        /// <summary>
        /// Utility function for setting up ad schedules for radio and newspaper
        /// </summary>
        /// <param name="num_days"></param>
        /// <returns></returns>
        private List<DayOfWeek> get_days(double num_days)
        {
            //Ad schedule loosly based on weekend shopping
            List<DayOfWeek> all_days = new List<DayOfWeek>();
            all_days.Add(DayOfWeek.Friday);
            all_days.Add(DayOfWeek.Wednesday);
            all_days.Add(DayOfWeek.Monday);
            all_days.Add(DayOfWeek.Thursday);
            all_days.Add(DayOfWeek.Tuesday);
            all_days.Add(DayOfWeek.Saturday);
            all_days.Add(DayOfWeek.Sunday);

            List<DayOfWeek> rval = new List<DayOfWeek>();
            for (int i = 0; i < num_days; i++)
            {
                rval.Add(all_days[i]);
            }

            return rval;
        }

        /// <summary>
        /// Adds the vehicle to the plan so that timing can be applied
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        private void add_vehicle(MediaVehicle vehicle, SimpleOption option)
        {
            MediaItem plan_vehicle = current_plan.AddMediaItem(vehicle, option);
            plan_vehicle.TimingList.Clear();
        }

        /// <summary>
        /// Updates the timing for the given vehicle using the specified option, dates, and ad counts
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="option">The ad option</param>
        /// <param name="ad_dates">The dates the ad will run on</param>
        /// <param name="ads_per_cycle">The number of ad per cycle</param>
        /// <returns></returns>
        private double update_timing(MediaVehicle vehicle, SimpleOption option, List<DateTime> ad_dates, double ads_per_cycle)
        {
            MediaItem vehicle_item = current_plan.GetMediaItem(vehicle);
            List<TimingInfo.TimingInfoItem> timing = new List<TimingInfo.TimingInfoItem>();
            foreach (DateTime date in ad_dates)
            {
                //For each date create a new item
                TimingInfo.TimingInfoItem item = new TimingInfo.TimingInfoItem();
                item.Date = date;
                item.AdCount = (int)ads_per_cycle;
                timing.Add(item);
            }

            //This function will handle the rest
            vehicle_item.AddTimingInfo(option, timing);
            return vehicle_item.TotalPrice;
        }
    }
}