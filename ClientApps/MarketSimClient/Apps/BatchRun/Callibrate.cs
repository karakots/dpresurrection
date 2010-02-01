using System;
using MrktSimDb;

namespace BatchRun
{
	/// <summary>
	/// Summary description for Callibrate.
	/// </summary>
	public class Calibrate
	{
		public delegate double[] Evaluator(out bool cancel);
		public event Evaluator Evaluate;


		// when we step the percent of interval we step by
		// this number should be less then 0.25 otherwise we may step over a minimum point

		double[] var;

		// control variables
		double stepSize;
		int maxIters;
		double tolerance;

		public double[] VariableValue
		{
			set
			{
				for(int ii = 0; ii < value.Length; ++ii)
					var[ii] = value[ii];
			}

			get
			{
				return var;
			}
		}

		public Calibrate(string controlString, int numVars)
		{
			var = new double[numVars];

			// start at one
			for (int ii = 0; ii < numVars; ++ii)
				var[ii] = 1.0;

			CalibrationControl control = new CalibrationControl(controlString);

			stepSize = control.StepSize;
			tolerance = control.Tolerance;
			maxIters = control.MaxIters;
		}

		/// <summary>
		/// perform calibration
		/// </summary>
		public bool Run()
		{
			bool done = false;

			int numIters = 0;

			while (!done)
			{
				bool cancel = false;

				double[] excessShare = Evaluate(out cancel);

				if (cancel)
					return false;

				done = true;
				for(int ii = 0; ii < excessShare.Length; ++ii)
				{
					if (Math.Abs(excessShare[ii]) < tolerance)
						continue;

					done = false;

					// Note: stepSize may be negative if share is negatively correlated with vals
					var[ii] = var[ii] - stepSize * excessShare[ii];
				}

				++numIters;

				if (numIters > maxIters)
					done = true;
			}

			return true;
		}
	}
}
