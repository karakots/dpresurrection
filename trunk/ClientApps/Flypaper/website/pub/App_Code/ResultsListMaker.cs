using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using HouseholdLibrary;
using BusinessLogic;
using MediaLibrary;
using WebLibrary;

/// <summary>
/// Summary description for ResultsListMaker
/// </summary>
public class ResultsListMaker
{
    private const string sectionHeaderBorderrColor = "#0077AA";

    private const int MAX_VEHICLES_PER_SEGMENT = 5;

    private const int totalCols = 6;

    private AllocationService allocation_service;
    private Scoring scoring_service;
    private MediaPlan plan;

    public ResultsListMaker(MediaPlan plan)
    {
        scoring_service = new Scoring(plan);
        allocation_service = new AllocationService();
        this.plan = plan;
    }
   
    /// <summary>
    /// Adds the HTML for the simulation results of the given plan(s) shopping list to the given DIV.
    /// </summary>
    /// <param name="editDiv"></param>
    /// <param name="plan"></param>
    /// <param name="footerHtml"></param>
    /// <param name="engineeringMode"></param>
    public string AddResultsListHTML( HtmlGenericControl editDiv, string footerHtml, bool engineeringMode)
    {
        Table table = new Table();
        table.CellSpacing = 0;
        table.CellPadding = 2;

        table.Style.Add( HtmlTextWriterStyle.FontFamily, "Arial" );
        table.Style.Add( HtmlTextWriterStyle.FontSize, "10pt" );
        table.Style.Add( HtmlTextWriterStyle.Width, "700px" );
        table.Style.Add( HtmlTextWriterStyle.Color, "Black" );

        AddTopBorderForTabs(editDiv, 0);
        
        
        Dictionary<MediaVehicle.MediaType, double> current_allocation = plan.GetTypeFractions();
        Dictionary<MediaVehicle.MediaType, double> suggested_allocation = allocation_service.GetTypeDollarsAllocation( plan );

        // add header row
        AddMainHeader(table, current_allocation, suggested_allocation);

        AddOverallResultsRow(table, true);
        for (int i = 0; i < plan.Specs.SegmentList.Count; i++)
        {
            AddSegmentResultsRow(table, i, true);
        }
               

        List<string> test = new List<string>();

        test.Add("Yippeee!");

        if (test.Contains("Congrats"))
        {
            AddICongratulationsText(table, test, engineeringMode);
        }
          

        editDiv.Controls.Add( table );

       
        AddFooter( editDiv, footerHtml );
       

        return editDiv.ToString();
    }


    private void AddTopBorderForTabs( HtmlGenericControl editDiv, int width ) 
    {

        Table table = new Table();
        table.CellSpacing = 0;
        table.CellPadding = 2;
        table.Style.Add( HtmlTextWriterStyle.Width, "640px" );

        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.Style.Add( HtmlTextWriterStyle.Height, "10px" );
        cell.Style.Add( HtmlTextWriterStyle.Width, "640px" );
        cell.Text = "&nbsp;";

        cell.Style.Add( "border-top", "solid 2px #88c0da" );

        row.Cells.Add( cell );
        table.Rows.Add( row );

        table.Style.Add( HtmlTextWriterStyle.MarginBottom, "10px" );
        editDiv.Controls.Add( table );
    }


