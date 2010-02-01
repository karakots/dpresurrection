#define USE_VEHICLES
using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using SimInterface;
using HouseholdLibrary;
using DemographicLibrary;
using MediaLibrary;
using GeoLibrary;

namespace WebLibrary
{
    /// <summary>
    /// SimUtils contains methods pertaining to running simulations.
    /// </summary>
    public class SimUtils
    {
        #region Simulation Running and Status Checking
        /// <summary>
        /// Returns a list of SimStatus objects describing the state of each item in the running-plans list, and returns the updated running-plans list.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <returns></returns>
        //public static List<Guid> UpdateRunningSimStatus( System.Web.UI.Page callingPage, List<Guid> runningPlanIDs, List<MediaPlan> all_plans, out List<SimStatus> simStatus ) {
        //    List<Guid> currentRunningPlans = new List<Guid>();
        //    simStatus = new List<SimStatus>();
        //    SimClient serverUpdateObject = Utils.GetUpdateObject( callingPage );

        //    foreach( Guid planID in runningPlanIDs ) {
        //        MediaPlan runningPlan = null;
        //        foreach (MediaPlan plan in all_plans)
        //        {
        //            if (plan.PlanID == planID)
        //            {
        //                runningPlan = plan;
        //            }
        //        }

        //        if( runningPlan != null && runningPlan.SimulationID != null ) {
        //            Guid simID = (Guid)runningPlan.SimulationID;

        //            SimStatus curSimStatus = null;

        //            if( runningPlan.SimulationStatus != null ) {
        //                curSimStatus = runningPlan.SimulationStatus;
        //            }
        //            else {
        //                // this is the first time this sim has been checked
        //                curSimStatus = new SimStatus( simID );
        //            }

        //            //get the sim status from the server
        //            curSimStatus.ProgressPercent = serverUpdateObject.GetSimProgress( simID ) * 100;
        //            curSimStatus.Done = serverUpdateObject.SimDone( simID );

        //            curSimStatus.TryCount += 1;
        //            runningPlan.SimulationStatus = curSimStatus;
        //            simStatus.Add( curSimStatus );

        //            if( curSimStatus.Done == false ) {
        //                // the plan is running
        //                runningPlan.PlanStatus = MediaPlan.MediaPlanStatus.RUNNING;

        //                // include only plans with a running sim in the return list of running plans
        //                currentRunningPlans.Add( runningPlan.PlanID );
        //            }
        //            else {
        //                // the simulation is done!
        //                runningPlan.PlanStatus = MediaPlan.MediaPlanStatus.SIMULATED;

        //                if( runningPlan.Results == null ) {
        //                    // This is the first time we've found this sim is done!  Get the sim results from the server !!
        //                    SimOutput simOutput = serverUpdateObject.GetResults( simID );

        //                    string resCnt = "null";
        //                    if( simOutput != null ){
        //                        if( simOutput.metrics != null ){
        //                            resCnt = simOutput.metrics.Count.ToString();
        //                        }
        //                        else {
        //                            resCnt = "an empty list of";
        //                        }
        //                    }
        //                    DataLogger.Log( "Sim Done - sim ID {0}.  Now {1} version (2) has {3} results values", simID.ToString(), runningPlan.CampaignName, runningPlan.PlanVersion, resCnt );

        //                    runningPlan.Results = simOutput;
        //                    runningPlan.SimulationID = null;          // prevent further checking of this sim on the server

        //                    //save the updated plan to permanent storage 
        //                    PlanStorage storage = new PlanStorage();
        //                    storage.SaveMediaPlan( Utils.GetUser( callingPage ), runningPlan );
        //                }
        //                else {
        //                    // just keep the existing results since we already got them from the server
        //                }
        //            }
        //        }
        //        else {
        //            // plan or sim ID not found!-- do not add ID to new list...

