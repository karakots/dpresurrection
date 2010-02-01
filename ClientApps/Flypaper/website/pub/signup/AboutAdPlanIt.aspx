<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AboutAdPlanIt.aspx.cs" Inherits="AboutAdPlanIt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>About AdPlanIt</title>
    <link href="styles/adPlanIt.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/InfoPages.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/AboutAdPlanit.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>
       <img class="Logo" src="images/Logo.gif" />
       <%--  <img class="BetaStamp" src="images/Beta1.gif" /> --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      <asp:Label runat="server" ID="PhoneLabel" Text="Call (800) 518-9202"></asp:Label>  
      </asp:Panel>

       <br /><br />
        <ul class="nav">
            <li></li>
            <li><a href="AboutAdPlanIt2.aspx" id="A2" class="nav_support" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about active" ></a></li>
            <li><a href="Index.aspx" id="ctl00_NavHome" class="nav_home" ></a></li>
        </ul>
              
       <%-- End of standard header --%>

       	 <div class="leftColumn">
	  <div class="copy" style="*margin-top: 10px;">
	   <h2><i>Plan</i> Your Advertising Campaign</h2>
	   <p>AdPlanit allows you to create an advertising campaign targeting your customer’s demographics, using your product characteristics.</p>  
	   <p>It suggests ways to improve your plan within your budget and goals.</p>
	   <p>Online and offline advertising types are included -  timing and placement of ads are all part of the solution.</p>
	   <h2><i>Try</i> Your Advertising Before You Buy It</h2>
        <p>AdPlanit is the ultimate test market for your ad campaign.  Use a virtual population of your customers, to improve your plan - before 
        buying advertising!  Get the best return on your advertising dollars.</p>
	   <h2><i>Shop</i> for Media with Confidence</h2>
        <p>Clear results show how your advertising will generate awareness,  persuade potential customers to buy, and how to get the best ad placement and timing.</p>
	  </div>
	  
       <div id="Footer" class="Footer">
       <table  id="Footer" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>   	 
     </div>
	             
	 <div class="rightColumn">
	  <div class="signupForm" style="*margin-top: 20px;">
		  <h2>Learn More</h2>
                Contact us directly at:<br /><br />
                (800) 518-9202<br />
                <a href="mailto:info@adplanit.com?subject=AdPlanit Beta">info@adplanit.com</a><br /><br />
                AdPlanit<br />
                106 East Campbell Avenue<br />
                Campbell, CA 95008<br />
	  </div>
	 </div>
	 <div class="rightColumnMid">
        <a href="Index.aspx"><h2>Sign Up<br />Be The First to Use</h2></a>
     </div>
      
 	 <div class="rightColumnBottom">
       &nbsp;
     </div>

     <asp:Image ID="beta" runat="server" ImageUrl="images/Beta3.gif" />
     <a href="Demo1.aspx"><asp:Image ID="DemoButton" runat="server" ImageUrl="images/DemoButton.gif" /></a>
     
       <div id="Div1" class="Footer">
       <table  id="Table1" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>           

   </div>
    </form>
</body>
</html>
