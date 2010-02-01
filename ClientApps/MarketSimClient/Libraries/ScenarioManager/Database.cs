using System;
using System.Collections;
using System.Text;

using MrktSimDb;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Database
    {
        private System.Data.OleDb.OleDbConnection dbConnection;

        private Project[] projects;

        private string appCode = "MSNIMOAPP";

        /// <summary>
        /// Checks to see if the given path is a UDL file that describes a connection to a valid MarketSim database.
        /// </summary>
        /// <param name="connectionFilePath">Full path of a UDL connection file.</param>
        /// <param name="err">Describes the error if null is returned for the Database</param>
        /// <returns>A Database oblject if the path is valid; null othewise</returns>
        public static Database ValidateDBConnection( string connectionFilePath, out ErrorInfo err ) {
            err = null;

            // initialize the connection
            System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection();
            try {
                connection.ConnectionString = "File Name=" + connectionFilePath + ";";
            }
            catch( Exception ex ) {
                err = new ErrorInfo( String.Format( "Error: Exception setting database conneciton file to {0}", connectionFilePath ), ex.Message );
                return null;
            }

            if( connection.DataSource == "" ) {
                err = new ErrorInfo( "Error: Connection has empty DataSource", String.Format( "Conneciton file: {0}", connectionFilePath ) );
                return null;
            }

            //check if we are connected to a MarketSim database
            MSConnect msConnect = new MSConnect( connectionFilePath, connection );
            string errorString = null;
            bool canConnect = false;
            bool doConvert = false;
            bool success = msConnect.TestConnection( out errorString, out canConnect, out doConvert );

            if( success == false ){
                err = new ErrorInfo( "Error: Database connection test failed", errorString );
                return null;
            }

            if( canConnect == false ){
                err = new ErrorInfo( "Error: Cannot connect to database", errorString );
                return null;
            }

            if( doConvert == true ){
                err = new ErrorInfo( "Error: Database is not current version", "Use MarketSim to update your database." );
                return null;
            }

            return new Database( connection );
        }

        /// <summary>
        /// Checks to see if the given connection is a connection to a valid MarketSim database.
        /// </summary>
        /// <param name="oleDbConnection">Database connection.</param>
        /// <param name="err">Describes the error if null is returned for the Database</param>
        /// <returns>A Database oblject if the path is valid; null othewise</returns>
        public static Database ValidateDBConnection( System.Data.OleDb.OleDbConnection oleDbConnection, out ErrorInfo err ) {
            err = null;

            //check if we are connected to a MarketSim database
            bool canConnect = false;
            bool doConvert = false;
 			string errorString = ProjectDb.testConnection( oleDbConnection, out canConnect, out doConvert );

            bool success = (errorString == null || errorString == "");

            if( success == false ) {
                err = new ErrorInfo( "Error: Database connection test failed", errorString );
                return null;
            }

            if( canConnect == false ) {
                err = new ErrorInfo( "Error: Cannot connect to database", errorString );
                return null;
            }

            if( doConvert == true ) {
                err = new ErrorInfo( "Error: Database is not current version", "Use MarketSim to update your database." );
                return null;
            }

            return new Database( oleDbConnection );
        }

        /// <summary>
        /// The list of all MarketSim projects in the database.
        /// </summary>
        public Project[] Projects {
            get { 
                RefreshProjects();
                return projects;
            }
        }

        /// <summary>
        /// The DB connection object.
        /// </summary>
        public System.Data.OleDb.OleDbConnection DBConnection {
            get {
                return dbConnection;
            }
        }

        /// <summary>
        /// Resets the app code (changes from predefined custom app code to standard MarketSim).
        /// </summary>
        public void ResetAppCode() {
            appCode = "";
        }

        /// <summary>
        /// Returns the app_code to use for legal models.
        /// </summary>
        public string AppCode {
            get {
                return appCode;
            }
        }

        /// <summary>
        /// Constructs a rnew database object.  For framework usage only.
        /// </summary>
        /// <param name="dbConnection"></param>
        private Database( System.Data.OleDb.OleDbConnection dbConnection ) {
            this.dbConnection = dbConnection;
        }

        /// <summary>
        /// Refreshes the models array to reflect the current state of the main database.
        /// </summary>
        private void RefreshProjects() {
            ProjectDb projectDb = new ProjectDb();
            projectDb.Connection = this.dbConnection;
            projectDb.Refresh();
            string query = "";
            MrktSimDBSchema.projectRow[] projRows = (MrktSimDBSchema.projectRow[])projectDb.Data.project.Select( query );
            projects = new Project[ projRows.Length ];
            for( int i = 0; i < projRows.Length; i++ ) {
                projects[ i ] = new Project( this, projRows[ i ] );
            }
        }
    }
}