        //            if( runningPlan != null && runningPlan.SimulationID == null ) {
        //                //...but the simStatus lis parallels the input IDs, so we do need to add an object there
        //                SimStatus nonRuningPlanStatus = new SimStatus( new Guid() );
        //                if( runningPlan.Results != null ) {
        //                    nonRuningPlanStatus.Done = true;
        //                    nonRuningPlanStatus.ProgressPercent = 100;
        //                }
        //                else {
        //                    // !not sure if thiw will ever happen!
        //                    nonRuningPlanStatus.Done = false;
        //                    nonRuningPlanStatus.ProgressPercent = 0;
        //                }
        //                simStatus.Add( nonRuningPlanStatus );
        //            }
        //        }
        //    }

        //    return currentRunningPlans;
        //}

        /// <summary>
        /// Runs the simulation of the given media plan on the server.  Clears any existing results for the media plan in preparation for the new sim results.
        /// </summary>
        /// <param name="callingPage"></param>
        /// <param name="planToSimulate"></param>
        //public static void RunSimulation( System.Web.UI.Page callingPage, MediaPlan planToSimulate, UserMedia userMedia ) {
        //    // do a final validation of the inputs before we connect to the server
        //    DataLogger.Log( "START {0} Simulation: {1} {2}...", Utils.GetUser( callingPage ), planToSimulate.CampaignName, planToSimulate.PlanVersion );
        //    DateTime t0 = DateTime.Now;

        //    SimInput dummy = SimUtils.ConvertMediaPlanToSimInput( planToSimulate, userMedia );
        //    DateTime t1 = DateTime.Now;

        //    SimClient serverUpdateObject = Utils.GetUpdateObject( callingPage );
        //    DateTime t2 = DateTime.Now;

        //    // STEP 1 -- create a new simulation on the server
        //    Guid? newSimID = serverUpdateObject.CreateSimulation( Utils.GetUser( callingPage ) );
        //    DateTime t3 = DateTime.Now;

        //    if( newSimID.HasValue == false ) {
        //         throw new Exception( "Error: Unable to create new simulation on server." );
        //   }
            
        //    planToSimulate.SimulationID = newSimID;
        //    planToSimulate.SimulationStatus = new SimStatus( (Guid)newSimID );
        //    planToSimulate.Results = null;
        //    planToSimulate.PlanStatus = MediaPlan.MediaPlanStatus.RUNNING;

        //    // STEP 2 - update the input settings on the server
        //    serverUpdateObject.UpdateInput( (Guid)newSimID, SimUtils.ConvertMediaPlanToSimInput( planToSimulate, userMedia ) );
        //    DateTime t4 = DateTime.Now;

        //    // STEP 3 - queue the sim to run
        //    serverUpdateObject.QueueSim( (Guid)newSimID );
        //    DateTime t5 = DateTime.Now;

        //    // update the list of plans to monitor
        //    List<Guid> runningPlans = Utils.RunningMediaPlanIDs( callingPage );
        //    if( runningPlans == null ) {
        //        runningPlans = new List<Guid>();
        //    }

        //    runningPlans.Add( planToSimulate.PlanID );
        //    Utils.SetRunningMediaPlanIDs( callingPage, runningPlans );
        //    DateTime t6 = DateTime.Now;

        //    int dt = (int)Math.Round( (t6 - t0).TotalMilliseconds );
        //    int dt1 = (int)Math.Round( (t1 - t0).TotalMilliseconds );
        //    int dt2 = (int)Math.Round( (t2 - t1).TotalMilliseconds );
        //    int dt3 = (int)Math.Round( (t3 - t2).TotalMilliseconds );
        //    int dt4 = (int)Math.Round( (t4 - t3).TotalMilliseconds );
        //    int dt5 = (int)Math.Round( (t5 - t4).TotalMilliseconds );
        //    int dt6 = (int)Math.Round( (t6 - t5).TotalMilliseconds );

