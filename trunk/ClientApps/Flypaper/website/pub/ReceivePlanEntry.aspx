<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReceivePlanEntry.aspx.cs" Inherits="ReceivePlanEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Receive Media Plan</title>
    <link rel="stylesheet" type="text/css" href="~/css/SendReceive.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
  <form id="form1" runat="server">
  <asp:HiddenField runat="server" ID="ReceivedPlanUser" />
  <asp:HiddenField runat="server" ID="ReceivedPlanID" />
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

        <h1>Receive Media Plan from Another User</h1><br />
        
        <asp:Panel ID="RcvPlanPanel" runat="server" CssClass="RcvPlanPanelWide">
        <table cellpadding="0" cellspacing="5"><tr><td colspan="2">
        Here are the details of the media plan that has been sent to you.<br /><br /> Set the Campaign Name and Plan Name as you wish them to be saved in your account.<br /></td></tr><tr><td>
        &nbsp;</td></tr><tr><td>
        Sender's Email:</td><td>
        <asp:Label runat="server" ID="SenderLabel" Text="Joe@schmo.com"></asp:Label></td></tr><tr><td>
        Campaign Name:</td><td>
        <asp:TextBox ID="CampaignName" runat="server" Text="Campaign 1 (from joe@schmo.com))" CssClass="NameEntry" ></asp:TextBox></td></tr>
        <tr><td>
        Plan Name:</td><td>
        <asp:TextBox ID="PlanName" runat="server" Text="Plan Name"  CssClass="NameEntry"></asp:TextBox>
        </td></tr>
         <tr><td ID="CompetitionNameLabelR" runat="server" >
        Competition Name:</td><td>
        <asp:TextBox ID="CompetitionName" runat="server" Text="none"  CssClass="NameEntry"></asp:TextBox>
        </td></tr>       
        </table><br /><br />
        <asp:Button runat="server" ID="GoButton" Text="Copy Plan to My Account and View" PostBackUrl="~/ReceivePlanCheck.aspx" />
        <asp:Button runat="server" ID="RefuseButton" Text="No Thank You" OnClick="RefuseButton_Click"/>
        </asp:Panel>
                     
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
