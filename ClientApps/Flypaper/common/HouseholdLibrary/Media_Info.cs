using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

using GeoLibrary;

namespace HouseholdLibrary
{
    [Serializable]
    public abstract class MediaVehicle
    {
        public Guid Guid;
        public enum MediaType
        {
            Magazine,
            Radio,
            Internet
        }
        public enum AdCycle
        {
            Instant,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Bimonthly,
            Quarterly
        }

        [NonSerialized]
        private GeoRegion region;

        public List<Demographic> Demographics;

        public AdCycle Cycle;
        public MediaType Type;
        public string SubType;
        public string Vehicle;
        public double CPM;
        public string URL;

        private string region_name;
        public GeoRegion Region
        {
            get
            {
                return region;
            }
            set
            {
                region = value;
                region_name = value.Name;
            }
        }
        public string RegionName
        {
            get
            {
                return region_name;
            }
        }

        protected Dictionary<string, AdOption> my_options;

        public MediaVehicle(MediaType type, string subtype, string vehicle, GeoRegion region, AdCycle cycle, double cpm, string url)
        {
            Guid = Guid.NewGuid();
            Type = type;
            Cycle = cycle;
            SubType = subtype;
            Vehicle = vehicle;
            Region = region;
            my_options = new Dictionary<string, AdOption>();
            Demographics = new List<Demographic>();
            CPM = cpm;
            URL = url;
        }

        public Dictionary<string, AdOption> GetOptions()
        {
            return my_options;
        }

        public virtual void AddOption(AdOption option)
        {
            my_options.Add(option.ID, option);
        }

        /// <summary>
        /// Computes the percent of demographic reached by this media
        /// Also the effeciency = percent of media subscribers who fit this demographic
        /// percent will be between 0 - 1
        /// </summary>
        /// <param name="dem"></param>
        /// <param name="reach"></param>
        /// <param name="eff"></param>
        public ReachInfo Reach(Demographic dem)
        {
            List<Demographic> tmp = new List<Demographic>();

            tmp.Add(dem);

            return Reach(tmp);
        }

        /// <summary>
        /// Computes the percent of demographic reached by this media
        /// Also the effeciency = percent of media subscribers who fit this demographic
        /// percent will be between 0 - 1
        /// </summary>
        /// <param name="dem"></param>
        /// <param name="reach"></param>
        /// <param name="eff"></param>
        public ReachInfo Reach(List<Demographic> dems)
        {

            ReachInfo rval = new ReachInfo(0, 0, 0);

            if (GeoGraph.TopGeo == null)
            {
                return rval;
            }

            // make sure demographics are set to have complete reach
            foreach (Demographic demo in dems)
            {
                demo.Info.Reach = 1;
            }

            // let the magic begin
            Dictionary<string, Dictionary<string, double>> mediaFactors = Demographic.Factor(Demographics);
            Dictionary<string, Dictionary<string, double>> demFactors = Demographic.Factor(dems);

            // compute totals
            double mediaTotal = 0;
            foreach (Dictionary<string, double> demos in mediaFactors.Values)
            {
                foreach (double val in demos.Values)
                {
                    mediaTotal += val;
                }
            }

            // compute the size of the demographic input
            double demTotal = 0;
            foreach (Dictionary<string, double> demos in demFactors.Values)
            {
                foreach (double val in demos.Values)
                {
                    demTotal += val;
                }
            }

            double mediaDemoProd = 0;
            foreach (string demRegionName in demFactors.Keys)
            {
                GeoRegion demRegion = GeoGraph.TopGeo.GetSubRegion(demRegionName);
                foreach (string medRegionName in mediaFactors.Keys)
                {
                    GeoRegion medRegion = GeoGraph.TopGeo.GetSubRegion(medRegionName);
                    if (medRegion.IsSubRegionOf(demRegion))
                    {
                        foreach (string baseType in demFactors[demRegionName].Keys)
                        {
                            if (demFactors[demRegionName][baseType] > 0)
                            {
                                mediaDemoProd += mediaFactors[medRegionName][baseType];
                            }
                        }
                    }
                }
            }

            if (mediaTotal > 0)
            {
                rval.Effeciency = mediaDemoProd / mediaTotal;
            }

            if (demTotal > 0)
            {
                rval.Reach = mediaDemoProd / demTotal;
            }

            return rval;
        }

        public static MediaVehicle.AdCycle GetCycle(string cycle)
        {
            switch (cycle)
            {
                case "Instant":
                    return MediaVehicle.AdCycle.Instant;
                case "Hourly":
                    return MediaVehicle.AdCycle.Hourly;
                case "Daily":
                    return MediaVehicle.AdCycle.Daily;
                case "Weekly":
                    return MediaVehicle.AdCycle.Weekly;
                case "Monthly":
                    return MediaVehicle.AdCycle.Monthly;
                case "Bimonthly":
                    return MediaVehicle.AdCycle.Bimonthly;
                case "Quarterly":
                    return MediaVehicle.AdCycle.Quarterly;
                default:
                    return MediaVehicle.AdCycle.Monthly;
            }
        }

