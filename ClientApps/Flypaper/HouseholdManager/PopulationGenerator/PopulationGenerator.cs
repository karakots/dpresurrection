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
using HouseholdLibrary;
using GeoLibrary;
using DemographicLibrary;

namespace PopulationGenerator
{
    public partial class PopulationGenerator : Form
    {
        private bool total_file_set;
        private bool size_file_set;
        private bool income_file_set;
        private bool age_file_set;

        //Dictionary<int, GeoInfo> geo_data;

        public List<Household> Households;

        //Data for a GEO_ID
        private int num_households;
        //<RACE, <TYPE, <SIZE, NUM>>>
        private Dictionary<string, Dictionary<string, Dictionary<int, int>>> size_dict;
        //<RACE, <INCOME, NUM>>
        private Dictionary<string, Dictionary<int,int>> income_dict;
        //<RACE, <SEX, <TYPE, <AGE, NUM>>>
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int,int>>>> age_dict;

        private Dictionary<string, StreamReader> file_readers;
        private Dictionary<string, List<string>> file_headers;
        private IFormatter formatter;
        private List<string> errors;

        private Random rand;

        public PopulationGenerator()
        {
            rand = new Random();

            InitializeComponent();

            total_file_set = false;
            size_file_set = false;
            income_file_set = false;
            age_file_set = false;
            file_readers = new Dictionary<string, StreamReader>();
            file_headers = new Dictionary<string, List<string>>();
            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            size_dict = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();
            income_dict = new Dictionary<string, Dictionary<int, int>>();
            age_dict = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>>();
            Households = new List<Household>();
            // geo_data = new Dictionary<int,GeoInfo>();

            errors = new List<string>();
        }

        public PopulationGenerator(DirectoryInfo dir)
        {
            rand = new Random();

            InitializeComponent();

            total_file_set = false;
            size_file_set = false;
            income_file_set = false;
            age_file_set = false;
            file_readers = new Dictionary<string, StreamReader>();
            file_headers = new Dictionary<string, List<string>>();
            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            size_dict = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();
            income_dict = new Dictionary<string, Dictionary<int, int>>();
            age_dict = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>>();
            Households = new List<Household>();

            errors = new List<string>();

            setDirectory( dir );
        }

        private void display_errors()
        {
            string error_string = "";
            foreach (string error in errors)
            {
                error_string += error + "\n";
            }

            MessageBox.Show(error_string, "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errors.Clear();
        }

        #region UI_controls
        private void total_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            total_file.Text = dialog.FileName;

            if (errors.Count > 0)
            {
                display_errors();
            }
        }

        private void size_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            size_file.Text = dialog.FileName;

            if (errors.Count > 0)
            {
                display_errors();
            }
        }

        private void income_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            income_file.Text = dialog.FileName;

