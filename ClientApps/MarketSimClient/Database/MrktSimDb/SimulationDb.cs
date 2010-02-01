using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{

    /// <summary>
    /// Interface to the MrktSimDBSchema and Project mangement tables
    /// </summary>
    public class SimulationDb : ProjectDb
    {
        #region types

        public enum SimulationType
        {
            Standard = 0,
            Parallel = 32,
            Serial = 64,
            Random = 80,
            Optimize = 96,
            Calibration = 100,
            Statistical = 128,
            CheckPoint = 150
        }

        public enum VariableType
        {
            Stepped = 0,
            RandomUniform,
            RandomCentered
        }

        public static DataTable simulation_type = new DataTable( "SimulationType" );
        private static void create_simulation_types() {
            simulation_type.Columns.Add( "type", typeof( string ) );
            DataColumn idCol = simulation_type.Columns.Add( "id", typeof( SimulationType ) );
            simulation_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = simulation_type.NewRow();
            row[ "type" ] = "standard";
            row[ "id" ] = SimulationType.Standard;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Parallel Search";
            row[ "id" ] = SimulationType.Parallel;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Serial Search";
            row[ "id" ] = SimulationType.Serial;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Random Search";
            row[ "id" ] = SimulationType.Random;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Optimization";
            row[ "id" ] = SimulationType.Optimize;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Calibration";
            row[ "id" ] = SimulationType.Calibration;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Statistical";
            row[ "id" ] = SimulationType.Statistical;
            simulation_type.Rows.Add( row );


            row = simulation_type.NewRow();
            row[ "type" ] = "Checkpoint";
            row[ "id" ] = SimulationType.CheckPoint;
            simulation_type.Rows.Add( row );

            simulation_type.AcceptChanges();
        }

        public static DataTable variable_type = new DataTable("VariableType");
        private static void create_variable_types()
        {
            variable_type.Columns.Add("type", typeof(string));
            DataColumn idCol = variable_type.Columns.Add("id", typeof(int));
            variable_type.PrimaryKey = new DataColumn[] { idCol }; ;

            DataRow row = null;

            row = variable_type.NewRow();
            row["type"] = "Stepped";
            row["id"] = VariableType.Stepped;
            variable_type.Rows.Add(row);

            row = variable_type.NewRow();
            row["type"] = "Random (Uniform)";
            row["id"] = VariableType.RandomUniform;
            variable_type.Rows.Add(row);

            row = variable_type.NewRow();
            row["type"] = "Random (Centered)";
            row["id"] = VariableType.RandomCentered;
            variable_type.Rows.Add(row);

            variable_type.AcceptChanges();
        }


        static SimulationDb()
        {
            create_simulation_types();
            create_variable_types();
        }

        #endregion

        private int simID = Database.AllID;
        public int Id
        {
            get
            {
                return simID;
            }

            set
            {
                simID = value;
                InitializeSelectQuery();
            }
        }

        public MrktSimDBSchema.simulationRow Simulation {
            get {
                    return Data.simulation.FindByid(simID);
            }
        }
        
        public static int NumSims(MrktSimDBSchema.simulationRow simulation)
        {
            if (simulation == null)
                return 0;

            // one lowly sim
            if (simulation.type == (byte)SimulationType.Standard)
                return 1;

            if (simulation.type == (byte)SimulationType.CheckPoint)
                return 1;

            // max number of iterations
            if (simulation.type == (byte)SimulationType.Calibration)
            {
                return 10;
            }

            // this will be added to later
            if (simulation.type == (byte)SimulationType.Statistical)
                return simulation.Getscenario_simseedRows().Length;

            if (simulation.Getscenario_variableRows().Length == 0)
                return 0;

            // max number of iterations
            if (simulation.type == (byte)SimulationType.Optimize)
            {
                if (simulation.Getscenario_variableRows().Length == 0)
                    return 0;
                else
                {
                    int num = (int)Math.Pow(3, simulation.Getscenario_variableRows().Length);
                    return 3 + num;
                }
            }


            // the numbers multiplies as the number of sims for each variable
            int numSims = 1;

            // unless we are doing a random search then the number is additive
            if (simulation.type == (byte)SimulationType.Random)
                numSims = 0;

            foreach (MrktSimDBSchema.scenario_variableRow variable in simulation.Getscenario_variableRows())
            {
                if (simulation.type == (byte)SimulationType.Random)
                {
                    // we add up the number specified
                    numSims += variable.num_steps;
                }
                else
                {
                    int varNum = variable.num_steps;

                    if (variable.type == 0)
                    {
                        if (variable.min >= variable.max)
                            varNum += 1;
                        else
                            varNum += 2;
                    }

                    numSims *= varNum;
                }

            }


            if (simulation.type == (byte)SimulationType.Serial)
            {
                numSims *= simulation.Getscenario_parameterRows().Length;
            }

            return numSims;
        }

        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.model_parameter ||
                table == Data.scenario_parameter ||
                table == Data.scenario_variable ||
                table == Data.scenario_simseed ||
                table == Data.scenario_metric ||
                table == Data.sim_queue ||
                table == Data.sim_variable_value )
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();

            OleDbDataAdapter adapter = null;

            // model data

            if (Id >= 0)
            {
                // get only data for this simulation
                adapter = getAdapter(Data.project);
                adapter.SelectCommand.CommandText += " WHERE id IN " +
                    "(SELECT project_id FROM Model_info WHERE model_id IN " +
                    "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                    "(SELECT scenario_id FROM simulation WHERE id = " + Id + ")))";

                adapter = getAdapter(Data.Model_info);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                    "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

                adapter = getAdapter(Data.scenario);
                adapter.SelectCommand.CommandText += " WHERE scenario_id IN " +
                    "(SELECT scenario_id FROM simulation WHERE id = " + Id + ")";

                adapter = getAdapter(Data.model_parameter);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM scenario WHERE scenario_id IN " +
                    "(SELECT scenario_id FROM simulation WHERE id = " + Id + "))";

                adapter = getAdapter(Data.simulation);
                adapter.SelectCommand.CommandText += " WHERE id = " + Id;

                adapter = getAdapter(Data.sim_queue);
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + Id;

                // items dependent on simulation
                adapter = getAdapter(Data.scenario_variable);
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + Id;

                adapter = getAdapter(Data.scenario_parameter);
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + Id;

                adapter = getAdapter(Data.scenario_simseed);
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + Id;


                adapter = getAdapter(Data.scenario_metric);
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + Id;

                adapter = getAdapter(Data.sim_variable_value);
                adapter.SelectCommand.CommandText += " WHERE run_id IN (SELECT run_id FROM sim_queue WHERE sim_id = " + Id + ")";

            }
        }

        private double evaluateModelParameter(MrktSimDBSchema.model_parameterRow modelParm)
        {
            string col = modelParm.col_name;
            string table = modelParm.table_name;
            string filter = modelParm.filter;

            this.genericCommand.CommandText = "SELECT " + col + " FROM " + table + " WHERE " + filter;

            double num;

            genericCommand.Connection.Open();

            try
            {
                num = Convert.ToDouble(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                num = Double.NaN;
            }

            genericCommand.Connection.Close();

            return num;
        }

        private bool applyScenarioParameter(MrktSimDBSchema.scenario_parameterRow scenarioParm)
        {
            MrktSimDBSchema.model_parameterRow modelParm = scenarioParm.model_parameterRow;

            string col = modelParm.col_name;
            string table = modelParm.table_name;
            string filter = modelParm.filter;

            double val = scenarioParm.aValue;

            this.genericCommand.CommandText = "UPDATE " + table + " SET " + col + " = " 
                +  val.ToString() + " WHERE " + filter;

            bool rval = true;

            genericCommand.Connection.Open();

            try
            {
                int num = genericCommand.ExecuteNonQuery();

                if (num == 0)
                    rval = false;
            }
            catch (System.Data.OleDb.OleDbException)
            {
                rval = false;
            }

            genericCommand.Connection.Close();

            return rval;
        }

        public MrktSimDBSchema.scenario_parameterRow GetSimulationParameter(int simId, int paramID)
        {

              // now check if parameter already exists
            string query = "sim_id = " + simID + " AND param_id = " + paramID;

            DataRow[] parmRows = this.Data.scenario_parameter.Select(query, "", DataViewRowState.CurrentRows);

            if (parmRows.Length > 0)
                return (MrktSimDBSchema.scenario_parameterRow)parmRows[0];

            return null;
        }

        public MrktSimDBSchema.scenario_parameterRow CreateScenarioParameter(int simID, int paramID)
        {
          
            MrktSimDBSchema.scenario_parameterRow simParm = GetSimulationParameter(simID,paramID);

            if( simParm != null )
                return simParm;

            MrktSimDBSchema.model_parameterRow modelParm = Data.model_parameter.FindByid(paramID);


            double val = evaluateModelParameter(modelParm);


            MrktSimDBSchema.scenario_parameterRow parameter = Data.scenario_parameter.Newscenario_parameterRow();

            parameter.model_id = modelParm.Model_infoRow.model_id;
            parameter.sim_id = simID;
            parameter.param_id = paramID;
            parameter.aValue = val;
            parameter.expression = "";

            Data.scenario_parameter.Addscenario_parameterRow(parameter);

            return parameter;
        }

        public MrktSimDBSchema.scenario_variableRow CreateScenarioVariable(MrktSimDBSchema.simulationRow simulation, string token)
        {
            string filter = "sim_id = " + simulation.id;

            // validate token
            token = token.Replace(" ", "_");
            token = token.Replace("'", "");
            token = token.Replace("*", "_");
            token = token.Replace("+", "_");
            token = token.Replace("^", "_");
            token = token.Replace("(", "_");
            token = token.Replace(")", "_");
            token = token.Replace("=", "_");
            token = token.Replace("/", "_");
            token = token.Replace(",", "_");
            token = token.Replace(".", "_");
            token = token.Replace(";", "_");
            token = token.Replace("-", "_");

            char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            token = token.TrimStart(numbers);

            if (token.Length == 0)
                return null;

            // create with defaults let caller set as desired
            // create a new brand
            MrktSimDBSchema.scenario_variableRow row = Data.scenario_variable.Newscenario_variableRow();

            row.sim_id = simulation.id;

            row.token = ModelDb.CreateUniqueName(Data.scenario_variable, "token", token, filter);
            row.min = 0;
            row.max = 1;
            row.num_steps = 0;

            row.descr = "";

            row.type = 0;

            row.product_id = ModelDb.AllID;

            Data.scenario_variable.Addscenario_variableRow(row);

            return row;
        }

        public MrktSimDBSchema.scenario_metricRow CreateScenarioMetric(MrktSimDBSchema.simulationRow simulation, string token)
        {
            MrktSimDBSchema.scenario_metricRow metric = findMetric(simulation, token);

            if (metric != null)
                return metric;

            // create metric
            metric = Data.scenario_metric.Newscenario_metricRow();

            metric.sim_id = simulation.id;
            metric.token = token;

            Data.scenario_metric.Addscenario_metricRow(metric);

            return metric;
        }

        private MrktSimDBSchema.scenario_metricRow findMetric(MrktSimDBSchema.simulationRow simulation, string token)
        {
            // check if metric already is listed
            foreach (MrktSimDBSchema.scenario_metricRow metric in simulation.Getscenario_metricRows())
            {
                if (metric.token == token)
                    return metric;
            }

            return null;
        }

        public void DeleteScenarioMetric(MrktSimDBSchema.simulationRow simulation, string token)
        {
            MrktSimDBSchema.scenario_metricRow metric = findMetric(simulation, token);

            if (metric != null)
                metric.Delete();
        }

        public MrktSimDBSchema.scenario_simseedRow CreateScenarioSimSeed(MrktSimDBSchema.simulationRow simulation, int val)
        {
            string filter = "sim_id = " + simulation.id;

            // create with defaults let caller set as desired
            // create a new brand
            MrktSimDBSchema.scenario_simseedRow row = this.Data.scenario_simseed.Newscenario_simseedRow();

            row.sim_id = simulation.id;

            row.seed = val;

            Data.scenario_simseed.Addscenario_simseedRow(row);

            return row;
        }



        #region Open\Close

        public bool SimulationRunning() {
            return this.SimulationRunning( this.Id );
        }

    

        private bool dbLocked() {

            genericCommand.CommandText = "SELECT COUNT(*) FROM simulation WHERE locked = 0 AND id = " + this.Id;

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return true;

            return false;
        }

    

        public bool Open() {
            // checks model access
            if( this.Connection == null )
                return false;

            if( Id == AllID )
                return false;
            
            string who = null;

            if(  SimulationLocked(Id, out who)) {
                ReadOnly = true;
            }
            else {
                ReadOnly = false;
                LockSimulation( Id, true );
            }

            return true;
        }

        public void Close() {
            // no need to close if read only
            if( ReadOnly )
                return;

            LockSimulation( Id, false );
        }

        #endregion


        #region queries

        public string GetRunStatus(int runID)
        {
            string rval = "";

            if (Connection == null)
                return rval;

            // compute num simulations in queue
            genericCommand.CommandText = "SELECT current_status FROM sim_queue WITH (NOLOCK) WHERE run_id = " + runID;

            Connection.Open();
            try
            {
                rval = Convert.ToString(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                rval = "error";
            }

            Connection.Close();

            return rval;
        }
     

        #endregion

        #region misc queries

        // this is needed when looking up parameter names in simulation editor (since market_plan is not in the schema)
        // added the extra 
        public  bool GetMarketPlanInfo(int scenario_id, int marketPlanID, out ModelDb.PlanType planType ) {

            bool rval = false;

            planType = PlanType.MarketPlan;

            genericCommand.CommandText = "SELECT type from scenario_market_plans WHERE id = " + marketPlanID +
                " AND scenario_id = " + scenario_id;

            Connection.Open();

            try {
                object obj = genericCommand.ExecuteScalar();

                if( obj != null ) {
                    planType = (ModelDb.PlanType)Convert.ToInt32( obj );

                    rval = true;
                }
            }
            catch( Exception) {
            }

            Connection.Close();

            return rval;
        }

        // this is needed when looking up parameter products and dates in the simulation editor (since market_plan is not in the schema)
        public bool GetMarketPlanInfo( string planFilter, out string product, out DateTime startDate, out DateTime endDate ) {
            product = "";
            int prodId = -2;

            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;

            bool rval = false;

            genericCommand.CommandText = "SELECT product_id, start_date, end_date from market_plan WHERE " + planFilter;

            Connection.Open();

            try {
                OleDbDataReader reader = genericCommand.ExecuteReader();

                if( reader != null && reader.HasRows ) {
                    reader.Read();
                    object[] row = new object[ 3 ];
                    reader.GetValues( row );

                    prodId = (int)row[ 0 ];
                    startDate = (DateTime)row[ 1 ];
                    endDate = (DateTime)row[ 2 ];

                }
            }
            catch( Exception ) {
            }

            Connection.Close();

            if( prodId != -2 ) {
                genericCommand.CommandText = "SELECT product_name from product WHERE product_id=" + prodId;

                Connection.Open();

                try {
                    object obj = genericCommand.ExecuteScalar();

                    if( obj != null ) {
                        product = (string)obj;

                        rval = true;
                    }
                }
                catch( Exception ) {
                }
            }

            Connection.Close();

            return rval;
        }

        public bool SimulationHasResults( int sim_id ) {
            genericCommand.CommandText = "SELECT COUNT(*) from results_std WHERE run_id in " +
                "(SELECT run_id FROM sim_queue WHERE sim_id = " + sim_id + ")";

            Connection.Open();
            int num = Convert.ToInt32( genericCommand.ExecuteScalar() );
            Connection.Close();

            if( num == 0 )
                return false;

            return true;
        }

        public int SimulationsToRun()
        {
            genericCommand.CommandText = "SELECT COUNT(*) from simulation " +
                " WHERE sim_num > 0";

            int num = 0;

            try
            {
                Connection.Open();
                num = Convert.ToInt32(genericCommand.ExecuteScalar());
                Connection.Close();
            }
            catch (Exception)
            {
                return 0;
            }

            return num;
        }

        public int RunningSimulation()
        {
            genericCommand.CommandText = "SELECT run_id from sim_queue WHERE status = 1";

            Connection.Open();

            object obj = genericCommand.ExecuteScalar();

            Connection.Close();

            if (obj == null)
                return ModelDb.AllID;

            return Convert.ToInt32(obj);
        }

      


        public void DeleteRun(MrktSimDBSchema.sim_queueRow simRow)
        {
            simRow.Delete();
        }

        public int GetParameterProduct(int parmID)
        {
            int rval = -1;

            if (Connection == null)
                return rval;

            // query fun
            // we need to get table and identityRow from parameter

            string theTable;
            genericCommand.CommandText = "SELECT table_name FROM model_parameter WHERE id = " + parmID;

            Connection.Open();

            try
            {
                theTable = Convert.ToString(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return rval;
            }

            Connection.Close();

            string theFilter;
            genericCommand.CommandText = "SELECT filter FROM model_parameter WHERE id = " + parmID;

            Connection.Open();

            try
            {
                theFilter = Convert.ToString(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return rval;
            }

            Connection.Close();

            // finally try to get product
            genericCommand.CommandText = "SELECT product_id FROM " + theTable + " WHERE " + theFilter;

            Connection.Open();

            try
            {
                rval = Convert.ToInt32(genericCommand.ExecuteScalar());
            }
            catch (System.Data.OleDb.OleDbException)
            {
                Connection.Close();
                return rval;
            }

            Connection.Close();

            return rval;
        }


        

    
        #endregion

  
    }
}
