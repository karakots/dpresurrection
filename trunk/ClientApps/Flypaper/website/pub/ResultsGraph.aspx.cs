using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;

using HouseholdLibrary;
using SimInterface;
using WebLibrary;

public partial class ResultsGraph : System.Web.UI.Page
{
    private const int width = 695;
    private const int height = 500;

    private const int margnLeft = 10;
    private const int margnRight = 10;
    private const int margnTop = 10;
    private const int margnBottom = 10;
    private const int spaceBetweenGraphs = 10;

    private const int nameColWidth = 100;
    private const int yAxisColWidth = 20;

    private MediaPlan mediaPlan;
    private SimOutput data;
    private int segmentNumber = 0;
    private int numGraphs;
    private int graphAreaHeight;

    //private bool noResultsForPlan = false;

    //private Brush graphBrush = new SolidBrush( Color.FromArgb( 200, 230, 55, 55 ) );
    //private Pen graphPen = new Pen( new SolidBrush( Color.FromArgb( 230, 55, 55 ) ), 3.0f );
    //private Pen graphAreaBorderPen = new Pen( new SolidBrush( Color.FromArgb( 240, 200, 0 ) ), 1.0f );

    protected Font labelFont = new Font( "Arial", 10f, FontStyle.Bold );
    protected Font yAxisLabelFont = new Font( "Arial", 8f, FontStyle.Bold );
    protected Brush labelBrush = new SolidBrush( Color.Black );
    protected Pen yAxisPen = new Pen( new SolidBrush( Color.Black ) );
    protected Color bkgColor = Color.White;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;

    void Page_Load( Object sender, EventArgs e ) {

        InitializeVariables();

        // Since we are outputting a Jpeg, set the ContentType appropriately
        Response.ContentType = "image/jpeg";

        Bitmap objBitmap = new Bitmap( width, height );
        Graphics objGraphics = Graphics.FromImage( objBitmap );

        string err = GetInputs();

        if( err != null ) {
            objGraphics.DrawString( err, new Font( "Arial", 10f ), Brushes.Red, new PointF( 20, 20 ) );
            FinishUp( objBitmap, objGraphics );
            return;
        }

        this.numGraphs = this.data.metrics.Count / this.mediaPlan.DemographicCount;
        this.graphAreaHeight = (int)Math.Round( (height - margnTop - margnBottom + spaceBetweenGraphs) / (double)this.numGraphs ) - spaceBetweenGraphs;
        objGraphics.FillRectangle( new SolidBrush( bkgColor ), 0, 0, width, height );

        int graphWidth = width - margnLeft - margnRight - nameColWidth - yAxisColWidth;
        int x = margnLeft;
        int y = margnTop;
        int metricIndex = this.segmentNumber * this.mediaPlan.DemographicCount;

        Color[] graphColors = new Color[] { 
            Color.FromArgb( 0, 0x69, 0xAA ), 
            Color.FromArgb(  0x69, 0x69, 0xAA ), 
            Color.FromArgb(  0x69, 0x69, 0x2A ), 
            Color.FromArgb(  0x69, 0xA9, 0x2A ),
            Color.FromArgb(  0xA9, 0xA9, 0x2A ),
        };

        for( int g = 0; g < this.numGraphs; g++ ) {

            Rectangle labelRect = new Rectangle( x, y, nameColWidth, graphAreaHeight );
            Rectangle yAxisRect = new Rectangle( x + nameColWidth + 1, y, yAxisColWidth, graphAreaHeight );
            Rectangle graphRect = new Rectangle( x + nameColWidth + yAxisColWidth + 2, y, graphWidth, graphAreaHeight );

            double graphMax = YAxisMax( GetMaxValue( data.metrics[ metricIndex + g ].values ) );

            string graphName = data.metrics[ metricIndex + g ].Type;
            if( graphName == null || graphName == "" ) {
                graphName = String.Format( "Metric {0}", g );
            }
            DrawGraphLabel( objGraphics, labelRect, graphName );
            DrawGraphYAxis( objGraphics, yAxisRect, graphMax );
            DrawDataGraph( objGraphics, graphRect, data.metrics[ metricIndex + g ].values, graphMax, graphColors[ g % graphColors.Length ] );

            y += graphAreaHeight + spaceBetweenGraphs;
        }

        FinishUp( objBitmap, objGraphics );
    }

    /// <summary>
    /// Draws the data part of a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawDataGraph( Graphics g, Rectangle rect, List<double> vals, double maxVal, Color color ) {
        g.FillRectangle( Brushes.White, rect );

        PointF[] graphPoints = new PointF[ vals.Count + 3 ];

