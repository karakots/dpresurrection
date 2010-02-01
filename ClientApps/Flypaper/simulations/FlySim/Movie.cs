using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using HouseholdLibrary;

using GeoLibrary;

using MediaLibrary;

namespace FlySim
{
    public class Movie
    {
        public static float CosLat = (float) Math.Cos( Math.PI / 4 );


        
        public static float Scale = 7.5F;

        public static float Nudge = 0.5F * Scale;

        public static int PicWidth = 640;
        public static int PicHeight = 480;


        public static float PieSize
        {
            get
            {
                return 5F * Scale;
            }
        }

        public static float PieX
        {
            get
            {
                return 10F * Scale;
            }
        }

        public static float PieY
        {
            get
            {
                return 22F * Scale;
            }
        }

        public static float DayX
        {
            get
            {
                return Scale;
            }
        }

        public static float DayY
        {
            get
            {
                return 20 * Scale;
            }
        }

        public static Font LargeFont = new Font( FontFamily.GenericSansSerif, 10 );
        public static Font SmallFont = new Font( FontFamily.GenericSansSerif, 8 );

        public static Dictionary<Agent, Point> agentPos = new Dictionary<Agent, Point>();

        public static void DrawAgent( Bitmap pic, Point pos, Color agentColor )
        {

            SetPixel( pic, pos.X, pos.Y, agentColor );

            SetPixel( pic, 1 + pos.X, -1 + pos.Y, agentColor );
            //SetPixel( pic, 1 + pos.x, pos.y, agentColor );
            SetPixel( pic, 1 + pos.X, 1 + pos.Y, agentColor );

            //SetPixel( pic, pos.x, 1 + pos.y, agentColor );

            SetPixel( pic, -1 + pos.X, 1 + pos.Y, agentColor );
            //SetPixel( pic, -1 + pos.x, pos.y, agentColor );
            SetPixel( pic, -1 + pos.X, -1 + pos.Y, agentColor );

            //SetPixel( pic, pos.x, -1 + pos.y, agentColor );
        }

        public static bool DrawColor( Bitmap pic, int x, int y, Color agentColor )
        {
            Color pixColor = pic.GetPixel( x, y );


            // no color
            if( pixColor.G == 255 & pixColor.R == 255 && pixColor.B == 255 )
            {
                return true;
            }

            if( pixColor.A < agentColor.A )
            {
                return true;
            }

            return false;
        }

        private static void SetPixel( Bitmap pic, int x, int y, Color agentColor )
        {
            if( !DrawColor(pic, x, y, agentColor ) )
            {
                return;
            }

            pic.SetPixel( x, y, agentColor );
        }

        [Serializable]
        public struct Pixel
        {
            public int x;
            public int y;
        }


        // colors

        static public Color AwareColor = Color.FromArgb(100, Color.LightBlue.R, Color.LightBlue.G,Color.LightBlue.B);
        static public Color LowColor = Color.FromArgb( 150, Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B );
        static public Color MedColor = Color.FromArgb( 200, Color.Green.R, Color.Green.G, Color.Green.B );
        static public Color HighColor = Color.FromArgb( 255, Color.Red.R, Color.Red.G, Color.Red.B );

        static public Brush AwareBrush = new SolidBrush( AwareColor );
        static public Brush LowBrush = new SolidBrush( LowColor );
        static public Brush MedBrush = new SolidBrush( MedColor );
        static public Brush HighBrush = new SolidBrush( HighColor );

        static public float DotSize = 2.0F;

        // probably move this to a seprate class when we know what we need

        int frame = 0;
        public void InitMovie()
        {
            frame = 0;
        }

        public void EndMovie()
        {
        }

