using System;
using System.IO;

namespace NitroReader.Library
{
    /// <summary>
    /// The ProductFile class provides for convenient access to data files that are installed to the
    /// product executable's directory.  This is non-trivial since a data file will not be checked in to CVS
    /// in the actual executable directory (bin/Debug or bin/Release); it will be checked in 
    /// along with the sources 2 levels higher up (or in a special data subdirectory, if desired).
    /// </summary>
    class ProductFile
    {
        /// <summary>
        /// Returns the given path, unless it is in [project]\bin\Debug or [project]\bin\Release - in 
        /// those cases the path returned is for the same-named file in the [project] directory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FilePath( string name ) {
            return ProductFile.FilePath( name, null );
        }

        /// <summary>
        /// Returns the given path, unless it is in [project]\bin\Debug or [project]\bin\Release - in 
        /// those cases the path returned is for the same-named file in [project]\[developmentDataDirectory].
        /// </summary>
        /// <param name="path">Name or full path of file to be accessed</param>
        /// <param name="developmentDataDirectory">Folder for the file when running in a development setup.</param>
        /// <returns></returns>
        public static string FilePath( string path, string developmentDataDirectory ) {

            // see if the requested path is part of a development setup
            FileInfo info = new FileInfo( path );
            string parent = info.DirectoryName;

            if( parent.EndsWith( "bin\\Debug" ) || parent.EndsWith( "bin\\Release" ) ) {
                // we are in a development setup -- use the alternate location
                string file = info.Name;
                string abspath = parent.Substring( 0, parent.LastIndexOf( "bin" ) );

                // add the specified development directory, if any
                if( developmentDataDirectory != null ) {
                    abspath += developmentDataDirectory;
                    if( abspath.EndsWith( "\\" ) == false ) {
                        abspath += "\\";
                    }
                }
                // create the path for development use
                path = abspath + file;
            }

            return path;
        }
    }
}
