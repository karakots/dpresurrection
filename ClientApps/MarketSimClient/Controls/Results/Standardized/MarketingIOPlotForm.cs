using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Results.Standardized
{
    public partial class MarketingIOPlotForm : Form
    {
        private int NUM_OK_CURVES = 13;

        private float graphRectTopMargin = 35;
        private float graphRectLeftMargin = 50;
        private float graphRectBottomMargin = 150;
        private float overallBottomMargin = 10;
        private float graphRectRightMargin = 50;
        private int dateRegionHeight = 80;
        private int legendSampleHeight = 10;
        private int legendSampleWidth = 40;
        private int legendLeftMargin = 40;
        private int legendRightMargin = 40;
        private Pen legendSamplePen = new Pen( Brushes.LightGray, 0.1f );

        private Font legendFont = new Font( "Arial Narrow", 8f );
        private Font titleFont = new Font( "Arial", 11f, FontStyle.Bold );
        private Font subTitleFont = new Font( "Arial Narrow", 8f, FontStyle.Italic );

        private Rectangle areaRect;
        private Rectangle graphRect;

        private Color overallBackgroundColor = Color.White;
        private Color graphAreaBackgroundBorderColor = Color.Black;
        private Color graphAreaBackgroundFillColor = Color.FromArgb( 248, 248, 245 );

        private DateTime[] dates;
        private double[ , ] data;
        private PointF[][] curvePoints;
        private Region[] curveRegion;
        private GraphicsPath[] graphicsPath;

        private double[] mins;
        private double[] maxs;

        private int[] xForIndex;

        private Brush bkgBrush;
        private Pen graphBkgBorderPen;
        private Brush graphBkgBrush;

        private double spanDays;
        private int numCurves;
        private int numValues;

        private MarketingIOPlotCurveInfo[] curveInfo;
        private int[] curveIndex;

        private double wideWidth = 2.5;
        private double medWidth = 1.5;
        private double narrowWidth = 0.5;
        private int dateLabelY;
        private const int numGraphs = 13;
        private string title;
        private string subTitle;
        private double[] errRange = new double[] { 0.01, 0.02, 0.05, 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100 };     // percent
        private double errAxisStep;

        private string mainTitleFormat = "{0} - MarketSim I/O Graph";

        public MarketingIOPlotForm( DateTime[] dates, double[,] data, string product, string title, string subTitle ) {
            InitializeComponent();

            this.Text = String.Format( mainTitleFormat, product );
            this.title = title;
            this.dates = dates;
            this.data = data;
            this.subTitle = subTitle;
            InitCurveInfo();

            Init();
        }

        private void InitCurveInfo() {
            curveInfo = new MarketingIOPlotCurveInfo[ numGraphs ];
            curveIndex = new int[ numGraphs ];

            // These are in order of the columns in the input data.
            curveInfo[ 0 ] = new MarketingIOPlotCurveInfo( "Share", 0, 0, 120, Color.Red, 11, wideWidth );

            curveInfo[ 1 ] = new MarketingIOPlotCurveInfo( "Persuasion", 0, 0, 120, Color.DarkRed, 9, wideWidth );

            curveInfo[ 2 ] = new MarketingIOPlotCurveInfo( "% Awareness", 0, 0, 120, Color.FromArgb( 255, 255, 175 ), 0, 1 );
            curveInfo[ 2 ].SetFillled( 100 );

            curveInfo[ 3 ] = new MarketingIOPlotCurveInfo( "Real Share", 0, 0, 120, Color.Black, 10, wideWidth );

            curveInfo[ 4 ] = new MarketingIOPlotCurveInfo( "Error", 0, -10, 20, Color.Gray, 4, medWidth );
            curveInfo[ 4 ].SetFillled( 40, false );

            curveInfo[ 5 ] = new MarketingIOPlotCurveInfo( "Price Unpromo", 0, 0, 120, Color.LimeGreen, 5, wideWidth );
            curveInfo[ 6 ] = new MarketingIOPlotCurveInfo( "% Promo", 0, 0, 120, Color.DarkGreen, 7, wideWidth );
            curveInfo[ 7 ] = new MarketingIOPlotCurveInfo( "% Dist", 0, 0, 120, Color.DarkBlue, 8, wideWidth );
            curveInfo[ 8 ] = new MarketingIOPlotCurveInfo( "Price Promo", 0, 0, 120, Color.Green, 6, wideWidth );

            curveInfo[ 9 ] = new MarketingIOPlotCurveInfo( "GRPs", 0, 0, 12000000, Color.BlueViolet, 1, narrowWidth );
            curveInfo[ 9 ].SetFillled( 30 );

            curveInfo[ 10 ] = new MarketingIOPlotCurveInfo( "% Display", 0, 0, 120, Color.Cyan, 2, 1 );
            curveInfo[ 10 ].SetFillled( 40 );
            curveInfo[ 11 ] = new MarketingIOPlotCurveInfo( "Coupons", 0, 0, 12000000, Color.Orange, 3, 1 );
            curveInfo[ 11 ].SetFillled( 75 );

            curveInfo[ 12 ] = new MarketingIOPlotCurveInfo( "Market Utility", 0, 0, 120, Color.Navy, 12, medWidth );

            for( int i = 0; i < curveInfo.Length; i++ ) {
                curveIndex[ curveInfo[ i ].ZIndex ] = i;
            }
        }

        // size-invariant initialization
        private void Init() {
            numCurves = data.GetLength( 1 );
            numValues = data.GetLength( 0 );

            curveIndex = new int[ numCurves ];
            InitCurveInfo();

            bkgBrush = new SolidBrush( overallBackgroundColor );
            graphBkgBorderPen = new Pen( graphAreaBackgroundBorderColor, 1.0f );
            graphBkgBrush = new SolidBrush( graphAreaBackgroundFillColor );

            TimeSpan ts = dates[ dates.Length - 1 ] - dates[ 0 ];
            spanDays = ts.TotalDays;

            mins = new double[ numCurves ];
            maxs = new double[ numCurves ];
            for( int c = 0; c < numCurves; c++ ) {
                mins[ c ] = double.MaxValue;
                maxs[ c ] = double.MinValue;
                for( int r = 0; r < numValues; r++ ) {
                    if( data[ r, c ] > maxs[ c ] ) {
                        maxs[ c ] = data[ r, c ];
                    }
                    if( data[ r, c ] < mins[ c ] ) {
                        mins[ c ] = data[ r, c ];
                    }
                }
            }
            for( int ii = 0; ii < curveInfo.Length; ii++ ) {
                Console.WriteLine( "{0}:    min {1}  max {2}", curveInfo[ ii ].CurveName, mins[ ii ], maxs[ ii ] );
            }

            // prices
            double maxp = Math.Max( maxs[ 5 ], maxs[ 8 ] );
            curveInfo[ 5 ].MaxDisplayedValue = maxp * 1.2;
            curveInfo[ 8 ].MaxDisplayedValue = maxp * 1.2;
            curveInfo[ 5 ].CurveName = String.Format( "{0} ({1:f2})", curveInfo[ 5 ].CurveName, maxs[ 5 ] );
            curveInfo[ 8 ].CurveName = String.Format( "{0} ({1:f2})", curveInfo[ 8 ].CurveName, maxs[ 8 ] );

            // share
            double maxshr = Math.Max( maxs[ 0 ], maxs[ 3 ] );
            curveInfo[ 0 ].MaxDisplayedValue = maxshr * 1.35;
            curveInfo[ 3 ].MaxDisplayedValue = maxshr * 1.35;
            curveInfo[ 0 ].CurveName = String.Format( "{0} ({1:f2} %)", curveInfo[ 0 ].CurveName, maxs[ 0 ] );
            curveInfo[ 3 ].CurveName = String.Format( "{0} ({1:f2} %)", curveInfo[ 3 ].CurveName, maxs[ 3 ] );

            //GRPs
            curveInfo[ 9 ].MaxDisplayedValue = maxs[ 9 ] * 2;

            //coupons
            curveInfo[ 11 ].MaxDisplayedValue = maxs[ 11 ] * 2;

            //persuasuion
            curveInfo[ 1 ].MaxDisplayedValue = maxs[ 1 ] * 2;
            curveInfo[ 1 ].Pen.DashStyle = DashStyle.Dot;
            curveInfo[ 1 ].CurveName = String.Format( "{0} ({1:f2})", curveInfo[ 1 ].CurveName, maxs[ 1 ] );

            //% Promo
            curveInfo[ 6 ].Pen.DashStyle = DashStyle.Dash;

            //% Dist
            curveInfo[ 7 ].Pen.DashStyle = DashStyle.DashDotDot;

            //Error
            double maxabserr = Math.Max( Math.Abs( maxs[ 4 ] ), Math.Abs( mins[ 4 ] ) );

            if( maxabserr < errRange[ errRange.Length - 1 ]  ) {
                // we can select a preset range
                int errRangeIndex = 0;
                errAxisStep = errRange[ 0 ];
                Console.WriteLine( "\nSetting error axis step.   maxabserr = {0}", maxabserr );
                while( (errRangeIndex < errRange.Length) && (errRange[ errRangeIndex ] <= maxabserr ) ){
                    errAxisStep = errRange[ errRangeIndex ];
                    Console.WriteLine( "Bumped error axis step to {0}", errAxisStep );
                    errRangeIndex += 1;
                }
            }
            else {
                // out of scaling range!
                errAxisStep = errRange[ errRange.Length - 1 ];
            }

            //errAxisStep = 10;
            Console.WriteLine( "\nUsing error axis step value = {0}", errAxisStep );
            curveInfo[ 4 ].MinDisplayedValue = -errAxisStep * 4;
            curveInfo[ 4 ].MaxDisplayedValue = errAxisStep * 6;
        }

        private double[] scaledY0;

        // size-dependent initialization
        private void InitForPaint() {
            areaRect = new Rectangle( 0, 0, this.ClientRectangle.Right, this.ClientRectangle.Bottom );
            graphRect = new Rectangle( (int)graphRectLeftMargin, (int)graphRectTopMargin, 
                (int)(this.ClientRectangle.Width - (graphRectLeftMargin + graphRectRightMargin)),
                (int)(this.ClientRectangle.Height - (graphRectTopMargin +  graphRectBottomMargin)) );

            dateLabelY = graphRect.Bottom + 4;

            // set up the array of real X values for each data index
            xForIndex = new int[ numValues ];
            for( int i = 0; i < numValues; i++ ) {
                TimeSpan ts = dates[ i ] - dates[ 0 ];
                double pointDays = ts.TotalDays;

                xForIndex[ i ] = (int)Math.Round( (pointDays / spanDays) * (graphRect.Width - 1) ) + graphRect.X + 1;
            }

            // set up the array of real Y values for each curve
            curvePoints = new PointF[ numCurves ][];
            scaledY0 = new double[ numCurves ];
            for( int c = 0; c < numCurves; c++ ) {
                curvePoints[ c ] = new PointF[ numValues ];

                if( c >= NUM_OK_CURVES ) {
                    break;
                }
                // adjust each Y value and create curve points
                MarketingIOPlotCurveInfo info = curveInfo[ c ];
                for( int i = 0; i <= numValues; i++ ) {
                    double offsetAdjustedValue = 0;
                    if( i < numValues ) {
                        offsetAdjustedValue = data[ i, c ] - info.ValueOffset;
                    }
                    double fractionOfRangeAboveMinVisible = 0;
                    if( info.ValueRange != 0 ) {
                        fractionOfRangeAboveMinVisible = (offsetAdjustedValue - info.MinDisplayedValue) / info.ValueRange;
                    }
                    int scaledYValue = graphRect.Bottom - (int)Math.Round( fractionOfRangeAboveMinVisible * graphRect.Height );
                    if( scaledYValue < graphRect.Top ) {
                        scaledYValue = graphRect.Top;
                    }
                    if( scaledYValue > graphRect.Bottom ) {
                        scaledYValue = graphRect.Bottom;
                    }
                    if( i < numValues ) {
                        curvePoints[ c ][ i ] = new PointF( xForIndex[ i ], scaledYValue );
                    }
                    else {
                        // the extra point is zero (for reference)
                        scaledY0[ c ] = scaledYValue;
                    }
                }
            }

            // put the data into per-curve Region structures
            curveRegion = new Region[ numCurves ];
            graphicsPath = new GraphicsPath[ numCurves ];
            for( int c = 0; c < numCurves; c++ ) {
                 if( c >= NUM_OK_CURVES ) {
                    break;
                }

                PointF[] closingPoints = new PointF[ 2 ];
                if( curveInfo[ c ].FillBottom == false ) {
                    closingPoints[ 0 ] = new PointF( graphRect.Right, (float)scaledY0[ c ] );
                    closingPoints[ 1 ] = new PointF( graphRect.Left, (float)scaledY0[ c ] );
                }
                else {
                    closingPoints[ 0 ] = new PointF( graphRect.Right, graphRect.Bottom );
                    closingPoints[ 1 ] = new PointF( graphRect.Left, graphRect.Bottom );
                }


                //PointF[] closingPoints = new PointF[ 4 ];
                //closingPoints[ 0 ] = curvePoints[ c ][ numValues - 1 ];

                //if( curveInfo[ c ].FillBottom == false ) {
                //    closingPoints[ 1 ] = new PointF( graphRect.Right, (float)scaledY0[ c ] );
                //    closingPoints[ 2 ] = new PointF( graphRect.Left, (float)scaledY0[ c ] );
                //}
                //else {
                //    closingPoints[ 1 ] = new PointF( graphRect.Right, graphRect.Bottom );
                //    closingPoints[ 2 ] = new PointF( graphRect.Left, graphRect.Bottom );
                //}

                //closingPoints[ 3 ] = curvePoints[ c ][ 0 ];

                graphicsPath[ c ] = new GraphicsPath( FillMode.Winding );
                for( int i = 0; i < numValues; i++ ) {
                    graphicsPath[ c ].AddLines( curvePoints[ c ] );
                    graphicsPath[ c ].AddLines( closingPoints );
                }
                curveRegion[ c ] = new Region( graphicsPath[ c ] );
            }
        }

        private void GraphForm_Paint( object sender, PaintEventArgs e ) {
            InitForPaint();

            DrawBackground( e.Graphics );

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            for( int z = 0; z < numCurves; z++ ) {
                int curve = curveIndex[ z ];
                if( curve < NUM_OK_CURVES ) {
                    DrawCurve( curve, e.Graphics );
                }
            }

            DrawDateXAxis( e.Graphics );
            DrawAuxXAxis( e.Graphics, 4 );

            DrawYAxis1( e.Graphics );
            DrawYAxis2( e.Graphics );

            DrawLegend( e.Graphics );

            DrawTitles( e.Graphics );
        }

        private void DrawBackground( Graphics g ) {
            g.FillRectangle( bkgBrush, areaRect );
            g.FillRectangle( graphBkgBrush, graphRect );
            g.DrawRectangle( graphBkgBorderPen, graphRect );
        }

        private void DrawCurve( int c, Graphics g ) {
            if( curveInfo[ c ].FillCurve == false || curveInfo[ c ].FillBottom == false ) {
                //g.DrawPath( curveInfo[ c ].Pen, graphicsPath[ c ] );
                g.DrawLines( curveInfo[ c ].Pen, curvePoints[ c ] );
            }

            if( curveInfo[ c ].FillCurve == true ) {
                g.FillRegion( curveInfo[ c ].Brush, curveRegion[ c ] );
            }
        }

        private void DrawDateXAxis( Graphics g ) {
            Font font = new Font( "Arial", 9f );
            SizeF typSize = g.MeasureString( "6/14/2008", font );
            double ht = typSize.Height;
            StringFormat vformat = new StringFormat();
            vformat.FormatFlags = StringFormatFlags.DirectionVertical;

            int nsteps = 1;
            if( xForIndex[ nsteps ] - xForIndex[ 0 ] < (ht * 1.5) ) {
                nsteps = 2;
                if( xForIndex[ nsteps ] - xForIndex[ 0 ] < (ht * 1.5) ) {
                    nsteps = 4;
                    if( xForIndex[ nsteps ] - xForIndex[ 0 ] < (ht * 1.5) ) {
                        nsteps = 8;
                        if( xForIndex[ nsteps ] - xForIndex[ 0 ] < (ht * 1.5) ) {
                            nsteps = 12;
                        }
                    }
                }
            }

            Pen p = new Pen( Color.Black, 1f );
            for( int d = 0; d < numValues; d += nsteps ) {
                PointF p0 = new PointF( (float)(xForIndex[ d ] - (ht/2)), dateLabelY );
                PointF p1 = new PointF( xForIndex[ d ], graphRect.Bottom );
                PointF p2 = new PointF( xForIndex[ d ], dateLabelY );

                g.DrawLine( p, p1, p2 );

                string dstr = dates[ d ].ToString( "M/d/yyyy" );
                g.DrawString( dstr, font, Brushes.Black, p0, vformat );
            }
        }

        private void DrawAuxXAxis( Graphics g, int curveNum ) {
            int axisY =(int)Math.Round( ScaledY( 0, curveInfo[ curveNum ].MinDisplayedValue, curveInfo[ curveNum ].MaxDisplayedValue ) );
            ////g.DrawLine( curveInfo[ curveNum ].Pen , graphRect.Left, axisY, graphRect.Right, axisY );
            g.DrawLine( curveInfo[ curveNum ].Pen, graphRect.Left, (float)scaledY0[ curveNum ], graphRect.Right, (float)scaledY0[ curveNum ] );
        }

        private void DrawYAxis1( Graphics g ) {
            Font font = new Font( "Arial", 9f );
            Pen pen = new Pen( Brushes.Black, 1f );
            for( double y = 0; y <= 120; y += 20 ) {
                double scaledYValue = ScaledY( y, 0, 120 );

                PointF p0 = new PointF( 5f, (float)scaledYValue - 6 );
                PointF p1 = new PointF( graphRect.Left, (float)scaledYValue );
                PointF p2 = new PointF( graphRect.Left - 6, (float)scaledYValue );
                g.DrawLine( pen, p1, p2 );
                string s = String.Format( "{0:f0}%", y );
                g.DrawString( s, font, Brushes.Black, p0 );
            }
        }

        private void DrawYAxis2( Graphics g ) {
            // the right-hand Y axis matches the error curve
            Font font = new Font( "Arial", 9f );
            Pen pen = new Pen( Brushes.Black, 1f );
            MarketingIOPlotCurveInfo errInfo = curveInfo[ 4 ];
            double errMin = errInfo.MinDisplayedValue;
            double errMax = errInfo.MaxDisplayedValue;

            PointF p0 = new PointF( -1, -1 );
            for( double y = -(errAxisStep * 2); y <= errAxisStep * 2; y += errAxisStep ) {
                double scaledYValue = ScaledY( y, errMin, errMax );

                p0 = new PointF( graphRect.Right + 8, (float)scaledYValue - 6 );
                PointF p1 = new PointF( graphRect.Right, (float)scaledYValue );
                PointF p2 = new PointF( graphRect.Right + 6, (float)scaledYValue );
                g.DrawLine( pen, p1, p2 );
                string s = String.Format( "{0:f2}", y / 100 );
                g.DrawString( s, font, Brushes.Black, p0 );
            }

            p0 = new PointF( p0.X + 1, p0.Y - 26 );
            g.DrawString( "Error", font, Brushes.Black, p0 );
        }

        private double ScaledY( double y, double ymin, double ymax ) {
            double fractionOfRangeAboveMinVisible = (y - ymin) / (ymax - ymin);
            double scaledYValue = graphRect.Bottom - fractionOfRangeAboveMinVisible * graphRect.Height;
            return scaledYValue;
        }

        private void DrawLegend( Graphics g ) {
            int lcols = 4;
            int lrows = 4;
            int colWid = (int)Math.Round( (areaRect.Width - legendLeftMargin - legendRightMargin) / (double)lcols );
            int rowHt = (int)Math.Round( (areaRect.Bottom - graphRect.Bottom - dateRegionHeight - overallBottomMargin) / (double)lrows );

            int n = 0;
            for( int c = 0; c < lcols; c++ ) {
                for( int r = 0; r < lrows; r++ ) {
                    int cnum = this.curveIndex[ n ];
                    MarketingIOPlotCurveInfo info = curveInfo[ cnum ];
                    int x = (c * colWid) + (colWid / 2) + legendLeftMargin;
                    int y = (r * rowHt) + graphRect.Bottom + dateRegionHeight;

                    if( info.FillCurve == false ) {
                        g.DrawLine( info.Pen, x - legendSampleWidth - 10, y + 8, x - 10, y + 8 );
                    }
                    else {
                        g.FillRectangle( info.Brush, x - legendSampleWidth - 10, y + 3, legendSampleWidth, legendSampleHeight );
                    }

                    g.DrawRectangle( legendSamplePen, x - legendSampleWidth - 10, y + 3, legendSampleWidth, legendSampleHeight );

                    g.DrawString( info.CurveName, legendFont, Brushes.Black, x, y );
                    n++;
                    if( n >= curveInfo.Length ) {
                        return;
                    }
                }
            }
        }

        private void DrawTitles( Graphics g ) {
            SizeF titleSize = g.MeasureString( title, titleFont );
            int x = (int)Math.Round( (areaRect.Width - titleSize.Width) / 2 );
            g.DrawString( title, titleFont, Brushes.MidnightBlue, x, 10 );

            if( subTitle != null ) {
                SizeF subTitleSize = g.MeasureString( subTitle, subTitleFont );
                int sx = (int)Math.Round( (areaRect.Width - subTitleSize.Width) - 5 );
                g.DrawString( subTitle, subTitleFont, Brushes.MidnightBlue, sx, 0 );
            }
        }

        private void GraphForm_SizeChanged( object sender, EventArgs e ) {
            this.Invalidate();
        }
    }
}