        public void CreateMovieFile( List<AgentData> datum )
        {

            // add a frame

            Bitmap tmp = new Bitmap( (Bitmap)Country.MapObj );

            int numAware = 0;
            int persuasion1 = 0;
            int persuasion2 = 0;
            int persuasion3plus = 0;

            int total = datum.Count;
            Color agentColor = Color.White;
            Brush brsh = Brushes.Red;

            using( Graphics g = Graphics.FromImage( tmp ) )
            {

                foreach( AgentData data in datum )
                {
                    if( data.active_media.Count > 0 )
                    {
                        int num_impressions = data.NumImpressions();
                        if (num_impressions >= 3)
                        {
                            brsh = HighBrush;
                            agentColor = HighColor;
                            persuasion3plus++;
                        }
                        else if (num_impressions >= 2)
                        {
                            brsh = MedBrush;
                            agentColor = MedColor;
                            persuasion2++;
                        }
                        else if (num_impressions >= 1)
                        {
                            brsh = LowBrush;
                            agentColor = LowColor;
                            persuasion1++;
                        }
                        else
                        {
                            brsh = AwareBrush;
                            agentColor = AwareColor;
                            numAware++;
                        }

                        try
                        {
                            Point pos = agentPos[data.agent];

                            //if( DrawColor( tmp, pos.x, pos.y, agentColor ) )
                            //{
                            //    g.FillRectangle( brsh, pos.x, pos.y, 2, 2 );
                            //}
                            DrawAgent( tmp, pos, agentColor );
                        }
                        catch
                        {
                        }
                    }
                }

                // compute angles

                // compute angles

                if( total == 0 )
                {
                    total = 1;
                }

                float aware = 360F * numAware / total;
                float pers3 = 360F * persuasion3plus / total;
                float pers2 = 360F * persuasion2 / total;
                float pers1 = 360F * persuasion1 / total;


                brsh = Brushes.Black;

                g.DrawString( "Day " + frame.ToString(), Movie.LargeFont, brsh, Movie.DayX, Movie.DayY );

                // aware
                g.FillPie( Movie.AwareBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, 0F, aware );

                // persuasion > 1
                g.FillPie( Movie.LowBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware, pers1 );

                // persuasion > 2
                g.FillPie( Movie.MedBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware + pers1, pers2 );

                // persuasion > 2
                g.FillPie( Movie.HighBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware + pers1 + pers2, pers3 );

            }


            try
            {
                // remember me
                tmp.Save( @"./agent_" + frame.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg );
            }
            catch( Exception )
            {
            }
            frame++;

        }
    }


          
    #region visualization code

        //private class HHView
        //{
        //    public double radius = 1;
        //    public Color color = Color.Violet;
        //}

        //private void initialize_visualization()
        //{
        //    if( hhinput_dir == null )
        //    {
        //        return;
        //    }

        //    FileInfo fi = new FileInfo( this.web_dir.FullName + @"\shape.geo" );

        //    if(!fi.Exists )
        //    {
        //        MessageBox.Show( "No shape.geo file to visualize" );
        //        return;
        //    }

        //    FileStream file = new FileStream( this.web_dir.FullName + @"\shape.geo", FileMode.Open, FileAccess.Read );
        //    USA = (Country)formatter.Deserialize(file);
        //    file.Close();

        //    // we map the counties in the USA to the appropriate geo items

        //    if( GeoRegion.TopGeo != null )
        //    {
        //        foreach( State state in USA.States.Values )
        //        {
        //            foreach( County county in state.Counties.Values )
        //            {
        //                GeoRegion geo = GeoRegion.TopGeo.GetSubRegion( county.Name + ", " + state.Name );

        //                if( geo == null )
        //                {
        //                    updateStatus( "Could not find county: " + county.Name + ", " + state.Name );
        //                }
        //                else
        //                {
        //                    geo.ShapeTag = county;
        //                }
        //            }
        //        }

        //        GeoRegion.TopGeo.ResetBoundaries();
        //    }


        //    //USA.Box.Max.x -= USA.Box.Min.x;
        //    //USA.Box.Max.y -= USA.Box.Min.y;

        //    //Shape.maxX = USA.Box.Max.x;
        //    //Shape.maxY = USA.Box.Max.y;
        //    //Shape.minX = USA.Box.Min.x;
        //    //Shape.minY = USA.Box.Min.y;


        //    //foreach (State state in USA.States.Values)
        //    //{
        //    //    foreach( County dma in state.Counties.Values )
        //    //    {
        //    //        foreach( Section section in dma.Sections.Values )
        //    //        {
        //    //            foreach( Coordinate point in section.Coordinates )
        //    //            {
        //    //                point.Offset( -Country.minX, -Country.minY );
        //    //            }
        //    //        }
        //    //    }
        //    //}
          

        //}

        //private Point mapPoint(Coordinate coord )
        //{
        //    int xLoc = (int) (Movie.Nudge + (int)((coord.x - GeoRegion.TopGeo.Box.Min.x) * Math.Cos( Math.PI / 4 ) * Movie.Scale));
        //    int yLoc = (int)(Movie.Nudge + (int)(((coord.y - GeoRegion.TopGeo.Box.Min.y) * Movie.Scale)));

        //    if( yLoc < 0 )
        //    {
        //        yLoc = 0;
        //    }

        //    if( xLoc < 0 )
        //    {
        //        xLoc = 0;
        //    }

        //    if( xLoc > Movie.PicWidth )
        //    {
        //        xLoc = Movie.PicWidth;
        //    }

        //    if( yLoc > Movie.PicHeight )
        //    {
        //        yLoc = Movie.PicHeight;
        //    }


        //    return new Point( xLoc, yLoc );
        //}

        //private Bitmap drawUS()
        //{

        //    if( Country.MapObj != null )
        //    {
        //        return (Bitmap)Country.MapObj;
        //    }

