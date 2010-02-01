<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="admin_Home" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanIt User Administration</title>
    <style type="text/css">
        .style1
        {
            width: 292px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <asp:SqlDataSource ID="UserDb" runat="server" ConnectionString="<%$ ConnectionStrings:loginConnectionString %>"
        ProviderName="<%$ ConnectionStrings:loginConnectionString.ProviderName %>" 
        SelectCommand="SELECT [UserName], DATEDIFF(mi, [LastActivityDate], GetUTCDate()) as TimeSinceLog FROM [vw_aspnet_Users] ORDER BY [TimeSinceLog]">
    </asp:SqlDataSource>
      <asp:SqlDataSource ID="LoginStats" runat="server" ConnectionString="<%$ ConnectionStrings:loginConnectionString %>"
        ProviderName="<%$ ConnectionStrings:loginConnectionString.ProviderName %>" 
        SelectCommand="SELECT [UserName], [lastActivityDate], [CreateDate]  FROM [vw_aspnet_Users], [aspnet_Membership] WHERE aspnet_Membership.userid = vw_aspnet_Users.userid ORDER BY [lastActivityDate]">
    </asp:SqlDataSource>
    <table border="1" style="width: 99%; height: 332px;">
        <tr>
            <td class="style1" valign="top">
                <div>
                    <asp:GridView ID="UserView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BorderStyle="Double" DataSourceID="UserDb" AllowPaging="True" 
                        Caption="Current users" CaptionAlign="Top" 
                        onselectedindexchanged="UserView_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" ReadOnly="true"/>
                            <asp:BoundField DataField="TimeSinceLog" HeaderText="Minutes" SortExpression="TimeSinceLog"
                                ReadOnly="True" />
                            <asp:CommandField ShowSelectButton="True" ButtonType="Button" />
                        </Columns>
                        <SelectedRowStyle BackColor="#6699FF" Font-Bold="True" />
                    </asp:GridView>
                </div>
            </td>
            <td valign="top">
                <div align="left" style="height: 60px" title="User operations">
                    <table style="width: 100%; margin-bottom: 50px; height: 60px;">
                     <tr>
                            <th>
                               Permissions
                            </th>
                            <td>
                                <asp:DropDownList ID="RoleList" runat="server" Height="25px" Width="128px">
                                </asp:DropDownList>
                            </td>
                            <td> 
                                <asp:Button ID="ChangeType" runat="server" Text="ChangeType" 
                                    onclick="ChangeType_Click" BackColor="#FF9900" ForeColor="White" />
                             </td>
                        </tr>
                        <tr>
                            <th>
                                User Name
                            </th>
                            <td>
                                <asp:TextBox ID="UserNameBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                          <tr>
                            <th>
                                First Name
                            </th>
                            <td>
                                <asp:TextBox ID="FirstNameBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> &nbsp </td>
                        </tr>
                         <tr>
                            <th>
                                  Last Name
                            </th>
                            <td>
                                <asp:TextBox ID="LastNameBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> <asp:Button ID="editNameButton" runat="server" Text="Edit Name" 
                                    onclick="editNameButton_Click" /> </td>
                        </tr>
                         <tr>
                            <th>
                            Company Name
                            </th>
                            <td>
                                <asp:TextBox ID="CompanyNameBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> &nbsp </td>
                        </tr>
                         <tr>
                            <th>
                            Company URL
                            </th>
                            <td>
                                <asp:TextBox ID="CompanyURLBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> &nbsp </td>
                        </tr>
                         <tr>
                            <th>
                               Phone
                            </th>
                            <td>
                                <asp:TextBox ID="PhoneBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> &nbsp </td>
                        </tr>
                         <tr>
                            <th>
                                Referral info
                            </th>
                            <td>
                                <asp:TextBox ID="ReferralinfoBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> 
                                <asp:Button ID="EditInfoButton" runat="server" Text="Edit Info" 
                                    onclick="EditInfoButton_Click" /> </td>
                        </tr>
                         <tr>
                            <th>
                               Password
                            </th>
                            <td>
                                <asp:TextBox ID="PasswordBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> 
                                <asp:Button ID="EditPasswordButton" runat="server" Text="Change Password" 
                                    onclick="EditPasswordButton_Click" /> </td>
                        </tr>
                        <tr>
                            <th>
                               Sign up date
                            </th>
                            <td>
                                <asp:TextBox ID="SignUpBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td> &nbsp</td>
                        
                        <tr>
                            <th>
                            <div align="right">
                                <asp:Button ID="DeleteUserButton" runat="server" OnClick="DeleteUserButton_Click"
                                    Text="Delete User" ToolTip="Deletes this User" Width="142px" />
                                    </div>
                            </th>
                            <th>
                                <div align="left">
                                    <asp:Button ID="SubmitUserChangesButton" runat="server" OnClick="SubmitUserChangesButton_Click"
                                        Text="Update User Info" ToolTip="Updates this User" Width="142px" 
                                        Enabled="False" />
                                </div>
                            </th>
                            <td> &nbsp 
                                <asp:Button ID="UserLogButton" runat="server" onclick="UserLogButton_Click" 
                                    Text=" View User Log" />
                            </td>
                        </tr>
                        </tr>
                        
                        <tr>
                        <th>
                         <div align="right">
                         <ajaxtoolkit:confirmbuttonextender 
                                id="ConfirmButtonExtender2" 
                                runat="server" 
                                targetcontrolid="DeleteUserButton"
                                onclientcancel="cancelClick" 
                                displaymodalpopupid="ModalPopupExtender1" />
                                <br />
                                <ajaxtoolkit:modalpopupextender id="ModalPopupExtender1" runat="server" 
                                targetcontrolid="DeleteUserButton"
                                    popupcontrolid="PNL" 
                                    okcontrolid="ButtonOk" 
                                    cancelcontrolid="ButtonCancel" 
                                    backgroundcssclass="modalBackground" />
                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 200px; background-color: White;
                                    border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                    Are you sure you want to delete this user?
                                    <br />
                                    <br />
                                    <div style="text-align: right;">
                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                                    </div>
                                </asp:Panel>
                                </div>
                                </th>
                                <td> &nbsp </td> <td> &nbsp </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
          <tr>
        <td colspan="2">
        <a href="http://adplanit.com/plesk-stat/webstat/" style="border-style: double; border-color: #FF0000; font-size: x-large; "> View Web Stats </a>
        </td>
        </tr>
        <tr>
        <td colspan="2">
            <div>
                <asp:GridView ID="StatsView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    BorderStyle="Solid" DataSourceID="LoginStats" AllowPaging="False" Caption="Login Info"
                    CaptionAlign="Top">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName"
                            ReadOnly="true" />
                        <asp:BoundField DataField="LastActivityDate" HeaderText="Last Login Date" SortExpression="LastActivityDate" ReadOnly="True" />
                             <asp:BoundField DataField="CreateDate" HeaderText="Account Creation Date" SortExpression="CreateDate" ReadOnly="True" />
                    </Columns>
                    <SelectedRowStyle BackColor="#6699FF" Font-Bold="True" />
                </asp:GridView>
            </div>
            </td>
        </tr>
    </table>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
