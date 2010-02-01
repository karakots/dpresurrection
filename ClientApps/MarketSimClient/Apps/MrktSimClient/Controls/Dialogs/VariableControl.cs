using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MarketSimUtilities;
using EquationParser;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class VariableControl : UserControl
    {
        public VariableControl()
        {
            InitializeComponent();
        }

        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private SimulationDb theDb;

        public SimulationDb Db
        {
            set
            {
                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(theDb.Id);

                if (currentSimulation != null)
                {

                    variableGrid.Table = theDb.Data.scenario_variable;

                    variableGrid.RowFilter = "sim_id = " + theDb.Id;
                    variableGrid.DescriptionWindow = false;

                    createTableStyle();

                    // if this sim has results do not allow editing
                    // variables tab

                    if (currentSimulation.sim_num >= 0)
                    {
                        this.variableGrid.EnabledGrid = false;
                        this.createVariableButton.Enabled = false;
                    }
                }
            }
        }

        private void createTableStyle()
        {
            variableGrid.Clear();
            variableGrid.AddTextColumn("token");
            variableGrid.AddComboBoxColumn( "product_id", theDb.Data.product, "product_name", "product_id", true );
            variableGrid.AddNumericColumn("min");
            variableGrid.AddNumericColumn("max");
            variableGrid.AddNumericColumn("num_steps");
            variableGrid.AddComboBoxColumn("type", SimulationDb.variable_type, "type", "type_id", false);
            variableGrid.AddTextColumn("descr");
            variableGrid.Reset();
        }

        private void createVariableButton_Click(object sender, EventArgs e)
        {
            InputString dlg = new InputString();

            dlg.Text = "Enter Mnemonic for Variable";
            dlg.InputText = "";

            DialogResult rslt = dlg.ShowDialog();

            if( rslt != DialogResult.OK )
            {
                return;
            }

            string token = dlg.InputText;

            if (token == "")
                return;

            theDb.CreateScenarioVariable(this.currentSimulation, token);
        }



        public string ParserTest()
        {
            // check if this actually uses the paser
            if (currentSimulation == null)
                return null;

            // one lowly sim
            if (currentSimulation.type == (byte)SimulationDb.SimulationType.Standard)
                return null;

            if (currentSimulation.type == (byte)SimulationDb.SimulationType.CheckPoint)
                return null;

            if (currentSimulation.type == (byte)SimulationDb.SimulationType.Statistical)
                return null;

            if (currentSimulation.Getscenario_variableRows().Length == 0)
                return "No variables defined";

            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();


            // values and stepsize
            double[] val = new double[variables.Length];
            int[] curEval = new int[variables.Length];
            string[] tokens = new string[variables.Length + 1];

            for (int ii = 0; ii < variables.Length; ii++)
            {
                // set each variable to the beginning value
                val[ii] = variables[ii].min;

                tokens[ii] = variables[ii].token;
            }

            tokens[variables.Length] = "CurVal";

            SimpleParser parser = new SimpleParser(tokens);

            // update scenario parameter values
            parser.updateValues(val);

            bool updated = false;

            foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
            {
                parser.updateValue("CurVal", param.aValue);

                if (param.expression != null && param.expression.Length > 0)
                {
                    updated = true;
                    try
                    {
                        double aval = parser.ParseEquation(param.expression);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }

            if (!updated)
                return "Parameters do not depend on Variables";

            return null;
        }

    }
}
