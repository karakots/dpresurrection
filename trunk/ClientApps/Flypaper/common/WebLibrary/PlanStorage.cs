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
using System.IO;

using System.Xml.Serialization;

using System.Runtime.Serialization.Formatters.Binary;

using WebLibrary.Timing;
using MediaLibrary;

namespace WebLibrary
{
    /// <summary>
    /// PlanStorage is the access point for permanent storage of media plans.
    /// </summary>
    public class PlanStorage
    {
        private static string baseFolder = "SavedPlans";
        private static string fileExtension = "mpd";
        private static string nameVersionSeparator = "-.-";
        private static string newItemFileExtension = "txt";

        /// <summary>
        /// Configures the plan-storage system.
        /// </summary>
        /// <param name="baseFolderName"></param>
        public static void Init() {
            baseFolder = ConfigurationSettings.AppSettings[ "AdPlanIt.UserPlanStorageRoot" ];
        }

        public List<MediaPlan> LoadCurrentPlans( string user, out Dictionary<string, List<PlanVersionInfo>> campaignPlanVersions,
            ref List<UserMedia.CurrentPlanVersion> currentPlanVersions, out bool curVersionChanged ) {

            return LoadCurrentPlans( user, out campaignPlanVersions, ref currentPlanVersions, out curVersionChanged, null );
        }

        public List<MediaPlan> LoadCurrentPlans( string user, out Dictionary<string, List<PlanVersionInfo>> campaignPlanVersions,
            ref List<UserMedia.CurrentPlanVersion> currentPlanVersions, out bool curVersionChanged, Guid? specificID ) {

            curVersionChanged = false;

            Dictionary<string, string> currentPlanVersionsDict = new Dictionary<string, string>();

            campaignPlanVersions = new Dictionary<string, List<PlanVersionInfo>>();
            List<string> campaignNames = new List<string>();

            List<string> campaignsWithValidVersions = new List<string>();
            List<MediaPlan> campaignPlans = new List<MediaPlan>();

            if( currentPlanVersions == null ) {
                currentPlanVersions = new List<UserMedia.CurrentPlanVersion>();
                curVersionChanged = true;
            }

            string userPlanFolder = FolderForPlans( user );

            if( Directory.Exists( userPlanFolder ) == false ) {
                // the user doesn't even have a folder yet
                return campaignPlans;
            }

            // get the files in the folder
            string[] planFiles = Directory.GetFiles( userPlanFolder, "*." + fileExtension );
            for( int i = 0; i < planFiles.Length; i++ ) {
                MediaPlan loadedPlan = MediaPlan.LoadPlan( planFiles[ i ] );
                if( loadedPlan == null ) {
                    continue;
                }

                // see if we are seeking just a specific plan, by ID
                if( specificID != null && loadedPlan.PlanID == specificID ) {
                    campaignPlans.Add( loadedPlan );
                    return campaignPlans;
                }

                if( campaignNames.Contains( loadedPlan.CampaignName ) == false ) {
                    // this is the first plan encountered from this campaign
                    campaignNames.Add( loadedPlan.CampaignName );
                    campaignPlanVersions.Add( loadedPlan.CampaignName, new List<PlanVersionInfo>() );
                }

                // add plan version to the appropriate versions list
                List<PlanVersionInfo> planVersions = campaignPlanVersions[ loadedPlan.CampaignName ];
                string planVersion = loadedPlan.PlanVersion;
                bool existingVersion = false;
                foreach( PlanVersionInfo info in planVersions ) {
                    if( info.Version == planVersion ) {
                        existingVersion = true;
                        break;
                    }
                }
                if( existingVersion == false ) {
                    planVersions.Add( new PlanVersionInfo( planVersion, loadedPlan.ModificationDate, loadedPlan.PlanOverallRatingStars, 
                        loadedPlan.PlanDescription, loadedPlan.SumOfItemBudgets, loadedPlan.PlanID, loadedPlan.Competitor ) );
                }

                // hang on to this plan if it is the current version
                foreach( UserMedia.CurrentPlanVersion curVer in currentPlanVersions ) {
                    if( curVer.CampaignName == loadedPlan.CampaignName ) {
                        if( curVer.Version == planVersion ) {

                            // update old-style plans to new version 
                            UpdateIfOldPlan( loadedPlan );

                            campaignsWithValidVersions.Add( loadedPlan.CampaignName );
                            campaignPlans.Add( loadedPlan );
                        }
                    }
                }
            }

            // make sure each plan has a valid current version (
            foreach( string campaignName in campaignNames ) {
                if( campaignsWithValidVersions.Contains( campaignName ) == false )
                {
                    // add a new campaign/plan to the currentPlanVersions list
                    PlanVersionInfo validVersionInfo = campaignPlanVersions[ campaignName ][ 0 ];
                    string validVersion = validVersionInfo.Version;
                    bool found = false;
                    foreach( UserMedia.CurrentPlanVersion curVer in currentPlanVersions ) {
                        if( curVer.CampaignName == campaignName ) {
                            found = true;
                            curVer.Version = validVersion;
                        }
                    }
                    if( found == false ) {
                        currentPlanVersions.Add( new UserMedia.CurrentPlanVersion( campaignName, validVersion ) );
                    }

                    curVersionChanged = true;
                    // load the newly-designated current version
                    MediaPlan curPlan = LoadMediaPlan( user, campaignName, validVersion );
                    campaignPlans.Add( curPlan );
                }
            }

            return campaignPlans;
        }

