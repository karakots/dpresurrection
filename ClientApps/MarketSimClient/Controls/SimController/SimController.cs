using System;
using System.Data;
using System.Diagnostics; // process

using MrktSimDb;
using MrktSimDb.Metrics;
using EquationParser;

namespace SimControlMethods
{
	/// <summary>
	/// Summary description for SimController.
	/// </summary>
	public class SimController
	{
		public enum SimState 
		{
			NotConfigured,
			NotRunning,
			Running,
			Stopping
		}

		#region static methods and variables

        static private SimQueueDb mySimQueueDb = new SimQueueDb();
        static public SimQueueDb Model
		{
			get
			{
				return mySimQueueDb;
			}
		}

		static public string ModelName
		{
			get
			{
				if (mySimQueueDb.Connection != null)
				{
					return mySimQueueDb.Connection.Database;
				}

				return null;
			}
		}

		static public void OpenModel(string udlPath, string fileName)
		{
            string error;

            OpenModel(udlPath, fileName, out error);
		}

        static public bool OpenModel(string udlPath, string fileName, out string error)
        {
            error = null;

            MSConnect msConnect = new MSConnect(udlPath);

            if (msConnect == null)
            {
                error = "Error creating connection object";
                return false;
            }

            msConnect.ConnectFile = fileName;

            if (!msConnect.TestConnection(out error))
            {
                mySimQueueDb.Connection = null;
                return false;
            }

            mySimQueueDb.Connection = msConnect.Connection;
            mySimQueueDb.ProjectID = Database.AllID;

            return true;
        }


        static public MrktSimDBSchema.simulationRow SimulationToRun()
		{	
			if (mySimQueueDb.Connection == null)
				return null;

			string query = "sim_num > 0";
			string sort = "";

			DataRow[] rows = mySimQueueDb.Data.simulation.Select(query, sort, DataViewRowState.CurrentRows);

			if (rows.Length > 0)
			{
                return (MrktSimDBSchema.simulationRow)rows[0];
			}

			return null;
		}

        static public string EngineConnectFile
        {
            get
            {
                if (mySimQueueDb.Connection == null)
                    return null;

                return engineConnectFileFromConnection(mySimQueueDb.Connection);
            }
        }

