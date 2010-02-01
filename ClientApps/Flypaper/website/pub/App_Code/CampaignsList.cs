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

using WebLibrary;

/// <summary>
/// CampaignsList generates the HTML
/// </summary>
public class CampaignsList
{
    private const string legendColor = "#cccccc";
    private const string campaignColor = "#8fC2F4"; //"#33CCFF";
    private const string planColor = "#FFFFFF";
    private const string campaignBorder = "solid 3px #009";

    private int rowIndex = 0;
    private int campaignRowIndex = 0;
    private int minDisplayRows = 40;
    private string userName;

    private Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions;

    private bool viewOnly = false;
    public CampaignsList( string userName, bool viewOnlyIn ) {
        this.userName = userName;
        this.viewOnly = viewOnlyIn;
    }

    /// <summary>
    /// Adds a listing of all campaigns and plans to the given DIV element.  Existing controls in the DIV are removed.  Footer text, if any,
    /// is added below the final editing block.
    /// </summary>
    /// <param name="edDiv"></param>
    public List<MediaPlan> AddCampaignListHTML( HtmlTableCell listCell, List<MediaPlan> currentPlansList, ref  Dictionary<string, List<PlanStorage.PlanVersionInfo>> allPlanVersions, 
        string footerHtml, string sortKey, List<Guid> expandedItemIDs) {

        this.allPlanVersions = allPlanVersions;
        listCell.Controls.Clear();

        Table table = new Table();
        table.CellSpacing = 0;
        table.CellPadding = 1;

        table.Style.Add( HtmlTextWriterStyle.FontFamily, "Arial" );
        table.Style.Add( HtmlTextWriterStyle.FontSize, "9pt" );
        table.Style.Add( HtmlTextWriterStyle.Width, "99%" );

        AddCampaignLegendRow( table, sortKey );

        this.rowIndex = 0;
        this.campaignRowIndex = 0;

        List<MediaPlan> sortedCurrentPlans = SortCampaignInfo( currentPlansList, sortKey );

        List<string> campaignCopyNames = GenerateCampaignCopyNames( sortedCurrentPlans );

        // loop over campaigns
        for( int c = 0; c < sortedCurrentPlans.Count; c++ ) {

            MediaPlan plan = sortedCurrentPlans[ c ];

            List<PlanStorage.PlanVersionInfo> allVersions = this.allPlanVersions[ plan.CampaignName ];      //Note: This does not preserve the sequence of the allVersions list
            allVersions = PlanStorage.SortPlanVersionInfo( allVersions, true, sortKey );

            bool expandRequested = expandedItemIDs.Contains( plan.PlanID );
            bool isExpanded = CheckItemExpansion( plan, expandRequested, allVersions );
            AddCampaign( table, plan, campaignCopyNames[c], allVersions, isExpanded);

            if( c != sortedCurrentPlans.Count - 1 )
            {
                AddBorderSpacerRow( table );
            }
        }

        listCell.Controls.Add( table );

        if( table.Rows.Count < minDisplayRows ) {
            for( int x = 0; x < minDisplayRows - table.Rows.Count; x++ )
            {
                AddSpacerRow( table );
            }
        }

        AddFooterTable( listCell, footerHtml );

        return sortedCurrentPlans;
    }

    private void addRowCells(TableRow row)
    {
        for( int ii = 0; ii < 9; ++ii )
        {
            TableCell aCell = new TableCell();

            // default
            aCell.Text = "&nbsp";
            aCell.Style.Add( "padding", "2px" );
            aCell.Style.Add( "margin-top", "2px" );
            aCell.Style.Add( "margin-bottom", "2px" );

            row.Cells.Add( aCell );
        }
    }