        /// <summary>
        /// Saves the given media plan to permanent storage.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public void SaveMediaPlan( string user, MediaPlan planData ) {
            string savePath = PathForPlan( user, planData.CampaignName, planData.PlanVersion );
            planData.Edited = false;
            planData.Save( savePath );
        }

        public static DirectoryInfo MoviePath( string user, MediaPlan planData )
        {
            string path = String.Format( "{0}/{1}/AgentLogs/{2}{3}{4}", baseFolder, EncodedUserName( user ), planData.CampaignName, nameVersionSeparator, planData.PlanVersion );

            return new DirectoryInfo( path );
        }

        public static void SaveMovieData(DirectoryInfo di, Dictionary<int, AgentLog> data)
        {
            if( di.Exists == false )
            {
                // we need to create the directory first
                di.Create();
            }

            BinaryFormatter serialization = new BinaryFormatter();

            Dictionary<int, AgentLog.Summary> agents = new Dictionary<int, AgentLog.Summary>();
             FileStream fs = null;

            foreach( int key in data.Keys )
            {
                AgentLog log = data[key];

                agents.Add( key, log.ComputeSummary() );

                string fileName = di.FullName + @"/agent" + key.ToString() + ".dat";

               fs = new FileStream( fileName, FileMode.Create, FileAccess.Write );

                serialization.Serialize( fs, log );

                fs.Flush();
                fs.Close();
            }

            // serialize Agent Data to a file
            string agentFile = di.FullName + @"/agents.dat";

            fs = new FileStream( agentFile, FileMode.Create, FileAccess.Write );

            serialization.Serialize( fs, agents );

            fs.Flush();
            fs.Close();
        }

        public static AgentLog ReadAgentLog(  DirectoryInfo di, int key )
        {
            AgentLog rval = null;


            if( di.Exists == false )
            {
                return rval;
            }

            if( di.Exists )
            {
                string fileName = di.FullName + @"/agent" + key.ToString() + ".dat";

                FileStream fs = new FileStream( fileName, FileMode.Open, FileAccess.Read );

                BinaryFormatter serialization = new BinaryFormatter();

                rval = (AgentLog) serialization.Deserialize( fs );

                fs.Close();
            }

            return rval;

        }

