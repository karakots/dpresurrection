using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DemographicLibrary;
using GeoLibrary;

namespace MediaLibrary
{
    public class MediaVehicle
    {
        
        static public List<MediaVehicle.MediaType> MediaTypes = new List<MediaVehicle.MediaType>();
        static public Dictionary<MediaVehicle.MediaType, string> MediaGrpString = new Dictionary<MediaVehicle.MediaType, string>();
        static MediaVehicle()
        {
            foreach( MediaVehicle.MediaType type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
            {
                MediaTypes.Add( type );
                MediaGrpString.Add( type, "GRP-" + type.ToString() );
            }
        }

        #region Constants
        public enum MediaType
        {
            Magazine,
            Radio,
            Internet,
            Newspaper,
            Yellowpages
        }
        public enum AdCycle
        {
            Instant,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Bimonthly,
            Quarterly,
            Yearly
        }

        public static MediaVehicle.AdCycle GetCycle( string cycle )
        {
            switch( cycle )
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

        public static MediaVehicle.MediaType GetType( string type )
        {
            switch( type.ToUpper() )
            {
                case "MAGAZINE":
                    return MediaVehicle.MediaType.Magazine;
                case "RADIO":
                    return MediaVehicle.MediaType.Radio;
                case "YELLOWPAGES":
                    return MediaVehicle.MediaType.Yellowpages;
                case "INTERNET":
                    return MediaVehicle.MediaType.Internet;
                case "NEWSPAPER":
                    return MediaVehicle.MediaType.Newspaper;
                default:
                    return MediaVehicle.MediaType.Magazine;
            }
        }
        #endregion

     
        #region Properties from db
        public Guid Guid
        {
            get
            {
                return vclRow.id;
            }
        }

    
        public AdCycle Cycle
        {
            get
            {
                return (AdCycle)vclRow.cycle;
            }
        }

        public string Vehicle
        {
            get
            {
                return vclRow.name;
            }
        }

        public double CPM
        {
            get
            {
                return vclRow.cpm;
            }

            set
            {
                vclRow.cpm = value;
            }
        }

        public string URL
        {
            get
            {
                return vclRow.info;
            }
        }

        #endregion

        public string SubType { get; set; }

        public MediaType Type { get; set; }

        public double Rating { get; set; }

        public double Size { get; set; }

        public string RegionName {get; set;}

        public MediaVehicle( Media.vehicleRow row, MediaVehicle.MediaType type, string subtype, string region, double size, Dictionary<int, AdOption> opts )
        {
            vclRow = row;
            Type = type;
            SubType = subtype;
            Size = size;
            RegionName = region;
            options = opts;
        }


        #region Private Fields
        private Media.vehicleRow vclRow;
        private Dictionary<int, AdOption> options;
        #endregion

        public Dictionary<int, AdOption> GetOptions()
        {
            return options;
        }

        public void ExcludeOptions( IEnumerable<Media.anti_vcl_optionRow> exclude )
        {
            if( exclude != null )
            {
                Dictionary<int, AdOption>  new_options = new Dictionary<int, AdOption>();

                foreach( int id in options.Keys )
                {
                    if( !exclude.Any( tmp => tmp.option_id == id ) )
                    {
                        new_options.Add( id, options[id] );
                    }
                }

                options = new_options;
            }
        }

        public void AdjustSize( double scale )
        {
            if( Type == MediaType.Magazine && RegionName == "USA" && vclRow.geo_info.Contains("D"))
            {
                Size *= scale;
            }
        }
    }

    [Serializable]
    public class AgentData
    {
        private static Random rand = new Random();
        public static double PersuasionDecay = 0.02;
        public static double AwareDecay = 0.02;
        public static double RecencyDecay = 0.1;
        public static int ImpressionLength = 30;
        public static int Max_Impressions = 4;

        public Agent agent = null;
        public List<Demographic> Demographics = new List<Demographic>();
        public List<Guid> active_media = new List<Guid>();
        public double shopProb = 1;
        public double Persuasion;
        public bool Aware;
        public double Recency;
        private bool in_consideration;
        public bool MadeChoice;
        public double ActionTaken;

        public int demoIndex = -1;

        private List<int> Impressions;
        public int ConsiderationImpressions;
        public Dictionary<MediaVehicle.MediaType, int> ImpressionsToday = null;

        public int num_online_impressions = 0;

        public AgentData(Agent agentIn)
        {
            Persuasion = 0;
            Aware = false;
            Recency = 0;
            InConsideration = false;
            ActionTaken = 0.0;
            MadeChoice = false;
            agent = agentIn;
        }

        public AgentData(AgentData data)
        {
            agent = data.agent;
            Demographics = data.Demographics;
            active_media = data.active_media;
            shopProb = data.shopProb;
            Persuasion = data.Persuasion;
            Aware = data.Aware;
            Recency = data.Recency;
            in_consideration = data.InConsideration;
            ActionTaken = data.ActionTaken;
            MadeChoice = data.MadeChoice;
            Impressions = new List<int>();
            for (int i = 0; i < data.Impressions.Count; i++)
            {
                Impressions.Add(data.Impressions[i]);
            }

            ImpressionsToday = new Dictionary<MediaVehicle.MediaType, int>();

            foreach( MediaVehicle.MediaType type in MediaVehicle.MediaTypes )
            {
                ImpressionsToday.Add( type, 0 );
            }
        }

        public void Reset()
        {
            Persuasion = 0;
            Aware = false;
            Recency = 0;
            Demographics.Clear();
            Impressions = new List<int>();
            for (int i = 0; i < AgentData.Max_Impressions; i++)
            {
                Impressions.Add(0);
            }

            ConsiderationImpressions = 0;

            ImpressionsToday = new Dictionary<MediaVehicle.MediaType, int>();

            foreach( MediaVehicle.MediaType type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
            {
                ImpressionsToday.Add( type, 0 );
            }

            InConsideration = false;
            ActionTaken = 0.0;
            MadeChoice = false;

            num_online_impressions = 0;
        }

        public bool AddDemographic(Demographic demo)
        {
            GeoRegion geoRegion = GeoRegion.TopGeo.GetSubRegion(demo.Region);
            if (agent.House.Match(demo) && (geoRegion == null || geoRegion.ContainsGeoId(agent.House.GeoID)))
            {
                Demographics.Add(demo);

                return true;
            }

            return false;
        }

        public bool InConsideration
        {
            set
            {

                in_consideration = value;

                if (in_consideration == false)
                {
                    ConsiderationImpressions = 0;
                }
            }
            get
            {
                return in_consideration;
            }
        }

        public void AddImpression(MediaVehicle.MediaType type)
        {
            Impressions[min_impression_index()] = ImpressionLength;

            ImpressionsToday[type]++;

            if (in_consideration)
            {
                ConsiderationImpressions++;
            }
        }

        public int NumImpressions()
        {
            int num = 0;
            for (int i = 0; i < Impressions.Count; i++)
            {
                if (Impressions[i] > 0)
                {
                    num++;
                }
            }

            return num;
        }

        public void Decay()
        {
            foreach( MediaVehicle.MediaType type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
            {
                ImpressionsToday[type] = 0;
            }

            for (int i = 0; i < Impressions.Count; i++)
            {
                Impressions[i]--;
            }

            Recency = 0;
            ActionTaken = 0.0;
            MadeChoice = false;

            if (rand.NextDouble() < AwareDecay)
            {
                Aware = false;
                Persuasion = 0;
            }
            else
            {
                Persuasion *= (1 - PersuasionDecay);
            }

            num_online_impressions = 0;
        }

        private int min_impression_index()
        {
            int min_index = 0;
            for (int i = 1; i < Impressions.Count; i++)
            {
                if (Impressions[i] < Impressions[min_index])
                {
                    min_index = i;
                }
            }
            return min_index;
        }

        public void Print( TextWriter writer, string prefix )
        {
            agent.House.Print( writer, prefix );
        }

        public void PrintStatic(TextWriter writer, string prefix)
        {
            writer.WriteLine(prefix + "DEMOGRAPHIC DETAILS");
            for (int i = 0; i < Demographics.Count; i++)
            {
                writer.WriteLine(prefix + "DEMOGRAPHIC " + (i + 1).ToString() + " DETAILS");
                writer.WriteLine(prefix + "NAME, " + Demographics[i].Name);
                writer.WriteLine(prefix + "REGION, " + Demographics[i].Region);
                writer.WriteLine(prefix + "RACE, " + Demographics[i].Race.ToString());
                writer.WriteLine(prefix + "GENDER, " + Demographics[i].Gender.ToString());
                writer.WriteLine(prefix + "AGE, " + Demographics[i].Age.ToString());
                writer.WriteLine(prefix + "INCOME, " + Demographics[i].Income.ToString());
                writer.WriteLine(prefix + "KIDS, " + Demographics[i].Kids.ToString());
            }

            // TBD  Need to fix this - if we still want it aorunf
            //writer.WriteLine(prefix + "ACTIVE MEDIA DETAILS");
            //int j = 1;
            //foreach (MediaVehicle vehicle in active_media.Values)
            //{
            //    writer.WriteLine(prefix + "MEDIA " + j.ToString() + " DETAILS");
            //    writer.WriteLine(prefix + "ID, " + vehicle.Guid.ToString());
            //    writer.WriteLine(prefix + "VEHICLE, " + vehicle.Vehicle);
            //    j++;
            //}
        }

        public void PrintDynamic(TextWriter writer, string prefix)
        {
            writer.WriteLine(prefix + "AWARE, " + Aware.ToString());
            writer.WriteLine(prefix + "PERSAUSION, " + Persuasion.ToString());
            writer.WriteLine(prefix + "RECENCY, " + Recency.ToString());
            writer.WriteLine(prefix + "IN CONSIDERATION, " + InConsideration.ToString());
            writer.WriteLine(prefix + "ACTION TAKEN, " + ActionTaken.ToString());
            writer.WriteLine(prefix + "NUM IMPRESSIONS, " + NumImpressions().ToString());
            writer.WriteLine(prefix + "IN CONSIDERATION IMPRESSIONS, " + ConsiderationImpressions.ToString());
        }

        public string GetDynamicString()
        {
            string rval = Aware.ToString() + ",";
            rval += Persuasion.ToString() + ",";
            rval += Recency.ToString() + ",";
            rval += InConsideration.ToString() + ",";
            rval += ActionTaken.ToString() + ",";
            rval += NumImpressions().ToString() + ",";
            rval += ConsiderationImpressions.ToString() + ",";
            return rval;
        }
    }

    [Serializable]
    public class MediaComp
    {
        //
        // To be added later
        public MediaVehicle.MediaType Type;

        // specifies the media part
        public Guid Guid = new Guid(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        //public double Strength = 0.02;

        // the day  the media component begins
        // simulation begins on day 0
        public int StartDate = 0;

        // default is seven days
        public int Span = 7;

        public double Impressions = 0;

        public List<string> target_regions = new List<string>();
        public List<Demographic> target_demogrpahic = new List<Demographic>();
        public double demo_fuzz_factor = 0.0;

        public double region_fuzz_factor = 0.0;

        public int ad_option;

        public MediaComp(MediaVehicle.MediaType type) 
        {
            Type = type;
        }
    }
}
