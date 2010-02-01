<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FAQ.aspx.cs" Inherits="FAQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Frequently Asked Questions</title>
    <link rel="stylesheet" type="text/css" href="~/css/Common.css" />     
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
            <li><a href="#" id="A2" class="nav_help active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
      <div ID="NewPlanDiv" runat="server" visible="false">
            You have successfuly created the Campaign named: ?<br /><br />
            Now what do you want to do?<br /><br />
      </div>
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="InfoDiv" runat="server" style="height:40px;" >
                <div ID="SubNavBar" ></div>
              </td>
               <td id="RHCol" runat="server" rowspan="3" >
               <div id="RHDiv">                
               <div ID="RHDiv1a" >Video Guides<br /><br />
                <a href="#"  onclick="OpenDemoWindow();">How to Plan<br />Using AdPlanit</a><br /><br />
                </div>
                <div ID="RHDiv2">What's New:<br /><br />bla bla bla...</div>
                </div>
               </td>
           </tr>
           <tr>
               <td ID="FAQCell" runat="server"  style="position:relative; top:0px; width:1015px; vertical-align:top; height:460px;" >&nbsp; </td>
           </tr>
           <tr>
               <td ID="FooterCell" align="center" ><table id="FooterTable" align="center"><tr><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table></td>
               </td>
           </tr>
           
       </table>


      <div ID="SubTabDiv" runat="server" visible="true">
      <ul class="nav">
            <li><a href="Glossary.aspx" id="A3" class="nav_advertising" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="#" id="A4" class="nav_faq active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Support.aspx" id="A5" class="nav_support" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
        </div>

       </div>      

    </div>
    </form>
</body>
</html>
