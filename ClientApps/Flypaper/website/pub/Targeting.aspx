<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Targeting.aspx.cs" Inherits="Targeting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlTolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Media Targeting</title>
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

       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="SupportTab" class="nav_help" runat="server" ></a></li>
            <li><a href="Campaigns.aspx" id="CampaignsTab" class="nav_mycampaigns active"  runat="server" ></a></li>
            <li><a href="Home.aspx" id="HomeTab" class="nav_home"  runat="server" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server">
       <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
               <td ID="TitleDiv" runat="server" style="width:150px;color:Black;padding-bottom:9px;font-size:10pt;" ><b>Media Targeting</b></td>
               <td ID="NameDiv" runat="server" style="width:50%;color:Black;padding-bottom:9px;font-size:11pt;padding-left:15px;" >?</td>
               <td colspan="4" style="width:50%;color:Black;padding-bottom:9px;font-size:10pt;padding-left:15px;" ><a id="ChangePlanLink" runat="server" href="#" >Plan:</a>&nbsp;<span ID="PlanNameLabel" runat="server"></span></td>
           </tr>
           <tr>
               <td ID="InfoListDiv" runat="server" colspan="6" >Targeting Selection Area</td>
           </tr>
           <tr>
               <td colspan="3">
               <br />Overall Targeting Level:<p style="margin-left:50px;margin-right:50px; font-size: 8pt; margin-top: 2px;" >Set the slider control below to describe the targeting level of this media. 
               Targeting level corresponds to price – the more targeted your impressions are, the more they will cost.</p>
               <div id="TargetingSliderDiv" style="position:relative; left: 150px;">
               
               <asp:TextBox runat="server" ID="TargetingLevelTextbox" >0</asp:TextBox>
               <ajaxControlTolkit:SliderExtender runat="server" ID="TargetingLevelSlider" TargetControlID="TargetingLevelTextbox" Maximum="30" Minimum="0" Length="400">
               </ajaxControlTolkit:SliderExtender>
               <p style="position:relative; left: 0px; display:inline;">Untargeted</p>
               <p style="position:relative; left: 70px; display:inline;">Moderately Targeted</p>
               <p style="position:relative; left: 150px; display:inline;">Highly Targeted</p>
               </div>
               </td>
           </tr>
           <tr>
               <td ID="Td4" runat="server" colspan="6" style="font-size:10pt;" ><br />Resulting Ad Cost Multiplier: X<span id="adPriceMultiplier" runat="server" >1.0</span></td>
           </tr>
           <tr>
               <td ID="MessageDiv" runat="server" colspan="6" style="color:Red;font-size:11pt;" >&nbsp;</td>
           </tr>
           <tr>
               <td ID="Td2" runat="server" style="width:35%;color:Black;padding-bottom:9px;font-size:11pt;padding-left:15px;" >&nbsp;</td>
               <td ID="Td3" runat="server"   style="width:30%;padding-bottom:9px;padding-left:35px;padding-top:35px;"><asp:ImageButton runat="server" ImageUrl="images/OKButton.gif" ID="OKButton" OnClick="OKButton_Click" />&nbsp;&nbsp;<asp:ImageButton runat="server" ImageUrl="images/CancelButton.gif" ID="ImageButton2" OnClick="CancelButton_Click" /></td>
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
            <a id="hideModalPopupViaClientButton" href="#"  runat="server" ><img src="images/Button-OK.gif" border="0" /></a>
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
