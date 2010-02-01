using System;
using System.Collections.Generic;
using System.Text;
using MediaLibrary;

namespace SimInterface
{
    /// <summary>
    /// Interface for the input into the simulation
    /// </summary>
    [Serializable]
    public class SimOutput
    {
        // note: Simulaitons begin on day 0
        public List<Metric> metrics = new List<Metric>();

        public List<FinalPersuasion> final_persuasion = new List<FinalPersuasion>();

        //public List<AgentLog> agent_microscope = new List<AgentLog>();

        public SimOutput() { }
    }

    [Serializable]
    public class Metric
    {
        public int Identifier = 0;

        public string Type = "";

        public string Segment = "";

        public int Span = 7;

        public List<double> values = new List<double>();

        public Metric() { }
    }

    [Serializable]
    public class FinalPersuasion
    {
        public double persuasion = 0.0;
        public int num_agents = 0;
    }
}

