﻿using System;
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

using WebLibrary;
using BusinessLogic;

public partial class FAQ : System.Web.UI.Page
{

    protected void Page_Load( object sender, EventArgs e ) {
        FAQMaker maker = new FAQMaker();
        maker.AddFAQ( this.FAQCell );

        if( this.IsPostBack == false ) {
            DataLogger.LogPageVisit( "FAQ", Utils.GetUser( this ), "-" );
        }
    }

}