        static private string engineConnectFileFromConnection(System.Data.OleDb.OleDbConnection connection)
        {
            return connection.DataSource.Replace(@"\", "_") + connection.Database;
        }


		#endregion

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

        #region Simulation Events
        public delegate void Refresh(object sender, CancelSim e, int run_id);
		public event Refresh SimUpdated;

		public delegate void SimDone(int id);
		public event SimDone ReportFinished;

        #endregion

        private bool clearAllResults = false;

        private bool done;
		public bool Done
		{
			get
			{
				return done;
			}
		}

		public Process Sim
		{
			get
			{
				return simProcess;
			}
		}

		public MrktSimDb.MrktSimDBSchema.simulationRow Simulation
		{
			get
			{
				return currentSimulation;
			}
		}

		public MrktSimDBSchema.sim_queueRow SimQueueItem
		{
			get
			{
				return currentRun;
			}
		}

        private SimulationDb simDb = null;
		private MSConnect msConnect;
		private Process simProcess;
		private CancelSim cancelSim;

		private System.Random rand;

		private MrktSimDb.MrktSimDBSchema.simulationRow currentSimulation;
		private MrktSimDBSchema.sim_queueRow currentRun = null;

		double[] val;
		double[] origParmValues;
		private SimpleParser parser;

        public SimController(MrktSimDBSchema.simulationRow simToRun, string fileName, string udlPath, Process simEngineProcess)
		{
			simProcess = simEngineProcess;
			
			//check connection
			msConnect = new MSConnect(udlPath);
            msConnect.ConnectFile = fileName;

			if (msConnect == null)
				return;

			if (!msConnect.TestConnection())
				return;


            // check if we are doing a calibration
            CalibrationControl cal = new CalibrationControl(simToRun.control_string);

            if (cal.Type != CalibrationControl.CalibrationType.Standard)
            {
                simDb = new CallibrationDb();
            }
            else
            {
                // open simulation
                simDb = new SimulationDb();
            }

            simDb.Id = simToRun.id;

			simDb.Connection = msConnect.Connection;

            simDb.Refresh();

            DataRow[] rows = simDb.Data.simulation.Select("", "", DataViewRowState.CurrentRows);

            if (rows.Length == 1)
            {
                currentSimulation = (MrktSimDb.MrktSimDBSchema.simulationRow)rows[0];
            }

			cancelSim = new CancelSim();
		
			rand = new Random();

			done = false;
		}

        private void refreshCurrentRun()
        {
            simDb.RefreshTable(simDb.Data.sim_queue);
        }

        private void RefreshDb()
        {
            int runID = Database.AllID;

            currentSimulation = null;

            if (currentRun != null)
            {
                runID = currentRun.run_id;
                currentRun = null;
            }

            simDb.Refresh();

            DataRow[] rows = simDb.Data.simulation.Select("", "", DataViewRowState.CurrentRows);

            if (rows.Length == 1)
            {
                currentSimulation = (MrktSimDb.MrktSimDBSchema.simulationRow)rows[0];
            }

            rows = simDb.Data.sim_queue.Select("run_id = " + runID, "", DataViewRowState.CurrentRows);

            if (rows.Length == 1)
            {
                currentRun = (MrktSimDb.MrktSimDBSchema.sim_queueRow)rows[0];
            }
        }

        public void DequeueSim()
        {
            if (currentSimulation == null)
                return;

            // no longer needs to be run
            currentSimulation.sim_num = -1;

            simDb.Update();
        }

        public bool QueuecurrentSimulation()
        {
            if (currentSimulation == null)
                return false;

            // no longer needs to be run
            currentSimulation.sim_num = 0;
            simDb.Update();

            return true;
        }

        public void Run()
        {
            cancelSim.Cancel = false;

            QueuecurrentSimulation();

            RuncurrentSimulation();

            currentSimulation.sim_num = -1;

            simDb.Update();

            done = true;

            if (ReportFinished != null)
                ReportFinished(currentSimulation.id);
        }

		// look in database for a scenario to run
		// if nothing there return false
		public bool RuncurrentSimulation()
		{

			switch (currentSimulation.type)
			{
				case (int) SimulationDb.SimulationType.Standard:
					runStandard();
					return true;
				
				case (int) SimulationDb.SimulationType.Parallel:
					return runParallel();

                case (int)SimulationDb.SimulationType.Serial:
					return runSerial();

                case (int)SimulationDb.SimulationType.Random:
					return runRandom();

                    // no used - moved to calibration
                case (int)SimulationDb.SimulationType.Optimize:
                    return false;

                case (int)SimulationDb.SimulationType.Statistical:				
					return runStatistical();

                case (int)SimulationDb.SimulationType.Calibration:				
					return runCalibration();

                case (int)SimulationDb.SimulationType.CheckPoint:				
					return runCheckPoint();
			}

			return false;
		}

		private bool runStandard()
		{
			bool cancel = false;

			currentRun = simDb.CreateRun(currentSimulation);

			update(out cancel);

			if (cancel)
				return false;

			runSim(out cancel);

			if (cancel)
				return false;

			return true;
		}

		private bool runCheckPoint()
		{
			bool cancel = false;

			currentRun = simDb.CreateRun(currentSimulation);

            currentRun.name = "checkpoint";

			update(out cancel);

			if (cancel)
				return false;

			string checkPointDirPath = this.simProcess.StartInfo.WorkingDirectory + @"\checkpoints";

			if (currentSimulation.scenarioRow.Model_infoRow.checkpoint_file == "NA")
			{
                string dirName = "chkpnt_" + currentSimulation.scenarioRow.Model_infoRow.model_name;

				if (dirName.Length > 100)
				{
					dirName = dirName.Substring(0, 100);
				}

				// create a check point directory
				// and set the model_info

				

				System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(checkPointDirPath);

				if (!dirInfo.Exists)
				{
					return false;
				}

				System.IO.DirectoryInfo[] dirs = dirInfo.GetDirectories();

				int index = 1;
				bool done = false;
				while(!done && index < 10000)
				{
					done = true;
					foreach(System.IO.DirectoryInfo dir in dirs)
					{
						if (dir.Name == dirName)
						{
							// mangle and try again
                            dirName = "chkpnt" + index + "_" + currentSimulation.scenarioRow.Model_infoRow.model_name;

							if (dirName.Length > 100)
							{
								dirName = dirName.Substring(0, 100);
							}

							index++;

							done = false;

							break;
						}
					}
				}

				// we have a valid directory name
				dirInfo.CreateSubdirectory(dirName);

				currentSimulation.scenarioRow.Model_infoRow.checkpoint_file = dirName;
			}
			else
			{
				// make sure the directory exists
				string checkPointDir = checkPointDirPath + @"\" + currentSimulation.scenarioRow.Model_infoRow.checkpoint_file;
				System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(checkPointDir);

				if (!dirInfo.Exists)
				{
					try
					{
						dirInfo.Create();
					}
					catch (System.Exception cantCreate)
					{
						string what = cantCreate.Message;

						updateUI(out cancel);
						return false;
					}
				}
			}

			currentSimulation.scenarioRow.Model_infoRow.checkpoint_date = currentSimulation.end_date;
            currentSimulation.scenarioRow.Model_infoRow.checkpoint_scale_factor = currentSimulation.scale_factor;

			update(out cancel);

            int run_id = currentRun.run_id;
            string connect_file = engineConnectFileFromConnection(msConnect.Connection);
            simProcess.StartInfo.Arguments = "-f " + connect_file + " -s " + run_id;

			try 
			{
				simProcess.Start();
			}
			catch (System.Exception oops)
			{
				string what = oops.Message;

				updateUI(out cancel);
				return false;
			}

			// hey, wait a second
			System.Threading.Thread.Sleep(5000);

			updateUI(out cancel);

			simProcess.WaitForExit();

            refreshCurrentRun();

			// check state of sim if not "done" then checkpointing is not valid
			if (currentRun.current_status == "done")
			{
				// need to update model_info
                currentSimulation.scenarioRow.Model_infoRow.checkpoint_valid = true;

                currentRun.current_status = "Check point created";
			}
			else
			{
                currentSimulation.scenarioRow.Model_infoRow.checkpoint_valid = false;
				
				currentRun.current_status = "Check point not created";
			}

			update(out cancel);

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
        private double randomVariableValue(MrktSimDBSchema.scenario_variableRow variable)
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
        private double variableValue(ref int curStep, MrktSimDBSchema.scenario_variableRow variable, out bool done)
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

        private bool runParallel(MrktSimDBSchema.scenario_parameterRow freeParm)
		{
			if (currentSimulation.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			
			MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
			MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

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
				currentRun = simDb.CreateRun(currentSimulation);

				if (freeParm != null)
				{
					// record the param_id on the sim
					currentRun.param_id = freeParm.param_id;
				}



                //for( int ii = 0; ii < variables.Length; ii++)
                //{
                //    currentRun.name += val[ii].ToString("F") + " ";
                //}

				// write variable values to database
				for( int ii = 0; ii < variables.Length; ii++)
				{
					simDb.CreateSimVariableValue(currentRun, variables[ii], val[ii]);
				}

				// update scenario parameter values
				parser.updateValues(val);

				parmDex = 0;
				foreach(MrktSimDBSchema.scenario_parameterRow param in parameters)
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
			MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

			foreach(MrktSimDBSchema.scenario_parameterRow param in parameters)
			{
				if(!runParallel(param))
					return false;
			}

			return true;
		}

		private bool runRandom()
		{
			if (currentSimulation.Getscenario_variableRows().Length == 0)
				return false;

			bool cancel = false;

			
			MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
			MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

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
				currentRun = simDb.CreateRun(currentSimulation);

				// write variable values to database
				for( int ii = 0; ii < variables.Length; ii++)
				{
					simDb.CreateSimVariableValue(currentRun, variables[ii], val[ii]);
				}

				// update scenario parameter values
				parser.updateValues(val);

				parmDex = 0;
				foreach(MrktSimDBSchema.scenario_parameterRow param in parameters)
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

	

		private bool runStatistical()
		{
			if (currentSimulation.Getscenario_simseedRows().Length == 0)
				return false;

			bool cancel = false;

		

			foreach(MrktSimDBSchema.scenario_simseedRow simseed in currentSimulation.Getscenario_simseedRows())
			{
				currentRun = simDb.CreateRun(currentSimulation);

				currentRun.seed = simseed.seed;

				currentRun.name = "stat " + simseed.seed;

				update(out cancel);

				if (cancel)
					return false;

				runSim(out cancel);

				if (cancel)
					return false;
			}

			return true;
        }

        #region Calibration
        private bool runCalibration()
        {
            CalibrationControl control = new CalibrationControl(currentSimulation.control_string);

            if (control.Type == CalibrationControl.CalibrationType.Standard)
            {
                return this.runStandard();
            }

            if (control.Type == CalibrationControl.CalibrationType.AutoMatic)
            {
                // run attribute calibration
                return runAutoCalibration( control );
            }
            
            if (control.Type == CalibrationControl.CalibrationType.Parametric)
            {
                return runGeneralCalibration(control);
            }

            if (control.Type == CalibrationControl.CalibrationType.Optimization)
            {
                return runOptimization(control);
            }

            return false;
		}

        private bool runAutoCalibration(CalibrationControl control)
        {

            bool cancel = false;

            AutoCalibration call = new AutoCalibration( control );

            call.Db = simDb as CallibrationDb;

            clearAllResults = call.ClearAll;

            call.Evaluate += new SimControlMethods.Calibrate.CalibrationMetricEvaluator(this.evaluateMetricCalibrationRun);

            cancel = !call.Run();

            return true;
        }

        //private bool runPriceSensitivityCalibration(CalibrationControl control)
        //{

        //    bool cancel = false;

        //    PriceSensitivityCalibration call = new PriceSensitivityCalibration(control);

        //    call.Db = simDb as CallibrationDb;

        //    clearAllResults = call.ClearAll;

        //    call.Evaluate += new SimControlMethods.Calibrate.CalibrationMetricEvaluator(this.evaluateMetricCalibrationRun);

        //    cancel = !call.Run();

        //    return true;
        //}

        private bool runGeneralCalibration(CalibrationControl control)
        {

            if (currentSimulation.Getscenario_variableRows().Length == 0)
                return false; 


            bool cancel = false;


            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

            // save current parm values
            origParmValues = new double[parameters.Length];

            int parmDex;
            for (parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
            {
                origParmValues[parmDex] = parameters[parmDex].aValue;
            }

            GeneralCalibration call = new GeneralCalibration(control, variables.Length);

            clearAllResults = call.ClearAll;

            val = call.VariableValue;

            string[] tokens = new string[variables.Length + 1];

            for (int ii = 0; ii < variables.Length; ii++)
            {
                tokens[ii] = variables[ii].token;
            }

            tokens[variables.Length] = "CurVal";


            call.VectorEvaluate += new SimControlMethods.Calibrate.VectorEvaluator(evaluateGeneralCalibration);

            parser = new SimpleParser(tokens);

            cancel = !call.Run();

            if (!cancel)
            {
                // update scenario parameter values to optimim
                parser.updateValues(val);

                parmDex = 0;
                foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
                {
                    parser.updateValue("CurVal", origParmValues[parmDex]);
                    parmDex++;
                    param.aValue = parser.ParseEquation(param.expression);


                    if (call.ApplyParameters)
                    {
                        simDb.ApplySimulationlParameter(param);
                    }
                }

                simDb.Update(); 
            }

            if (cancel)
                return false;

            return true;
        }

        private bool runOptimization(CalibrationControl control)
        {
            if (currentSimulation.Getscenario_variableRows().Length == 0)
                return false;

            bool cancel = false;


            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

            // save current parm values
            origParmValues = new double[parameters.Length];

            int parmDex;
            for (parmDex = 0; parmDex < origParmValues.Length; ++parmDex)
            {
                origParmValues[parmDex] = parameters[parmDex].aValue;
            }


            Optimize opt = new Optimize(control, variables.Length);

            val = opt.VariableValue;

            string[] tokens = new string[variables.Length + 1];
            double[] min = new double[variables.Length];
            double[] max = new double[variables.Length];

            for (int ii = 0; ii < variables.Length; ii++)
            {
                tokens[ii] = variables[ii].token;
                min[ii] = variables[ii].min;
                max[ii] = variables[ii].max;
            }

            tokens[variables.Length] = "CurVal";

            opt.VariableMin = min;
            opt.VariableMax = max;


            opt.Evaluate += new SimControlMethods.Optimize.MetricEvaluator(evaluateOptimizer);

            parser = new SimpleParser(tokens);

            cancel = !opt.Run();

            // update scenario parameter values to optimim
            parser.updateValues(val);

            parmDex = 0;
            foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
            {
                parser.updateValue("CurVal", origParmValues[parmDex]);
                parmDex++;
                param.aValue = parser.ParseEquation(param.expression);

                if (opt.ApplyParameters)
                {
                    simDb.ApplySimulationlParameter(param);
                }
            }


            if (cancel)
                return false;

            return true;
        }
    

        #endregion
        private void  runSim(out bool cancel)
        {
            cancel = false;

            // clear results before starting next run
            if (clearAllResults)
            {
                simDb.ClearAllResults();
            }
            
            int run_id = currentRun.run_id;

            string connect_file = engineConnectFileFromConnection(msConnect.Connection);
            simProcess.StartInfo.Arguments = "-f " + connect_file + " -s " + run_id;
			try 
			{
				simProcess.Start();
			}
			catch (System.Exception oops)
			{
				string what = oops.Message;

				updateUI(out cancel);

                cancel = true;

                return;
			}

			simProcess.WaitForExit();


			// calculate precalculated values
			preCalcValues();

          
            if (currentRun.simulationRow.delete_std_results == true)
            {
                simDb.ClearResults(currentRun.run_id);
            }
		}

		private void update(out bool cancel)
		{
            try
            {
                simDb.Update();
            }
            catch (Exception)
            {
                cancel = true;
                return;
            }

            updateUI(out cancel);
        
		}

		private void updateUI(out bool cancel)
		{
			cancel = false;

			if (SimUpdated != null)
			{
				SimUpdated(this, cancelSim, currentRun.run_id);

				if (cancelSim.Cancel)
					cancel = true;
			}
		}

	


	
		private bool metricMan_UpdateProgress(string status, double percentComplete)
		{
			currentRun.current_status = status;

			bool cancel = false;
			update(out cancel);

			return !cancel;
		}

		private void preCalcValues()
		{
			if (currentRun == null)
				return;

            refreshCurrentRun();

			string orig = "error";

			if (currentRun.current_status != null)
			{
				orig = currentRun.current_status;
			}

			currentRun.current_status = "Computing Metrics";

			bool cancel = false;
			update(out cancel);

			if (cancel)
				return;

            MetricMan metricMan = new MetricMan( simDb );

			metricMan.UpdateProgress +=new MrktSimDb.Metrics.MetricMan.Progress(metricMan_UpdateProgress);

			metricMan.Compute(currentRun.run_id);

			currentRun.current_status = orig.Replace("done", "Sim Processed");

			update(out cancel);
        }

        #region evaluators

        private int evaluateSimpleCalibrationRun(out bool cancel)
        {
            // create run
            currentRun = simDb.CreateRun(currentSimulation);

            update(out cancel);

            runSim(out cancel);

            return currentRun.run_id;
        }


        private int evaluateMetricCalibrationRun(Value metric, out bool cancel, out double value)
        {
            // create run
            currentRun = simDb.CreateRun(currentSimulation);

            update(out cancel);

            runSim(out cancel);

            MetricMan metricMan = new MetricMan(simDb);

            metric.Run = currentRun.run_id;

            value = metricMan.Evaluate(metric);

            return currentRun.run_id;
        }

        // each parameter has a unique product
        private double[] evaluateGeneralCalibration(Value metric, out bool cancel)
        {
            // create run
            currentRun = simDb.CreateRun(currentSimulation);

            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

            double[] callVal = new double[variables.Length];



            // write variable values to database
            for (int ii = 0; ii < variables.Length; ii++)
            {
                simDb.CreateSimVariableValue(currentRun, variables[ii], val[ii]);
            }

            // update scenario parameter values
            parser.updateValues(val);

            try
            {
                int parmDex = 0;
                foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
                {
                    try
                    {
                        parser.updateValue( "CurVal", origParmValues[parmDex] );
                        parmDex++;
                        param.aValue = parser.ParseEquation( param.expression );
                    }
                    catch( Exception )
                    {
                        cancel = true;
                        return callVal;
                    }
                }
            }
            catch (Exception)
            {
                cancel = true;
                return callVal;
            }


            update(out cancel);

            if (cancel)
                return callVal;


            runSim(out cancel);

            MetricMan metricMan = new MetricMan(simDb);
            //
            //			Metric[] metrics = new Metric[] {MetricMan.GetMetric("CalSummaryByProd")};
            //
            //			metricMan.Compute(currentSim.run_id, metrics, this.currentSimulation.start_date, this.currentSimulation.end_date);

            metric.Run = currentRun.run_id;

            // loop over variables and extract value
            for (int ii = 0; ii < variables.Length; ++ii)
            {
                metric.Product = variables[ii].product_id;

                callVal[ii] = metricMan.Evaluate(metric);
            }


            return callVal;
        }
        /// <summary>
        /// used by generic calibration optmization
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        private double evaluateOptimizer(Value metric, out bool cancel)
        {
            MrktSimDBSchema.scenario_variableRow[] variables = currentSimulation.Getscenario_variableRows();
            MrktSimDBSchema.scenario_parameterRow[] parameters = currentSimulation.Getscenario_parameterRows();

            // create run
            currentRun = simDb.CreateRun(currentSimulation);

            // write variable values to database
            for (int ii = 0; ii < variables.Length; ii++)
            {
                simDb.CreateSimVariableValue(currentRun, variables[ii], val[ii]);
            }

            // update scenario parameter values
            parser.updateValues(val);

            int parmDex = 0;
            foreach (MrktSimDBSchema.scenario_parameterRow param in parameters)
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

            MetricMan metricMan = new MetricMan(simDb);
         
            metric.Run = currentRun.run_id;
   
            rval = metricMan.Evaluate(metric);

            return rval;
        }
        #endregion
    }
}
