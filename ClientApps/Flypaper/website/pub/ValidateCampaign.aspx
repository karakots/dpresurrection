<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValidateCampaign.aspx.cs" Inherits="ValidateCampaign" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Campaign Entry Error</title>
    <link rel="stylesheet" type="text/css" href="~/styles/ErrorPage.css" />     
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDIv">
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      <asp:Label runat="server" ID="PhoneLabel" Text="Where Advertising Starts"></asp:Label>  
      </asp:Panel>
        <asp:LoginView ID="LoginView" runat="server">
           <AnonymousTemplate>
                 <div id="SignInDiv" runat="server">
                 Create a <a href="AddUser.aspx">free account</a><br />
                 Returning user? <a href="Login.aspx">Sign In</a>
                 </div>
           </AnonymousTemplate>
           <LoggedInTemplate>
                 <div id="UserNameDiv" runat="server">
                   Logged in as: <asp:LoginName ID="LoginName1" runat="server" /><br />
                   Not you? <a href="Login.aspx">Change user</a>
                 </div>
           </LoggedInTemplate>
       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="SupportInfo.aspx" id="A2" class="nav_support" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="AboutAdPlanIt.aspx" id="A1" class="nav_about" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home"></a></li>
        </ul>
              <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>
     
        <asp:Label ID="SubTitle" runat="server" Text="Step 1 – Media Campaign"/>
        
        <asp:Label ID="ErrorTitle" runat="server" Text="ERROR: One or more entries need to be corrected:"/>
        
        <div id="ErrorDetailsDiv">
            <asp:Label ID="ErrorDetails" runat="server" Text="Unspecifried error."/><br /><br />
            <asp:Button ID="OkButton" runat="server" Text="OK" OnClick="OkButton_Click" /><br /><br />
            
                 <div id="Footer" style="display:inline""><%= WebLibrary.Utils.Footer %></div>
                             
        </div>
        
        </div>
    </form>
</body>
</html>
