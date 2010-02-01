using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

using MediaLibrary;
using WebLibrary;
using BusinessLogic;
using DpSimQueue;
using DemographicLibrary;
using SimInterface;
using FlySim;

namespace Calibrator
{
    public partial class Calibrator : Form
    {
        private Scoring scoring_service;
        private DpMediaDb media_database;
        private DpUpdate sim_database;

        private Dictionary<Guid, MediaPlan> finished_plans;
        private Dictionary<Guid, MediaPlan> queued_plans;

        private SimCreator creator;

        private DirectoryInfo output_directory;

        private int num_sims;
        private int num_finished;
        private int num_created;

        private int sim_per_tick;

        private int plans_per_file;
        private int file_num;

        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer;

        public Calibrator()
        {
            InitializeComponent();

            output_directory = new DirectoryInfo(Properties.Settings.Default.OutputDirectory);

            while(!output_directory.Exists)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                while (dlg.ShowDialog() != DialogResult.OK)
                {
                }

                output_directory = new DirectoryInfo(dlg.SelectedPath);
            }

            Initialize(output_directory.FullName);


            scoring_service = new Scoring(new MediaPlan());

            media_database = Utils.MediaDatabase;
            sim_database = new DpUpdate(Properties.Settings.Default.SimConnection);

            finished_plans = new Dictionary<Guid, MediaPlan>();
            queued_plans = new Dictionary<Guid, MediaPlan>();

            num_sims = 2000;
            plans_per_file = 50;
            sim_per_tick = 1;

            serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        }

