<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit User Profile</title>
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
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="A2" class="nav_help active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="InfoDiv" runat="server" >
                <div ID="SubNavBar" ></div>
                <h1>My User Profile</h1>
                
		   <div class="formElement"><strong>First Name:</strong><br><input type="text" name="first_name" id="first_name" runat="server" /></div>
		   <div class="formElement"><strong>Last Name:</strong><br><input type="text" name="last_name" id="last_name" runat="server" /></div>
		   <div class="formElement"><strong>Company:</strong><br><input type="text" name="company" id="company" runat="server" /></div>
		   <div class="formElement"><strong>Phone:</strong><br><input type="text" name="phone" id="phone" runat="server" /></div>
		   <div class="formElement"><strong>Web Site:</strong><br><input type="text" name="URL" id="URL" runat="server" /></div><br /><br />
		   <div class="formButton" ><asp:ImageButton runat="server" src="images/Save.gif" alt="Submit" border="0" class="submit" id="submit" OnClientClick="SaveProfile();" ></asp:ImageButton></div>

              </td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="RHDiv"></div>
               </td>
           </tr>
           <tr>
               <td ID="PageDataCell" runat="server">&nbsp; </td>
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
