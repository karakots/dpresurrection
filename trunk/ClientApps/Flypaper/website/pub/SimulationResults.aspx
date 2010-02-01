<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SimulationResults.aspx.cs" Inherits="SimulationResults" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simulation Results</title>
    <link rel="stylesheet" type="text/css" href="~/styles/SimulationResults.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body onload="ScrollTranscriptToBottom()">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"  /> 
    <div id="MainDIv" >
      <%-- Start of standard header --%>
       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
        <div ID="NavBarLinks">
      </div>  
      <asp:Label runat="server" ID="PhoneLabel" Text="Call (800) 518-9202"></asp:Label>  
      <asp:Label runat="server" ID="HelpEmailLink" Text="send us a message"></asp:Label>  
      <asp:Image runat="server" ID="BreadcrumbLabel"  ImageUrl="images/Breadcrumbs4.gif"></asp:Image>  
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
            <li><a href="HQ.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>    
        <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>
       <asp:Label ID="SubTitle" Text="Results" runat="server"></asp:Label>

        <%-- This DIV gets replaced with the campaign summary --%>
        <div id="CampaignSummaryDiv" runat="server"></div>
        
        <%-- This DIV gets replaced with the results table --%>
        <div id="ResultTableDiv" runat="server"></div>
        
        <asp:UpdatePanel runat="server" ID="StoryContainer" UpdateMode="Conditional" RenderMode="Block">
        <ContentTemplate>
        <%-- This DIV gets replaced with the suggestions and other "story" elements --%>
        <div id="StoryDiv" runat="server"></div>
        <div id="StoryBodyDiv" runat="server"></div>
        <div id="StoryFooterDiv" runat="server"></div>
       </ContentTemplate></asp:UpdatePanel>
        
        
       <asp:ImageButton ID="ResultsNav1" runat="server" ImageUrl="images/SummaryTabActive.gif" Enabled="false" />
       <asp:ImageButton ID="ResultsNav2" runat="server" ImageUrl="images/ReportTab.gif" PostBackUrl="~/Report.aspx" />
       <asp:ImageButton ID="ResultsNav3" runat="server" ImageUrl="images/ShoppingListTab.gif" PostBackUrl="~/ShoppingList.aspx" />
       </div>
       
       <div id="DebugData" runat="server" Visible="false"><%= DebugDataText()%></div> 

        <div ID="DebugListDiv" runat="server" visible="false">
          Active Sims: <%=this.DisplayPlanCount %> <%=this.DisplayPlanIDListString() %> <br />
        </div>
    </form>
</body>
</html>
