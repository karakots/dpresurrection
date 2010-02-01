using System;
using System.Data;
using System.Diagnostics; // process

using MrktSimDb;
using MrktSimDb.Metrics;
using EquationParser;
using Common.Dialogs;
// hello
namespace BatchRun
{
	/// <summary>
	/// Summary description for SimController.
	/// </summary>
	public class SimController
	{
		public enum SimState 
		{
			NoSimEngine,
			NotRunning,
			Running,
			Stopping
		}

		public class CancelSim : EventArgs
		{
			private bool cancel;
			public bool Cancel
			{
				get
				{
					return cancel;
				}

				set
				{
					cancel = value;
				}
			}

			public CancelSim()
			{
				cancel = false;
			}
		}

		public delegate void Refresh(object sender, CancelSim e);
		public event Refresh SimUpdated;

		public delegate void SimDone();
		public event SimDone ReportFinished;

		private ModelInfoDb modelDb = null;
		private MSConnect msConnect;
		private Process simProcess;
		private CancelSim cancelSim;

		private System.Random rand;

		private MrktSimDb.ModelInfo.scenarioRow currentScenario;
		private ModelInfo.sim_queueRow currentSim = null;

		double[] val;
		double[] origParmValues;
		private SimpleParser parser;

		public SimController(int projectID, string udlPath, Process simEngineProcess)
		{
			simProcess = simEngineProcess;
			
			//check connection
			msConnect = new MSConnect(udlPath);

			if (msConnect == null)
				return;

			if (!msConnect.TestConnection())
				return;
			
			// open model
			modelDb = new ModelInfoDb();
			modelDb.Connection = msConnect.Connection;		
			modelDb.ProjectID = projectID;

			cancelSim = new CancelSim();
		
			rand = new Random();
		}

		public void Run()
		{
			cancelSim.Cancel = false;

			while (RunNextScenario() == true);

			if (ReportFinished != null)
				ReportFinished();
		}

		// look in database for a scenario to run
		// if nothing there return false
		public bool RunNextScenario()
		{
			if (modelDb == null)
				return false;

			modelDb.Refresh();

			// find scenario and create standard run
			string query = "queued = 1 AND sim_num > 0";
			string sort = "sim_num";

			DataRow[] rows = modelDb.ModelData.scenario.Select(query, sort, DataViewRowState.CurrentRows);

			if (rows.Length == 0)
				return false;

			currentScenario = (MrktSimDb.ModelInfo.scenarioRow) rows[0];

			switch (currentScenario.type)
			{
				case (int) ScenarioType.Standard:
					runStandard();
					return true;
				
				case (int) ScenarioType.Parallel:
					return runParallel();

				case (int) ScenarioType.Serial:
					return runSerial();

				case (int) ScenarioType.Random:
					return runRandom();

				case (int) ScenarioType.Optimize:
					return runOptimization();

				case (int) ScenarioType.Statistical:				
					return runStatistical();

				case (int) ScenarioType.Calibration:				
					return runCalibration();
			}

			return false;
		}

		private bool runStandard()
		{
			bool cancel = false;

			currentSim = modelDb.CreateRun(currentScenario);

			// no longer needs to be run
			currentScenario.sim_num = 0;

			update(out cancel);

			if (cancel)
				return false;

			runSim(out cancel);

			if (cancel)
				return false;

			return true;
		}

		private double uniform(double min, double max)
		{
			double val = rand.NextDouble();

			return min + val * ( max - min);
		}