        public static MediaVehicle.MediaType GetType(string type)
        {
            switch (type.ToUpper())
            {
                case "MAGAZINE":
                    return MediaVehicle.MediaType.Magazine;
                case "RADIO":
                    return MediaVehicle.MediaType.Radio;
                default:
                    return MediaVehicle.MediaType.Magazine;
            }
        }

        private static IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        private static List<string> files = new List<string>();
        private const string mediaDir = @"\media";
        public static DirectoryInfo MediaDir(string topDir)
        {
            return new DirectoryInfo(topDir + mediaDir);
        }

        public static string ReadMediaRegionsAndDemgraphics(string dirName, out List<MediaVehicle> rval)
        {
            rval = null;

            string error = GeoGraph.ReadFromFile( dirName );

            if( error != null )
            {
                rval = null;
                return error;
            }

            GeoGraph.BuildLibrary();

            error = read_media(MediaDir(dirName), out rval);

            if (error != null)
            {
                rval = null;
                return error;
            }

            foreach (MediaVehicle vehicle in rval)
            {
                vehicle.Region = GeoGraph.TopGeo.GetSubRegion(vehicle.RegionName);
            }

            return null;

            
        }

        private static string read_media(DirectoryInfo media_dir, out List<MediaVehicle> rval)
        {
            files.Clear();
            add_files(media_dir);
            rval = new List<MediaVehicle>();
            foreach (string file in files)
            {
                try
                {
                        FileStream in_stream = new FileStream(file, FileMode.Open);
                        rval.AddRange((List<MediaVehicle>)formatter.Deserialize(in_stream));
                        in_stream.Close();
                }
                catch(Exception e)
                {
                    return e.Message;
                }
            }

            return null;
        }

        private static void add_files(DirectoryInfo folder)
        {
            foreach (DirectoryInfo sub_directory in folder.GetDirectories())
            {
                add_files(sub_directory);
            }

            foreach (FileInfo file in folder.GetFiles())
            {
                files.Add(file.FullName);
            }
        }
    }

    [Serializable]
    public class ScalarVehicle : MediaVehicle
    {
        public double persuasion_scalar;
        public double prob_scalar;

        public ScalarVehicle(double Persuasion_Scalar, double Prob_Scalar, MediaType type, string subtype, string vehicle, GeoRegion region, AdCycle cycle, double cpm, string url) : base(type,subtype,vehicle,region,cycle,cpm, url)
        {
            persuasion_scalar = Persuasion_Scalar;
            prob_scalar = Prob_Scalar;
        }

        public void AddOption(SimpleOption option)
        {
            my_options.Add(option.ID, new SimpleOption(option.Name, option.ID, option.Prob_Per_Hour*prob_scalar, option.Awareness, option.Persuasion * persuasion_scalar, option.Recency, option.Cost_Modifier));
        }

        public void AddOption(OnlineOption option)
        {
            my_options.Add(option.ID, new OnlineOption(option.Name, option.ID, option.Awareness, option.Persuasion * persuasion_scalar, option.Recency, option.Click_Awaneness, option.Click_Persuasion * persuasion_scalar, option.Action_Awareness, option.Action_Persuasion * persuasion_scalar, option.Chance_Click, option.Chance_Action, option.Cost_Modifier));
        }
    }

    [Serializable]
    public class Magazine : ScalarVehicle
    {
        public enum PurchaseOption
        {
            National,
            DMA
        }

        private List<PurchaseOption> purchase_options;
        public double Circulation;

        public Magazine(double circulation, double Persuasion_Scalar, double Prob_Scalar, MediaType type, string subtype, string vehicle, GeoRegion region, AdCycle cycle, double cpm, string url)
            : base(Persuasion_Scalar, Prob_Scalar, type, subtype, vehicle, region, cycle, cpm, url)
        {
            Circulation = circulation;
            purchase_options = new List<PurchaseOption>();
        }

        public void AddPurchaseOption(PurchaseOption option)
        {
            purchase_options.Add(option);
        }

        public bool NationalAds
        {
            get
            {
                if (purchase_options.Contains(PurchaseOption.National))
                {
                    return true;
                }
                return false;
            }
        }

        public bool DMAAds
        {
            get
            {
                if (purchase_options.Contains(PurchaseOption.DMA))
                {
                    return true;
                }
                return false;
            }
        }
    }

    [Serializable]
    public class InternetAdNetwork : ScalarVehicle
    {
        public double CPC;
        public double CPA;
        public double UniqueUsers;
        public double MaxImpressions;
        public double ClickScalar;
        public double ActionScalar;

