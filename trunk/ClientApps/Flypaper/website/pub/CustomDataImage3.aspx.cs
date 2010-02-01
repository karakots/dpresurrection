using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;

public partial class _DynImage3 : System.Web.UI.Page
{
    protected const int width = 350;
    protected const int height = 150;

    protected const int awarenessX0 = 95;
    protected const int awarenessY0 = 0;
    protected const int awarenessHeight = 149;
    protected const int awarenessWidth = 40;

    protected const int persuasionX0 = 220;
    protected const int persuasionY0 = 15;
    protected const int persuasionBarHeight = 20;
    protected const int maxPersuasionBarWidth = 130;

    protected TextObj[] textItem = new TextObj[] { 
        new TextObj( "Unaware", 10, 20 ), 
        new TextObj( "Aware Only", 5, 75 ),
        new TextObj( "Aware & Tried", 0, 130 ),

        new TextObj( "0", 200, 20 ),
        new TextObj( "0 to 1", 180, 40 ),
        new TextObj( "1 to 2", 180, 60 ),
        new TextObj( "2 to 3", 180, 80 ),
        new TextObj( "3 to 4", 180, 100 ),
        new TextObj( "4+", 200, 120 ),
    };

    protected Font textItemFont = new Font( "Arial", 9f );
    protected Brush textItemBrush = new SolidBrush( Color.Black );
    protected Pen outlinePen = new Pen( new SolidBrush( Color.Black ) );

    protected Color unawareColor = Color.White;
    protected Color awareColor = Color.LightGray;
    protected Color triedColor = Color.LightGreen;

    protected Color[] pColor = new Color[]{
        Color.FromArgb( 255, 245, 255 ),
        Color.FromArgb( 255, 215, 255 ),
        Color.FromArgb( 255, 175, 255 ),
        Color.FromArgb( 255, 145, 255 ),
        Color.FromArgb( 255, 105, 255 ),
        Color.FromArgb( 255, 85, 255 ),
    };

    protected Color[] pxColor = new Color[]{
        Color.FromArgb( 255, 245, 245 ),
        Color.FromArgb( 255, 215, 215 ),
        Color.FromArgb( 255, 175, 175 ),
        Color.FromArgb( 255, 145, 145 ),
        Color.FromArgb( 255, 105, 105 ),
        Color.FromArgb( 255, 85, 85 ),
    };

    void Page_Load( Object sender, EventArgs e ) {
        // Since we are outputting a Jpeg, set the ContentType appropriately
        Response.ContentType = "image/jpeg";

        // awareness
        double unaware = GetRequestDouble( "au" );
        double awareOnly = GetRequestDouble( "ao" );
        double awareAndTried = GetRequestDouble( "at" );

        // persuasion
        double[] p = new double[ 6 ];
        double[] px = new double[ 6 ];

        p[ 0 ] = GetRequestDouble( "p0" );
        p[ 1 ] = GetRequestDouble( "p1" );
        p[ 2 ] = GetRequestDouble( "p2" );
        p[ 3 ] = GetRequestDouble( "p3" );
        p[ 4 ] = GetRequestDouble( "p4" );
        p[ 5 ] = GetRequestDouble( "p5" );

        px[ 0 ] = 0;
        px[ 1 ] = GetRequestDouble( "px1" );
        px[ 2 ] = GetRequestDouble( "px2" );
        px[ 3 ] = GetRequestDouble( "px3" );
        px[ 4 ] = GetRequestDouble( "px4" );
        px[ 5 ] = GetRequestDouble( "px5" );


        Bitmap objBitmap = new Bitmap( width, height );
        Graphics objGraphics = Graphics.FromImage( objBitmap );
        objGraphics.FillRectangle( new SolidBrush( Color.White ), 0, 0, width, height );
       //objGraphics.DrawRectangle( outlinePen, 0, 0, width, height );

        DrawLabels( objGraphics );

        DrawAwarenessRects( objGraphics, unaware, awareOnly, awareAndTried );

        DrawPersuasionRects( objGraphics, p, px );

        // Save the image to the OutputStream
        objBitmap.Save( Response.OutputStream, ImageFormat.Jpeg );

        // clean up...
        objGraphics.Dispose();
        objBitmap.Dispose();
    }

    protected double GetRequestDouble( string requestArg ) {
        double v1 = 0;
        string vs1 = Request[ requestArg ];
        if( vs1 != null ) {
            try {
                v1 = double.Parse( vs1 );
            }
            catch( Exception ) {
            }
        }
        return v1;
    }

    protected void DrawLabels( Graphics g ) {
        foreach( TextObj lbl in textItem ){
            g.DrawString( lbl.Text, textItemFont, textItemBrush, lbl.Location );
        }
    }