        //    DataLogger.Log( "...sim started in {0} ms.  Section times: {1}, {2}, {3}, {4}, {5}, {6}", dt, dt1, dt2, dt3, dt4, dt5, dt6 );
        //}
        #endregion

        #region Input Conversion
        /// <summary>
        /// Returns the SimInput object appropriate for simulating the given media plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public static SimInput ConvertMediaPlanToSimInput( MediaPlan plan) 
        {
            List<MediaItem> items = plan.GetAllItems();
            SimInput input = new SimInput();



            input.Demographics = GetDemographics(plan);
            input.PurchaseInterval = (int)Utils.PurchaseCycleLengthForCode(plan.PurchaseCycleCode);
            input.EndDate = (plan.EndDate - plan.StartDate).Days;
            input.ConsiderationInterval = plan.Specs.TimeInConsideration / 100.0;

            foreach (MediaItem item in items)
            {
                input.Media.AddRange(item.GetMediaComponents(plan.StartDate));
            }

            

            return input;
        }

        public static void GetLatLong( MediaPlan plan, out double aveLat, out double aveLng )
        {
            aveLat = 0;
            aveLng = 0;

            if( plan.Specs.GeoRegionNames.Count > 0 )
            {
                foreach( string reg in plan.Specs.GeoRegionNames )
                {
                    GeoRegion geo = GeoRegion.TopGeo.GetSubRegion( reg );

                    aveLat += geo.AveLat;
                    aveLng += geo.AveLng;
                }

                aveLng /= plan.Specs.GeoRegionNames.Count;
                aveLat /= plan.Specs.GeoRegionNames.Count;
            }
        }

        /// <summary>
        /// Converts the given media item's flighting spec into the set of individual MediaPulse objects it represents
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        //public static List<MediaPulse> GetIndividualPulses( MediaItem media, int totalPlanLengthDays ) {
        //    FlightingInfo info = media.Flighting;
        //    List<MediaPulse> pulseList = new List<MediaPulse>();

        //    int day = (int)info.PulseDelayDays;
        //    int cycleStartDay = (int)info.PulseDelayDays;

        //    do {
        //        // add a cycle
        //        for( int p = 0; p < info.PulsesPerCycle; p++ ) {

        //            // add a pulse
        //            int pulseDays = (int)Math.Ceiling( info.PulseDays );
        //            MediaPulse pulse = new MediaPulse( day, pulseDays );
        //            pulseList.Add( pulse );

        //            day = day + pulseDays + (int)info.DaysBetweenPulses;
        //            if( day >= totalPlanLengthDays ) {
        //                break;
        //            }
        //        }

        //        cycleStartDay += info.CycleDays;

        //        // dobule-check to be sure we don't have overlapping pulses
        //        if( day - info.DaysBetweenPulses > cycleStartDay ) {
        //            string msg = String.Format( "Error: Overlapping pulses specified for media \"{0}\". Overlap occurs on day {1} of the plan.", media.ItemName, cycleStartDay );
        //            throw new Exception( msg );
        //        }

        //        day = cycleStartDay;
        //    }
        //    while( day < totalPlanLengthDays );

        //    return pulseList;
        //}

        // code clean up
        //private static string LegalAdOption( string adOption, UserMedia userMedia ) {
        //    string legalAdOption = adOption;

        //    try {
        //        Guid userOptionOrPackageID = new Guid( adOption );
        //        if( userMedia != null && userMedia.MediaSpotSpecs != null && userMedia.MediaSpotSpecs.Count > 0 ) {
        //            foreach( UserMedia.MediaSpotSpec uSpotSpec in userMedia.MediaSpotSpecs ) {
        //                if( uSpotSpec.SpotID == userOptionOrPackageID ) {
        //                    // found a user-defined ad option with thid ID - replace with most similar existing ad option
        //                    legalAdOption = uSpotSpec.ComparableSpot;
        //                }
        //            }
        //        }
        //    }
        //    catch( Exception ) {
        //        // if not a guid, then the option is automatically legal
        //    }

