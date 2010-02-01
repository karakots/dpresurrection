<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LostPassword.aspx.cs" Inherits="SetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Retrieve Lost Password</title>
    <link rel="stylesheet" type="text/css" href="~/css/InfoPages.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
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

        <%-- <asp:Image runat="server" ID="Ad1" CssClass="Ad1" ImageUrl="images/336x280ad.gif" />     --%>
             
        <asp:Panel ID="LoginPanel" runat="server" CssClass="LoginPanel" >
        <asp:Label runat="server" ID="DivTitle" >Forgot your password?  Use this form to retrieve it.</asp:Label><br /><br />
        <asp:Label runat="server" ID="EmailLabel" >Email Address:</asp:Label>
        <asp:TextBox runat="server" ID="EmailTextbox"></asp:TextBox>
        <asp:Button runat="server" ID="GoButton" Text="Get Password" OnClick="GoButton_Click" /><br /><br />
        <asp:Label runat="server" ID="SentLabel" Visible="false" ><br /><br />An email has been sent to you with your password.</asp:Label>

        </asp:Panel>
         
        <br />
        <br />

         
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
