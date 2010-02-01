using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HouseholdLibrary;
using DemographicLibrary;
using GeoLibrary;

using System.IO;

namespace MediaLibrary
{
    [Serializable]
    public class Agent
    {
        #region Agent Stats
        /// <summary>
        /// Used when generating media database
        /// </summary> 

        static public bool KeepStats = false;
  
        // media -> media subtype -> region -> vec
        static public Dictionary<string, Dictionary<string, Dictionary<string, Demographic.PrimVector>>> MediaDemoStore = new Dictionary<string, Dictionary<string, Dictionary<string, Demographic.PrimVector>>>();
        static public Dictionary<Guid, double> VehicleSize = new Dictionary<Guid, double>();
       
        static public void ClearStats()
        {
            MediaDemoStore.Clear();
            VehicleSize.Clear();
        }

        #endregion
        private Household house;

        [NonSerialized]
        Dictionary<Guid, double> my_media = null;

        public Household House
        {
            get
            {
                return house;
            }
        }

        public Agent(Household house)
        {
            this.house = house;
        }


        public bool AddMedia(MediaVehicle media, Double rate)
        {
            bool rval = true;

            House.AddMedia( (int) media.Type, new HouseholdMedia( media.Guid, rate ) );

            if( KeepStats )
            {
                // vehicle size
                if( !VehicleSize.ContainsKey( media.Guid ) )
                {
                    VehicleSize.Add( media.Guid, 0 );
                }

                VehicleSize[media.Guid] += 1;

                string mediaType = media.Type.ToString();
                if( !MediaDemoStore.ContainsKey( mediaType ) )
                {
                    MediaDemoStore.Add( mediaType, new Dictionary<string, Dictionary<string, Demographic.PrimVector>>() );
                }

                Dictionary<string, Dictionary<string, Demographic.PrimVector>> subTypeStats = MediaDemoStore[mediaType];

                if( !subTypeStats.ContainsKey( media.SubType ) )
                {
                    subTypeStats.Add( media.SubType, new Dictionary<string, Demographic.PrimVector>() );
                }

                Dictionary<string, Demographic.PrimVector> regionStats = subTypeStats[media.SubType];

                string regionName = media.RegionName;
                if(regionName == GeoRegion.TopGeo.Name )
                {

                    GeoRegion geo = GeoRegion.TopGeo.GetSubRegion( this.House.GeoID.ToString() );

                    if( geo != null )
                    {
                        GeoRegion dma = geo.GetDMA();

                        regionName = dma.Name;
                    }
                }

                if( !regionStats.ContainsKey(regionName ) )
                {
                    regionStats.Add(regionName, new Demographic.PrimVector() );
                }

                Demographic.PrimVector vec = regionStats[regionName];

                bool match = false;
                for( int ii = 0; ii < Demographic.PrimitiveTypes.Count; ++ii )
                {
                    if( House.Match( Demographic.PrimitiveTypes[ii] ) )
                    {
                        vec[ii] += 1;

                        match = true;
                    }
                }

                if( match )
                {
                    vec.Any += 1;
                }
                else
                {
                    rval = false;
                }
            }

            return rval;
        }


        public double GetRate(Guid guid)
        {
            if (my_media.ContainsKey(guid))
            {
                return my_media[guid];
            }
            return 0.0;
        }

        public bool HasMedia(Guid guid)
        {
            if( my_media == null )
            {
                my_media = new Dictionary<Guid,double>();
                foreach( MediaVehicle.MediaType type in Enum.GetValues( typeof( MediaVehicle.MediaType ) ) )
                {
                    List<HouseholdMedia> hh_media = House.Media( (int)type );

                    foreach( HouseholdMedia media in hh_media )
                    {
                        my_media.Add( media.Guid, media.Rate );
                    }
                }
            }


            if (!my_media.ContainsKey(guid))
            {
                return false;
            }

            return true;
        }

        public Guid[] MyMedia
        {
            get
            {
                if (my_media == null)
                {
                    return new Guid[0];
                }

                return my_media.Keys.ToArray();
            }
        }

        // clears out media of a certian type
        public void ClearMedia()
        {
            if (my_media != null)
            {
                my_media.Clear();
            }
        }

        public void Print( TextWriter writer, string prefix )
        {
            House.Print( writer, prefix );
        }


        [Serializable]
        private class AgentMedia
        {
            private MediaVehicle media;
            private Double rate;

            public MediaVehicle Media { get { return media; } }
            public Double Rate { get { return rate; } set { rate = value; } }

            public AgentMedia(MediaVehicle media, Double rate)
            {
                this.media = media;
                this.rate = rate;
            }
        }
    }
}
