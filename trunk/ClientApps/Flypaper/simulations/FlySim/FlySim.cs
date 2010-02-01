using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimFrame;
using SimInterface;
using HouseholdLibrary;

namespace FlySim
{
    public class FlySim : Simulation
    {
        private static Random rand = new Random();

        private List<Household> households;
        private List<Demographic> segments;
        private List<double> hh_persuasion;
        private List<double> hh_recency;
        private List<bool> hh_awareness;

        private Dictionary<Guid, Media_Info> media_data;
        private Dictionary<int, Dictionary<Guid, MediaComp>> media_input;

        private List<List<Metric>> persuasion;
        private List<List<Metric>> awareness;
        private List<List<Metric>> recency;

        private Dictionary<Demographic, int> segment_ids;
        private Dictionary<int, int> geo_ids;
        private Dictionary<int, int> geo_size;

        public double PersuasionDecay {get;set;}
        public double AwarenessDecay {get;set;}
        public double RecencyDecay {get;set;}

        public FlySim(List<Household> houses, List<Media_Info> media)
        {
            households = houses;
            segments = new List<Demographic>();
            hh_persuasion = new List<double>();
            hh_awareness = new List<bool>();
            hh_recency = new List<double>();

            media_data = new Dictionary<Guid, Media_Info>();
            foreach (Media_Info media_part in media)
            {
                media_data.Add(media_part.Guid, media_part);
            }
            media_input = new Dictionary<int, Dictionary<Guid, MediaComp>>();

            persuasion = new List<List<Metric>>();
            awareness = new List<List<Metric>>();
            recency = new List<List<Metric>>();

            segment_ids = new Dictionary<Demographic, int>();
            geo_ids = new Dictionary<int, int>();
            geo_size = new Dictionary<int, int>();
            int value = 0;
            foreach (Household house in households)
            {
                hh_awareness.Add(false);
                hh_persuasion.Add(0);
                hh_recency.Add(0);

                if (!geo_size.ContainsKey(house.GeoID))
                {
                    geo_size.Add(house.GeoID, 0);
                }
                geo_size[house.GeoID]++;

                if (geo_ids.ContainsKey(house.GeoID))
                {
                    continue;
                }
                geo_ids.Add(house.GeoID, value);
                value++;
            }

            PersuasionDecay = 0.01;
            AwarenessDecay = 0.01;
            RecencyDecay = 0.01;
        }


        protected override Simulation.SimError Reset()
        {
            foreach (List<Metric> geos in persuasion)
            {
                geos.Clear();
            }
            persuasion.Clear();
            foreach (List<Metric> geos in awareness)
            {
                geos.Clear();
            }
            awareness.Clear();
            foreach (List<Metric> geos in recency)
            {
                geos.Clear();
            }
            recency.Clear();

            segments.Clear();
            segment_ids.Clear();

            foreach (Demographic demographic in Input.Demographics)
            {
                segments.Add(demographic);
                segment_ids.Add(demographic, persuasion.Count);
                persuasion.Add(new List<Metric>(geo_ids.Count));
                awareness.Add(new List<Metric>(geo_ids.Count));
                recency.Add(new List<Metric>(geo_ids.Count));
            }

            for (int i = 0; i < persuasion.Count; i++)
            {
                for (int ii = 0; ii < geo_ids.Count; ii++)
                {
                    persuasion[i].Add(new Metric());
                    awareness[i].Add(new Metric());
                    recency[i].Add(new Metric());
                }
            }

            foreach (Dictionary<Guid, MediaComp> media_dict in media_input.Values)
            {
                media_dict.Clear();
            }
            media_input.Clear();

            for (int i = Day; i <= Input.EndDate; i++)
            {
                media_input.Add(i, new Dictionary<Guid, MediaComp>());
                foreach (MediaComp media in Input.Media)
                {
                    if (media.ImpressionsOnDay(i) > 0)
                    {
                        media_input[i].Add(media.Guid, media);
                    }
                }
            }

            for (int i = 0; i < households.Count; i++)
            {
                hh_persuasion[i] = 0;
                hh_recency[i] = 0;
                hh_awareness[i] = false;
            }

            return SimError.NoError;

        }

        protected override Simulation.SimError Step()
        {
            for (int i = 0; i < households.Count; i++)
            {
                if (Day % 7 == 0)
                {
                    for (int j = 0; j < segments.Count; j++)
                    {
                        if (segments[j].Match(households[i]))
                        {
                            persuasion[segment_ids[segments[j]]][geo_ids[households[i].GeoID]].values.Add(hh_persuasion[i] / geo_size[households[i].GeoID]);
                            awareness[segment_ids[segments[j]]][geo_ids[households[i].GeoID]].values.Add(Convert.ToInt32(hh_awareness[i]) / geo_size[households[i].GeoID]);
                            recency[segment_ids[segments[j]]][geo_ids[households[i].GeoID]].values.Add(hh_recency[i] / geo_size[households[i].GeoID]);
                        }
                    }
                }
                for (int j = 0; j < households[i].Media.Count; j++)
                {
                    if (media_input[Day].ContainsKey(households[i].Media[j].Guid))
                    {
                        if (!hh_awareness[i])
                        {
                            double aware_prob = media_data[households[i].Media[j].Guid].Awareness * households[i].Media[j].Rate;
                            if(rand.NextDouble() < aware_prob)
                            {
                                hh_awareness[i] = true;
                            }
                        }
                        if(hh_awareness[i])
                        {
                            hh_persuasion[i] += media_data[households[i].Media[j].Guid].Persuasion * media_input[Day][households[i].Media[j].Guid].Strength * households[i].Media[j].Rate;
                            hh_recency[i] += media_data[households[i].Media[j].Guid].Recency * households[i].Media[j].Rate;
                        }
                    }
                }

                if(rand.NextDouble() < AwarenessDecay)
                {
                    hh_awareness[i] = false;
                }

                if(Math.Abs(hh_persuasion[i]) < PersuasionDecay)
                {
                    hh_persuasion[i] = 0;
                }
                else
                {
                    hh_persuasion[i] -= PersuasionDecay * Math.Sign(hh_persuasion[i]);
                }
                hh_recency[i] *= RecencyDecay;
            }
            return SimError.NoError;
        }

        protected override Simulation.SimError Compute()
        {
            foreach(Demographic segment in segments)
            {
                foreach(int geo_id in geo_ids.Keys)
                {
                    persuasion[segment_ids[segment]][geo_ids[geo_id]].Segment = segment.Name;
                    persuasion[segment_ids[segment]][geo_ids[geo_id]].Type = "Persuasion";
                    persuasion[segment_ids[segment]][geo_ids[geo_id]].Identifier = geo_id;
                    Output.metrics.Add(persuasion[segment_ids[segment]][geo_ids[geo_id]]);

                    awareness[segment_ids[segment]][geo_ids[geo_id]].Segment = segment.Name;
                    awareness[segment_ids[segment]][geo_ids[geo_id]].Type = "Awareness";
                    awareness[segment_ids[segment]][geo_ids[geo_id]].Identifier = geo_id;
                    Output.metrics.Add(awareness[segment_ids[segment]][geo_ids[geo_id]]);

                    recency[segment_ids[segment]][geo_ids[geo_id]].Segment = segment.Name;
                    recency[segment_ids[segment]][geo_ids[geo_id]].Type = "Recency";
                    recency[segment_ids[segment]][geo_ids[geo_id]].Identifier = geo_id;
                    Output.metrics.Add(recency[segment_ids[segment]][geo_ids[geo_id]]);
                }
            }

            return SimError.NoError;
        }
    }
}