        //    return legalAdOption;
        //}

        #endregion

        #region Media Package Utilities
        /// <summary>
        /// Expands the component into a set of components, if the given component is a media package.  Otherwise it returns a list with just the original component in it.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="userMedia"></param>
        /// <returns></returns>
        //private static List<MediaComp> ExpandIfPackage( MediaComp comp, UserMedia userMedia ) {
        //    List<MediaComp> compList = new List<MediaComp>();
        //    try {
        //        Guid packageID = new Guid( comp.ad_option );
        //        if( userMedia != null && userMedia.MediaPackageSpecs != null && userMedia.MediaPackageSpecs.Count > 0 ) {
        //            foreach( UserMedia.MediaPackageSpec uPackSpec in userMedia.MediaPackageSpecs ) {
        //                if( uPackSpec.PackageID == packageID ) {
        //                    // we found the package
        //                    compList.AddRange( ExpandPackage( comp, uPackSpec, userMedia ) );
        //                }
        //            }
        //        }
        //    }
        //    catch( Exception ) {
        //        // the ad option isn't a GUID
        //        compList.Add( comp );
        //    }
        //    return compList;
        //}

        ///// <summary>
        ///// Expands the given media component into the set of components represented by the given media package.
        ///// </summary>
        ///// <param name="comp"></param>
        ///// <param name="packageSpec"></param>
        ///// <returns></returns>
        //private static List<MediaComp> ExpandPackage( MediaComp comp, UserMedia.MediaPackageSpec packageSpec, UserMedia userMedia ) {
        //    List<MediaComp> compList = new List<MediaComp>();

        //    foreach( UserMedia.MediaPackageSpec.SpotItem spotItem in packageSpec.SpotItems ){
        //        // start with a copy of the given MediaComp
        //        MediaComp realComp = new MediaComp();
        //        realComp.demo_fuzz_factor = comp.demo_fuzz_factor;
        //        realComp.Guid = comp.Guid;
        //        realComp.Impressions = comp.Impressions;
        //        realComp.region_fuzz_factor = comp.region_fuzz_factor;
        //        realComp.Span = comp.Span;
        //        realComp.target_demo.Add(comp.target_demo);
        //        realComp.target_region.Add(comp.target_region);

        //        realComp.StartDate = comp.StartDate + spotItem.SpotDate;
        //        realComp.ad_option = LegalAdOption( spotItem.SpotType, userMedia );
        //        realComp.Span = 1;          //!!!TBD need to also handle media package for non-daily items

        //        compList.Add( realComp );
        //    }

        //    return compList;
        //}

        #endregion

        #region General Conversions
        /// <summary>
        /// Retuns the GeoRegion object specified by the given name list [ State, DMA, Locale ];
        /// </summary>
        /// <param name="geoRegionName"></param>
        /// <returns></returns>
        public static GeoRegion GeoRegionForName( List<string> geoRegionName ) {
            GeoRegion node = GeoRegion.TopGeo;

            // note we skip over the 0th value (the sate) since DMAS are actually directly in the top geo list
            for( int i = 1; i < geoRegionName.Count; i++ ) {
                // go in to subnode
                node = node.GetSubRegion(geoRegionName[i]);
            }
            return node;
        }

        /// <summary>
        /// Retuns the GeoRegion object specified by the DMA name;
        /// </summary>
        /// <param name="geoRegionName"></param>
        /// <returns></returns>
        public static GeoRegion GeoRegionForName( string geoRegionName ) {
            GeoRegion node = GeoRegion.TopGeo;
            node = node.GetSubRegion( geoRegionName );
            return node;
        }

