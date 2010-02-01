using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


using System.Runtime.Serialization.Formatters.Binary;

namespace GeoLibrary
{
    /// <summary>
    /// represents the leaves of the geo hierarchy
    /// </summary>
    [Serializable]
    public class GeoInfo : GeoRegion
    {
        private int geo_id = 0;
        public int GeoID
        {
            get
            {
                return geo_id;
            }
            set
            {
                if (GeoRegion.GeoLibrary.ContainsKey(geo_id.ToString()))
                {
                    GeoRegion.GeoLibrary.Remove(geo_id.ToString());
                    GeoRegion.GeoLibrary.Add(value.ToString(), this);
                }
                geo_id = value;
            }
        }

        public double Lat { get; set; }
        public double Long { get; set; }

        public override void AddToLibrary()
        {
            if (!GeoRegion.GeoLibrary.ContainsKey(geo_id.ToString()))
            {
                GeoRegion.GeoLibrary.Add(geo_id.ToString(), this);
            }
        }

        public override bool ContainsGeoId( int geoId )
        {
            return GeoID == geoId;
        }

        public override GeoInfo this[int geoId]
        {
            get
            {
                if( GeoID == geoId )
                {
                    return this;
                }

                return null;
            }
        }

        public override void ResetBoundaries()
        {
            Box.Max.x = Long;
            Box.Max.y = Lat;

            Box.Min = Box.Max;
        }

        public GeoInfo() 
        {
            Type = RegionType.City;
        }

        public GeoInfo( int geoId )
        {
            Type = RegionType.City;
            geo_id = geoId;
        }
    }

    /// <summary>
    /// represents geogrpahic hierarchy
    /// comparison is done by NAME
    /// </summary>

    [Serializable]
    public class GeoRegion
    {
        private static GeoRegion top_geo = null;
        public static GeoRegion TopGeo
        {
            get
            {
                return top_geo;
            }
        }

        public static void SetTopRegion(GeoRegion region)
        {
            top_geo = region;
        }

        #region enums
        public enum RegionType
        {
            Country,
            State,
            DMA,
            County,
            City
        }
        #endregion

        public static Dictionary<string, Dictionary<string, List<string>>> RegionMap = new Dictionary<string, Dictionary<string, List<string>>>();

        public static Dictionary<string, List<string>> GetRegionMap( string type )
        {
            return RegionMap[type];
        }

        public static Dictionary<string, List<string>> CreateNewMap( string type )
        {
            Dictionary<string, List<string>> rval = new Dictionary<string, List<string>>();

            RegionMap.Add( type, rval );

            return rval;
        }

        public static Dictionary<string, GeoRegion> GeoLibrary = new Dictionary<string, GeoRegion>();

        public static string[] GetTopRegions(string type)
        {
            string[] regions = new string[RegionMap[type].Keys.Count];

            int index = 0;
            foreach( string state in RegionMap[type].Keys )
            {
                regions[index] = state;
                index++;
            }

            return regions;
        }

        public static string[] GetAssociatedRegions( string type, string state )
        {
            return RegionMap[type][state].ToArray();
        }

        #region member data
        private List<GeoRegion> children = null;
        private GeoRegion parent = null;
        private string my_name;


        public BoundingBox Box = new BoundingBox();

        [NonSerialized]
        public Shape ShapeTag = null;

        #endregion

        #region creation & File IO
        // used only by GeoInfo
        protected GeoRegion()
        {
        }

        
        public GeoRegion(string name, RegionType type)
        {
            children = new List<GeoRegion>();
            Name = name;
            Type = type;

            string searchName = nameClean(name);

            if( !GeoLibrary.ContainsKey( searchName ) )
            {
                GeoLibrary.Add( searchName, this );
            }
        }

        public override string  ToString()
        {
            return Name;
        }

        #endregion
        // every region is contained in another -- except the world of course

        #region FILE IO

        private const string geoFile = @"\geo.dat";
        public static string ReadFromFile( string dirName )
        {

            // this needs to read in the world structure from disk
            string fileName = dirName + geoFile;
            BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            FileStream file = null;
            try
            {
                file = new FileStream( fileName, FileMode.Open, FileAccess.Read );
            }
            catch( Exception e )
            {
                return e.Message;
            }

            try
            {
                top_geo = (GeoRegion)formatter.Deserialize( file );
            }
            catch( Exception e2 )
            {
                top_geo = null;

                return e2.Message;
            }

            file.Close();

            top_geo.AddToLibrary();

            return null;
        }


