using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;

using Utilities;
using SimInterface;

namespace WebLibrary
{
    /// <summary>
    /// Interfaces to the web service for simulations
    /// </summary>
    public class SimClient
    {
        BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        static string simUrl = null;

        public SimClient() 
        {
            if( simUrl == null )
            {
                simUrl = System.Configuration.ConfigurationManager.AppSettings["Adplanit.SimulationServer"];
            }
        }

        public Guid? CreateSimulation( string userId )
        {
            string ignore = null;
            return CreateSimulation( userId, out ignore );
        }

        public Guid? CreateSimulation( string userId, out string error )
        {
            error = null;

            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );
            HttpWReq.Method = "GET";
            HttpWReq.Headers.Add( "action", "create" );
            HttpWReq.Headers.Add( "user", userId );


            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                System.IO.Stream stream = HttpWResp.GetResponseStream();

                Guid guid = (Guid)serializer.Deserialize( stream );

                return guid;
            }
            catch( Exception e)
            {
                error = e.Message;
            }

            return null;
        }

        public int UpdateInput( Guid simId, SimInput input )
        {
            // serialize the input
            MemoryStream str = new MemoryStream();

            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize( str, input );

            byte[] buffer = StreamUtilities.Compress( str.GetBuffer() );

            // send buffer to simulation web site}

            int index = 0;
            int length = 1000000;
            while( index < buffer.Length )
            {
                HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

                HttpWReq.Method = "POST";

                HttpWReq.Headers.Add( "action", "update" );
                HttpWReq.Headers.Add( "sim_id", simId.ToString() );

                if( index + length > buffer.Length )
                {
                    length = buffer.Length - index;
                }

                HttpWReq.ContentLength = length;

                Stream strm = HttpWReq.GetRequestStream();

                strm.Write( buffer, index, length );

                strm.Flush();
                strm.Close();

                index += length;

                // now post
                try
                {
                    HttpWebResponse res = (HttpWebResponse)HttpWReq.GetResponse();
                }
                catch( Exception )
                {
                    return -1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Tell Simulation web site to run simulation
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool QueueSim( Guid simId )
        {
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

            HttpWReq.Method = "GET";

            HttpWReq.Headers.Add( "action", "queue" );
            HttpWReq.Headers.Add( "sim_id", simId.ToString() );

            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                System.IO.Stream stream = HttpWResp.GetResponseStream();

                int val = stream.ReadByte();

                HttpWResp.Close();

                return (val == 1);
            }
            catch( Exception e )
            {
            }

            return false;
        }

        /// <summary>
        /// Gets results when done
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public SimOutput GetResults( Guid simId )
        {
            SimOutput rval = null;

            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

            HttpWReq.Method = "GET";
            HttpWReq.Headers.Add( "action", "results" );
            HttpWReq.Headers.Add( "sim_id", simId.ToString() );

            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

                System.IO.Stream stream = HttpWResp.GetResponseStream();

                rval = (SimOutput)serializer.Deserialize( stream );

                stream.Close();
            }
            catch( Exception e )
            {
            }

            return rval;
        }

        /// <summary>
        /// Get movie
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public byte[] GetData( Guid simId )
        {
            byte[] buffer = new byte[4096];

            byte[] rval = null;

            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

            HttpWReq.Method = "GET";
            HttpWReq.Headers.Add( "action", "movie" );
            HttpWReq.Headers.Add( "sim_id", simId.ToString() );

            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

                using( System.IO.Stream stream = HttpWResp.GetResponseStream() )
                {
                    using( MemoryStream memoryStream = new MemoryStream() )
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read( buffer, 0, buffer.Length );
                            memoryStream.Write( buffer, 0, count );

                        } while( count != 0 );

                        rval = Utilities.StreamUtilities.Decompress(memoryStream.GetBuffer());

                        memoryStream.Close();
                    }

                    stream.Close();
                }
            }
            catch( Exception)
            {
            }

            return rval;
        }

        /// <summary>
        /// progress is between 0 and 1
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public double GetSimProgress( Guid simId )
        {
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

            HttpWReq.Method = "GET"; 
            HttpWReq.Headers.Add( "action", "progress" );
            HttpWReq.Headers.Add( "sim_id", simId.ToString() );

            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                System.IO.Stream stream = HttpWResp.GetResponseStream();

                int val = stream.ReadByte();

                HttpWResp.Close();

                return ((double)val) / 255;
            }
            catch( Exception e )
            {
            }

            return 0;
        }

        /// <summary>
        /// Check if simulation is done
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool SimDone( Guid simId )
        {
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create( simUrl );

            HttpWReq.Method = "GET";
            HttpWReq.Headers.Add( "action", "status" );
            HttpWReq.Headers.Add( "sim_id", simId.ToString() );
            try
            {
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                System.IO.Stream stream = HttpWResp.GetResponseStream();

                int val = stream.ReadByte();

                HttpWResp.Close();

                return (val == 1);
            }
            catch( Exception e )
            {
            }

            return false;
        }
    }
}
