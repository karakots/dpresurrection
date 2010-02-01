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

public partial class InputTimingGraph : System.Web.UI.Page
{
    private int width = 621;
    private int height = 20;

    private const int margnLeft = 1;
    private const int margnRight = 0;
    private const int margnTop = 0;
    private const int margnBottom = 1;
    private const int dataMarginTop = 2;
    private const int dataMarginBottom = 0;
    private int dataYGap = 2;
    private int dataMaxHeight = 5;
    private const int spaceBetweenPanels = 1;

    private int panel1Width = 36;

    private MediaPlan mediaPlan;

    private int graphAreaHeight;

    private int styleCode = 0;              // 0=plan-editor, 1=report

    //private bool noResultsForPlan = false;

    //private Brush graphBrush = new SolidBrush( Color.FromArgb( 200, 230, 55, 55 ) );
    //private Pen graphPen = new Pen( new SolidBrush( Color.FromArgb( 230, 55, 55 ) ), 3.0f );
    //private Pen graphAreaBorderPen = new Pen( new SolidBrush( Color.FromArgb( 240, 200, 0 ) ), 1.0f );

    protected Font labelFont = new Font("Arial Narrow", 8f, FontStyle.Regular);
    //protected Font yAxisLabelFont = new Font( "Arial Narrow", 8f, FontStyle.Regular );
    protected Brush labelBrush = new SolidBrush(Color.Black);
    protected Pen yAxisPen = new Pen(new SolidBrush(Color.Black));
    protected Color bkgColor = Color.White;

    // session-level variables
    protected MediaPlan currentMediaPlan;
    protected List<MediaPlan> currentMediaPlans;
    protected List<Guid> runningPlans;
    protected bool engineeringMode;
    protected List<MediaPlan> plansBeingEdited;

    private void AdjustLayoutForStyle()
    {
        if (this.styleCode == 1)
        {
            panel1Width = 160;
            width = 689;
        }
    }

    void Page_Load(Object sender, EventArgs e)
    {
        InitializeVariables();

        string err = GetInputs();
        AdjustLayoutForStyle();

        // Since we are outputting a Jpeg, set the ContentType appropriately
        Response.ContentType = "image/jpeg";

        Bitmap objBitmap = new Bitmap(width, height);
        Graphics objGraphics = Graphics.FromImage(objBitmap);

        if (err != null)
        {
            objGraphics.DrawString(err, new Font("Arial", 10f), Brushes.Red, new PointF(20, 20));
            FinishUp(objBitmap, objGraphics);
            return;
        }

        this.graphAreaHeight = height - margnTop - margnBottom;
        objGraphics.FillRectangle(new SolidBrush(bkgColor), 0, 0, width, height);

        int graphWidth = width - margnLeft - margnRight - panel1Width - spaceBetweenPanels;

        Rectangle infoRect = new Rectangle(margnLeft, margnTop, panel1Width, graphAreaHeight);
        Rectangle graphRect = new Rectangle(margnLeft + panel1Width + spaceBetweenPanels, margnTop, graphWidth, graphAreaHeight);

        DrawGraphInfo(objGraphics, infoRect);
        DrawGraphBackground(objGraphics, graphRect);
        DrawGraphValues(objGraphics, graphRect);

        FinishUp(objBitmap, objGraphics);
    }

