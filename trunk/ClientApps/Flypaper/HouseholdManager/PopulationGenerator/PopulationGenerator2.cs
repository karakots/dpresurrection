using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Configuration;
using DemographicLibrary;
using MediaLibrary;
using HouseholdLibrary;
using GeoLibrary;

namespace PopulationGenerator
{
    public partial class PopulationGenerator2 : Form
    {
        private static Random rand = new Random();
        private static IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        private CensusDataSet database;
        private CensusDataSetTableAdapters.TableAdapterManager table_manager;

        private Tree house_tree;
        private Tree people_tree;

        private List<Agent> agents;
        private double scale;

        private DirectoryInfo directory;

        public PopulationGenerator2(string connectionString)
        {
            InitializeComponent();

            database = new CensusDataSet();

            if( connectionString == null )
            {
                connectionString = Properties.Settings.Default.CensusConnectionString;
            }

            initialize_adapters( connectionString );

            
            house_tree = new Tree();
            people_tree = new Tree();

            Directory = new DirectoryInfo("C:\\temp");

            agents = new List<Agent>();
        }

        public DirectoryInfo Directory
        {
            set
            {
                directory = value;
                SaveDirectoryText.Text = directory.FullName;
            }
            get
            {
                return directory;
            }
        }

        private void initialize_adapters( string connectStr )
        {
            table_manager = new global::PopulationGenerator.CensusDataSetTableAdapters.TableAdapterManager();
            

            table_manager.hh_totalTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.hh_totalTableAdapter();
            table_manager.hh_type_sizeTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.hh_type_sizeTableAdapter();
            table_manager.hh_race_type_childernTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.hh_race_type_childernTableAdapter();
            table_manager.hh_race_incomeTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.hh_race_incomeTableAdapter();
            //table_manager.pop_race_ageTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.pop_race_ageTableAdapter();
            table_manager.hh_race_age_incomeTableAdapter = new global::PopulationGenerator.CensusDataSetTableAdapters.hh_race_age_incomeTableAdapter();

            table_manager.Connection.ConnectionString = connectStr;
            table_manager.hh_totalTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.hh_type_sizeTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.hh_race_type_childernTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.hh_race_incomeTableAdapter.Connection.ConnectionString = connectStr;
            table_manager.hh_race_age_incomeTableAdapter.Connection.ConnectionString = connectStr;
          
        }

        private void load_tables(int geo_id)
        {
            table_manager.hh_type_sizeTableAdapter.Adapter.SelectCommand = new System.Data.SqlClient.SqlCommand( "SELECT * FROM hh_type_size WHERE GEOID = " + geo_id, table_manager.hh_type_sizeTableAdapter.Connection );
            table_manager.hh_race_type_childernTableAdapter.Adapter.SelectCommand = new System.Data.SqlClient.SqlCommand( "SELECT * FROM hh_race_type_childern WHERE GEOID = " + geo_id, table_manager.hh_race_type_childernTableAdapter.Connection );
            table_manager.hh_race_incomeTableAdapter.Adapter.SelectCommand = new System.Data.SqlClient.SqlCommand( "SELECT * FROM hh_race_income WHERE GEOID = " + geo_id, table_manager.hh_race_incomeTableAdapter.Connection );
            table_manager.hh_race_age_incomeTableAdapter.Adapter.SelectCommand = new System.Data.SqlClient.SqlCommand( "SELECT * FROM hh_race_age_income WHERE GEOID = " + geo_id, table_manager.hh_race_incomeTableAdapter.Connection );

            database.hh_type_size.Clear();
            database.hh_race_type_childern.Clear();
            database.hh_race_income.Clear();
            database.hh_race_age_income.Clear();

            table_manager.hh_type_sizeTableAdapter.Adapter.Fill( database.hh_type_size );
            table_manager.hh_race_type_childernTableAdapter.Adapter.Fill( database.hh_race_type_childern );
            table_manager.hh_race_incomeTableAdapter.Adapter.Fill( database.hh_race_income );
            table_manager.hh_race_age_incomeTableAdapter.Adapter.Fill( database.hh_race_age_income );
        }

