using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utilities
{
    public class DataLogger
    {
        private static string logFile = "log.txt";
        private static string dateFormat = "dMMMyy:hh:mm:sst";
        private static string logItemFormat = "{1}: {0}";

        /// <summary>
        /// Call this once before logging any messages.
        /// </summary>
        public static void Init() {
            logFile = System.Environment.CurrentDirectory + "\\log.txt";    // be sure use of the file picker can't change this
        }

        /// <summary>
        /// Appends the given message to the master application log file (default = log.txt).  The log file is created if it does not already exist.
        /// </summary>
        /// <param name="message"></param>
        public static void Log( string message ) {

            //!!! this should be done in a background thread to avoid impacting UI response
            LogMessage( logFile, message, DateTime.Now );
        }

        /// <summary>
        /// Actually writes (appends) the given message to the given log file.
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="message"></param>
        /// <param name="timeStamp"></param>
        private static void LogMessage( string logFile, string message, DateTime timeStamp ) {

            string line = String.Format( logItemFormat, message, timeStamp.ToString( dateFormat ) );

            StreamWriter sw = new StreamWriter( logFile, true );

            sw.WriteLine( line );

            sw.Flush();
            sw.Close();
        }
    }
}