        public static Dictionary<int, AgentLog.Summary> ReadAgentData( DirectoryInfo di )
        {
            Dictionary<int, AgentLog.Summary> rval = null;

            if( di.Exists == false )
            {
                return rval;
            }

            if( di.Exists )
            {
                FileStream fs = new FileStream( di.FullName + @"/agents.dat", FileMode.Open, FileAccess.Read );

                BinaryFormatter serialization = new BinaryFormatter();

                rval = (Dictionary<int, AgentLog.Summary>)serialization.Deserialize( fs );

                fs.Close();
            }

            return rval;
        }

        /// <summary>
        /// Loads the named media plan from permanent storage.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planName"></param>
        /// <returns></returns>
        public MediaPlan LoadMediaPlan( string user, string campaignName, string planVersion ) {
            string loadPath = PathForPlan( user, campaignName, planVersion );
            MediaPlan loadedPlan = MediaPlan.LoadPlan( loadPath );

            if( loadedPlan.Results == null ) {
                // make sure the stars rating has been cleared for plans w/no results (normally should be the case already)
                loadedPlan.PlanOverallRatingStars = -1;
            }

            // update old-style plans to new version 
            UpdateIfOldPlan( loadedPlan );

            return loadedPlan;
        }
        
        /// <summary>
        /// Loads the specified media plan from permanent storage.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planName"></param>
        /// <returns></returns>
        public MediaPlan LoadMediaPlan( string user, Guid planID ) {
            Dictionary<string, List<PlanVersionInfo>> aDum = null;
            List<UserMedia.CurrentPlanVersion> cDum = new List<UserMedia.CurrentPlanVersion>();
            bool bDum = false;
            List<MediaPlan> plans = LoadCurrentPlans( user, out aDum, ref cDum, out bDum, planID );
            if( plans != null && plans.Count == 1 ) {
                return plans[ 0 ];
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Increments the version number of the given plan, adds the new version to the campagn versions list, and sets the new version as current.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="campaignPlanVersions"></param>
        /// <param name="currentVersions"></param>
        /// <returns></returns>
        public void IncrementPlanVersion( MediaPlan plan, Dictionary<string, List<PlanVersionInfo>> campaignPlanVersions, List<UserMedia.CurrentPlanVersion> currentVersions ) {

            string version = plan.PlanVersion;

            // make sure there is a versions list for this plan
            if( campaignPlanVersions.ContainsKey( plan.CampaignName ) == false ) {
                campaignPlanVersions.Add( plan.CampaignName, new List<PlanVersionInfo>() );
            }

            string nextVersion = GetNextVersion( version, campaignPlanVersions[ plan.CampaignName ], plan.PlanDescription );     // also adds to the list, which is part of campaignPlanVersions

            plan.PlanVersion = nextVersion;
            plan.PlanID = Guid.NewGuid();

            foreach( UserMedia.CurrentPlanVersion curVer in currentVersions ) {
                if( curVer.CampaignName == plan.CampaignName ) {
                    curVer.Version = nextVersion;
                }
            }
        }

        /// <summary>
        /// Gets the next availablle version level
        /// </summary>
        /// <param name="version"></param>
        /// <param name="planVersions"></param>
        /// <returns></returns>
        private string GetNextVersion( string version, List<PlanVersionInfo> planVersions, string planDescription ) {
            int branchLevel = 0;
            string nextVersion = null;
            List<string> curVersions = new List<string>();
            foreach( PlanVersionInfo vinfo in planVersions ) {
                curVersions.Add( vinfo.Version );
            }

            do {
                nextVersion = NextVersion( version, branchLevel );
                branchLevel += 1;
            }
            while( curVersions.Contains( nextVersion ) );

            PlanVersionInfo nextVersionInfo = new PlanVersionInfo( nextVersion, DateTime.Now, -1, planDescription, 0, null, null );
            planVersions.Add( nextVersionInfo );
            return nextVersion;
        }

        /// <summary>
        /// Generates the next version number for the given current version and branch level
        /// </summary>
        /// <param name="curVersion"></param>
        /// <param name="branchLevel"></param>
        /// <returns></returns>
        private string NextVersion( string curVersion, int branchLevel ) {
            string[] verStrs = curVersion.Split( '.' );

            if( branchLevel < verStrs.Length ) {
                // case 1: increment an existing version number
                string curSubVer = verStrs[ branchLevel ];
                int subVer = int.Parse( curSubVer );
                subVer += 1;
                verStrs[ branchLevel ] = subVer.ToString();

                // regenerate the version string
                string nextVer = "";
                for( int i = 0; i < verStrs.Length; i++ ) {
                    nextVer += verStrs[ i ];
                    if( i < verStrs.Length - 1 ) {
                        nextVer += ".";
                    }
                }
                return nextVer;
            }
            else if( branchLevel == verStrs.Length ) {
                // case 2: start a new branch
                return curVersion + ".1";
            }
            else {     // branchLevel is > verStrs.Length
                string nextVer = curVersion;
                for( int i = 0; i < branchLevel - verStrs.Length; i++ ) {
                    nextVer += ".0";
                }
                nextVer += ".1";
                return nextVer;
            }
        }

        /// <summary>
        /// Deletes the given media campaign from permanent storage.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public void DeleteMediaCampaign( string user, string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, List<MediaPlan> currentPlans ) {

            for( int c = currentPlans.Count - 1; c >= 0; c-- ) {
                if( currentPlans[ c ].CampaignName == campaignName ) {
                    currentPlans.RemoveAt( c );
                }
            }

            List<PlanVersionInfo> vers = allPlanVersions[ campaignName ];
            foreach( PlanVersionInfo ver in vers ) {
                string delPath = PathForPlan( user, campaignName, ver.Version );
                if( File.Exists( delPath ) ) {
                    File.Delete( delPath );
                }
            }

            allPlanVersions.Remove( campaignName );
        }

        /// <summary>
        /// Copies the given media campaign in the permanent storage area.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public void CopyMediaCampaign( string user, string campaignName, string copiedCampaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, List<MediaPlan> currentPlans ) {

            List<PlanVersionInfo> vers = allPlanVersions[ campaignName ];
            MediaPlan copyPlan = null;
            List<PlanVersionInfo> copyVers = new List<PlanVersionInfo>();

            foreach( PlanVersionInfo ver in vers ) {
                string srcPath = PathForPlan( user, campaignName, ver.Version );
                if( File.Exists( srcPath ) ) {
                    copyVers.Add( ver );
                    copyPlan = LoadMediaPlan( user, campaignName, ver.Version );

                    //change the name of the campaign
                    copyPlan.Specs.CampaignName = copiedCampaignName;

                    // clear the simulation results, if any
                    copyPlan.Results = null;
                    copyPlan.PlanOverallRatingStars = -1;
                    copyPlan.PlanID = Guid.NewGuid();

                    SaveMediaPlan( user, copyPlan );            // save the copied plan
                }
            }
            allPlanVersions.Add( copiedCampaignName, copyVers );

            if( copyPlan != null ) {
                // just add the last media plan encountered, since we don't have any current version info for the copied campaign
                currentPlans.Add( copyPlan );
            }
        }