    private List<string> GenerateCampaignCopyNames( List<MediaPlan> plans ) {
        List<string> allCampNames = new List<string>();
        foreach( MediaPlan plan in plans ) {
            allCampNames.Add( plan.CampaignName );
        }

        List<string> copyNames = new List<string>();
        for( int i = 0; i < plans.Count; i++ ) {
            string cname = "Copy of " + plans[ i ].CampaignName;
            int cx = 2;
            while( allCampNames.Contains( cname ) == true ) {
                cname = "Copy (" + cx.ToString() + ") of " + plans[ i ].CampaignName;
                cx += 1;
            }
            copyNames.Add( cname );
        }

        return copyNames;
    }

    private List<MediaPlan> SortCampaignInfo( List<MediaPlan> plans,  string sortKey ) {

        Object[] sortKeys = new Object[ plans.Count ];
        MediaPlan[] plansArray = new MediaPlan[ plans.Count ];

        bool doReverse = !sortKey.EndsWith( "-" );
        sortKey = sortKey.TrimEnd( '-' );
        for( int i = 0; i < plans.Count; i++ ) {
            plansArray[ i ] = plans[ i ];
            switch( sortKey ) {
                case "SortByDate":
                    sortKeys[ i ] = plans[ i ].ModificationDate;
                    break;
                case "SortByBudget":
                    sortKeys[i] = plans[i].Specs.TargetBudget;
                    break;
                default:
                    sortKeys[ i ] = plans[ i ].CampaignName;
                    break;
            }
        }

        Array.Sort( sortKeys, plansArray );
        if( doReverse ) {
            Array.Reverse( plansArray );
        }

        List<MediaPlan> sortedPlans = new List<MediaPlan>();
        for( int i = 0; i < plansArray.Length; i++ ) {
            sortedPlans.Add( plansArray[ i ] );
        }

        return sortedPlans;
    }

