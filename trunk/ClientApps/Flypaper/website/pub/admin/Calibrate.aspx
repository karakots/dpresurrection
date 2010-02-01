<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calibrate.aspx.cs" Inherits="Simulation_Calibrate" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ad Option Calibration</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="MediaLibrary.SimpleOption"
        SelectMethod="Select" TypeName="MediaLibrary.SimpleAdOptionDb" UpdateMethod="Update"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        DataObjectTypeName="MediaLibrary.OnlineOption" SelectMethod="Select" 
        TypeName="MediaLibrary.OnlineAdOptionDb" UpdateMethod="Update"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" 
        DataObjectTypeName="MediaLibrary.SearchOption" SelectMethod="Select" 
        TypeName="MediaLibrary.SearchAdOptionDb" UpdateMethod="Update"></asp:ObjectDataSource>
    <table style="width: 100%;">
    <tr> <td>
            <a href ="../Analysis.aspx"> Switch to Analysis View</a> 
            <a href ="../AgentView/AgentDisplay.aspx"> Agent Microscope</a> 
            </td>
            </tr>
        <tr>
            <td colspan="5">
                <asp:TextBox ID="PlanData" runat="server" TextMode="MultiLine" ReadOnly="True" Height="100px"
                    Width="1155px" BorderStyle="Solid"></asp:TextBox>
                       
                     <div align="right">
                         <ajaxtoolkit:confirmbuttonextender 
                                id="ConfirmButtonExtender2" 
                                runat="server" 
                                targetcontrolid="UpdateDb"
                                onclientcancel="cancelClick" 
                                displaymodalpopupid="ModalPopupExtender1" />
                                <br />
                                <ajaxtoolkit:modalpopupextender id="ModalPopupExtender1" runat="server" 
                                targetcontrolid="UpdateDb"
                                    popupcontrolid="PNL" 
                                    okcontrolid="ButtonOk" 
                                    cancelcontrolid="ButtonCancel" 
                                    backgroundcssclass="modalBackground" />
                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 200px; background-color: White;
                                    border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                    This will make these changes permenant for all users...
                                    <br />
                                    <br />
                                    <div style="text-align: right;">
                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                                    </div>
                                </asp:Panel>
                                </div>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate> 
                  <asp:Timer ID="ProgressTimer" runat="server" Enabled="False" 
                        ontick="ProgressTimer_Tick" Interval="500">
                    </asp:Timer>
                <asp:TextBox ID="ProgressBox" runat="server" ReadOnly="True" Height="20px"
                    Width="1155px" BorderStyle="None"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="RunCalibration" runat="server" Text="Run Calibration" 
                    onclick="RunCalibration_Click" BackColor="#33CCFF" BorderStyle="Ridge" />
            </td>
            <td>
                <asp:Button ID="UndoDb" runat="server" Text="Undo Changes" OnClick="UndoDb_Click"
                    BackColor="#33CCFF" BorderStyle="Outset" />
            </td>
            <td>
                <asp:Button ID="UpdateDb" runat="server" Text="Submit Data Changes" OnClick="UpdateDb_Click"
                    BackColor="#33CCFF" BorderStyle="Solid" />
            </td>
            <td>
                <asp:Button ID="DownloadBut" runat="server" Text="Download options.dat" OnClick="DownloadBut_Click"
                    BackColor="#33CCFF" BorderStyle="Outset" />
            </td>
        </tr>
    </table>
    <p/><p/>
   
  
    &nbsp;<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
    <asp:RadioButton ID="SimpleButton" runat="server" 
        oncheckedchanged="SimpleButton_CheckedChanged" Text="Simple Options" 
        GroupName="OptionSelect" 
        AutoPostback="true" Checked="True"/>
    <asp:RadioButton ID="OnlineButton" runat="server" Text="Online Optoins" 
        GroupName="OptionSelect" oncheckedchanged="SimpleButton_CheckedChanged" 
        AutoPostback="true"/>
    <asp:RadioButton ID="SearchButton" runat="server" Text="Search Options" 
        GroupName="OptionSelect" oncheckedchanged="SimpleButton_CheckedChanged" 
        AutoPostback="true"/>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="SimOptionView" runat="server">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                AutoGenerateEditButton="True" BackColor="White" BorderColor="#E7E7FF" 
                BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                DataSourceID="ObjectDataSource1" GridLines="Horizontal">
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <Columns>
                    <asp:BoundField DataField="MediaType" HeaderText="MediaType" 
                        SortExpression="MediaType" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Cost_Modifier" HeaderText="Cost_Modifier" 
                        SortExpression="Cost_Modifier" />
                    <asp:BoundField DataField="Prob_Per_Hour" HeaderText="Prob_Per_Hour" 
                        SortExpression="Prob_Per_Hour" />
                    <asp:BoundField DataField="Persuasion" HeaderText="Persuasion" 
                        SortExpression="Persuasion" />
                    <asp:BoundField DataField="Awareness" HeaderText="Awareness" 
                        SortExpression="Awareness" />
                    <asp:BoundField DataField="Recency" HeaderText="Recency" 
                        SortExpression="Recency" />
                    <asp:BoundField DataField="ConsiderationProbScalar" 
                        HeaderText="ConsiderationProbScalar" SortExpression="ConsiderationProbScalar" />
                    <asp:BoundField DataField="ConsiderationPersuasionScalar" 
                        HeaderText="ConsiderationPersuasionScalar" 
                        SortExpression="ConsiderationPersuasionScalar" />
                    <asp:BoundField DataField="ConsiderationAwarenessScalar" 
                        HeaderText="ConsiderationAwarenessScalar" 
                        SortExpression="ConsiderationAwarenessScalar" />
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" />
                </Columns>
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                <AlternatingRowStyle BackColor="#F7F7F7" />
            </asp:GridView>
        </asp:View>
        <asp:View ID="OnLineView" runat="server">
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                DataSourceID="ObjectDataSource2" BackColor="White" BorderColor="#E7E7FF" 
                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" />
                    <asp:BoundField DataField="MediaType" HeaderText="MediaType" 
                        SortExpression="MediaType" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Persuasion" HeaderText="Persuasion" 
                        SortExpression="Persuasion" />
                    <asp:BoundField DataField="Awareness" HeaderText="Awareness" 
                        SortExpression="Awareness" />
                    <asp:BoundField DataField="Prob_Per_Hour" HeaderText="Prob_Per_Hour" 
                        SortExpression="Prob_Per_Hour" />
                    <asp:BoundField DataField="Num_Impressions" HeaderText="Num_Impressions" 
                        SortExpression="Num_Impressions" />
                    <asp:BoundField DataField="Click_Awaneness" HeaderText="Click_Awaneness" 
                        SortExpression="Click_Awaneness" />
                    <asp:BoundField DataField="Click_Persuasion" HeaderText="Click_Persuasion" 
                        SortExpression="Click_Persuasion" />
                    <asp:BoundField DataField="Action_Awareness" HeaderText="Action_Awareness" 
                        SortExpression="Action_Awareness" />
                    <asp:BoundField DataField="Action_Persuasion" HeaderText="Action_Persuasion" 
                        SortExpression="Action_Persuasion" />
                    <asp:BoundField DataField="Chance_Click" HeaderText="Chance_Click" 
                        SortExpression="Chance_Click" />
                    <asp:BoundField DataField="Chance_Action" HeaderText="Chance_Action" 
                        SortExpression="Chance_Action" />
                    <asp:CheckBoxField DataField="IsClick" HeaderText="IsClick" 
                        SortExpression="IsClick" />
                    <asp:BoundField DataField="Cost_Modifier" HeaderText="Cost_Modifier" 
                        SortExpression="Cost_Modifier" />
                    <asp:BoundField DataField="Recency" HeaderText="Recency" 
                        SortExpression="Recency" />
                    <asp:BoundField DataField="ConsiderationProbScalar" 
                        HeaderText="ConsiderationProbScalar" SortExpression="ConsiderationProbScalar" />
                    <asp:BoundField DataField="ConsiderationPersuasionScalar" 
                        HeaderText="ConsiderationPersuasionScalar" 
                        SortExpression="ConsiderationPersuasionScalar" />
                    <asp:BoundField DataField="ConsiderationAwarenessScalar" 
                        HeaderText="ConsiderationAwarenessScalar" 
                        SortExpression="ConsiderationAwarenessScalar" />
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" />
                </Columns>
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                <AlternatingRowStyle BackColor="#F7F7F7" />
            </asp:GridView>
        </asp:View>
        <asp:View ID="SearchOptionView" runat="server">
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                CellPadding="3" DataSourceID="ObjectDataSource3" GridLines="Horizontal">
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <Columns>
                    <asp:CommandField ButtonType="Button" ShowEditButton="True" />
                    <asp:BoundField DataField="MediaType" HeaderText="MediaType" 
                        SortExpression="MediaType" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Num_Impressions" HeaderText="Num_Impressions" 
                        SortExpression="Num_Impressions" />
                    <asp:BoundField DataField="Click_Awaneness" HeaderText="Click_Awaneness" 
                        SortExpression="Click_Awaneness" />
                    <asp:BoundField DataField="Click_Persuasion" HeaderText="Click_Persuasion" 
                        SortExpression="Click_Persuasion" />
                    <asp:BoundField DataField="Action_Awareness" HeaderText="Action_Awareness" 
                        SortExpression="Action_Awareness" />
                    <asp:BoundField DataField="Action_Persuasion" HeaderText="Action_Persuasion" 
                        SortExpression="Action_Persuasion" />
                    <asp:BoundField DataField="Chance_Click" HeaderText="Chance_Click" 
                        SortExpression="Chance_Click" />
                    <asp:BoundField DataField="Chance_Action" HeaderText="Chance_Action" 
                        SortExpression="Chance_Action" />
                    <asp:CheckBoxField DataField="IsClick" HeaderText="IsClick" 
                        SortExpression="IsClick" />
                    <asp:BoundField DataField="Cost_Modifier" HeaderText="Cost_Modifier" 
                        SortExpression="Cost_Modifier" />
                    <asp:BoundField DataField="Prob_Per_Hour" HeaderText="Prob_Per_Hour" 
                        SortExpression="Prob_Per_Hour" />
                    <asp:BoundField DataField="Persuasion" HeaderText="Persuasion" 
                        SortExpression="Persuasion" />
                    <asp:BoundField DataField="Awareness" HeaderText="Awareness" 
                        SortExpression="Awareness" />
                    <asp:BoundField DataField="Recency" HeaderText="Recency" 
                        SortExpression="Recency" />
                    <asp:BoundField DataField="ConsiderationProbScalar" 
                        HeaderText="ConsiderationProbScalar" SortExpression="ConsiderationProbScalar" />
                    <asp:BoundField DataField="ConsiderationPersuasionScalar" 
                        HeaderText="ConsiderationPersuasionScalar" 
                        SortExpression="ConsiderationPersuasionScalar" />
                    <asp:BoundField DataField="ConsiderationAwarenessScalar" 
                        HeaderText="ConsiderationAwarenessScalar" 
                        SortExpression="ConsiderationAwarenessScalar" />
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" />
                </Columns>
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                <AlternatingRowStyle BackColor="#F7F7F7" />
            </asp:GridView>
        </asp:View>
    </asp:MultiView>
    </ContentTemplate>
      </asp:UpdatePanel>
    </form>
</body>
</html>
