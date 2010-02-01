using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using DpSimQueue;
using SimInterface;

[WebService(Namespace = "http://www.adplanit.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class SimService : System.Web.Services.WebService
{
    public static string defaultUserId = "dpservice2718";

    // TEST
    //static DpUpdate dp = new DpUpdate();

    public SimService () {
    }

    public bool CheckID( string userId ) {
        if( userId == defaultUserId ) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Creates new instance of simulation object in db
    /// </summary>
    /// <returns> id of simulation object</returns>
    [WebMethod]
    public Guid CreateSimulation(string adminKey, string userId) {
        // check user id

        if( !CheckID( adminKey ) )
        {
            return new Guid();
        }

        // TEST
        //Guid? id = dp.CreateSimulation(userId);

        //if( id.HasValue ) {
        //    return id.Value;
        //}

        return new Guid();
    }

    /// <summary>
    /// Update the input data to simulation
    /// </summary>
    /// <param name="simId"></param>
    /// <param name="input"></param>
    /// <returns>0 if successful</returns>
    //[WebMethod]
    //public int UpdateInput( Guid simId, SimInput input )
    //{
       
    //   return dp.UpdateInput( simId, input );

    //    return 0;
    //}

    /// <summary>
    /// Queue sim to run
    /// </summary>
    /// <param name="simId"></param>
    /// <returns>True if successfully queued</returns>
    [WebMethod]
    public bool QueueSim(Guid simId )
    {
        //return dp.QueueSim( simId );
        return false;
    }

    /// <summary>
    /// Dequeue sim to run
    /// </summary>
    /// <returns> True if successfully dequeued</returns>
    [WebMethod]
    public bool DequeueSim( Guid simId )
    {
       // return dp.DequeueSim( simId );
        return false;
    }

    /// <summary>
    /// progress is between 0 and 1
    /// </summary>
    /// <param name="simId"></param>
    /// <returns></returns>
    [WebMethod]
    public double GetSimProgress( Guid simId )
    {
       // return dp.GetSimProgress( simId );

        return 1;
    }

    /// <summary>
    /// Check if simulation is done
    /// </summary>
    /// <param name="simId"></param>
    /// <returns>True if done</returns>
    [WebMethod]
    public bool SimDone( Guid simId )
    {
       // return dp.SimDone( simId );
        return true;
    }

    /// <summary>
    /// Returns results of simulation
    /// </summary>
    /// <param name="simId"></param>
    /// <returns></returns>
    //[WebMethod]
    //public SimOutput GetResults(Guid simId ) 
    //{
    //    return dp.GetResults( simId );
    //}
}
