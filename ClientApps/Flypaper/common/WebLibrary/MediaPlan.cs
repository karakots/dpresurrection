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

using SimInterface;
using HouseholdLibrary;
using MediaLibrary;

namespace WebLibrary
{
    [XmlInclude( typeof( MediaCampaignSpecs ) )]
    [XmlInclude( typeof( DemographicSettings ) )]

    /// <summary>
    /// A MediaPlan object is the fundamental object in the AdPlanIt website.  It represents a media plan, and includes:
    ///   -- A MediaCampaignSpecs object that describes the campaign (start/end dates, overall budget, business description, customer demographics and geography)
    ///   -- A list of MediaItem objects, each of which represents a generic media type in the plan (Radio, TV, etc.)
    ///            [the specific media subtypes for each generic MediaItem are availible in MediaItem.SubItems].
    ///   -- A SimOutput object that contains the complete simulation results data for the plan.
    /// </summary>
    [Serializable]
    public class MediaPlan
    {
        #region Public Fields
        /// <summary>
        /// Name for this media plan -- just an alternate name for PlanDescription!
        /// </summary>
        public string PlanName {
            set {
                this.PlanDescription = value;
            }
            get {
                return this.PlanDescription;
            }
        }

        /// <summary>
        /// Name for this media plan.
        /// </summary>
        public string PlanDescription {
            set {
                string s = value.Replace( (char)0x22, ' ' ).Replace( (char)0x27, ' ' );   // mask out single and double quotes in case the user pasted in a string including them
                this.PlanDescriptionValue = s;
            }
            get {
                return this.PlanDescriptionValue;
            }
        }

        private string PlanDescriptionValue;

        /// <summary>
        /// Version of this media plan.
        /// </summary>
        public string PlanVersion { set; get; }

        /// <summary>
        /// GUID for this plan.
        /// </summary>
        public Guid PlanID { set; get; }

        /// <summary>
        /// If there is only one plan in a campaign, and it has PlanValid=false, then that campaign has zero plans in it.
        /// </summary>
        /// <remarks>Given implementation time. it would probably be a better solution to store campaigns separately 
        /// from media plans, so a campaign can really have zero plans.</remarks>
        public bool PlanValid { set; get; }

        /// <summary>
        /// The target budget used when generating new plans or suggested improvements.
        /// </summary>
        public double TargetBudget { set; get; }

        /// <summary>
        /// The goals and corresponding weights for this plan.
        /// </summary>
        public List<PlanGoal> Goals;

        /// <summary>
        /// User (other then the account owner) who submitted this plan to the system as part of a competition.
        /// </summary>
        public string Competitor { set; get; }

        /// <summary>
        /// Campaign specs for this media plan.
        /// </summary>
        public MediaCampaignSpecs Specs { set; get; }

        /// <summary>
        /// Returns the campaign specs for the plan itself, not the campaign (target budget and goals may differ from campaign values).
        /// </summary>
        public MediaCampaignSpecs PlanSpecs {
            get {
                MediaCampaignSpecs planSpecs = new MediaCampaignSpecs( this.Specs );
                planSpecs.TargetBudget = this.TargetBudget;
                planSpecs.CampaignGoals = new List<MediaCampaignSpecs.CampaignGoal>();
                planSpecs.GoalWeights = new List<double>();
                for( int i = 0; i < this.Goals.Count; i++ ) {
                    planSpecs.CampaignGoals.Add( this.Goals[ i ].Goal );
                    planSpecs.GoalWeights.Add( this.Goals[ i ].Weight );
                }
                return planSpecs;
            }
        }

        /// <summary>
        /// The generic media types in this plan.  Access the specific subtypes using the SubItems list of each generic MediaItem.
        /// </summary>
        #region  hash code
        //private SortedDictionary<int, SortedDictionary<int, SortedDictionary<Guid, MediaItem>>> PlanMedia = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<Guid, MediaItem>>>();
        #endregion
        #region list code
        public List<MediaItem> MediaItems { set; get; }
        #endregion



        public int NumMediaItems
        {
            get
            {
                return GetAllItems().Count;
            }
        }

      

        /// <summary>
        /// The detailed status of this plan's simulation (always obtained fresh from the server)
        /// </summary>
        public SimUtils.SimStatus SimulationStatus { set; get; }

      

        /// <summary>
        /// Results from the simulation.
        /// </summary>
       

        /// <summary>
        /// Returns the overall plan "stars" rating (0-5) from the simulaiton results.
        /// </summary>
        public double PlanOverallRatingStars { set; get; }

        /// <summary>
        /// These are in the same sequence as the goals in the campaign.
        /// </summary>
        public List<double> PlanOverallGoalScores { set; get; }

        /// <summary>
        /// Controls whether or not this plan is to be displayed as editable in the UI.
        /// </summary>
        public bool Editable { set; get; }

        /// <summary>
        /// Controls whether or not this plan is to be displayed as editable in the UI.
        /// </summary>
        public bool Edited { set; get; }

        /// <summary>
        /// Special instrucitons, if any, for this plan.
        /// </summary>
        public string SpecialInstructions { set; get; }

        /// <summary>
        /// Is true if this plan, or the plan it was copied from,  was created from scratch by the user (as opposed to an automatically-allocated plan).
        /// </summary>
        public bool IsUserPlan { set; get; }

        /// <summary>
        /// The creation date of the plan (from the file system).  
        /// </summary>
        public DateTime CreationDate { set; get; }

        /// <summary>
        /// The modification date of the plan (from the file system).   
        /// </summary>
        public DateTime ModificationDate { set; get; }

        
        #endregion

        #region Properties
        /// <summary>
        /// Name of the campaign that includes this plan.
        /// </summary>
        public string CampaignName {
            get { return Specs.CampaignName; }
        }

        /// <summary>
        /// General comments for the campaign that includes this plan.
        /// </summary>
        public string CampaignComments {
            get { return Specs.Comments; }
        }

        /// <summary>
        /// Purchase cycle code for the campaign that includes this plan. 
        /// </summary>
        public int PurchaseCycleCode {
            get { return Specs.PurchaseCycleCode; }
        }

        /// <summary>
        /// Monthly spending rate for the campaign that includes this plan. 
        /// </summary>
        public double MonthlySpendRate {
            get { return Specs.MonthlySpendRate; }
        }

        /// <summary>
        /// Start date for the campaign that includes this plan. 
        /// </summary>
        public DateTime StartDate {
            get { return Specs.StartDate; }
        }

        /// <summary>
        /// End date for the campaign that includes this plan. 
        /// </summary>
        public DateTime EndDate {
            get { return Specs.EndDate; }
        }

        public List<DateTime> SpotDates
        {
            get
            {
                List<DateTime> rval = new List<DateTime>();
                List<MediaItem> all_items = GetAllItems();
                foreach (MediaItem item in all_items)
                {
                    rval.AddRange(item.GetAdDates());
                }
                rval.Sort();
                return rval;
            }
        }

        #region Simulation Status

        /// <summary>
        /// The general status of this plan's simulation (saved/restored along with the plan)
        /// </summary>
        private MediaPlanStatus status = MediaPlanStatus.READY;
        public MediaPlanStatus PlanStatus
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// Sets or gets the results valus from the sim engine.  
        /// Setting the value also sets PlanStatus to SIMULATED or READY, 
        /// depending if the value is non-null or null.
        /// </summary>
        private SimOutput simOutput = null;
        public SimOutput Results {
            get { return simOutput; }

            set {
                simOutput = value;
                if( simOutput != null ) 
                {
                    status = MediaPlanStatus.SIMULATED;
                }
                else
                {
                    simId = null;
                    status = MediaPlanStatus.READY;
                }
            }
        }


