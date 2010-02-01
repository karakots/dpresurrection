#define USE_VEHICLES
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

using WebLibrary;

namespace BusinessLogic
{
    /// <summary>
    /// Summary description for MediaRuleEngine
    /// </summary>
//    public class MediaRuleEngine
//    {
//        #region General Media Rules
//        /// <summary>
//        /// Applies the given media rule to each of the items in mediaItems, returning the scores in mediaItems and accumulating the overall scores in overallItems.
//        /// </summary>
//        /// <param name="mediaItems"></param>
//        /// <param name="overallItems"></param>
//        /// <param name="mediaRule"></param>
//        /// <param name="userSpecs"></param>
//        /// <param name="segment"></param>
//        public static void ApplyMediaRule( MediaRule mediaRule, MediaCampaignSpecs userSpecs, int segment,
//            Dictionary<string, MediaItemRanking> mediaItems, Dictionary<string, MediaItemRanking> overallItems ) {

//            if( mediaRule.RuleID == "SUM-MAX" ) {

//                double overallScore = 0.0;
//                List<MediaRuleReason> reasonsList = new List<MediaRuleReason>();

//                if( mediaRule.Groups.Count > 0 ) {
//                    foreach( MediaRuleGroup group in mediaRule.Groups ) {

//                        double groupScore = 0;
//                        MediaRuleReason groupReason = null;

//                        foreach( MediaRuleGroupValue gVal in group.Values ) {
//                            bool specHasValueSelected = false;

//                            if( gVal.IsRange() == false ) {
//                                specHasValueSelected = userSpecs.GetValue( segment, group.GroupName, gVal.ValueName );
//                            }
//                            else {
//                                specHasValueSelected = userSpecs.GetValue( segment, group.GroupName, gVal.MinValue, gVal.MaxValue );
//                            }

//                            if( specHasValueSelected ) {

//                                // take the maximum value we hit across the items in the group
//                                if( gVal.Weight >= groupScore ) {
//                                    groupScore = gVal.Weight;
//                                    groupReason = new MediaRuleReason( gVal.Reason, gVal.ReasonColor, gVal.Weight );
//                                }
//                            }
//                        }
//                        if( groupScore * group.Weight > 0 ) {
//                            reasonsList.Add( groupReason );
//                        }
//                        overallScore += groupScore * group.Weight;        // sum the group scores together
//                    }

//                    // determine the final score contribution of this rule
//                    double ruleScore = overallScore * mediaRule.Weight;

//                    // add this rule's score to the overall score for the appropriate media type
//                    if( ruleScore != 0 ) {
//#if USE_VEHICLES
//                        if( mediaRule.TypeName == "RADIO" ) {
//                            mediaRule.TypeName = "Radio";
//                        }
//                        if( mediaRule.TypeName == "MAGAZINES" ) {
//                            mediaRule.TypeName = "Magazine";
//                        }
//#else
//#endif
//                        if( mediaItems.ContainsKey( mediaRule.TypeName ) == false ) {
//                            throw new Exception( String.Format( "Error: Unable to find media item named \"{0}\" in media items list.", mediaRule.TypeName ) );
//                        }

//                        if( mediaItems[ mediaRule.TypeName ] != null ) {

//                            mediaItems[ mediaRule.TypeName ].Score += ruleScore;
//                            if( overallItems != null ) {
//                                overallItems[ mediaRule.TypeName ].Score += ruleScore;
//                            }

//                            foreach( MediaRuleReason reasonItem in reasonsList ) {
//                                AddReason( mediaItems[ mediaRule.TypeName ].Reasons, reasonItem.Text, reasonItem.Score, reasonItem.TextColor );
//                                if( overallItems != null ) {
//                                    AddReason( overallItems[ mediaRule.TypeName ].Reasons, reasonItem.Text, reasonItem.Score, reasonItem.TextColor );
//                                }
//                            }
//                        }
//                    }
//                }
//                else {
//                    throw new Exception( String.Format( "Error: Unknown Media Rule \"{0}\"", mediaRule.RuleID ) );
//                }
//            }
//        }
//        #endregion

//        /// <summary>
//        /// Computes the reach and efficiency for each radio subtype, for the given rule group and demographic segment number
//        /// </summary>
//        /// <param name="mediaRule"></param>
//        /// <param name="groupName"></param>
//        /// <param name="userSpecs"></param>
//        /// <param name="segment"></param>
//        /// <param name="mediaItems"></param>
//        public static void ApplyRadioRule( MediaRule mediaRule, string groupName, MediaCampaignSpecs userSpecs, int segment, Dictionary<string, ReachAndEfficiency> mediaItems ) {
//            if( mediaRule.RuleID == "CORR-SUM" ) {
//                if( mediaRule.Groups.Count > 0 ) {
//                    foreach( MediaRuleGroup group in mediaRule.Groups ) {
//                        if( group.GroupName != groupName ) {
//                            continue;
//                        }

//                        foreach( MediaRuleGroupValue gVal in group.Values ) {
//                            double groupCorrTotal = 0;
//                            double groupTotal = 0;

//                            for( int c = 0; c < mediaRule.ListColumns.Count; c++ ) {
//                                double colMin = mediaRule.ListColumns[ c ].MinValue;
//                                double colMax = mediaRule.ListColumns[ c ].Maxvalue;
//                                // see if the user has selected this range -- return value is 1 if the user has selected 100% of the given range
//                                double specHasValueSelected = userSpecs.GetValueCorrelationValue( segment, "AgeValue", colMin, colMax );
//                                double colFrac = specHasValueSelected * gVal.WeightList[ c ];
//                                groupCorrTotal += colFrac;
//                                groupTotal += gVal.WeightList[ c ];

//                            }
//                            mediaItems[ gVal.ValueName ].ItemGroup = groupName;
//                            mediaItems[ gVal.ValueName ].Reach = groupCorrTotal;
//                            if( groupTotal != 0 ) {
//                                mediaItems[ gVal.ValueName ].Efficiency = groupCorrTotal / groupTotal;
//                            }
//                            else {
//                                mediaItems[ gVal.ValueName ].Efficiency = 0;
//                            }
//                        }
//                    }
//                }
//            }
//            else {
//                throw new Exception( String.Format( "Error: Unknown Media Rule \"{0}\"", mediaRule.RuleID ) );
//            }
//        }

//        /// <summary>
//        /// Adds the given reason to the reasons list, adding the score if an existing reason is being re-added
//        /// </summary>
//        /// <param name="reasonsList"></param>
//        /// <param name="text"></param>
//        /// <param name="score"></param>
//        private static void AddReason( List<MediaRuleReason> reasonsList, string text, double score, string textColor ) {
//            MediaRuleReason r = null;
//            foreach( MediaRuleReason tst in reasonsList ) {
//                if( tst.Text == text ) {
//                    r = tst;
//                    break;
//                }
//            }
//            if( r == null ) {
//                r = new MediaRuleReason( text, textColor, 0 );
//                reasonsList.Add( r );
//            }
//            r.Score += score;
//        }
//    }
}