        public static List<Demographic> GetDemographics(MediaPlan plan)
        {
            // make sure demographic names are unique
            for( int ii = 0; ii < plan.Specs.Demographics.Count; ++ii )
            {
                int num = 0;
                for( int jj = ii + 1; jj < plan.Specs.Demographics.Count; ++jj )
                {
                    if( plan.Specs.Demographics[ii].DemographicName == plan.Specs.Demographics[jj].DemographicName )
                    {
                        num++;

                        plan.Specs.Demographics[jj].DemographicName += "(" + num.ToString() + ")";
                    }
                }
            }

            List<Demographic> rval = new List<Demographic>();
            foreach( string reg in plan.Specs.GeoRegionNames )
            {

                for( int ii = 0; ii < plan.Specs.Demographics.Count; ii++ )
                {
                    Demographic demo = ConvertToDemographic( plan, ii );
                    demo.Region = reg;
                    demo.Name += "@" + reg;

                    rval.Add( demo );
                }
            }


            if (rval.Count > 0)
            {
                plan.Specs.SegmentList = rval;
            }
            

            


            return rval;
        }

        public static double GetPopulationSize(MediaPlan plan)
        {
            List<GeoRegion> region_list = new List<GeoRegion>();
            List<Demographic> demographics = new List<Demographic>();

            foreach( string regionName in plan.Specs.GeoRegionNames )
            {
                region_list.Add( GeoRegionForName( regionName ) );
            }

            for( int i = 0; i < plan.Specs.Demographics.Count; i++ )
            {  
                demographics.Add( ConvertToDemographic( plan, i ) );
            }

            double target_pop_size = Utils.MediaDatabase.TargetPopulation( demographics, region_list ).Any * DpMediaDb.US_HH_SIZE;

            return target_pop_size;
        }

