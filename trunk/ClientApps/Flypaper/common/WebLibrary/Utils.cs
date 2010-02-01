//#define LOG_PAGE_VISITS
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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;

using SimInterface;
using HouseholdLibrary;
using GeoLibrary;
using MediaLibrary;

namespace WebLibrary
{
    /// <summary>
    /// Utils contains methods that are used by multiple pages.
    /// </summary>
    public class Utils
    {
        private const double CPM_TO_ACTUAL_COST_SCALE_FACTOR = 100;  //!!!DEBUG
        private const double RADIO_CPM_TO_ACTUAL_COST_SCALE_FACTOR = 2.564;  //!!!DEBUG

        public static string DefaultCampaignName = "Campaign";
        //public static string DefaultCampaignName = "Media Campaign";
        public static string DefaultPlanName = "AdPlanIt";
        public static string DefaultUserPlanName = "Plan";
        public static string EmptyCommentsText = "(comments)";

        protected const string firstVehicleLine = "( choose one )";
        protected const string noSubtypesLine = "( none found in this region )";
        protected const string noVehiclesLine = "( none found in this region )";

        public const string FooterHtml =
                     "<div id=\"Footer\">" + Footer + "</div>";

        //public const string Footer =
        //             "<table cellpadding=\"0\" cellspacing=\"0\" width=\"350\"><tr><td colspan=\"3\" align=\"center\">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width=\"150\"><a href=\"PrivacyPolicy.aspx\">Privacy Policy</a></td>" +
        //             "<td align =\"left\" width=\"50\"><a href=\"UserOptions.aspx\">options</a></td><td align =\"right\"  width=\"150\" ><a href=\"TermsAndConditions.aspx\">Terms and Conditions</a></td></tr></table>";

        //public const string Footer =
        //             "<table cellpadding=\"0\" cellspacing=\"0\" width=\"350\"><tr><td colspan=\"3\" align=\"center\">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width=\"150\"><a href=\"PrivacyPolicy.aspx\">Privacy Policy</a></td>" +
        //             "<td align =\"left\" width=\"50\">&nbsp;</td><td align =\"right\"  width=\"150\" ><a href=\"TermsAndConditions.aspx\">Terms and Conditions</a></td></tr></table>";

        public const string Footer =
                     "<table cellpadding=\"0\" cellspacing=\"0\" width=\"350\">" +
                     "<tr><td><a href=\"About.aspx\">about</a>&nbsp;-&nbsp;<a href=\"Support.aspx\">contact</a>&nbsp;-&nbsp;<a href=\"Privacy.aspx\">privacy</a>&nbsp;-&nbsp;<a href=\"TermsAndConditions.aspx\">terms and conditions</a></td>" +
                     "</tr></table>";

        private static string stateToDmaMap = "stateToDma.csv";
        private static List<string> allStates = null;
        private static Dictionary<string, List<string>> stateToDma = null;

        private static List<MediaVehicle> allMediaVehicles = null;
        private static SimClient serverUpdateObject = null;

        private const string connectionFileName = "dpsimConnectString.txt";

        public static DpMediaDb MediaDatabase = null;

        public static void RefreshMediaDatabase()
        {
            if( MediaDatabase == null )
            {
                string mediaDbConnectionString = ConfigurationManager.AppSettings["AdPlanIt.MediaDatabaseConnectionString"];

                RefreshMediaDatabase(mediaDbConnectionString);

                string dir = HttpContext.Current.Server.MapPath(null);
                string mediaInfoDirectory = dir + @"\Data";

                ReadStateToDMA(mediaInfoDirectory);
            }
        }

        public static void RefreshMediaDatabase(string conenction_string)
        {
            MediaDatabase = new DpMediaDb(conenction_string);

            MediaDatabase.RefreshWebData();

            GeoRegion.SetTopRegion( MediaDatabase.ComputeRegionTree() );

            GeoRegion.TopGeo.AddToLibrary();
        }

        public static void ReadStateToDMA(string mediaInfoDirectory)
        {
            // read in the state-to-dma map
            DirectoryInfo mediaDirectory = new DirectoryInfo(mediaInfoDirectory);
           //  string error = GeoLibrary.GeoRegion.ReadFromFile(mediaInfoDirectory);

            string mapFile = mediaDirectory.FullName + @"\stateToDma.csv";

            stateToDma = ReadStateToDmaMap(mapFile);
        }

        private static Random rnd = new Random( 952 );
        private static Dictionary<int, List<string>> male_names = null;

        private static Dictionary<int, List<string>> female_names = null;

        public static void InitNames( string fileName)
        {
             if( male_names == null )
            {
                male_names = new Dictionary<int, List<string>>();

                StreamReader reader = new StreamReader( fileName + @"\male_names.txt" );

                // these are the decades
                string header = reader.ReadLine();
                string[] decades = header.Split( '\t' );
                int[] decmap = new int[decades.Length];

                for(int index = 0; index < decades.Length; ++index)
                {
                    string decadeName = decades[index];

                    int dec = Int32.Parse( decadeName );

                    male_names.Add( dec, new List<string>() );
                    decmap[index] = dec;
                }

                while( !reader.EndOfStream )
                {
                    string line = reader.ReadLine();

                    string[] vals = line.Split( '\t' );
                    for( int index = 0; index < decades.Length; ++index )
                    {
                        male_names[decmap[index]].Add( vals[index] );
                    }
                }

                reader.Close();
            }

             if( female_names == null )
             {
                 female_names = new Dictionary<int, List<string>>();

                  StreamReader freader = null;

                 try
                 {
                     freader = new StreamReader( fileName + @"\female_names.txt" );
                 }
                 catch( Exception e )
                 {
                     return;
                 }

                 // these are the decades
                 string header = freader.ReadLine();
                 string[] decades = header.Split( '\t' );
                 int[] decmap = new int[decades.Length];

                 for( int index = 0; index < decades.Length; ++index )
                 {
                     string decadeName = decades[index];

                     int dec = Int32.Parse( decadeName );

                     female_names.Add( dec, new List<string>() );
                     decmap[index] = dec;
                 }

                 while( !freader.EndOfStream )
                 {
                     string line = freader.ReadLine();

                     string[] vals = line.Split( '\t' );
                     for( int index = 0; index < decades.Length; ++index )
                     {
                         female_names[decmap[index]].Add( vals[index] );
                     }
                 }

                 freader.Close();
             }
        }

        private static string getName( Dictionary<int, List<string>> nameDict, int age )
        {
            // map age to decade
            int today = DateTime.Now.Year;
            int dateBorn = today - age;

            IEnumerable<int> vals = nameDict.Keys.Where( val => val < dateBorn );

            int decade = nameDict.Keys.First();


            if( vals.Count() > 0 )
            {
                decade = vals.Max();
            }

            int nameDex = rnd.Next( 0, male_names[decade].Count - 1 );

            return nameDict[decade][nameDex];    
        }

        private static string getName(string gender, int age )
        {
            Dictionary<int, List<string>> nameDict = null;

            if( gender == "MALE" )
            {
                nameDict = male_names;
            }
            else
            {
                nameDict = female_names;
            }

            if( nameDict == null || nameDict.Count == 0 )
            {
                return "NA";
            }

            return getName( nameDict, age );
        }

        public static string AgentHtml( Agent agent, string fontSize )
        {
            string html = "<div  style=\"font-size:" + fontSize + "\">";
            html += "<table width=\"400\" border=\"1\">";

            //html += "<tr>";
            //html += "<th colspan=\"2\">";
            //html += "Household";
            //html += "</th>";
            //html += "</tr>";

            // hh header
            html += "<tr>";
            html += "<th>";
            html += "Type";
            html += "</th>";
            html += "<th>";
            html += "Income";
            html += "</th>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += agent.House.Type.ToString(); // Only for debugging +" ID: " + agent.House.Guid.ToString();
            html += "</td>";

            html += "<td>";
            html += agent.House.Income.ToString( "C" );
            html += "</td>";
            html += "</tr>";
            html += "</table>";

            html += "<table   width=\"400\" border=\"1\">";
            html += "<tr>";

            html += "<th colspan = \"4\">";

            if( agent.House.Size == 1 )
            {
                html += "1 Person in Household";
            }
            else
            {
                html += agent.House.Size.ToString() + " People in Household";
            }

            html += "</th>";

            html += "<tr>";
            html += "<th>";
            html += "Name";
            html += "</th>";
            html += "<th>";
            html += "Gender";
            html += "</th>";
            html += "<th>";
            html += "Age";
            html += "</th>";
            html += "<th>";
            html += "Race";
            html += "</th>";

            html += "</tr>";

            for( int i = 0; i < agent.House.People.Count; i++ )
            {
                html += "<tr>";
                html += "<td>";
                html += Utils.getName( agent.House.People[i].Gender.ToString(), agent.House.People[i].Age );
                html += "</td>";
                html += "<td>";
                html += agent.House.People[i].Gender.PCString();
                html += "</td>";
                html += "<td>";
                html += agent.House.People[i].Age.ToString();
                html += "</td>";
                html += "<td>";
                html += agent.House.People[i].Race.PCString();
                html += "</td>";
                html += "</tr>";

            }

            html += "</table>";
            html += "</div>";

            return html;

        }

