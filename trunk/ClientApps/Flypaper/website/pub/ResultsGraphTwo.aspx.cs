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
using System.IO;

using HouseholdLibrary;
using SimInterface;
using WebLibrary;

public partial class ResultsGraphTwo : System.Web.UI.Page
{
    private const int width = 600;
    private const int height = 200;

    private const int margnLeft = 10;
    private const int margnRight = 10;
    private const int margnTop = 10;
    private const int margnBottom = 20;
    private const int spaceBetweenGraphs = 10;

    private const int nameColWidth = 70;
    private const int yAxisColWidth = 40;

    private MediaPlan mediaPlan;
    private int segmentNumber = 0;
    private int metricNumber = 0;
    private int regionNumber = 0;
    private SimOutput data;
    private int dataMetricIndex = 0;

    private bool compareView = false;
    private string comparisonVersion;
    private SimOutput comparisonData;
    private int comparisonDataMetricIndex = 0;

    private int graphAreaHeight;

    protected Font labelFont = new Font( "Arial", 9f, FontStyle.Regular );
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

     

        Bitmap objBitmap = new Bitmap( width, height );
        Graphics objGraphics = Graphics.FromImage( objBitmap );

        string err = GetInputs();

        if( err != null ) {
            objGraphics.Clear( Color.White );
            objGraphics.DrawString( err, new Font( "Arial", 10f ), Brushes.Black, new PointF( 20, 20 ) );
            FinishUp( objBitmap, objGraphics );
            return;
        }

        this.graphAreaHeight = height - margnTop - margnBottom;
        objGraphics.FillRectangle( new SolidBrush( bkgColor ), 0, 0, width, height );

        int graphWidth = width - margnLeft - margnRight - nameColWidth - yAxisColWidth;
        int x = margnLeft;
        int y = margnTop;

        Color graphColor = Color.FromArgb(  0x69, 0xA9, 0x2A );

        Rectangle labelRect = new Rectangle( x, y, nameColWidth, graphAreaHeight );
        Rectangle yAxisRect = new Rectangle( x + nameColWidth + 1, y, yAxisColWidth, graphAreaHeight );
        Rectangle graphRect = new Rectangle( x + nameColWidth + yAxisColWidth + 2, y, graphWidth, graphAreaHeight );

        double pctFac = 0;
        double graphMax = 0;
        string pctUnits = null;
        string graphInfo = SetupGraphLabels( out graphMax, out pctUnits, out pctFac );

