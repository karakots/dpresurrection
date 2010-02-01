using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public class ResultsDb : PCSModel
    {

        private int scenarioID = Database.AllID;

        public int ScenarioID
        {
            set
            {
                scenarioID = value;
                InitializeSelectQuery();
            }

            get
            {
                return scenarioID;
            }
        }

        private int simID = Database.AllID;

        public int SimID
        {
            set
            {
                simID = value;
                InitializeSelectQuery();
            }

            get
            {
                return simID;
            }
        }


        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.scenario ||
                table == Data.simulation ||
                table == Data.sim_queue ||
                table == Data.scenario_variable ||
                table == Data.sim_variable_value)
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {

            base.InitializeSelectQuery();

            // add addtional filters for my tables
            OleDbDataAdapter adapter = null;

            adapter = getAdapter(Data.scenario);

            if (ScenarioID == Database.AllID && SimID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;
            }
            else if (SimID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE scenario_id = " + ScenarioID;
            }
            else
            {
                adapter.SelectCommand.CommandText += " WHERE scenario_id IN (SELECT scenario_id FROM simulation WHERE id = " + SimID + ")";
            }

            adapter = getAdapter(Data.simulation);

            if (ScenarioID == Database.AllID && simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE scenario_id IN (SELECT scenario_id FROM scenario WHERE model_id = " + ModelID + ")";
            }
            else if (simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE scenario_id = " + ScenarioID;
            }
            else
            {
                adapter.SelectCommand.CommandText += " WHERE id = " + SimID;
            }

            adapter = getAdapter(Data.sim_queue);

            if (ScenarioID == Database.AllID && simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;
            }
            else if (simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE sim_id IN (SELECT id FROM simulation WHERE scenario_id = " + ScenarioID + ")";
            }
            else
            {
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + SimID;
            }

            adapter = getAdapter(Data.scenario_variable);

            if (ScenarioID == Database.AllID && simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE sim_id IN (SELECT id FROM simulation WHERE scenario_id IN (SELECT scenario_id FROM scenario WHERE model_id = " + ModelID + "))";
            }
            else if (simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE sim_id IN (SELECT id FROM simulation WHERE scenario_id = " + ScenarioID + ")";
            }
            else
            {
                adapter.SelectCommand.CommandText += " WHERE sim_id = " + SimID;
            }

            adapter = getAdapter(Data.sim_variable_value);

            if (ScenarioID == Database.AllID && simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE run_id IN (SELECT run_id FROM sim_queue WHERE model_id = " + ModelID + ")";
            }
            else if (simID == Database.AllID)
            {
                adapter.SelectCommand.CommandText += " WHERE run_id IN (SELECT run_id FROM sim_queue WHERE sim_id IN (SELECT id FROM simulation WHERE scenario_id = " + ScenarioID + "))";
            }
            else
            {
                adapter.SelectCommand.CommandText += " WHERE run_id IN (SELECT run_id FROM sim_queue WHERE sim_id = " + SimID +")";
            }
        }
    }                                                     
}