        public InternetAdNetwork(double Cost_Per_Click, double Cost_Per_Action, double Unique_Users, double Max_Impressions, double Click_Scalar, double Action_Scalar, double Persuasion_Scalar, double Prob_Scalar, MediaType type, string subtype, string vehicle, GeoRegion region, AdCycle cycle, double cpm, string url)
            : base(Persuasion_Scalar, Prob_Scalar, type, subtype, vehicle, region, cycle, cpm, url)
        {
            CPC = Cost_Per_Click;
            CPA = Cost_Per_Action;
            UniqueUsers = Unique_Users;
            MaxImpressions = Max_Impressions;
            ClickScalar = Click_Scalar;
            ActionScalar = Action_Scalar;
        }


    }

    public interface AdOption
    {
        string Name { get; }
        string ID { get; }

        double prob_of_ad(AgentData data, double hours);

        void modify_agent(AgentData data, double hours);
    }

    [Serializable]
    public class SimpleOption : AdOption
    {
        public static Random rand = new Random();

        private string name;
        private string id;
        private double persuasion;
        private double recency;
        private double awareness;
        private double prob_per_hour;
        private double cost_modifier;

        public SimpleOption()
        {
            persuasion = 0.0;
            recency = 0.0;
            awareness = 0.0;
            prob_per_hour = 0.0;
            id = "NOID";
        }

        public SimpleOption(string Name, string ID, double Prob_Per_Hour, double Awareness, double Persuasion, double Recency, double Cost_Modifier)
        {
            name = Name;
            id = ID;
            prob_per_hour = Prob_Per_Hour;
            awareness = Awareness;
            persuasion = Persuasion;
            recency = Recency;
            cost_modifier = Cost_Modifier;
        }

        public double Cost_Modifier
        {
            get
            {
                return cost_modifier;
            }
            set
            {
                cost_modifier = value;
            }
        }

        public double Prob_Per_Hour
        {
            get
            {
                return prob_per_hour;
            }
            set
            {
                prob_per_hour = value;
            }
        }

        public double Persuasion
        {
            get
            {
                return persuasion;
            }
            set
            {
                persuasion = value;
            }
        }

        public double Awareness
        {
            get
            {
                return awareness;
            }
            set
            {
                awareness = value;
            }
        }

        public double Recency
        {
            get
            {
                return recency;
            }
            set
            {
                recency = value;
            }
        }

        #region AdOption Members

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string ID
        {
            get
            {
                return id;
            }
        }

        public virtual double prob_of_ad(AgentData data, double hours)
        {
            while (hours > 1)
            {
                if (rand.NextDouble() < Prob_Per_Hour)
                {
                    return 1.0;
                }
                hours--;
            }

            if (rand.NextDouble() < Prob_Per_Hour * hours)
            {
                return 1.0;
            }

            return 0.0;

        }

        public virtual void modify_agent(AgentData data, double hours)
        {
            data.AddImpression();
            if (data.Aware || rand.NextDouble() < awareness)
            {
                data.Aware = true;
                data.Persuasion += persuasion;
                data.Recency += recency;
            }
        }

        #endregion
    }

    [Serializable]
    public class OnlineOption : SimpleOption
    {
        public double Num_Impressions { get; set; }
        public double Click_Awaneness { get; set; }
        public double Click_Persuasion { get; set; }
        public double Action_Awareness { get; set; }
        public double Action_Persuasion { get; set; }
        public double Chance_Click { get; set; }
        public double Chance_Action { get; set; }

        public OnlineOption(string Name, string ID, double Awareness, double Persuasion, double Recency, double ClickAwareness, double ClickPersuasion, double ActionAwareness, double ActionPersuasion, double ChanceClick, double ChanceAction, double Cost_Modifier)
            : base(Name, ID, 0.0, Awareness, Persuasion, Recency, Cost_Modifier)
        {
            Click_Awaneness = ClickAwareness;
            Click_Persuasion = ClickPersuasion;
            Action_Awareness = ActionAwareness;
            Action_Persuasion = ActionPersuasion;
            Chance_Click = ChanceClick;
            Chance_Action = ChanceAction;
            Num_Impressions = 0;
        }

        public override void modify_agent(AgentData data, double hours)
        {
            data.AddImpression();
            if (data.Aware || rand.NextDouble() < Awareness)
            {
                data.Aware = true;
                data.Persuasion += Persuasion;
                data.Recency += Recency;
                if (rand.NextDouble() < Chance_Click)
                {
                    if (data.Aware || rand.NextDouble() < Click_Awaneness)
                    {
                        data.Aware = true;
                        data.Persuasion += Click_Persuasion;
                        if (rand.NextDouble() < Chance_Action)
                        {
                            data.Persuasion += Action_Persuasion;
                        }
                    }

                }
            }
            
        }

        public override double prob_of_ad(AgentData data, double hours)
        {
            if (Num_Impressions > 0)
            {
                Num_Impressions--;
                return 1.0;
            }

            return 0.0;
        }
    }

    [Serializable]
    public class ReachInfo
    {
        public double Reach { get; set; }

        public double Effeciency { get; set; }