            if (errors.Count > 0)
            {
                display_errors();
            }
        }

        private void age_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            age_file.Text = dialog.FileName;

            if (errors.Count > 0)
            {
                display_errors();
            }
        }

        private void setDirectory( DirectoryInfo dir)
        {
            foreach( FileInfo file in dir.GetFiles() )
            {
                if( file.Name.IndexOf( "num", StringComparison.OrdinalIgnoreCase ) > 0 )
                {
                    total_file.Text = file.FullName;
                }
                else if( file.Name.IndexOf( "size", StringComparison.OrdinalIgnoreCase ) > 0 )
                {
                    size_file.Text = file.FullName;
                }
                else if( file.Name.IndexOf( "income", StringComparison.OrdinalIgnoreCase ) > 0 )
                {
                    income_file.Text = file.FullName;
                }
                else if( file.Name.IndexOf( "age", StringComparison.OrdinalIgnoreCase ) > 0 )
                {
                    age_file.Text = file.FullName;
                }
            }

            if( errors.Count > 0 )
            {
                display_errors();
            }
        }

        #endregion

        #region Analysis
        private bool analyze_total_file(string total_file)
        {
            StreamReader reader = new StreamReader(total_file);

            List<string> header = new List<string>(reader.ReadLine().Split('\t'));

            if (header.Count != 2)
            {
                errors.Add("Total file: " + total_file + " has incorrect number of columns!");
                return false;
            }

            if (header[0] != "GEO_ID")
            {
                errors.Add("Total file: " + total_file + " does have GEO_ID as first column");
                return false;
            }

            if (header[1] != "NUM_HOUSEHOLDS")
            {
                errors.Add("Total file: " + total_file + " does have NUM_HOUSEHOLDS as second column");
                return false;
            }

            List<string> first_row = new List<string>(reader.ReadLine().Split('\t'));

            if (first_row.Count != 2)
            {
                errors.Add("Total file: " + total_file + " has incorrect number of columns!");
                return false;
            }

            int test;
            if (!int.TryParse(first_row[0], out test))
            {
                errors.Add("Total file: " + total_file + " unable to parse first column as an integer");
                return false;
            }

            if (!int.TryParse(first_row[1], out test))
            {
                errors.Add("Total file: " + total_file + " unable to parse second column as an integer");
                return false;
            }
            reader.Close();
            return true;

        }

        private bool analyze_size_file(string size_file)
        {
            StreamReader reader = new StreamReader(size_file);

            List<string> header = new List<string>(reader.ReadLine().Split('\t'));

            if (header.Count != 5)
            {
                errors.Add("Size file: " + size_file + " has incorrect number of columns!");
                return false;
            }
            if (header[0] != "GEO_ID")
            {
                errors.Add("Size file: " + size_file + " does have GEO_ID as first column");
                return false;
            }
            if (header[1] != "RACE")
            {
                errors.Add("Size file: " + size_file + " does have RACE as second column");
                return false;
            }
            if (header[2] != "TYPE")
            {
                errors.Add("Size file: " + size_file + " does have TYPE as third column");
                return false;
            }
            if (header[3] != "SIZE")
            {
                errors.Add("Size file: " + size_file + " does have SIZE as fourth column");
                return false;
            }
            if (header[4] != "NUM_HOUSEHOLDS")
            {
                errors.Add("Size file: " + size_file + " does have NUM_HOUSEHOLDS as fifth column");
                return false;
            }

            List<string> first_row = new List<string>(reader.ReadLine().Split('\t'));

            if (first_row.Count != 5)
            {
                errors.Add("Size file: " + size_file + " has incorrect number of columns!");
                return false;
            }
            int test;
            if (!int.TryParse(first_row[0], out test))
            {
                errors.Add("Size file: " + size_file + " unable to parse first column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[3], out test))
            {
                errors.Add("Size file: " + size_file + " unable to parse fourth column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[4], out test))
            {
                errors.Add("Size file: " + size_file + " unable to parse fifth column as an integer");
                return false;
            }
            reader.Close();
            return true;

        }

        private bool analyze_income_file(string income_file)
        {
            StreamReader reader = new StreamReader(income_file);

            List<string> header = new List<string>(reader.ReadLine().Split('\t'));
            if (header.Count != 4)
            {
                errors.Add("Income file: " + income_file + " has incorrect number of columns!");
                return false;
            }
            if (header[0] != "GEO_ID")
            {
                errors.Add("Income file: " + income_file + " does have GEO_ID as first column");
                return false;
            }
            if (header[1] != "RACE")
            {
                errors.Add("Income file: " + income_file + " does have RACE as second column");
                return false;
            }
            if (header[2] != "INCOME")
            {
                errors.Add("Income file: " + income_file + " does have INCOME as third column");
                return false;
            }
            if (header[3] != "NUM_HOUSEHOLDS")
            {
                errors.Add("Income file: " + income_file + " does have NUM_HOUSEHOLDS as fourth column");
                return false;
            }

            List<string> first_row = new List<string>(reader.ReadLine().Split('\t'));
            if (first_row.Count != 4)
            {
                errors.Add("Income file: " + income_file + " has incorrect number of columns!");
                return false;
            }
            int test;
            if (!int.TryParse(first_row[0], out test))
            {
                errors.Add("Income file: " + income_file + " unable to parse first column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[2], out test))
            {
                errors.Add("Income file: " + income_file + " unable to parse third column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[3], out test))
            {
                errors.Add("Income file: " + income_file + " unable to parse fourth column as an integer");
                return false;
            }
            reader.Close();
            return true;

        }

        private bool analyze_age_file(string age_file)
        {
            StreamReader reader = new StreamReader(age_file);

            List<string> header = new List<string>(reader.ReadLine().Split('\t'));

            if (header.Count != 5)
            {
                errors.Add("Age file: " + age_file + " has incorrect number of columns!");
                return false;
            }
            if (header[0] != "GEO_ID")
            {
                errors.Add("Age file: " + age_file + " does have GEO_ID as first column");
                return false;
            }
            if (header[1] != "RACE")
            {
                errors.Add("Age file: " + age_file + " does have RACE as second column");
                return false;
            }
            if (header[2] != "GENDER")
            {
                errors.Add("Age file: " + age_file + " does have GENDER as third column");
                return false;
            }
            if (header[3] != "AGE")
            {
                errors.Add("Age file: " + age_file + " does have AGE as fourth column");
                return false;
            }
            if (header[4] != "NUM_HOUSEHOLDS")
            {
                errors.Add("Age file: " + age_file + " does have NUM_HOUSEHOLDS as fifth column");
                return false;
            }

            List<string> first_row = new List<string>(reader.ReadLine().Split('\t'));
            if (first_row.Count != 5)
            {
                errors.Add("Age file: " + age_file + " has incorrect number of columns!");
                return false;
            }
            int test;
            if (!int.TryParse(first_row[0], out test))
            {
                errors.Add("Age file: " + age_file + " unable to parse first column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[3], out test))
            {
                errors.Add("Age file: " + age_file + " unable to parse fourth column as an integer");
                return false;
            }
            if (!int.TryParse(first_row[4], out test))
            {
                errors.Add("Age file: " + age_file + " unable to parse fifth column as an integer");
                return false;
            }
            reader.Close();
            return true;

        }

        private void total_file_TextChanged(object sender, EventArgs e)
        {
            if (total_file.Text != "Total Households File")
            {
                if (analyze_total_file(total_file.Text))
                {
                    total_file_set = true;
                }
                else
                {
                    total_file_set = false;
                    total_file.Text = "Total Households File";
                }
            }
        }

        private void size_file_TextChanged(object sender, EventArgs e)
        {
            if (size_file.Text != "Houshold Size and Type by Race")
            {
                if (analyze_size_file(size_file.Text))
                {
                    size_file_set = true;
                }
                else
                {
                    size_file_set = false;
                    size_file.Text = "Houshold Size and Type by Race";
                }
            }
        }

        private void income_file_TextChanged(object sender, EventArgs e)
        {
            if (income_file.Text != "Household Income by Race")
            {
                if (analyze_income_file(income_file.Text))
                {
                    income_file_set = true;
                }
                else
                {
                    income_file_set = false;
                    income_file.Text = "Household Income by Race";
                }
            }
        }

        private void age_file_TextChanged(object sender, EventArgs e)
        {
            if (age_file.Text != "Sex and Age by Race")
            {
                if (analyze_age_file(age_file.Text))
                {
                    age_file_set = true;
                }
                else
                {
                    age_file_set = false;
                    age_file.Text = "Sex and Age by Race";
                }
            }
        }
        #endregion

        #region Generation
        private void generate_button_Click(object sender, EventArgs e)
        {
            if (GenerateTimer.Enabled)
            {
                GenerateTimer.Stop();
                GenerateTimer.Enabled = false;
                clean_up();
                generate_button.Text = "Generate!";
                DialogResult = DialogResult.Cancel;
                return;
            }
            if (!(total_file_set && size_file_set && income_file_set && age_file_set))
            {
                return;
            }

            file_readers.Clear();

            try
            {
                file_readers.Add("NUM_HOUSEHOLDS", new StreamReader(total_file.Text));
                file_readers.Add("SIZE", new StreamReader(size_file.Text));
                file_readers.Add("INCOME", new StreamReader(income_file.Text));
                file_readers.Add("AGE", new StreamReader(age_file.Text));
            }
            catch (Exception ex)
            {
                errors.Add("Error opening files: " + ex.Message);
                display_errors();
                return;
            }

            try
            {
                foreach (KeyValuePair<string, StreamReader> pair in file_readers)
                {
                    file_headers.Add(pair.Key, new List<string>(pair.Value.ReadLine().Split('\t')));
                }
            }
            catch (Exception ex)
            {
                errors.Add("Error reading files: " + ex.Message);
                display_errors();
                return;
            }
            Households.Clear();
            Households = new List<Household>();
            generate_button.Text = "STOP!";

            controls_toggle(false);

            GenerateTimer.Enabled = true;
            GenerateTimer.Start();

        }

        private void controls_toggle(bool enabled)
        {
            total_browse.Enabled = enabled;
            size_browse.Enabled = enabled;
            age_browse.Enabled = enabled;
            income_browse.Enabled = enabled;
            scale_up_down.Enabled = enabled;
        }

        private void clean_up()
        {
            foreach (StreamReader reader in file_readers.Values)
            {
                reader.Close();
            }
            file_readers.Clear();
        }

        private void GenerateTimer_Tick(object sender, EventArgs e)
        {
            if (file_readers["NUM_HOUSEHOLDS"].EndOfStream)
            {
                GenerateTimer.Stop();
                clean_up();
                DialogResult = DialogResult.OK;
                return;
            }

            int geo_id = load_next_geo_id();

            //DirectoryInfo new_folder = new DirectoryInfo(save_folder.FullName + "\\" + geo_data[geo_id].State);

            //if (!new_folder.Exists)
            //{
            //    new_folder.Create();
            //}

            build_houses(geo_id);

            clear_data();

            progressBar1.Value++;
        }

        private void build_houses(int geo_id)
        {
            Household house;
            List<Household> houses = new List<Household>();
            while (num_households > 0 && size_dict.Count > 0)
            {
                house = build_house(geo_id);
                houses.Add(house);
                num_households--;
            }

            fill_houses(houses);
            Households.AddRange(houses);
            houses.Clear();
        }

        private Household build_house(int geo_id)
        {
            string size_race;
            string income_race;
            string type;
            int size;
            int income;
            size_race = size_dict.Keys.ElementAt(rand.Next(size_dict.Keys.Count));
            type = size_dict[size_race].Keys.ElementAt(rand.Next(size_dict[size_race].Keys.Count));
            size = size_dict[size_race][type].Keys.ElementAt(rand.Next(size_dict[size_race][type].Count));
            income_race = size_race;
            income = 35000;
            if (!income_dict.ContainsKey(income_race))
            {
                if (income_dict.Count > 0)
                {
                    income_race = income_dict.Keys.ElementAt(rand.Next(income_dict.Keys.Count));
                    income = income_dict[income_race].Keys.ElementAt(rand.Next(income_dict[income_race].Keys.Count));
                }

            }
            else
            {
                income = income_dict[income_race].Keys.ElementAt(rand.Next(income_dict[income_race].Keys.Count));
            }
            Household house = new Household(geo_id, size_race, size, type, income);
            
            size_dict[size_race][type][size]--;
            if (size_dict[size_race][type][size] == 0)
            {
                size_dict[size_race][type].Remove(size);
                if (size_dict[size_race][type].Count == 0)
                {
                    size_dict[size_race].Remove(type);
                    if (size_dict[size_race].Count == 0)
                    {
                        size_dict.Remove(size_race);
                    }
                }
            }
            

            if (income_dict.Count > 0)
            {
                income_dict[income_race][income]--;
                if (income_dict[income_race][income] == 0)
                {
                    income_dict[income_race].Remove(income);
                    if (income_dict[income_race].Count == 0)
                    {
                        income_dict.Remove(income_race);
                    }
                }
            }

            return house;
        }

        private void fill_houses(List<Household> houses)
        {
            if (houses.Count == 0)
            {
                return;
            }

            int count = 0;
            int index = 0;
            Person person;
            while (adult_left(houses[index]))
            {
                get_adult(houses[index], out person);
                houses[index].AddPerson(person);
                count++;
                index = count % houses.Count;
            }

            if (count < houses.Count)
            {
                for (int i = count; i < houses.Count; i++)
                {
                    get_adult(houses[i], out person);
                    houses[i].AddPerson(person);
                }
            }

            foreach (Household house in houses)
            {
                while (house.NumOccupants() < house.Size)
                {
                    get_child(house, out person);
                    house.AddPerson(person);
                }
            }
        }

        private bool adult_left(Household house)
        {
            if (!age_dict.ContainsKey(house.Race.ToString()) || !age_dict[house.Race.ToString()].ContainsKey("ADULT"))
            {
                return false;
            }

            return true;
        }

        private void get_adult(Household house, out Person person)
        {
            string person_race = house.Race.ToString();
            //For the first iteration no logice will be used...
            if (!age_dict.ContainsKey(person_race) || !age_dict[person_race].ContainsKey("ADULT"))
            {
                person = new Person(random_gender(), house.Race, rand.Next(18, 64));
            }
            else
            {
                string gender = age_dict[person_race]["ADULT"].Keys.ElementAt(rand.Next(age_dict[person_race]["ADULT"].Keys.Count));
                int age = age_dict[person_race]["ADULT"][gender].Keys.ElementAt(rand.Next(age_dict[person_race]["ADULT"][gender].Keys.Count));
                person = new Person(gender, person_race, age);
                age_dict[person_race]["ADULT"][gender][age]--;
                if (age_dict[person_race]["ADULT"][gender][age] < 1)
                {
                    age_dict[person_race]["ADULT"][gender].Remove(age);
                    if (age_dict[person_race]["ADULT"][gender].Keys.Count < 1)
                    {
                        age_dict[person_race]["ADULT"].Remove(gender);
                        if (age_dict[person_race]["ADULT"].Keys.Count < 1)
                        {
                            age_dict[person_race].Remove("ADULT");
                            if (age_dict[person_race].Keys.Count < 1)
                            {
                                age_dict.Remove(person_race);
                            }
                        }
                    }
                }
            }
        }

        private void get_child(Household house, out Person person)
        {
            string person_race = house.Race.ToString();
            //For the first iteration no logice will be used...
            if (!age_dict.ContainsKey(person_race) || !age_dict[person_race].ContainsKey("CHILD"))
            {
                person = new Person(random_gender(), house.Race, rand.Next(0, 18));
            }
            else
            {
                string gender = age_dict[person_race]["CHILD"].Keys.ElementAt(rand.Next(age_dict[person_race]["CHILD"].Keys.Count));
                int age = age_dict[person_race]["CHILD"][gender].Keys.ElementAt(rand.Next(age_dict[person_race]["CHILD"][gender].Keys.Count));
                person = new Person(gender, person_race, age);
                age_dict[person_race]["CHILD"][gender][age]--;
                if (age_dict[person_race]["CHILD"][gender][age] < 1)
                {
                    age_dict[person_race]["CHILD"][gender].Remove(age);
                    if (age_dict[person_race]["CHILD"][gender].Keys.Count < 1)
                    {
                        age_dict[person_race]["CHILD"].Remove(gender);
                        if (age_dict[person_race]["CHILD"].Keys.Count < 1)
                        {
                            age_dict[person_race].Remove("CHILD");
                            if (age_dict[person_race].Keys.Count < 1)
                            {
                                age_dict.Remove(person_race);
                            }
                        }
                    }
                }
            }
        }

        private DemographicType<Gender> random_gender()
        {
            if (rand.NextDouble() < 0.5)
            {
                return "FEMALE";
            }

            return "MALE";
        }

        private int load_next_geo_id()
        {
            string[] row;
            int value;
            int geo_id;

            row = file_readers["NUM_HOUSEHOLDS"].ReadLine().Split('\t');

            geo_id = int.Parse(row[0]);
            if (row[1] == "")
            {
                num_households = 0;
            }
            else
            {
                num_households = scaled_parse(row[1]);
            }

            //How do I know there are 104 rows?...I counted :)
            for (int i = 0; i < 104; i++)
            {
                row = file_readers["SIZE"].ReadLine().Split('\t');
                value = scaled_parse(row[4]);
                if (value > 0)
                {
                    if (!size_dict.ContainsKey(row[1]))
                    {
                        size_dict.Add(row[1], new Dictionary<string, Dictionary<int, int>>());
                    }
                    if (!size_dict[row[1]].ContainsKey(row[2]))
                    {
                        size_dict[row[1]].Add(row[2], new Dictionary<int, int>());
                    }
                    size_dict[row[1]][row[2]].Add(int.Parse(row[3]), value);
                }
            }

            for (int i = 0; i < 128; i++)
            {
                row = file_readers["INCOME"].ReadLine().Split('\t');
                value = scaled_parse(row[3]);
                if(value > 0)
                {
                    if (!income_dict.ContainsKey(row[1]))
                    {
                        income_dict.Add(row[1], new Dictionary<int, int>());
                    }
                    income_dict[row[1]].Add(int.Parse(row[2]), value);
                }
            }

            for (int i = 0; i < 368; i++)
            {
                row = file_readers["AGE"].ReadLine().Split('\t');
                value = scaled_parse(row[4]);
                if(value > 0)
                {
                    if (!age_dict.ContainsKey(row[1]))
                    {
                        age_dict.Add(row[1], new Dictionary<string, Dictionary<string, Dictionary<int, int>>>());
                    }
                    int age = int.Parse(row[3]);
                    string type;
                    if (age > 17)
                    {
                        type = "ADULT";
                    }
                    else
                    {
                        type = "CHILD";
                    }
                    if (!age_dict[row[1]].ContainsKey(type))
                    {
                        age_dict[row[1]].Add(type, new Dictionary<string , Dictionary<int, int>>());
                    }
                    if (!age_dict[row[1]][type].ContainsKey(row[2]))
                    {
                        age_dict[row[1]][type].Add(row[2], new Dictionary<int,int>());
                    }
                    age_dict[row[1]][type][row[2]].Add(age, value);
                }
            }

            return geo_id;

        }

        private void clear_data()
        {
            foreach (Dictionary<string, Dictionary<int,int>> dict1 in size_dict.Values)
            {
                foreach (Dictionary<int, int> dict2 in dict1.Values)
                {
                    dict2.Clear();
                }
                dict1.Clear();
            }
            size_dict.Clear();

            foreach (Dictionary<int, int> dict1 in income_dict.Values)
            {
                dict1.Clear();
            }
            income_dict.Clear();

            foreach (Dictionary<string, Dictionary<string, Dictionary<int, int>>> dict0 in age_dict.Values)
            {
                foreach (Dictionary<string, Dictionary<int, int>> dict1 in dict0.Values)
                {
                    foreach (Dictionary<int, int> dict2 in dict1.Values)
                    {
                        dict2.Clear();
                    }
                    dict1.Clear();
                }
                dict0.Clear();
            }
            age_dict.Clear();
        }

        private int scaled_parse(string int_string)
        {
            double value = int.Parse(int_string) * (double)scale_up_down.Value;
            int ret_val = (int)value;
            double part = value - ret_val;
            if (rand.NextDouble() < part)
            {
                ret_val++;
            }
            return ret_val;
        }
        #endregion

    }
}
