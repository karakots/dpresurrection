using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebLibrary
{
    /// <summary>
    /// This class corresponds to an MediaRule node in the XML
    /// </summary>
    public class MediaRule
    {
        public string RuleName;
        public string RuleID;

        public string TypeName;
        public double Weight;

        public List<MediaRuleGroup> Groups;
        public List<MediaLimitRule> Limits;

        public List<MediaRuleListColumnDefinition> ListColumns;

        public MediaRule( string ruleName, string mediaType, double weight ) {
            this.RuleName = ruleName;
            this.TypeName = mediaType;
            this.Weight = weight;
            this.Groups = new List<MediaRuleGroup>();
            this.Limits = new List<MediaLimitRule>();
        }
    }

    /// <summary>
    /// This class corresponds to a Group node in the XML
    /// </summary>
    public class MediaRuleGroup
    {
        public string GroupName;
        public double Weight;
        public List<MediaRuleGroupValue> Values;

        public MediaRuleGroup( string groupName, double weight ) {
            this.GroupName = groupName;
            this.Weight = weight;
            this.Values = new List<MediaRuleGroupValue>();
        }
    }

    /// <summary>
    /// This class corresponds to an ExpectedValue node in the XML
    /// </summary>
    public class MediaRuleGroupValue
    {
        public string ValueName;
        public string Reason;
        public string ReasonColor;
        public double Weight;
        public List<double> WeightList;

        private double minValue = Double.MinValue;
        private double maxValue = Double.MaxValue;

        public double MinValue {
            get { return minValue; }
        }

        public double MaxValue {
            get { return maxValue; }
        }

        public bool IsRange() {
            return (this.ValueName == null);
        }

        /// <summary>
        /// Create an object representing an ExpectedValue node with min and max attributes (no name)
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="weight"></param>
        public MediaRuleGroupValue( double minValue, double maxValue, double weight, string reason ) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.ValueName = null;
            this.Reason = reason;
            this.ReasonColor = "";
            this.Weight = weight;
        }

        /// <summary>
        /// Create an object representing an ExpectedValue node with a name attribute (no min/max)
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="weight"></param>
        public MediaRuleGroupValue( string valueName, double weight, string reason ) {
            this.ValueName = valueName;
            this.Reason = reason;
            this.ReasonColor = "";
            this.Weight = weight;
        }

        /// <summary>
        /// Create an object representing an ExpectedValue node with a list of values
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="weight"></param>
        public MediaRuleGroupValue( string valueName, List<double> weights ) {
            this.ValueName = valueName;
            this.WeightList = weights;
            this.ReasonColor = "";
        }
    }

    /// <summary>
    /// This class corresponds to a Limit node in the XML
    /// </summary>
    public class MediaLimitRule
    {
        public string ValueName;
        public string LimitType;
        public double LimitValue;

        public MediaLimitRule( string valueName, string limitType, double limitValue ) {
            this.ValueName = valueName;
            this.LimitType = limitType;
            this.LimitValue = limitValue;
        }
    }

    /// <summary>
    /// This class corresponds to a WeightDefinition node in the XML
    /// </summary>
    public class MediaRuleWeightValueDefinition
    {
        public string Text;
        public double Value;

        public string ReasonColor;

        public MediaRuleWeightValueDefinition( string text, double val, string reasonColor ) {
            this.Text = text;
            this.Value = val;
            this.ReasonColor = reasonColor;
        }
    }

    public class MediaRuleListColumnDefinition
    {
        public string ColumnName;
        public double MinValue;
        public double Maxvalue;

        public MediaRuleListColumnDefinition( string name, double min, double max ) {
            this.ColumnName = name;
            this.MinValue = min;
            this.Maxvalue = max;
        }
    }

    /// <summary>
    /// Describes a component of the media rules results, with a description and a the value it contributed
    /// </summary>
    [Serializable]
    public class MediaRuleReason
    {
        public string Text;
        public string TextColor;
        public double Score;

        public MediaRuleReason() {
            this.Text = "";
            this.TextColor = "";
            this.Score = 0;
        }

        public MediaRuleReason( string text, string textColor, double score ) {
            this.Text = text;
            this.TextColor = textColor;
            this.Score = score;
        }
    }
}
