using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using WebLibrary;
using HouseholdLibrary;
using SimInterface;
using DemographicLibrary;
using BusinessLogic;
using MediaLibrary;


public partial class Simulation_Calibrate : System.Web.UI.Page
{
    MediaPlan planToSimulate = null;
    SimClient serverUpdateObject = null;

    protected void Page_Load( object sender, EventArgs e )
    {
        serverUpdateObject = Utils.GetUpdateObject( this );
        planToSimulate = Utils.CurrentMediaPlan( this, false );

        if( IsPostBack == false )
        {
            AdOptionDb.Initdb(ConfigurationManager.AppSettings["AdPlanIt.MediaDatabaseConnectionString"]);

            Dictionary<int, AdOption> options = AdOptionDb.MediaDb.ReadOptions();

            List<int> unique_option_list = new List<int>();

            PlanData.Text = null;

            if( serverUpdateObject == null )
            {
                PlanData.Text += "No server Found\r\n";
                this.RunCalibration.Enabled = false;
            }

            if( planToSimulate != null )
            {
                PlanData.Text += "Plan: " + planToSimulate.PlanName;
                PlanData.Text += "\r\nVehicles: ";
                foreach(MediaItem item in planToSimulate.GetAllItems())
                {
                    PlanData.Text += item.VehicleName + ", ";
                   
                    foreach( WebLibrary.Timing.TimingInfo info in item.TimingList )
                    {
                        if (!unique_option_list.Contains(info.AdOptionID))
                        {
                            unique_option_list.Add(info.AdOptionID);
                        }

                    }
                }

                PlanData.Text += "\r\nAd Options: ";

                foreach( int index in unique_option_list )
                {
                    AdOption option = options[index];

                    PlanData.Text += option.Name + ", ";
                }
            }
            else
            {
                PlanData.Text += "No plan defined";
                this.RunCalibration.Enabled = false;
            }

            resetProgress();

            this.ProgressTimer.Enabled = false;
        }
    }


    const int progMax = 500;
    private void resetProgress()
    {
        ProgressBox.Text = "";

        for( int ii = 0; ii < progMax; ++ii )
        {
            ProgressBox.Text += "|";
        }
    }

    protected void SimpleButton_CheckedChanged( object sender, EventArgs e )
    {
        if( this.OnlineButton.Checked )
        {
            this.MultiView1.ActiveViewIndex = 1;
        }
        else if( this.SearchButton.Checked )
        {
            this.MultiView1.ActiveViewIndex = 2;
        }
        else
        {
            this.MultiView1.ActiveViewIndex = 0;
        }
    }

    // creates and runs a sim
    protected void RunCalibration_Click( object sender, EventArgs e )
    {
        if( serverUpdateObject == null || planToSimulate == null )
        {
            return;
        }

        // Create and queue up a simulation
        string error;

        Guid? newSimID = serverUpdateObject.CreateSimulation( Utils.GetUser( this ), out error );

        if( newSimID.HasValue == false )
        {
            // display error to user
            PlanData.Text += error;
            return;
        }

        planToSimulate.SimulationID = newSimID;
      

        SimInput input = SimUtils.ConvertMediaPlanToSimInput( planToSimulate);


        // note these options are in memory not necessarily in the db
        input.calOptions = AdOptionDb.MediaDb.ReadOptions();

        if( planToSimulate.PopulationSize == 0 )
        {
            planToSimulate.PopulationSize = SimUtils.GetPopulationSize( this.planToSimulate );
        }


        Calibrator.CalibrateInput( planToSimulate, input );


        // STEP 2 - update the input settings on the server
        serverUpdateObject.UpdateInput( (Guid)planToSimulate.SimulationID, input );

        // STEP 3 - queue it up
        serverUpdateObject.QueueSim( (Guid)planToSimulate.SimulationID );

        PlanData.Text += "Simulation Starting";


        // make sure plan reflects running state
        planToSimulate.UpdateStatus();
        Utils.SetCurrentMediaPlan( this, planToSimulate );

        resetProgress();

        this.ProgressTimer.Enabled = true;

    }

    protected void UpdateDb_Click( object sender, EventArgs e )
    {
        AdOptionDb.MediaDb.Update();
    }

    protected void UndoDb_Click( object sender, EventArgs e )
    {
        AdOptionDb.MediaDb.RefreshAdOptions();
        GridView1.DataBind();
        GridView2.DataBind();
        GridView3.DataBind();
    }

    protected void ProgressTimer_Tick( object sender, EventArgs e )
    {
        if( planToSimulate == null || serverUpdateObject == null )
        {
            ProgressTimer.Enabled = false;
            return;
        }

        Guid simId = planToSimulate.SimulationID.Value;

        double progress = serverUpdateObject.GetSimProgress( simId );

        int endex = (int) Math.Ceiling(progMax * progress);

        ProgressBox.Text = "";
        int ii = 0;
        for(; ii < endex; ++ii)
        {
            ProgressBox.Text += "#";
        }

         for(; ii < progMax; ++ii)
         {
            ProgressBox.Text += "|";
         }


        bool done = serverUpdateObject.SimDone( simId );

        if( done )
        {
            this.ProgressTimer.Enabled = false;

            SimOutput simOutput = serverUpdateObject.GetResults( simId );
            // byte[] bLogs = serverUpdateObject.GetData( simId );

            if( simOutput == null )
            {
                ProgressBox.Text = "Error reading results";
                return;
            }

            planToSimulate.Results = simOutput;

            Scoring scoringService = new Scoring( this.planToSimulate );
            planToSimulate.PlanOverallRatingStars = scoringService.GetRatingStars();

            planToSimulate.PlanOverallGoalScores = new System.Collections.Generic.List<double>();
            System.Collections.Generic.Dictionary<MediaCampaignSpecs.CampaignGoal, double> goalScores = scoringService.GoalScores();

            for( int g = 0; g < planToSimulate.Specs.CampaignGoals.Count; g++ )
            {
                double goalScore = goalScores[planToSimulate.Specs.CampaignGoals[g]];
                planToSimulate.PlanOverallGoalScores.Add( goalScore );
            }

            PlanStorage storage = new PlanStorage();
            storage.SaveMediaPlan( Utils.GetUser( this ), planToSimulate );
            Utils.SetCurrentMediaPlan( this, planToSimulate );

            ProgressBox.Text = "Simulation Complete";
        }

    }
    protected void DownloadBut_Click( object sender, EventArgs e )
    {

        Response.AddHeader( "Content-Disposition", "attachment; filename=options.dat" );
        Response.ContentType = "application/octet-stream";

        MemoryStream str = new MemoryStream();

         BinaryFormatter serializer = new BinaryFormatter();

         serializer.Serialize( str, AdOptionDb.MediaDb.ReadOptions() );


         Response.Clear();
         Response.BinaryWrite( str.GetBuffer() );
         Response.End();

        str.Close();

    }
}
