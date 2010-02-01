using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DemographicLibrary;

namespace GeoLibrary
{
    ///// <summary>
    ///// represents the geogrpahic hierarchy
    ///// Top is the world
    ///// </summary>

    //[Serializable]
    //public class GeoRegion
    //{
    //    static public GeoRegion topGeo = null;
    //    static public GeoRegion TopGeo
    //    {
    //        get
    //        {
    //            return topGeo;
    //        }
    //    }

    //    #region Initialize

    //    static public void NewGeoRegion(string name, GeoRegion.RegionType type)
    //    {

    //        topGeo = new GeoRegion(name, GeoRegion.RegionType.Country);
    //    }

    //    // create new geograph from TopGeo
    //    static public void Initialize()
    //    {
    //        if (topGeo != null)
    //        {
    //            geoSize.Clear();
    //            init(topGeo);
    //        }
    //    }

    //    public static void BuildLibrary()
    //    {
    //        TopGeo.AddToLibrary();
    //    }

    //    private static void init(GeoRegion reg)
    //    {
    //        if (reg.Type <= GeoRegion.RegionType.DMA)
    //        {
    //            if (!geoSize.ContainsKey(reg.Name))
    //            {
    //                geoSize.Add(reg.Name, 0);
    //            }

    //            if (!primVal.ContainsKey(reg.Name))
    //            {
    //                primVal.Add(reg.Name, new Dictionary<string, double>());

    //                foreach (Demographic dem in PrimitiveTypes)
    //                {
    //                    primVal[reg.Name].Add(dem.Name, 0);
    //                }
    //            }

    //            if (reg.SubRegions != null)
    //            {
    //                foreach (GeoRegion sub in reg.SubRegions)
    //                {
    //                    init(sub);
    //                }
    //            }
    //        }
    //    }
    //    #endregion

 

        

    //    private static Dictionary<int, GeoInfo> geoInfoData = new Dictionary<int, GeoInfo>();

        
    //    #region FILE IO

       
    //    private const string stateToDma = @"\stateToDma.csv";

    //    public static string ReadFromFile(string dirName)
    //    {
    //        string error = GeoRegion.ReadFromFile(dirName, out topGeo);

    //        if (error != null)
    //        {
    //            return error;
    //        }

    //        // read in the states

    //        // create file that has the state - dma relationship
    //        try
    //        {
    //            StreamReader stateDmaReader = new StreamReader(dirName + stateToDma);
    //            Dictionary<string, List<string>> map = GeoInfo.CreateNewMap("StateDma");
    //            while (stateDmaReader.EndOfStream)
    //            {
    //                string line = stateDmaReader.ReadLine();

    //                string[] vals = line.Split(',');

    //                string state = vals[0];
    //                string dma = vals[1];

    //                state = state.Trim();
    //                dma = dma.Trim();

    //                // check
    //                if (topGeo[dma] == null)
    //                {
    //                    throw new Exception("Mismatch DMA Name");
    //                }

    //                List<string> dmas = null;
    //                if (!map.ContainsKey(state))
    //                {
    //                    dmas = new List<string>();
    //                    map.Add(state, dmas);
    //                }
    //                else
    //                {
    //                    dmas = map[state];
    //                }

    //                dmas.Add(dma);
    //            }
    //            stateDmaReader.Close();
    //        }
    //        catch (Exception) { }


    //        return null;
    //    }

    //    public static string WriteToFile(string dirName)
    //    {
    //        string error = GeoRegion.WriteToFile(dirName, topGeo);

    //        if (error != null)
    //        {
    //            return error;
    //        }
    //    }





    //    #endregion


    //    #region main calculations

    //    public static double DemographicSize(string region, string baseType)
    //    {
    //        return geoSize[region] * primVal[region][baseType];
    //    }

    //    /// <summary>
    //    /// factors a geographic into primitive demgraphics
    //    /// </summary>
    //    /// <param name="dem"></param>
    //    /// <returns></returns>
    //    public static Dictionary<string, Dictionary<string, double>> Factor(Demographic dem)
    //    {
    //        return factor(dem);
    //    }

    //    private static Dictionary<string, Dictionary<string, double>> factor(Demographic dem)
    //    {
    //        string regionName;
    //        if (dem.Region == null)
    //        {
    //            regionName = topGeo.Name;
    //        }
    //        else if (topGeo != null)
    //        {
    //            regionName = dem.Region.Name;
    //        }
    //        else
    //        {
    //            return null;
    //        }

    //        Dictionary<string, Dictionary<string, double>> rval = new Dictionary<string, Dictionary<string, double>>();

    //        rval.Add(regionName, new Dictionary<string, double>());
    //        foreach (string baseType in primVal[regionName].Keys)
    //        {
    //            Demographic primDem = new Demographic(baseType);
    //            primDem.Region = dem.Region;

