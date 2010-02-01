<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Compete.aspx.cs" Inherits="Compete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Enter Media Plan in Competition</title>
    <link rel="stylesheet" type="text/css" href="~/css/SendReceive.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="ReceivedPlanUser" />
    <asp:HiddenField runat="server" ID="ReceivedPlanID" />
    <asp:HiddenField runat="server" ID="CampaignName" />
    <asp:HiddenField runat="server" ID="PlanName" />
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="#" id="A2" class="nav_help active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>

        <h1>Enter Media Plan in a Competition</h1><br />
        
        <asp:Panel ID="RcvPlanPanel" runat="server" CssClass="RcvPlanPanelWide">
        <asp:Label runat="server" ID="ErrorLabel" Text="<br><br>Transferring plan.<br><br>Please wait while the transfer is processed..." CssClass="ErrorLabel" ></asp:Label><br /><br /><br />
        <asp:Button runat="server" ID="OKButton" Text="OK" PostBackUrl="~/Analysis.aspx"/>
        <asp:Label runat="server" ID="InfoLabel" Text="(info)" style="color:White;" ></asp:Label>
        </asp:Panel>
                     
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
