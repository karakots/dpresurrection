<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit - Where Advertising Starts</title>
    <link href="styles/adPlanIt.css" rel="stylesheet" type="text/css" media="all" />
    <link rel="stylesheet" type="text/css" href="~/styles/Login.css" />     
   <script language="javascript">
    function ShowMainWaitCursor()
        {
            document.body.style.cursor = "progress";
            document.getElementById( "WaitLabel" ).style.visibility = "visible";
            document.getElementById( "ErrorLabel" ).style.visibility = "hidden";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDIv" >
      <%-- Start of standard header --%>
       <img class="Logo" src="images/Logo.gif" />
       <%--  <img class="BetaStamp" src="images/Beta1.gif" /> --%>
       <asp:Label runat="server" ID="TopLabel" Text="Advertise with Confidence!"></asp:Label>  

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      <asp:Label runat="server" ID="PhoneLabel" Text="Call (800) 518-9202"></asp:Label>  
      </asp:Panel>

       <br /><br />
        <ul class="nav">
            <li></li>
            <!-- TBD need some knd of nav? SSN
            <li><a href="Help.aspx" id="A3" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A4" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home active"  onclick="return true;" ></a></li>
            -->
        </ul>
              
       <%-- End of standard header --%>

       	 <div class="leftColumn">
	  <div class="copy" style="margin-top: 10px;">
	   <h3>Advertise with Confidence</h3>
	  </div>
	  
	 <div id="FooterDiv" class="FooterDiv">
       <table  id="Footer" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center" style="font-family: Verdana, Arial, sans-serif; font-size: 9px; color: #668; line-height: 1.5em; padding: 0; font-weight:700;">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"  style="font-family: Verdana, Arial, sans-serif; font-size: 9px; color: #668; line-height: 1.5em; padding: 0; font-weight:700;"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150"  style="font-family: Verdana, Arial, sans-serif; font-size: 9px; color: #668; line-height: 1.5em; padding: 0; font-weight:700;" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>           
	 </div>
	 <div class="rightColumn">
	  <div class="signupForm" style="margin-top: 10px;">
		  <h2>User Login</h2>

        <asp:Panel ID="LoginPanel" runat="server" CssClass="LoginPanel" >
           <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Campaigns.aspx"  CssClass="UserLogin"  >
               <LayoutTemplate> 
                   <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                       <tr>
                           <td>
                               <table border="0" cellpadding="0">
                                   <tr>
                                       <td align="left" colspan="2" style="font-size:11pt;font-weight:600;padding:3px">
                                           Log In to your AdPlanit Account</td>
                                   </tr>
                                   <tr>
                                       <td align="right">
                                           <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label></td>
                                       <td>
                                           <asp:TextBox ID="UserName" runat="server" CssClass="UserLoginControl"></asp:TextBox><br />
                                           <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                               ErrorMessage="Email is required." ToolTip="Email is required." ValidationGroup="Login1">Required!</asp:RequiredFieldValidator>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="right">
                                           <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></td>
                                       <td>
                                           <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="UserLoginPwControl"></asp:TextBox><br />
                                           <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                               ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">Required!</asp:RequiredFieldValidator>
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
                                       <td align="left" colspan="2">
                                       <div runat="server" id="LoginButtonDiv" >
                                           <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Proceed" ValidationGroup="Login1" CssClass="LoginButton"/>
                                           </div>
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                   </table>
               </LayoutTemplate>
           </asp:Login>
        </asp:Panel>
		   
		   <div id="LostPwDiv" >
		        <a href="LostPassword.aspx" id="LostPwLink">Forgot your password? Click here.</a>
		   </div>
		    <div id="NewUserDiv" >
		        <a runat="server" href="signup/Index.aspx" id="NewUserLink">New user? Click here to sign up.</a>
		   </div>
	  </div>
	 </div>
       
          </div>


    </form>
</body>
</html>
