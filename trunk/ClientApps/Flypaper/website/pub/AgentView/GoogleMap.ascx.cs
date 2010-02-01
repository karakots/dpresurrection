using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AgentView_GoogleMap : System.Web.UI.UserControl
{
    static private string defaultScript  = "  map.setCenter( new GLatLng( 37.4419, -100), 1 ); ";

    private string startScript = defaultScript;
    public string StartScript
    {
        get
        {
            return startScript;
        }

        set
        {
            startScript = value;
        }
    }

    public string GStyle { get; set; }

    private double zoom = 1;
    public int ZoomLevel {get;set;}

    private double lat = 37.4419;
    public double Lat { get; set; }

    private double lng = -100;
    public double Lng { get; set; }

    protected void Page_Load( object sender, EventArgs e )
    {
        InitMap();

        // load javascript
        string script = " <script type=\"text/javascript\"> ";
        script += "function initMap() { ";
        script += "desired.zoom = " + ZoomLevel.ToString() + "; " ;
        script += "desired.center = new GLatLng( " + Lat.ToString() + "," + Lng.ToString()+ ");  ";
        
        script += StartScript;
        script += " }</script>";
        Page.ClientScript.RegisterClientScriptBlock( this.GetType(), "GMapSpecs", script );
    }

    public void InitMap()
    {
        string stagingKey = "ABQIAAAALdbbhfBcKj_ZKDS1I3a80RR5Dw7sEU9MhjbzHT7iuvCzFz7rwxQ91OQfz7l8oIdFQsG7a6ENLnb9OA";
        string productKey = "ABQIAAAA6nAoGBvqXG466LAFFKdm-BTOaYYjd9mhQclxQRnTNXFc5uT-uhTkm3PWadv6VbBD9qUc4oQWG-F1Dg";

        string googleKey = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;key=";

        if( System.Configuration.ConfigurationManager.AppSettings["Adplanit.DevelopmentMode"] == "TRUE" )
        {
            googleKey += stagingKey;
        }
        else
        {
            googleKey += productKey;
        }

        googleKey += "\" type=\"text/javascript\"></script>";

        Page.ClientScript.RegisterStartupScript( Page.GetType(), "GMapKey", googleKey );

        //string rootpath = Server.MapPath( null );

        Page.ClientScript.RegisterClientScriptInclude( "MyScript", Page.ResolveUrl("~/AgentView/DpMap.js") );

        string mapStr = "<div id=\"DPEZGmap\" style=\"" + GStyle + "\">";

        GoogleMap.InnerHtml = mapStr;
    }

}
