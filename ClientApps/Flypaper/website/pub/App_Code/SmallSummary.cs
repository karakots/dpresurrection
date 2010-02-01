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
using MediaLibrary;

/// <summary>
/// Summary description for SmallSummary
/// </summary>
public class SmallSummary
{
    public SmallSummary() {
        //
        // TODO: Add constructor logic here
        //
    }

    public void AddSummaryHTML( HtmlGenericControl summaryDiv, MediaPlan plan, bool engineeringMode ) {
        AddSummaryHTML( summaryDiv, plan, engineeringMode, false );
    }

    public void AddSummaryHTML( HtmlGenericControl summaryDiv, MediaPlan plan, bool engineeringMode, bool chartOnly ) {
        Table table = new Table();
        table.CellPadding = 1;
        table.CellSpacing = 0;
        table.Style.Add( "font-size", "8pt" );

        if( chartOnly == false ) {
            AddHeaderRow( table, "Current Campaign", false );
            AddValueRow( table, "Budget:", String.Format( "$&nbsp;{0:0,0}", plan.Specs.TargetBudget ) );
            AddValueRow( table, "Start Date:", plan.StartDate.ToString( "MMM d, yyyy" ) );
            AddValueRow( table, "End Date:", plan.EndDate.ToString( "MMM d, yyyy" ) );
            for( int s = 0; s < plan.Specs.Demographics.Count; s++ ) {
                AddValueRow( table, String.Format( "Segment {0}:", s + 1 ), plan.Specs.Demographics[ s ].DemographicName );
            }

            AddHeaderRow( table, "Current Plan", true );
            AddValueRow( table, "Total Cost:", String.Format( "$&nbsp;{0:0,0}", plan.SumOfItemBudgets ), "padding-bottom", "15px" );
        }

        AddBreakupPieChart( table, plan );

        Table table2 = new Table();
        table2.CellPadding = 1;
        table2.CellSpacing = 0;
        table2.Style.Add( "font-size", "8pt" );

        if( chartOnly == false ) {
            AddGoalsScoresRows( table2, plan );
        }

        //AddValueRow( table, "suggestions", null, "padding-top", "25px" );

        summaryDiv.Controls.Clear();
        summaryDiv.Controls.Add( table );
        if( chartOnly == false ) {
            summaryDiv.Controls.Add( table2 );
        }
    }

    private void AddGoalsScoresRows( Table table, MediaPlan plan ) {
        for( int i = 0; i < plan.Specs.CampaignGoals.Count; i++ ) {
            string goalName = plan.Specs.CampaignGoals[ i ].ToString();
            if( goalName == "ReachAndAwareness" ) {
                goalName = "Reach & Awareness";
            }
            goalName += ":&nbsp;";

            int goalScore = -1;
            if( plan.PlanOverallGoalScores != null && plan.PlanOverallGoalScores.Count > i ) {
                goalScore = (int)Math.Round( plan.PlanOverallGoalScores[ i ] * 100 );
            }

            string scoreString = "";
            if( goalScore != -1 ) {
                scoreString = goalScore.ToString();
            }            
            if( i == 0 ) {
                // the first goal
                AddValueRow( table, goalName, scoreString, "padding-top", "15px" );
            }
            else if( i == plan.Specs.CampaignGoals.Count - 1 ) {
                // the last goal
                AddValueRow( table, goalName, scoreString, "padding-bottom", "25px" );
            }
            else {
                AddValueRow( table, goalName, scoreString );
            }
        }
    }

    private void AddBreakupPieChart( Table table, MediaPlan plan ) {
        TableRow row = new TableRow();
        TableCell cell1 = new TableCell();
        TableCell cell2 = new TableCell();

        cell1.Text = String.Format( "<img src=\"MediaPieChart.aspx?p={0}\" width=\"75\" height=\"75\" >", plan.PlanID );

        cell1.Style.Add( "height", "65px" );
        cell1.Style.Add( "background-color", "White" );

        Dictionary <MediaVehicle.MediaType, double> fracs = plan.GetTypeFractions();

        // sort the fractions
        double[] vals = new double[ fracs.Count ];
        MediaVehicle.MediaType[] typs = new MediaVehicle.MediaType[ fracs.Count ];
        int i = 0;
        foreach( MediaVehicle.MediaType mtype in fracs.Keys ) {
            typs[ i ] = mtype;
            vals[ i ] = fracs[ mtype ];
            i++;
        }
        Array.Sort( vals, typs );
        Array.Reverse( vals );
        Array.Reverse( typs );

        string s = "";
        for( int m = 0; m < typs.Length; m++ ) {
            string lblType = typs[ m ].ToString();
            System.Drawing.Color c = Utils.MediaBackgroundColor( typs[ m ] );
            string hc = String.Format( "#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B );
            double fval = vals[ m ];

            if( lblType == "Yellowpages" ) {
                lblType = "Yellowpgs";
            }
            if( double.IsNaN( fval ) == false ) {
                s += String.Format( "{0}: {1:f0}%<br>", lblType, fval * 100 );
            }
            else {
                s += String.Format( "{0}: --<br>", lblType, fval * 100 );
            }
        }
        if( s.Length > 0 ) {
            s = s.Substring( 0, s.Length - 4 );       // remove final break
        }

        cell2.Text = s;

        row.Cells.Add( cell1 );
        row.Cells.Add( cell2 );

        table.Rows.Add( row );
    }

    private void AddValueRow( Table table, string name, string val ) {
        AddValueRow( table, name, val, null, null );
    }

    private void AddValueRow( Table table, string name, string val, string styleName, string styleVal ) {

        TableRow row = new TableRow();
        TableCell cell1 = new TableCell();
        TableCell cell2 = new TableCell();

        cell1.Text = name;
        cell2.Text = val;
        if( styleName != null ) {
            cell1.Style.Add( styleName, styleVal );
            cell2.Style.Add( styleName, styleVal );
        }
        if( val == null ) {
            cell1.ColumnSpan = 2;
        }

        row.Cells.Add( cell1 );
        if( val != null ) {
            row.Cells.Add( cell2 );
        }

        table.Rows.Add( row );
    }

    private void AddHeaderRow( Table table, string headerTxt, bool padTop ) {

        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        cell.ColumnSpan = 2;
        cell.Style.Add( "font-weight", "bold" );
        cell.Style.Add( "text-align", "center" );
        if( padTop ) {
            cell.Style.Add( "padding-top", "20px" );
        }

        cell.Text = headerTxt;
        row.Cells.Add( cell );

        table.Rows.Add( row );
    }
}