    private void DrawGraphBackground(Graphics g, Rectangle rect)
    {
        double planDurationDays = (this.mediaPlan.EndDate - this.mediaPlan.StartDate).TotalDays;
        int startMonth = this.mediaPlan.StartDate.Month;
        int nMonths = this.mediaPlan.EndDate.Month - this.mediaPlan.StartDate.Month + 12 * (this.mediaPlan.EndDate.Year - this.mediaPlan.StartDate.Year);
        if (this.mediaPlan.EndDate.Day < this.mediaPlan.StartDate.Day)
        {
            nMonths -= 1;
        }

        string[] monthLetter = new string[] { "D", "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };
        Font f = new Font("Arial", 12f);
        Brush b = new SolidBrush(Color.FromArgb(255, 255, 255));

        if (nMonths == 0)
        {
            // all one month
            g.FillRectangle(new SolidBrush(MonthColor(startMonth)), rect);
            g.DrawString(monthLetter[startMonth], f, b, rect.X + 1, 1);
            return;
        }

        if (nMonths == 1)
        {
            // two months visible
            DateTime m0 = new DateTime(this.mediaPlan.StartDate.Year, this.mediaPlan.StartDate.Month, 1).AddMonths(1);
            int firstDays = (int)Math.Round((m0 - this.mediaPlan.StartDate).TotalDays);
            double firstFrac = firstDays / planDurationDays;
            int w1 = (int)Math.Round(rect.Width * firstFrac);

            Rectangle r1 = new Rectangle(rect.X, rect.Y, w1, rect.Height);
            Rectangle r2 = new Rectangle(rect.X + w1 + 1, rect.Y, rect.Width - w1 - 1, rect.Height);
            g.FillRectangle(new SolidBrush(MonthColor(startMonth)), r1);
            g.FillRectangle(new SolidBrush(MonthColor(startMonth + 1)), r2);
            g.DrawString(monthLetter[startMonth], f, b, r1.X + 1, 1);
            g.DrawString(monthLetter[(startMonth + 1) % 12], f, b, r2.X + 1, 1);
            return;
        }
        else
        {
            // there are three or more months visible
            DateTime m0 = new DateTime(this.mediaPlan.StartDate.Year, this.mediaPlan.StartDate.Month, 1).AddMonths(1);
            DateTime m999 = new DateTime(this.mediaPlan.EndDate.Year, this.mediaPlan.EndDate.Month, 1);
            int firstDays = (int)Math.Round((m0 - this.mediaPlan.StartDate).TotalDays);
            int lastDays = (int)Math.Round((this.mediaPlan.EndDate - m999).TotalDays);
            double firstFrac = firstDays / planDurationDays;
            double lastFrac = lastDays / planDurationDays;
            int w1 = (int)Math.Round(rect.Width * firstFrac);
            int w999 = (int)Math.Round(rect.Width * lastFrac);

            Rectangle r1 = new Rectangle(rect.X, rect.Y, w1, rect.Height);
            Rectangle r999 = new Rectangle(rect.X + rect.Width - w999, rect.Y, w999, rect.Height);
            g.FillRectangle(new SolidBrush(MonthColor(startMonth)), r1);
            g.FillRectangle(new SolidBrush(MonthColor(startMonth + nMonths)), r999);
            g.DrawString(monthLetter[startMonth], f, b, r1.X + 1, 1);
            g.DrawString(monthLetter[(startMonth + nMonths) % 12], f, b, r999.X + 1, 1);

            for (int m = 1; m < nMonths; m++)
            {
                DateTime m1 = m0.AddMonths(m - 1);
                DateTime m2 = m0.AddMonths(m);
                int d1 = (int)Math.Round((m1 - this.mediaPlan.StartDate).TotalDays);
                int d2 = (int)Math.Round((m2 - this.mediaPlan.StartDate).TotalDays);
                double d1Frac = d1 / planDurationDays;
                double d2Frac = d2 / planDurationDays;
                int x = (int)Math.Round((double)rect.Width * d1Frac);
                int w = (int)Math.Round((double)rect.Width * (d2Frac - d1Frac)) + 1;
                Rectangle rr = new Rectangle(rect.X + x, rect.Y, w, rect.Height);
                g.FillRectangle(new SolidBrush(MonthColor(startMonth + m)), rr);
                g.DrawString(monthLetter[(startMonth + m) % 12], f, b, rect.X + x + 1, 1);
            }
            return;
        }
    }

    private Color MonthColor(int monthNum)
    {
        if ((monthNum % 2) == 0)
        {
            return Color.FromArgb(200, 200, 200);
        }
        else
        {
            return Color.FromArgb(220, 220, 220);
        }
    }

    /// <summary>
    /// Draws the input media timing events as a graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawGraphValues(Graphics g, Rectangle rect)
    {

        double planDurationDays = (this.mediaPlan.EndDate - this.mediaPlan.StartDate).TotalDays;
        int nItems = mediaPlan.NumMediaItems;

        int yStep = (int)Math.Round(Math.Min((rect.Height - (dataMarginTop + dataMarginBottom)) / (double)nItems, dataMaxHeight + dataYGap));
        int y = margnTop + dataMarginTop;
        int yH = yStep - dataYGap;
        Color timingDataColor = Color.FromArgb(0, 0, 100);

        List<MediaItem> items = this.mediaPlan.GetAllItems();
        foreach (MediaItem item in items)
        {
            List<DateTime> dates = item.GetAdDates();

            if (dates.Count > 0)
            {
                foreach (DateTime spotDate in dates)
                {
                    double spotDay = (spotDate - this.mediaPlan.StartDate).TotalDays;
                    double xFrac = spotDay / planDurationDays;

                    int x = (int)Math.Round((rect.Width - 1) * xFrac) + rect.Left;
                    Pen pen = new Pen(new SolidBrush(timingDataColor), 3.0f);

                    g.DrawLine(pen, x + 1, y, x + 1, y + yH);

                }
                y += yStep;
            }


        }
    }

    /// <summary>
    /// Draws the info panel of the graph.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rect"></param>
    /// <param name="text"></param>
    private void DrawGraphInfo(Graphics g, Rectangle rect)
    {
        g.FillRectangle(Brushes.White, rect);
        if (this.styleCode == 0)
        {
            g.DrawString("Timing:", labelFont, labelBrush, rect.Left, 2 + rect.Top);
        }
        else if (this.styleCode == 1)
        {
            g.DrawString("Timing of Ads:", labelFont, labelBrush, rect.Left, 2 + rect.Top);
        }
    }

    /// <summary>
    /// Gets the inputs form the URL (plan ID and segment number).  Returns null if OK; error details if there was a problem.
    /// </summary>
    /// <returns></returns>
    private string GetInputs()
    {
        string planID = Request["p"];
        string err = null;

        if (planID == null)
        {
            err = "NULL planID";
        }
        else
        {
            // get the plan 
            this.mediaPlan = Utils.PlanForID( new Guid( planID ), this.currentMediaPlans );
            if (this.mediaPlan == null)
            {
                err = "No media plan with ID: " + planID;
                //this.mediaPlan = Utils.PlanForID(new Guid(planID), this.plansBeingEdited);
                //if (this.mediaPlan == null)
                //{
                //    err = "No media plan with ID: " + planID;
                //}
            }
        }

        string styleStr = Request["s"];
        if (styleStr != null)
        {
            try
            {
                this.styleCode = int.Parse(styleStr);
            }
            catch (Exception)
            {
                err = "Invalid style index (s): " + styleStr;
            }
        }

        return err;
    }

    /// <summary>
    /// Commits the drawing to the output
    /// </summary>
    /// <param name="objBitmap"></param>
    /// <param name="objGraphics"></param>
    private void FinishUp(Bitmap objBitmap, Graphics objGraphics)
    {
        // Save the image to the OutputStream
        objBitmap.Save(Response.OutputStream, ImageFormat.Jpeg);

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