        /// <summary>
        /// Renames a media campaign in the permanent storage area (changes the campaign name of all plans using that campaign).
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public void RenameMediaCampaign( string user, string oldName, string newName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, List<MediaPlan> currentPlans ) {

            // change the entry in the current-plans list
            foreach( MediaPlan curPlan in currentPlans ) {
                if( curPlan.CampaignName == oldName ) {
                    curPlan.Specs.CampaignName = newName;
                    break;
                }
            }

            // change each file in the campaign on disk to have the new campaign name
            List<PlanVersionInfo> vers = allPlanVersions[ oldName ];
            MediaPlan renamedPlan = null;
            List<PlanVersionInfo> renameVers = new List<PlanVersionInfo>();

            foreach( PlanVersionInfo ver in vers ) {
                string srcPath = PathForPlan( user, oldName, ver.Version );
                if( File.Exists( srcPath ) ) {
                    renameVers.Add( ver );
                    renamedPlan = LoadMediaPlan( user, oldName, ver.Version );

                    //change the name of the campaign
                    renamedPlan.Specs.CampaignName = newName;

                    File.Delete( srcPath );                                // the plan file wil have a different name!
                    SaveMediaPlan( user, renamedPlan );            // save the renamed plan
                }
            }

            // update the versions dictionary
            allPlanVersions.Remove( oldName );
            allPlanVersions.Add( newName, renameVers );
        }