        DrawGraphYAxis( objGraphics, yAxisRect, graphMax * pctFac, pctUnits );
        DrawDataGraph( objGraphics, graphRect, data.metrics[ this.dataMetricIndex ].values, graphMax, graphColor, true );
        if( this.compareView == true ) {
            DrawDataGraph( objGraphics, graphRect, comparisonData.metrics[ this.comparisonDataMetricIndex ].values, graphMax, graphColor, false );
        }
        DrawXAxisLabels( objGraphics, graphRect, this.mediaPlan.StartDate, this.mediaPlan.EndDate );
        DrawGraphLabel( objGraphics, labelRect, graphInfo );
        FinishUp( objBitmap, objGraphics );
    }

    /// <summary>
    /// Draws the data part of a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawDataGraph( Graphics g, Rectangle rect, List<double> vals, double maxVal, Color color, bool doFill ) {

        PointF[] graphPoints = new PointF[ vals.Count + 3 ];
        PointF[] graphLinePoints = new PointF[ vals.Count];

        double x0 = rect.Left;
        double y0 = rect.Bottom;
        double x = x0;
        double y = y0;

        double xStep = (double)rect.Width / ((double)vals.Count - 1);

        graphPoints[ 0 ] = new PointF( rect.Left, rect.Bottom );
        for( int p = 0; p < vals.Count; p++ ) {

            y = y0 - ((vals[ p ] / maxVal) * rect.Height);

            graphPoints[ p + 1 ] = new PointF( (float)x, (float)y );
            graphLinePoints[ p ] = new PointF( (float)x, (float)y );

            x += xStep;
        }
        x -= xStep;

        graphPoints[ vals.Count + 1 ] = new PointF( (float)x, rect.Bottom );
        graphPoints[ vals.Count + 2 ] = new PointF( rect.Left, rect.Bottom );

        if( doFill ) {
            g.FillRectangle( Brushes.White, rect );
            g.FillPolygon( new SolidBrush( color ), graphPoints );
        }
        else {
            g.DrawLines( new Pen( Color.Red ), graphLinePoints );
        }

        // draw the X axis
        g.DrawLine( yAxisPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom );
    }

    private void DrawXAxisLabels( Graphics g, Rectangle rect, DateTime start, DateTime end ) {
        Font f = new Font( "Verdana", 8.0f );
        string s1 = start.ToString( "M/d/yy" );
        string s2 = end.ToString( "M/d/yy" );
        int w1 = (int)g.MeasureString( s1, f ).Width;
        int w2 = (int)g.MeasureString( s2, f ).Width;
        g.DrawString( s1, f, labelBrush, rect.Left, rect.Bottom );
        g.DrawString( s2, f, labelBrush, rect.Right - w2, rect.Bottom );
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
    /// Returns the smallest value in the list.
    /// </summary>
    /// <param name="metricValues"></param>
    /// <returns></returns>
    private double GetMinValue( List<double> metricValues ) {
        double min = 0;
        foreach( double val in metricValues ) {
            min = Math.Min( val, min );
        }
        return min;
    }

    /// <summary>
    /// Returns the average of the values in the list.
    /// </summary>
    /// <param name="metricValues"></param>
    /// <returns></returns>
    private double GetAverageValue( List<double> metricValues ) {
        double avg = 0;
        foreach( double val in metricValues ) {
            avg += val;
        }
        if( metricValues.Count > 0 ) {
            avg = avg / (double)metricValues.Count;
        }
        return avg;
    }

    /// <summary>
    /// Returns the total of the values in the list.
    /// </summary>
    /// <param name="metricValues"></param>
    /// <returns></returns>
    private double GetTotalValue( List<double> metricValues ) {
        double tot = 0;
        foreach( double val in metricValues ) {
            tot += val;
        }
        return tot;
    }

    /// <summary>
    /// Returns the max value for the Y axis of a graph.  Value will be one of 1, 2, 5, 10, 20, 50, ...
    /// </summary>
    /// <param name="dataMaximum"></param>
    /// <returns></returns>
    private double YAxisMax( double dataMaximum ) {
        double tensPower = 0.000001;
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

    private string SetupGraphLabels( out double graphMax, out string pctUnits, out double pctFac ) {
        double dataMax = GetMaxValue( data.metrics[ this.dataMetricIndex ].values );
        double dataMin = GetMinValue( data.metrics[ this.dataMetricIndex ].values );
        double dataAvg = GetAverageValue( data.metrics[ this.dataMetricIndex ].values );
        double dataSum = GetTotalValue( data.metrics[ this.dataMetricIndex ].values );

        bool displayTotal = false;
        if( this.compareView == true ) {
            dataMax = Math.Max( dataMax, GetMaxValue( comparisonData.metrics[ this.comparisonDataMetricIndex ].values ) );
            dataMin = Math.Min( dataMin, GetMinValue( comparisonData.metrics[ this.comparisonDataMetricIndex ].values ) );
        }
        
        graphMax = YAxisMax( dataMax );

        string graphTypeName = data.metrics[ this.dataMetricIndex ].Type;
        if( graphTypeName == null || graphTypeName == "" ) {
            graphTypeName = String.Format( "Metric {0}", this.metricNumber );
        }

        pctFac = 100;
        pctUnits = "%";

        if( graphTypeName == "Persuasion" || 
            graphTypeName == "MarketIndex2" || 
            graphTypeName == "TotalActions" ||
            graphTypeName.Contains("GRP") )
        {
            pctFac = 1;
            pctUnits = "";
            displayTotal = true;
        }

        string graphName = "";

        // add the analysis data
        if( dataMin * pctFac > 1 ) {
            graphName += String.Format( "\r\nMin = {0:f1}{1}", dataMin * pctFac, pctUnits );
        }
        else if( dataMin > 0 ) {
            graphName += String.Format( "\r\nMin = {0:f3}{1}", dataMin * pctFac, pctUnits );
        }
        else  {
            graphName += String.Format( "\r\nMin = {0:f0}{1}", 0, pctUnits );
        }

        graphName += "\r\n";

        // add the analysis data
        if( dataMax * pctFac > 1 ) {
            graphName += String.Format( "\r\nMax = {0:f1}{1}", dataMax * pctFac, pctUnits );
        }
        else if( dataMax > 0 ) {
            graphName += String.Format( "\r\nMax = {0:f3}{1}", dataMax * pctFac, pctUnits );
        }
        else {
            graphName += String.Format( "\r\nMax={0:f0}{1}", 0, pctUnits );
        }

        graphName += "\r\n";

        if( dataAvg * pctFac > 1 ) {
            graphName += String.Format( "\r\nAvg={0:f1}{1}", dataAvg * pctFac, pctUnits );
        }
        else if( dataAvg > 0 ) {
            graphName += String.Format( "\r\nAvg={0:f3}{1}", dataAvg * pctFac, pctUnits );
        }
        else {
            graphName += String.Format( "\r\nAvg={0:f0}{1}", 0, pctUnits );
        }

        graphName += "\r\n";

        if( displayTotal )
        {
            if( dataSum > 0 )
            {
                graphName += String.Format( "\r\nTotal={0:f3}{1}", dataSum * pctFac, pctUnits );
            }
            else
            {
                graphName += String.Format( "\r\nTotal={0:f0}{1}", 0, pctUnits );
            }
        }

        return graphName;
    }

    /// <summary>
    /// Draws the main label area for a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawGraphYAxis( Graphics g, Rectangle rect, double maxVal, string pctUnits ) {
        g.FillRectangle( Brushes.White, rect );
        g.DrawLine( yAxisPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom );
        g.DrawLine( yAxisPen, rect.Right, rect.Bottom, rect.Right, rect.Top );
        g.DrawLine( yAxisPen, rect.Right, rect.Top, rect.Right - 20, rect.Top );
        string maxStr = "";
        if( maxVal >= 1 ) {
            maxStr = String.Format( "{0:f0}{1}", maxVal, pctUnits );
        }
        else if( maxVal > 0.01 ) {
            maxStr = String.Format( "{0:f2}{1}", maxVal, pctUnits );
        }
        else if( maxVal > 0 ) {
            maxStr = String.Format( "{0:0.0e0}{1}", maxVal, pctUnits );
        }
        else {
            maxStr = String.Format( "{0:f0}{1}", 0, pctUnits );
        }

        g.DrawString( maxStr, yAxisLabelFont, labelBrush, rect.Left, 2 + rect.Top );
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

            if( err == null ) {
                // get the metric index
                string metricNum = Request[ "m" ];
                try {
                    this.metricNumber = Convert.ToInt32( metricNum );
                }
                catch( Exception ) {
                    err = "Unable to parse metric index:  " + metricNum;
                }
            }

            if( err == null ) {
                // get the metric index
                string regionNum = Request[ "r" ];
                try {
                    this.regionNumber = Convert.ToInt32( regionNum );
                }
                catch( Exception ) {
                    err = "Unable to parse region index:  " + regionNum;
                }
            }

            if (err != null)
            {
                return err;
            }

            string metricType = data.metrics[ metricNumber ].Type;  // is this right??
            string regionName = this.currentMediaPlan.Specs.GeoRegionNames[ regionNumber ];
            string segmentName = this.currentMediaPlan.Specs.Demographics[ segmentNumber ].DemographicName;
            string demoIdentifier = segmentName + "@" + regionName;

            // find out what metric, segment, and region we are displaying
            for( this.dataMetricIndex = 0; this.dataMetricIndex < data.metrics.Count; this.dataMetricIndex++ ) {
                if( data.metrics[ this.dataMetricIndex ].Type == metricType && data.metrics[ this.dataMetricIndex ].Segment == demoIdentifier ) {
                    // we found it
                    break;
                }
            }
            if( this.dataMetricIndex == data.metrics.Count ) {

                // try to find old style metric
                // find out what metric, segment, and region we are displaying
                for( this.dataMetricIndex = 0; this.dataMetricIndex < data.metrics.Count; this.dataMetricIndex++ )
                {
                    if( data.metrics[this.dataMetricIndex].Type == metricType && data.metrics[this.dataMetricIndex].Segment == segmentName )
                    {
                        // we found it
                        break;
                    }
                }

                if( this.dataMetricIndex == data.metrics.Count )
                {

                    err = "Unable to find metric:  " + metricType + " for " + segmentName;
                }
            }

            this.compareView = false;
            if( err == null ) {
                // get the comparison plan version
                this.comparisonVersion = Request[ "c" ];
                if( this.comparisonVersion != null && this.comparisonVersion != "" && this.comparisonVersion != "0" ){
                    this.compareView = true;

                    // get the comparison data
                    PlanStorage storage = new PlanStorage();
                    MediaPlan comparisonPlan = storage.LoadMediaPlan( Utils.GetUser( this ), this.mediaPlan.CampaignName, this.comparisonVersion );
                    if( comparisonPlan != null ) {
                        if( comparisonPlan.Results != null ) {
                            this.comparisonData = comparisonPlan.Results;

                            // set up the comparison-data index
                            Metric chkMetric = comparisonData.metrics[ this.dataMetricIndex ];
                            if( chkMetric.Type == metricType && chkMetric.Segment == demoIdentifier ) {
                                // the metrics use the same indexing 
                                this.comparisonDataMetricIndex = this.dataMetricIndex;
                            }
                            else {
                                for( this.comparisonDataMetricIndex = 0; this.comparisonDataMetricIndex < comparisonData.metrics.Count; this.comparisonDataMetricIndex++ ) {
                                    if( comparisonData.metrics[ this.comparisonDataMetricIndex ].Type == metricType && 
                                        comparisonData.metrics[ this.comparisonDataMetricIndex ].Segment == demoIdentifier ) {
                                        // we found it
                                        break;
                                    }
                                }

                                if( this.comparisonDataMetricIndex == comparisonData.metrics.Count )
                                {

                                    for( this.comparisonDataMetricIndex = 0; this.comparisonDataMetricIndex < comparisonData.metrics.Count; this.comparisonDataMetricIndex++ )
                                    {
                                        if( comparisonData.metrics[this.comparisonDataMetricIndex].Type == metricType &&
                                            comparisonData.metrics[this.comparisonDataMetricIndex].Segment == segmentName )
                                        {
                                            // we found it
                                            break;
                                        }
                                    }
                                    if( this.comparisonDataMetricIndex == comparisonData.metrics.Count )
                                    {
                                        err = "Unable to find metric in comparison data:  " + metricType + " for " + segmentName;
                                    }
                                }
                            }
                        }
                        else {
                            err = "No results in comparison plan, version:  " + this.comparisonVersion;
                        }
                    }
                    else {
                        err = "Unable to load comparison plan, version:  " + this.comparisonVersion;
                    }
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
    private void FinishUp( Bitmap objBitmap, Graphics objGraphics ) 
    {
        Response.Clear();
        Response.ContentType = "image/png";

        using( MemoryStream stmMemory = new MemoryStream() )
        {
            objBitmap.Save( stmMemory, ImageFormat.Png );
            stmMemory.WriteTo( Response.OutputStream );
          
        }

       // Save the image to the OutputStream
       // objBitmap.Save( Response.OutputStream, ImageFormat.Png );

        Response.End();

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