		/// <summary>
		/// This produces a random number centered about (min + max)/2
		/// a number is first chose with a binomial distribution
		/// that picks a subinterval that we then perform a uniform selection on
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		private double centered(double min, double max)
		{
			// pick a number between 0, 4 wih a binomial distribution
			// probabilities are 
			// 0.000976563 0.009765625 0.043945313 0.1171875 0.205078125
			// 0.24609375 <-- center value
			// 0.205078125 0.1171875 0.043945313 0.009765625 0.000976563

			const int numIntervals = 11;
			double[] binom = {
									  0.000976563,
									  0.010742188,
									  0.0546875,
									  0.171875,
									  0.376953125,
									  0.623046875,
									  0.828125,
									  0.9453125,
									  0.989257813,
									  0.999023438,
									  1.0
								  };

			double size = (max - min)/numIntervals;

			// pick a number between 0 and 1
			double val = rand.NextDouble();

			// find out which number this is
			int interval = 0;
			while (interval < numIntervals - 1 && binom[interval] < val)
				interval++;

			double newMin = min + size * interval;
			double newMax = min + size * (interval + 1);

			return uniform(newMin, newMax);
		}


		// computes a random variable value for this variable
		private double randomVariableValue(ModelInfo.scenario_variableRow variable)
		{
			switch (variable.type)
			{
				case 2:
					return centered(variable.min, variable.max);
				default:
					return uniform(variable.min, variable.max);
			}
		}

		/// <summary>
		/// compute next value
		/// done is set to false if we should continue evaluating
		/// </summary>
		/// <param name="curStep"></param>
		/// <param name="?"></param>
		/// <param name="variable"></param>
		/// <param name="done"></param>
		/// <returns></returns>
		private double variableValue(ref int curStep, ModelInfo.scenario_variableRow variable, out bool done)
		{
			done = true;

			// use this to avoid roundoff error as we step along
			const double eps = 0.000000000001;

			double rval = 0.0;

			switch (variable.type)
			{
				case 1:
					if (curStep < variable.num_steps)
						done = false;
						
					rval =  uniform(variable.min, variable.max);
					break;

				case 2:
					if (curStep < variable.num_steps)
						done = false;

					rval =  centered(variable.min, variable.max);
					break;

				default: // stepped
					double scale = 1.0;

					if (variable.num_steps > 0)
						scale = (1.0 + eps)/(variable.num_steps + 1);

					double stepSize = scale * (variable.max - variable.min);

					// check if done
					// Note: always at least one evaluation
					if (curStep > 0)
					{
						double prevVal = variable.min + (curStep - 1) * stepSize;

						if (prevVal < variable.max)
							done = false;
					}

					if (done)
						rval = variable.min;
					else
					{
						rval = variable.min + curStep * stepSize;

						// avoid extra bit we added earlier
						if (rval > variable.max)
							rval = variable.max;
					}
			
					break;
			}

			if (done)
				curStep = 0;
	
			return rval;
		}

		private bool runParallel()
		{
			return runParallel(null);
		}

		private bool runParallel(ModelInfo.scenario_parameterRow freeParm)
		{
			if (currentScenario.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			// no longer needs to be run
			currentScenario.sim_num = 0;
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			// save current parm values
			origParmValues = new double[parameters.Length];

			int parmDex;
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				origParmValues[parmDex] = parameters[parmDex].aValue;
			}

			// we loop through all the variables creating values for each

			// initialize
			bool done = false;

			// values and stepsize
			val = new double[variables.Length];
			int[] curEval = new int[variables.Length];
			string[] tokens = new string[variables.Length + 1];

			for( int ii = 0; ii < variables.Length; ii++)
			{
				curEval[ii] = 0;

				// set each variable to the beginning value
				val[ii] = variableValue(ref curEval[ii], variables[ii], out done);

				tokens[ii] = variables[ii].token;
			}

			// special constants are added at end
			tokens[variables.Length] = "CurVal";

			parser = new SimpleParser(tokens);

			// Note: doen will be true when ALL variables return done = true
			done = false;

			while(!done)
			{
				// create run
				currentSim = modelDb.CreateRun(currentScenario);

				if (freeParm != null)
				{
					// record the param_id on the sim
					currentSim.param_id = freeParm.param_id;
				}

				currentSim.name = "run ";
				for( int ii = 0; ii < variables.Length; ii++)
				{
					currentSim.name += val[ii].ToString("F") + " ";
				}

				// write variable values to database
				for( int ii = 0; ii < variables.Length; ii++)
				{
					modelDb.CreateSimVariableValue(currentSim, variables[ii], val[ii]);
				}

				// update scenario parameter values
				parser.updateValues(val);

				parmDex = 0;
				foreach(ModelInfo.scenario_parameterRow param in parameters)
				{
					// special value for this parameter
					parser.updateValue("CurVal", origParmValues[parmDex]);
					parmDex++;

					if (param.expression != null && param.expression.Length > 0)
					{
						if (freeParm == null || freeParm == param)
						{
							param.aValue = parser.ParseEquation(param.expression);
						}
					}
				}

				update(out cancel);

				if (cancel)
					break;
				
				runSim(out cancel);
				
				if (cancel)
					break;

				// increment
				for( int ii = 0; ii < variables.Length; ii++)
				{
					// increment this value
					curEval[ii]++;

					val[ii] = variableValue(ref curEval[ii], variables[ii], out done);

					// we break out if not done
					// this causes us to continue with this variable value
					if (!done)
						break;

					// if we never break then we are done with ALL variables
				}
			}

			// return parameter values to orig
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				parameters[parmDex].aValue = origParmValues[parmDex];
			}