        /// <summary>
        /// Updates a media campaign in the permanent storage area (changes the campaign data of all plans using that campaign).  NOTE the campaign name should NOT be changed by this method!
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public void UpdateMediaCampaign( string user, MediaPlan modelPlan, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, List<MediaPlan> currentPlans ) {

            //bool renaming = (origCampaignName != modelPlan.CampaignName);

            //if( renaming ) {
            //    // change the entry in the current-plans list
            //    foreach( MediaPlan curPlan in currentPlans ) {
            //        if( curPlan.CampaignName == origCampaignName ) {
            //            curPlan.Specs.CampaignName = modelPlan.CampaignName;
            //            break;
            //        }
            //    }
            //}

            // change each file in the campaign on disk to have the new campaign name

            if( allPlanVersions.ContainsKey( modelPlan.CampaignName ) )
            {
                List<PlanVersionInfo> vers = allPlanVersions[modelPlan.CampaignName];
                MediaPlan updatedPlan = null;
                //List<PlanVersionInfo> updateVers = new List<PlanVersionInfo>();

                foreach( PlanVersionInfo ver in vers )
                {
                    if( ver.Version == modelPlan.PlanVersion )
                    {
                        //updateVers.Add( ver );
                        continue;
                    }
                    string srcPath = PathForPlan( user, modelPlan.CampaignName, ver.Version );
                    if( File.Exists( srcPath ) )
                    {
                        //updateVers.Add( ver );
                        updatedPlan = LoadMediaPlan( user, modelPlan.CampaignName, ver.Version );

                        //change the data for of the campaign
                        updatedPlan.CopyCampaignDataFrom( modelPlan.Specs );

                        //if( renaming ) {
                        //    File.Delete( srcPath );                                // the plan file wil have a different name!
                        //}
                        SaveMediaPlan( user, updatedPlan );            // save the renamed plan
                    }
                }
            }

            //if( renaming ) {
            //    // update the versions dictionary
            //    allPlanVersions.Remove( origCampaignName );
            //    allPlanVersions.Add( updatedPlan.CampaignName, updateVers );
            //}
        }

        /// <summary>
        /// Returns true if none of the plans associated with this campaign have results data.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="campaignName"></param>
        /// <param name="allPlanVersions"></param>
        /// <returns></returns>
        public bool AllPlansInCampaignHaveNoResults( string user, string campaignName, Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, List<MediaPlan> currentPlans ) {

            MediaPlan curVersion = null;
            foreach( MediaPlan plan in currentPlans ){
                if( plan.CampaignName == campaignName ){
                    curVersion = plan;
                    if( curVersion.Results != null ) {
                        // we're done if the current version has results
                        return false;
                    }
                    break;
                }
            }

            if( allPlanVersions.ContainsKey( campaignName ) ) {

                List<PlanVersionInfo> vers = allPlanVersions[ campaignName ];

                foreach( PlanVersionInfo ver in vers ) {
                    if( curVersion == null || curVersion.PlanVersion != ver.Version ) {
                        // load non-current versions from disk so we can check them too
                        if( ver.StarsRating != -1.0 ) {
                            // the stars rating will be -1 if there are results
                            return false;
                        }
                    }
                }
            }

            // if we get here we didn't find any results
            return true;
        }

