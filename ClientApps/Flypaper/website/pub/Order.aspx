<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Order" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Campaign Headquarters</title>
    <link rel="stylesheet" type="text/css" href="~/css/HQ.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body runat="server" id="PageBody">
    <form id="form1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <input type="hidden" ID="newPlanName" name="newPlanName" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="TabHelp" runat="server" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="TabCampaigns" runat="server" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="TabHome" runat="server" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       

       
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="XContentTable" cellpadding="0" cellspacing="0" width="100%" >
           <tr>
             <td>
                   <div ID="SubTabDiv" runat="server" visible="true">
 <div style="height:21px;">&nbsp;</div>
                   <ul class="nav">
                        <li><a href="HQ.aspx" id="TabShop" runat="server" class="nav_shop" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="#" id="TabOrder" runat="server" class="nav_order active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="Analysis.aspx" id="TabAnalysis" runat="server" class="nav_analyze" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                    </ul>
                    </div>
        
                <table id="InnerTable" style="width: 95%"><tr>
                    <td ID="XPlanSummaryCell" runat="server">&nbsp; </td>
                </tr></table>
                    <br /><br /><br />
                    <br /><br /><br />
                    <br /><br /><br />
               </td>
           
              <td id="HqRHCol" runat="server" rowspan="5">
                   <div id="RHDiv">
                      <asp:ImageButton runat="server"  ImageUrl="images/Button-PrintList.gif" ID="PrintListButton" 
                         OnClientClick="return ShowPrintView();" /><br />
                <%--     <asp:ImageButton runat="server"  ImageUrl="images/Button-CreateRFP.gif" ID="CreateRFPButton" 
                         OnClientClick="return DoCreatRFP();" />  --%> <br />
                        <br />
                   <div runat="server" ID="OrderRHInfoDiv"></div>
                   </div>
                </td>     
           </tr>  

       </table>
       </div>


    <div id="NameDiv">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>Campaign:</td>
                <td><asp:Label ID="CampaignName" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td align="right"><a id="ChangePlanLink" runat="server" href="#" >Plan:</a></td>
                <td><asp:Label ID="PlanName" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>

    <div id="PieChartDiv" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>Pie Chart</td>
            </tr>
        </table>
    </div>
    
    <div id="StarsDiv" runat="server"><img src="images/star.gif" /></div>

    </div>
            <%--  Begin Popup Dialog --%>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" style="display:none"/>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="programmaticModalPopup"
        BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup"
        PopupControlID="programmaticPopup" 
        BackgroundCssClass="modalBackground"
        DropShadow="True"
        PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowScroll" >
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopup" style="display:none;width:350px;padding:6px;border: solid 6px #8fC2F4;">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move;background-color:#DDDDDD;border:solid 1px Gray;color:Black;text-align:center;">
            <span id="PopupTitleText" >Enter Media Plan Name</span>
        </asp:Panel>
            <p id="PopupCaptionTextP" class="PopupCaptionTextP" runat="server" >Enter new name for the plan:</p> <input type="text" id="NewNameBox"  class="NewNameBoxShown" runat="server" /> 
            <table cellpadding="0" cellspacing="0" id="PopupButtonsTbl" class="PopupButtonsTable" ><tr><td>
            <a id="hideModalPopupViaClientButton" runat="server" href="#" ><img src="images/Button-OK.gif" border="0" /></a>
            <a id="CancelButton" href="#"  onclick="return OnGetNameCancel();"><img src="images/Button-Cancel.gif" border="0" /></a>
            </td></tr></table>    
    </asp:Panel>
        
    <script type="text/javascript">
        // The following snippet works around a problem where FloatingBehavior
        // doesn't allow drops outside the "content area" of the page - where "content
        // area" is a little unusual for our sample web pages due to their use of CSS
        // for layout.
        function setBodyHeightToContentHeight() {
            document.body.style.height = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight) + "px";
        }
        setBodyHeightToContentHeight();
        $addHandler(window, "resize", setBodyHeightToContentHeight);    
    </script>
    <%-- End Popup Dialog --%>
    
    </form>
    <script type="text/javascript" src ="google.js"></script>
</body>
</html>
