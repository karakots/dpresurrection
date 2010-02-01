<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scoreboard.aspx.cs" Inherits="Scoreboard" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Campaign Plan Scoreboard</title>
    <link rel="stylesheet" type="text/css" href="~/css/SendReceive.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
    <div id="InfoDiv" runat="server" visible="true">
        <h1>Campaign Plan Scoreboard</h1>
        <br /><br />
        <span runat="server" id="CompetitionNameLabel" >&nbsp;</span><br /><br />
        <span runat="server" id="ResultsData" >No Results</span>
    </div>
    </form>
</body>
</html>