    private void AddSpacerRow( Table table ) {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        row.Style.Add( "height", "5px" );
        cell.Text = "&nbsp;";

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    private void AddBorderSpacerRow( Table table )
    {
        TableRow row = new TableRow();
        addRowCells( row );

        foreach( TableCell cell in row.Cells )
        {
            row.Style.Add( "height", "3px" );
            cell.Text = "&nbsp;";

            cell.Style.Add( "border-top", campaignBorder );
            cell.Style.Add( "border-bottom", campaignBorder );
        }
        table.Rows.Add( row );
    }

    private bool CheckItemExpansion( MediaPlan plan, bool expandRequested, List<PlanStorage.PlanVersionInfo> versionInfo ) {
        bool isExpanded = expandRequested;
        bool hasZeroPlans = false;
        if( versionInfo.Count == 1 && plan.PlanValid == false ) {
            hasZeroPlans = true;
        }
        if( hasZeroPlans ) {
            isExpanded = false;
        }

        if( versionInfo.Count == 1 ) {
            // if there are 0 or 1 plans in this campaign, do not show the expand control
            if( hasZeroPlans == false ) {
                // always "expand" a single item
                isExpanded = true;
            }
        }
        return isExpanded;
    }

    /// <summary>
    /// Adds a campaign row to the table
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="table"></param>
    /// <param name="expandedItemIDs"></param>
    private void AddCampaign( Table table, MediaPlan plan, string campaignCopyName, List<PlanStorage.PlanVersionInfo> versionInfo, bool isExpanded)
    {
        TableRow planRow = new TableRow();

        addRowCells( planRow );

        planRow.Style.Add( "height", "25px" );
        MediaCampaignSpecs campaign = plan.Specs;

        bool hasZeroPlans = false;
        if( versionInfo.Count == 1 && plan.PlanValid == false )
        {
            hasZeroPlans = true;
        }

        planRow.Cells[0].Style.Add( "border-left", campaignBorder );
        foreach( TableCell cl in planRow.Cells )
        {
            cl.Style.Add( "border-top", campaignBorder );
        }
        planRow.Cells[planRow.Cells.Count -1].Style.Add( "border-right", campaignBorder );

        TableCell campName = planRow.Cells[1];

        TableCell planInfo = planRow.Cells[7];
        TableCell budget = planRow.Cells[3];
        TableCell scoreBoard = planRow.Cells[4];
        TableCell dateCell = planRow.Cells[2];
        TableCell actionCell = planRow.Cells[8];

        AddScoreboardLink( scoreBoard, plan, "" );

        dateCell.Text = CampaignDate( plan.ModificationDate );

        string dispName = plan.CampaignName.Replace( " ", "&nbsp;" );
        if( !this.viewOnly )
        {
            campName.Text = "<a href=Campaign.aspx?p=" + plan.PlanID.ToString() + " style=\"font-size:10pt;\" >" + dispName + "</a>";
        }
        else
        {
            campName.Text = "<div style=\"font-size:10pt;\" >" + plan.CampaignName + "</div>";
        }

        budget.Text = CampaignBudget(plan.Specs.TargetBudget );

        if( hasZeroPlans )
        {
            planInfo.Text = PlanCount(0);
        }

        if( !this.viewOnly )
        {
            AddCampaignActionControls( actionCell, plan.CampaignName, campaignCopyName, plan.Specs.IsCompetition );
        }
        else
        {
            actionCell.Text = "View Only";
        }

        //if( highlight )
        //{
        //    planRow.Style.Add( HtmlTextWriterStyle.BackgroundColor, legendColor);
        //}

        planRow.Style.Add( HtmlTextWriterStyle.BackgroundColor, campaignColor );

        table.Rows.Add( planRow );

        if( !hasZeroPlans )
        {
            AddMediaPlanRows( table, plan, versionInfo, isExpanded );
        }
        else
        {
            foreach( TableCell cl in planRow.Cells )
            {
                cl.Style.Add( "border-bottom", campaignBorder );
            }
        }

    }

    /// <summary>
    /// Returns a string that describes the number of plans in a non-expanded campaign row. 
    /// </summary>
    /// <param name="nPlans"></param>
    /// <param name="plan"></param>
    /// <returns></returns>
    private string PlanCount( int nPlans) {
        if( nPlans == 0 )
        {
            return "no media plans";
        }
        else if( nPlans == 1 ) 
        {
            return "1 media plan";
        }
        else 
        {
            return String.Format( "{0} media plans", nPlans );
        }
    }

    /// <summary>
    /// Adds the plan rows after the first one for an expanded campaign.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="curPlan"></param>
    /// <param name="rowNum"></param>
    /// <param name="allVersions"></param>
    private void AddMediaPlanRows( Table table, MediaPlan plan, List<PlanStorage.PlanVersionInfo> allVersions, bool isExpanded)
    {
        int nRows = allVersions.Count;
        List<string> suggestedCopiedPlanNames = new List<string>();
        for( int r = 0; r < nRows; r++ )
        {
            suggestedCopiedPlanNames.Add( PlanCopyNameFor( allVersions, r ) );
        }

        AddMediaPlanLegendRow( table, PlanCount( allVersions.Count ) );

        for( int r = 0; r < nRows; r++ )
        {
            bool hasExpandControl = (r == 0) && (nRows > 1);
            PlanStorage.PlanVersionInfo info = allVersions[r];

            TableRow row = new TableRow();

            addRowCells(row);

            row.Cells[0].Style.Add( "border-left", campaignBorder );
            row.Cells[row.Cells.Count - 1].Style.Add( "border-right", campaignBorder );


            row.Style.Add( "height", "25px" );

            row.Style.Add( HtmlTextWriterStyle.BackgroundColor, planColor );

            row.Cells[1].Text = PlanDescription( info.Description, plan.CampaignName, info.Version );

            row.Cells[2].Text = CampaignDate(info.ModificationDate);

            row.Cells[3].Text = CampaignBudget( info.TotalCost );

            AddPlanRatingStars( info.StarsRating, row.Cells[7], plan, info.Competitor );

            if( !this.viewOnly )
            {
                AddActionControls( row.Cells[8], plan.CampaignName, info.Description, info.Version, suggestedCopiedPlanNames[r] );
            }
            else
            {
                row.Cells[8].Text = "View Only";
            }

            table.Rows.Add( row );

            if( r == nRows - 1 || !isExpanded )
            {
                foreach( TableCell cl in row.Cells )
                {
                    cl.Style.Add( "border-bottom", campaignBorder );
                }
            }


            if( r == 0 )
            {
                row.Cells[0].Text = ExpandIcon( isExpanded, hasExpandControl, plan );

                if( !isExpanded )
                {
                    break;
                }
            }

        }
    }


    private string PlanCopyNameFor( List<PlanStorage.PlanVersionInfo> allVersions, int row ) {
        List<string> curNames = new List<string>();
        foreach( PlanStorage.PlanVersionInfo info in allVersions ) {
            curNames.Add( info.Description );
        }

        string curName = allVersions[ row ].Description;
        string newName = "Copy of " + curName;
        int cx = 2;
        while( curNames.Contains( newName ) == true ) {
            newName = "Copy {" + cx.ToString() + ") of " + curName;
            cx++;
        }
        return newName;
    }

    /// <summary>
    /// Adds the campaign action-initiation dropdown list to the given cell
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="campaignName"></param>
    /// <param name="planComments"></param>
    /// <param name="version"></param>
    private void AddCampaignActionControls( TableCell cell, string campaignName, string campaignCopyName, bool isCompetition ) {
        string controlID = "CAction-" + this.campaignRowIndex.ToString();
        campaignRowIndex += 1;

        DropDownList actionList = new DropDownList();
        actionList.ID = controlID;

        actionList.Items.Add( new ListItem( "-", "" ) );
        actionList.Items.Add( new ListItem( "view", "Edit-" + campaignName ) );
        actionList.Items.Add( new ListItem( "duplicate", "Copy-" + campaignName ) );
        actionList.Items.Add( new ListItem( "add plan...", "Add-" + campaignName ) );
        actionList.Items.Add( new ListItem( "delete", "Delete-" + campaignName ) );
        if( isCompetition ) {
            actionList.Items.Add( new ListItem( "scoreboard", "Scoreboard-" + campaignName ) );
        }

        actionList.SelectedIndex = 0;
        actionList.EnableViewState = false;
        actionList.Style.Add( "font-size", "8pt" );
        actionList.Style.Add( "background-color", "#EFF" );
        actionList.Style.Add( "width", "55px" );

        actionList.Attributes.Add( "onchange", "FireCampaignActionPostback('" + controlID + "'," + this.campaignRowIndex.ToString() + ", '" +
            campaignCopyName + "' );" ); 

        cell.Controls.Clear();
        cell.Controls.Add( actionList );
    }

    /// <summary>
    /// Adds the action-initiation dropdown list to the given cell
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="campaignName"></param>
    /// <param name="planComments"></param>
    /// <param name="version"></param>
    private void AddActionControls( TableCell cell, string campaignName, string planComments, string version, string suggestedCopiedPlanName ) {
        string controlID = "Action-" + this.rowIndex.ToString();
        this.rowIndex += 1;

        DropDownList actionList = new DropDownList();
        actionList.ID = controlID;

        string identifierSuffix = String.Format( "{0}::{1}::{2}", campaignName, planComments, version );

        actionList.Items.Add( new ListItem( "-", "" ) );
        //actionList.Items.Add( new ListItem( "view", "View-" + identifierSuffix ) );
        actionList.Items.Add( new ListItem( "edit", "Edit-" + identifierSuffix ) );
        actionList.Items.Add( new ListItem( "analyze", "Analyze-" + identifierSuffix ) );
        actionList.Items.Add( new ListItem( "order", "Order-" + identifierSuffix ) );
        actionList.Items.Add( new ListItem( "duplicate", "Duplicate-" + identifierSuffix ) );
        actionList.Items.Add( new ListItem( "delete", "Delete-" + identifierSuffix ) );

        if( System.Configuration.ConfigurationManager.AppSettings[ "Adplanit.DevelopmentMode" ] == "TRUE" ) {
            actionList.Items.Add( new ListItem( "send", "Send-" + identifierSuffix ) );
        }

        actionList.SelectedIndex = 0;
        actionList.EnableViewState = false;
        actionList.Style.Add( "font-size", "8pt" );
        actionList.Style.Add( "background-color", "#EFF" );
        actionList.Style.Add( "width", "55px" );

       
            actionList.Attributes.Add( "onchange", "FireActionPostback('" + controlID + "'," + this.campaignRowIndex.ToString() + ", '" +
                suggestedCopiedPlanName + "' );" );
     

        cell.Controls.Clear();
        cell.Controls.Add( actionList );
    }

    /// <summary>
    /// Formats the plan description string for display
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="showVersion"></param>
    /// <returns></returns>
    private string PlanDescription( string desc, string campaignName, string planVersion ) {

        if( desc == null || desc.Length == 0 ) {
            desc = "---";
        }
  //      desc = desc.Replace( " ", "&nbsp;" );

        string sel_campaign_plan_js = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}::{2}\" ); return false;", "SelectAndEditVersion", campaignName,  planVersion );

