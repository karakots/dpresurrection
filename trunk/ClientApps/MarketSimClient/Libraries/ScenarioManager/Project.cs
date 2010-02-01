using System;
using System.Collections;
using System.Text;

using MrktSimDb;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Project
    {
        private Database database;

        private Model[] models;

        private MrktSimDBSchema.projectRow projectRow;

        /// <summary>
        /// The name of this project
        /// </summary>
        public string Name {
            get {
                return projectRow.name;
            }
        }

        /// <summary>
        /// The description of this scenario
        /// </summary>
        public string Description {
            get {
                return projectRow.descr;
            }
        }

        /// <summary>
        /// All models in ths project that match the current Database.AppCode
        /// </summary>
        public Model[] Models {
            get {
                RefreshModels();
                return models;
            }
        }

        /// <summary>
        /// The database containing this project
        /// </summary>
        public Database Database {
            get {
                return database;
            }
        }

        /// <summary>
        /// Constructs a new Project object.  For framework usage only.
        /// </summary>
        /// <param name="database">Database that contains the project</param>
        /// <param name="projectRow">Internal representation of the project</param>
        public Project( Database database, MrktSimDBSchema.projectRow projectRow ) {
            this.database = database;
            this.projectRow = projectRow;
        }

        /// <summary>
        /// Refreshes the models array to reflect the current state of the main database.
        /// </summary>
        private void RefreshModels() {
            ProjectDb projectDb = new ProjectDb();
            projectDb.Connection = this.database.DBConnection;
            projectDb.Refresh();
            string query = String.Format( "project_id = {0} AND app_code = '{1}'", this.projectRow.id, database.AppCode );
            MrktSimDBSchema.Model_infoRow[] modelRows = (MrktSimDBSchema.Model_infoRow[])projectDb.Data.Model_info.Select( query );
            models = new Model[ modelRows.Length ];
            for( int i = 0; i < modelRows.Length; i++ ) {
                models[ i ] = new Model( this, modelRows[ i ] );
            }
        }
    }
}