    protected void DrawAwarenessRects( Graphics g, double unaware, double aware, double tried ) {
        //Rectangle r0 = new Rectangle( awarenessX0, awarenessY0, awarenessWidth - 1, awarenessHeight - 1 );
        //g.DrawRectangle( outlinePen, r0 );

        double sum = unaware + aware + tried;
        if( sum == 0 ) {
            sum = 1;
        }

        int h1 = (int)Math.Round( awarenessHeight * unaware / sum );
        int h2 = (int)Math.Round( awarenessHeight * aware / sum );
        int h3 =(int)Math.Round(  awarenessHeight * tried / sum );

        Rectangle r1 = new Rectangle( awarenessX0, awarenessY0, awarenessWidth - 1, h1 );
        Rectangle r2 = new Rectangle( awarenessX0, awarenessY0 + h1, awarenessWidth - 1, h2 );
        Rectangle r3 = new Rectangle( awarenessX0, awarenessY0 + h1 + h2, awarenessWidth - 1, h3 );

        Brush b1 = new SolidBrush( unawareColor );
        Brush b2 = new SolidBrush( awareColor );
        Brush b3 = new SolidBrush( triedColor );

        g.FillRectangle( b1, r1 );
        g.FillRectangle( b2, r2 );
        g.FillRectangle( b3, r3 );
        g.DrawRectangle( outlinePen, r1 );
        g.DrawRectangle( outlinePen, r2 );
        g.DrawRectangle( outlinePen, r3 );
    }

    protected void DrawPersuasionRects( Graphics g, double[] p, double[] px ) {
        double maxVal = 50;

        int x0 = persuasionX0;
        int y0 = persuasionY0;
        int w = maxPersuasionBarWidth - 1;
        int h = persuasionBarHeight - 1;
        for( int i = 0; i < p.Length; i++ ) {
            //Rectangle r = new Rectangle( x0, y0, w, h );
            //g.DrawRectangle( outlinePen, r );

            int w1 = (int)Math.Round( w * p[ i ] / maxVal );
            int w2 = (int)Math.Round( w * px[ i ] / maxVal );

            Rectangle r1 = new Rectangle( x0, y0, w1, h );
            Rectangle r2 = new Rectangle( x0 + w1, y0, w2, h );

            Brush b1 = new SolidBrush( pColor[ i ] );
            Brush b2 = new SolidBrush( pxColor[ i ] );

            g.FillRectangle( b1, r1 );
            g.FillRectangle( b2, r2 );
            g.DrawRectangle( outlinePen, r1 );
            g.DrawRectangle( outlinePen, r2 );

            y0 += persuasionBarHeight;
        }
    }

    protected class TextObj
    {
        public string Text;
        public Point Location;

        public TextObj( string text, int x0, int y0 ) {
            this.Text = text;
            this.Location = new Point( x0, y0 );
        }
    }

    //private void xxxx(){
 ////       double v1 = 0;
 ////       string vs1 = Request[ "v1" ];
 ////       if( vs1 != null ) {
 ////           try {
 ////               v1 = double.Parse( vs1 );
 ////           }
 ////           catch( Exception ) {
 ////           }
 ////       }

 ////       double v2 = 0;
 ////       string vs2 = Request[ "v2" ];
 ////       if( vs2 != null ) {
 ////           try {
 ////               v2 = double.Parse( vs2 );
 ////           }
 ////           catch( Exception ) {
 ////           }
 ////       }

 ////       double v3 = 0;
 ////       string vs3 = Request[ "v3" ];
 ////       if( vs3 != null ) {
 ////           try {
 ////               v3 = double.Parse( vs3 );
 ////           }
 ////           catch( Exception ) {
 ////           }
 ////       }

 ////       int h1 = (int)(v1 * 70 / 9000);
 ////       int h2 = (int)(v2 * 70 / 9000);
 ////       int h3 = (int)(v3 * 70 / 9000);

 ////       int x0 = 10;
 ////       int y0 = 85;

 ////       int wid = 27;

 ////       Rectangle r1 = new Rectangle( x0, y0 - h1, wid, h1 );
 ////       x0 += wid;
 ////       Rectangle r2 = new Rectangle( x0, y0 - h2, wid, h2 );
 ////       x0 += wid;
 ////       Rectangle r3 = new Rectangle( x0, y0 - h3, wid, h3 );

 ////       // Create a Bitmap instance that's 468x60, and a Graphics instance
 ////       const int width = 100, height = 100;

 ////       Bitmap objBitmap = new Bitmap( width, height );
 ////       Graphics objGraphics = Graphics.FromImage( objBitmap );

 ////       // Create a black background for the border
 //////       objGraphics.FillRectangle( new SolidBrush( Color.Black ), 0, 0, width, height );

 ////       // Create a LightBlue background
 ////       objGraphics.FillRectangle( new SolidBrush( Color.FromArgb( 255, 255, 255, 255 ) ), 5, 5,
 ////           width - 10, height - 10 );

 ////       // Create the simple bar graphs
 ////       objGraphics.FillRectangle( new SolidBrush( Color.Red ), r1 );
 ////       objGraphics.FillRectangle( new SolidBrush( Color.Green ), r2 );
 ////       objGraphics.FillRectangle( new SolidBrush( Color.Orange ), r3 );

 ////       string s = Request[ "t" ];
 ////       if( s != null ) {
 ////           s = "t = " + s;

 ////           Font font = new Font( "Verdana", 8, FontStyle.Regular );
 ////           objGraphics.DrawString( s, font, new SolidBrush( Color.Black ), new Rectangle( 4, 4, width, height ), new StringFormat() );
 ////       }

 ////       // Save the image to the OutputStream
 ////       objBitmap.Save( Response.OutputStream, ImageFormat.Jpeg );

 ////       // clean up...
 ////       objGraphics.Dispose();
 ////       objBitmap.Dispose();
    //}
}