        public static string WriteToFile( string dirName)
        {
            if( top_geo == null )
            {
                return "TopGeo Object Null";
            }

            // this needs to read in the world structure from disk

            string fileName = dirName + geoFile;
            BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            try
            {
                FileStream file = new FileStream( fileName, FileMode.Create );

                formatter.Serialize( file, top_geo );

                file.Close();
            }
            catch( Exception e )
            {
                return e.Message;
            }

            return null;
        }

        #endregion


        #region public properties

        public GeoRegion Parent
        {
            get
            {
                return parent;
            }
        }

        public int Id { get; set; }

        public double AveLat
        {
            get
            {
                return -0.5 * (Box.Min.y + Box.Max.y);
            }
        }

        public double AveLng
        {
            get
            {
               
                double max = Box.Max.x;

                if( max < -180 )
                {
                    max = max + 360;
                }

                double min = Box.Min.x;

                if( min < -180 )
                {
                    min = min + 360;
                }

                return 0.5 * (min + max);
            }
        }

        //Public Type
        public RegionType Type { get; set; }

        // Public name
        public string Name
        {
            get
            {
                return my_name;
            }
            set
            {
                string cleanName = null;
                if( my_name != null )
                {
                    cleanName = nameClean(my_name );

                    if( GeoRegion.GeoLibrary.ContainsKey( cleanName ) )
                    {
                        GeoRegion.GeoLibrary.Remove( cleanName );
                    }
                }

                my_name = value;

                cleanName = nameClean( my_name );

                if( !GeoLibrary.ContainsKey( cleanName ) )
                {
                    GeoRegion.GeoLibrary.Add( cleanName, this );
                }
            }
        }

        public List<GeoRegion> SubRegions
        {
            get
            {
                return children;
            }
        }

        public GeoRegion GetDMA()
        {
            if( parent == null )
            {
                return this;
            }

            if( Type == RegionType.DMA )
            {
                return this;
            }

            return parent.GetDMA();
        }

        #endregion

        #region public methods

        /// <summary>
        /// UI maintains the regions in (state, dma) list
        /// this tranlsates into a list of geo regions
        /// This routine does not check for the top geoRegion being in list
        /// </summary>
        /// <param name="UIRegions"></param>
        /// <returns></returns>
        public List<GeoRegion> StringsToGeoRegions( List<List<string>> UIRegions )
        {
            List<GeoRegion> rval = new List<GeoRegion>();
            foreach( List<string> stateDmaList in UIRegions )
            {
                string dmaName = stateDmaList[1];

                rval.Add( GeoRegion.TopGeo.GetSubRegion( dmaName ) );
            }

            return rval;
        }

        private string nameClean(string name)
        {
            return name.Replace( " ", "" ).ToUpper();
        }

        public virtual void AddToLibrary()
        {
            string cleanName = nameClean(Name);

            if (!GeoLibrary.ContainsKey(cleanName))
            {
                GeoLibrary.Add( cleanName, this );
            }

            foreach (GeoRegion child in children)
            {
                child.AddToLibrary();
            }
        }

        public void RemoveShapeTags()
        {
            ShapeTag = null;

            if( children != null )
            {
                foreach( GeoRegion reg in children )
                {
                    reg.RemoveShapeTags();
                }
            }
        }

        virtual public void ResetBoundaries()
        {
            Box.Reset();

            if( ShapeTag != null )
            {
                ShapeTag.ResetBoundaries();
                Box = ShapeTag.Box;
            }
            else if ( children != null)
            {
                foreach( GeoRegion reg in children )
                {
                    reg.ResetBoundaries();

                    Box.Add( reg.Box );
                }
            }
        }

        /// <summary>
        /// Checks for Name among children
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public GeoRegion AddSubRegion( string name, GeoRegion.RegionType type )
        {
            // check if we already have this
            foreach( GeoRegion reg in children )
            {
                if( reg.Name == name )
                {
                    return reg;
                }
            }

            // create a new region as a subregion
            GeoRegion sub = new GeoRegion(name, type);

            children.Add( sub );
            sub.parent = this;

            return sub;
        }

        /// <summary>
        /// Add info to this region
        /// </summary>
        /// <param name="reg"></param>
        public void Add( GeoRegion reg )
        {
            children.Add( reg );

            reg.parent = this;
        }

