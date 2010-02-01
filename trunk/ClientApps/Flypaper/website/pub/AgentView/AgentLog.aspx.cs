using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using WebLibrary;
using MediaLibrary;

public partial class AgentView_AgentLog : System.Web.UI.Page
{

    MediaPlan planToSimulate = null;

    protected void Page_Load( object sender, EventArgs e )
    {
      string agentStr = this.Request.Params["agent"];

        if (agentStr == null)
        {
            // display suitable message
            return;
        }

          planToSimulate = Utils.CurrentMediaPlan( this, false );

        if( planToSimulate == null)
        {
            // display suitable message
            return;
        }

        int agentIndex = Int32.Parse(agentStr);

        DirectoryInfo di = PlanStorage.MoviePath( Utils.GetUser( this ), planToSimulate );

        AgentLog log = PlanStorage.ReadAgentLog( di, agentIndex );

        this.AgentInfo.InnerHtml =  Utils.AgentHtml( log.agent, "x-small" );

        this.AgentMediaInfo.InnerHtml = Utils.MediaHtml( log.agent, "x-small", 0 );

        this.LogDiv.InnerHtml = LogHtml( log );

    }


    private double maxActionTaken = 0;
    private double maxPersuasion = 0;
    private string LogHtml( AgentLog diary )
    {
          
        AgentLog.Summary summary = diary.ComputeSummary();
        
        string html = "<table border=\"1\">";
        html += "<tr>";
        html += "<th>Day</th>";
        html += "<th>Aware</th>";
        html += "<th>In Consideration</th>";
        html += "<th>Choosing</th>";
        html += "<th>Impressions</th>";
        html += "<th>Persuasion</th>";
        html += "<th>Percent Action</th>";

        // media
        html += "<td>";
        html += "<table width = \"500\">";
        html += "<tr>";
        html += "<th colspan=\"3\">Media</th>";
        html += "</tr>";
        html += "<tr>";
        html += "<th width = \"300\">option</th>";
        html += "<th width = \"100\">state</th>";
        html += "<th width = \"100\">persuasion conferred</th>";
        html += "</tr>";
        html += "</table>";
        html += "</td>";
        html += "</tr>";


        // summary
        html += "<td><b>Avg</b></td>";
        html += "<td>" + ((int) Math.Min(100, Math.Ceiling(100 * summary.Aware ))).ToString() + "%</td>";
        html += "<td>" + ((int) Math.Min(100, Math.Ceiling(100 * summary.Consideration ))).ToString() + "%</td>";
        html += "<td>" + ((int)Math.Ceiling(summary.MadeChoice)).ToString() + "</td>";
      
        html += "<td>" + summary.NumImpressions.ToString("F") + "</td>";
        html += "<td>" + summary.Persuasion.ToString("F") + "</td>";
        html += "<td>" + (100 * summary.ActionTaken).ToString("F") + "</td>";

        html += "<td>";
        html += "<table  width = \"300\">";
        html += "<tr>";
        html += "<td colspan=\"3\">";
        html += "---";
        html += "</td>";
        html += "</tr>";
        html += "</table>";
        html += "</td>";

        foreach( DailyLog log in diary.Logs.Values )
        {
            maxActionTaken = Math.Max( maxActionTaken, log.ActionTaken );
            maxPersuasion = Math.Max( maxPersuasion, log.Persuasion );
        }

        foreach( DailyLog log in diary.Logs.Values )
        {
            html += "<tr>";
            html += DailyLogHtml( log );
            html += "</tr>";
        }
        html += "</table>";

        return html;
    }

    private string DailyLogHtml( DailyLog log )
    {
        string html = "<td><b> Day " + log.Day.ToString() + "</b></td>";

        if( log.Aware )
        {
            html += "<td  bgcolor=\"yellow\">&nbsp</td>";
        }
        else
        {
            html += "<td>&nbsp</td>";
        }

        if( log.in_consideration )
        {
            
            html += "<td  bgcolor=\"green\">&nbsp</td>";
        }
        else
        {
            html += "<td>&nbsp</td>";
        }

        if( log.MadeChoice )
        {
            html += "<td  bgcolor=\"red\">&nbsp</td>";
        }
        else
        {
            html += "<td>&nbsp</td>";
        }

        // get color from number

        string hexVal = hexFromInt( log.NumImpressions, 4 );

        html += "<td bgcolor=\"" + hexVal + "\">" + log.NumImpressions.ToString("F") + "</td>";

        hexVal = hexFromInt( log.Persuasion, maxPersuasion );
        html += "<td bgcolor=\"" + hexVal + "\">" + log.Persuasion.ToString("F") + "</td>";

        hexVal = hexFromInt( log.ActionTaken, maxActionTaken);
        html += "<td bgcolor=\"" + hexVal + "\">" + (100 * log.ActionTaken).ToString("F") + "</td>";

        html += "<td>";

        html += "<table  width = \"500\">";

        if( log.media.Count > 0 )
        {
            foreach( MediaRecord record in log.media )
            {
                html += "<tr>";
                // html += "vcl id" + record.vehicle.ToString(); // TBD get vcl name from database
                html += "<td  width = \"300\">" + record.option.Name.ToString() + "</td>";

                html += "<td  width = \"100\">";
                if( record.MadeAware && record.MadeRecent)
                {
                    html += "Made Recently Aware";
                }
                else if( record.MadeAware )
                {
                    html += "Made Aware";
                }
                else
                {
                    html += "No Change";
                }
                html += "</td>";

                html += "<td  width = \"100\">";
                html += record.PersuasionConferred.ToString("F");
                html += "</td>";
                html += "<tr>";
            }
        }
        else
        {
            html += "<tr>";
            html += "<td colspan=\"3\">";
            html += "none";
            html += "</td>";

            html += "</tr>";
        }

        html += "</table>";
        html += "</td>";

        return html;
    }

    private string hexFromInt( double val, double max )
    {

        if( val == 0 )
        {
            return "#FFFFFF";
        }

         int hex = 255;

         if( max > 0 )
         {
             hex = (int)Math.Ceiling( 255.0 * (1.0 - val / max) );


             hex = Math.Min( 255, hex );

             hex = Math.Max( 0, hex );
         }

        string hexOutput = String.Format( "{0:X}", hex );

        if (hex < 16)
        {
            hexOutput = "0" + hexOutput;
        }

        return "#00" + hexOutput + "FF";

    }
}
