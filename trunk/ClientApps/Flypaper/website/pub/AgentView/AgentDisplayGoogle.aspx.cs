using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using WebLibrary;
using MediaLibrary;

public partial class AgentView_AgentDisplayGoogle : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        SimClient serverUpdateObject = Utils.GetUpdateObject( this );
        MediaPlan planToSimulate = Utils.CurrentMediaPlan( this, false );

        // start os script
        string script = "map.setCenter( new GLatLng( 37.4419, -122.1419 ), 7 ); ";
        script += " map.enableScrollWheelZoom(); ";

        if( planToSimulate == null || !planToSimulate.SimulationID.HasValue )
        {
            script += DrawDefault();
        }
        else
        {
            DirectoryInfo di = PlanStorage.MoviePath( Utils.GetUser( this ), planToSimulate );

            Dictionary<int, AgentLog.Summary> agents = PlanStorage.ReadAgentData( di );

            if( agents != null )
            {
                script += DrawAgents( agents );
            }
        }

       GoogleMap1.StartScript = script;

        //ClientScript.RegisterStartupScript( this.GetType(), "GMapLoad", script );

    }

    private string DrawDefault()
    {
        string script = "";
        Random rnd = new Random( 7862 );

        string html = "<div> <table border=\"1\"> <tr><th style=\"font-size:small\"> one </th><th style=\"font-size:small\">two</th></tr><tr><td style=\"font-size:x-small\"> three </td><td style=\"font-size:x-small\">four</td></tr></table>";
        string title = "Agent : ";

        for( int ii = 0; ii < 100; ii++ )
        {
            double dx = 3 * (rnd.NextDouble() - 0.5);
            double dy = 3 * (rnd.NextDouble() - 0.5);

            int color = rnd.Next( 0, 255 );

            int size = rnd.Next( 5, 20 );

            bool aware = false;
            if( rnd.NextDouble() > 0.25 )
            {
                aware = true;
            }

            string popup = "<h4><a href=\"AgentLog.aspx?agent=" + ii.ToString() + "\" target=\"_blank\"> View Agent Log... </a></h4>";
            popup += "<p>";
            popup += html;

            script += createMarker( 34 + dx, -115 + dy, size, color, aware, title + ii.ToString(), popup );
        }

        script += "map.setCenter(latlong, 7);";

        // script += "}";


        return script;
    }

    const double delta = 0.1;
    private string DrawAgents(Dictionary<int, AgentLog.Summary> agents)
    {
        string script = "";

        double aveLat = 0;
        double aveLong = 0;

        Random rnd = new Random( 965 );

        foreach( int key in agents.Keys )
        {
            AgentLog.Summary summary = agents[key];
            Agent agent = summary.Agent;

            int geo_id = agent.House.GeoID;

            // find lat long for agent
             GeoLibrary.GeoInfo geo = GeoLibrary.GeoRegion.TopGeo[geo_id];

             double lat = -geo.Lat;
             double lng = geo.Long;

             if( geo.Long < -180 )
             {
                 lng = geo.Long + 360;
             }

             aveLat += lat;
             aveLong += lng;

             double dx = delta * (rnd.NextDouble() - 0.5);
             double dy = delta * (rnd.NextDouble() - 0.5);

             lng += dx;
             lat += dy;

               
            double persuasion = summary.Persuasion / (0.1 + summary.Persuasion);

            if( persuasion < 0 )
            {
                persuasion = 0;
            }

            bool aware = summary.Aware > 0;

            // size varies between 10 = no action to 20 for 100% action

            int size = (int) Math.Ceiling( 10 + 10 * summary.ActionTaken );
         
            int color = (int)Math.Min( 255, Math.Ceiling( 255 * persuasion ) );

            string popup = "<h4><a  style=\"font-size:small\" href=\"AgentLog.aspx?agent=" + key.ToString() + "\" target=\"_blank\"> View Agent Detail... </a></h4>";
            popup += Utils.AgentHtml( agent, "x-small" );
            popup += Utils.MediaHtml( agent, "x-small", 15 );

            script += createMarker( lat, lng, size, color, aware, geo.Name, popup );
        }

        aveLat /= agents.Count;
        aveLong /= agents.Count;

        script += "var center = new GLatLng(" + aveLat.ToString() + "," + aveLong.ToString() + ", true);";

        script += "map.setCenter(center, 9);";

        return script;
    }


    private string createMarker( double lat, double lng, int size, int color, bool active, string title, string popup )
    {
        string html = popup.Replace("\"", "\\\"");

        string script = "";

        script += "var latlong = new GLatLng(" + lat.ToString() + "," + lng.ToString() + ", true);";
        script += "var sqr = new GIcon();";
        script += "sqr.image = \"AgentIcon.aspx?size=" + size.ToString() +"&color= " + color.ToString() + "&active=" + active.ToString() + "\";";
        script += "sqr.iconSize = new GSize(" + size.ToString() + "," + size.ToString() + " );";
        script += "sqr.iconAnchor = new GPoint(0, 0);";
        script += "sqr.infoWindowAnchor = new GPoint(5, 1);";

        script += "var marker = new GMarker(latlong, { title: \" " + title + "\", icon: sqr });";

        script += "marker.bindInfoWindowHtml(\" " + html + " \");";

        //// marker.setImage("AgentIcon.aspx?size=10&color=0&active=false")
        script += "map.addOverlay(marker);";

        return script;

    }
}
