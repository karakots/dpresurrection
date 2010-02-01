using System;
using System.Drawing;
using System.Data.OleDb;
using ZedGraph;

namespace MrktSimClient.Controls.Graphing
{
	/// <summary>
	/// Wrapper around CurveItem -- copied from Results project
	/// </summary>
	public class DataCurve
	{
		private int index = 0;
		private Color color;
		public Color Color
		{
			get
			{
				return color;
			}

			set
			{
				color = value;
			}
		}

		private string text = null;
		private string units = null;
		public string Units
		{
			set
			{
				units = value;
			}

			get
			{
				return units;
			}
		}

		public TimeSpan Span
		{
			get
			{
				TimeSpan span = new TimeSpan(0,0,0,0,0);
				if(x.Length == 1 || x.Length == 0 || x[1] <= x[0])
				{
					return span;
				}
				else
				{
					span = XDate.XLDateToDateTime(x[1]) - XDate.XLDateToDateTime(x[0]);
					return span;
				}
			}
		}

		public TimeSpan GetSpan(int index)
		{
			TimeSpan span = new TimeSpan(0,0,0,0,0);
			if(x.Length == 1 || x.Length == 0 || x[1] <= x[0])
			{
				return span;
			}
			if(index == 0)
			{
				span = XDate.XLDateToDateTime(Math.Floor(x[1])) - XDate.XLDateToDateTime(Math.Floor(x[0]));
			}
			else
			{
				span = XDate.XLDateToDateTime(Math.Floor(x[index])) - XDate.XLDateToDateTime(Math.Floor(x[index-1]));
			}

			if(span.Days < 1 && index != 0)
			{
				span = XDate.XLDateToDateTime(Math.Floor(x[1])) - XDate.XLDateToDateTime(Math.Floor(x[0]));
			}

			return span;
		}

		public string Label
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		public double[] X
		{
			get
			{
				return x;
			}
		}

		public double[] Y
		{
			get
			{
				return y;
			}
		}

		private double[] x = null;
		private double[] y = null;

		public void Add(DateTime day, double val)
		{
			Add(XDate.DateTimeToXLDate(day), val);
		}

		public void Add(double xVal, double yVal)
		{
			if (x == null)
				return;

			if (index == x.Length)
				return;

			x[index] = xVal;
			y[index] = yVal;

			index++;
		}

		public double Eval(DateTime day, bool average)
		{
			if (index == x.Length)
			{
				index = 0;
			}

			DateTime earliest = XDate.XLDateToDateTime(x[0]).AddDays(-Span.Days);

			double val = XDate.DateTimeToXLDate(day);

			if(val > x[x.Length - 1] || day.CompareTo(earliest) < 0)
			{
				return 0;
			}

			if( val == x[x.Length - 1] )
			{
				return y[y.Length - 1]/((double)GetSpan(x.Length - 1).Days);
			}

			if( val <= x[0])
			{
				return y[0]/((double)GetSpan(0).Days);
			}

			while (x[index] < val && index < x.Length - 1)
			{
				index++;
			}

			while (index > 0 && x[index - 1] >= val)
			{
				index--;
			}

			//Calculate units per day for the current span
			return y[index]/((double)GetSpan(index).Days);
		}

		public DataCurve(int numVals)
		{
			x = new double[numVals];
			y = new double[numVals];
			index = 0;
		}

		#region Transforms
		// transform curve by averaging over number of items
		public DataCurve Average(DateTime start, DateTime end, TimeSpan numDays, bool average)
		{
			if (numDays.Days == 0)
			{
				return null;
			}


			// how many resulting days are there?

			// aveLength*numItems < x.length + 1 (see where we perform assignment in loop)

			// if length is 21 and numItems is 7 (one week)
			// then aveLength is 3
			// and (index + 1)*numItems - 1 takes on values
			// 6, 13, 20
			TimeSpan totalSpan = end - start;
			int aveLength = (int) Math.Floor((double) totalSpan.Days/numDays.Days);

			double scale = 1.0;

			if (average)
			{
				scale = 1.0/numDays.Days;
			}

			DataCurve aveCrv = new DataCurve(aveLength);
			for(int index = 0; index < aveLength; index++)
			{
				double ave = 0.0;
				for(DateTime curDay = start.AddDays(index*numDays.Days); 
					curDay.CompareTo(start.AddDays((index+1)*numDays.Days)) < 0; 
					curDay = curDay.AddDays(1))
				{
					ave += Eval(curDay, average);
				}

				ave *= scale;

				// this needs to be the last day the average is taken over
				// see calculation above
				double val = XDate.DateTimeToXLDate(start.AddDays((index+1)*numDays.Days-1));

				aveCrv.Add(val, ave);
			}

			aveCrv.Units = Units;
			aveCrv.Label = Label;

			return aveCrv;
		}

		#endregion

	}
}