        public static string MediaHtml( Agent agent, string fontSize, int maxLines )
        {
            string html = "<div  style=\"font-size:" + fontSize + "\">";

            html += "<table  width=\"400\" border=\"1\">";

            int linesPerType = 0;

            if( maxLines > 0 )
            {
                int numMedia = 0;

                foreach( int type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
                {
                    if( agent.House.HasMedia( type ) )
                    {
                        numMedia++;
                    }
                }

                int numPeople = agent.House.People.Count;
                linesPerType = (maxLines - numPeople) / numMedia;
            }

            foreach( int type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
            {
                if( agent.House.HasMedia( type ) )
                {
                    List<HouseholdLibrary.HouseholdMedia> vcls = agent.House.Media( type );

                    int numVcls = vcls.Count;

                    html += "<tr>";
                    html += "<th>";
                    html += Enum.GetName( typeof( MediaVehicle.MediaType ), type ).ToString();
                    html += " (" + numVcls.ToString() + " total)";
                    html += "</th>";
                    html += "<th  width=\"100\">";
                    html += "Hours per Day";
                    html += "</th>";
                    html += "</tr>";

                    int numLines = 0;
                    foreach( HouseholdLibrary.HouseholdMedia house_media in vcls )
                    {
                        if( linesPerType > 0 && numLines >= linesPerType )
                        {
                            html += "<tr>";
                            html += "<td colspan=\"2\">";
                            html += "...";
                            html += "</td>";
                            break;
                        }
                        else
                        {
                            string vclName = Utils.MediaDatabase.GetVehicleName( house_media.Guid );

                            html += "<tr>";
                            html += "<td>";
                            html += vclName;
                            html += "</td>";

                            html += "<td width=\"100\">";
                            html += house_media.Rate.ToString( "F" );
                            html += "</td>";
                            html += "</tr>";
                            numLines++;
                        }
                    }
                }
            }

            html += "</table>";


            html += "</div>";

            return html;

        }

        //public static string MediaHtml( Agent agent, string fontSize, int maxLines )
        //{
        //    string html = "<table style=\"width: 400px\" border=\"1\">";

        //    int linesPerType = 0;

        //    if( maxLines > 0 )
        //    {
        //        int numMedia = 0;

        //        foreach( int type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
        //        {
        //            if( agent.House.HasMedia( type ) )
        //            {
        //                numMedia++;
        //            }
        //        }

        //        int numPeople = agent.House.People.Count;
        //        linesPerType = (maxLines - numPeople) / numMedia;
        //    }

        //    foreach( int type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
        //    {
        //        if( agent.House.HasMedia( type ) )
        //        {
        //            List<HouseholdLibrary.HouseholdMedia> vcls = agent.House.Media( type );

        //            int numVcls = vcls.Count;

        //            html += "<tr>";
        //            html += "<th style=\"font-size:" + fontSize + ";width:200\">";
        //            html += Enum.GetName( typeof( MediaVehicle.MediaType ), type ).ToString();
        //            html += " (" + numVcls.ToString() + " total)";
        //            html += "</th>";
        //            html += "<th style=\"font-size:" + fontSize + ";width:200\">";
        //            html += "Hours per Day";
        //            html += "</th>";
        //            html += "</tr>";

        //            int numLines = 0;
        //            foreach( HouseholdLibrary.HouseholdMedia house_media in vcls )
        //            {
        //                if( linesPerType > 0 && numLines >= linesPerType )
        //                {
        //                    html += "<tr>";
        //                    html += "<td colspan=\"2\" style=\"font-size:" + fontSize + ";width:400\">";
        //                    html += "...";
        //                    html += "</td>";
        //                    break;
        //                }
        //                else
        //                {
        //                    string vclName = Utils.MediaDatabase.GetVehicleName( house_media.Guid );

        //                    html += "<tr>";
        //                    html += "<td style=\"font-size:" + fontSize + ";width:200\">";
        //                    html += vclName;
        //                    html += "</td>";

        //                    html += "<td style=\"font-size:" + fontSize + ";width:200\">";
        //                    html += house_media.Rate.ToString( "F" );
        //                    html += "</td>";
        //                    html += "</tr>";
        //                    numLines++;
        //                }
        //            }
        //        }
        //    }

        //    html += "</table>";

        //    return html;

        //}
        public static Dictionary<Guid, double> vehicle_sizes;

        public static void Init() {
            if( ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] != null )
            {
               
                MembershipProvider mem = Membership.Providers["SqlProvider"];

                System.Reflection.FieldInfo mem_connectionStringField = typeof( SqlMembershipProvider ).GetField( "_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic );

                mem_connectionStringField.SetValue( mem, ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] );

                RoleProvider rol = Roles.Providers["SqlProvider"];
                System.Reflection.FieldInfo rol_connectionStringField = typeof( SqlRoleProvider ).GetField( "_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic );
                rol_connectionStringField.SetValue( rol, ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] );
            }
        }

        #region Methods for Obtaining Session Data
        /// <summary>
        /// Loads the session values into corresponding variables.  If the session does not have a current media plan, a new one is created.  
        /// If the session does not have a saved-plan list, a new one is created containing the plans now on disk for the current user.
        /// </summary>
        /// <param name="currentMediaPlan"></param>
        /// <param name="allMediaPlans"></param>
        /// <param name="runningMediaPlanIDs"></param>
        public static void LoadValuesFromSession( System.Web.UI.Page callingPage,
            out MediaPlan currentMediaPlan, out List<MediaPlan> allMediaPlans, out List<Guid> runningMediaPlanIDs ) {

            LoadValuesFromSession( callingPage, out currentMediaPlan, out allMediaPlans, out runningMediaPlanIDs, false, true );
        }

        
        /// <summary>
        /// Loads the session values into corresponding variables.  If the session does not have a current media plan, a new one is created.  
        /// If the session does not have a saved-plan list, a new one is created containing the plans now on disk for the current user.
        /// </summary>
        /// <param name="currentMediaPlan"></param>
        /// <param name="allMediaPlans"></param>
        /// <param name="runningMediaPlanIDs"></param>
        public static void LoadValuesFromSession( System.Web.UI.Page callingPage,
            out MediaPlan currentMediaPlan, out List<MediaPlan> allMediaPlans, out List<Guid> runningMediaPlanIDs, bool okToCreate ) {

            LoadValuesFromSession( callingPage, out currentMediaPlan, out allMediaPlans, out runningMediaPlanIDs, okToCreate, true );
        }

        /// <summary>
        /// Loads the session values into corresponding variables.  If the session does not have a current media plan, a new one is created.  
        /// If the session does not have a saved-plan list, a new one is created containing the plans now on disk for the current user.
        /// </summary>
        /// <param name="currentMediaPlan"></param>
        /// <param name="allMediaPlans"></param>
        /// <param name="runningMediaPlanIDs"></param>
        public static void LoadValuesFromSession( System.Web.UI.Page callingPage,
            out MediaPlan currentMediaPlan, out List<MediaPlan> allMediaPlans, out List<Guid> runningMediaPlanIDs, bool okToCreate, bool logPageVisit ) {

            currentMediaPlan = CurrentMediaPlan( callingPage, okToCreate );
            allMediaPlans = AllMediaPlans( callingPage );
            runningMediaPlanIDs = RunningMediaPlanIDs( callingPage );

#if LOG_PAGE_VISITS
            if( logPageVisit == true && callingPage.IsPostBack == false ) {
                DataLogger.LogPageVisit( callingPage );
            }
#endif
        }

        /// <summary>
        /// Loads the values from the session pertaining to media plan comparison.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="currentMediaPlan"></param>
        /// <param name="allMediaPlans"></param>
        /// <param name="runningMediaPlanIDs"></param>
        /// <param name="referenceMediaPlanID"></param>
        /// <param name="comparisonMediaPlan"></param>
        public static void LoadComparisonValuesFromSession( System.Web.UI.Page callingPage, out Guid? referenceMediaPlanID, out Guid? comparisonMediaPlanID ) {
            referenceMediaPlanID = ReferenceMediaPlanID( callingPage );
            comparisonMediaPlanID = ComparisonMediaPlanID( callingPage );
        }

        /// <summary>
        /// Gets the current DpUpdate object, which is used to communicate with the sim server.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static SimClient GetUpdateObject( System.Web.UI.Page callingPage ) {
            if( serverUpdateObject == null ) {
                if( callingPage.Session[ "CurrentUpdateObject" ] != null ) {
                    serverUpdateObject = (SimClient)callingPage.Session[ "CurrentUpdateObject" ];
                }
                else {
                    // the update object is null and not already in the session - create it
                    serverUpdateObject = new SimClient();
                    callingPage.Session[ "CurrentUpdateObject" ] = serverUpdateObject;
                }
            }
            return serverUpdateObject;
        }

        /// <summary>
        /// Gets the current media plan from the session.  If the session does not have a current media plan, a new one is created and added.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static MediaPlan CurrentMediaPlan( System.Web.UI.Page callingPage, bool okToCreate ) {
            MediaPlan curPlan = null;
            if( callingPage.Session[ "CurrentMediaPlan" ] != null ) {
                curPlan = (MediaPlan)callingPage.Session[ "CurrentMediaPlan" ];
            }
            else if( okToCreate ) {
                curPlan = new MediaPlan();
                callingPage.Session.Add( "CurrentMediaPlan", curPlan );
            }
            return curPlan;
        }

        /// <summary>
        /// Gets the current user-profile data from the session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static UserMedia CurrentUserProfile( System.Web.UI.Page callingPage, out bool profileNeedsLoading ) {
            profileNeedsLoading = false;
            if( Utils.UserIsDemo( callingPage ) == true ) {
                return null;
            }