        /// <summary>
        /// GUID of the simulation for this plan.
        /// Setting to a non null value puts the plan into a RUNNING state
        /// </summary>
        private Guid? simId = null;
        public Guid? SimulationID
        {
            set
            {
                simId = value;
            }

            get
            {
                return simId;
            }
        }

        public void UpdateStatus()
        {
            if( simOutput != null )
            {
                status = MediaPlanStatus.SIMULATED;
            }
            else if( simId.HasValue )
            {
                status = MediaPlanStatus.RUNNING;
            }
            else
            {
                status = MediaPlanStatus.READY;
            }
        }

        #endregion

        /// <summary>
        /// Returns the total of all specific (non-generic) individual media item budgets
        /// </summary>
        public double SumOfItemBudgets {
            get
            {
                double itemTot = 0;
                #region hash code
                //foreach (int type_id in PlanMedia.Keys)
                //{
                //    foreach (int subtype_id in PlanMedia[type_id].Keys)
                //    {
                //        foreach (Guid vehicle_id in PlanMedia[type_id][subtype_id].Keys)
                //        {
                //            itemTot += PlanMedia[type_id][subtype_id][vehicle_id].TotalPrice;
                //        }
                //    }
                //}
                #endregion
                #region list code
                foreach (MediaItem type_item in MediaItems)
                {
                    if( type_item.sub_items != null ) {      //JJ1
                        foreach( MediaItem item in type_item.sub_items ) {
                            itemTot += item.TotalPrice;
                        }
                    }
                }
                #endregion
                
                
                return itemTot;
            }
        }

        public int SegmentCount
        {
            get
            {
                return Specs.SegmentList.Count;
            }
        }

        public int DemographicCount {
            get { return Specs.Demographics.Count; }
        }

        public int PlanDurationDays {
            get {
                int nDays = (int)Math.Round( (Specs.EndDate - Specs.StartDate).TotalDays ) + 1;
                return nDays;
            }
        }

        public Dictionary<MediaVehicle.MediaType, double> GetTypeFractions() {
            Dictionary<MediaVehicle.MediaType, double> rval = new Dictionary<MediaVehicle.MediaType, double>();
            double total = 0.0;
            #region hash code
            //foreach(int type_id in PlanMedia.Keys)
            //{
            //    double type_total = 0;
            //    foreach (int subtype_id in PlanMedia[type_id].Keys)
            //    {
            //        foreach (Guid vehicle_id in PlanMedia[type_id][subtype_id].Keys)
            //        {
            //            type_total += PlanMedia[type_id][subtype_id][vehicle_id].TotalPrice;
            //        }
            //    }
            //    rval.Add(Utils.MediaDatabase.GetTypeForID(type_id), type_total);
            //    total += type_total;
            //}
            //foreach (MediaVehicle.MediaType type in PlanMedia.Keys)
            //{
            //    rval[type] /= total;
            //}
            #endregion
            #region list code
            foreach(MediaItem type_item in MediaItems)
            {
                double type_total = 0.0;
                foreach(MediaItem vehicle_item in type_item.sub_items)
                {
                    type_total += vehicle_item.TotalPrice;
                }
                rval.Add(type_item.MediaType, type_total);
                total += type_total;
            }

            foreach(MediaItem type_item in MediaItems)
            {
                rval[type_item.MediaType] /= total;
            }
            #endregion

            /* JiimJ -- commented this out - seems there is no need to return values for types not in the plan (causes confusing displays)
            if (!rval.ContainsKey(MediaVehicle.MediaType.Radio))
            {
                rval.Add(MediaVehicle.MediaType.Radio, 0.0);
            }

            if (!rval.ContainsKey(MediaVehicle.MediaType.Internet))
            {
                rval.Add(MediaVehicle.MediaType.Internet, 0.0);
            }

            if (!rval.ContainsKey(MediaVehicle.MediaType.Magazine))
            {
                rval.Add(MediaVehicle.MediaType.Magazine, 0.0);
            }

            if (!rval.ContainsKey(MediaVehicle.MediaType.Newspaper))
            {
                rval.Add(MediaVehicle.MediaType.Newspaper, 0.0);
            }

            if (!rval.ContainsKey(MediaVehicle.MediaType.Yellowpages))
            {
                rval.Add(MediaVehicle.MediaType.Yellowpages, 0.0);
            }
            */

            return rval;
        }

        public double GetTypeSpending(MediaVehicle.MediaType type)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());

