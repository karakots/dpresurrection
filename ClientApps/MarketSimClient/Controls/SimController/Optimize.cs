using System;
using System.Collections;
using MrktSimDb;
// using Common.Utilities;
using EquationParser;

namespace SimControlMethods
{
	/// <summary>
	/// Summary description for Optimize.
	/// </summary>
	public class Optimize : Calibrate
	{
        public event MetricEvaluator Evaluate;

		// private static double eps = 0.000000000001; // machine epsilon
		private static double smallFloat = 0.00001;  // our local tolerance


        private double scale = 0.2;
        private int numEvals = 1;

		double[] var;
		double[] min;
		double[] max;
		double[] grad;
		double[] delta;
		double[] curMin;

		double curVal = 0.0;

		double curMinVal =  10000000.0;
		double curMaxVal = -10000000.0;

		ArrayList valueHistory;

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

		public double[] VariableMax
		{
			set
			{
				for(int ii = 0; ii < value.Length; ++ii)
					max[ii] = value[ii];
			}
		}

		public double[] VariableMin
		{
			set
			{
				for(int ii = 0; ii < value.Length; ++ii)
					min[ii] = value[ii];
			}
		}

		public Optimize(CalibrationControl control, int numVars) : base(control)
		{
			var = new double[numVars];

			min = new double[numVars];
			max = new double[numVars];

			grad = new double[numVars];

			delta = new double[numVars];

			curMin = new double[numVars];

			valueHistory = new ArrayList();
		}

		public override bool Run()
		{
			// if nothing to evaluate then just return
			if (Evaluate == null)
				return false;

			// initial seeding
			// first evaluate at grid of points
		

			// start var at min values

			// set up new min and max for loop
			// evaluate at new min first
			for( int ii = 0; ii < var.Length; ++ii)
			{
				delta[ii] = scale * (max[ii] - min[ii]);

				min[ii] += delta[ii];
				max[ii] -= delta[ii];

				var[ii] = min[ii];

				curMin[ii] = var[ii];
			}

			// loop incrementing seed values appropriately
			bool doneSeeding = false;
			bool cancel = false;

			while(!doneSeeding)
			{
				// perform first evaluation
				if (Evaluate != null)
					curVal = Evaluate(metric, out cancel);

				if (curVal < curMinVal)
				{
					curMinVal = curVal;

					for(int ii = 0; ii < var.Length; ++ii)
						curMin[ii] = var[ii];
				}

				if (curVal > curMaxVal)
					curMaxVal = curVal;

				// increment values
				for( int ii = 0; ii < var.Length; ii++)
				{
					// increment this value
					var[ii] += delta[ii];

					if (var[ii] > max[ii])
					{
						var[ii] = min[ii];
						doneSeeding = true;
					}
					else		
					{							// yes virginia, this really works
						doneSeeding = false;	// if we are here then we will continue
						break;					// if we never reach here then 
					}							// all vars have been incremented past the max
				}
			}

			// we restrict min and max to desired quadrant
			// current variable value is set at current min value
			curVal = curMinVal;
			for(int ii = 0; ii < var.Length; ++ii)
			{
				min[ii] = curMin[ii] - delta[ii];
				max[ii] = curMin[ii] + delta[ii];
				
				var[ii] =  curMin[ii];
			}

			// not worth the trip
			if ( (curMaxVal - curMinVal) < smallFloat)
				return false;


			// do while error is larger then what has been specified
			// or until we have performed too many iteration
			
			while(!cancel)
			{
				// compute  gradient around current value
				// and conpute step
				if (!computeStep(out cancel))
					break;

				// evaluate at new value
				if (Evaluate != null)
					curVal = Evaluate(metric, out cancel);

				valueHistory.Add(curVal);

				while (curVal > curMinVal)
				{
					// reset max and min if significantly larger
					if (curVal > curMinVal + tolerance)
					{
						for (int ii = 0; ii < var.Length; ++ii)
						{
							if (var[ii] > curMin[ii])
								max[ii] = var[ii];
							else if (var[ii] < curMin[ii])
								min[ii] = var[ii];
						}
					}

					// bisect until we are at min
					for (int ii = 0; ii < var.Length; ++ii)
						var[ii] = 0.5 *(var[ii] + curMin[ii]);

					// evaluate a new value
					if (Evaluate != null)
                        curVal = Evaluate(metric, out cancel);

					valueHistory.Add(curVal);

					if (checkIfDone(maxIters))
						break;
				}

				if (curVal < curMinVal)
				{
					// reset max and min if significantly smaller
					if (curVal < curMinVal - tolerance)
					{	
						for (int ii = 0; ii < var.Length; ++ii)
						{
							if (var[ii] > curMin[ii])
								min[ii] = curMin[ii];
							else if (var[ii] < curMin[ii])
								max[ii] = curMin[ii];						
						}
					}

					// reset cur min
					curMinVal = curVal;
					
					for( int ii = 0; ii < var.Length; ++ii)
						curMin[ii] = var[ii];
				}
				else
					break;	// I guess we are done optimizing we could not reduce any more on this line

				if (checkIfDone(maxIters))
					break;
			}

			//make sure var is current min
			for (int ii = 0; ii < var.Length; ++ii)
				var[ii] = curMin[ii];

			return !cancel;
		}

