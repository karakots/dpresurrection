using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ZedGraph;

namespace MrktSimClient.Controls.Graphing
{
	/// <summary>
	/// Summary description for Grapher.
	/// </summary>
	public class PlotControl : System.Windows.Forms.UserControl
	{	
		// call back when select button is hit
		public delegate void SelectMe(PlotControl me);
		public event SelectMe PlotSelected;
		public delegate void SelectedCurve(int index);
		public event SelectedCurve CurveSelected;

        private bool timeSeries = true;
		private System.Windows.Forms.ContextMenu optionsMenu;
		private System.Windows.Forms.MenuItem hideLegendItem;
		public bool TimeSeries
		{
			set
			{
				timeSeries = value;
				if (value)
				{
					zedGraphControl.GraphPane.XAxis.Type = AxisType.Date;
				}
				else
				{
					zedGraphControl.GraphPane.XAxis.Type = AxisType.Linear;
					zedGraphControl.GraphPane.XAxis.Title = "";
				}
			}
			get
			{
				return timeSeries;
			}
		}

		private bool scatterPlot = false;
		public bool ScatterPlot
		{
			set
			{
				scatterPlot = value;
			}
			get
			{
				return scatterPlot;
			}
		}
		public ZedGraph.CurveList Curves
		{
			get
			{
				return zedGraphControl.GraphPane.CurveList;
			}
		}
		public DataCurve Data
		{
			set
			{
				PointPairList ptList = new PointPairList();
				for(int ii = 0; ii < value.X.Length; ++ii)
				{
					string ptLabel;
					if (TimeSeries)
					{
						DateTime date = (new XDate(value.X[ii])).DateTime;
						ptLabel = value.Label +  " ( " + 
							date.ToShortDateString() +
							", " + value.Y[ii].ToString("F") + 
							" )";
					}
					else
					{
						ptLabel = value.Label +  " ( " + 
							value.X[ii].ToString("F") +
							", " + value.Y[ii].ToString("F") + 
							" )";
					}
					ptList.Add(new PointPair(value.X[ii], value.Y[ii], ptLabel));
				}

				LineItem myCurve;
				if (ScatterPlot)
				{
					myCurve = zedGraphControl.GraphPane.AddCurve(value.Label,  ptList, value.Color, ZedGraph.SymbolType.Star);
					myCurve.Line.IsVisible = false;
					myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
				}
				else
				{
					myCurve = zedGraphControl.GraphPane.AddCurve(value.Label,  ptList, value.Color, ZedGraph.SymbolType.None);
					
					if (value.Units == "%")
					{
						myCurve.IsY2Axis = true;

						myCurve.Label += " >";
						zedGraphControl.GraphPane.Y2Axis.IsVisible = true;
					}
				}

				if (value.Units != null && value.Units.Length > 0 && value.Units != "%")
					YAxis = value.Units;
			}
		}

      

        //jimj
        public void ClearAllGraphs() {
            zedGraphControl.GraphPane.CurveList.Clear();
        }

		public void PushToBack(LineItem crv)
		{
			zedGraphControl.GraphPane.CurveList.Remove(crv);
			zedGraphControl.GraphPane.CurveList.Add(crv);
		}
		public DateTime Start
		{
			set
			{
				zedGraphControl.GraphPane.XAxis.Min = new XDate(value);
			}
			get
			{
				XDate date = new XDate(zedGraphControl.GraphPane.XAxis.Min);
				
				return date.DateTime;
			}
		}
		public double MinX
		{
			set
			{
				zedGraphControl.GraphPane.XAxis.Min = value;
			}
			get
			{
				return zedGraphControl.GraphPane.XAxis.Min;
			}
		}

		public double MaxX
		{
			set
			{
				zedGraphControl.GraphPane.XAxis.Max = value;
			}
			get
			{
				return zedGraphControl.GraphPane.XAxis.Max;
			}
		}
		public DateTime End
		{
			set
			{
				zedGraphControl.GraphPane.XAxis.Max = new XDate(value);

                zedGraphControl.GraphPane.XAxis.Step = 30;

			}
			get
			{
				XDate date = new XDate(zedGraphControl.GraphPane.XAxis.Max);
				return date.DateTime;
			}
		}
		public double Min
		{
			set
			{
				zedGraphControl.GraphPane.YAxis.Min = value;
			}
			get
			{
				return zedGraphControl.GraphPane.YAxis.Min;
			}
		}

