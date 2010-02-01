using System;
using System.Collections;
using System.Collections.Generic;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using WebLibrary;
using HouseholdLibrary;
using SimInterface;
using DemographicLibrary;
using BusinessLogic;

using MediaLibrary;

public partial class Simulation_Simulate : System.Web.UI.Page
{
    const int ViewMapOnly = 2;
    const int AlwaysRun = 1;

    static int debugLevel = 0;

    MediaPlan planToSimulate = null;
    SimClient serverUpdateObject = null;

    private static int v = 1;

    protected void Page_Load( object sender, EventArgs e )
    {
        serverUpdateObject = Utils.GetUpdateObject( this );
        planToSimulate = Utils.CurrentMediaPlan( this, false );

        if( debugLevel != ViewMapOnly )
        {
            if( serverUpdateObject == null || planToSimulate == null )
            {
                Response.Redirect( @"..\Home.aspx" );
                return;
            }

            if( checkSim() && debugLevel != AlwaysRun )
            {
                Response.Redirect( @"..\Analysis.aspx" );
                return;
            }
            

            // no results to be had
            if( !IsPostBack )
            {
                if( this.Request.Params["SimInitialized"] != null )
                {
                    // draw agents as needed
                    StatusBox.Text = "Simulation Initialized";
                    DrawMap();
                    Redraw.Enabled = true;

                }
                else if( this.Request.Params["QueueSim"] != null )
                {
                  
                }
                else
                {
                    // First time - if no results we want to run a simulation
                    createSim();
                    
                    // come back right away to queue up simulation
                    // we do not update the status here
                    // StatusBox.Text = "Setting Up Simulation";

                    QueueSim_Tick( null, null );

                    double aveLat = 0.0;
                    double aveLng = 0.0;

                    SimUtils.GetLatLong( planToSimulate, out  aveLat, out aveLng );

                    GoogleMap1.Lat = aveLat;
                    GoogleMap1.Lng = aveLng;

                    GoogleMap1.ZoomLevel = 9;

                    // start up redraw timer
                    Redraw.Enabled = true;

                    StatusBox.Text = "Simulation sent to server";

                    // Server.Transfer( "Simulate.aspx?QueueSim=true", true );
                }
            }
            else
            {
               // redraw
                //if( this.Request.Params["SimInitialized"] == null && DataReady() )
                //{
                //    Redraw.Enabled = false;
                //    Response.Redirect( "Simulate.aspx?SimInitialized=true", true );
                //}
                //else if( this.Request.Params["SimInitialized"] == null )
                //{
                //    if( StatusBox.Text == "Simulation sent to server" )
                //    {
                //        StatusBox.Text = "Waiting ";
                //    }
                //    else if( StatusBox.Text.Contains( "Waiting" ) && StatusBox.Text.Length < 100)
                //    {
                //        StatusBox.Text += ".";
                //    }
                //}
            }
        }
        else
        {
            GoogleMap1.Lng = -122;
            GoogleMap1.Lat = 36;
            GoogleMap1.ZoomLevel = 8;
        }
    }

    private bool createSim()
    {
        // Create and queue up a simulation
        string error;
        planToSimulate.SimulationID = null;

        Guid? newSimID = serverUpdateObject.CreateSimulation( Utils.GetUser( this ), out error );

        if( newSimID.HasValue == false )
        {
            // display error to user
            StatusBox.Text = error;
            return false;
        }

       planToSimulate.SimulationID = newSimID;
       Utils.SetCurrentMediaPlan( this, planToSimulate );

        Redraw.Enabled = false;

        return true;
    }

    private bool checkSim()
    {
        if( !planToSimulate.SimulationID.HasValue )
        {
            return false;
        }

        if( planToSimulate.Results != null )
        {
            return true;
        }

        Guid simId = planToSimulate.SimulationID.Value;

        double prog = serverUpdateObject.GetSimProgress(simId );

        if( prog < 0 )
        {
            // this is an error
            StatusBox.Text =  "Error Running Simulation";
         
            // this is an error turn off timers
            Redraw.Enabled = false;
     
            return false;
        }

        int percent = (int) Math.Floor(100.0 * prog);

        if( percent == 100 )
        {
            StatusBox.Text = "Generating reports, please wait.";
        }
        else
        {
            StatusBox.Text = "Simulation Running: " + percent.ToString() + "% Complete.";
        }
      
        bool done = serverUpdateObject.SimDone( simId );

        if( done && debugLevel != AlwaysRun )
        {
            SimOutput simOutput = serverUpdateObject.GetResults( simId );

            if( simOutput == null )
            {
                StatusBox.Text = "Error reading results";
                Response.Redirect( @"..\HQ.aspx" );
            }

            planToSimulate.Results = simOutput;

            Scoring scoringService = new Scoring(this.planToSimulate);
            planToSimulate.PlanOverallRatingStars = scoringService.GetRatingStars();
            string scoreLog = String.Format( "s={0}", planToSimulate.PlanOverallRatingStars );

            planToSimulate.PlanOverallGoalScores = new System.Collections.Generic.List<double>();
            System.Collections.Generic.Dictionary<MediaCampaignSpecs.CampaignGoal, double> goalScores = scoringService.GoalScores();

            for( int g = 0; g < planToSimulate.Specs.CampaignGoals.Count; g++ ) {
                double goalScore = goalScores[ planToSimulate.Specs.CampaignGoals[ g ] ];
                planToSimulate.PlanOverallGoalScores.Add( goalScore );
                scoreLog += String.Format( "{0}={1}", planToSimulate.Specs.CampaignGoals[ g ].ToString().Substring( 0, 3 ).ToLower(), goalScore );
            }

            PlanStorage storage = new PlanStorage();
            storage.SaveMediaPlan( Utils.GetUser( this ), planToSimulate );
            Utils.SetCurrentMediaPlan( this, planToSimulate );

            try
            {
                // get agent data and store it

                byte[] data = serverUpdateObject.GetData( planToSimulate.SimulationID.Value );

                if( data != null )
                {


                    MemoryStream str = new MemoryStream( data );
                    BinaryFormatter serializer = new BinaryFormatter();

                    Dictionary<int, AgentLog> agent_logs = (Dictionary<int, AgentLog>)serializer.Deserialize( str );
                    DirectoryInfo di = PlanStorage.MoviePath( Utils.GetUser( this ), planToSimulate );
                    PlanStorage.SaveMovieData( di, agent_logs );
                }
            }
            catch(System.Exception e )
            {
            }

            DataLogger.Log( "SIMDONE", Utils.GetUser( this ), this.planToSimulate.PlanDescription, scoreLog );
        }

        return done;
    }

