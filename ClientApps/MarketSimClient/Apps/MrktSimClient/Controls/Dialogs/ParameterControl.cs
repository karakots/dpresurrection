using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using MrktSimDb;
using MarketSimUtilities;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class ParameterControl : UserControl
    {
        #region fields
        DataTable availableParms;
        DataTable usedParms;
        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private SimulationDb theDb;

        #endregion

        public new void SuspendLayout()
        {
            base.SuspendLayout();

            modelParmGrid.Suspend = true;
            scenarioParmGrid.Suspend = true;
        }

        new public void ResumeLayout()
        {
            base.ResumeLayout();

            modelParmGrid.Suspend = false;
            scenarioParmGrid.Suspend = false;
        }

        public ParameterControl()
        {
            InitializeComponent();

            availableParms = new DataTable();
            availableParms.TableName = "AvailableParms";
            availableParms.Columns.Add( "name", typeof( string ) );
            availableParms.Columns.Add( "table", typeof( string ) );
            availableParms.Columns.Add( "type", typeof( string ) );
            availableParms.Columns.Add( "product", typeof( string ) );
            availableParms.Columns.Add( "start_date", typeof( DateTime ) );
            availableParms.Columns.Add( "end_date", typeof( DateTime ) );
            availableParms.Columns.Add( "id", typeof( int ) );

            usedParms = new DataTable();
            usedParms.TableName = "UsedParms";
            usedParms.Columns.Add( "name", typeof( string ) );
            usedParms.Columns.Add( "value", typeof( double ) );
            usedParms.Columns.Add( "expression", typeof( string ) );
            usedParms.Columns.Add( "table", typeof( string ) );
            usedParms.Columns.Add( "type", typeof( string ) );
            usedParms.Columns.Add( "product", typeof( string ) );
            usedParms.Columns.Add( "start_date", typeof( DateTime ) );
            usedParms.Columns.Add( "end_date", typeof( DateTime ) );
            usedParms.Columns.Add( "id", typeof( int ) );

            modelParmGrid.Table = availableParms;
            modelParmGrid.DescriptionWindow = false;
            modelParmGrid.AllowDelete = false;

            scenarioParmGrid.Table = usedParms;
            scenarioParmGrid.DescriptionWindow = false;
            scenarioParmGrid.AllowDelete = false;

           
        }

        public SimulationDb Db
        {
            set
            {
                theDb = value;

                currentSimulation = value.Data.simulation.FindByid(value.Id);

                CreateTableStyle();

                if (currentSimulation != null)
                {
                    updateParms();

                    ////scenarioParmGrid.Table = value.Data.scenario_parameter;

                    ////scenarioParmGrid.DescriptionWindow = false;
                    ////scenarioParmGrid.AllowDelete = false;

                    ////scenarioParmGrid.RowFilter = "sim_id = " + currentSimulation.id;

                  
                }


            }
        }

        /// <summary>
        /// A better name for the type and col
        /// </summary>
        /// <param name="modelParamId"></param>
        /// <param name="type"></param>
        /// <param name="colVal"></param>
        /// <returns> false if unable to retrieve info</returns>
        private bool GetParmVals( int modelParamId, out string type, out string colVal ) {

            MrktSimDBSchema.model_parameterRow parm = theDb.Data.model_parameter.FindByid( modelParamId );

            type = parm.table_name;
            colVal = parm.col_name;

            if( type == "market_plan" ) {
                int marketPlanID = parm.row_id;
                ModelDb.PlanType planType;

                if( theDb.GetMarketPlanInfo( currentSimulation.scenario_id, marketPlanID, out  planType ) ) {
                    type = planType.ToString();

                    colVal = Common.ModelParameter.LookupParmName( colVal, planType );
                }
                else {
                    return false;
                }

            }
            return true;
        }

        private void updateParms()
        {
            modelParmGrid.Suspend = true;
            scenarioParmGrid.Suspend = true;

            availableParms.Clear();
            usedParms.Clear();

            foreach (MrktSimDBSchema.model_parameterRow parm in theDb.Data.model_parameter.Select())
            {
                string name = parm.name;
                string type = parm.table_name;
                string val = parm.col_name;
                int id = parm.id;
                MrktSimDBSchema.scenario_parameterRow[] scenarioParams;
                double paramValue = 0;
                string paramExpression = "";

                bool skipParm = false;

                // first check if scenario has parm
                string scenarioQuery = "sim_id = " + this.currentSimulation.id + " AND " + "param_id = " + parm.id;

                bool scenarioHasParm = false;
                scenarioParams = (MrktSimDBSchema.scenario_parameterRow[])theDb.Data.scenario_parameter.Select( scenarioQuery, "", DataViewRowState.CurrentRows );
                if( scenarioParams.Length > 0 ) {       // should always be 0 or 1
                    scenarioHasParm = true;
                    paramValue = scenarioParams[ 0 ].aValue;
                    paramExpression = scenarioParams[ 0 ].expression;
                }

                if (!GetParmVals(parm.id, out type, out val)) {
                    skipParm = true;
                }

                if( skipParm ) {
                    continue;
                }

                string product = "";
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                if( parm.filter != null && parm.filter != "" ) {
                    theDb.GetMarketPlanInfo( parm.filter, out product, out startDate, out endDate );
                }

                object startObj = null;
                object endObj = null;

                if( startDate != DateTime.MinValue )
                    startObj = startDate;

                if( endDate != DateTime.MinValue )
                    endObj = endDate;

                if( scenarioHasParm == false ) {
                    Object[] vals = { name, type, val, product, startObj, endObj, id };
                    availableParms.Rows.Add( vals );
                }
                else {
                    Object[] vals = { name, paramValue, paramExpression, type, val, product, startObj, endObj, id };
                    usedParms.Rows.Add( vals );
                }
            }

            availableParms.AcceptChanges();
            usedParms.AcceptChanges();

            modelParmGrid.Suspend = false;
            scenarioParmGrid.Suspend = false;
        }

        private void addparmButton_Click(object sender, System.EventArgs e)
        {
            ArrayList rows = this.modelParmGrid.GetSelected();

            foreach (DataRow row in rows)
            {
                int parm_id = (int)row["id"];
                theDb.CreateScenarioParameter(currentSimulation.id, parm_id);
            }

            updateParms();
        }

        private void removeParmButton_Click(object sender, System.EventArgs e)
        {

            ArrayList rows = this.scenarioParmGrid.GetSelected();

            foreach (DataRow row in rows)
            {
                int paramID = (int)row[ "id" ];
                string deQuery = "param_id = " + paramID.ToString();

                DataRow[] delRows = theDb.Data.scenario_parameter.Select( deQuery );
                foreach( DataRow delRow in delRows ) {
                    delRow.Delete();
                }
            }

            updateParms();
        }

        private void resetParamValuesButton_Click(object sender, System.EventArgs e)
        {
            string query = "sim_id = " + this.currentSimulation.id;

            DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query, "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.scenario_parameterRow parmRow in parmRows)
            {
                MrktSimDBSchema.model_parameterRow modelParm = parmRow.model_parameterRow;

                double val = theDb.EvaluateModelParameter(modelParm);

                parmRow.aValue = val;
            }

            updateParms();
        }

        public void CreateTableStyle()
        {
            modelParmGrid.Clear();
            modelParmGrid.AddTextColumn( "name", true );
            modelParmGrid.AddTextColumn( "type", true );
            modelParmGrid.AddTextColumn( "table", "applies to", true );
            modelParmGrid.AddTextColumn( "product", true );
            modelParmGrid.AddDateColumn( "start_date", "start date", true );
            modelParmGrid.AddDateColumn( "end_date", "end date", true );
            modelParmGrid.Reset();

            scenarioParmGrid.Clear();
            scenarioParmGrid.AddTextColumn( "name", true );
            scenarioParmGrid.AddNumericColumn( "value" );
            if( currentSimulation != null &&
                (currentSimulation.type == (byte)SimulationDb.SimulationType.Calibration ||
                currentSimulation.type == (byte)SimulationDb.SimulationType.Optimize ||
                currentSimulation.type == (byte)SimulationDb.SimulationType.Parallel ||
                currentSimulation.type == (byte)SimulationDb.SimulationType.Serial ||
                currentSimulation.type == (byte)SimulationDb.SimulationType.Random) ) {
                scenarioParmGrid.AddTextColumn( "expression" );
            }
            scenarioParmGrid.AddTextColumn( "type", true );
            scenarioParmGrid.AddTextColumn( "table", "applies to", true );
            scenarioParmGrid.AddTextColumn( "product", true );
            scenarioParmGrid.AddDateColumn( "start_date", "start date", true );
            scenarioParmGrid.AddDateColumn( "end_date", "end date", true );
            scenarioParmGrid.Reset();

            ////scenarioParmGrid.Clear();
            ////scenarioParmGrid.AddComboBoxColumn( "param_id", theDb.Data.model_parameter, "name", "id", true );
            ////scenarioParmGrid.AddNumericColumn( "aValue" );


            ////scenarioParmGrid.Reset();
        }

        private void applyParmsButton_Click(object sender, EventArgs e)
        {
            DialogResult rslt = MessageBox.Show("This will apply the selected simulation parameter values to the model. Proceed?",
                "Apply Parameter Values?", MessageBoxButtons.OKCancel);

            if (rslt == DialogResult.OK)
            {
                ArrayList rows = this.scenarioParmGrid.GetSelected();

                foreach( DataRow row in rows ) {
                    int paramID = (int)row[ "id" ];
                    string applyQuery = "param_id = " + paramID.ToString();

                    MrktSimDBSchema.scenario_parameterRow[] applylRows = (MrktSimDBSchema.scenario_parameterRow[])
                        theDb.Data.scenario_parameter.Select( applyQuery );
                    foreach( MrktSimDBSchema.scenario_parameterRow applylRow in applylRows ) {
                        theDb.ApplySimulationlParameter( applylRow );
                    }
                }
            }
        }

        private void UpdateRealTables() {
            // transfer the values (for aValue and expression) from the display table back to the actual table
            for( int i = 0; i < scenarioParmGrid.Table.Rows.Count; i++ ) {
                DataRow srow = scenarioParmGrid.Table.Rows[ i ];
                if( srow.RowState == DataRowState.Modified ) {

                    string modRowQuery = String.Format( "param_id = {0}", srow[ "id" ] );
                    MrktSimDBSchema.scenario_parameterRow[] modifyRow = (MrktSimDBSchema.scenario_parameterRow[])
                        theDb.Data.scenario_parameter.Select( modRowQuery );

                    if( modifyRow.Length == 1 ) {    // should always be the case
                        modifyRow[ 0 ].aValue = (double)srow[ "value" ];
                        modifyRow[ 0 ].expression = (string)srow[ "expression" ];
                    }
                    else {
                        Console.WriteLine( "Error! Unable to find matching row to modify in scenarioParmGrid_Validated()! " );
                    }
                }
            }
        }

        private void scenarioParmGrid_Validated( object sender, EventArgs e ) {
            UpdateRealTables();
        }
    }
}
