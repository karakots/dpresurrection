<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrivacyPolicy.aspx.cs" Inherits="PrivacyPolicy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Privacy and Legal</title>
    <link href="styles/adPlanIt.css" rel="stylesheet" type="text/css" media="all" />
    <link href="styles/InfoPages.css" rel="stylesheet" type="text/css" media="all" />
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
            <li><a href="AboutAdPlanIt2.aspx" id="A2" class="nav_support" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about active" ></a></li>
            <li><a href="Index.aspx" id="ctl00_NavHome" class="nav_home" ></a></li>
        </ul>
              
       <%-- End of standard header --%>
              
       <asp:Label ID="BodyInfo" Text="" runat="server"></asp:Label>      
       
       <asp:Label ID="SubTitle" runat="server" Text="Privacy and Legal"/>
       
<div id="PrivacyTextDiv">
       <h3>Privacy Policy</h3>

AdPlanit™ values your privacy and wants you to fully understand our terms and conditions. 
If you provide your business or personal information to AdPlanit, we may: <ul>
<li>Gather and store the information you provide to us through our Web site. </li>
<li>Create standard usage logs and reports through our Web server. </li>
<li>Send you email and/or postal mail updates on corporate information, service. announcements, product releases and general programs. </li>
<li>Request additional information (such as your name, address, telephone number, email address or other data) to help us better serve you, 
to promote our products and services or to engage in a further dialogue. </li>
</ul>
We will not: <ul>
<li>Release the information you provided to anyone else without your express consent. </li>
<li>Sell your information to third parties. </li></ul>
<h3>Opt-out information</h3>
If you do not wish to receive future email or postal mail messages from us, please let us know via email at 
<a href="mailto:webmaster@Adplanit.com?subject=Please Discontinue AdPlanit Emails" >webmaster@Adplanit.com</a> or by calling (800) 518-9202. 
<h3>Legal</h3>
All content and graphics on the AdPlanit Web site are protected by U.S. copyright and international treaties. They may not be copied, reproduced, 
republished, uploaded, posted, transmitted or distributed in any way. You may, however, download one copy of the materials for your personal, 
non-commercial use only provided that you keep intact all copyright and other proprietary notices. 
Reuse of any of the online content and graphics for any purpose is strictly prohibited unless you receive the express permission of DecisionPower, Inc.
 DecisionPower, Inc. reserves all rights. 
AdPlanit, "Advertise with Confidence”, “Where Advertising Starts” the AdPlanit logo and corporate identity, including the "look and feel" of the AdPlanit Web site, 
are trademarks of DecisionPower, Inc. 

       <div id="Footer" class="Footer">
       <table  id="Footer" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
                     <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
      </div>           

</div>

            </div>
    </form>
</body>
</html>
