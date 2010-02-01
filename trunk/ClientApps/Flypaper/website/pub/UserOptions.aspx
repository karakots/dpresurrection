<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserOptions.aspx.cs" Inherits="UserOptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Options</title>
    <link rel="stylesheet" type="text/css" href="~/styles/InfoPages.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="ReturnPage" name="ReturnPage" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
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
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>
       
       <asp:Label ID="SubTitle" runat="server" Text="User Option Settings"/>
       
       <div id="SetEngDiv" >
       <asp:CheckBox ID="StartInEngineeringModeBox" runat="server" Text="Automatically set Engineering Mode ON when logging in." AutoPostBack="true" OnCheckedChanged="AutoEng_Changed" />
       </div>
       
       <asp:Label ID="BodyInfo" Text="" runat="server"></asp:Label>   
          
       <br /><br />
       <%--  <asp:Button runat="server" ID="OkButton" Text="OK" OnClick="OkButton_Click" /> --%>
       <asp:Button runat="server" ID="ReturnButton" Text="Return to Previous Page " OnClick="CancelButton_Click" /> 

       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>

   </div>
    </form>
</body>
</html>