        private static void Initialize(string mediaInfoDirectory)
        {
            Utils.ReadStateToDMA(mediaInfoDirectory + @"\Data");

            if (!Scoring.LoadedStats)
            {
                string stats_file = mediaInfoDirectory + @"\stats.dat";
                if (File.Exists(stats_file) == false)
                {
                    string msg = String.Format("Error: Scoring statistics file not found: {0}", stats_file);
                    throw new Exception(msg);
                }
                FileStream fs = new FileStream(stats_file, FileMode.Open, FileAccess.Read);
                Scoring.Load_Stats(fs);
                fs.Close();
            }

            if (!Scoring.LoadedWeights)
            {
                string weights_file = mediaInfoDirectory + @"\scoring_values.txt";
                if (File.Exists(weights_file) == false)
                {
                    string msg = String.Format("Error: Scoring weights file not found: {0}", weights_file);
                    throw new Exception(msg);
                }
                FileStream fs = new FileStream(weights_file, FileMode.Open, FileAccess.Read);
                StreamReader str = new StreamReader(fs);
                Scoring.Load_Weights(str);
                str.Close();
                fs.Close();
            }

            if (!AllocationService.LoadedMatrices)
            {
                string matrix_file = mediaInfoDirectory + @"\plan_gen_values.txt";
                if (File.Exists(matrix_file) == false)
                {
                    string msg = String.Format("Error: Scoring weights file not found: {0}", matrix_file);
                    throw new Exception(msg);
                }
                FileStream fs = new FileStream(matrix_file, FileMode.Open, FileAccess.Read);
                StreamReader str = new StreamReader(fs);
                AllocationService.Load_Matrices(str);
                str.Close();
                fs.Close();
            }

            if (!BusinessLogic.Calibrator.LoadedStats)
            {
                string awareness_file = mediaInfoDirectory + @"\awareness.dat";
                string persuasion_file = mediaInfoDirectory + @"\persuasion.dat";
                if (!File.Exists(awareness_file))
                {
                    string msg = String.Format("Error: Input calibration stats file not found: {0}", awareness_file);
                    throw new Exception(msg);
                }
                if (!File.Exists(persuasion_file))
                {
                    string msg = String.Format("Error: Input calibration stats file not found: {0}", persuasion_file);
                    throw new Exception(msg);
                }
                FileStream a_fs = new FileStream(awareness_file, FileMode.Open, FileAccess.Read);
                FileStream p_fs = new FileStream(persuasion_file, FileMode.Open, FileAccess.Read);
                BusinessLogic.Calibrator.Load_Stats(p_fs, a_fs);
                a_fs.Close();
                p_fs.Close();
            }

            Utils.RefreshMediaDatabase(Properties.Settings.Default.MediaConnectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AllocationService allocation_service = new AllocationService();
            Utils.MediaDatabase.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            num_finished = 0;
            num_created = 0;
            file_num = 0;
            string file_name = "plan_dict_" + file_num + ".dat";
            FileInfo info = new FileInfo(output_directory.FullName + "\\" + file_name);
            while (info.Exists)
            {
                file_num++;
                file_name = "plan_dict_" + file_num + ".dat";
                info = new FileInfo(output_directory.FullName + "\\" + file_name);
            }
            

            creator = new SimCreator(media_database, Properties.Settings.Default.SimConnection);


            spawner.Start();
            sim_run.Start();
            write_timer.Start();
        }


        private void sim_run_Tick(object sender, EventArgs e)
        {
            List<Guid> finished = new List<Guid>();

            foreach (Guid guid in queued_plans.Keys)
            {
                bool done = sim_database.SimDone(guid);

                if (done)
                {
                    finished_plans.Add(guid, queued_plans[guid]);
                    SimOutput output = sim_database.GetResults(guid);
                    finished_plans[guid].Results = output;
                    finished.Add(guid);
                    num_finished++;
                }
            }
            foreach (Guid guid in finished)
            {
                queued_plans.Remove(guid);
            }
        }

        private void write_timer_Tick(object sender, EventArgs e)
        {
            if (finished_plans.Count >= plans_per_file || num_finished >= num_sims)
            {
                string file_name = "plan_dict_" + file_num + ".dat";
                FileStream str = new FileStream(output_directory.FullName + "\\" + file_name, FileMode.OpenOrCreate);
                serializer.Serialize(str, finished_plans);
                str.Close();
                file_num++;
                finished_plans.Clear();
            }

            if (num_finished == num_sims)
            {
                sim_run.Stop();
                write_timer.Stop();

                MessageBox.Show("All Finished");
            }
        }

        private void spawner_Tick(object sender, EventArgs e)
        {
            int num_to_create = Math.Min(sim_per_tick, num_sims - num_created);

            if (num_to_create == 0)
            {
                spawner.Stop();
            }

            Dictionary<Guid, MediaPlan> new_plans = creator.QueuePlans(num_to_create);
            foreach (Guid guid in new_plans.Keys)
            {
                queued_plans.Add(guid, new_plans[guid]);
                num_created++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlanStats stats = new PlanStats(output_directory);
            stats.WriteStats("stats.csv");
            stats.WriteAll("all_plans.csv");
            stats.WriteStatsComputer("stats.dat");
            stats.WriteInitialStats("persuasion.dat", "awareness.dat");
            
            MessageBox.Show("All Finished");
        }

        private void write_to_file_Click(object sender, EventArgs e)
        {
            AllocationService temp = new AllocationService();
            StreamWriter wrt = new StreamWriter(output_directory.FullName + "\\plan_gen_values.txt");
            AllocationService.write_matrices_to_file(wrt);
            wrt.Close();

            wrt = new StreamWriter(output_directory.FullName + "\\scoring_values.txt");
            Scoring.write_matrices_to_file(wrt);
            wrt.Close();
        }

        private void use_results_button_Click(object sender, EventArgs e)
        {
            PlanStats stats = new PlanStats(output_directory);
            stats.WriteStars("stars.csv");
            MessageBox.Show("All Finished");
        }
    }

    class PlanStats
    {
        private DirectoryInfo output_directory;
        private List<MediaPlan> my_plans;

        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer;

        private int centroids;
        private int max_iters;

        public PlanStats(DirectoryInfo output_dir)
        {
            output_directory = output_dir;

            serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            my_plans = new List<MediaPlan>();

            foreach (FileInfo file in output_directory.GetFiles())
            {
                try
                {
                    if (file.Name == "stats.dat")
                    {
                        FileStream str = new FileStream(file.FullName, FileMode.Open);
                        Scoring.Load_Stats(str);
                    }
                    else if (file.Name == "scoring_values.txt")
                    {
                        StreamReader str = new StreamReader(file.FullName);
                        Scoring.Load_Weights(str);
                    }
                    else
                    {
                        FileStream str = file.OpenRead();
                        Dictionary<Guid, MediaPlan> plan_dict = (Dictionary<Guid, MediaPlan>)serializer.Deserialize(str);
                        str.Close();
                        my_plans.AddRange(plan_dict.Values);
                    }
                }
                catch
                {
                }
            }

            centroids = 40;
            max_iters = 1000;
        }

        public void WriteAll(string file)
        {
            StreamWriter output = new StreamWriter(output_directory + "\\" + file);
            string header = "budget, length, dollar_per_day";
            Dictionary<string, double> metrics = (new Scoring(my_plans[0])).GetMetrics();
            foreach (string metric in metrics.Keys)
            {
                header += ", " + metric;
            }
            output.WriteLine(header);
            foreach (MediaPlan plan in my_plans)
            {
                Scoring scoring = new Scoring(plan);
                metrics = scoring.GetMetrics();

                if (metrics["Reach1"] < 0.001)
                {
                    continue;
                }

                double budget = plan.Specs.TargetBudget;
                double length = (plan.Specs.EndDate - plan.Specs.StartDate).Days;
                string line = budget.ToString();
                line += ", " + length.ToString();
                line += ", " + (budget / length).ToString();

                foreach (string metric in metrics.Keys)
                {
                    line += ", " + metrics[metric].ToString();
                }
                output.WriteLine(line);
            }
            output.Close();
        }

        public void WriteStatsComputer(string file)
        {
            Dictionary<string, Dictionary<Scoring.Locus, Scoring.Interpolater>> mapping_dict = new Dictionary<string, Dictionary<Scoring.Locus, Scoring.Interpolater>>();
            Dictionary<string, double> metrics = (new Scoring(my_plans[0])).GetMetrics();
            foreach (string metric in metrics.Keys)
            {
                mapping_dict.Add(metric, new Dictionary<Scoring.Locus, Scoring.Interpolater>());
            }

            Dictionary<Scoring.Locus, List<Scoring>> plan_scoring = new Dictionary<Scoring.Locus, List<Scoring>>();
            Dictionary<Scoring.Locus, MediaPlan> plan_dict = new Dictionary<Scoring.Locus, MediaPlan>();
            foreach (MediaPlan plan in my_plans)
            {
                plan_dict.Add(new Scoring.DoubleCentroid(plan), plan);
            }
            Dictionary<Scoring.Locus, List<MediaPlan>> plans = lloyds_algorithm(plan_dict, CreateDoubleCentroid);

            foreach (Scoring.Locus centroid in plans.Keys)
            {
                plan_scoring.Add(centroid, new List<Scoring>());
                foreach (MediaPlan plan in plans[centroid])
                {
                    plan_scoring[centroid].Add(new Scoring(plan));
                }
                foreach (string metric in mapping_dict.Keys)
                {
                    mapping_dict[metric].Add(centroid, new Scoring.Interpolater(0.5, 0.0, 1.0));
                }
            }

            foreach (Scoring.Locus key in plan_scoring.Keys)
            {
                Dictionary<string, double> avg_metrics = new Dictionary<string, double>();
                Dictionary<string, double> min_metric = new Dictionary<string, double>();
                Dictionary<string, double> max_metric = new Dictionary<string, double>();
                foreach (Scoring scoring in plan_scoring[key])
                {
                    metrics = scoring.GetMetrics();
                    if (metrics["Reach1"] < 0.001)
                    {
                        continue;
                    }
                    int num_plans = plan_scoring[key].Count;
                    foreach (string metric in metrics.Keys)
                    {
                        if (!avg_metrics.ContainsKey(metric))
                        {
                            avg_metrics.Add(metric, metrics[metric] / num_plans);
                            min_metric.Add(metric, metrics[metric]);
                            max_metric.Add(metric, metrics[metric]);
                        }
                        else
                        {
                            avg_metrics[metric] += metrics[metric] / num_plans;
                            if (metrics[metric] < min_metric[metric])
                            {
                                min_metric[metric] = metrics[metric];
                            }
                            if (metrics[metric] > max_metric[metric])
                            {
                                max_metric[metric] = metrics[metric];
                            }
                        }
                    }
                }
                foreach (string metric in avg_metrics.Keys)
                {
                    mapping_dict[metric][key] = new Scoring.Interpolater(avg_metrics[metric], min_metric[metric], max_metric[metric]);
                }
            }
            FileStream str = new FileStream(output_directory + "\\" + file, FileMode.OpenOrCreate);
            serializer.Serialize(str, mapping_dict);
            str.Close();
        }

        public void WriteStars(string file)
        {
            List<Scoring> scorers = new List<Scoring>();
            int num_latino = 0;
            foreach (MediaPlan plan in my_plans)
            {
                scorers.Add(new Scoring(plan));
                foreach (Demographic demographic in plan.Specs.SegmentList)
                {
                    if (demographic.ToString().Contains("LATINO"))
                    {
                        num_latino++;
                        break;
                    }
                }
            }

            Dictionary<double, List<Scoring>> star_values = new Dictionary<double, List<Scoring>>();

            foreach (Scoring scorer in scorers)
            {
                double stars = scorer.GetRatingStars();
                if (!star_values.ContainsKey(stars))
                {
                    star_values.Add(stars, new List<Scoring>());
                }

                star_values[stars].Add(scorer);
            }

            StreamWriter output = new StreamWriter(output_directory + "\\" + file);
            string header = "stars, num_plans";
            output.WriteLine(header);
            foreach (double stars in star_values.Keys)
            {
                output.WriteLine(stars.ToString() + ", " + star_values[stars].Count.ToString());
            }
            output.WriteLine(output.NewLine);
            header = "budget, length, interval, pop_size, category, demo one, demo two, demo three, demo four, demo five, type one, type one spending, type two, type two spending, type three, type three spending, type four, type four spending, type five, type five spending, goal one, goal one weight, goal two, goal two weight, goal three, goal three weight, goal four, goal four weight, goal five, goal five weight";
            output.WriteLine(header);
            foreach (Scoring scorer in star_values[5])
            {
                MediaPlan plan = scorer.plan;
                double budget = plan.Specs.TargetBudget;
                double length = (plan.Specs.EndDate - plan.Specs.StartDate).Days;
                double people = plan.PopulationSize;
                double consideration_length = Utils.PurchaseCycleLengthForCode(plan.PurchaseCycleCode);

                

                string line = scorer.plan.TargetBudget.ToString();
                line += ", " + length.ToString();
                line += ", " + consideration_length.ToString();
                line += ", " + people.ToString();
                line += ", " + plan.Specs.BusinessSubcategory.Replace(',',' ');

                int j = 0;
                foreach (Demographic demographic in plan.Specs.SegmentList)
                {
                    line += ", " + demographic.ToString();
                    j++;
                }
                for (; j < 5; j++)
                {
                    line += ",";
                }

                j = 0;
                Dictionary<MediaVehicle.MediaType, double> types = plan.GetTypeFractions();
                foreach (MediaVehicle.MediaType type in types.Keys)
                {
                    line += ", " + type.ToString() + ", " + types[type].ToString();
                    j++;
                }

                for(;j<5; j++)
                {
                    line += ", ,";
                }

                for (int i = 0; i < plan.Specs.CampaignGoals.Count; i++)
                {
                    line += ", " + plan.Specs.CampaignGoals[i].ToString() + ", " + plan.Specs.GoalWeights[i];
                }
                output.WriteLine(line);
            }

            output.Close();
        }

        public void WriteStats(string file)
        {
            Dictionary<Scoring.Locus, List<Scoring>> plan_scoring = new Dictionary<Scoring.Locus, List<Scoring>>();

            Dictionary<Scoring.Locus, MediaPlan> plan_dict = new Dictionary<Scoring.Locus, MediaPlan>();
            foreach (MediaPlan plan in my_plans)
            {
                plan_dict.Add(new Scoring.DoubleCentroid(plan), plan);
            }
            Dictionary<Scoring.Locus, List<MediaPlan>> plans = lloyds_algorithm(plan_dict, CreateDoubleCentroid);

            Scoring.Locus sample_key = null;
            foreach (Scoring.Locus centroid in plans.Keys)
            {
                plan_scoring.Add(centroid, new List<Scoring>());
                foreach (MediaPlan plan in plans[centroid])
                {
                    plan_scoring[centroid].Add(new Scoring(plan));
                }

                sample_key = centroid;
            }

            StreamWriter output = new StreamWriter(output_directory + "\\" + file);
            string header = "person_dollars, interval, num_plans";
            Dictionary<string, double> metrics = plan_scoring[sample_key][0].GetMetrics();
            foreach (string metric in metrics.Keys)
            {
                header += ", mean " + metric + ", std dev " + metric + ", min " + metric + ", max " + metric;
            }
            output.WriteLine(header);

            foreach (Scoring.Locus key in plan_scoring.Keys)
            {
                Scoring.DoubleCentroid centroid = key as Scoring.DoubleCentroid;
                string line = centroid.dollars_per_day.ToString() + ", ";
                line += centroid.interval.ToString() + ", ";
                int num_plans = plan_scoring[key].Count;
                line += num_plans.ToString();
                Dictionary<string, double> avg_metrics = new Dictionary<string, double>();
                Dictionary<string, double> std_dev_metric = new Dictionary<string, double>();
                Dictionary<string, double> min_metric = new Dictionary<string, double>();
                Dictionary<string, double> max_metric = new Dictionary<string, double>();
                foreach (Scoring scoring in plan_scoring[key])
                {
                    metrics = scoring.GetMetrics();
                    if (metrics["Reach1"] < 0.001)
                    {
                        continue;
                    }
                    foreach (string metric in metrics.Keys)
                    {
                        if (!avg_metrics.ContainsKey(metric))
                        {
                            avg_metrics.Add(metric, metrics[metric] / num_plans);
                            min_metric.Add(metric, metrics[metric]);
                            max_metric.Add(metric, metrics[metric]);
                        }
                        else
                        {
                            avg_metrics[metric] += metrics[metric] / num_plans;
                            if (metrics[metric] < min_metric[metric])
                            {
                                min_metric[metric] = metrics[metric];
                            }
                            if (metrics[metric] > max_metric[metric])
                            {
                                max_metric[metric] = metrics[metric];
                            }
                        }
                    }
                }
                foreach (Scoring scoring in plan_scoring[key])
                {
                    metrics = scoring.GetMetrics();
                    if (metrics["Reach1"] < 0.001)
                    {
                        continue;
                    }
                    foreach (string metric in metrics.Keys)
                    {
                        if (!std_dev_metric.ContainsKey(metric))
                        {
                            std_dev_metric.Add(metric, Math.Pow(metrics[metric] - avg_metrics[metric], 2) / num_plans);
                        }
                        else
                        {
                            std_dev_metric[metric] += Math.Pow(metrics[metric] - avg_metrics[metric], 2) / num_plans;
                        }
                    }
                }
                foreach (string metric in avg_metrics.Keys)
                {
                    std_dev_metric[metric] = Math.Sqrt(std_dev_metric[metric]);
                }
                foreach (string metric in avg_metrics.Keys)
                {
                    line += ", " + avg_metrics[metric].ToString() + ", " + std_dev_metric[metric].ToString() + ", " + min_metric[metric].ToString() + ", " + max_metric[metric];
                }
                output.WriteLine(line);
            }

            output.Close();
        }

        public void WriteInitialStats(string persuasion_file, string awarness_file)
        {
            Dictionary<Scoring.Locus, MediaPlan> plan_dict = new Dictionary<Scoring.Locus, MediaPlan>();
            foreach (MediaPlan plan in my_plans)
            {
                plan.Specs.PreviousMediaSpend = plan.TargetBudget;
                plan_dict.Add(new Scoring.SingleCentroid(plan), plan);
            }
            Dictionary<Scoring.Locus, List<MediaPlan>> plans = lloyds_algorithm(plan_dict, CreateSingleCentroid);

            Dictionary<Scoring.Locus, List<InitialPersuasion>> persuasion_dictionary = new Dictionary<Scoring.Locus, List<InitialPersuasion>>();
            Dictionary<Scoring.Locus, double> awareness_dictionary = new Dictionary<Scoring.Locus, double>();

            foreach (Scoring.Locus locus in plans.Keys)
            {
                List<MediaPlan> plan_list = plans[locus];
                List<double> awareness = new List<double>();
                List<List<FinalPersuasion>> final_persuasion = new List<List<FinalPersuasion>>();

                //First gather the metrics
                foreach (MediaPlan plan in plan_list)
                {
                    awareness.Add((new Scoring(plan)).GetMetrics()["Awareness"]);
                    final_persuasion.Add(plan.Results.final_persuasion);
                }

                //Get the average awareness
                double avg_awareness = awareness.Average();
                awareness_dictionary.Add(locus, avg_awareness);

                //Perusuasion is trickier and correctly converted to initial persuasion
                //First calculate statistics
                double max_persuasion = double.MinValue;
                double total_agents = 0.0;
                for (int i = 0; i < final_persuasion.Count; i++)
                {
                    for (int j = 0; j < final_persuasion[i].Count; j++)
                    {
                        total_agents += final_persuasion[i][j].num_agents;
                        if (final_persuasion[i][j].persuasion > max_persuasion)
                        {
                            max_persuasion = final_persuasion[i][j].persuasion;
                        }
                    }
                }

                if (max_persuasion > 0.0)
                {
                    //Then divide into buckets
                    double num_buckets = 20;
                    List<InitialPersuasion> initial_persuasion = new List<InitialPersuasion>();
                    for (int i = 0; i < num_buckets; i++)
                    {
                        initial_persuasion.Add(new InitialPersuasion());
                        initial_persuasion[i].persuasion = ((double)i) * (max_persuasion / num_buckets);
                    }
                    //Finally merge final persuasion into initial persuasion buckets
                    for (int i = 0; i < final_persuasion.Count; i++)
                    {
                        for (int j = 0; j < final_persuasion[i].Count; j++)
                        {
                            int bucket = (int)(final_persuasion[i][j].persuasion * (num_buckets - 1) / max_persuasion);
                            initial_persuasion[bucket].percent_aware += (double)final_persuasion[i][j].num_agents / total_agents;
                        }
                    }
                    persuasion_dictionary.Add(locus, initial_persuasion);
                }
            }

            FileStream str = new FileStream(output_directory + "\\" + persuasion_file, FileMode.OpenOrCreate);
            serializer.Serialize(str, persuasion_dictionary);
            str.Close();

            str = new FileStream(output_directory + "\\" + awarness_file, FileMode.OpenOrCreate);
            serializer.Serialize(str, awareness_dictionary);
            str.Close();
        }

        private delegate Scoring.Locus GetLocus(List<MediaPlan> plans);

        private Scoring.Locus CreateDoubleCentroid(List<MediaPlan> plans)
        {
            return new Scoring.DoubleCentroid(plans);
        }

        private Scoring.Locus CreateSingleCentroid(List<MediaPlan> plans)
        {
            return new Scoring.SingleCentroid(plans);
        }

        private Dictionary<Scoring.Locus, List<MediaPlan>> lloyds_algorithm(Dictionary<Scoring.Locus, MediaPlan> plan_dict, GetLocus generator)
        {
            

            Dictionary<Scoring.Locus, List<MediaPlan>> prev_centroids = new Dictionary<Scoring.Locus, List<MediaPlan>>();
            Dictionary<Scoring.Locus, List<MediaPlan>> new_centroids = new Dictionary<Scoring.Locus, List<MediaPlan>>();

            Random rand = new Random();
            List<Scoring.Locus> keys = new List<Scoring.Locus>(plan_dict.Keys);
            int iters = 0;
            Bucket bucket = new Bucket(keys.Count);
            while (new_centroids.Count < centroids && !bucket.Empty)
            {
                Scoring.Locus key = keys[bucket.Draw];
                if (!new_centroids.ContainsKey(key))
                {
                    new_centroids.Add(key, new List<MediaPlan>());
                }
                iters++;
            }

            add_plans(plan_dict, new_centroids, generator);

            iters = 0;
            while (!are_equal(prev_centroids, new_centroids) && iters < max_iters)
            {
                prev_centroids = new_centroids;
                new_centroids = recompute(prev_centroids, generator);
                add_plans(plan_dict, new_centroids, generator);
                iters++;
            }

            List<Scoring.Locus> loci = new List<Scoring.Locus>(new_centroids.Keys);

            foreach (Scoring.Locus locus in loci)
            {
                if (locus.IsNaN())
                {
                    new_centroids.Remove(locus);
                }
            }

            return new_centroids;
        }

        private bool are_equal(Dictionary<Scoring.Locus, List<MediaPlan>> centroids_one, Dictionary<Scoring.Locus, List<MediaPlan>> centroids_two)
        {
            foreach (Scoring.Locus locus_one in centroids_one.Keys)
            {
                bool found = false;
                foreach (Scoring.Locus locus_two in centroids_two.Keys)
                {
                    if (locus_one.SameCenter(locus_two))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            foreach (Scoring.Locus locus_one in centroids_two.Keys)
            {
                bool found = false;
                foreach (Scoring.Locus locus_two in centroids_one.Keys)
                {
                    if (locus_one.SameCenter(locus_two))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        private Dictionary<Scoring.Locus, List<MediaPlan>> recompute(Dictionary<Scoring.Locus, List<MediaPlan>> centroids, GetLocus generator)
        {
            Dictionary<Scoring.Locus, List<MediaPlan>> rval = new Dictionary<Scoring.Locus, List<MediaPlan>>();
            foreach (List<MediaPlan> plans in centroids.Values)
            {
                rval.Add(generator(plans), new List<MediaPlan>());
            }

            return rval;
        }

        private void add_plans(Dictionary<Scoring.Locus, MediaPlan> plans, Dictionary<Scoring.Locus, List<MediaPlan>> centroids, GetLocus generator)
        {
            foreach (MediaPlan plan in plans.Values)
            {
                double best_distance = Double.MaxValue;
                List<MediaPlan> temp_list = new List<MediaPlan>();
                temp_list.Add(plan);
                Scoring.Locus nearest_centroid = generator(temp_list);
                foreach (Scoring.Locus centroid in centroids.Keys)
                {
                    double distance = centroid.GetDistance(plan);
                    if (distance < best_distance)
                    {
                        best_distance = distance;
                        nearest_centroid = centroid;
                    }
                }
                centroids[nearest_centroid].Add(plan);
            }
        }
    }

    class SimCreator
    {
        private AllocationService allocation_service;
        private DpMediaDb media_database;
        private DpUpdate sim_database;

        private List<Demographic> demographics;
        private List<string> regions;
        private List<int> lengths;
        private List<double> budgets;
        private List<KeyValuePair<string, string>> categories;
        private List<int> purchase_cycles;
        private List<double> consideration_period;

        private List<MediaCampaignSpecs.CampaignGoal> goals;

        private Random rand;

        public SimCreator(DpMediaDb media, string sim_connection)
        {
            media_database = media;
            sim_database = new DpUpdate(sim_connection);

            allocation_service = new AllocationService();

            Dictionary<string, DemographicLibrary.Demographic.PrimVector> db_regions = media_database.RefreshRegionData();
            media_database.RefreshWebData();

            SortedDictionary<double, string> sorted_regions = new SortedDictionary<double, string>();

            foreach (string region in db_regions.Keys)
            {
                AllocationService.Insert<string>(-db_regions[region].Any, region, sorted_regions);
            }

            regions = new List<string>();

            int i = 0;
            foreach (double key in sorted_regions.Keys)
            {
                if (sorted_regions[key] == "USA")
                {
                    continue;
                }

                regions.Add(sorted_regions[key]);
                i++;
                if (i == 15)
                {
                    break;
                }
            }


            demographics = new List<Demographic>();

            Demographic everyone = new Demographic();
            Demographic men = new Demographic();
            men.Gender = "MALE";
            Demographic women = new Demographic();
            women.Gender = "FEMALE";
            Demographic high_income = new Demographic();
            high_income.Income.Clear();
            high_income.AddIncome(75000, 1000000);
            Demographic older = new Demographic();
            older.Age.Clear();
            older.AddAge(60, 100);
            Demographic latino = new Demographic();
            latino.Race = "LATINO";
            Demographic kids = new Demographic();
            kids.Kids = "YES";
            Demographic high_income_women = new Demographic();
            high_income_women.Gender = "FEMALE";
            high_income_women.Income.Clear();
            high_income_women.AddIncome(75000, 1000000);
            Demographic young_men = new Demographic();
            young_men.Gender = "MALE";
            young_men.Age.Clear();
            young_men.AddAge(18, 30);


            demographics.Add(everyone);
            demographics.Add(everyone);
            demographics.Add(men);
            demographics.Add(women);
            demographics.Add(high_income);
            demographics.Add(older);
            //demographics.Add(latino);
            demographics.Add(kids);
            demographics.Add(high_income_women);
            demographics.Add(young_men);

            purchase_cycles = new List<int>();
            purchase_cycles.Add(1);
            purchase_cycles.Add(6);
            purchase_cycles.Add(6);
            purchase_cycles.Add(6);
            purchase_cycles.Add(9);
            purchase_cycles.Add(9);
            purchase_cycles.Add(9);
            purchase_cycles.Add(12);

            consideration_period = new List<double>();
            consideration_period.Add(0.1);
            consideration_period.Add(0.25);
            consideration_period.Add(0.4);
            consideration_period.Add(0.5);
            consideration_period.Add(0.6);
            consideration_period.Add(0.75);
            consideration_period.Add(0.9);
            consideration_period.Add(1.0);



            categories = new List<KeyValuePair<string, string>>();

            Dictionary<string, int> cats = media_database.GetCategories();
            Dictionary<string, KeyValuePair<string, int>> sub_categories = new Dictionary<string, KeyValuePair<string, int>>();
            foreach (string cat in cats.Keys)
            {
                Dictionary<string, int> sub_cats = media_database.GetSubCategories(cats[cat]);
                foreach (string sub_cat in sub_cats.Keys)
                {
                    if (!sub_categories.ContainsKey(sub_cat))
                    {
                        sub_categories.Add(sub_cat, new KeyValuePair<string, int>(cat, sub_cats[sub_cat]));
                    }
                }
            }

            SortedDictionary<double, KeyValuePair<string, string>> sorted_subcats = new SortedDictionary<double, KeyValuePair<string, string>>();
            foreach (string subcat in sub_categories.Keys)
            {
                AllocationService.Insert<KeyValuePair<string, string>>(-media_database.GetSubTypes(sub_categories[subcat].Value).Count, new KeyValuePair<string, string>(sub_categories[subcat].Key, subcat), sorted_subcats);
            }

            categories = new List<KeyValuePair<string, string>>();
            categories.Add(new KeyValuePair<string, string>("", ""));

            i = 0;
            foreach (double key in sorted_subcats.Keys)
            {
                categories.Add(sorted_subcats[key]);
                i++;
                if (i == 8)
                {
                    break;
                }
            }

            lengths = new List<int>();
            lengths.Add(30);
            lengths.Add(60);
            lengths.Add(90);
            lengths.Add(180);
            lengths.Add(365);

            budgets = new List<double>();
            budgets.Add(5000);
            budgets.Add(10000);
            budgets.Add(10000);
            budgets.Add(15000);
            budgets.Add(25000);
            budgets.Add(50000);
            budgets.Add(10000);
            budgets.Add(15000);
            budgets.Add(25000);
            budgets.Add(50000);
            budgets.Add(75000);
            budgets.Add(100000);
            budgets.Add(15000);
            budgets.Add(25000);
            budgets.Add(50000);
            budgets.Add(75000);
            budgets.Add(100000);
            budgets.Add(500000);
            budgets.Add(1000000);

            goals = new List<MediaCampaignSpecs.CampaignGoal>();
            goals.Add(MediaCampaignSpecs.CampaignGoal.ReachAndAwareness);
            goals.Add(MediaCampaignSpecs.CampaignGoal.Persuasion);
            goals.Add(MediaCampaignSpecs.CampaignGoal.DemographicTargeting);
            goals.Add(MediaCampaignSpecs.CampaignGoal.GeoTargeting);
            goals.Add(MediaCampaignSpecs.CampaignGoal.Recency);

            rand = new Random();
        }

        public Dictionary<Guid, MediaPlan> QueuePlans(int num)
        {
            Dictionary<Guid, MediaPlan> rval = new Dictionary<Guid, MediaPlan>();
            for (int i = 0; i < num; i++)
            {
                List<Demographic> demos = get_demographics();
                List<string> regs = get_regions();
                int l = rand.Next(lengths.Count);
                int b = rand.Next(budgets.Count);
                int c = rand.Next(categories.Count);
                int p = rand.Next(purchase_cycles.Count);
                int cp = rand.Next(consideration_period.Count);
                MediaCampaignSpecs specs = get_specs("Test", demos, regs, lengths[l], budgets[b], categories[c], purchase_cycles[p], consideration_period[cp]);
                MediaPlan plan = allocation_service.CreateNewMediaPlan(specs);
                SimInput input = get_input(plan);

                Guid? guid = sim_database.CreateSimulation("Tester");

                if (!guid.HasValue)
                {
                    continue;
                }

                plan.SimulationID = guid.Value;
                sim_database.UpdateInput(plan.SimulationID.Value, input);
                sim_database.QueueSim(plan.SimulationID.Value);
                rval.Add(plan.SimulationID.Value, plan);
            }

            return rval;
        }

        private SimInput get_input(MediaPlan plan)
        {
            SimInput rval = SimUtils.ConvertMediaPlanToSimInput(plan, null);

            foreach (Demographic demographic in plan.Specs.SegmentList)
            {
                foreach (string region in plan.Specs.GeoRegionNames)
                {
                    Demographic demo = new Demographic(demographic);
                    demo.Region = region;
                    rval.Demographics.Add(demo);
                }
            }

            BusinessLogic.Calibrator.CalibrateInput(plan, rval);

            rval.microscope_size = 0;

            return rval;
        }

        private MediaCampaignSpecs get_specs(string name, Demographic demographic, string region, int length, double budget, KeyValuePair<string, string> category, int purchase_cycle, double consideration_period)
        {
            MediaCampaignSpecs rval = new MediaCampaignSpecs();
            rval.CampaignName = name;
            rval.SegmentList.Add(demographic);
            rval.GeoRegionNames.Add(region);
            rval.StartDate = DateTime.Today;
            rval.EndDate = rval.StartDate.AddDays(length);
            rval.TargetBudget = budget;

            rval.PreviousMediaSpend = 365 / length * budget;
            rval.InitialShare = budget / 10;

            if (category.Key == "")
            {
                rval.BusinessCategory = "(choose a category)";
            }
            else
            {
                rval.BusinessCategory = category.Key;
                rval.BusinessSubcategory = category.Value;
            }

            rval.PurchaseCycleCode = purchase_cycle;
            rval.TimeInConsideration = (int)(100.0 * consideration_period);

            set_goals(rval);

            return rval;
        }

        private MediaCampaignSpecs get_specs(string name, List<Demographic> demographics, List<string> regions, int length, double budget, KeyValuePair<string, string> category, int purchase_cycle, double consideration_period)
        {
            MediaCampaignSpecs rval = new MediaCampaignSpecs();
            rval.CampaignName = name;
            rval.SegmentList.AddRange(demographics);
            rval.GeoRegionNames.AddRange(regions);
            rval.StartDate = DateTime.Today;
            rval.EndDate = rval.StartDate.AddDays(length);
            rval.TargetBudget = budget;

            rval.PreviousMediaSpend = 365 / length * budget;
            rval.InitialShare = budget / 10;

            if (category.Key == "")
            {
                rval.BusinessCategory = "(choose a category)";
            }
            else
            {
                rval.BusinessCategory = category.Key;
                rval.BusinessSubcategory = category.Value;
            }

            rval.PurchaseCycleCode = purchase_cycle;
            rval.TimeInConsideration = (int)(100.0 * consideration_period);

            set_goals(rval);

            return rval;
        }

        private List<Demographic> get_demographics()
        {
            List<Demographic> rval = new List<Demographic>(); ;
            int num_demos = 1;
            for (int i = 0; i < 2; i++)
            {
                if (rand.NextDouble() < 0.4)
                {
                    num_demos++;
                }
            }

            Bucket bucket = new Bucket(demographics.Count);
            for (int i = 0; i < num_demos; i++)
            {
                int index = bucket.Draw;
                rval.Add(demographics[index]);
            }

            return rval;
        }

        private List<string> get_regions()
        {
            List<string> rval = new List<string>();
            int num_demos = 1;
            for (int i = 0; i < 2; i++)
            {
                if (rand.NextDouble() < 0.15)
                {
                    num_demos++;
                }
            }

            Bucket bucket = new Bucket(regions.Count);
            for (int i = 0; i < num_demos; i++)
            {
                int index = bucket.Draw;
                rval.Add(regions[index]);
            }

            return rval;
        }

        private void set_goals(MediaCampaignSpecs specs)
        {
            int num_goals = rand.Next(4) + 1;

            Dictionary<MediaCampaignSpecs.CampaignGoal, int> goal_scores = new Dictionary<MediaCampaignSpecs.CampaignGoal, int>();

            for (int i = 0; i < num_goals; i++)
            {
                MediaCampaignSpecs.CampaignGoal goal = goals[rand.Next(goals.Count)];
                if (!goal_scores.ContainsKey(goal))
                {
                    goal_scores.Add(goal, 1);
                }
                else
                {
                    goal_scores[goal]++;
                }
            }

            foreach (MediaCampaignSpecs.CampaignGoal goal in goal_scores.Keys)
            {
                specs.CampaignGoals.Add(goal);
                double multipler = (double)goal_scores[goal];
                if (rand.NextDouble() > 0.3)
                {
                    specs.GoalWeights.Add(rand.NextDouble() * 3 * multipler);
                }
                else
                {
                    specs.GoalWeights.Add(rand.NextDouble() * multipler);
                }
            }

            double total = 0.0;
            for (int i = 0; i < specs.GoalWeights.Count; i++)
            {
                total += specs.GoalWeights[i];
            }

            for (int i = 0; i < specs.GoalWeights.Count; i++)
            {
                specs.GoalWeights[i] = (specs.GoalWeights[i] / total) * 100;
            }
        }

    }

    public class HighLow : FitnessEvalulator
    {
        
        public List<double> GetFitness(List<List<double>> genes)
        {
            return new List<double>();
        }
    }

    public interface FitnessEvalulator
    {
        List<double> GetFitness(List<List<double>> genes);
    }

    class GenenticAlgorithm
    {
        private static Random rand = new Random();

        private List<double> best;
        private double best_fitness;
        private List<List<double>> population;
        private double val_min;
        private double val_max;

        private double fitness_cut_off;
        private double double_cross_prob;
        private double mutate_prob;
        private double mutate_max;

        private FitnessEvalulator fitness_evalulator;

        public GenenticAlgorithm(int pop_count, int gene_size, double val_min, double val_max, double fitness_cut_off, FitnessEvalulator fitness_evalulator, double double_cross_prob, double mutate_prob, double mutate_max)
        {
            population = new List<List<double>>();
            best = new List<double>();
            for (int i = 0; i < pop_count; i++)
            {
                population.Add(create_random(gene_size, val_min, val_max));
            }
            this.fitness_cut_off = fitness_cut_off;
            this.double_cross_prob = double_cross_prob;
            this.mutate_prob = mutate_prob;
            this.mutate_max = mutate_max;
        }

        public GenenticAlgorithm(int pop_count, List<double> sample, double sample_var, double fitness_cut_off, double double_cross_prob, double mutate_prob, double mutate_max)
        {
            population = new List<List<double>>();
            best = new List<double>();
            for (int i = 0; i < pop_count; i++)
            {
                population.Add(create_random(sample, sample_var));
            }
            this.fitness_cut_off = fitness_cut_off;
            this.double_cross_prob = double_cross_prob;
            this.mutate_prob = mutate_prob;
            this.mutate_max = mutate_max;
        }

        public List<double> create_random(int count, double min, double max)
        {
            List<double> rval = new List<double>();
            for (int i = 0; i < count; i++)
            {
                double value = (rand.Next() * (max - min)) + min;
                rval.Add(value);
            }
            return rval;
        }

        public List<double> create_random(List<double> sample, double mutate_max)
        {
            List<double> rval = new List<double>();
            for (int i = 0; i < sample.Count; i++)
            {
                double value = sample[i] + (2 * mutate_max * rand.Next()) - mutate_max;
                rval.Add(value);
            }
            return rval;
        }

        public void Step()
        {
            List<double> fitness = fitness_evalulator.GetFitness(population);

            double max = fitness.Max();
            if (max > best_fitness)
            {
                int index = fitness.LastIndexOf(max);
                best_fitness = max;
                best = population[index];
            }

            List<List<double>> children = new List<List<double>>();
            List<List<double>> parents = population;

            for (int i = 0; i < parents.Count-1; i++)
            {
                List<double> mom = parents[selection(fitness, 0.5)];
                List<double> dad = parents[selection(fitness, 0.5)];
                children.Add(mate(mom, dad));
            }

            children.Add(best);

            population = children;
        }

        private List<double> mate(List<double> mom, List<double> dad)
        {
            List<double> child = new List<double>();
            bool crossed = false;
            for (int i = 0; i < mom.Count; i++)
            {
                if (!crossed)
                {
                    child.Add(mom[i]);
                }
                else
                {
                    child.Add(dad[i]);
                }
                double prob = 1.0/((double)(mom.Count - i));
                if (rand.NextDouble() < prob)
                {
                    crossed = true;
                }
            }

            mutate(ref child);

            return child;

        }

        private void mutate(ref List<double> child)
        {
            if (rand.NextDouble() < mutate_prob)
            {
                int location = rand.Next(child.Count);
                double amount = 2 * rand.NextDouble() * mutate_max - mutate_max;
                child[location] = child[location] + amount;
            }
        }

        private int selection(List<double> values, double percent_max)
        {
            int max = (int)((double)values.Count * (1 - percent_max));
            return selection(values, max + 1);
        }

        private int selection(List<double> values, int max)
        {
            List<double> list = new List<double>();

            for (int i = 0; i < values.Count && i < max; i++)
            {
                list.Add(values[i]);
            }

            if (list.Count == 0)
            {
                return -1;
            }

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
