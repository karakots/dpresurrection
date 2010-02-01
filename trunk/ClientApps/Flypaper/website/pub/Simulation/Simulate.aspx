<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Simulate.aspx.cs" Inherits="Simulation_Simulate" %>

    <%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<%@ Register src="../AgentView/GoogleMap.ascx" tagname="GoogleMap" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Running a Simulation</title>
    <link rel="stylesheet" type="text/css" href="../css/HQ.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" />
    <div id="MainDIv">
        <%-- Start of standard header --%>
        <asp:Panel ID="NavBar" CssClass="NavBar" runat="server">
        </asp:Panel>
        <asp:Image runat="server" ID="BetaMarker" ImageUrl="../images/Beta-Green.gif" />
        <div style="height: 29px;">
            &nbsp;</div>
        <%-- <ul class="nav">
            <li></li>
            <li><a href="../Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;">
            </a></li>
            <li><a href="../Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;">
            </a></li>
            <li><a href="../Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;">
            </a></li>
        </ul>--%>
        <img class="Logo" src="../images/Logo_v2-2.gif" />
        <%-- End of standard header --%>
        <div id="ContentDiv" runat="server" visible="true">
            <table id="XContentTable" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div id="SubTabDiv" runat="server" visible="true">
                            <div style="height: 21px;">
                                <asp:Label ID="WarningLabel" runat="server" Text="Please do not close window while simulation is running."
                                    Font-Bold="True" Font-Size="Large"></asp:Label>
                            </div>
                            <%--  <ul class="nav">
                                <li><a href="../HQ.aspx" id="A3" class="nav_shop" onclick="ShowMainWaitCursor(); return true;">
                                </a></li>
                                <li><a href="../Order.aspx" id="A4" class="nav_order" onclick="ShowMainWaitCursor(); return true;">
                                </a></li>
                                <li><a href="#" id="A5" class="nav_analyze active" onclick="ShowMainWaitCursor(); return true;">
                                </a></li>
                            </ul>--%>
                        </div>
                        <table id="InnerTable">
                        </table>
                        <br />
                    </td>
                    <td id="HqRHCol" runat="server" rowspan="4">
                        <div id="Div2">
                            <br />
                            <br />
                            <div runat="server" id="RHInfoDiv" style="text-align:center">
                                <asp:Button ID="ExitButton" runat="server"   Text="Exit Simulation" BackColor="#FF9900"
                                    Font-Bold="True" ForeColor="White" OnClick="StopSim_Click" 
                                    BorderStyle="Outset" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="SimInfo">
                        <table>
                            <tr>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Timer ID="Redraw" runat="server" Interval="1000" OnTick="Redraw_Tick" Enabled="False">
                                        </asp:Timer>
                                        <div>
                                            <br />
                                            <%-- <asp:TextBox ID="StatusBox" runat="server" TextMode="MultiLine" Width = "400px" Height="400px" BackColor="#CCFFFF" BorderColor="#003300" BorderStyle="Groove" BorderWidth="1" ForeColor="#003300" ReadOnly="True"></asp:TextBox>
                                              --%>
                                            <h1>
                                                <asp:Label runat="server" ID="StatusBox" Text="Setting Up Simulation"></asp:Label>
                                            </h1>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <%--  <ve:Map ID="AgentMap" runat="server" Height="480px" Width="640px" ZoomLevel="4" NavigationControl3D="False" Dashboard="False" />--%>
                                <%--<asp:Image ID="SimDraw" ImageUrl="SimDraw.aspx?v=0" runat="server" 
                                        Height="480" Width="640" />--%>
                                <uc1:GoogleMap ID="GoogleMap1" runat="server" GStyle="width:640px;height:480px" />
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="ExitButton"
        OnClientCancel="cancelClick" DisplayModalPopupID="ModalPopupExtender1" />
    <br />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ExitButton"
        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 200px; background-color: InfoBackground;
        border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
        This will stop the simulation. Continue?
        <br />
        <br />
        <div style="text-align: right;">
            <asp:Button ID="ButtonOk" runat="server" Text="Yes" BackColor="#FF9900" Font-Bold="True"
                ForeColor="White" BorderColor="#FF9900" BorderStyle="Outset" />
            <asp:Button ID="ButtonCancel" runat="server" Text="No" BackColor="#FF9900" Font-Bold="True"
                ForeColor="White" BorderColor="#FF9900" BorderStyle="Outset" />
        </div>
    </asp:Panel>
    </form>
</body>
</html>
