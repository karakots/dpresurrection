using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;
using System.Xml;

using WebLibrary;

namespace BusinessLogic
{
    /// <summary>
    /// Summary description for AllocationRulesReader
    /// </summary>
    public class AllocationRulesReader
    {
        public AllocationRulesReader() {
            //
            // TODO: Add constructor logic here
            //
        }

        public static List<MediaRule> GetAllocationRules( string file ) {

            FileStream rulesStream = new FileStream( file, FileMode.Open, FileAccess.Read );

            // read the media rules from the file
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlTextReader reader = new XmlTextReader( rulesStream );

            List<MediaRule> mediaRules = new List<MediaRule>();
            List<MediaRuleWeightValueDefinition> weightDefnitions = new List<MediaRuleWeightValueDefinition>();       // the numeric values corresponding to H, M, L, etc.
            MediaRule curRule = null;
            MediaRuleGroup curGroup = null;

            List<MediaRuleListColumnDefinition> listColDefnitions = new List<MediaRuleListColumnDefinition>();            // defines columns in ExpectedValueList items
            string listColumnName = null;

            while( reader.Read() ) {
                if( reader.NodeType == XmlNodeType.Element ) {

                    switch( reader.Name ) {

                        case "MediaRule":
                            string type = reader.GetAttribute( "type" );
                            double weight = 1.0;
                            if( reader.GetAttribute( "weight" ) != null ) {
                                weight = Convert.ToDouble( reader.GetAttribute( "weight" ) );
                            }

                            curRule = new MediaRule( reader.GetAttribute( "name" ), type, weight );

                            if( reader.GetAttribute( "rule" ) != null ) {
                                curRule.RuleID = reader.GetAttribute( "rule" );
                            }
                            curRule.ListColumns = listColDefnitions;

                            mediaRules.Add( curRule );
                            break;

                        case "WeightDefinition":
                            string text = reader.GetAttribute( "string" );
                            double val = -1.0;
                            if( reader.GetAttribute( "value" ) != null ) {
                                val = Convert.ToDouble( reader.GetAttribute( "value" ) );
                            }
                            string colorStr = "";
                            if( reader.GetAttribute( "reasonColor" ) != null ) {
                                colorStr = reader.GetAttribute( "reasonColor" );
                            }
                            MediaRuleWeightValueDefinition weightDef = new MediaRuleWeightValueDefinition( text, val, colorStr );
                            weightDefnitions.Add( weightDef );
                            break;

                        case "ListColumnDefs":
                            listColumnName = reader.GetAttribute( "valueName" );
                            break;

                        case "ListColumnDef":
                            string colName = reader.GetAttribute( "name" );
                            double colMin = Convert.ToDouble( reader.GetAttribute( "min" ) );
                            double colMax = Convert.ToDouble( reader.GetAttribute( "max" ) );
                            MediaRuleListColumnDefinition cdef = new MediaRuleListColumnDefinition( colName, colMin, colMax );
                            listColDefnitions.Add( cdef );
                            break;

                        case "Group":
                            double groupWeight = 1.0;
                            if( reader.GetAttribute( "weight" ) != null ) {
                                groupWeight = Convert.ToDouble( reader.GetAttribute( "weight" ) );
                            }

                            curGroup = new MediaRuleGroup( reader.GetAttribute( "name" ), groupWeight );
                            curRule.Groups.Add( curGroup );
                            break;

                        case "Limit":
                            double limitValue = 0.0;
                            if( reader.GetAttribute( "value" ) != null ) {
                                limitValue = Convert.ToDouble( reader.GetAttribute( "value" ) );
                            }

                            MediaLimitRule limit = new MediaLimitRule( reader.GetAttribute( "name" ), reader.GetAttribute( "type" ), limitValue );
                            curRule.Limits.Add( limit );
                            break;

                        case "ExpectedValue":
                            double expectedValueWeight = 1.0;
                            double min = Double.MinValue;
                            double max = Double.MaxValue;
                            string reason = "";
                            string reasonColor = "";

                            if( reader.GetAttribute( "weight" ) != null ) {
                                string weightStr = reader.GetAttribute( "weight" );
                                if( weightStr.Length > 0 && Char.IsDigit( weightStr[ 0 ] ) ) {
                                    // the weight is a number already
                                    expectedValueWeight = Convert.ToDouble( weightStr );
                                }
                                else {
                                    // the weight is a string -- translate from the weight definitions
                                    bool isSet = false;
                                    foreach( MediaRuleWeightValueDefinition wdef in weightDefnitions ) {
                                        if( weightStr == wdef.Text ) {
                                            expectedValueWeight = wdef.Value;
                                            reasonColor = wdef.ReasonColor;
                                            isSet = true;
                                        }
                                    }
                                    if( isSet == false ) {
                                        throw new Exception( String.Format( "Error: Weight value \"{0}\" for ExpectedValue node not found in any WeightDefinition node.", weightStr ) );
                                    }
                                }
                            }

                            if( reader.GetAttribute( "min" ) != null ) {
                                min = Convert.ToDouble( reader.GetAttribute( "min" ) );
                            }
                            if( reader.GetAttribute( "max" ) != null ) {
                                max = Convert.ToDouble( reader.GetAttribute( "max" ) );
                            }
                            if( reader.GetAttribute( "reason" ) != null ) {
                                reason = reader.GetAttribute( "reason" );
                            }

                            MediaRuleGroupValue expValue = null;
                            if( reader.GetAttribute( "value" ) != null ) {
                                // a single (string) value expected
                                expValue = new MediaRuleGroupValue( reader.GetAttribute( "value" ), expectedValueWeight, reason );
                            }
                            else {
                                // a range of (numeric) values expected
                                expValue = new MediaRuleGroupValue( min, max, expectedValueWeight, reason );
                            }
                            if( reader.GetAttribute( "reasonColor" ) != null ) {
                                expValue.ReasonColor = reader.GetAttribute( "reasonColor" );
                            }
                            expValue.ReasonColor = reasonColor;
                            curGroup.Values.Add( expValue );
                            break;

                        case "ExpectedValueList":
                            string vals = reader.GetAttribute( "weights" );
                            string gName = reader.GetAttribute( "name" );

                            string[] valItems = vals.Split( ',' );
                            List<double> values = new List<double>();
                            char[] trimChars = new char[] { ' ', '\t' };
                            for( int i2 = 0; i2 < valItems.Length; i2++ ) {
                                values.Add( Convert.ToDouble( valItems[ i2 ].Trim( trimChars ) ) );
                            }
                            MediaRuleGroupValue expValue2 = new MediaRuleGroupValue( gName, values );
                            curGroup.Values.Add( expValue2 );
                            break;

                        default:
                            break;
                    }
                }
            }

            rulesStream.Close();

            return mediaRules;
        }
    }
}