        //    Bitmap map = new Bitmap( Movie.PicWidth, Movie.PicHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

               
        //    if( USA == null )
        //    {
        //        return map;
        //    }


        //    using( Graphics g = Graphics.FromImage( map ) )
        //    {

        //        g.Clear( Color.White );
             
        //        g.PageUnit = GraphicsUnit.Pixel;

        //        Pen pen = new Pen( Color.FromArgb( 100, Color.Black.R, Color.Black.G, Color.Black.B ), 1 );

        //        foreach( State state in USA.States.Values )
        //        {
        //            foreach( County dma in state.Counties.Values )
        //            {
        //                foreach( Section section in dma.Sections.Values )
        //                {
        //                    if( section.Coordinates.Count > 1 )
        //                    {
        //                        PointF[] points = new PointF[section.Coordinates.Count];
        //                        int ii = 0;
        //                        foreach( Coordinate point in section.Coordinates )
        //                        {
        //                            points[ii] = mapPoint( point );
        //                            ii++;
        //                        }

        //                        g.DrawPolygon( pen, points );

        //                    }
        //                }
        //            }
        //        }


        //        float width = 10F;

        //        Brush txtBrsh = Brushes.Black;

        //        g.DrawString( "AdPlanit presented by DecisionPower", Movie.LargeFont, txtBrsh, 0.25F * Movie.PicWidth, 0.95F * Movie.PicHeight );

        //        float seperation = 5F;
        //        float xPos = Movie.DayX;
        //        float yPos = Movie.DayY + Movie.LargeFont.Size + 5 * seperation;


        //        g.FillRectangle( Movie.AwareBrush, xPos, yPos, width, width );
        //        g.DrawString( "Aware", Movie.SmallFont, txtBrsh, xPos + width + seperation, yPos );


        //        yPos += width + seperation;

        //        g.FillRectangle( Movie.LowBrush, xPos, yPos, width, width );
        //        g.DrawString( "Low Persuasion", Movie.SmallFont, txtBrsh, xPos + width + seperation, yPos );

        //        yPos += width + seperation;

        //        g.FillRectangle( Movie.MedBrush, xPos, yPos, width, width );
        //        g.DrawString( "Medium Persuasion", Movie.SmallFont, txtBrsh, xPos + width + seperation, yPos );

        //        yPos += width + seperation;

        //        g.FillRectangle( Movie.HighBrush, xPos, yPos, width, width );
        //        g.DrawString( "High Persuasion", Movie.SmallFont, txtBrsh, xPos + width + seperation, yPos );
              
  
        //    }

        //    Country.MapObj = map;
          
        //    return  map;
        //}


        //private Bitmap usaAgents = null;

        //private void positionAgents()
        //{
        //    Coordinate coord = new Coordinate();
        //    Movie.agentPos.Clear();

        //    foreach( Agent agent in agents )
        //    {
        //        // get lat long
        //        //GeoInfo city = GeoRegion.TopGeo.FindRegion(agent.House.GeoID.ToString());
        //        GeoInfo city = new GeoInfo();

        //        if( city == null )
        //        {
        //            updateStatus( "Error positioning agents" );
        //            break;
        //        }

        //        GeoRegion county = city.Parent;

        //        if( county == null )
        //        {
        //            updateStatus( "Geo ID has no parent" );
        //            break;
        //        }


        //        // pick a point randomly in county

        //        coord.x = county.Box.Min.x + (county.Box.Max.x - county.Box.Min.x) * rand.NextDouble();
        //        coord.y = county.Box.Min.y + (county.Box.Max.y - county.Box.Min.y) * rand.NextDouble();

          
        //        Point pos = mapPoint( coord );
        //        Movie.agentPos.Add( agent, pos );
        //    }
        //}

        //private void drawAgents()
        //{
        //    const int frame = 42;
        //    const double tstPers1 = 0.30;
        //    const double tstPers2 = 0.20;
        //    const double tstPers3 = 0.10;

        //    Bitmap usa = drawUS();

        //    usaAgents = new Bitmap( usa );

        //    Color agentColor = Color.Blue;

        //    int numAware = 0;
        //    int persuasion1 = 0;
        //    int persuasion2 = 0;
        //    int persuasion3plus = 0;

        //    int total = agents.Count;

        //    // start a new day
        //    //double radius = 0;
        //    //double deltaX = 0;
        //    //double deltaY = 0;

        //    Brush brsh = Brushes.Transparent;

        //    brsh = Brushes.Black;

        //    using( Graphics g = Graphics.FromImage( usaAgents ) )
        //    {
        //        //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
        //        //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

        //        foreach( Agent agent in agents )
        //        {
        //            double tst = rand.NextDouble();

