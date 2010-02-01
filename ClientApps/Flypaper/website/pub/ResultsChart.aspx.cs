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
using BusinessLogic;

public partial class ResultsGraphOne : System.Web.UI.Page
{
    private const int width = 600;
    private int height = 200;

    private Color lineColor = Color.FromArgb( 200, 200, 200 );
    private Color borderColor = Color.FromArgb( 180, 180, 180 );

    private const int margnLeft = 10;
    private const int margnRight = 10;
    private const int margnTop = 10;
    private const int margnBottom = 10;
    private const int spaceBetweenGraphs = 10;

    private const int segNameColumnWidth = 170;
    private const int titleRowHeight = 41;
    private int lastColExpansion = 90;
    private const int mainColsWidth = 90;

    private MediaPlan mediaPlan;
    private SimOutput data;
    private int segmentNumber = 0;
    private string segmentName = null;
    private int rowCount = 0;

    private double overallStars = 0;
    private bool showAllMetrics = false;

    private Scoring scoringService;

    //protected Font labelFont = new Font( "Arial", 9f, FontStyle.Bold );
    //protected Font yAxisLabelFont = new Font( "Arial", 8f, FontStyle.Bold );
    protected Brush labelBrush = new SolidBrush( Color.Black );
    protected Pen yAxisPen = new Pen( new SolidBrush( Color.Black ) );
    protected Color bkgColor = Color.White;
    protected Font legendlFont = new Font( "Arial", 8f, FontStyle.Italic );

    protected List<Color> colColors;

    protected System.Drawing.Image starImage;
    protected System.Drawing.Image starEmptyImage;

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

        // make sure we have space for the legend
        if( this.segmentNumber == -1 && this.mediaPlan.Specs.SegmentList.Count > 2 ) {
            SizeF legRow = objGraphics.MeasureString( "Segment", this.legendlFont );
            this.height += (int)Math.Round( legRow.Height * (this.mediaPlan.Specs.SegmentList.Count - 2) );
            objBitmap = new Bitmap( width, height );
            objGraphics = Graphics.FromImage( objBitmap );
        }

        string dir = HttpContext.Current.Server.MapPath( null );
        string starImgFile = dir + @"\images\star19.gif";
        string starEmptyImgFile = dir + @"\images\star19Blank.gif";
        starImage = Bitmap.FromFile( starImgFile );
        starEmptyImage = Bitmap.FromFile( starEmptyImgFile );

        if( err != null ) {
            objGraphics.Clear( Color.White );
            objGraphics.DrawString( err, new Font( "Arial", 8f ), Brushes.Black, new PointF( 20, 20 ) );
            FinishUp( objBitmap, objGraphics );
            return;
        }

        // fill the background
        //this.graphAreaHeight = height - margnTop - margnBottom;

        objGraphics.FillRectangle( new SolidBrush( bkgColor ), 0, 0, width, height );
        
        // get the data
        this.rowCount = this.currentMediaPlan.Specs.CampaignGoals.Count;
        if( this.showAllMetrics == false ) {
            while( this.currentMediaPlan.Specs.GoalWeights[ this.rowCount - 1 ] == 0 ) {
                this.rowCount -= 1;
            }
        }

        List<List<double>> allData = new List<List<double>>();
        for( int r = 0; r < this.rowCount; r++ ) {
            List<double> rowData = GetResultsRowData( r );
            allData.Add( rowData );
        }

        //// !!!DEBUG !!!
        //if( this.segmentNumber == 999 ) {
        //    Random rand = new Random();
        //    allData = new List<List<double>>();
        //    for( int r = 0; r < 5; r++ ) {
        //        List<double> rowData = new List<double>();
        //        rowData.Add( 0 );
        //        for( int c = 1; c <= 2; c++ ) {
        //            rowData.Add( rand.NextDouble() * 100.0 );
        //            rowData.Add( rand.NextDouble() * 100.0 );
        //        }
        //        rowData[ 0 ] = (rowData[ 1 ] * rowData[ 2 ]) / 100;
        //        allData.Add( rowData );
        //    }
        //}
        //// !!!! END DEBUG !!!

        int chartWidth = width - margnLeft - margnRight;
        DrawResultsChart( objGraphics, allData, chartWidth, 150, margnLeft, margnTop );