    /// <summary>
    /// Adds the main headers
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    private void AddMainHeader( Table table, Dictionary<MediaVehicle.MediaType, double> current_allocation, Dictionary<MediaVehicle.MediaType, double> suggested_allocation ) 
    {
        const int numMedia = 5;
        TableRow row = new TableRow();

        TableCell cell = new TableCell();

        Dictionary<MediaVehicle.MediaType, TableCell> cells = new Dictionary<MediaVehicle.MediaType, TableCell>();
   

        //string rep_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "PlanReportLink", plan.PlanID.ToString() );
        //string repLink = String.Format( "<a href=\"#\" onclick='{0}' >View Full Report</a>", rep_JS );
        //string clone_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "PlanCloneLink", plan.PlanID.ToString() );
        //string cloneLink = String.Format( "<a href=\"#\" onclick='{0}' >Create a Copy of This Plan</a>", clone_JS );
        //cell.Text = String.Format( "{0}<br>{1}<br>{2}", plan.PlanName, repLink, cloneLink );

        foreach( MediaVehicle.MediaType medType in suggested_allocation.Keys)
        {
            cells.Add( medType, new TableCell() );
            cells[medType].Text = "Suggested " + medType.ToString() + " Allocation: " + suggested_allocation[medType];

            if( !current_allocation.ContainsKey( medType ) )
            {
                cells[medType].Text += "<br>Current " + medType.ToString() + " Allocation: " + 0;
            }
        }

        foreach( MediaVehicle.MediaType medType in current_allocation.Keys)
        {
            if( !cells.ContainsKey( medType ) )
            {
                cells.Add(medType, new TableCell());
                cells[medType].Text = "Suggested " + medType.ToString() + " Allocation: 0";
            }

            cells[medType].Text += "<br>Current " + medType.ToString() + " Allocation: " + current_allocation[medType];
        }

        //cell1.Text = "Suggested Radio Allocation: " + suggested_allocation[MediaVehicle.MediaType.Radio] + "<br>Current Radio Allocation: " + current_allocation[MediaVehicle.MediaType.Radio];
        //cell2.Text = "Suggested Internet Allocation: " + suggested_allocation[MediaVehicle.MediaType.Internet] + "<br>Current Internet Allocation: " + current_allocation[MediaVehicle.MediaType.Internet];
        //cell3.Text = "Suggested Magazine Allocation: " + suggested_allocation[MediaVehicle.MediaType.Magazine] + "<br>Current Magazine Allocation: " + current_allocation[MediaVehicle.MediaType.Magazine];
        //cell4.Text = "Suggested Newspaper Allocation: " + suggested_allocation[MediaVehicle.MediaType.Newspaper] + "<br>Current Newspaper Allocation: " + current_allocation[MediaVehicle.MediaType.Newspaper];
        //cell5.Text = "Suggested Yellowpages Allocation: " + suggested_allocation[MediaVehicle.MediaType.Yellowpages] + "<br>Current Yellowpages Allocation: " + current_allocation[MediaVehicle.MediaType.Yellowpages];

        row.Style.Add( HtmlTextWriterStyle.Height, "42px" );
        row.Style.Add( HtmlTextWriterStyle.FontSize, "10pt" );
        row.Style.Add( HtmlTextWriterStyle.TextAlign, "center" );
        cell.Style.Add( HtmlTextWriterStyle.TextAlign, "left" );
        cell.Style.Add( HtmlTextWriterStyle.FontSize, "12pt" );

        row.Cells.Add( cell );

        foreach(TableCell mcell in cells.Values )
        {
            row.Cells.Add( mcell );
        }

        table.Rows.Add( row );
    }