            // if we get here. the user is logged in
            if( callingPage.Session[ "CurrentUserProfile" ] == null ) {
                profileNeedsLoading = true;
                return null;
            }
            else {
                return (UserMedia)callingPage.Session[ "CurrentUserProfile" ];
            }
        }

        /// <summary>
        /// Gets the ID of the current reference media plan from the session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        private static Guid? ReferenceMediaPlanID( System.Web.UI.Page callingPage ) {
            Guid? refPlan = null;
            if( callingPage.Session[ "ReferenceMediaPlan" ] != null ) {
                refPlan = (Guid?)callingPage.Session[ "ReferenceMediaPlan" ];
            }
            return refPlan;
        }

        /// <summary>
        /// Gets the  ID of the current comparison media plan from the session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        private static Guid? ComparisonMediaPlanID( System.Web.UI.Page callingPage ) {
            Guid? cmpPlan = null;
            if( callingPage.Session[ "ComparisonMediaPlan" ] != null ) {
                cmpPlan = (Guid?)callingPage.Session[ "ComparisonMediaPlan" ];
            }
            return cmpPlan;
        }

        /// <summary>
        /// Gets the list of running media plans from the session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static List<MediaPlan> CurrentMediaPlans( System.Web.UI.Page callingPage ) {
            List<MediaPlan> curPlans = null;
            if( callingPage.Session[ "CurrentMediaPlans" ] != null ) {
                curPlans = (List<MediaPlan>)callingPage.Session[ "CurrentMediaPlans" ];
            }
            return curPlans;
        }

        /// <summary>
        /// Gets the dictionary of plan versions for the campaigns in this session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static Dictionary<string, List<PlanStorage.PlanVersionInfo>> AllPlanVersions( System.Web.UI.Page callingPage ) {
            Dictionary<string, List<PlanStorage.PlanVersionInfo>> allVersions = null;
            if( callingPage.Session[ "AllPlanVersions" ] != null ) {
                allVersions = (Dictionary<string, List<PlanStorage.PlanVersionInfo>>)callingPage.Session[ "AllPlanVersions" ];
            }
            return allVersions;
        }

        /// <summary>
        /// Gets the list of running media plans from the session. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static List<Guid> RunningMediaPlanIDs( System.Web.UI.Page callingPage ) {
            List<Guid> runningList = null;
            if( callingPage.Session[ "RunningMediaPlans" ] != null ) {
                runningList = (List<Guid>)callingPage.Session[ "RunningMediaPlans" ];
            }
            return runningList;
        }

        /// <summary>
        /// Gets the saved-plan list from the session.  If the session does not have a saved-plan list, a new one is created containing the plans now on disk.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static List<MediaPlan> AllMediaPlans( System.Web.UI.Page callingPage ) {
            // obsolete!!!
            return null;
            ////List<MediaPlan> curPlans = null;
            ////if( callingPage.Session[ "AllMediaPlans" ] != null ) {
            ////    curPlans = (List<MediaPlan>)callingPage.Session[ "AllMediaPlans" ];
            ////}
            ////else {
            ////    curPlans = new List<MediaPlan>();
            ////    curPlans.AddRange( PlanStorage.SavedPlans( GetUser( callingPage ) ) );
            ////    callingPage.Session.Add( "AllMediaPlans", curPlans );
            ////}
            ////return curPlans;
        }

        // TBD
        // This is not needed ?
        // SSN
        //public static List<MediaPlan> PlansBeingEdited( System.Web.UI.Page callingPage ) {
        //    List<MediaPlan> editngList = null;
        //    if( callingPage.Session[ "MediaPlansBeingEdited" ] != null ) {
        //        editngList = (List<MediaPlan>)callingPage.Session[ "MediaPlansBeingEdited" ];
        //    }
        //    else {
        //        editngList = new List<MediaPlan>();
        //        callingPage.Session.Add( "MediaPlansBeingEdited", editngList );
        //    }
        //    return editngList;
        //}
        #endregion

        #region Methods for Setting Session Data
        /// <summary>
        /// Sets the given plan to be the current plan in the session.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static void SetCurrentMediaPlan( System.Web.UI.Page callingPage, MediaPlan curPlan ) {
            callingPage.Session[ "CurrentMediaPlan" ] = curPlan;
        }

        /// <summary>
        /// Sets the given plan list to be the all-plans list in the session.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlans"></param>
        public static void SetAllMediaPlans( System.Web.UI.Page callingPage, List<MediaPlan> curPlans ) {
            callingPage.Session[ "AllMediaPlans" ] = curPlans;
        }

        // TBD
        // no longer needed ?
        // SSN
        /// <summary>
        /// Sets the given plan list to be the plans-being-edited page.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlans"></param>
        //public static void SetPlansBeingEdited( System.Web.UI.Page callingPage, List<MediaPlan> curEdPlans ) {
        //    callingPage.Session[ "MediaPlansBeingEdited" ] = curEdPlans;
        //}


        /// <summary>
        /// Sets the given userProfileData to be the current profile data in the session.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="userProfileData"></param>
        public static void SetCurrentUserProfile( System.Web.UI.Page callingPage, UserMedia userProfileData ) {
            callingPage.Session[ "CurrentUserProfile" ] = userProfileData;
        }

        /// <summary>
        /// Sets the given plan to be the reference plan in the session.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static void SetReferenceMediaPlan( System.Web.UI.Page callingPage, Guid? refPlanID ) {
            callingPage.Session[ "ReferenceMediaPlan" ] = refPlanID;
        }

        /// <summary>
        /// Sets the given plan to be the reference plan in the session.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static void SetComparisonMediaPlan( System.Web.UI.Page callingPage, Guid? comparisonPlanID ) {
            callingPage.Session[ "ComparisonMediaPlan" ] = comparisonPlanID;
        }

        /// <summary>
        /// Sets the list of plans as the current-version plans , one for each campaign. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="currentPlans"></param>
        public static void SetCurrentMediaPlans( System.Web.UI.Page callingPage, List<MediaPlan> currentPlans ) {
            callingPage.Session[ "CurrentMediaPlans" ] = currentPlans;
        }

        /// <summary>
        /// Sets the dictionary of all campaign versions. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="allVersions"></param>
        public static void SetAllPlanVersions( System.Web.UI.Page callingPage, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allVersions ) {
            callingPage.Session[ "AllPlanVersions" ] = allVersions;
        }

        /// <summary>
        /// Sets the given list of GUIDs as the IDs for the currently-running media plans. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static void SetRunningMediaPlanIDs( System.Web.UI.Page callingPage, List<Guid> runningPlanIDs ) {
            callingPage.Session[ "RunningMediaPlans" ] = runningPlanIDs;
        }

        /// <summary>
        /// Gets the ID for the currently-running sim (null if no sim is running)
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static Guid? GetRunningSim( System.Web.UI.Page callingPage ) {
            return (Guid?)callingPage.Session[ "SimID" ];
        }


        /// <summary>
        /// Sets the type of the media item that the MediaDetails.aspx page will show. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static void SetMediaTypeForDetails( System.Web.UI.Page callingPage, string curretMediaType ) {
            callingPage.Session[ "CurrentMediaDetailsType" ] = curretMediaType;
        }

        /// <summary>
        /// Gets the type of the media item that the MediaDetails.aspx page will show. 
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="curPlan"></param>
        public static string GetMediaTypeForDetails( System.Web.UI.Page callingPage ) {
            return (string)callingPage.Session[ "CurrentMediaDetailsType" ];
        }

        #endregion

        /// <summary>
        /// Returns the name (email address) of the current user
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static string GetUser( System.Web.UI.Page callingPage ) {
            if( callingPage.User.Identity.IsAuthenticated == true ) {
                return callingPage.User.Identity.Name;
            }
            else {
                return "DEMO";
            }
        }
        /// <summary>
        /// Returns true if the current user has logged in
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static bool UserIsDemo( System.Web.UI.Page callingPage ) {
            return !callingPage.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Set the flag to show extra engineering-only information
        /// </summary>
        /// <param name="callingPage"></param>
        public static void SetEngineeringMode( System.Web.UI.Page callingPage, bool set ) {
            if( callingPage.Session[ "EngineeringMode" ] != null ) {
                callingPage.Session[ "EngineeringMode" ] = set;
            }
            else {
                callingPage.Session.Add( "EngineeringMode", set );
            }
        }

        
        /// <summary>
        /// If thie returns true, we are in engineering mode (generally this means we should display some extra data)
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static bool InEngineeringMode( System.Web.UI.Page callingPage ) {
            return InEngineeringMode( callingPage, null );
        }

        /// <summary>
        /// If thie returns true, we are in engineering mode (generally this means we should display some extra data)
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        public static bool InEngineeringMode( System.Web.UI.Page callingPage, Label advisoryLabel ) {
            bool engMode = false;
            if( callingPage.Request[ "eng" ] == "true" ) {
                SetEngineeringMode( callingPage, true );
                engMode = true;
            }
            else if( callingPage.Request[ "eng" ] == "false" ) {
                SetEngineeringMode( callingPage, false );
                engMode = false;
            }
            else {
                if( callingPage.Session[ "EngineeringMode" ] != null ) {
                    engMode = (bool)callingPage.Session[ "EngineeringMode" ];
                }
            }

            if( advisoryLabel != null && engMode == true ) {
                SetEngineeringModeDisplay( advisoryLabel );
            }

            return engMode;
        }

        public static void SetEngineeringModeDisplay( Label advisoryLabel ) {
            advisoryLabel.Text = "ENGINEERING Mode&nbsp&nbsp;<a href=\"?eng=false\">(turn off)</a>";
            advisoryLabel.Style.Add( HtmlTextWriterStyle.BackgroundColor, "Yellow" );
            advisoryLabel.Style.Add( HtmlTextWriterStyle.Color, "Red" );
        }

#region General Utilities
        public static string FormatDollarAmount( double amt ) {
            string amtStr = "";
            if( amt == 0 ) {
                amtStr = "$&nbsp;0";
            }
            else if( amt < 1 ) {
                amtStr = String.Format( "$&nbsp;{0:f4}", amt );
            }
            else if( amt < 1000 ) {
                amtStr = String.Format( "$&nbsp;{0:f2}", amt );
            }
            else {
                amtStr = String.Format( "$&nbsp;{0:0,0}", amt );
            }
            return amtStr;
        }

        public static string FormatDollarAmount( string spanId, double amt ) {
            return String.Format( "<span ID=\"{0}\" >{1}<span>", spanId, FormatDollarAmount( amt ) );
        }

        public static void AddWaitCursorOnClick( WebControl control, string waitCursorDivId ) {
            control.Attributes.Add( "onClick", "document.getElementById( \"" + waitCursorDivId + "\" ).style.cursor = \"progress\"; return true;" );
        }

        /// <summary>
        /// Returns an HTML string corresponding to the media selections reasons given in the MediaRuleReason list.  
        /// The rules in the HTML are sorted in decreasing order of score.
        /// </summary>
        /// <param name="rawReasonsList"></param>
        /// <param name="htmlFormat"></param>
        /// <returns></returns>
        public static string AllReasons( List<MediaRuleReason> rawReasonsList, bool htmlFormat ) {
            int nReasons = rawReasonsList.Count;

            double[] scores = new double[ nReasons ];
            MediaRuleReason[] reasonsArray = new MediaRuleReason[ nReasons ];
            for( int i = 0; i < nReasons; i++ ) {
                reasonsArray[ i ] = rawReasonsList[ i ];
                scores[ i ] = rawReasonsList[ i ].Score;
            }
            Array.Sort( scores, reasonsArray );
            Array.Reverse( scores );
            Array.Reverse( reasonsArray );

            string all = "";
            for( int i = 0; i < nReasons; i++ ) {

                if( reasonsArray[ i ].TextColor == "" ) {
                    all += String.Format( "{0} ({1})", reasonsArray[ i ].Text, scores[ i ] );
                }
                else {
                    all += String.Format( "<font color={1}>{0} ({2})</font>", reasonsArray[ i ].Text, reasonsArray[ i ].TextColor, scores[ i ] );
                }

                if( i != nReasons - 1 ) {
                    if( htmlFormat ) {
                        all += "<br>";
                    }
                    else {
                        all += "\n";
                    }
                }
            }
            return all;
        }

        /// <summary>
        /// Configures a "save" button to ask for the name of the new plan, if saving will create a new plan version.
        /// </summary>
        /// <param name="currentPlan"></param>
        /// <param name="saveButton"></param>
        public static void SetupNextVersionNamePopup( MediaPlan currentPlan, ImageButton saveButton, string existingPlanNamesList, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions ) {

            string suggestedNewName = GetSuggestedPlanName( currentPlan.PlanDescription, currentPlan.CampaignName, allPlanVersions, false );

            // see if clicking Save will trigger a version change
            if( currentPlan.Edited == true && currentPlan.Results != null ) {
                // clicking save is going to generate a new plan version!  We need to advise the user and get a new plan description.
                saveButton.OnClientClick = String.Format( "return GetNameForNextVersion( \"{0}\" );", suggestedNewName );
            }
        }

        /// <summary>
        /// Returns the suggested name to be used when a new plan version is to be generated.
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="campaignName"></param>
        /// <param name="allPlanVersions"></param>
        /// <param name="isAutogenerated"></param>
        /// <returns></returns>
        public static string GetSuggestedPlanName( string baseName, string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions,
            bool isAutogenerated ) {

            List<string> existingNames = Utils.AllPlanNamesFor( campaignName, allPlanVersions );
            if( existingNames == null || existingNames.Contains( baseName ) == false ) {
                return baseName;
            }

            int numStart = -1;
            for( int c = baseName.Length - 1; c >= 0; c-- ) {
                if( Char.IsDigit( baseName[ c ] ) == false ) {
                    break;
                }
                // we're in a numeric suffix...keep going
                numStart = c;
            }

            long suffixNum = -1;
            string unNumberedBase = null;

            if( numStart != -1 ) {
                // the base name ends with a number
                unNumberedBase = baseName.Substring( 0, numStart );
                suffixNum = long.Parse( baseName.Substring( numStart ) );
            }
            else {
                // the base name does not end with a number now
                unNumberedBase = baseName + " v";
                suffixNum = 1;
            }
            // now we have a base name and a suffix number

            // get the next unused name
            string newName = null;
            do {
                suffixNum += 1;
                newName = unNumberedBase + suffixNum.ToString();
            } while( existingNames.Contains( newName ) );

            return newName;
        }

        /// <summary>
        /// Returns a lsit of all PlanVersionInfo objects associated with the given campaign.
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="allPlanVersions"></param>
        /// <returns></returns>
        public static List<PlanStorage.PlanVersionInfo> AllVersionsInfoForCampaign( string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions ){
            if( allPlanVersions.ContainsKey( campaignName ) ){
                return allPlanVersions[ campaignName ];
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of the names of all plan versions in the given campaign.
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="allPlanVersions"></param>
        /// <returns></returns>
        public static List<string> AllPlanNamesFor( string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions ) {
            List<PlanStorage.PlanVersionInfo> info = AllVersionsInfoForCampaign( campaignName, allPlanVersions );
            if( info == null ) {
                return null;
            }
            List<string> names = new List<string>();
            foreach( PlanStorage.PlanVersionInfo vInfo in info ) {
                string planName = vInfo.Description;
                if( planName == null ) {
                    planName = "-";           // just in case this is an old plan
                }
                names.Add( planName );
            }
            names.Sort();
            return names;
        }

        /// <summary>
        /// Returns a list of all plan versions in the given campaign.
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="allPlanVersions"></param>
        /// <returns></returns>
        public static List<string> AllPlanVersionsFor( string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions ) {
            List<PlanStorage.PlanVersionInfo> info = AllVersionsInfoForCampaign( campaignName, allPlanVersions );
            if( info == null ) {
                return null;
            }
            List<string> names = new List<string>();
            foreach( PlanStorage.PlanVersionInfo vInfo in info ) {
                names.Add( vInfo.Version );
            }
            names.Sort();
            return names;
        }

        private static string sepChar = "^";
        /// <summary>
        /// Converts a list to a single string so it can be passed to a Javascript method (and then split back into the list in the Javascript).
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string ConvertToJavascriptArg( List<string> strings ) {
            string s = "";
            if( strings != null ) {
                for( int i = 0; i < strings.Count; i++ ) {
                    s += strings[ i ];
                    if( i < strings.Count - 1 ) {
                        s += sepChar;
                    }
                }
            }
            return s;
        }
#endregion

        #region Media Utilities

        /// <summary>
        /// Returns a new name for a market campaign
        /// </summary>
        public static string NewCampaignName( List<MediaPlan> currentMediaPlans ) {
            string newCampaignNameBase = Utils.DefaultCampaignName;
            string newCampaignName = newCampaignNameBase;
            int newCampaignSerial = 0;
            bool existsAlready = false;

            do {
                existsAlready = false;
                newCampaignSerial += 1;
                newCampaignName = String.Format( "{0} {1}", newCampaignNameBase, newCampaignSerial );
                foreach( MediaPlan plan in currentMediaPlans ) {
                    if( plan.CampaignName == newCampaignName ) {
                        existsAlready = true;
                        break;
                    }
                }

            } while( existsAlready == true );

            return newCampaignName;
        }

        /// <summary>
        /// Returns a new name for a market plan
        /// </summary>
        public static string NewPlanName( string baseName, List<MediaPlan> allPlans ) {
            return NewPlanName( baseName, true, false, allPlans );
        }

        /// <summary>
        /// Returns a new name for a market plan
        /// </summary>
        public static string NewPlanName( string campaignName, bool isUserPlan, List<MediaPlan> allPlans ) {
            return NewPlanName( campaignName, isUserPlan, true, allPlans );
        }

        /// <summary>
        /// Returns a new name for a market plan
        /// </summary>
        private static string NewPlanName( string baseName, bool isUserPlan, bool addIdentifier,  List<MediaPlan> allPlans ) {
            if( baseName == null ) {
                baseName = "";
            }

            string newPlanNameBase = baseName;
            if( addIdentifier ) {
                if( newPlanNameBase != "" ) {
                    newPlanNameBase += " ";
                }

                if( isUserPlan ) {
                    newPlanNameBase += Utils.DefaultUserPlanName;
                }
                else {
                    newPlanNameBase += Utils.DefaultPlanName;
                }

                if( newPlanNameBase != "" ) {
                    newPlanNameBase += " ";      // put a space before the appended number
                }
            }

            string newPlanName = newPlanNameBase;
            int newPlanSerial = 0;
            bool existsAlready = false;

            if( addIdentifier == false ) {
                newPlanSerial = -1;               // try the given name first if addIdentifier == false 
            }

            do {
                existsAlready = false;
                newPlanSerial += 1;
                if( newPlanSerial > 0 ) {
                    newPlanName = String.Format( "{0}{1}", newPlanNameBase, newPlanSerial );
                }
                else {
                    newPlanName = newPlanNameBase;        // try the given name first if addIdentifier == false 
                }
                foreach( MediaPlan plan in allPlans ) {
                    if( plan.PlanDescription == newPlanName ) {
                        existsAlready = true;
                        break;
                    }
                }

            } while( existsAlready == true );

            return newPlanName;
        }

        /// <summary>
        /// Returns the list of all media type names.
        /// </summary>
        /// <returns></returns>
        public static List<string> AllMediaTypes() {

            List<string> allTypes = MediaDatabase.GetMediaTypes();
            return allTypes;

        }

        /// <summary>
        /// Returns the list of all media subtypes for a given media type.
        /// </summary>
        /// <returns></returns>
        //public static List<MediaVehicle> AllMediaSubtypes( string mediaType, List<List<string>> geoRegions ) {
        public static List<string> AllMediaSubtypes( string mediaType, List<List<string>> geoRegions ) {
            List<GeoLibrary.GeoRegion> geoRegionObjs = new List<GeoLibrary.GeoRegion>();
            if( geoRegions != null ) {
                foreach( List<string> geoNames in geoRegions ) {
                    if( geoNames.Count == 2 ) {
                        geoRegionObjs.Add( GeoLibrary.GeoRegion.TopGeo.GetSubRegion( geoNames[ 1 ] ) );
                    }
                    else {
                        // no regions --> doing a national campaign
                        geoRegionObjs.Add( GeoLibrary.GeoRegion.TopGeo );
                    }
                }
            }

            List<string> allSubTypes = MediaDatabase.GetSubTypes( mediaType, geoRegionObjs );
            return allSubTypes;
        }

        /// <summary>
        /// Returns the list of all media vehicles of the given media type.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="geoRegions"></param>
        /// <returns></returns>
        //public static List<MediaVehicle> AllMediaVehicles( string mediaType, List<List<string>> geoRegions, UserMedia userMedia ) {
        //    return AllMediaVehicles( mediaType, null, geoRegions, userMedia );
        //}

        ///// <summary>
        ///// Returns the list of all media part names for a given media type and subtype.
        ///// </summary>
        ///// <returns></returns>
        //public static List<MediaVehicle> AllMediaVehicles( string mediaType, string mediaSubtype, List<List<string>> geoRegions, UserMedia userMedia ) {
        //    List<GeoLibrary.GeoRegion> geoRegionObjs = new List<GeoLibrary.GeoRegion>();
        //    if( geoRegions != null ) {
        //        foreach( List<string> geoNames in geoRegions ) {
        //            if( geoNames.Count == 2 ) {
        //                geoRegionObjs.Add( GeoLibrary.GeoRegion.TopGeo.GetSubRegion( geoNames[ 1 ] ) );
        //            }
        //            else {
        //                // no regions --> doing a national campaign
        //                geoRegionObjs.Add( GeoLibrary.GeoRegion.TopGeo );
        //            }
        //        }
        //    }
        //    List<MediaVehicle> allVehicles = MediaDatabase.GetMediaVehicles( mediaType, mediaSubtype, geoRegionObjs );
        //    return allVehicles;

        //}

        /// <summary>
        /// Tests to see if the given media in in one of the given geo regions.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private static bool TestGeoRegion( MediaVehicle info, List<List<string>> geoRegions ) {
            if( info.RegionName == "USA" ) {
                return true;        // national media always passes
            }

            bool geoTestOK = true;

            // see if this is a nationwide campaign
            bool isNatiional = true;
            if( geoRegions != null ) {
                foreach( List<string> geo in geoRegions ) {
                    if( geo.Count > 1 || (geo.Count == 1 && geo[ 0 ] != "USA" && geo[ 0 ] != "Entire USA") ) {
                        isNatiional = false;
                    }
                }
            }


            if( isNatiional == false ) {
                geoTestOK = false;
                foreach( List<string> geoInfo in geoRegions ) {
                    if( geoInfo.Count > 1 && geoInfo[ 0 ] != "Entire USA" ) {
                        // only the DMA need to match
                        string regionDma = geoInfo[ 1 ];
                        if( info.RegionName == regionDma ) {
                            geoTestOK = true;
                            break;
                        }
                    }
                    else {
                        // the Entire USA (or no region) was specified for this geo region
                        geoTestOK = true;
                        break;
                    }
                }
            }
            else {
                // this is a nationwide campaign
                geoTestOK = true;
                if( info.RegionName != "USA" ) {
                    geoTestOK = false;
                }
            }
            return geoTestOK;
        }

        ///// <summary>
        ///// The primary method for getting MediaVehicle objects. Gets the media for the given ID from the master list of media; if not found in the master list, then also looks in the current user's locally-defined media.
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public static MediaVehicle MediaTypeForID( Guid guid, UserMedia userMedia ) {
        //    bool dum = false;
        //    return GetMediaVehicleForID( guid, userMedia, out dum );
        //}
            
        ///// <summary>
        ///// The primary method for getting MediaVehicle objects. Gets the media for the given ID from the master list of media; if not found in the master list, then also looks in the current user's locally-defined media.
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public static MediaVehicle GetMediaVehicleForID( Guid guid, UserMedia userMedia, out bool isUserMedia ) {
        //    isUserMedia = false;
        //    MediaVehicle listedVehicle = GetMediaVehicleForID(guid);
        //    if( listedVehicle != null ) {
        //        return listedVehicle;
        //    }

        //    // the ID was not found in the master list = check user items
        //    if (userMedia != null)
        //    {
        //        foreach (UserMedia.MediaVehicleSpec spec in userMedia.MediaVehicleSpecs)
        //        {
        //            if (spec.ItemID == guid)
        //            {
        //                isUserMedia = true;
        //                return MediaVehicleForSpec(spec);
        //            }
        //        }
        //    }

        //    // no media found with this ID
        //    return null;
        //}

        ///// <summary>
        ///// Gets the media for the given ID from the master list of media
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //private static MediaVehicle GetMediaVehicleForID(Guid guid)
        //{
        //    try {
        //        MediaVehicle listedVehicle = MediaDatabase.GetVehicle( guid, true );
        //        return listedVehicle;
        //    }
        //    catch( Exception ) {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Returns a MediaVehicle described by the given MediaVehicleSpec
        ///// </summary>
        ///// <param name="spec"></param>
        ///// <returns></returns>
        //public static MediaVehicle MediaVehicleForSpec( UserMedia.MediaVehicleSpec spec ) {

        //    // convert the spec items to the necessary objects
        //    MediaVehicle.AdCycle adCycle = (MediaVehicle.AdCycle) Enum.Parse( typeof( MediaVehicle.AdCycle ), spec.Cycle );
        //    MediaVehicle.MediaType mtype = (MediaVehicle.MediaType)Enum.Parse( typeof( MediaVehicle.MediaType ), spec.MediaType );

        //    GeoLibrary.GeoRegion region = new GeoLibrary.GeoRegion( "USA", GeoLibrary.GeoRegion.RegionType.DMA );
        //    if(  spec.GeoRegions != null && spec.GeoRegions.Count > 0 ){
        //        if( spec.GeoRegions[ 0 ].Count > 1 ) {
        //            region = new GeoLibrary.GeoRegion( spec.GeoRegions[ 0 ][ 1 ], GeoLibrary.GeoRegion.RegionType.DMA );       // there won't be more than one region
        //            ////region = SimUtils.GeoRegionForName( spec.GeoRegions[ 0 ] );        // there won't be more than one region
        //        }
        //    }

        //    // get the comparable vehicle
        //    MediaVehicle similarVehicle = null;
        //    if( spec.ComparableVehicleID != null ) {
        //        similarVehicle = GetMediaVehicleForID( (Guid)spec.ComparableVehicleID );
        //    }
        //    else {
        //        // just get the representative subtype vehicle
        //        //!!!! there is no longer such a concept Aug 2008
        //        ////similarVehicle = MediaInfoForName( spec.MediaType, spec.SubType );
        //    }

        //    double persuasionValue = 0;
        //    double probValue = 0;
        //    if( similarVehicle != null && similarVehicle is ScalarVehicle ) {
        //        persuasionValue = ((ScalarVehicle)similarVehicle).persuasion_scalar;
        //        probValue = ((ScalarVehicle)similarVehicle).prob_scalar;
        //    }

        //    MediaVehicle vehicle = null;
        //    if( similarVehicle.Type == MediaVehicle.MediaType.Magazine ) {
        //        vehicle = new Magazine( similarVehicle.Guid, ((Magazine)similarVehicle).Size, persuasionValue, probValue, mtype, spec.SubType, spec.Vehicle, region, adCycle, spec.CPM, spec.URL );
        //    }
        //    else {
        //        vehicle = new ScalarVehicle(similarVehicle.Guid, similarVehicle.Size, persuasionValue, probValue, mtype, spec.SubType, spec.Vehicle, region, adCycle, spec.CPM, spec.URL);
        //    }

        //    if( similarVehicle != null ){
        //        foreach( int opt in similarVehicle.GetOptions().Keys ){
        //            vehicle.AddOption( opt, similarVehicle.GetOptions()[ opt ] );
        //        }
        //    }

        //    return vehicle;
        //}

        /// <summary>
        /// Returns the maximum number of ad spots that the given media item can have in the given time interval.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        //public static int PhyscalMaximumAdSpots( MediaItem item, MediaPlan plan ) {
        //    MediaVehicle itemVehicle = Utils.GetMediaVehicleForID( item.vehicle_id );
        //    int planDays = (int)Math.Round( (plan.EndDate - plan.StartDate).TotalDays );

        //    int planMonths = (plan.EndDate.Month - plan.StartDate.Month) + 12 * (plan.EndDate.Year - plan.StartDate.Year);
        //    if( plan.EndDate.Day < plan.StartDate.Day - 1 ) {
        //        planMonths -= 1;
        //    }

        //    switch( itemVehicle.Cycle ) {
        //        case MediaVehicle.AdCycle.Instant:
        //            return planDays * 24 * 10000;            //!!!???what value should we return here??? usint 10K ads/day as limit for "instantaneous"
        //            break;

        //        case MediaVehicle.AdCycle.Hourly:
        //            return planDays * 24;

        //        case MediaVehicle.AdCycle.Daily:
        //            return planDays;

        //        case MediaVehicle.AdCycle.Weekly:
        //            return planDays / 7;

        //        case MediaVehicle.AdCycle.Bimonthly:
        //            return planMonths * 2;

        //        case MediaVehicle.AdCycle.Monthly:
        //            return planMonths;

        //        case MediaVehicle.AdCycle.Quarterly:
        //            return planMonths / 3;

        //        case MediaVehicle.AdCycle.Yearly:
        //            return planMonths / 12;
        //    }

        //    return 0;       // should never get here
        //}

        ///// <summary>
        ///// Returns the MediaItem that uses the given vehicle ID in the given plan.
        ///// </summary>
        ///// <param name="plan"></param>
        ///// <param name="mediaVehicleID"></param>
        ///// <returns></returns>
        //public static MediaItem MediaItemWithVehicleID( MediaPlan plan, int subItemIndex, Guid mediaVehicleID ) {
        //    foreach( MediaItem genericItem in plan.MediaItems ) {
        //        foreach( MediaItem subItem in genericItem.SubItems ) {
        //            if( subItem.MediaInfoID == mediaVehicleID ) {
        //                return subItem;
        //            }
        //        }
        //    }
        //    return null;
        //}


        /// <summary>
        /// Returns the state-to-DMA mapping dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<string>> StateToDma() {
            return stateToDma;
        }

        /// <summary>
        /// Returns the list of all USA states
        /// </summary>
        /// <returns></returns>
        public static List<string> AllUSAStates() {
            if( allStates != null ) {
                return allStates;
            }
            else {
                allStates = new List<string>();

                foreach( string st in stateToDma.Keys ) {
                    allStates.Add( st );
                }
                allStates.Sort();
            }
            return allStates;
        }

        /// <summary>
        /// Reads the state-to-DMA dictionary from the disk.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Dictionary<string, List<string>> ReadStateToDmaMap( string file ) {
            FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read );
            StreamReader sr = new StreamReader( fs );
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            string line = null;
            while( (line = sr.ReadLine()) != null ) {
                string[] items = line.Split( ',' );
                string state = items[ 0 ];
                string dma = items[ 1 ].Trim();

                if( dict.ContainsKey( state ) == false ) {
                    List<string> regionList = new List<string>();
                    regionList.Add( dma );
                    dict.Add( state, regionList );
                }
                else {
                    List<string> regionList = dict[ state ];
                    regionList.Add( dma );
                }
            }

            sr.Close();
            fs.Close();

            // alphabetize the DMAs
            foreach( string s in dict.Keys ) {
                List<string> regionList = dict[ s ];
                regionList.Sort();
            }

            return dict;
        }
        #endregion

        public static Color MediaBackgroundColor( MediaVehicle.MediaType mediaType ) {
            Color color = Color.Red;

            switch( mediaType ) {
                case MediaVehicle.MediaType.Radio:
                    color = Color.FromArgb( 0xAA, 0xFF, 0xAA );
                    break;
                case MediaVehicle.MediaType.Magazine:
                    color = Color.FromArgb( 0xAA, 0xAA, 0xFF );
                    break;
                case MediaVehicle.MediaType.Internet:
                    color = Color.FromArgb( 0xFF, 0xAA, 0xAA );
                    break;
                case MediaVehicle.MediaType.Yellowpages:
                    color = Color.FromArgb( 0xEE, 0xEE, 0x44 );
                    break;
                case MediaVehicle.MediaType.Newspaper:
                    color = Color.FromArgb( 0x77, 0x77, 0x77 );
                    break;
            }

            return color;
        }

        ///// <summary>
        ///// Returns the list of all available prominence choices for a given vehicle.
        ///// </summary>
        ///// <returns></returns>
        //public static List<int> MediaProminenceChoices( Guid vehicle_id, out List<string> fullProminenceNames, UserMedia userMedia) {
        //    MediaVehicle vehicle = GetMediaVehicleForID(vehicle_id);

        //    List<int> rval = new List<int>();
        //    fullProminenceNames = new List<string>();
        //    foreach (AdOption option in vehicle.GetOptions().Values)
        //    {
        //        rval.Add(option.ID);
        //        fullProminenceNames.Add(option.Name);
        //    }

        //    rval.Sort();

        //    return rval;
        //}

        /// <summary>
        /// Finds the plan with the given guid in the list.
        /// </summary>
        /// <param name="planID"></param>
        /// <param name="allPlans"></param>
        /// <returns></returns>
        public static MediaPlan PlanForID( string planID, List<MediaPlan> allPlans ) {
            return PlanForID( new Guid( planID ), allPlans );
        }

        /// <summary>
        /// Finds the plan with the given guid in the list.
        /// </summary>
        /// <param name="planID"></param>
        /// <param name="allPlans"></param>
        /// <returns></returns>
        public static MediaPlan PlanForID( Guid planID, List<MediaPlan> allPlans ) {
            foreach( MediaPlan plan in allPlans ) {
                if( plan.PlanID == planID ) {
                    return plan;
                }
            }
            return null;
        }

        public static MediaPlan PlanForID( Guid planID, List<MediaPlan> allPlans, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allVersions, string planOwner ) {
            if( PlanForID( planID, allPlans ) != null ) {
                return PlanForID( planID, allPlans );
            }

            foreach( string camp in allVersions.Keys ) {
                foreach( PlanStorage.PlanVersionInfo planVer in allVersions[ camp ] ) {
                    if( planVer.PlanID == planID ) {
                        // found it
                        PlanStorage storage = new PlanStorage();
                        MediaPlan plan = storage.LoadMediaPlan( planOwner, planID );
                        return plan;
                    }
                }
            }
            return null;
        }

        public static string PurchaseTypeForCode( int purchaseTypeCode ) {
            string s = null;
            switch( purchaseTypeCode ) {
                case 1:
                case 2:
                case 3:
                    s = "Impulse";
                    break;
                case 4:
                case 5:
                case 6:
                    s = "Habit";
                    break;
                case 7:
                case 8:
                case 9:
                    s = "Considered";
                    break;
            }
            return s;
        }

        public static string PurchaseCycleForCode( int purchaseCycleCode ) {
            string s = null;
            switch( purchaseCycleCode ) {
                case 1:
                case 2:
                case 3:
                    s = "Daily";
                    break;
                case 4:
                case 5:
                case 6:
                    s = "Weekly";
                    break;
                case 7:
                case 8:
                case 9:
                    s = "Monthly";
                    break;
                case 10:
                case 11:
                case 12:
                    s = "Yearly";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    s = "Years";
                    break;
            }
            return s;
        }

        public static double PurchaseCycleLengthForCode(int purchaseCycleCode)
        {
            double t = 30;
            switch (purchaseCycleCode)
            {
                case 1:
                case 2:
                case 3:
                    t = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    t = 7;
                    break;
                case 7:
                case 8:
                case 9:
                    t = 30;
                    break;
                case 10:
                case 11:
                case 12:
                    t = 365;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    t = 365*2.5;
                    break;
            }
            return t;
        }

        public static string BusinessSituationForCode( int bizSituationCode ) {
            string s = null;
            switch( bizSituationCode ) {
                case 1:
                    s = "Launch";
                    break;
                case 2:
                    s = "Steady State";
                    break;
                case 3:
                    s = "Growth";
                    break;
            }
            return s;
        }

        //public static double GetVehicleSize(Guid guid)
        //{
        //    List<GeoRegion> regions = new List<GeoRegion>();
        //    regions.Add(GeoRegion.TopGeo);

        //    return GetVehicleSize(guid, regions);
        //}

        //private static double GetVehicleSize(Guid guid, List<GeoRegion> regions)
        //{
        //    double size = 0.0;


        //    size = vehicle_size.Value * DpMediaDb.US_HH_SIZE;


        //    return size;
        //}

        //public static double GetVehicleSize(Guid guid, DemographicSettings demographic)
        //{
        //    double size = 0.0;
        //    List<GeoRegion> regions = new List<GeoRegion>();
        //    GeoRegion region = SimUtils.GeoRegionForName(demographic.DemographicRegionName);
        //    regions.Add(region);

        //    return GetVehicleSize(guid, regions);
        //}

        //public static double GetVehicleSize(Guid guid, List<DemographicSettings> demographics)
        //{
        //    double size = 0.0;
        //    List<GeoRegion> regions = new List<GeoRegion>();
        //    foreach (DemographicSettings demographic in demographics)
        //    {
        //        GeoRegion region = SimUtils.GeoRegionForName(demographic.DemographicRegionName);
        //        regions.Add(region);
        //    }

        //    return GetVehicleSize(guid, regions);
        //}

        ///// <summary>
        ///// Returns the price of an individual ad spot using the given vehicle and ad option.
        ///// </summary>
        ///// <param name="vehicle"></param>
        ///// <param name="adOptionID"></param>
        ///// <returns></returns>
        //public static double GetSpotPrice(MediaVehicle vehicle, int adOptionID, double size)
        //{
        //    SimpleOption option = null;
        //    if (vehicle.GetOptions().ContainsKey(adOptionID))
        //    {
        //        option = vehicle.GetOptions()[adOptionID] as SimpleOption;
        //    }

        //    if (option == null)
        //    {
        //        return 0.0;
        //    }

        //    return GetSpotPrice(vehicle.Type.ToString(), vehicle, option, size, 1);
        //}

        //public static double GetSpotPrice(MediaVehicle vehicle, int adOptionID, int num_ads, DemographicSettings demographics)
        //{
        //    List<GeoRegion> regions = new List<GeoRegion>();

        //    GeoRegion region = SimUtils.GeoRegionForName(demographics.DemographicRegionName);

        //    regions.Add(region);

        //    return GetSpotPrice(vehicle, adOptionID, num_ads, regions);
        //}

        //public static double GetSpotPrice(MediaVehicle vehicle, int adOptionID, int num_ads, List<GeoRegion> regions)
        //{
        //    SimpleOption option = null;
        //    if (vehicle.GetOptions().ContainsKey(adOptionID))
        //    {
        //        option = vehicle.GetOptions()[adOptionID] as SimpleOption;
        //    }

        //    if (option == null)
        //    {
        //        return 0.0;
        //    }

        //    string type = vehicle.Type.ToString();
        //    string subtype = vehicle.SubType;

        //    double size = 0.0;

        //    KeyValuePair<MediaVehicle, double> vehicle_size = Utils.MediaDatabase.GetMediaVehicleWithSize(vehicle.Guid, regions);

        //    size = vehicle_size.Value;

        //    double cost_per_spot = GetSpotPrice(type, vehicle, option, size, num_ads);

        //    return cost_per_spot;
        //}

        //public static double GetSpotPrice(string type, MediaVehicle vehicle, SimpleOption option, double size, double num_ads)
        //{
        //    return GetSpotPrice(type, vehicle.CPM, option.Cost_Modifier, size, num_ads);
        //}

        public static double GetSpotPrice(string type, double cpm, double cost_mod, double size, double num_ads)
        {
            double price_per_ad;
            double cost_per_spot;

            if (type == "Internet")
            {
                price_per_ad = cpm * cost_mod;
                cost_per_spot = price_per_ad;
            }
            else if (type == "Radio")
            {
                price_per_ad = cpm * cost_mod;
                cost_per_spot = size * price_per_ad;
            }
            else if (type == "Newspaper")
            {

                price_per_ad = cpm * cost_mod;
                cost_per_spot = size * price_per_ad;
            }
            else
            {
                price_per_ad = cpm * cost_mod;
                cost_per_spot = size * price_per_ad;
            }

            return cost_per_spot;
        }


        /// <summary>
        /// Returns the number of hours in the ad cycle
        /// </summary>
        /// <param name="adCycle"></param>
        /// <returns></returns>
        public static int HoursInAdCycle( MediaVehicle.AdCycle adCycle ) {
            int hoursInCycle = DaysInAdCycle( adCycle );
            if( adCycle != MediaVehicle.AdCycle.Hourly ) {
                hoursInCycle = hoursInCycle * 24;
            }
            return hoursInCycle;
        }

        /// <summary>
        /// Returns the number of days in the ad cycle
        /// </summary>
        /// <param name="adCycle"></param>
        /// <returns></returns>
        public static int DaysInAdCycle( MediaVehicle.AdCycle adCycle ) {
            int cycleDays = 0;
            switch( adCycle ) {
                case MediaVehicle.AdCycle.Instant:
                case MediaVehicle.AdCycle.Hourly:
                case MediaVehicle.AdCycle.Daily:
                    cycleDays = 1;
                    break;
                case MediaVehicle.AdCycle.Weekly:
                    cycleDays = 7;
                    break;
                case MediaVehicle.AdCycle.Bimonthly:
                    cycleDays = 14;
                    break;
                case MediaVehicle.AdCycle.Monthly:
                    cycleDays = 30;
                    break;
                case MediaVehicle.AdCycle.Quarterly:
                    cycleDays = 120;
                    break;
                case MediaVehicle.AdCycle.Yearly:
                    cycleDays = 365;
                    break;
            }
            return cycleDays;
        }

        /// <summary>
        /// Generates up to numAds ad spot dates for the plan, utilizing the constraints in the given media's flighting info and ad cycle
        /// </summary>
        /// <param name="flighting"></param>
        /// <param name="adCycle"></param>
        /// <param name="numAds"></param>
        /// <returns></returns>
        //public static List<DateTime> GenerateSpotDates( MediaItem mediaSubtypeItem, MediaVehicle.AdCycle adCycle, int numAds, DateTime startDate ) {

        //    List<DateTime> spotDates = new List<DateTime>();
        //    if( numAds == 0 ) {
        //        return spotDates;
        //    }

        //    List<SimUtils.MediaPulse> pulses = SimUtils.GetIndividualPulses( mediaSubtypeItem, mediaSubtypeItem.Flighting.TotalPlanDays );

        //    int cycleDays = Utils.DaysInAdCycle( adCycle );
        //    double cyclesPerPulse = mediaSubtypeItem.Flighting.PulseDays / cycleDays;

        //    if( mediaSubtypeItem.ItemName == "Radio" ) {
        //        // handle radio flighting 
        //        int nPerDay = 1;
        //        if( mediaSubtypeItem.Flighting.Continuous == false ) {
        //            nPerDay = (int)Math.Ceiling( mediaSubtypeItem.Flighting.AdsPerPulse / cyclesPerPulse );
        //        }
        //        else {
        //            // continuous flighting
        //            nPerDay = (int)Math.Ceiling( numAds / (double)mediaSubtypeItem.Flighting.TotalPlanDays );
        //        }

        //        for( int p = 0; p < pulses.Count; p++ ) {
        //            int pulseAdsRemaining = mediaSubtypeItem.Flighting.AdsPerPulse;
        //            if( mediaSubtypeItem.Flighting.Continuous == true ) {
        //                // there is just one pulse for continuous flighting
        //                pulseAdsRemaining = numAds;
        //            }
        //            DateTime d = startDate.AddDays( pulses[ p ].StartDay );

        //            for( int pulseDay = 0; pulseDay < mediaSubtypeItem.Flighting.PulseDays; pulseDay++ ) {
        //                for( int dayAdCnt = 0; dayAdCnt < nPerDay; dayAdCnt++ ) {
        //                    if( pulseAdsRemaining > 0 ) {
        //                        spotDates.Add( d );
        //                        pulseAdsRemaining -= 1;
        //                    }
        //                }
        //                if( pulseAdsRemaining <= 0 ) {
        //                    break;
        //                }
        //                d = d.AddDays( 1 );
        //            }
        //        }
        //        return spotDates;
        //    }
        //    else {
        //        // this is NOT a radio media

        //        // take the higher side of the roundiing here to be sure we try to place all of the requested spots
        //        int adsPerPulse = (int)Math.Ceiling( numAds / (double)pulses.Count );

        //        double cycleHours = cycleDays * 24.0;

        //        double nPerDay = 1;


        //        if( adCycle == MediaVehicle.AdCycle.Hourly ) {
        //            cyclesPerPulse *= FlightingInfo.ADVERTISING_HOURS_PER_DAY;
        //            cycleHours = 1;
        //        }

        //        // put in either all we can afford, or all the spots that are available, whichever is lower
        //        int nPerPulse = (int)Math.Min( adsPerPulse, cyclesPerPulse );

        //        int adsRemaining = numAds;

        //        // loop over pulses - a continuous plan has one big pulse
        //        for( int p = 0; p < pulses.Count; p++ ) {
        //            DateTime d = startDate.AddDays( pulses[ p ].StartDay );
        //            spotDates.Add( d );
        //            adsRemaining -= 1;

        //            int pulseAdsRemaining = nPerPulse - 1;
        //            //int pulseAdsRemaining = mediaSubtypeItem.Flighting.AdsPerPulse;

        //            if( nPerPulse > 1 ) {
        //                for( int p2 = 1; p2 < nPerPulse; p2++ ) {
        //                    d = d.AddHours( cycleHours );
        //                    spotDates.Add( d );
        //                    adsRemaining -= 1;
        //                    pulseAdsRemaining -= 1;
        //                    if( adsRemaining <= 0 || pulseAdsRemaining <= 0 ) {
        //                        break;
        //                    }
        //                }
        //            }

        //            if( adsRemaining <= 0 ) {
        //                break;
        //            }
        //        }
        //        return spotDates;
        //    }
        //}

        public static void FillSubtypesMenu( DropDownList menuDropDown, List<string> subTypes, string topItemString ) {
            // add the subtypes to their dropdown list after sorting by name
            string[] sortNames = new string[ subTypes.Count ];
            ListItem[] sortItems = new ListItem[ subTypes.Count ];
            for( int i = 0; i < subTypes.Count; i++ ) {
                sortNames[ i ] = MakeSafe( subTypes[ i ] );
                sortItems[ i ] = new ListItem( MakeSafe( subTypes[ i ] ) );
            }
            Array.Sort( sortNames, sortItems );

            menuDropDown.Items.Clear();
            if( topItemString != null ) {
                ListItem topItem = new ListItem( topItemString, "" );
                menuDropDown.Items.Add( topItem );
            }
            if( sortItems.Length > 0 ) {
                for( int i = 0; i < sortItems.Length; i++ ) {
                    menuDropDown.Items.Add( sortItems[ i ] );
                }
            }
            else {
                ListItem noSubtypestem = new ListItem( noSubtypesLine, "" );
                menuDropDown.Items.Add( noSubtypestem );
            }
        }

        public static void FillVehiclesMenu( DropDownList menuDropDown, List<MediaVehicle> allVehicles ) {
            // sort the vehicles by name
            string[] sortVNames = new string[ allVehicles.Count ];
            ListItem[] sortVItems = new ListItem[ allVehicles.Count ];
            for( int i = 0; i < allVehicles.Count; i++ ) {
                MediaVehicle info = allVehicles[ i ];
                string vname = FixVehicleCapitalization( info.Vehicle );
                sortVNames[ i ] = MakeSafe( vname );
                sortVItems[ i ] = new ListItem( MakeSafe( vname ), info.Guid.ToString() );
            }
            Array.Sort( sortVNames, sortVItems );

            // populate the dropdown list of vehicles
            menuDropDown.Items.Clear();
            if( sortVItems.Length > 0 ) {

                ListItem firstVehicleItem = new ListItem( firstVehicleLine, "" );
                menuDropDown.Items.Add( firstVehicleItem );

                for( int i = 0; i < sortVItems.Length; i++ ) {
                    menuDropDown.Items.Add( sortVItems[ i ] );
                }
            }
            else {
                ListItem noVehiclesItem = new ListItem( noVehiclesLine, "" );
                menuDropDown.Items.Add( noVehiclesItem );
            }
        }

        public const string MaxLengthPlanName = "Maximum-Length-NameXXXXX";

        public static string ElideString( string str, string maxLengthString ) {
            IntPtr serverDesktopHwnd = (IntPtr)0;
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd( serverDesktopHwnd );
            System.Drawing.Font font = new System.Drawing.Font( "Arial", 9 );
            System.Drawing.SizeF sz = g.MeasureString( str, font );
            System.Drawing.SizeF szMax = g.MeasureString( maxLengthString, font );
            if( sz.Width <= szMax.Width ) {
                return str;
            }
            else {
                System.Drawing.SizeF sze;
                string elidedStr = null;
                do {
                    str = str.Substring( 0, str.Length - 1 );
                    elidedStr = str + "...";
                    sze = g.MeasureString( elidedStr, font );
                } while( sze.Width > szMax.Width && elidedStr.Length > 6 );

                return elidedStr;
            }
        }

        public static string MakeSafe( string s ) {
            string s2 = s.Replace( "\"", "" );
            string s2a = s2.Replace( "'", "" );
            string s3 = s2a.Replace( ">", "" );
            string s4 = s3.Replace( "<", "" );
            return s4;
        }

        /// <summary>
        /// Changes the all-caps names of a media vehicle name to a display name
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        public static string FixVehicleCapitalization( string vehicleName ) {
            if( vehicleName.Length <= 4 && (vehicleName.StartsWith( "K" ) || vehicleName.StartsWith( "W" )) ) {
                // probably a radio station call letters -- leave alone
                return vehicleName;
            }
            if( (vehicleName.Length == 6 || vehicleName.Length == 7) && (vehicleName.EndsWith( "-FM" ) || vehicleName.EndsWith( "-AM" )) ) {
                // probably a radio station call letters -- leave alone
                return vehicleName;
            }

            string s = "";
            string[] words = vehicleName.Split( ' ' );
            for( int w = 0; w < words.Length; w++ ) {
                if( words[ w ].Length == 0 ) {
                    continue;
                }

                s += words[ w ].ToUpper().Substring( 0, 1 );
                if( words[ w ].Length > 1 ) {
                    s += words[ w ].ToLower().Substring( 1 );
                }
                s += " ";
            }
            return s;
        }

        public static string VehicleInfoLink( MediaVehicle vehicle ) {
            string itemWebsite = vehicle.URL.Trim();
            if( itemWebsite == "" && vehicle.Vehicle.ToLower().EndsWith( ".com" ) ) {
                itemWebsite = vehicle.Vehicle;
            }
            return VehicleInfoLink( MakeSafe( vehicle.Vehicle ), vehicle.Type, MakeSafe( vehicle.SubType ), itemWebsite, MakeSafe( vehicle.RegionName ) );
        }

        public static string VehicleInfoLink( MediaItem item ) {
            string itemWebsite = item.URL.Trim();
            if( itemWebsite == "" && item.VehicleName.ToLower().EndsWith( ".com" ) ) {
                itemWebsite = item.VehicleName;
            }
            return VehicleInfoLink( MakeSafe( item.VehicleName ), item.MediaType, MakeSafe( item.MediaSubtype ), itemWebsite, MakeSafe( item.Region ) );
        }

        public static string VehicleInfoLink( string vName, MediaVehicle.MediaType vType, string vSubType, string vUrl, string vRegion  ) {

            vSubType = FixSubtypeName( vSubType, vType, vName ).ToUpper();
            if( vRegion == "USA" ) {
                vRegion = "Entire USA";
            }

            string click_JS = String.Format( "PrepInfoPopup( '{0}', '{1}', '{2}', '{3}', '{4}', true );",
                vName, vType, vSubType, vUrl, vRegion );
            string over_JS = String.Format( "PrepInfoPopup( '{0}', '{1}', '{2}', '{3}', '{4}', false );",
                vName, vType, vSubType, vUrl, vRegion );
            string out_JS = "DismissInfoPopup();";

            string s = String.Format( "<a href='#' onclick=\"{0}\" onmouseover=\"{1}\" onmouseout=\"{2}\" class=\"MediaItemLink\" >{3}</a>", click_JS, over_JS, out_JS, vName );

            return s;
        }

        public static string FixSubtypeName( string subtypeName, MediaVehicle.MediaType mediaType, string vehicleName ) {
            string s = subtypeName.ToLower();

            if( s == "communityweekly" ) {
                s = "community weekly";
            }
            else if( s == "adnetwork" ) {
                s = "ad network";
            }
            else if( s == "generalinterest" ) {
                s = "general interest";
            }
            else if( s == "adultcontemporary" ) {
                s = "adult contemporary";
            }
            else if( s == "contemporaryhitsradio" ) {
                s = "contemporary hits radio";
            }
            else if( s == "at&tcity" ) {
                s = "AT&T city";
            }
            else if( s == "remainingformats" ) {
                s = "unspecified";
            }

            if( s.IndexOf( "smoothjazz" ) != -1 ) {
                s = s.Replace( "smoothjazz", "smooth jazz" );
            }
            if( s.IndexOf( "newac" ) != -1 ) {
                s = s.Replace( "newac", "new adult contemp." );
            }

            if( s.IndexOf( "&" ) != -1 && s.IndexOf( " & " ) == -1 && s.ToUpper().IndexOf( "AT&T" ) == -1 ) {
                s = s.Replace( "&", " & " );
            }
            if( s.IndexOf( "/" ) != -1 && s.IndexOf( " / " ) == -1 ) {
                s = s.Replace( "/", " / " );
            }

            if( mediaType == MediaVehicle.MediaType.Newspaper && vehicleName.ToUpper().EndsWith( "SUNDAY" ) && s == "daily" ) {
                s = "daily (Sunday)";
            }
            return s;
        }


        #region Development Methods
        public static string[] AllFlightingFrequencies() {
            return new string[] { 
            "Daily",
            "Weekly",
            "Monthly"
        };
        }

        public static string[] AllFlightingDutyCycles() {
            return new string[] { 
            "Narrow",
            "Wide",
            "Continuous"
        };
        }


        /// <summary>
        /// Returns an HTML string describing the current media plan for the given page.  For debug/development use.
        /// </summary>
        /// <param name="pageWithMediaPlan"></param>
        /// <returns></returns>
        public static string GetDebugData( Page pageWithMediaPlan ) {
            string planName = "???";
            string planCost = "???";
            string planTime = "???";
            string planRegion = "???";
            List<DemographicSettings> demos = null;

            MediaPlan plan;

            if( pageWithMediaPlan.Session[ "CurrentMediaPlan" ] != null ) {
                plan = (MediaPlan)pageWithMediaPlan.Session[ "CurrentMediaPlan" ];

                planName = plan.PlanDescription;

                planCost = String.Format( "{0:f0}", plan.TargetBudget );

                DateTime planStart = plan.StartDate;
                DateTime planEnd = plan.EndDate;
                planTime = String.Format( "Start: {0}   End: {1}", planStart.ToString( "MMM d, yyyyy" ), planEnd.ToString( "MMM d, yyyyy" ) );

                ////if( plan.RegionCity == "" ) {
                ////    planRegion = String.Format( "Region Code: {0}  City: -", plan.RegionStateCode );
                ////}
                ////else {
                ////    planRegion = String.Format( "Region Code: {0}  City: {1}", plan.RegionStateCode, plan.RegionCity );
                ////}

                demos = plan.Specs.Demographics;
            }

            string s = "Search Parameters: <br>";
            s += String.Format( "Name: {0}   <br>Cost: ${1}   <br>Duration: {2}", planName, planCost, planTime );
            s += String.Format( "<br>{0}", planRegion );

            if( demos != null ) {
                foreach( DemographicSettings demoGroup in demos ) {
                    string ss = "";
                    foreach( DemographicGroupValues demo in demoGroup.Values ) {
                        ss += demo.GroupName + ":";
                        for( int i = 0; i < demo.Values.Length; i++ ) {
                            ss += String.Format( "{0};", demo.Values[ i ] );
                        }
                        ss += " ";
                    }
                    s += "<br>" + ss;
                }
            }
            return s;
        }
        #endregion
    }
}