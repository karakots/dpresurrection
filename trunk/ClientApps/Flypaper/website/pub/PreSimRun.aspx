<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSimRun.aspx.cs" Inherits="PreSimRun" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Preparing to Run Media Simulation</title>
    <link rel="stylesheet" type="text/css" href="~/styles/SimulatePlan.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
    <script language="javascript">
        // starts the interval timer that refreshes the progress display
        function startTimer(){
            // alert( 'start timer' );  // check to make sure we aren''t starting a new timer every tick
            setTimeout( 'refreshProgressData()', 200 );
        }
        
        // does the postback that refreshes the progress display
        function refreshProgressData(){        
            __doPostBack( 'UpdateProgressPanel', "" );
        }
    </script>
</head>
<body id="Body" runat="server">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" runat="server">
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
       <asp:Label runat="server" ID="PhoneLabel" Text="Where Advertising Starts"></asp:Label>  
       <asp:Image runat="server" ID="BreadcrumbLabel"  ImageUrl="images/Breadcrumbs3.gif"></asp:Image>  
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
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo.gif" />
       <%-- End of standard header --%>
     
        <%-- This DIV gets replaced with the campaign summary --%>
        <div id="CampaignSummaryDiv" runat="server"></div>
      
       <asp:Label ID="SubTitle" runat="server" Text="Simulating Media Plan..."/>
       
       <asp:Label ID="BodyInfo" Text="" runat="server"></asp:Label>   
       
       <asp:Button ID="AbortButton" runat="server" OnClick="AbortButton_Click" Text="Abort" Visible="true" />
       
       <div id="ProgressImage" runat="server"></div> 
       
        <%-- This DIV gets replaced with the suggestions and other "story" elements --%>
        <div id="StoryDiv" runat="server"></div>
        <div id="StoryBodyDiv" runat="server"></div>
        <div id="StoryFooterDiv" runat="server"></div>
       
       <asp:Button ID="DebugProceedButton" name="DebugProceedButton" runat="server" Text="Start Simulation Now" Visible="false" OnClick="GoToNext_Click" />
       <asp:Button ID="DebugSkipSimButton" name="DebugSkipSimButton" runat="server" Text="Skip Simulation - Generate Fake Results" Visible="false" OnClick="GoToResults_Click" />
        
       <asp:Label ID="DebugExtraText" Text="" runat="server"></asp:Label>  

       <div id="Footer" runat="server"><%= WebLibrary.Utils.Footer %></div>

</div>

     </form>
</body>
</html>
