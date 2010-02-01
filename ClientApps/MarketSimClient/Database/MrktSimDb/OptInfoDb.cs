using System;

using System.Data;

using System.Data.OleDb;



namespace MrktSimDb

{

	/// <summary>

	/// Interface to the OptInfo tables

	/// </summary>

	public class OptInfoDB

	{

		private OptInfo optDataset = null; // the data set

		private System.Data.OleDb.OleDbConnection connection = null;

		public System.Data.OleDb.OleDbConnection Connection

		{

			set

			{

				connection = value;

				setConnection();

			}

		}



		private int iScenarioID = 0;



		// read only

		private System.Data.OleDb.OleDbDataAdapter optExploreModeAdapter;

		private System.Data.OleDb.OleDbCommand optExploreModeSelectCommand;

		private System.Data.OleDb.OleDbDataAdapter optForAdapter;

		private System.Data.OleDb.OleDbCommand optForSelectCommand;		

		private System.Data.OleDb.OleDbDataAdapter optModeExecAdapter;

		private System.Data.OleDb.OleDbCommand optModeExecSelectCommand;



		// read and write

		private System.Data.OleDb.OleDbDataAdapter optParamAdapter;

		private System.Data.OleDb.OleDbCommand optParamSelectCommand;

		private System.Data.OleDb.OleDbCommand optParamInsertCommand;

		private System.Data.OleDb.OleDbCommand optParamUpdateCommand;

		private System.Data.OleDb.OleDbCommand optParamDeleteCommand;



		private System.Data.OleDb.OleDbDataAdapter optPlanAdapter;

		private System.Data.OleDb.OleDbCommand optPlanSelectCommand;

		private System.Data.OleDb.OleDbCommand optPlanInsertCommand;

		private System.Data.OleDb.OleDbCommand optPlanUpdateCommand;

		private System.Data.OleDb.OleDbCommand optPlanDeleteCommand;



		private OleDbCommand genericCommand; 



		// access the optimization info

		public OptInfo OptData

		{

			get

			{

				return optDataset;

			}

		}



		public OptInfoDB()

		{

			optDataset = new OptInfo();



			initializeCommands();

		}



		public void Refresh()

		{

			if (connection == null)

				return;



			optDataset.Clear();



			optExploreModeAdapter.Fill(optDataset.optimization_explore_mode);

			optForAdapter.Fill(optDataset.optimization_optimize_for);

			optModeExecAdapter.Fill(optDataset.optimization_mode_exec);



			//optParamAdapter.Fill(optDataset.optimization_params);

			optPlanAdapter.Fill(optDataset.optimization_plan);

		}



		public void SetScenarioID(int scenario_id)

		{

			iScenarioID = scenario_id;

		}



		public void SelectParams()

		{

			optParamSelectCommand.CommandText = "SELECT model_id, scenario_id, component_name, parameter, lower, upper,leader,slave FROM optimization_params WHERE scenario_id = " + iScenarioID;

			optParamAdapter.Fill(optDataset.optimization_params);

		}





		/*

		// clear results for optimization to be run

		public void ClearOptResults()

		{

			genericCommand.CommandText = "DELETE FROM optimization_results_std " +

				"WHERE model_id IN (SELECT model_id FROM model_info WHERE model_type = 1)";



			connection.Open();



			try

			{

				genericCommand.ExecuteNonQuery();

			}

			catch(System.Data.OleDb.OleDbException)

			{

			}



			connection.Close();

		}

		*/



		

		private void initializeCommands()

