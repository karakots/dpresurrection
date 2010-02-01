<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendPlan.aspx.cs" Inherits="SendPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Send Media Plan</title>
    <link rel="stylesheet" type="text/css" href="~/css/SendReceive.css" />     
    <script type="text/javascript" src="AdPlanit.js"></script>
</head>
<body runat="server" id="PageBody">
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="CompetitionMode" value="false" />
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="#" id="A2" class="nav_help active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
        <h1>Send Media Plan to Another Person</h1><br />
             
        <asp:Panel ID="SendPlanPanel" runat="server" CssClass="SendPlanPanel" >
        <asp:Label runat="server" ID="DivTitle" >Use this form to send a copy of this media plan to someone else.&nbsp;&nbsp;They will receive an email 
        from support@adplanit.com with your message and a link they can click to get their own copy of this plan.
        </asp:Label><br /><br />
        <asp:Label runat="server" ID="CompetitonLabel" >Want to start a competition to see who can make the best plan for this campaign?  If you do, check this box:</asp:Label>
        <input type=checkbox runat="server" ID="CompetitionSelection" title="Start a Competition" onclick='ShowCompetitionControls();' /><br /><br />
        <table id="SendInfoTable" cellpadding="0" cellspacing="0"><tr><td>
        <asp:Label runat="server" ID="Label0" >Plan&nbsp;to&nbsp;Send:</asp:Label></td><td colspan="2">
        <asp:Label runat="server" ID="PlanName" CssClass="PlanInfo" >(plan name?)</asp:Label></td></tr><tr><td>
        <asp:Label runat="server" ID="Label3" >Campaign:</asp:Label></td><td colspan="2">
        <asp:Label runat="server" ID="CampaignName" CssClass="PlanInfo" >(campaign name?)</asp:Label></td></tr>
        <tr><td>
        <asp:Label runat="server" ID="Label4" >Receiver's&nbsp;Name(s):</asp:Label></td><td colspan="2">
        <asp:TextBox runat="server" ID="NameTextbox" CssClass="PlanInfoInput"></asp:TextBox></td></tr><tr><td>
        <asp:Label runat="server" ID="EmailLabel" >Receiver's&nbsp;Email(s):</asp:Label></td><td>
        <asp:TextBox runat="server" ID="ReceiverEmail" CssClass="PlanInfoInput"></asp:TextBox><br /><asp:Label runat="server" ID="EmailInfoLabel" >Separate multiple email addresses with semicolons.</asp:Label></td><td>
        <asp:Label runat="server" ID="InvalidEmail" CssClass="InputError" Text="Error:&nbsp;Invalid&nbsp;email&nbsp;address" Visible="false"></asp:Label></td></tr>
        <tr><td><asp:Label runat="server" ID="CompNameLabel"  CssClass="CompVisHidden" >Competition Title:</asp:Label>
        </td><td colspan="2">
        <asp:TextBox runat="server" ID="CompetitionName" CssClass="CompVisHidden" Text="AdPlanit Online Challenge"></asp:TextBox>
        </td></tr>        
        <tr><td colspan="3">
        <asp:Label runat="server" ID="Label2" >Message:</asp:Label></td></tr><tr><td colspan="3">
        <asp:TextBox runat="server" ID="MessageTextBox" Height="150" Width="700" TextMode="MultiLine"></asp:TextBox><br /><br />
        </td></tr></table><br />
        <asp:Button runat="server" ID="PreviewButton" Text="Preview Message"  OnClick="PreviewButton_Click" />
        <asp:Button runat="server" ID="GoButton" Text="Send Now" OnClick="GoButton_Click" /><br /><br />
        <asp:Label runat="server" ID="SentLabel" Visible="false" ><br /><br />An email has been sent to you with your password.</asp:Label>

        </asp:Panel>
         
        <br />
        <br />

         
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
