using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaLibrary;

namespace Calibration
{

    public class OptionModifier
    {
        public enum VariableType
        {
            Prob_Per_Hour,
            Persuasion,
            Awareness,
            Recency,
            Cost_Modifier,
            Consideration_Prob_Scalar,
            Consideration_Persuasion_Scalar,
            Consideration_Awareness_Scalar
        }



        private Dictionary<int, AdOption> ad_options = null;



        public OptionModifier( Dictionary<int, AdOption> ad_optionIn)
        {
            ad_options = ad_optionIn;
        }

        public void Modify(int ad_option, VariableType var_type, ModifyStyle mod_style, double amount)
        {
            AdOption option = ad_options[ad_option];
            modify(option, var_type, mod_style, amount);
        }
        

        private void modify(AdOption option, VariableType var_type, ModifyStyle mod_style, double amount)
        {
            SimpleOption simp_option = option as SimpleOption;
            if (simp_option == null)
            {
                return;
            }

            Modifier.value_mod mod = Modifier.get_functor(mod_style);

            switch (var_type)
            {
                case VariableType.Prob_Per_Hour:
                    simp_option.Prob_Per_Hour = mod(simp_option.Prob_Per_Hour, amount);
                    break;
                case VariableType.Persuasion:
                    simp_option.Persuasion = mod(simp_option.Persuasion, amount);
                    break;
                case VariableType.Awareness:
                    simp_option.Awareness = mod(simp_option.Awareness, amount);
                    break;
                case VariableType.Recency:
                    simp_option.Recency = mod(simp_option.Recency, amount);
                    break;
                case VariableType.Cost_Modifier:
                    simp_option.Cost_Modifier = mod(simp_option.Cost_Modifier, amount);
                    break;
                case VariableType.Consideration_Prob_Scalar:
                    simp_option.ConsiderationProbScalar = mod(simp_option.ConsiderationProbScalar, amount);
                    break;
                case VariableType.Consideration_Persuasion_Scalar:
                    simp_option.ConsiderationPersuasionScalar = mod(simp_option.ConsiderationPersuasionScalar, amount);
                    break;
                case VariableType.Consideration_Awareness_Scalar:
                    simp_option.ConsiderationAwarenessScalar = mod(simp_option.ConsiderationAwarenessScalar, amount);
                    break;
            }
        }
    }
}
