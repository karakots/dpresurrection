using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaLibrary;
using HouseholdLibrary;
using Calibration;

namespace HouseholdManager
{
    public partial class QuickCalibrate : Form
    {
        private Dictionary<MediaVehicle.MediaType, Dictionary<string, List<SimpleOption>>> options;

        private Modifier modifier;

        private Dictionary<string, double> curr_values;

        public QuickCalibrate(List<MediaVehicle> media)
        {
            InitializeComponent();

            modifier = new Modifier(media);

            curr_values = new Dictionary<string, double>();

            options = new Dictionary<MediaVehicle.MediaType,Dictionary<string,List<SimpleOption>>>();

            foreach (MediaVehicle vehicle in media)
            {
                if (!options.ContainsKey(vehicle.Type))
                {
                    options.Add(vehicle.Type, new Dictionary<string, List<SimpleOption>>());
                    TypeCombo.Items.Add(vehicle.Type);
                }
                foreach (AdOption option in vehicle.GetOptions().Values)
                {
                    SimpleOption simp_option = option as SimpleOption;
                    if (simp_option == null)
                    {
                        continue;
                    }
                    if (!options[vehicle.Type].ContainsKey(simp_option.ID))
                    {
                        options[vehicle.Type].Add(simp_option.ID, new List<SimpleOption>());
                    }
                    options[vehicle.Type][simp_option.ID].Add(simp_option);
                }
            }

            ValueCombo.Items.Add(Modifier.VariableType.Persuasion);
            ValueCombo.Items.Add(Modifier.VariableType.Awareness);
            ValueCombo.Items.Add(Modifier.VariableType.Recency);
            ValueCombo.Items.Add(Modifier.VariableType.Prob_Per_Hour);

            
            ValueCombo.SelectedIndex = 0;
            OperatorCombo.SelectedIndex = 0;
            ModifierUpDown.Value = new Decimal(1.00000);
            TypeCombo.SelectedIndex = 0;
        }

        private void TypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill_list();
        }

        private void ValueCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeCombo.SelectedIndex < 0)
            {
                return;
            }
            fill_list();
        }

        private void OperatorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeCombo.SelectedIndex < 0)
            {
                return;
            }
            revalue();
        }

        private void ModifierUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (TypeCombo.SelectedIndex < 0)
            {
                return;
            }
            revalue();
        }

        private void fill_list()
        {
            MediaVehicle.MediaType media_type = (MediaVehicle.MediaType)TypeCombo.SelectedItem;
            OptionsList.Items.Clear();
            curr_values.Clear();
            foreach (string ID in options[media_type].Keys)
            {
                string name = options[media_type][ID][0].Name.Trim();
                string id = options[media_type][ID][0].ID.Trim();
                double curr_value = 0.0;
                for (int i = 0; i < options[media_type][ID].Count; i++)
                {
                    curr_value += get_value(options[media_type][ID][i]);
                }
                curr_value = curr_value / options[media_type][ID].Count;
                string value = curr_value.ToString();
                string opterator = "x";
                double mod = 1.0;
                string modifier = mod.ToString();
                string new_value = curr_value.ToString();
                curr_values.Add(ID.Trim(), curr_value);
                ListViewItem item = new ListViewItem(new string[] { name, id, value, opterator, modifier, new_value });
                OptionsList.Items.Add(item);
            }

            revalue();
        }

        private void revalue()
        {
            string opterator = OperatorCombo.SelectedItem.ToString();
            string modifier = ModifierUpDown.Value.ToString();
            foreach (ListViewItem item in OptionsList.Items)
            {
                string ID = item.SubItems[1].Text;
                item.SubItems[3].Text = opterator;
                item.SubItems[4].Text = modifier;
                item.SubItems[5].Text = get_new_value(ID).ToString();
            }
        }

        private double get_new_value(string ID)
        {
            double modifier = (double)ModifierUpDown.Value;
            switch (OperatorCombo.SelectedItem.ToString())
            {
                case "x":
                    return curr_values[ID] * modifier;
                case "+":
                    return curr_values[ID] + modifier;
                case "=":
                    return modifier;
            }

            return 0.0;
        }

        private double get_value(SimpleOption option)
        {
            switch (ValueCombo.SelectedItem.ToString())
            {
                case "Persuasion":
                    return option.Persuasion;
                case "Awareness":
                    return option.Awareness;
                case "Recency":
                    return option.Recency;
                case "Prob_Per_Hour":
                    return option.Prob_Per_Hour;
            }

            return 0.0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> option_ids = new List<string>();
            foreach (ListViewItem item in OptionsList.Items)
            {
                if (item.Checked)
                {
                    option_ids.Add(item.SubItems[1].Text);
                }
            }

            Modifier.ModifyStyle style = get_operator();
            Modifier.VariableType variable = get_value_type();
            string type = TypeCombo.SelectedItem.ToString();
            double amount = (double)ModifierUpDown.Value;

            foreach (string ID in option_ids)
            {
                modifier.Modify(type, "ALL", "ALL", "ALL", ID, variable, style, amount);
            }

            fill_list();
        }

        private Modifier.ModifyStyle get_operator()
        {
            switch (OperatorCombo.SelectedItem.ToString())
            {
                case "x":
                    return Modifier.ModifyStyle.Scalar;
                case "+":
                    return Modifier.ModifyStyle.Incremental;
                case "=":
                    return Modifier.ModifyStyle.Absolute;
            }

            return Modifier.ModifyStyle.Scalar;
        }

        private Modifier.VariableType get_value_type()
        {
            switch (ValueCombo.SelectedItem.ToString())
            {
                case "Persuasion":
                    return Modifier.VariableType.Persuasion;
                case "Awareness":
                    return Modifier.VariableType.Awareness;
                case "Recency":
                    return Modifier.VariableType.Recency;
                case "Prob_Per_Hour":
                    return Modifier.VariableType.Prob_Per_Hour;
            }

            return Modifier.VariableType.Persuasion;
        }


    }
}
