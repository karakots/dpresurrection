<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReceivePlan.aspx.cs" Inherits="ReceivePlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Receive Media Plan</title>
    <link rel="stylesheet" type="text/css" href="~/css/SendReceive.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body runat="server" id="PageBody" >
  <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
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

        <h1>Receive Media Plan from Another User</h1><br />
        
        <asp:Panel ID="RcvPlanPanel" runat="server" CssClass="RcvPlanPanel">
        <br />To receive the AdPlanit media plan you have been sent, simply log in here.  The media plan will be copied to your account.<br /><br />
        Don't have an AdPlanit account already? <a runat="server" href="signup/Index.aspx" id="NewUserLink">Create a new free account,</a> 
        and in minutes you can be viewing this media plan. (Click the email link again after creating your account.)
        </asp:Panel>
        
        <asp:HiddenField ID="ReceivedPlanID" runat="server" />
        <asp:HiddenField ID="ReceivedPlanUser" runat="server" />
             
        <asp:Panel ID="LoginPanel" runat="server" CssClass="LoginPanel" >

<asp:LoginView runat="server" ID="LoginView">
<AnonymousTemplate>

           <asp:Login ID="UserLogin" runat="server"  DestinationPageUrl="~/ReceivePlan.aspx" CssClass="UserLogin"  >
               <LayoutTemplate> 
                   <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                       <tr>
                           <td>
                               <table border="0" cellpadding="0">
                                   <tr>
                                       <td align="left" colspan="2" style="font-size:8pt;padding:3px; font-weight:600;">
                                           Login</td>
                                   </tr>
                                   <tr>
                                       <td style="padding-left:7px;">
                                           <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label><asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                               ErrorMessage="Email is required." ToolTip="Email is required." ValidationGroup="Login1"> value Required!</asp:RequiredFieldValidator><br />
                                           <asp:TextBox ID="UserName" runat="server" CssClass="UserLoginControl"></asp:TextBox><br />
                                           
                                       </td>
                                   </tr>
                                   <tr>
                                       <td style="padding-left:7px">
                                           <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label><asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                               ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1"> value Required!</asp:RequiredFieldValidator><br />
                                           <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="UserLoginPwControl"></asp:TextBox><br />
                                           
                                       </td>
                                   </tr>
                                   <%-- 
                                   <tr>
                                       <td colspan="2" align="left" >
                                           <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." TextAlign="right" />
                                       </td>
                                 </tr>--%>
                                   <tr>
                                       <td align="left" colspan="2" style="color: red">
                                           <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="right" colspan="2">
                                       <div runat="server" id="LoginButtonDiv" >
                                           <asp:ImageButton ID="LoginButton" runat="server" CommandName="Login"  ImageUrl="images/Button-Login.gif" ValidationGroup="Login1" CssClass="LoginButton"/>
                                           </div>
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                   </table>
               </LayoutTemplate>
           </asp:Login>
		   <div id="LostPwDiv" >
		        <a href="LostPassword.aspx" id="LostPwLink">Forgot password?</a>
		   </div>

</AnonymousTemplate>
<LoggedInTemplate>
</LoggedInTemplate>
</asp:LoginView>
           
        </asp:Panel>         
        <br />
        <br />

         
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
