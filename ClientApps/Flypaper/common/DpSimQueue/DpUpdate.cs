using System;
using System.Collections.Generic;
using System.Text;
using SimInterface;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.OleDb;

using Utilities;

namespace DpSimQueue
{
    /// <summary>
    /// class for manageing updates to the simulation table
    /// </summary>
    public class DpUpdate
    {
        public static string Calibrator = "?";
        public string Database
        {
            get
            {
                return create.Connection.DataSource + @"/" + create.Connection.Database;
            }
        }

        // data adapter points to the macpro by default
        private SimQueueTableAdapters.createSim create = new DpSimQueue.SimQueueTableAdapters.createSim();

        public DpUpdate()
            : this( @"./dpsimConnectString.txt" ) {
        }

        public DpUpdate( string connectionFile ) {
            FileInfo fi = new FileInfo( connectionFile );

            if( fi.Exists )
            {

                // read connection string from file
                System.IO.StreamReader reader = new StreamReader( fi.FullName );

                string connectStr = reader.ReadLine();

                create.Connection.ConnectionString = connectStr;
                reader.Close();
            }
            else
            {
                create.Connection.ConnectionString = connectionFile;
            }
        }

        #region Control Panel

        DateTime lastUpdate = DateTime.Now;
        public int  Refresh(SimQueue.sim_queueDataTable sim_queue) {
            lastUpdate = DateTime.Now;
            return create.Fill(sim_queue );
        }

        public int RefreshSinceLast( SimQueue.sim_queueDataTable sim_queue ) {
            // fill with those items

            SimQueue.sim_queueDataTable tmpTable = new SimQueue.sim_queueDataTable();

            lastUpdate = DateTime.Now;

            create.Fill( tmpTable );

            // remove items NOT in new table

            foreach( SimQueue.sim_queueRow sim in sim_queue ) {

                if( tmpTable.FindByid( sim.id ) == null ) {
                    sim.Delete();
                }
            }

            sim_queue.Merge( tmpTable );

            sim_queue.AcceptChanges();

            return 0;
        }

        public int Update( SimQueue.sim_queueDataTable sim_queue ) {
            return create.Update( sim_queue );
        }

        #endregion
        #region Clent Side
     

