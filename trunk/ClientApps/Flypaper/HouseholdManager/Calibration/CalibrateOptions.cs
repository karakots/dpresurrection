using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MediaLibrary;

namespace Calibration
{
    public partial class CalibrateOptions : Form
    {
        private Dictionary<MediaVehicle.MediaType, Dictionary<int, SimpleOption>> type_options;
        private Dictionary<int, AdOption> ad_options;

        private OptionModifier modifier;

        private Dictionary<int, double> curr_values;

        string fileName = null;
        Dictionary<Guid, MediaVehicle> mediaDict;

        public CalibrateOptions(Dictionary<Guid, MediaVehicle> vcls, string vclFile)
        {
            InitializeComponent();

            fileName = vclFile;
            mediaDict = vcls;

            refresh();

            curr_values = new Dictionary<int, double>();


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

        private void refresh()
        {
            ad_options = new Dictionary<int, AdOption>();
            type_options = new Dictionary<MediaVehicle.MediaType, Dictionary<int, SimpleOption>>();

            foreach( MediaVehicle vcl in mediaDict.Values )
            {
                MediaVehicle.MediaType mediaType = vcl.Type;

                if( !TypeCombo.Items.Contains( mediaType ) )
                {
                    TypeCombo.Items.Add( mediaType );
                }

                if( !type_options.ContainsKey( mediaType ) )
                {
                    type_options.Add( mediaType, new Dictionary<int, SimpleOption>() );
                }

                Dictionary<int, AdOption> vcl_ad_options = vcl.GetOptions();



                foreach( int id in vcl_ad_options.Keys )
                {
                    if( !ad_options.ContainsKey( id ) )
                    {
                        ad_options.Add( id, vcl_ad_options[id] );
                    }

                    if( !type_options[mediaType].ContainsKey( id ) )
                    {
                        type_options[mediaType].Add( id, vcl_ad_options[id] as SimpleOption );
                    }
                }


            }

            modifier = new OptionModifier( ad_options );
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
            MediaVehicle.MediaType mediaType = (MediaVehicle.MediaType)TypeCombo.SelectedItem;
            OptionsList.Items.Clear();
            curr_values.Clear();
            foreach( int ID in type_options[mediaType].Keys )
            {
                string name = type_options[mediaType][ID].Name.Trim();
                int id = type_options[mediaType][ID].ID;
                double curr_value = get_value( type_options[mediaType][ID] );
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
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            FileStream file = new FileStream( fileName, FileMode.Open );

            Dictionary<Guid, MediaVehicle> tmpDict = (Dictionary<Guid, MediaVehicle>)serializer.Deserialize( file );
            file.Close();

            // Update ad options on vehicles

            foreach(Guid id  in tmpDict.Keys )
            {
                MediaVehicle vcl = mediaDict[id];
                MediaVehicle tmpVcl = tmpDict[id];

                Dictionary<int, AdOption> options = vcl.GetOptions();
                Dictionary<int, AdOption> tmpOptions = tmpVcl.GetOptions();

                foreach( int optId in tmpOptions.Keys )
                {
                    options[optId] = tmpOptions[optId];
                }
            }

            refresh();

            fill_list();

        }

        private void applyBut_Click( object sender, EventArgs e )
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            FileStream file = new FileStream( fileName, FileMode.Create );

            serializer.Serialize( file, mediaDict );
            file.Close();
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

                foreach( MediaVehicle vcl in mediaDict.Values )
                {
                    Dictionary<int, AdOption> options = vcl.GetOptions();

                    if( options.ContainsKey( ID ) )
                    {
                        options[ID] = ad_options[ID];
                    }
                }
            }

            fill_list();
        }
    }
}
