using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Live.ServerControls.VE;

using WebLibrary;
using Utilities;
using MediaLibrary;

public partial class AgentView_AgentDisplay : System.Web.UI.Page
{
    const double delta = 0.1;
    MediaPlan planToSimulate = null;
    SimClient serverUpdateObject = null;

    protected void Page_Load( object sender, EventArgs e )
    {
        serverUpdateObject = Utils.GetUpdateObject( this );
        planToSimulate = Utils.CurrentMediaPlan( this, false );

        if( planToSimulate == null || !planToSimulate.SimulationID.HasValue )
        {
            AgentMap.ZoomLevel = 4;

            LatLongWithAltitude pt = new LatLongWithAltitude( AgentMap.Center.Latitude, AgentMap.Center.Longitude );
            Shape shp = new Shape( ShapeType.Pushpin, pt );

            shp.Title = "<h2>Household Information</h2>";
            shp.Title += "<h4><a href=\"AgentLog.aspx target=\"_blank\"> View Agent Log... </a></h4>";

            shp.Description = "<div style='font-size:12px;font-weight:bold;border:solid 2px Black;background-color:Aqua;width:300px;z-index:3'>";

          
          
            shp.Description = "<div style='font-size:12px;font-weight:bold;border:solid 2px Black;background-color:Aqua;width:300px;Height:800px;z-index:3'>";

            shp.Description += "</div>";

            this.AgentMap.AddShape( shp );

            return;
        }

        DirectoryInfo di = PlanStorage.MoviePath( Utils.GetUser( this ), planToSimulate );

        if( !di.Exists && serverUpdateObject != null )
        {
            // get data and store it
            byte[] data = serverUpdateObject.GetData( planToSimulate.SimulationID.Value );

            if( data == null )
            {
                // No Agent Data available
                return;
            }

            MemoryStream str = new MemoryStream(data);
            BinaryFormatter serializer = new BinaryFormatter();

            Dictionary<int, AgentLog> agent_logs = (Dictionary<int, AgentLog>) serializer.Deserialize( str );

            PlanStorage.SaveMovieData( di , agent_logs );

            di = PlanStorage.MoviePath( Utils.GetUser( this ), planToSimulate );
        }

        Dictionary<int, AgentLog.Summary> agents = PlanStorage.ReadAgentData( di );

        DrawAgents(agents );
    }

    private void DrawAgents(Dictionary<int, AgentLog.Summary> agents )
    {

        if( agents.Count == 0 )
        {
            return;
        }

        AgentMap.ShowFindControl = false;
        AgentMap.ClearInfoBoxStyles = true;

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
             double lon = geo.Long;

             if( geo.Long < -180 )
             {
                 lon = geo.Long + 360;
             }

             //double x = double.Parse( row[4] ); // long
             //     double y = -double.Parse( row[3] ); // lat

             //     // translate to picture coords
             //     if( x > 0 )
             //     {
             //         x = -(360 - x);
             //     }

             //     info.Long = x;
             //     info.Lat = y;


             double dx = delta * (rnd.NextDouble() - 0.5);
             double dy = delta * (rnd.NextDouble() - 0.5);

             lon += dx;
             lat += dy;

            LatLongWithAltitude pt = new LatLongWithAltitude( lat, lon );
            aveLat += lat;
            aveLong += lon;

            Shape shp = new Shape( ShapeType.Pushpin, pt );

            // shp.CustomIconSpec = new CustomIconSpecification( bck, "", bck, "Star.png", px, false, "", "", false, px, 1, false );

           // shp.CustomIconSpec.Image = "Star.png";
            //shp.HideIcon();
            double persuasion = summary.Persuasion / (0.1 + summary.Persuasion);

            if( persuasion < 0 )
            {
                persuasion = 0;
            }

            string aware = summary.Aware > 0 ? "true" : "false";

            string picParms = "?";
            int size = (int)Math.Min( 40, Math.Ceiling( 10 + 100 * summary.ActionTaken ) );
         

            picParms += "size=" + size.ToString() ;
            int color = (int)Math.Min( 255, Math.Ceiling( 255 * persuasion ) );
            picParms += "&color=" + color.ToString();
            picParms += "&active=" + aware;

            shp.Altitude = 100 - size;
            shp.CustomIcon = "AgentIcon.aspx" + picParms;

            shp.Title = "<h2>Household Information</h2>";
            shp.Title += "<h4><a href=\"AgentLog.aspx?agent=" + key.ToString() + "\" target=\"_blank\"> View Agent Detail... </a></h4>";

            shp.Description = "<div style='font-size:12px;font-weight:bold;border:solid 2px Black;background-color:Aqua;width:300px;z-index:3'>";

            shp.Description += Utils.AgentHtml( agent, "small" );

            shp.Description += Utils.MediaHtml( agent, "small", 20 );
             
            shp.Description += "</div>";

          
            this.AgentMap.AddShape( shp );
        }

        aveLong /= agents.Keys.Count;
        aveLat /= agents.Keys.Count;

        LatLong center = new LatLong(aveLat, aveLong);

        AgentMap.Center = center;
    }
}