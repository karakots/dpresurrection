using System;
using System.Data.OleDb;


namespace MrktSimDb
{
	/// <summary>
	/// Static class for holding SQL Queries
	/// Future enhancement may dictate adapting queries to special adapters
	/// </summary>
	/// 

	public class SqlQuery
	{

		# region select commands

        static public void InitializeProjectSelect(OleDbCommand command)
        {
            command.CommandText = "SELECT id, name, descr, read_only, locked, created, modified FROM project ";
        }

		static public  void InitializeModelSelect(OleDbCommand command)
		{
			//
			// Model_info
			//
			
			// select
			command.CommandText = "SELECT model_id, project_id, model_name, model_type, descr, read_only, locked, " +
				"created, modified, start_date, end_date, app_code, task_based, profit_loss, product_extensions, product_dependency," +
				"segment_growth, consumer_budget, periodic_price, promoted_price, distribution, display, market_utility, social_network, attribute_pre_and_post, " +
                "checkpoint_file, checkpoint_date, checkpoint_valid, pop_size, checkpoint_scale_factor, season_freq_part " +
				"FROM Model_info ";
		}

		static public void InitializeMarketPlanSelect(OleDbCommand command)
		{
			command.CommandText = "SELECT model_id, id, name, descr, start_date, end_date, interval, product_id, segment_id, channel_id, task_id, type, parm1, parm2, parm3, parm4, parm5, parm6, user_name " +
				"FROM market_plan";
		}

		static public void InitializeScenarioSelect(OleDbCommand command)
		{
			command.CommandText = "SELECT " +
				" scenario_id, model_id, name, descr, user_name " +
				" FROM scenario ";
		}

        static public void InitializeSimulationSelect( OleDbCommand command ) {
            command.CommandText = "SELECT " +
                " id, scenario_id, name, descr, type, start_date, end_date, metric_start_date, metric_end_date, queued, sim_num, locked, delete_std_results, control_string, access_time, scale_factor, reset_panel_state " +
                " FROM simulation ";
        }

        // Removed reference to view and simply made this a direct query
        // added the field 
        static public void InitializeSimulationStatusSelect( OleDbCommand command ) {
            command.CommandText = "SELECT " +
                " simulation.name, " +
                " scenario_type.type,   " +
                " simulation_state_type.[name] as status,   " +
                " simulation.start_date, " +
                " simulation.end_date, " +
                " simulation.reset_panel_state,  " +
                " simulation.scenario_id,  " +
                " simulation.id as simulation_id,  " +
                " simulation.sim_num " +
            " FROM simulation, scenario_type, simulation_state_type  WITH (NOLOCK) " +
            " WHERE simulation_state_type.id = simulation.sim_num AND   scenario_type.scenario_type_id = simulation.[type]  ";
        }

        static public void InitializeSimQueueSelect( OleDbCommand command )
		{
			// sim queue
			command.CommandText = "SELECT " +
				" sim_queue.run_id, sim_queue.sim_id, sim_queue.status, sim_queue.num, sim_queue.model_id, " +
				" sim_queue.name, sim_queue.elapsed_time, sim_queue.current_status, sim_queue.[current_date], sim_queue.run_time, sim_queue.seed, sim_queue.param_id " +
				" FROM sim_queue WITH (NOLOCK) ";
		}

        static public void InitializeResultsStatusSelect( OleDbCommand command ) {
            command.CommandText = "SELECT " +
                " simulation.[name], " +
                " sim_queue.[name] as run_name, " +
                " sim_status.status, " +
                " sim_queue.run_time, " +
                " sim_queue.current_status,  " +
                " simulation.scenario_id, " +
                " simulation.id as simulation_id, " +
                " sim_queue.run_id,  " +
                " sim_queue.[current_date], " +
                " (sim_queue.elapsed_time/60000) as elapsed_time" +
                " FROM sim_queue, simulation, sim_status  WITH (NOLOCK) " +
                " WHERE sim_status.status_id = sim_queue.status AND simulation.id = sim_queue.sim_id ";
        }

        static public void InitializeRunLogSelect(OleDbCommand command)
        {
            // run log
            command.CommandText = "SELECT " +
                " run_log.run_id, run_log.calendar_date, run_log.product_id, run_log.segment_id, run_log.channel_id, run_log.comp_id, run_log.message_id, run_log.message FROM run_log ";
        }

		static public void InitializeSimValueSelect(OleDbCommand command)
		{
			// sim queue
			command.CommandText = "SELECT " +
				" run_id, var_id, val " +
				" FROM sim_variable_value ";
		}

		static public void InitializeScenarioVariableSelect(OleDbCommand command)
		{
			// sim queue
			command.CommandText = "SELECT " +
				" sim_id, id, token, descr, min, max, num_steps, type, product_id " +
				" FROM scenario_variable ";
		}

		static public void InitializeScenarioSimSeedSelect(OleDbCommand command)
		{
			// sim seed
			command.CommandText = "SELECT " +
				" sim_id, id, seed" +
				" FROM scenario_simseed ";
		}

		static public void InitializeScenarioParameterSelect(OleDbCommand command)
		{
			command.CommandText = "SELECT model_id, sim_id, param_id, aValue, expression " +
				"FROM scenario_parameter";
		}

		static public void InitializeScenarioMetricSelect(OleDbCommand command)
		{
			// sim seed
			command.CommandText = "SELECT " +
				" sim_id, token" +
				" FROM scenario_metric ";
		}

		#endregion