        /// <summary>
        /// Returns the HouseholdLibrary.Demographic object that represents the given segment of the given media plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="segmentIndex"></param>
        /// <returns></returns>
        public static Demographic ConvertToDemographic(MediaPlan plan, int segmentIndex)
        {
            Demographic demo = new Demographic();

            // segment name
            demo.Name = plan.Specs.Demographics[ segmentIndex ].DemographicName;

            // gender
            bool male = plan.Specs.GetValue( segmentIndex, "Gender", "Male" );
            bool female = plan.Specs.GetValue( segmentIndex, "Gender", "Female" );

            demo.Gender = new DemographicType<Gender>();
            if( ((male && female) == false) && ((male || female) == true) ) {        // take the "else" (set ANY) if all are true, or all are false
                if( male ) {
                    demo.Gender = demo.Gender | "MALE";
                }
                else if( female ) {
                    demo.Gender = demo.Gender | "FEMALE";
                }
            }
            else {
                demo.Gender = demo.Gender | "ANY";
            }

            // race
            bool asian = plan.Specs.GetValue( segmentIndex, "Race", "Asian" );
            bool black = plan.Specs.GetValue( segmentIndex, "Race", "African American" );
            bool latino = plan.Specs.GetValue( segmentIndex, "Race", "Hispanic" );
            bool other = plan.Specs.GetValue( segmentIndex, "Race", "Other" );

            demo.Race = new DemographicType<Race>();
            if( ((asian && black && latino && other) == false) && ((asian || black || latino || other) == true) ) {     // take the "else" (set ANY) if all are true, or all are false
                if( asian ) {
                    demo.Race = demo.Race | "ASIAN";
                }
                if( black ) {
                    demo.Race = demo.Race | "BLACK";
                }
                if( latino ) {
                    demo.Race = demo.Race | "LATINO";
                }
                if( other ) {
                    demo.Race = demo.Race | "OTHER";
                }
            }
            else {
                demo.Race = demo.Race | "ANY";
            }

            //age
            bool age1 = plan.Specs.GetValue( segmentIndex, "Age", "Under 18" );
            bool age2 = plan.Specs.GetValue( segmentIndex, "Age", "18-25" );
            bool age3 = plan.Specs.GetValue( segmentIndex, "Age", "26-35" );
            bool age4 = plan.Specs.GetValue( segmentIndex, "Age", "36-45" );
            bool age5 = plan.Specs.GetValue( segmentIndex, "Age", "46-55" );
            bool age6 = plan.Specs.GetValue( segmentIndex, "Age", "56-65" );
            bool age7 = plan.Specs.GetValue( segmentIndex, "Age", "Over 65" );
            demo.Age = new DemographicType<Age>();
            if( ((age1 && age2 && age3 && age4 && age5 && age6 && age7) == false) &&
                                 ((age1 || age2 || age3 || age4 || age5 || age6 || age7) == true) ) {        // take the "else" (set ANY) if all are true, or all are false
                if( age1 ) {
                    demo.Age = demo.Age | "Under18";
                }
                if( age2 ) {
                    demo.Age = demo.Age | "18to25";
                }
                if( age3 ) {
                    demo.Age = demo.Age | "26to35";
                }
                if( age4 ) {
                    demo.Age = demo.Age | "36to45";
                }
                if( age5 ) {
                    demo.Age = demo.Age | "46to55";
                }
                if( age6 ) {
                    demo.Age = demo.Age | "56to65";
                }
                if( age7 ) {
                    demo.Age = demo.Age | "Over65";
                }
            }
            else {
                demo.Age = demo.Age | "ANY";
            }

            //income
            bool income1 = plan.Specs.GetValue( segmentIndex, "Income", "Under 50K" );
            bool income2 = plan.Specs.GetValue( segmentIndex, "Income", "50K-75K" );
            bool income3 = plan.Specs.GetValue( segmentIndex, "Income", "75K-100K" );
            bool income4 = plan.Specs.GetValue( segmentIndex, "Income", "100K-150K" );
            bool income5 = plan.Specs.GetValue( segmentIndex, "Income", "Over 150K" );

            demo.Income = new DemographicType<Income>();
            if( ((income1 && income2 && income3 && income4 && income5) == false) &&
                                          ((income1 || income2 || income3 || income4 || income5) == true) ) {             // take the "else" (set ANY) if all are true, or all are false
                if( income1 ) {
                    demo.Income = demo.Income | "Under50K";
                }
                if( income2 ) {
                    demo.Income = demo.Income | "50Kto75K";
                }
                if( income3 ) {
                    demo.Income = demo.Income | "75Kto100K";
                }
                if( income4 ) {
                    demo.Income = demo.Income | "100Kto150K";
                }
                if( income5 ) {
                    demo.Income = demo.Income | "Over150K";
                }
            }
            else {
                demo.Income = demo.Income | "ANY";
            }

            // kids
            demo.Kids = new DemographicType<ChildStatus>();
            bool kids0 = plan.Specs.GetValue( segmentIndex, "Kids", "None" );
            bool kids1 = plan.Specs.GetValue( segmentIndex, "Kids", "One or more" );
            if( ((kids0 && kids1) == false) && ((kids0 || kids1) == true) ) {       // take the "else" (set ANY) if all are true, or all are false
                if( kids0 ) {
                    demo.Kids = "NO";
                }
                if( kids1 ) {
                    demo.Kids = "YES";
                }
            }
            else {
                demo.Kids = demo.Kids | "ANY";
            }

            return demo;
        }
        #endregion

        public class MediaPulse
        {
            public int StartDay { get; set; }
            public int SpanDays { get; set; }

            public MediaPulse( int start, int span ) {
                this.StartDay = start;
                this.SpanDays = span;
            }
        }

        /// <summary>
        /// Represents the status of a particular sim being run by the server.
        /// </summary>
        [Serializable]
        public class SimStatus
        {
            public Guid SimulationID;
            public int TryCount = 0;
            public double ProgressPercent = 0;
            public bool Done = false;

            public SimStatus() {
                this.SimulationID = new Guid();      // creates an ID of all 0s
            }

            public SimStatus( Guid simID ) {
                this.SimulationID = simID;
            }
        }
    }
}
