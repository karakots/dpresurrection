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

using WebLibrary;
using MediaLibrary;

public partial class MediaPieChart : System.Web.UI.Page
{
    private const int width = 150;
    private const int height = 150;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;

    private MediaPlan mediaPlan;

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

        Rectangle rect = new Rectangle( 20, 20, 110, 110 );

        objGraphics.FillRectangle( new SolidBrush( Color.White ), 0, 0, width, height );

        Dictionary<MediaVehicle.MediaType, double> mediaBudgetFracs = this.mediaPlan.GetTypeFractions();

        // sort the fractions
        double[] vals = new double[ mediaBudgetFracs.Count ];
        MediaVehicle.MediaType[] typs = new MediaVehicle.MediaType[ mediaBudgetFracs.Count ];
        int i = 0;
        foreach( MediaVehicle.MediaType mtype in mediaBudgetFracs.Keys ) {
            typs[ i ] = mtype;
            vals[ i ] = mediaBudgetFracs[ mtype ];
            i++;
        }
        Array.Sort( vals, typs );
        Array.Reverse( vals );
        Array.Reverse( typs );

        double angle = 0;
        double angleRange = 0;

        for( int m = 0; m < typs.Length; m++ ) {
            Color c = Utils.MediaBackgroundColor( typs[ m ] );
            Brush b = new SolidBrush( c );
            angleRange = vals[ m ] * 360.0;

            objGraphics.FillPie( b, rect, (float)angle, (float)angleRange );
            angle += angleRange;
        }

 //       Pen p = new Pen( Brushes.Black, 3 );
//        objGraphics.DrawEllipse( p, rect );
        //string[] graphNames = new string[] { "Persuasion", "Awareness", "Recency", "New Metric 4", "New Metric 5" };

        FinishUp( objBitmap, objGraphics );
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
