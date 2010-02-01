using System;
using System.Collections.Generic;
using System.Text;
using HouseholdLibrary;
using GeoLibrary;
using DemographicLibrary;
using MediaLibrary;

namespace SimInterface
{
    /// <summary>
    /// Interface for the input into the simulation
    /// </summary>
    [Serializable]
    public class SimInput
    {
        public Dictionary<int, AdOption> calOptions = null;

        // note: Simulaitons begin on day 0
        public int EndDate = 0;

        public List<MediaComp> Media = new List<MediaComp>();

        public List<Demographic> Demographics = new List<Demographic>();
        

        /// <summary>
        /// Number of days between purchases
        /// </summary>
        public int PurchaseInterval = 30;

        public double ConsiderationInterval = 0.5;

        public string option = null;

        public int microscope_size = 100;

        public double mu = 1;

        public double initial_awareness = 0.0;

        public List<InitialPersuasion> initial_persuasion = new List<InitialPersuasion>();

        public SimInput() { }
    }

    

    [Serializable]
    public class InitialPersuasion
    {
        public double persuasion = 0.0;
        public double percent_aware = 0.0;
    }
}
