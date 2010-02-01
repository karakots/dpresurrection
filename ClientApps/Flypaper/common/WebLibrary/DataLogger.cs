using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.IO;

namespace WebLibrary
{
    public class DataLogger
    {
        public static bool DataloggingEnabled = true;

        public static string DefaultLogfilePath = "C:\\FlypaperData\\log.txt";
        private static string logfilePath = DefaultLogfilePath;

        public static void Init(){

            DateTime today = DateTime.Now;
            string logfileName = String.Format( "\\log-{0}-{1:d2}.txt", today.Year, today.Month );

            logfilePath = ConfigurationSettings.AppSettings[ "AdPlanit.UserPlanStorageRoot" ] + logfileName;
            Log( "INFO", "Application Initialized" );
        }

        /// <summary>
        /// Logs a visit to a page
        /// </summary>
        /// <param name="pageID"></param>
        /// <param name="userName"></param>
        /// <param name="planName"></param>
        public static void LogPageVisit( string pageID, string userName, string planName ) {
            Log( "VIEW", userName, pageID, planName );
        }

        /// <summary>
        /// Logs a keyword and a set of parameters using CSV format
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="itemParams"></param>
        public static void Log( string keyword, string param1 ) {
            List<string> itemParams = new List<string>();
            itemParams.Add( param1 );
            Log( keyword, itemParams );
        }

        /// <summary>
        /// Logs a keyword and a set of parameters using CSV format
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="itemParams"></param>
        public static void Log( string keyword, string param1, string param2 ) {
            List<string> itemParams = new List<string>();
            itemParams.Add( param1 );
            itemParams.Add( param2 );
            Log( keyword, itemParams );
        }

        /// <summary>
        /// Logs a keyword and a set of parameters using CSV format
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="itemParams"></param>
        public static void Log( string keyword, string param1, string param2, string param3 ) {
            List<string> itemParams = new List<string>();
            itemParams.Add( param1 );
            itemParams.Add( param2 );
            itemParams.Add( param3 );
            Log( keyword, itemParams );
        }

        /// <summary>
        /// Logs a keyword and a set of parameters using CSV format
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="itemParams"></param>
        public static void Log( string keyword, string param1, string param2, string param3, string param4 ) {
            List<string> itemParams = new List<string>();
            itemParams.Add( param1 );
            itemParams.Add( param2 );
            itemParams.Add( param3 );
            itemParams.Add( param4 );
            Log( keyword, itemParams );
        }

        /// <summary>
        /// Logs a keyword and a set of parameters using CSV format
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="itemParams"></param>
        public static void Log( string keyword, List<string> itemParams ) {
            string csvList = "";
            for( int i = 0; i < itemParams.Count; i++ ) {
                string s = itemParams[ i ].Replace( "\"", "" );       // remove double quotes
                if( s.IndexOf( "," ) != -1 ) {
                    s = "\"" + s + "\"";
                }
                csvList += s;
                if( i != itemParams.Count - 1 ) {
                    csvList += ",";
                }
            }
            DoLog( "{0} {1}", keyword, csvList );
        }

        /// <summary>
        /// Logs a timestamped message to the master log file
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DoLog( string format, params object[] args ) {
            if( DataloggingEnabled == false ) {
                return;
            }

            try {
                string timestamp = DateTime.Now.ToString( "hh:mm:ss.fff M/d/yy" );
                string data = String.Format( format, args );
                string line = String.Format( "{0} - {1}", timestamp, data );
                StreamWriter sw = new StreamWriter( logfilePath, true );
                sw.WriteLine( line );
                sw.Flush();
                sw.Close();
            }
            catch( Exception ) {
                // oh well...
            }
        }
    }
}