        private Household build_house(int geo_id)
        {
            string race = house_tree.RandomLeaf();
            string type = house_tree[race].RandomLeaf();
            string size = house_tree[race][type].RandomLeaf();
            string income = people_tree[race].RandomLeaf();

            string[] incomes = income.Split(':');
            int min_income = int.Parse(incomes[0]);
            int max_income = int.Parse(incomes[1]);

            string house_type = type;

            Household house = new Household(geo_id, convert_race(race), int.Parse(size), house_type.Trim(), rand.Next(min_income, max_income));

            int num_people = house.Size;

            if (type == "FAMILY   ")
            {
                string fam_type = house_tree[race][type][size].RandomLeaf();
                if (fam_type != null)
                {
                    string[] values = fam_type.Split(':');
                    string gender = values[0];
                    string young_child = values[1];
                    string old_child = values[2];

                    if (gender == "BOTH  ")
                    {
                        Person mom = build_person(race, income, "FEMALE");
                        Person dad = build_person(race, income, "MALE");
                        house.AddPerson(mom);
                        house.AddPerson(dad);
                        num_people -= 2;
                    }
                    else
                    {
                        Person parent = build_person(race, income, gender);
                        house.AddPerson(parent);
                        num_people -= 1;
                    }

                    if (young_child == "YES")
                    {
                        Person child = build_child(1, 6);
                        house.AddPerson(child);
                        num_people -= 1;
                    }

                    if (old_child == "YES")
                    {
                        Person child = build_child(7, 18);
                        house.AddPerson(child);
                        num_people -= 1;
                    }

                    int num_children = rand.Next(num_people);
                    for (int i = 0; i < num_children; i++)
                    {
                        Person child = build_child(1, 18);
                        house.AddPerson(child);
                        num_people -= 1;
                    }
                }
            }

            for (int i = 0; i < num_people; i++)
            {
                Person person = build_person(race, income, "ANY");
                house.AddPerson(person);
            }

            return house;
        }

        private string convert_race(string race)
        {
            string ret_val = race.Trim();
            if (ret_val == "BLACK" || ret_val == "ASIAN" || ret_val == "WHITE" || ret_val == "LATINO")
            {
                return ret_val;
            }

            return "OTHER";
        }

        private Person build_person(string race, string income, string gender)
        {
            string age = people_tree[race][income].RandomLeaf();
            string[] ages = age.Split(':');
            int min_age = int.Parse(ages[0]);

            if( min_age == 0 )
            {
                min_age = 1;
            }

            int max_age = int.Parse(ages[1]);

            if (gender == "ANY")
            {
                if (rand.NextDouble() < 0.5)
                {
                    gender = "MALE";
                }
                else
                {
                    gender = "FEMALE";
                }
            }

            Person person = new Person(gender, convert_race(race), rand.Next(min_age, max_age));

            return person;
        }

        private Person build_child(int min_age, int max_age)
        {
            if( min_age == 0 )
            {
                min_age = 1;
            }

            return new Person(DemographicType<Gender>.ANY, DemographicType<Race>.ANY, rand.Next(min_age, max_age));
        }

        private void build_trees(int geo_id)
        {
            load_tables(geo_id);

            house_tree.Clear();
            people_tree.Clear();

            build_people_tree();
            build_house_tree();
        }

