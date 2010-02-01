using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using BusinessLogic;

/// <summary>
/// Summary description for StoryDisplayer
/// </summary>
public class StoryDisplayer
{
    private StoryGenerator storyGen;
    private string currentSuggestion;
    private bool engineeringMode;

    private string activeTabColor = "#EA0";
    private string inactiveTabColor = "#FEC";
    private string tabBorderColor = "#000";
    private string tabBorderLightColor = "#AAA";
    private string tabBorderWidth = "1px";
    private string tabBorderStyle = "solid";

    private int nTabs = 2;
    private int storyTabIndex = 0;

    public StoryDisplayer( StoryGenerator storyGenerator, string currentSuggestion, bool engineeringMode ) {
        this.storyGen = storyGenerator;
        this.currentSuggestion = currentSuggestion;
        this.engineeringMode = engineeringMode;
    }

    public void PopulateDisplayDiv( HtmlGenericControl storyDiv, HtmlGenericControl storyBodyDiv, HtmlGenericControl footerDiv, int selectedTab ) {
        Table tabTable = new Table();
        Table bodyTable = new Table();
        Table footerTable = new Table();
        tabTable.CellPadding = 2;
        tabTable.CellSpacing = 1;
        tabTable.Style.Add( HtmlTextWriterStyle.Width, "100%" );
        footerTable.Style.Add( HtmlTextWriterStyle.Width, "100%" );
        tabTable.Style.Add( HtmlTextWriterStyle.Height, "22px" );

        string[] tabTitles = new string[] { "Your session transcript:" };
        //string[] tabTitles = new string[] { "&nbsp;&nbsp;&nbsp;&nbsp;Your&nbsp;Story&nbsp;&nbsp;&nbsp;&nbsp;", "More?" };

        //if( selectedTab != storyTabIndex &&  this.currentSuggestion == "" ) {
        //    selectedTab = storyTabIndex;
        //}

        AddTabs( tabTitles, selectedTab, tabTable );

        AddBody( selectedTab, bodyTable );

        AddEmaiLink( footerTable );

        storyDiv.Controls.Clear();
        storyBodyDiv.Controls.Clear();
        footerDiv.Controls.Clear();

        storyDiv.Controls.Add( tabTable );
        storyBodyDiv.Controls.Add( bodyTable );
        footerDiv.Controls.Add( footerTable );
    }

    private void AddBody( int selectedTabIndex, Table table ) {
        TableRow row = new TableRow();
        row.Style.Add( "max-height", "200px" );
        TableCell cell = new TableCell();
        cell.Style.Add( HtmlTextWriterStyle.Padding, "10px" );

        if( selectedTabIndex == storyTabIndex ) {
            cell.Text = this.storyGen.StoryText( engineeringMode );
        }
        else {
            cell.Text = this.currentSuggestion;
        }
        row.Cells.Add( cell );
        table.Rows.Add( row );
    }


    private void AddEmaiLink( Table table ) {
        TableRow row = new TableRow();
        row.Style.Add( "max-height", "20px" );
        TableCell cell = new TableCell();
        cell.Style.Add( HtmlTextWriterStyle.Padding, "2px" );
        cell.Style.Add( HtmlTextWriterStyle.Width, "100%" );
        cell.Style.Add( HtmlTextWriterStyle.TextAlign, "center" );

        string link_JS = "if( confirm( \"Email your session transcript to you now?\" ) == true ){ __doPostBack( \"StoryContainer\", \"EmailStory\" ); } return false;";

        string linkFmt = "<a href='#'  onclick='{0}' >email transcript to me</a>";
        cell.Text = String.Format( linkFmt, link_JS );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    private void AddTabs( string[] titles, int selectedTabIndex, Table table ) {
        TableRow tabRow = new TableRow();

        for( int c = 0; c < titles.Length; c++ ) {
            TableCell cell = new TableCell();
            cell.Style.Add( HtmlTextWriterStyle.TextAlign, "center" );
            if( c == selectedTabIndex || (c == 0 && (this.currentSuggestion==null || this.currentSuggestion=="") ) ){
                cell.Text = titles[ c ];
            }
            else {
                string link_JS = String.Format( " __doPostBack( \"{0}\", \"{1}\" ); return false;", "StoryContainer", c.ToString() );

                string linkFmt = "<a href='#'  onclick='{0}' >{1}</a>";
                cell.Text = String.Format( linkFmt, link_JS, titles[ c ] );
            }

            if( c == 1 && (this.currentSuggestion==null || this.currentSuggestion=="") ){
                // the suggestions tab when there are no suggestions
                  cell.Style.Add( "color", tabBorderLightColor );
            }

            //if( c != 0 ) {
                cell.Style.Add( "border-left-width", tabBorderWidth );
                cell.Style.Add( "border-left-color", tabBorderColor );
                cell.Style.Add( "border-left-style", tabBorderStyle );
            //}
            //if( c != tabRow.Cells.Count - 1 ) {
                cell.Style.Add( "border-right-width", tabBorderWidth );
                cell.Style.Add( "border-right-color", tabBorderColor );
                cell.Style.Add( "border-right-style", tabBorderStyle );
            //}
            cell.Style.Add( "border-top-width", tabBorderWidth );
            cell.Style.Add( "border-top-color", tabBorderColor );
            cell.Style.Add( "border-top-style", tabBorderStyle );

            if( c != selectedTabIndex ) {
                // a regular tab
                cell.Style.Add( HtmlTextWriterStyle.BackgroundColor, inactiveTabColor );
                cell.Style.Add( "border-bottom-width", tabBorderWidth );
                cell.Style.Add( "border-bottom-color", tabBorderLightColor );
                cell.Style.Add( "border-bottom-style", tabBorderStyle );
            }
            else {
                // the selected tab
                cell.Style.Add( HtmlTextWriterStyle.BackgroundColor, activeTabColor );
            }
            tabRow.Cells.Add( cell );
        }
        table.Rows.Add( tabRow );
    }
}
