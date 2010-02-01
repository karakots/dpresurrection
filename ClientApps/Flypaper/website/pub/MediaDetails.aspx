<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MediaDetails.aspx.cs" Inherits="MediaDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Media Details</title>
    <link rel="stylesheet" type="text/css" href="~/css/MediaDetails.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" ID="MediaSortColumnName" name="MediaSortColumnName" runat="server" value="score" />
    <input type="hidden" ID="MediaType" name="MediaType" runat="server" />
    <input type="hidden" ID="newPlanName" name="newPlanName" runat="server" />
    <input type="hidden" ID="pageNumber" name="pageNumber" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />  
    
     <div style="position: absolute; top: 5px; left: 160px; z-index:10">
                        <asp:Panel ID="VisitorInfo" runat="server" BackColor="#0066FF" BorderStyle="Solid"
                            BorderWidth="2px" 
                            EnableViewState="False" Width="390px" Height="75px">
                            <div style="text-align:center; font-weight:bold;  font-size:medium">
                               Be our Guest! 
                               </div>
                               <div style="text-align:center; font-weight:bold;  font-size:medium">
                               Add or remove media
                               </div>
                                <div style="text-align:center; font-weight:bold;  font-size:medium">
                               Change the timing or ad type
                               </div>
                               <div style="text-align:center; font-style:italic;  font-size:small">
                                    (Saving will be enabled when you sign up)
                            </div>
                        </asp:Panel>
                    </div>  
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>
<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="SupportTab" class="nav_help" runat="server" ></a></li>
            <li><a href="Campaigns.aspx" id="CampaignsTab" class="nav_mycampaigns active" runat="server" ></a></li>
            <li><a href="Home.aspx" id="HomeTab" class="nav_home" runat="server" ></a></li>
            </ul>
            <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server">
       <table cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="TitleDiv" runat="server" style="color:Black;padding-bottom:9px;" >Plan&nbsp;Details:</td>
               <td ID="TypeDiv" runat="server"  style="padding-bottom:9px;padding-left:10px;"><asp:DropDownList runat="server" ID="TypeSelection" AutoPostBack="true" >
               <asp:ListItem>(types list here)
               </asp:ListItem>
               </asp:DropDownList></td>
               <td ID="SaveDiv" runat="server"   style="padding-bottom:9px;padding-left:35px;"><asp:ImageButton runat="server" ImageUrl="images/SaveButton2Disabled.gif" ID="SaveButton" OnClick="SaveButton_Click" /></td>
               <td ID="SaveNeededDiv" runat="server"  style="color:Black;font-size:8pt;padding-bottom:9px;padding-left:5px;">(not needed)</td>
               <td ID="ReturnDiv" runat="server" style="width:30%;padding-bottom:9px;padding-left:10px;"><asp:ImageButton runat="server" ImageUrl="images/ReturnToHQ.gif" ID="ReturnButton" OnClick="ReturnButton_Click" /></td>
               <td ID="BudgetDiv" runat="server"  style="width:50%;text-align:right;color:Black;padding-bottom:9px;"><a id="ChangePlanLink" runat="server" href="#" >Plan:</a>&nbsp;<span ID="PlanNameLabel" runat="server"></span>&nbsp;&nbsp;&nbsp;Total:&nbsp;<%= this.mediaTypeBudget %></td>
           </tr>
           <tr>
               <td colspan="6" ID="YourMedia" runat="server">Your Media</td>
           </tr>
           <tr>
               <td colspan="6" ID="SpacerDiv" runat="server"><br /><br /></td>
           </tr>
           <tr>
               <td colspan="6" ID="Td2" runat="server" style="padding-bottom:3px;" ><span id="MoreLink" runat="server" style="margin-right: 10px;">Page 1 of 1</span></td>
           </tr>
           <tr>
               <td colspan="6" ID="AvailMediaHeader" runat="server">
               <table cellpadding="0" cellspacing="0"  style="border-top:solid 1px #000;border-right:solid 1px #000;border-left:solid 1px #000;background-color:#CFC;font-size:9pt;width:99%">
               <tr >
               <td ID="Td1" runat="server" style="color:Black;padding-bottom:5px;padding-top:5px;padding-left:5px;" ><span 
               style="position:relative;top:-3px;font-size:11pt;" >Available&nbsp;Media</span>&nbsp;&nbsp;<a 
               href=# onclick='ShowMainWaitCursor(); __doPostBack( "AddItem", "" ); return false;' ><img 
               src="images/Add.gif" height=20 width-120 border=0 style="position:relative;top:3px;"></a>
               &nbsp;&nbsp&nbsp;<asp:DropDownList runat="server" ID="ProminenceSelection"  AutoPostBack="true" style="width:30%;max-width:30%;position:relative;top:-2px;" >
               <asp:ListItem Value="-">(prominence selection)</asp:ListItem>
               </asp:DropDownList>
               
               &nbsp&nbsp;&nbsp<span style="position:relative;top:-3px;" >Region:</span>&nbsp;&nbsp;<asp:DropDownList AutoPostBack="true" 
               runat="server" ID="RegionSelection" style="width:25%;max-width:25%;position:relative;top:-2px;">
               <asp:ListItem>(region selection)</asp:ListItem>
               </asp:DropDownList>
               </td>
               </tr>
               </table>
               </td>
           </tr>
           <tr>
               <td colspan="6" ID="AvailMedia" runat="server">Avail Media</td>
           </tr>
           <tr>
               <td colspan="6" ID="Td3" runat="server" style="padding-bottom:3px;" ><span id="MoreLink2" runat="server" style="margin-right: 10px;">Page 1 of 1</span></td>
           </tr>
           <tr>
               <td colspan="6" ID="Footer" runat="server"><br /><%= WebLibrary.Utils.Footer %></td>
           </tr>
       </table>
       </div>      

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
            <a id="hideModalPopupViaClientButton" href="#" runat="server" ><img src="images/Button-OK.gif" border="0" /></a>
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
    <asp:Panel runat="server" CssClass="modalPopup2" ID="programmaticPopup2" style="display:none;width:480px;padding:6px;border: solid 6px #8fC2F4;">
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