        public GeoRegion GetSubRegion( string name )
        {
            if( name == null )
            {
                return null;
            }

            if( this.Name.ToUpper() == name.ToUpper() )
            {
                return this;
            }

            if( children == null )
            {
                return null;
            }


            string cleanName = nameClean(name);

            if( GeoLibrary.ContainsKey( cleanName ) )
            {
                GeoRegion region = GeoLibrary[cleanName];
                if (this.IsAncestorOf(region))
                {
                    return region;
                }
                return null;
            }

            // check children
            foreach( GeoRegion reg in children )
            {
                GeoRegion sub = reg.GetSubRegion( name );

                if( sub != null )
                {
                    return sub;
                }
            }

            return null;
        }

        public virtual bool IsAncestorOf(GeoRegion region)
        {
            if (this.Name == region.Name)
            {
                return true;
            }

            if (region.Parent == null)
            {
                return false;
            }

            return this.IsAncestorOf(region.Parent);
        }

        public virtual bool ContainsGeoId( int geoId )
        {
            if( this.parent == null )
            {
                // I am the top
                return true;
            }

            string name = geoId.ToString();
            
            if (GeoLibrary.ContainsKey(name))
            {
                GeoRegion region = GeoLibrary[name];
                if (this.IsAncestorOf(region))
                {
                    return true;
                }
                return false;
            }

            foreach( GeoRegion child in children )
            {
                if( child.ContainsGeoId( geoId ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if this GeoRegion contains a GeoRegion of the specified name or is the root
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool ContainsSubregion(string name)
        {
            if (this.parent == null)
            {
                // I am the top
                return true;
            }

            if (this.Name == name)
            {
                return true;
            }

            foreach (GeoRegion child in children)
            {
                if (child.ContainsSubregion(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the GeoRegion of the specified name or null if there is no region with the specified name in any subregion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GeoRegion FindRegion(string name)
        {
            if (this.Name.ToUpper().Contains(name.ToUpper()))
            {
                return this;
            }

            if (children == null)
            {
                return null;
            }

            foreach (GeoRegion child in children)
            {
                GeoRegion region = child.GetSubRegion(name);
                if (region != null)
                {
                    return region;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a region of the specified type at or above this region, if no region of type exists returns null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GeoRegion GetType(RegionType type)
        {
            if (this.Type == type)
            {
                return this;
            }

            if (this.Parent == null)
            {
                return null;
            }

            return this.Parent.GetType(type);
        }

        /// <summary>
        /// Returns all regions of a specified type under this region
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<GeoRegion> GetRegions(RegionType type)
        {
            List<GeoRegion> regions = new List<GeoRegion>();

            AddByType(type, regions);

            return regions;
        }

        /// <summary>
        /// geo.SubRegionNames return the names of the immediate children
        /// </summary>
        /// <returns></returns>
        public string[] SubRegionNames()
        {
            if( children == null )
            {
                return null;
            }

            string[] names = new string[children.Count];


            for( int index = 0; index < children.Count; ++index )
            {
                if( children[index].SubRegions == null )
                {
                    names = new string[0];
                    break;
                }

                names[index] = children[index].Name;
            }

            return names;

        }

        /// <summary>
        /// The following syntax is supported
        /// if us is the Georegion corresponding to the US
        /// then us["CA"]} is the geoRegion corresponding to CA
        /// geo["CA"]["SC"] is the subregion "SC" of CA
        /// </summary>
        /// <returns></returns>
        public GeoRegion this[string name]
        {
            get
            {
                return GetSubRegion( name );
            }
        }

        public virtual GeoInfo this[int geoId]
        {
            get
            {
                // check if in libray
                string name = geoId.ToString();

                if( GeoLibrary.ContainsKey( name ) )
                {
                    return (GeoInfo)GeoLibrary[name];
                }
                
                foreach( GeoRegion reg in children )
                {
                    GeoInfo info = reg[geoId];

                    if( info != null )
                    {
                        return info;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Checks if a subregion OR equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSubRegionOf( GeoRegion other )
        {
            if( this.Name == other.Name )
            {
                return true;
            }

            if( other.SubRegions != null )
            {
                foreach( GeoRegion sub in other.SubRegions )
                {
                    if( IsSubRegionOf( sub ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

      
        private void AddByType(RegionType type, List<GeoRegion> curr_list)
        {
            if (this.Type == type)
            {
                curr_list.Add(this);
            }

            if (children == null)
            {
                return;
            }

            foreach (GeoRegion child in children)
            {
                child.AddByType(type, curr_list);
            }
        }

        #endregion
    }
}
