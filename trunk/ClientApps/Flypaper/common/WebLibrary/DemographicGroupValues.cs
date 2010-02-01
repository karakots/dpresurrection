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
    /// An object of this class represents all of the demographic checkboxes in the UI, as well as the targeting slider value
    /// </summary>
    [Serializable]
    public class DemographicSettingsAndTargeting
    {
        public DemographicSettings DemoSettings { set; get; }

        public int TargetingLevel = -1;

        public DemographicSettingsAndTargeting( DemographicSettings demoSettings, int targetingLevel ) {
            this.DemoSettings = demoSettings;
            this.TargetingLevel = targetingLevel;
        }
    }

    /// <summary>
    /// An object of this class represents all of the demographic checkboxes in the UI.
    /// </summary>
    [Serializable]
    public class DemographicSettings
    {
        public string DemographicName;
        public Guid DemographicID;

        public List<DemographicGroupValues> Values;

        public int ValueCount {
            get {
                if( this.Values != null ) {
                    return this.Values.Count;
                }
                else {
                    return 0;
                }
            }
        }

        public string SummaryDescription() {
            string s = "";

            foreach( DemographicGroupValues vals in this.Values ) {
                bool allTrue = true;
                bool allFalse = true;
                string subS = "";
                for( int i = 0; i < vals.ValueNames.Length; i++ ) {
                    bool v = vals.Values[ i ];
                    string nam = vals.ValueNames[ i ];
                    if( v == true ) {
                        subS += nam + ",";
                    }
                    allFalse &= !v;
                    allTrue &= v;
                }
                if( allFalse == false && allTrue == false ) {
                    if( subS.Length > 0 ) {
                        subS = subS.Substring( 0, subS.Length - 1 ) + "; ";
                        s += subS;
                    }
                }
            }

            return s;
        }

        public DemographicSettings() {
            this.DemographicName = "unknown";
            this.DemographicID = Guid.NewGuid();
            this.Values = new List<DemographicGroupValues>();
        }
    }

    /// <summary>
    /// An object of this class represents a set of checkboxes in the UI.
    /// </summary>
    [Serializable]
    public class DemographicGroupValues
    {
        private string groupName;
        private string[] valueNames;
        private bool[] values;

        /// <summary>
        /// Name of this group of checkboxes (Age, Income, etc.)
        /// </summary>
        public string GroupName {
            get { return groupName; }
            set { groupName = value; }
        }

        /// <summary>
        /// Checked state for each of the checkboxes in the group.
        /// </summary>
        public bool[] Values {
            get { return values; }
            set { values = value; }
        }

        /// <summary>
        /// Identifier (the UI string) for each of the checkboxes in the group.
        /// </summary>
        public string[] ValueNames {
            get { return valueNames; }
            set { valueNames = value; }
        }

        public DemographicGroupValues() {
            this.groupName = "unknown";
        }

        public DemographicGroupValues( string groupName ) {
            this.groupName = groupName;
        }
    }
}
