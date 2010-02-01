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
        #region scoring interface

        public Dictionary<MediaVehicle.MediaType, double> GetTypeDollarsAllocation(MediaPlan plan)
        {
            this.current_plan = plan;

            get_target_pop();
            if (target_pop_size <= 0)
            {
                return null;
            }

            MediaCampaignSpecs specs = plan.PlanSpecs;

            plan_span = (specs.EndDate - specs.StartDate).Days;
            double bpm = 1000 * specs.TargetBudget / (plan_span * target_pop_size);

            Dictionary<string, double> type_allocations = allocate_types();

            double totalAllocation = 0;
            foreach (double bval in type_allocations.Values)
            {
                totalAllocation += bval;
            }

            Dictionary<MediaVehicle.MediaType, double> rval = new Dictionary<MediaVehicle.MediaType, double>();
            foreach (string typ in type_allocations.Keys)
            {
                rval.Add((MediaVehicle.MediaType)Enum.Parse(typeof(MediaVehicle.MediaType), typ), type_allocations[typ] * plan.TargetBudget / totalAllocation);
            }

            return rval;
        }

        #endregion


        #region edit plan interface

        /// <summary>
        /// Returns a SelectDictionary of all the simple options associated with a give media type
        /// </summary>
        /// <param name="media_type">Media type to get options for</param>
        /// <param name="plan">The plan for which the options will be rated</param>
        /// <returns></returns>
        public SelectDictionary<SimpleOption> GetRatedProminence(MediaVehicle.MediaType media_type, MediaPlan plan)
        {
            current_plan = plan;
            int type_id = database.GetTypeID(media_type.ToString());

            if(!rated_options.ContainsKey(type_id))
            {
                rated_options.Add(type_id, get_rated_options(type_id));
            }

            return rated_options[type_id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_name"></param>
        /// <param name="region_name"></param>
        /// <param name="option_id"></param>
        /// <param name="plan"></param>
        /// <param name="scaleForUser"></param>
        /// <param name="minRating"></param>
        /// <param name="maxRating"></param>
        /// <returns></returns>
        public Dictionary<MediaVehicle, double> GetRatedVehicles(string type_name, string region_name, int option_id, MediaPlan plan, bool scaleForUser,
            out double minRating, out double maxRating)
        {
            Dictionary<MediaVehicle, double> rval = new Dictionary<MediaVehicle, double>();

            current_plan = plan;

            get_target_pop();

            List<GeoRegion> regions = new List<GeoRegion>();

            regions.Add( GeoRegion.TopGeo.GetSubRegion( region_name ) );
            List<string> preferred = get_preferred_subtypes();
            SelectDictionary<string> rated_subtypes = get_rated_subtypes(type_name, regions);
            SelectDictionary < MediaVehicle > rated_vehicles = get_vehicle_subtype_ratings(get_rated_vehicles(type_name, regions), rated_subtypes);
            maxRating = double.MinValue;
            minRating = double.MaxValue;
            foreach (MediaVehicle vehicle in rated_vehicles.Keys)
            {
                double val = rated_vehicles[vehicle];
                rval.Add(vehicle, val);

                if (val < minRating)
                {
                    minRating = val;
                }

                if (val > maxRating)
                {
                    maxRating = val;
                }
            }

            if (scaleForUser == true)
            {
                foreach (MediaVehicle vehicle in rated_vehicles.Keys)
                {
                    rval[vehicle] = DisplayRating(rval[vehicle], minRating, maxRating);
                }
            }

            return rval;
        }

        /// <summary>
        /// Returns a rating value that is scaled on a 0-100 scale (for display to the user)
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="minRating"></param>
        /// <param name="maxRating"></param>
        /// <returns></returns>
        public static double DisplayRating(double rating, double minRating, double maxRating)
        {
            if (maxRating - minRating > 0.001)
            {
                double scale = 100 / (maxRating - minRating);
                return Math.Max(scale * (rating - minRating), 0);
            }
            else
            {
                return rating;
            }
        }


        #endregion
    }
}