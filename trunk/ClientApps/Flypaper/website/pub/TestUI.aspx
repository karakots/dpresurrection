<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestUI.aspx.cs" Inherits="TestUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Campign Headquarters</title>
    <link rel="stylesheet" type="text/css" href="~/css/Common.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
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
            <li><a href="Help.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
      <div ID="NewPlanDiv" runat="server" visible="false">
            You have successfuly created the Campaign named: ?<br /><br />
            Now what do you want to do?<br /><br />
      </div>
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="InfoDiv" runat="server" >
                <div ID="SubNavBar" ></div>

                <h1>Main Header Text - main branch</h1>

                <p style="font-size:14pt; font-family:Verdana;">This is 14-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>                   
            

                 <p style="font-size:14pt;">This is 14-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>         
                
                                <p style="font-size:12pt; font-family:Verdana">This is 12-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>   
          
                <p style="font-size:12pt;">This is 12-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>   
                <p style="font-size:10pt; font-family:Verdana">This is 10-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.<br /><br /></p>
                

                This is 10-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br />
                <p style="font-size:9pt; font-family:Verdana">This is 9-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>    
                <p style="font-size:9pt;">This is 9-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>                
                 <p style="font-size:8pt; font-family:Verdana">This is 8-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                </p>                
                <p style="font-size:8pt;">This is 8-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>
                 <p style="font-size:7pt; font-family:Verdana">This is 7-point Verdana font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                </p>                
                <p style="font-size:7pt;">This is 7-point Arial font regular <b>and bold</b>.  Poih o qwecn n On ovueh pwhcowenc iLHB SClzsb lzHi vbhsdjkhvbs ljhvb zSLdbhv
                lzjHbdv ljzsbhdvljzshb vlzhbsd vlsHbdvl hksbv ljhsbv ljhsbv ljhbv ljhbv lshbv lajbhv  blhb jabhf v.
                <br /><br /></p>

              </td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="RHDiv"></div>
                <a href="Tutorial-Planning.aspx" ><img src="images/Button-Planning.gif" width="170" height="65"/></a>
                <a href="Tutorial-Campaign.aspx" ><img src="images/Button-Campaign.gif" width="170" height="65"/></a>
               </td>
           </tr>
           <tr>
               <td ID="MediaSummary" runat="server">&nbsp; </td>
           </tr>
           <tr>
               <td ID="FooterCell" align="center" ><table id="FooterTable" align="center"><tr><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table></td>
               </td>
           </tr>
           
       </table>


      <div ID="SubTabDiv" runat="server" visible="true">
      <ul class="nav">
            <li><a href="Help.aspx" id="A3" class="nav_advertising active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Faq.aspx" id="A4" class="nav_faq" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Support.aspx" id="A5" class="nav_support" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
        </div>

       </div>      

    </div>
    </form>
</body>
</html>