        /// <summary>
        /// Deletes the given media plan from permanent storage.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="planData"></param>
        public bool DeleteMediaPlan( string user, string campaignName, string planVersion ) {
            bool deleted = false;
            string delPath = PathForPlan( user, campaignName, planVersion );

            if( File.Exists( delPath ) ) {
                File.Delete( delPath );
                deleted = true;
            }
            else {
                // ignore repeated atempts to delete same plan
                //throw new Exception( String.Format( "Error: Attempt to delete non-existent media plan file \"{0}\"", delPath ) );
            }

            return deleted;
        }

        /// <summary>
        /// Returns the path of the folder for a user's stored plans.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string FolderForPlans( string userName ) {
            string path = String.Format( "{0}/{1}", baseFolder, EncodedUserName( userName ) );
            return path;
        }

        /// <summary>
        /// Returns the path to the file for the specified media plan.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="planName"></param>
        /// <returns></returns>
        private string PathForPlan( string userName, string campaignName, string planVersion ) {
            string path = String.Format( "{0}/{1}/{2}{3}{4}.{5}", baseFolder, EncodedUserName( userName ), campaignName, nameVersionSeparator, planVersion, fileExtension );
            return path;
        }

        /// <summary>
        /// Changes the user name (which is an email address) to a name suitable for a directory.  
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string EncodedUserName( string userName ) {

            string uname = userName;
            // replace the @ sign with a pair pf dashes
            if( uname.IndexOf( "@" ) != -1 ) {
                int atIndx = uname.IndexOf( "@" );
                uname = uname.Substring( 0, atIndx ) + "--" + uname.Substring( atIndx + 1 );
            }
            // replace dots with dashes
            uname = uname.Replace( '.', '-' );

            return uname;
        }

        #region METHODS FOR USER-DEFINED ITEMS
        //
        // Commented out - for clarity
        // SSN 9/6/2008
        //
        ///// <summary>
        ///// Saves the given media-vehicle spec to the "in-box" for review and potential addition to the system.
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="planData"></param>
        //public static void SaveNewVehicle( UserMedia.MediaVehicleSpec newVehicleSpec ) {
        //    string savePath = PathForNewVehicle( newVehicleSpec.ItemID );
        //    newVehicleSpec.Save( savePath );
        //}

        ///// <summary>
        ///// Saves the given media prominence spec to the "in-box" for review and potential addition to the system.
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="planData"></param>
        //public static void SaveNewProminence( UserMedia.MediaSpotSpec newSpotSpec ) {
        //    string savePath = PathForNewProminence( newSpotSpec.SpotID );
        //    newSpotSpec.Save( savePath );
        //}

        ///// <summary>
        ///// Saves the given media package spec to the "in-box" for review and potential addition to the system.
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="planData"></param>
        //public static void SaveNewPackage( UserMedia.MediaPackageSpec newPackageSpec ) {
        //    string savePath = PathForNewPackage( newPackageSpec.PackageID );
        //    newPackageSpec.Save( savePath );
        //}

        ///// <summary>
        ///// Returns the path to the file for the specified new media vehicle
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="planName"></param>
        ///// <returns></returns>
        //private static string PathForNewVehicle( Guid newVehicleID ) {
        //    string path = String.Format( "{0}/wiki/vehicles/{1}.{2}", baseFolder, newVehicleID.ToString(), newItemFileExtension );
        //    return path;
        //}        
        
        ///// <summary>
        ///// Returns the path to the file for the specified new media prominence
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="planName"></param>
        ///// <returns></returns>
        //private static string PathForNewProminence( Guid newVehicleID ) {
        //    string path = String.Format( "{0}/wiki/options/{1}.{2}", baseFolder, newVehicleID.ToString(), newItemFileExtension );
        //    return path;
        //}        
        
        ///// <summary>
        ///// Returns the path to the file for the specified new media package
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="planName"></param>
        ///// <returns></returns>
        //private static string PathForNewPackage( Guid newVehicleID ) {
        //    string path = String.Format( "{0}/wiki/packages/{1}.{2}", baseFolder, newVehicleID.ToString(), newItemFileExtension );
        //    return path;
        //}