        public double Rate { get; set; }

        public ReachInfo() { }

        public ReachInfo( double reach, double rate, double effeciency ) {
            Reach = reach;
            Rate = rate;
            Effeciency = effeciency;
        }

    }

    [Serializable]
    public class Demographic
    {
        private DemographicType<Age> age = new DemographicType<Age>();
        private DemographicType<Income> income = new DemographicType<Income>();
        private DemographicType<Gender> gender = new DemographicType<Gender>();
        private DemographicType<Race> race = new DemographicType<Race>();
        private DemographicType<ChildStatus> kids = new DemographicType<ChildStatus>();
        private DemographicType<HomeOwner> homeowner = new DemographicType<HomeOwner>();

        public GeoRegion Region {get; set;}
        public string Name { get;  set; }

   

        public uint[] GetDemographicBase()
        {
            uint[] rval = new uint[Demographic.DemoTypes.Count()];

            rval[0] = gender;
            rval[1] = age;
            rval[2] = race;
            rval[3] = income;
            rval[4] = kids;
            rval[5] = income;

            return rval;
        }

        #region operations on type

        public DemographicType<Gender> Gender {
            get {
                return gender;
            }
            set {
                gender = value;
            }
        }
        public DemographicType<Race> Race {
            get {
                return race;
            }
            set {
                race = value;
            }
        }
        public DemographicType<HomeOwner> Homeowner {
            get {
                return homeowner;
            }
            set {
                homeowner = value;
            }
        }

        public DemographicType<ChildStatus> Kids {
            get {
                return kids;
            }
            set {
                kids = value;
            }
        }

        public DemographicType<Age> Age {
            get {
                return age;
            }
            set {
                age = value;
            }
        }

        public DemographicType<Income> Income {
            get {
                return income;
            }
            set {
                income = value;
            }
        }

        #endregion


        public void AddAge( int anAge) {
            age.AddRange( anAge, anAge );
        }

        public void AddAge(int minAge, int maxAge ) {
            age.AddRange( minAge, maxAge );  
        }


        public void AddIncome( int anIncome ) {
            income.AddRange( anIncome, anIncome );  
        }
        public void AddIncome( int minIncome, int maxIncome ) {
            income.AddRange( minIncome, maxIncome );  
        }

        public int AgeMin {
            get {
                return age.Min();
            }
        }


        public int AgeMax {
            get {
                return age.Max();
            }
        }

        public int IncomeMin {
            get {
                return income.Min();
            }
        }

        public int IncomeMax {
            get {
                return income.Max();
            }
        }

        public ReachInfo Info { get; set; }

        public static DemoType[] DemoTypes = new DemoType[] { new Gender(), new Race(), new HomeOwner(), new ChildStatus(), new Age(), new Income() };

        private void init( string[] types )
        {
            if( types.Length != DemoTypes.Length )
            {
                throw new Exception( "wrong initilizer to demographic" );
            }

            // this assumed to be in the same order as above
            Name = types[0] + ":" + types[1] + ":" + types[2] + ":" + types[3] + ":" + types[4] + ":" + types[5];

            Gender = types[0];
            Race = types[1];
            Homeowner = types[2];
            Kids = types[3];
            Age = types[4];
            Income = types[5];
        }

       
        public Demographic( string[] types)
        {
            // no region specified

            Region = null;
            Info = new ReachInfo(0,0,0);

            init( types );
            
        }

        public Demographic( string basicType )
        {
            string[] types = basicType.Split( ':' );


            Region = null;
            Info = new ReachInfo( 0, 0, 0 );

            init( types );
        }

        public Demographic()
        {
            Info = new ReachInfo(0,0,0);
            Name = "Everybody";
            Age = DemographicType<Age>.ANY;
            Income = DemographicType<Income>.ANY;
            Gender = DemographicType<Gender>.ANY;
            Race = DemographicType<Race>.ANY;
            Homeowner = DemographicType<HomeOwner>.ANY;
            Kids = DemographicType<ChildStatus>.ANY;

            Region = null;
        }

        public Demographic(Demographic dem)
        {
            Info = new ReachInfo( 0, 0, 0 );
            Name = dem.Name;
            Age = dem.age;
            income = dem.income;
            Gender = dem.gender;
            Race = dem.race;
            Homeowner = dem.homeowner;
            Kids = dem.kids;

            Region = null;
        }

        /// <summary>
        /// demographic match to a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool Match( Person person )
        {
            if( !(person.Gender & gender ))
            {
                return false;
            }

            if( !(person.Race & race ))
            {
                return false;
            }

            if( person.Age < 18 && !(kids & "YES"))
            {
                return false;
            }

             DemographicType<Age> personAge = DemographicType<Age>.FromValue( person.Age );
             if( !(personAge & age) )
             {
                 return false;
             }

            return true;
        }