		{

			// commands for dataset

			optExploreModeSelectCommand = ModelInfoDb.newOleDbCommand();

			optForSelectCommand = ModelInfoDb.newOleDbCommand();

			optModeExecSelectCommand = ModelInfoDb.newOleDbCommand();



			optParamSelectCommand = ModelInfoDb.newOleDbCommand();

			optParamInsertCommand = ModelInfoDb.newOleDbCommand();

			optParamUpdateCommand = ModelInfoDb.newOleDbCommand();

			optParamDeleteCommand = ModelInfoDb.newOleDbCommand();



			optPlanSelectCommand = ModelInfoDb.newOleDbCommand();

			optPlanInsertCommand = ModelInfoDb.newOleDbCommand();

			optPlanUpdateCommand = ModelInfoDb.newOleDbCommand();

			optPlanDeleteCommand = ModelInfoDb.newOleDbCommand();

			

			// adapters 

			optExploreModeAdapter =  new System.Data.OleDb.OleDbDataAdapter();

			optForAdapter =  new System.Data.OleDb.OleDbDataAdapter();

			optModeExecAdapter =  new System.Data.OleDb.OleDbDataAdapter();



			optParamAdapter = new System.Data.OleDb.OleDbDataAdapter();

			optPlanAdapter = new System.Data.OleDb.OleDbDataAdapter();



			// a generic command - used for 'non-query'

			genericCommand = ModelInfoDb.newOleDbCommand();

			

			// assign select commands to adapters

			optExploreModeAdapter.SelectCommand = optExploreModeSelectCommand;

			optForAdapter.SelectCommand = optForSelectCommand;		

			optModeExecAdapter.SelectCommand = optModeExecSelectCommand;



			optParamAdapter.SelectCommand = optParamSelectCommand;

			optParamAdapter.InsertCommand = optParamInsertCommand;

			optParamAdapter.UpdateCommand = optParamUpdateCommand;

			optParamAdapter.DeleteCommand = optParamDeleteCommand;



			optPlanAdapter.SelectCommand = optPlanSelectCommand;

			optPlanAdapter.InsertCommand = optPlanInsertCommand;

			optPlanAdapter.UpdateCommand = optPlanUpdateCommand;

			optPlanAdapter.DeleteCommand = optPlanDeleteCommand;



			// assign queries



			// select

			optExploreModeSelectCommand.CommandText = "SELECT explore_mode_id, explore_mode FROM optimization_explore_mode";

			optForSelectCommand.CommandText = "SELECT optimize_for_id, optimize_for FROM optimization_optimize_for";

			optModeExecSelectCommand.CommandText = "SELECT mode_exec_id, mode_exec FROM optimization_mode_exec";

			optPlanSelectCommand.CommandText = "SELECT scenario_id,scenario_name,optimize_for,num_steps,mode_exec_id,explore_mode_id,is_active FROM optimization_plan";



			/*

			//

			// insert

			//

			modelDBInsertCommand.CommandText = "INSERT INTO model_info(model_name, start_date, end_date, model_type) VALUES (?, ?, ?, ?)";

			

			modelDBInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_name", System.Data.OleDb.OleDbType.VarChar, 100, "model_name"));

			modelDBInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));

			modelDBInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));

			modelDBInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_type", System.Data.OleDb.OleDbType.Integer, 4, "model_type"));

		

			optParamInsertCommand.Connection = connection;

			optPlanInsertCommand.Connection = connection;



			// update

			modelDBUpdateCommand.CommandText = "UPDATE model_info SET model_name = ?, start_date = ?, end_date = ?, model_type = ? WHERE (model_id = ?)";

			

			modelDBUpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_name", System.Data.OleDb.OleDbType.VarChar, 100, "model_name"));

			modelDBUpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("start_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "start_date"));

			modelDBUpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("end_date", System.Data.OleDb.OleDbType.DBTimeStamp, 8, "end_date"));

			modelDBUpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("model_type", System.Data.OleDb.OleDbType.Integer, 4, "model_type"));

			modelDBUpdateCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));



			optParamUpdateCommand.Connection = connection;

			optPlanUpdateCommand.Connection = connection;



			// delete

			modelDBDeleteCommand.CommandText = "DELETE FROM model_info WHERE (model_id = ?)";

			modelDBDeleteCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_model_id", System.Data.OleDb.OleDbType.Integer, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "model_id", System.Data.DataRowVersion.Original, null));



			optParamDeleteCommand.Connection = connection;

			optPlanDeleteCommand.Connection		

			*/

		}



		// assign connections to commands

		private void setConnection()

		{

			optForSelectCommand.Connection = connection;

			genericCommand.Connection = connection;

			optParamSelectCommand.Connection = connection;

			optParamInsertCommand.Connection = connection;

			optParamUpdateCommand.Connection = connection;

			optParamDeleteCommand.Connection = connection;



			optPlanSelectCommand.Connection = connection;

			optPlanInsertCommand.Connection = connection;

			optPlanUpdateCommand.Connection = connection;

			optPlanDeleteCommand.Connection = connection;



			optExploreModeSelectCommand.Connection = connection;

			optModeExecSelectCommand.Connection = connection;

		}



	}

}