        double x0 = rect.Left;
        double y0 = rect.Bottom;
        double x = x0;
        double y = y0;

        double xStep = (double)rect.Width / ((double)vals.Count - 1);

        graphPoints[ 0 ] = new PointF( rect.Left, rect.Bottom );
        for( int p = 0; p < vals.Count; p++ ) {

            y = y0 - ((vals[ p ] / maxVal) * rect.Height);

            graphPoints[ p + 1 ] = new PointF( (float)x, (float)y );

            x += xStep;
        }
        x -= xStep;

        graphPoints[ vals.Count + 1 ] = new PointF( (float)x, rect.Bottom );
        graphPoints[ vals.Count + 2 ] = new PointF( rect.Left, rect.Bottom );

        g.FillPolygon( new SolidBrush( color ), graphPoints );

        g.DrawLine( yAxisPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom );
    }

    /// <summary>
    /// Draws the main label area for a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawGraphLabel( Graphics g, Rectangle rect, string text ) {
        g.FillRectangle( Brushes.White, rect );
        g.DrawString( text, labelFont, labelBrush, 5 + rect.Left, 20 + rect.Top );
    }

    /// <summary>
    /// Returns the largest value in the list.
    /// </summary>
    /// <param name="metricValues"></param>
    /// <returns></returns>
    private double GetMaxValue( List<double> metricValues ) {
        double max = 0;
        foreach( double val in metricValues ) {
            max = Math.Max( val, max );
        }
        return max;
    }

    /// <summary>
    /// Returns the max value for the Y axis of a graph.  Value will be one of 1, 2, 5, 10, 20, 50, ...
    /// </summary>
    /// <param name="dataMaximum"></param>
    /// <returns></returns>
    private double YAxisMax( double dataMaximum ) {
        double tensPower = 0.001;
        do {
            if( dataMaximum < 1 * tensPower ) {
                return 1 * tensPower;
            }
            else if( dataMaximum < 2 * tensPower ) {
                return 2 * tensPower;
            }
            else if( dataMaximum < 5 * tensPower ) {
                return 5 * tensPower;
            }
            tensPower *= 10;
        }
        while( tensPower < 1000000 );       // sanity check only
        return 1000000;
    }

    /// <summary>
    /// Draws the main label area for a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawGraphYAxis( Graphics g, Rectangle rect, double maxVal ) {
        g.FillRectangle( Brushes.White, rect );
        g.DrawLine( yAxisPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom );
        g.DrawLine( yAxisPen, rect.Right, rect.Bottom, rect.Right, rect.Top );
        string maxStr = "";
        if( maxVal >= 1 ) {
            maxStr = String.Format( "{0:f0}", maxVal );
        }
        else {
            maxStr = String.Format( "{0:f3}", maxVal );
        }
        g.DrawString( maxStr, yAxisLabelFont, labelBrush, 5 + rect.Left, 2 + rect.Top );
    }


    /// <summary>
    /// Gets the inputs form the URL (plan ID and segment number).  Returns null if OK; error details if there was a problem.
    /// </summary>
    /// <returns></returns>
    private string GetInputs() {
        string planID = Request[ "p" ];
        string err = null;

        if( planID == null ) {
            err = "NULL planID";
        }
        else {
            // get the plan 
            this.mediaPlan = Utils.PlanForID( new Guid( planID ), this.currentMediaPlans );
            if( this.mediaPlan == null ) {
                err = "No media plan with ID: " + planID;
            }
            else {
                // get the sim results
                this.data = this.mediaPlan.Results;
                if( this.data == null ) {
                    err = "No Simulation Results for this Plan";
                    //this.noResultsForPlan = true;
                }
            }

            if( err == null ) {
                // get the segment number
                string segNum = Request[ "s" ];
                try {
                    this.segmentNumber = Convert.ToInt32( segNum );
                }
                catch( Exception ) {
                    err = "Unable to parse segment number:  " + segNum;
                }

                if (this.segmentNumber < 0 || this.segmentNumber > this.mediaPlan.SegmentCount)
                {
                    err = "Segment number out of range:  " + segNum;
                }
            }
        }
        return err;
    }

    /// <summary>
    /// Commits the drawing to the output
    /// </summary>
    /// <param name="objBitmap"></param>
    /// <param name="objGraphics"></param>
    private void FinishUp( Bitmap objBitmap, Graphics objGraphics ) {
        // Save the image to the OutputStream
        objBitmap.Save( Response.OutputStream, ImageFormat.Jpeg );

        // clean up...
        objGraphics.Dispose();
        objBitmap.Dispose();
    }

    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.engineeringMode = Utils.InEngineeringMode( this );
    }
}