        /// <summary>
        /// Is anyone in this house contained in this demographic
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public bool Match(Household house)
        {
            bool match = false;

            foreach( Person person in house.People ) {
                if( Match( person ) )
                {
                    match = true;
                    break;
                }
            }

            if( !match )
            {
                return false;
            }

            // if we must have kids we need to check if this is true
            if( kids > 0 && kids <= "YES" )
            {
                match = false;
                foreach( Person person in house.People )
                {
                    if(person.Age < 18)
                    {
                        match = true;
                        break;
                    }
                }
            }

            if( !match )
            {
                return false;
            }

            match = false;
            if( (house.Type & "NONFAMILY") && (this.homeowner & "RENTER")) {
                match = true;
            }
            if ((house.Type & "FAMILY") && (this.homeowner & "OWNER"))
            {
                match = true;
            }

            if( !match )
            {
                return false;
            }

            DemographicType<Income> houseIncome = DemographicType<Income>.FromValue( house.Income );

            if(!(houseIncome & income)) {
                return false;
            }

            // check region if indicated
            if( Region != null )
            {
                match = Region.ContainsGeoId( house.GeoID );
            }

            if (!match)
            {
                return false;
            }

            // so close...
            return match;
        }

        /// <summary>
        /// Is this demographic a subset of the given demographic
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public bool IsSubsetOf(Demographic demographic)
        {
            if (Race > demographic.Race)
            {
                return false;
            }

            if (Gender > demographic.Gender)
            {
                return false;
            }

            if (Age > demographic.Age)
            {
                return false;
            }

            if (Income > demographic.Income)
            {
                return false;
            }

            if (Homeowner > demographic.Homeowner)
            {
                return false;
            }

            if (Kids > demographic.Kids)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string ret_val = Gender.ToString() + ":";
            ret_val += Race.ToString() + ":";
            ret_val += Age.ToString() + ":";
            ret_val += Income.ToString() + ":";
            ret_val += Homeowner.ToString() + ":";
            ret_val += Kids.ToString();

            return ret_val;
        }

        public static bool Intersect( Demographic demA, Demographic demB )
        {
            if( demA.Region != null && demB.Region != null )
            {
                if( !demA.Region.IsSubRegionOf( demB.Region ) &&
                    !demB.Region.IsSubRegionOf( demA.Region ) )
                {
                    return false;
                }
            }


            if( !(demA.Gender & demB.Gender) )
            {
                return false;
            }

            if( !(demA.age & demB.age) )
            {
                return false;
            }

            if( !(demA.race & demB.race) )
            {
                return false;
            }

            if( !(demA.income & demB.income) )
            {
                return false;
            }

            if( !(demA.homeowner & demB.homeowner) )
            {
                return false;
            }


            if( !(demA.kids & demB.kids) )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// decomposes a list of demographics to a list of primitive demographics
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, Dictionary<string, double>> Factor( List<Demographic> demos )
        {
            // this is the list to be returned
            Dictionary<string, Dictionary<string, double>> rval = new Dictionary<string, Dictionary<string, double>>();
            foreach( Demographic dem in demos )
            {
                Dictionary<string, Dictionary<string, double>> prims = GeoGraph.Factor( dem );

                // add to rval
                foreach( string region in prims.Keys )
                {
                    if( rval.ContainsKey( region ) )
                    {
                        foreach( string baseType in prims[region].Keys )
                        {
                            rval[region][baseType] = Math.Max( rval[region][baseType], prims[region][baseType] );
                        }
                    }
                    else
                    {
                        rval.Add( region, prims[region] );
                    }
                }
            }
            
            return rval; ;
        }       
    }

    public class MediaReach
    {
        public enum DepthType
        {
            Type,
            Subtype,
            Region,
            Vehicle
        }

        private static IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        private static DirectoryInfo top_level;

        private static DepthType my_depth;

        private static Dictionary<Guid, string> vehicle_files;

        private static Dictionary<MediaVehicle.MediaType, DemographicReachInfo> type_reach;

        private static Dictionary<MediaVehicle.MediaType, Dictionary<string, DemographicReachInfo>> subtype_reach;

        private static Dictionary<MediaVehicle.MediaType, Dictionary<string, Dictionary<string, DemographicReachInfo>>> region_reach;

        private static Dictionary<Guid, DemographicReachInfo> vehicle_reach;

        private static int min_score;

        public static int VehicleMemory = 100;

        public static void Reset(DirectoryInfo directory, List<MediaVehicle> vehicles, List<Agent> agents, MediaReach.DepthType depth)
        {
            build_types(directory, vehicles, agents, depth);
        }

        public static void ReadFromDisk(DirectoryInfo directory, DepthType depth)
        {
            type_reach = new Dictionary<MediaVehicle.MediaType, DemographicReachInfo>();
            subtype_reach = new Dictionary<MediaVehicle.MediaType, Dictionary<string, DemographicReachInfo>>();
            region_reach = new Dictionary<MediaVehicle.MediaType, Dictionary<string, Dictionary<string, DemographicReachInfo>>>();
            vehicle_reach = new Dictionary<Guid, DemographicReachInfo>();
            vehicle_files = new Dictionary<Guid, string>();
            min_score = 0;

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                read_types(dir, depth);
            }
        }