            return GetTypeSpending(type_id);
        }

        public double GetTypeSpending(int type_id)
        {
            double total = 0.0;
            #region hash code
            //if(PlanMedia.ContainsKey(type_id))
            //{
            //    foreach (int subtype_id in PlanMedia[type_id].Keys)
            //    {
            //        foreach (Guid vehicle_id in PlanMedia[type_id][subtype_id].Keys)
            //        {
            //            total += PlanMedia[type_id][subtype_id][vehicle_id].TotalPrice;
            //        }
            //    }
            //}
            #endregion
            #region list code
            foreach (MediaItem type_item in MediaItems)
            {
                if (type_item.type_id == type_id)
                {
                    if( type_item.sub_items != null ) {       //JJ1
                        foreach( MediaItem vehicle_item in type_item.sub_items ) {
                            total += vehicle_item.TotalPrice;
                        }
                    }
                }
            }
            #endregion

            return total;
        }

        /// <summary>
        /// Returns a list of list of strings, each describing a geo location in the plan.
        /// </summary>
        /// <returns></returns>
        public List<string> AllGeoRegionsInPlan() {
            List<string> regionsList = new List<string>();

            foreach( string dmaRegionName in this.Specs.GeoRegionNames ) {
                regionsList.Add( dmaRegionName );
            }

            return regionsList;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new media plan
        /// </summary>
        public MediaPlan() {
            PlanID = Guid.NewGuid();

            this.Specs = new MediaCampaignSpecs();
            this.TargetBudget = this.Specs.TargetBudget;

            this.CreationDate = DateTime.Now;                //!!!??? can we get the (local) time from the browser or request???
            this.ModificationDate = this.CreationDate;

            this.PlanVersion = "1";
            this.PlanValid = true;
            this.PlanOverallRatingStars = -1.0;

            this.Editable = true;
            this.Edited = false;
            this.IsUserPlan = false;

            //this.PlanMedia = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<Guid, MediaItem>>>();
            this.MediaItems = new List<MediaItem>();
 //           this.SegmentMediaItems = new List<List<MediaItem>>();
        }

        /// <summary>
        /// Constructs a new media plan
        /// </summary>
        public MediaPlan( string campaignName )
            : this() {
            this.Specs.CampaignName = campaignName;
        }

        /// <summary>
        /// Constructs a new media plan that is a copy of an existing plan.
        /// </summary>
        public MediaPlan( MediaPlan planToCopy, string newPlanName ) {
            PlanID = Guid.NewGuid();

            this.Editable = true;
            this.Edited = false;
            this.IsUserPlan = false;

            this.CreationDate = DateTime.Now;          //!!!??? can we get the (local) time from the browser or request???
            this.ModificationDate = this.CreationDate;

            //this.Specs = new MediaCampaignSpecs( planToCopy.Specs );
            this.Specs = planToCopy.Specs;   //note that all plans in a campaign use the same campaign-specs object!
            this.PlanDescription = newPlanName;
            this.PlanOverallRatingStars = -1.0;
            this.PlanVersion = planToCopy.PlanVersion;
            this.PlanValid = planToCopy.PlanValid;
            this.TargetBudget = planToCopy.TargetBudget;
            this.Competitor = planToCopy.Competitor;
            this.Goals = new List<PlanGoal>();
            for( int i = 0; i < planToCopy.Goals.Count; i++ ) {
                this.Goals.Add( new  MediaPlan.PlanGoal( planToCopy.Goals[ i ].Goal,  planToCopy.Goals[ i ].Weight ) );
            }

 //           this.SegmentMediaItems = planToCopy.SegmentMediaItems;  // these items are used only for initial allocation so they won't be accessed from copied plans

            MediaItems = planToCopy.MediaItems;
            //this.PlanMedia = planToCopy.PlanMedia;
        }
        #endregion

        /// <summary>
        /// Updates the campaign specs with new values from the updated ones
        /// </summary>
        /// <param name="orig"></param>
        public void CopyCampaignDataFrom( MediaCampaignSpecs updatedSpecs ) {
            CopyCampaignDataFrom( updatedSpecs, false );
        }

        /// <summary>
        /// Updates the campaign specs with new values from the updated ones
        /// </summary>
        /// <param name="orig"></param>
        public void CopyCampaignDataFrom( MediaCampaignSpecs updatedSpecs, bool copyBudgetAndGoalsToPlan ) {
            this.Specs = new MediaCampaignSpecs( updatedSpecs );
            if( copyBudgetAndGoalsToPlan ) {
                this.TargetBudget = updatedSpecs.TargetBudget;
                this.Goals = new List<PlanGoal>();
                for( int i = 0; i < updatedSpecs.CampaignGoals.Count; i++ ) {
                    this.Goals.Add( new MediaPlan.PlanGoal( updatedSpecs.CampaignGoals[ i ], updatedSpecs.GoalWeights[ i ] ) );
                }
            }
        }

        /// <summary>
        /// Deletes all media types and vehicles from the plan.
        /// </summary>
        public void ClearAllMediaItems() {
            this.MediaItems = new List<MediaItem>();
        }

        /// <summary>
        /// Returns a string describing the current status of the plan.
        /// </summary>
        /// <returns></returns>
        public string GetStatus() {
            switch( PlanStatus ) {
                case MediaPlanStatus.READY:
                    return "Ready";
                case MediaPlanStatus.RUNNING:
                    return "Running";
                case MediaPlanStatus.SIMULATED:
                    return "Simulated";
            }
            return null;
        }

        #region Demographic Segment Methods
        /// <summary>
        /// Returns true if the plan contains the demographic segment with the given name.
        /// </summary>
        /// <param name="segmentName"></param>
        /// <returns></returns>
        public bool ContainsSegment( string segmentName ) {
            bool contains = false;
            foreach( DemographicSettings demo in this.Specs.Demographics ) {
                if( demo.DemographicName == segmentName ) {
                    return true;
                }
            }
            return contains;
        }

        /// <summary>
        /// Removes the segment with the given GUID from the plan
        /// </summary>
        /// <param name="segmentID"></param>
        /// <returns></returns>
        public bool RemoveSegment( Guid segmentID ) {
            bool removed = false;
            for( int i = 0; i < this.DemographicCount; i++ ) {
                DemographicSettings seg = this.Specs.Demographics[ i ];
                if( seg.DemographicID == segmentID ) {
                    this.Specs.Demographics.RemoveAt( i );
                    removed = true;
                    break;
                }
            }
            return removed;
        }

        public double PopulationSize { get; set; }
        #endregion

        #region Adding/Changing/Removing Media Types and Subtypes
        /// <summary>
        /// Adds a media type to the plan.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool AddMediaType( MediaVehicle.MediaType type ) 
        {
            return AddMediaType( Utils.MediaDatabase.GetTypeID(type.ToString()) );
        }

        public bool AddMediaType(string type_name)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type_name);

            return AddMediaType(type_id);
        }

        public bool AddMediaType(int type_id)
        {
            
            #region hash code
            //#region error checking
            //if (PlanMedia.ContainsKey(type_id))
            //{
            //    return false;
            //}
            //#endregion
            //PlanMedia.Add(type_id, new SortedDictionary<int, SortedDictionary<Guid, MediaItem>>());
            #endregion
            #region list code
            foreach (MediaItem item in MediaItems)
            {
                if (item.type_id == type_id)
                {
                    return false;
                }
            }

            MediaItems.Add(new MediaItem(type_id));

            #endregion

            this.Edited = true;
            return true;
        }

        /// <summary>
        /// Removes a media type (and any subitems) from the plan.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool RemoveMediaType( MediaVehicle.MediaType type ) 
        {
            return RemoveMediaType(Utils.MediaDatabase.GetTypeID(type.ToString()));
        }

        public bool RemoveMediaType(string type_name)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type_name);

            return RemoveMediaType(type_id);
        }

        public bool RemoveMediaType(int type_id)
        {
            #region hash code
            //#region error checking
            //if (!PlanMedia.ContainsKey(type_id))
            //{
            //    return false;
            //}
            //#endregion
            //PlanMedia.Remove(type_id);
            #endregion
            #region list code
            bool removed = false;
            for(int i = 0; i < MediaItems.Count; i++)
            {
                if (MediaItems[i].type_id == type_id)
                {
                    MediaItems.RemoveAt(i);
                    removed = true;
                }
            }
            #endregion
            
            return removed;
        }

        /// <summary>
        /// Adds a media subtype to the plan.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool AddSubtype(MediaVehicle.MediaType type, String subtype)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, subtype);

            return AddSubtype(type_id, subtype_id);
        }

        public bool AddSubtype(int type_id, int subtype_id)
        {
            //#region error checking
            //if (!PlanMedia.ContainsKey(type_id))
            //{
            //    return false;
            //}

            //if (PlanMedia[type_id].ContainsKey(subtype_id))
            //{
            //    return false;
            //}
            //#endregion

            //PlanMedia[type_id].Add(subtype_id, new SortedDictionary<Guid, MediaItem>());
            return true;
        }

        /// <summary>
        /// Removes a media subtype (and any subitems) from the plan.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool RemoveSubtype(MediaVehicle.MediaType type, String subtype)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, subtype);

            return RemoveSubtype(type_id, subtype_id);
        }

        public bool RemoveSubtype(int type_id, int subtype_id)
        {
            #region hash code
            //#region error checking
            //if (!PlanMedia.ContainsKey(type_id))
            //{
            //    return false;
            //}

            //if (!PlanMedia[type_id].ContainsKey(subtype_id))
            //{
            //    return false;
            //}
            //#endregion
            //PlanMedia[type_id].Remove(subtype_id);
            #endregion
            #region list code
            bool removed = false;
            foreach (MediaItem type_item in MediaItems)
            {
                if (type_item.type_id == type_id)
                {
                    for (int i = 0; i < type_item.sub_items.Count; i++)
                    {
                        if (type_item.sub_items[i].subtype_id == subtype_id)
                        {
                            type_item.sub_items.RemoveAt(i);
                            removed = true;
                        }
                    }
                }
            }
            #endregion
            return true;
        }

        /// <summary>
        /// Adds a media vehicle to the plan. Will add the type and subtype if necessary.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public MediaItem AddMediaVehicle(MediaVehicle vehicle)
        {
            AdOption option = null;
            foreach (AdOption example in vehicle.GetOptions().Values)
            {
                option = example;
                break;
            }
            return AddMediaItem(vehicle, option);
        }

        /// <summary>
        /// Removes a media vehicle from the plan.
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool RemoveMediaVehicle(MediaVehicle vehicle)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);

            return RemoveMediaVehicle(type_id, subtype_id, vehicle.Guid);
        }

        public bool RemoveMediaVehicle(int type_id, int subtype_id, Guid vehicle_id)
        {

            #region hash code
            //#region error checking
            //if (!PlanMedia.ContainsKey(type_id))
            //{
            //    return false;
            //}

            //if (!PlanMedia[type_id].ContainsKey(subtype_id))
            //{
            //    return false;
            //}

            //if (!PlanMedia[type_id][subtype_id].ContainsKey(vehicle_id))
            //{
            //    return false;
            //}
            //#endregion

            //PlanMedia[type_id][subtype_id].Remove(vehicle_id);
            #endregion
            #region list code
            bool removed = false;
            foreach (MediaItem type_item in MediaItems)
            {
                for (int i = 0; i < type_item.sub_items.Count; i++)
                {
                    if (type_item.sub_items[i].vehicle_id == vehicle_id)
                    {
                        type_item.sub_items.RemoveAt(i);
                        removed = true;
                    }
                }
            }
            #endregion

            this.Edited = true;
            return removed;
        }


        /// <summary>
        /// Adds a media item to the plan.  Also adds the type, subtype, vehicle if necessary
        /// </summary>
        /// <param name="mediaTypeName"></param>
        /// <returns></returns>
        public bool AddMediaItem(MediaItem item)
        {
            #region hash code
            //#region error_checking
            //if (!PlanMedia.ContainsKey(item.type_id))
            //{
            //    PlanMedia.Add(item.type_id, new SortedDictionary<int, SortedDictionary<Guid, MediaItem>>());
            //}

            //if (!PlanMedia[item.type_id].ContainsKey(item.subtype_id))
            //{
            //    PlanMedia[item.type_id].Add(item.subtype_id, new SortedDictionary<Guid, MediaItem>());
            //}

            //if (PlanMedia[item.type_id][item.subtype_id].ContainsKey(item.vehicle_id))
            //{
            //    return false;
            //}
            //#endregion

            //PlanMedia[item.type_id][item.subtype_id].Add(item.vehicle_id, item);
            #endregion
            #region list code
            bool added = false;
            foreach (MediaItem type_item in MediaItems)
            {
                if (type_item.type_id == item.type_id)
                {
                    type_item.sub_items.Add(item);
                    added = true;
                }
            }
            if (!added)
            {
                MediaItem type_item = new MediaItem(item.type_id);
                type_item.sub_items.Add(item);
                MediaItems.Add(type_item);
            }
            #endregion
            return true;
        }

        public MediaItem AddMediaItem( MediaVehicle vehicle, int option_id ) {
            AdOption option = Utils.MediaDatabase.GetAdOption( option_id );
            return AddMediaItem( vehicle, option );
        }


        public MediaItem AddMediaItem(MediaVehicle vehicle, AdOption ad_option)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);

            MediaItem item = new MediaItem(vehicle, ad_option, this);

            #region hash code
            //#region error_checking
            //if (!PlanMedia.ContainsKey(type_id))
            //{
            //    PlanMedia.Add(type_id, new SortedDictionary<int, SortedDictionary<Guid, MediaItem>>());
            //}

            //if (!PlanMedia[type_id].ContainsKey(subtype_id))
            //{
            //    PlanMedia[type_id].Add(subtype_id, new SortedDictionary<Guid, MediaItem>());
            //}

            //if (PlanMedia[type_id][subtype_id].ContainsKey(vehicle.Guid))
            //{
            //    return PlanMedia[type_id][subtype_id][vehicle.Guid];
            //}
            //#endregion
            //PlanMedia[type_id][subtype_id].Add(vehicle.Guid, item);
            #endregion
            #region list code
            bool added = false;
            foreach (MediaItem type_item in MediaItems)
            {
                if (type_item.type_id == type_id)
                {
                    foreach (MediaItem v_item in type_item.sub_items)
                    {
                        if (v_item.vehicle_id == vehicle.Guid)
                        {
                            return v_item;
                        }
                    }
                    type_item.sub_items.Add(item);
                    added = true;
                }
            }
            if (!added)
            {
                MediaItem type_item = new MediaItem(type_id);
                type_item.sub_items.Add(item);
                MediaItems.Add(type_item);
            }
            #endregion

            this.Edited = true;
            return item;
        }

        public bool RemoveMediaItem(MediaItem item)
        {
            return RemoveMediaVehicle(item.type_id, item.subtype_id, item.vehicle_id);
        }

        #region old code
        ///// <summary>
        ///// Add the media subtype to this media plan.  The new media item will get its settings from the generic media item.
        ///// </summary>
        ///// <returns></returns>
        //public MediaItem AddMediaSubtype( Guid mediaSubtypeID, string mediaTypeName, UserMedia userMedia, DemographicSettingsAndTargeting demoAndGeoInfo, int numImpressions, int prominenceID, double availBudget ) {
        //    return AddMediaSubtype( mediaSubtypeID, mediaTypeName, false, userMedia, demoAndGeoInfo, numImpressions, prominenceID, availBudget );
        //}

        ///// <summary>
        ///// Add the media vehicle (a special case of a subtype) to this media plan.  The new media item will get its settings from the generic media item.
        ///// </summary>
        ///// <returns></returns>
        //public MediaItem AddMediaVehicle( Guid mediaVehicleID, string mediaTypeName, UserMedia userMedia, DemographicSettingsAndTargeting demoAndGeoInfo, int numImpressions, int prominenceID, double availBudget ) {
        //    return AddMediaSubtype( mediaVehicleID, mediaTypeName, true, userMedia, demoAndGeoInfo, numImpressions, prominenceID, availBudget );
        //}

        ///// <summary>
        ///// Add the media subtype to this media plan.  The new media item will get its settings from the generic media item.
        ///// </summary>
        ///// <returns></returns>
        //public MediaItem AddMediaSubtype( Guid mediaSubtypeID, string mediaTypeName, bool subtypeIsVehicle, UserMedia userMedia, DemographicSettingsAndTargeting demoAndGeoInfo, int numImpressions, int prominenceID, double availBudget ) {

        //    MediaItem subItem = null;
        //    MediaVehicle mediaSubtypeInfo = Utils.MediaTypeForID( mediaSubtypeID, userMedia );
        //    double size = Utils.GetVehicleSize(mediaSubtypeInfo.Guid);
        //    if( mediaSubtypeInfo != null ) {
        //        foreach( MediaItem item in this.MediaItems ) {
        //            if( item.ItemName == mediaTypeName ) {

        //                // create the subitem
        //                subItem = new MediaItem( item, false );
        //                subItem.ItemBudget = availBudget;

        //                subItem.SpotCount = numImpressions;
        //                subItem.NumImpressions = numImpressions;

        //                subItem.MediaInfoID = mediaSubtypeID;
        //                if( demoAndGeoInfo != null ) {
        //                    subItem.DemoAndGeoInfo = demoAndGeoInfo.DemoSettings;
        //                    subItem.TargetingLevel = demoAndGeoInfo.TargetingLevel;
        //                }

        //                // get a reasonable ad option value
        //                Dictionary<int, AdOption> ad_opts = mediaSubtypeInfo.GetOptions();
        //                if( ad_opts.Keys.Count == 0 ){
        //                    string msg = String.Format( "Error: Media with ID {0} has zero ad_options values!", mediaSubtypeID.ToString() );
        //                    throw new Exception( msg );
        //                }

        //                // set the ad_option value
        //                if( prominenceID == null ) {
        //                    // we have no budget to help us choose - use the first available ad option    
        //                    int[] kys = new int[ ad_opts.Keys.Count ];
        //                    ad_opts.Keys.CopyTo( kys, 0 );
        //                    subItem.AdOptionID = kys[ 0 ];
        //                    subItem.AdOption = ad_opts[ kys[ 0 ] ].Name;
        //                }
        //                else {
        //                    // we have a specific value for the ad option
        //                    subItem.AdOptionID = prominenceID;
        //                    subItem.AdOption = ad_opts[ prominenceID ].Name;
        //                    subItem.SpotPrice = Utils.GetSpotPrice(mediaSubtypeInfo, prominenceID, size/1000);
        //                    subItem.ItemBudget = subItem.SpotPrice * subItem.SpotCount;
        //                }
        //                subItem.IsVehicle = subtypeIsVehicle;

        //                if( item.SubItems == null ) {
        //                    item.SubItems = new List<MediaItem>();     // adding the first subitem
        //                }
        //                subItem.HasParentItem = true;
        //                subItem.Flighting = new FlightingInfo( this.PlanDurationDays );

        //                // set the spot dates
        //                subItem.Flighting.SpotDates = Utils.GenerateSpotDates( subItem, mediaSubtypeInfo.Cycle, numImpressions, this.StartDate );
        //                item.SubItems.Add( subItem );
        //            }
        //        }
        //    }
        //    return subItem;
        //}

        ///// <summary>
        ///// Removes the media subtype from this media plan.
        ///// </summary>
        ///// <returns></returns>
        //public bool RemoveMediaSubtype( Guid mediaSubtypeID, int mediaSubypeIndex, string mediaTypeName ) {
        //    foreach( MediaItem mediaItem in this.MediaItems ) {
        //        if( mediaItem.ItemName == mediaTypeName ) {
        //            if( mediaItem.SubItems != null ) {
        //                for( int i = 0; i < mediaItem.SubItems.Count; i++ ) {
        //                    MediaItem subItem = mediaItem.SubItems[ i ];
        //                    if( subItem.MediaInfoID != null ) {
        //                        if( (subItem.MediaInfoID == mediaSubtypeID) && ((mediaSubypeIndex == -1) || (mediaSubypeIndex == i)) ) {
        //                            mediaItem.SubItems.RemoveAt( i );
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Removes the MediaItem using specified media vehicle from this media plan.  Returns the removed MediaItem, or null if the ID was not found.
        ///// </summary>
        ///// <returns></returns>
        //public MediaItem RemoveMediaVehicle( Guid mediaVehicleID ) {
        //    foreach( MediaItem genericItem in this.MediaItems ) {
        //        if( genericItem.SubItems != null ) {
        //            for( int i = 0; i < genericItem.SubItems.Count; i++ ) {
        //                MediaItem subItem = genericItem.SubItems[ i ];

        //                // new style
        //                if( subItem.SubItems != null ) {
        //                    for( int j = 0; j < subItem.SubItems.Count; j++ ) {
        //                        if( subItem.SubItems[ j ].IsVehicle && (subItem.SubItems[ j ].MediaInfoID == mediaVehicleID) ) {
        //                            MediaItem vehicleItem = subItem.SubItems[ j ];
        //                            subItem.SubItems.RemoveAt( j );
        //                            this.Edited = true;
        //                            return vehicleItem;
        //                        }
        //                    }
        //                }

        //                // old style
        //                if( subItem.IsVehicle && (subItem.MediaInfoID == mediaVehicleID) ) {
        //                    genericItem.SubItems.RemoveAt( i );
        //                    this.Edited = true;
        //                    return subItem;
        //                }
        //            }
        //        }
        //    }
        //    return null; // no match for the ID
        //}

        ///// <summary>
        ///// Gets the specified media subtype from this media plan.
        ///// </summary>
        ///// <returns></returns>
        //public MediaItem GetMediaSubItem( int mediaSubypeIndex, string mediaTypeName ) {
        //    foreach( MediaItem mediaItem in this.MediaItems ) {
        //        if( mediaItem.ItemName == mediaTypeName ) {
        //            if( mediaItem.SubItems != null ) {
        //                if( mediaSubypeIndex >= 0 && mediaSubypeIndex < mediaItem.SubItems.Count ) {
        //                    return mediaItem.SubItems[ mediaSubypeIndex ];
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}
        #endregion

        #endregion

        #region Setting Item Prominence, Budget, and Flighting
        public MediaItem GetMediaItem(int type_id, int subtype_id, Guid vehicle_id)
        {
            MediaItem rval;
            #region hash code
            //if (PlanMedia.ContainsKey(type_id))
            //{
            //    if (PlanMedia[type_id].ContainsKey(subtype_id))
            //    {
            //        if (PlanMedia[type_id][subtype_id].ContainsKey(vehicle_id))
            //        {
            //            return PlanMedia[type_id][subtype_id][vehicle_id];
            //        }
            //    }
            //}
            #endregion
            #region list code
            foreach (MediaItem type_item in MediaItems)
            {
                if (type_item.type_id == type_id)
                {
                    foreach (MediaItem item in type_item.sub_items)
                    {
                        if (item.vehicle_id == vehicle_id)
                        {
                            return item;
                        }
                    }
                }
            }
            #endregion

            return null;
        }

        public MediaItem GetMediaItem(MediaVehicle.MediaType type, string subtype, Guid vehicle_id)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, subtype);
            return GetMediaItem(type_id, subtype_id, vehicle_id); ;
        }

        public MediaItem GetMediaItem(Guid vehicle_id)
        {
            MediaItem rval;
            #region hash code
            //if (PlanMedia.ContainsKey(type_id))
            //{
            //    if (PlanMedia[type_id].ContainsKey(subtype_id))
            //    {
            //        if (PlanMedia[type_id][subtype_id].ContainsKey(vehicle_id))
            //        {
            //            return PlanMedia[type_id][subtype_id][vehicle_id];
            //        }
            //    }
            //}
            #endregion
            #region list code
            foreach (MediaItem type_item in MediaItems)
            {
                foreach (MediaItem item in type_item.sub_items)
                {
                    if (item.vehicle_id == vehicle_id)
                    {
                        return item;
                    }
                }
            }
            #endregion

            return null;
        }

        public MediaItem GetMediaItem(MediaVehicle vehicle)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
            int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);

            return GetMediaItem(type_id, subtype_id, vehicle.Guid);
        }

        #region old code

        //public bool SetMediaProminence(Guid vehicle_id, int option_id)
        //{
        //    MediaVehicle vehicle = Utils.MediaDatabase.GetVehicle(vehicle_id, true);

        //    return SetMediaProminence(vehicle, option_id);
        //}

        //public bool SetMediaProminence(MediaVehicle vehicle, int option_id)
        //{
        //    AdOption option = Utils.MediaDatabase.GetAdOption(option_id);

        //    return SetMediaProminence(vehicle, option);
        //}

        //public bool SetMediaProminence(MediaVehicle vehicle, AdOption option)
        //{
        //    int type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
        //    int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);

        //    return SetMediaProminence(type_id, subtype_id, vehicle, option);
        //}

        //public bool SetMediaProminence(int type_id, int subtype_id, MediaVehicle vehicle, AdOption option)
        //{
            
        //    #region hash code
        //    //#region error checking
        //    //if (!vehicle.GetOptions().ContainsKey(option.ID))
        //    //{
        //    //    return false;
        //    //}
        //    //if (!PlanMedia.ContainsKey(type_id))
        //    //{
        //    //    return false;
        //    //}

        //    //if (!PlanMedia[type_id].ContainsKey(subtype_id))
        //    //{
        //    //    return false;
        //    //}

        //    //if (!PlanMedia[type_id][subtype_id].ContainsKey(vehicle.Guid))
        //    //{
        //    //    return false;
        //    //}
        //    //#endregion

        //    //PlanMedia[type_id][subtype_id][vehicle.Guid].TimingList[0].ChangeAdOption(option);

        //    #endregion
        //    #region list code
        //    #region error checking
        //    if (!vehicle.GetOptions().ContainsKey(option.ID))
        //    {
        //        return false;
        //    }
        //    #endregion
        //    foreach (MediaItem type_item in MediaItems)
        //    {
        //        if (type_item.type_id == type_id)
        //        {
        //            foreach (MediaItem item in type_item.sub_items)
        //            {
        //                if (item.vehicle_id == vehicle.Guid)
        //                {
        //                    //item.TimingList[0].ChangeAdOption(option);
        //                }
        //            }
        //        }
        //    }


        //    #endregion

        //    return true;
        //}

        //public void ChangeAdCount(MediaVehicle vehicle, int AdCount)
        //{
        //    int type_id = Utils.MediaDatabase.GetTypeID(vehicle.Type.ToString());
        //    int subtype_id = Utils.MediaDatabase.GetSubtypeID(type_id, vehicle.SubType);

        //    #region hash code
        //    //PlanMedia[type_id][subtype_id][vehicle.Guid].TimingList[0].
        //    #endregion
        //    #region list code
        //    foreach (MediaItem type_item in MediaItems)
        //    {
        //        if (type_item.type_id == type_id)
        //        {
        //            foreach (MediaItem item in type_item.sub_items)
        //            {
        //                if (item.vehicle_id == vehicle.Guid)
        //                {
        //                    //item.TimingList[0].SpotCount = AdCount;
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //}

        #endregion


        public List<MediaItem> GetTypeItems(MediaVehicle.MediaType type)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());
            return GetTypeItems(type_id);
        }

        public List<MediaItem> GetTypeItems(string type_name)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type_name);
            return GetTypeItems(type_id);
        }

        public List<MediaItem> GetTypeItems(int type_id)
        {
            List<MediaItem> rval = new List<MediaItem>();

            #region hash code
            //if (PlanMedia.ContainsKey(type_id))
            //{
            //    foreach (int subtype_id in PlanMedia[type_id].Keys)
            //    {
            //        foreach (MediaItem item in PlanMedia[type_id][subtype_id].Values)
            //        {
            //            rval.Add(item);
            //        }
            //    }
            //}
            #endregion
            #region list code
            foreach (MediaItem genericItem in MediaItems)
            {
                if (genericItem.type_id == type_id)
                {
                    if( genericItem.sub_items != null ) {    //JJ1
                        rval.AddRange( genericItem.sub_items );
                    }
                }
            }
            #endregion

            return rval;
        }

        public List<MediaItem> GetAllItems()
        {
            List<MediaItem> rval = new List<MediaItem>();
            #region hash code
            //foreach (int type_id in PlanMedia.Keys)
            //{
            //    foreach (int subtype_id in PlanMedia.Keys)
            //    {
            //        rval.AddRange(PlanMedia[type_id][subtype_id].Values);
            //    }
            //}
            #endregion
            #region list code

            foreach (MediaItem genericType in MediaItems)
            {
                if( genericType.sub_items != null ) {   //JJ1
                    rval.AddRange( genericType.sub_items );
                }
            }

            #endregion

            return rval;
        }

        public List<string> GetVehicleNames()
        {
            List<string> rval = new List<string>();
            #region hash code
            //foreach (int type_id in PlanMedia.Keys)
            //{
            //    foreach (int subtype_id in PlanMedia[type_id].Keys)
            //    {
            //        foreach (Guid vehicle_id in PlanMedia[type_id][subtype_id].Keys)
            //        {
            //            rval.Add(PlanMedia[type_id][subtype_id][vehicle_id].VehicleName);
            //        }
            //    }
            //}
            #endregion
            #region list code

            foreach (MediaItem genericItem in MediaItems)
            {
                foreach (MediaItem vehicleItem in genericItem.sub_items)
                {
                    if (vehicleItem.is_vehicle == true)
                    {
                        rval.Add(vehicleItem.VehicleName);
                    }
                }
            }
            #endregion

            return rval;
        }

        public List<int> GetTypes()
        {
            List<int> rval = new List<int>();

            #region hash code
            //rval.AddRange(PlanMedia.Keys);
            #endregion

            #region list code
            foreach (MediaItem item in MediaItems)
            {
                rval.Add(item.type_id);
            }
            #endregion

            return rval;
        }

        public bool HasType(MediaVehicle.MediaType type)
        {
            int type_id = Utils.MediaDatabase.GetTypeID(type.ToString());

            return HasType(type_id);
        }

        public bool HasType(int type_id)
        {
            #region hash code
            //if (PlanMedia.ContainsKey(type_id))
            //{
            //    return true;
            //}
            #endregion
            #region list code
            foreach (MediaItem item in MediaItems)
            {
                if (item.type_id == type_id)
                {
                    return true;
                }
            }
            #endregion

            return false;
        }

        #region old plan conversion

        public void FixTypeIDS()
        {
            foreach (MediaItem item in MediaItems)
            {
                string type_name = item.MediaType.ToString();
                int type_id = Utils.MediaDatabase.GetTypeID(type_name);
                item.type_id = type_id;
            }
        }


        #endregion

        #region old_code
        ///// <summary>
        ///// Sets the prominence (ad option) for the specified media.  
        ///// </summary>
        ///// <returns></returns>
        //public bool SetMediaProminence( Guid vehicle_id, int media_subtype, int media_type, int option_id, UserMedia userMedia )
        //{
        //    foreach (MediaItem item in PlanMedia[media_type][media_subtype][vehicle_id])
        //    {
        //        item.AdOptionID = option_id;
        //    }
        //    return true;
        //    #region old_code
        //    //foreach( MediaItem mediaItem in this.MediaItems ) {
        //    //    if( mediaItem.ItemName == mediaTypeName ) {
        //    //        if( mediaItem.SubItems != null ) {
        //    //            for( int i = 0; i < mediaItem.SubItems.Count; i++ ) {
        //    //                MediaItem subItem = mediaItem.SubItems[ i ];
        //    //                if( subItem.MediaInfoID != null ) {
        //    //                    if( (subItem.MediaInfoID == mediaSubtypeID) && ((mediaSubypeIndex == -1) || (mediaSubypeIndex == i)) ) {
        //    //                        MediaVehicle theVehicle = Utils.MediaTypeForID( mediaSubtypeID, userMedia );

        //    //                        int newAdOption = 0;
        //    //                        if( theVehicle.GetOptions().ContainsKey( newAdOptionID ) ) {
        //    //                            newAdOption = theVehicle.GetOptions()[ newAdOptionID ].Name;  // look up the detailed name
        //    //                        }
        //    //                        else {
        //    //                            // perhaps it is a user item
        //    //                            if( userMedia != null && userMedia.MediaSpotSpecs != null && userMedia.MediaSpotSpecs.Count > 0 ) {
        //    //                                try {
        //    //                                    Guid userItemID = new Guid( newAdOptionID );
        //    //                                    foreach( UserMedia.MediaSpotSpec uSpot in userMedia.MediaSpotSpecs ) {
        //    //                                        if( uSpot.SpotID == userItemID ) {
        //    //                                            // found the user item
        //    //                                            newAdOption = uSpot.SpotName;
        //    //                                        }
        //    //                                    }
        //    //                                }
        //    //                                catch( Exception ) {
        //    //                                }
        //    //                            }

        //    //                            // perhaps it is a user item
        //    //                            if( userMedia != null && userMedia.MediaPackageSpecs != null && userMedia.MediaPackageSpecs.Count > 0 ) {
        //    //                                try {
        //    //                                    Guid userItemID = new Guid( newAdOptionID );
        //    //                                    foreach( UserMedia.MediaPackageSpec uPack in userMedia.MediaPackageSpecs ) {
        //    //                                        if( uPack.PackageID == userItemID ) {
        //    //                                            // found the user item
        //    //                                            newAdOption = uPack.PackageName;
        //    //                                        }
        //    //                                    }
        //    //                                }
        //    //                                catch( Exception ) {
        //    //                                }
        //    //                            }
        //    //                        }
        //    //                        double size = Utils.GetVehicleSize(theVehicle.Guid);
        //    //                        double newSpotPrice = Utils.GetSpotPrice( theVehicle, newAdOptionID, size / 1000 );

        //    //                        mediaItem.SubItems[ i ].AdOptionID = newAdOptionID;
        //    //                        mediaItem.SubItems[ i ].AdOption = newAdOption;
        //    //                        mediaItem.SubItems[ i ].SpotPrice = newSpotPrice;

        //    //                        // adjust the number of impressions
        //    //                        double itemBudg = mediaItem.SubItems[ i ].ItemBudget;

        //    //                        // we need the actual vehicle
        //    //                        MediaVehicle vehicle = Utils.MediaTypeForID( mediaSubtypeID, userMedia );

        //    //                        mediaItem.SubItems[ i ].SpotCount = (int)Math.Floor( itemBudg / newSpotPrice );
        //    //                        if( mediaItem.SubItems[ i ].ActualTotalPrice < 0 ) {
        //    //                            mediaItem.SubItems[ i ].ItemBudget = mediaItem.SubItems[ i ].SpotCount * newSpotPrice;
        //    //                        }
        //    //                        return true;
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //return false;
        //    #endregion
        //}

        ///// <summary>
        ///// Sets the budget (and corresponding spot count) for the specified media.
        ///// </summary>
        ///// <returns></returns>
        //public bool SetMediaSubtypeBudget( Guid mediaSubtypeID, int mediaSubypeIndex, string mediaTypeName, MediaVehicle.AdCycle cycle, 
        //                                                                                                string[] newBudgetInfo, UserMedia userMedia ) 
        //{
        //    //foreach( MediaItem mediaItem in this.MediaItems ) {
        //    //    if( mediaItem.ItemName == mediaTypeName ) {
        //    //        if( mediaItem.SubItems != null ) {
        //    //            for( int i = 0; i < mediaItem.SubItems.Count; i++ ) {
        //    //                MediaItem subItem = mediaItem.SubItems[ i ];
        //    //                if( subItem.MediaInfoID != null ) {
        //    //                    if( (subItem.MediaInfoID == mediaSubtypeID) && ((mediaSubypeIndex == -1) || (mediaSubypeIndex == i)) ) {

        //    //                        // get updated budget for the media
        //    //                        double newBudgetVal = mediaItem.SubItems[ i ].ItemBudget;

        //    //                        try {
        //    //                            newBudgetVal = Convert.ToDouble( newBudgetInfo[ 0 ] );
        //    //                        }
        //    //                        catch( Exception ) {
        //    //                        }

        //    //                        // get updated spot count for the media
        //    //                        int newSpotCountVal = mediaItem.SubItems[ i ].SpotCount;

        //    //                        try {
        //    //                            newSpotCountVal = Convert.ToInt32( newBudgetInfo[ 1 ] );
        //    //                        }
        //    //                        catch( Exception ) {
        //    //                        }

        //    //                        // negative spot count indicates an ignored comment-only menu item!
        //    //                        if( newSpotCountVal >= 0 ) {
        //    //                            // set the new vals
        //    //                            if( newBudgetVal >= 0 ) {
        //    //                                mediaItem.SubItems[ i ].ItemBudget = newBudgetVal;
        //    //                                mediaItem.SubItems[ i ].ActualTotalPrice = -1;     

        //    //                                mediaItem.SubItems[ i ].SpotCount = newSpotCountVal;
        //    //                                mediaItem.SubItems[ i ].SpotPrice = newBudgetVal / newSpotCountVal;

        //    //                                // regenerate the spot dates
        //    //                                mediaItem.SubItems[ i ].Flighting.SpotDates = Utils.GenerateSpotDates( mediaItem.SubItems[ i ], cycle, newSpotCountVal, this.StartDate );
        //    //                            }
        //    //                            else {
        //    //                                // negative budget amount is an exact amount paid - do not change spot count
        //    //                                mediaItem.SubItems[ i ].ActualTotalPrice = Math.Abs( newBudgetVal );
        //    //                                mediaItem.SubItems[ i ].ItemBudget = Math.Abs( newBudgetVal );
        //    //                            }
        //    //                            return true;
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //return false;
        //}

        ///// <summary>
        ///// Sets the flighting for the specified media
        ///// </summary>
        ///// <returns></returns>
        //public bool SetMediaFlighting( Guid , int mediaSubypeIndex, string newDuration, string newDelay, string newWait, string newNumPulses, string newNumAdsPerPulse, string newCycleLength, bool forceContinuous, UserMedia userMedia ) 
        //{
        //    foreach( MediaItem mediaItem in this.MediaItems ) {
        //        if( mediaItem.SubItems != null ) {
        //            for( int i = 0; i < mediaItem.SubItems.Count; i++ ) {
        //                MediaItem subItem = mediaItem.SubItems[ i ];
        //                if( subItem.MediaInfoID != null ) {
        //                    if( (subItem.MediaInfoID == mediaSubtypeID) && ((mediaSubypeIndex == -1) || (mediaSubypeIndex == i)) ) {

        //                        // found the media with the given ID
        //                        double newDurValue = mediaItem.SubItems[ i ].Flighting.PulseDays;
        //                        double newDelayValue = mediaItem.SubItems[ i ].Flighting.PulseDelayDays;
        //                        double newWaitValue = mediaItem.SubItems[ i ].Flighting.DaysBetweenPulses;
        //                        int newNumPulsesValue = mediaItem.SubItems[ i ].Flighting.PulsesPerCycle;
        //                        int newNumAdsPerPulseValue = mediaItem.SubItems[ i ].Flighting.AdsPerPulse;
        //                        int newCycleLengthValue = mediaItem.SubItems[ i ].Flighting.CycleDays;

        //                        try {
        //                            newDurValue = Convert.ToDouble( newDuration );
        //                            newDelayValue = Convert.ToDouble( newDelay );
        //                            newWaitValue = Convert.ToDouble( newWait );
        //                            newNumPulsesValue = Convert.ToInt32( newNumPulses );
        //                            newNumAdsPerPulseValue = Convert.ToInt32( newNumAdsPerPulse );
        //                            newCycleLengthValue = Convert.ToInt32( newCycleLength );
        //                        }
        //                        catch( Exception ) {
        //                        }

        //                        mediaItem.SubItems[ i ].Flighting.Continuous = forceContinuous;

        //                        mediaItem.SubItems[ i ].Flighting.PulseDays = newDurValue;
        //                        mediaItem.SubItems[ i ].Flighting.PulseDelayDays = newDelayValue;
        //                        mediaItem.SubItems[ i ].Flighting.DaysBetweenPulses = newWaitValue;
        //                        mediaItem.SubItems[ i ].Flighting.PulsesPerCycle = newNumPulsesValue;
        //                        mediaItem.SubItems[ i ].Flighting.AdsPerPulse = newNumAdsPerPulseValue;
        //                        mediaItem.SubItems[ i ].Flighting.CycleDays = newCycleLengthValue;

        //                        mediaItem.SubItems[ i ].Flighting.ModificationTimestamp = DateTime.Now.Ticks;  

        //                        // we need the actual vehicle
        //                        MediaVehicle vehicle = Utils.MediaTypeForID( mediaSubtypeID, userMedia );

        //                        mediaItem.SubItems[ i ].Flighting.SpotDates = Utils.GenerateSpotDates( mediaItem.SubItems[ i ], vehicle.Cycle,
        //                             mediaItem.SubItems[ i ].SpotCount, this.StartDate );
        //                        int newSpotCount = mediaItem.SubItems[ i ].Flighting.ActualSpotCount;

        //                        mediaItem.SubItems[ i ].SpotCount = newSpotCount;
        //                        if( mediaItem.SubItems[ i ].ActualTotalPrice < 0 ) {
        //                            mediaItem.SubItems[ i ].ItemBudget = newSpotCount * mediaItem.SubItems[ i ].SpotPrice;
        //                        }
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        #endregion

        #endregion

        public string GetTypeSummaryDescription( MediaVehicle.MediaType mediaType ) {
            List<MediaItem> typeVehicles = this.GetTypeItems( mediaType );

            string preposition = "in";
            string adName = "ad";
            if( mediaType == MediaVehicle.MediaType.Radio ){
                preposition = "on";
            }
            else if( mediaType == MediaVehicle.MediaType.Internet ){
                preposition = "by";
                adName = "impression";
            }

            if( typeVehicles.Count == 0 ) {
                return "none";
            }
            else if( typeVehicles.Count == 1 ) {
                bool dum = false;
                int adCnt = typeVehicles[ 0 ].SpotCount;
                if( adCnt == 1 ) {
                    return String.Format( "{0} {1} {2} {3}", adCnt, adName, preposition, Utils.VehicleInfoLink( typeVehicles[ 0 ] ) );
                }
                else {
                    return String.Format( "{0} {1}s {2} {3}", adCnt, adName, preposition, Utils.VehicleInfoLink( typeVehicles[ 0 ] ) );
                }
            }
            else {
                int adCnt = 0;
                foreach( MediaItem mitem in typeVehicles ) {
                    adCnt += mitem.SpotCount;
                }
                if( adCnt == 1 ) {
                    return String.Format( "{0} total {1} {2} {3} media", adCnt, adName, preposition, typeVehicles.Count );
                }
                else {
                    return String.Format( "{0} total {1}s {2} {3} media", adCnt, adName, preposition, typeVehicles.Count );
                }

            }
            return "";
        }

        /// <summary>
        /// Makes a reasonable estimate of the size of the population represented by each of the segments in the plan's demographics
        /// </summary>
        /// <returns></returns>
        public MediaCampaignSpecs.SegmentSize[] PopulationPercentTargeted() {
            double dummy;
            return Specs.PopulationPercentTargeted( out dummy );
        }

        /// <summary>
        /// Makes a reasonable estimate of the size of the population represented by each of the segments in the plan's demographics
        /// </summary>
        /// <returns></returns>
        public MediaCampaignSpecs.SegmentSize[] PopulationPercentTargeted( out double percentFromAllSegments ) {
            return Specs.PopulationPercentTargeted( out percentFromAllSegments );
        }


        #region Ratings-Related Methods
        public List<MediaItem> GetRatedItems() {
            return GetRatedItems( -1 );
        }

        public List<MediaItem> GetRatedItems( MediaVehicle.MediaType mediaType ) {
            int type_id = Utils.MediaDatabase.GetTypeID( mediaType.ToString() );
            return GetRatedItems( type_id );
        }

        public List<MediaItem> GetRatedItems( int typeID ) {
            List<MediaItem> allItems = null;
            if( typeID == -1 ) {
                allItems = GetAllItems();
            }
            else {
                allItems = GetTypeItems( typeID );
            }

            MediaItem[] sortedItems = new MediaItem[ allItems.Count ];
            double[] sortedScores = new double[ allItems.Count ];
            for( int i = 0; i < allItems.Count; i++ ) {
                sortedItems[ i ] = allItems[ i ];
                sortedScores[ i ] = allItems[ i ].VehicleRating;
            }
            Array.Sort( sortedScores, sortedItems );
            Array.Reverse( sortedItems );

            List<MediaItem> sortedList = new List<MediaItem>();
            for( int i = 0; i < allItems.Count; i++ ) {
                sortedList.Add( sortedItems[ i ] );
            }

            return sortedList;
        }
        #endregion

        #region Saving and Loading
        /// <summary>
        /// Saves the media plan to the given path.
        /// </summary>
        /// <param name="path"></param>
        public void Save( string path ) {
            FileInfo pathInfo = new FileInfo( path );
            if( pathInfo.Directory.Exists == false ) {
                // we need to create the directory first
                pathInfo.Directory.Create();
            }

            FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write );
            XmlSerializer serializer = new XmlSerializer(typeof(MediaPlan));
            serializer.Serialize( fs, this );
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// Loads a media plan from a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MediaPlan LoadPlan( string path ) {
            if( File.Exists( path ) == false ) {
                throw new Exception( "Error: Unable to load media plan file (file not found): " + path );
            }
            MediaPlan plan = null;
            FileStream fs = null;
            //try {
                XmlSerializer serializer = new XmlSerializer( typeof( MediaPlan ) );
                fs = new FileStream( path, FileMode.Open );
                plan = new MediaPlan();
                plan = (MediaPlan)serializer.Deserialize( fs );
            //}
            //catch (Exception)
            //{
            //    plan = null;
            //    return plan;
            //}
            if( fs != null ) {
                fs.Close();
            }

            plan.UpdateStatus();

            return plan;
        }
        #endregion

        // debug method -- forms a string describing the segment from the settings 
        public string SegmentName( int indx ) {
            string s = "";
            foreach( DemographicGroupValues demoVals in this.Specs.Demographics[ indx ].Values ) {
                bool allTrue = true;
                for( int i = 0; i < demoVals.Values.Length; i++ ) {
                    if( demoVals.Values[ i ] == false ) {
                        allTrue = false;
                    }
                }
                if( allTrue == false ) {
                    for( int i = 0; i < demoVals.Values.Length; i++ ) {
                        if( demoVals.Values[ i ] == true ) {
                            s += demoVals.ValueNames[ i ] + ", ";
                        }
                    }
                }
            }
            if( s.Length > 1 ) {
                s = s.Substring( 0, s.Length - 2 );
            }
            return s;
        }

        /// <summary>
        /// Specifies the status of this media plan's simulation.
        /// </summary>
        [Serializable]
        public enum MediaPlanStatus
        {
            READY,
            RUNNING,
            SIMULATED
        };

        /// <summary>
        /// Specifies a goal and its weight relative to the other goals.
        /// </summary>
        [Serializable]
        public class PlanGoal
        {
            public MediaCampaignSpecs.CampaignGoal Goal;
            public double Weight;

            public PlanGoal() {
            }

            public PlanGoal( MediaCampaignSpecs.CampaignGoal goal, double weight ) {
                this.Goal = goal;
                this.Weight = weight;
            }
        }
    }
}