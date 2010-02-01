using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using GeoLibrary;
using HouseholdLibrary;

using MediaLibrary;

namespace GeoFormatter
{
    public partial class GeoFormatter : Form
    {
        #region private members

        private Country country;

        private static string[] space = new string[] { " " };

        private GeoRegion usa;
        #endregion

        public GeoFormatter()
        {
            InitializeComponent();

            map = new Bitmap( 640, 480, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

            country = new Country( "US" );

            destRect = this.splitContainer1.Panel2.ClientRectangle;

            srcRect = new Rectangle( 0, 0, curWidth, curHeight );

            this.webFolderLabel.Text = Properties.Settings.Default.webDir;
            this.geoFolderlabel.Text = Properties.Settings.Default.geoDir;

            usa = new GeoRegion( "USA", GeoRegion.RegionType.Country );

        }

        #region UI controls

        private void saveBut_Click( object sender, EventArgs e )
        {
            // check for directory to save to
            DirectoryInfo di = new DirectoryInfo( Properties.Settings.Default.webDir );

            if( !di.Exists )
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();
                browse.ShowNewFolderButton = false;
                browse.Description = "Select the web folder to place save to";

                if( browse.ShowDialog() != DialogResult.OK )
                {
                    return;
                }

                Properties.Settings.Default.webDir = browse.SelectedPath;

                Properties.Settings.Default.Save();

                di = new DirectoryInfo( Properties.Settings.Default.webDir );
            }

            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            FileStream file = new FileStream( di.FullName + @"\shape.geo", FileMode.OpenOrCreate );

            formatter.Serialize( file, country );
            file.Close();


            // create file that has the state - dma relationship
            StreamWriter stateDmaWriter = new StreamWriter( di.FullName + @"\stateToDma.csv" );

            foreach( GeoRegion dma in usa.SubRegions )
            {
                bool hasState = false;
                // find all states this dma touches
                foreach( State state in country.States.Values )
                {
                    // does this state and dma chare a county ?

                    bool writeLine = false;
                    foreach( County county in state.Counties.Values )
                    {
                        if( dma.GetSubRegion( county.Name + ", " + state.Name ) != null )
                        {
                            writeLine = true;
                            break;
                        }
                    }

                    if( writeLine )
                    {
                        stateDmaWriter.WriteLine( state.Name + ", " + dma.Name );
                        hasState = true;
                    }
                }

                if( !hasState )
                {
                    throw new Exception( "oops" );
                }
            }

            stateDmaWriter.Close();

            // do not need this
            usa.RemoveShapeTags();

            GeoRegion.SetTopRegion(usa);

            GeoRegion.WriteToFile( di.FullName );
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            // check for directory to read from

            DirectoryInfo di = new DirectoryInfo( Properties.Settings.Default.geoDir );

            if( !di.Exists )
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();
                browse.ShowNewFolderButton = false;


                browse.Description = "Select the folder that has the region information";
                if( browse.ShowDialog() != DialogResult.OK )
                {
                    return;
                }

                Properties.Settings.Default.geoDir = browse.SelectedPath;

                Properties.Settings.Default.Save();

                di = new DirectoryInfo( Properties.Settings.Default.geoDir );
            }


            // step one - process the state and county files
            parse_state_files();
            parse_county_files();

            // step two : remove Puerto Rico (state code 72)
            // and set some boundaries
            country.States.Remove( "72" );
            // country.ResetBoundaries();

            // step three
            // read in the state code name to name file
            // and then process the county-dma map
            // and create the region tree
            readStateCode();
            parseCountyDmaFile();
           // createRegionTree();

            usa.ResetBoundaries();

          
            // finally add the cities to the region tree
            processCities();

            // build tree
            this.regionTree.Nodes.Clear();

            this.regionTree.Nodes.Add( createTree( usa ) );

            selectedRegion = usa;
           
            GetMap();

            this.Refresh();
        }

        #endregion

        
        #region private functions