    /// <summary>
    /// Adds the overall rating row for a media plan
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    private double AddOverallResultsRow( Table table, bool engineeringMode) {
        TableRow row = new TableRow();
        TableCell cell1 = new TableCell();
        TableCell cell2 = new TableCell();
        TableCell cell3 = new TableCell();
        TableCell cell4 = new TableCell();
        TableCell cell5 = new TableCell();
        TableCell cell6 = new TableCell();

        cell1.Text = "Overall<br>";

        Dictionary<MediaCampaignSpecs.CampaignGoal, double> scores = scoring_service.GoalScores();
        Dictionary<string, double> metrics = scoring_service.GetMetrics();

        double overall_score = 0.0;
        int num_scores = 0;
        foreach(double score in scores.Values)
        {
            num_scores++;
            overall_score += score;
        }

        overall_score *= 5/num_scores;


        cell1.Text += PlanOverallStarValue( overall_score );

        cell2.Text = ReachString(scores[MediaCampaignSpecs.CampaignGoal.ReachAndAwareness], metrics);
        cell3.Text = PersuasionString(scores[MediaCampaignSpecs.CampaignGoal.Persuasion], metrics);
        cell4.Text = TimingString(scores[MediaCampaignSpecs.CampaignGoal.Recency], metrics);
        cell5.Text = DemoTargetingString(scores[MediaCampaignSpecs.CampaignGoal.DemographicTargeting], metrics);
        cell6.Text = GeoTargetingString(scores[MediaCampaignSpecs.CampaignGoal.GeoTargeting], metrics);

        row.Style.Add( HtmlTextWriterStyle.Height, "42px" );
        row.Style.Add( HtmlTextWriterStyle.FontSize, "11pt" );
        row.Style.Add( HtmlTextWriterStyle.TextAlign, "center" );
        cell1.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );
        cell2.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );
        cell3.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );
        cell4.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );
        cell5.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );
        cell6.Style.Add( "border-bottom", "solid 3px " + sectionHeaderBorderrColor );

        row.Cells.Add( cell1 );
        row.Cells.Add( cell2 );
        row.Cells.Add( cell3 );
        row.Cells.Add( cell4 );
        row.Cells.Add( cell5 );
        row.Cells.Add( cell6 );
        table.Rows.Add( row );

        return overall_score;
    }

    /// <summary>
    /// Adds the  rating row for a media plan segment
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    private void AddSegmentResultsRow( Table table, int seg, bool engineeringMode ) {
        TableRow row = new TableRow();
        TableCell cell1 = new TableCell();
        TableCell cell2 = new TableCell();
        TableCell cell3 = new TableCell();
        TableCell cell4 = new TableCell();
        TableCell cell5 = new TableCell();
        TableCell cell6 = new TableCell();

        cell1.Text = plan.Specs.Demographics[ seg ].DemographicName + "<br>";

        Dictionary<MediaCampaignSpecs.CampaignGoal, double> scores = scoring_service.GoalScores(seg);
        Dictionary<string, double> metrics = scoring_service.GetMetrics(seg);

        double overall_score = 0.0;
        int num_scores = 0;
        foreach (double score in scores.Values)
        {
            num_scores++;
            overall_score += score;
        }

        overall_score *= 5 / num_scores;

        cell1.Text += PlanOverallStarValue(overall_score);
        cell2.Text = ReachString(scores[MediaCampaignSpecs.CampaignGoal.ReachAndAwareness], metrics);
        cell3.Text = PersuasionString(scores[MediaCampaignSpecs.CampaignGoal.Persuasion], metrics);
        cell4.Text = TimingString(scores[MediaCampaignSpecs.CampaignGoal.Recency], metrics);
        cell5.Text = DemoTargetingString(scores[MediaCampaignSpecs.CampaignGoal.DemographicTargeting], metrics);
        cell6.Text = GeoTargetingString(scores[MediaCampaignSpecs.CampaignGoal.GeoTargeting], metrics);

        row.Style.Add( HtmlTextWriterStyle.Height, "42px" );
        row.Style.Add( HtmlTextWriterStyle.FontSize, "11pt" );
        row.Style.Add( HtmlTextWriterStyle.TextAlign, "center" );

        row.Cells.Add( cell1 );
        row.Cells.Add( cell2 );
        row.Cells.Add( cell3 );
        row.Cells.Add( cell4 );
        row.Cells.Add( cell5 );
        row.Cells.Add( cell6 );
        table.Rows.Add( row );
    }

    /// <summary>
    /// Adds the footer to the table
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    private void AddFooter( HtmlGenericControl editDiv, string footerHtml ) {
        Table footerTable = new Table();
        footerTable.Style.Add( HtmlTextWriterStyle.FontFamily, "Arial" );
        footerTable.Style.Add( HtmlTextWriterStyle.FontSize, "10pt" );
        footerTable.Style.Add( HtmlTextWriterStyle.Width, "600px" );

        TableRow footerRow = new TableRow();
        TableCell footerCell = new TableCell();
        footerCell.ColumnSpan = 5;

        footerCell.Text = footerHtml;

        footerRow.Cells.Add( footerCell );
        footerTable.Rows.Add( footerRow );
        editDiv.Controls.Add( footerTable );
    }

    /// <summary>
    /// Adds the congratulatory text for a good plan.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    /// <param name="engineeringMode"></param>
    private void AddICongratulationsText( Table table, List<string> congrats, bool engineeringMode ) {
        TableRow row_tbl = new TableRow();
        TableCell cell_tbl = new TableCell();
        cell_tbl.ColumnSpan = totalCols;

        Table congratTable = new Table();
        SetImprovTableStyle( congratTable );

        cell_tbl.Style.Add( "padding-left", "0px" );

        for( int i = 0; i < congrats.Count; i++ ) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            cell.Text = congrats[ i ];
            cell.ColumnSpan = 3;

            row.Cells.Add( cell );
            congratTable.Rows.Add( row );
        }

        cell_tbl.Controls.Add( congratTable );
        row_tbl.Cells.Add( cell_tbl );
        table.Rows.Add( row_tbl );
    }

    /// <summary>
    /// Adds the text describing suggested plan improvements.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="plan"></param>
    /// <param name="engineeringMode"></param>
    private void AddSuggestionsText( Table table, MediaPlan plan, string reasons, string nextSteps, string problemSummary, bool engineeringMode ) {
        TableRow row_tbl = new TableRow();
        TableCell cell_tbl = new TableCell();
        cell_tbl.ColumnSpan = totalCols;
        cell_tbl.Style.Add( "padding-left", "0px" );

        Table improvTable = new Table();
        SetImprovTableStyle( improvTable );

        TableRow reasonsRow = new TableRow();
        TableCell reasonsCell = new TableCell();
        reasonsCell.Text = reasons;
        reasonsCell.ColumnSpan = 3;
        reasonsRow.Cells.Add( reasonsCell );
        improvTable.Rows.Add( reasonsRow );

        TableRow nextStepsRow = new TableRow();
        TableCell nextStepsCell = new TableCell();
        nextStepsCell.Text = nextSteps;
        nextStepsCell.ColumnSpan = 3;
        nextStepsRow.Cells.Add( nextStepsCell );
        improvTable.Rows.Add( nextStepsRow );

        if( engineeringMode == true ) {
            nextStepsCell.Text += String.Format( " [{0}]", problemSummary.Trim() );
        }

        cell_tbl.Controls.Add( improvTable );
        row_tbl.Cells.Add( cell_tbl );
        table.Rows.Add( row_tbl );
    }

    private void SetImprovTableStyle( Table table ) {
        table.Style.Add( HtmlTextWriterStyle.Width, "600px" );
        //table.Style.Add( HtmlTextWriterStyle.BorderStyle, "solid" );
        //table.Style.Add( HtmlTextWriterStyle.BorderWidth, "1px" );
        //table.Style.Add( HtmlTextWriterStyle.BorderColor, "#005" );
        table.Style.Add( HtmlTextWriterStyle.MarginTop, "40px" );
    }

    /// <summary>
    /// Displays a set of 5 stars, with 0-5 stars highlighted in half-star steps.
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segIndex"></param>
    /// <returns></returns>
    public string PlanOverallStarValue(double stars)
    {

        int nWhole = (int)Math.Min(stars, 5);
        int nHalf = 0;
        if ((stars - nWhole) >= 0.5)
        {
            nHalf = 1;
        }
        int nOff = 5 - nWhole - nHalf;

        string sval = "";
        for( int star = 0; star < nWhole; star++ ) {
            sval += "<img src=\"images/Star.gif\" width=\"20\" height=\"20\" >";
        }
        for( int star = 0; star < nHalf; star++ ) {
            sval += "<img src=\"images/StarHalf.gif\" width=\"20\" height=\"20\" >";
        }
        for( int star = 0; star < nOff; star++ ) {
            sval += "<img src=\"images/StarOff.gif\" width=\"20\" height=\"20\" >";
        }

        return sval;
    }

    /// <summary>
    /// Describes the reach rating of the given segment or overall plan (use segmentIndex = -1 for overall plan)
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segmentIndex"></param>
    /// <param name="engineeringMode"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public string ReachString( double score, Dictionary<string, double> metrics ) 
    {
        string rval = "Reach Score: " + score + "<br>";
        rval += "Reach1: " + metrics["Reach1"] + "<br>";
        rval += "Reach3: " + metrics["Reach3"] + "<br>";
        rval += "Reach3All: " + metrics["Reach3All"] + "<br>";
        rval += "Reach4: " + metrics["Reach4"] + "<br>";
        rval += "Awareness: " + metrics["Awareness"];

        return rval;
    }

    /// <summary>
    /// Describes the persuasion rating of the given segment or overall plan (use segmentIndex = -1 for overall plan)
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segmentIndex"></param>
    /// <param name="engineeringMode"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public string PersuasionString(double score, Dictionary<string, double> metrics) 
    {
        string rval = "Persuasion Score: " + score + "<br>";
        rval += "PersuasionIndex: " + metrics["PersuasionIndex"] + "<br>";
        rval += "MarketIndex1 (not used): " + metrics["MarketIndex1"] + "<br>";
        rval += "MarketIndex2 (not used): " + metrics["MarketIndex2"] + "<br>";
        rval += "Persuasion (not used): " + metrics["Persuasion"];

        return rval;
    }

    /// <summary>
    /// Describes the timing rating of the given segment or overall plan (use segmentIndex = -1 for overall plan)
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segmentIndex"></param>
    /// <param name="engineeringMode"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public string TimingString(double score, Dictionary<string, double> metrics) 
    {
        string rval = "Timing Score: " + score + "<br>";
        rval += "Recency: " + metrics["Recency"] + "<br>";
        rval += "Consideration1: " + metrics["Consideration1"] + "<br>";
        rval += "Consideration3: " + metrics["Consideration3"] + "<br>";
        rval += "Consideration4: " + metrics["Consideration4"];

        return rval;
    }

    /// <summary>
    /// Describes the demographic targeting rating of the given segment or overall plan (use segmentIndex = -1 for overall plan)
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segmentIndex"></param>
    /// <param name="engineeringMode"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public string DemoTargetingString(double score, Dictionary<string, double> metrics) 
    {
        string rval = "DemoTargeting Score: " + score + "<br>";
        rval += "PersuasionIndex: " + metrics["PersuasionIndex"] + "<br>";
        rval += "Reach3All: " + metrics["Reach3All"];

        return rval;
    }

    /// <summary>
    /// Describes the geographic targeting rating of the given segment or overall plan (use segmentIndex = -1 for overall plan)
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="segmentIndex"></param>
    /// <param name="engineeringMode"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public string GeoTargetingString(double score, Dictionary<string, double> metrics)
    {
        string rval = "GeoTargeting Score: " + score + "<br>";
        rval += "PersuasionIndex: " + metrics["PersuasionIndex"] + "<br>";
        rval += "Reach3All: " + metrics["Reach3All"];

        return rval;
    }
}
