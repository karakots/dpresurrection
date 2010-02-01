using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using HouseholdLibrary;
using MediaLibrary;
using SimInterface;

namespace WebLibrary
{
    /// <summary>
    /// Summary description for FlightingInfo
    /// </summary>
    [Serializable]
    public class FlightingInfo
    {
        //public const int ADVERTISING_HOURS_PER_DAY = 18;

        //public string AdOption { set; get; }
        //public string AdOptionIDStr { set; get; }       // !!! for compatibility with old-style plans only
        //public int AdOptionID { set; get; }

        //public int Span { set; get; }

        ///// <summary>
        ///// Duration of the overall plan using this flighting.
        ///// </summary>
        //public int TotalPlanDays { set; get; }

        ///// <summary>
        ///// Duration of an overall flighting cycle.  Value should not be greater than TotalPlanDays.
        ///// </summary>
        //public int CycleDays { set; get; }

        ///// <summary>
        ///// Use continuous flightng. If true, all other settings can be ignored.
        ///// </summary>
        //public bool Continuous { set; get; }

        ///// <summary>
        ///// Duration of each active individual media pulse.
        ///// </summary>
        //public double PulseDays { set; get; }

        ///// <summary>
        ///// Duration of the "silent" gap between each active individual media pulse.
        ///// </summary>
        //public double DaysBetweenPulses { set; get; }

        ///// <summary>
        ///// Number of pulses in each overall flighting cycle.
        ///// </summary>
        //public int PulsesPerCycle { set; get; }

        ///// <summary>
        ///// Number of actual individual ads in each pulse (can be more than one per day).
        ///// </summary>
        //public int AdsPerPulse { set; get; }

        ///// <summary>
        ///// Delay after the plan start of the first pulse.
        ///// </summary>
        //public double PulseDelayDays { set; get; }

        ///// <summary>
        ///// The time in tics that the flighting was last edited
        ///// </summary>
        //public long ModificationTimestamp { set; get; }

        ///// <summary>
        ///// Gets the number of pulse cycles in the overall plan.
        ///// </summary>
        //public double CyclesPerPlan
        //{
        //    get { return TotalPlanDays / CycleDays; }
        //}

        ///// <summary>
        ///// Score assigned by the rules-based selection process
        ///// </summary>
        //public double Score { set; get; }

        ///// <summary>
        ///// List of dates/times to run actual media spots.
        ///// </summary>
        //public List<KeyValuePair<DateTime, MediaComp>> SpotDates { set; get; }

        ///// <summary>
        ///// The number of media spots that are actually slated in the plan for this item.
        ///// </summary>
        //public int ActualSpotCount
        //{
        //    get { return SpotDates.Count; }
        //}

        ///// <summary>
        ///// Adds a spot to the flighting's SpotDates list
        ///// </summary>
        ///// <param name="spotDate"></param>
        //public void AddSpot(DateTime spotDate, MediaItem item)
        //{
        //    MediaComp media_comp = new MediaComp();
        //    media_comp.Guid = item.vehicle_id;
        //    media_comp.ad_option = item.ad_option_id;
        //    media_comp.Span = item.Span;
        //    media_comp.target_demo = item.Demographics;
        //    media_comp.target_regions = item.Regions;
        //    media_comp.Impressions = item.NumImpressions;
        //    media_comp.region_fuzz_factor = item.TargetingLevel / 100;
        //    media_comp.demo_fuzz_factor = item.TargetingLevel / 100;

        //    SpotDates.Add(new KeyValuePair<DateTime, MediaComp>(spotDate, media_comp));
        //}

        ///// <summary>
        ///// Creates a new FlightingInfo object.
        ///// </summary>
        //public FlightingInfo()
        //{
        //    this.Score = -1;
        //    this.Reasons = new List<MediaRuleReason>();
        //    this.SpotDates = new List<KeyValuePair<DateTime, MediaComp>>();
        //    this.Continuous = true;

        //    this.CycleDays = -1;
        //    this.PulseDelayDays = 0;
        //    this.PulseDays = 0;
        //    this.DaysBetweenPulses = 0;
        //    this.PulsesPerCycle = 1;
        //    this.AdsPerPulse = 1;
        //    this.Span = -1;

        //    this.TotalPlanDays = -1;
        //    this.ModificationTimestamp = DateTime.Now.Ticks;
        //}

        ///// <summary>
        ///// Creates a default (continuous) flighting for the given plan duration.
        ///// </summary>
        ///// <param name="totalPlanDays"></param>
        //public FlightingInfo(int totalPlanDays)
        //    : this()
        //{
        //    this.TotalPlanDays = totalPlanDays;
        //    this.CycleDays = totalPlanDays;
        //    this.PulseDays = totalPlanDays;

        //    this.dutyDesc = "Continuous";
        //    this.freqDesc = "";
        //}

        ///// <summary>
        ///// Creates a flighting info that is a copy of the given item
        ///// </summary>
        ///// <param name="infoToCopy"></param>
        //public FlightingInfo(FlightingInfo infoToCopy)
        //{
        //    this.Score = infoToCopy.Score;
        //    this.freqDesc = infoToCopy.freqDesc;
        //    this.dutyDesc = infoToCopy.dutyDesc;

        //    this.CycleDays = infoToCopy.CycleDays;
        //    this.PulseDelayDays = infoToCopy.PulseDelayDays;
        //    this.PulseDays = infoToCopy.PulseDays;
        //    this.DaysBetweenPulses = infoToCopy.DaysBetweenPulses;
        //    this.PulsesPerCycle = infoToCopy.PulsesPerCycle;
        //    this.AdsPerPulse = infoToCopy.AdsPerPulse;

        //    this.TotalPlanDays = infoToCopy.TotalPlanDays;

        //    this.Reasons = new List<MediaRuleReason>();
        //    this.ModificationTimestamp = infoToCopy.ModificationTimestamp;

        //    this.SpotDates = new List<DateTime>();
        //    foreach (DateTime spot in infoToCopy.SpotDates)
        //    {
        //        this.SpotDates.Add(spot);
        //    }
        //}
    }
}