		public double Max
		{
			set
			{
				zedGraphControl.GraphPane.YAxis.Max = value;

               // zedGraphControl.GraphPane.YAxis.Step = GenerateStepValue( value, 4 );          //jimj
            }
			get
			{
				return zedGraphControl.GraphPane.YAxis.Max;
			}
		}
		public double PercentMax
		{
			set
			{				
				zedGraphControl.GraphPane.Y2Axis.Max = value;
			}
			get
			{
				return zedGraphControl.GraphPane.Y2Axis.Max;
			}
		}
		public double PercentMin
		{
			set
			{				
				zedGraphControl.GraphPane.Y2Axis.Min = value;
			}
			get
			{
				return zedGraphControl.GraphPane.Y2Axis.Min;
			}
		}

		public void AutoScaleAxis()
		{
			zedGraphControl.GraphPane.YAxis.MaxAuto = true;
		
			zedGraphControl.GraphPane.Y2Axis.MaxAuto = true;
		}
		public Bitmap Image
		{
			get
			{
				return zedGraphControl.GraphPane.Image;
			}
		}
		public string Title
		{
			set
			{
				zedGraphControl.GraphPane.Title = value;
			}
			get
			{
				return zedGraphControl.GraphPane.Title;
			}
		}
		public string YAxis
		{
			set
			{
				zedGraphControl.GraphPane.YAxis.Title = value;
			}
			get
			{
				return zedGraphControl.GraphPane.YAxis.Title;
			}
		}
		public string XAxis
		{
			set
			{
				zedGraphControl.GraphPane.XAxis.Title = value;
			}

			get
			{
				return zedGraphControl.GraphPane.XAxis.Title;
			}
		}

		public void DataChanged()
		{
			// check if we should have a Y-AXIS or a Y2 axis even

			bool Y2AxisExists = false;
			bool YAxisExists = false;

			foreach(ZedGraph.CurveItem crv in zedGraphControl.GraphPane.CurveList)
			{
				if (crv.IsY2Axis)
				{
					Y2AxisExists = true;
				}
				else
				{
					YAxisExists = true;
				}

				if (YAxisExists && Y2AxisExists)
				{
					break;
				}
			}

			if (Y2AxisExists && !YAxisExists)
			{
				zedGraphControl.GraphPane.YAxis.IsVisible = false;
				zedGraphControl.GraphPane.YAxis.MaxAuto = false;
				zedGraphControl.GraphPane.YAxis.Max = 100;
				zedGraphControl.GraphPane.YAxis.Min = 0;
			}

			
			if (!Y2AxisExists && YAxisExists)
			{
				zedGraphControl.GraphPane.Y2Axis.IsVisible = false;
				zedGraphControl.GraphPane.Y2Axis.MaxAuto = false;
				zedGraphControl.GraphPane.Y2Axis.Min = 0;
			}

			zedGraphControl.AxisChange();
			zedGraphControl.Invalidate();
		}

		private ZedGraph.ZedGraphControl zedGraphControl;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public PlotControl()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			zedGraphControl.IsShowPointValues = true;
			zedGraphControl.GraphPane.XAxis.Type = AxisType.Date;
			zedGraphControl.GraphPane.XAxis.Title = "Date";
			zedGraphControl.GraphPane.FontSpec.Size = 24;
			zedGraphControl.GraphPane.Legend.IsVisible = false;
			zedGraphControl.GraphPane.YAxis.IsVisible = true;
			zedGraphControl.GraphPane.YAxis.IsShowGrid = true;
			zedGraphControl.GraphPane.YAxis.IsShowMinorGrid = true;
            zedGraphControl.GraphPane.YAxis.MaxAuto = true;
            zedGraphControl.GraphPane.YAxis.MinAuto = true;
            zedGraphControl.GraphPane.YAxis.StepAuto = true;