        public static void WriteToDisk()
        {
        }

        #region listings
        public static List<MediaVehicle.MediaType> GetTypes()
        {
            return new List<MediaVehicle.MediaType>(type_reach.Keys);
        }

        public static List<string> GetSubTypes(MediaVehicle.MediaType type)
        {
            return new List<string>(subtype_reach[type].Keys);
        }

        public static List<string> GetRegions(MediaVehicle.MediaType type, string subtype)
        {
            return new List<string>(region_reach[type][subtype].Keys);
        }

        public static List<Guid> GetVehicles(MediaVehicle.MediaType type, string subtype, string region)
        {
            return new List<Guid>(vehicle_files.Keys);
        }
        #endregion

        #region reach info
        public static ReachInfo TypeReach(MediaVehicle.MediaType type, List<Demographic> demographics)
        {

            return type_reach[type].Reach(demographics);
        }

        public static ReachInfo SubTypeReach(MediaVehicle.MediaType type, string subtype, List<Demographic> demographics)
        {
            if (my_depth >= DepthType.Subtype)
            {
                return subtype_reach[type][subtype].Reach(demographics);
            }

            return new ReachInfo();
        }

        public static ReachInfo RegionReach(MediaVehicle.MediaType type, string subtype, string region, List<Demographic> demographics)
        {
            if (my_depth >= DepthType.Region)
            {
                return region_reach[type][subtype][region].Reach(demographics);
            }

            return new ReachInfo();
        }

        public static ReachInfo VehicleReach(Guid vehicle, List<Demographic> demographics)
        {
            if (my_depth < DepthType.Vehicle)
            {
                return new ReachInfo();
            }

            if (vehicle_reach.ContainsKey(vehicle))
            {
                return vehicle_reach[vehicle].Reach(demographics);
            }
            else
            {
                FileStream istream = new FileStream(vehicle_files[vehicle], FileMode.Open);
                DemographicReachInfo reach_info = (DemographicReachInfo)formatter.Deserialize(istream);
                ReachInfo info = reach_info.Reach(demographics);
                if (reach_info.Score > min_score)
                {
                    update_memory(vehicle, reach_info);
                }
                return info;
            }
        }

        public static ReachInfo VehicleReach(MediaVehicle vehicle, List<Demographic> demographics)
        {
            return VehicleReach(vehicle.Guid, demographics);
        }
        #endregion

        #region build_trees
        private static Dictionary<MediaVehicle.MediaType, Dictionary<Guid, MediaVehicle>> build_type_tree(List<MediaVehicle> vehicles)
        {
            Dictionary<MediaVehicle.MediaType, Dictionary<Guid, MediaVehicle>> rval = new Dictionary<MediaVehicle.MediaType, Dictionary<Guid, MediaVehicle>>();

            foreach (MediaVehicle vehicle in vehicles)
            {
                if (!rval.ContainsKey(vehicle.Type))
                {
                    rval.Add(vehicle.Type, new Dictionary<Guid, MediaVehicle>());
                }

                rval[vehicle.Type].Add(vehicle.Guid, vehicle);
            }

            return rval;
        }

        private static Dictionary<string, Dictionary<Guid, MediaVehicle>> build_subtype_tree(List<MediaVehicle> vehicles)
        {
            Dictionary<string, Dictionary<Guid, MediaVehicle>> rval = new Dictionary<string, Dictionary<Guid, MediaVehicle>>();
            foreach (MediaVehicle vehicle in vehicles)
            {
                if (!rval.ContainsKey(vehicle.SubType))
                {
                    rval.Add(vehicle.SubType, new Dictionary<Guid, MediaVehicle>());
                }
                
                rval[vehicle.SubType].Add(vehicle.Guid, vehicle);
            }

            return rval;
        }

        private static Dictionary<string, Dictionary<Guid, MediaVehicle>> build_region_tree(List<MediaVehicle> vehicles)
        {
            Dictionary<string, Dictionary<Guid, MediaVehicle>> rval = new Dictionary<string, Dictionary<Guid, MediaVehicle>>();
            foreach (MediaVehicle vehicle in vehicles)
            {
                if (!rval.ContainsKey(vehicle.RegionName))
                {
                    rval.Add(vehicle.RegionName, new Dictionary<Guid, MediaVehicle>());
                }

                rval[vehicle.RegionName].Add(vehicle.Guid, vehicle);
            }

            return rval;
        }

