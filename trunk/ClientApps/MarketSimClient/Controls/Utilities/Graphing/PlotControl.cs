using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ZedGraph;

namespace Utilities.Graphing
{
	/// <summary>
	/// Summary description for Grapher.
	/// </summary>
	public class PlotControl : System.Windows.Forms.UserControl
	{	
		// call back when select button is hit
		public delegate void SelectMe(PlotControl me);
		public event SelectMe PlotSelected;
        public delegate void SelectedCurve( int index );
        public event SelectedCurve CurveSelected;
        public delegate void SetLegendVisibility( bool show );
        public event SetLegendVisibility LegendVisibilitySet;

        private bool timeSeries = true;
        private System.Windows.Forms.Button send;
        public Button Send {
            set { send = value; }
            get { return send; }
        }

        private System.Windows.Forms.ContextMenu optionsMenu;

		private System.Windows.Forms.MenuItem hideLegendItem;
        public MenuItem HideLegendItem {
            set { hideLegendItem = value; }
            get { return hideLegendItem; }
        }

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
                if (value == null)
                {
                    return;
                }

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
                        // show the Y2 axis if we are displaying a percentage
						zedGraphControl.GraphPane.Y2Axis.IsVisible = true;
                        if( zedGraphControl.GraphPane.CurveList.Count == 0 ) {
                            zedGraphControl.GraphPane.YAxis.IsVisible = false;    // hide the other axis if this is the only/first curve
                        }
                    }
                    else {
                        // show the Y axis if we are displaying a non-percentage
                        zedGraphControl.GraphPane.YAxis.IsVisible = true;
                        if( zedGraphControl.GraphPane.CurveList.Count == 0 ) {
                            zedGraphControl.GraphPane.Y2Axis.IsVisible = false;      // hide the other axis if this is the only/first curve
                        }
                    }


				}

				if (value.Units != null && value.Units.Length > 0 && value.Units != "%")
					YAxis = value.Units;
			}
		}

        public void MakeUnique() {
            foreach( LineItem crv in zedGraphControl.GraphPane.CurveList ) {
                crv.MakeUnique();
            }
        }

      

        //JimJ section
        public void ClearAllGraphs() {
            if( zedGraphControl.GraphPane != null && zedGraphControl.GraphPane.CurveList != null ) {
                zedGraphControl.GraphPane.CurveList.Clear();
            }
        }

        public bool MoveForward( LineItem crv ) {
            int crvIndex = IndexForCurve( crv );
            if( crvIndex > 0 ) {
                ExchangeCurves( crvIndex, crvIndex - 1 );
                return true;
            }
            else {
                return false;
            }
        }

        public bool MoveBackward( LineItem crv ) {
            int crvIndex = IndexForCurve( crv );
            if( crvIndex < zedGraphControl.GraphPane.CurveList.Count - 1 ) {
                ExchangeCurves( crvIndex, crvIndex + 1 );
                return true;
            }
            else {
                return false;
            }
        }

        private void ExchangeCurves( int index1, int index2 ) {
            LineItem crv1 = (LineItem)zedGraphControl.GraphPane.CurveList[ index1 ];
            LineItem crv2 = (LineItem)zedGraphControl.GraphPane.CurveList[ index2 ];

            zedGraphControl.GraphPane.CurveList[ index1 ] = crv2;
            zedGraphControl.GraphPane.CurveList[ index2 ] = crv1;
        }

        public int IndexForCurve( LineItem crv ) {
            for( int indx = 0; indx < zedGraphControl.GraphPane.CurveList.Count; indx++ ) {
                if( zedGraphControl.GraphPane.CurveList[ indx ] == crv ) {
                    return indx;
                }
            }

            return -1;       //not found
        }
        //end JimJ section

        public void PushToBack( LineItem crv ) {
            int crvIndex = IndexForCurve( crv );
            double thisScale = this.crvScale[ crvIndex ];

            zedGraphControl.GraphPane.CurveList.Remove( crv );
            zedGraphControl.GraphPane.CurveList.Add( crv );

            double[] newScales = new double[ this.crvScale.Length ];
            int sclIndx = 0;
            for( int i = 0; i < this.crvScale.Length; i++ ){
                if( i != crvIndex ){
                    newScales[ sclIndx ] = this.crvScale[ i ]; 
                    sclIndx += 1;
                }
            }
            newScales[ sclIndx ] = thisScale;
            this.crvScale = newScales;

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

 //               zedGraphControl.GraphPane.XAxis.Step = 30;

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
            zedGraphControl.GraphPane.XAxis.MaxAuto = true;
            zedGraphControl.GraphPane.XAxis.MinAuto = true;
			zedGraphControl.GraphPane.YAxis.MaxAuto = true;
            zedGraphControl.GraphPane.YAxis.MinAuto = true;
		
			zedGraphControl.GraphPane.Y2Axis.MaxAuto = true;
            zedGraphControl.GraphPane.Y2Axis.MinAuto = true;
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
        public string Y2Axis
        {
            set
            {
                zedGraphControl.GraphPane.Y2Axis.Title = value;
            }
            get
            {
                return zedGraphControl.GraphPane.Y2Axis.Title;
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

            if( Y2AxisExists && !YAxisExists ) {
                zedGraphControl.GraphPane.YAxis.IsVisible = false;
                //zedGraphControl.GraphPane.YAxis.MaxAuto = false;
                //zedGraphControl.GraphPane.YAxis.Max = 100;
                //zedGraphControl.GraphPane.YAxis.Min = 0;
            }


            if( !Y2AxisExists && YAxisExists ) {
                zedGraphControl.GraphPane.Y2Axis.IsVisible = false;
                //zedGraphControl.GraphPane.Y2Axis.MaxAuto = false;
                //zedGraphControl.GraphPane.Y2Axis.Min = 0;
            }

			zedGraphControl.AxisChange();
			zedGraphControl.Invalidate();
		}

		private ZedGraph.ZedGraphControl zedGraphControl;

        public ZedGraph.ZedGraphControl ZedGraphControl {
            set { zedGraphControl = value; }
            get { return zedGraphControl; }
        }

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
            this.send = new System.Windows.Forms.Button();
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // optionsMenu
            // 
            this.optionsMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.hideLegendItem} );
            // 
            // hideLegendItem
            // 
            this.hideLegendItem.Index = 0;
            this.hideLegendItem.Text = "Hide Legend";
            this.hideLegendItem.Click += new System.EventHandler( this.hideLegendItem_Click );
            // 
            // send
            // 
            this.send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.send.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.send.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.send.Location = new System.Drawing.Point( 292, 3 );
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size( 34, 16 );
            this.send.TabIndex = 1;
            this.send.Text = ">>>";
            this.send.UseVisualStyleBackColor = false;
            this.send.Click += new System.EventHandler( this.send_Click );
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.ContextMenu = this.optionsMenu;
            this.zedGraphControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.zedGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl.IsShowPointValues = true;
            this.zedGraphControl.Location = new System.Drawing.Point( 0, 0 );
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.PointValueFormat = "G";
            this.zedGraphControl.Size = new System.Drawing.Size( 336, 198 );
            this.zedGraphControl.TabIndex = 0;
            this.zedGraphControl.DoubleClick += new System.EventHandler( this.zedGraphControl_DoubleClick );
            this.zedGraphControl.MouseDown += new System.Windows.Forms.MouseEventHandler( this.zedGraphControl_MouseDown );
            // 
            // PlotControl
            // 
            this.Controls.Add( this.send );
            this.Controls.Add( this.zedGraphControl );
            this.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.Name = "PlotControl";
            this.Size = new System.Drawing.Size( 336, 198 );
            this.ResumeLayout( false );

		}
		#endregion

		#region private fields

		double[] crvScale = null;

		#endregion

		private const int sclLngth = 7;

        /// <summary>
        /// Write the plot data to a CSV file.  
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="useXYTableFormat"></param>
        /// <param name="xyTableHeading"></param>
        public void Write( System.IO.StreamWriter writer, bool useXYTableFormat, string xyTableHeading, ArrayList listOfPointLists )
		{
		    char comma = ',';
            char[] trim = { comma };
            string xyRowHeader = null;

            if( writer != null ){
		    // write out header
			    string header = "Date" + comma;

                if( useXYTableFormat == true && xyTableHeading != null ) {
                    XDate hdate0 = zedGraphControl.GraphPane.XAxis.Min;
                    XDate hdate1 = zedGraphControl.GraphPane.XAxis.Max;
                    header = hdate0.DateTime.ToString( "d-MMM-yy" ) + " to " + hdate1.DateTime.ToString( "d-MMM-yy" ) + comma;
                }

                // add the list of all metrics (curve names) to the header
			    int jj = 0;
			    foreach(CurveItem crv in zedGraphControl.GraphPane.CurveList )
			    {
				    string crvName = crv.Label.Replace(",","-");
				    if (this.crvScale != null && this.crvScale[jj] != 1)
					    crvName = crvName.Remove(crvName.Length - sclLngth, sclLngth);

                    if( useXYTableFormat == true ) {        // XY table names should be rowHeader::columnHeader
                        int split = crvName.IndexOf( "::" );
                        if( split >= 0 ) {
                            if( xyRowHeader == null ) {
                                xyRowHeader = crvName.Substring( 0, split );
                            }
                            crvName = crvName.Substring( split + 2 );
                        }
                    }

				    header += crvName + comma;
				    jj++;
			    }
			    header.TrimEnd(trim);

                // write out the header
                if( useXYTableFormat == true && xyTableHeading != null ) {
                    writer.WriteLine( xyTableHeading );
                }
                if( useXYTableFormat == false || xyTableHeading != null ) {
                    writer.WriteLine( header );
                }
            }

            XDate start = zedGraphControl.GraphPane.XAxis.Min;
			XDate end = zedGraphControl.GraphPane.XAxis.Max;
			int[] index = new int[zedGraphControl.GraphPane.CurveList.Count];
            int numCurves = index.Length;                                                       //!!!this value isn't necessarily the one we want!!! Curves are missing entirely in certain situations which isn't want we need to be happening!!!

            // initialize indexes and trim the beginning of each curve
            for( int ii = 0; ii < numCurves; ii++ )
			{
                index[ ii ] = 0;

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
                    for( int ii = 0; noNewData && ii < numCurves; ii++ )
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
                if( useXYTableFormat ) {
                    values = xyRowHeader + comma;
                }

				// check if there is still data to write
				done = true;
                ArrayList rowPoints = new ArrayList();
                if( listOfPointLists != null ) {
                    rowPoints.Add( xyRowHeader );             // the first value is the curve name
                }                

                for( int ii = 0; ii < numCurves; ii++ )
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
                        if( listOfPointLists != null ) {
                            rowPoints.Add( yVal );
                        }

						if ( pt.X <= day )
						{
							index[ii]++;
						}
					}
					values += comma;
				}
				if (!done)
				{
                    if( writer != null ) {
                        values.TrimEnd( trim );
                        writer.WriteLine( values );
                    }
                    if( listOfPointLists != null ) {
                        listOfPointLists.Add( rowPoints );
                    }
				}
				day.AddDays(1.0);
			}
		}

		public void Scale()
		{
			// scale all curves
			// initialize
            if( crvScale == null || crvScale.Length < zedGraphControl.GraphPane.CurveList.Count )
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

        public void ScaleCrv( int index, int power ) {
            ScaleCrv( index, power, 0 );
        }

		/// <summary>
		/// return the power of 10 that the crv is scaled by
		/// the negative sign is because crvScale is the inverse transform
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public void ScaleCrv( int index, int power, double absoluteScale )
		{
            Console.WriteLine( "ScaleCrv( {0}, {1}, {2} )", index, power, absoluteScale );
			double scale = Math.Pow(10, power);
			ZedGraph.CurveItem crv = zedGraphControl.GraphPane.CurveList[index];
			ZedGraph.PointPairList points = crv.Points;
			ZedGraph.PointPairList scaledPoints = new PointPairList();
            double scaleFactor = scale * crvScale[ index ];
            if( absoluteScale != 0 ) {
                scaleFactor = scale;
            }
            Console.WriteLine( "Scaling curve {0} points by {1}", index, scaleFactor );

            for( int ii = 0; ii < crv.NPts; ++ii ) {
                scaledPoints.Add( new PointPair( crv[ ii ].X, scaleFactor * crv[ ii ].Y, crv[ ii ].Tag.ToString() ) );
            }
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

			if (power != 0)
				crv.Label += " (10^" + power + ")";

			if (crv.IsY2Axis)
			{
				// add >
				crv.Label += " >";
			}

            if( absoluteScale != 0 ) {
                crvScale[ index ] = absoluteScale;
                Console.WriteLine( "ScaleCrv set crvScale[ {0} ] to {1}", index, crvScale[ index ] );
            }
            else {
                crvScale[ index ] = Math.Pow( 10, -power );
                Console.WriteLine( "ScaleCrv set crvScale[ {0} ] to {1} (reg path!!)", index, crvScale[ index ] );
            }
			this.DataChanged();
		}

        public void SetCrvScale( int[] newCrvScales, int[] existingCrvScales ) {
            for( int i = 0; i < newCrvScales.Length; i++ ) {
                double newScale = Math.Pow( 10, -newCrvScales[ i ] );
                double existingScale = Math.Pow( 10, -existingCrvScales[ i ] );
                int scalePowDelta = 0;
                if( newScale != existingScale ) {
                    double scaleRatio = newScale / existingScale;
                    scalePowDelta = -(int)Math.Log10( scaleRatio );

                    this.crvScale[ i ] = newScale;

                    Console.WriteLine( "calling ScaleCrv( {0}, {1} ) for {2}", i, scalePowDelta, this.Title );
                    Console.WriteLine( "new scale = {0}", newScale );
                }
                if( i < zedGraphControl.GraphPane.CurveList.Count ) {
                    ScaleCrv( i, scalePowDelta, newScale );
                }
                else {
                    Console.WriteLine( "WARNING: SetCrvScale() called for more curves than are displayed!" );
                }
            }

            //Console.WriteLine( "SetCrvScale - sanity check" );
            //for( int i = 0; i < newCrvScales.Length; i++ ) {
            //    double cs = this.crvScale[ i ];
            //    Console.WriteLine( "  scale {0} = {1}", i, cs );
            //}
      }

        public int CrvScale( int index )
		{
            //!!!JimJ  - until determing where/when Scale() s/b called, just extend crvScale[] as needed
            ////if( crvScale == null ) {
            ////    throw new Exception( "Error: PlotControl.CrvScale() has null crvScale array." );
            ////}
            ////else if( index >= crvScale.Length ) {
            ////    String msg = String.Format( "Error: PlotControl.CrvScale( {0} ) arg out of bounds (crvScale length = {1})", index, crvScale.Length );
            ////    throw new Exception( msg );
            ////}

            if( crvScale == null || index >= crvScale.Length ) {
                ArrayList ccsl = new ArrayList();
                int i = 0;
                if( crvScale != null ) {
                    for( ; i < crvScale.Length; i++ ) {
                        ccsl.Add( (double)crvScale[ i ] );
                    }
                }
                for( ; i <= index; i++ ) {
                    ccsl.Add( 1.0 );
                }
                crvScale = new double[ index + 1 ];
                ccsl.CopyTo( crvScale );
            }

            double vv =  crvScale[ index ];
            return (int)-Math.Floor( Math.Log10( crvScale[ index ] ) );
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

        public bool LegendVisible {
            get {
                return this.zedGraphControl.GraphPane.Legend.IsVisible;
            }

            set {
                if( this.zedGraphControl.GraphPane.Legend.IsVisible == value ) {
                    return;           // no change
                }
                hideLegendItem_Click( null, null );   // toggle legend visibility
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
            if( LegendVisibilitySet != null && sender != null ) {
                LegendVisibilitySet( this.zedGraphControl.GraphPane.Legend.IsVisible );
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

        private void send_Click( object sender, System.EventArgs e ) {
            if( PlotSelected != null )
                PlotSelected( this );
        }

        private void zedGraphControl_DoubleClick( object sender, EventArgs e ) {
            //if( this.Dock == System.Windows.Forms.DockStyle.Fill ) {
            //    return;
            //}

            if( PlotSelected != null )
                PlotSelected( this );
        }
	}
}
