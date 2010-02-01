using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Utilities
{
    public class HelpManager
    {
        private const string primaryAliasesFile = "Resources\\Help\\Reports\\Topic Aliases Report.htm";
        private const string secondaryAliasesFile = "..\\..\\..\\MrktSimClient\\Resources\\Help\\Reports\\Topic Aliases Report.htm";
        private const string primaryRedirectPath = "Resources\\Help\\Redirect\\";
        private const string secondaryRedirectPath = "..\\..\\..\\MrktSimClient\\Resources\\Help\\Redirect\\";
        private const string relativeHelpPath = "..\\";       // relative to redir path
        private const string primaryDefaultPath = "Resources\\Help\\Output\\TOC.html";
        private const string secondaryDefaultPath = "..\\..\\..\\MrktSimClient\\Resources\\Help\\Output\\TOC.html";


        private const string redirFileBody = "<html><body onLoad='location.href=\"{0}\"'>Redirecting to <a href=\"{0}\">Help Page</a>...</body></html>";

        private static Hashtable tagPaths = null;

        /// <summary>
        /// Show the browser with the help page corresponding to the given help tag.
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="helpTag"></param>
        public static void ShowHelp( object requester, string helpTag ) {
            if( tagPaths == null ){
                // load the help aliases info the first time help is requested
                Init();
            }

            if( helpTag == null ){
                helpTag = "null";
            }

            string helpPath = (string)tagPaths[ helpTag ];
            if( helpPath == null ){
                helpPath = (string)tagPaths[ "default" ];   // default to table of contents or index
                if( helpPath == null ) {
                    string msg = String.Format( "\r\n       ERROR: Unable to find help for this topic.       \r\n\r\n      Help topic tag: {0}\r\n", helpTag );
                    MessageBox.Show( msg, "Help Error" );
                    return;
                }
           }

           // launch the browser on the selected help file
           System.Diagnostics.Process.Start( helpPath );
       }

        /// <summary>
        /// Loads the actual HTML help URLs that correspond to the Help Tag strings.
        /// </summary>
       /// <remarks>The relationship if tags to URLs is based on the hyperlinks in the aliases file.  A set of redirect files is created,
       /// one per help tag, since directly opening a file like file1.html#tagname always ignores the #tagname portion!  To avoid
       /// requiring a local web sever or access to a central host, we use these redirect files.</remarks>
        private static void Init(){
            tagPaths = new Hashtable();

            string aliasesFile = Application.StartupPath + "\\" + primaryAliasesFile;
            //string helpFilePath = "http:\\\\localhost\\Help\\";
            string redirectFilePath = Application.StartupPath + "\\" + primaryRedirectPath;
            string defaultFilePath = Application.StartupPath + "\\" + primaryDefaultPath;

            if( File.Exists( aliasesFile ) == false ) {
                aliasesFile = Application.StartupPath + "\\" + secondaryAliasesFile;
                redirectFilePath = Application.StartupPath + "\\" + secondaryRedirectPath;
                defaultFilePath = Application.StartupPath + "\\" + secondaryDefaultPath;
            }
            if( File.Exists( aliasesFile ) == false ) {
                string msg = String.Format( "\r\n       ERROR: Unable to find help aliases index.       \r\n\r\n      Aliases index file: {0}\r\n", primaryAliasesFile );
                MessageBox.Show( msg, "Help Error" );
                return;
            }

            ClearRedirectFolder( redirectFilePath );

            FileStream fs = new FileStream( aliasesFile, FileMode.Open, FileAccess.Read );
            StreamReader sr = new StreamReader( fs );

            // read the lines from the alias (html) file
            string line = null;
            while( (line = sr.ReadLine()) != null ) {

                string ll = line.ToLower();
                if( ll.IndexOf( "<a " ) == -1 ) {
                    continue;   //ignore non-hyperlink lines
                }

                // parse the tag line - assumptions: one link per line; no extra whitespace
                string tag1 = "href=\"";
                int i1 = ll.IndexOf( tag1 ) + tag1.Length;
                int i2 = ll.IndexOf( "\"", i1 );
                int k1 = ll.IndexOf( ">", i1 ) + 1;
                int k2 = ll.IndexOf( "<", k1 + 1 );

                //skip illegal lines
                if( i1 < 0 || i2 < 0 || k1 < 0 || k2 < 0 ){
                    continue;
                }

                string aliasName = line.Substring( k1, k2 - k1 );
                string linkFile = line.Substring( i1, i2 - i1 ).Replace( '/', '\\' );
                if( linkFile.StartsWith( "..\\" ) ) {        // should always be the case
                    linkFile = linkFile.Substring( 3 );     // strip the ../
                }
                string linkPath = relativeHelpPath + linkFile;

                if( tagPaths.ContainsKey( aliasName ) ) {
                    string testPathVal = (string)tagPaths[ aliasName ];
                    if( linkPath == testPathVal ) {
                        continue;    // skip duplicate entries
                    }
                    else {
                        string msg = String.Format( "\r\n       WARNING: Multiple help alias entries for Help Tag.       \r\n\r\n      Help Tag: {0}\r\n", aliasName );
                        MessageBox.Show( msg, "Help Warning" );
                        continue; 
                    }
                }

                string redirPath = redirectFilePath + aliasName + ".html";

                tagPaths.Add( aliasName, redirPath );   // create the help alias
                CreateRedirectFile( redirPath, linkPath );

                Console.WriteLine( "Help alias: {0} :: {1}", aliasName, redirPath );
            }
            tagPaths.Add( "default", defaultFilePath );
        }

        private static void ClearRedirectFolder( string fpath ) {
            if( Directory.Exists( fpath ) == true ) {
                foreach( string file in Directory.GetFiles( fpath ) ) {
                    File.Delete( file );
                }
            }
            else {
                Directory.CreateDirectory( fpath );
            }
        }

        private static void CreateRedirectFile( string path, string redirPath ) {
            FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write );
            StreamWriter sw = new StreamWriter( fs );

            string data = String.Format( redirFileBody, redirPath.Replace( '\\', '/' ) );
            sw.WriteLine( data );

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
