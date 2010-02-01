using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WebLibrary
{
    /// <summary>
    /// UserMedia encapsulates all of a user's Profile settings.  It should be loaded once from the Profile, and then maintained in the
    /// session from then on for good performance.
    /// </summary>
    [Serializable]
    public class UserMedia
    {
        // - - - - - - - - - UI CONFIGURATION-SETTING VALUES
        /// <summary>
        /// The name of the current column used for sorting the media plans in the plans-list page
        /// </summary>
        public string plansListSortColumn { set; get; }

        /// <summary>
        /// The name of the current column used for sorting the list of available media items
        /// </summary>
        public string availableMediaSortColumn { set; get; }

        /// <summary>
        /// The geo regions that this user used last time they created a campaign
        /// </summary>
        public List<string> InitialGeoRegionChoices { set; get; }

        /// <summary>
        /// Current version info for the user's campaigns.  
        /// </summary>
        public List<CurrentPlanVersion> CurrentPlanVersions { set; get; }

        /// <summary>
        /// Flag that causes the UI to go into engineering mode automatically when the user logs in.
        /// </summary>
        public bool StartInEngineeringMode { set; get; }

        // user identification values
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyURL { get; set; }
        public string Phone { get; set; }
        public string Referralnfo { get; set; }

        /// <summary>
        /// Constructs a new object for storing persistent user settings
        /// </summary>
        public UserMedia() {
            plansListSortColumn = "name";
            availableMediaSortColumn = "score";
            StartInEngineeringMode = false;
        }

        public class CurrentPlanVersion
        {
            public string CampaignName;
            public string Version;

            public CurrentPlanVersion() {
            }

            public CurrentPlanVersion( string campaignName, string version ) {
                this.CampaignName = campaignName;
                this.Version = version;
            }
        }

        #region user defined media
        // - - - - - - - - USER-DEFINED MEDIA ITEMS - - - - - - - - 
        //
        // Commented out - SSN 9/6/2008
        // when we know how to use these we will pu tthem back in
        //
        /// <summary>
        /// Each MediaVehicleSpec object can be converted to a MediaVehicle for use in a media plan.
        /// </summary>
        //  public List<MediaVehicleSpec> MediaVehicleSpecs { get; set; }
        //public Dictionary<Guid, MediaVehicleSpec> MediaVehicleSpecsDict { set; get; }

        /// <summary>
        /// Each MediaSpotSpec object can be converted to a media spot type (prominence) for use in a media plan.
        /// </summary>
        // public List<MediaSpotSpec> MediaSpotSpecs { set; get; }

        /// <summary>
        /// Each MediaPackageSpec object can be converted to a media package type for use in a media plan.
        /// </summary>
        // public List<MediaPackageSpec> MediaPackageSpecs { set; get; }

        /// <summary>
        /// A MediaVehicleSpec is a set of user-entered values that can be converted to a MediaVehicle.
        /// </summary>
        //public class MediaVehicleSpec
        //{
        //    public Guid ItemID { set; get; }
        //    public string CreatorEmail { set; get; }
        //    public string MediaType { set; get; }
        //    public string SubType { set; get; }
        //    public string Vehicle { set; get; }
        //    public Guid? ComparableVehicleID { set; get; }
        //    public string Comments { set; get; }
        //    public string Cycle { set; get; }
        //    public double CPM { set; get; }
        //    public string URL { set; get; }

        //    public List<List<string>> GeoRegions { set; get; }
        //    public string Demographics { set; get; }

        //    /// <summary>
        //    /// Constructs a new MediaVehicleSpec
        //    /// </summary>
        //    /// <param name="mediaType"></param>
        //    /// <param name="subType"></param>
        //    /// <param name="vehicleName"></param>
        //    /// <param name="comparableVehicle"></param>
        //    /// <param name="comments"></param>
        //    public MediaVehicleSpec( string creatorEmail, string mediaType, string subType, string vehicleName, Guid? comparableVehicle, string comments, string adCycle, 
        //        double cpm, string url, string demographics, List<List<string>> geoRegions ) {
        //        this.ItemID = Guid.NewGuid();
        //        this.CreatorEmail = creatorEmail;
        //        this.MediaType = mediaType;
        //        this.SubType = subType;
        //        this.Vehicle = vehicleName;
        //        this.ComparableVehicleID = comparableVehicle;
        //        this.Comments = comments;
        //        this.Cycle = adCycle;
        //        this.CPM = cpm;
        //        this.URL = url;
        //        this.Demographics = demographics;
        //        this.GeoRegions = geoRegions;
        //    }

        //    /// <summary>
        //    /// For serialization use only
        //    /// </summary>
        //    public MediaVehicleSpec() {
        //    }

        //    /// <summary>
        //    /// Saves the object to the given path.
        //    /// </summary>
        //    /// <param name="path"></param>
        //    public void Save( string path ) {
        //        FileInfo pathInfo = new FileInfo( path );
        //        if( pathInfo.Directory.Exists == false ) {
        //            // we need to create the directory first
        //            pathInfo.Directory.Create();
        //        }

        //        FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write );
        //        XmlSerializer serializer = new XmlSerializer( typeof( MediaVehicleSpec ) );
        //        serializer.Serialize( fs, this );
        //        fs.Flush();
        //        fs.Close();
        //    }
        //}

        /// <summary>
        /// A MediaSpotSpec is a set of user-entered values that can be converted to a media spot type.
        /// </summary>
        //public class MediaSpotSpec
        //{
        //    public Guid SpotID { set; get; }
        //    public string CreatorEmail { set; get; }
        //    public string SpotName { set; get; }
        //    public string SpotComments { set; get; }
        //    public string MediaType { set; get; }

        //    public string ComparableSpot { set; get; }
        //    public double CostRelativeToComparable { set; get; }

        //    /// <summary>
        //    /// Constructs a new MediaSpotSpec
        //    /// </summary>
        //    /// <param name="spotName"></param>
        //    /// <param name="comments"></param>
        //    /// <param name="comparableSpot"></param>
        //    /// <param name="costRatioToComparable"></param>
        //    public MediaSpotSpec( string creatorEmail, string mediaType, string spotName, string comments, string comparableSpot, double costRatioToComparable ) {
        //        this.SpotID = Guid.NewGuid();
        //        this.CreatorEmail = creatorEmail;
        //        this.MediaType = mediaType;
        //        this.SpotName = spotName;
        //        this.SpotComments = comments;
        //        this.ComparableSpot = comparableSpot;
        //        this.CostRelativeToComparable = costRatioToComparable;
        //    }

        //    /// <summary>
        //    /// For serialization use only
        //    /// </summary>
        //    public MediaSpotSpec() {
        //    }

        //    /// <summary>
        //    /// Saves the object to the given path.
        //    /// </summary>
        //    /// <param name="path"></param>
        //    public void Save( string path ) {
        //        FileInfo pathInfo = new FileInfo( path );
        //        if( pathInfo.Directory.Exists == false ) {
        //            // we need to create the directory first
        //            pathInfo.Directory.Create();
        //        }

        //        FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write );
        //        XmlSerializer serializer = new XmlSerializer( typeof( MediaSpotSpec ) );
        //        serializer.Serialize( fs, this );
        //        fs.Flush();
        //        fs.Close();
        //    }
        //}

        /// <summary>
        /// A MediaPackageSpec is a set of user-entered values that can be converted to a media package type.
        /// </summary>
        //public class MediaPackageSpec
        //{
        //    public Guid PackageID { set; get; }
        //    public string CreatorEmail { set; get; }
        //    public string PackageName { set; get; }
        //    public string PackageComments { set; get; }
        //    public int PackageTotalDays { set; get; }
        //    public double PackageCost { set; get; }
        //    public string MediaType { set; get; }
        //    public Guid? VehicleID { set; get; }

        //    public List<List<string>> Prominences { set; get; }
        //    public List<SpotItem> SpotItems { set; get; }

        //    public MediaPackageSpec( string creatorEmail, string mediaType, Guid? vehicleID, string packageName, string comments, double totalCost, int durationDays ) {
        //        this.PackageID = Guid.NewGuid();
        //        this.CreatorEmail = creatorEmail;
        //        this.PackageName = packageName;
        //        this.PackageComments = comments;
        //        this.PackageTotalDays = durationDays;
        //        this.PackageCost = totalCost;
        //        this.Prominences = new List<List<string>>();
        //        this.SpotItems = new List<SpotItem>();
        //        this.MediaType = mediaType;
        //        this.VehicleID = vehicleID;
        //    }

        //    /// <summary>
        //    /// For serialization use only
        //    /// </summary>
        //    public MediaPackageSpec() {
        //    }

        //    public class SpotItem {
        //        public string SpotType { set; get; }
        //        public int SpotDate { set; get; }
        //    }

        //    /// <summary>
        //    /// Saves the object to the given path.
        //    /// </summary>
        //    /// <param name="path"></param>
        //    public void Save( string path ) {
        //        FileInfo pathInfo = new FileInfo( path );
        //        if( pathInfo.Directory.Exists == false ) {
        //            // we need to create the directory first
        //            pathInfo.Directory.Create();
        //        }

        //        FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write );
        //        XmlSerializer serializer = new XmlSerializer( typeof( MediaPackageSpec ) );
        //        serializer.Serialize( fs, this );
        //        fs.Flush();
        //        fs.Close();
        //    }
        //}

        #endregion
    }
}
