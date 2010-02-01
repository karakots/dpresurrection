using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

public partial class admin_LogCheck : System.Web.UI.Page
{
    private const string logFileFormat = "log-{0}-{1}.txt";

    private List<string> allLines;

    protected void Page_Load( object sender, EventArgs e ) {

        InitiaIzeUI();

         string user = Request["user"];

        string logfile = String.Format( logFileFormat, this.YearList.SelectedValue, this.MonthList.SelectedValue );

        allLines = ReadLogFile( logfile );

        if( allLines != null ) {
            ShowSummary();
            FillUsersMenu();

            if( user != "" && user != null )
            {
                UserList.SelectedValue = user;
            }

            ShowUserDetail();
        }

       
    }

    private void ShowUserDetail() {
        GetUserLogData( null, null );
    }

    private void FillUsersMenu() {
        if( IsPostBack == false ) {
            List<string> users = GetUsers();
            UserList.Items.Clear();
            UserList.Items.Add( new ListItem( "(select user)", "-" ) );
            foreach( string user in users ) {
                UserList.Items.Add( new ListItem( user ) );
            }
        }
    }

    /// <summary>
    /// Displays the data summary.
    /// </summary>
    private void ShowSummary() {
        List<string> users = GetUsers();
        AddSummaryHeaderRow();
        bool alt = false;
        foreach( string user in users ) {
            AddSummaryRow( user, alt );
            alt = !alt;
        }
    }

    /// <summary>
    /// Adds the header to the summary table.
    /// </summary>
    private void AddSummaryHeaderRow() {
        TableRow row = new TableRow();

        TableCell c1 = new TableCell();
        TableCell c2 = new TableCell();
        TableCell c3 = new TableCell();
        TableCell c4 = new TableCell();
        TableCell c5 = new TableCell();
        TableCell c6 = new TableCell();
        TableCell c7 = new TableCell();

        c1.Text = "User ID";
        c2.Text = "Logins";
        c3.Text = "Campaigns Created";
        c4.Text = "Plans Generated";
        c5.Text = "Plans Entered";
        c6.Text = "Simulations Run";
        c7.Text = "Suggested Improvements";

        c1.CssClass = "summaryHeaderCell";
        c2.CssClass = "summaryHeaderCell";
        c3.CssClass = "summaryHeaderCell";
        c4.CssClass = "summaryHeaderCell";
        c5.CssClass = "summaryHeaderCell";
        c6.CssClass = "summaryHeaderCell";
        c7.CssClass = "summaryHeaderCell";

        row.Cells.Add( c1 );
        row.Cells.Add( c2 );
        row.Cells.Add( c3 );
        row.Cells.Add( c4 );
        row.Cells.Add( c5 );
        row.Cells.Add( c6 );
        row.Cells.Add( c7 );
        SummaryTable.Rows.Add( row );
    }

    /// <summary>
    /// Gets the summary data for the given user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private List<int> GetSummaryData( string user ) {
        List<string> userLines = new List<string>();
        for( int i = 0; i < this.allLines.Count; i++ ) {
            if( GetUserIDFrom( allLines[ i ] ) == user ) {
                userLines.Add( allLines[ i ] );
            }
        }

        List<int> userData = new List<int>();
        for( int i = 0; i < 7; i++ ) {
            userData.Add( 0 );
        }
        foreach( string line in userLines ) {
            string code = GetCodeFrom( line, false );
            switch( code ) {
                case "LOGIN":
                    userData[ 0 ] += 1;
                    break;
                case "CREATECAM":
                    userData[ 1 ] += 1;
                    break;
                case "GEN-C":
                case "GEN-CL":
                case "GEN-HQ":
                    userData[ 2 ] += 1;
                    break;
                case "CREATE-C":
                case "CREATE-CL":
                    userData[ 3 ] += 1;
                    break;
                case "SIMDONE":
                    userData[ 4 ] += 1;
                    break;
                case "GEN-ISCN":
                case "GEN-ISME":
                    userData[ 5 ] += 1;
                    break;
            }
        }

