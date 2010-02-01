<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CampaignRegion.aspx.cs" Inherits="CampaignRegion" %>

<%@ Register src="~/AgentView/GoogleMap.ascx" tagname="GoogleMap" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Set Campaign Region</title>
    <link rel="stylesheet" type="text/css" href="~/css/CampaignDetails.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
    <script type="text/javascript" src="DemoPopup.js"></script>
    <style type="text/css">
        .style1
        {
            width: 66px;
        }
    </style>
    
  <!--<uc1:GoogleMap ID="GoogleMap1" runat="server" />-->
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
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
        <div id="ContentDiv" runat="server" visible="true">
            <table id="ContentTable" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td id="InfoDiv" runat="server">
                        <div id="SubNavBar">
                        </div>
                        <h1>
                            Set Campaign Region</h1>
                    </td>
                    <td id="RHCol" runat="server" rowspan="3" style="z-index: 1000">
                        <div id="RHDiv" style="text-align: center; height: 231px;">
                            <asp:Button ID="ImageButton1" runat="server" BackColor="#FF9900" Font-Bold="True"
                                ForeColor="White" BorderStyle="Outset" Text="OK" OnClick="AddRegion_Click" 
                                Width="100px" style="position:relative; top:30px;"/>
                                
                            <asp:Button ID="ImageButton2" runat="server" BackColor="#FF9900" Font-Bold="True"
                                ForeColor="White" BorderStyle="Outset" Text="Cancel" 
                                PostBackUrl="~/Campaign.aspx" Width="100px" style="position:relative; top:60px;"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="PageDataCell" runat="server">
                        &nbsp;
                        <table cellpadding="2" width="100%">
                            <tr id="Tr1">
                                <td width="60" class="PopSubTitle">
                                    State:
                                </td>
                                <td>
                                    <asp:DropDownList ID="RegionsList" runat="server" CssClass="RegionsList" Width="120"
                                        OnSelectedIndexChanged="RegionsList_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="SubregionLabel" runat="server" Text="&nbsp;&nbsp;Region:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="rp" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="RegionsList" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <img id="SetStateProgressImage" name="SetStateProgressImage" runat="server" width="16"
                                                height="16" src="images/progress16.gif" style="visibility: hidden; position: absolute;
                                                left: 0x; margin-top: 5px;" />
                                            <%-- To enable the Region (level-3) dropdown list, set AutoPostBack to true for SubregionsList --%>
                                            <asp:DropDownList ID="SubregionsList" runat="server" CssClass="SubregionsList" OnSelectedIndexChanged="SubregionsList_SelectedIndexChanged"
                                                AutoPostBack="True" Enabled="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                             <%-- <tr> <td colspan="4" >
                              <div style="margin-left:50px">
                                <uc1:GoogleMap ID="GoogleMap2" runat="server" GStyle="width:500px;height:250px;border-style:double; border-width:medium;"/>
                                </div>
                            </td>
                            </tr>--%>
                             <tr>
                            <td colspan="4">
                                <asp:CheckBox ID="UseCounties" style="float:left" runat="server" Text="Select Specific Counties" 
                                    oncheckedchanged="UseCounties_CheckedChanged" AutoPostBack="true" 
                                    Font-Bold="True"/>       
                            </td>
                            </tr>
                            <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="srp" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="SubregionsList" />
                                        <asp:AsyncPostBackTrigger ControlID="RegionsList"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="RegionsList"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="UseCounties"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:CheckBoxList ID="CountyList" runat="server" Height="75px" Width="702px" 
                                            RepeatDirection="Horizontal" RepeatColumns="5" Enabled="False">
                                        </asp:CheckBoxList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                            </tr>
                          
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding-bottom: 30px">
                      
                    </td>
                </tr>
                <tr>
                    <td id="FooterCell">
                        <table id="FooterTable" align="center">
                            <tr>
                                <td id="Footer">
                                    <%= WebLibrary.Utils.Footer %>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>


       </div>      

    </div>
    </form>
</body>
</html>
