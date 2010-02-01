<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TermsAndConditions.aspx.cs" Inherits="About" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AdPlanit Terms and Conditions</title>
    <link rel="stylesheet" type="text/css" href="~/css/Common.css" />     
    <link rel="stylesheet" type="text/css" href="~/css/InfoPages.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
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
            <li><a href="Support.aspx" id="A2" class="nav_help active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
        <div id="ContentDiv" runat="server" visible="true">
            <table id="ContentTable" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div id="PrivacyTextDiv">
                            <h3>
                                Privacy Policy</h3>
                            AdPlanit™ values your privacy and wants you to fully understand our terms and conditions.
                            If you provide your business or personal information to AdPlanit, we may:
                            <ul>
                                <li>Gather and store the information you provide to us through our Web site. </li>
                                <li>Create standard usage logs and reports through our Web server. </li>
                                <li>Send you email and/or postal mail updates on corporate information, service. announcements,
                                    product releases and general programs. </li>
                                <li>Request additional information (such as your name, address, telephone number, email
                                    address or other data) to help us better serve you, to promote our products and
                                    services or to engage in a further dialogue. </li>
                            </ul>
                            We will not:
                            <ul>
                                <li>Release the information you provided to anyone else without your express consent.
                                </li>
                                <li>Sell your information to third parties. </li>
                            </ul>
                            <h3>
                                Opt-out information</h3>
                            If you do not wish to receive future email or postal mail messages from us, please
                            let us know via email at <a href="mailto:webmaster@Adplanit.com?subject=Please Discontinue AdPlanit Emails">
                                webmaster@Adplanit.com</a> or by calling (800) 518-9202.
                            <h3>
                                Legal</h3>
                            All content and graphics on the AdPlanit Web site are protected by U.S. copyright
                            and international treaties. They may not be copied, reproduced, republished, uploaded,
                            posted, transmitted or distributed in any way. You may, however, download one copy
                            of the materials for your personal, non-commercial use only provided that you keep
                            intact all copyright and other proprietary notices. Reuse of any of the online content
                            and graphics for any purpose is strictly prohibited unless you receive the express
                            permission of DecisionPower, Inc. DecisionPower, Inc. reserves all rights. AdPlanit,
                            "Advertise with Confidence”, “Where Advertising Starts” the AdPlanit logo and corporate
                            identity, including the "look and feel" of the AdPlanit Web site, are trademarks
                            of DecisionPower, Inc.
                        </div>
                    </td>
                    <td id="RHCol" runat="server" rowspan="3" style="z-index: 1000">
                        <div id="TCRHDiv">
                        </div>
                    </td>
                </tr>
           <tr>
               <td ID="FooterCell" align="center" ><table id="FooterTable" align="center"><tr><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table></td>
               </td>
           </tr>
           
       </table>


       </div>      

    </div>
    </form>
</body>
</html>