        return userData;
    }

    /// <summary>
    /// Adds a row of summary data for the given user.
    /// </summary>
    /// <param name="user"></param>
    private void AddSummaryRow( string user, bool alt ) {
        List<int> data = GetSummaryData( user );

        TableRow row = new TableRow();

        TableCell c1 = new TableCell();
        TableCell c2 = new TableCell();
        TableCell c3 = new TableCell();
        TableCell c4 = new TableCell();
        TableCell c5 = new TableCell();
        TableCell c6 = new TableCell();
        TableCell c7 = new TableCell();

        c1.Text = user;
        c2.Text = data[ 0 ].ToString();
        c3.Text = data[ 1 ].ToString();
        c4.Text = data[ 2 ].ToString();
        c5.Text = data[ 3 ].ToString();
        c6.Text = data[ 4 ].ToString();
        c7.Text = data[ 5 ].ToString();

        string class1 = "summaryDataCellX";
        string class2 = "summaryDataCell";
        if( alt ) {
            class1 = "summaryDataCellXAlt";
            class2 = "summaryDataCellAlt";
            c1.CssClass = class2;
        }

        c2.CssClass = class1;
        c3.CssClass = class2;
        c4.CssClass = class1;
        c5.CssClass = class2;
        c6.CssClass = class1;
        c7.CssClass = class2;

        c2.Attributes.Add( "style", "padding-left:20px;" );
        c3.Attributes.Add( "style", "padding-left:20px;" );
        c4.Attributes.Add( "style", "padding-left:20px;" );
        c5.Attributes.Add( "style", "padding-left:20px;" );
        c6.Attributes.Add( "style", "padding-left:20px;" );
        c7.Attributes.Add( "style", "padding-left:20px;" );

        row.Cells.Add( c1 );
        row.Cells.Add( c2 );
        row.Cells.Add( c3 );
        row.Cells.Add( c4 );
        row.Cells.Add( c5 );
        row.Cells.Add( c6 );
        row.Cells.Add( c7 );
        SummaryTable.Rows.Add( row );
    }

    /// <summary>
    /// Returns the name of the log file for the selected month,
    /// </summary>
    /// <returns></returns>
    private string SelectedLogFile() {
        return String.Format( logFileFormat, this.YearList.SelectedValue, this.MonthList.SelectedValue );
    }

    /// <summary>
    /// Reads and returns the lines of data in the log file with the given name.  Returns null if the file does not exist.
    /// </summary>
    /// <param name="logFileName"></param>
    /// <returns></returns>
    private List<string> ReadLogFile( string logFileName ) {
        string logfilePath = ConfigurationSettings.AppSettings[ "AdPlanit.UserPlanStorageRoot" ] + " \\" + logFileName;
        if( File.Exists( logfilePath ) == false ) {
            SummaryTable.Rows.Clear();
            TableRow row = new TableRow();
            TableCell c1 = new TableCell();
            c1.Text = "Warning: No Log File Found for Selected Date";
            c1.Attributes.Add( "style", "font-size:10pt; font-weight:bold; color:#240; padding-top:40px; padding-left:40px; padding-bottom:40px; padding-right:340px;" );
            row.Cells.Add( c1 );
            SummaryTable.Rows.Add( row );
            return null;
        }

        FileStream fs = new FileStream( logfilePath, FileMode.Open, FileAccess.Read );
        StreamReader sr = new StreamReader( fs );

        List<string> fileLines = new List<string>();
        string line = null;
        while( (line = sr.ReadLine()) != null ) {
            fileLines.Add( line );
        }
        sr.Close();
        fs.Close();
        
        return fileLines;
    }

    /// <summary>
    /// Sets up the UI control state.
    /// </summary>
    private void InitiaIzeUI() {
        if( this.IsPostBack == false ) {
            this.MonthList.SelectedValue = DateTime.Now.Month.ToString();
            this.YearList.SelectedValue = DateTime.Now.Year.ToString();
        }
    }

    /// <summary>
    /// Returns the list of all user IDs in the current log data.
    /// </summary>
    /// <returns></returns>
    private List<string> GetUsers() {
        List<string> users = new List<string>();
        foreach( string line in this.allLines ) {
            string user = GetUserIDFrom( line );
            if( user != null && users.Contains( user ) == false ) {
                users.Add( user );
            }
        }
        return users;
    }

    /// <summary>
    /// Gets the date from the given log data line.
    /// </summary>
    /// <param name="logLine"></param>
    /// <returns></returns>
    private DateTime GetDateFrom( string logLine ) {
        string user = null;
        DateTime d = new DateTime();
        int n1 = logLine.IndexOf( " - " );
        if( n1 != -1 ) {
            string s = logLine.Substring( 0, n1 );
            d = DateTime.Parse( s );
        }
        return d;
    }

    /// <summary>
    /// Gets the user ID from the given log data line.
    /// </summary>
    /// <param name="logLine"></param>
    /// <returns></returns>
    private string GetUserIDFrom( string logLine ) {
        string user = null;
        int n1 = logLine.IndexOf( " - " );
        if( n1 != -1 ) {
            int n2 = logLine.IndexOf( " ", n1 + 3 );
            string s2 = logLine.Substring( n2 + 1 );
            if( s2.IndexOf( "," ) != -1 ) {
                s2 = s2.Substring( 0, s2.IndexOf( "," ) );
            }
            int m1 = s2.IndexOf( "@" );
            int m2 = s2.LastIndexOf( "." );
            if( m1 != -1 && m2 > m1 ) {
                user = s2;
            }
        }
        return user;
    }

    /// <summary>
    /// Gets the action code from the given log data line, in either raw ir user-friendly form.
    /// </summary>
    /// <param name="logLine"></param>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private string GetCodeFrom( string logLine, bool userCode ) {
        string code = null;
        int n1 = logLine.IndexOf( " - " );
        if( n1 != -1 ) {
            int n2 = logLine.IndexOf( " ", n1 + 3 );
            code = logLine.Substring(  n1 + 3, n2 - n1 - 3 );
        }

        string ucode = code;
        switch( code ) {
            case "LOGIN":
                ucode = "Logged in";
                break;
            case "CREATECAM":
                ucode = "Created New Campaign";
                break;
            case "CREATE-C":
                ucode = "Create Plan <small>(from Campaign page)</small>";
                break;
            case "CREATE-CL":
                ucode = "Create Plan <small>(from Campaign list page)</small>";
                break;
            case "GEN-C":
                ucode = "Autogenerated Plan <small>(from Campaign page)</small>";
                break;
            case "GEN-CL":
                ucode = "Autogenerated Plan <small>(from Campaign list page)</small>";
                break;
            case "GEN-HQ":
                ucode = "Autogenerated Plan <small>(from Plan Summary page)</small>";
                break;
            case "GEN-ISCN":
                ucode = "Utilized Suggested Improvement <small>(create new)</small>";
                break;
            case "GEN-ISME":
                ucode = "Utilized Suggested Improvement <small>(modify existing)</small>";
                break;
            case "SIMDONE":
                ucode = "Ran Simulation";
                break;
            case "VIEW":
                string info = GetInfoFrom( logLine );
                string pg = info;
                if( info.IndexOf( "," ) != -1 ) {
                    pg = info.Substring( 0, info.IndexOf( "," ) );
                }
                ucode = String.Format( "Viewed {0} Page", pg );
                break;
        }
        if( userCode ) {
            return ucode;
        }
        else {
            return code;
        }
    }

    /// <summary>
    /// Gets the additional info from the given log line.
    /// </summary>
    /// <param name="logLine"></param>
    /// <returns></returns>
    private string GetInfoFrom( string logLine ) {
        string info = "";
        string userID = UserList.SelectedValue;
        int n1 = logLine.IndexOf( userID );
        if( n1 != -1 ) {
            info = logLine.Substring( n1 + userID.Length );
            if( info.StartsWith( "," ) ) {
                info = info.Substring( 1 );
            }
        }
        return info;
    }

    protected void GetUserLogData( object sender, EventArgs e ) {
        List<string> userLines = new List<string>();
        for( int i = 0; i < this.allLines.Count; i++ ) {
            if( GetUserIDFrom( allLines[ i ] ) == UserList.SelectedValue ) {
                userLines.Add( allLines[ i ] );
            }
        }

        DetailsTable.Rows.Clear();

        bool alt = false;

        DateTime curDate = new DateTime();
        for( int i = 0; i < userLines.Count; i++ ) {
            TableRow row = new TableRow();
            TableCell c1 = new TableCell();
            TableCell c2 = new TableCell();
            TableCell c3 = new TableCell();
            TableCell c4 = new TableCell();
            TableCell c5 = new TableCell();

            DateTime lineDate = GetDateFrom( userLines[ i ] );
            if( lineDate.Day != curDate.Day || lineDate.Month != curDate.Month || lineDate.Year != curDate.Year ) {
                // it is a different day
                if( lineDate.Year == curDate.Year ) {
                    c1.Text = String.Format( "{0}, {1}<br>", lineDate.DayOfWeek, lineDate.ToString( "MMM d" ) );
                }
                else {
                    c1.Text = String.Format( "{0}, {1}<br>", lineDate.DayOfWeek, lineDate.ToString( "MMM d, yyyy" ) );
                }
                c1.Attributes.Add( "style", "background-color:#DDD" );
                c1.ColumnSpan = 5;
                row.Cells.Add( c1 );
            }
            else {
                // add a full detail row
                c1.Text = lineDate.ToString( "h:mm" );
                c2.Text = lineDate.ToString( "tt" );
                c2.Attributes.Add( "style", "font-size:7pt;" );

                c3.Text = GetCodeFrom( userLines[ i ], true );

                string uinfo = GetInfoFrom( userLines[ i ] );
                if( uinfo.IndexOf( "," ) == -1 ) {
                    c4.Text = uinfo;
                    c5.Text = "";
                }
                else {
                    string[] ss = uinfo.Split( ',' );
                    if( c3.Text.StartsWith( "View" ) == false ) {
                        c4.Text = ss[ 0 ];
                        c5.Text = ss[ 1 ];
                    }
                    else {
                        c4.Text = ss[ 1 ];
                        c5.Text = "";
                    }
                }

                if( c4.Text != "" && c4.Text != "-" ) {
                    c4.Text = "&quot;" + c4.Text + "&quot;";
                }

                if( alt ) {
                    row.Attributes.Add( "style", "background-color: #EEE" );
                }
                else {
                }
                alt = !alt;

                row.Cells.Add( c1 );
                row.Cells.Add( c2 );
                row.Cells.Add( c3 );
                row.Cells.Add( c4 );
                row.Cells.Add( c5 );
            }
            DetailsTable.Rows.Add( row );

            curDate = lineDate;
        }
    }

    // ----------------- OLD -----------------

    //protected void GetLogData( object sender, EventArgs e ) {
    //    if( this.allLines == null ) {
    //        return;
    //    }

    //    List<string> users = GetUsers();

    //    string s = "";
    //    UserList.Items.Clear();
    //    foreach( string u in users ) {
    //        UserList.Items.Add( new ListItem( u ) );
    //        s += u + "<br>";
    //    }

    //    ResultsLabel.Text = s;
    //}


}
