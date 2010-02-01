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
    /// MediaCampaignSpecs encapsulates the user specifications that define a campaign.
    /// </summary>
    [Serializable]
    public class MediaCampaignSpecs
    {
        #region Public Fields
        /// <summary>
        /// Name of the campaign.
        /// </summary>
        public string CampaignName { set; get; }

        /// <summary>
        /// General comments on the campaign.
        /// </summary>
        public string Comments { set; get; }

        /// <summary>
        /// Target budget ($ US) for the campaign. 
        /// </summary>
        public double TargetBudget { set; get; }

        /// <summary>
        /// Start date of the campaign.
        /// </summary>
        public DateTime StartDate { set; get; }

        /// <summary>
        /// End date of the campaign.
        /// </summary>
        public DateTime EndDate { set; get; }

        /// <summary>
        /// The demographic segments comprising the target customer population.
        /// </summary>
        public List<DemographicSettings> Demographics { set; get; }
        public List<DemographicLibrary.Demographic> SegmentList { set; get; }

        public List<string> GeoRegionNames { set; get; }

        public List<CampaignGoal> CampaignGoals { set; get; }
        public List<double> GoalWeights { set; get; }

        /// <summary>
        /// Purchase cycle code (1-16).  Values: larger val --> longer cycle
        /// </summary>
        public int PurchaseCycleCode { set; get; }

        /// <summary>
        /// The percent of time a target customer is considering buying the product.
        /// </summary>
        public int TimeInConsideration { set; get; }

        /// <summary>
        /// Business category
        /// </summary>
        public string BusinessCategory { set; get; }

        /// <summary>
        /// Business subcategory
        /// </summary>
        public string BusinessSubcategory { set; get; }       

        // thes next 3 values are set as you exit StartSearch.aspx (?)

        /// <summary>
        /// Overall spending rate in dollars per month
        /// </summary>
        public double MonthlySpendRate { set; get; }

        /// <summary>
        /// The initial share of the plan
        /// </summary>
        public Double InitialShare { get; set; }

        /// <summary>
        /// The units the share is measured in
        /// </summary>
        public string ShareUnits { get; set; }

        /// <summary>
        /// Previous media spend over the last year
        /// </summary>
        public Double PreviousMediaSpend { get; set; }

        ///  --- items for competitions ----
        
        /// <summary>
        /// Name of the competition.
        /// </summary>
        public string CompetitionName { set; get; }

        public bool IsCompetition { get { return this.CompetitionName != null; } }

        /// <summary>
        /// ID of the user who invoked this competition.
        /// </summary>
        public string CompetitionOwner { set; get; }

        /// <summary>
        /// Time this competition was opened.
        /// </summary>
        public DateTime CompetitionDate { set; get; }

        /// <summary>
        /// Name of the campaign where the competition is being held.
        /// </summary>
        public string CompetitionCampaign { set; get; }

        /// <summary>
        /// Guid of the base plan in the campaign where the competition is being held.
        /// </summary>
        public Guid? CompetitionBasePlanID { set; get; }

        /// <summary>
        /// User (other then the account owner) who submitted this plan to the system.  Null for plans that haven't been sent into a competition.
        /// </summary>
        public string Submitter { set; get; }
        #endregion

        /// <summary>
        /// Number of demographic segments in the campaign.
        /// </summary>
        public int SegmentCount {
            get {
                if( this.Demographics != null ) {
                    return this.Demographics.Count;
                }
                else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Creates a default campaign spec (starts tomorrow, lasts 3 months, $100K)
        /// </summary>
        public MediaCampaignSpecs() {
            this.CampaignName = "Unnamed Media Plan";
            this.Demographics = new List<DemographicSettings>();
            this.SegmentList = new List<DemographicLibrary.Demographic>();
            this.GeoRegionNames = new List<string>();

            this.StartDate = DateTime.Now.AddDays( 1 );
            this.EndDate = this.StartDate.AddDays( 56 );

            this.TargetBudget = 10000;
            this.MonthlySpendRate = 10000 / 2.0;     //??? is this used???

            this.CampaignGoals = new List<CampaignGoal>();
            this.GoalWeights = new List<double>();

            this.Comments = "";
            this.PurchaseCycleCode = 9;
            this.TimeInConsideration = 100;
            this.BusinessCategory = "";
            this.BusinessSubcategory = "";
        }
      
        /// <summary>
        /// Creates a new MediaCampaignSpecs object that is a copy of the given object.
        /// </summary>
        /// <param name="specsToCopy"></param>
        public MediaCampaignSpecs( MediaCampaignSpecs specsToCopy ) {
            this.CampaignName = specsToCopy.CampaignName;
            this.Demographics = specsToCopy.Demographics;
            this.SegmentList = specsToCopy.SegmentList;
            this.GeoRegionNames = specsToCopy.GeoRegionNames;
            this.PurchaseCycleCode = specsToCopy.PurchaseCycleCode;
            this.TimeInConsideration = specsToCopy.TimeInConsideration;
            this.TargetBudget = specsToCopy.TargetBudget;
            this.StartDate = specsToCopy.StartDate;
            this.EndDate = specsToCopy.EndDate;
            this.Comments = specsToCopy.Comments;
            this.MonthlySpendRate = specsToCopy.MonthlySpendRate;
            this.PreviousMediaSpend = specsToCopy.PreviousMediaSpend;
            this.ShareUnits = specsToCopy.ShareUnits;
            this.InitialShare = specsToCopy.InitialShare;
            this.BusinessCategory = specsToCopy.BusinessCategory;
            this.BusinessSubcategory = specsToCopy.BusinessSubcategory;
            this.CompetitionName = specsToCopy.CompetitionName;
            this.CompetitionDate = specsToCopy.CompetitionDate;
            this.CompetitionOwner = specsToCopy.CompetitionOwner;
            this.CompetitionCampaign = specsToCopy.CompetitionCampaign;
            this.CompetitionBasePlanID = specsToCopy.CompetitionBasePlanID;
            this.CampaignGoals = new List<CampaignGoal>();
            foreach(CampaignGoal goal in specsToCopy.CampaignGoals)
            {
                this.CampaignGoals.Add(goal);
            }
            this.GoalWeights = new List<double>();
            foreach( double w in specsToCopy.GoalWeights ) {
                this.GoalWeights.Add( w );
            }
        }

        /// <summary>
        /// Sets default values for the campagn goals
        /// </summary>
        public void SetDefaultGoals() {
            this.CampaignGoals = new List<CampaignGoal>();
            this.CampaignGoals.Add( CampaignGoal.Persuasion );
            this.CampaignGoals.Add( CampaignGoal.ReachAndAwareness );

            this.GoalWeights = new List<double>();
            this.GoalWeights.Add( 50 );
            this.GoalWeights.Add( 50 );
        }

        /// <summary>
        /// Returns a string that describes the campaign.
        /// </summary>
        public string GetCampaignSummary( string footerText ) {
            //string line1Format = "<div style=\"background-color: #88C0Da;padding-left:10px;padding-right:8px;padding-top:8px;padding-bottom:8px;\">Campaign <b>{0}</b><br>";
            //string line2Format = "&nbsp;{0}-{1}&nbsp;&nbsp;&nbsp;${2}<br>";
            //string line3Format = "&nbsp;Purchase:&nbsp;{0},&nbsp;{1}<br>";
            //string line4Format = "&nbsp;<b>{0}</b> - {1}<br>";
            //string line5Format = "<i>&nbsp;{0}</i><br>";
            string line1Format = "<div style=\"background-color: #88C0Da;padding-left:10px;padding-right:8px;padding-top:8px;padding-bottom:8px;\"><b>{0}</b><br>";
            string line2Format = " {0}-{1}  ${2}<br>";
            string line3Format = " Purchase: {0}, {1}<br>";
            string line4Format = " <b>{0}</b> - {1}<br>";
            string line5Format = "<i>{0}</i>";

            string s = String.Format( line1Format, "Campaign Specs:" );
            s += String.Format( line2Format, this.StartDate.ToShortDateString(), this.EndDate.ToShortDateString(), (int)this.TargetBudget );
            s += String.Format( line3Format, Utils.PurchaseCycleForCode( this.PurchaseCycleCode ),
                "" );
            s += " Segments: ";
            for( int i = 0; i < this.SegmentCount; i++ ) {
                DemographicSettings ds = this.Demographics[ i ];
                s += String.Format( line4Format, ds.DemographicName, "TO BE REMOVED" );
                s += String.Format( line5Format, ds.SummaryDescription() );
            }
            if( footerText != null ) {
                s += "<br>" + footerText;
            }
            s += "</div>";
            return s;
        }

        /// <summary>
        /// Returns a string that describes the campaign, formatted to fit the edit-plan page
        /// </summary>
        public string GetEditPageCampaignSummary() {
            string line1Format = "<div style=\";padding-left:1px;padding-right:8px;padding-top:1px;padding-bottom:8px;\"><b>Campaign: {0}</b>:&nbsp;&nbsp;&nbsp;[{1}]&nbsp;";
            string line2Format = "&nbsp;{0}-{1}&nbsp;&nbsp;&nbsp;${2}";
            string line3Format = "&nbsp;Purchase:&nbsp;{0},&nbsp;{1}<br>";
            string line4Format = "&nbsp;<b>{0}</b> - {1}&nbsp;";
            string line5Format = "<i>&nbsp;{0}</i>";

            string s = String.Format( line1Format, this.CampaignName.Replace( " ", "&nbsp;" ), "" );
            s += String.Format( line2Format, this.StartDate.ToShortDateString(), this.EndDate.ToShortDateString(), (int)this.TargetBudget );
            s += String.Format( line3Format, Utils.PurchaseCycleForCode( this.PurchaseCycleCode ),
                "" );
            for( int i = 0; i < this.SegmentCount; i++ ) {
                DemographicSettings ds = this.Demographics[ i ];
                s += String.Format( line4Format, ds.DemographicName, "TO BE REMOVED" );
                s += String.Format( line5Format, ds.SummaryDescription() );
            }
            s += "</div>";
            return s;
        }

        /// <summary>
        /// Returns a string that describes the campaign, formatted to fit a row of the saved-plans list.
        /// </summary>
        public string GetWideCampaignSummary( string editJavascript ) {
            string line1Format = "<div style=\";padding-left:1px;padding-right:8px;padding-top:1px;padding-bottom:8px;\"><b>Campaign: <a href=\"#\" onclick='{2}' >{0}</a></b>:&nbsp;&nbsp;&nbsp;[{1}]&nbsp;";
            string line2Format = "&nbsp;{0}-{1}&nbsp;&nbsp;&nbsp;${2}";
            string line3Format = "&nbsp;Purchase:&nbsp;{0},&nbsp;{1}<br>";
            string line4Format = "&nbsp;<b>{0}</b> - {1}&nbsp;";
            string line5Format = "<i>&nbsp;{0}</i>";

            string s = String.Format( line1Format, this.CampaignName, "", editJavascript );
            s += String.Format( line2Format, this.StartDate.ToShortDateString(), this.EndDate.ToShortDateString(), (int)this.TargetBudget );
            s += String.Format( line3Format, Utils.PurchaseCycleForCode( this.PurchaseCycleCode ),
                "" );
            for( int i = 0; i < this.SegmentCount; i++ ) {
                DemographicSettings ds = this.Demographics[ i ];
                s += String.Format( line4Format, ds.DemographicName, "TO BE REMOVED" );
                s += String.Format( line5Format, ds.SummaryDescription() );
            }
            s += "</div>";
            return s;
        }

        /// <summary>
        /// Makes a reasonable estimate of the size of the population represented by each of the segments in the plan's demographics
        /// </summary>
        /// <returns></returns>
        public SegmentSize[] PopulationPercentTargeted( out double percentFromAllSegments ) {
            percentFromAllSegments = -1;                    //  this computation TBD
            if( Demographics.Count == 0 ) {
                return new SegmentSize[ 1 ] { new SegmentSize( Guid.NewGuid(), 100.0 ) };        // not sure what to do about the ID here...
            }

            // initialize an object for the overall demographics
            ////List<DemographicGroupValues> overallList = new List<DemographicGroupValues>();
            ////for( int grp = 0; grp < Demographics[ 0 ].ValueCount; grp++ ) {
            ////    DemographicGroupValues overallValues = new DemographicGroupValues( Demographics[ 0 ].Values[ grp ].GroupName );
            ////    overallValues.ValueNames = new string[ Demographics[ 0 ].Values[ grp ].Values.Length ];
            ////    overallValues.Values = new bool[ Demographics[ 0 ].Values[ grp ].Values.Length ];
            ////    for( int i = 0; i < overallValues.Values.Length; i++ ) {
            ////        overallValues.ValueNames[ i ] = Demographics[ 0 ].Values[ grp ].ValueNames[ i ];
            ////        overallValues.Values[ i ] = false;
            ////    }
            ////    overallList.Add( overallValues );
            ////}

            Guid?[] segmentIDs = new Guid?[ Demographics.Count ];
            ////// accumulate the segments into the overall demographics
            for( int s = 0; s < Demographics.Count; s++ ) {
                ////    List<DemographicGroupValues> segList = Demographics[ s ].Values;
                segmentIDs[ s ] = Demographics[ s ].DemographicID;
                ////    for( int d = 0; d < segList.Count; d++ ) {
                ////        DemographicGroupValues dv = segList[ d ];
                ////        for( int i = 0; i < dv.Values.Length; i++ ) {
                ////            //!!! THIS IS NOT THE CORRECT WAY TO COMBINE THE SEGMENTS!!!
                ////            overallList[ d ].Values[ i ] |= dv.Values[ i ];              //"OR" together all the segment values to get the overall demographics
                ////        }
                ////    }
            }

            // make a list that includes the overall demographics as well as the segments
            List<List<DemographicGroupValues>> testSet = new List<List<DemographicGroupValues>> { };
            for( int s = 0; s < Demographics.Count; s++ ) {
                testSet.Add( Demographics[ s ].Values );
            }
            ////testSet.Add( overallList );

            // compute the percent of population represeented by each of the segments
            SegmentSize[] segmentRatio = PopulationPercentTargeted( segmentIDs, testSet );

            // retrieve the results for the overall demographic from the end of the list
            //percentFromAllSegments = segmentRatio[ segmentRatio.Length - 1 ];       //!!! computation TBD !!!

            ////Array.Resize( ref segmentRatio, segmentRatio.Length - 1 );

            return segmentRatio;
        }

        /// <summary>
        /// Calculates the percentage of overall population a given demographic represents.
        /// </summary>
        /// <param name="demoSets"></param>
        /// <returns></returns>
        private SegmentSize[] PopulationPercentTargeted( Guid?[] segmentIDs, List<List<DemographicGroupValues>> demoSets ) {
            //        double[] segmentRato = new double[ demoSets.Count ];

            if( demoSets.Count != segmentIDs.Length ) {
                throw new Exception( "Error: Mismatched segmentIDs list and demoSets in MediaCampaignSpecs.PopulationPercentTargeted()" );
            }
            int nSegments = segmentIDs.Length;

            SegmentSize[] segmentRato = new SegmentSize[ nSegments ];
            for( int seg = 0; seg < nSegments; seg++ ) {
                segmentRato[ seg ] = new SegmentSize( segmentIDs[ seg ], 1 );
            }

            for( int s = 0; s < nSegments; s++ ) {
                List<DemographicGroupValues> segList = demoSets[ s ];
                for( int d = 0; d < segList.Count; d++ ) {
                    DemographicGroupValues dv = segList[ d ];

                    //!!! AT THIS POINT WE SHOULD CONSULT ACTUAL DATA -- FOR NOW ALL SEGMENTS ARE EQIAL !!!
                    double tot = (double)dv.Values.Length;
                    double num = 0;
                    foreach( bool valIsChecked in dv.Values ) {
                        if( valIsChecked ) {
                            num += 1;
                        }
                    }
                    if( num != 0 ) {
                        segmentRato[ s ].SegmentSizeFraction *= num / tot;
                    }
                    //!!! END OF PART TO REPLACE
                }
                segmentRato[ s ].SegmentSizeFraction *= 100;      // return percents
            }
            return segmentRato;
        }

        public class SegmentSize
        {
            public Guid? SegmentID;
            public double SegmentSizeFraction;

            public SegmentSize( Guid? segmentID, double segmentSizeFraction ) {
                this.SegmentID = segmentID;
                this.SegmentSizeFraction = segmentSizeFraction;
            }
        }

        /// <summary>
        /// Returns true if the specified item in the specified group for the specified segment is set (or equal to the value)
        /// </summary>
        /// <param name="segmentIndex"></param>
        /// <param name="groupName"></param>
        /// <param name="itemNameOrValue"></param>
        /// <returns></returns>
        public bool GetValue( int segmentIndex, string groupName, string itemNameOrValue ) {
            if( segmentIndex != -1 && DemographicItemValue( segmentIndex, groupName, itemNameOrValue ) == true ) {
                return true;
            }
            else if( GeneralValueMatch( groupName, itemNameOrValue ) == true ) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the given (typically not segment-specific) input value is in the range.
        /// </summary>
        /// <param name="groupValueName"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public bool GetValue( int segmentIndex, string groupValueName, double minValue, double maxValue ) {
            if( groupValueName == "PurchaseType" ) {
                return false;
            }
            else if( groupValueName == "PurchaseCycle" ) {
                return InRange( this.PurchaseCycleCode, minValue, maxValue );
            }
            else if( groupValueName == "BusinessSituation" ) {
                return false;
            }
            else if( groupValueName == "MonthlySpendRate" ) {
                return InRange( this.MonthlySpendRate, minValue, maxValue );
            }
            else if( groupValueName == "GeoTargetingLevel" ) {
                return false;
            }
            else if( groupValueName == "DemoTargetingLevel" ) {
                return false;
            }
            else {
                throw new Exception( String.Format( "Error: Unknown group value name \"{0}\" in MediaCampaignSpecs.GetValue()", groupValueName ) );
            }
        }

        // convenience method
        /// <summary>
        /// Returns true if val is min is greater than or than or equal to min ands less than or equal to max.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool InRange( double val, double min, double max ) {
            bool inRange = (val >= min) && (val <= max);
            return inRange;
        }

        /// <summary>
        /// Returns a value between 0 and 1 corresponding to the fraction of the range that the user has selected in the given segment
        /// </summary>
        /// <param name="groupValueName"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public double GetValueCorrelationValue( int segmentIndex, string groupValueName, double minValue, double maxValue ) {
            if( groupValueName == "AgeValue" ) {
                return CheckedAgeRangeFraction( segmentIndex, minValue, maxValue );
            }
            else {
                throw new Exception( String.Format( "Error: Unknown group value name \"{0}\" in MediaCampaignSpecs.GetValueCorrelationValue()", groupValueName ) );
            }
        }

        /// <summary>
        /// Returns the fraction of the given age range that corresponds to checked age range(s) in the UI.
        /// </summary>
        /// <param name="segmentIndex"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private double CheckedAgeRangeFraction( int segmentIndex, double minValue, double maxValue ) {
            DemographicSettings dlist = Demographics[ segmentIndex ];
            DemographicGroupValues ageValues = null;
            foreach( DemographicGroupValues dv in dlist.Values ) {
                if( dv.GroupName == "Age" ) {
                    ageValues = dv;
                    break;
                }
            }
            if( ageValues == null ) {     // no specified demographics --> any age is ok
                return 1;
            }

            double totalCoverage = 0;

            for( int i = 0; i < ageValues.ValueNames.Length; i++ ) {
                double min = 0;
                double max = 120;

                if( ageValues.Values[ i ] == false ) {
                    continue;    // un-checked items do not contribute to a match
                }

                switch( ageValues.ValueNames[ i ] ) {
                    case "Under 18":
                        max = 17;
                        break;
                    case "18-25":
                        min = 18;
                        max = 25;
                        break;
                    case "26-35":
                        min = 26;
                        max = 35;
                        break;
                    case "36-45":
                        min = 36;
                        max = 45;
                        break;
                    case "46-55":
                        min = 46;
                        max = 55;
                        break;
                    case "56-65":
                        min = 56;
                        max = 65;
                        break;
                    case "Over 65":
                        min = 66;
                        break;
                    default:
                        throw new Exception( String.Format( "Error: Unknown age value \"{0}\" in CheckedAgeRangeFraction()", ageValues.ValueNames[ i ] ) );
                }

                // determine the overlap of the given range with the user's selected (checked) range
                //double minmin = Math.Min( Math.Max( min, minValue ), max );
                //double maxmax = Math.Max( Math.Min( max, maxValue ), min );
                double minmin = Math.Max( min, minValue );
                double maxmax = Math.Min( max, maxValue );

                double coveredRange = (maxmax - minmin) + 1;
                double fullRange = (maxValue - minValue) + 1;
                double coverageFraction = coveredRange / fullRange;
                if( coverageFraction > 0 ) {
                    totalCoverage += coverageFraction;
                    string s = String.Format( "{0}: min={1:f0} max={2:f0} minValue={3:f0} maxValue={4:f0} coveredRange={5:f0} fullRange={6:f0} coverageFraction={7:f3}",
                        i, min, max, minValue, maxValue, coveredRange, fullRange, coverageFraction );
                }
            }
            return totalCoverage;
        }

        /// <summary>
        /// Returns true if the general (not segment-specific) input value is equal to the given item value.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private bool GeneralValueMatch( string groupName, string itemValue ) {
            if( groupName == "PurchaseType" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return false;
            }
            else if( groupName == "PurchaseCycle" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return nameValue == this.PurchaseCycleCode;
            }
            else if( groupName == "BusinessSituation" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return false;
            }
            else if( groupName == "MonthlySpendRate" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return nameValue == this.MonthlySpendRate;
            }
            else if( groupName == "GeoTargetingLevel" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return false;
            }
            else if( groupName == "DemoTargetingLevel" ) {
                int nameValue = Convert.ToInt32( itemValue );
                return false;
            }
            return false;
        }
 
        /// <summary>
        /// Returns true if the specified named item checkbox in the specified group for the given segment exists and is checked.  
        /// </summary>
        /// <param name="setIndex"></param>
        /// <param name="groupName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public bool DemographicItemValue( int segmentIndex, string groupName, string itemName ) {
            foreach( DemographicGroupValues dvals in Demographics[ segmentIndex ].Values ) {
                if( dvals.GroupName == groupName ) {
                    for( int i = 0; i < dvals.ValueNames.Length; i++ ) {
                        if( dvals.ValueNames[ i ] == itemName ) {
                            return dvals.Values[ i ];
                        }
                    }
                }
            }
            // didn't find a matching value!
            return false;
        }

        public enum CampaignGoal
        {
            GeoTargeting,
            ReachAndAwareness,
            Persuasion,
            Recency,
            DemographicTargeting,
            InterestTargeting
        }
    }
}
