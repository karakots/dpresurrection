using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using DpSimQueue;

public partial class _Default : System.Web.UI.Page 
{

    BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

    // string connectString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dpsim;Data Source=chaos";
   // string connectString = "Password=sim2007;Persist Security Info=True;User ID=mrktsim;Initial Catalog=devlsim;Data Source=99.173.30.122";

    DpUpdate simQueue = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if( simQueue == null )
        {
            string connectString = ConfigurationSettings.AppSettings["dpsimConnectionString"];
            simQueue = new DpUpdate( connectString );
        }

        if( IsPostBack )
        {
            return;
        }

        if( Request.RequestType == "POST" )
        {
            string sim_id_string = Request.Headers["sim_id"];

            if( sim_id_string == null )
            {
                sim_id_string = Request.QueryString.Get( "sim_id" );
            }

            Guid sim_id = new Guid( sim_id_string );

            Stream strm = Request.InputStream;

            byte[] first = simQueue.GetRawInput( sim_id );

            int length = 0;
            byte[] tmp = null;

            if( first != null )
            {
                length = first.Length;

                tmp = new byte[Request.ContentLength + length];

                System.Buffer.BlockCopy( first, 0, tmp, 0, first.Length );
            }
            else
            {
                tmp = new byte[Request.ContentLength];
            }

            strm.Read( tmp, length, Request.ContentLength );

            simQueue.UpdateRawInput(sim_id, tmp);
        }
        else
        {
           
            string action = Request.Headers["action"];

            if( action == null )
            {
                action = Request.QueryString.Get( "action" );
            }

            Stream strm = Response.OutputStream;

            switch( action )
            {
                case "create":
                    {
                        string user = Request.Headers["user"];

                        if( user == null )
                        {
                            user = Request.QueryString.Get( "user" );
                        }

                        // create a simulation
                        Guid? guid = simQueue.CreateSimulation( user );

                        if( guid.HasValue )
                        {
                            MemoryStream mem = new MemoryStream();

                            serializer.Serialize( mem, guid.Value );

                            byte[] buffer = mem.GetBuffer();

                            strm.Write( buffer, 0, buffer.Length );
                        }
                    }
                    break;

                case "queue":
                    {
                        string sim_id_string = Request.Headers["sim_id"];

                        if( sim_id_string == null )
                        {
                            sim_id_string = Request.QueryString.Get( "sim_id" );
                        }

                        Guid sim_id = new Guid( sim_id_string );

                        bool done = simQueue.QueueSim( sim_id );

                        byte val = 0;

                        if( done )
                        {
                            val = 1;
                        }

                        strm.WriteByte( val );

                    }
                    break;
              

                case "status":
                    {
                        string sim_id_string = Request.Headers["sim_id"];

                        if( sim_id_string == null )
                        {
                            sim_id_string = Request.QueryString.Get( "sim_id" );
                        }

                        Guid sim_id = new Guid( sim_id_string );

                        bool done = simQueue.SimDone( sim_id );

                        byte val = 0;

                        if( done )
                        {
                            val = 1;
                        }

                        strm.WriteByte( val );

                    }
                    break;

                case "results":
                    {
                        string sim_id_string = Request.Headers["sim_id"];

                        if( sim_id_string == null )
                        {
                            sim_id_string = Request.QueryString.Get( "sim_id" );
                        }

                        Guid sim_id = new Guid( sim_id_string );

                        byte[] res = simQueue.GetRawResults( sim_id );

                        strm.Write( res, 0, res.Length );
                    }

                    break;
                case "movie":
                    {
                        string sim_id_string = Request.Headers["sim_id"];

                        if( sim_id_string == null )
                        {
                            sim_id_string = Request.QueryString.Get( "sim_id" );
                        }

                        Guid sim_id = new Guid( sim_id_string );

                        byte[] res = simQueue.GetData( sim_id );

                        strm.Write( res, 0, res.Length );
                    }

                    break;
                case "progress":
                    {
                        string sim_id_string = Request.Headers["sim_id"];

                        if( sim_id_string == null )
                        {
                            sim_id_string = Request.QueryString.Get( "sim_id" );
                        }

                        Guid sim_id = new Guid( sim_id_string );

                        double progress = simQueue.GetSimProgress( sim_id );

                        byte val = (byte) Math.Ceiling( 255 * progress );

                        strm.WriteByte( val );
                    }
                    break;
                default:
                    {
                        ConnectBox.Text = "Connected to > " + simQueue.Database;
                    }
                    break;
            }

            strm.Flush();
            strm.Close();
           
        }
    }
    protected void CreateSim_Click( object sender, EventArgs e )
    {
        Guid? guid = simQueue.CreateSimulation( "TEST" );

        if( guid.HasValue )
        {
            SimId.Text = guid.Value.ToString();
        }
    }
}
