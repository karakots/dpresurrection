using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Drawing.Imaging;

public partial class AgentView_AgentIcon : System.Web.UI.Page
{
   int w  = 50;
   int red = 0;
   bool active = false;

    protected void Page_Load( object sender, EventArgs e )
    {
        string size = this.Request.Params["size"];

        if( size != null )
        {
            w = Int32.Parse( size );
        }

        string color = this.Request.Params["color"];

        if( color != null )
        {
            red = Int32.Parse( color );
        }
             
        string activeStr = this.Request.Params["active"];

        if( activeStr != null && activeStr.ToLower() == "true")
        {
            active = true;
        }

        Response.ContentType = "image/jpeg";

        System.Drawing.Image img = drawIcon();

        img.Save( Response.OutputStream, ImageFormat.Jpeg );

        img.Dispose();

    }

    private Bitmap drawIcon()
    {
        Bitmap graph = new Bitmap( w, w, PixelFormat.Format32bppArgb );

        using( Graphics g = Graphics.FromImage( graph ) )
        {
        
           Brush brsh = Brushes.Blue;


           Color bckgrnd = Color.Yellow;

            if( active )
            {
               bckgrnd = Color.FromArgb( red, 0, 255 - red );
            }

            Rectangle rect = new Rectangle( 0, 0, w, w );

            g.Clear( bckgrnd );
            g.DrawRectangle( new Pen(brsh, 2), rect );
        }

        return graph;
    }
}