        private static Dictionary<Guid, Dictionary<Guid, MediaVehicle>> build_vehicle_tree(List<MediaVehicle> vehicles)
        {
            Dictionary<Guid, Dictionary<Guid, MediaVehicle>> rval = new Dictionary<Guid, Dictionary<Guid, MediaVehicle>>();
            foreach (MediaVehicle vehicle in vehicles)
            {
                if (!rval.ContainsKey(vehicle.Guid))
                {
                    rval.Add(vehicle.Guid, new Dictionary<Guid, MediaVehicle>());
                }

                rval[vehicle.Guid].Add(vehicle.Guid, vehicle);
            }

            return rval;
        }
        #endregion

        #region write_files
        private static void build_types(DirectoryInfo directory, List<MediaVehicle> media, List<Agent> agents, DepthType depth)
        {
            Dictionary<MediaVehicle.MediaType, Dictionary<Guid, MediaVehicle>> types = build_type_tree(media);

            foreach (MediaVehicle.MediaType type in types.Keys)
            {
                DirectoryInfo folder = directory.CreateSubdirectory(type.ToString().ToUpper());
                string file_name = folder.FullName + "\\" + type.ToString().ToUpper();
                build_reach_file(file_name, types[type], agents);

                if (depth >= DepthType.Subtype)
                {
                    List<MediaVehicle> type_list = new List<MediaVehicle>(types[type].Values);
                    build_subtypes(folder, type_list, agents, depth);
                }
                
            }
        }

        private static void build_subtypes(DirectoryInfo directory, List<MediaVehicle> media, List<Agent> agents, DepthType depth)
        {
            Dictionary<string, Dictionary<Guid, MediaVehicle>> subtypes = build_subtype_tree(media);

            foreach (string subtype in subtypes.Keys)
            {
                DirectoryInfo folder = directory.CreateSubdirectory(subtype.Replace("/","").ToUpper());
                string file_name = folder.FullName + "\\" + subtype.Replace("/", "").ToUpper();
                build_reach_file(file_name, subtypes[subtype], agents);

                if (depth >= DepthType.Region)
                {
                    List<MediaVehicle> subtype_list = new List<MediaVehicle>(subtypes[subtype].Values);
                    build_regions(folder, subtype_list, agents, depth);
                }
            }
        }

        private static void build_regions(DirectoryInfo directory, List<MediaVehicle> media, List<Agent> agents, DepthType depth)
        {
            Dictionary<string, Dictionary<Guid, MediaVehicle>> regions = build_region_tree(media);

            foreach (string region in regions.Keys)
            {
                DirectoryInfo folder = directory.CreateSubdirectory(region.ToUpper());
                string file_name = folder.FullName + "\\" + region.ToUpper();
                List<Agent> region_agents = filter_region(region, agents);
                build_reach_file(file_name, regions[region], region_agents);

                if (depth >= DepthType.Vehicle)
                {
                    List<MediaVehicle> region_list = new List<MediaVehicle>(regions[region].Values);
                    build_vehicles(folder, region_list, region_agents);
                }
            }
        }

        private static void build_vehicles(DirectoryInfo directory, List<MediaVehicle> media, List<Agent> agents)
        {
            Dictionary<Guid, Dictionary<Guid, MediaVehicle>> vehicles = build_vehicle_tree(media);

            foreach (Guid vehicle in vehicles.Keys)
            {
                string file_name = directory.FullName + "\\" + vehicle.ToString();
                build_reach_file(file_name, vehicles[vehicle], agents);
            }
        }

        private static void build_reach_file(string file_name, Dictionary<Guid, MediaVehicle> vehicles, List<Agent> agents)
        {
            DemographicReachInfo reach_info = new DemographicReachInfo();
            foreach (Agent agent in agents)
            {
                foreach (HouseholdMedia media in agent.House.Media)
                {
                    if(vehicles.ContainsKey(media.Guid))
                    {
                        reach_info.MatchAgent(agent);
                    }
                }
            }

            reach_info.BuildSizes(agents);

            FileStream ostream = new FileStream(file_name, FileMode.Create);
            formatter.Serialize(ostream, reach_info);
            ostream.Close();
        }
        #endregion

        #region read_files

        static private void read_types(DirectoryInfo directory, DepthType depth)
        {
            MediaVehicle.MediaType type = MediaVehicle.GetType(directory.Name);
            FileStream istream = new FileStream(directory.FullName + "\\" + type.ToString().ToUpper(), FileMode.Open);
            DemographicReachInfo reach_info = (DemographicReachInfo)formatter.Deserialize(istream);
            type_reach.Add(type, reach_info);
            if (depth >= DepthType.Subtype)
            {
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    read_subtypes(dir, type, depth);
                }
            }
        }

        static private void read_subtypes(DirectoryInfo directory, MediaVehicle.MediaType type, DepthType depth)
        {
            string subtype = directory.Name.ToUpper();
            FileStream istream = new FileStream(directory.FullName + "\\" + subtype, FileMode.Open);
            DemographicReachInfo reach_info = (DemographicReachInfo)formatter.Deserialize(istream);
            if (!subtype_reach.ContainsKey(type))
            {
                subtype_reach.Add(type, new Dictionary<string, DemographicReachInfo>());
            }
            subtype_reach[type].Add(subtype, reach_info);
        }

