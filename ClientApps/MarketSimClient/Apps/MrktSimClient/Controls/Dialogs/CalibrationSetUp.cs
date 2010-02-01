using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using MrktSimDb;
using MrktSimDb.Metrics;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class CalibrationSetUp : UserControl
    {
        string newVariableString = "New variables need to be created for calibration run";
        CallibrationDb theDb;
        MrktSimDBSchema.simulationRow currentSimulation;

        public CallibrationDb Db
        {
            set
            {
                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                if (currentSimulation == null)
                    return;

                CalibrationControl cal = new CalibrationControl(currentSimulation.control_string);

                decimal step = (decimal)cal.StepSize;

                if (step < stepSize.Minimum)
                    stepSize.Minimum = step;
                else if (step > stepSize.Maximum)
                    stepSize.Maximum = step;

                stepSize.Value = step;

                decimal calTolerance = (decimal)cal.Tolerance;

                if (calTolerance < tolerance.Minimum)
                    tolerance.Minimum = calTolerance;
                else if (calTolerance > stepSize.Maximum)
                    tolerance.Maximum = calTolerance;

                tolerance.Value = calTolerance;

                int iters = cal.MaxIters;

                if (iters < maxiters.Minimum)
                    maxiters.Minimum = iters;
                else if (iters > stepSize.Maximum)
                    maxiters.Maximum = iters;

                maxiters.Value = iters;

                this.checkBox1.Checked = cal.ClearAll;

                this.calibrationType.SelectedItem = Enum.GetName(typeof(CalibrationControl.CalibrationType), cal.Type);
                this.metricType.SelectedItem = cal.Metric;

                applyParms.Checked = cal.ApplyParameters;

                foreach( CalibrationControl.CalibrationSubType subType in Enum.GetValues( typeof( CalibrationControl.CalibrationSubType ) ) ) {
                    if( cal.Calibrate( subType ) ) {
                        calSubType.SetItemChecked( (int)subType, true );
                    }
                }

                initCalibrationInfo();
              
            }
        }

        public CalibrationSetUp()
        {
            InitializeComponent();

            this.calibrationType.DataSource = Enum.GetNames(typeof(CalibrationControl.CalibrationType));

            metricType.DisplayMember = "Descr";

            foreach( string subType in Enum.GetNames( typeof( CalibrationControl.CalibrationSubType ) ) ) {
                calSubType.Items.Add( subType );
            }
        }

        public bool CheckExpressions
        {
            get
            {
                if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.Optimization.ToString())
                {
                    return true;
                }

                return false;
            }
        }

        private void calibrationType_SelectedIndexChanged(object sender, EventArgs e)
        {

           

            metricType.Items.Clear();
            createVariables.Visible = false;

            if( calibrationType.SelectedItem == null ) {
                this.stepSize.Enabled = false;
                this.tolerance.Enabled = false;
                this.metricType.Enabled = false;
                this.maxiters.Enabled = false;
                this.checkBox1.Enabled = false;

                this.calSubType.Enabled = false;
            }
            else if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.Standard.ToString())
            {
                this.stepSize.Enabled = false;
                this.tolerance.Enabled = false;
                this.metricType.Enabled = false;
                this.maxiters.Enabled = false;
                this.checkBox1.Enabled = false;

                this.calSubType.Enabled = false;

            }
            else if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.AutoMatic.ToString())
            {
                metricType.Items.Add(MetricMan.MetricValues[2]);
                metricType.Items.Add(MetricMan.MetricValues[7]);
                metricType.SelectedIndex = 0;

                this.stepSize.Enabled = true;
                this.tolerance.Enabled = true;
                this.metricType.Enabled = true; 
                this.maxiters.Enabled = true;
                this.checkBox1.Enabled = true;
                createVariables.Visible = true;
                this.createVariables.Text = "Create Parameters";

                this.calSubType.Enabled = true;
            }
            else if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.Parametric.ToString())
            {
                metricType.Items.Add(MetricMan.MetricValues[0]);
                metricType.Items.Add(MetricMan.MetricValues[1]);
                metricType.SelectedIndex = 0;

                this.stepSize.Enabled = true;
                this.tolerance.Enabled = true;
                this.metricType.Enabled = true;
                this.maxiters.Enabled = true;
                this.checkBox1.Enabled = true;
                createVariables.Visible = true;
                this.createVariables.Text = "Create Variables";

                this.calSubType.Enabled = false;
            }
            else if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.Optimization.ToString())
            {
                metricType.Items.Add(MetricMan.MetricValues[2]);
                metricType.Items.Add(MetricMan.MetricValues[7]);
                metricType.SelectedIndex = 0;

                this.stepSize.Enabled = true;
                this.tolerance.Enabled = true;
                this.metricType.Enabled = true;
                this.maxiters.Enabled = true;
                this.checkBox1.Enabled = true;

                this.calSubType.Enabled = false;
            }

            initCalibrationInfo();
        }

        private string createPriceSensitivityParameters()
        {
            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID))
            {
                MrktSimDBSchema.model_parameterRow priceSensParm =
                    theDb.CreateModelParameter(currentSimulation.scenarioRow.model_id, seg, "price_disutility", "segment_id");

                priceSensParm.name = seg.segment_name + " price sensitivity";

                theDb.CreateScenarioParameter(currentSimulation.id, priceSensParm.id);
            }

            return null;
        }

        private string createMediaParameters() {

            // need list of media plan ids
            Dictionary<int, Dictionary<int, string>> plans = theDb.SimulationMediaPlans(currentSimulation.id);

            foreach( Dictionary<int, string> planNames in plans.Values ) {
                foreach( int mediaID in planNames.Keys ) {

                    MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( currentSimulation.id, "market_plan", "parm2", "id", mediaID );

                    if (simParm == null) {
                        return "No parameter exists for plan " + planNames[ mediaID ];
                    }
                }
            }

            return null;
        }
        private string createAttributeParameters()
        {
            foreach (MrktSimDBSchema.consumer_preferenceRow pref in theDb.Data.consumer_preference)
            {
                //pref.pre_preference_value
                // pref.record_id
                // pref.post_preference_value

                MrktSimDBSchema.model_parameterRow preParm = 
                    theDb.CreateModelParameter(currentSimulation.scenarioRow.model_id,  pref, "pre_preference_value", "record_id");

                preParm.name = pref.product_attributeRow.product_attribute_name + "_" + pref.segmentRow.segment_name + "_pre_use_preference";
                theDb.CreateScenarioParameter(currentSimulation.id, preParm.id);

                if (currentSimulation.scenarioRow.Model_infoRow.attribute_pre_and_post)
                {
                    MrktSimDBSchema.model_parameterRow postParm =
                    theDb.CreateModelParameter(currentSimulation.scenarioRow.model_id, pref, "post_preference_value", "record_id");

                    postParm.name = pref.product_attributeRow.product_attribute_name + "_" + pref.segmentRow.segment_name + "_post_use_preference";
                    theDb.CreateScenarioParameter(currentSimulation.id, postParm.id);
                }
            }

            return null;
        }

        private void initCalibrationInfo()
        {
            if (currentSimulation == null)
                return;

            if( calibrationType.SelectedItem == null )
                return;

            CalibrationControl cal = new CalibrationControl();

            cal.StepSize = (double)stepSize.Value;
            cal.Tolerance = (double)tolerance.Value;
            cal.MaxIters = (int)maxiters.Value;
            cal.ClearAll = checkBox1.Checked;
            cal.Metric = (Value)this.metricType.SelectedItem;
            cal.Type = (CalibrationControl.CalibrationType)Enum.Parse(typeof(CalibrationControl.CalibrationType), calibrationType.SelectedItem.ToString());

            cal.ApplyParameters = applyParms.Checked;

            foreach( CalibrationControl.CalibrationSubType subType in Enum.GetValues(typeof(CalibrationControl.CalibrationSubType))) {
                cal.SetCalibration(subType, false );
            }

            foreach( int subType in calSubType.CheckedIndices ) {
                cal.SetCalibration( (CalibrationControl.CalibrationSubType)subType, true );
            }

            currentSimulation.control_string = cal.Control;

          

            switch (cal.Type)
            {
                case CalibrationControl.CalibrationType.Standard:

                    calibrationInfo.Text = "This calibration will run one simulation\r\n";
                    calibrationInfo.Text += "Parameter values will be used.\r\n";
                    calibrationInfo.Text += "Variables will be ignored\r\n";
                    break;

                case CalibrationControl.CalibrationType.AutoMatic:
                    calibrationInfo.Text = "This calibration will minimize the selected metric by calibrating the selected items\r\n";
                    calibrationInfo.Text += "When the error is less then tolerance, the simulation will halt\r\n";
                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += "Click on the '" + createVariables.Text + "' button to automatically create parameters\r\n";
                    calibrationInfo.Text += "Remove any parameters, you do not wish modified\r\n";
                    calibrationInfo.Text += "\r\n";
                  
                    calibrationInfo.Text += currentSimulation.Getscenario_parameterRows().Length.ToString() + " parameters will be modified.\r\n";
                
                    break;

                case CalibrationControl.CalibrationType.Parametric:
                    calibrationInfo.Text = "This calibration will attempt to minimize share error by monitoring the chosen metric for each product.\r\n";
                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += "Click on the '" + createVariables.Text + "' button to automatically create variables from simulation parameters\r\n";
                    calibrationInfo.Text += "\r\n";
                  
                    calibrationInfo.Text += "The parameter vales will be scaled down to reduce the metric error.\r\n";
                    calibrationInfo.Text += "The scaling is equal to the  1 - (step size) x (metric error).\r\n";
                    calibrationInfo.Text += "Note that a negative step size will scale parameter values upwards.\r\n";
                    calibrationInfo.Text += "When the metric error is less then tolerance, the error is ignored. \r\n";
                  

                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += currentSimulation.Getscenario_parameterRows().Length.ToString() + " parameters will be modified.\r\n";
                    
                    calibrationInfo.Text += currentSimulation.Getscenario_variableRows().Length.ToString() + " variables have been created.\r\n";
                   
                    calibrationInfo.Text += "\r\n";

                    if (cal.ClearAll)
                    {
                        calibrationInfo.Text += "All results will be cleared after each run" + "\r\n";
                    }


                    string variableUpdate = createCalibrationVariables(true);

                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += variableUpdate;

                    if (variableUpdate != null)
                    {
                        this.createVariables.Enabled = true;
                    }
                    else
                    {
                        this.createVariables.Enabled = false;
                    }
                 
                    

                   
                    break;

                case CalibrationControl.CalibrationType.Optimization:

                    calibrationInfo.Text = "This calibration will attempt to minimize the metric value specified with the current set of variables.\r\n";
                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += "Optimization stops when differences in calculated values are less then the tolerance specified";
                    calibrationInfo.Text += " or when the number of iterations exceeds the maximum.\r\n";
                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += "The step size will control how aggressive the optimizer will step along a particular line.\r\n";
                    calibrationInfo.Text += "A smaller the step size will cause more evaulations and a slower convergence. \r\n";
                 
                    calibrationInfo.Text += "\r\n";
                    calibrationInfo.Text += currentSimulation.Getscenario_parameterRows().Length.ToString() + " parameters are bing modified.\r\n";
                    calibrationInfo.Text += currentSimulation.Getscenario_variableRows().Length.ToString() + " variables have been created.\r\n";
                    

                    if (cal.ClearAll)
                    {
                        calibrationInfo.Text += "All results will be cleared after each run" + "\r\n";
                    }

                    break;

            }
        }

        private void updateCalibrationInfo(object sender, EventArgs e)
        {
            initCalibrationInfo();
        }

        private void createVariables_Click(object sender, EventArgs e)
        {
            string error = null;
            string message = null;

            if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.AutoMatic.ToString())
            {
                if( calSubType.CheckedIndices.Contains((int)CalibrationControl.CalibrationSubType.Attribute))
                {
                // create attribute parameters
                    error = createAttributeParameters();
                    message += "Attribute parameters successfully created\n\r";
                }

                if( calSubType.CheckedIndices.Contains( (int)CalibrationControl.CalibrationSubType.PriceSensitivity ) ) {
                    // create attribute parameters
                    error = createPriceSensitivityParameters();
                    message += "Price Sensitivity parameters successfully created\n\r";
                }

                if( calSubType.CheckedIndices.Contains( (int) CalibrationControl.CalibrationSubType.Media ) ) {
                    // create attribute parameters
                    error = createMediaParameters();
                    message += "Media parameters successfully created\n\r";
                }


            }
            else if (calibrationType.SelectedItem.ToString() == CalibrationControl.CalibrationType.Parametric.ToString())
            {
                // move this to options form
                error = createCalibrationVariables(false);

                if (error != null)
                {
                    MessageBox.Show(error);
                }
                else
                {
                    message = "Calibration variables successfully created";
                }
            }

            this.initCalibrationInfo();

            if (message != null)
            {
                MessageBox.Show(message);
            }
        }


        private string createCalibrationVariables(bool checkOnly)
        {
            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();

            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

            if (parameters.Length == 0)
            {
                return "You must add parameters to the simulation first";
            }

            // compute list of products for this scenario
            ArrayList products = new ArrayList();

            int[] parameterProduct = new int[parameters.Length];

            int parmDex = 0;
            foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
            {
                MrktSimDBSchema.model_parameterRow modelParm = param.model_parameterRow;

                int product_id = theDb.ParameterProduct(modelParm);

                if (product_id == Database.AllID)
                {
                    continue;
                }

                // check if in list already

                if (products.IndexOf(product_id) < 0)
                {
                    products.Add(product_id);
                }

                parameterProduct[parmDex] = product_id;
                ++parmDex;
            }

            if (products.Count == 0)
            {
                return "No parameters reference a product";
            }

            bool createParms = false;

            if (variables.Length != products.Count)
                createParms = true;
            else
            {
                for (int ii = 0; !createParms && ii < variables.Length; ++ii)
                {
                    if (variables[ii].product_id == Database.AllID)
                    {
                        createParms = true;
                    }
                }
            }

            if (checkOnly)
            {
                if (createParms)
                {

                    return newVariableString;
                }

                return null;
            }

            if (createParms)
            {
                DialogResult rslt = MessageBox.Show(newVariableString, "Edit Variables?", MessageBoxButtons.OKCancel);

                if (rslt != DialogResult.OK)
                    return "Variables not created";

            }
            else
            {
                return null;
            }

            // remove any pre-existing variables
            foreach (MrktSimDBSchema.scenario_variableRow variable in variables)
            {
                variable.Delete();
            }

            // create one variable for each product referenced
            for (int ii = 0; ii < parameters.Length; ++ii)
            {
                parameters[ii].expression = "CurVal";

            }

            int varDex = 1;
            foreach (int product_id in products)
            {
                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id(product_id);

                if (prod != null)
                {
                    string prodName = prod.product_name;

                    int maxProdLen = 7;
                    if( varDex >= 10 ) {
                        maxProdLen = 6;
                    }
                    if( varDex >= 100 ) {
                        maxProdLen = 5;
                    }

                    if( prodName.Length > maxProdLen )
                    {
                        prodName = prodName.Substring( 0, maxProdLen );
                    }

                    prodName = "V" + varDex.ToString() + "_" + prodName;
                    varDex++;

                    MrktSimDBSchema.scenario_variableRow variable = theDb.CreateScenarioVariable(currentSimulation, prodName);
                    variable.product_id = product_id;

                    // all parsm referencing this product should evaluate off this variable
                    for (int ii = 0; ii < parameters.Length; ++ii)
                    {
                        if (parameterProduct[ii] == product_id)
                        {
                            MrktSimDBSchema.scenario_parameterRow param = parameters[ii];
                            param.expression = "CurVal*" + variable.token;
                        }
                    }
                }
                else
                {
                    return "Product with id = " + product_id + " does not exists.";
                }
            }

            return null;
        }

        public override void Refresh()
        {
            base.Refresh();

            initCalibrationInfo();
        }

        public void WriteData()
        {
            initCalibrationInfo();
        }
    }
}
