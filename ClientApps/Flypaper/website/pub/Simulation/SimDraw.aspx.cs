using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using WebLibrary;
using System.IO;

using GeoLibrary;


public partial class Simulation_SimDraw : System.Web.UI.Page
{
    //MediaPlan planToSimulate = null;
    //SimClient serverUpdateObject = null;
    //int curWidth = 320;
    //int curHeight = 240;

    protected void Page_Load( object sender, EventArgs e )
    {
        System.Drawing.Image img = drawMap();

        Response.Clear();
        Response.ContentType = "image/png";

        using( MemoryStream stmMemory = new MemoryStream() )
        {
            img.Save( stmMemory, ImageFormat.Png );
            stmMemory.WriteTo( Response.OutputStream );
        }

        // Save the image to the OutputStream
        // objBitmap.Save( Response.OutputStream, ImageFormat.Png );

        Response.End();

        // clean up...
        img.Dispose();

        // img.Save( Response.OutputStream, ImageFormat.Jpeg );
    }

    private Bitmap drawMap()
    {
        string dir = HttpContext.Current.Server.MapPath( null );

        string file = dir + @"\..\images\usa.jpg";
        Bitmap map = new Bitmap( file );

        return map;
    }
}
        