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

namespace AdPlanitSimController
{
    public partial class QuickCalibrate : Form
    {
        private Dictionary<int, Dictionary<int, SimpleOption>> type_options;

        private OptionModifier modifier;

        private Dictionary<int, double> curr_values;

        DpMediaDb db;

        public QuickCalibrate(DpMediaDb database)
        {
            InitializeComponent();

            db = database;

            modifier = new OptionModifier(database);

            curr_values = new Dictionary<int, double>();

            type_options = new Dictionary<int, Dictionary<int, SimpleOption>>();

            List<string> media_types = database.GetMediaTypes();


            foreach (string type in media_types)
            {
                TypeCombo.Items.Add(type);
                int type_id = database.GetTypeID(type);
                type_options.Add(type_id, new Dictionary<int, SimpleOption>());
                Dictionary<int, AdOption> options = database.GetTypeOptions(type_id);
                foreach (int option_id in options.Keys)
                {
                    SimpleOption simp_option = options[option_id] as SimpleOption;
                    if (simp_option != null)
                    {
                        type_options[type_id].Add(option_id, simp_option);
                    }
                }
            }

            ValueCombo.Items.Add(OptionModifier.VariableType.Persuasion);
            ValueCombo.Items.Add(OptionModifier.VariableType.Awareness);
            ValueCombo.Items.Add(OptionModifier.VariableType.Recency);
            ValueCombo.Items.Add(OptionModifier.VariableType.Prob_Per_Hour);
            ValueCombo.Items.Add(OptionModifier.VariableType.Cost_Modifier);
            ValueCombo.Items.Add(OptionModifier.VariableType.Consideration_Prob_Scalar);
            ValueCombo.Items.Add(OptionModifier.VariableType.Consideration_Persuasion_Scalar);
            ValueCombo.Items.Add(OptionModifier.VariableType.Consideration_Awareness_Scalar);


            ModifierUpDown.Value = new Decimal( 1.00000 );

            try
            {
                ValueCombo.SelectedIndex = 0;
                OperatorCombo.SelectedIndex = 0;
                TypeCombo.SelectedIndex = 0;
            }
            catch( Exception ) { }
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
            string media_type = (string)TypeCombo.SelectedItem;
            int type_id = db.GetTypeID(media_type);
            OptionsList.Items.Clear();
            curr_values.Clear();
            foreach (int ID in type_options[type_id].Keys)
            {
                string name = type_options[type_id][ID].Name.Trim();
                int id = type_options[type_id][ID].ID;
                double curr_value = get_value(type_options[type_id][ID]);
                string value = curr_value.ToString();
                string opterator = "x";
                double mod = 1.0;
                string modifier = mod.ToString();
                string new_value = curr_value.ToString();
                curr_values.Add(ID, curr_value);
                ListViewItem item = new ListViewItem(new string[] { name, ID.ToString(), value, opterator, modifier, new_value });
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
                int ID = Int32.Parse(item.SubItems[1].Text);
                item.SubItems[3].Text = opterator;
                item.SubItems[4].Text = modifier;
                item.SubItems[5].Text = get_new_value(ID).ToString();
            }
        }

        private double get_new_value(int ID)
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
                case "Cost_Modifier":
                    return option.Cost_Modifier;
                case "Consideration_Prob_Scalar":
                    return option.ConsiderationProbScalar;
                case "Consideration_Persuasion_Scalar":
                    return option.ConsiderationPersuasionScalar;
                case "Consideration_Awareness_Scalar":
                    return option.ConsiderationAwarenessScalar;
            }

            return 0.0;
        }

        private ModifyStyle get_operator()
        {
            switch (OperatorCombo.SelectedItem.ToString())
            {
                case "x":
                    return ModifyStyle.Scalar;
                case "+":
                    return ModifyStyle.Incremental;
                case "=":
                    return ModifyStyle.Absolute;
            }

            return ModifyStyle.Scalar;
        }

        private OptionModifier.VariableType get_value_type()
        {
            switch (ValueCombo.SelectedItem.ToString())
            {
                case "Persuasion":
                    return OptionModifier.VariableType.Persuasion;
                case "Awareness":
                    return OptionModifier.VariableType.Awareness;
                case "Recency":
                    return OptionModifier.VariableType.Recency;
                case "Prob_Per_Hour":
                    return OptionModifier.VariableType.Prob_Per_Hour;
                case "Cost_Modifier":
                    return OptionModifier.VariableType.Cost_Modifier;
                case "Consideration_Prob_Scalar":
                    return OptionModifier.VariableType.Consideration_Prob_Scalar;
                case "Consideration_Persuasion_Scalar":
                    return OptionModifier.VariableType.Consideration_Persuasion_Scalar;
                case "Consideration_Awareness_Scalar":
                    return OptionModifier.VariableType.Consideration_Awareness_Scalar;
            }

            return OptionModifier.VariableType.Persuasion;
        }

        private void cancel_button_Click( object sender, EventArgs e )
        {
            db.RefreshWebData();
            this.Close();
        }

        private void applyBut_Click( object sender, EventArgs e )
        {
            foreach (int type_id in type_options.Keys)
            {
                foreach (int option_id in type_options[type_id].Keys)
                {
                    db.UpdateOption(option_id, type_options[type_id][option_id]);
                }
            }
            db.Update();
            MessageBox.Show( "Howdy Doody" );
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            List<int> option_ids = new List<int>();
            foreach (ListViewItem item in OptionsList.Items)
            {
                if (item.Checked)
                {
                    option_ids.Add(Int32.Parse(item.SubItems[1].Text));
                }
            }

            ModifyStyle style = get_operator();
            OptionModifier.VariableType variable = get_value_type();
            string type = TypeCombo.SelectedItem.ToString();
            double amount = (double)ModifierUpDown.Value;

            foreach (int ID in option_ids)
            {
                modifier.Modify(ID, variable, style, amount);
            }

            fill_list();

        }


    }
}
