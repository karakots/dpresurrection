using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calibration
{
    public enum ModifyStyle
    {
        Absolute,
        Incremental,
        Scalar
    }

    class Modifier
    {
        public delegate double value_mod(double value, double amount);

        public static value_mod get_functor(ModifyStyle mod_style)
        {
            switch (mod_style)
            {
                case ModifyStyle.Absolute:
                    return new value_mod(absolute);
                case ModifyStyle.Incremental:
                    return new value_mod(increment);
                case ModifyStyle.Scalar:
                    return new value_mod(scale);
            }

            return new value_mod(scale);
        }

        public static double absolute(double value, double amount)
        {
            return amount;
        }

        public static double increment(double value, double amount)
        {
            return value + amount;
        }

        public static double scale(double value, double amount)
        {
            return value * amount;
        }
    }
}
