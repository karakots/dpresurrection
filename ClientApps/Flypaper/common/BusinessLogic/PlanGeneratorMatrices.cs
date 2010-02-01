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
        public static void Load_Matrices(StreamReader rdr)
        {
            read_matrices_from_file(rdr);
            LoadedMatrices = true;
        }

        //TODO add matrices for
        // goals -> (reach, effeciency, useCatagory, sub type size, num vehicles(?))
        // add extra media_allocation for different pdd

        public static bool LoadedMatrices = false;
        private static double[,] type_weighting_matrix = new double[6, 8] { { 5, 6, 4, 2, 5, 3, 5, 5 }, { 2, 2, 0, 6, 0, 4, 2, 1 }, { 6, 6, 6, 3, 6, 6, 3, 3 }, { 3, 6, 3, 6, 0, 0, 0, 2 }, { 4, 1, 2, 2, 6, 1, 2, 6 }, { 2, 4, 3, 6, 4, 3, 6, 6 } };
        private static double[,] prominence_weighting_matrix = new double[6, 4] { { 1, 3, 1, -7 }, { 5, 3, 1, -3 }, { 5, 3, 1, -3 }, { 5, 3, 3, -1 }, { 3, 2, 5, -2 }, { 5, 3, 1, -3 } };
        private static double[,] vehicle_weighting_matrix = new double[6, 3] { { 1, 3, 0 }, { 3, 2, 0 }, { 3, 2, 0 }, { 4, 1, 0 }, { 2, 3, 0 }, { 3, 2, 0 } };
        private static double[,] subtype_weighting_matrix = new double[6, 4] { { 5, 1, 3, 5 }, { 1, 5, 5, 3 }, { 3, 3, 3, 3 }, { 5, 3, 3, 3 }, { 3, 3, 3, 3 }, { 3, 5, 5, 1 } };
        private static double[,] vehicle_subtype_weighting_matrix = new double[6, 2] { { 5, 3 }, { 1, 7 }, { 4, 4 }, { 6, 2 }, { 4, 4 }, { 1, 7 } };
        private static double[,] num_vehicles_per_type_matrix = new double[6, 4] { { 0, 0, 3, 5 }, { 4, 4, 0, 0 }, { 2, 3, 3, 0 }, { 5, 3, 0, 0 }, { 2, 2, 2, 2 }, { 4, 4, 0, 0 } };
        private static double[,] ads_per_vehicle_matrix = new double[6, 5] { { 5, 3, 1, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 1, 3, 5 }, { 0, 0, 1, 3, 5 }, { 0, 0, 1, 3, 5 } };

        private static Dictionary<MediaCampaignSpecs.CampaignGoal, int> goal_location;
        private static Dictionary<string, int> media_location;
        private static Dictionary<string, int> min_media_costs;
        private static Dictionary<string, int> max_media_costs;

        private static void fill_location_dictionaries()
        {
            goal_location = new Dictionary<MediaCampaignSpecs.CampaignGoal, int>();
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.ReachAndAwareness, 0);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.DemographicTargeting, 1);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.GeoTargeting, 2);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.Persuasion, 3);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.Recency, 4);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.InterestTargeting, 5);

            media_location = new Dictionary<string, int>();
            media_location.Add("Radio", 0);
            media_location.Add("TV", 1);
            media_location.Add("Newspaper", 2);
            media_location.Add("Magazine", 3);
            media_location.Add("Yellowpages", 4);
            media_location.Add("Direct Marketing", 5);
            media_location.Add("Internet", 6);
            media_location.Add("Search", 7);

            min_media_costs = new Dictionary<string, int>();
            min_media_costs.Add("Radio", 500);
            min_media_costs.Add("TV", 5000);
            min_media_costs.Add("Newspaper", 500);
            min_media_costs.Add("Magazine", 1000);
            min_media_costs.Add("Yellowpages", 100);
            min_media_costs.Add("Direct Marketing", 200);
            min_media_costs.Add("Internet", 100);
            min_media_costs.Add("Search", 100);

            max_media_costs = new Dictionary<string, int>();
            max_media_costs.Add("Radio", 20000);
            max_media_costs.Add("TV", 1000000);
            max_media_costs.Add("Newspaper", 200000);
            max_media_costs.Add("Magazine", 500000);
            max_media_costs.Add("Yellowpages", 20000);
            max_media_costs.Add("Direct Marketing", 10000);
            max_media_costs.Add("Internet", 100000);
            max_media_costs.Add("Search", 50000);
        }

        public static void write_matrices_to_file(StreamWriter str)
        {
            List<double> genome = write_genome();
            foreach (double value in genome)
            {
                str.WriteLine(value.ToString());
            }
        }

        private static void read_matrices_from_file(StreamReader str)
        {
            List<double> genome = new List<double>();
            while (!str.EndOfStream)
            {
                double value = Double.Parse(str.ReadLine());
                genome.Add(value);
            }

            read_matrices_from_genome(genome);
        }

        public static List<double> write_genome()
        {
            List<double> genome = new List<double>();
            write_matrix(type_weighting_matrix, ref genome);
            write_matrix(prominence_weighting_matrix, ref genome);
            write_matrix(vehicle_weighting_matrix, ref genome);
            write_matrix(subtype_weighting_matrix, ref genome);
            write_matrix(vehicle_subtype_weighting_matrix, ref genome);
            write_matrix(ads_per_vehicle_matrix, ref genome);

            return genome;
        }

        private static void write_matrix(double[,] matrix, ref List<double> genome)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < cols; k++)
                {
                    genome.Add(matrix[i, k]);
                }
            }
        }

        public static void read_matrices_from_genome(List<double> values)
        {
            int current_index = 0;
            read_matrix(values, ref current_index, type_weighting_matrix);
            read_matrix(values, ref current_index, prominence_weighting_matrix);
            read_matrix(values, ref current_index, vehicle_weighting_matrix);
            read_matrix(values, ref current_index, subtype_weighting_matrix);
            read_matrix(values, ref current_index, vehicle_subtype_weighting_matrix);
            read_matrix(values, ref current_index, ads_per_vehicle_matrix);
        }

        private static void read_matrix(List<double> values, ref int current_index, double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < cols; k++)
                {
                    matrix[i, k] = values[current_index];
                    current_index++;
                }
            }
        }

        /// <summary>
        /// computes the suggested number of vehicles per subtype
        /// </summary>
        /// <returns></returns>
        private int get_vehicle_sorting()
        {
            return max_selection(compute_matrix(vehicle_weighting_matrix));
        }

        /// <summary>
        /// Computes the minimum percent converage for a vehicle
        /// </summary>
        /// <returns>Minimum percent converage</returns>
        private double get_min_percentage(string type)
        {
            int choice = selection(compute_matrix(num_vehicles_per_type_matrix));
            if (type == MediaVehicle.MediaType.Radio.ToString() || type == MediaVehicle.MediaType.Newspaper.ToString())
            {
                switch (choice)
                {
                    case 0:
                        return 1.0;
                    case 1:
                        return 0.75;
                    case 2:
                        return 0.5;
                    case 3:
                        return 0.25;
                    default:
                        return 0.5;
                }
            }
            else if (type == MediaVehicle.MediaType.Internet.ToString())
            {
                switch (choice)
                {
                    case 0:
                        return 0.1;
                    case 1:
                        return 0.06;
                    case 2:
                        return 0.03;
                    case 3:
                        return 0.01;
                    default:
                        return 0.03;
                }
            }
            else
            {
                switch (choice)
                {
                    case 0:
                        return 1.0;
                    case 1:
                        return 1.0;
                    case 2:
                        return 0.75;
                    case 3:
                        return 0.5;
                    default:
                        return 1.0;
                }
            }
        }

        /// <summary>
        /// Computes the percent converage to assume usage for a media vehicle
        /// </summary>
        /// <returns>Minimum percent converage</returns>
        private double get_use_percentage(string type)
        {
            int choice = selection(compute_matrix(num_vehicles_per_type_matrix));
            if (type == MediaVehicle.MediaType.Radio.ToString() || type == MediaVehicle.MediaType.Newspaper.ToString() || type == MediaVehicle.MediaType.Internet.ToString())
            {
                switch (choice)
                {
                    case 0:
                        return 1.0;
                    case 1:
                        return 1.0;
                    case 2:
                        return 0.75;
                    case 3:
                        return 0.5;
                    default:
                        return 0.75;
                }
            }
            else
            {
                switch (choice)
                {
                    case 0:
                        return 1.0;
                    case 1:
                        return 1.0;
                    case 2:
                        return 1.0;
                    case 3:
                        return 1.0;
                    default:
                        return 1.0;
                }
            }
        }

        /// <summary>
        /// Returns the rating of the current vehicle based on the goal weights
        /// </summary>
        /// <param name="size">vehicle size</param>
        /// <param name="cost">vehicle cost</param>
        /// <param name="user_rating">vehicle user rating</param>
        /// <returns></returns>
        private double get_vehicle_rating(double size, double cost, double user_rating)
        {
            double score = 0;
            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                int index = goal_location[current_plan.Specs.CampaignGoals[i]];
                score += vehicle_weighting_matrix[index, 0] * current_plan.Specs.GoalWeights[i] * size;
                score += vehicle_weighting_matrix[index, 1] * current_plan.Specs.GoalWeights[i] * cost;
                score += vehicle_weighting_matrix[index, 2] * current_plan.Specs.GoalWeights[i] * user_rating;
            }
            return score;
        }

        /// <summary>
        /// Returns a list of ratings corresponding to the lists of inputs, performs scaling on the inputs to guarantee the inputs are between 0 and 1
        /// </summary>
        /// <param name="sizes">List of the sizes for the vehicles</param>
        /// <param name="costs">List of the costs for the vehicles</param>
        /// <param name="user_ratings">List of the ratings for the vehicles</param>
        /// <returns></returns>
        private List<double> get_vehicle_ratings(List<double> sizes, List<double> costs, List<double> user_ratings)
        {
            List<double> rval = new List<double>();

            if( sizes.Count == 0 )
            {
                return rval;
            }

            double max_size = sizes.Max();
            double min_size = sizes.Min();
            double max_cost = costs.Max();
            double min_cost = costs.Min();
            double max_user_rating = user_ratings.Max();
            double min_user_rating = user_ratings.Min();

            for (int i = 0; i < sizes.Count; i++)
            {
                double scaled_size = (sizes[i] - min_size) / (max_size - min_size);
                if (Double.IsNaN(scaled_size))
                {
                    scaled_size = 0.5;
                }
                double scaled_cost = (costs[i] - min_cost) / (max_cost - min_cost);
                if (Double.IsNaN(scaled_cost))
                {
                    scaled_cost = 0.5;
                }
                double scaled_rating = (user_ratings[i] - min_user_rating) / (max_user_rating - min_user_rating);
                if (Double.IsNaN(scaled_rating))
                {
                    scaled_rating = 0.5;
                }
                rval.Add(get_vehicle_rating(scaled_size, scaled_cost, scaled_rating));
            }

            return rval;
        }

        /// <summary>
        /// Returns the rating of the prominence based on the goal weights
        /// </summary>
        /// <param name="persuasion">ad options persuasion modifier</param>
        /// <param name="awareness">ad options awareness modifier</param>
        /// <param name="recency">ad options recency modifier</param>
        /// <param name="cost_modifier">ad options cost_modifier modifier</param>
        /// <returns></returns>
        private double get_prominence_rating(double persuasion, double awareness, double recency, double cost_modifier)
        {
            double score = 0;
            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                int index = goal_location[current_plan.Specs.CampaignGoals[i]];
                score += prominence_weighting_matrix[index, 0] * current_plan.Specs.GoalWeights[i] * persuasion;
                score += prominence_weighting_matrix[index, 1] * current_plan.Specs.GoalWeights[i] * awareness;
                score += prominence_weighting_matrix[index, 2] * current_plan.Specs.GoalWeights[i] * recency;
                score += prominence_weighting_matrix[index, 3] * current_plan.Specs.GoalWeights[i] * cost_modifier;

            }
            return score;
        }

        /// <summary>
        /// Returns a list of ratings corresponding to the lists of inputs, performs scaling on the inputs to guarantee the inputs are between 0 and 1
        /// </summary>
        /// <param name="persuasion">List of persuasion values for the ad options</param>
        /// <param name="awareness">List of persuasion values for the ad options</param>
        /// <param name="recency">List of persuasion values for the ad options</param>
        /// <param name="cost_modifier">List of persuasion values for the ad options</param>
        /// <returns></returns>
        private List<double> get_prominence_ratings(List<double> persuasion, List<double> awareness, List<double> recency, List<double> cost_modifier)
        {
            double max_pers = persuasion.Max();
            double min_pers = persuasion.Min();
            double max_awr = awareness.Max();
            double min_awr = awareness.Min();
            double max_rec = recency.Max();
            double min_rec = recency.Min();
            double max_cost = cost_modifier.Max();
            double min_cost = cost_modifier.Min();

            List<double> rval = new List<double>();

            for (int i = 0; i < persuasion.Count; i++)
            {

                double scaled_pers = (persuasion[i] - min_pers) / (max_pers - min_pers);
                if (Double.IsNaN(scaled_pers))
                {
                    scaled_pers = 0.5;
                }
                double scaled_awr = (awareness[i] - min_awr) / (max_awr - min_awr);
                if (Double.IsNaN(scaled_awr))
                {
                    scaled_awr = 0.5;
                }
                double scaled_rec = (recency[i] - min_rec) / (max_rec - min_rec);
                if (Double.IsNaN(scaled_rec))
                {
                    scaled_rec = 0.5;
                }
                double scaled_cost = (cost_modifier[i] - min_cost) / (max_cost - min_cost);
                if (Double.IsNaN(scaled_cost))
                {
                    scaled_cost = 0.5;
                }
                rval.Add(get_prominence_rating(scaled_pers, scaled_awr, scaled_rec, scaled_cost));
            }

            return rval;
        }

        /// <summary>
        /// Returns the subtype rating based on the goal weights
        /// </summary>
        /// <param name="reach">reach of the subtype into the target demographics</param>
        /// <param name="efficency">efficeny of the subtype into the target demographics</param>
        /// <param name="in_category">bool determining whether the subtype is a match for the buisiness category</param>
        /// <param name="size">size of the subtype</param>
        /// <returns></returns>
        private double get_subtype_rating(double reach, double efficency, bool in_category, double size)
        {
            double category = 0;
            if (in_category)
            {
                category = 1;
            }
            double score = 0;
            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                int index = goal_location[current_plan.Specs.CampaignGoals[i]];
                score += subtype_weighting_matrix[index, 0] * current_plan.Specs.GoalWeights[i] * reach;
                score += subtype_weighting_matrix[index, 1] * current_plan.Specs.GoalWeights[i] * efficency;
                score += subtype_weighting_matrix[index, 2] * current_plan.Specs.GoalWeights[i] * category;
                score += subtype_weighting_matrix[index, 3] * current_plan.Specs.GoalWeights[i] * size;

            }

            return score;
        }

        /// <summary>
        /// Returns a list of ratings corresponding to the lists of inputs, performs scaling on the inputs to guarantee the inputs are between 0 and 1
        /// </summary>
        /// <param name="reach">List of reach values for the subtypes</param>
        /// <param name="efficency">List of reach values for the subtypes</param>
        /// <param name="in_category">List of reach values for the subtypes</param>
        /// <param name="size">List of reach values for the subtypes</param>
        /// <returns></returns>
        private List<double> get_subtype_ratings(List<double> reach, List<double> efficency, List<bool> in_category, List<double> size)
        {
            double max_reach = reach.Max();
            double min_reach = reach.Min();
            double max_eff = efficency.Max();
            double min_eff = efficency.Min();
            double max_size = size.Max();
            double min_size = size.Min();

            List<double> rval = new List<double>();

            for (int i = 0; i < reach.Count; i++)
            {
                double scaled_reach = (reach[i] - min_reach) / (max_reach - min_reach);
                if (Double.IsNaN(scaled_reach))
                {
                    scaled_reach = 0.5;
                }
                double scaled_eff = (efficency[i] - min_eff) / (max_eff - min_eff);
                if (Double.IsNaN(scaled_eff))
                {
                    scaled_eff = 0.5;
                }
                double scaled_size = (size[i] - min_size) / (max_size - min_size);
                if (Double.IsNaN(scaled_size))
                {
                    scaled_size = 0.5;
                }
                rval.Add(get_subtype_rating(scaled_reach, scaled_eff, in_category[i], scaled_size));
            }

            return rval;
        }

        /// <summary>
        /// Computes the rating for each vehicle based on the vehicle and subtype rating using the vehicle_subtype_weighting_matrix
        /// </summary>
        /// <param name="vehicles">Weighted vehicles</param>
        /// <param name="subtypes">Weighted subtypes</param>
        /// <returns>Reweighted vehicles</returns>
        private SelectDictionary<MediaVehicle> get_vehicle_subtype_ratings(SelectDictionary<MediaVehicle> vehicles, SelectDictionary<string> subtypes)
        {
            SelectDictionary<MediaVehicle> rval = new SelectDictionary<MediaVehicle>();

            if( vehicles.Count > 0 && subtypes.Count > 0 )
            {
                double max_vehicle_rating = vehicles.Values.Max();
                double min_vehicle_rating = vehicles.Values.Min();
                double max_subtype_rating = subtypes.Values.Max();
                double min_subtype_rating = subtypes.Values.Min();

                foreach( MediaVehicle vehicle in vehicles.Keys )
                {
                    //Get the vehicle rating and scale it between 0 and 1
                    double vehicle_rating = (vehicles[vehicle] - min_vehicle_rating) / (max_vehicle_rating - min_vehicle_rating);
                    if( Double.IsNaN( vehicle_rating ) )
                    {
                        vehicle_rating = 0.5;
                    }

                    //If the subtype does not exist use 0.0
                    double subtype_rating = 0.0;
                    string subtype = vehicle.SubType;
                    if( subtypes.ContainsKey( subtype ) )
                    {
                        //Subtype exists so scale it to be between 0 and 1
                        subtype_rating = (subtypes[subtype] - min_subtype_rating) / (max_subtype_rating - min_subtype_rating);
                        if( Double.IsNaN( subtype_rating ) )
                        {
                            subtype_rating = 0.5;
                        }
                    }

                    //Combine the vehicle and subtype ratings according to the matrix
                    rval.Add( vehicle, get_vehicle_subtype_rating( vehicle_rating, subtype_rating ) );
                }

                Normalize<MediaVehicle>( rval );
            }

            return rval;
        }

        private double get_vehicle_subtype_rating(double vehicle_rating, double subtype_rating)
        {
            double score = 0;
            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                int index = goal_location[current_plan.Specs.CampaignGoals[i]];
                score += vehicle_subtype_weighting_matrix[index, 0] * current_plan.Specs.GoalWeights[i] * vehicle_rating;
                score += vehicle_subtype_weighting_matrix[index, 1] * current_plan.Specs.GoalWeights[i] * subtype_rating;
            }
            return score;
        }

        private List<double> compute_matrix(double[,] matrix)
        {
            List<double> scores = new List<double>();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                scores.Add(0);
            }

            for (int i = 0; i < current_plan.Specs.CampaignGoals.Count; i++)
            {
                int index = goal_location[current_plan.Specs.CampaignGoals[i]];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    scores[j] += matrix[index, j] * current_plan.Specs.GoalWeights[i];
                }
            }

            double total = 0.0;
            for (int j = 0; j < scores.Count; j++)
            {
                total += scores[j];
            }

            for (int j = 0; j < scores.Count; j++)
            {
                scores[j] /= total;
            }



            return scores;
        }

        private int max_selection(List<double> list)
        {
            //This is more complicated then it seems because there can be more than one max
            double max_val = double.MinValue;
            List<int> max_indexes = new List<int>();
            List<double> mac_val = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > max_val)
                {
                    max_indexes.Clear();
                    max_indexes.Add(i);
                    max_val = list[i];
                }
                else if (list[i] == max_val)
                {
                    max_indexes.Add(i);
                }
                mac_val.Add(0.0);
            }
            for (int i = 0; i < max_indexes.Count; i++)
            {
                mac_val[max_indexes[i]] = 1;
            }

            return selection(mac_val);
        }

        private int selection(List<double> list)
        {
            double total = 0.0;
            List<double> scores = new List<double>();
            for (int j = 0; j < list.Count; j++)
            {
                if (!Double.IsNaN(list[j]))
                {
                    total += list[j];
                    scores.Add(list[j]);
                }
                else
                {
                    return rand.Next(list.Count);
                }
            }

            scores[0] = scores[0] / total;

            for (int j = 1; j < scores.Count; j++)
            {
                scores[j] = scores[j - 1] + (scores[j] / total);
            }

            double choice = rand.NextDouble();

            for (int j = 0; j < scores.Count; j++)
            {
                if (choice < scores[j])
                {
                    return j;
                }
            }

            return scores.Count;
        }

    }
}