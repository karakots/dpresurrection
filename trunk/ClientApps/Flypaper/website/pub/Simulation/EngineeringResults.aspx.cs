using System;
using System.Collections;
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

using WebLibrary;
// using BusinessLogic;

public partial class Simulation_EngineeringResults : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {

        MediaPlan plan = Utils.CurrentMediaPlan( this, false );

        if( plan.Results != null )
        {
            ResultsListMaker results_maker = new ResultsListMaker( plan );

            results_maker.AddResultsListHTML( this.ResultTableDiv, "", true );
        }
    }
}
