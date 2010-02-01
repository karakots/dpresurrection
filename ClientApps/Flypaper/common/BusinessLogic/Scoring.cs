using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

using WebLibrary;
using MediaLibrary;
using GeoLibrary;
using DemographicLibrary;

namespace BusinessLogic
{
    public class Scoring
    {
        public static void Load_Stats(Stream str)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            mapping_dict = (Dictionary<string, Dictionary<Locus, Interpolater>>)serializer.Deserialize(str);
            LoadedStats = true;
        }
        public static bool LoadedStats = false;
        private static Dictionary<string, Dictionary<Locus, Interpolater>> mapping_dict;

        public static void Load_Weights(StreamReader str)
        {
            read_matrices_from_file(str);
            LoadedWeights = true;
        }
        public static bool LoadedWeights = false;
        private static double[,] goal_weights = new double[6, 13] { { 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 }, { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

        public const double MinCongratulationsStars = 4;

        private const double average_cpm = 11;

        public MediaPlan plan;

        private double budget_scalar;

        private Dictionary<MediaCampaignSpecs.CampaignGoal, int> goal_location;
        private Dictionary<string, int> metric_locations;
        

        private Dictionary<MediaCampaignSpecs.CampaignGoal, double> spec_goal_weights;

        private Random rand = new Random();

        

        /// <summary>
        /// Creates a scoring object for the given media plan.
        /// </summary>
        /// <param name="plan"></param>
        public Scoring(MediaPlan plan)
        {
            this.plan = plan;

            goal_location = new Dictionary<MediaCampaignSpecs.CampaignGoal, int>();
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.ReachAndAwareness, 0);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.Persuasion, 1);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.Recency, 2);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.DemographicTargeting, 3);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.GeoTargeting, 4);
            goal_location.Add(MediaCampaignSpecs.CampaignGoal.InterestTargeting, 5);

            metric_locations = new Dictionary<string, int>();
            metric_locations.Add("Reach1", 0);
            metric_locations.Add("Reach3", 1);
            metric_locations.Add("Reach4", 2);
            metric_locations.Add("PersuasionIndex", 3);
            metric_locations.Add("Awareness", 4);
            metric_locations.Add("MarketIndex1", 5);
            metric_locations.Add("MarketIndex2", 6);
            metric_locations.Add("Recency", 7);
            metric_locations.Add("Persuasion", 8);
            metric_locations.Add("Reach3All", 9);
            metric_locations.Add("Consideration1", 10);
            metric_locations.Add("Consideration3", 11);
            metric_locations.Add("Consideration4", 12);


            //goal_weights = new double[6, 13];
            //read_matrices_from_db();


            spec_goal_weights = new Dictionary<MediaCampaignSpecs.CampaignGoal, double>();

            for (int i = 0; i < plan.Specs.CampaignGoals.Count; i++)
            {
                if( i < plan.Specs.GoalWeights.Count )
                {
                    // stop gap - we need to consolidate campaign level data
                    // to the campaign obj (where is that?)
                    // so we can stop assuming that indexes fom one list match the indexes into another list
                    // SSN 10/3/2008
                    spec_goal_weights.Add( plan.Specs.CampaignGoals[i], plan.Specs.GoalWeights[i] );
                }
            }

            budget_scalar = 0;
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
            write_matrix(goal_weights, ref genome);
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
            read_matrix(values, ref current_index, goal_weights);
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


        #region public methods

        public Dictionary<MediaCampaignSpecs.CampaignGoal, double> GoalScores()
        {
            Dictionary<MediaCampaignSpecs.CampaignGoal, double> rval = new Dictionary<MediaCampaignSpecs.CampaignGoal, double>();

            if (budget_scalar == 0)
            {
                budget_scalars();
            }

            foreach (MediaCampaignSpecs.CampaignGoal goal in goal_location.Keys)
            {
                rval.Add(goal, ComputeScore(goal));
            }

            return rval;
        }

        public Dictionary<MediaCampaignSpecs.CampaignGoal, double> GoalScores(int seg_index)
        {
            Dictionary<MediaCampaignSpecs.CampaignGoal, double> rval = new Dictionary<MediaCampaignSpecs.CampaignGoal, double>();

            if (budget_scalar == 0)
            {
                budget_scalars();
            }

            foreach (MediaCampaignSpecs.CampaignGoal goal in goal_location.Keys)
            {
                rval.Add(goal, ComputeScore(seg_index, goal));
            }

            return rval;
        }

        public Dictionary<string, double> GetMetrics()
        {
            Dictionary<string, double> rval = new Dictionary<string, double>();

            foreach (string metric in metric_locations.Keys)
            {
                rval.Add(metric, GetMetricValue(metric));
            }

            return rval;
        }

        public Dictionary<string, double> GetMetrics(int seg_index)
        {
            Dictionary<string, double> rval = new Dictionary<string, double>();

            foreach (string metric in metric_locations.Keys)
            {
                rval.Add(metric, GetMetricValue(seg_index, metric));
            }

            return rval;
        }
       
        #endregion

        #region Star-Rating Methods
        public double GetRatingStars() {
            return GetRatingStars( -1 );
        }

        public double GetRatingStars( int segIndex ) {
            Dictionary<MediaCampaignSpecs.CampaignGoal, double> scores = null;
            if( segIndex == -1 ) {
                scores = GoalScores();
            }
            else {
                scores = GoalScores( segIndex );
            }

            double overallScore = 0.0;
            int numScores = 0;
            foreach( MediaCampaignSpecs.CampaignGoal goal in scores.Keys ) {
                if (spec_goal_weights.ContainsKey(goal))
                {
                    numScores++;
                    overallScore += scores[goal] * spec_goal_weights[goal] / 100;
                }
            }

            //Until things are calibrated
            double num_stars = 0.0;
            if (overallScore > (1.0 - 1.0 / 6.0))
            {
                num_stars = 5;
            }
            else if (overallScore > (1.0 - 2.0 / 6.0))
            {
                num_stars = 4;
            }
            else if (overallScore > (1.0 - 3.0 / 6.0))
            {
                num_stars = 3;
            }
            else if (overallScore > (1.0 - 4.0 / 6.0))
            {
                num_stars = 2;
            }
            else if (overallScore > (1.0 - 5.0 / 6.0))
            {
                num_stars = 1;
            }

            return num_stars;
        }

        #endregion

        #region private methods

        #region budgets

        private void budget_scalars()
        {
            if (plan.PopulationSize == 0)
            {
                get_pop_size();
            }

            double purchase_cycle = Utils.PurchaseCycleLengthForCode(plan.PurchaseCycleCode);

            double plan_duration = plan.PlanDurationDays;

            double budget = plan.TargetBudget;

            double purchase_cycles_per_plan = plan_duration / purchase_cycle;

            double dollars_per_cycle = budget / purchase_cycles_per_plan;

            double dollar_per_person = dollars_per_cycle / (plan.PopulationSize * 0.8);

            double impression_per_person = dollar_per_person / (average_cpm / 1000);

            budget_scalar = impression_per_person / 3;
        }

        #endregion

        #region goal scoring

        private double ComputeScore(MediaCampaignSpecs.CampaignGoal goal)
        {
            double score = 0.0;
            double total = 0.0;
            foreach (string metric in metric_locations.Keys)
            {
                score += interpolate_value(metric, GetMetricValue(metric)) * goal_weights[goal_location[goal], metric_locations[metric]];
                total += goal_weights[goal_location[goal], metric_locations[metric]];
            }
            score /= total;
            score = Math.Max(Math.Min(score, 1.0), 0.0);

            return score;
        }

        private double ComputeScore(int seg_index, MediaCampaignSpecs.CampaignGoal goal)
        {
            double score = 0.0;
            double total = 0.0;
            foreach (string metric in metric_locations.Keys)
            {
                score += interpolate_value(metric, GetMetricValue(seg_index, metric)) * goal_weights[goal_location[goal], metric_locations[metric]];
                total += goal_weights[goal_location[goal], metric_locations[metric]];
            }
            score /= total;
            score = Math.Max(Math.Min(score, 1.0),0.0);

            return score;
        }

        #endregion

        #region metric values

        private double GetMetricValue(string metric)
        {
            return AverageMetricValue(metric);
        }

        private double GetMetricValue(int seg_index, string metric)
        {
            return AverageMetricValue(seg_index, metric);
        }

        private double interpolate_value(string metric, double value)
        {
            double budget = plan.Specs.TargetBudget;
            double length = (plan.Specs.EndDate - plan.Specs.StartDate).Days;
            double people = plan.PopulationSize;

            double people_dollars = (budget / (length * people));
            List<Locus> Loci = new List<Locus>(mapping_dict[metric].Keys);
            SortedDictionary<double, Locus> distances = new SortedDictionary<double, Locus>();
            foreach (Locus centroid in Loci)
            {
                AllocationService.Insert<Locus>(centroid.GetDistance(plan), centroid, distances);
            }

            //First attempt, just take nearest locus
            Locus nearest = distances.First().Value;
            Interpolater interpolater = mapping_dict[metric][nearest];
            return interpolater.interpolate(value);

            //double total = 0.0;
            //double weight = 0.0;
            //foreach (double distance in distances.Keys)
            //{
            //    Locus loci = distances[distance];
            //    Interpolater interpolater = mapping_dict[metric][loci];
            //    double score = interpolater.interpolate(value);
            //    total += score / Math.Pow(distance, 2);
            //    weight += 1 / Math.Pow(distance, 2);
            //}
            //return total / weight;

            //double value1 = mapping_dict[metric][values[min_index]].interpolate(value);
            //double value2 = mapping_dict[metric][values[max_index]].interpolate(value);
            //double weight1 = (double)(people_dollars - values[min_index]);
            //double weight2 = (double)(values[max_index] - people_dollars);

            //return weighted_average(value1, weight1, value2, weight2);

        }
        /// <summary>
        /// Computes the average of the specified metric over all segments in the plan.
        /// </summary>
        /// <param name="metric"></param>
        /// <returns></returns>
        private double AverageMetricValue(string metric)
        {
            double avg = 0;
            if (this.plan.SegmentCount > 0)
            {
                for (int s = 0; s < this.plan.SegmentCount; s++)
                {
                    avg += AverageMetricValue(s, metric);
                }
                avg /= (double)this.plan.SegmentCount;
            }
            return avg;
        }

        /// <summary>
        /// Computes the average of the specified metric in the specified segment.
        /// </summary>
        /// <param name="segmentIndex"></param>
        /// <param name="metricType"></param>
        /// <returns></returns>
        private double AverageMetricValue(int segmentIndex, string metricName)
        {
            if (this.plan.Results == null || this.plan.Results.metrics == null)
            {
                return -1;
            }

            string segment_name = plan.Specs.SegmentList[segmentIndex].Name;

            List<SimInterface.Metric> metrics = plan.Results.metrics;
            int metricIndx = 0;
            for (metricIndx = 0; metricIndx < metrics.Count; metricIndx++)
            {
                if (metricName == metrics[metricIndx].Type && segment_name == metrics[metricIndx].Segment)
                {
                    break;
                }
            }

            double tot = 0;
            double avg = 0;
            

            if (metricIndx >= 0 && metricIndx < metrics.Count)
            {
                SimInterface.Metric metric = metrics[metricIndx];
                foreach (double d in metric.values)
                {
                    tot += d;
                }
                avg = tot / (double)metric.values.Count;
            }

            return avg;
        }
        #endregion

        #region database methods
        /// <summary>
        /// gets the population size
        /// </summary>
        private void get_pop_size()
        {
            plan.PopulationSize = SimUtils.GetPopulationSize(plan);
        }

        #endregion

        #endregion

        #region Helper Classes

        /// <summary>
        /// Rating values for media plans/segments
        /// </summary>
        public enum Rating
        {
            Poor,
            Fair,
            Good,
            Excellent,
            Unknown
        }

        private double rating_to_score(Rating rating)
        {
            switch (rating)
            {
                case Rating.Excellent:
                    return 10;
                case Rating.Good:
                    return 8;
                case Rating.Fair:
                    return 4;
                case Rating.Poor:
                case Rating.Unknown:
                    return 0;
            }

            return 0;
        }

        private Rating score_to_rating(double score)
        {
            if (score >= 9)
            {
                return Rating.Excellent;
            }
            else if (score >= 6)
            {
                return Rating.Good;
            }
            else if(score >= 2)
            {
                return Rating.Fair;
            }

            return Rating.Poor;
        }

        private Rating avg_rating(Rating rating1, Rating rating2)
        {
            double avg = (rating_to_score(rating1) + rating_to_score(rating2))/2;
            return score_to_rating(avg);
        }

        [Serializable]
        public class Interpolater
        {
            private double min;
            private double max;
            private double mean;

            private static double max_value = 1.0;
            private static double min_value = 0.17;
            private static double mean_value = 0.667;

            public Interpolater(double mean, double min, double max)
            {
                this.mean = mean;
                this.max = max;
                this.min = min;
            }

            const double eps = 0.0000001;

            public double interpolate(double value)
            {
                if (value < mean)
                {
                    if( mean < min + eps )
                    {
                        return mean;
                    }

                    return min_value + (value - min) * (mean_value - min_value) / (mean - min);
                }

                if( max < mean + eps )
                {
                    return mean;
                }

                return mean_value + (value - mean) * (max_value - mean_value) / (max - mean);
                
            }
        }

        private double weighted_average(double value1, double weight1, double value2, double weight2)
        {
            double total = weight1 + weight2;
            return value1 * (weight1 / total) + value2 * (weight2 / total);
        }

        public interface Locus
        {
            double GetDistance(MediaPlan plan);

            double GetDistance(double[] parms );

            void Recompute(IEnumerable<MediaPlan> plans);

            void Initialize(MediaPlan plan);

            bool IsNaN();

            bool SameCenter(Scoring.Locus locus);

            double GetCenter();
        }

        [Serializable]
        public class DoubleCentroid : Scoring.Locus
        {
            public double dollars_per_day;
            public double dollars_per_interval;
            public double interval;

            public DoubleCentroid(MediaPlan plan)
            {
                Initialize(plan);
            }

            public DoubleCentroid(List<MediaPlan> plans)
            {
                Recompute(plans);
            }

            #region Locus Members

            public double GetDistance(MediaPlan plan)
            {
                return Math.Pow(dollars_per_day - day_dollars(plan), 2) + Math.Pow(interval - interval_length(plan), 2);
            }

            public double GetDistance( double[] parms)
            {
                return Math.Pow( dollars_per_day - parms[0], 2 ) + Math.Pow( interval - parms[1], 2 );
            }

            public void Recompute(IEnumerable<MediaPlan> plans)
            {
                dollars_per_day = 0.0;
                dollars_per_interval = 0.0;
                double num_plans = plans.Count();
                foreach (MediaPlan plan in plans)
                {
                    dollars_per_day += day_dollars(plan);
                    dollars_per_interval += consideration_dollars(plan);
                    interval += interval_length(plan);
                }

                dollars_per_day /= num_plans;
                dollars_per_interval /= num_plans;
                interval /= num_plans;
            }


            public void Initialize(MediaPlan plan)
            {
                dollars_per_day = day_dollars(plan);
                dollars_per_interval = consideration_dollars(plan);
                interval = interval_length(plan);
            }

            public bool IsNaN()
            {
                double value = dollars_per_day + dollars_per_interval + interval;
                if (Double.IsNaN(value))
                {
                    return true;
                }

                return false;
            }

            public double GetCenter()
            {
                return dollars_per_day + interval;
            }

            public bool SameCenter(Scoring.Locus locus)
            {
                if (GetCenter() == locus.GetCenter())
                {
                    return true;
                }

                return false;
            }

            #endregion

            private double day_dollars(MediaPlan plan)
            {
                double budget = plan.Specs.TargetBudget;
                double length = (plan.Specs.EndDate - plan.Specs.StartDate).Days;
                double people = plan.PopulationSize;

                return budget / (length * people);
            }

            private double consideration_dollars(MediaPlan plan)
            {
                double budget = plan.Specs.TargetBudget;
                double length = (plan.Specs.EndDate - plan.Specs.StartDate).Days;
                double people = plan.PopulationSize;
                double consideration_length = Utils.PurchaseCycleLengthForCode(plan.PurchaseCycleCode);

                return budget / ((length / consideration_length) * people);
            }

            private double interval_length(MediaPlan plan)
            {
                return Utils.PurchaseCycleLengthForCode(plan.PurchaseCycleCode);
            }
        }

        [Serializable]
        public class SingleCentroid : Scoring.Locus
        {
            public double dollars_per_day;

            public SingleCentroid(MediaPlan plan)
            {
                Initialize(plan);
            }

            public SingleCentroid(List<MediaPlan> plans)
            {
                Recompute(plans);
            }

            #region Locus Members

            public double GetDistance(MediaPlan plan)
            {
                double dist = dollars_per_day - day_dollars(plan);

                if (dist > 0)
                {
                    // plans above are much further away
                    dist *= 10;
                }

                return Math.Pow(dist, 2);
            }

            public double GetDistance( double[] parms )
            {
                return Math.Pow( dollars_per_day - parms[0], 2 );
            }

            public void Recompute(IEnumerable<MediaPlan> plans)
            {
                dollars_per_day = 0.0;
                double num_plans = plans.Count();
                foreach (MediaPlan plan in plans)
                {
                    dollars_per_day += day_dollars(plan);
                }

                dollars_per_day /= num_plans;
            }


            public void Initialize(MediaPlan plan)
            {
                dollars_per_day = day_dollars(plan);
            }

            public bool IsNaN()
            {
                if (Double.IsNaN(dollars_per_day))
                {
                    return true;
                }

                return false;
            }

            public double GetCenter()
            {
                return dollars_per_day;
            }

            public bool SameCenter(Scoring.Locus locus)
            {
                if (GetCenter() == locus.GetCenter())
                {
                    return true;
                }

                return false;
            }

            #endregion

            public bool SameCenter(SingleCentroid centroid)
            {
                if (dollars_per_day == centroid.dollars_per_day)
                {
                    return true;
                }

                return false;
            }

            private double day_dollars(MediaPlan plan)
            {
                double budget = plan.Specs.PreviousMediaSpend;
                double length = 365;
                double people = plan.PopulationSize;

                return budget / (length * people);
            }
        }
        
        #endregion
    }
}
