using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

/// <summary>
/// Summary description for FAQMaker
/// </summary>
public class FAQMaker
{
    public FAQMaker() {
        //
        // TODO: Add constructor logic here
        //
    }


    public void AddFAQ( HtmlTableCell faqCell ) {
        Table table = new Table();
        table.CellPadding = 2;
        table.CellSpacing = 0;
        table.Style.Add( "width", "99%" );

        Dictionary<string, string> allFAQs = LoadFAQs();     //??? perhaps this should be saved in a static for performace?

        int itemNum = 0;
        // add the list of questions
        foreach( string key in allFAQs.Keys ) {
            AddFAQRow( table, key, allFAQs[ key ], itemNum,  false );
            itemNum += 1;
        }

        itemNum = 0;
        // add the list of questions and answers
        foreach( string key in allFAQs.Keys ) {
            AddFAQRow( table, key, allFAQs[ key ], itemNum, true );
            itemNum += 1;
        }

        AddBottomPadding( table );

        faqCell.Controls.Clear();
        Label title = new Label();
        title.Text = "Frequently Asked Questions";

        title.Style.Add( "font-size", "14pt" );
        title.Style.Add( "color", "#4477CC" );
        title.Style.Add( "font-weight", "normal" );

        faqCell.Controls.Add( title );
        faqCell.Controls.Add( table );
    }

    private void AddFAQRow( Table table, string question, string answer, int qNumber, bool showAnswer ) {
        TableRow row = new TableRow();
        TableRow row2 = new TableRow();
        TableCell questionCell = new TableCell();
        TableCell answerCell = new TableCell();

        string termAnchor = "term" + qNumber.ToString();

        if( showAnswer == false ) {
            questionCell.Text = String.Format( "<a href=\"#{0}\">{1}</a>", termAnchor, question );
            questionCell.Style.Add( "padding-top", "10px" );
        }
        else {
            questionCell.Text = String.Format( "<a name=\"{0}\">{1}</a>", termAnchor, question );
            questionCell.Style.Add( "padding-top", "50px" );
            answerCell.Style.Add( "padding-top", "5px" );
        }

        if( qNumber == 0 && showAnswer == false ) {
            questionCell.Style.Add( "padding-top", "25px" );
        }

        answerCell.Text = answer;

        questionCell.Style.Add( "font-size", "8pt" );
        questionCell.Style.Add( "font-weight", "bold" );

        answerCell.Style.Add( "font-size", "8pt" );

        row.Cells.Add( questionCell );
        table.Rows.Add( row );

        if( showAnswer ) {
            row2.Cells.Add( answerCell );
            table.Rows.Add( row2 );
        }
    }

    private void AddBottomPadding( Table table ) {
        TableRow row = new TableRow();
        TableCell cell = new TableCell();

        cell.Text = "&nbsp;";
        cell.Style.Add( "height", "400px" );

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }

    /// <summary>
    /// Loads the glossary list from the Glossary.csv file
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, string> LoadFAQs() {
        Dictionary<string, string> faqs = new Dictionary<string, string>();

        string dir = HttpContext.Current.Server.MapPath( null );

        string faqFile = dir + @"\Help\FAQ.csv";

        if( File.Exists( faqFile ) == false ) {
            faqs.Add( "Error: File not found", faqFile );
            return faqs;
        }

        FileStream fs = new FileStream( faqFile, FileMode.Open, FileAccess.Read );
        StreamReader sr = new StreamReader( fs );

        string fileLine = null;
        while( (fileLine = sr.ReadLine()) != null ) {
            int stIndx = 0;
            if( fileLine.StartsWith( "\"" ) == true ) {
                // first term is quoted
                stIndx = fileLine.IndexOf( "\"", 1 );
                if( stIndx < 0 ) {
                    stIndx = 0; // just in case there is a line with just one quote in it
                }
            }

            int cIndx = fileLine.IndexOf( ",", stIndx );

            if( cIndx != -1 ) {
                string quest = StripQuotes( fileLine.Substring( 0, cIndx ) ).Trim();
                string ans = StripQuotes( fileLine.Substring( cIndx + 1 ) ).Trim();

                quest = quest.Replace( (char)65533, '\'' );
                quest = quest.Replace( "'", "&#39;" );
                ans = ans.Replace( (char)65533, '\'' );
                ans = ans.Replace( "'", "&#39;" );

                faqs.Add( quest, ans );
            }
            else {
                // no definition for this term!
                faqs.Add( fileLine.Trim(), "" );
            }
        }

        sr.Close();
        fs.Close();

        return faqs;
    }

    private string StripQuotes( string s ) {
        if( s.StartsWith( "\"" ) == false ) {
            return s;
        }
        else {
            return s.Substring( 1, s.Length - 2 );
        }
    }
}
