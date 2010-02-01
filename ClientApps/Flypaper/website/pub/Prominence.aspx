<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prominence.aspx.cs" Inherits="Prominence" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Media Prominence</title>
    <link rel="stylesheet" type="text/css" href="~/css/Timing.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body id="PageBody"  runat="server">
    <form id="form1" runat="server">
    <input type="hidden" ID="SortColumnName" name="SortColumnName" runat="server" />
    <input type="hidden" ID="TimingItemCount" name="TimingItemCount" runat="server" />
    <input type="hidden" ID="TypeSelection" name="TypeSelection" runat="server" />
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
            <li><a href="Support.aspx" id="SupportTab" class="nav_help" runat="server" ></a></li>
            <li><a href="Campaigns.aspx" id="CampaignsTab" class="nav_mycampaigns active" runat="server"></a></li>
            <li><a href="Home.aspx" id="HomeTab" class="nav_home" runat="server" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server">
       <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
               <td ID="TitleDiv" runat="server" style="width:200px;color:Black;padding-bottom:9px;font-size:10pt;" ><b>Media Prominence</b></td>
               <td ID="NameDiv" runat="server" style="width:50%;color:Black;padding-bottom:9px;font-size:10pt;padding-left:15px;text-align:left;" >?</td>
               <td colspan="4" style="width:50%;color:Black;padding-bottom:9px;font-size:10pt;padding-left:15px;" ><a id="ChangePlanLink" runat="server" href="#" >Plan:</a>&nbsp;<span ID="PlanNameLabel" runat="server"></span></td>
           </tr>
           <tr>
               <td ID="InfoListDiv" runat="server" colspan="6" >Prominence List Area</td>
           </tr>
           <tr>
               <td ID="Td2" runat="server" style="width:35%;color:Black;padding-bottom:9px;font-size:11pt;padding-left:15px;" >&nbsp;</td>
               <td ID="Td3" runat="server"   style="width:30%;padding-top:39px;padding-bottom:9px;padding-left:35px;"><asp:ImageButton runat="server" ImageUrl="images/OKButton.gif" ID="OKButton" OnClick="OKButton_Click" />&nbsp;&nbsp;<asp:ImageButton runat="server" ImageUrl="images/CancelButton.gif" ID="ImageButton2" OnClick="CancelButton_Click" /></td>
                <td ID="Td1" runat="server" style="width:35%;color:Black;padding-bottom:9px;font-size:11pt;padding-left:15px;" >&nbsp;</td>
          </tr>
           <tr>
               <td ID="Footer" runat="server" colspan="6" ><br /><%= WebLibrary.Utils.Footer %></td>
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
</body>
</html>
