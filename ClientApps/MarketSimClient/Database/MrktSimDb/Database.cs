using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public abstract partial class Database
    {
        private class DataRowPair
        {
            public DataRow parent = null;
            public DataRow child = null;
        }

        // Static Methods

        #region Version Control

        // currrent database verion
        private const int major_curr = 2;
        private const int minor_curr = 6;
        private const int release_curr = 0;

        // database compatibility
        // may be older then the current version
        private const int major_comp = 2;
        private const int minor_comp = 6;
        private const int release_comp = 0;


        public static void GetCurrentDbVersion(out int major, out int minor, out int release)
        {
            major = major_curr;
            minor = minor_curr;
            release = release_curr;
        }

        // clear results for all models with model_type = 1
        static public bool GetDbVersion(System.Data.OleDb.OleDbConnection aConnection,
            out int major, out int minor, out int release)
        {
            major = 0;
            minor = 0;
            release = -1;

            if (aConnection == null)
                return false;

            // now check product version
            OleDbCommand aCommand = new OleDbCommand("Select major_no, minor_no, release_no  FROM db_schema_info", aConnection);

            // 2 minutes to connect
            aCommand.CommandTimeout = 120;

            try
            {
                aCommand.Connection.Open();
            }
            catch
            {
                return false;
            }

            OleDbDataReader dataReader = aCommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int curMajor = dataReader.GetInt32(0);
                int curMinor = dataReader.GetInt32(1);
                int curRelease = dataReader.GetInt32(2);

                if (curMajor > major)
                {
                    major = curMajor;
                    minor = curMinor;
                    release = curRelease;
                }
                else if (curMajor == major && curMinor > minor)
                {
                    minor = curMinor;
                    release = curRelease;
                }
                else if (curMajor == major && curMinor == minor && curRelease > release)
                {
                    release = curRelease;
                }
            }

            dataReader.Close();

            return true;
        }

        #endregion

        #region NIMO & other App Codes

        public enum AppType
        {
            MarketSim,
            NIMO,
            DevlTest
        }

        private static AppType app = AppType.MarketSim;
        public static AppType Application
        {
            set
            {
                app = value;
            }

            get
            {
                return app;
            }
        }


        static public bool Nimo
        {
            get
            {
                return app == AppType.NIMO;
            }
        }

        static public string AppCode
        {
            get
            {
                if( app == AppType.MarketSim || app == AppType.DevlTest )
                {
                    return "";
                }

                return "MS" + AppName + "APP";
            }
        }

        static public string AppName
        {
            get
            {
                return Enum.GetName( typeof(AppType), app );
            }
        }

        #endregion

        #region Constants

        public const string NoProjectName = "No Open Project";

        public const int OpenAccessModel = 0;
        public const int ReadOnlyModel = 1; // TODO only using open
        public const int LockedModel = 2;

        public const int PendingSimQueue = 0;
        public const int RunningSimQueue = 1;
        public const int DoneSimQueue = 2;

        public const int AllID = -1;
        static public bool LegalName(string name)
        {
            char[] illegal = { '\'', '<', '>', '\"'};

            int index = name.IndexOfAny(illegal);

            if (index < 0)
                return true;

            return false;
        }

        static public string CreateUniqueName(DataTable table, string colName, string baseName, string filter)
        {
            string[] ban = { "'", "\n", "\t", "\r" };
            foreach (string remove in ban)
            {
                baseName = baseName.Replace(remove, "");
            }

            string query = colName + " = " + "'" + baseName + "'";

            if (filter != null && filter != "")
                query += " AND " + filter;

            DataRow[] rows = table.Select(query, "", DataViewRowState.CurrentRows);

            if (rows.Length == 0)
                return baseName;

            // we need to create a unique name
            // first we trim _X from tmp_name where X is an integer

            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] underBar = { '_' };
            int underBarLoc = baseName.LastIndexOfAny(underBar);
            int index = 1;

            if (underBarLoc >= 0 && underBarLoc < baseName.Length)
            {
                string integerSubString = baseName.Substring(underBarLoc + 1, baseName.Length - underBarLoc - 1);

                try
                {
                    index = Int32.Parse(integerSubString);
                    index++;
                }
                catch (Exception)
                {
                    index = 1;
                }

                integerSubString = integerSubString.TrimEnd(numbers);

                if (integerSubString.Length == 0)
                {
                    // ok string fits pattern so trim off
                    baseName = baseName.Remove(underBarLoc, baseName.Length - underBarLoc);
                }
            }

            string tmpName = null;

            while (tmpName == null)
            {
                tmpName = baseName + "_" + index;

                query = colName + " = " + "'" + tmpName + "'";

                if( filter != null && filter != "" )
                    query += " AND " + filter;

                rows = table.Select(query, "", DataViewRowState.CurrentRows);

                if (rows.Length > 0)
                {
                    tmpName = null;
                    index++;
                }
            }

            return tmpName;
        }


        #endregion

        #region Table dependency

        private DataTable[] dbTables = null;
        private DataTable[] getDataTables()
        {
            return new DataTable[] 
                    {
                        Data.project,
                        Data.Model_info,

                        Data.pack_size,
                        Data.product_type,
                        Data.product,
                        Data.product_tree,
                        Data.pack_size_dist,

                        Data.segment,
                        Data.channel,

                        Data.product_channel_size,

                        Data.segment_channel,
                        Data.network_parameter,
                        Data.segment_network,

                        Data.product_attribute,
                        Data.product_attribute_value,
                        Data.consumer_preference, 
                        Data.product_matrix,

                        Data.price_type,
                        Data.segment_price_utility,
                       
                        Data.task,
                        Data.task_product_fact,
                        Data.task_rate_fact,
                        Data.share_pen_brand_aware,
                         
                        // sceanrios and market plans
                        Data.scenario,
                        Data.market_plan,
                        Data.scenario_market_plan,
                        Data.market_plan_tree,
                        Data.distribution,
                        Data.display,
                        Data.mass_media,
                        Data.market_utility,
                        Data.product_channel,
                       
                      
                        // external factors
                        Data.product_event,
                        Data.task_event,
                        
                        // external data
                        Data.external_data,

                        // all important
                        Data.model_parameter,

                        // simulation
                        Data.simulation,
                        Data.simulation_status,
                        
                        // despite names these are associated to simulations
                        Data.scenario_parameter,
                        Data.scenario_variable,
                        Data.scenario_simseed,
                        Data.scenario_metric,

                        // results
                        Data.sim_queue,
                        Data.results_status,
                        Data.sim_variable_value,
                        Data.run_log
                    };
        }

        public DataTable[] DbTables
        {
            get
            {
                return dbTables;
            }
        }

        virtual protected bool TableInSchema(DataTable table)
        {
            if (table == Data.project ||
                table == Data.Model_info)
                return true;

            return false;
        }

        #endregion

        #region Connection Utilities
        /// <summary>
        /// create a new command - sets timeout to infinity by defaul
        /// </summary>
        /// <returns></returns>
        public static OleDbCommand newOleDbCommand()
        {
            OleDbCommand command = new OleDbCommand();

            command.CommandTimeout = 0;

            return command;
        }
        
        static public string testConnection(System.Data.OleDb.OleDbConnection aConnection)
        {
            bool convert = false;
            return testConnection(aConnection, out convert);
        }

        static public string testConnection(System.Data.OleDb.OleDbConnection aConnection, out bool convert)
        {
            bool dummy;

            return testConnection(aConnection, out dummy, out convert);

        }

        static public string testConnection(System.Data.OleDb.OleDbConnection aConnection, out bool canConnect, out bool convert)
        {
            canConnect = false;
            convert = false;

            if (aConnection == null)
                return "Null Connection";

            try
            {
                aConnection.Open();
            }
            catch
            {
                return "Error Opening Connection";
            }

            canConnect = true;

            string rval = null;
            int num = 0;


            OleDbCommand aCommand = new OleDbCommand("Select COUNT(*) FROM db_schema_info", aConnection);

            // wait a sec
            aCommand.CommandTimeout = 30;

            try
            {
                num = Convert.ToInt32(aCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                rval = "Not a MarketSim database: " + e.Message;
            }

            aConnection.Close();

            if (rval == null)
            {
                int major = 0;
                int minor = 0;
                int release = 0;

                GetDbVersion(aConnection, out major, out minor, out release);

                // compare version with compatibility
                if (major < major_comp ||
                    minor < minor_comp ||
                    release < release_comp)
                {
                    rval = "MarketSim database needs to be converted to current release";
                    convert = true;
                }
            }

            return rval;
        }

        #endregion


        #region Copy

        // utility
        protected static void copyTable(DataTable toTable, DataTable fromTable)
        {
            foreach (DataRow row in fromTable.Select("", "", DataViewRowState.CurrentRows))
            {
                toTable.LoadDataRow(row.ItemArray, false);
            }
        }

        #endregion

        #region Object State  queries

        // Current rules for locking
        // A opening model or simulation locks the model or simulaiton
        // if a model has any locked simulations it is report that it is locked as well
        // if the model for a simulation is locked then the simulation will report that it is locked
        // locking is not a transitive operation so 
        // locking a simulaiton does NOT lock other simulations

        public bool ProjectDeleted( int projId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM project WHERE id =  " + projId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return true;

            return false;
        }

        public bool ModelDeleted( int modId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM Model_info WHERE model_id =  " + modId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return true;

            return false;
        }

        public bool ScenarioDeleted( int scenId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM scenario WHERE scenario_id =  " + scenId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return true;

            return false;
        }

        public bool SimDeleted( int simId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE id =  " + simId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return true;

            return false;
        }


        protected void LockModel(int modId,  bool val ) {

            int num = 0;

            if( val )
                num = 1;

            genericCommand.CommandText = "UPDATE model_info SET " + 
                " locked_by = SYSTEM_USER, " +
                " locked_on = getdate(), " +
                " locked = " + num + 
                " WHERE model_id = " + modId;
            Connection.Open();
            genericCommand.ExecuteNonQuery();
            Connection.Close();
        }

        protected void LockSimulation( int simId, bool val ) {

            int num = 0;

            if( val )
                num = 1;

            genericCommand.CommandText = "UPDATE simulation SET "  +
                " locked_by = SYSTEM_USER, " +
                " locked_on = getdate(), " +
                " locked = " + num + 
                " WHERE id = " + simId;

            Connection.Open();
            genericCommand.ExecuteNonQuery();
            Connection.Close();
        }

        // Projects are considered locked if any models are locked
        public bool ProjectLocked( int projID ) {

            // check for locked models
            genericCommand.CommandText = "SELECT COUNT(*) FROM Model_info WHERE locked = 1 AND project_id = " + projID;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            // check for locked simulations
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE locked = 1 AND scenario_id IN " +
                " (SELECT scenario_id FROM scenario WHERE model_id IN " +
                " (SELECT model_id FROM Model_info WHERE project_id = " + projID +
                " ))";

            Connection.Open();
            num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }

        public bool ModelLocked( int modId, out string who ) {
            who = null;
            genericCommand.CommandText = "SELECT COUNT(*) FROM Model_info WHERE locked = 1 AND model_id =  " + modId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 ) {
                // find out who
                genericCommand.CommandText = "SELECT TOP 1 locked_by FROM Model_info WHERE locked = 1 AND model_id =  " + modId;

                Connection.Open();
                who = Convert.ToString( genericCommand.ExecuteScalar() );
                Connection.Close();

                return true;
            }

            // check if any simulations are locked
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE locked = 1 AND scenario_id IN " +
                " (Select scenario_id from scenario where model_id = " + modId + " )";

            Connection.Open();
            num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 ) {
                // find out who
                genericCommand.CommandText = "SELECT TOP 1 locked_by FROM simulation WHERE locked = 1 AND scenario_id IN " +
                " (Select scenario_id from scenario where model_id = " + modId + " )";

                Connection.Open();
                who = Convert.ToString( genericCommand.ExecuteScalar() );
                Connection.Close();
                return true;
            }


            return false;
        }

        // TBD
        // we do not lock scenarios serpately from models currently
        //public bool ScenarioLocked( int scenarioId ) {
        //    genericCommand.CommandText = "SELECT COUNT(*) FROM scenario WHERE locked = 1 AND scenario_id =  " + scenarioId;

        //    Connection.Open();
        //    int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
        //    Connection.Close();

        //    if( num > 0 )
        //        return true;

        //    return false;
        //}

        public bool SimulationLocked( int simId, out string who ) {
            
            who = null;

            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE locked = 1 AND id =  " + simId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 ) {
                genericCommand.CommandText = "SELECT TOP 1 locked_by FROM simulation WHERE locked = 1 AND id =  " + simId;

                Connection.Open();
                who = Convert.ToString( genericCommand.ExecuteScalar() );
                Connection.Close();
                return true;
            }

            // TBD
            // now check scenarios
            // when we lock scenarios seperate from models
            // SSN


            // check models
            genericCommand.CommandText = "SELECT COUNT(*) FROM model_info WHERE locked = 1 AND model_id IN " +
               " (SELECT model_id FROM scenario WHERE scenario_id IN " +
               " (SELECT scenario_id FROM simulation WHERE id = " + simId +
                   " ))";

            Connection.Open();
            num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 ) {
                genericCommand.CommandText = "SELECT TOP 1 locked_by FROM model_info WHERE locked = 1 AND model_id IN " +
              " (SELECT model_id FROM scenario WHERE scenario_id IN " +
              " (SELECT scenario_id FROM simulation WHERE id = " + simId +
                  " ))";

                Connection.Open();
                who = Convert.ToString( genericCommand.ExecuteScalar() );
                Connection.Close();
                return true;
            }

            // TBD
            // check projects
            // when we end up actually locking them

            return false;
        }


       // Checks for running simulations
        public bool ProjectHasSimulationRunning( int projId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE sim_num  >= 0 AND scenario_id IN " +
                " (SELECT scenario_id FROM scenario WHERE model_id IN " + 
                " (SELECT model_id from Model_info WHERE project_id = " + projId + "))";

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }

        public bool ModelHasSimulationRunning( int modId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE sim_num  >= 0 AND scenario_id IN " +
                " (SELECT scenario_id FROM scenario WHERE model_id = " + modId + ")";

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }

        public bool ScenarioHasSimulationRunning( int scenarioId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE sim_num  >= 0 AND scenario_id = " + scenarioId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }

        public bool SimulationRunning( int simId ) {
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE sim_num  >= 0 AND id = " + simId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }

        // this routine will only dequeue if the simulation is NOT already running
        public bool DequeueSim( int simId ) {
            genericCommand.CommandText = "UPDATE simulation SET sim_num = -1 WHERE sim_num  = 1 AND id = " + simId;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteNonQuery() );
            Connection.Close();

            if( num > 0 )
                return true;

            return false;
        }


        #endregion



        /// <summary>
        /// A Database is a particular view at the MrktSim Schema
        /// </summary>
        public Database()
        {
            adapterTable = new DataTable("AdapterTable");

            DataColumn idCol = adapterTable.Columns.Add("table", typeof(DataTable));
            adapterTable.PrimaryKey = new DataColumn[] { idCol };

            adapterTable.Columns.Add("adapter", typeof(OleDbDataAdapter));

            // used to update a row after insertion
            updateCommand.CommandText = "SELECT @@IDENTITY";

            dbTables = getDataTables();

            initializeAdapters();

            InitializeSelectQuery();

            createExpressionColumn();
            addExpressionColumns();

            initializeIdColumnsReadStatus();
        }

        private void initializeIdColumnsReadStatus()
        {
            //
            // when we insert, all columns need to be writable
            //

            Data.project.idColumn.ReadOnly = false;
            Data.Model_info.model_idColumn.ReadOnly = false;
            Data.scenario.scenario_idColumn.ReadOnly = false;
            //Data.brand.brand_idColumn.ReadOnly = false;
            Data.product.product_idColumn.ReadOnly = false;
            Data.product_type.idColumn.ReadOnly = false;
            Data.segment.segment_idColumn.ReadOnly = false;
            Data.channel.channel_idColumn.ReadOnly = false;
            Data.task.task_idColumn.ReadOnly = false;
            Data.product_attribute.product_attribute_idColumn.ReadOnly = false;
            Data.consumer_preference.record_idColumn.ReadOnly = false;
            Data.product_attribute_value.record_idColumn.ReadOnly = false;
            Data.market_plan.idColumn.ReadOnly = false;
            Data.product_channel.record_idColumn.ReadOnly = false;
            Data.distribution.record_idColumn.ReadOnly = false;
            Data.display.record_idColumn.ReadOnly = false;
            Data.market_utility.record_idColumn.ReadOnly = false;
            Data.mass_media.record_idColumn.ReadOnly = false;
            Data.product_event.record_idColumn.ReadOnly = false;
            Data.task_event.record_idColumn.ReadOnly = false;
            Data.network_parameter.idColumn.ReadOnly = false;
            Data.model_parameter.idColumn.ReadOnly = false;
            Data.sim_queue.run_idColumn.ReadOnly = false;
            Data.scenario_variable.idColumn.ReadOnly = false;
            Data.scenario_simseed.idColumn.ReadOnly = false;
            Data.simulation.idColumn.ReadOnly = false;
            Data.pack_size.idColumn.ReadOnly = false;
            Data.price_type.idColumn.ReadOnly = false;
        }

        #region fields and properties

        /// <summary>
        /// If True database will use all tables during an update
        /// Even if the table is not in current schema
        /// Use this to add data to schema
        /// </summary>
        public bool UpdateAll = false;

        public bool HasChanges()
        {
            if (ReadOnly)
            {
                return false;
            }

            return Data.HasChanges();
        }

        private bool readOnly = false;
        private bool queued = false;

        public bool Queued
        {
            get
            {
                return queued;
            }

            set
            {
                queued = value;
            }
        }


        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }

            set
            {
                readOnly = value;
            }
        }

        // just a generic command for use
        public OleDbCommand genericCommand = newOleDbCommand();	

        // for updating after a database insertion
        private OleDbCommand updateCommand = newOleDbCommand();

        
      
      


        private System.Data.OleDb.OleDbConnection connection = null;
        public System.Data.OleDb.OleDbConnection Connection
        {
            set
            {
                connection = null;

                if (value != null)
                {
                    connection = new OleDbConnection(value.ConnectionString);
                }

                genericCommand.Connection = connection;
                updateCommand.Connection = connection;

                // update the rest of the adapters

                foreach (DataRow row in this.adapterTable.Rows)
                {
                    OleDbDataAdapter adapter = (OleDbDataAdapter) row["adapter"];

                    adapter.SelectCommand.Connection = connection;
                    adapter.InsertCommand.Connection = connection;
                    adapter.UpdateCommand.Connection = connection;
                    adapter.DeleteCommand.Connection = connection;
                }
            }

            get
            {
                return connection;
            }
        }

        // access the model info
        private MrktSimDBSchema theDb = new MrktSimDBSchema();
        public MrktSimDBSchema Data
        {
            get
            {
                return theDb;
            }
        }

     
        #endregion

        // called from calibration
        // TBD fix this
        public void AddTable( DataTable table )
        {
            if( getAdapter( table ) == null )
            {
                OleDbDataAdapter adapter = addAdapterToTable( table );

                if( table != null )
                {
                    if( table == Data.scenario_market_plan )
                    {
                        adapter = getAdapter( Data.scenario_market_plan );
                        if( adapter != null )
                        {
                            SqlQuery.initialize_ScenarioMarketPlan_UpdateQuery( adapter );

                        }
                    }
                    else if( table == Data.market_plan )
                    {
                        adapter = getAdapter( Data.market_plan );
                        if( adapter != null )
                        {
                            SqlQuery.initialize_MarketPlan_UpdateQuery( adapter );
                            adapter.RowUpdated += new OleDbRowUpdatedEventHandler( market_plan_RowUpdated );
                        }
                    }
                    else if( table == Data.product_event )
                    {
                        adapter = getAdapter( Data.product_event );
                        if( adapter != null )
                        {
                            SqlQuery.initialize_Product_Event_UpdateQuery( adapter );
                        }
                    }
                }

                adapter.SelectCommand.Connection = Connection;
                adapter.InsertCommand.Connection = Connection;
                adapter.UpdateCommand.Connection = Connection;
                adapter.DeleteCommand.Connection = Connection;

                UpdateAll = true;
            }
        }


        private OleDbDataAdapter addAdapterToTable(DataTable table)
        {
            DataRow row = adapterTable.NewRow();
            row["table"] = table;

            OleDbDataAdapter adapter = new OleDbDataAdapter();

            //   adapter.UpdateBatchSize = 100;

            adapter.SelectCommand = newOleDbCommand();
            adapter.InsertCommand = newOleDbCommand();
            adapter.UpdateCommand = newOleDbCommand();
            adapter.DeleteCommand = newOleDbCommand();

            // adapter.ContinueUpdateOnError = true;

            row["adapter"] = adapter;
            adapterTable.Rows.Add(row);

            return adapter;
        }

        private DataTable adapterTable = null;

        private void initializeAdapters()
        {
            adapterTable.Clear();

            foreach (DataTable table in DbTables)
            {
                if (TableInSchema(table))
                {
                    addAdapterToTable(table);
                }
            }

            adapterTable.AcceptChanges();

            // setup the queries
            initializeAdapterUpdate(); //null);
        }

        protected System.Data.OleDb.OleDbDataAdapter getAdapter(DataTable table)
        {
            DataRow row = adapterTable.Rows.Find(table);

            if (row == null)
            {
                return null;
            }

            return (System.Data.OleDb.OleDbDataAdapter) row["adapter"];
        }

        private bool updateTable(DataTable table, DataViewRowState state)
        {

            DataRow[] rows = table.Select("", "", state);

            if (rows.Length > 0)
            {
                // get appropriate adapter
                OleDbDataAdapter adapter = getAdapter(table);

                if (adapter != null)
                {
                    int numAffected = 0;
                    try
                    {
                        numAffected = adapter.Update( rows );
                    }
                    catch( Exception e )
                    {
                        if( numAffected == rows.Length )
                        {
                            // sometime we do not update all the expected rows
                            // but this is not the error
                            throw e;
                        }
                    }
                }
            }

            return rows.Length > 0;
        }

          /// <summary>
        /// Refreshes table without clearing rows
        /// will not delete - only handles mods and additions
        /// use RefreshTable(table, idCol) to allow deletions
        /// </summary>
        /// <param name="table"></param>
        public string RefreshTable(DataTable table)
        {
            return RefreshTable(table, null);
        }

        /// <summary>
        /// Refreshes table without clearing rows
        /// deletes rows that have an id column idCol
        /// </summary>
        /// <param name="table"></param>
        public string RefreshTable(DataTable table, string idCol)
        {
            OleDbDataAdapter adapter = getAdapter(table);

            if (adapter == null)
                return "Table not in schema";

            MrktSimDBSchema data = new MrktSimDBSchema();

            data.EnforceConstraints = false;

            // fill a copy of this table

            try
            {
                adapter.Fill(data, table.TableName);
            }
            catch (Exception oops)
            {
                return oops.Message;
            }

            if (idCol != null)
            {
                foreach (DataRow orig in table.Select("", "", DataViewRowState.CurrentRows))
                {
                    int id = (int)orig[idCol];

                    if (data.Tables[table.TableName].Select(idCol + " = " + id).Length == 0)
                    {
                        orig.Delete();
                    }
                }
            }

            try
            {
                theDb.Merge(data, false);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            table.AcceptChanges();
            
           

            return null;
        }

        public void Refresh()
        {
            Data.Clear();

            // get version
            int major, minor, release;
            GetDbVersion(this.Connection, out major, out minor, out release);

            MrktSimDBSchema.db_schema_infoRow version = Data.db_schema_info.Newdb_schema_infoRow();

            version.major_no = major;
            version.minor_no = minor;
            version.release_no = release;

            Data.db_schema_info.Adddb_schema_infoRow(version);

            Data.db_schema_info.AcceptChanges();

          

            // projects and models are somewhat special cases
            OleDbDataAdapter adapter = getAdapter(Data.project);
            adapter.Fill(Data.project);

            adapter = getAdapter(Data.Model_info);
            adapter.Fill(Data.Model_info);

            createItemsAsAll();

            foreach (DataTable table in DbTables)
            {
                if (table == Data.project ||
                    table == Data.Model_info)
                    continue;

                if (TableInSchema(table))
                {
                    adapter = getAdapter(table);

                    if (adapter != null)
                    {
                      
                        adapter.Fill(table);
                       
                    }
                }
            }
        }

        /// <summary>
        /// Some conversion do not cause back incompatability
        /// </summary>
        private void performDbConversions()
        {
            // remove contraint
            genericCommand.CommandText = "IF EXISTS(SELECT name FROM sys.tables WHERE name = 'product_event_type') ALTER TABLE product_event DROP CONSTRAINT [ProductEventType]";

            genericCommand.Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch( Exception e) {
                string message = e.Message;

            }

            genericCommand.Connection.Close();


            // remove table
            genericCommand.CommandText = "IF EXISTS(SELECT name FROM sys.tables WHERE name = 'product_event_type') DROP TABLE product_event_type";

            genericCommand.Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch( Exception ) { }

            genericCommand.Connection.Close();

        }

        public void Update()
        {
            if (Connection == null)
                return;

            if (ReadOnly)
            {
                Data.AcceptChanges();
                return;
            }

            performDbConversions();

            Data.EnforceConstraints = false;

            removeExpressionColumns();

            // we do not write out the "all" items
            AcceptItemsMarkedAll();

            bool manageParameters = this.GetType() == typeof(ModelDb);

            // parameters do not have the luxury of having a relation with the actual
            // row they are tied to.
            // So we have to do them manually

            List<DataRowPair> parmRows = null;

            if (manageParameters)
            {
                UpdateParameters();

                // get current rows one for each parameter
                // we are really only concerned with rows with a changing ID so we will
                // double check that in the sequal
                parmRows = getParmRows();
            }

            foreach(DataTable table in DbTables)
            {
                if (TableInSchema(table) || UpdateAll)
                {
                    updateTable(table, DataViewRowState.Added);
                }
            }

            //
            // Now check if the parm rows have changed
            //
            // see how we need to do it prior to writing out
            if (parmRows != null)
            {
                checkParmRowID(parmRows);
            }

            // in case the model id changed
            // we do not write out the "all" items
            AcceptItemsMarkedAll();

            foreach (DataTable table in DbTables)
            {
                if (TableInSchema(table) || UpdateAll)
                {
                    updateTable(table, DataViewRowState.ModifiedCurrent);
                }
            }

            for (int ii = DbTables.Length; ii > 0; --ii)
            {
                DataTable table = DbTables[ii - 1];

                if (TableInSchema(table))
                {
                    updateTable(table, DataViewRowState.Deleted);
                }
            }


            addExpressionColumns();
            Data.EnforceConstraints = true;
        }

        private void resetPrimaryKey( DataTable table ) {

            DataColumn[] cols = table.PrimaryKey;

            if (cols.Length == 0)
            {
                return;
            }

            foreach( DataRow row in table.Select( "", "", DataViewRowState.CurrentRows ) ) {

                foreach( DataColumn key in cols ) {
                    int id = (int)row[ key ];

                    if( id > 0 ) {
                        row[ key ] = -100 - id;
                    }
                }
            }
        }

        public void ResetPrimaryKeys()
        {
            UpdateParameters();

            List<DataRowPair> parmRows = getParmRows();
            
            // do not reset key for project or model
             for (int ii = 0; ii < dbTables.Length; ++ii)
            {
                DataTable table = dbTables[ii];

                if( table != Data.project && table != Data.Model_info ) {
                    resetPrimaryKey( table );
                }
            }

            if( parmRows != null ) {
                checkParmRowID( parmRows );
            }
        }

        public void Copy(Database cp)
        {
            for (int ii = 0; ii < dbTables.Length; ++ii)
            {
                DataTable fromTable = dbTables[ii];
                DataTable toTable = cp.dbTables[ii];

                copyTable(toTable, fromTable);
            }

            cp.ResetPrimaryKeys();
        }

        // special plan update
        //public void ReadPlanData(MrktSimDBSchema.market_planRow plan)
        //{
        //    string query = "market_plan_id = " + plan.id;
        //    int numData = 0;

        //    switch ((PlanType)plan.type)
        //    {
        //        case PlanType.Price:
        //            numData = Data.product_channel.Select(query).Length;
        //            break;
        //        case PlanType.Distribution:
        //            numData = Data.distribution.Select(query).Length;
        //            break;
        //        case PlanType.Display:
        //            numData = Data.display.Select(query).Length;
        //            break;
        //        case PlanType.Media:
        //            numData = Data.mass_media.Select(query).Length;
        //            break;
        //        case PlanType.Coupons:
        //            numData = Data.mass_media.Select(query).Length;
        //            break;
        //        case PlanType.Market_Utility:
        //            numData = Data.market_utility.Select(query).Length;
        //            break;
        //    }

        //    if (numData > 0)
        //    {
        //        return;
        //    }

        //   // currentPlanID = plan.id;

        //    // initializeMSAdapterSelectQuery();

        //    switch ((PlanType)plan.type)
        //    {
        //        case PlanType.Price:
        //            getAdapter(Data.product_channel).Fill(Data.product_channel);
        //            break;
        //        case PlanType.Distribution:
        //            getAdapter(Data.distribution).Fill(Data.distribution);
        //            break;
        //        case PlanType.Display:
        //            getAdapter(Data.display).Fill(Data.display);
        //            break;
        //        case PlanType.Media:
        //            getAdapter(Data.mass_media).Fill(Data.mass_media);
        //            break;
        //        case PlanType.Coupons:
        //            getAdapter(Data.mass_media).Fill(Data.mass_media);
        //            break;
        //        case PlanType.Market_Utility:
        //            getAdapter(Data.market_utility).Fill(Data.market_utility);
        //            break;
        //    }

        //    // currentPlanID = ModelDb.AllID;
        //    // initializeMSAdapterSelectQuery();
        //}

        #region Adapter Updating

        protected virtual void InitializeSelectQuery()
        {
            //
            //some read only tables we use to get types out of
            //

            // Project 

            OleDbDataAdapter adapter = null;

            adapter = getAdapter(Data.project);

            if (adapter != null)
                SqlQuery.InitializeProjectSelect(adapter.SelectCommand);

            //
            // model info
            //
            // These are common to both the project and the model so need to be one place or the other.
            //
            adapter = getAdapter(Data.Model_info);

            if (adapter != null)
                SqlQuery.InitializeModelSelect(adapter.SelectCommand);
            
            // 
            // oleDbSelectCommand for sim_queue
            // 
            adapter = getAdapter( Data.sim_queue );
            if( adapter != null )
                SqlQuery.InitializeSimQueueSelect( adapter.SelectCommand );

            // 
            // oleDbSelectCommand for simulation_status
            // 
            adapter = getAdapter( Data.simulation_status );
            if( adapter != null )
                SqlQuery.InitializeSimulationStatusSelect( adapter.SelectCommand );

            // 
            // oleDbSelectCommand for results_status
            // 
            adapter = getAdapter( Data.results_status );
            if( adapter != null )
                SqlQuery.InitializeResultsStatusSelect( adapter.SelectCommand );

            // 
            // oleDbSelectCommand for sim_queue
            // 
            adapter = getAdapter(Data.run_log);
            if (adapter != null)
                SqlQuery.InitializeRunLogSelect(adapter.SelectCommand);
            
            //
            // scenario
            //
            adapter = getAdapter(Data.scenario);
            if (adapter != null)
                SqlQuery.InitializeScenarioSelect(adapter.SelectCommand);
            

            // product
            adapter = getAdapter(Data.product);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                "model_id, product_id, product_name, brand_id, type, product_group, related_group, percent_relation, " +
                "cost, initial_dislike_probability, repeat_like_probability, color, product_type, base_price, eq_units, pack_size_id " +
                "FROM product";
            
            // product_tree
            adapter = getAdapter(Data.product_tree);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, parent_id, child_id " +
                    "FROM product_tree";

            // product_type
            adapter = getAdapter(Data.product_type);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, id, type_name " +
                    "FROM product_type";

            // pack_size
            adapter = getAdapter( Data.pack_size );
            if( adapter != null )
                adapter.SelectCommand.CommandText = "SELECT id, model_id, name " +
                    "FROM pack_size";

            // pack_size dist
            adapter = getAdapter( Data.pack_size_dist );
            if( adapter != null )
                adapter.SelectCommand.CommandText = "SELECT pack_size_id, size, dist " +
                    "FROM pack_size_dist";

            adapter = getAdapter( Data.price_type );
            if( adapter != null )
                adapter.SelectCommand.CommandText = "SELECT id, model_id, name, relative, awareness, persuasion, BOGN " +
                    "FROM price_type";

            adapter = getAdapter( Data.segment_price_utility );
            if( adapter != null )
                adapter.SelectCommand.CommandText = "SELECT segment_id, price_type_id, util " +
                    "FROM segment_price_utility";


            // product_channel_size
            adapter = getAdapter(Data.product_channel_size);
            if (adapter != null)
            {
                adapter.SelectCommand.CommandText = "SELECT model_id, product_id, channel_id, prod_size " +
                    "FROM product_channel_size";
            }

            // segment
            adapter = getAdapter(Data.segment);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, segment_id, segment_model, segment_name, color, show_current_share_pie_chart, " +
                    "show_cmulative_share_chart, segment_size, growth_rate, growth_rate_people_percent, growth_rate_month_year, " +
                    "compress_population, variability, price_disutility, attribute_sensitivity, persuasion_scaling, display_utility, " +
                    "display_utility_scaling_factor, max_display_hits_per_trip, inertia, repurchase, repurchase_model, " +
                    "gamma_location_parameter_a, gamma_shape_parameter_k, repurchase_period_frequency, repurchase_frequency_variation, " +
                    "repurchase_timescale, avg_max_units_purch, shopping_trip_interval, category_penetration, " +
                    "category_rejection, num_initial_buyers, initial_buying_period, seed_with_repurchasers, use_budget, budget, " +
                    "budget_period, save_unspent, initial_savings, social_network_model, num_close_contacts, " +
                    "prob_talking_close_contact_pre, prob_talking_close_contact_post, use_local, num_distant_contacts, " +
                    "prob_talk_distant_contact_pre, prob_talk_distant_contact_post, awareness_weight_personal_message, " +
                    "pre_persuasion_prob, post_persuasion_prob, units_desired_trigger, awareness_model, awareness_threshold, " +
                    "awareness_decay_rate_pre, awareness_decay_rate_post, persuasion_decay_rate_pre, persuasion_decay_rate_post, " +
                    "persuasion_decay_method, product_choice_model, persuasion_score, persuasion_value_computation, " +
                    "persuasion_contribution_overall_score, utility_score, combination_part_utilities, price_contribution_overall_score, " +
                    "price_score, price_value, reference_price, choice_prob, inertia_model, error_term, error_term_user_value, loyalty, " +
                    " min_freq, max_freq " +
                    "FROM segment";

            // channel
            adapter = getAdapter(Data.channel);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, channel_id, channel_name " +
                    "FROM channel";

            // comsumer_preference
            adapter = getAdapter(Data.consumer_preference);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, record_id, segment_id, product_attribute_id, start_date, pre_preference_value, post_preference_value, price_sensitivity " +
                    "FROM consumer_preference";

            // distribution
            adapter = getAdapter(Data.distribution);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, product_id, channel_id, attr_value_F, attr_value_G, message_awareness_probability, " +
                    "message_persuation_probability, start_date, end_date, market_plan_id " +
                    " FROM dist_display";



            // display
            adapter = getAdapter(Data.display);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, product_id, channel_id, media_type, attr_value_F, message_awareness_probability, " +
                    "message_persuation_probability, start_date, end_date, market_plan_id " +
                    "FROM dist_display ";

            // market_utility
            adapter = getAdapter(Data.market_utility);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, market_plan_id, product_id, channel_id, segment_id, percent_dist, " +
                    "awareness, persuasion, utility, start_date, end_date " +
                    "FROM market_utility";

            // mass_media
            adapter = getAdapter(Data.mass_media);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, product_id, channel_id, segment_id, media_type, attr_value_G, attr_value_H, " +
                    "attr_value_I, message_awareness_probability, message_persuation_probability, start_date, end_date, market_plan_id " +
                    "FROM mass_media";

            // product_attribute
            adapter = getAdapter(Data.product_attribute);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, product_attribute_id, product_attribute_name, utility_max, utility_min, cust_pref_max, cust_pref_min, cust_tau, type " +
                    "FROM product_attribute";

            // product_attribute_value
            adapter = getAdapter(Data.product_attribute_value);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, product_id, product_attribute_id, start_date, " +
                    "pre_attribute_value, post_attribute_value, has_attribute " +
                    "FROM product_attribute_value";

            // product_channel
            adapter = getAdapter(Data.product_channel);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, product_id, channel_id, markup, price, periodic_price, how_often, " +
                    "percent_SKU_in_dist, price_type, start_date, end_date, market_plan_id " +
                    "FROM product_channel";

            // product_matrix
            adapter = getAdapter(Data.product_matrix);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, have_product_id, want_product_id, value " +
                    "FROM product_matrix";

            // segment_channel
            adapter = getAdapter(Data.segment_channel);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, segment_id, channel_id, probability_of_choice " +
                    "FROM segment_channel";

            // share_pen_brand_aware
            adapter = getAdapter(Data.share_pen_brand_aware);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, product_id, segment_id, initial_share, penetration, brand_awareness, persuasion " +
                    "FROM share_pen_brand_aware";

            // product_event
            adapter = getAdapter(Data.product_event);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, segment_id, channel_id, product_id, demand_modification, start_date, end_date, market_plan_id, type " +
                    "FROM product_event";

            // task_event
            adapter = getAdapter(Data.task_event);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "record_id, model_id, segment_id, task_id, demand_modification, start_date, end_date, market_plan_id " +
                    "FROM task_event";

            // task
            adapter = getAdapter(Data.task);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, task_id, task_name, start_date, end_date, suitability_min, suitability_max " +
                    "FROM task";

            // task_product_fact
            adapter = getAdapter(Data.task_product_fact);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, product_id, task_id,  pre_use_upsku, post_use_upsku, suitability " +
                    "FROM task_product_fact";

            // task_rate_fact
            adapter = getAdapter(Data.task_rate_fact);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT " +
                    "model_id, segment_id, task_id, start_date, end_date, time_period, task_rate " +
                    "FROM task_rate_fact";

            // market_plan
            adapter = getAdapter(Data.market_plan);
            if (adapter != null)
                SqlQuery.InitializeMarketPlanSelect(adapter.SelectCommand);



            // market_plan_tree
            adapter = getAdapter(Data.market_plan_tree);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, parent_id, child_id " +
                    "FROM market_plan_tree";

            // scenario-marketplan relation
            adapter = getAdapter(Data.scenario_market_plan);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, scenario_id, market_plan_id " +
                    "FROM scenario_market_plan";

            // social network

            // network parameters
            adapter = getAdapter(Data.network_parameter);
            if (adapter != null)

                adapter.SelectCommand.CommandText = "SELECT model_id, id, name, type, persuasion_pre_use, persuasion_post_use, awareness_weight, " +
                    " num_contacts, prob_of_talking_pre_use, prob_of_talking_post_use, use_local, percent_talking, neg_persuasion_reject, neg_persuasion_pre_use, neg_persuasion_post_use " +
                    " FROM network_parameter ";

            // segment_network
            adapter = getAdapter(Data.segment_network);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, from_id, to_id, network_param FROM segment_network";

            // 
            // oleDbSelectCommand for external data
            // 
            adapter = getAdapter(Data.external_data);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, calendar_date, segment_id, product_id, channel_id, type, value " +
                    "FROM external_data ";


            // 
            // oleDbSelectCommand for model parameters
            // 
            adapter = getAdapter(Data.model_parameter);
            if (adapter != null)
                adapter.SelectCommand.CommandText = "SELECT model_id, id, name, table_name, col_name, filter, identity_row, row_id " +
                    "FROM model_parameter ";

             adapter = getAdapter(Data.simulation);
             if (adapter != null)
             {
                 SqlQuery.InitializeSimulationSelect(adapter.SelectCommand);
             }

            // 
            // oleDbSelectCommand for scenario parameter reference
            // 
            adapter = getAdapter(Data.scenario_parameter);
            if (adapter != null)
                SqlQuery.InitializeScenarioParameterSelect(adapter.SelectCommand);

            adapter = getAdapter(Data.scenario_variable);
            if (adapter != null)
                SqlQuery.InitializeScenarioVariableSelect(adapter.SelectCommand);

            adapter = getAdapter(Data.scenario_simseed);
            if (adapter != null)
                SqlQuery.InitializeScenarioSimSeedSelect(adapter.SelectCommand);

            adapter = getAdapter(Data.scenario_metric);
            if (adapter != null)
                SqlQuery.InitializeScenarioMetricSelect(adapter.SelectCommand);

            adapter = getAdapter(Data.sim_variable_value);
            if (adapter != null)
                SqlQuery.InitializeSimValueSelect(adapter.SelectCommand);
        }

        private void initializeAdapterUpdate() // DataTable table)
        {

            // NOTE if you are adding a table that has an auto id then you need
            // set the callback for retrieving  it from the db
            // there is a generic one, if the id is "id", or "record_id" 
            // if yours has a speific name you need to create
            // a new callback following the templates (see definition of id_RowUpdated
            // adapter.RowUpdated +=new OleDbRowUpdatedEventHandler(id_RowUpdated);

            // you also need to set the column as readOnly = false
            // this is done at the end of the routine
            // initializeIdColumnsReadStatus();

            OleDbDataAdapter adapter = null;

            adapter = getAdapter( Data.project );
            SqlQuery.InitializeProjectQuery( adapter );
            adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );

            // model info
            adapter = getAdapter( Data.Model_info );
            SqlQuery.InitializeModelQuery( adapter );
            adapter.RowUpdated += new OleDbRowUpdatedEventHandler( model_info_RowUpdated );


            // products
            adapter = getAdapter( Data.product );
            if( adapter != null )
            {
                SqlQuery.initialize_Product_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( product_RowUpdated );
            }

            // segments
            adapter = getAdapter( Data.segment );
            if( adapter != null )
            {
                SqlQuery.initialize_Segment_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( segment_RowUpdated );
            }

            // channels
            adapter = getAdapter( Data.channel );
            if( adapter != null )
            {
                SqlQuery.initialize_Channel_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( channel_RowUpdated );
            }

            // tasks
            adapter = getAdapter( Data.task );
            if( adapter != null )
            {
                SqlQuery.initialize_Task_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( task_RowUpdated );
            }

            // share_pen_brand_aware
            adapter = getAdapter( Data.share_pen_brand_aware );
            if( adapter != null )
            {
                SqlQuery.initialize_SharePenAwareness_UpdateQuery( adapter );
            }

            // segment_channel
            adapter = getAdapter( Data.segment_channel );
            if( adapter != null )
            {
                SqlQuery.initialize_SegmentChannel_UpdateQuery( adapter );
            }

            // task_product_fact
            adapter = getAdapter( Data.task_product_fact );
            if( adapter != null )
            {
                SqlQuery.initialize_TaskProd_UpdateQuery( adapter );
            }

            // task_rate_fact
            adapter = getAdapter( Data.task_rate_fact );
            if( adapter != null )
            {
                SqlQuery.initialize_TaskSegment_UpdateQuery( adapter );
            }

            // product_attributes
            adapter = getAdapter( Data.product_attribute );
            if( adapter != null )
            {
                SqlQuery.initialize_ProductAttributes_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( product_attribute_RowUpdated );

            }

            // product_attribute_value
            adapter = getAdapter( Data.product_attribute_value );
            if( adapter != null )
            {
                SqlQuery.initialize_ProdAttributeValues_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            // comsumer_preference
            adapter = getAdapter( Data.consumer_preference );
            if( adapter != null )
            {
                SqlQuery.initialize_ConsumerPref_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            // product_matrix
            adapter = getAdapter( Data.product_matrix );
            if( adapter != null )
            {
                SqlQuery.initialize_ProdDependencies_UpdateQuery( adapter );
            }

            // dist_display
            adapter = getAdapter( Data.distribution );
            if( adapter != null )
            {
                SqlQuery.initialize_Distribution_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            adapter = getAdapter( Data.display );
            if( adapter != null )
            {
                SqlQuery.initialize_Display_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            //market_utility
            adapter = getAdapter( Data.market_utility );
            if( adapter != null )
            {
                SqlQuery.initialize_Market_Utility_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }


            // mass_media
            adapter = getAdapter( Data.mass_media );
            if( adapter != null )
            {
                SqlQuery.initialize_MassMedia_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            // special_event
            adapter = getAdapter( Data.product_event );
            if( adapter != null )
            {
                SqlQuery.initialize_Product_Event_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            adapter = getAdapter( Data.task_event );
            if( adapter != null )
            {
                SqlQuery.initialize_Task_Event_UpdateQuery( adapter );

                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            // product_channel
            adapter = getAdapter( Data.product_channel );
            if( adapter != null )
            {
                SqlQuery.initialize_ProdChannel_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( record_RowUpdated );
            }

            // product_tree
            adapter = getAdapter( Data.product_tree );
            if( adapter != null )
            {
                SqlQuery.initialize_ProductTree_UpdateQuery( adapter );

            }
            // product_type
            adapter = getAdapter( Data.product_type );
            if( adapter != null )
            {
                SqlQuery.initialize_ProductType_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            // pack_size
            adapter = getAdapter( Data.pack_size );
            if( adapter != null )
            {
                SqlQuery.initialize_PackSize_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            // pack_size_ist
            adapter = getAdapter( Data.pack_size_dist );
            if( adapter != null )
            {
                SqlQuery.initialize_PackSizeDist_UpdateQuery( adapter );
            }


            // price_type
            adapter = getAdapter( Data.price_type );
            if( adapter != null )
            {
                SqlQuery.initialize_PriceType_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            // pack_size_ist
            adapter = getAdapter( Data.segment_price_utility );
            if( adapter != null )
            {
                SqlQuery.initialize_SegmentPriceUtility_UpdateQuery( adapter );
            }

            // merged from 2.2
            adapter = getAdapter( Data.product_channel_size );
            if( adapter != null )
            {
                SqlQuery.initialize_ProductChannelSize_UpdateQuery( adapter );
            }


            adapter = getAdapter( Data.market_plan );
            if( adapter != null )
            {
                SqlQuery.initialize_MarketPlan_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( market_plan_RowUpdated );
            }

            adapter = getAdapter( Data.market_plan_tree );
            if( adapter != null )
            {
                SqlQuery.initialize_MarketPlanTree_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.scenario_market_plan );
            if( adapter != null )
            {
                SqlQuery.initialize_ScenarioMarketPlan_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.network_parameter );
            if( adapter != null )
            {
                SqlQuery.initialize_network_parameter_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            adapter = getAdapter( Data.segment_network );
            if( adapter != null )
            {
                SqlQuery.initialize_segment_network_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.external_data );
            if( adapter != null )
            {
                SqlQuery.initialize_external_data_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.model_parameter );
            if( adapter != null )
            {
                SqlQuery.initialize_model_parameter_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            adapter = getAdapter( Data.scenario );
            if( adapter != null )
            {
                SqlQuery.Initialize_Scenario_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( scenario_RowUpdated );
            }

            adapter = getAdapter( Data.scenario_parameter );
            if( adapter != null )
            {
                SqlQuery.initialize_scenario_parameter_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.scenario_variable );
            if( adapter != null )
            {
                SqlQuery.initialize_scenario_variable_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            adapter = getAdapter( Data.scenario_simseed );
            if( adapter != null )
            {
                SqlQuery.initialize_scenario_simseed_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            adapter = getAdapter( Data.scenario_metric );
            if( adapter != null )
            {
                SqlQuery.initialize_scenario_metric_UpdateQuery( adapter );

            }
            adapter = getAdapter( Data.simulation );
            if( adapter != null )
            {
                SqlQuery.Initialize_Simulation_UpdateQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( id_RowUpdated );
            }

            adapter = getAdapter( Data.sim_queue );
            if( adapter != null )
            {
                SqlQuery.InitializeSimQueueQuery( adapter );
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( sim_queue_RowUpdated );
            }

            adapter = getAdapter( Data.sim_variable_value );
            if( adapter != null )
            {
                SqlQuery.initialize_sim_variable_values_UpdateQuery( adapter );

            }

            // NOTE if you are adding a table that has an auto id then you need
            // set the callback for retrieving  it from the db
            // there is a generic one, if the id is "id", or "record_id" 
            // if yours has a speific name you need to create
            // a new callback following the templates (see definition of id_RowUpdated
            // adapter.RowUpdated +=new OleDbRowUpdatedEventHandler(id_RowUpdated);
            // you also need to set the column as readOnly = false
            // this is done at the end of the routine "initializeIdColumnsReadStatus()"
        }


        #endregion

        #region Dynamic Expressions

        // private DataColumn segment_mean_pack_size;

        // this looks like a hack - and i wrote it! SSN

        // product
        //private DataColumn product_brandName;
        //private DataColumn product_typeName;

        // sim_queue_scenario
        private DataColumn sim_queue_scenarioID;
        // product_channel
        private DataColumn productChannel_channelName;
        private DataColumn productChannel_productName;
        //private DataColumn productChannel_brandID;

        // task_product
        private DataColumn taskProduct_productName;
        private DataColumn taskSegment_segmentName;
        private DataColumn taskSegment_taskName;
        private DataColumn taskProduct_taskName;


        // attributes
        private DataColumn attribute_productName;
        private DataColumn attribute_segmentName;
        private DataColumn attribute_product_attributeName;
        private DataColumn attribute_segment_attributeName;

        // product matrix
        private DataColumn productproduct_matrix_have;
        private DataColumn productproduct_matrix_want;

        //mass media
        private DataColumn mass_media_productName;
        private DataColumn mass_media_segmentName;
        private DataColumn mass_media_channelName;

        //display
        private DataColumn display_productName;
        private DataColumn display_channelName;

        //distribution
        private DataColumn distribution_productName;
        private DataColumn distribution_channelName;

        // product events
        private DataColumn product_event_productName;
        private DataColumn product_event_channelName;
        private DataColumn product_event_segmentName;

        // task events
        private DataColumn task_event_taskName;
        private DataColumn task_event_segmentName;

        private DataColumn segment_network_fromSegmentName;
        private DataColumn segment_network_toSegmentName;

        // private DataColumn network_parameter_type;

        //		private DataColumn market_plan_plan_type;
        //
        //		private DataColumn market_plan_child_name;
        //		private DataColumn market_plan_product_name;
        //		private DataColumn scenario_market_plan_product_name;
        //		private DataColumn scenario_market_plan_marketplan_name;
        //		private DataColumn scenario_market_plan_marketplan_type;

        private void createExpressionColumn()
        {
            //brandName column
            /*product_brandName = new DataColumn();
            product_brandName.ColumnName = "brandName";
            product_brandName.DataType = typeof(string);

            product_brandName.Expression = "parent(brandproduct).brand_name";*/

            /*product_typeName = new DataColumn();
            product_typeName.ColumnName = "type_name";
            product_typeName.DataType = typeof(string);*/

            //product_typeName.Expression = "parent(productproduct_type).type_name";

            // product Channel needs names

            // sim_queue scenario
            sim_queue_scenarioID = new DataColumn();
            sim_queue_scenarioID.ColumnName = "scenario_id";
            sim_queue_scenarioID.DataType = typeof(int);

            sim_queue_scenarioID.Expression = "parent(SimulationSimQueue).scenario_id";

            //channelName 
            productChannel_channelName = new DataColumn();
            productChannel_channelName.ColumnName = "channelName";
            productChannel_channelName.DataType = typeof(string);

            productChannel_channelName.Expression = "parent(channelproduct_channel).channel_name";


            //productName
            productChannel_productName = new DataColumn();
            productChannel_productName.ColumnName = "productName";
            productChannel_productName.DataType = typeof(string);

            productChannel_productName.Expression = "parent(productproduct_channel).product_name";


            // brand id for filtering
            //productChannel_brandID = new DataColumn();
            //productChannel_brandID.ColumnName = "brand_id";
            //productChannel_brandID.DataType = typeof(int);

            //productChannel_brandID.Expression = "parent(productproduct_channel).brand_id";

            // task product

            taskProduct_productName = new DataColumn();
            taskProduct_productName.ColumnName = "product_name";
            taskProduct_productName.DataType = typeof(string);

            taskProduct_productName.Expression = "parent(producttask_product_fact).product_name";

            // task product -- task

            taskProduct_taskName = new DataColumn();
            taskProduct_taskName.ColumnName = "task_name";
            taskProduct_taskName.DataType = typeof(string);

            taskProduct_taskName.Expression = "parent(tasktask_product_fact).task_name";

            // task segment
            taskSegment_segmentName = new DataColumn();
            taskSegment_segmentName.ColumnName = "segment_name";
            taskSegment_segmentName.DataType = typeof(string);

            taskSegment_segmentName.Expression = "parent(segmenttask_rate_fact).segment_name";

            // task segment -- task
            taskSegment_taskName = new DataColumn();
            taskSegment_taskName.ColumnName = "task_name";
            taskSegment_taskName.DataType = typeof(string);

            taskSegment_taskName.Expression = "parent(tasktask_rate_fact).task_name";

            //attributes

            // attribute product
            attribute_productName = new DataColumn();
            attribute_productName.ColumnName = "product_name";
            attribute_productName.DataType = typeof(string);

            attribute_productName.Expression = "parent(productproduct_attribute_value).product_name";

            attribute_product_attributeName = new DataColumn();
            attribute_product_attributeName.ColumnName = "attribute_name";
            attribute_product_attributeName.DataType = typeof(string);

            attribute_product_attributeName.Expression = "parent(product_attributeproduct_attribute_value).product_attribute_name";

            // task segment
            attribute_segmentName = new DataColumn();
            attribute_segmentName.ColumnName = "segment_name";
            attribute_segmentName.DataType = typeof(string);

            attribute_segmentName.Expression = "parent(segmentconsumer_preference).segment_name";

            attribute_segment_attributeName = new DataColumn();
            attribute_segment_attributeName.ColumnName = "attribute_name";
            attribute_segment_attributeName.DataType = typeof(string);

            attribute_segment_attributeName.Expression = "parent(product_attributeconsumer_preference).product_attribute_name";

            // product matrix

            // have product
            productproduct_matrix_have = new DataColumn();
            productproduct_matrix_have.ColumnName = "have_product_name";
            productproduct_matrix_have.DataType = typeof(string);

            productproduct_matrix_have.Expression = "parent(productproduct_matrix_have).product_name";


            // want product
            productproduct_matrix_want = new DataColumn();
            productproduct_matrix_want.ColumnName = "want_product_name";
            productproduct_matrix_want.DataType = typeof(string);

            productproduct_matrix_want.Expression = "parent(productproduct_matrix_want).product_name";

            //
            // mass media
            //

            // product
            mass_media_productName = new DataColumn();
            mass_media_productName.ColumnName = "product_name";
            mass_media_productName.DataType = typeof(string);

            mass_media_productName.Expression = "parent(productmass_media).product_name";

            // segment
            mass_media_segmentName = new DataColumn();
            mass_media_segmentName.ColumnName = "segment_name";
            mass_media_segmentName.DataType = typeof(string);

            mass_media_segmentName.Expression = "parent(segmentmass_media).segment_name";

            // channel
            mass_media_channelName = new DataColumn();
            mass_media_channelName.ColumnName = "channel_name";
            mass_media_channelName.DataType = typeof(string);

            mass_media_channelName.Expression = "parent(channelmass_media).channel_name";


            //display

            // channel
            display_channelName = new DataColumn();
            display_channelName.ColumnName = "channel_name";
            display_channelName.DataType = typeof(string);

            display_channelName.Expression = "parent(channeldisplay).channel_name";

            // product
            display_productName = new DataColumn();
            display_productName.ColumnName = "product_name";
            display_productName.DataType = typeof(string);

            display_productName.Expression = "parent(productdisplay).product_name";

            //
            //distribution
            //

            // channel
            distribution_channelName = new DataColumn();
            distribution_channelName.ColumnName = "channel_name";
            distribution_channelName.DataType = typeof(string);

            distribution_channelName.Expression = "parent(channeldistribution).channel_name";

            // product
            distribution_productName = new DataColumn();
            distribution_productName.ColumnName = "product_name";
            distribution_productName.DataType = typeof(string);

            distribution_productName.Expression = "parent(productdistribution).product_name";

            // product events
            product_event_productName = new DataColumn();
            product_event_productName.ColumnName = "product_name";
            product_event_productName.DataType = typeof(string);
            product_event_productName.Expression = "parent(productproduct_event).product_name";


            product_event_channelName = new DataColumn();
            product_event_channelName.ColumnName = "channel_name";
            product_event_channelName.DataType = typeof(string);
            product_event_channelName.Expression = "parent(channelproduct_event).channel_name";

            product_event_segmentName = new DataColumn();
            product_event_segmentName.ColumnName = "segment_name";
            product_event_segmentName.DataType = typeof(string);
            product_event_segmentName.Expression = "parent(segmentproduct_event).segment_name";

            // task events
            task_event_taskName = new DataColumn();
            task_event_taskName.ColumnName = "task_name";
            task_event_taskName.DataType = typeof(string);
            task_event_taskName.Expression = "parent(tasktask_event).task_name";


            task_event_segmentName = new DataColumn();
            task_event_segmentName.ColumnName = "segment_name";
            task_event_segmentName.DataType = typeof(string);
            task_event_segmentName.Expression = "parent(segmenttask_event).segment_name";

            // social network
            segment_network_fromSegmentName = new DataColumn();
            segment_network_fromSegmentName.ColumnName = "from_segment_name";
            segment_network_fromSegmentName.DataType = typeof(string);
            segment_network_fromSegmentName.Expression = "parent(fromsegmentsegment_network).segment_name";

            segment_network_toSegmentName = new DataColumn();
            segment_network_toSegmentName.ColumnName = "to_segment_name";
            segment_network_toSegmentName.DataType = typeof(string);
            segment_network_toSegmentName.Expression = "parent(tosegmentsegment_network).segment_name";


            //			//market plans
            //			market_plan_plan_type = new DataColumn();
            //			market_plan_plan_type.ColumnName = "plan_type";
            //			market_plan_plan_type.DataType = typeof(string);
            //			market_plan_plan_type.Expression = "parent(market_plan_typemarket_plan).type";
            //
            //			// market_plan_tree
            //			market_plan_child_name = new DataColumn();
            //			market_plan_child_name.ColumnName = "name";
            //			market_plan_child_name.DataType = typeof(string);
            //			market_plan_child_name.Expression = "parent(market_planmarket_plan_tree_child).name";
            //
            //			// market_plan_product_name
            //			market_plan_product_name = new DataColumn();
            //			market_plan_product_name.ColumnName = "product_name";
            //			market_plan_product_name.DataType = typeof(string);
            //			market_plan_product_name.Expression = "parent(productmarket_plan).product_name";
            //
            //			// scenario_market_plan_marketplan_name
            //			scenario_market_plan_marketplan_name = new DataColumn();
            //			scenario_market_plan_marketplan_name.ColumnName = "name";
            //			scenario_market_plan_marketplan_name.DataType = typeof(string);
            //			scenario_market_plan_marketplan_name.Expression = "parent(market_planscenario_market_plan).name";
            //
            //			// scenario_market_plan_product_name
            //			scenario_market_plan_product_name = new DataColumn();
            //			scenario_market_plan_product_name.ColumnName = "product_name";
            //			scenario_market_plan_product_name.DataType = typeof(string);
            //			scenario_market_plan_product_name.Expression = "parent(market_planscenario_market_plan).product_name";
            //
            //			// scenario_market_plan_marketplan_type
            //			scenario_market_plan_marketplan_type = new DataColumn();
            //			scenario_market_plan_marketplan_type.ColumnName = "type";
            //			scenario_market_plan_marketplan_type.DataType = typeof(byte);
            //			scenario_market_plan_marketplan_type.Expression = "parent(market_planscenario_market_plan).type";


        }

        private void addExpressionColumns()
        {
            // sim_queue
            Data.sim_queue.Columns.Add(sim_queue_scenarioID);

            //brandName column
            //Data.product.Columns.Add(product_brandName);
            //Data.product.Columns.Add(product_typeName);

            // product Channel needs names

            //channelName 
            Data.product_channel.Columns.Add(productChannel_channelName);

            //productName
            Data.product_channel.Columns.Add(productChannel_productName);

            // brand id for filtering
            //Data.product_channel.Columns.Add(productChannel_brandID);

            // product name for task_product_fact
            Data.task_product_fact.Columns.Add(taskProduct_productName);
            Data.task_product_fact.Columns.Add(taskProduct_taskName);


            // segment name for task_rate_fact
            Data.task_rate_fact.Columns.Add(taskSegment_segmentName);
            Data.task_rate_fact.Columns.Add(taskSegment_taskName);

            // attributes
            Data.product_attribute_value.Columns.Add(attribute_productName);
            Data.product_attribute_value.Columns.Add(attribute_product_attributeName);

            Data.consumer_preference.Columns.Add(attribute_segmentName);
            Data.consumer_preference.Columns.Add(attribute_segment_attributeName);

            // product matrix
            Data.product_matrix.Columns.Add(productproduct_matrix_have);
            Data.product_matrix.Columns.Add(productproduct_matrix_want);

            // mass media
            Data.mass_media.Columns.Add(mass_media_productName);
            Data.mass_media.Columns.Add(mass_media_segmentName);
            Data.mass_media.Columns.Add(mass_media_channelName);

            // display
            Data.display.Columns.Add(display_channelName);
            Data.display.Columns.Add(display_productName);

            // distribution
            Data.distribution.Columns.Add(distribution_channelName);
            Data.distribution.Columns.Add(distribution_productName);


            // product events
            Data.product_event.Columns.Add(product_event_productName);
            Data.product_event.Columns.Add(product_event_channelName);
            Data.product_event.Columns.Add(product_event_segmentName);


            // task events
            Data.task_event.Columns.Add(task_event_taskName);
            Data.task_event.Columns.Add(task_event_segmentName);

            // social network
            Data.segment_network.Columns.Add(segment_network_fromSegmentName);
            Data.segment_network.Columns.Add(segment_network_toSegmentName);

            //			Data.network_parameter.Columns.Add(network_parameter_type);

            //			// market plans
            //			Data.market_plan.Columns.Add(market_plan_plan_type);
            //
            //			// marketing plan tree
            //			Data.market_plan_tree.Columns.Add(market_plan_child_name);
            //
            //			Data.market_plan.Columns.Add(market_plan_product_name);
            //
            //			// scenario marketing plan
            //			Data.scenario_market_plan.Columns.Add(scenario_market_plan_product_name);
            //			Data.scenario_market_plan.Columns.Add(scenario_market_plan_marketplan_name);
            //			Data.scenario_market_plan.Columns.Add(scenario_market_plan_marketplan_type);
        }

        private void removeExpressionColumns()
        {
            try
            {
                Data.sim_queue.Columns.Remove( sim_queue_scenarioID );
            }
            catch( Exception ) { }


            //brandName column
            //Data.product.Columns.Remove(product_brandName);
            //Data.product.Columns.Remove(product_typeName);

            // product Channel needs names

            //channelName 
            try
            {
                Data.product_channel.Columns.Remove( productChannel_channelName );
            }
            catch( Exception ) { }

            //productName
            try
            {
                Data.product_channel.Columns.Remove( productChannel_productName );
            }
            catch( Exception ) { }

            // brand id for filtering
            //Data.product_channel.Columns.Remove(productChannel_brandID);

            // product name for task_product_fact
            try
            {
                Data.task_product_fact.Columns.Remove( taskProduct_productName );
            }
            catch( Exception ) { }
            try
            {
                Data.task_product_fact.Columns.Remove( taskProduct_taskName );
            }
            catch( Exception ) { }


            // segment name for task_rate_fact
            try
            {
                Data.task_rate_fact.Columns.Remove( taskSegment_segmentName );
            }
            catch( Exception ) { }
            try
            {
                Data.task_rate_fact.Columns.Remove( taskSegment_taskName );
            }
            catch( Exception ) { }

            //attributes
            try
            {
                Data.product_attribute_value.Columns.Remove( attribute_productName );
            }
            catch( Exception ) { }
            try
            {
                Data.product_attribute_value.Columns.Remove( attribute_product_attributeName );
            }
            catch( Exception ) { }

            try
            {
                Data.consumer_preference.Columns.Remove( attribute_segmentName );
            }
            catch( Exception ) { }
            try
            {
                Data.consumer_preference.Columns.Remove( attribute_segment_attributeName );
            }
            catch( Exception ) { }

            // product matrix
            try
            {
                Data.product_matrix.Columns.Remove( productproduct_matrix_have );
            }
            catch( Exception ) { }
            try
            {
                Data.product_matrix.Columns.Remove( productproduct_matrix_want );
            }
            catch( Exception ) { }

            // mass media
            try
            {
                Data.mass_media.Columns.Remove( mass_media_productName );
            }
            catch( Exception ) { }
            try
            {
                Data.mass_media.Columns.Remove( mass_media_segmentName );
            }
            catch( Exception ) { }
            try
            {
                Data.mass_media.Columns.Remove( mass_media_channelName );
            }
            catch( Exception ) { }

            // display
            try
            {
                Data.display.Columns.Remove( display_channelName );
            }
            catch( Exception ) { }
            try
            {
                Data.display.Columns.Remove( display_productName );
            }
            catch( Exception ) { }

            // distribution
            try
            {
                Data.distribution.Columns.Remove( distribution_channelName );
            }
            catch( Exception ) { }
            try
            {
                Data.distribution.Columns.Remove( distribution_productName );
            }
            catch( Exception ) { }

            // product events
            try
            {
                Data.product_event.Columns.Remove( product_event_productName );
            }
            catch( Exception ) { }
            try
            {
                Data.product_event.Columns.Remove( product_event_channelName );
            }
            catch( Exception ) { }
            try
            {
                Data.product_event.Columns.Remove( product_event_segmentName );
            }
            catch( Exception ) { }

            // task events
            try
            {
                Data.task_event.Columns.Remove( task_event_taskName );
            }
            catch( Exception ) { }
            try
            {
                Data.task_event.Columns.Remove( task_event_segmentName );
            }
            catch( Exception ) { }

            // social network
            try
            {
                Data.segment_network.Columns.Remove( segment_network_fromSegmentName );
            }
            catch( Exception ) { }
            try
            {
                Data.segment_network.Columns.Remove( segment_network_toSegmentName );
            }
            catch( Exception ) { }

            //			Data.network_parameter.Columns.Remove(network_parameter_type);

            //			// scenario_market_plan
            //			Data.scenario_market_plan.Columns.Remove(scenario_market_plan_marketplan_name);
            //			Data.scenario_market_plan.Columns.Remove(scenario_market_plan_product_name);
            //			Data.scenario_market_plan.Columns.Remove(scenario_market_plan_marketplan_type);
            //
            //			// market plans
            //			Data.market_plan.Columns.Remove(market_plan_plan_type);
            //			Data.market_plan.Columns.Remove(market_plan_product_name);
            //
            //
            //			// marketing plan tree
            //			Data.market_plan_tree.Columns.Remove(market_plan_child_name);



        }


        #endregion

        #region Special All Objects

        private void createItemsAsAll()
        {
            if( getAdapter( Data.price_type ) != null )
            {
                foreach( MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows )
                {
                    CreateUnpromotedPriceType( model );
                }
            }

            if (getAdapter(Data.pack_size) != null)
            { 
                foreach (MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows)
                {
                    CreateDefaultPackSize( model );
                }
            }

            if (getAdapter(Data.product_type) != null)
            {
                foreach (MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows)
                {
                    createAllProductType(model);
                }
            }

            if (getAdapter(Data.product) != null)
            {
                foreach (MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows)
                {
                    createAllProduct(model);
                }
            }

            if (getAdapter(Data.segment) != null)
            {
                foreach (MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows)
                {
                    createAllSegment(model);
                }
            }

            if (getAdapter(Data.channel) != null)
            {
                foreach (MrktSimDBSchema.Model_infoRow model in Data.Model_info.Rows)
                {
                    createAllChannel(model);
                }
            }

            if (Data.task.FindBytask_id(AllID) == null && Data.Model_info.Rows.Count > 0)
            {
                createAllTask( (MrktSimDBSchema.Model_infoRow) Data.Model_info.Rows[0]);
            }

            AcceptItemsMarkedAll();
        }

        public void AcceptItemsMarkedAll()
        {
            DataRow[] rows;
            string query = "id = " + AllID;


            rows = this.Data.price_type.Select( query );
            foreach( DataRow row in rows )
                row.AcceptChanges();

            rows = this.Data.pack_size.Select( query );
            foreach( DataRow row in rows )
                row.AcceptChanges();

            rows = this.Data.product_type.Select(query);
            foreach(DataRow row in rows)
                row.AcceptChanges();

            query = "product_id = " + AllID;
            rows = this.Data.product.Select(query);
            foreach (DataRow row in rows)
                row.AcceptChanges();

            query = "segment_id = " + AllID;
            rows = this.Data.segment.Select(query);
            foreach (DataRow row in rows)
                row.AcceptChanges();

            query = "channel_id = " + AllID;
            rows = this.Data.channel.Select(query);
            foreach (DataRow row in rows)
                row.AcceptChanges();

            query = "task_id = " + AllID;
            rows = this.Data.task.Select(query);
            foreach (DataRow row in rows)
                row.AcceptChanges();
        }

        public MrktSimDBSchema.price_typeRow CreateUnpromotedPriceType( MrktSimDBSchema.Model_infoRow model )
        {
            MrktSimDBSchema.price_typeRow row = Data.price_type.Newprice_typeRow();

            row.model_id = model.model_id;
            row.id = AllID;
            row.awareness = 0;
            row.persuasion = 0;
            row.relative = false;
            row.BOGN = 0;

            row.name = "Unpromoted";

            Data.price_type.Addprice_typeRow( row );

            return row;
        }

        public MrktSimDBSchema.pack_sizeRow CreateDefaultPackSize( MrktSimDBSchema.Model_infoRow model )
        {
            MrktSimDBSchema.pack_sizeRow row = Data.pack_size.Newpack_sizeRow();

            row.model_id = model.model_id;
            row.id = AllID;
            row.name = "Default Pack Size";

            Data.pack_size.Addpack_sizeRow( row );

            return row;
        }

        private MrktSimDBSchema.product_typeRow createAllProductType(MrktSimDBSchema.Model_infoRow model)
        {
            MrktSimDBSchema.product_typeRow row = Data.product_type.Newproduct_typeRow();
            row.type_name = "Category";    //fixed typo - JimJ
            row.model_id = model.model_id;
            row.id = AllID;

            Data.product_type.Addproduct_typeRow(row);

            return row;
        }

        private MrktSimDBSchema.productRow createAllProduct(MrktSimDBSchema.Model_infoRow model)
        {
            // create a new product
            // create product within this brand
            MrktSimDBSchema.productRow row = Data.product.NewproductRow();

            row.model_id = model.model_id;
            row.brand_id = Database.AllID;
            row.product_id = AllID;

            // default values
            row.product_name = "All";
            row.type = "Initial";
            row.product_group = "1";
            row.related_group = "none";
            row.percent_relation = "irrellevant";
            row.cost = 0;
            row.initial_dislike_probability = 0;
            row.repeat_like_probability = 100;
            row.color = "Yellow";
            row.product_type = AllID;
            row.pack_size_id = -1;

            Data.product.AddproductRow(row);

            int productId = row.product_id;

            return row;
        }



        private void createAllSegment(MrktSimDBSchema.Model_infoRow model)
        {
            // create a new segment
            MrktSimDBSchema.segmentRow row = Data.segment.NewsegmentRow();
            
            // default values
            row.model_id = model.model_id;
            row.segment_name = "All";
            row.segment_id = AllID;
            row.segment_model = "m";
            row.color = "Red";
            row.show_current_share_pie_chart = "Yes";
            row.show_cmulative_share_chart = "Yes";
            row.segment_size = 100;
            row.growth_rate = 0;
            row.growth_rate_people_percent = "people";
            row.growth_rate_month_year = "month";
            row.compress_population = "yes";
            row.variability = 0;
            row.price_disutility = 3;
            row.attribute_sensitivity = 1;
            row.persuasion_scaling = 1;
            row.display_utility = 1;
            row.display_utility_scaling_factor = 1;
            row.max_display_hits_per_trip = 2;
            row.inertia = 0;
            row.repurchase = "Yes";
            row.repurchase_model = "R";
            row.gamma_location_parameter_a = 8.543;
            row.gamma_shape_parameter_k = 0.615;
            row.repurchase_period_frequency = 12;
            row.repurchase_frequency_variation = 1;
            row.repurchase_timescale = "Months";
            row.avg_max_units_purch = 1;
            row.shopping_trip_interval = 52;
            row.category_penetration = 100;
            row.category_rejection = 0;
            row.num_initial_buyers = 0;
            row.initial_buying_period = 10;
            row.seed_with_repurchasers = "Yes";
            row.use_budget = "No";
            row.budget = 100;
            row.budget_period = "Months";
            row.save_unspent = "No";
            row.initial_savings = 100;
            row.social_network_model = "Talk-Anytime";
            row.num_close_contacts = 0.0;
            row.prob_talking_close_contact_pre = 1;
            row.prob_talking_close_contact_post = 1;
            row.use_local = "No";
            row.num_distant_contacts = 0;
            row.prob_talk_distant_contact_pre = 1;
            row.prob_talk_distant_contact_post = 1;
            row.awareness_weight_personal_message = 0;
            row.pre_persuasion_prob = 0;
            row.post_persuasion_prob = 0;
            row.units_desired_trigger = 0;
            row.awareness_model = "Persuasion & Awareness";
            row.awareness_threshold = 0;
            row.awareness_decay_rate_pre = 0;
            row.awareness_decay_rate_post = 0;
            row.persuasion_decay_rate_pre = 0;
            row.persuasion_decay_rate_post = 0;
            row.persuasion_decay_method = "B: % agents losing 1 unit";
            row.product_choice_model = "N";
            row.persuasion_score = "*";
            row.persuasion_value_computation = "Absolute";
            row.persuasion_contribution_overall_score = "+";
            row.utility_score = "*";
            row.combination_part_utilities = "Scaled Sum of Products";
            row.price_contribution_overall_score = "+";
            row.price_score = "*";
            row.price_value = "Absolute";
            row.reference_price = 0;
            row.choice_prob = "Logit";
            row.inertia_model = "Brand";
            row.error_term = "None";
            row.error_term_user_value = 1;
            row.loyalty = 0;
            row.min_freq = 0.0;
            row.max_freq = 9999.0;

            Data.segment.AddsegmentRow(row);
        }


        private void createAllChannel(MrktSimDBSchema.Model_infoRow model)
        {
            // create a new channel
            MrktSimDBSchema.channelRow row = Data.channel.NewchannelRow();

            row.model_id = model.model_id;
            row.channel_id = AllID;

            // default values
            row.channel_name = "All";

            Data.channel.AddchannelRow(row);
        }

        private void createAllTask(MrktSimDBSchema.Model_infoRow model)
        {
            MrktSimDBSchema.taskRow row = Data.task.NewtaskRow();

            // initialize
            row.model_id = model.model_id;

            row.task_name = "All";
            row.task_id = AllID;

            row.start_date = model.start_date;
            row.end_date = model.end_date;
            row.suitability_min = 0;
            row.suitability_max = 1;

            Data.task.AddtaskRow(row);
        }

        #endregion

        /// <summary>
        /// Parameters need to be treated carefully
        /// </summary>
        #region Parameter Routines


  

        // chacks parms against rows and deletes if needed
        public void UpdateParameters()
        {
            DataRow[] parmRows = Data.model_parameter.Select("", "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.model_parameterRow parm in parmRows)
            {
                // validate parm
                DataRow row = getParmRow(parm);

                if (row == null)
                {
                    // this is not a valid parm
                    parm.Delete();
                }
            }

        }

        public static double convert2Double(Object obj)
        {
            if (obj.GetType() == typeof(double))
                return (double)obj;

            else if (obj.GetType() == typeof(int))
            {
                int ival = (int)obj;

                return (double)ival;
            }

            double val = double.Parse(obj.ToString());
            return val;
        }


        // get the row for this parm
        // returns null if filter is bad
        private DataRow getParmRow(MrktSimDBSchema.model_parameterRow parm)
        {
            // first check that this does not exist already
            DataTable table = Data.Tables[parm.table_name];

            if (table == null)
                return null;

            DataRow[] rows = table.Select(parm.filter, "", DataViewRowState.CurrentRows);

            if (rows.Length != 1)
                return null;

            return rows[0];
        }

        public double EvaluateModelParameter(MrktSimDBSchema.model_parameterRow parm)
        {
            object obj = null;

            genericCommand.CommandText = " SELECT " + parm.col_name + " FROM " + parm.table_name + " WITH (NOLOCK) WHERE " + parm.filter;

            genericCommand.Connection.Open();
            try
            {
                obj = genericCommand.ExecuteScalar();
            }
            catch (Exception )
            {
                return 0.0;
            }

            genericCommand.Connection.Close();

            if (obj != null && obj.GetType() != typeof(System.DBNull))
            {
                return Convert.ToDouble(obj);
            }

            return 0.0;
        }

        public void ApplySimulationlParameter(MrktSimDBSchema.scenario_parameterRow scenarioParm)
        {
            MrktSimDBSchema.model_parameterRow parm = scenarioParm.model_parameterRow;

            if (parm == null)
                return;

            genericCommand.CommandText = " UPDATE " + parm.table_name + " SET " + parm.col_name +
                " = " + scenarioParm.aValue.ToString();

            if (parm.filter != null)
            {
                genericCommand.CommandText += " WHERE " + parm.filter;
            }

            genericCommand.Connection.Open();
            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (Exception )
            {
            }

            genericCommand.Connection.Close();
        }


        // one routine generates the rows
        // the other checks them after a potential change to row_ids
        // so it is important that no parms get deleted between the two operations
        // ideally the parm should have a reference to the row
        // but I am not sure how to do that cleanly as parms are part of the
        // dataset and the class is generated
        // the clean way to do this will be to create a class which carries an instane of both
        // then checks
        // SSN 6-12-05
        private List<DataRowPair> getParmRows()
        {
            DataRow[] parms = Data.model_parameter.Select("", "id", DataViewRowState.CurrentRows);

            List<DataRowPair> parmRows = new List<DataRowPair>();

            foreach( MrktSimDBSchema.model_parameterRow parm in parms )
            {
                DataRowPair pair = new DataRowPair();

                pair.parent = parm;
                pair.child = getParmRow(parm);

                parmRows.Add( pair );

            }

            return parmRows;
        }

        private void checkParmRowID( List<DataRowPair> parmRows )
        {

            foreach( DataRowPair pair in parmRows ) {

                MrktSimDBSchema.model_parameterRow parm = (MrktSimDBSchema.model_parameterRow)pair.parent;
                DataRow row = pair.child;

                object idObj = row[ parm.identity_row ];

                int row_id = (int)idObj;

                // only change if we have to
                // in future we may only generate list of new items
                // but then we will need to generate list of parms with new items
                // see note above
                if( row_id != parm.row_id ) {
                    // fix this and filter
                    parm.row_id = row_id;
                    parm.filter = parm.identity_row + " = " + row_id;
                }
            }
        }

        // checks if model parameter already exists
        // returns null if not
        public MrktSimDBSchema.model_parameterRow ModelParameterExists(DataRow theRow, string colName, string identity_row)
        {
            string tableName = theRow.Table.TableName;

            // what is the "id" of this row
            object idObj = theRow[identity_row];

            // we need a way to signal errors better, but for now...
            // (famous last words)
            if (idObj == null)
                return null;

            // this will cause a crash if not correct, which is good
            int row_id = (int)idObj;

            // now check if parameter already exists
            string query = "table_name = '" + tableName + "'" +
                " AND col_name = '" + colName + "'" +
                " AND identity_row = '" + identity_row + "'" +
                " AND row_id = " + row_id;

            DataRow[] parmRows = Data.model_parameter.Select(query, "", DataViewRowState.CurrentRows);

            if (parmRows.Length > 0)
                return (MrktSimDBSchema.model_parameterRow)parmRows[0];

            return null;
        }

        public MrktSimDBSchema.model_parameterRow
            CreateModelParameter(int model_id, DataRow theRow, string colName, string identity_row)
        {
            MrktSimDBSchema.model_parameterRow parameter = ModelParameterExists(theRow, colName, identity_row);

            if (parameter != null)
                return parameter;

            string tableName = theRow.Table.TableName;

            // first check that this does not exist already
            DataTable table = Data.Tables[tableName];

            // ok this wasn't even a table in our schema
            if (table == null)
                return null;

            // what is the "id" of this row

            object idObj = theRow[identity_row];

            // we need a way to signal errors better, but for now...
            // (famous last words)
            if (idObj == null)
                return null;

            // this will cause a crash if not correct, which is good
            int row_id = (int)idObj;

            // create filter
            string filter = identity_row + " = " + row_id;

            // check table
            DataRow[] rows = table.Select(filter, "", DataViewRowState.CurrentRows);

            if (rows.Length != 1)
                return null;

            // houston we have a problem
            if (rows[0] != theRow)
                return null;

            // there is bonefide row for the filter
            object obj = theRow[colName];

            // so close...
            if (obj == null)
                return null;

            // this needs to be a numeric value
            double aVal = convert2Double(obj);

            // now check if parameter already exists
            string query = "table_name = '" + tableName + "'" +
                " AND col_name = '" + colName + "'" +
                " AND identity_row = '" + identity_row + "'" +
                " AND row_id = " + row_id;

            DataRow[] parmRows = Data.model_parameter.Select(query, "", DataViewRowState.CurrentRows);

            if (parmRows.Length > 0)
                return (MrktSimDBSchema.model_parameterRow)parmRows[0];

            parameter = Data.model_parameter.Newmodel_parameterRow();

            parameter.model_id = model_id;
            parameter.table_name = tableName;
            parameter.col_name = colName;
            parameter.filter = filter;
            parameter.name = tableName + "_" + row_id + "_" + colName;
            parameter.identity_row = identity_row;
            parameter.row_id = row_id;

            Data.model_parameter.Addmodel_parameterRow(parameter);

            return parameter;
        }

        #endregion


        #region database insertion Callbacks
        // What are all these callbacks?
        //
        // This was put here because of an issue with ADO and updating after an insertion
        // There is also an issue in that not all db's allow an update at the timof insertion
        // also the update command could change with db.
        //
        // If you add another table and it has a unique id generated by the DB then you need to
        // add a callback
        // or you could call the ID record_id and use the existing func.
        //
        //
        // There was also an attempt to not have to remove the dynamic columns added
        // to the dataset - but when the columns are left in we get an error about the wrong version
        // haven't figured out why an added readonly column should cause this error
        //
        // SSN 12/21/2004

        private int update()
        {
            Object obj = updateCommand.ExecuteScalar();

            if (obj == null)
            {
                throw (new System.Exception("MrktSimError: Failed to insert Object"));
            }

            int rval;

            try
            {
                rval = (int)Decimal.Parse(obj.ToString());
            }
            catch (System.Exception err)
            {
                string message = "MrktSimError: " + err.Message + " (Check if database was properly updated)";
                throw (new System.Exception(message));
            }

            return rval;
        }

        private void sim_queue_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.sim_queueRow row = (MrktSimDBSchema.sim_queueRow)e.Row;
            row.run_id = update();
            e.Row.AcceptChanges();
        }

        private void product_attribute_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.product_attributeRow row = (MrktSimDBSchema.product_attributeRow)e.Row;
            row.product_attribute_id = update();
            e.Row.AcceptChanges();
        }

        private void model_info_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.Model_infoRow row = (MrktSimDBSchema.Model_infoRow)e.Row;
            row.model_id = update();
            e.Row.AcceptChanges();
        }


        /*private void brand_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.brandRow row = (MrktSimDBSchema.brandRow) e.Row;
            row.AcceptChanges();
            row.brand_id = update();
            e.Row.AcceptChanges();
        }*/


        private void product_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow)e.Row;
            row.product_id = update();
            e.Row.AcceptChanges();
        }

        private void channel_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.channelRow row = (MrktSimDBSchema.channelRow)e.Row;
            row.channel_id = update();
            e.Row.AcceptChanges();
        }

        private void segment_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.segmentRow row = (MrktSimDBSchema.segmentRow)e.Row;
            row.segment_id = update();
            e.Row.AcceptChanges();
        }

        private void market_plan_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.market_planRow row = (MrktSimDBSchema.market_planRow)e.Row;
            row.id = update();
            e.Row.AcceptChanges();
        }

        private void scenario_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.scenarioRow row = (MrktSimDBSchema.scenarioRow)e.Row;
            row.scenario_id = update();
            e.Row.AcceptChanges();
        }

        private void task_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            MrktSimDBSchema.taskRow row = (MrktSimDBSchema.taskRow)e.Row;
            row.task_id = update();
            e.Row.AcceptChanges();
        }

        private void record_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            e.Row["record_id"] = update();
            e.Row.AcceptChanges();
        }

        private void id_RowUpdated(Object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType != StatementType.Insert)
                return;

            e.Row["id"] = update();
            e.Row.AcceptChanges();
        }

        #endregion
    }
}
