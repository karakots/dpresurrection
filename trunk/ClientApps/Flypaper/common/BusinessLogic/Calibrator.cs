using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using WebLibrary;
using MediaLibrary;
using SimInterface;

namespace BusinessLogic
{
    public class Calibrator
    {
        // this represents the highest value for mu
        // if no perusasion then there will be roughly 0.5% share among aware consumers
        const double mu_max = 200.0;
        public static void Load_Stats(Stream persuasion_str, Stream awareness_str)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            persuasion_dict = (Dictionary<Scoring.Locus, List<InitialPersuasion>>)serializer.Deserialize(persuasion_str);
            awareness_dict = (Dictionary<Scoring.Locus, double>)serializer.Deserialize(awareness_str);
            LoadedStats = true;
        }
        public static bool LoadedStats = false;
        private static Dictionary<Scoring.Locus, List<InitialPersuasion>> persuasion_dict;
        private static Dictionary<Scoring.Locus, double> awareness_dict;

        public static void CalibrateInput(MediaPlan plan, SimInput input)
        {
            double purchase_cycle = Utils.PurchaseCycleLengthForCode( plan.PurchaseCycleCode );
            double sales_per_person = 365 / purchase_cycle;
            double total_sales = sales_per_person * plan.PopulationSize;
            double plan_sales = plan.Specs.InitialShare * 12;
            double initial_share = plan_sales / total_sales;

            if( plan.Specs.PreviousMediaSpend > 0 )
            {
                //Find the nearest persuasion and awareness loci
                //Start with persuasion
                List<Scoring.Locus> persuasion_loci = new List<Scoring.Locus>( persuasion_dict.Keys );
                SortedDictionary<double, Scoring.Locus> distances = new SortedDictionary<double, Scoring.Locus>();
                foreach( Scoring.Locus centroid in persuasion_loci )
                {
                    AllocationService.Insert<Scoring.Locus>( centroid.GetDistance( plan ), centroid, distances );
                }
                Scoring.Locus persuasion_locus = distances.First().Value;
                //Now do awareness
                List<Scoring.Locus> awareness_loci = new List<Scoring.Locus>( awareness_dict.Keys );
                distances = new SortedDictionary<double, Scoring.Locus>();
                foreach( Scoring.Locus centroid in awareness_loci )
                {
                    AllocationService.Insert<Scoring.Locus>( centroid.GetDistance( plan ), centroid, distances );
                }
                Scoring.Locus awareness_locus = distances.First().Value;


                //First the easy part...set the initial awarness and persuasion
                input.initial_awareness = awareness_dict[awareness_locus];
                input.initial_persuasion = persuasion_dict[persuasion_locus];

                if( initial_share > 0 )
                {
                    input.mu = newtons_method( 1.0, initial_share, input.initial_awareness, input.initial_persuasion, 0.001 );
                }
                else
                {
                    // save ourselves the trouble
                    input.mu = mu_max;
                }

            }
            else
            {
                // persuasion is zero
                input.initial_persuasion = new List<InitialPersuasion>();
                InitialPersuasion pers = new InitialPersuasion();
                pers.percent_aware = 1;
                pers.persuasion = 0;
                input.initial_persuasion.Add( pers );
                input.mu = mu_max;
                input.initial_awareness = 0.0;

                if( initial_share > 0  )
                {
                    // this becomes tricky as we need to
                    // estimate both share and catagory constant
                    // with no media spend

                    if( initial_share > 1 / (1 + mu_max) )
                    {
                        // this is a relatively high share product
                        // considering there is no media spend
                        // we will assume 100% awareness and adjust mu
                        input.mu = (1.0 / initial_share) - 1.0;
                        input.initial_awareness = 1;
                    }
                    else
                    {
                        // calc: awareness/(1 + mu)  = share
                        input.initial_awareness = (1 + input.mu) * initial_share;
                    }
                }
            }

        }

        private static double newtons_method(double current_mu, double share, double awareness, List<InitialPersuasion> persuasion, double tolerance)
        {
            for (int i = 0; i < 1000; i++)
            {
                double f = share_function(current_mu, share, awareness, persuasion);
                double f_prime = share_function_dev(current_mu, share, awareness, persuasion);
                double new_mu = current_mu - f / f_prime;
                if (Math.Abs(current_mu - new_mu) < tolerance)
                {
                    return new_mu;
                }
                current_mu = new_mu;


                if( current_mu >= mu_max )
                {
                    break;
                }

            }

            if( Double.IsNaN( current_mu ) || Double.IsInfinity( current_mu ) || current_mu > mu_max )
            {
                current_mu = mu_max;
            }
            return current_mu;
        }

        private static double share_function(double current_mu, double share, double awareness, List<InitialPersuasion> persuasion)
        {
            double sum = 0.0;
            for (int i = 0; i < persuasion.Count; i++)
            {
                double ep = Math.Exp(persuasion[i].persuasion);
                sum += persuasion[i].percent_aware * ep / (current_mu + ep);
            }

            double rval = awareness * sum - share;
            return Math.Pow(rval, 2);
        }

        private static double share_function_dev(double current_mu, double share, double awareness, List<InitialPersuasion> persuasion)
        {
            double sum_one = 0.0;
            double sum_two = 0.0;
            for (int i = 0; i < persuasion.Count; i++)
            {
                double ep = Math.Exp(persuasion[i].persuasion);
                sum_one += persuasion[i].percent_aware * ep / Math.Pow(current_mu + ep,2);
                sum_two += persuasion[i].percent_aware * ep / (current_mu + ep);
            }

            double rval = -2 * awareness * sum_one * (awareness * sum_two - share);
            return rval;
        }

        
    }
}