    protected void QueueSim_Tick( object sender, EventArgs e )
    {
        // we have no results
        // the simulation has a value but curiously we are in the ready state
        // we put ourselves here so we need to queue up the sim
        SimInput input = SimUtils.ConvertMediaPlanToSimInput( planToSimulate);

        // input.calOptions = Utils.MediaDatabase.ReadOptions();

        // 
        // TBD Find out where PopulationSize == 0 is coming from
        //
        //
        if( planToSimulate.PopulationSize == 0 )
        {
            planToSimulate.PopulationSize = SimUtils.GetPopulationSize( this.planToSimulate );
        }

        Calibrator.CalibrateInput( planToSimulate, input );


        // STEP 2 - update the input settings on the server
        serverUpdateObject.UpdateInput( (Guid)planToSimulate.SimulationID, input );

        // STEP 3 - queue it up
        serverUpdateObject.QueueSim( (Guid)planToSimulate.SimulationID );

     


        // make sure plan reflects running state
        planToSimulate.UpdateStatus();
        Utils.SetCurrentMediaPlan( this, planToSimulate );
       
    }

    protected void Redraw_Tick( object sender, EventArgs e )
    {
    }

    private bool DataReady()
    {
        byte[] data = serverUpdateObject.GetData( planToSimulate.SimulationID.Value );

        return data != null;
    }

    protected void DrawMap( )
    {
        // get data and store it
        byte[] data = serverUpdateObject.GetData( planToSimulate.SimulationID.Value );

        if( data == null )
        {
            return;
        }

        MemoryStream str = new MemoryStream( data );
        BinaryFormatter serializer = new BinaryFormatter();

        Dictionary<int, AgentLog> agent_logs = (Dictionary<int, AgentLog>)serializer.Deserialize( str );

        double aveLat = 0;
        double aveLng = 0;
        SimUtils.GetLatLong( planToSimulate, out  aveLat, out aveLng );

        string script = " map.setCenter( new GLatLng( " + aveLat.ToString() + "," + aveLng.ToString() + " ), 7 ); ";

        if( agent_logs != null )
        {
            script += DrawAgents( agent_logs );
        }

        GoogleMap1.StartScript = script;
    }

    const double delta = 0.1;
    private string DrawAgents( Dictionary<int, AgentLog> agents )
    {

        if( agents.Count == 0 )
        {
            return null;
        }

        string script = "";

        double aveLat = 0;
        double aveLong = 0;

        Random rnd = new Random( 965 );

        foreach( int key in agents.Keys )
        {
            AgentLog log = agents[key];
            Agent agent = log.agent;

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

            script += createMarker( lat, lng, geo.Name );
        }

        GoogleMap1.Lat = aveLat / agents.Count;
        GoogleMap1.Lng = aveLong / agents.Count;
        GoogleMap1.ZoomLevel = 8;

        return script;
    }

private string createMarker( double lat, double lng, string title)
    {
        string script = "var latlong = new GLatLng(" + lat.ToString() + "," + lng.ToString() + ", true);";
        script += "var sqr = new GIcon();";
        script += "sqr.image = \"../AgentView/AgentIcon.aspx?size=10&color=255&active=true\";";
        script += "sqr.iconSize = new GSize(10,10);";
        script += "sqr.iconAnchor = new GPoint(0, 0);";
        script += "sqr.infoWindowAnchor = new GPoint(5, 1);";

        script += "var marker = new GMarker(latlong, { title: \" " + title + "\", icon: sqr });";

        script += "map.addOverlay(marker);";

        return script;

    }

    protected void StopSim_Click( object sender, EventArgs e )
    {
        Response.Redirect( @"..\HQ.aspx" );
    }
}