    //            if (Demographic.Intersect(primDem, dem))
    //            {
    //                rval[regionName].Add(baseType, geoSize[regionName] * dem.Info.Reach);
    //            }
    //            else
    //            {
    //                rval[regionName].Add(baseType, 0);
    //            }
    //        }

    //        return rval;
    //    }

    //    public static double DemoSize(Demographic dem)
    //    {
    //        return demoSize(dem);
    //    }

    //    public static GeoInfo GetGeoInfo(int geo_id)
    //    {
    //        GeoInfo rval = null;

    //        try
    //        {
    //            rval = geoInfoData[geo_id];
    //        }
    //        catch
    //        {
    //            rval = null;
    //        }

    //        if (rval == null)
    //        {
    //            rval = topGeo[geo_id];

    //            if (rval != null)
    //            {
    //                geoInfoData.Add(geo_id, rval);
    //            }
    //        }

    //        return rval;
    //    }

    //    private static double demoSize(Demographic dem)
    //    {
    //        string regionName;
    //        if (dem.Region == null)
    //        {
    //            regionName = topGeo.Name;
    //        }
    //        else if (topGeo != null)
    //        {
    //            regionName = dem.Region.Name;
    //        }
    //        else
    //        {
    //            return 0;
    //        }

    //        double size = 0;
    //        foreach (string baseType in primVal[regionName].Keys)
    //        {
    //            Demographic tmp = new Demographic(baseType);

    //            tmp.Region = null;

    //            if (Demographic.Intersect(tmp, dem))
    //            {
    //                size += primVal[regionName][baseType];
    //            }
    //        }

    //        return size;
    //    }
    //    #endregion

    //    #region Accounting of households

    //    public static void ComputeSizes(List<Agent> agents)
    //    {
    //        PrimitiveSizes.Clear();
    //        PrimitiveSizes.Capacity = PrimitiveTypes.Count;
    //        foreach (Agent agent in agents)
    //        {
    //            for (int i = 0; i < PrimitiveTypes.Count; i++)
    //            {
    //                if (PrimitiveTypes[i].Match(agent.House))
    //                {
    //                    PrimitiveSizes[i]++;
    //                }
    //            }
    //        }

    //        NumHouseholds = agents.Count;
    //    }

    //    // add one to each dma
    //    public static void AddHousehold(Household hh)
    //    {
    //        // find dma that contains this houshold
    //        GeoInfo info = topGeo[hh.GeoID];

    //        if (info != null)
    //        {
    //            GeoRegion dma = info.GetDMA();

    //            if (dma != null)
    //            {
    //                foreach (Demographic dem in PrimitiveTypes)
    //                {
    //                    if (dem.Match(hh))
    //                    {
    //                        primVal[dma.Name][dem.Name] += 1;
    //                    }
    //                }

    //                geoSize[dma.Name] = geoSize[dma.Name] + 1;
    //            }
    //        }
    //    }

    //    public static void Normalize()
    //    {
    //        sumHouseholds(topGeo);

    //        double norm = geoSize[topGeo.Name];

    //        if (norm > 0)
    //        {
    //            normalize(topGeo, 1 / norm);
    //        }
    //    }


    //    /// <summary>
    //    /// Add up all the households that exist at the DMA level
    //    /// </summary>
    //    private static void sumHouseholds(GeoRegion reg)
    //    {
    //        // do not need to sum at the DMA leve and below
    //        if (reg.Type >= GeoRegion.RegionType.DMA)
    //        {
    //            return;
    //        }

    //        geoSize[reg.Name] = 0;
    //        foreach (GeoRegion sub in reg.SubRegions)
    //        {
    //            sumHouseholds(sub);

    //            geoSize[reg.Name] += geoSize[sub.Name];

    //            // sum the demographics
    //            foreach (Demographic dem in PrimitiveTypes)
    //            {
    //                primVal[reg.Name][dem.Name] += primVal[sub.Name][dem.Name];
    //            }
    //        }
    //    }

    //    private static void normalize(GeoRegion reg, double scale)
    //    {
    //        if (reg.Type > GeoRegion.RegionType.DMA)
    //        {
    //            return;
    //        }

    //        geoSize[reg.Name] = scale * geoSize[reg.Name];

    //        // normalize the demographics
    //        foreach (Demographic dem in PrimitiveTypes)
    //        {
    //            primVal[reg.Name][dem.Name] = scale * primVal[reg.Name][dem.Name];
    //        }

    //        if (reg.Type < GeoRegion.RegionType.DMA)
    //        {
    //            foreach (GeoRegion sub in reg.SubRegions)
    //            {
    //                normalize(sub, scale);
    //            }
    //        }
    //    }

    //    #endregion
    //}
}