		#region Project Level
		static public void InitializeModelQuery(OleDbDataAdapter adapter)
		{
			// insert
			adapter.InsertCommand.CommandText = "INSERT INTO Model_info " +
				"(project_id, model_name, model_type, descr, read_only, locked, " +
				"start_date, end_date, app_code, task_based, profit_loss, product_extensions, " +
				"product_dependency, segment_growth, consumer_budget, periodic_price, " +
				"promoted_price, distribution, display, market_utility, social_network, attribute_pre_and_post, " +
                "checkpoint_file, checkpoint_date, checkpoint_valid, pop_size, checkpoint_scale_factor, season_freq_part) " +
				"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("project_id", System.Data.OleDb.OleDbType.Integer, 4, "project_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_name", System.Data.OleDb.OleDbType.VarChar, 100, "model_name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_type", System.Data.OleDb.OleDbType.Integer, 4, "model_type"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("read_only", System.Data.OleDb.OleDbType.Boolean, 1, "read_only"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("app_code", System.Data.OleDb.OleDbType.VarChar, 100, "app_code"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("task_based", System.Data.OleDb.OleDbType.Boolean, 1, "task_based"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("profit_loss", System.Data.OleDb.OleDbType.Boolean, 1, "profit_loss"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_extensions", System.Data.OleDb.OleDbType.Boolean, 1, "product_extensions"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_dependency", System.Data.OleDb.OleDbType.Boolean, 1, "product_dependency"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_growth", System.Data.OleDb.OleDbType.Boolean, 1, "segment_growth"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("consumer_budget", System.Data.OleDb.OleDbType.Boolean, 1, "consumer_budget"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("periodic_price", System.Data.OleDb.OleDbType.Boolean, 1, "periodic_price"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("promoted_price", System.Data.OleDb.OleDbType.Boolean, 1, "promoted_price"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("distribution", System.Data.OleDb.OleDbType.Boolean, 1, "distribution"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("display", System.Data.OleDb.OleDbType.Boolean, 1, "display"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("market_utility", System.Data.OleDb.OleDbType.Boolean, 1, "market_utility"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("social_network", System.Data.OleDb.OleDbType.Boolean, 1, "social_network"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("attribute_pre_and_post", System.Data.OleDb.OleDbType.Boolean, 1, "attribute_pre_and_post"));

			// checkpointing
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_file", System.Data.OleDb.OleDbType.VarChar, 100, "checkpoint_file"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "checkpoint_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_valid", System.Data.OleDb.OleDbType.Boolean, 1, "checkpoint_valid"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("pop_size", System.Data.OleDb.OleDbType.Integer, 4, "pop_size"));
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "checkpoint_scale_factor", System.Data.OleDb.OleDbType.Double, 8, "checkpoint_scale_factor" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "season_freq_part", System.Data.OleDb.OleDbType.Double, 8, "season_freq_part" ) );
		
			// Update
			adapter.UpdateCommand.CommandText = "UPDATE Model_info SET " +
				"project_id = ?, model_name = ?, model_type = ?, descr = ?, read_only = ?, locked = ?, " +
				"modified = getdate(), " +
				"start_date = ?, end_date = ?, app_code = ?, task_based = ?, profit_loss = ?, product_extensions = ?, " +
				"product_dependency = ?, segment_growth = ?, consumer_budget = ?, periodic_price = ?, " +
				"promoted_price = ?, distribution = ?, display = ?, market_utility = ?, social_network = ?, attribute_pre_and_post = ?, " +
                "checkpoint_file = ?, checkpoint_date = ?, checkpoint_valid = ?, pop_size = ?, checkpoint_scale_factor = ?, season_freq_part = ? " +
				"WHERE (model_id = ?)";

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("project_id", System.Data.OleDb.OleDbType.Integer, 4, "project_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_name", System.Data.OleDb.OleDbType.VarChar, 100, "model_name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_type", System.Data.OleDb.OleDbType.Integer, 4, "model_type"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("read_only", System.Data.OleDb.OleDbType.Boolean, 1, "read_only"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
			//adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("modified", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "modified"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("app_code", System.Data.OleDb.OleDbType.VarChar, 100, "app_code"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("task_based", System.Data.OleDb.OleDbType.Boolean, 1, "task_based"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("profit_loss", System.Data.OleDb.OleDbType.Boolean, 1, "profit_loss"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_extensions", System.Data.OleDb.OleDbType.Boolean, 1, "product_extensions"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_dependency", System.Data.OleDb.OleDbType.Boolean, 1, "product_dependency"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_growth", System.Data.OleDb.OleDbType.Boolean, 1, "segment_growth"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("consumer_budget", System.Data.OleDb.OleDbType.Boolean, 1, "consumer_budget"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("periodic_price", System.Data.OleDb.OleDbType.Boolean, 1, "periodic_price"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("promoted_price", System.Data.OleDb.OleDbType.Boolean, 1, "promoted_price"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("distribution", System.Data.OleDb.OleDbType.Boolean, 1, "distribution"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("display", System.Data.OleDb.OleDbType.Boolean, 1, "display"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("market_utility", System.Data.OleDb.OleDbType.Boolean, 1, "market_utility"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("social_network", System.Data.OleDb.OleDbType.Boolean, 1, "social_network"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("attribute_pre_and_post", System.Data.OleDb.OleDbType.Boolean, 1, "attribute_pre_and_post"));

			// checkpointing
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_file", System.Data.OleDb.OleDbType.VarChar, 100, "checkpoint_file"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "checkpoint_date"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("checkpoint_valid", System.Data.OleDb.OleDbType.Boolean, 1, "checkpoint_valid"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("pop_size", System.Data.OleDb.OleDbType.Integer, 4, "pop_size"));
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "checkpoint_scale_factor", System.Data.OleDb.OleDbType.Double, 8, "checkpoint_scale_factor" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "season_freq_part", System.Data.OleDb.OleDbType.Double, 8, "season_freq_part" ) );
		
			// reference
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			
		
			// Delete
			adapter.DeleteCommand.CommandText = "DELETE FROM model_info WHERE (model_id = ?)";
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
		}

		
		static public void InitializeSimQueueQuery(OleDbDataAdapter adapter)
		{	
			// insert
			adapter.InsertCommand.CommandText = "INSERT INTO sim_queue " +
				" (sim_id, status, num, model_id, name, elapsed_time, current_status, [current_date], run_time, seed, param_id) " +
				" VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("status", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "status"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num", System.Data.OleDb.OleDbType.Integer, 4, "num"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 25, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("elapsed_time", System.Data.OleDb.OleDbType.Integer, 4, "elapsed_time"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("current_status", System.Data.OleDb.OleDbType.VarChar, 100, "current_status"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("current_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "current_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("run_time", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "run_time"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("seed", System.Data.OleDb.OleDbType.Integer, 4, "seed"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("param_id", System.Data.OleDb.OleDbType.Integer, 4, "param_id"));
			
			// 
			// update
			// 
			adapter.UpdateCommand.CommandText = "UPDATE sim_queue SET " +
				"sim_id = ?, status = ?, num = ?, model_id = ?, name = ?, elapsed_time = ?, current_status = ?, [current_date] = ?, run_time = ?, seed = ?, param_id = ?" +
				" WHERE (run_id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("status", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "status"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num", System.Data.OleDb.OleDbType.Integer, 4, "num"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 25, "name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("elapsed_time", System.Data.OleDb.OleDbType.Integer, 4, "elapsed_time"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("current_status", System.Data.OleDb.OleDbType.VarChar, 100, "current_status"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("current_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "current_date"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("run_time", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "run_time"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("seed", System.Data.OleDb.OleDbType.Integer, 4, "seed"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("param_id", System.Data.OleDb.OleDbType.Integer, 4, "param_id"));
		

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_run_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "run_id", System.Data.DataRowVersion.Original, null));
			
			// 
			// delete
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM sim_queue WHERE (run_id = ?)";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_run_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "run_id", System.Data.DataRowVersion.Original, null));

		}
		
		static public void Initialize_Scenario_UpdateQuery(OleDbDataAdapter adapter)
		{
			// insert
			adapter.InsertCommand.CommandText = "INSERT INTO scenario(model_id, name, descr, user_name) " +
				"VALUES (?, ?, ?, ?)";

			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("user_name", System.Data.OleDb.OleDbType.VarChar, 100, "user_name"));
			
			// update
			adapter.UpdateCommand.CommandText = "UPDATE scenario SET " +
                "name = ?, descr = ?, user_name = ? " +
				"WHERE (scenario_id = ?)";

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("user_name", System.Data.OleDb.OleDbType.VarChar, 100, "user_name"));
			
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_scenario_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "scenario_id", System.Data.DataRowVersion.Original, null));

			// delete
			adapter.DeleteCommand.CommandText = "DELETE FROM scenario WHERE (scenario_id = ?)";
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_scenario_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "scenario_id", System.Data.DataRowVersion.Original, null));
		}

        static public void Initialize_Simulation_UpdateQuery(OleDbDataAdapter adapter)
        {
            // insert
            adapter.InsertCommand.CommandText = "INSERT INTO simulation(scenario_id, name, descr, type, start_date, end_date, metric_start_date, metric_end_date, delete_std_results, locked, queued, sim_num, control_string, access_time, scale_factor, reset_panel_state) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("scenario_id", System.Data.OleDb.OleDbType.Integer, 4, "scenario_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("metric_start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "metric_start_date"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("metric_end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "metric_end_date"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("delete_std_results", System.Data.OleDb.OleDbType.Boolean, 1, "delete_std_results"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("queued", System.Data.OleDb.OleDbType.Boolean, 1, "queued"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_num", System.Data.OleDb.OleDbType.Integer, 4, "sim_num"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("control_string", System.Data.OleDb.OleDbType.VarChar, 100, "control_string"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("access_time", System.Data.OleDb.OleDbType.Integer, 4, "access_time"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("scale_factor", System.Data.OleDb.OleDbType.Double, 8, "scale_factor"));
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "reset_panel_state", System.Data.OleDb.OleDbType.Boolean, 1, "reset_panel_state" ) );

            // update
            adapter.UpdateCommand.CommandText = "UPDATE simulation SET " +
                "scenario_id = ?, name = ?, descr = ?, type = ?, start_date = ?, end_date = ?, metric_start_date = ?, metric_end_date = ?, delete_std_results = ?, locked = ?, queued = ?, sim_num = ?, control_string = ?, access_time = ?, scale_factor = ?, reset_panel_state = ? " +
                "WHERE (id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("scenario_id", System.Data.OleDb.OleDbType.Integer, 4, "scenario_id"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("metric_start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "metric_start_date"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("metric_end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "metric_end_date"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("delete_std_results", System.Data.OleDb.OleDbType.Boolean, 1, "delete_std_results"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("queued", System.Data.OleDb.OleDbType.Boolean, 1, "queued"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_num", System.Data.OleDb.OleDbType.Integer, 4, "sim_num"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("control_string", System.Data.OleDb.OleDbType.VarChar, 100, "control_string"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("access_time", System.Data.OleDb.OleDbType.Integer, 4, "access_time"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("scale_factor", System.Data.OleDb.OleDbType.Double, 8, "scale_factor"));
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "reset_panel_state", System.Data.OleDb.OleDbType.Boolean, 1, "reset_panel_state" ) );

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));

            // delete
            adapter.DeleteCommand.CommandText = "DELETE FROM simulation WHERE (id = ?)";
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
        }


		static public void InitializeProjectQuery(OleDbDataAdapter adapter)
		{
			// 
			// oleDbInsertCommand
			// 
			adapter.InsertCommand.CommandText = "INSERT INTO project(name, descr, read_only, locked) " +
				"VALUES (?, ?, ?, ?)";
			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("read_only", System.Data.OleDb.OleDbType.Boolean, 1, "read_only"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
			
			// 
			// oleDbUpdateCommand
			// 
			adapter.UpdateCommand.CommandText = "UPDATE project SET " +
				"name = ?, descr = ?, read_only = ?, locked = ?, modified = ? " +
				"WHERE (id = ?)";
			
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("read_only", System.Data.OleDb.OleDbType.Boolean, 1, "read_only"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("locked", System.Data.OleDb.OleDbType.Boolean, 1, "locked"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("modified", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "modified"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM project WHERE (id = ?)";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
		}


        static public void initialize_scenario_parameter_UpdateQuery(OleDbDataAdapter adapter)
        {

            adapter.InsertCommand.CommandText = "INSERT INTO scenario_parameter" +
                "(model_id, sim_id, param_id, aValue, expression) VALUES (?, ?, ?, ?, ?);";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("param_id", System.Data.OleDb.OleDbType.Integer, 4, "param_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("aValue", System.Data.OleDb.OleDbType.Double, 8, "aValue"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("expression", System.Data.OleDb.OleDbType.VarChar, 200, "expression"));

            // only update value
            adapter.UpdateCommand.CommandText = @"UPDATE scenario_parameter SET " +
                " aValue = ?, expression = ?  WHERE (sim_id = ?) AND (param_id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("aValue", System.Data.OleDb.OleDbType.Double, 8, "aValue"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("expression", System.Data.OleDb.OleDbType.VarChar, 200, "expression"));

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_param_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "param_id", System.Data.DataRowVersion.Original, null));

            adapter.DeleteCommand.CommandText = @"DELETE FROM scenario_parameter " +
                " WHERE (sim_id = ?) AND (param_id = ?)";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_param_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "param_id", System.Data.DataRowVersion.Original, null));
        }

        static public void initialize_scenario_variable_UpdateQuery(OleDbDataAdapter adapter)
        {
            adapter.InsertCommand.CommandText = "INSERT INTO scenario_variable" +
                "(sim_id, token, descr, min, max, num_steps, type, product_id) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("token", System.Data.OleDb.OleDbType.VarChar, 10, "token"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("min", System.Data.OleDb.OleDbType.Double, 8, "min"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("max", System.Data.OleDb.OleDbType.Double, 8, "max"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_steps", System.Data.OleDb.OleDbType.Integer, 4, "num_steps"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));