			// we already know if we canceled or not
			// I don't want this to somehow return something different
			bool dummy;

			update(out dummy);

			if (cancel)
				return false;

			return true;
		}

		private bool runSerial()
		{
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			foreach(ModelInfo.scenario_parameterRow param in parameters)
			{
				if(!runParallel(param))
					return false;
			}

			return true;
		}

		private bool runRandom()
		{
			if (currentScenario.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			// no longer needs to be run
			currentScenario.sim_num = 0;
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			// save current parm values
			origParmValues = new double[parameters.Length];

			int parmDex;
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				origParmValues[parmDex] = parameters[parmDex].aValue;
			}

			// we sample randomly from all the variables
			// number of samples is the product of the number of samples normally computed
			// values and stepsize
			val = new double[variables.Length];
			string[] tokens = new string[variables.Length + 1];

			int numSamples = 0;
			for( int ii = 0; ii < variables.Length; ii++)
			{
				numSamples += variables[ii].num_steps;

				tokens[ii] = variables[ii].token;

				// set each variable to the beginning value
				val[ii] = randomVariableValue(variables[ii]);
			}

			tokens[variables.Length] = "CurVal";

			if (numSamples == 0)
				return false;

			parser = new SimpleParser(tokens);

			// Note: doen will be true when ALL variables return done = true
			int iter = 0;
			while(iter < numSamples)
			{
				iter++;

					// set each variable to a random value
				for( int jj = 0; jj < variables.Length; jj++)
				{
				
					val[jj] = randomVariableValue(variables[jj]);
				}


				// create run
				currentSim = modelDb.CreateRun(currentScenario);

				currentSim.name = "random ";
				for( int ii = 0; ii < variables.Length; ii++)
				{
					currentSim.name += val[ii].ToString("F") + " ";
				}

				// write variable values to database
				for( int ii = 0; ii < variables.Length; ii++)
				{
					modelDb.CreateSimVariableValue(currentSim, variables[ii], val[ii]);
				}

				// update scenario parameter values
				parser.updateValues(val);

				parmDex = 0;
				foreach(ModelInfo.scenario_parameterRow param in parameters)
				{
					parser.updateValue("CurVal", origParmValues[parmDex]);
					parmDex++;
					param.aValue = parser.ParseEquation(param.expression);
				}

				update(out cancel);

				if (cancel)
					break;
				
				runSim(out cancel);
				
				if (cancel)
					break;
			}

			// return parameter values to orig
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				parameters[parmDex].aValue = origParmValues[parmDex];
			}

			// we already know if we canceled or not
			// I don't want this to somehow return something different
			bool dummy;

			update(out dummy);

			if (cancel)
				return false;