        private void parse_state_files()
        {
            country = new Country("USA");
            StreamReader region_info = new StreamReader(Properties.Settings.Default.geoDir + @"\st99_d00a.dat");

            int region_id = 0;

            while( !region_info.EndOfStream )
            {
                while( !region_info.EndOfStream && !Int32.TryParse( region_info.ReadLine(), out region_id ) ) { };
                if( region_id == 0 )
                {
                    continue;
                }

                char[] trim = new char[] { '\"', ' ' };

                string state_id = region_info.ReadLine().TrimEnd( trim ).TrimStart( trim );
                string state = region_info.ReadLine();
                // string county = region_info.ReadLine();
                string state_name = state.Substring( 2, state.Length - 3 );
                if( !country.States.ContainsKey( state_id ) )
                {
                    country.States.Add( state_id, new State( state_name, state_id ) );
                }
            }

            region_info.Close();
        }

        private void parse_county_files()
        {
            StreamReader region_info = new StreamReader( Properties.Settings.Default.geoDir + @"\co99_d00a.dat" );
            StreamReader geo_info = new StreamReader( Properties.Settings.Default.geoDir + @"\co99_d00.dat" );

            int region_id = 0;

            while( !region_info.EndOfStream )
            {
                while( !region_info.EndOfStream && !Int32.TryParse( region_info.ReadLine(), out region_id ) ) { };
                if( region_id == 0 )
                {
                    continue;
                }

                char[] trim = new char[] {'\"', ' '};

                string state_id = region_info.ReadLine().TrimEnd( trim ).TrimStart( trim ).Trim();
                string county_id = region_info.ReadLine().TrimEnd( trim ).TrimStart( trim ).Trim();
                string county = region_info.ReadLine();
                // skip this
                region_info.ReadLine();
                string type = region_info.ReadLine().TrimEnd( trim ).TrimStart( trim ).Trim();
                // done reading

                string county_name = county.Substring( 2, county.Length - 3 );
                if( country.States.ContainsKey( state_id ) )
                {
                    State state = country.States[state_id];
                    
                    if( !state.Counties.ContainsKey( county_id ) )
                    {
                        state.Counties.Add( county_id, new County( county_name, type, county_id ));
                    }

                    County cc = state.Counties[county_id];


                    state.Counties[county_id].Sections.Add( region_id, new Section( region_id ) );


                    if( !read_geo_file( cc, region_id, geo_info ) )
                    {
                        break;
                    }

                }
            }

            region_info.Close();
            geo_info.Close();
            
        }

        private bool read_geo_file(GeoLibrary.County dma, int region_id, StreamReader geo_info )
        {
            if( geo_info.EndOfStream )
            {
                return false;
            }

            int cur_region = 0;

            bool valid = false;
            while( !valid )
            {
                // read in region from geo file
                string line = geo_info.ReadLine();

                string[] values = line.Split( space, StringSplitOptions.RemoveEmptyEntries );

                if( values.Length == 3 )
                {
                    valid = true;
                    cur_region = Int32.Parse( values[0] );
                    if( cur_region != region_id )
                    {
                        return false;
                    }
                }
                else
                {
                    valid = false;
                }

                if( !read_lat_long( dma, region_id, valid, geo_info ) )
                {
                    return false;
                }
            }

            return true;
        }