			zedGraphControl.GraphPane.Y2Axis.IsVisible = false;
			zedGraphControl.GraphPane.Y2Axis.IsShowGrid = true;
			zedGraphControl.GraphPane.Y2Axis.IsShowMinorGrid = true;
			zedGraphControl.GraphPane.Y2Axis.Type = ZedGraph.AxisType.Linear;
			zedGraphControl.GraphPane.Y2Axis.IsShowTitle = true;
			zedGraphControl.GraphPane.Y2Axis.Title = "Percent";
			zedGraphControl.GraphPane.Y2Axis.Max = 100;
			zedGraphControl.GraphPane.Y2Axis.Min = 0;
			zedGraphControl.GraphPane.Y2Axis.MaxAuto = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.optionsMenu = new System.Windows.Forms.ContextMenu();
            this.hideLegendItem = new System.Windows.Forms.MenuItem();
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // optionsMenu
            // 
            this.optionsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.hideLegendItem});
            // 
            // hideLegendItem
            // 
            this.hideLegendItem.Index = 0;
            this.hideLegendItem.Text = "Hide Legend";
            this.hideLegendItem.Click += new System.EventHandler(this.hideLegendItem_Click);
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.ContextMenu = this.optionsMenu;
            this.zedGraphControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.zedGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl.IsShowPointValues = true;
            this.zedGraphControl.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.PointValueFormat = "G";
            this.zedGraphControl.Size = new System.Drawing.Size(336, 198);
            this.zedGraphControl.TabIndex = 0;
            this.zedGraphControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zedGraphControl_MouseDown);
            // 
            // PlotControl
            // 
            this.Controls.Add(this.zedGraphControl);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "PlotControl";
            this.Size = new System.Drawing.Size(336, 198);
            this.ResumeLayout(false);

		}
		#endregion

		#region private fields

		double[] crvScale = null;

		#endregion

		private const int sclLngth = 7;
		public void Write(System.IO.StreamWriter writer)
		{
			// write out header
			char comma = ',';
			string header = "Date" + comma;
			int jj = 0;
			foreach(CurveItem crv in zedGraphControl.GraphPane.CurveList )
			{
				string crvName = crv.Label.Replace(",","-");
				if (this.crvScale != null && this.crvScale[jj] != 1)
					crvName = crvName.Remove(crvName.Length - sclLngth, sclLngth);
				header += crvName + comma;
				jj++;
			}
			char[] trim = {comma};
			header.TrimEnd(trim);
			writer.WriteLine(header);
			XDate start = zedGraphControl.GraphPane.XAxis.Min;
			XDate end = zedGraphControl.GraphPane.XAxis.Max;
			int[] index = new int[zedGraphControl.GraphPane.CurveList.Count];
			int ii;
			// initialize - probably done automatically - but what the hey
			for(ii = 0; ii < index.Length; ii++)
				index[ii] = 0;
			// trim the beginning of each curve
			for(ii = 0; ii < index.Length; ii++)
			{
				CurveItem crv = zedGraphControl.GraphPane.CurveList[ii];
				while (index[ii] < crv.Points.Count &&
					crv.Points[index[ii]].X < start )
				{
					index[ii]++;
				}
			}
			// we keep increasing day by 1 looking for new data
			XDate day = start;

			// check for all data written out
			bool done = false;
			while (!done && day <= end)
			{
				// advance the day until we get to new data
				bool noNewData = true;
				while (noNewData && day < end)
				{
					for(ii = 0; noNewData && ii < index.Length; ii++)
					{
						CurveItem crv = zedGraphControl.GraphPane.CurveList[ii];
						if (index[ii] < crv.Points.Count)
						{
							PointPair pt = crv.Points[index[ii]];
							if ( pt.X <= day )
							{
								noNewData = false;
							}
						}
					}
					if (noNewData)
						day.AddDays(1.0);	
				}
				string values = day.DateTime.ToShortDateString() + comma;
				// check if there is still data to write
				done = true;
				for(ii = 0; ii < index.Length; ii++)
				{
					CurveItem crv = zedGraphControl.GraphPane.CurveList[ii];
					if (index[ii] < crv.Points.Count)
					{
						// still data to write
						done = false;
						PointPair pt = crv.Points[index[ii]];
						double yVal = pt.Y;
						if (crvScale != null)
						{
							yVal *= crvScale[ii];
						}
						values += yVal.ToString();
						if ( pt.X <= day )
						{
							index[ii]++;
						}
					}
					values += comma;
				}
				if (!done)
				{
					values.TrimEnd(trim);
					writer.WriteLine(values);
				}
				day.AddDays(1.0);
			}
		}

		public void Scale()
		{
			// scale all curves
			// initialize
			if (crvScale == null)
				crvScale = new double[zedGraphControl.GraphPane.CurveList.Count];
			for(int index = 0; index < zedGraphControl.GraphPane.CurveList.Count; ++index)
			{
				crvScale[index] = 1.0;
			}

			// find max
			double max = 0.0;
			foreach(ZedGraph.CurveItem crv in zedGraphControl.GraphPane.CurveList)
			{
				if (!crv.IsY2Axis)
				{

					double xMin = 0.0;
					double xMax = 0.0;
					double yMin = 0.0;
					double curMax = 0.0;
					crv.GetRange(ref xMin, ref  xMax,ref  yMin, ref  curMax,
						true, zedGraphControl.GraphPane) ;
				
					if (curMax > max)
						max = curMax;
				}
			}
			if (max > 1)
			{
				
				double logMax = Math.Log10(max);
				double expMag = Math.Ceiling(logMax);
				int index = 0;
				foreach(ZedGraph.CurveItem crv in zedGraphControl.GraphPane.CurveList)
				{
					if (!crv.IsY2Axis)
					{

						double xMin = 0.0;
						double xMax = 0.0;
						double yMin = 0.0;
						double curMax = 0.0;
						crv.GetRange(ref xMin, ref  xMax,ref  yMin, ref  curMax,
							true, zedGraphControl.GraphPane) ;
						double curLogMax = Math.Log10(curMax);
						double curExpMag = Math.Ceiling(curLogMax);
						// below 10% is ok but below 100% is bad
						if (curLogMax + 1 < logMax)
						{
							double power = Math.Floor(logMax - curLogMax);
							if (power > 0)
							{
								// scale curve by 10^power
								double scale = Math.Pow(10, power);
								ZedGraph.PointPairList points = crv.Points;

								ZedGraph.PointPairList scaledPoints = new PointPairList();

								for(int ii = 0; ii < crv.NPts; ++ii)
									scaledPoints.Add(new PointPair(crv[ii].X, scale * crv[ii].Y, crv[ii].Tag.ToString()));
								crv.Clear();
								foreach(PointPair pt in scaledPoints)
								{
									crv.AddPoint(pt);
								}
								crv.Label += " (10^" + power + ")";
								crvScale[index] = Math.Pow(10, -power);
							}
						}
					}
					index++;
				}
			}

			scaleY2Axis();
		}
		/// <summary>
		/// return the power of 10 that the crv is scaled by
		/// the negative sign is because crvScale is the inverwse transform
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public void ScaleCrv(int index, int power)
		{
			double scale = Math.Pow(10, power);
			ZedGraph.CurveItem crv = zedGraphControl.GraphPane.CurveList[index];
			ZedGraph.PointPairList points = crv.Points;
			ZedGraph.PointPairList scaledPoints = new PointPairList();
			for(int ii = 0; ii < crv.NPts; ++ii)
				scaledPoints.Add(new PointPair(crv[ii].X, scale * crvScale[index] * crv[ii].Y, crv[ii].Tag.ToString()));
			crv.Clear();
			foreach(PointPair pt in scaledPoints)
			{
				crv.AddPoint(pt);
			}
			if (crv.IsY2Axis)
			{
				// remove >
				crv.Label = crv.Label.Substring(0, crv.Label.Length - 1);
			}
			int hatLoc = crv.Label.LastIndexOf("^");

			if (hatLoc > 3)
			{
				crv.Label = crv.Label.Substring(0,hatLoc - 4);
			}

			if (power > 0)
				crv.Label += " (10^" + power + ")";

			if (crv.IsY2Axis)
			{
				// add >
				crv.Label += " >";
			}
			crvScale[index] = Math.Pow(10, -power);
			this.DataChanged();
		}
		public int CrvScale(int index)
		{
			return (int) -Math.Floor(Math.Log10(crvScale[index]));
		}
		private void scaleY2Axis()
		{
			// scale all curves to 100
			int index = 0;
			foreach(ZedGraph.CurveItem crv in zedGraphControl.GraphPane.CurveList)
			{
				if (crv.IsY2Axis)
				{
					double xMin = 0.0;
					double xMax = 0.0;
					double yMin = 0.0;
					double curMax = 0.0;
					crv.GetRange(ref xMin, ref  xMax,ref  yMin, ref  curMax,
						true, zedGraphControl.GraphPane) ;
					double curLogMax = Math.Log10(curMax);
					// below 1% we scale up by a factor of 10
					if (curLogMax < 0)
					{
						// scale curve by 10^power
						double scale = 10;
						ZedGraph.PointPairList points = crv.Points;
						ZedGraph.PointPairList scaledPoints = new PointPairList();
						for(int ii = 0; ii < crv.NPts; ++ii)
							scaledPoints.Add(new PointPair(crv[ii].X, scale * crv[ii].Y, crv[ii].Tag.ToString()));
						crv.Clear();
						foreach(PointPair pt in scaledPoints)
						{
							crv.AddPoint(pt);
						}
						crv.Label = crv.Label.Substring(0, crv.Label.Length - 1) + " (10^1) >";
						crvScale[index] = 0.1;
					}
				}
				index++;
			}
		}

		public void RemoveScaling()
		{

			// no scaling
			if (crvScale == null)
				return;

			int index = 0;
			foreach(ZedGraph.CurveItem crv in zedGraphControl.GraphPane.CurveList)
			{
				if (crvScale[index] != 1.0)
				{
					ZedGraph.PointPairList points = crv.Points;
					ZedGraph.PointPairList scaledPoints = new PointPairList();
					for(int ii = 0; ii < crv.NPts; ++ii)
					{
						scaledPoints.Add(new PointPair(crv[ii].X, crvScale[index]  * crv[ii].Y, crv[ii].Tag.ToString()));
					}

					crv.Clear();
					foreach(PointPair pt in scaledPoints)
					{
						crv.AddPoint(pt);
					}
					int hatLoc = crv.Label.LastIndexOf("^");

					if (hatLoc > 3)
					{
						crv.Label = crv.Label.Substring(0,hatLoc - 3);
					}

					if (crv.IsY2Axis)
					{
						crv.Label += " >";
					}

					crvScale[index] = 1.0;
				}
				
				index++;
			}
		}

		public void hideLegendItem_Click(object sender, System.EventArgs e)
		{
			if (this.zedGraphControl.GraphPane.Legend.IsVisible == true)
			{
				this.zedGraphControl.GraphPane.Legend.IsVisible = false;
				hideLegendItem.Checked = true;
				this.DataChanged();
			}
			else
			{
				this.zedGraphControl.GraphPane.Legend.IsVisible = true;
				hideLegendItem.Checked = false;
			
				this.DataChanged();
			}
		}
		private void zedGraphControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// find nearest object and if it is a curve
			// let parent know that a curve was selected
			System.Drawing.PointF pt = new PointF(e.X, e.Y);
			Graphics g = Graphics.FromHwnd(this.Handle);
			object nearestObj;
			int index;
			zedGraphControl.GraphPane.FindNearestObject(pt, g, out nearestObj, out index);
			if (nearestObj != null && nearestObj.GetType() == typeof(ZedGraph.LineItem))
			{
				// compute index of object
				index = 0;
				foreach (LineItem crv in zedGraphControl.GraphPane.CurveList)
				{
					if (crv == nearestObj)
					{
						if (CurveSelected != null)
							CurveSelected(index);

						break;
					}
					index++;
				}
			}
		}

        // jimj
        private double GenerateStepValue( double range, int desiredNumSteps ) {
            double logMax = Math.Log10( range );
            double startingPower = Math.Floor( logMax ) - 1;
            double startStep = Math.Pow( 10.0, startingPower );

            double retval = startStep;
            if( (range / (startStep * 2)) < desiredNumSteps ) {
                retval = startStep;
            }
            else if( (range / (startStep * 5)) < desiredNumSteps ) {
                retval = startStep * 2;
            }
            else if( (range / (startStep * 10)) < desiredNumSteps ) {
                retval = startStep * 5;
            }
            else {
                retval = startStep * 10;
            }
            return retval;
        }
	}
}
