<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="About" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit - Where Advertising Starts</title>
    <link rel="stylesheet" type="text/css" href="~/css/Home.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="HomeForm1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    

    <div id="MainDIv" >    
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server"  >
      </asp:Panel>       

<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="HoomeInfoDiv" runat="server" >

              </td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="HomeRHDiv">
               <div style="position:relative; text-align:center; top: -21px; margin-bottom: -18px; margin-right: 10px;" > Welcome to AdPlanit, <br />
providing free online media<br />
 planning for advertisers.</div>


        <asp:Panel ID="LoginPanel" runat="server" CssClass="LoginPanel" >

<asp:LoginView runat="server" ID="LoginView">
<AnonymousTemplate>

           <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Campaigns.aspx"  CssClass="UserLogin"  >
               <LayoutTemplate> 
                   <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                       <tr>
                           <td>
                               <table border="0" cellpadding="0">
                                   <tr>
                                       <td align="left" colspan="2" style="font-size:8pt;padding:3px; font-weight:600;">
                                           Login</td>
                                   </tr>
                                   <tr>
                                       <td style="padding-left:7px;">
                                           <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label><asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                               ErrorMessage="Email is required." ToolTip="Email is required." ValidationGroup="Login1"> value Required!</asp:RequiredFieldValidator><br />
                                           <asp:TextBox ID="UserName" runat="server" CssClass="UserLoginControl"></asp:TextBox><br />
                                           
                                       </td>
                                   </tr>
                                   <tr>
                                       <td style="padding-left:7px">
                                           <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label><asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                               ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1"> value Required!</asp:RequiredFieldValidator><br />
                                           <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="UserLoginPwControl"></asp:TextBox><br />
                                           
                                       </td>
                                   </tr>
                                   <%-- 
                                   <tr>
                                       <td colspan="2" align="left" >
                                           <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." TextAlign="right" />
                                       </td>
                                 </tr>--%>
                                   <tr>
                                       <td align="left" colspan="2" style="color: red">
                                           <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="right" colspan="2">
                                       <div runat="server" id="LoginButtonDiv" >
                                           <asp:ImageButton ID="LoginButton" runat="server" CommandName="Login"  ImageUrl="images/Button-Login.gif" ValidationGroup="Login1" CssClass="LoginButton"/>
                                           </div>
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                   </table>
               </LayoutTemplate>
           </asp:Login>
		   <div id="LostPwDiv" >
		        <a href="LostPassword.aspx" id="LostPwLink" onmouseover="PrepInfoPopup();" onmouseout="DismissInfoPopup();" >Forgot password?</a>
		   </div>
		    <div id="NewUserDiv" >
		        <a runat="server" href="signup/Index.aspx" id="NewUserLink">Create new free account.</a>
		   </div>
</AnonymousTemplate>
<LoggedInTemplate>
                              <table border="0" cellpadding="0">
                                   <tr>
                                       <td align="left" style="font-size:8pt;padding:3px; font-weight:600;">
                                           Logged in as:</td>
                                   </tr>
                                   <tr>
                                       <td align="left" style="font-size:8pt;padding:0px;text-align:center; width: 160px;">
                                           <%= WebLibrary.Utils.GetUser( this ).Replace( "@", "@<br>" ) %>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="left" style="font-size:8pt;padding:3px;padding-left:55px;">
                                           <a href="#" onclick="__doPostBack( 'Logout', 0 );" >Log Out</a>
                                           </td>
                                   </tr>
                                </table>
</LoggedInTemplate>
</asp:LoginView>
           
        </asp:Panel>
		   


<div id="HomeRHBottomDiv"> 
<b>Who’s Using AdPlanit?</b><br /><br />
<img src="images/BlackDot.gif" class="WhiteDot" />
Companies planning local<br />&nbsp;&nbsp;&nbsp;&nbsp;advertising campaigns<br /><br />
<img src="images/BlackDot.gif" class="WhiteDot"  />
Media and Advertising<br />&nbsp;&nbsp;&nbsp;&nbsp;Agencies<br /><br />
<img src="images/BlackDot.gif" class="WhiteDot"  />
Small and Medium<br />&nbsp;&nbsp;&nbsp;&nbsp;Businesses<br /><br />
  
</div>
<div id="Col2VideoDiv1"><br /><a href="#" onclick="OpenDemoWindow();" target="_blank">How to Plan<br />Using AdPlanit</a><br /><span class="vidLabel">video</span></div>
   <div id="Col2VideoDiv2"><br /><a href="#" onclick="OpenDemo2Window();">Building an Ad<br />Campaign</a><br /><span class="vidLabel">demo</span></div>
   <div style="position:relative;  text-align:center; top:80px; left: -3px;" >
    <asp:Button ID="Demo" runat="server" Text="Take the Tour >>>" BackColor="#0066FF" 
                        Height="33px" Width="146px" Font-Bold="True" ForeColor="White" 
                        onclick="Demo_Click" />
                        </div>
             
               </div>
               </td>
               
               <%-- End of Right-hand Column --%>
           </tr>
           <tr>
               <td ID="PageDataCell" runat="server">
                <table id="HomePageInfoTable" cellpadding="0" cellspacing="0">
                <tr>
                <td id="Column1">
                <h1>Act With Confidence</h1><br />
                <h2><I>Plan</I> Your Advertising Campaign</h2><br />
<img src="images/BlackDot.gif" class="BlackDot" />
Create an advertising campaign targeting your specific customer.<br />
<img src="images/BlackDot.gif" class="BlackDot" />
Works with online and offline media types.<br />
<br />

<h2><I>Try</I> Your Advertising Before You Buy It</h2><br />
<img src="images/BlackDot.gif" class="BlackDot" />
Learn which advertising media is best by letting virtual consumers respond.<br /><br />
<h2><I>Act</I> with Confidence</h2><br />
<img src="images/BlackDot.gif" class="BlackDot" />
See exactly how best to spend your budget to reach your advertising goals.<br />
<img src="images/BlackDot.gif" class="BlackDot" />
Implement your plan using the shopping list and proposal features.<br /><br />

                <img id="MainImage" src="images/HomePageImage.gif" alt="Main Image" />
                </td>
                <td id="Column2">
                <div id="Col2TopTopDiv">
                 
                </div>
                <div id="Col2TopDiv">
              <p class="quote">"AdPlanit is the perfect solution for ad agencies who need to service smaller accounts”</p>

<p class="quote">“As an enterprise with local outlets, AdPlanit serves the unique needs of local advertising.“</p>

<p class="quote">“As a small business, advertising is very confusing.  AdPlanit lets me effectively plan and increase the return on my advertising investment.”</p>

                </div>
                <div id="Col2BottomDiv">
                <p class="subhd"> Why AdPlanit?</p>
                <img src="images/BlackDot.gif" class="CheckMark" />
Only AdPlanit uses a virtual population of consumers to test your advertising plan before you buy expensive media.  <br /><br />
                <img src="images/BlackDot.gif" class="CheckMark" />

Only AdPlanit uses advanced modeling technology to simulate the success of your advertising.    <br /><br />
                <img src="images/BlackDot.gif" class="CheckMark" />
View “How to Plan Using AdPlanit” to learn more.
                </div>
                </td>
                </tr>
                </table>
                </td>
           </tr>
           

           
       </table>
       <table width="100%" cellpadding="0" cellspacing="0" id="FooterTable"><tr align="center"><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table>


       </div>      

    </div>


    </form>
    <script type="text/javascript" src ="google.js"></script>
</body>
</html>
