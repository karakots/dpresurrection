<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demo1.aspx.cs" Inherits="Demo1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Media Simulation Demo</title>
    <link href="styles/adPlanIt.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/Demo1.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="AdPlanIt.js"></script>
    <script language="javascript">AC_FL_RunContent = 0;</script>
    <script src="AC_RunActiveContent.js" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
   <%--       <div ID="NavBarLinks">
            <asp:LinkButton ID="LinkButton0" runat="server" Text="Home" OnClick="HomeButton_Click"/>|
            <asp:LinkButton ID="ListButton2" runat="server" Text="Saved Media Plans" OnClick="ListButton_Click"/>|
            <asp:LinkButton ID="OverButton2" runat="server" Text="Start a New Plan" OnClick="OverButton_Click"/>
      </div>   --%>
      <asp:Label runat="server" ID="PhoneLabel" Text="Call (800) 518-9202"></asp:Label>  
      </asp:Panel>
        <asp:LoginView ID="LoginView" runat="server">
           <AnonymousTemplate>
                 <div id="SignInDiv" runat="server">
                 Create a <a href="AddUser.aspx">free account</a><br />
                 Returning user? <a href="Login.aspx">Sign In</a>
                 </div>
           </AnonymousTemplate>
           <LoggedInTemplate>
                 <div id="UserNameDiv" runat="server">
                   Logged in as: <asp:LoginName ID="LoginName1" runat="server" />
                   <br />Not you? <a href="Login.aspx">Change user</a>
                 </div>
           </LoggedInTemplate>
       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="SupportInfo.aspx" id="A2" class="nav_support" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about active" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>
          
       <asp:Label ID="BodyInfo" Text="" runat="server"></asp:Label>      
       <asp:Label ID="SubTitle" runat="server" Text="See Media Simulation In Action"/>
       
       <div id="DemoTextBkgDIv" runat="server">&nbsp;</div>
       <div id="DemoTextDiv1" runat="server">Watch as AdPlanit simulates the response of individual households across the USA to a media campaign.
       </div>
       
      <div id="Movie2" runat="server">
       <script language="javascript">
	if (AC_FL_RunContent == 0) {
		alert("This page requires AC_RunActiveContent.js.");
	} else {
		AC_FL_RunContent(
			'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0',
			'width', '640',
			'height', '480',
			'src', 'signup/planit-movie-2',
			'quality', 'high',
			'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
			'align', 'middle',
			'play', 'true',
			'loop', 'true',
			'scale', 'showall',
			'wmode', 'window',
			'devicefont', 'false',
			'id', 'planit-movie-2',
			'bgcolor', '#ffffff',
			'name', 'planit-movie-2',
			'menu', 'true',
			'allowFullScreen', 'false',
			'allowScriptAccess','sameDomain',
			'movie', 'signup/planit-movie-2',
			'salign', ''
			); //end AC code
	}
</script>
</div>
<%-- 
        <div id="DemoFooterDiv" class="DemoFooterDiv">
       <table  id="DemoFooter" class="DemoFooter" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>      
--%>

     </form>
</body>
</html>
