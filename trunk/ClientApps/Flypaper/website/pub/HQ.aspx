<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HQ.aspx.cs" Inherits="HQ" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Campign Headquarters</title>
    <link rel="stylesheet" type="text/css" href="~/css/HQ.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
    <style type="text/css">
        #RHDiv
        {
            margin-left: 0px;
        }
    </style>
</head>
<body>
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
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
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
                        <li><a href="#" id="A3" class="nav_shop active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="Order.aspx" id="A4" class="nav_order" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="Analysis.aspx" id="A5" class="nav_analyze" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                   </ul>
                   </div>
                 <table id="InnerTable">
                     <tr>
                         <td id="XPlanSummaryCell" runat="server">
                             &nbsp;
                         </td>
                     </tr>
                     <tr> <td>
                      <div style="position:relative;top:-200px; left: 150px; z-index:10">
                      <asp:Panel ID="VisitorInfo" 
                          runat="server" BackColor="#0066FF" Height="100px" 
                       Width="465px" BorderStyle="Solid" BorderWidth="2px" 
                       style="margin-left: 0px; padding:10px" EnableViewState="False">
                       <div style="font-weight:bold;">This is a summary of your media plan</div>
                       <div style="font-weight:bold;">
                       <ul>
                             <li>Click on the Order tab to view a detailed order summary.</li>
                             <li>Click on the Analyze tab to see simulation results.</li>
                             <li>Click on the Media buttons to edit the media details.</li>
                        </ul>
                   </div></asp:Panel></div>
                   </td>
                     </tr>
                    
                 </table>
                  <div>
                      <asp:ImageButton ID="AddButton"  runat="server"  />
                         <asp:DropDownList ID="AddTypeList" runat="server" >
                         </asp:DropDownList>
                        
                  </div>
                 
                 <br />
                 <br />
                 <br />
             </td>
               <td id="HqRHCol" runat="server" rowspan="5">
                   <div id="RHDiv">
                   <div style="text-align:left; padding:5px;">
                      <asp:Button runat="server" ID="CreatePlanButton" Text="Create Plan For Me"
                        onclick="AutoGenPlan_Click" BackColor="#FF9900" Font-Bold="True" 
                           Height="26px" Width="160px"/>
                           </div>
                     
                   <div runat="server" ID="RHInfoDiv"></div>
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
    
    <div id="StarsDiv" runat="server" ><img src="images/star.gif" /></div>

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
     <%-- End Popup Dialog --%>
       
     <%--  Begin Popup Media-Info Dialog --%>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" style="display:none"/>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="programmaticModalPopup2"
        BehaviorID="programmaticModalPopupBehavior2"
        TargetControlID="hiddenTargetControlForModalPopup2"
        PopupControlID="programmaticPopup2" 
        BackgroundCssClass="modalBackground"
        DropShadow="True"
        PopupDragHandleControlID="programmaticPopupDragHandle2"
        RepositionMode="RepositionOnWindowScroll" >
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" CssClass="modalPopup2" ID="programmaticPopup2" style="display:none;width:450px;padding:6px;border: solid 6px #8fC2F4;">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle2" Style="cursor: move;background-color:#DDDDDD;border:solid 1px Gray;color:Black;text-align:center;">
            <span id="InfoPopTitle" >Media Details</span>
        </asp:Panel>
            <table cellpadding="5" cellspacing="0" id="PopupInfoTbl" class="PopupInfoTbl" >
            <tr><td class="InfoPopLbl" >Name:</td></td><td><b><span id="InfoPopName"></span></b></td></tr>
            <tr><td class="InfoPopLbl" >Region:</td><td><span id="InfoPopGeo"></span></td></tr>
            <tr><td class="InfoPopLbl" >Type:</td><td><span id="InfoPopType"></span></td></tr>
            <tr><td class="InfoPopLbl" >Website:</td><td><span id="InfoPopURL"></span></td></tr>
            <tr><td colspan="2">
            <a id="CancelButton2" href="#"  onclick="return HideInfoPopup();"><img src="images/Button-OK.gif" border="0" /></a>
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
    <%-- End Popup media-info Dialog --%>

    <%-- Popup Dialog Script --%>
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
    <%-- End Popup Dialog Script --%>
    
    </form>
    <script type="text/javascript" src ="google.js"></script>
</body>
</html>