        private void build_house_tree()
        {
            foreach (CensusDataSet.hh_type_sizeRow row in database.hh_type_size.Rows)
            {
                string race = row.RACE;
                string type = row.TYPE;
                string size = row.SIZE.ToString();
                int num_hh = row.NUM_HH;

                if (num_hh == 0)
                {
                    continue;
                }

                if (!people_tree.ContainsID(race))
                {
                    continue;
                }

                house_tree.AddLeaf(race, 0.0).AddLeaf(type, 0.0).AddLeaf(size, num_hh);
            }

            foreach (Tree race_leaf in house_tree.Leafs)
            {
                foreach (Tree type_leaf in race_leaf.Leafs)
                {
                    type_leaf.SumLeafs();
                }

                race_leaf.SumLeafs();
            }

            foreach (CensusDataSet.hh_race_type_childernRow row in database.hh_race_type_childern.Rows)
            {
                string race = row.RACE;
                string type = "FAMILY   ";
                int min_size = row.MIN_SIZE;
                int max_size = row.MAX_SIZE;
                string id = row.GENDER + ":" + row.YOUNG_CHILD + ":" + row.OLD_CHILD;
                int num_hh = row.NUM_HH;

                if (num_hh == 0)
                {
                    continue;
                }

                if (!(house_tree.ContainsID(race) && house_tree[race].ContainsID(type)))
                {
                    continue;
                }

                foreach (string size in house_tree[race][type].Keys)
                {
                    int i_size = int.Parse(size);
                    if (i_size >= min_size && i_size <= max_size)
                    {
                        house_tree[race][type][size].AddLeaf(id, num_hh);
                    }
                }
            }

            house_tree.Normalize();
        }

        private void build_people_tree()
        {
            foreach (CensusDataSet.hh_race_incomeRow row in database.hh_race_income)
            {
                string race = row.RACE;
                string income = row.MIN_INCOME.ToString() + ":" + row.MAX_INCOME.ToString();
                int num_hh = row.NUM_HH;

                if (num_hh == 0)
                {
                    continue;
                }

                if (race == "MIXED           ")
                {
                    continue;
                }

                people_tree.AddLeaf(race, 0.0).AddLeaf(income, num_hh);
            }

            foreach (Tree leaf in people_tree.Leafs)
            {
                leaf.SumLeafs();
            }

            foreach (CensusDataSet.hh_race_age_incomeRow row in database.hh_race_age_income)
            {
                string race = row.RACE;
                string income = row.MIN_INCOME.ToString() + ":" + row.MAX_INCOME.ToString();
                string age = row.MIN_AGE.ToString() + ":" + row.MAX_AGE.ToString();
                int num_hh = row.NUM_HH;

                if (num_hh == 0)
                {
                    continue;
                }

                if (!people_tree.ContainsID(race) || !people_tree[race].ContainsID(income))
                {
                    continue;
                }

                people_tree[race][income].AddLeaf(age, num_hh);
            }

            List<string> null_races = new List<string>();
            foreach (Tree race_leaf in people_tree.Leafs)
            {
                double race_total = 0.0;
                List<string> null_incomes = new List<string>();
                foreach (Tree income_leaf in race_leaf.Leafs)
                {
                    double income_total = 0.0;
                    foreach (Tree age_leaf in income_leaf.Leafs)
                    {
                        income_total += age_leaf.Value;
                        race_total += age_leaf.Value;
                    }
                    if (income_total == 0)
                    {
                        null_incomes.Add(income_leaf.Itentifier);
                    }
                }
                foreach (string id in null_incomes)
                {
                    race_leaf.RemoveLeaf(id);
                }
                if (race_total == 0.0)
                {
                    null_races.Add(race_leaf.Itentifier);
                }
            }
            foreach (string id in null_races)
            {
                people_tree.RemoveLeaf(id);
            }

            people_tree.Normalize();
        }

        private void generate_button_Click(object sender, EventArgs e)
        {
            if( generate_button.Text != "STOP!")
            {
                pop_timer.Start();
                generate_button.Text = "STOP!";
                scale_up_down.Enabled = false;
                scale = (double)scale_up_down.Value;

                foreach (FileInfo file in Directory.GetFiles())
                {
                    file.Delete();
                }

                generate_pop();
            }
            else
            {
                generate_button.Text = "Generate";
                scale_up_down.Enabled = true;
            }

        }

