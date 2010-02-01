<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentDisplayGoogle.aspx.cs" Inherits="AgentView_AgentDisplayGoogle" %>

<%@ Register src="GoogleMap.ascx" tagname="GoogleMap" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Display</title>
    
    <link rel="stylesheet" type="text/css" href="~/css/Common.css" />

 
   
    <script type="text/javascript" src="../AdPlanIt.js"></script>
    
    <meta http-equiv="content-type" content="text/html; charset=utf-8"/>
   
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="MainDIv">
        <%-- Start of standard header --%>
        <asp:Panel ID="NavBar" CssClass="NavBar" runat="server">
        </asp:Panel>
        <asp:Image runat="server" ID="BetaMarker" ImageUrl="../images/Beta-Green.gif" />
        <div style="height: 29px;">
            &nbsp;</div>
        <ul class="nav">
           <%-- <li></li>
            <li><a href="#" id="SupportTab" class="nav_help" runat="server"></a></li>
            <li><a href="#" id="CampaignsTab" class="nav_mycampaigns active" runat="server"></a>
            </li>
            <li><a href="#" id="HomeTab" class="nav_home" runat="server"></a></li>--%>
        </ul>
        <img class="Logo" src="../images/Logo_v2-2.gif"/>
    </div>
    <%-- End of standard header --%>
    <div id="ContentDiv" style="position: relative;left:20px;z-index:0" runat="server">
    <table>
    <tr>
    <td align="right">
    <asp:Image ID="SimDraw" ImageUrl="AgentIcon.aspx?size=10&color=0&active=false" runat="server" 
                                        Height="10px" Width="10px" /> = Unaware
    </td>
    <td align="right">
    <asp:Image ID="Image1" ImageUrl="AgentIcon.aspx?size=10&color=0&active=true" runat="server" 
                                        Height="10px" Width="10px" /> = Low Perusasion
    </td>
     <td align="right">
    <asp:Image ID="Image2" ImageUrl="AgentIcon.aspx?size=10&color=255&active=true" runat="server" 
                                        Height="10px" Width="10px" /> = High Perusasion
    </td>
    <td align="center"> <h4>Size indicates probability of choice</h4></td>
    </tr>
    <tr>
    <td colspan="4">
    <uc1:GoogleMap ID="GoogleMap1" runat="server" GStyle="width:900px;height:600px" />
        </td>
        </tr>
        </table>
    </div>
   
    </form>
</body>
</html>