        string descHtml = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sel_campaign_plan_js, desc );

        return descHtml;
    }

    private string PlanVersion( string version ) {
        return "[v" + version + "]";
    }

    /// <summary>
    /// Formats the campaign budget for display
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    private string CampaignBudget( double budget ) {
        return String.Format( "$&nbsp;{0:0,0}", budget );
    }

    /// <summary>
    /// Returns the string that shows the expand/collapse control.
    /// </summary>
    /// <param name="isExpanded"></param>
    /// <param name="prefixText"></param>
    private string ExpandIcon( bool isExpanded, bool isVisible, MediaPlan plan ) {
        string expand_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "ExpandItem", plan.PlanID.ToString() );
        string collapse_item_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "CollapseItem", plan.PlanID.ToString() );
        string cellTxt = null;

        if( isVisible == true ) {
            if( isExpanded == false ) {
                string expandButtonTag = "<img src=\"images/Expand.gif\" width=20 height=20 border=0 >";
                cellTxt = String.Format( "<a href=\"#\" onclick='{0}'  style=\"font-size:8pt;position:relative;top:6px;\" >{1}</a><span style=\"position:relative;top:-4px;\"></span>", 
                    expand_item_JS, expandButtonTag);
            }
            else {
                string condenseButtonTag = "<img src=\"images/Condense.gif\" width=20 height=20 border=0 >";
                cellTxt = String.Format( "<a href=\"#\" onclick='{0}'  style=\"font-size:8pt;position:relative;top:4px;\" >{1}</a><span style=\"position:relative;top:-4px;\"></span>", 
                    collapse_item_JS, condenseButtonTag );
            }
        }
        else {
            cellTxt = String.Format( "<span  style=\"margin-left:20px;\" ></span>" );
        }
        return cellTxt;
    }


    /// <summary>
    /// Fills the given cell with the image for the given star rating.
    /// </summary>
    /// <param name="planOverallRatingStars"></param>
    /// <param name="ratingCell"></param>
    private void AddPlanRatingStars( double planOverallRatingStars, TableCell ratingCell, MediaPlan plan, string competitor ) {
        int nStars = (int)Math.Floor( planOverallRatingStars );
        if( planOverallRatingStars >= 0 ) {
            for( int i = 0; i < nStars; i++ ) {
                Image img = new Image();
                //img.ImageUrl = "images/SmallStarTrans.gif";
                img.ImageUrl = "images/Star16.gif";
                ratingCell.Controls.Add( img );
            }
            for( int i = nStars; i < 5; i++ ) {
                Image img = new Image();
                img.ImageUrl = "images/Star16bl.gif";
                ratingCell.Controls.Add( img );
            }
            Image spcr = new Image();
            spcr.ImageUrl = "images/Spacer.gif";
            spcr.Width = 4;
            ratingCell.Controls.Add( spcr );

            AddScoreboardLink( ratingCell, plan, competitor );
        }
        else {
            ratingCell.Text = "not&nbsp;simulated";
        }
    }

    private void AddScoreboardLink( TableCell cell, MediaPlan plan, string competitor ) {
        if( competitor == null || plan.Specs.IsCompetition == false ) {
            return;
        }

        Image timg = new Image();

        if( plan.Specs.CompetitionOwner == this.userName ) {
            timg.ToolTip = "Competition you Launched - Click to view Scoreboard";
            timg.ImageUrl = "images/TrophyYour.gif";
        }
        else {
            timg.ToolTip = "Competition - Click to view Scoreboard";
            timg.ImageUrl = "images/Trophy.gif";
        }
        timg.Attributes.Add( "position", "relative" );
        timg.Attributes.Add( "top", "5px;" );
        HyperLink hl1 = new HyperLink();
        hl1.Controls.Add( timg );

        string scoreboardAction = String.Format( "__doPostBack( \"DoCampaignAction\", \"Scoreboard-{0}\" )", plan.CampaignName );
        hl1.NavigateUrl = "#";
        hl1.Attributes.Add( "onclick", scoreboardAction );

        cell.Controls.Add( hl1 );
    }

    /// <summary>
    /// Generates the date display string.
    /// </summary>
    /// <param name="campaignModificationDate"></param>
    /// <returns></returns>
    private string CampaignDate( DateTime campaignModificationDate ) {
        //DateTime nowDate = DateTime.Now;

        //string dateStr = null;
        //if( (nowDate.Year == campaignModificationDate.Year) && (nowDate.Month == campaignModificationDate.Month) && (nowDate.Day == campaignModificationDate.Day) ) {
        //    // we are stil in the same day
        //    dateStr = campaignModificationDate.ToString( "h:mmtt" );
        //}
        //else if( nowDate.Year == campaignModificationDate.Year ) {
        //    // we are still in the same year
        //    dateStr = campaignModificationDate.ToString( "dd-MMM" );
        //}
        //else {
        //    // we are in the next year
        //    dateStr = campaignModificationDate.ToString( "ddMMM yyyy" );
        //}
        string dateStr = campaignModificationDate.ToString( "MM/dd/yy" );
        return dateStr;
    }


    /// <summary>
    /// Adds a legend row to the table.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="sortKey"></param>
    private void AddCampaignLegendRow( Table table, string sortKey) {
        TableRow legendRow = new TableRow();

        addRowCells( legendRow );
        TableCell nameCell = legendRow.Cells[1];

        TableCell dateCell = legendRow.Cells[2];
        TableCell actionCell = legendRow.Cells[8];
        TableCell budgetCell = legendRow.Cells[3];

        legendRow.Style.Add( HtmlTextWriterStyle.Height, "16px" );
        legendRow.Style.Add( HtmlTextWriterStyle.FontSize, "8pt" );
        legendRow.Style.Add( HtmlTextWriterStyle.Color, "Black" );
        legendRow.Style.Add( HtmlTextWriterStyle.BackgroundColor,legendColor );


        string sortUpImg = "<img src=\"images/Sort-Up.gif\" style=\"position:relative;top:2px;left:2px;\" >";
        string sortDownImg = "<img src=\"images/Sort-Down.gif\" style=\"position:relative;top:2px;left:2px;\" >";

        string dirStr = "-";
        string sortImg = sortDownImg;

        if( sortKey.EndsWith( "-" ) )
        {
            sortKey = sortKey.TrimEnd( '-' );
            sortImg = sortUpImg;
            dirStr = "";
        }

        string sort_by_name_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByName" + dirStr, "" );
        string sort_by_budget_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByBudget" + dirStr, "" );
        string sort_by_date_JS = String.Format( "ShowMainWaitCursor(); __doPostBack( \"{0}\", \"{1}\" ); return false;", "SortByDate" + dirStr, "" );

        dateCell.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_date_JS, "Date" );
        nameCell.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_name_JS, "Campaign" );
        budgetCell.Text = String.Format( "<a href=\"#\" onclick='{0}' >{1}</a>", sort_by_budget_JS, "Budget" );

         
        if( sortKey == "SortByBudget" )
        {
            budgetCell.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_budget_JS, "Budget", sortImg );
        }
        else if( sortKey == "SortByName" )
        {
            nameCell.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_name_JS, "Campaign", sortImg );
        }
         else // sort by date
        {
            dateCell.Text = String.Format( "<b><a style=\"color:#484\" href=\"#\" onclick='{0}' >{1}</a></b>{2}", sort_by_date_JS, "Date", sortImg );
        }

        actionCell.Text = "Action";

        //actionCell.Style.Add( HtmlTextWriterStyle.TextAlign, "right" );

        foreach( TableCell cell in legendRow.Cells )
        {
            SetLegendCellBorders( cell, false, false );
        }

        SetLegendCellBorders( legendRow.Cells[0], true, false );
        SetLegendCellBorders( legendRow.Cells[legendRow.Cells.Count - 1], false,  true);

        table.Rows.Add( legendRow );
    }
    /// <summary>
    /// Adds a legend row to the table.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="sortKey"></param>
    private void AddMediaPlanLegendRow( Table table, string text)
    {
        TableRow legendRow = new TableRow();

        addRowCells( legendRow );
        TableCell nameCell = legendRow.Cells[1];

        TableCell dateCell = legendRow.Cells[2];
        TableCell budgetCell = legendRow.Cells[3];
        TableCell rateCell = legendRow.Cells[7];
        TableCell actionCell = legendRow.Cells[8];

        legendRow.Style.Add( HtmlTextWriterStyle.Height, "16px" );
        legendRow.Style.Add( HtmlTextWriterStyle.FontSize, "8pt" );
        legendRow.Style.Add( HtmlTextWriterStyle.Color, "Black" );
        legendRow.Style.Add( HtmlTextWriterStyle.BackgroundColor, planColor );


        nameCell.Text = text;
        rateCell.Text = "Rating";

        foreach( TableCell cell in legendRow.Cells )
        {
            SetLegendCellBorders( cell, false, false );
        }

        legendRow.Cells[0].Style.Add( "border-left", campaignBorder );
        legendRow.Cells[legendRow.Cells.Count - 1].Style.Add( "border-right", campaignBorder );

        table.Rows.Add( legendRow );
    }

    /// <summary>
    /// Styles a legend cell's borders
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="hasLeft"></param>
    /// <param name="hasRight"></param>
    private void SetLegendCellBorders( TableCell cell, bool hasLeft, bool hasRight ) {
        string brdr = "solid 1px #009";
        cell.Style.Add( "border-top", brdr );
        cell.Style.Add( "border-bottom", brdr );
        if( hasLeft ) {
            cell.Style.Add( "border-left", brdr );
        }
        if( hasRight ) {
            cell.Style.Add( "border-right", brdr );
        }
      

        //cell.Style.Add( "text-align", "center" );
    }


    private void AddFooterTable( HtmlTableCell listDiv, string footerHtml ) {
        if( footerHtml != null ) {
            Table footerTable = new Table();
            footerTable.Style.Add( HtmlTextWriterStyle.FontFamily, "Arial" );
            footerTable.Style.Add( HtmlTextWriterStyle.FontSize, "10pt" );
            footerTable.Style.Add( HtmlTextWriterStyle.Width, "600px" );

            TableRow footerRow = new TableRow();
            TableCell footerCell = new TableCell();
            footerCell.ColumnSpan = 5;

            footerCell.Text = footerHtml;
            footerCell.Style.Add( "padding-top", "40px" );

            footerRow.Cells.Add( footerCell );
            footerTable.Rows.Add( footerRow );
            listDiv.Controls.Add( footerTable );
        }
    }

}