        private bool read_lat_long( GeoLibrary.County dma, int region_id, bool valid, StreamReader geo_info )
        {
            while( !geo_info.EndOfStream )
            {
                string line = geo_info.ReadLine();
               
                string[] values = line.Split( space, StringSplitOptions.RemoveEmptyEntries );

                if( values.Length == 2 )
                {
                    double longitude = Double.Parse( values[0] );
                    double latitude = Double.Parse( values[1] );

                    if( longitude > 0 )
                    {
                        longitude = -(360 - longitude);
                    }

                    if( valid )
                    {
                        dma.Sections[region_id].Coordinates.Add( new Coordinate( longitude, -latitude ) );
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        private Dictionary<string, string> stateCodeToName = new Dictionary<string,string>();
        private bool readStateCode()
        {
            FileInfo fi = new FileInfo( Properties.Settings.Default.geoDir + @"\StateCode.csv" );

            if(!fi.Exists )
            {
                return false;
            }

            StreamReader dmReader = new StreamReader( fi.FullName );
            stateCodeToName.Clear();

            while( !dmReader.EndOfStream )
            {
                // format state, county, dma
                string line = dmReader.ReadLine();

                string[] vals = line.Split( ',' );

                string code = vals[1];
                string name = vals[0];

                code = code.TrimStart( ' ' );
                code = code.TrimEnd( ' ' );

                name = name.TrimStart( ' ' );
                name = name.TrimEnd( ' ' );

                stateCodeToName.Add( code, name );
            }

            return true;
        }

        private void parseCountyDmaFile()
        {
            usa = new GeoRegion( "USA", GeoRegion.RegionType.Country );

            missingCounties.Items.Clear();
            missingCounties.Items.Add( "Unknown Counties" );
          
            FileInfo fi = new FileInfo( Properties.Settings.Default.geoDir + @"\DMA.csv" );

            if(!fi.Exists )
            {
                return;
            }

            StreamReader dmReader = new StreamReader( fi.FullName );

            // skip header
            dmReader.ReadLine();

            Dictionary<string, string> dmas = new Dictionary<string, string>();

            while( !dmReader.EndOfStream )
            {
                // format= type, state, county, dma
                // we actual use the type in a couple of cases
                string line = dmReader.ReadLine();

                string[] vals = line.Split( ',' );

                if( vals[0] == "" && vals[1] == "" && vals[2] == "")
                {
                    break;
                }

                string type = vals[0].Trim();
                string stateName = stateCodeToName[vals[1].TrimEnd( ' ' ).TrimStart( ' ' )];
                string dmaName = vals[3];
                string countyName = vals[2];
    
                State state = getCountryState( country, stateName );

                if (state == null)
                {
                    throw new Exception( "Unknown state: " + stateName );
                }

                // check if already added
                GeoRegion geoDma = usa.AddSubRegion( dmaName, GeoRegion.RegionType.DMA );
              
                // now find county in original state
                County county = getStateCounty( state, type, countyName );

                if( county == null )
                {
                   missingCounties.Items.Add(vals[0] + ", " + vals[1] + ", " + vals[2] + ", "  + vals[3]);
                }
                else
                {
                    GeoRegion geoCounty = geoDma.AddSubRegion( county.Name + ", " + state.Name, GeoRegion.RegionType.County );

                    geoCounty.ShapeTag = county;

                    // modify name for later accounting
                    county.Name = county.Name + "#";
                }
            }

            dmReader.Close();

            missingCounties.Items.Add( "Missing Counties" );

            foreach( State state in country.States.Values )
            {
                foreach( County county in state.Counties.Values )
                {
                    if( county.Name.Length > 1 && county.Name.Substring( county.Name.Length - 1 ) == "#" )
                    {
                        county.Name = county.Name.Substring( 0, county.Name.Length - 1 );
                    }
                    else
                    {
                        missingCounties.Items.Add( state.Name + ", " + county.Name + ", " +  county.Type );
                    }
                }
            }
        }

  
        private County getStateCounty(State state, string type, string dmaName)
        {
            foreach( County county in state.Counties.Values )
            {
                if( county.Name.ToLower().CompareTo( dmaName.ToLower() ) == 0 &&
                    county.Type.ToLower().CompareTo( type.ToLower() ) == 0 )
                {
                    return county;
                }
            }

            return null;
        }

        private State getCountryState( Country co, string stateName )
        {
            foreach( State state in co.States.Values )
            {
                string name1 = state.Name.ToLower();

                if( state.Name.ToLower().CompareTo( stateName.ToLower() ) == 0 )
                {
                    return state;
                }
            }

            return null;
        }

        private   List<GeoInfo>  parse_geo_file()
        {
            List<GeoInfo> cities = new List<GeoInfo>();

            FileInfo fi = new FileInfo( Properties.Settings.Default.geoDir + @"\GEO_DATA.geo" );

            if( !fi.Exists )
            {
                MessageBox.Show( "No GEO_DATA.geo file" );
                return cities;
            }

          

            StreamReader geo_file = new StreamReader( fi.FullName );
            List<string> header = new List<string>( geo_file.ReadLine().Split( '\t' ) );
            GeoInfo info;

            while( !geo_file.EndOfStream )
            {
                List<string> row = new List<string>( geo_file.ReadLine().Split( '\t' ) );
                info = new GeoInfo();

                string stateName = row[0];
                info.GeoID = int.Parse( row[1] );

                info.Name = row[2];

                double x = double.Parse( row[4] ); // long
                double y = -double.Parse( row[3] ); // lat

                // translate to picture coords
                if( x > 0 )
                {
                    x = -(360 - x);
                }

                info.Long = x;
                info.Lat = y;

                info.ResetBoundaries();

                cities.Add( info );
            }
            geo_file.Close();

            return cities;
        }

        private List<GeoInfo> missingCities = new List<GeoInfo>();
        private void processCities()
        {
            // parse the city lat long data
            missingCities.Clear();

            List<GeoInfo> cities = this.parse_geo_file();

            List<GeoRegion> counties = new List<GeoRegion>();

            foreach( GeoRegion dma in usa.SubRegions )
            {
                foreach( GeoRegion county in dma.SubRegions )
                {
                    if( !counties.Contains( county ) )
                    {
                        counties.Add( county );
                    }
                }
            }

            // draw special map where the colors index to counties
            DrawCountyColorMap( counties );

          
            // now find out which county each city is in
            foreach( GeoInfo geo in cities )
            {
                Point pp = mapPoint( usa.Box, new Coordinate( geo.Long, geo.Lat ) );

                bool found = false;
                int area = 0;
                int index = -1;
                while( !found && area < 10 )
                {
                    for( int dx = -area; !found && dx <= area; ++dx )
                    {
                        for( int dy = -area; !found && dy <= area; ++dy )
                        {
                            if( pp.X + dx < 0 || pp.X + dx > curWidth )
                            {
                                continue;
                            }

                            if( pp.Y + dy < 0 || pp.Y + dy > curHeight )
                            {
                                continue;
                            }

                            Color col = map.GetPixel( pp.X + dx, pp.Y + dy );
                            index = colorToIndex( col );
                            if( index >= 0 && index < counties.Count )
                            {
                                found = true;
                            }
                        }
                    }

                    ++area;
                }

             
                if( index < 0 || index >= counties.Count )
                {
                    missingCities.Add( geo );
                }
                else
                {
                    GeoRegion county = counties[index];

                    county.Add( geo );
                }
            }

            if( missingCities.Count > 0 )
            {
                MessageBox.Show( "missing " + missingCities.Count + " cities" );
            }
        }

        private TreeNode FindRegion( string regName )
        {
            TreeNode rval = null;

            foreach( TreeNode node in regionTree.Nodes )
            {
                rval = FindRegion( node, regName );

                if( rval != null )
                {
                    return rval;
                }
            }

            return rval;
        }

        private TreeNode FindRegion(TreeNode node, string regName )
        {
            if( node.Text == regName )
            {
                return node;
            }

            TreeNode rval = null;

            foreach( TreeNode sub in node.Nodes )
            {
                rval = FindRegion( sub, regName );

                if( rval != null )
                {
                    return rval;
                }
            }

            return rval;
        }

         private TreeNode createTree(GeoRegion region )
        {
            TreeNode node = new TreeNode( region.Name);
            node.Tag = region;


            if( region.SubRegions != null )
            {
                SortedDictionary<string, GeoRegion> subs = new SortedDictionary<string, GeoRegion>();

                foreach( GeoRegion sub in region.SubRegions )
                {
                    if( !subs.ContainsKey( sub.Name ) )
                    {
                        subs.Add( sub.Name, sub );
                    }
                    else
                    {
                    }
                }

                foreach( GeoRegion sub in subs.Values )
                {
                    node.Nodes.Add( createTree( sub ) );
                }
            }

            return node;
        }

         Dictionary<String, County> regionDmaMap = new Dictionary<string, County>();

        #endregion


         int scale = 45;
         int curHeight = 3048;
         int curWidth = 4096;
         int offset = 100;

        private Point mapPoint(BoundingBox box, Coordinate coord )
        {
            int xLoc = offset + (int)((coord.x - box.Min.x) * Math.Cos( Math.PI / 4 ) * scale);
            int yLoc = offset + (int)(((coord.y - box.Min.y) * scale));
            if( yLoc < 0 )
            {
                yLoc = 0;
            }

            if( xLoc < 0 )
            {
                xLoc = 0;
            }

            if( xLoc > curWidth )
            {
                xLoc = curWidth;
            }

            if( yLoc > curHeight )
            {
                yLoc = curHeight;
            }
                

            return new Point( xLoc, yLoc );
        }

        private Bitmap map = null;


        private void GetMap()
        {
            map = new Bitmap( curWidth, curHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

            BoundingBox box = usa.Box;

            GeoRegion selectedDMA = null;

            using( Graphics g = Graphics.FromImage( map ) )
            {
                g.Clear( Color.White );

                g.PageUnit = GraphicsUnit.Pixel;

                Pen pen = new Pen( Color.Black, 1 );

                Brush brsh = Brushes.Red;

                Brush grey = Brushes.LightGray;
                Brush black = Brushes.Black;

                Font font = new Font( FontFamily.GenericSansSerif, 4, GraphicsUnit.Pixel );

                Font dmafont = new Font( FontFamily.GenericSansSerif, 5, GraphicsUnit.Pixel );

                byte red = 0;
                byte green = 127;
                byte blue = 255;

                foreach( GeoRegion dma in usa.SubRegions )
                {
                    if( selectedRegion == dma )
                    {
                       //  box = dma.Box;
                        selectedDMA = dma;
                    }

                    brsh = new SolidBrush( Color.FromArgb( 255, red, green, blue ) );

                    red = (byte)((red + 1) % 255);

                    if( red > 250 )
                    {
                        red = 0;
                    }

                    blue = (byte)(blue - 1);
                    if( blue < 0 )
                    {
                        blue = 250;
                    }

                    green = (byte)((green + 31) % 250);

                    foreach( GeoRegion county in dma.SubRegions )
                    {
                        if( selectedRegion == county )
                        {
                            selectedDMA = county.Parent;
                        }
                        County shape = (County)county.ShapeTag;

                        PointF ave = new PointF( 0F, 0F );
                        int num = 0;

                        foreach( Section section in shape.Sections.Values )
                        {
                            if( section.Coordinates.Count > 1 )
                            {
                                Point[] points = new Point[section.Coordinates.Count];
                                int ii = 0;
                                foreach( Coordinate point in section.Coordinates )
                                {
                                    points[ii] = mapPoint( box, point );

                                    ave.X = ave.X + points[ii].X;
                                    ave.Y = ave.Y + points[ii].Y;
                                    num++;
                                    ii++;
                                }

                                if( selectedRegion != null &&
                                    county != selectedRegion &&
                                    dma != selectedRegion &&
                                    usa != selectedRegion )
                                {
                                    g.FillPolygon( grey, points );
                                }
                                else
                                {
                                    g.FillPolygon( brsh, points );
                                }

                                g.DrawPolygon( pen, points );

                            }
                        }


                        //if( county == selectedRegion ||
                        //    dma == selectedRegion )
                        //{
                        //    // draw cities
                        //    foreach( GeoRegion reg in county.SubRegions )
                        //    {
                        //        GeoInfo city = reg as GeoInfo;

                        //        Point pp = mapPoint( box, new Coordinate( city.Long, city.Lat ) );

                        //        g.FillEllipse( new SolidBrush( Color.FromArgb( 255, (byte)(255 - red), (byte)(255 - green), (byte)(255 - blue) ) ),
                        //             pp.X, pp.Y, 5, 5 );
                        //        // map.SetPixel( pp.X, pp.Y, Color.FromArgb( 255, (byte)(255 - red), (byte)(255 - green), (byte)(255 - blue) ) );
                        //    }
                        //}
                    }
                }


                if( selectedDMA != null )
                {
                    foreach( GeoRegion county in selectedDMA.SubRegions )
                    {
                        County shape = (County)county.ShapeTag;

                        PointF ave = new PointF( 0F, 0F );
                        int num = 0;

                        foreach( Section section in shape.Sections.Values )
                        {
                            if( section.Coordinates.Count > 1 )
                            {
                                Point[] points = new Point[section.Coordinates.Count];
                                int ii = 0;
                                foreach( Coordinate point in section.Coordinates )
                                {
                                    Point pp = mapPoint( box, point );

                                    ave.X = ave.X + pp.X;
                                    ave.Y = ave.Y + pp.Y;
                                    num++;
                                    ii++;
                                }
                            }
                        }

                        // put county name in
                        ave.X = ave.X / num;
                        ave.Y = ave.Y / num;

                        string nameToPrint = county.Name.Split( ',' )[0];

                        g.DrawString( nameToPrint, font, black, ave );

                    }
                }
                else
                {
                    foreach( GeoRegion dma in usa.SubRegions )
                    {
                        PointF ave = new PointF( 0F, 0F );
                        int num = 0;

                        foreach( GeoRegion county in dma.SubRegions )
                        {
                            County shape = (County)county.ShapeTag;
                            foreach( Section section in shape.Sections.Values )
                            {
                                if( section.Coordinates.Count > 1 )
                                {
                                    Point[] points = new Point[section.Coordinates.Count];

                                    foreach( Coordinate point in section.Coordinates )
                                    {
                                        Point pp = mapPoint( box, point );

                                        ave.X = ave.X + pp.X;
                                        ave.Y = ave.Y + pp.Y;
                                        num++;
                                    }
                                }
                            }
                        }

                        // put county name in
                        ave.X = ave.X / num;
                        ave.Y = ave.Y / num;



                        g.DrawString( dma.Name, dmafont, black, ave );
                    }
                }

                foreach( GeoInfo geo in missingCities )
                {
                    Point pp = mapPoint( box, new Coordinate( geo.Long, geo.Lat ) );

                    g.FillEllipse( new SolidBrush( Color.FromArgb( 255, 255, 0, 0)), pp.X, pp.Y, 5, 5 );
                }
            }
        }

        private Color indexToColor( int index )
        {
            byte r = (byte) (index % 256);

            int index10 = index / 256;

            byte g = (byte)(index10 % 256);

            int index100 = index10 / 256;

            byte b = (byte)(index100 % 256);

            return Color.FromArgb( 255, r, g, b );
        }

        private int colorToIndex( Color col )
        {
            return col.R + 256 * col.G + 256 * 256 * col.B;
        }

        private void DrawCountyColorMap(List<GeoRegion> counties)
        {
            map = new Bitmap( curWidth, curHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

            // compute new scale

            using( Graphics g = Graphics.FromImage( map ) )
            {
                g.PageUnit = GraphicsUnit.Pixel;

                g.Clear( Color.White );

                int index = 0;
                foreach( GeoRegion county in counties )
                {
                    Brush brsh = new SolidBrush(indexToColor( index ) );
                    
                    foreach( Section section in ((County)county.ShapeTag).Sections.Values )
                    {
                        if( section.Coordinates.Count > 1 )
                        {
                            Point[] points = new Point[section.Coordinates.Count];
                            int ii = 0;
                            foreach( Coordinate point in section.Coordinates )
                            {
                                points[ii] = mapPoint( usa.Box, point );
                                ii++;
                            }

                            g.FillPolygon( brsh, points );

                        }
                    }
                    index++;
                }

            }
        }


        Rectangle destRect;
        Rectangle srcRect;
        private void splitContainer1_Panel2_Paint( object sender, PaintEventArgs e )
        {
            destRect = e.ClipRectangle;
            using( Graphics g = e.Graphics )
            {
               // g.DrawImage(
                g.DrawImage( map, destRect, srcRect, g.PageUnit);
            }
        }

        GeoRegion selectedRegion = null;
        private void regionTree_AfterSelect( object sender, TreeViewEventArgs e )
        {
            selectedRegion = (GeoRegion) regionTree.SelectedNode.Tag;

            GetMap();

            //int id = mediaDb.GetRegion( selectedRegion );

            //if( id >= 0 )
            //{
            //    dbStatus.Text = "Found region id: " + id.ToString();
            //}
            //else
            //{
            //    dbStatus.Text = "Region not found";
            //}

            this.Refresh();
        }


        // zoom in
        private void button1_Click( object sender, EventArgs e )
        {
            float scale = 0.75F;

            if(srcRect.Size.Width < 128 )
            {
                return;
            }

            srcRect.X += (int) ((1 - scale) * srcRect.Size.Width / 2);
            srcRect.Y += (int)((1 - scale) * srcRect.Size.Height / 2);

            srcRect.Size = new Size( (int)(scale * srcRect.Size.Width), (int) (scale * srcRect.Size.Height) );

            this.Refresh();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            float scale = 1.25F;

            srcRect.X += (int)((1 - scale) * srcRect.Size.Width / 2);
            srcRect.Y += (int)((1 - scale) * srcRect.Size.Height / 2);
            srcRect.Size = new Size( (int)(scale * srcRect.Size.Width), (int)(scale * srcRect.Size.Height) );

            if( srcRect.X < 0 || srcRect.Y < 0 || srcRect.Size.Width > curWidth || srcRect.Size.Height > curHeight)
            {
                srcRect.X = 0;
                srcRect.Y = 0;

                srcRect.Size = new Size( curWidth, curHeight );
            }

            this.Refresh();
        }

        private void panLeft_Click( object sender, EventArgs e )
        {
            srcRect.X -= srcRect.Width / 4;

            if( srcRect.X < 0 )
            {
                srcRect.X = 0;
            }

            this.Refresh();
        }

        private void panRight_Click( object sender, EventArgs e )
        {
            srcRect.X += srcRect.Width / 4;

            if( srcRect.X > curWidth - srcRect.Width )
            {
                srcRect.X = curWidth - srcRect.Width;
            }

            this.Refresh();
        }

        private void panUp_Click( object sender, EventArgs e )
        {
            srcRect.Y -= srcRect.Height / 4;

            if( srcRect.Y < 0 )
            {
                srcRect.Y = 0;
            }

            this.Refresh();
        }

        private void panDown_Click( object sender, EventArgs e )
        {
            srcRect.Y += srcRect.Height / 4;

            if( srcRect.Y > curHeight - srcRect.Height )
            {
                srcRect.Y = curHeight - srcRect.Height;
            }

            this.Refresh();
        }

        private void geoFolderlabel_Click( object sender, EventArgs e )
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;


            browse.Description = "Select the folder that has the region information";
            if( browse.ShowDialog() != DialogResult.OK )
            {
                return;
            }

            Properties.Settings.Default.geoDir = browse.SelectedPath;

            Properties.Settings.Default.Save();

            this.geoFolderlabel.Text = Properties.Settings.Default.geoDir;
        }

        private void webFolderLabel_Click( object sender, EventArgs e )
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;
            browse.Description = "Select the web folder to place save to";

            if( browse.ShowDialog() != DialogResult.OK )
            {
                return;
            }

            Properties.Settings.Default.webDir = browse.SelectedPath;

            Properties.Settings.Default.Save();

            webFolderLabel.Text = Properties.Settings.Default.webDir;
        }

        private DpMediaDb mediaDb = new DpMediaDb( Properties.Settings.Default.mediaConnectionStr );
        private void openDb_Click( object sender, EventArgs e )
        {
            mediaDb.RefreshRegionInfo();

            usa = mediaDb.ComputeRegionTree();

            // build tree
            this.regionTree.Nodes.Clear();

            this.regionTree.Nodes.Add( createTree( usa ) );

            selectedRegion = usa;
        }

        private bool CheckNode( Media.regionRow parentRow, TreeNode node )
        {
            Media.regionRow regRow = mediaDb.GetRegion( parentRow, (GeoRegion)node.Tag );

            if( regRow == null)
            {
                dbStatus.Text = "Could not find in db: " + node.Text;
                node.BackColor = Color.Yellow;
                node.Parent.BackColor = Color.LightGray;
                return false;
            }

            foreach( TreeNode sub in node.Nodes )
            {
                CheckNode( regRow, sub );
            }

            return true;
        }
     

        private void updateDb_Click( object sender, EventArgs e )
        {
            mediaDb.RefreshRegionInfo();

            mediaDb.AddRegion( null, usa );

            mediaDb.Update();
        }

        private void DbCompare_Click( object sender, EventArgs e )
        {
            mediaDb.RefreshRegionInfo();

            GeoRegion tmp = mediaDb.ComputeRegionTree();

            if(FindRegion( tmp.Name ) == null)
            {
                dbStatus.Text = "Cannot find top level node: " + tmp.Name;
                return;
            }

            foreach( GeoRegion reg in tmp.SubRegions )
            {
                TreeNode node = FindRegion( reg.Name );

                if( node == null )
                {
                    this.missingCounties.Items.Add( reg.Name );
                    node.ForeColor = Color.Red;
                }
                else
                {
                    node.ForeColor = Color.Blue;
                }
            }

            // Now check for items in tree NOT in db
            foreach( TreeNode aNode in regionTree.Nodes )
            {
                CheckNode( null, aNode );
            }

            dbStatus.Text = "Database succesfully open";
        }

        //private void findDma_Click( object sender, EventArgs e )
        //{
        //    //GeoRegion reg = topGeo.GetSubRegion( textFInd.Text );

        //    //if( reg != null )
        //    //{
        //    //    TreeNode[] nodes = regionTree.Nodes.Find( reg.Name, true );

        //    //    if( nodes.Length > 0 )
        //    //    {
                  
        //    //        regionTree.SelectedNode = nodes[0];

        //    //        selectedRegion = (GeoRegion)regionTree.SelectedNode.Tag;
        //    //    }
        //    //}
        //}
    }
}