		private bool computeStep(out bool cancel)
		{
			bool stepForward = false;

			double[] x = new double[2*numEvals + 1];
			double[] y = new double[2*numEvals + 1];
			double[] acc = new double[var.Length];

		
			cancel = false;

			// step in all direction forward and back, evaluate a couple of times
			for(int ii = 0; !cancel && ii < var.Length; ++ii)
			{
				double step = this.stepSize * (max[ii] - min[ii]);
				
				x[0] = var[ii];
				y[0] = curVal;

				// stepping forward
				for(int jj = 0; jj < numEvals; ++jj)
				{
					var[ii] = var[ii] + (1 + jj) * step;

					if (var[ii] > max[ii])
						var[ii] = max[ii];

					x[1 + jj] = var[ii];

					if (Evaluate != null)
                        y[1 + jj] = Evaluate(metric, out cancel);
				
					// step back
					var[ii] = x[0];
				}

				// stepping backward
				for(int jj = 0; jj < numEvals; ++jj)
				{
					var[ii] = var[ii] - (1 + jj) * step;

					if (var[ii] < min[ii])
						var[ii] = min[ii];

					x[1 + numEvals + jj] = var[ii];

					if (Evaluate != null)
                        y[1 + numEvals + jj] = Evaluate(metric, out cancel);

					// step back
					var[ii] = x[0];
				}

				grad[ii] = computeSlope(x,y);
			}

			// compute next point
			for (int ii = 0; ii < var.Length; ++ii)
			{
				double nextStep = 0.0;

				if (Math.Abs(grad[ii]) > smallFloat)
				{
					nextStep = var[ii] - stepSize * grad[ii];

					stepForward = true;
				}

				// step toward max
				else if (grad[ii] < -smallFloat)
				{
					nextStep = 0.5 * (var[ii] + max[ii]);

					stepForward = true;
				}
				else if (grad[ii] > smallFloat)
				{
					nextStep = 0.5 * (var[ii] + min[ii]);

					stepForward = true;
				}

				if (nextStep > max[ii])
					nextStep  = 0.5 *(var[ii] + max[ii]);
				
				else if (nextStep < min[ii])
					nextStep  = 0.5 *(var[ii] + min[ii]);

				var[ii] = nextStep;
			}

			return stepForward;
		}

		private bool checkIfDone(int maxIters)
		{
			// see if we have exceeded max number of tries
			if (valueHistory.Count > maxIters)
				return true;

			bool done = true;
			for (int ii = 0; done && ii < var.Length; ++ii)
			{
				if (max[ii] - min[ii] > 0.1 * delta[ii])
					done = false;
			}

			if (done)
				return done;

			if (valueHistory.Count >=  3)
			{
				// check if the amount of change has stabalized
				double lastValue = (double) valueHistory[valueHistory.Count - 1];
				double prevValue = (double) valueHistory[valueHistory.Count - 2];
				double prev2Value = (double) valueHistory[valueHistory.Count - 3];

				double average = (lastValue + prevValue + prev2Value)/3;

				double sumDiff = Math.Abs(lastValue - average) + 
					Math.Abs(prevValue - average) + Math.Abs(prev2Value - average);

				if (sumDiff < 3 * tolerance)
					return true;
			}

			return false;
		}

		private bool gradIsZero()
		{
			double normSqr = 0;

			for(int ii = 0; ii < var.Length; ++ii)
				normSqr += grad[ii]*grad[ii];

			if (Math.Sqrt(normSqr) < smallFloat)
				return true;

			return false;
		}

		private double computeSlope(double[] x, double[] y )
		{
			// acc = 0.0;

			if (x.Length == 0)
				return 0;

			double scale = 1.0/x.Length;

			// normalize

			double aveX = 0;

			for (int ii = 0; ii < x.Length; ++ii)
				aveX += x[ii];

			aveX *= scale;

			double aveY = 0;

			for (int ii = 0; ii < x.Length; ++ii)
				aveY += y[ii];

			aveY *= scale;

			for (int ii = 0; ii < x.Length; ++ii)
			{
				x[ii] = (x[ii] - aveX)/stepSize;
				y[ii] = (y[ii] - aveY)/stepSize;
			}

			double covYX = 0.0;			
			double varXX = 0.0;

            //double varXXX = 0.0;
            //double varXXXX = 0.0;
            //double covYXX = 0.0;

			for (int ii = 0; ii < x.Length; ++ii)
			{
				covYX += y[ii] * x[ii];
				varXX += x[ii] * x[ii];

                //covYXX += y[ii] * x[ii] * x[ii];
                //varXXX += x[ii] * x[ii] * x[ii];
                //varXXXX += x[ii] * x[ii] * x[ii] * x[ii];
			}

			if (varXX < smallFloat)
				return 0;

			if (Math.Abs(covYX) < smallFloat)
				return 0;

			// compute acceleration
            //if ( varXXXX > eps)
            //{
            //    double skew =  varXXX/varXX;
            //    double delta =  covYXX - covYX * skew;
            //    if (delta > 0)
            //        acc = 2 * delta / varXXXX;
            //}
			
			return covYX/varXX;
		}
	}
}