        FinishUp( objBitmap, objGraphics );
    }

    private void DrawResultsChart( Graphics g, List<List<double>> allData, int wid, int ht,  int x0, int y0 ) {
        Rectangle r = new Rectangle( x0, y0, wid, ht );
        Pen p = new Pen( lineColor );
        if( this.segmentNumber == -1 ) {
            lastColExpansion = 0;
        }

        int nRows = this.rowCount;
        //int nRows = allData.Count;
        int nCols = allData[ 0 ].Count;

        int yDelta = (ht - titleRowHeight) / nRows;
        if( yDelta > 25 ) {
            yDelta = 25;
        }
        int y = y0;
        for( y = y0 + titleRowHeight; y < y0 + ht; y += yDelta ) {
            g.DrawLine( p, x0, y, x0 + wid, y );
        }
        int yMax = y - yDelta;

        int xDelta = 1 + (wid - segNameColumnWidth - lastColExpansion - (2 * mainColsWidth)) / (nCols - 2);
        int x = x0 + segNameColumnWidth;
        Pen pr = new Pen( Brushes.Red );

        int[] colX = new int[ nCols];
        for( int c = 0; c < nCols; c++ ) {
            colX[ c ] = x;

            if( c < 3 ) {
                g.DrawLine( p, x, y0, x, yMax );
            }
            else {
                g.DrawLine( p, x, y0 + 25, x, yMax );
            }

            if( c < 2 ) {
                x += mainColsWidth;
            }
            else {
                x += xDelta;
            }
        }

        Rectangle rr = new Rectangle( x0, y0, wid, yMax - y0 );
        g.DrawRectangle( new Pen( borderColor ), rr );

        DrawTitleCell( g, "Plan\r\nScore", colX[ 0 ], yDelta, y0, y0 );
        DrawTitleCell( g, "Goal\r\nImportance", colX[ 1 ], yDelta, y0, y0 );

        if( this.segmentNumber != -1 ) {
            string title3 = "Current Score (max = 100)\r\n" + this.segmentName;
            DrawTitleCell( g, title3, colX[ 2 ], yDelta, y0, y0 );
        }
        else {
            string title3 = "Current Score (max = 100)\r\nall";
            DrawTitleCell( g, title3, colX[ 2 ], yDelta, y0, y0 );
            for( int s = 0; s < this.mediaPlan.SegmentCount; s++ ) {
                string segTitle = "seg " + (s + 1).ToString();
                DrawTitleCell( g, "\r\n" + segTitle, colX[ s + 3 ] + 4, yDelta, y0, y0 );
            }
        }

        int planTotal = 0;
        for( int row = 0; row < nRows + 1; row++ ) {

            string rowTitle = "Total";
            if( row < nRows ) {
                rowTitle = "Goal " + (row + 1).ToString();
                if( row < this.mediaPlan.Specs.CampaignGoals.Count ) {
                    rowTitle = this.mediaPlan.Specs.CampaignGoals[ row ].ToString();
                    if( rowTitle == "ReachAndAwareness" ) {
                        rowTitle = "Reach & Awareness";
                    }
                }
            }

            DrawRowTitleCell( g, rowTitle, row, xDelta, yDelta, y0, y0 );

            int cellWidth = 0;
            for( int col = 0; col < nCols; col++ ) {
                int ci = col % colColors.Count;

                Color c1 = colColors[ ci ];
                if( col < nCols - 1 ) {
                    cellWidth = colX[ col + 1 ] - colX[ col ];
                }
                else if( col == nCols - 1 ) {
                    cellWidth = r.Right - colX[ col ];
                }

                if( row < nRows ) {
                    if( col <= 1 && allData[ row ][ 1 ] == 0 ) {
                        continue;
                    }
                    if( col == 0 ) {
                        planTotal += (int)Math.Round( allData[ row ][ col ] );
                    }

                    DrawCell( g, row, colX[ col ], (int)Math.Round( allData[ row ][ col ] ), 100, c1, cellWidth, yDelta, y0, y0 );
                }
                else {
                    if( col == 0 ) {
                        DrawCell( g, row, colX[ col ], planTotal, 100, Color.White, cellWidth, yDelta, y0, y0 );
                        string legend = "";
                        if( this.segmentNumber == -1 ) {
                            for( int s = 0; s < this.mediaPlan.Specs.SegmentList.Count; s++ ) {
                                legend += GetSegString( this.mediaPlan.Specs.SegmentList[ s ].Name, s + 1 ) + "\r\n";
                            }
                        }
                        //else {
                        //    legend += GetSegString( this.mediaPlan.Specs.SegmentList[ this.segmentNumber ].Name, 0 );
                        //}

                        int legendRow = (int)Math.Max( row, 4 );
                        DrawCellTextOnly( g, legendRow, colX[ 2 ], legend, yDelta, y0, legendlFont );
                    }
                }
            }
        }

        DrawStars( g, x0, y0 );
    }

    private string GetSegString( string segName, int segNum ) {
        string[] split = segName.Split( '@' );
        string demo_name = split[ 0 ];

        string final_name = demo_name;
        if( split.Length > 1 ) {
            string region_name = split[ 1 ];
            final_name = String.Format( "{0} -- {1}", demo_name, region_name );
        }

        if( segNum == 0 ) {
            final_name = String.Format( "Segment: {0}", final_name );
        }
        else {
            final_name = String.Format( "seg {0}: {1}", segNum, final_name );
        }
        return final_name;
    }

    private void DrawStars( Graphics g, int x, int y ) {
        int sx = x + 10;
        int sy = y + 10;

        Pen p = new Pen( Color.Black );
        int i = 0;
        for( ; i < (int)this.overallStars; i++ ) {
            if( this.starImage != null ) {
                g.DrawImage( this.starImage, sx, sy );
            }
            else {
                g.DrawString( "*", new Font( "Arial", 12.0f ), new SolidBrush( Color.Black ), new PointF( sx, sy ) );
            }
            sx += 20;
        }
        for( ; i < 5; i++ ) {
            if( this.starEmptyImage != null ) {
                g.DrawImage( this.starEmptyImage, sx, sy );
            }
            sx += 20;
        }
    }

    private void DrawTitleCell( Graphics g, string text, int x, int cellHeight, int x0, int y0 ) {
        //int x = x0 + (col * cellWidth) + segNameColumnWidth;
        int y = y0;
        Font f = new Font( "Verdana", 8.0f );
        Brush b = new SolidBrush( Color.Black );
        g.DrawString( text, f, b, x + 3, y + 3 );
    }

    private void DrawRowTitleCell( Graphics g, string text, int row, int cellWidth, int cellHeight, int x0, int y0 ) {
        int x = x0;
        int y = y0 + (row * cellHeight) + titleRowHeight;
        Font f = new Font( "Verdana", 8.0f );
        Brush b = new SolidBrush( Color.Black );
        g.DrawString( text, f, b, x + 2, y + 4 );
    }

    private void DrawCellTextOnly( Graphics g, int row, int x, string text, int cellHeight, int y0, Font f ) {
        int y = y0 + (row * cellHeight) + titleRowHeight;
        Brush b = new SolidBrush( Color.Black );
        g.DrawString( text, f, b, x + 4, y + 6 );
    }

    private void DrawCell( Graphics g, int row, int x, int val, int maxVal, Color c, int cellWidth, int cellHeight, int x0, int y0 ) {

        int y = y0 + (row * cellHeight) + titleRowHeight;

        Brush b = new SolidBrush( c );
        int barWid = (int)Math.Round(  (cellWidth - 4) * (val / (double)maxVal) );

        int fadeWid = 15;
        if( barWid < fadeWid ) {
            fadeWid = barWid;
        }

        g.FillRectangle( b, x + 3, y + 3, barWid - fadeWid, cellHeight - 5 );

        DrawFade( g, c, x + 3 + barWid - fadeWid, y + 3, fadeWid, cellHeight - 5 );


        Font f = new Font( "Verdana",  8.0f, FontStyle.Bold );
        Brush b2 = new SolidBrush( Color.Black );
        g.DrawString( val.ToString(), f, b2, x + 4, y + 6 );
    }

    private void DrawFade( Graphics gr, Color c, int x, int y, int fadeWid, int ht ) {
        double r = c.R;
        double g = c.G;
        double b = c.B;

        int nLeft = fadeWid ;
        for( int xx = x; xx < x + fadeWid; xx++ ) {
            double ratio = (double)nLeft / (double)fadeWid;
            int cr = 255 - (int)Math.Round( ratio * (255 - r) );
            int cg = 255 - (int)Math.Round( ratio * (255 - g) );
            int cb = 255 - (int)Math.Round( ratio * (255 - b) );
            Color fc = Color.FromArgb( cr, cg, cb );
            Pen fp = new Pen( fc );
            gr.DrawLine( fp, xx, y, xx, y + ht - 1 );
            nLeft -= 1;
        }
    }

    /// <summary>
    /// Returns plan score, metric weight, and  segment score(s) for the given row index (= goal number)
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    private List<double> GetResultsRowData( int rowIndex ) {
        List<double> resData = new List<double>();
        Dictionary<MediaCampaignSpecs.CampaignGoal, double> scores = null;

        if (this.segmentNumber == -1)
        {
            // overall plan scores
            scores = scoringService.GoalScores();
        }
        else {
            // single segment scores
            scores = scoringService.GoalScores( this.segmentNumber );
        }

        MediaCampaignSpecs.CampaignGoal goal = this.mediaPlan.Specs.CampaignGoals[ rowIndex ];
        double segScore = scores[ goal ];

        double goalWeight = 100;
        //!!! this "if" test needed only to handle old-version plans that have missing goal weights (otherwise this will always be the case)
        if( rowIndex < this.mediaPlan.Specs.GoalWeights.Count ) {
            goalWeight = this.mediaPlan.Specs.GoalWeights[ rowIndex ];
        }

        double planScore = goalWeight * segScore;

        resData.Add( planScore );
        resData.Add( goalWeight );
        resData.Add( segScore * 100 );

        if( this.segmentNumber == -1 ) {
            for( int s = 0; s < this.mediaPlan.Specs.SegmentList.Count; s++ ) {
                Dictionary<MediaCampaignSpecs.CampaignGoal, double> segScores = scoringService.GoalScores( s );
                double segSegScore = segScores[ goal ];
                resData.Add( segSegScore * 100 );
            }
        }

        return resData;
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
        }

        this.scoringService = new Scoring( this.mediaPlan );

        // get the segment number
        if( err == null ) {
            this.segmentNumber = 0;
            if( Request[ "s" ] != null ) {
                if( Request[ "s" ] == "X" ) {
                    // use the default
                    this.segmentNumber = -1;
                }
                else {
                    this.segmentNumber = int.Parse( Request[ "s" ] );
                }
            }
        }

        if( this.mediaPlan.SegmentCount == 1 ) {
            this.segmentNumber = 0;
        }

        if( Request[ "a" ] == "1" ) {
            this.showAllMetrics = true;
        }

        // get the overall stars rating (uses the scoring service)
        this.overallStars = ComputeOverallStars();

        this.segmentName = null;

        if( this.segmentNumber == -1 ) {
            this.segmentName = "All Segs";
        }
        else if (this.segmentNumber < this.mediaPlan.SegmentCount)
        {
            this.segmentName = this.mediaPlan.Specs.SegmentList[segmentNumber].Name;
        }
        else if( this.segmentNumber == 999 ) {
            this.segmentName = "Test Data";
        }
        else {
            this.segmentName = "Segment " + this.segmentNumber.ToString();
        }

        return err;
    }

    private double ComputeOverallStars() {

        double nStars = 0;
        if( this.segmentNumber == -1 ) {
            nStars = this.scoringService.GetRatingStars();
        }
        else {
            nStars = this.scoringService.GetRatingStars( this.segmentNumber );
        }

        return nStars;
    }

    /// <summary>
    /// Commits the drawing to the output
    /// </summary>
    /// <param name="objBitmap"></param>
    /// <param name="objGraphics"></param>
    private void FinishUp( Bitmap objBitmap, Graphics objGraphics ) {

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

        // Save the image to the OutputStream
        // objBitmap.Save( Response.OutputStream, ImageFormat.Jpeg );

        // clean up...
        objGraphics.Dispose();
        objBitmap.Dispose();
    }

    private void InitializeVariables() {
        this.currentMediaPlan = Utils.CurrentMediaPlan( this, false );
        this.currentMediaPlans = Utils.CurrentMediaPlans( this );
        this.engineeringMode = Utils.InEngineeringMode( this );

        colColors = new List<Color>();
        colColors.Add( Color.FromArgb( 150, 150, 230 ) );
        colColors.Add( Color.FromArgb( 140, 230, 140 ) );
        colColors.Add( Color.FromArgb( 230, 160, 160 ) );
        colColors.Add( Color.FromArgb( 150, 230, 230 ) );
        colColors.Add( Color.FromArgb( 230, 150, 230 ) );
        colColors.Add( Color.FromArgb( 230, 200, 230 ) );

    }
}
