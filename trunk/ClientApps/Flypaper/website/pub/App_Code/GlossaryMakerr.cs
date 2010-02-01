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
using System.IO;

/// <summary>
/// Summary description for GlossaryMakerr
/// </summary>
public class GlossaryMaker
{
    public GlossaryMaker() {
        //
        // TODO: Add constructor logic here
        //
    }

    public void AddGlossary( HtmlTableCell glossaryCell ) {
        Table table = new Table();
        table.CellPadding = 2;
        table.CellSpacing = 0;
        table.Style.Add( "width", "99%" );

        Dictionary<string, string> allTerms = LoadGlossaryTerms();     //??? perhaps this should be saved in a static for performace?

        foreach( string key in allTerms.Keys ) {
            AddGlossaryRow( table,  key, allTerms[ key ] );
        }

        glossaryCell.Controls.Clear();
        Label title = new Label();
        title.Text = "Glossary of Terms";

        title.Style.Add( "font-size", "14pt" );
        title.Style.Add( "color", "#4477CC" );
        title.Style.Add( "font-weight", "normal" );

        glossaryCell.Controls.Add( title );
        glossaryCell.Controls.Add( table );
    }

    private void AddGlossaryRow( Table table, string term, string definition ) {
        TableRow row = new TableRow();
        TableCell termCell = new TableCell();
        TableCell defCell = new TableCell();

        string termAnchor = term.Replace( ' ', 'X' );

        termCell.Text = String.Format( "<a name=\"{0}\">{1}</a>", termAnchor, term );

        defCell.Text = definition;

        termCell.Style.Add( "font-size", "8pt" );
        termCell.Style.Add( "font-weight", "bold" );
        termCell.Style.Add( "width", "110px" );
        termCell.Style.Add( "padding-top", "10px" );
        termCell.Style.Add( "vertical-align", "top" );

        defCell.Style.Add( "font-size", "8pt" );
        defCell.Style.Add( "padding-top", "10px" );
        defCell.Style.Add( "vertical-align", "top" );

        row.Cells.Add( termCell );
        row.Cells.Add( defCell );
        table.Rows.Add( row );
    }

    /// <summary>
    /// Loads the glossary list from the Glossary.csv file
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, string> LoadGlossaryTerms() {
        Dictionary<string, string> terms = new Dictionary<string, string>();

        string dir = HttpContext.Current.Server.MapPath( null );

        string glossaryFile = dir + @"\Help\Glossary.csv";

        if( File.Exists( glossaryFile ) == false ) {
            terms.Add( "Error: File not found", glossaryFile );
            return terms;
        }

        FileStream fs = new FileStream( glossaryFile, FileMode.Open, FileAccess.Read );
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
                string term = StripQuotes( fileLine.Substring( 0, cIndx ) ).Trim();
                string def = StripQuotes( fileLine.Substring( cIndx + 1 ) ).Trim();

                term = term.Replace( (char)65533, '\'' );
                term = term.Replace( "'", "&#39;" );
                def = def.Replace( (char)65533, '\'' );
                def = def.Replace( "'", "&#39;" );

                terms.Add( term, def );
            }
            else {
                // no definition for this term!
                terms.Add( fileLine.Trim(), "" );
            }
        }

        sr.Close();
        fs.Close();

        return terms;
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