        private void generate_pop()
        {
            int file_index = 0;

            table_manager.hh_totalTableAdapter.Fill( database.hh_total );

            foreach( CensusDataSet.hh_totalRow hh_tot in database.hh_total )
            {
                int geo_id = hh_tot.GEOID;
                int num_households = hh_tot.NUM_HH;

                num_households = (int)Math.Ceiling( scale * num_households );

                if( num_households == 0 )
                {
                    // update num_zero_households
                    continue;
                }


                build_trees( geo_id );

                if( house_tree.Leafs.Count == 0 || people_tree.Leafs.Count == 0 )
                {
                    // update no occupents
                    continue;
                }

                for( int i = 0; i < num_households; i++ )
                {
                    Agent agent = new Agent( build_house( geo_id ) );

                    // check agent
                    if( agent.House.Race == DemographicType<Race>.NULL ||
                        agent.House.People.Any( person => person.Age == 0 || person.Gender == DemographicType<Gender>.NULL || person.Race == DemographicType<Race>.NULL )
                        )
                    {
                        MessageBox.Show( "oops" );
                    }

                    agents.Add( agent );

                    if( agents.Count >= 10000 )
                    {
                        FileStream o_stream = new FileStream( Directory.FullName + "\\agent" + file_index + ".dat", FileMode.Create );
                        formatter.Serialize( o_stream, agents );
                        o_stream.Close();
                        agents.Clear();
                        file_index++;
                    }
                }
            }

            if( agents.Count >= 0 )
            {
                FileStream o_stream = new FileStream( Directory.FullName + "\\agent" + file_index + ".dat", FileMode.Create );
                formatter.Serialize( o_stream, agents );
                o_stream.Close();
                agents.Clear();
            }
        }

        private class Tree
        {
            private static Random rand = new Random();

            private double val;
            private string ident;

            private Dictionary<string, Tree> leafs;

            public Tree()
            {
                ident = "ROOT";
                val = 1.0;

                leafs = new Dictionary<string, Tree>();
            }

            private Tree(string identifier, double value)
            {
                ident = identifier;
                val = value;

                leafs = new Dictionary<string, Tree>();
            }

            public string Itentifier
            {
                get
                {
                    return ident;
                }
            }

            public double Value
            {
                get
                {
                    return val;
                }
            }

            public Dictionary<string, Tree>.ValueCollection Leafs
            {
                get
                {
                    return leafs.Values;
                }
            }

            public Dictionary<string, Tree>.KeyCollection Keys
            {
                get
                {
                    return leafs.Keys;
                }
            }

            public Tree this[string identifier]
            {
                get
                {
                    return leafs[identifier];
                }
            }

            public bool ContainsID(string identifier)
            {
                if (leafs.ContainsKey(identifier))
                {
                    return true;
                }
                return false;
            }

            public Tree AddLeaf(string identifier, double value)
            {
                Tree ret_val;
                if(!leafs.ContainsKey(identifier))
                {
                    ret_val = new Tree(identifier, value);
                    leafs.Add(identifier, ret_val);
                }
                else
                {
                    ret_val = leafs[identifier];
                }

                return ret_val;
            }

            public void RemoveLeaf(string identifier)
            {
                if (leafs.ContainsKey(identifier))
                {
                    leafs[identifier].Clear();
                    leafs.Remove(identifier);
                }
            }

            public void SumLeafs()
            {
                val = 0.0;
                foreach (Tree leaf in Leafs)
                {
                    val += leaf.Value;
                }
            }

            public void Normalize()
            {
                double total = 0.0;
                foreach (Tree leaf in Leafs)
                {
                    total += leaf.Value;
                }

                foreach (Tree leaf in Leafs)
                {
                    leaf.normalize(total);
                }
            }

            public string RandomLeaf()
            {
                double total = 0.0;
                double random = rand.NextDouble();
                foreach (Tree leaf in Leafs)
                {
                    if (random < total + leaf.Value)
                    {
                        return leaf.Itentifier;
                    }
                    else
                    {
                        total += leaf.Value;
                    }
                }

                return null;
            }

            public void Clear()
            {
                foreach (Tree leaf in Leafs)
                {
                    leaf.Clear();
                }

                leafs.Clear();
            }

            private void normalize(double sum)
            {
                val /= sum;

                double total = 0.0;
                List<string> null_ids = new List<string>();
                foreach (Tree leaf in Leafs)
                {
                    total += leaf.Value;
                }

                foreach (Tree leaf in Leafs)
                {
                    leaf.normalize(total);
                }
            }


        }

        
    }
}
