<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetPassword.aspx.cs" Inherits="SetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AdPlanit - Change Password</title>
    <link rel="stylesheet" type="text/css" href="~/styles/SetPassword.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDIv" >
      <%-- Start of standard header --%>
       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
       <asp:Label runat="server" ID="PhoneLabel" Text="Where Advertising Starts"></asp:Label>  
      </asp:Panel>
        <asp:LoginView ID="LoginView" runat="server">
           <AnonymousTemplate>
                 <div id="SignInDiv" runat="server">
                 Create a <a href="AddUser.aspx">free account</a><br /><br />
             
                 </div>
           </AnonymousTemplate>
           <LoggedInTemplate>
                 <div id="UserNameDiv" runat="server">
                   Logged in as: <asp:LoginName ID="LoginName1" runat="server" /><br />&nbsp;
                 </div>
           </LoggedInTemplate>
       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="SupportInfo.aspx" id="A2" class="nav_support" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
       <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>

        <%-- <asp:Image runat="server" ID="Ad1" CssClass="Ad1" ImageUrl="images/336x280ad.gif" />     --%>
             
        <asp:Panel ID="LoginPanel" runat="server" CssClass="LoginPanel" >
         <asp:ChangePassword ID="ChangePassword1" runat="server" SuccessPageUrl="~/Home.aspx">
            <ChangePasswordTemplate>
                <table border="0" cellpadding="1" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2"  style="font-size:11pt;font-weight:600;padding:3px">
                                        Change Your Password</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                            AssociatedControlID="CurrentPassword">Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                            ControlToValidate="CurrentPassword" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="NewPasswordLabel" runat="server" 
                                            AssociatedControlID="NewPassword">New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                            ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                            ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                            AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                            ControlToValidate="ConfirmNewPassword" 
                                            ErrorMessage="Confirm New Password is required." 
                                            ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                            ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                            Display="Dynamic" 
                                            ErrorMessage="The Confirm New Password must match the New Password entry." 
                                            ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="ChangePasswordPushButton" runat="server" 
                                            CommandName="ChangePassword" Text="Change Password" 
                                            ValidationGroup="ChangePassword1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ChangePasswordTemplate>
        </asp:ChangePassword>
        </asp:Panel>
         
        <br />
        <br />

         
       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>
               
    </form>
</body>
</html>