        //            if( tst < tstPers3 )
        //            {
        //                brsh = Movie.HighBrush;
        //                agentColor = Movie.HighColor;
        //                persuasion3plus++;
        //            }
        //            else if( tst < tstPers3 + tstPers2 )
        //            {
        //                brsh = Movie.MedBrush;
        //                agentColor = Movie.MedColor;
        //                persuasion2++;
        //            }
        //            else if( tst < tstPers3 + tstPers2 + tstPers1 )
        //            {
        //                brsh = Movie.LowBrush;
        //                agentColor = Movie.LowColor;
        //                persuasion1++;
        //            }
        //            else
        //            {
        //                brsh = Movie.AwareBrush;
        //                agentColor = Movie.AwareColor;
        //                numAware++;
        //            }

        //            try
        //            {
        //                Point pos = Movie.agentPos[agent];

        //                Movie.DrawAgent( usaAgents, pos, agentColor );
        //            }
        //            catch( Exception e )
        //            {
        //                string mess = e.Message;
        //            }
                  
        //        }

        //        // compute angles

        //        // compute angles

        //        if( total == 0 )
        //        {
        //            total = 1;
        //        }

        //        float aware = 360F * numAware / total;
        //        float pers3 = 360F * persuasion3plus / total;
        //        float pers2 = 360F * persuasion2 / total;
        //        float pers1 = 360F * persuasion1 / total;




        //        brsh = Brushes.Black;

        //        g.DrawString( "Day " + frame.ToString(), Movie.LargeFont, brsh, Movie.DayX, Movie.DayY );

        //        // aware
        //        g.FillPie( Movie.AwareBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, 0F, aware );

        //        // persuasion > 1
        //        g.FillPie( Movie.LowBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware, pers1 );

        //        // persuasion > 2
        //        g.FillPie( Movie.MedBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware + pers1, pers2 );

        //        // persuasion > 2
        //        g.FillPie( Movie.HighBrush, Movie.PieX, Movie.PieY, Movie.PieSize, Movie.PieSize, aware + pers1 + pers2, pers3 );

        //    }
        //}

        //private void Map_Paint(object sender, PaintEventArgs e)
        //{
        //    if (USA == null)
        //    {
        //        return;
        //    }

        //    Rectangle destRect = e.ClipRectangle;

        //    using( Graphics g = e.Graphics )
        //    {//760, 519
        //        if( usaAgents == null )
        //        {
        //            Bitmap usa = drawUS();
        //            g.DrawImage( usa, destRect );
        //        }
        //        else
        //        {
        //            g.DrawImage( usaAgents, destRect );
        //        }
        //    }
        //}

        //private void Redraw_Map(object sender, EventArgs e)
        //{
        //    MapPanel.Refresh();
        //}

        //#region old and in the way

        //// Color[] filterColors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Goldenrod, Color.Violet };

      
        ////private void build_agent_pixels()
        ////{ 
           
        ////    if( USA == null )
        ////    {
        ////        return;
        ////    }


        ////    // update filterList

        ////    filterList.Items.Clear();

        ////    house_pixels.Clear();
        ////    int index = 0;
        ////    foreach( Demographic filter in filters )
        ////    {
        ////        // set color for this demographic

        ////        if( index == filterColors.Length )
        ////        {
        ////            index = 0;
        ////        }

        ////        Color currentColor = filterColors[index];

        ////        string item = "(" + currentColor.Name + ") " + filter.ToString();

        ////        filterList.Items.Add( item );

        ////        foreach( Agent agent in agents )
        ////        {

        ////            if( !filter.Match( agent.House ) )
        ////            {
        ////                continue;
        ////            }

        ////            float x = (float)GeoRegion.TopGeo[agent.House.GeoID].Long;
        ////            float y = (float)-GeoRegion.TopGeo[agent.House.GeoID].Lat;

        ////            if( x > 0 )
        ////            {
        ////                x = -(360 - x);
        ////            }

        ////            x -= (float)Country.minX;
        ////            y -= (float)Country.minY;
        ////            if( y > Country.maxY || y < 0 )
        ////            {
        ////                continue;
        ////            }
        ////            if( x > Country.maxX || x < 0 )
        ////            {
        ////                continue;
        ////            }

        ////            PointF point = new PointF( x, y );

        ////            if( !house_pixels.ContainsKey( point ) )
        ////            {
        ////                HHView hhview = new HHView();
        ////                hhview.radius = 1;
        ////                hhview.color = currentColor;

        ////                house_pixels.Add( point, hhview);
        ////            }
        ////            else
        ////            {
        ////                house_pixels[point].radius = house_pixels[point].radius + 1;
        ////            }

        ////            if( house_pixels[point].radius > max_size )
        ////            {
        ////                max_size = house_pixels[point].radius;
        ////            }
        ////        }

        ////        index++;
        ////    }

        ////    drawAgents();
        ////}
        //#endregion

        #endregion
}