            // only update value
            adapter.UpdateCommand.CommandText = @"UPDATE scenario_variable SET " +
                " token = ?, descr = ?, min = ?, max = ?, num_steps = ?, type = ?, product_id = ?  WHERE (sim_id = ?) AND (id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("token", System.Data.OleDb.OleDbType.VarChar, 10, "token"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 100, "descr"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("min", System.Data.OleDb.OleDbType.Double, 8, "min"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("max", System.Data.OleDb.OleDbType.Double, 8, "max"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_steps", System.Data.OleDb.OleDbType.Integer, 4, "num_steps"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));


            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));

            // delete variable
            adapter.DeleteCommand.CommandText = @"DELETE FROM scenario_variable " +
                " WHERE (sim_id = ?) AND (id = ?)";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
        }


        static public void initialize_scenario_simseed_UpdateQuery(OleDbDataAdapter adapter)
        {
            adapter.InsertCommand.CommandText = "INSERT INTO scenario_simseed" +
                "(sim_id, seed) VALUES (?, ?)";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("seed", System.Data.OleDb.OleDbType.Double, 8, "seed"));

            // only update seed
            adapter.UpdateCommand.CommandText = @"UPDATE scenario_simseed SET " +
                " seed = ? WHERE (sim_id = ?) AND (id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("seed", System.Data.OleDb.OleDbType.Double, 8, "seed"));

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));

            // delete variable
            adapter.DeleteCommand.CommandText = @"DELETE FROM scenario_simseed " +
                " WHERE (sim_id = ?) AND (id = ?)";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
        }

        static public void initialize_scenario_metric_UpdateQuery(OleDbDataAdapter adapter)
        {
            adapter.InsertCommand.CommandText = "INSERT INTO scenario_metric" +
                "(sim_id, token) VALUES (?, ?)";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sim_id", System.Data.OleDb.OleDbType.Integer, 4, "sim_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("token", System.Data.OleDb.OleDbType.VarChar, 100, "token"));

            // no updating

            // delete metric
            adapter.DeleteCommand.CommandText = @"DELETE FROM scenario_metric " +
                " WHERE (sim_id = ?) AND (token = ?)";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_sim_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "sim_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_token", System.Data.OleDb.OleDbType.VarChar, 100, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "token", System.Data.DataRowVersion.Original, null));
        }

        static public void initialize_sim_variable_values_UpdateQuery(OleDbDataAdapter adapter)
        {
            adapter.InsertCommand.CommandText = "INSERT INTO sim_variable_value" +
                "(run_id, var_id, val) VALUES (?, ?, ?);";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("run_id", System.Data.OleDb.OleDbType.Integer, 4, "run_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("var_id", System.Data.OleDb.OleDbType.Integer, 4, "var_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("val", System.Data.OleDb.OleDbType.Double, 8, "val"));

            // only update value
            adapter.UpdateCommand.CommandText = @"UPDATE sim_variable_value SET " +
                " val = ?  WHERE (run_id = ?) AND (var_id = ?)";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("val", System.Data.OleDb.OleDbType.Double, 8, "val"));

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_run_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "run_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_var_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "var_id", System.Data.DataRowVersion.Original, null));

            adapter.DeleteCommand.CommandText = @"DELETE FROM sim_variable_value " +
                " WHERE (run_id = ?) AND (var_id = ?)";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_run_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "run_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_var_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "var_id", System.Data.DataRowVersion.Original, null));

        }

        static public void initialize_results_std_UpdateQuery(OleDbDataAdapter adapter)
        {

            // 
            // oleDbInsertCommand
            // 
            adapter.InsertCommand.CommandText = "INSERT INTO results_std" +
                "(run_id, calendar_date, segment_id, product_id, channel_id, " +
                "num_adds_sku, num_drop_sku, percent_aware_sku_cum, " +
                "persuasion_sku, GRPs_SKU_tick, promoprice, " +
                "unpromoprice, num_coupon_redemptions, num_units_bought_on_coupon, num_sku_bought, num_sku_triers, " +
                "num_sku_repeaters, num_trips, num_sku_repeater_trips_cum, sku_dollar_purchased_tick, " +
                "percent_preuse_distribution_sku, percent_on_display_sku, percent_sku_at_promo_price, " +
                "num_units_unpromo, num_units_promo, num_units_display, display_price, " +
                "percent_at_display_price, eq_units, volume ) " +
                " VALUES (?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                " ?, ?, ?)";
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("run_id", System.Data.OleDb.OleDbType.Integer, 4, "run_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("calendar_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "calendar_date"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_adds_sku", System.Data.OleDb.OleDbType.Integer, 4, "num_adds_sku"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_drop_sku", System.Data.OleDb.OleDbType.Integer, 4, "num_drop_sku"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_aware_sku_cum", System.Data.OleDb.OleDbType.Double, 8, "percent_aware_sku_cum"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("persuasion_sku", System.Data.OleDb.OleDbType.Double, 8, "persuasion_sku"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("GRPs_SKU_tick", System.Data.OleDb.OleDbType.Double, 8, "GRPs_SKU_tick"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("promoprice", System.Data.OleDb.OleDbType.Double, 8, "promoprice"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("unpromoprice", System.Data.OleDb.OleDbType.Double, 8, "unpromoprice"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_coupon_redemptions", System.Data.OleDb.OleDbType.Integer, 4, "num_coupon_redemptions"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_units_bought_on_coupon", System.Data.OleDb.OleDbType.Integer, 4, "num_units_bought_on_coupon"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_sku_bought", System.Data.OleDb.OleDbType.Integer, 4, "num_sku_bought"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_sku_triers", System.Data.OleDb.OleDbType.Integer, 4, "num_sku_triers"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_sku_repeaters", System.Data.OleDb.OleDbType.Integer, 4, "num_sku_repeaters"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_trips", System.Data.OleDb.OleDbType.Integer, 4, "num_trips"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_sku_repeater_trips_cum", System.Data.OleDb.OleDbType.Integer, 4, "num_sku_repeater_trips_cum"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("sku_dollar_purchased_tick", System.Data.OleDb.OleDbType.Double, 8, "sku_dollar_purchased_tick"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_preuse_distribution_sku", System.Data.OleDb.OleDbType.Double, 8, "percent_preuse_distribution_sku"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_on_display_sku", System.Data.OleDb.OleDbType.Double, 8, "percent_on_display_sku"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_sku_at_promo_price", System.Data.OleDb.OleDbType.Double, 8, "percent_sku_at_promo_price"));

            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_units_unpromo", System.Data.OleDb.OleDbType.Double, 8, "num_units_unpromo" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_units_promo", System.Data.OleDb.OleDbType.Double, 8, "num_units_promo" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_units_display", System.Data.OleDb.OleDbType.Double, 8, "num_units_display" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "display_price", System.Data.OleDb.OleDbType.Double, 8, "display_price" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "percent_at_display_price", System.Data.OleDb.OleDbType.Double, 8, "percent_at_display_price" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "eq_units", System.Data.OleDb.OleDbType.Double, 8, "eq_units" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "volume", System.Data.OleDb.OleDbType.Double, 8, "volume" ) );
        }


		#endregion


		#region Model


        static public void initialize_Product_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO product(model_id, product_name, brand_id, type, product_group, related_group, percent_relation, cost, initial_dislike_probability, repeat_like_probability, color, product_type, base_price, eq_units, pack_size_id) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

           

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_name", System.Data.OleDb.OleDbType.VarChar, 100, "product_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "brand_id", System.Data.OleDb.OleDbType.Integer, 4, "brand_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.VarChar, 25, "type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_group", System.Data.OleDb.OleDbType.VarChar, 25, "product_group" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "related_group", System.Data.OleDb.OleDbType.VarChar, 25, "related_group" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "percent_relation", System.Data.OleDb.OleDbType.VarChar, 25, "percent_relation" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cost", System.Data.OleDb.OleDbType.Double, 8, "cost" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_dislike_probability", System.Data.OleDb.OleDbType.Double, 8, "initial_dislike_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repeat_like_probability", System.Data.OleDb.OleDbType.Double, 8, "repeat_like_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "color", System.Data.OleDb.OleDbType.VarChar, 25, "color" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_type", System.Data.OleDb.OleDbType.Integer, 4, "product_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "base_price", System.Data.OleDb.OleDbType.Double, 8, "base_price" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "eq_units", System.Data.OleDb.OleDbType.Double, 8, "eq_units" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pack_size_id", System.Data.OleDb.OleDbType.Integer, 4, "pack_size_id" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE product SET product_name = ?, brand_id = ?, type = ?, product_group = ?, related_group = ?, percent_relation = ?, cost = ?, initial_dislike_probability = ?, repeat_like_probability = ?, color = ?, product_type = ?, base_price = ?, eq_units = ?, pack_size_id = ? WHERE (product_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_name", System.Data.OleDb.OleDbType.VarChar, 100, "product_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "brand_id", System.Data.OleDb.OleDbType.Integer, 4, "brand_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.VarChar, 25, "type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_group", System.Data.OleDb.OleDbType.VarChar, 25, "product_group" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "related_group", System.Data.OleDb.OleDbType.VarChar, 25, "related_group" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "percent_relation", System.Data.OleDb.OleDbType.VarChar, 25, "percent_relation" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cost", System.Data.OleDb.OleDbType.Double, 8, "cost" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_dislike_probability", System.Data.OleDb.OleDbType.Double, 8, "initial_dislike_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repeat_like_probability", System.Data.OleDb.OleDbType.Double, 8, "repeat_like_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "color", System.Data.OleDb.OleDbType.VarChar, 25, "color" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_type", System.Data.OleDb.OleDbType.Integer, 4, "product_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "base_price", System.Data.OleDb.OleDbType.Double, 8, "base_price" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "eq_units", System.Data.OleDb.OleDbType.Double, 8, "eq_units" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pack_size_id", System.Data.OleDb.OleDbType.Integer, 4, "pack_size_id" ) );

            // id
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM product WHERE (product_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Channel_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO channel(model_id, channel_name) VALUES (?, ?) ";// + // refresh

         
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_name", System.Data.OleDb.OleDbType.VarChar, 100, "channel_name" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE channel SET channel_name = ? WHERE (channel_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_name", System.Data.OleDb.OleDbType.VarChar, 100, "channel_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM channel WHERE (channel_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Segment_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;
            command.CommandText = "INSERT INTO segment " +
                "(model_id, segment_model, segment_name, color, show_current_share_pie_chart, " +
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
                " min_freq, max_freq) " +
                "VALUES (" +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                "?,  ?, ?, ?)";


            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_model", System.Data.OleDb.OleDbType.VarChar, 20, "segment_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_name", System.Data.OleDb.OleDbType.VarChar, 100, "segment_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "color", System.Data.OleDb.OleDbType.VarChar, 50, "color" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "show_current_share_pie_chart", System.Data.OleDb.OleDbType.VarChar, 3, "show_current_share_pie_chart" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "show_cmulative_share_chart", System.Data.OleDb.OleDbType.VarChar, 3, "show_cmulative_share_chart" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_size", System.Data.OleDb.OleDbType.Double, 8, "segment_size" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate", System.Data.OleDb.OleDbType.Double, 8, "growth_rate" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate_people_percent", System.Data.OleDb.OleDbType.VarChar, 20, "growth_rate_people_percent" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate_month_year", System.Data.OleDb.OleDbType.VarChar, 20, "growth_rate_month_year" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "compress_population", System.Data.OleDb.OleDbType.VarChar, 3, "compress_population" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "variability", System.Data.OleDb.OleDbType.Integer, 4, "variability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_disutility", System.Data.OleDb.OleDbType.Double, 8, "price_disutility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attribute_sensitivity", System.Data.OleDb.OleDbType.Double, 8, "attribute_sensitivity" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_scaling", System.Data.OleDb.OleDbType.Double, 8, "persuasion_scaling" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "display_utility", System.Data.OleDb.OleDbType.Double, 8, "display_utility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "display_utility_scaling_factor", System.Data.OleDb.OleDbType.Double, 8, "display_utility_scaling_factor" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "max_display_hits_per_trip", System.Data.OleDb.OleDbType.Double, 8, "max_display_hits_per_trip" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "inertia", System.Data.OleDb.OleDbType.Double, 8, "inertia" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase", System.Data.OleDb.OleDbType.VarChar, 3, "repurchase" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_model", System.Data.OleDb.OleDbType.VarChar, 3, "repurchase_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "gamma_location_parameter_a", System.Data.OleDb.OleDbType.Double, 8, "gamma_location_parameter_a" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "gamma_shape_parameter_k", System.Data.OleDb.OleDbType.Double, 8, "gamma_shape_parameter_k" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_period_frequency", System.Data.OleDb.OleDbType.Double, 8, "repurchase_period_frequency" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_frequency_variation", System.Data.OleDb.OleDbType.Double, 8, "repurchase_frequency_variation" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_timescale", System.Data.OleDb.OleDbType.VarChar, 10, "repurchase_timescale" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "avg_max_units_purch", System.Data.OleDb.OleDbType.Double, 8, "avg_max_units_purch" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "shopping_trip_interval", System.Data.OleDb.OleDbType.Double, 8, "shopping_trip_interval" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "category_penetration", System.Data.OleDb.OleDbType.Integer, 4, "category_penetration" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "category_rejection", System.Data.OleDb.OleDbType.Integer, 4, "category_rejection" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_initial_buyers", System.Data.OleDb.OleDbType.BigInt, 8, "num_initial_buyers" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_buying_period", System.Data.OleDb.OleDbType.Double, 8, "initial_buying_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "seed_with_repurchasers", System.Data.OleDb.OleDbType.VarChar, 1, "seed_with_repurchasers" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "use_budget", System.Data.OleDb.OleDbType.VarChar, 1, "use_budget" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "budget", System.Data.OleDb.OleDbType.Double, 8, "budget" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "budget_period", System.Data.OleDb.OleDbType.VarChar, 30, "budget_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "save_unspent", System.Data.OleDb.OleDbType.VarChar, 1, "save_unspent" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_savings", System.Data.OleDb.OleDbType.Double, 8, "initial_savings" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "social_network_model", System.Data.OleDb.OleDbType.VarChar, 20, "social_network_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_close_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_close_contacts" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talking_close_contact_pre", System.Data.OleDb.OleDbType.Double, 8, "prob_talking_close_contact_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talking_close_contact_post", System.Data.OleDb.OleDbType.Double, 8, "prob_talking_close_contact_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "use_local", System.Data.OleDb.OleDbType.VarChar, 1, "use_local" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_distant_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_distant_contacts" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talk_distant_contact_pre", System.Data.OleDb.OleDbType.Double, 8, "prob_talk_distant_contact_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talk_distant_contact_post", System.Data.OleDb.OleDbType.Double, 8, "prob_talk_distant_contact_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_weight_personal_message", System.Data.OleDb.OleDbType.Double, 8, "awareness_weight_personal_message" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_persuasion_prob", System.Data.OleDb.OleDbType.Double, 8, "pre_persuasion_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_persuasion_prob", System.Data.OleDb.OleDbType.Double, 8, "post_persuasion_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "units_desired_trigger", System.Data.OleDb.OleDbType.Double, 8, "units_desired_trigger" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_model", System.Data.OleDb.OleDbType.VarChar, 50, "awareness_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_threshold", System.Data.OleDb.OleDbType.Double, 8, "awareness_threshold" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_decay_rate_pre", System.Data.OleDb.OleDbType.Double, 8, "awareness_decay_rate_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_decay_rate_post", System.Data.OleDb.OleDbType.Double, 8, "awareness_decay_rate_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_rate_pre", System.Data.OleDb.OleDbType.Double, 8, "persuasion_decay_rate_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_rate_post", System.Data.OleDb.OleDbType.Double, 8, "persuasion_decay_rate_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_method", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_decay_method" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_choice_model", System.Data.OleDb.OleDbType.VarChar, 1, "product_choice_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_score", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_value_computation", System.Data.OleDb.OleDbType.VarChar, 20, "persuasion_value_computation" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_contribution_overall_score", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_contribution_overall_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_score", System.Data.OleDb.OleDbType.VarChar, 1, "utility_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "combination_part_utilities", System.Data.OleDb.OleDbType.VarChar, 50, "combination_part_utilities" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_contribution_overall_score", System.Data.OleDb.OleDbType.VarChar, 1, "price_contribution_overall_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_score", System.Data.OleDb.OleDbType.VarChar, 1, "price_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_value", System.Data.OleDb.OleDbType.VarWChar, 50, "price_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "reference_price", System.Data.OleDb.OleDbType.Double, 8, "reference_price" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "choice_prob", System.Data.OleDb.OleDbType.VarChar, 20, "choice_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "inertia_model", System.Data.OleDb.OleDbType.VarWChar, 50, "inertia_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "error_term", System.Data.OleDb.OleDbType.VarChar, 20, "error_term" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "error_term_user_value", System.Data.OleDb.OleDbType.Integer, 4, "error_term_user_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "loyalty", System.Data.OleDb.OleDbType.Double, 8, "loyalty" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "min_freq", System.Data.OleDb.OleDbType.Double, 8, "min_freq" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "max_freq", System.Data.OleDb.OleDbType.Double, 8, "max_freq" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE segment SET  " +
                "segment_model = ?, " +
                "segment_name = ?, " +
                "color = ?, " +
                "show_current_share_pie_chart = ?, " +
                "show_cmulative_share_chart = ?, " +
                "segment_size = ?, " +
                "growth_rate = ?, " +
                "growth_rate_people_percent = ?, " +
                "growth_rate_month_year = ?, " +
                "compress_population = ?, " +				// 10
                "variability = ?, " +
                "price_disutility = ?, " +
                "attribute_sensitivity = ?, " +
                "persuasion_scaling = ?, " +
                "display_utility = ?, " +
                "display_utility_scaling_factor = ?, " +
                "max_display_hits_per_trip = ?, " +
                "inertia = ?, " +
                "repurchase = ?, " +
                "repurchase_model = ?, " +					// 20
                "gamma_location_parameter_a = ?, " +
                "gamma_shape_parameter_k = ?, " +
                "repurchase_period_frequency = ?, " +
                "repurchase_frequency_variation = ?, " +
                "repurchase_timescale = ?, " +
                "avg_max_units_purch = ?, " +
                "shopping_trip_interval = ?, " +
                "category_penetration = ?, " +
                "category_rejection = ?, " +
                "num_initial_buyers = ?, " +				// 30
                "initial_buying_period = ?, " +
                "seed_with_repurchasers = ?, " +
                "use_budget = ?, " +
                "budget = ?, " +
                "budget_period = ?, " +
                "save_unspent = ?, " +
                "initial_savings = ?, " +
                "social_network_model = ?, " +
                "num_close_contacts = ?, " +
                "prob_talking_close_contact_pre = ?, " +	// 40
                "prob_talking_close_contact_post = ?, " +
                "use_local = ?, " +
                "num_distant_contacts = ?, " +
                "prob_talk_distant_contact_pre = ?, " +
                "prob_talk_distant_contact_post = ?, " +
                "awareness_weight_personal_message = ?, " +
                "pre_persuasion_prob = ?, " +
                "post_persuasion_prob = ?, " +
                "units_desired_trigger = ?, " +
                "awareness_model = ?, " +					// 50
                "awareness_threshold = ?, " +
                "awareness_decay_rate_pre = ?, " +
                "awareness_decay_rate_post = ?, " +
                "persuasion_decay_rate_pre = ?, " +
                "persuasion_decay_rate_post = ?, " +
                "persuasion_decay_method = ?, " +
                "product_choice_model = ?, " +
                "persuasion_score = ?, " +
                "persuasion_value_computation = ?, " +
                "persuasion_contribution_overall_score = ?, " +	// 60
                "utility_score = ?, " +
                "combination_part_utilities = ?, " +
                "price_contribution_overall_score = ?, " +
                "price_score = ?, " +
                "price_value = ?, " +
                "reference_price = ?, " +
                "choice_prob = ?, " +
                "inertia_model = ?, " +
                "error_term = ?, " +
                "error_term_user_value = ?, " +				// 70
                "loyalty = ?, " +
                "min_freq = ?, " +
                "max_freq = ? " +
                "WHERE (segment_id = ?) ";

            // this must be in same order as above
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_model", System.Data.OleDb.OleDbType.VarChar, 20, "segment_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_name", System.Data.OleDb.OleDbType.VarChar, 100, "segment_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "color", System.Data.OleDb.OleDbType.VarChar, 50, "color" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "show_current_share_pie_chart", System.Data.OleDb.OleDbType.VarChar, 3, "show_current_share_pie_chart" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "show_cmulative_share_chart", System.Data.OleDb.OleDbType.VarChar, 3, "show_cmulative_share_chart" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_size", System.Data.OleDb.OleDbType.Double, 8, "segment_size" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate", System.Data.OleDb.OleDbType.Double, 8, "growth_rate" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate_people_percent", System.Data.OleDb.OleDbType.VarChar, 20, "growth_rate_people_percent" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "growth_rate_month_year", System.Data.OleDb.OleDbType.VarChar, 20, "growth_rate_month_year" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "compress_population", System.Data.OleDb.OleDbType.VarChar, 3, "compress_population" ) );
            // 10

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "variability", System.Data.OleDb.OleDbType.Integer, 4, "variability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_disutility", System.Data.OleDb.OleDbType.Double, 8, "price_disutility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attribute_sensitivity", System.Data.OleDb.OleDbType.Double, 8, "attribute_sensitivity" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_scaling", System.Data.OleDb.OleDbType.Double, 8, "persuasion_scaling" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "display_utility", System.Data.OleDb.OleDbType.Double, 8, "display_utility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "display_utility_scaling_factor", System.Data.OleDb.OleDbType.Double, 8, "display_utility_scaling_factor" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "max_display_hits_per_trip", System.Data.OleDb.OleDbType.Double, 8, "max_display_hits_per_trip" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "inertia", System.Data.OleDb.OleDbType.Double, 8, "inertia" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase", System.Data.OleDb.OleDbType.VarChar, 3, "repurchase" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_model", System.Data.OleDb.OleDbType.VarChar, 3, "repurchase_model" ) );
            // 20

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "gamma_location_parameter_a", System.Data.OleDb.OleDbType.Double, 8, "gamma_location_parameter_a" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "gamma_shape_parameter_k", System.Data.OleDb.OleDbType.Double, 8, "gamma_shape_parameter_k" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_period_frequency", System.Data.OleDb.OleDbType.Double, 8, "repurchase_period_frequency" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_frequency_variation", System.Data.OleDb.OleDbType.Double, 8, "repurchase_frequency_variation" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "repurchase_timescale", System.Data.OleDb.OleDbType.VarChar, 10, "repurchase_timescale" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "avg_max_units_purch", System.Data.OleDb.OleDbType.Double, 8, "avg_max_units_purch" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "shopping_trip_interval", System.Data.OleDb.OleDbType.Double, 8, "shopping_trip_interval" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "category_penetration", System.Data.OleDb.OleDbType.Integer, 4, "category_penetration" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "category_rejection", System.Data.OleDb.OleDbType.Integer, 4, "category_rejection" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_initial_buyers", System.Data.OleDb.OleDbType.BigInt, 8, "num_initial_buyers" ) );
            // 30

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_buying_period", System.Data.OleDb.OleDbType.Double, 8, "initial_buying_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "seed_with_repurchasers", System.Data.OleDb.OleDbType.VarChar, 1, "seed_with_repurchasers" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "use_budget", System.Data.OleDb.OleDbType.VarChar, 1, "use_budget" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "budget", System.Data.OleDb.OleDbType.Double, 8, "budget" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "budget_period", System.Data.OleDb.OleDbType.VarChar, 30, "budget_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "save_unspent", System.Data.OleDb.OleDbType.VarChar, 1, "save_unspent" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_savings", System.Data.OleDb.OleDbType.Double, 8, "initial_savings" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "social_network_model", System.Data.OleDb.OleDbType.VarChar, 20, "social_network_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_close_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_close_contacts" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talking_close_contact_pre", System.Data.OleDb.OleDbType.Double, 8, "prob_talking_close_contact_pre" ) );
            // 40

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talking_close_contact_post", System.Data.OleDb.OleDbType.Double, 8, "prob_talking_close_contact_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "use_local", System.Data.OleDb.OleDbType.VarChar, 1, "use_local" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "num_distant_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_distant_contacts" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talk_distant_contact_pre", System.Data.OleDb.OleDbType.Double, 8, "prob_talk_distant_contact_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "prob_talk_distant_contact_post", System.Data.OleDb.OleDbType.Double, 8, "prob_talk_distant_contact_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_weight_personal_message", System.Data.OleDb.OleDbType.Double, 8, "awareness_weight_personal_message" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_persuasion_prob", System.Data.OleDb.OleDbType.Double, 8, "pre_persuasion_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_persuasion_prob", System.Data.OleDb.OleDbType.Double, 8, "post_persuasion_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "units_desired_trigger", System.Data.OleDb.OleDbType.Double, 8, "units_desired_trigger" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_model", System.Data.OleDb.OleDbType.VarChar, 50, "awareness_model" ) );
            // 50

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_threshold", System.Data.OleDb.OleDbType.Double, 8, "awareness_threshold" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_decay_rate_pre", System.Data.OleDb.OleDbType.Double, 8, "awareness_decay_rate_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness_decay_rate_post", System.Data.OleDb.OleDbType.Double, 8, "awareness_decay_rate_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_rate_pre", System.Data.OleDb.OleDbType.Double, 8, "persuasion_decay_rate_pre" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_rate_post", System.Data.OleDb.OleDbType.Double, 8, "persuasion_decay_rate_post" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_decay_method", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_decay_method" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_choice_model", System.Data.OleDb.OleDbType.VarChar, 1, "product_choice_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_score", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_value_computation", System.Data.OleDb.OleDbType.VarChar, 20, "persuasion_value_computation" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion_contribution_overall_score", System.Data.OleDb.OleDbType.VarChar, 1, "persuasion_contribution_overall_score" ) );
            // 60

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_score", System.Data.OleDb.OleDbType.VarChar, 1, "utility_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "combination_part_utilities", System.Data.OleDb.OleDbType.VarChar, 50, "combination_part_utilities" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_contribution_overall_score", System.Data.OleDb.OleDbType.VarChar, 1, "price_contribution_overall_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_score", System.Data.OleDb.OleDbType.VarChar, 1, "price_score" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_value", System.Data.OleDb.OleDbType.VarWChar, 50, "price_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "reference_price", System.Data.OleDb.OleDbType.Double, 8, "reference_price" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "choice_prob", System.Data.OleDb.OleDbType.VarChar, 20, "choice_prob" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "inertia_model", System.Data.OleDb.OleDbType.VarWChar, 50, "inertia_model" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "error_term", System.Data.OleDb.OleDbType.VarChar, 20, "error_term" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "error_term_user_value", System.Data.OleDb.OleDbType.Integer, 4, "error_term_user_value" ) );
            // 70

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "loyalty", System.Data.OleDb.OleDbType.Double, 8, "loyalty" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "min_freq", System.Data.OleDb.OleDbType.Double, 8, "min_freq" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "max_freq", System.Data.OleDb.OleDbType.Double, 8, "max_freq" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM segment WHERE (segment_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_ProductAttributes_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO product_attribute" +
                "(model_id, product_attribute_name, utility_max, utility_min, cust_pref_max, cust_pref_min, cust_tau, type) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

          
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_attribute_name", System.Data.OleDb.OleDbType.VarChar, 100, "product_attribute_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_max", System.Data.OleDb.OleDbType.Double, 8, "utility_max" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_min", System.Data.OleDb.OleDbType.Double, 8, "utility_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_pref_max", System.Data.OleDb.OleDbType.Double, 8, "cust_pref_max" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_pref_min", System.Data.OleDb.OleDbType.Double, 8, "cust_pref_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_tau", System.Data.OleDb.OleDbType.Double, 8, "cust_tau" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.Integer, 4, "type" ) );

            // 
            // update
            // 

            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE product_attribute SET " +
                "product_attribute_name = ?, utility_max = ?, utility_min = ?, cust_pref_max = ?, cust_pref_min = ?, cust_tau = ?, type = ? " +
                "WHERE (product_attribute_id = ?)";

            //command.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_attribute_name", System.Data.OleDb.OleDbType.VarChar, 100, "product_attribute_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_max", System.Data.OleDb.OleDbType.Double, 8, "utility_max" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility_min", System.Data.OleDb.OleDbType.Double, 8, "utility_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_pref_max", System.Data.OleDb.OleDbType.Double, 8, "cust_pref_max" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_pref_min", System.Data.OleDb.OleDbType.Double, 8, "cust_pref_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "cust_tau", System.Data.OleDb.OleDbType.Double, 8, "cust_tau" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.Integer, 4, "type" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_attribute_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_attribute_id", System.Data.DataRowVersion.Original, null ) );
            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM product_attribute WHERE (product_attribute_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_attribute_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_attribute_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Task_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;
            command.CommandText = "INSERT INTO task(model_id, task_name, start_date, end_date, suitability_min, suitability_max) " +
                "VALUES (?, ?, ?, ?, ?, ?) ";

        
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_name", System.Data.OleDb.OleDbType.VarChar, 100, "task_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability_min", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability_max", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability_max" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE task " +
                "SET task_name = ?, start_date = ?, end_date = ?, suitability_min = ?, suitability_max = ? " +
                "WHERE (task_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_name", System.Data.OleDb.OleDbType.VarChar, 100, "task_name" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability_min", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability_min" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability_max", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability_max" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM task WHERE (task_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_ConsumerPref_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;
            command.CommandText = "INSERT INTO consumer_preference(model_id, segment_id, product_attribute_id, start_date, pre_preference_value, post_preference_value, price_sensitivity) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?)";

     
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_attribute_id", System.Data.OleDb.OleDbType.Integer, 4, "product_attribute_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_preference_value", System.Data.OleDb.OleDbType.Double, 8, "pre_preference_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_preference_value", System.Data.OleDb.OleDbType.Double, 8, "post_preference_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_sensitivity", System.Data.OleDb.OleDbType.Double, 8, "price_sensitivity" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE consumer_preference " +
                "SET start_date = ?, pre_preference_value = ?, post_preference_value = ?, price_sensitivity = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_preference_value", System.Data.OleDb.OleDbType.Double, 8, "pre_preference_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_preference_value", System.Data.OleDb.OleDbType.Double, 8, "post_preference_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_sensitivity", System.Data.OleDb.OleDbType.Double, 8, "price_sensitivity" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM consumer_preference WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_ProdAttributeValues_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO product_attribute_value" +
                "(model_id, product_id, product_attribute_id, start_date, " +
                "pre_attribute_value, post_attribute_value, has_attribute) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?) ";

        

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_attribute_id", System.Data.OleDb.OleDbType.Integer, 4, "product_attribute_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_attribute_value", System.Data.OleDb.OleDbType.Double, 8, "pre_attribute_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_attribute_value", System.Data.OleDb.OleDbType.Double, 8, "post_attribute_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "has_attribute", System.Data.OleDb.OleDbType.Boolean, 1, "has_attribute" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE product_attribute_value SET " +
                "product_id = ?, product_attribute_id = ?, start_date = ?, " +
                "pre_attribute_value = ?, post_attribute_value = ?, " +
                "has_attribute = ? " +
                " WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_attribute_id", System.Data.OleDb.OleDbType.Integer, 4, "product_attribute_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_attribute_value", System.Data.OleDb.OleDbType.Double, 8, "pre_attribute_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_attribute_value", System.Data.OleDb.OleDbType.Double, 8, "post_attribute_value" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "has_attribute", System.Data.OleDb.OleDbType.Boolean, 1, "has_attribute" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM product_attribute_value WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_ProdDependencies_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO product_matrix" +
                "(model_id, have_product_id, want_product_id, value) " +
                "VALUES (?, ?, ?, ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "have_product_id", System.Data.OleDb.OleDbType.Integer, 4, "have_product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "want_product_id", System.Data.OleDb.OleDbType.Integer, 4, "want_product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "value", System.Data.OleDb.OleDbType.VarChar, 25, "value" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE product_matrix SET " +
                "value = ? " +
                "WHERE (model_id = ?) AND (have_product_id = ?) AND (want_product_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "value", System.Data.OleDb.OleDbType.VarChar, 18, "value" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_have_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "have_product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_want_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "want_product_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM product_matrix WHERE (model_id = ?) AND (have_product_id = ?) AND (want_product_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_have_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "have_product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_want_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "want_product_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_SegmentChannel_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;
            command.CommandText = "INSERT INTO segment_channel" +
                "(model_id, segment_id, channel_id, probability_of_choice) " +
                "VALUES (?, ?, ?, ?); ";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "probability_of_choice", System.Data.OleDb.OleDbType.VarChar, 18, "probability_of_choice" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE segment_channel SET " +
                "probability_of_choice = ? " +
                "WHERE (model_id = ?) AND (segment_id = ?) AND (channel_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "probability_of_choice", System.Data.OleDb.OleDbType.VarChar, 18, "probability_of_choice" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null ) );
            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM segment_channel WHERE (model_id = ?) AND (segment_id = ?) AND (channel_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null ) );


        }

        static public void initialize_SharePenAwareness_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;
            command.CommandText = "INSERT INTO share_pen_brand_aware(model_id, product_id, segment_id, initial_share, penetration, brand_awareness, persuasion) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?); ";// + // refresh
        
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_share", System.Data.OleDb.OleDbType.Double, 8, "initial_share" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "penetration", System.Data.OleDb.OleDbType.Double, 8, "penetration" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "brand_awareness", System.Data.OleDb.OleDbType.Double, 8, "brand_awareness" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.Double, 8, "persuasion" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE share_pen_brand_aware SET initial_share = ?, penetration = ?, brand_awareness = ?, persuasion = ? " +
                "WHERE (model_id = ?) AND (product_id = ?) AND (segment_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "initial_share", System.Data.OleDb.OleDbType.Double, 8, "initial_share" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "penetration", System.Data.OleDb.OleDbType.Double, 8, "penetration" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "brand_awareness", System.Data.OleDb.OleDbType.Double, 8, "brand_awareness" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.Double, 8, "persuasion" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_seegment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM share_pen_brand_aware WHERE (model_id = ?) AND (product_id = ?) AND (segment_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
        }


        static public void initialize_TaskProd_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO task_product_fact" +
                "(model_id, product_id, task_id,  pre_use_upsku, post_use_upsku, suitability) " +
                "VALUES (?, ?, ?, ?, ?, ?); ";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_use_upsku", System.Data.OleDb.OleDbType.Integer, 4, "pre_use_upsku" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_use_upsku", System.Data.OleDb.OleDbType.Integer, 4, "post_use_upsku" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;
            command.CommandText = "UPDATE task_product_fact " +
                "SET pre_use_upsku = ?, post_use_upsku = ?, suitability = ? " +
                "WHERE (model_id = ?) AND (product_id = ?) AND (task_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pre_use_upsku", System.Data.OleDb.OleDbType.Integer, 4, "pre_use_upsku" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "post_use_upsku", System.Data.OleDb.OleDbType.Integer, 4, "post_use_upsku" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "suitability", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "suitability" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;
            command.CommandText = "DELETE FROM task_product_fact WHERE (model_id = ?) AND (product_id = ?) AND (task_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_TaskSegment_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText =
                "INSERT INTO task_rate_fact" +
                "(model_id, segment_id, task_id, start_date, end_date, time_period, task_rate) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?);"; // no refresh needed

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "time_period", System.Data.OleDb.OleDbType.VarChar, 20, "time_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_rate", System.Data.OleDb.OleDbType.Double, 8, "task_rate" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE task_rate_fact " +
                "SET start_date = ?, end_date = ?, time_period = ?, task_rate = ? " +
                "WHERE (model_id = ?) AND(segment_id = ?) AND (task_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "time_period", System.Data.OleDb.OleDbType.VarChar, 20, "time_period" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_rate", System.Data.OleDb.OleDbType.Double, 8, "task_rate" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );


            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM task_rate_fact WHERE (model_id = ?) AND (segment_id = ?) AND (task_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_task_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "task_id", System.Data.DataRowVersion.Original, null ) );

        }



        static public void initialize_MassMedia_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO mass_media" +
                "(market_plan_id, model_id, product_id, channel_id, segment_id, media_type, attr_value_G, attr_value_H, attr_value_I, message_awareness_probability, message_persuation_probability, start_date, end_date)" +
                " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?); ";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "media_type", System.Data.OleDb.OleDbType.VarWChar, 1, "media_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_G", System.Data.OleDb.OleDbType.Double, 8, "attr_value_G" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_H", System.Data.OleDb.OleDbType.Double, 8, "attr_value_H" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_I", System.Data.OleDb.OleDbType.Double, 8, "attr_value_I" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            // 
            // oleDbUpdateCommand
            // 
            command.CommandText = "UPDATE mass_media SET " +
                "market_plan_id = ?, model_id = ?, product_id = ?, channel_id = ?, segment_id = ?, media_type = ?, attr_value_G = ?, attr_value_H = ?, attr_value_I = ?, message_awareness_probability = ?, message_persuation_probability = ?, start_date = ?, end_date = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "media_type", System.Data.OleDb.OleDbType.VarWChar, 1, "media_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_G", System.Data.OleDb.OleDbType.Double, 8, "attr_value_G" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_H", System.Data.OleDb.OleDbType.Double, 8, "attr_value_H" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_I", System.Data.OleDb.OleDbType.Double, 8, "attr_value_I" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM mass_media WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }


        static public void initialize_Display_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO dist_display" +
                "(market_plan_id, model_id, product_id, channel_id, media_type, attr_value_F, message_awareness_probability, message_persuation_probability, start_date, end_date) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "media_type", System.Data.OleDb.OleDbType.VarChar, 20, "media_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_F", System.Data.OleDb.OleDbType.Double, 8, "attr_value_F" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE dist_display SET " +
                "market_plan_id = ?, model_id = ?, product_id = ?, channel_id = ?, media_type = ?, attr_value_F = ?, message_awareness_probability = ?, message_persuation_probability = ?, start_date = ?, end_date = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "media_type", System.Data.OleDb.OleDbType.VarChar, 20, "media_type" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_F", System.Data.OleDb.OleDbType.Double, 8, "attr_value_F" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );



            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM dist_display WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Market_Utility_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO market_utility" +
                "(market_plan_id, model_id, product_id, channel_id, segment_id, percent_dist, awareness, persuasion, utility, start_date, end_date) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "percent_dist", System.Data.OleDb.OleDbType.Double, 8, "percent_dist" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness", System.Data.OleDb.OleDbType.Double, 8, "awareness" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.Double, 8, "persuasion" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility", System.Data.OleDb.OleDbType.Double, 8, "utility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE market_utility SET " +
                "market_plan_id = ?, model_id = ?, product_id = ?, channel_id = ?, segment_id = ?, percent_dist = ?, awareness = ?, persuasion = ?, utility = ?, start_date = ?, end_date = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "percent_dist", System.Data.OleDb.OleDbType.Double, 8, "percent_dist" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness", System.Data.OleDb.OleDbType.Double, 8, "awareness" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.Double, 8, "persuasion" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "utility", System.Data.OleDb.OleDbType.Double, 8, "utility" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );



            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM market_utility WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Distribution_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            // NOTE: media_type is always 'D'
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO dist_display" +
                "(market_plan_id, model_id, product_id, channel_id, media_type, attr_value_F, attr_value_G, message_awareness_probability, message_persuation_probability, start_date, end_date) " +
                "VALUES (?, ?, ?, ?, 'D', ?, ?, ?, ?, ?, ?)";

       
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_F", System.Data.OleDb.OleDbType.Double, 8, "attr_value_F" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_G", System.Data.OleDb.OleDbType.Double, 8, "attr_value_G" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );

            // 
            // update
            // 
            // NOTE: the media type is always 'D'
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE dist_display SET " +
                "market_plan_id = ?, model_id = ?, product_id = ?, channel_id = ?, media_type = 'D', attr_value_F = ?, attr_value_G = ?, message_awareness_probability = ?, message_persuation_probability = ?, start_date = ?, end_date = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_F", System.Data.OleDb.OleDbType.Double, 8, "attr_value_F" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "attr_value_G", System.Data.OleDb.OleDbType.Double, 8, "attr_value_G" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_awareness_probability", System.Data.OleDb.OleDbType.Double, 8, "message_awareness_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "message_persuation_probability", System.Data.OleDb.OleDbType.Double, 8, "message_persuation_probability" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );

            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM dist_display WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Product_Event_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO product_event" +
                "(market_plan_id, model_id, segment_id, channel_id, product_id,  demand_modification, start_date, end_date, type) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "demand_modification", System.Data.OleDb.OleDbType.Double, 8, "demand_modification" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE product_event SET " +
                "market_plan_id = ?, model_id = ?, segment_id = ?, channel_id = ?, product_id = ?,  demand_modification = ?, start_date = ?, end_date = ?, type = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "demand_modification", System.Data.OleDb.OleDbType.Double, 8, "demand_modification" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type" ) );

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );


            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM product_event WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_Task_Event_UpdateQuery( OleDbDataAdapter adapter )
        {
            OleDbCommand command;

            // 
            // Insert
            //
            command = adapter.InsertCommand;

            command.CommandText = "INSERT INTO task_event" +
                "(market_plan_id, model_id, segment_id, task_id, demand_modification, start_date, end_date) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "demand_modification", System.Data.OleDb.OleDbType.Double, 8, "demand_modification" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );

            // 
            // update
            // 
            command = adapter.UpdateCommand;

            command.CommandText = "UPDATE task_event SET " +
                "market_plan_id = ?, model_id = ?, segment_id = ?, task_id = ?, demand_modification = ?, start_date = ?, end_date = ? " +
                "WHERE (record_id = ?)";

            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "demand_modification", System.Data.OleDb.OleDbType.Double, 8, "demand_modification" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date" ) );
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );


            //
            // Delete
            // 
            command = adapter.DeleteCommand;

            command.CommandText = "DELETE FROM task_event WHERE (record_id = ?)";
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_ProductChannelSize_UpdateQuery(OleDbDataAdapter adapter)
        {

            // 
            // oleDbInsertCommand
            //
            adapter.InsertCommand.CommandText = "INSERT INTO product_channel_size" +
                "(model_id, product_id, channel_id, prod_size) VALUES (?, ?, ?, ?)";

            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
            adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prod_size", System.Data.OleDb.OleDbType.Double, 8, "prod_size"));

            // 
            // oleDbUpdateCommand
            // 

            adapter.UpdateCommand.CommandText = @"UPDATE product_channel_size SET " +
                " prod_size = ? WHERE model_id = ? AND product_id = ? AND channel_id = ?";

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prod_size", System.Data.OleDb.OleDbType.Double, 8, "prod_size"));

            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalModel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalProd_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null));
            adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalChan_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null));


            // 
            // oleDbDeleteCommand
            // 
            adapter.DeleteCommand.CommandText = "DELETE FROM product_channel_size WHERE model_id = ? AND product_id = ? AND channel_id = ?";

            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalModel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalProd_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null));
            adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OriginalChan_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null));

        }

		static public void initialize_MarketPlan_UpdateQuery(OleDbDataAdapter adapter)
		{
			const int numParms = 6;
			
			// 
			// oleDbInsertCommand
			// 
			adapter.InsertCommand.CommandText = "INSERT INTO market_plan" +
				"(model_id, name, descr, start_date, end_date, interval, " +
				"product_id, segment_id, channel_id, task_id, type, user_name";

			for(int ii = 1; ii <= numParms; ++ii)
			{
				string parm = ", parm" + ii;
				adapter.InsertCommand.CommandText += parm;
			}

			adapter.InsertCommand.CommandText += ") ";

			adapter.InsertCommand.CommandText += "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?";

			for(int ii = 1; ii <= numParms; ++ii)
			{
				string quest = ", ?";
				adapter.InsertCommand.CommandText += quest;
			}

			adapter.InsertCommand.CommandText += ")";
			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("interval", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "interval"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("user_name", System.Data.OleDb.OleDbType.VarChar, 100, "user_name"));

			// parms
			for(int ii = 1; ii <= numParms; ii++)
			{
				string parm = "parm" + ii;
				adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(parm, System.Data.OleDb.OleDbType.Double, 8, parm));
			}

			// 
			// oleDbUpdateCommand
			// 
			adapter.UpdateCommand.CommandText = "UPDATE market_plan SET " +
				"model_id = ?, name = ?, descr = ?, start_date = ?, end_date = ?, interval = ?, " +
				"product_id = ?, segment_id = ?, channel_id = ?, task_id = ?, type = ?, user_name = ?";

			for(int ii = 1; ii <= numParms; ++ii)
			{
				string quest = ", parm" + ii + " = ?";
				adapter.UpdateCommand.CommandText += quest;
			}
				
			adapter.UpdateCommand.CommandText += " WHERE (id = ?)";
			
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("descr", System.Data.OleDb.OleDbType.VarChar, 200, "descr"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("interval", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "interval"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("task_id", System.Data.OleDb.OleDbType.Integer, 4, "task_id"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.UnsignedTinyInt, 1, "type"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("user_name", System.Data.OleDb.OleDbType.VarChar, 100, "user_name"));

			// parms
			for(int ii = 1; ii <= numParms; ii++)
			{
				string parm = "parm" + ii;
				adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(parm, System.Data.OleDb.OleDbType.Double, 8, parm));
			}

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));

			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM market_plan WHERE (id = ?)";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
		}

		static public void initialize_ProdChannel_UpdateQuery(OleDbDataAdapter adapter)
		{
			OleDbCommand command;
			
			// 
			// Insert
			//
			command = adapter.InsertCommand;

			command.CommandText = "INSERT INTO product_channel" +
				"(market_plan_id, model_id, product_id, channel_id, markup, " +
				"price, periodic_price, how_often, percent_SKU_in_dist, price_type, " +
				"start_date, end_date) " +
				"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"; //refresh
	
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("markup", System.Data.OleDb.OleDbType.Double, 8, "markup"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("price", System.Data.OleDb.OleDbType.Double, 8, "price"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("periodic_price", System.Data.OleDb.OleDbType.Double, 8, "periodic_price"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("how_often", System.Data.OleDb.OleDbType.VarChar, 50, "how_often"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_SKU_in_dist", System.Data.OleDb.OleDbType.Double, 8, "percent_SKU_in_dist"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("price_type", System.Data.OleDb.OleDbType.Integer, 4, "price_type"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));

			// 
			// update
			// 
			command = adapter.UpdateCommand;

			command.CommandText = "UPDATE product_channel SET " +
				"market_plan_id = ?, model_id = ?, product_id = ?, channel_id = ?, markup = ?, price = ?, periodic_price = ?, how_often = ?, percent_SKU_in_dist = ?, price_type = ?, start_date = ?, end_date = ? " +
				"WHERE record_id = ?";
	
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("markup", System.Data.OleDb.OleDbType.Double, 8, "markup"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("price", System.Data.OleDb.OleDbType.Double, 8, "price"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("periodic_price", System.Data.OleDb.OleDbType.Double, 8, "periodic_price"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("how_often", System.Data.OleDb.OleDbType.VarChar, 50, "how_often"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_SKU_in_dist", System.Data.OleDb.OleDbType.Double, 8, "percent_SKU_in_dist"));
            command.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_type", System.Data.OleDb.OleDbType.Integer, 4, "price_type" ) );
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));

			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null));

			//
			// Delete
			// 
			command = adapter.DeleteCommand;
			command.CommandText = "DELETE FROM product_channel WHERE record_id = ?";
			
			command.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "record_id", System.Data.DataRowVersion.Original, null));
	
		}


		static public void initialize_MarketPlanTree_UpdateQuery(OleDbDataAdapter adapter)
		{
			
			// 
			// oleDbInsertCommand
			//
			adapter.InsertCommand.CommandText = "INSERT INTO market_plan_tree" +
				"(model_id, parent_id, child_id) VALUES (?, ?, ?)";
	
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("parent_id", System.Data.OleDb.OleDbType.Integer, 4, "parent_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("child_id", System.Data.OleDb.OleDbType.Integer, 4, "child_id"));
			
			// 
			// NO oleDbUpdateCommand
			// 
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM market_plan_tree WHERE model_id = ? AND parent_id = ? AND child_id = ?";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "parent_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "child_id", System.Data.DataRowVersion.Original, null));
		}

		static public void initialize_ProductTree_UpdateQuery(OleDbDataAdapter adapter)
		{
			
			// 
			// oleDbInsertCommand
			//
			adapter.InsertCommand.CommandText = "INSERT INTO product_tree" +
				"(model_id, parent_id, child_id) VALUES (?, ?, ?)";
	
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("parent_id", System.Data.OleDb.OleDbType.Integer, 4, "parent_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("child_id", System.Data.OleDb.OleDbType.Integer, 4, "child_id"));
			
			// 
			// NO oleDbUpdateCommand
			// 
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM product_tree WHERE model_id = ? AND parent_id = ? AND child_id = ?";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "parent_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "child_id", System.Data.DataRowVersion.Original, null));
		}


		static public void initialize_ProductType_UpdateQuery(OleDbDataAdapter adapter)
		{
			
			// 
			// oleDbInsertCommand
			//
			adapter.InsertCommand.CommandText = "INSERT INTO product_type" +
				"(model_id, type_name) VALUES (?, ?)";
	
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			//adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("id", System.Data.OleDb.OleDbType.Integer, 4, "id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type_name", System.Data.OleDb.OleDbType.VarChar, 25, "type_name"));
			
			// 
			// NO oleDbUpdateCommand
			// 
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM product_type WHERE model_id = ? AND id = ?";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
			
		}

        static public void initialize_PackSize_UpdateQuery( OleDbDataAdapter adapter )
        {
            // 
            // oleDbInsertCommand
            //
            adapter.InsertCommand.CommandText = "INSERT INTO pack_size " +
                "(model_id, name) VALUES (?, ?)";

            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "name", System.Data.OleDb.OleDbType.VarChar, 50, "name" ) );

            // 
            // update
            // 

            adapter.UpdateCommand.CommandText = "UPDATE pack_size SET " +
                "model_id = ?, name = ? " +
                "WHERE id = ?";

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "name", System.Data.OleDb.OleDbType.VarChar, 50, "name" ) );

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null ) );

            // 
            // oleDbDeleteCommand
            // 
            adapter.DeleteCommand.CommandText = "DELETE FROM pack_size WHERE model_id = ? AND id = ?";

            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null ) );

        }


        static public void initialize_PackSizeDist_UpdateQuery( OleDbDataAdapter adapter )
        {
            // 
            // oleDbInsertCommand
            //
            adapter.InsertCommand.CommandText = "INSERT INTO pack_size_dist " +
                "(pack_size_id, size, dist) VALUES (?, ?, ?)";

            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "pack_size_id", System.Data.OleDb.OleDbType.Integer, 4, "pack_size_id" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "size", System.Data.OleDb.OleDbType.Integer, 4, "size" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "dist", System.Data.OleDb.OleDbType.Double, 8, "dist" ) );
            // 
            // update
            // 

            adapter.UpdateCommand.CommandText = "UPDATE pack_size_dist SET " +
                "dist = ? " +
                "WHERE pack_size_id = ? AND size = ?";

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "dist", System.Data.OleDb.OleDbType.Double, 8, "dist" ) );

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_pack_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "pack_size_id", System.Data.DataRowVersion.Original, null ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_size", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "size", System.Data.DataRowVersion.Original, null ) );

            // 
            // oleDbDeleteCommand
            // 
            adapter.DeleteCommand.CommandText = "DELETE FROM pack_size_dist WHERE pack_size_id = ? AND size = ?";

            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_pack_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "pack_size_id", System.Data.DataRowVersion.Original, null ) );
            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_size", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "size", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_PriceType_UpdateQuery( OleDbDataAdapter adapter )
        {
            // 
            // oleDbInsertCommand
            //
            adapter.InsertCommand.CommandText = "INSERT INTO price_type " +
                "(model_id, name, relative, awareness, persuasion, BOGN) VALUES (?, ?, ?, ?, ?, ?)";

            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "name", System.Data.OleDb.OleDbType.VarChar, 50, "name" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "relative", System.Data.OleDb.OleDbType.VarChar, 50, "relative" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness", System.Data.OleDb.OleDbType.VarChar, 50, "awareness" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.VarChar, 50, "persuasion" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "BOGN", System.Data.OleDb.OleDbType.VarChar, 50, "BOGN" ) );

            // 
            // update
            // 
            adapter.UpdateCommand.CommandText = "UPDATE price_type SET " +
                "model_id = ?, name = ?, relative = ?, awareness = ?, persuasion = ?, BOGN = ? " +
                "WHERE id = ?";

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "name", System.Data.OleDb.OleDbType.VarChar, 50, "name" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "relative", System.Data.OleDb.OleDbType.VarChar, 50, "relative" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "awareness", System.Data.OleDb.OleDbType.VarChar, 50, "awareness" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "persuasion", System.Data.OleDb.OleDbType.VarChar, 50, "persuasion" ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "BOGN", System.Data.OleDb.OleDbType.VarChar, 50, "BOGN" ) );
          
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_record_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null ) );

            // 
            // oleDbDeleteCommand
            // 
            adapter.DeleteCommand.CommandText = "DELETE FROM price_type WHERE model_id = ? AND id = ?";

            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null ) );
            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null ) );
        }

        static public void initialize_SegmentPriceUtility_UpdateQuery( OleDbDataAdapter adapter )
        {
            // 
            // oleDbInsertCommand
            //
            adapter.InsertCommand.CommandText = "INSERT INTO segment_price_utility " +
                "(segment_id, price_type_id, util) VALUES (?, ?, ?)";

            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "price_type_id", System.Data.OleDb.OleDbType.Integer, 4, "price_type_id" ) );
            adapter.InsertCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "util", System.Data.OleDb.OleDbType.Double, 8, "util" ) );

            // 
            // update
            // 
            adapter.UpdateCommand.CommandText = "UPDATE segment_price_utility SET " +
                "util = ? " +
                "WHERE segment_id = ? AND price_type_id = ?";

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "util", System.Data.OleDb.OleDbType.Double, 8, "util" ) );

            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            adapter.UpdateCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_price_type_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "price_type_id", System.Data.DataRowVersion.Original, null ) );

            // 
            // oleDbDeleteCommand
            // 
            adapter.DeleteCommand.CommandText = "DELETE FROM segment_price_utility WHERE segment_id = ? AND price_type_id = ?";

            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null ) );
            adapter.DeleteCommand.Parameters.Add( new System.Data.OleDb.OleDbParameter( "Original_price_type_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "price_type_id", System.Data.DataRowVersion.Original, null ) );

        }

		static public void initialize_ScenarioMarketPlan_UpdateQuery(OleDbDataAdapter adapter)
		{	
			// 
			// oleDbInsertCommand
			//
			adapter.InsertCommand.CommandText = "INSERT INTO scenario_market_plan" +
				"(model_id, scenario_id, market_plan_id) VALUES (?, ?, ?)";
	
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("scenario_id", System.Data.OleDb.OleDbType.Integer, 4, "scenario_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, "market_plan_id"));
			
			// 
			// NO oleDbUpdateCommand
			// 
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = "DELETE FROM scenario_market_plan WHERE model_id = ? AND scenario_id = ? AND market_plan_id = ?";
			
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_scenario_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "scenario_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_market_plan_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "market_plan_id", System.Data.DataRowVersion.Original, null));
		}


		static public void initialize_network_parameter_UpdateQuery(OleDbDataAdapter adapter)
		{
			// 
			// oleDbInsertCommand
			// 
			adapter.InsertCommand.CommandText = @"INSERT INTO network_parameter" +
				"(model_id, name, type, persuasion_pre_use, persuasion_post_use, awareness_weight, num_contacts, prob_of_talking_pre_use, prob_of_talking_post_use, " +
				"use_local, percent_talking, neg_persuasion_reject, neg_persuasion_pre_use, neg_persuasion_post_use)" +
				"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";
			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.Integer, 4, "type"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("persuasion_pre_use", System.Data.OleDb.OleDbType.Double, 8, "persuasion_pre_use"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("persuasion_post_use", System.Data.OleDb.OleDbType.Double, 8, "persuasion_post_use"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("awareness_weight", System.Data.OleDb.OleDbType.Double, 8, "awareness_weight"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_contacts"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prob_of_talking_pre_use", System.Data.OleDb.OleDbType.Double, 8, "prob_of_talking_pre_use"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prob_of_talking_post_use", System.Data.OleDb.OleDbType.Double, 8, "prob_of_talking_post_use"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("use_local", System.Data.OleDb.OleDbType.Boolean, 1, "use_local"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_talking", System.Data.OleDb.OleDbType.Double, 8, "percent_talking"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_reject", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_reject"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_pre_use", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_pre_use"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_post_use", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_post_use"));
			
			// 
			// oleDbUpdateCommand
			// 

			adapter.UpdateCommand.CommandText = @"UPDATE network_parameter SET " +
				" name = ?, type = ?, persuasion_pre_use = ?, persuasion_post_use = ?, awareness_weight = ?, " +
				" num_contacts = ?, prob_of_talking_pre_use = ?, prob_of_talking_post_use = ?, use_local = ?, " +
				" percent_talking = ?, neg_persuasion_reject = ?, neg_persuasion_pre_use = ?, neg_persuasion_post_use = ? " +
				" WHERE id = ? ";

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 100, "name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.Integer, 4, "type"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("persuasion_pre_use", System.Data.OleDb.OleDbType.Double, 8, "persuasion_pre_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("persuasion_post_use", System.Data.OleDb.OleDbType.Double, 8, "persuasion_post_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("awareness_weight", System.Data.OleDb.OleDbType.Double, 8, "awareness_weight"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("num_contacts", System.Data.OleDb.OleDbType.Double, 8, "num_contacts"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prob_of_talking_pre_use", System.Data.OleDb.OleDbType.Double, 8, "prob_of_talking_pre_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("prob_of_talking_post_use", System.Data.OleDb.OleDbType.Double, 8, "prob_of_talking_post_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("use_local", System.Data.OleDb.OleDbType.Boolean, 1, "use_local"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("percent_talking", System.Data.OleDb.OleDbType.Double, 8, "percent_talking"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_reject", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_reject"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_pre_use", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_pre_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("neg_persuasion_post_use", System.Data.OleDb.OleDbType.Double, 8, "neg_persuasion_post_use"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
			
			// 
			// oleDbDeleteCommand
			// 
			adapter.DeleteCommand.CommandText = @"DELETE FROM network_parameter WHERE id = ?";
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
		}

		static public void initialize_segment_network_UpdateQuery(OleDbDataAdapter adapter)
		{

			adapter.InsertCommand.CommandText = "INSERT INTO segment_network" +
				"(model_id, from_id, to_id, network_param) VALUES (?, ?, ?, ?);";

			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("from_id", System.Data.OleDb.OleDbType.Integer, 4, "from_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("to_id", System.Data.OleDb.OleDbType.Integer, 4, "to_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("network_param", System.Data.OleDb.OleDbType.Integer, 4, "network_param"));

			adapter.DeleteCommand.CommandText = @"DELETE FROM segment_network " +
				" WHERE (from_id = ?) AND (to_id = ?) AND (network_param = ?)";

			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_from_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "from_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_to_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "to_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_network_param", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "network_param", System.Data.DataRowVersion.Original, null));
		}

		static public void initialize_external_data_UpdateQuery(OleDbDataAdapter adapter)
		{
			//
			// insert
			//
			adapter.InsertCommand.CommandText = "INSERT INTO external_data" +
				"(model_id, calendar_date, segment_id, product_id, channel_id, type, value) " + 
				" VALUES (?, ?, ?, ?, ?, ?, ?);";
			
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("calendar_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "calendar_date"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("segment_id", System.Data.OleDb.OleDbType.Integer, 4, "segment_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("product_id", System.Data.OleDb.OleDbType.Integer, 4, "product_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("channel_id", System.Data.OleDb.OleDbType.Integer, 4, "channel_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("type", System.Data.OleDb.OleDbType.Integer, 4, "type"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("value", System.Data.OleDb.OleDbType.Double, 8, "value"));

			//
			// update - only value
			//
			adapter.UpdateCommand.CommandText = @"Update external_data SET value = ? " +
				" WHERE model_id = ? AND calendar_date = ? AND segment_id = ? AND product_id = ? AND channel_id = ? AND type = ?";

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("value", System.Data.OleDb.OleDbType.Double, 8, "value"));

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_calendar_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "calendar_date", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_type", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "type", System.Data.DataRowVersion.Original, null));

			//
			// delete
			//
			adapter.DeleteCommand.CommandText = @"DELETE FROM external_data " +
				" WHERE model_id = ? AND calendar_date = ? AND segment_id = ? AND product_id = ? AND channel_id = ? AND type = ?";

			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_calendar_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "calendar_date", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_segment_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "segment_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_product_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "product_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_channel_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "channel_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("orig_type", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "type", System.Data.DataRowVersion.Original, null));
		}


		static public void initialize_model_parameter_UpdateQuery(OleDbDataAdapter adapter)
		{
			adapter.InsertCommand.CommandText = "INSERT INTO model_parameter" +
				"(model_id, name, table_name, col_name, filter, identity_row, row_id) VALUES (?, ?, ?, ?, ?, ?, ?);";

			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_id", System.Data.OleDb.OleDbType.Integer, 4, "model_id"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 50, "name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("table_name", System.Data.OleDb.OleDbType.VarChar, 50, "table_name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("col_name", System.Data.OleDb.OleDbType.VarChar, 50, "col_name"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("filter", System.Data.OleDb.OleDbType.VarChar, 50, "filter"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("identity_row", System.Data.OleDb.OleDbType.VarChar, 25, "identity_row"));
			adapter.InsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("row_id", System.Data.OleDb.OleDbType.Integer, 4, "row_id"));

			// only update the name, filter, and row_id
			adapter.UpdateCommand.CommandText = @"UPDATE model_parameter SET name = ?, filter = ?, row_id = ?  WHERE (model_id = ?) AND (id = ?)";
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("name", System.Data.OleDb.OleDbType.VarChar, 50, "name"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("filter", System.Data.OleDb.OleDbType.VarChar, 25, "filter"));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("row_id", System.Data.OleDb.OleDbType.Integer, 4, "row_id"));

			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.UpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));

			// delete
			adapter.DeleteCommand.CommandText = @"DELETE FROM model_parameter WHERE (model_id = ?) AND (id = ?)";
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));
			adapter.DeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "id", System.Data.DataRowVersion.Original, null));
		}


		#endregion
	}
}