#endregion


        /// <summary>
        /// Returrns the list of PlanVersionInfo objects, in sequence of versions unless sortByDate or sortByScore are true.
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        public static List<PlanVersionInfo> SortPlanVersionInfo( List<PlanVersionInfo> orig, bool ascending, string sortKey ) {
            PlanVersionInfo[] infoArray = new PlanVersionInfo[ orig.Count ];
            orig.CopyTo( infoArray );

            object[] sortKeys = new object[ orig.Count ];
            bool needsReverse = false;

            for( int i = 0; i < orig.Count; i++ ) {

                if( sortKey == "score" ) {
                    sortKeys[ i ] = orig[ i ].StarsRating;
                    needsReverse = true;
                }
                else if( sortKey == "date" ) {
                    sortKeys[ i ] = orig[ i ].ModificationDate;
                }
                else if( sortKey == "name" ) {
                    sortKeys[ i ] = orig[ i ].Description;
                }
                else if( sortKey == "version" ) {
                    // sort by version number
                    string v = orig[ i ].Version;
                    if( v.IndexOf( "." ) == -1 ) {
                        // a simple version
                        sortKeys[ i ] = double.Parse( v );
                    }
                    else {
                        string[] va = v.Split( '.' );
                        double multiplier = 1;
                        double accVal = 0;
                        for( int vn = 0; vn < va.Length; vn++ ) {
                            accVal += double.Parse( va[ vn ] ) * multiplier;
                            multiplier /= 1000;       // handle a max of 999 versions per branch level
                        }
                        sortKeys[ i ] = accVal;
                    }
                }
            }

            Array.Sort( sortKeys, infoArray );
            if( needsReverse ^ !ascending ) {
                Array.Reverse( infoArray );
            }

            List<PlanVersionInfo> sortedInfo = new List<PlanVersionInfo>();
            for( int i = 0; i < orig.Count; i++ ) {
                sortedInfo.Add( infoArray[ i ] );
            }
            return sortedInfo;
        }

        /// <summary>
        /// Encapsulates interesting information for old-version plans so they don't need to be loaded again except when doing detailed analysis or editing.
        /// </summary>
        public class PlanVersionInfo
        {
            public string Version;
            public DateTime ModificationDate;
            public double StarsRating;
            public string Description;
            public double TotalCost;
            public Guid? PlanID;
            public string Competitor;

            public PlanVersionInfo() {
            }

            public PlanVersionInfo( string version, DateTime modDate, double starsRating, string description, double totalCost, Guid? planID, string competitor ) {
                this.Version = version;
                this.ModificationDate = modDate;
                this.StarsRating = starsRating;
                this.Description = description;
                this.TotalCost = totalCost;
                this.PlanID = planID;
                this.Competitor = competitor;
            }

            public PlanVersionInfo( MediaPlan plan ) {
                this.Version = plan.PlanVersion;
                this.ModificationDate = plan.ModificationDate;
                this.StarsRating = plan.PlanOverallRatingStars;
                this.Description = plan.PlanDescription;
                this.TotalCost = plan.SumOfItemBudgets;
                this.PlanID = plan.PlanID;
                this.Competitor = plan.Competitor;
            }
        }

        /// <summary>
        // converts old-style plans by moving Flighting and AdOption values into the FlightingList
        /// </summary>
        /// <param name="plan"></param>
        private void UpdateIfOldPlan( MediaPlan plan ) 
        {
            plan.FixTypeIDS();

            if( plan.Goals.Count == 0 ) {
                // this is an old plan that doesn't have its own independent goals
                plan.Goals = new List<MediaPlan.PlanGoal>();
                for( int i = 0; i < plan.Specs.CampaignGoals.Count; i++ ) {
                    plan.Goals.Add( new MediaPlan.PlanGoal( plan.Specs.CampaignGoals[ i ], plan.Specs.GoalWeights[ i ] ) );
                }
            }

            if( plan.TargetBudget == 0 ) {
                // this is an old plan that doesn't have its own independent budget
                plan.TargetBudget = plan.Specs.TargetBudget;
            }
        }
    }
}
