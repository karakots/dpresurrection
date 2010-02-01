using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public class ProjectDb : Database
    {
        #region Types
        public enum SimulationStateType
        {
            CanEdit = -1,
            Running = 0,
            Waiting = 1
        }

        static ProjectDb()
        {
        }

        #endregion


        private int projID = Database.AllID;

        public int ProjectID
        {
            set
            {
                projID = value;
                InitializeSelectQuery();
            }

            get
            {
                return projID;
            }

        }

        public string ProjectName
        {
            get
            {
                MrktSimDBSchema.projectRow row = Data.project.FindByid(this.ProjectID);

                if (row != null)
                    return row.name;

                return NoProjectName;
            }
        }

        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.scenario
                ||
                table == Data.simulation 
                //||
                //table == Data.sim_queue
                )
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {
           base.InitializeSelectQuery();

            OleDbDataAdapter adapter = null;

            // model data

            if (this.ProjectID >= 0)
            {
                adapter = getAdapter(Data.project);
                adapter.SelectCommand.CommandText += " WHERE id = " + ProjectID;

                adapter = getAdapter(Data.Model_info);
                adapter.SelectCommand.CommandText += " WHERE project_id = " + ProjectID;

                // sceanrio
                adapter = getAdapter(Data.scenario);
                adapter.SelectCommand.CommandText += " WHERE model_id IN (SELECT model_id FROM Model_info WHERE project_id = " + this.ProjectID + ")";

                // simulation
                adapter = getAdapter(Data.simulation);
                adapter.SelectCommand.CommandText += " WHERE scenario_id  IN (SELECT scenario_id FROM scenario WHERE model_id IN (SELECT model_id FROM Model_info WHERE project_id = " + this.ProjectID + "))";

                // sim queue
                //adapter = getAdapter( Data.sim_queue );
                //adapter.SelectCommand.CommandText += " WHERE model_id IN (SELECT model_id FROM Model_info WHERE project_id = " + this.ProjectID + ")";
            }
        }

        public MrktSimDBSchema.projectRow CreateProject()
        {
            return CreateProject("project", "");
        }

        public MrktSimDBSchema.projectRow CreateProject(string name, string descr)
        {
            MrktSimDBSchema.projectRow row = Data.project.NewprojectRow();

            row.id = -1;
            row.name = ModelDb.CreateUniqueName(Data.project, "name", name, null);
            row.descr = descr;

            row.read_only = false;
            row.locked = false;
            row.created = DateTime.Now;
            row.modified = DateTime.Now;

            Data.project.AddprojectRow(row);

            return row;
        }


        //public MrktSimDBSchema.Model_infoRow CreateModel()
        //{
        //    return CreateModel("model", "");
        //}

        public MrktSimDBSchema.Model_infoRow CreateModel(MrktSimDBSchema.projectRow proj, string name, string descr)
        {
            MrktSimDBSchema.Model_infoRow row = Data.Model_info.NewModel_infoRow();

            row.project_id = proj.id;

            // unique only within project
            string filter = "project_id = " + proj.id;
            row.model_name = ModelDb.CreateUniqueName(Data.Model_info, "model_name", name, filter);

            row.descr = descr;
            row.read_only = false;
            row.locked = false;
            row.start_date = System.DateTime.Now.Date;
            row.created = System.DateTime.Now;
            row.modified = System.DateTime.Now;

            TimeSpan year = new TimeSpan(365, 0, 0, 0);

            row.end_date = row.start_date + year;
            row.model_type = 0;

            row.task_based = false;
            row.profit_loss = false;
            row.product_dependency = false;
            row.segment_growth = false;
            row.consumer_budget = false;
            row.periodic_price = false;
            row.promoted_price = true;
            row.display = true;
            row.distribution = true;
            row.social_network = false;
            row.product_extensions = false;
            row.attribute_pre_and_post = false;

            row.market_utility = false;

            row.checkpoint_file = "NA";
            row.checkpoint_date = row.end_date;
            row.checkpoint_valid = false;
            row.pop_size = 110000000;        //consumer population size of USA
            row.checkpoint_scale_factor = 0;

            row.app_code = Database.AppCode;

            row.season_freq_part = 85;
         
            Data.Model_info.AddModel_infoRow(row);

            return row;
        }

        public bool CreateProductType(int modelId, string type)
        {
            genericCommand.CommandText = "INSERT product_type (model_id, type_name) VALUES (" + modelId.ToString() + ", '" + type + "')";

            try
            {
                genericCommand.Connection.Open();
                genericCommand.ExecuteNonQuery();
                genericCommand.Connection.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        public MrktSimDBSchema.sim_queueRow CreateRun(MrktSimDBSchema.simulationRow simulation)
        {
            int queueNum = 0;

            string query = "status = " + ProjectDb.PendingSimQueue;
            DataRow[] pending = Data.sim_queue.Select(query, "", DataViewRowState.CurrentRows);

            foreach (DataRow aRow in pending)
            {
                MrktSimDBSchema.sim_queueRow pRow = (MrktSimDBSchema.sim_queueRow)aRow;
                if (queueNum < pRow.num)
                    queueNum = pRow.num;
            }

            MrktSimDBSchema.sim_queueRow row = Data.sim_queue.Newsim_queueRow();

            row.sim_id = simulation.id;
            row.name = "run " + simulation.Getsim_queueRows().Length;

            row.status = ProjectDb.PendingSimQueue;

            // if you are wondering why we duplicate the info here
            // this is a convenience so that we only have to look up
            // the sim queue to see which scenario and which model is running
            row.model_id = simulation.scenarioRow.Model_infoRow.model_id;

            // find last num in queue
            row.num = queueNum + 1;

            row.elapsed_time = 0;
            row.current_status = "waiting";
            row.current_date = simulation.scenarioRow.Model_infoRow.start_date;

            MrktSimDBSchema.scenario_simseedRow[] seeds = simulation.Getscenario_simseedRows();

            row.seed = 123;

            if (seeds.Length > 0)
                row.seed = seeds[0].seed;

            row.param_id = -1;

            Data.sim_queue.Addsim_queueRow(row);

            return row;
        }

     

        public MrktSimDBSchema.simulationRow CreateStandardSimulation(MrktSimDBSchema.scenarioRow scenario, string name)
        {
            MrktSimDBSchema.simulationRow row = Data.simulation.NewsimulationRow();

            row.scenario_id = scenario.scenario_id;

            string filter = "scenario_id = " + scenario.scenario_id;
            row.name = ModelDb.CreateUniqueName(Data.simulation, "name", name, filter);
            row.descr = "";
            row.type = (byte) SimulationDb.SimulationType.Standard;
            row.delete_std_results = false;
            row.start_date = scenario.Model_infoRow.start_date;
            row.end_date = scenario.Model_infoRow.end_date;
            row.metric_end_date = scenario.Model_infoRow.end_date;
            row.metric_start_date = scenario.Model_infoRow.start_date;
            row.locked = false;
            row.queued = false;
            row.sim_num = -1;
            row.control_string = "";

            row.reset_panel_state = false;

            if (scenario.Model_infoRow.pop_size > 1000)
            {
                row.scale_factor = 100.0 * 1000.0 / (double) scenario.Model_infoRow.pop_size;
            }
            else
            {
                row.scale_factor = 100;
            }


            row.access_time = 7;


            Data.simulation.AddsimulationRow(row);

            return row;
        }

   

        public MrktSimDBSchema.sim_variable_valueRow
            CreateSimVariableValue(MrktSimDBSchema.sim_queueRow sim, MrktSimDBSchema.scenario_variableRow variable, double runVal)
        {
            MrktSimDBSchema.sim_variable_valueRow row = Data.sim_variable_value.Newsim_variable_valueRow();

            row.run_id = sim.run_id;
            row.var_id = variable.id;
            row.val = runVal;

            Data.sim_variable_value.Addsim_variable_valueRow(row);

            return row;
        }

        #region Database queries

        public void DeleteSimulation(int id)
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "DELETE FROM simulation WHERE id =  " + id;

            Connection.Open();
            genericCommand.ExecuteNonQuery();
            Connection.Close();

        }

        public void DeleteSimQueue(int run_id)
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "DELETE FROM sim_queue WHERE run_id =  " + run_id;

            Connection.Open();
            genericCommand.ExecuteNonQuery();
            Connection.Close();

        }

        public void DeleteSimulationsRuns(int id)
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "DELETE FROM sim_queue WHERE sim_id =  " + id;

            Connection.Open();
            genericCommand.ExecuteNonQuery();
            Connection.Close();

        }

        public void ClearResults(int run_id)
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "DELETE FROM results_std " +
                "WHERE run_id = " + run_id;

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
            }

            Connection.Close();
        }

        public void ClearAllResults()
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "TRUNCATE TABLE results_std ";

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
            }

            Connection.Close();
        }

        public int NumQueuedOrRunningSims()
        {
            int rval = 0;

            if (Connection == null)
                return rval;

            // compute num simulations in queue
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WITH (NOLOCK) WHERE sim_num > -1";

            Connection.Open();
            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                rval = 0;
            }

            Connection.Close();

            return rval;
        }

        public void DequeueAllSims()
        {
            if (Connection == null)
                return;

            // take all simulations off queue
            genericCommand.CommandText = "UPDATE simulation SET sim_num = -1";

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
            }

            Connection.Close();
        }

        public int CheckSimStatus(int id)
        {
            int rval = -1;

            if (Connection == null)
                return rval;

            // take all simulations off queue
            genericCommand.CommandText = "SELECT sim_num FROM simulation WITH (NOLOCK) WHERE id = " + id;

            Connection.Open();
            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                rval = -1;
            }

            Connection.Close();

            return rval;
        }

        public int GetNumSims(int id)
        {
            int rval = 0;

            if (Connection == null)
                return rval;

            // compute num simulations in queue
            genericCommand.CommandText = "SELECT COUNT(*) FROM sim_queue WITH (NOLOCK) WHERE sim_id = " + id;

            Connection.Open();
            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                rval = 0;
            }

            Connection.Close();

            return rval;
        }

        /// <summary>
        /// make sure all simulations are registered as being stopped
        /// </summary>
        public void AllSimsStopped()
        {
            if (Connection == null)
                return;

            // if it was running previously then it is now stopped
            genericCommand.CommandText = "UPDATE sim_queue SET current_Status = 'stopped' WHERE status < 2";

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
            }

            Connection.Close();

            // take all simulations off queue
            genericCommand.CommandText = "UPDATE sim_queue SET status = 2";

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
            }

            Connection.Close();
        }

        /// <summary>
        /// This coipy is a little different in that it updates the database as a side effect
        /// </summary>
        /// <param name="orig"></param>
        public MrktSimDBSchema.simulationRow CopySimulation(MrktSimDBSchema.simulationRow orig)
        {
            // get a new name for scenario
            MrktSimDBSchema.simulationRow simulation = CreateStandardSimulation(orig.scenarioRow, orig.name);

            simulation.start_date = orig.start_date;
            simulation.end_date = orig.end_date;
            simulation.metric_start_date = orig.metric_start_date;
            simulation.metric_end_date = orig.metric_end_date;
            simulation.access_time = orig.access_time;
            simulation.delete_std_results = orig.delete_std_results;
            simulation.type = orig.type;
            simulation.control_string = orig.control_string;
            simulation.scale_factor = orig.scale_factor;

            simulation.descr = orig.descr;

            Update();


            // copy parameter references
            genericCommand.CommandText = "INSERT INTO scenario_parameter " +
                " SELECT model_id, param_id, aValue, origValue, expression, " + simulation.id + 
                " AS sim_id FROM scenario_parameter WHERE sim_id = " + orig.id;

            Connection.Open();

            genericCommand.ExecuteNonQuery();

            Connection.Close();

            // copy variables
            genericCommand.CommandText = "INSERT INTO scenario_variable " +
                " SELECT  token, descr, min, max, num_steps, type, product_id, " + simulation.id +
                " AS sim_id FROM scenario_variable WHERE sim_id = " + orig.id;

            Connection.Open();

            genericCommand.ExecuteNonQuery();

            Connection.Close();

            // copy metrics
            genericCommand.CommandText = "INSERT INTO scenario_metric SELECT token, " + simulation.id +
                " AS sim_id FROM scenario_metric WHERE sim_id = " + orig.id;

            Connection.Open();

            genericCommand.ExecuteNonQuery();

            Connection.Close();

            // copy random seed values
            genericCommand.CommandText = "INSERT INTO scenario_simseed SELECT seed, " + simulation.id +
                " AS sim_id FROM scenario_simseed WHERE sim_id = " + orig.id;

            Connection.Open();

            genericCommand.ExecuteNonQuery();

            Connection.Close();

            return simulation;
        }

        public bool LockedModels()
        {
            if (Connection == null)
                return false;

            genericCommand.CommandText = "SELECT COUNT(*) FROM model_info WHERE locked = 1";

            if (ProjectID != Database.AllID)
            {
                genericCommand.CommandText += " AND project_id  = " + ProjectID;
            }

            Connection.Open();

            int rval = 0;

            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return false;
            }

            Connection.Close();

            if (rval > 0)
                return true;

            // now test simulations
            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE locked = 1";
            if (ProjectID != Database.AllID)
            {
                genericCommand.CommandText += " AND scenario_id IN " +
                    " (SELECT scenario_id FROM scenario WHERE model_id  IN " +
                    " (SELECT model_id model_info WHERE project_id  = " + ProjectID + 
                    " ))";
            }

            Connection.Open();

            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return false;
            }

            Connection.Close();


            if (rval > 0)
                return true;

            return false;
        }

        public void UnLockModels()
        {
            if (Connection == null)
                return;

            genericCommand.CommandText = "UPDATE model_info SET locked = 0 WHERE locked = 1";
            if (ProjectID != Database.AllID)
            {
                genericCommand.CommandText += " AND project_id  = " + ProjectID;
            }

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return;
            }

            Connection.Close();

            genericCommand.CommandText = "UPDATE simulation SET locked = 0 WHERE locked = 1";
            if (ProjectID != Database.AllID)
            {
                genericCommand.CommandText += " AND scenario_id IN " +
                   " (SELECT scenario_id FROM scenario WHERE model_id  IN " +
                   " (SELECT model_id model_info WHERE project_id  = " + ProjectID +
                   " ))";
            }

            Connection.Open();

            try
            {
                genericCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return;
            }

            Connection.Close();
        }

        /// <summary>
        /// Returns a string describing the size of the table (or the entire database of the argument is null).
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string DatabaseSizeInfo( string tableName ) {
            double dum;
            return DatabaseSizeInfo( tableName, out dum );
        }

        /// <summary>
        /// Returns a string describing the size of the table (or the entire database of the argument is null).
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string DatabaseSizeInfo( string tableName, out double percentFree ) {

            string database_size = "";
            string unallocated_space = "";

            genericCommand.CommandText = "EXEC sp_spaceused";
            if( tableName != null ) {
                genericCommand.CommandText += " '" + tableName + "'";
            }

            Connection.Open();

            try {
                OleDbDataReader dbReader = genericCommand.ExecuteReader();
                if( dbReader.HasRows ) {
                    dbReader.Read();
                    if( tableName == null ) {
                        // tablename = null --> get overall database info

                        //  un-comment other lines to get additional info
                        //  string database_name = dbReader.GetString( 0 );
                        database_size = dbReader.GetString( 1 );
                        unallocated_space = dbReader.GetString( 2 );

                        //dbReader.NextResult();
                        //dbReader.Read();
                        //  string reserved = dbReader.GetString( 0 );
                        //  string data = dbReader.GetString( 1 );
                        //  string index_size = dbReader.GetString( 2 );
                        //string unused = dbReader.GetString( 3 );
                    }
                    else {
                        // get info for a particular table

                        //  un-comment other lines to get additional info
                        //  dbReader.Read();
                        //  string name = dbReader.GetString( 0 );
                        //  string rows = dbReader.GetString( 1 );
                        database_size = dbReader.GetString( 2 );
                        //  string data = dbReader.GetString( 3 );
                        //  string index_size = dbReader.GetString( 4 );
                        //  string unused = dbReader.GetString( 4 );
                    }
                }
            }
            catch( System.Data.OleDb.OleDbException ) {
            }

            Connection.Close();

            double sizeMBytes = -1;
            if( database_size.EndsWith( "MB" ) ) {
                try {
                    sizeMBytes = double.Parse( database_size.Substring( 0, database_size.Length - 2 ) );
                }
                catch( Exception ) {
                }

            }
            else if( database_size.EndsWith( "KB" ) ) {
                try {
                    sizeMBytes = double.Parse( database_size.Substring( 0, database_size.Length - 2 ) ) / 1024;
                }
                catch( Exception ) {
                }
            }

            double unallocMBytes = -1;
            percentFree = -1;

            if( unallocated_space.EndsWith( "MB" ) ) {
                try {
                    unallocMBytes = double.Parse( unallocated_space.Substring( 0, unallocated_space.Length - 2 ) );
                }
                catch( Exception ) {
                }

            }
            else if( unallocated_space.EndsWith( "KB" ) ) {
                try {
                    unallocMBytes = double.Parse( unallocated_space.Substring( 0, unallocated_space.Length - 2 ) ) / 1024;
                }
                catch( Exception ) {
                }
            }

            string info = String.Format( "{0:f2} MB", sizeMBytes );
            if( tableName == null ) {
                percentFree = unallocMBytes / sizeMBytes * 100;
                info = String.Format( "{0:f2} MB  ({1:f0}% free)", sizeMBytes, percentFree );
            }
            else {
                info = String.Format( "{0:f2} MB", sizeMBytes );
            }

            return info;
        }

        
        /// <summary>
        /// Returns a string describing the size of the table (or the entire database of the argument is null).
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool ShrinkDatabase( int targetPercentFree ) {

            bool noError = true;
            string dbName = genericCommand.Connection.Database;

            genericCommand.Connection.Open();

            // first back up LOG
            genericCommand.CommandText = "BACKUP LOG " + dbName + " WITH NO_LOG";
            try {
                int nRowsAffected = genericCommand.ExecuteNonQuery();
            }
            catch( Exception e ) {
                string s = e.Message;
                noError = false;
            }

            if( noError ) {
                // shrink database
                genericCommand.CommandText = "DBCC SHRINKDATABASE (" + dbName + ", " + targetPercentFree.ToString() + ")";
                try {
                    int nRowsAffected = genericCommand.ExecuteNonQuery();
                }
                catch( Exception ) {
                    noError = false;
                }
            }

            genericCommand.Connection.Close();


            return noError;
        }
        #endregion


    }
}