        public int UpdateInput( Guid simId, SimInput input ) {
            // serialize the input
            MemoryStream str = new MemoryStream();

            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize( str, input );

            byte[] buffer = StreamUtilities.Compress( str.GetBuffer() );

            int num = create.UpdateInput( buffer, simId );

            if( num != 1 ) {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Sets the sim state to 0
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
   
        /// <summary>
        /// Sets sim state back tp -1
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool DequeueSim( Guid simId ) {

            int num = create.DequeueSim( simId );

            if( num == 1 ) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets results when done
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public SimOutput GetResults( Guid simId ) {

            // Get input
            // deserialize
            byte[] bin =  (byte[]) create.GetOutput( simId );

           

            if( bin != null ) 
            {

                //byte[] simRes = StreamUtilities.Decompress( bin );

                MemoryStream str = new MemoryStream( bin );

                BinaryFormatter serializer = new BinaryFormatter();

                return (SimOutput)serializer.Deserialize( str );
            }

            return null;
        }

        #endregion

        #region extra data

        // server side
        public byte[] GetData(Guid simId)
        {
            try
            {
                return create.GetMovie( simId );
            }
            catch( Exception ) { }

            return null;
        }

        public bool DataEmpty( Guid simId )
        {
            int? val = null;

            try
            {
                val = create.MovieQueueEmpty( simId );
            }
            catch( Exception ) { }

            if (val.HasValue)
            {
                return val.Value == 1;
            }

            return  true;
        }

        public void UpdateData(Guid simId, byte[] data)
        {
            if( data == null )
            {
                return;
            }

            // replace movie in queue
            try
            {
                create.UpdateMovie( data, simId );
            }
            catch( Exception ) { }
        }

        #endregion

        #region Server side

        public Guid? CreateSimulation( string userId )
        {

            // create a simulation and return the new id
            SimQueue queue = new SimQueue();
            SimQueue.sim_queueRow sim = queue.sim_queue.Newsim_queueRow();
            sim.id = Guid.NewGuid();
            sim.state = -1; // new sim
            sim.status = "new sim";
            sim.progress = 0;

            sim.run_time = new DateTime( 2000, 1, 1 );

            sim.created = DateTime.Now;

            sim.user_id = userId;
            sim.total_time = 0;

            queue.sim_queue.Rows.Add( sim );

            int num = create.Update( queue );

            if( num != 1 )
            {
                return null;
            }

            return sim.id;
        }

        public byte[] GetRawInput( Guid simId )
        {

            // Get input
            // DO NOT deserialize

            return (byte[])create.GetInput( simId );
        }

        public void UpdateRawInput( Guid simId, byte[] data )
        {
            create.UpdateInput( data, simId );
        }

        public bool QueueSim( Guid simId )
        {

            int num = create.QueueSim( simId );

            if( num == 1 )
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// progress is between 0 and 1
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public double GetSimProgress( Guid simId )
        {
            // default return value is an error condition
            // client should consider the simulation dead
            double rval = -1;

            // TBD - add check for simulation queued but not running
            // need to add for higher load conditions
            // but if simulation does not get queued up within 5 minutes
            // maybe we have other problems
            // SSN

            // check if Sim is stalled
            int? time = create.TimeSinceLastUpdate( simId );

            if( time.HasValue )
            {
                // less then 5 minutes since last update
                if( time.Value < 300 )
                {
                    double? is_rval = (double?)create.GetProgress( simId );

                    if (is_rval.HasValue)
                    {
                        return is_rval.Value;
                    }
                }
            }

            return rval;
        }

        /// <summary>
        /// Check if simulation is done
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool SimDone( Guid simId )
        {

            bool rval = true;

            int? num = (int?)create.SimDone( simId );

            if( num.HasValue )
            {
                rval = num > 0;
            }


            return rval;
        }

        public byte[] GetRawResults( Guid simId )
        {
            // Get input
            // DO NOT deserialize
            return (byte[])create.GetOutput( simId );
        }

        #endregion

        #region Simulation Side
        public SimInput GetInput( Guid simId ) {

            // Get input
            // deserialize
            try
            {

                byte[] bin = (byte[])create.GetInput( simId );


                if( bin != null )
                {

                    byte[] simIn = StreamUtilities.Decompress( bin );

                    MemoryStream str = new MemoryStream( simIn );
                    BinaryFormatter serializer = new BinaryFormatter();
                    return (SimInput)serializer.Deserialize( str );
                }
            }
            catch( Exception e) { }


            return null;
        }

        public Guid? GetSimToRun(bool calibration) {

            try
            {
                if( calibration )
                {
                    return (Guid?)create.CalToRun( Calibrator );
                }

                return (Guid?)create.SimToRun( Calibrator );
            }
            catch( Exception )
            { }
               
            return null;
        }


        /// <summary>
        /// Can only set sim to run
        /// that is in ground state
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool SetSimToRun( Guid simId ) {

            try
            {
                int num = create.SetSimToRun( simId );

                if( num == 1 )
                {
                    return true;
                }
            }
            catch( Exception ) { }

            return false;
        }

        public bool SetSimState( Guid simId, int state ) {

            try
            {
                int num = create.SetState( state, simId );

                if( num == 1 )
                {
                    return true;
                }
            }
            catch( Exception ) { }

            return false;
        }

        public void UpdateProgress( Guid simId, double val ) {

            try
            {
                create.UpdateProgress( val, simId );
            }
            catch( Exception ) { }
        }

        public int UpdateResults( Guid simId, SimOutput output ) {
            // serialize the output

            try
            {
                MemoryStream str = new MemoryStream();

                BinaryFormatter serializer = new BinaryFormatter();

                serializer.Serialize( str, output );

                int num = create.UpdateOutput( str.GetBuffer(), simId );

                if( num != 1 )
                {
                    return -1;
                }
            }
            catch( Exception ) { return -1;  }

            return 0;
        }

        #endregion

    }
}
