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
        private SelectDictionary<MediaVehicle> get_rated_vehicles_with_subtypes(string type, List<GeoRegion> regions)
        {
            SelectDictionary<string> rated_subtypes = get_rated_subtypes(type, regions);
            SelectDictionary<MediaVehicle> rated_vehicles = get_vehicle_subtype_ratings(get_rated_vehicles(type, regions), rated_subtypes);
            return rated_vehicles;
        }

        private SelectDictionary<string> get_rated_subtypes(string type, List<GeoRegion> regions)
        {
            SelectDictionary<string> subtypes = new SelectDictionary<string>();

            Dictionary<string, Demographic.PrimVector> subtype_pops = database.GetSubTypeDemographics(type, regions);
            if (subtype_pops.Count == 0)
            {
                return subtypes;
            }

            Dictionary<string, double> reach = new Dictionary<string, double>();
            Dictionary<string, double> efficency = new Dictionary<string, double>();
            List<string> found_subs = new List<string>();

            foreach (string subtype in subtype_pops.Keys)
            {
                double target_size = 0.0;
                double subtype_size = 0.0;
                double intersect = 0.0;
                foreach (GeoRegion region in target_pop.Keys)
                {
                    for (int i = 0; i < target_pop[region].Count; i++)
                    {
                        target_size += target_pop[region][i] * target_pop[region].Any;
                        subtype_size += subtype_pops[subtype][i] * subtype_pops[subtype].Any;
                        intersect += subtype_pops[subtype][i] * Math.Ceiling(target_pop[region][i]) * subtype_pops[subtype].Any;
                    }
                }

                if (subtype_size > 0 && target_size > 0 && intersect > 0)
                {
                    found_subs.Add(subtype);
                    reach.Add(subtype, intersect / target_size);
                    efficency.Add(subtype, intersect / subtype_size);
                }
            }


            if (found_subs.Count == 0)
            {
                if (type == "Internet")
                {
                    subtypes.Add("ADNETWORK", 1);
                }
                return subtypes;
            }

            List<string> preferred = get_preferred_subtypes();

            //Build vectors for scaling values before rating
            List<double> reachs = new List<double>();
            List<double> efficencys = new List<double>();
            List<bool> in_categories = new List<bool>();
            List<double> sizes = new List<double>();
            foreach (string subtype in found_subs)
            {
                reachs.Add(reach[subtype]);
                efficencys.Add(efficency[subtype]);
                if (preferred.Exists(str => str == subtype))
                {
                    in_categories.Add(true);
                }
                else
                {
                    in_categories.Add(false);
                }
                sizes.Add(subtype_pops[subtype].Any);

            }
            List<double> scores = get_subtype_ratings(reachs, efficencys, in_categories, sizes);

            for (int i = 0; i < found_subs.Count; i++)
            {
                subtypes.Add(found_subs[i], scores[i]);
            }

            Normalize<string>(subtypes);

            return subtypes;
        }

        private List<string> get_preferred_subtypes()
        {
            string category = current_plan.Specs.BusinessCategory;
            string subcategory = current_plan.Specs.BusinessSubcategory;

            List<string> rval = new List<string>();

            if (category == "(choose a category)")
            {
                return rval;
            }

            Dictionary<string, int> categories = database.GetCategories();

            if (categories.ContainsKey(category))
            {
                int cat_id = categories[category];
                Dictionary<string, int> subCategories = database.GetSubCategories(cat_id);

                if (subCategories.ContainsKey(subcategory))
                {
                    int subcat_id = subCategories[subcategory];

                    Dictionary<string, int> subtypes = database.GetSubTypes(subcat_id);


                    rval.AddRange(subtypes.Keys);
                }
            }

            return rval;
        }

        private SelectDictionary<MediaVehicle> get_rated_vehicles(string type_name, List<GeoRegion> regions)
        {
            List<MediaVehicle> rval = database.GetMediaVehicleWithSize( type_name, regions );

            return get_rated_vehicles(rval, type_name);
        }

        private SelectDictionary<MediaVehicle> get_rated_vehicles(List<MediaVehicle> vehicles_sizes, string type_name)
        {
            SelectDictionary<MediaVehicle> rval = new SelectDictionary<MediaVehicle>();
            List<MediaVehicle> vehicles_list = new List<MediaVehicle>();
            List<double> sizes = new List<double>();
            List<double> costs = new List<double>();
            List<double> ratings = new List<double>();

            foreach (MediaVehicle vehicle in vehicles_sizes)
            {

                double size = vehicle.Size;
                double price_per_ad = Utils.GetSpotPrice(type_name, vehicle.CPM, 1.0, size / 1000, 1);

                vehicles_list.Add(vehicle);
                sizes.Add(size);
                costs.Add(price_per_ad);
                ratings.Add(0.0);
            }

            List<double> scores = get_vehicle_ratings(sizes, costs, ratings);

            for (int i = 0; i < vehicles_list.Count; i++)
            {
                vehicles_list[i].Rating = scores[i];
                rval.Add(vehicles_list[i], scores[i]);
            }

            Normalize<MediaVehicle>(rval);

            return rval;
        }

        private SelectDictionary<SimpleOption> get_rated_options(int type_id)
        {
            SelectDictionary<SimpleOption> rval = new SelectDictionary<SimpleOption>();
            Dictionary<int, AdOption> options = database.GetTypeOptions(type_id);
            List<SimpleOption> simple_options = new List<SimpleOption>();
            List<double> scores = new List<double>();


            foreach (AdOption option in options.Values)
            {
                SimpleOption simple = option as SimpleOption;
                if (simple != null)
                {
                    simple_options.Add(simple);
                }
            }

            if (simple_options.Count == 0)
            {
                return rval;
            }

            List<double> persuasions = new List<double>();
            List<double> awarenesss = new List<double>();
            List<double> recencys = new List<double>();
            List<double> cost_modifiers = new List<double>();
            for (int i = 0; i < simple_options.Count; i++)
            {
                persuasions.Add(simple_options[i].Persuasion);
                awarenesss.Add(simple_options[i].Awareness);
                recencys.Add(simple_options[i].Recency);
                cost_modifiers.Add(simple_options[i].Cost_Modifier);
            }

            scores = get_prominence_ratings(persuasions, awarenesss, recencys, cost_modifiers);


            for (int i = 0; i < scores.Count; i++)
            {
                rval.Add(simple_options[i], scores[i]);
            }

            Normalize<SimpleOption>(rval);

            return rval;
        }

        private SelectDictionary<SimpleOption> get_rated_options(MediaVehicle vehicle)
        {
            int type_id = database.GetTypeID(vehicle.Type.ToString());
            SelectDictionary<SimpleOption> rval = new SelectDictionary<SimpleOption>();
            Dictionary<int, AdOption> options = database.GetTypeOptions(type_id);
            List<SimpleOption> simple_options = new List<SimpleOption>();
            List<double> scores = new List<double>();


            foreach (AdOption option in options.Values)
            {
                SimpleOption simple = option as SimpleOption;
                if (simple != null && vehicle.GetOptions().ContainsKey(option.ID))
                {
                    simple_options.Add(simple);
                }
            }

            if (simple_options.Count == 0)
            {
                return rval;
            }

            List<double> persuasions = new List<double>();
            List<double> awarenesss = new List<double>();
            List<double> recencys = new List<double>();
            List<double> cost_modifiers = new List<double>();
            for (int i = 0; i < simple_options.Count; i++)
            {
                persuasions.Add(simple_options[i].Persuasion);
                awarenesss.Add(simple_options[i].Awareness);
                recencys.Add(simple_options[i].Recency);
                cost_modifiers.Add(simple_options[i].Cost_Modifier);
            }

            scores = get_prominence_ratings(persuasions, awarenesss, recencys, cost_modifiers);


            for (int i = 0; i < scores.Count; i++)
            {
                rval.Add(simple_options[i], scores[i]);
            }

            Normalize<SimpleOption>(rval);

            return rval;
        }

        private void Normalize<T>(SelectDictionary<T> dict)
        {
            double max = Double.MinValue;
            double min = Double.MaxValue;

            List<T> keys = new List<T>();

            foreach (T key in dict.Keys)
            {
                keys.Add(key);
                double value = dict[key];
                if (value > max)
                {
                    max = value;
                }

                if (value < min)
                {
                    min = value;
                }
            }

            max -= min;

            foreach (T key in keys)
            {
                dict[key] = (dict[key] - min) / max;
            }

            dict.Normalize();
        }
    }
}