        static private void read_regions(DirectoryInfo directory, MediaVehicle.MediaType type, string subtype, DepthType depth)
        {
            string region = directory.Name.ToUpper();
            FileStream istream = new FileStream(directory.FullName + "\\" + region, FileMode.Open);
            DemographicReachInfo reach_info = (DemographicReachInfo)formatter.Deserialize(istream);
            if (!region_reach.ContainsKey(type))
            {
                region_reach.Add(type, new Dictionary<string, Dictionary<string, DemographicReachInfo>>());
            }
            if (!region_reach[type].ContainsKey(subtype))
            {
                region_reach[type].Add(subtype, new Dictionary<string, DemographicReachInfo>());
            }
            region_reach[type][subtype].Add(region, reach_info);
        }

        static private void read_vehicles(DirectoryInfo directory, string region)
        {
            vehicle_reach.Clear();
            min_score = -1;
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Name == region)
                {
                    continue;
                }
                FileStream istream = new FileStream(file.FullName, FileMode.Open);
                DemographicReachInfo reach_info = (DemographicReachInfo)formatter.Deserialize(istream);

            }
        }

        #endregion

        #region private functions
        private static List<Agent> filter_region(string region, List<Agent> agents)
        {
            List<Agent> rval = new List<Agent>();
            foreach(Agent agent in agents)
            {
                if (GeoGraph.TopGeo.GetSubRegion(region).ContainsGeoId(agent.House.GeoID))
                {
                    rval.Add(agent);
                }
            }

            return rval;
        }

        private static void update_memory(Guid guid, DemographicReachInfo reach_info)
        {
            Guid min_index = new Guid();
            int score = int.MaxValue;
            foreach (Guid mem_guid in vehicle_reach.Keys)
            {
                if (vehicle_reach[mem_guid].Score < score)
                {
                    score = vehicle_reach[mem_guid].Score;
                    min_index = mem_guid;
                }
            }

            if (reach_info.Score > score)
            {
                vehicle_reach.Remove(min_index);
                vehicle_reach.Add(guid, reach_info);
                min_score = reach_info.Score;
            }
        }
        #endregion

        private class FileDictionaryPair<KType, VType>
        {
            private Dictionary<KType, VType> dictionary;
            private string file;

            public FileDictionaryPair(string file_name)
            {
                dictionary = new Dictionary<KType,VType>();
                file = file_name;
            }

            public string File
            {
                get
                {
                    return file;
                }
            }

            public Dictionary<KType,VType>.KeyCollection Keys
            {
                get
                {
                    return dictionary.Keys;
                }
            }

            public Dictionary<KType,VType>.ValueCollection Values
            {
                get
                {
                    return dictionary.Values;
                }
            }

            public int Count
            {
                get
                {
                    return dictionary.Count;
                }
            }

            public void Add(KType key, VType value)
            {
                if(dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                }
                else
                {
                    dictionary.Add(key, value);
                }
            }

            public VType this[KType key]
            {
                get
                {
                    return dictionary[key];
                }
            }
        }

        [Serializable]
        private class DemographicReachInfo
        {
            private List<double> reach;

            private List<double> sizes;

            private int score;

            private int total_reach;

            public DemographicReachInfo()
            {
                reach = new List<double>();
                foreach (Demographic demo in GeoGraph.PrimitiveTypes)
                {
                    reach.Add(0.0);
                }
                score = 0;
            }

            public int Score
            {
                get
                {
                    return score;
                }
            }

            public ReachInfo Reach(List<Demographic> demographics)
            {
                ReachInfo reach_info = new ReachInfo();

                List<double> size = new List<double>(reach.Count);

                for (int i = 0; i < GeoGraph.PrimitiveTypes.Count; i++)
                {
                    if (Match(i, demographics))
                    {
                        reach_info.Reach += reach[i]/sizes[i];
                        reach_info.Effeciency += reach[i];
                    }
                }

                reach_info.Effeciency /= total_reach;

                score++;

                return reach_info;
            }

            public void MatchAgent(Agent agent)
            {
                for (int i = 0; i < GeoGraph.PrimitiveTypes.Count; i++)
                {
                    if (GeoGraph.PrimitiveTypes[i].Match(agent.House))
                    {
                        reach[i]++;
                    }
                }

                total_reach++;
            }

            public void BuildSizes(List<Agent> agents)
            {
                foreach (Agent agent in agents)
                {
                    for (int i = 0; i < GeoGraph.PrimitiveTypes.Count; i++)
                    {
                        if (GeoGraph.PrimitiveTypes[i].Match(agent.House))
                        {
                            sizes[i]++;
                        }
                    }
                }
            }

            private bool Match(int index, List<Demographic> demographics)
            {
                foreach (Demographic demographic in demographics)
                {
                    if (GeoGraph.PrimitiveTypes[index].IsSubsetOf(demographic))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
