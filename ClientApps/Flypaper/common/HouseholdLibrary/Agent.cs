using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseholdLibrary
{
    [Serializable]
    public class Agent
    {
        private Household house;

        [NonSerialized]
        Dictionary<Guid, AgentMedia> my_media = new Dictionary<Guid,AgentMedia>();

        public Household House
        {
            get
            {
                return house;
            }
        }

        public Agent(Household house)
        {
            this.house = house;
        }

        public void AddMedia(MediaVehicle media, Double rate)
        {
            if (my_media == null)
            {
                my_media = new Dictionary<Guid, AgentMedia>();
            }
            my_media.Add(media.Guid, new AgentMedia(media, rate));
        }

        public double GetRate(MediaVehicle media)
        {
            if (my_media.ContainsKey(media.Guid))
            {
                return my_media[media.Guid].Rate;
            }
            return 0.0;
        }

        public double GetRate(Guid guid)
        {
            if (my_media.ContainsKey(guid))
            {
                return my_media[guid].Rate;
            }
            return 0.0;
        }

        public bool HasMedia(Guid guid)
        {
            if (my_media == null || !my_media.ContainsKey(guid))
            {
                return false;
            }

            return true;
        }

        public MediaVehicle GetMedia(Guid guid)
        {
            if (my_media.ContainsKey(guid))
            {
                return my_media[guid].Media;
            }
            return null;
        }

        public void ClearMedia()
        {
            if (my_media != null)
            {
                my_media.Clear();
            }
        }

        public void SetRate(Guid guid, double rate)
        {
            if (my_media != null)
            {
                my_media[guid].Rate = rate;
            }
        }

        [Serializable]
        private class AgentMedia
        {
            private MediaVehicle media;
            private Double rate;

            public MediaVehicle Media { get { return media; } }
            public Double Rate { get { return rate; } set { rate = value; } }

            public AgentMedia(MediaVehicle media, Double rate)
            {
                this.media = media;
                this.rate = rate;
            }
        }
    }

    public class AgentData
    {
        private static Random rand = new Random();
        public static double PersuasionDecay = 0.02;
        public static double AwareDecay = 0.02;
        public static double RecencyDecay = 0.1;
        public static int ImpressionLength = 30;
        public static int Max_Impressions = 4;

        public Agent agent = null;
        public List<Demographic> Demographics = new List<Demographic>();
        public Dictionary<Guid, MediaVehicle> active_media = new Dictionary<Guid, MediaVehicle>();
        public double shopProb = 1;
        public double Persuasion;
        public bool Aware;
        public double Recency;

        public int demoIndex = -1;

        private List<int> Impressions;

        public AgentData(Agent agentIn)
        {
            Persuasion = 0;
            Aware = false;
            Recency = 0;

            agent = agentIn;
        }

        public void Reset()
        {
            Persuasion = 0;
            Aware = false;
            Recency = 0;
            Demographics.Clear();
            Impressions = new List<int>();
            for (int i = 0; i < AgentData.Max_Impressions; i++ )
            {
                Impressions.Add(0);
            }
        }

        public bool AddDemographic(Demographic demo)
        {
            if (demo.Match(agent.House))
            {
                Demographics.Add(demo);

                return true;
            }

            return false;
        }

        public void AddImpression()
        {
            Impressions[min_impression_index()] = ImpressionLength;
        }

        public int NumImpressions()
        {
            int num = 0;
            for (int i = 0; i < Impressions.Count; i++)
            {
                if (Impressions[i] > 0)
                {
                    num++;
                }
            }

            return num;
        }

        public void Decay()
        {
            for (int i = 0; i < Impressions.Count; i++)
            {
                Impressions[i]--;
            }

            if (rand.NextDouble() < AwareDecay)
            {
                Aware = false;
                Persuasion = 0;
                Recency = 0;
            }
            else
            {
                Persuasion *= (1 - PersuasionDecay);
                Recency *= (1 - RecencyDecay);
            }
        }

        private int min_impression_index()
        {
            int min_index = 0;
            for (int i = 1; i < Impressions.Count; i++)
            {
                if (Impressions[i] < Impressions[min_index])
                {
                    min_index = i;
                }
            }
            return min_index;
        }
    }
}
