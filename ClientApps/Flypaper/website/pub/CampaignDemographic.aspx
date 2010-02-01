<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CampaignDemographic.aspx.cs" Inherits="CampaignDemographics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Set Campaign Demographic</title>
    <link rel="stylesheet" type="text/css" href="~/css/CampaignDetails.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
    <script type="text/javascript" src="DemoPopup.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="InfoDiv" runat="server" >
                <div ID="SubNavBar" ></div>
                <h1>Set Campaign Demographic</h1>

              </td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="RHDiv"></div>
               </td>
           </tr>
           <tr>
               <td ID="PageDataCell" runat="server">
               
                          <%-- - - - - - - -  POPUP DEMOGRAPHICS DIALOG - - - - - - --%>
       
       <asp:Panel ID="PopupBackground" name="PopupBackground" CssClass="PopupBackground" runat="server" ></asp:Panel>
       <asp:Panel ID="DemoPopupShadow" name="DemoPopupShadow" CssClass="DemoPopupShadow" runat="server" ></asp:Panel>
       <asp:Panel ID="DemoPopupPanel" name="DemoPopupPanel" CssClass="DemoPopupPanel" runat="server" >
       <asp:Panel ID="DemoPopupInsetPanel" name="DemoPopupInsetPanel" CssClass="DemoPopupInsetPanel" runat="server" >
       <asp:Panel ID="DemoPopupTitlePanel" name="DemoPopupTitlePanel" CssClass="DemoPopupTitlePanel" runat="server" >
       </asp:Panel>
       <table cellpadding="8"style="margin-bottom:10px">
       <tr>
       <td width="100" class="PopSubTitle">Segment Name:</td>
       <td><input type="text" id="SegmentName" size="50" runat="server" /></td>
       </tr>
       </table>
       <table cellpadding="2" bgcolor="#DDDDDD" width="100%">
       <tr class="HideRow" id="GenderRow">
       <td width="60" class="PopSubTitle"><b>Gender</b></td>
        <td>
           <asp:CheckBox ID="GenderAny" runat="server" Checked="True" Text="Any"  onclick="CheckAnyGender()"/>
           </td>
       <%--<td class="PopAnyTitle"><input type="checkbox" id="GenderAny" onclick="CheckAnyGender(); return true;" checked="true" />Any</td>--%>
       <td><input type="checkbox" id="GenderMale" runat="server" disabled='true'/>Male</td>
       <td><input type="checkbox" id="GenderFemale" runat="server" disabled='true'/>Female</td>
       <td width="295">&nbsp;</td>
       </tr>
       </table>
       <table cellpadding="2" width="100%" style="margin-top:20px;margin-bottom:20px">
       <tr class="HideRow2" id="AgeRow">
       <td width="60" class="PopSubTitle"><b>Age</b></td>
      <td>
           <asp:CheckBox ID="AgeAny" runat="server" Checked="True" Text="Any"  onclick="CheckAnyAge()"/>
           </td>
      
      <%--<td class="PopAnyTitle"><input type="checkbox" id="AgeAny" runat="server"" onclick="CheckAnyAge(); return true;" checked="true" />Any</td>--%>
       <td><input type="checkbox" id="Age1" runat="server" disabled='true'/>Under 18</td>
       <td><input type="checkbox" id="Age2" runat="server" disabled='true'/>18-25</td>
       <td><input type="checkbox" id="Age3" runat="server" disabled='true'/>26-35</td>
       <td><input type="checkbox" id="Age4" runat="server" disabled='true' />36-45</td>
       <td><input type="checkbox" id="Age5" runat="server" disabled='true'/>46-55</td>
       <td><input type="checkbox" id="Age6" runat="server" disabled='true'/>56-65</td>
       <td><input type="checkbox" id="Age7" runat="server" disabled='true'/>Over 65</td>
       <td width="25">&nbsp;</td>
       </tr>
       </table>
       <table cellpadding="2" bgcolor="#DDDDDD" width="100%">
       <tr class="HideRow" id="IncomeRow">
       <td width="60" class="PopSubTitle"><b>Income ($)</b></td>
       <td>
        <asp:CheckBox ID="IncomeAny" runat="server" Checked="True" Text="Any"  onclick="CheckAnyIncome()"/>
           </td>
       <%--<td class="PopAnyTitle"><input type="checkbox" id="IncomeAny" onclick="CheckAnyIncome(); return true;" checked="true" />Any</td>--%>
       <td><input type="checkbox" id="Income1" runat="server" disabled='true/>Under 50K</td>
       <td><input type="checkbox" id="Income2" runat="server" disabled='true/>50K-75K</td>
       <td><input type="checkbox" id="Income3" runat="server" disabled='true/>75K-100K</td>
       <td><input type="checkbox" id="Income4" runat="server" disabled='true/>100K-150K</td>
       <td><input type="checkbox" id="Income5" runat="server" disabled='true/>Over 150K</td>
       <td width="85">&nbsp;</td>
       </tr>
       </table>
       <table cellpadding="2" width="100%" style="margin-top:20px;margin-bottom:20px">
       <tr class="HideRow2" id="KidsRow">
       <td width="60" class="PopSubTitle"><b>Children</b></td>
       <td>
        <asp:CheckBox ID="KidsAny" runat="server" Checked="True" Text="Any"  onclick="CheckAnyKids()"/>
           </td>
       <%--<td class="PopAnyTitle"><input type="checkbox" id="KidsAny" onclick="CheckAnyKids(); return true;" checked="true" />Don't Care</td>--%>
       <td><input type="checkbox" id="Kids0" runat="server" disabled='true/>None</td>
       <td><input type="checkbox" id="Kids1" runat="server" disabled='true/>One or more</td>
       <td width="300">&nbsp;</td>
       </tr>
       </table>
       <table cellpadding="2" bgcolor="#DDDDDD" width="100%">
       <tr class="HideRow" id="RaceRow">
       <td width="60" class="PopSubTitle"><b>Race</b></td>
       <td>
        <asp:CheckBox ID="RaceAny" runat="server" Checked="True" Text="Any"  onclick="CheckAnyRace()"/>
           </td>
       <%--<td class="PopAnyTitle"><input type="checkbox" id="RaceAny" onclick="CheckAnyRace(); return true;" checked="true" />Any</td>--%>
       <td><input type="checkbox" id="Race1" runat="server" disabled='true/>Asian</td>
       <td><input type="checkbox" id="Race2" runat="server" disabled='true/>African American</td>
       <td><input type="checkbox" id="Race3" runat="server" disabled='true/>Hispanic</td>
       <td><input type="checkbox" id="Race4" runat="server" disabled='true/>Other</td>
       <td width="185">&nbsp;</td>
       </tr>
       </table>
       
       <br /><br /><br /><br /><br />

          <br /><br /><br />
       </asp:Panel>
       </asp:Panel>
        <%-- - - - - - - - END:  POPUP DEMOGRAPHICS DIALOG - - - - - - --%>
               
               </td>
           </tr>
           <tr>
               <td align="center" style="padding-bottom:30px">
           <asp:ImageButton ImageUrl="images/OKButton.gif" ID="DefineSegButton" runat="server" OnClick="AddSegment_Click"  />
          <asp:ImageButton ImageUrl="images/CancelButton.gif" ID="CancelButton" runat="server" PostBackUrl="~/Campaign.aspx"  />
          </td>
          </tr>
          <tr>
          <td  ID="FooterCell">
                <table id="FooterTable" align="center"><tr><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table></td>
               </td>
           </tr>
           
       </table>


       </div>      

    </div>
    </form>
</body>
</html>
