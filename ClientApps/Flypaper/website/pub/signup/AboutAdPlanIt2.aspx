<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AboutAdPlanIt2.aspx.cs" Inherits="AboutAdPlanIt2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>About AdPlanIt</title>
    <link href="styles/adPlanIt.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/InfoPages.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/AboutAdPlanit2.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>
       <img class="Logo" src="images/Logo.gif" />
       <%-- <img class="BetaStamp" src="images/Beta1.gif" /> --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      <asp:Label runat="server" ID="PhoneLabel" Text="Call (800) 518-9202"></asp:Label>  
      </asp:Panel>

       <br /><br />
        <ul class="nav">
            <li></li>
            <li><a href="AboutAdPlanIt2.aspx" id="A2" class="nav_support active" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about" ></a></li>
            <li><a href="Index.aspx" id="ctl00_NavHome" class="nav_home" ></a></li>
        </ul>
              
       <%-- End of standard header --%>

       	 <div class="leftColumn">
	  <div class="copy" style="*margin-top: 10px;">
	   <h2><i>Who We Are</i></h2>
	   <p>We have been providing advanced marketing and advertising tools to the biggest brands and advertising companies in the world.  
	    We use proven technology to create virtual test markets of consumers.</p>
        <p>AdPlanit brings the power of these “big company” tools to the small and mid-sized businesses.</p>
	   <h2><i>We Understand The Challenges You Face</i></h2>
        <p>We are a small business ourselves and learned from our own advertising efforts that the world needs a better way to do advertising planning.</p>
        <p>With the confusion in the advertising marketplace, one of the toughest decisions a small business faces is where to spend its advertising  budget.</p>
	   <h2><i>Our Values</i></h2>
        <p>Our commitment is to respect your time, resources and privacy.  Our goal is to provide results that are easy to understand, implement, and maximize 
        the return on your advertising spending. </p>
        <p>Our revenue is derived from arrangements with media providers, but the choice of who to use and how to buy is yours.</p>
	   </div>
	  
       <div id="Footer" class="Footer">
       <table  id="Footer" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>   	 
     </div>
	             
	 <div class="rightColumn">
	  <div class="signupForm" style="*margin-top: 20px;">
		  <h2>Our Management</h2>
		    <a href="mailto:dchalmers@adplanit.com?subject=AdPlanit Beta">Doug Chalmers</a>, CEO<br /><br />
            <a href="mailto:kkarakotsios@adplanit.com?subject=AdPlanit Beta">Ken Karakotsios</a>, President & Co-founder<br /><br />
            <a href="mailto:snoble@adplanit.com?subject=AdPlanit Beta">Steve Noble</a>, PhD, VP Engineering<br /><br />
            <a href="mailto:ddubbe@adplanit.com?subject=AdPlanit Beta">Dean Dubbe</a>, CFO & VP, Operations<br /><br />
	  </div>
	 </div>
	 <div class="rightColumnMid">
     </div>
 	 <div class="rightColumnBottom">
        <h2>Our Advertising Providers</h2>
        <p>The World of All Forms of Media!<br /><br />
        <a href="mailto:sales@adplanit.com?subject=AdPlanit Network Information">Learn how to join our network</a></p>
     </div>

   </div>
    </form>
</body>
</html>
