using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NitroReader.Library
{
    /// <summary>
    /// This classe allows a program to conveniently maintain a log file that always is added to.
    /// </summary>
    class DataLogger
    {
        private static string logFile = "log.txt";

        /// <summary>
        /// Make sure the log file location stays fixed by making it absolute (in the current directory).
        /// </summary>
        /// <param name="applicaiton"></param>
        public static void Init() {
            logFile = System.Environment.CurrentDirectory + "\\" + logFile;
        }

        /// <summary>
        /// Records a message in the log file.  Timestamp and program identificatiion info is added automatically.
        /// </summary>
        /// <param name="message"></param>
        public static void Log( string message ) {
            string header = String.Format( "{0} {1} {2}",
                System.Windows.Forms.Application.ProductName,
                System.Windows.Forms.Application.ProductVersion.ToString(),
                DateTime.Now.ToString( "h:mm:ss dd-MMM-yy" ) );
            string fullmsg = header + ": " + message;

            FileStream fs = new FileStream( logFile, FileMode.Append, FileAccess.Write );
            StreamWriter sw = new StreamWriter( fs );
            sw.WriteLine( fullmsg );
            sw.WriteLine();
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