			return true;
		}

		private bool runOptimization()
		{
			if (currentScenario.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			// no longer needs to be run
			currentScenario.sim_num = 0;
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			// save current parm values
			origParmValues = new double[parameters.Length];

			int parmDex;
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				origParmValues[parmDex] = parameters[parmDex].aValue;
			}

		
			Optimize opt = new Optimize(variables.Length);
			val = opt.VariableValue;

			string[] tokens = new string[variables.Length + 1];
			double[] min = new double[variables.Length];
			double[] max = new double[variables.Length];

			for( int ii = 0; ii < variables.Length; ii++)
			{
				tokens[ii] = variables[ii].token;
				min[ii] = variables[ii].min;
				max[ii] = variables[ii].max;
			}

			tokens[variables.Length] = "CurVal";

			opt.VariableMin = min;
			opt.VariableMax = max;
			opt.Evaluate +=new BatchRun.Optimize.Evaluator(evaluate);

			parser = new SimpleParser(tokens);

			cancel = !opt.Run(25);

			// return parameter values to orig

			// update scenario parameter values
			parser.updateValues(val);

			parmDex = 0;
			foreach(ModelInfo.scenario_parameterRow param in parameters)
			{
				parser.updateValue("CurVal", origParmValues[parmDex]);
				parmDex++;
				param.aValue = parser.ParseEquation(param.expression);
			}


			if (cancel)
				return false;

			return true;
		}

		private bool runStatistical()
		{
			if (currentScenario.Getscenario_simseedRows().Length == 0)
				return false;

			bool cancel = false;

			// no longer needs to be run
			currentScenario.sim_num = 0;

			foreach(ModelInfo.scenario_simseedRow simseed in currentScenario.Getscenario_simseedRows())
			{
				currentSim = modelDb.CreateRun(currentScenario);

				currentSim.seed = simseed.seed;

				currentSim.name = "stat " + simseed.seed;

				update(out cancel);

				if (cancel)
					return false;

				runSim(out cancel);

				if (cancel)
					return false;
			}

			return true;
		}


		/// <summary>
		/// For a calibration the assumption is that each variable 
		/// has a unique product associated to it
		/// </summary>
		/// <returns></returns>
		private bool runCalibration()
		{
			if (currentScenario.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			// no longer needs to be run
			currentScenario.sim_num = 0;
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			// save current parm values
			origParmValues = new double[parameters.Length];
			
			int parmDex;
			for(parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
			{
				origParmValues[parmDex] = parameters[parmDex].aValue;
			}

			Calibrate call = new Calibrate(currentScenario.control_string, variables.Length);

			val = call.VariableValue;

			string[] tokens = new string[variables.Length + 1];

			for( int ii = 0; ii < variables.Length; ii++)
			{
				tokens[ii] = variables[ii].token;
			}
		
			tokens[variables.Length] = "CurVal";

			call.Evaluate +=new BatchRun.Calibrate.Evaluator(evaluateCalibration);

			parser = new SimpleParser(tokens);

			cancel = !call.Run();

			if (!cancel)
			{
				// update scenario parameter values
				parser.updateValues(val);

				parmDex = 0;
				foreach(ModelInfo.scenario_parameterRow param in parameters)
				{
					parser.updateValue("CurVal", origParmValues[parmDex]);
					parmDex++;
					param.aValue = parser.ParseEquation(param.expression);
				}
			}

			if (cancel)
				return false;

			return true;
		}

		private void  runSim(out bool cancel)
		{
			try 
			{
				simProcess.Start();
			}
			catch (System.Exception oops)
			{
				string what = oops.Message;

				updateUI(out cancel);
				return;
			}

			// hey, wait a second
			System.Threading.Thread.Sleep(1000);

			updateUI(out cancel);

			simProcess.WaitForExit();

			updateUI(out cancel);

			// calculate precalculated values
			preCalcValues();
		}

		private void update(out bool cancel)
		{	
			modelDb.UpdateModelData();

			updateUI(out cancel);
		}

		private void updateUI(out bool cancel)
		{
			cancel = false;

			if (SimUpdated != null)
			{
				SimUpdated(this, cancelSim);

				if (cancelSim.Cancel)
					cancel = true;
			}
		}

		private double evaluate(out bool cancel)
		{
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			// create run
			currentSim = modelDb.CreateRun(currentScenario);

			currentSim.name = "opt ";
			for( int ii = 0; ii < val.Length; ii++)
			{
				currentSim.name += val[ii].ToString("F") + " ";
			}

			// write variable values to database
			for( int ii = 0; ii < variables.Length; ii++)
			{
				modelDb.CreateSimVariableValue(currentSim, variables[ii], val[ii]);
			}

			// update scenario parameter values
			parser.updateValues(val);

			int parmDex = 0;
			foreach(ModelInfo.scenario_parameterRow param in parameters)
			{
				parser.updateValue("CurVal", origParmValues[parmDex]);
				parmDex++;
				param.aValue = parser.ParseEquation(param.expression);
			}

			update(out cancel);

			if (cancel)
				return 0.0;
				
			runSim(out cancel);

			double rval = 0.0;

			Metric metric = Metric.GetMetric("aveSqrShareError");

			metric.Connection = modelDb.Connection;
			metric.Run = currentSim.run_id;

			rval = metric.Value;

			return rval;
		}


		// each parameter has a unique product
		private double[] evaluateCalibration(out bool cancel)
		{
			ModelInfo.scenario_variableRow[] variables = currentScenario.Getscenario_variableRows();
			ModelInfo.scenario_parameterRow[] parameters = currentScenario.Getscenario_parameterRows();

			double[] callVal = new double[variables.Length];

			// create run
			currentSim = modelDb.CreateRun(currentScenario);

			currentSim.name = "cal " + currentSim.run_id;

			// write variable values to database
			for( int ii = 0; ii < variables.Length; ii++)
			{
				modelDb.CreateSimVariableValue(currentSim, variables[ii], val[ii]);
			}

			// update scenario parameter values
			parser.updateValues(val);

			try
			{
				int parmDex = 0;
				foreach(ModelInfo.scenario_parameterRow param in parameters)
				{
					parser.updateValue("CurVal", origParmValues[parmDex]);
					parmDex++;
					param.aValue = parser.ParseEquation(param.expression);
				}
			}
			catch(Exception)
			{
				cancel = true;
				return callVal;
			}

			update(out cancel);

			if (cancel)
				return callVal;
				
			runSim(out cancel);

			Metric metric = Metric.GetMetric("shareDiff");
			
			metric.Connection = modelDb.Connection;
			metric.Run = currentSim.run_id;

			// loop over variables and extract product id
			for(int ii = 0; ii < variables.Length; ++ii)
			{
				metric.Product = variables[ii].product_id;
				callVal[ii] = metric.Value;
			}


			return callVal;
		}

		private void preCalcValues()
		{
			if (currentSim == null)
				return;

			modelDb.RefreshSimQueue();
			string orig = currentSim.current_status;
			currentSim.current_status = "Computing Metrics";

			bool cancel = false;
			update(out cancel);

			if (cancel)
				return;
		
			MetricMan metricMan = new MetricMan();
			metricMan.Connection = modelDb.Connection;
			metricMan.UseSimStartEndDates = true;
			metricMan.Compute(modelDb, currentSim.run_id);

			currentSim.current_status = orig;
			update(out cancel);

			if (currentSim.scenarioRow.delete_std_results == true)
			{
				modelDb.ClearResults(currentSim.run_id);
			}



//			// compute all metrics all the time
//			foreach(Metric metric in Metric.Types)
//			{
//				metric.Connection = modelDb.Connection;
//				metric.Run = currentSim.run_id;
//				metric.StartDate = currentScenario.start_date;
//				metric.EndDate = currentScenario.end_date;
//
//				// setting this will create pre-computed values in database
//				metric.ModelDb = modelDb;
//
//				double val = metric.Value;
//			}
		}
	}
}
